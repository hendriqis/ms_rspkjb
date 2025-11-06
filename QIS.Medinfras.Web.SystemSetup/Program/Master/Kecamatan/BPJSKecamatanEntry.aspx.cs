using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class BPJSKecamatanEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.BPJS_KECAMATAN;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                vBPJSKecamatan entity = BusinessLayer.GetvBPJSKecamatanList(string.Format("KecamatanID = {0}", ID))[0];
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtKodeKecamatan.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            }


        protected override void SetControlProperties()
        {
            String filterExpression1 = string.Format("ParentID IN ('{0}')", Constant.StandardCode.PROVINCE);
            List<StandardCode> lstState = BusinessLayer.GetStandardCodeList(filterExpression1);
            lstState.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtKodeKecamatan, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtNamaKecamatan, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKabupatenID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnKabupatenID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNamaKabupaten, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBPJSReferenceCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBPJSReferenceName, new ControlEntrySetting(true, true, true));
        }


        private void EntityToControl(vBPJSKecamatan entity)
        {
            txtKodeKecamatan.Text = entity.KodeKecamatan;
            txtNamaKecamatan.Text = entity.NamaKecamatan;
            txtNamaKabupaten.Text = entity.NamaKabupaten;
            hdnKabupatenID.Value = Convert.ToString(entity.KabupatenID);
            if (!String.IsNullOrEmpty(entity.BPJSReferenceInfo))
            {
                if (entity.BPJSReferenceInfo.Contains("|"))
                {
                    string[] bpjsInfo = entity.BPJSReferenceInfo.Split('|');
                    txtBPJSReferenceCode.Text = bpjsInfo[0];
                    txtBPJSReferenceName.Text = bpjsInfo[1];
                }
            }
            else
            {
                txtBPJSReferenceCode.Text = string.Empty;
                txtBPJSReferenceName.Text = string.Empty;
            }
        }

        private void ControlToEntity(Kecamatan entity)
        {
            entity.BPJSReferenceInfo = txtBPJSReferenceInfo.Text;
            entity.KabupatenID = Convert.ToInt32(hdnKabupatenID.Value);
            entity.KodeKecamatan = txtKodeKecamatan.Text;
            entity.NamaKecamatan = txtNamaKecamatan.Text;

            if (!string.IsNullOrEmpty(txtBPJSReferenceCode.Text))
            {
                entity.BPJSReferenceInfo = string.Format("{0}|{1}", txtBPJSReferenceCode.Text, txtBPJSReferenceName.Text);
            }
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("KodeKecamatan = '{0}'", txtKodeKecamatan.Text);
            List<vBPJSKecamatan> lst = BusinessLayer.GetvBPJSKecamatanList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Kecamatan with Code " + txtKodeKecamatan.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("KodeKecamatan = '{0}'", ID);
            List<vBPJSKecamatan> lst = BusinessLayer.GetvBPJSKecamatanList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Kecamatan with Code " + txtKodeKecamatan.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            KecamatanDao entityDao = new KecamatanDao(ctx);
            bool result = false;
            try
            {
                Kecamatan entity = new Kecamatan();
                ControlToEntity(entity);

                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                Kecamatan entity = BusinessLayer.GetKecamatan(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateKecamatan(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}