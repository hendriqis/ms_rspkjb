using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APInvoiceSupplierProcessPOTermCtl : BaseEntryPopupCtl
    {
        protected List<DataTempPOTerm> lstTemp = new List<DataTempPOTerm>();

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
            string filterExpression = string.Format("BusinessPartnerID = {0} AND TermAmount <> InvoiceAmount AND IsAllowProcessedAP = 1", AppSession.BusinessPartnerID);

            if (hdnIsUsedProductLineCtl.Value == "1")
            {
                filterExpression += string.Format(" AND ProductLineID = {0}", hdnProductLineIDCtl.Value);
            }

            filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}')", Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED);

            List<vPurchaseOrderTerm> lstEntity = BusinessLayer.GetvPurchaseOrderTermList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPurchaseOrderTerm entity = e.Item.DataItem as vPurchaseOrderTerm;
                TextBox txtTaxInvoiceNoPref = e.Item.FindControl("txtTaxInvoiceNoPref") as TextBox;

                txtTaxInvoiceNoPref.Text = "010";
            }
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
        protected void setDataBeforeSave()
        {
            string[] paramNew = hdnDataSave.Value.Split('$');
            lstTemp = new List<DataTempPOTerm>();

            for (int i = 0; i < paramNew.Length; i++)
            {
                if (!String.IsNullOrEmpty(paramNew[i]))
                {
                    string[] paramNewSplit = paramNew[i].Split('|');
                    int keyNew = Convert.ToInt32(paramNewSplit[1]);
                    Decimal termAmountNew = Convert.ToDecimal(paramNewSplit[2]);

                    string TaxInvoiceNoNew = paramNewSplit[3];
                    string[] taxInvoiceNoTemp = paramNewSplit[3].Split('^');
                    if (!string.IsNullOrEmpty(taxInvoiceNoTemp[0]))
                    {
                        TaxInvoiceNoNew = string.Format("{0}|{1}", taxInvoiceNoTemp[0], taxInvoiceNoTemp[1]);
                    }
                    else
                    {
                        TaxInvoiceNoNew = taxInvoiceNoTemp[1];
                    }

                    DateTime TaxInvoiceDateNew = Helper.GetDatePickerValue(paramNewSplit[4]);

                    DataTempPOTerm oData = new DataTempPOTerm();
                    oData.Key = keyNew;
                    oData.TermAmount = termAmountNew;
                    oData.txtTaxInvoiceNo = TaxInvoiceNoNew;
                    oData.txtTaxInvoiceDate = TaxInvoiceDateNew;
                    lstTemp.Add(oData);
                }
            }
        }

        private void ControlToEntity(IDbContext ctx, List<PurchaseInvoiceDt> lstEntityDt)
        {
            PurchaseOrderTermDao entityPOTermDao = new PurchaseOrderTermDao(ctx);

            setDataBeforeSave();
            foreach (DataTempPOTerm e in lstTemp)
            {
                PurchaseInvoiceDt entityDt = new PurchaseInvoiceDt();

                PurchaseOrderTerm entityPOTerm = entityPOTermDao.Get(e.Key);

                entityPOTerm.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                entityPOTerm.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityPOTermDao.Update(entityPOTerm);

                if (hdnIsUsedProductLineCtl.Value == "1")
                {
                    entityDt.ProductLineID = Convert.ToInt32(hdnProductLineIDCtl.Value);
                }
                entityDt.PurchaseOrderTermID = entityPOTerm.PurchaseOrderTermID;
                entityDt.PurchaseOrderID = entityPOTerm.PurchaseOrderID;

                string filterPOR = string.Format("PurchaseOrderID = {0} AND GCTransactionStatus <> '{1}' AND GCItemDetailStatus <> '{1}'", entityPOTerm.PurchaseOrderID, Constant.TransactionStatus.VOID);
                List<vPurchaseReceiveDt> lstPOR = BusinessLayer.GetvPurchaseReceiveDtList(filterPOR, ctx);
                if (lstPOR.Count() > 0)
                {
                    entityDt.PurchaseReceiveID = lstPOR.LastOrDefault().PurchaseReceiveID;
                }

                entityDt.ReferenceNo = null;
                entityDt.ReferenceDate = null;
                entityDt.TaxInvoiceNo = e.txtTaxInvoiceNo;
                if (e.txtTaxInvoiceDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                {
                    entityDt.TaxInvoiceDate = e.txtTaxInvoiceDate;
                }
                else
                {
                    entityDt.TaxInvoiceDate = null;
                }
                entityDt.TransactionAmount = e.TermAmount;
                entityDt.LineAmount = e.TermAmount;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;

                lstEntityDt.Add(entityDt);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseInvoiceHdDao entityHdDao = new PurchaseInvoiceHdDao(ctx);
            PurchaseInvoiceDtDao entityDtDao = new PurchaseInvoiceDtDao(ctx);

            int purchaseInvoiceID = 0;
            try
            {
                string errorMessage = "";
                DetailPage.SavePurchaseInvoiceHd(ctx, ref purchaseInvoiceID, ref errorMessage);
                if (String.IsNullOrEmpty(errorMessage))
                {
                    if (entityHdDao.Get(purchaseInvoiceID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        List<PurchaseInvoiceDt> lstEntityDt = new List<PurchaseInvoiceDt>();

                        ControlToEntity(ctx, lstEntityDt);
                        foreach (PurchaseInvoiceDt entityDt in lstEntityDt)
                        {
                            entityDt.PurchaseInvoiceID = purchaseInvoiceID;
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Insert(entityDt);
                        }

                        string filterExpression = string.Format("PurchaseInvoiceID = {0} AND IsDeleted = 0", purchaseInvoiceID);
                        List<PurchaseInvoiceDt> lstEntity = BusinessLayer.GetPurchaseInvoiceDtList(filterExpression, ctx);

                        decimal sum = 0;
                        decimal trans = 0;
                        decimal cn = 0;
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

    public class DataTempPOTerm
    {
        public int Key { get; set; }
        public Decimal TermAmount { get; set; }
        public String txtTaxInvoiceNo { get; set; }
        public DateTime txtTaxInvoiceDate { get; set; }
    }
}