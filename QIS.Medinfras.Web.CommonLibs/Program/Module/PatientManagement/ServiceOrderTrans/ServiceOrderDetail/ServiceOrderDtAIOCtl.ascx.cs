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
    public partial class ServiceOrderDtAIOCtl : BaseViewPopupCtl
    {
        public int PageCount = 1;
        public int CurrPage = 1;

        protected string filterExpressionSupplier = "";

        public override void InitializeDataControl(string param)
        {
            GetSettingParameter();
            tblVoidReason.Visible = hdnIsAllowVoid.Value == "1";

            string[] paramSplit = param.Split('|');
            hdnServiceOrderID.Value = paramSplit[0];
            hdnTransactionID.Value = paramSplit[1];
            hdnGCTransactionStatus.Value = paramSplit[2];

            vServiceOrderHdVisit entity = BusinessLayer.GetvServiceOrderHdVisitList(string.Format("ServiceOrderID = {0}", hdnServiceOrderID.Value)).FirstOrDefault();
            hdnLinkedChargesID.Value = entity.LinkedChargesID.ToString();
            hdnVisitID.Value = entity.VisitID.ToString();
            hdnClassID.Value = BusinessLayer.GetConsultVisit(entity.VisitID).ChargeClassID.ToString();
            hdnRegistrationID.Value = entity.RegistrationID.ToString();

            txtTransactionNo.Text = entity.ServiceOrderNo;
            txtServiceOrderDate.Text = entity.ServiceOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtServiceOrderTime.Text = entity.ServiceOrderTime;

            txtOrderUser.Text = entity.CreatedByName;
            txtOrderPhysicianName.Text = string.Format("{0} ({1})", entity.ParamedicName, entity.ParamedicCode);
            txtNotes.Text = entity.Remarks;

            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            hdnDepartmentID.Value = entity.OrderDepartmentID;

            //PatientChargesHd patientChargesHd = BusinessLayer.GetPatientChargesHdList(GetServiceOrderTransactionFilterExpression()).FirstOrDefault();
            //if (patientChargesHd != null)
            //{
            //    hdnTransactionID.Value = patientChargesHd.TransactionID.ToString();
            //}

            txtRealizationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRealizationTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            if (hdnDefaultParamedicID.Value != "")
            {
                ParamedicMaster oParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID  = {0}", hdnDefaultParamedicID.Value)).FirstOrDefault();
                if (oParamedic != null)
                {
                    hdnPhysicianID.Value = oParamedic.ParamedicID.ToString();
                    txtPhysicianCode.Text = oParamedic.ParamedicCode;
                    txtPhysicianName.Text = oParamedic.FullName;
                }
            }
            else
            {
                List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND IsDeleted = 0", hdnHealthcareServiceUnitID.Value));
                if (lstParamedic.Count == 1)
                {
                    ParamedicMaster paramedic = lstParamedic.FirstOrDefault();
                    hdnPhysicianID.Value = paramedic.ParamedicID.ToString();
                    txtPhysicianCode.Text = paramedic.ParamedicCode;
                    txtPhysicianName.Text = paramedic.FullName;
                }
                else
                {
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.OtherMedicalDiagnostic)
                    {
                        hdnPhysicianID.Value = entity.ParamedicID.ToString();
                        txtPhysicianCode.Text = entity.ParamedicCode;
                        txtPhysicianName.Text = entity.ParamedicName;
                    }
                }
            }

            Helper.SetControlEntrySetting(txtServiceOrderDate, new ControlEntrySetting(true, true, true), "mpEntry");
            Helper.SetControlEntrySetting(txtServiceOrderTime, new ControlEntrySetting(true, true, true), "mpEntry");
            Helper.SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true), "mpEntry");

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON));
            Methods.SetComboBoxField<StandardCode>(cboVoidReason, lstSc, "StandardCodeName", "StandardCodeID");
            cboVoidReason.SelectedIndex = 0;

            BindGridView(CurrPage, true, ref PageCount);
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                AppSession.UserLogin.HealthcareID,
                Constant.SettingParameter.SA_KONTROL_PEMBATALAN_ORDER));

            if (lstSettingParameterDt.Count > 0)
            {
                hdnIsAllowVoid.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.SA_KONTROL_PEMBATALAN_ORDER).FirstOrDefault().ParameterValue == "1" ? "0" : "1";
                hdnDefaultParamedicID.Value = "";
            }
            else
            {
                hdnIsAllowVoid.Value = "0";
            }
        }

        protected string GetServiceOrderTransactionFilterExpression()
        {
            return string.Format("HealthcareServiceUnitID = {0} AND ServiceorderID = {1} AND GCTransactionStatus = '{2}'", hdnHealthcareServiceUnitID.Value, hdnServiceOrderID.Value, Constant.TransactionStatus.OPEN);
        }

        private string GetFilterExpression()
        {
            return string.Format("ServiceOrderID = {0} AND IsDeleted = 0", hdnServiceOrderID.Value);
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                vServiceOrderDt entity = e.Row.DataItem as vServiceOrderDt;
                TextBox txtQty = e.Row.FindControl("txtQty") as TextBox;
                CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsSelected");

                if (entity.GCServiceOrderStatus != Constant.TestOrderStatus.OPEN)
                {
                    chkIsSelected.Visible = false;
                }

                if (!String.IsNullOrEmpty(hdnTransactionID.Value) && hdnTransactionID.Value != "0")
                {
                    string filter = string.Format("TransactionID = {0} AND ItemID = {1} AND IsDeleted = 0", hdnTransactionID.Value, entity.ItemID);
                    List<PatientChargesDt> lstChargesDt = BusinessLayer.GetPatientChargesDtList(filter);
                    decimal qty = 0;
                    foreach (PatientChargesDt c in lstChargesDt)
                    {
                        qty = qty + c.ChargedQuantity;
                    }
                    txtQty.Text = qty.ToString();
                }
                else
                {
                    txtQty.Text = entity.RemainingQty.ToString();
                }
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            filterExpression += " ORDER BY ItemName1 ASC";
            List<vServiceOrderDt> lstEntity = BusinessLayer.GetvServiceOrderDtList(filterExpression);

            if (lstEntity.Count > 0)
            {
                //if (lstEntity.Where(p => p.GCServiceOrderStatus == Constant.ServiceOrderStatus.OPEN).Count() < 1)
                //{
                //    trTransactionDateTime.Style.Add("display", "none");
                //    trTransactionParamedic.Style.Add("display", "none");
                //    tblApproveDecline.Style.Add("display", "none");
                //    tblVoidReason.Style.Add("display", "none");
                //}
                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                else
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }


        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string retval = "";
            string result = param + "|";
            string errMessage = "";

            if (param == "approve")
            {
                if (OnApproveRecord(ref errMessage, ref retval))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            else if (param == "decline")
            {
                if (OnDeclineRecord(ref errMessage))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            else if (param == "close")
            {
                //if (OnCloseRecord(ref errMessage))
                result += "success";
                //else
                //    result += "fail|" + errMessage;
            }

            BindGridView(CurrPage, true, ref PageCount);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }

        private bool OnApproveRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            ServiceOrderDtDao ServiceOrderDtDao = new ServiceOrderDtDao(ctx);
            ServiceOrderHdDao ServiceOrderHdDao = new ServiceOrderHdDao(ctx);
            try
            {
                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                int visitID = Convert.ToInt32(hdnVisitID.Value);
                if (!string.IsNullOrEmpty(hdnListServiceOrderDtID.Value))
                {
                    List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
                    int ct = 0;
                    string[] tempListOrderDt = hdnListServiceOrderDtID.Value.Split(',');
                    string[] tempListIsCito = hdnListIsCito.Value.Split(',');
                    string[] tempListServiceSupplier = hdnListServicePartnerID.Value.Split(',');
                    string[] tempListQty = hdnListQty.Value.Split(',');
                    foreach (string orderID in tempListOrderDt)
                    {
                        ServiceOrderDt ServiceDt = ServiceOrderDtDao.Get(Convert.ToInt32(orderID));
                        if (ServiceDt.GCServiceOrderStatus == Constant.TestOrderStatus.CANCELLED || ServiceDt.GCServiceOrderStatus == Constant.TestOrderStatus.IN_PROGRESS)
                        {
                            result = false;
                            errMessage = "Order sudah diproses oleh user lain, silahkan merefresh halaman ini";
                            break;
                        }

                        decimal qtyRemaining = (ServiceDt.ItemQty - ServiceDt.ChargeAIOQty);
                        ServiceDt.ChargeAIOQty = ServiceDt.ChargeAIOQty + Convert.ToDecimal(tempListQty[ct]);
                        if (ServiceDt.ChargeAIOQty > ServiceDt.ItemQty)
                        {
                            result = false;
                            errMessage = string.Format("Qty sudah melewati batas yang diperbolehkan. Qty yang masih bisa diproses adalah {0}", qtyRemaining);
                        }
                        else
                        {
                            if (ServiceDt.ChargeAIOQty == ServiceDt.ItemQty)
                            {
                                ServiceDt.GCServiceOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                            }
                        }
                        ServiceDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ServiceOrderDtDao.Update(ServiceDt);

                        PatientChargesDt patientChargesDt = new PatientChargesDt();
                        patientChargesDt.ItemID = ServiceDt.ItemID;
                        patientChargesDt.ChargeClassID = Convert.ToInt32(hdnClassID.Value);
                        patientChargesDt.ItemPackageID = ServiceDt.ItemPackageID;
                        patientChargesDt.ReferenceDtID = ServiceDt.ID;

                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, patientChargesDt.ChargeClassID, ServiceDt.ItemID, 1, DateTime.Now, ctx);

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

                        vItemService entityItemMaster = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", ServiceDt.ItemID)).FirstOrDefault();
                        patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                        patientChargesDt.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);

                        if (patientChargesDt.BusinessPartnerID != null)
                        {
                            patientChargesDt.IsSubContractItem = true;
                        }
                        else
                        {
                            patientChargesDt.IsSubContractItem = false;
                        }

                        patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visitID, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Helper.GetDatePickerValue(txtRealizationDate.Text), txtRealizationTime.Text).FirstOrDefault().RevenueSharingID;
                        if (patientChargesDt.RevenueSharingID == 0)
                            patientChargesDt.RevenueSharingID = null;
                        patientChargesDt.IsVariable = false;
                        patientChargesDt.IsUnbilledItem = false;

                        decimal grossLineAmount = 1 * price;

                        patientChargesDt.IsCITO = false;
                        patientChargesDt.CITOAmount = 0;
                        patientChargesDt.BaseCITOAmount = entityItemMaster.CITOAmount;
                        patientChargesDt.IsCITOInPercentage = entityItemMaster.IsCITOInPercentage;
                        if (tempListIsCito[ct].Equals("1"))
                        {
                            patientChargesDt.IsCITO = true;
                            if (entityItemMaster.IsCITOInPercentage)
                                patientChargesDt.CITOAmount = (entityItemMaster.CITOAmount * grossLineAmount) / 100;
                            else
                                patientChargesDt.CITOAmount = entityItemMaster.CITOAmount;
                            grossLineAmount += patientChargesDt.CITOAmount;
                        }

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

                        totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (1);

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
                            totalPayer = coverageAmount * 1;
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

                        patientChargesDt.IsComplication = false;
                        patientChargesDt.ComplicationAmount = 0;

                        patientChargesDt.IsDiscount = totalDiscountAmount != 0;
                        patientChargesDt.DiscountAmount = totalDiscountAmount;
                        patientChargesDt.DiscountComp1 = totalDiscountAmount1;
                        patientChargesDt.DiscountComp2 = totalDiscountAmount2;
                        patientChargesDt.DiscountComp3 = totalDiscountAmount3;

                        patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = Convert.ToDecimal(tempListQty[ct]);
                        patientChargesDt.PatientAmount = total - totalPayer;
                        patientChargesDt.PayerAmount = totalPayer;
                        patientChargesDt.LineAmount = total;
                        patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                        patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        lstPatientChargesDt.Add(patientChargesDt);
                        ct++;
                    }

                    if (result)
                    {
                        #region Patient Charges
                        PatientChargesHd patientChargesHd = null;
                        if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0" || patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value)).GCTransactionStatus != Constant.TransactionStatus.OPEN)
                        {
                            patientChargesHd = new PatientChargesHd();
                            patientChargesHd.VisitID = visitID;
                            //if (hdnLinkedChargesID.Value != "0" && hdnLinkedChargesID.Value != "")
                            //    patientChargesHd.LinkedChargesID = Convert.ToInt32(hdnLinkedChargesID.Value);
                            //else
                            //    patientChargesHd.LinkedChargesID = null;
                            patientChargesHd.LinkedChargesID = null;
                            patientChargesHd.ServiceOrderID = Convert.ToInt32(hdnServiceOrderID.Value);
                            patientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                            switch (hdnDepartmentID.Value)
                            {
                                case Constant.Facility.INPATIENT: patientChargesHd.TransactionCode = Constant.TransactionCode.IP_CHARGES; break;
                                case Constant.Facility.EMERGENCY: patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                                case Constant.Facility.DIAGNOSTIC:
                                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                        patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                        patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                                    else
                                        patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES; break;
                                default: patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
                            }
                            patientChargesHd.TransactionDate = Helper.GetDatePickerValue(txtRealizationDate.Text);
                            patientChargesHd.TransactionTime = txtRealizationTime.Text;
                            patientChargesHd.PatientBillingID = null;
                            patientChargesHd.ReferenceNo = "";
                            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            patientChargesHd.GCVoidReason = null;
                            patientChargesHd.TotalPatientAmount = 0;
                            patientChargesHd.TotalPayerAmount = 0;
                            patientChargesHd.TotalAmount = 0;
                            patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                            patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
                        }
                        else
                        {
                            patientChargesHd = patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                        }
                        retval = patientChargesHd.TransactionNo;
                        foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                        {
                            patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                            int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                            string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                            List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
                            foreach (vItemServiceDt isd in isdList)
                            {
                                PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                                dtpackage.PatientChargesDtID = ID;
                                dtpackage.ItemID = isd.DetailItemID;
                                dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                                int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, patientChargesHd.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                                if (revID != 0 && revID != null)
                                {
                                    dtpackage.RevenueSharingID = revID;
                                }
                                else
                                {
                                    dtpackage.RevenueSharingID = null;
                                }

                                dtpackage.ChargedQuantity = (isd.Quantity * patientChargesDt.ChargedQuantity);

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

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
                                decimal grossLineAmount = 0;

                                GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, patientChargesHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

                                basePrice = tariff.BasePrice;
                                basePriceComp1 = tariff.BasePriceComp1;
                                basePriceComp2 = tariff.BasePriceComp2;
                                basePriceComp3 = tariff.BasePriceComp3;
                                price = tariff.Price;
                                priceComp1 = tariff.PriceComp1;
                                priceComp2 = tariff.PriceComp2;
                                priceComp3 = tariff.PriceComp3;
                                isDiscountUsedComp = tariff.IsDiscountUsedComp;
                                discountAmount = tariff.DiscountAmount;
                                discountAmountComp1 = tariff.DiscountAmountComp1;
                                discountAmountComp2 = tariff.DiscountAmountComp2;
                                discountAmountComp3 = tariff.DiscountAmountComp3;
                                coverageAmount = tariff.CoverageAmount;
                                isDiscountInPercentage = tariff.IsDiscountInPercentage;
                                isDiscountInPercentageComp1 = tariff.IsDiscountInPercentageComp1;
                                isDiscountInPercentageComp2 = tariff.IsDiscountInPercentageComp2;
                                isDiscountInPercentageComp3 = tariff.IsDiscountInPercentageComp3;
                                isCoverageInPercentage = tariff.IsCoverageInPercentage;
                                costAmount = tariff.CostAmount;
                                grossLineAmount = dtpackage.ChargedQuantity * price;
                                
                                dtpackage.BaseTariff = tariff.BasePrice;
                                dtpackage.BaseComp1 = tariff.BasePriceComp1;
                                dtpackage.BaseComp2 = tariff.BasePriceComp2;
                                dtpackage.BaseComp3 = tariff.BasePriceComp3;
                                dtpackage.Tariff = tariff.Price;
                                dtpackage.TariffComp1 = tariff.PriceComp1;
                                dtpackage.TariffComp2 = tariff.PriceComp2;
                                dtpackage.TariffComp3 = tariff.PriceComp3;
                                dtpackage.CostAmount = tariff.CostAmount;

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
                                                dtpackage.DiscountPercentageComp1 = discountAmountComp1;
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
                                                dtpackage.DiscountPercentageComp2 = discountAmountComp2;
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
                                                dtpackage.DiscountPercentageComp3 = discountAmountComp3;
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
                                            dtpackage.DiscountPercentageComp1 = discountAmount;
                                        }

                                        if (priceComp2 > 0)
                                        {
                                            totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                            dtpackage.DiscountPercentageComp2 = discountAmount;
                                        }

                                        if (priceComp3 > 0)
                                        {
                                            totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                            dtpackage.DiscountPercentageComp3 = discountAmount;
                                        }
                                    }

                                    if (dtpackage.DiscountPercentageComp1 > 0)
                                    {
                                        dtpackage.IsDiscountInPercentageComp1 = true;
                                    }

                                    if (dtpackage.DiscountPercentageComp2 > 0)
                                    {
                                        dtpackage.IsDiscountInPercentageComp2 = true;
                                    }

                                    if (dtpackage.DiscountPercentageComp3 > 0)
                                    {
                                        dtpackage.IsDiscountInPercentageComp3 = true;
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

                                totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (dtpackage.ChargedQuantity);

                                if (grossLineAmount > 0)
                                {
                                    if (totalDiscountAmount > grossLineAmount)
                                    {
                                        totalDiscountAmount = grossLineAmount;
                                    }
                                }

                                dtpackage.DiscountAmount = totalDiscountAmount;
                                dtpackage.DiscountComp1 = totalDiscountAmount1;
                                dtpackage.DiscountComp2 = totalDiscountAmount2;
                                dtpackage.DiscountComp3 = totalDiscountAmount3;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                string filterIP = string.Format("ItemID = {0} AND IsDeleted = 0", isd.DetailItemID);
                                List<ItemPlanning> iplan = BusinessLayer.GetItemPlanningList(filterIP, ctx);
                                if (iplan.Count() > 0)
                                {
                                    dtpackage.AveragePrice = iplan.FirstOrDefault().AveragePrice;
                                }
                                else
                                {
                                    dtpackage.AveragePrice = 0;
                                }

                                dtpackage.CreatedBy = AppSession.UserLogin.UserID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityDtPackageDao.Insert(dtpackage);
                            }
                        }
                        #endregion

                        int ServiceOrderDtCount = BusinessLayer.GetServiceOrderDtRowCount(string.Format("ServiceOrderID = {0} AND GCServiceOrderStatus = '{1}' AND IsDeleted = 0", hdnServiceOrderID.Value, Constant.TestOrderStatus.OPEN), ctx);
                        if (ServiceOrderDtCount < 1)
                        {
                            ServiceOrderHd ServiceOrderHd = ServiceOrderHdDao.Get(Convert.ToInt32(hdnServiceOrderID.Value));
                            ServiceOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                            ServiceOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ServiceOrderHdDao.Update(ServiceOrderHd);
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    #region Patient Charges
                    PatientChargesHd patientChargesHd = null;
                    if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0" || patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value)).GCTransactionStatus != Constant.TransactionStatus.OPEN)
                    {
                        patientChargesHd = new PatientChargesHd();
                        patientChargesHd.VisitID = visitID;
                        //if (hdnLinkedChargesID.Value != "0" && hdnLinkedChargesID.Value != "")
                        //    patientChargesHd.LinkedChargesID = Convert.ToInt32(hdnLinkedChargesID.Value);
                        //else
                        //    patientChargesHd.LinkedChargesID = null;
                        patientChargesHd.LinkedChargesID = null;
                        patientChargesHd.ServiceOrderID = Convert.ToInt32(hdnServiceOrderID.Value);
                        patientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                        else
                            patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                        patientChargesHd.TransactionDate = Helper.GetDatePickerValue(txtRealizationDate.Text);
                        patientChargesHd.TransactionTime = txtRealizationTime.Text;
                        patientChargesHd.PatientBillingID = null;
                        patientChargesHd.ReferenceNo = "";
                        patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        patientChargesHd.GCVoidReason = null;
                        patientChargesHd.TotalPatientAmount = 0;
                        patientChargesHd.TotalPayerAmount = 0;
                        patientChargesHd.TotalAmount = 0;
                        patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                        patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
                    }
                    else
                    {
                        patientChargesHd = patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                    }
                    retval = patientChargesHd.TransactionNo;
                    #endregion

                    int ServiceOrderDtCount = BusinessLayer.GetServiceOrderDtRowCount(string.Format("ServiceOrderID = {0} AND GCServiceOrderStatus = '{1}' AND IsDeleted = 0", hdnServiceOrderID.Value, Constant.TestOrderStatus.OPEN), ctx);
                    if (ServiceOrderDtCount < 1)
                    {
                        ServiceOrderHd ServiceOrderHd = ServiceOrderHdDao.Get(Convert.ToInt32(hdnServiceOrderID.Value));
                        ServiceOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        ServiceOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ServiceOrderHdDao.Update(ServiceOrderHd);
                    }
                    ctx.CommitTransaction();
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

        private bool OnDeclineRecord(ref string errMessage)
        {
            bool result = true;

            if (String.IsNullOrEmpty(cboVoidReason.Value.ToString()))
            {
                result = false;
                errMessage = "Alasan Pembatalan harus diisi";
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            ServiceOrderDtDao ServiceOrderDtDao = new ServiceOrderDtDao(ctx);
            ServiceOrderHdDao ServiceOrderHdDao = new ServiceOrderHdDao(ctx);
            try
            {
                if (!string.IsNullOrEmpty(hdnListServiceOrderDtID.Value))
                {
                    String filterExpressionHd = String.Format("ServiceOrderID = {0} AND ID IN ({1})", hdnServiceOrderID.Value, hdnListServiceOrderDtID.Value);

                    List<ServiceOrderDt> lstServiceOrderDt = BusinessLayer.GetServiceOrderDtList(filterExpressionHd);
                    foreach (ServiceOrderDt ServiceDt in lstServiceOrderDt)
                    {
                        if (ServiceDt.GCServiceOrderStatus == Constant.TestOrderStatus.CANCELLED || ServiceDt.GCServiceOrderStatus == Constant.TestOrderStatus.IN_PROGRESS)
                        {
                            result = false;
                            errMessage = "Order sudah diproses oleh user lain, silahkan merefresh halaman ini";
                            break;
                        }
                        ServiceDt.GCServiceOrderStatus = Constant.TestOrderStatus.CANCELLED;
                        ServiceDt.GCVoidReason = cboVoidReason.Value.ToString();
                        if (ServiceDt.GCVoidReason == Constant.DeleteReason.OTHER)
                            ServiceDt.VoidReason = txtVoidReason.Text;
                        ServiceDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ServiceOrderDtDao.Update(ServiceDt);
                    }
                }
                if (result)
                {
                    List<ServiceOrderDt> lstServiceOrderDt = BusinessLayer.GetServiceOrderDtList(string.Format("ServiceOrderID = {0} AND IsDeleted = 0", hdnServiceOrderID.Value), ctx);
                    int ServiceOrderDtVoidCount = lstServiceOrderDt.Where(t => t.GCServiceOrderStatus == Constant.TestOrderStatus.CANCELLED).Count();
                    int ServiceOrderDtOpenCount = lstServiceOrderDt.Where(t => t.GCServiceOrderStatus == Constant.TestOrderStatus.OPEN).Count();
                    if (ServiceOrderDtOpenCount < 1)
                    {
                        ServiceOrderHd ServiceOrderHd = ServiceOrderHdDao.Get(Convert.ToInt32(hdnServiceOrderID.Value));
                        if (ServiceOrderDtVoidCount == lstServiceOrderDt.Count) ServiceOrderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        else ServiceOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        ServiceOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ServiceOrderHdDao.Update(ServiceOrderHd);
                    }
                    ctx.CommitTransaction();
                }
                else ctx.RollBackTransaction();
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

        private bool OnCloseRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceOrderHdDao ServiceOrderHdDao = new ServiceOrderHdDao(ctx);
            try
            {
                ServiceOrderHd ServiceOrderHd = ServiceOrderHdDao.Get(Convert.ToInt32(hdnServiceOrderID.Value));
                ServiceOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                ServiceOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                ServiceOrderHdDao.Update(ServiceOrderHd);
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