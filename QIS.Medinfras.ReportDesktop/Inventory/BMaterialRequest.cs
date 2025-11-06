using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BMaterialRequest : BaseCustomDailyPotraitRpt
    {
        public BMaterialRequest()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vItemRequestHd entityHD = BusinessLayer.GetvItemRequestHdList(param[0])[0];
            vLocation entityLocationFrom = BusinessLayer.GetvLocationList(string.Format("LocationID = {0}", entityHD.FromLocationID))[0];
            Healthcare entityHealthcareFrom = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", entityLocationFrom.HealthcareID))[0];
            vLocation entityLocationTo = BusinessLayer.GetvLocationList(string.Format("LocationID = {0}", entityHD.ToLocationID))[0];
            Healthcare entityHealthcareTo = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", entityLocationTo.HealthcareID))[0];
            
            #region Header
            cSiteFrom.Text = entityHealthcareFrom.HealthcareName;
            cSiteTo.Text = entityHealthcareTo.HealthcareName;
            cItemType.Text = entityLocationFrom.LocationGroup;
            cNotes.Text = entityHD.Remarks;
            #endregion

            #region Footer
            cTTDPreparedBy.Text = entityHD.CreatedByName;
            cTTDReviewedBy.Text = entityHD.LastUpdatedByName;
            cTTDPrintedBy.Text = appSession.UserFullName;
            #endregion

            base.InitializeReport(param);
        }

    }
}
