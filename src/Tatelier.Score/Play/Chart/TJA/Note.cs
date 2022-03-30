using System;
using System.Diagnostics;
using Tatelier.Score.Component;
using Tatelier.Score.Component.NoteSystem;

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
		public NoteType OriginalNoteType { get; private set; } = NoteType.Don;

		/// <summary>
		/// 音符文字種別
		/// </summary>
		public NoteTextType OriginalNoteTextType { get; private set; } = NoteTextType.Do;

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
		public BPM BPMInfo { get; private set; }

		/// <summary>
		/// スクロールスピード情報
		/// </summary>
		public ScrollSpeed ScrollSpeedInfo { get; private set; }

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
		/// 描画
		/// </summary>
		public int FinishDrawMillisec { get; set; } = int.MaxValue;

		/// <summary>
		/// 1msで動く座標量
		/// </summary>
		public float MovementPerMillisec { get; set; }

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
				StartDrawMillisec = (int)(StartMillisec - finishDrawPointX * Math.Abs(BPMInfo.OneMeasureMillisec) / area);
				FinishDrawMillisec = (int)(FinishMillisec - startDrawPointX * Math.Abs(BPMInfo.OneMeasureMillisec) / area);
			}
            else
			{
				StartDrawMillisec = (int)(StartMillisec - startDrawPointX * Math.Abs(BPMInfo.OneMeasureMillisec) / area);
				FinishDrawMillisec = (int)(FinishMillisec - finishDrawPointX * Math.Abs(BPMInfo.OneMeasureMillisec) / area);
			}

			MovementPerMillisec = (float)BPMInfo.GetDivision(area);
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
		/// HBSCROLL用描画データ
		/// </summary>
		public HBScrollDrawDataItem HBScrollDrawDataItem { get; set; }

		NoteTextType GetNoteTextType(NoteType noteType)
		{
			// TODO: 暫定処理のため、今後修正
			switch (NoteType)
			{
				case NoteType.Don:
				case NoteType.DonBig:
					return NoteTextType.Do;
				case NoteType.Kat:
				case NoteType.KatBig:
					return NoteTextType.Kat;
				case NoteType.Roll:
				case NoteType.RollBig:
					return NoteTextType.Renda;
				case NoteType.Balloon:
				case NoteType.Dull:
					return NoteTextType.GekiRenda;
				case NoteType.End:
				default:
					return NoteTextType.None;
			}
		}

		NoteType GetInverseNote(NoteType noteType)
		{
			switch (noteType)
			{
				case NoteType.Don:
					return NoteType.Kat;					
				case NoteType.Kat:
					return NoteType.Don;					
				case NoteType.DonBig:
					return NoteType.KatBig;					
				case NoteType.KatBig:
					return NoteType.DonBig;					
				default:
					return noteType;
			}
		}

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="noteType">音符種別</param>
        /// <param name="info"></param>
        internal Note(NoteType noteType, NotePivotInfo info)
		{
			Id = info.NoteId++;
			OriginalNoteType = noteType;
			OriginalNoteTextType = GetNoteTextType(noteType);

			NoteType = noteType;
			NoteTextType = GetNoteTextType(NoteType);

			if (info.IsInverse)
            {
				NoteType = GetInverseNote(NoteType);
				NoteTextType = GetNoteTextType(NoteType);
			}

            if (info.IsNoteRandom)
			{
				switch (OriginalNoteType)
				{
					case NoteType.Don:
					case NoteType.Kat:
					case NoteType.DonBig:
					case NoteType.KatBig:
						int r = info.Random.Next(100);
                        if (r < info.RandomRatio)
                        {
							NoteType = GetInverseNote(NoteType);
						}
						break;
				}
				NoteTextType = GetNoteTextType(NoteType);
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
