﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Net.Sockets;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientPhotoCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            vConsultVisitForPatientBanner oVisit = BusinessLayer.GetvConsultVisitForPatientBannerList(string.Format("RegistrationNo = '{0}' AND IsMainVisit = 1", param.ToString())).First();

            if (oVisit != null)
            {
                vConsultVisit14 oCV = BusinessLayer.GetvConsultVisit14List(string.Format("RegistrationNo = '{0}'", param.ToString())).FirstOrDefault();
                hdnVisitID.Value = oCV.VisitID.ToString();
                hdnParamedicName.Value = oCV.ParamedicName;
                hdnRegistrationDate.Value = oCV.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT_3);

                hdnRegistrationNo.Value = oVisit.RegistrationNo;
                hdnGuestNo.Value = oVisit.GuestNo;
                txtMRN.Text = oVisit.MedicalNo;
                txtSEPNo.Text = oVisit.NoSEP;
                txtIdentityNo.Text = oVisit.SSN;
                txtPatientName.Text = oVisit.PatientName;
                txtPatientName2.Text = oVisit.Name2;
                txtPreferredName.Text = oVisit.PreferredName;
                txtGender.Text = oVisit.Gender;
                txtBloodType.Text = oVisit.BloodType;
                txtBloodRhesus.Text = oVisit.BloodRhesus;
                txtReligion.Text = oVisit.Religion;
                txtNationality.Text = oVisit.Nationality;
                txtBirthPlace.Text = oVisit.CityOfBirth;
                txtDOB.Text = oVisit.DateOfBirthInString;
                txtAgeInYear.Text = oVisit.AgeInYear.ToString();
                txtAgeInMonth.Text = oVisit.AgeInMonth.ToString();
                txtAgeInDay.Text = oVisit.AgeInDay.ToString();

                txtAddress.Text = oVisit.HomeAddress;
                txtPhoneNo.Text = string.Format("{0}{1}{2}", oVisit.PhoneNo1, !String.IsNullOrEmpty(oVisit.PhoneNo2) ? "," : "", oVisit.PhoneNo2);
                txtMobilePhoneNo.Text = string.Format("{0}{1}{2}", oVisit.MobilePhoneNo1, !String.IsNullOrEmpty(oVisit.MobilePhoneNo2) ? "," : "", oVisit.MobilePhoneNo2);
                txtEmail.Text = oVisit.EmailAddress;

                txtSpouseName.Text = oVisit.SpouseName;
                txtMotherName.Text = oVisit.MotherName;
                txtFatherName.Text = oVisit.FatherName;

                txtWork.Text = oVisit.Occupation;
                txtPatientCategory.Text = oVisit.PatientCategory;

                txtLastAcuteInitialAssessmentDate.Text = oVisit.cfLastAcuteInitialAssessmentDate;
                txtLastChronicInitialAssessmentDate.Text = oVisit.cfLastChronicInitialAssessmentDate;

                txtJenisPeserta.Text = oVisit.JenisPeserta;
                txtNoPeserta.Text = oVisit.NoPeserta;

                #region Patient Image
                string logoPath = string.Format("Patient/{0}/", oVisit.MedicalNo);
                imgPreview.Src = string.Format("{0}{1}{2}?{3}", AppConfigManager.QISVirtualDirectory, logoPath, oVisit.PictureFileName, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_2));


                //if (File.Exists(imgPreview.Src) == false)
                //if (!File.Exists(Server.MapPath(imgPreview.Src)))
                if (!File.Exists(string.Format("{0}{1}{2}", AppConfigManager.QISPhysicalDirectory, logoPath, oVisit.PictureFileName)))
                {
                    if (oVisit.GCGender == Constant.Gender.MALE)
                    {
                        imgPreview.Src = string.Format("{0}Patient/patient_male.png", AppConfigManager.QISVirtualDirectory);
                    }
                    else if (oVisit.GCGender == Constant.Gender.FEMALE)
                    {
                        imgPreview.Src = string.Format("{0}Patient/patient_female.png", AppConfigManager.QISVirtualDirectory);
                    }
                }
                #endregion

            }
        }

        protected void cbpPhotoPatient_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                if (e.Parameter == "photo")
                {
                    SendInfoDesktopTools(ref errMessage);
                    result = errMessage;
                }

            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void SendInfoDesktopTools(ref string errMessage)
        {
            // transationType;TransactionCode|HealthcareID;RegistrationNo;|IsPatient;MedicalNo;PatientName;DOB;|RegistrationDate;VisitID;PatientName;Dokter|username
            //contoh  :  MD401;01|001;OPR/202208170001;|0;009-009-00;Daniel aditya andaru putra;1989-08-19| 2022-08-17 18:09;12121;daniel aditya; dr.sp.spog|sysadmin
            string medicalno = Request.Form[txtMRN.UniqueID];
            string PatientName = Request.Form[txtPatientName.UniqueID];
            string DOB = Request.Form[txtDOB.UniqueID];
            int IsPatient = 1;
            if (string.IsNullOrEmpty(medicalno))
            {
                medicalno = hdnGuestNo.Value;
                IsPatient = 0;
            }

            string ServiceCommand = string.Format("MD401;01|{0};{1};|{2};{3};{4};{5}| {6};{7};{8};{9}|{10};",
                   AppSession.UserLogin.HealthcareID, //0
                   hdnRegistrationNo.Value, //1
                   IsPatient, //2
                   medicalno, //3
                   PatientName, //4
                   DOB, //5
                   hdnRegistrationDate.Value, //6 
                   hdnVisitID.Value, //7
                   txtPatientName.Text, //8
                   hdnParamedicName.Value, //9
                   AppSession.UserLogin.UserName
                );


            try
            {
                string IP = GetClientIp();
                string Port = "6000";
                string filterexp = string.Format("ParameterCode='{0}'", Constant.SettingParameter.SA0219);
                SettingParameterDt osetpar = BusinessLayer.GetSettingParameterDtList(filterexp).FirstOrDefault();
                if (osetpar != null)
                {
                    Port = osetpar.ParameterValue;
                }
                TcpClient client = new TcpClient();
                client.Connect(IP, Convert.ToInt16(Port.ToString()));
                // Retrieve the network stream. 
                NetworkStream stream = client.GetStream();
                // Create a BinaryWriter for writing to the stream. 
                using (BinaryWriter w = new BinaryWriter(stream, Encoding.UTF8))
                { // Create a BinaryReader for reading from the stream. 
                    using (BinaryReader r = new BinaryReader(stream))
                    { // Start a dialogue. 
                        w.Write(string.Format("{0}", ServiceCommand).ToCharArray());
                    }
                }
                errMessage = string.Format("photo|1|Ok");
            }
            catch (Exception er)
            {
                errMessage = string.Format("photo|0|{0}", er.Message);
            }
        }

        private string GetClientIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
    }
}