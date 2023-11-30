using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.ResourceManagement.ResourceLocations
{
    /// <summary>
    /// 用于计算加载位置的大小的接口。
    /// Interface for computing size of loading a location.
    /// </summary>
    public interface ILocationSizeData
    {
        /// <summary>
        /// 计算指定位置需要下载的字节数。
        /// Compute the numder of bytes need to download for the specified location.
        /// </summary>
        /// <param name="location">The location to compute the size for.</param>
        /// <param name="resourceManager">The object that contains all the resource locations.</param>
        /// <returns>The size in bytes of the data needed to be downloaded.</returns>
        long ComputeSize(IResourceLocation location, ResourceManager resourceManager);
    }
}
