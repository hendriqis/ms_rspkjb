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

namespace QIS.Medinfras.Web.Nursing.Program
{
    public partial class DomainClassEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nursing.DOMAIN_CLASS;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                vNursingDomainClass entity = BusinessLayer.GetvNursingDomainClassList(String.Format("NurseDomainClassID = {0}", Convert.ToInt32(ID))).FirstOrDefault();
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtNurseDomainClassCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.NURSING_DOMAIN_CLASS_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboNurseDomainClassType , lstStandardCode, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNurseDomainClassCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNurseDomainClassText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboNurseDomainClassType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarksText, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtParentName, new ControlEntrySetting(false, false, true));
        }

        protected string OnGetConstantNursingDomain()
        {
            return Constant.NursingDomainClassType.DOMAIN;
        }


        private void EntityToControl(vNursingDomainClass entity)
        {
            txtNurseDomainClassCode.Text = entity.NurseDomainClassCode;
            txtNurseDomainClassText.Text = entity.NurseDomainClassText;
            txtRemarksText.Text = entity.Remarks;
            cboNurseDomainClassType.Value = entity.GCNurseDomainClassType;
            hdnParentID.Value = entity.NurseDomainClassParentID.ToString();
            txtParentCode.Text = entity.NurseDomainClassParentCode;
            txtParentName.Text = entity.NurseDomainClassParentText;
        }

        private void ControlToEntity(NursingDomainClass entity)
        {
            entity.NurseDomainClassCode = txtNurseDomainClassCode.Text;
            entity.NurseDomainClassText = txtNurseDomainClassText.Text;
            entity.Remarks = txtRemarksText.Text;
            entity.GCNurseDomainClassType = cboNurseDomainClassType.Value.ToString();
            if (hdnParentID.Value != String.Empty)
                entity.NurseDomainClassParentID = Convert.ToInt32(hdnParentID.Value);
        }

        //protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        //{
        //    errMessage = string.Empty;

        //    string FilterExpression = string.Format("NurseDomainClassCode = '{0}'", txtNurseDomainClassCode.Text);
        //    List<NursingDomainClass> lst = BusinessLayer.GetNursingDomainClassList(FilterExpression);

        //    if (lst.Count > 0)
        //        errMessage = " Domain Class With Code " + txtNurseDomainClassCode.Text + " is already exist!";

        //    return (errMessage == string.Empty);
        //}

        //protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        //{
        //    errMessage = string.Empty;
        //    string FilterExpression = string.Format("NurseDomainClassCode = '{0}' AND NurseDomainClassID != {1}", txtNurseDomainClassCode.Text, hdnID.Value);
        //    List<NursingDomainClass> lst = BusinessLayer.GetNursingDomainClassList(FilterExpression);

        //    if (lst.Count > 0)
        //        errMessage = " Domain Class With Code " + txtNurseDomainClassCode.Text + " is already exist!";

        //    return (errMessage == string.Empty);
        //}

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            NursingDomainClassDao entityDao = new NursingDomainClassDao(ctx);
            bool result = false;
            try
            {
                NursingDomainClass entity = new NursingDomainClass();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetNursingDomainClassMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
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
                NursingDomainClass entity = BusinessLayer.GetNursingDomainClass(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingDomainClass(entity);
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