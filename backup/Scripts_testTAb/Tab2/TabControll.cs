namespace Tab2
{
    using System.Linq;
    using UnityEngine.SceneManagement;

    public class TabControll : mScreen
    {
        private static TabControll _Instance;
        public static TabControll Instance => _Instance ?? (_Instance = new TabControll());

        public TabControll()
        {
            initCommand();
        }

        private static bool _selectTab;

        public static bool selectTab
        {
            get => _selectTab;
            set => _selectTab = value;
        }

        private static bool _isShow = true;

        public static bool isShow
        {
            get => _isShow;
            set => _isShow = value;
        }

        private static sbyte tabIndex = 0;
        private static TabCommand firstCommand = new TabCommand("Tab", () => showTabSelect());

        private static TabCommand[] TransferTab = Enumerable.Range(0, 6)
              .Select(i => new TabCommand((i + 1).ToString(), () => TransferTabIndex((sbyte)(i - 1))))
              .ToArray();

        private static string[] SceneNames = new string[]
        {
            "NROL",
            "NRO2",
            "NRO3",
            "NRO4",
            "NRO5",
            "NRO6"
        };
        private static TabType[] tabTypes = new TabType[]
        {
            TabType.Tab1,
            TabType.Tab2, TabType.Tab3,
            TabType.Tab4, TabType.Tab5, TabType.Tab6
        };
        private static void initCommand()
        {
            firstCommand.x = GameCanvas.w - 60;
            firstCommand.y = 0;
            for (int i = 0; i < TransferTab.Length; i++)
            {
                TransferTab[i].x = GameCanvas.w - 210  + i * 25;
                TransferTab[i].y = 0;
            }
        }
        public override void paint(mGraphics g)
        {
            if (!isShow) return;
            firstCommand.paint(g);
            paintTab(g);
            int currentTabIndex = -1;
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            for (int i = 0; i < SceneNames.Length; i++)
            {
                if (SceneNames[i] == currentScene)
                {
                    currentTabIndex = i;
                    break;
                }
            }
            mFont.tahoma_7b_red.drawString(g, "Tab " + (currentTabIndex + 1), firstCommand.x + 15 - 220, firstCommand.y + 35, 3);
            base.paint(g);
        }
        private void paintTab(mGraphics g)
        {
            if (!selectTab) return;
            foreach (var cmd in TransferTab)
            {
                cmd.paint(g);
            }
        }
        private static void TransferTabIndex(sbyte index)
        {
            tabIndex = (sbyte)(index + 1);

            // Reset game state khi chuyển tab để tránh conflict
            if (TabManagement.tab != TabType.Tab2)
            {
                GameScr.resetAllvector();
                mSystem.gcc();
            }

            SceneManager.LoadScene(SceneNames[index + 1]);
            TabManagement.tab = tabTypes[index + 1];
            _selectTab = false;
        }
        private static void showTabSelect()
        {
            _selectTab = !_selectTab;
        }
        public bool isPointerHoldInTab()
        {
            if (!isShow)
                return false;
            if (firstCommand.isPointerInside())
            {
                firstCommand.Invoke();
                return true;
            }
            if (selectTab)
            {
                foreach (var cmd in TransferTab)
                {
                    if (cmd.isPointerInside())
                    {
                        cmd.Invoke();
                        return true;
                    }
                }
            }
            return false;
        }
        public override void updateKey()
        {
            base.updateKey();
        }
    }
}
