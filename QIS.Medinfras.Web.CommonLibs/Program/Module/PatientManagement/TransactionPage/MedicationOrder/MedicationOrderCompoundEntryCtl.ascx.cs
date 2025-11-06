using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Text;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicationOrderCompoundEntryCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            hdnQueryString.Value = queryString;
            string[] param = queryString.Split('|');

            IsAdd = true;
            //lebih dari 3 add
            if (param.Length > 3)
            {
                hdnPrescriptionOrderID.Value = param[0];
                hdnVisitID.Value = param[5];
                hdnDispensaryServiceUnitID.Value = param[7];
                hdnLocationID.Value = param[8];

                hdnFilterExpression.Value = string.Format("ItemID NOT IN (SELECT ItemID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = {0} AND IsDeleted = 0) AND IsDeleted = 0", hdnPrescriptionOrderID.Value);
                vConsultVisit entityConsultVisit = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0}", hdnVisitID.Value))[0];
                hdnChargeClassID.Value = entityConsultVisit.ChargeClassID.ToString();
                hdnRegistrationID.Value = entityConsultVisit.RegistrationID.ToString();
                hdnDepartmentID.Value = entityConsultVisit.DepartmentID;
                SetControlProperties();
            }
            else
            {
                hdnPrescriptionOrderID.Value = param[0];
                hdnParentID.Value = param[1];
                hdnVisitID.Value = param[2].ToString();

                vConsultVisit entityConsultVisit = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0}", param[2]))[0];
                hdnChargeClassID.Value = entityConsultVisit.ChargeClassID.ToString();
                hdnRegistrationID.Value = entityConsultVisit.RegistrationID.ToString();

                SetControlProperties();
                if (hdnParentID.Value != "")
                {
                    List<PrescriptionOrderDt> lstEntity = BusinessLayer.GetPrescriptionOrderDtList(String.Format("PrescriptionOrderDetailID = {0}", hdnParentID.Value));
                    if (lstEntity.Count > 0) EntityToControl(lstEntity[0]);
                }
            }
            BindListView();

            Helper.SetControlEntrySetting(txtGenericName, new ControlEntrySetting(true, true, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemName1, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDose, new ControlEntrySetting(true, true, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDoseUnit, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtCompoundQty, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboCompoundUnit, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtConversionFactor, new ControlEntrySetting(true, true, true), "mpTrxPopup");
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        #region Bind List View
        private void BindListView() 
        {
            String filterExpression = "";
            if (hdnParentID.Value == "" || hdnParentID.Value == "0")
                filterExpression = "1 = 0";
            else
                filterExpression = string.Format("PrescriptionOrderDetailID = {0} OR ParentID = {0} AND IsDeleted = 0", hdnParentID.Value);
            List<vPrescriptionOrderDt> lstEntity = BusinessLayer.GetvPrescriptionOrderDtList(filterExpression);
            if (lstEntity.Count == 1)
                hdnIsEditable.Value = "0";
            else
                hdnIsEditable.Value = "1";
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindListView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtCompoundDrugName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnPopupSignaID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPopupSignaLabel, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPopupSignaName1, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboPopupDrugForm, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboPopupCoenamRule, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboFrequencyTimeline, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUsingSweetener, new ControlEntrySetting(true, true, false));
        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.DRUG_FORM, Constant.StandardCode.COENAM_RULE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPopupDrugForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField<StandardCode>(cboPopupCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            cboFrequencyTimeline.Value = Constant.DosingFrequency.DAY;

            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString("HH:mm");

            txtDispenseQty.Text = "1";
        }

        private void EntityToControl(PrescriptionOrderDt entity)
        {
            txtDispenseQty.Text = entity.DispenseQty.ToString();
            txtCompoundDrugName.Text = entity.CompoundDrugname;
            cboMedicationRoute.Value = entity.GCRoute;
            chkIsUsingSweetener.Checked = entity.IsUseSweetener;
            cboPopupCoenamRule.Value = entity.GCCoenamRule;
            cboFrequencyTimeline.Value = entity.GCDosingFrequency;
            txtFrequencyNumber.Text = entity.Frequency.ToString();
            cboPopupDrugForm.Value = entity.GCDrugForm;
            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = entity.StartTime;

            txtMedicationAdministration.Text = entity.MedicationAdministration;
            txtDosingDuration.Text = entity.DosingDuration.ToString();
            
            txtDosingDurationTimeline.Text = cboFrequencyTimeline.Text;
            if (entity.SignaID != null)
            {
                vSigna signa = BusinessLayer.GetvSignaList(String.Format("SignaID = {0}", entity.SignaID))[0];
                txtPopupSignaLabel.Text = signa.SignaLabel;
                txtPopupSignaName1.Text = signa.SignaName1;
                txtDispenseUnit.Text = signa.DrugForm;
                hdnPopupSignaID.Value = entity.SignaID.ToString();
            }
            cboPopupCoenamRule.Value = entity.GCCoenamRule;
        }

        protected void cboCompoundUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN ( (SELECT GCDoseUnit FROM vDrugInfo WHERE ItemID = {1}),(SELECT GCItemUnit FROM vDrugInfo WHERE ItemID = {2})))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value,hdnItemID.Value));
            Methods.SetComboBoxField<StandardCode>(cboCompoundUnit, lst, "StandardCodeName", "StandardCodeID");
            cboCompoundUnit.SelectedIndex = -1;
        }

        public void SavePrescriptionOrderHd(IDbContext ctx, ref int prescriptionOrderID)
        {
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHd entityHd = null;

            string[] paramHeader = hdnQueryString.Value.ToString().Split('|');
            if (hdnPrescriptionOrderID.Value == "0")
            {
                DateTime prescriptionDate = Helper.DateInStringToDateTime(paramHeader[1]);
                string prescriptionTime = paramHeader[2];
                int paramedicID = Convert.ToInt32(paramHeader[3]);
                string refillInstruction = paramHeader[4];

                entityHd = new PrescriptionOrderHd();
                entityHd.ParamedicID = paramedicID;
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.PrescriptionDate = prescriptionDate;
                entityHd.PrescriptionTime = prescriptionTime;
                entityHd.ClassID = Convert.ToInt32(hdnChargeClassID.Value);
                entityHd.DispensaryServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                entityHd.LocationID = Convert.ToInt32(hdnLocationID.Value);
                entityHd.GCPrescriptionType = Constant.PrescriptionType.DISCHARGE_PRESCRIPTION;
                entityHd.GCRefillInstruction = refillInstruction;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                    entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                else
                    entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                prescriptionOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                prescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
            }

        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            try
            {
                List<PrescriptionOrderDt> lstEntity = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderDetailID = {0} OR ParentID = {0} AND IsDeleted = 0", hdnParentID.Value), ctx);
                foreach (PrescriptionOrderDt entity in lstEntity)
                {
                    ControlToEntityCompoundHd(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entity);
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
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

        #region Process Detail
        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int prescriptionOrderID = 0;
            int parentID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnPrescriptionOrderDetailID.Value.ToString() != "")
                {
                    prescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                    parentID = Convert.ToInt32(hdnParentID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref prescriptionOrderID, ref parentID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                prescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                parentID = Convert.ToInt32(hdnParentID.Value);
                if (OnDeleteEntityDt(ref errMessage, ref parentID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpParentID"] = parentID.ToString();
            panel.JSProperties["cpPrescriptionOrderID"] = prescriptionOrderID.ToString();
        }

        private void ControlToEntityCompoundHd(PrescriptionOrderDt entity)
        {
            entity.CompoundDrugname = txtCompoundDrugName.Text;
            entity.GCDrugForm = cboPopupDrugForm.Value.ToString();

            entity.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);

            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.StartTime = txtStartTime.Text;

            entity.DosingDuration = Convert.ToInt32(txtDosingDuration.Text);
            if (hdnPopupSignaID.Value.ToString() != "")
                entity.SignaID = Convert.ToInt32(hdnPopupSignaID.Value);
            else
                entity.SignaID = null;

            entity.GCRoute = cboMedicationRoute.Value.ToString();
            if (cboPopupCoenamRule.Value != null && cboPopupCoenamRule.Value.ToString() != "")
                entity.GCCoenamRule = cboPopupCoenamRule.Value.ToString();
            else
                entity.GCCoenamRule = null;

            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
            if (entity.Dose > 0)
                entity.ChargeQty = entity.ResultQty = entity.TakenQty = (entity.NumberOfDosage / Convert.ToDecimal(entity.Dose)) * entity.DispenseQty;

            entity.IsUseSweetener = chkIsUsingSweetener.Checked;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
        }

        private void ControlToEntity(PrescriptionOrderDt entity)
        {
            entity.IsRFlag = true;
            entity.IsCompound = true;
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            entity.DrugName = Request.Form[txtItemName1.UniqueID];
            entity.GenericName = txtGenericName.Text;
            entity.CompoundQty = Convert.ToDecimal(txtCompoundQty.Text);
            entity.GCCompoundUnit = cboCompoundUnit.Value.ToString();
            entity.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;
            entity.Dose = Convert.ToDecimal(Request.Form[txtDose.UniqueID]);
            entity.GCDoseUnit = hdnGCDoseUnit.Value.ToString();

            if (entity.GCDoseUnit != entity.GCCompoundUnit)
                entity.NumberOfDosage = Convert.ToDecimal(entity.Dose) * entity.CompoundQty;
            else
                entity.NumberOfDosage = entity.CompoundQty;
            entity.GCDosingUnit = entity.GCDoseUnit;

            entity.ConversionFactor = Convert.ToDecimal(hdnConversionFactor.Value);

            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
            if (entity.Dose > 0)
            {
                entity.ChargeQty = entity.ResultQty = entity.TakenQty = (entity.NumberOfDosage / Convert.ToDecimal(entity.Dose)) * entity.DispenseQty;
            }
            else entity.ChargeQty = entity.ResultQty = entity.TakenQty = entity.CompoundQty;
            entity.IsUseSweetener = chkIsUsingSweetener.Checked;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int prescriptionOrderID, ref int parentID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            try
            {
                SavePrescriptionOrderHd(ctx, ref prescriptionOrderID);
                PrescriptionOrderDt entityDt = new PrescriptionOrderDt();
                ControlToEntityCompoundHd(entityDt);
                ControlToEntity(entityDt);
                if (hdnParentID.Value != "" && hdnParentID.Value != "0")
                {
                    entityDt.ParentID = Convert.ToInt32(hdnParentID.Value);
                    parentID = Convert.ToInt32(hdnParentID.Value);
                }
                else
                    entityDt.ParentID = null;
                entityDt.PrescriptionOrderID = prescriptionOrderID;
                entityDt.LastUpdatedBy = entityDt.CreatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Insert(entityDt);

                if (entityDt.ParentID == null)
                    parentID = BusinessLayer.GetPrescriptionOrderDtMaxID(ctx);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            try
            {
                PrescriptionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnPrescriptionOrderDetailID.Value));
                ControlToEntity(entityDt);
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
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

        private bool OnDeleteEntityDt(ref string errMessage, ref int parentID)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);

            try
            {
                PrescriptionOrderDt entity = entityDtDao.Get(Convert.ToInt32(hdnPrescriptionOrderDetailID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);

                if (entity.ParentID == null)
                {
                    string filterExpression = hdnFilterExpression.Value;
                    List<PrescriptionOrderDt> lstEntity = BusinessLayer.GetPrescriptionOrderDtList(string.Format("ParentID = {0} AND IsDeleted = 0", hdnPrescriptionOrderDetailID.Value), ctx);

                    if (lstEntity.Count > 0)
                    {
                        parentID = lstEntity[0].PrescriptionOrderDetailID;
                        lstEntity[0].ParentID = null;
                        lstEntity[0].GCCoenamRule = entity.GCCoenamRule;

                        foreach (PrescriptionOrderDt obj in lstEntity)
                        {
                            if (obj.ParentID != null)
                                obj.ParentID = parentID;

                            entityDtDao.Update(obj);
                        }
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
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
        #endregion
    }
}