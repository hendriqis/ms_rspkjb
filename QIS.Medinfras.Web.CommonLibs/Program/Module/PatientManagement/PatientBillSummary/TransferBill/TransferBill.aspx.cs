using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransferBill : BasePageTrx
    {
        private string pageTitle = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string GetPageTitle()
        {
            return pageTitle;
        }

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_TRANSFER_FROM_OTHER_UNIT;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.BILL_TRANSFER_FROM_OTHER_UNIT;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_TRANSFER_FROM_OTHER_UNIT;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_TRANSFER_FROM_OTHER_UNIT;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_TRANSFER_FROM_OTHER_UNIT;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_TRANSFER_FROM_OTHER_UNIT;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_TRANSFER_FROM_OTHER_UNIT;
                default: return Constant.MenuCode.Inpatient.BILL_TRANSFER_FROM_OTHER_UNIT;
            }
        }

        protected override void InitializeDataControl()
        {
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(string.Format(
                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                    AppSession.UserLogin.HealthcareID, //0
                    Constant.SettingParameter.FN_TRANSFER_TAGIHAN_KELAS_TERTINGGI, //1
                    Constant.SettingParameter.IP_IS_BLOCK_TRANSFER_BILL, //2
                    Constant.SettingParameter.FN_IS_PATIENT_TRANSFER_USED_DIFFERENT_CUSTOMER_BLOCK, //3
                    Constant.SettingParameter.FN_IS_PATIENT_TRANSFER_USED_HAS_DOWN_PAYMENT_BLOCK //4
                ));

            hdnIsTransferUsedHighestClass.Value = lstParam.FirstOrDefault(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_TRANSFER_TAGIHAN_KELAS_TERTINGGI).ParameterValue;
            hdnIsUsedBillTransferBlock.Value = lstParam.FirstOrDefault(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IP_IS_BLOCK_TRANSFER_BILL).ParameterValue;
            hdnIsUsedDifferentCustomerBlock.Value = lstParam.FirstOrDefault(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_PATIENT_TRANSFER_USED_DIFFERENT_CUSTOMER_BLOCK).ParameterValue;
            hdnIsUsedHasDownPaymentBlock.Value = lstParam.FirstOrDefault(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_PATIENT_TRANSFER_USED_HAS_DOWN_PAYMENT_BLOCK).ParameterValue;

            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnLinkedRegistrationID.Value = AppSession.RegisteredPatient.LinkedRegistrationID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            pageTitle = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (!String.IsNullOrEmpty(hdnRegistrationID.Value))
            {
                bool isHasOutstandingAR = false, isHasDifferentCustomer = false, isHasDownPayment = false, isHasPatientBill = false;
                int isHasDifferentCustomerCount = 0, isHasDownPaymentCount = 0;

                Registration registration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
                hdnMRN.Value = Convert.ToString(registration.MRN);

                string filter = string.Format("MRN = {0}", registration.MRN);
                List<vPatientHasOutstandingInvoicePayment> lstOutstanding = BusinessLayer.GetvPatientHasOutstandingInvoicePaymentList(filter);
                if (lstOutstanding.Count > 0)
                {
                    isHasOutstandingAR = true;
                }

                List<Registration> linkedRegList = BusinessLayer.GetRegistrationList(string.Format("LinkedToRegistrationID = {0} AND GCRegistrationStatus != '{1}'", hdnRegistrationID.Value, Constant.VisitStatus.CANCELLED));
                foreach (Registration linkedReg in linkedRegList)
                {
                    if (linkedReg.BusinessPartnerID != registration.BusinessPartnerID)
                    {
                        isHasDifferentCustomerCount += 1;
                    }

                    string filterBill = string.Format("RegistrationID IN (SELECT lfr.RegistrationID FROM Registration lfr WITH(NOLOCK) WHERE lfr.LinkedToRegistrationID = {0}) AND GCTransactionStatus NOT IN ('{1}','{2}')", hdnRegistrationID.Value, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                    List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(filterBill);
                    if (lstPatientBill.Count > 0)
                    {
                        hdnIsHasPatientBill.Value = "1";
                        isHasPatientBill = true;
                    }

                    string filterDP = string.Format("RegistrationID = {0} AND GCPaymentType = '{1}' AND GCTransactionStatus != '{2}'", linkedReg.RegistrationID, Constant.PaymentType.DOWN_PAYMENT, Constant.TransactionStatus.VOID);
                    List<PatientPaymentHd> lstPaymentDP = BusinessLayer.GetPatientPaymentHdList(filterDP);
                    if (lstPaymentDP.Count > 0)
                    {
                        isHasDownPaymentCount += 1;
                    }
                }

                if (isHasDifferentCustomerCount > 0)
                {
                    isHasDifferentCustomer = true;
                    hdnIsDifferentCustomer.Value = "1";
                }
                else
                {
                    isHasDifferentCustomer = false;
                    hdnIsDifferentCustomer.Value = "0";
                }

                if (isHasDownPaymentCount > 0)
                {
                    isHasDownPayment = true;
                    hdnIsHasDownPayment.Value = "1";
                }
                else
                {
                    isHasDownPayment = false;
                    hdnIsHasDownPayment.Value = "0";
                }

                if (isHasOutstandingAR || isHasDifferentCustomer || isHasDownPayment || isHasPatientBill)
                {
                    trWarning.Style.Remove("display");

                    if (isHasPatientBill)
                    {
                        lblWarningHasPatientBill.Style.Remove("display");
                    }
                    else
                    {
                        lblWarningHasPatientBill.Style.Add("display", "none");
                    }

                    if (isHasOutstandingAR)
                    {
                        lblWarningOutstandingAR.Style.Remove("display");
                    }
                    else
                    {
                        lblWarningOutstandingAR.Style.Add("display", "none");
                    }


                    if (isHasDifferentCustomer)
                    {
                        lblWarningDifferentCustomer.Style.Remove("display");
                    }
                    else
                    {
                        lblWarningDifferentCustomer.Style.Add("display", "none");
                    }


                    if (isHasDownPayment)
                    {
                        lblWarningHasDownPayment.Style.Remove("display");
                    }
                    else
                    {
                        lblWarningHasDownPayment.Style.Add("display", "none");
                    }
                }
                else
                {
                    trWarning.Style.Add("display", "none");
                }
            }

            BindGrid();

        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGrid();
        }


        private string GetFilterExpression()
        {
            string filterExpression = "";
            filterExpression = string.Format("(LinkedToRegistrationID = {0} AND IsChargesTransfered = 0) AND GCTransactionStatus NOT IN ('{1}','{2}') AND IsDeleted = 0",
                                                hdnRegistrationID.Value, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);

            return filterExpression;
        }

        private void BindGrid()
        {
            string filterExpression = GetFilterExpression();
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(filterExpression);
            if (lst.Count > 0) hdnRowCountData.Value = lst.Count.ToString();
            else hdnRowCountData.Value = "0";

            List<vPatientChargesDt8> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE || p.GCItemType == Constant.ItemGroupMaster.LABORATORY
                                                            || p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY || p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            ctlService.HideCheckBox();
            ctlService.BindGrid(lstService);

            List<vPatientChargesDt8> lstDrugMS = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ctlDrugMS.HideCheckBox();
            ctlDrugMS.BindGrid(lstDrugMS);

            List<vPatientChargesDt8> lstLogistic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ctlLogistic.HideCheckBox();
            ctlLogistic.BindGrid(lstLogistic);
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            SettingParameterDao entitySettingParameterDao = new SettingParameterDao(ctx);
            ItemMasterDao entityItemMasterDao = new ItemMasterDao(ctx);
            ItemServiceDao entityItemServiceDao = new ItemServiceDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            ConsultVisitDao consVisitDao = new ConsultVisitDao(ctx);
            TestOrderHdDao testOrderHdDao = new TestOrderHdDao(ctx);
            PatientBillDao patientBillDao = new PatientBillDao(ctx);

            if (type == "transferbill")
            {
                try
                {
                    if (hdnIsUsedDifferentCustomerBlock.Value == "1" && hdnIsDifferentCustomer.Value == "1")
                    {
                        result = false;
                        errMessage = "Tidak dapat proses Transfer Tagihan karena penjamin bayar di registrasi asal berbeda dengan penjamin bayar registrasi ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    else if (hdnIsUsedHasDownPaymentBlock.Value == "1" && hdnIsHasDownPayment.Value == "1")
                    {
                        result = false;
                        errMessage = "Tidak dapat proses Transfer Tagihan karena registrasi asal memiliki uang muka.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    else
                    {
                        #region Process Transfer Bill

                        bool flagHighestClass = hdnIsTransferUsedHighestClass.Value == "1";

                        decimal transferAmount = 0, allTransferAmount = 0;

                        Registration entity = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                        List<Registration> lstFromReg = BusinessLayer.GetRegistrationList(string.Format("LinkedToRegistrationID = {0} AND GCRegistrationStatus != '{1}'", entity.RegistrationID, Constant.VisitStatus.CANCELLED), ctx);
                        foreach (Registration entityLinked in lstFromReg)
                        {
                            List<vConsultVisit11> entityVisit = BusinessLayer.GetvConsultVisit11List(string.Format("RegistrationID IN ({0},{1})", entityLinked.RegistrationID, entityLinked.LinkedToRegistrationID), ctx);
                            vConsultVisit11 entityVisitTo = entityVisit.Where(t => t.RegistrationID == entityLinked.LinkedToRegistrationID).FirstOrDefault();
                            vConsultVisit11 entityVisitFrom = entityVisit.Where(t => t.RegistrationID == entityLinked.RegistrationID).FirstOrDefault();

                            //Mengapa ini perlu di VOID ?? jika suatu saat ada error ini di buang saja
                            string filterExpression = string.Format("VisitID IN (SELECT VisitID FROM ConsultVisit WHERE RegistrationID = {0}) AND GCTransactionStatus NOT IN ('{1}','{2}')", entityLinked.RegistrationID, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.VOID);
                            List<TestOrderHd> lstTestOrderHd = BusinessLayer.GetTestOrderHdList(filterExpression, ctx);
                            foreach (TestOrderHd testOrderHd in lstTestOrderHd)
                            {
                                testOrderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                testOrderHdDao.Update(testOrderHd);
                            }

                            filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}','{3}')", entityLinked.RegistrationID, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                            List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(filterExpression, ctx);
                            foreach (PatientBill patientBill in lstPatientBill)
                            {
                                patientBill.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                                patientBill.LastUpdatedDate = DateTime.Now;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                patientBillDao.Update(patientBill);
                            }

                            if (lstPatientBill.Count > 0)
                            {
                                String listBillingIDVoid = string.Join(",", lstPatientBill.Select(t => t.PatientBillingID));
                                List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID IN ({0})", listBillingIDVoid), ctx);
                                foreach (PatientChargesHd patientChargesHd in lstPatientChargesHd)
                                {
                                    patientChargesHd.PatientBillingID = null;
                                    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    patientChargesHdDao.Update(patientChargesHd);
                                }
                            }

                            string filter = string.Empty;

                            #region Update Tarif UGD Ke Class Tertinggi
                            if (flagHighestClass && (entityVisitTo.ClassPriority > entityVisitFrom.ClassPriority))
                            {
                                filter = string.Format("TransactionID IN (SELECT TransactionID FROM PatientChargesHd WHERE VisitID = {0})", entityVisitFrom.VisitID);
                                List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(filter, ctx);
                                foreach (PatientChargesDt entPDt in lstPatientChargesDt)
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    string GCItemType = entityItemMasterDao.Get(entPDt.ItemID).GCItemType;
                                    entPDt.ChargeClassID = entityVisitTo.ChargeClassID;
                                    if (GCItemType == Constant.ItemGroupMaster.SERVICE || GCItemType == Constant.ItemGroupMaster.RADIOLOGY || GCItemType == Constant.ItemGroupMaster.LABORATORY || GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC)
                                    {
                                        int testPartner = 0;
                                        if (entPDt.BusinessPartnerID != null) testPartner = Convert.ToInt32(entPDt.BusinessPartnerID);
                                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(entityVisitFrom.RegistrationID, entityVisitFrom.VisitID, entityVisitTo.ChargeClassID, entPDt.ItemID, 1, entityVisitFrom.VisitDate, ctx, testPartner);

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

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        ItemService entityIS = entityItemServiceDao.Get(entPDt.ItemID);

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

                                        decimal qty = entPDt.BaseQuantity;
                                        decimal grossLineAmount = qty * price;

                                        entPDt.BaseTariff = basePrice;
                                        entPDt.Tariff = price;
                                        entPDt.BaseComp1 = basePriceComp1;
                                        entPDt.BaseComp2 = basePriceComp2;
                                        entPDt.BaseComp3 = basePriceComp3;
                                        entPDt.TariffComp1 = priceComp1;
                                        entPDt.TariffComp2 = priceComp2;
                                        entPDt.TariffComp3 = priceComp3;

                                        if (entPDt.IsCITO)
                                        {
                                            if (entPDt.IsCITOInPercentage)
                                            {
                                                entPDt.CITOAmount = entPDt.CITOAmount * grossLineAmount / 100;
                                            }
                                            grossLineAmount += entPDt.CITOAmount;
                                        }

                                        if (entPDt.IsComplication)
                                        {
                                            if (entPDt.IsComplicationInPercentage)
                                            {
                                                entPDt.ComplicationAmount = entPDt.ComplicationAmount * grossLineAmount / 100;
                                            }
                                            grossLineAmount += entPDt.ComplicationAmount;
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
                                                        entPDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                                        entPDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                                        entPDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                                    entPDt.DiscountPercentageComp1 = discountAmount;
                                                }

                                                if (priceComp2 > 0)
                                                {
                                                    totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                    entPDt.DiscountPercentageComp2 = discountAmount;
                                                }

                                                if (priceComp3 > 0)
                                                {
                                                    totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                    entPDt.DiscountPercentageComp3 = discountAmount;
                                                }
                                            }

                                            if (entPDt.DiscountPercentageComp1 > 0)
                                            {
                                                entPDt.IsDiscountInPercentageComp1 = true;
                                            }

                                            if (entPDt.DiscountPercentageComp2 > 0)
                                            {
                                                entPDt.IsDiscountInPercentageComp2 = true;
                                            }

                                            if (entPDt.DiscountPercentageComp3 > 0)
                                            {
                                                entPDt.IsDiscountInPercentageComp3 = true;
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

                                        if (grossLineAmount > 0)
                                        {
                                            if (totalDiscountAmount > grossLineAmount)
                                            {
                                                totalDiscountAmount = grossLineAmount;
                                            }
                                        }

                                        entPDt.DiscountAmount = totalDiscountAmount;

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

                                        entPDt.IsDiscount = totalDiscountAmount != 0;
                                        entPDt.DiscountAmount = totalDiscountAmount;
                                        entPDt.DiscountComp1 = totalDiscountAmount1;
                                        entPDt.DiscountComp2 = totalDiscountAmount2;
                                        entPDt.DiscountComp3 = totalDiscountAmount3;

                                        entPDt.PayerAmount = totalPayer;
                                        entPDt.PatientAmount = total - totalPayer;
                                        entPDt.LineAmount = total;
                                        entPDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    }
                                    else
                                    {
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        ItemMaster entityItemMaster = entityItemMasterDao.Get(entPDt.ItemID);
                                        int typeItem = 2;
                                        if (entityItemMaster.GCItemType == Constant.ItemGroupMaster.LOGISTIC)
                                        {
                                            typeItem = 3;
                                        }
                                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(entityVisitFrom.RegistrationID, entityVisitFrom.VisitID, entityVisitTo.ChargeClassID, entPDt.ItemID, typeItem, entityVisitFrom.VisitDate, ctx);

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

                                        decimal qty = entPDt.ChargedQuantity * entPDt.ConversionFactor;
                                        decimal grossLineAmount = qty * price;
                                        entPDt.TariffComp1 = entPDt.Tariff = price;
                                        entPDt.BaseComp1 = entPDt.BaseTariff = basePrice;

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
                                                        entPDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                                        entPDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                                        entPDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                                    entPDt.DiscountPercentageComp1 = discountAmount;
                                                }

                                                if (priceComp2 > 0)
                                                {
                                                    totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                                    entPDt.DiscountPercentageComp2 = discountAmount;
                                                }

                                                if (priceComp3 > 0)
                                                {
                                                    totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                                    entPDt.DiscountPercentageComp3 = discountAmount;
                                                }
                                            }

                                            if (entPDt.DiscountPercentageComp1 > 0)
                                            {
                                                entPDt.IsDiscountInPercentageComp1 = true;
                                            }

                                            if (entPDt.DiscountPercentageComp2 > 0)
                                            {
                                                entPDt.IsDiscountInPercentageComp2 = true;
                                            }

                                            if (entPDt.DiscountPercentageComp3 > 0)
                                            {
                                                entPDt.IsDiscountInPercentageComp3 = true;
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

                                        if (grossLineAmount > 0)
                                        {
                                            if (totalDiscountAmount > grossLineAmount)
                                            {
                                                totalDiscountAmount = grossLineAmount;
                                            }
                                        }

                                        entPDt.DiscountAmount = totalDiscountAmount;

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

                                        entPDt.IsDiscount = totalDiscountAmount != 0;
                                        entPDt.DiscountAmount = totalDiscountAmount;
                                        entPDt.DiscountComp1 = totalDiscountAmount1;
                                        entPDt.DiscountComp2 = totalDiscountAmount2;
                                        entPDt.DiscountComp3 = totalDiscountAmount3;

                                        entPDt.PayerAmount = totalPayer;
                                        entPDt.PatientAmount = total - totalPayer;
                                        entPDt.LineAmount = total;
                                        entPDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    }
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    patientChargesDtDao.Update(entPDt);
                                }
                            }
                            #endregion

                            transferAmount = (entityLinked.ChargesAmount + entityLinked.AdminAmount - entityLinked.DiscountAmount - entityLinked.PaymentAmount + entityLinked.DownPaymentAmount);
                            allTransferAmount += transferAmount;
                            entityLinked.TransferAmount = transferAmount;

                            if (entityVisitFrom.DepartmentID != Constant.Facility.INPATIENT)
                            {
                                entityLinked.ClosedBy = AppSession.UserLogin.UserID;
                                entityLinked.ClosedDate = DateTime.Now;
                                entityLinked.ClosedTime = string.Format("{0}:{1}", DateTime.Now.Hour, DateTime.Now.Minute);

                                entityLinked.GCRegistrationStatus = Constant.VisitStatus.CLOSED;
                            }

                            entityLinked.IsChargesTransfered = true;
                            entityLinked.IsLockDown = false;
                            entityLinked.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            registrationDao.Update(entityLinked);

                            filter = string.Format("RegistrationID = {0}", entityLinked.RegistrationID);
                            List<ConsultVisit> entConsultList = BusinessLayer.GetConsultVisitList(filter, ctx);
                            foreach (ConsultVisit entConsult in entConsultList)
                            {
                                filter = string.Format("VisitID = {0} AND GCTransactionStatus NOT IN ('{1}', '{2}')", entConsult.VisitID, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                                List<PatientChargesHd> lstPatientChargesHdFinal = BusinessLayer.GetPatientChargesHdList(filter, ctx);
                                foreach (PatientChargesHd enPch in lstPatientChargesHdFinal)
                                {
                                    enPch.IsChargesTransfered = true;
                                    enPch.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    patientChargesHdDao.Update(enPch);
                                }

                                if (entityVisitFrom.DepartmentID != Constant.Facility.INPATIENT)
                                {
                                    entConsult.GCVisitStatus = Constant.VisitStatus.CLOSED;
                                    entConsult.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    consVisitDao.Update(entConsult);
                                }
                            }

                        }

                        if (entity.IsLockDown)
                        {
                            entity.IsLockDown = false;
                        }
                        entity.SourceAmount = allTransferAmount;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        registrationDao.Update(entity);

                        if (hdnIsHasPatientBill.Value != "1")
                        {
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("sudah ada tagihan di registrasi asal");
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }

                        #endregion
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
            }
            return result;
        }
    }
}