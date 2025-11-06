using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class EpisodeSummaryNotesCtl : BaseViewPopupCtl
    {
        private class ViewIntegratedNotes
        {
            private String _NoteDate;

            public String NoteDate
            {
                get { return _NoteDate; }
                set { _NoteDate = value; }
            }

            private String _NursingNote;

            public String NursingNote
            {
                get { return _NursingNote; }
                set { _NursingNote = value; }
            }

            private String _PhysicianNote;

            public String PhysicianNote
            {
                get { return _PhysicianNote; }
                set { _PhysicianNote = value; }
            }

            private String _Remarks;

            public String Remarks
            {
                get { return _Remarks; }
                set { _Remarks = value; }
            }
        }

        public override void InitializeDataControl(string queryString)
        {
            BindGridView(Convert.ToInt32(queryString));
        }

        private void BindGridView(int visitID)
        {
            vConsultVisit entityLinkedRegistration = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", visitID)).FirstOrDefault();
            int cvLinkedID = 0;
            if (entityLinkedRegistration != null)
            {
                cvLinkedID = entityLinkedRegistration.VisitID;
            }
            string filterExpression = string.Format("VisitID IN ({0},{1}) AND GCPatientNoteType IN ('{2}', '{3}', '{4}') ORDER BY NoteDate DESC, NoteTime DESC", visitID, cvLinkedID, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.FOLLOWUP_NOTES);
            List<ViewIntegratedNotes> lstIntegratedNotes = new List<ViewIntegratedNotes>();

            List<vPatientVisitNote> lstEntityVisit = BusinessLayer.GetvPatientVisitNoteList(filterExpression);
            SetDataForBinding(lstIntegratedNotes, lstEntityVisit);

            grdView.DataSource = lstIntegratedNotes;
            grdView.DataBind();
        }

        private void SetDataForBinding(List<ViewIntegratedNotes> lstIntegratedNotes, List<vPatientVisitNote> lstEntity)
        {
            List<DateTime> lstDateTime = new List<DateTime>();
            foreach (vPatientVisitNote entity in lstEntity)
            {
                if (lstDateTime.Where(t => t.Date == entity.NoteDate).Count() == 0)
                {
                    ViewIntegratedNotes newEntity = new ViewIntegratedNotes();
                    newEntity.NoteDate = entity.cfNoteDate;
                    IEnumerable<vPatientVisitNote> lstSameDateNotes = lstEntity.Where(t => t.cfNoteDate == newEntity.NoteDate);
                    foreach (vPatientVisitNote entitySameDateNote in lstSameDateNotes)
                    {
                        string outdated = string.Empty;
                        if (entitySameDateNote.IsDeleted)
                        {
                            outdated = "<span style='color:red; margin-left:10px'>OUTDATED</span>";
                        }
                        if (entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES || entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.FOLLOWUP_NOTES)
                        {
                            if (!string.IsNullOrEmpty(newEntity.NursingNote)) newEntity.NursingNote += "\n";
                            newEntity.NursingNote += string.Format("<b>{0}-{1}:{2} \n</b>", entitySameDateNote.NoteTime, entitySameDateNote.ParamedicName, outdated);
                            newEntity.NursingNote += string.Format("<div style=\"margin-left:10px;\">{0}</div>", entitySameDateNote.NoteText + "\n");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(newEntity.PhysicianNote)) newEntity.PhysicianNote += "\n";
                            newEntity.PhysicianNote += string.Format("<b>{0}-{1}:{2} \n</b>", entitySameDateNote.NoteTime, entitySameDateNote.ParamedicName, outdated);
                            newEntity.PhysicianNote += string.Format("<div style=\"margin-left:10px;\">{0}</div>", entitySameDateNote.NoteText + "\n");
                        }
                    }
                    lstIntegratedNotes.Add(newEntity);
                    lstDateTime.Add(entity.NoteDate);
                }
            }
        }
    }
}