using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Dashboard.Program
{
    public partial class FormCtl : BaseViewPopupCtl
    {
        private const string STATUS_IMAGE_PATH = "~/libs/Images/Dashboard/";
        public override void InitializeDataControl(string param)
        {
            List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));

            img1.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "nurse.png");
            img2.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "clinic.png");
            img3.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "bed.png");
        }
    }
}