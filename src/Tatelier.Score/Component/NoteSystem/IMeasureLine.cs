using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tatelier.Score.Component;
using Tatelier.Score.Play.Chart;

namespace Tatelier.Score.Component.NoteSystem
{
    /// <summary>
    /// 小節線インターフェース
    /// </summary>
    public interface IMeasureLine : INoteSystem
    {
        /// <summary>
        /// 小節線ID
        /// </summary>
        /// <remarks>
        /// デバッグで利用
        /// </remarks>
        int Id { get; }

        /// <summary>
        /// 小節線種別
        /// </summary>
        MeasureLineType MeasureLineType { get; }

        /// <summary>
        /// HBSCROLL用、開始X座標
        /// </summary>
        double HBScrollStartPointX { get; set; }

        /// <summary>
        /// HBSCROLL用描画データ
        /// </summary>
        HBScrollDrawDataItem HBScrollDrawDataItem { get; set; }

        /// <summary>
        /// 1msで動く座標量
        /// </summary>
        float MovementPerMillisec { get; set; }

        /// <summary>
        /// BPM情報
        /// </summary>
        BPM BPMInfo { get; }

        /// <summary>
        /// スクロールスピード情報
        /// </summary>
        ScrollSpeed ScrollSpeedInfo { get; }

        /// <summary>
        /// 表示
        /// </summary>
        bool Visible { get; }

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
