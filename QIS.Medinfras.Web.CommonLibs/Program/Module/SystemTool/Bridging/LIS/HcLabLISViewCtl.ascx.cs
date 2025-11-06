using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class HcLabLISViewCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (!String.IsNullOrEmpty(param))
            {
                string filter = string.Format("TransactionID = {0} AND IsResultAvailable = 1 AND MessageText IS NOT NULL", param);
                BridgingStatus log = BusinessLayer.GetBridgingStatusList(filter).LastOrDefault();
                if (log != null)
                {
                    if (!String.IsNullOrEmpty(log.MessageText))
                    {
                        txtResult.Text = log.cfMessageText;
                    }
                    else
                    {
                        txtResult.Text = "Tidak ada informasi";
                    }
                }
                else
                {
                    txtResult.Text = "Tidak ada informasi";
                }
            }
            else
            {
                txtResult.Text = "Tidak ada informasi";
            }
        }
    }
}