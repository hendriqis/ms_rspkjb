using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPembatalanPerjanjianPasien : BaseCustomDailyPotraitRpt
    {
        public LPembatalanPerjanjianPasien()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            String filter = String.Format("StartDate BETWEEN '{0}' AND '{1}'", Helper.YYYYMMDDToDate(temp[0]).ToString("yyyy-MM-dd"), Helper.YYYYMMDDToDate(temp[1]).ToString("yyyy-MM-dd"));
            List<Appointment> lstEntity = BusinessLayer.GetAppointmentList(filter);
            List<Appointment> lstEntityVoid = lstEntity.Where(p => p.GCAppointmentStatus == Constant.AppointmentStatus.DELETED).ToList();

            Decimal result = 0;
            Decimal total = lstEntity.Count();
            Decimal voidCount = lstEntityVoid.Count();
            result = (voidCount / total) * 100;
            lblPresBatal.Text = result.ToString("N2") + " %";

            base.InitializeReport(param);
        }
    }
}