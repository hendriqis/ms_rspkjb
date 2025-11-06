using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CheckNewPatientCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            txtSearchDOB.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            if (!string.IsNullOrEmpty(param))
            {
                hdnMotherParameter.Value = param;
            }
        }

        protected void cbpPatient_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";

            if (!String.IsNullOrEmpty(txtSearchName.Text))
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            grdPatient.DataSource = BusinessLayer.GetvPatientList(GenerateFilterExpression());
            grdPatient.DataBind();
        }

        private string GenerateFilterExpression()
        {
            string filterExpression = "";

            if (txtSearchName.Text != string.Empty)
            {
                string filterName = "(PatientName LIKE '%" + txtSearchName.Text.Replace(" ", "%' OR PatientName LIKE '%") + "%') ";
                filterExpression += filterName;
            }

            if (txtSearchDOB.Text != string.Empty)
            {
                if (filterExpression != string.Empty) filterExpression += "AND ";
                filterExpression += string.Format("DateOfBirth = '{0}'", Helper.GetDatePickerValue(txtSearchDOB.Text));
            }

            if (txtSearchNIK.Text != string.Empty)
            {
                if (filterExpression != string.Empty) filterExpression += "AND ";
                filterExpression += string.Format("SSN LIKE '%{0}%'", txtSearchNIK.Text);
            }

            if (txtSearchAddress.Text != string.Empty)
            {
                if (filterExpression != string.Empty) filterExpression += "AND ";
                filterExpression += string.Format("StreetName LIKE '%{0}%'", txtSearchAddress.Text);
            }

            string phone = txtSearchPhoneNo.Text;
            phone = Regex.Replace(phone, "[^0-9]", "");
            if (phone != string.Empty)
            {
                if (filterExpression != string.Empty) filterExpression += "AND ";
                filterExpression += string.Format("dbo.fnRemoveNonNumericCharacters(MobilePhoneNo1) LIKE '%{0}%'", phone);
            }

            if (txtSearchMotherName.Text != string.Empty)
            {
                if (filterExpression != string.Empty) filterExpression += "AND ";
                filterExpression += string.Format("MotherName LIKE '%{0}%'", txtSearchMotherName.Text);
            }

            return filterExpression;
        }
    }
}