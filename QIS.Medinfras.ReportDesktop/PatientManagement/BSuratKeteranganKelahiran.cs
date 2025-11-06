using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratKeteranganKelahiran : BaseRpt
    {
        public BSuratKeteranganKelahiran()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPatientBirthRecordFull entity = BusinessLayer.GetvPatientBirthRecordFullList(param[0])[0];
            ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID = '{0}'", entity.VisitID))[0];
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            PatientBirthRecord entityBirthRecord = BusinessLayer.GetPatientBirthRecord(entity.BirthRecordID);
            vConsultVisit1 entityVisitMom = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}",entityBirthRecord.MotherVisitID))[0];

            #region Header

            if (oHealthcare != null)
            {
                lblHealthcareName.Text = oHealthcare.HealthcareName;
            }

            #endregion

            #region Detail
            cellNoSKL.Text = String.Format("No SKL : {0}", entity.ReferenceNo);
            cellNama.Text = entity.BabyFullName;
            cellGender.Text = entity.Gender;
            if (entity.GCBornAt == Constant.BornAt.INHOSPITAL)
            {
                cellBornAt.Multiline = true;
                cellBornAt.Text = oHealthcare.HealthcareName + "\r\n" + oHealthcare.AddressLine1;
            }
            else
            {
                cellBornAt.Text = entity.BornAt;
            }
            cellDOB.Text = entity.DateOfBirthInString;
            cellTOB.Text = entity.TimeOfBirth;
            cellPanjang.Text = entity.Length.ToString();
            cellBerat.Text = entity.Weight.ToString();
            cellLingkarKepala.Text = entity.HeadCircumference.ToString();
            cellLingkarDada.Text = entity.ChestCircumference.ToString();
            cellNamaIbu.Text = entity.MotherFullName;
            cellUsiaIbu.Text = entity.MotherAgeInYear.ToString();
            cellAlamatIbu.Text = entity.MotherStreetName;
            cellTelpIbu.Text = entity.MotherPhoneNo1;
            cellNamaAyah.Text = entity.FatherFullName;
            cellUsiaAyah.Text = entity.FatherAgeInYear.ToString();
            cellAlamatAyah.Text = entity.FatherStreetName;
            cellTelpAyah.Text = entity.FatherPhoneNo1;

            #endregion

            #region Footer

            if (oHealthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                xrLogo.WidthF = 180;
                xrLogo.HeightF = 180;
                cHealthcareName.Text = oHealthcare.HealthcareName;
                cHealthcareAddress.Text = oHealthcare.StreetName;
                cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
                cHealthcarePhone.Text = oHealthcare.PhoneNo1;
                cHealthcareFax.Text = oHealthcare.FaxNo1;
            }

            if (entity.LastUpdatedDate != null && entity.LastUpdatedDateInString != "01-Jan-1900")
            {
                lblTglTTD.Text = string.Format("{0}, {1}", oHealthcare.City, entity.LastUpdatedDateInString);
                //lblTTD.Text = entity.LastUpdatedByUser;
                lblTTD.Text = entityVisitMom.ParamedicName;
            }
            else
            {
                lblTglTTD.Text = string.Format("{0}, {1}", oHealthcare.City, entity.CreatedDateInString);
                //lblTTD.Text = entity.CreatedByUser;
                lblTTD.Text = entityVisitMom.ParamedicName;
            }

            lblReportCode.Text = reportMaster.ReportCode;

            #endregion

            base.InitializeReport(param);
        }

    }
}
