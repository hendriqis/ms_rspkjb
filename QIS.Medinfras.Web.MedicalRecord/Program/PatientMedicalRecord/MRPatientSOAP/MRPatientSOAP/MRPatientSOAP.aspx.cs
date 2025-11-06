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
using QIS.Data.Core.Dal;


namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class MRPatientSOAP : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PATIENT_SOAP;
        }
        protected override void InitializeDataControl()
        {
            ctlToolbar.SetSelectedMenu(4);
            string filterExpression = string.Format("HealthcareServiceUnitID = '{0}'", AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString());
            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
            int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", AppSession.RegisteredPatient.HealthcareServiceUnitID));
            if (count > 0)
                hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
            else
                hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

            Helper.SetControlEntrySetting(txtNoteDate, new ControlEntrySetting(true, true, true), "mpPatientList");
            Helper.SetControlEntrySetting(txtNoteTime, new ControlEntrySetting(true, true, true), "mpPatientList");
            Helper.SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true), "mpPatientList");
            Helper.SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(true, true, true), "mpPatientList");
            Helper.SetControlEntrySetting(txtSubjective, new ControlEntrySetting(true, true, true), "mpPatientList");
            Helper.SetControlEntrySetting(txtObjective, new ControlEntrySetting(true, true, true), "mpPatientList");
            Helper.SetControlEntrySetting(txtAssessment, new ControlEntrySetting(true, true, true), "mpPatientList");
            Helper.SetControlEntrySetting(txtPlanning, new ControlEntrySetting(true, true, true), "mpPatientList");

            List<vPatientVisitNote> lstPatientVisitNote = BusinessLayer.GetvPatientVisitNoteList(string.Format("VisitID = '{0}'", AppSession.RegisteredPatient.VisitID));
            if (lstPatientVisitNote.Count > 0)
            {
                vPatientVisitNote entity = lstPatientVisitNote.FirstOrDefault(p => p.GCPatientNoteType == Constant.PatientVisitNotes.SUBJECTIVE_NOTES);
                if (entity != null)
                {
                    txtNoteDate.Text = entity.NoteDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtNoteTime.Text = entity.NoteTime;
                    txtPhysicianCode.Text = entity.ParamedicCode;
                    txtPhysicianName.Text = entity.ParamedicName;

                    txtSubjective.Text = lstPatientVisitNote.FirstOrDefault(p => p.GCPatientNoteType == Constant.PatientVisitNotes.SUBJECTIVE_NOTES).NoteText;
                    txtObjective.Text = lstPatientVisitNote.FirstOrDefault(p => p.GCPatientNoteType == Constant.PatientVisitNotes.OBJECTIVE_NOTES).NoteText;
                    txtAssessment.Text = lstPatientVisitNote.FirstOrDefault(p => p.GCPatientNoteType == Constant.PatientVisitNotes.ASSESSMENT_NOTES).NoteText;
                    txtPlanning.Text = lstPatientVisitNote.FirstOrDefault(p => p.GCPatientNoteType == Constant.PatientVisitNotes.PLANNING_NOTES).NoteText;
                }
                else
                {
                    InitializeHeaderInformation();
                }
            }
            else
            {
                InitializeHeaderInformation();
            }
        }

        private void InitializeHeaderInformation()
        {
            txtNoteDate.Text = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNoteTime.Text = AppSession.RegisteredPatient.VisitTime;
            ParamedicMaster paramedic = BusinessLayer.GetParamedicMaster(AppSession.RegisteredPatient.ParamedicID);
            hdnPhysicianID.Value = paramedic.ParamedicID.ToString();
            txtPhysicianCode.Text = paramedic.ParamedicCode;
            txtPhysicianName.Text = paramedic.FullName;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        private void ControlToEntity(PatientVisitNote entityS, PatientVisitNote entityO, PatientVisitNote entityA, PatientVisitNote entityP)
        {
            DateTime dtNoteDate = Helper.GetDatePickerValue(txtNoteDate);
            Int32 paramedicID = Convert.ToInt32(hdnPhysicianID.Value);

            entityS.NoteDate = dtNoteDate;
            entityS.NoteTime = txtNoteTime.Text;
            entityS.GCPatientNoteType = Constant.PatientVisitNotes.SUBJECTIVE_NOTES;
            entityS.NoteText = txtSubjective.Text;
            entityS.ParamedicID = paramedicID;

            entityO.NoteDate = dtNoteDate;
            entityO.NoteTime = txtNoteTime.Text;
            entityO.GCPatientNoteType = Constant.PatientVisitNotes.OBJECTIVE_NOTES;
            entityO.NoteText = txtObjective.Text;
            entityO.ParamedicID = paramedicID;

            entityA.NoteDate = dtNoteDate;
            entityA.NoteTime = txtNoteTime.Text;
            entityA.GCPatientNoteType = Constant.PatientVisitNotes.ASSESSMENT_NOTES;
            entityA.NoteText = txtAssessment.Text;
            entityA.ParamedicID = paramedicID;

            entityP.NoteDate = dtNoteDate;
            entityP.NoteTime = txtNoteTime.Text;
            entityP.GCPatientNoteType = Constant.PatientVisitNotes.PLANNING_NOTES;
            entityP.NoteText = txtPlanning.Text;
            entityP.ParamedicID = paramedicID;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "save")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PatientVisitNoteDao entityDao = new PatientVisitNoteDao(ctx);
                try
                {
                    List<PatientVisitNote> lstPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = '{0}'", AppSession.RegisteredPatient.VisitID));
                    if (lstPatientVisitNote.Count > 0)
                    {
                        int user = AppSession.UserLogin.UserID;
                        DateTime dtNoteDate = Helper.GetDatePickerValue(txtNoteDate);
                        Int32 paramedicID = Convert.ToInt32(hdnPhysicianID.Value);

                        PatientVisitNote entityS = lstPatientVisitNote.FirstOrDefault(p => p.GCPatientNoteType == Constant.PatientVisitNotes.SUBJECTIVE_NOTES);
                        if (entityS != null)
                        {
                            entityS.NoteDate = dtNoteDate;
                            entityS.NoteTime = txtNoteTime.Text;
                            entityS.GCPatientNoteType = Constant.PatientVisitNotes.SUBJECTIVE_NOTES;
                            entityS.NoteText = txtSubjective.Text;
                            entityS.ParamedicID = paramedicID;
                            entityS.VisitID = AppSession.RegisteredPatient.VisitID;
                            entityS.LastUpdatedBy = user;
                            entityDao.Update(entityS);
                        }
                        else
                        {
                            entityS = new PatientVisitNote();
                            entityS.NoteDate = dtNoteDate;
                            entityS.NoteTime = txtNoteTime.Text;
                            entityS.GCPatientNoteType = Constant.PatientVisitNotes.SUBJECTIVE_NOTES;
                            entityS.NoteText = txtSubjective.Text;
                            entityS.ParamedicID = paramedicID;
                            entityS.VisitID = AppSession.RegisteredPatient.VisitID;
                            entityS.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entityS);
                        }

                        PatientVisitNote entityO = lstPatientVisitNote.FirstOrDefault(p => p.GCPatientNoteType == Constant.PatientVisitNotes.OBJECTIVE_NOTES);
                        if (entityO != null)
                        {
                            entityO.NoteDate = dtNoteDate;
                            entityO.NoteTime = txtNoteTime.Text;
                            entityO.GCPatientNoteType = Constant.PatientVisitNotes.SUBJECTIVE_NOTES;
                            entityO.NoteText = txtObjective.Text;
                            entityO.ParamedicID = paramedicID;
                            entityO.VisitID = AppSession.RegisteredPatient.VisitID;
                            entityO.LastUpdatedBy = user;
                            entityDao.Update(entityO);
                        }
                        else
                        {
                            entityO = new PatientVisitNote();
                            entityO.NoteDate = dtNoteDate;
                            entityO.NoteTime = txtNoteTime.Text;
                            entityO.GCPatientNoteType = Constant.PatientVisitNotes.OBJECTIVE_NOTES;
                            entityO.NoteText = txtObjective.Text;
                            entityO.ParamedicID = paramedicID;
                            entityO.VisitID = AppSession.RegisteredPatient.VisitID;
                            entityO.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entityO);
                        }

                        PatientVisitNote entityA = lstPatientVisitNote.FirstOrDefault(p => p.GCPatientNoteType == Constant.PatientVisitNotes.ASSESSMENT_NOTES);
                        if (entityA != null)
                        {
                            entityA.NoteDate = dtNoteDate;
                            entityA.NoteTime = txtNoteTime.Text;
                            entityA.GCPatientNoteType = Constant.PatientVisitNotes.ASSESSMENT_NOTES;
                            entityA.NoteText = txtAssessment.Text;
                            entityA.ParamedicID = paramedicID;
                            entityA.VisitID = AppSession.RegisteredPatient.VisitID;
                            entityA.LastUpdatedBy = user;
                            entityDao.Update(entityA);
                        }
                        else
                        {
                            entityA = new PatientVisitNote();
                            entityA.NoteDate = dtNoteDate;
                            entityA.NoteTime = txtNoteTime.Text;
                            entityA.GCPatientNoteType = Constant.PatientVisitNotes.ASSESSMENT_NOTES;
                            entityA.NoteText = txtAssessment.Text;
                            entityA.ParamedicID = paramedicID;
                            entityA.VisitID = AppSession.RegisteredPatient.VisitID;
                            entityA.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entityA);
                        }

                        PatientVisitNote entityP = lstPatientVisitNote.FirstOrDefault(p => p.GCPatientNoteType == Constant.PatientVisitNotes.PLANNING_NOTES);
                        if (entityP != null)
                        {
                            entityP.NoteDate = dtNoteDate;
                            entityP.NoteTime = txtNoteTime.Text;
                            entityP.GCPatientNoteType = Constant.PatientVisitNotes.PLANNING_NOTES;
                            entityP.NoteText = txtPlanning.Text;
                            entityP.ParamedicID = paramedicID;
                            entityP.VisitID = AppSession.RegisteredPatient.VisitID;
                            entityP.LastUpdatedBy = user;
                            entityDao.Update(entityP);
                        }
                        else
                        {
                            entityP = new PatientVisitNote();
                            entityP.NoteDate = dtNoteDate;
                            entityP.NoteTime = txtNoteTime.Text;
                            entityP.GCPatientNoteType = Constant.PatientVisitNotes.PLANNING_NOTES;
                            entityP.NoteText = txtPlanning.Text;
                            entityP.ParamedicID = paramedicID;
                            entityP.VisitID = AppSession.RegisteredPatient.VisitID;
                            entityP.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entityP);
                        }
                    }
                    else
                    {
                        PatientVisitNote entityS = new PatientVisitNote();
                        PatientVisitNote entityO = new PatientVisitNote();
                        PatientVisitNote entityA = new PatientVisitNote();
                        PatientVisitNote entityP = new PatientVisitNote();

                        ControlToEntity(entityS, entityO, entityA, entityP);
                        entityS.VisitID = AppSession.RegisteredPatient.VisitID;
                        entityS.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entityS);

                        entityO.VisitID = AppSession.RegisteredPatient.VisitID;
                        entityO.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entityO);

                        entityA.VisitID = AppSession.RegisteredPatient.VisitID;
                        entityA.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entityA);

                        entityP.VisitID = AppSession.RegisteredPatient.VisitID;
                        entityP.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entityP);
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false; 
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            return false;
        }
    }
}