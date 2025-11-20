namespace Tab2
{
    using System;
    
    public abstract class BaseCommand
    {
        public int x, y, w, h;
        public bool isFocus;
        public string caption;
        public Action action;
        protected BaseCommand(string caption, Action action)
        {
            this.caption = caption;
            this.action = action;
        }
        public abstract void paint(mGraphics g);
        public abstract bool isPointerInside();
        public abstract void Invoke();
    }
}
