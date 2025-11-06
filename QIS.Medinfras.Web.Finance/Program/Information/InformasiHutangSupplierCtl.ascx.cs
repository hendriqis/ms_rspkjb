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
using QIS.Medinfras.Web.Finance.Program;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InformasiHutangSupplierCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        private InformasiHutangSupplier DetailPage
        {
            get { return (InformasiHutangSupplier)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            String[] lstParam = param.Split('|');
            hdnBusinessPartnerID.Value = lstParam[0];

            BusinessPartners im = BusinessLayer.GetBusinessPartners(Convert.ToInt32(hdnBusinessPartnerID.Value));
            txtItemName.Text = string.Format("{0} - {1}", im.BusinessPartnerCode, im.BusinessPartnerName);

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<string> lst = DetailPage.GetMovementDate().Split('|').ToList();
            string filterExpression = String.Format("MovementDate BETWEEN '{0}' AND '{1}' AND BusinessPartnerID = {2}",lst[0],lst[1],hdnBusinessPartnerID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvAPMovementRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            List<vAPMovement> lstDistributionDt = BusinessLayer.GetvAPMovementList(filterExpression, 10, pageIndex);
            grdPopupView.DataSource = lstDistributionDt;
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