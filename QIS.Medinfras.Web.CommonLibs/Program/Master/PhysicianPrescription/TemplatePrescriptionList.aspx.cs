using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TemplatePrescriptionList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.MASTER_PHYSICIAN_PRESCRIPTION_TEMPLATE;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        public override bool IsShowRightPanel()
        {
            return false;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = true;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtItemName, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true, "1"), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboFrequencyTimeline, new ControlEntrySetting(true, true, true, Constant.DosingFrequency.DAY), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true, "1"), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboDosingUnit, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true, "1"), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true, "1"), "mpEntryPopup");
            Helper.SetControlEntrySetting(chkIsUsingSweetener, new ControlEntrySetting(true, true, false), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboCoenamRule, new ControlEntrySetting(true, true, false), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false), "mpEntryPopup");

            String filterExp = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.COENAM_RULE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExp);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1" || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            cboFrequencyTimeline.Value = Constant.DosingFrequency.DAY;
            cboMedicationRoute.SelectedIndex = 0;

            txtDispenseQty.Text = "1";
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvPrescriptionTemplateHDRowCount(filterExpression) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        protected void cboDosingUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (hdnDrugID.Value != null && hdnDrugID.Value.ToString() != "")
            {
                //List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN ( (SELECT GCDoseUnit FROM vDrugInfo WHERE ItemID = {1}),(SELECT GCItemUnit FROM vDrugInfo WHERE ItemID = {2})))", Constant.StandardCode.ITEM_UNIT, hdnDrugID.Value, hdnDrugID.Value));
                List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (TagProperty LIKE '%1%' OR TagProperty LIKE '%PRE%')", Constant.StandardCode.ITEM_UNIT));
                Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lst, "StandardCodeName", "StandardCodeID");
            }
        }

        private string GetFilterExpression()
        {
            string filterExpression = string.Format("ParamedicID = {0} AND IsDeleted = 0", AppSession.UserLogin.ParamedicID);
            return filterExpression;
        }

        protected string OnGetItemMasterFilterExpression()
        {
            return string.Format("GCItemType IN ('{0}','{1}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPrescriptionTemplateHDRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }
            
            List<vPrescriptionTemplateHD> lstEntity = BusinessLayer.GetvPrescriptionTemplateHDList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionTemplateCode ASC");
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


        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl("~/Libs/Program/Master/PhysicianPrescription/TemplatePrescriptionEntry.aspx");
            return true;
        }
        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Libs/Program/Master/PhysicianPrescription/TemplatePrescriptionEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                PrescriptionTemplateHd entity = BusinessLayer.GetPrescriptionTemplateHd(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePrescriptionTemplateHd(entity);
                return true;
            }
            return false;
        }

        #region Item Request Dt
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnID.Value != "")
            {
                filterExpression = string.Format("PrescriptionTemplateID = {0} AND ParentID IS NULL AND OrderIsDeleted = 0", hdnID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPrescriptionTemplateDtRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPrescriptionTemplateDt> lstEntity = BusinessLayer.GetvPrescriptionTemplateDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionTemplateCode ASC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPrescriptionTemplateDt entity = e.Item.DataItem as vPrescriptionTemplateDt;
            }
        }

        private void ControlToEntity(PrescriptionTemplateDt entity)
        {
            entity.PrescriptionTemplateID = Convert.ToInt32(hdnID.Value);
            entity.IsRFlag = true;
            entity.ItemID = Int32.Parse(hdnItemID.Value);
            entity.DrugName = Request.Form[txtItemName.UniqueID];
            entity.CompoundQty = 0;
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
            entity.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            if (!string.IsNullOrEmpty(cboMedicationRoute.Value.ToString()))
            {
                entity.GCRoute = cboMedicationRoute.Value.ToString();
            }
            else
            {
                entity.GCRoute = Constant.MedicationRoute.OTHER;
            }
            entity.GCDosingUnit = cboDosingUnit.Value.ToString();
            entity.DosingDuration = Convert.ToInt16(txtDosingDuration.Text);
            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
            if (!String.IsNullOrEmpty(cboCoenamRule.Text))
                entity.GCCoenamRule = !String.IsNullOrEmpty(cboCoenamRule.Value.ToString()) ? cboCoenamRule.Value.ToString() : null;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            entity.MedicationPurpose = txtMedicationAdministration.Text;
            entity.IsUseSweetener = chkIsUsingSweetener.Checked;

            if (hdnSignaID.Value != "")
            {
                entity.SignaID = Convert.ToInt32(hdnSignaID.Value);
            }
            else
            {
                entity.SignaID = null;
            }
            string strengthUnit = hdnStrengthUnit.Value;
            string strengthAmount = hdnStrengthAmount.Value;
            if (!String.IsNullOrEmpty(strengthUnit))
            {
                entity.Dose = Convert.ToDecimal(strengthAmount);
                entity.GCDoseUnit = strengthUnit;
            }
            else
            {
                entity.Dose = 0;
                entity.GCDoseUnit = null;
            }
            entity.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            entity.GCDosingUnit = cboDosingUnit.Value.ToString();
            entity.IsMorning = chkIsMorning.Checked;
            entity.IsNoon = chkIsNoon.Checked;
            entity.IsEvening = chkIsEvening.Checked;
            entity.IsNight = chkIsNight.Checked;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            entity.DosingDuration = Convert.ToDecimal(txtDosingDuration.Text);
            entity.IsAsRequired = chkIsAsRequired.Checked;
            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
            entity.TakenQty = Convert.ToDecimal(txtDispenseQty.Text);
            entity.ResultQty = entity.DispenseQty;
            entity.ChargeQty = entity.DispenseQty;
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            int prescriptiontemplateDetailID = 0;

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "refresh")
                {
                    BindGridViewDt(1, true, ref pageCount);
                    result = string.Format("refresh|{0}", pageCount);
                }
                else if (param[0] == "save")
                {
                    if (hdnIsFlagAdd.Value.ToString() != "1")
                    {
                        prescriptiontemplateDetailID = Convert.ToInt32(hdnPrescriptionTemplateDetailID.Value);
                        if (OnSaveEditRecordDt(ref errMessage, prescriptiontemplateDetailID))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                    else
                    {
                        if (OnSaveAddRecordDt(ref errMessage))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                }
                else if (param[0] == "delete")
                {
                    prescriptiontemplateDetailID = Convert.ToInt32(hdnPrescriptionTemplateDetailID.Value);
                    if (OnDeleteEntityDt(ref errMessage, prescriptiontemplateDetailID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else // refresh
                {
                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            BindGridViewDt(1, true, ref pageCount);
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveAddRecordDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionTemplateHdDao entityHdDao = new PrescriptionTemplateHdDao(ctx);
            PrescriptionTemplateDtDao entityDtDao = new PrescriptionTemplateDtDao(ctx);
            try
            {
                PrescriptionTemplateHd entityHd = BusinessLayer.GetPrescriptionTemplateHd(Convert.ToInt32(hdnID.Value));
                PrescriptionTemplateDt entityDt = new PrescriptionTemplateDt();
                if (entityDt != null)
                {
                    ControlToEntity(entityDt);
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                    errMessage = "Obat tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
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

        private bool OnSaveEditRecordDt(ref string errMessage, int prescriptiontemplateDetailID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionTemplateHdDao entityHdDao = new PrescriptionTemplateHdDao(ctx);
            PrescriptionTemplateDtDao entityDtDao = new PrescriptionTemplateDtDao(ctx);
            try
            {
                PrescriptionTemplateDt entityDt = entityDtDao.Get(prescriptiontemplateDetailID);
                if (entityDt != null)
                {
                    ControlToEntity(entityDt);
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                    errMessage = "Obat tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
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

        private bool OnDeleteEntityDt(ref string errMessage, int prescriptiontemplateDetailID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionTemplateHdDao entityHdDao = new PrescriptionTemplateHdDao(ctx);
            PrescriptionTemplateDtDao entityDtDao = new PrescriptionTemplateDtDao(ctx);
            try
            {
                PrescriptionTemplateDt entityDt = entityDtDao.Get(prescriptiontemplateDetailID);
                if (entityDt != null)
                {
                    entityDt.IsDeleted = true;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                    errMessage = "Obat tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
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
    }
}