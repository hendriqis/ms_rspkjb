using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using System.IO;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class SupplierDocumentCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnSupplierID.Value = param;
            SupplierDocument supplierDocument = BusinessLayer.GetSupplierDocumentList(string.Format("SupplierID = {0}", hdnSupplierID.Value)).FirstOrDefault();
            BusinessPartners businessPartners = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = {0}", hdnSupplierID.Value)).FirstOrDefault();
            txtSupplierName.Text = businessPartners.BusinessPartnerName;

            SetControlProperties();

            BusinessPartners bp = BusinessLayer.GetBusinessPartners(businessPartners.BusinessPartnerID);

            string path = string.Format("{0}BusinessPartner\\{1}\\", AppConfigManager.QISVirtualDirectory.Replace('/', '\\'), bp.BusinessPartnerCode); ;
            hdnVirtualDirectory.Value = path;

            BindGridView();
        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}')", Constant.StandardCode.SUPPLIER_DOCUMENT_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCDocumentType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.SUPPLIER_DOCUMENT_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            txtDocumentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            Helper.SetControlEntrySetting(txtDocumentNo, new ControlEntrySetting(true, true, true), "mpEntryPopup");
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetSupplierDocumentList(string.Format("SupplierID = {0} AND IsDeleted = 0", hdnSupplierID.Value));
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnIsAdd.Value.ToString() == "0")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;

        }

        private void ControlToEntity(SupplierDocument entity)
        {
            entity.GCDocumentType = cboGCDocumentType.Value.ToString();
            entity.DocumentNo = txtDocumentNo.Text;
            entity.FileName = txtRename1.Text;
            entity.Remarks = txtNotes.Text;
            entity.IsDeleted = false;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                SupplierDocument entity = new SupplierDocument();
                BusinessPartners ec = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = {0}", hdnSupplierID.Value)).FirstOrDefault();

                string imageData = hdnUploadedFile1.Value;
                if (imageData != "")
                {
                    string[] parts = Regex.Split(imageData, ",").Skip(1).ToArray();
                    imageData = String.Join(",", parts);
                }

                string path = string.Format("{0}\\{1}\\{2}\\", AppConfigManager.QISPhysicalDirectory.Replace('/', '\\'), "BusinessPartner", ec.BusinessPartnerCode);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileName = String.Format("{0}", txtRename1.Text);
                FileStream fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                byte[] data = Convert.FromBase64String(imageData);
                bw.Write(data);
                bw.Close();

                ControlToEntity(entity);

                entity.SupplierID = Convert.ToInt32(hdnSupplierID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertSupplierDocument(entity);

                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                SupplierDocument entity = BusinessLayer.GetSupplierDocumentList(string.Format("ID = {0}", Convert.ToInt32(hdnID.Value))).FirstOrDefault();
                string filenameprev = entity.FileName;

                BusinessPartners ec = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = {0}", hdnSupplierID.Value)).FirstOrDefault();

                string path = string.Format("{0}\\{1}\\{2}\\", AppConfigManager.QISPhysicalDirectory.Replace('/', '\\'), "BusinessPartner", ec.BusinessPartnerCode);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                File.Delete(path + filenameprev);

                string imageDatacreate = hdnUploadedFile1.Value;
                if (imageDatacreate != "")
                {
                    string[] parts = Regex.Split(imageDatacreate, ",").Skip(1).ToArray();
                    imageDatacreate = String.Join(",", parts);
                }
                string fileName2 = String.Format("{0}", txtRename1.Text);
                FileStream fs2 = new FileStream(string.Format("{0}{1}", path, fileName2), FileMode.Create);
                BinaryWriter bw2 = new BinaryWriter(fs2);
                byte[] datacreate = Convert.FromBase64String(imageDatacreate);
                bw2.Write(datacreate);
                bw2.Close();

                entity.GCDocumentType = cboGCDocumentType.Value.ToString();
                entity.DocumentNo = txtDocumentNo.Text;
                entity.FileName = txtRename1.Text;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                BusinessLayer.UpdateSupplierDocument(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                SupplierDocument entity = BusinessLayer.GetSupplierDocumentList(string.Format("ID = {0}", Convert.ToInt32(hdnID.Value))).FirstOrDefault();

                BusinessPartners ec = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = {0}", hdnSupplierID.Value)).FirstOrDefault();

                string imageData = hdnUploadedFile1.Value;
                if (imageData != "")
                {
                    string[] parts = Regex.Split(imageData, ",").Skip(1).ToArray();
                    imageData = String.Join(",", parts);
                }

                string path = string.Format("{0}\\{1}\\{2}\\", AppConfigManager.QISPhysicalDirectory.Replace('/', '\\'), "BusinessPartner", ec.BusinessPartnerCode);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileName = String.Format("{0}", entity.FileName);

                File.Delete(path + fileName);

                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSupplierDocument(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}