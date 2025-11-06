using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class DoctorFeeEntryCtl1 : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnTransactionID.Value = paramInfo[0];
            SetControlProperties();
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string lstRecordID = hdnSelectedID.Value;
            string lstRecordRemarks = hdnSelectedRemarks.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            result = ProcessDoctorFeeEntry(hdnTransactionID.Value, lstRecordID, lstRecordRemarks);

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string ProcessDoctorFeeEntry(string prescriptionOrderID, string lstRecordID, string lstRecordRemarks)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);

            try
            {
                string filterExpression = string.Format("PrescriptionOrderID = {0} AND PrescriptionOrderDetailID IN ({1}) ORDER BY PrescriptionOrderDetailID", prescriptionOrderID, lstRecordID);
                List<PrescriptionOrderDt> lstDetail = BusinessLayer.GetPrescriptionOrderDtList(filterExpression, ctx);
                int i = 0;
                string[] remarksInfo = lstRecordRemarks.Split(',');
                foreach (PrescriptionOrderDt item in lstDetail)
                {
                    item.IsAlertConfirmed = true;
                    item.AlertConfirmedBy = AppSession.UserLogin.ParamedicID;
                    item.AlertConfirmedRemarks = remarksInfo[i];
                    orderDtDao.Update(item);
                    i += 1;
                }
                ctx.CommitTransaction();
                result = string.Format("process|1|");
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("process|0|{0}", ex.Message);
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        private void SetControlProperties()
        {
            BindGridView();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPhysicianItem item = (vPhysicianItem)e.Item.DataItem;
                CheckBox chkIsFOC = (CheckBox)e.Item.FindControl("chkIsFOC");
                TextBox txtDiscount = (TextBox)e.Item.FindControl("txtDiscount");
                TextBox txtUnitPrice = (TextBox)e.Item.FindControl("txtUnitPrice");
                if (item != null && txtUnitPrice != null)
                {
                    chkIsFOC.Enabled = item.IsUnbilledItem;
                    txtDiscount.Enabled = item.IsAllowDiscount;
                    txtUnitPrice.Enabled = item.IsAllowVariable;

                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID, AppSession.RegisteredPatient.ChargeClassID, item.ItemID, 1, DateTime.Now);

                    decimal basePrice = 0;
                    decimal basePriceComp1 = 0;
                    decimal basePriceComp2 = 0;
                    decimal basePriceComp3 = 0;
                    decimal price = 0;
                    decimal priceComp1 = 0;
                    decimal priceComp2 = 0;
                    decimal priceComp3 = 0;
                    bool isDiscountUsedComp = false;
                    decimal discountAmount = 0;
                    decimal discountAmountComp1 = 0;
                    decimal discountAmountComp2 = 0;
                    decimal discountAmountComp3 = 0;
                    decimal coverageAmount = 0;
                    bool isDiscountInPercentage = false;
                    bool isDiscountInPercentageComp1 = false;
                    bool isDiscountInPercentageComp2 = false;
                    bool isDiscountInPercentageComp3 = false;
                    bool isCoverageInPercentage = false;
                    decimal costAmount = 0;

                    if (list.Count > 0)
                    {
                        GetCurrentItemTariff obj = list[0];
                        basePrice = obj.BasePrice;
                        basePriceComp1 = obj.BasePriceComp1;
                        basePriceComp2 = obj.BasePriceComp2;
                        basePriceComp3 = obj.BasePriceComp3;
                        price = obj.Price;
                        priceComp1 = obj.PriceComp1;
                        priceComp2 = obj.PriceComp2;
                        priceComp3 = obj.PriceComp3;
                        isDiscountUsedComp = obj.IsDiscountUsedComp;
                        discountAmount = obj.DiscountAmount;
                        discountAmountComp1 = obj.DiscountAmountComp1;
                        discountAmountComp2 = obj.DiscountAmountComp2;
                        discountAmountComp3 = obj.DiscountAmountComp3;
                        coverageAmount = obj.CoverageAmount;
                        isDiscountInPercentage = obj.IsDiscountInPercentage;
                        isDiscountInPercentageComp1 = obj.IsDiscountInPercentageComp1;
                        isDiscountInPercentageComp2 = obj.IsDiscountInPercentageComp2;
                        isDiscountInPercentageComp3 = obj.IsDiscountInPercentageComp3;
                        isCoverageInPercentage = obj.IsCoverageInPercentage;
                        costAmount = obj.CostAmount;
                    }

                    txtUnitPrice.Text = price.ToString("N2");
                }
            }
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("ParamedicID = {0}", AppSession.UserLogin.ParamedicID);

            List<vPhysicianItem> lstEntity = BusinessLayer.GetvPhysicianItemList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}