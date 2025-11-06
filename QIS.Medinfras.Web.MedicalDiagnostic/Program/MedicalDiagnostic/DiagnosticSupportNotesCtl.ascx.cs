using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.MedicalDiagnostic.Program
{
    public partial class DiagnosticSupportNotesCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] localParam = param.Split('|');
                string registrationID = localParam[0];
                hdnHealthcareServiceUnitIDCtl.Value = localParam[1];
                vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", registrationID)).FirstOrDefault();
                txtMRN.ReadOnly = true;
                txtPatientName.ReadOnly = true;
                txtMRN.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;
                ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", registrationID)).FirstOrDefault();
                hdnVisitID.Value = entityCV.VisitID.ToString();
                BindGridView();
                SetControlProperties();
            } 
        }

        private void SetControlProperties()
        {
            txtVisitNoteDate.Attributes.Add("validationgroup", "mpPatientVisitNotes");
            txtVisitNoteTime.Attributes.Add("validationgroup", "mpPatientVisitNotes");
            txtNoteText.Attributes.Add("validationgroup", "mpPatientVisitNotes");
            txtVisitNoteDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtVisitNoteTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        }

        private void BindGridView()
        {
            grdVisitNotes.DataSource = BusinessLayer.GetvPatientVisitNoteList(string.Format("VisitID = {0} AND IsDeleted = 0 AND GCPatientNoteType = '{1}'", hdnVisitID.Value, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES));
            grdVisitNotes.DataBind();
        }

        protected void cbpPatientVisitNotes_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnVisitNoteID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else
            {
                result = "delete|";
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PatientVisitNote entity)
        {
            entity.NoteDate = Helper.GetDatePickerValue(txtVisitNoteDate);
            entity.NoteTime = txtVisitNoteTime.Text;
            entity.NoteText = txtNoteText.Text;
            entity.GCPatientNoteType = Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES;
            entity.ParamedicID = null;
            entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitIDCtl.Value);
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PatientVisitNote entity = new PatientVisitNote();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPatientVisitNote(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PatientVisitNote entity = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnVisitNoteID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientVisitNote(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PatientVisitNote entity = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnVisitNoteID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientVisitNote(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }
    }
}