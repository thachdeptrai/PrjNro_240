using System;
using System.Collections.Generic;
using UnityEngine;

public class QuayTamBao
{
    // Danh sách các vật phẩm hiển thị và nhận thưởng
    public static List<ItemTamBao> listItem = new List<ItemTamBao>();
    public static List<ItemTamBao> listNhan = new List<ItemTamBao>();

    // Biến chi phí quay
    public static int phiQuayx1 = 1;
    public static int phiQuayx10 = 10;

    // ID biểu tượng
    public static int icon1 = 10;
    public static int icon2 = 10;

    // Cờ trạng thái
    public static bool isTamBao = false;
    public static bool isTamBaoVip = false;
    public static bool isNhan = false;

    // Hình ảnh giao diện
    public static Image khung;
    public static Image background;
    public static Image btnExit;
    public static Image btnRollx1;
    public static Image btnRollx10;
    public static Image thuong;
    public static Image vip;
    public static Image txtThuong;
    public static Image txtVip;
    public static Image chucMung;
    public static Image btnDong;
    public static Image btnReRoll;
    public static Image imgSelect;
    public static Image[] bg = new Image[5];
    public static Image[] effr = new Image[4];
    public static Image[] effy = new Image[4];
    public static Image quay;
    public static Item item = new Item();
    public static int vanMay = 0;

    // Trạng thái quay
    public static bool isQuay = false;
    public static long lastQuay;
    public static int soLuot;

    // Phương thức tải hình ảnh
    public static void loadImage()
    {
        for (int i = 13; i <= 23; i++)
        {
            listItem.Add(new ItemTamBao(i, 6));
        }

        background = loadIMG("/quayTamBao/background");
        btnExit = loadIMG("/quayTamBao/btnExit");
        btnRollx1 = loadIMG("/quayTamBao/btnRollx1");
        btnRollx10 = loadIMG("/quayTamBao/btnRollx10");
        thuong = loadIMG("/quayTamBao/thuong");
        txtThuong = loadIMG("/quayTamBao/txtThuong");
        vip = loadIMG("/quayTamBao/vip");
        txtVip = loadIMG("/quayTamBao/txtVip");
        imgSelect = loadIMG("/quayTamBao/select");
        btnDong = loadIMG("/quayTamBao/btnDong");
        btnReRoll = loadIMG("/quayTamBao/btnReRoll");
        quay = loadIMG("/quayTamBao/quay");
        chucMung = loadIMG("/quayTamBao/chucMung");

        for (int i = 0; i < 5; i++)
        {
            bg[i] = loadIMG("/quayTamBao/bg" + i);
        }

        for (int i = 0; i < 4; i++)
        {
            effr[i] = loadIMG("/quayTamBao/effr" + i);
        }

        for (int i = 0; i < 4; i++)
        {
            effy[i] = loadIMG("/quayTamBao/effy" + i);
        }
    }

    // Phương thức tải hình ảnh từ đường dẫn
    static Image loadIMG(string path)
    {
        TextAsset texture = Resources.Load("res/x2" + path) as TextAsset;
        return Image.createImage(texture.bytes);
    }

