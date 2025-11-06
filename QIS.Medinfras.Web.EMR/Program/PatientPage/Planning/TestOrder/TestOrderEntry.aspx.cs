using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;
using System.Text;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class TestOrderEntry : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.TEST_ORDER;
        }

        #region List
        protected override void InitializeDataControl()
        {
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND IsDeleted = 0", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            List<vHealthcareServiceUnitCustom> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0 AND IsUsingJobOrder = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC));
            Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                AppSession.UserLogin.HealthcareID,
                Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM,
                Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI,
                Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE,
                Constant.SettingParameter.NOTES_TEST_ORDER_COPY_DIAGNOSE,
                Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE,
                Constant.SettingParameter.EM0091,
                Constant.SettingParameter.EM0092,
                Constant.SettingParameter.EM0094,
                Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN));

            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnOperatingRoomID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE).ParameterValue;
            hdnIsNotesCopyDiagnose.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.NOTES_TEST_ORDER_COPY_DIAGNOSE).ParameterValue;
            hdnEM0079.Value = AppSession.EM0079;
            hdnIsUsingMultiVisitScheduleOrder.Value = lstSettingParameter.Where(w => w.ParameterCode == Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE).FirstOrDefault().ParameterValue;
            hdnIsLimitedCPOEItemForBPJSLab.Value = lstSettingParameter.Where(w => w.ParameterCode == Constant.SettingParameter.EM0091).FirstOrDefault().ParameterValue;
            hdnIsLimitedCPOEItemForBPJSRad.Value = lstSettingParameter.Where(w => w.ParameterCode == Constant.SettingParameter.EM0092).FirstOrDefault().ParameterValue;
            hdnIsLimitedCPOEItemForBPJSOth.Value = lstSettingParameter.Where(w => w.ParameterCode == Constant.SettingParameter.EM0094).FirstOrDefault().ParameterValue;

            SettingParameterDt oParam1 = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault();

            string bpjsID = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault().ParameterValue;

            if (string.IsNullOrEmpty(bpjsID))
                bpjsID = "0";

            if (AppSession.RegisteredPatient.BusinessPartnerID == Convert.ToInt32(bpjsID))
            {
                hdnIsBPJS.Value = "1";
            }

            List<StandardCode> lstToBePerformed = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.TO_BE_PERFORMED));
            Methods.SetComboBoxField<StandardCode>(cboToBePerformed, lstToBePerformed, "StandardCodeName", "StandardCodeID");

            cboToBePerformed.SelectedIndex = 0;

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            if (Page.Request.QueryString.Count > 0)
            {
                hdnTestOrderID.Value = Page.Request.QueryString["id"];
            }

            if (hdnTestOrderID.Value != "")
            {
                TestOrderHd entity = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));

                txtTestOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTestOrderTime.Text = entity.TestOrderTime;
                cboParamedicID.ClientEnabled = false;
                cboParamedicID.Value = entity.ParamedicID.ToString();
                cboServiceUnit.ClientEnabled = false;
                cboServiceUnit.Value = entity.HealthcareServiceUnitID.ToString();
                cboToBePerformed.Value = entity.GCToBePerformed;
                if (hdnIsUsingMultiVisitScheduleOrder.Value == "1")
                {
                    vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID)).FirstOrDefault();
                    if (hsu != null)
                    {
                        if (hsu.IsAllowMultiVisitSchedule)
                        {
                            chkIsMultiVisitScheduleOrder.Checked = entity.IsMultiVisitScheduleOrder;
                            tdMultiVisitScheduleOrder.Attributes.Remove("style");
                        }
                        else
                        {
                            tdMultiVisitScheduleOrder.Attributes.Add("style", "display:none");
                        }
                    }
                    else
                    {
                        tdMultiVisitScheduleOrder.Attributes.Add("style", "display:none");
                    }
                }
                else
                {
                    tdMultiVisitScheduleOrder.Attributes.Add("style", "display:none");
                }
                if (entity.GCToBePerformed != Constant.ToBePerformed.SCHEDULLED)
                {
                    txtPerformDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtPerformTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                }
                else
                {
                    txtPerformDate.Text = entity.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtPerformTime.Text = entity.ScheduledTime;
                };

                chkIsCITO.Checked = entity.IsCITO;
                chkIsPathologicalAnatomyTest.Checked = entity.IsPathologicalAnatomyTest;
                hdnIsProposed.Value = entity.GCTransactionStatus == Constant.TransactionStatus.OPEN ? "0" : "1";
            }
            else
            {
                //hdnTestOrderID.Value = "";
                txtTestOrderDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTestOrderTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                txtPerformDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtPerformTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
                {
                    int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                    cboParamedicID.ClientEnabled = false;
                    cboParamedicID.Value = userLoginParamedic.ToString();
                }

                cboServiceUnit.SelectedIndex = 0;
                hdnIsProposed.Value = "0";
            }

            hdnDatePickerToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTimeToday.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            hdnPatientInformation.Value = string.Format("{0} (MRN = {1}, REG = {2}, LOC = {3}, DOB = {4})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.RegistrationNo, AppSession.RegisteredPatient.ServiceUnitName, AppSession.RegisteredPatient.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT));

            hdnDefaultVisitParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (hdnIsNotesCopyDiagnose.Value == "1")
            {
                string filterDiag = string.Format("VisitID = {0} AND ParamedicID = {1} AND IsDeleted = 0 ORDER BY GCDiagnoseType", hdnVisitID.Value, hdnDefaultVisitParamedicID.Value);
                List<vPatientDiagnosis> lstDiagnosis = BusinessLayer.GetvPatientDiagnosisList(filterDiag);

                //Create Diagnosis Summary for : CPOE Clinical Notes
                StringBuilder strDiagnosis = new StringBuilder();
                foreach (var item in lstDiagnosis)
                {
                    if (item.GCDifferentialStatus != Constant.DifferentialDiagnosisStatus.RULED_OUT)
                    {
                        strDiagnosis.AppendLine(string.Format("{0}", item.cfDiagnosisText));
                    }
                }
                hdnDefaultDiagnosa.Value = strDiagnosis.ToString();
            }
            else
            {
                hdnDefaultDiagnosa.Value = "";
            }
            
            string filterCC = string.Format("VisitID = {0} AND ParamedicID = {1} AND IsDeleted = 0 ORDER BY ID", hdnVisitID.Value, hdnDefaultVisitParamedicID.Value);
            List<vChiefComplaint> lstChiefComplaint = BusinessLayer.GetvChiefComplaintList(filterCC);
            if (lstChiefComplaint.Count > 0)
            {
                //Create Diagnosis Summary for : CPOE Clinical Notes
                StringBuilder strChiefComplaint = new StringBuilder();
                foreach (var item in lstChiefComplaint)
                {
                    strChiefComplaint.AppendLine(string.Format("{0}", item.ChiefComplaintText));
                }
                hdnDefaultChiefComplaint.Value = strChiefComplaint.ToString();
            }
            else
            {
                hdnDefaultChiefComplaint.Value = "";
            }

            if (hdnImagingServiceUnitID.Value == hdnHealthcareServiceUnitID.Value && AppSession.EM0079 == "0")
            {
                hdnDefaultChiefComplaint.Value = hdnDefaultDiagnosa.Value = hdnRemarks.Value = string.Empty;
            }

            if (string.IsNullOrEmpty(hdnDefaultDiagnosa.Value))
            {
                txtRemarks.Text = hdnDefaultChiefComplaint.Value;
                hdnRemarks.Value = hdnDefaultChiefComplaint.Value;
            }
            else
            {
                txtRemarks.Text = hdnDefaultDiagnosa.Value;
                hdnRemarks.Value = hdnDefaultDiagnosa.Value;
            }

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnTestOrderID.Value != "")
            {
                filterExpression = hdnFilterExpression.Value;
                if (filterExpression != "")
                    filterExpression += " AND ";
                filterExpression += string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnTestOrderID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvTestOrderDtRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vTestOrderDt> lstEntity = BusinessLayer.GetvTestOrderDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
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
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                TestOrderDt entity = BusinessLayer.GetTestOrderDt(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateTestOrderDt(entity);
                return true;
            }
            return false;
        }
        #endregion

        #region Entry
        protected override void SetControlProperties()
        {
            string filterExpression = "HealthcareServiceUnitID = {HealthcareServiceUnitID}";
            hdnFilterExpressionItemNewTransHd.Value = filterExpression;
            if (hdnTestOrderID.Value != "")
            {
                string tempFilterExpression = filterExpression + string.Format(" AND ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = {0} AND IsDeleted = 0)", hdnTestOrderID.Value);
                hdnFilterExpressionItemAdd.Value = tempFilterExpression;

                tempFilterExpression = filterExpression + string.Format(" AND (ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = {0} AND IsDeleted = 0) OR ItemID = {{ItemID}})", hdnTestOrderID.Value);
                hdnFilterExpressionItemEdit.Value = tempFilterExpression;
            }
            else
            {
                string tempFilterExpression = filterExpression + string.Format(" AND ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = {{TestOrderID}} AND IsDeleted = 0)");
                hdnFilterExpressionItemAdd.Value = tempFilterExpression;

                tempFilterExpression = filterExpression + string.Format(" AND (ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = {{TestOrderID}} AND IsDeleted = 0) OR ItemID = {{ItemID}})");
                hdnFilterExpressionItemEdit.Value = tempFilterExpression;
            }

            List<PatientDiagnosis> lstDiagnose = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID));
            Methods.SetComboBoxField<PatientDiagnosis>(cboDiagnose, lstDiagnose, "DiagnosisText", "DiagnoseID");

            SetControlEntrySetting(cboDiagnose, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboToBePerformed, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPerformDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPerformTime, new ControlEntrySetting(true, true, true, "00:00"));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, true));
        }

        private void ControlToEntity(TestOrderDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.ItemQty = Convert.ToDecimal(txtItemQty.Text);
            if (cboDiagnose.Value != null && cboDiagnose.Value != "")
            {
                entityDt.DiagnoseID = cboDiagnose.Value.ToString();
            }
            entityDt.Remarks = txtRemarks.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            ItemMasterDao entityItemMasterDao = new ItemMasterDao(ctx);
            try
            {
                TestOrderHd entityHd = null;
                if (hdnTestOrderID.Value == "")
                {
                    entityHd = new TestOrderHd();
                    entityHd.HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
                    entityHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
                    entityHd.TestOrderDate = Helper.GetDatePickerValue(txtTestOrderDate);
                    entityHd.TestOrderTime = txtTestOrderTime.Text;
                    entityHd.GCToBePerformed = cboToBePerformed.Value.ToString();
                    if (cboToBePerformed.Value.ToString() == Constant.ToBePerformed.SCHEDULLED)
                    {
                        entityHd.ScheduledDate = Helper.GetDatePickerValue(Request.Form[txtPerformDate.UniqueID]);
                        entityHd.ScheduledTime = Request.Form[txtPerformTime.UniqueID];
                    }
                    else
                    {
                        entityHd.ScheduledDate = entityHd.TestOrderDate;
                        entityHd.ScheduledTime = entityHd.TestOrderTime;
                    }
                    entityHd.IsCITO = chkIsCITO.Checked;
                    entityHd.IsMultiVisitScheduleOrder = chkIsMultiVisitScheduleOrder.Checked;
                    List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID));
                    string imagingServiceUnitID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                    string laboratoryServiceUnitID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;

                    vHealthcareServiceUnitCustom vsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", entityHd.HealthcareServiceUnitID)).FirstOrDefault();

                    if (entityHd.HealthcareServiceUnitID.ToString() == AppSession.ImagingServiceUnitID)
                        entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                    else if (vsu.IsLaboratoryUnit)
                    {
                        entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                        entityHd.IsPathologicalAnatomyTest = chkIsPathologicalAnatomyTest.Checked;
                    }
                    else if (entityHd.HealthcareServiceUnitID.ToString() == AppSession.RT0001)
                        entityHd.TransactionCode = Constant.TransactionCode.RADIOTHERAPHY_TEST_ORDER;
                    else
                        entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;

                    entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate);
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHd.TestOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                    hdnTestOrderID.Value = entityHd.TestOrderID.ToString();
                }
                else
                {
                    entityHd = entityHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                }
                retval = entityHd.TestOrderID.ToString();
                hdnTestOrderID.Value = retval;

                TestOrderDt entityDt = new TestOrderDt();
                ControlToEntity(entityDt);
                entityDt.ParamedicID = entityItemMasterDao.Get(entityDt.ItemID).DefaultParamedicID;
                entityDt.TestOrderID = entityHd.TestOrderID;
                entityDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                entityDt.ItemQty = 1;
                entityDt.ItemUnit = entityItemMasterDao.Get(entityDt.ItemID).GCItemUnit;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Insert(entityDt);
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
                TestOrderHd entityHd = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
                if (entityHd != null)
                {
                    entityHd.GCToBePerformed = cboToBePerformed.Value.ToString();
                    entityHd.TestOrderDate = Helper.GetDatePickerValue(txtTestOrderDate);
                    entityHd.TestOrderTime = txtTestOrderTime.Text;
                    if (cboToBePerformed.Value.ToString() == Constant.ToBePerformed.SCHEDULLED)
                    {
                        entityHd.ScheduledDate = Helper.GetDatePickerValue(Request.Form[txtPerformDate.UniqueID]);
                        entityHd.ScheduledTime = Request.Form[txtPerformTime.UniqueID];
                    }
                    else
                    {
                        entityHd.ScheduledDate = entityHd.TestOrderDate;
                        entityHd.ScheduledTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    }
                    entityHd.IsCITO = chkIsCITO.Checked;
                    if (entityHd.HealthcareServiceUnitID == Convert.ToInt32(hdnLaboratoryServiceUnitID.Value))
                    {
                       entityHd.IsPathologicalAnatomyTest = chkIsPathologicalAnatomyTest.Checked;
                    }
                    BusinessLayer.UpdateTestOrderHd(entityHd);
                }
                TestOrderDt entity = BusinessLayer.GetTestOrderDt(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateTestOrderDt(entity);
                retval = hdnTestOrderID.Value;
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
        #endregion

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = true;
            IsAllowEdit = false;
            IsAllowDelete = false;
        }

        protected void cbpSendOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int transactionID = Convert.ToInt32(hdnTestOrderID.Value);
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            try
            {
                if (param[0] == "sendOrder")
                {
                    TestOrderHd entity = BusinessLayer.GetTestOrderHdList(String.Format("TestOrderID = {0}", hdnTestOrderID.Value))[0];
                    if (entity.GCTransactionStatus != Constant.TransactionStatus.CLOSED)
                    {
                        entity.GCToBePerformed = cboToBePerformed.Value.ToString();
                        entity.TestOrderDate = Helper.GetDatePickerValue(txtTestOrderDate);
                        entity.TestOrderTime = txtTestOrderTime.Text;
                        entity.IsPathologicalAnatomyTest = chkIsPathologicalAnatomyTest.Checked;
                        if (cboToBePerformed.Value.ToString() == Constant.ToBePerformed.SCHEDULLED)
                        {
                            entity.ScheduledDate = Helper.GetDatePickerValue(Request.Form[txtPerformDate.UniqueID]);
                            entity.ScheduledTime = Request.Form[txtPerformTime.UniqueID];
                        }
                        else
                        {
                            entity.ScheduledDate = entity.TestOrderDate;
                            entity.ScheduledTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        }
                        entity.IsCITO = chkIsCITO.Checked;
                        if (hdnIsUsingMultiVisitScheduleOrder.Value == "1")
                        {
                            if (entity.IsMultiVisitScheduleOrder)
                            {
                                entity.GCOrderStatus = Constant.TestOrderStatus.CLOSED;
                                entity.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                            }
                            else
                            {
                                entity.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            }
                        }
                        else
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        }
                        BusinessLayer.UpdateTestOrderHd(entity);

                        if (AppSession.SA0137 == "1")
                        {
                            if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                            {
                                BridgingToMedinfrasV1(1, entity);
                            }
                        }
                    }
                }
                result += string.Format("success|{0}", errMessage);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID;
        }

        private void BridgingToMedinfrasV1(int ProcessType, TestOrderHd entity)
        {
            APIMessageLog apiLog = new APIMessageLog();
            apiLog.MessageDateTime = DateTime.Now;
            apiLog.Sender = Constant.BridgingVendor.HIS;
            apiLog.Recipient = Constant.BridgingVendor.MEDINFRAS_API;

            MedinfrasV1Service oService = new MedinfrasV1Service();
            string serviceResult = oService.OnSendOrderMedicalDiagnosticServices(ProcessType, entity, null, null);
            string[] serviceResultInfo = serviceResult.Split('|');
            if (serviceResultInfo[0] == "1")
            {
                apiLog.IsSuccess = true;
                apiLog.MessageText = serviceResultInfo[1];
                apiLog.Response = serviceResultInfo[2];
            }
            else
            {
                apiLog.IsSuccess = false;
                apiLog.MessageText = serviceResultInfo[1];
                apiLog.Response = serviceResultInfo[2];
                apiLog.ErrorMessage = serviceResultInfo[2];
            }
            BusinessLayer.InsertAPIMessageLog(apiLog);
        }
    }
}