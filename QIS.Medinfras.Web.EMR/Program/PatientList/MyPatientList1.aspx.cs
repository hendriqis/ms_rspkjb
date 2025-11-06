using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class MyPatientList1 : BasePageTrx
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_LIST;
        }
    }
}