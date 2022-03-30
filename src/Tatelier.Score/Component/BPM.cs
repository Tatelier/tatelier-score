using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tatelier.Score.Component.NoteSystem;

namespace Tatelier.Score.Component
{
    [DebuggerDisplay("{Value}: [{StartMillisec} to {FinishMillisec}]ms, NoteCount: {NoteList.Count}")]
    public class BPM
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
        public int FinishMillisec { get; private set; } = 3600000;

        /// <summary>
        /// BPM
        /// </summary>
        public double Value { get; set; } = 5;


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
            return value * Value / 240000;
        }

        /// <summary>
        /// 終了時間を設定する
        /// </summary>
        /// <param name="endMillisec">終了時間</param>
        public void SetEndMillisec(double endMillisec)
        {
            FinishMillisec = (int)endMillisec;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startMillisec"></param>
        /// <param name="bpm"></param>
        public void Set(double startMillisec, double bpm)
        {
            StartMillisec = (int)startMillisec;
            Value = bpm;
            OneMeasureMillisec = 240000 / Value;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="startMillisec">開始時間(ms)</param>
        /// <param name="bpm">コピー元情報</param>
        public BPM(double startMillisec, BPM info)
        {
            Set(startMillisec, info.Value);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="startMillisec">開始時間(ms)</param>
        /// <param name="bpm">BPM</param>
        public BPM(double startMillisec, double bpm)
        {
            Set(startMillisec, bpm);
        }
    }
}
