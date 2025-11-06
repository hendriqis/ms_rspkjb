using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ClaimProcedureEntry : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.DIAGNOSE_PROCEDURE_CLAIM;
        }

        #region List
        protected override void InitializeDataControl()
        {
            ctlToolbar.SetSelectedMenu(2);

            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();

            int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", AppSession.RegisteredPatient.HealthcareServiceUnitID));
            if (count > 0)
                hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
            else
                hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

            hdnDefaultParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
            ParamedicMaster entity = BusinessLayer.GetParamedicMaster(AppSession.RegisteredPatient.ParamedicID);
            hdnDefaultParamedicCode.Value = entity.ParamedicCode;
            hdnDefaultParamedicName.Value = entity.FullName;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientProcedureRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_FIVE);
            }

            List<vPatientProcedure> lstEntity = BusinessLayer.GetvPatientProcedureList(filterExpression, Constant.GridViewPageSize.GRID_FIVE, pageIndex, "ID DESC");
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
        #endregion

        #region Entry
        protected override void SetControlProperties()
        {
            txtClaimProcedureDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtClaimProcedureTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            hdnDefaultDateToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnDefaultTimeToday.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtClaimProcedureDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClaimProcedureTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClaimProcedureCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClaimProcedureName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtClaimProcedureText, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProcedurev5Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtProcedurev5Name, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtProcedurev6Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtProcedurev6Name, new ControlEntrySetting(false, false, false));
        }

        private void ControlToEntity(PatientProcedure entity)
        {
            entity.ClaimProcedureDate = Helper.GetDatePickerValue(txtClaimProcedureDate.Text);
            entity.ClaimProcedureTime = txtClaimProcedureTime.Text;
            entity.ClaimProcedureID = txtClaimProcedureCode.Text;
            entity.ClaimProcedureText = txtClaimProcedureText.Text;
            entity.ClaimINAProcedureID = txtProcedurev6Code.Text;
            entity.ClaimINAProcedureText = txtProcedurev6Name.Text;
            entity.ClaimProcedureBy = AppSession.UserLogin.UserID;
            entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
            entity.IsCreatedBySystem = false;
            entity.ReferenceID = null;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientProcedureDao oPatientProcedureDao = new PatientProcedureDao(ctx);

            try
            {
                PatientProcedure entity = new PatientProcedure();
                ControlToEntity(entity);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                oPatientProcedureDao.Insert(entity);

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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientProcedureDao oPatientProcedureDao = new PatientProcedureDao(ctx);

            try
            {
                PatientProcedure entity = oPatientProcedureDao.Get(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                oPatientProcedureDao.Update(entity);

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

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientProcedureDao oPatientProcedureDao = new PatientProcedureDao(ctx);

            try
            {
                PatientProcedure entity = BusinessLayer.GetPatientProcedure(Convert.ToInt32(hdnID.Value));
                if (entity != null)
                {
                    if (string.IsNullOrEmpty(entity.ProcedureID) && string.IsNullOrEmpty(entity.FinalProcedureID) && string.IsNullOrEmpty(entity.ProcedureText) && string.IsNullOrEmpty(entity.FinalProcedureText))
                    {
                        entity.IsDeleted = true;
                        entity.LastUpdatedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oPatientProcedureDao.Update(entity);
                    }
                    else
                    {
                        entity.ClaimProcedureDate = null;
                        entity.ClaimProcedureBy = null;
                        entity.ClaimINAProcedureID = null;
                        entity.ClaimINAProcedureText = null;
                        entity.ClaimProcedureID = null;
                        entity.ClaimProcedureText = null;
                        entity.LastUpdatedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oPatientProcedureDao.Update(entity);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Data prosedur/tindakan tidak ditemukan.";
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
    }
}