using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using System.Text;
using System.Reflection;
using System.Collections;
using System.IO;
using FindAndReplace;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class DownloadFormCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnVisitID.Value = param;
        }

        protected void cbpMedicalSickLeave_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";
            download();
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        private void download() {
            try
            {
                string ReportID = "PM-00109";
                string filterExpression = "MRN=100586";
                string GCDataSourceType = Constant.DataSourceType.VIEW;

                string pathFolder = AppConfigManager.QISPhysicalDirectory.Replace('/', '\\');

                string inputFile = string.Format("{0}\\FormTemplate\\R01.docx", pathFolder);
                string outputFile = string.Format("{0}\\FormTemplate\\Output.docx", pathFolder);  
                // Copy Word document.
                File.Copy(inputFile, outputFile, true);

                if (GCDataSourceType == Constant.DataSourceType.STORED_PROCEDURE)
                {
                    //List<dynamic> lstDynamic = null;
                    //List<Variable> lstVariable = new List<Variable>();
                    //List<vReportParameter> listReportParameter = BusinessLayer.GetvReportParameterList(string.Format("ReportID = {0} ORDER BY DisplayOrder", rm.ReportID));
                    //string[] value = filterParam.Split('|');
                    //for (int i = 0; i < listReportParameter.Count; ++i)
                    //{
                    //    vReportParameter reportParameter = listReportParameter[i];
                    //    lstVariable.Add(new Variable { Code = reportParameter.FieldName, Value = GetFilterExpression(value[i]) });

                    //    oReportParameter += string.Format("{0} = {1}|", reportParameter.FieldName, GetFilterExpression(value[i]));
                    //}

                    //lstDynamic = BusinessLayer.GetDataReport(rm.ObjectTypeName, lstVariable);

                    //dynamic fields = lstDynamic[0];

                    //foreach (var prop in fields.GetType().GetProperties())
                    //{
                    //    sbResult.Append(prop.Name);
                    //    sbResult.Append(",");
                    //}

                    //sbResult.Append("\r\n");

                    //for (int i = 0; i < lstDynamic.Count; ++i)
                    //{
                    //    dynamic entity = lstDynamic[i];

                    //    foreach (var prop in entity.GetType().GetProperties())
                    //    {
                    //        //sbResult.Append(prop.GetValue(entity, null));
                    //        sbResult.Append(prop.GetValue(entity, null).ToString().Replace(',', '_'));
                    //        sbResult.Append(",");
                    //    }

                    //    sbResult.Append("\r\n");
                    //}
                }
                else
                {

                    // Open copied document.
                    using (var flatDocument = new FlatDocument(outputFile))
                    {
                        //oReportParameter = filterExpression;
                        ReportMaster oReport = BusinessLayer.GetReportMasterList(string.Format("ReportCode='{0}'", ReportID)).FirstOrDefault();
                        MethodInfo method = typeof(BusinessLayer).GetMethod(oReport.ObjectTypeName, new[] { typeof(string) });
                        Object obj = method.Invoke(null, new string[] { filterExpression });
                        IList collection = (IList)obj;
                        dynamic fields = collection[0];
                        foreach (object temp in collection)
                        {
                            foreach (var prop in temp.GetType().GetProperties())
                            {
                                string  fieldName = string.Format("[{0}]", prop.Name);
                                var fieldValue = prop.GetValue(temp, null);
                                string textValid = "";

                                if (fieldValue != null)
                                {
                                    textValid = fieldValue.ToString();
                                }

                                flatDocument.FindAndReplace(fieldName, textValid);

                            }
                        }
                        // Save document on Dispose.
                        flatDocument.Dispose();
                    }
                }
 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               // result = string.Format("fail|{0}", ex.Message);
            }
            finally
            {
                
                Response.Flush();
                Response.End();
            }
        }
    }
}