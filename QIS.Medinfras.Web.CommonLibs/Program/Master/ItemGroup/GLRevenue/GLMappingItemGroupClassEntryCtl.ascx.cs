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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class GLMappingItemGroupClassEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnItemGroupIDCtlCOA.Value = param;

            ItemGroupMaster entity = BusinessLayer.GetItemGroupMaster(Convert.ToInt32(hdnItemGroupIDCtlCOA.Value));
            txtItemGroupCodeCtl.Text = entity.ItemGroupCode;
            txtItemGroupNameCtl.Text = entity.ItemGroupName1;

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("IsDeleted = 0");

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += string.Format("ItemGroupID = {0}", hdnItemGroupIDCtlCOA.Value);

            List<vGLRevenueItemGroupClass> lst = BusinessLayer.GetvGLRevenueItemGroupClassList(filterExpression);
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
            GLRevenueItemGroupClassDao entityDao = new GLRevenueItemGroupClassDao(ctx);
            try
            {
                GLRevenueItemGroupClass entity = new GLRevenueItemGroupClass();

                entity.ItemGroupID = Convert.ToInt32(hdnItemGroupIDCtlCOA.Value);
                entity.ClassID = Convert.ToInt32(hdnClassID.Value);

                if (hdnRevenueGLAccountID1.Value != "" && hdnRevenueGLAccountID1.Value != "0")
                {
                    entity.RevenueGLAccountID1 = Convert.ToInt32(hdnRevenueGLAccountID1.Value);
                }
                if (hdnRevenueGLAccountID2.Value != "" && hdnRevenueGLAccountID2.Value != "0")
                {
                    entity.RevenueGLAccountID2 = Convert.ToInt32(hdnRevenueGLAccountID2.Value);
                }
                if (hdnRevenueGLAccountID3.Value != "" && hdnRevenueGLAccountID3.Value != "0")
                {
                    entity.RevenueGLAccountID3 = Convert.ToInt32(hdnRevenueGLAccountID3.Value);
                }

                if (hdnDiscountGLAccountID1.Value != "" && hdnDiscountGLAccountID1.Value != "0")
                {
                    entity.DiscountGLAccountID1 = Convert.ToInt32(hdnDiscountGLAccountID1.Value);
                }
                if (hdnDiscountGLAccountID2.Value != "" && hdnDiscountGLAccountID2.Value != "0")
                {
                    entity.DiscountGLAccountID2 = Convert.ToInt32(hdnDiscountGLAccountID2.Value);
                }
                if (hdnDiscountGLAccountID3.Value != "" && hdnDiscountGLAccountID3.Value != "0")
                {
                    entity.DiscountGLAccountID3 = Convert.ToInt32(hdnDiscountGLAccountID3.Value);
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
            GLRevenueItemGroupClassDao entityDao = new GLRevenueItemGroupClassDao(ctx);
            try
            {
                GLRevenueItemGroupClass entity = entityDao.Get(Convert.ToInt32(hdnIDCtl.Value));
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