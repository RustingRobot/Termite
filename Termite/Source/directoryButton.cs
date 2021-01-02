using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Termite.Engine;

namespace Termite.Source
{
    class DirectoryButton: Button
    {
        public ChangeDirButton cdbtn;

        public DirectoryButton(string PATH, DirectoryNode NODE) : base(PATH, NODE)
        {
            node = NODE;
            path = PATH;
            size = node.size;
            cdbtn = new ChangeDirButton(NODE);
        }

        public override void Update(Vector2 OFFSET)
        {
            if (node.locked) { color = new Color(60, 10, 10, 80); }
            else
            {
                if (Globals.GetBoxOverlap((pos - dim / 2) + OFFSET, dim, Globals.mouse.newMousePos, Vector2.Zero))
                {
                    color = new Color(50, 50, 50, 50);
                    if (Globals.mouse.LeftClick())
                    {
                        //main.SwitchDirectory(node);
                        if (unfolded)
                        {
                            main.CloseDirectory(node);
                            unfolded = false;
                        }
                        else
                        {
                            main.OpenDirectory(node);
                            unfolded = true;
                        }
                    }
                }
                else { color = new Color(50, 50, 50, 80); }
            }
            cdbtn.pos = new Vector2(Globals.screenWidth - 88 + cdbtn.dim.X / 2, pos.Y);
            cdbtn.dim = new Vector2(40, 30);
            cdbtn.Update(OFFSET);
            base.Update(OFFSET);
        }

        public override void Draw(Vector2 OFFSET)
        {
            cdbtn.Draw(OFFSET);
            base.Draw(OFFSET);
        }
    }
}
