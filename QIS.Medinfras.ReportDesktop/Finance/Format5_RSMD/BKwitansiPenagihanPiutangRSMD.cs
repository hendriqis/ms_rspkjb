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
    public partial class BKwitansiPenagihanPiutangRSMD : BaseRpt
    {
        private string city = "";

        public BKwitansiPenagihanPiutangRSMD()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            List<vARInvoiceHd> lstEntity = BusinessLayer.GetvARInvoiceHdList(param[0]);

            city = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault().City;
            SettingParameter svSign = BusinessLayer.GetSettingParameter(Constant.SettingParameter.MANAGER_KEUANGAN);

            lblDateSign.Text = String.Format("{0}, {1}", city, lstEntity.FirstOrDefault().ARInvoiceDateInString);
            lblNameSign.Text = svSign.ParameterValue;

            vARInvoiceHd entity = lstEntity.FirstOrDefault();
            if (entity.BusinessPartnerID == 1)
            {
                Patient patient = BusinessLayer.GetPatient(entity.MRN);
                xrLabel2.Text = patient.FullName;
            }
            else 
            {
                xrLabel2.Text = entity.BusinessPartnerName;
            }

            this.DataSource = lstEntity;
        }
    }
}
