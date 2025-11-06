using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class OrderQuickPicksHistoryCtl1 : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override void InitializeDataControl(string param)
        {
            IsAdd = true;

            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnTransactionIDCtlQPH.Value = string.IsNullOrEmpty(temp[0]) ? "0" : temp[0];
            hdnLocationIDCtlQPH.Value = temp[1];
            hdnDefaultGCMedicationRouteCtlQPH.Value = temp[2];
            hdnParamedicIDCtlQPH.Value = temp[3];
            hdnRegistrationIDCtlQPH.Value = temp[4];
            hdnVisitIDCtlQPH.Value = temp[5];
            hdnChargeClassIDCtlQPH.Value = temp[6];

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
            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                                                            AppSession.UserLogin.HealthcareID, //0
                                                            Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT, //1
                                                            Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS, //2
                                                            Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN, //3
                                                            Constant.SettingParameter.PH0037, //4
                                                            Constant.SettingParameter.PH_CREATE_QUEUE_LABEL, //5
                                                            Constant.SettingParameter.PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX, //6
                                                            Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //7
                                                            Constant.SettingParameter.PH_IS_QPHISTORY_FOR_NEW_TRANSACTION, //8
                                                            Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1, //9
                                                            Constant.SettingParameter.IS_VISIBLE_FILTER_TAKEN_ITEM_IN_QUICK_PICKS_HISTORY, //10
                                                            Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS //11
                                                        );
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);

            hdnIsEndingAmountRoundingTo100.Value = lstParam.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstParam.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;

            string oParam1 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS).FirstOrDefault().ParameterValue;
            bool isLimitedCPOEItemForBPJS = oParam1 != null ? (oParam1 == "1" ? true : false) : false;

            string oParam2 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault().ParameterValue;
            int businnessPartnerID = oParam2 != null ? Convert.ToInt32(oParam2) : 0;

            string oParam3 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).FirstOrDefault().ParameterValue;
            string bpjsType = oParam3 != null ? oParam3 : string.Empty;

            hdnPrescriptionFeeAmount.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT).ParameterValue;
            hdnIsAutoGenerateReferenceNo.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH0037).ParameterValue;
            hdnIsGenerateQueueLabel.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_CREATE_QUEUE_LABEL).ParameterValue;
            hdnItemQtyWithSpecialQueuePrefix.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX).ParameterValue;
            hdnIsQPHistoryForNewTransactionCtlQPH.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_IS_QPHISTORY_FOR_NEW_TRANSACTION).ParameterValue;
            hdnIsVisibleFilterTakenItemInQuickPicksHistory.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_VISIBLE_FILTER_TAKEN_ITEM_IN_QUICK_PICKS_HISTORY).ParameterValue;

            if (AppSession.RegisteredPatient.GCCustomerType == bpjsType)
            {
                rblItemType.SelectedValue = "2";
                rblItemType.Enabled = !isLimitedCPOEItemForBPJS;
            }

            filterExpression = string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.TIPE_TRANSAKSI_BPJS);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboBPJSTransType, lstStandardCode, "StandardCodeName", "StandardCodeID");
