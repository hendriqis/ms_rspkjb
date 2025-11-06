using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class RecalculatePurchaseReceive : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.RECALCULATE_PURCHASE_RECEIVE;
        }
        protected int PageCount = 1;
        private GetUserMenuAccess menu;
        public override bool IsShowRightPanel()
        {
            return false;
        }
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }
        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                BindGridView(1, true, ref PageCount);
            }
        }

        protected string GetFilterExpression()
        {
            string filterExpression = string.Format(" PurchaseInvoiceID IS NULL");
            return filterExpression;
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (hdnPurchaseReceiveID.Value != null && hdnPurchaseReceiveID.Value != "")
            {
                filterExpression += string.Format(" AND PurchaseReceiveID = {0}", hdnPurchaseReceiveID.Value); 
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseReceiveHdRecalculateRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseReceiveHdRecalculate> lstEntity = BusinessLayer.GetvPurchaseReceiveHdRecalculateList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    vConsultVisit entity = e.Row.DataItem as vConsultVisit;
            //    PatientDiagnosis diagnosis = lstPatientDiagnosis.FirstOrDefault(p => p.VisitID == entity.VisitID);
            //    if (diagnosis != null)
            //    {
            //        HtmlInputText txtDiagnose = e.Row.FindControl("txtDiagnose") as HtmlInputText;
            //        txtDiagnose.Value = diagnosis.DiagnosisText;
            //    }
            //}
        }

        //protected void cbpSaveDiagnose_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        //{
        //    string result = "";
        //    try
        //    {
        //        //string[] param = e.Parameter.Split('|');
        //        //int visitID = Convert.ToInt32(param[0]);
        //        //String diagnoseText = param[1];
        //        //int paramedicID = Convert.ToInt32(param[2]);

        //        //PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", visitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();
        //        //bool IsAdd = false;
        //        //if (entity == null)
        //        //{
        //        //    entity = new PatientDiagnosis();
        //        //    IsAdd = true;
        //        //    entity.VisitID = visitID;
        //        //    entity.ParamedicID = paramedicID;
        //        //    entity.DifferentialDate = DateTime.Now;
        //        //    entity.DifferentialTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        //        //    entity.GCDiagnoseType = Constant.DiagnoseType.MAIN_DIAGNOSIS;
        //        //    entity.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
        //        //}
        //        //entity.DiagnosisText = diagnoseText;
        //        //if (IsAdd)
        //        //{
        //        //    entity.CreatedBy = AppSession.UserLogin.UserID;
        //        //    BusinessLayer.InsertPatientDiagnosis(entity);
        //        //}
        //        //else
        //        //{
        //        //    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
        //        //    BusinessLayer.UpdatePatientDiagnosis(entity);
        //        //}
        //        result = "success";
        //    }
        //    catch (Exception ex)
        //    {
        //        result = string.Format("fail|{0}", ex.Message);
        //    }
        //    ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
        //    panel.JSProperties["cpResult"] = result;
        //}
    }
}