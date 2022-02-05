using System;
using System.Diagnostics;

namespace Tatelier.Score.Play.Chart.TJA
{
	[DebuggerDisplay("{NoteType}, Time: {StartMillisec}～{FinishMillisec}")]
	public class Note : INote
	{
		INote INote.PrevNote => PrevNote;

		/// <summary>
		/// 音符ID (Unique)
		/// </summary>
		public int Id { get; } = -1;

		/// <summary>
		/// 音符種別
		/// </summary>
		public NoteType NoteType { get; private set; } = NoteType.Don;

		/// <summary>
		/// 音符文字種別
		/// </summary>
		public NoteTextType NoteTextType { get; private set; } = NoteTextType.Do;

		/// <summary>
		/// 1つ前の音符
		/// </summary>
		public Note PrevNote { get; set; } = null;

		/// <summary>
		/// BPM情報
		/// </summary>
		public BPMInfo BPMInfo { get; private set; }

		/// <summary>
		/// スクロールスピード情報
		/// </summary>
		public ScrollSpeedInfo ScrollSpeedInfo { get; private set; }

		/// <summary>
		/// 開始時間
		/// </summary>
		public int StartMillisec { get; set; }

		/// <summary>
		/// 終了時間
		/// </summary>
		public int FinishMillisec { get; set; }

		/// <summary>
		/// 描画開始時間
		/// </summary>
		public int StartDrawMillisec { get; set; } = int.MinValue;

		/// <summary>
		/// 1msで動く座標量
		/// </summary>
		public float Mag1msForDraw { get; set; }

		/// <summary>
		/// HBSCROLL用の音符の開始位置
		/// </summary>
		public double HBScrollStartPointX { get; set; } = 0;

		/// <summary>
		/// HBSCROLL用の音符の終了位置
		/// </summary>
		public double HBScrollFinishPointX { get; set; } = 0;

		/// <summary>
		/// 特別なデータ
		/// </summary>
		public object SpecialData { get; set; } = null;

		/// <summary>
		/// 描画開始時間関連を設定する
		/// </summary>
		/// <param name="noteAreaWidth">描画エリア全体の幅</param>
		/// <param name="screenWidth">スクリーン領域の幅</param>
		/// <param name="playOptionScrollSpeed">設定部のスクロールスピード</param>
		public void SetDrawTime(float noteAreaWidth, float screenWidth, float playOptionScrollSpeed)
		{
			var scrspd = (ScrollSpeedInfo.ScrollSpeed * playOptionScrollSpeed);
			var area = (noteAreaWidth * scrspd);
			StartDrawMillisec = (int)(StartMillisec - screenWidth * Math.Abs(BPMInfo.OneMeasureMillisec) / area);
			Mag1msForDraw = (float)BPMInfo.GetDivision(area);
		}

		/// <summary>
		/// 叩かれ状態
		/// / true : 叩かれ済, false : 未叩かれ
		/// </summary>
		public bool Hit { get; set; } = false;

		/// <summary>
		/// 描画するか否か
		/// / true : 描画する, false : しない
		/// </summary>
		public bool Visible { get; set; } = true;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="noteType">音符種別</param>
		/// <param name="info"></param>
		public Note(NoteType noteType, NotePivotInfo info)
		{
			Id = info.NoteId++;
			NoteType = noteType;

			// TODO: 暫定処理のため、今後修正
			switch (NoteType)
			{
				case NoteType.Don:
				case NoteType.DonBig:
					NoteTextType = NoteTextType.Do;
					break;
				case NoteType.Kat:
				case NoteType.KatBig:
					NoteTextType = NoteTextType.Kat;
					break;
				case NoteType.Roll:
				case NoteType.RollBig:
					NoteTextType = NoteTextType.Renda;
					break;
				case NoteType.Balloon:
				case NoteType.Dull:
					NoteTextType = NoteTextType.GekiRenda;
					break;
				case NoteType.End:
					break;
			}

			StartMillisec = (int)info.PivotMillisec;
			FinishMillisec = StartMillisec;

			BPMInfo = info.BPMInfo;
			ScrollSpeedInfo = info.ScrollSpeedInfo;

			// 終端音符の場合は、元となっている特殊音符を持っておく
			if (noteType == NoteType.End)
			{
				PrevNote = info.PrevNote as Note;

				// 1つ前の音符の終了時間を設定する
				PrevNote.FinishMillisec = StartMillisec;
			}
		}
	}
}
