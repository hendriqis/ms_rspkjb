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
    public partial class KartuPasien15Rpt : BaseRpt
    {
        public KartuPasien15Rpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string medicalNo = param[0];
            string filterExpression = string.Format(" MRN ='{0}'", medicalNo);
            Patient entity = BusinessLayer.GetPatientList(filterExpression)[0];
            string firstName = entity.FirstName;
            string lastName = entity.LastName;
            DateTime birthday = entity.DateOfBirth;

            lblPatientName.Text = entity.Name;
            xrBarCode1.Text = entity.MedicalNo;
            lblMedicalNo.Text = entity.MedicalNo;
            lblPatientBirth.Text = string.Format("{0}", entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT));
        }
    }
}
