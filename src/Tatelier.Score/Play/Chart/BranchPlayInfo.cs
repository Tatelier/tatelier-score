using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatelier.Score.Play.Chart
{
    /// <summary>
    /// 分岐演奏情報
    /// </summary>
    [DebuggerDisplay("[{StartMillisec} to {EndMillisec}]ms : {Type}, M:{MasterValue}, E:{ExpertValue}")]
	public class BranchPlayInfo
	{
		/// <summary>
		/// 1つ前の情報
		/// </summary>
		public BranchPlayInfo Prev;

		/// <summary>
		/// 開始時間
		/// </summary>
		public int StartMillisec = 0;

		/// <summary>
		/// 終了時間
		/// </summary>
		public int EndMillisec = int.MaxValue;

		/// <summary>
		/// 判定種別 [p:精度, r:連打, \t:LEVELHOLD]
		/// </summary>
		public char Type = 'p';

		/// <summary>
		/// 分岐種別
		/// </summary>
		public BranchType BranchType = BranchType.Common;

		/// <summary>
		/// 玄人値
		/// </summary>
		public float ExpertValue = 50;

		/// <summary>
		/// 達人値
		/// </summary>
		public float MasterValue = 100;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public BranchPlayInfo(double startMillisec, BranchPlayInfo prevBranchPlayInfo)
        {
			Prev = prevBranchPlayInfo;
			StartMillisec = (int)startMillisec;
        }
	}
}
