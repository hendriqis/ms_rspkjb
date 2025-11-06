using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using Newtonsoft.Json;
using QIS.Medinfras.Web.CommonLibs.Service;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class EpisodeTestOrderCtl1 : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnItemType.Value = paramInfo[0];
            hdnPopupVisitID.Value = paramInfo[1];

            List<ConsultVisit> lstCV = BusinessLayer.GetConsultVisitList(string.Format(
                                                "GCVisitStatus <> '{0}' AND RegistrationID IN (SELECT lfr.RegistrationID FROM Registration lfr WITH(NOLOCK) WHERE lfr.LinkedToRegistrationID = {1})",
                                                Constant.VisitStatus.CANCELLED, AppSession.RegisteredPatient.RegistrationID
                                            ));
            string lstVisitID = "";
            foreach (ConsultVisit visit in lstCV)
            {
                if (lstVisitID != "")
                {
                    lstVisitID += ", ";
                }
                lstVisitID += visit.VisitID;
            }
            hdnLinkedVisitID.Value = string.Format("({0})", lstVisitID);

            BindGridView();

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                    AppSession.UserLogin.HealthcareID, //0
                                                    Constant.SettingParameter.EM_IS_MEDICAL_RESUME_CAN_INSERT_LABORATORIUM_RESULT //1
                                                );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsResumeMedisCanInsertResultLab.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.EM_IS_MEDICAL_RESUME_CAN_INSERT_LABORATORIUM_RESULT).FirstOrDefault().ParameterValue;
        }

        private void BindGridView()
        {
            string transactionCode = string.Empty;
            switch (hdnItemType.Value)
            {
                case Constant.ItemType.LABORATORIUM:
                    transactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                    break;
                case Constant.ItemType.RADIOLOGI:
                    transactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                    break;
                default:
                    transactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                    break;
            }
            List<EpisodeDiagnosticSupportTestItem> lstDetail = BusinessLayer.GetEpisodeDiagnosticSupportTestItemList(Convert.ToInt32(hdnPopupVisitID.Value), transactionCode);
            grdView.DataSource = lstDetail;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            BindGridView();
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            string title = string.Empty;
            switch (hdnItemType.Value)
            {
                case Constant.ItemType.LABORATORIUM:
                    title = "Laboratorium :";
                    break;
                case Constant.ItemType.RADIOLOGI:
                    title = "Radiologi :";
                    break;
                default:
                    title = "Penunjang Medis Lainnya :";
                    break;
            }
            string medicationLineText = hdnSelectedItem.Value.Replace("|", Environment.NewLine);
            retval = string.Format("{0}{1}{2}", title, Environment.NewLine, medicationLineText);
            return result;
        }
    }
}