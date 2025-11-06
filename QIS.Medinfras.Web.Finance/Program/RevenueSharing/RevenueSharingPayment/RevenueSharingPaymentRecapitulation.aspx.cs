using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingPaymentRecapitulation : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_PAYMENT_RECAPITULATION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            
            txtPeriodeFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodeTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            hdnReportCode.Value = "FN-00100";
            hdnFileName.Value = string.Format("{0}_{1}_{2}.csv",
                                                BusinessLayer.GetReportMasterList("ReportCode = '" + hdnReportCode.Value + "'").FirstOrDefault().ClassName,
                                                DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112),
                                                DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Replace(":","_"));

            BindGridView();
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string oPaymentDate = string.Format("{0};{1}", Helper.GetDatePickerValue(txtPeriodeFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtPeriodeTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
            int oParamedicID = 0;
            if (hdnParamedicID.Value != "" && hdnParamedicID.Value != null)
            {
                oParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            }

            string oReportParameter = string.Format("{0},{1}", oPaymentDate, oParamedicID.ToString());

            List<GetTransRevenueSharingPaymentPerPeriodePerParamedic> lst = BusinessLayer.GetTransRevenueSharingPaymentPerPeriodePerParamedic(oReportParameter);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientPaymentDtInfoDao paymentDtInfoDao = new PatientPaymentDtInfoDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
            BPJSClaimDocumentTempDao claimDocumentTempDao = new BPJSClaimDocumentTempDao(ctx);
            PatientNotificationDao patientNotificationDao = new PatientNotificationDao(ctx);

            if (type == "download")
            {
                #region Download Document

                string reportCode = string.Format("ReportCode = '{0}'", hdnReportCode.Value);
                ReportMaster rm = BusinessLayer.GetReportMasterList(reportCode).FirstOrDefault();

                StringBuilder sbResult = new StringBuilder();

                #region Get Data Report

                List<dynamic> lstDynamic = null;
                List<Variable> lstVariable = new List<Variable>();

                string oPaymentDate = string.Format("{0};{1}", Helper.GetDatePickerValue(txtPeriodeFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtPeriodeTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
                string oParamedicID = "0";
                if (hdnParamedicID.Value != "" && hdnParamedicID.Value != null)
                {
                    oParamedicID = Convert.ToInt32(hdnParamedicID.Value).ToString();
                }

                string oReportParameter = string.Format("{0},{1}", oPaymentDate, oParamedicID.ToString());

                lstVariable.Add(new Variable { Code = "ReportParameter", Value = oReportParameter });

                lstDynamic = BusinessLayer.GetDataReport(rm.ObjectTypeName, lstVariable);

                #endregion

                dynamic fields = lstDynamic[0];

                foreach (var prop in fields.GetType().GetProperties())
                {
                    sbResult.Append(prop.Name);
                    sbResult.Append(",");
                }

                sbResult.Append("\r\n");

                for (int i = 0; i < lstDynamic.Count; ++i)
                {
                    dynamic entity = lstDynamic[i];

                    foreach (var prop in entity.GetType().GetProperties())
                    {
                        sbResult.Append(prop.   GetValue(entity, null).ToString().Replace(',', '_'));
                        sbResult.Append(",");
                    }

                    sbResult.Append("\r\n");
                }

                retval = sbResult.ToString();

                #endregion
            }
            else if (type == "email")
            {
                PatientNotification entity = new PatientNotification();
                entity.GCMailTypeOrder = Constant.MailTypeOrder.LAPORAN_PEMBAYARAN_HONDOK;
                entity.MailTo = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnParamedicID.Value)).EmailAddress1;
                entity.ReportCode = hdnReportCode.Value.ToString();
                entity.GCMailStatus = Constant.MailNotificationStatus.OPENED;
                entity.Remarks = string.Format("{0};{1},{2}", Helper.GetDatePickerValue(txtPeriodeFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtPeriodeTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112), hdnParamedicID.Value);
                entity.CreatedBy = entity.SentBy = AppSession.UserLogin.UserID;
                entity.CreatedDate = DateTime.Now;
                patientNotificationDao.Insert(entity);

            }
            ctx.CommitTransaction();
            return result;
        }

    }
}