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
    public partial class LIndeksTindakan : BaseCustomDailyLandscapeRpt
    {
        public LIndeksTindakan()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            if (!String.IsNullOrEmpty(param[2]))
            {
                txtDepartment.Text = string.Format("Department : {0}", param[2]);
            }
            else
            {
                txtDepartment.Text = string.Format("Department : Semua");
            }

            if (!String.IsNullOrEmpty(param[1]))
            {
                vHealthcareServiceUnit vsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", param[1])).FirstOrDefault();
                txtPelayanan.Text = string.Format("Pelayanan : {0}", vsu.ServiceUnitName);

            }
            else
            {
                txtPelayanan.Text = string.Format("Pelayanan : Semua");

            }
            base.InitializeReport(param);
        }
    }
}
