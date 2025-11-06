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
    public partial class BSensusHarianRawatInapNHS : BaseCustomDailyLandscapeRpt
    {
        public BSensusHarianRawatInapNHS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            if (param[1] == "0")
            {
                string[] temp = param[0].Split('|');
                string LogDateString = param[0];
                string ServiceUnit = param[1];
                string Logdate = param[2];
                string filterPatientOUT = string.Format("{0} AND GCATDStatus IN ('{1}', '{2}', '{3}', '{4}')", LogDateString, Constant.ATD_STATUS.DISCHARGED_WITH_AGREEMENT,
                    Constant.ATD_STATUS.DISCHARGED_TO_OTHER_FACILITY, Constant.ATD_STATUS.DISCHARGED_TO_ANOTHER_HOSPITAL, Constant.ATD_STATUS.DISCHARGED_FORCED_OUT);
                string filterPatientIN = string.Format("{0} AND GCATDStatus = '{1}'", LogDateString, Constant.ATD_STATUS.ADMISSION);
                string filterPatientMoveIN = string.Format("{0} AND GCATDStatus = '{1}'", LogDateString, Constant.ATD_STATUS.TRANSFER_IN);
                string filterPatientMoveOUT = string.Format("{0} AND GCATDStatus = '{1}'", LogDateString, Constant.ATD_STATUS.TRANSFER_OUT);
                string filterPatientDead = string.Format("{0} AND GCATDStatus IN ('{1}', '{2}')", LogDateString, Constant.ATD_STATUS.DIED_AFTER_48_HR, Constant.ATD_STATUS.DIED_BEFORE_48_HR);
           
                List<vPatientATDLog> lstPatientOUT = BusinessLayer.GetvPatientATDLogList(filterPatientOUT);
                List<vPatientATDLog> lstPatientIN = BusinessLayer.GetvPatientATDLogList(filterPatientIN);
                List<vPatientATDLog> lstPatientMoveIN = BusinessLayer.GetvPatientATDLogList(filterPatientMoveIN);
                List<vPatientATDLog> lstPatientMoveOUT = BusinessLayer.GetvPatientATDLogList(filterPatientMoveOUT);
                List<vPatientATDLog> lstPatientDead = BusinessLayer.GetvPatientATDLogList(filterPatientDead);
                List<PatientCencusInformation> lstPatientCensusClass = BusinessLayer.GetCencusInformationList(Convert.ToDateTime(Logdate), Convert.ToInt32(ServiceUnit));
                List<OccupiedRoomInpatient> lstOccupiedRoom = BusinessLayer.GetOccupiedRoomInpatient(Convert.ToDateTime(Logdate), Convert.ToInt32(ServiceUnit));
                List<NursingPatientNameDead> lstPatientNameDead = BusinessLayer.GetNursingPatientNameDead(Convert.ToDateTime(Logdate), Convert.ToInt32(ServiceUnit));
                List<NursingPatientNameDischarged> lstPatientNameDischarged = BusinessLayer.GetNursingPatientNameDischarged(Convert.ToDateTime(Logdate), Convert.ToInt32(ServiceUnit));

                #region PatientKamarTerisiNHS
                subPasienSensusKamarTerisi.CanGrow = true;
                pasienSensusKamarTerisi.InitializeReport(lstOccupiedRoom);
                #endregion

                #region PatientJumlahNHS
                subPasienSensusJumlahNHS.CanGrow = true;
                pasienSensusJumlah.InitializeReport(lstPatientCensusClass);
                #endregion

                #region PatientMasukNHS
                subPasienMasuk.CanGrow = true;
                pasienSensusMasuk.InitializeReport(lstPatientIN);
                #endregion

                #region PatientPindahanNHS
                subPasienPindahan.CanGrow = true;
                pasienSensusPindahan.InitializeReport(lstPatientMoveIN);
                #endregion

                #region PatientDipindahkanNHS
                subPasienDipindahkan.CanGrow = true;
                pasienSensusDipindahkan.InitializeReport(lstPatientMoveOUT);
                #endregion

                #region PatientKeluarNHS
                subPasienKeluar.CanGrow = true;
                pasienSensusKeluar.InitializeReport(lstPatientNameDischarged);
                #endregion

                #region PatientMeninggalNHS
                subPasienMeninggal.CanGrow = true;
                pasienSensusMeninggal.InitializeReport(lstPatientNameDead);
                #endregion

                DateTime Tanggal = Convert.ToDateTime(Logdate);
                string filterLabel = string.Format("HealthcareServiceUnitID = {0}", ServiceUnit);
                vHealthcareServiceUnit EntityServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterLabel).FirstOrDefault();

                lblServiceUnit.Text = "RUANGAN : SEMUA RUANG PERAWATAN";
                lblTanggal.Text = string.Format("Tanggal : {0}", Tanggal.ToString(Constant.FormatString.DATE_FORMAT));

                base.InitializeReport(param);
            }
            else 
            {
                string[] temp = param[0].Split('|');
                string LogDateString = param[0];
                string ServiceUnit = param[1];
                string Logdate = param[2];
                string filterPatientOUT = string.Format("{0} AND GCATDStatus IN ('{1}', '{2}', '{3}', '{4}') AND HealthcareServiceUnitID LIKE '{5}'", LogDateString, Constant.ATD_STATUS.DISCHARGED_WITH_AGREEMENT,
                    Constant.ATD_STATUS.DISCHARGED_TO_OTHER_FACILITY, Constant.ATD_STATUS.DISCHARGED_TO_ANOTHER_HOSPITAL, Constant.ATD_STATUS.DISCHARGED_FORCED_OUT, ServiceUnit);
                string filterPatientIN = string.Format("{0} AND GCATDStatus = '{1}' AND HealthcareServiceUnitID LIKE '{2}'", LogDateString, Constant.ATD_STATUS.ADMISSION, ServiceUnit);
                string filterPatientMoveIN = string.Format("{0} AND GCATDStatus = '{1}' AND HealthcareServiceUnitID LIKE '{2}'", LogDateString, Constant.ATD_STATUS.TRANSFER_IN, ServiceUnit);
                string filterPatientMoveOUT = string.Format("{0} AND GCATDStatus = '{1}' AND HealthcareServiceUnitID LIKE '{2}'", LogDateString, Constant.ATD_STATUS.TRANSFER_OUT, ServiceUnit);
                string filterPatientDead = string.Format("{0} AND GCATDStatus IN ('{1}', '{2}')", LogDateString, Constant.ATD_STATUS.DIED_AFTER_48_HR, Constant.ATD_STATUS.DIED_BEFORE_48_HR);

                List<vPatientATDLog> lstPatientOUT = BusinessLayer.GetvPatientATDLogList(filterPatientOUT);
                List<vPatientATDLog> lstPatientIN = BusinessLayer.GetvPatientATDLogList(filterPatientIN);
                List<vPatientATDLog> lstPatientMoveIN = BusinessLayer.GetvPatientATDLogList(filterPatientMoveIN);
                List<vPatientATDLog> lstPatientMoveOUT = BusinessLayer.GetvPatientATDLogList(filterPatientMoveOUT);
                List<vPatientATDLog> lstPatientDead = BusinessLayer.GetvPatientATDLogList(filterPatientDead);
                List<PatientCencusInformation> lstPatientCensusClass = BusinessLayer.GetCencusInformationList(Convert.ToDateTime(Logdate), Convert.ToInt32(ServiceUnit));
                List<OccupiedRoomInpatient> lstOccupiedRoom = BusinessLayer.GetOccupiedRoomInpatient(Convert.ToDateTime(Logdate), Convert.ToInt32(ServiceUnit));
                List<NursingPatientNameDead> lstPatientNameDead = BusinessLayer.GetNursingPatientNameDead(Convert.ToDateTime(Logdate), Convert.ToInt32(ServiceUnit));
                List<NursingPatientNameDischarged> lstPatientNameDischarged = BusinessLayer.GetNursingPatientNameDischarged(Convert.ToDateTime(Logdate), Convert.ToInt32(ServiceUnit));

                #region PatientKamarTerisiNHS
                subPasienSensusKamarTerisi.CanGrow = true;
                pasienSensusKamarTerisi.InitializeReport(lstOccupiedRoom);
                #endregion

                #region PatientJumlahNHS
                subPasienSensusJumlahNHS.CanGrow = true;
                pasienSensusJumlah.InitializeReport(lstPatientCensusClass);
                #endregion

                #region PatientMasuk
                subPasienMasuk.CanGrow = true;
                pasienSensusMasuk.InitializeReport(lstPatientIN);
                #endregion

                #region PatientPindahanNHS
                subPasienPindahan.CanGrow = true;
                pasienSensusPindahan.InitializeReport(lstPatientMoveIN);
                #endregion

                #region PatientDipindahkanNHS
                subPasienDipindahkan.CanGrow = true;
                pasienSensusDipindahkan.InitializeReport(lstPatientMoveOUT);
                #endregion

                #region PatientKeluarNHS
                subPasienKeluar.CanGrow = true;
                pasienSensusKeluar.InitializeReport(lstPatientNameDischarged);
                #endregion

                #region PatientMeninggalNHS
                subPasienMeninggal.CanGrow = true;
                pasienSensusMeninggal.InitializeReport(lstPatientNameDead);
                #endregion

                DateTime Tanggal = Convert.ToDateTime(Logdate);
                string filterLabel = string.Format("HealthcareServiceUnitID = {0}", ServiceUnit);
                vHealthcareServiceUnit EntityServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterLabel).FirstOrDefault();

                lblServiceUnit.Text = string.Format("RUANGAN : {0}", EntityServiceUnit.ServiceUnitName);
                lblTanggal.Text = string.Format("Tanggal : {0}", Tanggal.ToString(Constant.FormatString.DATE_FORMAT));

                base.InitializeReport(param);
            }
        }
    }
}
