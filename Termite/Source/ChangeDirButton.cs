using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Termite.Engine;

namespace Termite.Source
{
    class ChangeDirButton : ESprite2d
    {
        public Color color;
        public DirectoryNode node;

        public ChangeDirButton(DirectoryNode NODE) : base("Square", Vector2.Zero, Vector2.Zero)
        {
            node = NODE;
        }

        public ChangeDirButton() : base("Square", Vector2.Zero, Vector2.Zero)
        {
        }

        public override void Update(Vector2 OFFSET)
        {
            if (Globals.GetBoxOverlap((pos - dim / 2) + OFFSET, dim, Globals.mouse.newMousePos, Vector2.Zero))
            {
                color = new Color(50, 50, 50, 50);
                if (Globals.mouse.LeftClick() && node != null)
                {
                    main.SwitchDirectory(node);
                }
            }
            else { color = new Color(50, 50, 50, 80); }
        }

        public override void Draw(Vector2 OFFSET)
        {
            base.Draw(OFFSET, color);
        }
    }
}