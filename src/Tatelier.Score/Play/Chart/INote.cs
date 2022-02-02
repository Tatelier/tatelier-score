using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatelier.Score.Play.Chart
{
	public interface INote
	{
		/// <summary>
		/// 音符ID
		/// </summary>
		/// <remarks>
		/// デバッグで利用
		/// </remarks>
		int Id { get; }

		/// <summary>
		/// 音符種別
		/// </summary>
		NoteType NoteType { get; }

		/// <summary>
		/// 音符文字種別
		/// </summary>
		NoteTextType NoteTextType { get; }

		/// <summary>
		/// 1つ前の音符
		/// </summary>
		/// <remarks>
		/// 主に特殊音符で使用
		/// </remarks>
		INote PrevNote { get; }

		/// <summary>
		/// 開始時間(ms)
		/// </summary>
		int StartMillisec { get; }

		/// <summary>
		/// 開始時間(ms)
		/// </summary>
		int FinishMillisec { get; }

		/// <summary>
		/// 描画開始時間(ms)
		/// </summary>
		int StartDrawMillisec { get; }

		/// <summary>
		/// BPM情報
		/// </summary>
		BPMInfo BPMInfo { get; }

		/// <summary>
		/// スクロールスピード情報
		/// </summary>
		ScrollSpeedInfo ScrollSpeedInfo { get; }

		/// <summary>
		/// HBSCROLL用、開始X座標
		/// </summary>
		double HBScrollStartPointX { get; set; }

		/// <summary>
		/// 1msで動く座標量
		/// </summary>
		float Mag1msForDraw { get; set; }

		/// <summary>
		/// 叩かれ状態
		/// true: 叩かれ済, false: 未叩かれ
		/// </summary>
		bool Hit { get; set; }

		/// <summary>
		/// 描画するか否か
		/// true: 描画する, false: しない
		/// </summary>
		bool Visible { get; set; }

		/// <summary>
		/// 描画開始時間関連を設定する
		/// </summary>
		/// <param name="noteAreaWidth">描画エリア全体の幅</param>
		/// <param name="screenWidth">スクリーン領域の幅</param>
		/// <param name="playOptionScrollSpeed">設定部のスクロールスピード</param>
		void SetDrawTime(float noteAreaWidth, float screenWidth, float playOptionScrollSpeed);
	}
}
