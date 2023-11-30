using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.ResourceManagement.AsyncOperations
{
    /// <summary>
    /// 包含异步操作的下载信息。
    /// Contains download information for async operations.
    /// </summary>
    public struct DownloadStatus
    {
        /// <summary>
        /// 操作及其所有依赖项下载的字节数。
        /// The number of bytes downloaded by the operation and all of its dependencies.
        /// </summary>
        public long TotalBytes;

        /// <summary>
        /// 操作和依赖项需要下载的字节总数。
        /// The total number of bytes needed to download by the operation and dependencies.
        /// </summary>
        public long DownloadedBytes;

        /// <summary>
        /// 操作是否已完成。这用于确定当TotalBytes为0时，计算的百分比是0还是1。
        /// Is the operation completed.  This is used to determine if the computed Percent should be 0 or 1 when TotalBytes is 0.
        /// </summary>
        public bool IsDone;

        /// <summary>
        /// Returns the computed percent complete as a float value between 0 &amp; 1.  If TotalBytes == 0, 1 is returned.
        /// </summary>
        public float Percent => (TotalBytes > 0) ? ((float)DownloadedBytes / (float)TotalBytes) : (IsDone ? 1.0f : 0f);
    }
}
