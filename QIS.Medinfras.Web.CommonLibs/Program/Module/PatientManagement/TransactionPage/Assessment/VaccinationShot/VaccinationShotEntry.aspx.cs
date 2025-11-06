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
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VaccinationShotEntry : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        protected string PatientDOB = "";
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalDiagnostic.VACCINATION_SHOT;
        }

        #region List
        protected override void InitializeDataControl()
        {
            PatientDOB = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN).DateOfBirth.ToString("yyyyMMdd");   

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND IsDeleted = 0", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            hdnVaccinationShotID.Value = "";
            txtVaccinationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboParamedicID.ClientEnabled = false;
                cboParamedicID.Value = userLoginParamedic.ToString();
            }

            BindGridView(1, true, ref PageCount); 
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnVaccinationShotID.Value != "")
            {
                filterExpression = hdnFilterExpression.Value;
                if (filterExpression != "")
                    filterExpression += " AND ";
                filterExpression += string.Format("HeaderID = {0} AND IsDeleted = 0", hdnVaccinationShotID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvVaccinationShotDtRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vVaccinationShotDt> lstEntity = BusinessLayer.GetvVaccinationShotDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
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
                VaccinationShotDt entity = BusinessLayer.GetVaccinationShotDt(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVaccinationShotDt(entity);
                return true;
            }
            return false;
        }
        #endregion

        #region Entry
        protected override void SetControlProperties()
        {
            List<VaccinationType> lstVaccination = BusinessLayer.GetVaccinationTypeList("IsDeleted = 0");
            Methods.SetComboBoxField<VaccinationType>(cboVaccinationType, lstVaccination, "VaccinationTypeName", "VaccinationTypeID");
            cboVaccinationType.SelectedIndex = 0;

            List<StandardCode> lstVaccinationRoute = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.VACCINATION_ROUTE));
            Methods.SetComboBoxField<StandardCode>(cboVaccinationRoute, lstVaccinationRoute, "StandardCodeName", "StandardCodeID");
            cboVaccinationRoute.SelectedIndex = 0;
        }

        protected void cboDosingUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<vItemAlternateUnit> lst = BusinessLayer.GetvItemAlternateUnitList(string.Format("ItemID = {0}", hdnItemID.Value));
            StandardCode sc = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {0})", hdnItemID.Value)).FirstOrDefault();
            lst.Add(new vItemAlternateUnit { GCAlternateUnit = sc.StandardCodeID, AlternateUnit = sc.StandardCodeName });
            Methods.SetComboBoxField<vItemAlternateUnit>(cboDosingUnit, lst, "AlternateUnit", "GCAlternateUnit");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboVaccinationType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboVaccinationRoute, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtVaccinationNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDosingUnit, new ControlEntrySetting(true, true, true));
        }

        private void ControlToEntity(VaccinationShotDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.VaccinationNo = txtVaccinationNo.Text;
            entityDt.VaccinationTypeID = Convert.ToInt32(cboVaccinationType.Value);
            entityDt.GCVaccinationRoute = cboVaccinationRoute.Value.ToString();
            entityDt.Dose = Convert.ToDecimal(txtDosingDose.Text);
            entityDt.GCDoseUnit = cboDosingUnit.Value.ToString();
            entityDt.GCBaseUnit = hdnGCBaseUnit.Value;
            entityDt.IsBooster = chkIsBooster.Checked;
            entityDt.ConversionFactor = Convert.ToDecimal(hdnConversionFactor.Value);
            entityDt.ChargeQuantity = entityDt.StockQuantity = entityDt.Dose * entityDt.ConversionFactor;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            VaccinationShotHdDao entityHdDao = new VaccinationShotHdDao(ctx);
            VaccinationShotDtDao entityDtDao = new VaccinationShotDtDao(ctx);
            ItemMasterDao entityItemMasterDao = new ItemMasterDao(ctx);
            try
            {
                VaccinationShotHd entityHd = null;
                if (hdnVaccinationShotID.Value == "")
                {
                    entityHd = new VaccinationShotHd();
                    entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
                    entityHd.VaccinationDate = Helper.GetDatePickerValue(txtVaccinationDate);
                    entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
                    entityHd.AgeInYear = Convert.ToInt16(Request.Form[txtAgeInYear.UniqueID]);
                    entityHd.AgeInMonth = Convert.ToInt16(Request.Form[txtAgeInMonth.UniqueID]);
                    entityHd.AgeInDay = Convert.ToInt16(Request.Form[txtAgeInDay.UniqueID]);
                    entityHd.IsInternal = true;
                    entityHd.MRN = AppSession.RegisteredPatient.MRN;
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Insert(entityHd);

                    entityHd.ID = BusinessLayer.GetVaccinationShotHdMaxID(ctx);
                }
                else
                {
                    entityHd = entityHdDao.Get(Convert.ToInt32(hdnVaccinationShotID.Value));
                }
                retval = entityHd.ID.ToString();

                VaccinationShotDt entityDt = new VaccinationShotDt();
                ControlToEntity(entityDt);
                entityDt.HeaderID = entityHd.ID;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Insert(entityDt);
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                VaccinationShotDt entity = BusinessLayer.GetVaccinationShotDt(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVaccinationShotDt(entity);
                retval = hdnVaccinationShotID.Value;
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion
    }
}
