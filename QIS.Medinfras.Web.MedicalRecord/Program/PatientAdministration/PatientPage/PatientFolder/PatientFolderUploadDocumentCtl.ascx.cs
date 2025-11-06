using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Web.UI;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxUploadControl;
using System.IO;
using System.Drawing.Drawing2D;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxFileManager;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class PatientFolderUploadDocumentCtl : BaseEntryPopupCtl
    {
        private string[] listImageType = new string[6] { "jpg", "jpeg", "png", "gif", "bmp", "mp4" };
        const string ThumbnailFileName = "thumbPreview.jpg";

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            IsAdd = true;
            SetControlProperties();
            SetPathFolder(param); 

          
        }
        private void SetPathFolder(string param)
        {
            string[] pathFolder = param.Split('\\');
            if (pathFolder.Length > 1) //ambil setelah medical no 
            {
                string path = "";
                for (int i = 1; i < pathFolder.Length; i++)
                {
                    if (!string.IsNullOrEmpty(pathFolder[i]))
                    {
                        path += string.Format("{0}\\", pathFolder[i]);
                    }
                }
                hdnFolder.Value = path; 
            }
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
            SetControlEntrySetting(txtRename1, new ControlEntrySetting(true, true, true));
			SetControlEntrySetting(hdnFolder, new ControlEntrySetting(true, true, false));
        }

        private void ControlToEntity(PatientDocument entity)
        {
            entity.VisitID = null;
            entity.GCDocumentType = cboGCDocumentType.Value.ToString();
            entity.DocumentName = txtDocumentName1.Text;
            entity.DocumentDate = Helper.GetDatePickerValue(txtDocumentDate);
            entity.Notes = txtNotes.Text;
            entity.FileName = string.Format("{0}", txtRename1.Text);
            entity.DocumentPath = hdnFolder.Value;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                int mrn = AppSession.PatientDetail.MRN;
                Patient patient = BusinessLayer.GetPatient(mrn);

                string imageData = hdnUploadedFile1.Value;
                string fileType = imageData.Split(';')[0].Split('/')[1];

                string fileName = String.Format("{0}", txtRename1.Text);
                string fileNameExt = fileName.Split('.').LastOrDefault().ToLower();

                if (fileType.ToLower() != "msword" && fileType.ToLower() != "plain" && fileType.ToLower() != "jpg" && fileType.ToLower() != "jpeg" && fileType.ToLower() != "png" && fileType.ToLower() != "txt" && fileType.ToLower() != "doc" && fileType.ToLower() != "docx" && fileType.ToLower() != "pdf" && fileType.ToLower() != "mp4" && fileType.ToLower() != "dcm")
                {
                    errMessage = "Ekstension yang digunakan tidak sesuai dengan yang diizinkan.";
                    return false;
                }
                else if (fileNameExt != "jpg" && fileNameExt != "jpeg" && fileNameExt != "png" && fileNameExt != "txt" && fileNameExt != "doc" && fileNameExt != "docx" && fileNameExt != "pdf" && fileNameExt != "mp4" && fileNameExt != "dcm")
                {
                    errMessage = "Ekstension yang digunakan tidak sesuai dengan yang diizinkan.";
                    return false;
                }
                else
                {
                    if (imageData != "")
                    {
                        string[] parts = Regex.Split(imageData, ",").Skip(1).ToArray();
                        imageData = String.Join(",", parts);
                    }

                    string path = AppConfigManager.QISPhysicalDirectory;
                    if (!string.IsNullOrEmpty(hdnParam.Value) && hdnParam.Value.Contains("\\"))
                    {
                        path += string.Format("Patient\\{0}\\", hdnParam.Value);

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                    }
                    else
                    {
                        path += string.Format("{0}\\", AppConfigManager.QISPatientDocumentsPath.Replace('/', '\\'));

                        path = path.Replace("#MRN", patient.MedicalNo);
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                    }

                    //string fileName = String.Format("{0}", txtRename1.Text);
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
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }

        }
    }
}