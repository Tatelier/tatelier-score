using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatelier.Score.Play.Chart
{
	public class HBScrollDrawDataControl
	{
		public List<HBScrollDrawDataItem> ItemList = new List<HBScrollDrawDataItem>();

		public void Add(HBScrollDrawDataItem item)
		{
			ItemList.Add(item);
		}

		public void Clear()
		{
			ItemList.Clear();
		}

		public HBScrollDrawDataControl()
		{

		}
	}
}