//            cboBPJSTransType.SelectedIndex = 0;

            if (AppSession.RegisteredPatient.BusinessPartnerID == 1)
            {
                cboBPJSTransType.Value = Constant.BPJSTransactionType.DIBAYAR_PASIEN;
            }
            else
            {
                cboBPJSTransType.Value = Constant.BPJSTransactionType.DITAGIHKAN;
            }

            if (hdnIsVisibleFilterTakenItemInQuickPicksHistory.Value == "0")
            {
                trItemFilter.Style.Add("display", "none");
            }

            BindCboLocation();
        }

        private void BindCboLocation()
        {
            if (hdnLocationIDCtlQPH.Value != null && hdnLocationIDCtlQPH.Value != "" && hdnLocationIDCtlQPH.Value != "0")
            {
                Location loc = BusinessLayer.GetLocation(Convert.ToInt32(hdnLocationIDCtlQPH.Value));
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

            return filterExpression;
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
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisit4RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }

            List<vConsultVisit4> lstEntity = BusinessLayer.GetvConsultVisit4List(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex, "VisitID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        #region Detail Grid
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnSelectedVisitID.Value != "")
            {
                filterExpression = string.Format("VisitID = {0} AND OrderIsDeleted = 0 AND IsRFlag = 1 AND GCPrescriptionOrderStatus != '{1}'", hdnSelectedVisitID.Value, Constant.OrderStatus.CANCELLED);

                filterExpression += string.Format(" AND ItemID IN (SELECT ItemID FROM ItemBalance WHERE IsDeleted = 0 AND LocationID = '{0}')", Convert.ToInt32(cboPopupLocation.Value));

                switch (rblItemType.SelectedValue)
                {
                    case "1":
                        filterExpression += " AND (IsFormularium = 1 OR IsCompound = 1)";
                        break;
                    case "2":
                        filterExpression += " AND (IsBPJSFormularium = 1 OR IsCompound = 1)";
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

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPrescriptionOrderDt10RowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPrescriptionOrderDt10> lstEntity = BusinessLayer.GetvPrescriptionOrderDt10List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderDetailID");
            grdPopupViewDt.DataSource = lstEntity;
            grdPopupViewDt.DataBind();
        }

        protected void grdPopupViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPrescriptionOrderDt10 entity = e.Row.DataItem as vPrescriptionOrderDt10;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;

                if (entity.GCItemStatus == Constant.ItemStatus.IN_ACTIVE)
                {
                    chkIsSelected.Enabled = false;
                    chkIsSelected.CssClass = "chkIsNotSelected";
                }
                else
                {
                    chkIsSelected.Enabled = true;
                    chkIsSelected.CssClass = "chkIsSelected";
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

        private bool IsValidated(string lstDosage, string lstFrequency, string lstDispense, string lstTaken, ref string result)
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

                #region Validate Taken
                string[] selectedTaken = lstTaken.Split(',');
                foreach (string taken in selectedTaken)
                {
                    if (string.IsNullOrEmpty(taken))
                    {
                        tempMsg.AppendLine("There is medication with empty taken quantity. \n");
                        break;
                    }
                    else
                    {
                        if (!taken.Contains("/"))
                        {
                            Decimal value;
                            if (!Decimal.TryParse(taken, out value))
                            {
                                tempMsg.AppendLine(string.Format("There is medication with invalid taken {0} \n", taken));
                                break;
                            }
                            else
                            {
                                if (value <= 0)
                                {
                                    tempMsg.AppendLine(string.Format("There is medication with invalid taken {0} \n", taken));
                                    break;
                                }
                            }
                        }
                        else
                        {
                            string[] takenInfo = taken.Split('/');
                            Decimal value;
                            if (!Decimal.TryParse(takenInfo[0], out value))
                            {
                                tempMsg.AppendLine(string.Format("There is medication with invalid taken {0} \n", takenInfo[0]));
                                break;
                            }
                            if (!Decimal.TryParse(takenInfo[1], out value))
                            {
                                tempMsg.AppendLine(string.Format("There is medication with invalid taken {0} \n", takenInfo[1]));
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

            if (!IsValidated(hdnSelectedMemberQty.Value, hdnSelectedMemberFrequency.Value, hdnSelectedMemberDispenseQty.Value, hdnSelectedMemberTakenQty.Value, ref errMessage))
            {
                result = false;
                retval = "0";
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityOrderDtDao = new PrescriptionOrderDtDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesHdDao entityChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesHdInfoDao entityChargesHdInfoDao = new PatientChargesHdInfoDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);

            StringBuilder sbReferenceOrderInfo = new StringBuilder();

            try
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
                int transactionID = 0;
                string transactionNo = string.Empty;
                DateTime prescriptionDate = Helper.DateInStringToDateTime(hdnPrescriptionDate.Value);
                string prescriptionTime = hdnPrescriptionTime.Value;
                int prescriptionOrderDetailID = 0;

                if (hdnIsQPHistoryForNewTransactionCtlQPH.Value != "1" && hdnTransactionIDCtlQPH.Value != null && hdnTransactionIDCtlQPH.Value != "" && hdnTransactionIDCtlQPH.Value != "0")
                {
                    transactionID = Convert.ToInt32(hdnTransactionIDCtlQPH.Value);
                    PatientChargesHd chargesHd = entityChargesHdDao.Get(transactionID);
                    prescriptionOrderID = Convert.ToInt32(chargesHd.PrescriptionOrderID);
                }
                else
                {
                    #region PrescriptionOrderHd
                    PrescriptionOrderHd entityHd = new PrescriptionOrderHd();

                    entityHd.ParamedicID = Convert.ToInt32(hdnParamedicIDCtlQPH.Value);
                    entityHd.VisitID = Convert.ToInt32(hdnVisitIDCtlQPH.Value);
                    entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    entityHd.PrescriptionDate = prescriptionDate;
                    entityHd.PrescriptionTime = prescriptionTime;
                    entityHd.ClassID = Convert.ToInt32(hdnChargeClassIDCtlQPH.Value);
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                    entityHd.GCOrderStatus = Constant.OrderStatus.IN_PROGRESS;
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

                    entityHd.LocationID = Convert.ToInt32(hdnLocationIDCtlQPH.Value);
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHd.IsCreatedBySystem = true;
                    entityHd.Remarks = string.Format("Disalin dari Order No. : {0}{1}", Environment.NewLine, sbReferenceOrderInfo);
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    prescriptionOrderID = entityOrderHdDao.InsertReturnPrimaryKeyID(entityHd);
                    prescriptionOrderNo = entityHd.PrescriptionOrderNo;
                    #endregion

                    #region PatientChargesHd
                    PatientChargesHd entityPatientChargesHd = new PatientChargesHd();

                    entityPatientChargesHd.VisitID = Convert.ToInt32(hdnVisitIDCtlQPH.Value);
                    entityPatientChargesHd.TransactionDate = prescriptionDate;
                    entityPatientChargesHd.TransactionTime = prescriptionTime;
                    entityPatientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityPatientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnDispensaryUnitID.Value);
                    switch (AppSession.RegisteredPatient.DepartmentID)
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
                            if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() == AppSession.ImagingServiceUnitID.ToString())
                                entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_IMAGING;
                            else if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() == AppSession.LaboratoryServiceUnitID.ToString())
                                entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_LABORATORY;
                            else
                                entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_OTHER;
                            break;
                    }

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityPatientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityPatientChargesHd.TransactionCode, entityPatientChargesHd.TransactionDate, ctx);
                    if (hdnIsAutoGenerateReferenceNo.Value == "1")
                    {
                        entityPatientChargesHd.ReferenceNo = BusinessLayer.GeneratePrescriptionReferenceNo(entityPatientChargesHd.HealthcareServiceUnitID, entityPatientChargesHd.TransactionDate, ctx);
                    }

                    entityPatientChargesHd.Remarks = sbReferenceOrderInfo.ToString();
                    entityPatientChargesHd.PrescriptionOrderID = prescriptionOrderID;
                    entityPatientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    transactionID = entityChargesHdDao.InsertReturnPrimaryKeyID(entityPatientChargesHd);

                    PatientChargesHdInfo oChargesHdInfo = entityChargesHdInfoDao.Get(transactionID);
                    if (oChargesHdInfo != null)
                    {
                        oChargesHdInfo.GCBPJSTransactionType = cboBPJSTransType.Value.ToString();
                        entityChargesHdInfoDao.Update(oChargesHdInfo);
                    }
                    #endregion
                }

                #region PrescriptionOrderDt + PatientChargesDt
                List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();

                List<PrescriptionOrderDt> lstPrescriptionOrderDt = new List<PrescriptionOrderDt>();

                List<PrescriptionOrderDt> lstDrugInfo = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderDetailID IN ({0}) OR ParentID IN ({0}) AND IsDeleted = 0", hdnSelectedMember.Value), ctx);
                int ct = 0;
                decimal compoundTakenQty = 0;
                decimal dispenseQty = 0;
                decimal takenQty = 0;
                int totalDetailPerCompound = 0;
                int ctCompound = 1;
                foreach (PrescriptionOrderDt item in lstDrugInfo)
                {
                    if (totalDetailPerCompound == 0)
                    {
                        if (item.IsCompound) //jika racikan true
                        {
                            if (item.ParentID == null)
                            { //jika item nya adalah parentid
                                totalDetailPerCompound = lstDrugInfo.Where(w => w.ParentID == item.PrescriptionOrderDetailID).ToList().Count + 1; //ambil item berdasarkan parentID + 1 (si parent nya)
                            }
                            else
                            { //jika item bukan parentID
                                totalDetailPerCompound = lstDrugInfo.Where(w => w.ParentID == item.ParentID || w.PrescriptionOrderDetailID == item.ParentID).ToList().Count; //ambil semua racikan yg sama
                            }
                        }
                        else
                        {
                            totalDetailPerCompound = 0;
                        }
                    }
                    PrescriptionOrderDt entity = new PrescriptionOrderDt();
                    entity = item;

                    Int16 oldFrequency = item.Frequency;
                    Decimal oldNumberOfDosage = item.NumberOfDosage;
                    String oldGCDosingFrequency = item.GCDosingFrequency;

                    #region PrescriptionOrderDt
                    if (lstSelectedMemberOrderInfo.Length > ct)
                    {
                        if (!sbReferenceOrderInfo.ToString().Contains(lstSelectedMemberOrderInfo[ct]))
                        {
                            sbReferenceOrderInfo.AppendLine(lstSelectedMemberOrderInfo[ct]);
                        }
                    }

                    if (lstSelectedMemberDispenseQty.Length > ct){
                        dispenseQty = Convert.ToDecimal(lstSelectedMemberDispenseQty[ct]);
                    }

                    if (lstSelectedMemberTakenQty.Length > ct){
                        takenQty = Convert.ToDecimal(lstSelectedMemberTakenQty[ct]);
                    }

                    if (lstSelectedMemberRemarks.Length > ct)
                    {
                        entity.MedicationAdministration = lstSelectedMemberRemarks[ct];
                    }

                    bool isDay = true;
                    entity.Frequency = Convert.ToInt16(lstSelectedMemberFrequency[ct]);
                    entity.NumberOfDosage = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                    entity.StartDate = Helper.GetDatePickerValue(txtDefaultStartDate.Text);
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

                            entity.TakenQty = compoundTakenQty;

                            vDrugInfo1 oDrugInfo = BusinessLayer.GetvDrugInfo1List(string.Format("ItemID = {0}", entity.ItemID), ctx).FirstOrDefault();
                            if (oDrugInfo != null)
                            {
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

                                if (oDrugInfo.GCItemUnit != entity.GCCompoundUnit)
                                {
                                    decimal dose = Convert.ToDecimal(entity.Dose);
                                    entity.ResultQty = qty * entity.TakenQty / dose;
                                }
                                else
                                {
                                    entity.ResultQty = qty * entity.TakenQty;
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
                    if (oldFrequency == entity.Frequency && oldNumberOfDosage == entity.NumberOfDosage && oldGCDosingFrequency == entity.GCDosingFrequency)
                    {
                        entity.SignaID = item.SignaID;
                    }
                    else
                    {
                        entity.SignaID = null;
                    }

                    entity.GCPrescriptionOrderStatus = Constant.OrderStatus.IN_PROGRESS;

                    entity.IsCreatedFromOrder = false;
                    entity.PrescriptionOrderID = prescriptionOrderID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;

                    lstPrescriptionOrderDt.Add(entity);
                    //prescriptionOrderDetailID = entityOrderDtDao.InsertReturnPrimaryKeyID(entity);
                    #endregion

                    #region PatientChargesDt
                    PatientChargesDt entityChargesDt = new PatientChargesDt();

                    entityChargesDt.ItemID = (int)entity.ItemID;
                    entityChargesDt.LocationID = Convert.ToInt32(hdnLocationIDCtlQPH.Value);
                    entityChargesDt.ChargeClassID = Convert.ToInt32(hdnChargeClassIDCtlQPH.Value);

                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationIDCtlQPH.Value), Convert.ToInt32(hdnVisitIDCtlQPH.Value), entityChargesDt.ChargeClassID, (int)entity.ItemID, 2, prescriptionDate, ctx);

                    decimal basePrice = 0;
                    decimal basePriceComp1 = 0;
                    decimal basePriceComp2 = 0;
                    decimal basePriceComp3 = 0;
                    decimal price = 0;
                    decimal priceComp1 = 0;
                    decimal priceComp2 = 0;
                    decimal priceComp3 = 0;
                    bool isDiscountUsedComp = false;
                    decimal discountAmount = 0;
                    decimal discountAmountComp1 = 0;
                    decimal discountAmountComp2 = 0;
                    decimal discountAmountComp3 = 0;
                    decimal coverageAmount = 0;
                    bool isDiscountInPercentage = false;
                    bool isDiscountInPercentageComp1 = false;
                    bool isDiscountInPercentageComp2 = false;
                    bool isDiscountInPercentageComp3 = false;
                    bool isCoverageInPercentage = false;
                    decimal costAmount = 0;

                    if (list.Count > 0)
                    {
                        GetCurrentItemTariff obj = list[0];
                        basePrice = obj.BasePrice;
                        basePriceComp1 = obj.BasePriceComp1;
                        basePriceComp2 = obj.BasePriceComp2;
                        basePriceComp3 = obj.BasePriceComp3;
                        price = obj.Price;
                        priceComp1 = obj.PriceComp1;
                        priceComp2 = obj.PriceComp2;
                        priceComp3 = obj.PriceComp3;
                        isDiscountUsedComp = obj.IsDiscountUsedComp;
                        discountAmount = obj.DiscountAmount;
                        discountAmountComp1 = obj.DiscountAmountComp1;
                        discountAmountComp2 = obj.DiscountAmountComp2;
                        discountAmountComp3 = obj.DiscountAmountComp3;
                        coverageAmount = obj.CoverageAmount;
                        isDiscountInPercentage = obj.IsDiscountInPercentage;
                        isDiscountInPercentageComp1 = obj.IsDiscountInPercentageComp1;
                        isDiscountInPercentageComp2 = obj.IsDiscountInPercentageComp2;
                        isDiscountInPercentageComp3 = obj.IsDiscountInPercentageComp3;
                        isCoverageInPercentage = obj.IsCoverageInPercentage;
                        costAmount = obj.CostAmount;
                    }

                    entityChargesDt.BaseTariff = basePrice;
                    entityChargesDt.BaseComp1 = basePriceComp1;
                    entityChargesDt.BaseComp2 = basePriceComp2;
                    entityChargesDt.BaseComp3 = basePriceComp3;
                    entityChargesDt.Tariff = price;
                    entityChargesDt.TariffComp1 = priceComp1;
                    entityChargesDt.TariffComp2 = priceComp2;
                    entityChargesDt.TariffComp3 = priceComp3;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    ItemMaster entityItemMaster = itemDao.Get((int)entity.ItemID);
                    entityChargesDt.GCBaseUnit = entityItemMaster.GCItemUnit;
                    entityChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                    entityChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicIDCtlQPH.Value);

                    entityChargesDt.IsVariable = false;
                    entityChargesDt.IsUnbilledItem = false;

                    entityChargesDt.UsedQuantity = entity.ResultQty;
                    entityChargesDt.ChargedQuantity = entity.ChargeQty;
                    entityChargesDt.BaseQuantity = entity.ResultQty;

                    entityChargesDt.PrescriptionFeeAmount = Convert.ToDecimal(hdnPrescriptionFeeAmount.Value);

                    if (entity.EmbalaceID != null)
                    {
                        EmbalaceDt embalace = BusinessLayer.GetEmbalaceDtList(string.Format("EmbalaceID = {0} AND StartingQty <= {1} AND EndingQty >= {1}", entity.EmbalaceID, entity.EmbalaceQty), ctx).FirstOrDefault();
                        decimal tariff = 0;

                        if (embalace != null)
                        {
                            tariff = embalace.Tariff;
                        }

                        entityChargesDt.EmbalaceAmount = Convert.ToDecimal(tariff * entity.EmbalaceQty);
                    }
                    else
                    {
                        entityChargesDt.EmbalaceAmount = 0;
                    }

                    decimal grossLineAmount = (entityChargesDt.ChargedQuantity * price) + entityChargesDt.EmbalaceAmount + entityChargesDt.PrescriptionFeeAmount;

                    decimal totalDiscountAmount = 0;
                    decimal totalDiscountAmount1 = 0;
                    decimal totalDiscountAmount2 = 0;
                    decimal totalDiscountAmount3 = 0;

                    if (isDiscountInPercentage || isDiscountInPercentageComp1 || isDiscountInPercentageComp2 || isDiscountInPercentageComp3)
                    {
                        if (isDiscountUsedComp)
                        {
                            if (priceComp1 > 0)
                            {
                                if (isDiscountInPercentageComp1)
                                {
                                    totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                                    entityChargesDt.DiscountPercentageComp1 = discountAmountComp1;
                                }
                                else
                                {
                                    totalDiscountAmount1 = discountAmountComp1;
                                }
                            }

                            if (priceComp2 > 0)
                            {
                                if (isDiscountInPercentageComp2)
                                {
                                    totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                    entityChargesDt.DiscountPercentageComp2 = discountAmountComp2;
                                }
                                else
                                {
                                    totalDiscountAmount2 = discountAmountComp2;
                                }
                            }

                            if (priceComp3 > 0)
                            {
                                if (isDiscountInPercentageComp3)
                                {
                                    totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                    entityChargesDt.DiscountPercentageComp3 = discountAmountComp3;
                                }
                                else
                                {
                                    totalDiscountAmount3 = discountAmountComp3;
                                }
                            }
                        }
                        else
                        {
                            if (priceComp1 > 0)
                            {
                                totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                                entityChargesDt.DiscountPercentageComp1 = discountAmount;
                            }

                            if (priceComp2 > 0)
                            {
                                totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                entityChargesDt.DiscountPercentageComp2 = discountAmount;
                            }

                            if (priceComp3 > 0)
                            {
                                totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                entityChargesDt.DiscountPercentageComp3 = discountAmount;
                            }
                        }

                        if (entityChargesDt.DiscountPercentageComp1 > 0)
                        {
                            entityChargesDt.IsDiscountInPercentageComp1 = true;
                        }

                        if (entityChargesDt.DiscountPercentageComp2 > 0)
                        {
                            entityChargesDt.IsDiscountInPercentageComp2 = true;
                        }

                        if (entityChargesDt.DiscountPercentageComp3 > 0)
                        {
                            entityChargesDt.IsDiscountInPercentageComp3 = true;
                        }
                    }
                    else
                    {
                        if (isDiscountUsedComp)
                        {
                            if (priceComp1 > 0)
                                totalDiscountAmount1 = discountAmountComp1;
                            if (priceComp2 > 0)
                                totalDiscountAmount2 = discountAmountComp2;
                            if (priceComp3 > 0)
                                totalDiscountAmount3 = discountAmountComp3;
                        }
                        else
                        {
                            if (priceComp1 > 0)
                                totalDiscountAmount1 = discountAmount;
                            if (priceComp2 > 0)
                                totalDiscountAmount2 = discountAmount;
                            if (priceComp3 > 0)
                                totalDiscountAmount3 = discountAmount;
                        }
                    }

                    totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (entityChargesDt.ChargedQuantity);

                    if (grossLineAmount > 0)
                    {
                        if (totalDiscountAmount > grossLineAmount)
                        {
                            totalDiscountAmount = grossLineAmount;
                        }
                    }

                    decimal total = grossLineAmount - totalDiscountAmount;
                    decimal totalPayer = 0;
                    if (isCoverageInPercentage)
                    {
                        totalPayer = total * coverageAmount / 100;
                    }
                    else
                    {
                        totalPayer = coverageAmount * entityChargesDt.ChargedQuantity;
                    }

                    if (total == 0)
                    {
                        totalPayer = total;
                    }
                    else
                    {
                        if (totalPayer < 0 && totalPayer < total)
                        {
                            totalPayer = total;
                        }
                        else if (totalPayer > 0 & totalPayer > total)
                        {
                            totalPayer = total;
                        }
                    }

                    if (entity.ConversionFactor != 0)
                    {
                        entityChargesDt.ConversionFactor = entity.ConversionFactor;
                    }
                    else
                    {
                        entityChargesDt.ConversionFactor = 1;
                    }

                    ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityChargesDt.ItemID), ctx).FirstOrDefault();
                    entityChargesDt.AveragePrice = iPlanning.AveragePrice;
                    entityChargesDt.CostAmount = iPlanning.UnitPrice;

                    entityChargesDt.IsCITO = false;
                    entityChargesDt.CITOAmount = 0;
                    entityChargesDt.IsComplication = false;
                    entityChargesDt.ComplicationAmount = 0;
                    //entityChargesDt.IsDiscount = totalDiscountAmount > 0;
                    //entityChargesDt.DiscountAmount = totalDiscountAmount;
                    //if (entityChargesDt.ChargedQuantity > 0)
                    //{
                    //    entityChargesDt.DiscountComp1 = totalDiscountAmount / entityChargesDt.ChargedQuantity;
                    //}

                    entityChargesDt.IsDiscount = totalDiscountAmount != 0;
                    entityChargesDt.DiscountAmount = totalDiscountAmount;
                    entityChargesDt.DiscountComp1 = totalDiscountAmount1;
                    entityChargesDt.DiscountComp2 = totalDiscountAmount2;
                    entityChargesDt.DiscountComp3 = totalDiscountAmount3;

                    decimal oPatientAmount = total - totalPayer;
                    decimal oPayerAmount = totalPayer;
                    decimal oLineAmount = total;

                    if (hdnIsEndingAmountRoundingTo1.Value == "1")
                    {
                        decimal upPatientAmount = Math.Ceiling(oPatientAmount);
                        decimal diffUpPatientAmount = upPatientAmount - oPatientAmount;
                        if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
                        {
                            oPatientAmount = Math.Floor(oPatientAmount);
                        }
                        else
                        {
                            oPatientAmount = Math.Ceiling(oPatientAmount);
                        }

                        decimal upPayerAmount = Math.Ceiling(oPayerAmount);
                        decimal diffUpPayerAmount = upPayerAmount - oPayerAmount;
                        if (diffUpPayerAmount >= Convert.ToDecimal("0.5"))
                        {
                            oPayerAmount = Math.Floor(oPayerAmount);
                        }
                        else
                        {
                            oPayerAmount = Math.Ceiling(oPayerAmount);
                        }

                        oLineAmount = oPatientAmount + oPayerAmount;
                    }

                    if (hdnIsEndingAmountRoundingTo100.Value == "1")
                    {
                        oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                        oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                        oLineAmount = oPatientAmount + oPayerAmount;
                    }

                    entityChargesDt.PatientAmount = oPatientAmount;
                    entityChargesDt.PayerAmount = oPayerAmount;
                    entityChargesDt.LineAmount = oLineAmount;

                    entityChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                    entityChargesDt.IsCreatedBySystem = false;
                    entityChargesDt.CreatedBy = AppSession.UserLogin.UserID;

                    lstPatientChargesDt.Add(entityChargesDt);
                    #endregion

                    if (!item.IsCompound)
                    {
                        totalDetailPerCompound = 0;
                        ctCompound = 1;
                        ct++;
                    }
                    else
                    {
                        if (ctCompound != totalDetailPerCompound)
                        {
                            ctCompound++;
                        }
                        else
                        {
                            ctCompound = 1;
                            totalDetailPerCompound = 0;
                            ct++;
                        }
                    }
                }
                #endregion

                #region PatientChargesHd-Part2

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                PatientChargesHd chargesHdp2 = entityChargesHdDao.Get(transactionID);
                bool isSpecialTrx = lstPrescriptionOrderDt.Count(lst => lst.IsCompound) > 0;
                if (!isSpecialTrx)
                {
                    if (hdnItemQtyWithSpecialQueuePrefix.Value != "" && hdnItemQtyWithSpecialQueuePrefix.Value != "0")
                    {
                        if (lstPrescriptionOrderDt.Count > Convert.ToInt32(hdnItemQtyWithSpecialQueuePrefix.Value))
                        {
                            isSpecialTrx = true;
                        }
                    }
                }
                if (hdnIsGenerateQueueLabel.Value == "1")
                {
                    chargesHdp2.QueueNoLabel = BusinessLayer.GenerateChargesQueueNoLabel(chargesHdp2.HealthcareServiceUnitID, chargesHdp2.TransactionDate, isSpecialTrx, ctx);
                }
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityChargesHdDao.Update(chargesHdp2);

                #endregion

                #region Apply to Database

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
                            if (orderDt.ParentID == oldParentID)
                                orderDt.ParentID = parentID;
                        }
                    }
                    prescriptionOrderDetailID = entityOrderDtDao.InsertReturnPrimaryKeyID(orderDt);
                    if (orderDt.IsRFlag && orderDt.IsCompound)
                        parentID = prescriptionOrderDetailID;

                    lstOrderDetailID.Add(prescriptionOrderDetailID);
                }
                #endregion

                #region PatientChargesDt
                int counter = 0;
                foreach (PatientChargesDt chargesDt in lstPatientChargesDt)
                {
                    chargesDt.TransactionID = transactionID;
                    chargesDt.PrescriptionOrderDetailID = lstOrderDetailID[counter];
                    chargesDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityChargesDtDao.Insert(chargesDt);
                    counter += 1;
                }
                #endregion

                #endregion

                if (result == true)
                {
                    retval = transactionID.ToString();
                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = string.Format("<strong>{0} ({1})</strong><br/><br/><i>{2}</i>", ex.Message, ex.Source, ex.StackTrace);
                Helper.InsertErrorLog(ex);
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