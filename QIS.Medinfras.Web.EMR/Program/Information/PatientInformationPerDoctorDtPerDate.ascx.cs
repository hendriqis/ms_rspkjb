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
    public partial class PatientInformationPerDoctorDtPerDate : BaseViewPopupCtl
    {

        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');
            hdnDepartmentID.Value = temp[0];
            txtDepartmentName.Text = BusinessLayer.GetDepartmentList(string.Format("DepartmentID = '{0}'",hdnDepartmentID.Value)).FirstOrDefault().DepartmentName;            
            hdnParamedicIDCtl.Value = temp[1];
            hdnVisitDay.Value = temp[2];
            hdnVisitMonth.Value = temp[3];
            hdnVisitYear.Value = temp[4];
            hdnVisitStatus.Value = temp[5];
            BindGridView();
        }

        private void BindGridView()
        {
//            String filter = String.Format(@"ParamedicID = '{0}' AND DATEPART(DAY, VisitDate) = '{1}' 
//                                        AND DATEPART(MONTH, VisitDate) = '{2}' AND DATEPART(YEAR, VisitDate) = '{3}' AND DepartmentID = '{4}'",
//                                        hdnParamedicIDCtl.Value, hdnVisitDay.Value, hdnVisitMonth.Value, hdnVisitYear.Value, hdnDepartmentID.Value);

            String filter = String.Format(@"(ParamedicID = {0} OR {1} IN (SELECT pt.ParamedicID FROM ParamedicTeam pt WITH(NOLOCK) WHERE pt.RegistrationID = vConsultVisit.RegistrationID AND pt.IsDeleted = 0)) AND DATEPART(DAY, VisitDate) = '{2}' 
                                        AND DATEPART(MONTH, VisitDate) = '{3}' AND DATEPART(YEAR, VisitDate) = '{4}' AND DepartmentID = '{5}'",
                            hdnParamedicIDCtl.Value, AppSession.UserLogin.ParamedicID, hdnVisitDay.Value, hdnVisitMonth.Value, hdnVisitYear.Value, hdnDepartmentID.Value);

            if (hdnVisitStatus.Value == "Valid")
            {
                filter += String.Format("AND GCVisitStatus != '{0}'", Constant.VisitStatus.CANCELLED);
            }
            else if (hdnVisitStatus.Value == "Void")
            {
                filter += String.Format("AND GCVisitStatus = '{0}'", Constant.VisitStatus.CANCELLED);
            }

            //          List<vConsultVisit> lstConsultVisit = BusinessLayer.GetvConsultVisitList(string.Format(@"ParamedicID = '{0}' AND DATEPART(DAY, VisitDate) = '{1}' 
            //                                                                                    AND DATEPART(MONTH, VisitDate) = '{2}' AND DATEPART(YEAR, VisitDate) = '{3}' AND DepartmentID = '{4}'",
            //                                                                                    hdnParamedicIDCtl.Value, hdnVisitDay.Value, hdnVisitMonth.Value, hdnVisitYear.Value, hdnDepartmentID.Value));
            
            List<vConsultVisit> lstConsultVisit = BusinessLayer.GetvConsultVisitList(filter);

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