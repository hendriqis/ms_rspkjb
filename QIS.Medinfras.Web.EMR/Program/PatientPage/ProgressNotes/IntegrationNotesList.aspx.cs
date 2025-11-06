using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class IntegrationNotesList : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.INTEGRATION_NOTES;           
        }

        #region List
        protected override void InitializeDataControl()
        {
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            SetComboBox();
            BindGridView();
        }

        private void BindGridView()
        {
            vConsultVisit entityLinkedRegistration = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            int cvLinkedID = 0;
            if (entityLinkedRegistration != null)
            {
                cvLinkedID = entityLinkedRegistration.VisitID;
            }
            string filterExpression = string.Format("VisitID IN ({0},{1}) AND GCPatientNoteType IN ('{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}') ORDER BY NoteDate DESC, NoteTime DESC", AppSession.RegisteredPatient.VisitID, cvLinkedID, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.FOLLOWUP_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES, Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT);
            List<ViewIntegratedNotes> lstIntegratedNotes = new List<ViewIntegratedNotes>();
            if (cboDisplay.Value.ToString() == "2")
            {
                List<vPatientVisitNote> lstEntityVisit = BusinessLayer.GetvPatientVisitNoteList(filterExpression);
                List<vPatientVisitNoteHistory> lstEntityHistory = BusinessLayer.GetvPatientVisitNoteHistoryList(filterExpression);
                List<vPatientVisitNote> lstEntityVisitHistory = (from a in lstEntityHistory
                                                                 select new vPatientVisitNote()
                                                                 {
                                                                     ParamedicName = a.ParamedicName,
                                                                     NoteDate = a.NoteDate,
                                                                     NoteTime = a.NoteTime,
                                                                     DepartmentID = a.DepartmentID,
                                                                     NoteText = a.NoteText,
                                                                     GCPatientNoteType = a.GCPatientNoteType,
                                                                     IsDeleted = true,
                                                                     ServiceUnitName = a.ServiceUnitName,
                                                                     CreatedByName = a.CreatedByName
                                                                 }).ToList();
                List<vPatientVisitNote> lstCombinedEntity = lstEntityVisit.Concat(lstEntityVisitHistory).OrderByDescending(x => x.NoteDate).ThenByDescending(n => n.NoteTime).ToList();
                SetDataForBinding(lstIntegratedNotes, lstCombinedEntity);
            }
            else if (cboDisplay.Value.ToString() == "1")
            {
                List<vPatientVisitNoteHistory> lstEntityHistory = BusinessLayer.GetvPatientVisitNoteHistoryList(filterExpression);
                List<vPatientVisitNote> lstEntityVisitHistory = (from a in lstEntityHistory
                                                                 select new vPatientVisitNote()
                                                                 {
                                                                     ParamedicName = a.ParamedicName,
                                                                     NoteDate = a.NoteDate,
                                                                     NoteTime = a.NoteTime,
                                                                     DepartmentID = a.DepartmentID,
                                                                     NoteText = a.NoteText,
                                                                     GCPatientNoteType = a.GCPatientNoteType,
                                                                     IsDeleted = true,
                                                                     ServiceUnitName = a.ServiceUnitName,
                                                                     CreatedByName = a.CreatedByName
                                                                 }).ToList();
                SetDataForBinding(lstIntegratedNotes, lstEntityVisitHistory);
            }
            else
            {
                List<vPatientVisitNote> lstEntityVisit = BusinessLayer.GetvPatientVisitNoteList(filterExpression);
                SetDataForBinding(lstIntegratedNotes, lstEntityVisit);
            }
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
                        string confirmationInfo = string.Empty;

                        if (entitySameDateNote.IsNeedConfirmation)
                        {
                            if (!entitySameDateNote.IsConfirmed)
                            {
                                confirmationInfo = string.Format("<span style = 'color:red'>Need Confirmation : {0}</span>", entitySameDateNote.ConfirmationPhysicianName);
                            }
                            else
                            {
                                confirmationInfo = string.Format("<span style = 'color:red'>{0}</span>", entitySameDateNote.cfConfirmationInfo);
                            }
                        }

                        if (entitySameDateNote.IsDeleted)
                        {
                            outdated = "<span style='color:red; margin-left:10px'>OUTDATED</span>";
                        }
                        string paramedicName = entitySameDateNote.ParamedicName;
                        if (string.IsNullOrEmpty(paramedicName)) paramedicName = entitySameDateNote.CreatedByName;
                        string notes = string.Format("<b>{0}-{1}(<i>{3}</i>):{2} {4} {5} \n</b>", entitySameDateNote.NoteTime, paramedicName, outdated, entitySameDateNote.ServiceUnitName, entitySameDateNote.cfParamedicMasterType, confirmationInfo);
                        notes += string.Format("<div style=\"margin-left:10px;\">{0}</div>", entitySameDateNote.NoteText + "\n");
                        if (entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES || entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.FOLLOWUP_NOTES)
                        {
                            if (!string.IsNullOrEmpty(newEntity.NursingNote)) newEntity.NursingNote += "\n";
                            newEntity.NursingNote += notes;
                        }
                        else if ((entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES) ||
                            (entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT) ||
                            (entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT))
                        {
                            if (!string.IsNullOrEmpty(newEntity.PhysicianNote)) newEntity.PhysicianNote += "\n";
                            newEntity.PhysicianNote += notes;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(newEntity.OtherNote)) newEntity.OtherNote += "\n";
                            newEntity.OtherNote += notes;
                        }
                    }
                    lstIntegratedNotes.Add(newEntity);
                    lstDateTime.Add(entity.NoteDate);
                }
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #endregion

        private void SetComboBox()
        {
            List<Variable> lstDisplay = new List<Variable>() { new Variable() { Code = "Up-to-date", Value = "0" }
                , new Variable() { Code = "Outdated", Value = "1" }
                , new Variable() { Code = "All", Value = "2" }};
            Methods.SetComboBoxField(cboDisplay, lstDisplay, "Code", "Value");
            cboDisplay.Value = "0";
        }

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

            private String _OtherNote;

            public String OtherNote
            {
                get { return _OtherNote; }
                set { _OtherNote = value; }
            }

            private String _Remarks;

            public String Remarks
            {
                get { return _Remarks; }
                set { _Remarks = value; }
            }
        }
    }
}