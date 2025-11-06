using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.Service;


namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PSLeaveScheduleByServiceUnitCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            string[] par = param.Split('|');
            hdnParamedicID.Value = par[0];
            hdnHealthcareServiceUnitID.Value = par[1];

            vServiceUnitParamedic entity = BusinessLayer.GetvServiceUnitParamedicList(string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1}", hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value))[0];
            hdnHealthcareID.Value = entity.HealthcareID;
            txtParamedicName.Text = entity.ParamedicName;
            txtHealthcareName.Text = entity.HealthcareName;
            hdnDatePickerToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateLeave.Attributes.Add("readonly", "readonly");

            Helper.SetControlEntrySetting(txtDateLeave, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(true, true, true), "mpEntryPopup");

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            String filterExpression = string.Format("ParamedicID = {0} AND ParamedicLeaveScheduleIsDeleted = 0 AND IsDeleted = 0", hdnParamedicID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvParamedicLeaveScheduleServiceUnitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vParamedicLeaveScheduleServiceUnit> lstEntity = BusinessLayer.GetvParamedicLeaveScheduleServiceUnitList(filterExpression, 8, pageIndex, "ParamedicLeaveScheduleID ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }
            else
            {
                if (param[0] == "save")
                {
                    if (hdnID.Value != "")
                    {
                        if (OnSaveEditRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                    else
                    {
                        if (OnSaveAddRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                }
                else if (param[0] == "delete")
                {
                    if (OnDeleteRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Save Entity
        private void ControlToEntity(ParamedicLeaveScheduleServiceUnit entity)
        {
            entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entity.ParamedicLeaveScheduleID = Convert.ToInt32(hdnParamedicLeaveScheduleID.Value);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            //ParamedicLeaveScheduleDao entityDao = new ParamedicLeaveScheduleDao(ctx);
            ParamedicLeaveScheduleServiceUnitDao entityDao = new ParamedicLeaveScheduleServiceUnitDao(ctx);
            try
            {
                //ParamedicLeaveSchedule entity = new ParamedicLeaveSchedule();
                ParamedicLeaveScheduleServiceUnit entity = new ParamedicLeaveScheduleServiceUnit();
                Int32 paramedicLeaveScheduleID = Convert.ToInt32(hdnParamedicLeaveScheduleID.Value);
                //Int32 healthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);

                if (txtDateLeave.Text != "" && txtServiceUnitCode.Text != "")
                {
                    ControlToEntity(entity);
                    entity.IsDeleted = false;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entity.CreatedDate = DateTime.Now;
                    entityDao.InsertReturnPrimaryKeyID(entity);
                    result = true;
                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Harap pilih jadwal dokter dan klinik";
                    result = false;
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
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

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            //ParamedicLeaveScheduleDao entityDao = new ParamedicLeaveScheduleDao(ctx);
            ParamedicLeaveScheduleServiceUnitDao entityDao = new ParamedicLeaveScheduleServiceUnitDao(ctx);
            try
            {
                //ParamedicLeaveSchedule entity = BusinessLayer.GetParamedicLeaveSchedule(Convert.ToInt32(hdnID.Value));
                ParamedicLeaveScheduleServiceUnit entity = BusinessLayer.GetParamedicLeaveScheduleServiceUnit(Convert.ToInt32(hdnID.Value));
                if (!entity.IsDeleted)
                {
                    if (txtServiceUnitCode.Text != string.Empty)
                    {
                        ControlToEntity(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entity.LastUpdatedDate = DateTime.Now;
                        entityDao.Update(entity);
                        result = true;
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        errMessage = "Harap pilih klinik";
                        result = false;
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    errMessage = "";
                    result = false;
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            //ParamedicLeaveScheduleDao entityDao = new ParamedicLeaveScheduleDao(ctx);
            ParamedicLeaveScheduleServiceUnitDao entityDao = new ParamedicLeaveScheduleServiceUnitDao(ctx);

            try
            {
                //ParamedicLeaveSchedule entity = BusinessLayer.GetParamedicLeaveSchedule(Convert.ToInt32(hdnID.Value));
                ParamedicLeaveScheduleServiceUnit entity = BusinessLayer.GetParamedicLeaveScheduleServiceUnit(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedDate = DateTime.Now;
                entityDao.Update(entity);
                result = true;
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}