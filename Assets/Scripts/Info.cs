public class Info : IActionListener
{
	public MyVector infoWaitToShow = new MyVector();

	public InfoItem info;

	public int p1 = 5;

	public int p2;

	public int p3;

	public int x;

	public int strWidth;

	public int limLeft = 2;

	public int hI = 20;

	public int xChar;

	public int yChar;

	public int sayWidth = 100;

	public int sayRun;

	public string[] says;

	public int cx;

	public int cy;

	public int ch;

	public bool outSide;

	public int f;

	public int tF;

	public Image img;

	public static Image gocnhon = GameCanvas.loadImage("/mainImage/myTexture2dgocnhon.png");

	public int time;

	public int timeW;

	public int type;

	public int X;

	public int Y;

	public int W;

	public int H;

	public void hide()
	{
		says = null;
		infoWaitToShow.removeAllElements();
	}

    public void paint(mGraphics g, int x, int y, int dir) 
    {
        if (infoWaitToShow.size() == 0)
        {
            return;
        }

        g.translate(x, y);

        if (says != null && says.Length != 0 && type != 1)
        {
            if (outSide)
            {
                cx -= GameScr.cmx;
                cy -= GameScr.cmy;
                cy += 35;
            }

            int zoomOffset = (mGraphics.zoomLevel != 1) ? 10 : 0;

            if (info.charInfo == null)
            {
                PopUp.paintPopUp(g, X, Y, W, H, 16777215, isButton: false);
            }
            else
            {
                mSystem.paintPopUp2(g, X - 23, Y - zoomOffset / 2, W + 15, H + ((!GameCanvas.isTouch) ? 14 : 0) + zoomOffset);
            }

            if (info.charInfo == null)
            {
                g.drawRegion(
                    gocnhon, 0, 0, 9, 8,
                    (dir != 1) ? 2 : 0,
                    cx - 3 + ((dir != 1) ? 20 : (-15)),
                    cy - ch - 20 + sayRun + 2,
                    mGraphics.TOP | mGraphics.HCENTER
                );
            }

            int lastColorType = -1; 

            for (int i = 0; i < says.Length; i++)
            {
                mFont font = mFont.tahoma_7;
                int fontSize = 2;
                string text = says[i];
                int colorType = 0;

                if (says[i].StartsWith("|"))
                {
                    string[] arr = Res.split(says[i], "|", 0);
                    if (arr.Length == 3)
                    {
                        text = arr[2];
                    }
                    if (arr.Length == 4)
                    {
                        text = arr[3];
                        fontSize = int.Parse(arr[2]);
                    }
                    colorType = int.Parse(arr[1]);
                    lastColorType = colorType;
                }
                else
                {
                    colorType = lastColorType;
                }

                switch (colorType)
                {
                    case -1: font = mFont.tahoma_7; break;
                    case 0: font = mFont.tahoma_7b_dark; break;
                    case 1: font = mFont.tahoma_7b_green; break;
                    case 2: font = mFont.tahoma_7b_blue; break;
                    case 3: font = mFont.tahoma_7_red; break;
                    case 4: font = mFont.tahoma_7_green; break;
                    case 5: font = mFont.tahoma_7_blue; break;
                    case 7: font = mFont.tahoma_7b_red; break;
                }

                if (info.charInfo == null)
                {
                    font.drawString(
                        g, text, cx, cy - ch - 15 + sayRun + i * 12 - says.Length * 12 - 9, 2
                    );
                    continue;
                }

                int popupX = X - 23;
                int popupY = Y - zoomOffset / 2;
                int popupWidth = ((mSystem.clientType != 1) ? (W + 25) : (W + 28));
                int popupHeight = H + ((!GameCanvas.isTouch) ? 14 : 0) + zoomOffset;

                g.setColor(4465169); 
                g.fillRect(popupX, popupY + popupHeight, popupWidth, 2);

                int timeWidth = info.timeCount * popupWidth / info.maxTime;
                if (timeWidth < 0) timeWidth = 0;

                g.setColor(43758);  
                g.fillRect(popupX, popupY + popupHeight, timeWidth, 2);

                if (info.timeCount == 0)
                {
                    return;
                }

                info.charInfo.paintCharBody(g, X - 7, Y + H + 5, 1, 0, true);

                if (mGraphics.zoomLevel == 1)
                {
                    ((!info.isChatServer) ? mFont.tahoma_7b_greenSmall : mFont.tahoma_7b_yellowSmall2)
                        .drawString(g, (info.charInfo.isTichXanh ? "     " : string.Empty) + info.charInfo.cName, X + 12, Y + 3, 0);

                    if (info.charInfo.isTichXanh)
                        ModFunc.PaintTicks(g, X + 8, Y + 2);
                }
                else
                {
                    ((!info.isChatServer) ? mFont.tahoma_7b_greenSmall : mFont.tahoma_7b_yellowSmall2)
                        .drawString(g, (info.charInfo.isTichXanh ? "     " : string.Empty) + info.charInfo.cName, X + 12, Y - 3, 0);

                    if (info.charInfo.isTichXanh)
                        ModFunc.PaintTicks(g, X + 9, Y - 2);
                }

                if (!GameCanvas.isTouch)
                {
                    if (!TField.isQwerty)
                        mFont.tahoma_7b_green2Small.drawString(g, "Nhấn # để chat", X + W / 2 + 10, Y + H, mFont.CENTER);
                    else
                        mFont.tahoma_7b_green2Small.drawString(g, "Nhấn Y để chat", X + W / 2 + 10, Y + H, mFont.CENTER);
                }

                if (mGraphics.zoomLevel == 1)
                {
                    TextInfo.paint(g, text, X + 14, Y + H / 2 + 2, W - 16, H, mFont.tahoma_7_whiteSmall);
                }
                else
                {
                    string[] arrLines = mFont.tahoma_7_whiteSmall.splitFontArray(text, 120);
                    for (int j = 0; j < arrLines.Length; j++)
                    {
                        mFont.tahoma_7_whiteSmall.drawString(g, arrLines[j], X + 12, Y + 12 + j * 12 - 3, 0);
                    }
                }

                GameCanvas.resetTrans(g);
            }
        }

        g.translate(-x, -y);
    }


	public void update()
	{
		if (infoWaitToShow.size() == 0 || info.timeCount != 0)
		{
			return;
		}
		time++;
		if (time >= info.speed)
		{
			time = 0;
			infoWaitToShow.removeElementAt(0);
			if (infoWaitToShow.size() != 0)
			{
				InfoItem infoItem = (InfoItem)infoWaitToShow.firstElement();
				info = infoItem;
				getInfo();
			}
		}
	}

	public void getInfo()
	{
		sayWidth = 100;
		if (GameCanvas.w == 128)
		{
			sayWidth = 128;
		}
		int num;
		if (info.charInfo != null)
		{
			says = new string[1] { info.s };
			num = ((mGraphics.zoomLevel != 1) ? mFont.tahoma_7_whiteSmall.splitFontArray(info.s, 120).Length : says.Length);
		}
		else
		{
			says = mFont.tahoma_7.splitFontArray(info.s, sayWidth - 10);
			num = says.Length;
		}
		sayRun = 7;
		X = cx - sayWidth / 2 - 1;
		Y = cy - ch - 15 + sayRun - num * 12 - 15;
		W = sayWidth + 2 + ((info.charInfo != null) ? 30 : 0);
		H = (num + 1) * 12 + 1 + ((info.charInfo != null) ? 5 : 0);
	}

	public void addInfo(string s, int Type, Char cInfo, bool isChatServer)
	{
		type = Type;
		if (GameCanvas.w == 128)
		{
			limLeft = 1;
		}
		if (infoWaitToShow.size() > 10)
		{
			infoWaitToShow.removeElementAt(0);
		}
		if (infoWaitToShow.size() > 0)
		{
			s.Equals(((InfoItem)infoWaitToShow.lastElement()).s);
		}
		InfoItem infoItem = new InfoItem(s);
		if (type == 0)
		{
			infoItem.speed = s.Length;
		}
		if (infoItem.speed < 70)
		{
			infoItem.speed = 70;
		}
		if (type == 1)
		{
			infoItem.speed = 10000000;
		}
		if (type == 3)
		{
			infoItem.speed = 300;
			infoItem.last = mSystem.currentTimeMillis();
			infoItem.maxTime = (infoItem.timeCount = 80);
		}
		if (cInfo != null)
		{
			infoItem.charInfo = cInfo;
			infoItem.isChatServer = isChatServer;
			GameCanvas.panel.addChatMessage(infoItem);
			if (GameCanvas.isTouch && GameCanvas.panel.isViewChatServer)
			{
				GameScr.info2.cmdChat = new Command(mResources.CHAT, this, 1000, infoItem);
			}
		}
		if ((cInfo != null && GameCanvas.panel.isViewChatServer) || cInfo == null)
		{
			infoWaitToShow.addElement(infoItem);
		}
		if (infoWaitToShow.size() == 1)
		{
			info = (InfoItem)infoWaitToShow.firstElement();
			getInfo();
		}
		if (GameCanvas.isTouch && cInfo != null && GameCanvas.panel.isViewChatServer && GameCanvas.w - 50 > 155 + W)
		{
			GameScr.info2.cmdChat.x = GameCanvas.w - W - 50;
			GameScr.info2.cmdChat.y = 35;
		}
	}

	public void perform(int idAction, object p)
	{
		if (idAction == 1000)
		{
			ChatTextField.gI().startChat(GameScr.gI(), mResources.chat_player);
		}
	}
}
