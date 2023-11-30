using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.AddressableAssets.ResourceLocators
{
    /// <summary>
    /// Addressablesϵͳ���ڲ��Ҹ���keyλ�õĽӿڡ�
    /// Interface used by the Addressables system to find the locations of a given key.
    /// </summary>
    public interface IResourceLocator
    {
        /// <summary>
        /// ��λ��Id
        /// The id for this locator.
        /// </summary>
        string LocatorId { get; }

        /// <summary>
        /// �ɸö�λ�������keys��
        /// The keys defined by this locator.
        /// </summary>
        IEnumerable<object> Keys { get; }

#if ENABLE_BINARY_CATALOG
        /// <summary>
        /// �˶�λ�����õ�����λ�á�
        /// All locations that are available by this locator.
        /// </summary>
        IEnumerable<IResourceLocation> AllLocations { get; }
#endif

        /// <summary>
        /// ��ָ���Ĺؼ��ּ���λ�á�
        /// Retrieve the locations from a specified key.
        /// </summary>
        /// <param name="key">The key to use.</param>
        /// <param name="type">The resource type.</param>
        /// <param name="locations">The resulting set of locations for the key.</param>
        /// <returns>True if any locations were found with the specified key.</returns>
        bool Locate(object key, Type type, out IList<IResourceLocation> locations);
    }
}
