using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.UI;
using TMPro;

// �����²�������Դ
public class CheckUpdateAndDownload : MonoBehaviour
{
    /// <summary>
    /// ��ʾ����״̬�ͽ���
    /// </summary>
    public Text updateText;

    /// <summary>
    /// ���԰�ť
    /// </summary>
    public Button retryBtn;
    public Button enterGameBtn;

    void Start()
    {
        retryBtn.gameObject.SetActive(false);
        enterGameBtn.gameObject.SetActive(false);
        retryBtn.onClick.AddListener(() =>
        {
            StartCoroutine(DoUpdateAddressadble());
        });

        // Ĭ���Զ�ִ��һ�θ��¼��
        StartCoroutine(DoUpdateAddressadble());
    }

    IEnumerator DoUpdateAddressadble()
    {
        AsyncOperationHandle<IResourceLocator> initHandle = Addressables.InitializeAsync();
        yield return initHandle;

        // ������
        var checkHandle = Addressables.CheckForCatalogUpdates(false);
        yield return checkHandle;
        if (checkHandle.Status != AsyncOperationStatus.Succeeded)
        {
            OnError("CheckForCatalogUpdates Error\n" + checkHandle.OperationException.ToString());
            yield break;
        }

        if (checkHandle.Result.Count > 0)
        {
            List<string> catalogResult = checkHandle.Result;
            OnLog($"�����嵥����: {catalogResult.Count}");
            for(int i = 0; i < catalogResult.Count; i ++)
            {
                OnLog($"   {i}: {catalogResult[i]}");
            }

            var updateHandle = Addressables.UpdateCatalogs(checkHandle.Result, false);
            yield return updateHandle;

            if (updateHandle.Status != AsyncOperationStatus.Succeeded)
            {
                OnError("UpdateCatalogs Error\n" + updateHandle.OperationException.ToString());
                yield break;
            }

            // �����б������
            List<IResourceLocator> locators = updateHandle.Result;
            OnLog($"List<IResourceLocator> locators.Count: {locators.Count}");
            int j = 0;
            foreach (var locator in locators)
            {
                IEnumerable<object>  keys = locator.Keys;
                OnLog($"    {j}: locator.Keys: {new List<object>(keys).Count}");
                j++;
                // ��ȡ�����ص��ļ��ܴ�С
                var sizeHandle = Addressables.GetDownloadSizeAsync(locator.Keys);
                yield return sizeHandle;
                if (sizeHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    OnError("GetDownloadSizeAsync Error\n" + sizeHandle.OperationException.ToString());
                    yield break;
                }

                long totalDownloadSize = sizeHandle.Result;
                updateText.text = updateText.text + "\ndownload size : " + totalDownloadSize;
                Debug.Log("download size : " + totalDownloadSize);
                if (totalDownloadSize > 0)
                {
                    // ����
                    var downloadHandle = Addressables.DownloadDependenciesAsync(locator.Keys, Addressables.MergeMode.Union);
                    while (!downloadHandle.IsDone)
                    {
                        if (downloadHandle.Status == AsyncOperationStatus.Failed)
                        {
                            OnError("DownloadDependenciesAsync Error\n" + downloadHandle.OperationException.ToString());
                            yield break;
                        }
                        // ���ؽ���
                        float percentage = downloadHandle.PercentComplete;
                        Debug.Log($"������: {percentage}");
                        updateText.text = updateText.text + $"\n������: {percentage}";
                        yield return null;
                    }
                    if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        Debug.Log("�������!");
                        updateText.text = updateText.text + "\n�������";
                    }
                }
            }
        }
        else
        {
            updateText.text = updateText.text + "\nû�м�⵽����";
        }

        // ������Ϸ
        EnterGame();
    }

    // �쳣��ʾ
    private void OnError(string msg)
    {
        updateText.text = updateText.text + $"\n{msg}\n������! ";
        // ��ʾ���԰�ť
        retryBtn.gameObject.SetActive(true);
    }

    private void OnLog(string msg)
    {
        updateText.text = updateText.text + $"\n{msg} ";
    }

    // ������Ϸ
    void EnterGame()
    {
        OnLog("<<<<EnterGame>>>>");

        enterGameBtn.onClick.AddListener(() =>
        {
            // TODO
            TestLoadLabel testLoadLabel = GetComponent<TestLoadLabel>();
            if (testLoadLabel != null)
            {
                testLoadLabel.enabled = true;
            }

        });

        enterGameBtn.gameObject.SetActive(true);

    }
}
