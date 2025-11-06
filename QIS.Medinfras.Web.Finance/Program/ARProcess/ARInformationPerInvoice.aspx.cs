using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class ARInformationPerInvoice : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AR_INFORMATION_PER_INVOICE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            MenuMaster oMenu = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            hdnPageTitle.Value = oMenu.MenuCaption;

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            
            txtARInvoiceNo.Focus();

            string filterSetvar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                    AppSession.UserLogin.HealthcareID,
                                                    Constant.SettingParameter.FN_AR_LEAD_TIME,
                                                    Constant.SettingParameter.FN_AR_DUE_DATE_COUNT_FROM);
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(filterSetvar);

            hdnSetvarLeadTime.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_AR_LEAD_TIME).FirstOrDefault().ParameterValue;
            hdnSetvarHitungJatuhTempoDari.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_AR_DUE_DATE_COUNT_FROM).FirstOrDefault().ParameterValue;

        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "update")
            {
                string arInvoiceNo = txtARInvoiceNo.Text;
                if (OnUpdateARInvoice(ref errMessage))
                {
                    result += string.Format("success|{0}", arInvoiceNo);
                }
                else
                {
                    result += "fail|" + errMessage;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnUpdateARInvoice(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceHdDao entityHdDao = new ARInvoiceHdDao(ctx);
            TermDao termDao = new TermDao(ctx);
            try
            {
                if (hdnARInvoiceID.Value != "" && hdnARInvoiceID.Value != "0")
                {
                    ARInvoiceHd entity = entityHdDao.Get(Convert.ToInt32(hdnARInvoiceID.Value));
                    entity.DocumentDate = Helper.GetDatePickerValue(txtDocumentDate.Text);
                    entity.ARDocumentReceiveDate = Helper.GetDatePickerValue(txtARDocumentReceiveDate);
                    entity.ARDocumentReceiveByName = txtARDocumentReceiveByName.Text;
                    entity.Remarks = txtRemarks.Text;

                    Term term = termDao.Get(entity.TermID);

                    DateTime tempdue1; // 1 = TglInvoice(ARInvoiceDate) || 2 = TglDokumen(DocumentDate) || 3 = TglTerimaDokumen(ARDocumentReceiveDate)
                    if (hdnSetvarHitungJatuhTempoDari.Value == "1")
                    {
                        tempdue1 = entity.ARInvoiceDate.AddDays(term.TermDay);
                    }
                    else if (hdnSetvarHitungJatuhTempoDari.Value == "2")
                    {
                        tempdue1 = entity.DocumentDate.AddDays(term.TermDay);
                    }
                    else if (hdnSetvarHitungJatuhTempoDari.Value == "3")
                    {
                        tempdue1 = entity.ARDocumentReceiveDate.AddDays(term.TermDay);
                    }
                    else
                    {
                        tempdue1 = entity.DocumentDate.AddDays(term.TermDay);
                    }

                    DateTime tempdue2 = tempdue1.AddDays(Convert.ToInt32(hdnSetvarLeadTime.Value));
                    entity.DueDate = tempdue2;                            

                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, pilih nomor invoice nya terlebih dahulu.";
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

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');

                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            String filterExpression = string.Format("ARInvoiceNo = '{0}'", txtARInvoiceNo.Text);

            List<vARReceivingPerARInvoice> lstInvoiceDt = BusinessLayer.GetvARReceivingPerARInvoiceList(filterExpression);
            lvwView.DataSource = lstInvoiceDt;
            lvwView.DataBind();
        }
    }
}