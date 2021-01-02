using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Termite.Engine;

namespace Termite.Source
{
    class Button: ESprite2d
    {
        public bool delete, unfolded = false;
        public Color color;
        public EText text;
        public int ID, Indent = 0, rightBorder = 40;
        public EText sizeTxt, percentTxt;
        public string path, name, sizeUnit;
        public float size,persentage;
        public DirectoryNode node;


        public Button(string PATH, DirectoryNode NODE) : base("Square", Vector2.Zero, Vector2.Zero)
        {
            path = PATH;
            node = NODE;
            name = path.Substring(path.LastIndexOf("\\") + 1, path.Length - 1 - path.LastIndexOf("\\"));
            ID = Globals.nrOfButtons;
            Globals.nrOfButtons++;
            color = new Color(50, 50, 50, 80);
            text = new EText(Globals.SystemFont, name, new Vector2(20, 92), new Vector2(0.5f, 0.5f), Color.White);
            size = node.size;
            if (size > 1073741824 * 0.5)
            {
                size = size / 1073741824;
                sizeUnit = " GB";
            }
            else if (size > 1048576 * 0.5)
            {
                size = size / 1048576;
                sizeUnit = " MB";
            }
            else if (size > 1024 * 0.5)
            {
                size = size / 1024;
                sizeUnit = " KB";
            }
            else
            {
                sizeUnit = " B";
            }
            string sizeStr = size.ToString();
            try
            {
                sizeTxt = new EText(Globals.SystemFont, sizeStr.Substring(0, sizeStr.LastIndexOf(".") + 3) + sizeUnit, new Vector2(650, 92), new Vector2(0.5f, 0.5f), Color.White);
            }
            catch
            {
                sizeTxt = new EText(Globals.SystemFont, sizeStr + sizeUnit, new Vector2(650, 92), new Vector2(0.5f, 0.5f), Color.White);
            }
            persentage = (float)node.size / (float)Globals.currentNode.size * 100;
            string persentStr = persentage.ToString();
            if (persentStr.Length > 4)
            {
                persentStr = persentStr.Substring(0, persentStr.LastIndexOf(".") + 3) + " %";
            }
            else
            {
                persentStr = persentStr + " %";
            }
            percentTxt = new EText(Globals.SystemFont, persentStr, new Vector2(550, 92), new Vector2(0.5f, 0.5f), Color.White);
        }

        public override void Update(Vector2 OFFSET)
        {
            pos = new Vector2(Globals.screenWidth / 2 + Indent * 15 - rightBorder, 100 + ID * 32);
            text.pos = new Vector2(20 + Indent * 30, 92 + ID * 32);
            dim = new Vector2(Globals.screenWidth - 20 - rightBorder * 2 - Indent * 30, 30);
            sizeTxt.pos.Y = 92 + ID * 32;
            sizeTxt.pos.X = Globals.screenWidth - 150 - rightBorder;
            percentTxt.pos.Y = 92 + ID * 32;
            percentTxt.pos.X = Globals.screenWidth - 250 - rightBorder;

            sizeTxt.Update(OFFSET);
            text.Update(OFFSET);
            percentTxt.Update(OFFSET);
            base.Update(OFFSET);
        }

        public override void Draw(Vector2 OFFSET)
        {
            Globals.primitives.DrawRect(new Vector2(pos.X - dim.X / 2, pos.Y - dim.Y / 2) + OFFSET, new Vector2((persentage / 100) * dim.X,dim.Y), new Color(200,100,100));
            if (!node.locked)
            {
                sizeTxt.Draw(OFFSET);
                percentTxt.Draw(OFFSET);
            }
            text.Draw(OFFSET);
            base.Draw(OFFSET,color);
        }
    }
}
