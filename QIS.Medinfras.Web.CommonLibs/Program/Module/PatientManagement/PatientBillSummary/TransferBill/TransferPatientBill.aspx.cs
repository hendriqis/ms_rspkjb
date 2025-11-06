using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransferPatientBill : BasePageTrx
    {
        protected string laboratoryTransactionCode = "";

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                default: return Constant.MenuCode.Inpatient.BILL_TRANSFER_FROM_OTHER_UNIT;
            }
        }

        protected string GetDepartmentPharmacy()
        {
            return Constant.Facility.PHARMACY;
        }

        protected string GetDepartmentMedicalCheckup()
        {
            return Constant.Facility.MEDICAL_CHECKUP;
        }

        protected override void InitializeDataControl()
        {
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            hdnLinkedRegistrationID.Value = entityVisit.LinkedRegistrationID.ToString();

            string filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}')", hdnLinkedRegistrationID.Value, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.VOID);
            int count = BusinessLayer.GetvTestOrderHdRowCount(filterExpression);
            if (count < 1)
            {
                filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}')", hdnLinkedRegistrationID.Value, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                count = BusinessLayer.GetPatientBillRowCount(filterExpression);
            }
            if (count < 1)
                tblInfoOutstandingBill.Style.Add("display", "none");
            hdnOutstandingCount.Value = count.ToString();

            hdnDepartmentID.Value = entityVisit.DepartmentID;

            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(string.Format("RegistrationID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}') AND IsDeleted = 0", entityVisit.LinkedRegistrationID, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID));
            BindGrid(lst);
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        private void BindGrid(List<vPatientChargesDt8> lst)
        {
            List<vPatientChargesDt8> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE).ToList();
            ((TransactionDtServiceViewCtl)ctlService).BindGrid(lstService);
            ((TransactionDtServiceViewCtl)ctlService).HideCheckBox();

            List<vPatientChargesDt8> lstDrugMS = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ((TransactionDtProductViewCtl)ctlDrugMS).BindGrid(lstDrugMS);
            ((TransactionDtProductViewCtl)ctlDrugMS).HideCheckBox();

            List<vPatientChargesDt8> lstLogistic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ((TransactionDtProductViewCtl)ctlLogistic).BindGrid(lstLogistic);
            ((TransactionDtProductViewCtl)ctlDrugMS).HideCheckBox();

            List<vPatientChargesDt8> lstLaboratory = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY).ToList();
            ((TransactionDtServiceViewCtl)ctlLaboratory).BindGrid(lstLaboratory);
            ((TransactionDtServiceViewCtl)ctlLaboratory).HideCheckBox();

            List<vPatientChargesDt8> lstImaging = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY).ToList();
            ((TransactionDtServiceViewCtl)ctlImaging).BindGrid(lstImaging);
            ((TransactionDtServiceViewCtl)ctlImaging).HideCheckBox();

            List<vPatientChargesDt8> lstMedicalDiagnostic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            ((TransactionDtServiceViewCtl)ctlMedicalDiagnostic).BindGrid(lstMedicalDiagnostic);
            ((TransactionDtServiceViewCtl)ctlMedicalDiagnostic).HideCheckBox();

            txtTotalPayer.Text = lst.Sum(p => p.PayerAmount).ToString("N2");
            txtTotalPatient.Text = lst.Sum(p => p.PatientAmount).ToString("N2");
            txtTotal.Text = lst.Sum(p => p.LineAmount).ToString("N2");
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            //int count = Convert.ToInt32(hdnOutstandingCount.Value);
            //if (count < 1)
            //{
            //    if (OnProcessRecord(ref errMessage))
            //    {
            //        result += "success";
            //    }
            //    else
            //        result += string.Format("fail|{0}", errMessage);
            //}
            //else
            //    result = "fail|Masih Ada Bill Yang Belum Lunas / Order Yang Belum Direalisasi. Tagihan Tidak Bisa Ditransfer";

            if (OnProcessRecord(ref errMessage))
                result += "success";
            else
                result += string.Format("fail|{0}", errMessage);
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnProcessRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            SettingParameterDao entitySettingParameterDao = new SettingParameterDao(ctx);
            ItemMasterDao entityItemMasterDao = new ItemMasterDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            ConsultVisitDao consVisitDao = new ConsultVisitDao(ctx);
            TestOrderHdDao testOrderHdDao = new TestOrderHdDao(ctx);
            PatientBillDao patientBillDao = new PatientBillDao(ctx);
            try
            {
                Registration entity = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                List<vConsultVisit> entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID IN ({0},{1})", entity.RegistrationID, hdnLinkedRegistrationID.Value));
                vConsultVisit entityVisitTo = entityVisit.Where(t => t.RegistrationID == entity.RegistrationID).FirstOrDefault();
                vConsultVisit entityVisitFrom = entityVisit.Where(t => t.RegistrationID == Convert.ToInt32(hdnLinkedRegistrationID.Value)).FirstOrDefault();
                vSettingParameterDt setPar = BusinessLayer.GetvSettingParameterDtList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.FN_TRANSFER_TAGIHAN_KELAS_TERTINGGI)).FirstOrDefault();
                bool flagHighestClass = setPar.ParameterValue == "1";
                //ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();

                string filterExpression = string.Format("VisitID IN (SELECT VisitID FROM vConsultVisit WHERE RegistrationID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}'))", hdnLinkedRegistrationID.Value, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.VOID);
                List<TestOrderHd> lstTestOrderHd = BusinessLayer.GetTestOrderHdList(filterExpression, ctx);
                foreach (TestOrderHd testOrderHd in lstTestOrderHd)
                {
                    testOrderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    testOrderHdDao.Update(testOrderHd);
                }

                filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}','{3}')", hdnLinkedRegistrationID.Value, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(filterExpression, ctx);
                foreach (PatientBill patientBill in lstPatientBill)
                {
                    patientBill.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                    patientBill.LastUpdatedDate = DateTime.Now;
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
                        patientChargesHdDao.Update(patientChargesHd);
                    }
                }

                Registration entityLinked = registrationDao.Get((int)entity.LinkedRegistrationID);
                string filter = string.Empty;

                #region Update Tarif UGD Ke Class Tertinggi
                if (flagHighestClass && (entityVisitTo.ClassPriority > entityVisitFrom.ClassPriority))
                {
                    //update Tarif UGD
                    filter = string.Format("TransactionID IN (SELECT TransactionID FROM PatientChargesHd WHERE VisitID = {0})", entityVisitFrom.VisitID);
                    List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(filter, ctx);
                    foreach (PatientChargesDt entPDt in lstPatientChargesDt)
                    {
                        string GCItemType = BusinessLayer.GetItemMaster(entPDt.ItemID).GCItemType;
                        entPDt.ChargeClassID = entityVisitTo.ChargeClassID;
                        if (GCItemType == Constant.ItemGroupMaster.SERVICE || GCItemType == Constant.ItemGroupMaster.RADIOLOGY || GCItemType == Constant.ItemGroupMaster.LABORATORY || GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC)
                        {
                            int testPartner = 0;
                            if (entPDt.BusinessPartnerID != null) testPartner = Convert.ToInt32(entPDt.BusinessPartnerID);
                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(entityVisitFrom.RegistrationID, entityVisitFrom.VisitID, entityVisitTo.ChargeClassID, entPDt.ItemID, 1, entityVisitFrom.VisitDate, null, testPartner);

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

                            decimal qty = entPDt.BaseQuantity;
                            decimal grossLineAmount = qty * price;
                            vItemService entityItemService = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", entPDt.ItemID)).FirstOrDefault();

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
                                    if (priceComp1 > 0 && entityItemService.IsAllowDiscountTariffComp1)
                                        totalDiscountAmount1 = discountAmountComp1;
                                    if (priceComp2 > 0 && entityItemService.IsAllowDiscountTariffComp2)
                                        totalDiscountAmount2 = discountAmountComp2;
                                    if (priceComp3 > 0 && entityItemService.IsAllowDiscountTariffComp3)
                                        totalDiscountAmount3 = discountAmountComp3;
                                }
                                else
                                {
                                    if (priceComp1 > 0 && entityItemService.IsAllowDiscountTariffComp1)
                                        totalDiscountAmount1 = discountAmount;
                                    if (priceComp2 > 0 && entityItemService.IsAllowDiscountTariffComp2)
                                        totalDiscountAmount2 = discountAmount;
                                    if (priceComp3 > 0 && entityItemService.IsAllowDiscountTariffComp3)
                                        totalDiscountAmount3 = discountAmount;
                                }
                            }

                            totalDiscountAmount = totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3;

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

                            entPDt.IsDiscount = totalDiscountAmount > 0;
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
                            vItemMaster entityItemMaster = BusinessLayer.GetvItemMasterList(string.Format("ItemID = {0}", entPDt.ItemID)).FirstOrDefault();
                            int typeItem = 2;
                            if (entityItemMaster.GCItemType == Constant.ItemGroupMaster.LOGISTIC)
                                typeItem = 3;
                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(entityVisitFrom.RegistrationID, entityVisitFrom.VisitID, entityVisitTo.ChargeClassID, entPDt.ItemID, typeItem, entityVisitFrom.VisitDate);

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

                            decimal total = grossLineAmount - entPDt.DiscountAmount;
                            decimal totalPayer = 0;
                            if (isCoverageInPercentage)
                                totalPayer = total * coverageAmount / 100;
                            else
                                totalPayer = coverageAmount * qty;
                            if (totalPayer > total)
                                totalPayer = total;
                            entPDt.PayerAmount = totalPayer;
                            entPDt.PatientAmount = total - totalPayer;
                            entPDt.LineAmount = total;
                            entPDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        }
                        patientChargesDtDao.Update(entPDt);
                    }
                }
                #endregion

                entityLinked.GCRegistrationStatus = Constant.VisitStatus.CLOSED;
                entityLinked.IsChargesTransfered = true;
                entityLinked.LastUpdatedBy = AppSession.UserLogin.UserID;
                registrationDao.Update(entityLinked);

                filter = string.Format("RegistrationID = {0}", entityLinked.RegistrationID);
                ConsultVisit entConsult = BusinessLayer.GetConsultVisitList(filter, ctx)[0];
                entConsult.GCVisitStatus = Constant.VisitStatus.CLOSED;
                entConsult.LastUpdatedBy = AppSession.UserLogin.UserID;
                consVisitDao.Update(entConsult);

                filter = string.Format("TransactionID IN (SELECT TransactionID FROM vPatientChargesHd WHERE RegistrationID = {0}) AND GCTransactionStatus NOT IN ('{1}', '{2}')", entity.LinkedRegistrationID, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                List<PatientChargesHd> lstPatientChargesHdFinal = BusinessLayer.GetPatientChargesHdList(filter, ctx);
                foreach (PatientChargesHd enPch in lstPatientChargesHdFinal)
                {
                    enPch.IsChargesTransfered = true;
                    enPch.LastUpdatedBy = AppSession.UserLogin.UserID;
                    patientChargesHdDao.Update(enPch);
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
    }
}