using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for DownloadExcel
    /// </summary>
    public class DownloadExcel : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string fileName = "MCU_PatientList_Template.xlsx";
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.MC_FILE_TEMPLATE_EXCEL_DOWNLOAD));
            if (lstSetvarDt.Count > 0)
            {
                fileName = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.MC_FILE_TEMPLATE_EXCEL_DOWNLOAD).FirstOrDefault().ParameterValue;
            }
            
            try
            {
                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.ClearContent();
                response.Clear();
                response.ContentType = "Application/x-msexcel";
                response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}_", fileName.Split('.')[0]) + DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112) + "_" + DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Replace(':', '_') + ".xlsx");
                response.TransmitFile(string.Format("{0}\\Files\\{1}", AppConfigManager.QISPhysicalDirectory, fileName));
                response.Flush();
                response.End();
            }
            catch (Exception ex)
            {
                
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}