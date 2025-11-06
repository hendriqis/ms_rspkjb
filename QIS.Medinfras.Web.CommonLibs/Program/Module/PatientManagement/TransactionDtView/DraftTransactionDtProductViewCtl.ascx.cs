using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class DraftTransactionDtProductViewCtl : BaseUserControlCtl
    {
        public void BindGrid(List<vDraftPatientChargesDt> lstChargesDt)
        {
            lvwDrugMS.DataSource = lstChargesDt;
            lvwDrugMS.DataBind();
            if (lstChargesDt.Count > 0)
            {
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotalPayer")).InnerHtml = lstChargesDt.Sum(p => p.PayerAmount).ToString("N");
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotalPatient")).InnerHtml = lstChargesDt.Sum(p => p.PatientAmount).ToString("N");
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotal")).InnerHtml = lstChargesDt.Sum(p => p.LineAmount).ToString("N");
            }
        }

        public void HideCheckBox()
        {
            //((CheckBox)lvwDrugMS.FindControl("chkSelectAll")).Style.Add("display", "none");
            hdnHideCheckbox.Value = "1";
        }
    }
}