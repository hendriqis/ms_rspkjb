using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class HolidayEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.HOLIDAY;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                SetControlProperties();
                Holiday entity = BusinessLayer.GetHoliday(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

            cboHolidayYear.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            cboHolidayYear.DataSource = Enumerable.Range(DateTime.Now.Year, 10).Reverse();
            cboHolidayYear.EnableCallbackMode = false;
            cboHolidayYear.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboHolidayYear.DropDownStyle = DropDownStyle.DropDownList;
            cboHolidayYear.DataBind();
            //cboHolidayYear.Items.Insert(0, new ListEditItem { Value = "", Text = "" });

            cboHolidayMonth.DataSource = Enumerable.Range(1, 12).Select(a => new
            {
                MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                MonthNumber = a
            });

            cboHolidayMonth.TextField = "MonthName";
            cboHolidayMonth.ValueField = "MonthNumber";
            cboHolidayMonth.EnableCallbackMode = false;
            cboHolidayMonth.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboHolidayMonth.DropDownStyle = DropDownStyle.DropDownList;
            cboHolidayMonth.DataBind();

            //cboHolidayDate.DataSource = Enumerable.Range(1, 31);
            //cboHolidayDate.EnableCallbackMode = false;
            //cboHolidayDate.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            //cboHolidayDate.DropDownStyle = DropDownStyle.DropDownList;
            //cboHolidayDate.DataBind();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboHolidayDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboHolidayMonth, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsAnnual, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboHolidayYear, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtHolidayName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(Holiday entity)
        {
            cboHolidayDate.Value = entity.HolidayDate.ToString();
            cboHolidayMonth.Value = entity.HolidayMonth.ToString();
            chkIsAnnual.Checked = entity.IsAnnualHoliday;
            if (!entity.IsAnnualHoliday)
                cboHolidayYear.Value = entity.HolidayYear.ToString();
            txtHolidayName.Text = entity.HolidayName;
        }

        private void ControlToEntity(Holiday entity)
        {
            entity.HolidayDate = Convert.ToInt16(cboHolidayDate.Value);
            entity.HolidayMonth = Convert.ToInt16(cboHolidayMonth.Value);
            entity.IsAnnualHoliday = chkIsAnnual.Checked;
            if (!entity.IsAnnualHoliday)
                entity.HolidayYear = Convert.ToInt16(cboHolidayYear.Value);
            else
                entity.HolidayYear = null;
            entity.HolidayName = txtHolidayName.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = "";
            if (chkIsAnnual.Checked)
                FilterExpression = string.Format("HolidayDate = {0} AND HolidayMonth = {1} AND IsAnnualHoliday = 1 AND IsDeleted = 0", cboHolidayDate.Value, cboHolidayMonth.Value);
            else
                FilterExpression = string.Format("HolidayDate = {0} AND HolidayMonth = {1} AND IsAnnualHoliday = 0 AND HolidayYear = {2} AND IsDeleted = 0", cboHolidayDate.Value, cboHolidayMonth.Value, cboHolidayYear.Value);
            
            List<Holiday> lst = BusinessLayer.GetHolidayList(FilterExpression);
            if (lst.Count > 0)
                errMessage = " This Date is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = "";
            if (chkIsAnnual.Checked)
                FilterExpression = string.Format("HolidayDate = {0} AND HolidayMonth = {1} AND IsAnnualHoliday = 1 AND ID != {2} AND IsDeleted = 0", cboHolidayDate.Value, cboHolidayMonth.Value, hdnID.Value);
            else
                FilterExpression = string.Format("HolidayDate = {0} AND HolidayMonth = {1} AND IsAnnualHoliday = 0 AND HolidayYear = {2} AND ID != {3} AND IsDeleted = 0", cboHolidayDate.Value, cboHolidayMonth.Value, cboHolidayYear.Value, hdnID.Value);

            List<Holiday> lst = BusinessLayer.GetHolidayList(FilterExpression);
            if (lst.Count > 0)
                errMessage = " This Date is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = false;
            IDbContext ctx = DbFactory.Configure(true);
            HolidayDao entityDao = new HolidayDao(ctx);
            try
            {
                Holiday entity = new Holiday();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetHolidayMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
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
            try
            {
                Holiday entity = BusinessLayer.GetHoliday(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateHoliday(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        protected void cboHolidayDate_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (!chkIsAnnual.Checked && cboHolidayYear.Value != null)
            {
                cboHolidayDate.DataSource = Enumerable.Range(1, System.DateTime.DaysInMonth(Convert.ToInt16(cboHolidayYear.Value), Convert.ToInt16(cboHolidayMonth.Value)));
            }
            else
            {
                cboHolidayDate.DataSource = Enumerable.Range(1, System.DateTime.DaysInMonth(Convert.ToInt16(1900), Convert.ToInt16(cboHolidayMonth.Value)));
            }
            cboHolidayDate.EnableCallbackMode = false;
            cboHolidayDate.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboHolidayDate.DropDownStyle = DropDownStyle.DropDownList;
            cboHolidayDate.DataBind();
        }
    }
}