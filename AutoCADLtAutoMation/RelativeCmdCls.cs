using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

////namespace AutoCADLtAutoMation
////{
////    public class PanelInfor
////    {
////        public string PanelName { get; set; }
////        public string BlockName { get; set; }
////        public string DoneBy { get; set; }
////        public string CheckBy { get; set; }
////        public string PanelDecription { get; set; }
////        public string HullNO { get; set; }
////        public string MSSref { get; set; }
////        public string ItemNo { get; set; }
////        public string DrawingNo { get; set; }
////        public string Date { get; set; }
////        public string Alt { get; set; }
////    }

//    public class NestixPart
//    {
//        public Database curDb { get; set; }
//        public ObjectId PlateCuttingPartOID { get; set; }
//        public ObjectIdCollection PlateCuttingPartInternalOIDs { get; set; }
//        public ObjectIdCollection NestixPartInternalOIDs { get; set; }
//        public ObjectId NestixPartOID { get; set; }
//        public ObjectId PartNameTextOid { get; set; }
//        public Editor curEd { get; set; }
//        public string PanelName { get; set; }
//        public string PartName { get; set; }
//        public string Side { get; set; }
//        public string Thickness { get; set; }
//        public int PQty { get; set; }
//        public int CQty { get; set; }
//        public int SQty { get; set; }
//        public string Material { get; set; }
//        public NestixPart(ObjectId oidShape, ObjectId ncPartTableref)
//        {
//            this.PlateCuttingPartOID = oidShape;
//            this.curDb = oidShape.Database;
//            this.curEd = KFWH_CMD.Tools.GetEditor(this.curDb);
//            using (Transaction trans = this.curDb.TransactionManager.StartTransaction())
//            {
//                //获得零件外形
//                try
//                {
//                    Polyline partShape = oidShape.GetObject(OpenMode.ForRead) as Polyline;
//                    TypedValueList partInternal = new TypedValueList();
//                    partInternal.Add(DxfCode.LayerName, "NC-*");
//                    var psr_Internals = this.curEd.SelectCrossingPolygon(partShape.GetAllPoints(), new SelectionFilter(partInternal));
//                    if (psr_Internals.Status == PromptStatus.OK)
//                    {
//                        this.PlateCuttingPartInternalOIDs = new ObjectIdCollection();
//                        foreach (SelectedObject selObj in psr_Internals.Value)
//                        {
//                            if (selObj.ObjectId != this.PlateCuttingPartOID) this.PlateCuttingPartInternalOIDs.Add(selObj.ObjectId);
//                        }
//                    }
//                    //获得零件信息表格，块或者表格
//                    Entity ent = ncPartTableref.GetObject(OpenMode.ForRead) as Entity;
//                    TypedValueList typeValList = new TypedValueList();
//                    typeValList.Add(typeof(DBText));
//                    typeValList.Add(DxfCode.Color, 35);
//                    typeValList.Add(DxfCode.LayerName, "0");
//                    var psr_Text = this.curEd.SelectWindowPolygon(partShape.GetAllPoints(), new SelectionFilter(typeValList));
//                    if (psr_Text.Status == PromptStatus.OK)
//                    {
//                        this.PartName = (psr_Text.Value[0].ObjectId.GetObject(OpenMode.ForRead) as DBText).TextString;
//                        this.PartNameTextOid = psr_Text.Value[0].ObjectId;
//                        if (ent is Table)
//                        {
//                            Table tb = ent as Table;
//                            if (tb.TableStyleName == "KFWHSDTGBOM")
//                            {
//                                var str = tb.ObjectId.GetTGSize(this.PartName);
//                                this.Material = str[1];
//                                this.Thickness = str[0];
//                                this.PQty = int.Parse(tb.Cells[2, 1].Value.ToString());
//                                this.CQty = int.Parse(tb.Cells[2, 2].Value.ToString());
//                                this.SQty = int.Parse(tb.Cells[2, 3].Value.ToString());
//                                if (this.PQty != 0) this.Side = "P";
//                                if (this.CQty != 0 && this.PQty == 0) this.Side = "C";
//                                if (this.SQty != 0 && this.PQty == 0 && this.CQty == 0) this.Side = "S";
//                            }
//                        }
//                        if (ent is BlockReference)
//                        {
//                            if (ent.ObjectId.GetBlockReferenceName() == "KFWH_CP_SD_NCPartsInforTable")
//                            {
//                                this.Side = string.Empty;
//                                this.Thickness = ent.ObjectId.GetAttributeInBlockReference("THICKNESS");
//                                this.Material = ent.ObjectId.GetAttributeInBlockReference("MATERIALGRADE");
//                                this.PartName = ent.ObjectId.GetAttributeInBlockReference("PARTNAME");
//                                this.PQty = int.Parse(ent.ObjectId.GetAttributeInBlockReference("P"));
//                                this.CQty = int.Parse(ent.ObjectId.GetAttributeInBlockReference("C"));
//                                this.SQty = int.Parse(ent.ObjectId.GetAttributeInBlockReference("S"));
//                                if (this.PQty != 0) this.Side = "P";
//                                if (this.CQty != 0 && this.PQty == 0) this.Side = "C";
//                                if (this.SQty != 0 && this.PQty == 0 && this.CQty == 0) this.Side = "S";
//                            }

//                        }
//                        //{\C4;DNV S460ML 46J -40°C}

//                        if (this.Material.Contains(@"{\C4;")) this.Material = this.Material.Replace(@"{\C4;", string.Empty).Replace(@"}", string.Empty);
//                        if (this.PanelName.Contains(@"{\C4;")) this.PanelName = this.PanelName.Replace(@"{\C4;", string.Empty).Replace(@"}", string.Empty);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    this.curEd.WriteMessage(ex.Message);
//                }
//            }
//        }
//    }
//}
