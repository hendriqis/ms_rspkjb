using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class RecipeHistory : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.RECIPE_HISTORY;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected string OnGetLocationFilterExpression()
        {
            return string.Format("{0};{1};;", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
        }

        protected override void InitializeDataControl()
        {

            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());


            hdnID.Value = Page.Request.QueryString["id"];
            

            txtDateFrom.Text = DateTime.Now.AddDays(-7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

          
         
        
            BindGridView(CurrPage, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";

            if (!chkIsIgnoreDate.Checked)
            {
                filterExpression += string.Format(" PrescriptionDate BETWEEN '{0}' AND '{1}' AND GCTransactionStatus NOT IN ('{2}','{3}')", Helper.GetDatePickerValue(txtDateFrom).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtDateTo).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.OPEN, Constant.TransactionStatus.VOID);
            }
            else if (chkIsIgnoreDate.Checked)
            {
                
                filterExpression += string.Format(" GCTransactionStatus NOT IN ('{0}','{1}')", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.VOID);
            }

            if (hdnFilterExpressionQuickSearch.Value == "Search")
                hdnFilterExpressionQuickSearch.Value = " ";
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPrescriptionOrderHd2RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_MR_LIST);
            }

            List<vPrescriptionOrderHd2> lstPrescriptionOrderHD = BusinessLayer.GetvPrescriptionOrderHd2List(filterExpression,Constant.GridViewPageSize.GRID_PATIENT_MR_LIST,pageIndex);
            grdView.DataSource = lstPrescriptionOrderHD;
            grdView.DataBind();
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
                else 
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnParamID.Value.ToString() != "")
            {
                Patient entity = BusinessLayer.GetPatient(Convert.ToInt32(hdnID.Value));
                PatientDetail pt = new PatientDetail();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
                AppSession.PatientDetail = pt;

                Response.Redirect("~/Libs/Program/Module/MedicalRecord/PatientEMRView.aspx");
            }
        }
    }
}