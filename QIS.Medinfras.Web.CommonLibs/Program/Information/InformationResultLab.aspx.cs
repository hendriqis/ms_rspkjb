using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxPivotGrid;
using QIS.Medinfras.Web.Common;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;


namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InformationResultLab : BasePageTrx
    {
        protected int PageCount = 1;
        
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.INFORMATION_RESULT_LABORATORIUM;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            List<StandardCode> lstVariable = new List<StandardCode>();
            lstVariable.Insert(0, new StandardCode { StandardCodeName = "Semua", StandardCodeID = "0" });
            lstVariable.Insert(1, new StandardCode { StandardCodeName = "Belum Ada Hasil", StandardCodeID = "1" });
            lstVariable.Insert(2, new StandardCode { StandardCodeName = "Sudah Ada Hasil", StandardCodeID = "2" });
            Methods.SetComboBoxField<StandardCode>(cboResultType, lstVariable, "StandardCodeName", "StandardCodeID");
            cboResultType.SelectedIndex = 0;

            txtDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(1, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref PageCount);
                    result = "refresh|" + PageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public string GetInformationLabResultFilterExpression()
        {
            return Request.Form[hdnFilterExpression.UniqueID];
        }
        
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string fromDate = Helper.GetDatePickerValue(txtDateFrom).ToString("yyyyMMdd");
            string toDate = Helper.GetDatePickerValue(txtDateTo).ToString("yyyyMMdd");

            String filterExpression = String.Format("TransactionCode = '{0}'  AND CONVERT(date, TransactionDate) BETWEEN '{1}' AND '{2}'", Constant.TransactionCode.LABORATORY_CHARGES, fromDate, toDate);

            if (cboResultType.Value.ToString() == "1")
                filterExpression += " AND TransactionID NOT IN (SELECT ChargeTransactionID FROM LaboratoryResultHd WHERE IsDeleted = 0)";
            else if (cboResultType.Value.ToString() == "2")
                filterExpression += " AND TransactionID IN (SELECT ChargeTransactionID FROM LaboratoryResultHd WHERE IsDeleted = 0)";

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += String.Format(" AND {0} ", hdnFilterExpressionQuickSearch.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesHd7RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            List<vPatientChargesHd7> lstEntity = BusinessLayer.GetvPatientChargesHd7List(filterExpression, 10, pageIndex, "TransactionID");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}