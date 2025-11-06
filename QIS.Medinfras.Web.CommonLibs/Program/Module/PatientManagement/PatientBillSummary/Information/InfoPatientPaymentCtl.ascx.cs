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
    public partial class InfoPatientPaymentCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(String PaymentID)
        {
            //string[] param = queryString.Split('|');
            hdnParam.Value = PaymentID;

            PatientPaymentHd ph = BusinessLayer.GetPatientPaymentHd(Convert.ToInt32(PaymentID));
            txtPaymentNo.Text = string.Format("{0}",ph.PaymentNo);

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
            string filterExpression = string.Format("PaymentID = {0} AND IsDeleted = 0", hdnParam.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientPaymentDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vPatientPaymentDt> lstEntity = BusinessLayer.GetvPatientPaymentDtList(filterExpression, 8, pageIndex, "PaymentDetailID ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

    }
}