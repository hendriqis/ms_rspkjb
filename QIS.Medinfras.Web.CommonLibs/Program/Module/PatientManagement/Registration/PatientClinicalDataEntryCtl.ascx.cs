using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientClinicalDataEntryCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            Registration oRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(param));
            Patient oPatient = BusinessLayer.GetPatient(Convert.ToInt32(oRegistration.MRN));
            
            hdnMRNCtl.Value = oPatient.MRN.ToString();
            txtMedicalNoCtl.Text = oPatient.MedicalNo;
            txtPatientNameCtl.Text = oPatient.FullName;
            chkIsHasInfectiousCtl.Checked = oPatient.IsHasInfectious;
            chkIsHasAllergyCtl.Checked = oPatient.IsHasAllergy;

            IsAdd = false;

        }
        
        protected override void OnControlEntrySetting()
        {
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDao oPatientDao = new PatientDao(ctx);

            try
            {
                Patient oPatient = oPatientDao.Get(Convert.ToInt32(hdnMRNCtl.Value));
                oPatient.IsHasInfectious = chkIsHasInfectiousCtl.Checked;
                oPatient.IsHasAllergy = chkIsHasAllergyCtl.Checked;
                oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                oPatientDao.Update(oPatient);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
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