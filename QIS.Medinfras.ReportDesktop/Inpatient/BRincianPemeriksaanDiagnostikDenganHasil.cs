using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop 
{

    public partial class BRincianPemeriksaanDiagnostikDenganHasil : BaseCustomDailyPotraitRpt
    {
        public BRincianPemeriksaanDiagnostikDenganHasil()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            string[] temp = param[0].Split('|');
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", temp[0])).FirstOrDefault();

            string[] tempDate = param[1].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(tempDate[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(tempDate[1]).ToString(Constant.FormatString.DATE_FORMAT));

            cRegistrationNo.Text = string.Format("{0} | {1}", entityReg.RegistrationNo, entityReg.MedicalNo);
            cTanggalMasuk.Text = entityReg.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT);
            cNamaPasien.Text = entityReg.PatientName;
            cTanggalLahir.Text = entityReg.DateOfBirthInString;
            cDokterUtama.Text = entityReg.ParamedicName;
            cRuangPerawatan.Text = entityReg.ServiceUnitName;
            cKelas.Text = string.Format("{0} | {1}", entityReg.ClassName, entityReg.BedCode);
            cPenjaminBayar.Text = entityReg.BusinessPartnerName;

            if (entityReg.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) == "01-Jan-1900")
            {
                cTanggalKeluar.Text = "";
            }
            else
            {
                cTanggalKeluar.Text = entityReg.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            }

            base.InitializeReport(param);
        }

    }
}
