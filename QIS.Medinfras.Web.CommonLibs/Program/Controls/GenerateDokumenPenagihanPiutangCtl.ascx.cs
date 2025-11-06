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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class GenerateDokumenPenagihanPiutangCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramData = param.Split('|');
            hdnInvoiceID.Value = paramData[0];
            txtInvoiceNo.Text = paramData[1];
            hdnMenuIDCtl.Value = paramData[2];

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
                if (param[0] == "save")
                {

                    if (onSaveData(ref errMessage))
                    {
                        result += "success|";
                    }
                    else { 
                        result += "fail|" + errMessage;
                    }    
                    
                }
                else // refresh
                {
                    BindGridView();
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string filterexpresion = string.Format("MenuID = '{0}' AND ISDELETED=0", hdnMenuIDCtl.Value);
            List<vReportByMenuMaster> lstReportMenu = BusinessLayer.GetvReportByMenuMasterList(filterexpresion);
            grdReportMaster.DataSource = lstReportMenu;
            grdReportMaster.DataBind();
        }

        private bool onSaveData( ref string errMessage)
        {
            IDbContext Linkctx = DbFactory.Configure("medinfrasLINK", true);
            ARInvoiceReportHDDao arinvoiceReportHdDao = new ARInvoiceReportHDDao(Linkctx);
            ARInvoiceReportDtDao arinvoiceReportDtDao = new ARInvoiceReportDtDao(Linkctx);


            try
            {
                ARInvoiceHd oInvoiceData = BusinessLayer.GetARInvoiceHdList(string.Format("ARInvoiceID= '{0}'", hdnInvoiceID.Value)).FirstOrDefault();
                if (oInvoiceData != null)
                {

                    //tambah data HD
                    ARInvoiceReportHD oHDData = new ARInvoiceReportHD();
                    oHDData.ARInvoiceID = oInvoiceData.ARInvoiceID;
                    oHDData.ARInvoiceNo = oInvoiceData.ARInvoiceNo;
                    oHDData.ARInvoiceDate = oInvoiceData.ARInvoiceDate;
                    oHDData.JobRequestBy = AppSession.UserLogin.UserID;
                    oHDData.JobRequestDate = DateTime.Now;
                    oHDData.JobStatus = 0;
                    oHDData.ReportCode = hdnReportCode.Value;
                    int ID = arinvoiceReportHdDao.InsertReturnPrimaryKeyID(oHDData);

                    List<vARInvoiceDt> lstDt = BusinessLayer.GetvARInvoiceDtList(string.Format("ARInvoiceID= '{0}'", hdnInvoiceID.Value));
                    if (lstDt.Count > 0)
                    {

                        foreach (vARInvoiceDt row in lstDt)
                        {
                            vRegistration1 oReg = BusinessLayer.GetvRegistration1List(string.Format("RegistrationID='{0}'", row.RegistrationID)).FirstOrDefault();

                            ARInvoiceReportDt oDt = new ARInvoiceReportDt();
                            oDt.ARInvoiceReportID = ID;
                            oDt.JobStatusDt = 0;
                            oDt.PaymentDetailID = row.PaymentDetailID;
                            oDt.PaymentID = row.PaymentID;
                            oDt.RegistrationID = row.RegistrationID;
                            oDt.RegistrationNo = row.RegistrationNo;
                            oDt.LinkedToRegistrationNo = row.LinkedRegistrationNo; 
                            oDt.LinkedToRegistrationID = Convert.ToInt32(oReg.LinkedToRegistrationID); 
                            arinvoiceReportDtDao.Insert(oDt);
                        }
                    }
                }

                Linkctx.CommitTransaction();

                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Linkctx.RollBackTransaction();

                return false;
            }
            return true;
        }

        
    }
}