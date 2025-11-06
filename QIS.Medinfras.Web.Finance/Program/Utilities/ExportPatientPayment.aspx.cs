using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using DevExpress.Web.ASPxEditors;
using System.Globalization;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ExportPatientPayment : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.FN_UTILITY_EXPORT_PATIENT_PAYMENT;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            divImportMethod.InnerHtml = "TXT/CSV File";
            txtPeriod.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            if (ExportPatientPaymentTrx(ref errMessage))
                result += string.Format("success|{0}", errMessage);
            else
                result += string.Format("fail|{0}", errMessage);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool ExportPatientPaymentTrx(ref string errMessage)
        {
            try
            {
                DateTime trxDate = Helper.GetDatePickerValue(txtPeriod.Text);
                string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data/temp");
                string fileName = string.Format(@"HIS_AR_MISC_{0}_{1}_{2}.csv",trxDate.ToString(Constant.FormatString.DATE_FORMAT_112),DateTime.Now.Hour.ToString(),DateTime.Now.Minute.ToString());
                List<PatientPaymentEntryDTO> list = BusinessLayer.GetPatientPaymentDTOList(trxDate.ToString(Constant.FormatString.DATE_FORMAT_112));
                List<string> listJournal = new List<string>();
                bool result = true;
                if (list.Count > 0)
                {
                    #region Create CSV File in App_Data temp File
                    foreach (PatientPaymentEntryDTO item in list)
                        listJournal.Add(item.PatientPaymentResult);
                    File.Delete(string.Format(@"{0}/{1}",filePath,fileName));
                    File.WriteAllLines(string.Format(@"{0}/{1}", filePath, fileName), listJournal.AsEnumerable());
                    #endregion

                    //Send Email to Destination
                    result = SendEmailToDestination(ref errMessage,filePath, fileName);

                    if (result)
                    {
                        errMessage = string.Format("Interface file {0} berhasil dikirimkan via email.",fileName);
                    }
                    else
                    {
                        errMessage = string.Format("Interface file {0} berhasil dibuat tetapi proses pengiriman dengan email gagal.",fileName);
                    }
                }
                else
                {
                    result = false;
                }
                divFileName.InnerHtml = fileName;
                return result;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool SendEmailToDestination(ref string errMessage,string filePath,string fileName)
        {
            try
            {
                string addressFile = HttpContext.Current.Server.MapPath("~/Libs/App_Data/interface/recipient.adr");
                string emailCfg = HttpContext.Current.Server.MapPath("~/Libs/App_Data/interface/email.cfg");
                string[] config = File.ReadAllLines(emailCfg)[0].ToString().Split('|');
                string[] credentials = File.ReadAllLines(emailCfg)[1].ToString().Split('|');

                MailMessage message = new MailMessage();
                message.From = new MailAddress(credentials[0]);
                //Get Recipient Address
                IEnumerable<string> lstRecipient = File.ReadAllLines(addressFile);
                List<MailAddress> lstDestination = new List<MailAddress>();
                foreach (string address in lstRecipient)
                    message.To.Add(new MailAddress(address));

                message.Subject = string.Format("Transaksi Journal HIS dengan batch {0}", fileName);
                message.Body = "Transaction Journal List from HIS";
                message.IsBodyHtml = true;

                //Attachment
                string attachmentFileName = string.Format(@"{0}/{1}", filePath, fileName);
                Attachment attachment = new Attachment(attachmentFileName, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = attachment.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(attachmentFileName);
                disposition.ModificationDate = File.GetLastWriteTime(attachmentFileName);
                disposition.ReadDate = File.GetLastAccessTime(attachmentFileName);
                disposition.FileName = Path.GetFileName(attachmentFileName);
                disposition.Size = new FileInfo(attachmentFileName).Length;
                disposition.DispositionType = DispositionTypeNames.Attachment;
                message.Attachments.Add(attachment);

                string smtp = config[0];
                int port = Convert.ToInt16(config[1]);
                SmtpClient smtpClient = new SmtpClient(smtp, port);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new System.Net.NetworkCredential(credentials[0], credentials[1]);

                //smtpClient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
                //Object state = message;
                //smtpClient.SendAsync(message,state);
                smtpClient.Send(message);

                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        void smtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MailMessage mail = e.UserState as MailMessage;
            if (e.Cancelled || e.Error != null )
            {
                throw new Exception("Email gagal dikirim!");
            }
        }
    }
}