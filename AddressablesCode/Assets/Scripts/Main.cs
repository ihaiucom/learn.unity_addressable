using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Main : MonoBehaviour
{
    [AssetReferenceUILabelRestriction("remote_prefab")]
    public AssetReference prefabRef;

    public AssetReferenceMaterial materialReference;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        Debug.LogError($" UnityEditor.EditorUserBuildSettings.activeBuildTarget= {UnityEditor.EditorUserBuildSettings.activeBuildTarget}");
#endif

        Debug.LogError($"Application.consoleLogPath= {Application.consoleLogPath}" );
        Debug.LogError($"Application.dataPath= {Application.dataPath}");
        Debug.LogError($"Application.persistentDataPath= {Application.persistentDataPath}");
        Debug.LogError($"Application.streamingAssetsPath= {Application.streamingAssetsPath}");
        Debug.LogError($"Application.temporaryCachePath= {Application.temporaryCachePath}");
        Debug.LogError($"Addressables.RuntimePath= {Addressables.RuntimePath}");
        Debug.LogError($"Addressables.BuildPath= {Addressables.BuildPath}");
        Debug.LogError($"Caching.currentCacheForWriting.path= {Caching.currentCacheForWriting.path}");

        

        //InitRef();
        InitRefAsync();
        //InstantiateGO2Async();
    }

    private void InitRef()
    {
        prefabRef.LoadAssetAsync<GameObject>().Completed += (handle) => {
            // 预设
            GameObject prefabObj = handle.Result;
            // 实例化
            GameObject go = Instantiate<GameObject>(prefabObj);
        };
    }

    private async void InitRefAsync()
    {
        GameObject go = await prefabRef.InstantiateAsync().Task;
    }

    private void InstantiateGO()
    {
        AsyncOperationHandle<GameObject> op = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Sphere.prefab");
        op.Completed += (handle) =>
        {
            // 预设
            GameObject prefabObj = handle.Result;
            // 实例化
            GameObject go = Instantiate<GameObject>(prefabObj);
        };
    }


    private void InstantiateGO2()
    {
        AsyncOperationHandle<GameObject> op = Addressables.InstantiateAsync("Assets/Prefabs/Sphere.prefab");
        op.Completed += (handle) =>
        {
            GameObject go = handle.Result;
        };
    }

    private async void InstantiateGOAsync()
    {
        GameObject prefabObj = await Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Sphere.prefab").Task;
        // 实例化
        GameObject go = Instantiate<GameObject>(prefabObj);
    }

    private async void InstantiateGO2Async()
    {
        // 也可直接使用InstantiateAsync方法
        GameObject go = await Addressables.InstantiateAsync("Assets/Prefabs/Cube.prefab").Task;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
