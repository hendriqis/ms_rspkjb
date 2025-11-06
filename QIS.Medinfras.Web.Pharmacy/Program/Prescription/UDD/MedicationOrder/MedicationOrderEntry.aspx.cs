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
using System.Text;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class MedicationOrderEntry : BasePageTrx
    {
        protected bool isShowSwitchIcon = false;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.UDD_PRESCRIPTION_ENTRY;
        }

        public String IsEditable()
        {
            return hdnIsEditable.Value;
        }

        protected string OnGetUrlReferrer()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param[0] == "ptp")
                return ResolveUrl("~/Program/PatientList/VisitList.aspx?id=ptp");
            return ResolveUrl("~/Program/Prescription/PrescriptionEntry/PrescriptionEntryList.aspx");
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnPageTitle.Value = "Inpatient - Medication Order";

                List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FM_USING_UDD_FOR_INPATIENT));
                hdnIsUsingUDD.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FM_USING_UDD_FOR_INPATIENT).ParameterValue;
                string[] param = Page.Request.QueryString["id"].Split('|');

                String filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PRESCRIPTION_TYPE);
                List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
                Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode, "StandardCodeName", "StandardCodeID");
                cboPrescriptionType.SelectedIndex = 0;
                string transactionNo = string.Empty;
                if (param[0] == "to")
                {
                    hdnVisitID.Value = param[1];
                    hdnDefaultPrescriptionOrderID.Value = param[2];
                    PrescriptionOrderHd entityHd = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnDefaultPrescriptionOrderID.Value));
                    hdnDispensaryServiceUnitID.Value = entityHd.DispensaryServiceUnitID.ToString();
                    if (entityHd.GCOrderStatus == Constant.TestOrderStatus.COMPLETED)
                        btnOrderInfo.Style.Add("display", "none");
                    cboPrescriptionType.Value = entityHd.GCPrescriptionType;
                    transactionNo = entityHd.PrescriptionOrderNo;
                }
                else
                {
                    hdnVisitID.Value = param[1];
                    hdnDispensaryServiceUnitID.Value = param[2];
                    btnOrderInfo.Style.Add("display", "none");
                }

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
                hdnChargeClassID.Value = entity.ChargeClassID.ToString();
                hdnDepartmentID.Value = entity.DepartmentID;
                hdnDefaultParamedicID.Value = entity.ParamedicID.ToString();
                hdnDefaultParamedicCode.Value = entity.ParamedicCode;
                hdnDefaultParamedicName.Value = entity.ParamedicName;
                hdnMRN.Value = entity.MRN.ToString();
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
                hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();

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
                        hdnDefaultLocationID.Value = hdnLocationID.Value;
                }

                ctlPatientBanner.InitializePatientBanner(entity);
                hdnDepartmentID.Value = entity.DepartmentID;
                hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnClassID.Value = entity.ChargeClassID.ToString();

                hdnPhysicianID.Value = entity.ParamedicID.ToString();
                txtPhysicianCode.Text = entity.ParamedicCode;
                txtPhysicianName.Text = entity.ParamedicName;

                if (!string.IsNullOrEmpty(transactionNo))
                {
                    IsLoadFirstRecord = true;
                    filterExpression = OnGetFilterExpression();
                    pageIndexFirstLoad = BusinessLayer.GetvPrescriptionOrderHd1RowIndex (filterExpression, transactionNo, "PrescriptionOrderID DESC");
                }
                else
                {
                    if (param[0] != "to") IsLoadFirstRecord = (OnGetRowCount() > 0);
                }

                BindGridView();

                Helper.SetControlEntrySetting(txtGenericName, new ControlEntrySetting(false, false, false), "mpTrx");
                Helper.SetControlEntrySetting(txtDrugCode, new ControlEntrySetting(true, false, true), "mpTrx");
                Helper.SetControlEntrySetting(cboForm, new ControlEntrySetting(true, true, true), "mpTrx");

                Helper.SetControlEntrySetting(cboLocation, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtStrengthAmount, new ControlEntrySetting(false, false, false), "mpTrx");
                Helper.SetControlEntrySetting(txtStrengthUnit, new ControlEntrySetting(false, false, false), "mpTrx");
                Helper.SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(cboFrequencyTimeline, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(cboDosingUnit, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtExpiredDate, new ControlEntrySetting(true, true, false), "mpTrx");
                Helper.SetControlEntrySetting(txtPurposeOfMedication, new ControlEntrySetting(true, true, false), "mpTrx");
                Helper.SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false), "mpTrx");
                Helper.SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true), "mpTrx");
            }

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

        }

        //protected bool IsEditable = true;
        public override void OnAddRecord()
        {
            hdnIsEditable.Value = "1";
            hdnTransactionStatus.Value = "";
            cboLocation.SelectedIndex = 0;
            txtPhysicianCode.Text = "";
            txtPhysicianName.Text = "";
            cboPrescriptionType.SelectedIndex = 0;
            txtNotes.Text = "";
            BindGridView();
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
            List<StandardCode> lstToBePerformed = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.TO_BE_PERFORMED));
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.DRUG_FORM, Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.COENAM_RULE);

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            List<StandardCode> lstMedicationRoute = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList();

            Methods.SetComboBoxField<StandardCode>(cboForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstMedicationRoute, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<ClassCare>(cboChargeClass, lstClassCare, "ClassName", "ClassID");

            StandardCode defaultMedicationRoute = lstMedicationRoute.FirstOrDefault(p => p.IsDefault);
            if (defaultMedicationRoute == null)
                defaultMedicationRoute = lstMedicationRoute[0];
            hdnDefaultGCMedicationRoute.Value = defaultMedicationRoute.StandardCodeID;

            cboForm.SelectedIndex = 1;
            cboFrequencyTimeline.SelectedIndex = 1;
            txtDosingDurationTimeline.Text = cboFrequencyTimeline.Text;
            cboMedicationRoute.SelectedIndex = 1;
            cboChargeClass.SelectedIndex = 1;

            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString("HH:mm");
        }

        protected void cboDosingUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (hdnDrugID.Value != null && hdnDrugID.Value.ToString() != "")
            {
                List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN ( (SELECT GCDoseUnit FROM vDrugInfo WHERE ItemID = {1}),(SELECT GCItemUnit FROM vDrugInfo WHERE ItemID = {2})))", Constant.StandardCode.ITEM_UNIT, hdnDrugID.Value, hdnDrugID.Value));
                Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lst, "StandardCodeName", "StandardCodeID");
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnPrescriptionOrderID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTransactionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPrescriptionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPrescriptionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboPrescriptionType, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtPrescriptionOrderInfo, new ControlEntrySetting(false, false, false, string.Empty));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, true, hdnDefaultParamedicID.Value));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, hdnDefaultParamedicCode.Value));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false, hdnDefaultParamedicName.Value));
        }

        #region Load Entity
        protected string OnGetFilterExpression()
        {
            String filterExpression = "";
            filterExpression = string.Format("RegistrationID = {0} AND DispensaryServiceUnitID = {1} AND PrescriptionOrderID IS NOT NULL AND GCTransactionStatus != '{2}'", hdnRegistrationID.Value, hdnDispensaryServiceUnitID.Value,Constant.TransactionStatus.VOID);
            return filterExpression;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = OnGetFilterExpression();
            return BusinessLayer.GetvPrescriptionOrderHd1RowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = OnGetFilterExpression();
            vPrescriptionOrderHd1 entity = BusinessLayer.GetvPrescriptionOrderHd1(filterExpression, PageIndex, " PrescriptionOrderID DESC");
            if (entity != null)
            {
                EntityToControl(entity, ref isShowWatermark, ref watermarkText); 
            }
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = OnGetFilterExpression();
            PageIndex = BusinessLayer.GetvPrescriptionOrderHd1RowIndex(filterExpression, keyValue, "PrescriptionOrderID DESC");
            vPrescriptionOrderHd1 entity = BusinessLayer.GetvPrescriptionOrderHd1(filterExpression, PageIndex, "PrescriptionOrderID DESC");

            if (entity != null)
            {
                EntityToControl(entity, ref isShowWatermark, ref watermarkText); 
            }
        }

        private void EntityToControl(vPrescriptionOrderHd1 entityOrder, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entityOrder.GCOrderStatus != Constant.OrderStatus.RECEIVED)
            {
                isShowWatermark = true;
                watermarkText = entityOrder.OrderStatusWatermark;
                hdnIsEditable.Value = "0";
            }
            else
                hdnIsEditable.Value = "1";

            hdnPrescriptionOrderID.Value = entityOrder.PrescriptionOrderID.ToString();
            hdnTransactionID.Value = entityOrder.PrescriptionOrderID.ToString();
            txtTransactionNo.Text = entityOrder.PrescriptionOrderNo;
            txtPrescriptionDate.Text = entityOrder.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entityOrder.PrescriptionTime;
            cboPrescriptionType.Value = entityOrder.GCPrescriptionType;
            hdnPhysicianID.Value = entityOrder.ParamedicID.ToString();
            txtPhysicianCode.Text = entityOrder.ParamedicCode;
            txtPhysicianName.Text = entityOrder.ParamedicName;
            txtPrescriptionOrderInfo.Text = string.Format("{0}|{1}|{2}", entityOrder.PrescriptionOrderNo, entityOrder.LastUpdatedDateInString, entityOrder.CreatedByName);
            txtNotes.Text = entityOrder.Remarks;
            cboLocation.Value = entityOrder.LocationID;
            hdnLocationID.Value = entityOrder.LocationID.ToString();
            hdnTransactionStatus.Value = entityOrder.GCOrderStatus;
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnPrescriptionOrderID.Value != "" && hdnPrescriptionOrderID.Value != "0" && hdnTransactionID.Value != "0")
                filterExpression = string.Format("PrescriptionOrderID = {0} AND ParentID IS NULL AND OrderIsDeleted = 0 ORDER BY PrescriptionOrderDetailID", hdnPrescriptionOrderID.Value, Constant.OrderStatus.OPEN, Constant.OrderStatus.CANCELLED);
            List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
            if (oVisit != null)
                isShowSwitchIcon = oVisit.GCCustomerType != Constant.CustomerType.PERSONAL;
            else
                isShowSwitchIcon = false;
        }
        #endregion

        #region Save Entity
        private void ControlToEntity(PrescriptionOrderDt entity)
        {
            #region PrescriptionOrderDt
            entity.IsRFlag = true;
            entity.IsUsingUDD = chkIsAsRequired.Checked ? false : chkIsUsingUDD.Checked;  
            if (hdnDrugID.Value.ToString() != "")
                entity.ItemID = Convert.ToInt32(hdnDrugID.Value);
            else
                entity.ItemID = null;
            entity.DrugName = Request.Form[txtDrugName.UniqueID];
            entity.GenericName = Request.Form[txtGenericName.UniqueID];
            entity.GCDrugForm = cboForm.Value.ToString();
            if (hdnSignaID.Value != "")
                entity.SignaID = Convert.ToInt32(hdnSignaID.Value);
            else
                entity.SignaID = null;
            if (hdnGCDoseUnit.Value != null && hdnGCDoseUnit.Value.ToString() != "")
            {
                entity.Dose = Convert.ToDecimal(Request.Form[txtStrengthAmount.UniqueID]);
                entity.GCDoseUnit = hdnGCDoseUnit.Value.ToString();
            }
            else
            {
                entity.Dose = null;
                entity.GCDoseUnit = null;
            }
            entity.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            entity.GCDosingUnit = cboDosingUnit.Value.ToString();
            entity.GCRoute = cboMedicationRoute.Value.ToString();
            entity.StartDate = Helper.GetDatePickerValue(Request.Form[txtStartDate.UniqueID]);
            entity.StartTime = Request.Form[txtStartTime.UniqueID];
            entity.IsMorning = chkIsMorning.Checked;
            entity.IsNoon = chkIsNoon.Checked;
            entity.IsEvening = chkIsEvening.Checked;
            entity.IsNight = chkIsNight.Checked;
            entity.IsAsRequired = chkIsAsRequired.Checked;
            entity.MedicationPurpose = txtPurposeOfMedication.Text;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            entity.DosingDuration = Convert.ToDecimal(txtDosingDuration.Text);

            if (!entity.IsUsingUDD)
            {
                entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
                entity.TakenQty = entity.DispenseQty;
                entity.ChargeQty = entity.TakenQty;
                entity.ResultQty = entity.ResultQty;
            }
            else
            {
                entity.DispenseQty = 0;
                entity.TakenQty = 0;
                entity.ChargeQty = 0;
                entity.ResultQty = 0;
            }

            if (!string.IsNullOrEmpty(txtExpiredDate.Text))
                entity.ExpiredDate = Helper.GetDatePickerValue(Request.Form[txtExpiredDate.UniqueID]);
            if (hdnEmbalaceID.Value != "" && hdnEmbalaceID.Value != "0")
                entity.EmbalaceID = Convert.ToInt32(hdnEmbalaceID.Value);
            else
                entity.EmbalaceID = null;
            if (Request.Form[txtEmbalaceQty.UniqueID] != "")
                entity.EmbalaceQty = Convert.ToDecimal(Request.Form[txtEmbalaceQty.UniqueID]);
            else
                entity.EmbalaceQty = 0;

            if (cboCoenamRule.Value != null && cboCoenamRule.Value.ToString() != "")
                entity.GCCoenamRule = cboCoenamRule.Value.ToString();
            else
                entity.GCCoenamRule = null;

            entity.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
            #endregion
        }

        public void SavePrescriptionHd(IDbContext ctx, ref int prescriptionID, ref int transactionID, ref string transactionNo)
        {
            PrescriptionOrderHdDao entityOrderHdDao = new PrescriptionOrderHdDao(ctx);

            if (hdnPrescriptionOrderID.Value == "0" || hdnTransactionID.Value == "0")
            {
                #region PrescriptionOrderHd
                PrescriptionOrderHd entityOrderHd = new PrescriptionOrderHd();
                entityOrderHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                entityOrderHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityOrderHd.PrescriptionDate = Helper.GetDatePickerValue(Request.Form[txtPrescriptionDate.UniqueID]);
                entityOrderHd.PrescriptionTime = Request.Form[txtPrescriptionTime.UniqueID];
                entityOrderHd.ClassID = Convert.ToInt32(hdnClassID.Value);
                entityOrderHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                entityOrderHd.LocationID = Convert.ToInt32(cboLocation.Value);
                entityOrderHd.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                entityOrderHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                entityOrderHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                entityOrderHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityOrderHd.TransactionCode, entityOrderHd.PrescriptionDate, ctx);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                if (hdnDispensaryServiceUnitID.Value != "" && hdnDispensaryServiceUnitID.Value != null)
                    entityOrderHd.DispensaryServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                else
                    entityOrderHd.DispensaryServiceUnitID = 0;

                //entityOrderHd.LocationID = Convert.ToInt32(cboLocation.Value);
                entityOrderHd.CreatedBy = AppSession.UserLogin.UserID;
                entityOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityOrderHd.LastUpdatedDate = DateTime.Now;
                entityOrderHd.IsCreatedBySystem = true;
                prescriptionID = entityOrderHdDao.InsertReturnPrimaryKeyID(entityOrderHd);
                transactionID = prescriptionID;
                transactionNo = entityOrderHd.PrescriptionOrderNo;
                #endregion
            }
            else
            {
                prescriptionID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                transactionID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                transactionNo = txtTransactionNo.Text;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int PrescriptionID = 0;
                int TransactionID = 0;
                string TransactionNo = "";
                SavePrescriptionHd(ctx, ref PrescriptionID, ref TransactionID, ref TransactionNo);

                retval = PrescriptionID.ToString();
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
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
                //PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionHdID.Value));
                //entity.ReferenceNo = txtReferenceNo.Text;
                //entity.TransactionDate = Helper.GetDatePickerValue(txtTransactionDate.Text);
                //entity.TransactionTime = txtTransactionTime.Text;
                //BusinessLayer.UpdatePatientChargesHd(entity);
                return true;
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                errMessage = ex.Message;
                return false;
            }
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
                PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
                try
                {
                    string statusOld = "", statusNew = "";
                    Int32 transactionID = Convert.ToInt32(hdnTransactionID.Value);

                    //Update Status PrescriptionOrderHd
                    PrescriptionOrderHd orderHd = orderHdDao.Get(transactionID);
                    if (orderHd != null)
                    {
                        List<PrescriptionOrderDt> lstOrderDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", hdnTransactionID.Value), ctx);
                        foreach (PrescriptionOrderDt orderDt in lstOrderDt)
                        {
                            if (!orderDt.IsDeleted)
                            {
                                orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
                                orderDt.GCVoidReason = gcDeleteReason;
                                orderDt.VoidReason = reason;
                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                orderDtDao.Update(orderDt);
                            }
                        }

                        orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        orderHd.GCVoidReason = gcDeleteReason;
                        orderHd.VoidReason = reason;
                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        orderHdDao.Update(orderHd);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        errMessage = "Pembatalan transaksi tidak dapat dilakukan karena header Transaksi tidak ditemukan.";
                        result = false;
                        ctx.RollBackTransaction();
                    }
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
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

            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            try
            {
                PrescriptionOrderHd entity = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    #region UDD Processing
                    MedicationScheduleDao entityMedicationScheduleDao = new MedicationScheduleDao(ctx);
                    string filterExp = string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND (GCPrescriptionOrderStatus != '{1}' OR IsDeleted = 0)", hdnPrescriptionOrderID.Value,Constant.OrderStatus.CANCELLED);
                    List<PrescriptionOrderDt> lstDetail = BusinessLayer.GetPrescriptionOrderDtList(filterExp, ctx);
                    if (lstDetail.Count > 0)
                    {
                        foreach (PrescriptionOrderDt orderDt in lstDetail)
                        {
                            decimal duration = 1;

                            if (!orderDt.IsAsRequired)
                                duration = orderDt.DosingDuration;
                            else
                                duration = 1;

                            DateTime StartDate = orderDt.StartDate;
                            for (int i = 0; i < duration; i++)
                            {
                                DateTime currentDate = StartDate.AddDays(i);
                                int sequenceNo = 1;
                                for (int j = 0; j < orderDt.Frequency; j++)
                                {
                                    MedicationSchedule oSchedule = new MedicationSchedule();
                                    oSchedule.VisitID = AppSession.RegisteredPatient.VisitID;
                                    oSchedule.PrescriptionOrderID = orderDt.PrescriptionOrderID;
                                    oSchedule.PrescriptionOrderDetailID = orderDt.PrescriptionOrderDetailID;
                                    oSchedule.ItemID = orderDt.ItemID;
                                    if (orderDt.IsCompound)
                                        oSchedule.ItemName = !string.IsNullOrEmpty(orderDt.CompoundDrugname) ? orderDt.CompoundDrugname : "OBAT RACIKAN";
                                    else
                                        oSchedule.ItemName = orderDt.DrugName;
                                    oSchedule.MedicationDate = currentDate;
                                    oSchedule.SequenceNo = sequenceNo.ToString();
                                    oSchedule.MedicationTime = orderDt.StartTime;
                                    oSchedule.NumberOfDosage = orderDt.NumberOfDosage;
                                    oSchedule.NumberOfDosageInString = orderDt.NumberOfDosageInString;
                                    oSchedule.GCDosingUnit = orderDt.GCDosingUnit;
                                    oSchedule.ConversionFactor = orderDt.ConversionFactor;
                                    oSchedule.ResultQuantity = orderDt.ResultQty;
                                    oSchedule.ChargeQuantity = orderDt.ChargeQty;
                                    oSchedule.IsAsRequired = orderDt.IsAsRequired;
                                    oSchedule.IsMorning = oSchedule.IsNoon = oSchedule.IsEvening = oSchedule.IsNight = false;
                                    if (j == 0) oSchedule.IsMorning = true;
                                    if (j == 1) oSchedule.IsNoon = true;
                                    if (j == 2) oSchedule.IsEvening = true;
                                    if (j == 3) oSchedule.IsNight = true;
                                    oSchedule.GCRoute = orderDt.GCRoute;
                                    oSchedule.GCCoenamRule = orderDt.GCCoenamRule;
                                    oSchedule.MedicationAdministration = orderDt.MedicationAdministration;
                                    if (orderDt.IsCompound || !orderDt.IsUsingUDD)
                                        oSchedule.GCMedicationStatus = Constant.MedicationStatus.DIPROSES_FARMASI;
                                    else                                   
                                        oSchedule.GCMedicationStatus = Constant.MedicationStatus.OPEN;
                                    oSchedule.IsInternalMedication = true;
                                    oSchedule.CreatedBy = AppSession.UserLogin.UserID;
                                    sequenceNo++;
                                    entityMedicationScheduleDao.Insert(oSchedule);
                                }
                            }
                            orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.IN_PROGRESS;
                            orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(orderDt);
                        }

                        entity.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        entity.GCOrderStatus = Constant.OrderStatus.IN_PROGRESS;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entity);
                    }
                    #endregion
                }
                else
                {
                    errMessage = "Transaksi Sudah Diproses. Tidak Bisa Dipropose";
                    return false;
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private string ValidateTransaction()
        {
            string result;
            string lstSelectedID = "";
            List<PatientChargesDt> lstEntityDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsApproved = 0 AND IsDeleted = 0", hdnTransactionID.Value));
            if (lstEntityDt.Count > 0)
            {
                foreach (PatientChargesDt itm in lstEntityDt)
                    lstSelectedID += "," + itm.ItemID;
            }
            string filterExpression = string.Format(" LocationID = {0} AND ItemID IN({1}) AND IsDeleted = 0", hdnLocationID.Value, lstSelectedID.Substring(1));
            List<vItemBalanceForCheckStock> lstBalance = BusinessLayer.GetvItemBalanceForCheckStockList(filterExpression);
            StringBuilder errMessage = new StringBuilder();
            foreach (PatientChargesDt item in lstEntityDt)
            {
                vItemBalanceForCheckStock oBalance = lstBalance.Where(lst => lst.ItemID == item.ItemID).FirstOrDefault();
                if (oBalance != null)
                {
                    if (item.BaseQuantity > oBalance.QuantityEND)
                        errMessage.AppendLine(string.Format("Quantity Item {0} tidak mencukupi di Lokasi ini.", oBalance.ItemName1));
                }
            }

            filterExpression = string.Format("PrescriptionOrderID = '{0}' AND GCPrescriptionOrderStatus = '{1}' AND OrderIsDeleted = 0", hdnDefaultPrescriptionOrderID.Value, Constant.TestOrderStatus.OPEN);
            List<vPrescriptionOrderDt1> lstOpenOrder = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
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

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string isAllow = "notConfirm";
            int TransactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    if (OnSaveEditRecordEntityDt(ref errMessage, ref isAllow))
                        result += "success";
                    else
                        result += string.Format("fail|{0}|{1}", isAllow, errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref TransactionID, ref isAllow))
                        result += "success";
                    else
                        result += string.Format("fail|{0}|{1}", isAllow, errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                TransactionID = Convert.ToInt32(hdnEntryID.Value);
                if (OnDeleteEntityDt(ref errMessage, param[1]))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = TransactionID.ToString();
        }

        private bool IsPrescriptionItemValid(ref string errMessage, ref string isAllow)
        {
            #region Check For Allergy
            string filterExpression = string.Format("MRN = {0} AND GCAllergenType = '{1}' AND IsDeleted = 0", hdnMRN.Value, Constant.AllergenType.DRUG);
            string allergenName = string.Empty;
            vDrugInfo entityItem = null;

            List<PatientAllergy> lstPatientAllergy = null;
            if (hdnDrugID.Value != "")
            {
                entityItem = BusinessLayer.GetvDrugInfoList(string.Format("ItemID = {0}", hdnDrugID.Value))[0];
                if ((entityItem.ItemID != Convert.ToInt16(hdnDrugID.Value)) && (entityItem.GenericName != string.Empty))
                {
                    filterExpression += string.Format(" AND (Allergen LIKE '%{0}%' OR Allergen LIKE '%{1}%')", entityItem.GenericName, entityItem.ItemName1);
                }
                else filterExpression += string.Format(" AND Allergen LIKE '%{0}%'", entityItem.ItemName1);
                lstPatientAllergy = BusinessLayer.GetPatientAllergyList(filterExpression);
            }

            if (lstPatientAllergy.Count > 0)
            {
                errMessage = string.Format("Pasien memiliki alergi {0} ({1})", txtGenericName.Text, txtDrugName.Text).Replace("()", "");
                return false;
            }
            else
            {
                filterExpression = string.Format("ItemID = {0}", hdnDrugID.Value);
                List<DrugContent> contents = BusinessLayer.GetDrugContentList(filterExpression);
                foreach (DrugContent item in contents)
                {
                    filterExpression = string.Format("MRN = {0} AND GCAllergenType = '{1}' AND Allergen LIKE '%{2}%' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, Constant.AllergenType.DRUG, item.Keyword);
                    lstPatientAllergy = BusinessLayer.GetPatientAllergyList(filterExpression);
                    if (lstPatientAllergy.Count > 0)
                    {
                        errMessage = string.Format("Pasien memiliki alergi {0} ({1})", item.ContentText, txtDrugName.Text).Replace("()", "");
                        return false;
                    }
                }
            }
            #endregion

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.FM_KONTROL_ADVERSE_REACTION, Constant.SettingParameter.FM_MAKSIMUM_DURASI_NARKOTIKA, Constant.SettingParameter.FM_KONTROL_DUPLIKASI_TERAPI));
            SettingParameter setParMaxDurasiNarkotika = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_MAKSIMUM_DURASI_NARKOTIKA).FirstOrDefault();
            SettingParameter setParControlAdverseReaction = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_KONTROL_ADVERSE_REACTION).FirstOrDefault();
            bool isControlTheraphyDuplication = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_KONTROL_DUPLIKASI_TERAPI).FirstOrDefault().ParameterValue == "0" ? false : true;

            string prescriptionOrderId = hdnPrescriptionOrderID.Value;

            if (isControlTheraphyDuplication)
            {
                #region Duplicate Theraphy
                if (prescriptionOrderId != "0" && prescriptionOrderId != "")
                {
                    string filterExp = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", prescriptionOrderId);
                    List<vPrescriptionOrderDt1> itemlist = BusinessLayer.GetvPrescriptionOrderDt1List(filterExp);
                    foreach (var item in itemlist)
                    {
                        //Generic Name
                        if ((item.ItemID.ToString() != hdnDrugID.Value) && (item.GenericName.ToLower().TrimEnd() == txtGenericName.Text.ToLower().TrimEnd()) && !item.GenericName.Equals(string.Empty))
                        {
                            errMessage = string.Format("Duplikasi obat dengan nama generik {0} yang sama ({1})", item.GenericName.TrimEnd(), item.DrugName.TrimEnd());
                            return false;
                        }
                        vDrugInfo drugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemCode = '{0}'", item.ItemCode)).FirstOrDefault();
                        if (drugInfo != null)
                        {
                            //ATC Class
                            if ((item.ItemID.ToString() != hdnDrugID.Value) && ((drugInfo.ATCClassCode == entityItem.ATCClassCode) && (!String.IsNullOrEmpty(entityItem.ATCClassCode))))
                            {
                                errMessage = string.Format("Duplikasi obat dengan Kelompok/Kelas ATC {0} yang sama ({1})", drugInfo.ATCClassName.TrimEnd(), item.DrugName.TrimEnd());
                                return false;
                            }
                            //Kelompok Theraphy
                            if ((item.ItemID.ToString() != hdnDrugID.Value) && (drugInfo.MIMSClassCode.ToLower().TrimEnd() == entityItem.MIMSClassCode.ToLower().TrimEnd()) && (!String.IsNullOrEmpty(entityItem.MIMSClassCode)))
                            {
                                errMessage = string.Format("Duplikasi obat dengan Kelompok Terapi {0} yang sama ({1})", drugInfo.MIMSClassName.TrimEnd(), item.DrugName.TrimEnd());
                                return false;
                            }
                        }
                    }
                }
                #endregion
            }

            #region psikotropika & narkotika
            if (entityItem.GCDrugClass == Constant.DrugClass.MORPHIN || entityItem.GCDrugClass == Constant.DrugClass.NARKOTIKA || entityItem.GCDrugClass == Constant.DrugClass.PSIKOTROPIKA)
            {
                int frekuensiDay = 0;
                frekuensiDay = Convert.ToInt32(txtFrequencyNumber.Text);
                if (cboFrequencyTimeline.Value.ToString() == Constant.DosingFrequency.WEEK) frekuensiDay = frekuensiDay * 7;
                else if (cboFrequencyTimeline.Value.ToString() == Constant.DosingFrequency.HOUR) frekuensiDay = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(frekuensiDay) / 24.0));
                if (Convert.ToInt32(setParMaxDurasiNarkotika.ParameterValue) > 0)
                {
                    if (Convert.ToInt32(frekuensiDay) > Convert.ToInt32(setParMaxDurasiNarkotika.ParameterValue))
                    {
                        errMessage = string.Format("Obat {0} Mengandung Narkotika, pemakaian tidak boleh lebih dari {1} hari", entityItem.ItemName1.TrimEnd(), setParMaxDurasiNarkotika.ParameterValue);
                        return false;
                    }
                }
            }
            #endregion

            if (setParControlAdverseReaction.ParameterValue == "0")
            {
                isAllow = "confirm";
            }

            #region Adverse Reaction
            prescriptionOrderId = hdnPrescriptionOrderID.Value;
            if (prescriptionOrderId != "0")
            {
                filterExpression = string.Format("ItemID = {0}", hdnDrugID.Value);
                List<DrugReaction> reactions = BusinessLayer.GetDrugReactionList(filterExpression);
                foreach (DrugReaction advReaction in reactions)
                {
                    string filterExp = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", prescriptionOrderId);
                    List<vPrescriptionOrderDt> itemlist = BusinessLayer.GetvPrescriptionOrderDtList(filterExp);
                    foreach (var item in itemlist)
                    {
                        vDrugInfo drugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemCode = '{0}'", item.ItemCode)).FirstOrDefault();
                        if (drugInfo != null)
                        {
                            //Generic Name
                            if (drugInfo.GenericName.ToLower().TrimEnd().Contains(advReaction.AdverseReactionText1.ToLower().TrimEnd())
                                || advReaction.AdverseReactionText1.ToLower().TrimEnd().Contains(drugInfo.GenericName.ToLower().TrimEnd()))
                            {
                                errMessage = string.Format("Terjadi interaksi obat dengan {0} ({1}) \n Catatan Interaksi Obat: \n {2}", item.DrugName.TrimEnd(), drugInfo.GenericName, advReaction.AdverseReactionText2);
                                return false;
                            }
                        }
                    }
                }
            }
            #endregion


            return (errMessage == string.Empty);
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int TransactionID, ref string isAllow)
        {
            bool result = true;
            result = IsPrescriptionItemValid(ref errMessage, ref isAllow);
            if (result || isAllow.Equals("confirm"))
            {
                
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdDao entityOrderHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderDtDao entityOrderDtDao = new PrescriptionOrderDtDao(ctx);
                try
                {
                    int PrescriptionID = 0;
                    string TransactionNo = "";
                    SavePrescriptionHd(ctx, ref PrescriptionID, ref TransactionID, ref TransactionNo);

                    PrescriptionOrderHd entityOrderHd = entityOrderHdDao.Get(TransactionID);
                    if (entityOrderHd.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                    {
                        PrescriptionOrderDt entityOrderDt = new PrescriptionOrderDt();
                        ControlToEntity(entityOrderDt);

                        entityOrderDt.PrescriptionOrderID = PrescriptionID;
                        entityOrderDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityOrderDtDao.Insert(entityOrderDt);
                    }
                    else
                    {
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                        result = false;
                    }

                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    Helper.InsertErrorLog(ex);
                    result = false;
                    errMessage = ex.Message;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            return result;
        }

        private bool OnSaveEditRecordEntityDt(ref string errMessage, ref string isAllow)
        {
            bool result = true;
            errMessage = string.Empty;
            result = IsPrescriptionItemValid(ref errMessage, ref isAllow);
            if (result || isAllow.Equals("confirm"))
            {
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
                try
                {
                    int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                    if (transactionID > 0)
                    {
                        PrescriptionOrderHd entityHd = entityHdDao.Get(transactionID);
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                        {
                            PrescriptionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                            if (!entityDt.IsDeleted)
                            {
                                ControlToEntity(entityDt);
                                entityDtDao.Update(entityDt);
                            }
                        }
                        else
                        {
                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            result = false;
                        }
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    Helper.InsertErrorLog(ex);
                    result = false;
                    errMessage = ex.Message;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }

        private bool OnDeleteEntityDt(ref string errMessage, string param)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            string[] paramDelete = param.Split(';');
            int ID = Convert.ToInt32(paramDelete[0]);
            string gcDeleteReason = paramDelete[1];
            string reason = paramDelete[2];
            try
            {
                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                if (transactionID > 0)
                {
                    PrescriptionOrderHd entityHd = entityHdDao.Get(transactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                    {
                        PrescriptionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                        if (!entityDt.IsCompound)
                        {
                            if (entityDt.IsCreatedFromOrder)
                                entityDt.GCPrescriptionOrderStatus = Constant.OrderStatus.OPEN;
                            else
                                entityDt.IsDeleted = true;

                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                        }
                        else
                        {
                            List<PrescriptionOrderDt> lstPrescriptionDt = BusinessLayer.GetPrescriptionOrderDtList(String.Format("PrescriptionOrderDetailID = {0} OR ParentID = {0}", hdnEntryID.Value), ctx);

                            String lstOrderDetailID = "";
                            foreach (PrescriptionOrderDt obj in lstPrescriptionDt)
                            {
                                if (lstOrderDetailID == "") lstOrderDetailID += obj.PrescriptionOrderDetailID;
                                else lstOrderDetailID += "," + obj.PrescriptionOrderDetailID;
                            }

                            foreach (PrescriptionOrderDt obj in lstPrescriptionDt)
                            {
                                if (obj.IsCreatedFromOrder)
                                    obj.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;
                                else
                                    obj.IsDeleted = true;

                                obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(obj);
                            }
                        }

                        int dtProcessedCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.OrderStatus.RECEIVED), ctx);
                        if (dtProcessedCount == 0)
                        {
                            PrescriptionOrderHd orderHd = entityHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                            orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityHdDao.Update(orderHd);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                        result = false;
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
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