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
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class DownloadInvoiceCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            //string filterExpression = param;

            //List<vPatientPaymentBPJSClaim> lst = BusinessLayer.GetvPatientPaymentBPJSClaimList(filterExpression);
        }

        protected void btnDownloadProcess_Click(object sender, EventArgs e)
        {
            string filterExpression = "";

            #region download
            string result = "";
            string reportCode = string.Format("ReportCode = '{0}'", "PM-002123");
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

                MethodInfo method = typeof(BusinessLayer).GetMethod(rm.ObjectTypeName, new[] { typeof(string) });
                Object obj = method.Invoke(null, new string[] { filterExpression });
                //IList collection = (IList)obj;
                //dynamic fields = collection[0];

                //foreach (var prop in fields.GetType().GetProperties())
                //{
                //    sbResult.Append(prop.Name);
                //    sbResult.Append(",");
                //}
                //sbResult.Append("\r\n");

                //foreach (object temp in collection)
                //{
                //    foreach (var prop in temp.GetType().GetProperties())
                //    {
                //        var text = prop.GetValue(temp, null);
                //        string textValid = "";

                //        if (text != null)
                //        {
                //            textValid = text.ToString();
                //        }

                //        sbResult.Append(textValid.Replace(',', '_'));
                //        sbResult.Append(",");
                //    }

                //    sbResult.Append("\r\n");
                //}

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
            #endregion
        }
    }
}