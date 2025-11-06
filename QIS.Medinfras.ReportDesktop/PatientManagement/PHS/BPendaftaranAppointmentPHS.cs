using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPendaftaranAppointmentPHS : BaseRpt
    {
        private string businessPartner = "";
        private string tipeBPJS = BusinessLayer.GetSettingParameterDt("001", Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).ParameterValue;
        private string department = "";

        public BPendaftaranAppointmentPHS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            pctReceiptLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/LOGO_TEXT_PHS.png");
            cReceiptHealthcareName.Text = h.HealthcareName;
            cReceiptHealthcareAddress.Text = string.Format("{0} {1} {2}", h.StreetName, h.City, h.ZipCode);
            cReceiptHealthcarePhone.Text = string.Format("{0} {1}", h.PhoneNo1, h.FaxNo1);
 
            List<vAppointment> lstEntity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", param[0]));
            if (lstEntity.Count > 0)
            {
                vAppointment oData = lstEntity.FirstOrDefault();
                string DayName = GetDayFormat(oData.StartDate);
                lblStartDate.Text = string.Format("{0},{1}", DayName, oData.StartDate.ToString(Constant.FormatString.DATE_FORMAT_1));
            }

            this.DataSource = lstEntity;
        }

        private void lblQueueNo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (department != "OUTPATIENT")
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
        public static String GetDayFormat(DateTime date)
        {
            string[] hari = { "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu" };
            string hariIni = hari[(int)date.DayOfWeek];
            return string.Format("{0}", hariIni);
        }
    }
}
