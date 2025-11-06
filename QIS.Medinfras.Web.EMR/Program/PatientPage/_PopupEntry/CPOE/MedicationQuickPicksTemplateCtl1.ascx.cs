using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class MedicationQuickPicksTemplateCtl1 : BasePagePatientPageEntryCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override void InitializeDataControl(string param)
        {
            IsAdd = true;

            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnTransactionID.Value = string.IsNullOrEmpty(temp[0]) ? "0" : temp[0];
            hdnLocationID.Value = temp[1];
            hdnDefaultGCMedicationRoute.Value = temp[2];
            hdnParamedicID.Value = temp[3];
            hdnRegistrationID.Value = temp[4];
            hdnVisitID.Value = temp[5];
            hdnChargeClassID.Value = temp[6];

            //txtNotes.Text = temp[7];

            hdnDispensaryUnitID.Value = temp[8];
            hdnPrescriptionDate.Value = temp[9];
            hdnPrescriptionTime.Value = temp[10];
            hdnPrescriptionType.Value = temp[11];


            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT, //1
                                                        Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS, //2
                                                        Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN, //3
                                                        Constant.SettingParameter.PH0037, //4
                                                        Constant.SettingParameter.PH_CREATE_QUEUE_LABEL, //5
                                                        Constant.SettingParameter.PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX, //6
                                                        Constant.SettingParameter.EM_ORDER_RESEP_HANYA_BISA_PILIH_ITEM_STOK_RS, //7
                                                        Constant.SettingParameter.EM_IS_VALIDATION_EMPTY_STOCK_PRESCRIPTION_ORDER //8
                                                    );
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            hdnValidationEmptyStockCtlTemplate.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_IS_VALIDATION_EMPTY_STOCK_PRESCRIPTION_ORDER).ParameterValue;

            SettingParameterDt oParam1 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS).FirstOrDefault();
            SettingParameterDt oParam2 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault();
            bool isLimitedCPOEItemForBPJS = oParam1 != null ? (oParam1.ParameterValue == "1" ? true : false) : false;
            int businnessPartnerID = oParam2 != null ? Convert.ToInt32(oParam2.ParameterValue) : 0;

            hdnPrescriptionFeeAmount.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT).ParameterValue;
            hdnIsAutoGenerateReferenceNo.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH0037).ParameterValue;
            hdnIsGenerateQueueLabel.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_CREATE_QUEUE_LABEL).ParameterValue;
            hdnItemQtyWithSpecialQueuePrefix.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX).ParameterValue;
            hdnPrescriptionValidateStockAllRS.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_ORDER_RESEP_HANYA_BISA_PILIH_ITEM_STOK_RS).ParameterValue;

            if (AppSession.RegisteredPatient.BusinessPartnerID == businnessPartnerID)
            {
                rblItemType.SelectedValue = "2";
                rblItemType.Enabled = !isLimitedCPOEItemForBPJS;
            }

            if (AppSession.IsHasAllergy)
            {
                lblAllergy.Style.Add("color", "red");
                lblAllergy.Style.Add("font-weight", "bold");
            }
            else
            {
                lblAllergy.Style.Add("color", "black");
                lblAllergy.Style.Add("font-weight", "normal");
            }
            txtAllergyInfo.Text = AppSession.PatientAllergyInfo;

            BindCboLocation();
        }

        private void BindCboLocation()
        {
            if (Convert.ToInt32(hdnLocationID.Value) > 0)
            {
                Location loc = BusinessLayer.GetLocation(Convert.ToInt32(hdnLocationID.Value));
                List<Location> lstLocation = null;
                if (loc.IsHeader)
                    lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                else
                {
                    lstLocation = new List<Location>();
                    lstLocation.Add(loc);
                }
                Methods.SetComboBoxField<Location>(cboPopupLocation, lstLocation, "LocationName", "LocationID");
                cboPopupLocation.SelectedIndex = 0;
            }
        }

        protected void cboPopupLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
        }

        protected void cbpPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    vItemBalanceQuickPick1 entity = e.Row.DataItem as vItemBalanceQuickPick1;
            //    CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
            //    if (lstSelectedMember.Contains(entity.ItemID.ToString()))
            //        chkIsSelected.Checked = true;
            //    System.Drawing.Color foreColor = System.Drawing.Color.Black;
            //    if (entity.QuantityEND == 0)
            //        foreColor = System.Drawing.Color.Red;
            //    e.Row.Cells[2].ForeColor = foreColor;
            //    e.Row.Cells[3].ForeColor = foreColor;
            //}
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ParamedicID = {0} AND IsDeleted = 0", AppSession.UserLogin.ParamedicID);

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += String.Format(" AND {0} ", hdnFilterExpressionQuickSearch.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPrescriptionTemplateHDRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }

            List<vPrescriptionTemplateHD> lstEntity = BusinessLayer.GetvPrescriptionTemplateHDList(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex, "PrescriptionTemplateID");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        #region Detail Grid
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnSelectedVisitID.Value != "")
            {
                filterExpression = string.Format("PrescriptionTemplateID = {0} AND OrderIsDeleted = 0 AND IsRFlag = 1", hdnSelectedVisitID.Value);

                if (hdnPrescriptionValidateStockAllRS.Value == "1")
                {
                    filterExpression += " AND QtyOnHandAll > 0";
                }

                if (hdnTransactionID.Value != "0" && hdnTransactionID.Value != "")
                {
                    List<vPrescriptionOrderDt> lstItemID = BusinessLayer.GetvPrescriptionOrderDtList(string.Format(
                        "PrescriptionOrderID = {0} AND GCPrescriptionOrderStatus != '{1}' AND IsDeleted = 0", hdnTransactionID.Value, Constant.TestOrderStatus.CANCELLED));
                    string lstSelectedID = "";
                    if (lstItemID.Count > 0)
                    {
                        foreach (vPrescriptionOrderDt itm in lstItemID)
                            lstSelectedID += "," + itm.ItemID;
                        filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
                    }
                }

                switch (rblItemType.SelectedValue)
                {
                    case "1":
                        filterExpression += " AND IsFormularium = 1 ";
                        break;
                    case "2":
                        filterExpression += " AND IsBPJSFormularium = 1 ";
                        break;
                    default:
                        break;
                }

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPrescriptionTemplateDtRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPrescriptionTemplateDt> lstEntity = BusinessLayer.GetvPrescriptionTemplateDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionTemplateDetailID");
            grdPopupViewDt.DataSource = lstEntity;
            grdPopupViewDt.DataBind();
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdPopupViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPrescriptionTemplateDt entity = e.Row.DataItem as vPrescriptionTemplateDt;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                HtmlImage imgIsNotAvailable = e.Row.FindControl("imgIsNotAvailable") as HtmlImage;

                if (entity.GCItemStatus == Constant.ItemStatus.IN_ACTIVE)
                {
                    chkIsSelected.Visible = false;
                    chkIsSelected.CssClass = "chkIsNotSelected";

                    if (imgIsNotAvailable != null)
                    {
                        imgIsNotAvailable.Visible = true;
                    }
                }
                else
                {
                    chkIsSelected.Visible = true;
                    chkIsSelected.CssClass = "chkIsSelected";

                    if (imgIsNotAvailable != null)
                    {
                        imgIsNotAvailable.Visible = false;
                    }
                }
            }
        }
        #endregion

        private bool IsValidated(string lstDosage, string lstFrequency, string lstDispense, ref string result)
        {
            StringBuilder tempMsg = new StringBuilder();

            string message = string.Empty;

            if (string.IsNullOrEmpty(message))
            {
                #region Validate Dosage
                string[] selectedDosage = lstDosage.Split(',');
                foreach (string dosage in selectedDosage)
                {
                    if (string.IsNullOrEmpty(dosage))
                    {
                        tempMsg.AppendLine("There is medication with empty dosing quantity. \n");
                        break;
                    }
                    else
                    {
                        if (!dosage.Contains("/"))
                        {
                            Decimal value;
                            if (!Decimal.TryParse(dosage, out value))
                            {
                                tempMsg.AppendLine(string.Format("There is medication with invalid dosage {0} \n", dosage));
                                break;
                            }
                            else
                            {
                                if (value <= 0)
                                {
                                    tempMsg.AppendLine(string.Format("There is medication with invalid dosage {0} \n", dosage));
                                    break;
                                }
                            }
                        }
                        else
                        {
                            string[] dosageInfo = dosage.Split('/');
                            Decimal value;
                            if (!Decimal.TryParse(dosageInfo[0], out value))
                            {
                                tempMsg.AppendLine(string.Format("There is medication with invalid dosage {0} \n", dosageInfo[0]));
                                break;
                            }
                            if (!Decimal.TryParse(dosageInfo[1], out value))
                            {
                                tempMsg.AppendLine(string.Format("There is medication with invalid dosage {0} \n", dosageInfo[1]));
                                break;
                            }
                        }
                    }
                }
                #endregion

                #region Validate Frequency
                string[] selectedFrequency = lstFrequency.Split(',');
                foreach (string frequency in selectedFrequency)
                {
                    if (string.IsNullOrEmpty(frequency))
                    {
                        tempMsg.AppendLine("There is medication with empty frequency quantity. \n");
                        break;
                    }
                    else
                    {
                        if (!frequency.Contains("/"))
                        {
                            Decimal value;
                            if (!Decimal.TryParse(frequency, out value))
                            {
                                tempMsg.AppendLine(string.Format("There is medication with invalid frequency {0} \n", frequency));
                                break;
                            }
                            else
                            {
                                if (value <= 0)
                                {
                                    tempMsg.AppendLine(string.Format("There is medication with invalid frequency {0} \n", frequency));
                                    break;
                                }
                            }
                        }
                        else
                        {
                            string[] frequencyInfo = frequency.Split('/');
                            Decimal value;
                            if (!Decimal.TryParse(frequencyInfo[0], out value))
                            {
                                tempMsg.AppendLine(string.Format("There is medication with invalid frequency {0} \n", frequencyInfo[0]));
                                break;
                            }
                            if (!Decimal.TryParse(frequencyInfo[1], out value))
                            {
                                tempMsg.AppendLine(string.Format("There is medication with invalid frequency {0} \n", frequencyInfo[1]));
                                break;
                            }
                        }
                    }
                }
                #endregion

                #region Validate Dispense
                string[] selectedDispense = lstDispense.Split(',');
                foreach (string dispense in selectedDispense)
                {
                    if (string.IsNullOrEmpty(dispense))
                    {
                        tempMsg.AppendLine("There is medication with empty dispense quantity. \n");
                        break;
                    }
                    else
                    {
                        if (!dispense.Contains("/"))
                        {
                            Decimal value;
                            if (!Decimal.TryParse(dispense, out value))
                            {
                                tempMsg.AppendLine(string.Format("There is medication with invalid dispense {0} \n", dispense));
                                break;
                            }
                            else
                            {
                                if (value <= 0)
                                {
                                    tempMsg.AppendLine(string.Format("There is medication with invalid dispense {0} \n", dispense));
                                    break;
                                }
                            }
                        }
                        else
                        {
                            string[] dispenseInfo = dispense.Split('/');
                            Decimal value;
                            if (!Decimal.TryParse(dispenseInfo[0], out value))
                            {
                                tempMsg.AppendLine(string.Format("There is medication with invalid dispense {0} \n", dispenseInfo[0]));
                                break;
                            }
                            if (!Decimal.TryParse(dispenseInfo[1], out value))
                            {
                                tempMsg.AppendLine(string.Format("There is medication with invalid dispense {0} \n", dispenseInfo[1]));
                                break;
                            }
                        }
                    }
                }
                #endregion
            }

            message = tempMsg.ToString();

            if (!string.IsNullOrEmpty(message))
            {
                result = message;
            }
            return message == string.Empty;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            if (!IsValidated(hdnSelectedMemberQty.Value, hdnSelectedMemberFrequency.Value, hdnSelectedMemberDispenseQty.Value, ref errMessage))
            {
                result = false;
                retval = "0";
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityOrderDtDao = new PrescriptionOrderDtDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            RegistrationDao registratioDao = new RegistrationDao(ctx);
            ConsultVisitDao visitDao = new ConsultVisitDao(ctx);

            try
            {
                ConsultVisit visit = visitDao.Get(AppSession.RegisteredPatient.VisitID);
                if (visit != null)
                {
                    Registration reg = registratioDao.Get(visit.RegistrationID);
                    if (reg.MRN == AppSession.RegisteredPatient.MRN)
                    {
                        lstSelectedMember = hdnSelectedMember.Value.Split(',');
                        string[] lstSelectedMemberOrderInfo = hdnSelectedMemberOrderInfo.Value.Split(';');
                        string[] lstSelectedMemberStrength = hdnSelectedMemberStrength.Value.Split(',');
                        string[] lstSelectedMemberFrequency = hdnSelectedMemberFrequency.Value.Split(',');
                        //string[] lstSelectedMemberCoenam = hdnSelectedMemberCoenam.Value.Split(',');
                        //string[] lstSelectedMemberPRN = hdnSelectedMemberPRN.Value.Split(',');
                        string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                        string[] lstSelectedMemberDispenseQty = hdnSelectedMemberDispenseQty.Value.Split(',');
                        string[] lstSelectedMemberTakenQty = hdnSelectedMemberTakenQty.Value.Split(',');
                        string[] lstSelectedMemberRemarks = hdnSelectedMemberRemarks.Value.Split('|');
                        //string[] lstSelectedMemberStartTime = hdnSelectedMemberStartTime.Value.Split(',');
                        //string[] lstSelectedMemberRoute = hdnSelectedMemberRoute.Value.Split(',');

                        int prescriptionOrderID = 0;
                        string prescriptionOrderNo = string.Empty;
                        string transactionNo = string.Empty;
                        DateTime prescriptionDate = Helper.DateInStringToDateTime(hdnPrescriptionDate.Value);
                        string prescriptionTime = hdnPrescriptionTime.Value;
                        int prescriptionOrderDetailID = 0;
                        PrescriptionOrderHd entityHd;

                        #region PrescriptionOrderHd
                        if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
                        {

                            entityHd = new PrescriptionOrderHd();
                            entityHd.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                            entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                            entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                            entityHd.PrescriptionDate = prescriptionDate;
                            entityHd.PrescriptionTime = prescriptionTime;
                            entityHd.ClassID = Convert.ToInt32(hdnChargeClassID.Value);
                            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            entityHd.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                            entityHd.SendOrderBy = AppSession.UserLogin.UserID;
                            entityHd.SendOrderDateTime = DateTime.Now;
                            entityHd.LocationID = Convert.ToInt32(cboPopupLocation.Value);
                            entityHd.GCPrescriptionType = hdnPrescriptionType.Value;
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
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            if (hdnDispensaryUnitID.Value != "" && hdnDispensaryUnitID.Value != null)
                                entityHd.DispensaryServiceUnitID = Convert.ToInt32(hdnDispensaryUnitID.Value);
                            else
                                entityHd.DispensaryServiceUnitID = 0;

                            entityHd.LocationID = Convert.ToInt32(hdnLocationID.Value);
                            entityHd.CreatedBy = AppSession.UserLogin.UserID;
                            //entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityHd.IsCreatedBySystem = false;
                            entityHd.IsOrderedByPhysician = true;
                        }
                        else
                        {
                            prescriptionOrderID = Convert.ToInt32(hdnTransactionID.Value);
                            entityHd = entityOrderHdDao.Get(prescriptionOrderID);
                        }

                        #endregion

                        #region PrescriptionOrderDt
                        List<PrescriptionOrderDt> lstPrescriptionOrderDt = new List<PrescriptionOrderDt>();

                        List<PrescriptionTemplateDt> lstDrugInfo = BusinessLayer.GetPrescriptionTemplateDtList(string.Format("PrescriptionTemplateDetailID IN ({0}) OR ParentID IN ({0})", hdnSelectedMember.Value), ctx);

                        int ct = -1;
                        decimal compoundTakenQty = 0;
                        decimal dispenseQty = 0;
                        decimal takenQty = 0;
                        short frequency = 0;
                        decimal numberOfDosage = 0;
                        string medicationAdministration = string.Empty;

                        foreach (PrescriptionTemplateDt item in lstDrugInfo)
                        {
                            PrescriptionOrderDt entity = new PrescriptionOrderDt();

                            if (item.IsRFlag)
                                ct += 1;

                            entity.IsRFlag = item.IsRFlag;
                            entity.IsCompound = item.IsCompound;
                            entity.ItemID = item.ItemID;
                            entity.ParentID = item.ParentID;
                            entity.GenericName = item.GenericName;
                            entity.DrugName = item.DrugName;
                            entity.CompoundDrugname = item.CompoundDrugname;
                            entity.GCDrugForm = item.GCDrugForm;
                            entity.Dose = item.Dose;
                            entity.GCDoseUnit = item.GCDoseUnit;
                            entity.GCDosingFrequency = item.GCDosingFrequency;
                            entity.GCDosingUnit = item.GCDosingUnit;


                            #region PrescriptionOrderDt
                            if (lstSelectedMemberDispenseQty.Length > ct)
                                dispenseQty = Convert.ToDecimal(lstSelectedMemberDispenseQty[ct]);
                            if (lstSelectedMemberFrequency.Length > ct)
                                frequency = Convert.ToInt16(lstSelectedMemberFrequency[ct]);
                            if (lstSelectedMemberQty.Length > ct)
                                numberOfDosage = Convert.ToDecimal(lstSelectedMemberQty[ct]);

                            takenQty = dispenseQty;

                            bool isDay = true;
                            entity.Frequency = frequency;
                            entity.NumberOfDosage = numberOfDosage;
                            entity.NumberOfDosageInString = entity.NumberOfDosage.ToString();
                            entity.StartDate = Helper.GetDatePickerValue(txtDefaultStartDate.Text);
                            string[] medicationTime = Methods.GetMedicationSequenceTime(entity.Frequency).Split('|');
                            entity.Sequence1Time = medicationTime[0];
                            entity.Sequence2Time = medicationTime[1];
                            entity.Sequence3Time = medicationTime[2];
                            entity.Sequence4Time = medicationTime[3];
                            entity.Sequence5Time = medicationTime[4];
                            entity.Sequence6Time = medicationTime[5];
                            if (medicationTime[0] != "-")
                                entity.StartTime = medicationTime[0];
                            else
                                entity.StartTime = "00:00";
                            entity.DispenseQty = dispenseQty;
                            if (!entity.IsUsingUDD)
                            {
                                if (isDay)
                                {
                                    entity.DosingDuration = Math.Ceiling((decimal)(entity.DispenseQty / (entity.Frequency * entity.NumberOfDosage)));
                                }
                                else
                                {
                                    decimal numberOfTaken = Math.Ceiling(Convert.ToDecimal(24 / entity.Frequency));
                                    entity.DosingDuration = Math.Ceiling((decimal)(entity.DispenseQty / (numberOfTaken * entity.NumberOfDosage)));
                                }
                                if (!entity.IsCompound)
                                {
                                    entity.TakenQty = takenQty;
                                    entity.ResultQty = entity.ChargeQty = entity.TakenQty;
                                }
                                else
                                {
                                    if (entity.IsRFlag)
                                        compoundTakenQty = entity.TakenQty;

                                    entity.CompoundQty = item.CompoundQty;
                                    entity.CompoundQtyInString = item.CompoundQtyInString;
                                    entity.GCCompoundUnit = item.GCCompoundUnit;
                                    entity.IsUseSweetener = item.IsUseSweetener;

                                    entity.TakenQty = takenQty;

                                    vDrugInfo1 oDrugInfo = BusinessLayer.GetvDrugInfo1List(string.Format("ItemID = {0}", entity.ItemID), ctx).FirstOrDefault();
                                    if (oDrugInfo != null)
                                    {
                                        string compoundQty = entity.CompoundQtyInString;
                                        decimal qty = 0;
                                        decimal num1 = 0;
                                        decimal num2 = 0;
                                        if (compoundQty.Contains('/'))
                                        {
                                            string[] compoundInfo = compoundQty.Split('/');
                                            num1 = Convert.ToDecimal(compoundInfo[0]);
                                            num2 = Convert.ToDecimal(compoundInfo[1]);
                                        }
                                        else
                                        {
                                            num1 = Convert.ToDecimal(compoundQty);
                                            num2 = 1;
                                        }

                                        if (oDrugInfo.GCItemUnit != entity.GCCompoundUnit)
                                        {
                                            decimal dose = Convert.ToDecimal(entity.Dose);
                                            entity.ResultQty = (num1 / num2 * entity.TakenQty) / dose;
                                        }
                                        else
                                        {
                                            entity.ResultQty = (num1 * entity.TakenQty) / num2;
                                        }

                                        entity.ChargeQty = entity.ResultQty;

                                        if (oDrugInfo.GCStockDeductionType == Constant.QuantityDeductionType.DIBULATKAN) entity.ChargeQty = Math.Ceiling(entity.ChargeQty);
                                        if (oDrugInfo.GCConsumptionDeductionType == Constant.QuantityDeductionType.DIBULATKAN) entity.ResultQty = Math.Ceiling(entity.ResultQty);
                                    }
                                }
                            }
                            else
                            {
                                entity.DispenseQty = 0;
                                entity.TakenQty = 0;
                            }

                            entity.SignaID = item.SignaID;
                            entity.GCRoute = item.GCRoute;
                            entity.GCCoenamRule = item.GCCoenamRule;
                            entity.MedicationPurpose = "";
                            entity.MedicationAdministration = lstSelectedMemberRemarks[ct];
                            entity.EmbalaceID = null;
                            entity.EmbalaceQty = 0;
                            entity.GCPrescriptionOrderStatus = Constant.OrderStatus.OPEN;

                            entity.IsCreatedFromOrder = true;
                            entity.PrescriptionOrderID = prescriptionOrderID;
                            entity.CreatedBy = AppSession.UserLogin.UserID;

                            lstPrescriptionOrderDt.Add(entity);
                            #endregion
                        }
                        #endregion

                        #region Apply to Database
                        #region PrescriptionOrderHd
                        if (prescriptionOrderID == 0)
                        {
                            entityHd.CreatedBy = AppSession.UserLogin.UserID;
                            entityHd.CreatedDate = DateTime.Now;
                            entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            prescriptionOrderID = entityOrderHdDao.InsertReturnPrimaryKeyID(entityHd);
                        }
                        else
                        {
                            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityHd.LastUpdatedDate = DateTime.Now;
                            entityOrderHdDao.Update(entityHd);
                        }

                        prescriptionOrderNo = entityHd.PrescriptionOrderNo;
                        #endregion

                        #region PrescriptionOrderDt
                        List<int> lstOrderDetailID = new List<int>();
                        int parentID = 0;
                        int oldParentID = 0;
                        foreach (PrescriptionOrderDt orderDt in lstPrescriptionOrderDt)
                        {
                            orderDt.PrescriptionOrderID = prescriptionOrderID;

                            if (orderDt.IsCompound)
                            {
                                if (orderDt.IsRFlag)
                                {
                                    oldParentID = orderDt.PrescriptionOrderDetailID;
                                    orderDt.ParentID = null;
                                }
                                else
                                {
                                    orderDt.ParentID = parentID;
                                }
                            }
                            prescriptionOrderDetailID = entityOrderDtDao.InsertReturnPrimaryKeyID(orderDt);
                            if (orderDt.IsRFlag && orderDt.IsCompound)
                                parentID = prescriptionOrderDetailID;
                        }
                        #endregion

                        #endregion
                        if (result == true)
                        {
                            retval = prescriptionOrderID.ToString();
                            ctx.CommitTransaction();
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
                errMessage = string.Format("<strong>{0} ({1})</strong><br/><br/><i>{2}</i>", ex.Message, ex.Source, ex.StackTrace);
                ctx.RollBackTransaction();
                Helper.InsertErrorLog(ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}