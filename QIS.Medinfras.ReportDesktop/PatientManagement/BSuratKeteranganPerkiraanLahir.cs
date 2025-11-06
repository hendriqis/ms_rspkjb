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
    public partial class BSuratKeteranganPerkiraanLahir : BaseDailyPortraitRpt
    {
        public BSuratKeteranganPerkiraanLahir()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format(param[0]))[0];
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID))[0];

            lblName.Text = entity.PatientName;
            lblPrintDate.Text = string.Format("{0}, {1}", entityH1.City, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
            lblParamedic.Text = entity.ParamedicName;

            //String Tanggal = param[0];

            lblNoRegis.Text = string.Format("{0}", (param[1]));
            lblOccupation.Text = string.Format("{0}", (param[2]));
            lblDate.Text = string.Format("{0}", param[3]);
            //lblDate.Text = string.Format("{0}", Tanggal);
            base.InitializeReport(param);
        }
    }
}
