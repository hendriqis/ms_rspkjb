using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MPMRPatientSOAP : BaseMP
    {
        public void SetSelectedMenu(int index)
        {
            HtmlGenericControl li = null;
            switch (index)
            {
                case 1: li = liToolbarPatientDiagnose; break;
                case 2: li = liToolbarPatientProcedure; break;
                case 3: li = liToolbarSOAP; break;
                case 4: li = liToolbarPatientDischarge; break;
            }
            li.Attributes.Add("class", "selected");
        }
    }
}