using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ChangeTestOrderSurgeryReportEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.CHANGE_TEST_ORDER_SURGERY_REPORT;
        }

        protected string GetPageTitle()
        {
            return BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void InitializeDataControl()
        {

        }

        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnPatientSurgeryID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnTestOrderIDFrom, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTestOrderNoFrom, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtTestOrderDateFrom, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnTestOrderIDTo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTestOrderNoTo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtTestOrderDateTo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnVisitID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnRegistrationID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRegistrationNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPatient, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtParamedicName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtReportDate, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtReportTime, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtStartTime, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtEndDate, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtEndTime, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDuration, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSurgeryNo, new ControlEntrySetting(false, false, false));
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientSurgeryDao patientSurgeryDao = new PatientSurgeryDao(ctx);
            PreSurgeryAssessmentDao preSurgeryAssessmentDao = new PreSurgeryAssessmentDao(ctx);
            PreAnesthesyAssessmentDao preAnesthesyAssessmentDao = new PreAnesthesyAssessmentDao(ctx);
            SurgeryAnesthesyStatusDao surgeryAnesthesyStatusDao = new SurgeryAnesthesyStatusDao(ctx);
            PostSurgeryInstructionDao postSurgeryInstructionDao = new PostSurgeryInstructionDao(ctx);

            if (type == "process")
            {
                try
                {
                    if (hdnPatientSurgeryID.Value != null && hdnPatientSurgeryID.Value != "" && hdnPatientSurgeryID.Value != "0")
                    {
                        if (hdnTestOrderIDTo.Value != null && hdnTestOrderIDTo.Value != "" && hdnTestOrderIDTo.Value != "0")
                        {
                            PatientSurgery pSurgery = patientSurgeryDao.Get(Convert.ToInt32(hdnPatientSurgeryID.Value));
                            pSurgery.TestOrderID = Convert.ToInt32(hdnTestOrderIDTo.Value);
                            pSurgery.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientSurgeryDao.Update(pSurgery);

                            string filterAssessment = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnTestOrderIDFrom.Value);

                            List<PreSurgeryAssessment> lstPreSurgery = BusinessLayer.GetPreSurgeryAssessmentList(filterAssessment, ctx);
                            foreach (PreSurgeryAssessment entityPreSurgery in lstPreSurgery)
                            {
                                entityPreSurgery.TestOrderID = Convert.ToInt32(hdnTestOrderIDTo.Value);
                                entityPreSurgery.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                preSurgeryAssessmentDao.Update(entityPreSurgery);
                            }

                            List<PreAnesthesyAssessment> lstPreAnesthesy = BusinessLayer.GetPreAnesthesyAssessmentList(filterAssessment, ctx);
                            foreach (PreAnesthesyAssessment entityPreAnesthesy in lstPreAnesthesy)
                            {
                                entityPreAnesthesy.TestOrderID = Convert.ToInt32(hdnTestOrderIDTo.Value);
                                entityPreAnesthesy.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                preAnesthesyAssessmentDao.Update(entityPreAnesthesy);
                            }

                            List<SurgeryAnesthesyStatus> lstSurgeryAnesthesyStatus = BusinessLayer.GetSurgeryAnesthesyStatusList(filterAssessment, ctx);
                            foreach (SurgeryAnesthesyStatus entitySurgeryAnesthesyStatus in lstSurgeryAnesthesyStatus)
                            {
                                entitySurgeryAnesthesyStatus.TestOrderID = Convert.ToInt32(hdnTestOrderIDTo.Value);
                                entitySurgeryAnesthesyStatus.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                surgeryAnesthesyStatusDao.Update(entitySurgeryAnesthesyStatus);
                            }

                            List<PostSurgeryInstruction> lstPostSurgeryInstruction = BusinessLayer.GetPostSurgeryInstructionList(filterAssessment, ctx);
                            foreach (PostSurgeryInstruction entityPostSurgeryInstruction in lstPostSurgeryInstruction)
                            {
                                entityPostSurgeryInstruction.TestOrderID = Convert.ToInt32(hdnTestOrderIDTo.Value);
                                entityPostSurgeryInstruction.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                postSurgeryInstructionDao.Update(entityPostSurgeryInstruction);
                            }

                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Harap pilih Order Tujuan terlebih dahulu.";
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Harap lengkapi Order Awal dan Order Tujuan terlebih dahulu.";
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
            }

            return result;
        }
    }
}