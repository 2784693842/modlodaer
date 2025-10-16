using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x0200000C RID: 12
	public class FileSelector
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00002B5B File Offset: 0x00000D5B
		public FileSelector(string selectionCriteria) : this(selectionCriteria, true)
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B65 File Offset: 0x00000D65
		public FileSelector(string selectionCriteria, bool traverseDirectoryReparsePoints)
		{
			if (!string.IsNullOrEmpty(selectionCriteria))
			{
				this._Criterion = FileSelector._ParseCriterion(selectionCriteria);
			}
			this.TraverseReparsePoints = traverseDirectoryReparsePoints;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002B88 File Offset: 0x00000D88
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002B9F File Offset: 0x00000D9F
		public string SelectionCriteria
		{
			get
			{
				if (this._Criterion == null)
				{
					return null;
				}
				return this._Criterion.ToString();
			}
			set
			{
				if (value == null)
				{
					this._Criterion = null;
					return;
				}
				if (value.Trim() == "")
				{
					this._Criterion = null;
					return;
				}
				this._Criterion = FileSelector._ParseCriterion(value);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002BD2 File Offset: 0x00000DD2
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002BDA File Offset: 0x00000DDA
		public bool TraverseReparsePoints { get; set; }

		// Token: 0x06000031 RID: 49 RVA: 0x00002BE4 File Offset: 0x00000DE4
		private static string NormalizeCriteriaExpression(string source)
		{
			string[][] array = new string[][]
			{
				new string[]
				{
					"([^']*)\\(\\(([^']+)",
					"$1( ($2"
				},
				new string[]
				{
					"(.)\\)\\)",
					"$1) )"
				},
				new string[]
				{
					"\\((\\S)",
					"( $1"
				},
				new string[]
				{
					"(\\S)\\)",
					"$1 )"
				},
				new string[]
				{
					"^\\)",
					" )"
				},
				new string[]
				{
					"(\\S)\\(",
					"$1 ("
				},
				new string[]
				{
					"\\)(\\S)",
					") $1"
				},
				new string[]
				{
					"(=)('[^']*')",
					"$1 $2"
				},
				new string[]
				{
					"([^ !><])(>|<|!=|=)",
					"$1 $2"
				},
				new string[]
				{
					"(>|<|!=|=)([^ =])",
					"$1 $2"
				},
				new string[]
				{
					"/",
					"\\"
				}
			};
			string input = source;
			for (int i = 0; i < array.Length; i++)
			{
				string pattern = FileSelector.RegexAssertions.PrecededByEvenNumberOfSingleQuotes + array[i][0] + FileSelector.RegexAssertions.FollowedByEvenNumberOfSingleQuotesAndLineEnd;
				input = Regex.Replace(input, pattern, array[i][1]);
			}
			string pattern2 = "/" + FileSelector.RegexAssertions.FollowedByOddNumberOfSingleQuotesAndLineEnd;
			input = Regex.Replace(input, pattern2, "\\");
			pattern2 = " " + FileSelector.RegexAssertions.FollowedByOddNumberOfSingleQuotesAndLineEnd;
			return Regex.Replace(input, pattern2, "\u0006");
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002D7C File Offset: 0x00000F7C
		private static SelectionCriterion _ParseCriterion(string s)
		{
			if (s == null)
			{
				return null;
			}
			s = FileSelector.NormalizeCriteriaExpression(s);
			if (s.IndexOf(" ") == -1)
			{
				s = "name = " + s;
			}
			string[] array = s.Trim().Split(new char[]
			{
				' ',
				'\t'
			});
			if (array.Length < 3)
			{
				throw new ArgumentException(s);
			}
			SelectionCriterion selectionCriterion = null;
			Stack<FileSelector.ParseState> stack = new Stack<FileSelector.ParseState>();
			Stack<SelectionCriterion> stack2 = new Stack<SelectionCriterion>();
			stack.Push(FileSelector.ParseState.Start);
			int i = 0;
			while (i < array.Length)
			{
				string text = array[i].ToLower();
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				FileSelector.ParseState parseState;
				if (num <= 1563699588U)
				{
					if (num <= 739023492U)
					{
						if (num <= 329706515U)
						{
							if (num != 254395046U)
							{
								if (num != 329706515U)
								{
									goto IL_8CA;
								}
								if (!(text == "ctime"))
								{
									goto IL_8CA;
								}
								goto IL_449;
							}
							else
							{
								if (!(text == "and"))
								{
									goto IL_8CA;
								}
								goto IL_310;
							}
						}
						else if (num != 597743964U)
						{
							if (num != 739023492U)
							{
								goto IL_8CA;
							}
							if (!(text == ")"))
							{
								goto IL_8CA;
							}
							parseState = stack.Pop();
							if (stack.Peek() != FileSelector.ParseState.OpenParen)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							stack.Pop();
							stack.Push(FileSelector.ParseState.CriterionDone);
						}
						else
						{
							if (!(text == "size"))
							{
								goto IL_8CA;
							}
							goto IL_552;
						}
					}
					else if (num <= 1058081160U)
					{
						if (num != 755801111U)
						{
							if (num != 1058081160U)
							{
								goto IL_8CA;
							}
							if (!(text == "filename"))
							{
								goto IL_8CA;
							}
							goto IL_73C;
						}
						else
						{
							if (!(text == "("))
							{
								goto IL_8CA;
							}
							parseState = stack.Peek();
							if (parseState != FileSelector.ParseState.Start && parseState != FileSelector.ParseState.ConjunctionPending && parseState != FileSelector.ParseState.OpenParen)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							if (array.Length <= i + 4)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							stack.Push(FileSelector.ParseState.OpenParen);
						}
					}
					else if (num != 1361572173U)
					{
						if (num != 1563699588U)
						{
							goto IL_8CA;
						}
						if (!(text == "or"))
						{
							goto IL_8CA;
						}
						goto IL_310;
					}
					else
					{
						if (!(text == "type"))
						{
							goto IL_8CA;
						}
						goto IL_80C;
					}
				}
				else if (num <= 2746858573U)
				{
					if (num <= 2211460629U)
					{
						if (num != 2166136261U)
						{
							if (num != 2211460629U)
							{
								goto IL_8CA;
							}
							if (!(text == "length"))
							{
								goto IL_8CA;
							}
							goto IL_552;
						}
						else
						{
							if (text == null)
							{
								goto IL_8CA;
							}
							if (text.Length != 0)
							{
								goto IL_8CA;
							}
							stack.Push(FileSelector.ParseState.Whitespace);
						}
					}
					else if (num != 2369371622U)
					{
						if (num != 2746858573U)
						{
							goto IL_8CA;
						}
						if (!(text == "atime"))
						{
							goto IL_8CA;
						}
						goto IL_449;
					}
					else
					{
						if (!(text == "name"))
						{
							goto IL_8CA;
						}
						goto IL_73C;
					}
				}
				else if (num <= 3429620606U)
				{
					if (num != 2888110417U)
					{
						if (num != 3429620606U)
						{
							goto IL_8CA;
						}
						if (!(text == "xor"))
						{
							goto IL_8CA;
						}
						goto IL_310;
					}
					else
					{
						if (!(text == "mtime"))
						{
							goto IL_8CA;
						}
						goto IL_449;
					}
				}
				else if (num != 3791641492U)
				{
					if (num != 4191246291U)
					{
						goto IL_8CA;
					}
					if (!(text == "attrs"))
					{
						goto IL_8CA;
					}
					goto IL_80C;
				}
				else
				{
					if (!(text == "attributes"))
					{
						goto IL_8CA;
					}
					goto IL_80C;
				}
				IL_8E3:
				parseState = stack.Peek();
				if (parseState == FileSelector.ParseState.CriterionDone)
				{
					stack.Pop();
					if (stack.Peek() == FileSelector.ParseState.ConjunctionPending)
					{
						while (stack.Peek() == FileSelector.ParseState.ConjunctionPending)
						{
							CompoundCriterion compoundCriterion = stack2.Pop() as CompoundCriterion;
							compoundCriterion.Right = selectionCriterion;
							selectionCriterion = compoundCriterion;
							stack.Pop();
							parseState = stack.Pop();
							if (parseState != FileSelector.ParseState.CriterionDone)
							{
								throw new ArgumentException("??");
							}
						}
					}
					else
					{
						stack.Push(FileSelector.ParseState.CriterionDone);
					}
				}
				if (parseState == FileSelector.ParseState.Whitespace)
				{
					stack.Pop();
				}
				i++;
				continue;
				IL_310:
				parseState = stack.Peek();
				if (parseState != FileSelector.ParseState.CriterionDone)
				{
					throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
				}
				if (array.Length <= i + 3)
				{
					throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
				}
				LogicalConjunction conjunction = (LogicalConjunction)Enum.Parse(typeof(LogicalConjunction), array[i].ToUpper(), true);
				selectionCriterion = new CompoundCriterion
				{
					Left = selectionCriterion,
					Right = null,
					Conjunction = conjunction
				};
				stack.Push(parseState);
				stack.Push(FileSelector.ParseState.ConjunctionPending);
				stack2.Push(selectionCriterion);
				goto IL_8E3;
				IL_449:
				if (array.Length <= i + 2)
				{
					throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
				}
				DateTime dateTime;
				try
				{
					dateTime = DateTime.ParseExact(array[i + 2], "yyyy-MM-dd-HH:mm:ss", null);
				}
				catch (FormatException)
				{
					try
					{
						dateTime = DateTime.ParseExact(array[i + 2], "yyyy/MM/dd-HH:mm:ss", null);
					}
					catch (FormatException)
					{
						try
						{
							dateTime = DateTime.ParseExact(array[i + 2], "yyyy/MM/dd", null);
						}
						catch (FormatException)
						{
							try
							{
								dateTime = DateTime.ParseExact(array[i + 2], "MM/dd/yyyy", null);
							}
							catch (FormatException)
							{
								dateTime = DateTime.ParseExact(array[i + 2], "yyyy-MM-dd", null);
							}
						}
					}
				}
				dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local).ToUniversalTime();
				selectionCriterion = new TimeCriterion
				{
					Which = (WhichTime)Enum.Parse(typeof(WhichTime), array[i], true),
					Operator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]),
					Time = dateTime
				};
				i += 2;
				stack.Push(FileSelector.ParseState.CriterionDone);
				goto IL_8E3;
				IL_552:
				if (array.Length <= i + 2)
				{
					throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
				}
				string text2 = array[i + 2];
				long size;
				if (text2.ToUpper().EndsWith("K"))
				{
					size = long.Parse(text2.Substring(0, text2.Length - 1)) * 1024L;
				}
				else if (text2.ToUpper().EndsWith("KB"))
				{
					size = long.Parse(text2.Substring(0, text2.Length - 2)) * 1024L;
				}
				else if (text2.ToUpper().EndsWith("M"))
				{
					size = long.Parse(text2.Substring(0, text2.Length - 1)) * 1024L * 1024L;
				}
				else if (text2.ToUpper().EndsWith("MB"))
				{
					size = long.Parse(text2.Substring(0, text2.Length - 2)) * 1024L * 1024L;
				}
				else if (text2.ToUpper().EndsWith("G"))
				{
					size = long.Parse(text2.Substring(0, text2.Length - 1)) * 1024L * 1024L * 1024L;
				}
				else if (text2.ToUpper().EndsWith("GB"))
				{
					size = long.Parse(text2.Substring(0, text2.Length - 2)) * 1024L * 1024L * 1024L;
				}
				else
				{
					size = long.Parse(array[i + 2]);
				}
				selectionCriterion = new SizeCriterion
				{
					Size = size,
					Operator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1])
				};
				i += 2;
				stack.Push(FileSelector.ParseState.CriterionDone);
				goto IL_8E3;
				IL_73C:
				if (array.Length <= i + 2)
				{
					throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
				}
				ComparisonOperator comparisonOperator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]);
				if (comparisonOperator != ComparisonOperator.NotEqualTo && comparisonOperator != ComparisonOperator.EqualTo)
				{
					throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
				}
				string text3 = array[i + 2];
				if (text3.StartsWith("'") && text3.EndsWith("'"))
				{
					text3 = text3.Substring(1, text3.Length - 2).Replace("\u0006", " ");
				}
				selectionCriterion = new NameCriterion
				{
					MatchingFileSpec = text3,
					Operator = comparisonOperator
				};
				i += 2;
				stack.Push(FileSelector.ParseState.CriterionDone);
				goto IL_8E3;
				IL_80C:
				if (array.Length <= i + 2)
				{
					throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
				}
				ComparisonOperator comparisonOperator2 = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]);
				if (comparisonOperator2 != ComparisonOperator.NotEqualTo && comparisonOperator2 != ComparisonOperator.EqualTo)
				{
					throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
				}
				SelectionCriterion selectionCriterion2;
				if (!(text == "type"))
				{
					AttributesCriterion attributesCriterion = new AttributesCriterion();
					attributesCriterion.AttributeString = array[i + 2];
					selectionCriterion2 = attributesCriterion;
					attributesCriterion.Operator = comparisonOperator2;
				}
				else
				{
					TypeCriterion typeCriterion = new TypeCriterion();
					typeCriterion.AttributeString = array[i + 2];
					selectionCriterion2 = typeCriterion;
					typeCriterion.Operator = comparisonOperator2;
				}
				selectionCriterion = selectionCriterion2;
				i += 2;
				stack.Push(FileSelector.ParseState.CriterionDone);
				goto IL_8E3;
				IL_8CA:
				throw new ArgumentException("'" + array[i] + "'");
			}
			return selectionCriterion;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003724 File Offset: 0x00001924
		public override string ToString()
		{
			return "FileSelector(" + this._Criterion.ToString() + ")";
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003740 File Offset: 0x00001940
		private bool Evaluate(string filename)
		{
			return this._Criterion.Evaluate(filename);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000374E File Offset: 0x0000194E
		[Conditional("SelectorTrace")]
		private void SelectorTrace(string format, params object[] args)
		{
			if (this._Criterion != null && this._Criterion.Verbose)
			{
				Console.WriteLine(format, args);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000376C File Offset: 0x0000196C
		public ICollection<string> SelectFiles(string directory)
		{
			return this.SelectFiles(directory, false);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003778 File Offset: 0x00001978
		public ReadOnlyCollection<string> SelectFiles(string directory, bool recurseDirectories)
		{
			if (this._Criterion == null)
			{
				throw new ArgumentException("SelectionCriteria has not been set");
			}
			List<string> list = new List<string>();
			try
			{
				if (Directory.Exists(directory))
				{
					foreach (string text in Directory.GetFiles(directory))
					{
						if (this.Evaluate(text))
						{
							list.Add(text);
						}
					}
					if (recurseDirectories)
					{
						foreach (string text2 in Directory.GetDirectories(directory))
						{
							if (this.TraverseReparsePoints || (File.GetAttributes(text2) & FileAttributes.ReparsePoint) == (FileAttributes)0)
							{
								if (this.Evaluate(text2))
								{
									list.Add(text2);
								}
								list.AddRange(this.SelectFiles(text2, recurseDirectories));
							}
						}
					}
				}
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (IOException)
			{
			}
			return list.AsReadOnly();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003850 File Offset: 0x00001A50
		private bool Evaluate(ZipEntry entry)
		{
			return this._Criterion.Evaluate(entry);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003860 File Offset: 0x00001A60
		public ICollection<ZipEntry> SelectEntries(ZipFile zip)
		{
			if (zip == null)
			{
				throw new ArgumentNullException("zip");
			}
			List<ZipEntry> list = new List<ZipEntry>();
			foreach (ZipEntry zipEntry in zip)
			{
				if (this.Evaluate(zipEntry))
				{
					list.Add(zipEntry);
				}
			}
			return list;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000038C8 File Offset: 0x00001AC8
		public ICollection<ZipEntry> SelectEntries(ZipFile zip, string directoryPathInArchive)
		{
			if (zip == null)
			{
				throw new ArgumentNullException("zip");
			}
			List<ZipEntry> list = new List<ZipEntry>();
			string text = (directoryPathInArchive == null) ? null : directoryPathInArchive.Replace("/", "\\");
			if (text != null)
			{
				while (text.EndsWith("\\"))
				{
					text = text.Substring(0, text.Length - 1);
				}
			}
			foreach (ZipEntry zipEntry in zip)
			{
				if ((directoryPathInArchive == null || Path.GetDirectoryName(zipEntry.FileName) == directoryPathInArchive || Path.GetDirectoryName(zipEntry.FileName) == text) && this.Evaluate(zipEntry))
				{
					list.Add(zipEntry);
				}
			}
			return list;
		}

		// Token: 0x04000022 RID: 34
		internal SelectionCriterion _Criterion;

		// Token: 0x02000058 RID: 88
		private enum ParseState
		{
			// Token: 0x040002D6 RID: 726
			Start,
			// Token: 0x040002D7 RID: 727
			OpenParen,
			// Token: 0x040002D8 RID: 728
			CriterionDone,
			// Token: 0x040002D9 RID: 729
			ConjunctionPending,
			// Token: 0x040002DA RID: 730
			Whitespace
		}

		// Token: 0x02000059 RID: 89
		private static class RegexAssertions
		{
			// Token: 0x040002DB RID: 731
			public static readonly string PrecededByOddNumberOfSingleQuotes = "(?<=(?:[^']*'[^']*')*'[^']*)";

			// Token: 0x040002DC RID: 732
			public static readonly string FollowedByOddNumberOfSingleQuotesAndLineEnd = "(?=[^']*'(?:[^']*'[^']*')*[^']*$)";

			// Token: 0x040002DD RID: 733
			public static readonly string PrecededByEvenNumberOfSingleQuotes = "(?<=(?:[^']*'[^']*')*[^']*)";

			// Token: 0x040002DE RID: 734
			public static readonly string FollowedByEvenNumberOfSingleQuotesAndLineEnd = "(?=(?:[^']*'[^']*')*[^']*$)";
		}
	}
}
