using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientInformationPerDoctorDt : BaseViewPopupCtl
    {

        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');
            hdnDepartmentID.Value = temp[0];
            txtDepartmentName.Text = BusinessLayer.GetDepartmentList(string.Format("DepartmentID = '{0}'",hdnDepartmentID.Value)).FirstOrDefault().DepartmentName;            
            hdnParamedicIDCtl.Value = temp[1];
            hdnVisitMonth.Value = temp[2];
            hdnVisitYear.Value = temp[3];            
            BindGridView();
        }

        private void BindGridView()
        {
//            List<vConsultVisit> lstConsultVisit = BusinessLayer.GetvConsultVisitList(string.Format(@"ParamedicID = '{0}' AND DATEPART(MONTH, VisitDate) = '{1}' 
//                                                                                    AND DATEPART(YEAR, VisitDate) = '{2}' AND DepartmentID = '{3}'",
//                                                                                    hdnParamedicIDCtl.Value, hdnVisitMonth.Value, hdnVisitYear.Value, hdnDepartmentID.Value));
            List<vConsultVisit> lstConsultVisit = BusinessLayer.GetvConsultVisitList(string.Format(@"(ParamedicID = {0} OR {1} IN (SELECT pt.ParamedicID FROM ParamedicTeam pt WITH(NOLOCK) WHERE pt.RegistrationID = vConsultVisit.RegistrationID AND pt.IsDeleted = 0)) AND DATEPART(MONTH, VisitDate) = '{2}' 
                                                                                    AND DATEPART(YEAR, VisitDate) = '{3}' AND DepartmentID = '{4}'",
                                                                        hdnParamedicIDCtl.Value, AppSession.UserLogin.ParamedicID, hdnVisitMonth.Value, hdnVisitYear.Value, hdnDepartmentID.Value)).OrderBy(lst => lst.VisitDate).ToList();
            grdPopupView.DataSource = lstConsultVisit;
            grdPopupView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}