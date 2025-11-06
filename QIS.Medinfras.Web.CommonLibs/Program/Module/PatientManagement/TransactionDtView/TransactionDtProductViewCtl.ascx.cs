using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionDtProductViewCtl : BaseUserControlCtl
    {
        public void BindGrid(List<vPatientChargesDt8> lstChargesDt)
        {
            lvwDrugMS.DataSource = lstChargesDt;
            lvwDrugMS.DataBind();
            if (lstChargesDt.Count > 0)
            {
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotalPayer")).InnerHtml = lstChargesDt.Sum(p => p.PayerAmount).ToString("N2");
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotalPatient")).InnerHtml = lstChargesDt.Sum(p => p.PatientAmount).ToString("N2");
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotal")).InnerHtml = lstChargesDt.Sum(p => p.LineAmount).ToString("N2");
            }
        }

        public void HideCheckBox()
        {
            //((CheckBox)lvwDrugMS.FindControl("chkSelectAll")).Style.Add("display", "none");
            hdnHideCheckbox.Value = "1";
        }
    }
}