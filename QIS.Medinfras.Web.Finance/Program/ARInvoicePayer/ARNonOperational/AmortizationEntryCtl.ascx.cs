using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class AmortizationEntryCtl : BaseEntryPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            hdnARInvoiceDtID.Value = param;

            string filter = string.Format("ID = {0}", param);
            vARInvoiceDtNonOperational entity = BusinessLayer.GetvARInvoiceDtNonOperationalList(filter).FirstOrDefault();
            txtTransactionNonOperationalType.Text = string.Format("({0}) {1}", entity.TransactionNonOperationalTypeCode, entity.TransactionNonOperationalTypeName);
            txtCostRevenueSharing.Text = string.Format("({0}) {1}", entity.RevenueCostCenterCode, entity.RevenueCostCenterName);
            txtAmortizationPeriodInMonth.Text = entity.AmortizationPeriodInMonth.ToString();
            txtAmortizationFirstDate.Text = entity.AmortizationFirstDate.ToString(Constant.FormatString.DATE_FORMAT);
            txtClaimedAmortizationAmount.Text = entity.ClaimedAmount.ToString();
            txtTotalAmortizationAmount.Text = entity.TransactionAmount.ToString();

            BindGridView();
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string filter = string.Format("ARInvoiceDtID = {0} AND IsDeleted = 0 ORDER BY AmortizationDate", hdnARInvoiceDtID.Value);

            List<ARInvoiceDtAmortizationTemp> lst = BusinessLayer.GetARInvoiceDtAmortizationTempList(filter);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ARInvoiceDtAmortizationTemp entity = e.Item.DataItem as ARInvoiceDtAmortizationTemp;

                CheckBox chkIsSelectedDetail = (CheckBox)e.Item.FindControl("chkIsSelectedDetail");
                chkIsSelectedDetail.Checked = true;

                TextBox txtAmortizationAmount = (TextBox)e.Item.FindControl("txtAmortizationAmount");
                txtAmortizationAmount.Text = entity.AmortizationAmount.ToString();
                txtAmortizationAmount.Attributes.Add("hiddenVal", entity.AmortizationAmount.ToString());

                if (entity.AmortizationDate < DateTime.Now)
                {
                    txtAmortizationAmount.Attributes.Add("readonly", "readonly");
                }
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceDtAmortizationTempDao tempDao = new ARInvoiceDtAmortizationTempDao(ctx);

            try
            {
                if (hdnSelectedID.Value.Substring(0, 1) == ",")
                {
                    hdnSelectedID.Value = hdnSelectedID.Value.Substring(1);
                }

                if (hdnSelectedAmortizationAmount.Value.Substring(0, 1) == ",")
                {
                    hdnSelectedAmortizationAmount.Value = hdnSelectedAmortizationAmount.Value.Substring(1);
                }


                string[] lstSelectedID = hdnSelectedID.Value.Split(',');
                string[] lstSelectedAmortizationAmount = hdnSelectedAmortizationAmount.Value.Split(',');

                string filterTemp = string.Format("ARInvoiceDtID = {0} AND ID IN ({1}) AND IsDeleted = 0", hdnARInvoiceDtID.Value, hdnSelectedID.Value);
                List<ARInvoiceDtAmortizationTemp> lstTemp = BusinessLayer.GetARInvoiceDtAmortizationTempList(filterTemp, ctx);

                for (int i = 0; i < lstTemp.Count(); i++)
                {
                    int oID = Convert.ToInt32(lstSelectedID[i]);
                    decimal oAmortizationAmount = Convert.ToDecimal(lstSelectedAmortizationAmount[i]);

                    ARInvoiceDtAmortizationTemp entityTemp = tempDao.Get(oID);
                    entityTemp.AmortizationAmount = oAmortizationAmount;
                    entityTemp.LastUpdatedBy = AppSession.UserLogin.UserID;
                    tempDao.Update(entityTemp);
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

    }
}