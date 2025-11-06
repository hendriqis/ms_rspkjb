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
    public partial class APInvoiceSupplierProcessCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        protected List<DataTemp> lstTemp = new List<DataTemp>();

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
            hdnBusinessPartnerID.Value = AppSession.BusinessPartnerID.ToString();

            txtFilterDate.Text = filter[1];

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')", AppSession.UserLogin.HealthcareID,
                                                        Constant.SettingParameter.FN_SELISIH_HARI_JATUH_TEMPO,
                                                        Constant.SettingParameter.FN_INVOICE_DT_REMARKS_COPY_RECEIVE_REMARKS,
                                                        Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE);
            List<SettingParameterDt> setvarList = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            txtFilterDay.Text = setvarList.Where(a => a.ParameterCode == Constant.SettingParameter.FN_SELISIH_HARI_JATUH_TEMPO).FirstOrDefault().ParameterValue;
            hdnIsRemarksDtCopyFromPOR.Value = setvarList.Where(a => a.ParameterCode == Constant.SettingParameter.FN_INVOICE_DT_REMARKS_COPY_RECEIVE_REMARKS).FirstOrDefault().ParameterValue;
            hdnIsUsedProductLineCtl.Value = setvarList.Where(a => a.ParameterCode == Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE).FirstOrDefault().ParameterValue;

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format("SupplierID = {0} AND GCTransactionStatus IN ('{1}','{2}')", hdnBusinessPartnerID.Value, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED);

            // cek utk POR yg bukan dari PO Termin yg IsPurchaseReceiveRequired = 1
            filterExpression += string.Format(" AND PurchaseReceiveID NOT IN (SELECT ISNULL(prd.PurchaseReceiveID,0) FROM PurchaseReceiveDt prd INNER JOIN PurchaseOrderHd poh ON poh.PurchaseOrderID = prd.PurchaseOrderID INNER JOIN PurchaseOrderTerm pot ON pot.PurchaseOrderID = prd.PurchaseOrderID WHERE poh.IsUsingTermPO = 1 AND prd.GCItemDetailStatus != '{0}' AND poh.GCTransactionStatus != '{0}')", Constant.TransactionStatus.VOID);

            // cek utk POR yg blm ada di tukar faktur
            filterExpression += " AND PurchaseReceiveID NOT IN (SELECT PurchaseReceiveID FROM PurchaseInvoiceDt WHERE PurchaseReceiveID IS NOT NULL AND CreditNoteID IS NULL AND IsDeleted = 0";
            filterExpression += string.Format(" AND PurchaseInvoiceID NOT IN (SELECT PurchaseInvoiceID FROM PurchaseInvoiceHd WHERE GCTransactionStatus = '{0}'))", Constant.TransactionStatus.VOID);

            if (hdnIsChecked.Value == "1")
            {
                filterExpression += string.Format(" AND PaymentDueDate < '{0}'",
                    (Helper.GetDatePickerValue(txtFilterDate.Text).AddDays(Convert.ToDouble(txtFilterDay.Text))).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (hdnIsUsedProductLineCtl.Value == "1")
            {
                filterExpression += string.Format(" AND ProductLineID = {0}", hdnProductLineIDCtl.Value);
            }

            filterExpression += string.Format(" AND TransactionCode NOT IN ('{0}','{1}')", Constant.TransactionCode.CONSIGNMENT_RECEIVE, Constant.TransactionCode.DONATION_RECEIVE);

            filterExpression += " ORDER BY PurchaseReceiveID";

            lstSelectedMember = hdnSelectedPurchaseReceive.Value.Split(',');
            List<vPurchaseReceiveHd> lstEntity = BusinessLayer.GetvPurchaseReceiveHdList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPurchaseReceiveHd entity = e.Item.DataItem as vPurchaseReceiveHd;
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
            lstTemp = new List<DataTemp>();

            for (int i = 0; i < paramNew.Length; i++)
            {
                if (!String.IsNullOrEmpty(paramNew[i]))
                {
                    string[] paramNewSplit = paramNew[i].Split('|');
                    int keyNew = Convert.ToInt32(paramNewSplit[1]);
                    string referenceNoNew = paramNewSplit[2];
                    DateTime referenceDateNew = Helper.GetDatePickerValue(paramNewSplit[3]);

                    string taxInvoiceNoNew = paramNewSplit[4];
                    string[] taxInvoiceNoTemp = paramNewSplit[4].Split('^');
                    if (!string.IsNullOrEmpty(taxInvoiceNoTemp[0]))
                    {
                        taxInvoiceNoNew = string.Format("{0}|{1}", taxInvoiceNoTemp[0], taxInvoiceNoTemp[1]);
                    }
                    else
                    {
                        taxInvoiceNoNew = taxInvoiceNoTemp[1];
                    }

                    DateTime taxInvoiceDateNew = Helper.GetDatePickerValue(paramNewSplit[5]);
                    decimal netAmountNew = Convert.ToDecimal(paramNewSplit[6]);

                    DataTemp oData = new DataTemp();
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

            #region old
            //int count = 0;

            //string filterExpression = string.Format("PurchaseReceiveID IN ({0})", hdnSelectedPurchaseReceive.Value.Substring(1));
            //List<PurchaseReceiveHd> lstPORHD = BusinessLayer.GetPurchaseReceiveHdList(filterExpression, ctx);

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

            //foreach (PurchaseReceiveHd purchaseReceive in lstPORHD)
            //{
            //    PurchaseInvoiceDt entityDt = new PurchaseInvoiceDt();

            //    entityDt.PurchaseReceiveID = purchaseReceive.PurchaseReceiveID;
            //    entityDt.DiscountAmount = purchaseReceive.DiscountAmount;
            //    entityDt.ChargesAmount = purchaseReceive.ChargesAmount;
            //    entityDt.CreditNoteAmount = 0;
            //    entityDt.VATAmount = Convert.ToDecimal(purchaseReceive.VATPercentage * (purchaseReceive.TransactionAmount - purchaseReceive.DiscountAmount - purchaseReceive.FinalDiscount) / 100);
            //    entityDt.PPHAmount = purchaseReceive.PPHAmount;
            //    entityDt.FinalDiscountAmount = purchaseReceive.FinalDiscount;
            //    entityDt.DownPaymentAmount = purchaseReceive.DownPaymentAmount;
            //    entityDt.StampAmount = purchaseReceive.StampAmount;
            //    entityDt.TransactionAmount = purchaseReceive.TransactionAmount;
            //    entityDt.ReferenceNo = lstReferenceNo[count];
            //    entityDt.ReferenceDate = Helper.GetDatePickerValue(lstReferenceDate[count]);
            //    entityDt.TaxInvoiceNo = lstTaxInvoiceNo[count];
            //    entityDt.TaxInvoiceDate = Helper.GetDatePickerValue(lstTaxInvoiceDate[count]);

            //    if (purchaseReceive.ReferenceNo != lstReferenceNo[count])
            //        purchaseReceive.ReferenceNo = lstReferenceNo[count];
            //    if (purchaseReceive.ReferenceDate != Helper.GetDatePickerValue(lstReferenceDate[count]))
            //        purchaseReceive.ReferenceDate = Helper.GetDatePickerValue(lstReferenceDate[count]);
            //    if (purchaseReceive.TaxInvoiceNo != lstTaxInvoiceNo[count])
            //        purchaseReceive.TaxInvoiceNo = lstTaxInvoiceNo[count];
            //    if (purchaseReceive.TaxInvoiceDate != Helper.GetDatePickerValue(lstTaxInvoiceDate[count]))
            //        purchaseReceive.TaxInvoiceDate = Helper.GetDatePickerValue(lstTaxInvoiceDate[count]);

            //    decimal lineAmount = entityDt.TransactionAmount - entityDt.DiscountAmount - entityDt.FinalDiscountAmount
            //                            + entityDt.VATAmount + entityDt.PPHAmount + entityDt.PPH23Amount + entityDt.PPH25Amount + entityDt.StampAmount
            //                            + entityDt.ChargesAmount - entityDt.CreditNoteAmount - entityDt.DownPaymentAmount;
            //    decimal endAmount = Convert.ToDecimal(lstNetAmount[count]);
            //    decimal roundingAmount = endAmount - lineAmount;

            //    entityDt.RoundingAmount = roundingAmount;
            //    entityDt.LineAmount = endAmount;

            //    if (hdnIsUsedProductLineCtl.Value == "1")
            //    {
            //        entityDt.ProductLineID = Convert.ToInt32(hdnProductLineIDCtl.Value);
            //    }

            //    if (hdnIsRemarksDtCopyFromPOR.Value == "1")
            //    {
            //        entityDt.Remarks = purchaseReceive.Remarks;
            //    }

            //    lstEntityDt.Add(entityDt);

            //    purchaseReceive.LastUpdatedBy = AppSession.UserLogin.UserID;
            //    entityPRHdDao.Update(purchaseReceive);

            //    count++;
            //}
            #endregion

            #region new
            setDataBeforeSave();
            foreach (DataTemp e in lstTemp)
            {
                PurchaseReceiveHd purchaseReceive = entityPRHdDao.Get(e.Key);

                PurchaseInvoiceDt entityDt = new PurchaseInvoiceDt();

                entityDt.PurchaseReceiveID = purchaseReceive.PurchaseReceiveID;
                entityDt.DiscountAmount = purchaseReceive.DiscountAmount;
                entityDt.ChargesAmount = purchaseReceive.ChargesAmount;
                entityDt.CreditNoteAmount = 0;
                entityDt.VATAmount = Convert.ToDecimal(Convert.ToDecimal(purchaseReceive.VATPercentage * (purchaseReceive.TransactionAmount - purchaseReceive.DiscountAmount - purchaseReceive.FinalDiscount) / 100).ToString("N2"));
                entityDt.PPHAmount = purchaseReceive.PPHAmount;
                entityDt.FinalDiscountAmount = purchaseReceive.FinalDiscount;
                entityDt.DownPaymentAmount = purchaseReceive.DownPaymentAmount;
                entityDt.StampAmount = purchaseReceive.StampAmount;
                entityDt.TransactionAmount = purchaseReceive.TransactionAmount;
                entityDt.ReferenceNo = e.ReferenceNo;
                entityDt.ReferenceDate = e.ReferenceDate;
                entityDt.TaxInvoiceNo = e.TaxInvoiceNo;
                entityDt.TaxInvoiceDate = e.TaxInvoiceDate;

                if (purchaseReceive.ReferenceNo != e.ReferenceNo)
                    purchaseReceive.ReferenceNo = e.ReferenceNo;
                if (purchaseReceive.ReferenceDate != e.ReferenceDate)
                    purchaseReceive.ReferenceDate = e.ReferenceDate;
                if (purchaseReceive.TaxInvoiceNo != e.TaxInvoiceNo)
                    purchaseReceive.TaxInvoiceNo = e.TaxInvoiceNo;
                if (purchaseReceive.TaxInvoiceDate != e.TaxInvoiceDate)
                    purchaseReceive.TaxInvoiceDate = e.TaxInvoiceDate;

                decimal lineAmount = entityDt.TransactionAmount - entityDt.DiscountAmount - entityDt.FinalDiscountAmount
                                        + entityDt.VATAmount + entityDt.PPHAmount + entityDt.PPH23Amount + entityDt.PPH25Amount + entityDt.StampAmount
                                        + entityDt.ChargesAmount - entityDt.CreditNoteAmount - entityDt.DownPaymentAmount;
                decimal endAmount = e.NetAmount;
                decimal roundingAmount = endAmount - lineAmount;

                entityDt.RoundingAmount = roundingAmount;
                entityDt.LineAmount = endAmount;

                if (hdnIsUsedProductLineCtl.Value == "1")
                {
                    entityDt.ProductLineID = Convert.ToInt32(hdnProductLineIDCtl.Value);
                }

                if (hdnIsRemarksDtCopyFromPOR.Value == "1")
                {
                    entityDt.Remarks = purchaseReceive.Remarks;
                }

                lstEntityDt.Add(entityDt);

                purchaseReceive.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityPRHdDao.Update(purchaseReceive);
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
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
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

    public class DataTemp
    {
        public int Key { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime ReferenceDate { get; set; }
        public string TaxInvoiceNo { get; set; }
        public DateTime TaxInvoiceDate { get; set; }
        public decimal NetAmount { get; set; }
    }
}