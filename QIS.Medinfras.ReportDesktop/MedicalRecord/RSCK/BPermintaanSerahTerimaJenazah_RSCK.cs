using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;
namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPermintaanSerahTerimaJenazah_RSCK : BaseCustomDailyPotraitRpt
    {
        public BPermintaanSerahTerimaJenazah_RSCK()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)

        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            lblnote.Text = string.Format("Telah menerima dari {0} {1} {2} JENAZAH ayah/ibu/anak/adik/kakak/.........saya,", entityHealthcare.HealthcareName, entityHealthcare.StreetName, entityHealthcare.City);
            lblPrintDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblPrintDate1.Text = string.Format("{0}", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblPrintTime.Text = string.Format("{0}", DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));
            lblPrintDay.Text = DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("id-ID"));

            base.InitializeReport(param);
        }
    }
}
