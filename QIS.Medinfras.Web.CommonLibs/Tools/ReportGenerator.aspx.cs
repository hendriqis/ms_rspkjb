using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using System.Reflection;
using QIS.Medinfras.ReportDesktop;
using System.IO;

namespace QIS.Medinfras.Web.CommonLibs
{
    public partial class ReportGenerator : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void btnGenerateReport_Click(object sender, EventArgs e)
        {
            string targetFolder = HttpContext.Current.Request.MapPath("~/Libs/App_Data/Report/Original/");
            if (IsDirectoryEmpty(targetFolder))
            {
                List<ReportMaster> lstReportMaster = BusinessLayer.GetReportMasterList("IsDirectPrint = 'false' AND IsDeleted = 'false'");
                foreach (ReportMaster rm in lstReportMaster)
                {
                    Assembly assembly = Assembly.Load("QIS.Medinfras.ReportDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                    string rptName = rm.ClassName;
                    BaseRpt rpt = (BaseRpt)assembly.CreateInstance("QIS.Medinfras.ReportDesktop." + rptName);
                    rptName = rptName.Replace(".cs", "");
                    if (rpt != null)
                        rpt.SaveLayoutToXml(targetFolder + rptName);
                }
            }
        }
        public void btnGenerateBaseReport_Click(object sender, EventArgs e)
        {
            string targetFolder = HttpContext.Current.Request.MapPath("~/Libs/App_Data/Report/Base/");
            if (IsDirectoryEmpty(targetFolder))
            {
                List<string> lstBaseRpt = new List<string>();
                lstBaseRpt.Add("BaseCustomPaperRpt");
                lstBaseRpt.Add("BaseDailyLandscapeRpt");
                lstBaseRpt.Add("BaseDailyPortraitRpt");

                foreach (string rptName in lstBaseRpt)
                {
                    Assembly assembly = Assembly.Load("QIS.Medinfras.ReportDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                    BaseRpt rpt = (BaseRpt)assembly.CreateInstance("QIS.Medinfras.ReportDesktop." + rptName);
                    if (rpt != null)
                        rpt.SaveLayoutToXml(targetFolder + rptName);
                }
            }
        }
        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
    }
}