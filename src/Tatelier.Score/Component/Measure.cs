using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tatelier.Score.Component.NoteSystem;

namespace Tatelier.Score.Component
{
    [DebuggerDisplay("{Upper}/{Lower}: [{StartMillisec} to {FinishMillisec}]ms")]
    public class Measure
    {
        /// <summary>
        /// 開始時間(ms)
        /// </summary>
        public int StartMillisec { get; private set; }

        /// <summary>
        /// 終了時間(ms)
        /// </summary>
        public int FinishMillisec { get; private set; } = int.MaxValue;

        /// <summary>
        /// 拍子の分子
        /// </summary>
        public double Upper { get; private set; }

        /// <summary>
        /// 拍子の分母
        /// </summary>
        public double Lower { get; private set; }

        /// <summary>
        /// 拍子の値
        /// </summary>
        public double Value
        {
            get => Upper * 4 / Lower;
        }

        readonly List<INote> noteList = new List<INote>();

        /// <summary>
        /// 音符リスト
        /// </summary>
        public IReadOnlyList<INote> NoteList => noteList;

        /// <summary>
        /// 終了時間をセットする
        /// </summary>
        /// <param name="endMillisec">終了時間(ms)</param>
        public void SetEndMillisec(double endMillisec)
        {
            FinishMillisec = (int)endMillisec;
        }

        public decimal GetCalc(double val)
        {
            decimal dVal = new decimal(val);
            decimal dUpper = new decimal(Upper);
            decimal dLower = new decimal(Lower);

            return dVal * dUpper * 4 / dLower;
        }

        public void AddNote(INote note)
        {
            noteList.Add(note);
        }

        public Measure(double startMillisec, Measure info)
            : this(startMillisec, info.Upper, info.Lower)
        {
        }

        public Measure(double startMillisec, double upper, double lower)
        {
            StartMillisec = (int)startMillisec;
            Upper = upper;
            Lower = lower;
        }
    }
}
