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
using System.Text;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InformasiHutangSupplier : BasePageList
    {
        protected int PageCount = 1;
        protected string filterExpressionSupplier = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.INFORMASI_UMUR_PIUTANG;
        }

        public String GetMovementDate() 
        {

            return Request.Form[hdnMovementDate.UniqueID];
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            filterExpressionSupplier = string.Format("GCBusinessPartnerType = '{0}' AND IsDeleted = 0", Constant.BusinessObjectType.SUPPLIER);

            txtDateFrom.Text = DateTime.Now.AddDays(-7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            BindGridView(1, true, ref PageCount);
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
            if (isCountPageCount)
            {
                string filterExpression = string.Format("GCBusinessPartnerType = '{0}' AND IsDeleted = 0", Constant.BusinessObjectType.SUPPLIER);
                int rowCount = BusinessLayer.GetBusinessPartnersRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 12);
            }

            String MovementDate = String.Format("{0}|{1}", Helper.GetDatePickerValue(txtDateFrom.Text).ToString("yyyyMMdd"), Helper.GetDatePickerValue(txtDateTo.Text).ToString("yyyyMMdd"));
            Int32 BusinessPartnerID = 0;
            if (hdnSupplierID.Value != "")
            {
                BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
            }

            hdnMovementDate.Value = MovementDate;
            List<GetPurchaseInvoiceInformation> lstEntity = BusinessLayer.GetPurchaseInvoiceInformationList(MovementDate, BusinessPartnerID, pageIndex, 12);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }
    }
}