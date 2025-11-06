using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace QIS.Medinfras.Data.Service
{
    public static class AppConfigManager
    {
        static private string _QISNameFormat;
        static private string _QISVirtualDirectory;
        static private string _QISPhysicalDirectory;
        static private string _QISAppVirtualDirectory;
        static private string _QISLibsPhysicalDirectory;
        static private string _QISPatientImagePath;
        static private string _QISBodyDiagramImagePath;
        static private string _QISPatientBodyDiagramImagePath;
        static private string _QISPatientImagingImagePath;
        static private string _QISPatientDocumentsPath;
        static private string _QISBusinessPartnerLogoPath;
        static private string _QISFinanceUploadDocument;
        static private string _QISMCUResultForm;
        static private string _QISParamedicImagePath;
        static private string _QISParamedicSignaturePath;
        static private string _QISItemImagePath;
        static private string _QISMRNFormat;
        static private string _QISHealthcareName;
        static private string _QISWristbandType;
        static private string _QISInfantWristbandType;
        static private string _QISMRLabelType;
        static private string _DrugLabelSize;
        static private string _PreviewReportInPDF;
        static private string _QISPhysicalTempDirectory;
        static private string _QISPhysicalUploadExcelTempDirectory;
        static private string _InpatientPrescriptionTypeFilter;
        static private string _QISGuestImagePath;

        /* BPJS Bridging Configuration */
        static private string _BPJSDemoMode;
        /* BPJS Bridging Configuration */

        static public string QISNameFormat { get { return _QISNameFormat; } }
        static public string QISVirtualDirectory { get { return _QISVirtualDirectory; } }
        static public string QISPhysicalDirectory { get { return _QISPhysicalDirectory; } }
        static public string QISAppVirtualDirectory { get { return _QISAppVirtualDirectory; } }
        static public string QISLibsPhysicalDirectory { get { return _QISLibsPhysicalDirectory; } }
        static public string QISPatientImagePath { get { return _QISPatientImagePath; } }
        static public string QISBodyDiagramImagePath { get { return _QISBodyDiagramImagePath; } }
        static public string QISPatientImagingImagePath { get { return _QISPatientImagingImagePath; } }
        static public string QISPatientBodyDiagramImagePath { get { return _QISPatientBodyDiagramImagePath; } }
        static public string QISPatientDocumentsPath { get { return _QISPatientDocumentsPath; } }
        static public string QISBusinessPartnerLogoPath { get { return _QISBusinessPartnerLogoPath; } }
        static public string QISFinanceUploadDocument { get { return _QISFinanceUploadDocument; } }
        static public string QISMCUResultForm { get { return _QISMCUResultForm; } }
        static public string QISParamedicImagePath { get { return _QISParamedicImagePath; } }
        static public string QISParamedicSignaturePath { get { return _QISParamedicSignaturePath; } }
        static public string QISItemImagePath { get { return _QISItemImagePath; } }
        static public string QISMRNFormat { get { return _QISMRNFormat; } }
        static public string QISHealthcareName { get { return _QISHealthcareName; } }
        static public string BPJSDemoMode { get { return _BPJSDemoMode; } }
        static public string PreviewReportInPDF { get { return _PreviewReportInPDF; } }
        static public string QISPhysicalTempDirectory { get { return _QISPhysicalTempDirectory; } }
        static public string QISPhysicalUploadExcelTempDirectory { get { return _QISPhysicalUploadExcelTempDirectory; } }
        static public string QISGuestImagePath { get { return _QISGuestImagePath; } }
        

        static AppConfigManager()
        {
            // Cache all these values in static properties.
            _QISNameFormat = ConfigurationManager.AppSettings["QISNameFormat"];
            _QISMRNFormat = ConfigurationManager.AppSettings["QISMRNFormat"];
            _QISVirtualDirectory = ConfigurationManager.AppSettings["QISVirtualDirectory"];
            _QISPhysicalDirectory = ConfigurationManager.AppSettings["QISPhysicalDirectory"];
            _QISAppVirtualDirectory = ConfigurationManager.AppSettings["QISAppVirtualDirectory"];
            _QISLibsPhysicalDirectory = ConfigurationManager.AppSettings["QISLibsPhysicalDirectory"];
            _QISPatientImagePath = ConfigurationManager.AppSettings["QISPatientImagePath"];
            _QISBodyDiagramImagePath = ConfigurationManager.AppSettings["QISBodyDiagramImagePath"];
            _QISPatientImagingImagePath = ConfigurationManager.AppSettings["QISPatientImagingImagePath"];
            _QISPatientBodyDiagramImagePath = ConfigurationManager.AppSettings["QISPatientBodyDiagramImagePath"];
            _QISPatientDocumentsPath = ConfigurationManager.AppSettings["QISPatientDocumentsPath"];
            _QISBusinessPartnerLogoPath = ConfigurationManager.AppSettings["QISBusinessPartnerLogoPath"];
            _QISFinanceUploadDocument = ConfigurationManager.AppSettings["QISFinanceUploadDocument"];
            _QISMCUResultForm = ConfigurationManager.AppSettings["QISMCUResultForm"]; 
            _QISParamedicImagePath = ConfigurationManager.AppSettings["QISParamedicImagePath"];
            _QISParamedicSignaturePath = ConfigurationManager.AppSettings["QISParamedicSignaturePath"];
            _QISHealthcareName = ConfigurationManager.AppSettings["QISHealthcareName"];
            _PreviewReportInPDF = ConfigurationManager.AppSettings["PreviewReportInPDF"];
            _QISPhysicalTempDirectory = ConfigurationManager.AppSettings["QISPhysicalTempDirectory"];
            _QISPhysicalUploadExcelTempDirectory = ConfigurationManager.AppSettings["QISPhysicalUploadExcelTempDirectory"];
            _BPJSDemoMode = ConfigurationManager.AppSettings["BPJSDemoMode"];
            _QISItemImagePath = ConfigurationManager.AppSettings["QISItemImagePath"];
            _QISGuestImagePath = ConfigurationManager.AppSettings["QISGuestImagePath"];
        }
    }
}
