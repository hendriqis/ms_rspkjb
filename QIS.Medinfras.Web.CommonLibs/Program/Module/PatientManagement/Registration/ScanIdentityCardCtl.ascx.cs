using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ScanIdentityCardCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", param)).FirstOrDefault();

            if (oVisit != null)
            {
                LoadPatientInformation(oVisit);   
            }
        }

        private void LoadPatientInformation(vConsultVisit oVisit)
        {
            hdnMRN.Value = oVisit.MRN.ToString();
            hdnMedicalNo.Value = oVisit.MedicalNo;
            hdnPatientName.Value = oVisit.PatientName;
            hdnDateOfBirth.Value = oVisit.DateOfBirthInString;
            hdnGender.Value = oVisit.Gender;
            hdnHomeAddress.Value = oVisit.HomeAddress;

            txtMRN1.Text = oVisit.MedicalNo;
            txtPatientName1.Text = oVisit.PatientName;
            txtBirthPlace1.Text = oVisit.CityOfBirth;
            txtDOB1.Text = oVisit.DateOfBirthInString;
            txtGender1.Text = oVisit.Gender;
            txtAddress1.Text = oVisit.HomeAddress;
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            result = SendPatientInformationToScanner();

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string SendPatientInformationToScanner()
        {
            string result = string.Empty;

            try
            {
                StringBuilder sbMessage = new StringBuilder();
                //string localIPAddress = HttpContext.Current.Request.UserHostAddress;
                string localIPAddress = Methods.GetLocalIPAddress();
                string message = string.Format("MD101|{0};{1};{2};{3};{4};{5}", AppSession.UserLogin.HealthcareID, hdnMRN.Value, hdnMedicalNo.Value, hdnPatientName.Value, hdnDateOfBirth.Value, hdnGender.Value, hdnHomeAddress.Value);
                TcpClient client = new TcpClient();
                client.Connect(localIPAddress, Convert.ToInt16(hdnPort.Value));
                NetworkStream stream = client.GetStream();
                using (BinaryWriter w = new BinaryWriter(stream))
                {
                    using (BinaryReader r = new BinaryReader(stream))
                    {
                        w.Write(string.Format(@"{0}", message).ToCharArray());
                    }
                }

                result = string.Format("process|1|{0}|{1}|{2}", "Identity Card was processed successfully", string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}|{1}|{2}", ex.Message, string.Empty, string.Empty);
            }

            return result;
        }
    }
}