using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BKwitansiJasaDokter : BaseRpt
    {
        private string city = "";

        public BKwitansiJasaDokter()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();
            if (oHealthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/LOGO_TEXT.png");
            }

            lblHealthcare.Text = oHealthcare.HealthcareName;
            lblDate.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            List<GetKwitansiHonorDokter> lstEntity = BusinessLayer.GetKwitansiHonorDokter(Convert.ToInt32(param[0]));
            this.DataSource = lstEntity;
        }
    }
}
