﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Globalization;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class ProcedureAnesthesyStatusEntry1 : BasePagePatientPageList
    {
        protected int gridAllergyPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected static string _assessmentID = "0";
        protected static string _linkedVisitID;

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        protected string GetAssessmentID()
        {
            return _assessmentID;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_PROCEDURES;
        }

        protected override void InitializeDataControl()
        {
            string[] paramInfo = Page.Request.QueryString["id"].Split('|');

            hdnPatientChargesDtID.Value = paramInfo[0];
            txtTransactionNo.Text = paramInfo[1];
            txtTransactionNo.Enabled = false;
            _assessmentID = paramInfo[2];
            hdnAssessmentID.Value = _assessmentID;
            txtProcedureGroupSummary.Text = paramInfo[3];
               
            Helper.SetControlEntrySetting(txtASAStatusRemarks, new ControlEntrySetting(true, true, false), "mpPatientStatus");       

            txtAssessmentDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAssessmentTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            txtStartFastingDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnImagingServiceUnitID.Value = AppSession.ImagingServiceUnitID;
            hdnLaboratoryServiceUnitID.Value = AppSession.LaboratoryServiceUnitID;

            SetEntityToControl();

            BindGridViewAllergy(1, true, ref gridAllergyPageCount);
            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);

            hdnLinkedVisitID.Value = _linkedVisitID;
            hdnAssessmentID.Value = _assessmentID;
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            #region Monitors
            StringBuilder innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesyStatusMonitorList.html");

            divFormContent1.InnerHtml = innerHtml.ToString();
            hdnMonitoringCheckListLayout.Value = innerHtml.ToString();
            #endregion

            #region IV List
            innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesyStatusIVList.html");

            divFormContent2.InnerHtml = innerHtml.ToString();
            hdnIVCheckListLayout.Value = innerHtml.ToString();
            #endregion

            #region Accessories
            innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesyStatusAccessoriesList.html");

            divFormContent3.InnerHtml = innerHtml.ToString();
            hdnAccessoriesListLayout.Value = innerHtml.ToString();
            #endregion

            #region Position
            innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesyStatusPositionList.html");

            divFormContent4.InnerHtml = innerHtml.ToString();
            hdnPatientPositionLayout.Value = innerHtml.ToString();
            #endregion

            #region Airway Management
            innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesyStatusAirwayList.html");

            divFormContent5.InnerHtml = innerHtml.ToString();
            hdnAirwayManagementLayout.Value = innerHtml.ToString();
            #endregion

            #region Regional Anesthetics
            innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesyStatusRegionalList.html");

            divFormContent6.InnerHtml = innerHtml.ToString();
            hdnRegionalAnestheticLayout.Value = innerHtml.ToString();
            #endregion
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        private void SetEntityToControl()
        {
            vConsultVisit4 entityVisit = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnPatientInformation.Value = string.Format("{0} (MRN = {1}, REG = {2}, LOC = {3}, DOB = {4})", entityVisit.cfPatientNameInLabel, entityVisit.MedicalNo, entityVisit.RegistrationNo, entityVisit.ServiceUnitName, entityVisit.cfDateOfBirth);
            hdnDepartmentID.Value = entityVisit.DepartmentID;

            vPreSurgeryAssessment obj1 = BusinessLayer.GetvPreSurgeryAssessmentList(string.Format("VisitID = {0} AND PatientChargesDtID  = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnPatientChargesDtID.Value)).FirstOrDefault();
            if (obj1 != null)
            {
                chkIsPatientAllergyExists.Checked = !obj1.IsPatientAllergyExists;
            }

            vPreAnesthesyAssessment objPreAnesthesy = BusinessLayer.GetvPreAnesthesyAssessmentList(string.Format("VisitID = {0} AND PatientChargesDtID  = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnPatientChargesDtID.Value)).FirstOrDefault();
            if (objPreAnesthesy != null)
            {
                rblGCASAStatus.SelectedValue = objPreAnesthesy.GCASAStatus;
                chkIsASAStatusE.Checked = objPreAnesthesy.IsASAStatusE;
                rblIsASAChanged.SelectedValue = "0";
                if (!string.IsNullOrEmpty(txtStartFastingTime.Text))
                {
                    txtStartFastingDate.Text = objPreAnesthesy.StartFastingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtStartFastingTime.Text = objPreAnesthesy.StartFastingTime; 
                }
                //cboAnesthesiaType.Value = objPreAnesthesy.GCAnesthesiaType;
                cboRegionalAnesthesiaType.Value = objPreAnesthesy.GCRegionalAnesthesiaType;
                rblIsUsePremedication.SelectedValue = objPreAnesthesy.IsUsePremedication ? "1" : "0";
                chkIsGeneralAnesthesy.Checked = objPreAnesthesy.IsGeneralAnesthesy;
                chkIsLocal.Checked = objPreAnesthesy.IsLocal;
                chkIsSedation.Checked = objPreAnesthesy.IsSedation;
                chkIsRegional.Checked = objPreAnesthesy.IsRegional;
                cboRegionalAnesthesiaType.Value = objPreAnesthesy.GCRegionalAnesthesiaType;
                rblIsAnesthesiaTypeChanged.SelectedValue = "0";
            }

            vSurgeryAnesthesyStatus obj = BusinessLayer.GetvSurgeryAnesthesyStatusList(string.Format("VisitID = {0} AND AnesthesyStatusID = {1} AND IsDeleted = 0", hdnVisitID.Value, hdnAssessmentID.Value)).FirstOrDefault();
            if (obj != null)
            {
                if (obj.AnesthesyStatusID == 0)
                {
                    hdnAssessmentID.Value = "0";
                    hdnParamedicID.Value = "0";
                    _assessmentID = "0";

                    PopulateFormContent();
                }
                else
                {
                    _assessmentID = obj.AnesthesyStatusID.ToString();
                    hdnAssessmentID.Value = obj.AnesthesyStatusID.ToString();
                    hdnParamedicID.Value = obj.ParamedicID.ToString();
                    txtAssessmentDate.Text = obj.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtAssessmentTime.Text = obj.AssessmentTime;
                    txtStartDate.Text = obj.StartSurgeryDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtStartTime.Text = obj.StartSurgeryTime;
                    txtStartAnesthesyDate.Text = obj.StartAnesthesyDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtStartAnesthesyTime.Text = obj.StartAnesthesyTime;
                    txtDuration.Text = obj.SurgeryDuration.ToString();
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
                    if (!string.IsNullOrEmpty(obj.StartIncisionTime))
                    {
                        txtStartIncisionDate.Text = obj.StartIncisionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtStartIncisionTime.Text = obj.StartIncisionTime; 
                    }
                    if (!string.IsNullOrEmpty(obj.StartPremedicationTime))
                    {
                        txtPremedicationDate.Text = obj.StartPremedicationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtPremedicationTime.Text = obj.StartPremedicationTime; 
                    }
                    rblGCASAStatus.SelectedValue = obj.GCASAStatus;
                    chkIsASAStatusE.Checked = obj.IsASAStatusE;
                    rblIsASAChanged.SelectedValue = obj.IsASAChanged ? "1" : "0";
                    txtASAStatusRemarks.Text = obj.ASAStatusChangeRemarks;
                    chkIsGeneralAnesthesy.Checked = obj.IsGeneralAnesthesy;
                    chkIsLocal.Checked = obj.IsLocal;
                    chkIsSedation.Checked = obj.IsSedation;
                    chkIsRegional.Checked = obj.IsRegional;
                    cboRegionalAnesthesiaType.Value = obj.GCRegionalAnesthesiaType;
                    rblIsAnesthesiaTypeChanged.SelectedValue = obj.IsAnesthesiaTypeChanged ? "1" : "0";
                    txtAnesthesiaTypeChangeRemarks.Text = obj.AnesthesiaTypeChangeRemarks;
                    rblGCPremedicationType.SelectedValue = obj.GCPremedicationType;
                    chkIsTimeOutSafetyCheck.Checked = obj.IsTimeOutSafetyCheck;
                    divFormContent1.InnerHtml = obj.MonitoringCheckListLayout;
                    hdnMonitoringCheckListLayout.Value = obj.MonitoringCheckListLayout;
                    hdnMonitoringCheckListValue.Value = obj.MonitoringCheckListValue;
                    divFormContent2.InnerHtml = obj.IVCheckListLayout;
                    hdnIVCheckListLayout.Value = obj.IVCheckListLayout;
                    hdnIVCheckListValue.Value = obj.IVCheckListValue;
                    divFormContent3.InnerHtml = obj.AccessoriesListLayout;
                    hdnAccessoriesListLayout.Value = obj.AccessoriesListLayout;
                    hdnAccessoriesListValue.Value = obj.AccessoriesListValue;
                    divFormContent4.InnerHtml = obj.PatientPositionLayout;
                    hdnPatientPositionLayout.Value = obj.PatientPositionLayout;
                    hdnPatientPositionValue.Value = obj.PatientPositionValue;
                    divFormContent5.InnerHtml = obj.AirwayManagementLayout;
                    hdnAirwayManagementLayout.Value = obj.AirwayManagementLayout;
                    hdnAirwayManagementValue.Value = obj.AirwayManagementValue;
                    divFormContent6.InnerHtml = obj.RegionalAnestheticLayout;
                    hdnRegionalAnestheticLayout.Value = obj.RegionalAnestheticLayout;
                    hdnRegionalAnestheticValue.Value = obj.RegionalAnestheticValue;
                    hdnIsChanged.Value = "0";
                    hdnIsSaved.Value = "0";
                }
            }
            else
            {
                if (objPreAnesthesy != null)
                {
                    rblGCASAStatus.SelectedValue = objPreAnesthesy.GCASAStatus;
                }

                PopulateFormContent();

                _assessmentID = "0";
            }
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
    Constant.StandardCode.ALLERGEN_TYPE, Constant.StandardCode.REGIONAL_ANESTHESIA_TYPE);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);

            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboAllergenType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ALLERGEN_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboRegionalAnesthesiaType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.REGIONAL_ANESTHESIA_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            cboAllergenType.Value = Constant.AllergenType.DRUG;
        }

        private void UpdateAssessment(IDbContext ctx)
        {
            SurgeryAnesthesyStatusDao assessmentDao = new SurgeryAnesthesyStatusDao(ctx);
            SurgeryAnesthesyStatus entity = null;
            bool isNewAssessment = true;

            if (hdnAssessmentID.Value != "" && hdnAssessmentID.Value != "0")
            {
                entity = assessmentDao.Get(Convert.ToInt32(hdnAssessmentID.Value));
                isNewAssessment = false;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            }
            else
            {
                entity = new SurgeryAnesthesyStatus();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.PatientChargesDtID = Convert.ToInt32(hdnPatientChargesDtID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }

            DateTime startSurgeryDateTime = DateTime.Parse(string.Format("{0} {1}", Helper.GetDatePickerValue(txtStartDate).ToString(Constant.FormatString.DATE_FORMAT), txtStartTime.Text));
            DateTime endSurgeryDateTime = startSurgeryDateTime.AddMinutes(Convert.ToInt32(txtDuration.Text));

            DateTime startAnesthesyDateTime = DateTime.Parse(string.Format("{0} {1}", Helper.GetDatePickerValue(txtStartAnesthesyDate).ToString(Constant.FormatString.DATE_FORMAT), txtStartAnesthesyTime.Text));
            DateTime endAnesthesyDateTime = startAnesthesyDateTime.AddMinutes(Convert.ToInt32(txtAnesthesyDuration.Text));

            entity.AssessmentDate = Helper.GetDatePickerValue(txtAssessmentDate);
            entity.AssessmentTime = txtAssessmentTime.Text;
            entity.StartSurgeryDate = Helper.GetDatePickerValue(txtStartDate);
            entity.StartSurgeryTime = txtStartTime.Text;
            //entity.StartIncisionDate = Helper.GetDatePickerValue(txtStartIncisionDate);
            //entity.StartIncisionTime = txtStartIncisionTime.Text;
            entity.StartIncisionDate = entity.StartSurgeryDate;
            entity.StartIncisionTime = entity.StartSurgeryTime;
            if (!string.IsNullOrEmpty(txtDuration.Text) && Methods.IsNumeric(txtDuration.Text))
            {
                entity.SurgeryDuration = Convert.ToDecimal(txtDuration.Text); 
            }
            entity.StopSurgeryDate = endSurgeryDateTime.Date;
            entity.StopSurgeryTime = endSurgeryDateTime.ToString(Constant.FormatString.TIME_FORMAT);
            entity.StartAnesthesyDate = Helper.GetDatePickerValue(txtStartAnesthesyDate);
            entity.StartAnesthesyTime = txtStartAnesthesyTime.Text;
            entity.StopAnesthesyDate = endAnesthesyDateTime.Date;
            entity.StopAnesthesyTime = endAnesthesyDateTime.ToString(Constant.FormatString.TIME_FORMAT);
            if (!string.IsNullOrEmpty(txtAnesthesyDuration.Text) && Methods.IsNumeric(txtAnesthesyDuration.Text))
            {
                entity.AnesthesyDuration = Convert.ToDecimal(txtAnesthesyDuration.Text); 
            }
            entity.StartPremedicationDate = Helper.GetDatePickerValue(txtPremedicationDate);
            entity.StartPremedicationTime = txtPremedicationTime.Text;
            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);            
            entity.GCASAStatus = rblGCASAStatus.SelectedValue;
            entity.IsASAStatusE = chkIsASAStatusE.Checked;
            entity.IsASAChanged = rblIsASAChanged.SelectedValue == "1" ? true : false;
            entity.ASAStatusChangeRemarks = txtASAStatusRemarks.Text;

            entity.MonitoringCheckListLayout = hdnMonitoringCheckListLayout.Value;
            entity.MonitoringCheckListValue = hdnMonitoringCheckListValue.Value;
            entity.IVCheckListLayout = hdnIVCheckListLayout.Value;
            entity.IVCheckListValue = hdnIVCheckListValue.Value;
            entity.AccessoriesListLayout = hdnAccessoriesListLayout.Value;
            entity.AccessoriesListValue = hdnAccessoriesListValue.Value;
            entity.PatientPositionLayout = hdnPatientPositionLayout.Value;
            entity.PatientPositionValue = hdnPatientPositionValue.Value;
            entity.AirwayManagementLayout = hdnAirwayManagementLayout.Value;
            entity.AirwayManagementValue = hdnAirwayManagementValue.Value;
            entity.RegionalAnestheticLayout = hdnRegionalAnestheticLayout.Value;
            entity.RegionalAnestheticValue = hdnRegionalAnestheticValue.Value;
            entity.IsTimeOutSafetyCheck = chkIsTimeOutSafetyCheck.Checked;

            entity.IsGeneralAnesthesy = chkIsGeneralAnesthesy.Checked;
            entity.IsLocal = chkIsLocal.Checked;
            entity.IsRegional = chkIsRegional.Checked;
            entity.IsSedation = chkIsSedation.Checked;

            if (cboRegionalAnesthesiaType.Value != null)
            {
                entity.GCRegionalAnesthesiaType = cboRegionalAnesthesiaType.Value.ToString();
            }

            entity.IsAnesthesiaTypeChanged = rblIsAnesthesiaTypeChanged.SelectedValue == "1" ? true : false;

            if (!string.IsNullOrEmpty(txtAnesthesiaTypeChangeRemarks.Text))
            {
                entity.AnesthesiaTypeChangeRemarks = txtAnesthesiaTypeChangeRemarks.Text;
            }

            entity.IsUsePremedication = rblIsUsePremedication.SelectedValue == "1" ? true : false;
            if (entity.IsUsePremedication)
            {
                entity.Premedication = txtPremedication.Text;
                entity.GCPremedicationType = rblGCPremedicationType.SelectedValue;
            }

            if (isNewAssessment)
            {
                hdnAssessmentID.Value = assessmentDao.InsertReturnPrimaryKeyID(entity).ToString();
                _assessmentID = hdnAssessmentID.Value;
            }
            else
            {
                assessmentDao.Update(entity);
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string message)
        {
            if (type == "save")
            {
                bool result = true;

                if ((hdnAssessmentID.Value != "" && hdnAssessmentID.Value != "0") && !IsValidToSave(ref message))
                {
                    result = false;
                    hdnIsSaved.Value = "0";
                    return result;
                }

                IDbContext ctx = DbFactory.Configure(true);
                try
                {
                    UpdateAssessment(ctx);
                    ctx.CommitTransaction();

                    message = _assessmentID;
                    hdnIsSaved.Value = "1";
                    hdnIsChanged.Value = "0";
                }
                catch (Exception ex)
                {
                    result = false;
                    message = ex.Message;
                    hdnIsSaved.Value = "0";
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }

                return result;
            }
            return true;
        }


        private void BindGridViewAllergy(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAllergyRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAllergy> lstEntity = BusinessLayer.GetvPatientAllergyList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdAllergyView.DataSource = lstEntity;
            grdAllergyView.DataBind();

            chkIsPatientAllergyExists.Checked = !(lstEntity.Count > 0);
            chkIsPatientAllergyExists.Enabled = (lstEntity.Count == 0);
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

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;

            string assessmentID = !string.IsNullOrEmpty(_assessmentID) ? _assessmentID : "0";
            string linkedVisitID = !string.IsNullOrEmpty(_linkedVisitID) ? _linkedVisitID : "0";

            filterExpression += string.Format("VisitID IN ({0},{1}) AND SurgeryAnesthesyStatusID = {2} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, linkedVisitID, _assessmentID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0},{1}) AND SurgeryAnesthesyStatusID = {2} AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, linkedVisitID, _assessmentID));
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

        protected void cbpDeleteVitalSign_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnVitalSignRecordID.Value != "")
            {
                VitalSignHd entity = BusinessLayer.GetVitalSignHd(Convert.ToInt32(hdnVitalSignRecordID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVitalSignHd(entity);
            }
            else
            {
                result = "0|There is no record to be deleted !";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        protected void cbpAllergy_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";


            bool isError = false;

            IDbContext ctx = DbFactory.Configure(true);
            PatientAllergyDao allergyDao = new PatientAllergyDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            PatientAllergy oAllergy;

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        oAllergy = new PatientAllergy();
                        oAllergy.MRN = AppSession.RegisteredPatient.MRN;
                        oAllergy.AllergyLogDate = DateTime.Now.Date;
                        oAllergy.GCAllergenType = cboAllergenType.Value.ToString();
                        oAllergy.Allergen = txtAllergenName.Text;
                        oAllergy.GCAllergySource = Constant.AllergenFindingSource.PATIENT;
                        oAllergy.GCAllergySeverity = Constant.AllergySeverity.UNKNOWN;
                        oAllergy.KnownDate = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);
                        oAllergy.Reaction = txtReaction.Text;
                        oAllergy.CreatedBy = AppSession.UserLogin.UserID;
                        allergyDao.Insert(oAllergy);

                        Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                        if (!oPatient.IsHasAllergy)
                        {
                            oPatient.IsHasAllergy = true;
                            oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientDao.Update(oPatient);
                        }

                        result = "1|add|1";
                    }
                    else if (param[0] == "edit")
                    {
                        int allergyID = Convert.ToInt32(hdnAllergyID.Value);
                        oAllergy = allergyDao.Get(allergyID);

                        if (oAllergy != null)
                        {
                            oAllergy.GCAllergenType = cboAllergenType.Value.ToString();
                            oAllergy.Allergen = txtAllergenName.Text;
                            oAllergy.Reaction = txtReaction.Text;
                            oAllergy.LastUpdatedBy = AppSession.UserLogin.UserID;
                            allergyDao.Update(oAllergy);
                            result = "1|edit|1";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Allergy Record Information");
                            isError = true;
                        }
                    }
                    else
                    {
                        int allergyID = Convert.ToInt32(hdnAllergyID.Value);
                        oAllergy = allergyDao.Get(allergyID);

                        if (oAllergy != null)
                        {
                            //TODO : Prompt user for delete reason
                            oAllergy.IsDeleted = true;
                            oAllergy.LastUpdatedBy = AppSession.UserLogin.UserID;
                            allergyDao.Update(oAllergy);
                            result = "1|delete|";
                            string isHasAllergy = "0";

                            List<PatientAllergy> lstAllergy = BusinessLayer.GetPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN), ctx);
                            if (lstAllergy.Count > 0)
                            {
                                Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                                if (!oPatient.IsHasAllergy)
                                {
                                    oPatient.IsHasAllergy = true;
                                    oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientDao.Update(oPatient);
                                }

                                isHasAllergy = "1";
                            }
                            else
                            {
                                Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                                if (oPatient.IsHasAllergy)
                                {
                                    oPatient.IsHasAllergy = false;
                                    oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientDao.Update(oPatient);
                                }

                                isHasAllergy = "0";
                            }
                            result = "1|delete|" + isHasAllergy;
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Allergy Record Information");
                            isError = true;
                        }
                        result = "1|delete|";
                    }

                    if (!isError)
                        ctx.CommitTransaction();
                    else
                        ctx.RollBackTransaction();
                }

            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("0|process|{0}", ex.Message);
            }
            finally
            {
                ctx.Close();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

         private bool IsValidToSave(ref string errMessage)
        {
            StringBuilder errMsg = new StringBuilder();
            if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
            {
                int paramedicID = Convert.ToInt32(hdnParamedicID.Value);
                if (AppSession.UserLogin.ParamedicID != paramedicID)
                {
                    errMsg.AppendLine("Perubahan Asesmen hanya dapat dilakukan oleh Dokter yang melakukan Pengkajian");
                }
            }

            //DateTime anesthesyDateTime = new DateTime(
            DateTime anesthesyDateTime = DateTime.ParseExact(string.Format("{0} {1}", txtStartAnesthesyDate.Text, txtStartAnesthesyTime.Text), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);
            DateTime surgeryDateTime = DateTime.ParseExact(string.Format("{0} {1}", txtStartDate.Text, txtStartTime.Text), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);

            if (surgeryDateTime < anesthesyDateTime)
            {
                errMsg.AppendLine("Tanggal dan Jam Operasi harus lebih besar dari Tanggal dan Jam Anestesi");
            }

            errMessage = errMsg.ToString();

            return (errMessage == string.Empty);
        }
    }
}
