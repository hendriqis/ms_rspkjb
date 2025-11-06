using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class JournalTemplateDtEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnTemplateID.Value = param;
            JournalTemplateHd entity = BusinessLayer.GetJournalTemplateHd(Convert.ToInt32(hdnTemplateID.Value));
            txtTemplateCode.Text = entity.TemplateCode;
            txtTemplateName.Text = entity.TemplateName;
            if (entity.GCJournalTemplateType == "" || entity.GCJournalTemplateType == null)
            {
                txtTemplateType.Text = "";
                trAmountPercentage.Style.Add("display", "table-row");
                trAmount.Style.Add("display", "none");
            }
            else
            {
                StandardCode std = BusinessLayer.GetStandardCode(entity.GCJournalTemplateType);
                txtTemplateType.Text = std.StandardCodeName;

                if (std.StandardCodeID == Constant.JournalTemplateType.ALOKASI)
                {
                    trAmountPercentage.Style.Add("display", "table-row");
                    trAmount.Style.Add("display", "none");
                }
                else
                {
                    trAmountPercentage.Style.Add("display", "none");
                    trAmount.Style.Add("display", "table-row");
                }
            }

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
            hdnHealthcare.Value = "001";
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

            List<Variable> lstPosition = new List<Variable>();
            lstPosition.Add(new Variable { Code = "D", Value = GetLabel("Debit") });
            lstPosition.Add(new Variable { Code = "K", Value = GetLabel("Kredit") });
            Methods.SetRadioButtonListField<Variable>(rblPosition, lstPosition, "Value", "Code");

            BindGridView();
            
            txtGLAccountCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtSubLedgerDtCode.Attributes.Add("validationgroup", "mpEntryPopup");
            cboHealthcare.Attributes.Add("validationgroup", "mpEntryPopup");
            txtDepartmentID.Attributes.Add("validationgroup", "mpEntryPopup");
            txtServiceUnitCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtBusinessPartnerCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtAmountPercentage.Attributes.Add("validationgroup", "mpEntryPopup");
            txtAmount.Attributes.Add("validationgroup", "mpTrxPopup");
            txtDisplayOrder.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            hdnHealthcare.Value = "001";
            string filterExpression = string.Format("TemplateID = {0} AND IsDeleted = 0 ORDER BY DisplayOrder", hdnTemplateID.Value);
            List<vJournalTemplateDt> lstEntity = BusinessLayer.GetvJournalTemplateDtList(filterExpression);
            grdViewD.DataSource = lstEntity.Where(p => p.Position == "D").ToList();
            grdViewD.DataBind();

            grdViewK.DataSource = lstEntity.Where(p => p.Position == "K").ToList();
            grdViewK.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
            
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(JournalTemplateDt entity)
        {
            entity.GLAccountID = Convert.ToInt32(hdnGLAccountID.Value);
            if (hdnSubLedgerDtID.Value != "" && hdnSubLedgerDtID.Value != "0")
                entity.SubLedgerID = Convert.ToInt32(hdnSubLedgerDtID.Value);
            else
                entity.SubLedgerID = null;

            entity.HealthcareID = "001";
            entity.DepartmentID = hdnDepartmentID.Value;

            if (hdnServiceUnitID.Value != "" && hdnServiceUnitID.Value != "0")
            {
                entity.ServiceUnitID = Convert.ToInt32(hdnServiceUnitID.Value);
            }
            else
            {
                entity.ServiceUnitID = null;
            }

            if (hdnRevenueCostCenterID.Value != "" && hdnRevenueCostCenterID.Value != "0")
            {
                entity.RevenueCostCenterID = Convert.ToInt32(hdnRevenueCostCenterID.Value);
            }
            else
            {
                entity.RevenueCostCenterID = null;
            }

            if (hdnCustomerGroupID.Value != "" && hdnCustomerGroupID.Value != "0")
            {
                entity.CustomerGroupID = Convert.ToInt32(hdnCustomerGroupID.Value);
            }
            else
            {
                entity.CustomerGroupID = null;
            }

            if (hdnBusinessPartnerID.Value != "" && hdnBusinessPartnerID.Value != "0")
            {
                entity.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerID.Value);
            }
            else
            {
                entity.BusinessPartnerID = null;
            }

            entity.AmountPercentage = Convert.ToDecimal(txtAmountPercentage.Text);
            entity.Amount = Convert.ToDecimal(txtAmount.Text);
            entity.DisplayOrder = Convert.ToInt16(txtDisplayOrder.Text);
            entity.Position = Request.Form[rblPosition.UniqueID];
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                JournalTemplateDt entity = new JournalTemplateDt();
                ControlToEntity(entity);
                entity.TemplateID = Convert.ToInt32(hdnTemplateID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertJournalTemplateDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                JournalTemplateDt entity = BusinessLayer.GetJournalTemplateDt(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateJournalTemplateDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                JournalTemplateDt entity = BusinessLayer.GetJournalTemplateDt(Convert.ToInt32(hdnEntryID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateJournalTemplateDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}