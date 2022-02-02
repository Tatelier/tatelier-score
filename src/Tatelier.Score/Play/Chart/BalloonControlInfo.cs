using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatelier.Score.Play.Chart
{
	public class BalloonControlInfo
	{
		public List<(int, int)> CommonScore = new List<(int, int)>();

		public List<(int, int)> NormalScore = new List<(int, int)>(0);

		public List<(int, int)> ExpertScore = new List<(int, int)>(0);

		public List<(int, int)> MasterScore = new List<(int, int)>(0);

		public int Add(int time, int balloonCount, BranchType branchType)
		{
			switch (branchType)
			{
				case BranchType.Common:
					CommonScore.Add((time, balloonCount));
					break;
				case BranchType.Normal:
					NormalScore.Add((time, balloonCount));
					break;
				case BranchType.Expert:
					ExpertScore.Add((time, balloonCount));
					break;
				case BranchType.Master:
					MasterScore.Add((time, balloonCount));
					break;
			}

			return 0;
		}
	}
}
