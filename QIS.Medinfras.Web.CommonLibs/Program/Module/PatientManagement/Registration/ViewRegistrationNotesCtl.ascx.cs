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
    public partial class ViewRegistrationNotesCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                vConsultVisit9 entityCV = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0} AND IsMainVisit = 1", param)).FirstOrDefault();
                hdnVisitID.Value = entityCV.VisitID.ToString();
                txtMRN.ReadOnly = true;
                txtPatientName.ReadOnly = true;
                txtMRN.Text = entityCV.MedicalNo;
                txtPatientName.Text = entityCV.PatientName;

                BindGridView();
            } 
        }

        private void BindGridView()
        {
            grdVisitNotes.DataSource = BusinessLayer.GetvPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", hdnVisitID.Value, Constant.PatientVisitNotes.REGISTRATION_NOTES));
            grdVisitNotes.DataBind();
        }
    }
}