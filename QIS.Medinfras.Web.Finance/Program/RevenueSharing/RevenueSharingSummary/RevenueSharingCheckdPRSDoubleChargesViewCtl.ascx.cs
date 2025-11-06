using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingCheckdPRSDoubleChargesViewCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            BindGridView();
        }

        protected void cbpViewDoubleChargesPopUpCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView() 
        {
            List<GetPRSCheckCountDataDoubleDetail> lstEntity = BusinessLayer.GetPRSCheckCountDataDoubleDetailList(AppSession.ParamedicID);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }
    }
}