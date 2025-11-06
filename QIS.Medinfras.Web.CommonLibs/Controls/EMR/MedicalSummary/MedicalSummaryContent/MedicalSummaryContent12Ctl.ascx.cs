using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class MedicalSummaryContent12Ctl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            LoadContentInformation(Convert.ToInt32(queryString));
        }

        private void LoadContentInformation(int visitID)
        {
        }      
    }
}