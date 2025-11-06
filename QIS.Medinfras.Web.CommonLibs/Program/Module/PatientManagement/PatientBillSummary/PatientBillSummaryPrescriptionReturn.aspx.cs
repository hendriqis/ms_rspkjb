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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryPrescriptionReturn : BasePageTrx
    {
        protected bool IsShowSwitchIcon = false;

        public override string OnGetMenuCode()
        {
            switch (hdnVisitDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_PRESCRIPTION_RETURN;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_PRESCRIPTION_RETURN;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_PRESCRIPTION_RETURN;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_PRESCRIPTION_RETURN;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_PRESCRIPTION_RETURN;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_PRESCRIPTION_RETURN;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_PRESCRIPTION_RETURN;
                default:
                    return Constant.MenuCode.Outpatient.BILL_SUMMARY_PRESCRIPTION_RETURN;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnIsAdminCanCancelAllTransaction.Value = AppSession.IsAdminCanCancelAllTransaction ? "1" : "0";

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
            hdnPrescriptionFeeAmount.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT).ParameterValue;
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();

            IsShowSwitchIcon = entity.GCCustomerType != Constant.CustomerType.PERSONAL;

            if (entity.DischargeDate != null && entity.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
            {
                hdnIsDischarges.Value = "1";
            }
            else
            {
                hdnIsDischarges.Value = "0";
            }

            hdnChargeClassID.Value = entity.ChargeClassID.ToString();
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
            hdnGCCustomerType.Value = entity.GCCustomerType;
            hdnDefaultParamedicID.Value = entity.ParamedicID.ToString();
            hdnDefaultParamedicCode.Value = entity.ParamedicCode;
            hdnDefaultParamedicName.Value = entity.ParamedicName;
            hdnVisitDepartmentID.Value = entity.DepartmentID;
            hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
            List<SettingParameter> lstSettingParameterDiagnostic = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID));
            hdnImagingServiceUnitID.Value = lstSettingParameterDiagnostic.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameterDiagnostic.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnClassID.Value = entity.ClassID.ToString();
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            IsLoadFirstRecord = (OnGetRowCount() > 0);
            Helper.SetControlEntrySetting(txtPatientAmount, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(txtPayerAmount, new ControlEntrySetting(true, true, true), "mpTrx");
            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            BindGridView();
        }

        protected override void SetControlProperties()
        {
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<ClassCare>(cboChargeClass, lstClassCare, "ClassName", "ClassID");
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PRESCRIPTION_RETURN_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboReturnType, lstStandardCode, "StandardCodeName", "StandardCodeID");
            StandardCode scReturnType = lstStandardCode.FirstOrDefault(p => p.IsDefault);
            if (scReturnType == null)
                scReturnType = lstStandardCode.FirstOrDefault();
            cboReturnType.Value = scReturnType.StandardCodeID;
            hdnDefaultReturnType.Value = scReturnType.StandardCodeID;
            List<vHealthcareServiceUnit> lstDispensaryServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0 AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.PHARMACY));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboDispensaryServiceUnitID, lstDispensaryServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnGCTransactionStatus, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(hdnPrescriptionReturnOrderID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(chkIsCorrectionTransaction, new ControlEntrySetting(true, false, false));
            chkIsCorrectionTransaction.Checked = false;
            SetControlEntrySetting(txtTransactionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPrescriptionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPrescriptionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboDispensaryServiceUnitID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, true, hdnDefaultParamedicID.Value));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, hdnDefaultParamedicCode.Value));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false, hdnDefaultParamedicName.Value));
            SetControlEntrySetting(cboReturnType, new ControlEntrySetting(true, false, true, hdnDefaultReturnType.Value));
        }

        #region Load Entity
        public override void OnAddRecord()
        {
            //hdnTransactionStatus.Value = "";
            hdnIsEditable.Value = "1";
            BindGridView();

            divCreatedBy.InnerHtml = string.Empty;
            divCreatedDate.InnerHtml = string.Empty;
            divLastUpdatedBy.InnerHtml = string.Empty;
            divLastUpdatedDate.InnerHtml = string.Empty;
            divProposedBy.InnerHtml = string.Empty;
            divProposedDate.InnerHtml = string.Empty;
            trProposedBy.Style.Add("display", "none");
            trProposedDate.Style.Add("display", "none");
            divVoidBy.InnerHtml = string.Empty;
            divVoidDate.InnerHtml = string.Empty;
            divVoidReason.InnerHtml = string.Empty;
            trVoidBy.Style.Add("display", "none");
            trVoidDate.Style.Add("display", "none");
            trVoidReason.Style.Add("display", "none");
        }

        public String GetFilterExpression()
        {
            //String TransactionStatus = String.Format("'{0}'", Constant.TransactionStatus.CLOSED);
            String filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            filterExpression += string.Format(" AND DepartmentID = '{0}' AND PrescriptionReturnOrderID IS NOT NULL", Constant.Facility.PHARMACY);
            return filterExpression;
        }

        public String GetTransactionCode()
        {
            return Constant.TransactionCode.PH_CHARGES;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHd(filterExpression, PageIndex, " TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvPatientChargesHdRowIndex(filterExpression, keyValue, "TransactionID DESC");
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHd(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected bool IsEditable = true;
        private void EntityToControl(vPatientChargesHd entityCharges, ref bool isShowWatermark, ref string watermarkText)
        {
            IsShowSwitchIcon = hdnGCCustomerType.Value != Constant.CustomerType.PERSONAL;

            vPrescriptionReturnOrderHd entity = BusinessLayer.GetvPrescriptionReturnOrderHdList(string.Format("PrescriptionReturnOrderID = {0}", entityCharges.PrescriptionReturnOrderID)).FirstOrDefault();

            if (AppSession.IsAdminCanCancelAllTransaction)
            {
                if (entityCharges.GCTransactionStatus != Constant.TransactionStatus.OPEN && entityCharges.GCTransactionStatus != Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    isShowWatermark = true;
                    watermarkText = entityCharges.TransactionStatusWatermark;
                    IsEditable = false;
                    hdnIsEditable.Value = "0";
                }
                else
                {
                    isShowWatermark = false;
                    IsEditable = true;
                    hdnIsEditable.Value = "1";
                }

                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    IsAllowProposed = false;
                }
                else if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    IsAllowProposed = true;
                }
                else
                {
                    IsAllowProposed = false;
                }
            }
            else
            {
                if (entityCharges.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                {
                    isShowWatermark = true;
                    watermarkText = entityCharges.TransactionStatusWatermark;
                    IsEditable = false;
                    hdnIsEditable.Value = "0";
                    IsAllowProposed = false;
                }
                else
                {
                    isShowWatermark = false;
                    IsEditable = true;
                    hdnIsEditable.Value = "1";
                    IsAllowProposed = true;
                }
            }

            hdnGCTransactionStatus.Value = entityCharges.GCTransactionStatus;
            hdnPrescriptionReturnOrderID.Value = entity.PrescriptionReturnOrderID.ToString();
            hdnTransactionID.Value = entityCharges.TransactionID.ToString();
            txtTransactionNo.Text = entityCharges.TransactionNo;
            hdnLocationID.Value = entity.LocationID.ToString();
            txtPrescriptionDate.Text = entityCharges.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entityCharges.TransactionTime;
            cboDispensaryServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            cboReturnType.Value = entity.GCPrescriptionReturnType;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;

            BindGridView();

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdateByName;
            if (entity.LastUpdatedDate != null && entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
            {
                divProposedBy.InnerHtml = entity.ProposedByName;
                if (entity.ProposedDate != null && entity.ProposedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    divProposedDate.InnerHtml = entity.ProposedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                }

                trProposedBy.Style.Remove("display");
                trProposedDate.Style.Remove("display");
            }
            else
            {
                trProposedBy.Style.Add("display", "none");
                trProposedDate.Style.Add("display", "none");
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.VOID)
            {
                divVoidBy.InnerHtml = entity.VoidByName;
                if (entity.VoidDate != null && entity.VoidDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    divVoidDate.InnerHtml = entity.VoidDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                }

                string voidReason = "";

                if (entity.GCVoidReason == Constant.DeleteReason.OTHER)
                {
                    voidReason = entity.VoidReasonWatermark + " ( " + entity.VoidReason + " )";
                }
                else
                {
                    voidReason = entity.VoidReasonWatermark;
                }

                trVoidBy.Style.Remove("display");
                trVoidDate.Style.Remove("display");
                divVoidReason.InnerHtml = voidReason;
                trVoidReason.Style.Remove("display");
            }
            else
            {
                trVoidBy.Style.Add("display", "none");
                trVoidDate.Style.Add("display", "none");
                trVoidReason.Style.Add("display", "none");
            }

        }

        //public List<vPrescriptionOrderDt> tempLst;

        public List<PatientChargesDt> lstChargesDt;

        private void BindGridView()
        {
            string filterExpression = "1 = 0";

            if (hdnPrescriptionReturnOrderID.Value != "" && hdnPrescriptionReturnOrderID.Value != "0")
            {
                if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                {
                    filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
                }
                else
                {
                    filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
                }

                filterExpression += string.Format(" AND LocationID = {0} AND IsDeleted = 0", hdnLocationID.Value);

                //filterExpression = string.Format("{0} AND UsedQuantity > 0 AND (PrescriptionOrderDetailID IN (SELECT PrescriptionOrderDetailID FROM vPrescriptionOrderDt WHERE {0} AND IsCompound = 0) OR PrescriptionReturnOrderDtID IS NOT NULL) ORDER BY ItemName1 ", filterExpression);
                filterExpression = string.Format("{0} AND UsedQuantity > 0 AND PrescriptionReturnOrderDtID IS NOT NULL ORDER BY ItemName1 ", filterExpression);

                //List<vPatientChargesDt2> lstPrescriptionOrderDt = BusinessLayer.GetvPatientChargesDt2List(filterExpression);
                //tempLst = (from bs in lstPrescriptionOrderDt
                //           group bs by bs.ItemID into g
                //           select new vPrescriptionOrderDt
                //           {
                //               ItemID = g.Key,
                //               ItemName1 = g.First().ItemName1,
                //               GCItemUnit = g.First().GCBaseUnit,
                //               ItemUnit = g.First().ItemUnit,
                //               UsedQuantity = g.Sum(x => x.UsedQuantity),
                //               PatientAmount = g.Sum(x => x.PatientAmount),
                //               PayerAmount = g.Sum(x => x.PayerAmount)
                //           }).ToList();

                filterExpression = string.Format("PrescriptionReturnOrderID = {0} AND ID IS NOT NULL AND IsDeleted = 0 ORDER BY PrescriptionReturnOrderID DESC", hdnPrescriptionReturnOrderID.Value);


                string filterChargesDt = string.Format("ID IN (SELECT PatientChargesDtID FROM PrescriptionReturnOrderDt WHERE PrescriptionReturnOrderID = {0})", hdnPrescriptionReturnOrderID.Value);
                lstChargesDt = BusinessLayer.GetPatientChargesDtList(filterChargesDt);
            }
            hdnIsEditable.Value = IsEditable ? "1" : "0";

            List<vPrescriptionReturnOrderDt> lstEntity = BusinessLayer.GetvPrescriptionReturnOrderDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            decimal totalPatientAmount = lstEntity.Sum(p => p.PatientAmount);
            decimal totalPayerAmount = lstEntity.Sum(p => p.PayerAmount);
            decimal totalLineAmount = lstEntity.Sum(p => p.LineAmount);
            if (lstEntity.Count > 0)
            {
                ((HtmlTableCell)lvwView.FindControl("tdTotalPayer")).InnerHtml = totalPayerAmount.ToString("N");
                ((HtmlTableCell)lvwView.FindControl("tdTotalPatient")).InnerHtml = totalPatientAmount.ToString("N");
                ((HtmlTableCell)lvwView.FindControl("tdTotal")).InnerHtml = totalLineAmount.ToString("N");
            }
        }

        public void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPrescriptionReturnOrderDt entity = (vPrescriptionReturnOrderDt)e.Item.DataItem;
                //Decimal test = tempLst.FirstOrDefault(x => x.ItemID == entity.ItemID).PayerAmount;
                ((HtmlTableCell)e.Item.FindControl("tdPayerAmount")).InnerHtml = lstChargesDt.FirstOrDefault(x => x.ItemID == entity.ItemID).PayerAmount.ToString("N");
                ((HtmlTableCell)e.Item.FindControl("tdPatientAmount")).InnerHtml = lstChargesDt.FirstOrDefault(x => x.ItemID == entity.ItemID).PatientAmount.ToString("N");
            }
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
        #endregion

        #region Save Entity
        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                PrescriptionReturnOrderHd entity = BusinessLayer.GetPrescriptionReturnOrderHd(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value));
                if (AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        entity.GCPrescriptionReturnType = cboReturnType.Value.ToString();
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePrescriptionReturnOrderHd(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = "Order Retur " + entity.PrescriptionReturnOrderNo + " Sudah Diproses. Tidak Bisa Diubah";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
                }
                else
                {
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCPrescriptionReturnType = cboReturnType.Value.ToString();
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePrescriptionReturnOrderHd(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = "Order Retur " + entity.PrescriptionReturnOrderNo + " Sudah Diproses. Tidak Bisa Diubah";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private string ValidateTransaction(int PrescriptionReturnOrderID)
        {
            string result;
            string lstSelectedID = "";
            StringBuilder errMessage = new StringBuilder();
            List<PatientChargesDt> lstEntityDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsApproved = 0 AND IsDeleted = 0", hdnTransactionID.Value));
            if (lstEntityDt.Count > 0)
            {
                foreach (PatientChargesDt itm in lstEntityDt)
                    lstSelectedID += "," + itm.ItemID;
            }

            string filterExpression = string.Format("PrescriptionReturnOrderID = '{0}' AND GCPrescriptionReturnOrderStatus = '{1}' AND IsDeleted = 0",
                                            PrescriptionReturnOrderID.ToString(), Constant.TestOrderStatus.RECEIVED);
            List<PrescriptionReturnOrderDt> lstOpenOrder = BusinessLayer.GetPrescriptionReturnOrderDtList(filterExpression);
            if (lstOpenOrder.Count > 0)
            {
                errMessage.AppendLine("Masih ada item order yang belum diproses.");
            }

            if (!string.IsNullOrEmpty(errMessage.ToString()))
                result = string.Format("{0}|{1}", "0", errMessage.ToString());
            else
                result = string.Format("{0}|{1}", "1", "success");

            return result;
        }

        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PrescriptionReturnOrderHdDao entityOrderHdDao = new PrescriptionReturnOrderHdDao(ctx);
            PrescriptionReturnOrderDtDao entityOrderDtDao = new PrescriptionReturnOrderDtDao(ctx);
            ChargesStatusLogDao statusLogDao = new ChargesStatusLogDao(ctx);

            try
            {
                ChargesStatusLog statusLog = new ChargesStatusLog();

                string statusOld = "", statusNew = "";

                PatientChargesHd entity = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                statusOld = entity.GCTransactionStatus;
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string validationResult = ValidateTransaction(Convert.ToInt32(entity.PrescriptionReturnOrderID));
                    string[] resultInfo = validationResult.Split('|');

                    if (resultInfo[0] == "0")
                    {
                        errMessage = resultInfo[1];
                        return false;
                    }

                    PrescriptionReturnOrderHd orderHd = entityOrderHdDao.Get(Convert.ToInt32(entity.PrescriptionReturnOrderID));
                    if (orderHd != null)
                    {
                        orderHd.GCOrderStatus = Constant.TestOrderStatus.COMPLETED;
                        entityOrderHdDao.Update(orderHd);
                    }

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<PatientChargesDt> lstEntityDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsApproved = 0 AND IsDeleted = 0", hdnTransactionID.Value), ctx);
                    foreach (PatientChargesDt entityDt in lstEntityDt)
                    {
                        ItemMaster entityItemMaster = itemDao.Get(Convert.ToInt32(entityDt.ItemID));
                        PrescriptionReturnOrderDt orderDt = entityOrderDtDao.Get((int)entityDt.PrescriptionReturnOrderDtID);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        int oRegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                        int oLocationID = Convert.ToInt32(entityDt.LocationID);
                        int oItemID = entityDt.ItemID;
                        int oPatientChargesDtID = Convert.ToInt32(orderDt.PatientChargesDtID);
                        List<GetPrescriptionReturnOrderRemainingQtyPerItem> lstRecommend = BusinessLayer.GetPrescriptionReturnOrderRemainingQtyPerItemList(oRegistrationID, oLocationID, oItemID, oPatientChargesDtID, ctx);
                        decimal qtyRecommend = lstRecommend.Sum(a => a.RemainingChargesQty);
                        if ((entityDt.ChargedQuantity * -1) <= qtyRecommend)
                        {
                            if (orderDt != null)
                            {
                                if (!orderDt.IsDeleted && orderDt.GCPrescriptionReturnOrderStatus == Constant.TestOrderStatus.IN_PROGRESS)
                                {
                                    orderDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.COMPLETED;
                                    orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityOrderDtDao.Update(orderDt);
                                }
                            }

                            entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entityDt.IsApproved = true;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityDtDao.Update(entityDt);
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Transaksi item {0} [{1}] tidak dapat dibuatkan retur sejumlah {2} karena sudah melebihi jumlah rekomendasi retur sejumlah {3}.",
                                                            entityItemMaster.ItemName1, entityItemMaster.ItemCode, (entityDt.ChargedQuantity * -1).ToString(Constant.FormatString.NUMERIC_2), qtyRecommend.ToString(Constant.FormatString.NUMERIC_2));

                            break;
                        }
                    }

                    if (result)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entity.ProposedBy = AppSession.UserLogin.UserID;
                        entity.ProposedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHdDao.Update(entity);

                        statusNew = entity.GCTransactionStatus;

                        statusLog.VisitID = entity.VisitID;
                        statusLog.TransactionID = entity.TransactionID;
                        statusLog.GCTransactionStatusOLD = statusOld;
                        statusLog.GCTransactionStatusNEW = statusNew;
                        statusLog.LogDate = DateTime.Now;
                        statusLog.UserID = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        statusLogDao.Insert(statusLog);

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
                else
                {
                    result = false;
                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat di-proposed lagi.", entity.TransactionNo);
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
        #endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];
            bool result = true;

            if (param[0] == "void")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionReturnOrderHdDao presreturnHdDao = new PrescriptionReturnOrderHdDao(ctx);
                PrescriptionReturnOrderDtDao presreturnDtDao = new PrescriptionReturnOrderDtDao(ctx);
                PatientChargesHdDao chargesDao = new PatientChargesHdDao(ctx);
                PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
                ChargesStatusLogDao statusLogDao = new ChargesStatusLogDao(ctx);
                try
                {
                    ChargesStatusLog statusLog = new ChargesStatusLog();

                    string statusOld = "", statusNew = "";

                    bool isAllowSaveDt = false;
                    Int32 TransactionID = Convert.ToInt32(hdnTransactionID.Value);
                    PatientChargesHd entity = chargesDao.Get(TransactionID);
                    statusOld = entity.GCTransactionStatus;
                    if (AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            result = false;
                            errMessage = "Transaksi " + entity.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            result = false;
                            errMessage = "Transaksi " + entity.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }

                    if (isAllowSaveDt)
                    {
                        PrescriptionReturnOrderHd orderHd = presreturnHdDao.Get(Convert.ToInt32(entity.PrescriptionReturnOrderID));
                        List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value), ctx);
                        foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                        {
                            patientChargesDt.IsApproved = false;
                            patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            chargesDtDao.Update(patientChargesDt);

                            PrescriptionReturnOrderDt orderDt = presreturnDtDao.Get((int)patientChargesDt.PrescriptionReturnOrderDtID);
                            if (orderDt != null)
                            {
                                if (!orderDt.IsDeleted)
                                {
                                    if (!orderHd.IsCreatedBySystem)
                                    {
                                        if (hdnIsDischarges.Value == "1")
                                        {
                                            if (AppSession.IsAutoVoidPrescriptionOrderCreatedBySystem == "1")
                                            {
                                                orderDt.IsDeleted = true;
                                                orderDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                orderDt.GCDeleteReason = Constant.DeleteReason.OTHER;
                                                orderDt.DeleteReason = "Linked transaction was deleted";
                                            }
                                            else
                                            {
                                                orderDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                            }
                                        }
                                        else
                                        {
                                            orderDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                        }
                                    }
                                    else
                                    {
                                        orderDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                        orderDt.DeleteReason = Constant.DeleteReason.OTHER;
                                        orderDt.DeleteReason = "Linked transaction was deleted";
                                    }
                                    orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    presreturnDtDao.Update(orderDt);
                                }
                            }
                        }

                        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entity.GCVoidReason = gcDeleteReason;
                        if (gcDeleteReason == Constant.DeleteReason.OTHER)
                        {
                            entity.VoidReason = reason;
                        }
                        entity.VoidBy = AppSession.UserLogin.UserID;
                        entity.VoidDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        chargesDao.Update(entity);

                        statusNew = entity.GCTransactionStatus;

                        statusLog.VisitID = entity.VisitID;
                        statusLog.TransactionID = entity.TransactionID;
                        statusLog.GCTransactionStatusOLD = statusOld;
                        statusLog.GCTransactionStatusNEW = statusNew;
                        statusLog.LogDate = DateTime.Now;
                        statusLog.UserID = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        statusLogDao.Insert(statusLog);

                        if (orderHd != null)
                        {
                            if (!orderHd.IsCreatedBySystem)
                            {
                                if (hdnIsDischarges.Value == "1")
                                {
                                    if (AppSession.IsAutoVoidPrescriptionOrderCreatedBySystem == "1")
                                    {
                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                        orderHd.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                        orderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                        orderHd.VoidReason = "Linked transaction was deleted";
                                    }
                                    else
                                    {
                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                        orderHd.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                        orderHd.ProposedBy = AppSession.UserLogin.UserID;
                                        orderHd.ProposedDate = DateTime.Now;
                                    }
                                }
                                else
                                {
                                    orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    orderHd.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                }
                                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                presreturnHdDao.Update(orderHd);
                            }
                            else
                            {
                                orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                orderHd.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                orderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                orderHd.VoidReason = "Linked transaction was deleted";
                                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                presreturnHdDao.Update(orderHd);
                            }
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
            }
            return result;
        }

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int TransactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (OnSaveEditRecordEntityDt(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "switch")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnSwitchDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteEntityDt(ref errMessage, param[1]))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = TransactionID.ToString();
        }

        private bool OnSwitchDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                PatientChargesHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                if (AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted)
                        {
                            decimal temp = entity.PayerAmount;
                            entity.PayerAmount = entity.PatientAmount;
                            entity.PatientAmount = temp;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi " + entityHd.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        result = false;
                    }
                }
                else
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted)
                        {
                            decimal temp = entity.PayerAmount;
                            entity.PayerAmount = entity.PatientAmount;
                            entity.PatientAmount = temp;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi " + entityHd.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        result = false;
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                PatientChargesHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                if (AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDtList(String.Format("TransactionID = {0} AND ItemID = {1} AND IsDeleted = 0", hdnTransactionID.Value, hdnItemID.Value), ctx).FirstOrDefault();
                        if (!entityChargesDt.IsDeleted)
                        {
                            entityChargesDt.DiscountAmount = Convert.ToDecimal(Request.Form[txtDiscountAmount.UniqueID]);
                            entityChargesDt.DiscountComp1 = entityChargesDt.DiscountAmount / entityChargesDt.ChargedQuantity * -1;
                            entityChargesDt.PatientAmount = Convert.ToDecimal(txtPatientAmount.Text);
                            entityChargesDt.PayerAmount = Convert.ToDecimal(txtPayerAmount.Text);
                            entityChargesDt.LineAmount = Convert.ToDecimal(Request.Form[txtLineAmount.UniqueID]);
                            entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityChargesDt);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi " + entityHd.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        result = false;
                    }
                }
                else
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDtList(String.Format("TransactionID = {0} AND ItemID = {1} AND IsDeleted = 0", hdnTransactionID.Value, hdnItemID.Value), ctx).FirstOrDefault();
                        if (!entityChargesDt.IsDeleted)
                        {
                            entityChargesDt.DiscountAmount = Convert.ToDecimal(Request.Form[txtDiscountAmount.UniqueID]);
                            entityChargesDt.DiscountComp1 = entityChargesDt.DiscountAmount / entityChargesDt.ChargedQuantity * -1;
                            entityChargesDt.PatientAmount = Convert.ToDecimal(txtPatientAmount.Text);
                            entityChargesDt.PayerAmount = Convert.ToDecimal(txtPayerAmount.Text);
                            entityChargesDt.LineAmount = Convert.ToDecimal(Request.Form[txtLineAmount.UniqueID]);
                            entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityChargesDt);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi " + entityHd.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        result = false;
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

        private bool OnDeleteEntityDt(ref string errMessage, string param)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionReturnOrderDtDao entityDtDao = new PrescriptionReturnOrderDtDao(ctx);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            string[] paramDelete = param.Split(';');
            int ID = Convert.ToInt32(paramDelete[0]);
            string gcDeleteReason = paramDelete[1];
            string reason = paramDelete[2];
            try
            {
                PatientChargesHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                if (AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        PatientChargesDt entityChargesDt = entityChargesDtDao.Get(ID);
                        entityChargesDt.GCDeleteReason = gcDeleteReason;
                        entityChargesDt.DeleteReason = reason;
                        entityChargesDt.DeleteDate = DateTime.Now;
                        entityChargesDt.DeleteBy = AppSession.UserLogin.UserID;
                        entityChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                        entityChargesDt.IsApproved = false;
                        entityChargesDt.IsDeleted = true;
                        entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityChargesDtDao.Update(entityChargesDt);

                        PrescriptionReturnOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(entityChargesDt.PrescriptionReturnOrderDtID));
                        entityDt.IsDeleted = true;
                        entityDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.CANCELLED;
                        entityDt.DeleteReason = Constant.DeleteReason.OTHER;
                        entityDt.DeleteReason = "Linked transaction was deleted";
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }
                    else
                    {
                        errMessage = "Transaksi " + entityHd.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        result = false;
                    }
                }
                else
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entityChargesDt = entityChargesDtDao.Get(ID);
                        entityChargesDt.GCDeleteReason = gcDeleteReason;
                        entityChargesDt.DeleteReason = reason;
                        entityChargesDt.DeleteDate = DateTime.Now;
                        entityChargesDt.DeleteBy = AppSession.UserLogin.UserID;
                        entityChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                        entityChargesDt.IsApproved = false;
                        entityChargesDt.IsDeleted = true;
                        entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityChargesDtDao.Update(entityChargesDt);

                        PrescriptionReturnOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(entityChargesDt.PrescriptionReturnOrderDtID));
                        entityDt.IsDeleted = true;
                        entityDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.CANCELLED;
                        entityDt.DeleteReason = Constant.DeleteReason.OTHER;
                        entityDt.DeleteReason = "Linked transaction was deleted";
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }
                    else
                    {
                        errMessage = "Transaksi " + entityHd.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        result = false;
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
        #endregion

    }
}