using Tatelier.Score.Play.Chart;

namespace Tatelier.Score.Component.NoteSystem
{
    /// <summary>
    /// 音符インターフェース
    /// </summary>
    public interface INote : INoteSystem
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
        NoteType OriginalNoteType { get; }

        /// <summary>
        /// 音符文字種別
        /// </summary>
        NoteTextType OriginalNoteTextType { get; }

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
        /// 描画開始時間(ms)
        /// </summary>
        int StartDrawMillisec { get; }

        /// <summary>
        /// 描画終了時間(ms)
        /// </summary>
        int FinishDrawMillisec { get; }

        /// <summary>
        /// BPM情報
        /// </summary>
        BPM BPMInfo { get; }

        /// <summary>
        /// スクロールスピード情報
        /// </summary>
        ScrollSpeed ScrollSpeedInfo { get; }

        /// <summary>
        /// HBSCROLL用描画データ
        /// </summary>
        HBScrollDrawDataItem HBScrollDrawDataItem { get; set; }

        /// <summary>
        /// HBSCROLL用、開始X座標
        /// </summary>
        double HBScrollStartPointX { get; set; }

        /// <summary>
        /// 1msで動く座標量
        /// </summary>
        float MovementPerMillisec { get; set; }

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
        /// 
        /// </summary>
        object SpecialData { get; set; }

        /// <summary>
        /// 譜面描画用データを構築する
        /// </summary>
        /// <param name="oneMeasureWidth">4/4拍子の1小節分を描画するために必要な幅</param>
        /// <param name="startDrawPointX">描画開始座標X</param>
        /// <param name="finishDrawPointX">描画終了座標X</param>
        /// <param name="playOptionScrollSpeed">設定部のスクロールスピード</param>
        void BuildScoreRendererData(float oneMeasureWidth, float startDrawPointX, float finishDrawPointX, float playOptionScrollSpeed);
    }
}
