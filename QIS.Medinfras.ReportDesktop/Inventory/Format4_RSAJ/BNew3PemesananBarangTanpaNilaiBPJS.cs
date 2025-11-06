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
    public partial class BNew3PemesananBarangTanpaNilaiBPJS : BaseLegalRpt
    {
        public BNew3PemesananBarangTanpaNilaiBPJS()
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

            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                string filterExpression3 = string.Format("ParameterCode IN ('{0}')", "PH0004");
                List<SettingParameterDt> lstParam3 = BusinessLayer.GetSettingParameterDtList(filterExpression3);
                lblApoteker.Text = lstParam3.Where(lst => lst.ParameterCode == "PH0004").FirstOrDefault().ParameterValue;

                string filterExpression4 = string.Format("ParameterCode IN ('{0}')", "PH0005");
                List<SettingParameterDt> lstParam4 = BusinessLayer.GetSettingParameterDtList(filterExpression4);
                lblSIK.Text = lstParam4.Where(lst => lst.ParameterCode == "PH0005").FirstOrDefault().ParameterValue;

                string filterExpression5 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_NAME_CREATE_PO_PHARMACY);
                SettingParameterDt lstParam5 = BusinessLayer.GetSettingParameterDtList(filterExpression5).FirstOrDefault();
                lblEntryBy.Text = lstParam5.ParameterValue;
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                lblMengetahui2.Visible = false;
                lblPenanggungJawab.Visible = false;
                lblApoteker.Visible = false;
                lblSIK.Visible = false;

                string filterExpression5 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_NAME_CREATE_PO_PHARMACY);
                SettingParameterDt lstParam5 = BusinessLayer.GetSettingParameterDtList(filterExpression5).FirstOrDefault();
                lblEntryBy.Text = lstParam5.ParameterValue;
            }
        }
    }
}
