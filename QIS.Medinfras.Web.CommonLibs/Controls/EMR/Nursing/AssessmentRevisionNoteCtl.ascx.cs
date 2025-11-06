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
    public partial class AssessmentRevisionNoteCtl : BaseProcessPopupCtl
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

                if (!string.IsNullOrEmpty(txtRemarks.Text))
                {
                    string retVal = ProcessRecord(hdnRecordID.Value);
                    string[] retValInfo = retVal.Split('|');
                    if (retValInfo[0] == "0")
                    {
                        isError = true;
                        errMessage = retValInfo[1];
                    }
                }
                else
                {
                    isError = true;
                    errMessage = "Catatan harus diisi";
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
                            //Update old visit note to deleted
                            oNote.IsDeleted = true;
                            oVisitNoteDao.Update(oNote);

                            oVisitNote = oNote;
                            isCopyFromPrevious = true;
                        }
                    }

                    if (isCopyFromPrevious)
                    {
                        oVisitNote.NoteDate = oRecord.ChiefComplaintDate;
                        oVisitNote.NoteTime = oRecord.ChiefComplaintTime;
                        visitNoteID = oVisitNoteDao.InsertReturnPrimaryKeyID(oVisitNote);
                    }
                    else
                    {
                        oVisitNote.VisitID = AppSession.RegisteredPatient.VisitID;
                        oVisitNote.NoteDate = oRecord.ChiefComplaintDate;
                        oVisitNote.NoteTime = oRecord.ChiefComplaintTime;
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
                    oNewRecord.RevisionDate = DateTime.Now;
                    oNewRecord.RevisionRemarks = txtRemarks.Text;
                    oNewRecord.ParamedicID = oRecord.ParamedicID;
                    oNewRecord.RevisedByParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                    oNewRecord.PatientVisitNoteID = visitNoteID;
                    oNewRecord.GCAssessmentStatus = Constant.AssessmentStatus.OPEN;
                    oNurseCCDao.Insert(oNewRecord);

                    //Update Old Record
                    oRecord.ID = id;
                    oRecord.IsRevised = true;
                    oRecord.GCAssessmentStatus = Constant.AssessmentStatus.DIREVISI;
                    oRecord.RevisedByParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                    oRecord.RevisionDate = DateTime.Now;
                    oRecord.RevisionRemarks = txtRemarks.Text;
                    oRecord.LastUpdatedBy = AppSession.UserLogin.UserID;
                    oNurseCCDao.Update(oRecord);

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
                VitalSignHd lstVitalSignHd = BusinessLayer.GetVitalSignHdList(string.Format("VisitID = {0} AND PatientVisitNoteID = {1} AND IsDeleted = 0 ORDER BY ID", AppSession.RegisteredPatient.VisitID, oChiefComplaint.PatientVisitNoteID)).FirstOrDefault();
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