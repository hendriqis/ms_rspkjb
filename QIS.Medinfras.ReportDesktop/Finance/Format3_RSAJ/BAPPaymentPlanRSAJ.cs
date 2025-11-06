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
    public partial class BAPPaymentPlanRSAJ : BaseCustomDailyPotraitRpt
    {
        public BAPPaymentPlanRSAJ()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vSupplierPaymentHd entity = BusinessLayer.GetvSupplierPaymentHdList(param[0]).FirstOrDefault();
            cNoPembayaran.Text = entity.SupplierPaymentNo;
            cTanggalVerifikasi.Text = entity.VerificationDateInString;
            cNoReferensi.Text = entity.ReferenceNo;
            cTanggalReferensi.Text = entity.ReferenceDateInString;
            //cSupplier.Text = entity.BusinessPartnerName;
            if (entity.BankID != null && entity.BankID != 0)
            {
                cCaraPembayaran.Text = entity.PaymentMethod + " | " + entity.BankName;
            }
            else
            {
                cCaraPembayaran.Text = entity.PaymentMethod;
            }
            cNoCek.Text = entity.BankReferenceNo;
            cKeterangan.Text = entity.Remarks;

            cTTDPreparedBy.Text = entity.CreatedByName;
            cTTDReviewedBy.Text = entity.VerifiedByName;
            cTTDPrintedBy.Text = appSession.UserFullName;

            base.InitializeReport(param);
        }
    }
}
