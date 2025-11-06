using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
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
    public partial class UDDMedicationOrderCompoundEntryCtl1 : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private UDDMedicationOrderEntry DetailPage
        {
            get { return (UDDMedicationOrderEntry)Page; }
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

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            BindCboLocation();

            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.COENAM_RULE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimelineCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
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
            txtFrequencyNumber.Text = "1";
            txtDosingDose.Text = "1";
            txtDosingDuration.Text = "1";
            txtDispenseQty.Text = "1";
            txtTakenQty.Text = "1";
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
                filterExpression += string.Format("LocationID = '{0}' AND GCItemType IN ('{2}') AND IsDeleted = 0 AND (ItemName1 LIKE '%{1}%' OR GenericName LIKE '%{1}%') AND IsDeleted = 0", cboPopupLocation.Value, hdnFilterItem.Value, Constant.ItemType.OBAT_OBATAN);
            else
                filterExpression += string.Format("LocationID = '{0}' AND GCItemType IN ('{3}') AND IsDeleted = 0 AND (ItemName1 LIKE '%{1}%' OR GenericName LIKE '%{1}%') AND ItemGroupID IN(SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{2}/%') AND IsDeleted = 0", cboPopupLocation.Value, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value, Constant.ItemType.OBAT_OBATAN);

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

        private bool IsValidated(ref string result)
        {
            string message = string.Empty;

            if (string.IsNullOrEmpty(message))
            {
                #region Validate Duration
                if (string.IsNullOrEmpty(txtDosingDuration.Text))
                {
                    message = "There is medication with empty duration.";
                }
                else
                {
                    Decimal value;
                    if (!Decimal.TryParse(txtDosingDuration.Text, out value))
                    {
                        message = string.Format("There is medication with invalid duration {0}", txtDosingDuration.Text);
                    }
                    else
                    {
                        if (value == 0)
                        {
                            message = string.Format("There is medication with invalid duration {0}", txtDosingDuration.Text);
                        }
                    }
                }

                #endregion

                #region Validate Start Date
                if (string.IsNullOrEmpty(txtStartDate.Text))
                {
                    message = "There is medication with empty start date.";
                }
                else
                {
                    DateTime startDate;
                    string format = Constant.FormatString.DATE_PICKER_FORMAT;
                    try
                    {
                        startDate = DateTime.ParseExact(txtStartDate.Text, format, CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        message = string.Format("There is medication with invalid start date {0}", txtStartDate.Text);
                    }
                    DateTime sdate = Helper.GetDatePickerValue(txtStartDate);
                    if (DateTime.Compare(sdate, DateTime.Now.Date) < 0)
                    {
                        message = "There is medication with start date less than current date";
                    }
                }
                #endregion

                if (cboDosingUnitCompoundCtl.Value == null)
                {
                    message = "Satuan obat racikan belum didefinisikan";
                }
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

            if (!IsValidated(ref errMessage))
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
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');

                int prescriptionOrderID = 0;
                int parentID = 0;
                DetailPage.SavePrescriptionOrderHd(ctx, ref prescriptionOrderID);

                PrescriptionOrderHd orderHd = entityOrderHdDao.Get(prescriptionOrderID);
                if (orderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    List<vDrugInfo1> lstDrugInfo = BusinessLayer.GetvDrugInfo1List(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);
                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        vDrugInfo1 drugInfo = lstDrugInfo.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));

                        PrescriptionOrderDt entity = new PrescriptionOrderDt();
                        #region PrescriptionOrderDt
                        entity.IsRFlag = ct == 0;   
                        entity.IsCompound = true;
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

                        bool isUsingStrength = lstSelectedMemberStrength[ct] == "1";
                        if (isUsingStrength && !String.IsNullOrEmpty(drugInfo.GCDoseUnit))
                        {
                            entity.GCCompoundUnit = drugInfo.GCDoseUnit;
                            entity.ConversionFactor = 1 / drugInfo.Dose;
                        }
                        else
                        {
                            entity.GCCompoundUnit = drugInfo.GCItemUnit;
                            entity.ConversionFactor = 1;
                        }

                        entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text.Replace(',', '.'));
                        entity.TakenQty = Convert.ToDecimal(Request.Form[txtTakenQty.UniqueID].Replace(',', '.'));
                        if (drugInfo.GCItemUnit != entity.GCCompoundUnit)
                        {
                            string compoundQty = lstSelectedMemberQty[ct].Replace(',', '.');
                            decimal qty = 0;
                            if (compoundQty.Contains('/'))
                            {
                                string[] compoundInfo = compoundQty.Split('/');
                                decimal num1 = Convert.ToDecimal(compoundInfo[0]);
                                decimal num2 = Convert.ToDecimal(compoundInfo[1]);
                                //qty = Math.Round(num1 / num2, 2);

                                decimal dose = drugInfo.Dose;
                                entity.CompoundQtyInString = compoundQty;
                                entity.CompoundQty = num1 / num2 / dose;
                                entity.ResultQty = num1 / num2 * entity.TakenQty / dose;
                            }
                            else
                            {
                                qty = Convert.ToDecimal(compoundQty);

                                decimal dose = drugInfo.Dose;
                                entity.CompoundQtyInString = compoundQty;
                                entity.CompoundQty = qty / dose;
                                entity.ResultQty = qty * entity.TakenQty / dose;
                            }
                        }
                        else
                        {
                            string compoundQty = lstSelectedMemberQty[ct].Replace(',', '.');
                            decimal qty = 0;
                            if (compoundQty.Contains('/'))
                            {
                                string[] compoundInfo = compoundQty.Split('/');
                                decimal num1 = Convert.ToDecimal(compoundInfo[0]);
                                decimal num2 = Convert.ToDecimal(compoundInfo[1]);
                                //qty = Math.Round(num1 / num2, 2);

                                entity.CompoundQty = num1 / num2;
                                entity.CompoundQtyInString = compoundQty;
                                entity.ResultQty = num1 / num2 * entity.TakenQty;
                            }
                            else
                            {
                                qty = Convert.ToDecimal(compoundQty);

                                entity.CompoundQty = qty;
                                entity.CompoundQtyInString = compoundQty;
                                entity.ResultQty = qty * entity.TakenQty;
                            }
                        }

                        entity.ChargeQty = entity.ResultQty;
                        if (drugInfo.GCStockDeductionType == Constant.QuantityDeductionType.DIBULATKAN) entity.ChargeQty = Math.Ceiling(entity.ChargeQty);
                        if (drugInfo.GCConsumptionDeductionType == Constant.QuantityDeductionType.DIBULATKAN) entity.ResultQty = Math.Ceiling(entity.ResultQty);
                        entity.CompoundDrugname = txtCompoundMedicationName.Text;
                        entity.GCRoute = cboMedicationRouteCompoundCtl.Value.ToString();
                        entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text.Replace(',', '.'));
                        entity.GCDosingUnit = cboDosingUnitCompoundCtl.Value.ToString();
                        entity.IsUseSweetener = chkIsUsingSweetener.Checked;
                        entity.IsAsRequired = chkIsAsRequired.Checked;
                        entity.IsIMM = chkIsIMM.Checked;
                        entity.GCDosingFrequency = cboFrequencyTimelineCompoundCtl.Value.ToString();
                        entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
                        if (cboCoenamRuleCompoundCtl.Value != null)
                            entity.GCCoenamRule = cboCoenamRuleCompoundCtl.Value.ToString();
                        entity.DosingDuration = Convert.ToInt16(txtDosingDuration.Text);
                        entity.IsUsingUDD = false;
                        entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
                        entity.StartTime = txtStartTime1.Text.Replace('.', ':');
                        entity.Sequence1Time = txtStartTime1.Text.Replace('.', ':');
                        entity.Sequence2Time = txtStartTime2.Text.Replace('.', ':');
                        entity.Sequence3Time = txtStartTime3.Text.Replace('.', ':');
                        entity.Sequence4Time = txtStartTime4.Text.Replace('.', ':');
                        entity.Sequence5Time = txtStartTime5.Text.Replace('.', ':');
                        entity.Sequence6Time = txtStartTime6.Text.Replace('.', ':');
                        entity.IsMorning = chkIsMorning.Checked;
                        entity.IsNoon = chkIsNoon.Checked;
                        entity.IsEvening = chkIsEvening.Checked;
                        entity.IsNight = chkIsNight.Checked;
                        entity.MedicationAdministration = txtMedicationAdministration.Text;
                        entity.MedicationPurpose = txtMedicationPurpose.Text;

                        if (ct > 0)
                        {
                            entity.IsRFlag = false;
                            entity.ParentID = parentID;
                        }
                        else
                        {
                            entity.IsRFlag = true;
                            entity.ParentID = null;
                        }

                        if (entity.IsRFlag)
                        {
                            if (cboEmbalace.Value != null)
                            {
                                if (cboEmbalace.Value.ToString() != "" && cboEmbalace.Value.ToString() != "0")
                                {
                                    entity.EmbalaceID = Convert.ToInt32(cboEmbalace.Value);
                                    entity.EmbalaceQty = Convert.ToInt16(txtEmbalaceQty.Text);
                                }
                                else
                                {
                                    entity.EmbalaceID = null;
                                    entity.EmbalaceQty = 0;
                                }
                            }
                            else
                            {
                                entity.EmbalaceID = null;
                                entity.EmbalaceQty = 0;
                            }
                        }
                        else
                        {
                            entity.EmbalaceID = null;
                            entity.EmbalaceQty = 0;
                        }

                        entity.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;

                        #endregion

                        entity.PrescriptionOrderID = prescriptionOrderID;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entity.PrescriptionOrderDetailID = entityOrderDtDao.InsertReturnPrimaryKeyID(entity);

                        if (ct < 1)
                        {
                            parentID = entity.PrescriptionOrderDetailID;
                        }

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
                errMessage = string.Format("{0}<br /><br /><i>{1}</i>",ex.Message, ex.Source);
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