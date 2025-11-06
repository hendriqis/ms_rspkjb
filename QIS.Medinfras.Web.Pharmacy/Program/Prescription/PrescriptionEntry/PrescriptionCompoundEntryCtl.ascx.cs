using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Text;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using System.Data;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrescriptionCompoundEntryCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            hdnQueryString.Value = queryString;
            string[] param = queryString.Split('|');

            IsAdd = true;
            if (param[0] == "add")
            {
                hdnPrescriptionOrderID.Value = param[1];
                hdnTransactionID.Value = param[2];
                hdnVisitID.Value = param[7];
                hdnPrescriptionFeeAmount.Value = param[8];
                hdnLocationID.Value = param[9];
                hdnPhysicianID.Value = param[10];
                hdnDispensaryServiceUnitID.Value = param[11];
                hdnDepartmentID.Value = param[12];
                hdnImagingServiceUnitID.Value = param[13];
                hdnLaboratoryServiceUnitID.Value = param[14];

                vConsultVisit entityConsultVisit = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0}", hdnVisitID.Value))[0];
                hdnChargeClassID.Value = entityConsultVisit.ChargeClassID.ToString();
                hdnRegistrationID.Value = entityConsultVisit.RegistrationID.ToString();
                SetControlProperties();
            }
            else
            {
                hdnPrescriptionOrderID.Value = param[1];
                hdnTransactionID.Value = param[2];
                hdnParentID.Value = param[3];
                hdnVisitID.Value = param[4];
                hdnPrescriptionFeeAmount.Value = param[5];
                hdnLocationID.Value = param[6];
                hdnPhysicianID.Value = param[7];
                hdnDispensaryServiceUnitID.Value = param[8];
                hdnDepartmentID.Value = param[9];
                hdnImagingServiceUnitID.Value = param[10];
                hdnLaboratoryServiceUnitID.Value = param[11];
           
                SetControlProperties();
                vConsultVisit entityConsultVisit = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0}", hdnVisitID.Value))[0];
                hdnChargeClassID.Value = entityConsultVisit.ChargeClassID.ToString();
                hdnRegistrationID.Value = entityConsultVisit.RegistrationID.ToString();

                if (hdnParentID.Value != "")
                {
                    List<vPrescriptionOrderDt> lstEntity = BusinessLayer.GetvPrescriptionOrderDtList(String.Format("PrescriptionOrderDetailID = {0}", hdnParentID.Value));
                    if(lstEntity.Count > 0) EntityToControl(lstEntity[0]);
                }
            }
            if (hdnTransactionID.Value != "" && hdnTransactionID.Value != "0")
                hdnTransactionNo.Value = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionID.Value)).TransactionNo;

            BindListView();

            Helper.SetControlEntrySetting(txtGenericName, new ControlEntrySetting(true, true, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemName1, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDose, new ControlEntrySetting(true, true, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDoseUnit, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtCompoundQty, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboCompoundUnit, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtConversionFactor, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboPopupChargeClass, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtPopupPatientAmount, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtPopupPayerAmount, new ControlEntrySetting(true, true, true), "mpTrxPopup");
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        #region Bind List View
        private void BindListView() 
        {
            string[] param = hdnQueryString.Value.Split('|');
            String filterExpression = "";

            if (hdnPrescriptionOrderID.Value == "0" || hdnPrescriptionOrderID.Value == "")
                filterExpression = "1 = 0";
            else
            {
                if (hdnParentID.Value != "")
                    filterExpression = string.Format("PrescriptionOrderID = {0} AND (PrescriptionOrderDetailID = {1} OR ParentID = {1}) AND IsCompound = 1 AND IsDeleted = 0", hdnPrescriptionOrderID.Value, hdnParentID.Value);
                else
                    filterExpression = string.Format("PrescriptionOrderID = {0} AND PrescriptionOrderDetailID NOT IN (SELECT PrescriptionOrderDetailID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = {0} AND IsCompound = 1 AND IsDeleted = 0) AND IsCompound = 1 AND IsDeleted = 0", hdnPrescriptionOrderID.Value);
            }

            List<vPrescriptionOrderDt> lstEntity = BusinessLayer.GetvPrescriptionOrderDtList(filterExpression);
            if (lstEntity.Count == 1)
                hdnIsEditable.Value = "0";
            else
                hdnIsEditable.Value = "1";
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            if (lstEntity.Count > 0)
            {
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAllPayer")).InnerHtml = lstEntity.Sum(x => x.PayerAmount).ToString("N");
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAllPatient")).InnerHtml = lstEntity.Sum(x => x.PatientAmount).ToString("N");
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAll")).InnerHtml = lstEntity.Sum(x => x.LineAmount).ToString("N");
            }
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindListView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtCompoundDrugName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnPopupSignaID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPopupSignaLabel, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPopupSignaName1, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboPopupDrugForm, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboPopupCoenamRule, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnEmbalaceID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtEmbalaceCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmbalaceName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtEmbalaceQty, new ControlEntrySetting(true, true, true, "0"));

            SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboPopupFrequencyTimeline, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTakenQty, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUsingSweetener, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(vPrescriptionOrderDt entity)
        {
            txtDispenseQty.Text = entity.DispenseQtyInString;
            txtDispenseUnit.Text = entity.DrugForm;
            txtTakenQty.Text = entity.TakenQtyInString;
            txtTakenUnit.Text = entity.DrugForm;
            txtCompoundDrugName.Text = entity.CompoundDrugname;
            cboMedicationRoute.Value = entity.GCRoute;
            chkIsUsingSweetener.Checked = entity.IsUseSweetener;
            cboPopupCoenamRule.Value = entity.GCCoenamRule;
            cboPopupFrequencyTimeline.Value = entity.GCDosingFrequency;
            txtFrequencyNumber.Text = entity.Frequency.ToString();
            //cboCompoundUnit.Value = entity.GCCompoundUnit.ToString();
            cboPopupDrugForm.Value = entity.GCDrugForm;
            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = entity.StartTime;

            txtMedicationAdministration.Text = entity.MedicationAdministration;
            txtDosingDuration.Text = entity.DosingDuration.ToString();
            txtDosingDurationTimeline.Text = cboPopupFrequencyTimeline.Text;
            if (entity.SignaID != 0 && entity.SignaID.ToString() != "")
            {
                vSigna signa = BusinessLayer.GetvSignaList(String.Format("SignaID = {0}", entity.SignaID))[0];
                txtPopupSignaLabel.Text = signa.SignaLabel;
                txtPopupSignaName1.Text = signa.SignaName1;
                hdnPopupSignaID.Value = entity.SignaID.ToString();
            }
            txtDosingDose.Text = entity.NumberOfDosageInString;
            txtDispenseUnit.Text = entity.DrugForm;
            cboPopupCoenamRule.Value = entity.GCCoenamRule;
            if (entity.EmbalaceID > 0) 
            {
                EmbalaceHd entityEmbalace = BusinessLayer.GetEmbalaceHd(Convert.ToInt32(entity.EmbalaceID));
                hdnEmbalaceID.Value = entity.EmbalaceID.ToString();
                txtEmbalaceCode.Text = entityEmbalace.EmbalaceCode;
                txtEmbalaceName.Text = entityEmbalace.EmbalaceName;
                txtEmbalaceQty.Text = entity.EmbalaceQty.ToString();
                hdnEmbalaceAmount.Value = entity.EmbalaceAmount.ToString();
            }
        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.DRUG_FORM, Constant.StandardCode.COENAM_RULE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPopupFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPopupDrugForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            
            Methods.SetComboBoxField<StandardCode>(cboPopupCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<ClassCare>(cboPopupChargeClass, lstClassCare, "ClassName", "ClassID");
            cboPopupChargeClass.Value = 1;
            cboPopupFrequencyTimeline.Value = Constant.DosingFrequency.DAY;
            txtDosingDurationTimeline.Text = cboPopupFrequencyTimeline.Text;

            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString("HH:mm");

            
            txtDispenseQty.Text = "1";
        }

        public void SavePrescriptionOrderHd(IDbContext ctx, ref int transactionID, ref string transactionNo, ref int prescriptionOrderID)
        {
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PatientChargesHdDao entityChargesHdDao = new PatientChargesHdDao(ctx);
            PrescriptionOrderHd entityHd = null;

            string[] paramHeader = hdnQueryString.Value.ToString().Split('|');
            if (hdnPrescriptionOrderID.Value == "" || hdnPrescriptionOrderID.Value == "0")
            {
                #region Prescription Order
                DateTime prescriptionDate = Helper.DateInStringToDateTime(paramHeader[3]);
                string prescriptionTime = paramHeader[4];
                int paramedicID = Convert.ToInt32(paramHeader[5]);
                string refillInstruction = paramHeader[6];

                entityHd = new PrescriptionOrderHd();
                entityHd.ParamedicID = paramedicID;
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.PrescriptionDate = prescriptionDate;
                entityHd.PrescriptionTime = prescriptionTime;
                entityHd.ClassID = Convert.ToInt32(hdnChargeClassID.Value);
                entityHd.DispensaryServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                entityHd.LocationID = Convert.ToInt32(hdnLocationID.Value);
                entityHd.GCPrescriptionType = Constant.PrescriptionType.DISCHARGE_PRESCRIPTION;
                entityHd.GCRefillInstruction = refillInstruction;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.EMERGENCY:
                        entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                        break;
                    case Constant.Facility.OUTPATIENT:
                        entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                        break;
                    case Constant.Facility.INPATIENT:
                        entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                        break;
                    case Constant.Facility.PHARMACY:
                        entityHd.TransactionCode = Constant.TransactionCode.PH_MEDICATION_ORDER;
                        break;
                    case Constant.Facility.MEDICAL_CHECKUP:
                        entityHd.TransactionCode = Constant.TransactionCode.MCU_MEDICATION_ORDER;
                        break;
                    case Constant.Facility.DIAGNOSTIC:
                        //if (hdnHealthcareServiceUnitID.Value == hdnImagingServiceUnitID.Value)
                        //    entityHd.TransactionCode = Constant.TransactionCode.IMAGING_MEDICATION_ORDER;
                        //else if (hdnHealthcareServiceUnitID.Value == hdnLaboratoryServiceUnitID.Value)
                        //    entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_MEDICATION_ORDER;
                        //else
                            entityHd.TransactionCode = Constant.TransactionCode.OTHER_MEDICATION_ORDER;
                        break;
                }
                entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHdDao.Insert(entityHd);

                prescriptionOrderID = BusinessLayer.GetPrescriptionOrderHdMaxID(ctx);
                #endregion

                #region PatientChargesHd
                PatientChargesHd entityPatientChargesHd = new PatientChargesHd();
                entityPatientChargesHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityPatientChargesHd.TransactionDate = Helper.GetDatePickerValue(txtStartDate);
                entityPatientChargesHd.TransactionTime = txtStartTime.Text;
                entityPatientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityPatientChargesHd.PrescriptionOrderID = prescriptionOrderID;
                entityPatientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                //switch (hdnDepartmentID.Value)
                //{
                //    case Constant.Facility.EMERGENCY:
                //        entityPatientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES;
                //        break;
                //    case Constant.Facility.OUTPATIENT:
                //        entityPatientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES;
                //        break;
                //    case Constant.Facility.INPATIENT:
                //        entityPatientChargesHd.TransactionCode = Constant.TransactionCode.IP_CHARGES;
                //        break;
                //    case Constant.Facility.DIAGNOSTIC:
                //        if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() == hdnImagingServiceUnitID.Value)
                //            entityPatientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                //        else if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() == hdnLaboratoryServiceUnitID.Value)
                //            entityPatientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                //        break;
                //}
                entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PH_CHARGES;
                entityPatientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityPatientChargesHd.TransactionCode, entityPatientChargesHd.TransactionDate, ctx);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityPatientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                entityChargesHdDao.Insert(entityPatientChargesHd);
                transactionID = BusinessLayer.GetPatientChargesHdMaxID(ctx);
                transactionNo = entityPatientChargesHd.TransactionNo;
                #endregion
            }
            else
            {
                transactionNo = hdnTransactionNo.Value;
                prescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                transactionID = Convert.ToInt32(hdnTransactionID.Value);
            }

        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            try
            {
                int transactionID = 0;
                int prescriptionOrderID = 0;
                string transactionNo = "";
                if (hdnParentID.Value != "")
                {
                    SavePrescriptionOrderHd(ctx, ref transactionID, ref transactionNo, ref prescriptionOrderID);

                    List<PrescriptionOrderDt> lstEntity = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderDetailID = {0} OR ParentID = {0} AND IsDeleted = 0", hdnParentID.Value), ctx);
                    StringBuilder sbLstPresciptionOrderDtID = new StringBuilder();
                    foreach (PrescriptionOrderDt entity in lstEntity)
                    {
                        if (sbLstPresciptionOrderDtID.ToString() != "")
                            sbLstPresciptionOrderDtID.Append(",");
                        sbLstPresciptionOrderDtID.Append(entity.PrescriptionOrderDetailID);
                    }

                    List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("PrescriptionOrderDetailID IN ({0})", sbLstPresciptionOrderDtID.ToString()), ctx);
                    foreach (PrescriptionOrderDt entity in lstEntity)
                    {
                        PatientChargesDt patientChargesDt = lstPatientChargesDt.FirstOrDefault(p => p.PrescriptionOrderDetailID == entity.PrescriptionOrderDetailID);
                        ControlToEntityCompoundHd(entity, patientChargesDt);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entity);
                        patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityChargesDtDao.Update(patientChargesDt);
                    }
                    retval = transactionNo;
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Silahkan menambahkan obat dengan menekan label Add Data, karena paling tidak harus ada 1 obat terpilih";
                }
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

        #region Process Detail
        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int transactionID = 0;
            int prescriptionOrderID = 0;
            int parentID = 0;
            string transactionNo = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                transactionID = Convert.ToInt32(hdnTransactionID.Value);
                if (hdnPrescriptionOrderID.Value != "")
                    prescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                if (hdnParentID.Value != "")
                    parentID = Convert.ToInt32(hdnParentID.Value);
                transactionNo = hdnTransactionNo.Value;
                if (hdnPrescriptionOrderDetailID.Value.ToString() != "")
                {
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref transactionID, ref prescriptionOrderID, ref parentID, ref transactionNo))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                transactionID = Convert.ToInt32(hdnTransactionID.Value);
                prescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                parentID = Convert.ToInt32(hdnParentID.Value);
                if (OnDeleteEntityDt(ref errMessage, ref parentID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpParentID"] = parentID.ToString();
            panel.JSProperties["cpPrescriptionOrderID"] = prescriptionOrderID.ToString();
            panel.JSProperties["cpTransactionID"] = transactionID.ToString();
            panel.JSProperties["cpTransactionNo"] = transactionNo;
        }

        private void ControlToEntityCompoundHd(PrescriptionOrderDt entity, PatientChargesDt entityChargesDt)
        {
            entity.CompoundDrugname = txtCompoundDrugName.Text;
            entity.GCDrugForm = cboPopupDrugForm.Value.ToString();

            entity.GCDosingFrequency = cboPopupFrequencyTimeline.Value.ToString();
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);

            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.StartTime = txtStartTime.Text;

            entity.DosingDuration = Convert.ToInt32(txtDosingDuration.Text);
            if (hdnPopupSignaID.Value.ToString() != "")
                entity.SignaID = Convert.ToInt32(hdnPopupSignaID.Value);
            else
                entity.SignaID = null;

            entity.GCRoute = cboMedicationRoute.Value.ToString();
            if (cboPopupCoenamRule.Value != null && cboPopupCoenamRule.Value.ToString() != "")
                entity.GCCoenamRule = cboPopupCoenamRule.Value.ToString();
            else
                entity.GCCoenamRule = null;

            entity.IsUseSweetener = chkIsUsingSweetener.Checked;
            entity.MedicationAdministration = txtMedicationAdministration.Text;

            if (entity.ParentID == null)
            {
                if (hdnEmbalaceID.Value != "" && hdnEmbalaceID.Value != "0")
                    entity.EmbalaceID = Convert.ToInt32(hdnEmbalaceID.Value);
                else
                    entity.EmbalaceID = null;
                entity.EmbalaceQty = Convert.ToDecimal(txtEmbalaceQty.Text);
                //entityChargesDt.EmbalaceAmount = Convert.ToDecimal(hdnEmbalaceAmount.Value);
                string tempEmbalaceAmount = Request.Form[txtEmbalaceAmount.UniqueID];
                if (tempEmbalaceAmount != "")
                    entityChargesDt.EmbalaceAmount = Convert.ToDecimal(tempEmbalaceAmount);
                else
                    entityChargesDt.EmbalaceAmount = 0;
            }
            else
            {
                entity.EmbalaceID = null;
                entity.EmbalaceQty = 0;
                entityChargesDt.EmbalaceAmount = 0;
            }
            entityChargesDt.PrescriptionFeeAmount = Convert.ToDecimal(hdnPrescriptionFeeAmount.Value);
        }

        private void ControlToEntity(PrescriptionOrderDt entity, PatientChargesDt entityChargesDt)
        {
            #region Prescription Order Dt
            entity.IsCompound = true;
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            entity.DrugName = Request.Form[txtItemName1.UniqueID];
            entity.GenericName = txtGenericName.Text;
            entity.CompoundQty = Convert.ToDecimal(txtCompoundQty.Text);
            entity.GCCompoundUnit = cboCompoundUnit.Value.ToString();

            entity.Dose = Convert.ToDecimal(Request.Form[txtDose.UniqueID]);
            entity.GCDoseUnit = hdnGCDoseUnit.Value.ToString();

            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            entity.GCDosingUnit = entity.GCDrugForm;

            entity.ConversionFactor = Convert.ToDecimal(hdnConversionFactor.Value);

            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
            entity.TakenQty = Convert.ToDecimal(txtTakenQty.Text);
            entity.ChargeQty = entity.ResultQty = (entity.NumberOfDosage / Convert.ToDecimal(entity.Dose)) * entity.TakenQty;

            entity.IsUseSweetener = chkIsUsingSweetener.Checked;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            #endregion 

            #region PatientChargesDt
            entityChargesDt.LocationID = Convert.ToInt32(hdnLocationID.Value);
            entityChargesDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityChargesDt.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);

            entityChargesDt.BaseTariff = Convert.ToDecimal(hdnBaseTariff.Value);
            entityChargesDt.BaseComp1 = Convert.ToDecimal(hdnBaseTariff.Value);
            entityChargesDt.BaseComp2 = Convert.ToDecimal(0);
            entityChargesDt.BaseComp3 = Convert.ToDecimal(0);

            entityChargesDt.Tariff = Convert.ToDecimal(Request.Form[txtPopupTariff.UniqueID]);
            entityChargesDt.TariffComp1 = Convert.ToDecimal(entityChargesDt.Tariff);
            entityChargesDt.TariffComp2 = Convert.ToDecimal(0);
            entityChargesDt.TariffComp3 = Convert.ToDecimal(0);

            entityChargesDt.UsedQuantity = Convert.ToDecimal(hdnResultQty.Value);
            entityChargesDt.ChargedQuantity = Convert.ToDecimal(hdnResultQty.Value);
            entityChargesDt.BaseQuantity = Convert.ToDecimal(hdnResultQty.Value);
            entityChargesDt.GCBaseUnit = hdnGCBaseUnit.Value;
            entityChargesDt.GCItemUnit = entityChargesDt.GCBaseUnit;
            entityChargesDt.ConversionFactor = Convert.ToDecimal(hdnConversionFactor.Value);

            entityChargesDt.ChargeClassID = Convert.ToInt32(cboPopupChargeClass.Value);
            entityChargesDt.EmbalaceAmount = Convert.ToDecimal(Request.Form[txtEmbalaceAmount.UniqueID]);
            entityChargesDt.DiscountAmount = Convert.ToDecimal(Request.Form[txtPopupDiscountAmount.UniqueID]);
            entityChargesDt.PatientAmount = Convert.ToDecimal(txtPopupPatientAmount.Text);
            entityChargesDt.PayerAmount = Convert.ToDecimal(txtPopupPayerAmount.Text);
            entityChargesDt.LineAmount = Convert.ToDecimal(Request.Form[txtPopupLineAmount.UniqueID]);

            entityChargesDt.CreatedBy = AppSession.UserLogin.UserID;
            #endregion
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int transactionID, ref int prescriptionOrderID, ref int parentID, ref string transactionNo)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            try
            {
                SavePrescriptionOrderHd(ctx, ref transactionID, ref transactionNo, ref prescriptionOrderID);
                PrescriptionOrderDt entityDt = new PrescriptionOrderDt();
                PatientChargesDt entityChargesDt = new PatientChargesDt();
                if (hdnParentID.Value != "" && hdnParentID.Value != "0")
                {
                    entityDt.IsRFlag = false;
                    entityDt.ParentID = Convert.ToInt32(hdnParentID.Value);
                }
                else
                {
                    entityDt.IsRFlag = true;
                    entityDt.ParentID = null;
                }
                ControlToEntityCompoundHd(entityDt, entityChargesDt);
                ControlToEntity(entityDt, entityChargesDt);
                entityDt.PrescriptionOrderID = prescriptionOrderID;
                entityDt.LastUpdatedBy = entityDt.CreatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Insert(entityDt);

                if (entityDt.ParentID == null)
                    parentID = BusinessLayer.GetPrescriptionOrderDtMaxID(ctx);

                entityChargesDt.PrescriptionOrderDetailID = BusinessLayer.GetPrescriptionOrderDtMaxID(ctx);
                entityChargesDt.TransactionID = transactionID;
                entityChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                entityChargesDtDao.Insert(entityChargesDt);
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
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            try
            {
                PrescriptionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnPrescriptionOrderDetailID.Value));
                PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("PrescriptionOrderDetailID = {0}", hdnPrescriptionOrderDetailID.Value))[0];
                ControlToEntityCompoundHd(entityDt, entityChargesDt);
                ControlToEntity(entityDt, entityChargesDt);
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
                entityChargesDtDao.Update(entityChargesDt);
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

        private bool OnDeleteEntityDt(ref string errMessage, ref int parentID)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);

            try
            {
                PrescriptionOrderDt entity = entityDtDao.Get(Convert.ToInt32(hdnPrescriptionOrderDetailID.Value));
                PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("PrescriptionOrderDetailID = {0}", hdnPrescriptionOrderDetailID.Value))[0];
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);

                if (entity.ParentID == null)
                {
                    string filterExpression = string.Format("PrescriptionOrderID = {0} AND (PrescriptionOrderDetailID = {1} OR ParentID = {1}) AND IsCompound = 1 AND IsDeleted = 0", hdnPrescriptionOrderID.Value, hdnPrescriptionOrderDetailID.Value);
                    List<PrescriptionOrderDt> lstEntity = BusinessLayer.GetPrescriptionOrderDtList(filterExpression, ctx);

                    if (lstEntity.Count > 0)
                    {
                        PrescriptionOrderDt newParent = lstEntity[0];
                        parentID = newParent.PrescriptionOrderDetailID;
                        newParent.ParentID = null;
                        newParent.EmbalaceQty = entity.EmbalaceQty;
                        newParent.EmbalaceID = entity.EmbalaceID;

                        PatientChargesDt newParentChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("PrescriptionOrderDetailID = {0}", newParent.PrescriptionOrderDetailID), ctx).FirstOrDefault();
                        newParentChargesDt.EmbalaceAmount = entityChargesDt.EmbalaceAmount;
                        newParentChargesDt.PatientAmount += entityChargesDt.EmbalaceAmount;
                        newParentChargesDt.LineAmount += entityChargesDt.EmbalaceAmount;
                        newParentChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityChargesDtDao.Update(newParentChargesDt);

                        foreach (PrescriptionOrderDt obj in lstEntity)
                        {
                            if (obj.ParentID != null)
                                obj.ParentID = parentID;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(obj);
                        }
                    }
                }

                entityChargesDt.IsDeleted = true;
                entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityChargesDtDao.Update(entityChargesDt);

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
        #endregion

        protected void cboCompoundUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN ( (SELECT GCDoseUnit FROM vDrugInfo WHERE ItemID = {1}),(SELECT GCItemUnit FROM vDrugInfo WHERE ItemID = {2})))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value, hdnItemID.Value));
            Methods.SetComboBoxField<StandardCode>(cboCompoundUnit, lst, "StandardCodeName", "StandardCodeID");
            cboCompoundUnit.SelectedIndex = -1;
        }
    }
}