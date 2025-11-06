using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Text;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class DownloadTarifBookEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnBookID.Value = param;
            hdnFileName.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_FILE_NAME_DOCUMENT_UPLOAD_TARIFF).ParameterValue;

            //hdnCoverageTypeIDCtlDept.Value = param;
            //CoverageType entity = BusinessLayer.GetCoverageType(Convert.ToInt32(hdnCoverageTypeIDCtlDept.Value));
            //txtCoverageType.Text = string.Format("{0} - {1}", entity.CoverageTypeCode, entity.CoverageTypeName);

            //BindGridView();

            //txtDepartmentCode.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            //lvwView.DataSource = BusinessLayer.GetvCoverageTypeDepartmentList(string.Format("CoverageTypeID = {0} AND IsDeleted = 0 ORDER BY DepartmentName ASC", hdnCoverageTypeIDCtlDept.Value));
            //lvwView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    for (int i = 0; i < e.Row.Cells.Count; i++)
            //        e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            //}            
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "download")
            {
                if (OnDownloadRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }


        private bool OnDownloadRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                #region Download Document

                StringBuilder sbResult = new StringBuilder();

                List<dynamic> lstDynamic = null;
                List<Variable> lstVariable = new List<Variable>();


                string oBookID = "0";
                if (hdnBookID.Value != null && hdnBookID.Value != "")
                {
                    oBookID = hdnBookID.Value;
                }

                lstVariable.Add(new Variable { Code = "BookID", Value = oBookID });
                lstVariable.Add(new Variable { Code = "UserID", Value = AppSession.UserLogin.UserID.ToString() });
                lstVariable.Add(new Variable { Code = "TypeItem", Value = rblCheckAll.SelectedValue });
                lstDynamic = BusinessLayer.GetDataReport("GetTariffBookDtForUpload1", lstVariable);

                dynamic fields = lstDynamic[0];

                foreach (var prop in fields.GetType().GetProperties())
                {
                    if (prop.Name == "ProposedTariffComp1")
                    {
                        sbResult.Append("Tarif1");
                    }
                    else if (prop.Name == "ProposedTariffComp2")
                    {
                        sbResult.Append("Tarif2");
                    }
                    else if (prop.Name == "ProposedTariffComp3")
                    {
                        sbResult.Append("Tarif3");
                    }
                    else
                    {
                        sbResult.Append(prop.Name);
                    }

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
                hdnStringCSV.Value = sbResult.ToString();
                #endregion

            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }


        //private bool OnDownloadRecord(ref string errMessage)
        //{

        //    bool result = true;
        //    IDbContext ctx = DbFactory.Configure(true);
        //    TariffBookDtTempDao bookDtTempDao = new TariffBookDtTempDao(ctx);

        //    try
        //    {

        //        string fileName = "TemplateItemMasterTarif.xlsx";
        //        string PathFile = string.Format("{0}\\ItemTarifMaster\\", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISFinanceUploadDocument);
        //        string MasterFile = string.Format("{0}{1}", PathFile, fileName);

        //        if (!Directory.Exists(PathFile))
        //        {
        //            Directory.CreateDirectory(PathFile);
        //        }

        //        string FileNameTmp = string.Format("{0}.xlsx", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
        //        string TempFile = string.Format("{0}{1}", PathFile, FileNameTmp);

        //        File.Copy(MasterFile, TempFile);

        //        InsertExcelRow(TempFile);
        //        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        //        response.ClearContent();
        //        response.Clear();
        //        response.ContentType = "Application/x-msexcel";
        //        response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", FileNameTmp));
        //        response.TransmitFile(string.Format("{0}\\ItemTarifMaster\\{1}", AppConfigManager.QISPhysicalDirectory, FileNameTmp));
        //        response.Flush();
        //        response.End();

        //        File.Delete(string.Format("{0}\\ItemTarifMaster\\{1}", AppConfigManager.QISPhysicalDirectory, FileNameTmp));



        //    }
        //    catch (Exception ex)
        //    {
        //        result = false;
        //        errMessage = ex.Message;
        //        Helper.InsertErrorLog(ex);
        //        ctx.RollBackTransaction();
        //    }
        //    finally
        //    {
        //        ctx.Close();
        //    }

        //    return result;

        //}
        protected void btnSample_Click(object sender, EventArgs e)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TariffBookDtTempDao bookDtTempDao = new TariffBookDtTempDao(ctx);

            try
            {

                string fileName = "TemplateItemMasterTarif.xlsx";
                string PathFile = string.Format("{0}\\ItemTarifMaster\\", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISFinanceUploadDocument);
                string MasterFile = string.Format("{0}{1}", PathFile, fileName);

                if (!Directory.Exists(PathFile))
                {
                    Directory.CreateDirectory(PathFile);
                }

                string FileNameTmp = string.Format("{0}.xlsx", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                string TempFile = string.Format("{0}{1}", PathFile, FileNameTmp);

                File.Copy(MasterFile, TempFile);

                InsertExcelRow(TempFile);
                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.ClearContent();
                response.Clear();
                response.ContentType = "Application/x-msexcel";
                response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", FileNameTmp));
                response.TransmitFile(string.Format("{0}\\ItemTarifMaster\\{1}", AppConfigManager.QISPhysicalDirectory, FileNameTmp));
                response.Flush();
                //response.End();

                File.Delete(string.Format("{0}\\ItemTarifMaster\\{1}", AppConfigManager.QISPhysicalDirectory, FileNameTmp));

                //Response.Redirect(Request.RawUrl, false);

            }
            catch (Exception ex)
            {
                result = false;
                // errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }



        }
        //protected void btnDownload_Click(object sender, EventArgs e)
        //{

        //    bool result = true;
        //    String  errMessage = string.Empty;
        //    IDbContext ctx = DbFactory.Configure(true);
        //    TariffBookDtTempDao bookDtTempDao = new TariffBookDtTempDao(ctx);

        //    try
        //    {

        //                string fileName = "TemplateItemMasterTarif.xlsx";
        //                string PathFile = string.Format("{0}\\{1}\\", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISFinanceUploadDocument);
        //                string MasterFile = string.Format("{0}{1}", PathFile, fileName);

        //                if (!Directory.Exists(PathFile)) {
        //                    Directory.CreateDirectory(PathFile);
        //                }

        //                string FileNameTmp = string.Format("{0}.xlsx", DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT_2));
        //                string TempFile = string.Format("{0}{1}", PathFile, FileNameTmp);

        //                File.Copy(MasterFile, TempFile);

        //                InsertExcelRow(TempFile);
        //                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        //                response.ClearContent();
        //                response.Clear();
        //                response.ContentType = "Application/x-msexcel";
        //                response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", FileNameTmp));
        //                response.TransmitFile(string.Format("{0}\\Files\\{1}", AppConfigManager.QISPhysicalDirectory, FileNameTmp));
        //                response.Flush();
        //                response.End();

        //                File.Delete(string.Format("{0}\\Files\\{1}", AppConfigManager.QISPhysicalDirectory, FileNameTmp));



        //    }
        //    catch (Exception ex)
        //    {
        //        result = false;
        //        errMessage = ex.Message;
        //        Helper.InsertErrorLog(ex);
        //        ctx.RollBackTransaction();
        //    }
        //    finally
        //    {
        //        ctx.Close();
        //    }


        //}

        private Boolean InsertExcelRow(String strFilePath)
        {
            if (!File.Exists(strFilePath)) return false;
            String strExcelConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilePath + ";Mode=ReadWrite;Extended Properties=\"Excel 12.0;HDR=Yes\"";

            OleDbConnection connExcel = new OleDbConnection(strExcelConn);
            OleDbCommand cmdExcel = new OleDbCommand();
            try
            {
                cmdExcel.Connection = connExcel;

                //Check if the Sheet Exists
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                connExcel.Close();
                DataRow[] dr = dtExcelSchema.Select("TABLE_NAME = 'ItemMasterTarif'");

                //if not Create the Sheet
                if (dr == null || dr.Length == 0)
                {
                    cmdExcel.CommandText = string.Format("CREATE TABLE [ItemMasterTarif]" +
                        "(GCItemType varchar(15), ItemType varchar(15),ItemID numeric, ItemCode varchar(15), ItemName1 varchar(100)," +
                        "ClassID numeric,ClassCode varchar(100), ClassName varchar(100),ProposedTariffComp1 numeric, ProposedTariffComp2 numeric," +
                        "ProposedTariffComp3 numeric)");


                    connExcel.Open();
                    cmdExcel.ExecuteNonQuery();
                    connExcel.Close();
                }

                #region ADD NEW ROW TO EXCEL FILE
                List<GetTariffBookDtForUpload> lstData = BusinessLayer.GetTariffBookDtForUpload(Convert.ToInt16(hdnBookID.Value), AppSession.UserLogin.UserID);

                List<GetTariffBookDtForUpload> lstDataDT = new List<GetTariffBookDtForUpload>();
                if (rblCheckAll.SelectedValue == "0")
                {
                    lstDataDT = lstData.Where(p =>
                           p.GCItemType == Constant.ItemType.OBAT_OBATAN ||
                           p.GCItemType == Constant.ItemType.BARANG_MEDIS ||
                           p.GCItemType == Constant.ItemType.BARANG_UMUM ||
                           p.GCItemType == Constant.ItemType.BAHAN_MAKANAN
                       ).ToList();
                }
                else
                {
                    lstDataDT = lstData.Where(p =>
                            p.GCItemType != Constant.ItemType.OBAT_OBATAN ||
                            p.GCItemType != Constant.ItemType.BARANG_MEDIS ||
                            p.GCItemType != Constant.ItemType.BARANG_UMUM ||
                            p.GCItemType != Constant.ItemType.BAHAN_MAKANAN
                        ).ToList();
                }


                if (lstDataDT.Count > 0)
                {
                    foreach (GetTariffBookDtForUpload row in lstDataDT)
                    {

                        string GCItemType = row.GCItemType;
                        string ItemType = row.ItemType;
                        int ItemID = row.ItemID;
                        string ItemCode = row.ItemCode;
                        string ItemName1 = row.ItemName1;
                        int ClassID = row.ClassID;
                        string ClassCode = row.ClassCode;
                        string ClassName = row.ClassName;

                        Decimal ProposedTariffComp1 = row.ProposedTariffComp1;
                        Decimal ProposedTariffComp2 = row.ProposedTariffComp2;
                        Decimal ProposedTariffComp3 = row.ProposedTariffComp3;

                        connExcel.Open();
                        cmdExcel.CommandText = string.Format("INSERT INTO [ItemMasterTarif] ( GCItemType, ItemType, ItemID, ItemCode,ItemName1, ClassID, ClassCode, ClassName,ProposedTariffComp1,ProposedTariffComp2,ProposedTariffComp3) VALUES('{0}', '{1}', '{2}', '{3}','{4}', '{5}', '{6}','{7}','{8}','{9}', '{10}')",
                            GCItemType,
                            ItemType,
                            ItemID,
                            ItemCode,
                            ItemName1,
                            ClassID,
                            ClassCode,
                            ClassName,
                            ProposedTariffComp1,
                            ProposedTariffComp2,
                            ProposedTariffComp3
                            );
                        cmdExcel.ExecuteNonQuery();
                        connExcel.Close();
                    }
                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {

                string errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;


            }
            finally
            {
                cmdExcel.Dispose();
                connExcel.Dispose();
            }
        }

    }
}