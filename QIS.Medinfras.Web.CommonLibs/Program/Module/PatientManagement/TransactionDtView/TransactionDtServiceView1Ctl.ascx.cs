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
    public partial class TransactionDtServiceView1Ctl : BaseUserControlCtl
    {
        public void BindGrid(List<vPatientChargesDt8> lstChargesDt)
        {
            lvwService.DataSource = lstChargesDt;
            lvwService.DataBind();
            if (lstChargesDt.Count > 0)
            {
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPayer")).InnerHtml = lstChargesDt.Sum(p => p.PayerAmount).ToString("N2");
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPatient")).InnerHtml = lstChargesDt.Sum(p => p.PatientAmount).ToString("N2");
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotal")).InnerHtml = lstChargesDt.Sum(p => p.LineAmount).ToString("N2");
            }
        }

        public void HideCheckBox()
        {
            //((CheckBox)lvwService.FindControl("chkSelectAll")).Style.Add("display","none");
            hdnShowCheckbox.Value = "1";
        }

    }
}