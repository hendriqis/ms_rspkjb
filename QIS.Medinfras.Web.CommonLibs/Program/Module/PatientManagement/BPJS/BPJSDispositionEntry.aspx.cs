using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;


namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class BPJSDispositionEntry : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.IP_BPJS_CLAIM_DISPOSISI_PASIEN;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.ER_BPJS_CLAIM_DISPOSISI_PASIEN;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.PH_BPJS_CLAIM_DISPOSISI_PASIEN;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.LB_BPJS_CLAIM_DISPOSISI_PASIEN;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.IS_BPJS_CLAIM_DISPOSISI_PASIEN;
                    return Constant.MenuCode.MedicalDiagnostic.MD_BPJS_CLAIM_DISPOSISI_PASIEN;
                default: return Constant.MenuCode.MedicalRecord.BPJS_PROSES_TINDAK_LANJUT;
            }
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        protected string RegistrationDateTime = "";
        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnDepartmentID.Value = Page.Request.QueryString["id"];
            }
            //hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.DISCHARGE_ROUTINE, Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeRoutine, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeReason, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL).ToList(), "StandardCodeName", "StandardCodeID");

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = 'OUTPATIENT' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboClinic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");

            RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");

            Helper.SetControlEntrySetting(txtDischargeDate, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtDischargeTime, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboDischargeRoutine, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboPatientOutcome, new ControlEntrySetting(true, true, true), "mpPatientDischarge");

            ConsultVisit entity = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            if (entity.GCDischargeCondition != "" && entity.GCDischargeMethod != "")
            {
                EntityToControl(entity);
            }
            else
            {
                txtDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                txtAppointmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDateOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTimeOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            }
        }

        private void EntityToControl(ConsultVisit entity)
        {
            cboPatientOutcome.Value = entity.GCDischargeCondition;
            cboDischargeRoutine.Value = entity.GCDischargeMethod;
            txtDischargeDate.Text = entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDischargeTime.Text = entity.DischargeTime;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "process")
            {
                if (IsValidToDischarge())
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    ConsultVisitDao visitDao = new ConsultVisitDao(ctx);
                    try
                    {
                        List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = '{0}'", AppSession.RegisteredPatient.RegistrationID), ctx);
                        ConsultVisit entity = lstConsultVisit.Where(t => t.VisitID == AppSession.RegisteredPatient.VisitID).FirstOrDefault();
                        entity.GCVisitStatus = Constant.VisitStatus.DISCHARGED;
                        entity.GCDischargeCondition = cboPatientOutcome.Value.ToString();
                        entity.GCDischargeMethod = cboDischargeRoutine.Value.ToString();
                        entity.DischargeDate = Helper.GetDatePickerValue(txtDischargeDate);
                        entity.DischargeTime = txtDischargeTime.Text;
                        entity.LOSInDay = Convert.ToDecimal(hdnLOSInDay.Value);
                        entity.LOSInHour = Convert.ToDecimal(hdnLOSInHour.Value);
                        entity.LOSInMinute = Convert.ToDecimal(hdnLOSInMinute.Value);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                        if (cboDischargeRoutine.Value.ToString() == Constant.DischargeMethod.DISCHARGED_TO_WARD)
                        {
                            entity.ReferralPhysicianID = Convert.ToInt32(hdnParamedicID2.Value);
                            entity.IsRefferralProcessed = false;
                        }
                        else if (cboDischargeRoutine.Value.ToString() == Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT)
                        {
                            entity.ReferralUnitID = Convert.ToInt32(cboClinic.Value.ToString());
                            entity.ReferralPhysicianID = Convert.ToInt32(hdnPhysicianID.Value);
                            entity.ReferralDate = Helper.GetDatePickerValue(txtAppointmentDate);
                            entity.IsRefferralProcessed = false;
                        }

                        if (cboPatientOutcome.Value.ToString() == Constant.PatientOutcome.DEAD_BEFORE_48 || cboPatientOutcome.Value.ToString() == Constant.PatientOutcome.DEAD_AFTER_48)
                        {
                            entity.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeath);
                            entity.TimeOfDeath = txtTimeOfDeath.Text;

                            entity.ReferralUnitID = null;
                            entity.ReferralPhysicianID = null;
                            entity.ReferralDate = Helper.InitializeDateTimeNull();
                            entity.IsRefferralProcessed = false;

                            //Update Patient Death Status
                            PatientDao patientDao = new PatientDao(ctx);
                            Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                            if (oPatient != null)
                            {
                                oPatient.IsAlive = false;
                                oPatient.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeath);
                            }
                            else
                            {
                                oPatient.IsAlive = true;
                                oPatient.DateOfDeath = DateTime.MinValue;
                            }
                            oPatient.LastVisitDate = AppSession.RegisteredPatient.VisitDate;
                            patientDao.Update(oPatient);
                        }

                        if (lstConsultVisit.Where(t => t.GCVisitStatus != Constant.VisitStatus.PHYSICIAN_DISCHARGE && t.GCVisitStatus != Constant.VisitStatus.DISCHARGED && t.GCVisitStatus != Constant.VisitStatus.CLOSED && t.GCVisitStatus != Constant.VisitStatus.CANCELLED).Count() == 0)
                        {
                            RegistrationDao registrationDao = new RegistrationDao(ctx);
                            Registration entityRegis = registrationDao.Get(entity.RegistrationID);
                            entityRegis.GCRegistrationStatus = Constant.VisitStatus.DISCHARGED;
                            entityRegis.LastUpdatedBy = AppSession.UserLogin.UserID;
                            registrationDao.Update(entityRegis);
                        }

                        visitDao.Update(entity);
                        ctx.CommitTransaction();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        errMessage = ex.Message;
                        ctx.RollBackTransaction();
                    }
                    finally
                    {
                        ctx.Close();
                    }
                }
                else
                {
                    errMessage = "Diagnosa pasien harus terisi sebelum disposisi pasien.";
                    result = false;
                }
            }
            return result;
        }

        private bool IsValidToDischarge()
        {
            vPatientDiagnosis oDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            bool isDiagnosisExist = oDiagnosis != null;

            return isDiagnosisExist;
        }
    }
}