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
    public partial class ReferralNotesCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
                txtMRN.ReadOnly = true;
                txtPatientName.ReadOnly = true;
                txtMRN.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;
                ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", param)).FirstOrDefault();
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

            //List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0", Constant.StandardCode.TITLE, Constant.StandardCode.SALUTATION, Constant.StandardCode.SUFFIX, Constant.StandardCode.FAMILY_RELATION));
            //lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            //Methods.SetComboBoxField<StandardCode>(cboSalutation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SALUTATION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboTitle, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.TITLE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboSuffix, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SUFFIX || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboFamilyRelation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.FAMILY_RELATION).ToList(), "StandardCodeName", "StandardCodeID");
            //cboSalutation.SelectedIndex = 0;
            //cboTitle.SelectedIndex = 0;
            //cboSuffix.SelectedIndex = 0;
            //cboFamilyRelation.SelectedIndex = 0;
        }

        private void BindGridView()
        {
            grdVisitNotes.DataSource = BusinessLayer.GetvPatientVisitNoteList(
                string.Format("VisitID = {0} AND IsDeleted = 0 AND GCPatientNoteType = '{1}'", hdnVisitID.Value, Constant.PatientVisitNotes.REFERRAL_FROM_NOTES));
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
            entity.GCPatientNoteType = Constant.PatientVisitNotes.REFERRAL_FROM_NOTES;
            entity.ParamedicID = null;
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