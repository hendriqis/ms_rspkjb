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
    public partial class ViewPrescriptionOrderChangesLogCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnPrescriptionOrderID.Value = param;
                BindGridView();
            } 
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvPrescriptionOrderChangesLogList(string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", hdnPrescriptionOrderID.Value));
            grdView.DataBind();
        }

    }
}