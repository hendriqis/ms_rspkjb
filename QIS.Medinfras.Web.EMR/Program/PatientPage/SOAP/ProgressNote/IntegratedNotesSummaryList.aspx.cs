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
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class IntegratedNotesSummaryList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;

        public override string OnGetMenuCode()
        {
            if (string.IsNullOrEmpty(menuType))
                menuType = Page.Request.QueryString["id"];

            switch (menuType)
            {
                case "OP":
                    return Constant.MenuCode.EMR.OUTPATIENT_SOAP_PROGRESS_NOTE_SUMMARY;
                case "MD":
                    return Constant.MenuCode.EMR.SOAP_TEMPLATE_DIAGNOSTIC_INTEGRATION_NOTE_SUMMARY;
                case "IP":
                    return Constant.MenuCode.EMR.SOAP_TEMPLATE_INPATIENT_PROGRESS_NOTE_SUMMARY;
                case "ER":
                    return Constant.MenuCode.EMR.SOAP_TEMPLATE_EMERGENCY_PROGRESS_NOTE_SUMMARY;
                case "MC":
                    return Constant.MenuCode.EMR.MCU_SOAP_PROGRESS_NOTE_SUMMARY;
                default:
                    switch (AppSession.RegisteredPatient.DepartmentID)
                    {
                        case Constant.Facility.OUTPATIENT:
                            return Constant.MenuCode.EMR.OUTPATIENT_SOAP_PROGRESS_NOTE_SUMMARY;
                        case Constant.Facility.INPATIENT:
                            return Constant.MenuCode.EMR.SOAP_TEMPLATE_INPATIENT_PROGRESS_NOTE_SUMMARY;
                        case Constant.Facility.DIAGNOSTIC:
                            return Constant.MenuCode.EMR.SOAP_TEMPLATE_DIAGNOSTIC_INTEGRATION_NOTE_SUMMARY;
                        default:
                            return Constant.MenuCode.EMR.SOAP_TEMPLATE_EMERGENCY_PROGRESS_NOTE_SUMMARY;
                    }
            }
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            vConsultVisit entityLinkedRegistration = BusinessLayer.GetvConsultVisitList(string.Format(
                        "RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})",
                        AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            if (entityLinkedRegistration != null)
            {
                hdnVisitLinkedID.Value = entityLinkedRegistration.VisitID.ToString();
            }
            else
            {
                hdnVisitLinkedID.Value = "0";
            }

            if (Page.Request.QueryString["id"] != null)
            {
                menuType = Page.Request.QueryString["id"];
            }
            SetComboBox();
            BindGridView();
        }

        private void SetComboBox()
        {
            List<Variable> lstDisplay = new List<Variable>() { new Variable() { Code = "Active Note", Value = "0" }
                , new Variable() { Code = "Including Outdated/Deleted", Value = "1" }
                , new Variable() { Code = "All", Value = "2" }};
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
            List<Registration> dataRegID = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0}) OR RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID));
            string lstReg = "";
            if (dataRegID != null)
            {
                foreach (Registration reg in dataRegID)
                {
                    if (lstReg != "")
                    {
                        lstReg += ",";
                    }
                    lstReg += reg.RegistrationID;
                }
            }

            string filterExpression = string.Format("RegistrationID IN ({0}) AND GCPatientNoteType NOT IN ('{1}', '{2}', '{3}') ORDER BY NoteDate DESC, NoteTime DESC",
                        lstReg, Constant.PatientVisitNotes.REGISTRATION_NOTES, Constant.PatientVisitNotes.REFERRAL_FROM_NOTES, Constant.PatientVisitNotes.NURSE_NOTES);
            List<ViewIntegratedNotes> lstIntegratedNotes = new List<ViewIntegratedNotes>();
            if (cboDisplay.Value.ToString() == "2")
            {
                List<vPatientVisitNote> lstEntityVisit = BusinessLayer.GetvPatientVisitNoteList(filterExpression);
                List<vPatientVisitNoteHistory> lstEntityHistory = BusinessLayer.GetvPatientVisitNoteHistoryList(filterExpression);
                List<vPatientVisitNote> lstEntityVisitHistory = (from a in lstEntityHistory
                                                                 select new vPatientVisitNote()
                                                                 {
                                                                     ParamedicName = a.ParamedicName,
                                                                     GCParamedicMasterType = a.GCParamedicMasterType,
                                                                     ParamedicMasterType = a.ParamedicMasterType,
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
                                                                     GCParamedicMasterType = a.GCParamedicMasterType,
                                                                     ParamedicMasterType = a.ParamedicMasterType,
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
            //SetDataForBinding(lstIntegratedNotes, lstEntity);
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
                        string paramedicFullInfo = "";

                        if (entitySameDateNote.GCParamedicMasterType != Constant.ParamedicType.Physician)
                        {
                            if (!string.IsNullOrEmpty(paramedicName))
                            {
                                if (!string.IsNullOrEmpty(entitySameDateNote.ServiceUnitName))
                                {
                                    paramedicFullInfo = string.Format("{0}|{1} (<i>{2}</i>)", entitySameDateNote.ParamedicName, entitySameDateNote.ParamedicMasterType, entitySameDateNote.ServiceUnitName);
                                }
                                else
                                {
                                    paramedicFullInfo = string.Format("{0}|{1}", entitySameDateNote.ParamedicName, entitySameDateNote.ParamedicMasterType);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(entitySameDateNote.ServiceUnitName))
                                {
                                    paramedicFullInfo = string.Format("{0}|{1} (<i>{2}</i>)", entitySameDateNote.CreatedByName, entitySameDateNote.ParamedicMasterType, entitySameDateNote.ServiceUnitName);
                                }
                                else
                                {
                                    paramedicFullInfo = string.Format("{0}|{1}", entitySameDateNote.CreatedByName, entitySameDateNote.ParamedicMasterType);
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(paramedicName))
                            {
                                if (!string.IsNullOrEmpty(entitySameDateNote.ServiceUnitName))
                                {
                                    paramedicFullInfo = string.Format("{0}(<i>{1}</i>)", entitySameDateNote.ParamedicName, entitySameDateNote.ServiceUnitName);
                                }
                                else
                                {
                                    paramedicFullInfo = entitySameDateNote.ParamedicName;
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(entitySameDateNote.ServiceUnitName))
                                {
                                    paramedicFullInfo = string.Format("{0}(<i>{1}</i>)", entitySameDateNote.CreatedByName, entitySameDateNote.ServiceUnitName);
                                }
                                else
                                {
                                    paramedicFullInfo = entitySameDateNote.CreatedByName;
                                }
                            }
                        }

                        string notes = string.Format("<b>{0}-{1}:{2} {3} \n</b>", entitySameDateNote.NoteTime, paramedicFullInfo, outdated, confirmationInfo);
                        notes += string.Format("<div style=\"margin-left:10px;\">{0}</div>", entitySameDateNote.cfNoteSOAPI + "\n");
                        if (entitySameDateNote.GCParamedicMasterType != Constant.ParamedicType.Physician && entitySameDateNote.GCParamedicMasterType != Constant.ParamedicType.Nurse && entitySameDateNote.GCParamedicMasterType != Constant.ParamedicType.Bidan)
                        {
                            if (!string.IsNullOrEmpty(newEntity.OtherNote)) newEntity.OtherNote += "\n";
                            newEntity.OtherNote += notes;
                        }
                        else if (entitySameDateNote.GCParamedicMasterType == Constant.ParamedicType.Physician)
                        {
                            if (!string.IsNullOrEmpty(newEntity.PhysicianNote)) newEntity.PhysicianNote += "\n";
                            newEntity.PhysicianNote += notes;
                        }
                        else if (entitySameDateNote.GCParamedicMasterType == Constant.ParamedicType.Nurse || entitySameDateNote.GCParamedicMasterType == Constant.ParamedicType.Bidan)
                        {
                            if (!string.IsNullOrEmpty(newEntity.NursingNote)) newEntity.NursingNote += "\n";
                            newEntity.NursingNote += notes;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(newEntity.OtherNote)) newEntity.OtherNote += "\n";
                            newEntity.OtherNote += notes;
                        }

                        newEntity.IsNeedConfirmation = entity.IsNeedConfirmation;
                        newEntity.ConfirmationPhysicianID = entity.ConfirmationPhysicianID;
                        newEntity.ConfirmationPhysicianCode = entity.ConfirmationPhysicianCode;
                        newEntity.ConfirmationPhysicianName = entity.ConfirmationPhysicianName;
                        newEntity.IsConfirmed = entity.IsConfirmed;
                        newEntity.ConfirmationDateTime = entity.ConfirmationDateTime;
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

            public Boolean IsNeedConfirmation { get; set; }
            public Int32 ConfirmationPhysicianID { get; set; }
            public string ConfirmationPhysicianCode { get; set; }
            public string ConfirmationPhysicianName { get; set; }
            public Boolean IsConfirmed { get; set; }
            public DateTime ConfirmationDateTime { get; set; }

        }
    }
}