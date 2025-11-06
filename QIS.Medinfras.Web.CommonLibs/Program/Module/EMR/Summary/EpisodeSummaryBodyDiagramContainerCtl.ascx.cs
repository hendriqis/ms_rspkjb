using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class EpisodeSummaryBodyDiagramContainerCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string paneButtonCode)
        {
            getListIDPatientBodyDiagram();

        }
        public void getListIDPatientBodyDiagram()
        {
            string lstID;
            string filterExpression = String.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            List<Int32> lstPatientBodyDiagramHdID = BusinessLayer.GetvPatientBodyDiagramHdIDList(filterExpression);

            StringBuilder sb = new StringBuilder();
            foreach (Int32 ID in lstPatientBodyDiagramHdID)
            {
                sb.Append(ID);
                sb.Append('|');
            }
            lstID = sb.ToString();
            hdnIDBodyDiagram.Value = lstID.Substring(0, (lstID.Length > 0 ? lstID.Length - 1 : lstID.Length));
        }
    }
}