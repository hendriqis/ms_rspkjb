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
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicalRecordList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                {
                    return Constant.MenuCode.Outpatient.MEDICAL_RECORD_LIST;
                }
                else if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                {
                    return Constant.MenuCode.Inpatient.MEDICAL_RECORD_LIST;
                }
                else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY) 
                {
                    return Constant.MenuCode.EmergencyCare.MEDICAL_RECORD_LIST;
                }
            }
            return "";
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
                txtLogDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);             
                ((GridMedicalRecordStatusCtl)grdInpatientReg).InitializeControl();
            }
        }

        public override string GetFilterExpression()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnDepartmentID.Value = Page.Request.QueryString["id"];
            }

            string filterExpression = "";
            string date = Helper.GetDatePickerValue(txtLogDate).ToString("yyyyMMdd");

            switch (hdnDepartmentID.Value)
            {
                case "ER": filterExpression = string.Format("DepartmentID = '{0}'", Constant.Facility.EMERGENCY); break;
                case "OP": filterExpression = string.Format("DepartmentID = '{0}'", Constant.Facility.OUTPATIENT); break;
                case "IP": filterExpression = string.Format("DepartmentID = '{0}'", Constant.Facility.INPATIENT); break;
            }

            if (!String.IsNullOrEmpty(date)) 
            {
                filterExpression += String.Format("AND CONVERT(date, LogDate) = '{0}'", date);
            }

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