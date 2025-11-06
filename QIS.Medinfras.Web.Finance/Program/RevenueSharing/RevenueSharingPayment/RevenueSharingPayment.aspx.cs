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
    public partial class RevenueSharingPayment : BasePageTrx
    {
        private string[] lstSelectedMember = null;
        private string[] lstSelectedMemberPaymentDate = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_PAYMENT;
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

        private void BindGridView()
        {
            string filterExpression = string.Format("GCTransactionStatus = '{0}'", Constant.TransactionStatus.WAIT_FOR_APPROVAL);

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            lstSelectedMemberPaymentDate = hdnSelectedMemberPaymentDate.Value.Split(',');

            List<vTransRevenueSharingPaymentHd> lstEntity = BusinessLayer.GetvTransRevenueSharingPaymentHdList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vTransRevenueSharingPaymentHd entity = e.Item.DataItem as vTransRevenueSharingPaymentHd;
                CheckBox chkIsSelected = e.Item.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember != null)
                {
                    if (lstSelectedMember.Contains(entity.RSPaymentID.ToString()))
                        chkIsSelected.Checked = true;
                    else
                        chkIsSelected.Checked = false;
                }

                TextBox txtPaymentDate = e.Item.FindControl("txtPaymentDate") as TextBox;
                txtPaymentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

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
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingPaymentHdDao entityPaymentHdDao = new TransRevenueSharingPaymentHdDao(ctx);
            TransRevenueSharingSummaryHdDao entitySummaryHdDao = new TransRevenueSharingSummaryHdDao(ctx);

            List<String> lstPaymentDate = hdnSelectedMemberPaymentDate.Value.Split(',').ToList();
            List<String> lstPaymentMethod = hdnSelectedMemberPaymentMethod.Value.Split(',').ToList();
            List<String> lstBankID = hdnSelectedMemberBankID.Value.Split(',').ToList();
            List<String> lstReferenceNo = hdnSelectedMemberReference.Value.Split(',').ToList();
            lstPaymentDate.RemoveAt(0);
            lstPaymentMethod.RemoveAt(0);
            lstBankID.RemoveAt(0);
            lstReferenceNo.RemoveAt(0);

            if (type == "approve")
            {
                #region Approve

                int count = 0;
                List<TransRevenueSharingPaymentHd> lstHD = BusinessLayer.GetTransRevenueSharingPaymentHdList(string.Format("RSPaymentID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx);
                try
                {
                    foreach (TransRevenueSharingPaymentHd entity in lstHD)
                    {
                        if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL && result == true)
                        {
                            if (entity.RSPaymentDate != Helper.GetDatePickerValue(lstPaymentDate[count]))
                            {
                                entity.RSPaymentDate = Helper.GetDatePickerValue(lstPaymentDate[count]);
                            }

                            if (entity.GCSupplierPaymentMethod != lstPaymentMethod[count])
                            {
                                if (lstPaymentMethod[count] != "" && lstPaymentMethod[count] != "0")
                                {
                                    entity.GCSupplierPaymentMethod = lstPaymentMethod[count];
                                }
                            }

                            if (entity.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.TRANSFER ||
                                entity.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.GIRO ||
                                entity.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.CHEQUE)
                            {
                                if (entity.BankID != Convert.ToInt32(lstBankID[count]))
                                {
                                    if (lstBankID[count] != "" && lstBankID[count] != "0" && lstBankID[count] != "00")
                                    {
                                        entity.BankID = Convert.ToInt32(lstBankID[count]);
                                    }
                                    else if (lstBankID[count] == "00")
                                    {
                                        entity.BankID = null;
                                    }
                                }

                                if (entity.BankReferenceNo != lstReferenceNo[count])
                                {
                                    entity.BankReferenceNo = lstReferenceNo[count];
                                }
                            }
                            else
                            {
                                entity.BankID = null;
                                entity.BankReferenceNo = null;
                            }

                            entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            entity.ApprovedBy = AppSession.UserLogin.UserID;
                            entity.ApprovedDate = DateTime.Now;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityPaymentHdDao.Update(entity);

                            count++;
                        }
                        else
                        {
                            errMessage = "Pembayaran jasa medis tidak dapat diubah. Harap refresh halaman ini.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            result = false;
                        }
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

                #endregion
            }
            else if (type == "reopen")
            {
                #region Re-Open

                List<TransRevenueSharingPaymentHd> lstHD = BusinessLayer.GetTransRevenueSharingPaymentHdList(string.Format("RSPaymentID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx);
                try
                {
                    foreach (TransRevenueSharingPaymentHd entity in lstHD)
                    {
                        if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL && result == true)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityPaymentHdDao.Update(entity);

                            List<TransRevenueSharingSummaryHd> lstSummaryHd = BusinessLayer.GetTransRevenueSharingSummaryHdList(string.Format("RSSummaryID IN (SELECT RSSummaryID FROM TransRevenueSharingPaymentDt WHERE RSPaymentID = {0} AND IsDeleted = 0)", entity.RSPaymentID), ctx);
                            foreach (TransRevenueSharingSummaryHd summaryHd in lstSummaryHd)
                            {
                                summaryHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                summaryHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entitySummaryHdDao.Update(summaryHd);
                            }
                        }
                        else
                        {
                            errMessage = "Pembayaran jasa medis tidak dapat diubah. Harap refresh halaman ini.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            result = false;
                        }
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

                #endregion
            }
            return result;
        }
    }
}