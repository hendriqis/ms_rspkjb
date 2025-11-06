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
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APSupplierVerificationApprovalVoidList : BasePageTrx
    {
        private string[] lstSelectedMember = null;
        private string[] lstSelectedPaymentMethod = null;
        private string[] lstSelectedBankID = null;
        private string[] lstSelectedMemberReference = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AP_SUPPLIER_VERIFICATION_APPROVAL_VOID;
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

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowApproved.Value = CRUDMode.Contains("A") ? "1" : "0";

            //List<Bank> listBank = BusinessLayer.GetBankList(string.Format("IsDeleted = 0 AND GCBankType = '{0}'", Constant.BankType.BANK_HUTANG));
            //if (listBank.Count > 0)
            //{
            //    hdnCountBankHutang.Value = listBank.Count.ToString();
            //}
            //else
            //{
            //    hdnCountBankHutang.Value = "0";
            //}

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

            if (hdnIsAllowApproved.Value == "1")
            {
                filterExpression += string.Format("GCTransactionStatus = '{0}' AND GCApprovalTransactionStatus IN ('{0}','{1}')",
                    Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.APPROVED);
            }
            else
            {

                filterExpression += string.Format("GCTransactionStatus = '{0}' AND GCApprovalTransactionStatus = '{0}'",
                    Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            }

            return filterExpression;
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            lstSelectedPaymentMethod = hdnSelectedMemberPaymentMethod.Value.Split(',');
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

            if (type == "reopen")
            {
                IDbContext ctx = DbFactory.Configure(true);
                SupplierPaymentHdDao supplierPaymentHDDao = new SupplierPaymentHdDao(ctx);
                PurchaseInvoiceHdDao purchaseInvoiceHDDao = new PurchaseInvoiceHdDao(ctx);

                bool isTransactionLocked = false;
                TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);
                TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.SUPPLIER_PAYMENT_VERIFICATION);

                List<SupplierPaymentHd> lstHD = BusinessLayer.GetSupplierPaymentHdList(String.Format("SupplierPaymentID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx);
                try
                {
                    if (entityLock.LockedUntilDate != null)
                    {
                        DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                        DateTime DateNow = DateTime.Now;
                        if (DateNow > DateLock)
                        {
                            isTransactionLocked = false;
                        }
                        else
                        {
                            isTransactionLocked = true;
                        }
                    }
                    else
                    {
                        isTransactionLocked = false;
                    }

                    if (!isTransactionLocked)
                    {
                        foreach (SupplierPaymentHd entity in lstHD)
                        {
                            if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL && entity.GCApprovalTransactionStatus == Constant.TransactionStatus.APPROVED && result == true)
                            {
                                if (hdnIsAllowApproved.Value == "1")
                                {
                                    if (entity.ApprovalProposedBy != null)
                                    {
                                        if (entity.ApprovalProposedBy == entity.ApprovalApprovedBy)
                                        {
                                            entity.GCApprovalTransactionStatus = Constant.TransactionStatus.OPEN;
                                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        }
                                        else
                                        {
                                            entity.GCApprovalTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        }
                                    }
                                    else
                                    {
                                        entity.GCApprovalTransactionStatus = Constant.TransactionStatus.OPEN;
                                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    }
                                }

                                //supplierPaymentHDDao.Update(entity);

                                //List<PurchaseInvoiceHd> lstInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(string.Format("PurchaseInvoiceID IN (SELECT PurchaseInvoiceID FROM SupplierPaymentDt WHERE SupplierPaymentID = {0})", entity.SupplierPaymentID), ctx);
                                //foreach (PurchaseInvoiceHd invoice in lstInvoiceHd)
                                //{
                                //    invoice.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                //    invoice.LastUpdatedBy = AppSession.UserLogin.UserID;
                                //    purchaseInvoiceHDDao.Update(invoice);
                                //}
                            }
                            else if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL && entity.GCApprovalTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL && result == true)
                            {
                                entity.GCApprovalTransactionStatus = Constant.TransactionStatus.OPEN;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            }
                            else
                            {
                                errMessage = "Pembatalan Approval Verifikasi Hutang Supplier tidak dapat diubah. Harap refresh halaman ini.";
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                result = false;
                            }
                            supplierPaymentHDDao.Update(entity);
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }

                    if (result)
                    {
                        ctx.CommitTransaction();
                    }
                    else
                    {
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
            }
            return result;
        }
    }
}