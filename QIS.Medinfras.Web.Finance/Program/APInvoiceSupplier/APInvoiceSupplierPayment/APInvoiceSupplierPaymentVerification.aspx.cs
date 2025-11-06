using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APInvoiceSupplierPaymentVerification : BasePageTrx
    {
        private string[] lstSelectedMember = null;
        private string[] lstSelectedMemberPaymentDate = null;
        private string[] lstSelectedPaymentMethod = null;
        private string[] lstSelectedBankID = null;
        private string[] lstSelectedMemberReference = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AP_INVOICE_SUPPLIER_PAYMENT_VERIFICATION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTotalToBeVerified, new ControlEntrySetting(false, false, false, "0.00"));
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";

            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("GCTransactionStatus = '{0}' AND BusinessPartnerID = {1}",
                Constant.TransactionStatus.WAIT_FOR_APPROVAL, AppSession.BusinessPartnerID);

            return filterExpression;
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            lstSelectedMemberPaymentDate = hdnSelectedMemberPaymentDate.Value.Split(',');
            //lstSelectedPaymentMethod = hdnSelectedMemberPaymentMethod.Value.Split(',');
            lstSelectedBankID = hdnSelectedMemberBankID.Value.Split(',');
            lstSelectedMemberReference = hdnSelectedMemberReference.Value.Split(',');

            List<vSupplierPaymentHd> lstEntity = BusinessLayer.GetvSupplierPaymentHdList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vSupplierPaymentHd entity = e.Item.DataItem as vSupplierPaymentHd;
                CheckBox chkIsSelected = e.Item.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember != null)
                {
                    if (lstSelectedMember.Contains(entity.SupplierPaymentID.ToString()))
                        chkIsSelected.Checked = true;
                    else
                        chkIsSelected.Checked = false;
                }

                TextBox txtPaymentDate = e.Item.FindControl("txtPaymentDate") as TextBox;
                txtPaymentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                
                TextBox txtBankReferenceNo = e.Item.FindControl("txtBankReferenceNo") as TextBox;
                if (entity.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.TUNAI)
                {
                    txtBankReferenceNo.Enabled = false;
                }
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;

            if (type == "approve")
            {
                IDbContext ctx = DbFactory.Configure(true);
                SupplierPaymentHdDao supplierPaymentHDDao = new SupplierPaymentHdDao(ctx);

                List<String> lstPaymentDate = hdnSelectedMemberPaymentDate.Value.Split(',').ToList();
                //List<String> lstPaymentMethod = hdnSelectedMemberPaymentMethod.Value.Split(',').ToList();
                List<String> lstBankID = hdnSelectedMemberBankID.Value.Split(',').ToList();
                List<String> lstReferenceNo = hdnSelectedMemberReference.Value.Split(',').ToList();
                lstPaymentDate.RemoveAt(0);
                //lstPaymentMethod.RemoveAt(0);
                lstBankID.RemoveAt(0);
                lstReferenceNo.RemoveAt(0);

                int count = 0;
                List<SupplierPaymentHd> lstHD = BusinessLayer.GetSupplierPaymentHdList(String.Format(
                    "SupplierPaymentID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx);
                try
                {
                    foreach (SupplierPaymentHd entity in lstHD)
                    {
                        if (entity.PaymentDate != Helper.GetDatePickerValue(lstPaymentDate[count]))
                        {
                            entity.PaymentDate = Helper.GetDatePickerValue(lstPaymentDate[count]);
                        }
                        //if (entity.GCSupplierPaymentMethod != lstPaymentMethod[count])
                        //{
                        //    entity.GCSupplierPaymentMethod = lstPaymentMethod[count];
                        //}
                        if (entity.BankID != Convert.ToInt32(lstBankID[count]))
                        {
                            if (lstBankID[count] != "" && lstBankID[count] != "0")
                            {
                                entity.BankID = Convert.ToInt32(lstBankID[count]);
                            }
                        }
                        if (entity.BankReferenceNo != lstReferenceNo[count])
                        {
                            entity.BankReferenceNo = lstReferenceNo[count];
                        }
                        entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        supplierPaymentHDDao.Update(entity);
                        count++;
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            else if (type == "decline")
            {
                IDbContext ctx = DbFactory.Configure(true);
                SupplierPaymentHdDao supplierPaymentHDDao = new SupplierPaymentHdDao(ctx);
                PurchaseInvoiceHdDao purchaseInvoiceHDDao = new PurchaseInvoiceHdDao(ctx);

                List<SupplierPaymentHd> lstHD = BusinessLayer.GetSupplierPaymentHdList(String.Format(
                    "SupplierPaymentID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx);
                try
                {
                    foreach (SupplierPaymentHd entity in lstHD)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        supplierPaymentHDDao.Update(entity);

                        List<PurchaseInvoiceHd> lstInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(string.Format(
                            "PurchaseInvoiceID IN (SELECT PurchaseInvoiceID FROM SupplierPaymentDt WHERE SupplierPaymentID = {0})", entity.SupplierPaymentID));
                        foreach (PurchaseInvoiceHd invoice in lstInvoiceHd)
                        {
                            invoice.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                            invoice.LastUpdatedBy = AppSession.UserLogin.UserID;
                            purchaseInvoiceHDDao.Update(invoice);
                        }
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }
    }
}