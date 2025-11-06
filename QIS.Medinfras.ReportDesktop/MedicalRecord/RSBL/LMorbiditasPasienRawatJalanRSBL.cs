using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LMorbiditasPasienRawatJalanRSBL : BaseNewCustomDailyPotraitRpt
    {
        public LMorbiditasPasienRawatJalanRSBL()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            base.InitializeReport(param);

            #region Header 1 : Healthcare
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();
            Healthcare oHealthcare_ = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();
            if (oHealthcare != null)
            {
                cHealthcareName_.Text = oHealthcare.HealthcareName;
                cHealthcareCity.Text = oHealthcare.City;
                cHealthcareState.Text = oHealthcare.State;
                cHealthcareDirector.Text = oHealthcare_.DirectorName;
                cCity_PrintDate.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString("dd MMMM yyyy")); ;
            }
            #endregion
        }
    }
}