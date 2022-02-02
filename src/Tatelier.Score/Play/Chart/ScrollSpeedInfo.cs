using System.Diagnostics;

namespace Tatelier.Score.Play.Chart
{
	/// <summary>
	/// スクロールスピード情報クラス
	/// </summary>
	[DebuggerDisplay("{ScrollSpeed}: [{StartMillisec} to {EndMillisec}]ms")]
	public class ScrollSpeedInfo
	{
		/// <summary>
		/// 開始時間
		/// </summary>
		public int StartMillisec { get; private set; } = 0;

		/// <summary>
		/// 終了時間
		/// </summary>
		public int EndMillisec { get; private set; } = int.MaxValue;

		/// <summary>
		/// スクロールスピード
		/// </summary>
		public double ScrollSpeed { get; private set; } = 1.0;

		/// <summary>
		/// 終了時間をセットする
		/// </summary>
		/// <param name="endMillisec">終了時間(ms)</param>
		public void SetEndMillisec(double endMillisec)
		{
			EndMillisec = (int)endMillisec;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="startMillisec">開始時間(ms)</param>
		/// <param name="info">コピー元の情報</param>
		public ScrollSpeedInfo(double startMillisec, ScrollSpeedInfo info)
			: this(startMillisec, info.ScrollSpeed)
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="startMillisec">開始時間(ms)</param>
		/// <param name="scrollSpeed">スクロールスピード</param>
		public ScrollSpeedInfo(double startMillisec, double scrollSpeed)
		{
			this.StartMillisec = (int)startMillisec;
			this.ScrollSpeed = scrollSpeed;
		}
	}
}
