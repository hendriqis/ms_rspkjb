using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EmergencyCare.Program
{
    public partial class PhysicalExaminationToolbarCtl : BaseUserControlCtl
    {
        public void SetSelectedMenu(int index)
        {
            HtmlGenericControl li = null;
            switch (index)
            {
                case 1: li = liToolbarPatientStatus; break;
                case 2: li = liToolbarPatientExamination; break;
                case 3: li = liToolbarPatientPhysicalExamination; break;
                case 4: li = liToolbarPatientProgressNote; break;
            }
            li.Attributes.Add("class", "selected");
        }
    }
}