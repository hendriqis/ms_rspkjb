using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class HcLabLIS : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            switch (hdnModuleID.Value)
            {
                case Constant.Module.SYSTEM_SETUP:
                    return Constant.MenuCode.SystemSetup.LIS_HCLAB;
                case Constant.Module.LABORATORY:
                    return Constant.MenuCode.Laboratory.LB_LIS_HCLAB;
                default:
                    return Constant.MenuCode.SystemSetup.LIS_HCLAB;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetRefreshGridInterval()
        {
            return AppSession.RefreshGridInterval;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnModuleID.Value = Page.Request.QueryString["id"];
            }

            txtFromDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Variable> lstVariableCoverage = new List<Variable>();
            lstVariableCoverage.Add(new Variable { Code = "0", Value = "Semua" });
            lstVariableCoverage.Add(new Variable { Code = "1", Value = "Belum Dibayar" });
            lstVariableCoverage.Add(new Variable { Code = "2", Value = "Sudah Dibayar" });
            Methods.SetComboBoxField<Variable>(cboCoverage, lstVariableCoverage, "Value", "Code");
            cboCoverage.Value = "0";

            BindGridView(CurrPage, true, ref PageCount);

            hdnIsBridgingToPGxTest.Value = AppSession.SA0119;
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string fromDate = Helper.GetDatePickerValue(txtFromDate).ToString("yyyyMMdd");
            string toDate = Helper.GetDatePickerValue(txtToDate).ToString("yyyyMMdd");
            String filterExpression = String.Format("(CONVERT(DATE, TransactionDate) BETWEEN '{0}' AND '{1}')", fromDate, toDate);

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += String.Format(" AND {0} ", hdnFilterExpressionQuickSearch.Value);
            }

            if (cboCoverage.Value.ToString() == "1")
                filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}')", Constant.TransactionStatus.CLOSED);
            else if (cboCoverage.Value.ToString() == "2")
                filterExpression += string.Format(" AND GCTransactionStatus = '{0}'", Constant.TransactionStatus.CLOSED);


            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvLaboratoryTransHdLogForHCLabRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vLaboratoryTransHdLogForHCLab> lstEntity = BusinessLayer.GetvLaboratoryTransHdLogForHCLabList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TransactionID");
            grdView.DataSource = lstEntity;
            grdView.DataBind();

            if (hdnModuleID.Value != Constant.Module.LABORATORY)
            {
                ((DataControlField)grdView.Columns
                   .Cast<DataControlField>()
                   .Where(fld => (fld.HeaderText == "Send To LIS"))
                   .SingleOrDefault()).Visible = false;
            }
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vLaboratoryTransHdLogForHCLab entity = e.Row.DataItem as vLaboratoryTransHdLogForHCLab;
                Label lbl = e.Row.FindControl("lblStatusKirim") as Label;
                Label lbl1 = e.Row.FindControl("lblStatusHasil") as Label;
                Label lblBridgingStatus = e.Row.FindControl("lblBridgingStatus") as Label;

                BridgingStatus bd = new BridgingStatus();
                if (entity.IsSendToLIS)
                {
                    string filter = string.Format("TransactionID = {0}", entity.TransactionID);
                    bd = BusinessLayer.GetBridgingStatusList(filter).LastOrDefault();
                    lbl.Text = string.Format("{0}", bd.SendDateTime.ToString(Constant.FormatString.DATE_TIME_FORMAT_2));
                    lblBridgingStatus.Text = "SENT";
                }
                else
                {
                    lblBridgingStatus.Text = "OPEN";
                }

                if (entity.IsResultExist)
                {
                    if (bd != null)
                    {
                        if (bd.ResultDateTime.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                        {
                            lbl1.Text = string.Format("{0}", bd.ResultDateTime.ToString(Constant.FormatString.DATE_TIME_FORMAT_2));
                        }
                    }
                    else
                    {
                        string filter = string.Format("TransactionID = {0}", entity.TransactionID);
                        bd = BusinessLayer.GetBridgingStatusList(filter).LastOrDefault();
                        if (bd.ResultDateTime.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                        {
                            lbl1.Text = string.Format("{0}", bd.ResultDateTime.ToString(Constant.FormatString.DATE_TIME_FORMAT_2));
                        }
                    }
                }
            }
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

        protected void cbpAPICall_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string cpBridging = "";
            string[] paramInfo = e.Parameter.Split('|');
            if (paramInfo[0] == "sendToLIS")
            {
                cpBridging = paramInfo[0];
                IDbContext ctx = DbFactory.Configure(true);
                try
                {
                    int transactionID = Convert.ToInt32(paramInfo[1]);
                    switch (AppSession.LIS_PROVIDER)
                    {
                        case Constant.LIS_PROVIDER.HCLAB:
                            BusinessLayer.SendToLISInterfaceDB(transactionID, ctx);
                            break;
                    }
                    ctx.CommitTransaction();

                    result = "success";
                }
                catch (Exception ex)
                {
                    ctx.RollBackTransaction();
                    errMessage = ex.Message;
                    result = "fail";
                }
                finally
                {
                    ctx.Close();
                }
            }
            else
            {
                cpBridging = "Nalagenetics";
                try
                {
                    NalageneticsService oService = new NalageneticsService();

                    string mrn = paramInfo[0];
                    string transactionID = paramInfo[1];
                    string sampleID = paramInfo[2];

                    LaboratoryResultHd oResultHd = BusinessLayer.GetLaboratoryResultHdList(string.Format("ChargeTransactionID = {0}", transactionID)).FirstOrDefault();
                    if (oResultHd != null)
                    {
                        string apiResult = oService.GetReportData(mrn, transactionID, sampleID);
                        if (!string.IsNullOrEmpty(apiResult))
                        {
                            string[] apiResultInfo = apiResult.Split('|');
                            if (apiResultInfo[0] == "1")
                            {
                                result += string.Format("success|{0}", apiResultInfo[1]);
                            }
                            else
                            {
                                errMessage = apiResultInfo[2];
                                result += string.Format("fail|{0}", errMessage);
                            }
                        }
                        else
                        {
                            errMessage = "Proses Penarikan Data Pharmacogenomics gagal dilakukan. (Response Kosong)";
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                    else
                    {
                        errMessage = "Invalid Transaction ID.";
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result += string.Format("fail|{0}", errMessage);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpBridging"] = cpBridging;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = hdnMRN.Value;
        }
    }
}