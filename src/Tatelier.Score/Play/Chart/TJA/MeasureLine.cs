using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tatelier.Score.Component;
using Tatelier.Score.Component.NoteSystem;

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
		public int StartMillisec { get; set; }

		/// <summary>
		/// 描画開始時間(ms)
		/// </summary>
		public int StartDrawTimeMillisec = int.MinValue;

		/// <summary>
		/// 描画終了時間(ms)
		/// </summary>
		public int FinishDrawTimeMillisec = int.MaxValue;

		/// <summary>
		/// 1msで動く座標量
		/// </summary>
		public float MovementPerMillisec { get; set; }

		/// <summary>
		/// BPM情報
		/// </summary>
		public BPM BPMInfo { get; private set; }

		/// <summary>
		/// スクロールスピード情報
		/// </summary>
		public ScrollSpeed ScrollSpeedInfo { get; private set; }

		/// <summary>
		/// HBSCROLL用、開始X座標
		/// </summary>
		public double HBScrollStartPointX { get; set; } = 0;

		public HBScrollDrawDataItem HBScrollDrawDataItem { get; set; }

        #region INoteSystem
        int INoteSystem.FinishMillisec
		{
			get => StartMillisec;
			set => StartMillisec = value;
		}
		#endregion


		/// <summary>
		/// 譜面描画用データを構築する
		/// </summary>
		/// <param name="oneMeasureWidth">4/4拍子の1小節分を描画するために必要な幅</param>
		/// <param name="startDrawPointX">描画開始座標X</param>
		/// <param name="finishDrawPointX">描画終了座標X</param>
		/// <param name="playOptionScrollSpeed">設定部のスクロールスピード</param>
		public void BuildScoreRendererData(float oneMeasureWidth, float startDrawPointX, float finishDrawPointX, float playOptionScrollSpeed)
		{
			var scrspd = (ScrollSpeedInfo.Value * playOptionScrollSpeed);
			var area = (oneMeasureWidth * scrspd);

			if (scrspd < 0)
			{
				StartDrawTimeMillisec = (int)(StartMillisec - finishDrawPointX * Math.Abs(BPMInfo.OneMeasureMillisec) / area);
				FinishDrawTimeMillisec = (int)(StartMillisec - startDrawPointX * Math.Abs(BPMInfo.OneMeasureMillisec) / area);
			}
			else
			{
				StartDrawTimeMillisec = (int)(StartMillisec - startDrawPointX * Math.Abs(BPMInfo.OneMeasureMillisec) / area);
				FinishDrawTimeMillisec = (int)(StartMillisec - finishDrawPointX * Math.Abs(BPMInfo.OneMeasureMillisec) / area);
			}

			MovementPerMillisec = (float)BPMInfo.GetDivision(area);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="info"></param>
		internal MeasureLine(NotePivotInfo info)
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
