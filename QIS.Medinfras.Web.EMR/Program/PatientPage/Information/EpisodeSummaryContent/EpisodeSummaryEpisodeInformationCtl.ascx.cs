using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class EpisodeSummaryEpisodeInformationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            RegisteredPatient registeredPatient = AppSession.RegisteredPatient;
            lblRegistrationDateTime.InnerHtml = string.Format("{0} / {1}", registeredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112), registeredPatient.VisitTime);
    

            vConsultVisit patient = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", registeredPatient.VisitID))[0];
            lblVisitType.InnerHtml = patient.VisitTypeName;
            lblUnitName.InnerHtml = patient.ServiceUnitName;
            //lblLOS.InnerHtml = patient.LOS;
            lblPurpose.InnerHtml = "Sick Visit";

            List<vChiefComplaint> lstChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID));
            rptChiefComplaint.DataSource = lstChiefComplaint;
            rptChiefComplaint.DataBind();

            List<vPatientAllergy> lstAllergy = BusinessLayer.GetvPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0", registeredPatient.MRN));
            rptAllergies.DataSource = lstAllergy;
            rptAllergies.DataBind();
        }
    }
}