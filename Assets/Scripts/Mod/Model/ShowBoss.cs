using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod
{
	public class ShowBoss
	{
		public ShowBoss(string a)
		{
			a = a.Replace(a.Substring(0, 5), "|");
			a = a.Replace(" vừa xuất hiện tại", "|");
			a = a.Replace(" khu vực", "|");
			string[] array = a.Split(new char[]
			{
			'|'
			});
			nameBoss = array[1].Trim();
			mapName = array[2].Trim();
			mapID = ModFunc.GI().GetMapID(mapName);
            time = DateTime.Now.Millisecond;
		}

		public void PaintBoss(mGraphics g, int x, int y, int align)
		{
            g.fillRect(GameCanvas.w - 20, y + 3, 20, 10, 2721889, 90);
            string timeAppear = NinjaUtil.getTimeAgo(time / 1000);
			mFont mFont = mFont.tahoma_7_orange;
			if (TileMap.mapName.Trim().ToLower() == mapName.Trim().ToLower())
			{
				mFont = mFont.tahoma_7_red;
				for (int i = 0; i < GameScr.vCharInMap.size(); i++)
				{
					Char @char = (Char)GameScr.vCharInMap.elementAt(i);
					if (@char.cName == nameBoss)
					{
						mFont = mFont.tahoma_7b_red;
						break;
					}
				}
			}
			mFont.drawStringBorder(g, string.Concat(new object[]
			{
			nameBoss,
			" - ",
			mapName,
			" [",
			mapID,
			"] - ",
            timeAppear,
			" <<"
			}), x, y, align, mFont.tahoma_7_grey);
		}

		public string nameBoss;

		public string mapName;

		public string playerKill;

		public int mapID;

		public long time;
	}
}
