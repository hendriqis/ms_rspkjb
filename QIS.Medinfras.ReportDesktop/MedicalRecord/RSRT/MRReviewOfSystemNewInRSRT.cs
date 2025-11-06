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
    public partial class MRReviewOfSystemNewInRSRT : DevExpress.XtraReports.UI.XtraReport
    {
        public MRReviewOfSystemNewInRSRT()
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
                    string filterExpression = string.Format("VisitID IN ({0}) AND GCParamedicMasterType = '{1}' AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID, Constant.ParamedicType.Physician);
                    vReviewOfSystemHd entityHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression).FirstOrDefault();
                    if (entityHd != null)
                    {
                        vReviewOfSystemDt entityDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0}", entityHd.ID)).FirstOrDefault();
                    }
                    else
                    {
                        string filterExpressionDt = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                        vReviewOfSystemDt entityDt = BusinessLayer.GetvReviewOfSystemDtList(filterExpressionDt).FirstOrDefault();
                    }

                    if (entityHd != null)
                    {
                        List<vReviewOfSystemHd> lstHd = BusinessLayer.GetvReviewOfSystemHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                        List<vReviewOfSystemDt> lstDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                        var lst = (from p in lstHd
                                   select new
                                   {
                                       ObservationDateInString = p.ObservationDateInString,
                                       ObservationTime = p.ObservationTime,
                                       ParamedicName = p.ParamedicName,
                                       ReviewOfSystemDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                                   }).ToList();

                        this.DataSource = lst;
                        DetailReport.DataMember = "ReviewOfSystemDts";
                    }
                    else
                    {
                        xrLabel1.Visible = false;
                        xrLabel3.Visible = false;
                        xrLabel4.Visible = false;
                        xrLabel6.Visible = false;
                        xrLabel8.Visible = false;
                    }
                }
                else
                {
                    ConsultVisit entityLinkedentityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.LinkedRegistrationID)).FirstOrDefault();
                    string filterExpression = string.Format("VisitID IN ({0}) AND GCParamedicMasterType = '{1}' AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", entityLinkedentityVisit.VisitID, Constant.ParamedicType.Physician);
                    vReviewOfSystemHd entityHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression).FirstOrDefault();
                    if (entityHd != null)
                    {
                        if (entityHd != null)
                        {
                            vReviewOfSystemDt entityDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0}", entityHd.ID)).FirstOrDefault();
                        }
                        else
                        {
                            string filterExpressionDt = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                            vReviewOfSystemDt entityDt = BusinessLayer.GetvReviewOfSystemDtList(filterExpressionDt).FirstOrDefault();
                        }

                        if (entityHd != null)
                        {
                            List<vReviewOfSystemHd> lstHd = BusinessLayer.GetvReviewOfSystemHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                            List<vReviewOfSystemDt> lstDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                            var lst = (from p in lstHd
                                       select new
                                       {
                                           ObservationDateInString = p.ObservationDateInString,
                                           ObservationTime = p.ObservationTime,
                                           ParamedicName = p.ParamedicName,
                                           ReviewOfSystemDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                                       }).ToList();

                            this.DataSource = lst;
                            DetailReport.DataMember = "ReviewOfSystemDts";
                        }
                        else
                        {
                            xrLabel1.Visible = false;
                            xrLabel3.Visible = false;
                            xrLabel4.Visible = false;
                            xrLabel6.Visible = false;
                            xrLabel8.Visible = false;
                        }
                    }
                    else
                    {
                        string filterExpression2 = string.Format("VisitID IN ({0}) AND GCParamedicMasterType = '{1}' AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID, Constant.ParamedicType.Physician);
                        vReviewOfSystemHd entityHd2 = BusinessLayer.GetvReviewOfSystemHdList(filterExpression2).FirstOrDefault();
                        if (entityHd2 != null)
                        {
                            vReviewOfSystemDt entityDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0}", entityHd2.ID)).FirstOrDefault();
                        }
                        else
                        {
                            string filterExpressionDt = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                            vReviewOfSystemDt entityDt = BusinessLayer.GetvReviewOfSystemDtList(filterExpressionDt).FirstOrDefault();
                        }

                        if (entityHd2 != null)
                        {
                            List<vReviewOfSystemHd> lstHd = BusinessLayer.GetvReviewOfSystemHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd2.ID));
                            List<vReviewOfSystemDt> lstDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND IsDeleted = 0", entityHd2.ID));
                            var lst = (from p in lstHd
                                       select new
                                       {
                                           ObservationDateInString = p.ObservationDateInString,
                                           ObservationTime = p.ObservationTime,
                                           ParamedicName = p.ParamedicName,
                                           ReviewOfSystemDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                                       }).ToList();

                            this.DataSource = lst;
                            DetailReport.DataMember = "ReviewOfSystemDts";
                        }
                        else
                        {
                            xrLabel1.Visible = false;
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
                string filterExpression = string.Format("VisitID IN ({0}) AND GCParamedicMasterType = '{1}' AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID, Constant.ParamedicType.Physician);
                vReviewOfSystemHd entityHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression).FirstOrDefault();
                if (entityHd != null)
                {
                    vReviewOfSystemDt entityDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0}", entityHd.ID)).FirstOrDefault();
                }
                else
                {
                    string filterExpressionDt = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0 ORDER BY ID", VisitID);
                    vReviewOfSystemDt entityDt = BusinessLayer.GetvReviewOfSystemDtList(filterExpressionDt).FirstOrDefault();
                }

                if (entityHd != null)
                {
                    List<vReviewOfSystemHd> lstHd = BusinessLayer.GetvReviewOfSystemHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                    List<vReviewOfSystemDt> lstDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                    var lst = (from p in lstHd
                               select new
                               {
                                   ObservationDateInString = p.ObservationDateInString,
                                   ObservationTime = p.ObservationTime,
                                   ParamedicName = p.ParamedicName,
                                   ReviewOfSystemDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                               }).ToList();

                    this.DataSource = lst;
                    DetailReport.DataMember = "ReviewOfSystemDts";
                }
                else
                {
                    xrLabel1.Visible = false;
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
