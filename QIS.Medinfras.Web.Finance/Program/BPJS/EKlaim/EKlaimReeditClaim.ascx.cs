using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class EKlaimReeditClaim : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnIsBridgingToEKlaim.Value = AppSession.IsBridgingToEKlaim ? "1" : "0";

            hdnParam.Value = param;

            hdnSEPNo.Value = hdnParam.Value;
            txtSEPNo.Text = hdnParam.Value;
        }
    }
}