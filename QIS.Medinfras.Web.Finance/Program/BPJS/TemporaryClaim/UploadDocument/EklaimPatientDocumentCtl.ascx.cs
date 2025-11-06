using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using Newtonsoft.Json;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program.PatientPage
{
    public partial class EklaimPatientDocumentCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            SetControlProperties();
            if (param.ToString().Split('|')[1] != "")
            {
              
                hdnRegistraionID.Value = param.ToString().Split('|')[0];
                hdnID.Value = param.ToString().Split('|')[1];
                SetControlProperties();
                vPatientDocument entity = BusinessLayer.GetvPatientDocumentList(string.Format("ID='{0}' AND IsDeleted=0", Convert.ToInt32(hdnID.Value))).FirstOrDefault();
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                 
            }
        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}', '{1}')", Constant.StandardCode.DOCUMENT_TYPE, Constant.StandardCode.E_DOCUMENT_EKLAIM);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCDocumentType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOCUMENT_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            List<StandardCode> lstEkalimDocument = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.E_DOCUMENT_EKLAIM).ToList();
            Methods.SetComboBoxField<StandardCode>(cboFileClass, lstEkalimDocument, "StandardCodeName", "StandardCodeID");

        }

        

        private void EntityToControl(vPatientDocument entity)
        {
            txtDocumentDate.Text = entity.DocumentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            cboGCDocumentType.Value = entity.GCDocumentType.ToString();
            txtFileName.Text = entity.FileName;
            hdnMedicalNo.Value = entity.MedicalNo;
            vRegistrationBPJS1 oRegBpjs = BusinessLayer.GetvRegistrationBPJS1List(string.Format("RegistrationID='{0}' ", hdnRegistraionID.Value)).FirstOrDefault();
            if (oRegBpjs != null) 
            {
                txtSep.Text = oRegBpjs.NoSEP;
                txtFullname.Text = oRegBpjs.PatientName;
                txtNoPeserta.Text = oRegBpjs.NoPeserta;
            }
        }

        private void ControlToEntity(PatientDocument entity)
        {
            entity.MRN = AppSession.RegisteredPatient.MRN;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.GCDocumentType = cboGCDocumentType.Value.ToString();
            entity.DocumentDate = Helper.GetDatePickerValue(txtDocumentDate);
            entity.FileName = txtFileName.Text; 
        }


        private bool OnUploadEklaim(ref string errMessage, ref string respEklaim)
        {
            bool result = true;
            try
            {
                string path = AppConfigManager.QISPhysicalDirectory;
                path += string.Format("{0}\\{1}\\", AppConfigManager.QISPatientDocumentsPath.Replace('/', '\\'), hdnParam.Value);
                path = path.Replace("#MRN", hdnMedicalNo.Value);
                string pathFile = String.Format("{0}\\{1}", path, txtFileName.Text);
                if (File.Exists(pathFile))
                {
                    StandardCode oStandardCode = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID = '{0}'", cboFileClass.Value.ToString())).FirstOrDefault();
                    string file_class = oStandardCode.StandardCodeName; 
                    string filedata = GetBase64StringForImage(pathFile);
                    UploadFileMethod data = new UploadFileMethod()
                    {
                        metadata = new UploadFileMetadata()
                        {
                            method = "file_upload",
                            nomor_sep = txtSep.Text,
                            file_name = txtFileName.Text,
                            file_class = file_class
                        },
                        data = filedata
                    };

                    string jsonRequest = JsonConvert.SerializeObject(data);
                    EKlaimService eklaimService = new EKlaimService();
                    string response = eklaimService.UploadFile(jsonRequest);
                    respEklaim = response.ToString();
                    UploadFileResponse respInfo = JsonConvert.DeserializeObject<UploadFileResponse>(response);
                    if (respInfo.metadata.code == "200")
                    {
                        PatientDocument oDoc = BusinessLayer.GetPatientDocumentList(string.Format("ID='{0}'", hdnID.Value)).FirstOrDefault();
                        if (oDoc != null)
                        {
                            oDoc.EKlaimDocumentID = 0;
                            oDoc.EKlaimDocumentID = AppSession.UserLogin.UserID;
                            oDoc.EKlaimDocumentSentDate = DateTime.Now;
                            oDoc.GCEKlaimDocumentClass = file_class;
                            BusinessLayer.UpdatePatientDocument(oDoc);
                        
                        }
                    }


                    return result;
                }
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }
        protected static string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }  
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string respEklaim = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "uploadFileEklaim")
            {
                if (OnUploadEklaim(ref errMessage, ref respEklaim))
                    result += string.Format("success|{0}", respEklaim);
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

    }
}