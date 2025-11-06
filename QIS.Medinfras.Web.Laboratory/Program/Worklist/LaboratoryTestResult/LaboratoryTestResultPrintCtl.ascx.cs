using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Xml.Linq;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class LaboratoryTestResultPrintCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnLabResultID.Value = param;
                hdnFilterExpression.Value = string.Format("ID={0}", hdnLabResultID.Value);
                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);

                XDocument xdoc = Helper.LoadXMLFile(this, string.Format("right_panel/{0}.xml", ModuleID));
                if (xdoc != null)
                {
                    var lstQuickMenu = (from pg in xdoc.Descendants("page").Where(p => p.Attribute("menucode").Value == param)
                                        select new
                                        {
                                            Print = (from qm in pg.Descendants("print")
                                                     select new
                                                     {
                                                         Title = qm.Attribute("title").Value,
                                                         ReportCode = qm.Attribute("reportcode").Value
                                                     })

                                        }).FirstOrDefault();
                    if (lstQuickMenu != null)
                    {
                        if (lstQuickMenu.Print.Count() > 0)
                        {
                            rptPrint.DataSource = lstQuickMenu.Print;
                            rptPrint.DataBind();
                        }
                    }
                }
            }
        }
    }
}