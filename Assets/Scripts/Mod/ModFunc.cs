using JetBrains.Annotations;
using Mod;
using Mod.XMAP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Networking;

public class ModFunc : IActionListener
{
    private static readonly ModFunc Instance = new();

    public static string homeUrl = "Mod Build By EMTI";

    // public static string ipString = "74-55-43-5C-16-08-0C-08-02-17-04-0B-03-17-04-09-05-17-04-0D-04-03-05-0E-05-0F-0C-09-1A-7B-5A-4C-53-19-04-03-07-0D-18-0B-04-0C-18-0B-06-0A-18-0B-02-0B-0C-0A-01-0A-01-03-06-15-74-55-43-5C-16-0A-0C-08-02-17-04-0B-03-17-04-09-05-17-04-0D-04-03-05-0E-05-01-0C-09-1A-7B-5A-4C-53-19-62-7C-65-6D-0C-08-02-17-04-0B-03-17-04-09-05-17-04-0D-04-03-05-01-03-00-0C-09-1A-09-1A-09";

    /*  public static string ipString = Decrypt("勂勞勩劢勋勄劲勩勀勛劶务劽勛劺勪势勄劥勪劾劵劾勦劾勛勂勪勀勛劾劤劾勫勊劧劾労勩労勓効勇勝劺劵劺劧劾勅勂勦劾勛劺劢劽勛劺勨劾勪劥勪势劵劺劧劾勫勔勫势勫勠勨劽劶劻勤動勈勆勘劾勫勠勩势労劥勪劾勛勆勦劾勛劲勫劽勛劺务劾勛勠勫势勫劾劥勀勛劲勤勂勞勩劢勋勄劳勆勃勇势勆勀勛劶务劽勛劺勪势勄劥勪劾劵劾勦劾勛勂勪勀勛劾劥势勅勜劧劾労勨勨劽劵劲劮", 742001);
      //protected static string IP = "Ngọc Rồng Hà Nội:14.225.213.148:8888:0,0,0";
      public static string Decrypt(string encryptedText, int key)
      {
          StringBuilder stringBuilder = new StringBuilder();
          foreach (char c in encryptedText)
          {
              stringBuilder.Append((char)(c - key));
          }

          byte[] bytes = Convert.FromBase64String(stringBuilder.ToString());
          return Encoding.UTF8.GetString(bytes);
      }*/

    // For debug only
    private static bool isDebugEnable = false;

    private static long lastTimeLog = 0;

    public bool canUpdate = false;

    private long lastAutoAttack = 0;

    private readonly List<Skill> listSkillsAuto = new();

    public List<ItemAuto> listItemAuto = new();

    public static bool notifBoss = false;

    private bool lineToBoss;

    private bool focusBoss = false;

    private long lastFocusBoss;

    public static MyVector bossNotif = new();

    private long lastUpdateZones = 0;

    public bool isUpdateZones = false;

    public bool userOpenZones = false;

    public bool showCharsInMap = true;

    public MyVector charsInMap = new();

    public bool showInfoMap = false;

    public bool showInfoMe = false;

    private long lastUpdateInfoMe = 0;

    public bool autoAttack = false;

    public bool autoWakeUp = false;

    public long lastAutoWakeUp = 0;

    public bool isAutoPhaLe = false;

    public Item itemPhale;

    public int maxPhale = -1;

    public int currPhale = -1;

    public static bool isAutoLogin = false;

    public static AutoLogin autoLogin;

    public bool isAutoVQMM = false;

    public bool isOpenThuongDe = false;

    public bool isCollectAll = false;

    public bool isPaintThuongDe = false;

    public long lastVQMM;

    public bool isShowButton = true;

    public bool isIntroOff = false;

    public bool isHighFps;

    private int paramIntrinsic = -1;

    public string curSelectIntrinsic = "";

    public static string strAddAutoItem = "Thêm vào\nAutoItem";

    public static string strRemoveAutoItem = "Xoá khỏi\nAutoItem";

    public static string strTeleportTo = "Dịch\nchuyển tới";

    public static string strAutoBuy = "Mua 20 lần";

    public static string strChooseIntrinsic = "Chọn chỉ số";

    public static string strInCrease = "Tăng\ntới\nmức";

    public static string[] strPointTypes = { "HP", "MP", "Sức Đánh", "Giáp", "Chí mạng" };

    public static string strAccManager = "Q.L.T.K";

    public static string strModFunc = "Chức Năng MOD";

    public static string strUpdateZones = "Cập Nhật Khu";

    public static string strCharsInMap = "Nhân Vật Trong Khu";

    public static string strInfoMe = "Thông Tin Bản Thân";

    public static string strAutoPhaLe = "Tự Động Pha Lê Hóa";

    public static string strAutoVQMM = "Tự Động VQMM";

    public static string strAutoWakeUp = "Tự Động Hồi Sinh";

    public static string strAutoLogin = "Tự Động Đăng Nhập";

    public static string strShowButton = "Hiện Nút Trợ Năng";

    public static string strIntroOff = "Tắt Intro";

    public static string strHighFps = "FPS Cao";

    public static string strClickToChat = " [Ấn để chat]";

    public static string strPlayerInfo = "Thông tin player";

    public static string strPet2 = "Người iuu";

    public static string strUseForPet2 = "Sử dụng\ncho\nNg.iuu";

    public MyVector listNotifTichXanh = new();

    private bool startChat = false;

    private int xNotif;

    private long lastUpdateNotif;

    public bool isPeanPet = false;

    private long lastPeanPet = 0;

    public static int indexAutoPoint = -1;

    public static int pointIncrease = 0;

    public static bool autoPointForPet = false;

    private static int modKeyPosX;

    private static int modKeyPosY;

    public static Command cmdAccManager;

    public static bool isOpenAccMAnager = false;

    public static List<Account> accounts = new();

    public List<Command> cmdsChooseAcc = new();

    public List<Command> cmdsDelAcc = new();

    public static Command cmdCloseAccManager;

    public static bool startAutoItem = false;

    private static bool isAutoChat = false;

    private static string textAutoChat = string.Empty;

    private static bool isAutoChatTG = false;

    private static string textAutoChatTG = string.Empty;

    private long lastAutoChat = 0;

    private long lastAutoChatTG = 0;

    public static string ipServer = "Đổi IP";

    public static bool userOpenPet = false;

    public static bool isLockFocus = false;

    private long lastUpdateFPS;

    private static int lastFps;

    public static Image[] ticks = new Image[20];
    private static Image logo = new Image();
    private static Image[] logos = new Image[60];

    public static Image imgLogoBig = null;

    public static Image imgBg = null;

    public static bool isReadInt = false;

    public static int musicCount = 0;

    public static bool loadedMusic = false;

    public static bool isPlayingMusic = false;

    public static List<AudioClip> musics = new();

    public static ModFunc GI()
    {
        return Instance ?? new();
    }

    public static void DoEncodeIp()
    {
        //Debug.Log(EncodeStringToByteArrayString("Blue 1:14.225.203.242:3736:0,Blue 2:14.225.203.242:3737:0,Blue 3:14.225.203.242:3738:0,Blue TEST:14.225.203.242:3859:0,0,0", "69"));
    }

    public void OpenMenu()
    {
        MyVector myVector = new();
        myVector.addElement(new Command("Bản đồ", 883)); // Xong
        myVector.addElement(new Command("Luyện tập", 45)); // Xong
        myVector.addElement(new Command("Nhặt đồ", 89));
        //myVector.addElement(new Command("Đậu thần", 14));
        myVector.addElement(new Command("Đệ tử", 16));
        myVector.addElement(new Command("BOSS", 32)); // Xong
        //myVector.addElement(new Command(Mod.doHoa, 15));
        //myVector.addElement(new Command(Mod.upYardrat, 31));
        myVector.addElement(new Command("Khác", 53));
        GameCanvas.menu.startAt(myVector, 4);
    }

    public bool UpdateKey(int key)
    {
        switch (key)
        {
            case 'z':
                NewBagUI.GI().OpenBag();
                break;
            case 'a':
                // LEFT
                MoveTo(Char.myCharz().cx - 100, Char.myCharz().cy);
                break;
            case 'w':
                // UP
                MoveTo(Char.myCharz().cx, Char.myCharz().cy - 100);
                break;
            case 's':
                // DOWN
                MoveTo(Char.myCharz().cx, Char.myCharz().cy + 100);
                break;
            case 'd':
                // RIGHT
                MoveTo(Char.myCharz().cx + 100, Char.myCharz().cy);
                break;
            case 'e':
                // Friend
                Service.gI().friend(0, -1);
                InfoDlg.showWait();
                break;
            case 'h':
                // Update khu
                GameScr.gI().onChatFromMe("ukhu", string.Empty);
                break;
            case 'u':
                // Auto attack
                perform(42, null);
                break;
            case 'x':
                OpenMenu();
                break;
            case 'f':
                UsePorata();
                break;
            case 'c':
                UseItem(194);
                break;
            case 'm':
                userOpenZones = true;
                Service.gI().openUIZone();
                break;
            case 't':
                UseItem(521);
                break;
            case 'n':
                PickMob.IsAutoPickItems = !PickMob.IsAutoPickItems;
                GameScr.info1.addInfo("Tự động nhặt: " + (PickMob.IsAutoPickItems ? "Bật" : "Tắt"), 0);
                break;
            case 'j':
                ManualXmap.GI().LoadMapLeft();
                break;
            case 'k':
                ManualXmap.GI().LoadMapCenter();
                break;
            case 'l':
                ManualXmap.GI().LoadMapRight();
                break;
            case 'g':
                if (Char.myCharz().charFocus != null)
                {
                    Service.gI().giaodich(0, Char.myCharz().charFocus.charID, -1, -1);
                    GameScr.info1.addInfo("Đã gửi lời mời giao dịch đến " + Char.myCharz().charFocus.cName, 0);
                }
                break;
            default:
                return false;
        }
        return true;
    }

    public void LoadGame()
    {
        if (!loadedMusic)
        {
            InitMusic();
            loadedMusic = true;
        }
        Time.timeScale = 1.5f;
        listSkillsAuto.Clear();
        listItemAuto.Clear();
        int value = Rms.loadRMSInt("isHighFps");
        isHighFps = value == -1 || value == 1;
        ChangeFPSTarget();
    }

    public void MoveTo(int x, int y)
    {
        Char.myCharz().cx = x;
        Char.myCharz().cy = y;
        Service.gI().charMove();
        Char.myCharz().cx = x;
        Char.myCharz().cy = y + 1;
        Service.gI().charMove();
        Char.myCharz().cx = x;
        Char.myCharz().cy = y;
        Service.gI().charMove();
    }

    public void GotoNpc(int npcID)
    {
        for (int i = 0; i < GameScr.vNpc.size(); i++)
        {
            Npc npc = (Npc)GameScr.vNpc.elementAt(i);
            if (npc.template.npcTemplateId == npcID && Math.abs(npc.cx - Char.myCharz().cx) >= 50)
            {
                MoveTo(npc.cx, npc.cy - 1);
                Char.myCharz().FocusManualTo(npc);
                return;
            }
        }
    }

    public int FindItemIndex(int idItem)
    {
        if (Char.myCharz().arrItemBag == null)
        {
            return -1;
        }
        for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
        {
            if (Char.myCharz().arrItemBag[i] != null && Char.myCharz().arrItemBag[i].template.id == idItem)
            {
                return Char.myCharz().arrItemBag[i].indexUI;
            }
        }
        return -1;
    }

