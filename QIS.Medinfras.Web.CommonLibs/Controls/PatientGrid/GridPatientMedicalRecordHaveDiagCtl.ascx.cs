using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GridPatientMedicalRecordHaveDiagCtl : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        public void InitializeControl()
        {
            if (AppSession.LastPagingMR2 != null)
            {
                hdnLastPagging2.Value = AppSession.LastPagingMR2.PageID.ToString();
            }

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                            AppSession.UserLogin.HealthcareID, //0
                                            Constant.SettingParameter.RM_PATIENT_LIST_HAVE_DIAG_NOT_ONLY_MAIN_DIAGNOSIS //1
                                        );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnDiagnosisNotOnlyMainDiagnosis.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.RM_PATIENT_LIST_HAVE_DIAG_NOT_ONLY_MAIN_DIAGNOSIS).FirstOrDefault().ParameterValue;

            BindGridView(1, true, ref PageCount);
        }

        protected void cbpViewDiag_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ((BasePageRegisteredPatient)Page).LoadAllWords();
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ((BasePageRegisteredPatient)Page).GetFilterExpression();

            int rowCount = 0;
            if (isCountPageCount)
            {
                if (hdnDiagnosisNotOnlyMainDiagnosis.Value == "1")
                {
                    rowCount = BusinessLayer.GetvConsultVisitCodingAllDiagnoseRowCount(filterExpression);
                }
                else
                {
                    rowCount = BusinessLayer.GetvConsultVisitCodingRowCount(filterExpression);
                }
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_ITEM);
            }

            LastPagingMR2 mr = new LastPagingMR2();
            mr.PageID = pageIndex;
            AppSession.LastPagingMR2 = mr;

            List<vConsultVisitCodingAllDiagnose> lstEntityAllDiagnose = null;
            List<vConsultVisitCoding> lstEntity = null;

            if (hdnDiagnosisNotOnlyMainDiagnosis.Value == "1")
            {
                lstEntityAllDiagnose = BusinessLayer.GetvConsultVisitCodingAllDiagnoseList(filterExpression, Constant.GridViewPageSize.GRID_ITEM, pageIndex, "RegistrationNo");
            }
            else
            {
                lstEntity = BusinessLayer.GetvConsultVisitCodingList(filterExpression, Constant.GridViewPageSize.GRID_ITEM, pageIndex, "RegistrationNo");
            }

            if (lstEntityAllDiagnose != null)
            {
                lvwViewHaveDiag.DataSource = lstEntityAllDiagnose;
                lvwViewHaveDiag.DataBind();

                RowCountTable rc = new RowCountTable()
                {
                    TotalRow = lstEntityAllDiagnose.Count()
                };

                List<RowCountTable> rcList = new List<RowCountTable>();
                rcList.Add(rc);
                lvwViewCount.DataSource = rcList;
                lvwViewCount.DataBind();
            }
            else if (lstEntity != null)
            {
                lvwViewHaveDiag.DataSource = lstEntity;
                lvwViewHaveDiag.DataBind();

                RowCountTable rc = new RowCountTable()
                {
                    TotalRow = lstEntity.Count()
                };

                List<RowCountTable> rcList = new List<RowCountTable>();
                rcList.Add(rc);
                lvwViewCount.DataSource = rcList;
                lvwViewCount.DataBind();
            }
        }

        protected void lvwViewHaveDiag_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (hdnDiagnosisNotOnlyMainDiagnosis.Value == "1")
                {
                    vConsultVisitCodingAllDiagnose entity = e.Item.DataItem as vConsultVisitCodingAllDiagnose;
                    HtmlGenericControl divMedicalFileStatus = e.Item.FindControl("divMedicalFileStatus") as HtmlGenericControl;
                    HtmlGenericControl divChiefComplaint = e.Item.FindControl("divChiefComplaint") as HtmlGenericControl;
                    HtmlGenericControl divPDxID = e.Item.FindControl("divPDxID") as HtmlGenericControl;
                    HtmlGenericControl divPDxText = e.Item.FindControl("divPDxText") as HtmlGenericControl;
                    HtmlGenericControl divMRDxID = e.Item.FindControl("divMRDxID") as HtmlGenericControl;
                    HtmlGenericControl divMRDxText = e.Item.FindControl("divMRDxText") as HtmlGenericControl;
                    HtmlGenericControl divPhysicianDischarge = e.Item.FindControl("divPhysicianDischarge") as HtmlGenericControl;
                    HtmlGenericControl divDischarge = e.Item.FindControl("divDischarge") as HtmlGenericControl;
                    HtmlGenericControl divDischargeDate = e.Item.FindControl("divDischargeDate") as HtmlGenericControl;
                    HtmlGenericControl divO = e.Item.FindControl("divO") as HtmlGenericControl;

                    if (entity.GCMedicalFileStatus != null && entity.GCMedicalFileStatus != "")
                    {
                        divMedicalFileStatus.InnerHtml = string.Format("Status Berkas Terakhir : {0}", entity.MedicalFileStatus);
                    }
                    else
                    {
                        divMedicalFileStatus.Style.Add("display", "none");
                        divMedicalFileStatus.InnerHtml = "";
                    }

                    if (entity.ChiefComplaint != null && entity.ChiefComplaint != "")
                    {
                        divChiefComplaint.InnerHtml = "X";
                        divChiefComplaint.Style.Add("color", "blue");
                    }
                    else
                    {
                        divChiefComplaint.InnerHtml = "O";
                        divChiefComplaint.Style.Add("color", "red");
                    }

                    if (entity.DiagnoseID != null && entity.DiagnoseID != "")
                    {
                        divPDxID.InnerHtml = "X";
                        divPDxID.Style.Add("color", "blue");
                    }
                    else
                    {
                        divPDxID.InnerHtml = "O";
                        divPDxID.Style.Add("color", "red");
                    }

                    if (entity.DiagnosisText != null && entity.DiagnosisText != "")
                    {
                        divPDxText.InnerHtml = "X";
                        divPDxText.Style.Add("color", "blue");
                    }
                    else
                    {
                        divPDxText.InnerHtml = "O";
                        divPDxText.Style.Add("color", "red");
                    }

                    if (entity.FinalDiagnoseID != null && entity.FinalDiagnoseID != "")
                    {
                        divMRDxID.InnerHtml = "X";
                        divMRDxID.Style.Add("color", "blue");
                    }
                    else
                    {
                        divMRDxID.InnerHtml = "O";
                        divMRDxID.Style.Add("color", "red");
                    }

                    if (entity.FinalDiagnosisText != null && entity.FinalDiagnosisText != "")
                    {
                        divMRDxText.InnerHtml = "X";
                        divMRDxText.Style.Add("color", "blue");
                    }
                    else
                    {
                        divMRDxText.InnerHtml = "O";
                        divMRDxText.Style.Add("color", "red");
                    }

                    if (entity.PhysicianDischargedDate != null && entity.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                    {
                        divPhysicianDischarge.InnerHtml = "O";
                        divPhysicianDischarge.Style.Add("color", "red");
                    }
                    else
                    {
                        divPhysicianDischarge.InnerHtml = "X";
                        divPhysicianDischarge.Style.Add("color", "blue");
                    }

                    if (entity.DischargeDate != null && entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                    {
                        divDischargeDate.Style.Add("display", "none");
                        divDischarge.InnerHtml = "O";
                        divDischarge.Style.Add("color", "red");
                    }
                    else
                    {
                        divDischargeDate.InnerHtml = string.Format("{0} : {1} {2}", GetLabel("Pulang"), entity.DischargeDateInString, entity.DischargeTime);
                        divDischarge.InnerHtml = "X";
                        divDischarge.Style.Add("color", "blue");
                    }

                    if (entity.TotalVitalSignDt > 0 && entity.TotalROSDt > 0)
                    {
                        divO.InnerHtml = "X";
                        divO.Style.Add("color", "blue");
                    }
                    else
                    {
                        divO.InnerHtml = "X";
                        divO.Style.Add("color", "blue");
                    }
                }
                else
                {
                    vConsultVisitCoding entity = e.Item.DataItem as vConsultVisitCoding;
                    HtmlGenericControl divMedicalFileStatus = e.Item.FindControl("divMedicalFileStatus") as HtmlGenericControl;
                    HtmlGenericControl divChiefComplaint = e.Item.FindControl("divChiefComplaint") as HtmlGenericControl;
                    HtmlGenericControl divPDxID = e.Item.FindControl("divPDxID") as HtmlGenericControl;
                    HtmlGenericControl divPDxText = e.Item.FindControl("divPDxText") as HtmlGenericControl;
                    HtmlGenericControl divMRDxID = e.Item.FindControl("divMRDxID") as HtmlGenericControl;
                    HtmlGenericControl divMRDxText = e.Item.FindControl("divMRDxText") as HtmlGenericControl;
                    HtmlGenericControl divPhysicianDischarge = e.Item.FindControl("divPhysicianDischarge") as HtmlGenericControl;
                    HtmlGenericControl divDischargeDate = e.Item.FindControl("divDischargeDate") as HtmlGenericControl;
                    HtmlGenericControl divDischarge = e.Item.FindControl("divDischarge") as HtmlGenericControl;
                    HtmlGenericControl divO = e.Item.FindControl("divO") as HtmlGenericControl;

                    if (entity.GCMedicalFileStatus != null && entity.GCMedicalFileStatus != "")
                    {
                        divMedicalFileStatus.InnerHtml = string.Format("Status Berkas Terakhir : {0}", entity.MedicalFileStatus);
                    }
                    else
                    {
                        divMedicalFileStatus.Style.Add("display", "none");
                        divMedicalFileStatus.InnerHtml = "";
                    }

                    if (entity.ChiefComplaint != null && entity.ChiefComplaint != "")
                    {
                        divChiefComplaint.InnerHtml = "X";
                        divChiefComplaint.Style.Add("color", "blue");
                    }
                    else
                    {
                        divChiefComplaint.InnerHtml = "O";
                        divChiefComplaint.Style.Add("color", "red");
                    }

                    if (entity.DiagnoseID != null && entity.DiagnoseID != "")
                    {
                        divPDxID.InnerHtml = "X";
                        divPDxID.Style.Add("color", "blue");
                    }
                    else
                    {
                        divPDxID.InnerHtml = "O";
                        divPDxID.Style.Add("color", "red");
                    }

                    if (entity.DiagnosisText != null && entity.DiagnosisText != "")
                    {
                        divPDxText.InnerHtml = "X";
                        divPDxText.Style.Add("color", "blue");
                    }
                    else
                    {
                        divPDxText.InnerHtml = "O";
                        divPDxText.Style.Add("color", "red");
                    }

                    if (entity.FinalDiagnoseID != null && entity.FinalDiagnoseID != "")
                    {
                        divMRDxID.InnerHtml = "X";
                        divMRDxID.Style.Add("color", "blue");
                    }
                    else
                    {
                        divMRDxID.InnerHtml = "O";
                        divMRDxID.Style.Add("color", "red");
                    }

                    if (entity.FinalDiagnosisText != null && entity.FinalDiagnosisText != "")
                    {
                        divMRDxText.InnerHtml = "X";
                        divMRDxText.Style.Add("color", "blue");
                    }
                    else
                    {
                        divMRDxText.InnerHtml = "O";
                        divMRDxText.Style.Add("color", "red");
                    }

                    if (entity.PhysicianDischargedDate != null && entity.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                    {
                        divPhysicianDischarge.InnerHtml = "O";
                        divPhysicianDischarge.Style.Add("color", "red");
                    }
                    else
                    {
                        divPhysicianDischarge.InnerHtml = "X";
                        divPhysicianDischarge.Style.Add("color", "blue");
                    }

                    if (entity.DischargeDate != null && entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                    {
                        divDischargeDate.Style.Add("display", "none");
                        divDischarge.InnerHtml = "O";
                        divDischarge.Style.Add("color", "red");
                    }
                    else
                    {
                        divDischargeDate.InnerHtml = string.Format("{0} : {1} {2}", GetLabel("Pulang"), entity.DischargeDateInString, entity.DischargeTime);
                        divDischarge.InnerHtml = "X";
                        divDischarge.Style.Add("color", "blue");
                    }

                    if (entity.TotalVitalSignDt > 0 && entity.TotalROSDt > 0)
                    {
                        divO.InnerHtml = "X";
                        divO.Style.Add("color", "blue");
                    }
                    else
                    {
                        divO.InnerHtml = "X";
                        divO.Style.Add("color", "blue");
                    }
                }
            }
        }

        protected string GetLabel(string code)
        {
            return ((BasePageRegisteredPatient)Page).GetLabel(code);
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnTransactionNo.Value != "")
            {
                ((BasePageRegisteredPatient)Page).OnGrdRowClick(hdnTransactionNo.Value);
            }
        }

        private class RowCountTable
        {
            private Int32 totalRow;

            public Int32 TotalRow
            {
                get { return totalRow; }
                set { totalRow = value; }
            }
        }
    }
}