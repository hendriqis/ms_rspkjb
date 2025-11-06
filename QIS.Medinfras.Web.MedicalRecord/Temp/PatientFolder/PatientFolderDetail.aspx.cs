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

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class PatientFolderDetail : BasePage
    {
        protected int MRN = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                MRN = Convert.ToInt32(Page.Request.QueryString["id"]);
                vPatient entity = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", MRN))[0];
                EntityToControl(entity);

                string path = string.Format("{0}{1}Image", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISPatientImagePath.Replace('/', '\\').Replace("#MRN", entity.MedicalNo));
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                fileManager.Settings.RootFolder = path;
                
                //fileManager.Settings.ThumbnailFolder = string.Format("{0}Patient\\{1}", AppSession.SettingParameter.FirstOrDefault(p => p.Code == Constant.SettingParameter.PHYSICAL_DIR).Value, entity.MedicalNo);
                fileManager.Settings.InitialFolder = "Image";
            }
        }

        private void EntityToControl(vPatient entity)
        {
            hdnPatientGender.Value = entity.GCSex;
            imgPatientProfilePicture.Src = entity.PatientImageUrl;
            lblPatientName.InnerHtml = entity.PatientName;

            lblMRN.InnerHtml = entity.MedicalNo;
            lblDOB.InnerHtml = entity.DateOfBirthInString;
            lblPatientAge.InnerHtml = Helper.GetPatientAge(words, entity.DateOfBirth);
            lblGender.InnerHtml = entity.Sex;
        }

        protected void fileManager_CustomThumbnail(object source, FileManagerThumbnailCreateEventArgs e)
        {
            string fileName = e.File.FullName.Replace(AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISVirtualDirectory);
            fileName = fileName.Replace('\\', '/');
            e.ThumbnailImage.Url = fileName;
        }

        protected void cbUploadEnabled_CheckedChanged(object sender, EventArgs e)
        {
            //fileManager.SettingsUpload.Enabled = cbUploadEnabled.Checked;
        }

        protected void fileManager_ItemRenaming(object sender, FileManagerItemRenameEventArgs e)
        {
            string folderName = e.Item.RelativeName.Split('\\')[0];
            if (hdnIsFolder.Value == "1")
            {
                string oldName = e.Item.RelativeName;
                string[] param = e.Item.RelativeName.Split('\\');
                string path = String.Join("\\", param.Take(param.Length - 1));
                if (path != "")
                    path += "\\";
                string newName = string.Format("{0}{1}", path, e.NewName);

                MRN = Convert.ToInt32(Page.Request.QueryString["id"]);
                List<PatientDocument> lstEntity = BusinessLayer.GetPatientDocumentList(string.Format("MRN = {0} AND FileName LIKE '{1}\\%'", MRN, oldName));
                foreach (PatientDocument entity in lstEntity)
                {
                    string name = entity.FileName.Substring(oldName.Length);

                    entity.FileName = string.Format("{0}{1}", newName, name);
                    entity.FileName = entity.FileName.Replace("\\\\","\\");
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePatientDocument(entity);
                }
            }
            else
            {
                if (folderName == "BodyDiagram")
                {
                    string oldName = e.Item.Name;
                    MRN = Convert.ToInt32(Page.Request.QueryString["id"]);
                    List<vPatientBodyDiagramHd> lstEntity = BusinessLayer.GetvPatientBodyDiagramHdList(string.Format("MRN = {0} AND FileName = '{1}'", MRN, oldName));
                    foreach (vPatientBodyDiagramHd vEntity in lstEntity)
                    {
                        PatientBodyDiagramHd entity = BusinessLayer.GetPatientBodyDiagramHd(vEntity.ID);
                        entity.FileName = e.NewName;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePatientBodyDiagramHd(entity);
                    }
                }
                else
                {
                    string oldName = e.Item.RelativeName;
                    string[] param = e.Item.RelativeName.Split('\\');
                    string path = String.Format("{0}\\", String.Join("\\", param.Take(param.Length - 1)));

                    MRN = Convert.ToInt32(Page.Request.QueryString["id"]);
                    List<PatientDocument> lstEntity = BusinessLayer.GetPatientDocumentList(string.Format("MRN = {0} AND FileName = '{1}'", MRN, oldName));
                    foreach (PatientDocument entity in lstEntity)
                    {
                        entity.FileName = string.Format("{0}{1}", path, e.NewName);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePatientDocument(entity);
                    }
                }
            }
            ValidateSiteEdit(e);
        }

        protected void fileManager_ItemMoving(object sender, FileManagerItemMoveEventArgs e)
        {
            ValidateSiteEdit(e);
        }

        protected void fileManager_ItemDeleting(object sender, FileManagerItemDeleteEventArgs e)
        {
            if (hdnIsFolder.Value == "1")
            {
                string oldName = e.Item.RelativeName;
                string[] param = e.Item.RelativeName.Split('\\');
                string path = String.Format("{0}\\", String.Join("\\", param.Take(param.Length - 1)));

                MRN = Convert.ToInt32(Page.Request.QueryString["id"]);
                List<PatientDocument> lstEntity = BusinessLayer.GetPatientDocumentList(string.Format("MRN = {0} AND FileName LIKE '{1}%'", MRN, oldName));
                foreach (PatientDocument entity in lstEntity)
                {
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePatientDocument(entity);
                }
            }
            else
            {
                string oldName = e.Item.RelativeName;
                MRN = Convert.ToInt32(Page.Request.QueryString["id"]);
                List<PatientDocument> lstEntity = BusinessLayer.GetPatientDocumentList(string.Format("MRN = {0} AND FileName = '{1}'", MRN, oldName));
                foreach (PatientDocument entity in lstEntity)
                {
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePatientDocument(entity);
                }
            }
            ValidateSiteEdit(e);
        }

        protected void fileManager_FolderCreating(object sender, FileManagerFolderCreateEventArgs e)
        {
            ValidateSiteEdit(e);
        }

        void ValidateSiteEdit(FileManagerActionEventArgsBase e)
        {
        }

    }
}