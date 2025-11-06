using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program.PatientPage
{
    public partial class ViewLaboratoryResultCtl1 : BaseContentPopupCtl
    {
        protected string GCTemplateGroup = "";

        public override void InitializeControl(string param)
        {
            GCTemplateGroup = Constant.TemplateGroup.IMAGING;

            string[] par = param.Split('|');
            hdnItemIDCBCtl.Value = par[0];
            hdnTransactionIDCBCtl.Value = par[1];

            vPatientChargesDtLaboratoryResult oDetail = BusinessLayer.GetvPatientChargesDtLaboratoryResultList(string.Format("TransactionID = {0} AND ItemID = {1}", hdnTransactionIDCBCtl.Value, hdnItemIDCBCtl.Value)).FirstOrDefault();
            if (oDetail != null)
	        {
                hdnIDCBCtl.Value = oDetail.ID.ToString();
                EntityToControl(oDetail);
	        }
        }

        private void EntityToControl(vPatientChargesDtLaboratoryResult entity)
        {
            //txtMedicalNo.Text = entity.MedicalNo;
            //txtDateOfBirth.Text = entity.cfDateOfBirth2;
            //txtPatientName.Text = entity.PatientName;
            txtItemName.Text = entity.ItemName1;
            //txtReferenceNo.Text = entity.DetailReferenceNo;
            //hdnReferenceNoCBCtl.Value = txtReferenceNo.Text;
            //txtPhotoNumber.Text = entity.PhotoNumber;
            //contentIndonesia.InnerHtml = entity.TestResult1;
            //contentEnglish.InnerHtml = entity.TestResult2;
        }

        public string GetImagingResultImage()
        {
            string result = "";
            result = GeneratePreviewUrl(hdnReferenceNoCBCtl.Value);
            return result;
        }

        private string GeneratePreviewUrl(string referenceNo)
        {
            string result = "";
            string postData = referenceNo;

            string url = string.Format("{0}?{1}", AppSession.RIS_WEB_VIEW_URL, postData);
            if (AppSession.RIS_HL7_MESSAGE_FORMAT == Constant.RIS_HL7MessageFormat.MEDAVIS)
            {
                url = string.Format("{0}{1}", AppSession.RIS_WEB_VIEW_URL, referenceNo);
            }
            else
            {
                #region Post Parameter Data
                TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                string consID = referenceNo;
                string pass = AppSession.RIS_Consumer_Pwd;
                string data = unixTimestamp.ToString() + consID;

                postData = string.Format("X-cons-id={0}&X-timestamp={1}&X-signature={2}", consID, unixTimestamp.ToString(), HttpUtility.UrlEncode(Methods.GenerateSignature(data, pass)));

                UTF8Encoding encoding = new UTF8Encoding();
                byte[] postDataBytes = encoding.GetBytes(postData);
                #endregion

                url = string.Format("{0}?{1}", AppSession.RIS_WEB_VIEW_URL, postData);
            }

            return result = string.Format("{0}|{1}", "1", url);
        }

        #region Temporarary Obsolete Methods
        private string GeneratePreviewUrl1(string referenceNo)
        {
            string result = "";

            #region Post Parameter Data
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string consID = referenceNo;
            string pass = AppSession.RIS_Consumer_Pwd;
            string data = unixTimestamp.ToString() + consID;

            System.Collections.Specialized.NameValueCollection reqParam = new System.Collections.Specialized.NameValueCollection();
            reqParam.Add("X-cons-id", "123999"); //123999 is hard-coded accession number
            reqParam.Add("X-timestamp", pass);
            reqParam.Add("X-signature", Methods.GenerateSignature(data, pass));
            #endregion

            string remoteUrl = "http://172.17.200.22/medixsoftquickview/viewer.aspx";

            string html = "<html><head>";
            html += "</head><body onload='document.forms[0].submit()'>";
            html += String.Format("<form name='PostForm' method='POST' action='{0}'>", remoteUrl);
            foreach (string key in reqParam.Keys)
            {
                html += String.Format("<input name='{0}' type='hidden' value='{1}'>", key, reqParam[key]);
            }
            html += "</form></body></html>";

            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");
            Response.HeaderEncoding = Encoding.GetEncoding("ISO-8859-1");
            Response.Charset = "ISO-8859-1";
            Response.Write(html);
            Response.End();

            return result = string.Format("{0}|{1}", "1", "");
        }
        private string GeneratePreviewUrl2(string referenceNo)
        {
            string result = "";

            #region Post Parameter Data
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string consID = referenceNo;
            string pass = AppSession.RIS_Consumer_Pwd;
            string data = unixTimestamp.ToString() + consID;

            string postData = string.Format("x-consid={0}&x-timestamp={1}&x-signature={2}", consID, pass, data);

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postDataBytes = encoding.GetBytes(postData);
            #endregion

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", "http://172.17.200.22/medixsoftquickview/viewer.aspx"));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataBytes.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(postDataBytes, 0, postDataBytes.Length);
            }

            WebResponse response = (WebResponse)request.GetResponse();
            string responseMsg = string.Empty;
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                responseMsg = sr.ReadToEnd();
            };

            return result = string.Format("{0}|{1}", "1", responseMsg);
        } 
        #endregion
    }
}