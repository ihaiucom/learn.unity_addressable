# learn.unity_addressable
学习Addressables 



[【游戏开发探究】Unity Addressables资源管理方式用起来太爽了，资源打包、加载、热更变得如此轻松（Addressable Asset System | 简称AA）](https://blog.csdn.net/linxinfa/article/details/122390621)



[Addressables package](https://docs.unity3d.com/Packages/com.unity.addressables@1.21/manual/index.html)



[AssetStudio 解压AssetBundle工具](https://github.com/Perfare/AssetStudio)



[Addressables之配置 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/499172933)



[Unity Addressables学习 - 知乎 (zhihu.com)](https://www.zhihu.com/column/c_1499730067991793664)



[Addressables Runtime源码学习之总览 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/512097761)









|                                                       |                                                              |
| ----------------------------------------------------- | ------------------------------------------------------------ |
| UnityEditor.EditorUserBuildSettings.activeBuildTarget | StandaloneWindows64                                          |
| Application.consoleLogPath                            | C:/Users/fengzeng/AppData/Local/Unity/Editor/Editor.log      |
| Application.dataPath                                  | E:/zengfeng/githubs/learn.unity_addressable/AddressablesCode/Assets |
| Application.persistentDataPath                        | C:/Users/fengzeng/AppData/LocalLow/DefaultCompany/AddressablesCode |
| Application.streamingAssetsPath                       | E:/zengfeng/githubs/learn.unity_addressable/AddressablesCode/Assets/StreamingAssets |
| Application.temporaryCachePath                        | C:/Users/fengzeng/AppData/Local/Temp/DefaultCompany/AddressablesCode |
| Addressables.RuntimePath                              | Library/com.unity.addressables/aa/Windows                    |
| Addressables.BuildPath                                | Library/com.unity.addressables/aa/Windows                    |
|                                                       |                                                              |

宏

| ENABLE_ADDRESSABLE_PROFILER | 性能监控       |
| --------------------------- | -------------- |
| ADDRESSABLES_LOG_ALL        | 日志           |
| ENABLE_BINARY_CATALOG       | 启用二进制目录 |
|                             |                |
|                             |                |



#### C#条件属性和宏定义

[c# Conditional用法详解_conditional c#-CSDN博客](https://blog.csdn.net/fdyshlk/article/details/77603287)



#### C#永久的不被垃圾回收

[编程小知识之 GC.KeepAlive-CSDN博客](https://blog.csdn.net/tkokof1/article/details/92073033)



#### 对象池 LRU-least recently used-最近最少使用算法，是一种内存数据淘汰策略

[LRU原理与算法实现 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/161269766)



#### UnityWebRequestAssetBundle.GetAssetBundle 读本地缓存和下载远程

https://docs.unity.cn/cn/2019.4/ScriptReference/Networking.UnityWebRequestAssetBundle.GetAssetBundle.html

![image-20231130105913424](_img/README/image-20231130105913424.png)

![image-20231130110131847](_img/README/image-20231130110131847.png)





#### Bundle缓存目录关系

![image-20231130110152488](_img/README/image-20231130110152488.png)

![image-20231130110243339](_img/README/image-20231130110243339.png)







![image-20231130110329445](_img/README/image-20231130110329445.png)



# 源码笔记

### 异步操作

[AssetDatabase InitalizationObjectsOperation-ProcessOn](https://www.processon.com/diagraming/655f334f33955724ba6bf8f9)

[AssetDatabase.ProvideResource ResourceManagerRuntimeData-ProcessOn](https://www.processon.com/diagraming/655f007d4cdeeb0fc3624a7d)

![image-20231130111001313](_img/README/image-20231130111001313.png)

struct AsyncOperationHandle<TObject> : IEnumerator, IEquatable<AsyncOperationHandle<TObject>>

struct AsyncOperationHandle : IEnumerator



interface IAsyncOperation

class AsyncOperationBase<TObject> : IAsyncOperation





class ChainOperation<TObject, TObjectDependency> : AsyncOperationBase<TObject>

 class ChainOperationTypelessDepedency<TObject> : AsyncOperationBase<TObject>

class GroupOperation : AsyncOperationBase<IList<AsyncOperationHandle>>, ICachable

internal class UnityWebRequestOperation : AsyncOperationBase<UnityWebRequest>



internal class ProviderOperation<TObject> : AsyncOperationBase<TObject>, IGenericProviderOperation, ICachable

### 提供操作

interface IResourceProvider

struct ProvideHandle

 public abstract class ResourceProviderBase : IResourceProvider, IInitializableObject

 public class ContentCatalogProvider : ResourceProviderBase

 public class AssetBundleProvider : ResourceProviderBase

 public class AssetDatabaseProvider : ResourceProviderBase

public class AtlasSpriteProvider : ResourceProviderBase

 internal class BinaryDataProvider : ResourceProviderBase

public class BundledAssetProvider : ResourceProviderBase

public class LegacyResourcesProvider : ResourceProviderBase

 public class VirtualAssetBundleProvider : ResourceProviderBase, IUpdateReceiver

 public class VirtualBundledAssetProvider : ResourceProviderBase

public class TextDataProvider : ResourceProviderBase





[构建 AssetBundle - Unity 手册 (unity3d.com)](https://docs.unity3d.com/cn/current/Manual/AssetBundles-Building.html)



![image-20231130152332789](_img/README/image-20231130152332789.png)



通过 [UnityWebRequestAssetBundle](https://docs.unity3d.com/cn/current/ScriptReference/Networking.UnityWebRequestAssetBundle.html) 加载的 LZMA 压缩格式 Asset Bundle 会自动重新压缩为 LZ4 压缩格式并缓存在本地文件系统上。如果通过其他方式下载并存储捆绑包，则可以使用 [AssetBundle.RecompressAssetBundleAsync](https://docs.unity3d.com/cn/current/ScriptReference/AssetBundle.RecompressAssetBundleAsync.html) API 对其进行重新压缩。



LZ4 使用基于块的算法，允许按段或“块”加载 AssetBundle。解压缩单个块即可使用包含的资源，即使 AssetBundle 的其他块未解压缩也不影响。





[AssetBundle 依赖项 - Unity 手册 (unity3d.com)](https://docs.unity3d.com/cn/current/Manual/AssetBundles-Dependencies.html)

![image-20231130153755761](_img/README/image-20231130153755761.png)

默认情况下，Unity 不会执行任何优化措施来减少或最小化存储重复信息所需的内存。在创建构建版本期间，Unity 会在 AssetBundle 中对隐式引用的资源构建重复版本。 为避免发生此类重复，请将公共资源（例如材质）分配到它们自身的 AssetBundle。



**注意：**使用 LZ4 压缩和未压缩的 AssetBundle 时，[AssetBundle.LoadFromFile](https://docs.unity3d.com/cn/current/ScriptReference/AssetBundle.LoadFromFile.html) 仅在内存中加载其内容目录，而未加载内容本身。要检查是否发生了此情况，请使用[内存性能分析器 (Memory Profiler)](https://docs.unity3d.com/Packages/com.unity.memoryprofiler@0.2/manual/index.html) 包来[检查内存使用情况](https://docs.unity3d.com/Packages/com.unity.memoryprofiler@0.2/manual/workflow-memory-usage.html)。



[本机使用 AssetBundle - Unity 手册 (unity3d.com)](https://docs.unity3d.com/cn/current/Manual/AssetBundles-Native.html)

![image-20231130154107704](_img/README/image-20231130154107704.png)

如果捆绑包采用的是 LZMA 压缩方式，将在加载时解压缩 AssetBundle。LZ4 压缩包则会以压缩状态加载。





![image-20231130171736007](_img/README/image-20231130171736007.png)

[资源、资源和 AssetBundle - Unity Learn](https://learn.unity.com/tutorial/assets-resources-and-assetbundles#5c7f8528edbc2a002053b5a8)

![image-20231130171412416](_img/README/image-20231130171412416.png)



![image-20231130154333135](_img/README/image-20231130154333135.png)

如果捆绑包未压缩或采用了数据块 (LZ4) 压缩方式，LoadFromFile 将直接从磁盘加载捆绑包。使用此方法加载完全压缩的 (LZMA) 捆绑包将首先解压缩捆绑包，然后再将其加载到内存中。

![image-20231130171637922](_img/README/image-20231130171637922.png)

![image-20231130154552144](_img/README/image-20231130154552144.png)

![image-20231130172218663](_img/README/image-20231130172218663.png)

![image-20231130172352652](_img/README/image-20231130172352652.png)

[AssetBundle 压缩 - Unity 手册 (unity3d.com)](https://docs.unity3d.com/cn/current/Manual/AssetBundles-Cache.html)

![image-20231130155818195](_img/README/image-20231130155818195.png)

![image-20231130173903441](_img/README/image-20231130173903441.png)

![image-20231130174047399](_img/README/image-20231130174047399.png)

[故障排除 - Unity 手册 (unity3d.com)](https://docs.unity3d.com/cn/current/Manual/AssetBundles-Troubleshooting.html)





[Unity 使用AssetBundle-Browser打包助手打包AssetBundle（+复用）_assetbundlebrowser-CSDN博客](https://blog.csdn.net/WenHuiJun_/article/details/113178688)



[unity GUID查看项目资源使用情况工具 (ihaiu.com)](https://blog.ihaiu.com/unity-GUIDRef/)

