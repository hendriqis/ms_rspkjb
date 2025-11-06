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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARProportionalCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] lstParam = param.Split('|');

                hdnARInvoiceIDCtl.Value = lstParam[0];
                txtARInvoiceNoCtl.Text = lstParam[1];

            } 
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "process")
            {
                result = "process|";
                if (OnProcessProportional(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnProcessProportional(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceHdDao entityHdDao = new ARInvoiceHdDao(ctx);

            try
            {
                ARInvoiceHd entity = entityHdDao.Get(Convert.ToInt32(hdnARInvoiceIDCtl.Value));

                bool processProportional = BusinessLayer.ProcessARProportional(entity.ARInvoiceID, AppSession.UserLogin.UserID, ctx);

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