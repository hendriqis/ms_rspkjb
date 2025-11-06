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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PrescriptionEntryQuickPicksCtl1 : BasePagePatientPageEntryCtl
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

            hdnDefaultStartDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
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
            string filterExpression = "";

            if (hdnItemGroupDrugLogisticID.Value == "")
                filterExpression += string.Format("LocationID = '{0}' AND GCItemType IN ('{2}','{3}') AND IsDeleted = 0 AND (ItemName1 LIKE '%{1}%' OR GenericName LIKE '%{1}%') AND IsDeleted = 0", cboPopupLocation.Value, hdnFilterItem.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);
            else
                filterExpression += string.Format("LocationID = '{0}' AND GCItemType IN ('{3}','{4}') AND IsDeleted = 0 AND (ItemName1 LIKE '%{1}%' OR GenericName LIKE '%{1}%') AND ItemGroupID IN(SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{2}/%') AND IsDeleted = 0", cboPopupLocation.Value, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);

            if (rblItemSource.SelectedValue == "2")
            {
                // From History
                filterExpression += string.Format(" AND ItemID IN (SELECT ItemID FROM vPrescriptionOrderDt5 WHERE MRN = {0} AND IsRFlag = 1 AND IsCompound = 0)",AppSession.RegisteredPatient.MRN);
            }

            switch (rblItemType.SelectedValue)
            {
                case "1":
                    filterExpression += " AND IsFormularium = 1 ";
                    break;
                case "2":
                    filterExpression += " AND IsBPJSFormularium = 1 ";
                    break;
                case "3":
                    filterExpression += " AND IsEmployeeFormularium = 1 ";
                    break;
                default:
                    break;
            }
            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemBalanceQuickPick1 entity = e.Row.DataItem as vItemBalanceQuickPick1;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                    chkIsSelected.Checked = true;
                System.Drawing.Color foreColor = System.Drawing.Color.Black;
                if (entity.QuantityEND == 0)
                    foreColor = System.Drawing.Color.Red;
                e.Row.Cells[2].ForeColor = foreColor;
                e.Row.Cells[3].ForeColor = foreColor;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceQuickPick1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vItemBalanceQuickPick1> lstEntity = BusinessLayer.GetvItemBalanceQuickPick1List(filterExpression, 10, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private bool IsValidated(string lstDuration, string lstStartDate, ref string result)
        {
            StringBuilder tempMsg = new StringBuilder();

            string message = string.Empty;

            if (string.IsNullOrEmpty(message))
            {
                #region Validate Duration
                string[] selectedDuration = lstDuration.Split(',');
                foreach (string duration in selectedDuration)
                {
                    if (string.IsNullOrEmpty(duration))
                    {
                        tempMsg.AppendLine("There is medication with empty duration. \n");
                        break;
                    }
                    else
                    {
                        Decimal value;
                        if (!Decimal.TryParse(duration, out value))
                        {
                            tempMsg.AppendLine(string.Format("There is medication with invalid duration {0} \n", duration));
                            break;
                        }
                        else
                        {
                            if (value == 0)
                            {
                                tempMsg.AppendLine(string.Format("There is medication with invalid duration {0} \n", duration));
                                break;
                            }
                        }
                    }
                }
                #endregion

                #region Validate Start Date
                string[] selectedDate = lstStartDate.Split(',');
                foreach (string date in selectedDate)
                {
                    if (string.IsNullOrEmpty(date))
                    {
                        tempMsg.AppendLine("There is medication with empty start date.\n");
                        break;
                    }
                    else
                    {
                        DateTime startDate;
                        string format = Constant.FormatString.DATE_PICKER_FORMAT;
                        try
                        {
                            startDate = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
                        }
                        catch (FormatException)
                        {
                            tempMsg.AppendLine(string.Format("There is medication with invalid start date {0}\n", date));
                            break;
                        }
                        DateTime sdate = Helper.GetDatePickerValue(date);
                        if (DateTime.Compare(sdate, DateTime.Now.Date) < 0)
                        {
                            tempMsg.AppendLine("There is medication with start date less than current date\n");
                            break;
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

            if (!IsValidated(hdnSelectedMemberDuration.Value, hdnSelectedMemberStartDate.Value, ref errMessage))
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
                string[] lstSelectedMemberSigna = hdnSelectedMemberSigna.Value.Split(',');
                string[] lstSelectedMemberCoenam = hdnSelectedMemberCoenam.Value.Split(',');
                string[] lstSelectedMemberPRN = hdnSelectedMemberPRN.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                string[] lstSelectedMemberDuration = hdnSelectedMemberDuration.Value.Split(',');
                string[] lstSelectedMemberStartDate = hdnSelectedMemberStartDate.Value.Split(',');
                string[] lstSelectedMemberStartTime = hdnSelectedMemberStartTime.Value.Split(',');
                string[] lstSelectedMemberRoute = hdnSelectedMemberRoute.Value.Split(',');

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
                    //entityHd.LastUpdatedBy = AppSession.UserLogin.UserID; // ditutup oleh RN per 20201214 (patch 202012-03), karna saat awal insert harusnya gak isi LastUpdated nya
                    //entityHd.LastUpdatedDate = DateTime.Now; // ditutup oleh RN per 20201214 (patch 202012-03), karna saat awal insert harusnya gak isi LastUpdated nya
                    entityHd.IsCreatedBySystem = false;
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
                    List<vDrugInfo> lstDrugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);
                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        vDrugInfo drugInfo = lstDrugInfo.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));

                        PrescriptionOrderDt entity = new PrescriptionOrderDt();
                        #region PrescriptionOrderDt
                        entity.IsRFlag = true;
                        entity.ItemID = Convert.ToInt32(itemID);
                        entity.DrugName = drugInfo.ItemName1;
                        entity.GenericName = drugInfo.GenericName;
                        if (drugInfo.GCDrugForm != null)
                        {
                            entity.GCDrugForm = drugInfo.GCDrugForm;
                        }
                        entity.SignaID = null;
                        entity.Dose = drugInfo.Dose;
                        if (!String.IsNullOrEmpty(drugInfo.GCDoseUnit))
                        {
                            entity.GCDoseUnit = drugInfo.GCDoseUnit;
                        }

                        bool isDay = true;
                        #region Default Frequency
                        string frequency = "1";
                        entity.GCDosingFrequency = Constant.DosingFrequency.DAY;
                        #endregion

                        bool isTwoDigit = (lstSelectedMemberSigna[ct].Substring(0, 2).Contains('q') || lstSelectedMemberSigna[ct].Substring(0, 2).Contains('d')) ? false : true;
                        if (isTwoDigit)
                            frequency = lstSelectedMemberSigna[ct].Substring(0, 2);
                        else
                            frequency = lstSelectedMemberSigna[ct].Substring(0, 1);

                        if (lstSelectedMemberSigna[ct].ToLower().Contains("qh"))
                        {
                            isDay = false;
                            entity.GCDosingFrequency = Constant.DosingFrequency.HOUR;
                        }
                        else
                        {
                            entity.GCDosingFrequency = Constant.DosingFrequency.DAY;
                        }
                        entity.Frequency = Convert.ToInt16(frequency);

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

                        entity.NumberOfDosage = Convert.ToDecimal(lstSelectedMemberQty[ct]);

                        bool isUsingStrength = lstSelectedMemberStrength[ct] == "1";
                        if (isUsingStrength && !String.IsNullOrEmpty(drugInfo.GCDoseUnit))
                            entity.GCDosingUnit = drugInfo.GCDoseUnit;
                        else
                            entity.GCDosingUnit = drugInfo.GCItemUnit;

                        if (lstSelectedMemberCoenam[ct] != "-")
                        {
                            switch (lstSelectedMemberCoenam[ct])
                            {
                                case "ac":
                                    entity.GCCoenamRule = Constant.CoenamRule.AC;
                                    break;
                                case "dc":
                                    entity.GCCoenamRule = Constant.CoenamRule.DC;
                                    break;
                                case "pc":
                                    entity.GCCoenamRule = Constant.CoenamRule.PC;
                                    break;
                                default:
                                    break;
                            }
                        }

                        entity.IsAsRequired = lstSelectedMemberPRN[ct] == "1";
                        entity.StartDate = Helper.GetDatePickerValue(lstSelectedMemberStartDate[ct]);
                        entity.StartTime = lstSelectedMemberStartTime[ct];
                        entity.DosingDuration = Convert.ToDecimal(lstSelectedMemberDuration[ct]);

                        if (!entity.IsUsingUDD)
                        {
                            if (isDay)
                            {
                                entity.DispenseQty = Math.Ceiling((decimal)(entity.DosingDuration * (entity.Frequency * entity.NumberOfDosage)));
                            }
                            else
                            {
                                decimal numberOfTaken = Math.Ceiling(Convert.ToDecimal(24 / entity.Frequency));
                                entity.DispenseQty = Math.Ceiling((decimal)(entity.DosingDuration * (numberOfTaken * entity.NumberOfDosage)));
                            }
                            entity.TakenQty = entity.ResultQty = entity.ChargeQty = entity.DispenseQty;
                        }
                        else
                        {
                            entity.DispenseQty = 0;
                            entity.TakenQty = 0;
                        }

                        entity.GCRoute = lstSelectedMemberRoute[ct];
                        entity.MedicationPurpose = "";
                        entity.MedicationAdministration = "";
                        entity.EmbalaceID = null;
                        entity.EmbalaceQty = 0;
                        entity.GCPrescriptionOrderStatus = Constant.OrderStatus.OPEN;

                        #endregion

                        entity.PrescriptionOrderID = prescriptionOrderID;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityOrderDtDao.Insert(entity);

                        ct++;
                    }
                }
                else
                {
                    errMessage = "Transaksi Sudah Diproses. Tidak Bisa Ditambahkan Item";
                    result = false;
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