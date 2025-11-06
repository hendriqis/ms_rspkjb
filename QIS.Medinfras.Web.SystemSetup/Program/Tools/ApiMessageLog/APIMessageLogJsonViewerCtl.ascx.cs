using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Text;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class APIMessageLogJsonViewerCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnID.Value = param;
            APIMessageLog entity = BusinessLayer.GetAPIMessageLogList(string.Format("ID = {0}", hdnID.Value))[0];
        }
    }
}