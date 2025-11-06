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
    public partial class DigitalSignatureCtl1 : BaseViewPopupCtl
    {
        protected string MRN = "";
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnReferenceID.Value = paramInfo[0];
            txtSignatureName.Text = paramInfo[1];
            txtNoteDateTime.Text = string.Format("{0} - {1}", paramInfo[2], paramInfo[3]);
            hdnSignatureIndex.Value = paramInfo[4];

            if (paramInfo.Length >= 5)
            {
                hdnReferenceIDType.Value = paramInfo[5];
            }
            else
            {
                hdnReferenceIDType.Value = "1";
            }

            hdnSignatureName.Value = paramInfo[1];
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] paramInfo = e.Parameter.Split('|');

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            switch (hdnReferenceIDType.Value)
            {
                case "02": // Form Signature
                    result = UpdateFormDigitalSignature(paramInfo[0], paramInfo[1], paramInfo[2]);
                    break;
                case "03": // Consent Form Signature
                    result = UpdateConsentFormDigitalSignature(paramInfo[1], hdnSignatureIndex.Value);
                    break;
                default:
                    if (paramInfo[2] != "5")
                    {
                        result = UpdateDigitalSignature(paramInfo[0], paramInfo[1], paramInfo[2]);
                    }
                    else
                    {
                        result = UpdateDigitalSignatureAndVerifyAll(paramInfo[0], paramInfo[1], paramInfo[2]);
                    }
                    break;
            }

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string UpdateFormDigitalSignature(string visitNoteID, string streamData, string signatureIndex)
        {
            string result = string.Empty;

            try
            {
                bool isNewRecord = false;
                string GCSignatureType = Constant.ElectronicSignatureType.INTEGRATION_NOTES;
                switch (hdnReferenceIDType.Value)
                {
                    case "01":
                        GCSignatureType = Constant.ElectronicSignatureType.INTEGRATION_NOTES;
                        break;
                    case "02":
                        GCSignatureType = Constant.ElectronicSignatureType.PATIENT_EDUCATION;
                        break;
                    case "03":
                        GCSignatureType = Constant.ElectronicSignatureType.CONSENT_FORM;
                        break;
                    default:
                        GCSignatureType = Constant.ElectronicSignatureType.INTEGRATION_NOTES;
                        break;
                }
                ESignature oSignature = BusinessLayer.GetESignatureList(string.Format("ReferenceID = {0} AND GCSignatureType = '{1}'", hdnReferenceID.Value, GCSignatureType)).FirstOrDefault();
                if (oSignature == null)
                {
                    oSignature = new ESignature();
                    isNewRecord = true;

                    oSignature.ReferenceID = Convert.ToInt32(hdnReferenceID.Value);
                    switch (hdnReferenceIDType.Value)
                    {
                        case "01":
                            oSignature.GCSignatureType = Constant.ElectronicSignatureType.INTEGRATION_NOTES;
                            break;
                        case "02":
                            oSignature.GCSignatureType = Constant.ElectronicSignatureType.PATIENT_EDUCATION;
                            break;
                        default:
                            oSignature.GCSignatureType = Constant.ElectronicSignatureType.INTEGRATION_NOTES;
                            break;
                    }
                }

                switch (signatureIndex)
                {
                    case "1":
                        oSignature.Signature1 = streamData;
                        oSignature.Signature1DateTime = DateTime.Now;
                        if (hdnReferenceIDType.Value == "01" || hdnReferenceIDType.Value == "02")
                        {
                            oSignature.Signature1ID = AppSession.UserLogin.ParamedicID;
                        }
                        break;
                    case "2":
                        oSignature.Signature2 = streamData;
                        oSignature.Signature2DateTime = DateTime.Now;
                        if (hdnReferenceIDType.Value == "01")
                        {
                            oSignature.Signature2ID = AppSession.UserLogin.ParamedicID;
                        }
                        break;
                    case "3":
                        oSignature.Signature3 = streamData;
                        oSignature.Signature3DateTime = DateTime.Now;
                        if (hdnReferenceIDType.Value == "01")
                        {
                            oSignature.Signature3ID = AppSession.UserLogin.ParamedicID;
                        }
                        break;
                    case "4":
                        //Verifikasi DPJP Utama
                        oSignature.Signature4 = streamData;
                        oSignature.Signature4DateTime = DateTime.Now;
                        if (hdnReferenceIDType.Value == "01" )
                        {
                            oSignature.Signature4ID = AppSession.UserLogin.ParamedicID;
                        }
                        break;
                    default:
                        break;
                }

                if (isNewRecord)
                    BusinessLayer.InsertESignature(oSignature);
                else
                    BusinessLayer.UpdateESignature(oSignature);

                result = string.Format("process|1|{0}|{1}|{2}", "Digital Signature was processed successfully", string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}|{1}|{2}", ex.Message, string.Empty, string.Empty);
            }

            return result;
        }


        private string UpdateConsentFormDigitalSignature(string streamData, string signatureIndex)
        {
            string result = string.Empty;

            try
            {
                bool isNewRecord = false;
                string GCSignatureType = Constant.ElectronicSignatureType.INTEGRATION_NOTES;
                switch (hdnReferenceIDType.Value)
                {
                    case "01":
                        GCSignatureType = Constant.ElectronicSignatureType.INTEGRATION_NOTES;
                        break;
                    case "02":
                        GCSignatureType = Constant.ElectronicSignatureType.PATIENT_EDUCATION;
                        break;
                    case "03":
                        GCSignatureType = Constant.ElectronicSignatureType.CONSENT_FORM;
                        break;
                    default:
                        GCSignatureType = Constant.ElectronicSignatureType.INTEGRATION_NOTES;
                        break;
                }
                ESignature oSignature = BusinessLayer.GetESignatureList(string.Format("ReferenceID = {0} AND GCSignatureType = '{1}'", hdnReferenceID.Value, GCSignatureType)).FirstOrDefault();
                if (oSignature == null)
                {
                    oSignature = new ESignature();
                    isNewRecord = true;

                    oSignature.ReferenceID = Convert.ToInt32(hdnReferenceID.Value);
                    switch (hdnReferenceIDType.Value)
                    {
                        case "01":
                            oSignature.GCSignatureType = Constant.ElectronicSignatureType.INTEGRATION_NOTES;
                            break;
                        case "02":
                            oSignature.GCSignatureType = Constant.ElectronicSignatureType.PATIENT_EDUCATION;
                            break;
                        case "03":
                            oSignature.GCSignatureType = Constant.ElectronicSignatureType.CONSENT_FORM;
                            break;
                        default:
                            oSignature.GCSignatureType = Constant.ElectronicSignatureType.INTEGRATION_NOTES;
                            break;
                    }
                }

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
                        break;
                    case "3":
                        oSignature.Signature3 = streamData;
                        oSignature.Signature3DateTime = DateTime.Now;
                        break;
                    case "4":
                        oSignature.Signature4 = streamData;
                        oSignature.Signature4DateTime = DateTime.Now;
                        break;
                    default:
                        break;
                }

                if (isNewRecord)
                    BusinessLayer.InsertESignature(oSignature);
                else
                    BusinessLayer.UpdateESignature(oSignature);

                result = string.Format("process|1|{0}|{1}|{2}", "Digital Signature was processed successfully", string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}|{1}|{2}", ex.Message, string.Empty, string.Empty);
            }

            return result;
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

                string retVal = VerifyPlanningNote(hdnReferenceID.Value);
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
                vPatientVisitNote oVisitNote = BusinessLayer.GetvPatientVisitNoteList(string.Format("ID = {0}",hdnReferenceID.Value)).FirstOrDefault();
                if (oVisitNote != null)
                {
                    bool isNewRecord = oVisitNote.SignatureID == 0;

                    ESignature oSignature;
                    if (isNewRecord)
                    {
                        oSignature = new ESignature();
                        oSignature.ReferenceID = Convert.ToInt32(hdnReferenceID.Value);
                        switch (hdnReferenceIDType.Value)
                        {
                            case "01":
                                oSignature.GCSignatureType = Constant.ElectronicSignatureType.INTEGRATION_NOTES;
                                break;
                            case "02":
                                oSignature.GCSignatureType = Constant.ElectronicSignatureType.PATIENT_EDUCATION;
                                break;
                            default:
                                oSignature.GCSignatureType = Constant.ElectronicSignatureType.INTEGRATION_NOTES;
                                break;
                        }

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
                                if (hdnReferenceIDType.Value == "01" || hdnReferenceIDType.Value == "02")
                                {
                                    oSignature.Signature1ID = AppSession.UserLogin.ParamedicID; 
                                }
                                break;
                            case "2":
                                oSignature.Signature2 = streamData;
                                oSignature.Signature2DateTime = DateTime.Now;
                                if (hdnReferenceIDType.Value == "01")
                                {
                                    oSignature.Signature2ID = AppSession.UserLogin.ParamedicID; 
                                }
                                break;
                            case "3":
                                oSignature.Signature3 = streamData;
                                oSignature.Signature3DateTime = DateTime.Now;
                                if (hdnReferenceIDType.Value == "01")
                                {
                                    oSignature.Signature3ID = AppSession.UserLogin.ParamedicID;
                                }
                                break;
                            case "4":
                                //Verifikasi DPJP Utama
                                oSignature.Signature4 = streamData;
                                oSignature.Signature4DateTime = DateTime.Now;
                                if (hdnReferenceIDType.Value == "01")
                                {
                                    oSignature.Signature4ID = AppSession.UserLogin.ParamedicID;
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    if (isNewRecord)
                        BusinessLayer.InsertESignature(oSignature);
                    else
                        BusinessLayer.UpdateESignature(oSignature);

                    if (hdnReferenceIDType.Value == "01") //Integration Note
                    {
                        if (signatureIndex == "2")
                        {
                            string retVal = ConfirmPlanningNote(hdnReferenceID.Value);
                            string[] retValInfo = retVal.Split('|');
                            if (retValInfo[0] == "1")
                                result = string.Format("process|1|{0}|{1}|{2}", "Digital Signature was processed successfully", string.Empty, string.Empty);
                            else
                                result = string.Format("process|0|{0}|{1}|{2}", retValInfo[1], string.Empty, string.Empty);

                        }
                        else if (signatureIndex == "4")
                        {
                            string retVal = VerifyPlanningNote(hdnReferenceID.Value);
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
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                PatientVisitNote oVisitNote = BusinessLayer.GetPatientVisitNote(id);
                if (oVisitNote != null)
                {
                    if (oVisitNote.IsNeedConfirmation && !oVisitNote.IsConfirmed && oVisitNote.ConfirmationPhysicianID != AppSession.UserLogin.ParamedicID)
                    {
                        result = string.Format("0|{0}", "Catatan Terintegrasi belum di-readback oleh Dokter yang dituju!");
                    }
                    else
                    {
                        if (AppSession.UserLogin.ParamedicID == AppSession.RegisteredPatient.ParamedicID)
                        {
                            //Jika DPJP sekalian verified
                            oVisitNote.IsVerified = true;
                            oVisitNote.VerifiedBy = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            oVisitNote.VerifiedDateTime = DateTime.Now;
                        }
                        BusinessLayer.UpdatePatientVisitNote(oVisitNote);

                        result = string.Format("1|{0}", string.Empty);
                    }
                }

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
    }
}