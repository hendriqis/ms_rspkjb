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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class BedChargesProcess : BasePageTrx
    {
        String filterExpression = "";
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.MANUAL_BED_CHARGES;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
            hdnParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            Registration reg = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
            hdnIsJobBedClosed.Value = reg.IsJobBedClosed ? "1" : "0";
            hdnIsJobBedReopen.Value = reg.IsJobBedReopen ? "1" : "0";

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                                                             AppSession.UserLogin.HealthcareID,
                                                             Constant.SettingParameter.IP_BED_CHARGES_TIME_ROUNDING,
                                                             Constant.SettingParameter.IP_BED_CHARGES_TYPE_DATE,
                                                             Constant.SettingParameter.IP_BED_CHARGES_IN_DAY,
                                                             Constant.SettingParameter.IP_BED_CHARGES_HEALTHCARESERVICEUNIT));

            hdnBedChargesTimeRounding.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_BED_CHARGES_TIME_ROUNDING).FirstOrDefault().ParameterValue;
            hdnBedChargesTypeDate.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_BED_CHARGES_TYPE_DATE).FirstOrDefault().ParameterValue;
            hdnBedChargesInDay.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_BED_CHARGES_IN_DAY).FirstOrDefault().ParameterValue;
            hdnBedChargesHealthcareServiceUnit.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_BED_CHARGES_HEALTHCARESERVICEUNIT).FirstOrDefault().ParameterValue;

            lblBedChargesTimeRounding.InnerHtml = GetLabel(string.Format("Pembulatan {0}jam di akhir, akan terhitung pada saat klik 'Close Job Bed' dan baris terakhir di transaksi tempat tidur ini belum diproses nomor transaksi pasiennya.", hdnBedChargesTimeRounding.Value));

            BindGridDetail();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            if (param[0] == "save")
            {
                if (SaveCharges(ref param[1], ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else
            {
                BindGridDetail();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridDetail()
        {
            filterExpression = string.Format("RegistrationID = {0} AND IsDeleted = 0 ORDER BY ID ASC", AppSession.RegisteredPatient.RegistrationID);
            List<vRegistrationBedCharges> lst = BusinessLayer.GetvRegistrationBedChargesList(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vRegistrationBedCharges entity = e.Item.DataItem as vRegistrationBedCharges;

                HtmlInputButton btnSave = e.Item.FindControl("btnSave") as HtmlInputButton;

                if (entity.TransactionID != null && entity.TransactionID != 0)
                {
                    btnSave.Attributes.Add("enabled", "false");
                    //btnSave.Attributes.Add("style", "display-none");
                }
                else
                {
                    btnSave.Attributes.Remove("enabled");
                    //btnSave.Attributes.Remove("style");
                }
            }
        }

        private bool SaveCharges(ref string param, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao chargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
            RegistrationBedChargesDao bedChargesDao = new RegistrationBedChargesDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);

            try
            {
                RegistrationBedCharges bedCharges = bedChargesDao.Get(Convert.ToInt32(param));
                if (bedCharges != null)
                {
                    if (bedCharges.TransactionID == null)
                    {
                        int transactionID = 0;
                        int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                        int visitID = Convert.ToInt32(hdnVisitID.Value);
                        int healthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);

                        Registration oReg = registrationDao.Get(registrationID);

                        ItemMaster itemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID IN ({0})", Convert.ToInt32(bedCharges.ItemID)), ctx).FirstOrDefault();
                        if (itemMaster.GCItemStatus == Constant.ItemStatus.IN_ACTIVE || itemMaster.IsDeleted)
                        {
                            result = false;
                            errMessage = "Transaksi tidak dapat diproses karena item sudah tidak aktif / dihapus";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        else
                        {
                            #region PatientChargesHd

                            PatientChargesHd entityHd = new PatientChargesHd();
                            entityHd.VisitID = visitID;
                            if (Convert.ToInt32(hdnBedChargesHealthcareServiceUnit.Value) == 1)
                            {
                                entityHd.HealthcareServiceUnitID = healthcareServiceUnitID;
                            }
                            else if (Convert.ToInt32(hdnBedChargesHealthcareServiceUnit.Value) == 2)
                            {
                                entityHd.HealthcareServiceUnitID = bedCharges.ToHealthcareServiceUnitID;
                            }
                            entityHd.TransactionCode = Constant.TransactionCode.IP_CHARGES;
                            if (Convert.ToInt32(hdnBedChargesTypeDate.Value) == 1)
                            {
                                entityHd.TransactionDate = DateTime.Now;
                                entityHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            }
                            else if (Convert.ToInt32(hdnBedChargesTypeDate.Value) == 2)
                            {
                                entityHd.TransactionDate = bedCharges.ToDateTime;
                                entityHd.TransactionTime = bedCharges.ToDateTime.ToString(Constant.FormatString.TIME_FORMAT);
                            }
                            entityHd.PatientBillingID = null;
                            entityHd.Remarks = "Tarikan manual dari proses Transaksi Tempat Tidur.";
                            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            entityHd.GCVoidReason = null;
                            entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TransactionDate, ctx);
                            entityHd.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            transactionID = chargesHdDao.InsertReturnPrimaryKeyID(entityHd);

                            #endregion

                            #region PatientChargesDt

                            PatientChargesDt patientChargesDt = new PatientChargesDt();
                            patientChargesDt.ItemID = Convert.ToInt32(bedCharges.ItemID);
                            patientChargesDt.ChargeClassID = bedCharges.ToChargeClassID;

                            decimal discountAmount = 0;
                            decimal coverageAmount = 0;
                            decimal basePrice = 0;
                            decimal basePriceComp1 = 0;
                            decimal basePriceComp2 = 0;
                            decimal basePriceComp3 = 0;
                            decimal price = 0;
                            decimal priceComp1 = 0;
                            decimal priceComp2 = 0;
                            decimal priceComp3 = 0;
                            decimal costAmount = 0;
                            decimal numberOfHourInDay = Convert.ToDecimal(bedCharges.NumberOfHour) / Convert.ToDecimal(24);

                            if (Convert.ToInt32(hdnBedChargesInDay.Value) == 0)
                            {
                                basePrice = bedCharges.ChargesAmount / bedCharges.NumberOfHour;
                                basePriceComp1 = bedCharges.ChargesAmount / bedCharges.NumberOfHour;
                                price = bedCharges.ChargesAmount / bedCharges.NumberOfHour;
                                priceComp1 = bedCharges.ChargesAmount / bedCharges.NumberOfHour;
                            }
                            else
                            {
                                basePrice = Math.Round((Convert.ToDecimal(bedCharges.ChargesAmount) / Convert.ToDecimal(bedCharges.NumberOfHour)) * Convert.ToDecimal(24) / 1000) * 1000;
                                basePriceComp1 = Math.Round((Convert.ToDecimal(bedCharges.ChargesAmount) / Convert.ToDecimal(bedCharges.NumberOfHour)) * Convert.ToDecimal(24) / 1000) * 1000;
                                price = Math.Round((Convert.ToDecimal(bedCharges.ChargesAmount) / Convert.ToDecimal(bedCharges.NumberOfHour)) * Convert.ToDecimal(24) / 1000) * 1000;
                                priceComp1 = Math.Round((Convert.ToDecimal(bedCharges.ChargesAmount) / Convert.ToDecimal(bedCharges.NumberOfHour)) * Convert.ToDecimal(24) / 1000) * 1000;
                            }

                            basePriceComp2 = 0;
                            basePriceComp3 = 0;

                            priceComp2 = 0;
                            priceComp3 = 0;

                            patientChargesDt.BaseTariff = basePrice;
                            patientChargesDt.BaseComp1 = basePriceComp1;
                            patientChargesDt.BaseComp2 = basePriceComp2;
                            patientChargesDt.BaseComp3 = basePriceComp3;
                            patientChargesDt.Tariff = price;
                            patientChargesDt.TariffComp1 = priceComp1;
                            patientChargesDt.TariffComp2 = priceComp2;
                            patientChargesDt.TariffComp3 = priceComp3;
                            patientChargesDt.CostAmount = costAmount;

                            vItemService itemService = BusinessLayer.GetvItemServiceList(string.Format("ItemID IN ({0})", Convert.ToInt32(bedCharges.ItemID)), ctx).FirstOrDefault();
                            if (itemService != null)
                            {
                                patientChargesDt.IsCITOInPercentage = itemService.IsCITOInPercentage;
                                patientChargesDt.IsComplicationInPercentage = itemService.IsComplicationInPercentage;
                                patientChargesDt.BaseCITOAmount = itemService.CITOAmount;
                                patientChargesDt.BaseComplicationAmount = itemService.ComplicationAmount;

                                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = itemService.GCItemUnit;
                                patientChargesDt.IsSubContractItem = itemService.IsSubContractItem;
                            }


                            patientChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);

                            patientChargesDt.IsVariable = false;
                            patientChargesDt.IsUnbilledItem = false;

                            patientChargesDt.IsCITO = false;
                            patientChargesDt.CITOAmount = 0;
                            patientChargesDt.IsComplication = false;
                            patientChargesDt.ComplicationAmount = 0;
                            patientChargesDt.IsDiscount = false;
                            patientChargesDt.DiscountAmount = 0;
                            patientChargesDt.DiscountComp1 = 0;
                            patientChargesDt.DiscountComp2 = 0;
                            patientChargesDt.DiscountComp3 = 0;

                            if (Convert.ToInt32(hdnBedChargesInDay.Value) == 0)
                            {
                                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = bedCharges.NumberOfHour;
                                patientChargesDt.PatientAmount = oReg.BusinessPartnerID == 1 ? bedCharges.ChargesAmount : 0;
                                patientChargesDt.PayerAmount = oReg.BusinessPartnerID == 1 ? 0 : bedCharges.ChargesAmount;
                                patientChargesDt.LineAmount = bedCharges.ChargesAmount;
                            }
                            else
                            {
                                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = numberOfHourInDay;
                                patientChargesDt.PatientAmount = oReg.BusinessPartnerID == 1 ? Math.Round(patientChargesDt.Tariff, 2) * Math.Round(numberOfHourInDay, 2) : 0;
                                patientChargesDt.PayerAmount = oReg.BusinessPartnerID == 1 ? 0 : Math.Round(patientChargesDt.Tariff, 2) * Math.Round(numberOfHourInDay, 2);
                                patientChargesDt.LineAmount = Math.Round(patientChargesDt.Tariff, 2) * Math.Round(numberOfHourInDay, 2);
                            }

                            patientChargesDt.RevenueSharingID = null;

                            patientChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                            patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                            patientChargesDt.TransactionID = transactionID;
                            int ID = chargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                            #endregion

                            #region RegistrationBedCharges

                            bedCharges.TransactionID = transactionID;
                            bedCharges.LastUpdatedBy = AppSession.UserLogin.UserID;
                            bedChargesDao.Update(bedCharges);

                            #endregion

                            ctx.CommitTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Sudah dilakukan proses transaksi sebelumnya.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Daftar tinggal sementara tidak tersedia.";
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            RegistrationBedChargesDao bedChargesDao = new RegistrationBedChargesDao(ctx);

            try
            {
                if (type == "closejobbed")
                {
                    int registrationID = Convert.ToInt32(hdnRegistrationID.Value);

                    Registration oReg = entityRegistrationDao.Get(registrationID);
                    if (!oReg.IsJobBedClosed)
                    {
                        oReg.IsJobBedClosed = true;
                        oReg.JobBedClosedBy = AppSession.UserLogin.UserID;
                        oReg.JobBedClosedDate = DateTime.Now;
                        oReg.IsJobBedReopen = false;
                        oReg.JobBedReopenBy = null;
                        oReg.JobBedReopenDate = null;
                        oReg.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityRegistrationDao.Update(oReg);

                        string filterRBC = string.Format("RegistrationID = {0} AND TransactionID IS NULL AND ID = (SELECT ID = MAX(rbc.ID) FROM RegistrationBedCharges rbc WHERE rbc.IsDeleted = 0 AND rbc.RegistrationID = {0} GROUP BY rbc.RegistrationID)", oReg.RegistrationID);
                        List<RegistrationBedCharges> lstRBC = BusinessLayer.GetRegistrationBedChargesList(filterRBC, ctx);
                        if (lstRBC.Count() > 0)
                        {
                            RegistrationBedCharges oRBC = bedChargesDao.Get(lstRBC.FirstOrDefault().ID);
                            int oriNumberOfHour = oRBC.NumberOfHour;
                            if (Convert.ToInt32(hdnBedChargesTimeRounding.Value) == 0)
                            {
                                oRBC.NumberOfHour = oriNumberOfHour;
                                oRBC.ChargesAmount = (oRBC.ChargesAmount / oriNumberOfHour) * (oriNumberOfHour);
                            }
                            else
                            {
                                int roundingHour = (Convert.ToInt32(oriNumberOfHour % Convert.ToInt32(hdnBedChargesTimeRounding.Value)));
                                int newNumberOfHour = oriNumberOfHour - roundingHour + Convert.ToInt32(hdnBedChargesTimeRounding.Value);
                                oRBC.NumberOfHour = newNumberOfHour;
                                oRBC.ChargesAmount = (oRBC.ChargesAmount / oriNumberOfHour) * (newNumberOfHour);
                            }
                            oRBC.LastUpdatedBy = AppSession.UserLogin.UserID;
                            bedChargesDao.Update(oRBC);
                        }

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Maaf, job bed untuk registrasi <b>{0}</b> sudah ditutup pada <b>{1}</b>.", oReg.RegistrationNo, Convert.ToDateTime(oReg.JobBedClosedDate).ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT));
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
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