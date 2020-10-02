using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoCADLtAutoMation
{
    public partial class frmAccoreConsole : Form
    {
        public string CurAutoCADVer { get; set; }
        public string CurAutoCADAccoreconsole { get; set; }
        public List<string[]> CurPCAcadinfo { get; set; }

        public frmAccoreConsole()
        {
            InitializeComponent();
        }



        private void btn_ScrExecute_Click(object sender, EventArgs e)
        {
         
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            this.comboBox1.Items.Clear();
            this.CurPCAcadinfo=MyHelper.GetCurPCAutoCADs();
            foreach (var item in this.CurPCAcadinfo)
            {
                this.comboBox1.Items.Add(item[0]);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurAutoCADVer = this.comboBox1.Text;
            this.CurAutoCADAccoreconsole = this.CurPCAcadinfo.First(c => c[0] == this.CurAutoCADVer)[1];
        }

        private void btn_scrSelectFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Dwg文件(*.dwg)|*.dwg",
                RestoreDirectory = true,
                FilterIndex = 1,
                Title = "选取一张图纸"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (MyHelper.GetAutoCADDrawingVer(ofd.FileName) > 2018)
                {
                    MessageBox.Show("打开的图纸的版本过高，本软件最高支持AutoCAD2018 保存的文件！");
                    return;
                }

                if (MyHelper.FileInUse(ofd.FileName))
                {
                    MessageBox.Show("请保证该文件不是只读的！");
                    return;
                }
                this.textBox1.Text = ofd.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "scr文件(*.scr)|*.scr",
                RestoreDirectory = true,
                InitialDirectory=Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments),
                FilterIndex = 1,
                Title = "选取一个scr文件"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = ofd.FileName;
            }
        }

        private void btn_ScrExecute_Click_1(object sender, EventArgs e)
        {
            if (this.comboBox1.Text.Contains("LT"))
            {
                CMDHelper.ExecuteScr(this.textBox2.Text, this.textBox1.Text, Path.GetDirectoryName(this.CurAutoCADAccoreconsole));
                MyHelper.MyOpenFolder(Path.GetDirectoryName(this.textBox1.Text), Path.GetFileName(this.textBox1.Text));
            }
            else
            {
                CMDHelper.ExecuteCmd(this.textBox2.Text, this.textBox1.Text, this.CurAutoCADAccoreconsole);
                MessageBox.Show("关闭本程序,完成accoreconsole 命令的执行");
                MyHelper.MyOpenFolder(Path.GetDirectoryName(this.textBox1.Text), Path.GetFileName(this.textBox1.Text));
                Thread.CurrentThread.Abort();
            }
            
        }
    }
}
