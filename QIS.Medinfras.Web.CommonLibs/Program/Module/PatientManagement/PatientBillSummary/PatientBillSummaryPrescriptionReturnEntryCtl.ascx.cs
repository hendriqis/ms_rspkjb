using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryPrescriptionReturnEntryCtl : BaseEntryPopupCtl
    {
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
            hdnChargeClassID.Value = temp[6];
            hdnPhysicianID.Value = temp[7];
            hdnTransactionDate.Value = temp[8];
            hdnTransactionTime.Value = temp[9];
            hdnReturnType.Value = temp[10];
            hdnIsCorrectionTransaction.Value = temp[11];
            hdnLinkedRegistrationID.Value = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value)).LinkedRegistrationID.ToString();

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //1
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsEndingAmountRoundingTo100.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;
            
            BindCboLocation();
            BindGridView();
        }

        protected void cboReturnLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                Methods.SetComboBoxField<Location>(cboReturnLocation, lstLocation, "LocationName", "LocationID");
                cboReturnLocation.SelectedIndex = 0;
            }
        }

        protected List<GetPrescriptionReturnOrderRemainingQty> lstEntity = null;
        private void BindGridView()
        {
            //string filterExpression = "";
            //if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
            //    filterExpression = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value);
            //else
            //    filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            //filterExpression += string.Format(" AND LocationID = {0} AND IsDeleted = 0 ORDER BY ItemName1", hdnLocationID.Value);

            //lstEntity = BusinessLayer.GetvPrescriptionReturnOrderRemainingQtyList(filterExpression);

            lstEntity = BusinessLayer.GetPrescriptionReturnOrderRemainingQtyList(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnLocationID.Value));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //GetPrescriptionReturnOrderRemainingQty entity = e.Row.DataItem as GetPrescriptionReturnOrderRemainingQty;
                //ASPxComboBox cboChargeClass = e.Row.FindControl("cboChargeClass") as ASPxComboBox;
                //var tempLst = from bs in lstEntity.Where(p => p.ItemID == entity.ItemID)
                //              group bs by bs.ChargeClassID into g
                //              select new ClassCare
                //              {
                //                  ClassID = g.Key,
                //                  ClassName = g.First().ChargeClassName
                //              };
                //Methods.SetComboBoxField<ClassCare>(cboChargeClass, tempLst.ToList(), "ClassName", "ClassID");
                //cboChargeClass.SelectedIndex = entity.ChargeClassID;

                //HtmlGenericControl divChargeClassID = e.Row.FindControl("divChargeClassID") as HtmlGenericControl;
                //divChargeClassID.InnerHtml = cboChargeClass.Value.ToString();
            }
        }

        protected void cbpViewDrugMSReturn_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PrescriptionReturnOrderDtDao returnOrderDtDao = new PrescriptionReturnOrderDtDao(ctx);
            PrescriptionReturnOrderHdDao returnOrderHdDao = new PrescriptionReturnOrderHdDao(ctx);

            try
            {
                List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
                List<PrescriptionReturnOrderDt> lstReturnOrderDt = new List<PrescriptionReturnOrderDt>();
                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                int visitID = Convert.ToInt32(hdnVisitID.Value);
                string[] lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberItemID = hdnSelectedMemberItemID.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                string[] lstSelectedMemberChargeClassID = hdnSelectedMemberChargeClassID.Value.Split(',');
                string[] lstSelectedMemberGCItemUnit = hdnSelectedMemberGCItemUnit.Value.Split(',');

                List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, hdnSelectedMemberItemID.Value));
                int ct = 0;
                foreach (String chargesDtID in lstSelectedMember)
                {
                        //Get transaction original values
                        PatientChargesDt oldChargesDt = patientChargesDtDao.Get(Convert.ToInt32(chargesDtID));
                        if (oldChargesDt != null)
                        {
                            #region PrescriptionReturnOrderDt
                            PrescriptionReturnOrderDt returnOrderDt = new PrescriptionReturnOrderDt();
                            returnOrderDt.PatientChargesDtID = Convert.ToInt32(chargesDtID);
                            returnOrderDt.ItemID = Convert.ToInt32(lstSelectedMemberItemID[ct]);
                            returnOrderDt.ItemQty = Convert.ToDecimal(lstSelectedMemberQty[ct]) * -1;
                            returnOrderDt.GCItemUnit = lstSelectedMemberGCItemUnit[ct];
                            returnOrderDt.IsCreatedFromOrder = false;
                            returnOrderDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                            returnOrderDt.IsDeleted = false;
                            returnOrderDt.LastUpdatedBy = returnOrderDt.CreatedBy = AppSession.UserLogin.UserID;
                            returnOrderDt.LastUpdatedDate = returnOrderDt.CreatedDate = DateTime.Now;
                            lstReturnOrderDt.Add(returnOrderDt);
                            #endregion

                            #region PatientChargesDt
                            PatientChargesDt patientChargesDt = new PatientChargesDt();
                            patientChargesDt.ItemID = returnOrderDt.ItemID;
                            patientChargesDt.ChargeClassID = Convert.ToInt32(lstSelectedMemberChargeClassID[ct]);
                            patientChargesDt.PrescriptionOrderDetailID = null;

                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, Convert.ToInt32(lstSelectedMemberChargeClassID[ct]), patientChargesDt.ItemID, 2, DateTime.Now, ctx);

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

                            decimal persentaseRetur = 0;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            string filterSetVarDt = string.Format("HealthcareID = '{0}' AND ParameterCode = '{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_PERSENTASE_RETUR_RESEP);
                            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVarDt, ctx);
                            if (lstSetVarDt.Count() > 0)
                            {
                                if (lstSetVarDt.FirstOrDefault().ParameterValue != "" && lstSetVarDt.FirstOrDefault().ParameterValue != null)
                                {
                                    persentaseRetur = Convert.ToDecimal(lstSetVarDt.FirstOrDefault().ParameterValue);
                                }
                            }

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            patientChargesDt.BaseTariff = oldChargesDt.BaseTariff;
                            patientChargesDt.BaseComp1 = oldChargesDt.BaseComp1;
                            patientChargesDt.BaseComp2 = oldChargesDt.BaseComp2;
                            patientChargesDt.BaseComp3 = oldChargesDt.BaseComp3;
                            patientChargesDt.Tariff = oldChargesDt.Tariff;
                            patientChargesDt.Tariff = oldChargesDt.Tariff * persentaseRetur / 100;
                            patientChargesDt.TariffComp1 = oldChargesDt.TariffComp1 * persentaseRetur / 100;
                            patientChargesDt.TariffComp2 = oldChargesDt.TariffComp2 * persentaseRetur / 100;
                            patientChargesDt.TariffComp3 = oldChargesDt.TariffComp3 * persentaseRetur / 100;
                            patientChargesDt.GCBaseUnit = oldChargesDt.GCBaseUnit;
                            patientChargesDt.GCItemUnit = oldChargesDt.GCItemUnit;

                            patientChargesDt.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                            patientChargesDt.IsVariable = false;
                            patientChargesDt.IsUnbilledItem = false;

                            decimal qty = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                            decimal totalDiscountAmount = qty * ((oldChargesDt.DiscountAmount * persentaseRetur / 100) / oldChargesDt.ChargedQuantity);
                            decimal grossLineAmount = qty * patientChargesDt.Tariff;

                            decimal total = grossLineAmount - totalDiscountAmount;
                            decimal totalPayer = 0;

                            if (isCoverageInPercentage)
                                totalPayer = total * coverageAmount / 100;
                            else
                                totalPayer = coverageAmount * qty;

                            if (total > 0 && totalPayer > total)
                                totalPayer = total;

                            patientChargesDt.ConversionFactor = 1;
                            patientChargesDt.AveragePrice = lstItemPlanning.FirstOrDefault(p => p.ItemID == patientChargesDt.ItemID).AveragePrice;
                            patientChargesDt.IsCITO = false;
                            patientChargesDt.CITOAmount = 0;
                            patientChargesDt.IsComplication = false;
                            patientChargesDt.ComplicationAmount = 0;

                            patientChargesDt.IsDiscount = oldChargesDt.IsDiscount;
                            patientChargesDt.DiscountAmount = totalDiscountAmount * persentaseRetur / 100;
                            patientChargesDt.DiscountComp1 = oldChargesDt.DiscountComp1 * persentaseRetur / 100;
                            patientChargesDt.DiscountComp2 = oldChargesDt.DiscountComp2 * persentaseRetur / 100;
                            patientChargesDt.DiscountComp3 = oldChargesDt.DiscountComp3 * persentaseRetur / 100;
                            patientChargesDt.IsDiscountInPercentageComp1 = oldChargesDt.IsDiscountInPercentageComp1;
                            patientChargesDt.IsDiscountInPercentageComp2 = oldChargesDt.IsDiscountInPercentageComp2;
                            patientChargesDt.IsDiscountInPercentageComp3 = oldChargesDt.IsDiscountInPercentageComp3;
                            patientChargesDt.DiscountPercentageComp1 = oldChargesDt.DiscountPercentageComp1;
                            patientChargesDt.DiscountPercentageComp2 = oldChargesDt.DiscountPercentageComp2;
                            patientChargesDt.DiscountPercentageComp3 = oldChargesDt.DiscountPercentageComp3;

                            patientChargesDt.ReturnChargesTariffPercent = persentaseRetur;

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

                            patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                            patientChargesDt.LocationID = Convert.ToInt32(cboReturnLocation.Value);
                            patientChargesDt.IsApproved = false;
                            patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            lstPatientChargesDt.Add(patientChargesDt);
                            #endregion
                        }
                    ct++;
                }

                #region HD
                PatientChargesHd patientChargesHd = null;
                PrescriptionReturnOrderHd returnOrderHd = null; 
                if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
                {
                    #region PrescriptionReturnOrderHd
                    returnOrderHd = new PrescriptionReturnOrderHd();
                    returnOrderHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_RETURN_ORDER;
                    returnOrderHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                    returnOrderHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    returnOrderHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    returnOrderHd.FromVisitID = Convert.ToInt32(hdnVisitID.Value);
                    returnOrderHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                    returnOrderHd.OrderDate = Helper.GetDatePickerValue(hdnTransactionDate.Value);
                    returnOrderHd.OrderTime = hdnTransactionTime.Value;
                    returnOrderHd.IsCreatedBySystem = true;
                    returnOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                    returnOrderHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                    returnOrderHd.GCPrescriptionReturnType = hdnReturnType.Value;
                    returnOrderHd.PrescriptionReturnOrderNo = BusinessLayer.GenerateTransactionNo(returnOrderHd.TransactionCode, returnOrderHd.OrderDate, ctx);
                    returnOrderHd.LastUpdatedBy = returnOrderHd.CreatedBy = AppSession.UserLogin.UserID;
                    returnOrderHd.LastUpdatedDate = returnOrderHd.CreatedDate = DateTime.Now;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    returnOrderHdDao.Insert(returnOrderHd);
                    returnOrderHd.PrescriptionReturnOrderID = BusinessLayer.GetPrescriptionReturnOrderHdMaxID(ctx);
                    #endregion

                    #region PatientChargesHd
                    patientChargesHd = new PatientChargesHd();
                    patientChargesHd.VisitID = visitID;
                    patientChargesHd.TestOrderID = null;
                    patientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                    patientChargesHd.TransactionCode = Constant.TransactionCode.PH_CHARGES;
                    patientChargesHd.TransactionDate = Helper.GetDatePickerValue(hdnTransactionDate.Value);
                    patientChargesHd.TransactionTime = hdnTransactionTime.Value;
                    patientChargesHd.PatientBillingID = null;
                    patientChargesHd.ReferenceNo = "";
                    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    patientChargesHd.IsCorrectionTransaction = hdnIsCorrectionTransaction.Value == "1" ? true : false;
                    patientChargesHd.GCVoidReason = null;
                    patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                    patientChargesHd.PrescriptionReturnOrderID = returnOrderHd.PrescriptionReturnOrderID;
                    patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
                    #endregion
                }
                else 
                {
                    patientChargesHd = patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                    returnOrderHd = returnOrderHdDao.Get(Convert.ToInt32(patientChargesHd.PrescriptionReturnOrderID));
                }                    

                retval = patientChargesHd.TransactionID.ToString() + "|" + patientChargesHd.TransactionNo;
                if (AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        for (int ctr = 0; ctr < lstPatientChargesDt.Count(); ctr++)
                        {
                            lstReturnOrderDt[ctr].PrescriptionReturnOrderID = returnOrderHd.PrescriptionReturnOrderID;
                            returnOrderDtDao.Insert(lstReturnOrderDt[ctr]);

                            lstPatientChargesDt[ctr].PrescriptionReturnOrderDtID = BusinessLayer.GetPrescriptionReturnOrderDtMaxID(ctx);
                            lstPatientChargesDt[ctr].TransactionID = patientChargesHd.TransactionID;
                            patientChargesDtDao.Insert(lstPatientChargesDt[ctr]);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                        result = false;
                    }
                }
                else
                {
                    if (patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        for (int ctr = 0; ctr < lstPatientChargesDt.Count(); ctr++)
                        {
                            lstReturnOrderDt[ctr].PrescriptionReturnOrderID = returnOrderHd.PrescriptionReturnOrderID;
                            returnOrderDtDao.Insert(lstReturnOrderDt[ctr]);

                            lstPatientChargesDt[ctr].PrescriptionReturnOrderDtID = BusinessLayer.GetPrescriptionReturnOrderDtMaxID(ctx);
                            lstPatientChargesDt[ctr].TransactionID = patientChargesHd.TransactionID;
                            patientChargesDtDao.Insert(lstPatientChargesDt[ctr]);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                        result = false;
                    }
                }
                #endregion

                ctx.CommitTransaction();
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