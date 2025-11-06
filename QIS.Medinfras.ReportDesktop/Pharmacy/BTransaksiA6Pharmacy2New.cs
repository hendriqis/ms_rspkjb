using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BTransaksiA6Pharmacy2New : BaseA6Rpt
    {
        private int lineNumber = 0;
        private int detailID = 0;
        private int oldDetailID = 0;
        private decimal totalAmount = 0;

        public BTransaksiA6Pharmacy2New()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lineNumber = 0;

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", param[0])).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
            vPrescriptionOrderHd entityOrder = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("PrescriptionOrderID = {0}", entity.PrescriptionOrderID)).FirstOrDefault();

            String filterExpression = string.Format("PrescriptionOrderID = {0} OR ParentID = {0} AND IsDeleted = 0", entity.PrescriptionOrderID);
            List<vPrescriptionOrderDt> lstEntity = BusinessLayer.GetvPrescriptionOrderDtList(filterExpression);

            lblNoAntrian.Visible = false;
            pictMRNQR.Visible = false;
            if (!String.IsNullOrEmpty(entityOrder.ReferenceNo))
            {
                string[] refNo = entityOrder.ReferenceNo.Split('|');
                if (refNo.Length > 1)
                {
                    if (!string.IsNullOrEmpty(refNo[1]))
                    {
                        lblNoAntrian.Text = refNo[1];
                        lblNoAntrian.Visible = true;

                        #region QR Codes Image
                        string contents = string.Format(@"{0}", refNo[1]);

                        QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
                        qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                        qRCodeEncoder.QRCodeScale = 4;
                        qRCodeEncoder.QRCodeVersion = 0;
                        qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                        MemoryStream memoryStream = new MemoryStream();
                        //System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                        //imgBarCode.Height = 400;
                        //imgBarCode.Width = 400;

                        using (Bitmap bitMap = qRCodeEncoder.Encode(contents, System.Text.Encoding.UTF8))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                //byte[] byteImage = ms.ToArray();
                                //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                                pictMRNQR.Image = System.Drawing.Image.FromStream(ms, true, true);
                                pictMRNQR.Visible = true;
                            }
                        }
                        #endregion
                    }
                }
            }


            #region Header
            cRegistrationNo.Text = entityReg.RegistrationNo;
            if (entityReg.BedCode == "")
            {
                cPatient.Text = string.Format("{0} | {1}", entityReg.MedicalNo, entityReg.PatientName);
            }
            else
            {
                cPatient.Text = string.Format("{0} | {1} | {2}", entityReg.MedicalNo, entityReg.PatientName, entityReg.BedCode);
            }
            cDOB.Text = string.Format("{0} / {1} / {2}", entityReg.DateOfBirthInString, entityReg.PatientAge, entityReg.Gender);
            cParamedicName.Text = entityReg.ParamedicName;
            cTransaction.Text = string.Format("{0} | {1}", entity.TransactionDateInString, entity.TransactionTime);
            cTransactionNo.Text = entity.TransactionNo;
            cTransactionType.Text = entityOrder.PrescriptionType;
            cRemarksHTML.Html = entityOrder.cfRemarks;
            Int32 CountIsRFlag = 0;
            foreach (vPrescriptionOrderDt p in lstEntity)
            {
                String IsRFlag = p.IsRFlag.ToString();
                if (IsRFlag == "True")
                {
                    CountIsRFlag += 1;
                }
            }
            cJmlItem.Text = CountIsRFlag.ToString();
            cFarmasi.Text = entity.ServiceUnitName;
            cPenjamin.Text = entityReg.BusinessPartnerName;
            #endregion

            //#region Page Header
            //cHeaderPatient.Text = string.Format("{0}", entityReg.MedicalNo);
            //cHeaderRegTrans.Text = string.Format("{0} | {1}", entity.RegistrationNo, entity.TransactionNo);
            //#endregion

            #region Report Footer
            if (entity.LastUpdatedBy != 0 && entity.LastUpdatedBy != null)
            {
                lblUser.Text = BusinessLayer.GetUserAttribute(entity.LastUpdatedBy).FullName;
            }
            else
            {
                lblUser.Text = BusinessLayer.GetUserAttribute(entity.CreatedBy).FullName;
            }
            #endregion

            base.InitializeReport(param);
        }

        private void xrTableCell19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell19.Text = (++lineNumber).ToString();
            detailID = Convert.ToInt32(GetCurrentColumnValue("ID"));
            if (detailID != oldDetailID)
            {
                totalAmount += Convert.ToDecimal(GetCurrentColumnValue("LineAmount"));
            }
        }

        private void GroupFooter2_AfterPrint(object sender, EventArgs e)
        {
            lineNumber = 0;
            oldDetailID = detailID;
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("TeamDtParamedicName").ToString() != null && GetCurrentColumnValue("TeamDtParamedicName").ToString() != "")
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void tabParamedicTeam_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("TeamDtParamedicName").ToString() != null && GetCurrentColumnValue("TeamDtParamedicName").ToString() != "")
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void cTotalLineAmount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cTotalLineAmount.Text = totalAmount.ToString("N2");
        }
    }
}
