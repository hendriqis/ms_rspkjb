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
    public partial class ViewNotesCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", param)).FirstOrDefault();
                hdnVisitID.Value = entityCV.VisitID.ToString();
                BindGridView();
            } 
        }

        private void BindGridView()
        {
            string filter = string.Format("VisitID = {0} AND IsDeleted = 0 AND GCPatientNoteType = '{1}'", hdnVisitID.Value, Constant.PatientVisitNotes.REGISTRATION_NOTES);
            grdView.DataSource = BusinessLayer.GetvPatientVisitNoteList(filter, int.MaxValue, 1, "NoteDate DESC, NoteTime DESC");
            grdView.DataBind();
        }

    }
}