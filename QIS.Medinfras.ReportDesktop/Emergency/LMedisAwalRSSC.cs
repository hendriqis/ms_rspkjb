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
    public partial class LMedisAwalRSSC : BaseDailyPortraitRpt
    {
        public LMedisAwalRSSC()
        {
            InitializeComponent();
        }

        //public override void InitializeReport(string[] param)
        //{
        //    #region Header 1 : Healthcare
        //    vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
        //    if (oHealthcare != null)
        //    {
        //        xrPictureBox1.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
        //        cHealthcareName.Text = oHealthcare.HealthcareName;
        //        cHealthcareAddress.Text = oHealthcare.StreetName;
        //        cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
        //        cHealthcarePhone.Text = oHealthcare.PhoneNo1;
        //        cHealthcareFax.Text = oHealthcare.FaxNo1;

        //        lblCity.Text = oHealthcare.City;
        //    }
        //    #endregion
        //}
    }
}