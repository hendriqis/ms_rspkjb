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
    public partial class BPermintaanBarangRSSBB : BaseDailyPortraitRpt
    {
        public BPermintaanBarangRSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vItemRequestHd entity = BusinessLayer.GetvItemRequestHdList(param[0])[0];
            lblRequestNo.Text = entity.ItemRequestNo;
            lblRequestDate.Text = entity.TransactionDateInString;
            lblLocationCodeFrom.Text = entity.FromLocationCode;
            lblLocationNameFrom.Text = entity.FromLocationName;
            lblLocationCodeTo.Text = entity.ToLocationCode;
            lblLocationNameTo.Text = entity.ToLocationName; 
            
            lblCreatedByName.Text = entity.CreatedByName;

            //string filterExpression = string.Format(" ParameterCode IN ('{0}')", "IM0009");
            //List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            //lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;
            base.InitializeReport(param);
        }

    }
}
