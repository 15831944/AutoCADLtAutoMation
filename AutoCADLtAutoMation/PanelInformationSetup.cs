using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Reflection;
using Teigha.DatabaseServices;
using Teigha.Runtime;
using System.IO;


namespace AutoCADLtAutoMation
{
    public partial class PanelInformationSetup : Form
    {
        public PanelInfor CurPanel { get; set; }

        public string CurDocFileName { get; set; }

        public PanelInformationSetup()
        {
            InitializeComponent();
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.AddRange(MySQLHelper.GetAllProjectName().ToArray());
        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            this.comboBox2.Items.Clear();
            if (this.comboBox1.SelectedIndex == -1)
            {
                this.comboBox1.Focus();
            }
            else this.comboBox2.Items.AddRange(MySQLHelper.GetCurProjectBlks(this.comboBox1.Text).ToArray());
        }

        private void comboBox3_DropDown(object sender, EventArgs e)
        {
            this.comboBox3.Items.Clear();
            if (this.comboBox1.SelectedIndex == -1 || this.comboBox2.SelectedIndex == -1)
            {
                if (this.comboBox1.SelectedIndex == -1) this.comboBox1.Focus(); else this.comboBox2.Focus();
            }
            else this.comboBox3.Items.AddRange(MySQLHelper.GetCurProjectBlkPanels(this.comboBox1.Text, this.comboBox2.Text).ToArray());
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurPanel = MySQLHelper.GetPanelTitleBlockInfo(this.comboBox1.Text, this.comboBox2.Text, this.comboBox3.Text);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.textBox1.Text) && !string.IsNullOrWhiteSpace(this.textBox4.Text))
            {
                this.CurPanel.DrawingNo = this.textBox1.Text;
                this.CurPanel.Date = String.Format("{0:00}", DateTime.Now.Day) + "/" + String.Format("{0:00}", DateTime.Now.Month) + "/" + String.Format("{0:0000}", DateTime.Now.Year);
                this.CurPanel.Alt = this.textBox4.Text;
                using (Services svcs = new Services())
                {
                    Database db = new Database(false, true);
                    db.ReadDwgFile(this.CurDocFileName, System.IO.FileShare.Read, false, null);
                    using (Transaction trans = db.TransactionManager.StartTransaction())
                    {
                        List<string> properties = typeof(PanelInfor).GetProperties().Select(p => p.Name).ToList();
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        foreach (var item in properties) dic.Add(item, typeof(PanelInfor).GetProperty(item).GetValue(this.CurPanel, null).ToString());
                        if (db.HasSummaryInfo())
                        {
                            var info = new DatabaseSummaryInfoBuilder();
                            foreach (KeyValuePair<string, string> item in dic) info.CustomPropertyTable.Add(item.Key, item.Value);
                            db.SummaryInfo = info.ToDatabaseSummaryInfo();
                        }
                        else AddCustomInfo(dic, db);
                        trans.Commit();
                    }
                    //var newfn = this.CurDocFileName.Replace(".dwg", "-" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".dwg");
                    db.Save();
                    db.CloseInput(true);
                    MyHelper.MyOpenFolder(Path.GetDirectoryName(this.CurDocFileName), Path.GetFileName(this.CurDocFileName));
                }
                MessageBox.Show("Panel Inforamtion Update Complate!");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(this.textBox1.Text)) { this.textBox1.Focus(); MessageBox.Show("please input correct drawing Number"); }
                if (!string.IsNullOrWhiteSpace(this.textBox4.Text)) { this.textBox4.Focus(); MessageBox.Show("Please Input Hull Drawing Alteration"); }
            }

        }
        /// <summary>
        /// 向图纸中添加图纸属性
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="db">图形数据</param>
        private void AddCustomInfo(Dictionary<string, string> dic, Database db)
        {
            if (db.HasSummaryInfo()) return;
            var info = new DatabaseSummaryInfoBuilder(db.SummaryInfo);
            foreach (KeyValuePair<string, string> item in dic) info.CustomPropertyTable.Add(item.Key, item.Value);
            db.SummaryInfo = info.ToDatabaseSummaryInfo();
        }

        private void PanelInformationSetup_Load(object sender, EventArgs e)
        {

        }
    }
}
