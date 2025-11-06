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
using QIS.Medinfras.Web.CommonLibs.Program;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class LaboratoryOrderDetail : BasePageTrxPatientManagement
    {
        protected int testOrderOpenCount = 0;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.WORK_LIST;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED);
        }

        public override string GetGCCustomerType()
        {
            return hdnGCCustomerType.Value;
        }

        public override string GetGCTransactionStatus()
        {
            return hdnGCTransactionStatus.Value;
        }

        public override string GetRemarksHd()
        {
            return "";
        }

        public override string GetReferenceNoHd()
        {
            return "";
        }

        public override bool GetIsCorrectionTransactionHd()
        {
            return false;
        }

        public override bool GetIsAIOTransactionHd()
        {
            if (hdnIsCheckedAIOTransaction.Value == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetClassID()
        {
            return Convert.ToInt32(hdnClassID.Value);
        }
        public override int GetRegistrationPhysicianID()
        {
            return Convert.ToInt32(hdnRegistrationPhysicianID.Value);
        }
        public override int GetLocationID()
        {
            return Convert.ToInt32(hdnLocationID.Value);
        }
        public override int GetLogisticLocationID()
        {
            return Convert.ToInt32(hdnLogisticLocationID.Value);
        }
        public override int GetHealthcareServiceUnitID()
        {
            return Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
        }
        public override string GetTransactionHdID()
        {
            return hdnTransactionHdID.Value;
        }
        public override void SetTransactionHdID(string value)
        {
            hdnTransactionHdID.Value = value;
        }
        public override string GetDepartmentID()
        {
            return hdnDepartmentID.Value;
        }
        public override string GetTransactionDate()
        {
            return Request.Form[txtTransactionDateLab.UniqueID];
        }
        public override string GetTransactionTime()
        {
            return Request.Form[txtTransactionTime.UniqueID];
        }
        public override string GetTransactionDate2()
        {
            return txtTransactionDateLab.Text;
        }
        public override string GetTransactionTime2()
        {
            return txtTransactionTime.Text;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param[0] == "to")
                {
                    hdnVisitID.Value = param[1];
                    hdnTestOrderID.Value = param[2];
                    hdnHealthcareServiceUnitID.Value = param[3];
                    //testOrderOpenCount = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND GCTestOrderStatus = '{1}'", param[2], Constant.TestOrderStatus.OPEN)).Count;
                    //hdnHealthcareServiceUnitID.Value = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
                }
                else
                {
                    hdnVisitID.Value = param[0];
                    hdnHealthcareServiceUnitID.Value = param[2];
                    hdnTestOrderID.Value = "0";
                    btnClinicTransactionTestOrder.Style.Add("display", "none");
                }
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();

                hdnDepartmentFromID.Value = entity.DepartmentID;

                List<vPatientVisitNote> lstEntity = BusinessLayer.GetvPatientVisitNoteList(string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value));
                if (lstEntity.Count == 0)
                {
                    divVisitNote.Attributes.Add("style", "display:none");
                }

                ctlPatientBanner.InitializePatientBanner(entity);
                hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnRegistrationPhysicianID.Value = hdnPhysicianID.Value = entity.ParamedicID.ToString();
                hdnPhysicianCode.Value = entity.ParamedicCode;
                hdnPhysicianName.Value = entity.ParamedicName;

                hdnReferrerParamedicID.Value = entity.ReferrerParamedicID != null ? entity.ReferrerParamedicID.ToString() : "";

                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value)).FirstOrDefault();
                hdnDepartmentID.Value = hsu.DepartmentID;
                hdnLocationID.Value = hsu.LocationID.ToString();
                hdnLogisticLocationID.Value = hsu.LogisticLocationID.ToString();
                //hdnLocationID.Value = entity.LocationID.ToString();
                //hdnLogisticLocationID.Value = entity.LogisticLocationID.ToString();
                //hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;

                hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
                hdnGCCustomerType.Value = entity.GCCustomerType;
                hdnClassID.Value = entity.ChargeClassID.ToString();

                //IsLoadFirstRecord = (OnGetRowCount() > 0);
                //if (IsLoadFirstRecord)
                //{
                //    if (hdnTestOrderID.Value != "0")
                //    {
                //        string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND TestOrderID = {2} AND GCTransactionStatus != '{3}'", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value, hdnTestOrderID.Value, Constant.TransactionStatus.VOID);
                //        if (BusinessLayer.GetvPatientChargesHd2RowCount(filterExpression) <= 0) IsLoadFirstRecord = false;
                //    }
                //}

                string filterCh1 = string.Format("TestOrderID = '{0}' AND GCTransactionStatus <> '{1}'", hdnTestOrderID.Value, Constant.TransactionStatus.VOID);
                PatientChargesHd entityPatientChargesHd = BusinessLayer.GetPatientChargesHdList(filterCh1).FirstOrDefault();
                if (entityPatientChargesHd != null)
                {
                    IsLoadFirstRecord = true;
                    string filterCh2 = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);
                    pageIndexFirstLoad = BusinessLayer.GetvPatientChargesHd2RowIndex(filterCh2, entityPatientChargesHd.TransactionNo, "TransactionID DESC");
                }
                else
                {
                    if (param[0] != "to") IsLoadFirstRecord = (OnGetRowCount() > 0);
                }

                hdnTransactionHdID.Value = "0";

                List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND IsDeleted = 0", hdnHealthcareServiceUnitID.Value));
                if (lstParamedic.Count == 1)
                {
                    ParamedicMaster paramedic = lstParamedic.FirstOrDefault();
                    hdnPhysicianID.Value = paramedic.ParamedicID.ToString();
                    hdnPhysicianCode.Value = paramedic.ParamedicCode;
                    hdnPhysicianName.Value = paramedic.FullName;
                }
                else
                {
                    hdnPhysicianID.Value = hdnPhysicianCode.Value = hdnPhysicianName.Value = "";
                }

                int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value));
                if (count > 0)
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
                else
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

                ctlService.OnAddRecord();
                ctlDrugMS.OnAddRecord();
                ctlDrugMSReturn.OnAddRecord();
                ctlLogistic.OnAddRecord();

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

                GetSettingParameter();
            }

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_PROPOSE_VALIDASI_TARIF_0, Constant.SettingParameter.LB_IS_SEND_MEDICALNO_TO_LIS_OUTPATIENT_MUST_AFTER_CLOSE_TRANSACTION));
            if (lstParameter.Count > 0)
            {
                hdnValidateTariffOnPropose.Value = lstParameter.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FN_PROPOSE_VALIDASI_TARIF_0)).FirstOrDefault().ParameterValue;
                hdnIsOutpatientMustCloseTransactionForSentToLIS.Value = lstParameter.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_IS_SEND_MEDICALNO_TO_LIS_OUTPATIENT_MUST_AFTER_CLOSE_TRANSACTION)).FirstOrDefault().ParameterValue;
            }
        }

        protected override void SetControlProperties()
        {
            ctlService.SetControlProperties();
            ctlDrugMS.SetControlProperties();
            ctlLogistic.SetControlProperties();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionHdID, new ControlEntrySetting(false, false, false, "0"));

            SetControlEntrySetting(txtTransactionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTransactionDateLab, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTransactionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtTestOrderInfo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsPATest, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAutoTransaction, new ControlEntrySetting(false, false, false, false));
            chkIsAutoTransaction.Checked = false;
            SetControlEntrySetting(chkIsChargesGenerateMCU, new ControlEntrySetting(false, false, false, false));
            chkIsChargesGenerateMCU.Checked = false;
            SetControlEntrySetting(chkIsAIOTransaction, new ControlEntrySetting(true, false, false, false));
            chkIsAIOTransaction.Checked = false;
        }

        public override void OnAddRecord()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            //if (param[0] == "to")
            //    hdnTestOrderID.Value = param[2];
            hdnTestOrderID.Value = "0";
            hdnGCTransactionStatus.Value = Constant.TransactionStatus.OPEN;

            trIsAutoTransaction.Attributes.Add("style", "display:none");
            trIsChargesGenerateMCU.Attributes.Add("style", "display:none");

            imgIsAIOTransaction.Style.Add("display", "none");

            ctlService.OnAddRecord();
            ctlDrugMS.OnAddRecord();
            ctlDrugMSReturn.OnAddRecord();
            ctlLogistic.OnAddRecord();
        }

        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);
            return BusinessLayer.GetvPatientChargesHd2RowCount(filterExpression);
        }

        #region Load Entity
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Empty;
            filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);
            vPatientChargesHd2 entity = BusinessLayer.GetvPatientChargesHd2(filterExpression, PageIndex, " TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {

            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);
            PageIndex = BusinessLayer.GetvPatientChargesHd2RowIndex(filterExpression, keyValue, "TransactionID DESC");
            vPatientChargesHd2 entity = BusinessLayer.GetvPatientChargesHd2(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPatientChargesHd2 entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnTestOrderID.Value = entity.TestOrderID.ToString();
            hdnTransactionHdID.Value = entity.TransactionID.ToString();
            txtTransactionNo.Text = entity.TransactionNo;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtTestOrderInfo.Text = entity.TestOrderInfo;
            hdnProcedureGroupID.Value = entity.ProcedureGroupID.ToString();
            txtProcedureOrderInfo.Text = string.Format("{0} - {1}", entity.ProcedureGroupCode, entity.ProcedureGroupName);
            txtTransactionDateLab.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.TransactionTime;

            bool flagHaveCharges = false;
            if (entity != null) flagHaveCharges = true;

            chkIsAutoTransaction.Checked = entity.IsAutoTransaction;
            if (entity.IsAutoTransaction)
            {
                trIsAutoTransaction.Attributes.Remove("style");
            }
            else
            {
                trIsAutoTransaction.Attributes.Add("style", "display:none");
            }

            if (entity.ConsultVisitItemPackageID != null && entity.ConsultVisitItemPackageID != 0)
            {
                chkIsChargesGenerateMCU.Checked = true;
                trIsChargesGenerateMCU.Attributes.Remove("style");
            }
            else
            {
                chkIsChargesGenerateMCU.Checked = false;
                trIsChargesGenerateMCU.Attributes.Add("style", "display:none");
            }

            if (entity.IsAIOTransaction)
            {
                imgIsAIOTransaction.Src = string.Format("{0}", "~/libs/Images/Status/has_aio_package.png");
                imgIsAIOTransaction.Style.Remove("display");
            }
            else
            {
                imgIsAIOTransaction.Style.Add("display", "none");
            }

            hdnIsHasAIOPackage.Value = entity.IsHasAIOPackage ? "1" : "0";
            if (hdnIsHasAIOPackage.Value == "1")
            {
                trIsAIOTransaction.Attributes.Remove("style");
            }
            else
            {
                trIsAIOTransaction.Attributes.Add("style", "display:none");
            }

            chkIsAIOTransaction.Checked = entity.IsAIOTransaction;
            chkIsPATest.Checked = entity.IsPathologicalAnatomyTest;

            ctlService.InitializeTransactionControl(flagHaveCharges);
            ctlDrugMS.InitializeTransactionControl(flagHaveCharges);
            ctlDrugMSReturn.InitializeTransactionControl(flagHaveCharges);
            ctlLogistic.InitializeTransactionControl(flagHaveCharges);
        }
        #endregion

        #region Save Entity
        public override void SaveTransactionHeader(IDbContext ctx, ref int transactionID)
        {
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesHdInfoDao entityHdInfoDao = new PatientChargesHdInfoDao(ctx);
            if (hdnTransactionHdID.Value == "0")
            {
                PatientChargesHd entityHd = new PatientChargesHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                //entityHd.TransactionDate = Helper.GetDatePickerValue(txtTransactionDateLab.Text);
                entityHd.TransactionDate = Helper.GetDatePickerValue(GetTransactionDate());
                entityHd.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.PatientBillingID = null;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.GCVoidReason = null;
                entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TransactionDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                transactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                PatientChargesHdInfo entityHdInfo = entityHdInfoDao.Get(transactionID);
                entityHdInfo.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                entityHdInfo.LastUpdatedPAInfoBy = AppSession.UserLogin.UserID;
                entityHdInfo.LastUpdatedPAInfoDate = DateTime.Now;
                entityHdInfoDao.Update(entityHdInfo);
            }
            else
            {
                transactionID = Convert.ToInt32(hdnTransactionHdID.Value);

                PatientChargesHdInfo entityHdInfo = entityHdInfoDao.Get(transactionID);
                entityHdInfo.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                entityHdInfo.LastUpdatedPAInfoBy = AppSession.UserLogin.UserID;
                entityHdInfo.LastUpdatedPAInfoDate = DateTime.Now;
                entityHdInfoDao.Update(entityHdInfo);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesHdInfoDao entityHdInfoDao = new PatientChargesHdInfoDao(ctx);
            try
            {
                PatientChargesHd entityHd = new PatientChargesHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;

                entityHd.TransactionDate = Helper.GetDatePickerValue(GetTransactionDate());
                entityHd.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.PatientBillingID = null;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.GCVoidReason = null;
                entityHd.TotalPatientAmount = 0;
                entityHd.TotalPayerAmount = 0;
                entityHd.TotalAmount = 0;
                entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TransactionDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                retval = entityHdDao.InsertReturnPrimaryKeyID(entityHd).ToString();

                PatientChargesHdInfo entityHdInfo = entityHdInfoDao.Get(Convert.ToInt32(retval));
                entityHdInfo.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                entityHdInfo.LastUpdatedPAInfoBy = AppSession.UserLogin.UserID;
                entityHdInfo.LastUpdatedPAInfoDate = DateTime.Now;
                entityHdInfoDao.Update(entityHdInfo);

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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionHdID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.ReferenceNo = txtReferenceNo.Text;
                    entity.TransactionDate = Helper.GetDatePickerValue(GetTransactionDate());
                    entity.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePatientChargesHd(entity);

                    PatientChargesHdInfo entityHdInfo = BusinessLayer.GetPatientChargesHdInfo(entity.TransactionID);
                    entityHdInfo.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                    entityHdInfo.LastUpdatedPAInfoBy = AppSession.UserLogin.UserID;
                    entityHdInfo.LastUpdatedPAInfoDate = DateTime.Now;
                    BusinessLayer.UpdatePatientChargesHdInfo(entityHdInfo);

                    if (entity.TestOrderID != null && entity.TestOrderID != 0)
                    {
                        TestOrderHd orderHd = BusinessLayer.GetTestOrderHd(Convert.ToInt32(entity.TestOrderID));
                        orderHd.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                        BusinessLayer.UpdateTestOrderHd(orderHd);
                    }

                    return true;
                }
                else
                {
                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entity.TransactionNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
        #endregion

        //#region Void Entity
        //protected override bool OnVoidRecord(ref string errMessage)
        //{
        //    bool result = true;
        //    IDbContext ctx = DbFactory.Configure(true);
        //    PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
        //    TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
        //    TestOrderDtDao orderDtDao = new TestOrderDtDao(ctx);
        //    try
        //    {
        //        Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
        //        PatientChargesHd entity = entityHdDao.Get(TransactionID);
        //        if (entity.PatientBillingID == null && (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL))
        //        {
        //            entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
        //            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
        //            entityHdDao.Update(entity);

        //            ((PatientManagementTransactionDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
        //            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
        //            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnVoidAllChargesDt(ctx, TransactionID);
        //            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);

        //            //Update Status TestOrderHd
        //            //TestOrderHd orderHd = orderHdDao.Get((int)entity.TestOrderID);
        //            //if (orderHd != null)
        //            if (entity.TestOrderID != null)
        //            {
        //                TestOrderHd orderHd = orderHdDao.Get((int)entity.TestOrderID);
        //                if (orderHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
        //                {
        //                    orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
        //                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
        //                    orderHdDao.Update(orderHd);

        //                    List<TestOrderDt> lstTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted = 0", entity.TestOrderID.ToString()), ctx);
        //                    foreach (TestOrderDt orderDt in lstTestOrderDt)
        //                    {
        //                        if (orderDt != null && orderDt.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
        //                        {
        //                            if (!orderDt.IsDeleted)
        //                            {
        //                                orderDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
        //                                orderDt.BusinessPartnerID = null;
        //                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
        //                                orderDtDao.Update(orderDt);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            errMessage = "Transaksi Sudah Diproses. Tidak Bisa dibatalkan";
        //            result = false;
        //        }
        //        ctx.CommitTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        ctx.RollBackTransaction();
        //        errMessage = ex.Message;
        //        result = false;
        //    }
        //    finally
        //    {
        //        ctx.Close();
        //    }
        //    return result;
        //}
        //#endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];
            bool result = true;

            if (param[0] == "void")
            {
                IDbContext ctx = DbFactory.Configure(true);
                TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
                TestOrderDtDao orderDtDao = new TestOrderDtDao(ctx);
                PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
                //ChargesStatusLogDao logDao = new ChargesStatusLogDao(ctx);
                try
                {
                    //ChargesStatusLog log = new ChargesStatusLog();
                    //string statusOld = "", statusNew = "";
                    Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
                    PatientChargesHd entity = entityHdDao.Get(TransactionID);
                    //statusOld = entity.GCTransactionStatus;
                    if (AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entity.PatientBillingID == null && (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL))
                        {
                            ((PatientManagementTransactionDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
                            //((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);

                            entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entity.GCVoidReason = gcDeleteReason;
                            if (gcDeleteReason == Constant.DeleteReason.OTHER)
                            {
                                entity.VoidReason = reason;
                            }
                            entity.VoidBy = AppSession.UserLogin.UserID;
                            entity.VoidDate = DateTime.Now;
                            entityHdDao.Update(entity);

                            //statusNew = entity.GCTransactionStatus;

                            //log.VisitID = entity.VisitID;
                            //log.TransactionID = entity.TransactionID;
                            //log.LogDate = DateTime.Now;
                            //log.UserID = AppSession.UserLogin.UserID;
                            //log.GCTransactionStatusOLD = statusOld;
                            //log.GCTransactionStatusNEW = statusNew;
                            //logDao.Insert(log);

                            //Update Status TestOrderHd
                            if (entity.TestOrderID != null)
                            {
                                TestOrderHd orderHd = orderHdDao.Get((int)entity.TestOrderID);
                                if (orderHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                {
                                    orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    orderHdDao.Update(orderHd);

                                    List<TestOrderDt> lstTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted = 0", entity.TestOrderID.ToString()), ctx);
                                    foreach (TestOrderDt orderDt in lstTestOrderDt)
                                    {
                                        if (orderDt != null && orderDt.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                        {
                                            if (!orderDt.IsDeleted)
                                            {
                                                orderDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                                                orderDt.BusinessPartnerID = null;
                                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                orderDtDao.Update(orderDt);
                                            }
                                        }
                                    }
                                }
                            }
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat dibatalkan lagi.", entity.TransactionNo);
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            ((PatientManagementTransactionDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
                            //((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);

                            entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entity.GCVoidReason = gcDeleteReason;
                            if (gcDeleteReason == Constant.DeleteReason.OTHER)
                            {
                                entity.VoidReason = reason;
                            }
                            entity.VoidBy = AppSession.UserLogin.UserID;
                            entity.VoidDate = DateTime.Now;
                            entityHdDao.Update(entity);

                            //statusNew = entity.GCTransactionStatus;

                            //log.VisitID = entity.VisitID;
                            //log.TransactionID = entity.TransactionID;
                            //log.LogDate = DateTime.Now;
                            //log.UserID = AppSession.UserLogin.UserID;
                            //log.GCTransactionStatusOLD = statusOld;
                            //log.GCTransactionStatusNEW = statusNew;
                            //logDao.Insert(log);

                            //Update Status TestOrderHd
                            if (entity.TestOrderID != null)
                            {
                                TestOrderHd orderHd = orderHdDao.Get((int)entity.TestOrderID);
                                if (orderHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                {
                                    orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    orderHdDao.Update(orderHd);

                                    List<TestOrderDt> lstTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted = 0", entity.TestOrderID.ToString()), ctx);
                                    foreach (TestOrderDt orderDt in lstTestOrderDt)
                                    {
                                        if (orderDt != null && orderDt.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                        {
                                            if (!orderDt.IsDeleted)
                                            {
                                                orderDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                                                orderDt.BusinessPartnerID = null;
                                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                orderDtDao.Update(orderDt);
                                            }
                                        }
                                    }
                                }
                            }
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat dibatalkan lagi.", entity.TransactionNo);
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
            }
            return result;
        }

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesHdInfoDao entityHdInfoDao = new PatientChargesHdInfoDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao orderDtDao = new TestOrderDtDao(ctx);

            try
            {
                if (hdnValidateTariffOnPropose.Value == "1")
                {
                    string filterExpression = String.Format("TransactionID = {0} AND Tariff = 0 AND IsDeleted = 0", hdnTransactionHdID.Value);
                    List<PatientChargesDt> lstDetail = BusinessLayer.GetPatientChargesDtList(filterExpression, ctx);
                    if (lstDetail.Count > 0)
                    {
                        errMessage = "Masih ada item yang belum memiliki tariff, proses tidak bisa dilanjutkan!";
                        return false;
                    }
                }

                if (hdnTestOrderID.Value != "" && hdnTestOrderID.Value != "0")
                {
                    TestOrderHd testOrderHd = orderHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                    if (testOrderHd.GCTransactionStatus != Constant.TransactionStatus.PROCESSED && testOrderHd.GCTransactionStatus != Constant.TransactionStatus.VOID && testOrderHd.GCTransactionStatus != Constant.TransactionStatus.CLOSED)
                    {
                        errMessage = "Masih Ada Test Order Yang Belum Diproses. Silakan approve / decline / dijadwalkan terlebih dahulu";
                        return false;
                    }
                }

                Int32 transactionID = Convert.ToInt32(hdnTransactionHdID.Value);
                PatientChargesHd entity = entityHdDao.Get(transactionID);
                if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterChargesDt = string.Format("TransactionID = {0}  AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{1}'", entity.TransactionID, Constant.TransactionStatus.VOID);
                    List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(filterChargesDt, ctx);
                    foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                    {
                        if ((patientChargesDt.LocationID != null && patientChargesDt.LocationID != 0) && !patientChargesDt.IsApproved)
                        {
                            patientChargesDt.IsApproved = true;
                        }
                        patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;

                        if (entity.TestOrderID != null && patientChargesDt.ReferenceDtID == null && (entity.TransactionCode == Constant.TransactionCode.LABORATORY_CHARGES || entity.TransactionCode == Constant.TransactionCode.IMAGING_CHARGES || entity.TransactionCode == Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES))
                        {
                            TestOrderDt orderDt = new TestOrderDt();
                            orderDt.TestOrderID = Convert.ToInt32(entity.TestOrderID);
                            orderDt.ItemID = patientChargesDt.ItemID;
                            orderDt.ParamedicID = patientChargesDt.ParamedicID;
                            orderDt.GCTestOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                            orderDt.ItemQty = patientChargesDt.ChargedQuantity;
                            orderDt.IsCITO = patientChargesDt.IsCITO;
                            orderDt.ItemUnit = patientChargesDt.GCItemUnit;
                            orderDt.IsCreatedFromOrder = false;
                            orderDt.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            int oReferenceDtID = orderDtDao.InsertReturnPrimaryKeyID(orderDt);

                            if (oReferenceDtID != null && oReferenceDtID != 0)
                            {
                                patientChargesDt.ReferenceDtID = oReferenceDtID;
                            }
                        }

                        patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Update(patientChargesDt);
                    }

                    int oTestOrderID;
                    if (entity.TestOrderID == null && (entity.TransactionCode == Constant.TransactionCode.LABORATORY_CHARGES || entity.TransactionCode == Constant.TransactionCode.IMAGING_CHARGES || entity.TransactionCode == Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES))
                    {
                        #region Generate Test Order Hd + Dt

                        TestOrderHd orderHd = new TestOrderHd();
                        orderHd.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                        orderHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;

                        if (hdnReferrerParamedicID.Value != null && hdnReferrerParamedicID.Value != "" && hdnReferrerParamedicID.Value != "0")
                        {
                            orderHd.ParamedicID = Convert.ToInt32(hdnReferrerParamedicID.Value);
                        }
                        else
                        {
                            orderHd.ParamedicID = Convert.ToInt32(hdnRegistrationPhysicianID.Value);
                        }

                        orderHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                        orderHd.VisitID = entity.VisitID;
                        orderHd.TestOrderDate = entity.TransactionDate;
                        orderHd.TestOrderTime = entity.TransactionTime;
                        orderHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
                        orderHd.ScheduledDate = entity.TransactionDate;
                        orderHd.ScheduledTime = entity.TransactionTime;
                        orderHd.Remarks = entity.Remarks;
                        orderHd.IsCITO = false;
                        orderHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                        orderHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(orderHd.TransactionCode, orderHd.TestOrderDate, ctx);
                        orderHd.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                        orderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        orderHd.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                        orderHd.IsCreatedBySystem = true;
                        orderHd.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        oTestOrderID = Convert.ToInt32(orderHdDao.InsertReturnPrimaryKeyID(orderHd));

                        foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                        {
                            TestOrderDt orderDt = new TestOrderDt();
                            orderDt.TestOrderID = oTestOrderID;
                            orderDt.ItemID = patientChargesDt.ItemID;
                            orderDt.ParamedicID = patientChargesDt.ParamedicID;
                            orderDt.GCTestOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                            orderDt.ItemQty = patientChargesDt.ChargedQuantity;
                            orderDt.IsCITO = patientChargesDt.IsCITO;
                            orderDt.ItemUnit = patientChargesDt.GCItemUnit;
                            orderDt.IsCreatedFromOrder = false;
                            orderDt.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            orderDtDao.Insert(orderDt);
                        }

                        #endregion

                        entity.TestOrderID = oTestOrderID;
                    }

                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHdDao.Update(entity);

                    PatientChargesHdInfo hdInfo = entityHdInfoDao.Get(entity.TransactionID);
                    hdInfo.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                    hdInfo.LastUpdatedPAInfoBy = AppSession.UserLogin.UserID;
                    hdInfo.LastUpdatedPAInfoDate = DateTime.Now;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHdInfoDao.Update(hdInfo);

                    //if (AppSession.IsBridgingToQueue)
                    //{
                    //    //If Bridging to Queue - Send Information
                    //    if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT || hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC || hdnDepartmentID.Value == Constant.Facility.LABORATORY)
                    //    {
                    //        try
                    //        {
                    //            OrderDTO oData = new OrderDTO();
                    //            vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
                    //            oData = ConvertOrderToDTO(entity, oVisit);
                    //            QueueService oService = new QueueService();
                    //            string apiResult = oService.SendOrderInformation(oData);
                    //            string[] apiResultInfo = apiResult.Split('|');
                    //            if (apiResultInfo[0] == "0")
                    //            {
                    //                Exception ex = new Exception(apiResultInfo[1]);
                    //                Helper.InsertErrorLog(ex);
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            Helper.InsertErrorLog(ex);
                    //        }
                    //    }
                    //}

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat di-proposed lagi.", entity.TransactionNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                return true;
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        private OrderDTO ConvertOrderToDTO(PatientChargesHd chargesHd, vConsultVisit oVisit)
        {
            OrderDTO oData = new OrderDTO();

            oData.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
            oData.RegistrationNo = oVisit.RegistrationNo;
            oData.PatientID = AppSession.RegisteredPatient.MRN;
            oData.PatientInfo = new PatientData() { PatientID = oData.PatientID, MedicalNo = AppSession.RegisteredPatient.MedicalNo, FullName = AppSession.RegisteredPatient.PatientName };
            oData.VisitID = AppSession.RegisteredPatient.VisitID;
            if (oVisit != null)
            {
                oData.VisitInformation = new VisitInfo()
                {
                    VisitID = oVisit.VisitID,
                    VisitDate = oVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112),
                    VisitTime = oVisit.VisitTime,
                    PhysicianID = oVisit.ParamedicID,
                    PhysicianCode = oVisit.ParamedicCode,
                    PhysicianName = oVisit.ParamedicName
                };
            }
            else
            {
                oData.VisitInformation = new VisitInfo()
                {
                    VisitID = AppSession.RegisteredPatient.VisitID,
                    VisitDate = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112)
                };
            }
            oData.TransactionNo = chargesHd.TransactionNo;
            oData.TransactionDate = chargesHd.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            oData.TransactionTime = chargesHd.TransactionTime;
            oData.OrderType = "LABORATORY";
            oData.IsCompound = false;

            List<OrderDt> lstOrderDt = new List<OrderDt>();
            List<vPatientChargesDt> lstChargesDt = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", chargesHd.TransactionID));
            foreach (vPatientChargesDt item in lstChargesDt)
            {
                OrderDt orderDt = new OrderDt() { itemCode = item.ItemCode, itemName = item.ItemName1 };
                lstOrderDt.Add(orderDt);
            }
            oData.DetailList = lstOrderDt;
            return oData;
        }
    }
}