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
    public partial class GLRevenueItemGroupServiceUnitbyClassbyParamedicDetailCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.EMPLOYMENT_STATUS));

            Methods.SetComboBoxField<StandardCode>(cboEmploymentStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.EMPLOYMENT_STATUS).ToList(), "StandardCodeName", "StandardCodeID");

            BindGridView();
        }

        protected string GetTariffComponent1Text()
        {
            return hdnComp1Text.Value;
        }

        protected string GetTariffComponent2Text()
        {
            return hdnComp2Text.Value;
        }

        protected string GetTariffComponent3Text()
        {
            return hdnComp3Text.Value;
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("IsDeleted = 0");

            List<vGLRevenueItemGroupServiceUnitbyClassbyParamedic> lst = BusinessLayer.GetvGLRevenueItemGroupServiceUnitbyClassbyParamedicList(filterExpression);
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
            GLRevenueItemGroupServiceUnitByClassByParamedicDao entityDao = new GLRevenueItemGroupServiceUnitByClassByParamedicDao(ctx);
            try
            {
                GLRevenueItemGroupServiceUnitByClassByParamedic entity = new GLRevenueItemGroupServiceUnitByClassByParamedic();
                
                entity.ItemGroupID = Convert.ToInt32(hdnItemGroupID.Value);
                entity.ClassID = Convert.ToInt32(hdnClassID.Value);
                entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entity.GCEmployeeStatus = cboEmploymentStatus.Value.ToString();

                entity.RevenueGLAccountID1 = Convert.ToInt32(hdnGLAccount1ID.Value);
                entity.RevenueGLAccountID2 = Convert.ToInt32(hdnGLAccount2ID.Value);
                entity.RevenueGLAccountID3 = Convert.ToInt32(hdnGLAccount3ID.Value);

                entity.DiscountGLAccountID1 = Convert.ToInt32(hdnDiscountGLAccount1ID.Value);
                entity.DiscountGLAccountID2 = Convert.ToInt32(hdnDiscountGLAccount2ID.Value);
                entity.DiscountGLAccountID3 = Convert.ToInt32(hdnDiscountGLAccount3ID.Value);


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
            GLRevenueItemGroupServiceUnitByClassByParamedicDao entityDao = new GLRevenueItemGroupServiceUnitByClassByParamedicDao(ctx);
            try
            {
                GLRevenueItemGroupServiceUnitByClassByParamedic entity = entityDao.Get(Convert.ToInt32(hdnIDCtl.Value));
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