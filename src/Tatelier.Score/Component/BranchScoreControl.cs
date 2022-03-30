using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tatelier.Score.Component.NoteSystem;

namespace Tatelier.Score.Component
{
    /// <summary>
    /// 分岐譜面管理クラス
    /// ※分岐のない譜面も使う
    /// </summary>
    public class BranchScoreControl
    {
        /// <summary>
        /// 演奏用
        /// 譜面分岐開始時間のリスト
        /// </summary>
        public List<int> BranchStartTimeList { get; } = new List<int>();

        /// <summary>
        /// 分岐切替準備時間配列(1小節前時間リスト)
        /// ※要素数はBranchTypeListと同じになる
        /// TODO: 一元管理化したい
        /// </summary>
        public List<int> PrepareBranchTimeList { get; } = new List<int>();

        /// <summary>
        /// 分岐種別を保持するためのリスト
        /// </summary>
        public List<(BranchType BranchType, int Time)> LevelHoldList = new List<(BranchType BranchType, int Time)>();

        /// <summary>
        /// 演奏用
        /// 時間帯毎に分岐種別を管理するマップ
        /// T1: int 開始時間(ms) 
        /// T2: BranchType 分岐種別
        /// 
        /// ※演奏時の途中結果でBranchTypeが更新される
        /// </summary>
        public SortedDictionary<int, BranchType> BranchTypeList = new SortedDictionary<int, BranchType>();

        public SortedDictionary<int, BranchScore> CommonScoreList = new SortedDictionary<int, BranchScore>();
        public SortedDictionary<int, BranchScore> NormalScoreList = new SortedDictionary<int, BranchScore>();
        public SortedDictionary<int, BranchScore> ExpertScoreList = new SortedDictionary<int, BranchScore>();
        public SortedDictionary<int, BranchScore> MasterScoreList = new SortedDictionary<int, BranchScore>();


        BranchType nowBranchType;

        public BranchScore NowBranchScore { get; set; }

        int nowDeadlineIndex = 0;

        public List<int> OneBeforeMeasureTime = new List<int>();

        /// <summary>
        /// すべての音符を列挙する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<INote> EnumratesAllNote()
        {
            foreach (var item in GetAllBranchScoreList().Select(v => v.BranchScore))
            {
                foreach (var note in item.Notes)
                {
                    yield return note;
                }
            }
        }

        /// <summary>
        /// すべての小節線を列挙する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IMeasureLine> EnumratesAllMeasureLine()
        {
            foreach (var item in GetAllBranchScoreList().Select(v => v.BranchScore))
            {
                foreach (var measure in item.Measures)
                {
                    yield return measure;
                }
            }
        }

