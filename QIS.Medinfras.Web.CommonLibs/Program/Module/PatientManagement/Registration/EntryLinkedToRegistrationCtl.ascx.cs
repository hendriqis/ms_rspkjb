using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class EntryLinkedToRegistrationCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration = 1 AND IsActive = 1 AND DepartmentID NOT IN ('{0}')", Constant.Facility.MEDICAL_CHECKUP));
            Methods.SetComboBoxField<Department>(cboDepartmentID, lstDept, "DepartmentID", "DepartmentID");
            cboDepartmentID.Value = Constant.Facility.INPATIENT;

            vRegistration10 entity = BusinessLayer.GetvRegistration10List(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
            hdnRegistrationIDCtl.Value = entity.RegistrationID.ToString();
            txtRegistrationNoCtl.Text = entity.RegistrationNo;
            txtPatientCtl.Text = string.Format("({0}) {1}", entity.MedicalNo, entity.PatientName);
            txtServiceUnitCtl.Text = string.Format("{0} || {1}", entity.DepartmentID, entity.ServiceUnitName);
            txtParamedicCtl.Text = entity.ParamedicName;
            txtBusinessPartnerCtl.Text = entity.BusinessPartnerName;
            txtSEPNoCtl.Text = entity.NoSEP;

            IsAdd = false;

            if (entity.LinkedToRegistrationID != null && entity.LinkedToRegistrationID != 0)
            {
                hdnLinkedToRegistrationIDCtlORI.Value = hdnLinkedToRegistrationIDCtl.Value = entity.LinkedToRegistrationID.ToString();

                vRegistration10 entityLinkedTo = BusinessLayer.GetvRegistration10List(string.Format("RegistrationID = {0}", entity.LinkedToRegistrationID)).FirstOrDefault();
                txtLinkedToRegistrationNoCtl.Text = entityLinkedTo.RegistrationNo;
                txtLinkedToPatientCtl.Text = string.Format("({0}) {1}", entityLinkedTo.MedicalNo, entityLinkedTo.PatientName);
                txtLinkedToServiceUnitCtl.Text = string.Format("{0} || {1}", entityLinkedTo.DepartmentID, entityLinkedTo.ServiceUnitName);
                txtLinkedToParamedicCtl.Text = entityLinkedTo.ParamedicName;
                txtLinkedToBusinessPartnerCtl.Text = entityLinkedTo.BusinessPartnerName;
                txtLinkedToSEPNoCtl.Text = entityLinkedTo.NoSEP;
            }
            else
            {
                hdnLinkedToRegistrationIDCtlORI.Value = null;
                hdnLinkedToRegistrationIDCtl.Value = null;
                txtLinkedToRegistrationNoCtl.Text = null;
                txtLinkedToPatientCtl.Text = null;
                txtLinkedToServiceUnitCtl.Text = null;
                txtLinkedToParamedicCtl.Text = null;
                txtLinkedToBusinessPartnerCtl.Text = null;
                txtLinkedToSEPNoCtl.Text = null;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnLinkedToRegistrationIDCtl, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtLinkedToRegistrationNoCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtLinkedToPatientCtl, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtLinkedToServiceUnitCtl, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtLinkedToParamedicCtl, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtLinkedToBusinessPartnerCtl, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtLinkedToSEPNoCtl, new ControlEntrySetting(false, false, true));
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);

            try
            {
                Registration entityRegistration = entityRegistrationDao.Get(Convert.ToInt32(hdnRegistrationIDCtl.Value));
                if (entityRegistration.GCRegistrationStatus != Constant.VisitStatus.CANCELLED && entityRegistration.GCRegistrationStatus != Constant.VisitStatus.CLOSED)
                {
                    if (!entityRegistration.IsChargesTransfered)
                    {
                        if (hdnLinkedToRegistrationIDCtl.Value != null && hdnLinkedToRegistrationIDCtl.Value != "" && hdnLinkedToRegistrationIDCtl.Value != "0")
                        {
                            #region Ubah Link To Registration

                            // UPDATE LinkedToRegistrationID
                            entityRegistration.LinkedToRegistrationID = Convert.ToInt32(hdnLinkedToRegistrationIDCtl.Value);
                            entityRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityRegistrationDao.Update(entityRegistration);

                            if (hdnLinkedToRegistrationIDCtlORI.Value != null && hdnLinkedToRegistrationIDCtlORI.Value != "" && hdnLinkedToRegistrationIDCtlORI.Value != "0")
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                // UPDATE LinkedRegistrationID ORI
                                Registration entityLinkedRegistrationORI = entityRegistrationDao.Get(Convert.ToInt32(hdnLinkedToRegistrationIDCtlORI.Value));
                                entityLinkedRegistrationORI.LinkedRegistrationID = null;
                                entityLinkedRegistrationORI.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityRegistrationDao.Update(entityLinkedRegistrationORI);
                            }

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            // UPDATE LinkedRegistrationID
                            Registration entityLinkedRegistration = entityRegistrationDao.Get(Convert.ToInt32(hdnLinkedToRegistrationIDCtl.Value));
                            entityLinkedRegistration.LinkedRegistrationID = Convert.ToInt32(hdnRegistrationIDCtl.Value);
                            entityLinkedRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityRegistrationDao.Update(entityLinkedRegistration);

                            ctx.CommitTransaction();

                            #endregion
                        }
                        else
                        {
                            #region Hapus Link To Registration

                            // UPDATE LinkedToRegistrationID
                            entityRegistration.LinkedToRegistrationID = null;
                            entityRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityRegistrationDao.Update(entityRegistration);

                            if (hdnLinkedToRegistrationIDCtlORI.Value != null && hdnLinkedToRegistrationIDCtlORI.Value != "" && hdnLinkedToRegistrationIDCtlORI.Value != "0")
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                // UPDATE LinkedRegistrationID ORI
                                Registration entityLinkedRegistrationORI = entityRegistrationDao.Get(Convert.ToInt32(hdnLinkedToRegistrationIDCtlORI.Value));
                                entityLinkedRegistrationORI.LinkedRegistrationID = null;
                                entityLinkedRegistrationORI.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityRegistrationDao.Update(entityLinkedRegistrationORI);
                            }

                            ctx.CommitTransaction();

                            #endregion
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, data Registrasi dgn nomor <b>" + entityRegistration.RegistrationNo + "</b> transaksi & tagihannya sudah ditransfer.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, data Registrasi dgn nomor <b>" + entityRegistration.RegistrationNo + "</b> statusnya sudah VOID / CLOSED.";
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