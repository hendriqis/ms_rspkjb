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
using System.Globalization;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PrescriptionEntry : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PRESCRIPTION;
        }

        #region List
        protected override void InitializeDataControl()
        {
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND IsDeleted = 0", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            String filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.REFILL_INSTRUCTION);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboRefillInstruction, lstStandardCode, "StandardCodeName", "StandardCodeID");

            if (Page.Request.QueryString.Count > 0)
            {
                hdnPrescriptionOrderID.Value = Page.Request.QueryString["id"];
            }

            if (hdnPrescriptionOrderID.Value != "")
            {
                LoadHeaderInformation();
            }
            else
            {
                txtPrescriptionNo.Text = string.Empty;
                cboRefillInstruction.SelectedIndex = 0;
                txtPrescriptionDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtPrescriptionTime.Text = DateTime.Now.ToString("HH:mm");
                txtRemarks.Text = "";

                if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
                {
                    int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                    cboParamedicID.ClientEnabled = false;
                    cboParamedicID.Value = userLoginParamedic.ToString();
                }

                //Get Service Unit Dispensary Unit
                HealthcareServiceUnit oServiceUnit = BusinessLayer.GetHealthcareServiceUnit(AppSession.RegisteredPatient.HealthcareServiceUnitID);
                if (oServiceUnit != null)
                {
                    cboDispensaryUnit.Value = oServiceUnit.DispensaryServiceUnitID.ToString();
                }
            }

            BindCboLocation();
            BindGridView(1, true, ref PageCount);
        }

        private void LoadHeaderInformation()
        {
            string filterExpression;

            hdnFilterExpressionItem.Value = string.Format("ItemID NOT IN (SELECT ItemID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = {0} AND IsDeleted = 0) AND IsDeleted = 0", hdnPrescriptionOrderID.Value);
            PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));

            txtPrescriptionNo.Text = entity.PrescriptionOrderNo;
            txtPrescriptionDate.Text = entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entity.PrescriptionTime;
            cboParamedicID.ClientEnabled = false;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            txtRemarks.Text = entity.Remarks;
            cboDispensaryUnit.Value = entity.DispensaryServiceUnitID.ToString();
            cboDispensaryUnit.ClientEnabled = false;
            cboLocation.Value = entity.LocationID.ToString();
            cboLocation.ClientEnabled = false;
            hdnDefaultDispensaryServiceUnitID.Value = entity.DispensaryServiceUnitID.ToString();

            cboPrescriptionType.Value = entity.GCPrescriptionType;
            cboPrescriptionType.ClientEnabled = false;

            cboRefillInstruction.ClientEnabled = false;
            cboRefillInstruction.Value = entity.GCRefillInstruction.ToString();

            filterExpression = hdnFilterExpressionItem.Value;
            txtQuickEntry.QuickEntryHints[0].FilterExpression = filterExpression;
            ledDrugName.FilterExpression = filterExpression;

            hdnIsProposed.Value = entity.GCTransactionStatus == Constant.TransactionStatus.OPEN ? "0" : "1";
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnPrescriptionOrderID.Value != "")
            {
                filterExpression = hdnFilterExpression.Value;
                if (filterExpression != "")
                    filterExpression += " AND ";
                filterExpression += string.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0", hdnPrescriptionOrderID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPrescriptionOrderDt1RowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderDetailID");
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
                    if (Page.Request.QueryString.Count == 0)
                    {
                        LoadHeaderInformation();
                    }
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount + "|" + txtPrescriptionNo.Text;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                PrescriptionOrderDt entity = BusinessLayer.GetPrescriptionOrderDt(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdatePrescriptionOrderDt(entity);
                return true;
            }
            return false;
        }
        #endregion

        #region Entry
        protected override void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.PRESCRIPTION_TYPE, Constant.StandardCode.DRUG_FORM, Constant.StandardCode.COENAM_RULE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDrugForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE).ToList(), "StandardCodeName", "StandardCodeID");

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                if (!AppSession.IsUsedInpatientPrescriptionTypeFilter)
                {
                    Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
                }
                else
                {
                    if (!string.IsNullOrEmpty(AppSession.InpatientPrescriptionTypeFilter))
                    {
                        string[] prescriptionType = AppSession.InpatientPrescriptionTypeFilter.Split(',');
                        Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).Where(x => !prescriptionType.Contains(x.StandardCodeID)).ToList(), "StandardCodeName", "StandardCodeID");
                    }
                    else
                    {
                        Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
                    }
                }
            }
            else
            {
                Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            }

            cboPrescriptionType.SelectedIndex = 0;
            cboFrequencyTimeline.SelectedIndex = 1;
            cboDosingUnit.SelectedIndex = 0;
            cboMedicationRoute.SelectedIndex = 0;

            txtStartDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboDispensaryUnit, lstHealthcareServiceUnit.Where(x => x.DepartmentID == "PHARMACY").ToList(), "ServiceUnitName", "HealthcareServiceUnitID");
            if (cboDispensaryUnit.Value == null)
            {
                cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtGenericName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStrengthAmount, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboFrequencyTimeline, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDosingUnit, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPurposeOfMedication, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDispensaryUnit, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboLocation, new ControlEntrySetting(false, false, true));
        }

        private void ControlToEntity(PrescriptionOrderDt entity)
        {
            entity.IsRFlag = chkIsRx.Checked;
            if (hdnDrugID.Value.ToString() != "")
                entity.ItemID = Convert.ToInt32(hdnDrugID.Value);
            else
                entity.ItemID = null;
            entity.DrugName = hdnDrugName.Value;
            entity.GenericName = txtGenericName.Text;
            if (cboDrugForm.Value != null)
                entity.GCDrugForm = cboDrugForm.Value.ToString();

            string strengthUnit = hdnStrengthUnit.Value;
            string strengthAmount = hdnStrengthAmount.Value;
            if (!String.IsNullOrEmpty(strengthUnit))
            {
                entity.Dose = Convert.ToDecimal(strengthAmount);
                entity.GCDoseUnit = string.Format("X003^{0}", strengthUnit);
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
            if (cboCoenamRule.Value != null)
                entity.GCCoenamRule = cboCoenamRule.Value.ToString();
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.StartTime = txtStartTime.Text;
            entity.IsMorning = chkIsMorning.Checked;
            entity.IsNoon = chkIsNoon.Checked;
            entity.IsEvening = chkIsEvening.Checked;
            entity.IsNight = chkIsNight.Checked;
            entity.MedicationPurpose = txtPurposeOfMedication.Text;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            entity.DosingDuration = Convert.ToDecimal(txtDosingDuration.Text);
            entity.IsAsRequired = chkIsAsRequired.Checked;
            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
            entity.TakenQty = Convert.ToDecimal(txtDispenseQty.Text);
            entity.ResultQty = entity.DispenseQty;
            entity.ChargeQty = entity.DispenseQty;
            entity.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;
        }

        protected void cboLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
            hdnLocationID.Value = cboLocation.Value.ToString();
        }

        private void BindCboLocation()
        {
            if (cboDispensaryUnit.Value != null)
            {
                Location location = BusinessLayer.GetLocationList(string.Format("LocationID IN (SELECT LocationID FROM HealthcareServiceUnit WHERE HealthcareServiceUnitID = {0})", cboDispensaryUnit.Value)).FirstOrDefault();

                if (location != null)
                {
                    int locationID = location.LocationID;
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
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            vDrugInfo entityItem = null;
            entityItem = BusinessLayer.GetvDrugInfoList(string.Format("ItemID = {0}", hdnDrugID.Value))[0];
            bool result = true;
            string isAllow = "0";

            if (!string.IsNullOrEmpty(txtDosingDuration.Text) && !string.IsNullOrEmpty(txtDispenseQty.Text))
            {
                decimal dispenseQty = 0;
                bool isDecimal = decimal.TryParse(txtDispenseQty.Text, out dispenseQty);

                if (dispenseQty <= 0)
                {
                    result = false;
                    errMessage = "Dispense Quantity should be greater than 0";
                    return result;
                }
            }
            else
            {
                result = false;
                errMessage = "Medication Duration and Dispense Quantity should be greater than 0";
                return result;
            }

            if (IsPrescriptionItemValid(entityItem, ref errMessage, ref isAllow))
            {
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
                try
                {
                    PrescriptionOrderHd entityHd = null;
                    if (hdnPrescriptionOrderID.Value == "")
                    {
                        entityHd = new PrescriptionOrderHd();
                        entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                        entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
                        entityHd.PrescriptionDate = Helper.GetDatePickerValue(txtPrescriptionDate);
                        entityHd.PrescriptionTime = txtPrescriptionTime.Text;
                        entityHd.ClassID = AppSession.RegisteredPatient.ClassID;
                        entityHd.DispensaryServiceUnitID = Convert.ToInt32(cboDispensaryUnit.Value);
                        entityHd.LocationID = Convert.ToInt32(cboLocation.Value);
                        entityHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                        entityHd.GCRefillInstruction = cboRefillInstruction.Value.ToString();
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entityHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
                        switch (AppSession.RegisteredPatient.DepartmentID)
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
                            case Constant.Facility.DIAGNOSTIC:
                                entityHd.TransactionCode = Constant.TransactionCode.OTHER_MEDICATION_ORDER;
                                break;
                            default:
                                entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                                break;
                        }
                        entityHd.Remarks = txtRemarks.Text;
                        entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        entityHd.PrescriptionOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                    }
                    else
                    {
                        entityHd = entityHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                        entityHd.Remarks = txtRemarks.Text;
                        entityHdDao.Update(entityHd);
                    }
                    retval = entityHd.PrescriptionOrderID.ToString();

                    PrescriptionOrderDt entityDt = new PrescriptionOrderDt();
                    ControlToEntity(entityDt);
                    if (entityDt.GenericName == "")
                        entityDt.GenericName = entityItem.GenericName;
                    entityDt.PrescriptionOrderID = entityHd.PrescriptionOrderID;
                    entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;
                    entityDt.IsCreatedFromOrder = true;
                    entityDt.IsCompound = false;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
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
            }
            else
            {
                result = false;
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            vDrugInfo entityItem = null;
            entityItem = BusinessLayer.GetvDrugInfoList(string.Format("ItemID = {0}", hdnDrugID.Value))[0];
            bool result = true;
            string isAllow = "0";

            decimal dispenseQty = 0;
            bool isDecimal = decimal.TryParse(txtDispenseQty.Text, out dispenseQty);

            if (dispenseQty <= 0)
            {
                result = false;
                errMessage = "Dispense Quantity should be greater than 0";
                return result;
            }

            if (IsPrescriptionItemValid(entityItem, ref errMessage, ref isAllow))
            {
                try
                {
                    PrescriptionOrderDt entity = BusinessLayer.GetPrescriptionOrderDt(Convert.ToInt32(hdnEntryID.Value));
                    ControlToEntity(entity);
                    if (entity.GenericName == "")
                        entity.GenericName = entityItem.GenericName;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePrescriptionOrderDt(entity);
                    retval = hdnPrescriptionOrderID.Value;
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                }
            }
            return result;
        }

        protected override bool OnSaveQuickEntryRecord(string quickEntryText, ref string errMessage, ref string retval)
        {
            bool result = true;
            string isAllow = "0";
            string[] param = quickEntryText.Split(';');
            int itemID = 0;
            bool isNum = Int32.TryParse(param[0], out itemID);
            decimal dispenseQty = 0;
            bool isNumber = decimal.TryParse(param[3], out dispenseQty);

            if (!isNum)
            {
                result = false;
                errMessage = "You have not select any item yet";
                return result;
            }

            if (!isNumber)
            {
                result = false;
                errMessage = "Dispense Quantity should be greater than 0";
                return result;
            }
            else
            {
                if (dispenseQty <= 0)
                {
                    result = false;
                    errMessage = "Dispense Quantity should be greater than 0";
                    return result;
                }
            }

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            try
            {

                vDrugInfo entityItem = BusinessLayer.GetvDrugInfoList(string.Format("ItemID = {0}", itemID), ctx)[0];

                string dosageInText = param[2].Replace(',','.');
                decimal numberOfDosage = 0;
                if (dosageInText.Contains('/'))
                {
                    string[] dosageInTextInfo = dosageInText.Split('/');
                    decimal num1 = Convert.ToDecimal(dosageInTextInfo[0]);
                    decimal num2 = Convert.ToDecimal(dosageInTextInfo[1]);
                    numberOfDosage = Math.Round(num1 / num2, 2);
                }
                else
                {
                    numberOfDosage = Convert.ToDecimal(dosageInText);
                }

                decimal dosingDuration = CalculateDosingDuration(param[1], numberOfDosage.ToString(), param[3]);

                if (IsPrescriptionItemValid(entityItem, ref errMessage, ref isAllow, dosingDuration))
                {
                    PrescriptionOrderHd entityHd = null;
                    if (hdnPrescriptionOrderID.Value == "")
                    {
                        entityHd = new PrescriptionOrderHd();
                        entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                        entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
                        entityHd.PrescriptionDate = Helper.GetDatePickerValue(txtPrescriptionDate);
                        entityHd.DispensaryServiceUnitID = Convert.ToInt32(cboDispensaryUnit.Value);
                        entityHd.LocationID = Convert.ToInt32(cboLocation.Value);
                        entityHd.PrescriptionTime = txtPrescriptionTime.Text;
                        entityHd.ClassID = AppSession.RegisteredPatient.ClassID;
                        entityHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                        entityHd.GCRefillInstruction = cboRefillInstruction.Value.ToString();
                        entityHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        switch (AppSession.RegisteredPatient.DepartmentID)
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
                            case Constant.Facility.DIAGNOSTIC:
                                entityHd.TransactionCode = Constant.TransactionCode.OTHER_MEDICATION_ORDER;
                                break;
                            default:
                                entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                                break;
                        }
                        entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        entityHd.PrescriptionOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                    }
                    else
                    {
                        entityHd = entityHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                    }
                    retval = entityHd.PrescriptionOrderID.ToString();

                    PrescriptionOrderDt entityDt = new PrescriptionOrderDt();

                    entityDt.PrescriptionOrderID = entityHd.PrescriptionOrderID;
                    entityDt.IsRFlag = true;
                    entityDt.IsCompound = false;
                    entityDt.ItemID = itemID;
                    entityDt.DrugName = entityItem.ItemName1;
                    entityDt.GenericName = entityItem.GenericName;
                    entityDt.GCDrugForm = entityItem.GCDrugForm;
                    entityDt.Dose = entityItem.Dose;
                    entityDt.GCDoseUnit = entityItem.GCDoseUnit;

                    entityDt.StartDate = DateTime.Now.Date;
                    entityDt.StartTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                    bool isDay = true;
                    bool isPrn = false;
                    if (param[1].ToLower().Contains("prn"))
                    {
                        isPrn = true;
                        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                        entityDt.Frequency = 1;
                        entityDt.IsMorning = true;
                    }
                    else
                    {
                        if (!param[1].ToLower().Contains("qh") && !param[1].ToLower().Contains("dd"))
                        {
                            switch (param[1])
                            {
                                case Constant.PrescriptionFrequency.OD:
                                    entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                    entityDt.Frequency = 1;
                                    entityDt.IsMorning = true;
                                    break;
                                case Constant.PrescriptionFrequency.QD:
                                    entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                    entityDt.Frequency = 1;
                                    entityDt.IsMorning = true;
                                    break;
                                case Constant.PrescriptionFrequency.BID:
                                    entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                    entityDt.Frequency = 2;
                                    entityDt.IsMorning = true;
                                    entityDt.IsNoon = true;
                                    break;
                                case Constant.PrescriptionFrequency.TID:
                                    entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                    entityDt.Frequency = 3;
                                    entityDt.IsMorning = true;
                                    entityDt.IsNoon = true;
                                    entityDt.IsNight = true;
                                    break;
                                case Constant.PrescriptionFrequency.QID:
                                    entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                    entityDt.Frequency = 4;
                                    entityDt.IsMorning = true;
                                    entityDt.IsNoon = true;
                                    entityDt.IsEvening = true;
                                    entityDt.IsNight = true;
                                    break;
                                default:
                                    entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                    entityDt.Frequency = 1;
                                    entityDt.IsMorning = true;
                                    break;
                            }
                        }
                        else
                        {
                            #region Default Frequency
                            string frequency = "1";
                            entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                            #endregion

                            if (param[1].ToLower().Contains("qh"))
                            {
                                frequency = param[1].Substring(2);
                                isDay = false;
                                entityDt.GCDosingFrequency = Constant.DosingFrequency.HOUR;
                            }
                            else
                            {
                                frequency = param[1].Substring(0, 1);
                                entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                            }
                            entityDt.Frequency = Convert.ToInt16(frequency);

                            switch (entityDt.Frequency)
                            {
                                case 1:
                                    entityDt.IsMorning = true;
                                    break;
                                case 2:
                                    entityDt.IsMorning = true;
                                    entityDt.IsNoon = true;
                                    break;
                                case 3:
                                    entityDt.IsMorning = true;
                                    entityDt.IsNoon = true;
                                    entityDt.IsNight = true;
                                    break;
                                case 4:
                                    entityDt.IsMorning = true;
                                    entityDt.IsNoon = true;
                                    entityDt.IsEvening = true;
                                    entityDt.IsNight = true;
                                    break;
                                default:
                                    entityDt.IsMorning = true;
                                    entityDt.IsNoon = true;
                                    entityDt.IsEvening = true;
                                    entityDt.IsNight = true;
                                    break;
                            }
                        }
                    }

                    entityDt.NumberOfDosage = numberOfDosage;
                    entityDt.NumberOfDosageInString = param[2];
                    entityDt.GCDosingUnit = entityItem.GCItemUnit;
                    if (!string.IsNullOrEmpty(entityItem.GCMedicationRoute))
                        entityDt.GCRoute = entityItem.GCMedicationRoute; 
                    else
                        entityDt.GCRoute = Constant.MedicationRoute.OTHER;

                    entityDt.DispenseQty = Convert.ToDecimal(param[3]);
                    entityDt.IsCreatedFromOrder = true;

                    if (!entityItem.IsExternalMedication)
                    {
                        if (!isPrn)
                        {
                            if (isDay)
                            {
                                entityDt.DosingDuration = Math.Ceiling(entityDt.DispenseQty / (entityDt.Frequency * entityDt.NumberOfDosage));
                                //entityDt.DispenseQty = (decimal)(entityDt.Frequency * entityDt.NumberOfDosage * entityDt.DosingDuration);
                            }
                            else
                            {
                                decimal numberOfTaken = Math.Ceiling(Convert.ToDecimal(24 / entityDt.Frequency));
                                entityDt.DosingDuration = Math.Ceiling(entityDt.DispenseQty / (numberOfTaken * entityDt.NumberOfDosage));
                                //entityDt.DispenseQty = (decimal)(numberOfTaken * entityDt.NumberOfDosage * entityDt.DosingDuration);
                            }
                        }
                        else
                        {
                            entityDt.DosingDuration = 1;
                        }
                    }
                    else
                    {
                        entityDt.DosingDuration = 1;
                    }

                    entityDt.IsAsRequired = isPrn;
                    entityDt.TakenQty = entityDt.DispenseQty;
                    entityDt.ResultQty = entityDt.DispenseQty;
                    entityDt.ChargeQty = entityDt.DispenseQty;
                    if (param.Length == 5)
                    {
                        entityDt.MedicationAdministration = param[4];
                    }
                    entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;

                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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

        private decimal CalculateDosingDuration(string frequency, string dosage, string dispenseQty)
        {
            bool isDay = true;
            bool isPrn = false;
            int dosingFrequency = 1;
            if (frequency.ToLower().Contains("prn"))
            {
                isPrn = true;
                dosingFrequency = 1;
            }
            else
            {
                if (!frequency.ToLower().Contains("qh") && !frequency.ToLower().Contains("dd"))
                {
                    switch (frequency)
                    {
                        case Constant.PrescriptionFrequency.QD:
                            dosingFrequency = 1;
                            break;
                        case Constant.PrescriptionFrequency.BID:
                            dosingFrequency = 2;
                            break;
                        case Constant.PrescriptionFrequency.TID:
                            dosingFrequency = 3;
                            break;
                        case Constant.PrescriptionFrequency.QID:
                            dosingFrequency = 4;
                            break;
                        default:
                            dosingFrequency = 1;
                            break;
                    }
                }
                else
                {
                    if (frequency.ToLower().Contains("qh"))
                    {
                        dosingFrequency = Convert.ToInt16(frequency.Substring(2));
                        isDay = false;
                    }
                    else
                    {
                        dosingFrequency = Convert.ToInt16(frequency.Substring(0, 1));
                    }
                 }
            }

            decimal dispense = Convert.ToDecimal(dispenseQty);

            if (!isPrn)
            {
                if (isDay)
                {
                    return Math.Ceiling(dispense / (dosingFrequency * Convert.ToDecimal(dosage)));
                }
                else
                {
                    decimal numberOfTaken = Math.Ceiling(Convert.ToDecimal(24 / dosingFrequency));
                    return Math.Ceiling(dispense / (numberOfTaken * Convert.ToDecimal(dosage)));
                }
            }
            else
            {
                return 1;
            }
        }

        private bool IsPrescriptionItemValid(vDrugInfo entityItem, ref string errMessage, ref string isAllow, decimal dosingDuration = 1)
        {
            #region Check For Allergy
            string filterExpression = string.Format("MRN = {0} AND GCAllergenType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, Constant.AllergenType.DRUG);
            string allergenName = string.Empty;

            List<PatientAllergy> lstPatientAllergy = null;
            if (entityItem.GenericName != string.Empty)
                filterExpression += string.Format(" AND (Allergen LIKE '%{0}%' OR Allergen LIKE '%{1}%')", entityItem.GenericName, entityItem.ItemName1);
            else
                filterExpression += string.Format(" AND Allergen LIKE '%{0}%'", entityItem.ItemName1);

            lstPatientAllergy = BusinessLayer.GetPatientAllergyList(filterExpression);

            if (lstPatientAllergy.Count > 0)
            {
                errMessage = string.Format("Pasien memiliki alergi {0} ({1})", entityItem.GenericName, entityItem.ItemName1).Replace("()", "");
                return false;
            }
            else
            {
                filterExpression = string.Format("ItemID = {0}", entityItem.ItemID);
                List<DrugContent> contents = BusinessLayer.GetDrugContentList(filterExpression);
                foreach (DrugContent item in contents)
                {
                    filterExpression = string.Format("MRN = {0} AND GCAllergenType = '{1}' AND Allergen LIKE '%{2}%' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, Constant.AllergenType.DRUG, item.Keyword);
                    lstPatientAllergy = BusinessLayer.GetPatientAllergyList(filterExpression);
                    if (lstPatientAllergy.Count > 0)
                    {
                        errMessage = string.Format("Pasien memiliki alergi {0} ({1})", item.ContentText, entityItem.ItemName1).Replace("()", "");
                        return false;
                    }
                }
            }
            #endregion

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.FM_KONTROL_ADVERSE_REACTION, Constant.SettingParameter.FM_MAKSIMUM_DURASI_NARKOTIKA, Constant.SettingParameter.FM_KONTROL_DUPLIKASI_TERAPI));
            SettingParameter setParMaxDurasiNarkotika = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_MAKSIMUM_DURASI_NARKOTIKA).FirstOrDefault();

            bool isControlAdverseReaction = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_KONTROL_ADVERSE_REACTION).FirstOrDefault().ParameterValue == "0" ? false : true;
            bool isControlTheraphyDuplication = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_KONTROL_DUPLIKASI_TERAPI).FirstOrDefault().ParameterValue == "0" ? false : true;

            string prescriptionOrderId = hdnPrescriptionOrderID.Value;

            if (isControlTheraphyDuplication)
            {
                #region Duplicate Theraphy
                if (prescriptionOrderId != "0" && prescriptionOrderId != "")
                {
                    string filterExp = string.Format("PrescriptionOrderID = {0}", prescriptionOrderId);
                    List<vPrescriptionOrderDt1> itemlist = BusinessLayer.GetvPrescriptionOrderDt1List(filterExp);
                    foreach (var item in itemlist)
                    {
                        //Generic Name
                        if ((item.ItemID != entityItem.ItemID) && (item.GenericName.ToLower().TrimEnd() == entityItem.GenericName.ToLower().TrimEnd()) && !item.GenericName.Equals(string.Empty))
                        {
                            errMessage = string.Format("Duplikasi obat dengan nama generik {0} yang sama ({1})", item.GenericName.TrimEnd(), item.DrugName.TrimEnd());
                            return false;
                        }
                        vDrugInfo drugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemCode = '{0}'", item.ItemCode)).FirstOrDefault();
                        if (drugInfo != null)
                        {
                            //ATC Class
                            if ((item.ItemID != entityItem.ItemID) && ((drugInfo.ATCClassCode == entityItem.ATCClassCode) && (!String.IsNullOrEmpty(entityItem.ATCClassCode))))
                            {
                                errMessage = string.Format("Duplikasi obat dengan Kelompok/Kelas ATC {0} yang sama ({1})", drugInfo.ATCClassName.TrimEnd(), item.DrugName.TrimEnd());
                                return false;
                            }
                            //Kelompok Theraphy
                            if ((item.ItemID != entityItem.ItemID) && (drugInfo.MIMSClassCode.ToLower().TrimEnd() == entityItem.MIMSClassCode.ToLower().TrimEnd()) && (!String.IsNullOrEmpty(entityItem.MIMSClassCode)))
                            {
                                errMessage = string.Format("Duplikasi obat dengan Kelompok Terapi {0} yang sama ({1})", drugInfo.MIMSClassName.TrimEnd(), item.DrugName.TrimEnd());
                                return false;
                            }
                        }
                    }
                }
                #endregion
            }

            #region psikotropika & narkotika
            if (entityItem.GCDrugClass == Constant.DrugClass.MORPHIN || entityItem.GCDrugClass == Constant.DrugClass.NARKOTIKA || entityItem.GCDrugClass == Constant.DrugClass.PSIKOTROPIKA)
            {
                int duration = 0;
                if (dosingDuration > 0)
                    duration = Convert.ToInt32(dosingDuration);
                else
                    duration = Convert.ToInt32(txtDosingDuration.Text);
                if (duration > Convert.ToInt32(setParMaxDurasiNarkotika.ParameterValue))
                {
                    errMessage = string.Format("Obat {0} Mengandung Narkotika, pemakaian tidak boleh lebih dari {1} hari", entityItem.ItemName1.TrimEnd(), setParMaxDurasiNarkotika.ParameterValue);
                    return false;
                }
            }
            #endregion

            if (!isControlAdverseReaction)
            {
                isAllow = "1";
            }

            #region Adverse Reaction
            prescriptionOrderId = hdnPrescriptionOrderID.Value;
            if (prescriptionOrderId != "0")
            {
                filterExpression = string.Format("ItemID = {0}", entityItem.ItemID);
                List<DrugReaction> reactions = BusinessLayer.GetDrugReactionList(filterExpression);
                foreach (DrugReaction advReaction in reactions)
                {
                    string filterExp = string.Format("PrescriptionOrderID = {0}", prescriptionOrderId);
                    List<vPrescriptionOrderDt1> itemlist = BusinessLayer.GetvPrescriptionOrderDt1List(filterExp);
                    foreach (var item in itemlist)
                    {
                        vDrugInfo drugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemCode = '{0}'", item.ItemCode)).FirstOrDefault();
                        if (drugInfo != null)
                        {
                            //Generic Name
                            if (drugInfo.GenericName.ToLower().TrimEnd().Contains(advReaction.AdverseReactionText1.ToLower().TrimEnd())
                                || advReaction.AdverseReactionText1.ToLower().TrimEnd().Contains(drugInfo.GenericName.ToLower().TrimEnd()))
                            {
                                errMessage = string.Format("Terjadi interaksi obat dengan {0} ({1}) \n Catatan Interaksi Obat: \n {2}", item.DrugName.TrimEnd(), drugInfo.GenericName, advReaction.AdverseReactionText2);
                                return false;
                            }
                        }
                    }
                }
            }
            #endregion


            return (errMessage == string.Empty);
        }

        protected void cbpSendOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int transactionID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            try
            {
                if (param[0] == "sendOrder")
                {
                    PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHdList(String.Format("PrescriptionOrderID = {0}", hdnPrescriptionOrderID.Value))[0];

                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        BusinessLayer.UpdatePrescriptionOrderHd(entity);

                        //try
                        //{
                        //    HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(entity.DispensaryServiceUnitID));
                        //    string ipAddress = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                        //    if (!String.IsNullOrEmpty(ipAddress))
                        //    {
                        //        SendNotification(entity,ipAddress,"6000");
                        //    }
                        //}
                        //catch (Exception)
                        //{
                        //}

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
        #endregion
    }
}