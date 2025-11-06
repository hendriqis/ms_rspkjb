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
    public partial class EKlaimSendOnlineCollective : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BPJS_EKLAIM_SEND_ONLINE_COLLECTIVE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnIsBridgingToEKlaim.Value = AppSession.IsBridgingToEKlaim ? "1" : "0";

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<Variable> lstServiceUnitType = new List<Variable>();
            lstServiceUnitType.Add(new Variable { Code = "1", Value = "Rawat Inap" });
            lstServiceUnitType.Add(new Variable { Code = "2", Value = "Rawat Jalan" });
            lstServiceUnitType.Add(new Variable { Code = "3", Value = "Rawat Inap & Rawat Jalan" });
            Methods.SetComboBoxField<Variable>(cboServiceUnitType, lstServiceUnitType, "Value", "Code");
            cboServiceUnitType.SelectedIndex = 2;

            List<Variable> lstDataType = new List<Variable>();
            lstDataType.Add(new Variable { Code = "1", Value = "Tanggal Pulang" });
            lstDataType.Add(new Variable { Code = "2", Value = "Tanggal Grouping" });
            Methods.SetComboBoxField<Variable>(cboDataType, lstDataType, "Value", "Code");
            cboDataType.SelectedIndex = 0;

            txtParameterDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtParameterDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string respEklaim = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            if (param[0] == "sendclaimcollective")
            {
                if (OnSendClaimCollective(ref  errMessage, ref  respEklaim))
                {
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

        private bool OnSendClaimCollective(ref string errMessage, ref string respEklaim)
        {
            bool result = true;

            try
            {
                string start_dt = txtParameterDateFrom.Text.Substring(10, 6) + "-" + txtParameterDateFrom.Text.Substring(5, 3) + "-" + txtParameterDateFrom.Text.Substring(2, 0);
                string stop_dt = txtParameterDateTo.Text.Substring(10, 6) + "-" + txtParameterDateTo.Text.Substring(5, 3) + "-" + txtParameterDateTo.Text.Substring(2, 0);
                string jenis_rawat = cboServiceUnitType.Value.ToString();
                string date_type = cboDataType.Value.ToString();

                SendClaimMethod sendClaim = new SendClaimMethod()
                {
                    metadata = new SendClaimMetadata()
                    {
                        method = "send_claim"
                    },
                    data = new SendClaimData()
                    {
                        start_dt = start_dt,
                        stop_dt = stop_dt,
                        jenis_rawat = jenis_rawat,
                        date_type = date_type
                    }
                };
                string jsonRequest = JsonConvert.SerializeObject(sendClaim);
                EKlaimService eklaimService = new EKlaimService();

                string response = eklaimService.SendClaim(jsonRequest);
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

    }
}