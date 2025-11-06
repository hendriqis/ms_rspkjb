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
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLMappingItemGroupServiceUnitEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnGLAccountIDCtl.Value = param;

            ChartOfAccount coa = BusinessLayer.GetChartOfAccount(Convert.ToInt32(hdnGLAccountIDCtl.Value));
            txtGLAccountNoCtl.Text = hdnGLAccountNoCtl.Value = coa.GLAccountNo;
            txtGLAccountNameCtl.Text = hdnGLAccountNameCtl.Value = coa.GLAccountName;
            hdnGLPositionCtl.Value = coa.Position;

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("IsDeleted = 0");

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }

            if (hdnGLPositionCtl.Value == "K")
            {
                filterExpression += string.Format("(RevenueGLAccountID1 = {0} OR RevenueGLAccountID2 = {0} OR RevenueGLAccountID3 = {0})", hdnGLAccountIDCtl.Value);
            }
            else
            {
                filterExpression += string.Format("(DiscountGLAccountNo1 = {0} OR DiscountGLAccountNo2 = {0} OR DiscountGLAccountNo3 = {0})", hdnGLAccountIDCtl.Value);
            }

            List<vGLRevenueItemGroupServiceUnit> lst = BusinessLayer.GetvGLRevenueItemGroupServiceUnitList(filterExpression);
            grdView.DataSource = lst;
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "refresh")
            {
                BindGridView();
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                {
                    result += "success";
                    BindGridView();
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "save")
            {
                if (OnSaveAddRecord(ref errMessage))
                {
                    result += "success";
                    BindGridView();
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLRevenueItemGroupServiceUnitDao entityDao = new GLRevenueItemGroupServiceUnitDao(ctx);
            try
            {
                GLRevenueItemGroupServiceUnit entity = new GLRevenueItemGroupServiceUnit();
                
                entity.ItemGroupID = Convert.ToInt32(hdnItemGroupID.Value);
                entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);

                entity.RevenueGLAccountID1 = Convert.ToInt32(hdnRevenueGLAccountID1.Value);
                entity.RevenueGLAccountID2 = Convert.ToInt32(hdnRevenueGLAccountID2.Value);
                entity.RevenueGLAccountID3 = Convert.ToInt32(hdnRevenueGLAccountID3.Value);

                entity.DiscountGLAccountID1 = Convert.ToInt32(hdnDiscountGLAccountID1.Value);
                entity.DiscountGLAccountID2 = Convert.ToInt32(hdnDiscountGLAccountID2.Value);
                entity.DiscountGLAccountID3 = Convert.ToInt32(hdnDiscountGLAccountID3.Value);

                if (!String.IsNullOrEmpty(hdnHPPGLAccountID.Value))
                {
                    entity.HPPRevenueSharingID2 = Convert.ToInt32(hdnHPPGLAccountID.Value);
                }

                entity.IsDeleted = false;
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLRevenueItemGroupServiceUnitDao entityDao = new GLRevenueItemGroupServiceUnitDao(ctx);
            try
            {
                GLRevenueItemGroupServiceUnit entity = entityDao.Get(Convert.ToInt32(hdnIDCtl.Value));
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