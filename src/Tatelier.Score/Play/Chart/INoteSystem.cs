﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatelier.Score.Play.Chart
{
	/// <summary>
	/// 音符と小節線の基本インタフェース
	/// </summary>
    public interface INoteSystem
	{
		/// <summary>
		/// 開始時間(ms)
		/// </summary>
		int StartMillisec { get; }

		/// <summary>
		/// 開始時間(ms)
		/// </summary>
		int FinishMillisec { get; }
	}
}