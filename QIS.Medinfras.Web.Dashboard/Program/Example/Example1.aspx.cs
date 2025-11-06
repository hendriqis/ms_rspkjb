using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Dashboard.Program
{
    public partial class Example1 : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Dashboard.Example1;
        }

        protected override void InitializeDataControl()
        {
            ((KunjunganCtl)ctlKunjungan).InitializeControl("");
        }     
    }
}