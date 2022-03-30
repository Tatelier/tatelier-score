using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatelier.Score
{
    /// <summary>
    /// 分岐種別
    /// </summary>
    [Flags]
    public enum BranchType
    {
        /// <summary>
        /// 共通譜面
        /// </summary>
        Common = 0x00,

        /// <summary>
        /// 普通譜面
        /// </summary>
        Normal = 0x01,

        /// <summary>
        /// 玄人譜面
        /// </summary>
        Expert = 0x02,

        /// <summary>
        /// 達人譜面
        /// </summary>
        Master = 0x03,
        /// <summary>
        /// 1つ前
        /// </summary>
        OneBefore = 0xFF,
    }
}
