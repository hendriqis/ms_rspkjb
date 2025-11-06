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
    public partial class CustomerContractNotesViewCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramID = param.Split('|');
            String paramPayerID = paramID[0].ToString();
            String paramContractID = paramID[1].ToString();

            if (paramPayerID != "1")
            {
                List<vDocumentLog> lst = BusinessLayer.GetvDocumentLogList(string.Format("DocumentID = {0} AND IsDeleted = 0 ORDER BY LogDate DESC", paramContractID));
                rptView.DataSource = lst;
                rptView.DataBind();
            }
        }

    }
}