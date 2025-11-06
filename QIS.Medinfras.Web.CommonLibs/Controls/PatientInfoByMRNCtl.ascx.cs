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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientInfoByMRNCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            vPatient oPatient = BusinessLayer.GetvPatientList(string.Format("MRN = '{0}'", param.ToString())).First();
            if (oPatient != null)
            {
                txtMRN.Text = oPatient.MedicalNo;
                txtIdentityNoType.Text = oPatient.IdentityNoType;
                txtIdentityCardNo.Text = oPatient.SSN;
                txtPatientName.Text = oPatient.PatientName;
                txtPatientName2.Text = oPatient.name2;
                txtPreferredName.Text = oPatient.PreferredName;
                txtGender.Text = oPatient.Gender;
                txtBloodType.Text = oPatient.BloodType;
                txtBloodRhesus.Text = oPatient.BloodRhesus;
                txtReligion.Text = oPatient.Religion;
                txtNationality.Text = oPatient.Nationality;
                txtBirthPlace.Text = oPatient.CityOfBirth;
                txtDOB.Text = oPatient.DateOfBirthInString;
                txtAgeInYear.Text = oPatient.AgeInYear.ToString();
                txtAgeInMonth.Text = oPatient.AgeInMonth.ToString();
                txtAgeInDay.Text = oPatient.AgeInDay.ToString();

                txtAddress.Text = oPatient.HomeAddress;
                txtPhoneNo.Text = string.Format("{0}{1}{2}", oPatient.PhoneNo1, !String.IsNullOrEmpty(oPatient.PhoneNo2) ? "," : "", oPatient.PhoneNo2);
                txtMobilePhoneNo.Text = string.Format("{0}{1}{2}", oPatient.MobilePhoneNo1, !String.IsNullOrEmpty(oPatient.MobilePhoneNo2) ? "," : "", oPatient.MobilePhoneNo2);
                txtEmail.Text = oPatient.EmailAddress;
                
                txtSpouseName.Text = oPatient.SpouseName;
                txtMotherName.Text = oPatient.MotherName;
                txtFatherName.Text = oPatient.FatherName;

                txtWork.Text = oPatient.Occupation;
                txtPatientCategory.Text = oPatient.PatientCategory;

                txtLastAcuteInitialAssessmentDate.Text = oPatient.cfLastAcuteInitialAssessmentDate;
                txtLastChronicInitialAssessmentDate.Text = oPatient.cfLastChronicInitialAssessmentDate;
            }
        }
    }
}