using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;
using System.IO;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.MCU.Program
{
    public partial class NewPatientDocumentCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            SetControlProperties();
            if (param != "")
            {
                IsAdd = false;
                hdnID.Value = param;
                SetControlProperties();
                PatientDocument entity = BusinessLayer.GetPatientDocument(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
            }
        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}')", Constant.StandardCode.DOCUMENT_TYPE, Constant.StandardCode.E_DOCUMENT_FILE_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCDocumentType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOCUMENT_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFileType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.E_DOCUMENT_FILE_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDocumentDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(cboGCDocumentType, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtDocumentName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRename1, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(PatientDocument entity)
        {
            txtDocumentDate.Text = entity.DocumentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            cboGCDocumentType.Value = entity.GCDocumentType.ToString();
            if (!string.IsNullOrEmpty(entity.GCFileType))
            {
                cboFileType.Value = entity.GCFileType.ToString(); 
            }
            txtDocumentName.Text = entity.DocumentName;
            txtRename1.Text = entity.FileName;
        }

        private void ControlToEntity(PatientDocument entity)
        {
            string[] fileInfo = txtRename1.Text.Split('.');

            entity.MRN = AppSession.RegisteredPatient.MRN;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.GCDocumentType = cboGCDocumentType.Value.ToString();
            entity.DocumentName = txtDocumentName.Text;
            entity.DocumentDate = Helper.GetDatePickerValue(txtDocumentDate);
            entity.Notes = txtNotes.Text;
            entity.FileName = string.Format("{0}_{1}.{2}", fileInfo[0],entity.DocumentDate.ToString(Constant.FormatString.DATE_FORMAT_112), fileInfo[1]);
            entity.GCFileType = cboFileType.Value.ToString();
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                int mrn = AppSession.RegisteredPatient.MRN;
                Patient patient = BusinessLayer.GetPatient(mrn);

                string imageData = hdnUploadedFile1.Value;
                if (imageData != "")
                {
                    string[] parts = Regex.Split(imageData, ",").Skip(1).ToArray();
                    imageData = String.Join(",", parts);
                }

                string path = AppConfigManager.QISPhysicalDirectory;
                path += string.Format("{0}\\{1}\\", AppConfigManager.QISPatientDocumentsPath.Replace('/', '\\'), hdnParam.Value);

                path = path.Replace("#MRN", patient.MedicalNo);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string[] fileInfo = txtRename1.Text.Split('.');
                string fileName = string.Format("{0}_{1}.{2}", fileInfo[0], Helper.GetDatePickerValue(txtDocumentDate).ToString(Constant.FormatString.DATE_FORMAT_112), fileInfo[1]);

                FileStream fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                byte[] data = Convert.FromBase64String(imageData);
                bw.Write(data);
                bw.Close();

                PatientDocument entity = new PatientDocument();
                ControlToEntity(entity);
                entity.MRN = mrn;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPatientDocument(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                int mrn = AppSession.RegisteredPatient.MRN;
                Patient patient = BusinessLayer.GetPatient(mrn);

                if (hdnUploadedFile1.Value != "")
                {
                    string imageData = hdnUploadedFile1.Value;
                    if (imageData != "")
                    {
                        string[] parts = Regex.Split(imageData, ",").Skip(1).ToArray();
                        imageData = String.Join(",", parts);
                    }

                    string path = AppConfigManager.QISPhysicalDirectory;
                    path += string.Format("{0}\\{1}\\", AppConfigManager.QISPatientDocumentsPath.Replace('/', '\\'), hdnParam.Value);

                    path = path.Replace("#MRN", patient.MedicalNo);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string[] fileInfo = txtRename1.Text.Split('.');
                    string fileName = string.Format("{0}_{1}.{2}", fileInfo[0], Helper.GetDatePickerValue(txtDocumentDate).ToString(Constant.FormatString.DATE_FORMAT_112), fileInfo[1]);

                    FileStream fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);

                    byte[] data = Convert.FromBase64String(imageData);
                    bw.Write(data);
                    bw.Close();
                }

                PatientDocument entity = BusinessLayer.GetPatientDocument(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.MRN = mrn;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientDocument(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}