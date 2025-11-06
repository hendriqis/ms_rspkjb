using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Data.Service.DataLayer.Base
{
    public class ChartBase
    {
        public class ChartInfo
        {
            private String _Title;
            private String _XLabel;
            private String _YLabel;

            public String Title
            {
                get { return _Title; }
                set { _Title = value; }
            }

            public String XLabel
            {
                get { return _XLabel; }
                set { _XLabel = value; }
            }

            public String YLabel
            {
                get { return _YLabel; }
                set { _YLabel = value; }
            }
        }

        public class ChartPoint
        {
            private String _SeriesName;
            private String _XPoint;
            private Double _YPoint;
            private String _DataLabel;

            public String SeriesName
            {
                get { return _SeriesName; }
                set { _SeriesName = value; }
            }

            public String XPoint
            {
                get { return _XPoint; }
                set { _XPoint = value; }
            }

            public Double YPoint
            {
                get { return _YPoint; }
                set { _YPoint = value; }
            }

            public String DataLabel
            {
                get { return _DataLabel; }
                set { _DataLabel = value; }
            }
        }
    }
}
