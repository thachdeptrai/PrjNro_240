using System.Collections.Generic;

namespace Mod.XMAP
{
    public struct GroupMap
	{
		public GroupMap(string nameGroup, List<int> idMaps)
		{
			NameGroup = nameGroup;
			IdMaps = idMaps;
		}

		public string NameGroup;

		public List<int> IdMaps;
	}
}
