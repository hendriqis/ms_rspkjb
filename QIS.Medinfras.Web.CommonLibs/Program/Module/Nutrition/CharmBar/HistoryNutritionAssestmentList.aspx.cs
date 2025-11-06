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
using QIS.Data.Core.Dal;
using System.Text;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class HistoryNutritionAssestmentList : BasePage
    {
        protected int PageCount = 1;
        protected int PageCount2 = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                //txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                
                SetComboBox(); 

                BindGridView(1, true, ref PageCount);
                BindGridViewDt(1, true, ref PageCount2);

                //BindGridView2(1, true, ref PageCount2);
                //BindGridViewDt2(1, true, ref PageCount2);
            }
        }
        private void SetComboBox()
        {
            List<Variable> lstDisplay = new List<Variable>() { new Variable() { Code = "Catatan Ahli Gizi", Value = "4" }};
            Methods.SetComboBoxField(cboDisplay, lstDisplay, "Code", "Value");
            cboDisplay.Value = "4";
        }
       
        #region Header
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND VisitID != {1}", AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.VisitID);

            if (rblItemType.SelectedValue == "1") 
            {
                filterExpression = string.Format("MRN = {0} AND VisitID = {1}", AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.VisitID);
            }
          
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPreviousMedicalHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPreviousMedicalHistory> lstEntity = BusinessLayer.GetvPreviousMedicalHistoryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "VisitID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
             
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Detail
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            Registration oReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationNo='{0}'", hdnRegistrationNo.Value)).FirstOrDefault();
            if (oReg != null)
            {
             
                if (hdnFirstSelected.Value == "1")
                {
                    txtFromDate.Text = oReg.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    DateTime endDate = oReg.RegistrationDate.AddDays(7);
                    txtToDate.Text = endDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    hdnDateFrom.Value = txtFromDate.Text;
                    hdnDateTo.Value = txtToDate.Text;
                }

                List<Registration> dataRegID = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0}) OR RegistrationID = {0}", oReg.RegistrationID));
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

                string filterExpression = string.Format("RegistrationID IN ({0}) AND GCPatientNoteType NOT IN ('{1}','{2}','{3}') AND (NoteDate BETWEEN '{4}' AND '{5}')", lstReg, Constant.PatientVisitNotes.REGISTRATION_NOTES, Constant.PatientVisitNotes.REFERRAL_FROM_NOTES, Constant.PatientVisitNotes.NURSE_NOTES, Helper.GetDatePickerValue(txtFromDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToDate).ToString(Constant.FormatString.DATE_FORMAT_112));
                if (cboDisplay.Value.ToString() == "1")
                {
                    filterExpression = string.Format("RegistrationID IN ({0}) AND GCPatientNoteType IN ('{1}','{2}') AND GCParamedicMasterType = '{3}'", lstReg, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT, AppSession.UserLogin.GCParamedicMasterType);
                }
                else if (cboDisplay.Value.ToString() == "2")
                {
                    filterExpression = string.Format("RegistrationID IN ({0}) AND GCPatientNoteType IN ('{1}','{2}') AND ParamedicID = {3}", lstReg, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT, AppSession.UserLogin.ParamedicID);
                }
                else if (cboDisplay.Value.ToString() == "3")
                {
                    filterExpression += string.Format(" AND GCParamedicMasterType = '{0}'", Constant.ParamedicType.Physician);
                }
                else if (cboDisplay.Value.ToString() == "4")
                {
                    filterExpression += string.Format(" AND GCParamedicMasterType = '{0}'", Constant.ParamedicType.Nutritionist);
                }

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

                grdViewDt.DataSource = lstEntity;
                grdViewDt.DataBind();
            }
            else {
                grdViewDt.DataSource = null;
                grdViewDt.DataBind();
            }
            
        }
        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientVisitNote entity = e.Row.DataItem as vPatientVisitNote;
                HtmlInputButton btnVerified = e.Row.FindControl("btnVerify") as HtmlInputButton;
                HtmlControl divVerifiedInfo = e.Row.FindControl("divVerifiedInformation") as HtmlControl;
                HtmlControl divPhysicianVerifiedInfo = e.Row.FindControl("divPhysicianVerifiedInformation") as HtmlControl;
                HtmlControl divNursingNotesInfo = (HtmlControl)e.Row.FindControl("divNursingNotesInfo");
                HtmlControl divConfirmationInfo = (HtmlControl)e.Row.FindControl("divConfirmationInfo");
                //HtmlControl divSignature = e.Row.FindControl("divParamedicSignature") as HtmlControl;
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

                //if (divSignature != null)
                //{s
                //    divSignature.Visible = entity.ParamedicID == AppSession.UserLogin.ParamedicID;
                //}

                if (divView != null)
                {
                    divView.Visible = (entity.GCPatientNoteType == Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT) || (entity.GCPatientNoteType == Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT) || (entity.GCPatientNoteType == Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT);
                }
            }
        }

        #endregion

         

         
    }
    
}