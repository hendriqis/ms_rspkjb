using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.Parameters;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRVitalSignNewMRInRSRA : DevExpress.XtraReports.UI.XtraReport
    {
        public MRVitalSignNewMRInRSRA()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpressionSetvar = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.EM_VITALSIGN_AND_REVIEWOFSYSTEM_FROM_LINKED_REGISTRATION_IN_MEDICAL_RESUME);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpressionSetvar);
            String param = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_VITALSIGN_AND_REVIEWOFSYSTEM_FROM_LINKED_REGISTRATION_IN_MEDICAL_RESUME).FirstOrDefault().ParameterValue;
            ConsultVisit entityVisitHd = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", VisitID)).FirstOrDefault();
            Registration entityRegistrationHd = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entityVisitHd.RegistrationID)).FirstOrDefault();
            vHealthcareServiceUnitCustom entityHsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", entityVisitHd.HealthcareServiceUnitID)).FirstOrDefault();

            if (entityHsu.DepartmentID == Constant.Facility.INPATIENT)
            {
                if (param == "1")
                {
                    ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", VisitID)).FirstOrDefault();
                    Registration entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entityVisit.RegistrationID)).FirstOrDefault();

                    if (entityRegistration.LinkedRegistrationID == 0 || entityRegistration.LinkedRegistrationID == null)
                    {
                        string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                        vVitalSignHd entityHd = BusinessLayer.GetvVitalSignHdList(filterExpression).FirstOrDefault();
                        vVitalSignDt entityDt = BusinessLayer.GetvVitalSignDtList(filterExpression).FirstOrDefault();

                        if (entityHd != null)
                        {
                            List<vVitalSignHd> lstHd = BusinessLayer.GetvVitalSignHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                            List<vVitalSignDt> lstDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                            var lst = (from p in lstHd
                                       select new
                                       {
                                           ObservationDateInString = p.ObservationDateInString,
                                           ObservationTime = p.cfObservationTimeRM,
                                           ParamedicName = p.cfParamedicNameRM,
                                           Remarks = p.Remarks,
                                           VitalSignDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                                       }).ToList();

                            lblVitalSignIN.Text = string.Format("{0} / {1} ({2})", entityHd.ObservationDateInString, entityHd.ObservationTime, entityHd.ParamedicName);

                            this.DataSource = lst;
                            DetailReport.DataMember = "VitalSignDts";
                        }
                        else
                        {
                            xrLabel1.Visible = false;
                            xrLabel5.Visible = false;
                            xrLabel3.Visible = false;
                            xrLabel4.Visible = false;
                            xrLabel6.Visible = false;
                            xrLabel7.Visible = false;
                            lblVitalSignIN.Visible = false;
                            xrLabel9.Visible = false;
                        }
                    }
                    else
                    {
                        ConsultVisit entityLinkedentityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.LinkedRegistrationID)).FirstOrDefault();
                        string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", entityLinkedentityVisit.VisitID);
                        vVitalSignHd entityHd = BusinessLayer.GetvVitalSignHdList(filterExpression).FirstOrDefault();
                        vVitalSignDt entityDt = BusinessLayer.GetvVitalSignDtList(filterExpression).FirstOrDefault();

                        if (entityHd != null)
                        {
                            List<vVitalSignHd> lstHd = BusinessLayer.GetvVitalSignHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                            List<vVitalSignDt> lstDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                            var lst = (from p in lstHd
                                       select new
                                       {
                                           ObservationDateInString = p.ObservationDateInString,
                                           ObservationTime = p.cfObservationTimeRM,
                                           ParamedicName = p.cfParamedicNameRM,
                                           Remarks = p.Remarks,
                                           VitalSignDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                                       }).ToList();

                            lblVitalSignIN.Text = string.Format("{0} / {1} ({2})", entityHd.ObservationDateInString, entityHd.ObservationTime, entityHd.ParamedicName);

                            this.DataSource = lst;
                            DetailReport.DataMember = "VitalSignDts";
                        }
                        else
                        {
                            xrLabel1.Visible = false;
                            xrLabel5.Visible = false;
                            xrLabel3.Visible = false;
                            xrLabel4.Visible = false;
                            xrLabel6.Visible = false;
                            xrLabel7.Visible = false;
                            lblVitalSignIN.Visible = false;
                            xrLabel9.Visible = false;
                        }
                    }
                }
                else
                {
                    string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                    vVitalSignHd entityHd = BusinessLayer.GetvVitalSignHdList(filterExpression).FirstOrDefault();
                    vVitalSignDt entityDt = BusinessLayer.GetvVitalSignDtList(filterExpression).FirstOrDefault();

                    if (entityHd != null)
                    {
                        List<vVitalSignHd> lstHd = BusinessLayer.GetvVitalSignHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                        List<vVitalSignDt> lstDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                        var lst = (from p in lstHd
                                   select new
                                   {
                                       ObservationDateInString = p.ObservationDateInString,
                                       ObservationTime = p.cfObservationTimeRM,
                                       ParamedicName = p.cfParamedicNameRM,
                                       Remarks = p.Remarks,
                                       VitalSignDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                                   }).ToList();

                        lblVitalSignIN.Text = string.Format("{0} / {1} ({2})", entityHd.ObservationDateInString, entityHd.ObservationTime, entityHd.ParamedicName);

                        this.DataSource = lst;
                        DetailReport.DataMember = "VitalSignDts";
                    }
                    else
                    {
                        xrLabel1.Visible = false;
                        xrLabel5.Visible = false;
                        xrLabel3.Visible = false;
                        xrLabel4.Visible = false;
                        xrLabel6.Visible = false;
                        xrLabel7.Visible = false;
                        lblVitalSignIN.Visible = false;
                        xrLabel9.Visible = false;
                    }
                }
            }
            else
            {
                if (param == "0")
                {
                    ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", VisitID)).FirstOrDefault();
                    Registration entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entityVisit.RegistrationID)).FirstOrDefault();

                    if (entityRegistration.LinkedRegistrationID == 0 || entityRegistration.LinkedRegistrationID == null)
                    {
                        string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                        vVitalSignHd entityHd = BusinessLayer.GetvVitalSignHdList(filterExpression).FirstOrDefault();
                        vVitalSignDt entityDt = BusinessLayer.GetvVitalSignDtList(filterExpression).FirstOrDefault();

                        if (entityHd != null)
                        {
                            List<vVitalSignHd> lstHd = BusinessLayer.GetvVitalSignHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                            List<vVitalSignDt> lstDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                            var lst = (from p in lstHd
                                       select new
                                       {
                                           ObservationDateInString = p.ObservationDateInString,
                                           ObservationTime = p.cfObservationTimeRM,
                                           ParamedicName = p.cfParamedicNameRM,
                                           Remarks = p.Remarks,
                                           VitalSignDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                                       }).ToList();

                            lblVitalSignIN.Text = string.Format("{0} / {1}", entityHd.ObservationDateInString, entityHd.ObservationTime);

                            this.DataSource = lst;
                            DetailReport.DataMember = "VitalSignDts";
                        }
                        else
                        {
                            xrLabel1.Visible = false;
                            xrLabel5.Visible = false;
                            xrLabel3.Visible = false;
                            xrLabel4.Visible = false;
                            xrLabel6.Visible = false;
                            xrLabel7.Visible = false;
                            lblVitalSignIN.Visible = false;
                            xrLabel9.Visible = false;
                        }
                    }
                    else
                    {
                        ConsultVisit entityLinkedentityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.LinkedRegistrationID)).FirstOrDefault();
                        string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", entityLinkedentityVisit.VisitID);
                        vVitalSignHd entityHd = BusinessLayer.GetvVitalSignHdList(filterExpression).FirstOrDefault();
                        vVitalSignDt entityDt = BusinessLayer.GetvVitalSignDtList(filterExpression).FirstOrDefault();

                        if (entityHd != null)
                        {
                            List<vVitalSignHd> lstHd = BusinessLayer.GetvVitalSignHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                            List<vVitalSignDt> lstDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                            var lst = (from p in lstHd
                                       select new
                                       {
                                           ObservationDateInString = p.ObservationDateInString,
                                           ObservationTime = p.cfObservationTimeRM,
                                           ParamedicName = p.cfParamedicNameRM,
                                           Remarks = p.Remarks,
                                           VitalSignDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                                       }).ToList();

                            lblVitalSignIN.Text = string.Format("{0} / {1}", entityHd.ObservationDateInString, entityHd.ObservationTime);

                            this.DataSource = lst;
                            DetailReport.DataMember = "VitalSignDts";
                        }
                        else
                        {
                            xrLabel1.Visible = false;
                            xrLabel5.Visible = false;
                            xrLabel3.Visible = false;
                            xrLabel4.Visible = false;
                            xrLabel6.Visible = false;
                            xrLabel7.Visible = false;
                            lblVitalSignIN.Visible = false;
                            xrLabel9.Visible = false;
                        }
                    }
                }
                else
                {
                    string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                    vVitalSignHd entityHd = BusinessLayer.GetvVitalSignHdList(filterExpression).FirstOrDefault();
                    vVitalSignDt entityDt = BusinessLayer.GetvVitalSignDtList(filterExpression).FirstOrDefault();

                    if (entityHd != null)
                    {
                        List<vVitalSignHd> lstHd = BusinessLayer.GetvVitalSignHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                        List<vVitalSignDt> lstDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                        var lst = (from p in lstHd
                                   select new
                                   {
                                       ObservationDateInString = p.ObservationDateInString,
                                       ObservationTime = p.cfObservationTimeRM,
                                       ParamedicName = p.cfParamedicNameRM,
                                       Remarks = p.Remarks,
                                       VitalSignDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                                   }).ToList();

                        lblVitalSignIN.Text = string.Format("{0} / {1}", entityHd.ObservationDateInString, entityHd.ObservationTime);

                        this.DataSource = lst;
                        DetailReport.DataMember = "VitalSignDts";
                    }
                    else
                    {
                        xrLabel1.Visible = false;
                        xrLabel5.Visible = false;
                        xrLabel3.Visible = false;
                        xrLabel4.Visible = false;
                        xrLabel6.Visible = false;
                        xrLabel7.Visible = false;
                        lblVitalSignIN.Visible = false;
                        xrLabel9.Visible = false;
                    }
                }
            }
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (DetailReport.RowCount == 0)
            {
                Detail.Visible = false;
            }
        }

    }
}