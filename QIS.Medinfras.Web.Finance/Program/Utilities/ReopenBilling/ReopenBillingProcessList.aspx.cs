using System;
using System.Collections.Generic;
using System.Data;
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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ReopenBillingProcessList : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.FN_REOPEN_BILLING_PROCESS;
        }

        private GetUserMenuAccess menu;
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }
        
        protected override void InitializeDataControl()
        {
            txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsActive = 1 AND IsHasRegistration = 1"));
            lstDept = lstDept.OrderBy(lst => lst.TabOrder).ToList();
            Methods.SetComboBoxField<Department>(cboDepartment, lstDept, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;
            
            BindGridView();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowVoid = IsAllowNextPrev = false;
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("GCRegistrationStatus = '{0}' AND GCVisitStatus = '{0}' AND IsBillingClosed = 1 AND IsBillingReopen = 0", Constant.VisitStatus.CLOSED);

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += string.Format("RegistrationDate = '{0}'", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += string.Format("DepartmentID = '{0}'", cboDepartment.Value.ToString());

            if (hdnFilterHealthcareServiceUnitID.Value != "0" && hdnFilterHealthcareServiceUnitID.Value != "")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("HealthcareServiceUnitID = {0}", hdnFilterHealthcareServiceUnitID.Value);
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("{0}", hdnFilterExpressionQuickSearch.Value);
            }

            List<vRegistration9> lstEntity = BusinessLayer.GetvRegistration9List(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
                result = "refresh|";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "reopenbilling")
            {
                bool result = true;

                IDbContext ctx = DbFactory.Configure(true);
                RegistrationDao entityDao = new RegistrationDao(ctx);
                try
                {
                    string filterExpression = string.Format("RegistrationID IN ({0})", hdnSelectedRegistrationID.Value.Substring(1));
                    List<Registration> lstRegistration = BusinessLayer.GetRegistrationList(filterExpression, ctx);

                    if (lstRegistration.Count > 0)
                    {
                        string lstRegistrationID = "";
                        foreach (Registration registration in lstRegistration)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            registration.IsBillingClosed = false;
                            registration.BillingClosedBy = null;
                            registration.BillingClosedDate = null;
                            registration.IsBillingReopen = true;
                            registration.BillingReopenBy = AppSession.UserLogin.UserID;
                            registration.BillingReopenDate = DateTime.Now;
                            registration.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(registration);

                            if (lstRegistrationID != "")
                            {
                                lstRegistrationID += ",";
                            }
                            lstRegistrationID += registration.RegistrationID.ToString();
                        }

                    }
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
            return false;
        }
    }
}