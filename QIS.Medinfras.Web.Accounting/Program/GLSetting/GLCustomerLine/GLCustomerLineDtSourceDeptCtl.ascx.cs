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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLCustomerLineDtSourceDeptCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnCustomerLineIDCtl.Value = param;

            CustomerLine entity = BusinessLayer.GetCustomerLineList(string.Format("CustomerLineID = {0}", param)).FirstOrDefault();
            txtCustomerLine.Text = string.Format("{0} - {1}", entity.CustomerLineCode, entity.CustomerLineName);
          
            BindGridView(1, true, ref PageCount);

        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("CustomerLineID = {0} AND IsDeleted = 0", hdnCustomerLineIDCtl.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvCustomerLineDtSourceDepartmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vCustomerLineDtSourceDepartment> lstEntity = BusinessLayer.GetvCustomerLineDtSourceDepartmentList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "SourceDepartmentID, ID");
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
                result = string.Format("refresh|{0}", pageCount);
            }
            else
            {
                if (param[0] == "save")
                {
                    if (hdnID.Value.ToString() != "")
                    {
                        if (OnSaveEditRecord(ref errMessage))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                    else
                    {
                        if (OnSaveAddRecord(ref errMessage))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                }
                else if (param[0] == "delete")
                {
                    if (OnDeleteRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }

                BindGridView(1, true, ref pageCount);
                result += "|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(CustomerLineDtSourceDepartment entity)
        {
            entity.SourceDepartmentID = hdnSourceDepartmentID.Value;

            if (hdnARID.Value != null && hdnARID.Value != "" && hdnARID.Value != "0")
            {
                entity.AR = Convert.ToInt32(hdnARID.Value);
            }
            else
            {
                entity.AR = null;
            }

            if (hdnARInProcessID.Value != null && hdnARInProcessID.Value != "" && hdnARInProcessID.Value != "0")
            {
                entity.ARInProcess = Convert.ToInt32(hdnARInProcessID.Value);
            }
            else
            {
                entity.ARInProcess = null;
            }

            if (hdnARInCareID.Value != null && hdnARInCareID.Value != "" && hdnARInCareID.Value != "0")
            {
                entity.ARInCare = Convert.ToInt32(hdnARInCareID.Value);
            }
            else
            {
                entity.ARInCare = null;
            }

            if (hdnARAdjustmentID.Value != null && hdnARAdjustmentID.Value != "" && hdnARAdjustmentID.Value != "0")
            {
                entity.ARAdjustment = Convert.ToInt32(hdnARAdjustmentID.Value);
            }
            else
            {
                entity.ARAdjustment = null;
            }

            if (hdnARDiscountID.Value != null && hdnARDiscountID.Value != "" && hdnARDiscountID.Value != "0")
            {
                entity.ARDiscount = Convert.ToInt32(hdnARDiscountID.Value);
            }
            else
            {
                entity.ARDiscount = null;
            }
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            CustomerLineDtSourceDepartmentDao entityDao = new CustomerLineDtSourceDepartmentDao(ctx);
            try
            {
                CustomerLineDtSourceDepartment entity = new CustomerLineDtSourceDepartment();
                ControlToEntity(entity);

                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.CustomerLineID = Convert.ToInt32(hdnCustomerLineIDCtl.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);

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

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            CustomerLineDtSourceDepartmentDao entityDao = new CustomerLineDtSourceDepartmentDao(ctx);
            try
            {
                CustomerLineDtSourceDepartment entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);

                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            CustomerLineDtSourceDepartmentDao entityDao = new CustomerLineDtSourceDepartmentDao(ctx);
            try
            {
                CustomerLineDtSourceDepartment entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
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
    }
}