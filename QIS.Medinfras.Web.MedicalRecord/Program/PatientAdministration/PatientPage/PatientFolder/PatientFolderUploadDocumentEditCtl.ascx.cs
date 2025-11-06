using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Web.UI;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class PatientFolderUploadDocumentEditCtl : BaseEntryPopupCtl
    {
        private string[] listImageType = new string[5] { "jpg", "jpeg", "png", "gif", "bmp" };
        const string ThumbnailFileName = "thumbPreview.jpg";

        public override void InitializeDataControl(string param)
        {
            string[] par = param.Split('|');
            hdnParam.Value = par[0];
            string[] fileInfo = par[1].Split('\\');
            txtFileName.Text = fileInfo[fileInfo.Length - 1];
            IsAdd = false;

            PatientDocument entity = BusinessLayer.GetPatientDocumentList(string.Format("FileName = '{0}'", txtFileName.Text))[0];
            hdnID.Value = entity.ID.ToString();

            SetControlProperties();

            EntityToControl(entity);
        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}')", Constant.StandardCode.DOCUMENT_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCDocumentType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOCUMENT_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboGCDocumentType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDocumentName1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDocumentDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFileName, new ControlEntrySetting(false, false, true));
        }

        private void EntityToControl(PatientDocument entity)
        {
            cboGCDocumentType.Value = entity.GCDocumentType;
            txtDocumentName1.Text = entity.DocumentName;
            txtDocumentDate.Text = entity.DocumentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNotes.Text = entity.Notes;

            string path = AppConfigManager.QISPhysicalDirectory;
            path += string.Format("Patient\\{0}\\", hdnParam.Value);
            path = path.Replace('/', '\\');

            previewImage.Src = path;
        }

        private void ControlToEntity(PatientDocument entity)
        {
            entity.VisitID = null;
            entity.GCDocumentType = cboGCDocumentType.Value.ToString();
            entity.DocumentName = txtDocumentName1.Text;
            entity.DocumentDate = Helper.GetDatePickerValue(txtDocumentDate);
            entity.Notes = txtNotes.Text;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                PatientDocument entity = BusinessLayer.GetPatientDocument(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientDocument(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }

        }
    }
}