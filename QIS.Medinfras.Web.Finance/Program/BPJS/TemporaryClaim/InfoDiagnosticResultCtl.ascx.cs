using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InfoDiagnosticResultCtl : BaseContentPopupCtl
    {
        protected int PageCount = 1;

        private TemporaryClaim DetailPage
        {
            get { return (TemporaryClaim)Page; }
        }

        public override void InitializeControl(string param)
        {
            hdnRegistrationID.Value = param;

            string filterExpression = string.Format("RegistrationID = '{0}'", hdnRegistrationID.Value);
            vConsultVisitCasemix entity = BusinessLayer.GetvConsultVisitCasemixList(filterExpression).FirstOrDefault();

            hdnRegistrationNo.Value = entity.RegistrationNo;
            txtRegistrationNo.Text = string.Format("{0}", entity.RegistrationNo);
            txtSEPNo.Text = string.Format("{0}", entity.NoSEP);
            txtPatient.Text = string.Format("({0}) {1}", entity.MedicalNo, entity.PatientName);

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                        AppSession.UserLogin.HealthcareID, //0
                                        Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, //1
                                        Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI //2
                                    ));

            hdnLaboratoryHealthcareServiceUnitID.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;
            hdnImagingHealthcareServiceUnitID.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            
            Healthcare oHc = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            if (oHc != null)
            {
                hdnInitialHealthcare.Value = oHc.Initial;
            }
            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            List<GetRegistrationDiagnosticResult> lstEntity = BusinessLayer.GetRegistrationDiagnosticResultList(Convert.ToInt32(hdnRegistrationID.Value));
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                GetRegistrationDiagnosticResult entity = e.Item.DataItem as GetRegistrationDiagnosticResult;

            }
        }
        #endregion

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = e.Parameter;
            string result = param + "|";
            string retval = "";

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] paramResult = e.Parameter.Split('|');
                if (paramResult[0] == "upload")
                {
                    UploadFile(paramResult[1]);
                }
                else // refresh
                {
                    BindGridView();
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }

        private void UploadFile(string transactionNo)
        {
            // belum jalan

            ////string imageData = hdnUploadedFile1.Value;
            ////if (imageData != "")
            ////{
            ////    string[] parts = Regex.Split(imageData, ",").Skip(1).ToArray();
            ////    imageData = String.Join(",", parts);
            ////}

            string path = AppConfigManager.QISPhysicalDirectory;
            path += string.Format("{0}\\EKlaim\\{1}", AppConfigManager.QISPatientDocumentsPath.Replace('/', '\\'), hdnRegistrationNo.Value);

            path = path.Replace("#RegistrationNo", hdnRegistrationNo.Value);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = String.Format("{0}_DiagnosticResult_{1}.pdf", hdnRegistrationNo.Value, transactionNo);
            FileStream fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            byte[] data = Convert.FromBase64String(fileName);
            bw.Write(data);
            bw.Close();
        }
    }
}