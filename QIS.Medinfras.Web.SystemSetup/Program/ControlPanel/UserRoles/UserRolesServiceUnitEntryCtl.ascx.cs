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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class UserRolesServiceUnitEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnRoleID.Value = param;
            UserRole entity = BusinessLayer.GetUserRole(Convert.ToInt32(hdnRoleID.Value));
            txtRoleName.Text = entity.RoleName;

            List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("");
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, lstHealthcare, "HealthcareName", "HealthcareID");
            cboHealthcare.SelectedIndex = 0;

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsActive = 1");
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;

            List<Int32> lstServiceUnitID = BusinessLayer.GetServiceUnitUserRoleHealthcareServiceUnitIDList(string.Format("RoleID = {0} AND IsDeleted = 0", hdnRoleID.Value));
            hdnOldSelectedServiceUnit.Value = hdnSelectedServiceUnit.Value = String.Join(",", lstServiceUnitID.ToArray());

            BindGridView(1, true, ref PageCount);
        }


        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", cboHealthcare.Value, cboDepartment.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvHealthcareServiceUnitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }
            
            lstSelectedServiceUnit = hdnSelectedServiceUnit.Value.Split(',');

            List<vHealthcareServiceUnit> lstEntity = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "ServiceUnitName ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private string[] lstSelectedServiceUnit = null;
        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vHealthcareServiceUnit entity = e.Row.DataItem as vHealthcareServiceUnit;

                CheckBox chkServiceUnit = (CheckBox)e.Row.FindControl("chkServiceUnit");
                if (lstSelectedServiceUnit.Contains(entity.HealthcareServiceUnitID.ToString()))
                    chkServiceUnit.Checked = true;
            }
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
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

        private bool SaveData(ref string errMessage)
        {
            int roleID = Convert.ToInt32(hdnRoleID.Value);

            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceUnitUserRoleDao entityDao = new ServiceUnitUserRoleDao(ctx);
            try
            {
                if (hdnSelectedServiceUnit.Value != "")
                {
                    List<string> lstOldServiceUnit = new List<string>(hdnOldSelectedServiceUnit.Value.Split(','));
                    List<string> listSelectedServiceUnit = new List<string>(hdnSelectedServiceUnit.Value.Split(','));

                    foreach (String oldData in lstOldServiceUnit)
                    {
                        if (!listSelectedServiceUnit.Contains(oldData))
                        {
                            ServiceUnitUserRole entity = BusinessLayer.GetServiceUnitUserRoleList(string.Format("RoleID = {0} AND HealthcareServiceUnitID = {1} AND IsDeleted = 0", roleID, oldData), ctx)[0];
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
                        }
                    }
                    foreach (String newData in listSelectedServiceUnit)
                    {
                        if (!lstOldServiceUnit.Contains(newData))
                        {
                            ServiceUnitUserRole entity = new ServiceUnitUserRole();
                            entity.RoleID = roleID;
                            entity.HealthcareServiceUnitID = Convert.ToInt32(newData);
                            entity.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entity);
                        }
                    }
                }
                else
                {
                    List<string> lstOldServiceUnit = new List<string>(hdnOldSelectedServiceUnit.Value.Split(','));
                    foreach (String oldData in lstOldServiceUnit)
                    {
                            ServiceUnitUserRole entity = BusinessLayer.GetServiceUnitUserRoleList(string.Format("RoleID = {0} AND HealthcareServiceUnitID = {1} AND IsDeleted = 0", roleID, oldData), ctx)[0];
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
                    }

                    //ServiceUnitUserRole entity = BusinessLayer.GetServiceUnitUserRoleList(string.Format("RoleID = {0} AND IsDeleted = 0", roleID), ctx)[0];
                    //entity.IsDeleted = true;
                    //entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    //entityDao.Update(entity);
                }

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

        protected void cbpViewPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "save")
                {
                    string errMessage = "";
                    if (SaveData(ref errMessage))
                        result = "save|success";
                    else
                        result = "save|fail|" + errMessage;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}