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

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrescriptionReturnEntryCtl : BaseEntryPopupCtl
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
            hdnPreviousRegistrationID.Value = temp[5];
            hdnChargeClassID.Value = temp[6];
            hdnPhysicianID.Value = temp[7];
            hdnTransactionDate.Value = temp[8];
            hdnTransactionTime.Value = temp[9];
            hdnReturnType.Value = temp[10];

            hdnMRN.Value = temp[11];
            hdnGuestID.Value = temp[12];

            tblRegistrationNo.Visible = AppSession.RegisteredPatient.DepartmentID != Constant.Facility.INPATIENT;

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //1
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1 //2
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsEndingAmountRoundingTo100.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;

            BindGridView();
        }

        protected string OnGetRegistrationNoFilterExpression()
        {
            string filterExpression = string.Format("(MRN = '{0}' OR GuestID = '{1}')", hdnMRN.Value, hdnGuestID.Value);
            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                Registration entityReg = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
                filterExpression += string.Format(" AND (RegistrationID = {0} OR RegistrationID = {1})", hdnRegistrationID.Value, entityReg.LinkedRegistrationID != null ? entityReg.LinkedRegistrationID : 0);
            }
            return filterExpression;
        }

        protected List<GetPrescriptionReturnOrderRemainingQty> lstEntity = null;
        private void BindGridView()
        {
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            if (!string.IsNullOrEmpty(hdnPreviousRegistrationID.Value))
            {
                registrationID = Convert.ToInt32(hdnPreviousRegistrationID.Value);
            }
            lstEntity = BusinessLayer.GetPrescriptionReturnOrderRemainingQtyList(registrationID, Convert.ToInt32(hdnLocationID.Value));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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
                PatientChargesHd patientChargesHd = null;
                if (hdnTransactionID.Value != null && hdnTransactionID.Value != "0" && hdnTransactionID.Value != "")
                {
                    patientChargesHd = patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                }

                if (patientChargesHd == null || (patientChargesHd != null && patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN))
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

                    //List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, hdnSelectedMemberItemID.Value));
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
                            patientChargesDt.ItemID = Convert.ToInt32(returnOrderDt.ItemID);
                            patientChargesDt.ChargeClassID = oldChargesDt.ChargeClassID;
                            patientChargesDt.PrescriptionOrderDetailID = oldChargesDt.PrescriptionOrderDetailID;

                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, Convert.ToInt32(hdnChargeClassID.Value), patientChargesDt.ItemID, 2, oldChargesDt.CreatedDate, ctx);
                            decimal discountAmount = 0;
                            decimal coverageAmount = 0;
                            decimal price = 0;
                            decimal basePrice = 0;
                            bool isCoverageInPercentage = false;
                            bool isDiscountInPercentage = false;
                            if (list.Count > 0)
                            {
                                GetCurrentItemTariff obj = list[0];
                                discountAmount = obj.DiscountAmount;
                                coverageAmount = obj.CoverageAmount;
                                price = obj.Price;
                                basePrice = obj.BasePrice;
                                isCoverageInPercentage = obj.IsCoverageInPercentage;
                                isDiscountInPercentage = obj.IsDiscountInPercentage;
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
                            patientChargesDt.Tariff = oldChargesDt.Tariff * persentaseRetur / 100;
                            patientChargesDt.TariffComp1 = oldChargesDt.TariffComp1 * persentaseRetur / 100;
                            patientChargesDt.TariffComp2 = oldChargesDt.TariffComp2 * persentaseRetur / 100;
                            patientChargesDt.TariffComp3 = oldChargesDt.TariffComp3 * persentaseRetur / 100;
                            patientChargesDt.GCBaseUnit = oldChargesDt.GCBaseUnit;
                            patientChargesDt.GCItemUnit = oldChargesDt.GCItemUnit;
                            
                            patientChargesDt.ParamedicID = Convert.ToInt32(oldChargesDt.ParamedicID);
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
                            patientChargesDt.CostAmount = oldChargesDt.CostAmount;
                            patientChargesDt.AveragePrice = oldChargesDt.AveragePrice;
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

                            patientChargesDt.LocationID = Convert.ToInt32(hdnLocationID.Value);
                            patientChargesDt.IsApproved = false;
                            patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                            lstPatientChargesDt.Add(patientChargesDt);
                            #endregion
                        }
                        ct++;
                    }

                    PrescriptionReturnOrderHd returnOrderHd = null;
                    if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
                    {
                        #region PrescriptionReturnOrderHd
                        returnOrderHd = new PrescriptionReturnOrderHd();
                        returnOrderHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_RETURN_ORDER;
                        returnOrderHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                        returnOrderHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                        returnOrderHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                        if (!string.IsNullOrEmpty(hdnPreviousVisitID.Value))
                            returnOrderHd.FromVisitID = Convert.ToInt32(hdnPreviousVisitID.Value);
                        else
                            returnOrderHd.FromVisitID = Convert.ToInt32(hdnVisitID.Value);
                        returnOrderHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                        returnOrderHd.OrderDate = Helper.GetDatePickerValue(hdnTransactionDate.Value);
                        returnOrderHd.OrderTime = hdnTransactionTime.Value;
                        returnOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        returnOrderHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                        returnOrderHd.IsCreatedBySystem = true;
                        returnOrderHd.GCPrescriptionReturnType = hdnReturnType.Value;
                        returnOrderHd.PrescriptionReturnOrderNo = BusinessLayer.GenerateTransactionNo(returnOrderHd.TransactionCode, returnOrderHd.OrderDate, ctx);
                        returnOrderHd.LastUpdatedBy = returnOrderHd.CreatedBy = AppSession.UserLogin.UserID;
                        returnOrderHd.LastUpdatedDate = returnOrderHd.CreatedDate = DateTime.Now;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        int oPrescriptionReturnID = returnOrderHdDao.InsertReturnPrimaryKeyID(returnOrderHd);
                        returnOrderHd.PrescriptionReturnOrderID = oPrescriptionReturnID;
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
                        patientChargesHd.GCVoidReason = null;
                        patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                        patientChargesHd.PrescriptionReturnOrderID = oPrescriptionReturnID;
                        patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
                        #endregion
                    }
                    else
                    {
                        returnOrderHd = returnOrderHdDao.Get(Convert.ToInt32(patientChargesHd.PrescriptionReturnOrderID));
                    }

                    retval = patientChargesHd.TransactionNo;
                    for (int ctr = 0; ctr < lstPatientChargesDt.Count(); ctr++)
                    {
                        lstReturnOrderDt[ctr].PrescriptionReturnOrderID = returnOrderHd.PrescriptionReturnOrderID;
                        returnOrderDtDao.Insert(lstReturnOrderDt[ctr]);

                        lstPatientChargesDt[ctr].PrescriptionReturnOrderDtID = BusinessLayer.GetPrescriptionReturnOrderDtMaxID(ctx);
                        lstPatientChargesDt[ctr].TransactionID = patientChargesHd.TransactionID;
                        patientChargesDtDao.Insert(lstPatientChargesDt[ctr]);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", patientChargesHd.TransactionNo);
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