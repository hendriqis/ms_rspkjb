using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class LogisticsReturnQuickPicksCtl : BaseEntryPopupCtl
    {
        private BasePageTrxPatientManagement DetailPage
        {
            get { return (BasePageTrxPatientManagement)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = hdnParam.Value.Split('|');

            hdnVisitID.Value = temp[0];
            hdnLocationID.Value = temp[1];
            hdnTransactionID.Value = temp[2];
            hdnDepartmentID.Value = temp[3];
            hdnHealthcareServiceUnitID.Value = temp[4];
            hdnRegistrationID.Value = temp[5];
            hdnIsAccompany.Value = hdnParam.Value.Split('|')[6];
            hdnLinkedRegistrationID.Value = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value)).LinkedRegistrationID.ToString();

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //1
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1 //2
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsEndingAmountRoundingTo100.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;

            BindCboLocation();
            BindGridView();
        }

        protected void cboLogisticReturnLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
        }

        private void BindCboLocation()
        {
            if (Convert.ToInt32(hdnLocationID.Value) > 0)
            {
                Location loc = BusinessLayer.GetLocation(Convert.ToInt32(hdnLocationID.Value));
                List<Location> lstLocation = null;
                List<Location> lstLocationSelected = new List<Location>();

                if (loc.IsHeader)
                {
                    lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0} AND IsDeleted = 0", loc.LocationID));

                    List<GetLocationUserList> lstLocationByUser = BusinessLayer.GetLocationUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Empty, string.Empty);
                    if (lstLocationByUser.Count > 0)
                    {
                        foreach (GetLocationUserList locUser in lstLocationByUser)
                        {
                            foreach (Location lc in lstLocation)
                            {
                                if (locUser.LocationID == lc.LocationID)
                                {
                                    lstLocationSelected.Add(lc);
                                }
                            }
                        }

                        Methods.SetComboBoxField<Location>(cboLogisticReturnLocation, lstLocationSelected, "LocationName", "LocationID");
                    }
                    else
                    {
                        lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0} AND IsDeleted = 0", loc.LocationID));
                        Methods.SetComboBoxField<Location>(cboLogisticReturnLocation, lstLocation, "LocationName", "LocationID");
                    }

                    cboLogisticReturnLocation.SelectedIndex = 0;
                }
                else
                {
                    lstLocation = new List<Location>();
                    lstLocation.Add(loc);
                    Methods.SetComboBoxField<Location>(cboLogisticReturnLocation, lstLocation, "LocationName", "LocationID");
                    cboLogisticReturnLocation.SelectedIndex = 0;
                }
            }
        }

        protected List<vPatientChargesDt> lstPatientChargesDt = null;

        private string GetFilterExpressionChargeDt()
        {
            string filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
            {
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            }
            else
            {
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            }

            if (rblFilterItemType.SelectedValue == "nutrition")
            {
                filterExpression += string.Format(" AND GCItemType = '{0}'", Constant.ItemType.BAHAN_MAKANAN);
            }
            else
            {
                filterExpression += string.Format(" AND GCItemType = '{0}'", Constant.ItemType.BARANG_UMUM);
            }

            filterExpression += string.Format(" AND PrescriptionOrderID IS NULL AND PrescriptionOrderDetailID IS NULL");
            filterExpression += string.Format(" AND LocationID = {0} AND IsDeleted = 0", cboLogisticReturnLocation.Value);

            return filterExpression;
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpressionChargeDt();
            filterExpression += string.Format(" ORDER BY ItemName1");
            lstPatientChargesDt = BusinessLayer.GetvPatientChargesDtList(filterExpression);
            var tempLst = from bs in lstPatientChargesDt
                          group bs by bs.ItemID into g
                          select new vPatientChargesDt
                          {
                              ItemID = g.Key,
                              ItemName1 = g.First().ItemName1,
                              ItemUnit = g.First().ItemUnit,
                              UsedQuantity = g.Sum(x => x.UsedQuantity)
                          };
            grdView.DataSource = tempLst;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientChargesDt entity = e.Row.DataItem as vPatientChargesDt;
                ASPxComboBox cboChargeClass = e.Row.FindControl("cboChargeClass") as ASPxComboBox;
                var tempLst = from bs in lstPatientChargesDt.Where(p => p.ItemID == entity.ItemID)
                              group bs by bs.ChargeClassID into g
                              select new ClassCare
                              {
                                  ClassID = g.Key,
                                  ClassName = g.First().ChargeClassName
                              };
                Methods.SetComboBoxField<ClassCare>(cboChargeClass, tempLst.ToList(), "ClassName", "ClassID");
                cboChargeClass.SelectedIndex = 0;

                HtmlGenericControl divChargeClassID = e.Row.FindControl("divChargeClassID") as HtmlGenericControl;
                divChargeClassID.InnerHtml = cboChargeClass.Value.ToString();
            }
        }

        protected void cbpViewLogisticReturn_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);

            try
            {
                List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                int visitID = Convert.ToInt32(hdnVisitID.Value);
                string[] lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                string[] lstSelectedMemberChargeClassID = hdnSelectedMemberChargeClassID.Value.Split(',');

                #region Patient Charges Hd
                PatientChargesHd patientChargesHd = null;
                if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
                {
                    patientChargesHd = new PatientChargesHd();
                    patientChargesHd.VisitID = visitID;
                    patientChargesHd.TestOrderID = null;
                    patientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                    if (hdnIsAccompany.Value == "0")
                    {
                        switch (hdnDepartmentID.Value)
                        {
                            case Constant.Facility.INPATIENT: patientChargesHd.TransactionCode = Constant.TransactionCode.IP_CHARGES; break;
                            case Constant.Facility.EMERGENCY: patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                            case Constant.Facility.DIAGNOSTIC:
                                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.RADIOTHERAPHY_CHARGES;
                                else
                                    patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES; break;
                            default: patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
                        }
                    }
                    else
                    {
                        patientChargesHd.TransactionCode = Constant.TransactionCode.IP_PATIENT_ACCOMPANY_CHARGES;
                    }
                    patientChargesHd.TransactionDate = Helper.GetDatePickerValue(DetailPage.GetTransactionDate());
                    patientChargesHd.TransactionTime = DetailPage.GetTransactionTime();
                    patientChargesHd.PatientBillingID = null;
                    patientChargesHd.ReferenceNo = "";
                    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    patientChargesHd.GCVoidReason = null;
                    patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                    patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
                }
                else
                    patientChargesHd = patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));

                retval = patientChargesHd.TransactionNo;
                #endregion

                bool isAllowSaveDt = false;
                if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        isAllowSaveDt = true;
                    }
                    else
                    {
                        isAllowSaveDt = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat ditambah detail item lagi.", patientChargesHd.TransactionNo);
                        result = false;
                    }
                }
                else
                {
                    if (patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        isAllowSaveDt = true;
                    }
                    else
                    {
                        isAllowSaveDt = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat ditambah detail item lagi.", patientChargesHd.TransactionNo);
                        result = false;
                    }
                }

                if (isAllowSaveDt)
                {
                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        #region Patient Charges Dt
                        PatientChargesDt patientChargesDt = new PatientChargesDt();
                        patientChargesDt.ItemID = Convert.ToInt32(itemID);
                        patientChargesDt.ChargeClassID = DetailPage.GetClassID();
                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, Convert.ToInt32(lstSelectedMemberChargeClassID[ct]), Convert.ToInt32(itemID), 2, DateTime.Now, ctx);

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

                        patientChargesDt.BaseTariff = basePrice;
                        patientChargesDt.Tariff = price;
                        patientChargesDt.BaseComp1 = basePriceComp1;
                        patientChargesDt.BaseComp2 = basePriceComp2;
                        patientChargesDt.BaseComp3 = basePriceComp3;
                        patientChargesDt.TariffComp1 = priceComp1;
                        patientChargesDt.TariffComp2 = priceComp2;
                        patientChargesDt.TariffComp3 = priceComp3;
                        patientChargesDt.CostAmount = costAmount;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        ItemMaster entityItemMaster = itemDao.Get(Convert.ToInt32(itemID));
                        patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                        patientChargesDt.ParamedicID = Convert.ToInt32(DetailPage.GetRegistrationPhysicianID());

                        patientChargesDt.IsVariable = false;
                        patientChargesDt.IsUnbilledItem = false;

                        decimal qty = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                        decimal grossLineAmount = qty * price;

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
                                        patientChargesDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                        patientChargesDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                        patientChargesDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                    patientChargesDt.DiscountPercentageComp1 = discountAmount;
                                }

                                if (priceComp2 > 0)
                                {
                                    totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                    patientChargesDt.DiscountPercentageComp2 = discountAmount;
                                }

                                if (priceComp3 > 0)
                                {
                                    totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                    patientChargesDt.DiscountPercentageComp3 = discountAmount;
                                }
                            }

                            if (patientChargesDt.DiscountPercentageComp1 > 0)
                            {
                                patientChargesDt.IsDiscountInPercentageComp1 = true;
                            }

                            if (patientChargesDt.DiscountPercentageComp2 > 0)
                            {
                                patientChargesDt.IsDiscountInPercentageComp2 = true;
                            }

                            if (patientChargesDt.DiscountPercentageComp3 > 0)
                            {
                                patientChargesDt.IsDiscountInPercentageComp3 = true;
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

                        totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (qty);

                        if (totalDiscountAmount != 0 && totalDiscountAmount > grossLineAmount)
                        {
                            totalDiscountAmount = grossLineAmount;
                        }

                        decimal total = grossLineAmount - totalDiscountAmount;
                        decimal totalPayer = 0;
                        if (isCoverageInPercentage)
                        {
                            totalPayer = total * coverageAmount / 100;
                        }
                        else
                        {
                            totalPayer = coverageAmount * qty;
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

                        patientChargesDt.ConversionFactor = 1;

                        ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, patientChargesDt.ItemID), ctx).FirstOrDefault();
                        patientChargesDt.AveragePrice = iPlanning.AveragePrice;
                        patientChargesDt.CostAmount = iPlanning.UnitPrice;

                        patientChargesDt.IsCITO = false;
                        patientChargesDt.CITOAmount = 0;
                        patientChargesDt.IsComplication = false;
                        patientChargesDt.ComplicationAmount = 0;

                        patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                        patientChargesDt.DiscountAmount = totalDiscountAmount;
                        patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                        patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                        patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                        patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = qty * -1;

                        decimal oPatientAmount = (total - totalPayer);
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

                        patientChargesDt.PatientAmount = oPatientAmount * -1;
                        patientChargesDt.PayerAmount = oPayerAmount * -1;
                        patientChargesDt.LineAmount = oLineAmount * -1;

                        patientChargesDt.LocationID = Convert.ToInt32(cboLogisticReturnLocation.Value);
                        patientChargesDt.IsApproved = true;
                        patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientChargesDt.GCTransactionDetailStatus = patientChargesHd.GCTransactionStatus;
                        patientChargesDt.TransactionID = patientChargesHd.TransactionID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        string filterExpression = GetFilterExpressionChargeDt();
                        filterExpression += string.Format(" AND ItemID = {0}", patientChargesDt.ItemID);
                        List<vPatientChargesDt> lstPatientChargesDtChecked = BusinessLayer.GetvPatientChargesDtList(filterExpression, ctx);
                        decimal recommendQtyReturn = lstPatientChargesDtChecked.Sum(a => a.ChargedQuantity);

                        if (qty <= recommendQtyReturn)
                        {
                            patientChargesDtDao.Insert(patientChargesDt);
                            ct++;
                        }
                        else
                        {
                            errMessage = string.Format("Transaksi item {0} [{1}] tidak dapat dibuatkan retur sejumlah {2} karena sudah melebihi jumlah rekomendasi retur sejumlah {3}.",
                                                            entityItemMaster.ItemName1, entityItemMaster.ItemCode, qty.ToString(Constant.FormatString.NUMERIC_2), recommendQtyReturn.ToString(Constant.FormatString.NUMERIC_2));
                            result = false;
                            break;
                        }
                        #endregion
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
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
            return result;
        }
    }
}