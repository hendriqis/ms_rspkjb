using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class RecalculatePurchaseReceiveDetailCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnPurchaseReceiveID.Value = param;
            PurchaseReceiveHd entity = BusinessLayer.GetPurchaseReceiveHd(Convert.ToInt32(hdnPurchaseReceiveID.Value));
            txtPurchaseReceiveNo.Text = entity.PurchaseReceiveNo;

            BindGridView();
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvPurchaseReceiveDtList(string.Format("PurchaseReceiveID = '{0}' AND GCItemDetailStatus != '{1}'", hdnPurchaseReceiveID.Value, Constant.TransactionStatus.VOID));
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (OnSaveEditRecord(ref errMessage))
            {
                result += "success";
            }
            else
            {
                result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PurchaseReceiveDt entity)
        {
            entity.UnitPrice = Convert.ToDecimal(txtPrice.Text);
            entity.DiscountPercentage1 = Convert.ToDecimal(txtDiscount.Text);
            entity.DiscountPercentage2 = Convert.ToDecimal(txtDiscount2.Text);
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PurchaseReceiveDt entity = BusinessLayer.GetPurchaseReceiveDt(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePurchaseReceiveDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}