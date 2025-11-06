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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoPatientChargesCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(String ItemID)
        {
            //string[] param = queryString.Split('|');
            hdnParam.Value = ItemID;

            vPatientChargesDt ph = BusinessLayer.GetvPatientChargesDtList(string.Format("ItemID= {0}", ItemID)).FirstOrDefault();
           

            BindGridView(1, true, ref PageCount);
        }
        
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";

            if (e.Parameter != null && e.Parameter != "")
            {
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus != 'X121^999'", AppSession.RegisteredPatient.RegistrationID);
            filterExpression += string.Format(" AND ItemID = {0}", hdnParam.Value);
            filterExpression += string.Format(" AND PrescriptionOrderID IS NULL");
            filterExpression += string.Format(" AND PrescriptionReturnOrderID IS NULL");
            filterExpression += string.Format(" AND GCItemType IN ('X001^002', 'X001^003')");

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vPatientChargesDt> lstEntity = BusinessLayer.GetvPatientChargesDtList(filterExpression, 8, pageIndex, "ItemID ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

    }
}