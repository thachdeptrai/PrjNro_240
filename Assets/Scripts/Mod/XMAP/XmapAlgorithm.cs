using System.Collections.Generic;

namespace Mod.XMAP
{
    public class XmapAlgorithm
	{
		public static List<int> FindWay(int idMapStart, int idMapEnd)
		{
			List<int> wayPassedStart = GetWayPassedStart(idMapStart);
			return FindWay(idMapEnd, wayPassedStart);
		}

		private static List<int> FindWay(int idMapEnd, List<int> wayPassed)
		{
			int num = wayPassed[^1];
			bool flag = num == idMapEnd;
			List<int> result;
			if (flag)
			{
				result = wayPassed;
			}
			else
			{
				bool flag2 = !XmapData.GI().CanGetMapNexts(num);
				if (flag2)
				{
					result = null;
				}
				else
				{
					List<List<int>> list = new();
					foreach (MapNext mapNext in XmapData.GI().GetMapNexts(num))
					{
						List<int> list2 = null;
						bool flag3 = !wayPassed.Contains(mapNext.MapID);
						if (flag3)
						{
							List<int> wayPassedNext = GetWayPassedNext(wayPassed, mapNext.MapID);
							list2 = FindWay(idMapEnd, wayPassedNext);
						}
						bool flag4 = list2 != null;
						if (flag4)
						{
							list.Add(list2);
						}
					}
					result = GetBestWay(list);
				}
			}
			return result;
		}

		private static List<int> GetBestWay(List<List<int>> ways)
		{
			bool flag = ways.Count == 0;
			List<int> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				List<int> list = ways[0];
				for (int i = 1; i < ways.Count; i++)
				{
					bool flag2 = IsWayBetter(ways[i], list);
					if (flag2)
					{
						list = ways[i];
					}
				}
				result = list;
			}
			return result;
		}

		private static List<int> GetWayPassedStart(int idMapStart)
		{
			return new List<int>
			{
				idMapStart
			};
		}

		private static List<int> GetWayPassedNext(List<int> wayPassed, int idMapNext)
		{
			return new List<int>(wayPassed)
			{
				idMapNext
			};
		}

		private static bool IsWayBetter(List<int> way1, List<int> way2)
		{
			bool flag = IsBadWay(way1);
			bool flag2 = IsBadWay(way2);
			return (!flag || flag2) && ((!flag && flag2) || way1.Count < way2.Count);
		}

		private static bool IsBadWay(List<int> way)
		{
			return IsWayGoFutureAndBack(way);
		}

		private static bool IsWayGoFutureAndBack(List<int> way)
		{
			List<int> list = new()
            {
				27,
				28,
				29
			};
			for (int i = 1; i < way.Count - 1; i++)
			{
				bool flag = way[i] == 102 && way[i + 1] == 24 && list.Contains(way[i - 1]);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}
	}
}
