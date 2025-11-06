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
using System.Text;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientDischarge1 : BasePagePatientPageList
    {
        protected int gridDiagnosisPageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_DISCHARGE;
        }

        protected string RegistrationDateTime = "";
        protected override void InitializeDataControl()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}')", Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.DISCHARGE_ROUTINE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            List<StandardCode> lstDischargeOutcome = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME && p.IsActive == true).OrderBy(lst => lst.TagProperty).ToList();
            List<StandardCode> lstDischargeRoutine = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE && p.IsActive == true).OrderBy(lst => lst.TagProperty).ToList();

            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstDischargeOutcome, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeRoutine, lstDischargeRoutine, "StandardCodeName", "StandardCodeID");

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = 'OUTPATIENT' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboClinic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");

            Helper.SetControlEntrySetting(txtDischargeDate, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtDischargeTime, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboDischargeRoutine, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboPatientOutcome, new ControlEntrySetting(true, true, true), "mpPatientDischarge");

            ConsultVisit entity = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.EMERGENCY)
            {
                if (!string.IsNullOrEmpty(entity.StartServiceTime))
                {
                    RegistrationDateTime = entity.StartServiceDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                    RegistrationDateTime += entity.StartServiceTime.Replace(":", "");
                }
                else
                {
                    RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                    RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");
                }
            }
            else
            {
                RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");
            }

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

            BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
        }

        private void EntityToControl(ConsultVisit entity)
        {
            cboPatientOutcome.Value = entity.GCDischargeCondition;
            cboDischargeRoutine.Value = entity.GCDischargeMethod;
            txtDischargeDate.Text = entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDischargeTime.Text = entity.DischargeTime;
            if (entity.ReferralPhysicianID != null)
            {
                ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster((Int32)entity.ReferralPhysicianID);
                if (oParamedic != null)
                {
                    hdnParamedicID2.Value = entity.ReferralPhysicianID.ToString();
                    txtParamedicCode.Text = oParamedic.ParamedicCode;
                    txtParamedicName.Text = oParamedic.FullName;
                }
            }
            if (entity.ReferralUnitID != null)
            {
                cboClinic.Value = entity.ReferralUnitID.ToString();
                ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster((Int32)entity.ReferralPhysicianID);
                if (oParamedic != null)
                {
                    hdnPhysicianID.Value = entity.ReferralPhysicianID.ToString();
                    txtPhysicianCode.Text = oParamedic.ParamedicCode;
                    txtPhysicianName.Text = oParamedic.FullName;
                }
                txtAppointmentDate.Text = entity.ReferralDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            if (entity.GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48 || entity.GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48)
            {
                txtDateOfDeath.Text = entity.DateOfDeath.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTimeOfDeath.Text = entity.TimeOfDeath;
            }
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
                        List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = '{0}'", AppSession.RegisteredPatient.RegistrationID),ctx);
                        ConsultVisit entity = lstConsultVisit.Where(t => t.VisitID == AppSession.RegisteredPatient.VisitID).FirstOrDefault();
                        if (entity.GCVisitStatus == Constant.VisitStatus.CHECKED_IN || entity.GCVisitStatus == Constant.VisitStatus.RECEIVING_TREATMENT)
                        {
                            entity.GCVisitStatus = Constant.VisitStatus.PHYSICIAN_DISCHARGE; 
                        }
                        entity.GCDischargeCondition = cboPatientOutcome.Value.ToString();
                        entity.GCDischargeMethod = cboDischargeRoutine.Value.ToString();
                        entity.PhysicianDischargeOrderDate = Helper.GetDatePickerValue(txtDischargeDate);
                        entity.PhysicianDischargeOrderTime = txtDischargeTime.Text;
                        entity.PhysicianDischargedBy = AppSession.UserLogin.UserID;
                        entity.PhysicianDischargedDate = DateTime.Now;
                        //entity.DischargeDate = Helper.GetDatePickerValue(txtDischargeDate);
                        //entity.DischargeTime = txtDischargeTime.Text;
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
                            entityRegis.GCRegistrationStatus = Constant.VisitStatus.PHYSICIAN_DISCHARGE;
                            entityRegis.LastUpdatedBy =  AppSession.UserLogin.UserID;
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
                    errMessage = "You must be Registered Physician and must entry Patient Chief Complaint, Diagnosis before discharge this patient";
                    result = false;
                }
            }
            return result;
        }

        private bool IsValidToDischarge()
        {
            bool isDPJPPhysician = AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID;

            ChiefComplaint oChiefComplaint = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            bool isChiefComplaintExist = oChiefComplaint != null;

            PatientDiagnosis oDiagnosis = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            bool isDiagnosisExist = oDiagnosis != null;

            return isChiefComplaintExist && isDiagnosisExist && isDPJPPhysician;
        }

        #region Diagnosis
        private void BindGridViewDiagnosis(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "GCDiagnoseType");
            List<vPatientDiagnosis> lstMainDiagnosis = lstEntity.Where(lst => lst.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS).ToList();
            hdnIsMainDiagnosisExists.Value = lstMainDiagnosis.Count > 0 ? "1" : "0";

            grdDiagnosisView.DataSource = lstEntity;
            grdDiagnosisView.DataBind();

            if (hdnIsMainDiagnosisExists.Value == "1")
                cboDiagnosisType.Value = Constant.DiagnoseType.COMPLICATION;
            else
                cboDiagnosisType.Value = Constant.DiagnoseType.MAIN_DIAGNOSIS;

            //Create Diagnosis Summary for : CPOE Clinical Notes
            StringBuilder strDiagnosis = new StringBuilder();
            foreach (var item in lstEntity)
            {
                if (item.GCDifferentialStatus != Constant.DifferentialDiagnosisStatus.RULED_OUT)
                {
                    strDiagnosis.AppendLine(string.Format("{0}", item.cfDiagnosisText));
                }
            }
            hdnDiagnosisSummary.Value = strDiagnosis.ToString();
        }

        protected void cbpDiagnosisView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDiagnosis(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDiagnosis(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = hdnIsMainDiagnosisExists.Value;
        }
        protected void cbpDiagnosis_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        PatientDiagnosis entity = new PatientDiagnosis();

                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.DifferentialDate = Helper.GetDatePickerValue(txtDischargeDate);
                        entity.FinalDate = Helper.GetDatePickerValue(txtDischargeDate);
                        entity.DifferentialTime = txtDischargeTime.Text;
                        entity.FinalTime = txtDischargeTime.Text;

                        entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        entity.GCDiagnoseType = cboDiagnosisType.Value.ToString();
                        hdnIsMainDiagnosisExists.Value = entity.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS ? "1" : "0";
                        entity.GCDifferentialStatus = cboDiagnosisStatus.Value.ToString();
                        entity.GCFinalStatus = cboDiagnosisStatus.Value.ToString();

                        if (hdnEntryDiagnoseID.Value != "")
                            entity.DiagnoseID = hdnEntryDiagnoseID.Value;
                        else
                            entity.DiagnoseID = null;

                        entity.DiagnosisText = txtDiagnosisText.Text;
                        entity.MorphologyID = null;
                        entity.IsChronicDisease = false;
                        entity.IsFollowUpCase = false;
                        entity.Remarks = string.Empty;
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        BusinessLayer.InsertPatientDiagnosis(entity);

                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnDiagnosisID.Value);
                        PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(recordID);

                        if (entity != null)
                        {
                            entity.FinalDate = Helper.GetDatePickerValue(txtDischargeDate);
                            entity.FinalTime = txtDischargeTime.Text;

                            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            entity.GCDiagnoseType = cboDiagnosisType.Value.ToString();
                            hdnIsMainDiagnosisExists.Value = entity.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS ? "1" : "0";
                            entity.GCFinalStatus = cboDiagnosisStatus.Value.ToString();

                            if (hdnEntryDiagnoseID.Value != "")
                                entity.DiagnoseID = hdnEntryDiagnoseID.Value;
                            else
                                entity.DiagnoseID = null;

                            entity.DiagnosisText = txtDiagnosisText.Text;
                            entity.MorphologyID = null;
                            entity.IsChronicDisease = false;
                            entity.IsFollowUpCase = false;
                            entity.Remarks = string.Empty;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientDiagnosis(entity);
                            result = "1|edit|";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Diagnosis Record Information");
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnDiagnosisID.Value);
                        PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(recordID);

                        if (entity != null)
                        {
                            //TODO : Prompt user for delete reason
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientDiagnosis(entity);
                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Diagnosis Record Information");
                        }
                        result = "1|delete|";
                    }

                }

            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}