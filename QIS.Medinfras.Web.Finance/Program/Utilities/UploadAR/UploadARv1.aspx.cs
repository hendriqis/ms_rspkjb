using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxFileManager;
using DevExpress.Web.ASPxUploadControl;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class UploadARv1 : BasePageList
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
            Helper.SetControlEntrySetting(hdnUploadedFile, new ControlEntrySetting(true, true, false), "mpEntry");

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "ID IS NOT NULL";

            List<TestUploadBPJS> lstEntity = BusinessLayer.GetTestUploadBPJSList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            if (param[0] == "upload")
            {
                if (UploadTrx(ref errMessage))
                    result += string.Format("success|{0}", errMessage);
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool UploadTrx(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestUploadBPJSDao entityDao = new TestUploadBPJSDao(ctx);
            try
            {
                string fileUpload = hdnUploadedFile.Value;
                if (fileUpload != "")
                {
                    string[] parts = Regex.Split(fileUpload, ",").Skip(1).ToArray();
                    fileUpload = String.Join(",", parts);
                }

                string path = AppConfigManager.QISPhysicalDirectory;
                path += string.Format("{0}\\", AppConfigManager.QISFinanceUploadDocument.Replace('/', '\\'));

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    Directory.Delete(path, true);
                    Directory.CreateDirectory(path);

                    entityDao.Delete();
                }

                string fileName = "BPJS_UPLOAD.csv";

                FileStream fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                byte[] data = Convert.FromBase64String(fileUpload);
                bw.Write(data);
                bw.Close();

                string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, fileName));

                int rowCount = 0;
                foreach (string temp in lstTemp)
                {
                    if (rowCount != 0)
                    {
                        if (temp.Contains(','))
                        {
                            List<String> fieldTemp = temp.Split(',').ToList();

                            TestUploadBPJS oData = new TestUploadBPJS();
                            oData.ARInvoiceNo = fieldTemp[0];
                            oData.RegistrationNo = fieldTemp[2];
                            oData.SEPNo = fieldTemp[3];
                            oData.MedicalNo = fieldTemp[4];
                            oData.PatientName = fieldTemp[5];
                            oData.ARClaimedAmount = Convert.ToDecimal(fieldTemp[6]);
                            oData.ARPaymentAmount = Convert.ToDecimal(fieldTemp[7]);

                            oData.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(oData);
                        }
                    }
                    rowCount += 1;
                }

                ctx.CommitTransaction();

                return result;
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