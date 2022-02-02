using System.Diagnostics;

namespace Tatelier.Score.Play.Chart
{
    [DebuggerDisplay("time:[{StartTime} to {EndTime}]ms, point:[{StartPoint} to {EndPoint}]px")]
	public class HBScrollDrawDataItem
	{
		/// <summary>
		/// 開始時間(ms)
		/// </summary>
		public int StartMillisec;

		/// <summary>
		/// 開始座標
		/// </summary>
		public double StartPoint;

		/// <summary>
		/// 終了時間
		/// </summary>
		public int EndMillisec;

		/// <summary>
		/// 終了座標
		/// </summary>
		public double EndPoint;

		public HBScrollDrawDataItem()
		{

		}
	}
}
