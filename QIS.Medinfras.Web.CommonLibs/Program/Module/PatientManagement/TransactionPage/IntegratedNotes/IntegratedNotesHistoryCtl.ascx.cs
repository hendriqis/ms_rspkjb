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
    public partial class IntegratedNotesHistoryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                BindGridView(param);
            } 
        }

        private void BindGridView(string NoteID)
        {
            List<vPatientVisitNoteHistory> lstEntity = BusinessLayer.GetvPatientVisitNoteHistoryList(string.Format("ID = {0}", NoteID));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

    }
}