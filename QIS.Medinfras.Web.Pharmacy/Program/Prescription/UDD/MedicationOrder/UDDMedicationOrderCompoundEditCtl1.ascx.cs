using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class UDDMedicationOrderCompoundEditCtl1 : BaseEntryPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string queryString)
        {
            hdnQueryString.Value = queryString;
            string[] param = queryString.Split('|');

            IsAdd = false;


            hdnPrescriptionOrderID.Value = param[1];
            hdnParentID.Value = param[2];
            hdnVisitID.Value = param[7].ToString();

            vConsultVisitBase entityConsultVisit = BusinessLayer.GetvConsultVisitBaseList(String.Format("VisitID = {0}", hdnVisitID.Value))[0];
            hdnChargeClassID.Value = entityConsultVisit.ChargeClassID.ToString();
            hdnRegistrationID.Value = entityConsultVisit.RegistrationID.ToString();

            SetControlProperties();
            if (hdnParentID.Value != "")
            {
                List<PrescriptionOrderDt> lstEntity = BusinessLayer.GetPrescriptionOrderDtList(String.Format("PrescriptionOrderDetailID = {0}", hdnParentID.Value));
                if (lstEntity.Count > 0) EntityToControl(lstEntity[0]);
            }

            BindListView();

            Helper.SetControlEntrySetting(txtGenericName, new ControlEntrySetting(true, true, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemName1, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDose, new ControlEntrySetting(true, true, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDoseUnit, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtCompoundQty, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboCompoundUnit, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtConversionFactor, new ControlEntrySetting(true, true, true), "mpTrxPopup");
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        #region Bind List View
        private void BindListView()
        {
            String filterExpression = "";
            if (hdnParentID.Value == "" || hdnParentID.Value == "0")
                filterExpression = "1 = 0";
            else
                filterExpression = string.Format("PrescriptionOrderDetailID = {0} OR ParentID = {0}", hdnParentID.Value);
            List<vPrescriptionOrderDt4> lstEntity = BusinessLayer.GetvPrescriptionOrderDt4List(filterExpression);
            if (lstEntity.Count == 1)
                hdnIsEditable.Value = "0";
            else
                hdnIsEditable.Value = "1";
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
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
            SetControlEntrySetting(txtCompoundMedicationName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboMedicationRouteCompoundCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCoenamRuleCompoundCtl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboFrequencyTimeline, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUsingSweetener, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAsRequired, new ControlEntrySetting(true, true, false));
        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.COENAM_RULE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnitCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1" || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRouteCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRuleCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");


            List<EmbalaceHd> lstEmbalace = BusinessLayer.GetEmbalaceHdList("IsDeleted = 0");
            lstEmbalace.Insert(0, new EmbalaceHd() { EmbalaceID = 0, EmbalaceName = "" });
            Methods.SetComboBoxField<EmbalaceHd>(cboEmbalace, lstEmbalace, "EmbalaceName", "EmbalaceID");
            cboEmbalace.SelectedIndex = 0;

            cboFrequencyTimeline.Value = Constant.DosingFrequency.DAY;

            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString("HH:mm");

            txtDispenseQty.Text = "1";
        }

        private void EntityToControl(PrescriptionOrderDt entity)
        {
            txtDispenseQty.Text = entity.DispenseQty.ToString("G29");
            txtTakenQty.Text = entity.TakenQty.ToString("G29");
            txtCompoundMedicationName.Text = entity.CompoundDrugname;
            cboMedicationRouteCompoundCtl.Value = entity.GCRoute;
            chkIsUsingSweetener.Checked = entity.IsUseSweetener;
            chkIsAsRequired.Checked = entity.IsAsRequired;
            cboCoenamRuleCompoundCtl.Value = entity.GCCoenamRule;
            cboFrequencyTimeline.Value = entity.GCDosingFrequency;
            txtFrequencyNumber.Text = entity.Frequency.ToString();
            txtDosingDose.Text = entity.NumberOfDosage.ToString("G29");
            cboDosingUnitCompoundCtl.Value = entity.GCDosingUnit;
            txtDosingDuration.Text = entity.DosingDuration.ToString();
            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = entity.StartTime;
            txtStartTime1.Text = entity.Sequence1Time;
            txtStartTime2.Text = entity.Sequence2Time;
            txtStartTime3.Text = entity.Sequence3Time;
            txtStartTime4.Text = entity.Sequence4Time;
            txtStartTime5.Text = entity.Sequence5Time;
            txtStartTime6.Text = entity.Sequence6Time;
            chkIsMorning.Checked = entity.IsMorning;
            chkIsNoon.Checked = entity.IsNoon;
            chkIsEvening.Checked = entity.IsEvening;
            chkIsNight.Checked = entity.IsNight;
            cboCoenamRuleCompoundCtl.Value = entity.GCCoenamRule;
            cboEmbalace.Value = entity.EmbalaceID.ToString();
            txtEmbalaceQty.Text = entity.EmbalaceQty.ToString("G29");
            txtMedicationAdministration.Text = entity.MedicationAdministration;
            txtMedicationPurpose.Text = entity.MedicationPurpose;
        }

        protected void cboCompoundUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN ( (SELECT GCDoseUnit FROM vDrugInfo WHERE ItemID = {1}),(SELECT GCItemUnit FROM vDrugInfo WHERE ItemID = {2})))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value, hdnItemID.Value));
            Methods.SetComboBoxField<StandardCode>(cboCompoundUnit, lst, "StandardCodeName", "StandardCodeID");
            cboCompoundUnit.SelectedIndex = -1;
        }

        public void SavePrescriptionOrderHd(IDbContext ctx, ref int prescriptionOrderID)
        {
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHd entityHd = null;

            string[] paramHeader = hdnQueryString.Value.ToString().Split('|');
            if (hdnPrescriptionOrderID.Value == "0")
            {
                DateTime prescriptionDate = Helper.DateInStringToDateTime(paramHeader[1]);
                string prescriptionTime = paramHeader[2];
                int paramedicID = Convert.ToInt32(paramHeader[3]);
                string refillInstruction = paramHeader[4];

                entityHd = new PrescriptionOrderHd();
                entityHd.ParamedicID = paramedicID;
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entityHd.PrescriptionDate = prescriptionDate;
                entityHd.PrescriptionTime = prescriptionTime;
                entityHd.ClassID = Convert.ToInt32(hdnChargeClassID.Value);
                entityHd.DispensaryServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                entityHd.LocationID = Convert.ToInt32(hdnLocationID.Value);
                entityHd.GCPrescriptionType = Constant.PrescriptionType.DISCHARGE_PRESCRIPTION;
                entityHd.GCRefillInstruction = refillInstruction;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                    entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                else
                    entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                prescriptionOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                prescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
            }

        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            return true;
        }

        #region Process Detail
        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int prescriptionOrderID = 0;
            int parentID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnPrescriptionOrderDetailID.Value.ToString() != "")
                {
                    prescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                    parentID = Convert.ToInt32(hdnParentID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref prescriptionOrderID, ref parentID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
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
        }

        private void ControlToEntityCompoundHd(PrescriptionOrderDt entity, vDrugInfo1 drugInfo)
        {
            entity.CompoundDrugname = txtCompoundMedicationName.Text;

            entity.SignaID = null;
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
            entity.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text.Replace(',', '.'));
            entity.NumberOfDosageInString = txtDosingDose.Text;
            entity.DosingDuration = Convert.ToInt32(txtDosingDuration.Text.Replace(',', '.'));
            entity.Dose = drugInfo.Dose;

            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.StartTime = txtStartTime.Text;
            entity.IsMorning = chkIsMorning.Checked;
            entity.IsNoon = chkIsNoon.Checked;
            entity.IsEvening = chkIsEvening.Checked;
            entity.IsNight = chkIsNight.Checked;

            if (cboEmbalace.Value != null)
            {
                if (cboEmbalace.Value.ToString() != "" && cboEmbalace.Value.ToString() != "0")
                {
                    entity.EmbalaceID = Convert.ToInt32(cboEmbalace.Value);
                    entity.EmbalaceQty = Convert.ToInt16(txtEmbalaceQty.Text);
                }
                else
                {
                    entity.EmbalaceID = null;
                    entity.EmbalaceQty = 0;
                }
            }
            else
            {
                entity.EmbalaceID = null;
                entity.EmbalaceQty = 0;
            }

            entity.Sequence1Time = txtStartTime1.Text;
            entity.Sequence2Time = txtStartTime2.Text;
            entity.Sequence3Time = txtStartTime3.Text;
            entity.Sequence4Time = txtStartTime4.Text;
            entity.Sequence5Time = txtStartTime5.Text;
            entity.Sequence6Time = txtStartTime6.Text;

            entity.GCRoute = cboMedicationRouteCompoundCtl.Value.ToString();
            if (cboCoenamRuleCompoundCtl.Value != null && cboCoenamRuleCompoundCtl.Value.ToString() != "")
                entity.GCCoenamRule = cboCoenamRuleCompoundCtl.Value.ToString();
            else
                entity.GCCoenamRule = null;

            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text.Replace(',', '.'));
            entity.GCDosingUnit = cboDosingUnitCompoundCtl.Value.ToString();

            bool isUsingStrength = entity.GCCompoundUnit == entity.GCDoseUnit;
            if (isUsingStrength)
            {
                if (entity.Dose != null)
                    entity.ConversionFactor = 1 / (Decimal)entity.Dose;
                else
                    entity.ConversionFactor = 1;
            }
            else
            {
                entity.ConversionFactor = 1;
            }


            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text.Replace(',', '.'));
            entity.TakenQty = Convert.ToDecimal(txtTakenQty.Text.Replace(',', '.'));
            if (drugInfo.GCItemUnit != entity.GCCompoundUnit)
            {
                string compoundQty = entity.CompoundQtyInString.Replace(',', '.');
                decimal qty = 0;
                if (compoundQty.Contains('/'))
                {
                    string[] compoundInfo = compoundQty.Split('/');
                    decimal num1 = Convert.ToDecimal(compoundInfo[0]);
                    decimal num2 = Convert.ToDecimal(compoundInfo[1]);
                    //qty = Math.Round(num1 / num2, 2);

                    decimal dose = (Decimal)entity.Dose;
                    entity.CompoundQtyInString = compoundQty;
                    entity.CompoundQty = num1 / num2 / dose;
                    entity.ResultQty = num1 / num2 * entity.TakenQty / dose;
                }
                else
                {
                    qty = Convert.ToDecimal(compoundQty);
                    decimal dose = (Decimal)entity.Dose;
                    entity.CompoundQty = qty / dose;
                    entity.CompoundQtyInString = compoundQty;
                    entity.ResultQty = qty * entity.TakenQty / dose;
                }
            }
            else
            {
                string compoundQty = entity.CompoundQtyInString.Replace(',', '.');
                decimal qty = 0;
                if (compoundQty.Contains('/'))
                {
                    string[] compoundInfo = compoundQty.Split('/');
                    decimal num1 = Convert.ToDecimal(compoundInfo[0]);
                    decimal num2 = Convert.ToDecimal(compoundInfo[1]);
                    //qty = Math.Round(num1 / num2, 2);

                    entity.CompoundQty = num1 / num2;
                    entity.CompoundQtyInString = compoundQty;
                    entity.ResultQty = num1 / num2 * entity.TakenQty;
                }
                else
                {
                    qty = Convert.ToDecimal(compoundQty);

                    entity.CompoundQty = qty;
                    entity.CompoundQtyInString = compoundQty;
                    entity.ResultQty = qty * entity.TakenQty;
                }
            }

            entity.ChargeQty = entity.ResultQty;

            if (drugInfo.GCStockDeductionType == Constant.QuantityDeductionType.DIBULATKAN) entity.ChargeQty = Math.Ceiling(entity.ChargeQty);
            if (drugInfo.GCConsumptionDeductionType == Constant.QuantityDeductionType.DIBULATKAN) entity.ResultQty = Math.Ceiling(entity.ResultQty);


            entity.IsUseSweetener = chkIsUsingSweetener.Checked;
            entity.IsAsRequired = chkIsAsRequired.Checked;
            entity.MedicationPurpose = txtMedicationPurpose.Text;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
        }

        private void ControlToEntity(PrescriptionOrderDt entity, vDrugInfo1 drugInfo)
        {
            entity.IsCompound = true;
            entity.CompoundDrugname = txtCompoundMedicationName.Text;
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            entity.DrugName = Request.Form[txtItemName1.UniqueID];
            entity.GenericName = txtGenericName.Text;

            entity.GCPrescriptionOrderStatus = Constant.TestOrderStatus.RECEIVED;
            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text.Replace(',', '.'));
            entity.GCDosingUnit = cboDosingUnitCompoundCtl.Value.ToString();

            entity.Dose = drugInfo.Dose;
            if (!String.IsNullOrEmpty(drugInfo.GCDoseUnit))
            {
                entity.GCDoseUnit = drugInfo.GCDoseUnit;
            }

            entity.CompoundQtyInString = txtCompoundQty.Text.Replace(',', '.');
            string compoundQty = entity.CompoundQtyInString;
            decimal qty = 0;
            if (compoundQty.Contains('/'))
            {
                string[] compoundInfo = compoundQty.Split('/');
                decimal num1 = Convert.ToDecimal(compoundInfo[0]);
                decimal num2 = Convert.ToDecimal(compoundInfo[1]);
                qty = Math.Round(num1 / num2, 2);
            }
            else
            {
                qty = Convert.ToDecimal(compoundQty);
            }

            entity.GCCompoundUnit = cboCompoundUnit.Value.ToString();

            bool isUsingStrength = entity.GCCompoundUnit == entity.GCDoseUnit;
            if (isUsingStrength && !String.IsNullOrEmpty(drugInfo.GCDoseUnit))
            {
                entity.ConversionFactor = 1 / drugInfo.Dose;
            }
            else
            {
                entity.ConversionFactor = 1;
            }

            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
            entity.TakenQty = Convert.ToDecimal(txtDispenseQty.Text);
            if (drugInfo.GCItemUnit != entity.GCCompoundUnit)
            {
                entity.CompoundQty = qty / drugInfo.Dose;
                entity.ResultQty = qty * entity.TakenQty / drugInfo.Dose;
            }
            else
            {
                entity.CompoundQty = qty;
                entity.ResultQty = qty * entity.TakenQty;
            }
            entity.ChargeQty = entity.ResultQty;

            if (drugInfo.GCStockDeductionType == Constant.QuantityDeductionType.DIBULATKAN) entity.ChargeQty = Math.Ceiling(entity.ChargeQty);
            if (drugInfo.GCConsumptionDeductionType == Constant.QuantityDeductionType.DIBULATKAN) entity.ResultQty = Math.Ceiling(entity.ResultQty);

            if (entity.IsRFlag)
            {
                if (cboEmbalace.Value != null)
                {
                    if (cboEmbalace.Value.ToString() != "" && cboEmbalace.Value.ToString() != "0")
                    {
                        entity.EmbalaceID = Convert.ToInt32(cboEmbalace.Value);
                        entity.EmbalaceQty = Convert.ToInt16(txtEmbalaceQty.Text);
                    }
                    else
                    {
                        entity.EmbalaceID = null;
                        entity.EmbalaceQty = 0;
                    }
                }
                else
                {
                    entity.EmbalaceID = null;
                    entity.EmbalaceQty = 0;
                }
            }
            else
            {
                entity.EmbalaceID = null;
                entity.EmbalaceQty = 0;
            }

            if (hdnParentID.Value != "" && hdnParentID.Value != "0")
            {
                if (Convert.ToInt32(hdnParentID.Value) != entity.PrescriptionOrderDetailID)
                {
                    entity.IsRFlag = false;
                    entity.ParentID = Convert.ToInt32(hdnParentID.Value);
                }
                else
                {
                    entity.IsRFlag = true;
                    entity.ParentID = null;
                }
            }

            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.StartTime = txtStartTime1.Text.Replace('.', ':');
            entity.Sequence1Time = txtStartTime1.Text.Replace('.', ':');
            entity.Sequence2Time = txtStartTime2.Text.Replace('.', ':');
            entity.Sequence3Time = txtStartTime3.Text.Replace('.', ':');
            entity.Sequence4Time = txtStartTime4.Text.Replace('.', ':');
            entity.Sequence5Time = txtStartTime5.Text.Replace('.', ':');
            entity.Sequence6Time = txtStartTime6.Text.Replace('.', ':');

            entity.IsUseSweetener = chkIsUsingSweetener.Checked;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            entity.MedicationPurpose = txtMedicationPurpose.Text;

        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int prescriptionOrderID, ref int parentID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            try
            {
                SavePrescriptionOrderHd(ctx, ref prescriptionOrderID);
                PrescriptionOrderDt entityDt = new PrescriptionOrderDt();
                vDrugInfo1 drugInfo = BusinessLayer.GetvDrugInfo1List(string.Format("ItemID IN ({0})", hdnItemID.Value), ctx).FirstOrDefault();
                entityDt.CompoundQtyInString = txtCompoundQty.Text;
                entityDt.GCCompoundUnit = cboCompoundUnit.Value.ToString();
                ControlToEntityCompoundHd(entityDt, drugInfo);
                ControlToEntity(entityDt, drugInfo);


                entityDt.PrescriptionOrderID = prescriptionOrderID;
                entityDt.LastUpdatedBy = entityDt.CreatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Insert(entityDt);

                if (entityDt.ParentID == null)
                    parentID = BusinessLayer.GetPrescriptionOrderDtMaxID(ctx);
                else
                    parentID = Convert.ToInt32(hdnParentID.Value);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            try
            {
                List<PrescriptionOrderDt> lstEntity = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderDetailID = {0} OR ParentID = {0} AND IsDeleted = 0", hdnParentID.Value), ctx);
                foreach (PrescriptionOrderDt entity in lstEntity)
                {
                    vDrugInfo1 drugInfo = BusinessLayer.GetvDrugInfo1List(string.Format("ItemID IN ({0})", entity.ItemID), ctx).FirstOrDefault();
                    ControlToEntityCompoundHd(entity, drugInfo);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entity);
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
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
            try
            {
                PrescriptionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnPrescriptionOrderDetailID.Value));
                vDrugInfo1 drugInfo = BusinessLayer.GetvDrugInfo1List(string.Format("ItemID IN ({0})", hdnItemID.Value), ctx).FirstOrDefault();
                ControlToEntity(entityDt, drugInfo);
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
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

            try
            {
                PrescriptionOrderDt entity = entityDtDao.Get(Convert.ToInt32(hdnPrescriptionOrderDetailID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);

                if (entity.ParentID == null)
                {
                    string filterExpression = hdnFilterExpression.Value;
                    List<PrescriptionOrderDt> lstEntity = BusinessLayer.GetPrescriptionOrderDtList(string.Format("ParentID = {0} AND IsDeleted = 0", hdnPrescriptionOrderDetailID.Value), ctx);

                    if (lstEntity.Count > 0)
                    {
                        parentID = lstEntity[0].PrescriptionOrderDetailID;
                        lstEntity[0].ParentID = null;
                        lstEntity[0].IsRFlag = true;
                        lstEntity[0].GCCoenamRule = entity.GCCoenamRule;

                        foreach (PrescriptionOrderDt obj in lstEntity)
                        {
                            if (obj.ParentID != null)
                                obj.ParentID = parentID;

                            entityDtDao.Update(obj);
                        }
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
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