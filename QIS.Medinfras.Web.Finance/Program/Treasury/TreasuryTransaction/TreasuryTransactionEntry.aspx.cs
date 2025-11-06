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
    public partial class TreasuryTransactionEntry : BasePageTrx2
    {
        string menuType = string.Empty;

        public override string OnGetMenuCode()
        {
            if (menuType == "currentDate")
                return Constant.MenuCode.Finance.FN_TREASURY_TRANSACTION_CURRENT_DATE;
            else
                return Constant.MenuCode.Finance.FN_TREASURY_TRANSACTION;
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

            hdnUserLoginID.Value = AppSession.UserLogin.UserID.ToString();

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

            List<Healthcare> lstH = BusinessLayer.GetHealthcareList("GLAccountNoSegment IS NOT NULL");
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, lstH, "HealthcareName", "HealthcareID");
            cboHealthcare.SelectedIndex = 0;

            List<Variable> lstPosition = new List<Variable>();
            lstPosition.Add(new Variable { Code = "D", Value = "Debet" });
            lstPosition.Add(new Variable { Code = "K", Value = "Kredit" });
            Methods.SetComboBoxField<Variable>(cboPosition, lstPosition, "Value", "Code");
            cboPosition.SelectedIndex = 0;

            #region Binding from HealthcareParameter & SettingParameterDt

            List<HealthcareParameter> lstHealthcareParameter = BusinessLayer.GetHealthcareParameterList(string.Format(
                                                                "ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_REVENUE_COST_CENTER, //0
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_DEPARTMENT, //1
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_SERVICE_UNIT, //2
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_CUSTOMER_GROUP, //3
                                                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_BUSINESS_PARTNER, //4
                                                                Constant.HealthcareParameter.AC_COA_DIRECT_PURCHASE, //5
                                                                Constant.HealthcareParameter.AC_COA_AP_DOCTOR_FEE, //6
                                                                Constant.HealthcareParameter.AC_COA_KONTRA_PIUTANG, //7
                                                                Constant.SettingParameter.ARRECEIVING_FROM_TREASURY_CAN_DELETE_OR_VOID //8
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
            hdnCOAAPRevenueSharing.Value = lstHealthcareParameter.Find(a => a.ParameterCode == Constant.HealthcareParameter.AC_COA_AP_DOCTOR_FEE).ParameterValue;
            hdnCOAKontraPiutangID.Value = lstHealthcareParameter.Find(a => a.ParameterCode == Constant.HealthcareParameter.AC_COA_KONTRA_PIUTANG).ParameterValue;
            if (hdnCOAKontraPiutangID.Value != "" && hdnCOAKontraPiutangID.Value != "0" && hdnCOAKontraPiutangID.Value != null)
            {
                ChartOfAccount coaKP = BusinessLayer.GetChartOfAccount(Convert.ToInt32(hdnCOAKontraPiutangID.Value));
                hdnCOAKontraPiutangNo.Value = coaKP.GLAccountNo;
                hdnCOAKontraPiutangName.Value = coaKP.GLAccountName;
            }
            else
            {
                hdnCOAKontraPiutangID.Value = "";
                hdnCOAKontraPiutangNo.Value = "";
                hdnCOAKontraPiutangName.Value = "";
            }

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                                AppSession.UserLogin.HealthcareID,
                                                                Constant.SettingParameter.ARRECEIVING_FROM_TREASURY_CAN_DELETE_OR_VOID //1
                                                            )
                                                        );
            hdnIsARReceivingCanDeleteOrVoid.Value = lstSettingParameterDt.Find(a => a.ParameterCode == Constant.SettingParameter.ARRECEIVING_FROM_TREASURY_CAN_DELETE_OR_VOID).ParameterValue;

            #endregion

            if (hdnCashFlowTypeID.Value != null && hdnCashFlowTypeID.Value != "" && hdnCashFlowTypeID.Value != "0")
            {
                CashFlowType oCashFlowType = BusinessLayer.GetCashFlowType(Convert.ToInt32(hdnCashFlowTypeID.Value));
                if (oCashFlowType != null)
                {
                    txtCashFlowTypeCode.Text = oCashFlowType.CashFlowTypeCode;
                    txtCashFlowTypeName.Text = oCashFlowType.CashFlowTypeName;
                }
            }

            decimal totalDebit = -1;
            decimal totalKredit = -1;
            decimal selisih = -1;
            BindGridView(ref totalDebit, ref totalKredit, ref selisih);

            hdnIsEditable.Value = "1";
            hdnIsUsedAsTreasury.Value = "0";
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
            SetControlEntrySetting(lblTreasuryType, new ControlEntrySetting(true, false));
            SetControlEntrySetting(hdnTreasuryType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTreasuryType, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(lblTreasuryGroup, new ControlEntrySetting(true, false));
            SetControlEntrySetting(hdnTreasuryGroup, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTreasuryGroup, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(lblGLAccountTreasury, new ControlEntrySetting(true, false));
            SetControlEntrySetting(hdnGLAccountTreasuryID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccountCodeTreasury, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtGLAccountNameTreasury, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(lblCashFlowType, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnCashFlowTypeID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCashFlowTypeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCashFlowTypeName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnGLAccountTreasuryPosition, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, true));

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
            Helper.SetControlEntrySetting(txtDepartmentName, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnServiceUnitID, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtARPaymentMethod, new ControlEntrySetting(true, true, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtARBank, new ControlEntrySetting(false, false, false), "mpTrxPopup");
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
            hdnDisplayCount.Value = "2";
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
            return "GCTreasuryGroup IS NOT NULL AND TransactionCode BETWEEN '7281' AND '7300' AND TreasuryTypeTagProperty = '1' AND TreasuryGroupTagProperty = '1'";
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvGLTransactionHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText, ref string retval)
        {
            string filterExpression = GetFilterExpression();
            vGLTransactionHd entity = new vGLTransactionHd();
            if (!string.IsNullOrEmpty(filterExpression))
            {
                if (!string.IsNullOrEmpty(retval))
                {
                    filterExpression += string.Format(" AND GLTransactionID = {0}", retval);
                    entity = BusinessLayer.GetvGLTransactionHdList(filterExpression).FirstOrDefault();
                }
                else
                {
                    entity = BusinessLayer.GetvGLTransactionHd(filterExpression, PageIndex, "GLTransactionID DESC");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(retval))
                {
                    filterExpression = string.Format("GLTransactionID = {0}", retval);
                    entity = BusinessLayer.GetvGLTransactionHdList(filterExpression).FirstOrDefault();
                }
                else
                {
                    entity = BusinessLayer.GetvGLTransactionHd(filterExpression, PageIndex, "GLTransactionID DESC");
                }
            }

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
                txtCashFlowTypeCode.Enabled = true;
                txtCashFlowTypeName.Enabled = true;
                txtRemarks.Enabled = true;
            }
            else
            {
                txtCashFlowTypeCode.Enabled = false;
                txtCashFlowTypeName.Enabled = false;
                txtRemarks.Enabled = false;
            }

            tdTransactionNoAdd.Style.Add("display", "none");
            tdTransactionNoEdit.Style.Remove("display");
            hdnID.Value = entity.GLTransactionID.ToString();
            txtJournalNo.Text = entity.JournalNo;
            cboTransactionCode.Value = entity.TransactionCode;
            hdnTransactionStatus.Value = entity.GCTransactionStatus;
            hdnTreasuryType.Value = entity.GCTreasuryType;
            txtTreasuryType.Text = entity.TreasuryType;
            hdnTreasuryGroup.Value = entity.GCTreasuryGroup;
            txtTreasuryGroup.Text = entity.TreasuryGroup;

            string filterTreasury = string.Format("GLTransactionID = {0} AND IsDeleted = 0 AND IsUsedAsTreasury = 1", entity.GLTransactionID);
            vGLTransactionDt entityDt = BusinessLayer.GetvGLTransactionDtList(filterTreasury).FirstOrDefault();
            if (entityDt != null)
            {
                lblGLAccountTreasury.Attributes.Add("class", "disable");
                hdnGLAccountTreasuryID.Value = entityDt.GLAccount.ToString();
                txtGLAccountCodeTreasury.Text = entityDt.GLAccountNo;
                txtGLAccountNameTreasury.Text = entityDt.GLAccountName;
                hdnGLAccountTreasuryPosition.Value = entityDt.Position;
                hdnIsUsedAsTreasury.Value = entityDt.IsUsedAsTreasury ? "1" : "0";
                hdnCashFlowTypeID.Value = entityDt.CashFlowTypeID.ToString();
                txtCashFlowTypeCode.Text = entityDt.CashFlowTypeCode;
                txtCashFlowTypeName.Text = entityDt.CashFlowTypeName;
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
                        entityHd.GCTreasuryType = hdnTreasuryType.Value;
                        entityHd.GCTreasuryGroup = hdnTreasuryGroup.Value;
                        entityHd.Remarks = txtRemarks.Text;
                        entityHd.JournalNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.JournalDate, txtJournalPrefix.Text, ctx);
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        GLTransactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                        hdnID.Value = GLTransactionID.ToString();

                        GLTransactionDt entityDt = new GLTransactionDt();
                        entityDt.GLTransactionID = GLTransactionID;
                        entityDt.GLAccount = Convert.ToInt32(hdnGLAccountTreasuryID.Value);
                        entityDt.CashFlowTypeID = Convert.ToInt32(hdnCashFlowTypeID.Value);
                        if (hdnTreasuryType.Value == Constant.TreasuryType.PENERIMAAN)
                        {
                            entityDt.Position = "D";
                        }
                        else
                        {
                            entityDt.Position = "K";
                        }
                        entityDt.DisplayOrder = 1;
                        entityDt.HealthcareID = AppSession.UserLogin.HealthcareID;
                        entityDt.DebitAmount = 0;
                        entityDt.CreditAmount = 0;
                        if (txtRemarks.Text != "" && txtRemarks.Text != null)
                        {
                            entityDt.Remarks = txtRemarks.Text;
                        }
                        else
                        {
                            entityDt.Remarks = "TREASURY ACCOUNT";
                        }
                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Insert(entityDt);
                    }
                    else
                    {
                        GLTransactionHd entityHd = new GLTransactionHd();
                        entityHd.JournalDate = Helper.GetDatePickerValue(Request.Form[txtJournalDate.UniqueID]);
                        entityHd.GCJournalGroup = Constant.JournalGroup.MEMORIAL;
                        entityHd.TransactionCode = cboTransactionCode.Value.ToString();
                        entityHd.GCTreasuryType = hdnTreasuryType.Value;
                        entityHd.GCTreasuryGroup = hdnTreasuryGroup.Value;
                        entityHd.Remarks = txtRemarks.Text;
                        entityHd.JournalNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.JournalDate, txtJournalPrefix.Text, ctx);
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        GLTransactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                        hdnID.Value = GLTransactionID.ToString();

                        GLTransactionDt entityDt = new GLTransactionDt();
                        entityDt.GLTransactionID = GLTransactionID;
                        entityDt.GLAccount = Convert.ToInt32(hdnGLAccountTreasuryID.Value);
                        entityDt.CashFlowTypeID = Convert.ToInt32(hdnCashFlowTypeID.Value);
                        if (hdnTreasuryType.Value == Constant.TreasuryType.PENERIMAAN)
                        {
                            entityDt.Position = "D";
                        }
                        else
                        {
                            entityDt.Position = "K";
                        }
                        entityDt.DisplayOrder = 1;
                        entityDt.HealthcareID = AppSession.UserLogin.HealthcareID;
                        entityDt.DebitAmount = 0;
                        entityDt.CreditAmount = 0;
                        if (txtRemarks.Text != "" && txtRemarks.Text != null)
                        {
                            entityDt.Remarks = txtRemarks.Text;
                        }
                        else
                        {
                            entityDt.Remarks = "TREASURY ACCOUNT";
                        }
                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Insert(entityDt);
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
                            entityHd.GCTreasuryGroup = hdnTreasuryGroup.Value;
                            entityHd.TransactionCode = cboTransactionCode.Value.ToString();
                            entityHd.Remarks = txtRemarks.Text;
                            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdateGLTransactionHd(entityHd);
                            retval = entityHd.GLTransactionID.ToString();
                            return true;
                        }
                        else
                        {
                            entityHd.JournalDate = Helper.GetDatePickerValue(Request.Form[txtJournalDate.UniqueID]);
                            entityHd.GCJournalGroup = Constant.JournalGroup.MEMORIAL;
                            entityHd.GCTreasuryGroup = hdnTreasuryGroup.Value;
                            entityHd.TransactionCode = cboTransactionCode.Value.ToString();
                            entityHd.Remarks = txtRemarks.Text;
                            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdateGLTransactionHd(entityHd);
                            retval = entityHd.GLTransactionID.ToString();
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
                    errMessage = "GAGAL EDIT : Voucher " + entityHd.JournalNo + " sudah diproses";
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

        protected override bool OnApproveRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            if (Convert.ToDecimal(Request.Form[txtTotalSelisih.UniqueID]) == 0)
            {
                IDbContext ctx = DbFactory.Configure(true);
                GLTransactionHdDao GLTransactionHdDao = new GLTransactionHdDao(ctx);
                GLTransactionDtDao GlTransactionDtDao = new GLTransactionDtDao(ctx);
                ARReceivingHdDao ARReceivingHdDao = new ARReceivingHdDao(ctx);
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
                                string filterExpression = String.Format("GLTransactionID = {0} AND GCItemDetailStatus != '{1}' ORDER BY DisplayOrder", hdnID.Value, Constant.TransactionStatus.VOID);
                                List<GLTransactionDt> lstGLTransactionDt = BusinessLayer.GetGLTransactionDtList(filterExpression, ctx);
                                foreach (GLTransactionDt oGLTransactionDt in lstGLTransactionDt)
                                {
                                    // APPROVE GLTransactionDt
                                    oGLTransactionDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                    oGLTransactionDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    GlTransactionDtDao.Update(oGLTransactionDt);

                                    // APPROVE ARReceivingHd
                                    if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.AR_RECEIVING && oGLTransactionDt.DisplayOrder != 1)
                                    {
                                        if (oGLTransactionDt.ReferenceNo != null && oGLTransactionDt.ReferenceNo != "" && oGLTransactionDt.IsReferenceNoGeneratedBySystem)
                                        {
                                            string filterARRcv = string.Format("ARReceivingNo = '{0}'", oGLTransactionDt.ReferenceNo);
                                            List<ARReceivingHd> rcvHDLst = BusinessLayer.GetARReceivingHdList(filterARRcv, ctx);
                                            if (rcvHDLst.Count() > 0)
                                            {
                                                ARReceivingHd arrcv = rcvHDLst.FirstOrDefault();
                                                arrcv.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                                arrcv.ApprovedBy = AppSession.UserLogin.UserID;
                                                arrcv.ApprovedDate = DateTime.Now;
                                                arrcv.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                ARReceivingHdDao.Update(arrcv);
                                            }
                                        }
                                    }
                                }

                                // APPROVE GLTransactionHd
                                itemTransactionHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                itemTransactionHd.ApprovedBy = AppSession.UserLogin.UserID;
                                itemTransactionHd.ApprovedDate = DateTime.Now;
                                itemTransactionHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                GLTransactionHdDao.Update(itemTransactionHd);

                                retval = itemTransactionHd.GLTransactionID.ToString();
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                string filterExpression = String.Format("GLTransactionID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", hdnID.Value, Constant.TransactionStatus.VOID);
                                List<GLTransactionDt> lstGLTransactionDt = BusinessLayer.GetGLTransactionDtList(filterExpression, ctx);
                                foreach (GLTransactionDt oGLTransactionDt in lstGLTransactionDt)
                                {
                                    // APPROVE GLTransactionDt
                                    oGLTransactionDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                    oGLTransactionDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    GlTransactionDtDao.Update(oGLTransactionDt);

                                    // APPROVE ARReceivingHd
                                    if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.AR_RECEIVING && oGLTransactionDt.DisplayOrder != 1)
                                    {
                                        if (oGLTransactionDt.ReferenceNo != null && oGLTransactionDt.ReferenceNo != "" && oGLTransactionDt.IsReferenceNoGeneratedBySystem)
                                        {
                                            string filterARRcv = string.Format("ARReceivingNo = '{0}'", oGLTransactionDt.ReferenceNo);
                                            List<ARReceivingHd> rcvHDLst = BusinessLayer.GetARReceivingHdList(filterARRcv, ctx);
                                            if (rcvHDLst.Count() > 0)
                                            {
                                                ARReceivingHd arrcv = rcvHDLst.FirstOrDefault();
                                                arrcv.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                                arrcv.ApprovedBy = AppSession.UserLogin.UserID;
                                                arrcv.ApprovedDate = DateTime.Now;
                                                arrcv.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                ARReceivingHdDao.Update(arrcv);
                                            }
                                        }
                                    }
                                }

                                // APPROVE GLTransactionHd
                                itemTransactionHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                itemTransactionHd.ApprovedBy = AppSession.UserLogin.UserID;
                                itemTransactionHd.ApprovedDate = DateTime.Now;
                                itemTransactionHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                GLTransactionHdDao.Update(itemTransactionHd);

                                retval = itemTransactionHd.GLTransactionID.ToString();
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
                        errMessage = "GAGAL APPROVE : Voucher " + itemTransactionHd.JournalNo + " sudah diproses";
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

        protected override bool OnProposeRecord(ref string errMessage, ref string retval)
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

                                retval = entity.GLTransactionID.ToString();
                                return true;
                            }
                            else
                            {
                                entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdateGLTransactionHd(entity);

                                retval = entity.GLTransactionID.ToString();
                                return true;
                            }
                        }
                        else
                        {
                            errMessage = "GAGAL PROPOSED : Voucher " + entity.JournalNo + " sudah diproses";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            return false;
                        }
                    }
                    else
                    {
                        errMessage = "GAGAL PROPOSED : Voucher " + entity.JournalNo + " sudah diproses";
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
            SupplierPaymentHdDao supPaymentHdDao = new SupplierPaymentHdDao(ctx);
            SupplierPaymentDtDao supPaymentDtDao = new SupplierPaymentDtDao(ctx);
            ARReceivingHdDao arReceivingHdDao = new ARReceivingHdDao(ctx);
            ARInvoiceReceivingDao entityARRcvDao = new ARInvoiceReceivingDao(ctx);
            ARInvoiceReceivingDtDao entityARRcvDtDao = new ARInvoiceReceivingDtDao(ctx);
            ARInvoiceHdDao eInvoiceHd = new ARInvoiceHdDao(ctx);
            ARInvoiceDtDao eInvoiceDt = new ARInvoiceDtDao(ctx);
            TransRevenueSharingPaymentHdDao rspaymentHdDao = new TransRevenueSharingPaymentHdDao(ctx);
            TransRevenueSharingPaymentDtDao rspaymentDtDao = new TransRevenueSharingPaymentDtDao(ctx);
            DirectPurchaseHdDao directPurchaseHdDao = new DirectPurchaseHdDao(ctx);
            UserPatientPaymentBalanceDao uppbDao = new UserPatientPaymentBalanceDao(ctx);
            PatientPaymentDtInfoDao ppdtInfoDao = new PatientPaymentDtInfoDao(ctx);
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

                                    if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.SUPPLIER_PAYMENT && entityDt.DisplayOrder != 1)
                                    {
                                        #region Supplier Payment

                                        string filterSPHD = string.Format("SupplierPaymentNo = '{0}'", entityDt.ReferenceNo);
                                        List<SupplierPaymentHd> lstSupPaymentHd = BusinessLayer.GetSupplierPaymentHdList(filterSPHD, ctx);
                                        if (lstSupPaymentHd.Count() > 0)
                                        {
                                            foreach (SupplierPaymentHd supPaymentHd in lstSupPaymentHd)
                                            {
                                                string filterSPDT = string.Format("SupplierPaymentID = '{0}'", supPaymentHd.SupplierPaymentID);
                                                List<SupplierPaymentDt> lstSupPaymentDt = BusinessLayer.GetSupplierPaymentDtList(filterSPDT, ctx);
                                                foreach (SupplierPaymentDt supPaymentDt in lstSupPaymentDt)
                                                {
                                                    supPaymentDt.GLTransactionDtID = null;
                                                    supPaymentDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    supPaymentDtDao.Update(supPaymentDt);
                                                }

                                                supPaymentHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                                supPaymentHd.ApprovedBy = null;
                                                supPaymentHd.ApprovedDate = DateTime.Parse("1900-01-01");
                                                supPaymentHd.GLTransactionID = null;
                                                supPaymentHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                supPaymentHdDao.Update(supPaymentHd);
                                            }
                                        }

                                        #endregion
                                    }
                                    else if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.AR_RECEIVING && entityDt.DisplayOrder != 1)
                                    {
                                        #region AR Receiving

                                        if (entityDt.ReferenceNo != null && entityDt.ReferenceNo != "" && entityDt.IsReferenceNoGeneratedBySystem)
                                        {
                                            string filterAR = string.Format("ARReceivingNo = '{0}'", entityDt.ReferenceNo);
                                            List<ARReceivingHd> rcvHDLst = BusinessLayer.GetARReceivingHdList(filterAR, ctx);
                                            if (rcvHDLst.Count() > 0)
                                            {
                                                ARReceivingHd rcvHD = rcvHDLst.FirstOrDefault();

                                                string filterInvRcvList = string.Format("ARReceivingID = {0} AND IsDeleted = 0", rcvHD.ARReceivingID);
                                                List<ARInvoiceReceiving> lstInvoiceReceiving = BusinessLayer.GetARInvoiceReceivingList(filterInvRcvList, ctx);

                                                if (lstInvoiceReceiving.Count() > 0 && hdnIsARReceivingCanDeleteOrVoid.Value == "1")
                                                {
                                                    errMessage = "GAGAL VOID : Voucher " + entityHD.JournalNo + " dari penerimaan piutang tidak diperbolehkan void / delete (Setting Parameter : AC0010)";
                                                    result = false;
                                                    Exception ex = new Exception(errMessage);
                                                    Helper.InsertErrorLog(ex);
                                                    ctx.RollBackTransaction();
                                                    break;
                                                }
                                                else
                                                {
                                                    // VOID ARReceiving
                                                    rcvHD.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                                    rcvHD.GCVoidReason = Constant.DeleteReason.OTHER;
                                                    rcvHD.VoidReason = "Delete from TreasuryDT";
                                                    rcvHD.VoidBy = AppSession.UserLogin.UserID;
                                                    rcvHD.VoidDate = DateTime.Now;
                                                    rcvHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    arReceivingHdDao.Update(rcvHD);

                                                    // DELETE ARInvoiceReceiving
                                                    foreach (ARInvoiceReceiving invRcv in lstInvoiceReceiving)
                                                    {
                                                        invRcv.IsDeleted = true;
                                                        invRcv.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                        ctx.CommandType = CommandType.Text;
                                                        ctx.Command.Parameters.Clear();
                                                        entityARRcvDao.Update(invRcv);

                                                        string filterARInvRcvDt = string.Format("ARInvoiceReceivingID = {0} AND IsDeleted = 0", invRcv.ID);
                                                        List<ARInvoiceReceivingDt> lstARInvRcvDt = BusinessLayer.GetARInvoiceReceivingDtList(filterARInvRcvDt, ctx);
                                                        foreach (ARInvoiceReceivingDt arInvRcvDt in lstARInvRcvDt)
                                                        {
                                                            arInvRcvDt.IsDeleted = true;
                                                            arInvRcvDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                            ctx.CommandType = CommandType.Text;
                                                            ctx.Command.Parameters.Clear();
                                                            entityARRcvDtDao.Update(arInvRcvDt);
                                                        }

                                                        // mengurangi nilai saat sudah alokasi
                                                        ARInvoiceHd oInvoiceHd = eInvoiceHd.Get(invRcv.ARInvoiceID);
                                                        oInvoiceHd.TotalPaymentAmount -= invRcv.ReceivingAmount;
                                                        if (oInvoiceHd.TotalPaymentAmount == 0)
                                                        {
                                                            oInvoiceHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;

                                                            string filterInvDt = string.Format("ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", invRcv.ARInvoiceID, Constant.TransactionStatus.VOID);
                                                            List<ARInvoiceDt> lstInvoiceDt = BusinessLayer.GetARInvoiceDtList(filterInvDt, ctx);
                                                            foreach (ARInvoiceDt oInvoiceDt in lstInvoiceDt)
                                                            {
                                                                oInvoiceDt.PaymentAmount = 0;
                                                                oInvoiceDt.GCTransactionDetailStatus = Constant.TransactionStatus.APPROVED;
                                                                oInvoiceDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                                ctx.CommandType = CommandType.Text;
                                                                ctx.Command.Parameters.Clear();
                                                                eInvoiceDt.Update(oInvoiceDt);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            oInvoiceHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                                        }
                                                        oInvoiceHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                        eInvoiceHd.Update(oInvoiceHd);
                                                    }
                                                }
                                            }
                                        }

                                        #endregion
                                    }
                                    else if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.DIRECT_PURCHASE && entityDt.DisplayOrder != 1)
                                    {
                                        #region Direct Purchase

                                        string filterDP = string.Format("DirectPurchaseNo = '{0}'", entityDt.ReferenceNo);
                                        DirectPurchaseHd dpHD = BusinessLayer.GetDirectPurchaseHdList(filterDP, ctx).FirstOrDefault();
                                        dpHD.GLTransactionID = null;
                                        dpHD.GLTransactionDtID = null;
                                        dpHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        directPurchaseHdDao.Update(dpHD);

                                        #endregion
                                    }
                                    else if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.REVENUE_SHARING && entityDt.DisplayOrder != 1)
                                    {
                                        #region Revenue Sharing

                                        string filterRSPHD = string.Format("RSPaymentNo = '{0}'", entityDt.ReferenceNo);
                                        TransRevenueSharingPaymentHd paymentHd = BusinessLayer.GetTransRevenueSharingPaymentHdList(filterRSPHD, ctx).FirstOrDefault();
                                        paymentHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                        paymentHd.GLTransactionID = null;
                                        paymentHd.ApprovedBy = null;
                                        paymentHd.ApprovedDate = DateTime.Parse("1900-01-01");
                                        paymentHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        rspaymentHdDao.Update(paymentHd);

                                        string filterDt = string.Format("RSPaymentID = {0} AND IsDeleted = 0", paymentHd.RSPaymentID);
                                        List<TransRevenueSharingPaymentDt> lstPaymentDt = BusinessLayer.GetTransRevenueSharingPaymentDtList(filterDt, ctx);
                                        foreach (TransRevenueSharingPaymentDt paymentDt in lstPaymentDt)
                                        {
                                            paymentDt.GLTransactionDtID = null;
                                            paymentDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            rspaymentDtDao.Update(paymentDt);
                                        }
                                        #endregion
                                    }
                                    else if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.SETORAN_KASIR && entityDt.DisplayOrder != 1)
                                    {
                                        #region Setoran Kasir

                                        if (!String.IsNullOrEmpty(entityDt.ReferenceNo) && entityDt.IsReferenceNoGeneratedBySystem)
                                        {
                                            UserPatientPaymentBalance oUserPatientPaymentBalance = uppbDao.Get(Convert.ToInt32(entityDt.ReferenceNo));
                                            oUserPatientPaymentBalance.PaymentAmountOUT -= entityDt.DebitAmount + entityDt.CreditAmount;
                                            oUserPatientPaymentBalance.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            uppbDao.Update(oUserPatientPaymentBalance);
                                        }

                                        #endregion
                                    }
                                    else if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.SETORAN_KASIR_REKONSILIASI && entityDt.DisplayOrder != 1)
                                    {
                                        #region Setoran Kasir Rekonsiliasi

                                        if (!String.IsNullOrEmpty(entityDt.ReferenceNo) && entityDt.IsReferenceNoGeneratedBySystem)
                                        {
                                            PatientPaymentDtInfo ppDtInfo = ppdtInfoDao.Get(Convert.ToInt32(entityDt.ReferenceNo));
                                            ppDtInfo.IsReconciled = false;
                                            ppDtInfo.ReconciledBy = null;
                                            ppDtInfo.ReconciledDate = null;
                                            ppDtInfo.GLTransactionDtID = null;
                                            ppdtInfoDao.Update(ppDtInfo);
                                        }

                                        #endregion
                                    }
                                }

                                glTransactionHdDao.Update(entityHD);
                                retval = entityHD.JournalNo;
                                result = true;
                                ctx.CommitTransaction();

                            }
                        }
                        else
                        {
                            errMessage = "GAGAL VOID : Voucher " + entityHD.JournalNo + " sudah diproses";
                            result = false;
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "GAGAL VOID : Voucher " + entityHD.JournalNo + " sudah diproses";
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
            entityDt.GLAccount = Convert.ToInt32(hdnGLAccountID.Value);

            if (hdnSubLedgerDtID.Value != "" && hdnSubLedgerDtID.Value != "0")
            {
                entityDt.SubLedger = Convert.ToInt32(hdnSubLedgerDtID.Value);
            }
            else
            {
                entityDt.SubLedger = null;
            }

            if (hdnCashFlowTypeID.Value != "" && hdnCashFlowTypeID.Value != "0")
            {
                entityDt.CashFlowTypeID = Convert.ToInt32(hdnCashFlowTypeID.Value);
            }
            else
            {
                entityDt.CashFlowTypeID = null;
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

            //Decimal debit = Convert.ToDecimal(txtAmountD.Text);
            //Decimal kredit = Convert.ToDecimal(txtAmountK.Text);

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
            ChartOfAccountDao coaDao = new ChartOfAccountDao(ctx);
            ARReceivingHdDao receivingHdDao = new ARReceivingHdDao(ctx);
            ARReceivingDtDao receivingDtDao = new ARReceivingDtDao(ctx);

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

                bool isValidPatient = true;

                if (hdnBusinessPartnerID.Value == "1" && hdnMRN.Value == "")
                {
                    isValidPatient = false;
                }

                if (isValidAmount)
                {
                    if (isValidPatient)
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

                                #region ARReceiving

                                // INSERT ARReceivingHd
                                if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.AR_RECEIVING)
                                {
                                    ChartOfAccount entityCOA = coaDao.Get(Convert.ToInt32(entityDt.GLAccount));
                                    if ((hdnCOAKontraPiutangID.Value == entityCOA.GLAccountID.ToString()) // coa hrs kontra piutang
                                            || (hdnCOAKontraPiutangID.Value != entityCOA.GLAccountID.ToString() && (entityCOA.IsUsingCustomerGroup || entityCOA.IsUsingBusinessPartner))) // selain coa kontra piutang hrs cek flag CG / BP
                                    {
                                        ARReceivingHd rcvHD = new ARReceivingHd();
                                        rcvHD.ReceivingDate = Helper.GetDatePickerValue(Request.Form[txtJournalDate.UniqueID]);
                                        rcvHD.CustomerGroupID = entityDt.CustomerGroupID;
                                        rcvHD.BusinessPartnerID = entityDt.BusinessPartnerID;
                                        if (rcvHD.BusinessPartnerID == 1)
                                        {
                                            rcvHD.MRN = entityDt.MRN;
                                        }
                                        rcvHD.TotalReceivingAmount = entityDt.DebitAmount + entityDt.CreditAmount;
                                        rcvHD.TotalInvoiceAmount = 0;
                                        rcvHD.TotalFeeAmount = 0;
                                        rcvHD.CashBackAmount = 0;
                                        rcvHD.Remarks = "KAS BANK";
                                        rcvHD.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                        rcvHD.CreatedBy = AppSession.UserLogin.UserID;

                                        if (rcvHD.BusinessPartnerID != 1)
                                        {
                                            rcvHD.ARReceivingNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.AR_RECEIVE_PAYER, rcvHD.ReceivingDate, ctx);
                                        }
                                        else
                                        {
                                            rcvHD.MRN = entityDt.MRN;
                                            rcvHD.ARReceivingNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.AR_RECEIVE_PATIENT, rcvHD.ReceivingDate, ctx);
                                        }

                                        rcvHD.GLTransactionID = entityDt.GLTransactionID;
                                        rcvHD.GLTransactionDtID = entityDt.TransactionDtID;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        int ARReceivingID = receivingHdDao.InsertReturnPrimaryKeyID(rcvHD);

                                        // INSERT ARReceivingDt
                                        ARReceivingDt rcvDT = new ARReceivingDt();
                                        rcvDT.ARReceivingID = ARReceivingID;
                                        rcvDT.GCARPaymentMethod = hdnARPaymentMethod.Value;
                                        if (hdnARBank.Value != "" && hdnARBank.Value != "0")
                                        {
                                            rcvDT.BankID = Convert.ToInt32(hdnARBank.Value);
                                        }
                                        rcvDT.PaymentAmount = entityDt.DebitAmount + entityDt.CreditAmount;
                                        rcvDT.CreatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        receivingDtDao.Insert(rcvDT);

                                        // UPDATE GLTransactionDt
                                        entityDt.ReferenceNo = rcvHD.ARReceivingNo;
                                        entityDt.IsReferenceNoGeneratedBySystem = true;
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityDtDao.Update(entityDt);
                                    }
                                }

                                #endregion

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
                        errMessage = "Data Pasien tidak diperbolehkan kosong.";
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
            ARReceivingHdDao arReceivingHdDao = new ARReceivingHdDao(ctx);
            ARReceivingDtDao arReceivingDtDao = new ARReceivingDtDao(ctx);
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

                                if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.AR_RECEIVING)
                                {
                                    #region ARReceiving

                                    if (entityDt.ReferenceNo != null && entityDt.ReferenceNo != "" && entityDt.IsReferenceNoGeneratedBySystem)
                                    {
                                        // UPDATE ARReceivingHd
                                        string filterAR = string.Format("ARReceivingNo = '{0}'", entityDt.ReferenceNo);
                                        List<ARReceivingHd> rcvHDLst = BusinessLayer.GetARReceivingHdList(filterAR, ctx);
                                        if (rcvHDLst.Count() > 0)
                                        {
                                            ARReceivingHd rcvHD = rcvHDLst.FirstOrDefault();
                                            rcvHD.CustomerGroupID = entityDt.CustomerGroupID;
                                            rcvHD.BusinessPartnerID = entityDt.BusinessPartnerID;
                                            if (rcvHD.BusinessPartnerID == 1)
                                            {
                                                rcvHD.MRN = entityDt.MRN;
                                            }
                                            rcvHD.TotalReceivingAmount = entityDt.DebitAmount + entityDt.CreditAmount;
                                            rcvHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            arReceivingHdDao.Update(rcvHD);

                                            // UPDATE ARReceivingDt
                                            string filterDt = string.Format("ARReceivingID = {0} AND IsDeleted = 0", rcvHD.ARReceivingID);
                                            List<ARReceivingDt> lstDt = BusinessLayer.GetARReceivingDtList(filterDt, ctx);
                                            foreach (ARReceivingDt rcvDT in lstDt)
                                            {
                                                rcvDT.GCARPaymentMethod = hdnARPaymentMethod.Value;
                                                if (hdnARBank.Value != "" && hdnARBank.Value != "0")
                                                {
                                                    rcvDT.BankID = Convert.ToInt32(hdnARBank.Value);
                                                }
                                                rcvDT.PaymentAmount = entityDt.DebitAmount + entityDt.CreditAmount;
                                                rcvDT.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                arReceivingDtDao.Update(rcvDT);
                                            }
                                        }
                                    }

                                    #endregion
                                }

                                ctx.CommitTransaction();
                            }
                            else
                            {
                                ControlToEntity(entityDt);
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);

                                if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.AR_RECEIVING)
                                {
                                    #region ARReceiving

                                    if (entityDt.ReferenceNo != null && entityDt.ReferenceNo != "" && entityDt.IsReferenceNoGeneratedBySystem)
                                    {
                                        // UPDATE ARReceivingHd
                                        string filterAR = string.Format("ARReceivingNo = '{0}'", entityDt.ReferenceNo);
                                        List<ARReceivingHd> rcvHDLst = BusinessLayer.GetARReceivingHdList(filterAR, ctx);
                                        if (rcvHDLst.Count() > 0)
                                        {
                                            ARReceivingHd rcvHD = rcvHDLst.FirstOrDefault();
                                            rcvHD.CustomerGroupID = entityDt.CustomerGroupID;
                                            rcvHD.BusinessPartnerID = entityDt.BusinessPartnerID;
                                            if (rcvHD.BusinessPartnerID == 1)
                                            {
                                                rcvHD.MRN = entityDt.MRN;
                                            }
                                            rcvHD.TotalReceivingAmount = entityDt.DebitAmount + entityDt.CreditAmount;
                                            rcvHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            arReceivingHdDao.Update(rcvHD);

                                            // UPDATE ARReceivingDt
                                            string filterDt = string.Format("ARReceivingID = {0} AND IsDeleted = 0", rcvHD.ARReceivingID);
                                            List<ARReceivingDt> lstDt = BusinessLayer.GetARReceivingDtList(filterDt, ctx);
                                            foreach (ARReceivingDt rcvDT in lstDt)
                                            {
                                                rcvDT.GCARPaymentMethod = hdnARPaymentMethod.Value;
                                                if (hdnARBank.Value != "" && hdnARBank.Value != "0")
                                                {
                                                    rcvDT.BankID = Convert.ToInt32(hdnARBank.Value);
                                                }
                                                rcvDT.PaymentAmount = entityDt.DebitAmount + entityDt.CreditAmount;
                                                rcvDT.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                arReceivingDtDao.Update(rcvDT);
                                            }
                                        }
                                    }

                                    #endregion
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
            SupplierPaymentDtDao supPaymentDtDao = new SupplierPaymentDtDao(ctx);
            SupplierPaymentHdDao supPaymentHdDao = new SupplierPaymentHdDao(ctx);
            ARReceivingHdDao arReceivingHdDao = new ARReceivingHdDao(ctx);
            ARInvoiceReceivingDao invReceivingDao = new ARInvoiceReceivingDao(ctx);
            ARInvoiceReceivingDtDao invReceivingDtDao = new ARInvoiceReceivingDtDao(ctx);
            ARInvoiceHdDao eInvoiceHd = new ARInvoiceHdDao(ctx);
            ARInvoiceDtDao eInvoiceDt = new ARInvoiceDtDao(ctx);
            TransRevenueSharingPaymentHdDao rspaymentHdDao = new TransRevenueSharingPaymentHdDao(ctx);
            TransRevenueSharingPaymentDtDao rspaymentDtDao = new TransRevenueSharingPaymentDtDao(ctx);
            DirectPurchaseHdDao dpcHdDao = new DirectPurchaseHdDao(ctx);
            UserPatientPaymentBalanceDao uppbDao = new UserPatientPaymentBalanceDao(ctx);
            PatientPaymentDtInfoDao ppdtInfoDao = new PatientPaymentDtInfoDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                GLTransactionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                GLTransactionHd entityHd = entityHdDao.Get(entityDt.GLTransactionID);

                if (entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                {
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

                            // UPDATE semuaGLTransactionDt yang sama SupplierPaymentNo nya menjadi IsDeleted
                            if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.SUPPLIER_PAYMENT)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                string filterGLDT = string.Format("TransactionDtID != {0} AND ReferenceNo = '{1}' AND GCItemDetailStatus != '{2}' AND IsDeleted = 0", entityDt.TransactionDtID, entityDt.ReferenceNo, Constant.TransactionStatus.VOID);
                                List<GLTransactionDt> lstGLTransDt = BusinessLayer.GetGLTransactionDtList(filterGLDT, ctx);
                                if (lstGLTransDt.Count() > 0)
                                {
                                    foreach (GLTransactionDt otherDT in lstGLTransDt)
                                    {
                                        otherDT.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                                        otherDT.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        otherDT.IsDeleted = true;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityDtDao.Update(otherDT);
                                    }
                                }
                            }

                            if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.SUPPLIER_PAYMENT)
                            {
                                #region SupplierPayment

                                // VOID SupplierPayment
                                string filterSPHD = string.Format("SupplierPaymentNo = '{0}'", entityDt.ReferenceNo);
                                List<SupplierPaymentHd> lstSupPaymentHd = BusinessLayer.GetSupplierPaymentHdList(filterSPHD, ctx);
                                if (lstSupPaymentHd.Count() > 0)
                                {
                                    foreach (SupplierPaymentHd supPaymentHd in lstSupPaymentHd)
                                    {
                                        string filterSPDT = string.Format("SupplierPaymentID = '{0}'", supPaymentHd.SupplierPaymentID);
                                        List<SupplierPaymentDt> lstSupPaymentDt = BusinessLayer.GetSupplierPaymentDtList(filterSPDT, ctx);
                                        foreach (SupplierPaymentDt supPaymentDt in lstSupPaymentDt)
                                        {
                                            supPaymentDt.GLTransactionDtID = null;
                                            supPaymentDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            supPaymentDtDao.Update(supPaymentDt);
                                        }

                                        supPaymentHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                        supPaymentHd.ApprovedBy = null;
                                        supPaymentHd.ApprovedDate = DateTime.Parse("1900-01-01");
                                        supPaymentHd.GLTransactionID = null;
                                        supPaymentHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        supPaymentHdDao.Update(supPaymentHd);
                                    }
                                }

                                #endregion
                            }
                            else if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.AR_RECEIVING)
                            {
                                #region ARReceiving

                                if (entityDt.ReferenceNo != null && entityDt.ReferenceNo != "" && entityDt.IsReferenceNoGeneratedBySystem)
                                {
                                    string filterAR = string.Format("ARReceivingNo = '{0}'", entityDt.ReferenceNo);
                                    List<ARReceivingHd> rcvHDLst = BusinessLayer.GetARReceivingHdList(filterAR, ctx);
                                    if (rcvHDLst.Count() > 0)
                                    {
                                        ARReceivingHd rcvHD = rcvHDLst.FirstOrDefault();

                                        string filterInvRcv = string.Format("ARReceivingID = '{0}' AND IsDeleted = 0", rcvHD.ARReceivingID);
                                        List<ARInvoiceReceiving> lstInvRcv = BusinessLayer.GetARInvoiceReceivingList(filterInvRcv, ctx);

                                        if (lstInvRcv.Count() > 0 && hdnIsARReceivingCanDeleteOrVoid.Value == "1")
                                        {
                                            errMessage = "GAGAL DELETE DETAIL : Voucher " + entityHd.JournalNo + " dari penerimaan piutang tidak diperbolehkan void / delete (Setting Parameter : AC0010)";
                                            result = false;
                                            Exception ex = new Exception(errMessage);
                                            Helper.InsertErrorLog(ex);
                                            ctx.RollBackTransaction();
                                        }
                                        else
                                        {
                                            // VOID ARReceiving
                                            rcvHD.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                            rcvHD.GCVoidReason = Constant.DeleteReason.OTHER;
                                            rcvHD.VoidReason = "Delete from TreasuryDT";
                                            rcvHD.VoidBy = AppSession.UserLogin.UserID;
                                            rcvHD.VoidDate = DateTime.Now;
                                            rcvHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            arReceivingHdDao.Update(rcvHD);

                                            // DELETE ARInvoiceReceiving
                                            foreach (ARInvoiceReceiving invRcv in lstInvRcv)
                                            {
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();

                                                invRcv.IsDeleted = true;
                                                invRcv.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                invReceivingDao.Update(invRcv);

                                                string filterARInvRcvDt = string.Format("ARInvoiceReceivingID = {0} AND IsDeleted = 0", invRcv.ID);
                                                List<ARInvoiceReceivingDt> lstARInvRcvDt = BusinessLayer.GetARInvoiceReceivingDtList(filterARInvRcvDt, ctx);
                                                foreach (ARInvoiceReceivingDt arInvRcvDt in lstARInvRcvDt)
                                                {
                                                    arInvRcvDt.IsDeleted = true;
                                                    arInvRcvDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    invReceivingDtDao.Update(arInvRcvDt);
                                                }

                                                ARInvoiceHd invHd = eInvoiceHd.Get(invRcv.ARInvoiceID);
                                                invHd.TotalPaymentAmount -= rcvHD.TotalReceivingAmount;
                                                if (invHd.TotalPaymentAmount == 0)
                                                {
                                                    invHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;

                                                    string filterInvDt = string.Format("ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", invRcv.ARInvoiceID, Constant.TransactionStatus.VOID);
                                                    List<ARInvoiceDt> lstInvoiceDt = BusinessLayer.GetARInvoiceDtList(filterInvDt, ctx);
                                                    foreach (ARInvoiceDt oInvoiceDt in lstInvoiceDt)
                                                    {
                                                        oInvoiceDt.PaymentAmount = 0;
                                                        oInvoiceDt.GCTransactionDetailStatus = Constant.TransactionStatus.APPROVED;
                                                        oInvoiceDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                        ctx.CommandType = CommandType.Text;
                                                        ctx.Command.Parameters.Clear();
                                                        eInvoiceDt.Update(oInvoiceDt);
                                                    }
                                                }
                                                else
                                                {
                                                    invHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                                }
                                                invHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                eInvoiceHd.Update(invHd);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            else if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.DIRECT_PURCHASE)
                            {
                                #region Direct Purchase

                                string filterDPC = string.Format("DirectPurchaseNo = '{0}'", entityDt.ReferenceNo);
                                DirectPurchaseHd dpcHd = BusinessLayer.GetDirectPurchaseHdList(filterDPC, ctx).FirstOrDefault();
                                if (dpcHd != null)
                                {
                                    dpcHd.GLTransactionID = null;
                                    dpcHd.GLTransactionDtID = null;
                                    dpcHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    dpcHdDao.Update(dpcHd);
                                }

                                #endregion
                            }
                            else if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.REVENUE_SHARING)
                            {
                                #region Revenue Sharing

                                string filterRSPHD = string.Format("RSPaymentNo = '{0}'", entityDt.ReferenceNo);
                                TransRevenueSharingPaymentHd paymentHd = BusinessLayer.GetTransRevenueSharingPaymentHdList(filterRSPHD, ctx).FirstOrDefault();
                                paymentHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                paymentHd.GLTransactionID = null;
                                paymentHd.ApprovedBy = null;
                                paymentHd.ApprovedDate = DateTime.Parse("1900-01-01");
                                paymentHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                rspaymentHdDao.Update(paymentHd);

                                string filterDt = string.Format("RSPaymentID = {0} AND IsDeleted = 0", paymentHd.RSPaymentID);
                                List<TransRevenueSharingPaymentDt> lstPaymentDt = BusinessLayer.GetTransRevenueSharingPaymentDtList(filterDt, ctx);
                                foreach (TransRevenueSharingPaymentDt paymentDt in lstPaymentDt)
                                {
                                    paymentDt.GLTransactionDtID = null;
                                    paymentDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    rspaymentDtDao.Update(paymentDt);
                                }
                                #endregion
                            }
                            else if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.SETORAN_KASIR)
                            {
                                #region Setoran Kasir

                                if (entityDt.ReferenceNo != null && entityDt.ReferenceNo != "" && entityDt.IsReferenceNoGeneratedBySystem)
                                {
                                    UserPatientPaymentBalance oUserPatientPaymentBalance = uppbDao.Get(Convert.ToInt32(entityDt.ReferenceNo));
                                    if (oUserPatientPaymentBalance != null)
                                    {
                                        oUserPatientPaymentBalance.PaymentAmountOUT -= entityDt.DebitAmount + entityDt.CreditAmount;
                                        oUserPatientPaymentBalance.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        uppbDao.Update(oUserPatientPaymentBalance);
                                    }
                                }

                                #endregion
                            }
                            else if (hdnTreasuryGroup.Value == Constant.TreasuryGroup.SETORAN_KASIR_REKONSILIASI && entityDt.DisplayOrder != 1)
                            {
                                #region Setoran Kasir Rekonsiliasi

                                if (entityDt.ReferenceNo != null && entityDt.ReferenceNo != "" && entityDt.IsReferenceNoGeneratedBySystem)
                                {
                                    PatientPaymentDtInfo ppDtInfo = ppdtInfoDao.Get(Convert.ToInt32(entityDt.ReferenceNo));
                                    ppDtInfo.IsReconciled = false;
                                    ppDtInfo.ReconciledBy = null;
                                    ppDtInfo.ReconciledDate = null;
                                    ppDtInfo.GLTransactionDtID = null;
                                    ppdtInfoDao.Update(ppDtInfo);
                                }

                                #endregion
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
                    errMessage = "GAGAL DELETE DETAIL : Voucher " + entityHd.JournalNo + " tidak dapat diubah karena sudah diproses.";
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