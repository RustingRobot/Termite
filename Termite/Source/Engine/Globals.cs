#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Termite.Source;
#endregion

namespace Termite.Engine
{
    public delegate void PassObject(object i);
    public delegate object PassObjectAndReturn(object i);

    public class Globals
    {
        public static float screenHeight, screenWidth;
        public static PassObject PassButton;
        public static string SystemFont, curDir = "";
        public static int nrOfButtons, scrollMax, curAllDir, curDoneDir;
        public static DirectoryNode currentNode, topNode;
        public static Vector2 offset;

        public static ContentManager content;
        public static SpriteBatch spriteBatch;
        public static EPrimitives primitives;

        public static EKeyboard keyboard;
        public static EMouseControl mouse;

        public static GameTime gameTime;

        public static float GetDistance(Vector2 pos, Vector2 target)
        {
            return (float)Math.Sqrt(Math.Pow(pos.X - target.X, 2) + Math.Pow(pos.Y - target.Y, 2));
        }

        public static bool GetBoxOverlap(Vector2 P1, Vector2 D1, Vector2 P2, Vector2 D2)
        {
            if (P1.X < P2.X + D2.X &&
               P1.X + D1.X > P2.X &&
               P1.Y < P2.Y + D2.Y &&
               P1.Y + D1.Y > P2.Y)
            {
                return true;
            }
            return false;
        }
    }
}
