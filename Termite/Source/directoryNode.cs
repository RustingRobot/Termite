using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Termite.Engine;

namespace Termite.Source
{
    public class DirectoryNode
    {
        public DirectoryNode parent;
        public List<DirectoryNode> children;
        public long size;   // in bytes
        public string path, name;
        public bool file, locked = false;

        public DirectoryNode(DirectoryNode PARENT, string PATH, bool FILE)
        {
            file = FILE;
            parent = PARENT;
            path = PATH;
            name = path.Substring(path.LastIndexOf("\\") + 1, path.Length - path.LastIndexOf("\\") - 1);
        }

        public DirectoryNode getChild(string path)
        {
            foreach (var child in children)
            {
                if(child.path == path) { return child; }
            }
            return null;
        }

        public List<DirectoryNode> GetParents()
        {
            DirectoryNode tempNode = this;
            List<DirectoryNode> nodeList = new List<DirectoryNode>();
            while (tempNode.parent != Globals.topNode)
            {
                tempNode = tempNode.parent;
                nodeList.Add(tempNode);
            }
            return nodeList;
        }

        public long CalculateSize()
        {
            long b = 0;
            foreach (var child in children)
            {
                b += child.size;
            }
            return b;
        }
    }
}