<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewPostSurgeryInstructionCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.ViewPostSurgeryInstructionCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_viewPostSurgeryInstructionCtl1">
    $(function () {
        $('#leftPanel ul li').click(function () {
            $('#leftPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        $('#<%=grdMedicationView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdMedicationView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnPrescriptionOrderID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnPrescriptionOrderNo.ClientID %>').val($(this).find('.prescriptionOrderNo').html());
                cbpMedicationViewDtCtl.PerformCallback('refresh');
            }
        });
        /////$('#<%=grdMedicationView.ClientID %> tr:eq(1)').click();

        $('#<%=grdLaboratoryView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdLaboratoryView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });

        $('#<%=grdImagingView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdImagingView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });
        //#region Form Values
        if ($('#<%=hdnMonitoringValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnMonitoringValue.ClientID %>').val().split(';');
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
        if ($('#<%=hdnNutritionFormValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnNutritionFormValue.ClientID %>').val().split(';');
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
        if ($('#<%=hdnOtherInstructionValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnOtherInstructionValue.ClientID %>').val().split(';');
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

            $('#<%=hdnMonitoringValue.ClientID %>').val(controlValues);

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

            $('#<%=hdnNutritionFormValue.ClientID %>').val(controlValues);

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

            $('#<%=hdnOtherInstructionValue.ClientID %>').val(controlValues);

            return controlValues;
        }
        //#endregion 

        //#region Paging
        var pageCount = parseInt('<%=gridMedicationPageCount %>');
        $(function () {
            setPaging($("#pagingMedication"), pageCount, function (page) {
                cbpMedicationView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpMedicationViewEndCallback(s) {
            $('#containerImgLoadingMedicationView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdMedicationView.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingMedication"), pageCount, function (page) {
                    cbpMedicationView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdMedicationView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $('#leftPanel ul li').first().click();
    });

    var pageCount = parseInt('<%=gridLaboratoryPageCount %>');
    $(function () {
        setPaging($("#laboratoryPaging"), pageCount, function (page) {
            cbpLaboratoryView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpLaboratoryViewEndCallback(s) {
        var param = s.cpResult.split('|');
        var summaryText = s.cpSummary;
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdLaboratoryView.ClientID %> tr:eq(1)').click();

            setPaging($("#laboratoryPaging"), pageCount, function (page) {
                cbpLaboratoryView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdLaboratoryView.ClientID %> tr:eq(1)').click();

        $('#<%=hdnLaboratorySummary.ClientID %>').val(summaryText);
    }

    function showContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("divContent");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }
    function onCbpMedicationViewDtCtlEndCallback(s) {

        $('.imgLoadingGrdView').hide();

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
        
        .txtForm {
          border-color:#f5f6fa;
          background-color:#f5f6fa;
        }           
        .txtForm:disabled {
          border-color:#dfe6e9;
          background-color:#dfe6e9;
        }   
        .ddlForm {
          border-color:#f5f6fa;
          background-color:#f5f6fa;
        }            
        .ddlForm:disabled {
          border-color:#dfe6e9;
          background-color:#dfe6e9;
        }  
</style>
<div style="width: 100%;">
    <input type="hidden" runat="server" id="hdnMRN" value="0" />
    <input type="hidden" id="hdnPageCount" runat="server" value='0' />
    <input type="hidden" id="hdnPageIndex" runat="server" value='0' />
    <input type="hidden" id="hdnVisitID" runat="server" value='0' />
    <input type="hidden" id="hdnTestOrderID" runat="server" value='0' />
    <input type="hidden" id="hdnRecordID" runat="server" value="0" />
    <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionOrderNo" runat="server" />
    <input type="hidden" value="" id="hdnLaboratorySummary" runat="server" />
    <input type="hidden" value="" id="hdnImagingSummary" runat="server" />
    <input type="hidden" runat="server" id="hdnMonitoringLayout" value="" />
    <input type="hidden" runat="server" id="hdnMonitoringValue" value="" />
    <input type="hidden" runat="server" id="hdnNutritionLayout" value="" />
    <input type="hidden" runat="server" id="hdnNutritionFormValue" value="" />
    <input type="hidden" runat="server" id="hdnOtherInstructionLayout" value="" />
    <input type="hidden" runat="server" id="hdnOtherInstructionValue" value="" />
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
                    <%=GetLabel("INSTRUKSI PASKA BEDAH TERINTEGRASI")%></div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPanel" class="w3-border">
                    <div id="leftPageNavPanel" class="w3-border">
                        <ul>
                            <li contentID="divPage1" title="Data Pasien" class="w3-hover-red">Data Pasien</li>
                            <li contentID="divPage2" title="Observasi" class="w3-hover-red">Observasi</li>
                            <li contentID="divPage3" title="Nutrisi dan Transfusi" class="w3-hover-red">Nutrisi dan Transfusi</li>
                            <li contentID="divPage4" title="Manajemen Nyeri" class="w3-hover-red">Manajemen Nyeri</li>                                               
                            <li contentID="divPage5" title="Pemeriksaan Laboratorium" class="w3-hover-red">Pemeriksaan Laboratorium</li>      
                            <li contentID="divPage6" title="Pemeriksaan Radiologi" class="w3-hover-red">Pemeriksaan Radiologi</li>  
                            <li contentID="divPage7" title="Instruksi Lain-lain" class="w3-hover-red">Instruksi Lain-lain</li>
                        </ul>     
                    </div> 
                </div>
                <div>
                    <table class="w3-table-all" style="width: 100%">
                        <tr>
                            <td style="text-align: left" class="w3-blue-grey">
                                <div class=" w3-small">
                                    <%=GetLabel("Dibuat Oleh :")%></div>
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
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 200px" />
                            <col style="width: 200px" />
                            <col style="width: 200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal dan Waktu Pemberian Instruksi")%></label>
                            </td>
                            <td colspan="3">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtInstructionDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                                        </td>
                                        <td style="padding-left: 5px">
                                            <asp:TextBox ID="txtInstructionTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="padding-top: 5px;">
                            <td style="vertical-align:top" colspan="4">
                                <div id="divFormContent1" runat="server" style="height: 520px;overflow-y: auto;"></div>
                            </td>
                        </tr>                              
                    </table>
                </div>
                <div id="divPage3" class="w3-border divContent w3-animate-left" style="display: none">
                    <div style="max-height: 450px; overflow-y: auto; padding: 5px 0px 5px 0px">
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 200px" />
                                    <col style="width: 200px" />
                                    <col style="width: 200px" />
                                    <col />
                                </colgroup>
                                <tr style="padding-top: 5px;">
                                    <td style="vertical-align:top" colspan="4">
                                        <div id="divFormContent2" runat="server" style="height: 520px;overflow-y: auto;"></div>
                                    </td>
                                </tr>                              
                            </table>
                    </div>
                </div>
                <div id="divPage4" class="w3-border divContent w3-animate-left" style="display: none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 70%" />
                        </colgroup>
                        <tr>
                            <td valign="top">
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpMedicationView" runat="server" Width="100%" ClientInstanceName="cbpMedicationView"
                                        ShowLoadingPanel="false" OnCallback="cbpMedicationView_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingMedicationView').show(); }"
                                            EndCallback="function(s,e){ onCbpMedicationViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlMedicationView" CssClass="pnlContainerGridPatientPage">
                                                    <asp:GridView ID="grdMedicationView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="PrescriptionOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="PrescriptionOrderNo" HeaderStyle-CssClass="hiddenColumn prescriptionOrderNo"
                                                                ItemStyle-CssClass="hiddenColumn prescriptionOrderNo" />
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <div>
                                                                        <%=GetLabel("Prescription Date - Time")%>,
                                                                        <%=GetLabel("Prescription No.")%></div>
                                                                    <div style="width: 250px; float: left">
                                                                        <%=GetLabel("Physician")%></div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <div>
                                                                                    <%#: Eval("PrescriptionDateInString")%>,
                                                                                    <%#: Eval("PrescriptionTime") %>,
                                                                                    <%#: Eval("PrescriptionOrderNo")%>
                                                                                </div>
                                                                                <div style="width: 250px; float: left">
                                                                                    <%#: Eval("ParamedicName") %>
                                                                                    <b>
                                                                                        <%# Eval("IsCreatedBySystem").ToString() == "False" ? "":"(Diorder Farmasi)" %></b>
                                                                                </div>
                                                                                <div style="width: 250px; float: left; font-size: x-small; font-style: italic">
                                                                                    <%#: Eval("cfSendOrderDateInformationInString") %>
                                                                                    <%#: Eval("cfChargesProposedInformationInString") %>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada informasi order resep untuk pasien ini")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingMedicationView">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="pagingMedication">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td valign="top">
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpMedicationViewDtCtl" runat="server" Width="100%" ClientInstanceName="cbpMedicationViewDtCtl"
                                        ShowLoadingPanel="false" OnCallback="cbpMedicationViewDtCtl_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingMedicationViewDt').show(); }"
                                            EndCallback="function(s,e){ onCbpMedicationViewDtCtlEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent2" runat="server">
                                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                    <asp:GridView ID="grdMedicationViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                        OnRowDataBound="grdMedicationViewDt_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="PrescriptionOrderDetailID" HeaderStyle-CssClass="keyField"
                                                                ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="itemName" HeaderStyle-Width="220px" >
                                                                <HeaderTemplate>
                                                                    <div>
                                                                        <%=GetLabel("Drug Name")%>
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div <%# Eval("OrderIsDeleted").ToString() == "True"  || Eval("GCPrescriptionOrderStatus").ToString() == "X126^004" ? "Style='color:red;font-style:italic; text-decoration: line-through'":"" %>>
                                                                        <span style="font-weight: bold">
                                                                            <%#: Eval("cfMedicationName")%></span></div>
                                                                    <div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "white-space: pre-line;font-style:italic; padding-top:5px" %>'>
                                                                        <%#: Eval("cfCompoundDetail")%></div>
                                                                    <div>
                                                                        <%#: Eval("cfConsumeMethod3")%></div>
                                                                    <div>
                                                                        <%#: Eval("Route")%></div>
                                                                    <div>
                                                                        <%#: Eval("MedicationAdministration")%></div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Route" HeaderText="Route" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="StartDateInDatePickerFormat" HeaderText="Start Date" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="DispenseQtyInString" HeaderText="Quantity" HeaderStyle-HorizontalAlign="Right"
                                                                HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="cfTakenQty" HeaderText="Taken" HeaderStyle-HorizontalAlign="Right"
                                                                HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="ItemUnit" HeaderText="Unit" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px">
                                                                <ItemTemplate>
                                                                    <div>
                                                                        <img id="imgHAM" runat="server" class="blink-icon" src='<%# ResolveUrl("~/Libs/Images/Status/ham.png") %>'
                                                                            title='High Alert Medication' alt="" style="height: 24px; width: 24px;" /></div>
                                                                    <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsAllergyAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                                        title='<%=GetLabel("Allergy Alert") %>' alt="" />
                                                                    <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsAdverseReactionAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                                        title='<%=GetLabel("Adverse Reaction") %>' alt="" />
                                                                    <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsDuplicateTheraphyAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                                        title='<%=GetLabel("Duplicate Theraphy") %>' alt="" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="120px"
                                                                ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <div>
                                                                        <img src='<%# ResolveUrl("~/Libs/Images/Button/plus.png") %>' <%# Eval("IsCreatedFromOrder").ToString() == "False" ? "" : "Style ='display:none'" %>
                                                                            title='<%=GetLabel("Drug Add by Pharmacist") %>' alt="" width="10" height="10"
                                                                            style="padding: 2px" />
                                                                        <img src='<%# ResolveUrl("~/Libs/Images/Status/stop_service.png") %>' <%# Eval("OrderIsDeleted").ToString() == "True" || Eval("GCPrescriptionOrderStatus").ToString() == "X126^004" ? "Style='cursor:pointer;padding-right:10px'" : "Style ='display:none'" %>
                                                                            title='<%#: Eval("cfVoidReson")%>' alt="" width="15" height="15" />
                                                                    </div>
                                                                    <div <%# Eval("IsCreatedFromOrder").ToString() == "False" ? "" : "Style ='display:none'" %>>
                                                                        <i>
                                                                            <%=GetLabel("C : ")%></i>
                                                                        <%#: Eval("CreatedByUserFullName")%>
                                                                    </div>
                                                                    <div <%# Eval("OrderIsDeleted").ToString() == "True" || Eval("GCPrescriptionOrderStatus").ToString() == "X126^004" ? "" : "Style ='display:none'" %>>
                                                                        <i>
                                                                            <%=GetLabel("U : ")%></i>
                                                                        <%#: Eval("LastUpdatedByUserFullName")%>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada informasi order resep untuk pasien ini")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingMedicationViewDt">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="pagingMedicationDt">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage5" class="w3-border divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <tr>
                            <td>
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpLaboratoryView" runat="server" Width="100%" ClientInstanceName="cbpLaboratoryView"
                                        ShowLoadingPanel="false" OnCallback="cbpLaboratoryView_Callback">
                                        <ClientSideEvents EndCallback="function(s,e){ onCbpLaboratoryViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent7" runat="server">
                                                <asp:Panel runat="server" ID="Panel6" CssClass="pnlContainerGridPatientPage">
                                                    <asp:GridView ID="grdLaboratoryView" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                        OnRowDataBound="grdLaboratoryView_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                <ItemTemplate>
                                                                    <input type="hidden" value="<%#:Eval("TestOrderID") %>" bindingfield="TestOrderID" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                        <colgroup>
                                                                            <col style="width: 70%" />
                                                                            <col />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td>
                                                                                <div>
                                                                                    <%=GetLabel("Pemeriksaan") %></div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <div>
                                                                                    <%#: Eval("TestOrderDateTimeInString")%>,
                                                                                    <%#: Eval("ServiceUnitName")%>,
                                                                                    <%#: Eval("ParamedicName") %>, <span style="font-style: italic; font-weight: bold">
                                                                                        <%#: Eval("TestOrderNo")%></span></div>
                                                                                <div style="font-style: italic">
                                                                                    <asp:Repeater ID="rptLaboratoryDt" runat="server">
                                                                                        <ItemTemplate>
                                                                                            <div style="padding-left: 10px;">
                                                                                                <%#: DataBinder.Eval(Container.DataItem, "ItemName1") %>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <br style="clear: both" />
                                                                                        </FooterTemplate>
                                                                                    </asp:Repeater>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada data order pemeriksaan laboratorium untuk pasien ini")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="laboratoryPaging">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage6" class="w3-border divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <tr>
                            <td>
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpImagingView" runat="server" Width="100%" ClientInstanceName="cbpImagingView"
                                        ShowLoadingPanel="false" OnCallback="cbpImagingView_Callback">
                                        <ClientSideEvents EndCallback="function(s,e){ onCbpImagingViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent8" runat="server">
                                                <asp:Panel runat="server" ID="Panel7" CssClass="pnlContainerGridPatientPage">
                                                    <asp:GridView ID="grdImagingView" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                        OnRowDataBound="grdImagingView_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                <ItemTemplate>
                                                                    <input type="hidden" value="<%#:Eval("TestOrderID") %>" bindingfield="TestOrderID" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                        <colgroup>
                                                                            <col style="width: 70%" />
                                                                            <col />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td>
                                                                                <div>
                                                                                    <%=GetLabel("Pemeriksaan") %></div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <div>
                                                                                    <%#: Eval("TestOrderDateTimeInString")%>,
                                                                                    <%#: Eval("ServiceUnitName")%>,
                                                                                    <%#: Eval("ParamedicName") %>, <span style="font-style: italic; font-weight: bold">
                                                                                        <%#: Eval("TestOrderNo")%></span></div>
                                                                                <div style="font-style: italic">
                                                                                    <asp:Repeater ID="rptImagingDt" runat="server">
                                                                                        <ItemTemplate>
                                                                                            <div style="padding-left: 10px;">
                                                                                                <%#: DataBinder.Eval(Container.DataItem, "ItemName1") %>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <br style="clear: both" />
                                                                                        </FooterTemplate>
                                                                                    </asp:Repeater>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada data order pemeriksaan radiologi untuk pasien ini")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="imagingPaging">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage7" class="w3-border divContent w3-animate-left" style="display:none">
                    <table class="tblContentArea" style="width:100%">
                        <colgroup>
                            <col width="200px" />
                            <col width="150px" />
                            <col width="120px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="4" style="vertical-align:top">
                                <div id="divFormContent3" runat="server" style="height: 110px;overflow-y: auto;"></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <table border="0" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td class="tdLabel" valign="top" style="width:120px">
                                            <label class="lblNormal" id="lblAnesthesyRemarks">
                                                <%=GetLabel("Instruksi Lainnya")%></label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="5" Width="100%" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
