using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenerimaanPerHariRekapKwitansi : BaseDailyPortraitRpt
    {
        public LPenerimaanPerHariRekapKwitansi()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[1].Split(';');
            cParamPediode.Text = string.Format("Tanggal : {0}", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_FORMAT));
            cParamJam.Text = string.Format("Jam : {0} s/d {1}", (temp[0]).ToString(), (temp[1]).ToString());

            Healthcare h = BusinessLayer.GetHealthcare("001");
            Address a = BusinessLayer.GetAddressList(string.Format("AddressID = {0}", h.AddressID)).FirstOrDefault();
            lblTanggal.Text = string.Format("{0}, {1}", a.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            lblKasir.Text = appSession.UserFullName;

            //string filterExpression = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.FN_NAMA_KOORDINATOR_KASIR);
            //SettingParameterDt lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression).FirstOrDefault();
            //lblKasir.Text = lstParam.ParameterValue;

            base.InitializeReport(param);
        }

        private void lblReceiveAmountInWords_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Decimal RecieveAmount = Convert.ToDecimal(lblReceiveAmount.Summary.GetResult());
            string text = Function.NumberInWords(Convert.ToInt32(RecieveAmount), true);
            e.Result = text;
            e.Handled = true;
        }
    }
}
