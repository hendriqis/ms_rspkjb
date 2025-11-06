using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class InformationDtServiceCtl : BaseUserControlCtl
    {
        public void BindGrid(List<vPatientChargesDt> lstChargesDt)
        {
            lvwService.DataSource = lstChargesDt;
            lvwService.DataBind();
            if (lstChargesDt.Count > 0)
            {
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPayer")).InnerHtml = lstChargesDt.Sum(p => p.PayerAmount).ToString("N");
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPatient")).InnerHtml = lstChargesDt.Sum(p => p.PatientAmount).ToString("N");
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotal")).InnerHtml = lstChargesDt.Sum(p => p.LineAmount).ToString("N");
            }
        }
    }
}