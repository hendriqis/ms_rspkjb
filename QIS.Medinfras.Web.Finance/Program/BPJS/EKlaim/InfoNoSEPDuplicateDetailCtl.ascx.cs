using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InfoNoSEPDuplicateDetailCtl : BaseContentPopupCtl
    {
        protected int PageCount = 1;

        private EKlaimEntry DetailPage
        {
            get { return (EKlaimEntry)Page; }
        }

        public override void InitializeControl(string param)
        {
            hdnNoSEPCtl.Value = param;

            txtSEPNo.Text = string.Format("{0}", hdnNoSEPCtl.Value);

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("NoSEP = '{0}' ORDER BY RegistrationID", hdnNoSEPCtl.Value);
            List<vRegistrationDetailDuplicateNoSEP> lstEntity = BusinessLayer.GetvRegistrationDetailDuplicateNoSEPList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = e.Parameter;
            string result = param + "|";
            string retval = "";

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }
    }
}