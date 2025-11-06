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

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MedicalDiagnosticRegistrationRpt : BaseDailyPortraitRpt
    {
        public MedicalDiagnosticRegistrationRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            string filterExpression = reportMaster.AdditionalFilterExpression;
            if (filterExpression == "")
                filterExpression = string.Format("HealthcareServiceUnitID = {0}", param[1]);
            else
            {
                if (filterExpression.Contains("@LaboratoryID"))
                {
                    string laboratoryID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                    vHealthcareServiceUnit HSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1} AND IsDeleted = 0", appSession.HealthcareID, laboratoryID))[0];
                    filterExpression = filterExpression.Replace("@LaboratoryID", HSU.HealthcareServiceUnitID.ToString());
                }
                else if (filterExpression.Contains("@ImagingID"))
                {
                    string imagingID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                    vHealthcareServiceUnit HSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1} AND IsDeleted = 0", appSession.HealthcareID, imagingID))[0];
                    filterExpression = filterExpression.Replace("@ImagingID", HSU.HealthcareServiceUnitID.ToString());
                }
            }
            vHealthcareServiceUnit entity = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression)[0];
            lblPenunjangMedis.Text = entity.ServiceUnitName;
            base.InitializeReport(param);
        }
    }
}
