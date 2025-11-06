using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class CoverageTypeUploaded : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.COVERAGE_TYPE_UPLOAD;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        protected override void InitializeDataControl()
        {
            List<Variable> lstCoverageType = new List<Variable>();
            lstCoverageType.Add(new Variable { Code = "1", Value = "Master Skema Jaminan" });
            lstCoverageType.Add(new Variable { Code = "2", Value = "Skema Jaminan Instalasi" });
            lstCoverageType.Add(new Variable { Code = "3", Value = "Skema Jaminan Kelas Instalasi" });
            lstCoverageType.Add(new Variable { Code = "4", Value = "Skema Jaminan Item" });
            lstCoverageType.Add(new Variable { Code = "5", Value = "Skema Jaminan Kelas Item" });
            lstCoverageType.Add(new Variable { Code = "6", Value = "Skema Jaminan Kelompok Item" });
            lstCoverageType.Add(new Variable { Code = "7", Value = "Skema Jaminan Kelas Kelompok Item" });
            lstCoverageType.Add(new Variable { Code = "8", Value = "Skema Jaminan Unit Pelayanan" });
            Methods.SetComboBoxField<Variable>(cboCoverageType, lstCoverageType, "Value", "Code");
            cboCoverageType.Value = "1";

            BindGridView(CurrPage, true, ref PageCount);
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += "IsDeleted = 0";
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetCoverageTypeUploadRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<CoverageTypeUpload> lstEntity = BusinessLayer.GetCoverageTypeUploadList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpViewDetail1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<vCoverageTypeDepartmentUpload> lstDepartment = BusinessLayer.GetvCoverageTypeDepartmentUploadList(string.Format("CoverageTypeUploadID = {0} AND IsDeleted = 0 ORDER BY DepartmentName ASC", hdnExpandID.Value));
            lvwDetail1.DataSource = lstDepartment;
            lvwDetail1.DataBind();
        }

        protected void cbpViewDetail2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGrdDetail2(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGrdDetail2(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGrdDetail2(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("CoverageTypeUploadID = {0} AND IsDeleted = 0", hdnExpandID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvCoverageTypeItemUploadRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vCoverageTypeItemUpload> lstItem = BusinessLayer.GetvCoverageTypeItemUploadList(filterExpression, 8, pageIndex, "ItemName1 ASC");
            lvwDetail2.DataSource = lstItem;
            lvwDetail2.DataBind();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            CoverageTypeUploadDao entityCoverageTypeUploadDao = new CoverageTypeUploadDao(ctx);
            CoverageTypeDepartmentUploadDao entityCoverageTypeDepartmentUploadDao = new CoverageTypeDepartmentUploadDao(ctx);
            CoverageTypeDepartmentClassUploadDao entityCoverageTypeDepartmentClassUploadDao = new CoverageTypeDepartmentClassUploadDao(ctx);
            CoverageTypeItemUploadDao entityCoverageTypeItemUploadDao = new CoverageTypeItemUploadDao(ctx);
            CoverageTypeItemClassUploadDao entityCoverageTypeItemClassUploadDao = new CoverageTypeItemClassUploadDao(ctx);
            CoverageTypeItemGroupUploadDao entityCoverageTypeItemGroupUploadDao = new CoverageTypeItemGroupUploadDao(ctx);
            CoverageTypeItemGroupClassUploadDao entityCoverageTypeItemGroupClassUploadDao = new CoverageTypeItemGroupClassUploadDao(ctx);
            CoverageTypeServiceUnitUploadDao entityCoverageTypeServiceUnitUploadDao = new CoverageTypeServiceUnitUploadDao(ctx);
            CoverageTypeDao entityCoverageTypeDao = new CoverageTypeDao(ctx);
            CoverageTypeDepartmentDao entityCoverageTypeDepartmentDao = new CoverageTypeDepartmentDao(ctx);
            CoverageTypeDepartmentClassDao entityCoverageTypeDepartmentClassDao = new CoverageTypeDepartmentClassDao(ctx);
            CoverageTypeItemDao entityCoverageTypeItemDao = new CoverageTypeItemDao(ctx);
            CoverageTypeItemClassDao entityCoverageTypeItemClassDao = new CoverageTypeItemClassDao(ctx);
            CoverageTypeItemGroupDao entityCoverageTypeItemGroupDao = new CoverageTypeItemGroupDao(ctx);
            CoverageTypeItemGroupClassDao entityCoverageTypeItemGroupClassDao = new CoverageTypeItemGroupClassDao(ctx);
            CoverageTypeServiceUnitDao entityCoverageTypeServiceUnitDao = new CoverageTypeServiceUnitDao(ctx);

            try
            {
                string coverageType = cboCoverageType.Value.ToString();

                if (type == "download")
                {
                    #region Download Document
                    string reportCode = "";

                    if (coverageType == "1")
                    {
                        reportCode = string.Format("ReportCode = '{0}'", "FN-00104");
                    }
                    else if (coverageType == "2")
                    {
                        reportCode = string.Format("ReportCode = '{0}'", "FN-00105");
                    }
                    else if (coverageType == "3")
                    {
                        reportCode = string.Format("ReportCode = '{0}'", "FN-00106");
                    }
                    else if (coverageType == "4")
                    {
                        reportCode = string.Format("ReportCode = '{0}'", "FN-00107");
                    }
                    else if (coverageType == "5")
                    {
                        reportCode = string.Format("ReportCode = '{0}'", "FN-00108");
                    }
                    else if (coverageType == "6")
                    {
                        reportCode = string.Format("ReportCode = '{0}'", "FN-00109");
                    }
                    else if (coverageType == "7")
                    {
                        reportCode = string.Format("ReportCode = '{0}'", "FN-00110");
                    }
                    else
                    {
                        reportCode = string.Format("ReportCode = '{0}'", "FN-00111");
                    }

                    ReportMaster rm = BusinessLayer.GetReportMasterList(reportCode).FirstOrDefault();

                    StringBuilder sbResult = new StringBuilder();

                    List<dynamic> lstDynamic = null;
                    List<Variable> lstVariable = new List<Variable>();

                    lstVariable.Add(new Variable { Code = "CoverageType", Value = cboCoverageType.Value.ToString() });

                    lstDynamic = BusinessLayer.GetDataReport(rm.ObjectTypeName, lstVariable);

                    dynamic fields = lstDynamic[0];

                    foreach (var prop in fields.GetType().GetProperties())
                    {
                        sbResult.Append(prop.Name);
                        sbResult.Append(",");
                    }

                    sbResult.Append("\r\n");

                    for (int i = 0; i < lstDynamic.Count; ++i)
                    {
                        dynamic entity = lstDynamic[i];

                        foreach (var prop in entity.GetType().GetProperties())
                        {
                            sbResult.Append(prop.GetValue(entity, null).ToString().Replace(',', '_'));
                            sbResult.Append(",");
                        }

                        sbResult.Append("\r\n");
                    }

                    retval = sbResult.ToString();
                    #endregion

                    return true;
                }
                else if (type == "upload")
                {
                    #region Upload Document

                    string fileUpload = hdnUploadedFile.Value;
                    string fileUploadName = "";

                    if (coverageType == "1")
                    {
                        #region COVERAGE_TYPE
                        fileUploadName = "COVERAGE_TYPE.csv";
                        if (fileUpload != "")
                        {
                            string[] parts = Regex.Split(fileUpload, ",").Skip(1).ToArray();
                            fileUpload = String.Join(",", parts);
                        }

                        string path = AppConfigManager.QISPhysicalDirectory;
                        path += string.Format("{0}\\", AppConfigManager.QISFinanceUploadDocument.Replace('/', '\\'));

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        else
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<CoverageTypeUpload> coverageTypeList = BusinessLayer.GetCoverageTypeUploadList("1=1", ctx);
                            foreach (CoverageTypeUpload coverageTypeBegin in coverageTypeList)
                            {
                                entityCoverageTypeUploadDao.Delete(coverageTypeBegin.CoverageTypeUploadID);
                            }
                        }

                        FileStream fs = new FileStream(string.Format("{0}{1}", path, fileUploadName), FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);
                        byte[] data = Convert.FromBase64String(fileUpload);
                        bw.Write(data);
                        bw.Close();

                        string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, fileUploadName));

                        int rowCount = 0;
                        foreach (string temp in lstTemp)
                        {
                            if (rowCount != 0)
                            {
                                if (temp.Contains(','))
                                {
                                    List<String> fieldTemp = temp.Split(',').ToList();

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    CoverageTypeUpload oDataCoverageType = new CoverageTypeUpload();

                                    oDataCoverageType.CoverageTypeUploadCode = fieldTemp[0];
                                    oDataCoverageType.CoverageTypeUploadName = fieldTemp[1];
                                    oDataCoverageType.IsMarkupMarginReplaceDefault = fieldTemp[2] == "True" ? true : false;
                                    oDataCoverageType.Remarks = fieldTemp[3];
                                    oDataCoverageType.IsDeleted = fieldTemp[4] == "True" ? true : false;
                                    oDataCoverageType.CreatedBy = AppSession.UserLogin.UserID;
                                    oDataCoverageType.CreatedDate = DateTime.Now;
                                    oDataCoverageType.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    oDataCoverageType.LastUpdatedDate = DateTime.Now;

                                    entityCoverageTypeUploadDao.Insert(oDataCoverageType);
                                }
                            }
                            rowCount += 1;
                        }
                        #endregion
                    }
                    else if (coverageType == "2")
                    {
                        #region COVERAGE_TYPE_DEPARTMENT
                        fileUploadName = "COVERAGE_TYPE_DEPARTMENT.csv";
                        if (fileUpload != "")
                        {
                            string[] parts = Regex.Split(fileUpload, ",").Skip(1).ToArray();
                            fileUpload = String.Join(",", parts);
                        }

                        string path = AppConfigManager.QISPhysicalDirectory;
                        path += string.Format("{0}\\", AppConfigManager.QISFinanceUploadDocument.Replace('/', '\\'));

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        else
                        {
                            Directory.Delete(path, true);
                            Directory.CreateDirectory(path);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<CoverageTypeDepartmentUpload> coverageTypeList = BusinessLayer.GetCoverageTypeDepartmentUploadList("1=1", ctx);
                            foreach (CoverageTypeDepartmentUpload CoverageTypeDepartmentBegin in coverageTypeList)
                            {
                                entityCoverageTypeDepartmentUploadDao.Delete(CoverageTypeDepartmentBegin.ID);
                            }
                        }

                        FileStream fs = new FileStream(string.Format("{0}{1}", path, fileUploadName), FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);

                        byte[] data = Convert.FromBase64String(fileUpload);
                        bw.Write(data);
                        bw.Close();

                        string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, fileUploadName));

                        int rowCount = 0;
                        foreach (string temp in lstTemp)
                        {
                            if (rowCount != 0)
                            {
                                if (temp.Contains(','))
                                {
                                    List<String> fieldTemp = temp.Split(',').ToList();

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    CoverageTypeDepartmentUpload oDataCoverageTypeDepartment = new CoverageTypeDepartmentUpload();
                                    CoverageTypeUpload lstCoverageType = BusinessLayer.GetCoverageTypeUploadList(string.Format("CoverageTypeUploadCode = '{0}'", fieldTemp[0])).FirstOrDefault();

                                    oDataCoverageTypeDepartment.CoverageTypeUploadID = lstCoverageType.CoverageTypeUploadID;
                                    oDataCoverageTypeDepartment.DepartmentID = fieldTemp[1];
                                    oDataCoverageTypeDepartment.IsMarkupInPercentage1 = fieldTemp[2] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.MarkupAmount1 = Convert.ToDecimal(fieldTemp[3]);
                                    oDataCoverageTypeDepartment.IsDiscountInPercentage1 = fieldTemp[4] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.DiscountAmount1 = Convert.ToDecimal(fieldTemp[5]);
                                    oDataCoverageTypeDepartment.IsDiscount1UsedComp = fieldTemp[6] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.IsDiscountInPercentage1Comp1 = fieldTemp[7] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.DiscountAmount1Comp1 = Convert.ToDecimal(fieldTemp[8]);
                                    oDataCoverageTypeDepartment.IsDiscountInPercentage1Comp2 = fieldTemp[9] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.DiscountAmount1Comp2 = Convert.ToDecimal(fieldTemp[10]);
                                    oDataCoverageTypeDepartment.IsDiscountInPercentage1Comp3 = fieldTemp[11] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.DiscountAmount1Comp3 = Convert.ToDecimal(fieldTemp[12]);
                                    oDataCoverageTypeDepartment.IsCoverageInPercentage1 = fieldTemp[13] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartment.CoverageAmount1 = Convert.ToDecimal(fieldTemp[14]);
                                    oDataCoverageTypeDepartment.IsCashBackInPercentage1 = fieldTemp[15] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartment.CashBackAmount1 = Convert.ToDecimal(fieldTemp[16]);
                                    oDataCoverageTypeDepartment.IsMarkupInPercentage2 = fieldTemp[17] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartment.MarkupAmount2 = Convert.ToDecimal(fieldTemp[18]);
                                    oDataCoverageTypeDepartment.IsDiscountInPercentage2 = fieldTemp[19] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartment.DiscountAmount2 = Convert.ToDecimal(fieldTemp[20]);
                                    oDataCoverageTypeDepartment.IsDiscount2UsedComp = fieldTemp[21] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.IsDiscountInPercentage2Comp1 = fieldTemp[22] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.DiscountAmount2Comp1 = Convert.ToDecimal(fieldTemp[23]);
                                    oDataCoverageTypeDepartment.IsDiscountInPercentage2Comp2 = fieldTemp[24] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.DiscountAmount2Comp2 = Convert.ToDecimal(fieldTemp[25]);
                                    oDataCoverageTypeDepartment.IsDiscountInPercentage2Comp3 = fieldTemp[26] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.DiscountAmount2Comp3 = Convert.ToDecimal(fieldTemp[27]);
                                    oDataCoverageTypeDepartment.IsCoverageInPercentage2 = fieldTemp[28] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartment.CoverageAmount2 = Convert.ToDecimal(fieldTemp[29]);
                                    oDataCoverageTypeDepartment.IsCashBackInPercentage2 = fieldTemp[30] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartment.CashBackAmount2 = Convert.ToDecimal(fieldTemp[31]);
                                    oDataCoverageTypeDepartment.IsMarkupInPercentage3 = fieldTemp[32] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartment.MarkupAmount3 = Convert.ToDecimal(fieldTemp[33]);
                                    oDataCoverageTypeDepartment.IsDiscountInPercentage3 = fieldTemp[34] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartment.DiscountAmount3 = Convert.ToDecimal(fieldTemp[35]);
                                    oDataCoverageTypeDepartment.IsDiscount3UsedComp = fieldTemp[36] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.IsDiscountInPercentage3Comp1 = fieldTemp[37] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.DiscountAmount3Comp1 = Convert.ToDecimal(fieldTemp[38]);
                                    oDataCoverageTypeDepartment.IsDiscountInPercentage3Comp2 = fieldTemp[39] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.DiscountAmount3Comp2 = Convert.ToDecimal(fieldTemp[40]);
                                    oDataCoverageTypeDepartment.IsDiscountInPercentage3Comp3 = fieldTemp[41] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.DiscountAmount3Comp3 = Convert.ToDecimal(fieldTemp[42]);
                                    oDataCoverageTypeDepartment.IsCoverageInPercentage3 = fieldTemp[43] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartment.CoverageAmount3 = Convert.ToDecimal(fieldTemp[44]);
                                    oDataCoverageTypeDepartment.IsCashBackInPercentage3 = fieldTemp[45] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartment.CashBackAmount3 = Convert.ToDecimal(fieldTemp[46]);
                                    oDataCoverageTypeDepartment.IsDeleted = fieldTemp[47] == "True" ? true : false;
                                    oDataCoverageTypeDepartment.CreatedBy = AppSession.UserLogin.UserID;
                                    oDataCoverageTypeDepartment.CreatedDate = DateTime.Now;
                                    oDataCoverageTypeDepartment.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    oDataCoverageTypeDepartment.LastUpdatedDate = DateTime.Now;

                                    entityCoverageTypeDepartmentUploadDao.Insert(oDataCoverageTypeDepartment);
                                }
                            }
                            rowCount += 1;
                        }
                        #endregion
                    }
                    else if (coverageType == "3")
                    {
                        #region COVERAGE_TYPE_DEPARTMENT_CLASS
                        fileUploadName = "COVERAGE_TYPE_DEPARTMENT_CLASS.csv";
                        if (fileUpload != "")
                        {
                            string[] parts = Regex.Split(fileUpload, ",").Skip(1).ToArray();
                            fileUpload = String.Join(",", parts);
                        }

                        string path = AppConfigManager.QISPhysicalDirectory;
                        path += string.Format("{0}\\", AppConfigManager.QISFinanceUploadDocument.Replace('/', '\\'));

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        else
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<CoverageTypeDepartmentClassUpload> coverageTypeList = BusinessLayer.GetCoverageTypeDepartmentClassUploadList("1=1", ctx);
                            foreach (CoverageTypeDepartmentClassUpload CoverageTypeDepartmentClassBegin in coverageTypeList)
                            {
                                entityCoverageTypeDepartmentClassUploadDao.Delete(CoverageTypeDepartmentClassBegin.ID);
                            }
                        }

                        FileStream fs = new FileStream(string.Format("{0}{1}", path, fileUploadName), FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);
                        byte[] data = Convert.FromBase64String(fileUpload);
                        bw.Write(data);
                        bw.Close();

                        string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, fileUploadName));

                        int rowCount = 0;
                        foreach (string temp in lstTemp)
                        {
                            if (rowCount != 0)
                            {
                                if (temp.Contains(','))
                                {
                                    List<String> fieldTemp = temp.Split(',').ToList();

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    CoverageTypeDepartmentClassUpload oDataCoverageTypeDepartmentClass = new CoverageTypeDepartmentClassUpload();
                                    CoverageTypeUpload lstCoverageType = BusinessLayer.GetCoverageTypeUploadList(string.Format("CoverageTypeUploadCode = '{0}'", fieldTemp[0])).FirstOrDefault();
                                    ClassCare lstClassID = BusinessLayer.GetClassCareList(string.Format("ClassCode = '{0}'", fieldTemp[2])).FirstOrDefault();

                                    oDataCoverageTypeDepartmentClass.CoverageTypeUploadID = lstCoverageType.CoverageTypeUploadID;
                                    oDataCoverageTypeDepartmentClass.DepartmentID = fieldTemp[1];
                                    oDataCoverageTypeDepartmentClass.ClassID = lstClassID.ClassID;
                                    oDataCoverageTypeDepartmentClass.IsMarkupInPercentage1 = fieldTemp[3] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartmentClass.MarkupAmount1 = Convert.ToDecimal(fieldTemp[4]);
                                    oDataCoverageTypeDepartmentClass.IsDiscountInPercentage1 = fieldTemp[5] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartmentClass.DiscountAmount1 = Convert.ToDecimal(fieldTemp[6]);
                                    oDataCoverageTypeDepartmentClass.IsDiscount1UsedComp = fieldTemp[7] == "True" ? true : false;
                                    oDataCoverageTypeDepartmentClass.IsDiscountInPercentage1Comp1 = fieldTemp[8] == "True" ? true : false;
                                    oDataCoverageTypeDepartmentClass.DiscountAmount1Comp1 = Convert.ToDecimal(fieldTemp[9]);
                                    oDataCoverageTypeDepartmentClass.IsDiscountInPercentage1Comp2 = fieldTemp[10] == "True" ? true : false;
                                    oDataCoverageTypeDepartmentClass.DiscountAmount1Comp2 = Convert.ToDecimal(fieldTemp[11]);
                                    oDataCoverageTypeDepartmentClass.IsDiscountInPercentage1Comp3 = fieldTemp[12] == "True" ? true : false;
                                    oDataCoverageTypeDepartmentClass.DiscountAmount1Comp3 = Convert.ToDecimal(fieldTemp[13]);
                                    oDataCoverageTypeDepartmentClass.IsCoverageInPercentage1 = fieldTemp[14] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartmentClass.CoverageAmount1 = Convert.ToDecimal(fieldTemp[15]);
                                    oDataCoverageTypeDepartmentClass.IsCashBackInPercentage1 = fieldTemp[16] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartmentClass.CashBackAmount1 = Convert.ToDecimal(fieldTemp[17]);
                                    oDataCoverageTypeDepartmentClass.IsMarkupInPercentage2 = fieldTemp[18] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartmentClass.MarkupAmount2 = Convert.ToDecimal(fieldTemp[19]);
                                    oDataCoverageTypeDepartmentClass.IsDiscountInPercentage2 = fieldTemp[20] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartmentClass.DiscountAmount2 = Convert.ToDecimal(fieldTemp[21]);
                                    oDataCoverageTypeDepartmentClass.IsDiscount2UsedComp = fieldTemp[22] == "True" ? true : false;
                                    oDataCoverageTypeDepartmentClass.IsDiscountInPercentage2Comp1 = fieldTemp[23] == "True" ? true : false;
                                    oDataCoverageTypeDepartmentClass.DiscountAmount2Comp1 = Convert.ToDecimal(fieldTemp[24]);
                                    oDataCoverageTypeDepartmentClass.IsDiscountInPercentage2Comp2 = fieldTemp[25] == "True" ? true : false;
                                    oDataCoverageTypeDepartmentClass.DiscountAmount2Comp2 = Convert.ToDecimal(fieldTemp[26]);
                                    oDataCoverageTypeDepartmentClass.IsDiscountInPercentage2Comp3 = fieldTemp[27] == "True" ? true : false;
                                    oDataCoverageTypeDepartmentClass.DiscountAmount2Comp3 = Convert.ToDecimal(fieldTemp[28]);
                                    oDataCoverageTypeDepartmentClass.IsCoverageInPercentage2 = fieldTemp[29] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartmentClass.CoverageAmount2 = Convert.ToDecimal(fieldTemp[30]);
                                    oDataCoverageTypeDepartmentClass.IsCashBackInPercentage2 = fieldTemp[31] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartmentClass.CashBackAmount2 = Convert.ToDecimal(fieldTemp[32]);
                                    oDataCoverageTypeDepartmentClass.IsMarkupInPercentage3 = fieldTemp[33] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartmentClass.MarkupAmount3 = Convert.ToDecimal(fieldTemp[34]);
                                    oDataCoverageTypeDepartmentClass.IsDiscountInPercentage3 = fieldTemp[35] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartmentClass.DiscountAmount3 = Convert.ToDecimal(fieldTemp[36]);
                                    oDataCoverageTypeDepartmentClass.IsDiscount3UsedComp = fieldTemp[37] == "True" ? true : false;
                                    oDataCoverageTypeDepartmentClass.IsDiscountInPercentage3Comp1 = fieldTemp[38] == "True" ? true : false;
                                    oDataCoverageTypeDepartmentClass.DiscountAmount3Comp1 = Convert.ToDecimal(fieldTemp[39]);
                                    oDataCoverageTypeDepartmentClass.IsDiscountInPercentage3Comp2 = fieldTemp[40] == "True" ? true : false;
                                    oDataCoverageTypeDepartmentClass.DiscountAmount3Comp2 = Convert.ToDecimal(fieldTemp[41]);
                                    oDataCoverageTypeDepartmentClass.IsDiscountInPercentage3Comp3 = fieldTemp[42] == "True" ? true : false;
                                    oDataCoverageTypeDepartmentClass.DiscountAmount3Comp3 = Convert.ToDecimal(fieldTemp[43]);
                                    oDataCoverageTypeDepartmentClass.IsCoverageInPercentage3 = fieldTemp[44] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartmentClass.CoverageAmount3 = Convert.ToDecimal(fieldTemp[45]);
                                    oDataCoverageTypeDepartmentClass.IsCashBackInPercentage3 = fieldTemp[46] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartmentClass.CashBackAmount3 = Convert.ToDecimal(fieldTemp[47]);
                                    oDataCoverageTypeDepartmentClass.IsDeleted = fieldTemp[48] == "True" ? true : false; ;
                                    oDataCoverageTypeDepartmentClass.CreatedBy = AppSession.UserLogin.UserID;
                                    oDataCoverageTypeDepartmentClass.CreatedDate = DateTime.Now;
                                    oDataCoverageTypeDepartmentClass.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    oDataCoverageTypeDepartmentClass.LastUpdatedDate = DateTime.Now;

                                    entityCoverageTypeDepartmentClassUploadDao.Insert(oDataCoverageTypeDepartmentClass);
                                }
                            }
                            rowCount += 1;
                        }
                        #endregion
                    }
                    else if (coverageType == "4")
                    {
                        #region COVERAGE_TYPE_ITEM
                        fileUploadName = "COVERAGE_TYPE_ITEM.csv";
                        if (fileUpload != "")
                        {
                            string[] parts = Regex.Split(fileUpload, ",").Skip(1).ToArray();
                            fileUpload = String.Join(",", parts);
                        }

                        string path = AppConfigManager.QISPhysicalDirectory;
                        path += string.Format("{0}\\", AppConfigManager.QISFinanceUploadDocument.Replace('/', '\\'));

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        else
                        {
                            Directory.Delete(path, true);
                            Directory.CreateDirectory(path);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<CoverageTypeItemUpload> coverageTypeList = BusinessLayer.GetCoverageTypeItemUploadList("1=1", ctx);
                            foreach (CoverageTypeItemUpload CoverageTypeItemBegin in coverageTypeList)
                            {
                                entityCoverageTypeItemUploadDao.Delete(CoverageTypeItemBegin.ID);
                            }
                        }

                        FileStream fs = new FileStream(string.Format("{0}{1}", path, fileUploadName), FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);

                        byte[] data = Convert.FromBase64String(fileUpload);
                        bw.Write(data);
                        bw.Close();

                        string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, fileUploadName));

                        int rowCount = 0;
                        foreach (string temp in lstTemp)
                        {
                            if (rowCount != 0)
                            {
                                if (temp.Contains(','))
                                {
                                    List<String> fieldTemp = temp.Split(',').ToList();

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    CoverageTypeItemUpload oDataCoverageTypeItem = new CoverageTypeItemUpload();
                                    CoverageTypeUpload lstCoverageType = BusinessLayer.GetCoverageTypeUploadList(string.Format("CoverageTypeUploadCode = '{0}'", fieldTemp[0])).FirstOrDefault();
                                    ItemMaster lstItem = BusinessLayer.GetItemMasterList(string.Format("ItemCode = '{0}'", fieldTemp[1])).FirstOrDefault();

                                    oDataCoverageTypeItem.CoverageTypeUploadID = lstCoverageType.CoverageTypeUploadID;
                                    oDataCoverageTypeItem.ItemID = lstItem.ItemID;
                                    oDataCoverageTypeItem.ItemTariff = Convert.ToDecimal(fieldTemp[2]);
                                    oDataCoverageTypeItem.ItemTariffComp1 = Convert.ToDecimal(fieldTemp[3]);
                                    oDataCoverageTypeItem.ItemTariffComp2 = Convert.ToDecimal(fieldTemp[4]);
                                    oDataCoverageTypeItem.ItemTariffComp3 = Convert.ToDecimal(fieldTemp[5]);
                                    oDataCoverageTypeItem.IsMarkupInPercentage = fieldTemp[6] == "True" ? true : false;
                                    oDataCoverageTypeItem.MarkupAmount = Convert.ToDecimal(fieldTemp[7]);
                                    oDataCoverageTypeItem.IsDiscountInPercentage = fieldTemp[8] == "True" ? true : false;
                                    oDataCoverageTypeItem.DiscountAmount = Convert.ToDecimal(fieldTemp[9]);
                                    oDataCoverageTypeItem.IsDiscountUsedComp = fieldTemp[10] == "True" ? true : false;
                                    oDataCoverageTypeItem.IsDiscountInPercentageComp1 = fieldTemp[11] == "True" ? true : false;
                                    oDataCoverageTypeItem.DiscountAmountComp1 = Convert.ToDecimal(fieldTemp[12]);
                                    oDataCoverageTypeItem.IsDiscountInPercentageComp2 = fieldTemp[13] == "True" ? true : false;
                                    oDataCoverageTypeItem.DiscountAmountComp2 = Convert.ToDecimal(fieldTemp[14]);
                                    oDataCoverageTypeItem.IsDiscountInPercentageComp3 = fieldTemp[15] == "True" ? true : false;
                                    oDataCoverageTypeItem.DiscountAmountComp3 = Convert.ToDecimal(fieldTemp[16]);
                                    oDataCoverageTypeItem.IsCoverageInPercentage = fieldTemp[17] == "True" ? true : false;
                                    oDataCoverageTypeItem.CoverageAmount = Convert.ToDecimal(fieldTemp[18]);
                                    oDataCoverageTypeItem.IsCashBackInPercentage = fieldTemp[19] == "True" ? true : false;
                                    oDataCoverageTypeItem.CashBackAmount = Convert.ToDecimal(fieldTemp[20]);
                                    oDataCoverageTypeItem.IsDeleted = fieldTemp[21] == "True" ? true : false;
                                    oDataCoverageTypeItem.CreatedBy = AppSession.UserLogin.UserID;
                                    oDataCoverageTypeItem.CreatedDate = DateTime.Now;
                                    oDataCoverageTypeItem.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    oDataCoverageTypeItem.LastUpdatedDate = DateTime.Now;

                                    entityCoverageTypeItemUploadDao.Insert(oDataCoverageTypeItem);
                                }
                            }
                            rowCount += 1;
                        }
                        #endregion
                    }
                    else if (coverageType == "5")
                    {
                        #region COVERAGE_TYPE_ITEM_CLASS
                        fileUploadName = "COVERAGE_TYPE_ITEM_CLASS.csv";
                        if (fileUpload != "")
                        {
                            string[] parts = Regex.Split(fileUpload, ",").Skip(1).ToArray();
                            fileUpload = String.Join(",", parts);
                        }

                        string path = AppConfigManager.QISPhysicalDirectory;
                        path += string.Format("{0}\\", AppConfigManager.QISFinanceUploadDocument.Replace('/', '\\'));

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        else
                        {
                            Directory.Delete(path, true);
                            Directory.CreateDirectory(path);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<CoverageTypeItemClassUpload> coverageTypeList = BusinessLayer.GetCoverageTypeItemClassUploadList("1=1", ctx);
                            foreach (CoverageTypeItemClassUpload CoverageTypeItemClassBegin in coverageTypeList)
                            {
                                entityCoverageTypeItemClassUploadDao.Delete(CoverageTypeItemClassBegin.ID);
                            }
                        }

                        FileStream fs = new FileStream(string.Format("{0}{1}", path, fileUploadName), FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);

                        byte[] data = Convert.FromBase64String(fileUpload);
                        bw.Write(data);
                        bw.Close();

                        string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, fileUploadName));

                        int rowCount = 0;
                        foreach (string temp in lstTemp)
                        {
                            if (rowCount != 0)
                            {
                                if (temp.Contains(','))
                                {
                                    List<String> fieldTemp = temp.Split(',').ToList();

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    CoverageTypeItemClassUpload oDataCoverageTypeItemClass = new CoverageTypeItemClassUpload();
                                    CoverageTypeUpload lstCoverageType = BusinessLayer.GetCoverageTypeUploadList(string.Format("CoverageTypeUploadCode = '{0}'", fieldTemp[0])).FirstOrDefault();
                                    ItemMaster lstItem = BusinessLayer.GetItemMasterList(string.Format("ItemCode = '{0}'", fieldTemp[1])).FirstOrDefault();
                                    ClassCare lstClassID = BusinessLayer.GetClassCareList(string.Format("ClassCode = '{0}'", fieldTemp[2])).FirstOrDefault();

                                    oDataCoverageTypeItemClass.CoverageTypeUploadID = lstCoverageType.CoverageTypeUploadID;
                                    oDataCoverageTypeItemClass.ItemID = lstItem.ItemID;
                                    oDataCoverageTypeItemClass.ClassID = lstClassID.ClassID;
                                    oDataCoverageTypeItemClass.IsMarkupInPercentage = fieldTemp[3] == "True" ? true : false;
                                    oDataCoverageTypeItemClass.MarkupAmount = Convert.ToDecimal(fieldTemp[4]);
                                    oDataCoverageTypeItemClass.IsDiscountInPercentage = fieldTemp[5] == "True" ? true : false;
                                    oDataCoverageTypeItemClass.DiscountAmount = Convert.ToDecimal(fieldTemp[6]);
                                    oDataCoverageTypeItemClass.IsDiscountUsedComp = fieldTemp[7] == "True" ? true : false;
                                    oDataCoverageTypeItemClass.IsDiscountInPercentageComp1 = fieldTemp[8] == "True" ? true : false;
                                    oDataCoverageTypeItemClass.DiscountAmountComp1 = Convert.ToDecimal(fieldTemp[9]);
                                    oDataCoverageTypeItemClass.IsDiscountInPercentageComp2 = fieldTemp[10] == "True" ? true : false;
                                    oDataCoverageTypeItemClass.DiscountAmountComp2 = Convert.ToDecimal(fieldTemp[11]);
                                    oDataCoverageTypeItemClass.IsDiscountInPercentageComp3 = fieldTemp[12] == "True" ? true : false;
                                    oDataCoverageTypeItemClass.DiscountAmountComp3 = Convert.ToDecimal(fieldTemp[13]);
                                    oDataCoverageTypeItemClass.IsCoverageInPercentage = fieldTemp[14] == "True" ? true : false;
                                    oDataCoverageTypeItemClass.CoverageAmount = Convert.ToDecimal(fieldTemp[15]);
                                    oDataCoverageTypeItemClass.IsCashBackInPercentage = fieldTemp[16] == "True" ? true : false;
                                    oDataCoverageTypeItemClass.CashBackAmount = Convert.ToDecimal(fieldTemp[17]);
                                    oDataCoverageTypeItemClass.IsDeleted = fieldTemp[18] == "True" ? true : false;
                                    oDataCoverageTypeItemClass.CreatedBy = AppSession.UserLogin.UserID;
                                    oDataCoverageTypeItemClass.CreatedDate = DateTime.Now;
                                    oDataCoverageTypeItemClass.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    oDataCoverageTypeItemClass.LastUpdatedDate = DateTime.Now;

                                    entityCoverageTypeItemClassUploadDao.Insert(oDataCoverageTypeItemClass);
                                }
                            }
                            rowCount += 1;
                        }

                        #endregion
                    }
                    else if (coverageType == "6")
                    {
                        #region COVERAGE_TYPE_ITEM_GROUP
                        fileUploadName = "COVERAGE_TYPE_ITEM_GROUP.csv";
                        if (fileUpload != "")
                        {
                            string[] parts = Regex.Split(fileUpload, ",").Skip(1).ToArray();
                            fileUpload = String.Join(",", parts);
                        }

                        string path = AppConfigManager.QISPhysicalDirectory;
                        path += string.Format("{0}\\", AppConfigManager.QISFinanceUploadDocument.Replace('/', '\\'));

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        else
                        {
                            Directory.Delete(path, true);
                            Directory.CreateDirectory(path);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<CoverageTypeItemGroupUpload> coverageTypeList = BusinessLayer.GetCoverageTypeItemGroupUploadList("1=1", ctx);
                            foreach (CoverageTypeItemGroupUpload CoverageTypeItemGroupBegin in coverageTypeList)
                            {
                                entityCoverageTypeItemGroupUploadDao.Delete(CoverageTypeItemGroupBegin.ID);
                            }
                        }

                        FileStream fs = new FileStream(string.Format("{0}{1}", path, fileUploadName), FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);

                        byte[] data = Convert.FromBase64String(fileUpload);
                        bw.Write(data);
                        bw.Close();

                        string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, fileUploadName));

                        int rowCount = 0;

                        foreach (string temp in lstTemp)
                        {
                            if (rowCount != 0)
                            {
                                if (temp.Contains(','))
                                {
                                    List<String> fieldTemp = temp.Split(',').ToList();

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    CoverageTypeItemGroupUpload oDataCoverageTypeItemGroup = new CoverageTypeItemGroupUpload();
                                    CoverageTypeUpload lstCoverageType = BusinessLayer.GetCoverageTypeUploadList(string.Format("CoverageTypeUploadCode = '{0}'", fieldTemp[0])).FirstOrDefault();
                                    StandardCode lstItemType = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeName = '{0}' AND ParentID = '{1}'", fieldTemp[1], Constant.StandardCode.ITEM_TYPE)).FirstOrDefault();
                                    ItemGroupMaster lstItem = BusinessLayer.GetItemGroupMasterList(string.Format("ItemGroupCode = '{0}'", fieldTemp[2])).FirstOrDefault();

                                    oDataCoverageTypeItemGroup.CoverageTypeUploadID = lstCoverageType.CoverageTypeUploadID;
                                    oDataCoverageTypeItemGroup.GCItemType = lstItemType.StandardCodeID;
                                    oDataCoverageTypeItemGroup.ItemGroupID = lstItem.ItemGroupID;
                                    oDataCoverageTypeItemGroup.IsMarkupInPercentage = fieldTemp[3] == "True" ? true : false;
                                    oDataCoverageTypeItemGroup.MarkupAmount = Convert.ToDecimal(fieldTemp[4]);
                                    oDataCoverageTypeItemGroup.IsDiscountInPercentage = fieldTemp[5] == "True" ? true : false;
                                    oDataCoverageTypeItemGroup.DiscountAmount = Convert.ToDecimal(fieldTemp[6]);
                                    oDataCoverageTypeItemGroup.IsDiscountUsedComp = fieldTemp[7] == "True" ? true : false;
                                    oDataCoverageTypeItemGroup.IsDiscountInPercentageComp1 = fieldTemp[8] == "True" ? true : false;
                                    oDataCoverageTypeItemGroup.DiscountAmountComp1 = Convert.ToDecimal(fieldTemp[9]);
                                    oDataCoverageTypeItemGroup.IsDiscountInPercentageComp2 = fieldTemp[10] == "True" ? true : false;
                                    oDataCoverageTypeItemGroup.DiscountAmountComp2 = Convert.ToDecimal(fieldTemp[11]);
                                    oDataCoverageTypeItemGroup.IsDiscountInPercentageComp3 = fieldTemp[12] == "True" ? true : false;
                                    oDataCoverageTypeItemGroup.DiscountAmountComp3 = Convert.ToDecimal(fieldTemp[13]);
                                    oDataCoverageTypeItemGroup.IsCoverageInPercentage = fieldTemp[14] == "True" ? true : false;
                                    oDataCoverageTypeItemGroup.CoverageAmount = Convert.ToDecimal(fieldTemp[15]);
                                    oDataCoverageTypeItemGroup.IsCashBackInPercentage = fieldTemp[16] == "True" ? true : false;
                                    oDataCoverageTypeItemGroup.CashBackAmount = Convert.ToDecimal(fieldTemp[17]);
                                    oDataCoverageTypeItemGroup.IsDeleted = fieldTemp[18] == "True" ? true : false;
                                    oDataCoverageTypeItemGroup.CreatedDate = DateTime.Now;
                                    oDataCoverageTypeItemGroup.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    oDataCoverageTypeItemGroup.LastUpdatedDate = DateTime.Now;

                                    entityCoverageTypeItemGroupUploadDao.Insert(oDataCoverageTypeItemGroup);
                                }
                            }
                            rowCount += 1;
                        }
                        #endregion
                    }
                    else if (coverageType == "7")
                    {
                        #region COVERAGE_TYPE_ITEM_GROUP
                        fileUploadName = "COVERAGE_TYPE_ITEM_GROUP_CLASS.csv";
                        if (fileUpload != "")
                        {
                            string[] parts = Regex.Split(fileUpload, ",").Skip(1).ToArray();
                            fileUpload = String.Join(",", parts);
                        }

                        string path = AppConfigManager.QISPhysicalDirectory;
                        path += string.Format("{0}\\", AppConfigManager.QISFinanceUploadDocument.Replace('/', '\\'));

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        else
                        {
                            Directory.Delete(path, true);
                            Directory.CreateDirectory(path);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<CoverageTypeItemGroupClassUpload> coverageTypeList = BusinessLayer.GetCoverageTypeItemGroupClassUploadList("1=1", ctx);
                            foreach (CoverageTypeItemGroupClassUpload CoverageTypeItemGroupClassBegin in coverageTypeList)
                            {
                                entityCoverageTypeItemGroupClassUploadDao.Delete(CoverageTypeItemGroupClassBegin.ID);
                            }
                        }

                        FileStream fs = new FileStream(string.Format("{0}{1}", path, fileUploadName), FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);

                        byte[] data = Convert.FromBase64String(fileUpload);
                        bw.Write(data);
                        bw.Close();

                        string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, fileUploadName));

                        int rowCount = 0;

                        foreach (string temp in lstTemp)
                        {
                            if (rowCount != 0)
                            {
                                if (temp.Contains(','))
                                {
                                    List<String> fieldTemp = temp.Split(',').ToList();

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    CoverageTypeItemGroupClassUpload oDataCoverageTypeItemGroupClass = new CoverageTypeItemGroupClassUpload();
                                    CoverageTypeUpload lstCoverageType = BusinessLayer.GetCoverageTypeUploadList(string.Format("CoverageTypeUploadCode = '{0}'", fieldTemp[0])).FirstOrDefault();
                                    StandardCode lstItemType = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeName = '{0}' AND ParentID = '{1}'", fieldTemp[1], Constant.StandardCode.ITEM_TYPE)).FirstOrDefault();
                                    ItemGroupMaster lstItem = BusinessLayer.GetItemGroupMasterList(string.Format("ItemGroupCode = '{0}'", fieldTemp[2])).FirstOrDefault();
                                    ClassCare lstClassID = BusinessLayer.GetClassCareList(string.Format("ClassCode = '{0}'", fieldTemp[3])).FirstOrDefault();

                                    oDataCoverageTypeItemGroupClass.CoverageTypeUploadID = lstCoverageType.CoverageTypeUploadID;
                                    oDataCoverageTypeItemGroupClass.GCItemType = lstItemType.StandardCodeID;
                                    oDataCoverageTypeItemGroupClass.ItemGroupID = lstItem.ItemGroupID;
                                    oDataCoverageTypeItemGroupClass.ClassID = lstClassID.ClassID;
                                    oDataCoverageTypeItemGroupClass.IsMarkupInPercentage = fieldTemp[4] == "True" ? true : false;
                                    oDataCoverageTypeItemGroupClass.MarkupAmount = Convert.ToDecimal(fieldTemp[5]);
                                    oDataCoverageTypeItemGroupClass.IsDiscountInPercentage = fieldTemp[6] == "True" ? true : false;
                                    oDataCoverageTypeItemGroupClass.DiscountAmount = Convert.ToDecimal(fieldTemp[7]);
                                    oDataCoverageTypeItemGroupClass.IsDiscountUsedComp = fieldTemp[8] == "True" ? true : false;
                                    oDataCoverageTypeItemGroupClass.IsDiscountInPercentageComp1 = fieldTemp[9] == "True" ? true : false;
                                    oDataCoverageTypeItemGroupClass.DiscountAmountComp1 = Convert.ToDecimal(fieldTemp[10]);
                                    oDataCoverageTypeItemGroupClass.IsDiscountInPercentageComp2 = fieldTemp[11] == "True" ? true : false;
                                    oDataCoverageTypeItemGroupClass.DiscountAmountComp2 = Convert.ToDecimal(fieldTemp[12]);
                                    oDataCoverageTypeItemGroupClass.IsDiscountInPercentageComp3 = fieldTemp[13] == "True" ? true : false;
                                    oDataCoverageTypeItemGroupClass.DiscountAmountComp3 = Convert.ToDecimal(fieldTemp[14]);
                                    oDataCoverageTypeItemGroupClass.IsCoverageInPercentage = fieldTemp[15] == "True" ? true : false;
                                    oDataCoverageTypeItemGroupClass.CoverageAmount = Convert.ToDecimal(fieldTemp[16]);
                                    oDataCoverageTypeItemGroupClass.IsCashBackInPercentage = fieldTemp[17] == "True" ? true : false;
                                    oDataCoverageTypeItemGroupClass.CashBackAmount = Convert.ToDecimal(fieldTemp[18]);
                                    oDataCoverageTypeItemGroupClass.IsDeleted = fieldTemp[19] == "True" ? true : false;
                                    oDataCoverageTypeItemGroupClass.CreatedBy = AppSession.UserLogin.UserID;
                                    oDataCoverageTypeItemGroupClass.CreatedDate = DateTime.Now;
                                    oDataCoverageTypeItemGroupClass.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    oDataCoverageTypeItemGroupClass.LastUpdatedDate = DateTime.Now;

                                    entityCoverageTypeItemGroupClassUploadDao.Insert(oDataCoverageTypeItemGroupClass);
                                }
                            }
                            rowCount += 1;
                        }
                        #endregion
                    }
                    else
                    {
                        #region COVERAGE_TYPE_SERVICE_UNIT
                        fileUploadName = "COVERAGE_TYPE_SERVICE_UNIT.csv";
                        if (fileUpload != "")
                        {
                            string[] parts = Regex.Split(fileUpload, ",").Skip(1).ToArray();
                            fileUpload = String.Join(",", parts);
                        }

                        string path = AppConfigManager.QISPhysicalDirectory;
                        path += string.Format("{0}\\", AppConfigManager.QISFinanceUploadDocument.Replace('/', '\\'));

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        else
                        {
                            Directory.Delete(path, true);
                            Directory.CreateDirectory(path);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<CoverageTypeServiceUnitUpload> coverageTypeList = BusinessLayer.GetCoverageTypeServiceUnitUploadList("1=1", ctx);
                            foreach (CoverageTypeServiceUnitUpload CoverageTypeServiceUnitBegin in coverageTypeList)
                            {
                                entityCoverageTypeServiceUnitUploadDao.Delete(CoverageTypeServiceUnitBegin.ID);
                            }

                            FileStream fs = new FileStream(string.Format("{0}{1}", path, fileUploadName), FileMode.Create);
                            BinaryWriter bw = new BinaryWriter(fs);

                            byte[] data = Convert.FromBase64String(fileUpload);
                            bw.Write(data);
                            bw.Close();

                            string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, fileUploadName));

                            int rowCount = 0;
                            foreach (string temp in lstTemp)
                            {
                                if (rowCount != 0)
                                {
                                    if (temp.Contains(','))
                                    {
                                        List<String> fieldTemp = temp.Split(',').ToList();

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        CoverageTypeServiceUnitUpload oDataCoverageTypeServiceUnit = new CoverageTypeServiceUnitUpload();
                                        CoverageTypeUpload lstCoverageType = BusinessLayer.GetCoverageTypeUploadList(string.Format("CoverageTypeUploadCode = '{0}'", fieldTemp[0])).FirstOrDefault();
                                        ServiceUnitMaster lstServiceUnit = BusinessLayer.GetServiceUnitMasterList(string.Format("ServiceUnitCode = '{0}'", fieldTemp[1])).FirstOrDefault();

                                        oDataCoverageTypeServiceUnit.CoverageTypeUploadID = lstCoverageType.CoverageTypeUploadID;
                                        oDataCoverageTypeServiceUnit.ServiceUnitID = lstServiceUnit.ServiceUnitID;
                                        oDataCoverageTypeServiceUnit.IsMarkupInPercentage1 = fieldTemp[2] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.MarkupAmount1 = Convert.ToDecimal(fieldTemp[3]);
                                        oDataCoverageTypeServiceUnit.IsDiscountInPercentage1 = fieldTemp[4] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.DiscountAmount1 = Convert.ToDecimal(fieldTemp[5]);
                                        oDataCoverageTypeServiceUnit.IsDiscount1UsedComp = fieldTemp[6] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.IsDiscountInPercentage1Comp1 = fieldTemp[7] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.DiscountAmount1Comp1 = Convert.ToDecimal(fieldTemp[8]);
                                        oDataCoverageTypeServiceUnit.IsDiscountInPercentage1Comp2 = fieldTemp[9] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.DiscountAmount1Comp2 = Convert.ToDecimal(fieldTemp[10]);
                                        oDataCoverageTypeServiceUnit.IsDiscountInPercentage1Comp3 = fieldTemp[11] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.DiscountAmount1Comp3 = Convert.ToDecimal(fieldTemp[12]);
                                        oDataCoverageTypeServiceUnit.IsCoverageInPercentage1 = fieldTemp[13] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.CoverageAmount1 = Convert.ToDecimal(fieldTemp[14]);
                                        oDataCoverageTypeServiceUnit.IsCashBackInPercentage1 = fieldTemp[15] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.CashBackAmount1 = Convert.ToDecimal(fieldTemp[16]);
                                        oDataCoverageTypeServiceUnit.IsMarkupInPercentage2 = fieldTemp[17] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.MarkupAmount2 = Convert.ToDecimal(fieldTemp[18]);
                                        oDataCoverageTypeServiceUnit.IsDiscountInPercentage2 = fieldTemp[9] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.DiscountAmount2 = Convert.ToDecimal(fieldTemp[20]);
                                        oDataCoverageTypeServiceUnit.IsDiscount2UsedComp = fieldTemp[21] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.IsDiscountInPercentage2Comp1 = fieldTemp[22] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.DiscountAmount2Comp1 = Convert.ToDecimal(fieldTemp[23]);
                                        oDataCoverageTypeServiceUnit.IsDiscountInPercentage2Comp2 = fieldTemp[24] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.DiscountAmount2Comp2 = Convert.ToDecimal(fieldTemp[25]);
                                        oDataCoverageTypeServiceUnit.IsDiscountInPercentage2Comp3 = fieldTemp[26] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.DiscountAmount2Comp3 = Convert.ToDecimal(fieldTemp[27]);
                                        oDataCoverageTypeServiceUnit.IsCoverageInPercentage2 = fieldTemp[28] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.CoverageAmount2 = Convert.ToDecimal(fieldTemp[29]);
                                        oDataCoverageTypeServiceUnit.IsCashBackInPercentage2 = fieldTemp[30] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.CashBackAmount2 = Convert.ToDecimal(fieldTemp[31]);
                                        oDataCoverageTypeServiceUnit.IsMarkupInPercentage3 = fieldTemp[32] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.MarkupAmount3 = Convert.ToDecimal(fieldTemp[33]);
                                        oDataCoverageTypeServiceUnit.IsDiscountInPercentage3 = fieldTemp[34] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.DiscountAmount3 = Convert.ToDecimal(fieldTemp[35]);
                                        oDataCoverageTypeServiceUnit.IsDiscount3UsedComp = fieldTemp[36] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.IsDiscountInPercentage3Comp1 = fieldTemp[37] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.DiscountAmount3Comp1 = Convert.ToDecimal(fieldTemp[38]);
                                        oDataCoverageTypeServiceUnit.IsDiscountInPercentage3Comp2 = fieldTemp[39] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.DiscountAmount3Comp2 = Convert.ToDecimal(fieldTemp[40]);
                                        oDataCoverageTypeServiceUnit.IsDiscountInPercentage3Comp3 = fieldTemp[41] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.DiscountAmount3Comp3 = Convert.ToDecimal(fieldTemp[42]);
                                        oDataCoverageTypeServiceUnit.IsCoverageInPercentage3 = fieldTemp[43] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.CoverageAmount3 = Convert.ToDecimal(fieldTemp[44]);
                                        oDataCoverageTypeServiceUnit.IsCashBackInPercentage3 = fieldTemp[45] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.CashBackAmount3 = Convert.ToDecimal(fieldTemp[46]);
                                        oDataCoverageTypeServiceUnit.IsDeleted = fieldTemp[47] == "True" ? true : false;
                                        oDataCoverageTypeServiceUnit.CreatedBy = AppSession.UserLogin.UserID;
                                        oDataCoverageTypeServiceUnit.CreatedDate = DateTime.Now;
                                        oDataCoverageTypeServiceUnit.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        oDataCoverageTypeServiceUnit.LastUpdatedDate = DateTime.Now;

                                        entityCoverageTypeServiceUnitUploadDao.Insert(oDataCoverageTypeServiceUnit);
                                    }
                                }
                                rowCount += 1;
                            }
                        }
                        #endregion
                    }

                    #endregion
                }
                else if (type == "save")
                {
                    #region Save to Origin Table

                    if (coverageType == "1")
                    {
                        #region COVERAGE_TYPE

                        string filterCoverageTypeUpload = "IsDeleted = 0";
                        List<CoverageTypeUpload> lstCoverageTypeUpload = BusinessLayer.GetCoverageTypeUploadList(filterCoverageTypeUpload, ctx);
                        foreach (CoverageTypeUpload entityUpload in lstCoverageTypeUpload)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            CoverageType oCoverageType = new CoverageType();
                            oCoverageType.CoverageTypeCode = entityUpload.CoverageTypeUploadCode;
                            oCoverageType.CoverageTypeName = entityUpload.CoverageTypeUploadName;
                            oCoverageType.IsMarkupMarginReplaceDefault = entityUpload.IsMarkupMarginReplaceDefault;
                            oCoverageType.IsDeleted = entityUpload.IsDeleted;
                            oCoverageType.CreatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeDao.Insert(oCoverageType);

                            entityUpload.IsDeleted = true;
                            entityUpload.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeUploadDao.Update(entityUpload);
                        }
                        #endregion
                    }
                    else if (coverageType == "2")
                    {
                        #region COVERAGE_TYPE_DEPARTMENT

                        string filterCoverageTypeDepartmentUpload = "IsDeleted = 0";
                        List<CoverageTypeDepartmentUpload> lstCoverageTypeDepartmentUpload = BusinessLayer.GetCoverageTypeDepartmentUploadList(filterCoverageTypeDepartmentUpload, ctx);
                        foreach (CoverageTypeDepartmentUpload oCoverageTypeDepartmentUpload in lstCoverageTypeDepartmentUpload)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            CoverageTypeDepartment oCoverageTypeDepartment = new CoverageTypeDepartment();
                            CoverageTypeUpload oCoverageTypeUpload = BusinessLayer.GetCoverageTypeUploadList(string.Format("CoverageTypeUploadID = {0}", oCoverageTypeDepartmentUpload.CoverageTypeUploadID))[0];
                            CoverageType oCoverageType = BusinessLayer.GetCoverageTypeList(string.Format("CoverageTypeCode = '{0}'", oCoverageTypeUpload.CoverageTypeUploadCode))[0];

                            oCoverageTypeDepartment.CoverageTypeID = oCoverageType.CoverageTypeID;
                            oCoverageTypeDepartment.DepartmentID = oCoverageTypeDepartmentUpload.DepartmentID;
                            oCoverageTypeDepartment.IsMarkupInPercentage1 = oCoverageTypeDepartmentUpload.IsMarkupInPercentage1;
                            oCoverageTypeDepartment.MarkupAmount1 = oCoverageTypeDepartmentUpload.MarkupAmount1;
                            oCoverageTypeDepartment.IsDiscountInPercentage1 = oCoverageTypeDepartmentUpload.IsDiscountInPercentage1;
                            oCoverageTypeDepartment.DiscountAmount1 = oCoverageTypeDepartmentUpload.DiscountAmount1;
                            oCoverageTypeDepartment.IsDiscount1UsedComp = oCoverageTypeDepartmentUpload.IsDiscount1UsedComp;
                            oCoverageTypeDepartment.IsDiscountInPercentage1Comp1 = oCoverageTypeDepartmentUpload.IsDiscountInPercentage1Comp1;
                            oCoverageTypeDepartment.DiscountAmount1Comp1 = oCoverageTypeDepartmentUpload.DiscountAmount1Comp1;
                            oCoverageTypeDepartment.IsDiscountInPercentage1Comp2 = oCoverageTypeDepartmentUpload.IsDiscountInPercentage1Comp2;
                            oCoverageTypeDepartment.DiscountAmount1Comp2 = oCoverageTypeDepartmentUpload.DiscountAmount1Comp2;
                            oCoverageTypeDepartment.IsDiscountInPercentage1Comp3 = oCoverageTypeDepartmentUpload.IsDiscountInPercentage1Comp3;
                            oCoverageTypeDepartment.DiscountAmount1Comp3 = oCoverageTypeDepartmentUpload.DiscountAmount1Comp3;
                            oCoverageTypeDepartment.IsCoverageInPercentage1 = oCoverageTypeDepartmentUpload.IsCoverageInPercentage1;
                            oCoverageTypeDepartment.CoverageAmount1 = oCoverageTypeDepartmentUpload.CoverageAmount1;
                            oCoverageTypeDepartment.IsCashBackInPercentage1 = oCoverageTypeDepartmentUpload.IsCashBackInPercentage1;
                            oCoverageTypeDepartment.CashBackAmount1 = oCoverageTypeDepartmentUpload.CashBackAmount1;
                            oCoverageTypeDepartment.IsMarkupInPercentage2 = oCoverageTypeDepartmentUpload.IsMarkupInPercentage2;
                            oCoverageTypeDepartment.MarkupAmount2 = oCoverageTypeDepartmentUpload.MarkupAmount2;
                            oCoverageTypeDepartment.IsDiscountInPercentage2 = oCoverageTypeDepartmentUpload.IsDiscountInPercentage2;
                            oCoverageTypeDepartment.DiscountAmount2 = oCoverageTypeDepartmentUpload.DiscountAmount2;
                            oCoverageTypeDepartment.IsDiscount2UsedComp = oCoverageTypeDepartmentUpload.IsDiscount2UsedComp;
                            oCoverageTypeDepartment.IsDiscountInPercentage2Comp1 = oCoverageTypeDepartmentUpload.IsDiscountInPercentage2Comp1;
                            oCoverageTypeDepartment.DiscountAmount2Comp1 = oCoverageTypeDepartmentUpload.DiscountAmount2Comp1;
                            oCoverageTypeDepartment.IsDiscountInPercentage2Comp2 = oCoverageTypeDepartmentUpload.IsDiscountInPercentage2Comp2;
                            oCoverageTypeDepartment.DiscountAmount2Comp2 = oCoverageTypeDepartmentUpload.DiscountAmount2Comp2;
                            oCoverageTypeDepartment.IsDiscountInPercentage2Comp3 = oCoverageTypeDepartmentUpload.IsDiscountInPercentage2Comp3;
                            oCoverageTypeDepartment.DiscountAmount2Comp3 = oCoverageTypeDepartmentUpload.DiscountAmount2Comp3;
                            oCoverageTypeDepartment.IsCoverageInPercentage2 = oCoverageTypeDepartmentUpload.IsCoverageInPercentage2;
                            oCoverageTypeDepartment.CoverageAmount2 = oCoverageTypeDepartmentUpload.CoverageAmount2;
                            oCoverageTypeDepartment.IsCashBackInPercentage2 = oCoverageTypeDepartmentUpload.IsCashBackInPercentage2;
                            oCoverageTypeDepartment.CashBackAmount2 = oCoverageTypeDepartmentUpload.CashBackAmount2;
                            oCoverageTypeDepartment.IsMarkupInPercentage3 = oCoverageTypeDepartmentUpload.IsMarkupInPercentage3;
                            oCoverageTypeDepartment.MarkupAmount3 = oCoverageTypeDepartmentUpload.MarkupAmount3;
                            oCoverageTypeDepartment.IsDiscountInPercentage3 = oCoverageTypeDepartmentUpload.IsDiscountInPercentage3;
                            oCoverageTypeDepartment.DiscountAmount3 = oCoverageTypeDepartmentUpload.DiscountAmount3;
                            oCoverageTypeDepartment.IsDiscount3UsedComp = oCoverageTypeDepartmentUpload.IsDiscount3UsedComp;
                            oCoverageTypeDepartment.IsDiscountInPercentage3Comp1 = oCoverageTypeDepartmentUpload.IsDiscountInPercentage3Comp1;
                            oCoverageTypeDepartment.DiscountAmount3Comp1 = oCoverageTypeDepartmentUpload.DiscountAmount3Comp1;
                            oCoverageTypeDepartment.IsDiscountInPercentage3Comp2 = oCoverageTypeDepartmentUpload.IsDiscountInPercentage3Comp2;
                            oCoverageTypeDepartment.DiscountAmount3Comp2 = oCoverageTypeDepartmentUpload.DiscountAmount3Comp2;
                            oCoverageTypeDepartment.IsDiscountInPercentage3Comp3 = oCoverageTypeDepartmentUpload.IsDiscountInPercentage3Comp3;
                            oCoverageTypeDepartment.DiscountAmount3Comp3 = oCoverageTypeDepartmentUpload.DiscountAmount3Comp3;
                            oCoverageTypeDepartment.IsCoverageInPercentage3 = oCoverageTypeDepartmentUpload.IsCoverageInPercentage3;
                            oCoverageTypeDepartment.CoverageAmount3 = oCoverageTypeDepartmentUpload.CoverageAmount3;
                            oCoverageTypeDepartment.IsCashBackInPercentage3 = oCoverageTypeDepartmentUpload.IsCashBackInPercentage3;
                            oCoverageTypeDepartment.CashBackAmount3 = oCoverageTypeDepartmentUpload.CashBackAmount3;
                            oCoverageTypeDepartment.IsDeleted = oCoverageTypeDepartmentUpload.IsDeleted;

                            oCoverageTypeDepartment.CreatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeDepartmentDao.Insert(oCoverageTypeDepartment);

                            oCoverageTypeDepartmentUpload.IsDeleted = true;
                            oCoverageTypeDepartmentUpload.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeDepartmentUploadDao.Update(oCoverageTypeDepartmentUpload);
                        }
                        #endregion
                    }
                    else if (coverageType == "3")
                    {
                        #region COVERAGE_TYPE_DEPARTMENT_CLASS

                        string filterCoverageTypeDepartmentClassUpload = "IsDeleted = 0";
                        List<CoverageTypeDepartmentClassUpload> lstCoverageTypeUpload = BusinessLayer.GetCoverageTypeDepartmentClassUploadList(filterCoverageTypeDepartmentClassUpload, ctx);
                        foreach (CoverageTypeDepartmentClassUpload entityDepartmentClassUpload in lstCoverageTypeUpload)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            CoverageTypeDepartmentClass oCoverageTypeDepartmentClass = new CoverageTypeDepartmentClass();
                            CoverageTypeUpload oCoverageTypeUpload = BusinessLayer.GetCoverageTypeUploadList(string.Format("CoverageTypeUploadID = {0}", entityDepartmentClassUpload.CoverageTypeUploadID))[0];
                            CoverageType oCoverageType = BusinessLayer.GetCoverageTypeList(string.Format("CoverageTypeCode = '{0}'", oCoverageTypeUpload.CoverageTypeUploadCode))[0];

                            oCoverageTypeDepartmentClass.CoverageTypeID = oCoverageType.CoverageTypeID;
                            oCoverageTypeDepartmentClass.DepartmentID = entityDepartmentClassUpload.DepartmentID;
                            oCoverageTypeDepartmentClass.ClassID = entityDepartmentClassUpload.ClassID;
                            oCoverageTypeDepartmentClass.IsMarkupInPercentage1 = entityDepartmentClassUpload.IsMarkupInPercentage1;
                            oCoverageTypeDepartmentClass.MarkupAmount1 = entityDepartmentClassUpload.MarkupAmount1;
                            oCoverageTypeDepartmentClass.IsDiscountInPercentage1 = entityDepartmentClassUpload.IsDiscountInPercentage1;
                            oCoverageTypeDepartmentClass.DiscountAmount1 = entityDepartmentClassUpload.DiscountAmount1;
                            oCoverageTypeDepartmentClass.IsDiscount1UsedComp = entityDepartmentClassUpload.IsDiscount1UsedComp;
                            oCoverageTypeDepartmentClass.IsDiscountInPercentage1Comp1 = entityDepartmentClassUpload.IsDiscountInPercentage1Comp1;
                            oCoverageTypeDepartmentClass.DiscountAmount1Comp1 = entityDepartmentClassUpload.DiscountAmount1Comp1;
                            oCoverageTypeDepartmentClass.IsDiscountInPercentage1Comp2 = entityDepartmentClassUpload.IsDiscountInPercentage1Comp2;
                            oCoverageTypeDepartmentClass.DiscountAmount1Comp2 = entityDepartmentClassUpload.DiscountAmount1Comp2;
                            oCoverageTypeDepartmentClass.IsDiscountInPercentage1Comp3 = entityDepartmentClassUpload.IsDiscountInPercentage1Comp3;
                            oCoverageTypeDepartmentClass.DiscountAmount1Comp3 = entityDepartmentClassUpload.DiscountAmount1Comp3;
                            oCoverageTypeDepartmentClass.IsCoverageInPercentage1 = entityDepartmentClassUpload.IsCoverageInPercentage1;
                            oCoverageTypeDepartmentClass.CoverageAmount1 = entityDepartmentClassUpload.CoverageAmount1;
                            oCoverageTypeDepartmentClass.IsCashBackInPercentage1 = entityDepartmentClassUpload.IsCashBackInPercentage1;
                            oCoverageTypeDepartmentClass.CashBackAmount1 = entityDepartmentClassUpload.CashBackAmount1;
                            oCoverageTypeDepartmentClass.IsMarkupInPercentage2 = entityDepartmentClassUpload.IsMarkupInPercentage2;
                            oCoverageTypeDepartmentClass.MarkupAmount2 = entityDepartmentClassUpload.MarkupAmount2;
                            oCoverageTypeDepartmentClass.IsDiscountInPercentage2 = entityDepartmentClassUpload.IsDiscountInPercentage2;
                            oCoverageTypeDepartmentClass.DiscountAmount2 = entityDepartmentClassUpload.DiscountAmount2;
                            oCoverageTypeDepartmentClass.IsDiscount2UsedComp = entityDepartmentClassUpload.IsDiscount2UsedComp;
                            oCoverageTypeDepartmentClass.IsDiscountInPercentage2Comp1 = entityDepartmentClassUpload.IsDiscountInPercentage2Comp1;
                            oCoverageTypeDepartmentClass.DiscountAmount2Comp1 = entityDepartmentClassUpload.DiscountAmount2Comp1;
                            oCoverageTypeDepartmentClass.IsDiscountInPercentage2Comp2 = entityDepartmentClassUpload.IsDiscountInPercentage2Comp2;
                            oCoverageTypeDepartmentClass.DiscountAmount2Comp2 = entityDepartmentClassUpload.DiscountAmount2Comp2;
                            oCoverageTypeDepartmentClass.IsDiscountInPercentage2Comp3 = entityDepartmentClassUpload.IsDiscountInPercentage2Comp3;
                            oCoverageTypeDepartmentClass.DiscountAmount2Comp3 = entityDepartmentClassUpload.DiscountAmount2Comp3;
                            oCoverageTypeDepartmentClass.IsCoverageInPercentage2 = entityDepartmentClassUpload.IsCoverageInPercentage2;
                            oCoverageTypeDepartmentClass.CoverageAmount2 = entityDepartmentClassUpload.CoverageAmount2;
                            oCoverageTypeDepartmentClass.IsCashBackInPercentage2 = entityDepartmentClassUpload.IsCashBackInPercentage2;
                            oCoverageTypeDepartmentClass.CashBackAmount2 = entityDepartmentClassUpload.CashBackAmount2;
                            oCoverageTypeDepartmentClass.IsMarkupInPercentage3 = entityDepartmentClassUpload.IsMarkupInPercentage3;
                            oCoverageTypeDepartmentClass.MarkupAmount3 = entityDepartmentClassUpload.MarkupAmount3;
                            oCoverageTypeDepartmentClass.IsDiscountInPercentage3 = entityDepartmentClassUpload.IsDiscountInPercentage3;
                            oCoverageTypeDepartmentClass.DiscountAmount3 = entityDepartmentClassUpload.DiscountAmount3;
                            oCoverageTypeDepartmentClass.IsDiscount3UsedComp = entityDepartmentClassUpload.IsDiscount3UsedComp;
                            oCoverageTypeDepartmentClass.IsDiscountInPercentage3Comp1 = entityDepartmentClassUpload.IsDiscountInPercentage3Comp1;
                            oCoverageTypeDepartmentClass.DiscountAmount3Comp1 = entityDepartmentClassUpload.DiscountAmount3Comp1;
                            oCoverageTypeDepartmentClass.IsDiscountInPercentage3Comp2 = entityDepartmentClassUpload.IsDiscountInPercentage3Comp2;
                            oCoverageTypeDepartmentClass.DiscountAmount3Comp2 = entityDepartmentClassUpload.DiscountAmount3Comp2;
                            oCoverageTypeDepartmentClass.IsDiscountInPercentage3Comp3 = entityDepartmentClassUpload.IsDiscountInPercentage3Comp3;
                            oCoverageTypeDepartmentClass.DiscountAmount3Comp3 = entityDepartmentClassUpload.DiscountAmount3Comp3;
                            oCoverageTypeDepartmentClass.IsCoverageInPercentage3 = entityDepartmentClassUpload.IsCoverageInPercentage3;
                            oCoverageTypeDepartmentClass.CoverageAmount3 = entityDepartmentClassUpload.CoverageAmount3;
                            oCoverageTypeDepartmentClass.IsCashBackInPercentage3 = entityDepartmentClassUpload.IsCashBackInPercentage3;
                            oCoverageTypeDepartmentClass.CashBackAmount3 = entityDepartmentClassUpload.CashBackAmount3;
                            oCoverageTypeDepartmentClass.IsDeleted = entityDepartmentClassUpload.IsDeleted;
                            oCoverageTypeDepartmentClass.CreatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeDepartmentClassDao.Insert(oCoverageTypeDepartmentClass);

                            entityDepartmentClassUpload.IsDeleted = true;
                            entityDepartmentClassUpload.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeDepartmentClassUploadDao.Update(entityDepartmentClassUpload);
                        }
                        #endregion
                    }
                    else if (coverageType == "4")
                    {
                        #region COVERAGE_TYPE_ITEM

                        string filterCoverageTypeItemUpload = "IsDeleted = 0";
                        List<CoverageTypeItemUpload> lstCoverageTypeUpload = BusinessLayer.GetCoverageTypeItemUploadList(filterCoverageTypeItemUpload, ctx);
                        foreach (CoverageTypeItemUpload entityItemUpload in lstCoverageTypeUpload)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            CoverageTypeItem oCoverageTypeItem = new CoverageTypeItem();
                            CoverageTypeUpload oCoverageTypeUpload = BusinessLayer.GetCoverageTypeUploadList(string.Format("CoverageTypeUploadID = {0}", entityItemUpload.CoverageTypeUploadID))[0];
                            CoverageType oCoverageType = BusinessLayer.GetCoverageTypeList(string.Format("CoverageTypeCode = '{0}'", oCoverageTypeUpload.CoverageTypeUploadCode))[0];

                            oCoverageTypeItem.CoverageTypeID = oCoverageType.CoverageTypeID;
                            oCoverageTypeItem.ItemID = entityItemUpload.ItemID;
                            oCoverageTypeItem.IsMarkupInPercentage = entityItemUpload.IsMarkupInPercentage;
                            oCoverageTypeItem.MarkupAmount = entityItemUpload.MarkupAmount;
                            oCoverageTypeItem.IsDiscountInPercentage = entityItemUpload.IsDiscountInPercentage;
                            oCoverageTypeItem.DiscountAmount = entityItemUpload.DiscountAmount;
                            oCoverageTypeItem.IsDiscountUsedComp = entityItemUpload.IsDiscountUsedComp;
                            oCoverageTypeItem.IsDiscountInPercentageComp1 = entityItemUpload.IsDiscountInPercentageComp1;
                            oCoverageTypeItem.DiscountAmountComp1 = entityItemUpload.DiscountAmountComp1;
                            oCoverageTypeItem.IsDiscountInPercentageComp2 = entityItemUpload.IsDiscountInPercentageComp2;
                            oCoverageTypeItem.DiscountAmountComp2 = entityItemUpload.DiscountAmountComp2;
                            oCoverageTypeItem.IsDiscountInPercentageComp3 = entityItemUpload.IsDiscountInPercentageComp3;
                            oCoverageTypeItem.DiscountAmountComp3 = entityItemUpload.DiscountAmountComp3;
                            oCoverageTypeItem.IsCoverageInPercentage = entityItemUpload.IsCoverageInPercentage;
                            oCoverageTypeItem.CoverageAmount = entityItemUpload.CoverageAmount;
                            oCoverageTypeItem.IsCashBackInPercentage = entityItemUpload.IsCashBackInPercentage;
                            oCoverageTypeItem.CashBackAmount = entityItemUpload.CashBackAmount;
                            oCoverageTypeItem.IsDeleted = entityItemUpload.IsDeleted;
                            oCoverageTypeItem.CreatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeItemDao.Insert(oCoverageTypeItem);

                            entityItemUpload.IsDeleted = true;
                            entityItemUpload.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeItemUploadDao.Update(entityItemUpload);
                        }
                        #endregion
                    }
                    else if (coverageType == "5")
                    {
                        #region COVERAGE_TYPE_ITEM_CLASS

                        string filterCoverageTypeItemClassUpload = "IsDeleted = 0";
                        List<CoverageTypeItemClassUpload> lstCoverageTypeItemClassUpload = BusinessLayer.GetCoverageTypeItemClassUploadList(filterCoverageTypeItemClassUpload, ctx);
                        foreach (CoverageTypeItemClassUpload entityItemClassUpload in lstCoverageTypeItemClassUpload)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            CoverageTypeItemClass oCoverageTypeItemClass = new CoverageTypeItemClass();
                            CoverageTypeUpload oCoverageTypeUpload = BusinessLayer.GetCoverageTypeUploadList(string.Format("CoverageTypeUploadID = {0}", entityItemClassUpload.CoverageTypeUploadID))[0];
                            CoverageType oCoverageType = BusinessLayer.GetCoverageTypeList(string.Format("CoverageTypeCode = '{0}'", oCoverageTypeUpload.CoverageTypeUploadCode))[0];

                            oCoverageTypeItemClass.CoverageTypeID = oCoverageType.CoverageTypeID;
                            oCoverageTypeItemClass.ItemID = entityItemClassUpload.ItemID;
                            oCoverageTypeItemClass.ClassID = entityItemClassUpload.ClassID;
                            oCoverageTypeItemClass.IsMarkupInPercentage = entityItemClassUpload.IsMarkupInPercentage;
                            oCoverageTypeItemClass.MarkupAmount = entityItemClassUpload.MarkupAmount;
                            oCoverageTypeItemClass.IsDiscountInPercentage = entityItemClassUpload.IsDiscountInPercentage;
                            oCoverageTypeItemClass.DiscountAmount = entityItemClassUpload.DiscountAmount;
                            oCoverageTypeItemClass.IsDiscountUsedComp = entityItemClassUpload.IsDiscountUsedComp;
                            oCoverageTypeItemClass.IsDiscountInPercentageComp1 = entityItemClassUpload.IsDiscountInPercentageComp1;
                            oCoverageTypeItemClass.DiscountAmountComp1 = entityItemClassUpload.DiscountAmountComp1;
                            oCoverageTypeItemClass.IsDiscountInPercentageComp2 = entityItemClassUpload.IsDiscountInPercentageComp2;
                            oCoverageTypeItemClass.DiscountAmountComp2 = entityItemClassUpload.DiscountAmountComp2;
                            oCoverageTypeItemClass.IsDiscountInPercentageComp3 = entityItemClassUpload.IsDiscountInPercentageComp3;
                            oCoverageTypeItemClass.DiscountAmountComp3 = entityItemClassUpload.DiscountAmountComp3;
                            oCoverageTypeItemClass.IsCoverageInPercentage = entityItemClassUpload.IsCoverageInPercentage;
                            oCoverageTypeItemClass.CoverageAmount = entityItemClassUpload.CoverageAmount;
                            oCoverageTypeItemClass.IsCashBackInPercentage = entityItemClassUpload.IsCashBackInPercentage;
                            oCoverageTypeItemClass.CashBackAmount = entityItemClassUpload.CashBackAmount;
                            oCoverageTypeItemClass.IsDeleted = entityItemClassUpload.IsDeleted;
                            oCoverageTypeItemClass.CreatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeItemClassDao.Insert(oCoverageTypeItemClass);

                            entityItemClassUpload.IsDeleted = true;
                            entityItemClassUpload.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeItemClassUploadDao.Update(entityItemClassUpload);
                        }
                        #endregion
                    }
                    else if (coverageType == "6")
                    {
                        #region COVERAGE_TYPE_ITEM_GROUP

                        string filterCoverageTypeItemGroupUpload = "IsDeleted = 0";
                        List<CoverageTypeItemGroupUpload> lstCoverageTypeItemGroupUpload = BusinessLayer.GetCoverageTypeItemGroupUploadList(filterCoverageTypeItemGroupUpload, ctx);
                        foreach (CoverageTypeItemGroupUpload entityItemGroupUpload in lstCoverageTypeItemGroupUpload)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            CoverageTypeItemGroup oCoverageTypeItemGroup = new CoverageTypeItemGroup();
                            CoverageTypeUpload oCoverageTypeUpload = BusinessLayer.GetCoverageTypeUploadList(string.Format("CoverageTypeUploadID = {0}", entityItemGroupUpload.CoverageTypeUploadID))[0];
                            CoverageType oCoverageType = BusinessLayer.GetCoverageTypeList(string.Format("CoverageTypeCode = '{0}'", oCoverageTypeUpload.CoverageTypeUploadCode))[0];

                            oCoverageTypeItemGroup.CoverageTypeID = oCoverageType.CoverageTypeID;
                            oCoverageTypeItemGroup.GCItemType = entityItemGroupUpload.GCItemType;
                            oCoverageTypeItemGroup.ItemGroupID = entityItemGroupUpload.ItemGroupID;
                            oCoverageTypeItemGroup.IsMarkupInPercentage = entityItemGroupUpload.IsMarkupInPercentage;
                            oCoverageTypeItemGroup.MarkupAmount = entityItemGroupUpload.MarkupAmount;
                            oCoverageTypeItemGroup.IsDiscountInPercentage = entityItemGroupUpload.IsDiscountInPercentage;
                            oCoverageTypeItemGroup.DiscountAmount = entityItemGroupUpload.DiscountAmount;
                            oCoverageTypeItemGroup.IsDiscountUsedComp = entityItemGroupUpload.IsDiscountUsedComp;
                            oCoverageTypeItemGroup.IsDiscountInPercentageComp1 = entityItemGroupUpload.IsDiscountInPercentageComp1;
                            oCoverageTypeItemGroup.DiscountAmountComp1 = entityItemGroupUpload.DiscountAmountComp1;
                            oCoverageTypeItemGroup.IsDiscountInPercentageComp2 = entityItemGroupUpload.IsDiscountInPercentageComp2;
                            oCoverageTypeItemGroup.DiscountAmountComp2 = entityItemGroupUpload.DiscountAmountComp2;
                            oCoverageTypeItemGroup.IsDiscountInPercentageComp3 = entityItemGroupUpload.IsDiscountInPercentageComp3;
                            oCoverageTypeItemGroup.DiscountAmountComp3 = entityItemGroupUpload.DiscountAmountComp3;
                            oCoverageTypeItemGroup.IsCoverageInPercentage = entityItemGroupUpload.IsCoverageInPercentage;
                            oCoverageTypeItemGroup.CoverageAmount = entityItemGroupUpload.CoverageAmount;
                            oCoverageTypeItemGroup.IsCashBackInPercentage = entityItemGroupUpload.IsCashBackInPercentage;
                            oCoverageTypeItemGroup.CashBackAmount = entityItemGroupUpload.CashBackAmount;
                            oCoverageTypeItemGroup.IsDeleted = entityItemGroupUpload.IsDeleted;
                            oCoverageTypeItemGroup.CreatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeItemGroupDao.Insert(oCoverageTypeItemGroup);

                            entityItemGroupUpload.IsDeleted = true;
                            entityItemGroupUpload.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeItemGroupUploadDao.Update(entityItemGroupUpload);
                        }
                        #endregion
                    }
                    else if (coverageType == "7")
                    {
                        #region COVERAGE_TYPE_ITEM_GROUP_CLASS
                        string filterCoverageTypeItemgroupClassUpload = "IsDeleted= 0";
                        List<CoverageTypeItemGroupClassUpload> lstCoverageTypeItemGroupClassUpload = BusinessLayer.GetCoverageTypeItemGroupClassUploadList(filterCoverageTypeItemgroupClassUpload, ctx);
                        foreach (CoverageTypeItemGroupClassUpload entityItemGroupClassUpload in lstCoverageTypeItemGroupClassUpload)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            CoverageTypeItemGroupClass oCoverageTypeItemGroupClass = new CoverageTypeItemGroupClass();
                            CoverageTypeUpload oCoverageTypeUpload = BusinessLayer.GetCoverageTypeUploadList(string.Format("CoverageTypeUploadID = {0}", entityItemGroupClassUpload.CoverageTypeUploadID))[0];
                            CoverageType oCoverageType = BusinessLayer.GetCoverageTypeList(string.Format("CoverageTypeCode = '{0}'", oCoverageTypeUpload.CoverageTypeUploadCode))[0];

                            oCoverageTypeItemGroupClass.CoverageTypeID = oCoverageType.CoverageTypeID;
                            oCoverageTypeItemGroupClass.GCItemType = entityItemGroupClassUpload.GCItemType;
                            oCoverageTypeItemGroupClass.ItemGroupID = entityItemGroupClassUpload.ItemGroupID;
                            oCoverageTypeItemGroupClass.ClassID = entityItemGroupClassUpload.ClassID;
                            oCoverageTypeItemGroupClass.IsMarkupInPercentage = entityItemGroupClassUpload.IsMarkupInPercentage;
                            oCoverageTypeItemGroupClass.MarkupAmount = entityItemGroupClassUpload.MarkupAmount;
                            oCoverageTypeItemGroupClass.IsDiscountInPercentage = entityItemGroupClassUpload.IsDiscountInPercentage;
                            oCoverageTypeItemGroupClass.DiscountAmount = entityItemGroupClassUpload.DiscountAmount;
                            oCoverageTypeItemGroupClass.IsDiscountUsedComp = entityItemGroupClassUpload.IsDiscountUsedComp;
                            oCoverageTypeItemGroupClass.IsDiscountInPercentageComp1 = entityItemGroupClassUpload.IsDiscountInPercentageComp1;
                            oCoverageTypeItemGroupClass.DiscountAmountComp1 = entityItemGroupClassUpload.DiscountAmountComp1;
                            oCoverageTypeItemGroupClass.IsDiscountInPercentageComp2 = entityItemGroupClassUpload.IsDiscountInPercentageComp2;
                            oCoverageTypeItemGroupClass.DiscountAmountComp2 = entityItemGroupClassUpload.DiscountAmountComp2;
                            oCoverageTypeItemGroupClass.IsDiscountInPercentageComp3 = entityItemGroupClassUpload.IsDiscountInPercentageComp3;
                            oCoverageTypeItemGroupClass.DiscountAmountComp3 = entityItemGroupClassUpload.DiscountAmountComp3;
                            oCoverageTypeItemGroupClass.IsCoverageInPercentage = entityItemGroupClassUpload.IsCoverageInPercentage;
                            oCoverageTypeItemGroupClass.CoverageAmount = entityItemGroupClassUpload.CoverageAmount;
                            oCoverageTypeItemGroupClass.IsCashBackInPercentage = entityItemGroupClassUpload.IsCashBackInPercentage;
                            oCoverageTypeItemGroupClass.CashBackAmount = entityItemGroupClassUpload.CashBackAmount;
                            oCoverageTypeItemGroupClass.IsDeleted = entityItemGroupClassUpload.IsDeleted;
                            oCoverageTypeItemGroupClass.CreatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeItemGroupClassDao.Insert(oCoverageTypeItemGroupClass);

                            entityItemGroupClassUpload.IsDeleted = true;
                            entityItemGroupClassUpload.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeItemGroupClassUploadDao.Update(entityItemGroupClassUpload);
                        }
                        #endregion
                    }
                    else
                    {
                        #region COVERAGE_TYPE_SERVICE_UNIT
                        string filterCoverageTypeServiceUnitUpload = "IsDeleted = 0";
                        List<CoverageTypeServiceUnitUpload> lstCoverageTypeServiceUnitUpload = BusinessLayer.GetCoverageTypeServiceUnitUploadList(filterCoverageTypeServiceUnitUpload, ctx);
                        foreach (CoverageTypeServiceUnitUpload entityServiceUnitUpload in lstCoverageTypeServiceUnitUpload)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            CoverageTypeServiceUnit oCoverageTypeServiceUnit = new CoverageTypeServiceUnit();
                            CoverageTypeUpload oCoverageTypeUpload = BusinessLayer.GetCoverageTypeUploadList(string.Format("CoverageTypeUploadID = {0}", entityServiceUnitUpload.CoverageTypeUploadID))[0];
                            CoverageType oCoverageType = BusinessLayer.GetCoverageTypeList(string.Format("CoverageTypeCode = '{0}'", oCoverageTypeUpload.CoverageTypeUploadCode))[0];

                            oCoverageTypeServiceUnit.CoverageTypeID = oCoverageType.CoverageTypeID;
                            oCoverageTypeServiceUnit.ServiceUnitID = entityServiceUnitUpload.ServiceUnitID;
                            oCoverageTypeServiceUnit.IsMarkupInPercentage1 = entityServiceUnitUpload.IsMarkupInPercentage1;
                            oCoverageTypeServiceUnit.MarkupAmount1 = entityServiceUnitUpload.MarkupAmount1;
                            oCoverageTypeServiceUnit.IsDiscountInPercentage1 = entityServiceUnitUpload.IsDiscountInPercentage1;
                            oCoverageTypeServiceUnit.DiscountAmount1 = entityServiceUnitUpload.DiscountAmount1;
                            oCoverageTypeServiceUnit.IsDiscount1UsedComp = entityServiceUnitUpload.IsDiscount1UsedComp;
                            oCoverageTypeServiceUnit.IsDiscountInPercentage1Comp1 = entityServiceUnitUpload.IsDiscountInPercentage1Comp1;
                            oCoverageTypeServiceUnit.DiscountAmount1Comp1 = entityServiceUnitUpload.DiscountAmount1Comp1;
                            oCoverageTypeServiceUnit.IsDiscountInPercentage1Comp2 = entityServiceUnitUpload.IsDiscountInPercentage1Comp2;
                            oCoverageTypeServiceUnit.DiscountAmount1Comp2 = entityServiceUnitUpload.DiscountAmount1Comp2;
                            oCoverageTypeServiceUnit.IsDiscountInPercentage1Comp3 = entityServiceUnitUpload.IsDiscountInPercentage1Comp3;
                            oCoverageTypeServiceUnit.DiscountAmount1Comp3 = entityServiceUnitUpload.DiscountAmount1Comp3;
                            oCoverageTypeServiceUnit.IsCoverageInPercentage1 = entityServiceUnitUpload.IsCoverageInPercentage1;
                            oCoverageTypeServiceUnit.CoverageAmount1 = entityServiceUnitUpload.CoverageAmount1;
                            oCoverageTypeServiceUnit.IsCashBackInPercentage1 = entityServiceUnitUpload.IsCashBackInPercentage1;
                            oCoverageTypeServiceUnit.CashBackAmount1 = entityServiceUnitUpload.CashBackAmount1;
                            oCoverageTypeServiceUnit.IsMarkupInPercentage2 = entityServiceUnitUpload.IsMarkupInPercentage2;
                            oCoverageTypeServiceUnit.MarkupAmount2 = entityServiceUnitUpload.MarkupAmount2;
                            oCoverageTypeServiceUnit.IsDiscountInPercentage2 = entityServiceUnitUpload.IsDiscountInPercentage2;
                            oCoverageTypeServiceUnit.DiscountAmount2 = entityServiceUnitUpload.DiscountAmount2;
                            oCoverageTypeServiceUnit.IsDiscount2UsedComp = entityServiceUnitUpload.IsDiscount2UsedComp;
                            oCoverageTypeServiceUnit.IsDiscountInPercentage2Comp1 = entityServiceUnitUpload.IsDiscountInPercentage2Comp1;
                            oCoverageTypeServiceUnit.DiscountAmount2Comp1 = entityServiceUnitUpload.DiscountAmount2Comp1;
                            oCoverageTypeServiceUnit.IsDiscountInPercentage2Comp2 = entityServiceUnitUpload.IsDiscountInPercentage2Comp2;
                            oCoverageTypeServiceUnit.DiscountAmount2Comp2 = entityServiceUnitUpload.DiscountAmount2Comp2;
                            oCoverageTypeServiceUnit.IsDiscountInPercentage2Comp3 = entityServiceUnitUpload.IsDiscountInPercentage2Comp3;
                            oCoverageTypeServiceUnit.DiscountAmount2Comp3 = entityServiceUnitUpload.DiscountAmount2Comp3;
                            oCoverageTypeServiceUnit.IsCoverageInPercentage2 = entityServiceUnitUpload.IsCoverageInPercentage2;
                            oCoverageTypeServiceUnit.CoverageAmount2 = entityServiceUnitUpload.CoverageAmount2;
                            oCoverageTypeServiceUnit.IsCashBackInPercentage2 = entityServiceUnitUpload.IsCashBackInPercentage2;
                            oCoverageTypeServiceUnit.CashBackAmount2 = entityServiceUnitUpload.CashBackAmount2;
                            oCoverageTypeServiceUnit.IsMarkupInPercentage3 = entityServiceUnitUpload.IsMarkupInPercentage3;
                            oCoverageTypeServiceUnit.MarkupAmount3 = entityServiceUnitUpload.MarkupAmount3;
                            oCoverageTypeServiceUnit.IsDiscountInPercentage3 = entityServiceUnitUpload.IsDiscountInPercentage3;
                            oCoverageTypeServiceUnit.DiscountAmount3 = entityServiceUnitUpload.DiscountAmount3;
                            oCoverageTypeServiceUnit.IsDiscount3UsedComp = entityServiceUnitUpload.IsDiscount3UsedComp;
                            oCoverageTypeServiceUnit.IsDiscountInPercentage3Comp1 = entityServiceUnitUpload.IsDiscountInPercentage3Comp1;
                            oCoverageTypeServiceUnit.DiscountAmount3Comp1 = entityServiceUnitUpload.DiscountAmount3Comp1;
                            oCoverageTypeServiceUnit.IsDiscountInPercentage3Comp2 = entityServiceUnitUpload.IsDiscountInPercentage3Comp2;
                            oCoverageTypeServiceUnit.DiscountAmount3Comp2 = entityServiceUnitUpload.DiscountAmount3Comp2;
                            oCoverageTypeServiceUnit.IsDiscountInPercentage3Comp3 = entityServiceUnitUpload.IsDiscountInPercentage3Comp3;
                            oCoverageTypeServiceUnit.DiscountAmount3Comp3 = entityServiceUnitUpload.DiscountAmount3Comp3;
                            oCoverageTypeServiceUnit.IsCoverageInPercentage3 = entityServiceUnitUpload.IsCoverageInPercentage3;
                            oCoverageTypeServiceUnit.CoverageAmount3 = entityServiceUnitUpload.CoverageAmount3;
                            oCoverageTypeServiceUnit.IsCashBackInPercentage3 = entityServiceUnitUpload.IsCashBackInPercentage3;
                            oCoverageTypeServiceUnit.CashBackAmount3 = entityServiceUnitUpload.CashBackAmount3;
                            oCoverageTypeServiceUnit.IsDeleted = entityServiceUnitUpload.IsDeleted;
                            oCoverageTypeServiceUnit.CreatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeServiceUnitDao.Insert(oCoverageTypeServiceUnit);

                            entityServiceUnitUpload.IsDeleted = true;
                            entityServiceUnitUpload.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityCoverageTypeServiceUnitUploadDao.Update(entityServiceUnitUpload);
                        }
                        #endregion
                    }
                    #endregion
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                //errMessage = "Save Gagal, silahkan periksa data excel";
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;

        }
    }
}