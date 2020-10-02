
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teigha.DatabaseServices;

namespace AutoCADLtAutoMation
{
    public static class MySQLHelper
    {
        public static List<string> GetAllProjectName()
        {
            List<string> listProjects = new List<string>();
            string sqlcommand = $"select * from tb_Schedule where Project<>''";
            SqlDataReader sdr = SqlHelper.MyExecuteReader(sqlcommand);//调用自己编写的sqlhelper类
            if (sdr.HasRows)//判断行不为空
            {
                while (sdr.Read())//循环读取数据，知道无数据为止
                {
                    if (!listProjects.Contains(sdr["Project"].ToString())) listProjects.Add(sdr["Project"].ToString());
                }
            }
            return listProjects;
        }

        internal static object GetCurProjectBlkPanels(string text)
        {
            throw new NotImplementedException();
        }

        public static List<string> GetCurProjectBlks(string projectName)
        {
            List<string> listblks = new List<string>();
            string sqlcommand = $"select * from tb_Schedule where Project='{projectName}'";
            SqlDataReader sdr = SqlHelper.MyExecuteReader(sqlcommand);//调用自己编写的sqlhelper类
            if (sdr.HasRows)//判断行不为空
            {
                while (sdr.Read())//循环读取数据，知道无数据为止
                {
                    if (!listblks.Contains(sdr["BLOCK"].ToString())) listblks.Add(sdr["BLOCK"].ToString());
                }
            }
            return listblks;
        }

        public static List<string> GetCurProjectBlkPanels(string projectName, string blkName)
        {
            List<string> listPanels = new List<string>();
            string sqlcommand = $"select * from tb_Schedule where Project='{projectName}' and BLOCK='{blkName}'";
            SqlDataReader sdr = SqlHelper.MyExecuteReader(sqlcommand);//调用自己编写的sqlhelper类
            if (sdr.HasRows)//判断行不为空
            {
                while (sdr.Read())//循环读取数据，知道无数据为止
                {
                    if (!listPanels.Contains(sdr["Panel"].ToString())) listPanels.Add(sdr["Panel"].ToString());
                }
            }
            return listPanels;
        }

        public static string GetProjectLe(string projectName)
        {
            string leName = string.Empty;
            string sqlcommand = $"select * from ProjectLE where project='{projectName}'";
            SqlDataReader sdr = SqlHelper.MyExecuteReader(sqlcommand);//调用自己编写的sqlhelper类
            if (sdr.HasRows)//判断行不为空
            {
                while (sdr.Read())//循环读取数据，知道无数据为止
                {
                    leName = sdr["LE"].ToString();
                    break;
                }
            }
            return leName;
        }

        public static string GetBlocksLe(string projectName, string blkName)
        {
            string blkleName = string.Empty;
            List<string> listPanels = new List<string>();
            string sqlcommand = $"select * from TB_blockLE where project='{projectName}' and blockname='{blkName}'";
            SqlDataReader sdr = SqlHelper.MyExecuteReader(sqlcommand);//调用自己编写的sqlhelper类
            if (sdr.HasRows)//判断行不为空
            {
                while (sdr.Read())//循环读取数据，知道无数据为止
                {
                    blkleName = sdr["blockLE"].ToString();
                    break;
                }
            }
            return blkleName;
        }

        public static string GetBlockItemNo(string projectName, string blkName)
        {
            string ItemNo = string.Empty;
            List<string> listPanels = new List<string>();
            string sqlcommand = $"select * from tb_MTO where Project='{projectName}' and BLK='{blkName}'";
            SqlDataReader sdr = SqlHelper.MyExecuteReader(sqlcommand);//调用自己编写的sqlhelper类
            if (sdr.HasRows)//判断行不为空
            {
                while (sdr.Read())//循环读取数据，知道无数据为止
                {
                    ItemNo = sdr["ItemNo"].ToString();
                    break;
                }
            }
            return ItemNo;
        }

