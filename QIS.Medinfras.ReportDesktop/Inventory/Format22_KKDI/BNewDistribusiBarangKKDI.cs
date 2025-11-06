using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNewDistribusiBarangKKDI : BaseDailyPortraitRpt
    {
        public BNewDistribusiBarangKKDI()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param) 
        {
            vItemDistributionHd entityHd = BusinessLayer.GetvItemDistributionHdList(param[0])[0];
            lblDistributionNo.Text = entityHd.DistributionNo;
            lblTransactionDate.Text = entityHd.TransactionDateTimeInString;
            lblDistributionDate.Text = entityHd.DeliveryDateTimeInString;
            lblWarehouseCode.Text = String.Format("{0} - {1}", entityHd.FromLocationCode, entityHd.FromLocationName);
            lblOtherWarehouseCode.Text = String.Format("{0} - {1}", entityHd.ToLocationCode, entityHd.ToLocationName);
            lblRemarks.Text = entityHd.DeliveryRemarks;

            if (entityHd.RegistrationID != 0 && entityHd.RegistrationID != null)
            {
                lblRegistration.Text = string.Format("{0} | {1} | ({2}) {3}", entityHd.RegistrationNo, entityHd.ServiceUnitName, entityHd.MedicalNo, entityHd.PatientName);
            }
            else
            {
                lblRegistration.Text = "-";
            }

            string filterLokasi = string.Format(" ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC, 
                Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY);
            List<SettingParameterDt> lstLokasi= BusinessLayer.GetSettingParameterDtList(filterLokasi);
            string ParamLokasiGudangUmum = lstLokasi.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC).FirstOrDefault().ParameterValue;
            string ParamLokasiGudangFarmasi = lstLokasi.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY).FirstOrDefault().ParameterValue;

            string filterExpression = string.Format(" ParameterCode IN ('{0}', '{1}')", Constant.SettingParameter.KEPALA_LOGISTIK_OBAT, 
                Constant.SettingParameter.KEPALA_UNIT_LOGISTIK_UMUM);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);

            string FromLocation = String.Format("{0}",entityHd.FromLocationID);

            if (FromLocation == ParamLokasiGudangFarmasi)
            {
                lblCreatedByName.Text = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.KEPALA_LOGISTIK_OBAT).FirstOrDefault().ParameterValue;
            }
            else if (FromLocation == ParamLokasiGudangUmum)
            {
                lblCreatedByName.Text = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.KEPALA_UNIT_LOGISTIK_UMUM).FirstOrDefault().ParameterValue;
            }
            else
            {
                lblCreatedByName.Text = entityHd.CreatedByName;
            }
            base.InitializeReport(param);
        }
    }
}
