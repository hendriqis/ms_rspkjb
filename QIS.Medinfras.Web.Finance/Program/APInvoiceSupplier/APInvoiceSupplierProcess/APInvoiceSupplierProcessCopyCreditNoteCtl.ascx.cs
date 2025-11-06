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
    public partial class APInvoiceSupplierProcessCopyCreditNoteCtl : BaseEntryPopupCtl
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
            SettingParameterDt setvar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_APPROVE_CN_WHEN_APPROVE_POR);

            string filterExpression = string.Format("BusinessPartnerID = {0}", AppSession.BusinessPartnerID);
            if (setvar.ParameterValue == "1")
            {
                filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}')", Constant.TransactionStatus.APPROVED);
            }
            else
            {
                filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}', '{1}')", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.APPROVED);
            }
            filterExpression += " AND CreditNoteID NOT IN (";
            filterExpression += "SELECT CreditNoteID FROM PurchaseInvoiceDt WHERE PurchaseReceiveID IS NOT NULL AND CreditNoteID IS NOT NULL AND IsDeleted = 0";
            filterExpression += " AND PurchaseInvoiceID NOT IN (";
            filterExpression += string.Format("SELECT PurchaseInvoiceID FROM PurchaseInvoiceHd WHERE GCTransactionStatus = '{0}'))", Constant.TransactionStatus.VOID);

            if (hdnIsUsedProductLineCtl.Value == "1")
            {
                filterExpression += string.Format(" AND ProductLineID = {0}", hdnProductLineIDCtl.Value);
            }

            lstSelectedMember = hdnSelectedCreditNote.Value.Split(',');
            List<vSupplierCreditNote> lstEntity = BusinessLayer.GetvSupplierCreditNoteList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vSupplierCreditNote entity = e.Item.DataItem as vSupplierCreditNote;
                CheckBox chkCreditNote = e.Item.FindControl("chkCreditNote") as CheckBox;
                if (lstSelectedMember.Contains(entity.PurchaseReceiveID.ToString()))
                    chkCreditNote.Checked = true;
                else
                    chkCreditNote.Checked = false;

                CheckBox chkPPN = e.Item.FindControl("chkPPN") as CheckBox;
                chkPPN.Checked = entity.IsIncludeVAT;

                TextBox txtCNAmountBeforePPN = e.Item.FindControl("txtCNAmountBeforePPN") as TextBox;
                txtCNAmountBeforePPN.Text = entity.CNAmount.ToString();

                TextBox txtCNAmountAfterPPN = e.Item.FindControl("txtCNAmountAfterPPN") as TextBox;
                txtCNAmountAfterPPN.Text = entity.TotalCreditNoteAmount.ToString();

                TextBox txtRemarks = e.Item.FindControl("txtRemarks") as TextBox;
                txtRemarks.Text = entity.Remarks;
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
            lstTemp = new List<DataTempCN>();

            for (int i = 0; i < paramNew.Length; i++)
            {
                if (!String.IsNullOrEmpty(paramNew[i]))
                {
                    string[] paramNewSplit = paramNew[i].Split('|');
                    int keyNew = Convert.ToInt32(paramNewSplit[1]);
                    Decimal cnAmountNew = Convert.ToDecimal(paramNewSplit[2]);
                    string remarkNew = paramNewSplit[3];

                    DataTempCN oData = new DataTempCN();
                    oData.Key = keyNew;
                    oData.CNAmountBeforePPN = cnAmountNew;
                    oData.Remarks = remarkNew;
                    lstTemp.Add(oData);
                }
            }
        }

        private void ControlToEntity(IDbContext ctx, List<PurchaseInvoiceDt> lstEntityDt)
        {
            SupplierCreditNoteDao entityCNDao = new SupplierCreditNoteDao(ctx);

            #region old
            //string filterExpression = string.Format("CreditNoteID IN ({0})", hdnSelectedCreditNote.Value.Substring(1));
            //List<vSupplierCreditNote> lstVCN = BusinessLayer.GetvSupplierCreditNoteList(filterExpression);
            //List<SupplierCreditNote> lstCN = BusinessLayer.GetSupplierCreditNoteList(filterExpression, ctx);

            //int count = 0;
            //List<String> lstSelectedCreditNote = hdnSelectedCreditNote.Value.Split(',').ToList();
            //List<String> lstSelectedCNAmount = hdnSelectedCNAmount.Value.Split(',').ToList();
            //List<String> lstSelectedRemarks = hdnSelectedRemarks.Value.Split(',').ToList();
            //lstSelectedCreditNote.RemoveAt(0);
            //lstSelectedCNAmount.RemoveAt(0);
            //lstSelectedRemarks.RemoveAt(0);

            //foreach (vSupplierCreditNote entity in lstVCN)
            //{
            //    PurchaseInvoiceDt entityDt = new PurchaseInvoiceDt();

            //    SupplierCreditNote entityCN = lstCN.FirstOrDefault(p => p.CreditNoteID == entity.CreditNoteID);
            //    int PurchaseReceiveID = 0;
            //    if (entityCN.PurchaseReturnID != 0 && entityCN.PurchaseReturnID != null)
            //    {
            //        PurchaseReturnHd entityPR = BusinessLayer.GetPurchaseReturnHdList(string.Format("PurchaseReturnID = {0}", entityCN.PurchaseReturnID), ctx).FirstOrDefault();
            //        PurchaseReceiveID = entityPR.PurchaseReceiveID;
            //    }

            //    entityCN.CNAmount = Convert.ToDecimal(lstSelectedCNAmount[count]);
            //    entityCN.Remarks = lstSelectedRemarks[count];
            //    entityCN.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
            //    entityCN.LastUpdatedBy = AppSession.UserLogin.UserID;

            //    decimal nilai = Convert.ToDecimal(lstSelectedCNAmount[count]);
            //    decimal ppn = entityCN.VATPercentage;
            //    decimal total = 0;
            //    if (entityCN.IsIncludeVAT)
            //    {
            //        total = nilai + (nilai * ppn / 100);
            //    }
            //    else
            //    {
            //        total = nilai;
            //    }

            //    if (hdnIsUsedProductLineCtl.Value == "1")
            //    {
            //        entityDt.ProductLineID = Convert.ToInt32(hdnProductLineIDCtl.Value);
            //    }

            //    entityDt.PurchaseReceiveID = PurchaseReceiveID;
            //    entityDt.CreditNoteID = entity.CreditNoteID;
            //    entityDt.CreditNoteAmount = total;
            //    entityDt.CreatedBy = AppSession.UserLogin.UserID;
            //    lstEntityDt.Add(entityDt);

            //    entityCNDao.Update(entityCN);
            //    count++;
            //}
            #endregion

            #region new
            setDataBeforeSave();
            foreach (DataTempCN e in lstTemp)
            {
                PurchaseInvoiceDt entityDt = new PurchaseInvoiceDt();

                SupplierCreditNote entityCN = entityCNDao.Get(e.Key);
                int PurchaseReceiveID = 0;
                if (entityCN.PurchaseReturnID != 0 && entityCN.PurchaseReturnID != null)
                {
                    PurchaseReturnHd entityPR = BusinessLayer.GetPurchaseReturnHdList(string.Format("PurchaseReturnID = {0}", entityCN.PurchaseReturnID), ctx).FirstOrDefault();
                    PurchaseReceiveID = entityPR.PurchaseReceiveID;
                }

                entityCN.CNAmount = e.CNAmountBeforePPN;
                entityCN.Remarks = e.Remarks;
                entityCN.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                entityCN.LastUpdatedBy = AppSession.UserLogin.UserID;

                decimal nilai = e.CNAmountBeforePPN;
                decimal ppn = entityCN.VATPercentage;
                decimal total = 0;
                if (entityCN.IsIncludeVAT)
                {
                    total = nilai + (nilai * ppn / 100);
                }
                else
                {
                    total = nilai;
                }

                if (hdnIsUsedProductLineCtl.Value == "1")
                {
                    entityDt.ProductLineID = Convert.ToInt32(hdnProductLineIDCtl.Value);
                }

                entityDt.PurchaseReceiveID = PurchaseReceiveID;
                entityDt.CreditNoteID = e.Key;
                entityDt.CreditNoteAmount = total;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                lstEntityDt.Add(entityDt);

                entityCNDao.Update(entityCN);
            }
            #endregion
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

    public class DataTempCN
    {
        public int Key { get; set; }
        public Decimal CNAmountBeforePPN { get; set; }
        public string Remarks { get; set; }
    }
}