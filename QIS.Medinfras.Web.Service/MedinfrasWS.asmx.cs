using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using MedinfrasAPI.Models;

namespace QIS.Medinfras.Web.Service
{
    /// <summary>
    /// Summary description for Patient
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class MedinfrasWS : System.Web.Services.WebService
    {

        [WebMethod]
        public object GetPatientByMedicalNo(string medicalNo)
        {
            string filterExp = string.Format("MedicalNo = '{0}'", medicalNo);
            vPatient oPatient = BusinessLayer.GetvPatientList(filterExp).FirstOrDefault();
            PatientData oData = null;
            if (oPatient != null)
            {
                oData = new PatientData();
                oData.MRN = oPatient.MRN;
                oData.MedicalNo = oPatient.MedicalNo;
                oData.FirstName = oPatient.FirstName;
                oData.MiddleName = oPatient.MiddleName;
                oData.LastName = oPatient.LastName;
                oData.PrefferedName = oPatient.PreferredName;
                oData.Gender = oPatient.Gender;
                oData.Religion = oPatient.Religion;
                oData.MaritalStatus = oPatient.MaritalStatus;
                oData.DateOfBirth = oPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
                oData.CityOfBirth = oPatient.CityOfBirth;
                oData.HomeAddress = oPatient.HomeAddress;
                oData.HomeZipCode = oPatient.ZipCode;
                oData.HomePhoneNo1 = oPatient.PhoneNo1;
                oData.HomePhoneNo2 = oPatient.PhoneNo2;
                oData.MobileNo1 = oPatient.MobilePhoneNo1;
                oData.MobileNo2 = oPatient.MobilePhoneNo2;
            }
            return oData;
        }

        [WebMethod]
        public object GetPatientList(string fromMedicalNo, string toMedicalNo)
        {
            string filterExp = string.Format("MedicalNo BETWEEN '{0}' AND '{1}'", fromMedicalNo, toMedicalNo);
            List<vPatient> oPatientList = BusinessLayer.GetvPatientList(filterExp);
            List<PatientData> oDataList = null;
            foreach (vPatient oPatient in oPatientList)
            {
                PatientData oData;
                oData = new PatientData();
                oData.MRN = oPatient.MRN;
                oData.MedicalNo = oPatient.MedicalNo;
                oData.FirstName = oPatient.FirstName;
                oData.MiddleName = oPatient.MiddleName;
                oData.LastName = oPatient.LastName;
                oData.PrefferedName = oPatient.PreferredName;
                oData.Gender = oPatient.Gender;
                oData.Religion = oPatient.Religion;
                oData.MaritalStatus = oPatient.MaritalStatus;
                oData.DateOfBirth = oPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
                oData.CityOfBirth = oPatient.CityOfBirth;
                oData.HomeAddress = oPatient.HomeAddress;
                oData.HomeZipCode = oPatient.ZipCode;
                oData.HomePhoneNo1 = oPatient.PhoneNo1;
                oData.HomePhoneNo2 = oPatient.PhoneNo2;
                oData.MobileNo1 = oPatient.MobilePhoneNo1;
                oData.MobileNo2 = oPatient.MobilePhoneNo2;

                oDataList.Add(oData);
            }
            return oDataList;
        }
    }
}
