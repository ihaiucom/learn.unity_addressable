using System;
using System.Collections.Generic;

namespace UnityEngine.ResourceManagement.ResourceLocations
{
    /// <summary>
    /// 包含足够的信息以加载资产（内容/位置/方式/依赖项）
    /// Contains enough information to load an asset (what/where/how/dependencies)
    /// </summary>
    public interface IResourceLocation
    {
        /// <summary>
        /// 提供程序用于加载此位置的内部名称
        /// Internal name used by the provider to load this location
        /// </summary>
        /// <value>The identifier.</value>
        string InternalId { get; }

        /// <summary>
        /// 匹配用于提供/加载此位置的提供程序
        /// Matches the provider used to provide/load this location
        /// </summary>
        /// <value>The provider id.</value>
        string ProviderId { get; }

        /// <summary>
        /// Gets the dependencies to other IResourceLocations
        /// </summary>
        /// <value>The dependencies.</value>
        IList<IResourceLocation> Dependencies { get; }

        /// <summary>
        /// 此位置与指定类型组合的哈希。
        /// The hash of this location combined with the specified type.
        /// </summary>
        /// <param name="resultType">The type of the result.</param>
        /// <returns>位置和类型的组合哈希。 The combined hash of the location and the type.</returns>
        int Hash(Type resultType);

        /// <summary>
        /// 依赖项的预计算哈希代码。
        /// The precomputed hash code of the dependencies.
        /// </summary>
        int DependencyHashCode { get; }

        /// <summary>
        /// 获取对其他IResourceLocations的依赖项
        /// Gets the dependencies to other IResourceLocations
        /// </summary>
        /// <value>The dependencies.</value>
        bool HasDependencies { get; }

        /// <summary>
        /// 获取与此位置关联的任何数据对象
        /// Gets any data object associated with this locations
        /// </summary>
        /// <value>The object.</value>
        object Data { get; }

        /// <summary>
        /// 此位置的主要地址。
        /// Primary address for this location.
        /// </summary>
        string PrimaryKey { get; }

        /// <summary>
        /// 位置的资源类型。
        /// The type of the resource for th location.
        /// </summary>
        Type ResourceType { get; }
    }
}
