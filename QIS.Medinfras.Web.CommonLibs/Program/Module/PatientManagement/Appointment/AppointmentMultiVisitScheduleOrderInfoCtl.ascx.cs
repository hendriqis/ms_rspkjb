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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentMultiVisitScheduleOrderInfoCtl : BaseViewPopupCtl
    {  
        public override void InitializeDataControl(string param)
        {
            vDiagnosticVisitSchedule entity = BusinessLayer.GetvDiagnosticVisitScheduleList(string.Format("ID = {0}", param)).FirstOrDefault();
            if (entity != null)
            {
                if (entity.AppointmentID != null)
                {
                    vAppointment app = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", entity.AppointmentID)).FirstOrDefault();
                    txtAppointmentNo.Text = app.AppointmentNo;
                    txtPatientName.Text = string.Format("({0}) {1}", app.MedicalNo, app.PatientName);
                    txtAppointmentDate.Text = string.Format("{0} {1}", app.cfStartDate, app.StartTime);
                    txtParamedicName.Text = app.ParamedicName;
                    txtServiceUnitName.Text = app.ServiceUnitName;
                    txtVisitType.Text = app.VisitTypeName;
                    txtItemName.Text = entity.ItemName1;
                    txtSequenceNo.Text = entity.SequenceNo;
                    //txtQueueNo.Text = app.cfQueueNo;
                }
            }
        }
    }
}