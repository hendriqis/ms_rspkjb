using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class BodyDiagramListCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            getListIDPatientBodyDiagram();
        }
        public void getListIDPatientBodyDiagram()
        {
            string lstID;
            string filterExpression = String.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);
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