using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class FillNoAntrianCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {

            string[] paramInfo = param.Split('|');
            hdnTransactionIDCtl.Value = paramInfo[0];
            txtTransactionNo.Text = paramInfo[1];

            vPrescriptionOrderHd oPrescriptionOrderHD = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("ChargesTransactionID='{0}'", hdnTransactionIDCtl.Value)).FirstOrDefault();
            hdnPrescriptionOrderIDCtl.Value = oPrescriptionOrderHD.PrescriptionOrderID.ToString();

            if (!String.IsNullOrEmpty(oPrescriptionOrderHD.ReferenceNo))
            {
                string[] refNo = oPrescriptionOrderHD.ReferenceNo.Split('|');
                if (refNo.Length > 1)
                {
                    if (!string.IsNullOrEmpty(refNo[1]))
                    {
                        txtAntrianNo.Text = refNo[1];
                    }
                }
            }
        }

        protected void cbpGetAntrianProcessNonBridging_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            string result = "";
            Boolean isSuccess = false;
            String ErrMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "save")
                {
                    SaveNomorAntrian(ref  isSuccess, ref  ErrMessage);
                    if (isSuccess)
                    {
                        result = "save|success|";
                    }
                    else
                    {
                        result = "save|fail|" + ErrMessage;
                    }

                }
            }
            panel.JSProperties["cpResult"] = result;
        }

        public void SaveNomorAntrian(ref Boolean isSuccess, ref String ErrMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
            PatientChargesHdDao chargesHdDao = new PatientChargesHdDao(ctx);
            isSuccess = true;
            try
            {
                PatientChargesHd chargesHd = chargesHdDao.Get(Convert.ToInt32(hdnTransactionIDCtl.Value));
                if (chargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    PrescriptionOrderHd entityHd = orderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderIDCtl.Value));
                    string oldData = "";
                    if (!String.IsNullOrEmpty(entityHd.ReferenceNo))
                    {
                        string[] refNo = entityHd.ReferenceNo.Split('|');
                        oldData = refNo[0];
                    }
                    entityHd.ReferenceNo = string.Format("{0}|{1}", oldData, txtAntrianNo.Text);

                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    orderHdDao.Update(entityHd);
                    ctx.CommitTransaction();
                }
                else
                {
                    isSuccess = false;
                    ErrMessage = "Transaksi sudah diproses.";
                    Exception ex = new Exception(ErrMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                ErrMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
        }
    }
}