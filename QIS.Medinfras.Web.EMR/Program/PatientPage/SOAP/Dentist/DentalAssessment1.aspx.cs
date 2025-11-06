using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class DentalAssessment1 : BasePagePatientPageList
    {
        protected int NextTreatmentPageCount = 0;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.DENTAL_CHART;
        }

        protected override void InitializeDataControl()
        {
            rptListTooth.DataSource = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.TOOTH));
            rptListTooth.DataBind();
            CreateTableTooth();
            BindGrdNextTreatment(1, true, ref NextTreatmentPageCount);
        }

        protected string GetToothNumber(string StandardCodeID)
        {
            return StandardCodeID.Replace("X044^0", "");
        }

        #region List Tooth
        protected void cbpListTooth_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            CreateTableTooth();
        }
        #endregion

        #region Treatment Detail
        private void BindGrdTreatmentDetail(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            pageCount = 0;
            if (hdnToothID.Value != "")
            {
                string toothID = string.Format("X044^0{0}", Convert.ToInt32(hdnToothID.Value).ToString("00"));

                filterExpression = hdnFilterExpression.Value;
                if (filterExpression != "")
                    filterExpression += " AND ";
                filterExpression += string.Format("MRN = {0} AND GCTooth = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, toothID);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPatientDentalRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

            }

            List<vPatientDental> lstEntity = BusinessLayer.GetvPatientDentalList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdTreatmentDetail.DataSource = lstEntity;
            grdTreatmentDetail.DataBind();
        }

        protected void cbpTreatmentDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGrdTreatmentDetail(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGrdTreatmentDetail(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Next Treatment
        private void BindGrdNextTreatment(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("MRN = {0} AND NextGCToothStatus IS NOT NULL AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDentalRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDental> lstEntity = BusinessLayer.GetvPatientDentalList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdNextTreatment.DataSource = lstEntity;
            grdNextTreatment.DataBind();
        }

        protected void cbpNextTreatment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGrdNextTreatment(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGrdNextTreatment(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        protected override bool OnBeforeEditRecord(ref string errMessage)
        {
            if (hdnTreatmentDetailID.Value != "")
            {
                PatientDental entity = BusinessLayer.GetPatientDental(Convert.ToInt32(hdnTreatmentDetailID.Value));
                if (entity.VisitID != AppSession.RegisteredPatient.VisitID)
                {
                    errMessage = "Cannot Edit This Data";
                    return false;
                }
                return true;
            }
            else
            {
                errMessage = "Please select the row before edit";
                return false;
            }
        }

        protected override bool OnBeforeDeleteRecord(ref string errMessage)
        {
            if (hdnTreatmentDetailID.Value != "")
            {
                PatientDental entity = BusinessLayer.GetPatientDental(Convert.ToInt32(hdnTreatmentDetailID.Value));
                if (entity.VisitID != AppSession.RegisteredPatient.VisitID)
                {
                    errMessage = "Cannot Delete This Data";
                    return false;
                }
                return true;
            }
            else
            {
                errMessage = "Please select the row before delete";
                return false;
            }
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnToothID.Value != "0")
            {
                queryString = string.Format("add|{0}", hdnToothID.Value);
                url = ResolveUrl("~/Program/PatientPage/Assesment/DentalChart/DentalChartEntryCtl.ascx");
                popupHeaderText = "Dental Chart";
                popupWidth = 700;
                popupHeight = 500;
                //PopupHeaderText = Helper.GeneratePopupHeaderText("Dental Chart", AppSession.RegisteredPatient);
                return true;
            }
            return false;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnTreatmentDetailID.Value != "")
            {
                queryString = string.Format("edit|{0}", hdnTreatmentDetailID.Value);
                url = ResolveUrl("~/Program/PatientPage/Assesment/DentalChart/DentalChartEntryCtl.ascx");
                popupHeaderText = "Dental Chart";
                popupWidth = 700;
                popupHeight = 500;
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnTreatmentDetailID.Value != "")
            {
                bool result = true;

                IDbContext ctx = DbFactory.Configure(true);
                PatientDentalDao entityDao = new PatientDentalDao(ctx);
                PatientProcedureDao entityProcedureDao = new PatientProcedureDao(ctx);
                try
                {
                    PatientDental entity = entityDao.Get(Convert.ToInt32(hdnTreatmentDetailID.Value));
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);

                    List<PatientProcedure> lstEntityProcedure = BusinessLayer.GetPatientProcedureList(string.Format("ReferenceID = {0}", entity.ID), ctx);
                    if (lstEntityProcedure.Count > 0)
                    {
                        PatientProcedure entityProcedure = lstEntityProcedure[0];
                        entityProcedure.IsDeleted = true;
                        entityProcedure.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityProcedureDao.Update(entityProcedure);
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            return false;
        }


        #region Create Table Tooth
        class Tooth
        {
            public Int32 Line;
            public String ImageUrl;
            public Int32 Number;
            public Int32 IndexImage;
        }

        private void CreateTableTooth()
        {
            List<Tooth> lstTooth = new List<Tooth>();

            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("MRN = {0} AND IsDeleted = 0 ORDER BY ID DESC", AppSession.RegisteredPatient.MRN);
            List<vPatientDental> lstEntity = BusinessLayer.GetvPatientDentalList(filterExpression);

            for (int ctr = 1, i = 18, j = 28; i > 10; --i, --j, ctr++)
            {
                string imgUrl = "";
                if (i > 13)
                    imgUrl = "tooth.png";
                else
                    imgUrl = "tooth_2.png";
                lstTooth.Add(new Tooth { Line = 2, ImageUrl = getToothImage(imgUrl, lstEntity, i), Number = i, IndexImage = ctr });
                lstTooth.Add(new Tooth { Line = 2, ImageUrl = getToothImage(imgUrl, lstEntity, j), Number = j, IndexImage = 17 - ctr });

                lstTooth.Add(new Tooth { Line = 3, ImageUrl = getToothImage(imgUrl, lstEntity, i + 30), Number = i + 30, IndexImage = ctr });
                lstTooth.Add(new Tooth { Line = 3, ImageUrl = getToothImage(imgUrl, lstEntity, j + 10), Number = j + 10, IndexImage = 17 - ctr });
            }

            for (int ctr = 1, i = 55, j = 65; i > 50; --i, --j, ctr++)
            {
                string imgUrl = "";
                if (i > 13)
                    imgUrl = "tooth.png";
                else
                    imgUrl = "tooth_2.png";
                lstTooth.Add(new Tooth { Line = 1, ImageUrl = getToothImage(imgUrl, lstEntity, i), Number = i, IndexImage = ctr });
                lstTooth.Add(new Tooth { Line = 1, ImageUrl = getToothImage(imgUrl, lstEntity, j), Number = j, IndexImage = 11 - ctr });

                lstTooth.Add(new Tooth { Line = 4, ImageUrl = getToothImage(imgUrl, lstEntity, i + 30), Number = i + 30, IndexImage = ctr });
                lstTooth.Add(new Tooth { Line = 4, ImageUrl = getToothImage(imgUrl, lstEntity, j + 10), Number = j + 10, IndexImage = 11 - ctr });
            }

            containerTableTooth.Controls.Add(CreateTableTitle("upper right", "upper left"));

            for (int i = 1; i < 5; ++i)
            {
                HtmlTable tbl = new HtmlTable();
                tbl.Attributes.Add("Class", "tblTooth");
                HtmlTableRow row = new HtmlTableRow();
                List<Tooth> lst = lstTooth.Where(p => p.Line == i).OrderBy(p => p.IndexImage).ToList();
                foreach (Tooth tooth in lst)
                {
                    HtmlTableCell cell = new HtmlTableCell();

                    HtmlGenericControl div = new HtmlGenericControl("div");
                    div.InnerHtml = tooth.Number.ToString();

                    HtmlImage img = new HtmlImage();
                    img.Src = string.Format("{0}{1}", Page.ResolveUrl("~/Libs/Images/Medical/"), tooth.ImageUrl);
                    img.Alt = "";

                    if (i < 3)
                    {
                        cell.Controls.Add(div);
                        cell.Controls.Add(img);
                    }
                    else
                    {
                        cell.Controls.Add(img);
                        cell.Controls.Add(div);
                    }

                    row.Cells.Add(cell);
                }
                tbl.Rows.Add(row);

                containerTableTooth.Controls.Add(tbl);
            }
            containerTableTooth.Controls.Add(CreateTableTitle("lower right", "lower left"));
        }

        private string getToothImage(string imgUrl, List<vPatientDental> lstEntity, int ctr)
        {
            string toothID = string.Format("X044^0{0}", ctr.ToString("00"));
            vPatientDental entity = lstEntity.FirstOrDefault(p => p.GCTooth == toothID);
            if (entity != null)
                return string.Format("tooth_{0}.png", entity.ToothStatus.Substring(0, 3));
            return imgUrl;
        }

        private HtmlTable CreateTableTitle(string leftTitle, string rightTitle)
        {
            HtmlTable tbl = new HtmlTable();
            tbl.Attributes.Add("Class", "tblToothHeader");

            HtmlTableRow row = new HtmlTableRow();

            HtmlTableCell cell1 = new HtmlTableCell();
            cell1.InnerHtml = leftTitle;
            HtmlTableCell cell2 = new HtmlTableCell();
            cell2.InnerHtml = rightTitle;

            row.Cells.Add(cell1);
            row.Cells.Add(cell2);

            tbl.Rows.Add(row);
            return tbl;
        }
        #endregion
    }
}