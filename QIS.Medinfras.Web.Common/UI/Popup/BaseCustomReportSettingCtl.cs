using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Data.Service;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Common.UI
{
    public abstract class BaseCustomReportSettingCtl : BaseContentPopupCtl
    {
        public override void InitializeControl(string param)
        {
            base.InitializeControl(param);
            InitializeDataControl();
        }

        public abstract void InitializeDataControl();
        public abstract string GetReportParameter();
    }
}
