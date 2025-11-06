using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoPersalinanCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramData = param.Split('|');
            hdnRegistrationID.Value = paramData[0];
            txtRegistrationNo.Text = paramData[1];
            hdnVisitID.Value = paramData[2];

            vRegistrationBPJS oregBpjs = BusinessLayer.GetvRegistrationBPJSList(string.Format("RegistrationID='{0}'", hdnRegistrationID.Value)).FirstOrDefault();
            txtSepNo.Text = oregBpjs.NoSEP;
            txtPeserta.Text = oregBpjs.NoPeserta;
            hdnSepNo.Value = oregBpjs.NoSEP;
            hdnNoPeserta.Value = oregBpjs.NoPeserta;
            hdnRegistrationNo.Value = oregBpjs.RegistrationNo;

            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
            if (entity != null)
            {
                hdnMedicalNo.Value = entity.MedicalNo;
                hdnPatientName.Value = entity.PatientName;
            }

            BindGridView(); 
        }

        protected void cbpReportProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {

            int pageCount = 1;
            string errMessage = "";
            string result ="";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                result = param[0] +"|";
                if (param[0] == "printout")
                {
                    BindPrintout();
                    result = "printout";
                }else{
                 BindGridView();
                }
               
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string filterexpresion = string.Format("RegistrationID = '{0}' AND GCAssessmentType='X491^003' AND IsDeleted = 0 ORDER BY AssessmentID DESC ", hdnRegistrationID.Value);
            List<vPatientAssessment> lstReportMenu = BusinessLayer.GetvPatientAssessmentList(filterexpresion);
            grdReportMaster.DataSource = lstReportMenu;
            grdReportMaster.DataBind();
        }

        private void BindPrintout()
        {

                List<vPatientAssessment> lstPatient = BusinessLayer.GetvPatientAssessmentList(string.Format("RegistrationID = '{0}' AND GCAssessmentType='X491^003' AND IsDeleted = 0 ORDER BY AssessmentID DESC", hdnRegistrationID.Value));
                if (lstPatient.Count > 0)
                {
                    string strhtml = "";
                    List<string> content = new List<string>();
                    string readString = string.Empty;
                    string path = HttpContext.Current.Server.MapPath("~/Libs/App_Data/medicalForm/General");
                    string fileName = string.Format(@"{0}\GeneralPrint.html", path);

                    foreach (vPatientAssessment row in lstPatient)
                    {
                        string chk1 = "";
                        string chk2 = "";
                        string FormLayout = row.AssessmentFormLayout.ToString();
                        string ParamValue = row.AssessmentFormValue.ToString();
                        string contentLayout = Helper.ParsingFormElektronik(FormLayout, ParamValue, string.Empty);
                        if (row.IsInitialAssessment == true)
                        {
                            chk1 = string.Format("checked");
                        }
                        IEnumerable<string> lstText = File.ReadAllLines(fileName);
                        StringBuilder innerHtml = new StringBuilder();
                        string readText = File.ReadAllText(fileName);
                        readString = Regex.Replace(readText, "#date", string.Format("{0} {1}<br/>", row.cfAssessmentDate, row.AssessmentTime));
                        readString = Regex.Replace(readString, "#medicalno", string.Format("{0}<br/>", hdnMedicalNo.Value));
                        readString = Regex.Replace(readString, "#patientname", string.Format("{0}<br/>", hdnPatientName.Value));
                        readString = Regex.Replace(readString, "#ppa", string.Format("{0}<br/>", row.ParamedicName));
                        readString = Regex.Replace(readString, "#assessment", string.Format("{0}<br/>", row.AssessmentType));
                        readString = Regex.Replace(readString, "#assessmenttext", string.Format("{0}<br/>", row.AssessmentFormText));
                        readString = Regex.Replace(readString, "#content", string.Format("{0}<br/>", contentLayout));
                        readString = Regex.Replace(readString, "#checked1", string.Format("{0}", chk1));
                        readString = Regex.Replace(readString, "#checked2", string.Format("{0}", chk2));
                        strhtml = string.Format("{0}<br/>", readString);
                        content.Add(strhtml);
                    }
                    hdnFileString.Value = string.Join("", content);

                    #region createpdf
                    //StringReader sr = new StringReader(ovMCUResultForm.FormLayout.ToString());
                    //Document pdfDoc = new Document(PageSize.LETTER_LANDSCAPE);
                    //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    //using (MemoryStream memoryStream = new MemoryStream())
                    //{
                    //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    //    pdfDoc.Open();
                    //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    //    //htmlparser.Parse(sr);
                    //    pdfDoc.Close();

                    //    byte[] bytes = memoryStream.ToArray();
                    //    hdnFileString.Value = Convert.ToBase64String(bytes);
                    //    memoryStream.Close();
                    //}
                    #endregion
                }
            

        }

    }
}