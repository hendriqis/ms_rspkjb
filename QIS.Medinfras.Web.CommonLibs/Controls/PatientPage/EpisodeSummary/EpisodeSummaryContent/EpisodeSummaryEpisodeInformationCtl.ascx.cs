using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class EpisodeSummaryEpisodeInformationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            LoadSubjectiveContent(Convert.ToInt32(queryString));
        }

        private void LoadSubjectiveContent(int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY ID DESC", visitID);
            vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(filterExpression).FirstOrDefault();

            if (oChiefComplaint != null)
            {
                txtDate.Text = oChiefComplaint.ObservationDateInString;
                txtTime.Text = oChiefComplaint.ObservationTime;
                txtPhysicianName.Text = oChiefComplaint.ParamedicName;
                txtChiefComplaint.Text = oChiefComplaint.ChiefComplaintText;

                txtHPISummary.Text = oChiefComplaint.HPISummary;
                chkAutoAnamnesis.Checked = oChiefComplaint.IsAutoAnamnesis;
                chkAlloAnamnesis.Checked = oChiefComplaint.IsAlloAnamnesis;
            }

            //#region Get Subjective contents from Linked Registration
            //List<SubjectiveContent> list2 = new List<SubjectiveContent>();
            //if (entity.DepartmentID == Constant.Facility.INPATIENT)
            //{
            //    filterExpression = string.Format("RegistrationID = {0} AND IsMainVisit = 1", entity.LinkedRegistrationID);
            //    vConsultVisit linkVisit = BusinessLayer.GetvConsultVisitList(filterExpression).FirstOrDefault();
            //    if (linkVisit != null)
            //    {
            //        filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY ID DESC", linkVisit.VisitID);
            //        oChiefComplaint = BusinessLayer.GetvChiefComplaintList(filterExpression).FirstOrDefault();

            //        SubjectiveContent oContent = new SubjectiveContent();
            //        oContent.HistoryDate = oChiefComplaint.ObservationDateInString;
            //        oContent.HistoryTime = oChiefComplaint.ObservationTime;
            //        oContent.ParamedicName = oChiefComplaint.ParamedicName;
            //        oContent.ChiefComplaintText = oChiefComplaint.ChiefComplaintText;
            //        oContent.FromServiceUnit = linkVisit.ServiceUnitName;
            //        list2.Add(oContent);

            //        StringBuilder sbNotes = new StringBuilder();
            //        if (!string.IsNullOrEmpty(oChiefComplaint.Location))
            //            sbNotes.AppendLine(string.Format("- Location    (R) : {0}", oChiefComplaint.Location));
            //        if (!string.IsNullOrEmpty(oChiefComplaint.DisplayQuality))
            //            sbNotes.AppendLine(string.Format("- Quality     (Q) : {0}", oChiefComplaint.DisplayQuality));
            //        if (!string.IsNullOrEmpty(oChiefComplaint.DisplaySeverity))
            //            sbNotes.AppendLine(string.Format("- Severity    (S) : {0}", oChiefComplaint.DisplaySeverity));
            //        if (!string.IsNullOrEmpty(oChiefComplaint.DisplayOnset))
            //            sbNotes.AppendLine(string.Format("- Onset       (O) : {0}", oChiefComplaint.DisplayOnset));
            //        if (!string.IsNullOrEmpty(oChiefComplaint.CourseTiming))
            //            sbNotes.AppendLine(string.Format("- Timing      (T) : {0}", oChiefComplaint.CourseTiming));
            //        if (!string.IsNullOrEmpty(oChiefComplaint.DisplayProvocation))
            //            sbNotes.AppendLine(string.Format("- Provocation (T) : {0}", oChiefComplaint.DisplayProvocation));

            //        rptLinkedChiefComplaint.DataSource = list2;
            //        rptLinkedChiefComplaint.DataBind();

            //        divLinkedHPI.InnerHtml = sbNotes.ToString();
            //    }
            //}
            //#endregion
        }      
    }
}