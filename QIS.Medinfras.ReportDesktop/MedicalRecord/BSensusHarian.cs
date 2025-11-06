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
    public partial class BSensusHarian : BaseCustomDailyLandscapeRpt
    {
        public BSensusHarian()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split('|');
            string LogDateString = param[0];
            string ServiceUnit = param[1];
            string Logdate = param[2];
            string filterPatientOUT = string.Format("{0} AND GCATDStatus IN ('{1}', '{2}', '{3}', '{4}') AND HealthcareServiceUnitID = {5}", LogDateString, Constant.ATD_STATUS.DISCHARGED_WITH_AGREEMENT, 
                Constant.ATD_STATUS.DISCHARGED_TO_OTHER_FACILITY,Constant.ATD_STATUS.DISCHARGED_TO_ANOTHER_HOSPITAL, Constant.ATD_STATUS.DISCHARGED_FORCED_OUT, ServiceUnit);
            string filterPatientIN = string.Format("{0} AND GCATDStatus = '{1}' AND HealthcareServiceUnitID = {2}", LogDateString, Constant.ATD_STATUS.ADMISSION, ServiceUnit);
            string filterPatientMoveIN = string.Format("{0} AND GCATDStatus = '{1}' AND HealthcareServiceUnitID = {2}", LogDateString, Constant.ATD_STATUS.TRANSFER_IN, ServiceUnit);
            string filterPatientMoveOUT = string.Format("{0} AND GCATDStatus = '{1}' AND HealthcareServiceUnitID = {2}", LogDateString, Constant.ATD_STATUS.TRANSFER_OUT, ServiceUnit);
            string filterPatientDeadAfter48 = string.Format("{0} AND GCATDStatus = '{1}' AND HealthcareServiceUnitID = {2}", LogDateString, Constant.ATD_STATUS.DIED_AFTER_48_HR, ServiceUnit);
            string filterPatientDeadBefore48 = string.Format("{0} AND GCATDStatus = '{1}' AND HealthcareServiceUnitID = {2}", LogDateString, Constant.ATD_STATUS.DIED_BEFORE_48_HR, ServiceUnit);
           
            List<vPatientATDLog> lstPatientOUT = BusinessLayer.GetvPatientATDLogList(filterPatientOUT);
            List<vPatientATDLog> lstPatientIN = BusinessLayer.GetvPatientATDLogList(filterPatientIN);
            List<vPatientATDLog> lstPatientMoveIN = BusinessLayer.GetvPatientATDLogList(filterPatientMoveIN);
            List<vPatientATDLog> lstPatientMoveOUT = BusinessLayer.GetvPatientATDLogList(filterPatientMoveOUT);
            List<vPatientATDLog> lstPatientDeadAfter48 = BusinessLayer.GetvPatientATDLogList(filterPatientDeadAfter48);
            List<vPatientATDLog> lstPatientDeadBefore48 = BusinessLayer.GetvPatientATDLogList(filterPatientDeadBefore48);
            List<PatientCencusInformation> lstPatientCensusClass = BusinessLayer.GetCencusInformationList(Convert.ToDateTime(Logdate), Convert.ToInt32(ServiceUnit));

            #region PatientCencusIn
            subPasienSensusMasuk.CanGrow = true;
            pasienSensusMasuk1.InitializeReport(lstPatientIN);
            #endregion

            #region PatientCencusOut
            subPasienSensusKeluar.CanGrow = true;
            pasienSensusKeluar1.InitializeReport(lstPatientOUT);
            #endregion

            #region PatientMoveIN
            subPasienPindahMasuk.CanGrow = true;
            pasienSensusPindahMasuk1.InitializeReport(lstPatientMoveIN);
            #endregion

            #region PatientMoveOUT
            subPasienPindahMasuk.CanGrow = true;
            pasienSensusPindahKeluar1.InitializeReport(lstPatientMoveOUT);
            #endregion

            #region PatientDeadAfter48
            subPatientDeadAfter48.CanGrow = true;
            pasienSensusMeninggalSetelah481.InitializeReport(lstPatientDeadAfter48);
            #endregion

            #region PatientDeadBefore48
            subPatientDeadBefore48.CanGrow = true;
            pasienSensusMeninggalSebelum481.InitializeReport(lstPatientDeadBefore48);
            #endregion

            #region PatientCensusClass
            subPasienCencusClass.CanGrow = true;
            pasienSensusPerKelas1.InitializeReport(lstPatientCensusClass);
            #endregion

            DateTime Tanggal = Convert.ToDateTime(Logdate);
            string filterLabel = string.Format("HealthcareServiceUnitID = {0}", ServiceUnit);
            vHealthcareServiceUnit EntityServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterLabel).FirstOrDefault();

            lblServiceUnit.Text = string.Format("Ruang Perawatan : {0}", EntityServiceUnit.ServiceUnitName);
            lblTanggal.Text = string.Format("Tanggal : {0}", Tanggal.ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }

    }
}
