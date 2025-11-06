using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class BloodBankToolbarCtl : BaseUserControlCtl
    {
        public void SetSelectedMenu(int index)
        {
            HtmlGenericControl li = null;
            switch (index)
            {
                case 1: li = liToolbarPatientDiagnose; break;
                case 2: li = liToolbarAllergy; break;
                case 3: li = liToolbarPatientProcedure; break;
                case 4: li = liToolbarSOAP; break;
                case 5: li = liToolbarPatientDischarge; break;
                case 6: li = liToolbarCBGGrouper; break;
                case 7: li = liToolbarMedicalFolderStatus; break;
                case 8: li = liToolbarPatientNotes; break;
                case 9: li = liIntegratedNotes; break;
                case 10: li = liEpisodeSummary; break;
                case 11: li = liNursingNotes; break;
                case 12: li = liMedicalResume; break;
            }
            li.Attributes.Add("class", "selected");
        }
    }
}