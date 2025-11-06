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
    public partial class MRVitalSignNewMRInRSSES : DevExpress.XtraReports.UI.XtraReport
    {
        public MRVitalSignNewMRInRSSES()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpressionSetvar = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.EM_VITALSIGN_AND_REVIEWOFSYSTEM_FROM_LINKED_REGISTRATION_IN_MEDICAL_RESUME);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpressionSetvar);
            String param = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_VITALSIGN_AND_REVIEWOFSYSTEM_FROM_LINKED_REGISTRATION_IN_MEDICAL_RESUME).FirstOrDefault().ParameterValue;

            if (param == "1")
            {
                ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", VisitID)).FirstOrDefault();
                Registration entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entityVisit.RegistrationID)).FirstOrDefault();

                if (entityRegistration.LinkedRegistrationID == 0 || entityRegistration.LinkedRegistrationID == null)
                {
                    string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                    vVitalSignHd entityHd = BusinessLayer.GetvVitalSignHdList(filterExpression).FirstOrDefault();
                    if (entityHd != null)
                    {
                        vVitalSignRSSES entityDt = BusinessLayer.GetvVitalSignRSSESList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID)).FirstOrDefault();
                    }
                    else
                    {
                        string filterExpressionDt = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                        vVitalSignRSSES entityDt = BusinessLayer.GetvVitalSignRSSESList(filterExpressionDt).FirstOrDefault();
                    }

                    if (entityHd != null)
                    {
                        List<vVitalSignHd> lstHd = BusinessLayer.GetvVitalSignHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                        List<vVitalSignRSSES> lstDt = BusinessLayer.GetvVitalSignRSSESList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                        var lst = (from p in lstHd
                                   select new
                                   {
                                       ObservationDateInString = p.ObservationDateInString,
                                       ObservationTime = p.cfObservationTimeRM,
                                       Remarks = p.Remarks,
                                       VitalSignDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                                   }).ToList();

                        this.DataSource = lst;
                        DetailReport.DataMember = "VitalSignDts";
                    }
                    else
                    {
                        xrLabel1.Visible = false;
                        xrLabel2.Visible = false;
                        xrLabel3.Visible = false;
                        xrLabel4.Visible = false;
                        xrLabel6.Visible = false;
                        xrLabel8.Visible = false;
                    }
                }
                else
                {
                    ConsultVisit entityLinkedentityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.LinkedRegistrationID)).FirstOrDefault();
                    string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", entityLinkedentityVisit.VisitID);
                    vVitalSignHd entityHd = BusinessLayer.GetvVitalSignHdList(filterExpression).FirstOrDefault();
                    if (entityHd != null)
                    {
                        if (entityHd != null)
                        {
                            vVitalSignRSSES entityDt = BusinessLayer.GetvVitalSignRSSESList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID)).FirstOrDefault();
                        }
                        else
                        {
                            string filterExpressionDt = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                            vVitalSignRSSES entityDt = BusinessLayer.GetvVitalSignRSSESList(filterExpressionDt).FirstOrDefault();
                        }

                        if (entityHd != null)
                        {
                            List<vVitalSignHd> lstHd = BusinessLayer.GetvVitalSignHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                            List<vVitalSignRSSES> lstDt = BusinessLayer.GetvVitalSignRSSESList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                            var lst = (from p in lstHd
                                       select new
                                       {
                                           ObservationDateInString = p.ObservationDateInString,
                                           ObservationTime = p.cfObservationTimeRM,
                                           Remarks = p.Remarks,
                                           VitalSignDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                                       }).ToList();

                            this.DataSource = lst;
                            DetailReport.DataMember = "VitalSignDts";
                        }
                        else
                        {
                            xrLabel1.Visible = false;
                            xrLabel2.Visible = false;
                            xrLabel3.Visible = false;
                            xrLabel4.Visible = false;
                            xrLabel6.Visible = false;
                            xrLabel8.Visible = false;
                        }
                    }
                    else
                    {
                        string filterExpression2 = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                        vVitalSignHd entityHd2 = BusinessLayer.GetvVitalSignHdList(filterExpression2).FirstOrDefault();
                        if (entityHd2 != null)
                        {
                            vVitalSignRSSES entityDt = BusinessLayer.GetvVitalSignRSSESList(string.Format("ID = {0} AND IsDeleted = 0", entityHd2.ID)).FirstOrDefault();
                        }
                        else
                        {
                            string filterExpressionDt = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                            vVitalSignRSSES entityDt = BusinessLayer.GetvVitalSignRSSESList(filterExpressionDt).FirstOrDefault();
                        }

                        if (entityHd2 != null)
                        {
                            List<vVitalSignHd> lstHd = BusinessLayer.GetvVitalSignHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd2.ID));
                            List<vVitalSignRSSES> lstDt = BusinessLayer.GetvVitalSignRSSESList(string.Format("ID = {0} AND IsDeleted = 0", entityHd2.ID));
                            var lst = (from p in lstHd
                                       select new
                                       {
                                           ObservationDateInString = p.ObservationDateInString,
                                           ObservationTime = p.cfObservationTimeRM,
                                           Remarks = p.Remarks,
                                           VitalSignDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                                       }).ToList();

                            this.DataSource = lst;
                            DetailReport.DataMember = "VitalSignDts";
                        }
                        else
                        {
                            xrLabel1.Visible = false;
                            xrLabel2.Visible = false;
                            xrLabel3.Visible = false;
                            xrLabel4.Visible = false;
                            xrLabel6.Visible = false;
                            xrLabel8.Visible = false;
                        }
                    }
                }
            }
            else
            {
                string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                vVitalSignHd entityHd = BusinessLayer.GetvVitalSignHdList(filterExpression).FirstOrDefault();
                if (entityHd != null)
                {
                    vVitalSignRSSES entityDt = BusinessLayer.GetvVitalSignRSSESList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID)).FirstOrDefault();
                }
                else
                {
                    string filterExpressionDt = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                    vVitalSignRSSES entityDt = BusinessLayer.GetvVitalSignRSSESList(filterExpressionDt).FirstOrDefault();
                }

                if (entityHd != null)
                {
                    List<vVitalSignHd> lstHd = BusinessLayer.GetvVitalSignHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                    List<vVitalSignRSSES> lstDt = BusinessLayer.GetvVitalSignRSSESList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                    var lst = (from p in lstHd
                               select new
                               {
                                   ObservationDateInString = p.ObservationDateInString,
                                   ObservationTime = p.cfObservationTimeRM,
                                   Remarks = p.Remarks,
                                   VitalSignDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                               }).ToList();

                    this.DataSource = lst;
                    DetailReport.DataMember = "VitalSignDts";
                }
                else
                {
                    xrLabel1.Visible = false;
                    xrLabel2.Visible = false;
                    xrLabel3.Visible = false;
                    xrLabel4.Visible = false;
                    xrLabel6.Visible = false;
                    xrLabel8.Visible = false;
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