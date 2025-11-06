using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;
namespace QIS.Medinfras.ReportDesktop
{
    public partial class ImagingResultRptRSRT : BaseDailyPortraitRpt
    {
        public ImagingResultRptRSRT()
        {
            InitializeComponent();

            lblReportTitle.Visible = false;
            lineFooter.Visible = false;
            lblReportProperties.Visible = false;
            lblFooter.Text = string.Format("Print Date/Time {0}", DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT_2));
             
        }

        private void lblTestOrderInfo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (lblTestOrderInfo.Text.Contains(";"))
            {
                //RAI/20221001/00004;01-10-2022;03:30;dr. Herman Navis Nalapraya;INST. GAWAT DARURAT
                lblOrderParamedic.Text = lblTestOrderInfo.Text.Split(';')[3];
                string TestOrderNo = lblTestOrderInfo.Text.Split(';')[0];
                string ParamedicName = string.Empty;
                if (!string.IsNullOrEmpty(TestOrderNo)) {
                    string expression = string.Format("TestOrderNo='{0}'", TestOrderNo);
                    vImagingResultReport oResult = BusinessLayer.GetvImagingResultReportList(expression).FirstOrDefault();
                    if (oResult != null) 
                    {
                        vPatientChargesHd9 oTransaction = BusinessLayer.GetvPatientChargesHd9List(string.Format("TransactionID='{0}'", oResult.TransactionID)).FirstOrDefault();
                        if (oTransaction != null) {

                            if (oTransaction.IsOrderCreatedBySystem)
                            {
                                vConsultVisit5 oVisit = BusinessLayer.GetvConsultVisit5List(string.Format("VisitID='{0}'", oTransaction.VisitID)).FirstOrDefault();
                                if (oVisit.DepartmentID == Constant.Facility.IMAGING || oVisit.DepartmentID == Constant.Facility.DIAGNOSTIC)
                                {
                                    vRegistration3 registration = BusinessLayer.GetvRegistration3List(string.Format("RegistrationID = {0}", oVisit.RegistrationID)).FirstOrDefault();
                                    if (registration.ReferrerParamedicID > 0)
                                    {
                                        string strFilterExpression = string.Format("ParamedicID = '{0}'", registration.ReferrerParamedicID);
                                        ParamedicMaster pm = BusinessLayer.GetParamedicMasterList(strFilterExpression).FirstOrDefault();
                                        if (pm != null)
                                        {
                                            ParamedicName = pm.FullName;
                                        }
                                    }
                                    else
                                    {
                                        ParamedicName = registration.ReferrerName;
                                    }

                                    lblOrderParamedic.Text = ParamedicName;
                                    vPatientDiagnosis oDiagnosa = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID='{0}' and GCDiagnoseType='{1}' AND IsDeleted=0", oVisit.VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS)).FirstOrDefault();
                                    if (oDiagnosa != null)
                                    {
                                        if (!string.IsNullOrEmpty(oDiagnosa.DiagnoseName))
                                        {
                                            lblCatatanKlinis.Text = oDiagnosa.DiagnoseName;
                                        }
                                        else
                                        {
                                            lblCatatanKlinis.Text = oDiagnosa.DiagnosisText;
                                        }

                                    }
                                }
                            }
                            else 
                            {
                              lblCatatanKlinis.Text  = oResult.Remarks;
                            }
                           
                        }
                      
                    }
                }
               
            }
        }

        private void xrRichText1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
             XRRichText richText = (XRRichText)sender;

            using (DevExpress.XtraRichEdit.RichEditDocumentServer docServer = new DevExpress.XtraRichEdit.RichEditDocumentServer())
            {
                docServer.RtfText = richText.Rtf;
                docServer.Document.DefaultCharacterProperties.FontName = "Tahoma";
                docServer.Document.DefaultCharacterProperties.FontSize = 10;
                docServer.Document.DefaultCharacterProperties.Bold = false;
                richText.Rtf = docServer.RtfText;
            }  
        }

    }
}