    private void AttackChar()
    {
        try
        {
            MyVector myVector = new();
            myVector.addElement(Char.myCharz().charFocus);
            Service.gI().sendPlayerAttack(new MyVector(), myVector, 2);
        }
        catch
        {
        }
    }

    public void AttackMob(Mob mob)
    {
        try
        {
            MyVector myVector = new();
            myVector.addElement(mob);
            Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
        }
        catch
        {
        }
    }

    public void AutoAttack()
    {
        Char @char = Char.myCharz();
        if (!Char.isLoadingMap
            && !@char.stone
            && !@char.meDead
            && @char.statusMe != 14
            && @char.statusMe != 5
            && @char.myskill.template.type == 1
            && @char.myskill.template.id != 10
            && @char.myskill.template.id != 11
            && !@char.myskill.paintCanNotUseSkill)
        {
            if (mSystem.currentTimeMillis() - lastAutoAttack > 500)
            {
                if (GameScr.gI().isMeCanAttackMob(@char.mobFocus) && Res.abs(@char.mobFocus.xFirst - @char.cx) < @char.myskill.dx * 2)
                {
                    AttackMob(@char.mobFocus);
                    SetUsedSkill(@char.myskill);
                }
                else if (@char.isMeCanAttackOtherPlayer(@char.charFocus) && Res.abs(@char.charFocus.cx - @char.cx) < @char.myskill.dx * 2)
                {
                    AttackChar();
                    SetUsedSkill(@char.myskill);
                }
                lastAutoAttack = mSystem.currentTimeMillis();
            }
        }
    }

    public void SetUsedSkill(Skill skill)
    {
        skill.paintCanNotUseSkill = true;
        skill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
    }

    public void UsePorata()
    {
        int[] ids = new int[] { 454, 921, 2064, 2113, 1324 };
        foreach (int num in ids)
        {
            int index = FindItemIndex(num);
            if (index != -1)
            {
                Service.gI().useItem(0, 1, (sbyte)index, -1);
                Service.gI().petStatus(3);
                return;
            }
        }
        GameScr.info1.addInfo("Bạn không có bông tai", 0);
    }

