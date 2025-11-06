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
    public partial class OutpatientIntegrationNotes : BasePage
    {
        protected int PageCount = 1;

        #region List

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
                SetComboBox();
                BindGridView();
                BindGridViewDetail(1, true, ref PageCount);
            }
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("GCPatientNoteType IN ('{0}', '{1}', '{2}', '{3}','{4}')", Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.FOLLOWUP_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT);
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
                        if (entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES || entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.FOLLOWUP_NOTES || entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT)
                        {
                            if (!string.IsNullOrEmpty(newEntity.NursingNote)) newEntity.NursingNote += "\n";
                            newEntity.NursingNote += notes;
                        }
                        else if ((entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES) ||
                            (entitySameDateNote.GCPatientNoteType == Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT))
                        {
                            if (!string.IsNullOrEmpty(newEntity.PhysicianNote)) newEntity.PhysicianNote += "\n";
                            newEntity.PhysicianNote += notes;

                            if (entitySameDateNote.InstructionText != null && entitySameDateNote.InstructionText != "")
                            {
                                newEntity.PhysicianNote += "\n";
                                newEntity.PhysicianNote += string.Format("<div style=\"margin-left:10px;\">I:</div>\n");
                                newEntity.PhysicianNote += string.Format("<div style=\"margin-left:10px;\">{0}</div>", entitySameDateNote.InstructionText + "\n");
                            }
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

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDetail(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDetail(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewDetail(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<GetVisitHistoryListByMRN> lstEntity = BusinessLayer.GetVisitHistoryListByMRN(AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.VisitID);
            List<GetVisitHistoryListByMRN> lstData;
            if (Convert.ToInt32(cboDisplayHistory.Value) == 0)
            {
                lstData = lstEntity.OrderByDescending(t => t.VisitDate).ToList();
            }
            else
            {
                lstData = lstEntity.Where(x => x.HealthcareServiceUnitID == Convert.ToInt32(cboDisplayHistory.Value)).ToList().OrderByDescending(t => t.VisitDate).ToList();
            }
            grdViewDetail.DataSource = lstData;
            grdViewDetail.DataBind();
        }

        protected void grdViewDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GetVisitHistoryListByMRN obj = (GetVisitHistoryListByMRN)e.Row.DataItem;
                Repeater rptTestOrderDt = (Repeater)e.Row.FindControl("rptTestOrderDt");
                Repeater rptPrescription = (Repeater)e.Row.FindControl("rptPrescription");
                Repeater rptDiagnose = (Repeater)e.Row.FindControl("rptDiagnose");
                Repeater rptVitalSignHd = (Repeater)e.Row.FindControl("rptVitalSignHd");

                List<GetListItemForPatientObsygnHistoryList> lstData = BusinessLayer.GetListItemForPatientObsygnHistoryList(obj.VisitID);
                List<GetListItemForPatientObsygnHistoryList> lstTestOrder = lstData.Where(t => t.Jenis == "TestOrder").ToList();
                List<GetListItemForPatientObsygnHistoryList> lstPrescription = lstData.Where(t => t.Jenis == "Prescription").ToList();

                string filterDiagnose = string.Format("VisitID = {0} AND IsDeleted = 0", obj.VisitID);
                List<vPatientDiagnosis> lstDiagnose = BusinessLayer.GetvPatientDiagnosisList(filterDiagnose);

                rptTestOrderDt.DataSource = lstTestOrder;
                rptTestOrderDt.DataBind();

                rptPrescription.DataSource = lstPrescription;
                rptPrescription.DataBind();

                rptDiagnose.DataSource = lstDiagnose;
                rptDiagnose.DataBind();

                string filterVitalSignHd = string.Format("VisitID = {0} AND IsDeleted = 0", obj.VisitID);
                List<vVitalSignHd> lstVitalSignHd = BusinessLayer.GetvVitalSignHdList(filterVitalSignHd);
                rptVitalSignHd.DataSource = lstVitalSignHd;
                rptVitalSignHd.DataBind();
            }
        }

        protected void rptVitalSignHd_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Item.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Item.FindControl("rptVitalSignDt");

                string filterVitalSignDt = string.Format("ID = '{0}'", obj.ID);
                List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(filterVitalSignDt);

                rptVitalSignDt.DataSource = lstVitalSignDt;
                rptVitalSignDt.DataBind();
            }
        }

        private void SetComboBox()
        {
            List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vConsultVisit1 WHERE MRN = {0} AND DepartmentID NOT IN ('{1}','{2}','{3}')) AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, Constant.Facility.EMERGENCY, Constant.Facility.INPATIENT, Constant.Facility.PHARMACY));
            lstHSU.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
            Methods.SetComboBoxField(cboDisplay, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
            cboDisplay.Value = "0";
            Methods.SetComboBoxField(cboDisplayHistory, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
            cboDisplayHistory.Value = "0";
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