using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APInvoiceSupplierProcess : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AP_INVOICE_SUPPLIER_PROCESS;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowVoid = false;
        }

        protected override void InitializeDataControl()
        {
            hdnBusinessPartnerID.Value = AppSession.BusinessPartnerID.ToString();
            BusinessPartners bp = BusinessLayer.GetBusinessPartners(Convert.ToInt32(hdnBusinessPartnerID.Value));
            hdnGCBusinessPartnerType.Value = bp.GCBusinessPartnerType;

            hdnIsAPConsignmentFromOrder.Value = AppSession.IsAPConsignmentFromOrder;

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoidByReason.Value = CRUDMode.Contains("X") ? "1" : "0";

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList(string.Format("IsActive = 1"));
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.Value = "NON";

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format(
                                                                    "ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                                    Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE, //0
                                                                    Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE, //1
                                                                    Constant.SettingParameter.IS_PROCESS_INVOICE_CAN_CHANGE_AVERAGE_PRICE, //2
                                                                    Constant.SettingParameter.FN_DEFAULT_PENGAMBILAN_BERKAS_PEMBAYARAN, //3
                                                                    Constant.SettingParameter.VAT_PERCENTAGE, //4
                                                                    Constant.SettingParameter.FN0148 //5
                                                                ));

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}')",
                                                                    AppSession.UserLogin.HealthcareID, //0
                                                                    Constant.SettingParameter.FN0148, //1
                                                                    Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED, //2
                                                                    Constant.SettingParameter.FN_IS_PURCHASEINVOICEDATE_ALLOW_BACKDATE, //3
                                                                    Constant.SettingParameter.FN_IS_PURCHASEINVOICEDATE_ALLOW_FUTUREDATE, //4
                                                                    Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE //5
                                                                ));

            string svDueDate = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_DEFAULT_PENGAMBILAN_BERKAS_PEMBAYARAN).ParameterValue;
            hdnIsPpnAllowChanged.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED).FirstOrDefault().ParameterValue;
            
            hdnDateToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnIsAllowBackDate.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_PURCHASEINVOICEDATE_ALLOW_BACKDATE).FirstOrDefault().ParameterValue;
            hdnIsAllowFutureDate.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_PURCHASEINVOICEDATE_ALLOW_FUTUREDATE).FirstOrDefault().ParameterValue;

            hdnIsAPConsignmentFromOrderPerDetailReceive.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN0148).ParameterValue;
            hdnIsUsedProductLine.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE).ParameterValue;

            vSupplierTerm entitySupplierTerm = BusinessLayer.GetvSupplierTermList(String.Format("BusinessPartnerID = {0}", hdnBusinessPartnerID.Value)).FirstOrDefault();

            int termDay = 0;
            if (svDueDate == "0")
            {
                if (entitySupplierTerm != null)
                {
                    termDay = entitySupplierTerm.TermDay;
                }
            }
            else
            {
                termDay = Convert.ToInt32(svDueDate);
            }

            if (hdnIsUsedProductLine.Value == "1")
            {
                trProductLine.Style.Remove("display");
                lblProductLine.Attributes.Add("class", "lblLink lblMandatory");

                trProductLineDt.Style.Add("display", "none");
            }
            else
            {
                trProductLine.Style.Add("display", "none");
                lblProductLine.Attributes.Remove("class");

                trProductLineDt.Style.Remove("display");
            }

            SetControlProperties();

            hdnPPNPctg.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue;
            txtPpnPercentage.Text = hdnPPNPctg.Value;

            BindGridView(1, true, ref PageCount, false);
            BindGridViewCN(1, true, ref PageCount, false);

            Helper.SetControlEntrySetting(txtDueDate, new ControlEntrySetting(true, true, false), "mpEntry");
            Helper.SetControlEntrySetting(txtInvoiceDate, new ControlEntrySetting(true, true, false), "mpEntry");
            Helper.SetControlEntrySetting(txtPurchaseInvoiceDate, new ControlEntrySetting(true, true, false), "mpEntry");
            Helper.SetControlEntrySetting(txtTaxInvoiceDate, new ControlEntrySetting(true, true, false), "mpEntry");
            Helper.SetControlEntrySetting(txtInvoiceNo, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtTaxInvoiceNo, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, true), "mpEntryPopup");

            txtDueDate.Text = DateTime.Today.AddDays(termDay).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtInvoiceDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTaxInvoiceDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPurchaseInvoiceDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDocumentDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string filterPPHType = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.PPH_TYPE);
            List<StandardCode> lstPPHType = BusinessLayer.GetStandardCodeList(filterPPHType);

            Methods.SetComboBoxField<StandardCode>(cboPPHType1, lstPPHType.Where(a => a.ParentID == Constant.StandardCode.PPH_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboPPHType1.SelectedIndex = 0;

            //cboPPHOptions1.Items.Add("Plus");
            //cboPPHOptions1.Items.Add("Minus");
            //cboPPHOptions1.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboPPHType2, lstPPHType.Where(a => a.StandardCodeID == Constant.PPHType.PPH_2).ToList(), "StandardCodeName", "StandardCodeID");
            cboPPHType2.SelectedIndex = 0;

            //cboPPHOptions2.Items.Add("Plus");
            //cboPPHOptions2.Items.Add("Minus");
            //cboPPHOptions2.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboPPHType3, lstPPHType.Where(a => a.StandardCodeID == Constant.PPHType.PPH_3).ToList(), "StandardCodeName", "StandardCodeID");
            cboPPHType3.SelectedIndex = 0;

            //cboPPHOptions3.Items.Add("Plus");
            //cboPPHOptions3.Items.Add("Minus");
            //cboPPHOptions3.SelectedIndex = 0;

            hdnDueDate.Value = txtDueDate.Text;

            hdnIsDiscountAppliedToAveragePrice.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE).ParameterValue;
            hdnIsDiscountAppliedToUnitPrice.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE).ParameterValue;
            hdnIsProcessInvoiceCanChangeAveragePrice.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PROCESS_INVOICE_CAN_CHANGE_AVERAGE_PRICE).ParameterValue;

        }

        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.CURRENCY_CODE));
            List<Term> listTerm = BusinessLayer.GetTermList(string.Format("IsDeleted = 0"));
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnPurchaseInvoiceID, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(txtPurchaseInvoiceDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtDueDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtDocumentDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtTaxInvoiceDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnTotalAmount, new ControlEntrySetting(true, true, true, "0.00"));
            SetControlEntrySetting(txtTotalAmount, new ControlEntrySetting(false, false, true, "0.00"));
            SetControlEntrySetting(hdnTotalCNAmount, new ControlEntrySetting(true, true, true, "0.00"));
            SetControlEntrySetting(txtCreditNoteAmount, new ControlEntrySetting(false, false, false, "0.00"));
            SetControlEntrySetting(hdnFinalDiscountPctg, new ControlEntrySetting(true, true, true, "0.00"));
            SetControlEntrySetting(hdnFinalDiscountText, new ControlEntrySetting(true, true, true, "0.00"));
            SetControlEntrySetting(chkDiscountPercent, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(txtFinalDiscountPIPctg, new ControlEntrySetting(true, true, false, "0.00"));
            SetControlEntrySetting(txtFinalDIscountPI, new ControlEntrySetting(true, true, false, "0.00"));
            SetControlEntrySetting(chkPPN, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(txtPPNPI, new ControlEntrySetting(false, false, false, "0.00"));
            SetControlEntrySetting(chkPPHPercent1, new ControlEntrySetting(true, true, false, true));
            //SetControlEntrySetting(cboPPHOptions1, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPPHPIPctg1, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPPHPI1, new ControlEntrySetting(false, false, false, "0.00"));
            SetControlEntrySetting(txtChargesPI, new ControlEntrySetting(true, true, false, "0.00"));
            SetControlEntrySetting(txtStampPI, new ControlEntrySetting(true, true, false, "0.00"));
            SetControlEntrySetting(txtGrandTotalPI, new ControlEntrySetting(false, false, false, "0.00"));
            SetControlEntrySetting(hdnTransactionStatus, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(hdnPPHPctg, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnPPHText, new ControlEntrySetting(true, true, true, "0.00"));
            SetControlEntrySetting(hdnStampPI, new ControlEntrySetting(true, true, true, "0.00"));
            SetControlEntrySetting(hdnChargesPI, new ControlEntrySetting(true, true, true, "0.00"));
            SetControlEntrySetting(txtStampAmount, new ControlEntrySetting(true, true, false, "0.00"));
            SetControlEntrySetting(txtVAT, new ControlEntrySetting(false, false, false, "0.00"));
            SetControlEntrySetting(txtTransactionAmount, new ControlEntrySetting(true, true, false, "0.00"));
            //SetControlEntrySetting(txtDiscTransAmount, new ControlEntrySetting(false, false, false, "0.00"));
            SetControlEntrySetting(txtDiscountAmount, new ControlEntrySetting(true, true, false, "0.00"));
            SetControlEntrySetting(txtDownPayment, new ControlEntrySetting(true, true, false, "0.00"));
            SetControlEntrySetting(txtChargesAmount, new ControlEntrySetting(true, true, false, "0.00"));
            SetControlEntrySetting(txtCreditNote, new ControlEntrySetting(false, false, false, "0.00"));
            SetControlEntrySetting(txtRemarksDt, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRoundingHdAmount, new ControlEntrySetting(true, true, false, "0.00"));


            if (hdnIsUsedProductLine.Value == "1")
            {
                SetControlEntrySetting(lblProductLine, new ControlEntrySetting(true, false));
                SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(hdnProductLineItemType, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, true));
            }
            else
            {
                SetControlEntrySetting(lblProductLine, new ControlEntrySetting(true, true));
                SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(hdnProductLineItemType, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, false));
            }

            BusinessPartners entityBP = BusinessLayer.GetBusinessPartners(Convert.ToInt32(hdnBusinessPartnerID.Value));
            List<SettingParameterDt> lstPPH = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.PPH_PERCENTAGE, Constant.SettingParameter.PPH_PERCENTAGE_WITHOUT_NPWP));
            if (entityBP != null)
            {
                if (entityBP.VATRegistrationNo != null && entityBP.VATRegistrationNo != "")
                {
                    txtPPHPIPctg1.Text = lstPPH.Where(a => a.ParameterCode == Constant.SettingParameter.PPH_PERCENTAGE).FirstOrDefault().ParameterValue;
                    hdnPPHPctg.Value = lstPPH.Where(a => a.ParameterCode == Constant.SettingParameter.PPH_PERCENTAGE).FirstOrDefault().ParameterValue;
                }
                else
                {
                    txtPPHPIPctg1.Text = lstPPH.Where(a => a.ParameterCode == Constant.SettingParameter.PPH_PERCENTAGE_WITHOUT_NPWP).FirstOrDefault().ParameterValue;
                    hdnPPHPctg.Value = lstPPH.Where(a => a.ParameterCode == Constant.SettingParameter.PPH_PERCENTAGE_WITHOUT_NPWP).FirstOrDefault().ParameterValue;
                }

            }
        }

        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            hdnIsEditableCustom.Value = "1";
            hdnFinalDiscountPctg.Value = "0";
            chkPPN.Checked = false;
            hdnPPNPctg.Value = txtPpnPercentage.Text;

            //BusinessPartners entityBP = BusinessLayer.GetBusinessPartners(Convert.ToInt32(hdnBusinessPartnerID.Value));
            //List<SettingParameterDt> lstPPH = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.PPH_PERCENTAGE, Constant.SettingParameter.PPH_PERCENTAGE_WITHOUT_NPWP));
            //if (entityBP != null)
            //{
            //    if (entityBP.VATRegistrationNo != null && entityBP.VATRegistrationNo != "")
            //    {
            //        txtPPHPIPctg1.Text = lstPPH.Where(a => a.ParameterCode == Constant.SettingParameter.PPH_PERCENTAGE).FirstOrDefault().ParameterValue;
            //        hdnPPHPctg.Value = lstPPH.Where(a => a.ParameterCode == Constant.SettingParameter.PPH_PERCENTAGE).FirstOrDefault().ParameterValue;
            //    }
            //    else
            //    {
            //        txtPPHPIPctg1.Text = lstPPH.Where(a => a.ParameterCode == Constant.SettingParameter.PPH_PERCENTAGE_WITHOUT_NPWP).FirstOrDefault().ParameterValue;
            //        hdnPPHPctg.Value = lstPPH.Where(a => a.ParameterCode == Constant.SettingParameter.PPH_PERCENTAGE_WITHOUT_NPWP).FirstOrDefault().ParameterValue;
            //    }

            //}
        }

        protected string IsEditable()
        {
            return hdnIsEditableCustom.Value;
        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            return string.Format("BusinessPartnerID = {0}", AppSession.BusinessPartnerID);
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvPurchaseInvoiceHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vPurchaseInvoiceHd entity = BusinessLayer.GetvPurchaseInvoiceHd(filterExpression, PageIndex, "PurchaseInvoiceID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvPurchaseInvoiceHdRowIndex(filterExpression, keyValue, "PurchaseInvoiceID DESC");
            vPurchaseInvoiceHd entity = BusinessLayer.GetvPurchaseInvoiceHd(filterExpression, PageIndex, "PurchaseInvoiceID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPurchaseInvoiceHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
                hdnPrintStatus.Value = "true";
            else
                hdnPrintStatus.Value = "false";

            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                hdnIsEditableCustom.Value = "0";
                if (entity.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                {
                    SetControlEntrySetting(chkDiscountPercent, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(txtFinalDiscountPIPctg, new ControlEntrySetting(false, false, false, "0.00"));
                    SetControlEntrySetting(txtFinalDIscountPI, new ControlEntrySetting(false, false, false, "0.00"));
                    SetControlEntrySetting(chkPPN, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(chkPPHPercent1, new ControlEntrySetting(false, false, false));
                    //SetControlEntrySetting(cboPPHOptions1, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(txtPPNPI, new ControlEntrySetting(false, false, false, "0.00"));
                    SetControlEntrySetting(txtPPHPIPctg1, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(txtPPHPI1, new ControlEntrySetting(false, false, false, "0.00"));
                    SetControlEntrySetting(txtChargesPI, new ControlEntrySetting(false, false, false, "0.00"));
                    SetControlEntrySetting(txtStampPI, new ControlEntrySetting(false, false, false, "0.00"));
                    SetControlEntrySetting(txtRoundingHdAmount, new ControlEntrySetting(false, false, false));
                }
                txtPpnPercentage.Enabled = false;

                txtRemarks.Enabled = false;
                txtFinalDiscountPIPctg.Enabled = false;
                chkPPN.Enabled = false;
                cboPPHType1.Enabled = false;
                //cboPPHOptions1.Enabled = false;
                txtPPHPIPctg1.Enabled = false;
            }
            else
            {
                isShowWatermark = false;
                hdnIsEditableCustom.Value = "1";
                txtPpnPercentage.Enabled = true;

                txtRemarks.Enabled = true;
                txtFinalDiscountPIPctg.Enabled = true;
                chkPPN.Enabled = true;
                cboPPHType1.Enabled = true;
                //cboPPHOptions1.Enabled = true;
                txtPPHPIPctg1.Enabled = true;
            }

            hdnPurchaseInvoiceID.Value = entity.PurchaseInvoiceID.ToString();
            txtPurchaseInvoiceNo.Text = entity.PurchaseInvoiceNo;
            txtPurchaseInvoiceDate.Text = entity.PurchaseInvoiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnPurchaseInvoiceDate.Value = entity.PurchaseInvoiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDueDate.Text = entity.DueDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDocumentDate.Text = entity.DocumentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRemarks.Text = entity.Remarks;
            txtReferenceNo.Text = entity.ReferenceNo;

            if (entity.VATPercentage != 0)
                chkPPN.Checked = true;
            else
                chkPPN.Checked = false;

            if (chkPPN.Checked)
            {
                hdnPPNPctg.Value = entity.VATPercentage.ToString("0.##");
                txtPpnPercentage.Text = entity.VATPercentage.ToString();

                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (hdnIsPpnAllowChanged.Value == "1")
                    {
                        SetControlEntrySetting(txtPpnPercentage, new ControlEntrySetting(true, true, false));
                        txtPpnPercentage.Attributes.Remove("readonly");
                    }
                    else
                    {
                        SetControlEntrySetting(txtPpnPercentage, new ControlEntrySetting(false, false, false));
                        txtPpnPercentage.Attributes.Add("readonly", "readonly");
                    }
                }
                else
                {
                    SetControlEntrySetting(txtPpnPercentage, new ControlEntrySetting(false, false, false));
                    txtPpnPercentage.Attributes.Add("readonly", "readonly");
                }
            }
            else
            {
                txtPpnPercentage.Text = Convert.ToDecimal(hdnPPNPctg.Value).ToString("N2");
            }

            hdnStampPI.Value = entity.StampAmount.ToString();

            chkDiscountPercent.Checked = entity.IsFinalDiscountInPercentage;
            chkPPHPercent1.Checked = entity.IsPPHInPercentage;

            hdnFinalDiscountPctg.Value = entity.FinalDiscount.ToString();
            hdnFinalDiscountText.Value = entity.DiscountValue.ToString();
            txtFinalDiscountPIPctg.Text = entity.FinalDiscount.ToString();
            txtFinalDIscountPI.Text = entity.DiscountValue.ToString();

            cboPPHType1.Value = entity.GCPPHType;

            if (entity.PPHValue != 0)
            {
                if (entity.PPHMode)
                {
                    //cboPPHOptions1.Text = "Plus";
                    txtPPHMode.Text = "Plus";
                }
                else
                {
                    //cboPPHOptions1.Text = "Minus";
                    txtPPHMode.Text = "Minus";
                }
            }
            else
            {
                txtPPHMode.Text = "";
            }

            hdnPPHPctg.Value = entity.PPHPercentage.ToString();
            hdnPPHText.Value = entity.PPHValue.ToString();

            txtPPHPIPctg1.Text = entity.PPHPercentage.ToString();
            txtPPHPI1.Text = entity.PPHValue.ToString();

            hdnChargesPI.Value = entity.ChargesAmount.ToString();

            hdnTotalDPPHeader.Value = entity.TotalDPP.ToString();
            hdnTotalAmount.Value = entity.TotalTransactionAmount.ToString();
            hdnTotalCNAmount.Value = entity.TotalCreditNoteAmount.ToString();

            txtRoundingHdAmount.Text = entity.RoundingAmount.ToString();

            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            hdnProductLineItemType.Value = entity.GCItemType;

            hdnTransactionStatus.Value = entity.GCTransactionStatus;
            hdnPageCount.Value = PageCount.ToString();
            BindGridView(1, true, ref PageCount, true);
            BindGridViewCN(1, true, ref PageCount, false);

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

            if (entity.GCTransactionStatus == Constant.TransactionStatus.VOID)
            {
                SetControlEntrySetting(chkDiscountPercent, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkPPN, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkPPHPercent1, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtChargesPI, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtStampPI, new ControlEntrySetting(false, false, false));
                chkDiscountPercent.Checked = false;
                chkPPN.Checked = false;
                chkPPHPercent1.Checked = false;
                SetControlEntrySetting(txtFinalDiscountPIPctg, new ControlEntrySetting(false, false, false, "0.00"));
                SetControlEntrySetting(txtFinalDIscountPI, new ControlEntrySetting(false, false, false, "0.00"));
                SetControlEntrySetting(txtPPHPIPctg1, new ControlEntrySetting(false, false, false, "0.00"));
                SetControlEntrySetting(txtPPHPI1, new ControlEntrySetting(false, false, false, "0.00"));
                SetControlEntrySetting(txtRoundingHdAmount, new ControlEntrySetting(false, false, false));
                cboPPHType1.Value = "";
                //cboPPHOptions1.Value = "";
                txtPPHMode.Text = "";
                txtPPHPIPctg1.Text = "0.00".ToString();
                txtPPHPI1.Text = "0.00".ToString();
                txtChargesPI.Text = "0.00".ToString();
                txtStampPI.Text = "0.00".ToString();
                txtRoundingHdAmount.Text = "0.00".ToString();
                txtGrandTotalPI.Text = "0.00".ToString();
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount, bool isCountTotalAmount)
        {
            string filterExpression = "1 = 0";
            if (hdnPurchaseInvoiceID.Value != "" && hdnPurchaseInvoiceID.Value != "0")
            {
                filterExpression = string.Format("PurchaseInvoiceID = {0} AND CreditNoteID IS NULL AND IsDeleted = 0", hdnPurchaseInvoiceID.Value);

                if (isCountTotalAmount)
                {
                    PurchaseInvoiceHd entity = BusinessLayer.GetPurchaseInvoiceHd(Convert.ToInt32(hdnPurchaseInvoiceID.Value));
                    decimal total = entity.TotalTransactionAmount;
                    hdnTotalAmount.Value = total.ToString();
                }
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseInvoiceDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseInvoiceDt> lstEntity = BusinessLayer.GetvPurchaseInvoiceDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        private void BindGridViewCN(int pageIndex, bool isCountPageCount, ref int pageCount, bool isCountTotalAmount)
        {
            string filterExpression = "1 = 0";
            if (hdnPurchaseInvoiceID.Value != "" && hdnPurchaseInvoiceID.Value != "0")
            {
                filterExpression = string.Format("PurchaseInvoiceID = {0} AND CreditNoteID IS NOT NULL AND IsDeleted = 0", hdnPurchaseInvoiceID.Value);

                if (isCountTotalAmount)
                {
                    PurchaseInvoiceHd entity = BusinessLayer.GetPurchaseInvoiceHd(Convert.ToInt32(hdnPurchaseInvoiceID.Value));
                    decimal total = entity.TotalCreditNoteAmount;
                    hdnTotalCNAmount.Value = total.ToString();
                }
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseInvoiceDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }

            List<vPurchaseInvoiceDt> lstEntity = BusinessLayer.GetvPurchaseInvoiceDtList(filterExpression);
            lvwCN.DataSource = lstEntity;
            lvwCN.DataBind();
        }
        #endregion

        #region Save Edit Header
        public void SavePurchaseInvoiceHd(IDbContext ctx, ref int PurchaseInvoiceID, ref string errorMessage)
        {
            PurchaseInvoiceHdDao entityHdDao = new PurchaseInvoiceHdDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);
            TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.PURCHASE_INVOICE);

            if (hdnPurchaseInvoiceID.Value == "")
            {
                #region Add

                PurchaseInvoiceHd entityHd = new PurchaseInvoiceHd();
                entityHd.PurchaseInvoiceDate = Helper.GetDatePickerValue(txtPurchaseInvoiceDate.Text);
                entityHd.DueDate = Helper.GetDatePickerValue(txtDueDate.Text);
                entityHd.DocumentDate = Helper.GetDatePickerValue(txtDocumentDate.Text);
                entityHd.GCCurrencyCode = "X147^IDR";
                entityHd.GCChargesType = "X157^001";
                entityHd.BusinessPartnerID = AppSession.BusinessPartnerID;
                entityHd.Remarks = txtRemarks.Text;
                if (chkPPN.Checked)
                {
                    entityHd.VATPercentage = Convert.ToDecimal(hdnPPNPctg.Value);
                }
                else
                {
                    entityHd.VATPercentage = 0;
                }
                entityHd.IsFinalDiscountInPercentage = chkDiscountPercent.Checked;
                entityHd.FinalDiscount = Convert.ToDecimal(txtFinalDiscountPIPctg.Text);
                entityHd.FinalDiscountAmount = Convert.ToDecimal(txtFinalDIscountPI.Text);
                entityHd.IsPPHInPercentage = chkPPHPercent1.Checked;
                entityHd.GCPPHType = cboPPHType1.Value.ToString();
                entityHd.PPHPercentage = Convert.ToDecimal(txtPPHPIPctg1.Text);
                entityHd.PPHAmount = Convert.ToDecimal(txtPPHPI1.Text);
                if (entityHd.PPHAmount > 0)
                {
                    entityHd.PPHMode = true;
                }
                else
                {
                    entityHd.PPHMode = false;
                }

                if (hdnIsUsedProductLine.Value == "1")
                {
                    entityHd.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
                }

                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.StampAmount = Convert.ToDecimal(txtStampPI.Text);
                entityHd.ChargesAmount = Convert.ToDecimal(txtChargesPI.Text);
                entityHd.RoundingAmount = Convert.ToDecimal(txtRoundingHdAmount.Text);
                entityHd.TotalNetTransactionAmount = Convert.ToDecimal(Request.Form[txtGrandTotalPI.UniqueID]);
                entityHd.PurchaseInvoiceNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.PURCHASE_INVOICE, entityHd.PurchaseInvoiceDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                PurchaseInvoiceID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                #endregion
            }
            else
            {
                #region Update

                PurchaseInvoiceHd pihd = entityHdDao.Get(Convert.ToInt32(hdnPurchaseInvoiceID.Value));

                pihd.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHdDao.Update(pihd);
                PurchaseInvoiceID = Convert.ToInt32(hdnPurchaseInvoiceID.Value);

                #endregion
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int PurchaseInvoiceID = 0;
                string errorMessage = "";
                SavePurchaseInvoiceHd(ctx, ref PurchaseInvoiceID, ref errorMessage);
                if (String.IsNullOrEmpty(errorMessage))
                {
                    retval = PurchaseInvoiceID.ToString();
                    ctx.CommitTransaction();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                PurchaseInvoiceHd entity = BusinessLayer.GetPurchaseInvoiceHd(Convert.ToInt32(hdnPurchaseInvoiceID.Value));

                bool isLocked = false;
                TransactionTypeLock entityLock = BusinessLayer.GetTransactionTypeLock(Constant.TransactionCode.PURCHASE_INVOICE);
                if (entityLock.LockedUntilDate != null)
                {
                    DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                    DateTime DateNow = entity.PurchaseInvoiceDate;
                    if (DateNow > DateLock)
                    {
                        isLocked = false;
                    }
                    else
                    {
                        isLocked = true;
                    }
                }
                else
                {
                    isLocked = false;
                }

                if (!isLocked)
                {
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.PurchaseInvoiceDate = Helper.GetDatePickerValue(txtPurchaseInvoiceDate.Text);
                        entity.DueDate = Helper.GetDatePickerValue(txtDueDate.Text);
                        entity.DocumentDate = Helper.GetDatePickerValue(txtDocumentDate.Text);
                        entity.GCCurrencyCode = "X147^IDR";
                        entity.GCChargesType = "X157^001";
                        entity.BusinessPartnerID = AppSession.BusinessPartnerID;
                        entity.Remarks = txtRemarks.Text;
                        if (chkPPN.Checked)
                        {
                            entity.VATPercentage = Convert.ToDecimal(hdnPPNPctg.Value);
                        }
                        else
                        {
                            entity.VATPercentage = 0;
                        }
                        entity.IsFinalDiscountInPercentage = chkDiscountPercent.Checked;
                        entity.FinalDiscount = Convert.ToDecimal(txtFinalDiscountPIPctg.Text);
                        entity.FinalDiscountAmount = Convert.ToDecimal(txtFinalDIscountPI.Text);
                        entity.IsPPHInPercentage = chkPPHPercent1.Checked;
                        entity.GCPPHType = cboPPHType1.Value.ToString();
                        entity.PPHPercentage = Convert.ToDecimal(txtPPHPIPctg1.Text);
                        entity.PPHAmount = Convert.ToDecimal(Request.Form[txtPPHPI1.UniqueID]);
                        if (entity.PPHAmount > 0)
                        {
                            entity.PPHMode = true;
                        }
                        else
                        {
                            entity.PPHMode = false;
                        }
                        entity.StampAmount = Convert.ToDecimal(txtStampPI.Text);
                        entity.ChargesAmount = Convert.ToDecimal(txtChargesPI.Text);
                        entity.RoundingAmount = Convert.ToDecimal(txtRoundingHdAmount.Text);
                        entity.TotalNetTransactionAmount = Convert.ToDecimal(Request.Form[txtGrandTotalPI.UniqueID]);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePurchaseInvoiceHd(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = string.Format("Hutang supplier {0} tidak dapat diubah. Harap refresh halaman ini.", entity.PurchaseInvoiceNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        return false;
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
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PurchaseInvoiceHdDao entityDao = new PurchaseInvoiceHdDao(ctx);
            PurchaseInvoiceDtDao entityDtDao = new PurchaseInvoiceDtDao(ctx);
            PurchaseOrderHdDao orderHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao orderDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseReceiveHdDao purchaseHdDao = new PurchaseReceiveHdDao(ctx);
            PurchaseReceiveDtDao purchaseDtDao = new PurchaseReceiveDtDao(ctx);
            PurchaseOrderTermDao poTermDao = new PurchaseOrderTermDao(ctx);
            ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
            SupplierCreditNoteDao entityCNDao = new SupplierCreditNoteDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                PurchaseInvoiceHd entity = BusinessLayer.GetPurchaseInvoiceHd(Convert.ToInt32(hdnPurchaseInvoiceID.Value));

                bool isLocked = false;
                TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.PURCHASE_INVOICE);
                if (entityLock.LockedUntilDate != null)
                {
                    DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                    DateTime DateNow = entity.PurchaseInvoiceDate;
                    if (DateNow > DateLock)
                    {
                        isLocked = false;
                    }
                    else
                    {
                        isLocked = true;
                    }
                }
                else
                {
                    isLocked = false;
                }

                if (!isLocked)
                {
                    if (type.Contains("justvoid"))
                    {
                        #region Just Void

                        string[] param = type.Split(';');
                        string gcDeleteReason = param[1];
                        string reason = param[2];

                        if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            entity.GCVoidReason = gcDeleteReason;
                            entity.VoidReason = reason;
                            entity.VoidBy = AppSession.UserLogin.UserID;
                            entity.VoidDate = DateTime.Now;
                            entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);

                            List<PurchaseInvoiceDt> lstPurchaseInvoiceDt = BusinessLayer.GetPurchaseInvoiceDtList(String.Format("PurchaseInvoiceID = {0} AND IsDeleted = 0", entity.PurchaseInvoiceID), ctx);
                            foreach (PurchaseInvoiceDt purchaseInvoiceDt in lstPurchaseInvoiceDt)
                            {
                                purchaseInvoiceDt.IsDeleted = true;
                                purchaseInvoiceDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(purchaseInvoiceDt);

                                if (purchaseInvoiceDt.PurchaseOrderTermID != 0 && purchaseInvoiceDt.PurchaseOrderTermID != null)
                                {
                                    PurchaseOrderTerm poTerm = poTermDao.Get(Convert.ToInt32(purchaseInvoiceDt.PurchaseOrderTermID));
                                    if (poTerm.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                    {
                                        poTerm.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                        poTerm.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        poTermDao.Update(poTerm);
                                    }
                                }
                                else
                                {
                                    if (purchaseInvoiceDt.PurchaseReceiveID != 0 && purchaseInvoiceDt.PurchaseReceiveID != null)
                                    {
                                        PurchaseReceiveHd entityPR = purchaseHdDao.Get(Convert.ToInt32(purchaseInvoiceDt.PurchaseReceiveID));
                                        if (entityPR.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                        {
                                            entityPR.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                            entityPR.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            purchaseHdDao.Update(entityPR);

                                            List<PurchaseReceiveDt> lstPurchaseReceiveDt = BusinessLayer.GetPurchaseReceiveDtList(String.Format(
                                                                                                            "PurchaseReceiveID IN ({0}) AND GCItemDetailStatus != '{1}'",
                                                                                                            entityPR.PurchaseReceiveID, Constant.TransactionStatus.VOID), ctx);
                                            foreach (PurchaseReceiveDt purchaseDt in lstPurchaseReceiveDt)
                                            {
                                                purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                                purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                purchaseDtDao.Update(purchaseDt);
                                            }
                                        }
                                    }

                                    if (purchaseInvoiceDt.CreditNoteID != 0 && purchaseInvoiceDt.CreditNoteID != null)
                                    {
                                        SupplierCreditNote entityCN = entityCNDao.Get(Convert.ToInt32(purchaseInvoiceDt.CreditNoteID));
                                        if (entityCN.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                        {
                                            entityCN.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                            entityCN.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            entityCNDao.Update(entityCN);
                                        }
                                    }
                                }
                            }

                            retval = entity.PurchaseInvoiceNo;
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Hutang supplier {0} tidak dapat diubah. Harap refresh halaman ini.", entity.PurchaseInvoiceNo);
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }

                        #endregion
                    }
                    else if (type.Contains("approve"))
                    {
                        #region Approve

                        if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            entity.PurchaseInvoiceDate = Helper.GetDatePickerValue(txtPurchaseInvoiceDate.Text);
                            entity.DueDate = Helper.GetDatePickerValue(txtDueDate.Text);
                            entity.DocumentDate = Helper.GetDatePickerValue(txtDocumentDate.Text);
                            entity.GCCurrencyCode = "X147^IDR";
                            entity.GCChargesType = "X157^001";
                            entity.BusinessPartnerID = AppSession.BusinessPartnerID;
                            entity.Remarks = txtRemarks.Text;
                            if (chkPPN.Checked)
                            {
                                entity.VATPercentage = Convert.ToDecimal(hdnPPNPctg.Value);
                            }
                            else
                            {
                                entity.VATPercentage = 0;
                            }
                            entity.IsFinalDiscountInPercentage = chkDiscountPercent.Checked;
                            entity.FinalDiscount = Convert.ToDecimal(txtFinalDiscountPIPctg.Text);
                            entity.FinalDiscountAmount = Convert.ToDecimal(txtFinalDIscountPI.Text);
                            entity.IsPPHInPercentage = chkPPHPercent1.Checked;
                            entity.GCPPHType = cboPPHType1.Value.ToString();
                            entity.PPHPercentage = Convert.ToDecimal(txtPPHPIPctg1.Text);
                            entity.PPHAmount = Convert.ToDecimal(Request.Form[txtPPHPI1.UniqueID]);
                            if (entity.PPHAmount > 0)
                            {
                                entity.PPHMode = true;
                            }
                            else
                            {
                                entity.PPHMode = false;
                            }
                            entity.StampAmount = Convert.ToDecimal(txtStampPI.Text);
                            entity.ChargesAmount = Convert.ToDecimal(txtChargesPI.Text);
                            entity.RoundingAmount = Convert.ToDecimal(txtRoundingHdAmount.Text);
                            entity.TotalNetTransactionAmount = Convert.ToDecimal(Request.Form[txtGrandTotalPI.UniqueID]);

                            entity.ApprovedBy = AppSession.UserLogin.UserID;
                            entity.ApprovedDate = DateTime.Now;
                            entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);

                            List<PurchaseInvoiceDt> lstPurchaseInvoiceDt = BusinessLayer.GetPurchaseInvoiceDtList(String.Format(
                                                        "PurchaseInvoiceID = {0} AND IsDeleted = 0", entity.PurchaseInvoiceID), ctx);
                            foreach (PurchaseInvoiceDt purchaseInvoiceDt in lstPurchaseInvoiceDt)
                            {
                                if (purchaseInvoiceDt.PurchaseOrderTermID != 0 && purchaseInvoiceDt.PurchaseOrderTermID != null)
                                {
                                    PurchaseOrderTerm poTerm = poTermDao.Get(Convert.ToInt32(purchaseInvoiceDt.PurchaseOrderTermID));
                                    poTerm.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                    poTerm.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    poTermDao.Update(poTerm);

                                    if (poTerm.IsPurchaseReceiveRequired)
                                    {
                                        string filterPORDT = string.Format("PurchaseOrderID = {0} AND GCItemDetailStatus = '{1}'", poTerm.PurchaseOrderID, Constant.TransactionStatus.APPROVED);
                                        List<PurchaseReceiveDt> lstPORDT = BusinessLayer.GetPurchaseReceiveDtList(filterPORDT, ctx);
                                        foreach (PurchaseReceiveDt porDT in lstPORDT)
                                        {
                                            porDT.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                                            porDT.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            purchaseDtDao.Update(porDT);

                                            string filterPORDTforHDALL = string.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", porDT.PurchaseReceiveID, Constant.TransactionStatus.VOID);
                                            List<PurchaseReceiveDt> lstDTforHDALL = BusinessLayer.GetPurchaseReceiveDtList(filterPORDTforHDALL, ctx);

                                            string filterPORDTforHDClosed = string.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus = '{1}'", porDT.PurchaseReceiveID, Constant.TransactionStatus.APPROVED);
                                            List<PurchaseReceiveDt> lstDTforHDClosed = BusinessLayer.GetPurchaseReceiveDtList(filterPORDTforHDClosed, ctx);

                                            PurchaseReceiveHd porHD = purchaseHdDao.Get(porDT.PurchaseReceiveID);
                                            if (lstDTforHDALL.Count() == lstDTforHDClosed.Count())
                                            {
                                                porHD.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                            }
                                            else
                                            {
                                                porHD.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                            }
                                            porHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            purchaseHdDao.Update(porHD);
                                        }

                                    }
                                }
                                else
                                {
                                    if (purchaseInvoiceDt.PurchaseReceiveID != 0 && purchaseInvoiceDt.PurchaseReceiveID != null)
                                    {
                                        PurchaseReceiveHd purchaseReceiveHd = purchaseHdDao.Get(Convert.ToInt32(purchaseInvoiceDt.PurchaseReceiveID));
                                        purchaseReceiveHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                        purchaseReceiveHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        purchaseHdDao.Update(purchaseReceiveHd);

                                        List<PurchaseReceiveDt> lstPurchaseReceiveDt = BusinessLayer.GetPurchaseReceiveDtList(String.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", purchaseReceiveHd.PurchaseReceiveID, Constant.TransactionStatus.VOID), ctx);
                                        foreach (PurchaseReceiveDt purchaseDt in lstPurchaseReceiveDt)
                                        {
                                            purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                                            purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            purchaseDtDao.Update(purchaseDt);
                                        }
                                    }

                                    if (purchaseInvoiceDt.CreditNoteID != 0 && purchaseInvoiceDt.CreditNoteID != null)
                                    {
                                        SupplierCreditNote entityCN = entityCNDao.Get(Convert.ToInt32(purchaseInvoiceDt.CreditNoteID));
                                        entityCN.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                        entityCN.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityCNDao.Update(entityCN);
                                    }

                                    if (purchaseInvoiceDt.PurchaseOrderID != 0 && purchaseInvoiceDt.PurchaseOrderID != null)
                                    {
                                        PurchaseOrderHd purchaseOrderHd = orderHdDao.Get(Convert.ToInt32(purchaseInvoiceDt.PurchaseOrderID));
                                        if (purchaseOrderHd.TransactionCode == Constant.TransactionCode.CONSIGNMENT_ORDER)
                                        {
                                            purchaseOrderHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                            purchaseOrderHd.ClosedBy = AppSession.UserLogin.UserID;
                                            purchaseOrderHd.ClosedDate = DateTime.Now;
                                            purchaseOrderHd.GCClosedReason = Constant.POClosedReason.OTHER;
                                            purchaseOrderHd.ClosedReason = string.Format("Purchase Invoice {0} Approved", entity.PurchaseInvoiceNo);
                                            purchaseOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            orderHdDao.Update(purchaseOrderHd);

                                            List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(String.Format("PurchaseOrderID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0", purchaseOrderHd.PurchaseOrderID, Constant.TransactionStatus.VOID), ctx);
                                            foreach (PurchaseOrderDt orderDt in lstPurchaseOrderDt)
                                            {
                                                orderDt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                orderDtDao.Update(orderDt);
                                            }
                                        }
                                    }
                                }
                            }
                            retval = entity.PurchaseInvoiceNo;
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Hutang supplier {0} tidak dapat diubah. Harap refresh halaman ini.", entity.PurchaseInvoiceNo);
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }

                        #endregion
                    }
                }
                else
                {
                    result = false;
                    errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
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

        #region callBack Trigger
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            //hdnTotalAmount.Value = hdnTotalAmountBeforeDP.Value = "0";
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount, false);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount, true);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewCN_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewCN(Convert.ToInt32(param[1]), false, ref pageCount, false);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewCN(1, true, ref pageCount, true);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int PurchaseInvoiceID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    PurchaseInvoiceID = Convert.ToInt32(hdnPurchaseInvoiceID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref PurchaseInvoiceID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                PurchaseInvoiceID = Convert.ToInt32(hdnPurchaseInvoiceID.Value);
                if (OnDeleteEntityDt(ref errMessage, PurchaseInvoiceID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpPurchaseInvoiceID"] = PurchaseInvoiceID.ToString();
        }

        private void ControlToEntity(PurchaseInvoiceDt entityDt)
        {
            entityDt.TransactionAmount = Convert.ToDecimal(Request.Form[txtTransactionAmount.UniqueID]);
            entityDt.FinalDiscountAmount = Convert.ToDecimal(Request.Form[txtDiscountAmount.UniqueID]);
            entityDt.VATAmount = Convert.ToDecimal(Request.Form[txtVAT.UniqueID]);
            entityDt.ChargesAmount = Convert.ToDecimal(Request.Form[txtChargesAmount.UniqueID]);
            entityDt.StampAmount = Convert.ToDecimal(Request.Form[txtStampAmount.UniqueID]);
            entityDt.DownPaymentAmount = Convert.ToDecimal(Request.Form[txtDownPayment.UniqueID]);
            entityDt.CreditNoteAmount = Convert.ToDecimal(Request.Form[txtCreditNote.UniqueID]);

            if (!String.IsNullOrEmpty(txtTaxInvoiceNoPref.Text))
            {
                entityDt.TaxInvoiceNo = string.Format("{0}|{1}", txtTaxInvoiceNoPref.Text, txtTaxInvoiceNo.Text);
            }
            else
            {
                entityDt.TaxInvoiceNo = txtTaxInvoiceNo.Text;
            }

            entityDt.TaxInvoiceDate = Helper.GetDatePickerValue(txtTaxInvoiceDate.Text);
            entityDt.Remarks = txtRemarksDt.Text;

            if (hdnIsUsedProductLine.Value == "1")
            {
                entityDt.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }
            else
            {
                if (string.IsNullOrEmpty(txtProductLineDtCode.Text))
                {
                    entityDt.ProductLineID = null;
                }
                else
                {
                    entityDt.ProductLineID = Convert.ToInt32(hdnProductLineDtID.Value);
                }
            }

            entityDt.DepartmentID = cboDepartment.Value.ToString();
            if (string.IsNullOrEmpty(txtServiceUnitCode.Text))
            {
                entityDt.ServiceUnitID = null;
            }
            else
            {
                entityDt.ServiceUnitID = Convert.ToInt32(hdnServiceUnitID.Value);
            }
            entityDt.ReferenceNo = txtInvoiceNo.Text;
            entityDt.ReferenceDate = Helper.GetDatePickerValue(txtInvoiceDate.Text);
            if (txtRoundingDtAmount.Text != null && txtRoundingDtAmount.Text != "")
            {
                entityDt.RoundingAmount = Convert.ToDecimal(txtRoundingDtAmount.Text);
            }
            entityDt.LineAmount = entityDt.TransactionAmount - entityDt.DiscountAmount - entityDt.FinalDiscountAmount + entityDt.VATAmount + entityDt.PPH23Amount + entityDt.PPH25Amount + entityDt.StampAmount + entityDt.ChargesAmount - entityDt.CreditNoteAmount + entityDt.RoundingAmount;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int PurchaseInvoiceID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseInvoiceHdDao entityHdDao = new PurchaseInvoiceHdDao(ctx);
            PurchaseInvoiceDtDao entityDtDao = new PurchaseInvoiceDtDao(ctx);
            try
            {
                string errorMessage = "";
                SavePurchaseInvoiceHd(ctx, ref PurchaseInvoiceID, ref errorMessage);
                if (String.IsNullOrEmpty(errorMessage))
                {
                    if (entityHdDao.Get(PurchaseInvoiceID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PurchaseInvoiceDt entityDt = new PurchaseInvoiceDt();
                        ControlToEntity(entityDt);
                        entityDt.PurchaseInvoiceID = PurchaseInvoiceID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Hutang supplier {0} tidak dapat diubah. Harap refresh halaman ini.", entityHdDao.Get(PurchaseInvoiceID).PurchaseInvoiceNo);
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseInvoiceHdDao entityHdDao = new PurchaseInvoiceHdDao(ctx);
            PurchaseInvoiceDtDao entityDtDao = new PurchaseInvoiceDtDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                PurchaseInvoiceDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                PurchaseInvoiceHd entityHd = entityHdDao.Get(entityDt.PurchaseInvoiceID);

                bool isLocked = false;
                TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.PURCHASE_INVOICE);
                if (entityLock.LockedUntilDate != null)
                {
                    DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                    DateTime DateNow = entityHd.PurchaseInvoiceDate;
                    if (DateNow > DateLock)
                    {
                        isLocked = false;
                    }
                    else
                    {
                        isLocked = true;
                    }
                }
                else
                {
                    isLocked = false;
                }

                if (!isLocked)
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN && entityDt.IsDeleted == false)
                    {
                        ControlToEntity(entityDt);
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Hutang supplier {0} tidak dapat diubah. Harap refresh halaman ini.", entityHd.PurchaseInvoiceNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
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
            PurchaseInvoiceHdDao entityHdDao = new PurchaseInvoiceHdDao(ctx);
            PurchaseInvoiceDtDao entityDtDao = new PurchaseInvoiceDtDao(ctx);
            PurchaseOrderTermDao poTermDao = new PurchaseOrderTermDao(ctx);
            PurchaseReceiveHdDao purchaseHdDao = new PurchaseReceiveHdDao(ctx);
            PurchaseReceiveDtDao purchaseDtDao = new PurchaseReceiveDtDao(ctx);
            SupplierCreditNoteDao entityCNDao = new SupplierCreditNoteDao(ctx);
            TestPartnerTransactionHdDao entityTPTHdDao = new TestPartnerTransactionHdDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                PurchaseInvoiceDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                PurchaseInvoiceHd entityHd = entityHdDao.Get(entityDt.PurchaseInvoiceID);

                bool isLocked = false;
                TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.PURCHASE_INVOICE);
                if (entityLock.LockedUntilDate != null)
                {
                    DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                    DateTime DateNow = entityHd.PurchaseInvoiceDate;
                    if (DateNow > DateLock)
                    {
                        isLocked = false;
                    }
                    else
                    {
                        isLocked = true;
                    }
                }
                else
                {
                    isLocked = false;
                }

                if (!isLocked)
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN && entityDt.IsDeleted == false)
                    {
                        entityDt.IsDeleted = true;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);

                        if (entityDt.PurchaseOrderTermID != 0 && entityDt.PurchaseOrderTermID != null)
                        {
                            PurchaseOrderTerm poTerm = poTermDao.Get(Convert.ToInt32(entityDt.PurchaseOrderTermID));
                            if (poTerm.GCTransactionStatus != Constant.TransactionStatus.VOID)
                            {
                                poTerm.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                poTerm.LastUpdatedBy = AppSession.UserLogin.UserID;
                                poTermDao.Update(poTerm);
                            }
                        }
                        else if (entityDt.TestPartnerTransactionID != 0 && entityDt.TestPartnerTransactionID != null)
                        {
                            TestPartnerTransactionHd tptHd = entityTPTHdDao.Get(Convert.ToInt32(entityDt.TestPartnerTransactionID));
                            if (tptHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                            {
                                tptHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                tptHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityTPTHdDao.Update(tptHd);
                            }
                        }
                        else
                        {
                            if (entityDt.PurchaseReceiveID != 0 && entityDt.PurchaseReceiveID != null)
                            {
                                PurchaseReceiveHd entityPR = purchaseHdDao.Get(Convert.ToInt32(entityDt.PurchaseReceiveID));
                                if (entityPR.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                {
                                    entityPR.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                    entityPR.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    purchaseHdDao.Update(entityPR);

                                    List<PurchaseReceiveDt> lstPurchaseReceiveDt = BusinessLayer.GetPurchaseReceiveDtList(String.Format(
                                                                                                    "PurchaseReceiveID IN ({0}) AND GCItemDetailStatus != '{1}'",
                                                                                                    entityPR.PurchaseReceiveID, Constant.TransactionStatus.VOID), ctx);
                                    foreach (PurchaseReceiveDt purchaseDt in lstPurchaseReceiveDt)
                                    {
                                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        purchaseDtDao.Update(purchaseDt);
                                    }
                                }
                            }

                            if (entityDt.CreditNoteID != 0 && entityDt.CreditNoteID != null)
                            {
                                SupplierCreditNote entityCN = entityCNDao.Get(Convert.ToInt32(entityDt.CreditNoteID));
                                if (entityCN.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                {
                                    entityCN.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                    entityCN.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityCNDao.Update(entityCN);
                                }
                            }
                        }

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Hutang supplier {0} tidak dapat diubah. Harap refresh halaman ini.", entityHd.PurchaseInvoiceNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
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
}