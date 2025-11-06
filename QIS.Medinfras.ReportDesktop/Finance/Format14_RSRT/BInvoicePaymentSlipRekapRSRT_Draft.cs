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
    public partial class BInvoicePaymentSlipRekapRSRT_Draft : BaseCustomDailyPotraitRSRTRpt
    {
        public BInvoicePaymentSlipRekapRSRT_Draft()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vARInvoiceHd1 entity = BusinessLayer.GetvARInvoiceHd1List(param[0]).FirstOrDefault();
             
            lblTanggalTTD.Text = string.Format("{0}, {1}", h.City, entity.ARInvoiceDateInString);
       
            
            string createdBy = "";
            if (entity != null)
            {
                createdBy = entity.CreatedByName;
            }
            lblTTD1.Text = createdBy;
            lblTTD2.Text = "Bagian keuangan"; 

            base.InitializeReport(param);
        }

    }
}
