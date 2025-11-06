using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class EpisodeTriase : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeTriase()
        {
            InitializeComponent();
        }

        public void InitializeReport(int RegistrationID)
        {
            List<Registration> entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", RegistrationID));
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", RegistrationID)).FirstOrDefault();
            vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", entityCV.VisitID)).FirstOrDefault();

            if (entity.GCTriage != null)
            {
                if (entity.GCTriage == "X079^001")
                {
                    xATS1.Checked = true;
                }
                else if (entity.GCTriage == "X079^002")
                {
                    xATS2.Checked = true;
                }
                else if (entity.GCTriage == "X079^003")
                {
                    xATS3.Checked = true;
                }
                else if (entity.GCTriage == "X079^004")
                {
                    xATS4.Checked = true;
                }
                else if (entity.GCTriage == "X079^005")
                {
                    xATS5.Checked = true;
                }
                else if (entity.GCTriage == "X079^006")
                {
                    xDOA.Checked = true;
                }
            }
            else
            {
                entity.GCTriage = "";
            }

            if (entity.GCAdmissionCondition != null)
            {
                if (entity.GCAdmissionCondition == "0043^001")
                {
                    xNonTrauma.Checked = true;
                }
                else if (entity.GCAdmissionCondition == "0043^002")
                {
                    xTrauma.Checked = true;
                }
            }
            else if (entity.GCAdmissionCondition == null)
            {
                entity.GCAdmissionCondition = "";
            }

            if (entity.StartServiceTime != null)
            {
                xTriaseTime.Text = string.Format(entity.StartServiceTime,"hh:ss");
            }
            else
            {
                entity.StartServiceTime = "";
            }

            if (entity.NoteText != null)
            {
                xMekanisme.Text = entity.NoteText;
            }
            else
            {
                entity.NoteText = "";
            }

            if (entity.GCAdmissionRoute == "")
            {
                entity.GCAdmissionRoute = "";
            }
            else if (entity.GCAdmissionRoute != "")
            {
                StandardCode entitiSC_GCAdmissionRoute = BusinessLayer.GetStandardCode(entity.GCAdmissionRoute);
                xDatang.Text = entitiSC_GCAdmissionRoute.StandardCodeName;
            }
     
            this.DataSource = entityRegistration;
            
        }

    }
}
