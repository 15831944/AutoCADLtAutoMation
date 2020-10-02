using Microsoft.Win32;
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
using Teigha.DatabaseServices;
using Teigha.Geometry;
using Teigha.Runtime;

namespace AutoCADLtAutoMation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void 图框填写ToolStripMenuItem_Click(object sender, EventArgs e)
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
                int ver = MyHelper.GetAutoCADDrawingVer(ofd.FileName);
                if (ver == 0)
                {
                    MessageBox.Show("图纸是只读的,请检查文件的属性!!");
                    return;
                }
                if (MyHelper.GetAutoCADDrawingVer(ofd.FileName) > 2018)
                {
                    MessageBox.Show("打开的图纸的版本过高，本软件最高支持AutoCAD2018 保存的文件！");
                    return;
                }
                PanelInformationSetup frm = new PanelInformationSetup();
                frm.CurDocFileName = ofd.FileName;
                frm.Show();
            }
        }

        private void 打印成PdfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var acKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Autodesk\Hardcopy", false);
                List<string> acValues = acKey.GetValueNames().Where(c => c.Contains("AutoCAD") && !c.Contains("AutoCAD LT")).ToList();//找未阉割的的autocad的位置
                string strMess1 = string.Empty;
                if (acValues.Count == 0)
                {
                    acValues = acKey.GetValueNames().Where(c => c.Contains("AutoCAD LT")).ToList();//找lt的autocad的位置
                    strMess1="使用的是LT版本";
                }
                else strMess1 = "使用的是网络版本";

                if (acValues.Count > 0)
                {
                    //Autodesk\AutoCAD\R21.0\ACAD-0001:409 for autocad
                    //Autodesk\\AutoCAD LT\\R21\\ACADLT-E001:409" for autocadlt
                    var acadLtVer = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + acValues[acValues.Count - 1], false).GetValue("ProductName");

                    var autocadLtLocation = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + acValues[acValues.Count - 1], false).GetValue("Location") + "\\accoreconsole.exe";
                    strMess1 += $"===>{acadLtVer}";
                    MessageBox.Show(strMess1);
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
                        int ver = MyHelper.GetAutoCADDrawingVer(ofd.FileName);
                        if (ver == 0)
                        {
                            MessageBox.Show("图纸是只读的,请检查文件的属性!!");
                            return;
                        }
                        if (MyHelper.GetAutoCADDrawingVer(ofd.FileName) > 2018)
                        {
                            MessageBox.Show("打开的图纸的版本过高，本软件最高支持AutoCAD2018 保存的文件！");
                            return;
                        }
                        List<NeedPrinPdfPageInfo> listTitleBlks = new List<NeedPrinPdfPageInfo>();

                        #region//读取图纸信息
                        using (Services svcs = new Services())
                        {
                            Database db = new Database(false, false);
                            db.ReadDwgFile(ofd.FileName, System.IO.FileShare.Read, false, null);
                            using (Transaction trans = db.TransactionManager.StartTransaction())
                            {
                                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                                BlockTableRecord ms = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;

                                //获取全部的块信息
                                int i = 0;
                                foreach (ObjectId oid in ms)
                                {
                                    Entity ent = oid.GetObject(OpenMode.ForRead) as Entity;
                                    if (ent is BlockReference)
                                    {
                                        BlockReference blkref = ent as BlockReference;
                                        if (blkref.IsDynamicBlock)
                                        {
                                            var blk = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;

                                            if (blk.Name == "KFWH_CP_SD_A3_H" || blk.Name == "KFWH_CP_SD_A3_V" || blk.Name == "KFWH_CP_SD_A4")//图框
                                            {
                                                NeedPrinPdfPageInfo pdfinfo = new NeedPrinPdfPageInfo();

                                                pdfinfo.leftbtm_X = blkref.GeometricExtents.MinPoint.X;
                                                pdfinfo.leftbtm_Y = blkref.GeometricExtents.MinPoint.Y;
                                                pdfinfo.RightUp_X = blkref.GeometricExtents.MaxPoint.X;
                                                pdfinfo.RightUp_Y = blkref.GeometricExtents.MaxPoint.Y;

                                                switch (blk.Name)
                                                {
                                                    case "KFWH_CP_SD_A3_H":
                                                        pdfinfo.pageSize = PDFPageSize.ISO_full_bleed_A3_420_x_297_MM;

                                                        break;
                                                    case "KFWH_CP_SD_A3_V":
                                                        pdfinfo.pageSize = PDFPageSize.ISO_full_bleed_A3_297_x_420_MM;

                                                        break;
                                                    case "KFWH_CP_SD_A4":
                                                        pdfinfo.pageSize = PDFPageSize.ISO_full_bleed_A4_297_x_210_MM;

                                                        break;
                                                }

                                                var shtNo = blkref.ObjectId.GetAttributeInBlockReference("SHT");
                                                if (shtNo == string.Empty || shtNo == "") shtNo = blkref.ObjectId.GetAttributeInBlockReference("SH");
                                                if (shtNo == string.Empty || shtNo == "") shtNo = (i + 1).ToString();
                                                pdfinfo.SheetNum = shtNo;
                                                i++;
                                                listTitleBlks.Add(pdfinfo);
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                            db.CloseInput(false);
                        }
                        #endregion

                        #region//打印完成
                        if (listTitleBlks.Count == 0)
                        {
                            MessageBox.Show("图纸内部不含有标准的shop drawing 图框");
                            return;
                        }
                        string fn = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PrintoPdfScript For " + Path.GetFileNameWithoutExtension(ofd.FileName) + "at " + DateTime.Now.ToLongDateString() + ".scr");
                        StreamWriter sw = new StreamWriter(fn);
                        sw.WriteLine("dynmode -3");
                        string strMess = string.Empty;
                        string strA3H = string.Empty;
                        string strA3V = string.Empty;
                        string strA4 = string.Empty;
                        var temp = listTitleBlks.OrderBy(c => c.pageSize).ThenBy(c => c.SheetNum).ToList();
                        for (int i = 0; i < temp.Count; i++)
                        {
                            var item = temp[i];
                            sw.WriteLine("-plot");
                            sw.WriteLine("yes");//  Detailed plot configuration?[Yes / No] < No >: Yes
                            sw.WriteLine("Model");//   Enter a layout name or[?] < Model >: Model
                            sw.WriteLine("DWG To PDF.pc3");//Enter an output device name or[?] < None >: DWG To PDF.pc3
                            switch (item.pageSize)
                            {
                                case PDFPageSize.ISO_full_bleed_A3_420_x_297_MM:
                                    sw.WriteLine("ISO_full_bleed_A3_(420.00_x_297.00_MM)".Replace("_", " "));//图纸尺寸//Enter paper size or[?] < ISO full bleed A3(420.00 x 297.00 MM) >: ISO A3 (420.00 x 297.00 MM)
                                    sw.WriteLine("M");//Enter paper units[Inches / Millimeters] < Millimeters >: M
                                    sw.WriteLine("L"); //Enter drawing orientation[Portrait / Landscape] < Landscape >: L
                                    strA3H += "," + item.SheetNum;
                                    break;
                                case PDFPageSize.ISO_full_bleed_A3_297_x_420_MM:
                                    sw.WriteLine("ISO_full_bleed_A3_(297.00_x_420.00_MM)".Replace("_", " "));//图纸尺寸
                                    sw.WriteLine("M");
                                    sw.WriteLine("P");
                                    strA3V += "," + item.SheetNum;
                                    break;
                                case PDFPageSize.ISO_full_bleed_A4_297_x_210_MM:
                                    sw.WriteLine("ISO_full_bleed_A4_(297.00_x_210.00_MM)".Replace("_", " "));//图纸尺寸
                                    sw.WriteLine("M");//millimeters
                                    sw.WriteLine("L");//Landscape
                                    strA4 += "," + item.SheetNum;
                                    break;

                            }
                            sw.WriteLine("No"); //Plot upside down? [Yes/No] <No>: No
                            sw.WriteLine("Window");//Enter plot area[Display / Extents / Limits / View / Window] <Display>: W
                            sw.WriteLine(item.leftbtm_X + "," + item.leftbtm_Y);//Enter lower left corner of window<0.000000,0.000000>: 57890.4548,5664.4887
                            sw.WriteLine(item.RightUp_X + "," + item.RightUp_Y);//Enter upper right corner of window<0.000000,0.000000>: 74050.4544,17344.4885
                            sw.WriteLine("Fit"); //Enter plot scale (Plotted Millimeters = Drawing Units) or[Fit] <Fit>: Fit
                            sw.WriteLine("Center"); // Enter plot offset(x, y) or[Center] <11.55,-13.65>: center
                            sw.WriteLine("Yes"); //Plot with plot styles?[Yes / No] <Yes>: y
                            sw.WriteLine("monochrome.ctb");// Enter plot style table name or[?] (enter. for none) <>: monochrome.ctb
                            sw.WriteLine("Yes"); //Plot with lineweights? [Yes/No] <Yes>: Yes
                            sw.WriteLine("As");//Enter shade plot setting[As displayed / legacy Wireframe / legacy Hidden / Visual styles / Rendered] <As displayed>: As
                            sw.WriteLine(ofd.FileName.Replace(".dwg", "-" + item.SheetNum + ".pdf")); //Enter file name<D:\MyDeskTop\B379-53-006-Shop Drawing-ALT.1--nans-Model.pdf>: c:\1.pdf
                            sw.WriteLine("Yes");  //Save changes to page setup [Yes/No]? <N> Yes
                            sw.WriteLine("yes"); //Proceed with plot [Yes/No] <Y>: Yes
                        }
                        sw.WriteLine("filedia");
                        sw.WriteLine("1");
                        sw.WriteLine(" ");
                        sw.Close();
                        strMess = $"A3横向图框， sht：{strA3H}\n A3竖向图框， sht：{strA3V} \n A4横向图框， sht：{strA4}";
                        #endregion
                        if (autocadLtLocation.Contains("LT"))
                        {
                            CMDHelper.ExecuteScr(fn, ofd.FileName, Path.GetDirectoryName(autocadLtLocation));
                            MyHelper.MyOpenFolder(Path.GetDirectoryName(ofd.FileName), Path.GetFileName(ofd.FileName));
                        }
                        else
                        {
                            if (MessageBox.Show($"关闭程序以完成打印! \n {strMess}", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                            {
                                CMDHelper.ExecuteCmd(fn, ofd.FileName, autocadLtLocation);
                                MyHelper.MyOpenFolder(Path.GetDirectoryName(ofd.FileName), Path.GetFileName(ofd.FileName));
                                this.Close();
                            }
                        }
                    }
                }
                else MessageBox.Show($" 检测到你的电脑上未安装 AutoCAD LT/AutoCAD 版本，无法加载插件！！！");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($" 检测到你的电脑上未安装 AutoCAD，无法加载插件！！！");
            }
        }

        private void 页码填写ToolStripMenuItem_Click(object sender, EventArgs e)
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
                int ver = MyHelper.GetAutoCADDrawingVer(ofd.FileName);
                if (ver == 0)
                {
                    MessageBox.Show("图纸是只读的,请检查文件的属性!!");
                    return;
                }
                if (MyHelper.GetAutoCADDrawingVer(ofd.FileName) > 2018)
                {
                    MessageBox.Show("打开的图纸的版本过高，本软件最高支持AutoCAD2018 保存的文件！");
                    return;
                }
                using (Services svcs = new Services())
                {
                    Database db = new Database(false, true);
                    db.ReadDwgFile(ofd.FileName, System.IO.FileShare.Read, false, null);
                    int panelPartsQty = 0;//零件数量
                    using (Transaction trans = db.TransactionManager.StartTransaction())
                    {
                        BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord ms = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
                        List<BlockReference> listTitleBlks = new List<BlockReference>();
                        List<BlockReference> listProfilePartBlks = new List<BlockReference>();
                        List<BlockReference> listProfileDetailsPartBlks = new List<BlockReference>();
                        #region    //获取型材的数量
                        foreach (ObjectId oid in ms)
                        {
                            Entity ent = oid.GetObject(OpenMode.ForRead) as Entity;
                            if (ent is BlockReference)
                            {
                                BlockReference blkref = ent as BlockReference;
                                if (blkref.IsDynamicBlock)
                                {
                                    var blk = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
                                    if (blk.Name == "KFWHCPSD_ProfileInforTable")//型材details
                                    {
                                        listProfileDetailsPartBlks.Add(blkref);
                                        panelPartsQty++;
                                        continue;
                                    }
                                    if (blk.Name == "KFWH_CP_SD_A3_H" || blk.Name == "KFWH_CP_SD_A3_V" || blk.Name == "KFWH_CP_SD_A4")//图框
                                    {
                                        listTitleBlks.Add(blkref);
                                        continue;
                                    }
                                    if (blk.Name == "KFWH_CP_SD_PN_ProfilePart")//型材下料
                                    {
                                        listProfilePartBlks.Add(blkref);
                                        continue;
                                    }
                                }
                            }
                            if (ent is DBText && ent.Color.ColorIndex == 35) { panelPartsQty++; continue; }
                        }
                        #endregion

                        if (listTitleBlks.Count > 0)
                        {
                            //找到第一个图框
                            var sortTitleBlks = listTitleBlks.OrderByDescending(c => c.Position.Y).ThenBy(c => c.Position.X).ToList();
                            Dictionary<string, ObjectIdCollection> dicProfilePartsNestingPage = new Dictionary<string, ObjectIdCollection>();
                            Dictionary<ObjectId, List<string>> dicProfiles = new Dictionary<ObjectId, List<string>>();
                            #region//获取所有型材对应的页码的objectid
                            foreach (BlockReference item in sortTitleBlks)
                            {
                                if (item.IsDynamicBlock)
                                {
                                    var blk = item.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
                                    if (blk.Name == "KFWH_CP_SD_A4")
                                    {
                                        foreach (BlockReference blkref in listProfilePartBlks)
                                        {
                                            if (blkref.BlockIsInsideBlock(item))
                                            {
                                                var blk1 = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
                                                //
                                                List<string> strTemp = new List<string>();
                                                if (blk1.Name == "KFWH_CP_SD_PN_ProfilePart")
                                                {
                                                    var piecemark = blkref.ObjectId.GetAttributeInBlockReference("PIECE_MARK");
                                                    if (piecemark != string.Empty)
                                                    {
                                                        strTemp.Add(piecemark.Split(new string[] { " ", ",", ";" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                                                    }
                                                    else
                                                    {
                                                        Line l = new Line(Point3d.Origin, blkref.Position);
                                                        db.AddToModelSpace(l);
                                                    }
                                                    ObjectId oid = item.ObjectId.GetAttributeObjectIdInBlockReference("SHT");
                                                    if (dicProfiles.ContainsKey(oid))
                                                    {
                                                        var temp = dicProfiles[oid];
                                                        temp.AddRange(strTemp.ToArray());
                                                    }
                                                    else dicProfiles[oid] = strTemp;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //安零件名称统计套料页码！
                            foreach (KeyValuePair<ObjectId, List<string>> item in dicProfiles)
                            {
                                foreach (var strPartName in item.Value)
                                {
                                    if (dicProfilePartsNestingPage.ContainsKey(strPartName))
                                    {
                                        var temp = dicProfilePartsNestingPage[strPartName];
                                        if (!temp.Contains(item.Key)) temp.Add(item.Key);
                                        dicProfilePartsNestingPage[strPartName] = temp;
                                    }
                                    else dicProfilePartsNestingPage[strPartName] = new ObjectIdCollection(new ObjectId[] { item.Key });
                                }
                            }
                            #endregion

                            //获取KFWHCPSD_ProfileInforTable
                            #region//写入页码的字段表达式

                            foreach (BlockReference item in sortTitleBlks)
                            {
                                if (item.IsDynamicBlock)
                                {
                                    var blk = item.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
                                    if (blk.Name == "KFWH_CP_SD_A4")
                                    {
                                        foreach (BlockReference blkref in listProfileDetailsPartBlks)
                                        {
                                            if (blkref.BlockIsInsideBlock(item))
                                            {
                                                var blk2 = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
                                                if (blk2.Name == "KFWH_CP_SD_ProfileInforTable")//KFWH_CP_SD_ProfileInforTable
                                                {
                                                    Dictionary<string, string> dicNestingPageNo = new Dictionary<string, string>();
                                                    string strProfileName = blkref.ObjectId.GetAttributeInBlockReference("PARTNAME");
                                                    if (dicProfilePartsNestingPage.ContainsKey(strProfileName))
                                                    {
                                                        var oids = dicProfilePartsNestingPage[strProfileName];
                                                        for (int i = 0; i < oids.Count; i++)
                                                        {
                                                            if (i > 0)
                                                            {
                                                                //$"%<\\AcObjProp Object(%<\\_ObjId {oidProBlkref.GetAttributeObjectIdInBlockReference("GRADE").OldIdPtr.ToInt64()} >%).TextString >%"
                                                                dicNestingPageNo["SHTNO"] += $", %<\\AcObjProp Object(%<\\_ObjId {oids[i].OldIdPtr.ToInt64()}>%).TextString>%";
                                                            }
                                                            else dicNestingPageNo["SHTNO"] = $"%<\\AcObjProp Object(%<\\_ObjId {oids[i].OldIdPtr.ToInt64()}>%).TextString>%";
                                                        }
                                                        blkref.ObjectId.UpdateAttributesInBlock(dicNestingPageNo);
                                                    }
                                                    else MessageBox.Show($"{strProfileName} 未做型材套料图！");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                            // 填写页码。。。。。。
                            int startPageNo = (int)Math.Ceiling(panelPartsQty / 13.0) + 4;
                            for (int i = 0; i < sortTitleBlks.Count; i++)
                            {
                                Dictionary<string, string> dicShpageTotal = new Dictionary<string, string>();
                                //A4+A3_V
                                dicShpageTotal["SHT"] = (startPageNo + i).ToString();
                                dicShpageTotal["TOTAL"] = (startPageNo + sortTitleBlks.Count - 1).ToString();
                                //A3_H
                                dicShpageTotal["SH"] = (startPageNo + i).ToString();
                                dicShpageTotal["PAGE"] = (startPageNo + sortTitleBlks.Count - 1).ToString();
                                sortTitleBlks[i].ObjectId.UpdateAttributesInBlock(dicShpageTotal);
                                dicShpageTotal.Clear();
                            }
                            MessageBox.Show($"共完成 {sortTitleBlks.Count} 个图框的页码填写！");
                        }
                        trans.Commit();
                    }
                    db.Save();
                    db.CloseInput(true);
                    MyHelper.MyOpenFolder(Path.GetDirectoryName(ofd.FileName), Path.GetFileName(ofd.FileName));
                }
            }
        }

        private void accoreConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmAccoreConsole().Show();
        }
    }

    public struct NeedPrinPdfPageInfo
    {
        public double leftbtm_X { get; set; }
        public double leftbtm_Y { get; set; }
        public double RightUp_X { get; set; }
        public double RightUp_Y { get; set; }
        public PDFPageSize pageSize { get; set; }
        public string SheetNum { get; set; }

    }
    public enum PDFPageSize
    {
        ISO_full_bleed_A3_420_x_297_MM,
        ISO_full_bleed_A3_297_x_420_MM,
        ISO_full_bleed_A4_297_x_210_MM
    }

}
