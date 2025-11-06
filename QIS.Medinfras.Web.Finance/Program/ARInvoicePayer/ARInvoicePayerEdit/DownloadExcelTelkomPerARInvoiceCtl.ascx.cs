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
    public partial class DownloadExcelTelkomPerARInvoiceCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnARInvoiceID.Value = param;

            ARInvoiceHd arInvoiceHd = BusinessLayer.GetARInvoiceHd(Convert.ToInt32(hdnARInvoiceID.Value));
            txtARInvoiceNo.Text = arInvoiceHd.ARInvoiceNo;
            txtARInvoiceDate.Text = arInvoiceHd.ARInvoiceDateInString;

            hdnFileName.Value = string.Format("EXCEL_TAGIHAN_TELKOM_ARInvoiceNo_{0}_printedDate_{1}.csv", arInvoiceHd.ARInvoiceNo.Replace("/", ""), arInvoiceHd.ARInvoiceDateInString, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            return OnCustomButtonClick("download", ref errMessage, ref retval);
        }

        protected bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            if (type == "download")
            {
                #region Download Document

                string reportCode = string.Format("ReportCode = '{0}'", "FN-00088");
                ReportMaster rm = BusinessLayer.GetReportMasterList(reportCode).FirstOrDefault();

                StringBuilder sbResult = new StringBuilder();

                List<dynamic> lstDynamic = null;
                List<Variable> lstVariable = new List<Variable>();

                lstVariable.Add(new Variable { Code = "ARInvoiceID", Value = hdnARInvoiceID.Value });

                lstDynamic = BusinessLayer.GetDataReport(rm.ObjectTypeName, lstVariable);

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
                        sbResult.Append(prop.GetValue(entity, null).ToString().Replace(',', ';'));
                        sbResult.Append(",");
                    }

                    sbResult.Append("\r\n");
                }

                retval = sbResult.ToString();

                #endregion

            }

            return true;
        }
    }
}