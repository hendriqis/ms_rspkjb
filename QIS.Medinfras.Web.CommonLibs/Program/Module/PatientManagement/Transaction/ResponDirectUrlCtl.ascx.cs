using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ResponDirectUrlCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnID.Value = "~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/VitalSign/VitalSign.aspx?id=DIAGNOSTIC";
        }
    }
}