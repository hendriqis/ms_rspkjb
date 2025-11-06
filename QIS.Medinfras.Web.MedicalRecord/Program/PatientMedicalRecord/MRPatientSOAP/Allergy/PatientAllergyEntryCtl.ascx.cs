using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientAllergyEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = false;
                hdnID.Value = param;
                SetControlProperties();
                PatientAllergy entity = BusinessLayer.GetPatientAllergy(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
                SetControlProperties();
            }
        }

        private void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0",
                Constant.StandardCode.ALLERGEN_TYPE, Constant.StandardCode.ALLERGY_INFORMATION_SOURCE, Constant.StandardCode.ALLERGY_SEVERITY);

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboAllergenType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ALLERGEN_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFindingSource, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ALLERGY_INFORMATION_SOURCE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSeverity, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ALLERGY_SEVERITY).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtAllergenName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboAllergenType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboFindingSource, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboSeverity, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtReaction, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(PatientAllergy entity)
        {
            if (!entity.AllergyLogDate.Equals(new DateTime(1900, 1, 1)))
            {
                txtObservationDate.Text = entity.AllergyLogDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            cboAllergenType.Value = entity.GCAllergenType;
            cboFindingSource.Value = entity.GCAllergySource;
            cboSeverity.Value = entity.GCAllergySeverity;
            //entity.KnownDate = year + month + date;
            txtAllergenName.Text = entity.Allergen;
            txtReaction.Text = entity.Reaction;
        }

        private void ControlToEntity(PatientAllergy entity)
        {
            entity.GCAllergenType = cboAllergenType.Value.ToString();
            entity.GCAllergySource = cboFindingSource.Value.ToString();
            entity.GCAllergySeverity = cboSeverity.Value.ToString();
            entity.AllergyLogDate = Helper.GetDatePickerValue(txtObservationDate);
            //entity.KnownDate = year + month + date;
            entity.Allergen = txtAllergenName.Text;
            entity.Reaction = txtReaction.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PatientAllergy entity = new PatientAllergy();
                ControlToEntity(entity);
                entity.MRN = AppSession.RegisteredPatient.MRN;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.CreatedDate = DateTime.Now;
                BusinessLayer.InsertPatientAllergy(entity);
                if (entity.GCAllergenType == Constant.AllergenType.DRUG)
                {
                    //Update Patient's Allergy Flag
                    Patient oPatient = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
                    if (oPatient != null)
                    {
                        oPatient.IsHasAllergy = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entity.LastUpdatedDate = DateTime.Now;
                        BusinessLayer.UpdatePatient(oPatient);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PatientAllergy entity = BusinessLayer.GetPatientAllergy(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientAllergy(entity);
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