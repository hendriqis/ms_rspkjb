using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.API.Model;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChangeBloodTypeEntryCtl : BaseProcessPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            hdnRegistrationIDCtl.Value = param;

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.BLOOD_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboBloodType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.BLOOD_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            List<Variable> lstBloodRhesus = new List<Variable>();
            lstBloodRhesus.Add(new Variable { Code = "+", Value = "+" });
            lstBloodRhesus.Add(new Variable { Code = "-", Value = "-" });
            Methods.SetComboBoxField<Variable>(cboBloodRhesus, lstBloodRhesus, "Value", "Code");

            Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationIDCtl.Value)).FirstOrDefault();
            if (entityReg.MRN != null && entityReg.MRN != 0)
            {
                vPatient entityPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entityReg.MRN)).FirstOrDefault();

                hdnMRNCtl.Value = entityPatient.MRN.ToString();
                txtMedicalNoCtl.Text = entityPatient.MedicalNo;
                txtPatientNameCtl.Text = entityPatient.PatientName;
                cboBloodType.Value = entityPatient.GCBloodType;
                cboBloodRhesus.Value = entityPatient.BloodRhesus;
            }

        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDao entityDao = new PatientDao(ctx);
            try
            {
                if (hdnMRNCtl.Value != "" && hdnMRNCtl.Value != "0")
                {
                    Patient entity = entityDao.Get(Convert.ToInt32(hdnMRNCtl.Value));
                    entity.GCBloodType = Helper.GetComboBoxValue(cboBloodType, true);
                    entity.BloodRhesus = Helper.GetComboBoxValue(cboBloodRhesus, true);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, registrasi ini bukan dari pasien yang memiliki nomor rekam medis.";
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