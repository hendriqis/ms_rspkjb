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
    public partial class UserServiceUnitEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnUserID.Value = param;
            User entity = BusinessLayer.GetUser(Convert.ToInt32(hdnUserID.Value));
            txtUserName.Text = entity.UserName;

            List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList(string.Format("HealthcareID IN (SELECT HealthcareID FROM vUserInRole WHERE UserID = {0})", hdnUserID.Value));
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, lstHealthcare, "HealthcareName", "HealthcareID");
            cboHealthcare.SelectedIndex = 0;

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsActive = 1");
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;

            List<Int32> lstHealthcareServiceUnitID = BusinessLayer.GetServiceUnitUserHealthcareServiceUnitIDList(string.Format("UserID = {0} AND IsDeleted = 0", hdnUserID.Value));
            hdnOldSelectedServiceUnit.Value = hdnSelectedServiceUnit.Value = String.Join(",", lstHealthcareServiceUnitID.ToArray());

            BindGridView(1, true, ref PageCount);
        }

        
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<GetServiceUnitUserRoleList> lstEntity = BusinessLayer.GetServiceUnitUserRoleList(cboHealthcare.Value.ToString(), Convert.ToInt32(hdnUserID.Value), cboDepartment.Value.ToString(), "");
            if (isCountPageCount)
            {
                int rowCount = lstEntity.Count;
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            lstSelectedServiceUnit = hdnSelectedServiceUnit.Value.Split(',');
            List<GetServiceUnitUserRoleList> lst = lstEntity.Skip((pageIndex - 1) * Constant.GridViewPageSize.GRID_MATRIX).Take(10).ToList();
            grdView.DataSource = lst;
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
                GetServiceUnitUserRoleList entity = e.Row.DataItem as GetServiceUnitUserRoleList;

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
            List<string> listOldSelectedServiceUnit = new List<string>(hdnOldSelectedServiceUnit.Value.Split(','));
            List<string> listSelectedServiceUnit = new List<string>(hdnSelectedServiceUnit.Value.Split(','));

            int userID = Convert.ToInt32(hdnUserID.Value);

            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceUnitUserDao entityDao = new ServiceUnitUserDao(ctx);
            try
            {
                foreach (string oldData in listOldSelectedServiceUnit)
                {
                    if (oldData != "" && !listSelectedServiceUnit.Contains(oldData))
                    {
                        ServiceUnitUser entity = BusinessLayer.GetServiceUnitUserList(string.Format("UserID = {0} AND HealthcareServiceUnitID = {1} AND IsDeleted = 0", userID, oldData), ctx)[0];
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
                    }
                }
                foreach (String newData in listSelectedServiceUnit)
                {
                    if (newData != "" && !listOldSelectedServiceUnit.Contains(newData))
                    {
                        ServiceUnitUser entity = new ServiceUnitUser();
                        entity.UserID = userID;
                        entity.HealthcareServiceUnitID = Convert.ToInt32(newData);
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
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