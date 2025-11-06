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
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChangeReferralManualCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            vRegistration1 entity = BusinessLayer.GetvRegistration1List(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
            hdnRegistrationIDCtl.Value = entity.RegistrationID.ToString();
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtPatient.Text = string.Format("({0}) {1}", entity.MedicalNo, entity.PatientName);

            InitializeControlProperties();
        }

        private void InitializeControlProperties()
        {
            IsAdd = false;

            #region Referral

            string filterExpression = string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REFERRAL);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboReferral, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.REFERRAL || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            string filterReg = string.Format("RegistrationID = {0}", hdnRegistrationIDCtl.Value);
            vRegistration reg = BusinessLayer.GetvRegistrationList(filterReg).FirstOrDefault();
            cboReferral.Value = reg.GCReferrerGroup;
            hdnReferrerID.Value = reg.ReferrerID.ToString();
            hdnReferrerParamedicID.Value = reg.ReferrerParamedicID.ToString();
            if (reg.ReferrerID != 0)
            {
                txtReferralDescriptionCode.Text = reg.ReferrerCommCode;
                txtReferralDescriptionName.Text = reg.ReferrerName;
            }
            else if (reg.ReferrerParamedicID != 0)
            {
                ParamedicMaster pm = BusinessLayer.GetParamedicMaster(reg.ReferrerParamedicID);
                txtReferralDescriptionCode.Text = pm.ParamedicCode;
                txtReferralDescriptionName.Text = pm.FullName;
            }
            txtReferralNo.Text = reg.ReferralNo;

            #endregion
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboReferral, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnReferrerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnReferrerParamedicID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtReferralDescriptionCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtReferralDescriptionName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtReferralNo, new ControlEntrySetting(true, true, false));
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            RegistrationBPJSDao entityRegistrationBPJSDao = new RegistrationBPJSDao(ctx);
            try
            {
                Registration reg = entityRegistrationDao.Get(Convert.ToInt32(hdnRegistrationIDCtl.Value));
                if (cboReferral.Value != null)
                {
                    reg.GCReferrerGroup = cboReferral.Value.ToString();
                }
                else
                {
                    reg.GCReferrerGroup = null;
                }

                if (hdnReferrerID.Value != "" && hdnReferrerID.Value != "0")
                {
                    reg.ReferrerID = Convert.ToInt32(hdnReferrerID.Value);
                    reg.ReferrerParamedicID = null;
                }
                else if (hdnReferrerParamedicID.Value != "" && hdnReferrerParamedicID.Value != "0")
                {
                    reg.ReferrerID = null;
                    reg.ReferrerParamedicID = Convert.ToInt32(hdnReferrerParamedicID.Value);
                }
                else
                {
                    reg.ReferrerID = null;
                    reg.ReferrerParamedicID = null;
                }

                reg.ReferralNo = txtReferralNo.Text;
                reg.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityRegistrationDao.Update(reg);

                if (reg.GCCustomerType == Constant.CustomerType.BPJS)
                {
                    RegistrationBPJS entityRegBPJS = BusinessLayer.GetRegistrationBPJSList(string.Format("RegistrationID = {0}", hdnRegistrationIDCtl.Value)).FirstOrDefault();
                    if (entityRegBPJS != null)
                    {
                        entityRegBPJS.NoRujukan = txtReferralNo.Text;
                        entityRegBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityRegBPJS.LastUpdatedDate = DateTime.Now;

                        entityRegistrationBPJSDao.Update(entityRegBPJS);
                    }
                }

                ctx.CommitTransaction();
                retval = reg.RegistrationNo;
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
    }
}