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
    public partial class BSuratKontrolBPJS_RSDOSOBA : BaseA6Rpt
    {
        public BSuratKontrolBPJS_RSDOSOBA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
            ReportTitle1.Text = reportMaster.ReportTitle1;

            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityCV.RegistrationID)).FirstOrDefault();
            vRegistrationBPJSInfo2 entityRegBPJS = BusinessLayer.GetvRegistrationBPJSInfo2List(String.Format("RegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();
            lblNoSuratKontrol.Text = entityRegBPJS.NoSuratKontrol;

            NoRM.Text = entityReg.MedicalNo;
            Nama.Text = entityReg.cfPatientNameSalutation;
            Poli.Text = entityCV.ServiceUnitName;
            if (entityRegBPJS.KodeRujukan != BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.BPJS_CODE).ParameterValue)
            {
                StartDate.Text = entityRegBPJS.TanggalRujukan.ToString(Constant.FormatString.DATE_FORMAT);
                EndDate.Text = entityRegBPJS.TanggalPulang.ToString(Constant.FormatString.DATE_FORMAT);
            }
            else
            {
                StartDate.Text = "";
                EndDate.Text = "";
            }
            
            NoKartuJKN.Text = entityReg.NHSRegistrationNo;
            NoSEP.Text = entityReg.NoSEP;
            //Terapi.Text = "";  //permintaan rsdosoba dikosongi
            //Diagnosa.Text = "";//permintaan rsdosoba dikosongi

            lblPrintDate.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            lblParamedicName.Text = BusinessLayer.GetParamedicMaster(Convert.ToInt32(entityCV.ParamedicID)).FullName;

            base.InitializeReport(param);
        }

    }
}
