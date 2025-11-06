using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class InquiryList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Outpatient.PATIENT_VISIT_INQUIRY;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                //string filterExpression = string.Format("IsAvailable = 1");
                string filterExpression = string.Format("");

                List<vParamedicMaster> lstPrmd = BusinessLayer.GetvParamedicMasterList(filterExpression);
                Methods.SetComboBoxField<vParamedicMaster>(cboDokter, lstPrmd, "ParamedicName", "ParamedicID");
                cboDokter.SelectedIndex = 0;

                List<vHealthcareServiceUnit> lstKln = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboKlinik, lstKln, "ServiceUnitName", "HealthcareServiceUnitID");
                cboKlinik.SelectedIndex = 0;

                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                grdRegisteredPatient.InitializeControl();
            }
        }
        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("HealthcareServiceUnitID = '{0}' AND ParamedicID = '{1}' ", cboKlinik.Value, cboDokter.Value);
            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }


    }
}