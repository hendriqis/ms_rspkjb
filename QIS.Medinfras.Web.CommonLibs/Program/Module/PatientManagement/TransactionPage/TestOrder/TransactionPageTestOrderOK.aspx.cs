using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
    public partial class TransactionPageTestOrderOK : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_TEST_ORDER_OK;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_TEST_ORDER_OK;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.PATIENT_TRANSACTION_TEST_ORDER_OK;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.PATIENT_TRANSACTION_TEST_ORDER_OK;
                    return Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_TEST_ORDER_OK;
                default: return Constant.MenuCode.Outpatient.PATIENT_TRANSACTION_TEST_ORDER_OK;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED && !AppSession.RegisteredPatient.IsLockDown);
            IsAllowSave = !AppSession.RegisteredPatient.IsLockDown;
            IsAllowVoid = !AppSession.RegisteredPatient.IsLockDown;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                AppSession.UserLogin.HealthcareID,
                Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE));

            hdnOperatingRoomID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE).ParameterValue;

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnGCRegistrationStatus.Value = AppSession.RegisteredPatient.GCRegistrationStatus;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnClassID.Value = AppSession.RegisteredPatient.ClassID.ToString();

            hdnDefaultVisitParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
            hdnDefaultVisitParamedicCode.Value = AppSession.RegisteredPatient.ParamedicCode;
            hdnDefaultVisitParamedicName.Value = AppSession.RegisteredPatient.ParamedicName;

            cboToBePerformed.SelectedIndex = 0;

            hdnDatePickerToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTimeToday.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        }

        protected string GetServiceUnitFilterFilterExpression()
        {
            return string.Format("HealthcareID = '{0}' AND HealthcareServiceUnitID = {1} AND IsDeleted = 0 AND IsUsingRegistration = 1 AND IsUsingJobOrder = 1",
                                    AppSession.UserLogin.HealthcareID, hdnOperatingRoomID.Value);
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnTestOrderID.Value != "")
                filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0 ORDER BY ID DESC", hdnTestOrderID.Value);
            List<vTestOrderDt> lstEntity = BusinessLayer.GetvTestOrderDtList(filterExpression);
            //grdView.DataSource = lstEntity;
            //grdView.DataBind();
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

        protected override void SetControlProperties()
        {
            string filterExpression;
            filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND StandardCodeID != '{1}'", Constant.StandardCode.TO_BE_PERFORMED, Constant.ToBePerformed.SCHEDULLED);

            List<StandardCode> lstToBePerformed = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboToBePerformed, lstToBePerformed, "StandardCodeName", "StandardCodeID");
            cboToBePerformed.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTestOrderID, new ControlEntrySetting(false, false, false, "0"));

            SetControlEntrySetting(txtTestOderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTestOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTestOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtScheduledDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtScheduledTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnProcedureGroupID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtProcedureGroupCode, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtProcedureGroupName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, true, false, hdnDefaultVisitParamedicID.Value));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, hdnDefaultVisitParamedicCode.Value));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false, hdnDefaultVisitParamedicName.Value));

            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, false, true, hdnDefaultDiagnosa.Value));
            //SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsCITO, new ControlEntrySetting(true, false, false, false));

            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, true));
            SetControlEntrySetting(lblServiceUnit, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblProcedureGroup, new ControlEntrySetting(true, false));

            SetControlEntrySetting(cboToBePerformed, new ControlEntrySetting(true, false, false));
            if (cboToBePerformed.Items.Count > 0)
            {
                cboToBePerformed.SelectedIndex = 0;
            }
        }

        public override void OnAddRecord()
        {
        }

        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1}", hdnVisitID.Value, hdnOperatingRoomID.Value);
            return BusinessLayer.GetvTestOrderHdRowCount(filterExpression);
        }

        #region Load Entity
        protected bool IsEditable = true;
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            vTestOrderHd entity = BusinessLayer.GetvTestOrderHd(filterExpression, PageIndex, " TestOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1}", hdnVisitID.Value, hdnOperatingRoomID.Value);
            PageIndex = BusinessLayer.GetvTestOrderHdRowIndex(filterExpression, keyValue, "TestOrderID DESC");
            vTestOrderHd entity = BusinessLayer.GetvTestOrderHd(filterExpression, PageIndex, "TestOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vTestOrderHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            IsEditable = (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN);
            Registration entityRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
            IsEditable = entityRegistration.IsLockDown ? false : IsEditable;
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnTestOrderID.Value = entity.TestOrderID.ToString();
            txtTestOderNo.Text = entity.TestOrderNo;
            txtTestOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTestOrderTime.Text = entity.TestOrderTime;
            cboToBePerformed.Value = entity.GCToBePerformed;
            txtScheduledDate.Text = entity.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduledTime.Text = entity.ScheduledTime;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;

            hdnProcedureGroupID.Value = entity.ProcedureGroupID.ToString();
            txtProcedureGroupCode.Text = entity.ProcedureGroupCode;
            txtProcedureGroupName.Text = entity.ProcedureGroupName;

            if (hdnProcedureGroupID.Value != "" && hdnProcedureGroupID.Value != "0")
            {
                IsEditable = false;
            }

            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            chkIsCITO.Checked = entity.IsCITO;
            txtNotes.Text = entity.Remarks;

            if (entity.GCTransactionStatus == Constant.TransactionStatus.VOID)
            {

                string voidReason = "";

                if (entity.GCVoidReason == Constant.DeleteReason.OTHER)
                {
                    voidReason = entity.VoidReasonWatermark + " ( " + entity.VoidReason + " )";
                }
                else
                {
                    voidReason = entity.VoidReasonWatermark;
                }
            }

            BindGridView();
        }
        #endregion

        #region Save Entity
        public void SaveTestOrderHd(IDbContext ctx, ref int testOrderID)
        {
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            if (hdnTestOrderID.Value == "0")
            {
                TestOrderHd entityHd = new TestOrderHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entityHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entityHd.TestOrderDate = Helper.GetDatePickerValue(Request.Form[txtTestOrderDate.UniqueID]);
                entityHd.TestOrderTime = Request.Form[txtTestOrderTime.UniqueID];
                entityHd.GCToBePerformed = cboToBePerformed.Value.ToString();
                entityHd.ScheduledDate = Helper.GetDatePickerValue(Request.Form[txtScheduledDate.UniqueID]);
                entityHd.ScheduledTime = Request.Form[txtScheduledTime.UniqueID];
                entityHd.IsCITO = chkIsCITO.Checked;
                entityHd.Remarks = txtNotes.Text;
                if (hdnProcedureGroupID.Value != "" && hdnProcedureGroupID.Value != "0")
                {
                    entityHd.ProcedureGroupID = Convert.ToInt32(hdnProcedureGroupID.Value);
                }
                entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);

                if (hdnHealthcareServiceUnitID.Value == hdnImagingServiceUnitID.Value)
                {
                    entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                }
                else if (hdnIsLaboratoryUnit.Value == "1")
                {
                    entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                }
                else
                {
                    entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
                }

                entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
                entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                testOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                testOrderID = Convert.ToInt32(hdnTestOrderID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int testOrderID = 0;
                SaveTestOrderHd(ctx, ref testOrderID);
                ctx.CommitTransaction();
                retval = testOrderID.ToString();
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
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            try
            {
                TestOrderHd entity = entityHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (hdnProcedureGroupID.Value != "" && hdnProcedureGroupID.Value != null && hdnProcedureGroupID.Value != "0")
                    {
                        entity.ProcedureGroupID = Convert.ToInt32(hdnProcedureGroupID.Value);
                    }
                    entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                    entity.IsCITO = chkIsCITO.Checked;
                    entity.Remarks = txtNotes.Text;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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

        #region Void Entity
        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                Int32 TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                TestOrderHd entity = entityHdDao.Get(TestOrderID);
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<TestOrderDt> lstDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0}", TestOrderID), ctx);
                    //foreach (TestOrderDt dt in lstDt)
                    //{
                    //    TestOrderDt entityDt = entityDtDao.Get(dt.ID);
                    //    entityDt.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                    //    entityDt.GCVoidReason = Constant.DeleteReason.OTHER;
                    //    entityDt.VoidReason = "HEADER IS CANCELLED";
                    //    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    //    entityDtDao.Update(entityDt);
                    //}

                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.VoidDate = DateTime.Now;
                    entity.VoidBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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

        //protected override bool OnCustomButtonClick(string type, ref string errMessage)
        //{
        //    string[] param = type.Split(';');
        //    string gcDeleteReason = param[1];
        //    string reason = param[2];
        //    bool result = true;

        //    if (param[0] == "void")
        //    {
        //        IDbContext ctx = DbFactory.Configure(true);
        //        TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
        //        TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
        //        try
        //        {
        //            Int32 TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
        //            TestOrderHd entity = entityHdDao.Get(TestOrderID);
        //            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
        //            {
        //                entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
        //                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
        //                entity.GCVoidReason = gcDeleteReason;
        //                if (gcDeleteReason == Constant.DeleteReason.OTHER)
        //                {
        //                    entity.VoidReason = reason;
        //                }
        //                entity.VoidBy = AppSession.UserLogin.UserID;
        //                entity.VoidDate = DateTime.Now;
        //                entityHdDao.Update(entity);

        //                //List<TestOrderDt> lstDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0}", TestOrderID));
        //                //foreach (TestOrderDt dt in lstDt)
        //                //{
        //                //    TestOrderDt entityDt = entityDtDao.Get(dt.ID);
        //                //    entityDt.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
        //                //    entityDt.GCVoidReason = Constant.DeleteReason.OTHER;
        //                //    entityDt.VoidReason = "HEADER IS CANCELLED";
        //                //    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
        //                //    entityDtDao.Update(entityDt);
        //                //}
        //                ctx.CommitTransaction();
        //            }
        //            else
        //            {
        //                result = false;
        //                errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
        //                Exception ex = new Exception(errMessage);
        //                Helper.InsertErrorLog(ex);
        //                ctx.RollBackTransaction();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            result = false;
        //            errMessage = ex.Message;
        //            Helper.InsertErrorLog(ex);
        //            ctx.RollBackTransaction();
        //        }
        //        finally
        //        {
        //            ctx.Close();
        //        }
        //    }
        //    return result;
        //}
        //#endregion

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                string isCetakBukti = "0";
                string printFormat = string.Empty;
                string isSendNotification = "0";
                string isPrintWhenPropose = "0";

                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                            AppSession.UserLogin.HealthcareID, Constant.SettingParameter.OTOMATIS_CETAK_BUKTI_ORDER_PENUNJANG,
                            Constant.SettingParameter.SA_SISTEM_NOTIFIKASI_ORDER, Constant.SettingParameter.RM_FORMAT_BUKTI_ORDER_PENUNJANG,
                            Constant.SettingParameter.LB_PRINT_AFTER_PROPOSE_ORDER);
                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);

                if (lstParam != null)
                {
                    isCetakBukti = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.OTOMATIS_CETAK_BUKTI_ORDER_PENUNJANG)).FirstOrDefault().ParameterValue;
                    printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_ORDER_PENUNJANG)).FirstOrDefault().ParameterValue;
                    isSendNotification = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_SISTEM_NOTIFIKASI_ORDER)).FirstOrDefault().ParameterValue;
                    isPrintWhenPropose = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_PRINT_AFTER_PROPOSE_ORDER)).FirstOrDefault().ParameterValue;
                }

                Int32 testOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                TestOrderHd entity = entityHdDao.Get(testOrderID);
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    String TestOrderID = Convert.ToString(testOrderID);
                    List<TestOrderDt> lstDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0}", TestOrderID));

                    if (lstDt.Count() == 0)
                    {
                        entity.IsCITO = chkIsCITO.Checked;
                        entity.Remarks = txtNotes.Text;
                        entity.SendOrderBy = AppSession.UserLogin.UserID;
                        entity.SendOrderDateTime = DateTime.Now;
                        entity.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entity.ProposedBy = AppSession.UserLogin.UserID;
                        entity.ProposedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entity);

                        if (isCetakBukti == "1")
                            //PrintOrderReceipt(entity.HealthcareServiceUnitID, testOrderID, printFormat);

                            if (isSendNotification == "1")
                            {
                                try
                                {
                                    HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(hdnHealthcareServiceUnitID.Value));
                                    hdnIPAddress.Value = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                                    if (!String.IsNullOrEmpty(hdnIPAddress.Value))
                                    {
                                        //SendNotification(entity);
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }

                        if (isPrintWhenPropose == "1")
                        {
                            vJobOrderLab entityJob = BusinessLayer.GetvJobOrderLabList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            if (entityJob != null)
                            {
                                //PrintJobOrderLaboratory(entityJob.TestOrderID);
                            }
                        }
                    }
                    else
                    {
                        entity.IsCITO = chkIsCITO.Checked;
                        entity.Remarks = txtNotes.Text;
                        entity.SendOrderBy = AppSession.UserLogin.UserID;
                        entity.SendOrderDateTime = DateTime.Now;
                        entity.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entity.ProposedBy = AppSession.UserLogin.UserID;
                        entity.ProposedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                        //jika semua testOrderDt IsCito 1 dan testOrderHD IsCito 0 maka testOrderHd IsCito 1
                        int entityDtCitoCount = lstDt.Where(t => t.IsCITO).ToList().Count;
                        int entityDtValidCount = lstDt.Where(t => t.IsDeleted == false && t.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED).ToList().Count;

                        if (entityDtCitoCount == entityDtValidCount && entity.IsCITO == false)
                        {
                            entity.IsCITO = true;
                        }

                        //bila remarks di testorderDt blm ada isinya, ambil data dari testorderHd
                        foreach (TestOrderDt e in lstDt)
                        {
                            if (e.Remarks == "")
                            {
                                e.Remarks = txtNotes.Text;
                            }
                            else
                            {
                                //e.Remarks = txtRemarks.Text;
                            }
                            e.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(e);
                        }

                        //cek untuk setiap testOrderDt jika IsCito 0 sementara testOrderHD IsCito 1 maka testOrderDt IsCito 1
                        foreach (TestOrderDt e in lstDt)
                        {
                            if (entity.IsCITO == true)
                            {
                                if (e.IsCITO == false && e.IsDeleted == false && e.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                {
                                    e.IsCITO = true;
                                    if (e.Remarks == "")
                                    {
                                        e.Remarks = txtNotes.Text;
                                    }
                                    else
                                    {
                                        //e.Remarks = txtRemarks.Text;
                                    }
                                    e.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityDtDao.Update(e);
                                }
                            }
                        }

                        entityHdDao.Update(entity);

                        if (isCetakBukti == "1")
                            //PrintOrderReceipt(entity.HealthcareServiceUnitID, testOrderID, printFormat);

                            if (isSendNotification == "1")
                            {
                                try
                                {
                                    HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(hdnHealthcareServiceUnitID.Value));
                                    hdnIPAddress.Value = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                                    if (!String.IsNullOrEmpty(hdnIPAddress.Value))
                                    {
                                        //SendNotification(entity);
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        if (isPrintWhenPropose == "1")
                        {
                            vJobOrderLab entityJob = BusinessLayer.GetvJobOrderLabList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            if (entityJob != null)
                            {
                                //PrintJobOrderLaboratory(entityJob.TestOrderID);
                            }
                        }
                    }

                    //string resultCheckTestOrderGCToBePerformed = onCheckTestOrderGCToBePerformed(testOrderID, ctx);
                    //if (String.IsNullOrEmpty(resultCheckTestOrderGCToBePerformed))
                    //{
                    //    ctx.CommitTransaction();
                    //}
                    //else
                    //{
                    //    result = false;
                    //    errMessage = resultCheckTestOrderGCToBePerformed;
                    //    Exception ex = new Exception(errMessage);
                    //    Helper.InsertErrorLog(ex);
                    //    ctx.RollBackTransaction();
                    //}
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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
        #endregion
    }
}