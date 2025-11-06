using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class FNGLBalanceInformationCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            List<String> lstParam = param.Split('|').ToList();
            hdnGLAccountID.Value = lstParam[0];
            ChartOfAccount coa = BusinessLayer.GetChartOfAccount(Convert.ToInt32(hdnGLAccountID.Value));
            txtGLAccountName.Text = coa.GLAccountName;
            txtGLAccountNo.Text = coa.GLAccountNo;
            hdnYear.Value = lstParam[1];
            hdnMonth.Value = lstParam[2];
            hdnStatus.Value = lstParam[3];

            BindGridView();
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh|" + PageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            int SubLedgerID = 0, ServiceUnitID = 0, BusinessPartnerID = 0;
            string HealthcareID = "0", DepartmentID = "0";
            List<GetGLBalanceDtInformationPerPeriode> lstEntity = BusinessLayer.GetGLBalanceDtInformationPerPeriodeList(
                                                                                                 Convert.ToInt32(hdnGLAccountID.Value), 
                                                                                                 SubLedgerID,
                                                                                                 HealthcareID,
                                                                                                 DepartmentID,
                                                                                                 ServiceUnitID,
                                                                                                 BusinessPartnerID,
                                                                                                 Convert.ToInt32(hdnYear.Value),
                                                                                                 Convert.ToInt32(hdnMonth.Value),
                                                                                                 hdnStatus.Value,
                                                                                                 0);

            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}