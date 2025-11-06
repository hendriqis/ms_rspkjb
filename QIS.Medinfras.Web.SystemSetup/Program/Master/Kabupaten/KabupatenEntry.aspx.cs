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
    public partial class KabupatenEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.BPJS_KABUPATEN;
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
                Kabupaten entity = BusinessLayer.GetKabupatenList(string.Format("KabupatenID = {0}", ID))[0];
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
                SetControlProperties();
            }
            txtKodeKabupaten.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            String filterExpression1 = string.Format("ParentID IN ('{0}')", Constant.StandardCode.PROVINCE);
            List<StandardCode> lstState = BusinessLayer.GetStandardCodeList(filterExpression1);
            lstState.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboState, lstState, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtKodeKabupaten, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNama, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboState, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBPJSReferenceCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBPJSReferenceName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsCity, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(Kabupaten entity)
        {
            txtKodeKabupaten.Text = entity.KodeKabupaten;
            txtNama.Text = entity.NamaKabupaten;
            cboState.Text = entity.GCState;

            chkIsCity.Checked = entity.IsCity;
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

        private void ControlToEntity(Kabupaten entity)
        {
            entity.KodeKabupaten = txtKodeKabupaten.Text;
            entity.NamaKabupaten = txtNama.Text;
            entity.GCState = cboState.Value.ToString();


            entity.IsCity = chkIsCity.Checked;

            if (!string.IsNullOrEmpty(txtBPJSReferenceCode.Text))
            {
                entity.BPJSReferenceInfo = string.Format("{0}|{1}", txtBPJSReferenceCode.Text, txtBPJSReferenceName.Text);
            }
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("KodeKabupaten = '{0}' AND IsDeleted = 0", txtKodeKabupaten.Text);
            List<Kabupaten> lst = BusinessLayer.GetKabupatenList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Kabupaten with Code " + txtKodeKabupaten.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            KabupatenDao entityDao = new KabupatenDao(ctx);
            bool result = false;
            try
            {
                Kabupaten entity = new Kabupaten();
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
                Kabupaten entity = BusinessLayer.GetKabupaten(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateKabupaten(entity);
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