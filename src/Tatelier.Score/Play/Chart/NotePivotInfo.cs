using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tatelier.Score.Play.Chart
{
	/// <summary>
	/// パース用情報クラス
	/// </summary>
	public class NotePivotInfo
	{
		/// <summary>
		/// 分岐用の情報
		/// </summary>
		public NotePivotInfo BranchPivot = null;

		/// <summary>
		/// 音符ID
		/// </summary>
		public int NoteId = 0;

		/// <summary>
		/// 小節線ID
		/// </summary>
		public int MeasureId = 0;

		/// <summary>
		/// 分岐譜面
		/// </summary>
		public BranchScoreItem CurrentBranchScoreItem;

		/// <summary>
		/// 分岐種別
		/// </summary>
		public BranchType BranchType = BranchType.Common;

		/// <summary>
		/// 小節線状態
		/// 0: 非表示, 1: 表示
		/// </summary>
		public int BarLineState = 1;

		/// <summary>
		/// BPM情報
		/// </summary>
		public BPMInfo BPMInfo = new BPMInfo(-1000, 120);

		/// <summary>
		/// MeasureLine情報
		/// </summary>
		public MeasureInfo MeasureInfo = new MeasureInfo(-60000, 4, 4);

		/// <summary>
		/// ScrollSpeed情報
		/// </summary>
		public ScrollSpeedInfo ScrollSpeedInfo = new ScrollSpeedInfo(-60000, 1.0);
		
		/// <summary>
		/// 音符のミリ時間(only get)
		/// </summary>
		public double PivotMillisec => decimal.ToDouble(PivotMicrosec * 0.001m);

		/// <summary>
		/// 音符のマイクロ秒
		/// </summary>
		public decimal PivotMicrosec = 0;

		/// <summary>
		/// 1つ前の音符
		/// </summary>
		public INote PrevNote = null;

		/// <summary>
		/// 1つ前の小節線
		/// </summary>
		public IMeasureLine PrevMeasureLine = null;

		/// <summary>
		/// 演奏用分岐情報
		/// </summary>
		public BranchPlayInfo BranchPlayInfo = null;

		/// <summary>
		/// 現在の風船打数要素番号
		/// </summary>
		public int NowBalloonIndex = 0;

		/// <summary>
		/// 風船打数リスト
		/// </summary>
		public List<int> BalloonValueList = null;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public NotePivotInfo() { }

		/// <summary>
		/// シャドウコピー
		/// </summary>
		/// <returns></returns>
		public NotePivotInfo ShallowCopy()
		{
			return (NotePivotInfo)MemberwiseClone();
		}
	}
}
