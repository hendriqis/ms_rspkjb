using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class PrescriptionNonMasterEntryCtl : BaseEntryPopupCtl
    {
        private PrescriptionEntryDetail DetailPage
        {
            get { return (PrescriptionEntryDetail)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            if (temp[0] == "add")
            {
                hdnTransactionID.Value = temp[1];
                hdnLocationID.Value = temp[2];
                hdnDefaultGCMedicationRoute.Value = temp[3];
                hdnParamedicID.Value = temp[4];
                hdnRegistrationID.Value = temp[5];
                hdnVisitID.Value = temp[6];
                hdnChargeClassID.Value = temp[7];
                SetControlProperties();
                IsAdd = true;
            }
            else
            {
                hdnPrescriptionDtID.Value = temp[1];
                hdnLocationID.Value = temp[2];
                SetControlProperties();
                PrescriptionOrderDt entityDt = BusinessLayer.GetPrescriptionOrderDt(Convert.ToInt32(hdnPrescriptionDtID.Value));
                PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDtList(String.Format("PrescriptionOrderDetailID = {0}", hdnPrescriptionDtID.Value))[0];
                EntityToControl(entityChargesDt);
                IsAdd = false;
            }
        }

        private void EntityToControl(PatientChargesDt entityChargesDt)
        {
            cboNonMasterLocation.Value = entityChargesDt.LocationID.ToString();
            txtDrugName.Text = entityChargesDt.ItemName;
            cboNonMasterChargeClassID.Value = entityChargesDt.ChargeClassID.ToString();
            txtNonMasterUnitTariff.Text = entityChargesDt.BaseTariff.ToString();
            txtNonMasterQtyUsed.Text = entityChargesDt.UsedQuantity.ToString();
            txtNonMasterQtyCharged.Text = entityChargesDt.ChargedQuantity.ToString();
            cboNonMasterUoM.Value = entityChargesDt.GCItemUnit.ToString();
            txtNonMasterPriceDiscount.Text = entityChargesDt.DiscountAmount.ToString();
            txtNonMasterPatient.Text = entityChargesDt.PatientAmount.ToString();
            txtNonMasterPayer.Text = entityChargesDt.PayerAmount.ToString();
            txtNonMasterTotal.Text = entityChargesDt.LineAmount.ToString();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboNonMasterLocation, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDrugName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboNonMasterChargeClassID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNonMasterUnitTariff, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtNonMasterQtyUsed, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtNonMasterQtyCharged, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(cboNonMasterUoM, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNonMasterPriceDiscount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtNonMasterPatient, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtNonMasterPayer, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtNonMasterTotal, new ControlEntrySetting(false, false, false, 0));
        }

        private void SetControlProperties()
        {
            hdnItemID.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.NON_MASTER_ITEM).ParameterValue;

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //1
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1 //2
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsEndingAmountRoundingTo100.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<ClassCare>(cboNonMasterChargeClassID, lstClassCare, "ClassName", "ClassID");
            cboNonMasterChargeClassID.Value = hdnChargeClassID.Value;

            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ITEM_UNIT));
            Methods.SetComboBoxField<StandardCode>(cboNonMasterUoM, lst, "StandardCodeName", "StandardCodeID");
            cboNonMasterUoM.SelectedIndex = -1;

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
                Methods.SetComboBoxField<Location>(cboNonMasterLocation, lstLocation, "LocationName", "LocationID");
                cboNonMasterLocation.SelectedIndex = 0;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PrescriptionOrderDtDao entityOrderDtDao = new PrescriptionOrderDtDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            try
            {
                int PrescriptionID = 0, PrescriptionOrderDtID = 0;
                int TransactionID = 0;
                String TransactionNo = "";
                DetailPage.SavePrescriptionHd(ctx, ref PrescriptionID, ref TransactionID, ref TransactionNo);
                PatientChargesHd entityHd = patientChargesHdDao.Get(TransactionID);
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    PrescriptionOrderDt entity = new PrescriptionOrderDt();
                    #region PrescriptionOrderDt
                    entity.IsRFlag = false;
                    entity.ItemID = Convert.ToInt32(hdnItemID.Value);
                    entity.DrugName = txtDrugName.Text;
                    entity.GenericName = "";
                    entity.GCDrugForm = null;
                    entity.SignaID = null;
                    entity.Dose = null;
                    entity.GCDoseUnit = null;
                    entity.GCDosingFrequency = Constant.DosingFrequency.DAY;
                    entity.Frequency = 0;
                    entity.NumberOfDosage = 0;
                    entity.GCDosingUnit = cboNonMasterUoM.Value.ToString();
                    entity.GCRoute = hdnDefaultGCMedicationRoute.Value.ToString();
                    entity.StartDate = DateTime.Now;
                    entity.StartTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    entity.MedicationPurpose = "";
                    entity.MedicationAdministration = "";
                    entity.DosingDuration = 0;
                    entity.TakenQty = entity.DispenseQty = Convert.ToDecimal(txtNonMasterQtyUsed.Text);
                    entity.EmbalaceID = null;
                    entity.EmbalaceQty = 0;
                    entity.GCCoenamRule = null;
                    entity.GCPrescriptionOrderStatus = Constant.OrderStatus.IN_PROGRESS;

                    #endregion

                    #region PatientChargesDt
                    PatientChargesDt entityChargesDt = new PatientChargesDt();
                    entityChargesDt.LocationID = Convert.ToInt32(cboNonMasterLocation.Value);
                    entityChargesDt.ItemID = (int)entity.ItemID;
                    entityChargesDt.ItemName = entity.DrugName;
                    entityChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                    entityChargesDt.ChargeClassID = Convert.ToInt32(cboNonMasterChargeClassID.Value);

                    decimal basePrice = Convert.ToDecimal(txtNonMasterUnitTariff.Text);
                    entityChargesDt.BaseComp1 = entityChargesDt.BaseTariff = basePrice;
                    entityChargesDt.BaseComp2 = entityChargesDt.BaseComp3 = 0;
                    entityChargesDt.TariffComp1 = entityChargesDt.Tariff = basePrice;
                    entityChargesDt.TariffComp2 = entityChargesDt.TariffComp3 = 0;

                    entityChargesDt.BaseQuantity = entityChargesDt.UsedQuantity = entity.TakenQty;
                    entityChargesDt.ChargedQuantity = Convert.ToDecimal(txtNonMasterQtyCharged.Text);
                    entityChargesDt.GCBaseUnit = entityChargesDt.GCItemUnit = entity.GCDosingUnit;
                    entityChargesDt.ConversionFactor = 1;
                    entityChargesDt.EmbalaceAmount = 0;
                    entityChargesDt.PrescriptionFeeAmount = 0;

                    entityChargesDt.DiscountComp1 = Convert.ToDecimal(txtNonMasterPriceDiscount.Text);
                    entityChargesDt.DiscountComp2 = entityChargesDt.DiscountComp3 = 0;
                    entityChargesDt.DiscountAmount = entityChargesDt.DiscountComp1 * entityChargesDt.ChargedQuantity;

                    decimal oPatientAmount = Convert.ToDecimal(txtNonMasterPatient.Text);
                    decimal oPayerAmount = Convert.ToDecimal(txtNonMasterPayer.Text);
                    decimal oLineAmount = entityChargesDt.PatientAmount + entityChargesDt.PayerAmount;

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
                        if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
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
                    entityChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                    #endregion

                    entity.PrescriptionOrderID = PrescriptionID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    PrescriptionOrderDtID = entityOrderDtDao.InsertReturnPrimaryKeyID(entity);

                    entityChargesDt.PrescriptionOrderDetailID = PrescriptionOrderDtID;
                    entityChargesDt.TransactionID = TransactionID;
                    entityChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityChargesDtDao.Insert(entityChargesDt);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi " + entityHd.TransactionNo + " Sudah Diproses. Tidak Bisa Ditambahkan Item";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                retval = TransactionID.ToString();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            PatientChargesDtDao entityChargesDao = new PatientChargesDtDao(ctx);
            try
            {
                int PrescriptionID = 0;
                int TransactionID = 0;
                String TransactionNo = "";
                DetailPage.SavePrescriptionHd(ctx, ref PrescriptionID, ref TransactionID, ref TransactionNo);
                PatientChargesHd entityHd = patientChargesHdDao.Get(TransactionID);
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    PrescriptionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnPrescriptionDtID.Value));
                    PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDtList(String.Format("PrescriptionOrderDetailID = {0}", hdnPrescriptionDtID.Value), ctx)[0];

                    entityDt.DrugName = entityChargesDt.ItemName = txtDrugName.Text;
                    entityChargesDt.LocationID = Convert.ToInt32(cboNonMasterLocation.Value);
                    entityChargesDt.ChargeClassID = Convert.ToInt32(cboNonMasterChargeClassID.Value);
                    entityChargesDt.GCBaseUnit = entityChargesDt.GCItemUnit = entityDt.GCDosingUnit = cboNonMasterUoM.Value.ToString();

                    decimal basePrice = Convert.ToDecimal(txtNonMasterUnitTariff.Text);
                    entityChargesDt.BaseComp1 = entityChargesDt.BaseTariff = basePrice;
                    entityChargesDt.TariffComp1 = entityChargesDt.Tariff = basePrice;

                    entityChargesDt.BaseQuantity = entityChargesDt.UsedQuantity = entityDt.TakenQty = Convert.ToDecimal(txtNonMasterQtyUsed.Text);
                    entityChargesDt.ChargedQuantity = Convert.ToDecimal(txtNonMasterQtyCharged.Text);
                    entityChargesDt.ConversionFactor = 1;

                    entityChargesDt.DiscountComp1 = Convert.ToDecimal(txtNonMasterPriceDiscount.Text);
                    entityChargesDt.DiscountComp2 = entityChargesDt.DiscountComp3 = 0;
                    entityChargesDt.DiscountAmount = entityChargesDt.DiscountComp1 * entityChargesDt.ChargedQuantity;

                    decimal oPatientAmount = Convert.ToDecimal(txtNonMasterPatient.Text);
                    decimal oPayerAmount = Convert.ToDecimal(txtNonMasterPayer.Text);
                    decimal oLineAmount = entityChargesDt.PatientAmount + entityChargesDt.PayerAmount;

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
                        if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
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

                    entityChargesDt.LastUpdatedBy = entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    entityChargesDao.Update(entityChargesDt);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi " + entityHd.TransactionNo + " Sudah Diproses. Tidak Bisa Ditambahkan Item";
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