    public void AutoFocusBoss()
    {
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char @char = (Char)GameScr.vCharInMap.elementAt(i);
            if (@char != null && @char.charID < 0 && @char.cTypePk == 5 && !@char.cName.StartsWith("Đ"))
            {
                Char.myCharz().FocusManualTo(@char);
                return;
            }
        }
    }

    public int GetMapID(string mapName)
    {
        int result = -1;
        for (int i = 0; i < XmapController.mapNames.Length; i++)
        {
            if (XmapController.mapNames[i].Trim().ToLower().Equals(mapName.Trim().ToLower()))
            {
                result = i;
            }
        }
        return result;
    }

    private string CharGender(Char @char)
    {
        string result;
        if (@char.cTypePk == 5)
        {
            result = "BOSS";
        }
        else if (@char.cgender == 0)
        {
            result = "TĐ";
        }
        else if (@char.cgender == 1)
        {
            result = "NM";
        }
        else if (@char.cgender == 2)
        {
            result = "XD";
        }
        else
        {
            result = "";
        }
        return result;
    }

    public void UseItem(int itemId)
    {
        int index = FindItemIndex(itemId);
        if (index != -1)
        {
            Service.gI().useItem(0, 1, (sbyte)index, -1);
            return;
        }
        GameScr.info1.addInfo("Không tìm thấy vật phẩm", 0);
    }

    public void UseItemAuto()
    {
        if (!startAutoItem)
        {
            System.Threading.Tasks.Task.Delay(10000).ContinueWith(t => startAutoItem = true);
            return;
        }
        if (listItemAuto.Count > 0 && startAutoItem)
        {
            for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
            {
                Item item = Char.myCharz().arrItemBag[i];
                foreach (ItemAuto itemAuto in listItemAuto)
                {
                    if (item != null && item.template.iconID == itemAuto.iconID && item.template.id == itemAuto.id && !ItemTime.isExistItem(item.template.iconID))
                    {
                        Service.gI().useItem(0, 1, (sbyte)FindItemIndex(item.template.id), -1);
                        break;
                    }
                }
            }
        }
    }

    private void AutoHoiSinh()
    {
        if (Char.myCharz().cHP <= 0 || Char.myCharz().meDead || Char.myCharz().statusMe == 14)
        {
            Service.gI().wakeUpFromDead();
        }
    }

    //public static void AutoLogin()
    //{
    //    Thread.Sleep(1000);
    //    while (ServerListScreen.testConnect != 2)
    //    {
    //        GameCanvas.serverScreen.switchToMe();
    //        Thread.Sleep(1000);
    //    }
    //    GameCanvas.loginScr ??= new LoginScr();
    //    GameCanvas.loginScr.switchToMe();
    //    Thread.Sleep(500);

    //    Account account = FindAccWithUsername(accAutoLogin);
    //    if (account.getUsername().Length > 0)
    //    {
    //        Rms.saveRMSString("acc", account.getUsername());
    //        Rms.saveRMSString("pass", account.getPassword());
    //        if (GameCanvas.currentScreen == GameCanvas.loginScr)
    //        {
    //            GameCanvas.loginScr.setUserPass();
    //        }
    //    }

    //    Thread.Sleep(500);
    //    GameCanvas.loginScr.doLogin();
    //}

    public static int GetCurrPhaLe(Item item)
    {
        for (int i = 0; i < item.itemOption.Length; i++)
        {
            if (item.itemOption[i].optionTemplate.id == 107)
            {
                return item.itemOption[i].param;
            }
        }
        return 0;
    }

    public void AutoPhaLe()
    {
        while (isAutoPhaLe)
        {
            if (TileMap.mapID != 5)
            {
                GameScr.info1.addInfo("Cần đến Đảo Kame để sử dụng Tự động Pha lê hóa", 0);
                Thread.Sleep(500);
                break;
            }
            if (currPhale >= maxPhale && itemPhale != null && currPhale >= 0 && maxPhale > 0)
            {
                Sound.start(1f, Sound.l1);
                GameScr.info1.addInfo("Đã đạt đến số sao yêu cầu", 0);
                maxPhale = -1;
                itemPhale = null;
            }
            if (Char.myCharz().xu > 10000000000L)
            {
                GotoNpc(21);
                if (itemPhale != null && maxPhale > 0)
                {
                    while (!GameCanvas.menu.showMenu)
                    {
                        Service.gI().combine(1, GameCanvas.panel.vItemCombine);
                        Thread.Sleep(100);
                    }
                    Service.gI().confirmMenu(21, 0);
                    GameCanvas.menu.doCloseMenu();
                    GameCanvas.panel.currItem = null;
                    GameCanvas.panel.chatTField.isShow = false;
                }
            }
            else if (itemPhale != null)
            {
                BanVang();
            }
            Thread.Sleep(500);
        }
    }

    private void BanVang()
    {
        if (TileMap.mapID != 5)
        {
            GameScr.info1.addInfo("Cần đến Đảo Kame để Tự động bán vàng", 0);
            Thread.Sleep(1000);
            return;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            GameScr.info1.addInfo("Dừng bán vàng", 0);
            return;
        }
        while (Char.myCharz().xu <= 60000000000L && !Input.GetKey(KeyCode.Q))
        {
            if (FindItemIndex(457) == -1)
            {
                GameScr.info1.addInfo("Không tìm thấy thỏi vàng", 0);
                if (isAutoPhaLe)
                {
                    isAutoPhaLe = false;
                    GameScr.info1.addInfo("Vàng không đủ, đã tắt Tự động Pha lê hóa", 0);
                }
                return;
            }
            Service.gI().useItem(0, 1, (sbyte)FindItemIndex(457), -1);
            GameScr.info1.addInfo("Đang bán thỏi vàng", 0);
            Thread.Sleep(500);
        }
        GameScr.info1.addInfo("Đã bán xong", 0);
        Thread.Sleep(500);
    }

    public static Item FindItemBagWithIndexUI(int index)
    {
        foreach (Item item in Char.myCharz().arrItemBag)
        {
            if (item != null && item.indexUI == index)
            {
                return item;
            }
        }
        return null;
    }

    public void CollectAllThuongDe()
    {
        isCollectAll = true;
        Service.gI().openMenu(19);
        Service.gI().confirmMenu(19, 2);
        Service.gI().confirmMenu(19, 1);
        Service.gI().buyItem(2, 0, 0);
        Thread.Sleep(2000);
        isCollectAll = false;
    }

    private void OpenMenuThuongDe()
    {
        isOpenThuongDe = true;
        Service.gI().openMenu(19);
        Service.gI().confirmMenu(19, 2);
        Service.gI().confirmMenu(19, 0);
        isOpenThuongDe = false;
    }

    public void quayThuongDe()
    {
        if (!isCollectAll && !isOpenThuongDe)
        {
            if (!isPaintThuongDe && TileMap.mapID == 45)
            {
                OpenMenuThuongDe();
                return;
            }
            if (TileMap.mapID == 45)
            {
                if (Input.GetKey("q") || Char.myCharz().xu <= 200000000L)
                {
                    GameScr.info1.addInfo("Đã tắt Auto VQMM (2)", 0);
                    isAutoVQMM = false;
                    return;
                }
                Service.gI().openMenu(19);
                Service.gI().SendCrackBall(2, 7);
            }
        }
    }

    public bool Chat(string text)
    {
        if (text == "debug")
        {
            isDebugEnable = !isDebugEnable;
            GameScr.info1.addInfo("Debugging Mode: " + (isDebugEnable ? "ON" : "OFF"), 0);
            return true;
        }
        if (text == "loadskill")
        {
            perform(57, null);
            return true;
        }
        if (text == "ak")
        {
            perform(42, null);
            return true;
        }
        if (text == "ts")
        {
            perform(44, null);
            return true;
        }
        if (text == "tsnguoi")
        {
            perform(48, null);
            return true;
        }
        if (text == "vqmm")
        {
            isPaintThuongDe = false;
            isAutoVQMM = !isAutoVQMM;
            GameScr.info1.addInfo("Auto VQMM: " + (isAutoVQMM ? "Bật" : "Tắt"), 0);
            return true;
        }
        if (text == "ukhu")
        {
            isUpdateZones = !isUpdateZones;
            GameScr.info1.addInfo("Tự động cập nhật khu: " + (isUpdateZones ? "Bật" : "Tắt"), 0);
            return true;
        }
        // Start With
        if (text.StartsWith("k "))
        {
            bool success = int.TryParse(text.Replace("k ", ""), out int khu);
            if (success && khu >= 0)
            {
                Service.gI().requestChangeZone(khu, -1);
            }
            return true;
        }
        if (text.StartsWith("s "))
        {
            ChangeGameSpeed(text.Replace("s ", ""));
            return true;
        }
        if (text.StartsWith("atc "))
        {
            textAutoChat = text.Replace("atc ", "");
            return true;
        }
        if (text.StartsWith("atctg "))
        {
            textAutoChatTG = text.Replace("atctg ", "");
            return true;
        }

        //if (text == "attnl")
        //{
        //    Mod.isAutoTTNL = !Mod.isAutoTTNL;
        //    GameScr.info1.addInfo("Auto TTNL: " + (Mod.isAutoTTNL ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text == "abfdt")
        //{
        //    Mod.aDauDeTu = !Mod.aDauDeTu;
        //    GameScr.info1.addInfo("Auto buff đậu theo chỉ số đệ tử: " + (Mod.aDauDeTu ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text.StartsWith("bhpdt "))
        //{
        //    Mod.csHPDeTu = int.Parse(text.Split(new char[]
        //    {
        //        ' '
        //    })[1]);
        //    GameScr.info1.addInfo("HP buff đậu đệ tử: " + NinjaUtil.getMoneys((long)Mod.csHPDeTu), 0);
        //    text = "";
        //}
        //if (text.StartsWith("bkidt "))
        //{
        //    Mod.csKIDeTu = int.Parse(text.Split(new char[]
        //    {
        //        ' '
        //    })[1]);
        //    GameScr.info1.addInfo("KI buff đậu đệ tử: " + NinjaUtil.getMoneys((long)Mod.csKIDeTu), 0);
        //    text = "";
        //}
        //if (text == "abf")
        //{
        //    Mod.aBuffDau = !Mod.aBuffDau;
        //    GameScr.info1.addInfo("Auto buff đậu theo chỉ số: " + (Mod.aBuffDau ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text.StartsWith("bhp "))
        //{
        //    Mod.csHP = int.Parse(text.Split(new char[]
        //    {
        //        ' '
        //    })[1]);
        //    GameScr.info1.addInfo("HP buff đậu: " + NinjaUtil.getMoneys((long)Mod.csHP), 0);
        //    text = "";
        //}
        //if (text.StartsWith("bki "))
        //{
        //    Mod.csKI = int.Parse(text.Split(new char[]
        //    {
        //        ' '
        //    })[1]);
        //    GameScr.info1.addInfo("KI buff đậu: " + NinjaUtil.getMoneys((long)Mod.csKI), 0);
        //    text = "";
        //}
        //if (text == "akhu")
        //{
        //    Mod.isAutoVeKhu = !Mod.isAutoVeKhu;
        //    GameScr.info1.addInfo((Mod.isAutoVeKhu ? "Auto về khu cũ khi Login: Bật" : "Auto về khu cũ khi Login: Tắt") ?? "", 0);
        //    text = "";
        //}
        //if (text == "kk")
        //{
        //    Mod.khoakhu = !Mod.khoakhu;
        //    GameScr.info1.addInfo("Khóa chuyển khu: " + (Mod.khoakhu ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text == "kmap")
        //{
        //    Mod.khoamap = !Mod.khoamap;
        //    GameScr.info1.addInfo("Khóa map: " + (Mod.khoamap ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text == "nmt")
        //{
        //    if (Mod.getX(0) > 0 && Mod.getY(0) > 0)
        //    {
        //        Mod.GotoXY(Mod.getX(0), Mod.getY(0));
        //    }
        //    else
        //    {
        //        Mod.GotoXY(30, PickMobController.GetYsd(30));
        //    }
        //    text = "";
        //}
        //if (text == "nmp")
        //{
        //    if (Mod.getX(2) > 0 && Mod.getY(2) > 0)
        //    {
        //        Mod.GotoXY(Mod.getX(2), Mod.getY(2));
        //    }
        //    else
        //    {
        //        Mod.GotoXY(TileMap.pxw - 30, PickMobController.GetYsd(TileMap.pxw - 30));
        //    }
        //    text = "";
        //}
        //if (text == "nmg")
        //{
        //    if (Mod.getX(1) > 0 && Mod.getY(1) > 0)
        //    {
        //        Mod.GotoXY(Mod.getX(1), Mod.getY(1));
        //        Service.gI().getMapOffline();
        //        Service.gI().requestChangeMap();
        //    }
        //    else
        //    {
        //        Mod.GotoXY(TileMap.pxw / 2, PickMobController.GetYsd(TileMap.pxw / 2));
        //    }
        //    text = "";
        //}
        //if (text == "nmtr")
        //{
        //    if (Mod.getX(3) > 0 && Mod.getY(3) > 0)
        //    {
        //        Mod.GotoXY(Mod.getX(3), Mod.getY(3));
        //    }
        //    text = "";
        //}
        //if (text.StartsWith("do "))
        //{
        //    Mod.bossCanDo = text.Replace("do ", "");
        //    GameScr.info1.addInfo("Boss cần dò: " + Mod.bossCanDo, 0);
        //    text = "";
        //}
        //if (text.StartsWith("dk "))
        //{
        //    Mod.zoneMacDinh = int.Parse(text.Replace("dk ", ""));
        //    GameScr.info1.addInfo("Dò boss từ khu " + Mod.zoneMacDinh, 0);
        //    text = "";
        //}
        //if (text == "clrz")
        //{
        //    Mod.zoneMacDinh = 0;
        //    GameScr.info1.addInfo("Reset khu dò boss xuống", 0);
        //    text = "";
        //}
        //if (text == "doall")
        //{
        //    Mod.doBoss = !Mod.doBoss;
        //    GameScr.info1.addInfo("Dò boss: " + (Mod.doBoss ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text == "ksbs5")
        //{
        //    Mod.isKSBoss = false;
        //    Mod.isKSBossBangSkill5 = !Mod.isKSBossBangSkill5;
        //    GameScr.info1.addInfo((Mod.isKSBossBangSkill5 ? "KS Boss Bằng Skill 5: Bật" : "KS Boss Bằng Skill 5: Tắt") ?? "", 0);
        //    text = "";
        //}
        //if (text == "ksb")
        //{
        //    Mod.isKSBossBangSkill5 = false;
        //    Mod.isKSBoss = !Mod.isKSBoss;
        //    GameScr.info1.addInfo((Mod.isKSBoss ? "KS Boss bằng đấm thường: Bật" : "KS Boss bằng đấm thường: Tắt") ?? "", 0);
        //    text = "";
        //}
        //if (text.StartsWith("hpboss "))
        //{
        //    Mod.HPKSBoss = int.Parse(text.Replace("hpboss ", ""));
        //    GameScr.info1.addInfo("HP Boss khi đạt " + NinjaUtil.getMoneys((long)Mod.HPKSBoss) + " sẽ oánh bỏ con mẹ boss", 0);
        //    text = "";
        //}
        //if (text == "ttsp")
        //{
        //    Mod.isThongTinSuPhu = !Mod.isThongTinSuPhu;
        //    GameScr.info1.addInfo("Thông Tin Sư Phụ: " + (Mod.isThongTinSuPhu ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text == "ttdt")
        //{
        //    Mod.isThongTinDeTu = !Mod.isThongTinDeTu;
        //    GameScr.info1.addInfo("Thông Tin Đệ Tử: " + (Mod.isThongTinDeTu ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text == "xtb")
        //{
        //    Mod.xoaTauBay = !Mod.xoaTauBay;
        //    GameScr.info1.addInfo("Xóa tàu bay: " + (Mod.xoaTauBay ? "Tắt" : "Bật"), 0);
        //    text = "";
        //}
        //if (text == "xht")
        //{
        //    Mod.xoaHieuUngHopThe = !Mod.xoaHieuUngHopThe;
        //    GameScr.info1.addInfo("Hiệu ứng hợp thể: " + (Mod.xoaHieuUngHopThe ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text == "kvt")
        //{
        //    Mod.ghimX = Char.myCharz().cx;
        //    Mod.ghimY = Char.myCharz().cy;
        //    Mod.isKhoaViTri = !Mod.isKhoaViTri;
        //    GameScr.info1.addInfo("Khóa vị trí: " + (Mod.isKhoaViTri ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text == "ttnv")
        //{
        //    Mod.isBossM = false;
        //    Mod.isPKM = false;
        //    Mod.trangThai = !Mod.trangThai;
        //    GameScr.info1.addInfo("Trạng thái nhân vật đang trỏ: " + (Mod.trangThai ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}

        //if (text == "pkm")
        //{
        //    Mod.isPKM = !Mod.isPKM;
        //    Mod.isBossM = false;
        //    Mod.trangThai = false;
        //    GameScr.info1.addInfo("Bọn đấm nhau được trong khu: " + (Mod.isPKM ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text == "gdl")
        //{
        //    Mod.giamDungLuong = !Mod.giamDungLuong;
        //    GameScr.info1.addInfo("Giảm dung lượng: " + (Mod.giamDungLuong ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text == "xoamap")
        //{
        //    Mod.xoamap = !Mod.xoamap;
        //    GameScr.info1.addInfo("Xóa map: " + (Mod.xoamap ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text == "gmt")
        //{
        //    Mod.isGMT = false;
        //    text = "";
        //}
        //if (text.StartsWith("gmt "))
        //{
        //    int num = int.Parse(text.Remove(0, 4));
        //    if (num < GameScr.vCharInMap.size())
        //    {
        //        Mod.isGMT = true;
        //        Mod.charMT = (Char)GameScr.vCharInMap.elementAt(num);
        //    }
        //    text = "";
        //}
        //if (text == "abt")
        //{
        //    Mod.isAutoBT = !Mod.isAutoBT;
        //    GameScr.info1.addInfo("Auto bông tai: " + (Mod.isAutoBT ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text.StartsWith("bt "))
        //{
        //    Mod.timeBT = int.Parse(text.Replace("bt ", ""));
        //    GameScr.info1.addInfo("Delay auto bông tai: " + Mod.timeBT + "s", 0);
        //    text = "";
        //}
        //if (text == "anz")
        //{
        //    Mod.isAutoNhatXa = !Mod.isAutoNhatXa;
        //    if (Mod.isAutoNhatXa)
        //    {
        //        Mod.xNhatXa = Char.myCharz().cx;
        //        Mod.yNhatXa = Char.myCharz().cy;
        //        GameScr.info1.addInfo(string.Concat(new object[]
        //        {
        //            "Tọa Độ : ",
        //            Char.myCharz().cx,
        //            "|",
        //            Char.myCharz().cy
        //        }), 0);
        //    }
        //    GameScr.info1.addInfo("Auto Nhặt Xa : " + (Mod.isAutoNhatXa ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text.StartsWith("ndc "))
        //{
        //    Mod.textAutoChat = text.Replace("ndc ", "");
        //    GameScr.info1.addInfo("Nội dung auto chat : " + Mod.textAutoChat, 0);
        //    text = "";
        //}
        //if (text.StartsWith("ndctg "))
        //{
        //    Mod.textAutoChatTG = text.Replace("ndc ", "");
        //    GameScr.info1.addInfo("Nội dung auto chat thế giới : " + Mod.textAutoChatTG, 0);
        //    text = "";
        //}
        //if (text == "atchattg")
        //{
        //    Mod.isAutoCTG = !Mod.isAutoCTG;
        //    GameScr.info1.addInfo("Auto Chat Thế Giới: " + (Mod.isAutoCTG ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text == "atc")
        //{
        //    Mod.achat = !Mod.achat;
        //    GameScr.info1.addInfo("Auto chat : " + (Mod.achat ? "Bật" : "Tắt"), 0);
        //    text = string.Empty;
        //}
        //if (text.StartsWith("go "))
        //{
        //    int num2 = int.Parse(text.Remove(0, 3));
        //    if (num2 < GameScr.vCharInMap.size())
        //    {
        //        Char @char = (Char)GameScr.vCharInMap.elementAt(num2);
        //        Mod.GotoXY(@char.cx, @char.cy);
        //        Char.myCharz().focusManualTo(@char);
        //    }
        //    text = "";
        //}
        //if (text == "showhp")
        //{
        //    Mod.nvat = !Mod.nvat;
        //    GameScr.info1.addInfo("Thông tin người chơi trong map: " + (Mod.nvat ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        //if (text.StartsWith("kx "))
        //{
        //    new Thread(new ParameterizedThreadStart(Mod.VaoKhu)).Start(int.Parse(text.Remove(0, 3)));
        //    text = "";
        //}
        //if (text.StartsWith("tdc "))
        //{
        //    Mod.tocdochay = int.Parse(text.Replace("tdc ", ""));
        //    GameScr.info1.addInfo("Tốc độ phóng: " + Mod.tocdochay, 0);
        //    text = "";
        //}
        //if (text == "dapdo")
        //{
        //    Mod.isDapDo = !Mod.isDapDo;
        //    new Thread(new ThreadStart(Mod.AutoDapDo)).Start();
        //    GameScr.info1.addInfo("Đập đồ: " + (Mod.isDapDo ? "Bật" : "Tắt"), 0);
        //    text = "";
        //}
        return false;
    }

    private void UpdateTouch()
    {
        if (GameScr.gI().isNotPaintTouchControl())
        {
            return;
        }
        if (GameCanvas.isPointerHoldIn(modKeyPosX + 2, modKeyPosY - 41, 32, 32))
        {
            if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
            {
                UseItem(194);
                GameCanvas.clearAllPointerEvent();
            }
        }
        // F button
        else if (GameCanvas.isPointerHoldIn(modKeyPosX - 39, modKeyPosY + 6, 32, 32))
        {
            if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
            {
                UsePorata();
                GameCanvas.clearAllPointerEvent();
            }
        }
        // M button
        else if (GameCanvas.isPointerHoldIn(modKeyPosX - 84, modKeyPosY + 47, 32, 32))
        {
            if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
            {
                userOpenZones = true;
                Service.gI().openUIZone();
                GameCanvas.clearAllPointerEvent();
            }
        }
        // Open Menu Mod
        else if (GameCanvas.isPointerHoldIn(GameScr.imgPanel.getWidth() + 8, 3, GameScr.imgModFunc.getWidth() + 2, GameScr.imgModFunc.getHeight() + 2))
        {
            if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
            {
                OpenMenu();
                SoundMn.gI().buttonClick();
                GameCanvas.clearAllPointerEvent();
            }
        }
    }
    private long currTimeLogin;
    //public IEnumerator AutoLogin2()
    //{
    //    yield return new WaitForSecondsRealtime(10f);
    //    if (GameCanvas.currentScreen is ServerListScreen)
    //    {
    //        if (GameCanvas.loginScr == null)
    //        {
    //            GameCanvas.loginScr = new LoginScr();
    //        }
    //        GameCanvas.loginScr.switchToMe();
    //    }
    //    else if (GameCanvas.currentScreen is LoginScr)
    //    {
    //        yield return new WaitForSecondsRealtime(2f);
    //        GameCanvas.loginScr = new LoginScr();
    //        GameCanvas.loginScr.doLogin();
    //        Main.main.StartCoroutine(AutoLogin2());
    //    }

    //}

    public static bool AutoLogin()
    {
        if (autoLogin == null)
        {
            return false;
        }

        if (autoLogin.waitToNextLogin)
        {
            return true;
        }

        if (!Util.CanDoWithTime(autoLogin.lastTimeWait, 500))
        {
            return false;
        }

        if (ServerListScreen.testConnect != 2)
        {
            GameCanvas.serverScreen ??= new ServerListScreen();
            GameCanvas.serverScreen.switchToMe();
            autoLogin.lastTimeWait = mSystem.currentTimeMillis();
            return false;
        }

        if (GameCanvas.currentScreen != GameCanvas.loginScr)
        {
            GameCanvas.loginScr ??= new LoginScr();
            GameCanvas.loginScr.switchToMe();
            autoLogin.lastTimeWait = mSystem.currentTimeMillis();
            return false;
        }

        if (!autoLogin.hasSetUserPass)
        {
            Account account = autoLogin.GetAccWithUsername(accounts);
            if (account.getUsername().Length > 0)
            {
                Rms.saveRMSString("acc", account.getUsername());
                Rms.saveRMSString("pass", account.getPassword());
                if (GameCanvas.currentScreen == GameCanvas.loginScr)
                {
                    GameCanvas.loginScr.setUserPass();
                }
                autoLogin.hasSetUserPass = true;
            }
            autoLogin.lastTimeWait = mSystem.currentTimeMillis();
        }

        GameCanvas.loginScr.doLogin();
        autoLogin.waitToNextLogin = true;
        return true;
    }

    public void Update()
    {
        UpdateTouch();

        if (isPeanPet && mSystem.currentTimeMillis() - lastPeanPet >= 3000L)
        {
            Char pet = Char.myPetz();
            if (!pet.isDie && (Char.myPetz().cStamina <= Char.myPetz().cMaxStamina * 20 / 100
                || Char.myPetz().cHP < Char.myPetz().cHPFull * 20 / 100
                || Char.myPetz().cMP < Char.myPetz().cMPFull * 20 / 100))
            {
                GameScr.gI().doUseHP();
                lastPeanPet = mSystem.currentTimeMillis();
            }
        }

        //if (Input.GetKey("q") && SpecialSkill.gI().isnoitai)
        //{
        //    SpecialSkill.gI().isnoitai = false;
        //    GameScr.info1.addInfo("Đã Dừng", 0);
        //    if (isPaintCrackBall)
        //    {
        //        isThuongDeVip = false;
        //        isThuongDeVip = false;
        //        isThuongDeThuong = false;
        //        GameScr.info1.addInfo("Đã Dừng", 0);
        //    }
        //}

        if (isAutoPhaLe && itemPhale != null)
        {
            currPhale = GetCurrPhaLe(FindItemBagWithIndexUI(itemPhale.indexUI));
        }
        else
        {
            currPhale = -1;
        }

        if (isAutoChat && mSystem.currentTimeMillis() - lastAutoChat >= 4000L)
        {
            AutoChat();
            lastAutoChat = mSystem.currentTimeMillis();
        }

        if (isAutoChatTG && mSystem.currentTimeMillis() - lastAutoChatTG >= 30000L)
        {
            AutoChatTG();
            lastAutoChatTG = mSystem.currentTimeMillis();
        }

        //if (Mod.doBoss && mSystem.currentTimeMillis() - Mod.currDoBoss >= 1000L)
        //{
        //    Mod.DoBoss();
        //    Mod.currDoBoss = mSystem.currentTimeMillis();
        //}

        if (!TileMap.isOfflineMap() && mSystem.currentTimeMillis() - lastUpdateZones >= 1000L)
        {
            UseItemAuto();
            if (isUpdateZones)
            {
                Service.gI().openUIZone();
            }
            lastUpdateZones = mSystem.currentTimeMillis();
        }

        if (isAutoVQMM && mSystem.currentTimeMillis() - lastVQMM >= 1000L)
        {
            quayThuongDe();
            lastVQMM = mSystem.currentTimeMillis();
        }

        //Mod.AutoTTNL();

        if (autoWakeUp && mSystem.currentTimeMillis() - lastAutoWakeUp >= 1000)
        {
            AutoHoiSinh();
            lastAutoWakeUp = mSystem.currentTimeMillis();
        }

        //Mod.xd();
        //Mod.cd();
        //Mod.UseSkillAuto();

        if (focusBoss && mSystem.currentTimeMillis() - lastFocusBoss >= 500L)
        {
            AutoFocusBoss();
            lastFocusBoss = mSystem.currentTimeMillis();
        }

        //Mod.KSBoss();
        //Mod.KSBossBangSkill5();
        //Mod.khoaViTri();
        //Mod.gmt();
        //Mod.AutoBT();
        //Mod.AutoCTG();
        //Mod.AutoNhatXa();

        //if (Mod.isAutoVeKhu && mSystem.currentTimeMillis() - Mod.currVeKhuCu >= 20000L)
        //{
        //    Mod.currVeKhuCu = mSystem.currentTimeMillis();
        //    Mod.khuVeLai = TileMap.zoneID;
        //}

        //if (Mod.isAutoAnNho && Char.myCharz().cStamina <= 5 && mSystem.currentTimeMillis() - Mod.currAnNho >= 1000L)
        //{
        //    Mod.AnNho();
        //    Mod.currAnNho = mSystem.currentTimeMillis();
        //}

        if (autoAttack)
        {
            AutoAttack();
        }

        //if (Mod.isPKM && !Mod.isGMT && (Char.myCharz().charFocus == null || (Char.myCharz().charFocus != null && !Char.myCharz().isMeCanAttackOtherPlayer(Char.myCharz().charFocus))))
        //{
        //    for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        //    {
        //        Char @char = (Char)GameScr.vCharInMap.elementAt(i);
        //        if (@char != null && Char.myCharz().isMeCanAttackOtherPlayer(@char) && !@char.isPet && !@char.isMiniPet && !@char.cName.StartsWith("$") && !@char.cName.StartsWith("#") && @char.charID >= 0)
        //        {
        //            Char.myCharz().focusManualTo(@char);
        //            return;
        //        }
        //    }
        //}

        //if (Mod.thudau && mSystem.currentTimeMillis() - Mod.currThuDau >= 500L)
        //{
        //    Mod.td();
        //    Mod.currThuDau = mSystem.currentTimeMillis();
        //}

        //if (Mod.isAutoNeBoss && mSystem.currentTimeMillis() - Mod.currNeBoss >= 5000L)
        //{
        //    Mod.NeBoss();
        //    Mod.currNeBoss = mSystem.currentTimeMillis();
        //}

        UpdateNotifTichXanh();
    }

    public void PaintButton(mGraphics g, int xAnchor, int yAnchor)
    {
        if (Main.isIPhone && isShowButton)
        {
            if (GameCanvas.currentDialog != null || ChatPopup.currChatPopup != null || GameCanvas.menu.showMenu || GameScr.gI().isPaintPopup() || GameCanvas.panel.isShow || Char.myCharz().taskMaint.taskId == 0 || ChatTextField.gI().isShow || GameCanvas.currentScreen == MoneyCharge.instance)
            {
                return;
            }

            modKeyPosX = xAnchor;
            modKeyPosY = yAnchor;

            // C button
            g.drawImage(GameScr.imgNut, xAnchor + 20, yAnchor - 26, mGraphics.HCENTER | mGraphics.VCENTER);
            mFont.tahoma_7_green2.drawString(g, "C", xAnchor + 6 + (GameScr.imgNut.getWidth() / 2), yAnchor - 47 + (GameScr.imgNut.getHeight() / 2), mGraphics.HCENTER | mGraphics.VCENTER);
            if (GameCanvas.isPointerHoldIn(xAnchor + 2, yAnchor - 41, 32, 32))
            {
                g.drawImage(GameScr.imgNutF, xAnchor + 20, yAnchor - 26, mGraphics.HCENTER | mGraphics.VCENTER);
                mFont.tahoma_7b_focus.drawString(g, "C", xAnchor + 6 + (GameScr.imgNut.getWidth() / 2), yAnchor - 46 + (GameScr.imgNut.getHeight() / 2), mGraphics.HCENTER | mGraphics.VCENTER);
            }
            // F button
            g.drawImage(GameScr.imgNut, xAnchor - 21, yAnchor + 21, mGraphics.HCENTER | mGraphics.VCENTER);
            mFont.tahoma_7_green2.drawString(g, "F", xAnchor - 35 + (GameScr.imgNut.getWidth() / 2), yAnchor + (GameScr.imgNut.getHeight() / 2), mGraphics.HCENTER | mGraphics.VCENTER);
            if (GameCanvas.isPointerHoldIn(xAnchor - 39, yAnchor + 6, 32, 32))
            {
                g.drawImage(GameScr.imgNutF, xAnchor - 21, yAnchor + 21, mGraphics.HCENTER | mGraphics.VCENTER);
                mFont.tahoma_7b_focus.drawString(g, "F", xAnchor - 35 + (GameScr.imgNut.getWidth() / 2), yAnchor + 1 + (GameScr.imgNut.getHeight() / 2), mGraphics.HCENTER | mGraphics.VCENTER);
            }
            // M button
            g.drawImage(GameScr.imgNut, xAnchor - 66, yAnchor + 62, mGraphics.HCENTER | mGraphics.VCENTER);
            mFont.tahoma_7_green2.drawString(g, "M", xAnchor - 80 + (GameScr.imgNut.getWidth() / 2), yAnchor + 41 + (GameScr.imgNut.getHeight() / 2), mGraphics.HCENTER | mGraphics.VCENTER);
            if (GameCanvas.isPointerHoldIn(xAnchor - 84, yAnchor + 47, 32, 32))
            {
                g.drawImage(GameScr.imgNutF, xAnchor - 66, yAnchor + 62, mGraphics.HCENTER | mGraphics.VCENTER);
                mFont.tahoma_7b_focus.drawString(g, "M", xAnchor - 80 + (GameScr.imgNut.getWidth() / 2), yAnchor + 42 + (GameScr.imgNut.getHeight() / 2), mGraphics.HCENTER | mGraphics.VCENTER);
            }
        }
    }

    public void Paint(mGraphics g)
    {
        int imgHPWidth = mGraphics.getImageWidth(GameScr.imgHP);
        int imgMPWidth = mGraphics.getImageWidth(GameScr.imgMP);

        // FPS
        if (mSystem.currentTimeMillis() - lastUpdateFPS > 200L)
        {
            lastFps = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);
            lastUpdateFPS = mSystem.currentTimeMillis();
        }
        mFont.tahoma_7_white.drawStringBorder(g, "FPS: " + lastFps, 56 + imgMPWidth / 2, 30, mFont.LEFT, mFont.tahoma_7_grey);

        //mFont.tahoma_7_red.drawStringBorder(g, NinjaUtil.NumberTostring(Char.myCharz().cHP.ToString()), 84 + imgHPWidth / 2, 4, mFont.CENTER, mFont.tahoma_7_grey);
        //mFont.tahoma_7_blue1.drawStringBorder(g, NinjaUtil.NumberTostring(Char.myCharz().cMP.ToString()), 84 + imgMPWidth / 2, 17, mFont.CENTER, mFont.tahoma_7_grey);
        mFont.tahoma_7_red.drawStringBorder(g, (NinjaUtil.FormatNumber(Char.myCharz().cHP)), 84 + imgHPWidth / 2, 4, mFont.CENTER, mFont.tahoma_7_grey);
        mFont.tahoma_7_blue1.drawStringBorder(g, (NinjaUtil.FormatNumber(Char.myCharz().cMP)), 84 + imgMPWidth / 2, 17, mFont.CENTER, mFont.tahoma_7_grey);
        int xText = 15;
        int yText = 90;

        if (!showInfoMe)
        {
            mFont.tahoma_7_yellow.drawStringBorder(g, "Time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), xText, yText - 10, mFont.LEFT, mFont.tahoma_7_grey);
            int num2 = 0;
            mFont.tahoma_7_red.drawStringBorder(g, string.Concat(new object[]
            {
                TileMap.mapName,
                " [",
                TileMap.mapID,
                "]  - Khu: ",
                TileMap.zoneID
                }), xText, yText + num2, mFont.LEFT, mFont.tahoma_7_grey);

            num2 += 10;
            mFont.tahoma_7_red.drawStringBorder(g, string.Concat(new object[]
            {
                "X: ",
                Char.myCharz().cx,
                " - Y: ",
                Char.myCharz().cy
            }), xText, yText + num2, mFont.LEFT, mFont.tahoma_7_grey);
        }

        if (isAutoPhaLe)
        {
            mFont.tahoma_7b_red.drawString(g, (itemPhale != null) ? itemPhale.template.name : "Chưa Có", GameCanvas.w / 2, 72, mFont.CENTER);
            mFont.tahoma_7b_red.drawString(g, (itemPhale != null) ? ("Số Sao : " + currPhale.ToString()) : "Số Sao : -1", GameCanvas.w / 2, 82, mFont.CENTER);
            mFont.tahoma_7b_red.drawString(g, "Số Sao Cần Đập : " + maxPhale + " Sao", GameCanvas.w / 2, 92, mFont.CENTER);
        }
        if (isAutoPhaLe || isAutoVQMM)
        {
            Item tv = FindItemBagWithIndexUI(FindItemIndex(457));
            mFont.tahoma_7b_red.drawString(g, "Ngọc Xanh : " + NinjaUtil.getMoneys((long)Char.myCharz().luong) + " Ngọc Hồng : " + NinjaUtil.getMoneys(Char.myCharz().luongKhoa), GameCanvas.w / 2, 102, mFont.CENTER);
            mFont.tahoma_7b_red.drawString(g, string.Concat(new object[]
            {
                "Vàng : ",
                NinjaUtil.getMoneys(Char.myCharz().xu),
                " Thỏi Vàng : ",
                tv == null ? 0 : tv.quantity
            }), GameCanvas.w / 2, 112, mFont.CENTER);
        }

        //if (Mod.isPKM && !Mod.isGMT && (Char.myCharz().charFocus == null || (Char.myCharz().charFocus != null && !Char.myCharz().isMeCanAttackOtherPlayer(Char.myCharz().charFocus))))
        //{
        //    for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        //    {
        //        Char @char = (Char)GameScr.vCharInMap.elementAt(i);
        //        if (@char != null && Char.myCharz().isMeCanAttackOtherPlayer(@char) && !@char.isPet && !@char.isMiniPet && !@char.cName.StartsWith("$") && !@char.cName.StartsWith("#") && @char.charID >= 0)
        //        {
        //            Char.myCharz().focusManualTo(@char);
        //            return;
        //        }
        //    }
        //}

        if (showInfoMe)
        {
            if (mSystem.currentTimeMillis() - lastUpdateInfoMe > 3000L)
            {
                Service.gI().petInfo();
                lastUpdateInfoMe = mSystem.currentTimeMillis();
            }

            int num = 10;
            int numy = 64;
            mFont.tahoma_7b_yellow.drawStringBorder(g, "Sư Phụ :", xText, yText, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_white.drawStringBorder(g, "SM: " + NinjaUtil.FormatNumber(Char.myCharz().cPower), xText, yText + num, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_white.drawStringBorder(g, "TN: " + NinjaUtil.FormatNumber(Char.myCharz().cTiemNang), xText, yText + 2 * num, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_white.drawStringBorder(g, "SĐ: " + NinjaUtil.FormatNumber(Char.myCharz().cDamFull), xText, yText + 3 * num, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_white.drawStringBorder(g, "Giáp: " + NinjaUtil.getMoneys(Char.myCharz().cDefull), xText, yText + 4 * num, mFont.LEFT, mFont.tahoma_7_grey);

            xText += GameCanvas.w / 8;

            mFont.tahoma_7b_yellow.drawStringBorder(g, "Đệ Tử :", xText - numy, yText + numy, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_white.drawStringBorder(g, "SM: " + NinjaUtil.FormatNumber(Char.myPetz().cPower), xText - numy, yText + num + numy, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_white.drawStringBorder(g, "TN: " + NinjaUtil.FormatNumber(Char.myPetz().cTiemNang), xText - numy, yText + 2 * num + numy, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_white.drawStringBorder(g, "SĐ: " + NinjaUtil.FormatNumber(Char.myPetz().cDamFull), xText - numy, yText + 3 * num + numy, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_white.drawStringBorder(g, "HP : " + NinjaUtil.FormatNumber(Char.myPetz().cHP), xText - numy, yText + 4 * num + numy, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_white.drawStringBorder(g, "MP : " + NinjaUtil.FormatNumber(Char.myPetz().cMP), xText - numy, yText + 5 * num + numy, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_white.drawStringBorder(g, "Giáp: " + NinjaUtil.getMoneys(Char.myPetz().cDefull), xText - numy, yText + 6 * num + numy, mFont.LEFT, mFont.tahoma_7_grey);
        }

        if (notifBoss)
        {
            int numX = 38;
            for (int i = 0; i < bossNotif.size(); i++)
            {
                ((ShowBoss)bossNotif.elementAt(i)).PaintBoss(g, GameCanvas.w - 2, numX, mFont.RIGHT);
                numX += 10;
            }
        }

        if (showCharsInMap)
        {
            int numX = GameCanvas.w - 130;
            int numY = notifBoss ? 92 : 50;
            charsInMap.removeAllElements();
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                if (i > 15 || numY > GameScr.yHP - 20)
                {
                    g.fillRect(numX - 2, numY + 1, 150, 10, 2721889, 90);
                    mFont.tahoma_7_white.drawStringBorder(g, string.Concat(new object[]
                    {
                    i + 1,
                    " ..."
                    }), numX, numY, mFont.LEFT, mFont.tahoma_7_grey);
                    break;
                }
                Char char6 = (Char)GameScr.vCharInMap.elementAt(i);
                if (char6 != null && char6.cName != null && char6.cName.Length > 0
                    && !char6.isMiniPet // Minipet
                    && char6.cName.ToLower() != "trọng tài")
                {
                    g.fillRect(numX - 2, numY + 1, 150, 10, 2721889, 90);
                    string[] str = new string[]
                        {
                        (i + 1) < 10 ? "0" : "",
                        (i + 1).ToString(),
                        ". [",
                        CharGender(char6),
                        "] ",
                        char6.cName,
                        " [ ",
                        NinjaUtil.FormatNumber(char6.cHP).ToString(),
                        " ]"
                        };

                    // Player focus
                    if (char6 == Char.myCharz().charFocus)
                    {
                        mFont.tahoma_7_yellow.drawStringBorder(g, string.Concat(str), numX, numY, mFont.LEFT, mFont.tahoma_7_grey);
                    }
                    // Boss
                    else if (char6.charID < 0 && char6.charID > -1000 && char6.charID != -114)
                    {
                        mFont.tahoma_7_red.drawStringBorder(g, string.Concat(str), numX, numY, mFont.LEFT, mFont.tahoma_7_grey);
                    }
                    // Same clan
                    else if (Char.myCharz().clan != null && char6.clanID == Char.myCharz().clan.ID)
                    {
                        mFont.tahoma_7_green.drawStringBorder(g, string.Concat(str), numX, numY, mFont.LEFT, mFont.tahoma_7_grey);
                    }
                    // Others
                    else
                    {
                        mFont.tahoma_7_white.drawStringBorder(g, string.Concat(str), numX, numY, mFont.LEFT, mFont.tahoma_7_grey);
                    }

                    charsInMap.addElement(char6);
                    numY += 10;
                }
            }
        }

        int num4 = 70;
        Char charFocus = Char.myCharz().charFocus;
        if (charFocus != null && Char.myCharz().isMeCanAttackOtherPlayer(charFocus))
        {
            mFont.tahoma_7b_red.drawStringBorder(g, string.Concat(new object[]
            {
                        charFocus.cName,
                        " [",
                        NinjaUtil.FormatNumber(charFocus.cHP),
                        " / ",
                        NinjaUtil.FormatNumber(charFocus.cHPFull),
                        "] [",
                        CharGender(charFocus),
                        "]"
            }), GameCanvas.w / 2, num4, mFont.CENTER, mFont.tahoma_7_grey);
            num4 += 10;
            if (charFocus.protectEff)
            {
                mFont.tahoma_7b_red.drawString(g, "Đang khiên năng lượng", GameCanvas.w / 2, num4, mFont.CENTER);
                num4 += 10;
            }
            if (charFocus.isMonkey == 1)
            {
                mFont.tahoma_7b_red.drawString(g, "Đang biến khỉ", GameCanvas.w / 2, num4, mFont.CENTER);
                num4 += 10;
            }
            if (charFocus.sleepEff)
            {
                mFont.tahoma_7b_red.drawString(g, "Bị thôi miên", GameCanvas.w / 2, num4, mFont.CENTER);
                num4 += 10;
            }
            if (charFocus.holdEffID != 0)
            {
                mFont.tahoma_7b_red.drawString(g, "Bị trói", GameCanvas.w / 2, num4, mFont.CENTER);
                num4 += 10;
            }
            if (charFocus.isFreez)
            {
                mFont.tahoma_7b_red.drawString(g, "Bị TDHS: " + charFocus.freezSeconds.ToString(), GameCanvas.w / 2, num4, mFont.CENTER);
                num4 += 10;
            }
            if (charFocus.blindEff)
            {
                mFont.tahoma_7b_red.drawString(g, "Bị choáng", GameCanvas.w / 2, num4, mFont.CENTER);
            }
        }

        if (lineToBoss)
        {
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char @char = (Char)GameScr.vCharInMap.elementAt(i);
                if (@char != null && @char.cTypePk == 5 && !@char.cName.StartsWith("Đ"))
                {
                    g.setColor(Color.red);
                    g.drawLine(Char.myCharz().cx - GameScr.cmx, Char.myCharz().cy - GameScr.cmy, @char.cx - GameScr.cmx, @char.cy - GameScr.cmy);
                }
            }
        }

        if (TileMap.mapID != 51 && TileMap.mapID != 52 && TileMap.mapID != 113 && TileMap.mapID != 129 && TileMap.mapID != 165)
        {
            //g.drawImage(SplashScr.imgLogo, GameCanvas.w / 2, 25, 3);

            int id = GameCanvas.gameTick / 4 % 30;
            if (logos[id] != null)
            {
                int imgW = logos[id].getWidth() * mGraphics.zoomLevel / 4;
                int imgH = logos[id].getHeight() * mGraphics.zoomLevel / 4;
                g.drawImageScale(logos[id], (GameCanvas.w - imgW) / 2, -5, imgW, imgH);//chinh logo
            }

            //if (logo != null)
            //{
            //    int imgW = logo.getWidth() * mGraphics.zoomLevel / 4;
            //    int imgH = logo.getHeight() * mGraphics.zoomLevel / 4;
            //    g.drawImageScale(logo, (GameCanvas.w - imgW) / 2, -5, imgW, imgH);//chinh logo
            //}
        }

        g.drawImage(GameScr.imgModFunc, GameScr.imgPanel.getWidth() + 20, 15, 3);

        PaintPlayerTichXanh(g);
    }

    public void perform(int idAction, object p)
    {
        switch (idAction)
        {
            case 1:
                string notif;
                bool success = int.TryParse((string)p, out int mapId);
                if (success)
                {
                    XmapController.StartRunToMapId(mapId);
                    notif = "Di chuyển đến boss ở MAP " + mapId;
                }
                else
                {
                    notif = "Địa điểm không hợp lệ!";
                }
                GameScr.info1.addInfo(notif, 0);
                break;
            case 2:
                GameScr.info1.addInfo("Đã huỷ di chuyển đến Boss", 0);
                break;
            case 8:
                break;
            case 16:
                MyVector menuPet = new();
                menuPet.addElement(new Command(isPeanPet ? "Buff đậu cho đệ [Bật]" : "Buff đậu cho đệ [Tắt]", 17));
                GameCanvas.menu.startAt(menuPet, 4);
                break;
            case 17:
                isPeanPet = !isPeanPet;
                GameScr.info1.addInfo("Buff đậu cho đệ " + (isPeanPet ? "[Bật]" : "[Tắt]"), 0);
                break;
            case 32:
                MyVector myVector1 = new();
                myVector1.addElement(new Command(notifBoss ? "Thông báo BOSS [Bật]" : "Thông báo BOSS [Tắt]", 46));
                myVector1.addElement(new Command(lineToBoss ? "Kẻ đường tới BOSS [Bật]" : "Đường kẻ tới BOSS [Tắt]", 47));
                myVector1.addElement(new Command(focusBoss ? "Focus BOSS [Bật]" : "Focus BOSS [Tắt]", 52));
                GameCanvas.menu.startAt(myVector1, 4);
                break;
            // Goback
            case 38:
                PickMob.mapGoback = TileMap.mapID;
                PickMob.zoneGoback = TileMap.zoneID;
                PickMob.xGoback = Char.myCharz().cx;
                PickMob.yGoback = Char.myCharz().cy;

                PickMob.isGoBack = !PickMob.isGoBack;
                if (PickMob.isGoBack)
                {
                    GameScr.info1.addInfo(string.Concat(new object[]
                    {
                        "Map Goback: ",
                        TileMap.mapName,
                        " | Khu: ",
                        TileMap.zoneID
                    }), 0);
                    GameScr.info1.addInfo(string.Concat(new object[]
                    {
                        "Tọa độ X: ",
                        PickMob.xGoback,
                        " | Y: ",
                        PickMob.yGoback
                    }), 0);
                    if (Char.myCharz().cHP <= 0 || Char.myCharz().statusMe == 14)
                    {
                        Service.gI().returnTownFromDead();
                        new Thread(new ThreadStart(PickMob.GoBack)).Start();
                    }
                }
                GameScr.info1.addInfo("Goback tọa độ " + (PickMob.isGoBack ? "[Bật]" : "[Tắt]"), 0);
                break;
            case 42:
                autoAttack = !autoAttack;
                GameScr.info1.addInfo("Tự đánh " + (autoAttack ? "[Bật]" : "[Tắt]"), 0);
                break;
            // Ne sieu quai
            case 43:
                PickMob.neSieuQuai = !PickMob.neSieuQuai;
                GameScr.info1.addInfo("Né siêu quái " + (PickMob.neSieuQuai ? "[Bật]" : "[Tắt]"), 0);
                break;
            // Tan sat
            case 44:
                PickMob.tsPlayer = false;
                PickMob.tanSat = p != null ? (bool)p : !PickMob.tanSat;
                GameScr.info1.addInfo("Tàn sát " + (PickMob.tanSat ? "[Bật]" : "[Tắt]"), 0);
                break;
            // Menu train
            case 45:
                MyVector myVector = new();
                MyVector mobIds = new MyVector();
                for (int i = 0; i < GameScr.vMob.size(); i++)
                {
                    Mob mob = (Mob)GameScr.vMob.elementAt(i);
                    if (GameScr.gI().isMeCanAttackMob(mob) && !mobIds.contains(mob.templateId) && !PickMob.TypeMobsTanSat.Contains(mob.templateId))
                    {
                        mobIds.addElement(mob.templateId);
                        myVector.addElement(new Command("Tàn sát " + mob.getTemplate().name, 49, mob));
                    }
                }
                myVector.addElement(new Command(PickMob.tanSat ? "Tàn sát [Bật]" : "Tàn sát [Tắt]", 44));
                myVector.addElement(new Command(PickMob.tsPlayer ? "Tàn sát\nngười [Bật]" : "Tàn sát\nngười [Tắt]", 48));
                myVector.addElement(new Command(autoAttack ? "Tự đánh [Bật]" : "Tự đánh [Tắt]", 42));
                myVector.addElement(new Command(PickMob.neSieuQuai ? "Né siêu quái [Bật]" : "Né siêu quái [Tắt]", 43));
                myVector.addElement(new Command(PickMob.vuotDiaHinh ? "Vượt địa hình [Bật]" : "Vượt địa hình [Tắt]", 76));
                myVector.addElement(new Command(PickMob.telePem ? "Dịch chuyển\n[Bật]" : "Dịch chuyển\n[Tắt]", 80));
                myVector.addElement(new Command(PickMob.isGoBack ? "Goback Tọa Độ [Bật]" : "Goback Tọa Độ [Tắt]", 38));
                myVector.addElement(new Command("Xoá danh sách tàn sát", 51));
                GameCanvas.menu.startAt(myVector, 4);
                break;
            case 46:
                notifBoss = !notifBoss;
                GameScr.info1.addInfo("Thông báo BOSS " + (notifBoss ? "[Bật]" : "[Tắt]"), 0);
                break;
            case 47:
                lineToBoss = !lineToBoss;
                GameScr.info1.addInfo("Kẻ đường tới BOSS " + (lineToBoss ? "[Bật]" : "[Tắt]"), 0);
                break;
            // Tan sat nguoi
            case 48:
                PickMob.tanSat = false;
                PickMob.tsPlayer = p != null ? (bool)p : !PickMob.tsPlayer;
                GameScr.info1.addInfo("Tàn sát người " + (PickMob.tsPlayer ? "[Bật]" : "[Tắt]"), 0);
                break;
            case 49:
                Mob mobType = (Mob)p;
                if (!PickMob.TypeMobsTanSat.Contains(mobType.templateId))
                {
                    PickMob.TypeMobsTanSat.Add(mobType.templateId);
                }
                GameScr.info1.addInfo("Tàn sát " + mobType.getTemplate().name, 0);
                perform(44, true);
                break;
            case 51:
                PickMob.TypeMobsTanSat.Clear();
                GameScr.info1.addInfo("Đã xoá danh sách quái tàn sát!", 0);
                break;
            case 52:
                focusBoss = !focusBoss;
                GameScr.info1.addInfo("Focus BOSS " + (focusBoss ? "[Bật]" : "[Tắt]"), 0);
                break;
            case 53:
                MyVector menuOthers = new();
                menuOthers.addElement(new Command("Tốc độ\nGame", 54));
                menuOthers.addElement(new Command("Tự động\nChat " + (isAutoChat ? "[Bật]" : "[Tắt]"), 55));
                menuOthers.addElement(new Command("Tự động\nChat Thế\nGiới " + (isAutoChatTG ? "[Bật]" : "[Tắt]"), 56));
                menuOthers.addElement(new Command("Load ô\nskill", 57));
                menuOthers.addElement(new Command(isPlayingMusic ? "Tắt nhạc" : "Bật nhạc", 60));
                GameCanvas.menu.startAt(menuOthers, 4);
                break;
            case 54:
                MyChatTextField(ChatTextField.gI(), "Nhập tốc độ game", "1 đến 10");
                break;
            case 55:
                isAutoChat = !isAutoChat;
                GameScr.info1.addInfo("Tự động chat " + (isAutoChat ? "[Bật]" : "[Tắt]"), 0);
                break;
            case 56:
                isAutoChatTG = !isAutoChatTG;
                GameScr.info1.addInfo("Tự động chat thế giới " + (isAutoChatTG ? "[Bật]" : "[Tắt]"), 0);
                break;
            case 57:
                LoadSkillToScreen();
                GameScr.info1.addInfo("Đã load ô skill", 0);
                break;
            case 60:
                Sound.PlayMusic(UnityEngine.Random.Range(0, 3));
                Debug.Log("Music " + musics.Count);
                GameScr.info1.addInfo("Đã bật trình phát nhạc", 0);
                break;
            // Vuot dia hinh
            case 76:
                PickMob.vuotDiaHinh = !PickMob.vuotDiaHinh;
                GameScr.info1.addInfo("Vượt địa hình " + (PickMob.vuotDiaHinh ? "[Bật]" : "[Tắt]"), 0);
                break;
            // Dich chuyen danh quai
            case 80:
                PickMob.telePem = !PickMob.telePem;
                GameScr.info1.addInfo("Dịch chuyển đến quái\n" + (PickMob.telePem ? "[Bật]" : "[Tắt]"), 0);
                break;
            case 89:
                MyVector menuAutoPick = new();
                menuAutoPick.addElement(new Command("Tự động nhặt " + (PickMob.IsAutoPickItems ? "[Bật]" : "[Tắt]"), 90));
                menuAutoPick.addElement(new Command("Nhặt tất cả " + (PickMob.IsPickItemsAll ? "[Bật]" : "[Tắt]"), 91));
                menuAutoPick.addElement(new Command("Nhặt xa\n" + (PickMob.IsPickItemsDis ? "[Bật]" : "[Tắt]"), 92));
                GameCanvas.menu.startAt(menuAutoPick, 4);
                break;
            case 90:
                PickMob.IsAutoPickItems = !PickMob.IsAutoPickItems;
                GameScr.info1.addInfo("Tự động nhặt " + (PickMob.IsAutoPickItems ? "[Bật]" : "[Tắt]"), 0);
                break;
            case 91:
                PickMob.IsPickItemsAll = !PickMob.IsPickItemsAll;
                GameScr.info1.addInfo("Nhặt tất cả " + (PickMob.IsPickItemsAll ? "[Bật]" : "[Tắt]"), 0);
                break;
            case 92:
                PickMob.IsPickItemsDis = !PickMob.IsPickItemsDis;
                GameScr.info1.addInfo("Nhặt xa " + (PickMob.IsPickItemsDis ? "[Bật]" : "[Tắt]"), 0);
                break;
            case 100:
                string str = (string)p;
                int.TryParse(str.Split("-")[0], out indexAutoPoint);
                bool.TryParse(str.Split("-")[1], out autoPointForPet);
                GameCanvas.panel.hideNow();
                MyChatTextField(ChatTextField.gI(), "Tăng đến mức", "VD: 220000");
                break;
            case 101:
                isOpenAccMAnager = true;
                break;
            case 102:
                Account account = (Account)p;
                Rms.saveRMSString("acc", account.getUsername());
                Rms.saveRMSString("pass", account.getPassword());
                if (GameCanvas.loginScr != null && GameCanvas.currentScreen == GameCanvas.loginScr)
                {
                    GameCanvas.loginScr.setUserPass();
                }
                isOpenAccMAnager = false;
                break;
            case 103:
                int index = accounts.IndexOf((Account)p);
                accounts.RemoveAt(index);
                cmdsChooseAcc.RemoveAt(index);
                cmdsDelAcc.RemoveAt(index);
                SaveAcc();
                break;
            case 104:
                isOpenAccMAnager = false;
                break;
            case 500:
            case 501:
                AddOrRemoveAutoItem((Item)p, idAction == 500);
                break;
            // Menu XMAP
            case 883:
                XmapController.ShowXmapMenu();
                break;
        }
    }

    public void AutoBuyItem(int num, Item itemBuy)
    {
        new Thread(() =>
        {
            for (int i = 0; i < num; i++)
            {
                Service.gI().buyItem(3, itemBuy.template.id, 0);
                Thread.Sleep(200);
            }
            GameScr.info1.addInfo("Đã mua xong " + num + " " + itemBuy.template.name, 0);
        }).Start();
    }

    private void AddOrRemoveAutoItem(Item item, bool isAdd)
    {
        if (isAdd)
        {
            listItemAuto.Add(new ItemAuto(item.template.iconID, item.template.id));
            GameScr.info1.addInfo("Đã thêm " + item.template.name + " vào Auto Item", 0);
        }
        else
        {
            foreach (ItemAuto itemAuto in listItemAuto)
            {
                if (itemAuto.iconID == item.template.iconID && itemAuto.id == item.template.id)
                {
                    listItemAuto.Remove(itemAuto);
                    GameScr.info1.addInfo("Đã xóa " + item.template.name + " khỏi Auto Item", 0);
                    break;
                }
            }
        }
    }

    public void DoDoubleClickToObj(IMapObject obj)
    {
        if ((obj.Equals(Char.myCharz().npcFocus) || GameScr.gI().mobCapcha == null) && !GameScr.gI().checkClickToBotton(obj))
        {
            GameScr.gI().checkEffToObj(obj, false);
            Char.myCharz().cancelAttack();
            Char.myCharz().currentMovePoint = null;
            Char.myCharz().cvx = (Char.myCharz().cvy = 0);
            obj.stopMoving();
            GameScr.gI().auto = 10;
            GameScr.gI().doFire(isFireByShortCut: false, skipWaypoint: true);
            GameScr.gI().clickToX = obj.getX();
            GameScr.gI().clickToY = obj.getY();
            GameScr.gI().clickOnTileTop = false;
            GameScr.gI().clickMoving = true;
            GameScr.gI().clickMovingRed = true;
            GameScr.gI().clickMovingTimeOut = 20;
            GameScr.gI().clickMovingP1 = 30;
        }
    }

    public void MyChatTextField(ChatTextField chatTField, string strChat, string strName)
    {
        chatTField.strChat = strChat;
        chatTField.tfChat.name = strName;
        chatTField.to = string.Empty;
        chatTField.isShow = true;
        chatTField.tfChat.isFocus = true;
        chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
        chatTField.tfChat.setMaxTextLenght(10);
        if (!Main.isPC)
        {
            chatTField.startChat(GameCanvas.panel, string.Empty);
        }
        else if (GameCanvas.isTouch)
        {
            chatTField.tfChat.doChangeToTextBox();
        }
    }

    public void ChangeGameSpeed(string strSpeed)
    {
        bool success = int.TryParse(strSpeed, out int speed);
        if (success && speed > 0 && speed <= 10)
        {
            Time.timeScale = speed;
            GameScr.info1.addInfo("Tốc độ game: " + speed, 0);
        }
        else
        {
            GameScr.info1.addInfo("Chỉ nhập số từ 1 đến 10", 0);
        }
    }

    public void TeleportToPlayer(int charID)
    {
        Service.gI().gotoPlayer(charID);
    }

    public void AddNotifTichXanh(string notif)
    {
        listNotifTichXanh.addElement(notif);
        if (!startChat)
        {
            int halfW = GameCanvas.w / 2;
            startChat = true;
            xNotif = halfW + halfW / 2;
            lastUpdateNotif = mSystem.currentTimeMillis();
        }
    }

    private void PaintPlayerTichXanh(mGraphics g)
    {
        if (listNotifTichXanh.size() != 0)
        {
            string st = (string)listNotifTichXanh.elementAt(0);
            int halfW = GameCanvas.w / 2;
            g.setClip(halfW - halfW / 3, 50, halfW / 3 * 2, 12);
            g.fillRect(halfW - halfW / 3, 50, halfW / 3 * 2, 12, 0, 60);
            mFont.tahoma_7_yellow.drawStringBorder(g, st, xNotif, 50, 0, mFont.tahoma_7_grey);
            PaintTicks(g, xNotif - 12, 51);
        }
    }

    private void UpdateNotifTichXanh()
    {
        if (!startChat || mSystem.currentTimeMillis() - lastUpdateNotif < 10)
        {
            return;
        }
        xNotif -= 1;
        string strChat = (string)listNotifTichXanh.elementAt(0);
        lastUpdateNotif = mSystem.currentTimeMillis();
        if (xNotif < GameCanvas.w / 2 - 100 - mFont.tahoma_7_yellow.getWidth(strChat))
        {
            xNotif = GameCanvas.w / 2 + 100;
            listNotifTichXanh.removeElementAt(0);
            if (listNotifTichXanh.size() == 0)
            {
                startChat = false;
            }
        }
    }

    public void SetAutoIntrinsic(int param)
    {
        if (curSelectIntrinsic.Length > 0)
        {
            bool success = int.TryParse(curSelectIntrinsic.Split("đến ")[1].Split("%")[0], out int maxParam);
            if (success && param > 0 && param <= maxParam)
            {
                paramIntrinsic = param;
                if (curSelectIntrinsic.Contains("+"))
                {
                    curSelectIntrinsic = curSelectIntrinsic.Split("+")[0].Trim();
                }
                else if (curSelectIntrinsic.Contains("dưới"))
                {
                    curSelectIntrinsic = curSelectIntrinsic.Split("dưới ")[0].Trim();
                }
                else
                {
                    paramIntrinsic = -1;
                    curSelectIntrinsic = "";
                    GameScr.info1.addInfo("Có lỗi xảy ra, vui lòng liên hệ ADMIN!", 0);
                    return;
                }
                new Thread(new ThreadStart(DoAutoIntrinsic)).Start();
            }
            else
            {
                GameScr.info1.addInfo("Chỉ số đã chọn không đúng! (0)", 0);
            }
        }
        else
        {
            GameScr.info1.addInfo("Chỉ số đã chọn không đúng! (1)", 0);
        }
    }

    private void DoAutoIntrinsic()
    {
        while (paramIntrinsic != -1)
        {
            // Open Intrinsic
            Service.gI().speacialSkill(0);
            Thread.Sleep(500);
            Service.gI().confirmMenu(5, 2);
            Thread.Sleep(500);
            Service.gI().confirmMenu(5, 0);
            Thread.Sleep(500);
        }
    }

    public void CheckAutoIntrinsic(string info)
    {
        if (info.Contains("+"))
        {
            string[] recvInfo = info.Split("+");
            string recvName = recvInfo[0].Trim();
            int recvParam;
            if (int.TryParse(recvInfo[1].Split("%")[0], out recvParam))
            {
                if (curSelectIntrinsic == recvName && recvParam >= paramIntrinsic)
                {
                    GameScr.info1.addInfo("Mở nội tại " + curSelectIntrinsic + " " + paramIntrinsic + "% thành công!", 0);
                    paramIntrinsic = -1;
                    curSelectIntrinsic = "";
                    GameCanvas.menu.menuSelectedItem = GameCanvas.menu.menuItems.size() - 1;
                    GameCanvas.menu.performSelect();
                    GameCanvas.menu.doCloseMenu();
                }
            }
        }
        else if (info.Contains("dưới"))
        {
            string[] recvInfo = info.Split("dưới ");
            string recvName = recvInfo[0].Trim();
            int recvParam;
            if (int.TryParse(recvInfo[1].Split("%")[0], out recvParam))
            {
                if (curSelectIntrinsic == recvName && recvParam >= paramIntrinsic)
                {
                    GameScr.info1.addInfo("Mở nội tại " + curSelectIntrinsic + " " + paramIntrinsic + "% thành công!", 0);
                    paramIntrinsic = -1;
                    curSelectIntrinsic = "";
                    GameCanvas.menu.menuSelectedItem = GameCanvas.menu.menuItems.size() - 1;
                    GameCanvas.menu.performSelect();
                    GameCanvas.menu.doCloseMenu();
                }
            }
        }
        else
        {
            paramIntrinsic = -1;
            curSelectIntrinsic = "";
            GameCanvas.menu.doCloseMenu();
        }
    }

    public void SetIncreasePoint(string strPoint)
    {
        bool success = int.TryParse(strPoint, out int point);
        if (success && indexAutoPoint != -1 && point > 0)
        {
            pointIncrease = point;
            new Thread(new ThreadStart(DoAutoIncreasePoint)).Start();
            GameScr.info1.addInfo("Tự động tăng " + strPointTypes[indexAutoPoint] + " đến " + point, 0);

        }
        else
        {
            GameScr.info1.addInfo("Có lỗi xảy ra (100)", 0);
        }
    }

    private void DoAutoIncreasePoint()
    {
        while (indexAutoPoint != -1 && pointIncrease > 0)
        {
            Char @char = autoPointForPet ? Char.myPetz() : Char.myCharz();
            var currentPoint = indexAutoPoint switch
            {
                0 => @char.cHPGoc,
                1 => @char.cMPGoc,
                2 => @char.cDamGoc,
                3 => @char.cDefGoc,
                4 => @char.cCriticalGoc,
                _ => 0,
            };
            if (currentPoint >= pointIncrease)
            {
                indexAutoPoint = -1;
                pointIncrease = 0;
                GameScr.info1.addInfo("Đã đạt chỉ số yêu cầu", 0);
                break;
            }
            Service.gI().upPotential(autoPointForPet, indexAutoPoint, 100);
            Thread.Sleep(500);
        }
    }

    public void LoadAcc()
    {
        string text = Rms.loadRMSString("accManager");
        if (text == null || text.Trim('|') == string.Empty) return;

        accounts.Clear();
        cmdsChooseAcc.Clear();
        cmdsDelAcc.Clear();

        string[] accs = text.Trim('|').Split('|');
        for (int i = 0; i < accs.Length; i++)
        {
            string[] acc = accs[i].Split('$');

            Account account = new(acc[0], acc[1]);
            accounts.Add(account);

            Command cmd = new(account.getUsername(), this, 102, account);
            cmd.setType();
            cmdsChooseAcc.Add(cmd);

            Command cmdDel = new("Xoá", this, 103, account);
            cmdDel.setTypeDelete();
            cmdsDelAcc.Add(cmdDel);
        }
    }

    public void AddAccount(string user, string pass)
    {
        Account account = new(user, pass);

        int index = accounts.IndexOf(account);
        if (index != -1)
        {
            accounts.RemoveAt(index);
        }
        accounts.Insert(0, account);

        for (int i = 5; i < accounts.Count; i++)
        {
            accounts.RemoveAt(i);
        }
        SaveAcc();
    }

    private void SaveAcc()
    {
        string text = "";
        foreach (Account acc in accounts)
        {
            text += string.Join('$', acc.getUsername(), acc.getPassword());
            text += "|";
        }
        Rms.saveRMSString("accManager", text.Trim('|'));
    }

    //private static Account FindAccWithUsername(string username)
    //{
    //    foreach (Account acc in accounts)
    //    {
    //        if (acc.getUsername().Equals(username))
    //        {
    //            return acc;
    //        }
    //    }

    //    return new Account("", "");
    //}

    private void AutoChat()
    {
        if (string.IsNullOrEmpty(textAutoChat))
        {
            GameScr.info1.addInfo("Chưa cài nội dung tự động chat", 0);
        }
        else
        {
            Service.gI().chat(textAutoChat);
        }
    }

    private void AutoChatTG()
    {
        if (string.IsNullOrEmpty(textAutoChatTG))
        {
            GameScr.info1.addInfo("Chưa cài nội dung tự động chat thế giới", 0);
        }
        else
        {
            Service.gI().chatGlobal(textAutoChatTG);
        }
    }

    public static string EncodeStringToByteArrayString(string inputString, string key)
    {
        byte[] encodedBytes = EncodeToBytes(inputString, key);
        string byteArrayString = BitConverter.ToString(encodedBytes).Replace("-", "");

        return string.Join("-", SplitByLength(byteArrayString, 2));
    }

    static byte[] EncodeToBytes(string inputString, string key)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] encodedBytes = new byte[inputBytes.Length];

        for (int i = 0; i < inputBytes.Length; i++)
        {
            encodedBytes[i] = (byte)(inputBytes[i] ^ keyBytes[i % keyBytes.Length]);
        }

        return encodedBytes;
    }

    static string[] SplitByLength(string str, int length)
    {
        int strLength = str.Length;
        int numSegments = (strLength + length - 1) / length;
        string[] segments = new string[numSegments];

        for (int i = 0; i < numSegments; i++)
        {
            int startIndex = i * length;
            int segmentLength = Math.min(length, strLength - startIndex);
            segments[i] = str.Substring(startIndex, segmentLength);
        }

        return segments;
    }

    public static string DecodeByteArrayString(string byteArrayString, string key)
    {
        try
        {

            string[] hexValues = byteArrayString.Split('-');
            string concatenatedHex = string.Join("", hexValues);
            byte[] encodedBytes = new byte[concatenatedHex.Length / 2];

            for (int i = 0; i < encodedBytes.Length; i++)
            {
                encodedBytes[i] = Convert.ToByte(concatenatedHex.Substring(i * 2, 2), 16);
            }

            string decodedString = DecodeToString(encodedBytes, key);
            return decodedString;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    static string DecodeToString(byte[] encodedBytes, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] decodedBytes = new byte[encodedBytes.Length];

        for (int i = 0; i < encodedBytes.Length; i++)
        {
            decodedBytes[i] = (byte)(encodedBytes[i] ^ keyBytes[i % keyBytes.Length]);
        }

        return Encoding.UTF8.GetString(decodedBytes);
    }

    public static void Log(string text)
    {
        if (!isDebugEnable)
        {
            return;
        }
        Debug.Log(text);
    }

    public static void LogSlow(string text, long millis)
    {
        if (!isDebugEnable)
        {
            return;
        }
        if (mSystem.currentTimeMillis() - lastTimeLog > millis)
        {
            lastTimeLog = mSystem.currentTimeMillis();
            Debug.Log(text);
        }
    }

    public static void WriteLog(string message)
    {
        if (!isDebugEnable)
        {
            return;
        }
        try
        {
            string logFileName = "log_" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt";

            FileStream fileStream = new(logFileName, FileMode.OpenOrCreate);
            // 
            StreamWriter streamWriter = new(fileStream);
            streamWriter.WriteLine(DateTime.Today.ToString("HH:mm:ss") + ": " + message);
            streamWriter.Flush();
            streamWriter.Close();
        }
        catch (Exception e)
        {
            Log(e.Message);
        }
    }

    private void LoadSkillToScreen()
    {
        for (int i = 0; i < Char.myCharz().vSkill.size(); i++)
        {
            Skill skill = (Skill)Char.myCharz().vSkill.elementAt(i);

            if (GameCanvas.isTouch && !Main.isPC)
            {
                for (int j = 0; j < GameScr.onScreenSkill.Length; j++)
                {
                    if (GameScr.onScreenSkill[j] == skill)
                    {
                        GameScr.onScreenSkill[j] = null;
                    }
                }
                GameScr.onScreenSkill[i] = skill;
                GameScr.gI().saveonScreenSkillToRMS();
            }
            else
            {
                for (int k = 0; k < GameScr.keySkill.Length; k++)
                {
                    if (GameScr.keySkill[k] == skill)
                    {
                        GameScr.keySkill[k] = null;
                    }
                }
                GameScr.keySkill[i] = skill;
                GameScr.gI().saveKeySkillToRMS();
            }
        }
    }

    public static string DecodeByteArrayString(string byteArrayString)
    {
        try
        {
            string[] hexValues = byteArrayString.Split('-');
            string concatenatedHex = string.Join("", hexValues);
            byte[] encodedBytes = new byte[concatenatedHex.Length / 2];

            for (int i = 0; i < encodedBytes.Length; i++)
            {
                encodedBytes[i] = Convert.ToByte(concatenatedHex.Substring(i * 2, 2), 16);
            }

            string decodedString = DecodeToString(encodedBytes, 69.ToString());
            return decodedString;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public static void DoChatGlobal()
    {
        GameCanvas.endDlg();
        if (Char.myCharz().checkLuong() < 5)
        {
            GameCanvas.startOKDlg(mResources.not_enough_luong_world_channel);
            return;
        }
        if (GameCanvas.panel.chatTField == null)
        {
            GameCanvas.panel.chatTField = new ChatTextField();
            GameCanvas.panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
            GameCanvas.panel.chatTField.initChatTextField();
            GameCanvas.panel.chatTField.parentScreen = GameCanvas.panel;
        }
        GameCanvas.panel.chatTField.strChat = mResources.world_channel_5_luong;
        GameCanvas.panel.chatTField.tfChat.name = mResources.CHAT;
        GameCanvas.panel.chatTField.to = string.Empty;
        GameCanvas.panel.chatTField.isShow = true;
        GameCanvas.panel.chatTField.tfChat.isFocus = true;
        GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
        if (Main.isWindowsPhone)
        {
            GameCanvas.panel.chatTField.tfChat.strInfo = GameCanvas.panel.chatTField.strChat;
        }
        if (!Main.isPC)
        {
            GameCanvas.panel.chatTField.startChat(GameCanvas.panel, string.Empty);
        }
        else if (GameCanvas.isTouch)
        {
            GameCanvas.panel.chatTField.tfChat.doChangeToTextBox();
        }
    }

    public void GoToBoss(int mapId)
    {
        MyVector myVector = new();
        myVector.addElement(new Command("Đi tới\nMAP " + mapId, this, 1, mapId.ToString()));
        myVector.addElement(new Command("Huỷ", this, 2, null));

        GameCanvas.menu.startAt(myVector, 4);
    }

    public void ChangeFPSTarget()
    {
        Rms.saveRMSInt("isHighFps", isHighFps ? 1 : 0);
        if (isHighFps)
        {
            Application.targetFrameRate = 60;
        }
        else
        {
            Application.targetFrameRate = 30;
        }
    }

    public static Npc GetNpcByTempId(int tempId)
    {
        for (int i = 0; i < GameScr.vNpc.size(); i++)
        {
            Npc npc = (Npc)GameScr.vNpc.elementAt(i);
            if (npc.template.npcTemplateId == tempId)
            {
                return npc;
            }
        }
        return null;
    }

    public static void LoadLogoImages()
    {

        for (int i = 0; i < 60; i++)
        {
            logos[i] = GameCanvas.LoadImageFromRoot("/logo/" + i + ".png");
        }

        //logo = GameCanvas.LoadImageFromRoot("/logo/logo.png");
        imgBg = GameCanvas.LoadImageFromRoot("/logo/bg.png");
        imgLogoBig = GameCanvas.LoadImageFromRoot("/logo/logo.png");
        // imgLogoBig = GameCanvas.LoadImageFromRoot("/logo/logo2.png");
    }

    public static void LoadTickImages()
    {
        for (int i = 0; i < 20; i++)
        {
            ticks[i] = GameCanvas.loadImage("/tick/tick_" + i);
        }
    }

    public static void PaintTicks(mGraphics g, int x, int y)
    {
        int id = GameCanvas.gameTick / 4 % 20;
        if (ticks[id] != null)
        {
            g.drawImage(ticks[id], x, y);
        }
    }

    private static IEnumerator LoadFile(string fullPath)
    {
        string fileUri = "file://" + fullPath;

        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fileUri, AudioType.OGGVORBIS);
        www.certificateHandler = new BypassCertificateHandler();

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            AudioClip temp = DownloadHandlerAudioClip.GetContent(www);
            musics.Add(temp);
        }
    }

    public static void InitMusic()
    {
        int fromRms = Rms.loadRMSInt("musicSize");
        musicCount = (fromRms == -1 ? 0 : fromRms);
        for (int i = 0; i < musicCount; i++)
        {
            string fullPath = Rms.GetiPhoneDocumentsPath() + "/music_" + i + ".ogg";
            if (File.Exists(fullPath))
            {
                CoroutineRunner.Instance.RunCoroutine(LoadFile(fullPath));
            }
            else
            {
                Debug.LogWarning("File does not exist: " + fullPath);
            }
        }
    }
}