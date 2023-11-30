using System;
using System.Collections.Generic;

namespace UnityEngine.ResourceManagement.ResourceLocations
{
    /// <summary>
    /// �����㹻����Ϣ�Լ����ʲ�������/λ��/��ʽ/�����
    /// Contains enough information to load an asset (what/where/how/dependencies)
    /// </summary>
    public interface IResourceLocation
    {
        /// <summary>
        /// �ṩ�������ڼ��ش�λ�õ��ڲ�����
        /// Internal name used by the provider to load this location
        /// </summary>
        /// <value>The identifier.</value>
        string InternalId { get; }

        /// <summary>
        /// ƥ�������ṩ/���ش�λ�õ��ṩ����
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
        /// ��λ����ָ��������ϵĹ�ϣ��
        /// The hash of this location combined with the specified type.
        /// </summary>
        /// <param name="resultType">The type of the result.</param>
        /// <returns>λ�ú����͵���Ϲ�ϣ�� The combined hash of the location and the type.</returns>
        int Hash(Type resultType);

        /// <summary>
        /// �������Ԥ�����ϣ���롣
        /// The precomputed hash code of the dependencies.
        /// </summary>
        int DependencyHashCode { get; }

        /// <summary>
        /// ��ȡ������IResourceLocations��������
        /// Gets the dependencies to other IResourceLocations
        /// </summary>
        /// <value>The dependencies.</value>
        bool HasDependencies { get; }

        /// <summary>
        /// ��ȡ���λ�ù������κ����ݶ���
        /// Gets any data object associated with this locations
        /// </summary>
        /// <value>The object.</value>
        object Data { get; }

        /// <summary>
        /// ��λ�õ���Ҫ��ַ��
        /// Primary address for this location.
        /// </summary>
        string PrimaryKey { get; }

        /// <summary>
        /// λ�õ���Դ���͡�
        /// The type of the resource for th location.
        /// </summary>
        Type ResourceType { get; }
    }
}
