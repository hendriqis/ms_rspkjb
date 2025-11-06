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
    public partial class UserLocationEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnUserID.Value = param;
            User entity = BusinessLayer.GetUser(Convert.ToInt32(hdnUserID.Value));
            txtUserName.Text = entity.UserName;

            List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("");
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, lstHealthcare, "HealthcareName", "HealthcareID");
            cboHealthcare.SelectedIndex = 0;

            List<Int32> lstLocationID = BusinessLayer.GetLocationUserLocationIDList(string.Format("UserID = {0} AND IsDeleted = 0", hdnUserID.Value));
            hdnOldSelectedLocation.Value = hdnSelectedLocation.Value = String.Join(",", lstLocationID.ToArray());

            int count = BusinessLayer.GetLocationUserRoleRowCount(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE HealthcareID = '{0}' AND UserID = {1}) AND IsDeleted = 0", cboHealthcare.Value, hdnUserID.Value));
            hdnIsLocationUserRoleEmpty.Value = count > 0 ? "0" : "1";

            BindGridView(1, true, ref PageCount);
        }

        
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("HealthcareID = '{0}' AND IsHeader = 0 AND IsDeleted = 0", cboHealthcare.Value);

            filterExpression += string.Format(" AND LocationID NOT IN (SELECT ISNULL(a.LocationID,0) FROM vLocationFromIM0008 a)");

            if (hdnIsLocationUserRoleEmpty.Value == "0")
                filterExpression += string.Format(" AND LocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE HealthcareID = '{0}' AND UserID = {1}) AND IsDeleted = 0)", cboHealthcare.Value, hdnUserID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvLocationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            lstSelectedLocation = hdnSelectedLocation.Value.Split(',');

            List<vLocation> lstEntity = BusinessLayer.GetvLocationList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "LocationCode");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private string[] lstSelectedLocation = null;
        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vLocation entity = e.Row.DataItem as vLocation;

                CheckBox chkLocation = (CheckBox)e.Row.FindControl("chkLocation");
                if (lstSelectedLocation.Contains(entity.LocationID.ToString()))
                    chkLocation.Checked = true;
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
            List<string> lstOldLocation = new List<string>(hdnOldSelectedLocation.Value.Split(','));
            List<string> listSelectedLocation = new List<string>(hdnSelectedLocation.Value.Split(','));

            int roleID = Convert.ToInt32(hdnUserID.Value);

            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            LocationUserDao entityDao = new LocationUserDao(ctx);
            try
            {
                foreach (String oldData in lstOldLocation)
                {
                    if (!listSelectedLocation.Contains(oldData))
                    {
                        LocationUser entity = BusinessLayer.GetLocationUserList(string.Format("UserID = {0} AND LocationID = {1} AND IsDeleted = 0", roleID, oldData), ctx)[0];
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
                    }
                }
                foreach (String newData in listSelectedLocation)
                {
                    if (newData != "")
                    {
                        if (!lstOldLocation.Contains(newData))
                        {
                            LocationUser entity = new LocationUser();
                            entity.UserID = roleID;
                            entity.LocationID = Convert.ToInt32(newData);
                            entity.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entity);
                        }
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