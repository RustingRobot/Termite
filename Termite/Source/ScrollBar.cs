using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Termite.Engine;

namespace Termite.Source
{
    public class ScrollBar : ESprite2d
    {
        public float swPos, clickPos;
        public Color swCol;
        public bool dragged;

        public ScrollBar() : base("Square", Vector2.Zero, Vector2.Zero)
        {
            swCol = new Color(100, 100, 100);
        }

        public override void Update(Vector2 OFFSET)
        {
            if (Globals.GetBoxOverlap((new Vector2(Globals.screenWidth - 10, swPos + 40) - new Vector2(20, 80) / 2), new Vector2(20, 80), Globals.mouse.newMousePos, Vector2.Zero))
            {
                swCol = new Color(80, 80, 80);
                if (Globals.mouse.LeftClick() && !dragged)
                {
                    dragged = true;
                    clickPos = Globals.mouse.newMousePos.Y - swPos;
                }
            }
            else if(!dragged) { swCol = new Color(100, 100, 100); }
            if (Globals.mouse.LeftClickRelease()) { dragged = false; }
            if (dragged)
            {
                swPos = Globals.mouse.newMousePos.Y - clickPos;
                if (swPos < 40) swPos = 40;
                if (swPos > Globals.screenHeight - 80) swPos = Globals.screenHeight - 80;
                Globals.offset.Y = (swPos - 40) / (Globals.screenHeight - 120) * Globals.scrollMax;
            }
            else
            {
                swPos = 40 + (Globals.offset.Y / Globals.scrollMax * (Globals.screenHeight - 120));
            }
            base.Update(OFFSET);
        }

        public override void Draw(Vector2 OFFSET)
        {
            Globals.primitives.DrawRect(new Vector2(Globals.screenWidth - 20, 40), new Vector2(20, Globals.screenHeight),new Color(50, 50, 50, 80));    //bg
            Globals.primitives.DrawRect(new Vector2(Globals.screenWidth - 20, swPos), new Vector2(20, 80), swCol);   //actual bar
            base.Draw(OFFSET);
        }
    }
}
