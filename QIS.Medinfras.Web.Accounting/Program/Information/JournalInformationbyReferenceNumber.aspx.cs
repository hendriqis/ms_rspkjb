using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QISEncryption;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;


namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class JournalInformationbyReferenceNumber : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.JOURNAL_INFORMATION_BY_REFERENCE_NUMBER;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND StandardCodeID IN ('{1}','{2}')",
                                                                                        Constant.StandardCode.TRANSACTION_STATUS, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.APPROVED));
            lst.Insert(0, new StandardCode { StandardCodeID = "%%", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboTransactionStatus, lst, "StandardCodeName", "StandardCodeID");
            cboTransactionStatus.SelectedIndex = 0;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref PageCount);
                    result = "refresh|" + PageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpTotal_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            SetTotalText();
        }

        private void SetTotalText()
        {
            decimal debit = 0, credit = 0;
            string oReferenceNo = txtReferenceNo.Text;

            List<GetJournalInformationbyReferenceNumber> lstEntity = null;

            lstEntity = BusinessLayer.GetJournalInformationbyReferenceNumber(oReferenceNo, cboTransactionStatus.Value.ToString());

            debit = lstEntity.Sum(a => a.DebitAmount);
            credit = lstEntity.Sum(b => b.CreditAmount);

            txtTotalBalanceDEBIT.Text = debit.ToString(Constant.FormatString.NUMERIC_2);
            txtTotalBalanceCREDIT.Text = credit.ToString(Constant.FormatString.NUMERIC_2);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string oReferenceNo = txtReferenceNo.Text;
            string oGCTransactionStatus = cboTransactionStatus.Value.ToString();

            List<GetJournalInformationbyReferenceNumber> lstEntity = BusinessLayer.GetJournalInformationbyReferenceNumber(oReferenceNo, oGCTransactionStatus);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionDtDao entityDtDao = new GLTransactionDtDao(ctx);

            try
            {
                if (hdnSelectedTransactionDtID.Value.Substring(0, 1) == ",")
                {
                    hdnSelectedTransactionDtID.Value = hdnSelectedTransactionDtID.Value.Substring(1);
                }

                string filterDt = string.Format("TransactionDtID IN ({0}) AND IsDeleted = 0 AND GCItemDetailStatus != '{1}'", hdnSelectedTransactionDtID.Value, Constant.TransactionStatus.VOID);
                List<GLTransactionDt> lstGLDT = BusinessLayer.GetGLTransactionDtList(filterDt, ctx);
                foreach (GLTransactionDt gldt in lstGLDT)
                {
                    if (type == "verified")
                    {
                        gldt.IsVerified = true;
                        gldt.LastVerifiedBy = AppSession.UserLogin.UserID;
                        gldt.LastVerifiedDate = DateTime.Now;
                        gldt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(gldt);
                    }
                    else if (type == "unverified")
                    {
                        gldt.IsVerified = false;
                        gldt.LastVerifiedBy = null;
                        gldt.LastVerifiedDate = null;
                        gldt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(gldt);
                    }
                }

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

        protected void btnRAWExport_Click(object sender, EventArgs e)
        {
            string result = "";
            string reportCode = string.Format("ReportCode = 'AC-00012'");
            string reportParameter = "";
            ReportMaster rm = BusinessLayer.GetReportMasterList(reportCode).FirstOrDefault();
            string fileName = string.Format(@"{0}_{1}_{2}_{3}", rm.ClassName, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString());
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
                Response.Charset = "";
                Response.ContentType = "application/text";

                StringBuilder sbResult = new StringBuilder();
                SqlConnection sqlCon = new SqlConnection(getCNSetting());
                sqlCon.Open();

                string query = string.Format("EXEC {0}", rm.ObjectTypeName);

                string oReferenceNo = txtReferenceNo.Text;
                string oGCTransactionStatus = cboTransactionStatus.Value.ToString();
                query += string.Format(" '{0}', '{1}'", oReferenceNo, oGCTransactionStatus);

                reportParameter = query;

                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                SqlDataReader reader = sqlCmd.ExecuteReader();
                object[] output = new object[reader.FieldCount];

                for (int i = 0; i < reader.FieldCount; i++)
                    output[i] = reader.GetName(i);

                sbResult.Append(string.Join(",", output));
                sbResult.Append("\r\n");

                while (reader.Read())
                {
                    reader.GetValues(output);

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        output[i] = output[i].ToString().Replace(',', '_');
                    }

                    sbResult.Append(string.Join(",", output));
                    sbResult.Append("\r\n");
                }

                reader.Close();

                sqlCon.Close();
                Response.Output.Write(sbResult.ToString());
                result = "success";
            }
            catch (Exception ex)
            {
                result = string.Format("fail|{0}", ex.Message);
            }
            finally
            {
                Response.Flush();
                Response.End();
            }
            InsertReportPrintLog(reportCode, reportParameter);
        }

        public String getCNSetting()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["cnsetting"];
            string cnstring = settings.ConnectionString;
            string paramDec = Encryption.DecryptString(cnstring);

            return paramDec;
        }

        protected bool InsertReportPrintLog(string ReportCode, string ReportParameter)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ReportPrintLogDao entityDao = new ReportPrintLogDao(ctx);
            try
            {
                if (ReportCode != null && ReportCode != "")
                {
                    ReportMaster rm = BusinessLayer.GetReportMasterList(string.Format("ReportCode = '{0}'", ReportCode), ctx).FirstOrDefault();

                    ReportPrintLog entity = new ReportPrintLog();
                    entity.ReportID = rm.ReportID;
                    entity.ReportCode = rm.ReportCode;
                    entity.ReportParameter = ReportParameter;
                    entity.PrintedBy = AppSession.UserLogin.UserID;
                    entity.PrintedDate = DateTime.Now;
                    entityDao.Insert(entity);

                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
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