    // Phương thức vẽ giao diện
    public static void paint(mGraphics g)
    {
        g.drawImageScale(background, -1, -2, GameCanvas.w + 10, GameCanvas.h + 10, 0);
        g.drawImageScale(btnExit, GameCanvas.w - 30, 10, 20, 20, 0);

        // Vẽ các nút thường và VIP
        g.drawImage(thuong, GameCanvas.hw - 70, 15);
        g.drawImage(vip, GameCanvas.hw + 10, 15);

        if (isNhan)
        {
            paintNhanThuong(g, listNhan);
        }
        else
        {
            paintListItem(g, listItem);
        }

        // Vẽ nút quay
        mFont.tahoma_7b_white.drawString(g, isNhan ? "" : ("Tốn " + phiQuayx1.ToString()), GameCanvas.w / 2 - 90, GameCanvas.h - 45, mFont.LEFT);

        // Vẽ biểu tượng chìa khóa
        SmallImage.drawSmallImage(g, isTamBaoVip ? icon2 : icon1, GameCanvas.w / 2 - 60, GameCanvas.h - 40, 0, 3);
        g.drawImageScale(isNhan ? btnDong : btnRollx1, GameCanvas.w / 2 - 110, GameCanvas.h - 35, 66, 26, 0);

        mFont.tahoma_7b_white.drawString(g, isNhan ? "" : ("Tốn " + phiQuayx10.ToString()), GameCanvas.w / 2 + 40, GameCanvas.h - 45, mFont.LEFT);
        SmallImage.drawSmallImage(g, isTamBaoVip ? icon2 : icon1, GameCanvas.w / 2 + 75, GameCanvas.h - 40, 0, 3);
        g.drawImageScale(isNhan ? btnReRoll : btnRollx10, GameCanvas.w / 2 + 30, GameCanvas.h - 35, 66, 26, 0);
    }

    // Phương thức vẽ phần thưởng nhận được
    public static void paintNhanThuong(mGraphics g, List<ItemTamBao> listItemz)
    {
        int y = GameCanvas.hh;
        g.drawImageScale(chucMung, GameCanvas.hw - 70, 40, 140, 60, 0);

        for (int i = 0; i < listItemz.Count; i++)
        {
            int x = GameCanvas.w / 2 - 30 * (listItemz.Count / 2) + 35 * i;
            int id = listItemz[i].id;
            int type = ItemTemplates.get((short)id).type;

            int colorIndex;
            switch (type)
            {
                case 21:
                    colorIndex = 4;
                    break;
                case 93:
                    colorIndex = 1;
                    break;
                case 5:
                    colorIndex = 3;
                    break;
                case 11:
                    colorIndex = 2;
                    break;
                default:
                    colorIndex = 0;
                    break;
            }

            g.drawImageScale(bg[colorIndex], x - 13, y - 13, 25, 25, 0);

            if (type == 21 || type == 5)
            {
                g.drawImageScale(effr[count % 4], x - 18, y - 18, 35, 35, 0);
            }

            if (i == select && isQuay)
            {
                g.drawImageScale(imgSelect, x - 20, y - 20, 40, 40, 0);
            }

            SmallImage.drawSmallImage(g, ItemTemplates.get((short)id).iconID, x, y, 0, 3);
        }

        if (mSystem.currentTimeMillis() - last > 20)
        {
            count++;
            last = mSystem.currentTimeMillis();
        }
    }



    // Biến đếm và lựa chọn
    public static long count = 0;
    public static long select = 0;
    public static long lastSelect = mSystem.currentTimeMillis();
    public static long last = mSystem.currentTimeMillis();

    // Phương thức vẽ danh sách các vật phẩm

