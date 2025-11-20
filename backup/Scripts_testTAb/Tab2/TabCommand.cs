namespace Tab2
{
    using System;

    public class TabCommand : BaseCommand
    {
        private static Image menu, menu1;

        public static int spacing = 25;
        public TabCommand(string caption, Action action, int index, int startX, int startY) : base(caption, action)
        {
            this.caption = caption;
            this.action = action;
            this.w = 20;
            this.h = 20;
            this.x = startX + index * spacing;
            this.y = startY;
        }
        public TabCommand(string caption, Action action) : base(caption, action)
        {
            this.caption = caption;
            this.action = action;
            this.w = 20  ;
            this.h = 30;
        }
        public static void loadBG()
        {
            menu = GameCanvas.loadImage("/mainImage/img1.png");
            menu1 = GameCanvas.loadImage("/mainImage/img2.png");
        }
        public override void paint(mGraphics g)
        {
            g.drawImage(isFocus ? menu1 : menu, x - 220, y);
            mFont.tahoma_7b_dark.drawString(g, caption, x + w / 2 - 220, y + h / 2 - mFont.tahoma_7b_dark.getHeight() / 2,mFont.LEFT);
        }

        public override bool isPointerInside()
        {
            isFocus = false;
            if (GameCanvas.isPointerHoldIn(x - 220 , y, w, h))
            {
                if (GameCanvas.isPointerDown)
                {
                    isFocus = true;
                }
                if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                {
                    return true;
                }
            }
            return false;
        }
        public override void Invoke()
        {
            GameCanvas.clearAllPointerEvent();
            action?.Invoke();
        }
    }
}
