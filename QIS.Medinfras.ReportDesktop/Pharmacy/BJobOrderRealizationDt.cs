using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BJobOrderRealizationDt : DevExpress.XtraReports.UI.XtraReport
    {

        protected AppSessionReport appSession;

        public BJobOrderRealizationDt()
        {
            InitializeComponent();
        }

        ////public void InitializeReport(GetPrescriptionOrderDtCustom lst)
        ////{
        ////    ReportMaster reportMaster = BusinessLayer.GetReportMasterList(String.Format("ReportCode = 'PH-00007'")).FirstOrDefault();
        ////    SettingParameterDt PHARMACIST = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.PHARMACIST);
        ////    cApoteker.Text = PHARMACIST.ParameterValue;
        ////    SettingParameterDt PHARMACIST_LICENSE_NO = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.PHARMACIST_LICENSE_NO);
        ////    cSIPA.Text = PHARMACIST_LICENSE_NO.ParameterValue;

        ////    lblReportTitle.Text = reportMaster.ReportTitle1;
        ////    lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);

        ////    //Show or Hide Header
        ////    xrLogo.Visible = reportMaster.IsShowHeader;
        ////    tableHeader.Visible = reportMaster.IsShowHeader;

        ////    //Show or Hide Footer
        ////    PageFooter.Visible = reportMaster.IsShowFooter;

        ////    //Set Top Margin
        ////    TopMargin.HeightF = TopMargin.HeightF + reportMaster.TopMargin;

        ////    //Load Healthcare Information
        ////    if (reportMaster.IsShowHeader)
        ////    {
        ////        vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
        ////        if (oHealthcare != null)
        ////        {
        ////            xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
        ////            cHealthcareName.Text = oHealthcare.HealthcareName;
        ////            cHealthcareAddress.Text = string.Format("{0} {1} {2}", oHealthcare.StreetName, oHealthcare.City, oHealthcare.ZipCode);
        ////            cHealthcarePhone.Text = oHealthcare.PhoneNo1;
        ////            cHealthcareFax.Text = oHealthcare.FaxNo1;
        ////        }
        ////    }
        ////}

        //protected string ResolveUrl(string url)
        //{
        //    return url.Replace("~", AppConfigManager.QISAppVirtualDirectory);
        //}
    }
}
