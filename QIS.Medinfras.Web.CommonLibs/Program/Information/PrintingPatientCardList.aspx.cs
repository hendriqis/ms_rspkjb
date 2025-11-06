using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PrintingPatientCardList : BasePageCheckRegisteredPatient
    {

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case "OP" : return Constant.MenuCode.Outpatient.PRINTING_PATIENT_CARD;
                case "ER": return Constant.MenuCode.EmergencyCare.PRINTING_PATIENT_CARD;
                case "IP": return Constant.MenuCode.Inpatient.PRINTING_PATIENT_CARD;
                case "IS": return Constant.MenuCode.Imaging.PRINTING_PATIENT_CARD;
                case "LB": return Constant.MenuCode.Laboratory.PRINTING_PATIENT_CARD;
                case "MD": return Constant.MenuCode.MedicalDiagnostic.PRINTING_PATIENT_CARD;
                case "PH": return Constant.MenuCode.Pharmacy.PRINTING_PATIENT_CARD;
                case "RT": return Constant.MenuCode.Radiotheraphy.PRINTING_PATIENT_CARD;
                default: return Constant.MenuCode.Outpatient.PRINTING_PATIENT_CARD;
            }
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

            txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            ((GridPatientBillDetailCtl)grdRegisteredPatientPrint).InitializeControl();
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string filterExpression = "IsPrintingPatientCard = 1";
           
            string id = Page.Request.QueryString["id"];

            if (!chkIsIgnoreDate.Checked)
            {
                filterExpression += string.Format(" AND RegistrationDate = '{0}'", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }
            else
            {
                filterExpression += string.Format(" AND RegistrationDate <= '{0}'", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (id == "OP")
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.OUTPATIENT);
            }
            else if (id == "ER")
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.EMERGENCY);
            }
            else if (id == "IP")
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.INPATIENT);
            }
            else if (id == "IS" || id == "LB" || id == "MD")
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.DIAGNOSTIC);
            }
            else if (id == "PH")
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.PHARMACY);
            }
            else
            {
                filterExpression += string.Format(" AND DepartmentID IS NOT NULL");
            }

            if (hdnFilterExpressionQuickSearch.Value == "Search")
            {
                hdnFilterExpressionQuickSearch.Value = "";
            }
            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }
            else
            {
                filterExpression += string.Format(" AND {0}", "1=0");
            }

            return filterExpression;
        }
    }
}