    public static void paintListItem(mGraphics g, List<ItemTamBao> listItemz)
    {
        int y = 70;
        g.drawImageScale(isTamBaoVip ? txtVip : txtThuong, GameCanvas.hw - 25, 37, 50, 20, 0);

        // Số lượng cột cho các hàng khác nhau
        int[] columnsPerRow = { 11, 2, 2, 2, 2, 11 };

        // Tính toán các điểm x của các hàng
        int totalColumns = 11;  // Số cột trong hàng 1 và 6
        int itemWidth = 35;
        int itemSpacing = 35; // Khoảng cách giữa các ô
        int startOffset = GameCanvas.w / 2 - (totalColumns / 2) * itemSpacing;
        int endOffset = GameCanvas.w / 2 + (totalColumns / 2) * itemSpacing;

        int rowSpacing = 30;  // Khoảng cách giữa các hàng

        int r = columnsPerRow.Length;
        select %= listItemz.Count;

        int itemIndex = 0;

        for (int i = 0; i < r; i++)
        {
            int c = columnsPerRow[i];
            for (int j = 0; j < c && itemIndex < listItemz.Count; j++, itemIndex++)
            {
                int x;
                if (i == 0 || i == 5) // Hàng 1 và hàng 6
                {
                    x = startOffset + j * itemSpacing;
                }
                else // Hàng 2 đến 5
                {
                    if (j == 0)
                    {
                        x = startOffset; // Ô đầu hàng 2, 3, 4, 5
                    }
                    else
                    {
                        x = endOffset; // Ô cuối hàng 2, 3, 4, 5
                    }
                }

                int id = listItemz[itemIndex].id;
                int type = ItemTemplates.get((short)id).type;

                // Xác định màu sắc dựa trên type
                int colorIndex;
                switch (type)
                {
                    case 21:
                        colorIndex = 4; // Màu tương ứng với type 21
                        break;
                    case 93:
                        colorIndex = 1; // Màu tương ứng với type 93
                        break;
                    case 5:
                        colorIndex = 3; // Màu tương ứng với type 5
                        break;
                    case 11:
                        colorIndex = 2; // Màu tương ứng với type 11
                        break;
                    default:
                        colorIndex = 0; // Màu mặc định
                        break;
                }

                g.drawImageScale(bg[colorIndex], x - 13, y - 13, 25, 25, 0);

                // Chỉ vẽ viền sáng cho các loại 21 và 5
                if (type == 21 || type == 5)
                {
                    g.drawImageScale(effr[count % 4], x - 18, y - 18, 35, 35, 0);
                }
                //else
                //{
                //    if (listItemz[itemIndex].color % 5 == 3)
                //    {
                //        g.drawImageScale(effy[count % 4], x - 18, y - 18, 35, 35, 0);
                //    }
                //    else if (listItemz[itemIndex].color % 5 == 4)
                //    {
                //        g.drawImageScale(effr[count % 4], x - 18, y - 18, 35, 35, 0);
                //    }
                //}

                if (itemIndex == select && isQuay)
                {
                    g.drawImageScale(imgSelect, x - 20, y - 20, 40, 40, 0);
                }

                SmallImage.drawSmallImage(g, ItemTemplates.get((short)id).iconID, x, y, 0, 3);
            }
            y += rowSpacing;
        }

        if (mSystem.currentTimeMillis() - last > 20)
        {
            count++;
            last = mSystem.currentTimeMillis();
        }

        if (mSystem.currentTimeMillis() - lastSelect > speed && isQuay)
        {
            select++;
            lastSelect = mSystem.currentTimeMillis();

            if (mSystem.currentTimeMillis() - lastQuay >= 3000)
            {
                isQuay = false;
                isNhan = true;
                select = 0;
            }
        }
    }

    // Tốc độ quay
    public static int speed = 30;

    // Phương thức xử lý khi quay tầm bảo
    public static void doTamBao()
    {
        if (GameCanvas.isPointerHoldIn(GameCanvas.hw - 70, 15, 58, 24) && GameCanvas.isPointerJustRelease)
        {
            QuayTamBao.isTamBaoVip = false;
            sendDataTamBao();
        }

        if (GameCanvas.isPointerHoldIn(GameCanvas.hw + 10, 15, 58, 24) && GameCanvas.isPointerJustRelease)
        {
            QuayTamBao.isTamBaoVip = true;
            sendDataTamBao();
        }

        if (GameCanvas.isPointerHoldIn(GameCanvas.w - 30, 10, 20, 20) && GameCanvas.isPointerJustRelease)
        {
            QuayTamBao.isTamBao = false;
        }

        if (!isQuay && GameCanvas.isPointerHoldIn(GameCanvas.w / 2 - 110, GameCanvas.h - 35, 66, 26) && GameCanvas.isPointerJustRelease)
        {
            speed = Res.random(10, 50);
            lastQuay = mSystem.currentTimeMillis();

            if (isNhan)
            {
                isNhan = false;
            }
            else
            {
                soLuot = 1;
                sendTamBao(soLuot);
                soLuot = 1;
            }
        }


        if (!isQuay && GameCanvas.isPointerHoldIn(GameCanvas.w / 2 + 30, GameCanvas.h - 35, 66, 26) && GameCanvas.isPointerJustRelease)
        {
            speed = Res.random(10, 50);
            lastQuay = mSystem.currentTimeMillis();
            if (isNhan)
            {
                isNhan = false;
            }
            else
            {
                soLuot = 10;
                sendTamBao(soLuot);
                soLuot = 10;
            }
        }

        Char.myCharz().currentMovePoint = null;
        GameCanvas.clearAllPointerEvent();
    }


