using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common.UI
{
    public class BaseMP : System.Web.UI.MasterPage
    {
        protected string GetLabel(string code)
        {
            BasePage page = (BasePage)this.Page;
            return page.GetLabel(code);
        }
    }
}
