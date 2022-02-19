using System.Diagnostics;

namespace Tatelier.Score.Play.Chart
{
    [DebuggerDisplay("time:[{StartMillisec} to {EndMillisec}]ms, point:[{StartPoint} to {EndPoint}]px")]
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

		public bool IsDelay = false;

		public bool IsApplicable(int startMillisec, int finishMillisec)
		{
			return (StartMillisec <= startMillisec && finishMillisec < EndMillisec);

			// HBSCROLL挙動調査用処理
			//if (StartMillisec < EndMillisec)
			//{
			//	return (StartMillisec <= startMillisec && finishMillisec < EndMillisec);
			//}
			//else
			//{
			//	return (EndMillisec <= startMillisec && finishMillisec < StartMillisec);
			//}
		}

		public bool IsApplicable(int millisec)
		{
			return IsApplicable(millisec, millisec);
		}
		public bool IsApplicable(INote note)
		{
			return IsApplicable(note.StartMillisec, note.FinishMillisec);
		}

		public bool IsApplicable(IMeasureLine line)
		{
			return IsApplicable(line.StartMillisec, line.FinishMillisec);
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
