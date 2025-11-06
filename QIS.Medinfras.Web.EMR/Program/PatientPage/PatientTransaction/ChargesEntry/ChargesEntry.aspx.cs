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
    public partial class ChargesEntry : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_CHARGES_ENTRY;
        }

        #region List
        protected override void InitializeDataControl()
        {
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND IsDeleted = 0", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (Page.Request.QueryString.Count > 0)
            {
                hdnTransactionID.Value = Page.Request.QueryString["id"];
            }

            if (hdnTransactionID.Value != "")
            {
                LoadHeaderInformation();
            }
            else
            {
                txtTransactionNo.Text = string.Empty; 
                txtTransactionDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTransactionTime.Text = DateTime.Now.ToString("HH:mm");
                txtRemarks.Text = "";

                if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
                {
                    int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                    cboParamedicID.ClientEnabled = false;
                    cboParamedicID.Value = userLoginParamedic.ToString();
                }
            }

            BindCboLocation();
            BindGridView(1, true, ref PageCount);
        }

        private void LoadHeaderInformation()
        {
            string filterExpression;

            hdnFilterExpressionItem.Value = string.Format("ItemID NOT IN (SELECT ItemID FROM PatientChargesDt WHERE TransactionID = {0} AND IsDeleted = 0) AND IsDeleted = 0", hdnTransactionID.Value);
            PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionID.Value));

            txtTransactionNo.Text = entity.TransactionNo;
            txtTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.TransactionTime;
            cboParamedicID.ClientEnabled = false;
            cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            txtRemarks.Text = entity.Remarks;
            cboServiceUnit.Value = entity.HealthcareServiceUnitID.ToString();
            BindCboLocation();
            cboServiceUnit.ClientEnabled = false;
            cboLocation.ClientEnabled = false;

            filterExpression = hdnFilterExpressionItem.Value;
            txtQuickEntry.QuickEntryHints[0].FilterExpression = filterExpression;
            //ledItemName.FilterExpression = filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnTransactionID.Value != "")
            {
                filterExpression = hdnFilterExpression.Value;
                if (filterExpression != "")
                    filterExpression += " AND ";
                filterExpression += string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPatientChargesDtRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPatientChargesDt> lstEntity = BusinessLayer.GetvPatientChargesDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID");
            lvwService.DataSource = lstEntity;
            lvwService.DataBind();
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
                    result = "refresh|" + pageCount + "|" + txtTransactionNo.Text;
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
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT,Constant.StandardCode.PRESCRIPTION_TYPE,Constant.StandardCode.DRUG_FORM,Constant.StandardCode.COENAM_RULE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            List<vServiceUnitParamedic> lstHealthcareServiceUnit = BusinessLayer.GetvServiceUnitParamedicList(string.Format("ParamedicID = {0} AND IsDeleted = 0",AppSession.UserLogin.ParamedicID));
            Methods.SetComboBoxField<vServiceUnitParamedic>(cboServiceUnit, lstHealthcareServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            if (cboServiceUnit.Value == null) 
            {
                cboServiceUnit.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
            }
            BindCboLocation();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true,false,true));
            SetControlEntrySetting(cboLocation, new ControlEntrySetting(false, false, true));
        }

        private void ControlToEntity(vPatientChargesDt entity)
        {
            //if (hdnItemID.Value.ToString() != "")
            //    entity.ItemID = Convert.ToInt32(hdnItemID.Value);
   
            //entity.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            //entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
            //entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            //entity.GCDosingUnit = cboDosingUnit.Value.ToString();
            //entity.GCRoute = cboMedicationRoute.Value.ToString();
            //if (cboCoenamRule.Value != null)
            //    entity.GCCoenamRule = cboCoenamRule.Value.ToString();
            //entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            //entity.StartTime = txtStartTime.Text;
            //entity.IsMorning = chkIsMorning.Checked;
            //entity.IsNoon = chkIsNoon.Checked;
            //entity.IsEvening = chkIsEvening.Checked;
            //entity.IsNight = chkIsNight.Checked;
            //entity.MedicationPurpose = txtPurposeOfMedication.Text;
            //entity.MedicationAdministration = txtMedicationAdministration.Text;
            //entity.IsAsRequired = chkIsAsRequired.Checked;
            //entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
            //entity.TakenQty = Convert.ToDecimal(txtDispenseQty.Text);
            //entity.ResultQty = entity.DispenseQty;
            //entity.ChargeQty = entity.DispenseQty;
            //entity.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;
        }

        protected void cboLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
            hdnLocationID.Value = cboLocation.Value.ToString();
        }

        private void BindCboLocation()
        {
            if (cboServiceUnit.Value != null)
            {
                Location location = BusinessLayer.GetLocationList(string.Format("LocationID IN (SELECT LocationID FROM HealthcareServiceUnit WHERE HealthcareServiceUnitID = {0})", cboServiceUnit.Value)).FirstOrDefault();

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
            bool result = true;
            //vDrugInfo entityItem = null;
            //entityItem = BusinessLayer.GetvDrugInfoList(string.Format("ItemID = {0}", hdnItemID.Value))[0];
            //string isAllow = "0";

            //decimal dispenseQty = 0;
            //bool isDecimal = decimal.TryParse(txtDispenseQty.Text,out dispenseQty);

            //if (dispenseQty <= 0)
            //{
            //    result = false;
            //    errMessage = "Dispense Quantity should be greater than 0";
            //    return result;
            //}

            //if (IsPrescriptionItemValid(entityItem,ref errMessage, ref isAllow))
            //{
            //    IDbContext ctx = DbFactory.Configure(true);
            //    PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            //    PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            //    try
            //    {
            //        //PrescriptionOrderHd entityHd = null;
            //        //if (hdnTransactionID.Value == "")
            //        //{
            //        //    entityHd = new PrescriptionOrderHd();
            //        //    entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            //        //    entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
            //        //    entityHd.PrescriptionDate = Helper.GetDatePickerValue(txtTransactionDate);
            //        //    entityHd.PrescriptionTime = txtTransactionTime.Text;
            //        //    entityHd.ClassID = AppSession.RegisteredPatient.ClassID;
            //        //    entityHd.DispensaryServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
            //        //    entityHd.LocationID = Convert.ToInt32(cboLocation.Value);
            //        //    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            //        //    entityHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
            //        //    switch (AppSession.RegisteredPatient.DepartmentID)
            //        //    {
            //        //        case Constant.Facility.EMERGENCY:
            //        //            entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
            //        //            break;
            //        //        case Constant.Facility.OUTPATIENT:
            //        //            entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
            //        //            break;
            //        //        case Constant.Facility.INPATIENT:
            //        //            entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
            //        //            break;
            //        //        case Constant.Facility.DIAGNOSTIC:
            //        //            entityHd.TransactionCode = Constant.TransactionCode.OTHER_MEDICATION_ORDER;
            //        //            break;
            //        //        default:
            //        //            entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
            //        //            break;
            //        //    }
            //        //    entityHd.Remarks = txtRemarks.Text;
            //        //    entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
            //        //    ctx.CommandType = CommandType.Text;
            //        //    ctx.Command.Parameters.Clear();
            //        //    entityHd.CreatedBy = AppSession.UserLogin.UserID;
            //        //    entityHdDao.Insert(entityHd);

            //        //    entityHd.PrescriptionOrderID = BusinessLayer.GetPrescriptionOrderHdMaxID(ctx);
            //        //}
            //        //else
            //        //{
            //        //    entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
            //        //    entityHd.Remarks = txtRemarks.Text;
            //        //    entityHdDao.Update(entityHd);
            //        //}
            //        //retval = entityHd.PrescriptionOrderID.ToString();

            //        //PrescriptionOrderDt entityDt = new PrescriptionOrderDt();
            //        //ControlToEntity(entityDt);
            //        //if (entityDt.GenericName == "")
            //        //    entityDt.GenericName = entityItem.GenericName;
            //        //entityDt.PrescriptionOrderID = entityHd.PrescriptionOrderID;
            //        //entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;
            //        //entityDt.IsCreatedFromOrder = true;
            //        //entityDt.IsCompound = false;
            //        //entityDt.CreatedBy = AppSession.UserLogin.UserID;
            //        //entityDtDao.Insert(entityDt);
            //        //ctx.CommitTransaction();
            //    }
            //    catch (Exception ex)
            //    {
            //        result = false;
            //        errMessage = ex.Message;
            //        ctx.RollBackTransaction();
            //    }
            //    finally
            //    {
            //        ctx.Close();
            //    }
            //}
            //else
            //{
            //    result = false;
            //}
            return result; 
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            //vDrugInfo entityItem = null;
            //entityItem = BusinessLayer.GetvDrugInfoList(string.Format("ItemID = {0}", hdnItemID.Value))[0];
            //string isAllow = "0";

            //decimal dispenseQty = 0;
            //bool isDecimal = decimal.TryParse(txtDispenseQty.Text, out dispenseQty);

            //if (dispenseQty <= 0)
            //{
            //    result = false;
            //    errMessage = "Dispense Quantity should be greater than 0";
            //    return result;
            //}

            //if (IsPrescriptionItemValid(entityItem,ref errMessage, ref isAllow))
            //{
            //    try
            //    {
            //        //PrescriptionOrderDt entity = BusinessLayer.GetPrescriptionOrderDt(Convert.ToInt32(hdnEntryID.Value));
            //        //ControlToEntity(entity);
            //        //if (entity.GenericName == "")
            //        //    entity.GenericName = entityItem.GenericName;
            //        //entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            //        //BusinessLayer.UpdatePrescriptionOrderDt(entity);
            //        //retval = hdnTransactionID.Value;
            //        result = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        result = false;
            //        errMessage = ex.Message;
            //    } 
            //}
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

                //if (IsPrescriptionItemValid(entityItem, ref errMessage, ref isAllow))
                //{
                //    PrescriptionOrderHd entityHd = null;
                //    if (hdnTransactionID.Value == "")
                //    {
                //        entityHd = new PrescriptionOrderHd();
                //        entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                //        entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
                //        entityHd.PrescriptionDate = Helper.GetDatePickerValue(txtTransactionDate);
                //        entityHd.DispensaryServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
                //        entityHd.LocationID = Convert.ToInt32(cboLocation.Value);
                //        entityHd.PrescriptionTime = txtTransactionTime.Text;
                //        entityHd.ClassID = AppSession.RegisteredPatient.ClassID;
                //        entityHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
                //        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                //        switch (AppSession.RegisteredPatient.DepartmentID)
                //        {
                //            case Constant.Facility.EMERGENCY:
                //                entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                //                break;
                //            case Constant.Facility.OUTPATIENT:
                //                entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                //                break;
                //            case Constant.Facility.INPATIENT:
                //                entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                //                break;
                //            case Constant.Facility.DIAGNOSTIC:
                //                entityHd.TransactionCode = Constant.TransactionCode.OTHER_MEDICATION_ORDER;
                //                break;
                //            default:
                //                entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                //                break;
                //        }
                //        entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                //        ctx.CommandType = CommandType.Text;
                //        ctx.Command.Parameters.Clear();
                //        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                //        entityHdDao.Insert(entityHd);

                //        entityHd.PrescriptionOrderID = BusinessLayer.GetPrescriptionOrderHdMaxID(ctx);
                //    }
                //    else
                //    {
                //        entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                //    }
                //    retval = entityHd.PrescriptionOrderID.ToString();

                //    PrescriptionOrderDt entityDt = new PrescriptionOrderDt();

                //    entityDt.PrescriptionOrderID = entityHd.PrescriptionOrderID;
                //    entityDt.IsRFlag = true;
                //    entityDt.IsCompound = false;
                //    entityDt.ItemID = itemID;
                //    entityDt.DrugName = entityItem.ItemName1;
                //    entityDt.GenericName = entityItem.GenericName;
                //    entityDt.GCDrugForm = entityItem.GCDrugForm;
                //    entityDt.Dose = entityItem.Dose;
                //    entityDt.GCDoseUnit = entityItem.GCDoseUnit;

                //    entityDt.StartDate = AppSession.RegisteredPatient.VisitDate;
                //    entityDt.StartTime = AppSession.RegisteredPatient.VisitTime;

                //    bool isDay = true;
                //    bool isPrn = false;
                //    if (param[1].ToLower().Contains("prn"))
                //    {
                //        isPrn = true;
                //        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                //        entityDt.Frequency = 1;
                //        entityDt.IsMorning = true;
                //    }
                //    else
                //    {
                //        if (!param[1].ToLower().Contains("qh") && !param[1].ToLower().Contains("dd"))
                //        {
                //            switch (param[1])
                //            {
                //                case Constant.PrescriptionFrequency.QD:
                //                    entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                //                    entityDt.Frequency = 1;
                //                    entityDt.IsMorning = true;
                //                    break;
                //                case Constant.PrescriptionFrequency.BID:
                //                    entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                //                    entityDt.Frequency = 2;
                //                    entityDt.IsMorning = true;
                //                    entityDt.IsNoon = true;
                //                    break;
                //                case Constant.PrescriptionFrequency.TID:
                //                    entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                //                    entityDt.Frequency = 3;
                //                    entityDt.IsMorning = true;
                //                    entityDt.IsNoon = true;
                //                    entityDt.IsNight = true;
                //                    break;
                //                case Constant.PrescriptionFrequency.QID:
                //                    entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                //                    entityDt.Frequency = 4;
                //                    entityDt.IsMorning = true;
                //                    entityDt.IsNoon = true;
                //                    entityDt.IsEvening = true;
                //                    entityDt.IsNight = true;
                //                    break;
                //                default:
                //                    entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                //                    entityDt.Frequency = 1;
                //                    entityDt.IsMorning = true;
                //                    break;
                //            }
                //        }
                //        else
                //        {
                //            #region Default Frequency
                //            string frequency = "1";
                //            entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                //            #endregion

                //            if (param[1].ToLower().Contains("qh"))
                //            {
                //                frequency = param[1].Substring(2);
                //                isDay = false;
                //                entityDt.GCDosingFrequency = Constant.DosingFrequency.HOUR;
                //            }
                //            else
                //            {
                //                frequency = param[1].Substring(0, 1);
                //                entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                //            }
                //            entityDt.Frequency = Convert.ToInt16(frequency);

                //            switch (entityDt.Frequency)
                //            {
                //                case 1:
                //                    entityDt.IsMorning = true;
                //                    break;
                //                case 2:
                //                    entityDt.IsMorning = true;
                //                    entityDt.IsNoon = true;
                //                    break;
                //                case 3:
                //                    entityDt.IsMorning = true;
                //                    entityDt.IsNoon = true;
                //                    entityDt.IsNight = true;
                //                    break;
                //                case 4:
                //                    entityDt.IsMorning = true;
                //                    entityDt.IsNoon = true;
                //                    entityDt.IsEvening = true;
                //                    entityDt.IsNight = true;
                //                    break;
                //                default:
                //                    entityDt.IsMorning = true;
                //                    entityDt.IsNoon = true;
                //                    entityDt.IsEvening = true;
                //                    entityDt.IsNight = true;
                //                    break;
                //            }
                //        } 
                //    }

                //    entityDt.NumberOfDosage = Convert.ToDecimal(param[2]);
                //    entityDt.GCDosingUnit = entityItem.GCItemUnit;
                //    entityDt.GCRoute = entityItem.GCMedicationRoute;
                //    entityDt.DispenseQty = Convert.ToDecimal(param[3]);
                //    entityDt.IsCreatedFromOrder = true;

                //    if (!entityItem.IsExternalMedication)
                //    {
                //        if (!isPrn)
                //        {
                //            if (isDay)
                //            {
                //                entityDt.DosingDuration = Math.Ceiling(entityDt.DispenseQty / (entityDt.Frequency * entityDt.NumberOfDosage));
                //                //entityDt.DispenseQty = (decimal)(entityDt.Frequency * entityDt.NumberOfDosage * entityDt.DosingDuration);
                //            }
                //            else
                //            {
                //                decimal numberOfTaken = Math.Ceiling(Convert.ToDecimal(24 / entityDt.Frequency));
                //                entityDt.DosingDuration = Math.Ceiling(entityDt.DispenseQty / (numberOfTaken * entityDt.NumberOfDosage));
                //                //entityDt.DispenseQty = (decimal)(numberOfTaken * entityDt.NumberOfDosage * entityDt.DosingDuration);
                //            }
                //        }
                //        else
                //        {
                //            entityDt.DosingDuration = 1;
                //        }
                //    }
                //    else
                //    {
                //        entityDt.DosingDuration = 1;
                //    }

                //    entityDt.IsAsRequired = isPrn;
                //    entityDt.TakenQty = entityDt.DispenseQty;
                //    entityDt.ResultQty = entityDt.DispenseQty;
                //    entityDt.ChargeQty = entityDt.DispenseQty;
                //    if (param.Length==5)
                //    {
                //        entityDt.MedicationAdministration = param[4]; 
                //    }
                //    entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;
                //    entityDt.CreatedBy = AppSession.UserLogin.UserID;

                //    entityDtDao.Insert(entityDt);
                //    ctx.CommitTransaction();
                //}
                //else
                //{
                //    result = false;
                //}
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
    }
}