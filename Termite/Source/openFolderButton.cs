using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Termite.Engine;

namespace Termite.Source
{
    class OpenFolderButton : ESprite2d
    {
        public Color color;
        public EText text;
        public ChangeDirButton cdbtn;

        public OpenFolderButton() : base("Square", new Vector2(Globals.screenWidth / 2 - 20, 20), new Vector2(Globals.screenWidth - 55, 30))    //55s
        {
            color = new Color(50, 50, 50, 80);
            text = new EText(Globals.SystemFont, "", new Vector2(20, 12), new Vector2(0.5f, 0.5f), Color.White);
            cdbtn = new ChangeDirButton();
        }

        public override void Update(Vector2 OFFSET)
        {
            try
            {
                text.txt = (Globals.curDir == "") ? Globals.currentNode.path : "processing: " + Globals.curDir;
            }
            catch
            {
                text.txt = "Select Folder";
            }
            if (Globals.GetBoxOverlap((pos - dim / 2) + OFFSET, dim, Globals.mouse.newMousePos, Vector2.Zero) && Globals.curDir == "")
            {
                color = new Color(50, 50, 50, 50);
                if (Globals.mouse.LeftClick())
                {
                    using (var dialog = new FolderBrowserDialog())
                    {
                        if (DialogResult.OK == dialog.ShowDialog())
                        {
                            Globals.currentNode = new DirectoryNode(null, dialog.SelectedPath, false);
                            main.SwitchDirectory(Globals.currentNode);
                        }
                    }
                }
            }
            else { color = new Color(50, 50, 50, 80); }
            pos = new Vector2(Globals.screenWidth / 2 - 40, 20);
            dim = new Vector2(Globals.screenWidth - 100, 30);    //60
            text.Update(OFFSET);
            cdbtn.pos = new Vector2(Globals.screenWidth - 88 + cdbtn.dim.X / 2, pos.Y);
            cdbtn.dim = new Vector2(40, 30);
            if(Globals.curDir == "")
                cdbtn.Update(OFFSET);
            try { cdbtn.node = Globals.currentNode.parent; } catch { }
            base.Update(OFFSET);
        }

        public override void Draw(Vector2 OFFSET)
        {
            Globals.primitives.DrawRect(Vector2.Zero, new Vector2(Globals.screenWidth, 40), new Color(100, 100, 100));
            if (Globals.curAllDir > 0)
            {
                Globals.primitives.DrawRect(new Vector2(pos.X - dim.X / 2, pos.Y - dim.Y / 2) + OFFSET, new Vector2(((float)Globals.curDoneDir / (float)Globals.curAllDir) * dim.X, dim.Y), new Color(200, 100, 100));
            }
            text.Draw(OFFSET);
            cdbtn.Draw(OFFSET);
            base.Draw(OFFSET, color);
        }
    }
}
