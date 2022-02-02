using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatelier.Score.Play.Chart
{
	public static class Utility
	{
		/// <summary>
		/// 一意のコース名を取得する
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string GetCourse(string text)
		{
			switch (text.Replace(" ", "").ToUpper())
			{
				case "0":
				case "EASY":
					return "Easy";
				case "1":
				case "NORMAL":
					return "Normal";
				case "2":
				case "HARD":
					return "Hard";
				case "3":
				case "ONI":
					return "Oni";
				case "4":
				case "EDIT":
				case "URA":
					return "Edit";
				default:
					return text;
			}
		}
	}
}
