using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLSubLedgerInformationCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            List<String> lstParam = param.Split('|').ToList();
            hdnGLAccountID.Value = lstParam[0];
            hdnSubledger.Value = lstParam[1];
            hdnYear.Value = lstParam[2];
            hdnMonth.Value = lstParam[3];
            txtGLAccountNo.Text = lstParam[4];
            txtGLAccountName.Text = lstParam[5];
            hdnStatus.Value = lstParam[6];

            SetTotalText();
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

        private void SetTotalText()
        {
            decimal debit = 0, credit = 0;
            int ServiceUnitID = 0, BusinessPartnerID = 0;
            string HealthcareID = "0", DepartmentID = "0";
            List<GetGLBalanceDtInformationPerPeriode> lstEntity = BusinessLayer.GetGLBalanceDtInformationPerPeriodeList(
                                                                                                Convert.ToInt32(hdnGLAccountID.Value),
                                                                                                Convert.ToInt32(hdnSubledger.Value),
                                                                                                HealthcareID,
                                                                                                DepartmentID,
                                                                                                ServiceUnitID,
                                                                                                BusinessPartnerID,
                                                                                                Convert.ToInt32(hdnYear.Value),
                                                                                                Convert.ToInt32(hdnMonth.Value),
                                                                                                hdnStatus.Value,
                                                                                                0);
            debit = lstEntity.Sum(a => a.DEBITAmount);
            credit = lstEntity.Sum(b => b.CREDITAmount);

            txtTotalBalanceDEBIT.Text = debit.ToString("N");
            txtTotalBalanceCREDIT.Text = credit.ToString("N");
        }

        private void BindGridView()
        {
            int ServiceUnitID = 0, BusinessPartnerID = 0;
            string HealthcareID = "0", DepartmentID = "0";
            List<GetGLBalanceDtInformationPerPeriode> lstEntity = null;

            if (hdnGLAccountID.Value == "")
            {
                PageCount = 0;
                lstEntity = new List<GetGLBalanceDtInformationPerPeriode>();
                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
            else
            {
                lstEntity = BusinessLayer.GetGLBalanceDtInformationPerPeriodeList(
                                                    Convert.ToInt32(hdnGLAccountID.Value),
                                                    Convert.ToInt32(hdnSubledger.Value),
                                                    HealthcareID,
                                                    DepartmentID,
                                                    ServiceUnitID,
                                                    BusinessPartnerID,
                                                    Convert.ToInt32(hdnYear.Value),
                                                    Convert.ToInt32(hdnMonth.Value),
                                                    hdnStatus.Value.ToString(),
                                                    0);

                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
        }
    }
}