        public static string GetUserShortName(string spmName)
        {
            string userName = string.Empty;
            string sqlcommand = $"select * from UserInfor where SPMAccount='{spmName}'";
            SqlDataReader sdr = SqlHelper.MyExecuteReader(sqlcommand);//调用自己编写的sqlhelper类
            if (sdr.HasRows)//判断行不为空
            {
                while (sdr.Read())//循环读取数据，知道无数据为止
                {
                    userName = sdr["ShortName"].ToString();
                    break;
                }
            }
            return userName;
        }

        public static string GetKFLEName(string projectName)
        {
            string kfle = string.Empty;
            string sqlcommand = $"select * from tb_SDTitleBlock where ProjectName='{projectName}'";
            SqlDataReader sdr = SqlHelper.MyExecuteReader(sqlcommand);//调用自己编写的sqlhelper类
            if (sdr.HasRows)//判断行不为空
            {
                while (sdr.Read())//循环读取数据，知道无数据为止
                {
                    kfle = sdr["KFLE"].ToString();
                    break;
                }
            }
            return kfle;
        }

        public static string GetCurProjectNumber_TitleBlock(string projectName)
        {
            string HullNumber_titleBlock = projectName;
            string sqlcommand = $"select * from tb_SDTitleBlock where ProjectName='{projectName}'";
            SqlDataReader sdr = SqlHelper.MyExecuteReader(sqlcommand);//调用自己编写的sqlhelper类
            if (sdr.HasRows)//判断行不为空
            {
                while (sdr.Read())//循环读取数据，知道无数据为止
                {
                    HullNumber_titleBlock = sdr["Others"].ToString();
                    break;
                }
            }
            return HullNumber_titleBlock;
        }

        public static PanelInfor GetPanelTitleBlockInfo(string projectName, string blockName, string paenlName)
        {
            PanelInfor curPanel = new PanelInfor() { BlockName = blockName, HullNO = MySQLHelper.GetCurProjectNumber_TitleBlock(projectName), PanelName = paenlName };
            string sqlcommand = $"select * from tb_Schedule where Project='{projectName}' and BLOCK='{blockName}' and Panel='{paenlName}'";
            SqlDataReader sdr = SqlHelper.MyExecuteReader(sqlcommand);//调用自己编写的sqlhelper类
            if (sdr.HasRows)//判断行不为空
            {
                while (sdr.Read())//循环读取数据，知道无数据为止
                {
                    curPanel.PanelDecription = sdr["DESCRIPTION"].ToString();
                    curPanel.DoneBy = MySQLHelper.GetUserShortName(sdr["DoneBy"].ToString());
                    curPanel.MSSref = sdr["MSSREFNO"].ToString().Replace(projectName + "-", "");
                    break;
                }
            }
            curPanel.ItemNo = MySQLHelper.GetBlockItemNo(projectName, blockName);
            curPanel.CheckBy = MySQLHelper.GetUserShortName(MySQLHelper.GetBlocksLe(projectName, blockName)) +
                "/" + MySQLHelper.GetUserShortName(MySQLHelper.GetProjectLe(projectName)) +
                "/" + MySQLHelper.GetKFLEName(projectName);
            return curPanel;
        }

        public static ObjectId InsertKFWHSDBlkstoCurDoc(this Database db, string BlkName)
        {
            ObjectId btrid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                if (!bt.Has(BlkName))
                {
                    List<string> listDynamicFldName = new List<string>();
                    string sqlcommand = $"select * from tb_PluginUsedFiles where Class_Name='KFWH_SD_DynamicBlks' and FileName='KFWH_2D_SD_DynamicBlock'";
                    SqlDataReader sdr = SqlHelper.MyExecuteReader(sqlcommand);//调用自己编写的sqlhelper类
                    if (sdr.HasRows)//判断行不为空
                    {
                        while (sdr.Read())//循环读取数据，知道无数据为止
                        {
                            listDynamicFldName.Add(sdr["Path"].ToString() + "\\" + sdr["FileName"].ToString());
                        }
                    }
                    string fldName = listDynamicFldName[0];

                    if (Directory.Exists(fldName))
                    {
                        if (File.Exists(Path.Combine(fldName, BlkName + ".dwg")))
                        {
                            using (Database dwgdb = new Database(false, true))
                            {
                                dwgdb.ReadDwgFile(Path.Combine(fldName, BlkName + ".dwg"), System.IO.FileShare.Read, true, null);
                                dwgdb.CloseInput(true);
                                btrid = db.Insert(BlkName, dwgdb, false);
                            }
                        }
                    }
                    trans.Commit();
                }
                else return bt[BlkName];
            }

