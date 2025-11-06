using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class BPJSClaimToolbarCtl : BaseUserControlCtl
    {
        public void SetSelectedMenu(int index)
        {
            HtmlGenericControl li = null;
            switch (index)
            {
                case 1: li = liToolbarPatientDiagnoseClaim; break;
                case 2: li = liToolbarPatientProcedureClaim; break;
                case 3: li = liToolbarPatientIntegratedNotesClaim; break;
                case 4: li = liToolbarPatientEpisodeSummaryClaim; break;
            }
            li.Attributes.Add("class", "selected");
        }
    }
}