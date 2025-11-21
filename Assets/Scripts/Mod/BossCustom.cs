using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Mod
{
    public class BossCustom
    {
        // Token: 0x060001C1 RID: 449 RVA: 0x00008972 File Offset: 0x00006B72
        public BossCustom()
        {
            this.bosses = new Dictionary<string, List<CustomBossNotification>>();
        }

        // Token: 0x060001C2 RID: 450 RVA: 0x00021274 File Offset: 0x0001F474
        public void addBoss(string chatvip)
        {
            if (chatvip.Contains("tiêu diệt"))
            {
                this.getBossInfo(chatvip);
            }
            else if (chatvip.StartsWith("BOSS") || chatvip.StartsWith("Boss"))
            {
                this.getBossXuatHien(chatvip);
            }
            if (GameCanvas.panel.isShow)
            {
                if (GameCanvas.panel.type == 32)
                {
                    GameCanvas.panel.setTabBossNotification();
                    return;
                }
                if (GameCanvas.panel.type == 33)
                {
                    GameCanvas.panel.setTabInfoBoss();
                }
            }
        }

        // Token: 0x060001C3 RID: 451
        private string getBossesNof(string a)
        {
            if (a.StartsWith("BOSS "))
            {
                a = "BOSS ";
            }
            else if (a.StartsWith("Boss "))
            {
                a = "Boss ";
            }
            return a;
        }
        private void getBossXuatHien(string string_0)
        {
            string[] array = ((string_0.Replace(getBossesNof(string_0), "").Replace(" vừa xuất hiện tại ", "|").Replace(" appear at ", "|"))).Split(new char[]
            {
            '|'
            });
            CustomBossNotification customBossNotification = new CustomBossNotification
            {
                name = array[0].Trim(),
                map = array[1].Trim(),
                player = "",
                mapId = this.getMapID(array[0]),
                timeStart = new DateTime?(DateTime.Now)
            };
            if (!this.bosses.ContainsKey(customBossNotification.name))
            {
                this.bosses.Add(customBossNotification.name, new List<CustomBossNotification>());
            }
            this.bosses[customBossNotification.name].Insert(0, customBossNotification);
        }

        // Token: 0x060001C4 RID: 452
        private void getBossInfo(string string_0)
        {
            string[] array = string_0.Replace(": Đã tiêu diệt được ", "|").Replace(" mọi người đều ngưỡng mộ.", "").Split(new char[]
            {
            '|'
            });
            if (!this.bosses.ContainsKey(array[1].Trim()))
            {
                this.bosses.Add(array[1].Trim(), new List<CustomBossNotification>());
            }
            List<CustomBossNotification> list = this.bosses[array[1].Trim()];
            if (list.Count == 0)
            {
                list.Add(new CustomBossNotification
                {
                    player = array[0].Trim(),
                    name = array[1].Trim(),
                    timeEnd = new DateTime?(DateTime.Now)
                });
                return;
            }
            list[0].player = array[0].Trim();
            list[0].name = array[1].Trim();
            list[0].timeEnd = new DateTime?(DateTime.Now);
        }

        // Token: 0x060001C5 RID: 453
        private int getMapID(string string_0)
        {
            for (int i = 0; i < TileMap.mapNames.Length; i++)
            {
                if (TileMap.mapNames[i].Equals(string_0))
                {
                    return i;
                }
            }
            return -1;
        }

        // Token: 0x040001FA RID: 506
        public static BossCustom getBossCustom = new BossCustom();

        // Token: 0x040001FB RID: 507
        public Dictionary<string, List<CustomBossNotification>> bosses;
    }
}