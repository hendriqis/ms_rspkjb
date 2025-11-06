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
    public partial class BNew3PermintaanPembelianTanpaNilai : BaseDailyPortraitRpt
    {
        public BNew3PermintaanPembelianTanpaNilai()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseRequestHd entity = BusinessLayer.GetvPurchaseRequestHdList(param[0])[0];
            lblPurchaseRequestNo.Text = entity.PurchaseRequestNo;
            lblLocation.Text = string.Format("{0} ({1})", entity.LocationName, entity.LocationCode);
            lblKeterangan.Text = entity.Remarks;

            string filterExpression = string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}')", Constant.SettingParameter.DIREKTUR_YANMED, Constant.SettingParameter.MANAGER_FARMASI, Constant.SettingParameter.PHARMACIST, Constant.SettingParameter.IM_MANAGER_LOGISTIK, Constant.SettingParameter.IM_MANAGER_FARMASI);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);

            lblDirektur.Text = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.DIREKTUR_YANMED).FirstOrDefault().ParameterValue;
            lblManajer.Text = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.MANAGER_FARMASI).FirstOrDefault().ParameterValue;
            lblPenanggungJawab.Text = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.PHARMACIST).FirstOrDefault().ParameterValue;

            lblCreatedByName.Text = entity.CreatedByName;

            if (entity.GCLocationGroup == Constant.LocationGroup.LOGISTIC)
            {
                lblManagerCaption.Text = lstParam.Where(t => t.ParameterCode == Constant.SettingParameter.IM_MANAGER_LOGISTIK).FirstOrDefault().ParameterValue;
                lblManajer.Text = "";
                lblPenanggungJawab.Text = "";
            }
            else
            {
                lblManagerCaption.Text = lstParam.Where(t => t.ParameterCode == Constant.SettingParameter.IM_MANAGER_FARMASI).FirstOrDefault().ParameterValue;
            }

            if (entity.IsUrgent)
            {
                lblIsUrgent.Visible = true;
            }
            else
            {
                lblIsUrgent.Visible = false;
            }

            base.InitializeReport(param);
        }

        private void StockRs_beforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Int32 ItemID = Convert.ToInt32(GetCurrentColumnValue("ItemId").ToString());
            List<ItemBalance> lstEntity = BusinessLayer.GetItemBalanceList(String.Format("ItemID = {0} AND IsDeleted = 0", ItemID));
            Int32 Jumlah = lstEntity.Sum(x => Convert.ToInt32(x.QuantityEND));
            xrTableCell10.Text = Jumlah.ToString();
        }
    }
}
