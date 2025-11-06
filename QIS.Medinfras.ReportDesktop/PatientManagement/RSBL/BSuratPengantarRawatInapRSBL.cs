using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;
namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratPengantarRawatInapRSBL : BaseCustomDailyPotraitRpt
    {
        public BSuratPengantarRawatInapRSBL()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string lab = "";
            string rad = "";
            string other = "";

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format(param[0])).FirstOrDefault();
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityVisit.RegistrationID)).FirstOrDefault();
            ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = '{0}'", entityVisit.ParamedicID))[0];
                
            if (entity.DiagnoseID == null && entity.DiagnoseID.ToString() == "")
                lblDianosis.Text = string.Format("{0}", entity.DiagnosisText);
            else
                lblDianosis.Text = string.Format("{0}", entity.DiagnoseName);

            string filterExpressionLab = string.Format("VisitID = {0} AND TransactionCode  = {1} AND GCTransactionStatus != '{2}'", entityVisit.VisitID, Constant.TransactionCode.LABORATORY_TEST_ORDER, Constant.TransactionStatus.VOID);
            List<vTestOrderHd1> lstLab = BusinessLayer.GetvTestOrderHd1List(filterExpressionLab);
            StringBuilder sbTextLab = new StringBuilder();
            foreach (vTestOrderHd1 item in lstLab)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbTextLab.AppendLine(string.Format(" {0}", detail.ItemName1));
                }
            }

            string filterExpressionRad = string.Format("VisitID = {0} AND TransactionCode = {1} AND GCTransactionStatus != '{2}'", entityVisit.VisitID, Constant.TransactionCode.IMAGING_TEST_ORDER, Constant.TransactionStatus.VOID);
            List<vTestOrderHd1> lstRad = BusinessLayer.GetvTestOrderHd1List(filterExpressionRad);
            StringBuilder sbTextRad = new StringBuilder();
            foreach (vTestOrderHd1 item in lstRad)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbTextRad.AppendLine(string.Format(" {0}", detail.ItemName1));
                }
            }

            string filterExpressionOther = string.Format("VisitID = {0} AND TransactionCode NOT IN ({1},{2}) AND GCTransactionStatus != '{3}'", entityVisit.VisitID, Constant.TransactionCode.LABORATORY_TEST_ORDER, Constant.TransactionCode.IMAGING_TEST_ORDER, Constant.TransactionStatus.VOID);
            List<vTestOrderHd1> lstOther = BusinessLayer.GetvTestOrderHd1List(filterExpressionOther);
            StringBuilder sbTextOther = new StringBuilder();
            foreach (vTestOrderHd1 item in lstOther)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbTextOther.AppendLine(string.Format(" {0}", detail.ItemName1));
                }
            }

            if (!string.IsNullOrEmpty(sbTextLab.ToString()))
            {
                lab = string.Format("Laboratorium : {0}", sbTextLab.ToString());
            }
            if (!string.IsNullOrEmpty(sbTextRad.ToString()))
            {
                rad = string.Format("Radiologi : {0}", sbTextRad.ToString());
            }
            if (!string.IsNullOrEmpty(sbTextOther.ToString()))
            {
                other = string.Format("Penunjang : {0}", sbTextOther.ToString());
            }

            lblPlanning.Text = string.Format("{0}, {1}, {2}", lab, rad, other);

            #region QR Codes Image
            string contents = string.Format(@"{0}\r\n{1}",
                entityParamedic.FullName, entityParamedic.LicenseNo);

            QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
            qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qRCodeEncoder.QRCodeScale = 4;
            qRCodeEncoder.QRCodeVersion = 0;
            qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            MemoryStream memoryStream = new MemoryStream();

            using (Bitmap bitMap = qRCodeEncoder.Encode(contents, System.Text.Encoding.UTF8))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ttdDokter.Image = System.Drawing.Image.FromStream(ms, true, true);
                }
            }
            #endregion

            lbParamedicVisit.Text = string.Format("({0})", entity.ParamedicName);
            lblPrintDate.Text = string.Format("{0}, {1}", entityHealthcare.City, entity.RegistrationDateInString);

            base.InitializeReport(param);
        }
    }
}
