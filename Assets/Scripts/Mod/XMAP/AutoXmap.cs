using System;

namespace Mod.XMAP
{
    public class AutoXmap
    {
        public static bool Chat(string text)
        {
            bool flag = text == "xmp";
            if (flag)
            {
                bool isXmapRunning = IsXmapRunning;
                if (isXmapRunning)
                {
                    XmapController.FinishXmap();
                    GameScr.info1.addInfo("Đã hủy Xmap", 0);
                }
                else
                {
                    XmapController.ShowXmapMenu();
                }
            }
            else
            {
                bool flag2 = IsGetInfoChat<int>(text, "xmp");
                if (flag2)
                {
                    bool isXmapRunning2 = IsXmapRunning;
                    if (isXmapRunning2)
                    {
                        XmapController.FinishXmap();
                        GameScr.info1.addInfo("Đã hủy Xmap", 0);
                    }
                    else
                    {
                        XmapController.StartRunToMapId(GetInfoChat<int>(text, "xmp"));
                    }
                }
                else
                {
                    bool flag3 = text == "csb";
                    if (flag3)
                    {
                        IsUseCapsuleNormal = !IsUseCapsuleNormal;
                        GameScr.info1.addInfo("Sử dụng capsule thường Xmap: " + (IsUseCapsuleNormal ? "Bật" : "Tắt"), 0);
                    }
                    else
                    {
                        bool flag4 = !(text == "csdb");
                        if (flag4)
                        {
                            return false;
                        }
                        IsUseCapsuleVip = !IsUseCapsuleVip;
                        GameScr.info1.addInfo("Sử dụng capsule đặc biệt Xmap: " + (IsUseCapsuleVip ? "Bật" : "Tắt"), 0);
                    }
                }
            }
            return true;
        }

        public static bool HotKeys()
        {
            int keyAsciiPress = GameCanvas.keyAsciiPress;
            if (keyAsciiPress != 99)
            {
                if (keyAsciiPress != 120)
                {
                    return false;
                }
                Chat("xmp");
            }
            else
            {
                Chat("csb");
            }
            return true;
        }

        public static void Update()
        {
            bool isLoading = XmapData.GI().IsLoading;
            if (isLoading)
            {
                XmapData.GI().Update();
            }
            if (IsXmapRunning)
            {
                XmapController.Update();
            }
        }

        public static void Info(string text)
        {
            bool flag = text.Equals("Bạn chưa thể đến khu vực này");
            if (flag)
            {
                XmapController.FinishXmap();
            }
            bool flag2 = (text.ToLower().Contains("chức năng bảo vệ") || text.ToLower().Contains("đã hủy xmap")) && IsXmapRunning;
            if (flag2)
            {
                XmapController.FinishXmap();
            }
        }

        public static bool XoaTauBay(object obj)
        {
            Teleport teleport = (Teleport)obj;
            bool isMe = teleport.isMe;
            bool result;
            if (isMe)
            {
                Char.myCharz().isTeleport = false;
                bool flag = teleport.type == 0;
                if (flag)
                {
                    Controller.isStopReadMessage = false;
                    Char.ischangingMap = true;
                }
                Teleport.vTeleport.removeElement(teleport);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public static void SelectMapTrans(int selected)
        {
            if (IsMapTransAsXmap)
            {
                XmapController.HideInfoDlg();
                XmapController.StartRunToMapId(XmapData.GetIdMapFromPanelXmap(GameCanvas.panel.mapNames[selected]));
            }
            else
            {
                XmapController.SaveIdMapCapsuleReturn();
                Service.gI().requestMapSelect(selected);
            }
        }

        public static void ShowPanelMapTrans()
        {
            IsMapTransAsXmap = false;
            if (IsShowPanelMapTrans)
            {
                GameCanvas.panel.setTypeMapTrans();
                GameCanvas.panel.show();
            }
            else
            {
                IsShowPanelMapTrans = true;
            }
        }

        public static void FixBlackScreen()
        {
            Controller.gI().loadCurrMap(0);
            Service.gI().finishLoadMap();
            Char.isLoadingMap = false;
        }

        private static bool IsGetInfoChat<T>(string text, string s)
        {
            bool flag = text.StartsWith(s);
            bool result;
            if (flag)
            {
                try
                {
                    Convert.ChangeType(text[s.Length..], typeof(T));
                }
                catch
                {
                    return false;
                }
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private static T GetInfoChat<T>(string text, string s)
        {
            return (T)Convert.ChangeType(text[s.Length..], typeof(T));
        }

        public static bool IsXmapRunning = false;

        public static bool IsMapTransAsXmap = false;

        public static bool IsShowPanelMapTrans = true;

        public static bool IsUseCapsuleNormal = true;

        public static bool IsUseCapsuleVip = true;

        public static int IdMapCapsuleReturn = -1;
    }
}
