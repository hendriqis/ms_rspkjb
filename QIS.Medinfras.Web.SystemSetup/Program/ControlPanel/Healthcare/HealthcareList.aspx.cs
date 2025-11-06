using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class HealthcareList : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.HEALTHCARE;
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Healthcare ID", "Healthcare Name", "Short Name" };
            fieldListValue = new string[] { "HealthcareID", "HealthcareName", "ShortName" };
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            List<Healthcare> lstEntity = BusinessLayer.GetHealthcareList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = hdnFilterExpression.Value;
            List<Healthcare> lstEntity = BusinessLayer.GetHealthcareList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl("~/Program/ControlPanel/Healthcare/HealthcareEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/ControlPanel/Healthcare/HealthcareEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                BusinessLayer.DeleteHealthcare(hdnID.Value);
                return true;
            }
            return false;
        }
    }
}