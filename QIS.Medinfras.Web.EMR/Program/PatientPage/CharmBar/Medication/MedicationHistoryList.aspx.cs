using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class MedicationHistoryList : BasePage
    {
        protected int PageCount = 1;

        protected class MedicationHistory
        {
            public Int32 ItemID { get; set; }
            public string DrugName { get; set; }
            public decimal Dose { get; set; }
            public string DoseUnit { get; set; }
        }

        List<MedicationHistory> lstHeader = new List<MedicationHistory>();
        List<vPatientMedicationHistory> lstDetail = new List<vPatientMedicationHistory>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //InitializeDataSource();
            //BindGridView(1, true, ref PageCount);
        }

        private void InitializeDataSource()
        {
            //string filterExpression = string.Format("MedicalNo = {0} AND GCTransactionStatus = 0", AppSession.RegisteredPatient.MedicalNo, Constant.TransactionStatus.PROCESSED);
            //lstDetail = BusinessLayer.GetvPatientMedicationHistoryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, 1, "DrugName DESC");
            //IEnumerable<int> lstItem = (from p in lstDetail
            //                            select p.ItemID).Distinct();

            //foreach (int itemID in lstItem)
            //{
            //    vPatientMedicationHistory item = lstDetail.Select(lst => lst.ItemID.Equals(itemID)).FirstOrDefault();
            //}
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("MedicalNo = {0} AND GCTransactionStatus = 0", AppSession.RegisteredPatient.MedicalNo, Constant.TransactionStatus.PROCESSED);

            //string code = ddlViewType.SelectedValue;
            //if (code == "1")
            //    filterExpression += string.Format(" AND (EndDate IS NULL OR EndDate >= '{0}')", DateTime.Now.ToString("yyyyMMdd"));
            //else if (code == "2")
            //    filterExpression += string.Format(" AND EndDate < '{0}'", DateTime.Now.ToString("yyyyMMdd"));

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientMedicationHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientMedicationHistory> lstEntity = BusinessLayer.GetvPatientMedicationHistoryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "DrugName");
            grdView.DataSource = lstEntity;
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
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}