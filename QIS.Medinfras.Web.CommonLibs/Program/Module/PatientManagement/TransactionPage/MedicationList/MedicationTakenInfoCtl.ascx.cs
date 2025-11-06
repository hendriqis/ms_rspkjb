using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicationTakenInfoDetailCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            //var orderDetailID|itemID|itemName|paramedicName;
            string[] paramInfo = param.Split('|');
            hdnPrescriptionOrderDetailID.Value = paramInfo[0];
            hdnPastMedicationID.Value = paramInfo[1];
            hdnItemID.Value = paramInfo[2];
            hdnItemName.Value = paramInfo[3];
            hdnParamedicName.Value = paramInfo[4];

            txtItemName.Text = hdnItemName.Value;
            txtParamedicName.Text = hdnParamedicName.Value;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;

            if (hdnPastMedicationID.Value != null && hdnPastMedicationID.Value != "" && hdnPastMedicationID.Value != "0")
            {
                filterExpression += string.Format(" PastMedicationID = {0} AND GCMedicationStatus = '{1}' AND IsDeleted = 0", hdnPastMedicationID.Value, Constant.MedicationStatus.TELAH_DIBERIKAN);
            }
            else
            {
                filterExpression += string.Format(" PrescriptionOrderDetailID = {0} AND GCMedicationStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderDetailID.Value, Constant.MedicationStatus.TELAH_DIBERIKAN);
            }
            
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMedicationScheduleRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_ITEM);
            }

            List<vMedicationSchedule> lstTransaction = BusinessLayer.GetvMedicationScheduleList(filterExpression, Constant.GridViewPageSize.GRID_ITEM, pageIndex, "MedicationDate, MedicationTime");
            grdPopupView.DataSource = lstTransaction;
            grdPopupView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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