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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class VoidFAItemCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] lstParam = param.Split('|');
            hdnParam.Value = lstParam[0];
            hdnTransactionCodeVoidFAItemCtl.Value = lstParam[1];

            string filterDt = string.Format("ID = {0} AND TransactionCode = '{1}'", hdnParam.Value, hdnTransactionCodeVoidFAItemCtl.Value);
            vPurchaseReceiveDtProductLine pdt = BusinessLayer.GetvPurchaseReceiveDtProductLineList(filterDt).FirstOrDefault();
            EntityToControl(pdt);
        }
        
        private void EntityToControl(vPurchaseReceiveDtProductLine pdt)
        {
            hdnProductLineID.Value = pdt.ProductLineID.ToString();
            txtProductLineCode.Text = pdt.ProductLineCode;
            txtProductLineName.Text = pdt.ProductLineName;
        }

        protected void cbpVoidFAItem_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            if (param[0] == "void")
            {
                if (OnUpdateRecord(ref errMessage))
                    result = "success";
                else
                    result = "fail|" + errMessage;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnUpdateRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReceiveDtDao purchaseReceiveDtDao = new PurchaseReceiveDtDao(ctx);
            DirectPurchaseDtDao directPurchaseDtDao = new DirectPurchaseDtDao(ctx);

            try
            {
                if (hdnTransactionCodeVoidFAItemCtl.Value == Constant.TransactionCode.DIRECT_PURCHASE)
                {
                    DirectPurchaseDt directPurchaseDt = directPurchaseDtDao.Get(Convert.ToInt32(hdnParam.Value));

                    string filterFAItem = string.Format("TransactionCode = '{0}' AND PurchaseReceiveID = {1} AND ItemID = {2} AND IsDeleted = 0", Constant.TransactionCode.DIRECT_PURCHASE, directPurchaseDt.DirectPurchaseID, directPurchaseDt.ItemID);
                    List<FAItem> lstFAItem = BusinessLayer.GetFAItemList(filterFAItem, ctx);

                    if (lstFAItem.Count == 0)
                    {
                        if (directPurchaseDt.ProductLineID == null)
                        {
                            directPurchaseDt.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
                            directPurchaseDt.FixedAssetVoidBy = AppSession.UserLogin.UserID;
                            directPurchaseDt.FixedAssetVoidDate = DateTime.Now;
                            directPurchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            directPurchaseDtDao.Update(directPurchaseDt);

                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Data penerimaan sudah dibatalkan menjadi Item Aset.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Data penerimaan sudah diproses menjadi Item Aset.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    PurchaseReceiveDt receiveDt = purchaseReceiveDtDao.Get(Convert.ToInt32(hdnParam.Value));

                    string filterFAItem = string.Format("TransactionCode = '{0}' AND PurchaseReceiveID = {1} AND ItemID = {2} AND IsDeleted = 0", Constant.TransactionCode.PURCHASE_RECEIVE, receiveDt.PurchaseReceiveID, receiveDt.ItemID);
                    List<FAItem> lstFAItem = BusinessLayer.GetFAItemList(filterFAItem, ctx);

                    if (lstFAItem.Count == 0)
                    {
                        if (receiveDt.ProductLineID == null)
                        {
                            receiveDt.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
                            receiveDt.FixedAssetVoidBy = AppSession.UserLogin.UserID;
                            receiveDt.FixedAssetVoidDate = DateTime.Now;
                            receiveDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            purchaseReceiveDtDao.Update(receiveDt);

                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Data penerimaan sudah dibatalkan menjadi Item Aset.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Data penerimaan sudah diproses menjadi Item Aset.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
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