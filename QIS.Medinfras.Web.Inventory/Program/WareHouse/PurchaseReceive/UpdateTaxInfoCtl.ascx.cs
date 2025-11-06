using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class UpdateTaxInfoCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnPurchaseReceiveID.Value = hdnParam.Value.Split('|')[0];

            PurchaseReceiveHd prhd = BusinessLayer.GetPurchaseReceiveHd(Convert.ToInt32(hdnPurchaseReceiveID.Value));
            List<PurchaseInvoiceDt> entityPIDT = BusinessLayer.GetPurchaseInvoiceDtList(string.Format("PurchaseReceiveID = {0}", prhd.PurchaseReceiveID));
            //hdnFlag.Value = IsHavePurchaseInvoiceDt(entityPIDT);
            IsHavePurchaseInvoiceDt(entityPIDT);
            EntityToControl(prhd);
        }

        private string IsHavePurchaseInvoiceDt(List<PurchaseInvoiceDt> dt)
        {
            //string flag = "";

            if (dt.Count > 0)
            {
                hdnFlag.Value = "1";
            }
            else
            {
                hdnFlag.Value = "2";
            }

            return hdnFlag.Value;
        }

        protected void cbpUpdateTaxInfo_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            if (param[0] == "refresh")
            {
                if (OnSaveRecord(ref errMessage))
                    result = "success";
                else
                    result = "fail|" + errMessage;
            }
            else if (param[0] == "update")
            {
                if (OnUpdateRecord(ref errMessage))
                    result = "success";
                else
                    result = "fail|" + errMessage;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void EntityToControl(PurchaseReceiveHd prhd)
        {
            txtTaxNo.Text = prhd.TaxInvoiceNo;
            txtTaxDate.Text = Convert.ToDateTime(prhd.TaxInvoiceDate).ToString(Constant.FormatString.DATE_PICKER_FORMAT);          
        }

        private bool OnSaveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReceiveHdDao entityDao = new PurchaseReceiveHdDao(ctx);
            try
            {
                PurchaseReceiveHd prhd = entityDao.Get(Convert.ToInt32(hdnPurchaseReceiveID.Value));

                if (prhd.IsIncludeVAT)
                {
                    prhd.TaxInvoiceNo = txtTaxNo.Text;
                    prhd.TaxInvoiceDate = Helper.GetDatePickerValue(txtTaxDate.Text);
                    entityDao.Update(prhd);
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, No.BPB <b>" + prhd.PurchaseReceiveNo + "</b> tidak memiliki PPN";
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

        private bool OnUpdateRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReceiveHdDao entityDao = new PurchaseReceiveHdDao(ctx);
            PurchaseInvoiceDtDao entityInvoiceDtDao = new PurchaseInvoiceDtDao(ctx);
            try
            {
                PurchaseReceiveHd prhd = entityDao.Get(Convert.ToInt32(hdnPurchaseReceiveID.Value));
                List<PurchaseInvoiceDt> pidt = BusinessLayer.GetPurchaseInvoiceDtList(string.Format("PurchaseReceiveID = {0}", prhd.PurchaseReceiveID), ctx);

                foreach (PurchaseInvoiceDt dt in pidt)
                {
                    PurchaseInvoiceDt pidtDao = entityInvoiceDtDao.Get(Convert.ToInt32(dt.ID));
                    pidtDao.TaxInvoiceNo = txtTaxNo.Text;
                    pidtDao.TaxInvoiceDate = Helper.GetDatePickerValue(txtTaxDate.Text);
                    entityInvoiceDtDao.Update(pidtDao);
                }
                if (prhd.IsIncludeVAT)
                {
                    prhd.TaxInvoiceNo = txtTaxNo.Text;
                    prhd.TaxInvoiceDate = Helper.GetDatePickerValue(txtTaxDate.Text);
                    entityDao.Update(prhd);
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, No.BPB <b>" + prhd.PurchaseReceiveNo + "</b> tidak memiliki PPN";
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