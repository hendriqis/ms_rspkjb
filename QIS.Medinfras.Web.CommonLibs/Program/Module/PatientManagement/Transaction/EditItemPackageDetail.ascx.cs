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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class EditItemPackageDetail : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] data = param.Split('|');
            hdnChargesDtID.Value = data[0];

            hdnTariffComp1TextCtl.Value = data[2];
            hdnTariffComp2TextCtl.Value = data[3];
            hdnTariffComp3TextCtl.Value = data[4];

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1 //1
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsEndingAmountRoundingTo1.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;

            hdnVisitIDCtl.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnRegistrationIDCtl.Value = AppSession.RegisteredPatient.RegistrationID.ToString();

            PatientChargesDt chargesDt = BusinessLayer.GetPatientChargesDt(Convert.ToInt32(hdnChargesDtID.Value));
            hdnChargesClassID.Value = chargesDt.ChargeClassID.ToString();
            hdnPatientChargesDtID.Value = chargesDt.ID.ToString();

            PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHd(chargesDt.TransactionID);
            hdnChargesHealthcareServiceUnitID.Value = entityHd.HealthcareServiceUnitID.ToString();
            hdnTransactionDate.Value = entityHd.TransactionDateInDatePickerFormat;
            hdnTransactionTime.Value = entityHd.TransactionTime;

            string filterItem = string.Format("ItemID = '{0}'", Convert.ToInt32(data[1]));
            vItemService imaster = BusinessLayer.GetvItemServiceList(filterItem).FirstOrDefault();
            hdnItemID.Value = imaster.ItemID.ToString();
            hdnIsUsingAccumulatedPrice.Value = Convert.ToInt32(imaster.IsUsingAccumulatedPrice).ToString();
            txtItemServiceName.Text = string.Format("{0} - {1}", imaster.ItemCode, imaster.ItemName1);

            txtServiceDiscPercentComp1CtlAll.Text = "0.00";
            txtServiceDiscPercentComp2CtlAll.Text = "0.00";
            txtServiceDiscPercentComp3CtlAll.Text = "0.00";

            if (imaster.IsUsingAccumulatedPrice)
            {
                tblDiscountAll.Style.Remove("display");
            }
            else
            {
                tblDiscountAll.Style.Add("display", "none");
            }

            SetControlEntrySetting();

            BindGridView();
        }

        protected string GetTariffComponent1Text()
        {
            return hdnTariffComp1TextCtl.Value;
        }

        protected string GetTariffComponent2Text()
        {
            return hdnTariffComp2TextCtl.Value;
        }

        protected string GetTariffComponent3Text()
        {
            return hdnTariffComp3TextCtl.Value;
        }

        private void SetControlEntrySetting()
        {
            Helper.SetControlEntrySetting(hdnDetailItemID, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailItemCode, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailItemName1, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(hdnParamedicID, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(false, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtParamedicName, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(hdnRevenueSharingID, new ControlEntrySetting(false, false, false), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtChargedQuantity, new ControlEntrySetting(false, true, true), "mpEntryPopup");

            Helper.SetControlEntrySetting(hdnDetailItemIDObat, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailItemCodeObat, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailItemName1Obat, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(hdnParamedicIDObat, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtParamedicCodeObat, new ControlEntrySetting(false, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtParamedicNameObat, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(hdnRevenueSharingIDObat, new ControlEntrySetting(false, false, false), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtChargedQuantity, new ControlEntrySetting(false, true, true), "mpEntryPopup");

            Helper.SetControlEntrySetting(hdnDetailItemIDBarang, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailItemCodeBarang, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailItemName1Barang, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(hdnParamedicIDBarang, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtParamedicCodeBarang, new ControlEntrySetting(false, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtParamedicNameBarang, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(hdnRevenueSharingIDBarang, new ControlEntrySetting(false, false, false), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtChargedQuantity, new ControlEntrySetting(false, true, true), "mpEntryPopup");
        }

        private void BindGridView()
        {
            string filterDt = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0 ORDER BY ID ASC", hdnChargesDtID.Value);
            List<vPatientChargesDtPackage> lstItemService = BusinessLayer.GetvPatientChargesDtPackageList(filterDt);

            grdView.DataSource = from i in lstItemService
                                 where i.GCItemType != Constant.ItemType.OBAT_OBATAN && i.GCItemType != Constant.ItemType.BARANG_MEDIS && i.GCItemType != Constant.ItemType.BARANG_UMUM && i.GCItemType != Constant.ItemType.BAHAN_MAKANAN
                                 select i;
            grdView.DataBind();

            grdViewObat.DataSource = from i in lstItemService
                                     where i.GCItemType == Constant.ItemType.OBAT_OBATAN || i.GCItemType == Constant.ItemType.BARANG_MEDIS
                                     select i;
            grdViewObat.DataBind();

            grdViewBarang.DataSource = from i in lstItemService
                                       where i.GCItemType == Constant.ItemType.BARANG_UMUM || i.GCItemType == Constant.ItemType.BAHAN_MAKANAN
                                       select i;
            grdViewBarang.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "saveAll")
            {
                if (OnSaveEditRecordAll(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PatientChargesDtPackage entity)
        {
            entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            if (hdnRevenueSharingID.Value != "" && hdnRevenueSharingID.Value != "0")
            {
                entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            }
            entity.ChargedQuantity = Convert.ToDecimal(txtChargedQuantity.Text);

            entity.DiscountAmount = Convert.ToDecimal(Request.Form[txtServiceDiscTotalCtl.UniqueID]);
            entity.DiscountComp1 = Convert.ToDecimal(Request.Form[txtServiceDiscComp1Ctl.UniqueID]);
            entity.DiscountComp2 = Convert.ToDecimal(Request.Form[txtServiceDiscComp2Ctl.UniqueID]);
            entity.DiscountComp3 = Convert.ToDecimal(Request.Form[txtServiceDiscComp3Ctl.UniqueID]);

            if (entity.DiscountComp1 > 0 && entity.DiscountComp1 > 0)
            {
                entity.DiscountPercentageComp1 = Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp1Ctl.UniqueID]);
                if (entity.DiscountPercentageComp1 > 0)
                {
                    entity.IsDiscountInPercentageComp1 = true;
                }
                else
                {
                    entity.IsDiscountInPercentageComp1 = false;
                }
            }
            else
            {
                entity.DiscountPercentageComp1 = 0;
                entity.IsDiscountInPercentageComp1 = false;
            }

            if (entity.DiscountComp2 > 0 && entity.DiscountComp2 > 0)
            {
                entity.DiscountPercentageComp2 = Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp2Ctl.UniqueID]);
                if (entity.DiscountPercentageComp2 > 0)
                {
                    entity.IsDiscountInPercentageComp2 = true;
                }
                else
                {
                    entity.IsDiscountInPercentageComp2 = false;
                }
            }
            else
            {
                entity.DiscountPercentageComp2 = 0;
                entity.IsDiscountInPercentageComp2 = false;
            }

            if (entity.DiscountComp3 > 0 && entity.DiscountComp3 > 0)
            {
                entity.DiscountPercentageComp3 = Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp3Ctl.UniqueID]);
                if (entity.DiscountPercentageComp3 > 0)
                {
                    entity.IsDiscountInPercentageComp3 = true;
                }
                else
                {
                    entity.IsDiscountInPercentageComp3 = false;
                }
            }
            else
            {
                entity.DiscountPercentageComp3 = 0;
                entity.IsDiscountInPercentageComp3 = false;
            }
        }

        private void ControlToEntity2(PatientChargesDtPackage entity)
        {
            if (entity.TariffComp1 > 0)
            {
                if (Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp1CtlAll.UniqueID]) > 0)
                {
                    entity.DiscountPercentageComp1 = Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp1CtlAll.UniqueID]);
                    decimal disc1 = ((Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp1CtlAll.UniqueID]) / 100) * entity.TariffComp1);
                    entity.DiscountComp1 = disc1;
                }
                else
                {
                    entity.DiscountPercentageComp1 = 0;
                    entity.DiscountComp1 = 0;
                }
            }

            if (entity.TariffComp2 > 0)
            {
                if (Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp2CtlAll.UniqueID]) > 0)
                {
                    entity.DiscountPercentageComp2 = Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp2CtlAll.UniqueID]);
                    decimal disc2 = ((Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp2CtlAll.UniqueID]) / 100) * entity.TariffComp2);
                    entity.DiscountComp2 = disc2;
                }
                else
                {
                    entity.DiscountPercentageComp2 = 0;
                    entity.DiscountComp2 = 0;
                }
            }

            if (entity.TariffComp3 > 0)
            {
                if (Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp3CtlAll.UniqueID]) > 0)
                {
                    entity.DiscountPercentageComp3 = Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp3CtlAll.UniqueID]);
                    decimal disc3 = ((Convert.ToDecimal(Request.Form[txtServiceDiscPercentComp3CtlAll.UniqueID]) / 100) * entity.TariffComp3);
                    entity.DiscountComp3 = disc3;
                }
                else
                {
                    entity.DiscountPercentageComp3 = 0;
                    entity.DiscountComp3 = 0;
                }
            }

            entity.DiscountAmount = ((entity.DiscountComp1 + entity.DiscountComp2 + entity.DiscountComp3) * entity.ChargedQuantity);

            if (entity.DiscountPercentageComp1 > 0)
            {
                entity.IsDiscountInPercentageComp1 = true;
            }
            else
            {
                entity.IsDiscountInPercentageComp1 = false;
            }

            if (entity.DiscountPercentageComp2 > 0)
            {
                entity.IsDiscountInPercentageComp2 = true;
            }
            else
            {
                entity.IsDiscountInPercentageComp2 = false;
            }

            if (entity.DiscountPercentageComp3 > 0)
            {
                entity.IsDiscountInPercentageComp3 = true;
            }
            else
            {
                entity.IsDiscountInPercentageComp3 = false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao entityDtDao = new PatientChargesDtPackageDao(ctx);
            PatientChargesDtParamedicDao entityDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
            try
            {
                string filterPackage = string.Format("PatientChargesDtID = '{0}' AND IsDeleted = 0", hdnPatientChargesDtID.Value);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<PatientChargesDtPackage> lstDtPackage = BusinessLayer.GetPatientChargesDtPackageList(filterPackage, ctx);
                PatientChargesDtPackage entity = lstDtPackage.Where(t => t.ID == Convert.ToInt32(hdnID.Value)).FirstOrDefault();

                ControlToEntity(entity);

                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);

                if (hdnIsUsingAccumulatedPrice.Value == "1")
                {
                    PatientChargesDt pcdt = entityChargesDtDao.Get(entity.PatientChargesDtID);

                    decimal BaseTariff = 0;
                    decimal BaseComp1 = 0;
                    decimal BaseComp2 = 0;
                    decimal BaseComp3 = 0;
                    decimal Tariff = 0;
                    decimal TariffComp1 = 0;
                    decimal TariffComp2 = 0;
                    decimal TariffComp3 = 0;
                    decimal DiscountAmount = 0;
                    decimal DiscountComp1 = 0;
                    decimal DiscountComp2 = 0;
                    decimal DiscountComp3 = 0;
                    foreach (PatientChargesDtPackage e in lstDtPackage)
                    {
                        BaseTariff += e.BaseTariff * e.ChargedQuantity;
                        BaseComp1 += e.BaseComp1 * e.ChargedQuantity;
                        BaseComp2 += e.BaseComp2 * e.ChargedQuantity;
                        BaseComp3 += e.BaseComp3 * e.ChargedQuantity;
                        Tariff += e.Tariff * e.ChargedQuantity;
                        TariffComp1 += e.TariffComp1 * e.ChargedQuantity;
                        TariffComp2 += e.TariffComp2 * e.ChargedQuantity;
                        TariffComp3 += e.TariffComp3 * e.ChargedQuantity;
                        DiscountAmount += e.DiscountAmount;
                        DiscountComp1 += e.DiscountComp1 * e.ChargedQuantity;
                        DiscountComp2 += e.DiscountComp2 * e.ChargedQuantity;
                        DiscountComp3 += e.DiscountComp3 * e.ChargedQuantity;

                    }

                    pcdt.BaseTariff = BaseTariff / pcdt.ChargedQuantity;
                    pcdt.BaseComp1 = BaseComp1 / pcdt.ChargedQuantity;
                    pcdt.BaseComp2 = BaseComp2 / pcdt.ChargedQuantity;
                    pcdt.BaseComp3 = BaseComp3 / pcdt.ChargedQuantity;
                    pcdt.Tariff = Tariff / pcdt.ChargedQuantity;
                    pcdt.TariffComp1 = TariffComp1 / pcdt.ChargedQuantity;
                    pcdt.TariffComp2 = TariffComp2 / pcdt.ChargedQuantity;
                    pcdt.TariffComp3 = TariffComp3 / pcdt.ChargedQuantity;
                    pcdt.DiscountAmount = DiscountAmount;
                    pcdt.DiscountComp1 = DiscountComp1 / pcdt.ChargedQuantity;
                    pcdt.DiscountComp2 = DiscountComp2 / pcdt.ChargedQuantity;
                    pcdt.DiscountComp3 = DiscountComp3 / pcdt.ChargedQuantity;

                    if (pcdt.IsCITO)
                    {
                        if (pcdt.IsCITOInPercentage)
                        {
                            decimal tariff = (pcdt.Tariff * pcdt.ChargedQuantity);
                            pcdt.CITOAmount = ((pcdt.BaseCITOAmount / 100) * tariff);
                        }
                        else
                        {
                            pcdt.CITOAmount = pcdt.BaseCITOAmount * pcdt.ChargedQuantity;
                        }
                    }
                    else
                    {
                        pcdt.CITOAmount = 0;
                    }

                    if (pcdt.DiscountAmount != 0)
                    {
                        pcdt.IsDiscount = true;
                    }
                    else
                    {
                        pcdt.IsDiscount = false;
                    }

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationIDCtl.Value), Convert.ToInt32(hdnVisitIDCtl.Value), pcdt.ChargeClassID, pcdt.ItemID, 1, pcdt.CreatedDate, ctx);

                    decimal coverageAmount = 0;
                    bool isCoverageInPercentage = false;
                    if (list.Count > 0)
                    {
                        GetCurrentItemTariff obj = list[0];
                        coverageAmount = obj.CoverageAmount;
                        isCoverageInPercentage = obj.IsCoverageInPercentage;
                    }

                    decimal grossLineAmount = (pcdt.Tariff * pcdt.ChargedQuantity) + (pcdt.CITOAmount - pcdt.CITODiscount);
                    decimal totalDiscountAmount = pcdt.DiscountAmount;
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
                        totalPayer = coverageAmount * pcdt.ChargedQuantity;
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

                    pcdt.PatientAmount = oPatientAmount;
                    pcdt.PayerAmount = oPayerAmount;
                    pcdt.LineAmount = oLineAmount;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityChargesDtDao.Update(pcdt);
                }

                string filterTeam = string.Format("ParamedicParentID = {0} AND IsDeleted = 0", entity.ParamedicID);
                List<ParamedicMasterTeam> pmtList = BusinessLayer.GetParamedicMasterTeamList(filterTeam, ctx);
                foreach (ParamedicMasterTeam pmt in pmtList)
                {
                    PatientChargesDtParamedic dtParamedicCheck = BusinessLayer.GetPatientChargesDtParamedic(Convert.ToInt32(hdnChargesDtID.Value), pmt.ParamedicID, entity.ItemID);
                    if (dtParamedicCheck == null)
                    {
                        PatientChargesDtParamedic dtparamedic = new PatientChargesDtParamedic();
                        dtparamedic.ID = Convert.ToInt32(hdnChargesDtID.Value);
                        dtparamedic.ItemID = entity.ItemID;
                        dtparamedic.ParamedicID = pmt.ParamedicID;
                        dtparamedic.GCParamedicRole = pmt.GCParamedicRole;
                        dtparamedic.RevenueSharingID = pmt.RevenueSharingID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtParamedicDao.Insert(dtparamedic);
                    }
                }

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

        private bool OnSaveEditRecordAll(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao entityDtDao = new PatientChargesDtPackageDao(ctx);
            try
            {
                string filterPackage = string.Format("PatientChargesDtID = '{0}' AND IsDeleted = 0", hdnPatientChargesDtID.Value);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<vPatientChargesDtPackage> lstDtPackage = BusinessLayer.GetvPatientChargesDtPackageList(filterPackage, ctx);

                List<PatientChargesDtPackage> lstDtFinal = new List<PatientChargesDtPackage>();
                foreach (vPatientChargesDtPackage e in lstDtPackage)
                {
                    PatientChargesDtPackage entity = entityDtDao.Get(e.ID);
                    //if (e.GCItemType == Constant.ItemType.PELAYANAN)
                    //{
                    ControlToEntity2(entity);

                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entity);
                    //                    }
                    lstDtFinal.Add(entity);
                }

                if (hdnIsUsingAccumulatedPrice.Value == "1")
                {
                    PatientChargesDt pcdt = entityChargesDtDao.Get(Convert.ToInt32(hdnChargesDtID.Value));

                    decimal BaseTariff = 0;
                    decimal BaseComp1 = 0;
                    decimal BaseComp2 = 0;
                    decimal BaseComp3 = 0;
                    decimal Tariff = 0;
                    decimal TariffComp1 = 0;
                    decimal TariffComp2 = 0;
                    decimal TariffComp3 = 0;
                    decimal DiscountAmount = 0;
                    decimal DiscountComp1 = 0;
                    decimal DiscountComp2 = 0;
                    decimal DiscountComp3 = 0;
                    foreach (PatientChargesDtPackage e in lstDtFinal)
                    {
                        BaseTariff += e.BaseTariff * e.ChargedQuantity;
                        BaseComp1 += e.BaseComp1 * e.ChargedQuantity;
                        BaseComp2 += e.BaseComp2 * e.ChargedQuantity;
                        BaseComp3 += e.BaseComp3 * e.ChargedQuantity;
                        Tariff += e.Tariff * e.ChargedQuantity;
                        TariffComp1 += e.TariffComp1 * e.ChargedQuantity;
                        TariffComp2 += e.TariffComp2 * e.ChargedQuantity;
                        TariffComp3 += e.TariffComp3 * e.ChargedQuantity;
                        DiscountAmount += e.DiscountAmount;
                        DiscountComp1 += e.DiscountComp1 * e.ChargedQuantity;
                        DiscountComp2 += e.DiscountComp2 * e.ChargedQuantity;
                        DiscountComp3 += e.DiscountComp3 * e.ChargedQuantity;

                    }

                    pcdt.BaseTariff = BaseTariff / pcdt.ChargedQuantity;
                    pcdt.BaseComp1 = BaseComp1 / pcdt.ChargedQuantity;
                    pcdt.BaseComp2 = BaseComp2 / pcdt.ChargedQuantity;
                    pcdt.BaseComp3 = BaseComp3 / pcdt.ChargedQuantity;
                    pcdt.Tariff = Tariff / pcdt.ChargedQuantity;
                    pcdt.TariffComp1 = TariffComp1 / pcdt.ChargedQuantity;
                    pcdt.TariffComp2 = TariffComp2 / pcdt.ChargedQuantity;
                    pcdt.TariffComp3 = TariffComp3 / pcdt.ChargedQuantity;
                    pcdt.DiscountAmount = DiscountAmount;
                    pcdt.DiscountComp1 = DiscountComp1 / pcdt.ChargedQuantity;
                    pcdt.DiscountComp2 = DiscountComp2 / pcdt.ChargedQuantity;
                    pcdt.DiscountComp3 = DiscountComp3 / pcdt.ChargedQuantity;

                    if (pcdt.IsCITO)
                    {
                        if (pcdt.IsCITOInPercentage)
                        {
                            decimal tariff = (pcdt.Tariff * pcdt.ChargedQuantity);
                            pcdt.CITOAmount = ((pcdt.BaseCITOAmount / 100) * tariff);
                        }
                        else
                        {
                            pcdt.CITOAmount = pcdt.BaseCITOAmount * pcdt.ChargedQuantity;
                        }
                    }
                    else
                    {
                        pcdt.CITOAmount = 0;
                    }

                    if (pcdt.DiscountAmount != 0)
                    {
                        pcdt.IsDiscount = true;
                    }
                    else
                    {
                        pcdt.IsDiscount = false;
                    }

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationIDCtl.Value), Convert.ToInt32(hdnVisitIDCtl.Value), pcdt.ChargeClassID, pcdt.ItemID, 1, pcdt.CreatedDate, ctx);

                    decimal coverageAmount = 0;
                    bool isCoverageInPercentage = false;
                    if (list.Count > 0)
                    {
                        GetCurrentItemTariff obj = list[0];
                        coverageAmount = obj.CoverageAmount;
                        isCoverageInPercentage = obj.IsCoverageInPercentage;
                    }

                    decimal grossLineAmount = (pcdt.Tariff * pcdt.ChargedQuantity) + (pcdt.CITOAmount - pcdt.CITODiscount);
                    decimal totalDiscountAmount = pcdt.DiscountAmount;
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
                        totalPayer = coverageAmount * pcdt.ChargedQuantity;
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

                    pcdt.PatientAmount = oPatientAmount;
                    pcdt.PayerAmount = oPayerAmount;
                    pcdt.LineAmount = oLineAmount;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityChargesDtDao.Update(pcdt);
                }

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

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                PatientChargesDtPackage entity = BusinessLayer.GetPatientChargesDtPackage(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientChargesDtPackage(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        protected void grdViewObat_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }

        protected void cbpEntryPopupViewObat_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnIDObat.Value.ToString() != "")
                {
                    if (OnSaveEditRecordObat(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecordObat(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntityObat(PatientChargesDtPackage entity)
        {
            entity.ParamedicID = Convert.ToInt32(hdnParamedicIDObat.Value);
            if (hdnRevenueSharingIDObat.Value != "" && hdnRevenueSharingIDObat.Value != "0")
            {
                entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingIDObat.Value);
            }
            entity.ChargedQuantity = Convert.ToDecimal(txtChargedQuantityObat.Text);

            entity.DiscountAmount = Convert.ToDecimal(Request.Form[txtObatDiscTotalCtl.UniqueID]);
            entity.DiscountComp1 = Convert.ToDecimal(Request.Form[txtObatDiscComp1Ctl.UniqueID]);
            entity.DiscountComp2 = Convert.ToDecimal(Request.Form[txtObatDiscComp2Ctl.UniqueID]);
            entity.DiscountComp3 = Convert.ToDecimal(Request.Form[txtObatDiscComp3Ctl.UniqueID]);

            if (entity.DiscountComp1 > 0 && entity.DiscountComp1 > 0)
            {
                entity.DiscountPercentageComp1 = Convert.ToDecimal(Request.Form[txtObatDiscPercentComp1Ctl.UniqueID]);
                if (entity.DiscountPercentageComp1 > 0)
                {
                    entity.IsDiscountInPercentageComp1 = true;
                }
                else
                {
                    entity.IsDiscountInPercentageComp1 = false;
                }
            }
            else
            {
                entity.DiscountPercentageComp1 = 0;
                entity.IsDiscountInPercentageComp1 = false;
            }

            if (entity.DiscountComp2 > 0 && entity.DiscountComp2 > 0)
            {
                entity.DiscountPercentageComp2 = Convert.ToDecimal(Request.Form[txtObatDiscPercentComp2Ctl.UniqueID]);
                if (entity.DiscountPercentageComp2 > 0)
                {
                    entity.IsDiscountInPercentageComp2 = true;
                }
                else
                {
                    entity.IsDiscountInPercentageComp2 = false;
                }
            }
            else
            {
                entity.DiscountPercentageComp2 = 0;
                entity.IsDiscountInPercentageComp2 = false;
            }

            if (entity.DiscountComp3 > 0 && entity.DiscountComp3 > 0)
            {
                entity.DiscountPercentageComp3 = Convert.ToDecimal(Request.Form[txtObatDiscPercentComp3Ctl.UniqueID]);
                if (entity.DiscountPercentageComp3 > 0)
                {
                    entity.IsDiscountInPercentageComp3 = true;
                }
                else
                {
                    entity.IsDiscountInPercentageComp3 = false;
                }
            }
            else
            {
                entity.DiscountPercentageComp3 = 0;
                entity.IsDiscountInPercentageComp3 = false;
            }
        }

        private bool OnSaveEditRecordObat(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao entityDtDao = new PatientChargesDtPackageDao(ctx);
            PatientChargesDtParamedicDao entityDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
            try
            {
                string filterPackage = string.Format("PatientChargesDtID = '{0}' AND IsDeleted = 0", hdnPatientChargesDtID.Value);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<PatientChargesDtPackage> lstDtPackage = BusinessLayer.GetPatientChargesDtPackageList(filterPackage, ctx);
                PatientChargesDtPackage entity = lstDtPackage.Where(t => t.ID == Convert.ToInt32(hdnIDObat.Value)).FirstOrDefault();

                ControlToEntityObat(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);

                if (hdnIsUsingAccumulatedPrice.Value == "1")
                {
                    PatientChargesDt pcdt = entityChargesDtDao.Get(entity.PatientChargesDtID);

                    decimal BaseTariff = 0;
                    decimal BaseComp1 = 0;
                    decimal BaseComp2 = 0;
                    decimal BaseComp3 = 0;
                    decimal Tariff = 0;
                    decimal TariffComp1 = 0;
                    decimal TariffComp2 = 0;
                    decimal TariffComp3 = 0;
                    decimal DiscountAmount = 0;
                    decimal DiscountComp1 = 0;
                    decimal DiscountComp2 = 0;
                    decimal DiscountComp3 = 0;
                    foreach (PatientChargesDtPackage e in lstDtPackage)
                    {
                        BaseTariff += e.BaseTariff * e.ChargedQuantity;
                        BaseComp1 += e.BaseComp1 * e.ChargedQuantity;
                        BaseComp2 += e.BaseComp2 * e.ChargedQuantity;
                        BaseComp3 += e.BaseComp3 * e.ChargedQuantity;
                        Tariff += e.Tariff * e.ChargedQuantity;
                        TariffComp1 += e.TariffComp1 * e.ChargedQuantity;
                        TariffComp2 += e.TariffComp2 * e.ChargedQuantity;
                        TariffComp3 += e.TariffComp3 * e.ChargedQuantity;
                        DiscountAmount += e.DiscountAmount;
                        DiscountComp1 += e.DiscountComp1 * e.ChargedQuantity;
                        DiscountComp2 += e.DiscountComp2 * e.ChargedQuantity;
                        DiscountComp3 += e.DiscountComp3 * e.ChargedQuantity;

                    }

                    pcdt.BaseTariff = BaseTariff / pcdt.ChargedQuantity;
                    pcdt.BaseComp1 = BaseComp1 / pcdt.ChargedQuantity;
                    pcdt.BaseComp2 = BaseComp2 / pcdt.ChargedQuantity;
                    pcdt.BaseComp3 = BaseComp3 / pcdt.ChargedQuantity;
                    pcdt.Tariff = Tariff / pcdt.ChargedQuantity;
                    pcdt.TariffComp1 = TariffComp1 / pcdt.ChargedQuantity;
                    pcdt.TariffComp2 = TariffComp2 / pcdt.ChargedQuantity;
                    pcdt.TariffComp3 = TariffComp3 / pcdt.ChargedQuantity;
                    pcdt.DiscountAmount = DiscountAmount;
                    pcdt.DiscountComp1 = DiscountComp1 / pcdt.ChargedQuantity;
                    pcdt.DiscountComp2 = DiscountComp2 / pcdt.ChargedQuantity;
                    pcdt.DiscountComp3 = DiscountComp3 / pcdt.ChargedQuantity;

                    if (pcdt.IsCITO)
                    {
                        if (pcdt.IsCITOInPercentage)
                        {
                            decimal tariff = (pcdt.Tariff * pcdt.ChargedQuantity);
                            pcdt.CITOAmount = ((pcdt.BaseCITOAmount / 100) * tariff);
                        }
                        else
                        {
                            pcdt.CITOAmount = pcdt.BaseCITOAmount * pcdt.ChargedQuantity;
                        }
                    }
                    else
                    {
                        pcdt.CITOAmount = 0;
                    }

                    if (pcdt.DiscountAmount != 0)
                    {
                        pcdt.IsDiscount = true;
                    }
                    else
                    {
                        pcdt.IsDiscount = false;
                    }

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationIDCtl.Value), Convert.ToInt32(hdnVisitIDCtl.Value), pcdt.ChargeClassID, pcdt.ItemID, 1, pcdt.CreatedDate, ctx);

                    decimal coverageAmount = 0;
                    bool isCoverageInPercentage = false;
                    if (list.Count > 0)
                    {
                        GetCurrentItemTariff obj = list[0];
                        coverageAmount = obj.CoverageAmount;
                        isCoverageInPercentage = obj.IsCoverageInPercentage;
                    }

                    decimal grossLineAmount = (pcdt.Tariff * pcdt.ChargedQuantity) + (pcdt.CITOAmount - pcdt.CITODiscount);
                    decimal totalDiscountAmount = pcdt.DiscountAmount;
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
                        totalPayer = coverageAmount * pcdt.ChargedQuantity;
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

                    pcdt.PatientAmount = total - totalPayer;
                    pcdt.PayerAmount = totalPayer;
                    pcdt.LineAmount = total;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityChargesDtDao.Update(pcdt);
                }

                string filterTeam = string.Format("ParamedicParentID = {0} AND IsDeleted = 0", entity.ParamedicID);
                List<ParamedicMasterTeam> pmtList = BusinessLayer.GetParamedicMasterTeamList(filterTeam, ctx);
                foreach (ParamedicMasterTeam pmt in pmtList)
                {
                    PatientChargesDtParamedic dtParamedicCheck = BusinessLayer.GetPatientChargesDtParamedic(Convert.ToInt32(hdnChargesDtID.Value), pmt.ParamedicID, entity.ItemID);
                    if (dtParamedicCheck == null)
                    {
                        PatientChargesDtParamedic dtparamedic = new PatientChargesDtParamedic();
                        dtparamedic.ID = Convert.ToInt32(hdnChargesDtID.Value);
                        dtparamedic.ItemID = entity.ItemID;
                        dtparamedic.ParamedicID = pmt.ParamedicID;
                        dtparamedic.GCParamedicRole = pmt.GCParamedicRole;
                        dtparamedic.RevenueSharingID = pmt.RevenueSharingID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtParamedicDao.Insert(dtparamedic);
                    }
                }

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

        private bool OnDeleteRecordObat(ref string errMessage)
        {
            try
            {
                PatientChargesDtPackage entity = BusinessLayer.GetPatientChargesDtPackage(Convert.ToInt32(hdnIDObat.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientChargesDtPackage(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        protected void grdViewBarang_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }

        protected void cbpEntryPopupViewBarang_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnIDBarang.Value.ToString() != "")
                {
                    if (OnSaveEditRecordBarang(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecordBarang(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntityBarang(PatientChargesDtPackage entity)
        {
            entity.ParamedicID = Convert.ToInt32(hdnParamedicIDBarang.Value);
            if (hdnRevenueSharingIDBarang.Value != "" && hdnRevenueSharingIDBarang.Value != "0")
            {
                entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingIDBarang.Value);
            }
            entity.ChargedQuantity = Convert.ToDecimal(txtChargedQuantityBarang.Text);

            entity.DiscountAmount = Convert.ToDecimal(Request.Form[txtBarangDiscTotalCtl.UniqueID]);
            entity.DiscountComp1 = Convert.ToDecimal(Request.Form[txtBarangDiscComp1Ctl.UniqueID]);
            entity.DiscountComp2 = Convert.ToDecimal(Request.Form[txtBarangDiscComp2Ctl.UniqueID]);
            entity.DiscountComp3 = Convert.ToDecimal(Request.Form[txtBarangDiscComp3Ctl.UniqueID]);

            if (entity.DiscountComp1 > 0 && entity.DiscountComp1 > 0)
            {
                entity.DiscountPercentageComp1 = Convert.ToDecimal(Request.Form[txtBarangDiscPercentComp1Ctl.UniqueID]);
                if (entity.DiscountPercentageComp1 > 0)
                {
                    entity.IsDiscountInPercentageComp1 = true;
                }
                else
                {
                    entity.IsDiscountInPercentageComp1 = false;
                }
            }
            else
            {
                entity.DiscountPercentageComp1 = 0;
                entity.IsDiscountInPercentageComp1 = false;
            }

            if (entity.DiscountComp2 > 0 && entity.DiscountComp2 > 0)
            {
                entity.DiscountPercentageComp2 = Convert.ToDecimal(Request.Form[txtBarangDiscPercentComp2Ctl.UniqueID]);
                if (entity.DiscountPercentageComp2 > 0)
                {
                    entity.IsDiscountInPercentageComp2 = true;
                }
                else
                {
                    entity.IsDiscountInPercentageComp2 = false;
                }
            }
            else
            {
                entity.DiscountPercentageComp2 = 0;
                entity.IsDiscountInPercentageComp2 = false;
            }

            if (entity.DiscountComp3 > 0 && entity.DiscountComp3 > 0)
            {
                entity.DiscountPercentageComp3 = Convert.ToDecimal(Request.Form[txtBarangDiscPercentComp3Ctl.UniqueID]);
                if (entity.DiscountPercentageComp3 > 0)
                {
                    entity.IsDiscountInPercentageComp3 = true;
                }
                else
                {
                    entity.IsDiscountInPercentageComp3 = false;
                }
            }
            else
            {
                entity.DiscountPercentageComp3 = 0;
                entity.IsDiscountInPercentageComp3 = false;
            }
        }

        private bool OnSaveEditRecordBarang(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao entityDtDao = new PatientChargesDtPackageDao(ctx);
            PatientChargesDtParamedicDao entityDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
            try
            {
                string filterPackage = string.Format("PatientChargesDtID = '{0}' AND IsDeleted = 0", hdnPatientChargesDtID.Value);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<PatientChargesDtPackage> lstDtPackage = BusinessLayer.GetPatientChargesDtPackageList(filterPackage, ctx);
                PatientChargesDtPackage entity = lstDtPackage.Where(t => t.ID == Convert.ToInt32(hdnIDBarang.Value)).FirstOrDefault();

                ControlToEntityBarang(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);

                if (hdnIsUsingAccumulatedPrice.Value == "1")
                {
                    PatientChargesDt pcdt = entityChargesDtDao.Get(entity.PatientChargesDtID);

                    decimal BaseTariff = 0;
                    decimal BaseComp1 = 0;
                    decimal BaseComp2 = 0;
                    decimal BaseComp3 = 0;
                    decimal Tariff = 0;
                    decimal TariffComp1 = 0;
                    decimal TariffComp2 = 0;
                    decimal TariffComp3 = 0;
                    decimal DiscountAmount = 0;
                    decimal DiscountComp1 = 0;
                    decimal DiscountComp2 = 0;
                    decimal DiscountComp3 = 0;
                    foreach (PatientChargesDtPackage e in lstDtPackage)
                    {
                        BaseTariff += e.BaseTariff * e.ChargedQuantity;
                        BaseComp1 += e.BaseComp1 * e.ChargedQuantity;
                        BaseComp2 += e.BaseComp2 * e.ChargedQuantity;
                        BaseComp3 += e.BaseComp3 * e.ChargedQuantity;
                        Tariff += e.Tariff * e.ChargedQuantity;
                        TariffComp1 += e.TariffComp1 * e.ChargedQuantity;
                        TariffComp2 += e.TariffComp2 * e.ChargedQuantity;
                        TariffComp3 += e.TariffComp3 * e.ChargedQuantity;
                        DiscountAmount += e.DiscountAmount;
                        DiscountComp1 += e.DiscountComp1 * e.ChargedQuantity;
                        DiscountComp2 += e.DiscountComp2 * e.ChargedQuantity;
                        DiscountComp3 += e.DiscountComp3 * e.ChargedQuantity;

                    }

                    pcdt.BaseTariff = BaseTariff / pcdt.ChargedQuantity;
                    pcdt.BaseComp1 = BaseComp1 / pcdt.ChargedQuantity;
                    pcdt.BaseComp2 = BaseComp2 / pcdt.ChargedQuantity;
                    pcdt.BaseComp3 = BaseComp3 / pcdt.ChargedQuantity;
                    pcdt.Tariff = Tariff / pcdt.ChargedQuantity;
                    pcdt.TariffComp1 = TariffComp1 / pcdt.ChargedQuantity;
                    pcdt.TariffComp2 = TariffComp2 / pcdt.ChargedQuantity;
                    pcdt.TariffComp3 = TariffComp3 / pcdt.ChargedQuantity;
                    pcdt.DiscountAmount = DiscountAmount;
                    pcdt.DiscountComp1 = DiscountComp1 / pcdt.ChargedQuantity;
                    pcdt.DiscountComp2 = DiscountComp2 / pcdt.ChargedQuantity;
                    pcdt.DiscountComp3 = DiscountComp3 / pcdt.ChargedQuantity;

                    if (pcdt.IsCITO)
                    {
                        if (pcdt.IsCITOInPercentage)
                        {
                            decimal tariff = (pcdt.Tariff * pcdt.ChargedQuantity);
                            pcdt.CITOAmount = ((pcdt.BaseCITOAmount / 100) * tariff);
                        }
                        else
                        {
                            pcdt.CITOAmount = pcdt.BaseCITOAmount * pcdt.ChargedQuantity;
                        }
                    }
                    else
                    {
                        pcdt.CITOAmount = 0;
                    }

                    if (pcdt.DiscountAmount != 0)
                    {
                        pcdt.IsDiscount = true;
                    }
                    else
                    {
                        pcdt.IsDiscount = false;
                    }

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationIDCtl.Value), Convert.ToInt32(hdnVisitIDCtl.Value), pcdt.ChargeClassID, pcdt.ItemID, 1, pcdt.CreatedDate, ctx);

                    decimal coverageAmount = 0;
                    bool isCoverageInPercentage = false;
                    if (list.Count > 0)
                    {
                        GetCurrentItemTariff obj = list[0];
                        coverageAmount = obj.CoverageAmount;
                        isCoverageInPercentage = obj.IsCoverageInPercentage;
                    }

                    decimal grossLineAmount = (pcdt.Tariff * pcdt.ChargedQuantity) + (pcdt.CITOAmount - pcdt.CITODiscount);
                    decimal totalDiscountAmount = pcdt.DiscountAmount;
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
                        totalPayer = coverageAmount * pcdt.ChargedQuantity;
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

                    pcdt.PatientAmount = total - totalPayer;
                    pcdt.PayerAmount = totalPayer;
                    pcdt.LineAmount = total;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityChargesDtDao.Update(pcdt);
                }

                string filterTeam = string.Format("ParamedicParentID = {0} AND IsDeleted = 0", entity.ParamedicID);
                List<ParamedicMasterTeam> pmtList = BusinessLayer.GetParamedicMasterTeamList(filterTeam, ctx);
                foreach (ParamedicMasterTeam pmt in pmtList)
                {
                    PatientChargesDtParamedic dtParamedicCheck = BusinessLayer.GetPatientChargesDtParamedic(Convert.ToInt32(hdnChargesDtID.Value), pmt.ParamedicID, entity.ItemID);
                    if (dtParamedicCheck == null)
                    {
                        PatientChargesDtParamedic dtparamedic = new PatientChargesDtParamedic();
                        dtparamedic.ID = Convert.ToInt32(hdnChargesDtID.Value);
                        dtparamedic.ItemID = entity.ItemID;
                        dtparamedic.ParamedicID = pmt.ParamedicID;
                        dtparamedic.GCParamedicRole = pmt.GCParamedicRole;
                        dtparamedic.RevenueSharingID = pmt.RevenueSharingID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtParamedicDao.Insert(dtparamedic);
                    }
                }

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

        private bool OnDeleteRecordBarang(ref string errMessage)
        {
            try
            {
                PatientChargesDtPackage entity = BusinessLayer.GetPatientChargesDtPackage(Convert.ToInt32(hdnIDBarang.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientChargesDtPackage(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

    }
}