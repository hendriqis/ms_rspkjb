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
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APInvoiceSupplierProcessCopyTestPartnerTransactionCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        protected List<DataTempCN> lstTemp = new List<DataTempCN>();

        private APInvoiceSupplierProcess DetailPage
        {
            get { return (APInvoiceSupplierProcess)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            string[] filter = param.Split('|');
            hdnPurchaseInvoiceIDCtl.Value = filter[0];
            hdnProductLineIDCtl.Value = filter[1];

            hdnIsUsedProductLineCtl.Value = AppSession.IsUsedProductLine;

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format("BusinessPartnerID = {0} AND GCTransactionStatus = '{1}'", AppSession.BusinessPartnerID, Constant.TransactionStatus.APPROVED);

            filterExpression += " AND TransactionID NOT IN (";
            filterExpression += "SELECT TestPartnerTransactionID FROM PurchaseInvoiceDt WITH(NOLOCK) WHERE TestPartnerTransactionID IS NOT NULL AND IsDeleted = 0";
            filterExpression += " AND PurchaseInvoiceID NOT IN (";
            filterExpression += string.Format("SELECT PurchaseInvoiceID FROM PurchaseInvoiceHd WITH(NOLOCK) WHERE GCTransactionStatus = '{0}'))", Constant.TransactionStatus.VOID);

            if (hdnIsUsedProductLineCtl.Value == "1")
            {
                filterExpression += string.Format(" AND ProductLineID = {0}", hdnProductLineIDCtl.Value);
            }

            List<vTestPartnerTransactionHd> lstEntity = BusinessLayer.GetvTestPartnerTransactionHdList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Save Entity

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseInvoiceHdDao entityHdDao = new PurchaseInvoiceHdDao(ctx);
            PurchaseInvoiceDtDao entityDtDao = new PurchaseInvoiceDtDao(ctx);
            TestPartnerTransactionHdDao entityTPTHdDao = new TestPartnerTransactionHdDao(ctx);

            int purchaseInvoiceID = 0;
            try
            {
                string errorMessage = "";
                DetailPage.SavePurchaseInvoiceHd(ctx, ref purchaseInvoiceID, ref errorMessage);
                if (String.IsNullOrEmpty(errorMessage))
                {
                    if (entityHdDao.Get(purchaseInvoiceID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        string filterExpression = string.Format("TransactionID IN ({0}) AND GCTransactionStatus = '{1}'",
                                                                        hdnSelectedTestPartnerTransactionID.Value.Substring(1), //0
                                                                        Constant.TransactionStatus.APPROVED //1
                                                                    );
                        List<TestPartnerTransactionHd> lstTPTHd = BusinessLayer.GetTestPartnerTransactionHdList(filterExpression, ctx);
                        foreach (TestPartnerTransactionHd tptHd in lstTPTHd)
                        {
                            PurchaseInvoiceDt invoiceDt = new PurchaseInvoiceDt();
                            invoiceDt.PurchaseInvoiceID = purchaseInvoiceID;
                            invoiceDt.TestPartnerTransactionID = tptHd.TransactionID;
                            invoiceDt.LineAmount = tptHd.NettPartnerTransactionAmount;
                            invoiceDt.CreatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Insert(invoiceDt);

                            tptHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                            tptHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityTPTHdDao.Update(tptHd);
                        }


                        decimal sum = 0;
                        decimal trans = 0;
                        decimal cn = 0;
                        string filterExpressionInvoiceDt = string.Format("PurchaseInvoiceID = {0} AND IsDeleted = 0", purchaseInvoiceID);
                        List<PurchaseInvoiceDt> lstEntity = BusinessLayer.GetPurchaseInvoiceDtList(filterExpressionInvoiceDt, ctx);
                        foreach (PurchaseInvoiceDt entityDt in lstEntity)
                        {
                            trans += entityDt.LineAmount;
                            cn += entityDt.CreditNoteAmount;
                        }
                        sum = trans - cn;

                        PurchaseInvoiceHd entityHd = entityHdDao.Get(purchaseInvoiceID);
                        entityHd.TotalNetTransactionAmount = sum;
                        entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entityHd);

                        retval = purchaseInvoiceID.ToString();
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Hutang supplier {0} tidak dapat diubah. Harap refresh halaman ini.", entityHdDao.Get(purchaseInvoiceID).PurchaseInvoiceNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = errorMessage;
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
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
        #endregion
    }

}