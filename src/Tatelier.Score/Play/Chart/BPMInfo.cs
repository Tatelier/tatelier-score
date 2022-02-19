using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatelier.Score.Play.Chart
{
	[DebuggerDisplay("{BPM}: [{StartMillisec} to {EndMillisec}]ms, NoteCount: {NoteList.Count}")]
	public class BPMInfo
	{
		readonly List<INote> noteList = new List<INote>();
		readonly List<IMeasureLine> measureLineList = new List<IMeasureLine>();

		/// <summary>
		/// 開始時間(ms)
		/// </summary>
		public int StartMillisec { get; private set; } = 0;

		/// <summary>
		/// 終了時間(ms)
		/// </summary>
		public int EndMillisec { get; private set; } = int.MaxValue;

		/// <summary>
		/// BPM
		/// </summary>
		public double BPM { get; set; } = 5;


		public bool IsDelay { get; set; } = false;

		/// <summary>
		/// 1小節の時間(ms)
		/// </summary>
		public double OneMeasureMillisec { get; private set; }

		/// <summary>
		/// 1msの移動量
		/// </summary>
		public float MovementPerMillisec { get; private set; } = 0.0F;

		/// <summary>
		/// 音符リスト
		/// </summary>
		public IReadOnlyList<INote> NoteList => noteList;

		/// <summary>
		/// 小節線リスト
		/// </summary>
		public IReadOnlyList<IMeasureLine> MeasureLineList => measureLineList;

		/// <summary>
		/// 音符を追加する
		/// </summary>
		/// <param name="note">音符</param>
		public void AddNote(INote note)
		{
			noteList.Add(note);
		}

		/// <summary>
		/// 小節線を追加する
		/// </summary>
		/// <param name="measure">小節線</param>
		public void AddMeasure(IMeasureLine measure)
		{
			measureLineList.Add(measure);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public double GetDivision(double value)
		{
			return value * BPM / 240000;
		}

		/// <summary>
		/// 終了時間を設定する
		/// </summary>
		/// <param name="endMillisec">終了時間</param>
		public void SetEndMillisec(double endMillisec)
		{
			EndMillisec = (int)endMillisec;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="startMillisec"></param>
		/// <param name="bpm"></param>
		public void Set(double startMillisec, double bpm)
		{
			StartMillisec = (int)startMillisec;
			BPM = bpm;
			OneMeasureMillisec = 240000 / BPM;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="startMillisec">開始時間(ms)</param>
		/// <param name="bpm">コピー元情報</param>
		public BPMInfo(double startMillisec, BPMInfo info)
		{
			Set(startMillisec, info.BPM);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="startMillisec">開始時間(ms)</param>
		/// <param name="bpm">BPM</param>
		public BPMInfo(double startMillisec, double bpm)
		{
			Set(startMillisec, bpm);
		}
	}
}
