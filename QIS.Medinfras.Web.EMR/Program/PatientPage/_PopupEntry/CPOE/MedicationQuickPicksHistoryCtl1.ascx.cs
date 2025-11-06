﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;
using System.Globalization;
using System.Text;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class MedicationQuickPicksHistoryCtl1 : BasePagePatientPageEntryCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override void InitializeDataControl(string param)
        {
            IsAdd = true;
            hdnDateToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
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
            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                                        AppSession.UserLogin.HealthcareID,
                                                        Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS,
                                                        Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN,
                                                        Constant.SettingParameter.EM_ORDER_RESEP_HANYA_BISA_PILIH_ITEM_STOK_RS,
                                                        Constant.SettingParameter.EM_IS_VALIDATION_EMPTY_STOCK_PRESCRIPTION_ORDER,
                                                        Constant.SettingParameter.EM_IS_QUICK_PICKS_HISTORY_READ_ORDER_DATE,
                                                        Constant.SettingParameter.EM_DEFAULT_DISPLAY_FILTER_ALL,
                                                        Constant.SettingParameter.IS_VISIBLE_FILTER_TAKEN_ITEM_IN_QUICK_PICKS_HISTORY
                                                    );
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);

            hdnPrescriptionValidateStockAllRS.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_ORDER_RESEP_HANYA_BISA_PILIH_ITEM_STOK_RS).FirstOrDefault().ParameterValue;
            hdnValidationEmptyStockCtlHistory.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_IS_VALIDATION_EMPTY_STOCK_PRESCRIPTION_ORDER).FirstOrDefault().ParameterValue;
            hdnIsQuickPicksReadOrderDate.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_IS_QUICK_PICKS_HISTORY_READ_ORDER_DATE).FirstOrDefault().ParameterValue;
            hdnDefaultDisplayFilterAll.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_DEFAULT_DISPLAY_FILTER_ALL).ParameterValue;
            hdnIsVisibleFilterTakenItemInQuickPicksHistory.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_VISIBLE_FILTER_TAKEN_ITEM_IN_QUICK_PICKS_HISTORY).ParameterValue;

            SettingParameterDt oParam1 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS).FirstOrDefault();
            SettingParameterDt oParam2 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault();
            bool isLimitedCPOEItemForBPJS = oParam1 != null ? (oParam1.ParameterValue == "1" ? true : false) : false;
            int businnessPartnerID = oParam2 != null ? Convert.ToInt32(oParam2.ParameterValue) : 0;

            txtFilterDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            if (hdnDefaultDisplayFilterAll.Value == "1")
            {
                if (AppSession.RegisteredPatient.GCCustomerType == AppSession.TipeCustomerBPJS)
                {
                    rblItemType.SelectedValue = "2";
                    rblItemType.Enabled = !AppSession.IsLimitedCPOEItemForBPJS;
                }
            }
            else
            {
                rblItemType.SelectedValue = "0";
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

            if (hdnIsQuickPicksReadOrderDate.Value == "1")
            {
                trFilterDate.Style.Remove("display");
            }
            else
            {
                trFilterDate.Style.Add("display", "none");
            }
            txtAllergyInfo.Text = AppSession.PatientAllergyInfo;

            if (hdnIsVisibleFilterTakenItemInQuickPicksHistory.Value == "0")
            {
                trItemFilter.Style.Add("display", "none");
            }

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


        private string GetFilterExpression()
        {
            string filterExpression = string.Format("MRN = {0} AND VisitID IN (SELECT VisitID FROM PrescriptionOrderHd WHERE GCTransactionStatus NOT IN ('{1}','{2}'))", AppSession.RegisteredPatient.MRN, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.VOID);

            string startDate = DateTime.Now.Date.AddDays(-90).ToString(Constant.FormatString.DATE_FORMAT_112);
            string endDate = DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112);

            if (rblVisitPeriod.SelectedValue == "1")
            {
                filterExpression += string.Format(" AND VisitDate BETWEEN '{0}' AND '{1}'", startDate, endDate);
            }
            else if (rblVisitPeriod.SelectedValue == "2")
            {
                startDate = DateTime.Now.Date.AddDays(-180).ToString(Constant.FormatString.DATE_FORMAT_112);
                filterExpression += string.Format(" AND VisitDate BETWEEN '{0}' AND '{1}'", startDate, endDate);
            }

            if (rblVisitType.SelectedValue == "1")
            {
                filterExpression += string.Format(" AND ParamedicID = '{0}'", AppSession.UserLogin.ParamedicID);
            }

            //if (rblItemSource.SelectedValue == "2")
            //{
            //    // From History
            //    filterExpression += string.Format(" AND ItemID IN (SELECT ItemID FROM vPrescriptionOrderDt5 WHERE MRN = {0} AND IsRFlag = 1 AND IsCompound = 0)",AppSession.RegisteredPatient.MRN);
            //}

            //switch (rblItemType.SelectedValue)
            //{
            //    case "1":
            //        filterExpression += " AND IsFormularium = 1 ";
            //        break;
            //    case "2":
            //        filterExpression += " AND IsBPJSFormularium = 1 ";
            //        break;
            //    case "3":
            //        filterExpression += " AND IsEmployeeFormularium = 1 ";
            //        break;
            //    default:
            //        break;
            //}
            return filterExpression;
        }

        protected void grdPopupViewHd_RowDataBound(object sender, GridViewRowEventArgs e)
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
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisit4RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }

            List<vConsultVisit4> lstEntity = BusinessLayer.GetvConsultVisit4List(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex, "VisitID DESC");
            grdPopupViewHd.DataSource = lstEntity;
            grdPopupViewHd.DataBind();
        }

        #region Detail Grid
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnSelectedVisitID.Value != "")
            {
                //filterExpression = string.Format("VisitID = {0} AND IsRFlag = 1 AND OrderIsDeleted = 0 AND GCTransactionStatus != 'X121^999' AND GCPrescriptionOrderStatus != '{1}'", hdnSelectedVisitID.Value, Constant.OrderStatus.CANCELLED);
                filterExpression = string.Format("VisitID = {0} AND IsRFlag = 1 AND OrderIsDeleted = 0 AND GCTransactionStatus != 'X121^999'", hdnSelectedVisitID.Value);

                if (hdnPrescriptionValidateStockAllRS.Value == "1")
                {
                    filterExpression += " AND QtyOnHandAll > 0";
                }

                if (hdnIsQuickPicksReadOrderDate.Value == "1")
                {
                    if (!chkIsAllDate.Checked)
                    {
                        filterExpression += string.Format(" AND PrescriptionDate = '{0}'", Helper.GetDatePickerValue(txtFilterDate).ToString(Constant.FormatString.DATE_FORMAT_112));
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

                if (hdnIsVisibleFilterTakenItemInQuickPicksHistory.Value == "1")
                {
                    switch (rblItemFilter.SelectedValue)
                    {
                        case "1":
                            filterExpression += " AND (ChargeQty > 0)";
                            break;
                        default:
                            break;
                    }
                }

                if (hdnTransactionID.Value != "0" && hdnTransactionID.Value != "")
                {
                    List<PrescriptionOrderDt> lstItemID = BusinessLayer.GetPrescriptionOrderDtList(string.Format(
                        "PrescriptionOrderID = {0} AND GCPrescriptionOrderStatus != '{1}' AND IsDeleted = 0", hdnTransactionID.Value, Constant.TestOrderStatus.CANCELLED));
                    string lstSelectedID = "";
                    if (lstItemID.Count > 0)
                    {
                        foreach (PrescriptionOrderDt itm in lstItemID)
                            lstSelectedID += "," + itm.ItemID;
                        filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
                    }
                }

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPatientPrescriptionHistoryRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPatientPrescriptionHistory> lstEntity = BusinessLayer.GetvPatientPrescriptionHistoryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderDetailID");
            grdPopupViewDt.DataSource = lstEntity;
            grdPopupViewDt.DataBind();
        }

        protected void grdPopupViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientPrescriptionHistory entity = e.Row.DataItem as vPatientPrescriptionHistory;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                HtmlImage imgIsNotAvailable = e.Row.FindControl("imgIsNotAvailable") as HtmlImage;
                System.Drawing.Color foreColor = System.Drawing.Color.Black;

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

                if (hdnValidationEmptyStockCtlHistory.Value == "0")
                {
                    if (entity.QtyOnHandAll == 0)
                    {
                        foreColor = System.Drawing.Color.Red;
                        chkIsSelected.Visible = false;
                    }
                }
            }
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

            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberStrength = hdnSelectedMemberStrength.Value.Split(',');
                string[] lstSelectedMemberFrequency = hdnSelectedMemberFrequency.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                string[] lstSelectedMemberDispenseQty = hdnSelectedMemberDispenseQty.Value.Split(',');
                string[] lstSelectedMemberRemarks = hdnSelectedMemberRemarks.Value.Split('|');

                int prescriptionOrderID = 0;
                int transactionID = 0;
                string transactionNo = string.Empty;

                if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
                {
                    PrescriptionOrderHd entityHd = new PrescriptionOrderHd();

                    DateTime prescriptionDate = Helper.DateInStringToDateTime(hdnPrescriptionDate.Value);
                    string prescriptionTime = hdnPrescriptionTime.Value;

                    entityHd.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                    entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    entityHd.PrescriptionDate = prescriptionDate;
                    entityHd.PrescriptionTime = prescriptionTime;
                    entityHd.ClassID = Convert.ToInt32(hdnChargeClassID.Value);
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.LocationID = Convert.ToInt32(cboPopupLocation.Value);
                    entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
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
                    entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    if (hdnDispensaryUnitID.Value != "" && hdnDispensaryUnitID.Value != null)
                        entityHd.DispensaryServiceUnitID = Convert.ToInt32(hdnDispensaryUnitID.Value);
                    else
                        entityHd.DispensaryServiceUnitID = 0;

                    //entityOrderHd.LocationID = Convert.ToInt32(cboLocation.Value);
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHd.LastUpdatedDate = DateTime.Now;
                    entityHd.IsCreatedBySystem = false;
                    entityHd.IsOrderedByPhysician = true;
                    prescriptionOrderID = entityOrderHdDao.InsertReturnPrimaryKeyID(entityHd);
                    transactionID = prescriptionOrderID;
                    transactionNo = entityHd.PrescriptionOrderNo;
                }
                else
                {
                    prescriptionOrderID = Convert.ToInt32(hdnTransactionID.Value);
                }


                PrescriptionOrderHd orderHd = entityOrderHdDao.Get(prescriptionOrderID);
                if (orderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<vPrescriptionOrderDt1> lstDrugInfo = BusinessLayer.GetvPrescriptionOrderDt1List(string.Format("PrescriptionOrderDetailID IN ({0})", hdnSelectedMember.Value), ctx);
                    int ct = 0;
                    foreach (String detailID in lstSelectedMember)
                    {
                        vPrescriptionOrderDt1 drugInfo = lstDrugInfo.FirstOrDefault(p => p.PrescriptionOrderDetailID == Convert.ToInt32(detailID));

                        PrescriptionOrderDt entity = new PrescriptionOrderDt();
                        #region PrescriptionOrderDt
                        entity.IsRFlag = true;
                        entity.IsCompound = drugInfo.IsCompound;
                        entity.ItemID = Convert.ToInt32(drugInfo.ItemID);
                        entity.DrugName = drugInfo.DrugName;
                        entity.CompoundDrugname = drugInfo.CompoundDrugname;
                        entity.GenericName = drugInfo.GenericName;
                        if (drugInfo.GCDrugForm != null)
                        {
                            entity.GCDrugForm = drugInfo.GCDrugForm;
                        }
                        //entity.SignaID = drugInfo.SignaID;
                        entity.Dose = drugInfo.Dose;
                        if (!String.IsNullOrEmpty(drugInfo.GCDoseUnit))
                        {
                            entity.GCDoseUnit = drugInfo.GCDoseUnit;
                        }

                        bool isDay = true;
                        #region Frequency
                        entity.Frequency = Convert.ToInt16(lstSelectedMemberFrequency[ct]);
                        entity.GCDosingFrequency = drugInfo.GCDosingFrequency;
                        switch (entity.Frequency)
                        {
                            case 1:
                                entity.IsMorning = true;
                                break;
                            case 2:
                                entity.IsMorning = true;
                                entity.IsNoon = true;
                                break;
                            case 3:
                                entity.IsMorning = true;
                                entity.IsNoon = true;
                                entity.IsNight = true;
                                break;
                            case 4:
                                entity.IsMorning = true;
                                entity.IsNoon = true;
                                entity.IsEvening = true;
                                entity.IsNight = true;
                                break;
                            default:
                                entity.IsMorning = true;
                                entity.IsNoon = true;
                                entity.IsEvening = true;
                                entity.IsNight = true;
                                break;
                        }
                        #endregion

                        entity.NumberOfDosage = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                        entity.GCDosingUnit = drugInfo.GCDosingUnit;

                        if (drugInfo.Frequency == entity.Frequency && drugInfo.NumberOfDosage == entity.NumberOfDosage && drugInfo.GCDosingFrequency == entity.GCDosingFrequency)
                        {
                            entity.SignaID = drugInfo.SignaID;
                        }
                        else
                        {
                            entity.SignaID = null;
                        }

                        entity.GCCoenamRule = drugInfo.GCCoenamRule;
                        entity.IsAsRequired = drugInfo.IsAsRequired;
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
                        entity.DispenseQty = Convert.ToDecimal(lstSelectedMemberDispenseQty[ct]);
                        entity.IsUsingUDD = false;

                        decimal takenQty = entity.DispenseQty;
                        decimal compoundTakenQty = 0;

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

                                entity.CompoundQty = drugInfo.CompoundQty;
                                entity.CompoundQtyInString = drugInfo.CompoundQtyInString;
                                entity.GCCompoundUnit = drugInfo.GCCompoundUnit;
                                entity.IsUseSweetener = drugInfo.IsUseSweetener;

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

                        entity.GCRoute = drugInfo.GCRoute;
                        entity.MedicationPurpose = "";
                        entity.MedicationAdministration = lstSelectedMemberRemarks[ct];
                        entity.EmbalaceID = null;
                        entity.EmbalaceQty = 0;
                        entity.GCPrescriptionOrderStatus = Constant.OrderStatus.OPEN;

                        #endregion

                        entity.IsCreatedFromOrder = true;
                        entity.PrescriptionOrderID = prescriptionOrderID;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        int id = entityOrderDtDao.InsertReturnPrimaryKeyID(entity);

                        #region Racikan
                        if (drugInfo.IsRFlag && drugInfo.IsCompound)
                        {
                            int parentID = id;
                            List<vPrescriptionOrderDt1> lstCompoundDetailInfo = BusinessLayer.GetvPrescriptionOrderDt1List(string.Format("ParentID = {0} AND OrderIsDeleted = 0", drugInfo.PrescriptionOrderDetailID), ctx).OrderBy(lst => lst.PrescriptionOrderDetailID).ToList();

                            foreach (vPrescriptionOrderDt1 compoundDetailInfo in lstCompoundDetailInfo)
                            {
                                PrescriptionOrderDt compoundEntity = new PrescriptionOrderDt();
                                #region PrescriptionOrderDt
                                compoundEntity.IsRFlag = false;
                                compoundEntity.IsCompound = true;
                                compoundEntity.ItemID = Convert.ToInt32(compoundDetailInfo.ItemID);
                                compoundEntity.ParentID = parentID;
                                compoundEntity.DrugName = compoundDetailInfo.DrugName;
                                compoundEntity.GenericName = compoundDetailInfo.GenericName;
                                if (compoundDetailInfo.GCDrugForm != null)
                                {
                                    compoundEntity.GCDrugForm = compoundDetailInfo.GCDrugForm;
                                }
                                compoundEntity.SignaID = compoundDetailInfo.SignaID;
                                compoundEntity.Dose = compoundDetailInfo.Dose;
                                if (!String.IsNullOrEmpty(compoundDetailInfo.GCDoseUnit))
                                {
                                    compoundEntity.GCDoseUnit = compoundDetailInfo.GCDoseUnit;
                                }

                                #region Frequency
                                compoundEntity.Frequency = entity.Frequency;
                                compoundEntity.GCDosingFrequency = compoundDetailInfo.GCDosingFrequency;
                                compoundEntity.IsMorning = entity.IsMorning;
                                compoundEntity.IsNoon = entity.IsNoon;
                                compoundEntity.IsEvening = entity.IsEvening;
                                compoundEntity.IsNight = entity.IsNight;
                                #endregion

                                compoundEntity.NumberOfDosage = entity.NumberOfDosage;
                                compoundEntity.GCDosingUnit = entity.GCDosingUnit;

                                compoundEntity.GCCoenamRule = entity.GCCoenamRule;
                                compoundEntity.IsAsRequired = entity.IsAsRequired;
                                compoundEntity.StartDate = entity.StartDate;
                                compoundEntity.Sequence1Time = medicationTime[0];
                                compoundEntity.Sequence2Time = medicationTime[1];
                                compoundEntity.Sequence3Time = medicationTime[2];
                                compoundEntity.Sequence4Time = medicationTime[3];
                                compoundEntity.Sequence5Time = medicationTime[4];
                                compoundEntity.Sequence6Time = medicationTime[5];
                                if (medicationTime[0] != "-")
                                    compoundEntity.StartTime = medicationTime[0];
                                else
                                    compoundEntity.StartTime = "00:00";
                                compoundEntity.DispenseQty = entity.DispenseQty;
                                compoundEntity.IsUsingUDD = false;

                                compoundEntity.CompoundQty = compoundDetailInfo.CompoundQty;
                                compoundEntity.CompoundQtyInString = compoundDetailInfo.CompoundQtyInString;
                                compoundEntity.GCCompoundUnit = compoundDetailInfo.GCCompoundUnit;
                                compoundEntity.IsUseSweetener = compoundDetailInfo.IsUseSweetener;

                                compoundEntity.TakenQty = entity.TakenQty;

                                vDrugInfo1 oDrugInfo = BusinessLayer.GetvDrugInfo1List(string.Format("ItemID = {0}", compoundDetailInfo.ItemID), ctx).FirstOrDefault();
                                if (oDrugInfo != null)
                                {
                                    string compoundQty = compoundDetailInfo.CompoundQtyInString;
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

                                    if (oDrugInfo.GCItemUnit != compoundDetailInfo.GCCompoundUnit)
                                    {
                                        decimal dose = Convert.ToDecimal(compoundDetailInfo.Dose);
                                        compoundEntity.ResultQty = (num1 / num2 * compoundEntity.TakenQty) / dose;
                                    }
                                    else
                                    {
                                        compoundEntity.ResultQty = (num1 * compoundEntity.TakenQty) / num2;
                                    }

                                    compoundEntity.ChargeQty = compoundEntity.ResultQty;

                                    if (oDrugInfo.GCStockDeductionType == Constant.QuantityDeductionType.DIBULATKAN) compoundEntity.ChargeQty = Math.Ceiling(compoundEntity.ChargeQty);
                                    if (oDrugInfo.GCConsumptionDeductionType == Constant.QuantityDeductionType.DIBULATKAN) compoundEntity.ResultQty = Math.Ceiling(compoundEntity.ResultQty);
                                }

                                compoundEntity.GCRoute = entity.GCRoute;
                                compoundEntity.MedicationPurpose = "";
                                compoundEntity.MedicationAdministration = entity.MedicationAdministration;
                                compoundEntity.EmbalaceID = null;
                                compoundEntity.EmbalaceQty = 0;
                                compoundEntity.GCPrescriptionOrderStatus = Constant.OrderStatus.OPEN;

                                #endregion

                                compoundEntity.IsCreatedFromOrder = true;
                                compoundEntity.PrescriptionOrderID = prescriptionOrderID;
                                compoundEntity.CreatedBy = AppSession.UserLogin.UserID;
                                entityOrderDtDao.InsertReturnPrimaryKeyID(compoundEntity);
                            }
                        }
                        #endregion

                        ct++;
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi Sudah Diproses. Tidak Bisa Ditambahkan Item";
                    ctx.RollBackTransaction();
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                }
                if (result == true)
                {
                    retval = prescriptionOrderID.ToString();
                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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