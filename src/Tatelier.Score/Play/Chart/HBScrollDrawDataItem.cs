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

		public bool IsApplicable(INote note)
        {
			return (StartMillisec <= note.StartMillisec && note.FinishMillisec < EndMillisec);
		}

		public bool IsApplicable(IMeasureLine line)
		{
			return (StartMillisec <= line.StartMillisec && line.StartMillisec < EndMillisec);
		}

		public double GetHBScrollPivotX(double per)
        {
			return StartPoint + (EndPoint - StartPoint) * per;
		}

		public double GetElapsedRate(int nowMillisec)
        {
			return (double)(nowMillisec - StartMillisec) / (EndMillisec - StartMillisec);
		}

		public HBScrollDrawDataItem()
		{

		}
	}
}
