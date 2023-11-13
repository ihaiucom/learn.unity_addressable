using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.UI;
using TMPro;

// 检测更新并下载资源
public class CheckUpdateAndDownload : MonoBehaviour
{
    /// <summary>
    /// 显示下载状态和进度
    /// </summary>
    public Text updateText;

    /// <summary>
    /// 重试按钮
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

        // 默认自动执行一次更新检测
        StartCoroutine(DoUpdateAddressadble());
    }

    IEnumerator DoUpdateAddressadble()
    {
        AsyncOperationHandle<IResourceLocator> initHandle = Addressables.InitializeAsync();
        yield return initHandle;

        // 检测更新
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
            OnLog($"更新清单数量: {catalogResult.Count}");
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

            // 更新列表迭代器
            List<IResourceLocator> locators = updateHandle.Result;
            OnLog($"List<IResourceLocator> locators.Count: {locators.Count}");
            int j = 0;
            foreach (var locator in locators)
            {
                IEnumerable<object>  keys = locator.Keys;
                OnLog($"    {j}: locator.Keys: {new List<object>(keys).Count}");
                j++;
                // 获取待下载的文件总大小
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
                    // 下载
                    var downloadHandle = Addressables.DownloadDependenciesAsync(locator.Keys, Addressables.MergeMode.Union);
                    while (!downloadHandle.IsDone)
                    {
                        if (downloadHandle.Status == AsyncOperationStatus.Failed)
                        {
                            OnError("DownloadDependenciesAsync Error\n" + downloadHandle.OperationException.ToString());
                            yield break;
                        }
                        // 下载进度
                        float percentage = downloadHandle.PercentComplete;
                        Debug.Log($"已下载: {percentage}");
                        updateText.text = updateText.text + $"\n已下载: {percentage}";
                        yield return null;
                    }
                    if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        Debug.Log("下载完毕!");
                        updateText.text = updateText.text + "\n下载完毕";
                    }
                }
            }
        }
        else
        {
            updateText.text = updateText.text + "\n没有检测到更新";
        }

        // 进入游戏
        EnterGame();
    }

    // 异常提示
    private void OnError(string msg)
    {
        updateText.text = updateText.text + $"\n{msg}\n请重试! ";
        // 显示重试按钮
        retryBtn.gameObject.SetActive(true);
    }

    private void OnLog(string msg)
    {
        updateText.text = updateText.text + $"\n{msg} ";
    }

    // 进入游戏
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
