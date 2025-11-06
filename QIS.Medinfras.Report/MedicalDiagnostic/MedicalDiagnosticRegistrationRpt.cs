using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Report
{
    public partial class MedicalDiagnosticRegistrationRpt : BaseDailyPortraitRpt
    {
        public MedicalDiagnosticRegistrationRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filterExpression = "";

            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.OtherMedicalDiagnostic)
            {
                filterExpression = string.Format("HealthcareServiceUnitID = {0}", param[1]);
            }
            else
                filterExpression = string.Format("HealthcareServiceUnitID = {0}", AppSession.MedicalDiagnostic.HealthcareServiceUnitID);
            vHealthcareServiceUnit entity = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression)[0];
            lblPenunjangMedis.Text = entity.ServiceUnitName;
            base.InitializeReport(param);
        }
    }
}
