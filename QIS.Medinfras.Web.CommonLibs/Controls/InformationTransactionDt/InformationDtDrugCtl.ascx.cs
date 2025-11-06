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
    public partial class InformationDtDrugCtl : BaseUserControlCtl
    {
        public void BindGrid(List<vPatientChargesDt> lstChargesDt)
        {
            lvwDrug.DataSource = lstChargesDt;
            lvwDrug.DataBind();
            if (lstChargesDt.Count > 0)
            {
                ((HtmlTableCell)lvwDrug.FindControl("tdDrugTotalPayer")).InnerHtml = lstChargesDt.Sum(p => p.PayerAmount).ToString("N");
                ((HtmlTableCell)lvwDrug.FindControl("tdDrugTotalPatient")).InnerHtml = lstChargesDt.Sum(p => p.PatientAmount).ToString("N");
                ((HtmlTableCell)lvwDrug.FindControl("tdDrugTotal")).InnerHtml = lstChargesDt.Sum(p => p.LineAmount).ToString("N");
            }
        }
    }
}