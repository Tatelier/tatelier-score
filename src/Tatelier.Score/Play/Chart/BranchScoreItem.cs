using System.Collections.Generic;
using System.Linq;
using Tatelier.Score.Component;

namespace Tatelier.Score.Play.Chart
{
    public class BranchScore
	{
		public bool IsFixed = false;
		public int StartMillisec;

		public List<IMeasureLine> Measures = new List<IMeasureLine>();
		public List<INote> Notes = new List<INote>();

		public List<BPM> BPMList = new List<BPM>();
		public List<Measure> MeasureList = new List<Measure>();
		public List<ScrollSpeed> ScrollSpeedList = new List<ScrollSpeed>();

		readonly SortedDictionary<int, List<List<INote>>> noteList;

		public HBScrollDrawDataControl HBScrollDrawDataControl = new HBScrollDrawDataControl();

		/// <summary>
		/// 音符リスト
		/// 1次元目: レイヤー層
		/// 2次元目: セクション層
		/// 3次元目: 音符リスト
		/// </summary>
		public INote[][][] NoteList;

		int nowLayer = 0;

		public int GetDonKatSum()
		{
			return Notes.Count(v =>
			{
				switch (v.NoteType)
				{
					case NoteType.Don:
					case NoteType.Kat:
					case NoteType.DonBig:
					case NoteType.KatBig:
						return true;
					default:
						return false;
				}
			});
		}

		public void Build()
		{
			NoteList = noteList.Select(v => v.Value.Select(w => w.ToArray()).ToArray()).ToArray();

			foreach(var note in Notes)
			{
				BPM prev = BPMList.LastOrDefault();

				foreach (var item in BPMList.Reverse<BPM>())
				{
					if(item.StartMillisec <= note.StartMillisec && note.StartMillisec < item.FinishMillisec)
					{
						if (item.IsDelay)
						{
							prev.AddNote(note);
							break;
						}
						else
						{
							item.AddNote(note);
							break;
						}
					}

					prev = item;
				}
			}

			foreach(var measure in Measures)
			{
				BPM prev = BPMList.LastOrDefault();

				foreach (var item in BPMList.Reverse<BPM>())
				{
					if (item.StartMillisec <= measure.StartMillisec && measure.StartMillisec < item.FinishMillisec)
					{
						if (item.IsDelay)
						{
							prev.AddMeasure(measure);
							break;
						}
						else
						{
							item.AddMeasure(measure);
							break;
						}
					}
					prev = item;
				}
			}
		}

		public void ChangeLayer(int layer)
		{
			nowLayer = layer;
			if (!noteList.ContainsKey(nowLayer))
			{
				noteList[nowLayer] = new List<List<INote>>();
			}
		}

		public void ChangeSection()
		{
			noteList[nowLayer].Add(new List<INote>());
		}

		public void AddMeasure(IMeasureLine measureLine)
		{
			Measures.Add(measureLine);

		}

		public void AddNote(INote note)
		{
			Notes.Add(note);
			noteList[nowLayer][noteList[nowLayer].Count - 1].Add(note);
		}

		public void Reset()
		{
			IsFixed = false;
		}

		public BranchScore(NotePivotInfo info)
		{
			BPMList.Add(new BPM(info.PivotMillisec, info.BPMInfo));
			MeasureList.Add(new Measure(info.PivotMillisec, info.MeasureInfo));
			ScrollSpeedList.Add(new ScrollSpeed(info.PivotMillisec, info.ScrollSpeedInfo));

			noteList = new SortedDictionary<int, List<List<INote>>>();
			noteList[nowLayer] = new List<List<INote>>();

			ChangeSection();
		}
	}
}