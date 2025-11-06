using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientDisposition : BasePagePatientPageList
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.TRANSACTION_PAGE_PATIENT_DISPOSITION;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.TRANSACTION_PAGE_PATIENT_DISPOSITION;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.TRANSACTION_PAGE_PATIENT_DISPOSITION;
                default: return Constant.MenuCode.Outpatient.TRANSACTION_PAGE_PATIENT_DISPOSITION;
            } 
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected string RegistrationDateTime = "";

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.DISCHARGE_ROUTINE, Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            List<StandardCode> lstStandardCode2 = BusinessLayer.GetStandardCodeList(String.Format("StandardCodeID IN ('{0}','{1}')", Constant.Referrer.FASKES, Constant.Referrer.RUMAH_SAKIT));
            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = 'OUTPATIENT' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID));

            //            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME && p.StandardCodeID != Constant.PatientOutcome.DEAD_AFTER_48 && p.StandardCodeID != Constant.PatientOutcome.DEAD_BEFORE_48).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPatientOutcomeDead, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME && (p.StandardCodeID == Constant.PatientOutcome.DEAD_AFTER_48 || p.StandardCodeID == Constant.PatientOutcome.DEAD_BEFORE_48)).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeRoutine, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeReason, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboReferrerGroup, lstStandardCode2.ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboClinic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");

            RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");
            txtDateOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTimeOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            Helper.SetControlEntrySetting(txtDischargeDate, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtDischargeTime, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboDischargeRoutine, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboPatientOutcome, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboPatientOutcomeDead, new ControlEntrySetting(true, true, true), "mpPatientDischarge");

            string filterVisit = string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID);
            vConsultVisit5 entity = BusinessLayer.GetvConsultVisit5List(filterVisit).FirstOrDefault();
            hdnDepartmentID.Value = entity.DepartmentID;
            EntityToControl(entity);
        }

        protected string GetPatientOutcomeDeadBefore48()
        {
            return Constant.PatientOutcome.DEAD_BEFORE_48;
        }

        protected string GetPatientOutcomeDeadAfter48()
        {
            return Constant.PatientOutcome.DEAD_AFTER_48;
        }

        private void EntityToControl(vConsultVisit5 entity)
        {
            if (entity.GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48 || entity.GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48)
            {
                chkIsDead.Checked = true;
                cboPatientOutcomeDead.Value = entity.GCDischargeCondition;
            }
            else
            {
                cboPatientOutcome.Value = entity.GCDischargeCondition;
            }
            cboDischargeRoutine.Value = entity.GCDischargeMethod;
            if (!entity.DischargeDate.Equals(new DateTime(1900, 1, 1)))
            {
                txtDischargeDate.Text = entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            else txtDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            if (!string.IsNullOrEmpty(entity.DischargeTime))
            {
                txtDischargeTime.Text = entity.DischargeTime;
            }
            else
            {
                txtDischargeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }

            if (!entity.ReferralDate.Equals(new DateTime(1900, 1, 1)))
            {
                txtAppointmentDate.Text = entity.ReferralDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT); ;
            }
            else
            {
                txtAppointmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }

            hdnLOSInDay.Value = entity.LOSInDay.ToString();
            hdnLOSInHour.Value = entity.LOSInHour.ToString();
            hdnLOSInMinute.Value = entity.LOSInMinute.ToString();
            cboDischargeReason.Value = entity.GCReferralDischargeReason;
            txtDischargeOtherReason.Text = entity.ReferralDischargeReasonOther;

            hdnParamedicID2.Value = entity.ReferralPhysicianID.ToString();
            txtParamedicCode.Text = entity.ReferralPhysicianCode;
            txtParamedicName.Text = entity.ReferralPhysicianName;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "save")
            {
                IDbContext ctx = DbFactory.Configure(true);
                ConsultVisitDao entityDao = new ConsultVisitDao(ctx);
                try
                {
                    //TODO : Validasi Tagihan Pasien
                    if (cboDischargeRoutine.Value.ToString() != Constant.DischargeMethod.DISCHARGED_TO_WARD)
                    {
                        bool isCheckOutstanding = true;
                        SettingParameterDt oParam = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_VALIDASI_TAGIHAN_KETIKA_PULANG);
                        if (oParam != null)
                            isCheckOutstanding = oParam.ParameterValue == "1" ? true : false;

                        if (isCheckOutstanding)
                        {
                            List<vRegistrationOutstanding> lstOutstanding = BusinessLayer.GetvRegistrationOutstandingList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID));
                            if (lstOutstanding.Count > 0)
                            {
                                errMessage = "Pasien tidak dapat dipulangkan karena masih memiliki sisa tagihan";
                                return false;
                            }
                        }
                    }
                    List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = '{0}'", AppSession.RegisteredPatient.RegistrationID), ctx);
                    ConsultVisit entity = lstConsultVisit.Where(t => t.VisitID == AppSession.RegisteredPatient.VisitID).FirstOrDefault();
                    entity.GCVisitStatus = Constant.VisitStatus.DISCHARGED;
                    if (chkIsDead.Checked)
                    {
                        entity.GCDischargeCondition = cboPatientOutcomeDead.Value.ToString();
                    }
                    else
                    {
                        entity.GCDischargeCondition = cboPatientOutcome.Value.ToString();
                    }
                    entity.GCDischargeMethod = cboDischargeRoutine.Value.ToString();
                    if (AppSession.UserLogin.ParamedicID != null && AppSession.UserLogin.ParamedicID != 0)
                    {
                        entity.PhysicianDischargedBy = AppSession.UserLogin.UserID;
                        entity.PhysicianDischargedDate = DateTime.Now;
                    }
                    entity.DischargeDate = Helper.GetDatePickerValue(txtDischargeDate);
                    entity.DischargeTime = txtDischargeTime.Text;
                    entity.LOSInDay = Convert.ToDecimal(hdnLOSInDay.Value);
                    entity.LOSInHour = Convert.ToDecimal(hdnLOSInHour.Value);
                    entity.LOSInMinute = Convert.ToDecimal(hdnLOSInMinute.Value);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.AdminDischargedBy = AppSession.UserLogin.UserID;
                    entity.AdminDischargedDate = DateTime.Now;

                    if (entity.GCDischargeMethod == Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER)
                    {
                        entity.ReferralTo = Convert.ToInt32(hdnReferrerID.Value);
                    }
                    else if (entity.GCDischargeMethod == Constant.DischargeMethod.DISCHARGED_TO_WARD)
                    {
                        entity.ReferralPhysicianID = Convert.ToInt32(hdnParamedicID2.Value);
                        entity.IsRefferralProcessed = false;
                    }
                    else if (entity.GCDischargeMethod == Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT)
                    {
                        entity.ReferralUnitID = Convert.ToInt32(cboClinic.Value.ToString());
                        entity.ReferralPhysicianID = Convert.ToInt32(hdnPhysicianID.Value);
                        entity.ReferralDate = Helper.GetDatePickerValue(txtAppointmentDate);
                        entity.IsRefferralProcessed = false;
                    }

                    if (cboDischargeReason.Value != null)
                    {
                        entity.GCReferralDischargeReason = cboDischargeReason.Value.ToString();
                        if (cboDischargeReason.Value.ToString() == Constant.ReferralDischargeReason.LAINNYA)
                        {
                            entity.ReferralDischargeReasonOther = txtDischargeOtherReason.Text;
                        }
                    }

                    if (lstConsultVisit.Where(t => t.GCVisitStatus != Constant.VisitStatus.PHYSICIAN_DISCHARGE && t.GCVisitStatus != Constant.VisitStatus.DISCHARGED && t.GCVisitStatus != Constant.VisitStatus.CLOSED && t.GCVisitStatus != Constant.VisitStatus.CANCELLED).Count() == 0)
                    {
                        RegistrationDao registrationDao = new RegistrationDao(ctx);
                        Registration entityRegis = registrationDao.Get(entity.RegistrationID);
                        entityRegis.GCRegistrationStatus = Constant.VisitStatus.DISCHARGED;
                        entityRegis.LastUpdatedBy = AppSession.UserLogin.UserID;
                        registrationDao.Update(entityRegis);
                    }

                    entityDao.Update(entity);
                    ctx.CommitTransaction();
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
            return result;
        }
    }
}