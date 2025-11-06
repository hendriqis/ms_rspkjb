using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MultiVisitScheduleOrderEditCtl : BaseViewPopupCtl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void InitializeDataControl(string param)
        {
            hdnTestOrderID.Value = param;       
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0 AND GCTestOrderStatus NOT IN ('{1}') and ItemID IN (SELECT DISTINCT ItemID FROM DiagnosticVisitSchedule WITH(NOLOCK) WHERE TestOrderID = {0} and GCDiagnosticScheduleStatus = '{2}')", hdnTestOrderID.Value, Constant.TestOrderStatus.CANCELLED, Constant.DiagnosticVisitScheduleStatus.OPEN);
            List<vTestOrderDt> lstEntity = BusinessLayer.GetvTestOrderDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwViewPrint_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderDt entity = e.Row.DataItem as vTestOrderDt;
                TextBox txtScheduleStartDate = e.Row.FindControl("txtScheduleStartDate") as TextBox;
                TextBox txtScheduleEndDate = e.Row.FindControl("txtScheduleEndDate") as TextBox;
                txtScheduleStartDate.Text = entity.StartDateSchedule.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtScheduleEndDate.Text = entity.EndDateSchedule.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "update")
                {
                    result = "update|";
                    if (OnUpdateTestOrderDt(param[1]))
                    {
                        result += "success";
                    }
                    else
                    {
                        result += "fail";
                    }
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnUpdateTestOrderDt(string param)
        {
            bool result = true;

            string[] lstParam = param.Split('&');

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);

            try
            {
                for (int i = 0; i < lstParam.Length; i++)
                {
                    if (lstParam[i].Contains('~'))
                    {
                        string[] paramDetail = lstParam[i].Split('~');
                        int orderDtId = Convert.ToInt32(paramDetail[0]);
                        DateTime endDate = Helper.GetDatePickerValue(paramDetail[1]);

                        TestOrderDt orderDt = BusinessLayer.GetTestOrderDt(orderDtId);
                        orderDt.EndDateSchedule = endDate;
                        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        orderDt.LastUpdatedDate = DateTime.Now;

                        entityDtDao.Update(orderDt);
                    }
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                ctx.RollBackTransaction();
                Helper.InsertErrorLog(ex);
            }
            finally
            {
                ctx.Close();
            }


            return result;
        }
    }
}