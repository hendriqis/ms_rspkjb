using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MDTestResultDetailCtl : BaseEntryPopupCtl
    {
        protected string GCTemplateGroupIS = "";
        protected string GCTemplateGroupMD = "";

        private MDTestResultDetail DetailPage
        {
            get { return (MDTestResultDetail)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            GCTemplateGroupIS = Constant.TemplateGroup.IMAGING;
            GCTemplateGroupMD = Constant.TemplateGroup.DIAGNOSTIC;

            hdnMedicalNo.Value = AppSession.RegisteredPatient.MedicalNo;
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            string[] par = param.Split('|');
            hdnIsImagingResultCtlResultDt.Value = par[0];
            hdnItemIDCtlResultDt.Value = par[1];
            hdnIDCtlResultDt.Value = par[2];
            hdnParamedicIDCtlResultDt.Value = par[5];

            txtItemInfo.Text = string.Format("({0}) {1}", par[4], par[3]);

            List<StandardCode> lstBorderLine = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.DIAGNOSTIC_RESULT_INTERPRETATION));
            Methods.SetComboBoxField<StandardCode>(cboBorderLine, lstBorderLine, "StandardCodeName", "StandardCodeID");
            cboBorderLine.SelectedIndex = 0;

            ImagingResultDt entityDT = BusinessLayer.GetImagingResultDtList(string.Format("ID = {0} AND ItemID = {1}", hdnIDCtlResultDt.Value, hdnItemIDCtlResultDt.Value)).FirstOrDefault();
            if (entityDT != null)
            {
                EntityToControl(entityDT);
                IsAdd = false;
            }
            else
            {
                txtPhotoNumber.Text = "";
                txtFileName.Text = "";
            }
        }

        private void EntityToControl(ImagingResultDt entity)
        {
            string[] fileInfo = entity.FileName.Split('.');

            cboBorderLine.Text = entity.GCImagingTestBorderline;
            txtPhotoNumber.Text = entity.PhotoNumber;
            txtTestResult1.Text = entity.TestResult1;
            txtTestResult2.Text = entity.TestResult2;
            txtFileName.Text = entity.FileName;

            if (entity.FileName != "")
            {
                txtUploadFile.Value = fileInfo[0];
                txtExtention.Value = fileInfo[1];
            }

            string patientImagingPath = string.Format("Patient/{0}/Document/Imaging/", hdnMedicalNo.Value);
            imgPreview.Src = string.Format("{0}{1}{2}", AppConfigManager.QISVirtualDirectory, patientImagingPath, entity.FileName);
        }

        private void ControlToEntity(ImagingResultDt entity)
        {
            entity.ItemID = Convert.ToInt32(hdnItemIDCtlResultDt.Value);
            entity.GCImagingTestBorderline = Convert.ToString(cboBorderLine.Value);
            entity.PhotoNumber = txtPhotoNumber.Text;
            entity.TestResult1 = Helper.GetHTMLEditorText(txtTestResult1);
            entity.TestResult2 = Helper.GetHTMLEditorText(txtTestResult2);
            entity.ParamedicID = Convert.ToInt32(hdnParamedicIDCtlResultDt.Value);
        }

        private void UploadPhoto(int ImagingHdID, ref string fileName)
        {
            if (hdnUploadedFile1.Value != "")
            {
                string imageData = hdnUploadedFile1.Value;
                string fileType = imageData.Split(';')[0].Split('/')[1];

                fileName = String.Format("{0}_{1}_{2}.{3}", hdnVisitID.Value, ImagingHdID, hdnItemIDCtlResultDt.Value, txtExtention.Value);
                string fileNameExt = fileName.Split('.').LastOrDefault().ToLower();

                if (fileType.ToLower() != "msword" && fileType.ToLower() != "plain" && fileType.ToLower() != "jpg" && fileType.ToLower() != "jpeg" && fileType.ToLower() != "png" && fileType.ToLower() != "txt" && fileType.ToLower() != "doc" && fileType.ToLower() != "docx" && fileType.ToLower() != "pdf" && fileType.ToLower() != "mp4" && fileType.ToLower() != "dcm")
                {
                    //errMessage = "Ekstension yang digunakan tidak sesuai dengan yang diizinkan.";
                    //return false;
                }
                else if (fileNameExt != "jpg" && fileNameExt != "jpeg" && fileNameExt != "png" && fileNameExt != "txt" && fileNameExt != "doc" && fileNameExt != "docx" && fileNameExt != "pdf" && fileNameExt != "mp4" && fileNameExt != "dcm")
                {
                    //errMessage = "Ekstension yang digunakan tidak sesuai dengan yang diizinkan.";
                    //return false;
                }
                else
                {
                    if (imageData != "")
                    {
                        string[] parts = Regex.Split(imageData, ",").Skip(1).ToArray();
                        imageData = String.Join(",", parts);
                    }

                    string path = AppConfigManager.QISPhysicalDirectory;
                    path += string.Format("{0}\\Imaging\\", AppConfigManager.QISPatientDocumentsPath.Replace('/', '\\'));

                    path = path.Replace("#MRN", hdnMedicalNo.Value);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    fileName = String.Format("{0}_{1}_{2}.{3}", hdnVisitID.Value, ImagingHdID, hdnItemIDCtlResultDt.Value, txtExtention.Value);
                    FileStream fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);

                    byte[] data = Convert.FromBase64String(imageData);
                    bw.Write(data);
                    bw.Close();
                }
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ImagingResultDtDao entityDtDao = new ImagingResultDtDao(ctx);
            int ImagingHdID = Convert.ToInt32(hdnIDCtlResultDt.Value);
            try
            {
                DetailPage.SaveImagingResultHd(ctx, ref ImagingHdID);

                string fileName = "";
                UploadPhoto(ImagingHdID, ref fileName);

                ImagingResultDt entity = new ImagingResultDt();
                ControlToEntity(entity);
                entity.FileName = fileName;
                entity.ID = ImagingHdID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Insert(entity);
                retval = ImagingHdID.ToString();

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ImagingResultDtDao entityDtDao = new ImagingResultDtDao(ctx);
            int ImagingHdID = Convert.ToInt32(hdnIDCtlResultDt.Value);
            try
            {
                string fileName = "";
                UploadPhoto(Convert.ToInt32(hdnIDCtlResultDt.Value), ref fileName);

                ImagingResultDt entity = entityDtDao.Get(Convert.ToInt32(hdnIDCtlResultDt.Value), Convert.ToInt32(hdnItemIDCtlResultDt.Value));
                ControlToEntity(entity);

                if (hdnUploadedFile1.Value != "")
                    entity.FileName = fileName;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}