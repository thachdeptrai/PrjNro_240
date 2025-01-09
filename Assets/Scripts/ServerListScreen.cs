using System;
using System.Text;
using UnityEngine;

public class ServerListScreen : mScreen, IActionListener
{
    public static string[] nameServer;

    public static string[] address;

    public static sbyte serverPriority;

    public static bool[] hasConnected;

    public static short[] port;

    public static int selected;

    public static bool isWait;

    public static Command cmdUpdateServer;

    public static sbyte[] language;

    private Command[] cmd;

    private int nCmdPlay;

    public static string keyDecryptString;

    public static Command cmdDeleteRMS;

    //public static string linkDefault = ModFunc.DecodeByteArrayString(ModFunc.ipString, "69");

//    public static string linkDefault = Decrypt("勶勹動勹勷勸匓匜勲勧勘匝勱匎勹匙勱匎勥匝勰匎勩勗勱勸匓匞勳勨勽匛勳匎勥北勱勧匛匛", 545444);
    //public static string linkDefault = Decrypt("勵匏匜勺勶勸匓匜勲勧勘匝勱匎勹匙勱匎勥匝勰匎勩勗勱勸匓匞勳勨勹勖勳匎勥北勱勧匛匛", 545444);
    //local
    public static string linkDefault = Decrypt("勅劶努劵勂勆勨劧劾勅劺劤劽勛劲勦劾労劥勩勀勛劶务势劵勂劢勀勛劲勤劾労勨勨", 742001);
    //src emti
    //public static string linkDefault = Decrypt("勂勞勩劢勋勄劲勩勀勛劶务劽勛劺勪势勄劥勪劾劵劾勦劾勛勂勪勀勛劾劥势勛劲劧劾労勨勨劽劵劲劮", 742001);
    //public static string linkDefault = Decrypt("勃劣努勠勊勈劥勘劾勅勠勩势労劥勪劾勛勆勦劾勛劲勪劽勛勔勨勀勛劶务势劵勂劢勀勛劲勤勃劣努勠勊勈劥勘劾勛勠勩势労劥勪劾勛勆勦劾勛劲勪劽勛勔勨勀勛劶务势劵勂劣勀勛劲勤劾労勨勨", 742001);
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
    }

    public const sbyte languageVersion = 2;

    public new int keyTouch = -1;

    public static bool stopDownload;

    public static string linkweb = ModFunc.homeUrl;

    public static int countDieConnect;

    public static bool waitToLogin;

    public static int tWaitToLogin;

    public static int[] lengthServer = new int[3];

    public static int ipSelect;

    public static int flagServer;

    public static bool bigOk;

    public static int percent;

    public static string strWait;

    public static int nBig;

    public static int nBg;

    public static int demPercent;

    public static int maxBg;

    public static bool isGetData = false;

    public static Command cmdDownload;

    private Command cmdStart;

    public string dataSize;

    public static int p;

    public static int testConnect = -1;

    public static bool loadScreen;

    public static bool isAutoConect = true;

    public ServerListScreen()
    {
        int num = 4;
        int num2 = num * 32 + 23 + 33;
        if (num2 >= GameCanvas.w)
        {
            num--;
            num2 = num * 32 + 23 + 33;
        }
        initCommand();
        if (!GameCanvas.isTouch)
        {
            selected = 0;
            processInput();
        }
        GameScr.loadCamera(fullmScreen: true, -1, -1);
        GameScr.cmx = 100;
        GameScr.cmy = 200;
        cmdUpdateServer = new Command
        {
            actionChat = delegate (string str)
        {
            string text = str;
            string text2 = str;
            if (text == null)
            {
                text = linkDefault;
            }
            else
            {
                if (text == null && text2 != null)
                {
                    if (text2.Equals(string.Empty) || text2.Length < 20)
                    {
                        text2 = linkDefault;
                    }
                    GetServerList(text2);
                }
                if (text != null && text2 == null)
                {
                    if (text.Equals(string.Empty) || text.Length < 20)
                    {
                        text = linkDefault;
                    }
                    GetServerList(text);
                }
                if (text != null && text2 != null)
                {
                    if (text.Length > text2.Length)
                    {
                        GetServerList(text);
                    }
                    else
                    {
                        GetServerList(text2);
                    }
                }
            }
        }
        };
        ModFunc.DoEncodeIp();
    }

    public static void createDeleteRMS()
    {
        if (cmdDeleteRMS == null)
        {
            if (GameCanvas.serverScreen == null)
            {
                GameCanvas.serverScreen = new ServerListScreen();
            }
            cmdDeleteRMS = new Command(string.Empty, GameCanvas.serverScreen, 14, null);
            cmdDeleteRMS.x = GameCanvas.w - 78;
            cmdDeleteRMS.y = GameCanvas.h - 26;
        }
    }

    private void initCommand()
    {
        nCmdPlay = 0;
        string text = Rms.loadRMSString("acc");
        string text2 = Rms.loadRMSString("pass");
        if (text == null)
        {
            if (Rms.loadRMS("userAo" + ipSelect) != null)
            {
                nCmdPlay = 1;
            }
        }
        else if (text.Equals(string.Empty))
        {
            if (Rms.loadRMS("userAo" + ipSelect) != null)
            {
                nCmdPlay = 1;
            }
        }
        else
        {
            nCmdPlay = 1;
        }
        cmd = new Command[(Main.isIPhone || mGraphics.zoomLevel <= 1) ? (5 + nCmdPlay) : (4 + nCmdPlay)];
        int num = GameCanvas.hh - 15 * cmd.Length + 28;
        for (int i = 0; i < cmd.Length; i++)
        {
            switch (i)
            {
                case 0:
                    cmd[0] = new Command(string.Empty, this, 3, null);
                    if (text == null)
                    {
                        cmd[0].caption = mResources.playNew;
                        if (Rms.loadRMS("userAo" + ipSelect) != null)
                        {
                            cmd[0].caption = mResources.choitiep;
                        }
                        break;
                    }
                    if (text.Equals(string.Empty))
                    {
                        cmd[0].caption = mResources.playNew;
                        if (Rms.loadRMS("userAo" + ipSelect) != null)
                        {
                            cmd[0].caption = mResources.choitiep;
                        }
                        break;
                    }
                    cmd[0].caption = mResources.playAcc + ": " + (!text.Contains(',') ? text : "");
                    if (cmd[0].caption.Length > 23)
                    {
                        cmd[0].caption = cmd[0].caption.Substring(0, 23);
                        cmd[0].caption += "...";
                    }
                    break;
                case 1:
                    if (nCmdPlay == 1)
                    {
                        cmd[1] = new Command(string.Empty, this, 10100, null);
                        cmd[1].caption = mResources.playNew;
                    }
                    else
                    {
                        cmd[1] = new Command(mResources.change_account, this, 7, null);
                    }
                    break;
                case 2:
                    if (nCmdPlay == 1)
                    {
                        cmd[2] = new Command(mResources.change_account, this, 7, null);
                    }
                    else
                    {
                        cmd[2] = new Command(string.Empty, this, 17, null);
                    }
                    break;
                case 3:
                    if (nCmdPlay == 1)
                    {
                        cmd[3] = new Command(string.Empty, this, 17, null);
                    }
                    else
                    {
                        cmd[3] = new Command(ModFunc.ipServer, ModFunc.GI(), 8, null);
                    }
                    break;
                case 4:
                    if (nCmdPlay == 1)
                    {
                        cmd[4] = new Command(ModFunc.ipServer, ModFunc.GI(), 8, null);
                    }
                    else
                    {
                        cmd[4] = new Command(mResources.option, this, 8, null);
                    }
                    break;
                case 5:
                    cmd[5] = new Command(mResources.option, this, 8, null);
                    break;
            }
            cmd[i].y = num;
            cmd[i].setType();
            cmd[i].x = (GameCanvas.w - cmd[i].w) / 2;
            num += 30;
        }
    }

    public static void doUpdateServer()
    {
        if (cmdUpdateServer == null && GameCanvas.serverScreen == null)
        {
            GameCanvas.serverScreen = new ServerListScreen();
        }
        Net.connectHTTP2(linkDefault, cmdUpdateServer);
    }

    public static void GetServerList(string str)
    {
        lengthServer = new int[3];
        string[] array = Res.split(str.Trim(), ",", 0);
        mResources.loadLanguague(sbyte.Parse(array[array.Length - 2]));
        nameServer = new string[array.Length - 2];
        address = new string[array.Length - 2];
        port = new short[array.Length - 2];
        language = new sbyte[array.Length - 2];
        hasConnected = new bool[2];
        for (int i = 0; i < array.Length - 2; i++)
        {
            string[] array2 = Res.split(array[i].Trim(), ":", 0);
            nameServer[i] = array2[0];
            address[i] = array2[1];
            port[i] = short.Parse(array2[2]);
            language[i] = sbyte.Parse(array2[3].Trim());
            lengthServer[language[i]]++;
        }
        serverPriority = sbyte.Parse(array[^1]);
        SaveIP();
    }
    Texture2D texture;
    public override void paint(mGraphics g)
    {
        //if (!loadScreen)
        //{
        //    g.setColor(0);
        //    g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
        //}
        //else
        //{
        //    GameCanvas.paintBGGameScr(g);
        //}
        if (texture == null)
        {
            texture = Resources.Load("res/logo/bg") as Texture2D;
            return;
        }
        GUI.DrawTexture(new Rect(0,0,ScaleGUI.WIDTH,ScaleGUI.HEIGHT),texture);
        int num2 = 2;
        mFont.tahoma_7_white.drawStringBorder(g, "v" + GameMidlet.VERSION + " (x" + mGraphics.zoomLevel + ")", GameCanvas.w - 2, num2 + 15, 1, mFont.tahoma_7_grey);
        string empty = string.Empty;
        empty = (testConnect != 0) ? (empty + nameServer[ipSelect] + " connected") : (empty + nameServer[ipSelect] + " disconnect");
        if (mSystem.isTest)
        {
            mFont.tahoma_7_white.drawString(g, empty, GameCanvas.w - 2, num2 + 15 + 15, 1, mFont.tahoma_7_grey);
        }
        if (!isGetData || loadScreen)
        {
            if (mSystem.clientType == 1 && !GameCanvas.isTouch)
            {
                mFont.tahoma_7_white.drawStringBorder(g, linkweb, GameCanvas.w - 2, GameCanvas.h - 15, 1, mFont.tahoma_7_grey);
            }
            else
            {
                mFont.tahoma_7_white.drawStringBorder(g, linkweb, GameCanvas.w - 2, num2, 1, mFont.tahoma_7_grey);
            }
        }
        else
        {
            mFont.tahoma_7_white.drawStringBorder(g, linkweb, GameCanvas.w - 2, num2, 1, mFont.tahoma_7_grey);
        }

        _ = (GameCanvas.w < 200) ? 160 : 180;
        if (cmdDeleteRMS != null)
        {
            mFont.tahoma_7_white.drawStringBorder(g, mResources.xoadulieu, GameCanvas.w - 2, GameCanvas.h - 15, 1, mFont.tahoma_7_grey);
        }
        if (GameCanvas.currentDialog == null)
        {
            if (!loadScreen)
            {
                if (!bigOk)
                {
                    //g.drawImageScale(ModFunc.imgBg, 0, 0, GameCanvas.w, GameCanvas.h, 0);
                    //g.drawImage(SplashScr.imgLogo, GameCanvas.hw, GameCanvas.hh - 32, 3);

                    int imgW = ModFunc.imgLogoBig.getWidth() * mGraphics.zoomLevel / 4;
                    int imgH = ModFunc.imgLogoBig.getHeight() * mGraphics.zoomLevel / 4;
                    g.drawImageScale(ModFunc.imgLogoBig, GameCanvas.hw - (imgW / 2), GameCanvas.hh - (imgH / 2) - 32, imgW, imgH);

                    if (!isGetData)
                    {
                        mFont.tahoma_7b_white.drawString(g, mResources.taidulieudechoi, GameCanvas.hw, GameCanvas.hh + 24, 2);
                        cmdDownload?.paint(g);
                    }
                    else
                    {
                        cmdDownload?.paint(g);
                        GameScr.paintOngMauPercent(GameScr.frBarPow20, GameScr.frBarPow21, GameScr.frBarPow22, GameCanvas.w / 2 - 50, GameCanvas.hh + 45, 100, 100f, g);
                        GameScr.paintOngMauPercent(GameScr.frBarPow0, GameScr.frBarPow1, GameScr.frBarPow2, GameCanvas.w / 2 - 50, GameCanvas.hh + 45, 100, percent, g);
                    }
                }
            }
            else
            {
                int num4 = GameCanvas.hh - 15 * cmd.Length - 15;
                if (num4 < 25)
                {
                    num4 = 25;
                }
                if (ModFunc.imgLogoBig != null)
                {
                    //g.drawImage(SplashScr.imgLogo, GameCanvas.hw, num4, 3);

                    int imgW = ModFunc.imgLogoBig.getWidth() * mGraphics.zoomLevel / 4;
                    int imgH = ModFunc.imgLogoBig.getHeight() * mGraphics.zoomLevel / 4;
                    g.drawImageScale(ModFunc.imgLogoBig, GameCanvas.hw - (imgW / 2), num4 - (imgH / 2), imgW, imgH);
                }
                for (int i = 0; i < cmd.Length; i++)
                {
                    cmd[i].paint(g);
                }
                g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
                if (testConnect == -1)
                {
                    if (GameCanvas.gameTick % 20 > 10)
                    {
                        g.drawRegion(GameScr.imgRoomStat, 0, 14, 7, 7, 0, (GameCanvas.w - mFont.tahoma_7b_dark.getWidth(cmd[2 + nCmdPlay].caption) >> 1) - 10, cmd[2 + nCmdPlay].y + 10, 0);
                    }
                }
                else
                {
                    g.drawRegion(GameScr.imgRoomStat, 0, testConnect * 7, 7, 7, 0, (GameCanvas.w - mFont.tahoma_7b_dark.getWidth(cmd[2 + nCmdPlay].caption) >> 1) - 10, cmd[2 + nCmdPlay].y + 9, 0);
                }
            }
        }
        base.paint(g);
    }

    public void selectServer()
    {
        flagServer = 100;
        GameCanvas.startWaitDlg(mResources.PLEASEWAIT);
        Session_ME.gI().close();
        GameMidlet.IP = address[ipSelect];
        GameMidlet.PORT = port[ipSelect];
        GameMidlet.LANGUAGE = language[ipSelect];
        Rms.saveRMSInt("svselect", ipSelect);
        if (language[ipSelect] != mResources.language)
        {
            mResources.loadLanguague(language[ipSelect]);
        }
        LoginScr.serverName = nameServer[ipSelect];
        initCommand();
        loadScreen = true;
        countDieConnect = 0;
        testConnect = -1;
        isAutoConect = true;
    }

    public override void update()
    {
        if (ModFunc.AutoLogin())
        {
            return;
        }
        if (waitToLogin)
        {
            tWaitToLogin++;
            if (tWaitToLogin == 50)
            {
                GameCanvas.serverScreen.selectServer();
            }
            if (tWaitToLogin == 100)
            {
                GameCanvas.loginScr ??= new LoginScr();
                GameCanvas.loginScr.doLogin();
                Service.gI().finishUpdate();
                waitToLogin = false;
            }
        }
        if (flagServer > 0)
        {
            flagServer--;
            if (flagServer == 0)
            {
                GameCanvas.endDlg();
            }
            if (testConnect == 2)
            {
                flagServer = 0;
                GameCanvas.endDlg();
            }
        }
        if (flagServer <= 0 && isAutoConect)
        {
            countDieConnect++;
            if (countDieConnect > 100000)
            {
                countDieConnect = 0;
            }
        }
        for (int i = 0; i < cmd.Length; i++)
        {
            if (i == selected)
            {
                cmd[i].isFocus = true;
            }
            else
            {
                cmd[i].isFocus = false;
            }
        }
        GameScr.cmx++;
        if (!loadScreen && (bigOk || percent == 100))
        {
            cmdDownload = null;
        }
        base.update();
        if (Char.isLoadingMap || !loadScreen || !isAutoConect || GameCanvas.currentScreen != this || testConnect == 2)
        {
            return;
        }
        if (countDieConnect < 5)
        {
            if (flagServer <= 0)
            {
                flagServer = 100;
                GameCanvas.startWaitDlg(mResources.PLEASEWAIT);
                GameCanvas.connect();
            }
        }
        else if (!Session_ME.gI().isConnected())
        {
            if (flagServer <= 0)
            {
                Command cmdYes = new(mResources.YES, GameCanvas.serverScreen, 18, null);
                Command cmdNo = new(mResources.NO, GameCanvas.serverScreen, 19, null);
                GameCanvas.startYesNoDlg(mResources.maychutathoacmatsong + ". " + mResources.confirmChangeServer, cmdYes, cmdNo);
                flagServer = 100;
            }
        }
        else if (flagServer <= 0)
        {
            countDieConnect = 0;
        }
    }

    private void processInput()
    {
        if (loadScreen)
        {
            center = new Command(string.Empty, this, cmd[selected].idAction, null);
        }
        else
        {
            center = cmdDownload;
        }
    }

    public static void updateDeleteData()
    {
        if (cmdDeleteRMS != null && cmdDeleteRMS.isPointerPressInside())
        {
            cmdDeleteRMS.performAction();
        }
    }

    public override void updateKey()
    {
        if (GameCanvas.isTouch)
        {
            updateDeleteData();
            if (!loadScreen)
            {
                if (cmdDownload != null && cmdDownload.isPointerPressInside())
                {
                    cmdDownload.performAction();
                }
                base.updateKey();
                return;
            }
            for (int i = 0; i < cmd.Length; i++)
            {
                if (cmd[i] == null || !cmd[i].isPointerPressInside())
                {
                    continue;
                }
                if (testConnect == -1 || testConnect == 0)
                {
                    if (cmd[i].caption.IndexOf(mResources.server) != -1)
                    {
                        cmd[i].performAction();
                    }
                }
                else
                {
                    cmd[i].performAction();
                }
            }
        }
        else if (loadScreen)
        {
            if (GameCanvas.keyPressed[8])
            {
                int num = ((mGraphics.zoomLevel <= 1) ? 4 : 2);
                GameCanvas.keyPressed[8] = false;
                selected++;
                if (selected > num)
                {
                    selected = 0;
                }
                processInput();
            }
            if (GameCanvas.keyPressed[2])
            {
                int num2 = ((mGraphics.zoomLevel <= 1) ? 4 : 2);
                GameCanvas.keyPressed[2] = false;
                selected--;
                if (selected < 0)
                {
                    selected = num2;
                }
                processInput();
            }
        }
        if (!isWait)
        {
            base.updateKey();
        }
    }

    public static void SaveIP()
    {
        try
        {
            Rms.saveRMSString("NRlink2", ModFunc.EncodeStringToByteArrayString(linkDefault, "69"));
            SplashScr.loadIP();
        }
        catch (Exception)
        {
        }
    }

    public static void SaveIPNew(string ip)
    {
        try
        {
            Rms.saveRMSString("NRlink2", ModFunc.EncodeStringToByteArrayString(ip, "69"));
            SplashScr.loadIP();
        }
        catch (Exception)
        {
        }
    }

    public static bool allServerConnected()
    {
        for (int i = 0; i < 2; i++)
        {
            if (!hasConnected[i])
            {
                return false;
            }
        }
        return true;
    }

    public static void LoadIP()
    {
        string load = Rms.loadRMSString("NRlink2");
        if (load == null || load.Length == 0)
        {
            GetServerList(linkDefault);
            return;
        }
        string ip = ModFunc.DecodeByteArrayString(load, "69");
        if (ip == null || ip.Length == 0)
        {
            GetServerList(linkDefault);
            return;
        }
        try
        {
            lengthServer = new int[3];
            mResources.loadLanguague(mResources.VIETNAM);
            string[] serverList = ip.Split(":0");
            int serverLength = serverList.Length - 1;

            nameServer = new string[serverLength];
            address = new string[serverLength];
            port = new short[serverLength];
            language = new sbyte[serverLength];

            for (int i = 0; i < serverLength; i++)
            {
                string[] comps = serverList[i].Trim(':').Trim(',').Split(':');

                nameServer[i] = comps[0];
                address[i] = comps[1];
                port[i] = short.Parse(comps[2]);
                language[i] = mResources.VIETNAM;
                lengthServer[language[i]]++;
            }
            serverPriority = 0;
            SplashScr.loadIP();
        }
        catch (Exception)
        {
            Rms.saveRMSString("NRlink2", ModFunc.EncodeStringToByteArrayString(linkDefault, "69"));
        }
    }

    public override void switchToMe()
    {
        EffectManager.remove();
        GameScr.cmy = 0;
        GameScr.cmx = 0;
        initCommand();
        isWait = false;
        GameCanvas.loginScr = null;
        string text = Rms.loadRMSString("ResVersion");
        int num = (text == null || !(text != string.Empty)) ? (-1) : int.Parse(text);
        if (num > 0)
        {
            loadScreen = true;
            GameCanvas.loadBG(18);
        }
        bigOk = true;
        cmd[2 + nCmdPlay].caption = mResources.server + ": " + nameServer[ipSelect];
        center = new Command(string.Empty, this, cmd[selected].idAction, null);
        cmd[1 + nCmdPlay].caption = mResources.change_account;
        if (cmd.Length == 5 + nCmdPlay)
        {
            cmd[4 + nCmdPlay].caption = mResources.option;
        }
        Char.isLoadingMap = false;
        ModFunc.startAutoItem = false;
        mSystem.resetCurInapp();
        base.switchToMe();
    }

    public void switchToMe2()
    {
        GameScr.cmy = 0;
        GameScr.cmx = 0;
        initCommand();
        isWait = false;
        GameCanvas.loginScr = null;
        string text = Rms.loadRMSString("ResVersion");
        int num = (text == null || !(text != string.Empty)) ? (-1) : int.Parse(text);
        if (num > 0)
        {
            loadScreen = true;
            GameCanvas.loadBG(18);
        }
        bigOk = true;
        cmd[2 + nCmdPlay].caption = mResources.server + ": " + nameServer[ipSelect];
        center = new Command(string.Empty, this, cmd[selected].idAction, null);
        cmd[1 + nCmdPlay].caption = mResources.change_account;
        if (cmd.Length == 5 + nCmdPlay)
        {
            cmd[4 + nCmdPlay].caption = mResources.option;
        }
        mSystem.resetCurInapp();
        base.switchToMe();
    }

    public void connectOk()
    {
    }

    public void cancel()
    {
        if (GameCanvas.serverScreen == null)
        {
            GameCanvas.serverScreen = new ServerListScreen();
        }
        demPercent = 0;
        percent = 0;
        stopDownload = true;
        GameCanvas.serverScreen.show2();
        isGetData = false;
        cmdDownload.isFocus = true;
        center = new Command(string.Empty, this, 2, null);
    }

    public void perform(int idAction, object p)
    {
        if (idAction == 1000)
        {
            GameCanvas.connect();
        }
        else if (idAction == 1 || idAction == 4)
        {
            Session_ME.gI().close();
            isAutoConect = false;
            countDieConnect = 0;
            loadScreen = true;
            testConnect = 0;
            isGetData = false;
            Rms.clearAll();
            switchToMe();
        }
        else if (idAction == 2)
        {
            stopDownload = false;
            cmdDownload = new Command(mResources.huy, this, 4, null);
            cmdDownload.x = GameCanvas.w / 2 - mScreen.cmdW / 2;
            cmdDownload.y = GameCanvas.hh + 65;
            right = null;
            if (!GameCanvas.isTouch)
            {
                cmdDownload.x = GameCanvas.w / 2 - mScreen.cmdW / 2;
                cmdDownload.y = GameCanvas.h - mScreen.cmdH - 1;
            }
            center = new Command(string.Empty, this, 4, null);
            if (!isGetData)
            {
                Service.gI().updateData();
                Service.gI().getResource(1, null);
                if (!GameCanvas.isTouch)
                {
                    cmdDownload.isFocus = true;
                    center = new Command(string.Empty, this, 4, null);
                }
                isGetData = true;
            }
        }
        else if (idAction == 3)
        {
            if (GameCanvas.loginScr == null)
            {
                GameCanvas.loginScr = new LoginScr();
            }
            GameCanvas.loginScr.switchToMe();
            bool flag = Rms.loadRMSString("acc") != null && ((!Rms.loadRMSString("acc").Equals(string.Empty)) ? true : false);
            bool flag2 = Rms.loadRMSString("userAo" + ipSelect) != null && ((!Rms.loadRMSString("userAo" + ipSelect).Equals(string.Empty)) ? true : false);
            if (!flag && !flag2)
            {
                GameCanvas.connect();
                string text = Rms.loadRMSString("userAo" + ipSelect);
                if (text == null || text.Equals(string.Empty))
                {
                    Service.gI().login2(string.Empty);
                }
                else
                {
                    GameCanvas.loginScr.isLogin2 = true;
                    GameCanvas.connect();
                    Service.gI().setClientType();
                    Service.gI().login(text, string.Empty, GameMidlet.VERSION, 1);
                }
                if (Session_ME.connected)
                {
                    GameCanvas.startWaitDlg();
                }
                else
                {
                    GameCanvas.startOKDlg(mResources.maychutathoacmatsong);
                }
            }
            else
            {
                GameCanvas.loginScr.doLogin();
            }
            LoginScr.serverName = nameServer[ipSelect];
        }
        else if (idAction == 10100)
        {
            if (GameCanvas.loginScr == null)
            {
                GameCanvas.loginScr = new LoginScr();
            }
            GameCanvas.loginScr.switchToMe();
            GameCanvas.connect();
            Service.gI().login2(string.Empty);
            GameCanvas.startWaitDlg();
            LoginScr.serverName = nameServer[ipSelect];
        }
        else if (idAction == 5)
        {
            doUpdateServer();
            if (nameServer.Length == 1)
            {
                return;
            }
            MyVector myVector = new MyVector(string.Empty);
            for (int i = 0; i < nameServer.Length; i++)
            {
                myVector.addElement(new Command(nameServer[i], this, 6, null));
            }
            GameCanvas.menu.startAt(myVector, 0);
            if (!GameCanvas.isTouch)
            {
                GameCanvas.menu.menuSelectedItem = ipSelect;
            }
        }
        else if (idAction == 6)
        {
            ipSelect = GameCanvas.menu.menuSelectedItem;
            selectServer();
        }
        else if (idAction == 7)
        {
            if (GameCanvas.loginScr == null)
            {
                GameCanvas.loginScr = new LoginScr();
            }
            GameCanvas.loginScr.switchToMe();
        }
        else if (idAction == 8)
        {
            bool lowGraphic = Rms.loadRMSInt("lowGraphic") == 1;
            MyVector myVector2 = new MyVector("cau hinh");
            myVector2.addElement(new Command(mResources.cauhinhthap, this, 9, null));
            myVector2.addElement(new Command(mResources.cauhinhcao, this, 10, null));
            GameCanvas.menu.startAt(myVector2, 0);
            if (lowGraphic)
            {
                GameCanvas.menu.menuSelectedItem = 0;
            }
            else
            {
                GameCanvas.menu.menuSelectedItem = 1;
            }
        }
        else if (idAction == 9)
        {
            Rms.saveRMSInt("lowGraphic", 1);
            GameCanvas.startOK(mResources.plsRestartGame, 8885, null);
        }
        else if (idAction == 10)
        {
            Rms.saveRMSInt("lowGraphic", 0);
            GameCanvas.startOK(mResources.plsRestartGame, 8885, null);
        }
        else if (idAction == 11)
        {
            if (GameCanvas.loginScr == null)
            {
                GameCanvas.loginScr = new LoginScr();
            }
            GameCanvas.loginScr.switchToMe();
            string text2 = Rms.loadRMSString("userAo" + ipSelect);
            if (text2 == null || text2.Equals(string.Empty))
            {
                Service.gI().login2(string.Empty);
            }
            else
            {
                GameCanvas.loginScr.isLogin2 = true;
                GameCanvas.connect();
                Service.gI().setClientType();
                Service.gI().login(text2, string.Empty, GameMidlet.VERSION, 1);
            }
            GameCanvas.startWaitDlg(mResources.PLEASEWAIT);
        }
        else if (idAction == 12)
        {
            GameMidlet.instance.exit();
        }
        else if (idAction == 13 && (!isGetData || loadScreen))
        {
            switch (mSystem.clientType)
            {
                case 1:
                    mSystem.callHotlineJava();
                    break;
                case 3:
                case 5:
                    mSystem.callHotlineIphone();
                    break;
                case 6:
                    mSystem.callHotlineWindowsPhone();
                    break;
                case 4:
                    mSystem.callHotlinePC();
                    break;
            }
        }
        else if (idAction == 14)
        {
            Command cmdYes = new Command(mResources.YES, GameCanvas.serverScreen, 15, null);
            Command cmdNo = new Command(mResources.NO, GameCanvas.serverScreen, 16, null);
            GameCanvas.startYesNoDlg(mResources.deletaDataNote, cmdYes, cmdNo);
        }
        else if (idAction == 15)
        {
            Rms.clearAll();
            GameCanvas.startOK(mResources.plsRestartGame, 8885, null);
        }
        else if (idAction == 16)
        {
            InfoDlg.hide();
            GameCanvas.currentDialog = null;
        }
        else if (idAction == 17)
        {
            if (GameCanvas.serverScr == null)
            {
                GameCanvas.serverScr = new ServerScr();
            }
            GameCanvas.serverScr.switchToMe();
        }
        else if (idAction == 18)
        {
            GameCanvas.endDlg();
            InfoDlg.hide();
            if (GameCanvas.serverScr == null)
            {
                GameCanvas.serverScr = new ServerScr();
            }
            GameCanvas.serverScr.switchToMe();
        }
        else if (idAction == 19)
        {
            if (mSystem.clientType == 1)
            {
                InfoDlg.hide();
                GameCanvas.currentDialog = null;
            }
            else
            {
                countDieConnect = 0;
                testConnect = 0;
                isAutoConect = true;
            }
        }
    }

    public void init()
    {
        if (!loadScreen)
        {
            cmdDownload = new Command(mResources.taidulieu, this, 2, null);
            cmdDownload.isFocus = true;
            cmdDownload.x = GameCanvas.w / 2 - cmdW / 2;
            cmdDownload.y = GameCanvas.hh + 45;
            if (cmdDownload.y > GameCanvas.h - 26)
            {
                cmdDownload.y = GameCanvas.h - 26;
            }
            cmdDownload.performAction();
        }
        if (!GameCanvas.isTouch)
        {
            selected = 0;
            processInput();
        }
    }

    public void show2()
    {
        GameScr.cmx = 0;
        GameScr.cmy = 0;
        initCommand();
        loadScreen = false;
        percent = 0;
        bigOk = false;
        isGetData = false;
        p = 0;
        demPercent = 0;
        strWait = mResources.PLEASEWAIT;
        Char.isLoadingMap = false;
        init();
        base.switchToMe();
    }
}
