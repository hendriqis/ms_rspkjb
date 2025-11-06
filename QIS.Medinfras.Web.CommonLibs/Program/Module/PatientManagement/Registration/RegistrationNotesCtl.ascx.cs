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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RegistrationNotesCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                vConsultVisit9 entityCV = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0} AND IsMainVisit = 1", param)).FirstOrDefault();
                hdnVisitID.Value = entityCV.VisitID.ToString();
                txtMRN.ReadOnly = true;
                txtPatientName.ReadOnly = true;
                txtMRN.Text = entityCV.MedicalNo;
                txtPatientName.Text = entityCV.PatientName;

                BindGridView();
                SetControlProperties();
            } 
        }

        private void SetControlProperties()
        {
            txtVisitNoteDate.Attributes.Add("validationgroup", "mpPatientVisitNotes");
            txtVisitNoteTime.Attributes.Add("validationgroup", "mpPatientVisitNotes");
            cboGCNoteType.Attributes.Add("validationgroup", "mpPatientVisitNotes");
            txtNoteText.Attributes.Add("validationgroup", "mpPatientVisitNotes");

            txtVisitNoteDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtVisitNoteTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            String filterExpression = string.Format("ParentID IN ('{0}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.PATIENT_NOTE_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboGCNoteType, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboGCNoteType.SelectedIndex = 0;
        }

        private void BindGridView()
        {
            grdVisitNotes.DataSource = BusinessLayer.GetvPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", hdnVisitID.Value, Constant.PatientVisitNotes.REGISTRATION_NOTES));
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
            entity.GCPatientNoteType = Constant.PatientVisitNotes.REGISTRATION_NOTES;
            entity.ParamedicID = null;
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.GCNoteType = cboGCNoteType.Value.ToString();
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
                Helper.InsertErrorLog(ex);
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
                Helper.InsertErrorLog(ex);
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
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }
    }
}