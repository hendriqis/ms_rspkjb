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
    public partial class RevenueSharingVerification : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        private string[] lstNilai = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_PAYMENT_VERIFICATION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetRevenuePaymentMethodTransfer()
        {
            return Constant.RevenuePaymentMethod.TRANSFER;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoidByReason.Value = CRUDMode.Contains("X") ? "1" : "0";

            hdnIsAdd.Value = "1";

            Helper.SetControlEntrySetting(txtPaymentDate, new ControlEntrySetting(true, false, false), "mpEntry");
            Helper.SetControlEntrySetting(txtVerificationDate, new ControlEntrySetting(true, false, false), "mpEntry");

            Helper.SetControlEntrySetting(txtRSSummaryDateFrom, new ControlEntrySetting(true, false, false), "mpEntry");
            Helper.SetControlEntrySetting(txtRSSummaryDateTo, new ControlEntrySetting(true, false, false), "mpEntry");

            txtRSSummaryDateFrom.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRSSummaryDateTo.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                                            "ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0",
                                                                            Constant.StandardCode.REVENUE_PAYMENT_METHOD)
                                                                        );
            Methods.SetComboBoxField<StandardCode>(cboPaymentMethod, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.REVENUE_PAYMENT_METHOD).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            cboPaymentMethod.Value = Constant.RevenuePaymentMethod.TUNAI;

            List<Bank> lstBank = BusinessLayer.GetBankList(string.Format("IsDeleted = 0 AND (GCBankType = '{0}' OR GCBankType IS NULL)", Constant.BankType.BANK_HUTANG));
            Methods.SetComboBoxField<Bank>(cboBank, lstBank, "BankName", "BankID");
            cboBank.SelectedIndex = 0;

            BindGridView();
        }

        protected String IsAdd()
        {
            return hdnIsAdd.Value;
        }

        public override void OnAddRecord()
        {
            hdnIsAdd.Value = "1";
            hdnIsEditable.Value = "1";
            IsAdd();
            BindGridView();
        }

        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnFilterExpressionQuickSearch, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(hdnSelectedMember, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(hdnIsAdd, new ControlEntrySetting(false, false, false, "1"));
            SetControlEntrySetting(hdnRSPaymentID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtPaymentDate, new ControlEntrySetting(true, false, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtVerificationDate, new ControlEntrySetting(true, false, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(cboPaymentMethod, new ControlEntrySetting(true, false, true, Constant.RevenuePaymentMethod.TUNAI));
            SetControlEntrySetting(cboBank, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtBankReferenceNo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtRSSummaryDateFrom, new ControlEntrySetting(true, true, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtRSSummaryDateTo, new ControlEntrySetting(true, true, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
        }

        public override int OnGetRowCount()
        {
            string filterExpression = "";
            return BusinessLayer.GetvTransRevenueSharingPaymentHdRowCount(filterExpression);
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            hdnIsAdd.Value = "0";
            string filterExpression = "";
            vTransRevenueSharingPaymentHd entity = BusinessLayer.GetvTransRevenueSharingPaymentHd(filterExpression, PageIndex, "RSPaymentID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            hdnIsAdd.Value = "0";
            string filterExpression = "";
            if (keyValue != "0" && keyValue != "" && keyValue != null)
            {
                filterExpression = string.Format("RSPaymentNo = '{0}'", keyValue);
                PageIndex = BusinessLayer.GetvTransRevenueSharingPaymentHdRowIndex(filterExpression, keyValue, "RSPaymentID DESC");
                vTransRevenueSharingPaymentHd entity = BusinessLayer.GetvTransRevenueSharingPaymentHd(filterExpression, PageIndex, "RSPaymentID DESC");
                EntityToControl(entity, ref isShowWatermark, ref watermarkText);
            }
            else
            {
                BindGridView();
            }
        }

        private void EntityToControl(vTransRevenueSharingPaymentHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                hdnIsEditable.Value = "0";
                watermarkText = entity.TransactionStatus;
                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL || entity.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                {
                    SetControlEntrySetting(txtPaymentDate, new ControlEntrySetting(false, false, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                    SetControlEntrySetting(txtVerificationDate, new ControlEntrySetting(false, false, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                    SetControlEntrySetting(txtRemarks, new ControlEntrySetting(false, false, false));
                }
            }
            else
            {
                hdnIsEditable.Value = "1";

            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                cboPaymentMethod.Enabled = true;
            }
            else
            {
                cboPaymentMethod.Enabled = false;
            } 

            hdnRSPaymentID.Value = entity.RSPaymentID.ToString();
            txtPaymentNo.Text = entity.RSPaymentNo;
            txtVerificationDate.Text = entity.VerificationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPaymentDate.Text = entity.RSPaymentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRemarks.Text = entity.Remarks;
            cboPaymentMethod.Value = entity.GCSupplierPaymentMethod;
            if (entity.GCSupplierPaymentMethod == GetRevenuePaymentMethodTransfer())
            {
                trBank.Attributes.Remove("style");
                trBankRef.Attributes.Remove("style");
                cboBank.Value = entity.BankID.ToString();
                txtBankReferenceNo.Text = entity.BankReferenceNo;
            }
            else
            {
                trBank.Attributes.Add("style", "display:none");
                trBankRef.Attributes.Add("style", "display:none");
            }

            hdnTransactionStatus.Value = entity.GCTransactionStatus;

            List<vTransRevenueSharingPaymentDt> dt = BusinessLayer.GetvTransRevenueSharingPaymentDtList(string.Format("RSPaymentID = {0} AND IsDeleted = 0", entity.RSPaymentID));

            decimal total = 0;
            foreach (vTransRevenueSharingPaymentDt lst in dt)
            {
                total += lst.TotalRevenueSharingAmount;
            }

            txtRSPaymentAmount.Text = total.ToString(Constant.FormatString.NUMERIC_2);

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);

            if (entity.ApprovedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trApprovedBy.Style.Add("display", "none");
                trApprovedDate.Style.Add("display", "none");

                divApprovedBy.InnerHtml = "";
                divApprovedDate.InnerHtml = "";
            }
            else
            {
                trApprovedBy.Style.Remove("display");
                trApprovedDate.Style.Remove("display");

                divApprovedBy.InnerHtml = entity.ApprovedByName;
                divApprovedDate.InnerHtml = entity.ApprovedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.VoidDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trVoidBy.Style.Add("display", "none");
                trVoidDate.Style.Add("display", "none");

                divVoidBy.InnerHtml = "";
                divVoidDate.InnerHtml = "";
            }
            else
            {
                trVoidBy.Style.Remove("display");
                trVoidDate.Style.Remove("display");

                divVoidBy.InnerHtml = entity.VoidByName;
                divVoidDate.InnerHtml = entity.VoidDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedBy.InnerHtml = "";
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            BindGridView();
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            else
            {
                result = "refresh";
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingSummaryHdDao summaryHdDao = new TransRevenueSharingSummaryHdDao(ctx);
            TransRevenueSharingPaymentDtDao paymentDtDao = new TransRevenueSharingPaymentDtDao(ctx);

            if (hdnRSSummaryID.Value != "")
            {
                try
                {
                    TransRevenueSharingPaymentDt entity = paymentDtDao.Get(Convert.ToInt32(hdnRSSummaryID.Value));
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    paymentDtDao.Update(entity);

                    TransRevenueSharingSummaryHd entityHd = summaryHdDao.Get(Convert.ToInt32(entity.RSSummaryID));
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    summaryHdDao.Update(entityHd);

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
            }
            return result;
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 1";
            if (hdnIsAdd.Value == "0")
            {
                if (hdnRSPaymentID.Value != "" && hdnRSPaymentID.Value != "0")
                {
                    filterExpression = string.Format("RSPaymentID = {0} AND IsDeleted = 0 ORDER BY RSSummaryID", hdnRSPaymentID.Value);
                    List<vTransRevenueSharingPaymentDt> lstEntity = BusinessLayer.GetvTransRevenueSharingPaymentDtList(filterExpression);
                    grdView.DataSource = lstEntity;
                    grdView.DataBind();
                }
            }
            else
            {
                if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
                {
                    filterExpression = hdnFilterExpressionQuickSearch.Value;
                }

                if (!string.IsNullOrEmpty(txtRSSummaryDateFrom.Text) && !string.IsNullOrEmpty(txtRSSummaryDateTo.Text))
                {
                    if (filterExpression != "")
                    {
                        filterExpression += " AND ";
                    }
                    filterExpression += string.Format("RSSummaryDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtRSSummaryDateFrom.Text), Helper.GetDatePickerValue(txtRSSummaryDateTo.Text));
                }
                else
                {
                    if (filterExpression != "")
                    {
                        filterExpression += " AND ";
                    }
                    filterExpression += string.Format("RSSummaryDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtRSSummaryDateFrom.Text), Helper.GetDatePickerValue(txtRSSummaryDateTo.Text));
                }

                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("GCTransactionStatus = '{0}'", Constant.TransactionStatus.APPROVED);

                if (cboPaymentMethod.Value != null)
                {
                    if (filterExpression != "")
                    {
                        filterExpression += " AND ";
                    }

                    filterExpression += string.Format("(GCRevenueSharingPayment = '{0}' OR GCRevenueSharingPayment IS NULL)", cboPaymentMethod.Value);
                }
                else
                {
                    if (filterExpression != "")
                    {
                        filterExpression += " AND ";
                    }
                    filterExpression += "GCRevenueSharingPayment IS NULL";
                }

                filterExpression += string.Format(" ORDER BY RSSummaryID");

                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                lstNilai = hdnSelectedPayment.Value.Split(',');

                List<vTransRevenueSharingSummaryHd> lst = BusinessLayer.GetvTransRevenueSharingSummaryHdList(filterExpression);
                lvwView.DataSource = lst;
                lvwView.DataBind();
            }
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vTransRevenueSharingSummaryHd entity = e.Item.DataItem as vTransRevenueSharingSummaryHd;
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");

                if (lstSelectedMember.Contains(entity.RSSummaryID.ToString()))
                {
                    int idx = Array.IndexOf(lstSelectedMember, entity.RSSummaryID.ToString());
                    chkIsSelected.Checked = true;
                }
            }
        }

        #region Save Edit
        public void SaveTransRevenueSharingPaymentHd(IDbContext ctx, ref int RSPaymentID, ref string errorMessage)
        {
            TransRevenueSharingPaymentHdDao entityHdDao = new TransRevenueSharingPaymentHdDao(ctx);
            if (hdnRSPaymentID.Value == "0")
            {
                TransRevenueSharingPaymentHd entityHd = new TransRevenueSharingPaymentHd();
                entityHd.RSPaymentDate = Helper.GetDatePickerValue(txtPaymentDate.Text);
                entityHd.VerificationDate = Helper.GetDatePickerValue(txtVerificationDate.Text);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.Remarks = txtRemarks.Text;
                entityHd.GCSupplierPaymentMethod = cboPaymentMethod.Value.ToString();

                if (entityHd.GCSupplierPaymentMethod == GetRevenuePaymentMethodTransfer())
                {
                    entityHd.BankID = Convert.ToInt32(cboBank.Value.ToString());
                    entityHd.BankReferenceNo = txtBankReferenceNo.Text;
                }
                else
                {
                    entityHd.BankID = null;
                    entityHd.BankReferenceNo = null;
                }

                entityHd.RSPaymentNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.TRANS_REVENUE_SHARING_PAYMENT_ENTRY, entityHd.VerificationDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                RSPaymentID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                RSPaymentID = Convert.ToInt32(hdnRSPaymentID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingPaymentDtDao entityDtDao = new TransRevenueSharingPaymentDtDao(ctx);
            TransRevenueSharingSummaryHdDao entitySummaryHdDao = new TransRevenueSharingSummaryHdDao(ctx);
            TransRevenueSharingSummaryDtDao entitySummaryDtDao = new TransRevenueSharingSummaryDtDao(ctx);
            RevenueSharingFeeHdDao entityRevenueHdDao = new RevenueSharingFeeHdDao(ctx);

            String RSSummaryID = hdnSelectedMember.Value;
            String RevenueSharingFeeID = hdnSelectedMember2.Value;

            try
            {
                if (RSSummaryID != "" || RevenueSharingFeeID != "")
                {
                    int RSPaymentID = 0;
                    string errorMessage = "";
                    SaveTransRevenueSharingPaymentHd(ctx, ref RSPaymentID, ref errorMessage);

                    if (String.IsNullOrEmpty(errorMessage))
                    {
                        if (RSSummaryID != "")
                        {
                            string[] lstSelectedRSSummaryID = hdnSelectedMember.Value.Substring(1).Split(',');

                            List<TransRevenueSharingSummaryHd> lstTransRevenueSharingSummaryHd = BusinessLayer.GetTransRevenueSharingSummaryHdList(string.Format("RSSummaryID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx);

                            for (int i = 0; i < lstSelectedRSSummaryID.Length; ++i)
                            {
                                int oRSSummaryID = Convert.ToInt32(lstSelectedRSSummaryID[i]);
                                TransRevenueSharingSummaryHd rsSummaryHd = lstTransRevenueSharingSummaryHd.FirstOrDefault(p => p.RSSummaryID == oRSSummaryID);

                                TransRevenueSharingPaymentDt entityDt = new TransRevenueSharingPaymentDt();
                                entityDt.RSPaymentID = RSPaymentID;
                                entityDt.RSSummaryID = oRSSummaryID;
                                //entityDt.TotalRevenueSharingAmount = (rsSummaryHd.TotalRevenueSharingAmount + rsSummaryHd.TotalAdjustmentAmount);
                                entityDt.TotalRevenueSharingAmount = rsSummaryHd.TotalRevenueSharingAmount;
                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Insert(entityDt);

                                rsSummaryHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                rsSummaryHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entitySummaryHdDao.Update(rsSummaryHd);
                            }
                        }

                        if (RevenueSharingFeeID != "")
                        {
                            string[] lstSelectedRSSummaryID2 = hdnSelectedMember2.Value.Substring(1).Split(',');

                            List<RevenueSharingFeeHd> lstRevenueSharingFeeHd = BusinessLayer.GetRevenueSharingFeeHdList(string.Format("RevenueSharingFeeID IN ({0})", hdnSelectedMember2.Value.Substring(1)), ctx);

                            for (int i = 0; i < lstSelectedRSSummaryID2.Length; ++i)
                            {
                                int oSharingFeeID = Convert.ToInt32(lstSelectedRSSummaryID2[i]);
                                RevenueSharingFeeHd sharingFeeHd = lstRevenueSharingFeeHd.FirstOrDefault(p => p.RevenueSharingFeeID == oSharingFeeID);

                                sharingFeeHd.isPayment = true;
                                sharingFeeHd.RSPaymentID = RSPaymentID;
                                sharingFeeHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityRevenueHdDao.Update(sharingFeeHd);
                            }
                        }
                    }

                    retval = RSPaymentID.ToString();
                    ctx.CommitTransaction();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingPaymentHdDao entityHdDao = new TransRevenueSharingPaymentHdDao(ctx);

            try
            {
                TransRevenueSharingPaymentHd entity = entityHdDao.Get(Convert.ToInt32(hdnRSPaymentID.Value));
                entity.Remarks = txtRemarks.Text;
                entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entity);

                retval = entity.RSPaymentID.ToString();
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

        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingPaymentHdDao entityHdDao = new TransRevenueSharingPaymentHdDao(ctx);
            TransRevenueSharingSummaryHdDao entitySummaryHdDao = new TransRevenueSharingSummaryHdDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                TransRevenueSharingPaymentHd entity = BusinessLayer.GetTransRevenueSharingPaymentHd(Convert.ToInt32(hdnRSPaymentID.Value));

                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.Remarks = txtRemarks.Text;
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.VerifiedBy = AppSession.UserLogin.UserID;
                    entity.VerifiedDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);

                    List<TransRevenueSharingPaymentDt> lstEntityDt = BusinessLayer.GetTransRevenueSharingPaymentDtList(string.Format("RSPaymentID = {0} AND IsDeleted = 0", hdnRSPaymentID.Value), ctx);
                    foreach (TransRevenueSharingPaymentDt paymentDt in lstEntityDt)
                    {
                        TransRevenueSharingSummaryHd summaryHd = entitySummaryHdDao.Get(paymentDt.RSSummaryID);
                        summaryHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                        summaryHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entitySummaryHdDao.Update(summaryHd);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi verifikasi jasa medis tidak dapat diubah. Harap refresh halaman ini.";
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

        #region Custom Button Click
        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingSummaryHdDao oTransRevenueSharingSummaryHDDao = new TransRevenueSharingSummaryHdDao(ctx);
            TransRevenueSharingSummaryDtDao oTransRevenueSharingSummaryDTDao = new TransRevenueSharingSummaryDtDao(ctx);
            TransRevenueSharingPaymentHdDao oTransRevenueSharingPaymentHdDao = new TransRevenueSharingPaymentHdDao(ctx);
            RevenueSharingFeeHdDao oRevenueSharingFeeHdDao = new RevenueSharingFeeHdDao(ctx);

            try
            {
                if (type.Contains("justvoid"))
                {
                    #region TransRevenueSharingPayment

                    string[] param = type.Split(';');
                    string gcDeleteReason = param[1];
                    string reason = param[2];

                    TransRevenueSharingPaymentHd entity = oTransRevenueSharingPaymentHdDao.Get(Convert.ToInt32(hdnRSPaymentID.Value));

                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        string filterSRS = string.Format("RSSummaryID IN (SELECT RSSummaryID FROM TransRevenueSharingPaymentDt WHERE RSPaymentID = {0} AND IsDeleted = 0)", hdnRSPaymentID.Value);
                        List<TransRevenueSharingSummaryHd> lstInvoiceHd = BusinessLayer.GetTransRevenueSharingSummaryHdList(filterSRS, ctx);
                        foreach (TransRevenueSharingSummaryHd invoice in lstInvoiceHd)
                        {
                            TransRevenueSharingSummaryHd rsSummaryHd = oTransRevenueSharingSummaryHDDao.Get(invoice.RSSummaryID);
                            if (rsSummaryHd.GCTransactionStatus == Constant.TransactionStatus.PROCESSED && result == true)
                            {
                                rsSummaryHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                rsSummaryHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                oTransRevenueSharingSummaryHDDao.Update(rsSummaryHd);
                            }
                            else
                            {
                                errMessage = "Transaksi rekap jasa medis tidak dapat diubah. Harap refresh halaman ini.";
                                result = false;
                            }
                        }

                        string filterFee = string.Format("RSPaymentID = {0}", hdnRSPaymentID.Value);
                        List<RevenueSharingFeeHd> lstSharingFeeHd = BusinessLayer.GetRevenueSharingFeeHdList(filterFee, ctx);
                        foreach (RevenueSharingFeeHd sharingFee in lstSharingFeeHd)
                        {
                            RevenueSharingFeeHd rsSharingFeeHd = oRevenueSharingFeeHdDao.Get(sharingFee.RevenueSharingFeeID);

                            rsSharingFeeHd.isPayment = false;
                            rsSharingFeeHd.RSPaymentID = null;
                            rsSharingFeeHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            oRevenueSharingFeeHdDao.Update(rsSharingFeeHd);
                        }

                        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entity.GCVoidReason = gcDeleteReason;
                        entity.VoidReason = reason;
                        entity.VoidBy = AppSession.UserLogin.UserID;
                        entity.VoidDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entity.LastUpdatedDate = DateTime.Now;
                        oTransRevenueSharingPaymentHdDao.Update(entity);

                        retval = entity.RSPaymentNo;
                        ctx.CommitTransaction();
                        return true;
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Transaksi rekap jasa medis tidak dapat diubah. Harap refresh halaman ini.");
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    #endregion
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