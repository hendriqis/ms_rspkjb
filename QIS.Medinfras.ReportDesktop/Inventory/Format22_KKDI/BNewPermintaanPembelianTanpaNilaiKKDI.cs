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
    public partial class BNewPermintaanPembelianTanpaNilaiKKDI : BaseDailyPortraitRpt
    {
        public BNewPermintaanPembelianTanpaNilaiKKDI() 
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filterexpression = string.Format("PurchaseRequestID = {0}",param[0]);
            vPurchaseRequestHd entity = BusinessLayer.GetvPurchaseRequestHdList(filterexpression).FirstOrDefault();
            lblPurchaseRequestNo.Text = entity.PurchaseRequestNo;
            lblLocation.Text = string.Format("{0} ({1})", entity.LocationName, entity.LocationCode);
            lblProductLine.Text = entity.ProductLineName;
            lblKeterangan.Text = entity.Remarks;

   
           
            if (entity.IsUrgent)
            {
                lblIsUrgent.Visible = true;
            }
            else
            {
                lblIsUrgent.Visible = false;
            }

            string filterparam = string.Format(" ParameterCode IN ('{0}', '{1}','{2}')", Constant.SettingParameter.KEPALA_LOGISTIK_OBAT,
                Constant.SettingParameter.KEPALA_LOGISTIK_UMUM, Constant.SettingParameter.PHARMACIST);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterparam);


            lblMeminta.Text = entity.CreatedByName;

            if (entity.GCItemType == Constant.ItemType.BARANG_UMUM)
            {
                lblMengetahui.Text = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.KEPALA_LOGISTIK_UMUM).FirstOrDefault().ParameterValue;
            }
            else 
            {
                lblMengetahui.Text = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.PHARMACIST).FirstOrDefault().ParameterValue;
            }

            lblMenyetujui.Text = "dr. SUSI ANGGRAINI, MM";

            base.InitializeReport(param);
            
        }
    }
}
