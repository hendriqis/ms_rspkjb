<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="PatientInformationPerDoctor.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientInformationPerDoctor" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <div style="height: 50px">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle()) %></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <%--<script type="text/javascript" src='<%= ResolveUrl("~Libs/Scripts/CustomGridViewList.js" %>'></script>--%>
    <script type="text/javascript">
        function onDateChanged() {
            cbpView.PerformCallback('refresh');
        }

        $('.grdPatientInformation .lblDetail.lblLink').live('click', function () {
            var departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            var month = cboMonth.GetValue();
            var year = cboYear.GetText();
            var param = departmentID + '|' + paramedicID + '|' + month + '|' + year;
            var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDt.ascx");
            openUserControlPopup(url, param, 'Detail Information', 800, 550);
        });

        //#region Detail Per Date
        var departmentID;
        var paramedicID;
        var day;
        var month;
        var year;
        var totalPatient;
        var param;

        $('.grdPatientInformation .lblDetailPerDateValid01.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid01').val();

            if (totalPatient != null) {
                day = 01;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid01.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid01').val();

            if (totalPatient != null) {
                day = 01;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid02.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid02').val();

            if (totalPatient != null) {
                day = 02;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid02.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid02').val();

            if (totalPatient != null) {
                day = 02;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid03.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid03').val();

            if (totalPatient != null) {
                day = 03;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid03.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid03').val();

            if (totalPatient != null) {
                day = 03;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid04.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid04').val();

            if (totalPatient != null) {
                day = 04;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid04.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid04').val();

            if (totalPatient != null) {
                day = 04;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid05.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid05').val();

            if (totalPatient != null) {
                day = 05;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid05.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid05').val();

            if (totalPatient != null) {
                day = 05;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid06.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid06').val();

            if (totalPatient != null) {
                day = 06;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid06.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid06').val();

            if (totalPatient != null) {
                day = 06;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });


        $('.grdPatientInformation .lblDetailPerDateValid07.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid07').val();

            if (totalPatient != null) {
                day = 07;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid07.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid07').val();

            if (totalPatient != null) {
                day = 07;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid08.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid08').val();

            if (totalPatient != null) {
                day = 08;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid08.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid08').val();

            if (totalPatient != null) {
                day = 08;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid09.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid09').val();

            if (totalPatient != null) {
                day = 09;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid09.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid09').val();

            if (totalPatient != null) {
                day = 09;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid10.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid10').val();

            if (totalPatient != null) {
                day = 10;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid10.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid10').val();

            if (totalPatient != null) {
                day = 10;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid11.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid11').val();

            if (totalPatient != null) {
                day = 11;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid11.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid11').val();

            if (totalPatient != null) {
                day = 11;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid12.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid12').val();

            if (totalPatient != null) {
                day = 12;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid12.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid12').val();

            if (totalPatient != null) {
                day = 12;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid13.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid13').val();

            if (totalPatient != null) {
                day = 13;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid13.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid13').val();

            if (totalPatient != null) {
                day = 13;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid14.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid14').val();

            if (totalPatient != null) {
                day = 14;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid14.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid14').val();

            if (totalPatient != null) {
                day = 14;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid15.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid15').val();

            if (totalPatient != null) {
                day = 15;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid15.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid15').val();

            if (totalPatient != null) {
                day = 15;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid16.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid16').val();

            if (totalPatient != null) {
                day = 16;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid16.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid16').val();

            if (totalPatient != null) {
                day = 16;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid17.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid17').val();

            if (totalPatient != null) {
                day = 17;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid17.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid17').val();

            if (totalPatient != null) {
                day = 17;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid18.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid18').val();

            if (totalPatient != null) {
                day = 18;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid18.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid18').val();

            if (totalPatient != null) {
                day = 18;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid19.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid19').val();

            if (totalPatient != null) {
                day = 19;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid19.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid19').val();

            if (totalPatient != null) {
                day = 19;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid20.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid20').val();

            if (totalPatient != null) {
                day = 20;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid20.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid20').val();

            if (totalPatient != null) {
                day = 20;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid21.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid21').val();

            if (totalPatient != null) {
                day = 21;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid21.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid21').val();

            if (totalPatient != null) {
                day = 21;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid22.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid22').val();

            if (totalPatient != null) {
                day = 22;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid22.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid22').val();

            if (totalPatient != null) {
                day = 22;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid23.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid23').val();

            if (totalPatient != null) {
                day = 23;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid23.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid23').val();

            if (totalPatient != null) {
                day = 23;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid24.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid24').val();

            if (totalPatient != null) {
                day = 24;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid24.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid24').val();

            if (totalPatient != null) {
                day = 24;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid25.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid25').val();

            if (totalPatient != null) {
                day = 25;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid25.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid25').val();

            if (totalPatient != null) {
                day = 25;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid26.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid26').val();

            if (totalPatient != null) {
                day = 26;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid26.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid26').val();

            if (totalPatient != null) {
                day = 26;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid27.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid27').val();

            if (totalPatient != null) {
                day = 27;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid27.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid27').val();

            if (totalPatient != null) {
                day = 27;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid28.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid28').val();

            if (totalPatient != null) {
                day = 28;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid28.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid28').val();

            if (totalPatient != null) {
                day = 28;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid29.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid29').val();

            if (totalPatient != null) {
                day = 29;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid29.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid29').val();

            if (totalPatient != null) {
                day = 29;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });
        $('.grdPatientInformation .lblDetailPerDateValid30.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid30').val();

            if (totalPatient != null) {
                day = 30;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid30.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid30').val();

            if (totalPatient != null) {
                day = 30;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateValid31.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientValid31').val();

            if (totalPatient != null) {
                day = 31;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Valid';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });

        $('.grdPatientInformation .lblDetailPerDateVoid31.lblLink').live('click', function () {
            departmentID = $.trim($(this).closest('tr').find('.keyField').html());
            paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            month = cboMonth.GetValue();
            year = cboYear.GetText();

            $tr = $(this).closest('tr');
            totalPatient = $tr.find('.hdnPatientVoid31').val();

            if (totalPatient != null) {
                day = 31;
                param = departmentID + '|' + paramedicID + '|' + day + '|' + month + '|' + year + '|' + 'Void';

                if (totalPatient != 0) {
                    var url = ResolveUrl("~/Program/Information/PatientInformationPerDoctorDtPerDate.ascx");
                    openUserControlPopup(url, param, 'Detail Information Per Date', 800, 550);
                } else {
                    showToast('Warning', 'No data to display');
                }
            }
        });
        //#endregion

        $('#<%:chkIncludeInpatient.ClientID %>').live('change', function () {
            cbpView.PerformCallback('refresh');
        });
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <table width="100%">
        <tr>
            <td>
                <table style="width: 550px;">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 400px" />
                    </colgroup>
                    <tr>
                        <td>
                            <label>
                                <%=GetLabel("Tahun") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboYear" runat="server" ClientInstanceName="cboYear" Width="250px">
                                <ClientSideEvents ValueChanged="function(s,e){ onDateChanged(s);}" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                <%=GetLabel("Bulan") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboMonth" runat="server" ClientInstanceName="cboMonth" Width="250px">
                                <ClientSideEvents ValueChanged="function(s,e){ onDateChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        <asp:CheckBox ID="chkIncludeInpatient" runat="server" /><%:GetLabel("Termasuk Pasien Rawat Inap")%>
                        </td>
                    </tr>
                </table>
                <div style="text-align: right;">
                    <%=GetLabel("NOTES : V (Registered) | X (Cancelled)")%>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){showLoadingPanel();}" EndCallback="function(s,e){hideLoadingPanel();}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdPatientInformation grdSelected" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th class="keyField" rowspan="3">
                                                    &nbsp;
                                                </th>
                                                <th rowspan="3" style="width: 150px">
                                                    <%=GetLabel("Department") %>
                                                </th>
                                                <th colspan="62">
                                                    <%=GetLabel("Tanggal") %>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th colspan="2" style="width: 10px">
                                                    1
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    2
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    3
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    4
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    5
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    6
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    7
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    8
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    9
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    10
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    11
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    12
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    13
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    14
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    15
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    16
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    17
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    18
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    19
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    20
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    21
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    22
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    23
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    24
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    25
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    26
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    27
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    28
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    29
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    30
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    31
                                                </th>
                                            </tr>
                                            <tr>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="64">
                                                    <%=GetLabel("No Data To Display") %>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdPatientInformation grdSelected" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th class="keyField" rowspan="3">
                                                    &nbsp;
                                                </th>
                                                <th rowspan="3" style="width: 150px">
                                                    <%=GetLabel("Department") %>
                                                </th>
                                                <th colspan="62">
                                                    <%=GetLabel("Tanggal") %>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th colspan="2" style="width: 10px">
                                                    1
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    2
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    3
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    4
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    5
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    6
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    7
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    8
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    9
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    10
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    11
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    12
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    13
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    14
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    15
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    16
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    17
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    18
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    19
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    20
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    21
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    22
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    23
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    24
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    25
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    26
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    27
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    28
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    29
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    30
                                                </th>
                                                <th colspan="2" style="width: 10px">
                                                    31
                                                </th>
                                            </tr>
                                            <tr>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                                <th>
                                                    V
                                                </th>
                                                <th>
                                                    X
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="keyField">
                                                <%#: Eval("DepartmentID") %>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetail">
                                                    <%#: Eval("DepartmentName") %>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid01">
                                                    <div <%# Eval("Tgl1_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl1_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid01">
                                                    <div <%# Eval("Tgl1_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl1_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid02">
                                                    <div <%# Eval("Tgl2_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl2_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid02">
                                                    <div <%# Eval("Tgl2_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl2_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid03">
                                                    <div <%# Eval("Tgl3_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl3_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid03">
                                                    <div <%# Eval("Tgl3_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl3_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid04">
                                                    <div <%# Eval("Tgl4_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl4_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid04">
                                                    <div <%# Eval("Tgl4_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl4_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid05">
                                                    <div <%# Eval("Tgl5_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl5_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid05">
                                                    <div <%# Eval("Tgl5_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl5_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid06">
                                                    <div <%# Eval("Tgl6_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl6_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid06">
                                                    <div <%# Eval("Tgl6_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl6_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid07">
                                                    <div <%# Eval("Tgl7_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl7_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid07">
                                                    <div <%# Eval("Tgl7_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl7_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid08">
                                                    <div <%# Eval("Tgl8_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl8_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid08">
                                                    <div <%# Eval("Tgl8_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl8_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid09">
                                                    <div <%# Eval("Tgl9_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl9_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid09">
                                                    <div <%# Eval("Tgl9_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl9_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid10">
                                                    <div <%# Eval("Tgl10_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl10_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid10">
                                                    <div <%# Eval("Tgl10_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl10_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid11">
                                                    <div <%# Eval("Tgl11_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl11_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid11">
                                                    <div <%# Eval("Tgl11_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl11_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid12">
                                                    <div <%# Eval("Tgl12_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl12_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid12">
                                                    <div <%# Eval("Tgl12_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl12_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid13">
                                                    <div <%# Eval("Tgl13_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl13_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid13">
                                                    <div <%# Eval("Tgl13_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl13_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid14">
                                                    <div <%# Eval("Tgl14_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl14_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid14">
                                                    <div <%# Eval("Tgl14_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl14_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid15">
                                                    <div <%# Eval("Tgl15_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl15_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid15">
                                                    <div <%# Eval("Tgl15_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl15_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid16">
                                                    <div <%# Eval("Tgl16_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl16_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid16">
                                                    <div <%# Eval("Tgl16_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl16_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid17">
                                                    <div <%# Eval("Tgl17_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl17_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid17">
                                                    <div <%# Eval("Tgl17_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl17_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid18">
                                                    <div <%# Eval("Tgl18_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl18_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid18">
                                                    <div <%# Eval("Tgl18_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl18_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid19">
                                                    <div <%# Eval("Tgl19_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl19_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid19">
                                                    <div <%# Eval("Tgl19_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl19_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid20">
                                                    <div <%# Eval("Tgl20_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl20_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid20">
                                                    <div <%# Eval("Tgl20_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl20_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid21">
                                                    <div <%# Eval("Tgl21_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl21_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid21">
                                                    <div <%# Eval("Tgl21_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl21_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid22">
                                                    <div <%# Eval("Tgl22_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl22_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid22">
                                                    <div <%# Eval("Tgl22_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl22_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid23">
                                                    <div <%# Eval("Tgl23_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl23_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid23">
                                                    <div <%# Eval("Tgl23_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl23_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid24">
                                                    <div <%# Eval("Tgl24_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl24_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid24">
                                                    <div <%# Eval("Tgl24_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl24_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid25">
                                                    <div <%# Eval("Tgl25_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl25_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid25">
                                                    <div <%# Eval("Tgl25_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl25_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid26">
                                                    <div <%# Eval("Tgl26_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl26_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid26">
                                                    <div <%# Eval("Tgl26_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl26_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid27">
                                                    <div <%# Eval("Tgl27_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl27_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid27">
                                                    <div <%# Eval("Tgl27_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl27_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid28">
                                                    <div <%# Eval("Tgl28_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl28_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid28">
                                                    <div <%# Eval("Tgl28_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl28_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid29">
                                                    <div <%# Eval("Tgl29_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl29_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid29">
                                                    <div <%# Eval("Tgl29_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl29_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid30">
                                                    <div <%# Eval("Tgl30_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl30_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid30">
                                                    <div <%# Eval("Tgl30_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl30_void") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateValid31">
                                                    <div <%# Eval("Tgl31_valid").ToString() != "0" ? "Style='font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl31_valid") %>
                                                    </div>
                                            </td>
                                            <td>
                                                <label class="lblLink lblDetailPerDateVoid31">
                                                    <div <%# Eval("Tgl31_void").ToString() != "0" ? "Style='color:red; font-weight:bold'":"" %>>
                                                        <%#: Eval("Tgl31_void") %>
                                                    </div>
                                            </td>
                                            <input type="hidden" value='<%#:Eval("Tgl1_valid") %>' class="hdnPatientValid01" />
                                            <input type="hidden" value='<%#:Eval("Tgl1_void") %>' class="hdnPatientVoid01" />
                                            <input type="hidden" value='<%#:Eval("Tgl2_valid") %>' class="hdnPatientValid02" />
                                            <input type="hidden" value='<%#:Eval("Tgl2_void") %>' class="hdnPatientVoid02" />
                                            <input type="hidden" value='<%#:Eval("Tgl3_valid") %>' class="hdnPatientValid03" />
                                            <input type="hidden" value='<%#:Eval("Tgl3_void") %>' class="hdnPatientVoid03" />
                                            <input type="hidden" value='<%#:Eval("Tgl4_valid") %>' class="hdnPatientValid04" />
                                            <input type="hidden" value='<%#:Eval("Tgl4_void") %>' class="hdnPatientVoid04" />
                                            <input type="hidden" value='<%#:Eval("Tgl5_valid") %>' class="hdnPatientValid05" />
                                            <input type="hidden" value='<%#:Eval("Tgl5_void") %>' class="hdnPatientVoid05" />
                                            <input type="hidden" value='<%#:Eval("Tgl6_valid") %>' class="hdnPatientValid06" />
                                            <input type="hidden" value='<%#:Eval("Tgl6_void") %>' class="hdnPatientVoid06" />
                                            <input type="hidden" value='<%#:Eval("Tgl7_valid") %>' class="hdnPatientValid07" />
                                            <input type="hidden" value='<%#:Eval("Tgl7_void") %>' class="hdnPatientVoid07" />
                                            <input type="hidden" value='<%#:Eval("Tgl8_valid") %>' class="hdnPatientValid08" />
                                            <input type="hidden" value='<%#:Eval("Tgl8_void") %>' class="hdnPatientVoid08" />
                                            <input type="hidden" value='<%#:Eval("Tgl9_valid") %>' class="hdnPatientValid09" />
                                            <input type="hidden" value='<%#:Eval("Tgl9_void") %>' class="hdnPatientVoid09" />
                                            <input type="hidden" value='<%#:Eval("Tgl10_valid") %>' class="hdnPatientValid10" />
                                            <input type="hidden" value='<%#:Eval("Tgl10_void") %>' class="hdnPatientVoid10" />
                                            <input type="hidden" value='<%#:Eval("Tgl11_valid") %>' class="hdnPatientValid11" />
                                            <input type="hidden" value='<%#:Eval("Tgl11_void") %>' class="hdnPatientVoid11" />
                                            <input type="hidden" value='<%#:Eval("Tgl12_valid") %>' class="hdnPatientValid12" />
                                            <input type="hidden" value='<%#:Eval("Tgl12_void") %>' class="hdnPatientVoid12" />
                                            <input type="hidden" value='<%#:Eval("Tgl13_valid") %>' class="hdnPatientValid13" />
                                            <input type="hidden" value='<%#:Eval("Tgl13_void") %>' class="hdnPatientVoid13" />
                                            <input type="hidden" value='<%#:Eval("Tgl14_valid") %>' class="hdnPatientValid14" />
                                            <input type="hidden" value='<%#:Eval("Tgl14_void") %>' class="hdnPatientVoid14" />
                                            <input type="hidden" value='<%#:Eval("Tgl15_valid") %>' class="hdnPatientValid15" />
                                            <input type="hidden" value='<%#:Eval("Tgl15_void") %>' class="hdnPatientVoid15" />
                                            <input type="hidden" value='<%#:Eval("Tgl16_valid") %>' class="hdnPatientValid16" />
                                            <input type="hidden" value='<%#:Eval("Tgl16_void") %>' class="hdnPatientVoid16" />
                                            <input type="hidden" value='<%#:Eval("Tgl17_valid") %>' class="hdnPatientValid17" />
                                            <input type="hidden" value='<%#:Eval("Tgl17_void") %>' class="hdnPatientVoid17" />
                                            <input type="hidden" value='<%#:Eval("Tgl18_valid") %>' class="hdnPatientValid18" />
                                            <input type="hidden" value='<%#:Eval("Tgl18_void") %>' class="hdnPatientVoid18" />
                                            <input type="hidden" value='<%#:Eval("Tgl19_valid") %>' class="hdnPatientValid19" />
                                            <input type="hidden" value='<%#:Eval("Tgl19_void") %>' class="hdnPatientVoid19" />
                                            <input type="hidden" value='<%#:Eval("Tgl20_valid") %>' class="hdnPatientValid20" />
                                            <input type="hidden" value='<%#:Eval("Tgl20_void") %>' class="hdnPatientVoid20" />
                                            <input type="hidden" value='<%#:Eval("Tgl21_valid") %>' class="hdnPatientValid21" />
                                            <input type="hidden" value='<%#:Eval("Tgl21_void") %>' class="hdnPatientVoid21" />
                                            <input type="hidden" value='<%#:Eval("Tgl22_valid") %>' class="hdnPatientValid22" />
                                            <input type="hidden" value='<%#:Eval("Tgl22_void") %>' class="hdnPatientVoid22" />
                                            <input type="hidden" value='<%#:Eval("Tgl23_valid") %>' class="hdnPatientValid23" />
                                            <input type="hidden" value='<%#:Eval("Tgl23_void") %>' class="hdnPatientVoid23" />
                                            <input type="hidden" value='<%#:Eval("Tgl24_valid") %>' class="hdnPatientValid24" />
                                            <input type="hidden" value='<%#:Eval("Tgl24_void") %>' class="hdnPatientVoid24" />
                                            <input type="hidden" value='<%#:Eval("Tgl25_valid") %>' class="hdnPatientValid25" />
                                            <input type="hidden" value='<%#:Eval("Tgl25_void") %>' class="hdnPatientVoid25" />
                                            <input type="hidden" value='<%#:Eval("Tgl26_valid") %>' class="hdnPatientValid26" />
                                            <input type="hidden" value='<%#:Eval("Tgl26_void") %>' class="hdnPatientVoid26" />
                                            <input type="hidden" value='<%#:Eval("Tgl27_valid") %>' class="hdnPatientValid27" />
                                            <input type="hidden" value='<%#:Eval("Tgl27_void") %>' class="hdnPatientVoid27" />
                                            <input type="hidden" value='<%#:Eval("Tgl28_valid") %>' class="hdnPatientValid28" />
                                            <input type="hidden" value='<%#:Eval("Tgl28_void") %>' class="hdnPatientVoid28" />
                                            <input type="hidden" value='<%#:Eval("Tgl29_valid") %>' class="hdnPatientValid29" />
                                            <input type="hidden" value='<%#:Eval("Tgl29_void") %>' class="hdnPatientVoid29" />
                                            <input type="hidden" value='<%#:Eval("Tgl30_valid") %>' class="hdnPatientValid30" />
                                            <input type="hidden" value='<%#:Eval("Tgl30_void") %>' class="hdnPatientVoid30" />
                                            <input type="hidden" value='<%#:Eval("Tgl31_valid") %>' class="hdnPatientValid31" />
                                            <input type="hidden" value='<%#:Eval("Tgl31_void") %>' class="hdnPatientVoid31" />
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</asp:Content>
