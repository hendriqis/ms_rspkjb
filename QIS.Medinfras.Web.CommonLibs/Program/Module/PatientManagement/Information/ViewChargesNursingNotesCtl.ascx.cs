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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewChargesNursingNotesCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                hdnVisitID.Value = paramInfo[0];
                hdnTransactionID.Value = paramInfo[1];
                BindGridView();
            } 
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvNursingJournalList(string.Format("VisitID = {0} AND ChargeTransactionID = {1} AND IsDeleted = 0", hdnVisitID.Value, hdnTransactionID.Value));
            grdView.DataBind();
        }

    }
}