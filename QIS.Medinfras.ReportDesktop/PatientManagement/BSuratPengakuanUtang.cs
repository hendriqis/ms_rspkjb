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
    public partial class BSuratPengakuanUtang : BaseCustomDailyPotraitRpt
    {
        public BSuratPengakuanUtang()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vSettingParameter entityAdm = BusinessLayer.GetvSettingParameterList(String.Format("ParameterCode = '{0}'", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN))[0];
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthCareID = 001"))[0];

            lblPetugas.Text = entityAdm.ParameterValue;
            lblHealthCare.Text = entityHealthcare.HealthcareName;
            lblHealthcareAddress.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString("dd-MMM-yyyy"));
            base.InitializeReport(param);
        }
        

    }
}
