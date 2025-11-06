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
    public partial class BSebabKematian : BaseCustomDailyPotraitRpt
    {
        public BSebabKematian()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Healthcare healthcare = BusinessLayer.GetHealthcare(appSession.HealthcareID);
            Address address = BusinessLayer.GetAddress(Convert.ToInt32(healthcare.AddressID));
            cHealthcareEmail.Text = address.Email;
            cHealthcareCity.Text = string.Format("{0}, {1}", address.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }

    }
}
