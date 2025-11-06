using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using System.Web.UI.HtmlControls;
using System.IO;
using DevExpress.Web.ASPxFileManager;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class PatientFolderList : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PATIENT_FOLDER;
        }

        protected int MRN;
        protected override void InitializeDataControl()
        {
            MRN = AppSession.PatientDetail.MRN;
            hdnMRN.Value = MRN.ToString();
            string path = string.Format("{0}{1}", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISPatientImagePath.Replace('/', '\\').Replace("#MRN", AppSession.PatientDetail.MedicalNo));
            hdnPatientDocumentUrl.Value = string.Format("{0}/{1}", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientImagePath.Replace('/', '\\').Replace("#MRN", ""));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            fileManager.Settings.RootFolder = path;

            //fileManager.Settings.ThumbnailFolder = string.Format("{0}Patient\\{1}", AppSession.SettingParameter.FirstOrDefault(p => p.Code == Constant.SettingParameter.PHYSICAL_DIR).Value, entity.MedicalNo);
            fileManager.Settings.InitialFolder = "Document";
        }


        protected void fileManager_CustomThumbnail(object source, FileManagerThumbnailCreateEventArgs e)
        {
            string fileName = e.File.FullName.Replace(AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISVirtualDirectory);
            fileName = fileName.Replace('\\', '/');

            e.ThumbnailImage.Height = new Unit(100, UnitType.Pixel);
            e.ThumbnailImage.Width = new Unit(70, UnitType.Pixel);
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

                List<PatientDocument> lstEntity = BusinessLayer.GetPatientDocumentList(string.Format("MRN = {0} AND FileName LIKE '{1}\\%'", AppSession.PatientDetail.MRN, oldName));
                foreach (PatientDocument entity in lstEntity)
                {
                    string name = entity.FileName.Substring(oldName.Length);

                    entity.FileName = string.Format("{0}{1}", newName, name);
                    entity.FileName = entity.FileName.Replace("\\\\", "\\");
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePatientDocument(entity);
                }
            }
            else
            {
                if (folderName == "BodyDiagram")
                {
                    string oldName = e.Item.Name;
                    List<vPatientBodyDiagramHd> lstEntity = BusinessLayer.GetvPatientBodyDiagramHdList(string.Format("MRN = {0} AND FileName = '{1}'", AppSession.PatientDetail.MRN, oldName));
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

                    List<PatientDocument> lstEntity = BusinessLayer.GetPatientDocumentList(string.Format("MRN = {0} AND FileName = '{1}'", AppSession.PatientDetail.MRN, oldName));
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
                Patient oPatient = BusinessLayer.GetPatient(Convert.ToInt32(hdnMRN.Value));
              
                string path = AppConfigManager.QISPhysicalDirectory + AppConfigManager.QISPatientImagePath + e.Item.RelativeName;
                path = path.Replace("#MRN", oPatient.MedicalNo);
                path = path.Replace('/', '\\');
                if (Directory.Exists(path)) {
                    string[] FileData = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                    string fileName = string.Empty; 
                    if (FileData.Length > 0) {
                        for (int i = 0; i < FileData.Length; i++)
                        {
                            if(File.Exists(FileData[i]))
                            {
                                fileName += string.Format("'{0}',", Path.GetFileName(FileData[i]) );
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(fileName)) {
                        fileName = fileName.Remove(fileName.Length - 1);

                        bool isDeletedFile = true;
                        string filterexpresion = string.Format("MRN = {0} AND FileName IN ({1}) AND IsDeleted=0", AppSession.PatientDetail.MRN, fileName);
                        List<PatientDocument> lstEntity = BusinessLayer.GetPatientDocumentList(filterexpresion);
                        List<PatientDocument> lstResultBridgin = lstEntity.Where(p => p.GCDocumentType == Constant.DocumentType.DIAGNOSTIC_IMAGING && p.CreatedBy == 0).ToList();
                        if (lstResultBridgin.Count > 0)
                        {
                            isDeletedFile = false;
                            e.Cancel = true;
                            e.ErrorText = string.Format("Didalam folder {0} terdapat file yang merupakan hasil dari bridging yang tidak boleh dihapus", e.Item.RelativeName);
                        }
                        if (isDeletedFile)
                        {
                            foreach (PatientDocument entity in lstEntity)
                            {
                                entity.IsDeleted = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdatePatientDocument(entity);
                            }
                        }
                    }
                }
            }
            else
            {
                string[] param = e.Item.RelativeName.Split('\\');
                string oldName = param[1];
                List<PatientDocument> lstEntity = BusinessLayer.GetPatientDocumentList(string.Format("MRN = {0} AND FileName = '{1}' AND IsDeleted = 0", AppSession.PatientDetail.MRN, oldName));
                foreach (PatientDocument entity in lstEntity)
                {
                    bool isDeletedFile = true;

                    if (entity.CreatedBy == 0 && entity.GCDocumentType == Constant.DocumentType.DIAGNOSTIC_IMAGING){
                        isDeletedFile = false;
                        e.Cancel = true;
                        e.ErrorText = string.Format("File {0} merupakan hasil dari bridging yang tidak boleh dihapus", oldName); 
                    }
                    else
                    {
                        Patient oPatient = BusinessLayer.GetPatient(entity.MRN);
                        if (oPatient != null)
                        {
                            string path = AppConfigManager.QISPhysicalDirectory;
                            path += string.Format("{0}{1}", AppConfigManager.QISPatientDocumentsPath, entity.FileName);
                            path = path.Replace('/', '\\');
                            path = path.Replace("#MRN", oPatient.MedicalNo);
                            if (Directory.Exists(path))
                            {
                                if (File.Exists(path))
                                {
                                    File.Delete(path);
                                }
                            }
                        }
                    }

                    if (isDeletedFile) {
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePatientDocument(entity);
                    }
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

        private class FilesDocument { 
            public string FileName {get; set; }
        }
    }
}