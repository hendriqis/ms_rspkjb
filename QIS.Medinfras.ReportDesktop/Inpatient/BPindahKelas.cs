using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPindahKelas : BaseCustomDailyPotraitRpt
    {
        public BPindahKelas()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0])[0];
            string MRN = string.Format("{0}", entityReg.MRN);
            vPatient entityPat = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", MRN))[0];
            vPatientFamily entityFam = BusinessLayer.GetvPatientFamilyList(string.Format("MRN = {0} AND IsDeleted = 0", MRN)).FirstOrDefault();
            EmergencyContact entityEC = BusinessLayer.GetEmergencyContactList(string.Format("RegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();

            string FirstName = entityPat.FirstName;
            string MiddleName = entityPat.MiddleName;
            string LastName = entityPat.LastName;
            string NoReg = entityReg.RegistrationNo;
            string RM = entityPat.MedicalNo;
            int AgeYear = entityPat.AgeInYear;
            int AgeMonth = entityPat.AgeInMonth;
            int AgeDays = entityPat.AgeInDay;

            lblNoReg.Text = string.Format("{0}", NoReg);
            lblNama.Text = string.Format("{0} {1} {2} | {3}", FirstName, MiddleName, LastName, RM);
            lblUmur.Text = string.Format("{0} Tahun {1} Bulan {2} Hari", AgeYear, AgeMonth, AgeDays);

            if (entityFam == null)
            {
                if (entityEC != null)
                {
                    lblTTD.Text = entityEC.ContactName;
                }
            }
            else
            {
                lblTTD.Text = entityFam.FullName;
            }

            //if (entityFam != null)
            //{
            //    if (entityFam.FamilyID == null || entityFam.FamilyID == 0)
            //    {
            //        lblTTD.Text = entityFam.FullName;
            //    }
            //    else if (entityFam.FamilyID != null || entityFam.FamilyID != 0)
            //    {
            //        lblTTD.Text = entityEC.ContactName;
            //    }
            //    else
            //    {
            //        lblTTD.Text = entityFam.FullName;
            //    }
            //}
            //else if (entityFam == null)
            //{
            //    lblTTD.Text = entityEC.ContactName;
            //}
            //else if (entityEC != null)
            //{
            //    if (entityEC.FamilyID == null || entityEC.FamilyID == 0)
            //    {
            //        lblTTD.Text = entityEC.ContactName;
            //    }
            //    else if (entityEC.FamilyID != null || entityEC.FamilyID != 0)
            //    {
            //        lblTTD.Text = entityFam.FullName;
            //    }
            //    else
            //    {
            //        lblTTD.Text = entityEC.ContactName;
            //    }
            //}
            //else if (entityEC != null)
            //{
            //    lblTTD.Text = entityFam.FullName;
            //}
            base.InitializeReport(param);
        }
    }
}
