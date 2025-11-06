using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Common
{
    public class CommonConstant
    {
        public static class HL7_MEDAVIS_MSG
        {
            public const string IDENTIFICATION_1 = "medavis RIS";
            public const string IDENTIFICATION_2 = "MEDAVIS";
            public const string HL7_VERSION = "2.3.1";
        }

        public static class HL7_INFINITT_MSG
        {
            public const string IDENTIFICATION_1 = "INFINITT RIS";
            public const string IDENTIFICATION_2 = "INFINITT";
            public const string HL7_VERSION = "2.3.1";
        }

        public static class HL7_FUJIFILM_MSG
        {
            public const string IDENTIFICATION_1 = "FUJIFILM RIS";
            public const string IDENTIFICATION_2 = "FUJIFILM";
            public const string HL7_VERSION = "2.3.1";
        }

        public static class HL7_ZED_MSG
        {
            public const string IDENTIFICATION_1 = "ZED RIS";
            public const string IDENTIFICATION_2 = "ZED";
            public const string HL7_VERSION = "2.3.1";
        }

        public static class HL7_ROCHE_MSG
        {
            public const string IDENTIFICATION_1 = "ROCHE LIS";
            public const string IDENTIFICATION_2 = "ROCHE";
            public const string HL7_VERSION = "2.5";
        }

        public static class HL7_MEDSYNAPTIC_MSG
        {
            public const string IDENTIFICATION_1 = "MEDSYNAPTIC RIS";
            public const string IDENTIFICATION_2 = "MEDSYNAPTIC";
            public const string HL7_VERSION = "2.3.1";
        }

        public static class HL7_LIFETRACK_MSG
        {
            public const string IDENTIFICATION_1 = "LMSSCHED";
            public const string IDENTIFICATION_2 = "LMS";
            public const string HL7_VERSION = "2.4";
        }

        public static class HL7_JIVEX_MSG
        {
            public const string IDENTIFICATION_1 = "JiveX PACS";
            public const string IDENTIFICATION_2 = "JiveX";
            public const string HL7_VERSION = "2.3.1";
        }
    }
}
