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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PrescriptionCompoundEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            hdnDateToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            Int32 hour = DateTime.Now.Hour;
            Int32 minute = DateTime.Now.Minute;
            string hourInString = "";
            string minuteInString = "";
            if (hour < 10)
            {
                hourInString = string.Format("0{0}", hour);
            }
            else
            {
                hourInString = string.Format("{0}", hour);
            }

            if (minute < 10)
            {
                minuteInString = string.Format("0{0}", minute);
            }
            else
            {
                minuteInString = string.Format("{0}", minute);
            }
            hdnTimeToday.Value = string.Format("{0}:{1}", hourInString, minuteInString);

            hdnQueryString.Value = queryString;
            if (AppSession.UserLogin.ParamedicID != null)
                hdnParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            string[] param = queryString.Split('|');

            if (param.Length > 3)
            {
                IsAdd = true;
                SetControlProperties();
                tblTemplate.Style.Add("display", "table");
                hdnIsAddNew.Value = "1";
                hdnLocationIDCtlCompound.Value = param[6];
            }
            else
            {
                IsAdd = true;
                SetControlProperties();
                int prescriptionDetailID = Convert.ToInt32(param[1]);
                PrescriptionOrderDt entity = BusinessLayer.GetPrescriptionOrderDt(prescriptionDetailID);
                EntityToControl(entity);
                tblTemplate.Style.Add("display", "none");
                divSaveAsNewTemplate.Style.Add("display", "none");
                hdnLocationIDCtlCompound.Value = param[2];
            }

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
            SetControlEntrySetting(chkIsPRN, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboEmbalaceCompoundCtl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmbalaceQty, new ControlEntrySetting(true, true, false, "1"));
            SetControlEntrySetting(cboMedicationRouteCompoundCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCoenamRuleCompoundCtl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(PrescriptionOrderDt entity)
        {
            txtDispenseQty.Text = entity.DispenseQty.ToString();
            txtCompoundMedicationName.Text = entity.CompoundDrugname;
            cboMedicationRouteCompoundCtl.Value = entity.GCRoute;
            txtDosingDose.Text = entity.NumberOfDosage.ToString();
            cboDosingUnitCompoundCtl.Value = entity.GCDosingUnit;
            chkIsUsingSweetener.Checked = entity.IsUseSweetener;
            chkIsPRN.Checked = entity.IsAsRequired;

            cboFrequencyTimelineCompoundCtl.Value = entity.GCDosingFrequency;
            txtFrequencyNumber.Text = entity.Frequency.ToString();
            txtDosingDuration.Text = entity.DosingDuration.ToString();
            cboCoenamRuleCompoundCtl.Value = entity.GCCoenamRule;

            if (entity.SignaID != null && entity.SignaID != 0)
            {
                hdnSignaIDCompound.Value = entity.SignaID.ToString();
                Signa signa = BusinessLayer.GetSigna(Convert.ToInt32(entity.SignaID));
                txtSignaLabelCompound.Text = signa.SignaLabel;
                txtSignaName1Compound.Text = signa.SignaName1;
            }

            chkIsMorning.Checked = entity.IsMorning;
            chkIsNoon.Checked = entity.IsNoon;
            chkIsEvening.Checked = entity.IsEvening;
            chkIsNight.Checked = entity.IsNight;

            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = entity.StartTime;
            txtStartTime1.Text = entity.Sequence1Time;
            txtStartTime2.Text = entity.Sequence2Time;
            txtStartTime3.Text = entity.Sequence3Time;
            txtStartTime4.Text = entity.Sequence4Time;
            txtStartTime5.Text = entity.Sequence5Time;
            txtStartTime6.Text = entity.Sequence6Time;

            if (entity.EmbalaceID != null && entity.EmbalaceID != 0)
            {
                EmbalaceHd embalace = BusinessLayer.GetEmbalaceHd((int)entity.EmbalaceID);
                cboEmbalaceCompoundCtl.Value = entity.EmbalaceID.ToString();
                txtEmbalaceQty.Text = entity.EmbalaceQty.ToString(Constant.FormatString.NUMERIC_2);
                if (!embalace.IsUsingRangePricing)
                {
                    txtEmbalaceQty.Attributes.Remove("readonly");
                }
                else
                {
                    txtEmbalaceQty.Attributes.Add("readonly", "readonly");
                }
            }

            txtMedicationAdministration.Text = entity.MedicationAdministration;
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

            filterExpression = string.Format("HealthcareID = '{0}'  AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                                                    AppSession.UserLogin.HealthcareID,
                                                    Constant.SettingParameter.PH_USED_STRENGTH_UNIT_AS_DEFAULT,
                                                    Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS,
                                                    Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS,
                                                    Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN,
                                                    Constant.SettingParameter.FN_PENJAMIN_INHEALTH,
                                                    Constant.SettingParameter.EM_PEMBATASAN_CPOE_INHEALTH,
                                                    Constant.SettingParameter.EM_ORDER_RESEP_HANYA_BISA_PILIH_ITEM_STOK_RS,
                                                    Constant.SettingParameter.EMR_COMPOUND_ONLINE_PRES_EMBALACEQTY_AUTOMATIC_FILLED,
                                                    Constant.SettingParameter.EM_IS_VALIDATION_EMPTY_STOCK_PRESCRIPTION_ORDER
                                                );

            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);

            SettingParameterDt oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.PH_USED_STRENGTH_UNIT_AS_DEFAULT).FirstOrDefault();
            hdnIsDefaultUsingStrengthUnit.Value = oParam != null ? oParam.ParameterValue : "0";
            hdnValidationEmptyStockCtlCompound.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_IS_VALIDATION_EMPTY_STOCK_PRESCRIPTION_ORDER).FirstOrDefault().ParameterValue;

            SettingParameterDt oParam1 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS).FirstOrDefault();
            SettingParameterDt oParam2 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault();
            SettingParameterDt oParam3 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_INHEALTH).FirstOrDefault();
            SettingParameterDt oParam4 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_INHEALTH).FirstOrDefault();

            hdnPrescriptionValidateStockAllRS.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_ORDER_RESEP_HANYA_BISA_PILIH_ITEM_STOK_RS).FirstOrDefault().ParameterValue;
            hdnIsEmbalaceQtyTerisiOtomatis.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EMR_COMPOUND_ONLINE_PRES_EMBALACEQTY_AUTOMATIC_FILLED).FirstOrDefault().ParameterValue;

            string bpjsID = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault().ParameterValue;
            string bpjsType = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).FirstOrDefault().ParameterValue;
            bool isLimitedCPOEItemForBPJS = oParam1 != null ? (oParam1.ParameterValue == "1" ? true : false) : false;

            if (string.IsNullOrEmpty(bpjsID))
                bpjsID = "0";

            bool isLimitedCPOEItemForInhealth = oParam3 != null ? (oParam3.ParameterValue == "1" ? true : false) : false;
            string inHealthCustomerType = oParam4 != null ? oParam4.ParameterValue : string.Empty;

            string lookupEditFilterExp = String.Format("IsDeleted = 0 AND GCItemType = '{0}'", Constant.ItemType.OBAT_OBATAN);

            if (AppSession.RegisteredPatient.BusinessPartnerID.ToString() == bpjsID && isLimitedCPOEItemForBPJS)
            {
                lookupEditFilterExp = "IsBPJSFormularium = 1 AND IsDeleted = 0";
            }

            if (AppSession.RegisteredPatient.GCCustomerType == inHealthCustomerType && isLimitedCPOEItemForInhealth)
            {
                lookupEditFilterExp = "IsInhealthFormularium = 1 AND IsDeleted = 0";
            }

            if (hdnPrescriptionValidateStockAllRS.Value == "1")
            {
                lookupEditFilterExp += " AND QtyOnHandAll > 0";
            }

            lookupEditFilterExp += string.Format(" AND ISNULL(GCItemStatus,'') != '{0}'", Constant.ItemStatus.IN_ACTIVE);

            ledProduct.FilterExpression = lookupEditFilterExp;

            List<EmbalaceHd> lstEmbalace = BusinessLayer.GetEmbalaceHdList("IsDeleted = 0");
            lstEmbalace.Insert(0, new EmbalaceHd() { EmbalaceID = 0, EmbalaceName = "" });
            Methods.SetComboBoxField<EmbalaceHd>(cboEmbalaceCompoundCtl, lstEmbalace, "EmbalaceName", "EmbalaceID");
            cboEmbalaceCompoundCtl.SelectedIndex = 0;

            cboFrequencyTimelineCompoundCtl.Value = Constant.DosingFrequency.DAY;
            cboMedicationRouteCompoundCtl.SelectedIndex = 0;

            txtStartDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtDispenseQty.Text = "1";
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            String[] listParam = hdnInlineEditingData.Value.Split('|');
            foreach (String param in listParam)
            {
                String[] dataSplit = param.Split(';');

                bool isChanged = dataSplit[0] == "1" ? true : false;
                int ID = Convert.ToInt32(dataSplit[1]);
                if (isChanged || ID > 0)
                {
                    decimal qtyCheck = Convert.ToDecimal(dataSplit[4]);
                    string[] compoundInfo = dataSplit[6].Split('/');
                    if (compoundInfo.Count() > 1)
                    {
                        decimal num1 = Convert.ToDecimal(compoundInfo[0].Replace(',', '.'));
                        decimal num2 = Convert.ToDecimal(compoundInfo[1].Replace(',', '.'));
                        qtyCheck = Math.Round(num1 / num2, 2);
                    }

                    if (qtyCheck < 0)
                    {
                        result = false;
                        errMessage = "Compound Quantity should be greater than 0";
                    }
                }

                if (Convert.ToInt16(txtFrequencyNumber.Text) < 0)
                {
                    result = false;
                    errMessage = "Frequency Quantity should be greater than 0";
                }

                if (Convert.ToDecimal(txtDosingDose.Text) < 0)
                {
                    result = false;
                    errMessage = "Dose Quantity should be greater than 0";
                }

                if (Convert.ToInt16(txtDosingDuration.Text) < 0)
                {
                    result = false;
                    errMessage = "Duration should be greater than 0";
                }

                if (Convert.ToDecimal(txtEmbalaceQty.Text) < 0)
                {
                    result = false;
                    errMessage = "Embalace Quantity should be greater than 0";
                }
            }

            if (result)
            {
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
                PrescriptionOrderHdOriginalDao entityHdOriginalDao = new PrescriptionOrderHdOriginalDao(ctx);
                PrescriptionOrderDtOriginalDao entityDtOriginalDao = new PrescriptionOrderDtOriginalDao(ctx);
                CompoundTemplateHdDao templateHdDao = new CompoundTemplateHdDao(ctx);
                CompoundTemplateDtDao templateDtDao = new CompoundTemplateDtDao(ctx);
                PhysicianCompoundTemplateDao physicianTemplateDao = new PhysicianCompoundTemplateDao(ctx);
                RegistrationDao registratioDao = new RegistrationDao(ctx);
                ConsultVisitDao visitDao = new ConsultVisitDao(ctx);

                CompoundTemplateHd templateHd = null;
                List<CompoundTemplateDt> templateDtList = null;

                bool isSaveAsNewTemplate = chkSaveAsNewTemplate.Checked;
                try
                {
                    ConsultVisit visit = visitDao.Get(AppSession.RegisteredPatient.VisitID);
                    if (visit != null)
                    {
                        Registration reg = registratioDao.Get(visit.RegistrationID);
                        if (reg.MRN == AppSession.RegisteredPatient.MRN)
                        {
                            int prescriptionID = -1;
                            string[] paramHeader = hdnQueryString.Value.ToString().Split('|');
                            bool isAdd = paramHeader[0] == "";
                            if (paramHeader[0] == "")
                            {
                                #region Save Header
                                DateTime prescriptionDate = Helper.DateInStringToDateTime(paramHeader[1]);
                                string prescriptionTime = paramHeader[2];
                                int paramedicID = Convert.ToInt32(paramHeader[3]);

                                PrescriptionOrderHd entityHd = new PrescriptionOrderHd();
                                entityHd.PrescriptionDate = prescriptionDate;
                                entityHd.PrescriptionTime = prescriptionTime;
                                entityHd.ParamedicID = paramedicID;
                                entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
                                entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                                entityHd.ClassID = AppSession.RegisteredPatient.ClassID;
                                entityHd.GCPrescriptionType = paramHeader[4];
                                entityHd.DispensaryServiceUnitID = Convert.ToInt32(paramHeader[5]);
                                entityHd.LocationID = Convert.ToInt32(paramHeader[6]);
                                entityHd.GCRefillInstruction = paramHeader[7];
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
                                    default:
                                        entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                                        break;
                                }
                                entityHd.IsCreatedBySystem = false;
                                entityHd.IsOrderedByPhysician = true;
                                entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                prescriptionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                                #endregion
                            }
                            else
                            {
                                prescriptionID = Convert.ToInt32(paramHeader[0]);
                                #region Delete
                                int prescriptionDetailID = Convert.ToInt32(paramHeader[1]);
                                StringBuilder listID = new StringBuilder();
                                foreach (String param in listParam)
                                {
                                    string itemID = param.Split(';')[3];
                                    if (itemID != "")
                                    {
                                        int ID = Convert.ToInt32(itemID);
                                        if (ID > 0)
                                        {
                                            if (listID.ToString() != "")
                                                listID.Append(", ");
                                            listID.Append(ID);
                                        }
                                    }
                                }
                                if (listID.ToString() != "")
                                {
                                    string filterExpression = String.Format("(PrescriptionOrderDetailID = {0} OR ParentID = {0}) AND PrescriptionOrderDetailID NOT IN ({1}) AND IsCompound = 1 AND IsDeleted = 0", prescriptionDetailID, listID.ToString());
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    string PrescriptionOrderDetailID = "";
                                    List<PrescriptionOrderDt> lstDeletedEntity = BusinessLayer.GetPrescriptionOrderDtList(filterExpression, ctx);
                                    foreach (PrescriptionOrderDt deletedEntity in lstDeletedEntity)
                                    {
                                        deletedEntity.IsDeleted = true;
                                        deletedEntity.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
                                        deletedEntity.GCVoidReason = Constant.DeleteReason.OTHER;
                                        deletedEntity.VoidReason = "Deleted from order detail.";
                                        deletedEntity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityDtDao.Update(deletedEntity);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        PrescriptionOrderDetailID += string.Format("{0},", deletedEntity.PrescriptionOrderDetailID);
                                    }

                                    //dt original
                                    if (!string.IsNullOrEmpty(PrescriptionOrderDetailID))
                                    {
                                        PrescriptionOrderDetailID = PrescriptionOrderDetailID.Remove(PrescriptionOrderDetailID.Length - 1);
                                        if (!string.IsNullOrEmpty(PrescriptionOrderDetailID))
                                        {
                                            string filterExpressionOriginal = string.Format("PrescriptionOrderDetailID IN({0})", PrescriptionOrderDetailID);

                                            List<PrescriptionOrderDtOriginal> lstDeletedEntityOri = BusinessLayer.GetPrescriptionOrderDtOriginalList(filterExpressionOriginal, ctx);
                                            foreach (PrescriptionOrderDtOriginal deletedEntityOri in lstDeletedEntityOri)
                                            {
                                                deletedEntityOri.IsDeleted = true;
                                                deletedEntityOri.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
                                                deletedEntityOri.GCVoidReason = Constant.DeleteReason.OTHER;
                                                deletedEntityOri.VoidReason = "Deleted from order detail.";
                                                deletedEntityOri.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                entityDtOriginalDao.Update(deletedEntityOri);
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                            }

                                        }
                                    }
                                }

                                #endregion
                            }
                            retval = prescriptionID.ToString();

                            #region Save as New Template
                            if (isSaveAsNewTemplate && hdnIsAddNew.Value == "1")
                            {
                                templateHd = new CompoundTemplateHd();

                                templateHd.CompoundTemplateName = txtCompoundMedicationName.Text;
                                templateHd.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
                                templateHd.GCDosingFrequency = cboFrequencyTimelineCompoundCtl.Value.ToString();
                                templateHd.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
                                templateHd.GCDosingUnit = cboDosingUnitCompoundCtl.Value.ToString();
                                templateHd.DosingDuration = Convert.ToInt16(txtDosingDuration.Text);
                                templateHd.DispenseQuantity = Convert.ToDecimal(txtDispenseQty.Text);
                                if (!String.IsNullOrEmpty(cboEmbalaceCompoundCtl.Text))
                                {
                                    templateHd.EmbalaceID = Convert.ToInt16(cboEmbalaceCompoundCtl.Value);
                                    templateHd.EmbalaceQty = Convert.ToDecimal(txtEmbalaceQty.Text);
                                }
                                else
                                {
                                    templateHd.EmbalaceID = null;
                                    templateHd.EmbalaceQty = 0;
                                }

                                templateHd.GCMedicationRoute = cboMedicationRouteCompoundCtl.Value.ToString();
                                if (!String.IsNullOrEmpty(cboCoenamRuleCompoundCtl.Text))
                                    templateHd.GCCoenamRule = !String.IsNullOrEmpty(cboCoenamRuleCompoundCtl.Value.ToString()) ? cboCoenamRuleCompoundCtl.Value.ToString() : null;
                                templateHd.MedicationAdministration = txtMedicationAdministration.Text;
                                templateHd.MedicationPurpose = txtMedicationAdministration.Text;
                                templateHd.IsUseSweetener = chkIsUsingSweetener.Checked;
                                templateHd.IsAsRequired = chkIsPRN.Checked;
                                templateHd.CreatedBy = AppSession.UserLogin.UserID;

                                templateDtList = new List<CompoundTemplateDt>();
                            }
                            #endregion

                            short ctr = 0;
                            int parentID = 0;
                            int noOfItems = listParam.Count();

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
                                    entityDt.PrescriptionOrderID = prescriptionID;
                                    entityDt.IsRFlag = (ctr < 1);
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

                                    string compoundQty = data[6].Replace(',', '.');
                                    decimal qty = 0;
                                    if (compoundQty.Contains('/'))
                                    {
                                        string[] compoundInfo = compoundQty.Split('/');
                                        decimal num1 = Convert.ToDecimal(compoundInfo[0].Replace(',', '.'));
                                        decimal num2 = Convert.ToDecimal(compoundInfo[1].Replace(',', '.'));
                                        qty = Math.Round(num1 / num2, 2);
                                    }
                                    else
                                    {
                                        qty = Convert.ToDecimal(compoundQty);
                                    }

                                    entityDt.GCCompoundUnit = data[7];
                                    entityDt.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
                                    entityDt.TakenQty = Convert.ToDecimal(txtDispenseQty.Text);
                                    if (GCItemUnit != entityDt.GCCompoundUnit)
                                    {
                                        decimal dose = Convert.ToDecimal(data[10]);
                                        entityDt.CompoundQtyInString = data[6].Replace(',', '.');
                                        entityDt.CompoundQty = qty / dose;
                                        entityDt.ResultQty = qty * entityDt.DispenseQty / dose;
                                    }
                                    else
                                    {
                                        entityDt.CompoundQty = qty;
                                        entityDt.CompoundQtyInString = data[6].Replace(',', '.'); ;
                                        entityDt.ResultQty = qty * entityDt.DispenseQty;
                                    }

                                    entityDt.DrugName = data[9];
                                    entityDt.ChargeQty = entityDt.ResultQty;

                                    if (data[15] == Constant.QuantityDeductionType.DIBULATKAN)
                                        entityDt.ChargeQty = Math.Ceiling(entityDt.ChargeQty);

                                    if (data[16] == Constant.QuantityDeductionType.DIBULATKAN)
                                        entityDt.ResultQty = Math.Ceiling(entityDt.ResultQty);

                                    entityDt.CompoundDrugname = txtCompoundMedicationName.Text;
                                    entityDt.GCRoute = cboMedicationRouteCompoundCtl.Value.ToString();

                                    if (hdnSignaIDCompound.Value != "" && hdnSignaIDCompound.Value != "0")
                                    {
                                        entityDt.SignaID = Convert.ToInt32(hdnSignaIDCompound.Value);
                                    }
                                    else
                                    {
                                        entityDt.SignaID = null;
                                    }

                                    entityDt.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
                                    entityDt.GCDosingUnit = cboDosingUnitCompoundCtl.Value.ToString();
                                    //entityDt.GCCoenamRule = cboCoenamRuleCompoundCtl.Value.ToString();

                                    if (!String.IsNullOrEmpty(cboCoenamRuleCompoundCtl.Text))
                                        entityDt.GCCoenamRule = !String.IsNullOrEmpty(cboCoenamRuleCompoundCtl.Value.ToString()) ? cboCoenamRuleCompoundCtl.Value.ToString() : null;

                                    entityDt.IsUseSweetener = chkIsUsingSweetener.Checked;
                                    entityDt.IsAsRequired = chkIsPRN.Checked;

                                    entityDt.GCDosingFrequency = cboFrequencyTimelineCompoundCtl.Value.ToString();
                                    entityDt.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
                                    entityDt.DosingDuration = Convert.ToInt16(txtDosingDuration.Text);

                                    entityDt.StartDate = Helper.GetDatePickerValue(txtStartDate);
                                    entityDt.StartTime = txtStartTime1.Text.Replace('.', ':');
                                    entityDt.Sequence1Time = txtStartTime1.Text.Replace('.', ':');
                                    entityDt.Sequence2Time = txtStartTime2.Text.Replace('.', ':');
                                    entityDt.Sequence3Time = txtStartTime3.Text.Replace('.', ':');
                                    entityDt.Sequence4Time = txtStartTime4.Text.Replace('.', ':');
                                    entityDt.Sequence5Time = txtStartTime5.Text.Replace('.', ':');
                                    entityDt.Sequence6Time = txtStartTime6.Text.Replace('.', ':');

                                    entityDt.IsMorning = chkIsMorning.Checked;
                                    entityDt.IsNoon = chkIsNoon.Checked;
                                    entityDt.IsEvening = chkIsEvening.Checked;
                                    entityDt.IsNight = chkIsNight.Checked;

                                    entityDt.MedicationAdministration = txtMedicationAdministration.Text;
                                    entityDt.MedicationPurpose = txtCompoundMedicationName.Text;

                                    if (cboEmbalaceCompoundCtl.Value != null)
                                    {
                                        entityDt.EmbalaceID = Convert.ToInt32(cboEmbalaceCompoundCtl.Value);
                                        entityDt.EmbalaceQty = Convert.ToDecimal(txtEmbalaceQty.Text);
                                    }
                                    else
                                    {
                                        if (hdnIsEmbalaceQtyTerisiOtomatis.Value == "1")
                                        {
                                            entityDt.EmbalaceQty = 1;
                                        }
                                    }

                                    entityDt.IsCreatedFromOrder = true;
                                    entityDt.GCPrescriptionOrderStatus = Constant.OrderStatus.OPEN;

                                    if (ctr > 0)
                                    {
                                        entityDt.ParentID = parentID;
                                    }
                                    else
                                    {
                                        entityDt.ParentID = null;
                                        entityDt.TakenQty = entityDt.DispenseQty;
                                    }

                                    entityDt.CreatedBy = AppSession.UserLogin.UserID;

                                    if (ctr < 1)
                                    {
                                        parentID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);
                                    }
                                    else
                                    {
                                        entityDtDao.Insert(entityDt);
                                    }

                                    #region Save As New Template
                                    if (templateHd != null)
                                    {
                                        CompoundTemplateDt templateDt = new CompoundTemplateDt();

                                        templateDt.ItemID = (int)entityDt.ItemID;
                                        templateDt.CompoundQty = entityDt.CompoundQty;
                                        templateDt.CompoundQtyInString = entityDt.CompoundQtyInString;
                                        templateDt.GCCompoundUnit = entityDt.GCCompoundUnit;
                                        templateDt.ConversionFactor = entityDt.ConversionFactor;
                                        templateDt.ResultQuantity = entityDt.ResultQty;
                                        templateDt.DisplayOrder = ctr;
                                        templateDtList.Add(templateDt);
                                    }
                                    #endregion

                                    ctr++;
                                }
                            }

                            #region Save as New Template
                            if (templateHd != null)
                            {
                                int rowCount = BusinessLayer.GetvCompoundTemplateHdRowCount(string.Empty, ctx);
                                string templateCode = (rowCount + 1).ToString().PadLeft(5, '0');
                                templateHd.CompoundTemplateCode = templateCode;
                                int templateID = templateHdDao.InsertReturnPrimaryKeyID(templateHd);

                                foreach (CompoundTemplateDt item in templateDtList)
                                {
                                    item.CompoundTemplateID = templateID;
                                    templateDtDao.Insert(item);
                                }

                                PhysicianCompoundTemplate oTemplate = new PhysicianCompoundTemplate() { ParamedicID = (int)AppSession.UserLogin.ParamedicID, CompoundTemplateID = templateID };
                                physicianTemplateDao.Insert(oTemplate);
                            }
                            #endregion

                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Maaf ada perbedaan data antara session. Harap Refresh Halaman Ini.");
                            Exception ex = new Exception(errMessage);
                            ctx.RollBackTransaction();
                            Helper.InsertErrorLog(ex);
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Maaf ada perbedaan data antara session. Harap Refresh Halaman Ini.");
                        Exception ex = new Exception(errMessage);
                        ctx.RollBackTransaction();
                        Helper.InsertErrorLog(ex);
                    }
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
            }

            return result;
        }
    }
}