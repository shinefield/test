using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileWatcher
{
    public partial class Form1 : Form
    {
        Thread thread;

        public Form1()
        {
            InitializeComponent();
        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            string content = (e.FullPath + " 被更改");
            new Thread(new ParameterizedThreadStart(Record)).Start(content);
        }

        private void fileSystemWatcher1_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            string content = (e.FullPath + " 被创建");
            new Thread(new ParameterizedThreadStart(Record)).Start(content);
        }

        private void fileSystemWatcher1_Deleted(object sender, System.IO.FileSystemEventArgs e)
        {
            string content = (e.FullPath + " 被删除");
            new Thread(new ParameterizedThreadStart(Record)).Start(content);
        }

        private void fileSystemWatcher1_Renamed(object sender, System.IO.RenamedEventArgs e)
        {
            string content = (e.OldFullPath + " 重命名为 " + e.FullPath);
            new Thread(new ParameterizedThreadStart(Record)).Start(content);
        }

        private void Record(object content)
        {
            Log(content.ToString());
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore.exe", "http://www.shinefield.cn");
        }

        private void textBoxLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                textBoxLog.SelectAll();
            }
        }


        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBoxPath.Text = folderBrowserDialog1.SelectedPath;
            folderBrowserDialog1.SelectedPath = textBoxPath.Text;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBoxPath.Text))
            {
                Log("路径错误，请选择正确路径！");
                return;
            }
            string path = textBoxPath.Text;
            string type = textBoxType.Text;
            fileSystemWatcher1.EnableRaisingEvents = true;
            fileSystemWatcher1.Path = path;
            fileSystemWatcher1.IncludeSubdirectories = true;
            fileSystemWatcher1.Filter = type;
            fileSystemWatcher1.EndInit();
            SetControlEnabled(false);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            fileSystemWatcher1.EnableRaisingEvents = false;
            SetControlEnabled(true);
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {
            textBoxLog.Text = string.Empty;
        }


        private delegate void SetTextBoxTextDelegate(string text, TextBox textBox);
        private void SetTextBoxText(string text,TextBox textBox)
        {
            if (textBox.InvokeRequired)
            {
                Invoke(new SetTextBoxTextDelegate(SetTextBoxText), new object[] { text, textBox });
            }
            else
            {
                textBox.Text = text;
            }
        }

        private delegate void SetControlEnabledDelegte(bool b, Control control);
        private void SetControlEnabled(bool b, Control control)
        {
            if (control.InvokeRequired)
            {
                Invoke(new SetControlEnabledDelegte(SetControlEnabled), new object[] { b, control });
            }
            else
            {
                control.Enabled = b;
            }
        }

        private void SetControlEnabled(bool b)
        {
            SetControlEnabled(b, textBoxPath);
            SetControlEnabled(b, buttonBrowse);
            SetControlEnabled(b, textBoxType);
            SetControlEnabled(b, buttonStart);
        }

        private void Log(string str)
        {
            SetTextBoxText(" 【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】" + str + System.Environment.NewLine + textBoxLog.Text, textBoxLog);
        }

        
    }
}
