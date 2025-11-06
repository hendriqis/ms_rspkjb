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
    public partial class BNew3PermintaanPembelianDenganNilai : BaseDailyPortraitRpt
    {
        public BNew3PermintaanPembelianDenganNilai() 
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseRequestHd entity = BusinessLayer.GetvPurchaseRequestHdList(param[0])[0];
            lblPurchaseRequestNo.Text = entity.PurchaseRequestNo;
            lblLocation.Text = string.Format("{0} ({1})", entity.LocationName, entity.LocationCode);
            lblKeterangan.Text = entity.Remarks;            

            string filterExpression1 = string.Format("ParameterCode IN ('{0}')", "SA0020");
            List<SettingParameterDt> lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1);
            lblDirektur.Text = lstParam1.Where(lst => lst.ParameterCode == "SA0020").FirstOrDefault().ParameterValue;

            string filterExpression2 = string.Format("ParameterCode IN ('{0}')", "SA0021");
            List<SettingParameterDt> lstParam2 = BusinessLayer.GetSettingParameterDtList(filterExpression2);
            lblManajer.Text = lstParam2.Where(lst => lst.ParameterCode == "SA0021").FirstOrDefault().ParameterValue;

            string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST);
            SettingParameterDt lstParam3 = BusinessLayer.GetSettingParameterDtList(filterExpression3).FirstOrDefault();
            lblPenanggungJawab.Text = lstParam3.ParameterValue;

            lblCreatedByName.Text = entity.CreatedByName;

            if (entity.GCLocationGroup == Constant.LocationGroup.LOGISTIC)
            {
                lblManagerCaption.Text = "Manager Umum,";
                lblManajer.Text = "";
                lblPenanggungJawab.Text = "";
            }

            base.InitializeReport(param);
        }
    }
}
