using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LNewsAttachmentPerShift : BaseDailyPortraitRpt
    {
        public LNewsAttachmentPerShift()
        {
            InitializeComponent();
        }
        //private List<GetAttachmentMinutesPerShift> lstAttachment = null;
        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            if (oHealthcare != null)
            {
                lblCabang.Text = oHealthcare.HealthcareName;
            }
            #endregion  
            List<vPatientChargesHd> lstPatientCharges = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionDate = '{0}' AND GCTransactionStatus != '{1}'", param[0], Constant.TransactionStatus.VOID));
            List<GetNewsAttachmentPerShift> lstAttachment = BusinessLayer.GetNewsAttachmentPerShiftList(param[0], param[1], param[2]);
            lblTanggalShift.Text = string.Format("{0} / {1} / {2}", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_PICKER_FORMAT), lstAttachment.FirstOrDefault().Shift, lstAttachment.FirstOrDefault().CreatedByName);
            #region Payment News
            SubPaymentNews.CanGrow = true;
            billPaymentDetailNewsPaymentAllGRANOSTIC.InitializeReport(lstAttachment);
            #endregion
            base.InitializeReport(param);
        }
    }
}
