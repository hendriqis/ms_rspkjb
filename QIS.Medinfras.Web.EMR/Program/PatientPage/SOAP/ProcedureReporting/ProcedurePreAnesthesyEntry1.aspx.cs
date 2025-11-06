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
    public partial class ProcedurePreAnesthesyEntry1 : BasePagePatientPageList
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

            hdnPatientChargesDtID.Value = paramInfo[0];
            txtTransactionNo.Text = paramInfo[1];
            txtTransactionNo.Enabled = false;
            hdnAssessmentID.Value = paramInfo[2];
            txtItemName.Text = paramInfo[3];
            txtProcedureGroupSummary.Enabled = false;

            Helper.SetControlEntrySetting(txtAnamnesisText, new ControlEntrySetting(true, true, true), "mpPatientStatus");

            txtServiceDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtServiceTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            txtStartFastingDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartFastingTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

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

            #region Pemeriksaan Fisik
            StringBuilder innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesyPhysicalExam.html");

            divFormContent1.InnerHtml = innerHtml.ToString();
            hdnPhysicalExamLayout.Value = innerHtml.ToString();
            #endregion

            #region Pemeriksaan Penunjang
            innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesydiagnosticTest.html");

            divFormContent2.InnerHtml = innerHtml.ToString();
            hdnDiagnosticTestCheckListLayout.Value = innerHtml.ToString();
            #endregion

            #region Rencana Anestesi
            innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "anesthesyPlanning.html");

            divFormContent3.InnerHtml = innerHtml.ToString();
            hdnAnesthesyPlanningLayout.Value = innerHtml.ToString();
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

            //vPreSurgeryAssessment obj1 = BusinessLayer.GetvPreSurgeryAssessmentList(string.Format("VisitID = {0} AND TestOrderID  = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnTestOrderID.Value)).FirstOrDefault();
            //if (obj1 != null)
            //{
            //    txtPreOpDiagnosisInfo.Text = string.Format("{0}", obj1.PreDiagnoseText);
            //}

            //List<vTestOrderDtProcedureGroup> lstProcedure = BusinessLayer.GetvTestOrderDtProcedureGroupList(string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", hdnVisitID.Value, hdnTestOrderID.Value));
            //if (lstProcedure.Count > 0)
            //{
            //    StringBuilder procedureList = new StringBuilder();
            //    foreach (vTestOrderDtProcedureGroup item in lstProcedure)
            //    {
            //        procedureList.AppendLine(item.ProcedureGroupName);
            //    }
            //    txtProcedureGroupSummary.Text = procedureList.ToString();
            //}
            vPreAnesthesyAssessment obj = BusinessLayer.GetvPreAnesthesyAssessmentList(string.Format("PreAnesthesyAssessmentID = {1} AND VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value, hdnAssessmentID.Value)).FirstOrDefault();
            if (obj != null)
            {
                if (obj.PreAnesthesyAssessmentID == 0)
                {
                    hdnAssessmentID.Value = "0";
                    hdnParamedicID.Value = "0";
                    _assessmentID = "0";

                    PopulateFormContent();
                }
                else
                {
                    _assessmentID = obj.PreAnesthesyAssessmentID.ToString();
                    hdnAssessmentID.Value = obj.PreAnesthesyAssessmentID.ToString();
                    hdnParamedicID.Value = obj.ParamedicID.ToString();
                    txtServiceDate.Text = obj.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtServiceTime.Text = obj.AssessmentTime;
                    txtAnamnesisText.Text = obj.PreAnesthesyAssessmentText;
                    txtPastSurgicalHistory.Text = obj.PastSurgicalHistory;
                    txtMedicationHistory.Text = obj.PastMedicationHistory;
                    chkIsPatientAllergyExists.Checked = !chkIsPatientAllergyExists.Checked;
                    chkAlloAnamnesis.Checked = obj.IsAlloAnamnesis;
                    chkAutoAnamnesis.Checked = obj.IsAutoAnamnesis;
                    cboFamilyRelation.Value = obj.GCFamilyRelation;
                    divFormContent1.InnerHtml = obj.PhysicalExamLayout;
                    hdnPhysicalExamLayout.Value = obj.PhysicalExamLayout;
                    hdnPhysicalExamValue.Value = obj.PhysicalExamValue;
                    txtDiagnosticResultSummary.Text = obj.DiagnosticResultSummary;
                    divFormContent2.InnerHtml = obj.DiagnosticTestLayout;
                    hdnDiagnosticTestCheckListLayout.Value = obj.DiagnosticTestLayout;
                    hdnDiagnosticTestCheckListValue.Value = obj.DiagnosticTestValue;
                    divFormContent3.InnerHtml = obj.AnesthesyPlanLayout;
                    hdnAnesthesyPlanningLayout.Value = obj.AnesthesyPlanLayout;
                    hdnAnesthesyPlanningValue.Value = obj.AnesthesyPlanValue;

                    if (!string.IsNullOrEmpty(obj.StartFastingTime))
                    {
                        txtStartFastingDate.Text = obj.StartFastingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtStartFastingTime.Text = obj.StartFastingTime; 
                    }
                    rblGCASAStatus.SelectedValue = obj.GCASAStatus;
                    chkIsASAStatusE.Checked = obj.IsASAStatusE;

                    chkIsGeneralAnesthesy.Checked = obj.IsGeneralAnesthesy;
                    chkIsLocal.Checked = obj.IsLocal;
                    chkIsRegional.Checked = obj.IsRegional;
                    chkIsSedation.Checked = obj.IsSedation;
                    cboRegionalAnesthesiaType.Value = obj.GCRegionalAnesthesiaType;

                    rblIsUsePremedication.SelectedValue = obj.IsUsePremedication ? "1" : "0";
                    txtPremedication.Text = obj.Premedication;
                    txtPremedication.Enabled = obj.IsUsePremedication;

                    rblIsSedativePlan.SelectedValue = obj.IsSedativePlan ? "1" : "0";
                    txtSedativeMedication.Text = obj.SedativeMedication;
                    txtSedativeMedication.Enabled = obj.IsSedativePlan;

                    rblIsMaintenancePlan.SelectedValue = obj.IsMaintenancePlan ? "1" : "0";
                    txtMaintenanceMedication.Text = obj.MaintenanceMedication;
                    txtMaintenanceMedication.Enabled = obj.IsMaintenancePlan;

                    rblIsRegionalAnesthesy.SelectedValue = obj.IsRegionalAnesthesyPlan ? "1" : "0";
                    txtRegionalAnesthesyMedication.Text = obj.RegionalAnesthesyMedicationPlan;
                    txtRegionalAnesthesyMedication.Enabled = obj.IsRegionalAnesthesyPlan;

                    rblIsLocalAnesthesy.SelectedValue = obj.IsLocalAnesthesyPlan ? "1" : "0";
                    txtLocalAnesthesyMedication.Text = obj.LocalAnesthesyMedicationPlan;
                    txtLocalAnesthesyMedication.Enabled = obj.IsLocalAnesthesyPlan;


                    chkIsAsthma.Checked = obj.IsHasAsthma;

                    hdnIsChanged.Value = "0";
                    hdnIsSaved.Value = "0";
                }
            }
            else
            {
                PopulateFormContent();
                _assessmentID = "0";
            }
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}', '{2}') AND IsActive = 1 AND IsDeleted = 0",
    Constant.StandardCode.ALLERGEN_TYPE, Constant.StandardCode.FAMILY_RELATION, Constant.StandardCode.REGIONAL_ANESTHESIA_TYPE);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);

            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboAllergenType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ALLERGEN_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboFamilyRelation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.FAMILY_RELATION).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboRegionalAnesthesiaType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.REGIONAL_ANESTHESIA_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            cboAllergenType.Value = Constant.AllergenType.DRUG;
        }

        private void UpdateAssessment(IDbContext ctx)
        {
            PreAnesthesyAssessmentDao assessmentDao = new PreAnesthesyAssessmentDao(ctx);
            PreAnesthesyAssessment entity = null;
            bool isNewAssessment = true;

            if (hdnAssessmentID.Value != "" && hdnAssessmentID.Value != "0")
            {
                entity = assessmentDao.Get(Convert.ToInt32(hdnAssessmentID.Value));
                isNewAssessment = false;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            }
            else
            {
                entity = new PreAnesthesyAssessment();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.PatientChargesDtID = Convert.ToInt32(hdnPatientChargesDtID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }

            entity.AssessmentDate = Helper.GetDatePickerValue(txtServiceDate);
            entity.AssessmentTime = txtServiceTime.Text;
            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            entity.PreAnesthesyAssessmentText = txtAnamnesisText.Text;
            entity.PastMedicationHistory = txtMedicationHistory.Text;
            entity.PastSurgicalHistory = txtPastSurgicalHistory.Text;

            entity.PhysicalExamLayout = hdnPhysicalExamLayout.Value;
            entity.PhysicalExamValue = hdnPhysicalExamValue.Value;
            entity.DiagnosticResultSummary = txtDiagnosticResultSummary.Text;
            entity.DiagnosticTestLayout = hdnDiagnosticTestCheckListLayout.Value;
            entity.DiagnosticTestValue = hdnDiagnosticTestCheckListValue.Value;
            if (!string.IsNullOrEmpty(txtStartFastingTime.Text))
            {
                entity.StartFastingDate = Helper.GetDatePickerValue(txtStartFastingDate);
                entity.StartFastingTime = txtStartFastingTime.Text;
            }
            entity.GCASAStatus = rblGCASAStatus.SelectedValue;
            entity.IsASAStatusE = chkIsASAStatusE.Checked;
            entity.IsGeneralAnesthesy = chkIsGeneralAnesthesy.Checked;
            entity.IsLocal = chkIsLocal.Checked;
            entity.IsRegional = chkIsRegional.Checked;
            entity.IsSedation = chkIsSedation.Checked;

            if (cboRegionalAnesthesiaType.Value != null)
            {
                entity.GCRegionalAnesthesiaType = cboRegionalAnesthesiaType.Value.ToString();
            }

            entity.IsUsePremedication = rblIsUsePremedication.SelectedValue == "1" ? true : false;
            if (entity.IsUsePremedication)
            {
                entity.Premedication = txtPremedication.Text;
            }
            else
            {
                entity.Premedication = null;
            }

            entity.IsSedativePlan = rblIsSedativePlan.SelectedValue == "1" ? true : false;
            if (entity.IsSedativePlan)
            {
                entity.SedativeMedication = txtSedativeMedication.Text;
            }
            else
            {
                entity.SedativeMedication = null;
            }

            entity.IsMaintenancePlan = rblIsMaintenancePlan.SelectedValue == "1" ? true : false;
            if (entity.IsMaintenancePlan)
            {
                entity.MaintenanceMedication = txtMaintenanceMedication.Text;
            }
            else
            {
                entity.MaintenanceMedication = null;
            }

            entity.IsRegionalAnesthesyPlan = rblIsRegionalAnesthesy.SelectedValue == "1" ? true : false;
            if (entity.IsRegionalAnesthesyPlan)
            {
                entity.RegionalAnesthesyMedicationPlan = txtRegionalAnesthesyMedication.Text;
            }
            else
            {
                entity.RegionalAnesthesyMedicationPlan = null;
            }

            entity.IsLocalAnesthesyPlan = rblIsLocalAnesthesy.SelectedValue == "1" ? true : false;
            if (entity.IsLocalAnesthesyPlan)
            {
                entity.LocalAnesthesyMedicationPlan = txtLocalAnesthesyMedication.Text;
            }
            else
            {
                entity.LocalAnesthesyMedicationPlan = null;
            }

            entity.AnesthesyPlanLayout = hdnAnesthesyPlanningLayout.Value;
            entity.AnesthesyPlanValue = hdnAnesthesyPlanningValue.Value;
            entity.IsPatientAllergyExists = !chkIsPatientAllergyExists.Checked;
            entity.IsAutoAnamnesis = Convert.ToBoolean(chkAutoAnamnesis.Checked);
            entity.IsAlloAnamnesis = Convert.ToBoolean(chkAlloAnamnesis.Checked);
            entity.IsHasAsthma = chkIsAsthma.Checked;

            if (entity.IsAlloAnamnesis)
            {
                if (cboFamilyRelation.Value != null)
                {
                    entity.GCFamilyRelation = cboFamilyRelation.Value.ToString();
                } 
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
                    PreAnesthesyAssessment obj = BusinessLayer.GetPreAnesthesyAssessmentList(string.Format("VisitID = {0} AND PatientChargesDtID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnPatientChargesDtID.Value), ctx).FirstOrDefault();
                    if (obj != null)
                        hdnAssessmentID.Value = obj.ID.ToString();
                    else
                        hdnAssessmentID.Value = "0";

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

            filterExpression += string.Format("VisitID IN ({0},{1}) AND PreAnesthesyAssessmentID = {2} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, linkedVisitID, _assessmentID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0},{1}) AND PreAnesthesyAssessmentID = {2} AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, linkedVisitID, _assessmentID));
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
                        oAllergy.GCAllergenType = cboAllergenType.Value.ToString();
                        oAllergy.Allergen = txtAllergenName.Text;
                        oAllergy.GCAllergySource = Constant.AllergenFindingSource.PATIENT;
                        oAllergy.GCAllergySeverity = Constant.AllergySeverity.UNKNOWN;
                        oAllergy.KnownDate = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);
                        oAllergy.Reaction = txtReaction.Text;
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
                            oAllergy.GCAllergenType = cboAllergenType.Value.ToString();
                            oAllergy.Allergen = txtAllergenName.Text;
                            oAllergy.Reaction = txtReaction.Text;
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

        protected void cbpDeleteTestOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (e.Parameter != null && e.Parameter != "")
            {
                string testOrderID = "0";

                switch (e.Parameter)
                {
                    case "LB":
                        testOrderID = hdnLaboratoryTestOrderID.Value;
                        break;
                    case "IS":
                        testOrderID = hdnImagingTestOrderID.Value;
                        break;
                    default:
                        testOrderID = hdnDiagnosticTestOrderID.Value;
                        break;
                }

                if (testOrderID != "0")
                {
                    TestOrderHd entity = BusinessLayer.GetTestOrderHd(Convert.ToInt32(testOrderID));
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateTestOrderHd(entity);
                    result = string.Format("1|{0}", e.Parameter);
                }
                else
                {
                    result = string.Format("0|{0}|There is no record to be deleted !");
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpSendOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            int transactionID = Convert.ToInt32(param[2]);
            result = param[0] + "|" + param[1] + "|";
            try
            {
                if (param[0] == "sendOrder")
                {
                    if (param[1] != "PH")
                    {
                        TestOrderHd entity = BusinessLayer.GetTestOrderHdList(String.Format("TestOrderID = {0}", transactionID))[0];
                        if (entity.GCTransactionStatus != Constant.TransactionStatus.CLOSED)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            BusinessLayer.UpdateTestOrderHd(entity);
                            try
                            {
                                HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(entity.HealthcareServiceUnitID));
                                string ipAddress = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                                if (!String.IsNullOrEmpty(ipAddress))
                                {
                                    SendNotification(entity, ipAddress);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                result += string.Format("success|{0}", errMessage);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID;
        }

        private void SendNotification(TestOrderHd order, string ipAddress)
        {
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.AppendLine(string.Format("No  : {0}", order.TestOrderNo));
            sbMessage.AppendLine(string.Format("Fr  : {0}", string.Format("{0} ({1})", AppSession.RegisteredPatient.ServiceUnitName, AppSession.UserLogin.UserFullName)));
            sbMessage.AppendLine(string.Format("Px  : {0}", AppSession.RegisteredPatient.PatientName));
            sbMessage.AppendLine(string.Format("PDx :    "));
            sbMessage.AppendLine(string.Format("{0}", order.Remarks));
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(ipAddress), 6000);
            NetworkStream stream = client.GetStream();
            using (BinaryWriter w = new BinaryWriter(stream))
            {
                using (BinaryReader r = new BinaryReader(stream))
                {
                    w.Write(string.Format(@"{0}", sbMessage.ToString()).ToCharArray());
                }
            }
        }

        private bool IsValidToSave(ref string errMessage)
        {
            StringBuilder errMsg = new StringBuilder();
            if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
            {
                int paramedicID = Convert.ToInt32(hdnParamedicID.Value);
                if (AppSession.UserLogin.ParamedicID != paramedicID)
                {
                    errMsg.AppendLine("Perubahan Kajian Pasien hanya dapat dilakukan oleh Dokter yang melakukan Pengkajian");
                }
            }
            //if (string.IsNullOrEmpty(txtAnamnesisText.Text))
            //{
            //    errMsg.AppendLine("Anamnesis (Data Subjektif) harus diisi.");
            //}

            errMessage = errMsg.ToString();

            return (errMessage == string.Empty);
        }
    }
}
