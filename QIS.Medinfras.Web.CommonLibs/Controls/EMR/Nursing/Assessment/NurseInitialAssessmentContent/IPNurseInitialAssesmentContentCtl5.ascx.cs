using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class IPNurseInitialAssesmentContentCtl5 : BaseDataCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            LoadContent(Convert.ToInt32(queryString));
        }

        private void LoadContent(int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY ChiefComplaintID DESC", visitID);
            vNurseChiefComplaint oChiefComplaint = BusinessLayer.GetvNurseChiefComplaintList(filterExpression).FirstOrDefault();

            if (oChiefComplaint != null)
            {
                divFormContent2.InnerHtml = oChiefComplaint.SocialHistoryLayout;
                hdnSocialHistoryLayout.Value = oChiefComplaint.SocialHistoryLayout;
                hdnSocialHistoryValue.Value = oChiefComplaint.SocialHistoryValues;
            }
        }    
    }
}