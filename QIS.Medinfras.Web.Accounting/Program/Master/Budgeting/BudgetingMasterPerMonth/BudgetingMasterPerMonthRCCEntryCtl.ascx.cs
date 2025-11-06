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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class BudgetingMasterPerMonthRCCEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnBudgetDtIDCtl.Value = param;

            BindGridView();
        }

        protected override void OnLoad(EventArgs e)
        {
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnBudgetDtIDCtl.Value != "")
            {
                filterExpression = string.Format("BudgetTransactionDtID = {0} AND IsDeleted = 0", hdnBudgetDtIDCtl.Value);
            }

            List<vBudgetTransactionDtRevenueCostCenter> lstEntity = BusinessLayer.GetvBudgetTransactionDtRevenueCostCenterList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            if (param[0] == "save")
            {
                if (hdnID.Value.ToString() != "" && hdnID.Value.ToString() != "0")
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
                if (OnSaveDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BudgetTransactionDtRevenueCostCenterDao entityDtRCCDao = new BudgetTransactionDtRevenueCostCenterDao(ctx);
            try
            {
                BudgetTransactionDtRevenueCostCenter entityDt = new BudgetTransactionDtRevenueCostCenter();
                entityDt.BudgetTransactionDtID = Convert.ToInt32(hdnBudgetDtIDCtl.Value);
                entityDt.DepartmentID = hdnDepartmentID.Value;
                entityDt.RevenueCostCenterID = Convert.ToInt32(hdnRCCID.Value);
                entityDt.BudgetAmount = Convert.ToDecimal(txtBudgetAmountCtl.Text);
                entityDt.Remarks = txtRemarksDtCtl.Text;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                entityDtRCCDao.Insert(entityDt);

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

        protected bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BudgetTransactionDtRevenueCostCenterDao entityDtDao = new BudgetTransactionDtRevenueCostCenterDao(ctx);
            try
            {
                if (hdnID.Value != null && hdnID.Value != "" && hdnID.Value != "0")
                {
                    BudgetTransactionDtRevenueCostCenter entityDt = entityDtDao.Get(Convert.ToInt32(hdnID.Value));
                    entityDt.RevenueCostCenterID = Convert.ToInt32(hdnRCCID.Value);
                    entityDt.DepartmentID = hdnDepartmentID.Value;
                    entityDt.BudgetAmount = Convert.ToDecimal(txtBudgetAmountCtl.Text);
                    entityDt.Remarks = txtRemarksDtCtl.Text;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Proses gagal, silahkan refresh ulang halaman.";
                    Exception ex = new Exception(errMessage);
                    ctx.RollBackTransaction();
                }
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

        protected bool OnSaveDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BudgetTransactionDtRevenueCostCenterDao entityDtDao = new BudgetTransactionDtRevenueCostCenterDao(ctx);
            try
            {
                if (hdnID.Value != null && hdnID.Value != "" && hdnID.Value != "0")
                {
                    BudgetTransactionDtRevenueCostCenter entityDt = entityDtDao.Get(Convert.ToInt32(hdnID.Value));
                    entityDt.IsDeleted = true;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Proses gagal, silahkan refresh ulang halaman.";
                    Exception ex = new Exception(errMessage);
                    ctx.RollBackTransaction();
                }
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