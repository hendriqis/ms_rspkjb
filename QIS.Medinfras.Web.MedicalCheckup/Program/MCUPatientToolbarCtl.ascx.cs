using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class MCUPatientToolbarCtl : BaseUserControlCtl
    {
        
        public void SetSelectedMenu(int index)
        {
            HtmlGenericControl li = null;
            switch (index)
            {
                case 1: li = liToolbarFormMCU; break;
                case 2: li = liToolbarPatientDocument; break;
            }
            li.Attributes.Add("class", "selected");
        }

       
    }
}