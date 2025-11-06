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
    public partial class CoverageRegistrationInfoCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            //vConsultVisitForPatientBanner oVisit = BusinessLayer.GetvConsultVisitForPatientBannerList(string.Format("RegistrationNo = '{0}' AND IsMainVisit = 1", param.ToString())).First();
            //if (oVisit != null)
            //{
            //    txtMRN.Text = oVisit.MedicalNo;
            //    txtSEPNo.Text = oVisit.NoSEP;
            //    txtIdentityNo.Text = oVisit.SSN;
            //    txtPatientName.Text = oVisit.PatientName;
            //    txtPatientName2.Text = oVisit.Name2;
            //    txtPreferredName.Text = oVisit.PreferredName;
            //    txtGender.Text = oVisit.Gender;
            //    txtBloodType.Text = oVisit.BloodType;
            //    txtBloodRhesus.Text = oVisit.BloodRhesus;
            //    txtReligion.Text = oVisit.Religion;
            //    txtNationality.Text = oVisit.Nationality;
            //    txtBirthPlace.Text = oVisit.CityOfBirth;
            //    txtDOB.Text = oVisit.DateOfBirthInString;
            //    txtAgeInYear.Text = oVisit.AgeInYear.ToString();
            //    txtAgeInMonth.Text = oVisit.AgeInMonth.ToString();
            //    txtAgeInDay.Text = oVisit.AgeInDay.ToString();
            //}
        }
    }
}