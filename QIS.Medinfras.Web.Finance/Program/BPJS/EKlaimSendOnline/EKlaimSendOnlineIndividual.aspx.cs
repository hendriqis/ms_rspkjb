using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class EKlaimSendOnlineIndividual : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BPJS_EKLAIM_SEND_ONLINE_INDIVIDUAL;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnIsBridgingToEKlaim.Value = AppSession.IsBridgingToEKlaim ? "1" : "0";

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string respEklaim = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            if (param[0] == "sendclaimindividual")
            {
                if (OnSendClaimIndividual(ref  errMessage, ref  respEklaim))
                {
                    OnUpdateStatusLogBPJS("sendclaimindividual", ref   errMessage);
                    result += string.Format("success|{0}", respEklaim);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region EKLAIM SERVICE

        private bool OnSendClaimIndividual(ref string errMessage, ref string respEklaim)
        {
            bool result = true;

            try
            {
                string nomor_sep = txtNoSEP.Text;

                SendClaimIndividualMethod sendClaimInidividual = new SendClaimIndividualMethod()
                {
                    metadata = new SendClaimIndividualMetadata()
                    {
                        method = "send_claim_individual"
                    },
                    data = new SendClaimIndividualData()
                    {
                        nomor_sep = nomor_sep
                    }
                };
                string jsonRequest = JsonConvert.SerializeObject(sendClaimInidividual);
                EKlaimService eklaimService = new EKlaimService();

                string response = eklaimService.SendClaimIndividual(jsonRequest);
                respEklaim = response;
            }
            catch (Exception ex)
            {
                result = false;
                respEklaim = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        #endregion

        private bool OnUpdateStatusLogBPJS(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao entityDao = new RegistrationBPJSDao(ctx);
            try
            {
                string noSEP = txtNoSEP.Text;
                string filterRegBPJS = string.Format("NoSEP = '{0}'", noSEP);
                RegistrationBPJS entity = BusinessLayer.GetRegistrationBPJSList(filterRegBPJS, ctx).FirstOrDefault();
                if (entity != null)
                {
                    if (type == "sendclaimindividual")
                    {
                        entity.EKlaimSendOnlineBy = AppSession.UserLogin.UserID;
                        entity.EKlaimSendOnlineDateTime = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entity.LastUpdatedDate = DateTime.Now;
                        entityDao.Update(entity);
                        ctx.CommitTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Registrasi dari No SEP tersebut tidak ditemukan.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
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