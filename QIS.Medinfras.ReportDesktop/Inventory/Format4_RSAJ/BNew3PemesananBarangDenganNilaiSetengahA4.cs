using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNew3PemesananBarangDenganNilaiSetengahA4 : BaseLegalRpt
    {
        public BNew3PemesananBarangDenganNilaiSetengahA4()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];
            List<PurchaseOrderDt> lstEntityDT = BusinessLayer.GetPurchaseOrderDtList(string.Format("PurchaseOrderID = {0} AND IsDeleted = 0", entity.PurchaseOrderID));
            var result = lstEntityDT.GroupBy(pr => pr.PurchaseRequestID).Select(grp => grp.First()).ToList().OrderBy(x => x.PurchaseRequestID); //distinct pr

            vSupplier entitySupplier = BusinessLayer.GetvSupplierList(string.Format("BusinessPartnerID = {0}", entity.BusinessPartnerID)).FirstOrDefault();

            lblSupplierCode.Text = entity.BusinessPartnerCode;
            lblSupplierName.Text = entity.BusinessPartnerName;
            lblAddressSupplier1.Text = string.Format("{0} {1} {2}", entitySupplier.StreetName, entitySupplier.County, entitySupplier.City);
            lblAddressSupplier2.Text = string.Format("Tlp:{0} | Fax:{1}", entitySupplier.PhoneNo1, entitySupplier.FaxNo1);

            string prNo = "";
            foreach (PurchaseOrderDt e in result)
            {
                if (e.PurchaseRequestID != 0 && e.PurchaseRequestID != null)
                {
                    PurchaseRequestHd entityHd = BusinessLayer.GetPurchaseRequestHd(Convert.ToInt32(e.PurchaseRequestID));
                    if (String.IsNullOrEmpty(prNo))
                    {
                        prNo = entityHd.PurchaseRequestNo;
                    }
                    else
                    {
                        prNo = prNo + ", " + entityHd.PurchaseRequestNo;
                    }
                }
            }
            lblPurchaseRequestNo.Text = prNo;

            if (entity.ReferenceNo != null && entity.ReferenceNo != "")
            {
                lblPurchaseOrderNo.Text = string.Format("{0} ({1})", entity.PurchaseOrderNo, entity.ReferenceNo);
            }
            else
            {
                lblPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            }
            lblPurchaseOrderDate.Text = entity.OrderDateInString;
            lblLocation.Text = entity.LocationName;
            lblNotes.Text = entity.Remarks;
            lblSyarat.Text = entity.PaymentRemarks;

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

            #region Total Header

            lblTotal.Text = total.ToString("N2");
            lblDiskon.Text = totalDiskon.ToString("N2");
            lblPPN.Text = ppn.ToString("N2");
            lblTotalPemesanan.Text = totalPemesanan.ToString("N2");

            #endregion

            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST);
                SettingParameterDt lstParam3 = BusinessLayer.GetSettingParameterDtList(filterExpression3).FirstOrDefault();
                lblPenanggungJawab.Text = lstParam3.ParameterValue;

                string filterExpression4 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST_LICENSE_NO);
                SettingParameterDt lstParam4 = BusinessLayer.GetSettingParameterDtList(filterExpression4).FirstOrDefault();
                lblSIK.Text = lstParam4.ParameterValue;

                string filterExpression5 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_NAME_CREATE_PO_PHARMACY);
                SettingParameter lstParam5 = BusinessLayer.GetSettingParameterList(filterExpression5).FirstOrDefault();
                lblCreatedByName.Text = lstParam5.ParameterValue;
                lblCreatedByRemark.Text = lstParam5.Notes;

                xrLine3.Visible = false;
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                lblMengetahui2.Visible = false;
                lblPenanggungJawabCaption.Visible = false;
                lblPenanggungJawab.Visible = false;
                lblSIK.Visible = false;

                string filterExpression5 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_NAME_CREATE_PO_PHARMACY);
                SettingParameter lstParam5 = BusinessLayer.GetSettingParameterList(filterExpression5).FirstOrDefault();
                lblCreatedByName.Text = lstParam5.ParameterValue;
                lblCreatedByRemark.Text = lstParam5.Notes;
            }
        }
    }
}
