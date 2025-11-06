using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LEvaluasiDietPasien : QIS.Medinfras.ReportDesktop.BaseDailyPortraitRpt
    {
        private int recordnumber = 1;
        private int nilainormal = 0;
        private int ha1 = 0;
        private int ha2 = 0;
        private int ha3 = 0;
        private int ha4 = 0;
        private int ha5 = 0;
        private int ha0 = 0;
        private int lh1 = 0;
        private int lh2 = 0;
        private int lh3 = 0;
        private int lh4 = 0;
        private int lh5 = 0;
        private int lh0 = 0;
        private int ln1 = 0;
        private int ln2 = 0;
        private int ln3 = 0;
        private int ln4 = 0;
        private int ln5 = 0;
        private int ln0 = 0;
        private int sy1 = 0;
        private int sy2 = 0;
        private int sy3 = 0;
        private int sy4 = 0;
        private int sy5 = 0;
        private int sy0 = 0;
        private int bh1 = 0;
        private int bh2 = 0;
        private int bh3 = 0;
        private int bh4 = 0;
        private int bh5 = 0;
        private int bh0 = 0;
        private int fl1 = 0;
        private int fl2 = 0;
        private int fl3 = 0;
        private int fl4 = 0;
        private int fl5 = 0;
        private int fl0 = 0;

        public LEvaluasiDietPasien()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

        private void xrTableCell9_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = recordnumber.ToString();
            recordnumber++;
        }

        private void GroupFooter3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            #region setnilai
            ha1 = Convert.ToInt32(lblSumHa1.Summary.GetResult());
            ha2 = Convert.ToInt32(lblSumHa2.Summary.GetResult());
            ha3 = Convert.ToInt32(lblSumHa3.Summary.GetResult());
            ha4 = Convert.ToInt32(lblSumHa4.Summary.GetResult());
            ha5 = Convert.ToInt32(lblSumHa5.Summary.GetResult());
            //ha0 = Convert.ToInt32(lblSumHa0.Summary.GetResult());

            lh1 = Convert.ToInt32(lblSumLh1.Summary.GetResult());
            lh2 = Convert.ToInt32(lblSumLh2.Summary.GetResult());
            lh3 = Convert.ToInt32(lblSumLh3.Summary.GetResult());
            lh4 = Convert.ToInt32(lblSumLh4.Summary.GetResult());
            lh5 = Convert.ToInt32(lblSumLh5.Summary.GetResult());
            //lh0 = Convert.ToInt32(lblSumLh0.Summary.GetResult());

            ln1 = Convert.ToInt32(lblSumLn1.Summary.GetResult());
            ln2 = Convert.ToInt32(lblSumLn2.Summary.GetResult());
            ln3 = Convert.ToInt32(lblSumLn3.Summary.GetResult());
            ln4 = Convert.ToInt32(lblSumLn4.Summary.GetResult());
            ln5 = Convert.ToInt32(lblSumLn5.Summary.GetResult());
            //ln0 = Convert.ToInt32(lblSumLn0.Summary.GetResult());

            sy1 = Convert.ToInt32(lblSumSy1.Summary.GetResult());
            sy2 = Convert.ToInt32(lblSumSy2.Summary.GetResult());
            sy3 = Convert.ToInt32(lblSumSy3.Summary.GetResult());
            sy4 = Convert.ToInt32(lblSumSy4.Summary.GetResult());
            sy5 = Convert.ToInt32(lblSumSy5.Summary.GetResult());
            //sy0 = Convert.ToInt32(lblSumSy0.Summary.GetResult());

            bh1 = Convert.ToInt32(lblSumBh1.Summary.GetResult());
            bh2 = Convert.ToInt32(lblSumBh2.Summary.GetResult());
            bh3 = Convert.ToInt32(lblSumBh3.Summary.GetResult());
            bh4 = Convert.ToInt32(lblSumBh4.Summary.GetResult());
            bh5 = Convert.ToInt32(lblSumBh5.Summary.GetResult());
            //bh0 = Convert.ToInt32(lblSumBh0.Summary.GetResult());

            fl1 = Convert.ToInt32(lblSumFl1.Summary.GetResult());
            fl2 = Convert.ToInt32(lblSumFl2.Summary.GetResult());
            fl3 = Convert.ToInt32(lblSumFl3.Summary.GetResult());
            fl4 = Convert.ToInt32(lblSumFl4.Summary.GetResult());
            fl5 = Convert.ToInt32(lblSumFl5.Summary.GetResult());
            //fl0 = Convert.ToInt32(lblSumFl0.Summary.GetResult());

            nilainormal = Convert.ToInt32(lblnilainormal.Summary.GetResult());
            #endregion
            #region hitung

            double hasilha1 = 0;
            double hasilha2 = 0;
            double hasilha3 = 0;
            double hasilha4 = 0;
            double hasilha5 = 0;
            //double hasilha0 = 0;

            double hasillh1 = 0;
            double hasillh2 = 0;
            double hasillh3 = 0;
            double hasillh4 = 0;
            double hasillh5 = 0;
            //double hasillh0 = 0;
            
            double hasilln1 = 0;
            double hasilln2 = 0;
            double hasilln3 = 0;
            double hasilln4 = 0;
            double hasilln5 = 0;
            //double hasilln0 = 0;

            double hasilsy1 = 0;
            double hasilsy2 = 0;
            double hasilsy3 = 0;
            double hasilsy4 = 0;
            double hasilsy5 = 0;
            //double hasilsy0 = 0;

            double hasilbh1 = 0;
            double hasilbh2 = 0;
            double hasilbh3 = 0;
            double hasilbh4 = 0;
            double hasilbh5 = 0;
            //double hasilbh0 = 0;
            
            double hasilfl1 = 0;
            double hasilfl2 = 0;
            double hasilfl3 = 0;
            double hasilfl4 = 0;
            double hasilfl5 = 0;
            //double hasilfl0 = 0;


            hasilha1 = ((Convert.ToDouble(ha1) / Convert.ToDouble(nilainormal)) * 100);
            hasilha2 = ((Convert.ToDouble(ha2) / Convert.ToDouble(nilainormal)) * 100);
            hasilha3 = ((Convert.ToDouble(ha3) / Convert.ToDouble(nilainormal)) * 100);
            hasilha4 = ((Convert.ToDouble(ha4) / Convert.ToDouble(nilainormal)) * 100);
            hasilha5 = ((Convert.ToDouble(ha5) / Convert.ToDouble(nilainormal)) * 100);
            //hasilha0 = ((Convert.ToDouble(ha0) / Convert.ToDouble(nilainormal)) * 100);

            hasillh1 = ((Convert.ToDouble(lh1) / Convert.ToDouble(nilainormal)) * 100);
            hasillh2 = ((Convert.ToDouble(lh2) / Convert.ToDouble(nilainormal)) * 100);
            hasillh3 = ((Convert.ToDouble(lh3) / Convert.ToDouble(nilainormal)) * 100);
            hasillh4 = ((Convert.ToDouble(lh4) / Convert.ToDouble(nilainormal)) * 100);
            hasillh5 = ((Convert.ToDouble(lh5) / Convert.ToDouble(nilainormal)) * 100);
            //hasillh0 = ((Convert.ToDouble(lh0) / Convert.ToDouble(nilainormal)) * 100);

            hasilln1 = ((Convert.ToDouble(ln1) / Convert.ToDouble(nilainormal)) * 100);
            hasilln2 = ((Convert.ToDouble(ln2) / Convert.ToDouble(nilainormal)) * 100);
            hasilln3 = ((Convert.ToDouble(ln3) / Convert.ToDouble(nilainormal)) * 100);
            hasilln4 = ((Convert.ToDouble(ln4) / Convert.ToDouble(nilainormal)) * 100);
            hasilln5 = ((Convert.ToDouble(ln5) / Convert.ToDouble(nilainormal)) * 100);
            //hasilln0 = ((Convert.ToDouble(ln0) / Convert.ToDouble(nilainormal)) * 100);
            
            hasilsy1 = ((Convert.ToDouble(sy1) / Convert.ToDouble(nilainormal)) * 100);
            hasilsy2 = ((Convert.ToDouble(sy2) / Convert.ToDouble(nilainormal)) * 100);
            hasilsy3 = ((Convert.ToDouble(sy3) / Convert.ToDouble(nilainormal)) * 100);
            hasilsy4 = ((Convert.ToDouble(sy4) / Convert.ToDouble(nilainormal)) * 100);
            hasilsy5 = ((Convert.ToDouble(sy5) / Convert.ToDouble(nilainormal)) * 100);
            //hasilsy0 = ((Convert.ToDouble(sy0) / Convert.ToDouble(nilainormal)) * 100);
           
            hasilbh1 = ((Convert.ToDouble(bh1) / Convert.ToDouble(nilainormal)) * 100);
            hasilbh2 = ((Convert.ToDouble(bh2) / Convert.ToDouble(nilainormal)) * 100);
            hasilbh3 = ((Convert.ToDouble(bh3) / Convert.ToDouble(nilainormal)) * 100);
            hasilbh4 = ((Convert.ToDouble(bh4) / Convert.ToDouble(nilainormal)) * 100);
            hasilbh5 = ((Convert.ToDouble(bh5) / Convert.ToDouble(nilainormal)) * 100);
            //hasilbh0 = ((Convert.ToDouble(bh0) / Convert.ToDouble(nilainormal)) * 100);

            hasilfl1 = ((Convert.ToDouble(fl1) / Convert.ToDouble(nilainormal)) * 100);
            hasilfl2 = ((Convert.ToDouble(fl2) / Convert.ToDouble(nilainormal)) * 100);
            hasilfl3 = ((Convert.ToDouble(fl3) / Convert.ToDouble(nilainormal)) * 100);
            hasilfl4 = ((Convert.ToDouble(fl4) / Convert.ToDouble(nilainormal)) * 100);
            hasilfl5 = ((Convert.ToDouble(fl5) / Convert.ToDouble(nilainormal)) * 100);
            //hasilfl0 = ((Convert.ToDouble(fl0) / Convert.ToDouble(nilainormal)) * 100);


            nilaiha1.Text = ha1.ToString();
            nilaiha2.Text = ha2.ToString();
            nilaiha3.Text = ha3.ToString();
            nilaiha4.Text = ha4.ToString();
            nilaiha5.Text = ha5.ToString();

            pcha1.Text = hasilha1.ToString("#.##");
            pcha2.Text = hasilha2.ToString("#.##");
            pcha3.Text = hasilha3.ToString("#.##");
            pcha4.Text = hasilha4.ToString("#.##");
            pcha5.Text = hasilha5.ToString("#.##");

            nilailh1.Text = lh1.ToString();
            nilailh2.Text = lh2.ToString();
            nilailh3.Text = lh3.ToString();
            nilailh4.Text = lh4.ToString();
            nilailh5.Text = lh5.ToString();
            pclh1.Text = hasillh1.ToString("#.##");
            pclh2.Text = hasillh2.ToString("#.##");
            pclh3.Text = hasillh3.ToString("#.##");
            pclh4.Text = hasillh4.ToString("#.##");
            pclh5.Text = hasillh5.ToString("#.##");

            nilailn1.Text = ln1.ToString();
            nilailn2.Text = ln2.ToString();
            nilailn3.Text = ln3.ToString();
            nilailn4.Text = ln4.ToString();
            nilailn5.Text = ln5.ToString();
            pcln1.Text = hasilln1.ToString("#.##");
            pcln2.Text = hasilln2.ToString("#.##");
            pcln3.Text = hasilln3.ToString("#.##");
            pcln4.Text = hasilln4.ToString("#.##");
            pcln5.Text = hasilln5.ToString("#.##");

            nilaisy1.Text = sy1.ToString();
            nilaisy2.Text = sy2.ToString();
            nilaisy3.Text = sy3.ToString();
            nilaisy4.Text = sy4.ToString();
            nilaisy5.Text = sy5.ToString();
            pcsy1.Text = hasilsy1.ToString("#.##");
            pcsy2.Text = hasilsy2.ToString("#.##");
            pcsy3.Text = hasilsy3.ToString("#.##");
            pcsy4.Text = hasilsy4.ToString("#.##");
            pcsy5.Text = hasilsy5.ToString("#.##");

            nilaibh1.Text = bh1.ToString();
            nilaibh2.Text = bh2.ToString();
            nilaibh3.Text = bh3.ToString();
            nilaibh4.Text = bh4.ToString();
            nilaibh5.Text = bh5.ToString();
            pcbh1.Text = hasilbh1.ToString("#.##");
            pcbh2.Text = hasilbh2.ToString("#.##");
            pcbh3.Text = hasilbh3.ToString("#.##");
            pcbh4.Text = hasilbh4.ToString("#.##");
            pcbh5.Text = hasilbh5.ToString("#.##");

            nilaifl1.Text = fl1.ToString();
            nilaifl2.Text = fl2.ToString();
            nilaifl3.Text = fl3.ToString();
            nilaifl4.Text = fl4.ToString();
            nilaifl5.Text = fl5.ToString();
            pcfl1.Text = hasilfl1.ToString("#.##");
            pcfl2.Text = hasilfl2.ToString("#.##");
            pcfl3.Text = hasilfl3.ToString("#.##");
            pcfl4.Text = hasilfl4.ToString("#.##");
            pcfl5.Text = hasilfl5.ToString("#.##");
            #endregion
        }
    }
}
