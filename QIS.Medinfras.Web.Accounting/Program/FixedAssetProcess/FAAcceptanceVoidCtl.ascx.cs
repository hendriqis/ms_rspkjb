using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAAcceptanceVoidCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            List<StandardCode> lstVoidReason = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON));
            Methods.SetComboBoxField<StandardCode>(cboVoidReason, lstVoidReason, "StandardCodeName", "StandardCodeID");
            cboVoidReason.SelectedIndex = 0;
        }

        protected void cbpFAAcceptanceVoid_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnVoidRecord(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            FAAcceptanceHdDao entityDao = new FAAcceptanceHdDao(ctx);
            FAAcceptanceDtDao directPurchaseDtDao = new FAAcceptanceDtDao(ctx);

            try
            {
                    string filterExpression = string.Format("FAAcceptanceID = {0} AND IsDeleted = 0", hdnParam.Value);

                    FAAcceptanceHd entity = entityDao.Get(Convert.ToInt32(hdnParam.Value));

                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        List<FAAcceptanceDt> lstEntity = BusinessLayer.GetFAAcceptanceDtList(string.Format("FAAcceptanceID = {0} AND IsDeleted = 0", hdnParam.Value, ctx));
                        foreach (FAAcceptanceDt entityDt in lstEntity)
                        {
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            directPurchaseDtDao.Update(entityDt);
                        }
                    }

                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.GCVoidReason = cboVoidReason.Value.ToString();
                    if (cboVoidReason.Value.ToString() == Constant.DeleteReason.OTHER)
                    {
                        entity.VoidReason = txtReason.Text;
                    }
                    entity.VoidBy = AppSession.UserLogin.UserID;
                    entity.VoidDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                    entityDao.Update(entity);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
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