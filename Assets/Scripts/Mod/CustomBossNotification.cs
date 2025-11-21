using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Mod
{
    // Token: 0x0200001E RID: 30
    public class CustomBossNotification
    {
        // Token: 0x060001BB RID: 443 RVA: 0x000088DE File Offset: 0x00006ADE
        public CustomBossNotification()
        {
            this.timeStart = null;
            this.timeEnd = null;
        }

        // Token: 0x060001BC RID: 444
        public string getTimeStartBoss()
        {
            if (this.timeStart == null)
            {
                return "Chưa có thông tin";
            }
            TimeSpan timeSpan = DateTime.Now.Subtract(this.timeStart.Value);
            int num = (int)timeSpan.TotalSeconds;
            return string.Concat(new string[]
            {
            this.timeStart.Value.ToString("HH"),
            "h:",
            this.timeStart.Value.ToString("mm"),
            " (",
            (num < 60) ? (num.ToString() + "s") : (timeSpan.Minutes.ToString() + "ph"),
            " trước)"
            });
        }

        // Token: 0x060001BD RID: 445
        public string getMapBoss()
        {
            if (this.map != null && !(this.map == ""))
            {
                return this.map;
            }
            return "Chưa có thông tin";
        }

        // Token: 0x060001BE RID: 446
        public string getTimeBossDie()
        {
            if (this.timeEnd == null)
            {
                return "Chưa có thông tin";
            }
            TimeSpan timeSpan = DateTime.Now.Subtract(this.timeEnd.Value);
            int num = (int)timeSpan.TotalSeconds;
            return string.Concat(new string[]
            {
            this.timeEnd.Value.ToString("HH"),
            "h:",
            this.timeEnd.Value.ToString("mm"),
            " (",
            (num < 60) ? (num.ToString() + "s") : (timeSpan.Minutes.ToString() + "ph"),
            " trước)"
            });
        }

        // Token: 0x060001BF RID: 447
        public string getBossKiller()
        {
            if (this.player != null && !(this.player == ""))
            {
                return this.player;
            }
            return "Chưa có thông tin";
        }

        // Token: 0x040001F4 RID: 500
        public string name;

        // Token: 0x040001F5 RID: 501
        public string map;

        // Token: 0x040001F6 RID: 502
        public int mapId;

        // Token: 0x040001F7 RID: 503
        public DateTime? timeStart;

        // Token: 0x040001F8 RID: 504
        public DateTime? timeEnd;

        // Token: 0x040001F9 RID: 505
        public string player;
    }
}