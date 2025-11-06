<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewAnesthesyStatusCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.ViewAnesthesyStatusCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_viewAnesthesyStatusCtl1">
    $(function () {
        $('#leftPanel ul li').click(function () {
            $('#leftPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        $('#<%=grdAllergyView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdAllergyView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
        });
        $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();

        //#region Form Values
        if ($('#<%=hdnMonitoringCheckListValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnMonitoringCheckListValue.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent1.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent1.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent1.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent1.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }
        if ($('#<%=hdnIVCheckListValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnIVCheckListValue.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent2.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent2.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent2.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent2.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }
        if ($('#<%=hdnAccessoriesListValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnAccessoriesListValue.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent3.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent3.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent3.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent3.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }
        if ($('#<%=hdnPatientPositionValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnPatientPositionValue.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent4.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent4.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent4.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent4.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }
        if ($('#<%=hdnAirwayManagementValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnAirwayManagementValue.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent5.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent5.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent5.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent5.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }
        if ($('#<%=hdnRegionalAnestheticValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnRegionalAnestheticValue.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent6.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent6.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent6.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent6.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }
        //#endregion

        //#region Get Form Values
        function getFormContent1Values() {
            var controlValues = '';
            $('#<%=divFormContent1.ClientID %>').find('.ddlForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent1.ClientID %>').find('.txtForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent1.ClientID %>').find('.optForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent1.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
                $(this).prop('disabled', true);
            });

            $('#<%=hdnMonitoringCheckListValue.ClientID %>').val(controlValues);

            return controlValues;
        }

        function getFormContent2Values() {
            var controlValues = '';
            $('#<%=divFormContent2.ClientID %>').find('.ddlForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent2.ClientID %>').find('.txtForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent2.ClientID %>').find('.optForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent2.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
                $(this).prop('disabled', true);
            });

            $('#<%=hdnIVCheckListValue.ClientID %>').val(controlValues);

            return controlValues;
        }

        function getFormContent3Values() {
            var controlValues = '';
            $('#<%=divFormContent3.ClientID %>').find('.ddlForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent3.ClientID %>').find('.txtForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent3.ClientID %>').find('.optForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent3.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
                $(this).prop('disabled', true);
            });

            $('#<%=hdnAccessoriesListValue.ClientID %>').val(controlValues);

            return controlValues;
        }
        function getFormContent4Values() {
            var controlValues = '';
            $('#<%=divFormContent4.ClientID %>').find('.ddlForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent4.ClientID %>').find('.txtForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent4.ClientID %>').find('.optForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent4.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
                $(this).prop('disabled', true);
            });

            $('#<%=hdnPatientPositionValue.ClientID %>').val(controlValues);

            return controlValues;
        }
        function getFormContent5Values() {
            var controlValues = '';
            $('#<%=divFormContent5.ClientID %>').find('.ddlForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent5.ClientID %>').find('.txtForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent5.ClientID %>').find('.optForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent5.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
                $(this).prop('disabled', true);
            });

            $('#<%=hdnAirwayManagementValue.ClientID %>').val(controlValues);

            return controlValues;
        }
        function getFormContent6Values() {
            var controlValues = '';
            $('#<%=divFormContent6.ClientID %>').find('.ddlForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent6.ClientID %>').find('.txtForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent6.ClientID %>').find('.optForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
                $(this).prop('disabled', true);
            });
            $('#<%=divFormContent6.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
                $(this).prop('disabled', true);
            });

            $('#<%=hdnRegionalAnestheticValue.ClientID %>').val(controlValues);

            return controlValues;
        }
        //#endregion 

        registerCollapseExpandHandler();

        $('#leftPanel ul li').first().click();
    });

    $(function () {
        setPaging($("#allergyPaging"), parseInt('<%=gridAllergyPageCount %>'), function (page) {
            cbpDiagnosisView.PerformCallback('changepage|' + page);
        });

        setPaging($("#vitalSignPaging"), parseInt('<%=gridVitalSignPageCount %>'), function (page) {
            cbpVitalSignView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpAllergyViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();

            setPaging($("#allergyPaging"), pageCount, function (page) {
                cbpAllergyView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();
    }

    function onCbpVitalSignViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

            setPaging($("#vitalSignPaging"), pageCount, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();
    }

    function showContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("divContent");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }
</script>
<style type="text/css">
    #leftPanel
    {
        border: 1px solid #6E6E6E;
        width: 100%;
        height: 100%;
        position: relative;
    }
    #leftPanel > ul
    {
        margin: 0;
        padding: 2px;
        border-bottom: 1px groove black;
    }
    #leftPanel > ul > li
    {
        list-style-type: none;
        font-size: 15px;
        display: list-item;
        border: 1px solid #fdf5e6 !important;
        padding: 5px 8px;
        cursor: pointer;
        background-color: #87CEEB !important;
    }
    #leftPanel > ul > li.selected
    {
        background-color: #ff5722 !important;
        color: White;
    }
    .divContent
    {
        padding-left: 3px;
        min-height: 490px;
    }
</style>
<div style="width: 100%;">
    <input type="hidden" runat="server" id="hdnMRN" value="0" />
    <input type="hidden" id="hdnPageCount" runat="server" value='0' />
    <input type="hidden" id="hdnPageIndex" runat="server" value='0' />
    <input type="hidden" id="hdnVisitID" runat="server" value='0' />
    <input type="hidden" id="hdnTestOrderID" runat="server" value='0' />
    <input type="hidden" id="hdnAnesthesyStatusID" runat="server" value='0' />
    <input type="hidden" runat="server" id="hdnMonitoringCheckListLayout" value="" />
    <input type="hidden" runat="server" id="hdnMonitoringCheckListValue" value="" />
    <input type="hidden" runat="server" id="hdnIVCheckListLayout" value="" />
    <input type="hidden" runat="server" id="hdnIVCheckListValue" value="" />
    <input type="hidden" runat="server" id="hdnAccessoriesListLayout" value="" />
    <input type="hidden" runat="server" id="hdnAccessoriesListValue" value="" />
    <input type="hidden" runat="server" id="hdnPatientPositionLayout" value="" />
    <input type="hidden" runat="server" id="hdnPatientPositionValue" value="" />
    <input type="hidden" runat="server" id="hdnAirwayManagementLayout" value="" />
    <input type="hidden" runat="server" id="hdnAirwayManagementValue" value="" />
    <input type="hidden" runat="server" id="hdnRegionalAnestheticLayout" value="" />
    <input type="hidden" runat="server" id="hdnRegionalAnestheticValue" value="" />
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <colgroup>
            <col style="width: 300px" />
            <col />
        </colgroup>
        <tr>
            <td>
                <div id="lblMedicalNo" runat="server" class="w3-lime w3-xxlarge" style="text-align: center;
                    text-shadow: 1px 1px 0 #444; width: 100%">
                </div>
            </td>
            <td>
                <div id="lblTitle" runat="server" class="w3-blue-grey w3-xxlarge" style="text-align: center;
                    text-shadow: 1px 1px 0 #444">
                    <%=GetLabel("STATUS ANESTESI")%></div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPanel" class="w3-border">
                    <ul>
                        <li contentid="divPage1" title="Data Pasien" class="w3-hover-red">Data Pasien</li>
                        <li contentID="divPage2" title="Asesmen Pra Induksi" class="w3-hover-red">Asesmen Pra Induksi</li>
                        <li contentID="divPage3" title="Riwayat Alergi" class="w3-hover-red">Riwayat Alergi</li>
                        <li contentID="divPage4" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">Tanda Vital dan Indikator Lainnya</li>                                               
                        <li contentID="divPage5" title="Invasive Monitors" class="w3-hover-red">Invasive Monitors</li>      
                        <li contentID="divPage6" title="Intravenous Lines" class="w3-hover-red">Intravenous Lines</li>      
                        <li contentID="divPage7" title="Accessories" class="w3-hover-red">Accessories</li>
                        <li contentID="divPage8" title="Patient Position(s) Used During Case" class="w3-hover-red">Patient Position(s) Used During Case</li>
                        <li contentID="divPage9" title="Airway Management" class="w3-hover-red">Airway Management</li>
                        <li contentID="divPage10" title="Regional Anesthetic(s)" class="w3-hover-red">Regional Anesthetic(s)</li>
                    </ul>
                </div>
                <div>
                    <table class="w3-table-all" style="width: 100%">
                        <tr>
                            <td style="text-align: left" class="w3-blue-grey">
                                <div class=" w3-small">
                                    <%=GetLabel("Dikaji Oleh :")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <div id="lblPhysicianName2" runat="server" class="w3-medium">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="vertical-align: top; padding-left: 5px;">
                <div id="divPage1" class="w3-border divContent w3-animate-left" style="display: none">
                    <table style="margin-top: 5px; width: 100%" cellpadding="0" border="0" cellspacing="0">
                        <colgroup style="width: 130px" />
                        <colgroup style="width: 10px; text-align: center" />
                        <colgroup />
                        <colgroup style="width: 130px" />
                        <tr>
                            <td style="vertical-align: top">
                                <img style="width: 110px; height: 125px" runat="server" runat="server" id="imgPatientImage" />
                            </td>
                            <td />
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <colgroup style="width: 160px" />
                                    <colgroup style="width: 10px; text-align: center" />
                                    <colgroup />
                                    <tr>
                                        <td colspan="3" style="width: 100%">
                                            <span id="lblPatientName" runat="server" class="w3-sand w3-large" style="font-weight: bold;
                                                width: 100%"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Jenis Kelamin")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblGender" runat="server" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Tanggal Lahir (Umur)")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblDateOfBirth" runat="server" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Tanggal & Jam Registrasi")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <div id="lblRegistrationDateTime" runat="server" style="color: Black">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("No. Registrasi")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <div id="lblRegistrationNo" runat="server" style="color: Black">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("DPJP Utama")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblPhysician" runat="server" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Pembayar")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblPayerInformation" runat="server" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Lokasi Pasien")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblPatientLocation" runat="server" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel" style="vertical-align: top">
                                            <%=GetLabel("Diagnosa")%>
                                        </td>
                                        <td class="tdLabel" style="vertical-align: top">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <textarea id="lblDiagnosis" runat="server" style="border: 0; width: 100%; height: 120px;
                                                background-color: transparent" readonly></textarea>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="vertical-align: top">
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divContent w3-animate-left" style="display: none">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <colgroup>
                            <col width="100%" />
                        </colgroup>
                        <tr>
                            <td style="vertical-align: top">
                                <div class="containerTblEntryContent">
                                    <div style="max-height: 100%; overflow-y: auto; padding: 5px 0px 5px 0px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 99%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Tanggal dan Waktu")%></label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtServiceDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                                                            </td>
                                                            <td style="padding-left: 5px">
                                                                <asp:TextBox ID="txtServiceTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label id="lblOrderNo">
                                                        <%:GetLabel("Nomor Order")%></label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="1" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTestOrderNo" Width="150px" runat="server" Enabled="false" ReadOnly="true" />
                                                            </td>
                                                            <td class="tdLabel">
                                                                <label id="Label1">
                                                                    <%:GetLabel("Diagnosa Pre Op")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPreOpDiagnosisInfo" Width="100%" runat="server" Enabled="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </td>
                                            </tr>  
                                            <tr>
                                                <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                    <%=GetLabel("Rencana Tindakan") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtProcedureGroupSummary" runat="server" Width="100%" TextMode="Multiline"
                                                        Rows="2" Enabled="false" ReadOnly="true" />
                                                </td>
                                            </tr>  
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Mulai Puasa")%></label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtStartFastingDate" Width="120px" runat="server" Style="text-align: center" Enabled="false" ReadOnly="true" />
                                                            </td>
                                                            <td style="padding-left: 5px">
                                                                <asp:TextBox ID="txtStartFastingTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" ReadOnly="true" Enabled="false" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Tanggal Operasi")%></label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col width="150px" />
                                                            <col width="100px" />
                                                            <col width="80px" />
                                                            <col width="100px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtStartDate" Width="120px" Style="text-align: center" runat="server" Enabled="false" ReadOnly="true" />
                                                            </td>
                                                            <td class="tdLabel" style="padding-left : 5px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Mulai Operasi")%></label>
                                                            </td>
                                                            <td style="padding-left : 5px">
                                                                <asp:TextBox ID="txtStartTime" Width="60px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" Enabled="false" ReadOnly="true" />
                                                            </td>
                                                            <td class="tdLabel">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Lama Operasi") %></label>
                                                            </td>
                                                            <td><asp:TextBox ID="txtDuration" Width="40px" CssClass="number" runat="server" ReadOnly="true" /> menit</td>       
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>                                       
                                            </tr>       
                                            <tr>
                                                <td class="tdLabel"">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Tanggal Mulai Anestesi")%></label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col width="150px" />
                                                            <col width="100px" />
                                                            <col width="80px" />
                                                            <col width="100px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtStartAnesthesyDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                                                            </td>
                                                            <td class="tdLabel" style="padding-left : 5px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Mulai Anestesi")%></label>
                                                            </td>
                                                            <td style="padding-left : 5px">
                                                                <asp:TextBox ID="txtStartAnesthesyTime" Width="60px" CssClass="time" runat="server" Style="text-align: center" ReadOnly="true" />
                                                            </td>
                                                            <td class="tdLabel"">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Lama Anestesi")%></label>
                                                            </td>
                                                            <td><asp:TextBox ID="txtAnesthesyDuration" Width="40px" CssClass="number" runat="server" ReadOnly="true" /> menit</td>           
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Tanggal Selesai Operasi")%></label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col width="150px" />
                                                            <col width="100px" />
                                                            <col width="80px" />
                                                            <col width="100px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtStopSurgeryDate" Width="120px" Style="text-align: center" runat="server" ReadOnly="true" />
                                                            </td>
                                                            <td style="padding-left : 5px">
                                                                <asp:TextBox ID="txtStopSurgeryTime" Width="60px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Tanggal Selesai Anestesi")%></label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col width="150px" />
                                                            <col width="100px" />
                                                            <col width="80px" />
                                                            <col width="100px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtStopAnesthesyDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                                                            </td>
                                                            <td style="padding-left : 5px">
                                                                <asp:TextBox ID="txtStopAnesthesyTime" Width="60px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Tanggal Mulai Insisi")%></label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col width="150px" />
                                                            <col width="100px" />
                                                            <col width="80px" />
                                                            <col width="100px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtStartIncisionDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                                                            </td>
                                                            <td class="tdLabel" style="padding-left : 5px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Mulai Insisi")%></label>
                                                            </td>
                                                            <td style="padding-left : 5px">
                                                                <asp:TextBox ID="txtStartIncisionTime" Width="60px" CssClass="time" runat="server" Style="text-align: center" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>   
                                            <tr>
                                                <td class="tdLabel"><%=GetLabel("Status Fisik ASA") %></td>
                                                <td>
                                                    <table border="0" cellpadding="1" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButtonList ID="rblGCASAStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                                    <asp:ListItem Text=" 1" Value="X455^1" Enabled="false" />
                                                                    <asp:ListItem Text=" 2" Value="X455^2" Enabled="false" />
                                                                    <asp:ListItem Text=" 3" Value="X455^3" Enabled="false" />
                                                                    <asp:ListItem Text=" 4" Value="X455^4" Enabled="false" />
                                                                    <asp:ListItem Text=" 5" Value="X455^5" Enabled="false" />
                                                                </asp:RadioButtonList>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsASAStatusE" Checked="false" Text = " E" runat="server" Enabled="false" />
                                                            </td>
                                                            <td style="padding-left:40px">
                                                                <asp:RadioButtonList ID="rblIsASAChanged" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                                    <asp:ListItem Text=" Tidak berubah" Value="0" Enabled="false" />
                                                                    <asp:ListItem Text=" Berubah (Catatan Perubahan)" Value="1" Enabled="false" />
                                                                </asp:RadioButtonList>
                                                            </td>  
                                                        </tr>
                                                    </table>
                                                </td>  
                                            </tr>                                                                                                                                                       
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <table border="0" cellpadding="0" cellspacing="1">
                                                        <tr>
                                                            <td class="tdLabel" valign="top" style="width:120px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Catatan Perubahan Status ASA")%></label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtASAStatusRemarks" runat="server" TextMode="MultiLine" Rows="2" Width="100%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Mulai Premedikasi")%></label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col width="150px" />
                                                            <col width="100px" />
                                                            <col width="100px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtPremedicationDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                                                            </td>
                                                            <td class="tdLabel" style="padding-left : 5px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Jam Premedikasi")%></label>
                                                            </td>
                                                            <td style="padding-left : 5px">
                                                                <asp:TextBox ID="txtPremedicationTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" ReadOnly="true" />
                                                            </td>
                                                            <td style="padding-left : 5px">
                                                                <asp:RadioButtonList ID="rblGCPremedicationType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                                    <asp:ListItem Text=" Oral" Value="X456^01" Enabled="false" />
                                                                    <asp:ListItem Text=" IM" Value="X456^02" Enabled="false" />
                                                                    <asp:ListItem Text=" IV" Value="X456^03" Enabled="false" />
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr> 
                                            <tr>
                                                <td class="tdLabel" style="width: 120px">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Teknik Anestesi")%></label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="1" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 130px">
                                                                <asp:TextBox ID="txtAnesthesiaType" Width="100px" runat="server" Style="text-align: left" ReadOnly="true" />
                                                            </td>
                                                            <td class="tdLabel" style="width: 120px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Regional Anestesi")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRegionalAnesthesiaType" Width="150px" runat="server" Style="text-align: left" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left:20px" colspan="2">
                                                    <asp:RadioButtonList ID="rblIsAnesthesiaTypeChanged" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                        <asp:ListItem Text=" Tidak berubah" Value="0" />
                                                        <asp:ListItem Text=" Berubah" Value="1" />
                                                    </asp:RadioButtonList>
                                                </td> 
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <table border="0" cellpadding="0" cellspacing="1">
                                                        <tr>
                                                            <td class="tdLabel" valign="top" style="width:120px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Catatan Perubahan Teknik Anestesi")%></label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAnesthesiaTypeChangeRemarks" runat="server" TextMode="MultiLine" Rows="2" Width="100%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:CheckBox ID="chkIsTimeOutSafetyCheck" runat="server" Text=" TIME OUT (Correct : Patient, Procedure, Side/Site, Position, Special Equipment" Checked="false" Enabled="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage3" class="w3-border divContent w3-animate-left" style="display: none">
                    <div style="max-height: 450px; overflow-y: auto; padding: 5px 0px 5px 0px">
                        <table border="0" cellpadding="1" cellspacing="0" width="99%" style="margin-top: 2px">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkIsPatientAllergyExists" runat="server" Text="Tidak ada Alergi"
                                        Checked="false" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpAllergyView" runat="server" Width="100%" ClientInstanceName="cbpAllergyView"
                                        ShowLoadingPanel="false" OnCallback="cbpAllergyView_Callback">
                                        <ClientSideEvents EndCallback="function(s,e){ onCbpAllergyViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent3" runat="server">
                                                <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage3">
                                                    <asp:GridView ID="grdAllergyView" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField">
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                <ItemTemplate>
                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                    <input type="hidden" value="<%#:Eval("Allergen") %>" bindingfield="Allergen" />
                                                                    <input type="hidden" value="<%#:Eval("GCAllergenType") %>" bindingfield="GCAllergenType" />
                                                                    <input type="hidden" value="<%#:Eval("GCAllergySource") %>" bindingfield="GCAllergySource" />
                                                                    <input type="hidden" value="<%#:Eval("GCAllergySeverity") %>" bindingfield="GCAllergySeverity" />
                                                                    <input type="hidden" value="<%#:Eval("KnownDate") %>" bindingfield="KnownDate" />
                                                                    <input type="hidden" value="<%#:Eval("Reaction") %>" bindingfield="Reaction" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Allergen" HeaderText="Allergen Name" HeaderStyle-Width="200px"
                                                                HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                            <asp:BoundField DataField="AllergySource" HeaderText="Finding Source" HeaderStyle-Width="150px"
                                                                HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                            <asp:BoundField DataField="DisplayDate" HeaderText="Since" ItemStyle-HorizontalAlign="Left"
                                                                HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                            <asp:BoundField DataField="AllergySeverity" HeaderText="Severity" HeaderStyle-Width="120px"
                                                                HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                            <asp:BoundField DataField="Reaction" HeaderText="Reaction" HeaderStyle-HorizontalAlign="Left">
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada data alergi pasien dalam episode ini")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="allergyPaging">
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="divPage4" class="w3-border divContent w3-animate-left" style="display: none">
                    <div>
                        <dxcp:ASPxCallbackPanel ID="cbpVitalSignView" runat="server" Width="100%" ClientInstanceName="cbpVitalSignView"
                            ShowLoadingPanel="false" OnCallback="cbpVitalSignView_Callback">
                            <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage4">
                                        <asp:GridView ID="grdVitalSignView" runat="server" CssClass="grdSelected grdPatientPage"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                            OnRowDataBound="grdVitalSignView_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <h3>
                                                            <%=GetLabel("Tanda Vital dan Indikator Lainnya")%></h3>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("ObservationDateInString")%>,
                                                                <%#: Eval("ObservationTime") %>,
                                                                <%#: Eval("ParamedicName") %>
                                                            </b>
                                                            <br />
                                                            <span style="font-style: italic">
                                                                <%#: Eval("Remarks") %>
                                                            </span>
                                                            <br />
                                                        </div>
                                                        <div>
                                                            <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                                <ItemTemplate>
                                                                    <div style="padding-left: 20px; float: left; width: 350px;">
                                                                        <strong>
                                                                            <div style="width: 110px; float: left;" class="labelColumn">
                                                                                <%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %></div>
                                                                            <div style="width: 20px; float: left;">
                                                                                :</div>
                                                                        </strong>
                                                                        <div style="float: left;">
                                                                            <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %></div>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <br style="clear: both" />
                                                                </FooterTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada pemeriksaan tanda vital") %>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="vitalSignPaging">
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divPage5" class="w3-border divContent w3-animate-left" style="display:none">
                    <table class="tblContentArea">
                        <colgroup>
                            <col width="450px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td style="vertical-align:top" colspan="2">
                                <div id="divFormContent1" runat="server" style="height: 480px;overflow-y: auto;"></div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage6" class="w3-border divContent w3-animate-left" style="display:none">
                    <table class="tblContentArea">
                        <colgroup>
                            <col width="450px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2" style="vertical-align:top">
                                <div id="divFormContent2" runat="server" style="height: 480px;overflow-y: auto;"></div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage7" class="w3-border divContent w3-animate-left" style="display:none">
                    <table class="tblContentArea">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2" style="vertical-align:top">
                                <div id="divFormContent3" runat="server" style="height: 480px;overflow-y: auto;"></div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage8" class="w3-border divContent w3-animate-left" style="display:none">
                    <table class="tblContentArea">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2" style="vertical-align:top">
                                <div id="divFormContent4" runat="server" style="height: 480px;overflow-y: auto;"></div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage9" class="w3-border divContent w3-animate-left" style="display:none">
                    <table class="tblContentArea">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2" style="vertical-align:top">
                                <div id="divFormContent5" runat="server" style="height: 480px;overflow-y: auto;"></div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage10" class="w3-border divContent w3-animate-left" style="display:none">
                    <table class="tblContentArea">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2" style="vertical-align:top">
                                <div id="divFormContent6" runat="server" style="height: 480px;overflow-y: auto;"></div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
