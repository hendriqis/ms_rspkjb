using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Lsboratory.Program.LaboratoryOrder
{
    public partial class TestPartnerCtl : BaseViewPopupCtl
    {

        private const string DEFAULT_GRDVIEW_FILTER = "TransactionID = '{0}' AND IsDeleted = 0";
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {           
            hdnIDCtl.Value = param;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = String.Format(DEFAULT_GRDVIEW_FILTER, Convert.ToInt32(hdnIDCtl.Value));
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vPatientChargesDt> lstEntity = BusinessLayer.GetvPatientChargesDtList(filterExpression, 8, pageIndex);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                result = "refresh|" + pageCount;
            }
            else if (param[0] == "save")
            {
                if (OnSaveEditRecord(ref errMessage))
                {
                    result += "success";
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
                    BindGridView(1, true, ref pageCount);
                    result += "|" + pageCount;
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PatientChargesDt entity)
        {
            if (!String.IsNullOrEmpty(hdnBusinessPartnerIDctl.Value))
            {
                entity.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerIDctl.Value);
            }
            else 
            {
                entity.BusinessPartnerID = null;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao entityDao = new PatientChargesDtDao(ctx);
            try
            {
                PatientChargesDt entity = entityDao.Get(Convert.ToInt32(dtTransactionIdCtl.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityDao.Update(entity);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
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