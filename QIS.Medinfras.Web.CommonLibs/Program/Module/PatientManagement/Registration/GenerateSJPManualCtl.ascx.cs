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
    public partial class GenerateSJPManualCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            vRegistration1 entity = BusinessLayer.GetvRegistration1List(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
            hdnRegistrationIDCtl.Value = entity.RegistrationID.ToString();
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtPatient.Text = string.Format("({0}) {1}", entity.MedicalNo, entity.PatientName);

            InitializeControlProperties();
        }

        private void InitializeControlProperties()
        {
            List<vRegistrationInhealth1> entityInhealthList = BusinessLayer.GetvRegistrationInhealth1List(string.Format("RegistrationID = {0}", hdnRegistrationIDCtl.Value));
            if (entityInhealthList.Count() > 0)
            {
                IsAdd = false;
                vRegistrationInhealth1 entityInhealth = entityInhealthList.FirstOrDefault();
                txtNoSJP.Text = entityInhealth.NoSJP;

                if (entityInhealth.TanggalSJP.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                {
                    txtTglSJP.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }
                else
                {
                    txtTglSJP.Text = entityInhealth.TanggalSJP.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }

                if (entityInhealth.JamSJP == "" || entityInhealth.JamSJP == null)
                {
                    txtJamSJP.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                }
                else
                {
                    txtJamSJP.Text = entityInhealth.JamSJP;
                }
            }
            else
            {
                IsAdd = true;
                txtTglSJP.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtJamSJP.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNoSJP, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTglSJP, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtJamSJP, new ControlEntrySetting(true, true, true));
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationInhealthDao entityRegistrationInhealthDao = new RegistrationInhealthDao(ctx);
            try
            {
                RegistrationInhealth newRegInhealth = new RegistrationInhealth();
                newRegInhealth.RegistrationID = Convert.ToInt32(hdnRegistrationIDCtl.Value);
                newRegInhealth.NoSJP = txtNoSJP.Text;
                newRegInhealth.TanggalSJP = Helper.GetDatePickerValue(txtTglSJP.Text);
                newRegInhealth.JamSJP = txtJamSJP.Text;
                newRegInhealth.IsManualSJP = true;
                newRegInhealth.CreatedBy = AppSession.UserLogin.UserID;
                entityRegistrationInhealthDao.Insert(newRegInhealth);

                ctx.CommitTransaction();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationInhealthDao entityRegistrationInhealthDao = new RegistrationInhealthDao(ctx);
            try
            {
                RegistrationInhealth regInhealth = entityRegistrationInhealthDao.Get(Convert.ToInt32(hdnRegistrationIDCtl.Value));
                if (regInhealth != null)
                {
                    regInhealth.NoSJP = txtNoSJP.Text;
                    regInhealth.TanggalSJP = Helper.GetDatePickerValue(txtTglSJP.Text);
                    regInhealth.JamSJP = txtJamSJP.Text;
                    regInhealth.IsManualSJP = true;
                    regInhealth.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityRegistrationInhealthDao.Update(regInhealth);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, data tidak ditemukan.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
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