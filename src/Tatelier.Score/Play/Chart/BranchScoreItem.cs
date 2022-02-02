using System.Collections.Generic;
using System.Linq;

namespace Tatelier.Score.Play.Chart
{
	public class BranchScoreItem
	{
		public bool IsFixed = false;
		public int StartMillisec;

		public List<IMeasureLine> Measures = new List<IMeasureLine>();
		public List<INote> Notes = new List<INote>();

		public List<BPMInfo> BPMInfoList = new List<BPMInfo>();
		public List<MeasureInfo> MeasureInfoList = new List<MeasureInfo>();
		public List<ScrollSpeedInfo> ScrollSpeedInfoList = new List<ScrollSpeedInfo>();

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
				foreach(var item in BPMInfoList.Reverse<BPMInfo>())
				{
					if(item.StartMillisec <= note.StartMillisec && note.StartMillisec < item.EndMillisec)
					{
						item.AddNote(note);
						break;
					}
				}
			}

			foreach(var measure in Measures)
			{
				foreach (var item in BPMInfoList.Reverse<BPMInfo>())
				{
					if (item.StartMillisec <= measure.StartMillisec && measure.StartMillisec < item.EndMillisec)
					{
						item.AddMeasure(measure);
						break;
					}
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

		public BranchScoreItem(NotePivotInfo info)
		{
			BPMInfoList.Add(new BPMInfo(info.PivotMillisec, info.BPMInfo));
			MeasureInfoList.Add(new MeasureInfo(info.PivotMillisec, info.MeasureInfo));
			ScrollSpeedInfoList.Add(new ScrollSpeedInfo(info.PivotMillisec, info.ScrollSpeedInfo));

			noteList = new SortedDictionary<int, List<List<INote>>>();
			noteList[nowLayer] = new List<List<INote>>();

			ChangeSection();
		}
	}
}