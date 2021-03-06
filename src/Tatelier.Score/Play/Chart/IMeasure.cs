using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatelier.Score.Play.Chart
{
	public interface IMeasureLine
	{
		/// <summary>
		/// 小節線ID
		/// </summary>
		/// <remarks>
		/// デバッグで利用
		/// </remarks>
		int Id { get; }

		/// <summary>
		/// 小節線種別
		/// </summary>
		MeasureLineType MeasureLineType { get; }

		/// <summary>
		/// 開始時間(ms)
		/// </summary>
		int StartMillisec { get; }

		/// <summary>
		/// HBSCROLL用、開始X座標
		/// </summary>
		double HBScrollStartPointX { get; set; }

		/// <summary>
		/// 1msで動く座標量
		/// </summary>
		float Mag1msForDraw { get; set; }

		/// <summary>
		/// BPM情報
		/// </summary>
		BPMInfo BPMInfo { get; }

		/// <summary>
		/// スクロールスピード情報
		/// </summary>
		ScrollSpeedInfo ScrollSpeedInfo { get; }

		/// <summary>
		/// 表示
		/// </summary>
		bool Visible { get; }

		/// <summary>
		/// 描画開始時間関連を設定する
		/// </summary>
		/// <param name="noteAreaWidth">描画エリア全体の幅</param>
		/// <param name="screenWidth">スクリーン領域の幅</param>
		/// <param name="playOptionScrollSpeed">設定部のスクロールスピード</param>
		void SetDrawTime(float noteAreaWidth, float screenWidth, float playOptionScrollSpeed);

	}
}
