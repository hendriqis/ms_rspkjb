using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RequestRealizationCashReceiptEntry : BasePageTrx
    {
        string menuType = string.Empty;

        public override string OnGetMenuCode()
        {
            if (menuType == "currentDate")
                return Constant.MenuCode.Finance.FN_REQUEST_REALIZATION_CASH_RECEIPT_CURRENT_DATE;
            else
                return Constant.MenuCode.Finance.FN_REQUEST_REALIZATION_CASH_RECEIPT;
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

        protected int minDate = -1;
        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                menuType = Page.Request.QueryString["id"];
                hdnMenuType.Value = menuType;
            }

            MenuMaster oMenu = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            hdnPageTitle.Value = oMenu.MenuCaption;
            hdnIsAllowVoidByReason.Value = oMenu.CRUDMode.Contains("X") ? "1" : "0";

            GLTransactionHd entity = BusinessLayer.GetGLTransactionHd(string.Format(
                                        "TransactionCode = '{0}' AND GCTransactionStatus != '{1}'",
                                        Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR, Constant.TransactionStatus.VOID
                                        ), 0, "JournalDate DESC"
                                    );
            if (entity != null)
            {
                hdnLastPostingDate.Value = entity.JournalDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                minDate = (DateTime.Now - entity.JournalDate).Days - 1;
            }

            List<TransactionType> lstTransactionType = BusinessLayer.GetTransactionTypeList("TransactionCode BETWEEN '7281' AND '7288'");
            Methods.SetComboBoxField<TransactionType>(cboTransactionCode, lstTransactionType, "TransactionName", "TransactionCode");

            List<StandardCode> lstTreasuryType = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND TagProperty = '2'", Constant.StandardCode.TREASURY_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboTreasuryType, lstTreasuryType, "StandardCodeName", "StandardCodeID");
            cboTreasuryType.SelectedIndex = 0;
            hdnTreasuryType.Value = "";

            List<StandardCode> lstTreasuryGroup = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND TagProperty = '2'", Constant.StandardCode.TREASURY_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboTreasuryGroup, lstTreasuryGroup, "StandardCodeName", "StandardCodeID");
            cboTreasuryGroup.SelectedIndex = 0;
            hdnTreasuryGroup.Value = "";

            List<Healthcare> lstH = BusinessLayer.GetHealthcareList("GLAccountNoSegment IS NOT NULL");
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, lstH, "HealthcareName", "HealthcareID");
            cboHealthcare.SelectedIndex = 0;

            List<Variable> lstPosition = new List<Variable>();
            lstPosition.Add(new Variable { Code = "D", Value = "Debet" });
            lstPosition.Add(new Variable { Code = "K", Value = "Kredit" });
            Methods.SetComboBoxField<Variable>(cboPosition, lstPosition, "Value", "Code");
            cboPosition.SelectedIndex = 0;

            #region Binding from HealthcareParameter

            List<HealthcareParameter> lstHealthcareParameter = BusinessLayer.GetHealthcareParameterList(string.Format(
                                                                "ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_REVENUE_COST_CENTER,
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_DEPARTMENT,
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_SERVICE_UNIT,
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_CUSTOMER_GROUP,
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_BUSINESS_PARTNER,
                                                                Constant.HealthcareParameter.AC_COA_DIRECT_PURCHASE,
                                                                Constant.HealthcareParameter.AC_COA_PERMINTAAN_SPK,
                                                                Constant.HealthcareParameter.AC_COA_REALISASI_SPK,
                                                                Constant.HealthcareParameter.AC_COA_KAS_BON
                                                            )
                                                        );

            hdnHealthcare.Value = AppSession.UserLogin.HealthcareID;

            hdnDefaultDepartmentID.Value = lstHealthcareParameter.Where(a => a.ParameterCode == Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_DEPARTMENT).FirstOrDefault().ParameterValue;
            if (hdnDefaultDepartmentID.Value != null && hdnDefaultDepartmentID.Value != "" && hdnDefaultDepartmentID.Value != "0")
            {
                Department oDepartment = BusinessLayer.GetDepartment(hdnDefaultDepartmentID.Value);
                hdnDepartmentID.Value = txtDepartmentID.Text = oDepartment.DepartmentID;
                hdnDefaultDepartmentName.Value = txtDepartmentName.Text = oDepartment.DepartmentName;
            }
            else
            {
                hdnDefaultDepartmentID.Value = hdnDepartmentID.Value = "";
                hdnDefaultDepartmentName.Value = txtDepartmentName.Text = "";
            }

            hdnDefaultServiceUnitID.Value = lstHealthcareParameter.Where(a => a.ParameterCode == Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_SERVICE_UNIT).FirstOrDefault().ParameterValue;
            if (hdnDefaultServiceUnitID.Value != null && hdnDefaultServiceUnitID.Value != "" && hdnDefaultServiceUnitID.Value != "0")
            {
                ServiceUnitMaster oServiceUnitMaster = BusinessLayer.GetServiceUnitMaster(Convert.ToInt32(hdnDefaultServiceUnitID.Value));
                hdnServiceUnitID.Value = oServiceUnitMaster.ServiceUnitID.ToString();
                hdnDefaultServiceUnitCode.Value = txtServiceUnitCode.Text = oServiceUnitMaster.ServiceUnitCode;
                hdnDefaultServiceUnitName.Value = txtServiceUnitName.Text = oServiceUnitMaster.ServiceUnitName;
            }
            else
            {
                hdnDefaultServiceUnitID.Value = hdnServiceUnitID.Value = "";
                hdnDefaultServiceUnitCode.Value = txtServiceUnitCode.Text = "";
                hdnDefaultServiceUnitName.Value = txtServiceUnitName.Text = "";
            }

            hdnDefaultRevenueCostCenterID.Value = lstHealthcareParameter.Where(a => a.ParameterCode == Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_REVENUE_COST_CENTER).FirstOrDefault().ParameterValue;
            if (hdnDefaultRevenueCostCenterID.Value != null && hdnDefaultRevenueCostCenterID.Value != "" && hdnDefaultRevenueCostCenterID.Value != "0")
            {
                RevenueCostCenter oRevenueCostCenter = BusinessLayer.GetRevenueCostCenter(Convert.ToInt32(hdnDefaultRevenueCostCenterID.Value));
                hdnRevenueCostCenterID.Value = oRevenueCostCenter.RevenueCostCenterID.ToString();
                hdnDefaultRevenueCostCenterCode.Value = txtRevenueCostCenterCode.Text = oRevenueCostCenter.RevenueCostCenterCode;
                hdnDefaultRevenueCostCenterName.Value = txtRevenueCostCenterName.Text = oRevenueCostCenter.RevenueCostCenterName;
            }
            else
            {
                hdnDefaultRevenueCostCenterID.Value = hdnRevenueCostCenterID.Value = "";
                hdnDefaultRevenueCostCenterCode.Value = txtRevenueCostCenterCode.Text = "";
                hdnDefaultRevenueCostCenterName.Value = txtRevenueCostCenterName.Text = "";
            }

            hdnDefaultCustomerGroupID.Value = lstHealthcareParameter.Where(a => a.ParameterCode == Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_CUSTOMER_GROUP).FirstOrDefault().ParameterValue;
            if (hdnDefaultCustomerGroupID.Value != null && hdnDefaultCustomerGroupID.Value != "" && hdnDefaultCustomerGroupID.Value != "0")
            {
                CustomerGroup oCustomerGroup = BusinessLayer.GetCustomerGroup(Convert.ToInt32(hdnDefaultCustomerGroupID.Value));
                hdnCustomerGroupID.Value = oCustomerGroup.CustomerGroupID.ToString();
                hdnDefaultCustomerGroupCode.Value = txtCustomerGroupCode.Text = oCustomerGroup.CustomerGroupCode;
                hdnDefaultCustomerGroupName.Value = txtCustomerGroupName.Text = oCustomerGroup.CustomerGroupName;
            }
            else
            {
                hdnDefaultCustomerGroupID.Value = hdnCustomerGroupID.Value = "";
                hdnDefaultCustomerGroupCode.Value = txtCustomerGroupCode.Text = "";
                hdnDefaultCustomerGroupName.Value = txtCustomerGroupName.Text = "";
            }

            hdnDefaultBusinessPartnerID.Value = lstHealthcareParameter.Where(a => a.ParameterCode == Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_BUSINESS_PARTNER).FirstOrDefault().ParameterValue;
            if (hdnDefaultBusinessPartnerID.Value != null && hdnDefaultBusinessPartnerID.Value != "" && hdnDefaultBusinessPartnerID.Value != "0")
            {
                BusinessPartners entityBP = BusinessLayer.GetBusinessPartners(Convert.ToInt32(hdnDefaultBusinessPartnerID.Value));
                hdnDefaultBusinessPartnerCode.Value = entityBP.BusinessPartnerCode;
                hdnDefaultBusinessPartnerName.Value = entityBP.BusinessPartnerName;
            }
            else
            {
                hdnDefaultBusinessPartnerID.Value = "";
                hdnDefaultBusinessPartnerCode.Value = "";
                hdnDefaultBusinessPartnerName.Value = "";
            }

            hdnCOADirectPurchase.Value = lstHealthcareParameter.Find(a => a.ParameterCode == Constant.HealthcareParameter.AC_COA_DIRECT_PURCHASE).ParameterValue;
            hdnCOAPermintaanSPK.Value = lstHealthcareParameter.Find(a => a.ParameterCode == Constant.HealthcareParameter.AC_COA_PERMINTAAN_SPK).ParameterValue;
            hdnCOARealisasiSPK.Value = lstHealthcareParameter.Find(a => a.ParameterCode == Constant.HealthcareParameter.AC_COA_REALISASI_SPK).ParameterValue;
            hdnCOAKasBon.Value = lstHealthcareParameter.Find(a => a.ParameterCode == Constant.HealthcareParameter.AC_COA_KAS_BON).ParameterValue;

            #endregion

            decimal totalDebit = -1;
            decimal totalKredit = -1;
            decimal selisih = -1;
            BindGridView(ref totalDebit, ref totalKredit, ref selisih);

            hdnIsEditable.Value = "1";
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnTransactionStatus, new ControlEntrySetting(true, true, true));
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

            SetControlEntrySetting(cboTransactionCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboTreasuryType, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboTreasuryGroup, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, true));


            Helper.SetControlEntrySetting(hdnGLAccountID, new ControlEntrySetting(true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnSearchDialogTypeName, new ControlEntrySetting(true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnSubLedgerID, new ControlEntrySetting(true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtGLAccountCode, new ControlEntrySetting(true, true, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtGLAccountName, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(lblSubLedgerDt, new ControlEntrySetting(false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnSubLedgerDtID, new ControlEntrySetting(true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtSubLedgerDtCode, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtSubLedgerDtName, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboHealthcare, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            Helper.SetControlEntrySetting(hdnDepartmentID, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDepartmentID, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDepartmentName, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnServiceUnitID, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, true), "mpTrxPopup");
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

        protected string GetFilterExpression()
        {
            return "GCTreasuryGroup IS NOT NULL AND TransactionCode BETWEEN '7281' AND '7300' AND TreasuryTypeTagProperty = '2' AND TreasuryGroupTagProperty = '2'";
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

            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                txtRemarks.Enabled = true;
            }
            else
            {
                txtRemarks.Enabled = false;
            }

            tdTransactionNoAdd.Style.Add("display", "none");
            tdTransactionNoEdit.Style.Remove("display");
            hdnID.Value = entity.GLTransactionID.ToString();
            txtJournalNo.Text = entity.JournalNo;
            cboTransactionCode.Value = entity.TransactionCode;
            hdnTransactionStatus.Value = entity.GCTransactionStatus;
            cboTreasuryType.Value = entity.GCTreasuryType;
            hdnTreasuryType.Value = entity.GCTreasuryType;
            cboTreasuryGroup.Value = entity.GCTreasuryGroup;
            hdnTreasuryGroup.Value = entity.GCTreasuryGroup;

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

            List<vGLTransactionDtCashReceipt> lstEntity = BusinessLayer.GetvGLTransactionDtCashReceiptList(filterExpression);
            totalDebet = lstEntity.Where(x => x.Position == "D").Sum(x => x.DebitAmount);
            totalKredit = lstEntity.Where(x => x.Position == "K").Sum(x => x.CreditAmount);
            selisih = totalDebet - totalKredit;
            txtTotalDebit.Text = totalDebet.ToString("N");
            txtTotalKredit.Text = totalKredit.ToString("N");
            txtTotalSelisih.Text = selisih.ToString("N");
            hdnDisplayCount.Value = Convert.ToString(lstEntity.Count() + 1);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        #region Save Header
        public void SaveGLTransactionHd(IDbContext ctx, ref int GLTransactionID, ref string errorMessage)
        {
            GLTransactionHdDao entityHdDao = new GLTransactionHdDao(ctx);
            GLTransactionDtDao entityDtDao = new GLTransactionDtDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

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
                    if (cboTransactionCode.Value.ToString() == Constant.TransactionCode.JOURNAL_MEMORIAL_UMUM
                            || cboTransactionCode.Value.ToString() == Constant.TransactionCode.JOURNAL_MEMORIAL_KAS_KELUAR
                            || cboTransactionCode.Value.ToString() == Constant.TransactionCode.JOURNAL_MEMORIAL_KAS_MASUK
                            || cboTransactionCode.Value.ToString() == Constant.TransactionCode.JOURNAL_MEMORIAL_BANK_KELUAR
                            || cboTransactionCode.Value.ToString() == Constant.TransactionCode.JOURNAL_MEMORIAL_BANK_MASUK
                            || cboTransactionCode.Value.ToString() == Constant.TransactionCode.JOURNAL_MEMORIAL_PENYESUAIAN
                            || cboTransactionCode.Value.ToString() == Constant.TransactionCode.JOURNAL_MEMORIAL_PENYUSUTAN_AKTIVA
                            || cboTransactionCode.Value.ToString() == Constant.TransactionCode.JOURNAL_MEMORIAL_PEMUSNAHAN_AKTIVA
                            || cboTransactionCode.Value.ToString() == Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR)
                    {
                        GLTransactionHd entityHd = new GLTransactionHd();
                        entityHd.JournalDate = Helper.GetDatePickerValue(Request.Form[txtJournalDate.UniqueID]);
                        entityHd.GCJournalGroup = Constant.JournalGroup.MEMORIAL;
                        entityHd.TransactionCode = cboTransactionCode.Value.ToString();
                        entityHd.GCTreasuryType = cboTreasuryType.Value.ToString();
                        entityHd.GCTreasuryGroup = cboTreasuryGroup.Value.ToString();
                        entityHd.Remarks = txtRemarks.Text;
                        entityHd.JournalNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.JournalDate, txtJournalPrefix.Text, ctx);
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        GLTransactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                        hdnID.Value = GLTransactionID.ToString();

                    }
                    else
                    {
                        GLTransactionHd entityHd = new GLTransactionHd();
                        entityHd.JournalDate = Helper.GetDatePickerValue(Request.Form[txtJournalDate.UniqueID]);
                        entityHd.GCJournalGroup = Constant.JournalGroup.MEMORIAL;
                        entityHd.TransactionCode = cboTransactionCode.Value.ToString();
                        entityHd.GCTreasuryType = cboTreasuryType.Value.ToString();
                        entityHd.GCTreasuryGroup = cboTreasuryGroup.Value.ToString();
                        entityHd.Remarks = txtRemarks.Text;
                        entityHd.JournalNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.JournalDate, txtJournalPrefix.Text, ctx);
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        GLTransactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                        hdnID.Value = GLTransactionID.ToString();

                    }

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
            GLTransactionDtDao entityDtDao = new GLTransactionDtDao(ctx);

            try
            {
                int GLTransactionID = 0;
                string errorMessage = "";
                SaveGLTransactionHd(ctx, ref GLTransactionID, ref errorMessage);

                if (GLTransactionID != 0)
                {
                    retval = GLTransactionID.ToString();
                }
                else if (!String.IsNullOrEmpty(errorMessage))
                {
                    errMessage = errorMessage;
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
                else
                {
                    errMessage = "Journal Pada Periode ini Telah Diposting";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                GLTransactionHd entityHd = BusinessLayer.GetGLTransactionHd(Convert.ToInt32(hdnID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    bool isLock = false;
                    TransactionTypeLock entityLock = BusinessLayer.GetTransactionTypeLock(entityHd.TransactionCode);
                    if (entityLock.LockedUntilDate != null)
                    {
                        DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                        DateTime DateNow = entityHd.JournalDate;
                        if (DateNow > DateLock)
                        {
                            isLock = false;
                        }
                        else
                        {
                            isLock = true;
                        }
                    }
                    else
                    {
                        isLock = false;
                    }

                    if (!isLock)
                    {
                        if (entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_UMUM
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_KAS_KELUAR
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_KAS_MASUK
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_BANK_KELUAR
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_BANK_MASUK
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PENYESUAIAN
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PENYUSUTAN_AKTIVA
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PEMUSNAHAN_AKTIVA
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR)
                        {
                            entityHd.JournalDate = Helper.GetDatePickerValue(Request.Form[txtJournalDate.UniqueID]);
                            entityHd.GCJournalGroup = Constant.JournalGroup.MEMORIAL;
                            entityHd.GCTreasuryGroup = cboTreasuryGroup.Value.ToString();
                            entityHd.TransactionCode = cboTransactionCode.Value.ToString();
                            entityHd.Remarks = txtRemarks.Text;
                            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdateGLTransactionHd(entityHd);
                            retval = entityHd.JournalNo;
                            return true;
                        }
                        else
                        {
                            entityHd.JournalDate = Helper.GetDatePickerValue(Request.Form[txtJournalDate.UniqueID]);
                            entityHd.GCJournalGroup = Constant.JournalGroup.MEMORIAL;
                            entityHd.GCTreasuryGroup = cboTreasuryGroup.Value.ToString();
                            entityHd.TransactionCode = cboTransactionCode.Value.ToString();
                            entityHd.Remarks = txtRemarks.Text;
                            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdateGLTransactionHd(entityHd);
                            retval = entityHd.JournalNo;
                            return true;
                        }
                    }
                    else
                    {
                        errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
                }
                else
                {
                    errMessage = "GAGAL EDIT : Voucher sudah diproses";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        protected override bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            if (Convert.ToDecimal(Request.Form[txtTotalSelisih.UniqueID]) == 0)
            {
                IDbContext ctx = DbFactory.Configure(true);
                GLTransactionHdDao GLTransactionHdDao = new GLTransactionHdDao(ctx);
                GLTransactionDtDao GlTransactionDtDao = new GLTransactionDtDao(ctx);
                TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

                try
                {
                    GLTransactionHd itemTransactionHd = GLTransactionHdDao.Get(Convert.ToInt32(hdnID.Value));
                    if (itemTransactionHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        bool isLock = false;
                        TransactionTypeLock entityLock = entityLockDao.Get(itemTransactionHd.TransactionCode);
                        if (entityLock.LockedUntilDate != null)
                        {
                            DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                            DateTime DateNow = itemTransactionHd.JournalDate;
                            if (DateNow > DateLock)
                            {
                                isLock = false;
                            }
                            else
                            {
                                isLock = true;
                            }
                        }
                        else
                        {
                            isLock = false;
                        }

                        if (!isLock)
                        {
                            if (itemTransactionHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_UMUM
                                    || itemTransactionHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_KAS_KELUAR
                                    || itemTransactionHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_KAS_MASUK
                                    || itemTransactionHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_BANK_KELUAR
                                    || itemTransactionHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_BANK_MASUK
                                    || itemTransactionHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PENYESUAIAN
                                    || itemTransactionHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PENYUSUTAN_AKTIVA
                                    || itemTransactionHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PEMUSNAHAN_AKTIVA
                                    || itemTransactionHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR)
                            {
                                // APPROVE GLTransactionHd
                                itemTransactionHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                itemTransactionHd.ApprovedBy = AppSession.UserLogin.UserID;
                                itemTransactionHd.ApprovedDate = DateTime.Now;
                                itemTransactionHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                GLTransactionHdDao.Update(itemTransactionHd);

                                string filterExpression = String.Format("GLTransactionID = {0} AND GCItemDetailStatus != '{1}' ORDER BY DisplayOrder", hdnID.Value, Constant.TransactionStatus.VOID);
                                List<GLTransactionDt> lstGLTransactionDt = BusinessLayer.GetGLTransactionDtList(filterExpression, ctx);
                                foreach (GLTransactionDt GlTransactionDt in lstGLTransactionDt)
                                {
                                    // APPROVE GLTransactionDt
                                    GlTransactionDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                    GlTransactionDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    GlTransactionDtDao.Update(GlTransactionDt);

                                }
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                // APPROVE GLTransactionHd
                                itemTransactionHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                itemTransactionHd.ApprovedBy = AppSession.UserLogin.UserID;
                                itemTransactionHd.ApprovedDate = DateTime.Now;
                                itemTransactionHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                GLTransactionHdDao.Update(itemTransactionHd);

                                string filterExpression = String.Format("GLTransactionID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", hdnID.Value, Constant.TransactionStatus.VOID);
                                List<GLTransactionDt> lstGLTransactionDt = BusinessLayer.GetGLTransactionDtList(filterExpression, ctx);
                                foreach (GLTransactionDt GlTransactionDt in lstGLTransactionDt)
                                {
                                    // APPROVE GLTransactionDt
                                    GlTransactionDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                    GlTransactionDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    GlTransactionDtDao.Update(GlTransactionDt);

                                }
                                ctx.CommitTransaction();
                            }
                        }
                        else
                        {
                            errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            return false;
                        }
                    }
                    else
                    {
                        errMessage = "GAGAL APPROVE : Voucher sudah diproses";
                        result = false;
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
            }
            else
            {
                result = false;
                errMessage = "Voucher Tidak Seimbang";
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

                    bool isLock = false;
                    TransactionTypeLock entityLock = BusinessLayer.GetTransactionTypeLock(entity.TransactionCode);
                    if (entityLock.LockedUntilDate != null)
                    {
                        DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                        DateTime DateNow = entity.JournalDate;
                        if (DateNow > DateLock)
                        {
                            isLock = false;
                        }
                        else
                        {
                            isLock = true;
                        }
                    }
                    else
                    {
                        isLock = false;
                    }

                    if (!isLock)
                    {
                        if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            if (entity.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_UMUM
                                    || entity.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_KAS_KELUAR
                                    || entity.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_KAS_MASUK
                                    || entity.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_BANK_KELUAR
                                    || entity.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_BANK_MASUK
                                    || entity.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PENYESUAIAN
                                    || entity.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PENYUSUTAN_AKTIVA
                                    || entity.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PEMUSNAHAN_AKTIVA
                                    || entity.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR)
                            {
                                entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdateGLTransactionHd(entity);
                                return true;
                            }
                            else
                            {
                                entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdateGLTransactionHd(entity);
                                return true;
                            }
                        }
                        else
                        {
                            errMessage = "GAGAL PROPOSED : Voucher sudah diproses";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            return false;
                        }
                    }
                    else
                    {
                        errMessage = "GAGAL PROPOSED : Voucher sudah diproses";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    return false;
                }
            }
            else
            {
                errMessage = "Voucher Tidak Seimbang";
                return false;
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionHdDao glTransactionHdDao = new GLTransactionHdDao(ctx);
            GLTransactionDtDao glTransactionDtDao = new GLTransactionDtDao(ctx);
            PurchaseRequestHdDao purchaseRequestHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseReceiveHdDao purchaseReceiveHdDao = new PurchaseReceiveHdDao(ctx);
            DirectPurchaseHdDao directPurchaseHdDao = new DirectPurchaseHdDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                GLTransactionHd entityHD = glTransactionHdDao.Get(Convert.ToInt32(hdnID.Value));
                if (type.Contains("voidbyreason"))
                {
                    string[] param = type.Split(';');
                    string gcDeleteReason = param[1];
                    string reason = param[2];

                    if (entityHD.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        bool isLock = false;
                        TransactionTypeLock entityLock = entityLockDao.Get(entityHD.TransactionCode);
                        if (entityLock.LockedUntilDate != null)
                        {
                            DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                            DateTime DateNow = entityHD.JournalDate;
                            if (DateNow > DateLock)
                            {
                                isLock = false;
                            }
                            else
                            {
                                isLock = true;
                            }
                        }
                        else
                        {
                            isLock = false;
                        }

                        if (!isLock)
                        {
                            if (entityHD.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_UMUM
                                    || entityHD.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_KAS_KELUAR
                                    || entityHD.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_KAS_MASUK
                                    || entityHD.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_BANK_KELUAR
                                    || entityHD.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_BANK_MASUK
                                    || entityHD.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PENYESUAIAN
                                    || entityHD.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PENYUSUTAN_AKTIVA
                                    || entityHD.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PEMUSNAHAN_AKTIVA
                                    || entityHD.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR)
                            {
                                #region Jurnal Memorial

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

                                    string filterRequest = string.Format("GLTransactionIDRequest = {0} AND GLTransactionDtIDRequest = {1}", entityHD.GLTransactionID, entityDt.TransactionDtID);
                                    string filterRealization = string.Format("GLTransactionID = {0} AND GLTransactionDtID = {1}", entityHD.GLTransactionID, entityDt.TransactionDtID);

                                    if (entityHD.GCTreasuryType == Constant.TreasuryType.PERMINTAAN_KAS_BON && entityHD.GCTreasuryGroup == Constant.TreasuryGroup.SURAT_PERINTAH_KERJA)
                                    {
                                        List<PurchaseRequestHd> lstPurchaseRequestHd = BusinessLayer.GetPurchaseRequestHdList(filterRequest, ctx);
                                        foreach (PurchaseRequestHd purchaseRequestHd in lstPurchaseRequestHd)
                                        {
                                            purchaseRequestHd.GLTransactionIDRequest = null;
                                            purchaseRequestHd.GLTransactionDtIDRequest = null;
                                            purchaseRequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            purchaseRequestHdDao.Update(purchaseRequestHd);
                                        }
                                    }

                                    if (entityHD.GCTreasuryType == Constant.TreasuryType.REALISASI_KAS_BON && entityHD.GCTreasuryGroup == Constant.TreasuryGroup.SURAT_PERINTAH_KERJA)
                                    {
                                        List<PurchaseReceiveHd> lstPurchaseReceiveHd = BusinessLayer.GetPurchaseReceiveHdList(filterRealization, ctx);
                                        foreach (PurchaseReceiveHd purchaseReceiveHd in lstPurchaseReceiveHd)
                                        {
                                            purchaseReceiveHd.GLTransactionID = null;
                                            purchaseReceiveHd.GLTransactionDtID = null;
                                            purchaseReceiveHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            purchaseReceiveHdDao.Update(purchaseReceiveHd);
                                        }
                                    }

                                    if (entityHD.GCTreasuryType == Constant.TreasuryType.PERMINTAAN_KAS_BON && entityHD.GCTreasuryGroup == Constant.TreasuryGroup.PERMINTAAN_PEMBELIAN_TUNAI)
                                    {
                                        List<DirectPurchaseHd> lstDirectPurchaseHd = BusinessLayer.GetDirectPurchaseHdList(filterRequest, ctx);
                                        foreach (DirectPurchaseHd directPurchaseHd in lstDirectPurchaseHd)
                                        {
                                            directPurchaseHd.GLTransactionIDRequest = null;
                                            directPurchaseHd.GLTransactionDtIDRequest = null;
                                            directPurchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            directPurchaseHdDao.Update(directPurchaseHd);
                                        }
                                    }

                                    if (entityHD.GCTreasuryType == Constant.TreasuryType.REALISASI_KAS_BON && entityHD.GCTreasuryGroup == Constant.TreasuryGroup.REALISASI_PEMBELIAN_TUNAI)
                                    {
                                        List<DirectPurchaseHd> lstDirectPurchaseHd = BusinessLayer.GetDirectPurchaseHdList(filterRealization, ctx);
                                        foreach (DirectPurchaseHd directPurchaseHd in lstDirectPurchaseHd)
                                        {
                                            directPurchaseHd.GLTransactionID = null;
                                            directPurchaseHd.GLTransactionDtID = null;
                                            directPurchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            directPurchaseHdDao.Update(directPurchaseHd);
                                        }
                                    }
                                }

                                glTransactionHdDao.Update(entityHD);
                                retval = entityHD.JournalNo;
                                result = true;
                                ctx.CommitTransaction();

                                #endregion
                            }
                        }
                        else
                        {
                            errMessage = "GAGAL APPROVE : Voucher sudah diproses";
                            result = false;
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "GAGAL VOID : Voucher sudah diproses";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
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
            if (hdnGLAccountID.Value != null && hdnGLAccountID.Value != "" && hdnGLAccountID.Value != "0")
            {
                entityDt.GLAccount = Convert.ToInt32(hdnGLAccountID.Value);
            }
            else
            {
                entityDt.GLAccount = null;
            }

            if (hdnSubLedgerDtID.Value != "" && hdnSubLedgerDtID.Value != "0")
            {
                entityDt.SubLedger = Convert.ToInt32(hdnSubLedgerDtID.Value);
            }
            else
            {
                entityDt.SubLedger = null;
            }

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

            Decimal voucher = Convert.ToDecimal(txtVoucherAmount.Text);

            if (cboPosition.Value.ToString() == "K")
            {
                entityDt.Position = "K";
                entityDt.DebitAmount = 0;
                entityDt.CreditAmount = voucher;
            }
            else
            {
                entityDt.Position = "D";
                entityDt.DebitAmount = voucher;
                entityDt.CreditAmount = 0;
            }

            entityDt.ReferenceNo = txtReferenceNo.Text;

            if (txtDisplayOrder.Text != "")
                entityDt.DisplayOrder = Convert.ToInt16(txtDisplayOrder.Text);
            else
                entityDt.DisplayOrder = 0;

            entityDt.Remarks = txtRemarksDt.Text;
        }

        private bool IsVourcherAmountValid()
        {
            bool isValid = true;

            Decimal voucher = Convert.ToDecimal(txtVoucherAmount.Text);
            if (voucher == 0)
            {
                isValid = false;
            }

            return isValid;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int GLTransactionID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionHdDao entityHdDao = new GLTransactionHdDao(ctx);
            GLTransactionDtDao entityDtDao = new GLTransactionDtDao(ctx);

            try
            {
                bool isValidDate = true;
                if (hdnLastPostingDate.Value != "")
                {
                    DateTime journalDate = Helper.GetDatePickerValue(Request.Form[txtJournalDate.UniqueID]);
                    DateTime lastPostingDate = Helper.GetDatePickerValue(hdnLastPostingDate.Value);
                    if (journalDate <= lastPostingDate)
                        isValidDate = false;
                }

                bool isValidAmount = true;
                if (!IsVourcherAmountValid())
                {
                    isValidAmount = false;
                }

                if (isValidAmount)
                {
                    if (isValidDate)
                    {
                        string errorMessage = "";
                        SaveGLTransactionHd(ctx, ref GLTransactionID, ref errorMessage);
                        if (GLTransactionID != 0 && String.IsNullOrEmpty(errorMessage))
                        {
                            GLTransactionDt entityDt = new GLTransactionDt();
                            ControlToEntity(entityDt);
                            entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            entityDt.GLTransactionID = GLTransactionID;
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                            entityDt.TransactionDtID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

                            ctx.CommitTransaction();
                        }
                        else if (GLTransactionID == 0 && String.IsNullOrEmpty(errorMessage))
                        {
                            errMessage = "Journal pada periode ini sudah di-posting.";
                            result = false;
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        else
                        {
                            errMessage = errorMessage;
                            result = false;
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        errMessage = "Journal pada periode ini sudah di-posting.";
                        result = false;
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    errMessage = "Nilai voucher tidak diperbolehkan bernilai nol.";
                    result = false;
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionHdDao entityHdDao = new GLTransactionHdDao(ctx);
            GLTransactionDtDao entityDtDao = new GLTransactionDtDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                bool isValidAmount = true;
                if (!IsVourcherAmountValid())
                {
                    isValidAmount = false;
                }

                if (isValidAmount)
                {
                    GLTransactionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    if (entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                    {
                        GLTransactionHd entityHd = entityHdDao.Get(entityDt.GLTransactionID);

                        bool isLock = false;
                        TransactionTypeLock entityLock = entityLockDao.Get(entityHd.TransactionCode);
                        if (entityLock.LockedUntilDate != null)
                        {
                            DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                            DateTime DateNow = entityHd.JournalDate;
                            if (DateNow > DateLock)
                            {
                                isLock = false;
                            }
                            else
                            {
                                isLock = true;
                            }
                        }
                        else
                        {
                            isLock = false;
                        }

                        if (!isLock)
                        {
                            if (entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_UMUM
                                    || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_KAS_KELUAR
                                    || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_KAS_MASUK
                                    || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_BANK_KELUAR
                                    || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_BANK_MASUK
                                    || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PENYESUAIAN
                                    || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PENYUSUTAN_AKTIVA
                                    || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PEMUSNAHAN_AKTIVA
                                    || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR)
                            {
                                ControlToEntity(entityDt);
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);

                                ctx.CommitTransaction();
                            }
                            else
                            {
                                ControlToEntity(entityDt);
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);

                                ctx.CommitTransaction();
                            }
                        }
                        else
                        {
                            errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                            result = false;
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        errMessage = "Data tidak dapat diubah karena sudah diproses.";
                        result = false;
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    errMessage = "Nilai voucher tidak diperbolehkan bernilai nol.";
                    result = false;
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

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionHdDao entityHdDao = new GLTransactionHdDao(ctx);
            GLTransactionDtDao entityDtDao = new GLTransactionDtDao(ctx);
            PurchaseRequestHdDao purchaseRequestHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseReceiveHdDao purchaseReceiveHdDao = new PurchaseReceiveHdDao(ctx);
            DirectPurchaseHdDao directPurchaseHdDao = new DirectPurchaseHdDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                GLTransactionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    GLTransactionHd entityHd = entityHdDao.Get(entityDt.GLTransactionID);

                    bool isLock = false;
                    TransactionTypeLock entityLock = entityLockDao.Get(entityHd.TransactionCode);
                    if (entityLock.LockedUntilDate != null)
                    {
                        DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                        DateTime DateNow = entityHd.JournalDate;
                        if (DateNow > DateLock)
                        {
                            isLock = false;
                        }
                        else
                        {
                            isLock = true;
                        }
                    }
                    else
                    {
                        isLock = false;
                    }

                    if (!isLock)
                    {
                        if (entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_UMUM
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_KAS_KELUAR
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_KAS_MASUK
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_BANK_KELUAR
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_BANK_MASUK
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PENYESUAIAN
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PENYUSUTAN_AKTIVA
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_PEMUSNAHAN_AKTIVA
                                || entityHd.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR)
                        {
                            entityDt.IsDeleted = true;
                            entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);

                            string filterRequest = string.Format("GLTransactionIDRequest = {0} AND GLTransactionDtIDRequest = {1}", entityHd.GLTransactionID, entityDt.TransactionDtID);
                            string filterRealization = string.Format("GLTransactionID = {0} AND GLTransactionDtID = {1}", entityHd.GLTransactionID, entityDt.TransactionDtID);

                            if (entityHd.GCTreasuryType == Constant.TreasuryType.PERMINTAAN_KAS_BON && entityHd.GCTreasuryGroup == Constant.TreasuryGroup.SURAT_PERINTAH_KERJA)
                            {
                                List<PurchaseRequestHd> lstPurchaseRequestHd = BusinessLayer.GetPurchaseRequestHdList(filterRequest, ctx);
                                foreach (PurchaseRequestHd purchaseRequestHd in lstPurchaseRequestHd)
                                {
                                    purchaseRequestHd.GLTransactionIDRequest = null;
                                    purchaseRequestHd.GLTransactionDtIDRequest = null;
                                    purchaseRequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    purchaseRequestHdDao.Update(purchaseRequestHd);
                                }
                            }

                            if (entityHd.GCTreasuryType == Constant.TreasuryType.REALISASI_KAS_BON && entityHd.GCTreasuryGroup == Constant.TreasuryGroup.SURAT_PERINTAH_KERJA)
                            {
                                List<PurchaseReceiveHd> lstPurchaseReceiveHd = BusinessLayer.GetPurchaseReceiveHdList(filterRealization, ctx);
                                foreach (PurchaseReceiveHd purchaseReceiveHd in lstPurchaseReceiveHd)
                                {
                                    purchaseReceiveHd.GLTransactionID = null;
                                    purchaseReceiveHd.GLTransactionDtID = null;
                                    purchaseReceiveHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    purchaseReceiveHdDao.Update(purchaseReceiveHd);
                                }
                            }

                            if (entityHd.GCTreasuryType == Constant.TreasuryType.PERMINTAAN_KAS_BON && entityHd.GCTreasuryGroup == Constant.TreasuryGroup.PERMINTAAN_PEMBELIAN_TUNAI)
                            {
                                List<DirectPurchaseHd> lstDirectPurchaseHd = BusinessLayer.GetDirectPurchaseHdList(filterRequest, ctx);
                                foreach (DirectPurchaseHd directPurchaseHd in lstDirectPurchaseHd)
                                {
                                    directPurchaseHd.GLTransactionIDRequest = null;
                                    directPurchaseHd.GLTransactionDtIDRequest = null;
                                    directPurchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    directPurchaseHdDao.Update(directPurchaseHd);
                                }
                            }

                            if (entityHd.GCTreasuryType == Constant.TreasuryType.REALISASI_KAS_BON && entityHd.GCTreasuryGroup == Constant.TreasuryGroup.REALISASI_PEMBELIAN_TUNAI)
                            {
                                List<DirectPurchaseHd> lstDirectPurchaseHd = BusinessLayer.GetDirectPurchaseHdList(filterRealization, ctx);
                                foreach (DirectPurchaseHd directPurchaseHd in lstDirectPurchaseHd)
                                {
                                    directPurchaseHd.GLTransactionID = null;
                                    directPurchaseHd.GLTransactionDtID = null;
                                    directPurchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    directPurchaseHdDao.Update(directPurchaseHd);
                                }
                            }

                            ctx.CommitTransaction();
                        }
                    }
                    else
                    {
                        errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                        result = false;
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    errMessage = "Data tidak dapat diubah karena sudah diproses.";
                    result = false;
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