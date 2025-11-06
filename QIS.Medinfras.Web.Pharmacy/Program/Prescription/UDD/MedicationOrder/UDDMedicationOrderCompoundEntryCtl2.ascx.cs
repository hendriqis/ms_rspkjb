using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class UDDMedicationOrderCompoundEntryCtl2 : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            hdnQueryString.Value = queryString;
            string[] param = queryString.Split('|');
            hdnLocationID.Value = param[10];

            ledProduct.FilterExpression = string.Format("LocationID = {0} AND IsDeleted = 0",hdnLocationID.Value);
            
            if (param[0] == "add")
            {
                IsAdd = true;
                SetControlProperties();
                tblTemplate.Style.Add("display", "table");
            }
            else
            {
                IsAdd = true;
                SetControlProperties();
                int prescriptionDetailID = Convert.ToInt32(param[2]);
                PrescriptionOrderDt entity = BusinessLayer.GetPrescriptionOrderDt(prescriptionDetailID);
                EntityToControl(entity);
                tblTemplate.Style.Add("display", "none");
            }

            hdnParamedicID.Value = param[5];
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtCompoundMedicationName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(cboFrequencyTimelineCompoundCtl, new ControlEntrySetting(true, true, true, Constant.DosingFrequency.DAY));
            SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(cboDosingUnitCompoundCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(chkIsUsingSweetener, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboEmbalace, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmbalaceQty, new ControlEntrySetting(true, true, false, "1"));
            SetControlEntrySetting(cboMedicationRouteCompoundCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCoenamRuleCompoundCtl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMedicationPurpose, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(PrescriptionOrderDt entity)
        {
            txtDispenseQty.Text = entity.DispenseQty.ToString();
            txtTakenQty.Text = entity.TakenQty.ToString();
            txtCompoundMedicationName.Text = entity.CompoundDrugname;
            cboMedicationRouteCompoundCtl.Value = entity.GCRoute;
            txtDosingDose.Text = entity.NumberOfDosage.ToString();
            cboDosingUnitCompoundCtl.Value = entity.GCDosingUnit;
            chkIsUsingSweetener.Checked = entity.IsUseSweetener;

            cboFrequencyTimelineCompoundCtl.Value = entity.GCDosingFrequency;
            txtFrequencyNumber.Text = entity.Frequency.ToString();
            txtDosingDuration.Text = entity.DosingDuration.ToString();

            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = entity.StartTime;

            txtMedicationAdministration.Text = entity.MedicationAdministration;
            txtMedicationPurpose.Text = entity.MedicationPurpose;

        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.COENAM_RULE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboCompoundUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimelineCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCompoundStrengthUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnitCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1" || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRouteCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRuleCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            List<EmbalaceHd> lstEmbalace = BusinessLayer.GetEmbalaceHdList("IsDeleted = 0");
            lstEmbalace.Insert(0, new EmbalaceHd() { EmbalaceID = 0, EmbalaceName = "" });
            Methods.SetComboBoxField<EmbalaceHd>(cboEmbalace, lstEmbalace, "EmbalaceName", "EmbalaceID");
            cboEmbalace.SelectedIndex = 0;

            cboFrequencyTimelineCompoundCtl.Value = Constant.DosingFrequency.DAY;
            cboMedicationRouteCompoundCtl.SelectedIndex = 0;

            txtStartDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtDispenseQty.Text = "1";
            txtTakenQty.Text = "1";
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            //isAdd;0;genericName;itemID;doseqty;gcdoseunit;compoundqty;gcitemunit;conversionfactor;itemname;doselabel;doseunit;gcdoseunit;itemunit;gcitemunit

            bool result = true;
            String[] listParam = hdnInlineEditingData.Value.Split('|');

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);

            PatientChargesHdDao entityChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            try
            {
                int prescriptionID = -1;
                int transactionID = -1;
                string transactionNo;
                string[] paramHeader = hdnQueryString.Value.ToString().Split('|');
                if (paramHeader[2] == "0")
                {
                    #region Save Header
                    DateTime prescriptionDate = Helper.DateInStringToDateTime(paramHeader[3]);
                    string prescriptionTime = paramHeader[4];
                    int paramedicID = Convert.ToInt32(hdnParamedicID.Value);

                    PrescriptionOrderHd entityHd = new PrescriptionOrderHd();
                    entityHd.PrescriptionDate = prescriptionDate;
                    entityHd.PrescriptionTime = prescriptionTime;
                    entityHd.ParamedicID = paramedicID;
                    entityHd.VisitID = Convert.ToInt32(paramHeader[7]);
                    entityHd.ClassID = Convert.ToInt32(paramHeader[14]);
                    entityHd.GCPrescriptionType = paramHeader[13];
                    entityHd.DispensaryServiceUnitID = Convert.ToInt32(paramHeader[11]);
                    entityHd.LocationID = Convert.ToInt32(paramHeader[9]);
                    entityHd.GCRefillInstruction = paramHeader[6];
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                    entityHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                    switch (paramHeader[12])
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
                            if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() == hdnImagingServiceUnitID.Value)
                                entityHd.TransactionCode = Constant.TransactionCode.IMAGING_MEDICATION_ORDER;
                            else if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() == hdnLaboratoryServiceUnitID.Value)
                                entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_MEDICATION_ORDER;
                            else
                                entityHd.TransactionCode = Constant.TransactionCode.PH_MEDICATION_ORDER;
                            break;
                        case Constant.Facility.PHARMACY:
                            entityHd.TransactionCode = Constant.TransactionCode.PH_MEDICATION_ORDER;
                            break;
                        default:
                            entityHd.TransactionCode = Constant.TransactionCode.PH_MEDICATION_ORDER;
                            break;
                    }
                    entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHd.LastUpdatedDate = DateTime.Now;
                    entityHd.IsCreatedBySystem = true;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    prescriptionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                    #endregion

                    #region PatientChargesHd
                    PatientChargesHd entityPatientChargesHd = new PatientChargesHd();

                    entityPatientChargesHd.VisitID = Convert.ToInt32(paramHeader[7]);
                    entityPatientChargesHd.TransactionDate = prescriptionDate;
                    entityPatientChargesHd.TransactionTime = prescriptionTime;
                    entityPatientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityPatientChargesHd.PrescriptionOrderID = prescriptionID;
                    entityPatientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(paramHeader[11]);
                    switch (paramHeader[12])
                    {
                        case Constant.Facility.EMERGENCY:
                            entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_EMERGENCY;
                            break;
                        case Constant.Facility.OUTPATIENT:
                            entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_OUTPATIENT;
                            break;
                        case Constant.Facility.INPATIENT:
                            entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_INPATIENT;
                            break;
                        case Constant.Facility.PHARMACY:
                            entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PH_CHARGES;
                            break;
                        case Constant.Facility.MEDICAL_CHECKUP:
                            entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_OTHER;
                            break;
                        case Constant.Facility.DIAGNOSTIC:
                            if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() == hdnImagingServiceUnitID.Value)
                                entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_IMAGING;
                            else if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() == hdnLaboratoryServiceUnitID.Value)
                                entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_LABORATORY;
                            else
                                entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_OTHER;
                            break;
                    }
                    entityPatientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityPatientChargesHd.TransactionCode, entityPatientChargesHd.TransactionDate, ctx);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityPatientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                    transactionID = entityChargesHdDao.InsertReturnPrimaryKeyID(entityPatientChargesHd);
                    transactionNo = entityPatientChargesHd.TransactionNo;
                    #endregion
                }
                else
                {
                    prescriptionID = Convert.ToInt32(paramHeader[1]);
                    transactionID = Convert.ToInt32(paramHeader[2]);
                    if (transactionID > 0)
                    {
                        if (paramHeader[2] != "")
                        {
                            #region Delete
                            int prescriptionDetailID = Convert.ToInt32(paramHeader[2]);

                            string filterExpression = String.Format("(PrescriptionOrderDetailID = {0} OR ParentID = {0}) AND IsCompound = 1 AND IsDeleted = 0", prescriptionDetailID);
                            List<PrescriptionOrderDt> lstDeletedEntity = BusinessLayer.GetPrescriptionOrderDtList(filterExpression);
                            foreach (PrescriptionOrderDt deletedEntity in lstDeletedEntity)
                            {
                                deletedEntity.IsDeleted = true;
                                deletedEntity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(deletedEntity);

                                PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDtList(String.Format("PrescriptionOrderDetailID = {0}", deletedEntity.PrescriptionOrderDetailID), ctx)[0];
                                entityChargesDt.PrescriptionOrderDetailID = null;
                                entityChargesDt.IsDeleted = true;
                                entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityChargesDtDao.Update(entityChargesDt);
                            }
                            #endregion 
                        }
                    }
                }
                retval = transactionID.ToString();

                int ctr = 0;
                int parentID = 0;
                foreach (String param in listParam)
                {
                    String[] data = param.Split(';');
                    bool isChanged = data[0] == "1" ? true : false;
                    string itemID = param.Split(';')[3];
                    int ID = Convert.ToInt32(data[1]);
                    if (itemID != "")
                    {
                        PrescriptionOrderDt entityDt = new PrescriptionOrderDt();

                        entityDt.IsCompound = true;
                        entityDt.IsUsingUDD = false;
                        entityDt.PrescriptionOrderID = prescriptionID;
                        entityDt.GenericName = data[2];
                        if (data[3] != "")
                            entityDt.ItemID = Convert.ToInt32(data[3]);
                        else
                            entityDt.ItemID = null;
                        if (data[5] != "")
                        {
                            entityDt.Dose = Convert.ToDecimal(data[4]);
                            entityDt.GCDoseUnit = data[5];
                        }
                        else
                        {
                            entityDt.Dose = null;
                            entityDt.GCDoseUnit = null;
                        }
                        string GCItemUnit = data[14];

                        //////////// comment by RN (20190806) : ditutup karena perhitungan racikan dengan "/"
                        ////string compoundQty = data[6];
                        ////decimal qty = 0;
                        ////if (compoundQty.Contains('/'))
                        ////{
                        ////    string[] compoundInfo = compoundQty.Split('/');
                        ////    decimal num1 = Convert.ToDecimal(compoundInfo[0]);
                        ////    decimal num2 = Convert.ToDecimal(compoundInfo[1]);
                        ////    qty = Math.Round(num1 / num2, 2);
                        ////}
                        ////else
                        ////{
                        ////    qty = Convert.ToDecimal(compoundQty);
                        ////}

                        ////entityDt.GCCompoundUnit = data[7];
                        ////entityDt.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
                        ////entityDt.TakenQty = Convert.ToDecimal(txtTakenQty.Text);
                        ////if (GCItemUnit != entityDt.GCCompoundUnit)
                        ////{
                        ////    decimal dose = Convert.ToDecimal(data[10]);
                        ////    entityDt.CompoundQtyInString = data[6];
                        ////    entityDt.CompoundQty = qty / dose;
                        ////    entityDt.ResultQty = qty * entityDt.TakenQty / dose;
                        ////}
                        ////else
                        ////{
                        ////    entityDt.CompoundQty = qty;
                        ////    entityDt.CompoundQtyInString = data[6];
                        ////    entityDt.ResultQty = qty * entityDt.TakenQty;
                        ////}

                        entityDt.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
                        entityDt.TakenQty = Convert.ToDecimal(txtTakenQty.Text);
                        if (GCItemUnit != entityDt.GCCompoundUnit)
                        {
                            string compoundQty = data[6];
                            decimal qty = 0;
                            if (compoundQty.Contains('/'))
                            {
                                string[] compoundInfo = compoundQty.Split('/');
                                decimal num1 = Convert.ToDecimal(compoundInfo[0]);
                                decimal num2 = Convert.ToDecimal(compoundInfo[1]);
                                //qty = Math.Round(num1 / num2, 2);

                                decimal dose = Convert.ToDecimal(data[10]);
                                entityDt.CompoundQtyInString = compoundQty;
                                entityDt.CompoundQty = num1 / num2 / dose;
                                entityDt.ResultQty = num1 / num2 * entityDt.TakenQty / dose;
                            }
                            else
                            {
                                qty = Convert.ToDecimal(compoundQty);

                                decimal dose = Convert.ToDecimal(data[10]);
                                entityDt.CompoundQtyInString = compoundQty;
                                entityDt.CompoundQty = qty / dose;
                                entityDt.ResultQty = qty * entityDt.TakenQty / dose;
                            }
                        }
                        else
                        {
                            string compoundQty = data[6];
                            decimal qty = 0;
                            if (compoundQty.Contains('/'))
                            {
                                string[] compoundInfo = compoundQty.Split('/');
                                decimal num1 = Convert.ToDecimal(compoundInfo[0]);
                                decimal num2 = Convert.ToDecimal(compoundInfo[1]);
                                //qty = Math.Round(num1 / num2, 2);

                                entityDt.CompoundQty = num1 / num2;
                                entityDt.CompoundQtyInString = compoundQty;
                                entityDt.ResultQty = num1 / num2 * entityDt.TakenQty;
                            }
                            else
                            {
                                qty = Convert.ToDecimal(compoundQty);

                                entityDt.CompoundQty = qty;
                                entityDt.CompoundQtyInString = compoundQty;
                                entityDt.ResultQty = qty * entityDt.TakenQty;
                            }
                        }

                        entityDt.DrugName = data[9];
                        entityDt.ChargeQty = entityDt.ResultQty;
                        if (data[15] == Constant.QuantityDeductionType.DIBULATKAN) entityDt.ChargeQty = Math.Ceiling(entityDt.ChargeQty);
                        entityDt.CompoundDrugname = txtCompoundMedicationName.Text;
                        entityDt.GCRoute = cboMedicationRouteCompoundCtl.Value.ToString();
                        entityDt.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
                        entityDt.GCDosingUnit = cboDosingUnitCompoundCtl.Value.ToString();
                        entityDt.IsUseSweetener = chkIsUsingSweetener.Checked;
                        entityDt.GCDosingFrequency = cboFrequencyTimelineCompoundCtl.Value.ToString();
                        entityDt.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
                        entityDt.DosingDuration = Convert.ToInt16(txtDosingDuration.Text);
                        entityDt.StartDate = Helper.GetDatePickerValue(txtStartDate);
                        entityDt.StartTime = txtStartTime.Text;
                        entityDt.MedicationAdministration = txtMedicationAdministration.Text;
                        entityDt.MedicationPurpose = txtMedicationPurpose.Text;
                        entityDt.GCPrescriptionOrderStatus = Constant.OrderStatus.IN_PROGRESS;
                        if (ctr > 0)
                        {
                            entityDt.IsRFlag = false;
                            entityDt.ParentID = parentID;
                        }
                        else
                        {
                            entityDt.IsRFlag = true;
                            entityDt.ParentID = null;
                        }

                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDt.PrescriptionOrderDetailID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

                        if (ctr < 1)
                        {
                            parentID = entityDt.PrescriptionOrderDetailID;
                        }

                        #region PatientChargesDt
                        PatientChargesDt entityChargesDt = new PatientChargesDt();
                        entityChargesDt.PrescriptionOrderDetailID = entityDt.PrescriptionOrderDetailID;
                        entityChargesDt.TransactionID = transactionID;
                        entityChargesDt.ItemID = (int)entityDt.ItemID;
                        entityChargesDt.LocationID = Convert.ToInt32(paramHeader[9]);
                        entityChargesDt.ChargeClassID = Convert.ToInt32(paramHeader[14]);
                        entityChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(AppSession.RegisteredPatient.RegistrationID), AppSession.RegisteredPatient.VisitID, entityChargesDt.ChargeClassID, entityChargesDt.ItemID, 2, DateTime.Now, ctx);
                        decimal discountAmount = 0;
                        decimal coverageAmount = 0;
                        decimal price = 0;
                        decimal basePrice = 0;
                        decimal tariffComp1 = 0;
                        bool isCoverageInPercentage = false;
                        bool isDiscountInPercentage = false;
                        if (list.Count > 0)
                        {
                            GetCurrentItemTariff objItemTariff = list[0];
                            discountAmount = objItemTariff.DiscountAmount;
                            coverageAmount = objItemTariff.CoverageAmount;
                            tariffComp1 = objItemTariff.PriceComp1;
                            price = objItemTariff.Price;
                            basePrice = objItemTariff.BasePrice;
                            isCoverageInPercentage = objItemTariff.IsCoverageInPercentage;
                            isDiscountInPercentage = objItemTariff.IsDiscountInPercentage;
                        }

                        entityChargesDt.BaseTariff = basePrice;
                        entityChargesDt.Tariff = price;
                        entityChargesDt.TariffComp1 = tariffComp1;
                        entityChargesDt.UsedQuantity = entityDt.ResultQty;
                        if (data[16] == Constant.QuantityDeductionType.DIBULATKAN) entityChargesDt.UsedQuantity = Math.Ceiling(entityChargesDt.UsedQuantity);
                        entityChargesDt.ChargedQuantity = entityDt.ChargeQty;
                        entityChargesDt.BaseQuantity = entityChargesDt.UsedQuantity;
                        entityChargesDt.GCItemUnit = GCItemUnit;
                        entityChargesDt.GCBaseUnit = GCItemUnit;

                        decimal grossLineAmount = 0;
                        if (ctr == 0)
                        {
                            if (entityDt.EmbalaceID != null)
                            {
                                EmbalaceDt embalace = BusinessLayer.GetEmbalaceDtList(string.Format("EmbalaceID = {0} AND StartingQty <= {1} AND EndingQty >= {1}", entityDt.EmbalaceID, entityDt.EmbalaceQty)).FirstOrDefault();
                                entityChargesDt.EmbalaceAmount = Convert.ToDecimal(embalace.Tariff * entityDt.EmbalaceQty);
                            }
                            else
                                entityChargesDt.EmbalaceAmount = 0;
                            entityChargesDt.PrescriptionFeeAmount = Convert.ToDecimal(paramHeader[8]);
                            grossLineAmount = (entityChargesDt.ChargedQuantity * price)+entityChargesDt.EmbalaceAmount+entityChargesDt.PrescriptionFeeAmount;
                        }
                        else
                        {
                            entityChargesDt.EmbalaceAmount = 0;
                            entityChargesDt.PrescriptionFeeAmount = 0;
                            grossLineAmount = entityChargesDt.ChargedQuantity * price;
                        }

                        decimal totalDiscountAmount = 0;
                        if (isDiscountInPercentage)
                            totalDiscountAmount = grossLineAmount * discountAmount / 100;
                        else
                            totalDiscountAmount = discountAmount * 1;
                        if (totalDiscountAmount > grossLineAmount)
                            totalDiscountAmount = grossLineAmount;

                        decimal total = grossLineAmount - totalDiscountAmount;
                        decimal totalPayer = 0;
                        if (isCoverageInPercentage)
                            totalPayer = total * coverageAmount / 100;
                        else
                            totalPayer = coverageAmount * 1;
                        if (totalPayer > total)
                            totalPayer = total;

                        entityChargesDt.ConversionFactor = entityDt.ConversionFactor;
                        entityChargesDt.IsCITO = false;
                        entityChargesDt.CITOAmount = 0;
                        entityChargesDt.IsComplication = false;
                        entityChargesDt.ComplicationAmount = 0;
                        entityChargesDt.IsDiscount = false;
                        entityChargesDt.DiscountAmount = totalDiscountAmount;
                        entityChargesDt.PatientAmount = total - totalPayer;
                        entityChargesDt.PayerAmount = totalPayer;
                        entityChargesDt.LineAmount = total;

                        entityChargesDt.CreatedBy = entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityChargesDtDao.Insert(entityChargesDt);
                        #endregion
                        ctr++;
                    }
                }
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
    }
}