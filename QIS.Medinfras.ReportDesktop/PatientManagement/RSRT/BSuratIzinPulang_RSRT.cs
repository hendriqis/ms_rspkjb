using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratIzinPulang_RSRT : BaseCustomDailyPotraitRptRSRT
    {
        public BSuratIzinPulang_RSRT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            
            vRegistration entity = BusinessLayer.GetvRegistrationList(param[0])[0];
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];

            lblNamaPasien.Text = entityP.PatientName;
            lblNoRM.Text = entityP.MedicalNo;
            lblNoReg.Text = entity.RegistrationNo;

            lblKasir.Text = appSession.UserFullName;
            string date = "";
            if (entity.DischargeDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
            {
                date = entity.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
            else {
                date = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
            }

            lblTglTTD.Text = string.Format("Jakarta, {0}", date);

            base.InitializeReport(param);

            lblTitle.Text = reportMaster.ReportTitle1;

             
        }

    }
}
