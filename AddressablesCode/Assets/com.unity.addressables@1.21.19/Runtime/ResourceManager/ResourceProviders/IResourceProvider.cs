using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.SceneManagement;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
    /// <summary>
    /// 资源提供程序行为的选项。
    /// Options for resource provider behavior.
    /// </summary>
    public enum ProviderBehaviourFlags
    {
        /// <summary>
        /// 指示提供程序没有额外指定的行为。
        /// Indicates that the provider does not have extra specified behavior.
        /// </summary>
        None = 0,

        /// <summary>
        /// 指示即使存在失败的依赖关系，提供程序仍将满足请求。
        /// Indicates that the provider will still fulfill requests even with failed dependencies.
        /// </summary>
        CanProvideWithFailedDependencies = 1
    }

    /// <summary>
    /// 提供程序满足请求所需的所有数据的容器。
    /// Container for all data need by providers to fulfill requests.
    /// </summary>
    public struct ProvideHandle
    {
        int m_Version;
        IGenericProviderOperation m_InternalOp;
        ResourceManager m_ResourceManager;

        internal ProvideHandle(ResourceManager rm, IGenericProviderOperation op)
        {
            m_ResourceManager = rm;
            m_InternalOp = op;
            m_Version = op.ProvideHandleVersion;
        }

        internal bool IsValid => m_InternalOp != null && m_InternalOp.ProvideHandleVersion == m_Version;

        internal IGenericProviderOperation InternalOp
        {
            get
            {
                if (m_InternalOp.ProvideHandleVersion != m_Version)
                {
                    throw new Exception(ProviderOperation<object>.kInvalidHandleMsg);
                }

                return m_InternalOp;
            }
        }

        /// <summary>
        /// The ResourceManager used to create the operation.
        /// </summary>
        public ResourceManager ResourceManager
        {
            get { return m_ResourceManager; }
        }

        /// <summary>
        /// The requested object type.
        /// </summary>
        public Type Type
        {
            get { return InternalOp.RequestedType; }
        }

        /// <summary>
        /// The location for the request.
        /// </summary>
        public IResourceLocation Location
        {
            get { return InternalOp.Location; }
        }

        /// <summary>
        /// Number of dependencies.
        /// </summary>
        public int DependencyCount
        {
            get { return InternalOp.DependencyCount; }
        }

        /// <summary>
        /// Get a specific dependency object.
        /// </summary>
        /// <typeparam name="TDepObject">The dependency type.</typeparam>
        /// <param name="index">The index of the dependency.</param>
        /// <returns>The dependency object.</returns>
        public TDepObject GetDependency<TDepObject>(int index)
        {
            return InternalOp.GetDependency<TDepObject>(index);
        }

        /// <summary>
        /// Get the depedency objects.
        /// </summary>
        /// <param name="list">The list of dependecies to fill in.</param>
        public void GetDependencies(IList<object> list)
        {
            InternalOp.GetDependencies(list);
        }

        /// <summary>
        /// Set the func for handling progress requests.
        /// </summary>
        /// <param name="callback">The callback function.</param>
        public void SetProgressCallback(Func<float> callback)
        {
            InternalOp.SetProgressCallback(callback);
        }

        /// <summary>
        /// Set the func for handling download progress requests.
        /// </summary>
        /// <param name="callback">The callback function.</param>
        public void SetDownloadProgressCallbacks(Func<DownloadStatus> callback)
        {
            InternalOp.SetDownloadProgressCallback(callback);
        }


        /// <summary>
        /// Set the func for handling a request to wait for the completion of the operation
        /// </summary>
        /// <param name="callback">The callback function.</param>
        public void SetWaitForCompletionCallback(Func<bool> callback)
        {
            InternalOp.SetWaitForCompletionCallback(callback);
        }

        /// <summary>
        /// Called to complete the operation.
        /// </summary>
        /// <typeparam name="T">The type of object requested.</typeparam>
        /// <param name="result">The result object.</param>
        /// <param name="status">True if the operation was successful, false otherwise.</param>
        /// <param name="exception">The exception if the operation failed.</param>
        public void Complete<T>(T result, bool status, Exception exception)
        {
            InternalOp.ProviderCompleted<T>(result, status, exception);
        }
    }


    /// <summary>
    /// 资源提供程序处理对象的加载（提供）和卸载（释放）
    /// Resoure Providers handle loading (Provide) and unloading (Release) of objects
    /// </summary>
    public interface IResourceProvider
    {
        /// <summary>
        /// 此提供程序的唯一标识符，由资源位置用来查找合适的提供程序
        /// Unique identifier for this provider, used by Resource Locations to find a suitable Provider
        /// </summary>
        /// <value>The provider identifier.</value>
        string ProviderId { get; }

        /// <summary>
        /// 此提供程序可以提供的默认对象类型。
        /// The default type of object that this provider can provide.
        /// </summary>
        /// <param name="location">The location that can be used to determine the type.</param>
        /// <returns>The type of object that can be provided.</returns>
        Type GetDefaultType(IResourceLocation location);

        /// <summary>
        /// 确定此提供程序是否可以从指定位置提供指定的对象类型。
        /// Determine if this provider can provide the specified object type from the specified location.
        /// </summary>
        /// <param name="type">The type of object.</param>
        /// <param name="location">The resource location of the object.</param>
        /// <returns>True if this provider can create the specified object.</returns>
        bool CanProvide(Type type, IResourceLocation location);

        /// <summary>
        /// 告诉provide它需要提供资源，并通过传递的provideHandle报告结果。调用此函数时，所有依赖项都已完成，并且可以通过provideHandle使用。
        /// Tells the provide that it needs to provide a resource and report the results through the passed provideHandle. When this is called, all dependencies have completed and are available through the provideHandle.
        /// </summary>
        /// <param name="provideHandle">A handle used to update the operation.</param>
        void Provide(ProvideHandle provideHandle);

        /// <summary>
        /// 释放和/或卸载给定的资源位置和资产
        /// Release and/or unload the given resource location and asset
        /// </summary>
        /// <param name="location">Location to release.</param>
        /// <param name="asset">Asset to unload.</param>
        void Release(IResourceLocation location, object asset);

        /// <summary>
        /// 提供程序的自定义标志。
        /// Custom flags for the provider.
        /// </summary>
        ProviderBehaviourFlags BehaviourFlags { get; }
    }
}
