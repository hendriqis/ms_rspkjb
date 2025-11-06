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
    public partial class DepartmentList : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.FACILITY;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            List<Department> lstEntity = BusinessLayer.GetDepartmentList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Facility Code", "Facility Name", "Short Name" };
            fieldListValue = new string[] { "DepartmentID", "DepartmentName", "ShortName" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += "IsActive = 1";
            return filterExpression;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = GetFilterExpression();
            List<Department> lstEntity = BusinessLayer.GetDepartmentList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl("~/Program/Master/Department/DepartmentEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/Master/Department/DepartmentEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }
    }
}