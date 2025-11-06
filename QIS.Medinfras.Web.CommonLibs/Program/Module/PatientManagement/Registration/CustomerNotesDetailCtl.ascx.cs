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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CustomerNotesDetailCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            BusinessPartners entity = BusinessLayer.GetBusinessPartners(Convert.ToInt32(param));
            divCustomerNotes.InnerHtml = entity.Remarks;            
        }
    }
}