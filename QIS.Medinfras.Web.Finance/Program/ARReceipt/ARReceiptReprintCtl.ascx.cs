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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARReceiptReprintCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnARReceiptIDCtl.Value = param;

            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                    AppSession.UserLogin.HealthcareID,
                                                    Constant.SettingParameter.FN_REPORT_CODE_AR_RECEIPT
                                                ));

            hdnReportCodeCtl.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORT_CODE_AR_RECEIPT).FirstOrDefault().ParameterValue;
            
            List<StandardCode> lstReprintReason = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.RECEIPT_REPRINT_REASON));
            Methods.SetComboBoxField<StandardCode>(cboReprintReason, lstReprintReason, "StandardCodeName", "StandardCodeID");
            cboReprintReason.SelectedIndex = 0;
        }

        protected void cbpARReceiptReprint_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnReprintRecord(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnReprintRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARReceiptHdDao entityHdDao = new ARReceiptHdDao(ctx);
            try
            {
                ARReceiptHd entity = entityHdDao.Get(Convert.ToInt32(hdnARReceiptIDCtl.Value));;
                entity.GCReprintReason = cboReprintReason.Value.ToString();
                if (txtReason.Text != "" && txtReason.Text != null)
                {
                    entity.ReprintReason = txtReason.Text;
                }
                entity.PrintNumber += 1;
                entity.LastPrintedDate = DateTime.Now;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityHdDao.Update(entity);

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