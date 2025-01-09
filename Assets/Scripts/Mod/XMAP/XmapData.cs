using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.XMAP
{
    public class XmapData
    {
        private XmapData()
        {
            GroupMaps = new List<GroupMap>();
            MyLinkMaps = null;
            IsLoading = false;
            IsLoadingCapsule = false;
        }

        public static XmapData GI()
        {
            Instance ??= new XmapData();
            return Instance;
        }

        public void LoadLinkMaps()
        {
            IsLoading = true;
        }

        public void Update()
        {
            bool isLoadingCapsule = IsLoadingCapsule;
            if (isLoadingCapsule)
            {
                if (!IsWaitInfoMapTrans())
                {
                    LoadLinkMapCapsule();
                    IsLoadingCapsule = false;
                    IsLoading = false;
                }
            }
            else
            {
                LoadLinkMapBase();
                if (CanUseCapsuleVip())
                {
                    XmapController.UseCapsuleVip();
                    IsLoadingCapsule = true;
                }
                else
                {
                    if (CanUseCapsuleNormal())
                    {
                        XmapController.UseCapsuleNormal();
                        IsLoadingCapsule = true;
                    }
                    else
                    {
                        IsLoading = false;
                    }
                }
            }
        }

        public void LoadGroupMapsFromFile()
        {
            List<int> idMaps = new()
            {
                44,
                23,
                14,
                15,
                16,
                17,
                18,
                20,
                19,
                35,
                36,
                37,
                38,
                26,
                52,
                84,
                129
            };
            GroupMapData map1 = new(idMaps, "Xayda");
            idMaps = new List<int>
            {
                42,21,0,1,2,3,4,5,6,27,28,29,30,47,46,45,48,50,111,24,53,58,59,60,61,62,55,56,54,57
            };
            GroupMapData map2 = new(idMaps, "Trái Đất");
            idMaps = new List<int>
            {
                43, 22, 7 ,8 ,9 ,11, 12, 13, 10, 31, 32 ,33 ,34 ,25
            };
            GroupMapData map3 = new(idMaps, "Namek");
            idMaps = new List<int>
            {
                68,69,70,71,72,64,65,63,66,67,73,74,75,76,77,81,82,83,79,80,131,132,133
            };
            GroupMapData map4 = new(idMaps, "Nappa");
            idMaps = new List<int>
            {
                102,92,93,94,96,97,98,99,100,103
            };
            GroupMapData map5 = new(idMaps, "Tương Lai");
            idMaps = new List<int>
            {
                109,108,107,110,106,105
            };
            GroupMapData map6 = new(idMaps, "Cold");
            //idMaps = new List<int>
            //{
            //    160,161,162,163
            //};
            //GroupMapData map7 = new(idMaps, "Hành Tinh\nThực Vật");
            //idMaps = new List<int>
            //{
            //    139,140
            //};
            //GroupMapData map8 = new(idMaps, "Potaufeu");
            //idMaps = new List<int>
            //{
            //    149,147,152,151,148
            //};
            //GroupMapData map9 = new(idMaps, "Khí Gas");
            idMaps = new List<int>
            {
                122,123,124
            };
            GroupMapData map10 = new(idMaps, "Ngũ Hành Sơn");
            GroupMaps.Clear();
            try
            {
                GroupMaps.Add(new GroupMap(map1.mapname, map1.datamap));
                GroupMaps.Add(new GroupMap(map2.mapname, map2.datamap));
                GroupMaps.Add(new GroupMap(map3.mapname, map3.datamap));
                GroupMaps.Add(new GroupMap(map4.mapname, map4.datamap));
                GroupMaps.Add(new GroupMap(map5.mapname, map5.datamap));
                GroupMaps.Add(new GroupMap(map6.mapname, map6.datamap));
                //GroupMaps.Add(new GroupMap(map7.mapname, map7.datamap));
                //GroupMaps.Add(new GroupMap(map8.mapname, map8.datamap));
                //GroupMaps.Add(new GroupMap(map9.mapname, map9.datamap));
                GroupMaps.Add(new GroupMap(map10.mapname, map10.datamap));
            }
            catch (Exception ex)
            {
                GameScr.info1.addInfo(ex.Message, 0);
            }
            RemoveMapsHomeInGroupMaps();
        }

        private void RemoveMapsHomeInGroupMaps()
        {
            int cgender = Char.myCharz().cgender;
            foreach (GroupMap groupMap in GroupMaps)
            {
                bool flag = cgender != 0;
                if (flag)
                {
                    bool flag2 = cgender != 1;
                    if (flag2)
                    {
                        groupMap.IdMaps.Remove(21);
                        groupMap.IdMaps.Remove(22);
                    }
                    else
                    {
                        groupMap.IdMaps.Remove(21);
                        groupMap.IdMaps.Remove(23);
                    }
                }
                else
                {
                    groupMap.IdMaps.Remove(22);
                    groupMap.IdMaps.Remove(23);
                }
            }
        }

        private void LoadLinkMapCapsule()
        {
            AddKeyLinkMaps(TileMap.mapID);
            string[] mapNames = GameCanvas.panel.mapNames;
            for (int i = 0; i < mapNames.Length; i++)
            {
                int idMapFromName = GetIdMapFromName(mapNames[i]);
                bool flag = idMapFromName != -1;
                if (flag)
                {
                    int[] info = new int[]
                    {
                        i
                    };
                    MyLinkMaps[TileMap.mapID].Add(new MapNext(idMapFromName, TypeMapNext.Capsule, info));
                }
            }
        }

        private void LoadLinkMapBase()
        {
            MyLinkMaps = new Dictionary<int, List<MapNext>>();
            LoadLinkMapsFromFile();
            LoadLinkMapsAutoWaypointFromFile();
            LoadLinkMapsHome();
            LoadLinkMapSieuThi();
            LoadLinkMapToCold();
        }

        public string[] DataXMap = new string[]
        {
            "24 25 1 10 0",
            "25 24 1 11 0",
            "45 48 1 19 3",
            "48 45 1 20 3 0",
            "48 50 1 20 3 1",
            "50 48 1 44 0",
            "24 26 1 10 1",
            "26 24 1 12 0",
            "25 26 1 11 1",
            "26 25 1 12 1",
            "24 84 1 10 2",
            "25 84 1 11 2",
            "26 84 1 12 2",
            "19 68 1 12 1",
            "68 19 1 12 0",
            "80 131 1 60 0",
            "27 102 1 38 1",
            "28 102 1 38 1",
            "29 102 1 38 1",
            "102 24 1 38 1",
            "27 53 1 25 0",
            "52 129 1 23 3",
            "0 149 1 67 3 0",
            "45 48 1 19 3",
            "50 48 1 44 0",
            "139 25 1 63 1",
            "139 26 1 63 2",
            "24 139 1 63 0",
            "139 24 1 63 0",
            "19 126 1 53 0",
            "126 19 1 53 0",
            "24 139 1 63 0",
            "139 24 1 63 0",
            "19 126 1 53 0",
            "126 19 1 53 0",
            "0 122 1 49 0",
            "80 160 1 60 0"
        };

        private void LoadLinkMapsFromFile()
        {

            try
            {
                for (int i = 0; i < DataXMap.Length; i++)
                {
                    string text;
                    bool flag = (text = DataXMap[i]) == null;
                    if (!flag)
                    {
                        text = text.Trim();
                        bool flag2 = !text.StartsWith("#") && !text.Equals("");
                        if (flag2)
                        {
                            int[] array = Array.ConvertAll(text.Split(new char[]
                            {
                                ' '
                            }), (string s) => int.Parse(s));
                            int num = array.Length - 3;
                            int[] array2 = new int[num];
                            Array.Copy(array, 3, array2, 0, num);
                            LoadLinkMap(array[0], array[1], (TypeMapNext)array[2], array2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GameScr.info1.addInfo(ex.Message, 0);
            }
        }

        public string[] DataWayPoint = new string[]
{
            "42 0 1 2 3 4 5 6",
            "3 27 28 29 30",
            "2 24",
            "1 47 46 45",
            "5 29",
            "47 111",
            "53 58 59 60 61 62 55 56 54 57",
            "53 27",
            "43 7 8 9 11 12 13 10",
            "11 31 32 33 34",
            "9 25",
            "13 33",
            "52 44 14 15 16 17 18 20 19",
            "17 35 36 37 38",
            "16 26",
            "20 37",
            "68 69 70 71 72 64 65 63 66 67 73 74 75 76 77 81 82 83 79 80",
            "102 92 93 94 96 97 98 99 100 103",
            "109 108 107 110 106",
            "109 105",
            "109 106",
            "106 107",
            "108 105",
            "131 132 133",
            "80 105",
            "160 161 162 163",
            "139 140",
            "149 147 152 151 148",
            "122 123 124"
};
        private void LoadLinkMapsAutoWaypointFromFile()
        {
            try
            {
                for (int z = 0; z < DataWayPoint.Length; z++)
                {
                    string text;
                    if ((text = DataWayPoint[z]) != null)
                    {
                        text = text.Trim();
                        if (!text.StartsWith("#") && !text.Equals(""))
                        {
                            int[] array = Array.ConvertAll(text.Split(new char[]
                            {
                                ' '
                            }), (string s) => int.Parse(s));
                            for (int i = 0; i < array.Length; i++)
                            {
                                if (i != 0)
                                {
                                    LoadLinkMap(array[i], array[i - 1], TypeMapNext.AutoWaypoint, null);
                                }
                                if (i != array.Length - 1)
                                {
                                    LoadLinkMap(array[i], array[i + 1], TypeMapNext.AutoWaypoint, null);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GameScr.info1.addInfo(ex.Message, 0);
            }
        }

        private void LoadLinkMapsHome()
        {
            int cgender = Char.myCharz().cgender;
            int num = 21 + cgender;
            int num2 = 7 * cgender;
            LoadLinkMap(num2, num, TypeMapNext.AutoWaypoint, null);
            LoadLinkMap(num, num2, TypeMapNext.AutoWaypoint, null);
        }

        private void LoadLinkMapSieuThi()
        {
            int cgender = Char.myCharz().cgender;
            int idMapNext = 24 + cgender;
            int[] array = new int[2];
            array[0] = 10;
            int[] info = array;
            LoadLinkMap(84, idMapNext, TypeMapNext.NpcMenu, info);
        }

        private void LoadLinkMapToCold()
        {
            bool flag = Char.myCharz().taskMaint.taskId > 30;
            if (flag)
            {
                int[] array = new int[2];
                array[0] = 12;
                int[] info = array;
                LoadLinkMap(19, 109, TypeMapNext.NpcMenu, info);
            }
        }

        public List<MapNext> GetMapNexts(int idMap)
        {
            List<MapNext> result;
            if (CanGetMapNexts(idMap))
            {
                result = MyLinkMaps[idMap];
            }
            else
            {
                result = null;
            }
            return result;
        }

        public bool CanGetMapNexts(int idMap)
        {
            return MyLinkMaps.ContainsKey(idMap);
        }

        private void LoadLinkMap(int idMapStart, int idMapNext, TypeMapNext type, int[] info)
        {
            AddKeyLinkMaps(idMapStart);
            MapNext item = new(idMapNext, type, info);
            MyLinkMaps[idMapStart].Add(item);
        }

        private void AddKeyLinkMaps(int idMap)
        {
            bool flag = !MyLinkMaps.ContainsKey(idMap);
            if (flag)
            {
                MyLinkMaps.Add(idMap, new List<MapNext>());
            }
        }

        private bool IsWaitInfoMapTrans()
        {
            return !AutoXmap.IsShowPanelMapTrans;
        }

        public static int GetIdMapFromPanelXmap(string mapName)
        {
            return int.Parse(mapName.Split(new char[]
            {
                '['
            })[1].Trim(']'));
        }

        public static Waypoint FindWaypoint(int idMap)
        {
            int i = 0;
            while (i < TileMap.vGo.size())
            {
                Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
                bool flag = XmapController.IdMapEnd == 124 && TileMap.mapID == 123;
                if (flag)
                {
                    for (int j = 0; j < TileMap.vGo.size(); j++)
                    {
                        Waypoint result = (Waypoint)TileMap.vGo.elementAt(j);
                        bool flag2 = j == TileMap.vGo.size() - 1;
                        if (flag2)
                        {
                            return result;
                        }
                    }
                }
                bool flag3 = GetTextPopup(waypoint.popup).Trim().ToLower().Equals(XmapController.GetMapName(idMap).Trim().ToLower());
                if (!flag3)
                {
                    i++;
                    continue;
                }
                return waypoint;
            }
            return null;
        }

        public static int GetPosWaypointX(Waypoint waypoint)
        {
            bool flag = waypoint.maxX < 60;
            int result;
            if (flag)
            {
                result = 15;
            }
            else
            {
                bool flag2 = waypoint.minX > TileMap.pxw - 60;
                if (flag2)
                {
                    result = TileMap.pxw - 15;
                }
                else
                {
                    result = waypoint.minX + 30;
                }
            }
            return result;
        }

        public static int GetPosWaypointY(Waypoint waypoint)
        {
            return waypoint.maxY;
        }

        public static bool IsMyCharDie()
        {
            return Char.myCharz().statusMe == 14 || Char.myCharz().cHP <= 0;
        }

        public static bool CanNextMap()
        {
            return !Char.isLoadingMap && !Char.ischangingMap && !Controller.isStopReadMessage;
        }

        private static int GetIdMapFromName(string mapName)
        {
            int cgender = Char.myCharz().cgender;
            bool flag = mapName.Equals("Về nhà");
            int result;
            if (flag)
            {
                result = 21 + cgender;
            }
            else
            {
                bool flag2 = mapName.Equals("Trạm tàu vũ trụ");
                if (flag2)
                {
                    result = 24 + cgender;
                }
                else
                {
                    bool flag3 = mapName.Contains("Về chỗ cũ: ");
                    if (flag3)
                    {
                        mapName = mapName.Replace("Về chỗ cũ: ", "");
                        bool flag4 = XmapController.GetMapName(AutoXmap.IdMapCapsuleReturn).Equals(mapName);
                        if (flag4)
                        {
                            return AutoXmap.IdMapCapsuleReturn;
                        }
                        bool flag5 = mapName.Equals("Rừng đá");
                        if (flag5)
                        {
                            return -1;
                        }
                    }
                    for (int i = 0; i < TileMap.mapNames.Length; i++)
                    {
                        bool flag6 = mapName.Equals(XmapController.GetMapName(i));
                        if (flag6)
                        {
                            return i;
                        }
                    }
                    result = -1;
                }
            }
            return result;
        }

        public static string GetTextPopup(PopUp popUp)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < popUp.says.Length; i++)
            {
                stringBuilder.Append(popUp.says[i]);
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString().Trim();
        }

        private static bool CanUseCapsuleNormal()
        {
            return !IsMyCharDie() && AutoXmap.IsUseCapsuleNormal && HasItemCapsuleNormal();
        }

        private static bool HasItemCapsuleNormal()
        {
            Item[] arrItemBag = Char.myCharz().arrItemBag;
            for (int i = 0; i < arrItemBag.Length; i++)
            {
                bool flag = arrItemBag[i] != null && arrItemBag[i].template.id == 193;
                if (flag)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool CanUseCapsuleVip()
        {
            return !IsMyCharDie() && AutoXmap.IsUseCapsuleVip && HasItemCapsuleVip();
        }

        private static bool HasItemCapsuleVip()
        {
            Item[] arrItemBag = Char.myCharz().arrItemBag;
            for (int i = 0; i < arrItemBag.Length; i++)
            {
                bool flag = arrItemBag[i] != null && arrItemBag[i].template.id == 194;
                if (flag)
                {
                    return true;
                }
            }
            return false;
        }

        private const int ID_MAP_HOME_BASE = 21;

        private const int ID_MAP_TTVT_BASE = 24;

        private const int ID_ITEM_CAPSUAL_VIP = 194;

        private const int ID_ITEM_CAPSUAL_NORMAL = 193;

        private const int ID_MAP_TPVGT = 19;

        private const int ID_MAP_TO_COLD = 109;

        public List<GroupMap> GroupMaps;

        public Dictionary<int, List<MapNext>> MyLinkMaps;

        public bool IsLoading;

        private bool IsLoadingCapsule;

        private static XmapData Instance;
    }
}
