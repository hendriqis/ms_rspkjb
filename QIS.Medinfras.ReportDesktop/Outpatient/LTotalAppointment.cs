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
    public partial class LTotalAppointment : BaseDailyPortraitRpt
    {
        public LTotalAppointment()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            List<GetTotalAppointmentGranostic> lstTotalVisit = BusinessLayer.GetTotalAppointmentGranosticList(param[0]);

            int TCC = lstTotalVisit.AsEnumerable().Where(a => a.AppointmentMethod == ("Call Center")).Sum(a => a.totalAppointment);
            int TMA = lstTotalVisit.AsEnumerable().Where(a => a.AppointmentMethod == ("Mobile Apps")).Sum(a => a.totalAppointment);
            int TRL = lstTotalVisit.AsEnumerable().Where(a => a.AppointmentMethod == ("Registrasi Langsung")).Sum(a => a.totalAppointment);
            int TWR = lstTotalVisit.AsEnumerable().Where(a => a.AppointmentMethod == ("Web Registrasi")).Sum(a => a.totalAppointment);

            if (lstTotalVisit.Where(a => a.AppointmentMethod == "Call Center") != null)
            {
                lblTAPS.Text = string.Format("{0}", TCC);
            }
            else
            {
                lblTAPS.Text = string.Format("0");
            }
            if (lstTotalVisit.Where(a => a.AppointmentMethod == "Mobile Apps") != null)
            {
                lblTAPD.Text = string.Format("{0}", TMA);
            }
            else
            { 
                lblTAPD.Text = string.Format("0");
            }
            if (lstTotalVisit.Where(a => a.AppointmentMethod == "Registrasi Langsung") != null)
            {
                lblTAPP.Text = string.Format("{0}", TRL);
            }
            else
            {
                lblTAPP.Text = string.Format("0");
            }
            if (lstTotalVisit.Where(a => a.AppointmentMethod == "Web Registrasi") != null)
            {
                lblTAPL.Text = string.Format("{0}", TWR);
            }
            else
            {
                lblTAPL.Text = string.Format("0");
            }
            base.InitializeReport(param);
        }

    }
}
