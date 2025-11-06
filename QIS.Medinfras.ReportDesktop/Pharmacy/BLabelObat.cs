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
    public partial class BLabelObat : BaseRpt
    {
        private string City = "";

        public BLabelObat()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            List<vPrescriptionOrderDt> lstEntity = BusinessLayer.GetvPrescriptionOrderDtList(string.Format("TransactionID = {0}", param[0]));
            vConsultVisit9 entity = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMaster(lstEntity.FirstOrDefault().ParamedicID);

            #region Page Header
            lblPatientName.Text = entity.PatientName;
            DateTime prescriptionDate = lstEntity.FirstOrDefault().PrescriptionDate;
            lblDate.Text = prescriptionDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblParamedicName.Text = entityParamedic.FullName;
            lblMedicalNo.Text = entity.MedicalNo;
            #endregion

            this.DataSource = lstEntity;
        }
    }
}
