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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class SurgeryReportEntry1 : BasePagePatientPageList
    {
        protected int gridImplantPageCount = 1;
        protected int gridParamedicTeamPageCount = 1;
        protected int gridProcedureGroupPageCount = 1;
        protected static string _reportID = "0";
        protected static string _linkedVisitID;

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        protected string GetReportID()
        {
            return _reportID;
        }

        public override string OnGetMenuCode()
        {
            string[] paramInfo = Page.Request.QueryString["id"].Split('|');
            switch (paramInfo[3])
            {
                case "anesthesy":
                    return Constant.MenuCode.EMR.PENGKAJIAN_ANESTESI;
                case "surgery":
                    return Constant.MenuCode.EMR.ASESMEN_PRA_BEDAH;
                default:
                    return Constant.MenuCode.EMR.ASESMEN_PRA_BEDAH;
            }
        }

        protected override void InitializeDataControl()
        {
            string[] paramInfo = Page.Request.QueryString["id"].Split('|');

            hdnTestOrderID.Value = paramInfo[0];
            txtTestOrderNo.Text = paramInfo[1];
            hdnReportID.Value = paramInfo[2];
            txtTestOrderNo.Enabled = false;

            Helper.SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtDiagnosisText, new ControlEntrySetting(true, true, true), "mpPatientStatus");

            txtReportDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReportTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtProfilaxisTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();

            hdnImagingServiceUnitID.Value = AppSession.ImagingServiceUnitID;
            hdnLaboratoryServiceUnitID.Value = AppSession.LaboratoryServiceUnitID;

            SetEntityToControl();

            BindGridViewImplant(1, false, ref gridImplantPageCount);
            BindGridViewParamedicTeam(1, false, ref gridParamedicTeamPageCount);
            BindGridViewProcedureGroup(1, false, ref gridProcedureGroupPageCount);

            LoadBodyDiagram();

            hdnLinkedVisitID.Value = _linkedVisitID;
            hdnReportID.Value = _reportID;
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

            vPatientSurgery obj = BusinessLayer.GetvPatientSurgeryList(string.Format("PatientSurgeryID = {0} AND IsDeleted = 0 AND VisitID IN (SELECT cv.VisitID FROM ConsultVisit cv WITH(NOLOCK) WHERE cv.RegistrationID IN (SELECT r.RegistrationID FROM Registration r WITH(NOLOCK) WHERE r.RegistrationID = {1} OR r.LinkedToRegistrationID = {1}))", hdnReportID.Value, entityVisit.RegistrationID)).FirstOrDefault();
            if (obj != null)
            {
                #region Data Operasi
                _reportID = obj.PatientSurgeryID.ToString();
                hdnReportID.Value = obj.PatientSurgeryID.ToString();
                hdnParamedicID.Value = obj.ParamedicID.ToString();
                txtReportDate.Text = obj.ReportDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtReportTime.Text = obj.ReportTime;
                txtStartDate.Text = obj.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtStartTime.Text = obj.StartTime;
                txtEndDate.Text = obj.EndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtEndTime.Text = obj.EndTime;
                txtDuration.Text = obj.Duration.ToString();
                txtSurgeryNo.Text = obj.SurgeryNo.ToString();
                rblSurgeryNoType.SelectedValue = obj.IsFirstSurgery ? "1" : "0";

                if (!obj.IsFirstSurgery)
                {
                    txtSurgeryNo.Enabled = true;
                }

                if (obj.GCAnesthesiaType != null)
                {
                    cboAnesthesiaType.Value = obj.GCAnesthesiaType;
                }
                if (obj.GCWoundType != null)
                {
                    cboWoundType.Value = obj.GCWoundType;
                }
                rblIsUsingProfilaksis.SelectedValue = obj.IsUsingAntibiotics ? "1" : "0";
                txtProfilaxis.Text = obj.AntibioticsType;
                if (rblIsUsingProfilaksis.SelectedValue == "1")
                {
                    txtProfilaxisTime.Text = obj.AntibioticsTime;
                    txtProfilaxisTime.Enabled = true;
                    txtProfilaxis.Enabled = true;
                }
                rblIsHasComplexity.SelectedValue = obj.IsHasComplication ? "1" : "0";
                if (obj.IsHasComplication)
                {
                    txtComplexityRemarks.Enabled = true;
                }
                txtComplexityRemarks.Text = obj.ComplicationRemarks;
                rblIsHemorrhage.SelectedValue = obj.IsHasHemorrhage ? "1" : "0";
                if (obj.IsHasHemorrhage)
                {
                    txtHemorrhage.Enabled = true;
                }
                txtHemorrhage.Text = obj.Hemorrhage.ToString();
                rblIsBloodDrain.SelectedValue = obj.IsBloodDrain ? "1" : "0";
                if (obj.IsBloodDrain)
                {
                    txtOtherBloodDrainType.Enabled = true;
                }
                txtOtherBloodDrainType.Text = obj.OtherBloodDrainType;
                rblIsUsingTampon.SelectedValue = obj.IsUsingTampon ? "1" : "0";
                if (obj.IsUsingTampon)
                {
                    txtTamponType.Enabled = true;
                }
                txtTamponType.Text = obj.TamponType;
                rblIsUsingTourniquet.SelectedValue = obj.IsUsingTourniquet ? "1" : "0";
                rblIsBloodTransfussion.SelectedValue = obj.IsBloodTransfussion ? "1" : "0";
                if (obj.IsBloodTransfussion)
                {
                    txtBloodTransfussion.Enabled = true;
                }
                txtBloodTransfussion.Text = obj.BloodTransfussion.ToString();
                rblIsTestKultur.SelectedValue = obj.IsTestKultur ? "1" : "0";
                if (obj.IsTestKultur)
                {
                    txtOtherTestKulturType.Enabled = true;
                }
                txtOtherTestKulturType.Text = obj.OtherTestKulturType;
                rblIsTestCytology.SelectedValue = obj.IsTestCytology ? "1" : "0";
                if (obj.IsTestCytology)
                {
                    txtOtherTestCytologyType.Enabled = true;
                }
                txtOtherTestCytologyType.Text = obj.OtherTestCytologyType;
                rblIsSpecimenTest.SelectedValue = obj.IsTestPA ? "1" : "0";
                if (obj.IsTestPA)
                {
                    cboSpecimen.Enabled = true;
                }
                if (obj.SpecimenID != null && obj.SpecimenID != 0)
                {
                    cboSpecimen.Value = obj.SpecimenID.ToString();
                    Specimen sp = BusinessLayer.GetSpecimenList(string.Format("SpecimenID = {0}", obj.SpecimenID)).FirstOrDefault();

                    if (sp.SpecimenCode == "SP999")
                    {
                        trOtherSpecimenType.Style.Remove("display");
                    }

                    if (!string.IsNullOrEmpty(obj.OtherSpecimenType.ToString()))
                    {
                        txtOtherSpecimenType.Text = obj.OtherSpecimenType.ToString();
                    }
                }
                #endregion

                #region Diagnosa dan Jenis Operasi
                hdnEntryDiagnoseID.Value = obj.PreOperativeDiagnosisID;
                ledDiagnose.Value = obj.PreOperativeDiagnosisText;
                txtDiagnosisText.Text = obj.PreOperativeDiagnosisText;
                hdnEntryPostDiagnoseID.Value = obj.PostOperativeDiagnosisID;
                ledPostDiagnose.Value = obj.PostOperativeDiagnosisText;
                txtPostDiagnosisText.Text = obj.PostOperativeDiagnosisText;
                txtProcedureGroupRemarks.Text = obj.ProcedureGroupRemarks;
                #endregion

                #region Team Pelaksana
                txtReferralSummary.Text = obj.ReferralSummary;
                #endregion

                #region Uraian Pembedahan
                txtRemarks.Text = obj.PostOperativeDiagnosisRemarks;
                #endregion


                hdnIsChanged.Value = "0";
                hdnIsSaved.Value = "0";
            }
            else
            {
                hdnReportID.Value = "0";
                hdnParamedicID.Value = "0";
                _reportID = "0";

                // Get Default Values
                vSurgeryTestOrderHd1 orderInfo = BusinessLayer.GetvSurgeryTestOrderHd1List(string.Format("TestOrderID = {0} AND VisitID IN (SELECT cv.VisitID FROM ConsultVisit cv WITH(NOLOCK) WHERE cv.RegistrationID IN (SELECT r.RegistrationID FROM Registration r WITH(NOLOCK) WHERE r.RegistrationID = {1} OR r.LinkedToRegistrationID = {1}))", hdnTestOrderID.Value, entityVisit.RegistrationID)).FirstOrDefault();
                if (orderInfo != null)
                {
                    txtStartDate.Text = orderInfo.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtStartTime.Text = orderInfo.ScheduledTime;
                    txtDuration.Text = orderInfo.EstimatedDuration.ToString();
                }

                vPreSurgeryAssessment assessment = BusinessLayer.GetvPreSurgeryAssessmentList(string.Format("TestOrderID = {0} AND IsDeleted = 0 AND VisitID IN (SELECT cv.VisitID FROM ConsultVisit cv WITH(NOLOCK) WHERE cv.RegistrationID IN (SELECT r.RegistrationID FROM Registration r WITH(NOLOCK) WHERE r.RegistrationID = {1} OR r.LinkedToRegistrationID = {1}))", hdnTestOrderID.Value, entityVisit.RegistrationID)).FirstOrDefault();
                if (assessment != null)
                {
                    hdnEntryDiagnoseID.Value = assessment.PreDiagnoseID;
                    ledDiagnose.Value = assessment.PreDiagnoseText;
                    txtDiagnosisText.Text = assessment.PreDiagnoseText;
                }

                vSurgicalSafetyCheck safetyCheck = BusinessLayer.GetvSurgicalSafetyCheckList(string.Format("TestOrderID = {0} AND IsDeleted = 0 AND VisitID IN (SELECT cv.VisitID FROM ConsultVisit cv WITH(NOLOCK) WHERE cv.RegistrationID IN (SELECT r.RegistrationID FROM Registration r WITH(NOLOCK) WHERE r.RegistrationID = {1} OR r.LinkedToRegistrationID = {1}))", hdnTestOrderID.Value, entityVisit.RegistrationID)).FirstOrDefault();
                if (safetyCheck != null)
                {
                    if (!string.IsNullOrEmpty(safetyCheck.cfSurgeryEndDate))
                    {
                        txtEndDate.Text = safetyCheck.cfSurgeryEndDate;
                        txtEndTime.Text = safetyCheck.cfSurgeryEndTime;

                        DateTime startDateTime = DateTime.Parse(string.Format("{0} {1}", Helper.GetDatePickerValue(txtStartDate).ToString(Constant.FormatString.DATE_FORMAT), txtStartTime.Text));
                        DateTime endDateTime = DateTime.Parse(string.Format("{0} {1}", Helper.GetDatePickerValue(txtEndDate).ToString(Constant.FormatString.DATE_FORMAT), txtEndTime.Text));
                        double duration = (endDateTime - startDateTime).TotalMinutes;
                        txtDuration.Text = duration.ToString();
                    }
                    else
                    {
                        DateTime startDateTime = DateTime.Parse(string.Format("{0} {1}", Helper.GetDatePickerValue(txtStartDate).ToString(Constant.FormatString.DATE_FORMAT), txtStartTime.Text));
                        DateTime endDateTime = startDateTime.AddMinutes(Convert.ToInt32(txtDuration.Text));

                        txtEndDate.Text = endDateTime.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtEndTime.Text = endDateTime.ToString(Constant.FormatString.TIME_FORMAT);
                    }
                }
            }
        }

        private void LoadBodyDiagram()
        {
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnTestOrderID.Value);
            int pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
            hdnPageCount.Value = pageCount.ToString();

            if (pageCount > 0)
            {
                hdnPageIndex.Value = "0";
                OnLoadBodyDiagram(0);
                tblBodyDiagramNavigation.Style.Remove("display");
            }
            else
            {
                divBodyDiagram.Style.Add("display", "none");
                tblEmpty.Style.Remove("display");
            }
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ANESTHESIA_TYPE, Constant.StandardCode.JENIS_PEMBEDAHAN, Constant.StandardCode.SURGERY_TEAM_ROLE);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);

            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboAnesthesiaType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ANESTHESIA_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboWoundType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.JENIS_PEMBEDAHAN).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboParamedicType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SURGERY_TEAM_ROLE).ToList(), "StandardCodeName", "StandardCodeID");

            List<Specimen> lstSpecimen = BusinessLayer.GetSpecimenList("IsDeleted = 0 ORDER BY SpecimenCode");
            Methods.SetComboBoxField(cboSpecimen, lstSpecimen, "SpecimenName", "SpecimenID");
        }

        private void UpdateSurgeryReport(IDbContext ctx)
        {
            PatientSurgeryDao assessmentDao = new PatientSurgeryDao(ctx);
            TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
            PatientSurgery entity = null;
            bool isNewAssessment = true;

            DateTime startDateTime = DateTime.Parse(string.Format("{0} {1}", Helper.GetDatePickerValue(txtStartDate).ToString(Constant.FormatString.DATE_FORMAT), txtStartTime.Text));
            DateTime endDateTime = startDateTime.AddMinutes(Convert.ToInt32(txtDuration.Text));

            if (hdnReportID.Value != "" && hdnReportID.Value != "0")
            {
                entity = assessmentDao.Get(Convert.ToInt32(hdnReportID.Value));
                isNewAssessment = false;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            }
            else
            {
                entity = new PatientSurgery();
                entity.MRN = AppSession.RegisteredPatient.MRN;
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }

            #region Data Operasi
            entity.ReportDate = Helper.GetDatePickerValue(txtReportDate);
            entity.ReportTime = txtReportTime.Text;
            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.StartTime = txtStartTime.Text;
            entity.Duration = Convert.ToDecimal(txtDuration.Text);
            entity.EndDate = endDateTime.Date;
            entity.EndTime = endDateTime.ToString(Constant.FormatString.TIME_FORMAT);
            entity.IsFirstSurgery = rblSurgeryNoType.SelectedValue == "1";
            if (!entity.IsFirstSurgery)
            {
                if (!string.IsNullOrEmpty(txtSurgeryNo.Text))
                {
                    entity.SurgeryNo = Convert.ToInt16(txtSurgeryNo.Text);
                }
                else
                {
                    entity.SurgeryNo = 1;
                }
            }
            if (cboAnesthesiaType.Value != null)
            {
                if (!string.IsNullOrEmpty(cboAnesthesiaType.Value.ToString()))
                    entity.GCAnesthesiaType = cboAnesthesiaType.Value.ToString();
            }
            if (cboWoundType.Value != null)
            {
                if (!string.IsNullOrEmpty(cboWoundType.Value.ToString()))
                    entity.GCWoundType = cboWoundType.Value.ToString();
            }
            if (!string.IsNullOrEmpty(rblIsUsingProfilaksis.SelectedValue))
            {
                entity.IsUsingAntibiotics = rblIsUsingProfilaksis.SelectedValue == "1";
                if (entity.IsUsingAntibiotics)
                {
                    entity.AntibioticsTime = txtProfilaxisTime.Text;
                    entity.AntibioticsType = txtProfilaxis.Text;
                }
            }
            if (!string.IsNullOrEmpty(rblIsHasComplexity.SelectedValue))
            {
                entity.IsHasComplication = rblIsHasComplexity.SelectedValue == "1";
                if (entity.IsHasComplication)
                {
                    entity.ComplicationRemarks = txtComplexityRemarks.Text;
                }
            }
            if (!string.IsNullOrEmpty(rblIsHemorrhage.SelectedValue))
            {
                entity.IsHasHemorrhage = rblIsHemorrhage.SelectedValue == "1";
                if (entity.IsHasHemorrhage)
                {
                    if (!string.IsNullOrEmpty(txtHemorrhage.Text))
                        entity.Hemorrhage = Convert.ToDecimal(txtHemorrhage.Text);
                    else
                        entity.Hemorrhage = 0;
                }
            }
            if (!string.IsNullOrEmpty(rblIsBloodDrain.SelectedValue))
            {
                entity.IsBloodDrain = rblIsBloodDrain.SelectedValue == "1";
                if (entity.IsBloodDrain)
                {
                    entity.OtherBloodDrainType = txtOtherBloodDrainType.Text;
                }
            }
            if (!string.IsNullOrEmpty(rblIsUsingTampon.SelectedValue))
            {
                entity.IsUsingTampon = rblIsUsingTampon.SelectedValue == "1";
                if (entity.IsUsingTampon)
                {
                    entity.TamponType = txtTamponType.Text;
                }
            }
            if (!string.IsNullOrEmpty(rblIsUsingTourniquet.SelectedValue))
            {
                entity.IsUsingTourniquet = rblIsUsingTourniquet.SelectedValue == "1";
            }
            if (!string.IsNullOrEmpty(rblIsBloodTransfussion.SelectedValue))
            {
                entity.IsBloodTransfussion = rblIsBloodTransfussion.SelectedValue == "1";
                if (entity.IsBloodTransfussion)
                {
                    if (!string.IsNullOrEmpty(txtBloodTransfussion.Text))
                        entity.BloodTransfussion = Convert.ToDecimal(txtBloodTransfussion.Text);
                    else
                        entity.BloodTransfussion = 0;
                }
            }
            if (!string.IsNullOrEmpty(rblIsTestKultur.SelectedValue))
            {
                entity.IsTestKultur = rblIsTestKultur.SelectedValue == "1";
                if (entity.IsTestKultur)
                {
                    entity.OtherTestKulturType = txtOtherTestKulturType.Text;
                }
            }

            if (!string.IsNullOrEmpty(rblIsTestCytology.SelectedValue))
            {
                entity.IsTestCytology = rblIsTestCytology.SelectedValue == "1";
                if (entity.IsTestCytology)
                {
                    entity.OtherTestCytologyType = txtOtherTestCytologyType.Text;
                }
            }

            if (!string.IsNullOrEmpty(rblIsSpecimenTest.SelectedValue))
            {
                entity.IsTestPA = rblIsSpecimenTest.SelectedValue == "1";
                if (!String.IsNullOrEmpty(hdnSpecimenID.Value))
                {
                    if (hdnSpecimenID.Value != "0")
                    {
                        entity.SpecimenID = Convert.ToInt32(hdnSpecimenID.Value);
                        Specimen sp = BusinessLayer.GetSpecimenList(string.Format("SpecimenID = {0}", hdnSpecimenID.Value)).FirstOrDefault();
                        if (sp.SpecimenCode == "SP999")
                        {
                            entity.OtherSpecimenType = txtOtherSpecimenType.Text;
                        }
                        else
                        {
                            entity.OtherSpecimenType = null;
                        }
                    }
                    else
                    {
                        entity.SpecimenID = 0;
                        entity.OtherSpecimenType = null;
                    }
                }
            }
            #endregion

            #region Diagnosa dan Tindakan Operasi
            if (!string.IsNullOrEmpty(hdnEntryDiagnoseID.Value))
            {
                entity.PreOperativeDiagnosisID = hdnEntryDiagnoseID.Value;
            }
            entity.PreOperativeDiagnosisText = txtDiagnosisText.Text;
            if (!string.IsNullOrEmpty(hdnEntryPostDiagnoseID.Value))
            {
                entity.PostOperativeDiagnosisID = hdnEntryPostDiagnoseID.Value;
            }
            entity.PostOperativeDiagnosisText = txtPostDiagnosisText.Text;

            if (!string.IsNullOrEmpty(txtProcedureGroupRemarks.Text))
            {
                entity.ProcedureGroupRemarks = txtProcedureGroupRemarks.Text;
            }
            #endregion

            #region Team Pelaksana
            entity.ReferralSummary = txtReferralSummary.Text;
            #endregion

            #region Daftar Implant
            entity.IsUsingImplant = chkIsUsingImplant.Checked;
            #endregion

            #region Uraian Pembedahan
            entity.PostOperativeDiagnosisRemarks = txtRemarks.Text;
            #endregion

            if (isNewAssessment)
            {
                hdnReportID.Value = assessmentDao.InsertReturnPrimaryKeyID(entity).ToString();
                _reportID = hdnReportID.Value;
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

                if ((hdnReportID.Value != "" && hdnReportID.Value != "0") && !IsValidToSave(ref message))
                {
                    result = false;
                    hdnIsSaved.Value = "0";
                    return result;
                }

                IDbContext ctx = DbFactory.Configure(true);
                try
                {
                    PatientSurgery obj = BusinessLayer.GetPatientSurgeryList(string.Format("PatientSurgeryID = {0} AND IsDeleted = 0 AND VisitID IN (SELECT cv.VisitID FROM ConsultVisit cv WITH(NOLOCK) WHERE cv.RegistrationID IN (SELECT r.RegistrationID FROM Registration r WITH(NOLOCK) WHERE r.RegistrationID = {1} OR r.LinkedToRegistrationID = {1}))", hdnReportID.Value, hdnRegistrationID.Value), ctx).FirstOrDefault();
                    if (obj != null)
                        hdnReportID.Value = obj.PatientSurgeryID.ToString();
                    else
                        hdnReportID.Value = "0";

                    UpdateSurgeryReport(ctx);
                    ctx.CommitTransaction();

                    message = _reportID;
                    hdnReportID.Value = _reportID;

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


        private void BindGridViewImplant(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, hdnTestOrderID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientMedicalDeviceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientMedicalDevice> lstEntity = BusinessLayer.GetvPatientMedicalDeviceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdImplantView.DataSource = lstEntity;
            grdImplantView.DataBind();

            chkIsUsingImplant.Checked = (lstEntity.Count > 0);
        }

        protected void cbpImplantView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewImplant(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewImplant(1, false, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Team
        private void BindGridViewParamedicTeam(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;

            string reportID = !string.IsNullOrEmpty(_reportID) ? _reportID : "0";

            filterExpression += string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnTestOrderID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientSurgeryTeamRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_SERATUS);
            }

            List<vPatientSurgeryTeam> lstEntity = BusinessLayer.GetvPatientSurgeryTeamList(filterExpression, Constant.GridViewPageSize.GRID_SERATUS, pageIndex, "ID DESC");
            grdParamedicTeamView.DataSource = lstEntity;
            grdParamedicTeamView.DataBind();
        }

        protected void cbpParamedicTeamView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewParamedicTeam(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewParamedicTeam(1, false, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpParamedicTeam_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            IDbContext ctx = DbFactory.Configure(true);
            PatientSurgeryTeamDao paramedicTeamDao = new PatientSurgeryTeamDao(ctx);

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        PatientSurgeryTeam obj = new PatientSurgeryTeam();

                        obj.PatientSurgeryID = Convert.ToInt32(hdnReportID.Value);
                        obj.ParamedicID = Convert.ToInt32(hdnEntryParamedicID.Value);
                        obj.GCParamedicRole = cboParamedicType.Value.ToString();
                        obj.CreatedBy = AppSession.UserLogin.UserID;
                        paramedicTeamDao.Insert(obj);

                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnOrderDtParamedicTeamID.Value);
                        PatientSurgeryTeam entity = BusinessLayer.GetPatientSurgeryTeam(recordID);

                        if (entity != null)
                        {
                            entity.ParamedicID = Convert.ToInt32(hdnEntryParamedicID.Value);
                            entity.GCParamedicRole = cboParamedicType.Value.ToString();
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientSurgeryTeam(entity);
                            result = "1|edit|";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Informasi Dokter/Tenaga Medis tidak valid");
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnOrderDtParamedicTeamID.Value);
                        PatientSurgeryTeam entity = BusinessLayer.GetPatientSurgeryTeam(recordID);

                        if (entity != null)
                        {
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientSurgeryTeam(entity);
                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Jenis Dokter/Tenaga Medis tidak valid");
                        }
                        result = "1|delete|";
                    }
                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        protected void cbpImplant_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        PatientAllergy oAllergy = new PatientAllergy();
                        oAllergy.MRN = AppSession.RegisteredPatient.MRN;
                        oAllergy.AllergyLogDate = DateTime.Now.Date;
                        oAllergy.GCAllergySource = Constant.AllergenFindingSource.PATIENT;
                        oAllergy.GCAllergySeverity = Constant.AllergySeverity.UNKNOWN;
                        oAllergy.KnownDate = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);
                        oAllergy.CreatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.InsertPatientAllergy(oAllergy);

                        Patient oPatient = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
                        if (!oPatient.IsHasAllergy)
                        {
                            oPatient.IsHasAllergy = true;
                            oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatient(oPatient);
                        }

                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int allergyID = Convert.ToInt32(hdnAllergyID.Value);
                        PatientAllergy oAllergy = BusinessLayer.GetPatientAllergy(allergyID);

                        if (oAllergy != null)
                        {
                            //oAllergy.GCAllergenType = cboAllergenType.Value.ToString();
                            //oAllergy.Allergen = txtAllergenName.Text;
                            //oAllergy.Reaction = txtReaction.Text;
                            oAllergy.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientAllergy(oAllergy);
                            result = "1|edit|";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Allergy Record Information");
                        }
                    }
                    else
                    {
                        int allergyID = Convert.ToInt32(hdnAllergyID.Value);
                        PatientAllergy oAllergy = BusinessLayer.GetPatientAllergy(allergyID);

                        if (oAllergy != null)
                        {
                            //TODO : Prompt user for delete reason
                            oAllergy.IsDeleted = true;
                            oAllergy.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientAllergy(oAllergy);
                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Allergy Record Information");
                        }
                        result = "1|delete|";
                    }

                }

            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpSpecimen_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "enable")
                {
                    cboSpecimen.Enabled = true;
                }
                else
                {
                    cboSpecimen.Enabled = false;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Body Diagram
        protected void cbpBodyDiagramView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            int pageIndex = Convert.ToInt32(hdnPageIndex.Value);
            int pageCount = Convert.ToInt32(hdnPageCount.Value);
            if (e.Parameter == "refresh")
            {
                string filterExpression = "";
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

                pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
                result = "count|" + pageCount;
                if (pageCount > 0)
                    OnLoadBodyDiagram(0);
            }
            else if (e.Parameter == "edit")
            {
                result = "edit";
                OnLoadBodyDiagram(pageIndex);
            }
            else
            {
                if (e.Parameter == "next")
                {
                    pageIndex++;
                    if (pageIndex == pageCount)
                        pageIndex = 0;
                }
                else if (e.Parameter == "prev")
                {
                    pageIndex--;
                    if (pageIndex < 0)
                        pageIndex = pageCount - 1;
                }
                OnLoadBodyDiagram(pageIndex);
                result = "index|" + pageIndex;
            }

            if (pageCount > 0)
            {
                hdnPageIndex.Value = "0";
                tblBodyDiagramNavigation.Style.Remove("display");
            }
            else
            {
                divBodyDiagram.Style.Add("display", "none");
                tblEmpty.Style.Remove("display");
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void OnLoadBodyDiagram(int PageIndex)
        {
            string filterExpression = "";
            filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnTestOrderID.Value);
            vPatientBodyDiagramHd entity = BusinessLayer.GetvPatientBodyDiagramHd(filterExpression, PageIndex, "ID DESC");
            BodyDiagramToControl(entity);

            filterExpression = string.Format("ID = {0} AND IsDeleted = 0", entity.ID);
            rptRemarks.DataSource = BusinessLayer.GetvPatientBodyDiagramDtList(filterExpression);
            rptRemarks.DataBind();
        }

        private void BodyDiagramToControl(vPatientBodyDiagramHd entity)
        {
            spnParamedicName.InnerHtml = entity.ParamedicName;
            spnObservationDateTime.InnerHtml = entity.DisplayObservationDateTime;
            spnDiagramName.InnerHtml = entity.DiagramName;

            imgBodyDiagram.Src = entity.FileImageUrl;
            hdnBodyDiagramID.Value = entity.ID.ToString();

        }

        protected void cbpDeleteBodyDiagram_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnBodyDiagramID.Value != "")
            {
                PatientBodyDiagramHd entity = BusinessLayer.GetPatientBodyDiagramHd(Convert.ToInt32(hdnBodyDiagramID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientBodyDiagramHd(entity);
            }
            else
            {
                result = "0|There is no record to be deleted !";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        private bool IsValidToSave(ref string errMessage)
        {
            StringBuilder errMsg = new StringBuilder();
            if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
            {
                int paramedicID = Convert.ToInt32(hdnParamedicID.Value);
                if (AppSession.UserLogin.ParamedicID != paramedicID)
                {
                    errMsg.AppendLine("Perubahan Kajian Awal Pasien hanya dapat dilakukan oleh Dokter yang melakukan Pengkajian");
                }
            }
            if (string.IsNullOrEmpty(txtRemarks.Text))
            {
                errMsg.AppendLine("Uraian Pembedahan harus diisi.");
            }
            if (string.IsNullOrEmpty(txtDiagnosisText.Text))
            {
                errMsg.AppendLine("Pre Diagnosis (Text) harus diisi.");
            }

            errMessage = errMsg.ToString();

            return (errMessage == string.Empty);
        }

        #region Procedure Group
        private void BindGridViewProcedureGroup(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("PatientSurgeryID = {0} AND IsDeleted = 0", hdnReportID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientSurgeryProcedureGroupRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }

            List<vPatientSurgeryProcedureGroup> lstEntity = BusinessLayer.GetvPatientSurgeryProcedureGroupList(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex, "ProcedureGroupCode");

            grdProcedureGroupView.DataSource = lstEntity;
            grdProcedureGroupView.DataBind();
        }
        protected void cbpProcedureGroupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewProcedureGroup(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewProcedureGroup(1, false, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = string.Empty;
        }
        protected void cbpProcedureGroup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            IDbContext ctx = DbFactory.Configure(true);
            PatientSurgeryProcedureGroupDao procedureGroupDao = new PatientSurgeryProcedureGroupDao(ctx);

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    int orderID = 0;

                    if (param[0] == "add")
                    {
                        PatientSurgeryProcedureGroup obj = new PatientSurgeryProcedureGroup();

                        obj.PatientSurgeryID = Convert.ToInt32(hdnReportID.Value);
                        obj.ProcedureGroupID = Convert.ToInt32(hdnEntryProcedureGroupID.Value);
                        obj.CreatedBy = AppSession.UserLogin.UserID;
                        procedureGroupDao.Insert(obj);

                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnOrderDtProcedureGroupID.Value);
                        PatientSurgeryProcedureGroup entity = BusinessLayer.GetPatientSurgeryProcedureGroup(recordID);

                        if (entity != null)
                        {
                            entity.PatientSurgeryID = Convert.ToInt32(hdnReportID.Value);
                            entity.ProcedureGroupID = Convert.ToInt32(hdnEntryProcedureGroupID.Value);
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientSurgeryProcedureGroup(entity);
                            result = "1|edit|";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Jenis Tindakan Operasi tidak valid");
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnOrderDtProcedureGroupID.Value);
                        PatientSurgeryProcedureGroup entity = BusinessLayer.GetPatientSurgeryProcedureGroup(recordID);

                        if (entity != null)
                        {
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientSurgeryProcedureGroup(entity);
                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Jenis Tindakan Operasi tidak valid");
                        }
                        result = "1|delete|";
                    }
                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpDeleteDevice_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteDevice(param);
                string[] retValInfo = retVal.Split('|');
                if (retValInfo[0] == "1")
                    result += string.Format("success|{0}", string.Empty);
                else
                    result += string.Format("fail|{0}", retValInfo[1]);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = param;
        }

        private string DeleteDevice(string recordID)
        {
            string result = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);
            PatientMedicalDeviceDao deviceDao = new PatientMedicalDeviceDao(ctx);
            PatientChargesDtInfoDao infoDao = new PatientChargesDtInfoDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                PatientMedicalDevice obj = deviceDao.Get(Convert.ToInt32(id));
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    deviceDao.Update(obj);
                    if (obj.TransactionDtID > 0)
                    {
                        string filterMedicalMinOne = string.Format("TestOrderID = {0} AND TransactionDtID = {1} AND IsDeleted = 0 AND ID < {2}", obj.TestOrderID, obj.TransactionDtID, obj.ID);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        PatientMedicalDevice entityCheckMinOne = BusinessLayer.GetPatientMedicalDeviceList(filterMedicalMinOne, ctx).LastOrDefault();
                        PatientChargesDtInfo info = infoDao.Get(Convert.ToInt32(obj.TransactionDtID));
                        if (info != null)
                        {
                            if (entityCheckMinOne != null)
                            {
                                info.SerialNo = entityCheckMinOne.SerialNumber;
                            }
                            else
                            {
                                info.SerialNo = null;
                            }
                            info.LastUpdatedBy = AppSession.UserLogin.UserID;
                            infoDao.Update(info);
                        }
                    }
                    List<PatientMedicalDevice> lstDevice = BusinessLayer.GetPatientMedicalDeviceList(string.Format("MRN = {0} AND IsDeleted = 0", obj.MRN), ctx);

                    if (lstDevice.Count == 0)
                    {
                        Patient oPatient = patientDao.Get(obj.MRN);
                        if (oPatient.IsUsingImplant)
                        {
                            #region Update Patient Status - Using Implant
                            //oPatient.IsUsingImplant = false;
                            oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientDao.Update(oPatient);
                            #endregion
                        }

                    }
                    ctx.CommitTransaction();

                    result = string.Format("1|{0}", string.Empty);
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("0|{0}", ex.Message);
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }
        #endregion
    }
}
