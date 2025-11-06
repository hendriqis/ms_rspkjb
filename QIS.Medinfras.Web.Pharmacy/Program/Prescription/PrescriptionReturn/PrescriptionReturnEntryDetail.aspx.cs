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

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrescriptionReturnEntryDetail : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.PRESCRIPTION_RETURN;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT));
                hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                hdnPrescriptionFeeAmount.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT).ParameterValue;
                string transactionNo = string.Empty;
                string[] param = Page.Request.QueryString["id"].Split('|');

                if (param[0] == "to")
                {
                    hdnVisitID.Value = param[1];
                    hdnDefaultPrescriptionReturnOrderID.Value = param[2];
                    hdnPrescriptionReturnOrderID.Value = param[2];
                    PrescriptionReturnOrderHd entityHd = BusinessLayer.GetPrescriptionReturnOrderHd(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value));
                    hdnDispensaryServiceUnitID.Value = entityHd.HealthcareServiceUnitID.ToString();
                    PatientChargesHd entityPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PrescriptionReturnOrderID = '{0}' AND GCTransactionStatus <> '{1}'", hdnPrescriptionReturnOrderID.Value, Constant.TransactionStatus.VOID)).FirstOrDefault();
                    if (entityPatientChargesHd != null) transactionNo = entityPatientChargesHd.TransactionNo;
                }
                else
                {
                    hdnVisitID.Value = param[0];
                    hdnDispensaryServiceUnitID.Value = param[1];
                    btnClinicTransactionTestOrder.Style.Add("display", "none");
                }

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];

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
                hdnMRN.Value = entity.MRN.ToString();
                hdnGuestID.Value = entity.GuestID.ToString();                
                hdnDefaultParamedicID.Value = entity.ParamedicID.ToString();
                hdnDefaultParamedicCode.Value = entity.ParamedicCode;
                hdnDefaultParamedicName.Value = entity.ParamedicName;
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();

                List<SettingParameter> lstSettingParameterDiagnostic = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID));
                hdnImagingServiceUnitID.Value = lstSettingParameterDiagnostic.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                hdnLaboratoryServiceUnitID.Value = lstSettingParameterDiagnostic.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;

                int locationID = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnDispensaryServiceUnitID.Value)).FirstOrDefault().LocationID;
                hdnLocationID.Value = locationID.ToString();

                if (locationID > 0)
                {
                    Location loc = BusinessLayer.GetLocation(locationID);
                    List<Location> lstLocation = null;
                    if (loc.IsHeader)
                        lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                    else
                    {
                        lstLocation = new List<Location>();
                        lstLocation.Add(loc);
                    }
                    Methods.SetComboBoxField<Location>(cboLocation, lstLocation, "LocationName", "LocationID");
                    cboLocation.SelectedIndex = 0;
                    if (lstLocation.Count == 1)
                        hdnDefaultLocationID.Value = cboLocation.Value.ToString();
                }
                ctlPatientBanner.InitializePatientBanner(entity);
                hdnDepartmentID.Value = entity.DepartmentID;
                hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnClassID.Value = entity.ClassID.ToString();

                hdnPhysicianID.Value = entity.ParamedicID.ToString();
                txtPhysicianCode.Text = entity.ParamedicCode;
                txtPhysicianName.Text = entity.ParamedicName;

                BindGridView();

                if (!string.IsNullOrEmpty(transactionNo))
                {
                    IsLoadFirstRecord = true;
                    string filterExpression = GetFilterExpression();
                    pageIndexFirstLoad = BusinessLayer.GetvPatientChargesHdRowIndex(filterExpression, transactionNo, "TransactionID DESC");
                }
                else
                {
                    if (param[0] != "to") IsLoadFirstRecord = (OnGetRowCount() > 0);
                }

                Helper.SetControlEntrySetting(txtPatientAmount, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtPayerAmount, new ControlEntrySetting(true, true, true), "mpTrx");

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            }

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";
        }

        protected override void SetControlProperties()
        {
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<ClassCare>(cboChargeClass, lstClassCare, "ClassName", "ClassID");

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PRESCRIPTION_RETURN_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboReturnType, lstStandardCode, "StandardCodeName", "StandardCodeID");

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnPrescriptionReturnOrderID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTransactionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPrescriptionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPrescriptionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboLocation, new ControlEntrySetting(true, false, true, hdnDefaultLocationID.Value));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, true, hdnDefaultParamedicID.Value));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, hdnDefaultParamedicCode.Value));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false, hdnDefaultParamedicName.Value));
            SetControlEntrySetting(cboReturnType, new ControlEntrySetting(true, true, true));
        }

        #region Load Entity
        public override void OnAddRecord()
        {
            //hdnTransactionStatus.Value = "";
            hdnIsEditable.Value = "1";
            BindGridView();
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        public String GetFilterExpression()
        {
            //String TransactionStatus = String.Format("'{0}','{1}'", Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            String filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            filterExpression += string.Format(" AND HealthcareServiceUnitID = {0} AND PrescriptionReturnOrderID IS NOT NULL", hdnDispensaryServiceUnitID.Value);
            return filterExpression;
        }

        public String GetTransactionCode()
        {
            return Constant.TransactionCode.PRESCRIPTION_RETURN_ORDER;
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

        private void EntityToControl(vPatientChargesHd entityCharges, ref bool isShowWatermark, ref string watermarkText)
        {
            vPrescriptionReturnOrderHd entity = BusinessLayer.GetvPrescriptionReturnOrderHdList(string.Format("PrescriptionReturnOrderID = {0}", entityCharges.PrescriptionReturnOrderID)).FirstOrDefault();
            if (entity.ChargesGCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entityCharges.TransactionStatusWatermark;
                hdnIsEditable.Value = "0";
            }
            else
            {
                hdnIsEditable.Value = "1";
            }
            hdnTransactionStatus.Value = entityCharges.GCTransactionStatus;
            hdnPrescriptionReturnOrderID.Value = entity.PrescriptionReturnOrderID.ToString();
            hdnTransactionID.Value = entityCharges.TransactionID.ToString();
            txtTransactionNo.Text = entityCharges.TransactionNo;
            txtPrescriptionDate.Text = entityCharges.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entityCharges.TransactionTime;
            cboReturnType.Value = entity.GCPrescriptionReturnType;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            //if (hdnPrescriptionReturnOrderID.Value != "" && hdnPrescriptionReturnOrderID.Value != "0")
            //    filterExpression = string.Format("PrescriptionReturnOrderID = {0} AND ID IS NOT NULL AND IsDeleted = 0 ORDER BY PrescriptionReturnOrderID DESC", hdnPrescriptionReturnOrderID.Value);
            //List<vPrescriptionReturnOrderDt> lstEntity = BusinessLayer.GetvPrescriptionReturnOrderDtList(filterExpression);
            if (hdnTransactionID.Value != "")
            {
                filterExpression = string.Format("TransactionID = {0}", hdnTransactionID.Value);
            }
            List<vPatientChargesDt2> lstEntity = BusinessLayer.GetvPatientChargesDt2List(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
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

        public void SavePrescriptionReturnHd(IDbContext ctx, ref int prescriptionReturnID, ref int transactionID, ref string transactionNo)
        {
            PrescriptionReturnOrderHdDao entityOrderReturnHdDao = new PrescriptionReturnOrderHdDao(ctx);
            PatientChargesHdDao entityPatientChargesHdDao = new PatientChargesHdDao(ctx);

            if (hdnPrescriptionReturnOrderID.Value == "" || hdnPrescriptionReturnOrderID.Value == "0" || hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
            {
                #region PrescriptionOrderHd
                PrescriptionReturnOrderHd entityReturnOrderHd = new PrescriptionReturnOrderHd();
                entityReturnOrderHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_RETURN_ORDER;
                entityReturnOrderHd.HealthcareServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                entityReturnOrderHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityReturnOrderHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                entityReturnOrderHd.OrderDate = Helper.GetDatePickerValue(Request.Form[txtPrescriptionDate.UniqueID]);
                entityReturnOrderHd.OrderTime = Request.Form[txtPrescriptionTime.UniqueID];
                entityReturnOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                entityReturnOrderHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                entityReturnOrderHd.IsCreatedBySystem = true;
                entityReturnOrderHd.GCPrescriptionReturnType = cboReturnType.Value.ToString();
                entityReturnOrderHd.PrescriptionReturnOrderNo = BusinessLayer.GenerateTransactionNo(entityReturnOrderHd.TransactionCode, entityReturnOrderHd.OrderDate, ctx);
                entityReturnOrderHd.LastUpdatedBy = entityReturnOrderHd.CreatedBy = AppSession.UserLogin.UserID;
                entityReturnOrderHd.LastUpdatedDate = entityReturnOrderHd.CreatedDate = DateTime.Now;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                prescriptionReturnID = entityOrderReturnHdDao.InsertReturnPrimaryKeyID(entityReturnOrderHd);
                #endregion

                #region PatientChargesHd
                PatientChargesHd entityPatientChargesHd = new PatientChargesHd();
                entityPatientChargesHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityPatientChargesHd.TestOrderID = null;
                entityPatientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PH_CHARGES;
                entityPatientChargesHd.TransactionDate = entityReturnOrderHd.OrderDate;
                entityPatientChargesHd.TransactionTime = entityReturnOrderHd.OrderTime;
                entityPatientChargesHd.PatientBillingID = null;
                entityPatientChargesHd.ReferenceNo = "";
                entityPatientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityPatientChargesHd.GCVoidReason = null;
                entityPatientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityPatientChargesHd.TransactionCode, entityPatientChargesHd.TransactionDate, ctx);
                entityPatientChargesHd.PrescriptionReturnOrderID = prescriptionReturnID;
                entityPatientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                transactionID = entityPatientChargesHdDao.InsertReturnPrimaryKeyID(entityPatientChargesHd);
                transactionNo = entityPatientChargesHd.TransactionNo;
                #endregion
            }
            else
            {
                prescriptionReturnID = Convert.ToInt32(hdnPrescriptionReturnOrderID.Value);
                transactionID = Convert.ToInt32(hdnTransactionID.Value);
                transactionNo = txtTransactionNo.Text;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int PrescriptionReturnID = 0;
                int TransactionReturnID = 0;
                string TransactionNo = "";
                SavePrescriptionReturnHd(ctx, ref PrescriptionReturnID, ref TransactionReturnID, ref TransactionNo);
                retval = PrescriptionReturnID.ToString();
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
                PatientChargesHd entityCharges = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionID.Value));
                if (entityCharges.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    PrescriptionReturnOrderHd entity = BusinessLayer.GetPrescriptionReturnOrderHd(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value));
                    entity.GCPrescriptionReturnType = cboReturnType.Value.ToString();
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePrescriptionReturnOrderHd(entity);
                    return true;
                }
                else
                {
                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", entityCharges.TransactionNo);
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

        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ConsultVisitDao visitDao = new ConsultVisitDao(ctx);
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
                        //int oRegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                        int oFromVisit = Convert.ToInt32(orderHd.FromVisitID);
                        int oVisitID = Convert.ToInt32(orderHd.VisitID);
                        ConsultVisit oVisit = visitDao.Get(oVisitID);
                        int oRegistrationID = Convert.ToInt32(oVisit.RegistrationID);

                        if (oFromVisit == 0 || oFromVisit == null)
                        {
                            oVisit = visitDao.Get(oVisitID);
                            oRegistrationID = Convert.ToInt32(oVisit.RegistrationID);
                        }
                        else
                        {
                            oVisit = visitDao.Get(oFromVisit);
                            oRegistrationID = Convert.ToInt32(oVisit.RegistrationID);
                        }
                      
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
        #endregion

        //#region Void Entity
        //protected override bool OnVoidRecord(ref string errMessage)
        //{
        //    bool result = true;
        //    IDbContext ctx = DbFactory.Configure(true);
        //    PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
        //    PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
        //    try
        //    {
        //        PatientChargesHd entity = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
        //        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
        //        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
        //        entityHdDao.Update(entity);

        //        List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value), ctx);
        //        foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
        //        {
        //            patientChargesDt.IsApproved = false;
        //            patientChargesDt.IsDeleted = true;
        //            patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
        //            entityDtDao.Update(patientChargesDt);
        //        }
        //        ctx.CommitTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        Helper.InsertErrorLog(ex);
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
                PrescriptionReturnOrderHdDao presreturnHdDao = new PrescriptionReturnOrderHdDao(ctx);
                PrescriptionReturnOrderDtDao presreturnDtDao = new PrescriptionReturnOrderDtDao(ctx);
                PatientChargesHdDao chargesDao = new PatientChargesHdDao(ctx);
                PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
                ChargesStatusLogDao statusLogDao = new ChargesStatusLogDao(ctx);
                try
                {
                    ChargesStatusLog statusLog = new ChargesStatusLog();

                    string statusOld = "", statusNew = "";

                    Int32 TransactionID = Convert.ToInt32(hdnTransactionID.Value);
                    PatientChargesHd entity = chargesDao.Get(TransactionID);
                    statusOld = entity.GCTransactionStatus;
                    if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PrescriptionReturnOrderHd orderHd = presreturnHdDao.Get(Convert.ToInt32(entity.PrescriptionReturnOrderID));
                        List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') <> '{1}'", hdnTransactionID.Value, Constant.TransactionStatus.VOID), ctx);
                        foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                        {
                            patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            patientChargesDt.IsApproved = false;
                            patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            chargesDtDao.Update(patientChargesDt);

                            PrescriptionReturnOrderDt orderDt = presreturnDtDao.Get((int)patientChargesDt.PrescriptionReturnOrderDtID);
                            if (orderDt != null)
                            {
                                if (!orderDt.IsDeleted)
                                {
                                    if (orderHd.IsCreatedBySystem)
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
                                        orderDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.RECEIVED;
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
                            if (orderHd.IsCreatedBySystem)
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
                                orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                orderHd.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                presreturnHdDao.Update(orderHd);
                            }

                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi " + entity.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah.";
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            try
            {
                PatientChargesHd entityHd = entityChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDtList(String.Format("TransactionID = {0} AND ItemID = {1} AND IsDeleted = 0", hdnTransactionID.Value, hdnItemID.Value))[0];
                    if (!entityChargesDt.IsDeleted)
                    {
                        entityChargesDt.DiscountAmount = Convert.ToDecimal(hdnDiscountAmount.Value);
                        entityChargesDt.DiscountComp1 = entityChargesDt.DiscountAmount / entityChargesDt.ChargedQuantity * -1;
                        entityChargesDt.PatientAmount = Convert.ToDecimal(txtPatientAmount.Text);
                        entityChargesDt.PayerAmount = Convert.ToDecimal(txtPayerAmount.Text);
                        entityChargesDt.LineAmount = Convert.ToDecimal(Request.Form[txtLineAmount.UniqueID]);
                        entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityChargesDtDao.Update(entityChargesDt);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", entityHd.TransactionNo);
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

        private bool OnDeleteEntityDt(ref string errMessage, string param)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionReturnOrderDtDao entityDtDao = new PrescriptionReturnOrderDtDao(ctx);
            PatientChargesHdDao entityChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            string[] paramDelete = param.Split(';');
            int ID = Convert.ToInt32(paramDelete[0]);
            string gcDeleteReason = paramDelete[1];
            string reason = paramDelete[2];
            try
            {
                PatientChargesHd entityHd = entityChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    String filterExpression = String.Format("ID = {0} AND ItemID = {1} AND IsDeleted = 0", hdnChargesDetailIDNew.Value, hdnItemID.Value);
                    PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDtList(filterExpression, ctx)[0];
                    entityChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                    entityChargesDt.GCDeleteReason = gcDeleteReason;
                    entityChargesDt.DeleteReason = reason;
                    entityChargesDt.DeleteDate = DateTime.Now;
                    entityChargesDt.DeleteBy = AppSession.UserLogin.UserID;
                    entityChargesDt.IsDeleted = true;
                    entityChargesDt.IsApproved = false;
                    entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityChargesDtDao.Update(entityChargesDt);

                    PrescriptionReturnOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    if (!entityDt.IsCreatedFromOrder)
                    {
                        if (AppSession.IsAutoVoidPrescriptionOrderCreatedBySystem == "1")
                        {
                            entityDt.IsDeleted = true;
                            entityDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.CANCELLED;
                            entityDt.GCDeleteReason = Constant.DeleteReason.OTHER;
                            entityDt.DeleteReason = "Linked transaction was deleted";
                        }
                        else
                        {
                            entityDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.RECEIVED;
                        }
                    }
                    else
                    {
                        entityDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.RECEIVED;
                    }
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat dihapus lagi.", entityHd.TransactionNo);
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}