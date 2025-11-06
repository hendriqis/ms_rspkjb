using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InhealthClaimEntry : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.INHEALTH_SIMPAN_TINDAKAN;
            //switch (hdnRequestID.Value)
            //{
                //case Constant.Facility.INPATIENT: return Constant.MenuCode.Finance.INHEALTH_SIMPAN_TINDAKAN_RAWAT_INAP;
                //case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Finance.INHEALTH_SIMPAN_TINDAKAN_RAWAT_JALAN;
                //default: return Constant.MenuCode.Finance.INHEALTH_SIMPAN_TINDAKAN_RAWAT_JALAN;
            //}
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
            {
                return GetLabel(menu.MenuCaption);
            }
            return "";
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
            IsAllowSave = false;
        }

        protected override void InitializeDataControl()
        {
            //if (Page.Request.QueryString["id"] != null)
            //{
            //    hdnRequestID.Value = Page.Request.QueryString["id"];
            //}
            //else
            //{
            //    hdnRequestID.Value = "ALL";
            //}

            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            txtSearchRegistrationDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtSearchRegistrationDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Variable> lstVariable = new List<Variable>();
            lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
            lstVariable.Add(new Variable { Code = "1", Value = "Belum Terkirim" });
            lstVariable.Add(new Variable { Code = "2", Value = "Terkirim" });
            lstVariable.Add(new Variable { Code = "3", Value = "Batal" });
            Methods.SetComboBoxField<Variable>(cboSentStatus, lstVariable, "Value", "Code");
            cboSentStatus.SelectedIndex = 1;

            BindGridView(1, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = string.Empty;

            filterExpression = string.Format("(RegistrationDate BETWEEN '{0}' AND '{1}')",
                                                        Helper.GetDatePickerValue(txtSearchRegistrationDateFrom.Text),
                                                        Helper.GetDatePickerValue(txtSearchRegistrationDateTo.Text));

            if (!string.IsNullOrEmpty(hdnRegistrationID.Value.ToString()))
            {
                if (filterExpression != "") {
                    filterExpression += string.Format(" AND ");
                }
                filterExpression += string.Format("(RegistrationID = '{0}' OR LinkedToRegistrationID = '{0}')", hdnRegistrationID.Value);
            }

            if (!string.IsNullOrEmpty(hdnItemID.Value))
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("ItemID = {0}", hdnItemID.Value);
            }

            if (cboSentStatus.Value != null)
            {
                if (filterExpression != "" && cboSentStatus.Value.ToString() != "0")
                {
                    filterExpression += " AND ";
                }

                if (cboSentStatus.Value.ToString() == "1")
                {
                    filterExpression += string.Format("IsSentToInhealth IS NULL");
                }
                else if (cboSentStatus.Value.ToString() == "2")
                {
                    filterExpression += string.Format("IsSentToInhealth = 1");
                }
                else if (cboSentStatus.Value.ToString() == "3")
                {
                    filterExpression += string.Format("IsSentToInhealth = 0");
                }
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesDtInhealthInformationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientChargesDtInhealthInformation> lstEntity = BusinessLayer.GetvPatientChargesDtInhealthInformationList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "RegistrationNo");
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
                else if (param[0] == "send") 
                {
                    result = param[0] + "|";
                    string lstID = param[1];
                    string resultService = OnSendToInhealth(param[0], lstID);
                    string[] resultServiceInfo = resultService.Split('|');
                    if (resultServiceInfo[0] == "1")
                    {
                        result += "success|" + resultServiceInfo[1];
                    }
                    else
                    {
                        result += "failed|" + resultServiceInfo[1];
                    }
                }
                else if (param[0] == "delete")
                {
                    result = param[0] + "|";
                    string lstID = param[1];
                    string resultService = OnSendToInhealth(param[0], lstID);
                    string[] resultServiceInfo = resultService.Split('|');
                    if (resultServiceInfo[0] == "1")
                    {
                        result += "success|" + resultServiceInfo[1];
                    }
                    else
                    {
                        result += "failed|" + resultServiceInfo[1];
                    }
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

        private string OnSendToInhealth(string type, string lstID = "")
        {
            string result = string.Empty;
            string filterExpression = GetFilterExpression();
            InhealthService oService = new InhealthService();

            switch (type)
            {
                case "send" :
                    result = oService.SimpanTindakanByFilter_API(
                        hdnRegistrationID.Value.ToString(),
                        string.Format("{0};{1}", Helper.GetDatePickerValue(txtSearchRegistrationDateFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtSearchRegistrationDateTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112)),
                        lstID,
                        hdnItemID.Value.ToString(),
                        cboSentStatus.Value.ToString(),
                        AppSession.UserLogin.UserID.ToString());
                    break;
                case "delete" :
                    result = oService.HapusTindakan_API(
                        hdnRegistrationID.Value.ToString(),
                        string.Format("{0};{1}", Helper.GetDatePickerValue(txtSearchRegistrationDateFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtSearchRegistrationDateTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112)),
                        lstID,
                        hdnItemID.Value.ToString(),
                        cboSentStatus.Value.ToString(),
                        AppSession.UserLogin.UserID.ToString(),
                        txtCancelReason.Text);
                    break;
            }

            return result;
        }
    }
}