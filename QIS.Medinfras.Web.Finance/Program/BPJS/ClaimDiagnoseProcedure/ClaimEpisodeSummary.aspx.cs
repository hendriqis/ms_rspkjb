using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ClaimEpisodeSummary : BasePagePatientPageList
    {
        public string chartData = "";
        private List<vTestOrderHd> lstTestOrderHd = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        protected List<vVitalSignDt> lstVitalSignDt = null;

        class SubjectiveContent
        {
            public string HistoryDate { get; set; }
            public string HistoryTime { get; set; }
            public string ParamedicName { get; set; }
            public string ChiefComplaintText { get; set; }
        }

        class DiagnosisInfo
        {
            public string DiagnosisDate { get; set; }
            public string DiagnosisTime { get; set; }
            public string PhysicianName { get; set; }
            public string DiagnosisText { get; set; }
            public string DiagnosisType { get; set; }
        }

        class LaboratoryHdInfo
        {
            public int ChargesID { get; set; }
            public string TestDate { get; set; }
            public string TestTime { get; set; }
            public string PhysicianName { get; set; }
            public int ItemID { get; set; }
            public string ItemName { get; set; }
        }

        class LaboratoryDtInfo
        {
            public int ChargesID { get; set; }
            public int ItemID { get; set; }
            public string FractionName { get; set; }
            public string ResultValue { get; set; }
            public string ResultUnit { get; set; }
            public string RefRange { get; set; }
            public string ResultFlag { get; set; }
        }

        class ImagingHdInfo
        {
            public int ChargesID { get; set; }
            public string TestDate { get; set; }
            public string TestTime { get; set; }
            public string PhysicianName { get; set; }
            public int ItemID { get; set; }
            public string ItemName { get; set; }
        }

        class ImagingDtInfo
        {
            public int ChargesID { get; set; }
            public int ItemID { get; set; }
            public string ResultValue { get; set; }
        }

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
            ctlToolbar.SetSelectedMenu(4);

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID));
            hdnHSULaboratoryID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
            hdnHSUImagingID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;

            LoadSubjectiveContent();
            LoadObjectiveContent();
            LoadAssessmentContent();
            LoadPlanningContent();
            LoadDischargeContent();
        }

        private void LoadSubjectiveContent()
        {
            List<SubjectiveContent> list = new List<SubjectiveContent>();

            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(filterExpression, 5, 1, "ID DESC").FirstOrDefault();

            if (oChiefComplaint != null)
            {
                SubjectiveContent oContent = new SubjectiveContent();
                oContent.HistoryDate = oChiefComplaint.ObservationDateInString;
                oContent.HistoryTime = oChiefComplaint.ObservationTime;
                oContent.ParamedicName = oChiefComplaint.ParamedicName;
                oContent.ChiefComplaintText = oChiefComplaint.ChiefComplaintText;
                list.Add(oContent);

                StringBuilder sbNotes = new StringBuilder();
                if (!string.IsNullOrEmpty(oChiefComplaint.Location))
                    sbNotes.AppendLine(string.Format("- Location    (R) : {0}", oChiefComplaint.Location));
                if (!string.IsNullOrEmpty(oChiefComplaint.DisplayQuality))
                    sbNotes.AppendLine(string.Format("- Quality     (Q) : {0}", oChiefComplaint.DisplayQuality));
                if (!string.IsNullOrEmpty(oChiefComplaint.DisplaySeverity))
                    sbNotes.AppendLine(string.Format("- Severity    (S) : {0}", oChiefComplaint.DisplaySeverity));
                if (!string.IsNullOrEmpty(oChiefComplaint.DisplayOnset))
                    sbNotes.AppendLine(string.Format("- Onset       (O) : {0}", oChiefComplaint.DisplayOnset));
                if (!string.IsNullOrEmpty(oChiefComplaint.CourseTiming))
                    sbNotes.AppendLine(string.Format("- Timing      (T) : {0}", oChiefComplaint.CourseTiming));
                if (!string.IsNullOrEmpty(oChiefComplaint.DisplayProvocation))
                    sbNotes.AppendLine(string.Format("- Provocation (T) : {0}", oChiefComplaint.DisplayProvocation));

                divHPI.InnerHtml = sbNotes.ToString();
            }
            else
            {
                SubjectiveContent oContent = new SubjectiveContent();
                #region Subjective Notes from Medical Record
                filterExpression = string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.SUBJECTIVE_NOTES);
                List<vPatientVisitNote> lstNotes = new List<vPatientVisitNote>();

                filterExpression += " ORDER BY NoteDate,NoteTime";
                lstNotes = BusinessLayer.GetvPatientVisitNoteList(filterExpression);
                if (lstNotes.Count > 0)
                {
                    foreach (vPatientVisitNote item in lstNotes)
                    {
                        oContent = new SubjectiveContent();
                        oContent.HistoryDate = item.NoteDateInString;
                        oContent.HistoryTime = item.NoteTime;
                        oContent.ParamedicName = string.Format("{0} (Entried By : {1})", item.ParamedicName, item.CreatedByName);
                        oContent.ChiefComplaintText = item.NoteText;
                        list.Add(oContent);
                    }
                }
                else
                {
                    oContent.HistoryDate = string.Empty;
                    oContent.HistoryTime = string.Empty;
                    oContent.ParamedicName = string.Empty;
                    oContent.ChiefComplaintText = string.Empty;
                    list.Add(oContent);
                }
                #endregion
            }

            rptChiefComplaint.DataSource = list;
            rptChiefComplaint.DataBind();
        }

        private void LoadObjectiveContent()
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            List<vReviewOfSystemHd> lstReviewOfSystemHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, 10, 1, "ID DESC");
            if (lstReviewOfSystemHd.Count > 0)
            {
                string lstID = String.Join(",", lstReviewOfSystemHd.Select(c => c.ID).ToArray());
                lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN ({0}) AND IsDeleted = 0", lstID));
                rptReviewOfSystemHd.DataSource = lstReviewOfSystemHd;
                rptReviewOfSystemHd.DataBind();
            }

            List<vVitalSignHd> lstVitalSignHd = BusinessLayer.GetvVitalSignHdList(filterExpression, 3, 1, "ID DESC");
            if (lstVitalSignHd.Count > 0)
            {
                string lstID = String.Join(",", lstVitalSignHd.Select(c => c.ID).ToArray());
                lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID IN ({0}) AND IsDeleted = 0", lstID));
                rptVitalSignHd.DataSource = lstVitalSignHd;
                rptVitalSignHd.DataBind();
            }
        }

        private void LoadPlanningContent()
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            #region Laboratory
            List<LaboratoryHdInfo> lstLaboratoryHd = new List<LaboratoryHdInfo>();
            filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} ORDER BY TransactionDate DESC", AppSession.RegisteredPatient.VisitID, hdnHSULaboratoryID.Value);
            List<vPatientVisitLaboratory> lstCharges1 = BusinessLayer.GetvPatientVisitLaboratoryList(filterExpression);
            if (lstCharges1.Count > 0)
            {
                foreach (vPatientVisitLaboratory item in lstCharges1)
                {
                    LaboratoryHdInfo oHeader = new LaboratoryHdInfo();
                    oHeader.ChargesID = item.TransactionID;
                    oHeader.TestDate = item.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT);
                    oHeader.TestTime = item.TransactionTime;
                    oHeader.ItemID = item.ItemID;
                    oHeader.ItemName = item.ItemName1;
                    lstLaboratoryHd.Add(oHeader);
                }
            }
            divLaboratoryNA.Style.Add("display", "none");

            rptLabTestOrder.DataSource = lstLaboratoryHd;
            rptLabTestOrder.DataBind();

            #endregion

            #region Imaging
            List<ImagingHdInfo> lstImagingHd = new List<ImagingHdInfo>();
            filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} ORDER BY TransactionDate DESC", AppSession.RegisteredPatient.VisitID, hdnHSUImagingID.Value);
            List<vPatientVisitImaging> lstCharges2 = BusinessLayer.GetvPatientVisitImagingList(filterExpression);
            if (lstCharges2.Count > 0)
            {
                foreach (vPatientVisitImaging item in lstCharges2)
                {
                    ImagingHdInfo oHeader = new ImagingHdInfo();
                    oHeader.ChargesID = item.TransactionID;
                    oHeader.TestDate = item.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT);
                    oHeader.TestTime = item.TransactionTime;
                    oHeader.ItemID = item.ItemID;
                    oHeader.ItemName = item.ItemName1;
                    lstImagingHd.Add(oHeader);
                }
            }


            if (lstCharges2.Count > 0)
            {
                divImagingNA.Style.Add("display", "none");
            }
            else
            {
                divImagingNA.Style.Add("display", "block");
            }
            rptImagingTestOrder.DataSource = lstImagingHd;
            rptImagingTestOrder.DataBind();

            #endregion

            #region Medication
            filterExpression = string.Format("VisitID = {0} ORDER BY PrescriptionOrderDetailID DESC", AppSession.RegisteredPatient.VisitID);
            List<vPatientVisitPrescription> list = BusinessLayer.GetvPatientVisitPrescriptionList(filterExpression);
            rptMedication.DataSource = list;
            rptMedication.DataBind();

            if (list.Count > 0)
            {
                divMedicationNA.Style.Add("display", "none");
            }
            else
            {
                divMedicationNA.Style.Add("display", "block");
            }
            #endregion
        }

        private void LoadAssessmentContent()
        {
            List<DiagnosisInfo> lstDiagnosis = new List<DiagnosisInfo>();
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            List<vPatientDiagnosis> lstPatientDiagnose = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
            foreach (vPatientDiagnosis dxItem in lstPatientDiagnose)
            {
                DiagnosisInfo oDiagnosis = new DiagnosisInfo();
                oDiagnosis.DiagnosisDate = dxItem.DifferentialDateInString;
                oDiagnosis.DiagnosisTime = dxItem.DifferentialTime;
                oDiagnosis.PhysicianName = dxItem.ParamedicName;
                oDiagnosis.DiagnosisText = dxItem.DiagnosisText;
                oDiagnosis.DiagnosisType = dxItem.DiagnoseType;
                lstDiagnosis.Add(oDiagnosis);
            }
            rptDifferentDiagnosis.DataSource = lstDiagnosis.OrderBy(p => p.DiagnosisType);
            rptDifferentDiagnosis.DataBind();
        }

        private void LoadDischargeContent()
        {

            string filterExpression = string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID);
            rptDischargeInformation.DataSource = BusinessLayer.GetvConsultVisitBaseList(filterExpression);
            rptDischargeInformation.DataBind();

            filterExpression = string.Format("FromVisitID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}')", AppSession.RegisteredPatient.VisitID, Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED);
            rptFollowUpVisit.DataSource = BusinessLayer.GetvAppointmentList(filterExpression, 3, 1, "AppointmentID DESC");
            rptFollowUpVisit.DataBind();
        }
        protected void rptTestOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vTestOrderDt obj = (vTestOrderDt)e.Item.DataItem;

                HtmlGenericControl spnTestOrderDtInformation = (HtmlGenericControl)e.Item.FindControl("spnTestOrderDtInformation");
                vTestOrderHd entityHd = lstTestOrderHd.FirstOrDefault(p => p.TestOrderID == obj.TestOrderID);
                spnTestOrderDtInformation.InnerHtml = string.Format("{0}, {1}", entityHd.TestOrderDateTimeInString, entityHd.ParamedicName);
            }
        }

        protected void rptReviewOfSystemHd_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vReviewOfSystemHd obj = (vReviewOfSystemHd)e.Item.DataItem;
                Repeater rptReviewOfSystemDt = (Repeater)e.Item.FindControl("rptReviewOfSystemDt");
                rptReviewOfSystemDt.DataSource = lstReviewOfSystemDt.Where(p => p.ID == obj.ID).ToList();
                rptReviewOfSystemDt.DataBind();
            }
        }

        protected void rptVitalSignHd_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Item.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Item.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = lstVitalSignDt.Where(p => p.ID == obj.ID).ToList();
                rptVitalSignDt.DataBind();
            }
        }

        protected void rptLabTestOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LaboratoryHdInfo obj = (LaboratoryHdInfo)e.Item.DataItem;
                List<LaboratoryDtInfo> lstResultDt = new List<LaboratoryDtInfo>();
                Repeater rptLaboratoryDt = (Repeater)e.Item.FindControl("rptLaboratoryDt");
                if (obj.ItemID != 0) // Is Not From Migration
                {
                    List<vLaboratoryResultDt> lstLabEntity = BusinessLayer.GetvLaboratoryResultDtList(string.Format("ChargeTransactionID = {0} AND ItemID = {1} ORDER BY DisplayOrder", obj.ChargesID, obj.ItemID));
                    if (lstLabEntity.Count > 0)
                    {
                        foreach (vLaboratoryResultDt result in lstLabEntity)
                        {
                            LaboratoryDtInfo oDetail = new LaboratoryDtInfo();
                            oDetail.ChargesID = result.ChargeTransactionID;
                            oDetail.ItemID = result.ItemID;
                            oDetail.FractionName = result.FractionName1;
                            if (!String.IsNullOrEmpty(result.TextValue))
                            {
                                string resultText = CleanHTMLTagFromResult(result.TextValue);
                                oDetail.ResultValue = StripHTML(resultText);
                                oDetail.ResultUnit = result.MetricUnit;
                            }
                            else
                            {
                                oDetail.ResultValue = result.MetricResultValue.ToString("G29");
                                oDetail.ResultUnit = result.MetricUnit;
                            }
                            oDetail.RefRange = string.Format("{0} - {1}", result.MinMetricNormalValue.ToString("G29"), result.MaxMetricNormalValue.ToString("G29"));
                            oDetail.ResultFlag = string.IsNullOrEmpty(result.ResultFlag) ? "N" : result.ResultFlag;

                            lstResultDt.Add(oDetail);
                        }
                    }
                }

                rptLaboratoryDt.DataSource = lstResultDt;
                rptLaboratoryDt.DataBind();
            }
        }

        protected void rptImagingTestOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ImagingHdInfo obj = (ImagingHdInfo)e.Item.DataItem;
                List<ImagingDtInfo> lstResultDt = new List<ImagingDtInfo>();
                List<vImagingResultDt> lstEntity = BusinessLayer.GetvImagingResultDtList(string.Format("ID IN (SELECT ID FROM ImagingResultHd WHERE ChargeTransactionID = {0}) AND ItemID = {1}", obj.ChargesID, obj.ItemID));
                Repeater rptImagingTestOrderDt = (Repeater)e.Item.FindControl("rptImagingTestOrderDt");
                if (lstEntity.Count > 0)
                {
                    foreach (vImagingResultDt result in lstEntity)
                    {
                        ImagingDtInfo oDetail = new ImagingDtInfo();
                        oDetail.ChargesID = obj.ChargesID;
                        oDetail.ItemID = result.ItemID;
                        oDetail.ResultValue = result.TestResult1;

                        lstResultDt.Add(oDetail);
                    }
                }
                rptImagingTestOrderDt.DataSource = lstResultDt;
                rptImagingTestOrderDt.DataBind();
            }
        }

        private string CleanHTMLTagFromResult(string resultText)
        {
            string result = resultText.Replace(@"<strong>", "");
            result = resultText.Replace(@"</strong>", "");
            result = resultText.Replace("&nbsp;", "");
            result = resultText.Replace(@"<br />", Environment.NewLine);
            return result;
        }

        public static string StripHTML(string HTMLText, bool decode = true)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            var stripped = reg.Replace(HTMLText, "");
            return decode ? HttpUtility.HtmlDecode(stripped) : stripped;
        }
    }
}