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
    public partial class BNew3PemesananBarangTanpaNilaiDraft : BaseLegalRpt
    {
        public BNew3PemesananBarangTanpaNilaiDraft()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];
            List<PurchaseOrderDt> entityDT = BusinessLayer.GetPurchaseOrderDtList(string.Format("PurchaseOrderID = {0} AND IsDeleted = 0", entity.PurchaseOrderID));
            vSupplier entitySupplier = BusinessLayer.GetvSupplierList(string.Format("BusinessPartnerID = {0}", entity.BusinessPartnerID)).FirstOrDefault();

            lblSupplierCode.Text = entity.BusinessPartnerCode;
            lblSupplierName.Text = entity.BusinessPartnerName;
            lblAddressSupplier1.Text = string.Format("{0} {1} {2}", entitySupplier.StreetName, entitySupplier.County, entitySupplier.City);
            lblAddressSupplier2.Text = string.Format("Tlp:{0} | Fax:{1}", entitySupplier.PhoneNo1, entitySupplier.FaxNo1);

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

            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                xrLabel18.Visible = false;
                lblApprover.Visible = false;
                xrLine3.Visible = false;

                lblJabatanDirektur.Text = "Direktur";

                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_KEUANGAN);
                SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                lblDirektur.Text = lstParam1.ParameterValue;

                string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST);
                SettingParameterDt lstParam3 = BusinessLayer.GetSettingParameterDtList(filterExpression3).FirstOrDefault();
                lblPenanggungJawab.Text = lstParam3.ParameterValue;

                string filterExpression4 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST_LICENSE_NO);
                SettingParameterDt lstParam4 = BusinessLayer.GetSettingParameterDtList(filterExpression4).FirstOrDefault();
                lblSIK.Text = lstParam4.ParameterValue;

                string filterExpression5 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_NAME_CREATE_PO_PHARMACY);
                SettingParameterDt lstParam5 = BusinessLayer.GetSettingParameterDtList(filterExpression5).FirstOrDefault();
                lblCreatedByName.Text = lstParam5.ParameterValue;
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                if (totalPemesanan >= amount)
                {
                    string filterExpression0 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_APPROVER_PURCHASE_ORDER);
                    SettingParameterDt lstParam0 = BusinessLayer.GetSettingParameterDtList(filterExpression0).FirstOrDefault();
                    lblApprover.Text = lstParam0.ParameterValue;
                }
                else
                {
                    xrLabel18.Visible = false;
                    lblApprover.Visible = false;
                    xrLine3.Visible = false;
                }

                lblJabatanDirektur.Text = "Manager Keuangan";

                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_KEUANGAN);
                SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                lblDirektur.Text = lstParam1.ParameterValue;

                lblPenanggungJawab.Text = "";
                lblSIK.Visible = false;

                lblCreatedByName.Text = entity.CreatedByName;
            }
        }
    }
}
