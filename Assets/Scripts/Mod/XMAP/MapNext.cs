namespace Mod.XMAP
{
    public struct MapNext
	{
		public MapNext(int mapID, TypeMapNext type, int[] info)
		{
			MapID = mapID;
			Type = type;
			Info = info;
		}

		public int MapID;

		public TypeMapNext Type;

		public int[] Info;
	}
}
