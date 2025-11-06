using System;
using System.Collections;
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
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientManagementTransactionTestOrderCtl2 : BaseViewPopupCtl
    {
        protected bool IsEditable = true;

        protected string GetServiceUnitFilterFilterExpression()
        {
            return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID != {2} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, AppSession.RegisteredPatient.HealthcareServiceUnitID);
        }

        public override void InitializeDataControl(string param)
        {
            hdnCurrentHealthcareServiceUnitID.Value = param;
            ControlEntryList.Clear();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            string filterexpression = string.Format("  HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}', '{3}')",
                                        AppSession.UserLogin.HealthcareID,
                                        Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI,
                                        Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM,
                                        Constant.SettingParameter.MC_ITEMTAMBAHAN_PROPOSE
                                    );

            List<vSettingParameterDt> lstSettingParameter = BusinessLayer.GetvSettingParameterDtList(filterexpression);
         
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnItemTambahanMCUAutoRealisasi.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MC_ITEMTAMBAHAN_PROPOSE).ParameterValue;

            hdnDatePickerToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTimeToday.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            hdnDefaultVisitParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            OnControlEntrySetting();
            ReInitControl();
            SetControlProperties();
        }

        #region Popup Filter Expression
        protected string OnGetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }
        #endregion

        private void SetControlProperties()
        {
            List<StandardCode> lstToBePerformed = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.TO_BE_PERFORMED));
            Methods.SetComboBoxField<StandardCode>(cboToBePerformed, lstToBePerformed, "StandardCodeName", "StandardCodeID");
            cboToBePerformed.SelectedIndex = 0;

        }

        private void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTestOrderID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtTestOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTestOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTestOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtScheduledDate, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtScheduledTime, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(lblServiceUnit, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtStatus, new ControlEntrySetting(false, false));
            SetControlEntrySetting(cboToBePerformed, new ControlEntrySetting(true, false, false));
            if (cboToBePerformed.Items.Count > 0)
                cboToBePerformed.SelectedIndex = 0;
            SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(chkIsCITO, new ControlEntrySetting(true, false, false));

            if (hdnItemTambahanMCUAutoRealisasi.Value == "1")
            {
                Helper.SetControlEntrySetting(txtExecutorParamedicCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
                trExecutorParamedicID.Style.Remove("display");
               
            }
        }

        private void OnLoadEntity(string keyValue)
        {
            string filterExpression = string.Empty;
            if (keyValue.Equals(string.Empty))
            {
                filterExpression = string.Format("TestOrderNo = '{0}'", txtTestOrderNo.Text);
            }
            else filterExpression = string.Format("TestOrderID = {0}", keyValue);
            vTestOrderHd entity = BusinessLayer.GetvTestOrderHdList(filterExpression).FirstOrDefault();
            EntityToControl(entity);
        }

        public void LoadPage(string testOrderID)
        {
            SetControlEnabled(false);
            SetControlProperties();
            OnLoadEntity(testOrderID);
        }

        protected void cbpMainPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string testOrderID = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "load")
            {
                LoadPage(string.Empty);
            }
            else if (param[0] == "loadaftersave")
            {
                if (!String.IsNullOrEmpty(hdnTestOrderID.Value))
                {
                    if (param.Length > 1)
                    {
                        result += param[1];
                    }
                    LoadPage(hdnTestOrderID.Value);
                }
            }
            else if (param[0] == "new")
            {
                ReInitControl();
                SetControlProperties();
            }
            else if (param[0] == "save")
            {
                if (OnSaveAddRecord(ref errMessage, ref testOrderID))
                {
                    result += string.Format("success|{0}", testOrderID);
                    LoadPage(testOrderID);
                }
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "proposed")
            {
                if (OnProposeRecord(ref errMessage))
                {
                    result += string.Format("success|{0}", hdnTestOrderID.Value);
                    LoadPage(hdnTestOrderID.Value);
                }
                else result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "void")
            {
                if (OnVoidRecord(ref errMessage))
                {
                    result += string.Format("success|{0}", hdnTestOrderID.Value);
                    LoadPage(hdnTestOrderID.Value);
                }
                else result += string.Format("fail|{0}", errMessage);
            }
            //else if (e.Parameter != null && e.Parameter != "")
            //{
            //    BindGridView();
            //    result = "refresh";
            //}

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = testOrderID;
        }

        #region Save Entity
        public void SaveTestOrderHd(IDbContext ctx, ref int testOrderID)
        {
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            if (hdnTestOrderID.Value == "0")
            {
                TestOrderHd entityHd = new TestOrderHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entityHd.FromHealthcareServiceUnitID = Convert.ToInt32(hdnCurrentHealthcareServiceUnitID.Value);
                entityHd.VisitHealthcareServiceUnitID = Convert.ToInt32(hdnCurrentHealthcareServiceUnitID.Value);
                entityHd.TestOrderDate = Helper.GetDatePickerValue(Request.Form[txtTestOrderDate.UniqueID]);
                entityHd.TestOrderTime = Request.Form[txtTestOrderTime.UniqueID];
                entityHd.GCToBePerformed = cboToBePerformed.Value.ToString();
                entityHd.ScheduledDate = Helper.GetDatePickerValue(Request.Form[txtScheduledDate.UniqueID]);
                entityHd.ScheduledTime = Request.Form[txtScheduledTime.UniqueID];
                entityHd.IsCITO = chkIsCITO.Checked;
                entityHd.Remarks = txtNotes.Text;
                if (hdnPhysicianID.Value == null || hdnPhysicianID.Value == "")
                    entityHd.ParamedicID = Convert.ToInt32(hdnDefaultVisitParamedicID.Value);
                else
                    entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                if (hdnHealthcareServiceUnitID.Value == hdnImagingServiceUnitID.Value)
                    entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                else if (hdnHealthcareServiceUnitID.Value == hdnLaboratoryServiceUnitID.Value)
                    entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                else
                    entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
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
                TestOrderHd entityHd = new TestOrderHd();
                testOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                entityHd = BusinessLayer.GetTestOrderHd(testOrderID);
                entityHd.Remarks = txtNotes.Text;
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entityHd);
            }
        }

        private bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int testOrderID = 0;
                SaveTestOrderHd(ctx, ref testOrderID);
                hdnTestOrderID.Value = testOrderID.ToString();
                retval = testOrderID.ToString();
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

        private bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                string x = retval;
                TestOrderHd entity = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.IsCITO = chkIsCITO.Checked;
                    entity.Remarks = txtNotes.Text;
                    BusinessLayer.UpdateTestOrderHd(entity);
                    return true;
                }
                else
                {
                    errMessage = "Order " + entity.TestOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        #region Void Entity
        private bool OnVoidRecord(ref string errMessage)
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
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.GCVoidReason = Constant.DeleteReason.OTHER;
                    entity.VoidReason = "HEADER IS CANCELLED";
                    entity.VoidDate = DateTime.Now;
                    entity.VoidBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);

                    List<TestOrderDt> lstDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0}", TestOrderID));
                    foreach (TestOrderDt dt in lstDt)
                    {
                        TestOrderDt entityDt = entityDtDao.Get(dt.ID);
                        entityDt.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                        entityDt.GCVoidReason = Constant.DeleteReason.OTHER;
                        entityDt.VoidReason = "HEADER IS CANCELLED";
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Order " + entity.TestOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        #region Proposed Entity
        private bool OnProposeRecord(ref string errMessage)
        {
            try
            {
                Int32 testOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                TestOrderHd entity = BusinessLayer.GetTestOrderHd(testOrderID);
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;

                    List<TestOrderDt> lstDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0}", hdnTestOrderID.Value));
                    if (lstDt != null)
                    {
                        //jika semua testOrderDt IsCito 1 dan testOrderHD IsCito 0 maka testOrderHd IsCito 1
                        int entityDtCitoCount = lstDt.Where(t => t.IsCITO).ToList().Count;
                        int entityDtValidCount = lstDt.Where(t => t.IsDeleted == false && t.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED).ToList().Count;

                        if (entityDtCitoCount == entityDtValidCount && entity.IsCITO == false)
                        {
                            entity.IsCITO = true;
                        }

                        //cek untuk setiap testOrderDt jika IsCito 0 sementara testOrderHD IsCito 1 maka testOrderDt IsCito 1
                        foreach (TestOrderDt e in lstDt)
                        {
                            if (entity.IsCITO == true)
                            {
                                if (e.IsCITO == false && e.IsDeleted == false && e.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                {
                                    e.IsCITO = true;
                                    BusinessLayer.UpdateTestOrderDt(e);
                                }
                            }
                        }
                    }

                    entity.Remarks = txtNotes.Text;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateTestOrderHd(entity);

                    return true;
                }
                else
                {
                    errMessage = "Order " + entity.TestOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        #region Process Details
        protected void cbpViewCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int testOrderID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    testOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref testOrderID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                testOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                if (OnDeleteEntityDt(ref errMessage, testOrderID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTestOrderID"] = testOrderID.ToString();
        }

        private void ControlToEntity(TestOrderDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            if (!string.IsNullOrEmpty(txtDiagnoseID.Text))
            {
                entityDt.DiagnoseID = txtDiagnoseID.Text;
            }
            entityDt.IsCITO = chkIsCITO_Detail.Checked;
            entityDt.Remarks = txtNotes.Text;

            if (hdnItemTambahanMCUAutoRealisasi.Value == "1") {
                if (!string.IsNullOrEmpty(hdnExecutorParamedicID.Value) && hdnExecutorParamedicID.Value != "0")
                {
                    entityDt.ParamedicID = Convert.ToInt32(hdnExecutorParamedicID.Value);
                }
            }
            
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int testOrderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                SaveTestOrderHd(ctx, ref testOrderID);
                TestOrderDt entityDt = new TestOrderDt();
                ControlToEntity(entityDt);
                hdnTestOrderID.Value = testOrderID.ToString();
                entityDt.TestOrderID = testOrderID;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                entityDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                entityDt.ItemQty = 1;
                entityDt.ItemUnit = hdnGCItemUnit.Value;
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                TestOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entityDt);
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
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

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                TestOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                entityDt.IsDeleted = true;
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
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

        private void EntityToControl(vTestOrderHd entity)
        {
            //if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            //{
            //    //isShowWatermark = true;
            //    //watermarkText = entity.TransactionStatusWatermark;
            //}
            txtStatus.Text = entity.TransactionStatusWatermark;
            IsEditable = (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == null);
            //Registration entityRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
            //IsEditable = entityRegistration.IsLockDown ? false : IsEditable;
            hdnTestOrderID.Value = entity.TestOrderID.ToString();
            txtTestOrderNo.Text = entity.TestOrderNo;
            txtTestOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTestOrderTime.Text = entity.TestOrderTime;
            cboToBePerformed.Value = entity.GCToBePerformed;
            txtScheduledDate.Text = entity.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduledTime.Text = entity.ScheduledTime;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            chkIsCITO.Checked = entity.IsCITO;
            txtNotes.Text = entity.Remarks;
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnTestOrderID.Value != "")
                filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0 AND GCTestOrderStatus != '{1}' ORDER BY ID DESC",
                                                    hdnTestOrderID.Value, Constant.TransactionStatus.VOID);
            List<vTestOrderDt> lstEntity = BusinessLayer.GetvTestOrderDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        #region Utility Function
        private void SetControlEnabled(bool isAdd)
        {
            foreach (DictionaryEntry entry in ControlEntryList)
            {
                Control ctrl = (Control)Helper.FindControlRecursive(this, entry.Key.ToString());
                ControlEntrySetting setting = (ControlEntrySetting)entry.Value;
                bool isEnabled = (isAdd ? setting.IsEditAbleInAddMode : setting.IsEditAbleInEditMode);
                SetControlAttribute(ctrl, isEnabled);
            }
        }

        public void ReInitControl()
        {
            SetControlEnabled(true);
            LoadWords();
            foreach (DictionaryEntry entry in ControlEntryList)
            {
                Control ctrl = (Control)Helper.FindControlRecursive(this, entry.Key.ToString());
                if (ctrl is ASPxEdit || ctrl is WebControl || ctrl is HtmlInputHidden)
                {
                    ControlEntrySetting setting = (ControlEntrySetting)entry.Value;
                    switch (setting.DefaultValue.ToString())
                    {
                        case Constant.DefaultValueEntry.DATE_NOW: SetControlValue(ctrl, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)); break;
                        case Constant.DefaultValueEntry.TIME_NOW: SetControlValue(ctrl, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)); break;
                        default: SetControlValue(ctrl, setting.DefaultValue); break;
                    }
                    if (ctrl is ASPxEdit)
                    {
                        ASPxEdit ctl = ctrl as ASPxEdit;
                        ctl.ValidationSettings.RequiredField.IsRequired = setting.IsRequired;
                        ctl.ValidationSettings.RequiredField.ErrorText = "";
                        ctl.ValidationSettings.CausesValidation = true;
                        ctl.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
                        ctl.ValidationSettings.ErrorFrameStyle.Paddings.Padding = new System.Web.UI.WebControls.Unit(0);

                        //if (setting.IsRequired)
                        ctl.ValidationSettings.ValidationGroup = "mpEntryPopup";
                    }
                    else if (ctrl is WebControl)
                    {
                        if (setting.IsRequired)
                            Helper.AddCssClass(((WebControl)ctrl), "required");
                        ((WebControl)ctrl).Attributes.Add("validationgroup", "mpEntryPopup");
                    }
                }
            }
        }

        protected void SetControlEntrySetting(Control ctrl, ControlEntrySetting setting)
        {
            ControlEntryList.Add(ctrl.ID, setting);
            if (ctrl is ASPxEdit)
            {
                ASPxEdit ctl = ctrl as ASPxEdit;
                ctl.ValidationSettings.RequiredField.IsRequired = setting.IsRequired;
                ctl.ValidationSettings.RequiredField.ErrorText = "";
                ctl.ValidationSettings.CausesValidation = true;
                ctl.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
                ctl.ValidationSettings.ErrorFrameStyle.Paddings.Padding = new System.Web.UI.WebControls.Unit(0);

                //if (setting.IsRequired)
                ctl.ValidationSettings.ValidationGroup = "mpEntryPopup";
            }
            else if (ctrl is WebControl)
            {
                if (setting.IsRequired)
                    Helper.AddCssClass(((WebControl)ctrl), "required");
                ((WebControl)ctrl).Attributes.Add("validationgroup", "mpEntryPopup");
            }
        }

        private void SetControlAttribute(Control ctrl, bool isEnabled)
        {
            if (ctrl is ASPxEdit)
            {
                ((ASPxEdit)ctrl).ClientEnabled = isEnabled;
            }
            else if (ctrl is TextBox)
            {
                if (isEnabled)
                    ((TextBox)ctrl).ReadOnly = false;
                else
                    ((TextBox)ctrl).ReadOnly = true;
            }
            else if (ctrl is DropDownList)
            {
                ((DropDownList)ctrl).Enabled = isEnabled;
            }
            else if (ctrl is CheckBox)
            {
                ((CheckBox)ctrl).Enabled = isEnabled;
            }
            else if (ctrl is HtmlGenericControl)
            {
                HtmlGenericControl lbl = ctrl as HtmlGenericControl;
                if (!isEnabled)
                    lbl.Attributes.Add("class", "lblDisabled");
            }
        }

        private void SetControlValue(Control ctrl, object value)
        {
            if (ctrl is ASPxEdit)
                ((ASPxEdit)ctrl).Value = value;
            else if (ctrl is TextBox)
                ((TextBox)ctrl).Text = value.ToString();
            else if (ctrl is DropDownList)
                Helper.SetDropDownListValue((DropDownList)ctrl, value.ToString());
            else if (ctrl is CheckBox)
            {
                if (value.ToString() == "")
                    ((CheckBox)ctrl).Checked = false;
                else
                    ((CheckBox)ctrl).Checked = Convert.ToBoolean(value);
            }

            else if (ctrl is HtmlInputHidden)
                ((HtmlInputHidden)ctrl).Value = value.ToString();
        }
        #endregion

        #region Session & View State
        public Hashtable ControlEntryList
        {
            get
            {
                if (Session["__PopupControlEntryList"] == null)
                    Session["__PopupControlEntryList"] = new Hashtable();

                return (Hashtable)Session["__PopupControlEntryList"];
            }
            set { Session["__PopupControlEntryList"] = value; }
        }
        #endregion
    }
}