        /// <summary>
        /// 分岐切替タイミングかどうかを取得する
        /// </summary>
        /// <param name="time">演奏経過時間(ms)</param>
        /// <returns>true: 切替, false: 何もしない</returns>
        public bool TryChangeDeadline(int time)
        {
            if (nowDeadlineIndex == OneBeforeMeasureTime.Count) return false;

            if (OneBeforeMeasureTime[nowDeadlineIndex] < time)
            {
                nowDeadlineIndex++;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 分岐状態を保持するかどうかを取得する
        /// </summary>
        /// <param name="time">演奏経過時間(ms)</param>
        /// <returns>true: 保持する, false: 何もしない</returns>
        public bool TryLevelHold(int time)
        {
            foreach (var item in LevelHoldList)
            {
                if (nowBranchType == item.BranchType
                    && item.Time < time)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 分岐情報を変更する
        /// </summary>
        /// <param name="time">変更対象の時間</param>
        /// <param name="branchType">変更後の分岐種別</param>
        public void ChangeBranchType(int time, BranchType branchType)
        {
            // カレントの分岐種別を更新する
            nowBranchType = branchType;

            for (int i = 0; i < BranchStartTimeList.Count; i++)
            {
                // 準備時間より現在時間が長い場合はスキップ
                if (OneBeforeMeasureTime[i] < time) continue;

                // 準備時間の方が長い = [i-1]～[i]の範囲が変更対象となるため、
                // [i-1]の分岐情報を変更する。
                BranchTypeList[BranchStartTimeList[i - 1]] = branchType;
                return;
            }

            // 最後の分岐情報を変える
            BranchTypeList[BranchStartTimeList.Last()] = branchType;
        }


        /// <summary>
        /// ※未使用
        /// </summary>
        /// <returns></returns>
        public IEnumerable<(BranchType Type, BranchScore Score)> GetBranchTypeScoreList()
        {
            foreach (var item in CommonScoreList.Values)
            {
                yield return (BranchType.Common, item);
            }

            foreach (var t in BranchTypeList)
            {
                switch (t.Value)
                {
                    case BranchType.Normal:
                        {
                            if (NormalScoreList.TryGetValue(t.Key, out var score))
                            {
                                yield return (BranchType.Normal, score);
                            }
                        }
                        break;
                    case BranchType.Expert:
                        {
                            if (ExpertScoreList.TryGetValue(t.Key, out var score))
                            {
                                yield return (BranchType.Expert, score);
                            }
                        }
                        break;
                    case BranchType.Master:
                        {
                            if (MasterScoreList.TryGetValue(t.Key, out var score))
                            {
                                yield return (BranchType.Master, score);
                            }
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// 分岐譜面リストを取得する
        /// </summary>
        /// <remarks>
        /// ※描画処理で利用する場合は、LinQでReverseをかけて最後尾から処理をすると想定の挙動をする
        /// </remarks>
        /// <returns>分岐譜面リスト</returns>
#warning Pansystar::共通譜面と分岐譜面の重なりがテレコしているため修正が必要
        public IEnumerable<BranchScore> GetBranchScoreList()
        {
            foreach (var t in BranchTypeList)
            {
                switch (t.Value)
                {
                    case BranchType.Normal:
                        {
                            if (NormalScoreList.TryGetValue(t.Key, out var score))
                            {
                                yield return score;
                            }
                        }
                        break;
                    case BranchType.Expert:
                        {
                            if (ExpertScoreList.TryGetValue(t.Key, out var score))
                            {
                                yield return score;
                            }
                        }
                        break;
                    case BranchType.Master:
                        {
                            if (MasterScoreList.TryGetValue(t.Key, out var score))
                            {
                                yield return score;
                            }
                        }
                        break;
                }
            }

            foreach (var item in CommonScoreList.Values)
            {
                yield return item;
            }

        }

        /// <summary>
        /// 未使用
        /// </summary>
        /// <returns></returns>
        public IEnumerable<(BranchType BranchType, BranchScore BranchScore)> GetAllBranchScoreList()
        {
            foreach (var item in CommonScoreList) yield return (BranchType.Common, item.Value);

            foreach (var item in NormalScoreList) yield return (BranchType.Normal, item.Value);

            foreach (var item in ExpertScoreList) yield return (BranchType.Expert, item.Value);

            foreach (var item in MasterScoreList) yield return (BranchType.Master, item.Value);
        }

        public BPM GetBPMInfo(int millisec, BranchType branchType)
        {
            SortedDictionary<int, BranchScore> list = null;

            switch (branchType)
            {
                case BranchType.Normal:
                    {
                        list = NormalScoreList;
                    }
                    break;
                case BranchType.Expert:
                    {
                        list = ExpertScoreList;
                    }
                    break;
                case BranchType.Master:
                    {
                        list = MasterScoreList;
                    }
                    break;
                case BranchType.Common:
                    {
                        list = CommonScoreList;
                    }
                    break;
            }

            foreach (var item in list.Values)
            {
                foreach (var bpm in item.BPMList)
                {
                    if (bpm.StartMillisec <= millisec && millisec < bpm.FinishMillisec)
                    {
                        return bpm;
                    }
                }
            }

            foreach (var item in CommonScoreList.Values)
            {
                foreach (var bpm in item.BPMList)
                {
                    if (bpm.StartMillisec <= millisec && millisec < bpm.FinishMillisec)
                    {
                        return bpm;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 音符リストを構築する
        /// ※各処理で使う前に必ず呼び出すこと
        /// </summary>
        public void Build()
        {
            foreach (var item in CommonScoreList.Values) item.Build();
            foreach (var item in NormalScoreList.Values) item.Build();
            foreach (var item in ExpertScoreList.Values) item.Build();
            foreach (var item in MasterScoreList.Values) item.Build();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BranchScoreControl()
        {
            // 特に処理しない
        }
    }
}
