using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratPengantarTagihanRJRSDOSKA : BaseDailyPortraitRpt
    {
        public BSuratPengantarTagihanRJRSDOSKA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Sign
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            lblHealthcare.Text = oHealthcare.HealthcareName;
            lblSignDate.Text = String.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)); // Surakarta, 24-Jul-2019
            lblSignHealthcareName.Text = oHealthcare.HealthcareName;

            List<SettingParameter> lstSP = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_KEUANGAN));
            lblSignCaption.Text = lstSP.Where(a => a.ParameterCode == Constant.SettingParameter.DIREKTUR_KEUANGAN).FirstOrDefault().ParameterName;
            lblSignName.AutoWidth = true;
            lblSignName.Text = lstSP.Where(a => a.ParameterCode == Constant.SettingParameter.DIREKTUR_KEUANGAN).FirstOrDefault().ParameterValue;

            base.InitializeReport(param);
            #endregion

            #region Nomor Surat Pengantar Tagihan Ranap
            vARInvoiceHdTagihan entity = BusinessLayer.GetvARInvoiceHdTagihanList(param[0]).FirstOrDefault();
            lblSuratPengantarTagihanRJ.Text = string.Format("No:              {0}", entity.cfNoTagihanRJRSDOSKA);
            #endregion
        }

    }
}