    // Phương thức nhận tin nhắn
    public static void receiveMsg(Message msg)
    {
        try
        {
            int type = msg.reader().readByte();
            switch (type)
            {
                case 0:
                case 3:
                    if (type == 3) isTamBaoVip = true;
                    else isTamBaoVip = false;

                    listItem = new List<ItemTamBao>();
                    int size = msg.readInt3Byte();
                    for (int i = 0; i < size; i++)
                    {
                        listItem.Add(new ItemTamBao(msg.readInt3Byte(), msg.readInt3Byte()));
                    }
                    vanMay = msg.readInt3Byte();
                    icon1 = msg.readInt3Byte();
                    icon2 = msg.readInt3Byte();
                    break;
                case 1:
                    isQuay = true;
                    lastQuay = mSystem.currentTimeMillis();
                    listNhan = new List<ItemTamBao>();
                    size = msg.readInt3Byte();
                    for (int i = 0; i < size; i++)
                    {
                        listNhan.Add(new ItemTamBao(msg.readInt3Byte(), msg.readInt3Byte()));
                    }
                    break;
            }
        }
        catch (Exception e)
        {
            e.ToString();
        }
    }


    // Phương thức gửi yêu cầu quay tầm bảo
    public static void sendTamBao(int luotQuay)
    {
        try
        {
            Message msg = new Message(70);
            msg.writer().writeByte(isTamBaoVip ? 4 : 1); // Kiểu quay
            msg.writer().writeInt(luotQuay); // Số lượt quay
            Session_ME.gI().sendMessage(msg); // Gửi thông điệp
        }
        catch (Exception e)
        {
            e.ToString();
        }
    }


    // Phương thức gửi dữ liệu tầm bảo
    public static void sendDataTamBao()
    {
        try
        {
            Message msg = new Message(70);
            msg.writer().writeByte(isTamBaoVip ? 3 : 0);
            Session_ME.gI().sendMessage(msg);
        }
        catch (Exception e)
        {
            e.ToString();
        }
    }
}

/*
loadImage(): Phương thức này tải tất cả các hình ảnh cần thiết từ các đường dẫn chỉ định.
loadIMG(string path): Tải một hình ảnh từ đường dẫn chỉ định.
paint(mGraphics g): Phương thức này vẽ giao diện chính, bao gồm nền, các nút, và các vật phẩm.
paintNhanThuong(mGraphics g, List<ItemTamBao> listItemz): Vẽ danh sách các vật phẩm nhận được khi quay.
paintListItem(mGraphics g, List<ItemTamBao> listItemz): Vẽ danh sách các vật phẩm có thể quay được.
doTamBao(): Xử lý các sự kiện khi người chơi nhấn vào các nút quay, nút thường và VIP.
receiveMsg(Message msg): Nhận và xử lý các tin nhắn từ máy chủ, cập nhật danh sách vật phẩm và trạng thái.
sendTamBao(int luotQuay): Gửi yêu cầu quay tầm bảo lên máy chủ.
sendDataTamBao(): Gửi dữ liệu tầm bảo lên máy chủ.
*/
