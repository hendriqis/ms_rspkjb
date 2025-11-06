using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class PrinterLocationEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.PRINTER_LOCATION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            String[] param = Request.QueryString["id"].Split('|');
            if (param[0] == "edit")
            {
                IsAdd = false;
                string ID = param[1];
                txtStandardCodeID.Text = ID;
                StandardCode entity = BusinessLayer.GetStandardCode(ID);
                hdnParentID.Value = entity.ParentID;
                EntityToControl(entity);
            }
            else
            {
                hdnParentID.Value = Constant.StandardCode.LOKASI_PENDAFTARAN;
                txtNotes.Text = "Printer_Label|Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female;Printer_Wristband_Infant_Male;Printer_Wristband_Infant_Female|Printer_RegistrationSlip";
                IsAdd = true;
            }
            txtStandardCodeID.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtStandardCodeID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtStandardCodeName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPrinterLabel, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPrinterGelangDewasaL, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPrinterGelangDewasaP, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPrinterGelangAnakL, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPrinterGelangAnakP, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPrinterBayiBaruLahir, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPrinterBuktiPendaftaran, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(StandardCode entity)
        {
            txtStandardCodeID.Text = entity.StandardCodeID.Split('^')[1];
            txtStandardCodeName.Text = entity.StandardCodeName;
            txtNotes.Text = entity.Notes;

            string tagPropertyDB = entity.TagProperty;
            string[] tagPropertyText = tagPropertyDB.Split(new string[] { "|", ";" }, StringSplitOptions.None);
            foreach (string s in tagPropertyText)
            {
                txtPrinterLabel.Text = tagPropertyText[0];
                txtPrinterGelangDewasaL.Text = tagPropertyText[1];
                txtPrinterGelangDewasaP.Text = tagPropertyText[2];
                txtPrinterGelangAnakL.Text = tagPropertyText[3];
                txtPrinterGelangAnakP.Text = tagPropertyText[4];
                txtPrinterBayiBaruLahir.Text = tagPropertyText[5];
                txtPrinterBuktiPendaftaran.Text = tagPropertyText[6];
            }
        }

        private void ControlToEntity(StandardCode entity)
        {
            entity.StandardCodeName = txtStandardCodeName.Text;
            entity.TagProperty = hdnTagProperty.Value;
            entity.Notes = txtNotes.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("StandardCodeID = '{0}^{1}'", hdnParentID.Value, txtStandardCodeID.Text);
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(FilterExpression);

            if (lst.Count > 0)
            {
                errMessage = " Standard Code with ID " + txtStandardCodeID.Text + " is already exist!";
            }
            
            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                string pLabel = String.Format(@"{0}", txtPrinterLabel.Text);
                string pGelangDewasaL = String.Format(@"{0}", txtPrinterGelangDewasaL.Text);
                string pGelangDewasaP = String.Format(@"{0}", txtPrinterGelangDewasaP.Text);
                string pGelangAnakL = String.Format(@"{0}", txtPrinterGelangAnakL.Text);
                string pGelangAnakP = String.Format(@"{0}", txtPrinterGelangAnakP.Text);
                string pBayiBaruLahir = String.Format(@"{0}", txtPrinterBayiBaruLahir.Text);
                string pBukuPaduan = String.Format(@"{0}", txtPrinterBuktiPendaftaran.Text);

                string tagProperty = string.Format("{0}|{1};{2};{3};{4}|{5}|{6}", 
                    pLabel, pGelangDewasaL, pGelangDewasaP, pGelangAnakL, pGelangAnakP, pBayiBaruLahir, pBukuPaduan);
                hdnTagProperty.Value = tagProperty;

                StandardCode entity = new StandardCode();
                ControlToEntity(entity);
                entity.StandardCodeID = String.Format("{0}^{1}", hdnParentID.Value, txtStandardCodeID.Text);
                entity.ParentID = hdnParentID.Value;
                entity.IsEditableByUser = true;
                entity.IsHeader = false;
                entity.IsDefault = false;
                entity.IsActive = true;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertStandardCode(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                string pLabel = String.Format(@"{0}", txtPrinterLabel.Text);
                string pGelangDewasaL = String.Format(@"{0}", txtPrinterGelangDewasaL.Text);
                string pGelangDewasaP = String.Format(@"{0}", txtPrinterGelangDewasaP.Text);
                string pGelangAnakL = String.Format(@"{0}", txtPrinterGelangAnakL.Text);
                string pGelangAnakP = String.Format(@"{0}", txtPrinterGelangAnakP.Text);
                string pBayiBaruLahir = String.Format(@"{0}", txtPrinterBayiBaruLahir.Text);
                string pBukuPaduan = String.Format(@"{0}", txtPrinterBuktiPendaftaran.Text);

                string tagProperty = string.Format("{0}|{1};{2};{3};{4}|{5}|{6}",
                    pLabel, pGelangDewasaL, pGelangDewasaP, pGelangAnakL, pGelangAnakP, pBayiBaruLahir, pBukuPaduan);
                hdnTagProperty.Value = tagProperty;

                string StandardCodeID = string.Format("{0}^{1}", hdnParentID.Value, txtStandardCodeID.Text);
                StandardCode entity = BusinessLayer.GetStandardCode(StandardCodeID);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateStandardCode(entity);
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