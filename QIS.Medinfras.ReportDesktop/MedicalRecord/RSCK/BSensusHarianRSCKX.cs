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
    public partial class BSensusHarianRSCKX : BaseCustomDailyPotraitRpt
    {
        public BSensusHarianRSCKX()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split('|');
            string LogDateString = param[0];
            string ServiceUnit = param[1];
            string Logdate = param[2];
            string filterPatientOUT = string.Format(" LogDate = '{0}' AND GCATDStatus IN ('{1}', '{2}', '{3}', '{4}') AND HealthcareServiceUnitID = {5}", LogDateString, Constant.ATD_STATUS.DISCHARGED_WITH_AGREEMENT, 
                Constant.ATD_STATUS.DISCHARGED_TO_OTHER_FACILITY,Constant.ATD_STATUS.DISCHARGED_TO_ANOTHER_HOSPITAL, Constant.ATD_STATUS.DISCHARGED_FORCED_OUT, ServiceUnit);
            string filterPatientIN = string.Format("LogDate = '{0}' AND GCATDStatus = '{1}' AND HealthcareServiceUnitID = {2}", LogDateString, Constant.ATD_STATUS.ADMISSION, ServiceUnit);
            string filterPatientMoveIN = string.Format("LogDate = '{0}' AND GCATDStatus = '{1}' AND HealthcareServiceUnitID = {2}", LogDateString, Constant.ATD_STATUS.TRANSFER_IN, ServiceUnit);
            string filterPatientMoveOUT = string.Format("LogDate = '{0}' AND GCATDStatus = '{1}' AND HealthcareServiceUnitID = {2}", LogDateString, Constant.ATD_STATUS.TRANSFER_OUT, ServiceUnit);

            List<vPatientATDLogRSCK> lstPatientOUT = BusinessLayer.GetvPatientATDLogRSCKList(filterPatientOUT);
            List<vPatientATDLogRSCK> lstPatientIN = BusinessLayer.GetvPatientATDLogRSCKList(filterPatientIN);
            List<vPatientATDLogRSCK> lstPatientMoveIN = BusinessLayer.GetvPatientATDLogRSCKList(filterPatientMoveIN);
            List<vPatientATDLogRSCK> lstPatientMoveOUT = BusinessLayer.GetvPatientATDLogRSCKList(filterPatientMoveOUT);
            List<GetCencusInformationPasienSisa> lstPatientALL = BusinessLayer.GetCencusInformationPasienSisaList(Convert.ToDateTime(LogDateString), Convert.ToInt32(ServiceUnit));
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

            #region PatientAll
            subPasienAll.CanGrow = true;
            pasienSensusAll1.InitializeReport(lstPatientALL);
            #endregion

            DateTime Tanggal = Convert.ToDateTime(Logdate);
            string filterLabel = string.Format("HealthcareServiceUnitID = {0}", ServiceUnit);
            vHealthcareServiceUnit EntityServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterLabel).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            lblServiceUnit.Text = string.Format("Ruang Perawatan : {0}", EntityServiceUnit.ServiceUnitName);
            lblTanggal.Text = string.Format("Tanggal : {0}", Tanggal.ToString(Constant.FormatString.DATE_FORMAT));
            lblPrintDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblUserName.Text = string.Format("({0})", AppSession.UserLogin.UserFullName);

            base.InitializeReport(param);
        }

        private void BSensusHarianRSCK_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

    }
}
