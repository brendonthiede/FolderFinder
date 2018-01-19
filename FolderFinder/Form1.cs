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
        private List<string> folderList = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            updateRootFolder();
        }

        public static void ProcessDirectory(string targetDirectory, List<String> folderList)
        {
            try
            {
                // Recurse into subdirectories of this directory.
                string[] subdirectoryEntries = System.IO.Directory.GetDirectories(targetDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                {
                    folderList.Add(subdirectory);
                    ProcessDirectory(subdirectory, folderList);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        private void resetFolderList ()
        {
            string rootFolder = this.folderBrowserDialog1.SelectedPath;
            folderList.Clear();
            folderList.Add(rootFolder);
            ProcessDirectory(rootFolder, folderList);
        }

        private void loadTreeView(string[] masterList)
        {
            listBox1.Items.Clear();
            for (int i = 0; i < masterList.Length; i++)
            {
                listBox1.Items.Add(masterList[i]);
            }
        }

        private void updateRootFolder()
        {
            DialogResult result = this.folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                resetFolderList();
                loadTreeView(folderList.ToArray());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateRootFolder();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Process.Start(@listBox1.SelectedItem.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filter = textBox1.Text;
            string[] filtered = folderList.Cast<string>().Where(key => key.ToLowerInvariant().Contains(filter.ToLowerInvariant())).ToArray();
            loadTreeView(filtered);
        }
    }
}
