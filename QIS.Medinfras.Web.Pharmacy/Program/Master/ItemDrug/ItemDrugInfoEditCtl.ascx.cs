using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class ItemDrugInfoEditCtl : BaseEntryPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            hdnItemID.Value = param;

            SetControlProperties();

            vDrugInfo entityDrug = BusinessLayer.GetvDrugInfoList(string.Format("ItemID = {0}", param)).FirstOrDefault();
            EntityToControl(entityDrug);
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}') AND IsDeleted = 0",
                                                        Constant.StandardCode.ITEM_UNIT,
                                                        Constant.StandardCode.DRUG_FORM,
                                                        Constant.StandardCode.DRUG_CLASSIFICATION,
                                                        Constant.StandardCode.PREGNANCY_CATEGORY,
                                                        Constant.StandardCode.STATUS_VEN,
                                                        Constant.StandardCode.MEDICATION_ROUTE,
                                                        Constant.StandardCode.COENAM_RULE);

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            Methods.SetComboBoxField<StandardCode>(cboDose, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDrugForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDrugClassification, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_CLASSIFICATION).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPregnancyCategory, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PREGNANCY_CATEGORY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboStatusVEN, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.STATUS_VEN).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            #region Drug Information
            SetControlEntrySetting(txtGenericName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboDose, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDose, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(cboDrugForm, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnMIMSClassID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtMIMSClassCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMIMSClassName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnATCClassID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtATCClassCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtATCClassName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboDrugClassification, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboCoenamRule, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnDefaultSignaID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSignaLabel, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSignaName1, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboStatusVEN, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboPregnancyCategory, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsGeneric, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsFormularium, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsBPJSFormularium, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsExternalMedication, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHasRestrictionInformation, new ControlEntrySetting(true, true, false));
            #endregion
        }

        private void EntityToControl(vDrugInfo entityDrug)
        {
            #region Drug Information
            if (entityDrug != null)
            {
                txtGenericName.Text = entityDrug.GenericName;
                txtDose.Text = entityDrug.Dose.ToString();
                cboDose.Text = entityDrug.GCDoseUnit;
                cboDrugForm.Value = entityDrug.GCDrugForm;
                cboCoenamRule.Value = entityDrug.GCCoenamRule;
                cboMedicationRoute.Value = entityDrug.GCMedicationRoute;
                cboStatusVEN.Value = entityDrug.GCStatusVEN;
                hdnMIMSClassID.Value = entityDrug.MIMSClassID.ToString();
                txtMIMSClassCode.Text = entityDrug.MIMSClassCode;
                txtMIMSClassName.Text = entityDrug.MIMSClassName;
                hdnATCClassID.Value = entityDrug.ATCClassID.ToString();
                txtATCClassCode.Text = entityDrug.ATCClassCode;
                txtATCClassName.Text = entityDrug.ATCClassName;
                cboDrugClassification.Value = entityDrug.GCDrugClass;
                cboPregnancyCategory.Value = entityDrug.GCPregnancyCategory;
                chkIsGeneric.Checked = entityDrug.IsGenericDrug;
                chkIsFormularium.Checked = entityDrug.IsFormularium;
                chkIsBPJSFormularium.Checked = entityDrug.IsBPJSFormularium;
                chkIsExternalMedication.Checked = entityDrug.IsExternalMedication;

                chkIsChronic.Checked = entityDrug.IsChronic;
                chkISHAM.Checked = entityDrug.IsHAM;
                chkIsLASA.Checked = entityDrug.IsLASA;
                chkIsOOT.Checked = entityDrug.IsOOT;
                chkIsPrecursor.Checked = entityDrug.IsPrecursor;
                chkIsFORNAS.Checked = entityDrug.IsFORNAS;
                chkIsInjection.Checked = entityDrug.IsInjection; 
                chkIsHasRestrictionInformation.Checked = entityDrug.IsHasRestrictionInformation;

                if (chkIsHasRestrictionInformation.Checked)
                {
                    trRestrictionInformation.Attributes.CssStyle.Remove("display");
                }

                txtRestrictionInformation.Text = entityDrug.RestrictionInformation;

                txtPurposeOfMedication.Text = entityDrug.MedicationPurpose;
                txtMedicationAdministration.Text = entityDrug.MedicationAdministration;
                hdnDefaultSignaID.Value = entityDrug.SignaID.ToString();
                txtSignaLabel.Text = entityDrug.SignaLabel;
                txtSignaName1.Text = entityDrug.SignaName1;
                txtStorageRemarks.Text = entityDrug.StorageRemarks;
            }
            #endregion
        }

        private void ControlToEntity(DrugInfo entityDrug)
        {
            #region Drug Information
            if (entityDrug != null)
            {
                entityDrug.GenericName = txtGenericName.Text;
                entityDrug.Dose = Convert.ToDecimal(txtDose.Text);
                if (cboDose.Value != null && cboDose.Value.ToString() != "")
                    entityDrug.GCDoseUnit = cboDose.Value.ToString();
                else
                    entityDrug.GCDoseUnit = null;
                if (cboDrugForm.Value == null)
                    entityDrug.GCDrugForm = null;
                else
                    entityDrug.GCDrugForm = cboDrugForm.Value.ToString();
                if (string.IsNullOrEmpty(txtMIMSClassCode.Text))
                    entityDrug.MIMSClassID = null;
                else
                    entityDrug.MIMSClassID = Convert.ToInt32(hdnMIMSClassID.Value);

                if (string.IsNullOrEmpty(txtATCClassCode.Text))
                    entityDrug.ATCClassID = null;
                else
                    entityDrug.ATCClassID = Convert.ToInt32(hdnATCClassID.Value);

                if (cboDrugClassification.Value != null && cboDrugClassification.Value.ToString() != "")
                    entityDrug.GCDrugClass = cboDrugClassification.Value.ToString();
                else
                    entityDrug.GCDrugClass = null;

                if (cboCoenamRule.Value != null && cboCoenamRule.Value.ToString() != "")
                    entityDrug.GCCoenamRule = cboCoenamRule.Value.ToString();
                else
                    entityDrug.GCCoenamRule = null;

                if (cboMedicationRoute.Value != null && cboMedicationRoute.Value.ToString() != "")
                    entityDrug.GCMedicationRoute = cboMedicationRoute.Value.ToString();
                else
                    entityDrug.GCMedicationRoute = Constant.MedicationRoute.OTHER;

                if (cboStatusVEN.Value != null && cboStatusVEN.Value.ToString() != "")
                    entityDrug.GCStatusVEN = cboStatusVEN.Value.ToString();
                else
                    entityDrug.GCStatusVEN = null;

                if (cboPregnancyCategory.Value != null && cboPregnancyCategory.Value.ToString() != "")
                    entityDrug.GCPregnancyCategory = cboPregnancyCategory.Value.ToString();
                else
                    entityDrug.GCPregnancyCategory = null;

                entityDrug.IsGenericDrug = chkIsGeneric.Checked;
                entityDrug.IsFormularium = chkIsFormularium.Checked;
                entityDrug.IsBPJSFormularium = chkIsBPJSFormularium.Checked;
                entityDrug.IsEmployeeFormularium = chkIsEmployeeFormularium.Checked;
                entityDrug.IsExternalMedication = chkIsExternalMedication.Checked;

                entityDrug.IsChronic = chkIsChronic.Checked;
                entityDrug.IsHAM = chkISHAM.Checked;
                entityDrug.IsLASA = chkIsLASA.Checked;
                entityDrug.IsOOT = chkIsOOT.Checked;
                entityDrug.IsPrecursor = chkIsPrecursor.Checked;
                entityDrug.IsFORNAS = chkIsFORNAS.Checked;
                entityDrug.IsInjection = chkIsInjection.Checked;
                entityDrug.IsHasRestrictionInformation = chkIsHasRestrictionInformation.Checked;

                entityDrug.RestrictionInformation = txtRestrictionInformation.Text;
                entityDrug.MedicationPurpose = txtPurposeOfMedication.Text;
                entityDrug.MedicationAdministration = txtMedicationAdministration.Text;
                if (string.IsNullOrEmpty(txtSignaLabel.Text))
                    entityDrug.SignaID = null;
                else
                    entityDrug.SignaID = Convert.ToInt32(hdnDefaultSignaID.Value);
                entityDrug.StorageRemarks = txtStorageRemarks.Text;
            }
            #endregion
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DrugInfoDao entityDao = new DrugInfoDao(ctx);
            try
            {
                DrugInfo entity = entityDao.Get(Convert.ToInt32(hdnItemID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
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
    }
}