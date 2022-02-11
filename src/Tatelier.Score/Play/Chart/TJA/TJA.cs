﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tatelier.Score.Play.Chart.TJA
{
	public class TJALoadInfo
	{
		/// <summary>
		/// ファイルパス
		/// </summary>
		public string FilePath = string.Empty;

		/// <summary>
		/// 取得するコース名一覧
		/// 0: Player1, n:Player(n+1)
		/// </summary>
		public string[] CourseNames = new string[0];
	}

	/// <summary>
	/// Taiko-san Jiro A
	/// </summary>
	public class TJA
	{
		/// <summary>
		/// ログイベント
		/// </summary>
		public event Action<string> Logged;

		/// <summary>
		/// タイトル
		/// </summary>
		public string Title { get; private set; }

		/// <summary>
		/// 譜面ID
		/// </summary>
		public string Id { get; private set; } = "undefined";

		/// <summary>
		/// ファイルパス
		/// </summary>
		public string FilePath { get; private set; }

		/// <summary>
		/// 音源ファイルパス
		/// </summary>
		public string WaveFileName { get; private set; }

		/// <summary>
		/// 歌詞ファイルパス
		/// </summary>
		public string LyricFileName { get; private set; }

		/// <summary>
		/// 譜面バージョン
		/// </summary>
		public string Version { get; private set; }

		/// <summary>
		/// 開始BPM
		/// </summary>
		public double StartBPM { get; private set; }

		/// <summary>
		/// 音源オフセット(ミリ秒)
		/// </summary>
		public double OffsetMillisec { get; private set; }

		/// <summary>
		/// 譜面リスト
		/// </summary>
		public IReadOnlyList<Score> Scores;

		/// <summary>
		/// 歌詞ファイルパスを取得する。
		/// </summary>
		/// <param name="dir">ディレクトリパス</param>
		/// <returns>歌詞ファイルパス</returns>
		public string GetLyricFilePath(string dir)
		{
            if (LyricFileName?.Length > 0)
            {
				return Path.Combine(dir, LyricFileName);
            }
            else
            {
				var lyricFileName = Path.GetFileNameWithoutExtension(WaveFileName) + ".lrc";
				return Path.Combine(dir, lyricFileName);
            }
		}

		/// <summary>
		/// 譜面描画用データを構築する
		/// </summary>
		/// <param name="oneMeasureWidth">4/4拍子の1小節分を描画するために必要な幅</param>
		/// <param name="startDrawPointX">描画開始座標X</param>
		/// <param name="finishDrawPointX">描画終了座標X</param>
		/// <param name="playOptionScrollSpeed">設定部のスクロールスピード</param>
		public void BuildScoreRendererData(float oneMeasureWidth, float startDrawPointX, float finishDrawPointX, float playOptionScrollSpeed)
		{
			foreach (var item in Scores)
			{
				item.BuildScoreRendererData(oneMeasureWidth, startDrawPointX, finishDrawPointX, playOptionScrollSpeed);
			}
		}

		/// <summary>
		/// 譜面データをロードする
		/// </summary>
		/// <param name="info"></param>
		public void Load(TJALoadInfo info)
		{
			var path = info.FilePath;
			var courseNameList = info.CourseNames;

			FilePath = path;

			var regex = new Regex(@"(\S+):(.+)");

			var score = new List<Score>();

			var encoding = Tatelier.Score.Utility.GetEncodingFromFile(info.FilePath) ?? Encoding.UTF8;

			bool isAll = false;

			if (courseNameList?.Length == 0)
			{
				isAll = true;
				courseNameList = new string[]
				{
					"Oni"
				};
			}

			using (var sr = new StreamReader(path, encoding))
			{
				foreach (var s in courseNameList)
				{
					string courseName = Utility.GetCourse(s);
					string nowCourse = Utility.GetCourse("Oni");
					var sb = new StringBuilder();

					bool nowReadScore = false;
					bool hasHBScroll = false;
					int[] ballonCountArray = new int[0];

					while (!sr.EndOfStream)
					{
						// 1行読む
						var line = sr.ReadLine();

						// #のやつら
						if (line.Length > 0
							&& line[0] == '#')
						{
							if (line.StartsWith("#HBSCROLL"))
							{
								hasHBScroll = true;
							}
							else if (line.StartsWith("#START"))
							{
								nowReadScore = true;
								sb.AppendLine(line);
							}
							else if (line.StartsWith("#END"))
							{
								nowReadScore = false;
								sb.AppendLine(line);

								if (isAll || s == nowCourse)
								{
									score.Add(new Score(sb, new ScoreInfo()
									{
										CourseName = nowCourse,
										StartBPM = StartBPM,
										OffsetMillisec = OffsetMillisec,
										HasHBScroll = hasHBScroll,
										BalloonCountList = ballonCountArray
									}));
								}
								sb.Clear();
								hasHBScroll = false;
							}
							else
							{
								sb.AppendLine(line);
							}
						}
						else
						{
							if (nowReadScore)
							{
								sb.AppendLine(line);
							}
							else
							{
								// 例) line = "TITLE:夏祭り"
								if (regex.IsMatch(line))
								{
									var match = regex.Match(line);
									var groups = match.Groups;

									switch (groups[1].Value.ToUpper())
									{
										case "BALLOON":
											if (courseName == nowCourse)
											{
												ballonCountArray = groups[2].Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(v => int.Parse(v.Replace(" ", string.Empty))).ToArray();
											}
											break;
										case "ID":
											Id = groups[2].Value;
											break;
										case "LYRIC:":
											LyricFileName = groups[2].Value;
											break;
										case "TITLE":
											Title = groups[2].Value;
											break;
										case "WAVE":
											WaveFileName = groups[2].Value;
											break;
										case "OFFSET":
											OffsetMillisec = double.Parse(groups[2].Value);
											break;
										case "BPM":
											StartBPM = double.Parse(groups[2].Value);
											break;
										case "COURSE":
											nowCourse = Utility.GetCourse(groups[2].Value);
											break;
										case "LEVEL":
											break;
									}
								}
							}
						}
					}

					if(nowReadScore 
						&& sb.Length > 0)
					{
						nowReadScore = false;
						score.Add(new Score(sb, new ScoreInfo()
						{
							CourseName = nowCourse,
							StartBPM = StartBPM,
							OffsetMillisec = OffsetMillisec,
							HasHBScroll = hasHBScroll,
							BalloonCountList = ballonCountArray
						}));
						sb.Clear();
					}

					sr.BaseStream.Seek(0, SeekOrigin.Begin);
				}
			}

			Scores = score.ToArray();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TJA()
		{

		}
	}
}
