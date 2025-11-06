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
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class MedicalSummaryContent4Ctl : BaseViewPopupCtl
    {
        protected int Content4PageCount = 1;

        public override void InitializeDataControl(string queryString)
        {
            LoadContentInformation(Convert.ToInt32(queryString));
        }

        private void LoadContentInformation(int visitID)
        {
            string registrationID = "0";
            vConsultVisit4 oVisit = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", visitID)).FirstOrDefault();
            if (oVisit != null)
            {

                if (oVisit.GCVisitStatus != Constant.VisitStatus.DISCHARGED && oVisit.GCVisitStatus != Constant.VisitStatus.CLOSED)
                {
                    txtFromDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtToDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }
                else
                {
                    txtFromDate.Text = oVisit.DischargeDate.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtToDate.Text = oVisit.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }

                registrationID = string.Format("{0},{1}", oVisit.RegistrationID, oVisit.LinkedRegistrationID);
            }

            hdnContentRegistrationID.Value = registrationID;

            BindGridView(1, true, ref Content4PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("RegistrationID IN ({0}) AND GCPatientNoteType NOT IN ('{1}','{2}') AND (NoteDate BETWEEN '{3}' AND '{4}')", hdnContentRegistrationID.Value, Constant.PatientVisitNotes.REGISTRATION_NOTES, Constant.PatientVisitNotes.REFERRAL_FROM_NOTES, Helper.GetDatePickerValue(txtFromDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            //if (cboDisplay.Value.ToString() == "1")
            //{
            //    filterExpression = string.Format("RegistrationID IN ({0}) AND GCPatientNoteType IN ('{1}','{2}') AND (NoteDate BETWEEN '{3}' AND '{4}')", lstReg, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.NURSE_ANAMNESIS, Helper.GetDatePickerValue(txtFromDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            //}
            //else if (cboDisplay.Value.ToString() == "2")
            //{
            //    filterExpression = string.Format("RegistrationID IN ({0}) AND GCPatientNoteType IN ('{1}','{2}') AND ParamedicID = {3} AND (NoteDate BETWEEN '{4}' AND '{5}')", lstReg, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.NURSE_ANAMNESIS, AppSession.UserLogin.ParamedicID, Helper.GetDatePickerValue(txtFromDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            //}

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += "IsDeleted = 0";

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientVisitNoteRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientVisitNote> lstEntity = BusinessLayer.GetvPatientVisitNoteList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "NoteDate DESC, NoteTime DESC");
            grdMedicalSummaryContent4.DataSource = lstEntity;
            grdMedicalSummaryContent4.DataBind();
        }

        protected void grdMedicalSummaryContent4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientVisitNote entity = e.Row.DataItem as vPatientVisitNote;
                HtmlInputButton btnVerified = e.Row.FindControl("btnVerify") as HtmlInputButton;
                HtmlControl divVerifiedInfo = e.Row.FindControl("divVerifiedInformation") as HtmlControl;
                HtmlControl divPhysicianVerifiedInfo = e.Row.FindControl("divPhysicianVerifiedInformation") as HtmlControl;
                HtmlControl divNursingNotesInfo = (HtmlControl)e.Row.FindControl("divNursingNotesInfo");
                HtmlControl divConfirmationInfo = (HtmlControl)e.Row.FindControl("divConfirmationInfo");
                HtmlControl divSignature = e.Row.FindControl("divParamedicSignature") as HtmlControl;
                HtmlControl divView = e.Row.FindControl("divView") as HtmlControl;

                if (entity.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES)
                {
                    if (!entity.IsWrite)
                    {
                        divNursingNotesInfo.Style.Add("display", "none");
                    }

                    if (!entity.IsConfirmed)
                    {
                        if (divConfirmationInfo != null)
                            divConfirmationInfo.Style.Add("display", "none");
                    }
                    else
                    {
                        if (divConfirmationInfo != null)
                            divConfirmationInfo.Style.Add("display", "block");
                        divNursingNotesInfo.Style.Add("display", "block");
                    }
                }
                else
                {
                    divNursingNotesInfo.Style.Add("display", "none");
                    divConfirmationInfo.Style.Add("display", "none");
                }

                if (!entity.IsVerified)
                {
                    if (divPhysicianVerifiedInfo != null)
                        divPhysicianVerifiedInfo.Visible = false;
                }
                else
                {
                    if (divPhysicianVerifiedInfo != null)
                        divPhysicianVerifiedInfo.Visible = true;
                }

                if (AppSession.UserLogin.IsPrimaryNurse)
                {
                    if (!entity.IsVerifiedByPrimaryNurse && entity.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES)
                    {
                        btnVerified.Visible = true;
                        divVerifiedInfo.Visible = false;
                    }
                    else
                    {
                        btnVerified.Visible = false;
                        if (entity.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES)
                        {
                            divVerifiedInfo.Visible = true;
                        }
                        else
                        {
                            divVerifiedInfo.Visible = false;
                        }
                    }
                }
                else
                {
                    btnVerified.Visible = false;
                    divVerifiedInfo.Visible = false;
                }

                if (divSignature != null)
                {
                    divSignature.Visible = entity.ParamedicID == AppSession.UserLogin.ParamedicID;
                }

                if (divView != null)
                {
                    divView.Visible = entity.GCPatientNoteType == Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT;
                }
            }
        }

        protected void cbpMedicalSummaryContent4View_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref Content4PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref Content4PageCount);
                    result = "refresh|" + Content4PageCount;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}