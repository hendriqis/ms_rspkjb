using System;
using System.ComponentModel;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class EpisodeSummaryIntegratedNotesRptNew : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeSummaryIntegratedNotesRptNew()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            //string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY NoteDate, NoteTime", VisitID);
            //List<vPatientVisitNote> lstEntity = BusinessLayer.GetvPatientVisitNoteList(filterExpression);
            //lstEntity = lstEntity.Where(t => !string.IsNullOrEmpty(t.NoteText)).ToList();
            //List<ViewIntegratedNotes> lstIntegratedNotes = new List<ViewIntegratedNotes>();
            //SetDataForBinding(lstIntegratedNotes, lstEntity);

            //vConsultVisit entityLinkedRegistration = BusinessLayer.GetvConsultVisitList(string.Format(
            //    "RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", 
            //    AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();

            vConsultVisit entityLinkedRegistration = BusinessLayer.GetvConsultVisitList(string.Format(
                    "VisitID = {0}", VisitID)).FirstOrDefault();
            int cvLinkedID = 0;
            if (entityLinkedRegistration.LinkedRegistrationID != null)
            {
                cvLinkedID = entityLinkedRegistration.LinkedRegistrationID;
            }

            string filterExpression = string.Format(
                "VisitID IN ({0},{1}) AND GCPatientNoteType IN ('{2}', '{3}', '{4}', '{5}', '{6}') ORDER BY NoteDate DESC, NoteTime DESC",
               VisitID, cvLinkedID, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.FOLLOWUP_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES);
            List<ViewIntegratedNotes> lstIntegratedNotes = new List<ViewIntegratedNotes>();

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

            this.DataSource = lstIntegratedNotes;

        }

        private void SetDataForBinding(List<ViewIntegratedNotes> lstIntegratedNotes, List<vPatientVisitNote> lstEntity)
        {
            //List<TempDateTime> lstTemp = new List<TempDateTime>();
            //foreach (vPatientVisitNote entity in lstEntity)
            //{
            //    if (lstTemp.Where(t => t.Date == entity.cfNoteDate.ToString() && t.Time == entity.NoteTime.ToString()).Count() == 0)
            //    {
            //        ViewIntegratedNotes newEntity = new ViewIntegratedNotes();
            //        newEntity.NoteDate = entity.NoteDate.ToString(Constant.FormatString.DATE_FORMAT);
            //        newEntity.NoteTime = entity.NoteTime;
            //        if (lstTemp.Where(t => t.Date == newEntity.NoteDate).Count() > 0)
            //        {
            //            newEntity.FirstDate = false;
            //        }
            //        else
            //        {
            //            newEntity.FirstDate = true;
            //        }
            //        IEnumerable<vPatientVisitNote> lstSameTimeNotes = lstEntity.Where(t => t.cfNoteDate == newEntity.NoteDate && t.NoteTime == newEntity.NoteTime);
            //        foreach (vPatientVisitNote entitySameTimeNotes in lstSameTimeNotes)
            //        {
            //            if (entitySameTimeNotes.GCPatientNoteType == Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES)
            //            {
            //                if (!string.IsNullOrEmpty(newEntity.PhysicianNote)) newEntity.PhysicianNote += "\r\n";

            //                newEntity.PhysicianNote += entitySameTimeNotes.ParamedicName + "\r\n" + entitySameTimeNotes.NoteText;
            //            }
            //            else if (entitySameTimeNotes.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES)
            //            {
            //                if (!string.IsNullOrEmpty(newEntity.NursingNote)) newEntity.NursingNote += "\r\n";

            //                newEntity.NursingNote += entitySameTimeNotes.ParamedicName + "\r\n" + entitySameTimeNotes.NoteText;
            //            }
            //            else
            //            {
            //                if (!string.IsNullOrEmpty(newEntity.OthersNote)) newEntity.OthersNote += "\r\n";

            //                newEntity.OthersNote += entitySameTimeNotes.ParamedicName + "\r\n" + entitySameTimeNotes.NoteText;
            //            }
            //        }
            //        TempDateTime newTempDateTime = new TempDateTime();
            //        newTempDateTime.Date = newEntity.NoteDate;
            //        newTempDateTime.Time = newEntity.NoteTime;
            //        lstIntegratedNotes.Add(newEntity);
            //        lstTemp.Add(newTempDateTime);
            //    }
            //}

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
                            outdated = "OUTDATED";
                        }
                        string paramedicName = entitySameDateNote.ParamedicName;
                        if (string.IsNullOrEmpty(paramedicName)) paramedicName = entitySameDateNote.CreatedByName;
                        string notes = string.Format("{0} - {1} ({3}) : {2}\r\n", entitySameDateNote.NoteTime, paramedicName, outdated, entitySameDateNote.ServiceUnitName);
                        notes += string.Format("{0}", entitySameDateNote.NoteText + "\r\n");
                        if (entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES || entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.FOLLOWUP_NOTES)
                        {
                            if (!string.IsNullOrEmpty(newEntity.NursingNote)) newEntity.NursingNote += "\r\n";
                            newEntity.NursingNote += notes;
                        }
                        else if (entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES)
                        {
                            if (!string.IsNullOrEmpty(newEntity.PhysicianNote)) newEntity.PhysicianNote += "\r\n";
                            newEntity.PhysicianNote += notes;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(newEntity.OthersNote)) newEntity.OthersNote += "\r\n";
                            newEntity.OthersNote += notes;
                        }
                    }
                    lstIntegratedNotes.Add(newEntity);
                    lstDateTime.Add(entity.NoteDate);
                }
            }
        }

        private class ViewIntegratedNotes
        {
            private bool _FirstDate;

            public bool FirstDate
            {
                get { return _FirstDate; }
                set { _FirstDate = value; }
            }

            private String _NoteDate;

            public String NoteDate
            {
                get { return _NoteDate; }
                set { _NoteDate = value; }
            }

            private String _NoteTime;

            public String NoteTime
            {
                get { return _NoteTime; }
                set { _NoteTime = value; }
            }

            private String _PhysicianNote;

            public String PhysicianNote
            {
                get { return _PhysicianNote; }
                set { _PhysicianNote = value; }
            }

            private String _NursingNote;

            public String NursingNote
            {
                get { return _NursingNote; }
                set { _NursingNote = value; }
            }

            private String _OthersNote;

            public String OthersNote
            {
                get { return _OthersNote; }
                set { _OthersNote = value; }
            }

            private String _Remarks;

            public String Remarks
            {
                get { return _Remarks; }
                set { _Remarks = value; }
            }
        }

        private class TempDateTime
        {
            private string _Date;

            public string Date
            {
                get { return _Date; }
                set { _Date = value; }
            }

            private string _Time;

            public string Time
            {
                get { return _Time; }
                set { _Time = value; }
            }

        }
    }
}
