using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class KartuPasien2NewRpt : QIS.Medinfras.ReportDesktop.BaseRpt
    {
        public KartuPasien2NewRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            this.Landscape = true;
            string medicalNo = param[0];
            string filterExpression = string.Format(" MRN ='{0}'", medicalNo);
            Patient entity = BusinessLayer.GetPatientList(filterExpression)[0];
            string firstName = entity.FirstName;
            string lastName = entity.LastName;

            lblPatientName.Text = entity.Name;
            lblDateOfBirth.Text = String.Format("{0}", entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT));
            xrBarCode1.Text = entity.MedicalNo;
            lblMedicalNo.Text = entity.MedicalNo;
        }
    }
}
