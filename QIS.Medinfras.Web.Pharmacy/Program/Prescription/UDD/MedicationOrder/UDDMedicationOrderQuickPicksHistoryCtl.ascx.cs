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
    public partial class UDDMedicationOrderQuickPicksHistoryCtl : BaseEntryPopupCtl
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
            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}')",
                AppSession.UserLogin.HealthcareID,
                Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT,
                Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS,
                Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN,
                Constant.SettingParameter.PH0037,
                Constant.SettingParameter.PH_CREATE_QUEUE_LABEL,
                Constant.SettingParameter.PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX
                );
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);

            SettingParameterDt oParam1 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS).FirstOrDefault();
            SettingParameterDt oParam2 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault();
            bool isLimitedCPOEItemForBPJS = oParam1 != null ? (oParam1.ParameterValue == "1" ? true : false) : false;
            int businnessPartnerID = oParam2 != null ? Convert.ToInt32(oParam2.ParameterValue) : 0;

            hdnPrescriptionFeeAmount.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT).ParameterValue;
            hdnIsAutoGenerateReferenceNo.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH0037).ParameterValue;
            hdnIsGenerateQueueLabel.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_CREATE_QUEUE_LABEL).ParameterValue;
            hdnItemQtyWithSpecialQueuePrefix.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX).ParameterValue;

            if (AppSession.RegisteredPatient.GCCustomerType == AppSession.TipeCustomerBPJS)
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
            string filterExpression = string.Format("MRN = {0} AND DepartmentID = '{1}' AND VisitID IN (SELECT VisitID FROM PrescriptionOrderHd WHERE GCTransactionStatus NOT IN ('{2}','{3}'))", AppSession.RegisteredPatient.MRN, Constant.Facility.INPATIENT, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.VOID);

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

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPrescriptionOrderDt1RowCount(filterExpression);
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

        private bool IsValidated(string lstDosage, ref string result)
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
                                if (value == 0)
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

            if (!IsValidated(hdnSelectedMemberQty.Value, ref errMessage))
            {
                result = false;
                retval = "0";
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityOrderDtDao = new PrescriptionOrderDtDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);

            StringBuilder sbReferenceOrderInfo = new StringBuilder();

            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberOrderInfo = hdnSelectedMemberOrderInfo.Value.Split(';');
                string[] lstSelectedMemberStrength = hdnSelectedMemberStrength.Value.Split(',');
                string[] lstSelectedMemberFrequency = hdnSelectedMemberFrequency.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                string[] lstSelectedMemberDispenseQty = hdnSelectedMemberDispenseQty.Value.Split(',');
                string[] lstSelectedMemberTakenQty = hdnSelectedMemberTakenQty.Value.Split(',');
                string[] lstSelectedMemberRemarks = hdnSelectedMemberRemarks.Value.Split('|');

                int prescriptionOrderID = 0;
                string prescriptionOrderNo = string.Empty;
                DateTime prescriptionDate = Helper.DateInStringToDateTime(hdnPrescriptionDate.Value);
                string prescriptionTime = hdnPrescriptionTime.Value;
                int prescriptionOrderDetailID = 0;

                #region PrescriptionOrderHd
                PrescriptionOrderHd entityHd = new PrescriptionOrderHd();

                entityHd.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entityHd.PrescriptionDate = prescriptionDate;
                entityHd.PrescriptionTime = prescriptionTime;
                entityHd.ClassID = Convert.ToInt32(hdnChargeClassID.Value);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
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
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHd.IsCreatedBySystem = true;
                #endregion

                #region PrescriptionOrderDt
                List<PrescriptionOrderDt> lstPrescriptionOrderDt = new List<PrescriptionOrderDt>();

                List<PrescriptionOrderDt> lstDrugInfo = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderDetailID IN ({0}) OR ParentID IN ({0})", hdnSelectedMember.Value), ctx);
                int ct = 0;
                decimal compoundTakenQty = 0;
                decimal dispenseQty = 0;
                decimal takenQty = 0;
                foreach (PrescriptionOrderDt item in lstDrugInfo)
                {
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

                    if (item.Frequency == entity.Frequency && item.NumberOfDosage == entity.NumberOfDosage && item.GCDosingFrequency == entity.GCDosingFrequency)
                    {
                        entity.SignaID = item.SignaID;
                    }
                    else
                    {
                        entity.SignaID = null;
                    }
                    entity.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;

                    entity.IsCreatedFromOrder = false;
                    entity.PrescriptionOrderID = prescriptionOrderID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;

                    lstPrescriptionOrderDt.Add(entity);
                    #endregion

                    ct++;
                }
                #endregion

                #region Apply to Database
                #region PrescriptionOrderHd
                entityHd.Remarks = string.Format("Disalin dari Order No. : {0}{1}", Environment.NewLine, sbReferenceOrderInfo);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                prescriptionOrderID = entityOrderHdDao.InsertReturnPrimaryKeyID(entityHd);
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

                #endregion
                if (result == true)
                {
                    retval = prescriptionOrderID.ToString();
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