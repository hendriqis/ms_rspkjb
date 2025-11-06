using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class BRingkasanMasukKeluarRSUKI : BaseCustomDailyPotraitRpt
    {
        public BRingkasanMasukKeluarRSUKI()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Healthcare healthcare = BusinessLayer.GetHealthcare(appSession.HealthcareID);
            Address address = BusinessLayer.GetAddress(Convert.ToInt32(healthcare.AddressID));
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vRegistrationBPJS entityBPJS = BusinessLayer.GetvRegistrationBPJSList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vConsultVisit entityVisitFrom = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", entity.LinkedRegistrationID)).FirstOrDefault();
            vReferrer entityReferrer = BusinessLayer.GetvReferrerList(string.Format("BusinessPartnerID = {0} AND GCReferrerGroup IN ('{1}','{2}')", entityVisit.ReferrerID, Constant.Referrer.DOKTER_RS, Constant.Referrer.PRAKTEK_DOKTER_LUAR)).FirstOrDefault();


            if (entityVisitFrom != null)
            {
                if (entityVisitFrom.DepartmentID == Constant.Facility.EMERGENCY)
                {
                    lblServiceUnitFrom.Text = string.Format("MELALUI IGD");

                    if (entityVisit.ReferralPhysicianName == null || entityVisit.ReferralPhysicianName == "")
                    {
                        if (entityReferrer == null)
                        {
                            lblParamedicNameFrom.Text = "-";
                        }
                        else
                        {
                            lblParamedicNameFrom.Text = entityReferrer.BusinessPartnerName;
                        }
                    }
                    else
                    {
                        lblParamedicNameFrom.Text = entityVisit.ReferralPhysicianName;
                    }
                }
                else if (entityVisitFrom.DepartmentID == Constant.Facility.OUTPATIENT || entityVisitFrom.DepartmentID == Constant.Facility.IMAGING || entityVisitFrom.DepartmentID == Constant.Facility.DIAGNOSTIC
                    || entityVisitFrom.DepartmentID == Constant.Facility.LABORATORY)
                {
                    lblServiceUnitFrom.Text = string.Format("MELALUI RAWAT JALAN");
                }
                if (entityVisit.ReferralPhysicianName == null || entityVisit.ReferralPhysicianName == "")
                {
                    if (entityReferrer == null)
                    {
                        lblParamedicNameFrom.Text = "-";
                    }
                    else
                    {
                        lblParamedicNameFrom.Text = entityReferrer.BusinessPartnerName;
                    }
                }
                else
                {
                    lblParamedicNameFrom.Text = entityVisit.ReferralPhysicianName;
                }
            }
            else
            {
                if (entityVisit.DepartmentID == Constant.Facility.INPATIENT)
                {
                    lblServiceUnitFrom.Text = string.Format("LANGSUNG RAWAT INAP");
                }
                else
                {
                    lblServiceUnitFrom.Text = string.Format("-");
                }
                lblParamedicNameFrom.Text = string.Format("-");
            }
            lblSSN.Text = entity.SSN;
            lblPatientAge.Text = string.Format("{0}/({1})", entity.cfPatientAge, entity.cfGenderInitial);
            lblServiceUnitName.Text = entityVisit.ServiceUnitName;
            lblParamedicName.Text = entityVisit.ParamedicName;
            lblClassName.Text = entityVisit.ClassName;
            if (entityBPJS != null)
            {
                lblClassNameBPJS.Text = entityBPJS.BPJSClassName;
            }
            else
            {
                lblClassNameBPJS.Text = "-";
            }
            lblVisitDate.Text = string.Format("{0} {1}", entity.RegistrationDateInString, entity.RegistrationTime);
            lblDischargeDate.Text = entityVisit.DischargeDateInString;
            if (entityVisit.LOSInDay != 0)
            {
                lblLOS.Text = string.Format("{0} Hr", entityVisit.LOSInDay);
            }
            else
            {
                lblLOS.Text = "";
            }
            lblReportSubTitle.Visible = false;
            lblReportTitle.Visible = false;

            base.InitializeReport(param);
        }

    }
}
