using System;
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
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ServiceOrderDetail : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EmergencyCare.SERVICE_ORDER_TRANS;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED);
            IsAllowVoid = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID));
                hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;

                string[] param = Page.Request.QueryString["id"].Split('|');

                if (param[0] == "to")
                {
                    hdnVisitID.Value = param[1];
                    hdnPrescriptionOrderID.Value = param[2];
                }
                else {
                    hdnVisitID.Value = param[0];
                    btnClinicTransactionTestOrder.Style.Add("display", "none");
                }
                
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
               
                String filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.REFILL_INSTRUCTION);
                List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
                Methods.SetComboBoxField<StandardCode>(cboRefillInstruction, lstStandardCode, "StandardCodeName", "StandardCodeID");

                Location location = BusinessLayer.GetLocationList(string.Format("LocationID IN (SELECT LocationID FROM HealthcareServiceUnit WHERE HealthcareServiceUnitID IN (SELECT DispensaryServiceUnitID FROM HealthcareServiceUnit WHERE HealthcareServiceUnitID = {0}))", entity.HealthcareServiceUnitID)).FirstOrDefault();

                int locationID = location.LocationID;
                if (locationID > 0)
                {
                    Location loc = BusinessLayer.GetLocation(locationID);
                    List<Location> lstLocation = null;
                    if (loc.IsHeader)
                        lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                    else
                    {
                        lstLocation = new List<Location>();
                        lstLocation.Add(loc);
                    }
                    Methods.SetComboBoxField<Location>(cboLocation, lstLocation, "LocationName", "LocationID");
                    cboLocation.SelectedIndex = 0;
                }
                /*
                String filterExpression2 = string.Format("IsDeleted = 0");
                List<Location> lstLocation = BusinessLayer.GetLocationList(filterExpression2);
                Methods.SetComboBoxField<Location>(cboLocation, lstLocation, "LocationName", "LocationID");
                */
                 
                ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
                hdnDepartmentID.Value = entity.DepartmentID;
                hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnClassID.Value = entity.ClassID.ToString();
                

                hdnPhysicianID.Value = entity.ParamedicID.ToString();
                txtPhysicianCode.Text = entity.ParamedicCode;
                txtPhysicianName.Text = entity.ParamedicName;

                BindGridView();

                Helper.SetControlEntrySetting(txtPrescriptionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtPrescriptionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)), "mpTrxPopup");
                Helper.SetControlEntrySetting(cboRefillInstruction, new ControlEntrySetting(true, false, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtDrugName, new ControlEntrySetting(true, false, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtPrescriptionDate, new ControlEntrySetting(true, false, true), "mpTrxPopup");

                Helper.SetControlEntrySetting(txtGenericName, new ControlEntrySetting(true, true, false), "mpTrxPopup");
                Helper.SetControlEntrySetting(cboForm, new ControlEntrySetting(true, true, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtStrengthAmount, new ControlEntrySetting(true, true, false), "mpTrxPopup");
                Helper.SetControlEntrySetting(cboStrengthUnit, new ControlEntrySetting(true, true, false), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(cboFrequencyTimeline, new ControlEntrySetting(true, true, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(cboDosingUnit, new ControlEntrySetting(true, true, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtPurposeOfMedication, new ControlEntrySetting(true, true, false), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            }   

        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnPrescriptionID.Value != "")
                filterExpression = string.Format("PrescriptionID = {0} AND IsDeleted = 0 ORDER BY PrescriptionDetailID DESC", hdnPrescriptionID.Value);
            List<vPrescriptionDt> lstEntity = BusinessLayer.GetvPrescriptionDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
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

        protected override void SetControlProperties()
        {
            List<StandardCode> lstToBePerformed = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.TO_BE_PERFORMED));
           // Methods.SetComboBoxField<StandardCode>(cboToBePerformed, lstToBePerformed, "StandardCodeName", "StandardCodeID");
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0", Constant.StandardCode.DRUG_FORM, Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboStrengthUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");

            cboForm.SelectedIndex = 1;
            cboFrequencyTimeline.SelectedIndex = 1;
            cboStrengthUnit.SelectedIndex = 1;
            cboDosingUnit.SelectedIndex = 1;
            cboMedicationRoute.SelectedIndex = 1;

            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString("HH:mm");

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnPrescriptionID, new ControlEntrySetting(false, false, false, "0"));

            SetControlEntrySetting(txtPrescriptionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPrescriptionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboRefillInstruction, new ControlEntrySetting(true, false, true));
            //SetControlEntrySetting(txtDrugCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false, true));
        }

        public override void OnAddRecord()
        {
            
            //txtPrescriptionDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            //txtPrescriptionTime.Text = DateTime.Now.ToString("HH:mm"); 
            //chkIsRx.Checked = false;
            //txtGenericName.Text = "";
            //cboLocation.SelectedIndex = 0;
            //txtDrugCode.Text = "";
            //txtDrugName.Text = "";
            //txtStrengthAmount.Text = "";
            //cboStrengthUnit.SelectedIndex = 0;
            //cboForm.SelectedIndex = 0;
            //txtPurposeOfMedication.Text = "";
            //txtFrequencyNumber.Text = "";
            //cboFrequencyTimeline.SelectedIndex = 0;
            //txtDosingDose.Text = "";
            //cboDosingUnit.SelectedIndex = 0;
            //cboMedicationRoute.SelectedIndex = 0;
            //txtDispenseQty.Text = "";
            //txtMedicationAdministration.Text = "";
        }

        #region Load Entity
        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            return BusinessLayer.GetvPrescriptionHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            vPrescriptionHd entity = BusinessLayer.GetvPrescriptionHd(filterExpression, PageIndex, " PrescriptionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            PageIndex = BusinessLayer.GetvPrescriptionHdRowIndex(filterExpression, keyValue, "PrescriptionID DESC");
            vPrescriptionHd entity = BusinessLayer.GetvPrescriptionHd(filterExpression, PageIndex, "PrescriptionID DESC");
            
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPrescriptionHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                //watermarkText = entity.TransactionStatusWatermark;
            }
            hdnPrescriptionID.Value = entity.PrescriptionID.ToString();
            txtPrescriptionNo.Text = entity.PrescriptionNo;
            txtPrescriptionDate.Text = entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entity.PrescriptionTime;
            cboRefillInstruction.Value = entity.GCRefillInstruction;
            //hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            //txtServiceUnitCode.Text = entity.ServiceUnitCode;
            //txtServiceUnitName.Text = entity.ServiceUnitName;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            ParamedicMaster PM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entity.ParamedicID)).FirstOrDefault();
            txtPhysicianCode.Text = PM.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            
            BindGridView();
        }

        private void ControlToEntity(PrescriptionDt entity)
        {
            entity.IsRFlag = chkIsRx.Checked;
            if (hdnDrugID.Value.ToString() != "")
                entity.ItemID = Convert.ToInt32(hdnDrugID.Value);
            else
                entity.ItemID = null;
            entity.DrugName = hdnDrugName.Value;
            entity.GenericName = txtGenericName.Text;
            entity.GCDrugForm = cboForm.Value.ToString();
            if (cboStrengthUnit.Value != null && cboStrengthUnit.Value.ToString() != "")
            {
                entity.Dose = Convert.ToDecimal(txtStrengthAmount.Text);
                entity.GCDoseUnit = cboStrengthUnit.Value.ToString();
            }
            else
            {
                entity.Dose = null;
                entity.GCDoseUnit = null;
            }
            entity.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            entity.GCDosingUnit = cboDosingUnit.Value.ToString();
            entity.GCRoute = cboMedicationRoute.Value.ToString();
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.StartTime = txtStartTime.Text;
            entity.MedicationPurpose = txtPurposeOfMedication.Text;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
        }
        #endregion

        #region Save Entity
        public void SavePrescriptionHd(IDbContext ctx, ref int prescriptionID)
        {
            PrescriptionHdDao entityHdDao = new PrescriptionHdDao(ctx);
            PrescriptionDtDao entityDtDao = new PrescriptionDtDao(ctx);
            PrescriptionHd entityHd = null;
            if (hdnPrescriptionID.Value == "0")
            {
                entityHd = new PrescriptionHd();
                entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.PrescriptionDate = Helper.GetDatePickerValue(txtPrescriptionDate);
                entityHd.PrescriptionTime = txtPrescriptionTime.Text;
                entityHd.ClassID = Convert.ToInt32(hdnClassID.Value);
                entityHd.GCPrescriptionType = Constant.PrescriptionType.DISCHARGE_PRESCRIPTION;
                entityHd.GCRefillInstruction = cboRefillInstruction.Value.ToString();
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_OUTPATIENT;
                entityHd.PrescriptionNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHdDao.Insert(entityHd);

                prescriptionID = BusinessLayer.GetPrescriptionHdMaxID(ctx);
            }
            else
            {
                prescriptionID = Convert.ToInt32(hdnPrescriptionID.Value);
            }
            //PrescriptionDt
            
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int PrescriptionID = 0;
                SavePrescriptionHd(ctx, ref PrescriptionID);
                
                retval = PrescriptionID.ToString();
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                //PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionHdID.Value));
                //entity.ReferenceNo = txtReferenceNo.Text;
                //entity.TransactionDate = Helper.GetDatePickerValue(txtTransactionDate.Text);
                //entity.TransactionTime = txtTransactionTime.Text;
                //BusinessLayer.UpdatePatientChargesHd(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion
        
        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int prescriptionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    prescriptionID = Convert.ToInt32(hdnPrescriptionID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref prescriptionID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                prescriptionID = Convert.ToInt32(hdnEntryID.Value);
                if (OnDeleteEntityDt(ref errMessage, prescriptionID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpPrescriptionID"] = prescriptionID.ToString();
        }

        private void ControlToEntity(TestOrderDt entityDt)
        {
            //entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            //entityDt.DiagnoseID = txtDiagnoseID.Text;
           // entityDt.GCToBePerformed = cboToBePerformed.Value.ToString();
           // if (entityDt.GCToBePerformed == Constant.ToBePerformed.SCHEDULLED)
           //     entityDt.PerformedDate = Helper.GetDatePickerValue(txtPerformDate);
           // entityDt.Remarks = txtRemarks.Text;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int prescriptionID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionDtDao entityDtDao = new PrescriptionDtDao(ctx);
            try
            {
                SavePrescriptionHd(ctx, ref prescriptionID);
                PrescriptionDt entityDt = new PrescriptionDt();
                ControlToEntity(entityDt);
                //entityDt.TestOrderID = testOrderID;
                entityDt.PrescriptionID = prescriptionID;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                //entityDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                //entityDt.ItemQty = 1;
               // entityDt.ItemUnit = hdnGCItemUnit.Value;
                entityDtDao.Insert(entityDt);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionDtDao entityDtDao = new PrescriptionDtDao(ctx);
            try
            {
                PrescriptionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entityDt);
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionDtDao entityDtDao = new PrescriptionDtDao(ctx);
            try
            {
                PrescriptionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                entityDt.IsDeleted = true;
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
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