using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class CustomerContractSummaryViewCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            CustomerContract entity = BusinessLayer.GetCustomerContract(Convert.ToInt32(param));
            divContractSummary.InnerHtml = entity.ContractSummary;            
        }
    }
}