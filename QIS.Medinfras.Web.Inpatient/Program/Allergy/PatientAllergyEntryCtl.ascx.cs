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
using System.Globalization;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.Inpatient.Program
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

            FillAllergyPeriod();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtAllergenName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboAllergenType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboFindingSource, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboYear, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboMonth, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboSeverity, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtReaction, new ControlEntrySetting(true, true, true));
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
            if (entity.KnownDate != null)
            {
                if (entity.KnownDate.Length > 3)
                {
                    cboYear.Value = entity.KnownDate.Substring(0, 4);
                }

                if (entity.KnownDate.Length > 4)
                {
                    cboMonth.Value = entity.KnownDate.Substring(4, 2);
                }
                else if (entity.KnownDate.Length == 2)
                {
                    cboMonth.Value = entity.KnownDate.Substring(0, 2);
                }
            }

            if (entity.GCAllergenType == Constant.AllergenType.DRUG)
            {
                trAllergenName.Style.Add("display", "none");
                trDrugGenericName.Style.Remove("display");

                txtDrugGenericName.Text = entity.Allergen;
            }
            else
            {
                trDrugGenericName.Style.Add("display", "none");
                trAllergenName.Style.Remove("display");

                txtAllergenName.Text = entity.Allergen;
            }

            txtReaction.Text = entity.Reaction;
        }

        private void ControlToEntity(PatientAllergy entity)
        {
            entity.GCAllergenType = cboAllergenType.Value.ToString();
            entity.GCAllergySource = cboFindingSource.Value.ToString();
            entity.GCAllergySeverity = cboSeverity.Value.ToString();
            entity.AllergyLogDate = Helper.GetDatePickerValue(txtObservationDate);

            String year = "";
            String month = "";
            String date = "";
            if (cboYear.Value != null)
            {
                year = cboYear.Value.ToString();
            }
            if (cboMonth.Value != null)
            {
                month = cboMonth.Value.ToString();
            }
            entity.KnownDate = year + month + date;

            if (entity.GCAllergenType == Constant.AllergenType.DRUG)
            {
                entity.Allergen = Request.Form[txtDrugGenericName.UniqueID];
            }
            else
            {
                entity.Allergen = txtAllergenName.Text;
            }

            entity.Reaction = txtReaction.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PatientAllergy entity = new PatientAllergy();
                ControlToEntity(entity);

                vRegistration vreg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                entity.VisitID = vreg.VisitID;
                entity.MRN = AppSession.RegisteredPatient.MRN;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.CreatedDate = DateTime.Now;
                BusinessLayer.InsertPatientAllergy(entity);

                List<PatientAllergy> lstAllergy = BusinessLayer.GetPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN));
                if (lstAllergy.Count > 0)
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
                else
                {
                    Patient oPatient = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
                    if (oPatient != null)
                    {
                        oPatient.IsHasAllergy = false;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entity.LastUpdatedDate = DateTime.Now;
                        BusinessLayer.UpdatePatient(oPatient);
                    }
                }

                if (AppSession.SA0137 == "1")
                {
                    if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                    {
                        BridgingToMedinfrasV1(1);
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

                List<PatientAllergy> lstAllergy = BusinessLayer.GetPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN));
                if (lstAllergy.Count > 0)
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
                else
                {
                    Patient oPatient = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
                    if (oPatient != null)
                    {
                        oPatient.IsHasAllergy = false;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entity.LastUpdatedDate = DateTime.Now;
                        BusinessLayer.UpdatePatient(oPatient);
                    }
                }

                if (AppSession.SA0137 == "1")
                {
                    if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                    {
                        BridgingToMedinfrasV1(2);
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        private void FillAllergyPeriod()
        {
            cboYear.DataSource = Enumerable.Range(DateTime.Now.Year - 99, 100).Reverse();
            cboYear.EnableCallbackMode = false;
            cboYear.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboYear.DropDownStyle = DropDownStyle.DropDownList;
            cboYear.DataBind();
            cboYear.Items.Insert(0, new ListEditItem { Value = "", Text = "" });
            cboYear.Value = "";

            List<Variable> lstMonth = new List<Variable>();
            lstMonth.Add(new Variable { Code = "", Value = "" });
            lstMonth.Add(new Variable { Code = "01", Value = "Januari" });
            lstMonth.Add(new Variable { Code = "02", Value = "Februari" });
            lstMonth.Add(new Variable { Code = "03", Value = "Maret" });
            lstMonth.Add(new Variable { Code = "04", Value = "April" });
            lstMonth.Add(new Variable { Code = "05", Value = "Mei" });
            lstMonth.Add(new Variable { Code = "06", Value = "Juni" });
            lstMonth.Add(new Variable { Code = "07", Value = "Juli" });
            lstMonth.Add(new Variable { Code = "08", Value = "Agustus" });
            lstMonth.Add(new Variable { Code = "09", Value = "September" });
            lstMonth.Add(new Variable { Code = "10", Value = "Oktober" });
            lstMonth.Add(new Variable { Code = "11", Value = "November" });
            lstMonth.Add(new Variable { Code = "12", Value = "Desember" });
            Methods.SetComboBoxField<Variable>(cboMonth, lstMonth, "Value", "Code");
            cboMonth.Value = "";
        }

        private void BridgingToMedinfrasV1(int ProcessType)
        {
            APIMessageLog apiLog = new APIMessageLog();
            apiLog.MessageDateTime = DateTime.Now;
            apiLog.Sender = Constant.BridgingVendor.HIS;
            apiLog.Recipient = Constant.BridgingVendor.MEDINFRAS_API;

            MedinfrasV1Service oService = new MedinfrasV1Service();
            string serviceResult = string.Empty;
            serviceResult = oService.OnSendPatientAllergiesInformation(ProcessType, AppSession.RegisteredPatient.RegistrationNo);
            if (!string.IsNullOrEmpty(serviceResult))
            {
                string[] serviceResultInfo = serviceResult.Split('|');
                if (serviceResultInfo[0] == "1")
                {
                    apiLog.IsSuccess = true;
                    apiLog.MessageText = serviceResultInfo[1];
                    apiLog.Response = serviceResultInfo[2];
                }
                else
                {
                    apiLog.IsSuccess = false;
                    apiLog.MessageText = serviceResultInfo[1];
                    apiLog.Response = serviceResultInfo[2];
                    apiLog.ErrorMessage = serviceResultInfo[2];
                }
                BusinessLayer.InsertAPIMessageLog(apiLog);
            }
        }
    }
}