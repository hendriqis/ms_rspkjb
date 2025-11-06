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
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingFeeProcessEntry : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_FEE_PROCESS;
        }

        protected string OnGetParamedicFilterExpression()
        {
            return string.Format("IsDeleted = 0 AND IsHasRevenueSharing = 1");
        }

        protected override void InitializeDataControl()
        {
            hdnIsEditable.Value = "1";
            txtDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            cboStatus.Items.Add("SEMUA");
            cboStatus.Items.Add("OPEN");
            cboStatus.Items.Add("APPROVE");
            cboStatus.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);
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

        int paramParamedic = 0;
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (hdnParamedicID.Value != "")
            {
                paramParamedic = Convert.ToInt32(hdnParamedicID.Value);
            }

            string param = "";
            if (Convert.ToString(cboStatus.Value) == "OPEN")
            {
                param = "OPEN";
            }
            else if (Convert.ToString(cboStatus.Value) == "APPROVE")
            {
                param = "APPROVE";
            }
            else
            {
                param = "SEMUA";
            }

            List<GetProcessFeeRevenueSharing> lstentity = BusinessLayer.GetProcessFeeRevenueSharing(Helper.GetDatePickerValue(txtDateFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtDateTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112), paramParamedic, param);
            lvwView.DataSource = lstentity;
            lvwView.DataBind();
            hdnIsEditable.Value = "1";
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            if (e.Parameter == "approve")
            {
                if (OnApproveRecord(ref errMessage))
                    result = "success";
                else
                    result = "fail|" + errMessage;
            }
            else if (e.Parameter == "void")
            {
                if (OnVoidRecord(ref errMessage))
                    result = "success";
                else
                    result = "fail|" + errMessage;
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnApproveRecord(ref string errMessage)
        {
            bool result = false;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                String status = "approve";
                List<GetProcessFeeRevenueSharingParamedic> lstentity = BusinessLayer.GetProcessFeeRevenueSharingParamedic(Helper.GetDatePickerValue(txtDateFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtDateTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112), hdnSelectedMemberValue.Value, Convert.ToString(AppSession.UserLogin.UserID), status);

                result = true;
                ctx.CommitTransaction();
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
            return result;
        }

        private bool OnVoidRecord(ref string errMessage)
        {
            bool result = false;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                String status = "void";
                List<GetProcessFeeRevenueSharingParamedic> lstentity = BusinessLayer.GetProcessFeeRevenueSharingParamedic(Helper.GetDatePickerValue(txtDateFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtDateTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112), hdnSelectedMemberValue.Value, Convert.ToString(AppSession.UserLogin.UserID), status);

                result = true;
                ctx.CommitTransaction();
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
            return result;
        }
    }
}