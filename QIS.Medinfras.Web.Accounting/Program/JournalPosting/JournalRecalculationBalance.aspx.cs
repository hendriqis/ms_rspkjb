using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class JournalRecalculationBalance : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.JOURNAL_RECALCULATE_BALANCE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
        
        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }
        
        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnTodayYear.Value = DateTime.Now.Year.ToString();
            hdnTodayMonth.Value = DateTime.Now.Month.ToString();

            if (Convert.ToInt16(hdnTodayMonth.Value) < 10)
            {
                hdnTodayMonth.Value = string.Format("0{0}", hdnTodayMonth.Value);
            }

            txtPeriodYear.Text = hdnTodayYear.Value;
            txtPeriodMonth.Text = hdnTodayMonth.Value;
        }

        protected override void SetControlProperties()
        {
        }
        
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "recalculate")
            {
                if (RecalculateJournalBalance(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool RecalculateJournalBalance(ref string errMessage)
        {
            try
            {
                string paramcPeriod = string.Format("{0}{1}", txtPeriodYear.Text, txtPeriodMonth.Text);

                string filterCheckJurnalFiskal = string.Format("TransactionCode = '{0}' AND YEAR(JournalDate) = '{1}' AND MONTH(JournalDate) = '{2}' AND GCTransactionStatus <> '{3}'",
                                                                Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR, txtPeriodYear.Text, txtPeriodMonth.Text, Constant.TransactionStatus.VOID);
                List<GLTransactionHd> jurnalFiskalLst = BusinessLayer.GetGLTransactionHdList(filterCheckJurnalFiskal);
                if (jurnalFiskalLst.Count() > 0)
                {
                    errMessage = "Tidak dapat Rekalkulasi Saldo COA karena sudah dilakukan Posting Jurnal pada periode tahun " + txtPeriodYear.Text + " bulan " + txtPeriodMonth.Text;
                    return false;
                }
                else
                {
                    bool result = BusinessLayer.RecalculationJournal(paramcPeriod, AppSession.UserLogin.UserID);
                    if (result)
                    {
                        return true;
                    }
                    errMessage = "Maaf, masih ada Jurnal yang Belum Seimbang";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}