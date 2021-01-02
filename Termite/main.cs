using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Termite.Engine;
using Termite.Source;

using Path = Pri.LongPath.Path;
using Directory = Pri.LongPath.Directory;
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
using File = Pri.LongPath.File;
using Pri.LongPath;
using System.Threading.Tasks;

namespace Termite
{
    public class main : Game
    {
        GraphicsDeviceManager graphics;
        static List<Button> buttons = new List<Button>();
        static List<DirectoryNode> nodeTree = new List<DirectoryNode>();

        OpenFolderButton fb;
        public static ScrollBar sb;

        public main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
            Globals.screenWidth = 800;
            Globals.screenHeight = 500;
            graphics.PreferredBackBufferWidth = (int)Globals.screenWidth;
            graphics.PreferredBackBufferHeight = (int)Globals.screenHeight;
            graphics.ApplyChanges();
            base.Initialize();
            fb = new OpenFolderButton();
            sb = new ScrollBar();
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Globals.screenWidth = Window.ClientBounds.Width;
            Globals.screenHeight = Window.ClientBounds.Height;
            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.content = Content;
            Globals.SystemFont = "Roboto";
            Globals.PassButton = AddButton;
            Globals.keyboard = new EKeyboard();
            Globals.mouse = new EMouseControl();
            Globals.primitives = new EPrimitives();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Globals.scrollMax = -Globals.nrOfButtons * 32;
            Globals.offset.Y += Globals.mouse.ScrollChange() * 0.2f;
            if (Globals.offset.Y > 0) Globals.offset.Y = 0;
            if (Globals.offset.Y < Globals.scrollMax) Globals.offset.Y = Globals.scrollMax;
            if (Globals.keyboard.GetPress("A")) { SwitchDirectory(Globals.currentNode.parent); }
            Globals.gameTime = gameTime;
            Globals.keyboard.Update();
            Globals.mouse.Update();
            fb.Update(Vector2.Zero);
            sb.Update(Vector2.Zero);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Update(Globals.offset);
                if (buttons[i].delete)
                {
                    buttons.RemoveAt(i);
                    i--;
                }
            }
            Globals.keyboard.LateUpdate();
            Globals.mouse.LateUpdate();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            Globals.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Draw(Globals.offset);
            }
            fb.Draw(Vector2.Zero);
            sb.Draw(Vector2.Zero);
            Globals.spriteBatch.End();
            base.Draw(gameTime);
        }

        public virtual void AddButton(object INFO)
        {
            buttons.Add((Button)INFO);
        }

        static async Task GenerateNodeTreeAsync(string dir, DirectoryNode parent)
        {
            Globals.curAllDir = 0;
            Globals.curDoneDir = 0;
            Globals.curDir = "";
            await Task.Run(() => GenerateNodeTree(dir, parent));
            Globals.curAllDir = 0;
            Globals.curDoneDir = 0;
            Globals.curDir = "";
        }

        static DirectoryNode GenerateNodeTree(string dir, DirectoryNode parent)
        {
            Globals.curDir = dir;
            DirectoryNode newNode = new DirectoryNode(parent, dir, false);
            try
            {
                string[] files = Directory.GetFiles(dir, "*.*");
                long size = 0;
                List<DirectoryNode> children = new List<DirectoryNode>();
                foreach (string name in files)
                {
                    FileInfo info = new FileInfo(name);
                    size += info.Length;
                    DirectoryNode tempFileNode = new DirectoryNode(newNode, name, true) { size = info.Length };
                    nodeTree.Add(tempFileNode);
                    children.Add(tempFileNode);
                }
                string[] c = Directory.GetDirectories(dir, "*.*");
                Globals.curAllDir += c.Length;
                foreach (string name in c)
                {
                    DirectoryNode tempNode = GenerateNodeTree(name, newNode);
                    size += tempNode.size;
                    children.Add(tempNode);
                }
                newNode.size = size;
                newNode.children = children;
                nodeTree.Add(newNode);
                Globals.curDoneDir++;
            }
            catch
            {
                newNode.size = 0;
                newNode.locked = true;
            }
            return newNode;
        }

        public async static void SwitchDirectory(DirectoryNode dir)
        {
            Globals.nrOfButtons = 0;
            foreach (var button in buttons) { button.delete = true; }
            DirectoryNode tempDirectory = null;
            foreach(var node in nodeTree) { if(node.path == dir.path) { tempDirectory = node; } }    //replace with hash table?
            if(tempDirectory == null)
            {
                nodeTree = new List<DirectoryNode>();
                Globals.topNode = new DirectoryNode(null, dir.path, false);
                await GenerateNodeTreeAsync(dir.path, Globals.topNode);   //async?
                SwitchDirectory(dir);
                return;
            }
            Globals.currentNode = tempDirectory;
            foreach(var child in tempDirectory.children)
            {
                if(!child.file) buttons.Add(new DirectoryButton(child.path, child));
            }
            string[] files = Directory.GetFiles(dir.path, "*.*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                buttons.Add(new Button(file, tempDirectory.getChild(file)));
            }
            Globals.scrollMax = -Globals.nrOfButtons * 32;
            sb.swPos = 40;
            Globals.offset.Y = 0;
        }

        public static void OpenDirectory(DirectoryNode dir)
        {
            Button tempButton = null;
            foreach(var button in buttons) { if (button.node.path == dir.path) { tempButton = button; } }
            for (int i = tempButton.ID + 1; i < buttons.Count; i++)
            {
                buttons[i].ID += tempButton.node.children.Count;
            }
            for (int i = 0; i < tempButton.node.children.Count; i++)
            {
                Button tempChild = (tempButton.node.children[i].file) ? new Button(tempButton.node.children[i].path, tempButton.node.children[i]) : new DirectoryButton(tempButton.node.children[i].path, tempButton.node.children[i]);
                tempChild.ID = i + tempButton.ID + 1;
                tempChild.Indent = tempButton.Indent + 1;
                buttons.Insert(tempChild.ID, tempChild);
            }
        }

        public static void CloseDirectory(DirectoryNode dir)
        {
            Button tempButton = null;
            foreach (var button in buttons) { if (button.node.path == dir.path) { tempButton = button; } }
            if (tempButton.node.children.Count == 0) { return; }
            int m = tempButton.ID + 1;
            while (!tempButton.node.GetParents().Contains(buttons[m].node.parent))
            {
                buttons[m].unfolded = false;
                buttons[m].delete = true;
                Globals.nrOfButtons--;
                m++;
                if(buttons.Count == m) { break; }
            }
            for (int i = m; i < buttons.Count; i++)
            {
                buttons[i].ID -= m - (tempButton.ID + 1);
            }
        }
    }

#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new main())
                game.Run();
        }
    }
#endif
}
