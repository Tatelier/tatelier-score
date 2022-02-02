using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tatelier.Score.Play.Chart.TJA
{
	[DebuggerDisplay("Time: {StartMillisec}, HBSPoint: {HBScrollStartPointX}")]
	public class MeasureLine : IMeasureLine
	{
		/// <summary>
		/// 小節線種別
		/// </summary>
		public MeasureLineType MeasureLineType { get; private set; } = MeasureLineType.Normal;

		/// <summary>
		/// 小節線ID
		/// </summary>
		public int Id { get; } = -1;

		/// <summary>
		/// 表示／非表示
		/// </summary>
		public bool Visible { get; set; } = true;

		/// <summary>
		/// 開始時間(ms)
		/// </summary>
		public int StartMillisec { get; private set; }

		/// <summary>
		/// 描画開始時間(ms)
		/// </summary>
		public int StartDrawTimeMillisec = int.MinValue;

		/// <summary>
		/// 1msで動く座標量
		/// </summary>
		public float Mag1msForDraw { get; set; }

		/// <summary>
		/// BPM情報
		/// </summary>
		public BPMInfo BPMInfo { get; private set; }

		/// <summary>
		/// スクロールスピード情報
		/// </summary>
		public ScrollSpeedInfo ScrollSpeedInfo { get; private set; }

		/// <summary>
		/// HBSCROLL用、開始X座標
		/// </summary>
		public double HBScrollStartPointX { get; set; } = 0;

		/// <summary>
		/// 描画開始時間関連を設定する
		/// </summary>
		/// <param name="noteAreaWidth">音符描画エリア全体の幅</param>
		/// <param name="screenWidth">スクリーンの幅</param>
		/// <param name="playOptionScrollSpeed">設定部のスクロールスピード</param>
		public void SetDrawTime(float noteAreaWidth, float screenWidth, float playOptionScrollSpeed)
		{
			var scrspd = (ScrollSpeedInfo.ScrollSpeed * playOptionScrollSpeed);
			var area = noteAreaWidth * scrspd;

			StartDrawTimeMillisec = (int)(StartMillisec - screenWidth * Math.Abs(BPMInfo.OneMeasureMillisec) / area);
			Mag1msForDraw = (float)BPMInfo.GetDivision(area);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="info"></param>
		public MeasureLine(NotePivotInfo info)
		{
			Id = info.MeasureId++;
			if (info.BranchPivot == null)
			{
				MeasureLineType = MeasureLineType.Normal;
			}
			else
			{
				MeasureLineType = info.BranchPivot.PivotMillisec == info.PivotMillisec ? MeasureLineType.BranchStart : MeasureLineType.Normal;
			}

			Visible = info.BarLineState == 1;
			StartMillisec = (int)info.PivotMillisec;
			ScrollSpeedInfo = info.ScrollSpeedInfo;
			BPMInfo = info.BPMInfo;
		}
	}
}
