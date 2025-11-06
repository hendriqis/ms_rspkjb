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
    public partial class PatientFolderDetail2 : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PATIENT_FOLDER;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int MRN = Convert.ToInt32(Page.Request.QueryString["id"]);
                vPatient entity = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", MRN))[0];
                EntityToControl(entity);
                fileManager.Settings.RootFolder = string.Format("{0}Patient\\{1}", AppConfigManager.QISPhysicalDirectory, entity.MedicalNo);
                //fileManager.Settings.ThumbnailFolder = string.Format("{0}Patient\\{1}", AppSession.SettingParameter.FirstOrDefault(p => p.Code == Constant.SettingParameter.PHYSICAL_DIR).Value, entity.MedicalNo);
                fileManager.Settings.InitialFolder = entity.MedicalNo;
            }
        }

        private void EntityToControl(vPatient entity)
        {
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

        protected void fileManager_FileUploading(object sender, FileManagerFileUploadEventArgs e)
        {
            ValidateSiteEdit(e);
        }

        protected void fileManager_ItemRenaming(object sender, FileManagerItemRenameEventArgs e)
        {
            ValidateSiteEdit(e);
        }

        protected void fileManager_ItemMoving(object sender, FileManagerItemMoveEventArgs e)
        {
            ValidateSiteEdit(e);
        }

        protected void fileManager_ItemDeleting(object sender, FileManagerItemDeleteEventArgs e)
        {
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