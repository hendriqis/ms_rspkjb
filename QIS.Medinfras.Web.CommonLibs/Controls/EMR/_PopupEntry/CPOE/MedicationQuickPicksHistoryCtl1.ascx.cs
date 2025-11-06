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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicationQuickPicksHistoryCtl1 : BasePagePatientPageEntryCtl
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
            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS, Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);

            SettingParameterDt oParam1 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS).FirstOrDefault();
            SettingParameterDt oParam2 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault();
            bool isLimitedCPOEItemForBPJS = oParam1 != null ? (oParam1.ParameterValue == "1" ? true : false) : false;
            int businnessPartnerID = oParam2 != null ? Convert.ToInt32(oParam2.ParameterValue) : 0;

            if (AppSession.RegisteredPatient.BusinessPartnerID == businnessPartnerID)
            {
                rblItemType.SelectedValue = "2";
                rblItemType.Enabled = !isLimitedCPOEItemForBPJS;
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
                filterExpression += string.Format(" AND VisitDate BETWEEN '{0}' AND '{1}'", startDate,endDate);
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
                filterExpression = string.Format("VisitID = {0} AND OrderIsDeleted = 0 AND IsCompound = 0", hdnSelectedVisitID.Value);

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
                    int rowCount = BusinessLayer.GetvPrescriptionOrderDt1RowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderDetailID");
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

            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberStrength = hdnSelectedMemberStrength.Value.Split(',');
                string[] lstSelectedMemberFrequency = hdnSelectedMemberFrequency.Value.Split(',');
                //string[] lstSelectedMemberCoenam = hdnSelectedMemberCoenam.Value.Split(',');
                //string[] lstSelectedMemberPRN = hdnSelectedMemberPRN.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                string[] lstSelectedMemberDispenseQty = hdnSelectedMemberDispenseQty.Value.Split(',');
                string[] lstSelectedMemberRemarks = hdnSelectedMemberRemarks.Value.Split('|');
                //string[] lstSelectedMemberStartTime = hdnSelectedMemberStartTime.Value.Split(',');
                //string[] lstSelectedMemberRoute = hdnSelectedMemberRoute.Value.Split(',');

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
                    entityHd.IsCreatedBySystem = true;
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
                    List<vPrescriptionOrderDt1> lstDrugInfo = BusinessLayer.GetvPrescriptionOrderDt1List(string.Format("PrescriptionOrderDetailID IN ({0})", hdnSelectedMember.Value),ctx);
                    int ct = 0;
                    foreach (String detailID in lstSelectedMember)
                    {
                        vPrescriptionOrderDt1 drugInfo = lstDrugInfo.FirstOrDefault(p => p.PrescriptionOrderDetailID == Convert.ToInt32(detailID));

                        PrescriptionOrderDt entity = new PrescriptionOrderDt();
                        #region PrescriptionOrderDt
                        entity.IsRFlag = true;
                        entity.ItemID = Convert.ToInt32(drugInfo.ItemID);
                        entity.DrugName = drugInfo.DrugName;
                        entity.GenericName = drugInfo.GenericName;
                        if (drugInfo.GCDrugForm != null)
                        {
                            entity.GCDrugForm = drugInfo.GCDrugForm;
                        }
                        entity.SignaID = drugInfo.SignaID;
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

                        entity.GCCoenamRule = drugInfo.GCCoenamRule;
                        entity.IsAsRequired = drugInfo.IsAsRequired;
                        entity.StartDate = Helper.GetDatePickerValue(txtDefaultStartDate.Text);
                        entity.StartTime = "00:00";
                        entity.DispenseQty = Convert.ToDecimal(lstSelectedMemberDispenseQty[ct]);
                        entity.IsUsingUDD = false;

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
                            entity.TakenQty = entity.ResultQty = entity.ChargeQty = entity.DispenseQty;
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