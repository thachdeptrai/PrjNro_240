using System;
using System.Security.Cryptography;
using Assets.src.e;
using Assets.src.f;
using Assets.src.g;
using Mod.XMAP;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Controller : IMessageHandler
{
    protected static Controller me;

    protected static Controller me2;

    public Message messWait;

    public static bool isLoadingData = false;

    public static bool isConnectOK;

    public static bool isConnectionFail;

    public static bool isDisconnected;
    private float demCount;
    public static bool isMain;

    private int move;

    private int total;

    public static bool isStopReadMessage;

    public static MyHashTable frameHT_NEWBOSS = new MyHashTable();

    public const sbyte PHUBAN_TYPE_CHIENTRUONGNAMEK = 0;

    public const sbyte PHUBAN_START = 0;

    public const sbyte PHUBAN_UPDATE_POINT = 1;

    public const sbyte PHUBAN_END = 2;

    public const sbyte PHUBAN_LIFE = 4;

    public const sbyte PHUBAN_INFO = 5;

    public static Controller gI()
    {
        if (me == null)
        {
            me = new Controller();
        }
        return me;
    }

    public static Controller gI2()
    {
        if (me2 == null)
        {
            me2 = new Controller();
        }
        return me2;
    }

    public void onConnectOK(bool isMain1)
    {
        isMain = isMain1;
        mSystem.onConnectOK();
    }

    public void onConnectionFail(bool isMain1)
    {
        isMain = isMain1;
        mSystem.onConnectionFail();
    }

    public void onDisconnected(bool isMain1)
    {
        isMain = isMain1;
        mSystem.onDisconnected();
    }

    public void requestItemPlayer(Message msg)
    {
        try
        {
            int num = msg.reader().readUnsignedByte();
            Item item = GameScr.currentCharViewInfo.arrItemBody[num];
            item.saleCoinLock = msg.reader().readInt();
            item.sys = msg.reader().readByte();
            item.options = new MyVector();
            try
            {
                while (true)
                {
                    item.options.addElement(new ItemOption(msg.reader().readUnsignedByte(), msg.reader().readUnsignedShort()));
                }
            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception ex2)
        {
        }
    }

    public void onMessage(Message msg)
    {
        GameCanvas.debugSession.removeAllElements();
        GameCanvas.debug("SA1", 2);
        
        try
        {
            // Debug.Log("<<<Read cmd= " + msg.command);
           
            Char @char = null;
            Mob mob = null;
            MyVector myVector = new MyVector();
            int num = 0;
            GameCanvas.timeLoading = 15;
            Controller2.readMessage(msg);
            switch (msg.command)
            {
                case 70:
                    QuayTamBao.receiveMsg(msg);
                    break;
                case 0:
                    readLogin(msg);
                    break;
                case 24:
                    read_opt(msg);
                    break;
                case 20:
                    phuban_Info(msg);
                    break;
                case 66:
                    readGetImgByName(msg);
                    break;
                case 65:
                    {
                        sbyte b67 = msg.reader().readSByte();
                        string text6 = msg.reader().readUTF();
                        short num162 = msg.reader().readShort();
                        if (ItemTime.isExistMessage(b67))
                        {
                            if (num162 != 0)
                            {
                                ItemTime.getMessageById(b67).initTimeText(b67, text6, num162);
                            }
                            else
                            {
                                GameScr.textTime.removeElement(ItemTime.getMessageById(b67));
                            }
                        }
                        else
                        {
                            ItemTime itemTime = new ItemTime();
                            itemTime.initTimeText(b67, text6, num162);
                            GameScr.textTime.addElement(itemTime);
                        }
                        break;
                    }
                case 112:
                    {
                        sbyte type = msg.reader().readByte();
                        if (type == 0)
                        {
                            Panel.spearcialImage = msg.reader().readShort();
                            Panel.specialInfo = msg.reader().readUTF(); // name
                            ModFunc.GI().UpdateIntrinsicInfo(Panel.specialInfo);
                        }
                        else
                        {
                            if (type != 1)
                            {
                                break;
                            }
                            sbyte tabSize = msg.reader().readByte();
                            Char.myCharz().infoSpeacialSkill = new string[tabSize][];
                            Char.myCharz().imgSpeacialSkill = new short[tabSize][];
                            GameCanvas.panel.speacialTabName = new string[tabSize][];
                            for (int j = 0; j < tabSize; j++)
                            {
                                GameCanvas.panel.speacialTabName[j] = new string[2];
                                string[] array10 = Res.split(msg.reader().readUTF(), "\n", 0);
                                if (array10.Length == 2)
                                {
                                    GameCanvas.panel.speacialTabName[j] = array10;
                                }
                                if (array10.Length == 1)
                                {
                                    GameCanvas.panel.speacialTabName[j][0] = array10[0];
                                    GameCanvas.panel.speacialTabName[j][1] = string.Empty;
                                }
                                int size = msg.reader().readByte();
                                Char.myCharz().infoSpeacialSkill[j] = new string[size];
                                Char.myCharz().imgSpeacialSkill[j] = new short[size];
                                for (int i = 0; i < size; i++)
                                {
                                    Char.myCharz().imgSpeacialSkill[j][i] = msg.reader().readShort();
                                    Char.myCharz().infoSpeacialSkill[j][i] = msg.reader().readUTF();
                                }
                            }
                            GameCanvas.panel.tabName[25] = GameCanvas.panel.speacialTabName;
                            GameCanvas.panel.setTypeSpeacialSkill();
                            GameCanvas.panel.show();
                        }
                        break;
                    }
                case -98:
                    {
                        sbyte b39 = msg.reader().readByte();
                        GameCanvas.menu.showMenu = false;
                        if (b39 == 0)
                        {
                            GameCanvas.startYesNoDlg(msg.reader().readUTF(), new Command(mResources.YES, GameCanvas.instance, 888397, msg.reader().readUTF()), new Command(mResources.NO, GameCanvas.instance, 888396, null));
                        }
                        break;
                    }
                case -97:
                    Char.myCharz().cNangdong = msg.reader().readInt();
                    break;
                case -96:
                    {
                        sbyte typeTop = msg.reader().readByte();
                        GameCanvas.panel.vTop.removeAllElements();
                        string topName = msg.reader().readUTF();
                        sbyte size = msg.reader().readByte();
                        for (int n = 0; n < size; n++)
                        {
                            int rank = msg.reader().readInt();
                            int pId = msg.reader().readInt();
                            short headID = msg.reader().readShort();
                            short headICON = msg.reader().readShort();
                            short body = msg.reader().readShort();
                            short leg = msg.reader().readShort();
                            string name = msg.reader().readUTF();
                            string info2 = msg.reader().readUTF();
                            TopInfo topInfo = new()
                            {
                                rank = rank,
                                headID = headID,
                                headICON = headICON,
                                body = body,
                                leg = leg,
                                name = name,
                                info = info2,
                                info2 = msg.reader().readUTF(),
                                pId = pId
                            };
                            GameCanvas.panel.vTop.addElement(topInfo);
                        }
                        GameCanvas.panel.topName = topName;
                        GameCanvas.panel.setTypeTop(typeTop);
                        GameCanvas.panel.show();
                        break;
                    }
                case -94:
                    while (msg.reader().available() > 0)
                    {
                        short num136 = msg.reader().readShort();
                        int num137 = msg.reader().readInt();
                        for (int num138 = 0; num138 < Char.myCharz().vSkill.size(); num138++)
                        {
                            Skill skill = (Skill)Char.myCharz().vSkill.elementAt(num138);
                            if (skill != null && skill.skillId == num136)
                            {
                                if (num137 < skill.coolDown)
                                {
                                    skill.lastTimeUseThisSkill = mSystem.currentTimeMillis() - (skill.coolDown - num137);
                                }
                            }
                        }
                    }
                    break;
                case -95:
                    {
                        sbyte type = msg.reader().readByte();
                        if (type == 0)
                        {
                            int num139 = msg.reader().readInt();
                            short templateId = msg.reader().readShort();
                            double hp = msg.reader().readDouble();
                            SoundMn.gI().explode_1();
                            if (num139 == Char.myCharz().charID)
                            {
                                Char.myCharz().mobMe = new Mob(num139, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, hp, 0, hp, (short)(Char.myCharz().cx + ((Char.myCharz().cdir != 1) ? (-40) : 40)), (short)Char.myCharz().cy, 4, 0)
                                {
                                    isMobMe = true
                                };
                                EffecMn.addEff(new Effect(18, Char.myCharz().mobMe.x, Char.myCharz().mobMe.y, 2, 10, -1));
                                Char.myCharz().tMobMeBorn = 30;
                                GameScr.vMob.addElement(Char.myCharz().mobMe);
                            }
                            else
                            {
                                @char = GameScr.findCharInMap(num139);
                                if (@char != null)
                                {
                                    Mob mob6 = new(num139, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, hp, 0, hp, (short)@char.cx, (short)@char.cy, 4, 0);
                                    mob6.isMobMe = true;
                                    @char.mobMe = mob6;
                                    GameScr.vMob.addElement(@char.mobMe);
                                }
                                else
                                {
                                    Mob mob7 = GameScr.findMobInMap(num139);
                                    if (mob7 == null)
                                    {
                                        mob7 = new Mob(num139, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, hp, 0, hp, -100, -100, 4, 0)
                                        {
                                            isMobMe = true
                                        };
                                        GameScr.vMob.addElement(mob7);
                                    }
                                }
                            }
                        }
                        if (type == 1)
                        {
                            int num141 = msg.reader().readInt();
                            int mobId = msg.reader().readByte();
                            if (num141 == Char.myCharz().charID)
                            {
                                if (GameScr.findMobInMap(mobId) != null)
                                {
                                    Char.myCharz().mobMe.attackOtherMob(GameScr.findMobInMap(mobId));
                                }
                            }
                            else
                            {
                                @char = GameScr.findCharInMap(num141);
                                if (@char != null && GameScr.findMobInMap(mobId) != null)
                                {
                                    @char.mobMe.attackOtherMob(GameScr.findMobInMap(mobId));
                                }
                            }
                        }
                        if (type == 2)
                        {
                            int num142 = msg.reader().readInt();
                            int num143 = msg.reader().readInt();
                            double dameHit = msg.reader().readDouble();
                            double cHPNew = msg.reader().readDouble();
                            if (num142 == Char.myCharz().charID)
                            {
                                @char = GameScr.findCharInMap(num143);
                                if (@char != null)
                                {
                                    @char.cHPNew = cHPNew;
                                    if (Char.myCharz().mobMe.isBusyAttackSomeOne)
                                    {
                                        @char.doInjure(dameHit, 0, isCrit: false, isMob: true);
                                    }
                                    else
                                    {
                                        Char.myCharz().mobMe.dame = dameHit;
                                        Char.myCharz().mobMe.setAttack(@char);
                                    }
                                }
                            }
                            else
                            {
                                mob = GameScr.findMobInMap(num142);
                                if (mob != null)
                                {
                                    if (num143 == Char.myCharz().charID)
                                    {
                                        Char.myCharz().cHPNew = cHPNew;
                                        if (mob.isBusyAttackSomeOne)
                                        {
                                            Char.myCharz().doInjure(dameHit, 0, isCrit: false, isMob: true);
                                        }
                                        else
                                        {
                                            mob.dame = dameHit;
                                            mob.setAttack(Char.myCharz());
                                        }
                                    }
                                    else
                                    {
                                        @char = GameScr.findCharInMap(num143);
                                        if (@char != null)
                                        {
                                            @char.cHPNew = cHPNew;
                                            if (mob.isBusyAttackSomeOne)
                                            {
                                                @char.doInjure(dameHit, 0, isCrit: false, isMob: true);
                                            }
                                            else
                                            {
                                                mob.dame = dameHit;
                                                mob.setAttack(@char);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (type == 3)
                        {
                            int num145 = msg.reader().readInt();
                            int mobId2 = msg.reader().readInt();
                            double hp = msg.reader().readDouble();
                            double dame = msg.reader().readDouble();
                            @char = null;
                            @char = ((Char.myCharz().charID != num145) ? GameScr.findCharInMap(num145) : Char.myCharz());
                            if (@char != null)
                            {
                                mob = GameScr.findMobInMap(mobId2);
                                if (@char.mobMe != null)
                                {
                                    @char.mobMe.attackOtherMob(mob);
                                }
                                if (mob != null)
                                {
                                    mob.hp = hp;
                                    mob.updateHp_bar();
                                    if (dame == 0)
                                    {
                                        mob.x = mob.xFirst;
                                        mob.y = mob.yFirst;
                                        GameScr.startFlyText(mResources.miss, mob.x, mob.y - mob.h, 0, -2, mFont.MISS);
                                    }
                                    else
                                    {
                                        GameScr.startFlyText("-" + dame, mob.x, mob.y - mob.h, 0, -2, mFont.ORANGE);
                                    }
                                }
                            }
                        }
                        if (type == 4)
                        {
                        }
                        if (type == 5)
                        {
                            int num147 = msg.reader().readInt();
                            sbyte b61 = msg.reader().readByte();
                            int mobId3 = msg.reader().readInt();
                            double num148 = msg.reader().readDouble();
                            double hp2 = msg.reader().readDouble();
                            @char = null;
                            @char = ((num147 != Char.myCharz().charID) ? GameScr.findCharInMap(num147) : Char.myCharz());
                            if (@char == null)
                            {
                                return;
                            }
                            if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
                            {
                                @char.setSkillPaint(GameScr.sks[b61], 0);
                            }
                            else
                            {
                                @char.setSkillPaint(GameScr.sks[b61], 1);
                            }
                            Mob mob8 = GameScr.findMobInMap(mobId3);
                            if (@char.cx <= mob8.x)
                            {
                                @char.cdir = 1;
                            }
                            else
                            {
                                @char.cdir = -1;
                            }
                            @char.mobFocus = mob8;
                            mob8.hp = hp2;
                            mob8.updateHp_bar();
                            if (num148 == 0)
                            {
                                mob8.x = mob8.xFirst;
                                mob8.y = mob8.yFirst;
                                GameScr.startFlyText(mResources.miss, mob8.x, mob8.y - mob8.h, 0, -2, mFont.MISS);
                            }
                            else
                            {
                                GameScr.startFlyText("-" + num148, mob8.x, mob8.y - mob8.h, 0, -2, mFont.ORANGE);
                            }
                        }
                        if (type == 6)
                        {
                            int num149 = msg.reader().readInt();
                            if (num149 == Char.myCharz().charID)
                            {
                                Char.myCharz().mobMe.startDie();
                            }
                            else
                            {
                                GameScr.findCharInMap(num149)?.mobMe.startDie();
                            }
                        }
                        if (type != 7)
                        {
                            break;
                        }
                        int num150 = msg.reader().readInt();
                        if (num150 == Char.myCharz().charID)
                        {
                            Char.myCharz().mobMe = null;
                            for (int num151 = 0; num151 < GameScr.vMob.size(); num151++)
                            {
                                if (((Mob)GameScr.vMob.elementAt(num151)).mobId == num150)
                                {
                                    GameScr.vMob.removeElementAt(num151);
                                }
                            }
                            break;
                        }
                        @char = GameScr.findCharInMap(num150);
                        for (int num152 = 0; num152 < GameScr.vMob.size(); num152++)
                        {
                            if (((Mob)GameScr.vMob.elementAt(num152)).mobId == num150)
                            {
                                GameScr.vMob.removeElementAt(num152);
                            }
                        }
                        if (@char != null)
                        {
                            @char.mobMe = null;
                        }
                        break;
                    }
                case -92:
                    Main.typeClient = msg.reader().readByte();
                    if (Rms.loadRMSString("ResVersion") == null)
                    {
                        Rms.clearAll();
                    }
                    Rms.saveRMSInt("clienttype", Main.typeClient);
                    Rms.saveRMSInt("lastZoomlevel", mGraphics.zoomLevel);
                    if (Rms.loadRMSString("ResVersion") == null)
                    {
                        GameCanvas.startOK(mResources.plsRestartGame, 8885, null);
                    }
                    break;
                case -91:
                    {
                        sbyte b37 = msg.reader().readByte();
                        GameCanvas.panel.mapNames = new string[b37];
                        GameCanvas.panel.planetNames = new string[b37];
                        for (int num84 = 0; num84 < b37; num84++)
                        {
                            GameCanvas.panel.mapNames[num84] = msg.reader().readUTF();
                            GameCanvas.panel.planetNames[num84] = msg.reader().readUTF();
                        }
                        AutoXmap.ShowPanelMapTrans();
                        break;
                    }
                case -90:
                    {
                        sbyte b35 = msg.reader().readByte();
                        int num79 = msg.reader().readInt();
                        @char = ((Char.myCharz().charID != num79) ? GameScr.findCharInMap(num79) : Char.myCharz());
                        if (b35 != -1)
                        {
                            short num80 = msg.reader().readShort();
                            short num81 = msg.reader().readShort();
                            short num82 = msg.reader().readShort();
                            sbyte isMonkey = msg.reader().readByte();
                            if (@char != null)
                            {
                                if (@char.charID == num79)
                                {
                                    @char.isMask = true;
                                    @char.isMonkey = isMonkey;
                                    if (@char.isMonkey != 0)
                                    {
                                        @char.isWaitMonkey = false;
                                        @char.isLockMove = false;
                                    }
                                }
                                else if (@char != null)
                                {
                                    @char.isMask = true;
                                    @char.isMonkey = isMonkey;
                                }
                                if (num80 != -1)
                                {
                                    @char.head = num80;
                                }
                                if (num81 != -1)
                                {
                                    @char.body = num81;
                                }
                                if (num82 != -1)
                                {
                                    @char.leg = num82;
                                }
                            }
                        }
                        if (b35 == -1 && @char != null)
                        {
                            @char.isMask = false;
                            @char.isMonkey = 0;
                        }
                        if (@char == null)
                        {
                        }
                        break;
                    }
                case -88:
                    GameCanvas.endDlg();
                    GameCanvas.serverScreen.switchToMe();
                    break;
                case -87:
                    {
                        msg.reader().mark(100000);
                        createData(msg.reader(), isSaveRMS: true);
                        msg.reader().reset();
                        sbyte[] data3 = new sbyte[msg.reader().available()];
                        msg.reader().readFully(ref data3);
                        sbyte[] data4 = new sbyte[1] { GameScr.vcData };
                        Rms.saveRMS("NRdataVersion", data4);
                        LoginScr.isUpdateData = false;
                        if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
                        {
                            GameScr.gI().readDart();
                            GameScr.gI().readEfect();
                            GameScr.gI().readArrow();
                            GameScr.gI().readSkill();
                            Service.gI().clientOk();
                            return;
                        }
                        break;
                    }
                case -86:
                    {
                        sbyte b42 = msg.reader().readByte();
                        if (b42 == 0)
                        {
                            int playerID = msg.reader().readInt();
                            GameScr.gI().giaodich(playerID);
                        }
                        if (b42 == 1)
                        {
                            int num97 = msg.reader().readInt();
                            Char char7 = GameScr.findCharInMap(num97);
                            if (char7 == null)
                            {
                                return;
                            }
                            GameCanvas.panel.setTypeGiaoDich(char7);
                            GameCanvas.panel.show();
                            Service.gI().getPlayerMenu(num97);
                        }
                        if (b42 == 2)
                        {
                            sbyte b43 = msg.reader().readByte();
                            for (int num98 = 0; num98 < GameCanvas.panel.vMyGD.size(); num98++)
                            {
                                Item item2 = (Item)GameCanvas.panel.vMyGD.elementAt(num98);
                                if (item2.indexUI == b43)
                                {
                                    GameCanvas.panel.vMyGD.removeElement(item2);
                                    break;
                                }
                            }
                        }
                        if (b42 == 6)
                        {
                            GameCanvas.panel.isFriendLock = true;
                            if (GameCanvas.panel2 != null)
                            {
                                GameCanvas.panel2.isFriendLock = true;
                            }
                            GameCanvas.panel.vFriendGD.removeAllElements();
                            if (GameCanvas.panel2 != null)
                            {
                                GameCanvas.panel2.vFriendGD.removeAllElements();
                            }
                            int friendMoneyGD = msg.reader().readInt();
                            sbyte b44 = msg.reader().readByte();
                            for (int num99 = 0; num99 < b44; num99++)
                            {
                                Item item3 = new Item();
                                item3.template = ItemTemplates.get(msg.reader().readShort());
                                item3.quantity = msg.reader().readInt();
                                int num100 = msg.reader().readUnsignedByte();
                                if (num100 != 0)
                                {
                                    item3.itemOption = new ItemOption[num100];
                                    for (int num101 = 0; num101 < item3.itemOption.Length; num101++)
                                    {
                                        int num102 = msg.reader().readUnsignedByte();
                                        int param5 = msg.reader().readUnsignedShort();
                                        if (num102 != -1)
                                        {
                                            item3.itemOption[num101] = new ItemOption(num102, param5);
                                            item3.compare = GameCanvas.panel.getCompare(item3);
                                        }
                                    }
                                }
                                if (GameCanvas.panel2 != null)
                                {
                                    GameCanvas.panel2.vFriendGD.addElement(item3);
                                }
                                else
                                {
                                    GameCanvas.panel.vFriendGD.addElement(item3);
                                }
                            }
                            if (GameCanvas.panel2 != null)
                            {
                                GameCanvas.panel2.setTabGiaoDich(isMe: false);
                                GameCanvas.panel2.friendMoneyGD = friendMoneyGD;
                            }
                            else
                            {
                                GameCanvas.panel.friendMoneyGD = friendMoneyGD;
                                if (GameCanvas.panel.currentTabIndex == 2)
                                {
                                    GameCanvas.panel.setTabGiaoDich(isMe: false);
                                }
                            }
                        }
                        if (b42 == 7)
                        {
                            InfoDlg.hide();
                            if (GameCanvas.panel.isShow)
                            {
                                GameCanvas.panel.hide();
                            }
                        }
                        break;
                    }
                case -85:
                    {
                        sbyte b47 = msg.reader().readByte();
                        if (b47 == 0)
                        {
                            int num106 = msg.reader().readUnsignedShort();
                            sbyte[] data2 = new sbyte[num106];
                            msg.reader().read(ref data2, 0, num106);
                            GameScr.imgCapcha = Image.createImage(data2, 0, num106);
                            GameScr.gI().keyInput = "-----";
                            GameScr.gI().strCapcha = msg.reader().readUTF();
                            GameScr.gI().keyCapcha = new int[GameScr.gI().strCapcha.Length];
                            GameScr.gI().mobCapcha = new Mob();
                            GameScr.gI().right = null;
                        }
                        if (b47 == 1)
                        {
                            MobCapcha.isAttack = true;
                        }
                        if (b47 == 2)
                        {
                            MobCapcha.explode = true;
                            GameScr.gI().right = GameScr.gI().cmdFocus;
                        }
                        break;
                    }
                case -112:
                    {
                        sbyte b40 = msg.reader().readByte();
                        if (b40 == 0)
                        {
                            sbyte mobIndex = msg.reader().readByte();
                            GameScr.findMobInMap(mobIndex).clearBody();
                        }
                        if (b40 == 1)
                        {
                            sbyte mobIndex2 = msg.reader().readByte();
                            GameScr.findMobInMap(mobIndex2).setBody(msg.reader().readShort());
                        }
                        break;
                    }
                case -84:
                    {
                        int index2 = msg.reader().readUnsignedByte();
                        Mob mob4 = null;
                        try
                        {
                            mob4 = (Mob)GameScr.vMob.elementAt(index2);
                        }
                        catch (Exception)
                        {
                        }
                        if (mob4 != null)
                        {
                            mob4.maxHp = msg.reader().readInt();
                        }
                        break;
                    }
                case -83:
                    {
                        sbyte b30 = msg.reader().readByte();
                        if (b30 == 0)
                        {
                            int num66 = msg.reader().readShort();
                            int bgRID = msg.reader().readShort();
                            int num67 = msg.reader().readUnsignedByte();
                            int num68 = msg.reader().readInt();
                            string text2 = msg.reader().readUTF();
                            int num69 = msg.reader().readShort();
                            int num70 = msg.reader().readShort();
                            sbyte b31 = msg.reader().readByte();
                            if (b31 == 1)
                            {
                                GameScr.gI().isRongNamek = true;
                            }
                            else
                            {
                                GameScr.gI().isRongNamek = false;
                            }
                            GameScr.gI().xR = num69;
                            GameScr.gI().yR = num70;
                            if (Char.myCharz().charID == num68)
                            {
                                GameCanvas.panel.hideNow();
                                GameScr.gI().activeRongThanEff(isMe: true);
                            }
                            else if (TileMap.mapID == num66 && TileMap.zoneID == num67)
                            {
                                GameScr.gI().activeRongThanEff(isMe: false);
                            }
                            else if (mGraphics.zoomLevel > 1)
                            {
                                GameScr.gI().doiMauTroi();
                            }
                            GameScr.gI().mapRID = num66;
                            GameScr.gI().bgRID = bgRID;
                            GameScr.gI().zoneRID = num67;
                        }
                        if (b30 == 1)
                        {
                            if (TileMap.mapID == GameScr.gI().mapRID && TileMap.zoneID == GameScr.gI().zoneRID)
                            {
                                GameScr.gI().hideRongThanEff();
                            }
                            else
                            {
                                GameScr.gI().isRongThanXuatHien = false;
                                if (GameScr.gI().isRongNamek)
                                {
                                    GameScr.gI().isRongNamek = false;
                                }
                            }
                        }
                        if (b30 != 2)
                        {
                        }
                        break;
                    }
                case -82:
                    {
                        sbyte size = msg.reader().readByte();
                        TileMap.tileIndex = new int[size][][];
                        TileMap.tileType = new int[size][];
                        for (int i = 0; i < size; i++)
                        {
                            sbyte b15 = msg.reader().readByte();
                            TileMap.tileType[i] = new int[b15];
                            TileMap.tileIndex[i] = new int[b15][];
                            for (int j = 0; j < b15; j++)
                            {
                                TileMap.tileType[i][j] = msg.reader().readInt();
                                sbyte b16 = msg.reader().readByte();
                                TileMap.tileIndex[i][j] = new int[b16];
                                for (int k = 0; k < b16; k++)
                                {
                                    TileMap.tileIndex[i][j][k] = msg.reader().readByte();
                                }
                            }
                        }
                        break;
                    }
                case -81:
                    {
                        sbyte b24 = msg.reader().readByte();
                        if (b24 == 0)
                        {
                            string src = msg.reader().readUTF();
                            string src2 = msg.reader().readUTF();
                            GameCanvas.panel.setTypeCombine();
                            GameCanvas.panel.combineInfo = mFont.tahoma_7b_blue.splitFontArray(src, Panel.WIDTH_PANEL);
                            GameCanvas.panel.combineTopInfo = mFont.tahoma_7.splitFontArray(src2, Panel.WIDTH_PANEL);
                            GameCanvas.panel.show();
                        }
                        if (b24 == 1)
                        {
                            GameCanvas.panel.vItemCombine.removeAllElements();
                            sbyte b25 = msg.reader().readByte();
                            for (int num51 = 0; num51 < b25; num51++)
                            {
                                sbyte b26 = msg.reader().readByte();
                                for (int num52 = 0; num52 < Char.myCharz().arrItemBag.Length; num52++)
                                {
                                    Item item = Char.myCharz().arrItemBag[num52];
                                    if (item != null && item.indexUI == b26)
                                    {
                                        item.isSelect = true;
                                        GameCanvas.panel.vItemCombine.addElement(item);
                                    }
                                }
                            }
                            if (GameCanvas.panel.isShow)
                            {
                                GameCanvas.panel.setTabCombine();
                            }
                        }
                        if (b24 == 2)
                        {
                            GameCanvas.panel.combineSuccess = 0;
                            GameCanvas.panel.setCombineEff(0);
                        }
                        if (b24 == 3)
                        {
                            GameCanvas.panel.combineSuccess = 1;
                            GameCanvas.panel.setCombineEff(0);
                        }
                        if (b24 == 4)
                        {
                            short iconID = msg.reader().readShort();
                            GameCanvas.panel.iconID3 = iconID;
                            GameCanvas.panel.combineSuccess = 0;
                            GameCanvas.panel.setCombineEff(1);
                        }
                        if (b24 == 5)
                        {
                            short iconID2 = msg.reader().readShort();
                            GameCanvas.panel.iconID3 = iconID2;
                            GameCanvas.panel.combineSuccess = 0;
                            GameCanvas.panel.setCombineEff(2);
                        }
                        if (b24 == 6)
                        {
                            short iconID3 = msg.reader().readShort();
                            short iconID4 = msg.reader().readShort();
                            GameCanvas.panel.combineSuccess = 0;
                            GameCanvas.panel.setCombineEff(3);
                            GameCanvas.panel.iconID1 = iconID3;
                            GameCanvas.panel.iconID3 = iconID4;
                        }
                        if (b24 == 7)
                        {
                            short iconID5 = msg.reader().readShort();
                            GameCanvas.panel.iconID3 = iconID5;
                            GameCanvas.panel.combineSuccess = 0;
                            GameCanvas.panel.setCombineEff(4);
                        }
                        if (b24 == 8)
                        {
                            GameCanvas.panel.iconID3 = -1;
                            GameCanvas.panel.combineSuccess = 1;
                            GameCanvas.panel.setCombineEff(4);
                        }
                        short num53 = 21;
                        int num54 = 0;
                        int num55 = 0;
                        try
                        {
                            num53 = msg.reader().readShort();
                            num54 = msg.reader().readShort();
                            num55 = msg.reader().readShort();
                            GameCanvas.panel.xS = num54 - GameScr.cmx;
                            GameCanvas.panel.yS = num55 - GameScr.cmy;
                        }
                        catch (Exception)
                        {
                        }
                        for (int num56 = 0; num56 < GameScr.vNpc.size(); num56++)
                        {
                            Npc npc = (Npc)GameScr.vNpc.elementAt(num56);
                            if (npc.template.npcTemplateId == num53)
                            {
                                GameCanvas.panel.xS = npc.cx - GameScr.cmx;
                                GameCanvas.panel.yS = npc.cy - GameScr.cmy;
                                GameCanvas.panel.idNPC = num53;
                                break;
                            }
                        }
                        break;
                    }
                case -80:
                    {
                        sbyte b48 = msg.reader().readByte();
                        InfoDlg.hide();
                        if (b48 == 0)
                        {
                            GameCanvas.panel.vFriend.removeAllElements();
                            int num108 = msg.reader().readUnsignedByte();
                            for (int num109 = 0; num109 < num108; num109++)
                            {
                                Char char9 = new Char();
                                char9.charID = msg.reader().readInt();
                                char9.head = msg.reader().readShort();
                                char9.headICON = msg.reader().readShort();
                                char9.body = msg.reader().readShort();
                                char9.leg = msg.reader().readShort();
                                char9.bag = msg.reader().readUnsignedByte();
                                char9.cName = msg.reader().readUTF();
                                bool isOnline = msg.reader().readBoolean();
                                InfoItem infoItem2 = new InfoItem(mResources.power + ": " + msg.reader().readUTF());
                                infoItem2.charInfo = char9;
                                infoItem2.isOnline = isOnline;
                                GameCanvas.panel.vFriend.addElement(infoItem2);
                            }
                            GameCanvas.panel.setTypeFriend();
                            GameCanvas.panel.show();
                        }
                        if (b48 == 3)
                        {
                            MyVector vFriend = GameCanvas.panel.vFriend;
                            int num110 = msg.reader().readInt();
                            for (int num111 = 0; num111 < vFriend.size(); num111++)
                            {
                                InfoItem infoItem3 = (InfoItem)vFriend.elementAt(num111);
                                if (infoItem3.charInfo != null && infoItem3.charInfo.charID == num110)
                                {
                                    infoItem3.isOnline = msg.reader().readBoolean();
                                    break;
                                }
                            }
                        }
                        if (b48 != 2)
                        {
                            break;
                        }
                        MyVector vFriend2 = GameCanvas.panel.vFriend;
                        int num112 = msg.reader().readInt();
                        for (int num113 = 0; num113 < vFriend2.size(); num113++)
                        {
                            InfoItem infoItem4 = (InfoItem)vFriend2.elementAt(num113);
                            if (infoItem4.charInfo != null && infoItem4.charInfo.charID == num112)
                            {
                                vFriend2.removeElement(infoItem4);
                                break;
                            }
                        }
                        if (GameCanvas.panel.isShow)
                        {
                            GameCanvas.panel.setTabFriend();
                        }
                        break;
                    }
                case -99:
                    {
                        InfoDlg.hide();
                        sbyte b41 = msg.reader().readByte();
                        if (b41 == 0)
                        {
                            GameCanvas.panel.vEnemy.removeAllElements();
                            int num95 = msg.reader().readUnsignedByte();
                            for (int num96 = 0; num96 < num95; num96++)
                            {
                                Char char6 = new Char();
                                char6.charID = msg.reader().readInt();
                                char6.head = msg.reader().readShort();
                                char6.headICON = msg.reader().readShort();
                                char6.body = msg.reader().readShort();
                                char6.leg = msg.reader().readShort();
                                char6.bag = msg.reader().readShort();
                                char6.cName = msg.reader().readUTF();
                                InfoItem infoItem = new InfoItem(msg.reader().readUTF());
                                bool flag8 = msg.reader().readBoolean();
                                infoItem.charInfo = char6;
                                infoItem.isOnline = flag8;
                                GameCanvas.panel.vEnemy.addElement(infoItem);
                            }
                            GameCanvas.panel.setTypeEnemy();
                            GameCanvas.panel.show();
                        }
                        break;
                    }
                case -79:
                    {
                        InfoDlg.hide();
                        int num104 = msg.reader().readInt();
                        Char charMenu = GameCanvas.panel.charMenu;
                        if (charMenu == null)
                        {
                            return;
                        }
                        charMenu.cPower = msg.reader().readLong();
                        charMenu.currStrLevel = msg.reader().readUTF();
                        break;
                    }
                case -93:
                    {
                        short num15 = msg.reader().readShort();
                        BgItem.newSmallVersion = new sbyte[num15];
                        for (int l = 0; l < num15; l++)
                        {
                            BgItem.newSmallVersion[l] = msg.reader().readByte();
                        }
                        break;
                    }
                case -77:
                    {
                        short num93 = msg.reader().readShort();
                        SmallImage.newSmallVersion = new sbyte[num93];
                        SmallImage.maxSmall = num93;
                        SmallImage.imgNew = new Small[num93];
                        for (int num94 = 0; num94 < num93; num94++)
                        {
                            SmallImage.newSmallVersion[num94] = msg.reader().readByte();
                        }
                        break;
                    }
                case -76:
                    {
                        sbyte type = msg.reader().readByte();
                        if (type == 0)
                        {
                            sbyte sz = msg.reader().readByte();
                            if (sz <= 0)
                            {
                                return;
                            }
                            Char.myCharz().arrArchive = new Archivement[sz];
                            for (int m = 0; m < sz; m++)
                            {
                                Char.myCharz().arrArchive[m] = new Archivement
                                {
                                    info1 = m + 1 + ". " + msg.reader().readUTF(),
                                    info2 = msg.reader().readUTF(),
                                    money = msg.reader().readShort(),
                                    isFinish = msg.reader().readBoolean(),
                                    isRecieve = msg.reader().readBoolean()
                                };
                            }
                            GameCanvas.panel.setTypeArchivement();
                            GameCanvas.panel.show();
                        }
                        else if (type == 1)
                        {
                            int idArchive = msg.reader().readUnsignedByte();
                            if (Char.myCharz().arrArchive[idArchive] != null)
                            {
                                Char.myCharz().arrArchive[idArchive].isRecieve = true;
                            }
                        }
                        break;
                    }
                case -74:
                    {
                        if (ServerListScreen.stopDownload)
                        {
                            return;
                        }
                        if (!GameCanvas.isGetResourceFromServer())
                        {
                            Service.gI().getResource(3, null);
                            SmallImage.loadBigRMS();
                            if (Rms.loadRMSString("acc") != null || Rms.loadRMSString("userAo" + ServerListScreen.ipSelect) != null)
                            {
                                LoginScr.isContinueToLogin = true;
                            }
                            GameCanvas.loginScr = new LoginScr();
                            GameCanvas.loginScr.switchToMe();
                            return;
                        }
                        sbyte b38 = msg.reader().readByte();
                        if (b38 == 0)
                        {
                            int num89 = msg.reader().readInt();
                            string text3 = Rms.loadRMSString("ResVersion");
                            int num90 = ((text3 == null || !(text3 != string.Empty)) ? (-1) : int.Parse(text3));
                            if (Session_ME.gI().isCompareIPConnect())
                            {
                                if (num90 == -1 || num90 != num89)
                                {
                                    GameCanvas.serverScreen.show2();
                                }
                                else
                                {
                                    SmallImage.loadBigRMS();
                                    ServerListScreen.loadScreen = true;
                                    if (GameCanvas.currentScreen != GameCanvas.loginScr)
                                    {
                                        GameCanvas.serverScreen.switchToMe();
                                    }
                                }
                            }
                            else
                            {
                                Session_ME.gI().close();
                                ServerListScreen.loadScreen = true;
                                ServerListScreen.isAutoConect = false;
                                ServerListScreen.countDieConnect = 1000;
                                GameCanvas.serverScreen.switchToMe();
                            }
                 //     ServerListScreen.keyDecryptString = msg.reader().readUTF();
                        }
                        if (b38 == 1)
                        {
                            ServerListScreen.strWait = mResources.downloading_data;
                            short nBig = msg.reader().readShort();
                            ServerListScreen.nBig = nBig;
                            Service.gI().getResource(2, null);
                        }
                        if (b38 == 2)
                        {
                            try
                            {
                                isLoadingData = true;
                                GameCanvas.endDlg();
                                ServerListScreen.demPercent++;
                                ServerListScreen.percent = ServerListScreen.demPercent * 100 / ServerListScreen.nBig;
                                string original = msg.reader().readUTF();
                                string[] array8 = Res.split(original, "/", 0);
                                string filename = "x" + mGraphics.zoomLevel + array8[array8.Length - 1];
                                int num91 = msg.reader().readInt();
                                sbyte[] data = new sbyte[num91];
                                msg.reader().read(ref data, 0, num91);
                                Rms.saveRMS(filename, data);
                            }
                            catch (Exception)
                            {
                                GameCanvas.startOK(mResources.pls_restart_game_error, 8885, null);
                            }
                        }
                        if (b38 == 3)
                        {
                            Rms.saveRMSInt("musicSize", ModFunc.musicCount);
                            ModFunc.InitMusic();
                            isLoadingData = false;
                            int num92 = msg.reader().readInt();
                            Rms.saveRMSString("ResVersion", num92 + string.Empty);
                            Service.gI().getResource(3, null);
                            GameCanvas.endDlg();
                            SmallImage.loadBigRMS();
                            mSystem.gcc();
                            ServerListScreen.bigOk = true;
                            ServerListScreen.loadScreen = true;
                            GameScr.gI().loadGameScr();
                            if (GameCanvas.currentScreen != GameCanvas.loginScr)
                            {
                                GameCanvas.serverScreen.switchToMe();
                            }
                        }
                        if (b38 == 4)
                        {
                            string name = msg.reader().readUTF();
                            sbyte[] data = null;
                            try
                            {
                                data = NinjaUtil.readByteArray(msg);
                            }
                            catch (Exception)
                            {
                                data = null;
                            }
                            if (data != null)
                            {
                                ModFunc.musicCount += 1;
                                Rms.saveRMS("music_" + name, data);
                            }
                        }
                        break;
                    }
                case -43:
                    {
                        sbyte itemAction = msg.reader().readByte();
                        sbyte where = msg.reader().readByte();
                        sbyte index = msg.reader().readByte();
                        string info = msg.reader().readUTF();
                        GameCanvas.panel.itemRequest(itemAction, info, where, index);
                        break;
                    }
                case -59:
                    {
                        sbyte typePK = msg.reader().readByte();
                        GameScr.gI().player_vs_player(msg.reader().readInt(), msg.reader().readInt(), msg.reader().readUTF(), typePK);
                        break;
                    }
                case -62:
                    {
                        byte id = msg.reader().readUnsignedByte();
                        sbyte size = msg.reader().readByte();
                        int[] idImage = new int[size];

                        if (size > 0)
                        {
                            for (int i = 0; i < size; i++)
                            {
                                idImage[i] = msg.reader().readShort();
                                if (idImage[i] > 0)
                                {
                                    SmallImage.vKeys.addElement(idImage[i] + string.Empty);
                                }
                            }
                        }

                        short idNew;
                        try
                        {
                            idNew = msg.reader().readShort();
                        }
                        catch
                        {
                            idNew = id;
                        }

                        if (size > 0)
                        {
                            for (int i = 0; i < size; i++)
                            {
                                try
                                {
                                    idImage[i] = msg.reader().readInt();
                                }
                                catch
                                {
                                }
                                if (idImage[i] > 0)
                                {
                                    SmallImage.vKeys.addElement(idImage[i] + string.Empty);
                                }
                            }
                        }

                        ClanImage clanImage = ClanImage.getClanImage(idNew);
                        if (clanImage == null)
                        {
                            break;
                        }
                        clanImage.idImage = idImage;
                        break;
                    }
                case -65:
                    {
                        InfoDlg.hide();
                        int num83 = msg.reader().readInt();
                        sbyte b36 = msg.reader().readByte();
                        if (b36 == 0)
                        {
                            break;
                        }
                        if (Char.myCharz().charID == num83)
                        {
                            isStopReadMessage = true;
                            GameScr.lockTick = 500;
                            GameScr.gI().center = null;
                            if (b36 == 0 || b36 == 1 || b36 == 3)
                            {
                                Teleport p = new Teleport(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().head, Char.myCharz().cdir, 0, isMe: true, (b36 != 1) ? b36 : Char.myCharz().cgender);
                                Teleport.addTeleport(p);
                            }
                            if (b36 == 2)
                            {
                                GameScr.lockTick = 50;
                                Char.myCharz().hide();
                            }
                        }
                        else
                        {
                            Char char5 = GameScr.findCharInMap(num83);
                            if ((b36 == 0 || b36 == 1 || b36 == 3) && char5 != null)
                            {
                                char5.isUsePlane = true;
                                Teleport teleport = new Teleport(char5.cx, char5.cy, char5.head, char5.cdir, 0, isMe: false, (b36 != 1) ? b36 : char5.cgender);
                                teleport.id = num83;
                                Teleport.addTeleport(teleport);
                            }
                            if (b36 == 2)
                            {
                                char5.hide();
                            }
                        }
                        break;
                    }
                case -64:
                    {
                        int num19 = msg.reader().readInt();
                        int num20 = msg.reader().readUnsignedByte();
                        @char = null;
                        @char = ((num19 != Char.myCharz().charID) ? GameScr.findCharInMap(num19) : Char.myCharz());
                        if (@char == null)
                        {
                            return;
                        }
                        @char.bag = num20;
                        for (int num21 = 0; num21 < 54; num21++)
                        {
                            @char.removeEffChar(0, 201 + num21);
                        }
                        if (@char.bag >= 201 && @char.bag < 255)
                        {
                            Effect effect = new Effect(@char.bag, @char, 2, -1, 10, 1);
                            effect.typeEff = 5;
                            @char.addEffChar(effect);
                        }
                        break;
                    }
                case -63:
                    {
                        byte id = msg.reader().readUnsignedByte();
                        sbyte size = msg.reader().readByte();
                        int[] idImages = new int[size];

                        if (size > 0)
                        {
                            for (int i = 0; i < size; i++)
                            {
                                idImages[i] = msg.reader().readShort();
                                // Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaa");
                            }
                            // Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaa");

                        }

                        short idNew;
                        try
                        {
                            idNew = msg.reader().readShort();
                        }
                        catch
                        {
                            idNew = id;
                        }
                        if (size > 0)
                        {
                            for (int i = 0; i < size; i++)
                            {
                                try
                                {
                                    idImages[i] = msg.reader().readInt();
                                }
                                catch
                                {
                                }
                            }
                        }

                        ClanImage clanImage3 = new()
                        {
                            ID = idNew,
                            idImage = idImages
                        };
                        if (size > 0)
                        {
                            ClanImage.idImages.put(id + string.Empty, clanImage3);
                        }
                        break;
                    }
                case -57:
                    {
                        string strInvite = msg.reader().readUTF();
                        int clanID = msg.reader().readInt();
                        int code = msg.reader().readInt();
                        GameScr.gI().clanInvite(strInvite, clanID, code);
                        break;
                    }
                case -51:
                    InfoDlg.hide();
                    readClanMsg(msg, 0);
                    if (GameCanvas.panel.isMessage && GameCanvas.panel.type == 5)
                    {
                        GameCanvas.panel.initTabClans();
                    }
                    break;
                case -53:
                    {
                        InfoDlg.hide();
                        bool flag6 = false;
                        int num74 = msg.reader().readInt();
                        if (num74 == -1)
                        {
                            flag6 = true;
                            Char.myCharz().clan = null;
                            ClanMessage.vMessage.removeAllElements();
                            if (GameCanvas.panel.member != null)
                            {
                                GameCanvas.panel.member.removeAllElements();
                            }
                            if (GameCanvas.panel.myMember != null)
                            {
                                GameCanvas.panel.myMember.removeAllElements();
                            }
                            if (GameCanvas.currentScreen == GameScr.gI())
                            {
                                GameCanvas.panel.setTabClans();
                            }
                            return;
                        }
                        GameCanvas.panel.tabIcon = null;
                        if (Char.myCharz().clan == null)
                        {
                            Char.myCharz().clan = new Clan();
                        }
                        Char.myCharz().clan.ID = num74;
                        Char.myCharz().clan.name = msg.reader().readUTF();
                        Char.myCharz().clan.slogan = msg.reader().readUTF();
                        Char.myCharz().clan.imgID = msg.reader().readUnsignedByte();
                        Char.myCharz().clan.powerPoint = msg.reader().readUTF();
                        Char.myCharz().clan.leaderName = msg.reader().readUTF();
                        Char.myCharz().clan.currMember = msg.reader().readUnsignedByte();
                        Char.myCharz().clan.maxMember = msg.reader().readUnsignedByte();
                        Char.myCharz().role = msg.reader().readByte();
                        Char.myCharz().clan.clanPoint = msg.reader().readInt();
                        Char.myCharz().clan.level = msg.reader().readByte();
                        GameCanvas.panel.myMember = new MyVector();
                        for (int num75 = 0; num75 < Char.myCharz().clan.currMember; num75++)
                        {
                            Member member2 = new Member();
                            member2.ID = msg.reader().readInt();
                            member2.head = msg.reader().readShort();
                            member2.headICON = msg.reader().readShort();
                            member2.leg = msg.reader().readShort();
                            member2.body = msg.reader().readShort();
                            member2.name = msg.reader().readUTF();
                            member2.role = msg.reader().readByte();
                            member2.powerPoint = msg.reader().readUTF();
                            member2.donate = msg.reader().readInt();
                            member2.receive_donate = msg.reader().readInt();
                            member2.clanPoint = msg.reader().readInt();
                            member2.curClanPoint = msg.reader().readInt();
                            member2.joinTime = NinjaUtil.getDate(msg.reader().readInt());
                            GameCanvas.panel.myMember.addElement(member2);
                        }
                        int num76 = msg.reader().readUnsignedByte();
                        for (int num77 = 0; num77 < num76; num77++)
                        {
                            readClanMsg(msg, -1);
                        }
                        if (GameCanvas.panel.isSearchClan || GameCanvas.panel.isViewMember || GameCanvas.panel.isMessage)
                        {
                            GameCanvas.panel.setTabClans();
                        }
                        if (flag6)
                        {
                            GameCanvas.panel.setTabClans();
                        }
                        break;
                    }
                case -52:
                    {
                        sbyte b34 = msg.reader().readByte();
                        if (b34 == 0)
                        {
                            Member member3 = new Member();
                            member3.ID = msg.reader().readInt();
                            member3.head = msg.reader().readShort();
                            member3.headICON = msg.reader().readShort();
                            member3.leg = msg.reader().readShort();
                            member3.body = msg.reader().readShort();
                            member3.name = msg.reader().readUTF();
                            member3.role = msg.reader().readByte();
                            member3.powerPoint = msg.reader().readUTF();
                            member3.donate = msg.reader().readInt();
                            member3.receive_donate = msg.reader().readInt();
                            member3.clanPoint = msg.reader().readInt();
                            member3.joinTime = NinjaUtil.getDate(msg.reader().readInt());
                            if (GameCanvas.panel.myMember == null)
                            {
                                GameCanvas.panel.myMember = new MyVector();
                            }
                            GameCanvas.panel.myMember.addElement(member3);
                            GameCanvas.panel.initTabClans();
                        }
                        if (b34 == 1)
                        {
                            GameCanvas.panel.myMember.removeElementAt(msg.reader().readByte());
                            GameCanvas.panel.currentListLength--;
                            GameCanvas.panel.initTabClans();
                        }
                        if (b34 == 2)
                        {
                            Member member4 = new Member();
                            member4.ID = msg.reader().readInt();
                            member4.head = msg.reader().readShort();
                            member4.headICON = msg.reader().readShort();
                            member4.leg = msg.reader().readShort();
                            member4.body = msg.reader().readShort();
                            member4.name = msg.reader().readUTF();
                            member4.role = msg.reader().readByte();
                            member4.powerPoint = msg.reader().readUTF();
                            member4.donate = msg.reader().readInt();
                            member4.receive_donate = msg.reader().readInt();
                            member4.clanPoint = msg.reader().readInt();
                            member4.joinTime = NinjaUtil.getDate(msg.reader().readInt());
                            for (int num78 = 0; num78 < GameCanvas.panel.myMember.size(); num78++)
                            {
                                Member member5 = (Member)GameCanvas.panel.myMember.elementAt(num78);
                                if (member5.ID == member4.ID)
                                {
                                    if (Char.myCharz().charID == member4.ID)
                                    {
                                        Char.myCharz().role = member4.role;
                                    }
                                    Member o = member4;
                                    GameCanvas.panel.myMember.removeElement(member5);
                                    GameCanvas.panel.myMember.insertElementAt(o, num78);
                                    return;
                                }
                            }
                        }
                        break;
                    }
                case -50:
                    {
                        InfoDlg.hide();
                        GameCanvas.panel.member = new MyVector();
                        sbyte b22 = msg.reader().readByte();
                        for (int num49 = 0; num49 < b22; num49++)
                        {
                            Member member = new Member();
                            member.ID = msg.reader().readInt();
                            member.head = msg.reader().readShort();
                            member.headICON = msg.reader().readShort();
                            member.leg = msg.reader().readShort();
                            member.body = msg.reader().readShort();
                            member.name = msg.reader().readUTF();
                            member.role = msg.reader().readByte();
                            member.powerPoint = msg.reader().readUTF();
                            member.donate = msg.reader().readInt();
                            member.receive_donate = msg.reader().readInt();
                            member.clanPoint = msg.reader().readInt();
                            member.joinTime = NinjaUtil.getDate(msg.reader().readInt());
                            GameCanvas.panel.member.addElement(member);
                        }
                        GameCanvas.panel.isViewMember = true;
                        GameCanvas.panel.isSearchClan = false;
                        GameCanvas.panel.isMessage = false;
                        GameCanvas.panel.currentListLength = GameCanvas.panel.member.size() + 2;
                        GameCanvas.panel.initTabClans();
                        break;
                    }
                case -47:
                    {
                        InfoDlg.hide();
                        sbyte b69 = msg.reader().readByte();
                        if (b69 == 0)
                        {
                            GameCanvas.panel.clanReport = mResources.cannot_find_clan;
                            GameCanvas.panel.clans = null;
                        }
                        else
                        {
                            GameCanvas.panel.clans = new Clan[b69];
                            for (int num169 = 0; num169 < GameCanvas.panel.clans.Length; num169++)
                            {
                                GameCanvas.panel.clans[num169] = new Clan();
                                GameCanvas.panel.clans[num169].ID = msg.reader().readInt();
                                GameCanvas.panel.clans[num169].name = msg.reader().readUTF();
                                GameCanvas.panel.clans[num169].slogan = msg.reader().readUTF();
                                GameCanvas.panel.clans[num169].imgID = msg.reader().readUnsignedByte();
                                GameCanvas.panel.clans[num169].powerPoint = msg.reader().readUTF();
                                GameCanvas.panel.clans[num169].leaderName = msg.reader().readUTF();
                                GameCanvas.panel.clans[num169].currMember = msg.reader().readUnsignedByte();
                                GameCanvas.panel.clans[num169].maxMember = msg.reader().readUnsignedByte();
                                GameCanvas.panel.clans[num169].date = msg.reader().readInt();
                            }
                        }
                        GameCanvas.panel.isSearchClan = true;
                        GameCanvas.panel.isViewMember = false;
                        GameCanvas.panel.isMessage = false;
                        if (GameCanvas.panel.isSearchClan)
                        {
                            GameCanvas.panel.initTabClans();
                        }
                        break;
                    }
                case -46:
                    {
                        InfoDlg.hide();
                        sbyte type = msg.reader().readByte();
                        if (type == 1 || type == 3)
                        {
                            GameCanvas.endDlg();
                            ClanImage.vClanImage.removeAllElements();
                            int size = msg.reader().readUnsignedByte();
                            for (int i = 0; i < size; i++)
                            {
                                byte id = msg.reader().readUnsignedByte();
                                string name = msg.reader().readUTF();
                                int xu = msg.reader().readInt();
                                int luong = msg.reader().readInt();

                                ClanImage clanImage2 = new()
                                {
                                    ID = id,
                                    name = name,
                                    xu = xu,
                                    luong = luong
                                };

                                if (!ClanImage.isExistClanImage(clanImage2.ID))
                                {
                                    ClanImage.addClanImage(clanImage2);
                                    continue;
                                }
                                ClanImage.getClanImage((short)clanImage2.ID).name = clanImage2.name;
                                ClanImage.getClanImage((short)clanImage2.ID).xu = clanImage2.xu;
                                ClanImage.getClanImage((short)clanImage2.ID).luong = clanImage2.luong;
                            }
                            if (Char.myCharz().clan != null)
                            {
                                GameCanvas.panel.changeIcon();
                            }
                        }
                        if (type == 4)
                        {
                            Char.myCharz().clan.imgID = msg.reader().readUnsignedByte();
                            Char.myCharz().clan.slogan = msg.reader().readUTF();
                        }
                        break;
                    }
                case -61:
                    {
                        int num105 = msg.reader().readInt();
                        if (num105 != Char.myCharz().charID)
                        {
                            if (GameScr.findCharInMap(num105) != null)
                            {
                                GameScr.findCharInMap(num105).clanID = msg.reader().readInt();
                                if (GameScr.findCharInMap(num105).clanID == -2)
                                {
                                    GameScr.findCharInMap(num105).isCopy = true;
                                }
                            }
                        }
                        else if (Char.myCharz().clan != null)
                        {
                            Char.myCharz().clan.ID = msg.reader().readInt();
                        }
                        break;
                    }
                case -42:
                    Char.myCharz().cHPGoc = msg.reader().readDouble();
                    Char.myCharz().cMPGoc = msg.reader().readDouble();
                    Char.myCharz().cDamGoc = msg.reader().readDouble();
                    Char.myCharz().cHPFull = msg.reader().readDouble();
                    Char.myCharz().cMPFull = msg.reader().readDouble();
                    Char.myCharz().cHP = msg.reader().readDouble();
                    Char.myCharz().cMP = msg.reader().readDouble();
                    Char.myCharz().cspeed = msg.reader().readByte();
                    Char.myCharz().hpFrom1000TiemNang = msg.reader().readByte();
                    Char.myCharz().mpFrom1000TiemNang = msg.reader().readByte();
                    Char.myCharz().damFrom1000TiemNang = msg.reader().readByte();
                    Char.myCharz().cDamFull = msg.reader().readDouble();
                    Char.myCharz().cDefull = msg.reader().readInt();
                    Char.myCharz().cCriticalFull = msg.reader().readByte();
                    Char.myCharz().cTiemNang = msg.reader().readLong();
                    Char.myCharz().expForOneAdd = msg.reader().readShort();
                    Char.myCharz().cDefGoc = msg.reader().readShort();
                    Char.myCharz().cCriticalGoc = msg.reader().readByte();

                    try
                    {
                        Char.myCharz().tlDef = msg.readInt3Byte();
                        Char.myCharz().tlPst = msg.readInt3Byte();
                        Char.myCharz().tlNeDon = msg.readInt3Byte();
                        Char.myCharz().tlHutHp = msg.readInt3Byte();
                        Char.myCharz().tlHutMp = msg.readInt3Byte();

                        Char.myCharz().tileGiamTDHS = msg.readInt3Byte();
                        Char.myCharz().timeGiamTDHS = msg.readInt3Byte();
                        Char.myCharz().khangTDHS = msg.reader().readBool();
                        Char.myCharz().isKhongLanh = msg.reader().readBool();
                        Char.myCharz().wearingVoHinh = msg.reader().readBool();
                        Char.myCharz().teleport = msg.reader().readBool();
                    }
                    catch
                    {
                        Char.myCharz().tlDef = 0;
                        Char.myCharz().tlPst = 0;
                        Char.myCharz().tlNeDon = 0;
                        Char.myCharz().tlHutHp = 0;
                        Char.myCharz().tlHutMp = 0;

                        Char.myCharz().tileGiamTDHS = 0;
                        Char.myCharz().timeGiamTDHS = 0;
                        Char.myCharz().khangTDHS = false;
                        Char.myCharz().isKhongLanh = false;
                        Char.myCharz().wearingVoHinh = false;
                        Char.myCharz().teleport = false;
                    }

                    InfoDlg.hide();
                    break;
                case 1:
                    {
                        bool flag9 = msg.reader().readBool();
                        Res.outz("isRes= " + flag9);
                        if (!flag9)
                        {
                            GameCanvas.startOKDlg(msg.reader().readUTF());
                            break;
                        }
                        GameCanvas.loginScr.isLogin2 = false;
                        Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, string.Empty);
                        GameCanvas.endDlg();
                        GameCanvas.loginScr.doLogin();
                        break;
                    }
                case 2:
                    Char.isLoadingMap = false;
                    LoginScr.isLoggingIn = false;
                    if (!GameScr.isLoadAllData)
                    {
                        GameScr.gI().initSelectChar();
                    }
                    BgItem.clearHashTable();
                    GameCanvas.endDlg();
                    CreateCharScr.isCreateChar = true;
                    CreateCharScr.gI().switchToMe();
                    break;
                case -107:
                    {
                        sbyte b21 = msg.reader().readByte();
                        if (b21 == 0)
                        {
                            Char.myCharz().havePet = false;
                        }
                        if (b21 == 1)
                        {
                            Char.myCharz().havePet = true;
                        }
                        if (b21 != 2)
                        {
                            break;
                        }
                        InfoDlg.hide();
                        Char.myPetz().head = msg.reader().readShort();
                        Char.myPetz().setDefaultPart();
                        int num40 = msg.reader().readUnsignedByte();
                        Char.myPetz().arrItemBody = new Item[num40];
                        for (int num41 = 0; num41 < num40; num41++)
                        {
                            short num42 = msg.reader().readShort();
                            if (num42 == -1)
                            {
                                continue;
                            }
                            Char.myPetz().arrItemBody[num41] = new Item
                            {
                                template = ItemTemplates.get(num42)
                            };
                            int num43 = Char.myPetz().arrItemBody[num41].template.type;
                            Char.myPetz().arrItemBody[num41].quantity = msg.reader().readInt();
                            Char.myPetz().arrItemBody[num41].info = msg.reader().readUTF();
                            Char.myPetz().arrItemBody[num41].content = msg.reader().readUTF();
                            int num44 = msg.reader().readUnsignedByte();
                            if (num44 != 0)
                            {
                                Char.myPetz().arrItemBody[num41].itemOption = new ItemOption[num44];
                                for (int num45 = 0; num45 < Char.myPetz().arrItemBody[num41].itemOption.Length; num45++)
                                {
                                    int num46 = msg.reader().readUnsignedByte();
                                    int param3 = msg.reader().readUnsignedShort();
                                    if (num46 != -1)
                                    {
                                        Char.myPetz().arrItemBody[num41].itemOption[num45] = new ItemOption(num46, param3);
                                    }
                                }
                            }
                            switch (num43)
                            {
                                case 0:
                                    Char.myPetz().body = Char.myPetz().arrItemBody[num41].template.part;
                                    break;
                                case 1:
                                    Char.myPetz().leg = Char.myPetz().arrItemBody[num41].template.part;
                                    break;
                            }
                        }
                        Char.myPetz().cHP = msg.reader().readDouble();
                        Char.myPetz().cHPFull = msg.reader().readDouble();
                        Char.myPetz().cMP = msg.reader().readDouble();
                        Char.myPetz().cMPFull = msg.reader().readDouble();
                        Char.myPetz().cDamFull = msg.reader().readDouble();
                        Char.myPetz().cName = msg.reader().readUTF();
                        Char.myPetz().currStrLevel = msg.reader().readUTF();
                        Char.myPetz().cPower = msg.reader().readLong();
                        Char.myPetz().cTiemNang = msg.reader().readLong();
                        Char.myPetz().petStatus = msg.reader().readByte();
                        Char.myPetz().cStamina = msg.reader().readShort();
                        Char.myPetz().cMaxStamina = msg.reader().readShort();
                        Char.myPetz().cCriticalFull = msg.reader().readByte();
                        Char.myPetz().cDefull = msg.reader().readShort();
                        Char.myPetz().arrPetSkill = new Skill[msg.reader().readByte()];
                        for (int num47 = 0; num47 < Char.myPetz().arrPetSkill.Length; num47++)
                        {
                            short num48 = msg.reader().readShort();
                            if (num48 != -1)
                            {
                                Char.myPetz().arrPetSkill[num47] = Skills.get(num48);
                                continue;
                            }
                            Char.myPetz().arrPetSkill[num47] = new Skill();
                            Char.myPetz().arrPetSkill[num47].template = null;
                            Char.myPetz().arrPetSkill[num47].moreInfo = msg.reader().readUTF();
                        }
                        if (!ModFunc.userOpenPet)
                        {
                            return;
                        }
                        if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
                        {
                            GameCanvas.panel2 = new Panel();
                            GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
                            GameCanvas.panel2.setTypeBodyOnly();
                            GameCanvas.panel2.show();
                            GameCanvas.panel.setTypePetMain();
                            GameCanvas.panel.show();
                            ModFunc.userOpenPet = false;
                        }
                        else
                        {
                            GameCanvas.panel.tabName[21] = mResources.petMainTab;
                            GameCanvas.panel.setTypePetMain();
                            GameCanvas.panel.show();
                            ModFunc.userOpenPet = false;
                        }
                        break;
                    }
                case 3:
                    {
                        sbyte type = msg.reader().readByte();
                        if (type == 0)
                        {
                            Char.myCharz().havePet2 = false;
                        }
                        if (type == 1)
                        {
                            Char.myCharz().havePet2 = true;
                        }
                        if (type != 2)
                        {
                            break;
                        }
                        InfoDlg.hide();
                        Char.MyPet2z().head = msg.reader().readShort();
                        Char.MyPet2z().setDefaultPart();
                        int arrBodySz = msg.reader().readUnsignedByte();
                        Char.MyPet2z().arrItemBody = new Item[arrBodySz];
                        for (int i = 0; i < arrBodySz; i++)
                        {
                            short tempId = msg.reader().readShort();
                            if (tempId == -1)
                            {
                                continue;
                            }
                            Char.MyPet2z().arrItemBody[i] = new Item
                            {
                                template = ItemTemplates.get(tempId)
                            };
                            int num43 = Char.MyPet2z().arrItemBody[i].template.type;
                            Char.MyPet2z().arrItemBody[i].quantity = msg.reader().readInt();
                            Char.MyPet2z().arrItemBody[i].info = msg.reader().readUTF();
                            Char.MyPet2z().arrItemBody[i].content = msg.reader().readUTF();
                            int num44 = msg.reader().readUnsignedByte();
                            if (num44 != 0)
                            {
                                Char.MyPet2z().arrItemBody[i].itemOption = new ItemOption[num44];
                                for (int num45 = 0; num45 < Char.MyPet2z().arrItemBody[i].itemOption.Length; num45++)
                                {
                                    int num46 = msg.reader().readUnsignedByte();
                                    int param3 = msg.reader().readUnsignedShort();
                                    if (num46 != -1)
                                    {
                                        Char.MyPet2z().arrItemBody[i].itemOption[num45] = new ItemOption(num46, param3);
                                    }
                                }
                            }
                            switch (num43)
                            {
                                case 0:
                                    Char.MyPet2z().body = Char.MyPet2z().arrItemBody[i].template.part;
                                    break;
                                case 1:
                                    Char.MyPet2z().leg = Char.MyPet2z().arrItemBody[i].template.part;
                                    break;
                            }
                        }
                        Char.MyPet2z().cHP = msg.reader().readDouble();
                        Char.MyPet2z().cHPFull = msg.reader().readDouble();
                        Char.MyPet2z().cMP = msg.reader().readDouble();
                        Char.MyPet2z().cMPFull = msg.reader().readDouble();
                        Char.MyPet2z().cDamFull = msg.reader().readDouble();
                        Char.MyPet2z().cName = msg.reader().readUTF();
                        Char.MyPet2z().currStrLevel = msg.reader().readUTF();
                        Char.MyPet2z().cPower = msg.reader().readLong();
                        Char.MyPet2z().cTiemNang = msg.reader().readLong();
                        Char.MyPet2z().petStatus = msg.reader().readByte();
                        Char.MyPet2z().cStamina = msg.reader().readShort();
                        Char.MyPet2z().cMaxStamina = msg.reader().readShort();
                        Char.MyPet2z().cCriticalFull = msg.reader().readByte();
                        Char.MyPet2z().cDefull = msg.reader().readInt();
                        Char.MyPet2z().arrPetSkill = new Skill[msg.reader().readByte()];
                        for (int num47 = 0; num47 < Char.MyPet2z().arrPetSkill.Length; num47++)
                        {
                            short num48 = msg.reader().readShort();
                            if (num48 != -1)
                            {
                                Char.MyPet2z().arrPetSkill[num47] = Skills.get(num48);
                                continue;
                            }
                            Char.MyPet2z().arrPetSkill[num47] = new Skill();
                            Char.MyPet2z().arrPetSkill[num47].template = null;
                            Char.MyPet2z().arrPetSkill[num47].moreInfo = msg.reader().readUTF();
                        }
                        if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
                        {
                            GameCanvas.panel2 = new Panel();
                            GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
                            GameCanvas.panel2.setTypeBodyOnly();
                            GameCanvas.panel2.show();
                            GameCanvas.panel.setTypePet2Main();
                            GameCanvas.panel.show();
                        }
                        else
                        {
                            GameCanvas.panel.tabName[21] = mResources.petMainTab;
                            GameCanvas.panel.setTypePet2Main();
                            GameCanvas.panel.show();
                        }
                        break;
                    }
                case -109:
                    Char.myPetz().cHPGoc = msg.reader().readDouble();
                    Char.myPetz().cMPGoc = msg.reader().readDouble();
                    Char.myPetz().cDamGoc = msg.reader().readDouble();
                    Char.myPetz().cDefGoc = msg.reader().readShort();
                    Char.myPetz().cCriticalGoc = msg.reader().readByte();
                    break;
                case -37:
                    {
                        sbyte b20 = msg.reader().readByte();
                        if (b20 != 0)
                        {
                            break;
                        }
                        Char.myCharz().head = msg.reader().readShort();
                        Char.myCharz().setDefaultPart();
                        int num33 = msg.reader().readUnsignedByte();
                        Res.outz("num body = " + num33);
                        Char.myCharz().arrItemBody = new Item[num33];
                        for (int num34 = 0; num34 < num33; num34++)
                        {
                            short num35 = msg.reader().readShort();
                            if (num35 == -1)
                            {
                                continue;
                            }
                            Char.myCharz().arrItemBody[num34] = new Item();
                            Char.myCharz().arrItemBody[num34].template = ItemTemplates.get(num35);
                            int num36 = Char.myCharz().arrItemBody[num34].template.type;
                            Char.myCharz().arrItemBody[num34].quantity = msg.reader().readInt();
                            Char.myCharz().arrItemBody[num34].info = msg.reader().readUTF();
                            Char.myCharz().arrItemBody[num34].content = msg.reader().readUTF();
                            int num37 = msg.reader().readUnsignedByte();
                            if (num37 != 0)
                            {
                                Char.myCharz().arrItemBody[num34].itemOption = new ItemOption[num37];
                                for (int num38 = 0; num38 < Char.myCharz().arrItemBody[num34].itemOption.Length; num38++)
                                {
                                    int num39 = msg.reader().readUnsignedByte();
                                    int param2 = msg.reader().readUnsignedShort();
                                    if (num39 != -1)
                                    {
                                        Char.myCharz().arrItemBody[num34].itemOption[num38] = new ItemOption(num39, param2);
                                    }
                                }
                            }
                            switch (num36)
                            {
                                case 0:
                                    Char.myCharz().body = Char.myCharz().arrItemBody[num34].template.part;
                                    break;
                                case 1:
                                    Char.myCharz().leg = Char.myCharz().arrItemBody[num34].template.part;
                                    break;
                            }
                        }
                        break;
                    }
                case -36:
                    {
                        sbyte b7 = msg.reader().readByte();
                        Res.outz("cAction= " + b7);
                        if (b7 == 0)
                        {
                            int num11 = msg.reader().readUnsignedByte();
                            Char.myCharz().arrItemBag = new Item[num11];
                            GameScr.hpPotion = 0;
                            Res.outz("numC=" + num11);
                            for (int j = 0; j < num11; j++)
                            {
                                short num12 = msg.reader().readShort();
                                if (num12 == -1)
                                {
                                    continue;
                                }
                                Char.myCharz().arrItemBag[j] = new Item();
                                Char.myCharz().arrItemBag[j].template = ItemTemplates.get(num12);
                                Char.myCharz().arrItemBag[j].quantity = msg.reader().readInt();
                                Char.myCharz().arrItemBag[j].info = msg.reader().readUTF();
                                Char.myCharz().arrItemBag[j].content = msg.reader().readUTF();
                                Char.myCharz().arrItemBag[j].indexUI = j;
                                int num13 = msg.reader().readUnsignedByte();
                                if (num13 != 0)
                                {
                                    Char.myCharz().arrItemBag[j].itemOption = new ItemOption[num13];
                                    for (int k = 0; k < Char.myCharz().arrItemBag[j].itemOption.Length; k++)
                                    {
                                        int num14 = msg.reader().readUnsignedByte();
                                        int param = msg.reader().readUnsignedShort();
                                        if (num14 != -1)
                                        {
                                            Char.myCharz().arrItemBag[j].itemOption[k] = new ItemOption(num14, param);
                                        }
                                    }
                                    Char.myCharz().arrItemBag[j].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemBag[j]);
                                }
                                if (Char.myCharz().arrItemBag[j].template.type == 11)
                                {
                                }
                                if (Char.myCharz().arrItemBag[j].template.type == 6)
                                {
                                    GameScr.hpPotion += Char.myCharz().arrItemBag[j].quantity;
                                }
                            }
                        }
                        if (b7 == 2)
                        {
                            sbyte b8 = msg.reader().readByte();
                            int quantity = msg.reader().readInt();
                            int quantity2 = Char.myCharz().arrItemBag[b8].quantity;
                            Char.myCharz().arrItemBag[b8].quantity = quantity;
                            if (Char.myCharz().arrItemBag[b8].quantity < quantity2 && Char.myCharz().arrItemBag[b8].template.type == 6)
                            {
                                GameScr.hpPotion -= quantity2 - Char.myCharz().arrItemBag[b8].quantity;
                            }
                            if (Char.myCharz().arrItemBag[b8].quantity == 0)
                            {
                                Char.myCharz().arrItemBag[b8] = null;
                            }
                        }
                        break;
                    }
                case -35:
                    {
                        sbyte b63 = msg.reader().readByte();
                        Res.outz("cAction= " + b63);
                        if (b63 == 0)
                        {
                            int num154 = msg.reader().readUnsignedByte();
                            Char.myCharz().arrItemBox = new Item[num154];
                            GameCanvas.panel.hasUse = 0;
                            for (int num155 = 0; num155 < num154; num155++)
                            {
                                short num156 = msg.reader().readShort();
                                if (num156 == -1)
                                {
                                    continue;
                                }
                                Char.myCharz().arrItemBox[num155] = new Item();
                                Char.myCharz().arrItemBox[num155].template = ItemTemplates.get(num156);
                                Char.myCharz().arrItemBox[num155].quantity = msg.reader().readInt();
                                Char.myCharz().arrItemBox[num155].info = msg.reader().readUTF();
                                Char.myCharz().arrItemBox[num155].content = msg.reader().readUTF();
                                int num157 = msg.reader().readUnsignedByte();
                                if (num157 != 0)
                                {
                                    Char.myCharz().arrItemBox[num155].itemOption = new ItemOption[num157];
                                    for (int num158 = 0; num158 < Char.myCharz().arrItemBox[num155].itemOption.Length; num158++)
                                    {
                                        int num159 = msg.reader().readUnsignedByte();
                                        int param6 = msg.reader().readUnsignedShort();
                                        if (num159 != -1)
                                        {
                                            Char.myCharz().arrItemBox[num155].itemOption[num158] = new ItemOption(num159, param6);
                                        }
                                    }
                                }
                                GameCanvas.panel.hasUse++;
                            }
                        }
                        if (b63 == 1)
                        {
                            bool isBoxClan = false;
                            try
                            {
                                sbyte b64 = msg.reader().readByte();
                                if (b64 == 1)
                                {
                                    isBoxClan = true;
                                }
                            }
                            catch (Exception)
                            {
                            }
                            GameCanvas.panel.setTypeBox();
                            GameCanvas.panel.isBoxClan = isBoxClan;
                            GameCanvas.panel.show();
                        }
                        if (b63 == 2)
                        {
                            sbyte b65 = msg.reader().readByte();
                            int quantity3 = msg.reader().readInt();
                            Char.myCharz().arrItemBox[b65].quantity = quantity3;
                            if (Char.myCharz().arrItemBox[b65].quantity == 0)
                            {
                                Char.myCharz().arrItemBox[b65] = null;
                            }
                        }
                        break;
                    }
                case -45:
                    {
                        sbyte type = msg.reader().readByte();
                        int playerId = msg.reader().readInt();
                        short skillId = msg.reader().readShort();
                        if (type == 20)
                        {
                            sbyte b50 = msg.reader().readByte();
                            sbyte dir = msg.reader().readByte();
                            short timeGong = msg.reader().readShort();
                            bool isFly = msg.reader().readByte() != 0;
                            sbyte typePaint = msg.reader().readByte();
                            sbyte typeItem = -1;
                            try
                            {
                                typeItem = msg.reader().readByte();
                            }
                            catch (Exception)
                            {
                            }
                            sbyte level = -1;
                            try
                            {
                                level = msg.reader().readByte();
                            }
                            catch (Exception)
                            {
                            }
                            @char = ((Char.myCharz().charID != playerId) ? GameScr.findCharInMap(playerId) : Char.myCharz());
                            @char.SetSkillPaint_NEW(skillId, isFly, b50, typePaint, dir, timeGong, typeItem, level);
                        }
                        if (type == 21)
                        {
                            Point point = new()
                            {
                                x = msg.reader().readShort(),
                                y = msg.reader().readShort()
                            };
                            short timeDame = msg.reader().readShort();
                            short rangeDame = msg.reader().readShort();
                            sbyte typePaint2 = 0;
                            sbyte typeItem2 = -1;
                            Point[] targets = null;
                            @char = (Char.myCharz().charID != playerId) ? GameScr.findCharInMap(playerId) : Char.myCharz();
                            try
                            {
                                typePaint2 = msg.reader().readByte();
                                sbyte targetSz = msg.reader().readByte();
                                targets = new Point[targetSz];
                                for (int i = 0; i < targets.Length; i++)
                                {
                                    targets[i] = new Point
                                    {
                                        type = msg.reader().readByte()
                                    };
                                    if (targets[i].type == 0)
                                    {
                                        targets[i].id = msg.reader().readByte();
                                    }
                                    else
                                    {
                                        targets[i].id = msg.reader().readInt();
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                            try
                            {
                                typeItem2 = msg.reader().readByte();
                            }
                            catch (Exception)
                            {
                            }
                            sbyte level = -1;
                            try
                            {
                                level = msg.reader().readByte();
                            }
                            catch (Exception)
                            {
                            }
                            @char.SetSkillPaint_STT(1, skillId, point, timeDame, rangeDame, typePaint2, targets, typeItem2, level);
                        }
                        if (type == 0)
                        {
                            Res.outz("id use= " + playerId);
                            if (Char.myCharz().charID != playerId)
                            {
                                @char = GameScr.findCharInMap(playerId);
                                if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
                                {
                                    @char.setSkillPaint(GameScr.sks[skillId], 0);
                                }
                                else
                                {
                                    @char.setSkillPaint(GameScr.sks[skillId], 1);
                                    @char.delayFall = 20;
                                }
                            }
                            else
                            {
                                Char.myCharz().saveLoadPreviousSkill();
                                Res.outz("LOAD LAST SKILL");
                            }
                            sbyte b52 = msg.reader().readByte();
                            Res.outz("npc size= " + b52);
                            for (int num120 = 0; num120 < b52; num120++)
                            {
                                sbyte b53 = msg.reader().readByte();
                                sbyte b54 = msg.reader().readByte();
                                Res.outz("index= " + b53);
                                if (skillId >= 42 && skillId <= 48)
                                {
                                    ((Mob)GameScr.vMob.elementAt(b53)).isFreez = true;
                                    ((Mob)GameScr.vMob.elementAt(b53)).seconds = b54;
                                    ((Mob)GameScr.vMob.elementAt(b53)).last = (((Mob)GameScr.vMob.elementAt(b53)).cur = mSystem.currentTimeMillis());
                                }
                            }
                            sbyte b55 = msg.reader().readByte();
                            for (int num121 = 0; num121 < b55; num121++)
                            {
                                int num122 = msg.reader().readInt();
                                sbyte b56 = msg.reader().readByte();
                                Res.outz("player ID= " + num122 + " my ID= " + Char.myCharz().charID);
                                if (skillId < 42 || skillId > 48)
                                {
                                    continue;
                                }
                                if (num122 == Char.myCharz().charID)
                                {
                                    if (!Char.myCharz().isFlyAndCharge && !Char.myCharz().isStandAndCharge)
                                    {
                                        GameScr.gI().isFreez = true;
                                        Char.myCharz().isFreez = true;
                                        Char.myCharz().freezSeconds = b56;
                                        Char.myCharz().lastFreez = (Char.myCharz().currFreez = mSystem.currentTimeMillis());
                                        Char.myCharz().isLockMove = true;
                                    }
                                }
                                else
                                {
                                    @char = GameScr.findCharInMap(num122);
                                    if (@char != null && !@char.isFlyAndCharge && !@char.isStandAndCharge)
                                    {
                                        @char.isFreez = true;
                                        @char.seconds = b56;
                                        @char.freezSeconds = b56;
                                        @char.lastFreez = (GameScr.findCharInMap(num122).currFreez = mSystem.currentTimeMillis());
                                    }
                                }
                            }
                        }
                        if (type == 1 && playerId != Char.myCharz().charID)
                        {
                            GameScr.findCharInMap(playerId).isCharge = true;
                        }
                        if (type == 3)
                        {
                            if (playerId == Char.myCharz().charID)
                            {
                                Char.myCharz().isCharge = false;
                                SoundMn.gI().taitaoPause();
                                Char.myCharz().saveLoadPreviousSkill();
                            }
                            else
                            {
                                GameScr.findCharInMap(playerId).isCharge = false;
                            }
                        }
                        if (type == 4)
                        {
                            if (playerId == Char.myCharz().charID)
                            {
                                Char.myCharz().seconds = msg.reader().readShort() - 1000;
                                Char.myCharz().last = mSystem.currentTimeMillis();
                                Res.outz("second= " + Char.myCharz().seconds + " last= " + Char.myCharz().last);
                            }
                            else if (GameScr.findCharInMap(playerId) != null)
                            {
                                switch (GameScr.findCharInMap(playerId).cgender)
                                {
                                    case 0:
                                        GameScr.findCharInMap(playerId).useChargeSkill(isGround: false);
                                        break;
                                    case 1:
                                        GameScr.findCharInMap(playerId).useChargeSkill(isGround: true);
                                        break;
                                }
                                GameScr.findCharInMap(playerId).skillTemplateId = skillId;
                                GameScr.findCharInMap(playerId).isUseSkillAfterCharge = true;
                                GameScr.findCharInMap(playerId).seconds = msg.reader().readShort();
                                GameScr.findCharInMap(playerId).last = mSystem.currentTimeMillis();
                            }
                        }
                        if (type == 5)
                        {
                            if (playerId == Char.myCharz().charID)
                            {
                                Char.myCharz().stopUseChargeSkill();
                            }
                            else if (GameScr.findCharInMap(playerId) != null)
                            {
                                GameScr.findCharInMap(playerId).stopUseChargeSkill();
                            }
                        }
                        if (type == 6)
                        {
                            if (playerId == Char.myCharz().charID)
                            {
                                Char.myCharz().setAutoSkillPaint(GameScr.sks[skillId], 0);
                            }
                            else if (GameScr.findCharInMap(playerId) != null)
                            {
                                GameScr.findCharInMap(playerId).setAutoSkillPaint(GameScr.sks[skillId], 0);
                                SoundMn.gI().gong();
                            }
                        }
                        if (type == 7)
                        {
                            if (playerId == Char.myCharz().charID)
                            {
                                Char.myCharz().seconds = msg.reader().readShort();
                                Res.outz("second = " + Char.myCharz().seconds);
                                Char.myCharz().last = mSystem.currentTimeMillis();
                            }
                            else if (GameScr.findCharInMap(playerId) != null)
                            {
                                GameScr.findCharInMap(playerId).useChargeSkill(isGround: true);
                                GameScr.findCharInMap(playerId).seconds = msg.reader().readShort();
                                GameScr.findCharInMap(playerId).last = mSystem.currentTimeMillis();
                                SoundMn.gI().gong();
                            }
                        }
                        if (type == 8 && playerId != Char.myCharz().charID && GameScr.findCharInMap(playerId) != null)
                        {
                            GameScr.findCharInMap(playerId).setAutoSkillPaint(GameScr.sks[skillId], 0);
                        }
                        break;
                    }
			case -44:
			{
				bool flag6 = false;
				if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
				{
					flag6 = true;
				}
				sbyte b32 = msg.reader().readByte();
				int num65 = msg.reader().readUnsignedByte();
				Char.myCharz().arrItemShop = new Item[num65][];
				GameCanvas.panel.shopTabName = new string[num65 + ((!flag6) ? 1 : 0)][];
				for (int num66 = 0; num66 < GameCanvas.panel.shopTabName.Length; num66++)
				{
					GameCanvas.panel.shopTabName[num66] = new string[2];
				}
				if (b32 == 2)
				{
					GameCanvas.panel.maxPageShop = new int[num65];
					GameCanvas.panel.currPageShop = new int[num65];
				}
				if (!flag6)
				{
					GameCanvas.panel.shopTabName[num65] = mResources.inventory;
				}
				for (int num67 = 0; num67 < num65; num67++)
				{
					string[] array5 = Res.split(msg.reader().readUTF(), "\n", 0);
					if (b32 == 2)
					{
						GameCanvas.panel.maxPageShop[num67] = msg.reader().readUnsignedByte();
					}
					if (array5.Length == 2)
					{
						GameCanvas.panel.shopTabName[num67] = array5;
					}
					if (array5.Length == 1)
					{
						GameCanvas.panel.shopTabName[num67][0] = array5[0];
						GameCanvas.panel.shopTabName[num67][1] = string.Empty;
					}
					int num68 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemShop[num67] = new Item[num68];
					Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy;
					if (b32 == 1)
					{
						Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy2;
					}
					for (int num69 = 0; num69 < num68; num69++)
					{
						short num70 = msg.reader().readShort();
						if (num70 == -1)
						{
							continue;
						}
						Char.myCharz().arrItemShop[num67][num69] = new Item();
						Char.myCharz().arrItemShop[num67][num69].template = ItemTemplates.get(num70);
						Res.outz("name " + num67 + " = " + Char.myCharz().arrItemShop[num67][num69].template.name + " id templat= " + Char.myCharz().arrItemShop[num67][num69].template.id);
						if (b32 == 8)
						{
							Char.myCharz().arrItemShop[num67][num69].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num67][num69].buyGold = msg.reader().readInt();
							Char.myCharz().arrItemShop[num67][num69].quantity = msg.reader().readInt();
						}
						else if (b32 == 4)
						{
							Char.myCharz().arrItemShop[num67][num69].reason = msg.reader().readUTF();
						}
						else if (b32 == 0)
						{
							Char.myCharz().arrItemShop[num67][num69].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num67][num69].buyGold = msg.reader().readInt();
						}
						else if (b32 == 1)
						{
							Char.myCharz().arrItemShop[num67][num69].powerRequire = msg.reader().readLong();
						}
						else if (b32 == 2)
						{
							Char.myCharz().arrItemShop[num67][num69].itemId = msg.reader().readShort();
							Char.myCharz().arrItemShop[num67][num69].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num67][num69].buyGold = msg.reader().readInt();
							Char.myCharz().arrItemShop[num67][num69].buyType = msg.reader().readByte();
							Char.myCharz().arrItemShop[num67][num69].quantity = msg.reader().readInt();
							Char.myCharz().arrItemShop[num67][num69].isMe = msg.reader().readByte();
						}
						else if (b32 == 3)
						{
							Char.myCharz().arrItemShop[num67][num69].isBuySpec = true;
							Char.myCharz().arrItemShop[num67][num69].iconSpec = msg.reader().readShort();
							Char.myCharz().arrItemShop[num67][num69].buySpec = msg.reader().readInt();
						}
						int num71 = msg.reader().readUnsignedByte();
						if (num71 != 0)
						{
									
                                    

                                    Char.myCharz().arrItemShop[num67][num69].itemOption = new ItemOption[num71];
							//List<string> t = new List<string>();
							for (int num72 = 0; num72 < Char.myCharz().arrItemShop[num67][num69].itemOption.Length; num72++)
							{
								int num73 = msg.reader().readUnsignedByte();
								int param5 = msg.reader().readUnsignedShort();
								if (num73 != -1)
								{
									Char.myCharz().arrItemShop[num67][num69].itemOption[num72] = new ItemOption(num73, param5);
										//	t.Add("["+Char.myCharz().arrItemShop[num67][num69].itemOption[num72].getOptionString()+",optionId="+num73+",param="+param5+"]");
									Char.myCharz().arrItemShop[num67][num69].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemShop[num67][num69]);
								}
							}
                                  /*  using (StreamWriter writer = new StreamWriter("D:\\ServerGame\\NroFull\\data\\vinh\\shop\\item.txt", true))
                                    {
                                        writer.WriteLine(Char.myCharz().arrItemShop[num67][num69].template.name+"|"+ Char.myCharz().arrItemShop[num67][num69].reason + "|"+ string.Join("|",t));
                                    }*/
                                }
						sbyte b33 = msg.reader().readByte();
						Char.myCharz().arrItemShop[num67][num69].newItem = ((b33 != 0) ? true : false);
						sbyte b34 = msg.reader().readByte();
						if (b34 == 1)
						{
							int headTemp = msg.reader().readShort();
							int bodyTemp = msg.reader().readShort();
							int legTemp = msg.reader().readShort();
							int bagTemp = msg.reader().readShort();
							Char.myCharz().arrItemShop[num67][num69].setPartTemp(headTemp, bodyTemp, legTemp, bagTemp);
						}
					}
				}
				if (flag6)
				{
					if (b32 != 2)
					{
						GameCanvas.panel2 = new Panel();
						GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
						GameCanvas.panel2.setTypeBodyOnly();
						GameCanvas.panel2.show();
					}
					else
					{
						GameCanvas.panel2 = new Panel();
						GameCanvas.panel2.setTypeKiGuiOnly();
						GameCanvas.panel2.show();
					}
				}
				GameCanvas.panel.tabName[1] = GameCanvas.panel.shopTabName;
				if (b32 == 2)
				{
					string[][] array6 = GameCanvas.panel.tabName[1];
					if (flag6)
					{
						GameCanvas.panel.tabName[1] = new string[4][]
						{
							array6[0],
							array6[1],
							array6[2],
							array6[3]
						};
					}
					else
					{
						GameCanvas.panel.tabName[1] = new string[5][]
						{
							array6[0],
							array6[1],
							array6[2],
							array6[3],
							array6[4]
						};
					}
				}
				GameCanvas.panel.setTypeShop(b32);
				GameCanvas.panel.show();
				break;
			}
                case -41:
                    {
                        sbyte b23 = msg.reader().readByte();
                        Char.myCharz().strLevel = new string[b23];
                        for (int i = 0; i < b23; i++)
                        {
                            string text = msg.reader().readUTF();
                            Char.myCharz().strLevel[i] = text;
                        }
                        Res.outz("---   xong  level caption cmd : " + msg.command);
                        break;
                    }
                case -34:
                    {
                        sbyte b12 = msg.reader().readByte();
                        Res.outz("act= " + b12);
                        if (b12 == 0 && GameScr.gI().magicTree != null)
                        {
                            Res.outz("toi duoc day");
                            MagicTree magicTree = GameScr.gI().magicTree;
                            magicTree.id = msg.reader().readShort();
                            magicTree.name = msg.reader().readUTF();
                            magicTree.name = Res.changeString(magicTree.name);
                            magicTree.x = msg.reader().readShort();
                            magicTree.y = msg.reader().readShort();
                            magicTree.level = msg.reader().readByte();
                            magicTree.currPeas = msg.reader().readShort();
                            magicTree.maxPeas = msg.reader().readShort();
                            Res.outz("curr Peas= " + magicTree.currPeas);
                            magicTree.strInfo = msg.reader().readUTF();
                            magicTree.seconds = msg.reader().readInt();
                            magicTree.timeToRecieve = magicTree.seconds;
                            sbyte b13 = msg.reader().readByte();
                            magicTree.peaPostionX = new int[b13];
                            magicTree.peaPostionY = new int[b13];
                            for (int num22 = 0; num22 < b13; num22++)
                            {
                                magicTree.peaPostionX[num22] = msg.reader().readByte();
                                magicTree.peaPostionY[num22] = msg.reader().readByte();
                            }
                            magicTree.isUpdate = msg.reader().readBool();
                            magicTree.last = (magicTree.cur = mSystem.currentTimeMillis());
                            GameScr.gI().magicTree.isUpdateTree = true;
                        }
                        if (b12 == 1)
                        {
                            myVector = new MyVector();
                            try
                            {
                                while (msg.reader().available() > 0)
                                {
                                    string caption = msg.reader().readUTF();
                                    myVector.addElement(new Command(caption, GameCanvas.instance, 888392, null));
                                }
                            }
                            catch (Exception ex4)
                            {
                                Cout.println("Loi MAGIC_TREE " + ex4.ToString());
                            }
                            GameCanvas.menu.startAt(myVector, 3);
                        }
                        if (b12 == 2)
                        {
                            GameScr.gI().magicTree.remainPeas = msg.reader().readShort();
                            GameScr.gI().magicTree.seconds = msg.reader().readInt();
                            GameScr.gI().magicTree.last = (GameScr.gI().magicTree.cur = mSystem.currentTimeMillis());
                            GameScr.gI().magicTree.isUpdateTree = true;
                            GameScr.gI().magicTree.isPeasEffect = true;
                        }
                        break;
                    }
                case 11:
                    {
                        GameCanvas.debug("SA9", 2);
                        int num9 = msg.reader().readByte();
                        sbyte b6 = msg.reader().readByte();
                        if (b6 != 0)
                        {
                            Mob.arrMobTemplate[num9].data.readDataNewBoss(NinjaUtil.readByteArray(msg), b6);
                        }
                        else
                        {
                            Mob.arrMobTemplate[num9].data.readData(NinjaUtil.readByteArray(msg));
                        }
                        for (int i = 0; i < GameScr.vMob.size(); i++)
                        {
                            mob = (Mob)GameScr.vMob.elementAt(i);
                            if (mob.templateId == num9)
                            {
                                mob.w = Mob.arrMobTemplate[num9].data.width;
                                mob.h = Mob.arrMobTemplate[num9].data.height;
                            }
                        }
                        sbyte[] array2 = NinjaUtil.readByteArray(msg);
                        Image img = Image.createImage(array2, 0, array2.Length);
                        Mob.arrMobTemplate[num9].data.img = img;
                        int num10 = msg.reader().readByte();
                        Mob.arrMobTemplate[num9].data.typeData = num10;
                        if (num10 == 1 || num10 == 2)
                        {
                            readFrameBoss(msg, num9);
                        }
                        break;
                    }
                case -69:
                    Char.myCharz().cMaxStamina = msg.reader().readShort();
                    break;
                case -68:
                    Char.myCharz().cStamina = msg.reader().readShort();
                    break;
                case -67:
                    {

                        int iconId = msg.reader().readInt();
                        //string key = msg.reader().readUTF();
                        // Debug.Log("GET ICON: " + iconId);
                        sbyte[] data = null;
                        try
                        {
                            data = NinjaUtil.readByteArray(msg);
                           // Image img = createImage(data, key);
                            Image img = createImage(data);
                            SmallImage.imgNew[iconId].img = img;
                            if (mGraphics.zoomLevel > 1)
                            {
                                SmallImage.imageRaw.Add(iconId, img);
                        }
                        }
                        catch (Exception)
                        {
                            SmallImage.imgNew[iconId].img = Image.createRGBImage(new int[1], 1, 1, bl: true);
                        }
                                                break;
                    }
                /*  case -67:
                      {
                          Res.outz("RECIEVE ICON");
                          demCount += 1f;
                          int num174 = msg.reader().readInt();
                          string key = msg.reader().readUTF();
                          sbyte[] array18 = null;
                          try
                          {
                              array18 = NinjaUtil.readByteArray(msg);
                              if (num174 == 3896)
                              {
                                  Res.outz("SIZE CHECK= " + array18.Length);
                              }
                              SmallImage.imgNew[num174].img = createImage(array18, key);
                          }
                          catch (Exception)
                          {
                              array18 = null;
                              SmallImage.imgNew[num174].img = Image.createRGBImage(new int[1], 1, 1, true);
                          }
                          break;
                      }*/

                case -66:
                    {
                        short id2 = msg.reader().readShort();
                        sbyte[] data5 = NinjaUtil.readByteArray(msg);
                        EffectData effDataById = Effect.getEffDataById(id2);
                        sbyte b62 = msg.reader().readSByte();
                        if (b62 == 0)
                        {
                            effDataById.readData(data5);
                        }
                        else
                        {
                            effDataById.readDataNewBoss(data5, b62);
                        }
                        sbyte[] array15 = NinjaUtil.readByteArray(msg);
                        effDataById.img = Image.createImage(array15, 0, array15.Length);
                        break;
                    }
                case -32:
                    {
                        short num131 = msg.reader().readShort();
                        int num132 = msg.reader().readInt();
                        sbyte[] array11 = null;
                        Image image = null;
                        try
                        {
                            array11 = new sbyte[num132];
                            for (int num133 = 0; num133 < num132; num133++)
                            {
                                array11[num133] = msg.reader().readByte();
                            }
                            image = Image.createImage(array11, 0, num132);
                            BgItem.imgNew.put(num131 + string.Empty, image);
                        }
                        catch (Exception)
                        {
                            array11 = null;
                            BgItem.imgNew.put(num131 + string.Empty, Image.createRGBImage(new int[1], 1, 1, bl: true));
                        }
                        if (array11 != null)
                        {
                            if (mGraphics.zoomLevel > 1)
                            {
                                Rms.saveRMS(mGraphics.zoomLevel + "bgItem" + num131, array11);
                            }
                            BgItemMn.blendcurrBg(num131, image);
                        }
                        break;
                    }
                case 92:
                    {
                        if (GameCanvas.currentScreen == GameScr.instance)
                        {
                            GameCanvas.endDlg();
                        }
                        string text4 = msg.reader().readUTF();
                        string str2 = msg.reader().readUTF();
                        str2 = Res.changeString(str2);
                        string empty = string.Empty;
                        Char char8 = null;
                        sbyte b46 = 0;
                        if (!text4.Equals(string.Empty))
                        {
                            char8 = new Char();
                            char8.charID = msg.reader().readInt();
                            char8.head = msg.reader().readShort();
                            char8.headICON = msg.reader().readShort();
                            char8.body = msg.reader().readShort();
                            char8.bag = msg.reader().readShort();
                            char8.leg = msg.reader().readShort();
                            b46 = msg.reader().readByte();
                            char8.cName = text4;
                            try
                            {
                                char8.isTichXanh = msg.reader().readByte() == 1;
                            }
                            catch (Exception)
                            {
                                char8.isTichXanh = false;
                            }
                        }
                        empty += str2;
                        InfoDlg.hide();
                        if (text4.Equals(string.Empty))
                        {
                            GameScr.info1.addInfo(empty, 0);
                            break;
                        }
                        GameScr.info2.addInfoWithChar(empty, char8, (b46 == 0) ? true : false);
                        if (GameCanvas.panel.isShow && GameCanvas.panel.type == 8)
                        {
                            GameCanvas.panel.initLogMessage();
                        }
                        break;
                    }
                case -26:
                    ServerListScreen.testConnect = 2;
                    string msgDlg = msg.reader().readUTF();
                    if (msgDlg == "Vui lòng mở giới hạn sức mạnh" || msgDlg == "")
                    {
                        ModFunc.indexAutoPoint = -1;
                        ModFunc.pointIncrease = 0;
                        ModFunc.autoPointForPet = false;
                        GameScr.info1.addInfo("Chỉ số đã đạt tối đa", 0);
                    }
                    GameCanvas.startOKDlg(msgDlg);
                    InfoDlg.hide();
                    LoginScr.isContinueToLogin = false;
                    Char.isLoadingMap = false;
                    if (GameCanvas.currentScreen == GameCanvas.loginScr)
                    {
                        GameCanvas.serverScreen.switchToMe();
                        //Main.main.StartCoroutine(ModFunc.GI().AutoLogin2());
                    }
                    if (ModFunc.autoLogin != null)
                    {
                        ModFunc.autoLogin.waitToNextLogin = false;
                    }
                    break;
                case -25:
                    GameScr.info1.addInfo(msg.reader().readUTF(), 0);
                    break;
                case 94:
                    GameScr.info1.addInfo(msg.reader().readUTF(), 0);
                    break;
                case 47:
                    GameScr.gI().resetButton();
                    break;
                case 81:
                    {
                        Mob mob5 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        mob5.isDisable = msg.reader().readBool();
                        break;
                    }
                case 82:
                    {
                        Mob mob5 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        mob5.isDontMove = msg.reader().readBool();
                        break;
                    }
                case 85:
                    {
                        Mob mob5 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        mob5.isFire = msg.reader().readBool();
                        break;
                    }
                case 86:
                    {
                        Mob mob5 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        mob5.isIce = msg.reader().readBool();
                        if (!mob5.isIce)
                        {
                            ServerEffect.addServerEffect(77, mob5.x, mob5.y - 9, 1);
                        }
                        break;
                    }
                case 87:
                    {
                        Mob mob5 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        mob5.isWind = msg.reader().readBool();
                        break;
                    }
                case 56:
                    {
                        @char = null;
                        int charID = msg.reader().readInt();
                        if (charID == Char.myCharz().charID)
                        {
                            bool flag3 = false;
                            @char = Char.myCharz();
                            @char.cHP = msg.reader().readDouble();
                            double dameHit = msg.reader().readDouble();
                            if (dameHit != 0)
                            {
                                @char.doInjure();
                            }
                            try
                            {
                                flag3 = msg.reader().readBoolean();
                                sbyte effId = msg.reader().readByte();
                                if (effId != -1)
                                {
                                    EffecMn.addEff(new Effect(effId, @char.cx, @char.cy, 3, 1, -1));
                                }
                            }
                            catch (Exception)
                            {
                            }
                            if (Char.myCharz().cTypePk != 4)
                            {
                                if (dameHit == 0)
                                {
                                    GameScr.startFlyText(mResources.miss, @char.cx, @char.cy - @char.ch, 0, -3, mFont.MISS_ME);
                                }
                                else
                                {
                                    GameScr.startFlyText("-" + dameHit, @char.cx, @char.cy - @char.ch, 0, -3, flag3 ? mFont.FATAL : mFont.RED);
                                }
                            }
                        }
                        else
                        {
                            @char = GameScr.findCharInMap(charID);
                            if (@char == null)
                            {
                                return;
                            }
                            @char.cHP = msg.reader().readDouble();
                            bool flag4 = false;
                            double dameHit1 = msg.reader().readDouble();
                            if (dameHit1 != 0)
                            {
                                @char.doInjure();
                            }
                            int num32 = 0;
                            try
                            {
                                flag4 = msg.reader().readBoolean();
                                sbyte effId = msg.reader().readByte();
                                if (effId != -1)
                                {
                                    EffecMn.addEff(new Effect(effId, @char.cx, @char.cy, 3, 1, -1));
                                }
                            }
                            catch (Exception)
                            {
                            }
                            dameHit1 += num32;
                            if (@char.cTypePk != 4)
                            {
                                if (dameHit1 == 0)
                                {
                                    GameScr.startFlyText(mResources.miss, @char.cx, @char.cy - @char.ch, 0, -3, mFont.MISS);
                                }
                                else
                                {
                                    GameScr.startFlyText("-" + dameHit1, @char.cx, @char.cy - @char.ch, 0, -3, flag4 ? mFont.FATAL : mFont.ORANGE);
                                }
                            }
                        }
                        break;
                    }
                case 83:
                    {
                        int num18 = msg.reader().readInt();
                        @char = ((num18 != Char.myCharz().charID) ? GameScr.findCharInMap(num18) : Char.myCharz());
                        if (@char == null)
                        {
                            return;
                        }
                        Mob mobToAttack = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        if (@char.mobMe != null)
                        {
                            @char.mobMe.attackOtherMob(mobToAttack);
                        }
                        break;
                    }
                case 84:
                    {
                        int num18 = msg.reader().readInt();
                        if (num18 == Char.myCharz().charID)
                        {
                            @char = Char.myCharz();
                        }
                        else
                        {
                            @char = GameScr.findCharInMap(num18);
                            if (@char == null)
                            {
                                return;
                            }
                        }
                        @char.cHP = @char.cHPFull;
                        @char.cMP = @char.cMPFull;
                        @char.cx = msg.reader().readShort();
                        @char.cy = msg.reader().readShort();
                        @char.liveFromDead();
                        break;
                    }
                case 46:
                    GameCanvas.debug("SA5", 2);
                    Cout.LogWarning("Controler RESET_POINT  " + Char.ischangingMap);
                    Char.isLockKey = false;
                    Char.myCharz().setResetPoint(msg.reader().readShort(), msg.reader().readShort());
                    break;
                case -29:
                    messageNotLogin(msg);
                    break;
                case -28:
                    messageNotMap(msg);
                    break;
                case -30:
                    messageSubCommand(msg);
                    break;
                case 62:
                    @char = GameScr.findCharInMap(msg.reader().readInt());
                    if (@char != null)
                    {
                        @char.killCharId = Char.myCharz().charID;
                        Char.myCharz().npcFocus = null;
                        Char.myCharz().mobFocus = null;
                        Char.myCharz().itemFocus = null;
                        Char.myCharz().charFocus = @char;
                        Char.isManualFocus = true;
                        GameScr.info1.addInfo(@char.cName + mResources.CUU_SAT, 0);
                    }
                    break;
                case 63:
                    Char.myCharz().killCharId = msg.reader().readInt();
                    Char.myCharz().npcFocus = null;
                    Char.myCharz().mobFocus = null;
                    Char.myCharz().itemFocus = null;
                    Char.myCharz().charFocus = GameScr.findCharInMap(Char.myCharz().killCharId);
                    Char.isManualFocus = true;
                    break;
                case 64:
                    GameCanvas.debug("SZ5", 2);
                    @char = Char.myCharz();
                    try
                    {
                        @char = GameScr.findCharInMap(msg.reader().readInt());
                    }
                    catch (Exception ex2)
                    {
                        Cout.println("Loi CLEAR_CUU_SAT " + ex2.ToString());
                    }
                    @char.killCharId = -9999;
                    break;
                case 39:
                    GameCanvas.debug("SA49", 2);
                    GameScr.gI().typeTradeOrder = 2;
                    if (GameScr.gI().typeTrade >= 2 && GameScr.gI().typeTradeOrder >= 2)
                    {
                        InfoDlg.showWait();
                    }
                    break;
                case 57:
                    {
                        GameCanvas.debug("SZ6", 2);
                        MyVector myVector2 = new MyVector();
                        myVector2.addElement(new Command(msg.reader().readUTF(), GameCanvas.instance, 88817, null));
                        GameCanvas.menu.startAt(myVector2, 3);
                        break;
                    }
                case 58:
                    {
                        int charId = msg.reader().readInt();
                        Char char10 = (charId != Char.myCharz().charID) ? GameScr.findCharInMap(charId) : Char.myCharz();
                        char10.moveFast = new short[3];
                        char10.moveFast[0] = 0;
                        short x = msg.reader().readShort();
                        short y = msg.reader().readShort();
                        char10.moveFast[1] = x;
                        char10.moveFast[2] = y;
                        try
                        {
                            charId = msg.reader().readInt();
                            Char char11 = (charId != Char.myCharz().charID) ? GameScr.findCharInMap(charId) : Char.myCharz();
                            char11.cx = x;
                            char11.cy = y;
                        }
                        catch (Exception ex25)
                        {
                            Cout.println("Loi MOVE_FAST " + ex25.ToString());
                        }
                        break;
                    }
                case 88:
                    {
                        string info4 = msg.reader().readUTF();
                        short num170 = msg.reader().readShort();
                        GameCanvas.inputDlg.show(info4, new Command(mResources.ACCEPT, GameCanvas.instance, 88818, num170), TField.INPUT_TYPE_ANY);
                        break;
                    }
                case 27:
                    {
                        myVector = new MyVector();
                        string text7 = msg.reader().readUTF();
                        int num164 = msg.reader().readByte();
                        for (int num165 = 0; num165 < num164; num165++)
                        {
                            string caption4 = msg.reader().readUTF();
                            short num166 = msg.reader().readShort();
                            myVector.addElement(new Command(caption4, GameCanvas.instance, 88819, num166));
                        }
                        GameCanvas.menu.startWithoutCloseButton(myVector, 3);
                        break;
                    }
                case 33:
                    {
                        InfoDlg.hide();
                        GameCanvas.clearKeyHold();
                        GameCanvas.clearKeyPressed();
                        myVector = new MyVector();
                        try
                        {
                            while (true)
                            {
                                string caption3 = msg.reader().readUTF();
                                myVector.addElement(new Command(caption3, GameCanvas.instance, 88822, null));
                            }
                        }
                        catch (Exception ex22)
                        {
                            Cout.println("Loi OPEN_UI_MENU " + ex22.ToString());
                        }
                        if (Char.myCharz().npcFocus == null)
                        {
                            return;
                        }
                        for (int num153 = 0; num153 < Char.myCharz().npcFocus.template.menu.Length; num153++)
                        {
                            string[] array16 = Char.myCharz().npcFocus.template.menu[num153];
                            myVector.addElement(new Command(array16[0], GameCanvas.instance, 88820, array16));
                        }
                        GameCanvas.menu.startAt(myVector, 3);
                        break;
                    }
                case 40:
                    {
                        GameCanvas.debug("SA52", 2);
                        GameCanvas.taskTick = 150;
                        short taskId = msg.reader().readShort();
                        sbyte index3 = msg.reader().readByte();
                        string str3 = msg.reader().readUTF();
                        str3 = Res.changeString(str3);
                        string str4 = msg.reader().readUTF();
                        str4 = Res.changeString(str4);
                        string[] array12 = new string[msg.reader().readByte()];
                        string[] array13 = new string[array12.Length];
                        GameScr.tasks = new int[array12.Length];
                        GameScr.mapTasks = new int[array12.Length];
                        short[] array14 = new short[array12.Length];
                        short count = -1;
                        for (int num134 = 0; num134 < array12.Length; num134++)
                        {
                            string str5 = msg.reader().readUTF();
                            str5 = Res.changeString(str5);
                            GameScr.tasks[num134] = msg.reader().readByte();
                            GameScr.mapTasks[num134] = msg.reader().readShort();
                            string str6 = msg.reader().readUTF();
                            str6 = Res.changeString(str6);
                            array14[num134] = -1;
                            if (!str5.Equals(string.Empty))
                            {
                                array12[num134] = str5;
                                array13[num134] = str6;
                            }
                        }
                        try
                        {
                            count = msg.reader().readShort();
                            for (int num135 = 0; num135 < array12.Length; num135++)
                            {
                                array14[num135] = msg.reader().readShort();
                            }
                        }
                        catch (Exception ex21)
                        {
                            Cout.println("Loi TASK_GET " + ex21.ToString());
                        }
                        Char.myCharz().taskMaint = new Task(taskId, index3, str3, str4, array12, array14, count, array13);
                        if (Char.myCharz().npcFocus != null)
                        {
                            Npc.clearEffTask();
                        }
                        Char.taskAction(isNextStep: false);
                        break;
                    }
                case 41:
                    GameCanvas.debug("SA53", 2);
                    GameCanvas.taskTick = 100;
                    Res.outz("TASK NEXT");
                    Char.myCharz().taskMaint.index++;
                    Char.myCharz().taskMaint.count = 0;
                    Npc.clearEffTask();
                    Char.taskAction(isNextStep: true);
                    break;
                case 50:
                    {
                        sbyte b59 = msg.reader().readByte();
                        Panel.vGameInfo.removeAllElements();
                        for (int num130 = 0; num130 < b59; num130++)
                        {
                            GameInfo gameInfo = new GameInfo();
                            gameInfo.id = msg.reader().readShort();
                            gameInfo.main = msg.reader().readUTF();
                            gameInfo.content = msg.reader().readUTF();
                            Panel.vGameInfo.addElement(gameInfo);
                            bool hasRead = Rms.loadRMSInt(gameInfo.id + string.Empty) != -1;
                            gameInfo.hasRead = hasRead;
                        }
                        break;
                    }
                case 43:
                    GameCanvas.taskTick = 50;
                    GameCanvas.debug("SA55", 2);
                    Char.myCharz().taskMaint.count = msg.reader().readShort();
                    if (Char.myCharz().npcFocus != null)
                    {
                        Npc.clearEffTask();
                    }
                    try
                    {
                        short num127 = msg.reader().readShort();
                        short num128 = msg.reader().readShort();
                        Char.myCharz().x_hint = num127;
                        Char.myCharz().y_hint = num128;
                        Res.outz("CMD   TASK_UPDATE:43_mapID =    x|y " + num127 + "|" + num128);
                        for (int num129 = 0; num129 < TileMap.vGo.size(); num129++)
                        {
                            Res.outz("===> " + TileMap.vGo.elementAt(num129));
                        }
                    }
                    catch (Exception)
                    {
                    }
                    break;
                case 90:
                    GameCanvas.debug("SA577", 2);
                    requestItemPlayer(msg);
                    break;
                case 29:
                    GameCanvas.debug("SA58", 2);
                    GameScr.gI().openUIZone(msg);
                    break;
                case -21:
                    {
                        GameCanvas.debug("SA60", 2);
                        short itemMapID = msg.reader().readShort();
                        for (int num123 = 0; num123 < GameScr.vItemMap.size(); num123++)
                        {
                            if (((ItemMap)GameScr.vItemMap.elementAt(num123)).itemMapID == itemMapID)
                            {
                                GameScr.vItemMap.removeElementAt(num123);
                                break;
                            }
                        }
                        break;
                    }
                case -20:
                    {
                        GameCanvas.debug("SA61", 2);
                        Char.myCharz().itemFocus = null;
                        short itemMapID = msg.reader().readShort();
                        for (int num116 = 0; num116 < GameScr.vItemMap.size(); num116++)
                        {
                            ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(num116);
                            if (itemMap2.itemMapID != itemMapID)
                            {
                                continue;
                            }
                            itemMap2.setPoint(Char.myCharz().cx, Char.myCharz().cy - 10);
                            string text5 = msg.reader().readUTF();
                            num = 0;
                            try
                            {
                                num = msg.reader().readShort();
                                if (itemMap2.template.type == 9)
                                {
                                    num = msg.reader().readShort();
                                    Char.myCharz().xu += num;
                                    Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
                                }
                                else if (itemMap2.template.type == 10)
                                {
                                    num = msg.reader().readShort();
                                    Char.myCharz().luong += num;
                                    Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
                                }
                                else if (itemMap2.template.type == 34)
                                {
                                    num = msg.reader().readShort();
                                    Char.myCharz().luongKhoa += num;
                                    Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            if (text5.Equals(string.Empty))
                            {
                                if (itemMap2.template.type == 9)
                                {
                                    GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.YELLOW);
                                    SoundMn.gI().getItem();
                                }
                                else if (itemMap2.template.type == 10)
                                {
                                    GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.GREEN);
                                    SoundMn.gI().getItem();
                                }
                                else if (itemMap2.template.type == 34)
                                {
                                    GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.RED);
                                    SoundMn.gI().getItem();
                                }
                                else
                                {
                                    GameScr.info1.addInfo(mResources.you_receive + " " + ((num <= 0) ? string.Empty : (num + " ")) + itemMap2.template.name, 0);
                                    SoundMn.gI().getItem();
                                }
                                if (num > 0 && Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 4683)
                                {
                                    ServerEffect.addServerEffect(55, Char.myCharz().petFollow.cmx, Char.myCharz().petFollow.cmy, 1);
                                    ServerEffect.addServerEffect(55, Char.myCharz().cx, Char.myCharz().cy, 1);
                                }
                            }
                            else if (text5.Length == 1)
                            {
                                Cout.LogError3("strInf.Length =1:  " + text5);
                            }
                            else
                            {
                                GameScr.info1.addInfo(text5, 0);
                            }
                            break;
                        }
                        break;
                    }
                case -19:
                    {
                        short itemMapID = msg.reader().readShort();
                        @char = GameScr.findCharInMap(msg.reader().readInt());
                        for (int num115 = 0; num115 < GameScr.vItemMap.size(); num115++)
                        {
                            ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(num115);
                            if (itemMap.itemMapID != itemMapID)
                            {
                                continue;
                            }
                            if (@char == null)
                            {
                                return;
                            }
                            itemMap.setPoint(@char.cx, @char.cy - 10);
                            if (itemMap.x < @char.cx)
                            {
                                @char.cdir = -1;
                            }
                            else if (itemMap.x > @char.cx)
                            {
                                @char.cdir = 1;
                            }
                            break;
                        }
                        break;
                    }
                case -18:
                    {
                        GameCanvas.debug("SA63", 2);
                        int num114 = msg.reader().readByte();
                        GameScr.vItemMap.addElement(new ItemMap(msg.reader().readShort(), Char.myCharz().arrItemBag[num114].template.id, Char.myCharz().cx, Char.myCharz().cy, msg.reader().readShort(), msg.reader().readShort()));
                        Char.myCharz().arrItemBag[num114] = null;
                        break;
                    }
                case 68:
                    {
                        short itemMapID = msg.reader().readShort();
                        short itemTemplateID = msg.reader().readShort();
                        int x = msg.reader().readShort();
                        int y = msg.reader().readShort();
                        int num107 = msg.reader().readInt();
                        short r = 0;
                        if (num107 == -2)
                        {
                            r = msg.reader().readShort();
                        }
                        ItemMap o2 = new(num107, itemMapID, itemTemplateID, x, y, r);
                        GameScr.vItemMap.addElement(o2);
                        break;
                    }
                case 69:
                    SoundMn.IsDelAcc = ((msg.reader().readByte() != 0) ? true : false);
                    break;
                case -14:
                    @char = GameScr.findCharInMap(msg.reader().readInt());
                    if (@char == null)
                    {
                        return;
                    }
                    GameScr.vItemMap.addElement(new ItemMap(msg.reader().readShort(), msg.reader().readShort(), @char.cx, @char.cy, msg.reader().readShort(), msg.reader().readShort()));
                    break;
                case -22:
                    Char.isLockKey = true;
                    Char.ischangingMap = true;
                    GameScr.gI().timeStartMap = 0;
                    GameScr.gI().timeLengthMap = 0;
                    Char.myCharz().mobFocus = null;
                    Char.myCharz().npcFocus = null;
                    Char.myCharz().charFocus = null;
                    Char.myCharz().itemFocus = null;
                    Char.myCharz().focus.removeAllElements();
                    Char.myCharz().testCharId = -9999;
                    Char.myCharz().killCharId = -9999;
                    GameCanvas.resetBg();
                    GameScr.gI().resetButton();
                    GameScr.gI().center = null;
                    break;
                case -70:
                    {
                        GameCanvas.endDlg();
                        if (PickMob.tanSat)
                        {
                            ModFunc.GI().perform(44, true);
                            return;
                        }
                        int avatar2 = msg.reader().readShort();
                        string chat3 = msg.reader().readUTF();
                        Npc npc6 = new(-1, 0, 0, 0, 0, 0)
                        {
                            avatar = avatar2
                        };
                        ChatPopup.addBigMessage(chat3, 100000, npc6);
                        sbyte type = msg.reader().readByte();
                        if (type == 0)
                        {
                            ChatPopup.serverChatPopUp.cmdMsg1 = new Command(mResources.CLOSE, ChatPopup.serverChatPopUp, 1001, null)
                            {
                                x = GameCanvas.w / 2 - 35,
                                y = GameCanvas.h - 35
                            };
                        }
                        if (type == 1)
                        {
                            string p2 = msg.reader().readUTF();
                            string caption2 = msg.reader().readUTF();
                            ChatPopup.serverChatPopUp.cmdMsg1 = new Command(mResources.CLOSE, ChatPopup.serverChatPopUp, 1001, null)
                            {
                                x = GameCanvas.w / 2 + 11,
                                y = GameCanvas.h - 35
                            };
                            ChatPopup.serverChatPopUp.cmdMsg2 = new Command(caption2, ChatPopup.serverChatPopUp, 1000, p2)
                            {
                                x = GameCanvas.w / 2 - 75,
                                y = GameCanvas.h - 35
                            };
                        }
                        break;
                    }
                case 38:
                    {
                        InfoDlg.hide();
                        int num85 = msg.reader().readShort();
                        string str = msg.reader().readUTF();
                        str = Res.changeString(str);
                        for (int num103 = 0; num103 < GameScr.vNpc.size(); num103++)
                        {
                            Npc npc4 = (Npc)GameScr.vNpc.elementAt(num103);
                            if (npc4.template.npcTemplateId == num85)
                            {
                                ChatPopup.addChatPopupMultiLine(str, 100000, npc4);
                                GameCanvas.panel.hideNow();
                                return;
                            }
                        }
                        Npc npc5 = new Npc(num85, 0, 0, 0, num85, GameScr.info1.charId[Char.myCharz().cgender][2]);
                        if (npc5.template.npcTemplateId == 5)
                        {
                            npc5.charID = 5;
                        }
                        try
                        {
                            npc5.avatar = msg.reader().readShort();
                        }
                        catch (Exception)
                        {
                        }
                        ChatPopup.addChatPopupMultiLine(str, 100000, npc5);
                        GameCanvas.panel.hideNow();
                        break;
                    }
                case 32:
                    {
                        int npcId = msg.reader().readShort();
                        for (int i = 0; i < GameScr.vNpc.size(); i++)
                        {
                            Npc npc = (Npc)GameScr.vNpc.elementAt(i);
                            if (npc.template.npcTemplateId == npcId && npc.Equals(Char.myCharz().npcFocus))
                            {
                                string chat = msg.reader().readUTF();
                                string[] menu = new string[msg.reader().readByte()];
                                for (int num87 = 0; num87 < menu.Length; num87++)
                                {
                                    menu[num87] = msg.reader().readUTF();
                                }
                                GameScr.gI().createMenu(menu, npc);
                                ChatPopup.addChatPopup(chat, 100000, npc);

                                if (npcId == 21 && chat.Contains("tối đa"))
                                {
                                    ModFunc.GI().maxPhale = ModFunc.GI().currPhale;
                                }

                                return;
                            }
                        }
                        Npc npc1 = new(npcId, 0, -100, 100, npcId, GameScr.info1.charId[Char.myCharz().cgender][2]);
                        string chat1 = msg.reader().readUTF();
                        string[] menu1 = new string[msg.reader().readByte()];
                        for (int j = 0; j < menu1.Length; j++)
                        {
                            menu1[j] = msg.reader().readUTF();
                        }
                        try
                        {
                            short avatar = msg.reader().readShort();
                            npc1.avatar = avatar;
                        }
                        catch (Exception)
                        {
                        }
                        GameScr.gI().createMenu(menu1, npc1);
                        ChatPopup.addChatPopup(chat1, 100000, npc1);
                        break;
                    }
                case 7:
                    {
                        sbyte type = msg.reader().readByte();
                        short id = msg.reader().readShort();
                        string info3 = msg.reader().readUTF();
                        GameCanvas.panel.saleRequest(type, info3, id);
                        break;
                    }
                case 6:
                    Char.myCharz().xu = msg.reader().readLong();
                    Char.myCharz().luong = msg.reader().readInt();
                    Char.myCharz().luongKhoa = msg.reader().readInt();
                    Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
                    Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
                    Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
                    GameCanvas.endDlg();
                    break;
                case -23:
                    LoadAuraNpcs(msg);
                    break;
                case -24:
                    if (GameCanvas.currentScreen is GameScr)
                    {
                        GameCanvas.timeBreakLoading = mSystem.currentTimeMillis() + 3000;
                    }
                    else
                    {
                        GameCanvas.timeBreakLoading = mSystem.currentTimeMillis() + 30000;
                    }
                    Char.isLoadingMap = true;
                    GameScr.gI().magicTree = null;
                    GameCanvas.isLoading = true;
                    GameScr.resetAllvector();
                    GameCanvas.endDlg();
                    TileMap.vGo.removeAllElements();
                    PopUp.vPopups.removeAllElements();
                    mSystem.gcc();
                    TileMap.mapID = msg.reader().readUnsignedByte();
                    TileMap.planetID = msg.reader().readByte();
                    TileMap.tileID = msg.reader().readByte();
                    TileMap.bgID = msg.reader().readByte();
                    TileMap.typeMap = msg.reader().readByte();
                    TileMap.mapName = msg.reader().readUTF();
                    TileMap.zoneID = msg.reader().readByte();
                    try
                    {
                        TileMap.loadMapFromResource(TileMap.mapID);
                    }
                    catch (Exception)
                    {
                        Service.gI().requestMaptemplate(TileMap.mapID);
                        messWait = msg;
                        return;
                    }
                    loadInfoMap(msg);
                    try
                    {
                        sbyte b33 = msg.reader().readByte();
                        TileMap.isMapDouble = ((b33 != 0) ? true : false);
                    }
                    catch (Exception)
                    {
                    }
                    GameScr.cmx = GameScr.cmtoX;
                    GameScr.cmy = GameScr.cmtoY;
                    break;
                case -31:
                    {
                        TileMap.vItemBg.removeAllElements();
                        short num71 = msg.reader().readShort();
                        for (int num72 = 0; num72 < num71; num72++)
                        {
                            BgItem bgItem = new BgItem();
                            bgItem.id = num72;
                            bgItem.idImage = msg.reader().readShort();
                            bgItem.layer = msg.reader().readByte();
                            bgItem.dx = msg.reader().readShort();
                            bgItem.dy = msg.reader().readShort();
                            sbyte b32 = msg.reader().readByte();
                            bgItem.tileX = new int[b32];
                            bgItem.tileY = new int[b32];
                            for (int num73 = 0; num73 < b32; num73++)
                            {
                                bgItem.tileX[num72] = msg.reader().readByte();
                                bgItem.tileY[num72] = msg.reader().readByte();
                            }
                            TileMap.vItemBg.addElement(bgItem);
                        }
                        break;
                    }
                case -4:
                    {
                        GameCanvas.debug("SA76", 2);
                        @char = GameScr.findCharInMap(msg.reader().readInt());
                        if (@char == null)
                        {
                            return;
                        }
                        GameCanvas.debug("SA76v1", 2);
                        if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
                        {
                            @char.setSkillPaint(GameScr.sks[msg.reader().readUnsignedByte()], 0);
                        }
                        else
                        {
                            @char.setSkillPaint(GameScr.sks[msg.reader().readUnsignedByte()], 1);
                        }
                        GameCanvas.debug("SA76v2", 2);
                        @char.attMobs = new Mob[msg.reader().readByte()];
                        for (int num26 = 0; num26 < @char.attMobs.Length; num26++)
                        {
                            Mob mob3 = (Mob)GameScr.vMob.elementAt(msg.reader().readByte());
                            @char.attMobs[num26] = mob3;
                            if (num26 == 0)
                            {
                                if (@char.cx <= mob3.x)
                                {
                                    @char.cdir = 1;
                                }
                                else
                                {
                                    @char.cdir = -1;
                                }
                            }
                        }
                        GameCanvas.debug("SA76v3", 2);
                        @char.charFocus = null;
                        @char.mobFocus = @char.attMobs[0];
                        Char[] array = new Char[10];
                        num = 0;
                        try
                        {
                            for (num = 0; num < array.Length; num++)
                            {
                                int num18 = msg.reader().readInt();
                                Char char4 = (array[num] = ((num18 != Char.myCharz().charID) ? GameScr.findCharInMap(num18) : Char.myCharz()));
                                if (num == 0)
                                {
                                    if (@char.cx <= char4.cx)
                                    {
                                        @char.cdir = 1;
                                    }
                                    else
                                    {
                                        @char.cdir = -1;
                                    }
                                }
                            }
                        }
                        catch (Exception ex5)
                        {
                            Cout.println("Loi PLAYER_ATTACK_N_P " + ex5.ToString());
                        }
                        GameCanvas.debug("SA76v4", 2);
                        if (num > 0)
                        {
                            @char.attChars = new Char[num];
                            for (num = 0; num < @char.attChars.Length; num++)
                            {
                                @char.attChars[num] = array[num];
                            }
                            @char.charFocus = @char.attChars[0];
                            @char.mobFocus = null;
                        }
                        GameCanvas.debug("SA76v5", 2);
                        break;
                    }
                case 54:
                    {
                        @char = GameScr.findCharInMap(msg.reader().readInt());
                        if (@char == null)
                        {
                            return;
                        }
                        int num17 = msg.reader().readUnsignedByte();
                        if ((TileMap.tileTypeAtPixel(@char.cx, @char.cy) & 2) == 2)
                        {
                            @char.setSkillPaint(GameScr.sks[num17], 0);
                        }
                        else
                        {
                            @char.setSkillPaint(GameScr.sks[num17], 1);
                        }
                        Mob[] array3 = new Mob[10];
                        num = 0;
                        try
                        {
                            for (num = 0; num < array3.Length; num++)
                            {
                                Mob mob2 = (array3[num] = (Mob)GameScr.vMob.elementAt(msg.reader().readByte()));
                                if (num == 0)
                                {
                                    if (@char.cx <= mob2.x)
                                    {
                                        @char.cdir = 1;
                                    }
                                    else
                                    {
                                        @char.cdir = -1;
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        if (num > 0)
                        {
                            @char.attMobs = new Mob[num];
                            for (num = 0; num < @char.attMobs.Length; num++)
                            {
                                @char.attMobs[num] = array3[num];
                            }
                            @char.charFocus = null;
                            @char.mobFocus = @char.attMobs[0];
                        }
                        break;
                    }
                case -60:
                    {
                        GameCanvas.debug("SA7666", 2);
                        int num2 = msg.reader().readInt();
                        int num3 = -1;
                        if (num2 != Char.myCharz().charID)
                        {
                            Char char2 = GameScr.findCharInMap(num2);
                            if (char2 == null)
                            {
                                return;
                            }
                            if (char2.currentMovePoint != null)
                            {
                                char2.createShadow(char2.cx, char2.cy, 10);
                                char2.cx = char2.currentMovePoint.xEnd;
                                char2.cy = char2.currentMovePoint.yEnd;
                            }
                            int num4 = msg.reader().readUnsignedByte();
                            if ((TileMap.tileTypeAtPixel(char2.cx, char2.cy) & 2) == 2)
                            {
                                char2.setSkillPaint(GameScr.sks[num4], 0);
                            }
                            else
                            {
                                char2.setSkillPaint(GameScr.sks[num4], 1);
                            }
                            sbyte b = msg.reader().readByte();
                            Char[] array = new Char[b];
                            for (num = 0; num < array.Length; num++)
                            {
                                num3 = msg.reader().readInt();
                                Char char3;
                                if (num3 == Char.myCharz().charID)
                                {
                                    char3 = Char.myCharz();
                                    if (!GameScr.isChangeZone && GameScr.isAutoPlay && GameScr.canAutoPlay)
                                    {
                                        Service.gI().requestChangeZone(-1, -1);
                                        GameScr.isChangeZone = true;
                                    }
                                }
                                else
                                {
                                    char3 = GameScr.findCharInMap(num3);
                                }
                                array[num] = char3;
                                if (num == 0)
                                {
                                    if (char2.cx <= char3.cx)
                                    {
                                        char2.cdir = 1;
                                    }
                                    else
                                    {
                                        char2.cdir = -1;
                                    }
                                }
                            }
                            if (num > 0)
                            {
                                char2.attChars = new Char[num];
                                for (num = 0; num < char2.attChars.Length; num++)
                                {
                                    char2.attChars[num] = array[num];
                                }
                                char2.mobFocus = null;
                                char2.charFocus = char2.attChars[0];
                            }
                        }
                        else
                        {
                            sbyte b2 = msg.reader().readByte();
                            sbyte b3 = msg.reader().readByte();
                            num3 = msg.reader().readInt();
                        }
                        try
                        {
                            sbyte b4 = msg.reader().readByte();
                            if (b4 != 1)
                            {
                                break;
                            }
                            sbyte b5 = msg.reader().readByte();
                            if (num3 == Char.myCharz().charID)
                            {
                                bool flag = false;
                                @char = Char.myCharz();
                                double num5 = msg.reader().readDouble();
                                @char.isDie = msg.reader().readBoolean();
                                if (@char.isDie)
                                {
                                    Char.isLockKey = true;
                                }
                                long num6 = 0;
                                flag = (@char.isCrit = msg.reader().readBoolean());
                                @char.isMob = false;
                                num5 = (@char.damHP = num5 + num6);
                                if (b5 == 0)
                                {
                                    @char.doInjure(num5, 0, flag, isMob: false);
                                }
                            }
                            else
                            {
                                @char = GameScr.findCharInMap(num3);
                                if (@char == null)
                                {
                                    return;
                                }
                                bool flag2 = false;
                                double num7 = msg.reader().readDouble();
                                @char.isDie = msg.reader().readBoolean();
                                long num8 = 0;
                                flag2 = (@char.isCrit = msg.reader().readBoolean());
                                @char.isMob = false;
                                num7 = (@char.damHP = num7 + num8);
                                if (b5 == 0)
                                {
                                    @char.doInjure(num7, 0, flag2, isMob: false);
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    }
            }
            switch (msg.command)
            {
                case -2:
                    {
                        GameCanvas.debug("SA77", 22);
                        int num195 = msg.reader().readInt();
                        Char.myCharz().yen += num195;
                        GameScr.startFlyText((num195 <= 0) ? (string.Empty + num195) : ("+" + num195), Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
                        break;
                    }
                case 95:
                    {
                        GameCanvas.debug("SA77", 22);
                        int num182 = msg.reader().readInt();
                        Char.myCharz().xu += num182;
                        Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
                        GameScr.startFlyText((num182 <= 0) ? (string.Empty + num182) : ("+" + num182), Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
                        break;
                    }
                case 96:
                    GameCanvas.debug("SA77a", 22);
                    Char.myCharz().taskOrders.addElement(new TaskOrder(msg.reader().readByte(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readUTF(), msg.reader().readUTF(), msg.reader().readByte(), msg.reader().readByte()));
                    break;
                case 97:
                    {
                        sbyte b75 = msg.reader().readByte();
                        for (int num188 = 0; num188 < Char.myCharz().taskOrders.size(); num188++)
                        {
                            TaskOrder taskOrder = (TaskOrder)Char.myCharz().taskOrders.elementAt(num188);
                            if (taskOrder.taskId == b75)
                            {
                                taskOrder.count = msg.reader().readShort();
                                break;
                            }
                        }
                        break;
                    }
                case -1:
                    {
                        GameCanvas.debug("SA77", 222);
                        int num194 = msg.reader().readInt();
                        Char.myCharz().xu += num194;
                        Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
                        Char.myCharz().yen -= num194;
                        GameScr.startFlyText("+" + num194, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
                        break;
                    }
                case -3:
                    {
                        sbyte type = msg.reader().readByte();
                        long param = msg.reader().readInt();
                        if (type == 0)
                        {
                            Char.myCharz().cPower += param;
                        }
                        if (type == 1)
                        {
                            Char.myCharz().cTiemNang += param;
                        }
                        if (type == 2)
                        {
                            Char.myCharz().cPower += param;
                            Char.myCharz().cTiemNang += param;
                        }
                        Char.myCharz().applyCharLevelPercent();
                        if (Char.myCharz().cTypePk != 3)
                        {
                            GameScr.startFlyText(((param <= 0) ? string.Empty : "+") + param, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -4, mFont.GREEN);
                            if (param > 0 && Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5002)
                            {
                                ServerEffect.addServerEffect(55, Char.myCharz().petFollow.cmx, Char.myCharz().petFollow.cmy, 1);
                                ServerEffect.addServerEffect(55, Char.myCharz().cx, Char.myCharz().cy, 1);
                            }
                        }
                        break;
                    }
                case -73:
                    {
                        sbyte npcId = msg.reader().readByte();
                        for (int i = 0; i < GameScr.vNpc.size(); i++)
                        {
                            Npc npc7 = (Npc)GameScr.vNpc.elementAt(i);
                            if (npc7.template.npcTemplateId == npcId)
                            {
                                sbyte isHide = msg.reader().readByte();
                                if (isHide == 0)
                                {
                                    npc7.isHide = true;
                                }
                                else
                                {
                                    npc7.isHide = false;
                                }
                                break;
                            }
                        }
                        break;
                    }
                case -5:
                    {
                        int charID = msg.reader().readInt();
                        int num184 = msg.reader().readInt();
                        Char char15;
                        if (num184 != -100)
                        {
                            char15 = new Char
                            {
                                charID = charID,
                                clanID = num184
                            };
                        }
                        else
                        {
                            char15 = new Mabu
                            {
                                charID = charID,
                                clanID = num184
                            };
                        }
                        if (char15.clanID == -2)
                        {
                            char15.isCopy = true;
                        }
                        if (readCharInfo(char15, msg))
                        {
                            sbyte b73 = msg.reader().readByte();
                            if (char15.cy <= 10 && b73 != 0 && b73 != 2)
                            {
                                Teleport teleport2 = new Teleport(char15.cx, char15.cy, char15.head, char15.cdir, 1, isMe: false, (b73 != 1) ? b73 : char15.cgender);
                                teleport2.id = char15.charID;
                                char15.isTeleport = true;
                                Teleport.addTeleport(teleport2);
                            }
                            if (b73 == 2)
                            {
                                char15.show();
                            }
                            for (int num185 = 0; num185 < GameScr.vMob.size(); num185++)
                            {
                                Mob mob10 = (Mob)GameScr.vMob.elementAt(num185);
                                if (mob10 != null && mob10.isMobMe && mob10.mobId == char15.charID)
                                {
                                    char15.mobMe = mob10;
                                    char15.mobMe.x = char15.cx;
                                    char15.mobMe.y = char15.cy - 40;
                                    break;
                                }
                            }
                            if (GameScr.findCharInMap(char15.charID) == null)
                            {
                                GameScr.vCharInMap.addElement(char15);
                            }
                            char15.isMonkey = msg.reader().readByte();
                            short num186 = msg.reader().readShort();
                            if (num186 != -1)
                            {
                                char15.isHaveMount = true;
                                switch (num186)
                                {
                                    case 346:
                                    case 347:
                                    case 348:
                                        char15.isMountVip = false;
                                        break;
                                    case 349:
                                    case 350:
                                    case 351:
                                        char15.isMountVip = true;
                                        break;
                                    case 396:
                                        char15.isEventMount = true;
                                        break;
                                    case 532:
                                        char15.isSpeacialMount = true;
                                        break;
                                    default:
                                        if (num186 >= Char.ID_NEW_MOUNT)
                                        {
                                            char15.idMount = num186;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                char15.isHaveMount = false;
                            }
                        }
                        sbyte b74 = msg.reader().readByte();
                        char15.cFlag = b74;
                        char15.isNhapThe = msg.reader().readByte() == 1;
                        try
                        {
                            char15.idAuraEff = msg.reader().readShort();
                            char15.idEff_Set_Item = msg.reader().readSByte();
                            char15.idHat = msg.reader().readShort();
                            if (char15.bag >= 201 && char15.bag < 255)
                            {
                                Effect effect2 = new Effect(char15.bag, char15, 2, -1, 10, 1);
                                effect2.typeEff = 5;
                                char15.addEffChar(effect2);
                            }
                            else
                            {
                                for (int num187 = 0; num187 < 54; num187++)
                                {
                                    char15.removeEffChar(0, 201 + num187);
                                }
                            }
                        }
                        catch (Exception ex37)
                        {
                            Res.outz("cmd: -5 err: " + ex37.StackTrace);
                        }
                        char15.isTichXanh = msg.reader().readByte() == 1;
                        GameScr.gI().getFlagImage(char15.charID, char15.cFlag);
                        break;
                    }
                case -7:
                    {
                        int num177 = msg.reader().readInt();
                        for (int num180 = 0; num180 < GameScr.vCharInMap.size(); num180++)
                        {
                            Char char14 = null;
                            try
                            {
                                char14 = (Char)GameScr.vCharInMap.elementAt(num180);
                            }
                            catch (Exception)
                            {
                            }
                            if (char14 == null)
                            {
                                break;
                            }
                            if (char14.charID == num177)
                            {
                                GameCanvas.debug("SA8x2y" + num180, 2);
                                char14.moveTo(msg.reader().readShort(), msg.reader().readShort(), 0);
                                char14.lastUpdateTime = mSystem.currentTimeMillis();
                                break;
                            }
                        }
                        GameCanvas.debug("SA80x3", 2);
                        break;
                    }
                case -6:
                    {
                        GameCanvas.debug("SA81", 2);
                        int num177 = msg.reader().readInt();
                        for (int num178 = 0; num178 < GameScr.vCharInMap.size(); num178++)
                        {
                            Char char13 = (Char)GameScr.vCharInMap.elementAt(num178);
                            if (char13 != null && char13.charID == num177)
                            {
                                if (!char13.isInvisiblez && !char13.isUsePlane)
                                {
                                    ServerEffect.addServerEffect(60, char13.cx, char13.cy, 1);
                                }
                                if (!char13.isUsePlane)
                                {
                                    GameScr.vCharInMap.removeElementAt(num178);
                                }
                                return;
                            }
                        }
                        break;
                    }
                case -13:
                    {
                        int num189 = msg.reader().readUnsignedByte();
                        if (num189 > GameScr.vMob.size() - 1 || num189 < 0)
                        {
                            return;
                        }
                        Mob mob9 = (Mob)GameScr.vMob.elementAt(num189);
                        if (mob9.status != 0 && mob9.status != 1)
                        {
                            return;
                        }
                        mob9.sys = msg.reader().readByte();
                        mob9.levelBoss = msg.reader().readByte();
                        if (mob9.levelBoss != 0)
                        {
                            mob9.typeSuperEff = Res.random(0, 3);
                        }
                        mob9.x = mob9.xFirst;
                        mob9.y = mob9.yFirst;
                        mob9.status = 5;
                        mob9.injureThenDie = false;
                        mob9.hp = msg.reader().readDouble();
                        mob9.maxHp = mob9.hp;
                        mob9.updateHp_bar();
                        ServerEffect.addServerEffect(60, mob9.x, mob9.y, 1);
                        break;
                    }
                case -75:
                    {
                        Mob mob9 = null;
                        try
                        {
                            mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        }
                        catch (Exception)
                        {
                        }
                        if (mob9 != null)
                        {
                            mob9.levelBoss = msg.reader().readByte();
                            if (mob9.levelBoss > 0)
                            {
                                mob9.typeSuperEff = Res.random(0, 3);
                            }
                        }
                        break;
                    }
                case -9:
                    {
                        Mob mob9 = null;
                        try
                        {
                            mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        }
                        catch (Exception)
                        {
                        }
                        if (mob9 != null)
                        {
                            mob9.hp = msg.reader().readDouble();
                            mob9.updateHp_bar();
                            double dame = msg.reader().readDouble();
                            if (dame == 1)
                            {
                                return;
                            }
                            if (dame > 1)
                            {
                                mob9.setInjure();
                            }
                            bool flag10 = false;
                            try
                            {
                                flag10 = msg.reader().readBoolean();
                            }
                            catch (Exception)
                            {
                            }
                            sbyte b72 = msg.reader().readByte();
                            if (b72 != -1)
                            {
                                EffecMn.addEff(new Effect(b72, mob9.x, mob9.getY(), 3, 1, -1));
                            }
                            if (flag10)
                            {
                                GameScr.startFlyText("-" + dame, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.FATAL);
                            }
                            else if (dame == 0)
                            {
                                mob9.x = mob9.xFirst;
                                mob9.y = mob9.yFirst;
                                GameScr.startFlyText(mResources.miss, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.MISS);
                            }
                            else if (dame > 1)
                            {
                                GameScr.startFlyText("-" + dame, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.ORANGE);
                            }
                        }
                        break;
                    }
                case 45:
                    {
                        Mob mob9 = null;
                        try
                        {
                            mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        }
                        catch (Exception ex28)
                        {
                        }
                        if (mob9 != null)
                        {
                            mob9.hp = msg.reader().readInt();
                            mob9.updateHp_bar();
                            GameScr.startFlyText(mResources.miss, mob9.x, mob9.y - mob9.h, 0, -2, mFont.MISS);
                        }
                        break;
                    }
                case -12:
                    {
                        Mob mob9 = null;
                        try
                        {
                            mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        }
                        catch (Exception)
                        {
                        }
                        if (mob9 == null || mob9.status == 0 || mob9.status == 0)
                        {
                            break;
                        }
                        mob9.startDie();
                        try
                        {
                            double dameHit = msg.reader().readDouble();
                            if (msg.reader().readBool())
                            {
                                GameScr.startFlyText("-" + dameHit, mob9.x, mob9.y - mob9.h, 0, -2, mFont.FATAL);
                            }
                            else
                            {
                                GameScr.startFlyText("-" + dameHit, mob9.x, mob9.y - mob9.h, 0, -2, mFont.ORANGE);
                            }
                            sbyte b76 = msg.reader().readByte();
                            for (int num191 = 0; num191 < b76; num191++)
                            {
                                ItemMap itemMap4 = new ItemMap(msg.reader().readShort(), msg.reader().readShort(), mob9.x, mob9.y, msg.reader().readShort(), msg.reader().readShort());
                                int num192 = (itemMap4.playerId = msg.reader().readInt());
                                GameScr.vItemMap.addElement(itemMap4);
                                if (Res.abs(itemMap4.y - Char.myCharz().cy) < 24 && Res.abs(itemMap4.x - Char.myCharz().cx) < 24)
                                {
                                    Char.myCharz().charFocus = null;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    }
                case 74:
                    {
                        Mob mob9 = null;
                        try
                        {
                            mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        }
                        catch (Exception)
                        {
                        }
                        if (mob9 != null && mob9.status != 0 && mob9.status != 0)
                        {
                            mob9.status = 0;
                            ServerEffect.addServerEffect(60, mob9.x, mob9.y, 1);
                            ItemMap itemMap3 = new ItemMap(msg.reader().readShort(), msg.reader().readShort(), mob9.x, mob9.y, msg.reader().readShort(), msg.reader().readShort());
                            GameScr.vItemMap.addElement(itemMap3);
                            if (Res.abs(itemMap3.y - Char.myCharz().cy) < 24 && Res.abs(itemMap3.x - Char.myCharz().cx) < 24)
                            {
                                Char.myCharz().charFocus = null;
                            }
                        }
                        break;
                    }
                case -11:
                    {
                        Mob mob9 = null;
                        try
                        {
                            int index4 = msg.reader().readUnsignedByte();
                            mob9 = (Mob)GameScr.vMob.elementAt(index4);
                        }
                        catch (Exception)
                        {
                        }
                        if (mob9 != null)
                        {
                            Char.myCharz().isDie = false;
                            Char.isLockKey = false;
                            double dame = msg.reader().readDouble();
                            double num175;
                            try
                            {
                                num175 = msg.reader().readDouble();
                            }
                            catch (Exception)
                            {
                                num175 = 0;
                            }
                            if (mob9.isBusyAttackSomeOne)
                            {
                                Char.myCharz().doInjure(dame, num175, isCrit: false, isMob: true);
                                break;
                            }
                            mob9.dame = dame;
                            mob9.dameMp = num175;
                            mob9.setAttack(Char.myCharz());
                        }
                        break;
                    }
                case -10:
                    {
                        Mob mob9 = null;
                        try
                        {
                            mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        }
                        catch (Exception)
                        {
                        }
                        if (mob9 != null)
                        {
                            @char = GameScr.findCharInMap(msg.reader().readInt());
                            if (@char == null)
                            {
                                return;
                            }
                            double cHP = msg.reader().readDouble();
                            mob9.dame = @char.cHP - cHP;
                            @char.cHPNew = cHP;
                            try
                            {
                                @char.cMP = msg.reader().readDouble();
                            }
                            catch (Exception)
                            {
                            }
                            if (mob9.isBusyAttackSomeOne)
                            {
                                @char.doInjure(mob9.dame, 0, isCrit: false, isMob: true);
                            }
                            else
                            {
                                mob9.setAttack(@char);
                            }
                        }
                        break;
                    }
                case -17:
                    Char.myCharz().meDead = true;
                    Char.myCharz().cPk = msg.reader().readByte();
                    Char.myCharz().startDie(msg.reader().readShort(), msg.reader().readShort());
                    try
                    {
                        Char.myCharz().cPower = msg.reader().readLong();
                        Char.myCharz().applyCharLevelPercent();
                    }
                    catch (Exception)
                    {
                        Cout.println("Loi tai ME_DIE " + msg.command);
                    }
                    Char.myCharz().countKill = 0;
                    break;
                case 66:
                    break;
                case -8:
                    @char = GameScr.findCharInMap(msg.reader().readInt());
                    if (@char == null)
                    {
                        return;
                    }
                    @char.cPk = msg.reader().readByte();
                    @char.waitToDie(msg.reader().readShort(), msg.reader().readShort());
                    break;
                case -16:
                    if (Char.myCharz().wdx != 0 || Char.myCharz().wdy != 0)
                    {
                        Char.myCharz().cx = Char.myCharz().wdx;
                        Char.myCharz().cy = Char.myCharz().wdy;
                        Char.myCharz().wdx = (Char.myCharz().wdy = 0);
                    }
                    Char.myCharz().liveFromDead();
                    Char.myCharz().isLockMove = false;
                    Char.myCharz().meDead = false;
                    break;
                case 44:
                    {
                        int num176 = msg.reader().readInt();
                        string text8 = msg.reader().readUTF();
                        @char = ((Char.myCharz().charID != num176) ? GameScr.findCharInMap(num176) : Char.myCharz());
                        if (@char == null)
                        {
                            return;
                        }
                        @char.addInfo(text8);
                        break;
                    }
                case 18:
                    {
                        sbyte b70 = msg.reader().readByte();
                        for (int num173 = 0; num173 < b70; num173++)
                        {
                            int charId = msg.reader().readInt();
                            int cx = msg.reader().readShort();
                            int cy = msg.reader().readShort();
                            double cHPShow = msg.reader().readDouble();
                            Char char12 = GameScr.findCharInMap(charId);
                            if (char12 != null)
                            {
                                char12.cx = cx;
                                char12.cy = cy;
                                char12.cHP = (char12.cHPShow = cHPShow);
                                char12.lastUpdateTime = mSystem.currentTimeMillis();
                            }
                        }
                        break;
                    }
                case 19:
                    Char.myCharz().countKill = msg.reader().readUnsignedShort();
                    Char.myCharz().countKillMax = msg.reader().readUnsignedShort();
                    break;
            }
        }
        catch (Exception ex40)
        {
        }
        finally
        {
            msg?.cleanup();
        }
    }

    private void readLogin(Message msg)
    {
        sbyte b = msg.reader().readByte();
        ChooseCharScr.playerData = new PlayerData[b];
        Res.outz("[LEN] sl nguoi choi " + b);
        for (int i = 0; i < b; i++)
        {
            int playerID = msg.reader().readInt();
            string name = msg.reader().readUTF();
            short head = msg.reader().readShort();
            short body = msg.reader().readShort();
            short leg = msg.reader().readShort();
            long ppoint = msg.reader().readLong();
            ChooseCharScr.playerData[i] = new PlayerData(playerID, name, head, body, leg, ppoint);
        }
        GameCanvas.chooseCharScr.switchToMe();
        GameCanvas.chooseCharScr.updateChooseCharacter((byte)b);
    }

    private void createItem(myReader d)
    {
        GameScr.vcItem = d.readByte();
        ItemTemplates.itemTemplates.clear();
        GameScr.gI().iOptionTemplates = new ItemOptionTemplate[d.readUnsignedByte()];
        for (int i = 0; i < GameScr.gI().iOptionTemplates.Length; i++)
        {
            GameScr.gI().iOptionTemplates[i] = new ItemOptionTemplate();
            GameScr.gI().iOptionTemplates[i].id = i;
            GameScr.gI().iOptionTemplates[i].name = d.readUTF();
            GameScr.gI().iOptionTemplates[i].type = d.readByte();
        }
        int num = d.readShort();
        for (int j = 0; j < num; j++)
        {
            ItemTemplate it = new ItemTemplate((short)j, d.readByte(), d.readByte(), d.readUTF(), d.readUTF(), d.readByte(), d.readInt(), d.readShort(), d.readShort(), d.readBool());
            ItemTemplates.add(it);
        }
    }

    private void createSkill(myReader d)
    {
        GameScr.vcSkill = d.readByte();
        GameScr.gI().sOptionTemplates = new SkillOptionTemplate[d.readByte()];
        for (int i = 0; i < GameScr.gI().sOptionTemplates.Length; i++)
        {
            GameScr.gI().sOptionTemplates[i] = new SkillOptionTemplate();
            GameScr.gI().sOptionTemplates[i].id = i;
            GameScr.gI().sOptionTemplates[i].name = d.readUTF();
        }
        GameScr.nClasss = new NClass[d.readByte()];
        for (int j = 0; j < GameScr.nClasss.Length; j++)
        {
            GameScr.nClasss[j] = new NClass();
            GameScr.nClasss[j].classId = j;
            GameScr.nClasss[j].name = d.readUTF();
            GameScr.nClasss[j].skillTemplates = new SkillTemplate[d.readByte()];
            for (int k = 0; k < GameScr.nClasss[j].skillTemplates.Length; k++)
            {
                GameScr.nClasss[j].skillTemplates[k] = new SkillTemplate();
                GameScr.nClasss[j].skillTemplates[k].id = d.readByte();
                GameScr.nClasss[j].skillTemplates[k].name = d.readUTF();
                GameScr.nClasss[j].skillTemplates[k].maxPoint = d.readByte();
                GameScr.nClasss[j].skillTemplates[k].manaUseType = d.readByte();
                GameScr.nClasss[j].skillTemplates[k].type = d.readByte();
                GameScr.nClasss[j].skillTemplates[k].iconId = d.readShort();
                GameScr.nClasss[j].skillTemplates[k].damInfo = d.readUTF();
                int lineWidth = 130;
                if (GameCanvas.w == 128 || GameCanvas.h <= 208)
                {
                    lineWidth = 100;
                }
                GameScr.nClasss[j].skillTemplates[k].description = mFont.tahoma_7_green2.splitFontArray(d.readUTF(), lineWidth);
                GameScr.nClasss[j].skillTemplates[k].skills = new Skill[d.readByte()];
                for (int l = 0; l < GameScr.nClasss[j].skillTemplates[k].skills.Length; l++)
                {
                    GameScr.nClasss[j].skillTemplates[k].skills[l] = new Skill();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].skillId = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].template = GameScr.nClasss[j].skillTemplates[k];
                    GameScr.nClasss[j].skillTemplates[k].skills[l].point = d.readByte();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].powRequire = d.readLong();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].manaUse = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].coolDown = d.readInt();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].dx = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].dy = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].maxFight = d.readByte();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].damage = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].price = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].moreInfo = d.readUTF();
                    Skills.add(GameScr.nClasss[j].skillTemplates[k].skills[l]);
                }
            }
        }
    }

    private void createMap(myReader d)
    {
        GameScr.vcMap = d.readByte();
        TileMap.mapNames = new string[d.readUnsignedByte()];
        for (int i = 0; i < TileMap.mapNames.Length; i++)
        {
            TileMap.mapNames[i] = d.readUTF();
        }
        Npc.arrNpcTemplate = new NpcTemplate[d.readByte()];
        for (sbyte b = 0; b < Npc.arrNpcTemplate.Length; b++)
        {
            Npc.arrNpcTemplate[b] = new NpcTemplate();
            Npc.arrNpcTemplate[b].npcTemplateId = b;
            Npc.arrNpcTemplate[b].name = d.readUTF();
            Npc.arrNpcTemplate[b].headId = d.readShort();
            Npc.arrNpcTemplate[b].bodyId = d.readShort();
            Npc.arrNpcTemplate[b].legId = d.readShort();
            Npc.arrNpcTemplate[b].menu = new string[d.readByte()][];
            for (int j = 0; j < Npc.arrNpcTemplate[b].menu.Length; j++)
            {
                Npc.arrNpcTemplate[b].menu[j] = new string[d.readByte()];
                for (int k = 0; k < Npc.arrNpcTemplate[b].menu[j].Length; k++)
                {
                    Npc.arrNpcTemplate[b].menu[j][k] = d.readUTF();
                }
            }
        }
        Mob.arrMobTemplate = new MobTemplate[d.readByte()];
        for (sbyte b2 = 0; b2 < Mob.arrMobTemplate.Length; b2++)
        {
            Mob.arrMobTemplate[b2] = new MobTemplate();
            Mob.arrMobTemplate[b2].mobTemplateId = b2;
            Mob.arrMobTemplate[b2].type = d.readByte();
            Mob.arrMobTemplate[b2].name = d.readUTF();
            Mob.arrMobTemplate[b2].hp = d.readInt();
            Mob.arrMobTemplate[b2].rangeMove = d.readByte();
            Mob.arrMobTemplate[b2].speed = d.readByte();
            Mob.arrMobTemplate[b2].dartType = d.readByte();
        }
    }

    private void createData(myReader d, bool isSaveRMS)
    {
        GameScr.vcData = d.readByte();
        if (isSaveRMS)
        {
            Rms.saveRMS("NR_dart", NinjaUtil.readByteArray(d));
            Rms.saveRMS("NR_arrow", NinjaUtil.readByteArray(d));
            Rms.saveRMS("NR_effect", NinjaUtil.readByteArray(d));
            Rms.saveRMS("NR_image", NinjaUtil.readByteArray(d));
            Rms.saveRMS("NR_part", NinjaUtil.readByteArray(d));
            Rms.saveRMS("NR_skill", NinjaUtil.readByteArray(d));
            Rms.DeleteStorage("NRdata");
        }
    }

    private Image createImage(sbyte[] arr)
    {
        try
        {
            return Image.createImage(arr, 0, arr.Length);
        }
        catch (Exception)
        {
        }
        return null;
    }

    public int[] arrayByte2Int(sbyte[] b)
    {
        int[] array = new int[b.Length];
        for (int i = 0; i < b.Length; i++)
        {
            int num = b[i];
            if (num < 0)
            {
                num += 256;
            }
            array[i] = num;
        }
        return array;
    }

    public void readClanMsg(Message msg, int index)
    {
        try
        {
            ClanMessage clanMessage = new ClanMessage();
            sbyte b = msg.reader().readByte();
            clanMessage.type = b;
            clanMessage.id = msg.reader().readInt();
            clanMessage.playerId = msg.reader().readInt();
            clanMessage.playerName = msg.reader().readUTF();
            clanMessage.role = msg.reader().readByte();
            clanMessage.time = msg.reader().readInt() + 1000000000;
            bool flag = false;
            GameScr.isNewClanMessage = false;
            if (b == 0)
            {
                string text = msg.reader().readUTF();
                GameScr.isNewClanMessage = true;
                if (mFont.tahoma_7.getWidth(text) > Panel.WIDTH_PANEL - 60)
                {
                    clanMessage.chat = mFont.tahoma_7.splitFontArray(text, Panel.WIDTH_PANEL - 10);
                }
                else
                {
                    clanMessage.chat = new string[1];
                    clanMessage.chat[0] = text;
                }
                clanMessage.color = msg.reader().readByte();
            }
            else if (b == 1)
            {
                clanMessage.recieve = msg.reader().readByte();
                clanMessage.maxCap = msg.reader().readByte();
                flag = msg.reader().readByte() == 1;
                if (flag)
                {
                    GameScr.isNewClanMessage = true;
                }
                if (clanMessage.playerId != Char.myCharz().charID)
                {
                    if (clanMessage.recieve < clanMessage.maxCap)
                    {
                        clanMessage.option = new string[1] { mResources.donate };
                    }
                    else
                    {
                        clanMessage.option = null;
                    }
                }
                if (GameCanvas.panel.cp != null)
                {
                    GameCanvas.panel.updateRequest(clanMessage.recieve, clanMessage.maxCap);
                }
            }
            else if (b == 2 && Char.myCharz().role == 0)
            {
                GameScr.isNewClanMessage = true;
                clanMessage.option = new string[2]
                {
                    mResources.CANCEL,
                    mResources.receive
                };
            }
            if (GameCanvas.currentScreen != GameScr.instance)
            {
                GameScr.isNewClanMessage = false;
            }
            else if (GameCanvas.panel.isShow && GameCanvas.panel.type == 0 && GameCanvas.panel.currentTabIndex == 3)
            {
                GameScr.isNewClanMessage = false;
            }
            ClanMessage.addMessage(clanMessage, index, flag);
        }
        catch (Exception)
        {
        }
    }

    public void loadCurrMap(sbyte teleport3)
    {
        GameScr.gI().auto = 0;
        GameScr.isChangeZone = false;
        CreateCharScr.instance = null;
        GameScr.info1.isUpdate = false;
        GameScr.info2.isUpdate = false;
        GameScr.lockTick = 0;
        GameCanvas.panel.isShow = false;
        SoundMn.gI().stopAll();
        if (!GameScr.isLoadAllData && !CreateCharScr.isCreateChar)
        {
            GameScr.gI().initSelectChar();
        }
        GameScr.loadCamera(fullmScreen: false, (teleport3 != 1) ? (-1) : Char.myCharz().cx, (teleport3 == 0) ? (-1) : 0);
        TileMap.loadMainTile();
        TileMap.loadMap(TileMap.tileID);
        Char.myCharz().cvx = 0;
        Char.myCharz().statusMe = 4;
        Char.myCharz().currentMovePoint = null;
        Char.myCharz().mobFocus = null;
        Char.myCharz().charFocus = null;
        Char.myCharz().npcFocus = null;
        Char.myCharz().itemFocus = null;
        Char.myCharz().skillPaint = null;
        Char.myCharz().setMabuHold(m: false);
        Char.myCharz().skillPaintRandomPaint = null;
        GameCanvas.clearAllPointerEvent();
        if (Char.myCharz().cy >= TileMap.pxh - 100)
        {
            Char.myCharz().isFlyUp = true;
            Char.myCharz().cx += Res.abs(Res.random(0, 80));
            Service.gI().charMove();
        }
        GameScr.gI().loadGameScr();
        GameCanvas.loadBG(TileMap.bgID);
        Char.isLockKey = false;
        for (int i = 0; i < Char.myCharz().vEff.size(); i++)
        {
            EffectChar effectChar = (EffectChar)Char.myCharz().vEff.elementAt(i);
            if (effectChar.template.type == 10)
            {
                Char.isLockKey = true;
                break;
            }
        }
        GameCanvas.clearKeyHold();
        GameCanvas.clearKeyPressed();
        GameScr.gI().dHP = Char.myCharz().cHP;
        GameScr.gI().dMP = Char.myCharz().cMP;
        Char.ischangingMap = false;
        GameScr.gI().switchToMe();
        if (Char.myCharz().cy <= 10 && teleport3 != 0 && teleport3 != 2)
        {
            Teleport p = new Teleport(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().head, Char.myCharz().cdir, 1, isMe: true, (teleport3 != 1) ? teleport3 : Char.myCharz().cgender);
            Teleport.addTeleport(p);
            Char.myCharz().isTeleport = true;
        }
        if (teleport3 == 2)
        {
            Char.myCharz().show();
        }
        if (GameScr.gI().isRongThanXuatHien)
        {
            if (TileMap.mapID == GameScr.gI().mapRID && TileMap.zoneID == GameScr.gI().zoneRID)
            {
                GameScr.gI().callRongThan(GameScr.gI().xR, GameScr.gI().yR);
            }
            if (mGraphics.zoomLevel > 1)
            {
                GameScr.gI().doiMauTroi();
            }
        }
        InfoDlg.hide();
        InfoDlg.show(TileMap.mapName, mResources.zone + " " + TileMap.zoneID, 30);
        GameCanvas.endDlg();
        GameCanvas.isLoading = false;
        Hint.clickMob();
        Hint.clickNpc();
    }

    public void loadInfoMap(Message msg)
    {
        try
        {
            if (mGraphics.zoomLevel == 1)
            {
                SmallImage.clearHastable();
            }
            Char.myCharz().cx = (Char.myCharz().cxSend = (Char.myCharz().cxFocus = msg.reader().readShort()));
            Char.myCharz().cy = (Char.myCharz().cySend = (Char.myCharz().cyFocus = msg.reader().readShort()));
            Char.myCharz().xSd = Char.myCharz().cx;
            Char.myCharz().ySd = Char.myCharz().cy;
            if (Char.myCharz().cx >= 0 && Char.myCharz().cx <= 100)
            {
                Char.myCharz().cdir = 1;
            }
            else if (Char.myCharz().cx >= TileMap.tmw - 100 && Char.myCharz().cx <= TileMap.tmw)
            {
                Char.myCharz().cdir = -1;
            }
            int num = msg.reader().readByte();
            if (!GameScr.info1.isDone)
            {
                GameScr.info1.cmx = Char.myCharz().cx - GameScr.cmx;
                GameScr.info1.cmy = Char.myCharz().cy - GameScr.cmy;
            }
            for (int i = 0; i < num; i++)
            {
                Waypoint waypoint = new(msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readUTF());
                if ((TileMap.mapID != 21 && TileMap.mapID != 22 && TileMap.mapID != 23) || waypoint.minX < 0 || waypoint.minX <= 24)
                {
                }
            }
            Resources.UnloadUnusedAssets();
            GC.Collect();
            num = msg.reader().readByte();
            Mob.newMob.removeAllElements();
            for (sbyte b = 0; b < num; b++)
            {
                Mob mob = new Mob(b,
                    msg.reader().readBoolean(),
                    msg.reader().readBoolean(),
                    msg.reader().readBoolean(),
                    msg.reader().readBoolean(),
                    msg.reader().readBoolean(),
                    msg.reader().readByte(),
                    msg.reader().readByte(),
                    msg.reader().readDouble(), // HP
                    msg.reader().readByte(),
                    msg.reader().readDouble(), // MAX HP
                    msg.reader().readShort(),
                    msg.reader().readShort(),
                    msg.reader().readByte(),
                    msg.reader().readByte());
                mob.xSd = mob.x;
                mob.ySd = mob.y;
                mob.isBoss = msg.reader().readBoolean();
                if (Mob.arrMobTemplate[mob.templateId].type != 0)
                {
                    if (b % 3 == 0)
                    {
                        mob.dir = -1;
                    }
                    else
                    {
                        mob.dir = 1;
                    }
                    mob.x += 10 - b % 20;
                }
                mob.isMobMe = false;
                BigBoss bigBoss = null;
                BachTuoc bachTuoc = null;
                BigBoss2 bigBoss2 = null;
                NewBoss newBoss = null;
                if (mob.templateId == 70)
                {
                    bigBoss = new BigBoss(b, (short)mob.x, (short)mob.y, 70, mob.hp, mob.maxHp, mob.sys);
                }
                if (mob.templateId == 71)
                {
                    bachTuoc = new BachTuoc(b, (short)mob.x, (short)mob.y, 71, mob.hp, mob.maxHp, mob.sys);
                }
                if (mob.templateId == 72)
                {
                    bigBoss2 = new BigBoss2(b, (short)mob.x, (short)mob.y, 72, mob.hp, mob.maxHp, 3);
                }
                if (mob.isBoss)
                {
                    newBoss = new NewBoss(b, (short)mob.x, (short)mob.y, mob.templateId, mob.hp, mob.maxHp, mob.sys);
                }
                if (newBoss != null)
                {
                    GameScr.vMob.addElement(newBoss);
                }
                else if (bigBoss != null)
                {
                    GameScr.vMob.addElement(bigBoss);
                }
                else if (bachTuoc != null)
                {
                    GameScr.vMob.addElement(bachTuoc);
                }
                else if (bigBoss2 != null)
                {
                    GameScr.vMob.addElement(bigBoss2);
                }
                else
                {
                    GameScr.vMob.addElement(mob);
                }
            }
            if (Char.myCharz().mobMe != null && GameScr.findMobInMap(Char.myCharz().mobMe.mobId) == null)
            {
                Char.myCharz().mobMe.getData();
                Char.myCharz().mobMe.x = Char.myCharz().cx;
                Char.myCharz().mobMe.y = Char.myCharz().cy - 40;
                GameScr.vMob.addElement(Char.myCharz().mobMe);
            }
            num = msg.reader().readByte();
            for (byte b2 = 0; b2 < num; b2++)
            {
            }
            num = msg.reader().readByte();
            for (int j = 0; j < num; j++)
            {
                sbyte b3 = msg.reader().readByte();
                short cx = msg.reader().readShort();
                short num2 = msg.reader().readShort();
                sbyte b4 = msg.reader().readByte();
                short num3 = msg.reader().readShort();
                if (b4 != 6 && ((Char.myCharz().taskMaint.taskId >= 7 && (Char.myCharz().taskMaint.taskId != 7 || Char.myCharz().taskMaint.index > 1)) || (b4 != 7 && b4 != 8 && b4 != 9)) && (Char.myCharz().taskMaint.taskId >= 6 || b4 != 16))
                {
                    if (b4 == 4)
                    {
                        GameScr.gI().magicTree = new MagicTree(j, b3, cx, num2, b4, num3);
                        Service.gI().magicTree(2);
                        GameScr.vNpc.addElement(GameScr.gI().magicTree);
                    }
                    else
                    {
                        Npc o = new Npc(j, b3, cx, num2 + 3, b4, num3);
                        GameScr.vNpc.addElement(o);
                    }
                }
            }
            num = msg.reader().readByte();
            string empty = string.Empty;
            empty = empty + "item: " + num;
            for (int k = 0; k < num; k++)
            {
                short itemMapID = msg.reader().readShort();
                short num4 = msg.reader().readShort();
                int x = msg.reader().readShort();
                int y = msg.reader().readShort();
                int num5 = msg.reader().readInt();
                short r = 0;
                if (num5 == -2)
                {
                    r = msg.reader().readShort();
                }
                ItemMap itemMap = new ItemMap(num5, itemMapID, num4, x, y, r);
                bool flag = false;
                for (int l = 0; l < GameScr.vItemMap.size(); l++)
                {
                    ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(l);
                    if (itemMap2.itemMapID == itemMap.itemMapID)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    GameScr.vItemMap.addElement(itemMap);
                }
                empty = empty + num4 + ",";
            }
            TileMap.vCurrItem.removeAllElements();
            if (mGraphics.zoomLevel == 1)
            {
                BgItem.clearHashTable();
            }
            BgItem.vKeysNew.removeAllElements();
            if (!GameCanvas.lowGraphic || (GameCanvas.lowGraphic && TileMap.isVoDaiMap()) || TileMap.mapID == 45 || TileMap.mapID == 46 || TileMap.mapID == 47 || TileMap.mapID == 48)
            {
                short num6 = msg.reader().readShort();
                empty = "item high graphic: ";
                for (int m = 0; m < num6; m++)
                {
                    short num7 = msg.reader().readShort();
                    short num8 = msg.reader().readShort();
                    short num9 = msg.reader().readShort();
                    if (TileMap.getBIById(num7) != null)
                    {
                        BgItem bIById = TileMap.getBIById(num7);
                        BgItem bgItem = new BgItem();
                        bgItem.id = num7;
                        bgItem.idImage = bIById.idImage;
                        bgItem.dx = bIById.dx;
                        bgItem.dy = bIById.dy;
                        bgItem.x = num8 * TileMap.size;
                        bgItem.y = num9 * TileMap.size;
                        bgItem.layer = bIById.layer;
                        if (TileMap.isExistMoreOne(bgItem.id))
                        {
                            bgItem.trans = ((m % 2 != 0) ? 2 : 0);
                            if (TileMap.mapID == 45)
                            {
                                bgItem.trans = 0;
                            }
                        }
                        Image image = null;
                        if (!BgItem.imgNew.containsKey(bgItem.idImage + string.Empty))
                        {
                            if (mGraphics.zoomLevel == 1)
                            {
                                image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
                                if (image == null)
                                {
                                    image = Image.createRGBImage(new int[1], 1, 1, bl: true);
                                    Service.gI().getBgTemplate(bgItem.idImage);
                                }
                                BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
                            }
                            else
                            {
                                bool flag2 = false;
                                sbyte[] array = Rms.loadRMS(mGraphics.zoomLevel + "bgItem" + bgItem.idImage);
                                if (array != null)
                                {
                                    if (BgItem.newSmallVersion != null)
                                    {
                                        if (array.Length % 127 != BgItem.newSmallVersion[bgItem.idImage])
                                        {
                                            flag2 = true;
                                        }
                                    }
                                    if (!flag2)
                                    {
                                        image = Image.createImage(array, 0, array.Length);
                                        if (image != null)
                                        {
                                            BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
                                        }
                                        else
                                        {
                                            flag2 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    flag2 = true;
                                }
                                if (flag2)
                                {
                                    image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
                                    if (image == null)
                                    {
                                        image = Image.createRGBImage(new int[1], 1, 1, bl: true);
                                        Service.gI().getBgTemplate(bgItem.idImage);
                                    }
                                    BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
                                }
                            }
                            BgItem.vKeysLast.addElement(bgItem.idImage + string.Empty);
                        }
                        if (!BgItem.isExistKeyNews(bgItem.idImage + string.Empty))
                        {
                            BgItem.vKeysNew.addElement(bgItem.idImage + string.Empty);
                        }
                        bgItem.changeColor();
                        TileMap.vCurrItem.addElement(bgItem);
                    }
                    empty = empty + num7 + ",";
                }
                for (int n = 0; n < BgItem.vKeysLast.size(); n++)
                {
                    string text = (string)BgItem.vKeysLast.elementAt(n);
                    if (!BgItem.isExistKeyNews(text))
                    {
                        BgItem.imgNew.remove(text);
                        if (BgItem.imgNew.containsKey(text + "blend" + 1))
                        {
                            BgItem.imgNew.remove(text + "blend" + 1);
                        }
                        if (BgItem.imgNew.containsKey(text + "blend" + 3))
                        {
                            BgItem.imgNew.remove(text + "blend" + 3);
                        }
                        BgItem.vKeysLast.removeElementAt(n);
                        n--;
                    }
                }
                BackgroudEffect.isFog = false;
                BackgroudEffect.nCloud = 0;
                EffecMn.vEff.removeAllElements();
                BackgroudEffect.vBgEffect.removeAllElements();
                Effect.newEff.removeAllElements();
                short num10 = msg.reader().readShort();
                for (int num11 = 0; num11 < num10; num11++)
                {
                    string key = msg.reader().readUTF();
                    string value = msg.reader().readUTF();
                    keyValueAction(key, value);
                }
            }
            else
            {
                short num12 = msg.reader().readShort();
                for (int num13 = 0; num13 < num12; num13++)
                {
                    short num14 = msg.reader().readShort();
                    short num15 = msg.reader().readShort();
                    short num16 = msg.reader().readShort();
                }
                short num17 = msg.reader().readShort();
                for (int num18 = 0; num18 < num17; num18++)
                {
                    string text2 = msg.reader().readUTF();
                    string text3 = msg.reader().readUTF();
                }
            }
            TileMap.bgType = msg.reader().readByte();
            sbyte teleport = msg.reader().readByte();
            loadCurrMap(teleport);
            Char.isLoadingMap = false;
            Resources.UnloadUnusedAssets();
            GC.Collect();
            ModFunc.GI().canUpdate = true;
        }
        catch (Exception)
        {
            AutoXmap.FixBlackScreen();
        }
    }

    public void LoadAuraNpcs(Message msg)
    {
        sbyte sz = msg.reader().readByte();
        for (sbyte i = 0; i < sz; i++)
        {
            sbyte npcId = msg.reader().readByte();
            short auraId = msg.reader().readShort();
            Npc npc = ModFunc.GetNpcByTempId(npcId);
            if (npc != null)
            {
                npc.idAura = auraId;
            }
        }
    }

    public void keyValueAction(string key, string value)
    {
        if (key.Equals("eff"))
        {
            if (Panel.graphics > 0)
            {
                return;
            }
            string[] array = Res.split(value, ".", 0);
            int id = int.Parse(array[0]);
            int layer = int.Parse(array[1]);
            int x = int.Parse(array[2]);
            int y = int.Parse(array[3]);
            int loop;
            int loopCount;
            if (array.Length <= 4)
            {
                loop = -1;
                loopCount = 1;
            }
            else
            {
                loop = int.Parse(array[4]);
                loopCount = int.Parse(array[5]);
            }
            Effect effect = new Effect(id, x, y, layer, loop, loopCount);
            if (array.Length > 6)
            {
                effect.typeEff = int.Parse(array[6]);
                if (array.Length > 7)
                {
                    effect.indexFrom = int.Parse(array[7]);
                    effect.indexTo = int.Parse(array[8]);
                }
            }
            EffecMn.addEff(effect);
        }
        else if (key.Equals("beff") && Panel.graphics <= 1)
        {
            BackgroudEffect.addEffect(int.Parse(value));
        }
    }

    public void messageNotMap(Message msg)
    {
        try
        {
            sbyte b = msg.reader().readByte();
            switch (b)
            {
                case 16:
                    MoneyCharge.gI().switchToMe();
                    break;
                case 17:
                    Char.myCharz().clearTask();
                    break;
                case 18:
                    {
                        GameCanvas.isLoading = false;
                        GameCanvas.endDlg();
                        int num2 = msg.reader().readInt();
                        GameCanvas.inputDlg.show(mResources.changeNameChar, new Command(mResources.OK, GameCanvas.instance, 88829, num2), TField.INPUT_TYPE_ANY);
                        break;
                    }
                case 20:
                    Char.myCharz().cPk = msg.reader().readByte();
                    GameScr.info1.addInfo(mResources.PK_NOW + " " + Char.myCharz().cPk, 0);
                    break;
                case 35:
                    GameCanvas.endDlg();
                    GameScr.gI().resetButton();
                    GameScr.info1.addInfo(msg.reader().readUTF(), 0);
                    break;
                case 36:
                    GameScr.typeActive = msg.reader().readByte();
                    break;
                case 4:
                    {
                        GameCanvas.loginScr.savePass();
                        GameScr.isAutoPlay = false;
                        GameScr.canAutoPlay = false;
                        LoginScr.isUpdateAll = true;
                        LoginScr.isUpdateData = true;
                        LoginScr.isUpdateMap = true;
                        LoginScr.isUpdateSkill = true;
                        LoginScr.isUpdateItem = true;
                        GameScr.vsData = msg.reader().readByte();
                        GameScr.vsMap = msg.reader().readByte();
                        GameScr.vsSkill = msg.reader().readByte();
                        GameScr.vsItem = msg.reader().readByte();
                        sbyte b3 = msg.reader().readByte();
                        if (GameCanvas.loginScr.isLogin2)
                        {
                            Rms.saveRMSString("acc", string.Empty);
                            Rms.saveRMSString("pass", string.Empty);
                        }
                        else
                        {
                            Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, string.Empty);
                        }
                        if (GameScr.vsData != GameScr.vcData)
                        {
                            GameScr.isLoadAllData = false;
                            Service.gI().updateData();
                        }
                        else
                        {
                            try
                            {
                                LoginScr.isUpdateData = false;
                            }
                            catch (Exception)
                            {
                                GameScr.vcData = -1;
                                Service.gI().updateData();
                            }
                        }
                        if (GameScr.vsMap != GameScr.vcMap)
                        {
                            GameScr.isLoadAllData = false;
                            Service.gI().updateMap();
                        }
                        else
                        {
                            try
                            {
                                if (!GameScr.isLoadAllData)
                                {
                                    DataInputStream dataInputStream = new DataInputStream(Rms.loadRMS("NRmap"));
                                    createMap(dataInputStream.r);
                                }
                                LoginScr.isUpdateMap = false;
                            }
                            catch (Exception)
                            {
                                GameScr.vcMap = -1;
                                Service.gI().updateMap();
                            }
                        }
                        if (GameScr.vsSkill != GameScr.vcSkill)
                        {
                            GameScr.isLoadAllData = false;
                            Service.gI().updateSkill();
                        }
                        else
                        {
                            try
                            {
                                if (!GameScr.isLoadAllData)
                                {
                                    DataInputStream dataInputStream2 = new(Rms.loadRMS("NRskill"));
                                    createSkill(dataInputStream2.r);
                                }
                                LoginScr.isUpdateSkill = false;
                            }
                            catch (Exception)
                            {
                                GameScr.vcSkill = -1;
                                Service.gI().updateSkill();
                            }
                        }
                        if (GameScr.vsItem != GameScr.vcItem)
                        {
                            GameScr.isLoadAllData = false;
                            Service.gI().updateItem();
                        }
                        else
                        {
                            try
                            {
                                DataInputStream dataInputStream3 = new(Rms.loadRMS("NRitem0"));
                                loadItemNew(dataInputStream3.r, 0, isSave: false);
                                DataInputStream dataInputStream4 = new(Rms.loadRMS("NRitem1"));
                                loadItemNew(dataInputStream4.r, 1, isSave: false);
                                DataInputStream dataInputStream5 = new(Rms.loadRMS("NRitem2"));
                                loadItemNew(dataInputStream5.r, 2, isSave: false);
                                DataInputStream dataInputStream6 = new(Rms.loadRMS("NRitem100"));
                                loadItemNew(dataInputStream6.r, 100, isSave: false);
                                LoginScr.isUpdateItem = false;
                            }
                            catch (Exception)
                            {
                                GameScr.vcItem = -1;
                                Service.gI().updateItem();
                            }
                        }
                        if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
                        {
                            if (!GameScr.isLoadAllData)
                            {
                                GameScr.gI().readDart();
                                GameScr.gI().readEfect();
                                GameScr.gI().readArrow();
                                GameScr.gI().readSkill();
                            }
                            Service.gI().clientOk();
                        }
                        sbyte b4 = msg.reader().readByte();
                        GameScr.exps = new long[b4];
                        for (int j = 0; j < GameScr.exps.Length; j++)
                        {
                            GameScr.exps[j] = msg.reader().readLong();
                        }
                        break;
                    }
                case 6:
                    {
                        msg.reader().mark(100000);
                        createMap(msg.reader());
                        msg.reader().reset();
                        sbyte[] data3 = new sbyte[msg.reader().available()];
                        msg.reader().readFully(ref data3);
                        Rms.saveRMS("NRmap", data3);
                        sbyte[] data4 = new sbyte[1] { GameScr.vcMap };
                        Rms.saveRMS("NRmapVersion", data4);
                        LoginScr.isUpdateMap = false;
                        if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
                        {
                            GameScr.gI().readDart();
                            GameScr.gI().readEfect();
                            GameScr.gI().readArrow();
                            GameScr.gI().readSkill();
                            Service.gI().clientOk();
                        }
                        break;
                    }
                case 7:
                    {
                        msg.reader().mark(100000);
                        createSkill(msg.reader());
                        msg.reader().reset();
                        sbyte[] data = new sbyte[msg.reader().available()];
                        msg.reader().readFully(ref data);
                        Rms.saveRMS("NRskill", data);
                        sbyte[] data2 = new sbyte[1] { GameScr.vcSkill };
                        Rms.saveRMS("NRskillVersion", data2);
                        LoginScr.isUpdateSkill = false;
                        if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
                        {
                            GameScr.gI().readDart();
                            GameScr.gI().readEfect();
                            GameScr.gI().readArrow();
                            GameScr.gI().readSkill();
                            Service.gI().clientOk();
                        }
                        break;
                    }
                case 8:
                    Res.outz("GET UPDATE_ITEM " + msg.reader().available() + " bytes");
                    createItemNew(msg.reader());
                    break;
                case 10:
                    try
                    {
                        Char.isLoadingMap = true;
                        Res.outz("REQUEST MAP TEMPLATE");
                        GameCanvas.isLoading = true;
                        TileMap.maps = null;
                        TileMap.types = null;
                        mSystem.gcc();
                        GameCanvas.debug("SA99", 2);
                        TileMap.tmw = msg.reader().readByte();
                        TileMap.tmh = msg.reader().readByte();
                        TileMap.maps = new int[TileMap.tmw * TileMap.tmh];
                        Res.err("   M apsize= " + TileMap.tmw * TileMap.tmh);
                        for (int i = 0; i < TileMap.maps.Length; i++)
                        {
                            int num = msg.reader().readByte();
                            if (num < 0)
                            {
                                num += 256;
                            }
                            TileMap.maps[i] = (ushort)num;
                        }
                        TileMap.types = new int[TileMap.maps.Length];
                        msg = messWait;
                        loadInfoMap(msg);
                        try
                        {
                            sbyte b2 = msg.reader().readByte();
                            TileMap.isMapDouble = ((b2 != 0) ? true : false);
                        }
                        catch (Exception ex)
                        {
                            Res.err(" 1 LOI TAI CASE REQUEST_MAPTEMPLATE " + ex.ToString());
                        }
                    }
                    catch (Exception ex2)
                    {
                        Res.err("2 LOI TAI CASE REQUEST_MAPTEMPLATE " + ex2.ToString());
                    }
                    msg.cleanup();
                    messWait.cleanup();
                    msg = (messWait = null);
                    GameScr.gI().switchToMe();
                    break;
                case 12:
                    GameCanvas.debug("SA10", 2);
                    break;
                case 9:
                    GameCanvas.debug("SA11", 2);
                    break;
            }
        }
        catch (Exception)
        {
            Cout.LogError("LOI TAI messageNotMap + " + msg.command);
        }
        finally
        {
            msg?.cleanup();
        }
    }

    public void messageNotLogin(Message msg)
    {
        try
        {
            sbyte b = msg.reader().readByte();
            if (b != 2)
            {
                return;
            }
            string linkDefault = msg.reader().readUTF();
            if (Rms.loadRMSInt("AdminLink") == 1)
            {
                return;
            }
            if (mSystem.clientType == 1)
            {
                ServerListScreen.linkDefault = linkDefault;
            }
            else
            {
                ServerListScreen.linkDefault = linkDefault;
            }
            mSystem.AddIpTest();
            ServerListScreen.GetServerList(ServerListScreen.linkDefault);
            try
            {
                sbyte b2 = msg.reader().readByte();
                Panel.CanNapTien = b2 == 1;
                sbyte b3 = msg.reader().readByte();
                Rms.saveRMSInt("AdminLink", b3);
            }
            catch (Exception)
            {
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            msg?.cleanup();
        }
    }

    public void messageSubCommand(Message msg)
    {
        try
        {
            sbyte b = msg.reader().readByte();
            // Debug.Log("byte: " + b);
            switch (b)
            {
                case 63:
                    {
                        sbyte b5 = msg.reader().readByte();
                        if (b5 > 0)
                        {
                            GameCanvas.panel.vPlayerMenu_id.removeAllElements();
                            InfoDlg.showWait();
                            MyVector vPlayerMenu = GameCanvas.panel.vPlayerMenu;
                            for (int j = 0; j < b5; j++)
                            {
                                string caption = msg.reader().readUTF();
                                string caption2 = msg.reader().readUTF();
                                short num5 = msg.reader().readShort();
                                GameCanvas.panel.vPlayerMenu_id.addElement(num5 + string.Empty);
                                Char.myCharz().charFocus.menuSelect = num5;
                                Command command = new Command(caption, 11115, Char.myCharz().charFocus);
                                command.caption2 = caption2;
                                vPlayerMenu.addElement(command);
                            }
                            InfoDlg.hide();
                            GameCanvas.panel.setTabPlayerMenu();
                        }
                        break;
                    }
                case 1:
                    GameCanvas.debug("SA13", 2);
                    Char.myCharz().nClass = GameScr.nClasss[msg.reader().readByte()];
                    Char.myCharz().cTiemNang = msg.reader().readLong();
                    Char.myCharz().vSkill.removeAllElements();
                    Char.myCharz().vSkillFight.removeAllElements();
                    Char.myCharz().myskill = null;
                    break;
                case 2:
                    {
                        GameCanvas.debug("SA14", 2);
                        if (Char.myCharz().statusMe != 14 && Char.myCharz().statusMe != 5)
                        {
                            Char.myCharz().cHP = Char.myCharz().cHPFull;
                            Char.myCharz().cMP = Char.myCharz().cMPFull;
                            Cout.LogError2(" ME_LOAD_SKILL");
                        }
                        Char.myCharz().vSkill.removeAllElements();
                        Char.myCharz().vSkillFight.removeAllElements();
                        sbyte b2 = msg.reader().readByte();
                        for (sbyte b3 = 0; b3 < b2; b3++)
                        {
                            short skillId = msg.reader().readShort();
                            Skill skill2 = Skills.get(skillId);
                            useSkill(skill2);
                        }
                        GameScr.gI().sortSkill();
                        if (GameScr.isPaintInfoMe)
                        {
                            GameScr.indexRow = -1;
                            GameScr.gI().left = (GameScr.gI().center = null);
                        }
                        break;
                    }
                case 19:
                    GameCanvas.debug("SA17", 2);
                    Char.myCharz().boxSort();
                    break;
                case 21:
                    {
                        int num3 = msg.reader().readInt();
                        Char.myCharz().xuInBox -= num3;
                        Char.myCharz().xu += num3;
                        Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
                        break;
                    }
                case 0:
                    {
                        RadarScr.list = new MyVector();
                        Teleport.vTeleport.removeAllElements();
                        GameScr.vCharInMap.removeAllElements();
                        GameScr.vItemMap.removeAllElements();
                        Char.vItemTime.removeAllElements();
                        GameScr.loadImg();
                        GameScr.currentCharViewInfo = Char.myCharz();
                        Char.myCharz().charID = msg.reader().readInt();
                        Char.myCharz().ctaskId = msg.reader().readByte();
                        Char.myCharz().cgender = msg.reader().readByte();
                        Char.myCharz().head = msg.reader().readShort();
                        Char.myCharz().cName = msg.reader().readUTF();
                        Char.myCharz().cPk = msg.reader().readByte();
                        Char.myCharz().cTypePk = msg.reader().readByte();
                        Char.myCharz().cPower = msg.reader().readLong();
                        Char.myCharz().applyCharLevelPercent();
                        Char.myCharz().eff5BuffHp = msg.reader().readShort();
                        Char.myCharz().eff5BuffMp = msg.reader().readShort();
                        Char.myCharz().nClass = GameScr.nClasss[msg.reader().readByte()];
                        Char.myCharz().vSkill.removeAllElements();
                        Char.myCharz().vSkillFight.removeAllElements();
                        GameScr.gI().dHP = Char.myCharz().cHP;
                        GameScr.gI().dMP = Char.myCharz().cMP;
                        sbyte b2 = msg.reader().readByte();
                        for (sbyte b6 = 0; b6 < b2; b6++)
                        {
                            Skill skill3 = Skills.get(msg.reader().readShort());
                            useSkill(skill3);
                        }
                        GameScr.gI().sortSkill();
                        GameScr.gI().loadSkillShortcut();
                        Char.myCharz().xu = msg.reader().readLong();
                        Char.myCharz().luongKhoa = msg.reader().readInt();
                        Char.myCharz().luong = msg.reader().readInt();
                        Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
                        Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
                        Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
                        Char.myCharz().arrItemBody = new Item[msg.reader().readByte()];
                        try
                        {
                            Char.myCharz().setDefaultPart();
                            for (int k = 0; k < Char.myCharz().arrItemBody.Length; k++)
                            {
                                short num6 = msg.reader().readShort();
                                if (num6 == -1)
                                {
                                    continue;
                                }
                                ItemTemplate itemTemplate = ItemTemplates.get(num6);
                                int num7 = itemTemplate.type;
                                Char.myCharz().arrItemBody[k] = new Item();
                                Char.myCharz().arrItemBody[k].template = itemTemplate;
                                Char.myCharz().arrItemBody[k].quantity = msg.reader().readInt();
                                Char.myCharz().arrItemBody[k].info = msg.reader().readUTF();
                                Char.myCharz().arrItemBody[k].content = msg.reader().readUTF();
                                int num8 = msg.reader().readUnsignedByte();
                                if (num8 != 0)
                                {
                                    Char.myCharz().arrItemBody[k].itemOption = new ItemOption[num8];
                                    for (int l = 0; l < Char.myCharz().arrItemBody[k].itemOption.Length; l++)
                                    {
                                        int num9 = msg.reader().readUnsignedByte();
                                        int param = msg.reader().readUnsignedShort();
                                        if (num9 != -1)
                                        {
                                            Char.myCharz().arrItemBody[k].itemOption[l] = new ItemOption(num9, param);
                                        }
                                    }
                                }
                                switch (num7)
                                {
                                    case 0:
                                        Char.myCharz().body = Char.myCharz().arrItemBody[k].template.part;
                                        break;
                                    case 1:
                                        Char.myCharz().leg = Char.myCharz().arrItemBody[k].template.part;
                                        break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        Char.myCharz().arrItemBag = new Item[msg.reader().readByte()];
                        GameScr.hpPotion = 0;
                        for (int m = 0; m < Char.myCharz().arrItemBag.Length; m++)
                        {
                            short num10 = msg.reader().readShort();
                            if (num10 == -1)
                            {
                                continue;
                            }
                            Char.myCharz().arrItemBag[m] = new Item();
                            Char.myCharz().arrItemBag[m].template = ItemTemplates.get(num10);
                            Char.myCharz().arrItemBag[m].quantity = msg.reader().readInt();
                            Char.myCharz().arrItemBag[m].info = msg.reader().readUTF();
                            Char.myCharz().arrItemBag[m].content = msg.reader().readUTF();
                            Char.myCharz().arrItemBag[m].indexUI = m;
                            sbyte b7 = msg.reader().readByte();
                            if (b7 != 0)
                            {
                                Char.myCharz().arrItemBag[m].itemOption = new ItemOption[b7];
                                for (int n = 0; n < Char.myCharz().arrItemBag[m].itemOption.Length; n++)
                                {
                                    int num11 = msg.reader().readUnsignedByte();
                                    int param2 = msg.reader().readUnsignedShort();
                                    if (num11 != -1)
                                    {
                                        Char.myCharz().arrItemBag[m].itemOption[n] = new ItemOption(num11, param2);
                                        Char.myCharz().arrItemBag[m].getCompare();
                                    }
                                }
                            }
                            if (Char.myCharz().arrItemBag[m].template.type == 6)
                            {
                                GameScr.hpPotion += Char.myCharz().arrItemBag[m].quantity;
                            }
                        }
                        Char.myCharz().arrItemBox = new Item[msg.reader().readByte()];
                        GameCanvas.panel.hasUse = 0;
                        for (int num12 = 0; num12 < Char.myCharz().arrItemBox.Length; num12++)
                        {
                            short num13 = msg.reader().readShort();
                            if (num13 == -1)
                            {
                                continue;
                            }
                            Char.myCharz().arrItemBox[num12] = new Item();
                            Char.myCharz().arrItemBox[num12].template = ItemTemplates.get(num13);
                            Char.myCharz().arrItemBox[num12].quantity = msg.reader().readInt();
                            Char.myCharz().arrItemBox[num12].info = msg.reader().readUTF();
                            Char.myCharz().arrItemBox[num12].content = msg.reader().readUTF();
                            Char.myCharz().arrItemBox[num12].itemOption = new ItemOption[msg.reader().readByte()];
                            for (int num14 = 0; num14 < Char.myCharz().arrItemBox[num12].itemOption.Length; num14++)
                            {
                                int num15 = msg.reader().readUnsignedByte();
                                int param3 = msg.reader().readUnsignedShort();
                                if (num15 != -1)
                                {
                                    Char.myCharz().arrItemBox[num12].itemOption[num14] = new ItemOption(num15, param3);
                                    Char.myCharz().arrItemBox[num12].getCompare();
                                }
                            }
                            GameCanvas.panel.hasUse++;
                        }
                        Char.myCharz().statusMe = 4;
                        int num16 = Rms.loadRMSInt(Char.myCharz().cName + "vci");
                        if (num16 < 1)
                        {
                            GameScr.isViewClanInvite = false;
                        }
                        else
                        {
                            GameScr.isViewClanInvite = true;
                        }
                        short num17 = msg.reader().readShort();
                        Char.idHead = new short[num17];
                        Char.idAvatar = new short[num17];
                        for (int num18 = 0; num18 < num17; num18++)
                        {
                            Char.idHead[num18] = msg.reader().readShort();
                            Char.idAvatar[num18] = msg.reader().readShort();
                        }
                        for (int num19 = 0; num19 < GameScr.info1.charId.Length; num19++)
                        {
                            GameScr.info1.charId[num19] = new int[3];
                        }
                        GameScr.info1.charId[Char.myCharz().cgender][0] = msg.reader().readShort();
                        GameScr.info1.charId[Char.myCharz().cgender][1] = msg.reader().readShort();
                        GameScr.info1.charId[Char.myCharz().cgender][2] = msg.reader().readShort();
                        Char.myCharz().isNhapThe = msg.reader().readByte() == 1;
                        GameScr.deltaTime = mSystem.currentTimeMillis() - (long)msg.reader().readInt() * 1000L;
                        GameScr.isNewMember = msg.reader().readByte();
                        Char.myCharz().isTichXanh = GameScr.isNewMember == 1;
                        Service.gI().updateCaption((sbyte)Char.myCharz().cgender);
                        Service.gI().androidPack();
                        try
                        {
                            Char.myCharz().idAuraEff = msg.reader().readShort();
                            Char.myCharz().idEff_Set_Item = msg.reader().readSByte();
                            Char.myCharz().idHat = msg.reader().readShort();
                            break;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                case 4:
                    Char.myCharz().xu = msg.reader().readLong();
                    Char.myCharz().luong = msg.reader().readInt();
                    Char.myCharz().cHP = msg.reader().readDouble();
                    Char.myCharz().cMP = msg.reader().readDouble();
                    Char.myCharz().luongKhoa = msg.reader().readInt();
                    Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
                    Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
                    Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
                    break;
                case 5:
                    {
                        double cHP = Char.myCharz().cHP;
                        Char.myCharz().cHP = msg.reader().readDouble();
                        if (Char.myCharz().cHP > cHP && Char.myCharz().cTypePk != 4)
                        {
                            GameScr.startFlyText("+" + (Char.myCharz().cHP - cHP) + " " + mResources.HP, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 20, 0, -1, mFont.HP);
                            SoundMn.gI().HP_MPup();
                            if (Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5003)
                            {
                                MonsterDart.addMonsterDart(Char.myCharz().petFollow.cmx + ((Char.myCharz().petFollow.dir != 1) ? (-10) : 10), Char.myCharz().petFollow.cmy + 10, isBoss: true, -1, -1, Char.myCharz(), 29);
                            }
                        }
                        if (Char.myCharz().cHP < cHP)
                        {
                            GameScr.startFlyText("-" + (cHP - Char.myCharz().cHP) + " " + mResources.HP, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 20, 0, -1, mFont.HP);
                        }
                        GameScr.gI().dHP = Char.myCharz().cHP;
                        if (GameScr.isPaintInfoMe)
                        {
                        }
                        break;
                    }
                case 6:
                    {
                        if (Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5)
                        {
                            break;
                        }
                        double cMP = Char.myCharz().cMP;
                        Char.myCharz().cMP = msg.reader().readDouble();
                        if (Char.myCharz().cMP > cMP)
                        {
                            GameScr.startFlyText("+" + (Char.myCharz().cMP - cMP) + " " + mResources.KI, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 23, 0, -2, mFont.MP);
                            SoundMn.gI().HP_MPup();
                            if (Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5001)
                            {
                                MonsterDart.addMonsterDart(Char.myCharz().petFollow.cmx + ((Char.myCharz().petFollow.dir != 1) ? (-10) : 10), Char.myCharz().petFollow.cmy + 10, isBoss: true, -1, -1, Char.myCharz(), 29);
                            }
                        }
                        if (Char.myCharz().cMP < cMP)
                        {
                            GameScr.startFlyText("-" + (cMP - Char.myCharz().cMP) + " " + mResources.KI, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 23, 0, -2, mFont.MP);
                        }
                        GameScr.gI().dMP = Char.myCharz().cMP;
                        if (GameScr.isPaintInfoMe)
                        {
                        }
                        break;
                    }
                case 7:
                    {
                        Char @char = GameScr.findCharInMap(msg.reader().readInt());
                        if (@char == null)
                        {
                            break;
                        }
                        @char.clanID = msg.reader().readInt();
                        if (@char.clanID == -2)
                        {
                            @char.isCopy = true;
                        }
                        readCharInfo(@char, msg);
                        try
                        {
                            @char.idAuraEff = msg.reader().readShort();
                            @char.idEff_Set_Item = msg.reader().readSByte();
                            @char.idHat = msg.reader().readShort();
                            if (@char.bag >= 201)
                            {
                                Effect effect = new Effect(@char.bag, @char, 2, -1, 10, 1);
                                effect.typeEff = 5;
                                @char.addEffChar(effect);
                            }
                            else
                            {
                                @char.removeEffChar(0, 201);
                            }
                            break;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                case 8:
                    {
                        GameCanvas.debug("SA26", 2);
                        Char @char = GameScr.findCharInMap(msg.reader().readInt());
                        if (@char != null)
                        {
                            @char.cspeed = msg.reader().readByte();
                        }
                        break;
                    }
                case 9:
                    {
                        GameCanvas.debug("SA27", 2);
                        Char @char = GameScr.findCharInMap(msg.reader().readInt());
                        if (@char != null)
                        {
                            @char.cHP = msg.reader().readDouble();
                            @char.cHPFull = msg.reader().readDouble();
                        }
                        break;
                    }
                case 10:
                    {
                        GameCanvas.debug("SA28", 2);
                        Char @char = GameScr.findCharInMap(msg.reader().readInt());
                        if (@char != null)
                        {
                            @char.cHP = msg.reader().readDouble();
                            @char.cHPFull = msg.reader().readDouble();
                            @char.eff5BuffHp = msg.reader().readShort();
                            @char.eff5BuffMp = msg.reader().readShort();
                            @char.wp = msg.reader().readShort();
                            if (@char.wp == -1)
                            {
                                @char.setDefaultWeapon();
                            }
                        }
                        break;
                    }
                case 11:
                    {
                        GameCanvas.debug("SA29", 2);
                        Char @char = GameScr.findCharInMap(msg.reader().readInt());
                        if (@char != null)
                        {
                            @char.cHP = msg.reader().readDouble();
                            @char.cHPFull = msg.reader().readDouble();
                            @char.eff5BuffHp = msg.reader().readShort();
                            @char.eff5BuffMp = msg.reader().readShort();
                            @char.body = msg.reader().readShort();
                            if (@char.body == -1)
                            {
                                @char.setDefaultBody();
                            }
                        }
                        break;
                    }
                case 12:
                    {
                        GameCanvas.debug("SA30", 2);
                        Char @char = GameScr.findCharInMap(msg.reader().readInt());
                        if (@char != null)
                        {
                            @char.cHP = msg.reader().readDouble();
                            @char.cHPFull = msg.reader().readDouble();
                            @char.eff5BuffHp = msg.reader().readShort();
                            @char.eff5BuffMp = msg.reader().readShort();
                            @char.leg = msg.reader().readShort();
                            if (@char.leg == -1)
                            {
                                @char.setDefaultLeg();
                            }
                        }
                        break;
                    }
                case 13:
                    {
                        GameCanvas.debug("SA31", 2);
                        int num2 = msg.reader().readInt();
                        Char @char = ((num2 != Char.myCharz().charID) ? GameScr.findCharInMap(num2) : Char.myCharz());
                        if (@char != null)
                        {
                            @char.cHP = msg.reader().readDouble();
                            @char.cHPFull = msg.reader().readDouble();
                            @char.eff5BuffHp = msg.reader().readShort();
                            @char.eff5BuffMp = msg.reader().readShort();
                        }
                        break;
                    }
                case 14:
                    {
                        Char @char = GameScr.findCharInMap(msg.reader().readInt());
                        if (@char == null)
                        {
                            break;
                        }
                        @char.cHP = msg.reader().readDouble();
                        sbyte b4 = msg.reader().readByte();
                        if (b4 == 1)
                        {
                            ServerEffect.addServerEffect(11, @char, 5);
                            ServerEffect.addServerEffect(104, @char, 4);
                        }
                        if (b4 == 2)
                        {
                            @char.doInjure();
                        }
                        try
                        {
                            @char.cHPFull = msg.reader().readDouble();
                            break;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                case 15:
                    {
                        Char @char = GameScr.findCharInMap(msg.reader().readInt());
                        if (@char != null)
                        {
                            @char.cHP = msg.reader().readDouble();
                            @char.cHPFull = msg.reader().readDouble();
                            @char.cx = msg.reader().readShort();
                            @char.cy = msg.reader().readShort();
                            @char.statusMe = 1;
                            @char.cp3 = 3;
                            ServerEffect.addServerEffect(109, @char, 2);
                        }
                        break;
                    }
                case 35:
                    {
                        GameCanvas.debug("SY3", 2);
                        int num4 = msg.reader().readInt();
                        Res.outz("CID = " + num4);
                        if (TileMap.mapID == 130)
                        {
                            GameScr.gI().starVS();
                        }
                        if (num4 == Char.myCharz().charID)
                        {
                            Char.myCharz().cTypePk = msg.reader().readByte();
                            if (GameScr.gI().isVS() && Char.myCharz().cTypePk != 0)
                            {
                                GameScr.gI().starVS();
                            }
                            Res.outz("type pk= " + Char.myCharz().cTypePk);
                            Char.myCharz().npcFocus = null;
                            if (!GameScr.gI().isMeCanAttackMob(Char.myCharz().mobFocus))
                            {
                                Char.myCharz().mobFocus = null;
                            }
                            Char.myCharz().itemFocus = null;
                        }
                        else
                        {
                            Char @char = GameScr.findCharInMap(num4);
                            if (@char != null)
                            {
                                Res.outz("type pk= " + @char.cTypePk);
                                @char.cTypePk = msg.reader().readByte();
                                if (@char.isAttacPlayerStatus())
                                {
                                    Char.myCharz().charFocus = @char;
                                }
                            }
                        }
                        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                        {
                            Char char2 = GameScr.findCharInMap(i);
                            if (char2 != null && char2.cTypePk != 0 && char2.cTypePk == Char.myCharz().cTypePk)
                            {
                                if (!Char.myCharz().mobFocus.isMobMe)
                                {
                                    Char.myCharz().mobFocus = null;
                                }
                                Char.myCharz().npcFocus = null;
                                Char.myCharz().itemFocus = null;
                                break;
                            }
                        }
                        Res.outz("update type pk= ");
                        break;
                    }
                case 61:
                    {
                        string text = msg.reader().readUTF();
                        sbyte[] data = new sbyte[msg.reader().readInt()];
                        msg.reader().read(ref data);
                        if (data.Length == 0)
                        {
                            data = null;
                        }
                        if (text.Equals("KSkill"))
                        {
                            GameScr.gI().onKSkill(data);
                        }
                        else if (text.Equals("OSkill"))
                        {
                            GameScr.gI().onOSkill(data);
                        }
                        else if (text.Equals("CSkill"))
                        {
                            GameScr.gI().onCSkill(data);
                        }
                        break;
                    }
                case 23:
                    {
                        short num = msg.reader().readShort();
                        Skill skill = Skills.get(num);
                        useSkill(skill);
                        if (num != 0 && num != 14 && num != 28)
                        {
                            GameScr.info1.addInfo(mResources.LEARN_SKILL + " " + skill.template.name, 0);
                        }
                        break;
                    }
                case 62:
                    Res.outz("ME UPDATE SKILL");
                    read_UpdateSkill(msg);
                    break;
            }
        }
        catch (Exception ex5)
        {
            Cout.println("Loi tai Sub : " + ex5.ToString());
        }
        finally
        {
            msg?.cleanup();
        }
    }

    private void useSkill(Skill skill)
    {
        if (Char.myCharz().myskill == null)
        {
            Char.myCharz().myskill = skill;
        }
        else if (skill.template.Equals(Char.myCharz().myskill.template))
        {
            Char.myCharz().myskill = skill;
        }
        Char.myCharz().vSkill.addElement(skill);
        if ((skill.template.type == 1
            || skill.template.type == 4
            || skill.template.type == 2
            || skill.template.type == 3) && (skill.template.maxPoint == 0 || (skill.template.maxPoint > 0 && skill.point > 0)))
        {
            if (skill.template.id == Char.myCharz().skillTemplateId)
            {
                Service.gI().selectSkill(Char.myCharz().skillTemplateId);
            }
            Char.myCharz().vSkillFight.addElement(skill);
        }
    }

    public bool readCharInfo(Char c, Message msg)
    {
        try
        {
            c.clevel = msg.reader().readByte();
            c.isInvisiblez = msg.reader().readBoolean();
            c.cTypePk = msg.reader().readByte();
            c.nClass = GameScr.nClasss[msg.reader().readByte()];
            c.cgender = msg.reader().readByte();
            c.head = msg.reader().readShort();
            c.cName = msg.reader().readUTF();
            c.cHP = msg.reader().readDouble();
            c.dHP = c.cHP;
            if (c.cHP == 0)
            {
                c.statusMe = 14;
            }
            c.cHPFull = msg.reader().readDouble();
            if (c.cy >= TileMap.pxh - 100)
            {
                c.isFlyUp = true;
            }
            c.body = msg.reader().readShort();
            c.leg = msg.reader().readShort();
            c.bag = msg.reader().readUnsignedByte();
            c.isShadown = true;
            sbyte b = msg.reader().readByte();
            if (c.wp == -1)
            {
                c.setDefaultWeapon();
            }
            if (c.body == -1)
            {
                c.setDefaultBody();
            }
            if (c.leg == -1)
            {
                c.setDefaultLeg();
            }
            c.cx = msg.reader().readShort();
            c.cy = msg.reader().readShort();
            c.xSd = c.cx;
            c.ySd = c.cy;
            c.eff5BuffHp = msg.reader().readShort();
            c.eff5BuffMp = msg.reader().readShort();
            int num = msg.reader().readByte();
            for (int i = 0; i < num; i++)
            {
                EffectChar effectChar = new EffectChar(msg.reader().readByte(), msg.reader().readInt(), msg.reader().readInt(), msg.reader().readShort());
                c.vEff.addElement(effectChar);
                if (effectChar.template.type == 12 || effectChar.template.type == 11)
                {
                    c.isInvisiblez = true;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            ex.StackTrace.ToString();
        }
        return false;
    }

    private void readGetImgByName(Message msg)
    {
        try
        {
            string text = msg.reader().readUTF();
            sbyte nFrame = msg.reader().readByte();
            sbyte[] array = null;
            array = NinjaUtil.readByteArray(msg);
            Image img = createImage(array);
            ImgByName.SetImage(text, img, nFrame);
            if (array != null)
            {
                ImgByName.saveRMS(text, nFrame, array);
            }
        }
        catch (Exception)
        {
        }
    }


    public Image createImage(sbyte[] arr, string key)
    {
        try
        {
            string keyOrigin = DecryptString(key);
            sbyte[] decrypt = Array.ConvertAll(DecryptImage(Array.ConvertAll(arr, a => (byte)a), keyOrigin), a => (sbyte)a);
            return Image.createImage(decrypt, 0, decrypt.Length);
        }
        catch (Exception)
        {
        }
        return null;
    }
    public static string DecryptString(string encryptedText)
    {
        byte[] keyData = System.Text.Encoding.UTF8.GetBytes(ServerListScreen.keyDecryptString);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = keyData;
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            byte[] byteArray = Convert.FromBase64String(encryptedText);
            for (int i = 0; i < 4; i++)
            {
                byteArray = decryptor.TransformFinalBlock(byteArray, 0, byteArray.Length);
            }

            return System.Text.Encoding.UTF8.GetString(byteArray);
        }
    }


    public static byte[] DecryptImage(byte[] dataImage, string key)
    {
        Texture2D encryptedTexture = new Texture2D(1, 1);
        encryptedTexture.LoadImage(dataImage);
        int width = encryptedTexture.width;
        int height = encryptedTexture.height;
        ulong seed = (ulong)JavaHashCode(key);
        Assets.Assets.Scripts.Assembly_CSharp.Random random = new Assets.Assets.Scripts.Assembly_CSharp.Random(seed);

        Color32[] pixels = encryptedTexture.GetPixels32();
        Color32[] rearrangedPixels = new Color32[pixels.Length];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                rearrangedPixels[(height - y - 1) * width + x] = pixels[y * width + x];
            }
        }
        for (int i = 0; i < rearrangedPixels.Length; i++)
        {
            Color32 pixel = rearrangedPixels[i];
            if (pixel.a != 0)
            {
                int red = (pixel.r - random.NextInt(256) + 256) % 256;
                int green = (pixel.g - random.NextInt(256) + 256) % 256;
                int blue = (pixel.b - random.NextInt(256) + 256) % 256;
                rearrangedPixels[i] = new Color32((byte)red, (byte)green, (byte)blue, pixel.a);
            }
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                encryptedTexture.SetPixel(x, y, rearrangedPixels[y * width + (width - 1 - x)]);
            }
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                encryptedTexture.SetPixel(x, height - 1 - y, rearrangedPixels[y * width + x]);
            }
        }
        encryptedTexture.Apply();
        return encryptedTexture.EncodeToPNG();
    }
    private Color ConvertToUnityColor(Color32 color32)
    {
        return new Color32(color32.r, color32.g, color32.b, color32.a);
    }

    public static int JavaHashCode(string str)
    {
        int hash = 0;
        for (int i = 0; i < str.Length; i++) { hash = 31 * hash + str[i]; }
        return hash;
    }


    private void createItemNew(myReader d)
    {
        try
        {
            loadItemNew(d, -1, isSave: true);
        }
        catch (Exception)
        {
        }
    }

    private void loadItemNew(myReader d, sbyte type, bool isSave)
    {
        try
        {
            d.mark(100000);
            GameScr.vcItem = d.readByte();
            type = d.readByte();
            if (type == 0)
            {
                GameScr.gI().iOptionTemplates = new ItemOptionTemplate[d.readUnsignedByte()];
                for (int i = 0; i < GameScr.gI().iOptionTemplates.Length; i++)
                {
                    GameScr.gI().iOptionTemplates[i] = new ItemOptionTemplate();
                    GameScr.gI().iOptionTemplates[i].id = i;
                    GameScr.gI().iOptionTemplates[i].name = d.readUTF();
                    GameScr.gI().iOptionTemplates[i].type = d.readByte();
                }
                if (isSave)
                {
                    d.reset();
                    sbyte[] data = new sbyte[d.available()];
                    d.readFully(ref data);
                    Rms.saveRMS("NRitem0", data);
                }
            }
            else if (type == 1)
            {
                ItemTemplates.itemTemplates.clear();
                int num = d.readShort();
                for (int j = 0; j < num; j++)
                {
                    ItemTemplate it = new ItemTemplate((short)j, d.readByte(), d.readByte(), d.readUTF(), d.readUTF(), d.readByte(), d.readInt(), d.readShort(), d.readShort(), d.readBoolean());
                    ItemTemplates.add(it);
                }
                if (isSave)
                {
                    d.reset();
                    sbyte[] data2 = new sbyte[d.available()];
                    d.readFully(ref data2);
                    Rms.saveRMS("NRitem1", data2);
                }
            }
            else if (type == 2)
            {
                int num2 = d.readShort();
                int num3 = d.readShort();
                for (int k = num2; k < num3; k++)
                {
                    ItemTemplate it2 = new ItemTemplate((short)k, d.readByte(), d.readByte(), d.readUTF(), d.readUTF(), d.readByte(), d.readInt(), d.readShort(), d.readShort(), d.readBoolean());
                    ItemTemplates.add(it2);
                }
                if (isSave)
                {
                    d.reset();
                    sbyte[] data3 = new sbyte[d.available()];
                    d.readFully(ref data3);
                    Rms.saveRMS("NRitem2", data3);
                    sbyte[] data4 = new sbyte[1] { GameScr.vcItem };
                    Rms.saveRMS("NRitemVersion", data4);
                    LoginScr.isUpdateItem = false;
                    if (GameScr.vsData == GameScr.vcData && GameScr.vsMap == GameScr.vcMap && GameScr.vsSkill == GameScr.vcSkill && GameScr.vsItem == GameScr.vcItem)
                    {
                        GameScr.gI().readDart();
                        GameScr.gI().readEfect();
                        GameScr.gI().readArrow();
                        GameScr.gI().readSkill();
                        Service.gI().clientOk();
                    }
                }
            }
            else if (type == 100)
            {
                Char.Arr_Head_2Fr = readArrHead(d);
                if (isSave)
                {
                    d.reset();
                    sbyte[] data5 = new sbyte[d.available()];
                    d.readFully(ref data5);
                    Rms.saveRMS("NRitem100", data5);
                }
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }

    private void readFrameBoss(Message msg, int mobTemplateId)
    {
        try
        {
            int num = msg.reader().readByte();
            int[][] array = new int[num][];
            for (int i = 0; i < num; i++)
            {
                int num2 = msg.reader().readByte();
                array[i] = new int[num2];
                for (int j = 0; j < num2; j++)
                {
                    array[i][j] = msg.reader().readByte();
                }
            }
            frameHT_NEWBOSS.put(mobTemplateId + string.Empty, array);
        }
        catch (Exception)
        {
        }
    }

    private int[][] readArrHead(myReader d)
    {
        int[][] array = new int[1][] { new int[2] { 542, 543 } };
        try
        {
            int num = d.readShort();
            array = new int[num][];
            for (int i = 0; i < array.Length; i++)
            {
                int num2 = d.readByte();
                array[i] = new int[num2];
                for (int j = 0; j < num2; j++)
                {
                    array[i][j] = d.readShort();
                }
            }
        }
        catch (Exception)
        {
        }
        return array;
    }

    public void phuban_Info(Message msg)
    {
        try
        {
            sbyte b = msg.reader().readByte();
            if (b == 0)
            {
                readPhuBan_CHIENTRUONGNAMEK(msg, b);
            }
        }
        catch (Exception)
        {
        }
    }

    private void readPhuBan_CHIENTRUONGNAMEK(Message msg, int type_PB)
    {
        try
        {
            sbyte b = msg.reader().readByte();
            if (b == 0)
            {
                short idmapPaint = msg.reader().readShort();
                string nameTeam = msg.reader().readUTF();
                string nameTeam2 = msg.reader().readUTF();
                int maxPoint = msg.reader().readInt();
                short timeSecond = msg.reader().readShort();
                int maxLife = msg.reader().readByte();
                GameScr.phuban_Info = new InfoPhuBan(type_PB, idmapPaint, nameTeam, nameTeam2, maxPoint, timeSecond);
                GameScr.phuban_Info.maxLife = maxLife;
                GameScr.phuban_Info.updateLife(type_PB, 0, 0);
            }
            else if (b == 1)
            {
                int pointTeam = msg.reader().readInt();
                int pointTeam2 = msg.reader().readInt();
                if (GameScr.phuban_Info != null)
                {
                    GameScr.phuban_Info.updatePoint(type_PB, pointTeam, pointTeam2);
                }
            }
            else if (b == 2)
            {
                sbyte b2 = msg.reader().readByte();
                short type = 0;
                short num = -1;
                if (b2 == 1)
                {
                    type = 1;
                    num = 3;
                }
                else if (b2 == 2)
                {
                    type = 2;
                }
                num = -1;
                GameScr.phuban_Info = null;
                GameScr.addEffectEnd(type, num, 0, GameCanvas.hw, GameCanvas.hh, 0, 0, -1, null);
            }
            else if (b == 5)
            {
                short timeSecond2 = msg.reader().readShort();
                if (GameScr.phuban_Info != null)
                {
                    GameScr.phuban_Info.updateTime(type_PB, timeSecond2);
                }
            }
            else if (b == 4)
            {
                int lifeTeam = msg.reader().readByte();
                int lifeTeam2 = msg.reader().readByte();
                if (GameScr.phuban_Info != null)
                {
                    GameScr.phuban_Info.updateLife(type_PB, lifeTeam, lifeTeam2);
                }
            }
        }
        catch (Exception)
        {
        }
    }

    public void read_opt(Message msg)
    {
        try
        {
            sbyte b = msg.reader().readByte();
            if (b == 0)
            {
                short idHat = msg.reader().readShort();
                Char.myCharz().idHat = idHat;
                SoundMn.gI().getStrOption();
            }
            else if (b == 2)
            {
                int num = msg.reader().readInt();
                sbyte b2 = msg.reader().readByte();
                short num2 = msg.reader().readShort();
                string v = num2 + "," + b2;
                MainImage imagePath = ImgByName.getImagePath("banner_" + num2, ImgByName.hashImagePath);
                GameCanvas.danhHieu.put(num + string.Empty, v);
            }
            else if (b == 3)
            {
                short num3 = msg.reader().readShort();
                SmallImage.createImage(num3);
                BackgroudEffect.id_water1 = num3;
            }
            else if (b == 4)
            {
                string o = msg.reader().readUTF();
                GameCanvas.messageServer.addElement(o);
            }
        }
        catch (Exception)
        {
        }
    }

    public void read_UpdateSkill(Message msg)
    {
        try
        {
            short num = msg.reader().readShort();
            sbyte b = -1;
            try
            {
                b = msg.reader().readSByte();
            }
            catch (Exception)
            {
            }
            if (b == 0)
            {
                short curExp = msg.reader().readShort();
                for (int i = 0; i < Char.myCharz().vSkill.size(); i++)
                {
                    Skill skill = (Skill)Char.myCharz().vSkill.elementAt(i);
                    if (skill.skillId == num)
                    {
                        skill.curExp = curExp;
                        break;
                    }
                }
            }
            else if (b == 1)
            {
                sbyte b2 = msg.reader().readByte();
                for (int j = 0; j < Char.myCharz().vSkill.size(); j++)
                {
                    Skill skill2 = (Skill)Char.myCharz().vSkill.elementAt(j);
                    if (skill2.skillId == num)
                    {
                        for (int k = 0; k < 20; k++)
                        {
                            string nameImg = "Skills_" + skill2.template.id + "_" + b2 + "_" + k;
                            MainImage imagePath = ImgByName.getImagePath(nameImg, ImgByName.hashImagePath);
                        }
                        break;
                    }
                }
            }
            else
            {
                if (b != -1)
                {
                    return;
                }
                Skill skill3 = Skills.get(num);
                for (int l = 0; l < Char.myCharz().vSkill.size(); l++)
                {
                    Skill skill4 = (Skill)Char.myCharz().vSkill.elementAt(l);
                    if (skill4.template.id == skill3.template.id)
                    {
                        Char.myCharz().vSkill.setElementAt(skill3, l);
                        break;
                    }
                }
                for (int m = 0; m < Char.myCharz().vSkillFight.size(); m++)
                {
                    Skill skill5 = (Skill)Char.myCharz().vSkillFight.elementAt(m);
                    if (skill5.template.id == skill3.template.id)
                    {
                        Char.myCharz().vSkillFight.setElementAt(skill3, m);
                        break;
                    }
                }
                for (int n = 0; n < GameScr.onScreenSkill.Length; n++)
                {
                    if (GameScr.onScreenSkill[n] != null && GameScr.onScreenSkill[n].template.id == skill3.template.id)
                    {
                        GameScr.onScreenSkill[n] = skill3;
                        break;
                    }
                }
                for (int num2 = 0; num2 < GameScr.keySkill.Length; num2++)
                {
                    if (GameScr.keySkill[num2] != null && GameScr.keySkill[num2].template.id == skill3.template.id)
                    {
                        GameScr.keySkill[num2] = skill3;
                        break;
                    }
                }
                if (Char.myCharz().myskill.template.id == skill3.template.id)
                {
                    Char.myCharz().myskill = skill3;
                }
                GameScr.info1.addInfo(mResources.hasJustUpgrade1 + skill3.template.name + mResources.hasJustUpgrade2 + skill3.point, 0);
            }
        }
        catch (Exception)
        {
        }
    }
}
