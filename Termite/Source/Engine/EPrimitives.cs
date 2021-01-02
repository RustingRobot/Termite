﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Termite.Engine
{
    public class EPrimitives
    {
        private Texture2D square;

        public EPrimitives()
        {
            square = Globals.content.Load<Texture2D>("Square");
        }

        public void DrawLine(Vector2 P1, Vector2 P2,int SIZE, Color COLOR)    //this must be called inside of a SpriteBatch!
        {
            float angle = (float)Math.Atan2(P1.Y - P2.Y, P1.X - P2.X);
            float dist = Vector2.Distance(P1, P2);
            Globals.spriteBatch.Draw(square,new Rectangle((int)P2.X, (int)P2.Y, (int)dist, SIZE), null, COLOR, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public void DrawRect(Vector2 POS, Vector2 DIM, Color COLOR)    //this must be called inside of a SpriteBatch!
        {
            Globals.spriteBatch.Draw(square,new Rectangle((int)POS.X, (int)POS.Y, (int)DIM.X, (int)DIM.Y),COLOR);
        }
    }
}