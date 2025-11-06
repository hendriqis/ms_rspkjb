using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class OutstandingTransactionOrderList : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST;
                case Constant.Facility.LABORATORY: return Constant.MenuCode.Laboratory.BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST; 
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST;
                default: return Constant.MenuCode.Inpatient.BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST;
            }
        }

        protected override void SetControlProperties()
        {
            string filterExpression1 = string.Format("RegistrationID = {0} AND GCTransactionStatus IN ('{1}')",
                                                  hdnRegistrationID.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);

            List<vPatientOrderAll> lstEntity1 = BusinessLayer.GetvPatientOrderAllList(filterExpression1);
            String filterExpression = string.Empty;
            if (lstEntity1.Count > 0)
            {
                string filterhsu = string.Empty;
                List<vPatientOrderAll> lsthsu = lstEntity1.GroupBy(p => p.HealthcareServiceUnitID).Select(x => x.FirstOrDefault()).ToList();
                if (lsthsu.Count > 0)
                {
                    foreach (vPatientOrderAll row in lsthsu)
                    {
                        filterhsu += string.Format("'{0}',", row.HealthcareServiceUnitID);

                    }
                    filterhsu = filterhsu.Remove(filterhsu.Length - 1);
                }
                filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID NOT IN ('{1}','{2}','{3}') AND HealthcareServiceUnitID  IN({4}) AND IsDeleted = 0 AND IsUsingRegistration = 1",
                     AppSession.UserLogin.HealthcareID, Constant.Facility.INPATIENT, Constant.Facility.MEDICAL_CHECKUP, Constant.Facility.OUTPATIENT, filterhsu);

            }
            else
            {
                filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID NOT IN ('{1}','{2}','{3}') AND HealthcareServiceUnitID != {4} AND IsDeleted = 0 AND IsUsingRegistration = 1",
                    AppSession.UserLogin.HealthcareID, Constant.Facility.INPATIENT, Constant.Facility.MEDICAL_CHECKUP, Constant.Facility.OUTPATIENT, AppSession.RegisteredPatient.HealthcareServiceUnitID);

            }

            List<vHealthcareServiceUnit> lstEntity = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            vHealthcareServiceUnit obj = new vHealthcareServiceUnit();
            obj.ServiceUnitName = "";
            obj.HealthcareServiceUnitID = 0;
            lstEntity.Add(obj);
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnitPerHealthcare, lstEntity.OrderBy(p => p.HealthcareServiceUnitID).ToList(), "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnitPerHealthcare.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnSelectedMember, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnSelectedMemberType, new ControlEntrySetting(false, false, false));
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            //vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnGCRegistrationStatus.Value = AppSession.RegisteredPatient.GCRegistrationStatus;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnFromHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
            hdnClassID.Value = AppSession.RegisteredPatient.ClassID.ToString();
            
            List<SettingParameterDt> lstsetpar = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID='{0}' AND ParameterCode IN ('{1}', '{2}','{3}')",
                                                                                       AppSession.UserLogin.HealthcareID, //0
                                                                                       Constant.SettingParameter.SA0218, //1
                                                                                       Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID,//2
                                                                                       Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI));
            hdnIsAutoPropose.Value = lstsetpar.Where(p => p.ParameterCode == Constant.SettingParameter.SA0218).FirstOrDefault().ParameterValue;
            hdnLaboratoryHSUID.Value = lstsetpar.Where(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
            hdnRadiologiHSUID.Value = lstsetpar.Where(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue; 


            BindGridView();
        }

        private void BindGridView()
        {
            //string filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus IN ('{1}')", hdnVisitID.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            string filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus IN ('{1}')",
                                              hdnRegistrationID.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);

            if (cboServiceUnitPerHealthcare.Value != null && cboServiceUnitPerHealthcare.Value.ToString() != "")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = '{0}'", cboServiceUnitPerHealthcare.Value);
            }
            else
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = ''");
            }

            filterExpression += " ORDER BY OrderDate, OrderTime, OrderID";
            List<vPatientOrderAll> lstEntity = BusinessLayer.GetvPatientOrderAllList(filterExpression);

            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;

            String[] paramID = hdnSelectedMember.Value.Substring(1).Split(',');
            String[] paramType = hdnSelectedMemberType.Value.Substring(1).Split(',');
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            TestOrderHdDao entityTestOrderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityTestOrderDtDao = new TestOrderDtDao(ctx);
            try
            {
                if (type == "Process")
                {
                    #region PROCESS
                    for (int i = 0; i < paramID.Length; i++)
                    {
                        List<vPatientOrderAll> lstEntity = BusinessLayer.GetvPatientOrderAllList(String.Format("OrderID IN ({0}) AND OrderType = '{1}'", paramID[i], paramType[i]));

                        List<TestOrderHd> lstTestOrder = new List<TestOrderHd>();

                        #region List
                        foreach (vPatientOrderAll entity in lstEntity)
                        {
                            if (entity.OrderType == "TO")
                            {
                                TestOrderHd entityTo = new TestOrderHd();
                                entityTo = BusinessLayer.GetTestOrderHdList(String.Format("TestOrderID = {0} AND GCTransactionStatus = '{1}'",
                                                    entity.OrderID, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();

                                if (entityTo != null)
                                {
                                    lstTestOrder.Add(entityTo);
                                }
                            }
                        }

                        #endregion

                        #region Test Order
                        if (lstTestOrder.Count > 0)
                        {
                            foreach (TestOrderHd entity in lstTestOrder)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                                {
                                    List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();

                                    List<TestOrderDt> lstDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted=0", entity.TestOrderID), ctx);
                                    foreach (TestOrderDt entityDt in lstDt)
                                    {
                                        if (entityDt.IsCITO == false && entityDt.IsDeleted == false && entityDt.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                        {
                                            entityDt.GCTestOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            entityTestOrderDtDao.Update(entityDt);
                                        }

                                        PatientChargesDt patientChargesDt = new PatientChargesDt();
                                        patientChargesDt.ItemID = entityDt.ItemID;
                                        patientChargesDt.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                                        patientChargesDt.ChargeClassID = Convert.ToInt32(hdnClassID.Value);
                                        patientChargesDt.ItemPackageID = entityDt.ItemPackageID;
                                        patientChargesDt.ReferenceDtID = entityDt.ID;

                                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), entity.VisitID, patientChargesDt.ChargeClassID, entityDt.ItemID, 1, DateTime.Now, ctx);

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

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        vItemService entityItemMaster = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0} AND IsDeleted=0", entityDt.ItemID), ctx).FirstOrDefault();
                                        patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;

                                        patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, entity.VisitID, entity.HealthcareServiceUnitID, DateTime.Now, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT), ctx).FirstOrDefault().RevenueSharingID;
                                        if (patientChargesDt.RevenueSharingID == 0)
                                            patientChargesDt.RevenueSharingID = null;
                                        patientChargesDt.IsVariable = false;
                                        patientChargesDt.IsUnbilledItem = false;

                                        patientChargesDt.IsCITO = false;
                                        patientChargesDt.CITOAmount = 0;
                                        patientChargesDt.BaseCITOAmount = entityItemMaster.CITOAmount;
                                        patientChargesDt.IsCITOInPercentage = entityItemMaster.IsCITOInPercentage;

                                        decimal grossLineAmount = entityDt.ItemQty * price;

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

                                        totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (entityDt.ItemQty);

                                        if (totalDiscountAmount > grossLineAmount)
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

                                        patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = entityDt.ItemQty;
                                        patientChargesDt.PatientAmount = total - totalPayer;
                                        patientChargesDt.PayerAmount = totalPayer;
                                        patientChargesDt.LineAmount = total;
                                        patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                                        patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        lstPatientChargesDt.Add(patientChargesDt);
                                        //ct++;
                                    }
                                    if (result)
                                    {
                                        #region Patient Charges
                                        PatientChargesHd patientChargesHd = null;
                                        if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0" || patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value)).GCTransactionStatus != Constant.TransactionStatus.OPEN)
                                        {
                                            patientChargesHd = new PatientChargesHd();
                                            patientChargesHd.VisitID = entity.VisitID;
                                            patientChargesHd.LinkedChargesID = null;
                                            patientChargesHd.TestOrderID = Convert.ToInt32(entity.TestOrderID);
                                            patientChargesHd.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;

                                            if (cboServiceUnitPerHealthcare.Value.ToString() == hdnLaboratoryHSUID.Value)
                                            {
                                                patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                                            }
                                            else if (cboServiceUnitPerHealthcare.Value.ToString() == hdnRadiologiHSUID.Value)
                                            {
                                                patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                                            }
                                            else
                                            {
                                                patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                                            }

                                            //if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                            //    patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                                            //else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                            //    patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                                            //else
                                            //    patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                                            
                                            patientChargesHd.TransactionDate = DateTime.Now;
                                            patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                                            patientChargesHd.PatientBillingID = null;
                                            patientChargesHd.ReferenceNo = "";
                                            if (hdnIsAutoPropose.Value == "1")
                                            {
                                                patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                            }
                                            else
                                            {
                                                patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                            }
                                            patientChargesHd.GCVoidReason = null;
                                            patientChargesHd.TotalPatientAmount = 0;
                                            patientChargesHd.TotalPayerAmount = 0;
                                            patientChargesHd.TotalAmount = 0;
                                            patientChargesHd.Remarks = entity.Remarks;
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
                                            if (hdnIsAutoPropose.Value == "1")
                                            {
                                                if ((patientChargesDt.LocationID != null && patientChargesDt.LocationID != 0) && !patientChargesDt.IsApproved)
                                                {
                                                    patientChargesDt.IsApproved = true;
                                                }
                                                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                            }
                                            patientChargesDtDao.Insert(patientChargesDt);
                                        }
                                        #endregion
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        entity.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityTestOrderHdDao.Update(entity);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    ctx.CommitTransaction();

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
            return result;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}