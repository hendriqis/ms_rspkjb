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
    public partial class APInvoiceSupplierProcessConsignmentFromOrderPerDetailReceiveCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        public List<DataTempConsOrderFromReceive> lstTemp = new List<DataTempConsOrderFromReceive>();

        private APInvoiceSupplierProcess DetailPage
        {
            get { return (APInvoiceSupplierProcess)Page; }
        }

        protected string DateTimeNowDatePicker()
        {
            return DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        public override void InitializeDataControl(string param)
        {
            string[] filter = param.Split('|');
            hdnPurchaseInvoiceIDCtl.Value = filter[0];
            hdnDueDate.Value = Helper.GetDatePickerValue(filter[1]).ToString();
            hdnProductLineIDCtl.Value = filter[2];
            hdnIsChecked.Value = "1";

            txtFilterDate.Text = filter[1];

            hdnIsUsedProductLineCtl.Value = AppSession.IsUsedProductLine;

            SettingParameterDt setvar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_SELISIH_HARI_JATUH_TEMPO);
            txtFilterDay.Text = setvar.ParameterValue;

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format("SupplierID = {0} AND GCTransactionStatus IN ('{1}','{2}')",
                AppSession.BusinessPartnerID, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.CLOSED);
            filterExpression += " AND PurchaseOrderID NOT IN (";
            filterExpression += "SELECT PurchaseOrderID FROM PurchaseInvoiceDt WHERE PurchaseOrderID IS NOT NULL AND CreditNoteID IS NULL AND IsDeleted = 0";
            filterExpression += " AND PurchaseInvoiceID NOT IN (";
            filterExpression += string.Format("SELECT PurchaseInvoiceID FROM PurchaseInvoiceHd WHERE GCTransactionStatus = '{0}'))", Constant.TransactionStatus.VOID);

            if (hdnIsChecked.Value == "1")
            {
                filterExpression += string.Format(" AND PaymentDueDate < '{0}'",
                    (Helper.GetDatePickerValue(txtFilterDate.Text).AddDays(Convert.ToDouble(txtFilterDay.Text))).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (hdnIsUsedProductLineCtl.Value == "1")
            {
                filterExpression += string.Format(" AND ProductLineID = {0}", hdnProductLineIDCtl.Value);
            }

            filterExpression += " ORDER BY PurchaseOrderID";

            lstSelectedMember = hdnSelectedPurchaseReceive.Value.Split(',');
            List<vPurchaseOrderReceiveConsignmentPerDetailReceive> lstEntity = BusinessLayer.GetvPurchaseOrderReceiveConsignmentPerDetailReceiveList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPurchaseOrderReceiveConsignmentPerDetailReceive entity = e.Item.DataItem as vPurchaseOrderReceiveConsignmentPerDetailReceive;
                CheckBox chkPurchaseReceive = e.Item.FindControl("chkPurchaseReceive") as CheckBox;
                if (lstSelectedMember.Contains(entity.PurchaseReceiveID.ToString()))
                    chkPurchaseReceive.Checked = true;
                else
                    chkPurchaseReceive.Checked = false;

                TextBox txtReferenceNo = e.Item.FindControl("txtReferenceNo") as TextBox;
                txtReferenceNo.Text = entity.ReferenceNo;

                TextBox txtReferenceDate = e.Item.FindControl("txtReferenceDate") as TextBox;
                txtReferenceDate.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                TextBox txtTaxInvoiceNoPref = e.Item.FindControl("txtTaxInvoiceNoPref") as TextBox;
                TextBox txtTaxInvoiceNo = e.Item.FindControl("txtTaxInvoiceNo") as TextBox;
                if (entity.TaxInvoiceNo != null && entity.TaxInvoiceNo != "")
                {
                    string[] temp = entity.TaxInvoiceNo.Split('|');
                    txtTaxInvoiceNo.Text = entity.TaxInvoiceNo;
                    if (temp.Length > 1)
                    {
                        txtTaxInvoiceNoPref.Text = temp[0];
                        txtTaxInvoiceNo.Text = temp[1];
                    }
                }
                else
                {
                    string[] temp = entity.ReferenceNo.Split('|');
                    txtTaxInvoiceNo.Text = entity.ReferenceNo;
                    if (temp.Length > 1)
                    {
                        txtTaxInvoiceNoPref.Text = temp[0];
                        txtTaxInvoiceNo.Text = temp[1];
                    }
                }

                if (String.IsNullOrEmpty(txtTaxInvoiceNo.Text) || txtTaxInvoiceNo.Text == "-" || txtTaxInvoiceNo.Text != "0")
                {
                    txtTaxInvoiceNoPref.Text = "010";
                }

                TextBox txtTaxInvoiceDate = e.Item.FindControl("txtTaxInvoiceDate") as TextBox;
                if (entity.TaxInvoiceDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
                {
                    txtTaxInvoiceDate.Text = entity.TaxInvoiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }
                else
                {
                    txtTaxInvoiceDate.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }

                TextBox txtPaymentDueDate = e.Item.FindControl("txtPaymentDueDate") as TextBox;
                txtPaymentDueDate.Text = entity.PaymentDueDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                TextBox txtNetAmount = e.Item.FindControl("txtNetAmount") as TextBox;
                txtNetAmount.Text = entity.NetTransactionAmount.ToString("N2");
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
            lstTemp = new List<DataTempConsOrderFromReceive>();

            for (int i = 0; i < paramNew.Length; i++)
            {
                if (!String.IsNullOrEmpty(paramNew[i]))
                {
                    string[] paramNewSplit = paramNew[i].Split('|');
                    int poIDNew = Convert.ToInt32(paramNewSplit[1]);
                    int keyNew = Convert.ToInt32(paramNewSplit[2]);
                    string referenceNoNew = paramNewSplit[3];
                    DateTime referenceDateNew = Helper.GetDatePickerValue(paramNewSplit[4]);

                    string taxInvoiceNoNew = paramNewSplit[5];
                    string[] taxInvoiceNoTemp = paramNewSplit[5].Split('^');
                    if (!string.IsNullOrEmpty(taxInvoiceNoTemp[0]))
                    {
                        taxInvoiceNoNew = string.Format("{0}|{1}", taxInvoiceNoTemp[0], taxInvoiceNoTemp[1]);
                    }
                    else
                    {
                        taxInvoiceNoNew = taxInvoiceNoTemp[1];
                    }

                    DateTime taxInvoiceDateNew = Helper.GetDatePickerValue(paramNewSplit[6]);
                    decimal netAmountNew = Convert.ToDecimal(paramNewSplit[7]);

                    DataTempConsOrderFromReceive oData = new DataTempConsOrderFromReceive();
                    oData.poID = poIDNew;
                    oData.Key = keyNew;
                    oData.ReferenceNo = referenceNoNew;
                    oData.ReferenceDate = referenceDateNew;
                    oData.TaxInvoiceNo = taxInvoiceNoNew;
                    oData.TaxInvoiceDate = taxInvoiceDateNew;
                    oData.NetAmount = netAmountNew;
                    lstTemp.Add(oData);
                }
            }
        }

        private void ControlToEntity(IDbContext ctx, List<PurchaseInvoiceDt> lstEntityDt)
        {
            PurchaseReceiveHdDao entityPRHdDao = new PurchaseReceiveHdDao(ctx);
            PurchaseOrderHdDao orderHdDao = new PurchaseOrderHdDao(ctx);

            #region old
            ////////string filterExpression = string.Format("PurchaseReceiveID IN ({0})", hdnSelectedPurchaseReceive.Value.Substring(1));
            ////////List<vPurchaseReceiveHd> lstPurchaseReceive = BusinessLayer.GetvPurchaseReceiveHdList(filterExpression);
            ////////List<PurchaseReceiveHd> lstPrhd = BusinessLayer.GetPurchaseReceiveHdList(filterExpression, ctx);

            //string filterExpressionPO = string.Format("PurchaseOrderID IN ({0})", hdnSelectedPurchaseOrder.Value.Substring(1));
            //List<PurchaseOrderHd> lstPurchaseOrder = BusinessLayer.GetPurchaseOrderHdList(filterExpressionPO);

            //int count = 0;
            //List<String> lstReferenceNo = hdnSelectedReferenceNo.Value.Split(',').ToList();
            //List<String> lstReferenceDate = hdnSelectedReferenceDate.Value.Split(',').ToList();
            //List<String> lstTaxInvoiceNo = hdnSelectedTaxInvoiceNo.Value.Split(',').ToList();
            //List<String> lstTaxInvoiceDate = hdnSelectedTaxInvoiceDate.Value.Split(',').ToList();
            //List<String> lstNetAmount = hdnSelectedNetAmount.Value.Split(',').ToList();

            //lstReferenceNo.RemoveAt(0);
            //lstReferenceDate.RemoveAt(0);
            //lstTaxInvoiceNo.RemoveAt(0);
            //lstTaxInvoiceDate.RemoveAt(0);
            //lstNetAmount.RemoveAt(0);

            //foreach (PurchaseOrderHd purchaseOrder in lstPurchaseOrder)
            //{
            //    PurchaseInvoiceDt entityDt = new PurchaseInvoiceDt();

            //    string filterPOPRDt = string.Format("PurchaseOrderID = {0} AND GCItemDetailStatus != '{1}'", purchaseOrder.PurchaseOrderID, Constant.TransactionStatus.VOID);
            //    PurchaseReceiveDt porDt = BusinessLayer.GetPurchaseReceiveDtList(filterPOPRDt, ctx).FirstOrDefault();
            //    string filterPOPRHd = string.Format("PurchaseReceiveID = {0}", porDt.PurchaseReceiveID);
            //    PurchaseReceiveHd porHd = BusinessLayer.GetPurchaseReceiveHdList(filterPOPRHd, ctx).FirstOrDefault();

            //    entityDt.PurchaseOrderID = purchaseOrder.PurchaseOrderID;
            //    entityDt.PurchaseReceiveID = porHd.PurchaseReceiveID;
            //    entityDt.DiscountAmount = purchaseOrder.FinalDiscount;
            //    entityDt.ChargesAmount = porHd.ChargesAmount;
            //    //entityDt.CreditNoteAmount = purchaseReceive.CNAmount;
            //    entityDt.CreditNoteAmount = 0;
            //    entityDt.PPH23Amount = 0; // ini juga perlu dipertanyakan soalnya di receive kan ga ada pph
            //    entityDt.PPH25Amount = 0;
            //    entityDt.FinalDiscountAmount = purchaseOrder.FinalDiscount;
            //    entityDt.DownPaymentAmount = purchaseOrder.DownPaymentAmount;
            //    entityDt.StampAmount = porHd.StampAmount;
            //    entityDt.TransactionAmount = purchaseOrder.TransactionAmount;
            //    entityDt.ReferenceNo = lstReferenceNo[count];
            //    entityDt.TaxInvoiceNo = lstTaxInvoiceNo[count];
            //    entityDt.ReferenceDate = Helper.GetDatePickerValue(lstReferenceDate[count]);
            //    entityDt.TaxInvoiceDate = Helper.GetDatePickerValue(lstTaxInvoiceDate[count]);

            //    if (porHd.ReferenceNo != lstReferenceNo[count])
            //        porHd.ReferenceNo = lstReferenceNo[count];
            //    if (porHd.ReferenceDate != Helper.GetDatePickerValue(lstReferenceDate[count]))
            //        porHd.ReferenceDate = Helper.GetDatePickerValue(lstReferenceDate[count]);
            //    if (porHd.TaxInvoiceNo != lstTaxInvoiceNo[count])
            //        porHd.TaxInvoiceNo = lstTaxInvoiceNo[count];
            //    if (porHd.TaxInvoiceDate != Helper.GetDatePickerValue(lstTaxInvoiceDate[count]))
            //        porHd.TaxInvoiceDate = Helper.GetDatePickerValue(lstTaxInvoiceDate[count]);

            //    #region Hitung Total

            //    decimal total = purchaseOrder.TransactionAmount;

            //    decimal totalDiskon = 0;
            //    if (purchaseOrder.FinalDiscount > 0)
            //    {
            //        totalDiskon = (purchaseOrder.FinalDiscount / 100 * purchaseOrder.TransactionAmount);
            //    }

            //    decimal ppn = 0;
            //    if (purchaseOrder.IsIncludeVAT)
            //    {
            //        ppn = ((purchaseOrder.VATPercentage * (purchaseOrder.TransactionAmount - totalDiskon)) / 100);
            //    }
            //    else
            //    {
            //        ppn = 0;
            //    }

            //    decimal totalPemesanan = (total - totalDiskon) + ppn;

            //    #endregion

            //    entityDt.VATAmount = ppn;

            //    decimal lineAmount = totalPemesanan;
            //    decimal endAmount = Convert.ToDecimal(lstNetAmount[count]);
            //    decimal roundingAmount = endAmount - lineAmount;

            //    entityDt.RoundingAmount = roundingAmount;
            //    entityDt.LineAmount = endAmount;

            //    if (hdnIsUsedProductLineCtl.Value == "1")
            //    {
            //        entityDt.ProductLineID = Convert.ToInt32(hdnProductLineIDCtl.Value);
            //    }

            //    lstEntityDt.Add(entityDt);

            //    porHd.LastUpdatedBy = AppSession.UserLogin.UserID;
            //    entityPRHdDao.Update(porHd);

            //    count++;
            //}
            #endregion

            #region new
            setDataBeforeSave();
            foreach (DataTempConsOrderFromReceive e in lstTemp)
            {
                PurchaseOrderHd purchaseOrder = orderHdDao.Get(e.poID);
                PurchaseInvoiceDt entityDt = new PurchaseInvoiceDt();

                //string filterPOPRDt = string.Format("PurchaseOrderID = {0} AND GCItemDetailStatus != '{1}'", purchaseOrder.PurchaseOrderID, Constant.TransactionStatus.VOID);
                //PurchaseReceiveDt porDt = BusinessLayer.GetPurchaseReceiveDtList(filterPOPRDt, ctx).FirstOrDefault();

                string filterPOPRHd = string.Format("PurchaseReceiveID = {0}", e.Key);
                PurchaseReceiveHd porHd = BusinessLayer.GetPurchaseReceiveHdList(filterPOPRHd, ctx).FirstOrDefault();

                entityDt.PurchaseOrderID = purchaseOrder.PurchaseOrderID;
                entityDt.PurchaseReceiveID = porHd.PurchaseReceiveID;
                entityDt.DiscountAmount = purchaseOrder.FinalDiscount;
                entityDt.ChargesAmount = porHd.ChargesAmount;
                //entityDt.CreditNoteAmount = purchaseReceive.CNAmount;
                entityDt.CreditNoteAmount = 0;
                entityDt.PPH23Amount = 0; // ini juga perlu dipertanyakan soalnya di receive kan ga ada pph
                entityDt.PPH25Amount = 0;
                entityDt.FinalDiscountAmount = purchaseOrder.FinalDiscount;
                entityDt.DownPaymentAmount = purchaseOrder.DownPaymentAmount;
                entityDt.StampAmount = porHd.StampAmount;
                entityDt.TransactionAmount = purchaseOrder.TransactionAmount;
                entityDt.ReferenceNo = e.ReferenceNo;
                entityDt.TaxInvoiceNo = e.TaxInvoiceNo;
                entityDt.ReferenceDate = e.ReferenceDate;
                entityDt.TaxInvoiceDate = e.TaxInvoiceDate;

                if (porHd.ReferenceNo != e.ReferenceNo)
                    porHd.ReferenceNo = e.ReferenceNo;
                if (porHd.ReferenceDate != e.ReferenceDate)
                    porHd.ReferenceDate = e.ReferenceDate;
                if (porHd.TaxInvoiceNo != e.TaxInvoiceNo)
                    porHd.TaxInvoiceNo = e.TaxInvoiceNo;
                if (porHd.TaxInvoiceDate != e.TaxInvoiceDate)
                    porHd.TaxInvoiceDate = e.TaxInvoiceDate;

                #region Hitung Total

                decimal total = purchaseOrder.TransactionAmount;

                decimal totalDiskon = 0;
                if (purchaseOrder.FinalDiscount > 0)
                {
                    totalDiskon = (purchaseOrder.FinalDiscount / 100 * purchaseOrder.TransactionAmount);
                }

                decimal ppn = 0;
                if (purchaseOrder.IsIncludeVAT)
                {
                    ppn = ((purchaseOrder.VATPercentage * (purchaseOrder.TransactionAmount - totalDiskon)) / 100);
                }
                else
                {
                    ppn = 0;
                }

                decimal totalPemesanan = (total - totalDiskon) + ppn;

                #endregion

                entityDt.VATAmount = ppn;

                decimal lineAmount = totalPemesanan;
                decimal endAmount = e.NetAmount;
                decimal roundingAmount = endAmount - lineAmount;

                entityDt.RoundingAmount = roundingAmount;
                entityDt.LineAmount = endAmount;

                if (hdnIsUsedProductLineCtl.Value == "1")
                {
                    entityDt.ProductLineID = Convert.ToInt32(hdnProductLineIDCtl.Value);
                }

                lstEntityDt.Add(entityDt);

                porHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityPRHdDao.Update(porHd);
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

    public class DataTempConsOrderFromReceive
    {
        public int poID { get; set; }
        public int Key { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime ReferenceDate { get; set; }
        public string TaxInvoiceNo { get; set; }
        public DateTime TaxInvoiceDate { get; set; }
        public decimal NetAmount { get; set; }
    }
}