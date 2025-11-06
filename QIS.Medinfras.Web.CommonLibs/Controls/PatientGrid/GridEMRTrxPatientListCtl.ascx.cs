using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GridEMRTrxPatientListCtl : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        public void InitializeControl()
        {
            BindGridView(1, true, ref PageCount);
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ((BasePageRegisteredPatient)Page).LoadAllWords();
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

        //List<LaboratoryResultHd> lstLaboratoryResultHd = null;
        //List<ImagingResultHd> lstImagingResultHd = null;
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ((BasePageRegisteredPatient)Page).GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesHdVisit2RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vPatientChargesHdVisit2> lstEntity = BusinessLayer.GetvPatientChargesHdVisit2List(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "RegistrationNo");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();


            //StringBuilder lstChargeTransactionID = new StringBuilder();
            //foreach (vPatientChargesHdVisit2 entity in lstEntity)
            //{
            //    if (lstChargeTransactionID.ToString() != "")
            //        lstChargeTransactionID.Append(",");
            //    lstChargeTransactionID.Append(entity.TransactionID.ToString());
            //}

            //if (lstChargeTransactionID.ToString() != "")
            //    filterExpression = string.Format("ChargeTransactionID IN ({0}) AND IsDeleted = 0", lstChargeTransactionID.ToString());
            //else
            //    filterExpression = "1 = 0";

            //if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
            //    lstLaboratoryResultHd = BusinessLayer.GetLaboratoryResultHdList(filterExpression);
            //else
            //    lstImagingResultHd = BusinessLayer.GetImagingResultHdList(filterExpression);

            //RowCountTable rc = new RowCountTable()
            //{
            //    TotalRow = lvwView.Items.Count
            //};

            //List<RowCountTable> rcList = new List<RowCountTable>();
            //rcList.Add(rc);

            //lvwViewCount.DataSource = rcList;
            //lvwViewCount.DataBind();

        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            //if (e.Item.ItemType == ListViewItemType.DataItem)
            //{
            //    vPatientChargesHdVisit2 entity = (vPatientChargesHdVisit2)e.Item.DataItem;
            //    HtmlGenericControl spnProcessed = e.Item.FindControl("spnProcessed") as HtmlGenericControl;

            //    if (lstImagingResultHd.FirstOrDefault(p => p.ChargeTransactionID == entity.TransactionID) == null)
            //        spnProcessed.Style.Add("display", "none");
            //}
        }

        protected string GetLabel(string code)
        {
            return ((BasePageRegisteredPatient)Page).GetLabel(code);
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnTransactionNo.Value != "")
            {
                ((BasePageRegisteredPatient)Page).OnGrdRowClick(hdnTransactionNo.Value);
            }
        }

        private class RowCountTable
        {
            private Int32 totalRow;

            public Int32 TotalRow
            {
                get { return totalRow; }
                set { totalRow = value; }
            }            
        }
    }
}