            return btrid;
        }

        public static ObjectId InsertKFWHSDBlkstoCurDoc(this Database db, string BlkName, bool overwrite)
        {
            ObjectId btrid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                List<string> listDynamicFldName = new List<string>();
                string sqlcommand = $"select * from tb_PluginUsedFiles where Class_Name='KFWH_SD_DynamicBlks' and FileName='KFWH_2D_SD_DynamicBlock'";
                SqlDataReader sdr = SqlHelper.MyExecuteReader(sqlcommand);//调用自己编写的sqlhelper类
                if (sdr.HasRows)//判断行不为空
                {
                    while (sdr.Read())//循环读取数据，知道无数据为止
                    {
                        listDynamicFldName.Add(sdr["Path"].ToString() + "\\" + sdr["FileName"].ToString());
                    }
                }
                string fldName = listDynamicFldName[0];
                if (Directory.Exists(fldName))
                {
                    if (File.Exists(Path.Combine(fldName, BlkName + ".dwg")))
                    {
                        using (Database dwgdb = new Database(false, true))
                        {
                            dwgdb.ReadDwgFile(Path.Combine(fldName, BlkName + ".dwg"), System.IO.FileShare.Read, true, null);
                            dwgdb.CloseInput(true);
                            btrid = db.Insert(BlkName, dwgdb, false);
                        }
                    }
                }
                trans.Commit();
            }

