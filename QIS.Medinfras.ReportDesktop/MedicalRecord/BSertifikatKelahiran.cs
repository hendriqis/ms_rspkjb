using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSertifikatKelahiran : BaseRpt
    {
        public BSertifikatKelahiran()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[]param)
        {
            vPatientBirthRecordFull entity = BusinessLayer.GetvPatientBirthRecordFullList(param[0])[0];
            ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID = '{0}'", entity.VisitID))[0];
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            HealthcareServiceUnit entityHUS = BusinessLayer.GetHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = '{0}'", entityVisit.HealthcareServiceUnitID))[0];
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", entityHUS.HealthcareID))[0];
            vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = '{0}'", entityVisit.VisitID))[0];

            #region Detail
            cellNoSKL.Text = entity.ReferenceNo;
            cellGender.Text = entity.Gender;
            cellNamaAyah.Text = entity.FatherFullName;
            cellNamaIbu.Text = entity.MotherFullName;
            cellPanjang.Text = entity.Length.ToString();
            cellBerat.Text = entity.Weight.ToString();
            //cellDOB.Text = entity.DateOfBirthInString;
            cellTOB.Text = entity.TimeOfBirth;
            cellAlamat.Text = entity.MotherStreetName;
            cellAnakke.Text = entity.ChildNo.ToString();
            cellNegara.Text = entity.Negara;
            lblRumahSakit.Text = String.Format("dilahirkan di {0} pada :", oHealthcare.HealthcareName);

            if (entity.ParamedicID1 == 0 || entity.ParamedicID1.ToString() == "0")
            {
                cellDokter.Text = entity.ParamedicName2;
            }
            else
            {
                cellDokter.Text = entity.ParamedicName1;
            }
            cellBidan.Text = entity.ParamedicName3;
              
            lblTTD.Text = oVisit.ParamedicName;
            cellDOB.Text = String.Format("{0}, {1}", entity.cfHari, entity.DateOfBirthInString);

            #endregion

            #region Footer
            if (entity.LastUpdatedDate != null && entity.LastUpdatedDateInString != "01-Jan-1900")
            {
                lblTglTTD.Text = string.Format("{0}, {1}", entityHealthcare.City, entity.LastUpdatedDateInString);
            }
            else
            {
                lblTglTTD.Text = string.Format("{0}, {1}", entityHealthcare.City, entity.CreatedDateInString);
            }
            #endregion
            base.InitializeReport(param);
        }
    }
}

