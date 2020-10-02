using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace AutoCADLtAutoMation
{
    //public class 图框信息和打印到pdf
    //{
    //    public Document acDoc = Application.DocumentManager.MdiActiveDocument;
    //    public Database acDb = Application.DocumentManager.MdiActiveDocument.Database;
    //    public Editor acEd = Application.DocumentManager.MdiActiveDocument.Editor;

    //    [myAutoCADcmd("Update shop drawing title blcok information as per panel what you select", "SD_PanelInfo.png")]
    //    [CommandMethod("SD_PanelInfo")]
    //    public void KFWHSD_设置Panel信息()
    //    {
    //        PanelInformationSetup frm = new PanelInformationSetup();
    //        Application.ShowModelessDialog(frm);
    //    }
    //    [myAutoCADcmd("Update shop drawing title blcok information as per panel what you select", "SD_AddPageNo.png")]
    //    [CommandMethod("SD_AddPageNo")]
    //    public void KFWHSD_填图框页码()
    //    {

    //        if (Application.GetSystemVariable("WORLDUCS").ToString() != "1") { this.acEd.CurrentUserCoordinateSystem = Matrix3d.Identity; this.acEd.Regen(); }
    //        //设置颜色样式
    //        if (Application.GetSystemVariable("CECOLOR").ToString() != "BYLAYER") Application.SetSystemVariable("CECOLOR", "BYLAYER");
    //        Point3dCollection pnts = new Point3dCollection();
    //        PromptPointOptions ppo = new PromptPointOptions("选取需要填写图框页码的区域的左下角:\n");
    //        int startPageNo = 0;
    //        var ppr = this.acEd.GetPoint(ppo);
    //        if (ppr.Status == PromptStatus.OK)
    //        {
    //            var ppr1 = this.acEd.GetCorner("选取需要填写图框页码的区域的右上角：\n", ppr.Value);
    //            pnts.Add(ppr.Value);
    //            if (ppr1.Status == PromptStatus.OK) pnts.Add(ppr1.Value);
    //        }
    //        if (pnts.Count != 2) return;
    //        using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //        {
    //            BlockTable bt = trans.GetObject(acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
    //            BlockTableRecord ms = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
    //            List<BlockReference> listTitleBlks = new List<BlockReference>();
    //            TypedValueList list = new TypedValueList();
    //            list.Add(typeof(BlockReference));
    //            var psr = this.acEd.SelectWindow(pnts[0], pnts[1], new SelectionFilter(list));
    //            #region// 获取零件的总数量
    //            int panelPartsQty = 0;
    //            TypedValueList listProfiles = new TypedValueList();
    //            TypedValueList listNcParts = new TypedValueList();
    //            listProfiles.Add(typeof(BlockReference));
    //            listNcParts.Add(typeof(DBText));
    //            listNcParts.Add(DxfCode.Color, 35);
    //            var psr_Profiles = this.acEd.SelectWindow(pnts[0], pnts[1], new SelectionFilter(listProfiles));
    //            var psr_NCparts = this.acEd.SelectWindow(pnts[0], pnts[1], new SelectionFilter(listNcParts));
    //            //计算需要NC的零件号的数量
    //            if (psr_NCparts.Status == PromptStatus.OK) panelPartsQty += psr_NCparts.Value.Count;
    //            //计算型材的零件号的数量
    //            if (psr_Profiles.Status == PromptStatus.OK)
    //            {
    //                foreach (SelectedObject sid in psr_Profiles.Value)
    //                {
    //                    Entity ent = sid.ObjectId.GetObject(OpenMode.ForRead) as Entity;
    //                    if (ent is BlockReference)
    //                    {
    //                        BlockReference blkref = ent as BlockReference;
    //                        if (blkref.IsDynamicBlock)
    //                        {
    //                            var blk = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
    //                            if (blk.Name == "KFWHCPSD_ProfileInforTable") panelPartsQty++;
    //                        }
    //                    }
    //                }
    //            }
    //            var pir = this.acEd.GetInteger(new PromptIntegerOptions("请输入起始页码：\n") { AllowNone = true, AllowNegative = false, UseDefaultValue = true, DefaultValue = (int)Math.Ceiling(panelPartsQty / 13.0) + 4 });
    //            ////返回大于或等于指定的双精度浮点数的最小整数值。
    //            if (pir.Status == PromptStatus.OK) startPageNo = pir.Value;
    //            #endregion
    //            #region//获取用户指定范围内的所有图框
    //            if (psr.Status == PromptStatus.OK)
    //            {
    //                foreach (SelectedObject sid in psr.Value)
    //                {
    //                    Entity ent = sid.ObjectId.GetObject(OpenMode.ForRead) as Entity;
    //                    if (ent is BlockReference)
    //                    {
    //                        BlockReference blkref = ent as BlockReference;
    //                        if (blkref.IsDynamicBlock)
    //                        {
    //                            var blk = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
    //                            if (blk.Name == "KFWH_CP_SD_A3_H" || blk.Name == "KFWH_CP_SD_A3_V" || blk.Name == "KFWH_CP_SD_A4")
    //                            {
    //                                listTitleBlks.Add(blkref);
    //                                //Line l = new Line(Point3d.Origin, blkref.GeometricExtents.MinPoint);
    //                                //KFWH_CMD.Tools.AddToCurrentSpace(this.acDb, l);
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //            #endregion
    //            if (listTitleBlks.Count > 0)
    //            {
    //                //找到第一个图框
    //                var sortTitleBlks = listTitleBlks.OrderByDescending(c => c.Position.Y).ThenBy(c => c.Position.X).ToList();
    //                Dictionary<string, ObjectIdCollection> dicProfilePartsNestingPage = new Dictionary<string, ObjectIdCollection>();
    //                Dictionary<ObjectId, List<string>> dicProfiles = new Dictionary<ObjectId, List<string>>();
    //                #region//获取所有型材对应的页码的objectid
    //                TypedValueList listprofileParts = new TypedValueList();
    //                listprofileParts.Add(typeof(BlockReference));
    //                foreach (BlockReference item in sortTitleBlks)
    //                {
    //                    if (item.IsDynamicBlock)
    //                    {
    //                        var blk = item.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
    //                        if (blk.Name == "KFWH_CP_SD_A4")
    //                        {
    //                            var psr_curTitleBlksProfileParts = this.acEd.SelectWindow(item.GeometricExtents.MinPoint, item.GeometricExtents.MaxPoint, new SelectionFilter(listprofileParts));
    //                            if (psr_curTitleBlksProfileParts.Status == PromptStatus.OK)
    //                            {
    //                                foreach (SelectedObject selObject in psr_curTitleBlksProfileParts.Value)
    //                                {
    //                                    Entity ent = selObject.ObjectId.GetObject(OpenMode.ForRead) as Entity;
    //                                    if (ent is BlockReference)
    //                                    {
    //                                        BlockReference blkref = ent as BlockReference;
    //                                        if (blkref.IsDynamicBlock)
    //                                        {
    //                                            var blk1 = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
    //                                            //
    //                                            List<string> strTemp = new List<string>();
    //                                            if (blk1.Name == "KFWH_CP_SD_PN_ProfilePart")
    //                                            {
    //                                                var piecemark = blkref.ObjectId.GetAttributeInBlockReference("PIECE_MARK");
    //                                                if (piecemark != string.Empty)
    //                                                {
    //                                                    strTemp.Add(piecemark.Split(new string[] { " ", ",", ";" }, StringSplitOptions.RemoveEmptyEntries)[0]);
    //                                                }
    //                                                else
    //                                                {
    //                                                    Line l = new Line(Point3d.Origin, blkref.Position);
    //                                                    KFWH_CMD.Tools.AddToModelSpace(this.acDb, l);
    //                                                }
    //                                                ObjectId oid = item.ObjectId.GetAttributeObjectIdInBlockReference("SHT");
    //                                                if (dicProfiles.ContainsKey(oid))
    //                                                {
    //                                                    var temp = dicProfiles[oid];
    //                                                    temp.AddRange(strTemp.ToArray());
    //                                                }
    //                                                else dicProfiles[oid] = strTemp;
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //                //安零件名称统计套料页码！
    //                foreach (KeyValuePair<ObjectId, List<string>> item in dicProfiles)
    //                {
    //                    foreach (var strPartName in item.Value)
    //                    {
    //                        if (dicProfilePartsNestingPage.ContainsKey(strPartName))
    //                        {
    //                            var temp = dicProfilePartsNestingPage[strPartName];
    //                            if (!temp.Contains(item.Key)) temp.Add(item.Key);
    //                            dicProfilePartsNestingPage[strPartName] = temp;
    //                        }
    //                        else dicProfilePartsNestingPage[strPartName] = new ObjectIdCollection(new ObjectId[] { item.Key });
    //                    }
    //                }
    //                #endregion
    //                //获取KFWHCPSD_ProfileInforTable
    //                #region//写入页码的字段表达式
    //                foreach (BlockReference item in sortTitleBlks)
    //                {
    //                    if (item.IsDynamicBlock)
    //                    {
    //                        var blk = item.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
    //                        if (blk.Name == "KFWH_CP_SD_A4")
    //                        {
    //                            var psr_curTitleBlksProfileSketch = this.acEd.SelectWindow(item.GeometricExtents.MinPoint, item.GeometricExtents.MaxPoint, new SelectionFilter(listprofileParts));
    //                            if (psr_curTitleBlksProfileSketch.Status == PromptStatus.OK)
    //                            {
    //                                foreach (SelectedObject selObject in psr_curTitleBlksProfileSketch.Value)
    //                                {
    //                                    Entity ent = selObject.ObjectId.GetObject(OpenMode.ForRead) as Entity;
    //                                    if (ent is BlockReference)
    //                                    {
    //                                        BlockReference blkref = ent as BlockReference;
    //                                        if (blkref.IsDynamicBlock)
    //                                        {
    //                                            var blk2 = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
    //                                            if (blk2.Name == "KFWH_CP_SD_ProfileInforTable")//KFWH_CP_SD_ProfileInforTable
    //                                            {
    //                                                Dictionary<string, string> dicNestingPageNo = new Dictionary<string, string>();
    //                                                string strProfileName = blkref.ObjectId.GetAttributeInBlockReference("PARTNAME");
    //                                                if (dicProfilePartsNestingPage.ContainsKey(strProfileName))
    //                                                {
    //                                                    var oids = dicProfilePartsNestingPage[strProfileName];
    //                                                    for (int i = 0; i < oids.Count; i++)
    //                                                    {
    //                                                        //%<\AcObjProp Object(%<\_ObjId 1265480848>%).TextString>%
    //                                                        if (i > 0)
    //                                                        {
    //                                                            //$"%<\\AcObjProp Object(%<\\_ObjId {oidProBlkref.GetAttributeObjectIdInBlockReference("GRADE").OldIdPtr.ToInt64()} >%).TextString >%"
    //                                                            dicNestingPageNo["SHTNO"] += $", %<\\AcObjProp Object(%<\\_ObjId {oids[i].OldIdPtr.ToInt64()}>%).TextString>%";
    //                                                        }
    //                                                        else dicNestingPageNo["SHTNO"] = $"%<\\AcObjProp Object(%<\\_ObjId {oids[i].OldIdPtr.ToInt64()}>%).TextString>%";
    //                                                    }
    //                                                    blkref.ObjectId.UpdateAttributesInBlock(dicNestingPageNo);
    //                                                }
    //                                                else Application.ShowAlertDialog($"{strProfileName} 未做型材套料图！");
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //                #endregion
    //                // 填写页码。。。。。。
    //                for (int i = 0; i < sortTitleBlks.Count; i++)
    //                {
    //                    Dictionary<string, string> dicShpageTotal = new Dictionary<string, string>();
    //                    //A4+A3_V
    //                    dicShpageTotal["SHT"] = (startPageNo + i).ToString();
    //                    dicShpageTotal["TOTAL"] = (startPageNo + sortTitleBlks.Count - 1).ToString();
    //                    //A3_H
    //                    dicShpageTotal["SH"] = (startPageNo + i).ToString();
    //                    dicShpageTotal["PAGE"] = (startPageNo + sortTitleBlks.Count - 1).ToString();
    //                    sortTitleBlks[i].ObjectId.UpdateAttributesInBlock(dicShpageTotal);
    //                    dicShpageTotal.Clear();
    //                }
    //                Application.ShowAlertDialog($"共完成 {sortTitleBlks.Count} 个图框的页码填写！");
    //            }
    //            trans.Commit();
    //        }
    //    }
    //    [myAutoCADcmd("Update shop drawing title blcok information as per panel what you select", "SD_PrinttoPdf.png")]
    //    [CommandMethod("SD_PrinttoPdf")]
    //    public void KFWHSD_图框打印成pdf()
    //    {
    //        if (Application.GetSystemVariable("WORLDUCS").ToString() != "1") { this.acEd.CurrentUserCoordinateSystem = Matrix3d.Identity; this.acEd.Regen(); }
    //        //设置颜色样式
    //        if (Application.GetSystemVariable("CECOLOR").ToString() != "BYLAYER") Application.SetSystemVariable("CECOLOR", "BYLAYER");
    //        //if (Application.Version.Major >= 21)
    //        //{

    //        //    if (System.Windows.Forms.MessageBox.Show("由于你使用的是高版本的CAD,请确保你的AutoCAD的系统变量PDFSHX的值为0，否则打印出来的PDF会有很多很标记！！！！！", "",
    //        //         System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Information, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
    //        //    {
    //        //        this.acDoc.SendStringToExecute("PDFSHX ", true, false, true);
    //        //    }

    //        //}
    //        //设置应用到布局！！！！！

    //        //using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //        //{
    //        //    Layout acLayout = trans.GetObject(LayoutManager.Current.GetLayoutId(LayoutManager.Current.CurrentLayout), OpenMode.ForRead) as Layout;
    //        //    if (acLayout.CanonicalMediaName == "A3")
    //        //    {
    //        //        Application.ShowAlertDialog("该文件的打印设置未设置，请先设置并保存到布局然后再重启本命令！\n");
    //        //        return;
    //        //    }
    //        //}
    //        Point3dCollection pnts = new Point3dCollection();
    //        PromptPointOptions ppo = new PromptPointOptions("选取需要打印shop drawing区域的左下角:\n");
    //        var ppr = this.acEd.GetPoint(ppo);
    //        if (ppr.Status == PromptStatus.OK)
    //        {
    //            var ppr1 = this.acEd.GetCorner("选取需要打印shop drawing区域右上角：\n", ppr.Value);
    //            pnts.Add(ppr.Value);
    //            if (ppr1.Status == PromptStatus.OK) pnts.Add(ppr1.Value);
    //        }

    //        if (pnts.Count != 2) return;

    //        using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //        {
    //            BlockTable bt = trans.GetObject(acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
    //            BlockTableRecord ms = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
    //            List<BlockReference> listTitleBlks = new List<BlockReference>();
    //            TypedValueList list = new TypedValueList();
    //            list.Add(typeof(BlockReference));
    //            var psr = this.acEd.SelectWindow(pnts[0], pnts[1], new SelectionFilter(list));
    //            #region//获取用户指定范围内的所有图框
    //            if (psr.Status == PromptStatus.OK)
    //            {
    //                foreach (SelectedObject sid in psr.Value)
    //                {
    //                    Entity ent = sid.ObjectId.GetObject(OpenMode.ForRead) as Entity;
    //                    if (ent is BlockReference)
    //                    {
    //                        BlockReference blkref = ent as BlockReference;
    //                        if (blkref.IsDynamicBlock)
    //                        {
    //                            var blk = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
    //                            if (blk.Name == "KFWH_CP_SD_A3_H" || blk.Name == "KFWH_CP_SD_A3_V" || blk.Name == "KFWH_CP_SD_A4")
    //                            {
    //                                listTitleBlks.Add(blkref);
    //                                //Line l = new Line(Point3d.Origin, blkref.GeometricExtents.MinPoint);
    //                                //KFWH_CMD.Tools.AddToCurrentSpace(this.acDb, l);
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //            #endregion
    //            if (listTitleBlks.Count > 0)
    //            {
    //                //找到第一个图框
    //                var sortTitleBlks = listTitleBlks.OrderByDescending(c => c.Position.Y).ThenBy(c => c.Position.X).ToList();
    //                // 填写页码。。。。。。
    //                string shtNo = string.Empty;
    //                for (int i = 0; i < sortTitleBlks.Count; i++)
    //                {
    //                    #region//打印成pdf
    //                    Layout acLayout = trans.GetObject(LayoutManager.Current.GetLayoutId(LayoutManager.Current.CurrentLayout), OpenMode.ForRead) as Layout;
    //                    PlotInfo pi = new PlotInfo() { Layout = acLayout.ObjectId };
    //                    PlotSettings pss = new PlotSettings(true);
    //                    pss.CopyFrom(acLayout);
    //                    pss.PlotHidden = false;
    //                    pss.ShowPlotStyles = false;
    //                    pss.ShadePlot = PlotSettingsShadePlotType.Wireframe;
    //                    PlotSettingsValidator psv = PlotSettingsValidator.Current;
    //                    psv.RefreshLists(pss);
    //                    psv.SetCurrentStyleSheet(pss, "monochrome.ctb");
    //                    psv.SetPlotCentered(pss, true);
    //                    psv.SetPlotPaperUnits(pss, PlotPaperUnit.Millimeters);
    //                    //2020-08-05
    //                    double x_min = sortTitleBlks[i].GeometricExtents.MinPoint.X;
    //                    double y_min = sortTitleBlks[i].GeometricExtents.MinPoint.Y;
    //                    double x_max = sortTitleBlks[i].GeometricExtents.MaxPoint.X;
    //                    double y_max = sortTitleBlks[i].GeometricExtents.MaxPoint.Y;
    //                    Point3d ptTarget = (Point3d)Application.GetSystemVariable("TARGET");
    //                    psv.SetPlotWindowArea(pss, new Extents2d(x_min - ptTarget.X, y_min - ptTarget.Y,
    //                      x_max - ptTarget.X, y_max - ptTarget.Y));

    //                    psv.SetPlotType(pss, Autodesk.AutoCAD.DatabaseServices.PlotType.Window);
    //                    psv.SetUseStandardScale(pss, true);
    //                    psv.SetStdScaleType(pss, StdScaleType.ScaleToFit);
    //                    psv.SetPlotRotation(pss, PlotRotation.Degrees000);

    //                    if (sortTitleBlks[i].IsDynamicBlock)
    //                    {
    //                        var blk = sortTitleBlks[i].DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
    //                        switch (blk.Name)
    //                        {
    //                            case "KFWH_CP_SD_A3_H":
    //                                psv.SetPlotConfigurationName(pss, "DWG To PDF.pc3", "ISO_full_bleed_A3_(420.00_x_297.00_MM)");
    //                                break;
    //                            case "KFWH_CP_SD_A3_V":
    //                                psv.SetPlotConfigurationName(pss, "DWG To PDF.pc3", "ISO_full_bleed_A3_(297.00_x_420.00_MM)");
    //                                break;
    //                            case "KFWH_CP_SD_A4":
    //                                psv.SetPlotConfigurationName(pss, "DWG To PDF.pc3", "ISO_full_bleed_A4_(297.00_x_210.00_MM)");
    //                                break;
    //                        }
    //                        shtNo = sortTitleBlks[i].ObjectId.GetAttributeInBlockReference("SHT");
    //                        if (shtNo == string.Empty || shtNo == "") shtNo = sortTitleBlks[i].ObjectId.GetAttributeInBlockReference("SH");
    //                        if (shtNo == string.Empty || shtNo == "") shtNo = (i + 1).ToString();
    //                    }
    //                    pi.OverrideSettings = pss;
    //                    PlotInfoValidator piv = new PlotInfoValidator() { MediaMatchingPolicy = MatchingPolicy.MatchEnabled };
    //                    piv.Validate(pi);
    //                    //为了防止后台打印问题，必须在调用打印API时设置BACKGROUNDPLOT系统变量为0
    //                    short backPlot = (short)Application.GetSystemVariable("BACKGROUNDPLOT");
    //                    Application.SetSystemVariable("BACKGROUNDPLOT", 0);
    //                    if (PlotFactory.ProcessPlotState == ProcessPlotState.NotPlotting)
    //                    {
    //                        using (PlotEngine pe = PlotFactory.CreatePublishEngine())
    //                        {
    //                            PlotProgressDialog plotprogDlg = new PlotProgressDialog(false, 1, true);
    //                            using (plotprogDlg)
    //                            {
    //                                plotprogDlg.OnBeginPlot();
    //                                plotprogDlg.IsVisible = true;
    //                                pe.BeginPlot(plotprogDlg, null);
    //                                pe.BeginDocument(pi, this.acDoc.Name, null, 1, true, this.acDoc.Name.Replace(".dwg", "-" + shtNo + ".pdf"));
    //                                if (shtNo == string.Empty) this.acEd.WriteMessage("请先填写图框的页码再来打印成PDF文件\n\t");
    //                                pe.BeginPage(new PlotPageInfo(), pi, true, null);
    //                                pe.BeginGenerateGraphics(null);
    //                                pe.EndGenerateGraphics(null);
    //                                pe.EndPage(null);
    //                                pe.EndDocument(null);
    //                                pe.EndPlot(null);
    //                            }
    //                        }
    //                    }
    //                    Application.SetSystemVariable("BACKGROUNDPLOT", backPlot);
    //                    #endregion
    //                }
    //                Application.ShowAlertDialog($"共完成 {sortTitleBlks.Count} 个图框的打印成pdf！");
    //            }
    //            trans.Commit();
    //        }
    //    }
    //}
    ///// <summary>
    ///// 自动生成下料图---plate cutting
    ///// </summary>
    //public class 板材下料图
    //{
    //    public Document acDoc = Application.DocumentManager.MdiActiveDocument;
    //    public Database acDb = Application.DocumentManager.MdiActiveDocument.Database;
    //    public Editor acEd = Application.DocumentManager.MdiActiveDocument.Editor;

    //    //20190824
    //    [myAutoCADcmd("根据Panel Sketch图纸生成大板的下料图纸", "SD_SingleHplateCutting.png")]
    //    [CommandMethod("SD_SingleHplateCutting", CommandFlags.Session)]

    //    public void KFWHSD_根据panelsketch生成下料图()
    //    {
    //        using (DocumentLock dl = this.acDoc.LockDocument())
    //        {
    //            //将用户坐标系转换成世界坐标系
    //            if (Application.GetSystemVariable("WORLDUCS").ToString() != "1") { this.acEd.CurrentUserCoordinateSystem = Matrix3d.Identity; this.acEd.Regen(); }
    //            //设置颜色样式
    //            if (Application.GetSystemVariable("CECOLOR").ToString() != "BYLAYER") Application.SetSystemVariable("CECOLOR", "BYLAYER");
    //            this.acDb.MyCreateLayer("NC-CUTTING", 1, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-LABEL", 221, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-MARKING", 3, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-MARKING_TXT", 4, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-BEVEL", 42, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            BlockReference curLayoutTb = null;
    //            double titleblkScale = 0;
    //            string DIR_Xstring = ""; string DIR_Ystring = "";
    //            Scale3d dirSymbolScale = new Scale3d();
    //            List<SD_BigPlateSingle> listSinglePlates = new List<SD_BigPlateSingle>();
    //            using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //            {
    //                bool flag_A3tb = false; int errcount = 0;
    //                #region// to check whether was A3 Sketch Title block
    //                do
    //                {
    //                    PromptEntityResult per = this.acEd.GetEntity("请选择Panel Sketch drawing 的图框:");
    //                    if (per.Status == PromptStatus.OK)
    //                    {
    //                        Entity ent = per.ObjectId.GetObject(OpenMode.ForRead) as Entity;
    //                        if (ent is BlockReference)
    //                        {
    //                            if (ent.ObjectId.GetBlockReferenceName() == "KFWH_CP_SD_A3_H" || ent.ObjectId.GetBlockReferenceName() == "KFWH_CP_SD_A3_V")
    //                            {
    //                                curLayoutTb = ent as BlockReference;
    //                                flag_A3tb = true;
    //                            }
    //                            else { Application.ShowAlertDialog("你未选中A3的标准图框！3次退出本命令"); errcount++; }
    //                        }
    //                        else { Application.ShowAlertDialog("你未选中A3的标准图框！3次退出本命令"); errcount++; }
    //                    }
    //                    else { Application.ShowAlertDialog("你未选中任何图元！3次退出本命令"); errcount++; }
    //                }
    //                while (flag_A3tb == false && errcount < 3);

    //                #endregion
    //                if (curLayoutTb != null)
    //                {
    //                    //取出方位信息
    //                    titleblkScale = curLayoutTb.ScaleFactors.X;
    //                    #region //获取方位字符
    //                    var psr_Direction = this.acEd.SelectWindow(curLayoutTb.GeometricExtents.MaxPoint, curLayoutTb.GeometricExtents.MinPoint,
    //                    new SelectionFilter(new TypedValue[] {
    //                        new TypedValue((int)DxfCode.Start,"Insert"),
    //                        new TypedValue((int)DxfCode.BlockName,"KFWH_CP_SD_PanelSketchDirection")}));
    //                    if (psr_Direction.Status == PromptStatus.OK)
    //                    {
    //                        var blkref = psr_Direction.Value[0].ObjectId.GetObject(OpenMode.ForRead) as BlockReference;
    //                        dirSymbolScale = blkref.ScaleFactors;
    //                        foreach (ObjectId item in blkref.AttributeCollection)
    //                        {
    //                            AttributeReference attref = item.GetObject(OpenMode.ForRead) as AttributeReference;
    //                            switch (attref.Tag)
    //                            {
    //                                case "DIR-X":
    //                                    DIR_Xstring = attref.TextString;
    //                                    break;
    //                                case "DIR-Y":
    //                                    DIR_Ystring = attref.TextString;
    //                                    break;
    //                            }
    //                        }
    //                    }
    //                    #endregion
    //                    #region // 创建零件
    //                    var psr = this.acEd.SelectWindow(
    //                    curLayoutTb.GeometricExtents.MaxPoint, curLayoutTb.GeometricExtents.MinPoint,
    //                    new SelectionFilter(new TypedValue[] {
    //                        new TypedValue((int)DxfCode.Start,"LWPolyline"),
    //                        new TypedValue((int)DxfCode.Color,1)}));
    //                    if (psr.Status == PromptStatus.OK)
    //                    {
    //                        foreach (SelectedObject selObj in psr.Value)
    //                        {
    //                            Polyline polyline = selObj.ObjectId.GetObject(OpenMode.ForRead) as Polyline;
    //                            var psrMleader = acEd.SelectCrossingPolygon(polyline.GetAllPoints(), new SelectionFilter(new TypedValue[] { new TypedValue((int)DxfCode.Start, "MULTILEADER") }));
    //                            if (psrMleader.Status == PromptStatus.OK)
    //                            {
    //                                var item = psrMleader.Value[0].ObjectId.GetObject(OpenMode.ForRead) as MLeader;
    //                                if (((item as MLeader).MLeaderStyle.GetObject(OpenMode.ForRead) as MLeaderStyle).Name == "SectionPartML_Basic")
    //                                {
    //                                    MLeader ml = item as MLeader;
    //                                    Point2d pnt = new Point2d(ml.GetFirstVertex(0).X, ml.GetFirstVertex(0).Y);
    //                                    if (PointIsInPolyline.PtRelationToPoly(polyline, pnt, 0) == 1)
    //                                    {
    //                                        listSinglePlates.Add(new SD_BigPlateSingle(selObj.ObjectId, item.ObjectId));
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //                #endregion
    //                trans.Commit();
    //            }
    //            if (listSinglePlates.Count > 0)
    //            {
    //                #region //放入图框，摆入零件

    //                var sortPlateParts = listSinglePlates.OrderBy(c => c.PartName).ThenBy(c => c.PartName).ToList();

    //                PromptPointResult ppr = acEd.GetPoint("Please pick a point to Put Plate Cutting drawing Result:");

    //                if (ppr.Status == PromptStatus.OK)
    //                {
    //                    Point3d pnt = ppr.Value;
    //                    var TitbleBlockoid = this.acDb.InsertKFWHSDBlkstoCurDoc("KFWH_CP_SD_A4");
    //                    var ncinforBlockoid = this.acDb.InsertKFWHSDBlkstoCurDoc("KFWH_CP_SD_NCPartsInforTable");
    //                    using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //                    {
    //                        BlockTable bt = trans.GetObject(this.acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
    //                        BlockTableRecord modelspace = bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite) as BlockTableRecord;
    //                        for (int i = 0; i < sortPlateParts.Count; i++)
    //                        {
    //                            #region //插入图框
    //                            ObjectId tbblkrefid = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_A4", pnt, new Scale3d(titleblkScale, titleblkScale, titleblkScale), 0);
    //                            Dictionary<string, string> dictb = new Dictionary<string, string>();
    //                            dictb["MATL"] = "HPlate"; dictb["GRADE"] = "AS SHOWN"; dictb["SIZE"] = "AS SHOWN";
    //                            tbblkrefid.UpdateAttributesInBlock(dictb);
    //                            #endregion
    //                            #region //插入零件外形
    //                            Point3d curPartInsPnt = new Point3d(pnt.X + (100 * titleblkScale), pnt.Y + (10 * titleblkScale), pnt.Z);
    //                            var curPart = sortPlateParts[i];
    //                            Polyline oldPart = curPart.OriginalPartshape.GetObject(OpenMode.ForRead) as Polyline;
    //                            curPart.Partshape = curPart.OriginalPartshape.Copy(curPartInsPnt, oldPart.GeometricExtents.MinPoint);
    //                            Polyline newPart = curPart.Partshape.GetObject(OpenMode.ForRead) as Polyline;
    //                            newPart.UpgradeOpen();
    //                            newPart.Layer = "NC-CUTTING";
    //                            newPart.ColorIndex = 256;
    //                            newPart.DowngradeOpen();
    //                            #endregion
    //                            #region//插入坡口代码
    //                            if (curPart.bevelCodeTextIds.Count > 0)
    //                            {
    //                                curPart.FinallybevelCodeTextIds = new ObjectIdCollection();
    //                                foreach (ObjectId item in curPart.bevelCodeTextIds)
    //                                {
    //                                    var txtod = item.Copy(curPartInsPnt, oldPart.GeometricExtents.MinPoint);
    //                                    var txtent = txtod.GetObject(OpenMode.ForRead) as DBText;
    //                                    txtent.UpgradeOpen();
    //                                    txtent.Layer = "NC-BEVEL";
    //                                    txtent.ColorIndex = 256;
    //                                    txtent.TextString = $"%<\\AcObjProp Object(%<\\_ObjId {item.OldIdPtr.ToInt64()}>%).TextString>%";
    //                                    txtent.DowngradeOpen();
    //                                }
    //                            }
    //                            #endregion
    //                            #region //插入骨材的marking线
    //                            if (curPart.markingLineWithMl.Count > 0)
    //                            {
    //                                curPart.FinallymarkingLines = new List<ObjectId>();
    //                                foreach (ObjectId[] item in curPart.markingLineWithMl)
    //                                {
    //                                    var o1 = ((ObjectId)item[0]).Copy(curPartInsPnt, oldPart.GeometricExtents.MinPoint);
    //                                    var o2 = ((ObjectId)item[1]).Copy(curPartInsPnt, oldPart.GeometricExtents.MinPoint);
    //                                    Curve cur = o1.GetObject(OpenMode.ForRead) as Curve;
    //                                    if (cur is Line)
    //                                    {
    //                                        Point3dCollection crossPnts = new Point3dCollection();
    //                                        cur.IntersectWith(newPart, Intersect.OnBothOperands, crossPnts, IntPtr.Zero, IntPtr.Zero);
    //                                        Line l1 = cur as Line;
    //                                        if (crossPnts.Count > 0)
    //                                        {
    //                                            Polyline markingLine = new Polyline() { Layer = "NC-MARKING" };
    //                                            Point2dCollection Pnts2d = new Point2dCollection();
    //                                            l1.Erase();
    //                                            switch (crossPnts.Count)
    //                                            {
    //                                                default:
    //                                                    break;
    //                                                case 1:
    //                                                    if (PointIsInPolyline.PtRelationToPoly(newPart,
    //                                                        new Point2d(l1.StartPoint.X, l1.StartPoint.Y), 0.0001) == 1)
    //                                                    {
    //                                                        Pnts2d.Add(new Point2d(l1.StartPoint.X, l1.StartPoint.Y));
    //                                                        Pnts2d.Add(new Point2d(crossPnts[0].X, crossPnts[0].Y));
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        Pnts2d.Add(new Point2d(l1.EndPoint.X, l1.EndPoint.Y));
    //                                                        Pnts2d.Add(new Point2d(crossPnts[0].X, crossPnts[0].Y));
    //                                                    }
    //                                                    break;
    //                                                case 2:
    //                                                    Pnts2d.Add(new Point2d(crossPnts[0].X, crossPnts[0].Y));
    //                                                    Pnts2d.Add(new Point2d(crossPnts[1].X, crossPnts[1].Y));
    //                                                    break;
    //                                            }
    //                                            markingLine.CreatePolyline(Pnts2d);
    //                                            markingLine.Layer = "NC-MARKING";
    //                                            markingLine.ColorIndex = 256;
    //                                            curPart.FinallymarkingLines.Add(KFWH_CMD.Tools.AddToModelSpace(this.acDb, markingLine));
    //                                        }
    //                                        else
    //                                        {
    //                                            l1.UpgradeOpen();
    //                                            l1.Layer = "NC-MARKING";
    //                                            l1.ColorIndex = 256;
    //                                            l1.DowngradeOpen();
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                            #endregion
    //                            //零件名称
    //                            DBText dbtextPieceMark = new DBText() { Height = 2 * titleblkScale, Layer = "0", ColorIndex = 35, Position = newPart.GetCpnt(), TextString = curPart.PartName };
    //                            curPart.PieceMarkInSidePart = KFWH_CMD.Tools.AddToModelSpace(this.acDb, dbtextPieceMark);
    //                            #region //方位文字
    //                            if (DIR_Xstring != string.Empty || DIR_Ystring != string.Empty)
    //                            {
    //                                DBText dir_Xtext = new DBText() { TextString = DIR_Xstring, Layer = "NC-MARKING_TXT", Height = titleblkScale * 2, Rotation = Math.PI * 0.5 };
    //                                DBText dir_Ytext = new DBText() { TextString = DIR_Ystring, Layer = "NC-MARKING_TXT", Height = titleblkScale * 2, Rotation = 0 };
    //                                double w = newPart.GeometricExtents.MaxPoint.Y - newPart.GeometricExtents.MinPoint.Y;
    //                                double l = newPart.GeometricExtents.MaxPoint.X - newPart.GeometricExtents.MinPoint.X;

    //                                var lbPnt = newPart.GeometricExtents.MinPoint;
    //                                var ltPnt = new Point3d(newPart.GeometricExtents.MinPoint.X, newPart.GeometricExtents.MaxPoint.Y, newPart.GeometricExtents.MaxPoint.Z);
    //                                var rbPnt = new Point3d(newPart.GeometricExtents.MaxPoint.X, newPart.GeometricExtents.MinPoint.Y, newPart.GeometricExtents.MaxPoint.Z);
    //                                var rtPnt = newPart.GeometricExtents.MaxPoint;

    //                                if (dirSymbolScale.X > 0)
    //                                {
    //                                    dir_Xtext.Position = new Point3d(rtPnt.X - 1.5 * dir_Xtext.Height, 0.5 * (rtPnt.Y + rbPnt.Y), rtPnt.Z);
    //                                }
    //                                else
    //                                {
    //                                    dir_Xtext.Position = new Point3d(ltPnt.X + 1.5 * dir_Xtext.Height, 0.5 * (ltPnt.Y + lbPnt.Y), rtPnt.Z);
    //                                }

    //                                if (dirSymbolScale.Y > 0)
    //                                {
    //                                    dir_Ytext.Position = new Point3d((rtPnt.X + ltPnt.X) * 0.5, rtPnt.Y - 1.5 * dir_Ytext.Height, rtPnt.Z);
    //                                }
    //                                else
    //                                {
    //                                    dir_Ytext.Position = new Point3d((rtPnt.X + ltPnt.X) * 0.5, rbPnt.Y + 1.5 * dir_Ytext.Height, rtPnt.Z);
    //                                }

    //                                this.acDb.AddToCurrentSpace(dir_Ytext);
    //                                this.acDb.AddToCurrentSpace(dir_Xtext);
    //                            }
    //                            #endregion               
    //                            //获取下一个图框的插入点
    //                            pnt = new Point3d(pnt.X + (titleblkScale * 310), pnt.Y, pnt.Z);
    //                        }
    //                        trans.Commit();
    //                    }
    //                }
    //                #endregion
    //            }
    //        }
    //    }

    //    [myAutoCADcmd("根据plate layout图纸生成大板的下料图图纸", "SD_HplateCutting.png")]
    //    [CommandMethod("SD_HplateCutting", CommandFlags.Session)]
    //    public void KFWHSD_根据layout生成plateCutting()
    //    {
    //        using (DocumentLock dl = this.acDoc.LockDocument())
    //        {
    //            //将用户坐标系转换成世界坐标系
    //            if (Application.GetSystemVariable("WORLDUCS").ToString() != "1") { this.acEd.CurrentUserCoordinateSystem = Matrix3d.Identity; this.acEd.Regen(); }
    //            //设置颜色样式
    //            if (Application.GetSystemVariable("CECOLOR").ToString() != "BYLAYER") Application.SetSystemVariable("CECOLOR", "BYLAYER");
    //            this.acDb.MyCreateLayer("NC-CUTTING", 1, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-LABEL", 221, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-MARKING", 3, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-MARKING_TXT", 4, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-BEVEL", 42, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            List<SD_BigPlateLayoutPart> ListBigPlateParts = new List<SD_BigPlateLayoutPart>();
    //            BlockReference curLayoutTb = null;
    //            double titleblkScale = 0;
    //            using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //            {
    //                bool flag_A3tb = false; int errcount = 0;
    //                #region// to check whether was A3 Sketch Title block
    //                do
    //                {
    //                    PromptEntityResult per = this.acEd.GetEntity("请选择Layou图框:");
    //                    if (per.Status == PromptStatus.OK)
    //                    {
    //                        Entity ent = per.ObjectId.GetObject(OpenMode.ForRead) as Entity;
    //                        if (ent is BlockReference)
    //                        {
    //                            var blkref = ent as BlockReference;

    //                            if (blkref.IsDynamicBlock)
    //                            {
    //                                var blk = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
    //                                if (blk.Name == "KFWH_CP_SD_A3_H" || blk.Name == "KFWH_CP_SD_A3_V")
    //                                {
    //                                    curLayoutTb = blkref;
    //                                    flag_A3tb = true;
    //                                }
    //                            }
    //                            else { Application.ShowAlertDialog("你未选中A3的标准图框,3次退出本命令"); errcount++; }
    //                        }
    //                        else { Application.ShowAlertDialog("你未选中A3的标准图框！3次退出本命令"); errcount++; }
    //                    }
    //                    else { Application.ShowAlertDialog("你未选中任何图元！3次退出本命令"); errcount++; }
    //                }
    //                while (flag_A3tb == false && errcount < 3);

    //                #endregion
    //                if (curLayoutTb != null)
    //                {
    //                    //取出方位信息
    //                    Scale3d dirSymbolScale = new Scale3d();
    //                    titleblkScale = curLayoutTb.ScaleFactors.X;
    //                    string DIR_Xstring = ""; string DIR_Ystring = "";
    //                    #region //获取方位字符
    //                    var psr_Direction = this.acEd.SelectWindow(curLayoutTb.GeometricExtents.MaxPoint, curLayoutTb.GeometricExtents.MinPoint,
    //                    new SelectionFilter(new TypedValue[] {
    //                        new TypedValue((int)DxfCode.Start,"Insert"),
    //                        new TypedValue((int)DxfCode.BlockName,"KFWH_CP_SD_PanelSketchDirection")}));
    //                    if (psr_Direction.Status == PromptStatus.OK)
    //                    {
    //                        var blkref = psr_Direction.Value[0].ObjectId.GetObject(OpenMode.ForRead) as BlockReference;
    //                        dirSymbolScale = blkref.ScaleFactors;
    //                        foreach (ObjectId item in blkref.AttributeCollection)
    //                        {
    //                            AttributeReference attref = item.GetObject(OpenMode.ForRead) as AttributeReference;
    //                            switch (attref.Tag)
    //                            {
    //                                case "DIR-X":
    //                                    DIR_Xstring = attref.TextString;
    //                                    break;
    //                                case "DIR-Y":
    //                                    DIR_Ystring = attref.TextString;
    //                                    break;
    //                            }
    //                        }
    //                    }
    //                    #endregion
    //                    #region // 创建零件
    //                    var psr = this.acEd.SelectWindow(
    //                    curLayoutTb.GeometricExtents.MaxPoint, curLayoutTb.GeometricExtents.MinPoint,
    //                    new SelectionFilter(new TypedValue[] {
    //                        new TypedValue((int)DxfCode.Start,"LWPolyline"),
    //                        new TypedValue((int)DxfCode.LayerName,"NC-CUTTING"),
    //                        new TypedValue((int)DxfCode.Color,6)}));
    //                    bool needCreateParts = false;
    //                    if (psr.Status == PromptStatus.OK)
    //                    {
    //                        foreach (SelectedObject selObj in psr.Value)
    //                        {
    //                            Entity ent = selObj.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
    //                            if (ent is Polyline)
    //                            {
    //                                var oidforbou = MyTools.Copy(ent,
    //                                    new Point3d(curLayoutTb.GeometricExtents.MinPoint.X,
    //                                    curLayoutTb.GeometricExtents.MinPoint.Y - 2 * (curLayoutTb.GeometricExtents.MaxPoint.Y - curLayoutTb.GeometricExtents.MinPoint.Y),
    //                                    curLayoutTb.GeometricExtents.MinPoint.Z),
    //                                     curLayoutTb.GeometricExtents.MinPoint);
    //                                var bouNew = (oidforbou.GetObject(OpenMode.ForWrite) as Polyline);
    //                                var plineBoundary = ent as Polyline;
    //                                //获取多行引线对象
    //                                var psrMleader = acEd.SelectCrossingPolygon(plineBoundary.GetAllPoints(), new SelectionFilter(new TypedValue[] { new TypedValue((int)DxfCode.Start, "MULTILEADER") }));
    //                                if (psrMleader.Status == PromptStatus.OK)
    //                                {
    //                                    var item = psrMleader.Value[0].ObjectId.GetObject(OpenMode.ForRead) as MLeader;

    //                                    if (((item as MLeader).MLeaderStyle.GetObject(OpenMode.ForRead) as MLeaderStyle).Name == "SectionPartML_Basic")
    //                                    {
    //                                        SD_BigPlateLayoutPart oldplate = new SD_BigPlateLayoutPart(item as MLeader, plineBoundary);

    //                                        if (PointIsInPolyline.PtRelationToPoly(plineBoundary, new Point2d(oldplate.leaderPnt.X, oldplate.leaderPnt.Y), 0) == 1)
    //                                        {
    //                                            var oidforMleader = MyTools.Copy(item as Entity,
    //                                new Point3d(curLayoutTb.GeometricExtents.MinPoint.X,
    //                                curLayoutTb.GeometricExtents.MinPoint.Y - 2 * (curLayoutTb.GeometricExtents.MaxPoint.Y - curLayoutTb.GeometricExtents.MinPoint.Y),
    //                                curLayoutTb.GeometricExtents.MinPoint.Z),
    //                                 curLayoutTb.GeometricExtents.MinPoint);
    //                                            ent.ColorIndex = 256; bouNew.ColorIndex = 256; needCreateParts = true;

    //                                            #region //获取坡口信息并写入零件中
    //                                            //KFWH_CP_SD_LayoutMargin,KFWH_CP_SD_LayoutBevelCode

    //                                            var bevecodeLable = acEd.SelectCrossingPolygon(plineBoundary.GetAllPoints(),
    //                                                new SelectionFilter(new TypedValue[] {
    //                                                        new TypedValue((int)DxfCode.Start, RXClass.GetClass(typeof(DBText)).DxfName),
    //                                                        new TypedValue((int)DxfCode.LayerName, "KFWH_CP_SD_LayoutBevelCode"),
    //                                                        new TypedValue((int)DxfCode.Color, 256)
    //                                                    }));
    //                                            if (bevecodeLable.Status == PromptStatus.OK)
    //                                            {
    //                                                foreach (SelectedObject obj in bevecodeLable.Value)
    //                                                {
    //                                                    var oribevelCodeText = obj.ObjectId.GetObject(OpenMode.ForRead) as DBText;

    //                                                    var oidnewBevelText = MyTools.Copy(oribevelCodeText as Entity,
    //                               new Point3d(curLayoutTb.GeometricExtents.MinPoint.X,
    //                               curLayoutTb.GeometricExtents.MinPoint.Y - 2 * (curLayoutTb.GeometricExtents.MaxPoint.Y - curLayoutTb.GeometricExtents.MinPoint.Y),
    //                               curLayoutTb.GeometricExtents.MinPoint.Z),
    //                                curLayoutTb.GeometricExtents.MinPoint);
    //                                                    //
    //                                                    DBText newbevelCode = oidnewBevelText.GetObject(OpenMode.ForWrite) as DBText;
    //                                                    newbevelCode.Layer = "NC-BEVEL"; newbevelCode.ColorIndex = 256;
    //                                                }
    //                                            }

    //                                            #endregion

    //                                            #region// 获取 margin的marking线，并写入零件中
    //                                            ObjectIdCollection curPartOids = new ObjectIdCollection();
    //                                            var psrmarginMarkings = acEd.SelectCrossingPolygon(plineBoundary.GetAllPoints(),
    //                                                new SelectionFilter(new TypedValue[] {
    //                                                        new TypedValue((int)DxfCode.Start, RXClass.GetClass(typeof(Polyline)).DxfName),
    //                                                        new TypedValue((int)DxfCode.LayerName, "KFWH_CP_SD_LayoutMargin"),
    //                                                        new TypedValue((int)DxfCode.Color, 256),
    //                                                         new TypedValue((int)DxfCode.LinetypeName, "HIDDENX2")
    //                                                    }));
    //                                            if (psrmarginMarkings.Status == PromptStatus.OK)
    //                                            {
    //                                                foreach (SelectedObject seloid in psrmarginMarkings.Value)
    //                                                {
    //                                                    var markingRefLine = seloid.ObjectId.GetObject(OpenMode.ForWrite) as Polyline;
    //                                                    if (markingRefLine.Closed == false) markingRefLine.Closed = true;
    //                                                    Point3dCollection crossPnts = new Point3dCollection();
    //                                                    markingRefLine.IntersectWith(plineBoundary, Intersect.OnBothOperands, crossPnts, IntPtr.Zero, IntPtr.Zero);
    //                                                    if (crossPnts.Count > 0)
    //                                                    {
    //                                                        #region//marking line 与零件有交点
    //                                                        //产生按照交点分割的曲线
    //                                                        DBObjectCollection dbobjs = markingRefLine.GetSplitCurves(crossPnts);
    //                                                        if (dbobjs.Count % 2 == 0)
    //                                                        {
    //                                                            foreach (DBObject obj in dbobjs)
    //                                                            {
    //                                                                bool boolLineInPoly = true;
    //                                                                if (obj is Polyline)
    //                                                                {
    //                                                                    var objPlineMarkings = obj as Polyline;
    //                                                                    //判断marking线是否在当前零件内部？？？
    //                                                                    for (int n = 0; n < objPlineMarkings.NumberOfVertices; n++)
    //                                                                    {
    //                                                                        int intCheckVal = PointIsInPolyline.PtRelationToPoly(plineBoundary, new Point2d(objPlineMarkings.GetPoint3dAt(n).X, objPlineMarkings.GetPoint3dAt(n).Y), 0.001);
    //                                                                        if (intCheckVal == -1)
    //                                                                        {
    //                                                                            boolLineInPoly = false;
    //                                                                            break;
    //                                                                        }
    //                                                                    }
    //                                                                    if (boolLineInPoly)
    //                                                                    {
    //                                                                        objPlineMarkings.Layer = "NC-MARKING";
    //                                                                        objPlineMarkings.ColorIndex = 256;
    //                                                                        objPlineMarkings.Linetype = "Continuous";
    //                                                                        objPlineMarkings.MyMove(new Point3d(curLayoutTb.GeometricExtents.MinPoint.X, curLayoutTb.GeometricExtents.MinPoint.Y - 2 * (curLayoutTb.GeometricExtents.MaxPoint.Y - curLayoutTb.GeometricExtents.MinPoint.Y), curLayoutTb.GeometricExtents.MinPoint.Z), curLayoutTb.GeometricExtents.MinPoint);
    //                                                                        var markgLinesoid = this.acDb.AddToCurrentSpace(objPlineMarkings);
    //                                                                        curPartOids.Add(markgLinesoid);
    //                                                                    }
    //                                                                }
    //                                                            }
    //                                                        }
    //                                                        else
    //                                                        {
    //                                                            if (markingRefLine.IsOnlyLines)
    //                                                            {
    //                                                                var rossPnts = new List<Point3d>();
    //                                                                foreach (Point3d p in crossPnts) rossPnts.Add(p);
    //                                                                var sortPnt = rossPnts.OrderByDescending(c => c.Y).ThenBy(c => c.X).ToList();
    //                                                                for (int i = sortPnt.Count - 1; i > 0; i--)
    //                                                                {
    //                                                                    Polyline pl1 = new Polyline();
    //                                                                    if (i == 0)
    //                                                                    {
    //                                                                        pl1.CreatePolyline(new Point2d(sortPnt[i].X, crossPnts[i].Y), new Point2d(sortPnt[sortPnt.Count - 1].X, sortPnt[sortPnt.Count - 1].Y));
    //                                                                    }
    //                                                                    else
    //                                                                    {
    //                                                                        pl1.CreatePolyline(new Point2d(sortPnt[i].X, sortPnt[i].Y), new Point2d(sortPnt[i - 1].X, sortPnt[i - 1].Y));
    //                                                                    }
    //                                                                    var tempPoints = new Point3dCollection();
    //                                                                    pl1.IntersectWith(markingRefLine, Intersect.OnBothOperands, tempPoints, IntPtr.Zero, IntPtr.Zero);
    //                                                                    if (tempPoints.Count == 0)
    //                                                                    {
    //                                                                        pl1.Layer = "NC-MARKING";
    //                                                                        pl1.ColorIndex = 256;
    //                                                                        pl1.Linetype = "Continuous";
    //                                                                        pl1.MyMove(new Point3d(curLayoutTb.GeometricExtents.MinPoint.X, curLayoutTb.GeometricExtents.MinPoint.Y - 2 * (curLayoutTb.GeometricExtents.MaxPoint.Y - curLayoutTb.GeometricExtents.MinPoint.Y), curLayoutTb.GeometricExtents.MinPoint.Z), curLayoutTb.GeometricExtents.MinPoint);
    //                                                                        curPartOids.Add(this.acDb.AddToCurrentSpace(pl1));
    //                                                                    }
    //                                                                }
    //                                                            }
    //                                                        }
    //                                                        #endregion
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        var copyMarks = markingRefLine.Copy(new Point3d(curLayoutTb.GeometricExtents.MinPoint.X, curLayoutTb.GeometricExtents.MinPoint.Y - 2 * (curLayoutTb.GeometricExtents.MaxPoint.Y - curLayoutTb.GeometricExtents.MinPoint.Y), curLayoutTb.GeometricExtents.MinPoint.Z), curLayoutTb.GeometricExtents.MinPoint);
    //                                                        var entcopyMarks = copyMarks.GetObject(OpenMode.ForWrite) as Entity;
    //                                                        entcopyMarks.Layer = "NC-MARKING";
    //                                                        entcopyMarks.ColorIndex = 256;
    //                                                        entcopyMarks.Linetype = "Continuous";
    //                                                        curPartOids.Add(copyMarks);
    //                                                    }
    //                                                }
    //                                            }
    //                                            #endregion
    //                                            #region// added 2020-07-29 for nc cutting line and nc marking lines
    //                                            TypedValueList typeListMarkingLines = new TypedValueList();
    //                                            typeListMarkingLines.Add(DxfCode.Start, "Circle,Line,LWPolyline");
    //                                            typeListMarkingLines.Add(DxfCode.LayerName, "NC-MARKING");
    //                                            typeListMarkingLines.Add(DxfCode.Color, 23);
    //                                            var psr_ncmarkings = this.acEd.SelectCrossingPolygon(plineBoundary.GetAllPoints(), new SelectionFilter(typeListMarkingLines));
    //                                            if (psr_ncmarkings.Status == PromptStatus.OK)
    //                                            {
    //                                                foreach (SelectedObject seloid in psr_ncmarkings.Value)
    //                                                {
    //                                                    var markingRefLine = seloid.ObjectId.GetObject(OpenMode.ForWrite) as Polyline;
    //                                                    if (markingRefLine.Closed == false) markingRefLine.Closed = true;
    //                                                    Point3dCollection crossPnts = new Point3dCollection();
    //                                                    markingRefLine.IntersectWith(plineBoundary, Intersect.OnBothOperands, crossPnts, IntPtr.Zero, IntPtr.Zero);
    //                                                    if (crossPnts.Count > 0)
    //                                                    {
    //                                                        #region//marking line 与零件有交点
    //                                                        //产生按照交点分割的曲线
    //                                                        DBObjectCollection dbobjs = markingRefLine.GetSplitCurves(crossPnts);
    //                                                        if (dbobjs.Count % 2 == 0)
    //                                                        {
    //                                                            foreach (DBObject obj in dbobjs)
    //                                                            {
    //                                                                bool boolLineInPoly = true;
    //                                                                if (obj is Polyline)
    //                                                                {
    //                                                                    var objPlineMarkings = obj as Polyline;
    //                                                                    //判断marking线是否在当前零件内部？？？
    //                                                                    for (int n = 0; n < objPlineMarkings.NumberOfVertices; n++)
    //                                                                    {
    //                                                                        int intCheckVal = PointIsInPolyline.PtRelationToPoly(plineBoundary, new Point2d(objPlineMarkings.GetPoint3dAt(n).X, objPlineMarkings.GetPoint3dAt(n).Y), 0.001);
    //                                                                        if (intCheckVal == -1)
    //                                                                        {
    //                                                                            boolLineInPoly = false;
    //                                                                            break;
    //                                                                        }
    //                                                                    }
    //                                                                    if (boolLineInPoly)
    //                                                                    {
    //                                                                        objPlineMarkings.Layer = "NC-MARKING";
    //                                                                        objPlineMarkings.ColorIndex = 256;
    //                                                                        objPlineMarkings.Linetype = "Continuous";
    //                                                                        objPlineMarkings.MyMove(new Point3d(curLayoutTb.GeometricExtents.MinPoint.X, curLayoutTb.GeometricExtents.MinPoint.Y - 2 * (curLayoutTb.GeometricExtents.MaxPoint.Y - curLayoutTb.GeometricExtents.MinPoint.Y), curLayoutTb.GeometricExtents.MinPoint.Z), curLayoutTb.GeometricExtents.MinPoint);
    //                                                                        var markgLinesoid = this.acDb.AddToCurrentSpace(objPlineMarkings);
    //                                                                        curPartOids.Add(markgLinesoid);
    //                                                                    }
    //                                                                }
    //                                                            }
    //                                                        }
    //                                                        else
    //                                                        {
    //                                                            if (markingRefLine.IsOnlyLines)
    //                                                            {
    //                                                                var rossPnts = new List<Point3d>();
    //                                                                foreach (Point3d p in crossPnts) rossPnts.Add(p);
    //                                                                var sortPnt = rossPnts.OrderByDescending(c => c.Y).ThenBy(c => c.X).ToList();
    //                                                                for (int i = sortPnt.Count - 1; i > 0; i--)
    //                                                                {
    //                                                                    Polyline pl1 = new Polyline();
    //                                                                    if (i == 0)
    //                                                                    {
    //                                                                        pl1.CreatePolyline(new Point2d(sortPnt[i].X, crossPnts[i].Y), new Point2d(sortPnt[sortPnt.Count - 1].X, sortPnt[sortPnt.Count - 1].Y));
    //                                                                    }
    //                                                                    else
    //                                                                    {
    //                                                                        pl1.CreatePolyline(new Point2d(sortPnt[i].X, sortPnt[i].Y), new Point2d(sortPnt[i - 1].X, sortPnt[i - 1].Y));
    //                                                                    }
    //                                                                    var tempPoints = new Point3dCollection();
    //                                                                    pl1.IntersectWith(markingRefLine, Intersect.OnBothOperands, tempPoints, IntPtr.Zero, IntPtr.Zero);
    //                                                                    if (tempPoints.Count == 0)
    //                                                                    {
    //                                                                        pl1.Layer = "NC-MARKING";
    //                                                                        pl1.ColorIndex = 256;
    //                                                                        pl1.Linetype = "Continuous";
    //                                                                        pl1.MyMove(new Point3d(curLayoutTb.GeometricExtents.MinPoint.X, curLayoutTb.GeometricExtents.MinPoint.Y - 2 * (curLayoutTb.GeometricExtents.MaxPoint.Y - curLayoutTb.GeometricExtents.MinPoint.Y), curLayoutTb.GeometricExtents.MinPoint.Z), curLayoutTb.GeometricExtents.MinPoint);
    //                                                                        curPartOids.Add(this.acDb.AddToCurrentSpace(pl1));
    //                                                                    }
    //                                                                }
    //                                                            }
    //                                                        }
    //                                                        #endregion
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        var copyMarks = markingRefLine.Copy(new Point3d(curLayoutTb.GeometricExtents.MinPoint.X, curLayoutTb.GeometricExtents.MinPoint.Y - 2 * (curLayoutTb.GeometricExtents.MaxPoint.Y - curLayoutTb.GeometricExtents.MinPoint.Y), curLayoutTb.GeometricExtents.MinPoint.Z), curLayoutTb.GeometricExtents.MinPoint);
    //                                                        var entcopyMarks = copyMarks.GetObject(OpenMode.ForWrite) as Entity;
    //                                                        entcopyMarks.Layer = markingRefLine.Layer;
    //                                                        entcopyMarks.ColorIndex = 256;
    //                                                        entcopyMarks.Linetype = "Continuous";
    //                                                        //curPartOids.Add(copyMarks);
    //                                                    }
    //                                                }
    //                                            }
    //                                            TypedValueList typeListCuttingLines = new TypedValueList();
    //                                            typeListCuttingLines.Add(DxfCode.Start, "Circle,Line,LWPolyline");
    //                                            typeListCuttingLines.Add(DxfCode.LayerName, "NC-CUTTING");
    //                                            typeListCuttingLines.Add(DxfCode.Color, 170);

    //                                            var psr_NCcuttings = this.acEd.SelectCrossingPolygon(plineBoundary.GetAllPoints(), new SelectionFilter(typeListCuttingLines));
    //                                            if (psr_NCcuttings.Status == PromptStatus.OK)
    //                                            {
    //                                                foreach (SelectedObject seloid in psr_NCcuttings.Value)
    //                                                {
    //                                                    var markingRefLine = seloid.ObjectId.GetObject(OpenMode.ForWrite) as Polyline;
    //                                                    if (markingRefLine.Closed == false) markingRefLine.Closed = true;
    //                                                    Point3dCollection crossPnts = new Point3dCollection();
    //                                                    markingRefLine.IntersectWith(plineBoundary, Intersect.OnBothOperands, crossPnts, IntPtr.Zero, IntPtr.Zero);
    //                                                    if (crossPnts.Count > 0)
    //                                                    {
    //                                                        #region//marking line 与零件有交点
    //                                                        //产生按照交点分割的曲线
    //                                                        DBObjectCollection dbobjs = markingRefLine.GetSplitCurves(crossPnts);
    //                                                        if (dbobjs.Count % 2 == 0)
    //                                                        {
    //                                                            foreach (DBObject obj in dbobjs)
    //                                                            {
    //                                                                bool boolLineInPoly = true;
    //                                                                if (obj is Polyline)
    //                                                                {
    //                                                                    var objPlineMarkings = obj as Polyline;
    //                                                                    //判断marking线是否在当前零件内部？？？
    //                                                                    for (int n = 0; n < objPlineMarkings.NumberOfVertices; n++)
    //                                                                    {
    //                                                                        int intCheckVal = PointIsInPolyline.PtRelationToPoly(plineBoundary, new Point2d(objPlineMarkings.GetPoint3dAt(n).X, objPlineMarkings.GetPoint3dAt(n).Y), 0.001);
    //                                                                        if (intCheckVal == -1)
    //                                                                        {
    //                                                                            boolLineInPoly = false;
    //                                                                            break;
    //                                                                        }
    //                                                                    }
    //                                                                    if (boolLineInPoly)
    //                                                                    {
    //                                                                        objPlineMarkings.Layer = "NC-MARKING";
    //                                                                        objPlineMarkings.ColorIndex = 256;
    //                                                                        objPlineMarkings.Linetype = "Continuous";
    //                                                                        objPlineMarkings.MyMove(new Point3d(curLayoutTb.GeometricExtents.MinPoint.X, curLayoutTb.GeometricExtents.MinPoint.Y - 2 * (curLayoutTb.GeometricExtents.MaxPoint.Y - curLayoutTb.GeometricExtents.MinPoint.Y), curLayoutTb.GeometricExtents.MinPoint.Z), curLayoutTb.GeometricExtents.MinPoint);
    //                                                                        var markgLinesoid = this.acDb.AddToCurrentSpace(objPlineMarkings);
    //                                                                        curPartOids.Add(markgLinesoid);
    //                                                                    }
    //                                                                }
    //                                                            }
    //                                                        }
    //                                                        else
    //                                                        {
    //                                                            if (markingRefLine.IsOnlyLines)
    //                                                            {
    //                                                                var rossPnts = new List<Point3d>();
    //                                                                foreach (Point3d p in crossPnts) rossPnts.Add(p);
    //                                                                var sortPnt = rossPnts.OrderByDescending(c => c.Y).ThenBy(c => c.X).ToList();
    //                                                                for (int i = sortPnt.Count - 1; i > 0; i--)
    //                                                                {
    //                                                                    Polyline pl1 = new Polyline();
    //                                                                    if (i == 0)
    //                                                                    {
    //                                                                        pl1.CreatePolyline(new Point2d(sortPnt[i].X, crossPnts[i].Y), new Point2d(sortPnt[sortPnt.Count - 1].X, sortPnt[sortPnt.Count - 1].Y));
    //                                                                    }
    //                                                                    else
    //                                                                    {
    //                                                                        pl1.CreatePolyline(new Point2d(sortPnt[i].X, sortPnt[i].Y), new Point2d(sortPnt[i - 1].X, sortPnt[i - 1].Y));
    //                                                                    }
    //                                                                    var tempPoints = new Point3dCollection();
    //                                                                    pl1.IntersectWith(markingRefLine, Intersect.OnBothOperands, tempPoints, IntPtr.Zero, IntPtr.Zero);
    //                                                                    if (tempPoints.Count == 0)
    //                                                                    {
    //                                                                        pl1.Layer = "NC-MARKING";
    //                                                                        pl1.ColorIndex = 256;
    //                                                                        pl1.Linetype = "Continuous";
    //                                                                        pl1.MyMove(new Point3d(curLayoutTb.GeometricExtents.MinPoint.X, curLayoutTb.GeometricExtents.MinPoint.Y - 2 * (curLayoutTb.GeometricExtents.MaxPoint.Y - curLayoutTb.GeometricExtents.MinPoint.Y), curLayoutTb.GeometricExtents.MinPoint.Z), curLayoutTb.GeometricExtents.MinPoint);
    //                                                                        curPartOids.Add(this.acDb.AddToCurrentSpace(pl1));
    //                                                                    }
    //                                                                }
    //                                                            }
    //                                                        }
    //                                                        #endregion
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        var copyMarks = markingRefLine.Copy(new Point3d(curLayoutTb.GeometricExtents.MinPoint.X, curLayoutTb.GeometricExtents.MinPoint.Y - 2 * (curLayoutTb.GeometricExtents.MaxPoint.Y - curLayoutTb.GeometricExtents.MinPoint.Y), curLayoutTb.GeometricExtents.MinPoint.Z), curLayoutTb.GeometricExtents.MinPoint);
    //                                                        var entcopyMarks = copyMarks.GetObject(OpenMode.ForWrite) as Entity;
    //                                                        entcopyMarks.Layer = markingRefLine.Layer;
    //                                                        entcopyMarks.ColorIndex = 256;
    //                                                        entcopyMarks.Linetype = "Continuous";
    //                                                        //curPartOids.Add(copyMarks);
    //                                                    }
    //                                                }
    //                                            }

    //                                            #endregion

    //                                            #region  // 方位文字写入零件中
    //                                            if (DIR_Xstring != string.Empty || DIR_Ystring != string.Empty)
    //                                            {
    //                                                DBText dir_Xtext = new DBText() { TextString = DIR_Xstring, Layer = "NC-MARKING_TXT", Height = titleblkScale * 2, Rotation = Math.PI * 0.5 };
    //                                                DBText dir_Ytext = new DBText() { TextString = DIR_Ystring, Layer = "NC-MARKING_TXT", Height = titleblkScale * 2, Rotation = 0 };
    //                                                double w = bouNew.GeometricExtents.MaxPoint.Y - bouNew.GeometricExtents.MinPoint.Y;
    //                                                double l = bouNew.GeometricExtents.MaxPoint.X - bouNew.GeometricExtents.MinPoint.X;

    //                                                var lbPnt = bouNew.GeometricExtents.MinPoint;
    //                                                var ltPnt = new Point3d(bouNew.GeometricExtents.MinPoint.X, bouNew.GeometricExtents.MaxPoint.Y, bouNew.GeometricExtents.MaxPoint.Z);
    //                                                var rbPnt = new Point3d(bouNew.GeometricExtents.MaxPoint.X, bouNew.GeometricExtents.MinPoint.Y, bouNew.GeometricExtents.MaxPoint.Z);
    //                                                var rtPnt = bouNew.GeometricExtents.MaxPoint;

    //                                                if (dirSymbolScale.X > 0)
    //                                                {
    //                                                    dir_Xtext.Position = new Point3d(rtPnt.X - 1.5 * dir_Xtext.Height, 0.5 * (rtPnt.Y + rbPnt.Y), rtPnt.Z);
    //                                                }
    //                                                else
    //                                                {
    //                                                    dir_Xtext.Position = new Point3d(ltPnt.X + 1.5 * dir_Xtext.Height, 0.5 * (ltPnt.Y + lbPnt.Y), rtPnt.Z);
    //                                                }

    //                                                if (dirSymbolScale.Y > 0)
    //                                                {
    //                                                    dir_Ytext.Position = new Point3d((rtPnt.X + ltPnt.X) * 0.5, rtPnt.Y - 1.5 * dir_Ytext.Height, rtPnt.Z);
    //                                                }
    //                                                else
    //                                                {
    //                                                    dir_Ytext.Position = new Point3d((rtPnt.X + ltPnt.X) * 0.5, rbPnt.Y + 1.5 * dir_Ytext.Height, rtPnt.Z);
    //                                                }

    //                                                this.acDb.AddToCurrentSpace(dir_Ytext);
    //                                                this.acDb.AddToCurrentSpace(dir_Xtext);
    //                                            }

    //                                            //20190705
    //                                            #endregion
    //                                            var mleaderNew = (oidforMleader.GetObject(OpenMode.ForWrite) as MLeader);

    //                                            SD_BigPlateLayoutPart newplate = new SD_BigPlateLayoutPart(mleaderNew, bouNew, curPartOids);

    //                                            ListBigPlateParts.Add(newplate);

    //                                        }
    //                                    }
    //                                    if (needCreateParts == false) bouNew.Erase();
    //                                }
    //                                else bouNew.Erase();
    //                            }
    //                        }
    //                    }
    //                    #endregion
    //                }
    //                trans.Commit();
    //            }
    //            if (ListBigPlateParts.Count > 0)
    //            {
    //                #region //放入图框，摆入零件

    //                var sortPlateParts = ListBigPlateParts.OrderBy(c => c.PartName).ThenBy(c => c.Length).ThenBy(c => c.Width).ToList();

    //                PromptPointResult ppr = acEd.GetPoint("Please pick a point to Put Plate Cutting drawing Result:");

    //                if (ppr.Status == PromptStatus.OK)
    //                {
    //                    Point3d pnt = ppr.Value;

    //                    var TitbleBlockoid = this.acDb.InsertKFWHSDBlkstoCurDoc("KFWH_CP_SD_A4");

    //                    var ncinforBlockoid = this.acDb.InsertKFWHSDBlkstoCurDoc("KFWH_CP_SD_NCPartsInforTable");

    //                    using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //                    {
    //                        BlockTable bt = trans.GetObject(this.acDb.BlockTableId, OpenMode.ForRead) as BlockTable;

    //                        BlockTableRecord modelspace = bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite) as BlockTableRecord;

    //                        for (int i = 0; i < sortPlateParts.Count; i = i + 2)
    //                        {

    //                            #region //插入图框
    //                            ObjectId tbblkrefid = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_A4", pnt, new Scale3d(titleblkScale, titleblkScale, titleblkScale), 0);
    //                            Dictionary<string, string> dictb = new Dictionary<string, string>();
    //                            dictb["MATL"] = "HPlate"; dictb["GRADE"] = "AS SHOWN"; dictb["SIZE"] = "AS SHOWN";
    //                            tbblkrefid.UpdateAttributesInBlock(dictb);
    //                            #endregion
    //                            #region //插入零件
    //                            Point3d part1InsPnt = new Point3d(pnt.X + (176 * titleblkScale), pnt.Y + (150 * titleblkScale), pnt.Z);

    //                            Point3d part2InsPnt = new Point3d(pnt.X + (176 * titleblkScale), pnt.Y + (82 * titleblkScale), pnt.Z);

    //                            var curPart1 = sortPlateParts[i];

    //                            SD_BigPlateLayoutPart curPart2 = null;

    //                            if (i + 1 < sortPlateParts.Count) curPart2 = sortPlateParts[i + 1];

    //                            AddedParts2CurTitleBlock(titleblkScale, modelspace, part1InsPnt, curPart1);

    //                            if (curPart2 != null) AddedParts2CurTitleBlock(titleblkScale, modelspace, part2InsPnt, curPart2);

    //                            #endregion
    //                            //获取下一个图框的插入点
    //                            pnt = new Point3d(pnt.X + (titleblkScale * 310), pnt.Y, pnt.Z);
    //                        }
    //                        trans.Commit();
    //                    }
    //                }
    //                #endregion
    //            }
    //        }
    //    }

    //    [myAutoCADcmd("选择A4图框插入bevel 节点图", "SD_A4BevelDetail.png")]
    //    [CommandMethod("SD_A4BevelDetail", CommandFlags.Session)]
    //    public void KFWHSD_为PlateCutting插入bevel节点()
    //    {
    //        using (DocumentLock dl = this.acDoc.LockDocument())
    //        {
    //            //将用户坐标系转换成世界坐标系
    //            if (Application.GetSystemVariable("WORLDUCS").ToString() != "1") { this.acEd.CurrentUserCoordinateSystem = Matrix3d.Identity; this.acEd.Regen(); }
    //            //设置颜色样式
    //            if (Application.GetSystemVariable("CECOLOR").ToString() != "BYLAYER") Application.SetSystemVariable("CECOLOR", "BYLAYER");
    //            var pso = new PromptSelectionOptions() { MessageForAdding = "选择A4的下料图框插入坡口节点：" };
    //            var psr = this.acEd.GetSelection(pso);
    //            if (psr.Status == PromptStatus.OK)
    //            {
    //                using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //                {
    //                    BlockTable bt = trans.GetObject(this.acDb.BlockTableId, OpenMode.ForRead) as BlockTable;

    //                    BlockTableRecord modelspace = bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite) as BlockTableRecord;

    //                    foreach (SelectedObject item in psr.Value)
    //                    {
    //                        Entity ent = item.ObjectId.GetObject(OpenMode.ForRead) as Entity;
    //                        if (ent is BlockReference)
    //                        {
    //                            if (item.ObjectId.GetBlockReferenceName() == "KFWH_CP_SD_A4")
    //                            {
    //                                BlockReference blkref = item.ObjectId.GetObject(OpenMode.ForRead) as BlockReference;

    //                                List<string> strBevelCodes = new List<string>();

    //                                double tbScale = blkref.ScaleFactors.X;

    //                                Point3d bevelDetInsPnt = new Point3d(blkref.GeometricExtents.MinPoint.X + (245 * tbScale), blkref.GeometricExtents.MinPoint.Y + (18 * tbScale), blkref.GeometricExtents.MinPoint.Z);// bevel detail 插入点

    //                                #region //获取块内部的代表坡口的字符串
    //                                var psr_BevelCode_ProfileInforTable = this.acEd.SelectWindow(blkref.GeometricExtents.MinPoint, blkref.GeometricExtents.MaxPoint);
    //                                if (psr_BevelCode_ProfileInforTable.Status == PromptStatus.OK)
    //                                {
    //                                    List<string> str = new List<string>();
    //                                    foreach (SelectedObject obj in psr_BevelCode_ProfileInforTable.Value)
    //                                    {
    //                                        var ent11 = obj.ObjectId.GetObject(OpenMode.ForRead) as Entity;
    //                                        if (ent11 is BlockReference)
    //                                        {
    //                                            var blkrefTable = ent11 as BlockReference;
    //                                            if (blkrefTable.IsDynamicBlock)
    //                                            {
    //                                                var blk = blkrefTable.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
    //                                                if (blk.Name == "KFWH_CP_SD_ProfileInforTable")
    //                                                {
    //                                                    str.Add(obj.ObjectId.GetAttributeInBlockReference("WEB_BEVCODE_A"));
    //                                                    str.Add(obj.ObjectId.GetAttributeInBlockReference("WEB_BEVCODE_B"));
    //                                                    str.Add(obj.ObjectId.GetAttributeInBlockReference("FLG_BEVCODE_A"));
    //                                                    str.Add(obj.ObjectId.GetAttributeInBlockReference("FLG_BEVCODE_B"));
    //                                                }
    //                                            }
    //                                        }

    //                                    }
    //                                    foreach (var s in str) if (!strBevelCodes.Contains(s) && s != "-") strBevelCodes.Add(s);
    //                                }
    //                                #endregion

    //                                #region //获取NC 零件的下料图框内的坡口代码文字
    //                                var psr_BevelCode_NCParts = this.acEd.SelectWindow(blkref.GeometricExtents.MinPoint, blkref.GeometricExtents.MaxPoint, new SelectionFilter(new TypedValue[] {
    //                            new TypedValue((int)DxfCode.Start,RXClass.GetClass(typeof(DBText)).DxfName),new TypedValue((int)DxfCode.LayerName,"NC-BEVEL"),new TypedValue((int)DxfCode.Color,256)
    //                        }));
    //                                if (psr_BevelCode_NCParts.Status == PromptStatus.OK)
    //                                {
    //                                    foreach (SelectedObject obj in psr_BevelCode_NCParts.Value)
    //                                    {
    //                                        DBText bevelText = obj.ObjectId.GetObject(OpenMode.ForRead) as DBText;
    //                                        if (!strBevelCodes.Contains(bevelText.TextString)) strBevelCodes.Add(bevelText.TextString);
    //                                    }
    //                                }
    //                                #endregion

    //                                #region //插入插入当前图框捏零件的所有bevel的节点

    //                                if (strBevelCodes.Count > 0)
    //                                {
    //                                    foreach (var bevCode in strBevelCodes)
    //                                    {
    //                                        KFWHBevelCode bev = new KFWHBevelCode(bevCode);
    //                                        if (bev.BevelCodeDwgName != null && bev.WeldingType != KFWHBevelCode.KFWHWeldType.WrongCode)
    //                                        {
    //                                            var oidblkBev = this.acDb.InsertKFWHSDBevelDtailtoCurDoc(bev.BevelCodeDwgName);
    //                                            if (oidblkBev != ObjectId.Null)
    //                                            {
    //                                                ObjectId oidblkrefBev = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", bev.BevelCodeDwgName, bevelDetInsPnt, new Scale3d(tbScale, tbScale, tbScale), 0);
    //                                                Dictionary<string, string> dicAttVal = new Dictionary<string, string>();
    //                                                dicAttVal["NAME"] = "%%U" + bev.BevelCodeLabel;
    //                                                if (bev.WeldingType != KFWHBevelCode.KFWHWeldType.ChamferOnly && bev.WeldingType != KFWHBevelCode.KFWHWeldType.DoubleSideChmfer)
    //                                                {
    //                                                    switch (bev.WeldingType)
    //                                                    {
    //                                                        case KFWHBevelCode.KFWHWeldType.ManualWeld:
    //                                                            switch (bev.BevelStyle)
    //                                                            {
    //                                                                case KFWHBevelCode.KFWHBevelStyle.X:
    //                                                                    dicAttVal["ANGLE_NS"] = bev.AngleValueNearSide + "%%d";
    //                                                                    dicAttVal["ANGLE_FS"] = bev.AngleValueFarSide + "%%d";
    //                                                                    break;
    //                                                                case KFWHBevelCode.KFWHBevelStyle.V:
    //                                                                    if (bev.AngleValueNearSide != 0) dicAttVal["ANGLE"] = bev.AngleValueNearSide + "%%d"; else dicAttVal["ANGLE"] = bev.AngleValueFarSide + "%%d";
    //                                                                    break;
    //                                                                case KFWHBevelCode.KFWHBevelStyle.F:
    //                                                                    if (bev.AngleValueNearSide * bev.AngleValueFarSide != 0)
    //                                                                    {
    //                                                                        dicAttVal["ANGLE_NS"] = bev.AngleValueNearSide + "%%d";
    //                                                                        dicAttVal["ANGLE_FS"] = bev.AngleValueFarSide + "%%d";
    //                                                                    }
    //                                                                    else
    //                                                                    {
    //                                                                        if (bev.AngleValueNearSide != 0) dicAttVal["ANGLE"] = bev.AngleValueNearSide + "%%d"; else dicAttVal["ANGLE"] = bev.AngleValueFarSide + "%%d";
    //                                                                    }
    //                                                                    break;
    //                                                                case KFWHBevelCode.KFWHBevelStyle.N:
    //                                                                    if (bev.AngleValueNearSide != 0) dicAttVal["ANGLE"] = bev.AngleValueNearSide + "%%d"; else dicAttVal["ANGLE"] = bev.AngleValueFarSide + "%%d";
    //                                                                    break;
    //                                                            }
    //                                                            break;
    //                                                        case KFWHBevelCode.KFWHWeldType.AutoWeld:
    //                                                            if (bev.BevelStyle == KFWHBevelCode.KFWHBevelStyle.K)
    //                                                            {
    //                                                                dicAttVal["ANGLE_NS"] = bev.AngleValueNearSide + "%%d";
    //                                                                dicAttVal["ANGLE_FS"] = bev.AngleValueFarSide + "%%d";
    //                                                            }
    //                                                            else
    //                                                            {
    //                                                                if (bev.AngleValueNearSide != 0) dicAttVal["ANGLE"] = bev.AngleValueNearSide + "%%d"; else dicAttVal["ANGLE"] = bev.AngleValueFarSide + "%%d";
    //                                                            }
    //                                                            break;
    //                                                        case KFWHBevelCode.KFWHWeldType.CreamicWeld:
    //                                                            if (bev.AngleValueNearSide != 0) dicAttVal["ANGLE"] = bev.AngleValueNearSide + "%%d"; else dicAttVal["ANGLE"] = bev.AngleValueFarSide + "%%d";
    //                                                            break;
    //                                                    }
    //                                                }
    //                                                if (dicAttVal.Count > 0) oidblkrefBev.UpdateAttributesInBlock(dicAttVal);

    //                                                bevelDetInsPnt = new Point3d(bevelDetInsPnt.X - (45 * tbScale), bevelDetInsPnt.Y, bevelDetInsPnt.Z);
    //                                            }
    //                                            else Application.ShowAlertDialog($"无法为{bevCode}找到对应的节点，请手动放置节点");
    //                                        }
    //                                        else Application.ShowAlertDialog($"无法为{bevCode}找到对应的节点，请手动放置节点");
    //                                    }
    //                                }
    //                                else Application.ShowAlertDialog($"无法插入破口节点，未找到任何表示破口的文字！");
    //                                #endregion
    //                            }
    //                        }

    //                    }
    //                    trans.Commit();
    //                }
    //            }
    //        }
    //    }


    //    [myAutoCADcmd("选择A3图框插入bevel 节点图", "SD_A3BevelDetail.png")]
    //    [CommandMethod("SD_A3BevelDetail", CommandFlags.Session)]
    //    public void KFWHSD_为A3图插入bevel节点()
    //    {
    //        using (DocumentLock dl = this.acDoc.LockDocument())
    //        {
    //            //将用户坐标系转换成世界坐标系
    //            if (Application.GetSystemVariable("WORLDUCS").ToString() != "1") { this.acEd.CurrentUserCoordinateSystem = Matrix3d.Identity; this.acEd.Regen(); }
    //            //设置颜色样式
    //            if (Application.GetSystemVariable("CECOLOR").ToString() != "BYLAYER") Application.SetSystemVariable("CECOLOR", "BYLAYER");
    //            var pso = new PromptSelectionOptions() { MessageForAdding = "选择A3图框插入坡口节点：" };
    //            var psr = this.acEd.GetSelection(pso);
    //            if (psr.Status == PromptStatus.OK)
    //            {
    //                using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //                {
    //                    BlockTable bt = trans.GetObject(this.acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
    //                    BlockTableRecord modelspace = bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite) as BlockTableRecord;
    //                    foreach (SelectedObject item in psr.Value)
    //                    {
    //                        var ent = item.ObjectId.GetObject(OpenMode.ForRead) as Entity;
    //                        if (ent is BlockReference)
    //                        {
    //                            var blkref = ent as BlockReference;
    //                            if (blkref.IsDynamicBlock)
    //                            {
    //                                var blk = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
    //                                if (blk.Name == "KFWH_CP_SD_A3_H" || blk.Name == "KFWH_CP_SD_A3_V")
    //                                {
    //                                    List<string> strBevelCodes = new List<string>();
    //                                    double tbScale = blkref.ScaleFactors.X;
    //                                    Point3d bevelDetInsPnt = new Point3d(blkref.GeometricExtents.MinPoint.X + (11 * tbScale), blkref.GeometricExtents.MinPoint.Y + (18 * tbScale), blkref.GeometricExtents.MinPoint.Z);// bevel detail 插入点
    //                                    #region //获取A3图框内的坡口代码文字
    //                                    var psr_BevelCode_NCParts = this.acEd.SelectWindow(blkref.GeometricExtents.MinPoint, blkref.GeometricExtents.MaxPoint, new SelectionFilter(new TypedValue[] {
    //                            new TypedValue((int)DxfCode.Start,RXClass.GetClass(typeof(DBText)).DxfName),new TypedValue((int)DxfCode.LayerName,"KFWH_CP_SD_LayoutBevelCode"),new TypedValue((int)DxfCode.Color,256)
    //                        }));
    //                                    if (psr_BevelCode_NCParts.Status == PromptStatus.OK)
    //                                    {
    //                                        foreach (SelectedObject obj in psr_BevelCode_NCParts.Value)
    //                                        {
    //                                            DBText bevelText = obj.ObjectId.GetObject(OpenMode.ForRead) as DBText;
    //                                            if (!strBevelCodes.Contains(bevelText.TextString)) strBevelCodes.Add(bevelText.TextString);
    //                                        }
    //                                    }
    //                                    #endregion
    //                                    #region //插入插入当前图框捏零件的所有bevel的节点
    //                                    if (strBevelCodes.Count > 0)
    //                                    {
    //                                        foreach (var bevCode in strBevelCodes)
    //                                        {
    //                                            KFWHBevelCode bev = new KFWHBevelCode(bevCode);
    //                                            if (bev.BevelCodeDwgName != null && bev.WeldingType != KFWHBevelCode.KFWHWeldType.WrongCode)
    //                                            {
    //                                                var oidblkBev = this.acDb.InsertKFWHSDBevelDtailtoCurDoc(bev.BevelCodeDwgName);
    //                                                if (oidblkBev != ObjectId.Null)
    //                                                {
    //                                                    ObjectId oidblkrefBev = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", bev.BevelCodeDwgName, bevelDetInsPnt, new Scale3d(tbScale, tbScale, tbScale), 0);
    //                                                    Dictionary<string, string> dicAttVal = new Dictionary<string, string>();
    //                                                    dicAttVal["NAME"] = "%%U" + bev.BevelCodeLabel;
    //                                                    if (bev.WeldingType != KFWHBevelCode.KFWHWeldType.ChamferOnly && bev.WeldingType != KFWHBevelCode.KFWHWeldType.DoubleSideChmfer)
    //                                                    {
    //                                                        switch (bev.WeldingType)
    //                                                        {
    //                                                            case KFWHBevelCode.KFWHWeldType.ManualWeld:
    //                                                                switch (bev.BevelStyle)
    //                                                                {
    //                                                                    case KFWHBevelCode.KFWHBevelStyle.X:
    //                                                                        dicAttVal["ANGLE_NS"] = bev.AngleValueNearSide + "%%d";
    //                                                                        dicAttVal["ANGLE_FS"] = bev.AngleValueFarSide + "%%d";
    //                                                                        break;
    //                                                                    case KFWHBevelCode.KFWHBevelStyle.V:
    //                                                                        if (bev.AngleValueNearSide != 0) dicAttVal["ANGLE"] = bev.AngleValueNearSide + "%%d"; else dicAttVal["ANGLE"] = bev.AngleValueFarSide + "%%d";
    //                                                                        break;
    //                                                                    case KFWHBevelCode.KFWHBevelStyle.F:
    //                                                                        if (bev.AngleValueNearSide * bev.AngleValueFarSide != 0)
    //                                                                        {
    //                                                                            dicAttVal["ANGLE_NS"] = bev.AngleValueNearSide + "%%d";
    //                                                                            dicAttVal["ANGLE_FS"] = bev.AngleValueFarSide + "%%d";
    //                                                                        }
    //                                                                        else
    //                                                                        {
    //                                                                            if (bev.AngleValueNearSide != 0) dicAttVal["ANGLE"] = bev.AngleValueNearSide + "%%d"; else dicAttVal["ANGLE"] = bev.AngleValueFarSide + "%%d";
    //                                                                        }
    //                                                                        break;

    //                                                                    //if (bev.AngleValueNearSide != 0) dicAttVal["ANGLE"] = bev.AngleValueNearSide + "%%d"; else dicAttVal["ANGLE"] = bev.AngleValueFarSide + "%%d";
    //                                                                    //break;
    //                                                                    case KFWHBevelCode.KFWHBevelStyle.N:
    //                                                                        if (bev.AngleValueNearSide != 0) dicAttVal["ANGLE"] = bev.AngleValueNearSide + "%%d"; else dicAttVal["ANGLE"] = bev.AngleValueFarSide + "%%d";
    //                                                                        break;
    //                                                                }
    //                                                                break;
    //                                                            case KFWHBevelCode.KFWHWeldType.AutoWeld:
    //                                                                if (bev.BevelStyle == KFWHBevelCode.KFWHBevelStyle.K)
    //                                                                {
    //                                                                    dicAttVal["ANGLE_NS"] = bev.AngleValueNearSide + "%%d";
    //                                                                    dicAttVal["ANGLE_FS"] = bev.AngleValueFarSide + "%%d";
    //                                                                }
    //                                                                else
    //                                                                {
    //                                                                    if (bev.AngleValueNearSide != 0) dicAttVal["ANGLE"] = bev.AngleValueNearSide + "%%d"; else dicAttVal["ANGLE"] = bev.AngleValueFarSide + "%%d";
    //                                                                }
    //                                                                break;
    //                                                            case KFWHBevelCode.KFWHWeldType.CreamicWeld:
    //                                                                if (bev.AngleValueNearSide != 0) dicAttVal["ANGLE"] = bev.AngleValueNearSide + "%%d"; else dicAttVal["ANGLE"] = bev.AngleValueFarSide + "%%d";
    //                                                                break;
    //                                                        }
    //                                                    }
    //                                                    if (dicAttVal.Count > 0) oidblkrefBev.UpdateAttributesInBlock(dicAttVal);

    //                                                    bevelDetInsPnt = new Point3d(bevelDetInsPnt.X + (60 * tbScale), bevelDetInsPnt.Y, bevelDetInsPnt.Z);
    //                                                }
    //                                                else Application.ShowAlertDialog($"无法为{bevCode}找到对应的节点，请手动放置节点");
    //                                            }
    //                                            else Application.ShowAlertDialog($"无法为{bevCode}找到对应的节点，请手动放置节点");
    //                                        }
    //                                    }
    //                                    else Application.ShowAlertDialog($"无法插入破口节点，未找到任何表示破口的文字！");
    //                                    #endregion
    //                                }
    //                            }
    //                        }

    //                    }
    //                    trans.Commit();
    //                }
    //            }
    //        }
    //    }


    //    [myAutoCADcmd("生成T-Girder的下料图", "SD_TgirderDrawing.png")]
    //    [CommandMethod("SD_TgirderDrawing", CommandFlags.Session)]
    //    public void KFWHSD_根据Section生成Tgirder下料图纸()
    //    {
    //        using (DocumentLock dl = this.acDoc.LockDocument())
    //        {
    //            //将用户坐标系转换成世界坐标系
    //            if (Application.GetSystemVariable("WORLDUCS").ToString() != "1") { this.acEd.CurrentUserCoordinateSystem = Matrix3d.Identity; this.acEd.Regen(); }
    //            //设置颜色样式
    //            if (Application.GetSystemVariable("CECOLOR").ToString() != "BYLAYER") Application.SetSystemVariable("CECOLOR", "BYLAYER");
    //            //
    //            this.acDb.MyCreateLayer("NC-CUTTING", 1, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-LABEL", 221, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-MARKING", 3, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-MARKING_TXT", 4, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-BEVEL", 42, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            //
    //            var pso = new PromptSelectionOptions() { MessageForAdding = "选择A3的图框，主要有section 和一些detail 节点" };
    //            var psr = this.acEd.GetSelection(pso);
    //            //
    //            List<SD_TgirderPart> listAllGirder = new List<SD_TgirderPart>();
    //            #region//获取所有的T-Girder零件
    //            if (psr.Status == PromptStatus.OK)
    //            {
    //                using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //                {
    //                    foreach (SelectedObject selObj in psr.Value)
    //                    {
    //                        Entity ent = selObj.ObjectId.GetObject(OpenMode.ForRead) as Entity;
    //                        if (ent is BlockReference)
    //                        {
    //                            var blkref = ent as BlockReference;
    //                            if (blkref.IsDynamicBlock)
    //                            {
    //                                var blk = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
    //                                if (blk.Name == "KFWH_CP_SD_A3_H" || blk.Name == "KFWH_CP_SD_A3_V")
    //                                {
    //                                    string dirX_Value = string.Empty;
    //                                    string dirY_Value = string.Empty;
    //                                    TypedValueList filter_Direction = new TypedValueList();
    //                                    filter_Direction.Add(typeof(BlockReference));
    //                                    filter_Direction.Add(DxfCode.BlockName, "KFWH_CP_SD_PanelSketchDirection");
    //                                    var psr_DirectionSymbol = this.acEd.SelectWindow(ent.GeometricExtents.MinPoint, ent.GeometricExtents.MaxPoint, new SelectionFilter(filter_Direction));
    //                                    if (psr_DirectionSymbol.Status == PromptStatus.OK)
    //                                    {
    //                                        BlockReference blkrefDirectionSymbol = psr_DirectionSymbol.Value[0].ObjectId.GetObject(OpenMode.ForRead) as BlockReference;
    //                                        dirX_Value = blkrefDirectionSymbol.ScaleFactors.X + "|" + psr_DirectionSymbol.Value[0].ObjectId.GetAttributeInBlockReference("DIR-X");
    //                                        dirY_Value = blkrefDirectionSymbol.ScaleFactors.Y + "|" + psr_DirectionSymbol.Value[0].ObjectId.GetAttributeInBlockReference("DIR-Y");
    //                                    }
    //                                    TypedValueList filter_web = new TypedValueList();
    //                                    filter_web.Add(typeof(Polyline));
    //                                    filter_web.Add(DxfCode.LayerName, "NC-CUTTING");
    //                                    filter_web.Add(DxfCode.Color, 6);
    //                                    var psr_GirderWeb = this.acEd.SelectWindow(ent.GeometricExtents.MinPoint, ent.GeometricExtents.MaxPoint, new SelectionFilter(filter_web));
    //                                    if (psr_GirderWeb.Status == PromptStatus.OK)
    //                                    {
    //                                        #region//获取腹板和面板
    //                                        foreach (SelectedObject selObjWeb in psr_GirderWeb.Value)
    //                                        {
    //                                            Polyline webPlateShape = selObjWeb.ObjectId.GetObject(OpenMode.ForRead) as Polyline;
    //                                            TypedValueList filter_webMlLabel = new TypedValueList();
    //                                            filter_webMlLabel.Add(typeof(MLeader));
    //                                            var psrWebLeader = this.acEd.SelectCrossingPolygon(webPlateShape.GetAllPoints(), new SelectionFilter(filter_webMlLabel));
    //                                            if (psrWebLeader.Status == PromptStatus.OK)
    //                                            {
    //                                                foreach (SelectedObject selobj in psrWebLeader.Value)
    //                                                {
    //                                                    var ml = selobj.ObjectId.GetObject(OpenMode.ForRead) as MLeader;
    //                                                    var pntMl = ml.GetFirstVertex(0);
    //                                                    if ((ml.MLeaderStyle.GetObject(OpenMode.ForRead) as MLeaderStyle).Name == "SectionPartML_Basic")
    //                                                    {
    //                                                        if (PointIsInPolyline.PtRelationToPoly(webPlateShape, new Point2d(pntMl.X, pntMl.Y), 0.00001) == 1)
    //                                                        {
    //                                                            SD_TgirderPart t_girder = new SD_TgirderPart(ml.ObjectId, webPlateShape.ObjectId, this.acDb, dirX_Value, dirY_Value);
    //                                                            listAllGirder.Add(t_girder);
    //                                                        }
    //                                                    }
    //                                                }
    //                                            }
    //                                            else Autodesk.AutoCAD.ApplicationServices.Core.Application.ShowAlertDialog("请检查腹板的形状！");
    //                                        }
    //                                        #endregion
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                    trans.Commit();
    //                }
    //            }
    //            #endregion
    //            if (listAllGirder.Count > 0)
    //            {
    //                var sortlistTGirders = listAllGirder.OrderBy(c => c.WebName).ToList();

    //                #region //放入图框，摆入零件

    //                PromptPointResult ppr = acEd.GetPoint("Please pick a point to Put Plate Cutting drawing Result:");
    //                if (ppr.Status == PromptStatus.OK)
    //                {
    //                    Point3d pnt = ppr.Value;
    //                    var TitbleBlockoid = this.acDb.InsertKFWHSDBlkstoCurDoc("KFWH_CP_SD_A4");
    //                    var ncinforBlockoid = this.acDb.InsertKFWHSDBlkstoCurDoc("KFWH_CP_SD_NCPartsInforTable");
    //                    using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //                    {
    //                        BlockTable bt = trans.GetObject(this.acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
    //                        BlockTableRecord modelspace = bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite) as BlockTableRecord;
    //                        for (int i = 0; i < sortlistTGirders.Count; i++)
    //                        {
    //                            var titleblkScale = GetA4TitleBlocksScale(sortlistTGirders[i].Length);
    //                            ObjectId tbblkrefid = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_A4", pnt, new Scale3d(titleblkScale, titleblkScale, titleblkScale), 0);
    //                            Dictionary<string, string> dictb = new Dictionary<string, string>();
    //                            dictb["MATL"] = "T_Girder"; dictb["GRADE"] = "AS SHOWN"; dictb["SIZE"] = "AS SHOWN";
    //                            tbblkrefid.UpdateAttributesInBlock(dictb);
    //                            Point3d webInsPnt = new Point3d(pnt.X + (100 * titleblkScale), pnt.Y + (100 * titleblkScale), pnt.Z);
    //                            var tgirderPart = sortlistTGirders[i];
    //                            Entity ent = tgirderPart.webPlateShapeOld.GetObject(OpenMode.ForWrite) as Entity;
    //                            ent.ColorIndex = 256;
    //                            //插入腹板
    //                            tgirderPart.webPlateShapeFinally = ent.Copy(webInsPnt, ent.GeometricExtents.MinPoint);
    //                            var entNew = tgirderPart.webPlateShapeFinally.GetObject(OpenMode.ForWrite) as Entity;
    //                            entNew.ColorIndex = 256;
    //                            #region// 插入marking线和开孔
    //                            if (tgirderPart.WebPlatemarkingsOld.Count > 0)
    //                            {
    //                                tgirderPart.WebPlatemarkingsFinally = new ObjectIdCollection();
    //                                foreach (ObjectId item in tgirderPart.WebPlatemarkingsOld)
    //                                {
    //                                    ObjectId oid = item.Copy(webInsPnt, ent.GeometricExtents.MinPoint);
    //                                    var oidNew = oid.GetObject(OpenMode.ForWrite) as Entity;
    //                                    oidNew.ColorIndex = 256;
    //                                    tgirderPart.WebPlatemarkingsFinally.Add(oid);
    //                                }
    //                            }

    //                            if (tgirderPart.WebPlateCutingLinesOld.Count > 0)
    //                            {
    //                                tgirderPart.WebPlateCutingLinesFinally = new ObjectIdCollection();
    //                                foreach (ObjectId item in tgirderPart.WebPlateCutingLinesOld)
    //                                {
    //                                    ObjectId oid = item.Copy(webInsPnt, ent.GeometricExtents.MinPoint);
    //                                    var oidNew = oid.GetObject(OpenMode.ForWrite) as Entity;
    //                                    oidNew.ColorIndex = 256;
    //                                    tgirderPart.WebPlateCutingLinesFinally.Add(oid);
    //                                }
    //                            }
    //                            #endregion

    //                            #region//插入腹板名称和方位文字
    //                            DBText txtName = new DBText() { Position = entNew.GetCpnt(), Height = titleblkScale * 2.0, Rotation = 0, ColorIndex = 35, Layer = "0", TextString = tgirderPart.WebName };


    //                            if (tgirderPart.DirX != string.Empty)
    //                            {
    //                                DBText directionTxt = null;
    //                                if (!tgirderPart.DirX.Contains("-"))
    //                                {
    //                                    directionTxt = new DBText()
    //                                    {
    //                                        Position = new Point3d(entNew.GetCpnt().X + (tgirderPart.Length / 2) - titleblkScale * 4, entNew.GetCpnt().Y, entNew.GetCpnt().Z),
    //                                        Height = titleblkScale * 2.0,
    //                                        Rotation = Math.PI / 2,
    //                                        ColorIndex = 256,
    //                                        Layer = "NC-MARKING_TXT",
    //                                        TextString = tgirderPart.DirX.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[1]
    //                                    };
    //                                }
    //                                else
    //                                {
    //                                    directionTxt = new DBText()
    //                                    {
    //                                        Position = new Point3d(entNew.GetCpnt().X - (tgirderPart.Length / 2) + titleblkScale * 4, entNew.GetCpnt().Y, entNew.GetCpnt().Z),
    //                                        Height = titleblkScale * 2.0,
    //                                        Rotation = Math.PI / 2,
    //                                        ColorIndex = 256,
    //                                        Layer = "NC-MARKING_TXT",
    //                                        TextString = tgirderPart.DirX.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[1]
    //                                    };
    //                                }
    //                                KFWH_CMD.Tools.AddToModelSpace(this.acDb, directionTxt);
    //                            }

    //                            tgirderPart.webNameTextoid = KFWH_CMD.Tools.AddToModelSpace(this.acDb, txtName);
    //                            #endregion

    //                            #region//插入面板
    //                            if (tgirderPart.curGirderFacePlates.Count > 0)
    //                            {
    //                                List<Polyline> listFaces = new List<Polyline>();
    //                                for (int k = 0; k < tgirderPart.curGirderFacePlates.Count; k++)
    //                                {
    //                                    SD_TgirderFacePlatePart face = tgirderPart.curGirderFacePlates[k];
    //                                    face.facePlateProfileFinally = face.facePlateProfileOld.Copy(webInsPnt, ent.GeometricExtents.MinPoint);
    //                                    var entFaceProfile = face.facePlateProfileFinally.GetObject(OpenMode.ForRead) as Entity;
    //                                    entFaceProfile.Layer = "0";
    //                                    Point3d facePlateInPoint = Point3d.Origin;
    //                                    Polyline plineFace = new Polyline();
    //                                    Line faceCenterLine = null;
    //                                    DBText txtFaceName01 = null;
    //                                    if (entFaceProfile.GeometricExtents.MinPoint.Y < entNew.GeometricExtents.MinPoint.Y)
    //                                    {
    //                                        facePlateInPoint = new Point3d(entFaceProfile.GeometricExtents.MinPoint.X, entFaceProfile.GeometricExtents.MinPoint.Y - titleblkScale * 15, entFaceProfile.GeometricExtents.MinPoint.Z);
    //                                        plineFace.CreateRectangle(new Point2d(facePlateInPoint.X, facePlateInPoint.Y), new Point2d(facePlateInPoint.X + face.Length, facePlateInPoint.Y - double.Parse(face.Width)));
    //                                        txtFaceName01 = new DBText()
    //                                        {
    //                                            Position = new Point3d(facePlateInPoint.X + (face.Length * 0.5), facePlateInPoint.Y + titleblkScale * 10, facePlateInPoint.Z),
    //                                            Height = titleblkScale * 2.0,
    //                                            Rotation = 0,
    //                                            ColorIndex = 30,
    //                                            Layer = "0",
    //                                            TextString = face.FaceName
    //                                        };
    //                                        faceCenterLine = new Line(new Point3d(facePlateInPoint.X, facePlateInPoint.Y - 0.5 * double.Parse(face.Width), facePlateInPoint.Z),
    //                                            new Point3d(facePlateInPoint.X + face.Length, facePlateInPoint.Y - 0.5 * double.Parse(face.Width), facePlateInPoint.Z));
    //                                    }
    //                                    else
    //                                    {
    //                                        facePlateInPoint = new Point3d(entFaceProfile.GeometricExtents.MinPoint.X, entFaceProfile.GeometricExtents.MaxPoint.Y + titleblkScale * 15, entFaceProfile.GeometricExtents.MinPoint.Z);
    //                                        plineFace.CreateRectangle(new Point2d(facePlateInPoint.X, facePlateInPoint.Y), new Point2d(facePlateInPoint.X + face.Length, facePlateInPoint.Y + double.Parse(face.Width)));
    //                                        txtFaceName01 = new DBText()
    //                                        {
    //                                            Position = new Point3d(facePlateInPoint.X + (face.Length * 0.5), facePlateInPoint.Y - titleblkScale * 10, facePlateInPoint.Z),
    //                                            Height = titleblkScale * 2.0,
    //                                            Rotation = 0,
    //                                            ColorIndex = 30,
    //                                            Layer = "0",
    //                                            TextString = face.FaceName
    //                                        };
    //                                        faceCenterLine = new Line(new Point3d(facePlateInPoint.X, facePlateInPoint.Y + 0.5 * double.Parse(face.Width), facePlateInPoint.Z),
    //                                            new Point3d(facePlateInPoint.X + face.Length, facePlateInPoint.Y + 0.5 * double.Parse(face.Width), facePlateInPoint.Z));
    //                                    }
    //                                    listFaces.Add(plineFace);
    //                                    faceCenterLine.Layer = "NC-MARKING"; faceCenterLine.ColorIndex = 256;
    //                                    KFWH_CMD.Tools.AddToModelSpace(this.acDb, txtFaceName01);
    //                                    KFWH_CMD.Tools.AddToModelSpace(this.acDb, faceCenterLine);
    //                                    if (face.FaceType == SD_GirderFaceType.WithOutSection) plineFace.Layer = "NC-CUTTING"; plineFace.ColorIndex = 256;
    //                                    face.facePlateShapeFinally = KFWH_CMD.Tools.AddToModelSpace(this.acDb, plineFace);

    //                                    #region //插入名称，方位文字
    //                                    DBText txtFaceName = new DBText() { Position = plineFace.GetCpnt(), Height = titleblkScale * 2.0, Rotation = 0, ColorIndex = 35, Layer = "0", TextString = face.FaceName };
    //                                    MyInsertMoudleLineSymbol(plineFace.GetCpnt(), this.acDb, titleblkScale, MoudleLineType.Plate_Mid, Math.PI);
    //                                    DBText txtFaceDirection = null;
    //                                    if (tgirderPart.DirX != string.Empty)
    //                                    {
    //                                        if (!tgirderPart.DirX.Contains("-"))
    //                                        {
    //                                            txtFaceDirection = new DBText()
    //                                            {
    //                                                Position = new Point3d(plineFace.GetCpnt().X + (face.Length / 2) - titleblkScale * 3, plineFace.GetCpnt().Y, plineFace.GetCpnt().Z),
    //                                                Height = titleblkScale * 2.0,
    //                                                Rotation = Math.PI / 2,
    //                                                ColorIndex = 256,
    //                                                Layer = "NC-MARKING_TXT",
    //                                                TextString = tgirderPart.DirX.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[1]
    //                                            };
    //                                        }
    //                                        else
    //                                        {
    //                                            txtFaceDirection = new DBText()
    //                                            {
    //                                                Position = new Point3d(plineFace.GetCpnt().X - (face.Length / 2) + titleblkScale * 3, plineFace.GetCpnt().Y, plineFace.GetCpnt().Z),
    //                                                Height = titleblkScale * 2.0,
    //                                                Rotation = Math.PI / 2,
    //                                                ColorIndex = 256,
    //                                                Layer = "NC-MARKING_TXT",
    //                                                TextString = tgirderPart.DirX.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[1]
    //                                            };
    //                                        }
    //                                        KFWH_CMD.Tools.AddToModelSpace(this.acDb, txtFaceDirection);
    //                                    }

    //                                    face.FaceNameTextoid = KFWH_CMD.Tools.AddToModelSpace(this.acDb, txtFaceName);
    //                                    #endregion
    //                                }
    //                            }
    //                            #endregion

    //                            // 插入明细表
    //                            Point3d pntTable = new Point3d(webInsPnt.X + tgirderPart.Length / 5, webInsPnt.Y - (titleblkScale * 55), webInsPnt.Z);
    //                            var oidTbstyle = TableTools.AddTableStyle(this.acDb, "KFWHSDTGBOM");
    //                            var tableOid = this.acDb.CreateTable(pntTable, 5 + tgirderPart.curGirderFacePlates.Count, 4, oidTbstyle, titleblkScale);
    //                            var bom = tableOid.GetObject(OpenMode.ForWrite) as Table;
    //                            bom.SetRowTextString(0, "???? Face Show");
    //                            bom.Rows[0].ContentColor = Autodesk.AutoCAD.Colors.Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.ByAci, 6);
    //                            bom.SetRowTextString(1, "AssemblyName", "P", "C", "S");
    //                            bom.Rows[1].ContentColor = Autodesk.AutoCAD.Colors.Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.ByAci, 2);
    //                            bom.SetRowTextString(2, tgirderPart.WebName.Substring(0, tgirderPart.WebName.LastIndexOf("-")), "0", "1", "0");
    //                            bom.Rows[2].ContentColor = Autodesk.AutoCAD.Colors.Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.ByAci, 3);
    //                            bom.SetRowTextString(3, "Piece Mark", "Thickness", "Material", "Area");
    //                            bom.Rows[3].ContentColor = Autodesk.AutoCAD.Colors.Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.ByAci, 2);
    //                            //----> 面积Field表达式：   %<\AcObjProp Object(%<\_ObjId 868629520>%).Area  \f "%lu2%pr3">%
    //                            //----> 文字内容Field表达式： %<\AcObjProp Object(%<\_ObjId 922480288 >%).TextString >%
    //                            bom.SetRowTextString(4, $"%<\\AcObjProp Object(%<\\_ObjId {tgirderPart.webNameTextoid.OldIdPtr.ToInt64()} >%).TextString >%", tgirderPart.Thickness, tgirderPart.Grade,
    //                                $"%<\\AcObjProp Object(%<\\_ObjId {tgirderPart.webPlateShapeFinally.OldIdPtr.ToInt64()}>%).Area \\f \"%lu2%pr3\">%");
    //                            bom.Rows[4].ContentColor = Autodesk.AutoCAD.Colors.Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.ByAci, 4);
    //                            if (tgirderPart.curGirderFacePlates.Count > 0)
    //                            {
    //                                for (int k = 0; k < tgirderPart.curGirderFacePlates.Count; k++)
    //                                {
    //                                    bom.SetRowTextString(k + 5, $"%<\\AcObjProp Object(%<\\_ObjId {tgirderPart.curGirderFacePlates[k].FaceNameTextoid.OldIdPtr.ToInt64()} >%).TextString >%", tgirderPart.curGirderFacePlates[k].Thickness, tgirderPart.curGirderFacePlates[k].Grade, $"%<\\AcObjProp Object(%<\\_ObjId {tgirderPart.curGirderFacePlates[k].facePlateShapeFinally.OldIdPtr.ToInt64()}>%).Area \\f \"%lu2%pr3\">%");
    //                                    bom.Rows[k + 5].ContentColor = Autodesk.AutoCAD.Colors.Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.ByAci, 4);
    //                                }
    //                            }
    //                            pnt = new Point3d(pnt.X + (310 * titleblkScale), pnt.Y, pnt.Z);
    //                        }
    //                        trans.Commit();
    //                    }
    //                }
    //                #endregion
    //            }
    //            else Autodesk.AutoCAD.ApplicationServices.Core.Application.ShowAlertDialog("没有发现任何 T-girder构件！");
    //        }
    //    }

    //    [myAutoCADcmd("生成Bracket的下料图", "SD_BracketDrawing.png")]
    //    [CommandMethod("SD_BracketDrawing", CommandFlags.Session)]
    //    public void KFWHSD_根据detail和section生成肘板PlateCutting图纸()
    //    {
    //        using (DocumentLock dl = this.acDoc.LockDocument())
    //        {
    //            //将用户坐标系转换成世界坐标系
    //            if (Application.GetSystemVariable("WORLDUCS").ToString() != "1") { this.acEd.CurrentUserCoordinateSystem = Matrix3d.Identity; this.acEd.Regen(); }
    //            //设置颜色样式
    //            if (Application.GetSystemVariable("CECOLOR").ToString() != "BYLAYER") Application.SetSystemVariable("CECOLOR", "BYLAYER");
    //            //
    //            this.acDb.MyCreateLayer("NC-CUTTING", 1, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-LABEL", 221, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-MARKING", 3, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-MARKING_TXT", 4, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-BEVEL", 42, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);

    //            var pso = new PromptSelectionOptions() { MessageForAdding = "选择A3的图框，主要有section 和一些detail 节点" };
    //            var psr = this.acEd.GetSelection(pso);
    //            List<SD_SmallPlatePart> listBrackets = new List<SD_SmallPlatePart>();

    //            //肘板形状的筛选条件
    //            TypedValueList valBracket = new TypedValueList();
    //            valBracket.Add(DxfCode.LayerName, "NC-CUTTING");
    //            valBracket.Add(typeof(Polyline));
    //            valBracket.Add(DxfCode.Color, 150);

    //            #region //提取符合条件的肘板
    //            if (psr.Status == PromptStatus.OK)
    //            {
    //                using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //                {
    //                    foreach (SelectedObject selObj in psr.Value)
    //                    {
    //                        Entity ent = selObj.ObjectId.GetObject(OpenMode.ForRead) as Entity;
    //                        if (ent is BlockReference)
    //                        {
    //                            var blkref = ent as BlockReference;
    //                            if (blkref.IsDynamicBlock)
    //                            {
    //                                var blk = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
    //                                if (blk.Name == "KFWH_CP_SD_A3_H" || blk.Name == "KFWH_CP_SD_A3_V")
    //                                {
    //                                    TypedValueList valBracketML = new TypedValueList();
    //                                    valBracketML.Add(typeof(MLeader));
    //                                    var psr_Bracket = this.acEd.SelectWindow(ent.GeometricExtents.MinPoint, ent.GeometricExtents.MaxPoint, new SelectionFilter(valBracket));
    //                                    if (psr_Bracket.Status == PromptStatus.OK)
    //                                    {
    //                                        foreach (SelectedObject brakcetoid in psr_Bracket.Value)
    //                                        {
    //                                            var brakcetPline = brakcetoid.ObjectId.GetObject(OpenMode.ForRead) as Polyline;
    //                                            var psr_Ml = this.acEd.SelectCrossingPolygon(brakcetPline.GetAllPoints(), new SelectionFilter(valBracketML));
    //                                            if (psr_Ml.Status == PromptStatus.OK)
    //                                            {
    //                                                foreach (SelectedObject mloid in psr_Ml.Value)
    //                                                {
    //                                                    var ml = mloid.ObjectId.GetObject(OpenMode.ForRead) as MLeader;
    //                                                    var pntMl = ml.GetFirstVertex(0);
    //                                                    if ((ml.MLeaderStyle.GetObject(OpenMode.ForRead) as MLeaderStyle).Name == "SectionPartML_Basic")
    //                                                    {
    //                                                        if (PointIsInPolyline.PtRelationToPoly(brakcetPline, new Point2d(pntMl.X, pntMl.Y), 0.00001) == 1)
    //                                                        {
    //                                                            listBrackets.Add(new SD_SmallPlatePart(mloid.ObjectId, brakcetoid.ObjectId, this.acDb));
    //                                                        }
    //                                                    }
    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                var psr_Ml1 = this.acEd.SelectCrossingWindow(brakcetPline.GeometricExtents.MinPoint, brakcetPline.GeometricExtents.MaxPoint, new SelectionFilter(valBracketML));
    //                                                if (psr_Ml1.Status == PromptStatus.OK)
    //                                                {
    //                                                    foreach (SelectedObject mloid in psr_Ml1.Value)
    //                                                    {
    //                                                        var ml = mloid.ObjectId.GetObject(OpenMode.ForRead) as MLeader;
    //                                                        var pntMl = ml.GetFirstVertex(0);
    //                                                        if ((ml.MLeaderStyle.GetObject(OpenMode.ForRead) as MLeaderStyle).Name == "SectionPartML_Basic")
    //                                                        {
    //                                                            if (PointIsInPolyline.PtRelationToPoly(brakcetPline, new Point2d(pntMl.X, pntMl.Y), 0.00001) == 1)
    //                                                            {
    //                                                                listBrackets.Add(new SD_SmallPlatePart(mloid.ObjectId, brakcetoid.ObjectId, this.acDb));
    //                                                            }
    //                                                        }
    //                                                    }
    //                                                }
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                    trans.Commit();
    //                }
    //            }
    //            #endregion

    //            #region //插入肘板
    //            if (listBrackets.Count > 0)
    //            {
    //                var sortlistBrackets = listBrackets.OrderBy(c => c.Name).ToList();

    //                var ppr = this.acEd.GetPoint(new PromptPointOptions("拾取一个点放置下料图："));
    //                if (ppr.Status == PromptStatus.OK)
    //                {
    //                    Point3d pnt = ppr.Value;
    //                    var TitbleBlockoid = this.acDb.InsertKFWHSDBlkstoCurDoc("KFWH_CP_SD_A4");
    //                    var ncinforBlockoid = this.acDb.InsertKFWHSDBlkstoCurDoc("KFWH_CP_SD_NCPartsInforTable");
    //                    using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //                    {
    //                        BlockTable bt = trans.GetObject(this.acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
    //                        BlockTableRecord modelspace = bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite) as BlockTableRecord;
    //                        for (int i = 0; i < sortlistBrackets.Count; i += 2)
    //                        {
    //                            var tbsc = GetA4TitleBlocksScale(sortlistBrackets[i].Length * 2);
    //                            ObjectId tbblkrefid = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_A4", pnt, new Scale3d(tbsc, tbsc, tbsc), 0);
    //                            Dictionary<string, string> dictb = new Dictionary<string, string>();
    //                            dictb["MATL"] = "Bracket"; dictb["GRADE"] = "AS SHOWN"; dictb["SIZE"] = "AS SHOWN";
    //                            tbblkrefid.UpdateAttributesInBlock(dictb);
    //                            var pnt_FirstBkt = new Point3d(pnt.X + tbsc * 90, pnt.Y + 100 * tbsc, pnt.Z);
    //                            AddedBketParts2CurTitleBlock(tbsc, modelspace, pnt_FirstBkt, sortlistBrackets[i]);
    //                            if (i != sortlistBrackets.Count - 1)
    //                            {
    //                                var pnt_SecBkt = new Point3d(pnt.X + tbsc * 190, pnt.Y + 100 * tbsc, pnt.Z);
    //                                AddedBketParts2CurTitleBlock(tbsc, modelspace, pnt_SecBkt, sortlistBrackets[i + 1]);
    //                            }
    //                            pnt = new Point3d(pnt.X + tbsc * 350, pnt.Y, pnt.Z);
    //                        }
    //                        trans.Commit();
    //                    }
    //                }
    //            }
    //            #endregion
    //            //
    //        }
    //    }
    //    public void AddedParts2CurTitleBlock(double titleblkScale, BlockTableRecord modelspace, Point3d part1InsPnt, SD_BigPlateLayoutPart curPart1)
    //    {
    //        var partBoundary = curPart1.Partshape.GetObject(OpenMode.ForWrite) as Polyline;
    //        Point3d partsRotaionCenPnt = MyTools.GetCpnt(partBoundary);

    //        var pprcurPartInternalEntitys = this.acEd.SelectWindowPolygon(partBoundary.GetAllPoints());//

    //        ObjectId partNameTxtOid = ObjectId.Null;
    //        DBText PartNameText;
    //        if (pprcurPartInternalEntitys.Status == PromptStatus.OK)
    //        {
    //            foreach (SelectedObject obj in pprcurPartInternalEntitys.Value)
    //            {
    //                var PartInternalEntity = obj.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
    //                PartInternalEntity.MyRotate(partsRotaionCenPnt, Math.PI * 2 - curPart1.OptimizationAngle);
    //                PartInternalEntity.MyMove(part1InsPnt, partsRotaionCenPnt);
    //                if (PartInternalEntity is MLeader) PartInternalEntity.Erase();
    //            }
    //        }
    //        PartNameText = new DBText() { Layer = "0", ColorIndex = 35, Position = part1InsPnt, Rotation = 0, Height = 2.5 * titleblkScale, TextString = curPart1.PartName };

    //        partNameTxtOid = this.acDb.AddToCurrentSpace(PartNameText);

    //        partBoundary.MyRotate(partsRotaionCenPnt, Math.PI * 2 - curPart1.OptimizationAngle);

    //        partBoundary.MyMove(part1InsPnt, partsRotaionCenPnt);

    //        foreach (ObjectId oid in curPart1.markingsIds)
    //        {
    //            oid.MyRotate(partsRotaionCenPnt, Math.PI * 2 - curPart1.OptimizationAngle);
    //            oid.MyMove(part1InsPnt, partsRotaionCenPnt);
    //        }
    //        //插入零件信息
    //        var cutPartInfoPnt = new Point3d(part1InsPnt.X, partBoundary.GeometricExtents.MinPoint.Y - (2 * titleblkScale), part1InsPnt.Z);
    //        var ncinfortbblkrefid = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_NCPartsInforTable", cutPartInfoPnt, new Scale3d(titleblkScale, titleblkScale, titleblkScale), 0);
    //        #region//写入零件信息到动态块中。
    //        Dictionary<string, string> dicPartInfoDic = new Dictionary<string, string>();
    //        //%<\AcObjProp Object(%<\_ObjId 932283008>%).TextString>%
    //        //%<\AcObjProp Object(%<\_ObjId 882847488>%).TextString >%
    //        //%<\AcObjProp Object(%<\_ObjId 1281317136>%).TextString>%
    //        dicPartInfoDic["PARTNAME"] = $"%<\\AcObjProp Object(%<\\_ObjId {partNameTxtOid.OldIdPtr.ToInt64()}>%).TextString>%";

    //        dicPartInfoDic["THICKNESS"] = curPart1.Thickness;

    //        dicPartInfoDic["MATERIALGRADE"] = curPart1.MaterialGrade;

    //        dicPartInfoDic["P"] = "0";

    //        dicPartInfoDic["C"] = "1";

    //        dicPartInfoDic["S"] = "0";

    //        ncinfortbblkrefid.UpdateAttributesInBlock(dicPartInfoDic);

    //        #endregion
    //    }

    //    public void AddedBketParts2CurTitleBlock(double titleblkScale, BlockTableRecord modelspace, Point3d partInsPnt, SD_SmallPlatePart curPart)
    //    {
    //        var partBoundary = curPart.PlateShapeOld.GetObject(OpenMode.ForWrite) as Polyline;
    //        partBoundary.ColorIndex = 256;
    //        curPart.PlateShapeFinally = partBoundary.Copy(partInsPnt, partBoundary.GeometricExtents.MinPoint);
    //        var newPartBoundary = curPart.PlateShapeFinally.GetObject(OpenMode.ForRead) as Polyline;
    //        newPartBoundary.ColorIndex = 256;
    //        ObjectId partNameTxtOid = ObjectId.Null;
    //        DBText PartNameText;
    //        PartNameText = new DBText()
    //        {
    //            Layer = "0",
    //            ColorIndex = 35,
    //            Position = new Point3d(partInsPnt.X + curPart.Length / 2, partInsPnt.Y + curPart.Width / 2, partInsPnt.Z),
    //            Rotation = 0,
    //            Height = 2.5 * titleblkScale,
    //            TextString = curPart.Name
    //        };
    //        partNameTxtOid = this.acDb.AddToCurrentSpace(PartNameText);

    //        //插入零件信息
    //        var ncinfortbblkrefid = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_NCPartsInforTable", new Point3d(partInsPnt.X + curPart.Length / 2, partInsPnt.Y - titleblkScale * 20, partInsPnt.Z), new Scale3d(titleblkScale, titleblkScale, titleblkScale), 0);
    //        #region//写入零件信息到动态块中。
    //        Dictionary<string, string> dicPartInfoDic = new Dictionary<string, string>();
    //        dicPartInfoDic["PARTNAME"] = $"%<\\AcObjProp Object(%<\\_ObjId {partNameTxtOid.OldIdPtr.ToInt64()}>%).TextString>%";

    //        dicPartInfoDic["THICKNESS"] = curPart.Thickness;

    //        dicPartInfoDic["MATERIALGRADE"] = curPart.Grade;

    //        dicPartInfoDic["P"] = "0";

    //        dicPartInfoDic["C"] = "0";

    //        dicPartInfoDic["S"] = "0";

    //        ncinfortbblkrefid.UpdateAttributesInBlock(dicPartInfoDic);

    //        #endregion
    //    }
    //    public static int GetA4TitleBlocksScale(double partLength)
    //    {
    //        var sc = partLength / 130;
    //        int i = (int)Math.Ceiling(sc / 5);
    //        return i * 5;
    //    }
    //    public ObjectId MyInsertMoudleLineSymbol(Point3d pnt, Database curDb, double scale, MoudleLineType mlType, double rotationAngle)
    //    {
    //        ObjectId oid = ObjectId.Null;
    //        ObjectId oidBlkmlSymbol = MySQLHelper.InsertKFWHSDBlkstoCurDoc(curDb, "KFWH_CP_SD_ML");
    //        using (Transaction trans = curDb.TransactionManager.StartTransaction())
    //        {
    //            BlockTable bt = trans.GetObject(curDb.BlockTableId, OpenMode.ForRead) as BlockTable;
    //            BlockTableRecord modelspace = bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite) as BlockTableRecord;
    //            oid = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_ML", pnt, new Scale3d(scale, scale, scale), rotationAngle);
    //            var blkref = oid.GetObject(OpenMode.ForWrite) as BlockReference;
    //            foreach (DynamicBlockReferenceProperty item in blkref.DynamicBlockReferencePropertyCollection)
    //            {
    //                if (item.PropertyName == "Type") item.Value = mlType.ToString();
    //                if (item.PropertyName == "Angle1") item.Value = Math.PI;
    //            }
    //            trans.Commit();
    //        }
    //        return oid;
    //    }
    //    public static string[] GetMoudleLineInfor(ObjectId oidBlkref)
    //    {
    //        string[] str = new string[3];
    //        using (Transaction trans = oidBlkref.Database.TransactionManager.StartTransaction())
    //        {
    //            var blkref = oidBlkref.GetObject(OpenMode.ForRead) as BlockReference;
    //            foreach (DynamicBlockReferenceProperty item in blkref.DynamicBlockReferencePropertyCollection)
    //            {
    //                if (item.PropertyName == "Type") str[0] = item.Value.ToString();
    //                if (item.PropertyName == "Angle1") str[1] = item.Value.ToString();
    //                if (item.PropertyName == "ThkMirror") str[2] = item.Value.ToString();
    //            }
    //            trans.Commit();
    //        }
    //        return str;
    //    }
    //    public enum MoudleLineType
    //    {
    //        Plate_Mid, Plate_Left, Angle_Left, Angle_Thk
    //    }
    //}
    //public class Nestix零件工具
    //{
    //    public Document acDoc = Application.DocumentManager.MdiActiveDocument;
    //    public Database acDb = Application.DocumentManager.MdiActiveDocument.Database;
    //    public Editor acEd = Application.DocumentManager.MdiActiveDocument.Editor;

    //    [myAutoCADcmd("根据A4下料图生成Nestix格式的Dxf文件", "SD_CreateNestixParts.png")]
    //    [CommandMethod("SD_CreateNestixParts", CommandFlags.Session)]
    //    public void KFWHSD_PlateCutting转Nestix()
    //    {
    //        using (DocumentLock dl = this.acDoc.LockDocument())
    //        {
    //            //将用户坐标系转换成世界坐标系
    //            if (Application.GetSystemVariable("WORLDUCS").ToString() != "1") { this.acEd.CurrentUserCoordinateSystem = Matrix3d.Identity; this.acEd.Regen(); }
    //            //设置颜色样式
    //            if (Application.GetSystemVariable("CECOLOR").ToString() != "BYLAYER") Application.SetSystemVariable("CECOLOR", "BYLAYER");
    //            //
    //            this.acDb.MyCreateLayer("NC-CUTTING", 1, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-LABEL", 221, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-MARKING", 3, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-MARKING_TXT", 4, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            this.acDb.MyCreateLayer("NC-BEVEL", 42, Autodesk.AutoCAD.Colors.ColorMethod.ByAci, true);
    //            Point3dCollection pnts = new Point3dCollection();
    //            PromptPointOptions ppo = new PromptPointOptions("选取需要转化为Nestix格式的图框所在的区域的左下角:\n");
    //            string curPanelName = MyHelper.GetCurShopDrawingPanelName();
    //            var ppr = this.acEd.GetPoint(ppo);
    //            if (ppr.Status == PromptStatus.OK)
    //            {
    //                var ppr1 = this.acEd.GetCorner("选取需要转化为Nestix格式的图框所在的区域的右上角:：\n", ppr.Value);
    //                pnts.Add(ppr.Value);
    //                if (ppr1.Status == PromptStatus.OK) pnts.Add(ppr1.Value);
    //            }
    //            if (pnts.Count != 2) return;
    //            List<NestixPart> listNestixParts = new List<NestixPart>();
    //            using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //            {
    //                BlockTable bt = trans.GetObject(acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
    //                BlockTableRecord ms = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
    //                List<BlockReference> listTitleBlks = new List<BlockReference>();
    //                TypedValueList list = new TypedValueList();
    //                list.Add(typeof(BlockReference));
    //                var psr = this.acEd.SelectWindow(pnts[0], pnts[1], new SelectionFilter(list));
    //                #region//获取用户指定范围内的所有图框
    //                if (psr.Status == PromptStatus.OK)
    //                {
    //                    foreach (SelectedObject sid in psr.Value)
    //                    {
    //                        Entity ent = sid.ObjectId.GetObject(OpenMode.ForRead) as Entity;
    //                        if (ent is BlockReference)
    //                        {
    //                            BlockReference blkref = ent as BlockReference;
    //                            if (blkref.IsDynamicBlock)
    //                            {
    //                                var blk = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
    //                                if (blk.Name == "KFWH_CP_SD_A4") listTitleBlks.Add(blkref);
    //                            }
    //                        }
    //                    }
    //                }
    //                #endregion

    //                if (listTitleBlks.Count > 0)
    //                {
    //                    #region// 提取零件信息
    //                    //提取零件块信息的筛选器
    //                    TypedValueList listNCinforBlks = new TypedValueList();
    //                    listNCinforBlks.Add(typeof(BlockReference));
    //                    //提取零件图形的筛选器
    //                    TypedValueList listNCParts = new TypedValueList();
    //                    listNCParts.Add(typeof(Polyline));
    //                    listNCParts.Add(DxfCode.LayerName, "NC-CUTTING");
    //                    listNCParts.Add(DxfCode.Color, 256);
    //                    //提取零件表格，T-girder的筛选器
    //                    TypedValueList listNCinforTbs = new TypedValueList();
    //                    listNCinforTbs.Add(typeof(Table));

    //                    //遍历各个图框提取图形和信息
    //                    for (int i = 0; i < listTitleBlks.Count; i++)
    //                    {
    //                        var tbblkref = listTitleBlks[i];
    //                        List<Polyline> ncPartsShape = new List<Polyline>();//存储当前图框内部的零件的形状
    //                        List<Table> ncPartsNCinforTbs = new List<Table>();//存储当前图框内部的零件的信息的表格
    //                        List<BlockReference> ncPartsNCinforBlks = new List<BlockReference>();//存储当前图框内部的零件的信息的图块 
    //                        #region//获取当前图框内部零件形状和信息的集合
    //                        var psr_PartShape = this.acEd.SelectWindow(tbblkref.GeometricExtents.MinPoint, tbblkref.GeometricExtents.MaxPoint, new SelectionFilter(listNCParts));

    //                        if (psr_PartShape.Status == PromptStatus.OK)
    //                        {
    //                            foreach (SelectedObject item in psr_PartShape.Value) ncPartsShape.Add(item.ObjectId.GetObject(OpenMode.ForRead) as Polyline);
    //                        }

    //                        var psr_NCinforBlk = this.acEd.SelectWindow(tbblkref.GeometricExtents.MinPoint, tbblkref.GeometricExtents.MaxPoint, new SelectionFilter(listNCinforBlks));

    //                        if (psr_NCinforBlk.Status == PromptStatus.OK)
    //                        {
    //                            foreach (SelectedObject item in psr_NCinforBlk.Value)
    //                            {
    //                                if (item.ObjectId.GetBlockReferenceName() == "KFWH_CP_SD_NCPartsInforTable") ncPartsNCinforBlks.Add(item.ObjectId.GetObject(OpenMode.ForRead) as BlockReference);
    //                            }
    //                        }

    //                        var psr_NCinforTbs = this.acEd.SelectWindow(tbblkref.GeometricExtents.MinPoint, tbblkref.GeometricExtents.MaxPoint, new SelectionFilter(listNCinforTbs));
    //                        if (psr_NCinforTbs.Status == PromptStatus.OK)
    //                        {
    //                            foreach (SelectedObject item in psr_NCinforTbs.Value)
    //                            {
    //                                Table tb = item.ObjectId.GetObject(OpenMode.ForRead) as Table;
    //                                if (tb.TableStyleName == "KFWHSDTGBOM") ncPartsNCinforTbs.Add(tb);
    //                            }
    //                        }
    //                        #endregion

    //                        if (ncPartsShape.Count > 0 && (ncPartsNCinforBlks.Count > 0 || ncPartsNCinforTbs.Count > 0))
    //                        {
    //                            #region//获取当前图框的零件
    //                            TypedValueList typeValList = new TypedValueList();
    //                            typeValList.Add(typeof(DBText));
    //                            typeValList.Add(DxfCode.Color, 35);
    //                            typeValList.Add(DxfCode.LayerName, "0");
    //                            string partName = string.Empty;
    //                            if (ncPartsNCinforTbs.Count > 0)
    //                            {
    //                                foreach (var item in ncPartsShape) listNestixParts.Add(new NestixPart(item.ObjectId, ncPartsNCinforTbs[0].ObjectId));
    //                            }
    //                            else
    //                            {
    //                                foreach (var item in ncPartsShape)
    //                                {
    //                                    var psr_Text = this.acEd.SelectWindowPolygon(item.GetAllPoints(), new SelectionFilter(typeValList));
    //                                    if (psr_Text.Status == PromptStatus.OK)
    //                                    {
    //                                        partName = (psr_Text.Value[0].ObjectId.GetObject(OpenMode.ForRead) as DBText).TextString;
    //                                        var blkOid = ncPartsNCinforBlks.Find(c => c.ObjectId.GetAttributeInBlockReference("PARTNAME") == partName);
    //                                        if (blkOid != null) listNestixParts.Add(new NestixPart(item.ObjectId, blkOid.ObjectId)); else this.acEd.WriteMessage($"{partName} 名称对不上，请检查名称！");
    //                                    }
    //                                    else
    //                                    {
    //                                        KFWH_CMD.Tools.AddToModelSpace(this.acDb, new Line(item.GetCpnt(), Point3d.Origin));
    //                                        Application.ShowAlertDialog($"请检查拉线的零件内部的35号色，0层的代表零件名称的文字是否存在！");
    //                                    }
    //                                }
    //                            }
    //                            #endregion
    //                        }
    //                    }
    //                    #endregion
    //                }
    //                trans.Commit();
    //            }

    //            if (listNestixParts.Count > 0)
    //            {
    //                listNestixParts.ForEach(c => c.PanelName = curPanelName);
    //                PromptDistanceOptions pdo = new PromptDistanceOptions("")
    //                { BasePoint = pnts[0], Only2d = true };
    //                var ppr_New = this.acEd.GetDistance(pdo);
    //                if (ppr_New.Status == PromptStatus.OK)
    //                {
    //                    Point3d pnt = new Point3d(pnts[0].X + ppr_New.Value, pnts[0].Y, pnts[0].Z);
    //                    using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //                    {
    //                        foreach (NestixPart item in listNestixParts)
    //                        {
    //                            item.NestixPartOID = item.PlateCuttingPartOID.Copy(pnt, pnts[0]);
    //                            if (item.PlateCuttingPartInternalOIDs != null)
    //                            {
    //                                item.NestixPartInternalOIDs = new ObjectIdCollection();
    //                                foreach (ObjectId oid in item.PlateCuttingPartInternalOIDs)
    //                                {
    //                                    item.NestixPartInternalOIDs.Add(oid.Copy(pnt, pnts[0]));
    //                                }
    //                                for (int i = 0; i < item.NestixPartInternalOIDs.Count; i++)
    //                                {
    //                                    Entity entInternal = item.NestixPartInternalOIDs[i].GetObject(OpenMode.ForRead) as Entity;
    //                                    entInternal.UpgradeOpen();
    //                                    entInternal.ColorIndex = 256;
    //                                    entInternal.DowngradeOpen();
    //                                }
    //                            }
    //                            var ent = item.NestixPartOID.GetObject(OpenMode.ForRead) as Polyline;
    //                            var oidPartNameText = item.PartNameTextOid.Copy(pnt, pnts[0]);
    //                            var txt = oidPartNameText.GetObject(OpenMode.ForRead) as DBText;
    //                            double angle = txt.Rotation;
    //                            DBText txt_Name, txt_Thk, txt_Matl, txt_Qty, txt_Side;

    //                            txt_Name = new DBText() { Layer = "NC-LABEL", TextString = "N: " + item.PanelName + "-" + item.PartName, Height = txt.Height / 5, ColorIndex = 256, Position = new Point3d(txt.Position.X, txt.Position.Y + txt.Height * 0.6, txt.Position.Z) };

    //                            txt_Thk = new DBText() { Layer = "NC-LABEL", TextString = "T: " + item.Thickness, Height = txt.Height / 5, ColorIndex = 256, Position = new Point3d(txt.Position.X, txt.Position.Y + txt.Height * 0.3, txt.Position.Z) };

    //                            txt_Matl = new DBText() { Layer = "NC-LABEL", TextString = "M: " + item.Material, Height = txt.Height / 5, ColorIndex = 256, Position = new Point3d(txt.Position.X, txt.Position.Y, txt.Position.Z) };

    //                            switch (item.Side)
    //                            {
    //                                case "P":
    //                                    txt_Qty = new DBText() { Layer = "NC-LABEL", TextString = "Q: " + item.PQty, Height = txt.Height / 5, ColorIndex = 256, Position = new Point3d(txt.Position.X, txt.Position.Y - txt.Height * 0.3, txt.Position.Z) };
    //                                    break;
    //                                case "C":
    //                                    txt_Qty = new DBText() { Layer = "NC-LABEL", TextString = "Q: " + item.CQty, Height = txt.Height / 5, ColorIndex = 256, Position = new Point3d(txt.Position.X, txt.Position.Y - txt.Height * 0.3, txt.Position.Z) };
    //                                    break;
    //                                case "S":
    //                                    txt_Qty = new DBText() { Layer = "NC-LABEL", TextString = "Q: " + item.SQty, Height = txt.Height / 5, ColorIndex = 256, Position = new Point3d(txt.Position.X, txt.Position.Y - txt.Height * 0.3, txt.Position.Z) };
    //                                    break;
    //                                default:
    //                                    txt_Qty = new DBText() { Layer = "NC-LABEL", TextString = "Q: ", Height = txt.Height / 5, ColorIndex = 256, Position = new Point3d(txt.Position.X, txt.Position.Y - txt.Height * 0.3, txt.Position.Z) };
    //                                    break;
    //                            }
    //                            txt_Side = new DBText() { Layer = "NC-LABEL", TextString = "SI: " + item.Side, Height = txt.Height / 5, ColorIndex = 256, Position = new Point3d(txt.Position.X, txt.Position.Y - txt.Height * 0.6, txt.Position.Z) };

    //                            KFWH_CMD.Tools.AddToModelSpace(this.acDb, txt_Name, txt_Thk, txt_Matl, txt_Qty, txt_Side);
    //                        }
    //                        trans.Commit();
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    ////public class 动态块工具
    ////{
    ////    public Document acDoc = Application.DocumentManager.MdiActiveDocument;
    ////    public Database acDb = Application.DocumentManager.MdiActiveDocument.Database;
    ////    public Editor acEd = Application.DocumentManager.MdiActiveDocument.Editor;
    ////    //[myAutoCADcmd("Update shop drawing title blcok information as per panel what you select", "SD_PanelInfo.png")]
    ////    //[CommandMethod("SD_EndCutDynamicBlockUpdate")]
    ////    public void KFWHSD_型材端部型式动态块一键更新()
    ////    {
    ////        //将用户坐标系转换成世界坐标系
    ////        if (Application.GetSystemVariable("WORLDUCS").ToString() != "1") { this.acEd.CurrentUserCoordinateSystem = Matrix3d.Identity; this.acEd.Regen(); }
    ////        //设置颜色样式
    ////        if (Application.GetSystemVariable("CECOLOR").ToString() != "BYLAYER") Application.SetSystemVariable("CECOLOR", "BYLAYER");
    ////        //
    ////        using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    ////        {
    ////            BlockTable bt = trans.GetObject(acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
    ////            BlockTableRecord ms = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
    ////            List<BlockReference> listmyDynamicBlks = new List<BlockReference>();
    ////            foreach (var oid in ms)
    ////            {
    ////                Entity ent = oid.GetObject(OpenMode.ForRead) as Entity;
    ////                if (ent is BlockReference)
    ////                {
    ////                    if (oid.GetBlockReferenceName() == "KFWH_CP_SD_AngleBarEndCut" || oid.GetBlockReferenceName() == "KFWH_CP_SD_IBEAMEndCut")
    ////                    {
    ////                        listmyDynamicBlks.Add(ent as BlockReference);
    ////                    }
    ////                }
    ////            }
    ////            if (listmyDynamicBlks.Count > 0)
    ////            {
    ////                for (int i = 0; i < listmyDynamicBlks.Count; i++)
    ////                {
    ////                    var blkref = listmyDynamicBlks[i];
    ////                    var oid = this.acDb.InsertKFWHSDBlkstoCurDoc(blkref.ObjectId.GetBlockReferenceName(), true);
    ////                    if (blkref.ObjectId.GetBlockReferenceName() == "KFWH_CP_SD_AngleBarEndCut")
    ////                    {

    ////                        string AngleBarEndcutType = string.Empty;
    ////                        short RightOrLeft = short.MinValue;
    ////                        foreach (DynamicBlockReferenceProperty dbrp in blkref.DynamicBlockReferencePropertyCollection)
    ////                        {
    ////                            switch (dbrp.PropertyName)
    ////                            {
    ////                                case "AngleBarEndcutType":
    ////                                    AngleBarEndcutType = dbrp.Value.ToString();
    ////                                    break;
    ////                                case "RightOrLeft":
    ////                                    RightOrLeft = (short)dbrp.Value;
    ////                                    break;
    ////                            }
    ////                        }
    ////                        blkref.UpgradeOpen();
    ////                        blkref.ResetBlock();
    ////                        foreach (DynamicBlockReferenceProperty dbrp in blkref.DynamicBlockReferencePropertyCollection)
    ////                        {
    ////                            switch (dbrp.PropertyName)
    ////                            {
    ////                                case "AngleBarEndcutType":
    ////                                    dbrp.Value = AngleBarEndcutType;
    ////                                    break;
    ////                                case "RightOrLeft":
    ////                                    dbrp.Value = RightOrLeft;
    ////                                    break;
    ////                            }
    ////                        }
    ////                        blkref.DowngradeOpen();
    ////                    }
    ////                    else
    ////                    {
    ////                        string IbeamWebCorner = string.Empty;
    ////                        short RightOrLeft = short.MinValue;
    ////                        short UpORBottom = short.MinValue;
    ////                        foreach (DynamicBlockReferenceProperty dbrp in blkref.DynamicBlockReferencePropertyCollection)
    ////                        {
    ////                            switch (dbrp.PropertyName)
    ////                            {
    ////                                case "IbeamWebCorner":
    ////                                    IbeamWebCorner = dbrp.Value.ToString();
    ////                                    break;
    ////                                case "RightOrLeft":
    ////                                    RightOrLeft = (short)dbrp.Value;
    ////                                    break;
    ////                                case "UpORBottom":
    ////                                    UpORBottom = (short)dbrp.Value;
    ////                                    break;
    ////                            }
    ////                        }
    ////                        blkref.UpgradeOpen();
    ////                        blkref.ResetBlock();
    ////                        foreach (DynamicBlockReferenceProperty dbrp in blkref.DynamicBlockReferencePropertyCollection)
    ////                        {
    ////                            switch (dbrp.PropertyName)
    ////                            {
    ////                                case "IbeamWebCorner":
    ////                                    dbrp.Value = IbeamWebCorner;
    ////                                    break;
    ////                                case "RightOrLeft":
    ////                                    dbrp.Value = RightOrLeft;
    ////                                    break;
    ////                                case "UpORBottom":
    ////                                    dbrp.Value = UpORBottom;
    ////                                    break;
    ////                            }
    ////                        }
    ////                        blkref.DowngradeOpen();
    ////                    }
    ////                }
    ////                Application.ShowAlertDialog("型材的端部型式动态图块更新完毕！");
    ////            }
    ////            trans.Commit();
    ////        }

    ////    }
    ////}

    //public class 型材节点图
    //{
    //    public Document acDoc = Application.DocumentManager.MdiActiveDocument;
    //    public Database acDb = Application.DocumentManager.MdiActiveDocument.Database;
    //    public Editor acEd = Application.DocumentManager.MdiActiveDocument.Editor;

    //    [myAutoCADcmd("根据Panel Sketch图纸生成profile sketch 图纸", "SD_ProfileSketch.png")]
    //    [CommandMethod("SD_ProfileSketch")]
    //    public void KFWHSD_根据panelsketch生成型材节点图()

    //    {
    //        if (Application.GetSystemVariable("WORLDUCS").ToString() != "1") { this.acEd.CurrentUserCoordinateSystem = Matrix3d.Identity; this.acEd.Regen(); }
    //        //设置颜色样式
    //        if (Application.GetSystemVariable("CECOLOR").ToString() != "BYLAYER") Application.SetSystemVariable("CECOLOR", "BYLAYER");
    //        List<KFWH_CP_SD_ProfileEntity> listProfileEnts = new List<KFWH_CP_SD_ProfileEntity>();
    //        KFWH_CP_SD_PanelSketchDirection directionSymbol = null;//方位图块
    //        PromptEntityOptions peo = new PromptEntityOptions("选择panel sketch的图框：\n");
    //        peo.SetRejectMessage("你选择的不是A3图框！");
    //        peo.AddAllowedClass(typeof(BlockReference), true);
    //        #region//选取A3图框,提取型材的信息
    //        using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //        {
    //            var per = this.acEd.GetEntity(peo);
    //            if (per.Status != PromptStatus.OK) return;
    //            if (per.ObjectId.GetBlockReferenceName() == "KFWH_CP_SD_A3_H" || per.ObjectId.GetBlockReferenceName() == "KFWH_CP_SD_A3_V")
    //            {
    //                TypedValueList tvforProfileEnt = new TypedValueList();
    //                tvforProfileEnt.Add(typeof(BlockReference));
    //                var pts = (per.ObjectId.GetObject(OpenMode.ForRead) as Entity).GeometricExtents;
    //                var psrProfileEnts = this.acEd.SelectCrossingWindow(pts.MinPoint, pts.MaxPoint, new SelectionFilter(tvforProfileEnt));
    //                if (psrProfileEnts.Status != PromptStatus.OK) { this.acEd.WriteMessage($"图框中找不到任何的块"); return; }
    //                foreach (SelectedObject selObj in psrProfileEnts.Value)
    //                {
    //                    if (selObj.ObjectId.GetBlockReferenceName() == "KFWH_CP_SD_PanelSketchDirection")
    //                    {
    //                        directionSymbol = new KFWH_CP_SD_PanelSketchDirection(selObj.ObjectId);
    //                    }
    //                }
    //                foreach (SelectedObject selObj in psrProfileEnts.Value)
    //                {
    //                    if (selObj.ObjectId.GetBlockReferenceName() == "KFWH_CP_SD_ProfileEntity")
    //                    {
    //                        if (directionSymbol != null)
    //                        {
    //                            listProfileEnts.Add(new KFWH_CP_SD_ProfileEntity(selObj.ObjectId, directionSymbol));
    //                        }
    //                        else
    //                        {
    //                            Application.ShowAlertDialog("Panel sketch 中缺少方位图块!请检查之后再运行此命令");
    //                            return;
    //                        }

    //                    }
    //                }
    //            }
    //        }
    //        #endregion
    //        //生成profile sketch图纸
    //        if (listProfileEnts.Count > 0)
    //        {
    //            List<KFWH_CP_SD_ProfileEntity> listProfiles = new List<KFWH_CP_SD_ProfileEntity>();
    //            #region//合并相同的零件
    //            for (int i = 0; i < listProfileEnts.Count; i++)
    //            {
    //                if (listProfiles.Count(c => c.PROFILENAME == listProfileEnts[i].PROFILENAME) > 0)//名称相同就认为是同一个零件
    //                {
    //                    var temp = listProfiles.First(c => c.PROFILENAME == listProfileEnts[i].PROFILENAME);
    //                    temp.Qty += 1;
    //                    listProfiles[listProfiles.IndexOf(temp)] = temp;
    //                }
    //                else
    //                {
    //                    listProfileEnts[i].Qty = 1;
    //                    listProfiles.Add(listProfileEnts[i]);
    //                }
    //            }
    //            var listPros = listProfiles.OrderBy(c => c.PROFILENAME).ToList();
    //            #endregion
    //            PromptPointOptions ppo = new PromptPointOptions("拾取一个点放置型材节点图纸：");
    //            using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //            {
    //                PromptPointResult ppr = this.acEd.GetPoint(ppo);
    //                if (ppr.Status != PromptStatus.OK) { this.acEd.WriteMessage($"用户未指定任何的点，终止程序！"); return; }
    //                Point3d insPnt = ppr.Value;
    //                //插入图块到数据库
    //                acDb.InsertKFWHSDBlkstoCurDoc("KFWH_CP_SD_A4");//KFWH_CP_SD_A4
    //                acDb.InsertKFWHSDBlkstoCurDoc("KFWH_CP_SD_ProfileInforTable");//KFWH_CP_SD_ProfileInforTable
    //                acDb.InsertKFWHSDBlkstoCurDoc("KFWH_CP_SD_ProfileSketch");//KFWH_CP_SD_ProfileSketch
    //                acDb.InsertKFWHSDBlkstoCurDoc("KFWH_CP_SD_AngleBarEndCut");//KFWH_CP_SD_AngleBarEndCut
    //                acDb.InsertKFWHSDBlkstoCurDoc("KFWH_CP_SD_DirectionArrow");//KFWH_CP_SD_DirectionArrow
    //                BlockTable bt = trans.GetObject(this.acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
    //                BlockTableRecord modelspace = bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite) as BlockTableRecord;
    //                for (int i = 0; i < listPros.Count; i += 2)
    //                {
    //                    //插入图框
    //                    ObjectId oidTb = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_A4", insPnt, new Scale3d(60, 60, 60), 0);
    //                    Dictionary<string, string> dictb = new Dictionary<string, string>();
    //                    dictb["MATL"] = "Profile"; dictb["GRADE"] = "AS SHOWN"; dictb["SIZE"] = "AS SHOWN"; dictb["NOTE:"] = string.Empty;
    //                    oidTb.UpdateAttributesInBlock(dictb);
    //                    KFWH_CP_SD_ProfileEntity firstPart = listPros[i];
    //                    InsertPS2A4(oidTb, true, firstPart);
    //                    if (i + 1 <= listPros.Count - 1)//不是最后一个构件
    //                    {
    //                        KFWH_CP_SD_ProfileEntity secondPart = listPros[i + 1];
    //                        InsertPS2A4(oidTb, false, secondPart);
    //                    }
    //                    insPnt = insPnt.Add(new Vector3d(22160, 0, 0));
    //                }
    //                trans.Commit();
    //            }
    //        }
    //        else Application.ShowAlertDialog("图框中找不到任何表示型材的动态图块！");
    //    }

    //    public void InsertPS2A4(ObjectId oidfortb, bool firstOrSecondPart, KFWH_CP_SD_ProfileEntity profilePart)
    //    {
    //        Database acDb = oidfortb.Database;
    //        using (Transaction trans = acDb.TransactionManager.StartTransaction())
    //        {
    //            BlockTable bt = trans.GetObject(this.acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
    //            BlockTableRecord modelspace = bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite) as BlockTableRecord;
    //            BlockReference tb = oidfortb.GetObject(OpenMode.ForWrite) as BlockReference;
    //            ObjectId oidPSinforTable = ObjectId.Null;//明细栏的objectid
    //            #region////  零件明细栏
    //            if (firstOrSecondPart)
    //            {
    //                oidPSinforTable = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_ProfileInforTable", new Point3d(tb.Position.X + 65 * 60, tb.Position.Y + 180 * 60, tb.Position.Z), new Scale3d(60, 60, 60), 0);
    //            }
    //            else
    //            {
    //                oidPSinforTable = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_ProfileInforTable", new Point3d(tb.Position.X + 65 * 60, tb.Position.Y + 90 * 60, tb.Position.Z), new Scale3d(60, 60, 60), 0);
    //            }
    //            Dictionary<string, string> dictb = new Dictionary<string, string>();
    //            dictb["PARTNAME"] = profilePart.PROFILENAME; dictb["SIZE"] = profilePart.SizeFieldExpression; dictb["GRADE"] = profilePart.GRADEFieldExpression;
    //            dictb["NET_LTH"] = profilePart.NetLengthFieldExpression; dictb["SHTNO"] = "N.A.";
    //            dictb["WEB_BEVCODE_A"] = profilePart.ENDABEVEL_WEBFieldExpression; dictb["WEB_BEVCODE_B"] = profilePart.ENDBBEVEL_WEBFieldExpression;

    //            dictb["FLG_BEVCODE_A"] = profilePart.ENDABEVEL_FLGFieldExpression; dictb["FLG_BEVCODE_B"] = profilePart.ENDBBEVEL_FLGFieldExpression;

    //            dictb["MARGIN_A"] = profilePart.ENDAMARGINFieldExpression; dictb["MARGIN_B"] = profilePart.ENDBMARGINFieldExpression; dictb["MARGIN"] = profilePart.ENDAMARGINFieldExpression + "+" + profilePart.ENDBMARGINFieldExpression;

    //            dictb["P"] = "0"; dictb["S"] = "0"; dictb["C"] = profilePart.Qty.ToString();

    //            oidPSinforTable.UpdateAttributesInBlock(dictb);

    //            BlockReference blkrefPsTable = oidPSinforTable.GetObject(OpenMode.ForRead) as BlockReference;

    //            blkrefPsTable.UpgradeOpen();

    //            foreach (DynamicBlockReferenceProperty item in blkrefPsTable.DynamicBlockReferencePropertyCollection)
    //            {
    //                if (item.PropertyName == "ProfileType")
    //                {
    //                    if (profilePart.PROFILENAME.StartsWith("HP")) item.Value = "BULBPLATE";
    //                    if (profilePart.PROFILENAME.StartsWith("AL")) item.Value = "ANGLEBAR";
    //                    if (profilePart.PROFILENAME.StartsWith("BF")) item.Value = "BULBPLATE";
    //                    break;
    //                }
    //            }
    //            blkrefPsTable.DowngradeOpen();
    //            #endregion
    //            //插节点图 
    //            ObjectId oidPS = ObjectId.Null; //节点图的objectid
    //            #region//插入节点图纸
    //            if (firstOrSecondPart)
    //            {
    //                oidPS = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_ProfileSketch", new Point3d(tb.Position.X + 88 * 60, tb.Position.Y + 118 * 60, tb.Position.Z), new Scale3d(60, 60, 60), 0);
    //            }
    //            else
    //            {
    //                oidPS = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_ProfileSketch", new Point3d(tb.Position.X + 88 * 60, tb.Position.Y + 22 * 60, tb.Position.Z), new Scale3d(60, 60, 60), 0);
    //            }
    //            BlockReference blkrefPs = oidPS.GetObject(OpenMode.ForRead) as BlockReference;
    //            blkrefPs.UpgradeOpen();
    //            foreach (DynamicBlockReferenceProperty item in blkrefPs.DynamicBlockReferencePropertyCollection)
    //            {
    //                if (item.PropertyName == "ProfileType")
    //                {
    //                    if (profilePart.PROFILENAME.StartsWith("HP")) item.Value = "BulbPlate";
    //                    if (profilePart.PROFILENAME.StartsWith("AL")) item.Value = "AngleBar";
    //                    if (profilePart.PROFILENAME.StartsWith("BF")) item.Value = "BulbPlate";
    //                    break;
    //                }
    //            }
    //            blkrefPs.DowngradeOpen();
    //            #endregion
    //            //插端部形式
    //            ObjectId oidendCutA = ObjectId.Null; //节点图的objectid
    //            #region//插入节点图纸...........endA
    //            if (firstOrSecondPart)
    //            {
    //                oidendCutA = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_AngleBarEndCut", new Point3d(tb.Position.X + 88 * 60, tb.Position.Y + 118 * 60, tb.Position.Z), new Scale3d(60, 60, 60), 0);
    //            }
    //            else
    //            {
    //                oidendCutA = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_AngleBarEndCut", new Point3d(tb.Position.X + 88 * 60, tb.Position.Y + 22 * 60, tb.Position.Z), new Scale3d(60, 60, 60), 0);
    //            }
    //            BlockReference blkrefendCutA = oidendCutA.GetObject(OpenMode.ForRead) as BlockReference;
    //            blkrefendCutA.UpgradeOpen();
    //            foreach (DynamicBlockReferenceProperty item in blkrefendCutA.DynamicBlockReferencePropertyCollection)
    //            {
    //                if (item.PropertyName == "AngleBarEndcutType") item.Value = profilePart.EndAcode;
    //                if (item.PropertyName == "RightOrLeft") item.Value = item.Value = item.GetAllowedValues()[1];
    //            }
    //            blkrefendCutA.DowngradeOpen();
    //            #endregion
    //            ObjectId oidendCutB = ObjectId.Null; //节点图的objectid
    //            #region//插入节点图纸...........endB
    //            if (firstOrSecondPart)
    //            {
    //                oidendCutB = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_AngleBarEndCut", new Point3d(tb.Position.X + (88 + 167.6) * 60, tb.Position.Y + 118 * 60, tb.Position.Z), new Scale3d(60, 60, 60), 0);
    //            }
    //            else
    //            {
    //                oidendCutB = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_AngleBarEndCut", new Point3d(tb.Position.X + (88 + 167.6) * 60, tb.Position.Y + 22 * 60, tb.Position.Z), new Scale3d(60, 60, 60), 0);
    //            }
    //            BlockReference blkrefendCutB = oidendCutB.GetObject(OpenMode.ForRead) as BlockReference;
    //            blkrefendCutB.UpgradeOpen();
    //            foreach (DynamicBlockReferenceProperty item in blkrefendCutB.DynamicBlockReferencePropertyCollection)
    //            {
    //                if (item.PropertyName == "AngleBarEndcutType") item.Value = profilePart.EndBcode;
    //                if (item.PropertyName == "RightOrLeft") item.Value = item.GetAllowedValues()[0];
    //            }
    //            blkrefendCutB.DowngradeOpen();
    //            #endregion

    //            ObjectId oidEndDirection = ObjectId.Null; //B端的方位objectid
    //            #region//插入B端的方位...........endB
    //            if (firstOrSecondPart)
    //            {
    //                oidEndDirection = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_DirectionArrow", new Point3d(tb.Position.X + 257 * 60, tb.Position.Y + 156 * 60, tb.Position.Z), new Scale3d(60, 60, 60), 0);
    //            }
    //            else
    //            {
    //                oidEndDirection = AutoCAD_Net_Project.MTO.InsertBlockref(modelspace.ObjectId, "0", "KFWH_CP_SD_DirectionArrow", new Point3d(tb.Position.X + 257 * 60, tb.Position.Y + 65 * 60, tb.Position.Z), new Scale3d(60, 60, 60), 0);
    //            }
    //            BlockReference blkrefDirArr = oidEndDirection.GetObject(OpenMode.ForRead) as BlockReference;
    //            blkrefDirArr.UpgradeOpen();
    //            foreach (DynamicBlockReferenceProperty item in blkrefDirArr.DynamicBlockReferencePropertyCollection)
    //            {
    //                if (item.PropertyName == "DirectionText") item.Value = item.GetAllowedValues()[(int)profilePart.ENDBDirectionVal];
    //            }
    //            blkrefDirArr.DowngradeOpen();
    //            Dictionary<string, string> dictb1 = new Dictionary<string, string>();
    //            dictb1["DIRECTIONSHOWNTEXT"] = "%<\\AcObjProp Object(%<\\_ObjId " + blkrefDirArr.ObjectId.OldIdPtr.ToInt64() + ">%).Parameter(88).lookupString>%";
    //            oidEndDirection.UpdateAttributesInBlock(dictb1);
    //            #endregion
    //            trans.Commit();
    //        }
    //    }

    //    [myAutoCADcmd("选择型材下料表格更新Profile Sketch端部形式", "SD_UpdateProfileEndCut.png")]
    //    [CommandMethod("SD_UpdateProfileEndCut")]
    //    public void KFWHSD_更新ProfileSketch()
    //    {
    //        if (Application.GetSystemVariable("WORLDUCS").ToString() != "1") { this.acEd.CurrentUserCoordinateSystem = Matrix3d.Identity; this.acEd.Regen(); }
    //        //设置颜色样式
    //        if (Application.GetSystemVariable("CECOLOR").ToString() != "BYLAYER") Application.SetSystemVariable("CECOLOR", "BYLAYER");
    //        //选择需要更新的ProfileSketch表格
    //        TypedValueList tvl = new TypedValueList();
    //        tvl.Add(typeof(BlockReference));
    //        PromptSelectionOptions pso = new PromptSelectionOptions();
    //        pso.MessageForAdding = "选择panel sketch的图框：\n";
    //        List<ObjectId[]> listProfileSketchs = new List<ObjectId[]>();
    //        #region//将型材表格和图形绑定在一起
    //        using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //        {
    //            var psr_ProfileSketchtbs = this.acEd.GetSelection(pso, new SelectionFilter(tvl));
    //            if (psr_ProfileSketchtbs.Status != PromptStatus.OK) { Application.ShowAlertDialog("你选择的不是Profile sketch的图框!"); return; }
    //            foreach (SelectedObject item in psr_ProfileSketchtbs.Value)
    //            {
    //                if (item.ObjectId.GetBlockReferenceName() == "KFWH_CP_SD_A4")
    //                {
    //                    BlockReference blkrefTb = item.ObjectId.GetObject(OpenMode.ForRead, true) as BlockReference;
    //                    var psr_ProfileSketch = this.acEd.SelectCrossingWindow(blkrefTb.GeometricExtents.MinPoint, blkrefTb.GeometricExtents.MaxPoint, new SelectionFilter(tvl));
    //                    if (psr_ProfileSketch.Status == PromptStatus.OK)
    //                    {
    //                        foreach (SelectedObject selObj in psr_ProfileSketch.Value)
    //                        {
    //                            if (selObj.ObjectId.GetBlockReferenceName() == "KFWH_CP_SD_ProfileInforTable")
    //                            {
    //                                BlockReference blkrefProfileSketchTable = selObj.ObjectId.GetObject(OpenMode.ForRead, true) as BlockReference;
    //                                Point3d p1 = blkrefProfileSketchTable.GeometricExtents.MinPoint;
    //                                Point3d p2 = blkrefProfileSketchTable.GeometricExtents.MaxPoint;
    //                                p1 = p1.Add(new Vector3d(0, -76 * blkrefProfileSketchTable.ScaleFactors.X, 0));
    //                                p2 = p2.Add(new Vector3d(0, -12 * blkrefProfileSketchTable.ScaleFactors.X, 0));
    //                                var psr_ProfileSketchdwgs = this.acEd.SelectCrossingWindow(p1, p2, new SelectionFilter(tvl));
    //                                if (psr_ProfileSketchdwgs.Status == PromptStatus.OK)
    //                                {
    //                                    ObjectId oidEndA = ObjectId.Null; ObjectId oidEndB = ObjectId.Null;
    //                                    foreach (SelectedObject selobjEndcut in psr_ProfileSketchdwgs.Value)
    //                                    {
    //                                        if (selobjEndcut.ObjectId.GetBlockReferenceName() == "KFWH_CP_SD_AngleBarEndCut")
    //                                        {
    //                                            BlockReference blkrefPsEnd = selobjEndcut.ObjectId.GetObject(OpenMode.ForRead) as BlockReference;
    //                                            foreach (DynamicBlockReferenceProperty pro in blkrefPsEnd.DynamicBlockReferencePropertyCollection)
    //                                            {
    //                                                if (pro.PropertyName == "RightOrLeft")
    //                                                {
    //                                                    if (pro.Value.ToString() == pro.GetAllowedValues()[0].ToString()) oidEndB = selobjEndcut.ObjectId;
    //                                                    if (pro.Value.ToString() == pro.GetAllowedValues()[1].ToString()) oidEndA = selobjEndcut.ObjectId;
    //                                                }
    //                                            }
    //                                        }
    //                                    }
    //                                    listProfileSketchs.Add(new ObjectId[] { selObj.ObjectId, oidEndA, oidEndB });
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //            trans.Commit();
    //        }
    //        #endregion
    //        List<KFWH_CP_SD_ProfileEntity> listProfiles = new List<KFWH_CP_SD_ProfileEntity>();
    //        using (Transaction trans = this.acDb.TransactionManager.StartTransaction())
    //        {
    //            BlockTable bt = trans.GetObject(this.acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
    //            BlockTableRecord btrMs = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
    //            #region//读取端部形式
    //            foreach (ObjectId item in btrMs)
    //            {
    //                Entity ent = item.GetObject(OpenMode.ForRead) as Entity;
    //                if (!(ent is BlockReference)) continue;
    //                if (item.GetBlockReferenceName() != "KFWH_CP_SD_ProfileEntity") continue;
    //                listProfiles.Add(new KFWH_CP_SD_ProfileEntity(item));
    //            }
    //            #endregion
    //            //合并common parts
    //            List<KFWH_CP_SD_ProfileEntity> listProfiles_combine = new List<KFWH_CP_SD_ProfileEntity>();
    //            #region//合并相同的零件
    //            if (listProfiles.Count > 0)
    //            {
    //                for (int i = 0; i < listProfiles.Count; i++)
    //                {
    //                    if (listProfiles_combine.Count(c => c.PROFILENAME == listProfiles[i].PROFILENAME) > 0)//名称相同就认为是同一个零件
    //                    {
    //                        var temp = listProfiles_combine.First(c => c.PROFILENAME == listProfiles[i].PROFILENAME);
    //                        temp.Qty += 1;
    //                        listProfiles_combine[listProfiles_combine.IndexOf(temp)] = temp;
    //                    }
    //                    else
    //                    {
    //                        listProfiles[i].Qty = 1;
    //                        listProfiles_combine.Add(listProfiles[i]);
    //                    }
    //                }
    //            }
    //            #endregion
    //            var listPros = listProfiles_combine.OrderBy(c => c.PROFILENAME).ToList();
    //            //
    //            #region//更新端部形式
    //            foreach (ObjectId[] item in listProfileSketchs)
    //            {
    //                string partName = item[0].GetAttributeInBlockReference("PARTNAME");
    //                int partQty = int.Parse(item[0].GetAttributeInBlockReference("P")) + int.Parse(item[0].GetAttributeInBlockReference("C")) + int.Parse(item[0].GetAttributeInBlockReference("S"));
    //                if (listPros.Count(c => c.PROFILENAME == partName) > 0)
    //                {
    //                    var profilePart = listPros.Where(c => c.PROFILENAME == partName).ToList()[0];
    //                    //检查并更新A端部节点
    //                    BlockReference blkrefendCutA = item[1].GetObject(OpenMode.ForRead) as BlockReference;
    //                    blkrefendCutA.UpgradeOpen();
    //                    foreach (DynamicBlockReferenceProperty pro in blkrefendCutA.DynamicBlockReferencePropertyCollection)
    //                    {
    //                        if (pro.PropertyName == "AngleBarEndcutType")
    //                        {
    //                            if (pro.Value.ToString() != profilePart.EndAcode)
    //                            {
    //                                pro.Value = profilePart.EndAcode;
    //                                this.acEd.WriteMessage($"{profilePart.PROFILENAME} 的A端部节点更新完毕" + Environment.NewLine);
    //                            }
    //                            else this.acEd.WriteMessage($"{profilePart.PROFILENAME} 的A端部节点不需要更新" + Environment.NewLine);
    //                        }
    //                    }
    //                    blkrefendCutA.DowngradeOpen();
    //                    //检查并更新B端部节点
    //                    BlockReference blkrefendCutB = item[2].GetObject(OpenMode.ForRead) as BlockReference;
    //                    blkrefendCutB.UpgradeOpen();
    //                    foreach (DynamicBlockReferenceProperty pro in blkrefendCutB.DynamicBlockReferencePropertyCollection)
    //                    {
    //                        if (pro.PropertyName == "AngleBarEndcutType")
    //                        {
    //                            if (pro.Value.ToString() != profilePart.EndBcode)
    //                            {
    //                                pro.Value = profilePart.EndBcode;
    //                                this.acEd.WriteMessage($"{profilePart.PROFILENAME} 的B端部节点更新完毕" + Environment.NewLine);
    //                            }
    //                            else this.acEd.WriteMessage($"{profilePart.PROFILENAME} 的B端部节点不需要更新" + Environment.NewLine);
    //                        }
    //                    }
    //                    blkrefendCutB.DowngradeOpen();
    //                    //检查并且更新数量
    //                    if (partQty != profilePart.Qty)
    //                    {
    //                        Dictionary<string, string> dictb = new Dictionary<string, string>();
    //                        dictb["P"] = "0"; dictb["S"] = "0"; dictb["C"] = profilePart.Qty.ToString();
    //                        item[0].UpdateAttributesInBlock(dictb);
    //                        this.acEd.WriteMessage($"{profilePart.PROFILENAME} 的数量信息更新完毕" + Environment.NewLine);
    //                    }
    //                    else this.acEd.WriteMessage($"{profilePart.PROFILENAME} 的数量信息不需要更新" + Environment.NewLine);
    //                }
    //                else Application.ShowAlertDialog($"找不到零件名称为 {partName} 块名称为\" KFWH_CP_SD_ProfileEntity \"的动态块");
    //            }
    //            #endregion
    //            trans.Commit();
    //        }
    //    }
    //}
}

