using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data.Service.Helper;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BDistribusiBarangRSDOSOBA1HVS : BaseDailyPortraitRpt 
    {
        public BDistribusiBarangRSDOSOBA1HVS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param) 
        {
            vItemDistributionHd entityHd = BusinessLayer.GetvItemDistributionHdList(param[0])[0];
            lblDistributionNo.Text = entityHd.DistributionNo;
            lblTransactionDate.Text = entityHd.TransactionDateTimeInString;
            lblDistributionDate.Text = entityHd.DeliveryDateTimeInString;
            lblWarehouseCode.Text = String.Format("{0} - {1}", entityHd.FromLocationCode, entityHd.FromLocationName);
            lblOtherWarehouseCode.Text = String.Format("{0} - {1}", entityHd.ToLocationCode, entityHd.ToLocationName);
            lblRemarks.Text = entityHd.DeliveryRemarks;

            if (entityHd.RegistrationID != 0 && entityHd.RegistrationID != null)
            {
                lblRegistration.Text = string.Format("{0} | {1} | ({2}) {3}", entityHd.RegistrationNo, entityHd.ServiceUnitName, entityHd.MedicalNo, entityHd.PatientName);
            }
            else
            {
                lblRegistration.Text = "-";
            }

            //string filterExpression = string.Format(" ParameterCode IN ('{0}')", "SA0020");
            //List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            //lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "SA0020").FirstOrDefault().ParameterValue;

            lblCreatedByName.Text = entityHd.CreatedByName;

            lblFooterInfo.Text = String.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), AppSession.UserLogin.UserName);
            
            base.InitializeReport(param);
        }
    }
}
