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

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class UDDMedicationOrderQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        private string[] lstDosingUnitValue = null;
        private string[] lstDosingUnitText = null;

        private UDDMedicationOrderEntry DetailPage
        {
            get { return (UDDMedicationOrderEntry)Page; }
        }

        public string GetItemUnitListCode()
        {
            return string.Join(",", lstDosingUnitValue);
        }

        public string GetItemUnitListText()
        {
            return string.Join(",", lstDosingUnitText);
        }

        protected string GetPrescriptionType()
        {
            return hdnPrescriptionType.Value;
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnTransactionID.Value = temp[0];
            hdnLocationID.Value = temp[1];
            hdnDefaultGCMedicationRoute.Value = temp[2];
            hdnParamedicID.Value = temp[3];
            hdnRegistrationID.Value = temp[4];
            hdnVisitID.Value = temp[5];
            hdnChargeClassID.Value = temp[6];
            hdnOrderNotes.Value = temp[7];
            hdnPrescriptionType.Value = temp[8];

            txtNotesQP.Text = temp[7];

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}')", Constant.StandardCode.ITEM_UNIT);
            lstDosingUnitValue = null;
            lstDosingUnitText = null;
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            List<StandardCode> lstItemUnit = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && (sc.TagProperty.Contains("PRE") || sc.TagProperty.Contains("1"))).ToList();
            lstDosingUnitValue = lstItemUnit.Select(lst => lst.StandardCodeID).ToArray();
            lstDosingUnitText = lstItemUnit.Select(lst => lst.StandardCodeName).ToArray();

            if (AppSession.RegisteredPatient.GCCustomerType == AppSession.TipeCustomerBPJS)
            {
                rblItemType.SelectedValue = "2";
                rblItemType.Enabled = !AppSession.IsLimitedCPOEItemForBPJS;
            }

            bool isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealth;

            switch (AppSession.RegisteredPatient.DepartmentID)
            {
                case Constant.Facility.INPATIENT:
                    isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthIP;
                    break;
                case Constant.Facility.OUTPATIENT:
                    isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthOP;
                    break;
                case Constant.Facility.EMERGENCY:
                    isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthER;
                    break;
                case Constant.Facility.DIAGNOSTIC:
                    isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthMD;
                    break;
                case Constant.Facility.PHARMACY:
                    isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthPH;
                    break;
                case Constant.Facility.MEDICAL_CHECKUP:
                    isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthMC;
                    break;
                default:
                    break;
            }

            if (AppSession.RegisteredPatient.GCCustomerType == AppSession.BusinessPartnerIDInhealth)
            {
                rblItemType.SelectedValue = "3";
                rblItemType.Enabled = !isLimitedCPOEItemForInhealth;
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

        private string GetFilterExpression()
        {
            string filterExpression = "";

            if (hdnItemGroupDrugLogisticID.Value == "")
                filterExpression += string.Format("LocationID = '{0}' AND GCItemType IN ('{2}','{3}') AND IsDeleted = 0 AND (ItemName1 LIKE '%{1}%' OR GenericName LIKE '%{1}%') AND IsDeleted = 0", cboPopupLocation.Value, hdnFilterItem.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);
            else
                filterExpression += string.Format("LocationID = '{0}' AND GCItemType IN ('{3}','{4}') AND IsDeleted = 0 AND (ItemName1 LIKE '%{1}%' OR GenericName LIKE '%{1}%') AND ItemGroupID IN(SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{2}/%') AND IsDeleted = 0", cboPopupLocation.Value, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);

            switch (rblItemType.SelectedValue)
            {
                case "1":
                    filterExpression += " AND IsFormularium = 1 ";
                    break;
                case "2":
                    filterExpression += " AND IsBPJSFormularium = 1 ";
                    break;
                case "3":
                    filterExpression += " AND IsInhealthFormularium = 1 ";
                    break;
                case "4":
                    filterExpression += " AND IsEmployeeFormularium = 1 ";
                    break;
                default:
                    break;
            }

            filterExpression += string.Format(" AND GCItemStatus = '{0}'", Constant.ItemStatus.ACTIVE);

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
                System.Drawing.Color backColor = System.Drawing.Color.White;

                if (entity.IsBPJSFormularium)
                {
                    backColor = System.Drawing.ColorTranslator.FromHtml("#adff2f");
                    foreColor = System.Drawing.Color.Black;
                }
                else if (entity.IsFormularium)
                {
                    backColor = System.Drawing.ColorTranslator.FromHtml("#FFFFE0");
                    foreColor = System.Drawing.Color.Black;
                }
                else if (entity.IsInhealthFormularium)
                {
                    backColor = System.Drawing.ColorTranslator.FromHtml("#b0e2ff");
                    foreColor = System.Drawing.Color.Black;
                }
                else if (entity.IsEmployeeFormularium)
                {
                    backColor = System.Drawing.ColorTranslator.FromHtml("#faf0e6");
                    foreColor = System.Drawing.Color.Black;
                }
                if (entity.QuantityEND == 0)
                    foreColor = System.Drawing.Color.Red;

                e.Row.Cells[2].BackColor = backColor;
                e.Row.Cells[3].BackColor = backColor;
                e.Row.Cells[4].BackColor = backColor;
                e.Row.Cells[5].BackColor = backColor;
                e.Row.Cells[2].ForeColor = foreColor;
                e.Row.Cells[3].ForeColor = foreColor;
                e.Row.Cells[4].ForeColor = foreColor;
                e.Row.Cells[5].ForeColor = foreColor;

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
            string message = string.Empty;

            if (string.IsNullOrEmpty(message))
            {
                #region Validate Duration
                string[] selectedDuration = lstDuration.Split(',');
                foreach (string duration in selectedDuration)
                {
                    if (string.IsNullOrEmpty(duration))
                    {
                        message = "There is medication with empty duration.";
                        break;
                    }
                    else
                    {
                        Decimal value;
                        if (!Decimal.TryParse(duration, out value))
                        {
                            message = string.Format("There is medication with invalid duration {0}", duration);
                            break;
                        }
                        else
                        {
                            if (value == 0)
                            {
                                message = string.Format("There is medication with invalid duration {0}", duration);
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
                        message = "There is medication with empty start date.";
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
                            message = string.Format("There is medication with invalid start date {0}", date);
                            break;
                        }
                        DateTime sdate = Helper.GetDatePickerValue(date);
                        if (DateTime.Compare(sdate, DateTime.Now.Date) < 0)
                        {
                            message = "There is medication with start date less than current date";
                            break;
                        }
                    }
                }
                #endregion
            }

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
                string[] lstSelectedMemberSigna = hdnSelectedMemberSigna.Value.Split(',');
                string[] lstSelectedMemberCoenam = hdnSelectedMemberCoenam.Value.Split(',');
                string[] lstSelectedMemberPRN = hdnSelectedMemberPRN.Value.Split(',');
                string[] lstSelectedMemberIMM = hdnSelectedMemberIMM.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                string[] lstSelectedMemberDosingUnit = hdnSelectedMemberDosingUnit.Value.Split(',');
                string[] lstSelectedMemberDispenseQty = hdnSelectedMemberDispenseQty.Value.Split(',');
                string[] lstSelectedMemberDuration = hdnSelectedMemberDuration.Value.Split(',');
                string[] lstSelectedMemberStartDate = hdnSelectedMemberStartDate.Value.Split(',');
                string[] lstSelectedMemberIsUsingUDD = hdnSelectedMemberIsUsingUDD.Value.Split(',');
                string[] lstSelectedMemberRoute = hdnSelectedMemberRoute.Value.Split(',');

                int prescriptionOrderID = 0;
                DetailPage.SavePrescriptionOrderHd(ctx, ref prescriptionOrderID, hdnPrescriptionType.Value);

                PrescriptionOrderHd orderHd = entityOrderHdDao.Get(prescriptionOrderID);
                if (orderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    List<vDrugInfo> lstDrugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);
                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        vDrugInfo drugInfo = lstDrugInfo.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));

                        PrescriptionOrderDt entity = new PrescriptionOrderDt();
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
                        string frequency = "1";
                        entity.GCDosingFrequency = Constant.DosingFrequency.DAY;

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
                        entity.NumberOfDosage = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                        entity.GCDosingUnit = lstSelectedMemberDosingUnit[ct];

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

                        entity.IsAsRequired = lstSelectedMemberPRN[ct] == "1" ? true : false;
                        entity.IsIMM = lstSelectedMemberIMM[ct] == "1" ? true : false;
                        entity.IsUsingUDD = lstSelectedMemberIsUsingUDD[ct] == "1" ? true : false;
                        entity.StartDate = Helper.GetDatePickerValue(lstSelectedMemberStartDate[ct]);
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
                        
                        entity.DosingDuration = Convert.ToDecimal(lstSelectedMemberDuration[ct]);

                        if (!entity.IsUsingUDD)
                        {
                            entity.DispenseQty = Convert.ToDecimal(lstSelectedMemberDispenseQty[ct]);
                            entity.TakenQty = entity.DispenseQty;
                            entity.ResultQty = entity.DispenseQty;
                            entity.ChargeQty = entity.DispenseQty;
                        }
                        else
                        {
                            entity.DispenseQty = 0;
                            entity.TakenQty = 0;
                            entity.ResultQty = 0;
                            entity.ChargeQty = 0;
                        }

                        entity.GCRoute = lstSelectedMemberRoute[ct];
                        entity.MedicationPurpose = drugInfo.MedicationPurpose;
                        entity.MedicationAdministration = "";
                        entity.EmbalaceID = null;
                        entity.EmbalaceQty = 0;
                        entity.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                        
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
                retval = prescriptionOrderID.ToString();
                ctx.CommitTransaction();
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