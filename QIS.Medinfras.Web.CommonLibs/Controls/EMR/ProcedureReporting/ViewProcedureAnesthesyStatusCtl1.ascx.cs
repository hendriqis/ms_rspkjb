using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class ViewProcedureAnesthesyStatusCtl1 : BaseViewPopupCtl
    {
        protected int gridAllergyPageCount = 1;
        protected int gridROSPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridProcedureGroupPageCount = 1;
        private List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnVisitID.Value = paramInfo[0];
            hdnPatientChargesDtID.Value = paramInfo[1];
            hdnAnesthesyStatusID.Value = paramInfo[2];
            txtTransactionNo.Text = paramInfo[3];
            txtProcedureGroupSummary.Text = paramInfo[4];

            #region Patient Information
            vConsultVisit4 registeredPatient = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
            lblPatientName.InnerHtml = registeredPatient.cfPatientNameSalutation;
            lblGender.InnerHtml = registeredPatient.Gender;
            lblDateOfBirth.InnerHtml = string.Format("{0} ({1})", registeredPatient.cfDateOfBirth, Helper.GetPatientAge(words, registeredPatient.DateOfBirth));

            lblRegistrationDateTime.InnerHtml = string.Format("{0} / {1}", registeredPatient.cfVisitDate, registeredPatient.VisitTime);
            lblRegistrationNo.InnerHtml = registeredPatient.RegistrationNo;
            lblPhysician.InnerHtml = registeredPatient.ParamedicName;

            lblMedicalNo.InnerHtml = registeredPatient.MedicalNo;

            lblPayerInformation.InnerHtml = registeredPatient.BusinessPartnerName;
            lblPatientLocation.InnerHtml = registeredPatient.cfPatientLocation;
            imgPatientImage.Src = registeredPatient.PatientImageUrl;
            #endregion

            string filterExp = string.Format("VisitID = {0} AND AnesthesyStatusID = {1} AND IsDeleted = 0 ORDER BY AnesthesyStatusID DESC", hdnVisitID.Value, hdnAnesthesyStatusID.Value);
            vSurgeryAnesthesyStatus obj = BusinessLayer.GetvSurgeryAnesthesyStatusList(filterExp).FirstOrDefault();
            lblPhysicianName2.InnerHtml = obj.ParamedicName;

            hdnMRN.Value = obj.MRN.ToString();

            if (obj != null)
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

                #region Asesmen Pra Induksi
                txtServiceDate.Text = obj.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime.Text = obj.AssessmentTime;
                txtStartDate.Text = obj.StartSurgeryDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtStartTime.Text = obj.StartSurgeryTime;
                txtDuration.Text = obj.SurgeryDuration.ToString();
                txtStartAnesthesyDate.Text = obj.StartAnesthesyDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtStartAnesthesyTime.Text = obj.StartAnesthesyTime;
                txtAnesthesyDuration.Text = obj.AnesthesyDuration.ToString();
                if (obj.AnesthesyDuration > 0)
                {
                    DateTime startAnesthesyDateTime = DateTime.Parse(string.Format("{0} {1}", obj.StartAnesthesyDate.ToString(Constant.FormatString.DATE_FORMAT), obj.StartAnesthesyTime));
                    DateTime endAnesthesyDateTime = startAnesthesyDateTime.AddMinutes(Convert.ToInt32(obj.AnesthesyDuration));

                    txtStopAnesthesyDate.Text = endAnesthesyDateTime.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtStopAnesthesyTime.Text = endAnesthesyDateTime.ToString(Constant.FormatString.TIME_FORMAT);
                }
                if (obj.SurgeryDuration > 0)
                {
                    DateTime startSurgeryDateTime = DateTime.Parse(string.Format("{0} {1}", obj.StartSurgeryDate.ToString(Constant.FormatString.DATE_FORMAT), obj.StartSurgeryTime));
                    DateTime endSurgeryDateTime = startSurgeryDateTime.AddMinutes(Convert.ToInt32(obj.SurgeryDuration));

                    txtStopSurgeryDate.Text = endSurgeryDateTime.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtStopSurgeryTime.Text = endSurgeryDateTime.ToString(Constant.FormatString.TIME_FORMAT);
                } 
                txtStartIncisionDate.Text = obj.StartIncisionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtStartIncisionTime.Text = obj.StartIncisionTime;
                rblGCASAStatus.SelectedValue = obj.GCASAStatus;
                chkIsASAStatusE.Checked = obj.IsASAStatusE;
                rblIsASAChanged.SelectedValue = obj.IsASAChanged ? "1" : "0";
                txtASAStatusRemarks.Text = obj.ASAStatusChangeRemarks;

                vPreAnesthesyAssessment objPreAnesthesy = BusinessLayer.GetvPreAnesthesyAssessmentList(string.Format("VisitID = {0} AND PatientChargesDtID  = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnPatientChargesDtID.Value)).FirstOrDefault();
                if (objPreAnesthesy != null)
                {
                    txtStartFastingDate.Text = objPreAnesthesy.StartFastingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtStartFastingTime.Text = objPreAnesthesy.StartFastingTime; 
                }

                if (obj.StartPremedicationDate != null && obj.StartPremedicationDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
                {
                    txtPremedicationDate.Text = obj.StartPremedicationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }
                else
                {
                    txtPremedicationDate.Text = "";
                }
                txtPremedicationTime.Text = obj.StartPremedicationTime;
                rblGCPremedicationType.SelectedValue = obj.GCPremedicationType;
                txtAnesthesiaType.Text = obj.cfAnesthesiaType;
                txtRegionalAnesthesiaType.Text = obj.RegionalAnesthesiaType;
                rblIsAnesthesiaTypeChanged.SelectedValue = obj.IsAnesthesiaTypeChanged ? "1" : "0";
                txtAnesthesiaTypeChangeRemarks.Text = obj.AnesthesiaTypeChangeRemarks;
                chkIsTimeOutSafetyCheck.Checked = obj.IsTimeOutSafetyCheck;
                #endregion

                #region Monitors
                StringBuilder innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesyStatusMonitorList.html");

                divFormContent1.InnerHtml = obj.MonitoringCheckListLayout;
                hdnMonitoringCheckListLayout.Value = obj.MonitoringCheckListLayout;
                hdnMonitoringCheckListValue.Value = obj.MonitoringCheckListValue;
                #endregion

                #region IV List
                innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesyStatusIVList.html");

                divFormContent2.InnerHtml = obj.IVCheckListLayout;
                hdnIVCheckListLayout.Value = obj.IVCheckListLayout;
                hdnIVCheckListValue.Value = obj.IVCheckListValue;
                #endregion

                #region Accessories
                innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesyStatusAccessoriesList.html");

                divFormContent3.InnerHtml = obj.AccessoriesListLayout;
                hdnAccessoriesListLayout.Value = obj.AccessoriesListLayout;
                hdnAccessoriesListValue.Value = obj.AccessoriesListValue;
                #endregion

                #region Position
                innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesyStatusPositionList.html");

                divFormContent4.InnerHtml = obj.PatientPositionLayout;
                hdnPatientPositionLayout.Value = obj.PatientPositionLayout;
                hdnPatientPositionValue.Value = obj.PatientPositionValue;
                #endregion

                #region Airway Management
                innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesyStatusAirwayList.html");

                divFormContent5.InnerHtml = obj.AirwayManagementLayout;
                hdnAirwayManagementLayout.Value = obj.AirwayManagementLayout;
                hdnAirwayManagementValue.Value = obj.AirwayManagementValue;
                #endregion

                #region Regional Anesthetics
                innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesyStatusRegionalList.html");

                divFormContent6.InnerHtml = obj.RegionalAnestheticLayout;
                hdnRegionalAnestheticLayout.Value = obj.RegionalAnestheticLayout;
                hdnRegionalAnestheticValue.Value = obj.RegionalAnestheticValue;
                #endregion

                BindGridViewAllergy(1, true, ref gridAllergyPageCount);
                BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
            }
        }

        #region Allergy
        private void BindGridViewAllergy(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", hdnMRN.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAllergyRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAllergy> lstEntity = BusinessLayer.GetvPatientAllergyList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdAllergyView.DataSource = lstEntity;
            grdAllergyView.DataBind();

            chkIsPatientAllergyExists.Checked = !(lstEntity.Count > 0);
        }

        protected void cbpAllergyView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewAllergy(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewAllergy(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;

            filterExpression += string.Format("SurgeryAnesthesyStatusID = {0} AND IsDeleted = 0",hdnAnesthesyStatusID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("SurgeryAnesthesyStatusID = {0} AND IsDeleted = 0 ORDER BY DisplayOrder", hdnAnesthesyStatusID.Value));
            grdVitalSignView.DataSource = lstEntity;
            grdVitalSignView.DataBind();
        }

        protected void grdVitalSignView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Row.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Row.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = GetVitalSignDt(obj.ID);
                rptVitalSignDt.DataBind();
            }
        }

        protected List<vVitalSignDt> GetVitalSignDt(Int32 ID)
        {
            return lstVitalSignDt.Where(p => p.ID == ID).ToList();
        }

        protected void cbpVitalSignView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewVitalSign(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewVitalSign(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
        }

        #endregion
    }
}