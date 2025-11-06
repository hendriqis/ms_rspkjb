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
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientFluidBalanceList1 : BasePage
    {
        protected int PageCount = 1;
        protected int PageCount2 = 1;
        string menuType = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnRegistrationIDCBCtl.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitIDCBCtl.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnMRNCBCtl.Value = AppSession.RegisteredPatient.MRN.ToString();

            txtFromDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            txtFromTime.Text = "06:00";
            txtToTime.Text = "06:00";

            BindGridView(1, true, ref PageCount);
            BindGridView6(1, true, ref PageCount2);

        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpressionCBCtl.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("(RegistrationID = {0} OR LinkedToRegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFluidBalanceSummary1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vFluidBalanceSummary1> lstEntity = BusinessLayer.GetvFluidBalanceSummary1List(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "LogDate DESC");
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

        protected void cbpView6_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView6(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView6(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt1(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt1(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt3_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt3(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt3(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt2(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt2(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt4_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt4(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt4(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt5_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt5(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt5(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Detail List
        private void BindGridViewDt1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("(RegistrationID = {0} OR LinkedToRegistrationID = {0}) AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND IsInitializeIntake = 1 AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, hdnLogDateCBCtl.Value, Constant.FluidBalanceGroup.Intake);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFluidBalance1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFluidBalance1> lstEntity = BusinessLayer.GetvFluidBalance1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime DESC");

            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind(); 
        }

        private void BindGridViewDt4(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("(RegistrationID = {0} OR LinkedToRegistrationID = {0}) AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND FluidName = '{3}' AND IsInitializeIntake = 0 AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, hdnLogDateCBCtl.Value, Constant.FluidBalanceGroup.Intake, hdnFluidNameCBCtl.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFluidBalance1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFluidBalance1> lstEntity = BusinessLayer.GetvFluidBalance1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime DESC");

            grdViewDt4.DataSource = lstEntity;
            grdViewDt4.DataBind();
        }

        private void BindGridViewDt2(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("(RegistrationID = {0} OR LinkedToRegistrationID = {0}) AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, hdnLogDateCBCtl.Value, Constant.FluidBalanceGroup.Output);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFluidBalance1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFluidBalance1> lstEntity = BusinessLayer.GetvFluidBalance1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime DESC");

            grdViewDt2.DataSource = lstEntity;
            grdViewDt2.DataBind(); 
        }

        private void BindGridViewDt3(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("(RegistrationID = {0} OR LinkedToRegistrationID = {0}) AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, hdnLogDateCBCtl.Value, Constant.FluidBalanceGroup.Output_Tidak_Diukur);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFluidBalance1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFluidBalance1> lstEntity = BusinessLayer.GetvFluidBalance1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime ASC");

            grdViewDt3.DataSource = lstEntity;
            grdViewDt3.DataBind();
        }

        private void BindGridViewDt5(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("(RegistrationID = {0} OR LinkedToRegistrationID = {0}) AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, hdnLogDateCBCtl.Value, Constant.FluidBalanceGroup.Intake_Tidak_Diukur);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFluidBalance1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFluidBalance1> lstEntity = BusinessLayer.GetvFluidBalance1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime DESC");

            grdViewDt5.DataSource = lstEntity;
            grdViewDt5.DataBind();
        }

        private void BindGridView6(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpressionCBCtl.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("(RegistrationID = {0} OR LinkedToRegistrationID = {0}) AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvIVTheraphyNote1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vIVTheraphyNote1> lstEntity = BusinessLayer.GetvIVTheraphyNote1List(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "IVTherapyNoteDate DESC");
            grdView6.DataSource = lstEntity;
            grdView6.DataBind();
        }
        #endregion

        protected void cbpCalculateBalanceSummary_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string value = "0;0;0;0";
            string[] paramInfo = e.Parameter.Split('|');
            try
            {
                DateTime fromDateTime = DateTime.ParseExact(string.Format("{0} {1}", Helper.GetDatePickerValue(paramInfo[0]).ToString(Constant.FormatString.DATE_FORMAT_112), paramInfo[2]), Constant.FormatString.DATE_TIME_FORMAT_3, CultureInfo.InvariantCulture, DateTimeStyles.None);
                DateTime toDateTime = DateTime.ParseExact(string.Format("{0} {1}", Helper.GetDatePickerValue(paramInfo[1]).ToString(Constant.FormatString.DATE_FORMAT_112), paramInfo[3]), Constant.FormatString.DATE_TIME_FORMAT_3, CultureInfo.InvariantCulture, DateTimeStyles.None);
                string filterExpression = string.Format("VisitID = {0} AND CONVERT(DATETIME, CONVERT(VARCHAR(8), LogDate,112) + ' '+ LogTime) BETWEEN '{1}' AND '{2}' AND IsInitializeIntake = 0 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, 
                    fromDateTime,
                    toDateTime);

                List<vFluidBalance> lstEntity = BusinessLayer.GetvFluidBalanceList(filterExpression);
                decimal totalIntake1 = 0;
                decimal totalIntake2 = 0;
                decimal totalOutput1 = 0;
                decimal totalOutput2 = 0;

                foreach (vFluidBalance item in lstEntity)
                {
                    if (item.GCFluidGroup == Constant.FluidBalanceGroup.Intake)
                    {
                        totalIntake1 += item.FluidAmount;
                    }
                    else if (item.GCFluidGroup == Constant.FluidBalanceGroup.Intake_Tidak_Diukur)
                    {
                        totalIntake2 += item.Frequency;
                    }
                    else if (item.GCFluidGroup == Constant.FluidBalanceGroup.Output)
                    {
                        totalOutput1 += item.FluidAmount;
                    }
                    else
                    {
                        totalOutput2 += item.Frequency;
                    }
                }
                value = string.Format("{0};{1};{2};{3}", totalIntake1, totalIntake2, totalOutput1, totalOutput2);
                result += string.Format("success|{0}", value);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}