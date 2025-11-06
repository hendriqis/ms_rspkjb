using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class EditPatientPhotoCtl : BaseViewPopupCtl
    {
        protected string MRN = "";
        protected string isUsingMRN = "1";
        public override void InitializeDataControl(string param)
        {
            if (param.Contains('|'))
            {
                string[] paramArr = param.Split('|');
                isUsingMRN = paramArr[0];
                MRN = paramArr[1];
            }
            else {
                MRN = param;
            }
                
        }
    }
}