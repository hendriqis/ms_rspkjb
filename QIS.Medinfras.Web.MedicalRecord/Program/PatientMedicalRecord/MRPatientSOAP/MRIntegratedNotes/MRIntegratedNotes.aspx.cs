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

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class MRIntegratedNotes : BasePagePatientPageList
    {
        private string pageTitle = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PATIENT_SOAP;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            ctlToolbar.SetSelectedMenu(9);
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            pageTitle = BusinessLayer.GetMenuMasterList(string.Format("MenuCode= '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            SetComboBox();
            BindGridView();
        }

        private void SetComboBox()
        {
            List<Variable> lstDisplay =
                new List<Variable>() {
                    new Variable() {Code = "Up-to-date", Value = "0" },
                    new Variable() { Code = "Outdated", Value = "1" },
                    new Variable() { Code = "All", Value = "2" }
                };
            Methods.SetComboBoxField(cboDisplay, lstDisplay, "Code", "Value");
            cboDisplay.Value = "0";
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

        private void BindGridView()
        {
            List<ConsultVisit> entityLinkedRegistration = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID IN (SELECT RegistrationID FROM Registration WITH(NOLOCK) WHERE LinkedToRegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID));

            string cvLinkedID = AppSession.RegisteredPatient.VisitID.ToString();

            foreach (ConsultVisit cv in entityLinkedRegistration)
            {
                if (cvLinkedID != "")
                {
                    cvLinkedID += ",";
                }
                cvLinkedID += cv.VisitID;
            }

            string filterExpression = string.Format("VisitID IN ({0}) ORDER BY NoteDate DESC, NoteTime DESC", cvLinkedID);
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
                                                                     IsDeleted = true
                                                                 }).ToList();
                List<vPatientVisitNote> lstCombinedEntity = lstEntityVisit.Concat(lstEntityVisitHistory).OrderBy(x => x.NoteDate).ThenBy(n => n.NoteTime).ToList();
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
                                                                     IsDeleted = true
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
                        if (entitySameDateNote.IsDeleted)
                        {
                            outdated = "<span style='color:red; margin-left:10px'>OUTDATED</span>";
                        }
                        if (entitySameDateNote.GCParamedicMasterType == Constant.ParamedicType.Nurse)
                        {
                            if (!string.IsNullOrEmpty(newEntity.NursingNote)) newEntity.NursingNote += "\n";
                            newEntity.NursingNote += string.Format("<b>{0}-{1}:{2} \n</b>", entitySameDateNote.NoteTime, entitySameDateNote.ParamedicName, outdated);
                            newEntity.NursingNote += string.Format("<div style=\"margin-left:10px;\">{0}</div>", entitySameDateNote.NoteText + "\n");
                        }
                        else if (entitySameDateNote.GCParamedicMasterType == Constant.ParamedicType.Nutritionist || entitySameDateNote.GCParamedicMasterType == Constant.ParamedicType.Pharmacist)
                        {
                            if (!string.IsNullOrEmpty(newEntity.OtherNote))
                            {
                                newEntity.OtherNote += "\n";
                            }
                            if (!string.IsNullOrEmpty(entitySameDateNote.ParamedicName))
                            {
                                newEntity.OtherNote += string.Format("<b>{0}-{1}:{2} \n</b>", entitySameDateNote.NoteTime, entitySameDateNote.ParamedicName, outdated);
                                newEntity.OtherNote += string.Format("<div style=\"margin-left:10px;\">{0}</div>", entitySameDateNote.NoteText + "\n");
                            }
                            else
                            {
                                newEntity.OtherNote += string.Format("<b>{0}-{1}:{2} \n</b>", entitySameDateNote.NoteTime, entitySameDateNote.cfCreatedByName, outdated);
                                newEntity.OtherNote += string.Format("<div style=\"margin-left:10px;\">{0}</div>", entitySameDateNote.NoteText + "\n");
                            }
                        }
                        else if (entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES || entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT)
                        {
                            if (!string.IsNullOrEmpty(newEntity.NursingNote)) newEntity.NursingNote += "\n";
                            newEntity.NursingNote += string.Format("<b>{0}-{1}:{2} \n</b>",entitySameDateNote.NoteTime, entitySameDateNote.ParamedicName,outdated);
                            newEntity.NursingNote += string.Format("<div style=\"margin-left:10px;\">{0}</div>", entitySameDateNote.NoteText + "\n");
                        }
                        else if (entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT || entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT || entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES)
                        {
                            if (!string.IsNullOrEmpty(newEntity.PhysicianNote)) newEntity.PhysicianNote += "\n";
                            newEntity.PhysicianNote += string.Format("<b>{0}-{1}:{2} \n</b>", entitySameDateNote.NoteTime, entitySameDateNote.ParamedicName, outdated);
                            newEntity.PhysicianNote += string.Format("<div style=\"margin-left:10px;\">{0}</div>", entitySameDateNote.NoteText + "\n");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(newEntity.OtherNote))
                            {
                                newEntity.OtherNote += "\n";
                            }

                            if (!string.IsNullOrEmpty(entitySameDateNote.ParamedicName))
                            {
                                newEntity.OtherNote += string.Format("<b>{0}-{1}:{2} \n</b>", entitySameDateNote.NoteTime, entitySameDateNote.ParamedicName, outdated);
                                newEntity.OtherNote += string.Format("<div style=\"margin-left:10px;\">{0}</div>", entitySameDateNote.NoteText + "\n");
                            }
                            else
                            {
                                newEntity.OtherNote += string.Format("<b>{0}-{1}:{2} \n</b>", entitySameDateNote.NoteTime, entitySameDateNote.cfCreatedByName, outdated);
                                newEntity.OtherNote += string.Format("<div style=\"margin-left:10px;\">{0}</div>", entitySameDateNote.NoteText + "\n");
                            }
                        }
                    }
                    lstIntegratedNotes.Add(newEntity);
                    lstDateTime.Add(entity.NoteDate);
                }
            }
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