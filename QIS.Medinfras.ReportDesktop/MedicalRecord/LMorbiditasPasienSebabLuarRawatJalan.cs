using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class LMorbiditasPasienSebabLuarRawatJalan : BaseCustomDailyLandscapeA3Rpt
    {
        public LMorbiditasPasienSebabLuarRawatJalan()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vHealthcare entity = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
            lblKodeProv.Text = entity.State;
            lblKodeRS.Text = entity.HealthcareID;
            lblNamaRS.Text = entity.ShortName;
            lblKabKota.Text = entity.City;
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
            
        }
    }
}
