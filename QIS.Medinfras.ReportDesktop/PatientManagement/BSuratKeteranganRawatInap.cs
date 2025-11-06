using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratKeteranganRawatInap : BaseRpt
    {
        public BSuratKeteranganRawatInap()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            int RegistrationID = Convert.ToInt32(param[0]);
            string jamPrint = DateTime.Now.ToString("HHmmss");
            string userName = AppSession.UserLogin.UserName;
            string namaKepala = "";
            List<vConsultVisit> entity = BusinessLayer.GetvConsultVisitList(String.Format("RegistrationID = '{0}'", RegistrationID));
            List<SettingParameterDt> lstEntityParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = {0} AND ParameterCode = '{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IP_KEPALA_BAGIAN_REGISTRASI_RAWAT_INAP));
            namaKepala = lstEntityParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IP_KEPALA_BAGIAN_REGISTRASI_RAWAT_INAP).ParameterValue;
            lblServiceUnit.Text = string.Format("Di : {0}", entity.FirstOrDefault().ServiceUnitName);
            lblDateNow.Text = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
            lblJamPrint.Text = string.Format("{0}{1}{2}", jamPrint, entity.FirstOrDefault().RegistrationNo, userName);
            lblNamaKepala.Text = namaKepala;
            this.DataSource = entity;
        }
    }
}
