using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs
{
    public partial class DataLayerGenerator : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<SysObjects> lstTable = BusinessLayer.GetSysObjectsList("type = 'U' ORDER BY name ASC");
                Methods.SetComboBoxField<SysObjects>(cboTable, lstTable, "Name", "ObjectID");

                List<SysObjects> lstView = BusinessLayer.GetSysObjectsList("type = 'V' ORDER BY name ASC");
                Methods.SetComboBoxField<SysObjects>(cboView, lstView, "Name", "ObjectID");

                List<SysObjects> lstProc = BusinessLayer.GetSysObjectsList("type = 'P' ORDER BY name ASC");
                Methods.SetComboBoxField<SysObjects>(cboProc, lstProc, "Name", "ObjectID");
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            int objectID = Convert.ToInt32(cboTable.SelectedValue);
            List<SysColumns> lstColumns = BusinessLayer.GetSysColumnsList("OBJECT_ID = " + objectID);

            string tableName = txtTableName.Text;
            string oldTableName = cboTable.SelectedItem.Text;
            List<String> listPK = BusinessLayer.GetSysColumnsPKList(tableName);

            String listMember = "";
            String listField = "";
            String paramPK = "";
            String passParamPK = "";

            string createdDateDao = "";
            string lastUpdatedDateDao = "";
            foreach (SysColumns col in lstColumns)
            {
                if (col.Name == "CreatedDate")
                    createdDateDao = "\nrecord.CreatedDate = DateTime.Now;";
                else if (col.Name == "LastUpdatedDate")
                    lastUpdatedDateDao = "\nrecord.LastUpdatedDate = DateTime.Now;";

                string colType = col.Type;
                string identity = "";
                if (col.IsIdentity)
                    identity = ", IsIdentity = true";
                string nullable = "";
                if (col.IsNullable)
                {
                    nullable = ", IsNullable = true";
                    if (colType.Contains("Int"))
                    {
                        colType += "?";
                    }
                    else if (colType.Contains("DateTime"))
                    {
                        colType += "?";
                    }
                }
                listMember += string.Format("private {0} _{1};\n", colType, col.Name);

                string primaryKey = "";
                if (listPK.Contains(col.Name))
                {
                    primaryKey = ", IsPrimaryKey = true";
                    if (paramPK != "")
                        paramPK += ", ";
                    paramPK += string.Format("{0} {1}", col.Type, col.Name);

                    if (passParamPK != "")
                        passParamPK += ", ";
                    passParamPK += string.Format("{0}", col.Name);
                }
                listField += string.Format("[Column(Name = \"{0}\", DataType = \"{1}\"{2}{3}{4})]\n", col.Name, col.Type, primaryKey, identity, nullable);
                listField += string.Format("public {1} {0}\n{{\nget {{ return _{0}; }}\nset {{ _{0} = value; }}\n}}\n", col.Name, colType);

            }

            String result = "";
            result = String.Format("#region {0}\n[Serializable]\n[Table(Name = \"{1}\")]\npublic class {0} : DbDataModel\n{{\n", tableName, oldTableName);
            result += listMember + "\n";
            result += listField;
            result += "}\n\n";

            //Dao
            String listStringPK = "";
            string ctxPK = "";
            foreach (String pk in listPK)
            {
                listStringPK += string.Format("private const string p_{0} = \"@p_{0}\";\n", pk);
                ctxPK += string.Format("_ctx.Add(p_{0}, {0});\n", pk);
            }

            result += string.Format("public class {0}Dao\n{{\nprivate readonly IDbContext _ctx = DbFactory.Configure();\nprivate readonly DbHelper _helper = new DbHelper(typeof({0}));\nprivate bool _isAuditLog = false;\n", tableName);
            result += listStringPK;

            result += string.Format("public {0}Dao(){{ }}\n", tableName);
            result += string.Format("public {0}Dao(IDbContext ctx)\n{{\n_ctx = ctx;\n}}\n", tableName);

            result += string.Format("public {0} Get({1})\n{{\n_ctx.CommandText = _helper.GetRecord();\n{2}DataRow row = DaoBase.GetDataRow(_ctx);\nreturn (row == null) ? null : ({0})_helper.DataRowToObject(row, new {0}());\n}}\n", tableName, paramPK, ctxPK);

            result += string.Format("public int Insert({0} record)\n{{{1}\n_helper.Insert(_ctx, record, _isAuditLog);\nreturn DaoBase.ExecuteNonQuery(_ctx);\n}}\n", tableName, createdDateDao);

            result += string.Format("public int InsertReturnPrimaryKeyID({0} record)\n{{{1}\n_helper.InsertReturnPrimaryKeyID(_ctx, record, _isAuditLog);\nreturn DaoBase.ExecuteScalar(_ctx);\n}}\n", tableName, createdDateDao);

            result += string.Format("public int Update({0} record)\n{{{1}\n_helper.Update(_ctx, record, _isAuditLog);\nreturn DaoBase.ExecuteNonQuery(_ctx, true);\n}}\n", tableName, lastUpdatedDateDao);

            result += string.Format("public int Delete({1})\n{{\n{0} record;\nif (_ctx.Transaction == null)\nrecord = new {0}Dao().Get({2});\nelse\nrecord = Get({2});\n_helper.Delete(_ctx, record, _isAuditLog);\nreturn DaoBase.ExecuteNonQuery(_ctx);\n}}\n", tableName, paramPK, passParamPK);
            result += "}\n#endregion";
            txtResult.Text = result;




            string resultBusinessLayer = "";
            resultBusinessLayer = string.Format("#region {0}\n", tableName);
            resultBusinessLayer += string.Format("public static {0} Get{0}({1})\n{{\nreturn new {0}Dao().Get({2});\n}}\n", tableName, paramPK, passParamPK);

            resultBusinessLayer += string.Format("public static int Insert{0}({0} record)\n{{\nreturn new {0}Dao().Insert(record);\n}}\n", tableName);

            resultBusinessLayer += string.Format("public static int Update{0}({0} record)\n{{\nreturn new {0}Dao().Update(record);\n}}\n", tableName);

            resultBusinessLayer += string.Format("public static int Delete{0}({1})\n{{\nreturn new {0}Dao().Delete({2});\n}}\n", tableName, paramPK, passParamPK);

            resultBusinessLayer += string.Format("public static List< {0} > Get{0}List(string filterExpression)\n{{\n", tableName);
            resultBusinessLayer += string.Format("List< {0} > result = new List< {0} >();\n", tableName);
            resultBusinessLayer += "IDbContext ctx = DbFactory.Configure();\ntry\n{\n";
            resultBusinessLayer += string.Format("DbHelper helper = new DbHelper(typeof({0}));\nctx.CommandText = helper.Select(filterExpression);\n", tableName);
            resultBusinessLayer += "using (IDataReader reader = DaoBase.GetDataReader(ctx))\nwhile (reader.Read())\n";
            resultBusinessLayer += string.Format("result.Add(({0})helper.IDataReaderToObject(reader, new {0}()));\n", tableName);
            resultBusinessLayer += "}\ncatch (Exception ex)\n{\nthrow new Exception(ex.Message, ex);\n}\nfinally\n{\nctx.Close();\n}\nreturn result;\n}\n#endregion";

            txtBusinessLayer.Text = resultBusinessLayer;
        }

        protected void btnGenerateView_Click(object sender, EventArgs e)
        {
            int objectID = Convert.ToInt32(cboView.SelectedValue);
            List<SysColumns> lstColumns = BusinessLayer.GetSysColumnsList("OBJECT_ID = " + objectID);

            string tableName = txtViewName.Text;
            string oldTableName = cboView.SelectedItem.Text;
            List<String> listPK = BusinessLayer.GetSysColumnsPKList(tableName);

            String listMember = "";
            String listField = "";
            foreach (SysColumns col in lstColumns)
            {
                listMember += string.Format("private {0} _{1};\n", col.Type, col.Name);

                listField += string.Format("[Column(Name = \"{0}\", DataType = \"{1}\")]\n", col.Name, col.Type);
                listField += string.Format("public {1} {0}\n{{\nget {{ return _{0}; }}\nset {{ _{0} = value; }}\n}}\n", col.Name, col.Type);

            }

            String result = "";
            result = String.Format("#region {0}\n[Serializable]\n[Table(Name = \"{1}\")]\npublic class {0}\n{{\n", tableName, oldTableName);
            result += listMember + "\n";
            result += listField;
            result += "}\n#endregion";

            txtResult.Text = result;




            string resultBusinessLayer = "";
            resultBusinessLayer = string.Format("#region {0}\n", tableName);

            resultBusinessLayer += string.Format("public static List< {0} > Get{0}List(string filterExpression)\n{{\n", tableName);
            resultBusinessLayer += string.Format("List< {0} > result = new List< {0} >();\n", tableName);
            resultBusinessLayer += "IDbContext ctx = DbFactory.Configure();\ntry\n{\n";
            resultBusinessLayer += string.Format("DbHelper helper = new DbHelper(typeof({0}));\nctx.CommandText = helper.Select(filterExpression);\n", tableName);
            resultBusinessLayer += "using (IDataReader reader = DaoBase.GetDataReader(ctx))\nwhile (reader.Read())\n";
            resultBusinessLayer += string.Format("result.Add(({0})helper.IDataReaderToObject(reader, new {0}()));\n", tableName);
            resultBusinessLayer += "}\ncatch (Exception ex)\n{\nthrow new Exception(ex.Message, ex);\n}\nfinally\n{\nctx.Close();\n}\nreturn result;\n}\n#endregion";

            txtBusinessLayer.Text = resultBusinessLayer;
        }
    }
}