using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.AddressableAssets.ResourceLocators
{
    /// <summary>
    /// Addressables系统用于查找给定key位置的接口。
    /// Interface used by the Addressables system to find the locations of a given key.
    /// </summary>
    public interface IResourceLocator
    {
        /// <summary>
        /// 定位器Id
        /// The id for this locator.
        /// </summary>
        string LocatorId { get; }

        /// <summary>
        /// 由该定位器定义的keys。
        /// The keys defined by this locator.
        /// </summary>
        IEnumerable<object> Keys { get; }

#if ENABLE_BINARY_CATALOG
        /// <summary>
        /// 此定位器可用的所有位置。
        /// All locations that are available by this locator.
        /// </summary>
        IEnumerable<IResourceLocation> AllLocations { get; }
#endif

        /// <summary>
        /// 从指定的关键字检索位置。
        /// Retrieve the locations from a specified key.
        /// </summary>
        /// <param name="key">The key to use.</param>
        /// <param name="type">The resource type.</param>
        /// <param name="locations">The resulting set of locations for the key.</param>
        /// <returns>True if any locations were found with the specified key.</returns>
        bool Locate(object key, Type type, out IList<IResourceLocation> locations);
    }
}
