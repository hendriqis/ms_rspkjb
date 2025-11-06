using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class BloodBankOrderEntryCtl1 : BaseEntryPopupCtl3
    {
        protected int gridProcedureGroupPageCount = 1;
        protected int gridParamedicTeamPageCount = 1;
        protected int gridPageCount = 1;

        protected static string _orderID = "0";

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnTestOrderIDCtlEntry.Value = paramInfo[0];
            hdnParameterRegistrationID.Value = paramInfo[1];
            hdnParameterVisitID.Value = paramInfo[2];
            hdnParameterParamedicID.Value = paramInfo[3];
            hdnHealthcareServiceUnitID.Value = paramInfo[4];

            string filterSetVarDt = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                            AppSession.UserLogin.HealthcareID, //0
                                                            Constant.SettingParameter.MD0018, //1
                                                            Constant.SettingParameter.MD_IS_BLOODBANK_ORDER_REMARKS_COPY_FROM_DIAGNOSE //2
                                                        );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVarDt);

            hdnBloodBankHSUID.Value = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD0018).ParameterValue;
            hdnIsRemarksCopyFromDiagnose.Value = lstSetVarDt.FirstOrDefault(a => a.ParameterCode == Constant.SettingParameter.MD_IS_BLOODBANK_ORDER_REMARKS_COPY_FROM_DIAGNOSE).ParameterValue;

            if (hdnIsRemarksCopyFromDiagnose.Value == "1")
            {
                string filterDiag = string.Format("VisitID = {0} AND ParamedicID = {1} AND IsDeleted = 0 ORDER BY GCDiagnoseType", hdnParameterVisitID.Value, hdnParameterParamedicID.Value);
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
                txtRemarks.Text = hdnDefaultDiagnosa.Value;
            }
            else
            {
                hdnDefaultDiagnosa.Value = "";
                txtRemarks.Text = "";
            }

            if (hdnTestOrderIDCtlEntry.Value != "" && hdnTestOrderIDCtlEntry.Value != "0")
            {
                IsAdd = false;
                _orderID = hdnTestOrderIDCtlEntry.Value;
                OnControlEntrySettingLocal();
                ReInitControl();
                string filterExpression = string.Format("TestOrderID = {0}", hdnTestOrderIDCtlEntry.Value);
                vBloodBankOrder1 entity = BusinessLayer.GetvBloodBankOrder1List(filterExpression).FirstOrDefault();
                EntityToControl(entity);
            }
            else
            {
                OnControlEntrySettingLocal();
                
                ReInitControl();
                hdnTestOrderIDCtlEntry.Value = "0";
                _orderID = hdnTestOrderIDCtlEntry.Value;
                IsAdd = true;
            }

            BindGridViewDetail(1, true, ref gridPageCount);
        }

        private void SetControlProperties()
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicTeam> lstParamedic = BusinessLayer.GetvParamedicTeamList(string.Format("RegistrationID = {0} AND IsDeleted = 0", hdnParameterRegistrationID.Value));
            if (lstParamedic.Count() > 0)
            {
                Methods.SetComboBoxField<vParamedicTeam>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
                cboParamedicID.Value = hdnParameterParamedicID.Value;
            }
            else
            {
                List<ParamedicMaster> lstPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", AppSession.RegisteredPatient.ParamedicID));
                Methods.SetComboBoxField<ParamedicMaster>(cboParamedicID, lstPM, "FullName", "ParamedicID");
                cboParamedicID.Value = hdnParameterParamedicID.Value;
            }

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND StandardCodeID NOT IN ('{1}') AND IsActive = 1 AND IsDeleted = 0",
                                                                                Constant.StandardCode.BLOOD_TYPE, Constant.BloodType.BloodType_NA));

            int MRN = AppSession.RegisteredPatient.MRN;
            string oGCBloodType = "", oBloodRhesus = "";
            if (MRN != null && MRN != 0)
            {
                Patient entityPatient = BusinessLayer.GetPatient(MRN);
                oGCBloodType = entityPatient.GCBloodType;
                oBloodRhesus = entityPatient.BloodRhesus;
            }

            List<StandardCode> lstBloodType = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BLOOD_TYPE).ToList();
            if (!string.IsNullOrEmpty(oGCBloodType))
            {
                if (oGCBloodType != Constant.BloodType.BloodType_NA)
                {
                    lstBloodType = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BLOOD_TYPE && (lst.StandardCodeID == oGCBloodType || lst.StandardCodeID == Constant.BloodType.BloodType_O)).ToList();
                }
            }
            Methods.SetComboBoxField<StandardCode>(cboBloodType, lstBloodType, "StandardCodeName", "StandardCodeID");
            cboBloodType.Value = oGCBloodType;

            List<Variable> lstBloodRhesus = new List<Variable>();
            lstBloodRhesus.Add(new Variable { Code = "+", Value = "+ (Pos)" });
            lstBloodRhesus.Add(new Variable { Code = "-", Value = "- (Neg)" });
            Methods.SetComboBoxField<Variable>(cboRhesus, lstBloodRhesus, "Value", "Code");
            cboRhesus.Value = oBloodRhesus;

            List<vItemService> lstItemService = BusinessLayer.GetvItemServiceList(string.Format("IsBloodBankItem = 1 AND IsDeleted = 0"));
            Methods.SetComboBoxField<vItemService>(cboBloodComponentType, lstItemService, "ItemName1", "ItemID");

        }

        private void OnControlEntrySettingLocal()
        {
            SetControlEntrySetting(txtOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            SetControlProperties();
        }

        private void EntityToControl(vBloodBankOrder1 entity)
        {
            txtOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtOrderTime.Text = entity.TestOrderTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            cboBloodType.Value = entity.GCBloodType;
            cboRhesus.Value = entity.BloodRhesus;
            rblGCSourceType.SelectedValue = entity.GCSourceType;
            rblGCUsageType.SelectedValue = entity.GCUsageType;
            rblGCPaymentType.SelectedValue = entity.GCPaymentType;

            if (rblGCSourceType.SelectedValue == Constant.BloodBankSourceType.PMI)
            {
                trPaymentTypeInfo.Attributes.Remove("style");
            }
            else
            {
                trPaymentTypeInfo.Attributes.Add("style", "display:none");
            }

            chkIsCITO.Checked = entity.IsCITO;
            txtRemarks.Text = entity.Remarks;
            txtMedicalHistory.Text = entity.TransfusionHistory;
        }

        private void ControlToEntity(TestOrderHd entity)
        {
            entity.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
            if (!string.IsNullOrEmpty(AppSession.HealthcareServiceUnitID))
                entity.FromHealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
            else
                entity.FromHealthcareServiceUnitID = Convert.ToInt32(AppSession.RegisteredPatient.HealthcareServiceUnitID);

            entity.HealthcareServiceUnitID = Convert.ToInt32(hdnBloodBankHSUID.Value);
            entity.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.TestOrderDate = Helper.GetDatePickerValue(txtOrderDate);
            entity.TestOrderTime = txtOrderTime.Text;
            entity.ScheduledDate = Helper.GetDatePickerValue(txtOrderDate);
            entity.ScheduledTime = txtOrderTime.Text;
            entity.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;

            entity.GCBloodType = cboBloodType.Value.ToString();
            entity.BloodRhesus = cboRhesus.Value.ToString();
            entity.GCSourceType = rblGCSourceType.SelectedValue;
            entity.GCUsageType = rblGCUsageType.SelectedValue;
            if (!string.IsNullOrEmpty(rblGCPaymentType.SelectedValue))
            {
                entity.GCPaymentType = rblGCPaymentType.SelectedValue;
            }

            entity.IsCITO = chkIsCITO.Checked;
            entity.Remarks = txtRemarks.Text;
            entity.TransfusionHistory = txtMedicalHistory.Text;
            entity.IsBloodBankOrder = true;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            if (!IsValidated(ref errMessage))
            {
                result = false;
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                int TestorderID = Convert.ToInt32(_orderID);
                if (TestorderID == 0)
                {
                    //TestOrderHd entity1 = new TestOrderHd();
                    //ControlToEntity(entity1);
                    //entity1.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    //entity1.GCOrderStatus = Constant.OrderStatus.OPEN;
                    //entity1.TestOrderNo = BusinessLayer.GenerateTransactionNo(entity1.TransactionCode, entity1.TestOrderDate, ctx);
                    //entity1.CreatedBy = AppSession.UserLogin.UserID;

                    //ctx.CommandType = CommandType.Text;
                    //ctx.Command.Parameters.Clear();
                    //int _testOrderID = entityHdDao.InsertReturnPrimaryKeyID(entity1);

                    //_orderID = _testOrderID.ToString();

                    //ctx.CommitTransaction();

                    result = false;
                    errMessage = string.Format("Harap Isi Jenis Produk Darah Terlebih Dahulu");
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                else
                { //jika jenis darah dahulu yg diinput

                    TestOrderHd entity1 = BusinessLayer.GetTestOrderHd(TestorderID);
                    ControlToEntity(entity1);
                    entity1.CreatedBy = AppSession.UserLogin.UserID;
                    entity1.CreatedDate = DateTime.Now;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHdDao.Update(entity1);
                    ctx.CommitTransaction();
                }
                retVal = _orderID;
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            if (!IsValidated(ref errMessage))
            {
                result = false;
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
            BloodBankOrderDao entityDao = new BloodBankOrderDao(ctx);

            try
            {
                TestOrderHd entityUpdate = orderHdDao.Get(Convert.ToInt32(_orderID));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                orderHdDao.Update(entityUpdate);
                _orderID = entityUpdate.TestOrderID.ToString();

                retVal = entityUpdate.TestOrderID.ToString();
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

        private bool IsValidated(ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            if (string.IsNullOrEmpty(txtOrderTime.Text))
            {
                message.AppendLine("Jam order harus diisi|");
            }
            else
            {
                if (!Methods.ValidateTimeFormat(txtOrderTime.Text))
                    message.AppendLine("Format Jam Order tidak sesuai format (HH:MM)|");
                else
                {
                    DateTime startDateTime = DateTime.ParseExact(string.Format("{0} {1}", txtOrderDate.Text, txtOrderTime.Text), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);

                    if (startDateTime.Date > DateTime.Now.Date)
                    {
                        message.AppendLine("Tanggal Order harus lebih kecil atau sama dengan tanggal hari ini.|");
                    }
                }
            }

            if (string.IsNullOrEmpty(rblGCSourceType.SelectedValue))
            {
                message.AppendLine("Sumber/asal darah harus dipilih|");
            }

            if (string.IsNullOrEmpty(rblGCUsageType.SelectedValue))
            {
                message.AppendLine("Cara penyimpanan harus dipilih|");
            }

            if (string.IsNullOrEmpty(txtRemarks.Text))
            {
                message.AppendLine("Catatan Klinis / Diagnosa harus diisi|");
            }

            if (cboBloodType.Value == null)
            {
                message.AppendLine("Jenis golongan darah harus diisi|");
            }
            else if (string.IsNullOrEmpty(cboBloodType.Value.ToString()))
            {
                message.AppendLine("Jenis golongan darah harus diisi|");
            }

            if (cboRhesus.Value == null)
            {
                message.AppendLine("Rhesus golongan darah harus diisi|");
            }
            else if (string.IsNullOrEmpty(cboRhesus.Value.ToString()))
            {
                message.AppendLine("Rhesus golongan darah harus diisi|");
            }

            errMessage = message.ToString().Replace(@"|", "<br />");

            return string.IsNullOrEmpty(errMessage);
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        #region Detail
        private bool IsValidateEntry(ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            if (cboBloodComponentType.Value != null)
            {
                if (string.IsNullOrEmpty(cboBloodComponentType.Value.ToString()))
                {
                    message.AppendLine("Jenis Darah harus diisi|");
                }
            }
            else
            {
                message.AppendLine("Jenis Darah harus diisi|");
            }

            if (string.IsNullOrEmpty(txtOrderQty.Text))
            {
                message.AppendLine("Jumlah permintaan harus diisi|");
            }
            else
            {
                if (!Methods.IsNumeric(txtOrderQty.Text))
                {
                    message.AppendLine("Jumlah permintaan harus dalam angka|");
                }
                else
                {
                    if (Convert.ToDecimal(txtOrderQty.Text) <= 0)
                    {
                        message.AppendLine("Jumlah permintaan harus lebih besar dari 0|");
                    }
                }
            }

            errMessage = message.ToString().Replace("|", "<br />");

            return string.IsNullOrEmpty(errMessage);
        }
        private void BindGridViewDetail(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (Page.IsCallback && _orderID != "0")
            {
                hdnTestOrderIDCtlEntry.Value = _orderID;
            }

            List<vTestOrderDt> lstEntity = new List<vTestOrderDt>();
            if (hdnTestOrderIDCtlEntry.Value != "0")
            {
                string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", hdnParameterVisitID.Value, hdnTestOrderIDCtlEntry.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvTestOrderSpecimenRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                lstEntity = BusinessLayer.GetvTestOrderDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID");
            }

            grdDetailView.DataSource = lstEntity;
            grdDetailView.DataBind();
        }
        protected void cbpDetailView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDetail(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDetail(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = string.Empty;
        }
        protected void cbpDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtDao detailDao = new TestOrderDtDao(ctx);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    int orderID = Convert.ToInt32(hdnTestOrderIDCtlEntry.Value);

                    if (param[0] == "add" || param[0] == "edit")
                    {
                        string errMessage = string.Empty;
                        if (!IsValidateEntry(ref errMessage))
                        {
                            result = string.Format("0|process|{0}", errMessage);
                            ctx.RollBackTransaction();
                        }
                        else
                        {
                            if (param[0] == "add")
                            {
                                if (hdnTestOrderIDCtlEntry.Value == "0")
                                {
                                    TestOrderHd entityHd = new TestOrderHd();
                                    ControlToEntity(entityHd);
                                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                    entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
                                    entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                                    orderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                                    _orderID = orderID.ToString();
                                    hdnTestOrderIDCtlEntry.Value = orderID.ToString();
                                }
                                else
                                {
                                    hdnTestOrderIDCtlEntry.Value = _orderID;
                                    orderID = Convert.ToInt32(hdnTestOrderIDCtlEntry.Value);
                                }

                                TestOrderDt obj = new TestOrderDt();

                                obj.TestOrderID = orderID;
                                obj.ItemID = Convert.ToInt32(cboBloodComponentType.Value.ToString());
                                obj.ItemQty = Convert.ToDecimal(txtOrderQty.Text);
                                obj.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                                obj.CreatedBy = AppSession.UserLogin.UserID;
                                detailDao.Insert(obj);

                                result = "1|add|" + _orderID;
                            }
                            else
                            {
                                int recordID = Convert.ToInt32(hdnDetailRecordID.Value);
                                TestOrderDt entity = detailDao.Get(recordID);
                                if (entity != null)
                                {
                                    entity.ItemID = Convert.ToInt32(cboBloodComponentType.Value.ToString());
                                    entity.ItemQty = Convert.ToDecimal(txtOrderQty.Text);
                                    entity.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    detailDao.Update(entity);
                                    result = "1|edit|" + _orderID;
                                }
                                else
                                {
                                    result = string.Format("0|delete|{0}", "Jenis Darah tidak valid");
                                }
                            }
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnDetailRecordID.Value);
                        TestOrderDt entity = detailDao.Get(recordID);
                        if (entity != null)
                        {
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            detailDao.Update(entity);
                            result = "1|delete|" + _orderID;
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Jenis Darah tidak valid");
                        }
                        result = "1|delete|" + _orderID;
                    }
                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}