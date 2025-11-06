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
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.Imaging.Program
{
    public partial class ImagingTestResultDetailCtl : BaseEntryPopupCtl
    {
        protected string GCTemplateGroup = "";

        public override void InitializeDataControl(string param)
        {
            GCTemplateGroup = Constant.TemplateGroup.IMAGING;

            hdnItemID.Value = param;
            string[] par = param.Split('|');
            hdnItemID.Value = par[0];
            hdnID.Value = par[1];

            List<StandardCode> lstBorderLine = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.DIAGNOSTIC_RESULT_INTERPRETATION));
            Methods.SetComboBoxField<StandardCode>(cboBorderLine, lstBorderLine, "StandardCodeName", "StandardCodeID");
            //cboBorderLine.SelectedIndex = 0;

            ImagingResultDt entityDT = BusinessLayer.GetImagingResultDtList(string.Format("ID = {0} AND ItemID = {1}", hdnID.Value, hdnItemID.Value)).FirstOrDefault();
            if (entityDT != null)
            {
                EntityToControl(entityDT);
                IsAdd = false;
            }
            else
            {
                txtPhotoNumber.Text = "";
                txtTestResult.Text = "";
                txtFileName.Text = "";
            }
        }

        private void BindGridView()
        {
            
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            //BindGridView();
        }

        private void EntityToControl(ImagingResultDt entity)
        {
            cboBorderLine.Text = entity.GCImagingTestBorderline;
            txtPhotoNumber.Text = entity.PhotoNumber;
            txtTestResult.Text = entity.TestResult; 
            txtFileName.Text = entity.FileName;
        }

        private void ControlToEntity(ImagingResultDt entity)
        {
            entity.ID = Convert.ToInt32(hdnID.Value);
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            entity.GCImagingTestBorderline = Convert.ToString(cboBorderLine.Value);
            entity.PhotoNumber = txtPhotoNumber.Text;
            entity.TestResult = Helper.GetHTMLEditorText(txtTestResult);
            entity.FileName = txtFileName.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
                {
                    ImagingResultDt entityIRDT = new ImagingResultDt();
                    ControlToEntity(entityIRDT);
                    BusinessLayer.InsertImagingResultDt(entityIRDT);
                    return true;
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    return false;
                }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                ImagingResultDt entityDT = BusinessLayer.GetImagingResultDtList(string.Format("ID = {0} AND ItemID = {1}", hdnID.Value, hdnItemID.Value)).FirstOrDefault();
                ControlToEntity(entityDT);
                BusinessLayer.UpdateImagingResultDt(entityDT);
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