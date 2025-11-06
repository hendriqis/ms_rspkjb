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
    public partial class ClassList : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.CLASS_CARE;
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Class Care Code", "Class Care Name", "Short Name" };
            fieldListValue = new string[] { "ClassCode", "ClassName", "ShortName" };
        }

        protected override void InitializeDataControl()
        {
            List<ClassCare> lstEntity = BusinessLayer.GetClassCareList("IsDeleted = 0 ORDER BY ClassCode ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += "IsDeleted = 0 ORDER BY ClassCode ASC";
            List<ClassCare> lstEntity = BusinessLayer.GetClassCareList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();            
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl("~/Program/ClassCare/ClassEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/ClassCare/ClassEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }
    }
}