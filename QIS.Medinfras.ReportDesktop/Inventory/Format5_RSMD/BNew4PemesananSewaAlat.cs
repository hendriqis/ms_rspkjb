using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNew4PemesananSewaAlat : BaseDailyPortraitRpt
    {
        public BNew4PemesananSewaAlat()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            if (entity.IsUrgent)
            {
                lblIsUrgent.Visible = true;
            }
            else 
            {
                lblIsUrgent.Visible = false;            
            }

            string filterExpression = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_MAX_TRANSACTION_AMOUNT_PURCHASE_ORDER);
            SettingParameterDt lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression).FirstOrDefault();
            Decimal amount = Convert.ToDecimal(lstParam.ParameterValue);

            #region Hitung Total

            decimal total = entity.TransactionAmount;

            decimal totalDiskon = 0;
            if (entity.FinalDiscount > 0)
            {
                totalDiskon = (entity.FinalDiscount / 100 * entity.TransactionAmount);
            }

            decimal ppn = 0;
            if (entity.IsIncludeVAT)
            {
                ppn = ((entity.VATPercentage * (entity.TransactionAmount - totalDiskon)) / 100);
            }
            else
            {
                ppn = 0;
            }

            decimal totalPemesanan = (total - totalDiskon) + ppn;

            #endregion

            #region Header

            vSupplier bp = BusinessLayer.GetvSupplierList(string.Format("BusinessPartnerID = {0}", entity.BusinessPartnerID)).FirstOrDefault();
            lblSupplierName.Text = bp.BusinessPartnerName;
            lblSupplierAddress.Text = bp.Address;
            lblSupplierPhone.Text = string.Format("Telp:{0} Fax:{1}", bp.PhoneNo1, bp.FaxNo1);

            lblAlamat.Text = entityHealthcare.StreetName;
            lblTelepon.Text = string.Format(" Telepon : {0} Fax {1}", entityHealthcare.PhoneNo1, entityHealthcare.FaxNo1);
            lblReportTitle.Text = "SEWA ALAT";
            #endregion

            #region Footer
            lblTermName.Text = string.Format("Cara Pembayaran : {0} setelah alat digunakan", entity.TermName);
            Int16 PPN = Convert.ToInt16(entity.IsIncludeVAT);
            if (PPN == 0)
            {
                lblCatatanPPN.Visible = false;
            }
            else
            {
                lblCatatanPPN.Visible = true;
                lblCatatanPPN.Text = string.Format("Harga tersebut diatas sudah termasuk PPn 10%.");
            }
            lblPPN.Text = ppn.ToString("N2");
            lblTotalPemesanan.Text = totalPemesanan.ToString("N2");
            lblTerbilang.Text = string.Format("Terbilang : #{0}#", Function.NumberInWords(Convert.ToInt32(totalPemesanan), true));
            lblTanggal.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString("dd-MMM-yyyy"));
            #endregion

            base.InitializeReport(param);
        }
    }
}
