using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class CopyMDNurseAssessmentCtl : BaseProcessPopupCtl
    {
        protected string MRN = "";
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnVisitID.Value = paramInfo[0];
            hdnRecordID.Value = paramInfo[1];
            txtParamedicName.Text = AppSession.UserLogin.UserFullName;
            txtNoteDateTime.Text = string.Format("{0} - {1}", DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));

            hdnUserParamedicName.Value = AppSession.UserLogin.UserFullName;
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            try
            {
                int id = Convert.ToInt32(hdnRecordID.Value);
                string referenceNo = string.Empty;
                bool isError = false;

                string retVal = ProcessRecord(hdnRecordID.Value);
                string[] retValInfo = retVal.Split('|');
                if (retValInfo[0] == "0")
                {
                    isError = true;
                    errMessage = retValInfo[1];
                }

                result = !isError;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
        }

        private string ProcessRecord(string recordID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            NurseChiefComplaintDao oNurseCCDao = new NurseChiefComplaintDao(ctx);
            PatientVisitNoteDao oVisitNoteDao = new PatientVisitNoteDao(ctx);

            try
            {
                int id = Convert.ToInt32(recordID);
                NurseChiefComplaint oRecord = BusinessLayer.GetNurseChiefComplaintList(string.Format("ID = {0}",id),ctx).FirstOrDefault();
                if (oRecord != null)
                {
                    #region Patient Visit Note
                    //Create New Record : Patient Visit Note
                    PatientVisitNote oVisitNote = new PatientVisitNote();
                    bool isCopyFromPrevious = false;
                    int visitNoteID = 0;
                    if (oRecord.PatientVisitNoteID != null && oRecord.PatientVisitNoteID != 0)
                    {
                        PatientVisitNote oNote = BusinessLayer.GetPatientVisitNoteList(string.Format("ID = {0}", oRecord.PatientVisitNoteID), ctx).FirstOrDefault();
                        if (oNote != null)
                        {
                            oVisitNote = oNote;
                            isCopyFromPrevious = true;
                        }
                    }

                    if (isCopyFromPrevious)
                    {
                        oVisitNote.NoteDate = DateTime.Now.Date;
                        oVisitNote.NoteTime = DateTime.Now.Date.ToString(Constant.FormatString.TIME_FORMAT);
                        visitNoteID = oVisitNoteDao.InsertReturnPrimaryKeyID(oVisitNote);
                    }
                    else
                    {
                        oVisitNote.VisitID = AppSession.RegisteredPatient.VisitID;
                        oVisitNote.NoteDate = DateTime.Now.Date;
                        oVisitNote.NoteTime = DateTime.Now.Date.ToString(Constant.FormatString.TIME_FORMAT);
                        oVisitNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        oVisitNote.GCPatientNoteType = Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT;
                        oVisitNote.CreatedBy = AppSession.UserLogin.UserID;
                        SetPatientVisitNote(oRecord, oVisitNote);
                        visitNoteID = oVisitNoteDao.InsertReturnPrimaryKeyID(oVisitNote);
                    }
                    #endregion

                    //Create New Record from Old Record
                    NurseChiefComplaint oNewRecord = new NurseChiefComplaint();
                    oNewRecord = oRecord;
                    oNewRecord.ID = 0;
                    oNewRecord.ChiefComplaintDate = DateTime.Now.Date;
                    oNewRecord.ChiefComplaintTime = DateTime.Now.Date.ToString(Constant.FormatString.TIME_FORMAT);
                    oNewRecord.VisitID = AppSession.RegisteredPatient.VisitID;
                    oNewRecord.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                    oNewRecord.PatientVisitNoteID = visitNoteID;
                    oNewRecord.GCAssessmentStatus = Constant.AssessmentStatus.OPEN;
                    oNurseCCDao.Insert(oNewRecord);

                    ctx.CommitTransaction();

                    result = string.Format("1|{0}", string.Empty);
                }

            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("0|{0}", ex.Message);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private void SetPatientVisitNote(NurseChiefComplaint oChiefComplaint, PatientVisitNote oVisitNote)
        {
            string soapNote = GenerateSOAPText(oChiefComplaint);

            string subjectiveText = string.Empty;
            string objectiveText = string.Empty;
            string assessmentText = string.Empty;
            string planningText = string.Empty;

            oVisitNote.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            oVisitNote.NoteText = soapNote;
            oVisitNote.NoteDate = oChiefComplaint.ChiefComplaintDate;
            oVisitNote.NoteTime = oChiefComplaint.ChiefComplaintTime;
        }

        private string GenerateSOAPText(NurseChiefComplaint oChiefComplaint)
        {
            StringBuilder sbNotes = new StringBuilder();
            StringBuilder sbSubjective = new StringBuilder();
            sbNotes.AppendLine("Subjektive :");
            sbNotes.AppendLine("-".PadRight(15, '-'));

            sbNotes.AppendLine(string.Format("Keluhan Utama  : "));
            sbNotes.AppendLine(string.Format(" {0}   ", oChiefComplaint.NurseChiefComplaintText));
            sbNotes.AppendLine(string.Format("Keluhan Lain yang menyertai : "));
            sbNotes.AppendLine(string.Format(" {0}   ", oChiefComplaint.HPISummary));
            sbNotes.AppendLine(string.Format("Riwayat Penyakit Dahulu     : "));
            sbNotes.AppendLine(string.Format(" {0}   ", oChiefComplaint.MedicalHistory));
            sbNotes.AppendLine(string.Format("Riwayat Kesehatan Keluarga  : "));
            sbNotes.AppendLine(string.Format(" {0}   ", oChiefComplaint.FamilyHistory));

            if (oChiefComplaint.PatientVisitNoteID != null && oChiefComplaint.PatientVisitNoteID != 0)
            {

                string filterExpression = string.Format("VisitID = {0} AND NurseAssessmentID = {1} AND IsDeleted = 0", hdnVisitID.Value, hdnRecordID.Value);
                VitalSignHd lstVitalSignHd = BusinessLayer.GetVitalSignHdList(filterExpression).FirstOrDefault();
                if (lstVitalSignHd == null)
                {
                    sbNotes.AppendLine("");
                }
                else
                {
                    List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} ORDER BY DisplayOrder", lstVitalSignHd.ID));
                    if (lstVitalSignDt.Count > 0)
                    {
                        sbNotes.AppendLine(" ");
                        sbNotes.AppendLine("Objektif :");
                        sbNotes.AppendLine("-".PadRight(15, '-'));
                        sbNotes.AppendLine("Tanda Vital :");
                        foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                        {
                            sbNotes.AppendLine(string.Format(" {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                        }
                    }
                } 
            }

            return sbNotes.ToString();
        }
    }
}