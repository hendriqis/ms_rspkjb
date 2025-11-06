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
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ServiceOrderRemarksCtl : BaseContentPopupCtl
    {
        protected string GCTemplateGroup = "";
        public override void InitializeControl(string param)
        {
            hdnServiceOrderID.Value = param;

            vServiceOrderHd entityHd = BusinessLayer.GetvServiceOrderHdList(string.Format("ServiceOrderID = {0}", hdnServiceOrderID.Value)).FirstOrDefault();
            EntityToControl(entityHd);
        }

        private void EntityToControl(vServiceOrderHd entity)
        {
            txtServiceOrderNo.Text = entity.ServiceOrderNo;
            txtParamedic.Text = entity.ParamedicName;
            txtServiceOrderDate.Text = entity.ServiceOrderDate.ToString(Constant.FormatString.DATE_FORMAT);
            txtServiceOrderTime.Text = entity.ServiceOrderTime;
            txtParamedic.Text = string.Format("{0} - {1}", entity.ParamedicName, entity.ParamedicCode);
            txtRemarks.Text = entity.Remarks;
        }

        protected void cbpEntryPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = "";
            string errMessage = "";

            if (OnProcessRecord(ref errMessage))
                result += "success";
            else
                result += "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnProcessRecord(ref string errMessage)
        {
            try
            {
                ServiceOrderHd serviceOrderHd = BusinessLayer.GetServiceOrderHd(Convert.ToInt32(hdnServiceOrderID.Value));
                serviceOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                serviceOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateServiceOrderHd(serviceOrderHd);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}