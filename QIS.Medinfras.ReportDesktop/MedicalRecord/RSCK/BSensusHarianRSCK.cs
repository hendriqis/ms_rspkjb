using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSensusHarianRSCK : BaseCustomDailyPotraitRpt
    {
        public BSensusHarianRSCK()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split('|');
            string logDateString = param[0];
            string serviceUnit = param[1];
            string logdate = param[2];

            //string[] param1 = // param[0].Split('|');

            string filterPatientAdmission = string.Format("{0} AND GCATDStatus = '{1}' AND HealthcareServiceUnitID = {2}", logDateString, Constant.ATD_STATUS.ADMISSION, serviceUnit);
            string filterPatientIN = string.Format("{0} AND GCATDStatus = '{1}' AND HealthcareServiceUnitID = {2}", logDateString, Constant.ATD_STATUS.TRANSFER_IN, serviceUnit);
            string filterPatientOUT = string.Format("{0} AND GCATDStatus = '{1}' AND HealthcareServiceUnitID = {2}", logDateString, Constant.ATD_STATUS.TRANSFER_OUT, serviceUnit);
            string filterPatientDISCHARGED = string.Format("{0} AND GCATDStatus IN ('{1}', '{2}', '{3}', '{4}') AND HealthcareServiceUnitID = {5}", logDateString, Constant.ATD_STATUS.DISCHARGED_WITH_AGREEMENT,
                                        Constant.ATD_STATUS.DISCHARGED_TO_OTHER_FACILITY, Constant.ATD_STATUS.DISCHARGED_TO_ANOTHER_HOSPITAL, Constant.ATD_STATUS.DISCHARGED_FORCED_OUT, serviceUnit);
            string filterPatientDISCHARGEDDIED = string.Format("{0}  AND GCATDStatus IN ('{1}', '{2}') AND HealthcareServiceUnitID = {3}", logDateString, Constant.ATD_STATUS.DIED_BEFORE_48_HR,
                                        Constant.ATD_STATUS.DIED_AFTER_48_HR, serviceUnit);

            List<vPatientATDLogRSCK> lstPatientAdmission = BusinessLayer.GetvPatientATDLogRSCKList(filterPatientAdmission);
            List<vPatientATDLogRSCK> lstPatientIN = BusinessLayer.GetvPatientATDLogRSCKList(filterPatientIN);
            List<vPatientATDLogRSCK> lstPatientOUT = BusinessLayer.GetvPatientATDLogRSCKList(filterPatientOUT);
            List<vPatientATDLogRSCK> lstPatientDISCHARGED = BusinessLayer.GetvPatientATDLogRSCKList(filterPatientDISCHARGED);
            List<vPatientATDLogRSCK> lstPatientDISCHARGEDDIED = BusinessLayer.GetvPatientATDLogRSCKList(filterPatientDISCHARGEDDIED);
            List<GetCencusInformationPasienSisa> lstPatientALL = BusinessLayer.GetCencusInformationPasienSisaList(Convert.ToDateTime(logdate), Convert.ToInt32(serviceUnit));

            #region PatientAdmission
            subPasienSensusMasuk.CanGrow = true;
            pasienSensusMasuk1.InitializeReport(lstPatientAdmission);
            #endregion

            #region PatientIN
            subPasienPindahMasuk.CanGrow = true;
            pasienSensusPindahMasuk1.InitializeReport(lstPatientIN);
            #endregion

            #region PatientOUT
            subPasienPindahMasuk.CanGrow = true;
            pasienSensusPindahKeluar1.InitializeReport(lstPatientOUT);
            #endregion

            #region PatientCencusDischarge
            subPasienSensusKeluar.CanGrow = true;
            pasienSensusKeluar1.InitializeReport(lstPatientDISCHARGED);
            #endregion

            #region PatientCencusDischargeDied
            subPasienSensusKeluarMeninggal.CanGrow = true;
            pasienSensusKeluarMeninggal1.InitializeReport(lstPatientDISCHARGEDDIED);
            #endregion

            #region PatientAll
            subPasienAll.CanGrow = true;
            pasienSensusAll1.InitializeReport(lstPatientALL);
            #endregion

            DateTime Tanggal = Convert.ToDateTime(logdate);
            string filterLabel = string.Format("HealthcareServiceUnitID = {0}", serviceUnit);
            vHealthcareServiceUnit EntityServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterLabel).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            if (Convert.ToInt32(serviceUnit) != 0)
            {
                lblServiceUnit.Text = string.Format("Ruang Perawatan : {0}", EntityServiceUnit.ServiceUnitName);
            }
            else
            {
                lblServiceUnit.Text = "Ruang Perawatan : SEMUA";
            }
            lblTanggal.Text = string.Format("Tanggal : {0}", Tanggal.ToString(Constant.FormatString.DATE_FORMAT));
            lblPrintDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblUserName.Text = string.Format("({0})", AppSession.UserLogin.UserFullName);

            base.InitializeReport(param);
        }
    }
}
