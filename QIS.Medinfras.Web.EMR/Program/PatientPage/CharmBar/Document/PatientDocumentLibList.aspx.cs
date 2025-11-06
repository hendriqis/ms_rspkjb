using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using System.Text;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientDocumentLibList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadPatientDocumentList();
            }
        }
        public void LoadPatientDocumentList()
        {
            string lstID;
            string filterExpression = String.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);
            List<Int32> lstRecordID = BusinessLayer.GetvPatientDocumentIDList(filterExpression);

            StringBuilder sb = new StringBuilder();
            foreach (Int32 ID in lstRecordID)
            {
                sb.Append(ID);
                sb.Append('|');
            }
            lstID = sb.ToString();
            hdnRecordID.Value = lstID.Substring(0, (lstID.Length > 0 ? lstID.Length - 1 : lstID.Length));
        }
    }
}