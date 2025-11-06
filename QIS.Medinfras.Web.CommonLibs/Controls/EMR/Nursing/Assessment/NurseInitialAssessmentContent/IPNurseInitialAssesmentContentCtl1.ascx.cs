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
    public partial class IPNurseInitialAssesmentContentCtl1 : BaseDataCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            LoadSubjectiveContent(Convert.ToInt32(queryString));
        }

        private void LoadSubjectiveContent(int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY ChiefComplaintID DESC", visitID);
            vNurseChiefComplaint oChiefComplaint = BusinessLayer.GetvNurseChiefComplaintList(filterExpression).FirstOrDefault();

            if (oChiefComplaint != null)
            {
                txtDate.Text = oChiefComplaint.ChiefComplaintDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTime.Text = oChiefComplaint.ChiefComplaintTime;
                txtPhysicianName.Text = oChiefComplaint.ParamedicName;
                txtChiefComplaint.Text = oChiefComplaint.NurseChiefComplaintText;

                txtHPISummary.Text = oChiefComplaint.HPISummary;
                chkAutoAnamnesis.Checked = oChiefComplaint.IsAutoAnamnesis;
                chkAlloAnamnesis.Checked = oChiefComplaint.IsAlloAnamnesis;
            }
        }      
    }
}