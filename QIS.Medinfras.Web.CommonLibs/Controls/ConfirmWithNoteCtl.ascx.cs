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
    public partial class ConfirmWithNoteCtl : BaseProcessPopupCtl
    {
        protected string MRN = "";
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnPatientVisitNoteID.Value = paramInfo[0];
            txtParamedicName.Text = AppSession.UserLogin.UserFullName;
            txtNoteDateTime.Text = string.Format("{0} - {1}", DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));
            hdnSignatureIndex.Value = paramInfo[4];
            if (!string.IsNullOrEmpty(paramInfo[5]))
            {
                txtRemarks.Text = "";
            }
            else
            {
                txtRemarks.Text = paramInfo[5];
            }

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
                int id = Convert.ToInt32(hdnPatientVisitNoteID.Value);
                string referenceNo = string.Empty;
                bool isError = false;

                if (!string.IsNullOrEmpty(txtRemarks.Text))
                {
                    if (hdnSignatureIndex.Value == "2")
                    {
                        string retVal = ConfirmPlanningNote(hdnPatientVisitNoteID.Value);
                        string[] retValInfo = retVal.Split('|');
                        if (retValInfo[0] == "0")
                        {
                            isError = true;
                            errMessage = retValInfo[1];
                        }
                    }
                    else if (hdnSignatureIndex.Value == "4")
                    {
                        string retVal = VerifyPlanningNote(hdnPatientVisitNoteID.Value);
                        string[] retValInfo = retVal.Split('|');
                        if (retValInfo[0] == "0")
                        {
                            isError = true;
                            errMessage = retValInfo[1];
                        }
                    }
                    else
                    {
                        isError = false;
                        errMessage = string.Empty;
                    }

                    result = true;
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

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] paramInfo = e.Parameter.Split('|');

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            if (paramInfo[2] != "5")
            {
                result = UpdateDigitalSignature(paramInfo[0], paramInfo[1], paramInfo[2]);
            }
            else
            {
                result = UpdateDigitalSignatureAndVerifyAll(paramInfo[0], paramInfo[1], paramInfo[2]);
            }

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string UpdateDigitalSignatureAndVerifyAll(string visitNoteID, string streamData, string signatureIndex)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PatientVisitNoteDao visitNoteDao = new PatientVisitNoteDao(ctx);
            ESignatureDao signatureDao = new ESignatureDao(ctx);
            try
            {
                string filterExpression = string.Empty;
                vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                int cvLinkedID = 0;
                if (entityLinkedRegistration != null)
                {
                    cvLinkedID = entityLinkedRegistration.VisitID;
                }

                if (filterExpression != "")
                    filterExpression += " AND ";
                filterExpression += string.Format("VisitID IN ('{0}','{1}') AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, cvLinkedID);
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}','{3}','{4}') AND (IsVerified = 0 OR IsVerified IS NULL)", Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES);

                List<PatientVisitNote> lstVisitNote = BusinessLayer.GetPatientVisitNoteList(filterExpression, ctx);
                foreach (PatientVisitNote visitNote in lstVisitNote)
                {
                    bool isValid = true;
                    if ((visitNote.IsNeedConfirmation && !visitNote.IsConfirmed))
                        isValid = false;
                    if (isValid)
                    {
                        visitNote.IsVerified = true;
                        visitNote.VerifiedDateTime = DateTime.Now;
                        visitNoteDao.Update(visitNote);

                        string filterExp = string.Format("ReferenceID = {0}", visitNote.ID);
                        ESignature oSignature = BusinessLayer.GetESignatureList(filterExp, ctx).FirstOrDefault();
                        bool isNewRecord = oSignature != null;

                        if (isNewRecord)
                        {
                            oSignature = new ESignature();
                            oSignature.ReferenceID = visitNote.ID;
                            oSignature.GCSignatureType = Constant.ElectronicSignatureType.INTEGRATION_NOTES;
                        }

                        if (oSignature != null)
                        {
                            oSignature.Signature4 = streamData;
                            oSignature.Signature4DateTime = DateTime.Now;
                            oSignature.Signature4ID = AppSession.UserLogin.ParamedicID;
                        }

                        if (isNewRecord)
                            signatureDao.Insert(oSignature);
                        else
                            signatureDao.Update(oSignature);
                    }
                }
                ctx.CommitTransaction();

                string retVal = VerifyPlanningNote(hdnPatientVisitNoteID.Value);
                string[] retValInfo = retVal.Split('|');
                if (retValInfo[0] == "1")
                    result = string.Format("process|1|{0}|{1}|{2}", "Digital Signature was processed successfully", string.Empty, string.Empty);
                else
                    result = string.Format("process|0|{0}|{1}|{2}", retValInfo[1], string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}|{1}|{2}", ex.Message, string.Empty, string.Empty);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private string UpdateDigitalSignature(string visitNoteID,string streamData, string signatureIndex)
        {
            string result = string.Empty;

            try
            {
                vPatientVisitNote oVisitNote = BusinessLayer.GetvPatientVisitNoteList(string.Format("ID = {0}",hdnPatientVisitNoteID.Value)).FirstOrDefault();
                if (oVisitNote != null)
                {
                    bool isNewRecord = oVisitNote.SignatureID == 0;

                    ESignature oSignature;
                    if (isNewRecord)
                    {
                        oSignature = new ESignature();
                        oSignature.ReferenceID = Convert.ToInt32(hdnPatientVisitNoteID.Value);
                        oSignature.GCSignatureType = Constant.ElectronicSignatureType.INTEGRATION_NOTES;
                    }
                    else
                    {
                        oSignature = BusinessLayer.GetESignature(oVisitNote.SignatureID);
                    }

                    if (oSignature != null)
                    {
                        switch (signatureIndex)
                        {
                            case "1":
                                oSignature.Signature1 = streamData;
                                oSignature.Signature1DateTime = DateTime.Now;
                                oSignature.Signature1ID = AppSession.UserLogin.ParamedicID;
                                break;
                            case "2":
                                oSignature.Signature2 = streamData;
                                oSignature.Signature2DateTime = DateTime.Now;
                                oSignature.Signature2ID = AppSession.UserLogin.ParamedicID;
                                break;
                            case "3":
                                oSignature.Signature3 = streamData;
                                oSignature.Signature3DateTime = DateTime.Now;
                                oSignature.Signature3ID = AppSession.UserLogin.ParamedicID;
                                break;
                            case "4":
                                //Verifikasi DPJP Utama
                                oSignature.Signature4 = streamData;
                                oSignature.Signature4DateTime = DateTime.Now;
                                oSignature.Signature4ID = AppSession.UserLogin.ParamedicID;
                                break;
                            default:
                                break;
                        }
                    }

                    if (isNewRecord)
                        BusinessLayer.InsertESignature(oSignature);
                    else
                        BusinessLayer.UpdateESignature(oSignature);

                    if (signatureIndex == "2")
                    {
                        string retVal = ConfirmPlanningNote(hdnPatientVisitNoteID.Value);
                        string[] retValInfo = retVal.Split('|');
                        if (retValInfo[0] == "1")
                            result = string.Format("process|1|{0}|{1}|{2}", "Digital Signature was processed successfully", string.Empty, string.Empty);
                        else
                            result = string.Format("process|0|{0}|{1}|{2}", retValInfo[1], string.Empty, string.Empty);

                    }
                    else if (signatureIndex == "4")
                    {
                        string retVal = VerifyPlanningNote(hdnPatientVisitNoteID.Value);
                        string[] retValInfo = retVal.Split('|');
                        if (retValInfo[0] == "1")
                            result = string.Format("process|1|{0}|{1}|{2}", "Digital Signature was processed successfully", string.Empty, string.Empty);
                        else
                            result = string.Format("process|0|{0}|{1}|{2}", retValInfo[1], string.Empty, string.Empty);                        
                    }
                    else
                    {
                        result = string.Format("process|1|{0}|{1}|{2}", "Digital Signature was processed successfully", string.Empty, string.Empty);
                    }
                }
                else
                {
                    result = string.Format("process|0|{0}|{1}|{2}", "Invalid link to digital signature", string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}|{1}|{2}", ex.Message, string.Empty, string.Empty);
            }

            return result;
        }

        private string ConfirmPlanningNote(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                PatientVisitNote oVisitNote = BusinessLayer.GetPatientVisitNote(id);
                if (oVisitNote != null)
                {
                    if (oVisitNote.ConfirmationPhysicianID == AppSession.RegisteredPatient.ParamedicID)
                    {
                        //Jika DPJP sekalian verified
                        oVisitNote.IsVerified = true;
                        oVisitNote.VerifiedBy = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        oVisitNote.VerifiedDateTime = DateTime.Now;
                    }
                    oVisitNote.IsConfirmed = true;
                    oVisitNote.ConfirmationDateTime = DateTime.Now;
                    oVisitNote.ConfirmationRemarks = txtRemarks.Text;
                    BusinessLayer.UpdatePatientVisitNote(oVisitNote);
                }

                result = string.Format("1|{0}", string.Empty);
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}", ex.Message);
            }
            finally
            {
            }
            return result;
        }


        private string VerifyPlanningNote(string recordID)
        {
            string result = "1|";

            IDbContext ctx = DbFactory.Configure(true);
            PatientVisitNoteDao oVisitNoteDao = new PatientVisitNoteDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                bool isValid = true;
                bool isNurseInitialAssessment = false;
                int nurseChiefComplaintID = 0;
                PatientVisitNote oVisitNote = oVisitNoteDao.Get(id);
                if (oVisitNote != null)
                {
                    if (oVisitNote.IsNeedConfirmation && !oVisitNote.IsConfirmed && oVisitNote.ConfirmationPhysicianID != AppSession.UserLogin.ParamedicID)
                    {
                        result = string.Format("0|{0}", "Catatan Terintegrasi belum di-readback oleh Dokter yang dituju!");
                        isValid = false;
                    }
                    else
                    {
                        if (oVisitNote.GCPatientNoteType == Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT)
                        {
                            NurseChiefComplaint oNurseChiefComplaint = BusinessLayer.GetNurseChiefComplaintList(string.Format("PatientVisitNoteID = {0}", oVisitNote.ID),ctx).FirstOrDefault();
                            if (oNurseChiefComplaint != null)
                            {
                                if (oNurseChiefComplaint.GCAssessmentStatus == Constant.AssessmentStatus.OPEN)
                                {
                                    result = string.Format("0|{0}", "Proses kajian awal perawat belum selesai dilakukan!");
                                    isValid = false;
                                }
                                else
                                {
                                    nurseChiefComplaintID = oNurseChiefComplaint.ID;
                                    isNurseInitialAssessment = true;
                                }
                            }
                            else
                            {
                                nurseChiefComplaintID = 0;
                                isNurseInitialAssessment = false;
                            }
                        }
                    }

                    if (isValid)
                    {
                        if (AppSession.UserLogin.ParamedicID == AppSession.RegisteredPatient.ParamedicID)
                        {
                            //Jika DPJP sekalian verified
                            oVisitNote.IsVerified = true;
                            oVisitNote.VerifiedBy = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            oVisitNote.VerifiedDateTime = DateTime.Now;
                            oVisitNote.VerificationRemarks = txtRemarks.Text;
                            oVisitNoteDao.Update(oVisitNote);

                            if (isNurseInitialAssessment)
                            {
                                NurseChiefComplaintDao oNurseChiefComplaintDao = new NurseChiefComplaintDao(ctx);
                                NurseChiefComplaint oNurseChiefComplaint = oNurseChiefComplaintDao.Get(nurseChiefComplaintID);
                                if (oNurseChiefComplaint != null)
                                {
                                    oNurseChiefComplaint.IsVerifiedByRegisteredPhysician = true;
                                    oNurseChiefComplaint.RegisteredPhysicianID = AppSession.UserLogin.ParamedicID;
                                    oNurseChiefComplaint.RegisteredPhysicianVerifiedDateTime = DateTime.Now;
                                    oNurseChiefComplaint.RegisteredPhysicianVerificationRemarks = txtRemarks.Text;
                                    oNurseChiefComplaintDao.Update(oNurseChiefComplaint);
                                }
                            }
                            result = string.Format("1|{0}", string.Empty); 
                        }
                    }
                }
                ctx.CommitTransaction();
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
    }
}