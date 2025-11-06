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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class UpdateReferenceInfoCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnPurchaseReceiveID.Value = hdnParam.Value.Split('|')[0];

            PurchaseReceiveHd prhd = BusinessLayer.GetPurchaseReceiveHd(Convert.ToInt32(hdnPurchaseReceiveID.Value));
            EntityToControl(prhd);
        }

        protected void cbpUpdateReferenceInfo_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        private void EntityToControl(PurchaseReceiveHd prhd)
        {
            txtReferenceNo.Text = prhd.ReferenceNo;
            txtReferenceDate.Text = Convert.ToDateTime(prhd.ReferenceDate).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        private bool OnSaveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReceiveHdDao entityDao = new PurchaseReceiveHdDao(ctx);
            TermDao termDao = new TermDao(ctx);
            try
            {
                PurchaseReceiveHd prhd = entityDao.Get(Convert.ToInt32(hdnPurchaseReceiveID.Value));
                prhd.ReferenceNo = txtReferenceNo.Text;
                prhd.ReferenceDate = Helper.GetDatePickerValue(txtReferenceDate.Text);

                int termDay = termDao.Get(prhd.TermID).TermDay;
                string setvar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_PROSES_DUE_DATE_FROM_PORDATE_OR_REFERENCE_DATE).ParameterValue;
                if (setvar == "2")
                {
                    prhd.PaymentDueDate = prhd.ReferenceDate.AddDays(termDay);
                }

                entityDao.Update(prhd);

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