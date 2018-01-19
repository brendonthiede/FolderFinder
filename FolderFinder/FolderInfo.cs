using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderFinder
{
    public class FolderInfo
    {
        public string path;
        public string name;
        public string parent;
        public System.Windows.Forms.TreeNode parentNode;

        private char[] trimChars = { '\\' };

        public FolderInfo(string path, string parent)
        {
            this.path = path;
            this.parent = parent;
            this.name = path.TrimEnd(trimChars);
            try
            {
                this.name = System.IO.Path.GetFileName(this.name);
            }
            catch
            {
            }
        }
    }
}
