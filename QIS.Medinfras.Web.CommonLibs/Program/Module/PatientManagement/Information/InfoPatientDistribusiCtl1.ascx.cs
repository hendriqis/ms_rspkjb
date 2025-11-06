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
    public partial class InfoPatientDistribusiCtl1 : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(String ItemID)
        {
            //string[] param = queryString.Split('|');
            hdnParam.Value = ItemID;

            vItemDistributionDt ph = BusinessLayer.GetvItemDistributionDtList(string.Format("ItemID = {0}", ItemID)).FirstOrDefault();
            //txtPaymentNo.Text = string.Format("{0}",ph.DistributionID);
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
            string filterExpression = string.Format("RegistrationID = {0} AND GCDistributionStatus != 'X160^999'", AppSession.RegisteredPatient.RegistrationID);
            filterExpression += string.Format(" AND ItemID = {0}", hdnParam.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemDistributionDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vItemDistributionDt> lstEntity = BusinessLayer.GetvItemDistributionDtList(filterExpression, 8, pageIndex, "ID ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

    }
}