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
    public partial class InformationDtLogisticCtl : BaseUserControlCtl
    {
        public void BindGrid(List<vPatientChargesDt> lstChargesDt)
        {
            lvwLogistic.DataSource = lstChargesDt;
            lvwLogistic.DataBind();
            if (lstChargesDt.Count > 0)
            {
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotalPayer")).InnerHtml = lstChargesDt.Sum(p => p.PayerAmount).ToString("N");
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotalPatient")).InnerHtml = lstChargesDt.Sum(p => p.PatientAmount).ToString("N");
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotal")).InnerHtml = lstChargesDt.Sum(p => p.LineAmount).ToString("N");
            }
        }
    }
}