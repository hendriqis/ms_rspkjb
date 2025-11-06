using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPermintaanPemeriksaanRSBL : BaseDailyPortraitRpt
    {
        public BPermintaanPemeriksaanRSBL()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filterExpression = "";

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", param[0])).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();

            cParamedicName.Text = entityReg.ParamedicName;
            cDateOfBirth.Text = string.Format("{0} / {1} Tahun / {2}", entityReg.DateOfBirthInString, entityReg.PatientAgeInYear, entityReg.cfGenderInitial);
            lblBusinessPartnerName.Text = entityReg.BusinessPartnerName;

            if (param[1] != "" && param[1] != "0")
            {
                filterExpression = string.Format("TestOrderID = {0}", param[1]);
                
                vTestOrderHd entityOrderHd = BusinessLayer.GetvTestOrderHdList(filterExpression).FirstOrDefault();
                lblNoOrder.Text = entityOrderHd.TestOrderNo;
                lblTglOrder.Text = entityOrderHd.TestOrderDateTimeInString;
                lblRemaksHd.Text = entityOrderHd.Remarks;
            }
            else
            {
                lblNoOrder.Text = "";
                lblTglOrder.Text = "";
                lblRemaksHd.Text = "";
            }

            base.InitializeReport(param);

        }
    }
}
