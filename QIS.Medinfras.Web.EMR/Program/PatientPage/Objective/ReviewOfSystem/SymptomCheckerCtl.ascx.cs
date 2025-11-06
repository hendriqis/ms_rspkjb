using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class SymptomCheckerCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            IsAdd = true;
            string imgFrontSrc = "";
            string imgBackSrc = "";
            string useMapFront = "";
            string useMapBack = "";
            Patient entity = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
            if (entity.GCGender == Constant.Gender.MALE)
            {
                imgFrontSrc = "male_front.png";
                imgBackSrc = "male_back.png";
                useMapFront = "#symptomcheckerfrontmale";
                useMapBack = "#symptomcheckerbackmale";
            }
            else
            {
                imgFrontSrc = "female_front.png";
                imgBackSrc = "female_back.png";
                useMapFront = "#symptomcheckerfrontfemale";
                useMapBack = "#symptomcheckerbackfemale";
            }
            imgFront.Src = ResolveUrl("~/Libs/Images/Medical/BodyDiagram/") + imgFrontSrc;
            imgBack.Src = ResolveUrl("~/Libs/Images/Medical/BodyDiagram/") + imgBackSrc;
            imgFront.Attributes.Add("usemap", useMapFront);
            imgBack.Attributes.Add("usemap", useMapBack);
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            String[] tmp = hdnSaveData.Value.Split(new string[] { "||" }, StringSplitOptions.None);

            String[] listParam = tmp[1].Split('|');

            ReviewOfSystemHd entityHd = null;

            IDbContext ctx = DbFactory.Configure(true);
            ReviewOfSystemHdDao entityHdDao = new ReviewOfSystemHdDao(ctx);
            ReviewOfSystemDtDao entityDtDao = new ReviewOfSystemDtDao(ctx);

            try
            {
                entityHd = new ReviewOfSystemHd();
                entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
                entityHd.ObservationDate = DateTime.Now;
                entityHd.ObservationTime = DateTime.Now.ToString("HH:mm");
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Insert(entityHd);

                entityHd.ID = BusinessLayer.GetReviewOfSystemHdMaxID(ctx);

                for (int i = 0; i < listParam.Count(); i += 2)
                {
                    ReviewOfSystemDt entityDt = new ReviewOfSystemDt();
                    entityDt.ID = entityHd.ID;
                    entityDt.IsNormal = false;
                    entityDt.GCROSystem = listParam[i];
                    entityDt.Remarks = listParam[i + 1];

                    entityDtDao.Insert(entityDt);
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}