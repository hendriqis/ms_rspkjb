using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ERChiefComplaint : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EmergencyCare.PHYSICAL_EXAMINATION;
        }

        protected override void InitializeDataControl()
        {
        }
    }
}