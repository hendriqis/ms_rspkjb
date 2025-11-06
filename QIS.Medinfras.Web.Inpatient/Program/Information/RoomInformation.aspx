<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="RoomInformation.aspx.cs" Inherits="QIS.Medinfras.Web.Inpatient.Program.RoomInformation" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        //#region tab
        $(function () {
            $('#ulTabLabResult li').click(function () {
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerInfo').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
        });
        //#endregion

        //#region tab containerDetail
        $('#<%=rblFilter.ClientID %>').live('change', function () {
            var filter = $('#<%=rblFilter.ClientID %>').find(":checked").val();
            if (filter == "filterClass") {
                $('#<%:trServiceUnit.ClientID %>').attr('style', 'display:none');
                $('#<%:trClass.ClientID %>').removeAttr('style');
                cbpView.PerformCallback('refresh');
            } else {
                $('#<%:trServiceUnit.ClientID %>').removeAttr('style');
                $('#<%:trClass.ClientID %>').attr('style', 'display:none');
                cbpView.PerformCallback('refresh');
            }
        });

        function onCboBedPicksWardValueChanged() {
            cbpView.PerformCallback('refresh');
        }

        function onCboClassPicksValueChanged() {
            cbpView.PerformCallback('refresh');
        }

        $('#tblRoom tr').live('click', function () {
            $('#tblRoom tr.selected').removeClass('selected');
            $(this).addClass('selected');
            var id = $(this).closest("td:first").andSelf().find('input:hidden').val();
            if (typeof id != "undefined") {
                $('#<%=hdnRoomID.ClientID %>').val(id);
                cbpViewBed.PerformCallback('refresh');
            }
        });

        function onCbpViewEndCallback() {
            hideLoadingPanel();
        }

        //#endregion

        //#region tab containerSummary
        $('.lblDetailOccupied').live('click', function () {
            $tr = $(this).closest('tr');
            var TotalOccupied = $tr.find('.hdnTotalOccupied').val();
            var RoomID = $tr.find('.hdnRoomIDSumm').val();
            var ClassID = $tr.find('.hdnClassID').val();
            var param = RoomID + '|' + ClassID;

            if (TotalOccupied != 0) {
                var url = ResolveUrl("~/Program/Information/BedOccupiedInfoDtCtl.ascx");
                openUserControlPopup(url, param, 'Detail Tempat Tidur Terisi', 1200, 550);
            } else {
                showToast('Warning', 'No data to display');
            }

        });

        $('.lblDetailBedWaiting').live('click', function () {
            $tr = $(this).closest('tr');
            var TotalWaiting = $tr.find('.hdnTotalWaiting').val();
            var RoomID = $tr.find('.hdnRoomIDSumm').val();
            var ClassID = $tr.find('.hdnClassID').val();
            var param = RoomID + '|' + ClassID;

            if (TotalWaiting != 0) {
                var url = ResolveUrl("~/Program/Information/BedTransferInfoDtCtl.ascx");
                openUserControlPopup(url, param, 'Detail Tempat Tidur Menunggu Transfer', 1200, 550);
            } else {
                showToast('Warning', 'No data to display');
            }

        });

        $('.lblDetailBedBooking').live('click', function () {
            $tr = $(this).closest('tr');
            var TotalBooking = $tr.find('.hdnTotalBooking').val();
            var RoomID = $tr.find('.hdnRoomIDSumm').val();
            var ClassID = $tr.find('.hdnClassID').val();
            var param = RoomID + '|' + ClassID;

            if (TotalBooking != 0) {
                var url = ResolveUrl("~/Program/Information/BedBookingInfoDtCtl.ascx");
                openUserControlPopup(url, param, 'Detail Booking Tempat Tidur', 1200, 550);
            } else {
                showToast('Warning', 'No data to display');
            }

        });

        $('#<%=rblFilterSum.ClientID %>').live('change', function () {
            var filter = $('#<%=rblFilterSum.ClientID %>').find(":checked").val();
            if (filter == "filterClassSum") {
                $('#<%:trServiceUnitSum.ClientID %>').attr('style', 'display:none');
                $('#<%:trClassSum.ClientID %>').removeAttr('style');
                cbpViewSum.PerformCallback('refresh');
            } else {
                $('#<%:trServiceUnitSum.ClientID %>').removeAttr('style');
                $('#<%:trClassSum.ClientID %>').attr('style', 'display:none');
                cbpViewSum.PerformCallback('refresh');
            }
        });

        function onCboBedPicksWardSumValueChanged() {
            cbpViewSum.PerformCallback('refresh');
        }

        function onCboClassPicksSumValueChanged() {
            cbpViewSum.PerformCallback('refresh');
        }

        function onRefreshGridViewSum() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                cbpViewSum.PerformCallback('refresh');
            }
        }

        function onCbpViewSumEndCallback() {
            hideLoadingPanel();
        }

        //#endregion

        //#region tab containerTitipan
        $('#<%=rblFilterTitipan.ClientID %>').live('change', function () {
            var filter = $('#<%=rblFilterTitipan.ClientID %>').find(":checked").val();
            if (filter == "filterClassTitipan") {
                $('#<%:trServiceUnitTitipan.ClientID %>').attr('style', 'display:none');
                $('#<%:trClassTitipan.ClientID %>').removeAttr('style');
                cbpViewTitipan.PerformCallback('refresh');
            } else {
                $('#<%:trServiceUnitTitipan.ClientID %>').removeAttr('style');
                $('#<%:trClassTitipan.ClientID %>').attr('style', 'display:none');
                cbpViewTitipan.PerformCallback('refresh');
            }
        });

        function onCboBedPicksWardTitipanValueChanged() {
            cbpViewTitipan.PerformCallback('refresh');
        }

        function onCboClassPicksTitipanValueChanged() {
            cbpViewTitipan.PerformCallback('refresh');
        }

        function onRefreshGridViewTitipan() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                cbpViewTitipan.PerformCallback('refresh');
            }
        }

        function onCbpViewTitipanEndCallback() {
            hideLoadingPanel();
        }
        //#endregion

        //#region tab containerBooking
        $('#<%=rblFilterBedBooking.ClientID %>').live('change', function () {
            var filter = $('#<%=rblFilterBedBooking.ClientID %>').find(":checked").val();
            if (filter == "filterClassBedBooking") {
                $('#<%:trServiceUnitBedBooking.ClientID %>').attr('style', 'display:none');
                $('#<%:trClassBedBooking.ClientID %>').removeAttr('style');
                cbpViewBedBooking.PerformCallback('refresh');
            } else {
                $('#<%:trServiceUnitBedBooking.ClientID %>').removeAttr('style');
                $('#<%:trClassBedBooking.ClientID %>').attr('style', 'display:none');
                cbpViewBedBooking.PerformCallback('refresh');
            }
        });

        function onCboBedPicksWardBedBookingValueChanged() {
            cbpViewBedBooking.PerformCallback('refresh');
        }

        function onCboClassPicksBedBookingValueChanged() {
            cbpViewBedBooking.PerformCallback('refresh');
        }

        function onRefreshGridViewBedBooking() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                cbpViewBedBooking.PerformCallback('refresh');
            }
        }

        function onCbpViewBedBookingEndCallback() {
            hideLoadingPanel();
        }
        //#endregion

        //#region tab containerDischargePlan
        $('#<%=rblFilterPlan.ClientID %>').live('change', function () {
            var filter = $('#<%=rblFilterPlan.ClientID %>').find(":checked").val();
            if (filter == "filterClassPlan") {
                $('#<%:trServiceUnitPlan.ClientID %>').attr('style', 'display:none');
                $('#<%:trClassPlan.ClientID %>').removeAttr('style');
                cbpViewPatientPlan.PerformCallback('refresh');
            } else {
                $('#<%:trServiceUnitPlan.ClientID %>').removeAttr('style');
                $('#<%:trClassPlan.ClientID %>').attr('style', 'display:none');
                cbpViewPatientPlan.PerformCallback('refresh');
            }
        });

        function onCboBedPicksWardPatientPlanValueChanged() {
            cbpViewPatientPlan.PerformCallback('refresh');
        }

        function onCboClassPicksPatientPlanValueChanged() {
            cbpViewPatientPlan.PerformCallback('refresh');
        }

        function onRefreshGridViewPatientPlan() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                cbpViewPatientPlan.PerformCallback('refresh');
            }
        }

        function onCbpViewPatientPlanEndCallback() {
            hideLoadingPanel();
        }
        //#endregion

    </script>
    <style type="text/css">
        .ulBed
        {
            margin: 0;
            padding: 0;
        }
        .ulBed li
        {
            display: inline-block;
            border-radius: 5px;
            list-style-type: none;
            width: 275px;
            height: 135px;
            margin: 0 3px;
            padding: 5px;
        }
        
        .ulFooter li
        {
            display: inline-block;
            border-radius: 2px;
            list-style-type: none;
            min-width: 75px;
            height: 15px;
            margin: 0 10px;
            padding: 5px;
            font-size: 11px;
        }
        .genderStyle
        {
            font-size: 11px;
        }
        
        .fontCustom
        {
            font-size: 12px;
        }
        
        .trGenderM
        {
            background-color: blue;
        }
        .trGenderF
        {
            background-color: #FF69B4;
        }
        .liBedStatusU
        {
            background-color: #A1A4A6;
        }
        .liBedStatusW
        {
            background-color: #DEEC83;
        }
        .liBedStatusH
        {
            background-color: #B3A360;
        }
        .liBedStatusI
        {
            background-color: #F8C299;
        }
        .liBedStatusO
        {
            background-color: #4ac5e3;
        }
        .liBedStatusCo
        {
            background-color: #E7B4DE;
        }
        .liBedStatusB
        {
            background-color: #f1f262;
        }
        .liBedStatusOM
        {
            background-color: #4ac5e3;
        }
        .liBedStatusOF
        {
            background-color: #ffbdde;
        }
        
        .ulTab
        {
            margin: 0;
            padding: 0;
        }
        .ulTab li
        {
            list-style-type: none;
            width: 100px;
            height: 40px;
            margin: 0 10px;
            padding: 5px;
        }
        .TabContent
        {
            background-color: #F8C299;
        }
    </style>
    <div style="padding: 15px">
        <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
        <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
        <input type="hidden" id="hdnRoomSum" value="" runat="server" />
        <div class="containerUlTabPage" style="margin-bottom: 3px;">
            <ul class="ulTabPage" id="ulTabLabResult">
                <li contentid="containerDetail" class="selected">
                    <%=GetLabel("DETAIL")%></li>
                <li contentid="containerSummary">
                    <%=GetLabel("REKAP")%></li>
                <li contentid="containerTitipan">
                    <%=GetLabel("TITIPAN")%></li>
                <li contentid="containerBooking">
                    <%=GetLabel("DIBOOKING")%></li>
                <li contentid="containerDischargePlan">
                    <%=GetLabel("RENCANA PULANG")%></li>
            </ul>
        </div>
        <div id="containerDetail" class="containerInfo">
            <div class="pageTitle">
                <%=GetLabel("Informasi Tempat Tidur : DETAIL")%></div>
            <table class="tblContentArea" style="width: 100%">
                <colgroup>
                    <col width="120px" />
                </colgroup>
                <tr>
                    <td colspan="3">
                        <div id="ulFooter" style="text-align: center; height: 20px">
                            <asp:Repeater ID="rptFooter" runat="server">
                                <HeaderTemplate>
                                    <ul class="ulFooter">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li class="liBedStatus<%#:Eval("cfStandardCodeID") %>">
                                        <center>
                                            <%#:Eval("StandardCodeName") %></center>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div id="divShow0" class="divShow">
                            <table class="tblContentArea" style="width: 100%;">
                                <tr>
                                    <td style="padding: 5px; vertical-align: top" colspan="2">
                                        <fieldset id="Fieldset1">
                                            <table class="tblEntryContent">
                                                <tr>
                                                    <td class="tdLabel">
                                                        <%=GetLabel("Filter")%>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblFilter" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Per Ruang" Value="filterRoom" Selected="True" />
                                                            <asp:ListItem Text="Per Kelas" Value="filterClass" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr id="trServiceUnit" runat="server">
                                                    <td class="tdLabel" style="width: 120px">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Ruang Perawatan")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboBedPicksWard" ClientInstanceName="cboBedPicksWard" runat="server"
                                                            Width="180px">
                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboBedPicksWardValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr id="trClass" runat="server">
                                                    <td class="tdLabel" style="width: 120px">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Kelas")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboClassPicks" ClientInstanceName="cboClassPicks" runat="server"
                                                            Width="180px">
                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboClassPicksValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 360px; border: 1px solid #EAEAEA" valign="top">
                                        <input type="hidden" value="" id="hdnRoomID" runat="server" />
                                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent1" runat="server">
                                                    <asp:Repeater ID="rptRoom" runat="server" OnItemDataBound="rptRoom_ItemDataBound">
                                                        <HeaderTemplate>
                                                            <div style="overflow-y: scroll; max-height: 500px">
                                                                <table class="grdSelected" id="tblRoom">
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="text-align: center">
                                                                    <input id="Hidden1" type="hidden" value='<%#: Eval("RoomID")%>' class="RoomID" runat="server" />
                                                                    <h2>
                                                                        <%#: Eval("RoomCode")%></h2>
                                                                </td>
                                                                <td>
                                                                    <div class="tblRoomDetail" onclick='return false;'>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <colgroup>
                                                                                <col width="100px" />
                                                                                <col width="10px" />
                                                                                <col />
                                                                            </colgroup>
                                                                            <tr>
                                                                                <td style="font-style: italic">
                                                                                    <%=GetLabel("Kelas")%>
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td>
                                                                                    <%#:Eval("ClassName")%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-style: italic">
                                                                                    <%=GetLabel("Tempat Tidur")%>
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td>
                                                                                    <span id="spnRoomInformation" runat="server"></span>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-style: italic">
                                                                                    <%=GetLabel("Catatan")%>
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td>
                                                                                    <span id="spnRoomRemarks" runat="server"></span>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table> </div>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                    </td>
                                    <td align="left" id="footer" valign="top" style="width: auto; height: 500px;">
                                        <div style="height: 500px; overflow-y: scroll;">
                                            <dxcp:ASPxCallbackPanel ID="cbpViewBed" runat="server" Width="100%" ClientInstanceName="cbpViewBed"
                                                ShowLoadingPanel="false" OnCallback="cbpViewBed_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                                        <asp:Repeater ID="rptBed" runat="server">
                                                            <HeaderTemplate>
                                                                <ul class="ulBed grdSelected" id="ulBedPicksBedSelected">
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <li class='liBedStatus<%#:Eval("BedCodeSuffix")%>'>
                                                                    <table class="fontCustom" width="100%">
                                                                        <tr>
                                                                            <td rowspan="3" valign="top" style="width: 80px;">
                                                                                <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="100px"
                                                                                    width="75px" /><br />
                                                                                <input type="hidden" value='<%#: Eval("GCGender")%>' class="hdnPatientGender" />
                                                                                <%--<div style="margin-top: 5px;color: black; font-weight:bold;text-align:center "><%#:Eval("MedicalNo")%></div>--%>
                                                                            </td>
                                                                            <td style="vertical-align: top">
                                                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td style="font-weight: bold; font-size: 1.3em">
                                                                                            <div style="float: left; padding-top: 1px">
                                                                                                <%#:Eval("MedicalNo")%></div>
                                                                                            <div style="float: right;">
                                                                                                <%#:Eval("BedCode")%></div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <hr style="border-color: White; height: 1px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="font-weight: bold">
                                                                                            <%#:Eval("Salutation")%> <%#:Eval("PatientName")%>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <input id="hdnRegistrationID" type="hidden" value='<%#: Eval("RegistrationID")%>'
                                                                                        class="hdnRegistrationID" runat="server" />
                                                                                    <tr>
                                                                                        <td>
                                                                                            <%#:Eval("RegistrationNo")%>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <%#:Eval("PatientAge")%>,
                                                                                            <%#:Eval("Religion")%>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <%#:Eval("ParamedicName")%>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span style='color: Red; display: <%#: Eval("CustomPlanDischargeDate") == "" ? "none" : "" %>'>
                                                                                                <%=GetLabel("Rencana Pulang : ")%>
                                                                                                <%#:Eval("CustomPlanDischargeDate")%></span>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span style='color: Purple'>
                                                                                                <%#:Eval("cfLastUnoccupiedDate")%></span>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <input id="hdnBedID" type="hidden" value='<%#: Eval("BedID")%>' class="hdnBedID"
                                                                        runat="server" />
                                                                </li>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </ul>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="containerSummary" class="containerInfo" style="display: none">
            <div class="pageTitle">
                <%=GetLabel("Informasi Tempat Tidur : REKAP")%></div>
            <table class="tblContentArea" style="width: 100%">
                <colgroup>
                    <col width="120px" />
                </colgroup>
                <tr>
                    <td colspan="2">
                        <div id="divShow1" class="divShow">
                            <table class="tblContentArea" style="width: 100%;">
                                <tr>
                                    <td style="padding: 5px; vertical-align: top" colspan="2">
                                        <fieldset id="Fieldset2">
                                            <table class="tblEntryContent">
                                                <tr>
                                                    <td class="tdLabel">
                                                        <%=GetLabel("Filter")%>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblFilterSum" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Per Ruang" Value="filterRoomSum" Selected="True" />
                                                            <asp:ListItem Text="Per Kelas" Value="filterClassSum" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr id="trServiceUnitSum" runat="server">
                                                    <td class="tdLabel" style="width: 120px">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Ruang Perawatan")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboBedPicksWardSum" ClientInstanceName="cboBedPicksWardSum"
                                                            runat="server" Width="180px">
                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboBedPicksWardSumValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr id="trClassSum" runat="server">
                                                    <td class="tdLabel" style="width: 120px">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Kelas")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboClassPicksSum" ClientInstanceName="cboClassPicksSum" runat="server"
                                                            Width="180px">
                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboClassPicksSumValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <dxcp:ASPxCallbackPanel ID="cbpViewSum" runat="server" Width="100%" ClientInstanceName="cbpViewSum"
                                            OnCallback="cbpViewSum_Callback" ShowLoadingPanel="false">
                                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewSumEndCallback(); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent3" runat="server">
                                                    <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                                        margin-right: auto; position: relative; font-size: 0.95em; height: 500px; overflow-y: scroll;">
                                                        <table id="grdView" class="grdSelected grdPatientPage" cellspacing="0" rules="all">
                                                            <asp:ListView ID="lvwView" runat="server">
                                                                <EmptyDataTemplate>
                                                                    <table id="tblViewSum" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                                        rules="all">
                                                                        <tr>
                                                                            <th style="width: 251px" rowspan="2" align="center">
                                                                                <%=GetLabel("KAMAR PERAWATAN")%>
                                                                            </th>
                                                                            <th style="width: 150px" rowspan="2" align="center">
                                                                                <%=GetLabel("KELAS")%>
                                                                            </th>
                                                                            <th colspan="6" align="center">
                                                                                <%=GetLabel("STATUS")%>
                                                                            </th>
                                                                            <th style="width: 100px" rowspan="2" align="center">
                                                                                <%=GetLabel("TOTAL BED")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Dibooking")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Sedang Dibersihkan")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Tutup")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Menunggu Transfer")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Terisi")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Kosong")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr class="trEmpty">
                                                                            <td colspan="9">
                                                                                <%=GetLabel("No Data To Display") %>
                                                                            </td>
                                                                        </tr>
                                                                        </tr>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                                <LayoutTemplate>
                                                                    <table id="tblViewSum" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                                        rules="all">
                                                                        <tr>
                                                                            <th style="width: 251px" rowspan="2" align="center">
                                                                                <%=GetLabel("KAMAR PERAWATAN")%>
                                                                            </th>
                                                                            <th style="width: 150px" rowspan="2" align="center">
                                                                                <%=GetLabel("KELAS")%>
                                                                            </th>
                                                                            <th colspan="6" align="center">
                                                                                <%=GetLabel("STATUS")%>
                                                                            </th>
                                                                            <th style="width: 100px" rowspan="2" align="center">
                                                                                <%=GetLabel("TOTAL BED")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Dibooking")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Sedang Dibersihkan")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Tutup")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Menunggu Transfer")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Terisi")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="center">
                                                                                <%=GetLabel("Kosong")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr runat="server" id="itemPlaceholder">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                            <%#: Eval("RoomName")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#: Eval("ClassName")%>
                                                                        </td>
                                                                        <%--                                                                        <td align="center">
                                                                            <%#:Eval("BedBooking", "{0, 0:N2}")%>
                                                                        </td>--%>
                                                                        <td align="center">
                                                                            <label class="lblLink lblDetailBedBooking">
                                                                                <%#:Eval("BedBooking", "{0, 0:N2}")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#:Eval("BedCleaned", "{0, 0:N2}")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#:Eval("BedClosed", "{0, 0:N2}")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <label class="lblLink lblDetailBedWaiting">
                                                                                <%#:Eval("BedWaiting", "{0, 0:N2}")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <label class="lblLink lblDetailOccupied">
                                                                                <%#:Eval("BedOccupied", "{0, 0:N2}")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#:Eval("BedEmpty", "{0, 0:N2}")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#:Eval("TotalBed", "{0, 0:N2}")%>
                                                                        </td>
                                                                        <input type="hidden" value='<%#:Eval("RoomID") %>' class="hdnRoomIDSumm" />
                                                                        <input type="hidden" value='<%#:Eval("ClassID") %>' class="hdnClassID" />
                                                                        <input type="hidden" value='<%#:Eval("BedOccupied") %>' class="hdnTotalOccupied" />
                                                                        <input type="hidden" value='<%#:Eval("BedWaiting") %>' class="hdnTotalWaiting" />
                                                                        <input type="hidden" value='<%#:Eval("BedBooking") %>' class="hdnTotalBooking" />
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:ListView>
                                                        </table>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="containerTitipan" class="containerInfo" style="display: none">
            <div class="pageTitle">
                <%=GetLabel("Informasi Tempat Tidur : TITIPAN")%></div>
            <table class="tblContentArea" style="width: 100%">
                <colgroup>
                    <col width="120px" />
                </colgroup>
                <tr>
                    <td colspan="2">
                        <div id="div2" class="divShow">
                            <table class="tblContentArea" style="width: 100%;">
                                <tr>
                                    <td style="padding: 5px; vertical-align: top" colspan="2">
                                        <fieldset id="Fieldset5">
                                            <table class="tblEntryContent">
                                                <tr>
                                                    <td class="tdLabel">
                                                        <%=GetLabel("Filter")%>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblFilterTitipan" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Per Ruang" Value="filterRoomTitipan" Selected="True" />
                                                            <asp:ListItem Text="Per Kelas" Value="filterClassTitipan" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr id="trServiceUnitTitipan" runat="server">
                                                    <td class="tdLabel" style="width: 120px">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Ruang Perawatan")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboBedPicksWardTitipan" ClientInstanceName="cboBedPicksWardTitipan"
                                                            runat="server" Width="180px">
                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboBedPicksWardTitipanValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr id="trClassTitipan" runat="server">
                                                    <td class="tdLabel" style="width: 120px">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Kelas")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboClassPicksTitipan" ClientInstanceName="cboClassPicksTitipan"
                                                            runat="server" Width="180px">
                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboClassPicksTitipanValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <dxcp:ASPxCallbackPanel ID="cbpViewTitipan" runat="server" Width="100%" ClientInstanceName="cbpViewTitipan"
                                            OnCallback="cbpViewTitipan_Callback" ShowLoadingPanel="false">
                                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewTitipanEndCallback(); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent6" runat="server">
                                                    <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                                                        position: relative; font-size: 0.95em; height: 500px; overflow-y: scroll;">
                                                        <table id="Table3" class="grdSelected grdPatientPage" cellspacing="0" rules="all">
                                                            <asp:ListView ID="lstViewTitipan" runat="server">
                                                                <EmptyDataTemplate>
                                                                    <table id="tblViewTitipan" runat="server" class="grdCollapsible lstViewTitipan" cellspacing="0"
                                                                        rules="all">
                                                                        <tr>
                                                                            <th style="width: 150px" align="left">
                                                                                <%=GetLabel("NO. REGISTRASI")%>
                                                                            </th>
                                                                            <th align="left">
                                                                                <%=GetLabel("PASIEN")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="left">
                                                                                <%=GetLabel("RUANG TITIPAN")%>
                                                                            </th>
                                                                            <th style="width: 200px" align="left">
                                                                                <%=GetLabel("BED TITIPAN")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="left">
                                                                                <%=GetLabel("KELAS TUJUAN")%>
                                                                            </th>
                                                                            <th style="width: 70px" align="left">
                                                                                <%=GetLabel("STATUS")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr class="trEmpty">
                                                                            <td colspan="5">
                                                                                <%=GetLabel("No Data To Display") %>
                                                                            </td>
                                                                        </tr>
                                                                        </tr>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                                <LayoutTemplate>
                                                                    <table id="tblViewTitipan" runat="server" class="grdCollapsible lstViewTitipan" cellspacing="0"
                                                                        rules="all">
                                                                        <tr>
                                                                            <th style="width: 150px" align="left">
                                                                                <%=GetLabel("NO. REGISTRASI")%>
                                                                            </th>
                                                                            <th align="left">
                                                                                <%=GetLabel("PASIEN")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="left">
                                                                                <%=GetLabel("RUANG TITIPAN")%>
                                                                            </th>
                                                                            <th style="width: 200px" align="left">
                                                                                <%=GetLabel("BED TITIPAN")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="left">
                                                                                <%=GetLabel("KELAS TUJUAN")%>
                                                                            </th>
                                                                            <th style="width: 70px" align="left">
                                                                                <%=GetLabel("STATUS")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr runat="server" id="itemPlaceholder">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <%#: Eval("RegistrationNo")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <i>
                                                                                <%#: Eval("MedicalNo")%>
                                                                            </i>
                                                                            <%#: Eval("Salutation")%>
                                                                            <%#: Eval("PatientName")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("RoomName")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("BedCode")%>
                                                                            (<%#: Eval("ClassName")%>)
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("RequestClassName")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("VisitStatus")%>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:ListView>
                                                        </table>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="containerBooking" class="containerInfo" style="display: none">
            <div class="pageTitle">
                <%=GetLabel("Informasi Tempat Tidur : DIBOOKING")%></div>
            <table class="tblContentArea" style="width: 100%">
                <colgroup>
                    <col width="120px" />
                </colgroup>
                <tr>
                    <td colspan="2">
                        <div id="div4" class="divShow">
                            <table class="tblContentArea" style="width: 100%;">
                                <tr>
                                    <td style="padding: 5px; vertical-align: top" colspan="2">
                                        <fieldset id="Fieldset4">
                                            <table class="tblEntryContent">
                                                <tr>
                                                    <td class="tdLabel">
                                                        <%=GetLabel("Filter")%>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblFilterBedBooking" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Per Ruang" Value="filterRoomBedBooking" Selected="True" />
                                                            <asp:ListItem Text="Per Kelas" Value="filterClassBedBooking" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr id="trServiceUnitBedBooking" runat="server">
                                                    <td class="tdLabel" style="width: 120px">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Ruang Perawatan")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboBedPicksWardBedBooking" ClientInstanceName="cboBedPicksWardBedBooking"
                                                            runat="server" Width="180px">
                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboBedPicksWardBedBookingValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr id="trClassBedBooking" runat="server">
                                                    <td class="tdLabel" style="width: 120px">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Kelas")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboClassPicksBedBooking" ClientInstanceName="cboClassPicksBedBooking"
                                                            runat="server" Width="180px">
                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboClassPicksBedBookingValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <dxcp:ASPxCallbackPanel ID="cbpViewBedBooking" runat="server" Width="100%" ClientInstanceName="cbpViewBedBooking"
                                            OnCallback="cbpViewBedBooking_Callback" ShowLoadingPanel="false">
                                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewBedBookingEndCallback(); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent5" runat="server">
                                                    <asp:Panel runat="server" ID="pnlGridView4" Style="width: 100%; margin-left: auto;
                                                        margin-right: auto; position: relative; font-size: 0.95em; height: 500px; overflow-y: scroll;">
                                                        <table id="Table2" class="grdSelected grdPatientPage" cellspacing="0" rules="all">
                                                            <asp:ListView ID="lvwViewBedBooking" runat="server">
                                                                <EmptyDataTemplate>
                                                                    <table id="tblViewBedBooking" runat="server" class="grdCollapsible lvwViewBedBooking"
                                                                        cellspacing="0" rules="all">
                                                                        <tr>
                                                                            <th style="width: 125px" align="left">
                                                                                <%=GetLabel("KAMAR PERAWATAN")%>
                                                                            </th>
                                                                            <th style="width: 90px" align="left">
                                                                                <%=GetLabel("KELAS")%>
                                                                            </th>
                                                                            <th style="width: 70px" align="left">
                                                                                <%=GetLabel("BED")%>
                                                                            </th>
                                                                            <th style="width: 150px" align="left">
                                                                                <%=GetLabel("NO. REGISTRASI")%>
                                                                            </th>
                                                                            <th style="width: 250px" align="left">
                                                                                <%=GetLabel("PASIEN ")%>
                                                                            </th>
                                                                            <th style="width: 50px" align="left">
                                                                                <%=GetLabel("STATUS")%>
                                                                            </th>
                                                                            <th style="width: 50px" align="left">
                                                                                <%=GetLabel("CATATAN")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr class="trEmpty">
                                                                            <td colspan="5">
                                                                                <%=GetLabel("No Data To Display") %>
                                                                            </td>
                                                                        </tr>
                                                                        </tr>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                                <LayoutTemplate>
                                                                    <table id="tblViewBedBooking" runat="server" class="grdCollapsible lvwViewBedBooking"
                                                                        cellspacing="0" rules="all">
                                                                        <tr>
                                                                            <th style="width: 125px" align="left">
                                                                                <%=GetLabel("KAMAR PERAWATAN")%>
                                                                            </th>
                                                                            <th style="width: 90px" align="left">
                                                                                <%=GetLabel("KELAS")%>
                                                                            </th>
                                                                            <th style="width: 70px" align="left">
                                                                                <%=GetLabel("BED")%>
                                                                            </th>
                                                                            <th style="width: 150px" align="left">
                                                                                <%=GetLabel("NO. REGISTRASI")%>
                                                                            </th>
                                                                            <th style="width: 250px" align="left">
                                                                                <%=GetLabel("PASIEN ")%>
                                                                            </th>
                                                                            <th style="width: 50px" align="left">
                                                                                <%=GetLabel("STATUS")%>
                                                                            </th>
                                                                            <th style="width: 50px" align="left">
                                                                                <%=GetLabel("CATATAN")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr runat="server" id="itemPlaceholder">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <%#: Eval("RoomName")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("ClassName")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("BedCode")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("RegistrationNo")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("cfPasien")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("BedStatus")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("Remarks")%>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:ListView>
                                                        </table>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="containerDischargePlan" class="containerInfo" style="display: none">
            <div class="pageTitle">
                <%=GetLabel("Informasi Tempat Tidur : RENCANA PULANG")%></div>
            <table class="tblContentArea" style="width: 100%">
                <colgroup>
                    <col width="120px" />
                </colgroup>
                <tr>
                    <td colspan="3">
                        <div id="divShow2" class="divShow">
                            <table class="tblContentArea" style="width: 100%;">
                                <tr>
                                    <td style="padding: 5px; vertical-align: top" colspan="2">
                                        <fieldset id="Fieldset3">
                                            <table class="tblEntryContent">
                                                <tr>
                                                    <td class="tdLabel">
                                                        <%=GetLabel("Filter")%>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblFilterPlan" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Per Ruang" Value="filterRoomPlan" Selected="True" />
                                                            <asp:ListItem Text="Per Kelas" Value="filterClassPlan" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr id="trServiceUnitPlan" runat="server">
                                                    <td class="tdLabel" style="width: 120px">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Ruang Perawatan")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboBedPicksWardPatientPlan" ClientInstanceName="cboBedPicksWardPatientPlan"
                                                            runat="server" Width="180px">
                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboBedPicksWardPatientPlanValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr id="trClassPlan" runat="server">
                                                    <td class="tdLabel" style="width: 120px">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Kelas")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboClassPicksPatientPlan" ClientInstanceName="cboClassPicksPatientPlan"
                                                            runat="server" Width="180px">
                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboClassPicksPatientPlanValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <dxcp:ASPxCallbackPanel ID="cbpViewPatientPlan" runat="server" Width="100%" ClientInstanceName="cbpViewPatientPlan"
                                            OnCallback="cbpViewPatientPlan_Callback" ShowLoadingPanel="false">
                                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewPatientPlanEndCallback(); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent4" runat="server">
                                                    <asp:Panel runat="server" ID="pnlGridView2" Style="width: 100%; margin-left: auto;
                                                        margin-right: auto; position: relative; font-size: 0.95em; height: 500px; overflow-y: scroll;">
                                                        <table id="Table1" class="grdSelected grdPatientPage" cellspacing="0" rules="all">
                                                            <asp:ListView ID="lvwViewPatientPlan" runat="server">
                                                                <EmptyDataTemplate>
                                                                    <table id="tblViewPatientPlan" runat="server" class="grdCollapsible lvwViewPatientPlan"
                                                                        cellspacing="0" rules="all">
                                                                        <tr>
                                                                            <th style="width: 180px" align="Left">
                                                                                <%=GetLabel("KAMAR PERAWATAN")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="left">
                                                                                <%=GetLabel("KELAS")%>
                                                                            </th>
                                                                            <th style="width: 80px" align="left">
                                                                                <%=GetLabel("BED")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="left">
                                                                                <%=GetLabel("NO REGISTRASI")%>
                                                                            </th>
                                                                            <th style="width: 250px" align="left">
                                                                                <%=GetLabel("PASIEN ")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="left">
                                                                                <%=GetLabel("RENCANA PULANG ")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr class="trEmpty">
                                                                            <td colspan="6">
                                                                                <%=GetLabel("No Data To Display") %>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                                <LayoutTemplate>
                                                                    <table id="tblViewPatientPlan" runat="server" class="grdCollapsible lvwViewPatientPlan"
                                                                        cellspacing="0" rules="all">
                                                                        <tr>
                                                                            <th style="width: 180px" align="left">
                                                                                <%=GetLabel("KAMAR PERAWATAN")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="left">
                                                                                <%=GetLabel("KELAS")%>
                                                                            </th>
                                                                            <th style="width: 80px" align="left">
                                                                                <%=GetLabel("BED")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="left">
                                                                                <%=GetLabel("NO REGISTRASI")%>
                                                                            </th>
                                                                            <th style="width: 250px" align="left">
                                                                                <%=GetLabel("PASIEN ")%>
                                                                            </th>
                                                                            <th style="width: 100px" align="left">
                                                                                <%=GetLabel("RENCANA PULANG ")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr runat="server" id="itemPlaceholder">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <%#: Eval("RoomName")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("ClassName")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("BedCode")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("RegistrationNo")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("cfPasien")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("cfPlanDischargeDate")%>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:ListView>
                                                        </table>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
