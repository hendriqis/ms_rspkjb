using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class JournalEntry : BasePageTrx
    {
        protected int minDate = -1;
        string menuType = string.Empty;

        public override string OnGetMenuCode()
        {
            if (menuType == "v2")
                return Constant.MenuCode.Accounting.JOURNAL_AUDITED_ENTRY;
            else if (menuType == "currentDate")
                return Constant.MenuCode.Accounting.JOURNAL_ENTRY_CURRENT_DATE;
            else
                return Constant.MenuCode.Accounting.JOURNAL_ENTRY;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        #region Html Getter
        protected string GetJournalGroupPendapatanPenerimaan()
        {
            return Constant.JournalGroup.PENDAPATAN_PENERIMAAN;
        }
        protected string GetJournalGroupHutangPiutang()
        {
            return Constant.JournalGroup.HUTANG_PIUTANG;
        }
        protected string GetJournalGroupInventory()
        {
            return Constant.JournalGroup.INVENTORY;
        }
        protected string GetJournalGroupMemorial()
        {
            return Constant.JournalGroup.MEMORIAL;
        }
        #endregion

        public Int32 GetDisplayCount()
        {
            return Convert.ToInt32(hdnDisplayCount.Value) - 1;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                menuType = Page.Request.QueryString["id"];
                hdnMenuType.Value = menuType;
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnUserLoginID.Value = AppSession.UserLogin.UserID.ToString();

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoidByReason.Value = CRUDMode.Contains("X") ? "1" : "0";

            vGLTransactionHd entity = BusinessLayer.GetvGLTransactionHd(string.Format(
                    "TransactionCode = '{0}' AND GCTransactionStatus != '{1}'", Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR, Constant.TransactionStatus.VOID), 0, "JournalDate DESC");
            if (entity != null)
            {
                hdnLastPostingDate.Value = entity.JournalDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                minDate = (DateTime.Now - entity.JournalDate).Days - 1;
            }

            Helper.SetControlEntrySetting(hdnGLAccountID, new ControlEntrySetting(true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnSearchDialogTypeName, new ControlEntrySetting(true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnSubLedgerID, new ControlEntrySetting(true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtGLAccountCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtGLAccountName, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(lblSubLedgerDt, new ControlEntrySetting(false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnSubLedgerDtID, new ControlEntrySetting(true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtSubLedgerDtCode, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtSubLedgerDtName, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboHealthcare, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            List<TransactionType> lstTransactionType = BusinessLayer.GetTransactionTypeList("TransactionCode BETWEEN '7281' AND '7289'");
            Methods.SetComboBoxField<TransactionType>(cboTransactionCode, lstTransactionType, "TransactionName", "TransactionCode");

            List<Healthcare> lstH = BusinessLayer.GetHealthcareList("GLAccountNoSegment IS NOT NULL");
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, lstH, "HealthcareName", "HealthcareID");
            cboHealthcare.SelectedIndex = 0;

            #region Binding from HealthcareParameter

            List<HealthcareParameter> lstSerVarDt = BusinessLayer.GetHealthcareParameterList(string.Format(
                                                                "ParameterCode IN ('{0}','{1}','{2}','{3}','{4}')",
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_REVENUE_COST_CENTER,
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_DEPARTMENT,
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_SERVICE_UNIT,
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_CUSTOMER_GROUP,
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_BUSINESS_PARTNER
                                                            )
                                                        );
            hdnHealthcare.Value = AppSession.UserLogin.HealthcareID;
            hdnDepartmentID.Value = lstSerVarDt.Where(a => a.ParameterCode == Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_DEPARTMENT).FirstOrDefault().ParameterValue;
            hdnServiceUnitID.Value = lstSerVarDt.Where(a => a.ParameterCode == Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_SERVICE_UNIT).FirstOrDefault().ParameterValue;
            hdnRevenueCostCenterID.Value = lstSerVarDt.Where(a => a.ParameterCode == Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_REVENUE_COST_CENTER).FirstOrDefault().ParameterValue;
            hdnCustomerGroupID.Value = lstSerVarDt.Where(a => a.ParameterCode == Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_CUSTOMER_GROUP).FirstOrDefault().ParameterValue;
            hdnBusinessPartnerID.Value = lstSerVarDt.Where(a => a.ParameterCode == Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_BUSINESS_PARTNER).FirstOrDefault().ParameterValue;

            if (hdnDepartmentID.Value != null && hdnDepartmentID.Value != "" && hdnDepartmentID.Value != "0")
            {
                Department oDepartment = BusinessLayer.GetDepartment(hdnDepartmentID.Value);
                if (oDepartment != null)
                {
                    txtDepartmentID.Text = oDepartment.DepartmentID;
                    txtDepartmentName.Text = oDepartment.DepartmentName;
                }
            }

            if (hdnServiceUnitID.Value != null && hdnServiceUnitID.Value != "" && hdnServiceUnitID.Value != "0")
            {
                ServiceUnitMaster oServiceUnitMaster = BusinessLayer.GetServiceUnitMaster(Convert.ToInt32(hdnServiceUnitID.Value));
                if (oServiceUnitMaster != null)
                {
                    txtServiceUnitCode.Text = oServiceUnitMaster.ServiceUnitCode;
                    txtServiceUnitName.Text = oServiceUnitMaster.ServiceUnitName;
                }
            }

            if (hdnRevenueCostCenterID.Value != null && hdnRevenueCostCenterID.Value != "" && hdnRevenueCostCenterID.Value != "0")
            {
                RevenueCostCenter oRevenueCostCenter = BusinessLayer.GetRevenueCostCenter(Convert.ToInt32(hdnRevenueCostCenterID.Value));
                if (oRevenueCostCenter != null)
                {
                    txtRevenueCostCenterCode.Text = oRevenueCostCenter.RevenueCostCenterCode;
                    txtRevenueCostCenterName.Text = oRevenueCostCenter.RevenueCostCenterName;
                }
            }

            if (hdnCustomerGroupID.Value != null && hdnCustomerGroupID.Value != "" && hdnCustomerGroupID.Value != "0")
            {
                CustomerGroup oCustomerGroup = BusinessLayer.GetCustomerGroup(Convert.ToInt32(hdnCustomerGroupID.Value));
                if (oCustomerGroup != null)
                {
                    txtCustomerGroupCode.Text = oCustomerGroup.CustomerGroupCode;
                    txtCustomerGroupName.Text = oCustomerGroup.CustomerGroupName;
                }
            }

            if (hdnBusinessPartnerID.Value != null && hdnBusinessPartnerID.Value != "" && hdnBusinessPartnerID.Value != "0")
            {
                BusinessPartners oBusinessPartner = BusinessLayer.GetBusinessPartners(Convert.ToInt32(hdnBusinessPartnerID.Value));
                if (oBusinessPartner != null)
                {
                    txtBusinessPartnerCode.Text = oBusinessPartner.BusinessPartnerCode;
                    txtBusinessPartnerName.Text = oBusinessPartner.BusinessPartnerName;
                }
            }

            #endregion

            hdnFileName.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.AC_FILE_NAME_DOCUMENT_DOWNLOAD_GLTRANSDT).ParameterValue;

            decimal totalDebit = -1;
            decimal totalKredit = -1;
            decimal selisih = -1;
            BindGridView(ref totalDebit, ref totalKredit, ref selisih);
            hdnIsEditable.Value = "1";
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtJournalPrefix, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtJournalNo, new ControlEntrySetting(true, false, true));

            if (hdnMenuType.Value == "currentDate")
            {
                SetControlEntrySetting(txtJournalDate, new ControlEntrySetting(false, false, true, Constant.DefaultValueEntry.DATE_NOW));
            }
            else
            {
                SetControlEntrySetting(txtJournalDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
            }
            
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTransactionCode, new ControlEntrySetting(true, false, true));

            Helper.SetControlEntrySetting(hdnGLAccountID, new ControlEntrySetting(true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnSearchDialogTypeName, new ControlEntrySetting(true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnSubLedgerID, new ControlEntrySetting(true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtGLAccountCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtGLAccountName, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(lblSubLedgerDt, new ControlEntrySetting(false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnSubLedgerDtID, new ControlEntrySetting(true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtSubLedgerDtCode, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtSubLedgerDtName, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboHealthcare, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            Helper.SetControlEntrySetting(hdnDepartmentID, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDepartmentID, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDepartmentName, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnServiceUnitID, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnParamedicID, new ControlEntrySetting(true, true, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(true, true, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtParamedicName, new ControlEntrySetting(false, false, false), "mpTrxPopup");
        }

        protected override void SetControlProperties()
        {
        }

        #region Load Entity
        public override void OnAddRecord()
        {
            hdnID.Value = "0";
            hdnIsEditable.Value = "1";
            hdnDisplayCount.Value = "1";
            tdTransactionNoEdit.Style.Add("display", "none");
            tdTransactionNoAdd.Style.Remove("display");
        }

        public string GetGCTransactionStatusOpen()
        {
            return Constant.TransactionStatus.OPEN;
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        protected string IsUsedAsTreasury()
        {
            return hdnIsUsedAsTreasury.Value;
        }

        protected string GetFilterExpression()
        {
            string filterExpression = "TransactionCode BETWEEN '7281' AND '7300'";

            if (hdnMenuType.Value == "audit")
            {
                filterExpression += " AND IsAuditedJournal = 1";
            }
            else
            {
                filterExpression += " AND IsAuditedJournal = 0";
            }

            return filterExpression;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvGLTransactionHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vGLTransactionHd entity = BusinessLayer.GetvGLTransactionHd(filterExpression, PageIndex, "GLTransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvGLTransactionHdRowIndex(filterExpression, keyValue, "GLTransactionID DESC");
            vGLTransactionHd entity = BusinessLayer.GetvGLTransactionHd(filterExpression, PageIndex, "GLTransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
        }

        private void EntityToControl(vGLTransactionHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                hdnIsEditable.Value = "0";
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            else
            {
                hdnIsEditable.Value = "1";
            }
            tdTransactionNoAdd.Style.Add("display", "none");
            tdTransactionNoEdit.Style.Remove("display");
            hdnID.Value = entity.GLTransactionID.ToString();
            txtJournalNo.Text = entity.JournalNo;
            cboTransactionCode.Value = entity.TransactionCode;
            hdnTransactionStatus.Value = entity.GCTransactionStatus;
            txtTreasuryType.Text = entity.TreasuryType;
            txtTreasuryGroup.Text = entity.JournalGroup;

            string filterTreasury = string.Format("GLTransactionID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}' AND IsUsedAsTreasury = 1",
                                                    entity.GLTransactionID, Constant.TransactionStatus.VOID);
            vGLTransactionDt entityDt = BusinessLayer.GetvGLTransactionDtList(filterTreasury).FirstOrDefault();
            if (entityDt != null)
            {
                hdnGLAccountTreasuryID.Value = entityDt.GLAccount.ToString();
                txtGLAccountCodeTreasury.Text = entityDt.GLAccountNo;
                txtGLAccountNameTreasury.Text = entityDt.GLAccountName;
                hdnGLAccountTreasuryPosition.Value = entityDt.Position;
                hdnIsUsedAsTreasury.Value = entityDt.IsUsedAsTreasury ? "1" : "0";
            }

            txtJournalDate.Text = entity.JournalDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRemarks.Text = entity.Remarks;

            decimal totalDebet = -1;
            decimal totalKredit = -1;
            decimal selisih = -1;
            BindGridView(ref totalDebet, ref totalKredit, ref selisih);

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

        }

        private void BindGridView(ref decimal totalDebet, ref decimal totalKredit, ref decimal selisih)
        {
            string filterExpression = "1 = 0";
            if (hdnID.Value != "")
            {
                filterExpression = string.Format("GLTransactionID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder ASC",
                                            hdnID.Value, Constant.TransactionStatus.VOID);
            }

            List<vGLTransactionDt> lstEntity = BusinessLayer.GetvGLTransactionDtList(filterExpression);
            totalDebet = lstEntity.Where(x => x.Position == "D").Sum(x => x.DebitAmount);
            totalKredit = lstEntity.Where(x => x.Position == "K").Sum(x => x.CreditAmount);
            selisih = totalDebet - totalKredit;
            txtTotalDebit.Text = totalDebet.ToString(Constant.FormatString.NUMERIC_2);
            txtTotalKredit.Text = totalKredit.ToString(Constant.FormatString.NUMERIC_2);
            txtTotalSelisih.Text = selisih.ToString(Constant.FormatString.NUMERIC_2);
            hdnDisplayCount.Value = Convert.ToString(lstEntity.Count() + 1);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        #region Save Header
        public void SaveGLTransactionHd(IDbContext ctx, ref int GLTransactionID)
        {
            GLTransactionHdDao entityHdDao = new GLTransactionHdDao(ctx);
            if (hdnID.Value == "" || hdnID.Value == "0")
            {
                DateTime journalDate = Helper.GetDatePickerValue(Request.Form[txtJournalDate.UniqueID]);
                bool isAllowSave = true;
                if (hdnLastPostingDate.Value != "")
                {
                    DateTime lastPostingDate = Helper.GetDatePickerValue(hdnLastPostingDate.Value);
                    if (journalDate <= lastPostingDate)
                        isAllowSave = false;
                }

                if (isAllowSave)
                {
                    GLTransactionHd entityHd = new GLTransactionHd();
                    entityHd.JournalDate = Helper.GetDatePickerValue(Request.Form[txtJournalDate.UniqueID]);
                    entityHd.GCJournalGroup = Constant.JournalGroup.MEMORIAL;
                    entityHd.TransactionCode = cboTransactionCode.Value.ToString();

                    entityHd.Remarks = txtRemarks.Text;
                    entityHd.JournalNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.JournalDate, txtJournalPrefix.Text, ctx);
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                    if (hdnMenuType.Value == "audit")
                    {
                        entityHd.IsAuditedJournal = true;
                    }

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    GLTransactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                    hdnID.Value = GLTransactionID.ToString();
                }
                else
                {
                    GLTransactionID = 0;
                }
            }
            else
            {
                GLTransactionID = Convert.ToInt32(hdnID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);

            try
            {
                int OrderID = 0;
                SaveGLTransactionHd(ctx, ref OrderID);
                if (OrderID != 0)
                {
                    retval = OrderID.ToString();
                }
                else
                {
                    errMessage = "Journal Pada Periode ini Telah Diposting";
                    result = false;
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                errMessage = ex.Message;
                result = false;
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
            try
            {
                GLTransactionHd entityHd = BusinessLayer.GetGLTransactionHd(Convert.ToInt32(hdnID.Value));
                entityHd.JournalDate = Helper.GetDatePickerValue(Request.Form[txtJournalDate.UniqueID]);
                entityHd.GCJournalGroup = Constant.JournalGroup.MEMORIAL;
                entityHd.TransactionCode = cboTransactionCode.Value.ToString();
                entityHd.Remarks = txtRemarks.Text;
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateGLTransactionHd(entityHd);
                return true;
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionHdDao GLTransactionHdDao = new GLTransactionHdDao(ctx);
            GLTransactionDtDao GlTransactionDtDao = new GLTransactionDtDao(ctx);

            if (Convert.ToDecimal(Request.Form[txtTotalSelisih.UniqueID]) == 0)
            {
                try
                {
                    GLTransactionHd itemTransactionHd = GLTransactionHdDao.Get(Convert.ToInt32(hdnID.Value));
                    itemTransactionHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    itemTransactionHd.ApprovedBy = AppSession.UserLogin.UserID;
                    itemTransactionHd.ApprovedDate = DateTime.Now;
                    itemTransactionHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                 

                    string filterExpression = String.Format("GLTransactionID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", hdnID.Value, Constant.TransactionStatus.VOID);
                    List<GLTransactionDt> lstGLTransactionDt = BusinessLayer.GetGLTransactionDtList(filterExpression, ctx);
                    foreach (GLTransactionDt GlTransactionDt in lstGLTransactionDt)
                    {
                        GlTransactionDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                        GlTransactionDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        ctx.Command.CommandTimeout = 90;
                        GlTransactionDtDao.Update(GlTransactionDt);
                    }

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    ctx.Command.CommandTimeout = 90;
                    GLTransactionHdDao.Update(itemTransactionHd);

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
            else
            {
                errMessage = "Journal Tidak Seimbang";
                result = false;
                ctx.RollBackTransaction();
            }

            return result;
        }

        protected override bool OnProposeRecord(ref string errMessage)
        {
            if (Convert.ToDecimal(txtTotalSelisih.Text) == 0)
            {
                try
                {
                    GLTransactionHd entity = BusinessLayer.GetGLTransactionHd(Convert.ToInt32(hdnID.Value));
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateGLTransactionHd(entity);
                    return true;
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    return false;
                }
            }
            else
            {
                errMessage = "Journal Tidak Seimbang";
                return false;
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionHdDao glTransactionHdDao = new GLTransactionHdDao(ctx);
            GLTransactionDtDao glTransactionDtDao = new GLTransactionDtDao(ctx);
            GLTransactionDtDownloadUploadTempDao documentTempDao = new GLTransactionDtDownloadUploadTempDao(ctx);

            try
            {
                GLTransactionHd entityHD = glTransactionHdDao.Get(Convert.ToInt32(hdnID.Value));
                if (entityHD.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (type.Contains("voidbyreason"))
                    {
                        #region Void by Reason

                        string[] param = type.Split(';');
                        string gcDeleteReason = param[1];
                        string reason = param[2];

                        entityHD.GCVoidReason = gcDeleteReason;
                        entityHD.VoidReason = reason;
                        entityHD.VoidBy = AppSession.UserLogin.UserID;
                        entityHD.VoidDate = DateTime.Now;
                        entityHD.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entityHD.LastUpdatedBy = AppSession.UserLogin.UserID;

                        List<GLTransactionDt> lstEntityDt = BusinessLayer.GetGLTransactionDtList(String.Format(
                                "GLTransactionID = {0} AND GCItemDetailStatus = '{1}' AND IsDeleted = 0", entityHD.GLTransactionID, Constant.TransactionStatus.OPEN), ctx);
                        foreach (GLTransactionDt entityDt in lstEntityDt)
                        {
                            entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            glTransactionDtDao.Update(entityDt);
                        }

                        glTransactionHdDao.Update(entityHD);
                        result = true;
                        ctx.CommitTransaction();

                        #endregion
                    }
                    else if (type == "download")
                    {
                        #region Download Document

                        string reportCode = string.Format("ReportCode = '{0}'", "AC-00011");
                        ReportMaster rm = BusinessLayer.GetReportMasterList(reportCode).FirstOrDefault();

                        StringBuilder sbResult = new StringBuilder();

                        List<dynamic> lstDynamic = null;
                        List<Variable> lstVariable = new List<Variable>();

                        lstVariable.Add(new Variable { Code = "DownloadedBy", Value = AppSession.UserLogin.UserID.ToString() });

                        lstDynamic = BusinessLayer.GetDataReport(rm.ObjectTypeName, lstVariable);

                        dynamic fields = lstDynamic[0];

                        foreach (var prop in fields.GetType().GetProperties())
                        {
                            sbResult.Append(prop.Name);
                            sbResult.Append(",");
                        }

                        sbResult.Append("\r\n");

                        for (int i = 0; i < lstDynamic.Count; ++i)
                        {
                            dynamic entity = lstDynamic[i];

                            foreach (var prop in entity.GetType().GetProperties())
                            {
                                sbResult.Append(prop.GetValue(entity, null).ToString().Replace(',', '_'));
                                sbResult.Append(",");
                            }

                            sbResult.Append("\r\n");
                        }

                        retval = sbResult.ToString();

                        ctx.RollBackTransaction();

                        #endregion
                    }
                    else if (type == "upload")
                    {
                        #region Upload Document

                        string fileUpload = hdnUploadedFile.Value;
                        if (fileUpload != "")
                        {
                            string[] parts = Regex.Split(fileUpload, ",").Skip(1).ToArray();
                            fileUpload = String.Join(",", parts);
                        }

                        string path = AppConfigManager.QISPhysicalDirectory;
                        path += string.Format("{0}\\", AppConfigManager.QISFinanceUploadDocument.Replace('/', '\\'));

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        else
                        {
                            Directory.Delete(path, true);
                            Directory.CreateDirectory(path);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<GLTransactionDtDownloadUploadTemp> documentTempList = BusinessLayer.GetGLTransactionDtDownloadUploadTempList(string.Format("GLTransactionID = {0}", hdnID.Value), ctx);
                            foreach (GLTransactionDtDownloadUploadTemp documentTemp in documentTempList)
                            {
                                documentTempDao.Delete();
                            }
                        }

                        FileStream fs = new FileStream(string.Format("{0}{1}", path, hdnFileName.Value), FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);
                        byte[] data = Convert.FromBase64String(fileUpload);
                        bw.Write(data);
                        bw.Close();

                        string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, hdnFileName.Value));

                        int rowCount = 0;
                        foreach (string temp in lstTemp)
                        {
                            if (rowCount != 0)
                            {
                                if (temp.Contains(','))
                                {
                                    List<String> fieldTemp = temp.Split(',').ToList();

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    GLTransactionDtDownloadUploadTemp oData = new GLTransactionDtDownloadUploadTemp();
                                    oData.GLTransactionID = Convert.ToInt32(hdnID.Value);
                                    oData.GLAccountNo = fieldTemp[0] == "" ? null : fieldTemp[0];
                                    oData.DepartmentID = fieldTemp[1] == "" ? null : fieldTemp[1];
                                    oData.ServiceUnitCode = fieldTemp[2] == "" ? null : fieldTemp[2];
                                    oData.RevenueCostCenterCode = fieldTemp[3] == "" ? null : fieldTemp[3];
                                    oData.BusinessPartnerCode = fieldTemp[4] == "" ? null : fieldTemp[4];
                                    oData.MedicalNo = fieldTemp[5] == "" ? null : fieldTemp[5];
                                    oData.ParamedicCode = fieldTemp[6] == "" ? null : fieldTemp[6];
                                    oData.DebitAmount = Convert.ToDecimal(fieldTemp[7]);
                                    oData.CreditAmount = Convert.ToDecimal(fieldTemp[8]);
                                    oData.Remarks = fieldTemp[9] == "" ? null : fieldTemp[9];
                                    oData.ReferenceNo = fieldTemp[10] == "" ? null : fieldTemp[10];
                                    oData.UploadedBy = AppSession.UserLogin.UserID;
                                    oData.UploadedDate = DateTime.Now;
                                    documentTempDao.Insert(oData);

                                    Int32 displayOrder = 0;
                                    string filterExisting = string.Format("GLTransactionID = {0}", oData.GLTransactionID);
                                    List<GLTransactionDt> lstTransDt = BusinessLayer.GetGLTransactionDtList(filterExisting, ctx);
                                    if (lstTransDt.Count() > 0)
                                    {
                                        displayOrder = lstTransDt.LastOrDefault().DisplayOrder;
                                    }

                                    if (oData.GLTransactionID != null && oData.GLTransactionID != 0)
                                    {
                                        displayOrder += 1;
                                        GLTransactionDt entityDt = new GLTransactionDt();
                                        entityDt.GLTransactionID = oData.GLTransactionID;
                                        entityDt.HealthcareID = AppSession.UserLogin.HealthcareID;

                                        if (oData.GLAccountNo != null)
                                        {
                                            string filter = string.Format("GLAccountNo = '{0}' AND IsDeleted = 0 AND IsActive = 1", oData.GLAccountNo);
                                            List<ChartOfAccount> lst = BusinessLayer.GetChartOfAccountList(filter, ctx);
                                            entityDt.GLAccount = lst.FirstOrDefault().GLAccountID;
                                        }

                                        entityDt.DepartmentID = oData.DepartmentID;

                                        if (oData.ServiceUnitCode != null)
                                        {
                                            string filter = string.Format("ServiceUnitCode = '{0}' AND IsDeleted = 0", oData.ServiceUnitCode);
                                            List<ServiceUnitMaster> lst = BusinessLayer.GetServiceUnitMasterList(filter, ctx);
                                            if (lst.Count() > 0)
                                            {
                                                entityDt.ServiceUnitID = lst.FirstOrDefault().ServiceUnitID;
                                            }
                                        }

                                        if (oData.RevenueCostCenterCode != null)
                                        {
                                            string filter = string.Format("RevenueCostCenterCode = '{0}' AND IsDeleted = 0", oData.RevenueCostCenterCode);
                                            List<RevenueCostCenter> lst = BusinessLayer.GetRevenueCostCenterList(filter, ctx);
                                            if (lst.Count() > 0)
                                            {
                                                entityDt.RevenueCostCenterID = lst.FirstOrDefault().RevenueCostCenterID;
                                            }
                                        }

                                        if (oData.BusinessPartnerCode != null)
                                        {
                                            string filter = string.Format("BusinessPartnerCode = '{0}' AND IsDeleted = 0", oData.BusinessPartnerCode);
                                            List<BusinessPartners> lst = BusinessLayer.GetBusinessPartnersList(filter, ctx);
                                            if (lst.Count() > 0)
                                            {
                                                entityDt.BusinessPartnerID = lst.FirstOrDefault().BusinessPartnerID;
                                            }
                                        }

                                        if (oData.MedicalNo != null)
                                        {
                                            string filter = string.Format("MedicalNo = '{0}' AND IsDeleted = 0", oData.BusinessPartnerCode);
                                            List<Patient> lst = BusinessLayer.GetPatientList(filter, ctx);
                                            if (lst.Count() > 0)
                                            {
                                                entityDt.MRN = lst.FirstOrDefault().MRN;
                                            }
                                        }

                                        if (oData.ParamedicCode != null)
                                        {
                                            string filter = string.Format("ParamedicCode = '{0}' AND IsDeleted = 0", oData.ParamedicCode);
                                            List<ParamedicMaster> lst = BusinessLayer.GetParamedicMasterList(filter, ctx);
                                            if (lst.Count() > 0)
                                            {
                                                entityDt.ParamedicID = lst.FirstOrDefault().ParamedicID;
                                            }
                                        }

                                        entityDt.DebitAmount = oData.DebitAmount;
                                        entityDt.CreditAmount = oData.CreditAmount;
                                        if (entityDt.DebitAmount != 0)
                                        {
                                            entityDt.Position = "D";
                                        }
                                        else
                                        {
                                            entityDt.Position = "K";
                                        }

                                        entityDt.DisplayOrder = displayOrder;

                                        entityDt.Remarks = oData.Remarks;
                                        entityDt.ReferenceNo = oData.ReferenceNo;

                                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                                        entityDt.IsDeleted = false;

                                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        glTransactionDtDao.Insert(entityDt);
                                    }
                                }
                            }
                            rowCount += 1;
                        }

                        ctx.CommitTransaction();

                        #endregion
                    }
                }
                else
                {
                    result = false;
                    errMessage = "GAGAL VOID : Jurnal sudah diproses";
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

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int GLTransactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    GLTransactionID = Convert.ToInt32(hdnID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref GLTransactionID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                GLTransactionID = Convert.ToInt32(hdnID.Value);
                if (OnDeleteEntityDt(ref errMessage, GLTransactionID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpGLTransactionID"] = GLTransactionID.ToString();
        }

        private void ControlToEntity(GLTransactionDt entityDt)
        {
            entityDt.GLAccount = Convert.ToInt32(hdnGLAccountID.Value);
            if (hdnSubLedgerDtID.Value != "" && hdnSubLedgerDtID.Value != "0")
                entityDt.SubLedger = Convert.ToInt32(hdnSubLedgerDtID.Value);
            else
                entityDt.SubLedger = null;

            entityDt.HealthcareID = hdnHealthcare.Value;
            entityDt.DepartmentID = hdnDepartmentID.Value;

            if (hdnServiceUnitID.Value != "" && hdnServiceUnitID.Value != "0")
            {
                entityDt.ServiceUnitID = Convert.ToInt32(hdnServiceUnitID.Value);
            }
            else
            {
                entityDt.ServiceUnitID = null;
            }

            if (hdnRevenueCostCenterID.Value != "" && hdnRevenueCostCenterID.Value != "0")
            {
                entityDt.RevenueCostCenterID = Convert.ToInt32(hdnRevenueCostCenterID.Value);
            }
            else
            {
                entityDt.RevenueCostCenterID = null;
            }

            if (hdnCustomerGroupID.Value != "" && hdnCustomerGroupID.Value != "0")
            {
                entityDt.CustomerGroupID = Convert.ToInt32(hdnCustomerGroupID.Value);
            }
            else
            {
                entityDt.CustomerGroupID = null;
            }

            if (hdnBusinessPartnerID.Value != "" && hdnBusinessPartnerID.Value != "0")
            {
                entityDt.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerID.Value);
            }
            else
            {
                entityDt.BusinessPartnerID = null;
            }

            if (hdnMRN.Value != "" && hdnMRN.Value != "0")
            {
                entityDt.MRN = Convert.ToInt32(hdnMRN.Value);
            }
            else
            {
                entityDt.MRN = null;
            }

            if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
            {
                entityDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            }
            else
            {
                entityDt.ParamedicID = null;
            }

            Decimal debit = Convert.ToDecimal(txtAmountD.Text);
            Decimal kredit = Convert.ToDecimal(txtAmountK.Text);
            if (debit != 0)
            {
                entityDt.Position = "D";
                entityDt.DebitAmount = debit;
                entityDt.CreditAmount = 0;
            }
            else
            {
                entityDt.Position = "K";
                entityDt.CreditAmount = kredit;
                entityDt.DebitAmount = 0;
            }
            entityDt.ReferenceNo = txtReferenceNo.Text;
            if (txtDisplayOrder.Text != "")
                entityDt.DisplayOrder = Convert.ToInt16(txtDisplayOrder.Text);
            else
                entityDt.DisplayOrder = 0;

            entityDt.Remarks = txtRemarksDt.Text;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int GLTransactionID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionDtDao entityDtDao = new GLTransactionDtDao(ctx);
            try
            {
                DateTime journalDate = Helper.GetDatePickerValue(Request.Form[txtJournalDate.UniqueID]);
                bool isAllowSave = true;
                if (hdnLastPostingDate.Value != "")
                {
                    DateTime lastPostingDate = Helper.GetDatePickerValue(hdnLastPostingDate.Value);
                    if (journalDate <= lastPostingDate)
                        isAllowSave = false;
                }

                if (isAllowSave)
                {
                    SaveGLTransactionHd(ctx, ref GLTransactionID);
                    if (GLTransactionID != 0)
                    {
                        GLTransactionDt entityDt = new GLTransactionDt();
                        ControlToEntity(entityDt);
                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        entityDt.GLTransactionID = GLTransactionID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Journal Pada Periode ini Telah Diposting";
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionDtDao entityDtDao = new GLTransactionDtDao(ctx);
            try
            {
                GLTransactionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entityDt);
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
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

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionDtDao entityDtDao = new GLTransactionDtDao(ctx);
            try
            {
                GLTransactionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityDt.GLTransactionIDSource != null && entityDt.GLTransactionIDSource != 0)
                {
                    string filterGLDTDelete = string.Format("GLTransactionID = {0} AND IsDeleted = 0 AND GLTransactionIDSource = {1}", hdnID.Value, entityDt.GLTransactionIDSource);
                    List<GLTransactionDt> gldtList = BusinessLayer.GetGLTransactionDtList(filterGLDTDelete);
                    foreach (GLTransactionDt gldt in gldtList)
                    {
                        gldt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                        gldt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        gldt.IsDeleted = true;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Update(gldt);
                    }
                }
                else
                {
                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDt.IsDeleted = true;
                    entityDtDao.Update(entityDt);
                }
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
        #endregion

        #region Callback
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            decimal totalDebit = 0;
            decimal totalKredit = 0;
            decimal selisih = 0;
            string result = "";
            BindGridView(ref totalDebit, ref totalKredit, ref selisih);
            result = string.Format("refresh|{0}|{1}|{2}", totalDebit, totalKredit, selisih);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}