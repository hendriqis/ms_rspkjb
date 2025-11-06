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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class OutpatientIntegrationNotes : BasePagePatientPageList
    {
        private string pageTitle = string.Empty;
        protected int PageCount = 1;
        string menuType = string.Empty;

        public override string OnGetMenuCode()
        {
            if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                return Constant.MenuCode.EmergencyCare.INTEGRATED_NOTE;
            else if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                if (menuType == "fo")
                    return Constant.MenuCode.Inpatient.FOLLOWUP_INTEGRATED_NOTE;
                else
                    return Constant.MenuCode.Inpatient.PATIENT_PAGE_INTEGRATED_NOTE_DISPLAY_BY_DATE;
            }
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                if (menuType == "tr")
                    return Constant.MenuCode.MedicalDiagnostic.INTEGRATED_NOTE_TRANSACTION;
                else if (menuType == "fo")
                    return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES;
                else
                    return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES;
            }
            else if (hdnDepartmentID.Value == Constant.Facility.PHARMACY)
            {
                return Constant.MenuCode.Pharmacy.PHARMACY_INTEGRATION_NOTES;
            }
            else if (hdnDepartmentID.Value == Constant.Facility.LABORATORY)
            {
                return Constant.MenuCode.Laboratory.PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES;
            }
            else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                if (menuType == "dp")
                {
                    if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    {
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES;
                    }
                    else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                    {
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES;
                    }
                    else
                    {
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES;
                    }
                }
                else if (menuType == "fo")
                    return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES;
                else
                    return Constant.MenuCode.Outpatient.PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES;
            }
            else
                return Constant.MenuCode.Outpatient.PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected string GetPageTitle()
        {
            return pageTitle;
        }

        protected override void InitializeDataControl()
        {
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
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
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnDepartmentID.Value = param[0];
                if (param.Length > 1)
                {
                    menuType = param[1];
                }
            }
            pageTitle = BusinessLayer.GetMenuMasterList(string.Format("MenuCode= '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            SetComboBox();
            BindGridView();
        }

        private void SetComboBox()
        {
            List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vConsultVisit1 WHERE MRN = {0} AND DepartmentID NOT IN ('{1}','{2}','{3}')) AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, Constant.Facility.EMERGENCY, Constant.Facility.INPATIENT, Constant.Facility.PHARMACY));
            lstHSU.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
            Methods.SetComboBoxField(cboDisplay, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
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
            string filterExpression = string.Format("GCPatientNoteType IN ('{0}', '{1}', '{2}', '{3}', '{4}')", Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.FOLLOWUP_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES);
            List<ViewIntegratedNotes> lstIntegratedNotes = new List<ViewIntegratedNotes>();
            if (cboDisplay.Value.ToString() != "0")
                filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM vConsultVisit1 WHERE MRN = {0} AND HealthcareServiceUnitID = {1} AND GCVisitStatus != '{1}')", AppSession.RegisteredPatient.MRN, cboDisplay.Value, Constant.VisitStatus.CANCELLED);
            else
                filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM vConsultVisit1 WHERE MRN = {0} AND DepartmentID NOT IN ('{1}','{2}','{3}') AND GCVisitStatus != '{4}') ", AppSession.RegisteredPatient.MRN, Constant.Facility.EMERGENCY, Constant.Facility.PHARMACY, Constant.Facility.INPATIENT, Constant.VisitStatus.CANCELLED);

            filterExpression += " ORDER BY NoteDate DESC, NoteTime DESC";

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
                        string notes = string.Format("<b>{0} - <span style='color: blue'>{1}</span> (<i>{3}</i>) :{2} {4} {5} \n</b>", entitySameDateNote.NoteTime, paramedicName, outdated, entitySameDateNote.ServiceUnitName, entitySameDateNote.cfParamedicMasterType, confirmationInfo);
                        notes += string.Format("<div style=\"margin-left:10px;\">{0}</div>", entitySameDateNote.NoteText + "\n");
                        if (entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES || entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.FOLLOWUP_NOTES)
                        {
                            if (!string.IsNullOrEmpty(newEntity.NursingNote)) newEntity.NursingNote += "\n";
                            newEntity.NursingNote += notes;
                        }
                        else if ((entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES) ||
                            (entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT))
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