            return btrid;
        }

        public static ObjectId InsertKFWHSDBevelDtailtoCurDoc(this Database db, string BlkName)
        {
            ObjectId btrid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                if (!bt.Has(BlkName))
                {
                    List<string> listDynamicFldName = new List<string>();
                    string sqlcommand = $"select * from tb_PluginUsedFiles where Class_Name='KFWH_SD_DynamicBlks' and FileName='KFWH_2D_SD_BevelCodes'";
                    SqlDataReader sdr = SqlHelper.MyExecuteReader(sqlcommand);//调用自己编写的sqlhelper类
                    if (sdr.HasRows)//判断行不为空
                    {
                        while (sdr.Read())//循环读取数据，知道无数据为止
                        {
                            listDynamicFldName.Add(sdr["Path"].ToString());
                        }
                    }
                    string fldName = listDynamicFldName[0];

                    if (Directory.Exists(fldName))
                    {
                        if (File.Exists(Path.Combine(fldName, BlkName + ".dwg")))
                        {
                            using (Database dwgdb = new Database(false, true))
                            {
                                dwgdb.ReadDwgFile(Path.Combine(fldName, BlkName + ".dwg"), System.IO.FileShare.Read, true, null);
                                dwgdb.CloseInput(true);
                                btrid = db.Insert(BlkName, dwgdb, false);
                            }
                        }
                    }
                    trans.Commit();
                }
                else btrid = bt[BlkName];
            }
            return btrid;
        }

        /// <summary>
        /// 更新块参照中的属性值
        /// </summary>
        /// <param name="blockRefId">块参照的Id</param>
        /// <param name="attNameValues">需要更新的属性名称与取值</param>
        public static void UpdateAttributesInBlock(this ObjectId blockRefId, Dictionary<string, string> attNameValues)
        {
            //获取块参照对象
            BlockReference blockRef = blockRefId.GetObject(OpenMode.ForRead) as BlockReference;
            if (blockRef != null)
            {
                //遍历块参照中的属性
                foreach (ObjectId id in blockRef.AttributeCollection)
                {
                    //获取属性
                    AttributeReference attref = id.GetObject(OpenMode.ForRead) as AttributeReference;
                    //判断是否包含指定的属性名称
                    if (attNameValues.ContainsKey(attref.Tag.ToUpper()))
                    {
                        attref.UpgradeOpen();//切换属性对象为写的状态
                        //设置属性值
                        attref.TextString = attNameValues[attref.Tag.ToUpper()].ToString();
                        attref.DowngradeOpen();//为了安全，将属性对象的状态改为读
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定名称的块属性值
        /// </summary>
        /// <param name="blockReferenceId">块参照的Id</param>
        /// <param name="attributeName">属性名</param>
        /// <returns>返回指定名称的块属性值</returns>
        public static string GetAttributeInBlockReference(this ObjectId blockReferenceId, string attributeName)
        {
            string attributeValue = string.Empty; // 属性值
            Database db = blockReferenceId.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                // 获取块参照
                BlockReference bref = (BlockReference)trans.GetObject(blockReferenceId, OpenMode.ForRead);
                // 遍历块参照的属性
                foreach (ObjectId attId in bref.AttributeCollection)
                {
                    // 获取块参照属性对象
                    AttributeReference attRef = (AttributeReference)trans.GetObject(attId, OpenMode.ForRead);
                    //判断属性名是否为指定的属性名
                    if (attRef.Tag.ToUpper() == attributeName.ToUpper())
                    {
                        attributeValue = attRef.TextString;//获取属性值
                        break;
                    }
                }
                trans.Commit();
            }
            return attributeValue; //返回块属性值
        }

        public static ObjectId GetAttributeObjectIdInBlockReference(this ObjectId blockReferenceId, string attributeName)
        {
            ObjectId oid = ObjectId.Null; // 属性值
            Database db = blockReferenceId.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                // 获取块参照
                BlockReference bref = (BlockReference)trans.GetObject(blockReferenceId, OpenMode.ForRead);
                // 遍历块参照的属性
                foreach (ObjectId attId in bref.AttributeCollection)
                {
                    // 获取块参照属性对象
                    AttributeReference attRef = (AttributeReference)trans.GetObject(attId, OpenMode.ForRead);
                    //判断属性名是否为指定的属性名
                    if (attRef.Tag.ToUpper() == attributeName.ToUpper())
                    {
                        oid = attId;//获取属性值
                        break;
                    }
                }
                trans.Commit();
            }
            return oid; //返回块属性值
        }

    }


    public static class MyHelper
    {
        /// <summary>
        /// 获取块的名称
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public static String GetBlockReferenceName(this ObjectId oid)
        {
            Database db = oid.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                Entity ent = oid.GetObject(OpenMode.ForRead) as Entity;
                if (ent is BlockReference)
                {
                    var blkref = ent as BlockReference;

                    if (blkref.IsDynamicBlock)
                    {
                        var blk = blkref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
                        return blk.Name;

                    }
                    else
                    {
                        var blk = blkref.BlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
                        return blk.Name;
                    }
                }
                else return string.Empty;

            }
        }

        /// <summary>
        /// 获取panel名称
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static string GetCurShopDrawingPanelName(Database db)
        {
            if (db.HasSummaryInfo())
            {
                string str0 = string.Empty, str1 = string.Empty;
                var info = new DatabaseSummaryInfoBuilder(db.SummaryInfo);
                if (db.HasCustomProperty("BlockName")) str0 = info.CustomPropertyTable["BlockName"].ToString();
                if (db.HasCustomProperty("PanelName")) str1 = info.CustomPropertyTable["PanelName"].ToString();
                if (str0.EndsWith("-W")) str0 = str0.Replace("-W", string.Empty);
                return str0 + "/" + str1;
            }
            else return string.Empty;

        }
        /// <summary>
        /// 获取当前图纸中的项目名称
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <returns></returns>
        public static string GetCurShopDrawingProjectName(Database db)
        {
            if (db.HasSummaryInfo())
            {
                string str0 = string.Empty;
                var info = new DatabaseSummaryInfoBuilder(db.SummaryInfo);
                if (db.HasCustomProperty("HullNO")) str0 = info.CustomPropertyTable["HullNO"].ToString();
                return str0;
            }
            else return string.Empty;
        }

        /// <summary>
        /// 获取属性块的属性值
        /// </summary>
        /// <param name="blockReferenceId">属性块的id</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public static object GetDyanmicBlockPropertyValue(this ObjectId blockReferenceId, string propertyName)
        {
            object result = 0;
            Database db = blockReferenceId.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                // 获取块参照
                BlockReference bref = (BlockReference)trans.GetObject(blockReferenceId, OpenMode.ForRead);
                // 遍历块参照的属性
                foreach (DynamicBlockReferenceProperty pro in bref.DynamicBlockReferencePropertyCollection)
                {
                    if (pro.PropertyName == propertyName)
                    {
                        result = pro.Value;
                    }
                    else continue;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取当前图纸属性。。。。。
        /// </summary>
        /// <param name="drawingPropertiesName">属性名称</param>
        /// <param name="db">dwg数据库</param>
        /// <returns></returns>
        public static string GetCurDrawingPropertyValue(string drawingPropertiesName, Database db)
        {
            if (db.HasSummaryInfo())
            {
                string str = string.Empty;
                var info = new DatabaseSummaryInfoBuilder(db.SummaryInfo);
                if (db.HasCustomProperty(drawingPropertiesName)) str = info.CustomPropertyTable[drawingPropertiesName].ToString();
                return str;
            }
            else return string.Empty;
        }

        /// <summary>
        /// 获取cad的图纸的保存版本
        /// </summary>
        /// <param name="fn">图纸的全路径</param>
        /// <returns>cad的版本号</returns>
        public static int GetAutoCADDrawingVer(string fn)
        {
            if (!MyHelper.FileInUse(fn))
            {
                StreamReader sr = new StreamReader(fn);
                char[] buffer = new char[6];
                var str = sr.Read(buffer, 0, 6);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++) sb.Append(buffer[i]);
                sr.Close();
                return int.Parse(sb.ToString().Replace("AC", "")) - 1019 + 2005;
            } else return 0;
            
        }

        /// <summary>
        /// 用资源管理器打开文件夹并且选中文件
        /// </summary>
        /// <param name="fdPath"></param>
        /// <param name="selectFileName"></param>
        /// 
        public static void MyOpenFolder(string fdPath, string selectFileName = "")
        {
            if (selectFileName == "") System.Diagnostics.Process.Start(fdPath); else System.Diagnostics.Process.Start("Explorer.exe", "/select," + Path.Combine(fdPath, selectFileName));
        }

        /// <summary>
        /// 导出报表为Csv
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="strFilePath">物理路径</param>
        /// <param name="tableheader">表头</param>
        /// <param name="columname">字段标题,逗号分隔</param>
        /// 
        public static bool dt2csv<T>(List<T> Ts, string strFilePath)
        {
            try
            {
                string strBufferLine = "";
                StreamWriter strmWriterObj = new StreamWriter(strFilePath, false, System.Text.Encoding.UTF8);
                string columname = string.Empty;
                Type t = typeof(T);
                //
                var strnamespace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;

                for (int i = 0; i < t.GetProperties().Where(c => !c.PropertyType.FullName.StartsWith(strnamespace)).ToList().Count; i++)
                {
                    if (i > 0) columname += ",";
                    columname += t.GetProperties()[i].Name;
                }
                //属性是类的情况
                for (int i = 0; i < t.GetProperties().Where(c => c.PropertyType.FullName.StartsWith(strnamespace)).ToList().Count; i++)
                {
                    foreach (var item in t.GetProperties().Where(c => c.PropertyType.FullName.StartsWith(strnamespace)).ToList()[i].PropertyType.GetProperties())
                    {
                        columname += "," + item.DeclaringType.Name + "_" + item.Name;
                    }
                }
                strmWriterObj.WriteLine(columname);
                //插入数据
                for (int i = 0; i < Ts.Count; i++)
                {
                    strBufferLine = "";
                    for (int j = 0; j < t.GetProperties().Where(c => !c.PropertyType.FullName.StartsWith(strnamespace)).ToList().Count; j++)
                    {
                        if (j > 0) strBufferLine += ",";
                        object objVal = t.GetProperties()[j].GetValue(Ts[i], null);
                        if (objVal != null) strBufferLine += objVal.ToString(); else strBufferLine += "N.A.";
                    }
                    for (int j = 0; j < t.GetProperties().Where(c => c.PropertyType.FullName.StartsWith(strnamespace)).ToList().Count; j++)
                    {
                        object propertyObj = t.GetProperties().Where(c => c.PropertyType.FullName.StartsWith(strnamespace)).ToList()[j].GetValue(Ts[i], null);
                        foreach (var item in t.GetProperties().Where(c => c.PropertyType.FullName.StartsWith(strnamespace)).ToList()[j].PropertyType.GetProperties())
                        {
                            object objVal = item.GetValue(propertyObj, null);
                            if (objVal != null) strBufferLine += "," + objVal.ToString(); else strBufferLine += ",N.A.";
                        }
                    }
                    strmWriterObj.WriteLine(strBufferLine);
                }
                strmWriterObj.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 类的集合另存为excel文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myList"></param>
        public static void ListClassToExcelSh<T>(List<T> myList)
        {
            dynamic xlapp = Type.GetTypeFromProgID("Excel.Application");
            dynamic wb = xlapp.Workbooks.Add();
            dynamic ws = (wb.Sheets[1]);
            Type t = myList[0].GetType();
            var pros = t.GetProperties().Where(c => c.DeclaringType.IsPublic).ToArray();
            for (int i = 0; i < pros.Length; i++) ws.Cells[1, i + 1] = pros[i].Name;
            for (int i = 0; i < myList.Count; i++)
            {
                for (int j = 0; j < pros.Length; j++) ws.Cells[i + 2, j + 1] = pros[j].GetValue(myList[i],null);
            }
            ws.UsedRange.EntireColumn.AutoFit();
            xlapp.Visible = true;
        }

        /// <summary>
        /// 检查文件是否是只读的
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool FileInUse(string filename)
        {
            bool use = true;
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None);
                use = false;
            }
            catch (Exception)
            {

            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return use;
        }

        public static string GetAutoCADPpltters()
        {
            //HKEY_CURRENT_USER\Software\Autodesk\AutoCAD\R21.0\ACAD-0001:409\Profiles\<<Unnamed Profile>>\General
            string plot = @"%USERPROFILE%\appdata\roaming\autodesk\autocad 2017\r21.0\enu\plotters";
            return plot+ "\\DWG To PDF.pc3";
        }

        /// <summary>
        /// 将实体添加到模型空间
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="ent">要添加的实体</param>
        /// <returns>返回添加到模型空间中的实体ObjectId</returns>
        public static ObjectId AddToModelSpace(this Database db, Entity ent)
        {
            ObjectId entId;//用于返回添加到模型空间中的实体ObjectId
            //定义一个指向当前数据库的事务处理，以添加直线
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //以读方式打开块表
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                //以写方式打开模型空间块表记录.
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                entId = btr.AppendEntity(ent);//将图形对象的信息添加到块表记录中
                trans.AddNewlyCreatedDBObject(ent, true);//把对象添加到事务处理中
                trans.Commit();//提交事务处理
            }
            return entId; //返回实体的ObjectId
        }

        /// <summary>
        /// 将实体添加到模型空间
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="ents">要添加的多个实体</param>
        /// <returns>返回添加到模型空间中的实体ObjectId集合</returns>
        public static ObjectIdCollection AddToModelSpace(this Database db, params Entity[] ents)
        {
            ObjectIdCollection ids = new ObjectIdCollection();
            var trans = db.TransactionManager;
            BlockTableRecord btr = (BlockTableRecord)trans.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite);
            foreach (var ent in ents)
            {
                ids.Add(btr.AppendEntity(ent));
                trans.AddNewlyCreatedDBObject(ent, true);
            }
            btr.DowngradeOpen();
            return ids;
        }

        public static bool BlockIsInsideBlock(this BlockReference blkrefInside,BlockReference blkrefOutSide)
        {
            bool flg1 = blkrefInside.GeometricExtents.MaxPoint.X < blkrefOutSide.GeometricExtents.MaxPoint.X && blkrefInside.GeometricExtents.MaxPoint.X < blkrefOutSide.GeometricExtents.MaxPoint.Y;
            bool flg2 = blkrefInside.GeometricExtents.MinPoint.X> blkrefOutSide.GeometricExtents.MinPoint.X && blkrefInside.GeometricExtents.MinPoint.Y > blkrefOutSide.GeometricExtents.MinPoint.Y;
            if (flg1 && flg2) return true; else return false;
        }

        public static List<string[]> GetCurPCAutoCADs()
        {
            List<string[]> res = new List<string[]>();

            var acKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Autodesk\Hardcopy", false);
            foreach (var item in acKey.GetValueNames())
            {
                var acadVer = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + item, false).GetValue("ProductName").ToString();

                var autocadExeLocation = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + item, false).GetValue("Location") + "\\accoreconsole.exe";
                res.Add(new string[2] { acadVer, autocadExeLocation });
            }
           
            return res;
        }

    }


    public static class CMDHelper
    {
        /// <summary>
        /// 调用accoreconsole.exe 执行命令
        /// </summary>
        /// <param name="commandstring">NetApi中注册的命令</param>
        /// <param name="dllPath">NetApi程序集的全路径</param>
        /// <param name="dwgfn">cad文件的路径</param>
        /// <param name="accoreconsolePath">Accoreconsole的全路径</param>
        public static void ExecuteCmd(string commandstring, string dllPath, string dwgfn, string accoreconsolePath)
        {
            System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };
            Process pro = new Process() { StartInfo = psi };
            pro.Start();
            pro.StandardInput.WriteLine("\"" + accoreconsolePath + "\" /i " + "\"" + dwgfn + "\"");
            pro.StandardInput.WriteLine("SECURELOAD");
            pro.StandardInput.WriteLine("0");
            pro.StandardInput.WriteLine("netload");
            pro.StandardInput.WriteLine("\"" + dllPath + "\"");
            pro.StandardInput.WriteLine("FILEDIA");
            pro.StandardInput.WriteLine("1");
            pro.StandardInput.WriteLine(commandstring);
            pro.StandardInput.WriteLine("Qsave");
            pro.StandardInput.WriteLine("QUIT");
            pro.StandardInput.WriteLine("Exit");
            //return pro.StandardOutput.ReadToEnd();
        }

        /// <summary>
        /// 执行scr脚本文件
        /// </summary>
        /// <param name="scrFilePath">scr脚本文件的全路径</param>
        /// <param name="dwgfn">dwg文件的全部路径</param>
        /// <param name="accoreconsolePath">Accoreconsole的全路径</param>
        public static void ExecuteCmd(string scrFilePath, string dwgfn, string accoreconsolePath)
        {
            System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = accoreconsolePath,
                Arguments = " /i " + "\"" + dwgfn + "\" /s " + "\"" + scrFilePath + "\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true

            };
            Process pro = new Process() { StartInfo = psi };
            pro.Start();
            pro.StandardInput.WriteLine("FILEDIA");
            pro.StandardInput.WriteLine("1");
            pro.StandardInput.WriteLine("Exit");
        }

        //"C:\Program Files\Autodesk\AutoCAD 2017\acad.exe" /b "C:\Users\sheng.nan\Desktop\Piping ISO Print to PDF.scr"
        //"C:\Program Files\Autodesk\AutoCAD LT 2015\acadlt.exe" /language "en-US" 
        /// <summary>
        /// 执行scr脚本文件
        /// </summary>
        /// <param name="scrFilePath">scr脚本文件的全路径</param>
        public static void ExecuteScr(string scrFilePath,string fn,string autocadltExePath)
        {
            string acadPath = autocadltExePath + "\\acadlt.exe";
            System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "\"" + acadPath + "\"",
                Arguments = " \""+ fn + "\"" +" /nologo /nohardware /b " + "\"" + scrFilePath + "\"",
                UseShellExecute = false
            };
            Process pro = new Process() { StartInfo = psi };
            pro.Start();
            if (pro.WaitForInputIdle())
            {
                System.Windows.Forms.MessageBox.Show("请等待等待执行完毕");
            }
            //pro.Id

        }
    }

    public class PanelInfor
    {
        public string PanelName { get; set; }
        public string BlockName { get; set; }
        public string DoneBy { get; set; }
        public string CheckBy { get; set; }
        public string PanelDecription { get; set; }
        public string HullNO { get; set; }
        public string MSSref { get; set; }
        public string ItemNo { get; set; }
        public string DrawingNo { get; set; }
        public string Date { get; set; }
        public string Alt { get; set; }
    }

    /// <summary>
    /// 摘要信息操作类
    /// </summary>
    public static class SummaryInfoTools
    {
        /// <summary>
        /// 获取摘要信息中的自定义属性个数
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <returns>返回自定义属性个数</returns>
        public static int NumCustomProperties(this Database db)
        {
            DatabaseSummaryInfo info = db.SummaryInfo; //获取数据库摘要信息
            //获取摘要信息中的自定义属性集合
            System.Collections.IDictionaryEnumerator props = info.CustomProperties;
            int count = 0;//计数器，用于统计自定义属性个数
            while (props.MoveNext())//遍历自定义属性
            {
                count++;//计数器累加
            }
            return count;//返回自定义属性个数
        }

        /// <summary>
        /// 判断图形中是否存在指定的自定义属性
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="key">自定义属性的名称</param>
        /// <returns>如果存在指定的自定义属性，返回true，否则返回false</returns>
        public static bool HasCustomProperty(this Database db, string key)
        {
            DatabaseSummaryInfo info = db.SummaryInfo; //获取数据库摘要信息
            //获取摘要信息中的自定义属性集合
            System.Collections.IDictionaryEnumerator props = info.CustomProperties;
            while (props.MoveNext())//遍历自定义属性
            {
                //如果存在指定的自定义属性，返回true
                if (props.Key.ToString().ToUpper() == key.ToUpper()) return true;
            }
            return false;//不存在指定的自定义属性，返回false
        }

        /// <summary>
        /// 判断图形中是否存在摘要信息
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <returns>如果存在摘要信息，返回true，否则返回false</returns>
        public static bool HasSummaryInfo(this Database db)
        {
            //如果存在自定义属性，则说明肯定有摘要信息
            if (db.NumCustomProperties() > 0) return true;
            DatabaseSummaryInfo info = db.SummaryInfo;//数据库的摘要信息
            //如果存在摘要信息，则返回true
            if (!string.IsNullOrWhiteSpace(info.Author) && !string.IsNullOrWhiteSpace(info.Comments)
                && !string.IsNullOrWhiteSpace(info.HyperlinkBase) && !string.IsNullOrWhiteSpace(info.Keywords)
                && !string.IsNullOrWhiteSpace(info.RevisionNumber)
                && !string.IsNullOrWhiteSpace(info.Subject) && !string.IsNullOrWhiteSpace(info.Title))
                return true;
            return false;//不存在摘要信息，返回false
        }
    }
}
