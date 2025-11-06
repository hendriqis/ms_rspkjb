using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryPayReceiptVoidCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnRegistrationID.Value = hdnParam.Value.Split('|')[0];
            hdnPaymentReceiptID.Value = hdnParam.Value.Split('|')[1];
            hdnDepartmentID.Value = hdnParam.Value.Split('|')[2];
            List<StandardCode> lstVoidReason = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON));
            Methods.SetComboBoxField<StandardCode>(cboVoidReason, lstVoidReason, "StandardCodeName", "StandardCodeID");
            cboVoidReason.SelectedIndex = 0;
        }

        protected void cbpPatientPaymentReceiptVoid_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
            PaymentReceiptDao entityDao = new PaymentReceiptDao(ctx);
            PatientPaymentHdDao entityHdDao = new PatientPaymentHdDao(ctx);

            try
            {
                PaymentReceipt entity = BusinessLayer.GetPaymentReceiptList(string.Format("PaymentReceiptID = {0} AND IsDeleted = 0", hdnPaymentReceiptID.Value))[0];
                entity.GCVoidReason = cboVoidReason.Value.ToString();
                if (txtReason.Text != "" && txtReason.Text != null)
                    entity.VoidReason = txtReason.Text;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.IsDeleted = true;

                entityDao.Update(entity);

                List<PatientPaymentHd> entityHd = BusinessLayer.GetPatientPaymentHdList(string.Format("PaymentReceiptID = {0} AND IsDeleted=0", entity.PaymentReceiptID));
                foreach (PatientPaymentHd item in entityHd)
                {
                    item.PaymentReceiptID = null;
                    item.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(item);
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
    }
}