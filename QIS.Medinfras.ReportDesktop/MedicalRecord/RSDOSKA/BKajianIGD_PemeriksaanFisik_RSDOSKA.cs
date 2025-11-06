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
    public partial class BKajianIGD_PemeriksaanFisik_RSDOSKA : DevExpress.XtraReports.UI.XtraReport
    {
        public BKajianIGD_PemeriksaanFisik_RSDOSKA()
        {
            InitializeComponent();
        }

        public void InitializeReport(int RegistrationID)
        {
            List<Registration> entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", RegistrationID));
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", RegistrationID)).FirstOrDefault();
            vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", entityCV.VisitID)).FirstOrDefault();

            vReviewOfSystemDt entityROSHead = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID = {0} AND GCROSystem IN ('X098^003', 'X098^025') ORDER BY ID DESC", entityCV.VisitID)).FirstOrDefault();
            if (entityROSHead != null)
            {
                if (entityROSHead.IsNormal)
                {
                    xrTableCell5.Text = entityROSHead.Remarks;
                    xrTableCell6.Text = string.Empty;
                }
                else
                {
                    xrTableCell5.Text = string.Empty;
                    xrTableCell6.Text = entityROSHead.Remarks;
                }
            }
            vReviewOfSystemDt entityROSEyes = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID = {0} AND GCROSystem IN ('X098^005', 'X098^69') ORDER BY ID DESC", entityCV.VisitID)).FirstOrDefault();
            if (entityROSEyes != null)
            {
                if (entityROSEyes.IsNormal)
                {
                    xrTableCell2.Text = entityROSEyes.Remarks;
                    xrTableCell3.Text = string.Empty;
                }
                else
                {
                    xrTableCell2.Text = string.Empty;
                    xrTableCell3.Text = entityROSEyes.Remarks;
                }
            }
            vReviewOfSystemDt entityROSNeck = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID = {0} AND GCROSystem IN ('X098^004', 'X098^026') ORDER BY ID DESC", entityCV.VisitID)).FirstOrDefault();
            if (entityROSNeck != null)
            {
                if (entityROSNeck.IsNormal)
                {
                    xrTableCell14.Text = entityROSNeck.Remarks;
                    xrTableCell15.Text = string.Empty;
                }
                else
                {
                    xrTableCell14.Text = string.Empty;
                    xrTableCell15.Text = entityROSNeck.Remarks;
                }
            }
            vReviewOfSystemDt entityROSChest = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID = {0} AND GCROSystem IN ('X098^027') ORDER BY ID DESC", entityCV.VisitID)).FirstOrDefault();
            if (entityROSChest != null)
            {
                if (entityROSChest.IsNormal)
                {
                    xrTableCell11.Text = entityROSChest.Remarks;
                    xrTableCell12.Text = string.Empty;
                }
                else
                {
                    xrTableCell11.Text = string.Empty;
                    xrTableCell12.Text = entityROSChest.Remarks;
                }
            }
            vReviewOfSystemDt entityROSStomach = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID = {0} AND GCROSystem IN ('X098^012', 'X098^028') ORDER BY ID DESC", entityCV.VisitID)).FirstOrDefault();
            if (entityROSStomach != null)
            {
                if (entityROSStomach.IsNormal)
                {
                    xrTableCell8.Text = entityROSStomach.Remarks;
                    xrTableCell9.Text = string.Empty;
                }
                else
                {
                    xrTableCell8.Text = string.Empty;
                    xrTableCell9.Text = entityROSStomach.Remarks;
                }
            }
            vReviewOfSystemDt entityROSPelvis = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID = {0} AND GCROSystem IN ('X098^029') ORDER BY ID DESC", entityCV.VisitID)).FirstOrDefault();
            if (entityROSPelvis != null)
            {
                if (entityROSPelvis.IsNormal)
                {
                    xrTableCell20.Text = entityROSPelvis.Remarks;
                    xrTableCell21.Text = string.Empty;
                }
                else
                {
                    xrTableCell20.Text = string.Empty;
                    xrTableCell21.Text = entityROSPelvis.Remarks;
                }
            }
            vReviewOfSystemDt entityROSEkstremitas = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID = {0} AND GCROSystem IN ('X098^013', 'X098^030') ORDER BY ID DESC", entityCV.VisitID)).FirstOrDefault();
            if (entityROSEkstremitas != null)
            {
                if (entityROSEkstremitas.IsNormal)
                {
                    xrTableCell17.Text = entityROSEkstremitas.Remarks;
                    xrTableCell18.Text = string.Empty;
                }
                else
                {
                    xrTableCell17.Text = string.Empty;
                    xrTableCell18.Text = entityROSEkstremitas.Remarks;
                }
            }

            this.DataSource = entityRegistration;
        }
    }
}
