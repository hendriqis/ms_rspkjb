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
    public partial class BPermintaanPembelian_GRANOSTIC : BaseDailyPortraitRpt
    {
        public BPermintaanPembelian_GRANOSTIC()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseRequestHd entity = BusinessLayer.GetvPurchaseRequestHdList(param[0])[0];
            lblPurchaseRequestNo.Text = entity.PurchaseRequestNo;
            lblLocation.Text = string.Format("{0} ({1})", entity.LocationName, entity.LocationCode);
            lblTipePermintaan.Text = entity.PurchaseOrderType;
            lblKeterangan.Text = entity.Remarks;
            lblProductLine.Text = entity.ProductLineName;

            vHealthcare hsu = BusinessLayer.GetvHealthcareList(string.Format("HealthCareID = {0}", appSession.HealthcareID))[0];
            lblTanggal.Text = string.Format("{0}, {1}", hsu.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
            if (entity.IsUrgent)
            {
                lblIsUrgent.Visible = true;
            }
            else
            {
                lblIsUrgent.Visible = false;
            }

            if (entity.GCPurchaseOrderType == Constant.PurchaseOrderType.DRUGMS || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.OBAT_RANAP || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.OBAT_RAJAL || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.ALKES)
            {
                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_FARMASI);
                string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_KEUANGAN);
                List<SettingParameter> lstParam1 = BusinessLayer.GetSettingParameterList(filterExpression1);
                List<SettingParameter> lstParam2 = BusinessLayer.GetSettingParameterList(filterExpression2);

                lblKepalaGudang.Text = AppSession.UserLogin.UserFullName;
                lblJabatan.Text = "Gudang Farmasi Alkes";

                lblTextCatatan.Text = "Mengetahui";
                lblMengetahui.Text = lstParam1.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui.Text = lstParam1.FirstOrDefault().ParameterName;

                lblTextCatatan2.Text = "Menyetujui";
                lblMenyetujui.Text = lstParam2.FirstOrDefault().ParameterValue;
                lblJabatanMenyetujui.Text = lstParam2.FirstOrDefault().ParameterName;
            }
            else if (entity.GCPurchaseOrderType == Constant.PurchaseOrderType.FOOD)
            {
                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_UMUM);
                string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_INSTALASI_GIZI);
                List<SettingParameter> lstParam1 = BusinessLayer.GetSettingParameterList(filterExpression1);
                List<SettingParameter> lstParam2 = BusinessLayer.GetSettingParameterList(filterExpression2);

                lblKepalaGudang.Text = lstParam2.FirstOrDefault().ParameterValue;
                lblJabatan.Text = lstParam2.FirstOrDefault().ParameterName;

                lblTextCatatan.Text = "Menyetujui";
                lblMengetahui.Text = lstParam1.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui.Text = lstParam1.FirstOrDefault().ParameterName;

                lblTextCatatan2.Visible = false;
                lblMenyetujui.Visible = false;
                lblJabatanMenyetujui.Visible = false;
            }
            else if (entity.GCPurchaseOrderType == Constant.PurchaseOrderType.LOGISTIC)
            {
                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_UMUM);
                string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_KEUANGAN);
                List<SettingParameter> lstParam1 = BusinessLayer.GetSettingParameterList(filterExpression1);
                List<SettingParameter> lstParam2 = BusinessLayer.GetSettingParameterList(filterExpression2);

                lblKepalaGudang.Text = AppSession.UserLogin.UserFullName;
                lblJabatan.Text = "Gudang Umum";

                lblTextCatatan.Text = "Mengetahui";
                lblMengetahui.Text = lstParam1.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui.Text = lstParam1.FirstOrDefault().ParameterName;

                lblTextCatatan2.Text = "Menyetujui";
                lblMenyetujui.Text = lstParam2.FirstOrDefault().ParameterValue;
                lblJabatanMenyetujui.Text = lstParam2.FirstOrDefault().ParameterName;
            }
            else
            {
                lblKepalaGudang.Text = "";
                lblJabatan.Text = "";

                lblTextCatatan.Text = "Mengetahui";
                lblMengetahui.Text = "";
                lblJabatanMengetahui.Text = "";

                lblTextCatatan2.Visible = false;
                lblMenyetujui.Visible = false;
                lblJabatanMenyetujui.Visible = false;
            }
        }
    }
}
