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
    public partial class BNew4PermintaanPembelianDenganNilai : BaseDailyPortraitRpt
    {
        public BNew4PermintaanPembelianDenganNilai() 
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseRequestHd entity = BusinessLayer.GetvPurchaseRequestHdList(param[0])[0];
            lblPurchaseRequestNo.Text = entity.PurchaseRequestNo;
            lblPurchaseRequestDate.Text = entity.TransactionDateInString;
            lblLocation.Text = string.Format("{0} ({1})", entity.LocationName, entity.LocationCode);
            lblProductLine.Text = entity.ProductLineName;

            string filterExpression1 = string.Format("ParameterCode IN ('{0}')", "SA0020");
            List<SettingParameterDt> lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1);
            //lblDirektur.Text = lstParam1.Where(lst => lst.ParameterCode == "SA0020").FirstOrDefault().ParameterValue;

            string filterExpression2 = string.Format("ParameterCode IN ('{0}')", "SA0021");
            List<SettingParameterDt> lstParam2 = BusinessLayer.GetSettingParameterDtList(filterExpression2);
            //lblKeuangan.Text = lstParam2.Where(lst => lst.ParameterCode == "SA0021").FirstOrDefault().ParameterValue;

            string filterExpression5 = string.Format("ParameterCode IN ('{0}')", "IM0009");
            List<SettingParameterDt> lstParam5 = BusinessLayer.GetSettingParameterDtList(filterExpression5);
            //lblKepalaLogistik.Text = lstParam5.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;

            base.InitializeReport(param);
        }
    }
}
