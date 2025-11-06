using System;
using System.Collections.Generic;
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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARInvoicePayerUpdateDocumentRecieveDate : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnARInvoiceID.Value = hdnParam.Value.Split('|')[0];

            ARInvoiceHd arHD = BusinessLayer.GetARInvoiceHd(Convert.ToInt32(hdnARInvoiceID.Value));
            EntityToControl(arHD);
        }

        protected void cbpUpdateARDocRecieveDatePayer_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnSaveRecord(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void EntityToControl(ARInvoiceHd arHD)
        {
            txtARDocumentReceiveDate.Text = arHD.ARDocumentReceiveDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        private bool OnSaveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceHdDao entityDao = new ARInvoiceHdDao(ctx);
            TermDao termDao = new TermDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                ARInvoiceHd arHD = BusinessLayer.GetARInvoiceHd(Convert.ToInt32(hdnARInvoiceID.Value));
                TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.AR_INVOICE_PAYER);
                if (entityLock.LockedUntilDate != null)
                {
                    DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                    DateTime DateNow = arHD.ARInvoiceDate;
                    if (DateNow > DateLock)
                    {
                        arHD.ARDocumentReceiveDate = Helper.GetDatePickerValue(txtARDocumentReceiveDate);

                        Term term = termDao.Get(arHD.TermID);
                        SettingParameterDt leadTime = BusinessLayer.GetSettingParameterDt("001", Constant.SettingParameter.FN_AR_LEAD_TIME);
                        DateTime tempdue1 = arHD.ARDocumentReceiveDate.AddDays(term.TermDay);
                        DateTime tempdue2 = tempdue1.AddDays(Convert.ToInt16(leadTime.ParameterValue));
                        arHD.DueDate = tempdue2;

                        entityDao.Update(arHD);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    arHD.ARDocumentReceiveDate = Helper.GetDatePickerValue(txtARDocumentReceiveDate);

                    Term term = termDao.Get(arHD.TermID);
                    SettingParameterDt leadTime = BusinessLayer.GetSettingParameterDt("001", Constant.SettingParameter.FN_AR_LEAD_TIME);
                    DateTime tempdue1 = arHD.ARDocumentReceiveDate.AddDays(term.TermDay);
                    DateTime tempdue2 = tempdue1.AddDays(Convert.ToInt16(leadTime.ParameterValue));
                    arHD.DueDate = tempdue2;

                    entityDao.Update(arHD);
                }
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