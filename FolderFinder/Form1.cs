using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderFinder
{
    public partial class Form1 : Form
    {
        private SortedList<string, FolderInfo> folderList = new SortedList<string, FolderInfo>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            updateRootFolder();
        }

        public static void ProcessDirectory(string targetDirectory, TreeNode parentNode)
        {
            try
            {
                // Recurse into subdirectories of this directory.
                string[] subdirectoryEntries = System.IO.Directory.GetDirectories(targetDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                {
                    string newBase = subdirectory.Substring(targetDirectory.Length);
                    if (newBase.StartsWith("\\"))
                    {
                        newBase = newBase.Substring(1);
                    }
                    TreeNode newNode = new TreeNode(newBase);
                    newNode.Tag = subdirectory;
                    parentNode.Nodes.Add(newNode);
                    ProcessDirectory(subdirectory, newNode);
                }
            }
            catch (UnauthorizedAccessException ex)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void ProcessDirectory(string targetDirectory, SortedList<String, FolderInfo> folderList)
        {
            try
            {
                // Recurse into subdirectories of this directory.
                string[] subdirectoryEntries = System.IO.Directory.GetDirectories(targetDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                {
                    folderList.Add(subdirectory, new FolderInfo(subdirectory, targetDirectory));
                    ProcessDirectory(subdirectory, folderList);
                }
            }
            catch (UnauthorizedAccessException ex)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void resetFolderList ()
        {
            string rootFolder = this.folderBrowserDialog1.SelectedPath;
            folderList.Clear();
            folderList.Add(rootFolder, new FolderInfo(rootFolder, null));
            ProcessDirectory(rootFolder, folderList);
        }

        private void resetTreeView()
        {
            this.treeView1.Nodes.Clear();
            this.treeView1.Nodes.Add(this.folderBrowserDialog1.SelectedPath);
            ProcessDirectory(this.folderBrowserDialog1.SelectedPath, this.treeView1.Nodes[0]);
            this.treeView1.Nodes[0].Expand();
        }

        private void loadTreeView(SortedList<string, FolderInfo> masterList)
        {
            treeView1.Nodes.Clear();
            masterList.Keys.ToList<string>().ForEach(delegate (String path)
            {
                FolderInfo folderInfo = new FolderInfo(path, null);
                masterList.TryGetValue(path, out folderInfo);
                if (folderInfo.parent != null)
                {
                    TreeNode newNode = new TreeNode
                    {
                        Tag = folderInfo.path,
                        Text = folderInfo.name
                    };
                    folderInfo.parentNode.Nodes.Add(newNode);

                    FolderInfo[] childFolders = folderList.Values.Cast<FolderInfo>().Where(children => children.parent == path).ToArray();
                    for (int i = 0; i < childFolders.Length; i++)
                    {
                        childFolders[i].parentNode = newNode;
                    }

                }
                else
                {
                    TreeNode rootNode = new TreeNode
                    {
                        Tag = path,
                        Text = path
                    };
                    treeView1.Nodes.Add(rootNode);
                    FolderInfo[] childFolders = folderList.Values.Cast<FolderInfo>().Where(children => children.parent == path).ToArray();
                    for (int i = 0; i < childFolders.Length; i++)
                    {
                        childFolders[i].parentNode = rootNode;
                    }
                }
            });

            this.treeView1.Nodes[0].Expand();
        }

        private void updateRootFolder()
        {
            DialogResult result = this.folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                //resetTreeView();
                resetFolderList();
                loadTreeView(folderList);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateRootFolder();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = treeView1.SelectedNode.Text;
            if (treeView1.SelectedNode.Tag != null)
            {
                path = treeView1.SelectedNode.Tag.ToString();
            }
            Process.Start(@path);
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            // Make sure this is the right button.
            if (e.Button != MouseButtons.Right) return;

            // Select this node.
            TreeNode node_here = treeView1.GetNodeAt(e.X, e.Y);
            treeView1.SelectedNode = node_here;

            // See if we got a node.
            if (node_here == null) return;

            contextMenuStrip1.Show(treeView1, new Point(e.X, e.Y));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //treeView1.Nodes[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
