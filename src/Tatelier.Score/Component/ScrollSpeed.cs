using System.Diagnostics;

namespace Tatelier.Score.Component
{
    /// <summary>
    /// スクロールスピード情報クラス
    /// </summary>
    [DebuggerDisplay("{Value}: [{StartMillisec} to {FinishMillisec}]ms")]
    public class ScrollSpeed
    {
        /// <summary>
        /// 開始時間
        /// </summary>
        public int StartMillisec { get; private set; } = 0;

        /// <summary>
        /// 終了時間
        /// </summary>
        public int FinishMillisec { get; private set; } = int.MaxValue;

        /// <summary>
        /// スクロールスピード
        /// </summary>
        public double Value { get; private set; } = 1.0;

        /// <summary>
        /// 終了時間をセットする
        /// </summary>
        /// <param name="endMillisec">終了時間(ms)</param>
        public void SetEndMillisec(double endMillisec)
        {
            FinishMillisec = (int)endMillisec;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="startMillisec">開始時間(ms)</param>
        /// <param name="info">コピー元の情報</param>
        public ScrollSpeed(double startMillisec, ScrollSpeed info)
            : this(startMillisec, info.Value)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="startMillisec">開始時間(ms)</param>
        /// <param name="scrollSpeed">スクロールスピード</param>
        public ScrollSpeed(double startMillisec, double scrollSpeed)
        {
            StartMillisec = (int)startMillisec;
            Value = scrollSpeed;
        }
    }
}
