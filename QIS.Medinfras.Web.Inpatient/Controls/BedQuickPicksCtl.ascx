<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BedQuickPicksCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inpatient.Controls.BedQuickPicksCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_bedquickpicksctl">

    $('#<%=rblFilter.ClientID %>').live('change', function () {
        var filter = $('#<%=rblFilter.ClientID %>').find(":checked").val();
        if (filter == "filterClass") {
            $('#<%:trServiceUnit.ClientID %>').attr('style', 'display:none');
            $('#<%:trClass.ClientID %>').removeAttr('style');
            cbpBedPicksView.PerformCallback('refresh');
        }
        else if (filter == "filterGender") {
            $('#<%:trServiceUnit.ClientID %>').attr('style', 'display:none');
            $('#<%:trClass.ClientID %>').attr('style', 'display:none');
            cbpBedPicksView.PerformCallback('refresh');
        }
        else {
            $('#<%:trServiceUnit.ClientID %>').removeAttr('style');
            $('#<%:trClass.ClientID %>').attr('style', 'display:none');
            cbpBedPicksView.PerformCallback('refresh');
        }
    });

    function onCboBedPicksWardValueChanged() {
        cbpBedPicksView.PerformCallback('refresh');
    }

    function onCboClassPicksValueChanged() {
        cbpBedPicksView.PerformCallback('refresh');
    }

    function onCboGenderPicksValueChanged() {
        cbpBedPicksView.PerformCallback('refresh');
    }

    $('#tblBedPicksRoom > tbody > tr').die('click');
    $('#tblBedPicksRoom > tbody > tr').live('click', function () {
        var id = $(this).find('.hdnRoomID').val();
        $('#tblBedPicksRoom tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $('#<%=hdnRoomID.ClientID %>').val(id);
        cbpBedPicksViewBed.PerformCallback('refresh');
    });

    $('#ulBedPicksBedSelected li').die('click');
    $('#ulBedPicksBedSelected li').live('click', function () {
        var GCBedStatus = $(this).find('.hdnGCBedStatus').val();
        var registrationID = $(this).find('.hdnRegistrationID').val();
        var patientRegistrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        if ((registrationID == '0' && GCBedStatus == '<%=GCBedStatusUnoccupied %>') || registrationID == patientRegistrationID) {
            $selectedRoom = $('#tblBedPicksRoom tr.selected');
            var BedID = $(this).find('.hdnBedID').val();
            var Bedcode = $(this).find('.hdnBedCode').val();
            var HSUID = $(this).find('.hdnHSUID').val();
            var ServiceUnitCode = $(this).find('.hdnServiceUnitCode').val();
            var ServiceUnitName = $(this).find('.hdnServiceUnitName').val();
            var RoomID = $selectedRoom.find('.hdnRoomID').val();
            var RoomCode = $selectedRoom.find('.hdnRoomCode').val();
            var RoomName = $selectedRoom.find('.hdnRoomName').val();
            var ClassID = $selectedRoom.find('.hdnClassID').val();
            var ClassCode = $selectedRoom.find('.hdnClassCode').val();
            var ClassName = $selectedRoom.find('.hdnClassName').val();
            var ChargeClassID = $selectedRoom.find('.hdnChargeClassID').val();
            var ChargeClassCode = $selectedRoom.find('.hdnChargeClassCode').val();
            var ChargeClassName = $selectedRoom.find('.hdnChargeClassName').val();
            var ChargeClassBPJSCode = $selectedRoom.find('.hdnChargeClassBPJSCode').val();
            var ChargeClassBPJSType = $selectedRoom.find('.hdnChargeClassBPJSType').val();
            onAfterClickBedQuickPicks(HSUID, ServiceUnitCode, ServiceUnitName, RoomID, RoomCode, RoomName, ClassID, ClassCode, ClassName, ChargeClassID, ChargeClassBPJSCode, ChargeClassCode, ChargeClassName, BedID, Bedcode, ChargeClassBPJSType);
            pcRightPanelContent.Hide();
        }
        else showToast('Warning', 'Tempat tidur tidak bisa digunakan');
    });

    function onCbpBedPicksViewEndCallback() {
        hideLoadingPanel();
        $('#tblBedPicksRoom tr:eq(0)').click();
    }

    function onCbpBedPicksViewBedEndCallback() {
        hideLoadingPanel();
        Methods.checkImageError('imgPatientImage', 'patient', 'hdnPatientGender');
    }
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
        cursor: pointer;
        border-radius: 3px;
        list-style-type: none;
        width: 250px;
        height: 150px;
        margin: 3px 3px;
        padding: 3px;
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
<div style="padding: 5px; font-size: 0.85em; height: 550px">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnRoomSum" runat="server" />
    <input type="hidden" value="" id="hdnRoomID" runat="server" />
    <div class="pageTitle">
        <%=GetLabel("Pilih Tempat Tidur")%></div>
    <table class="tblContentArea" style="width: 100%">
        <colgroup>
            <col width="200px" />
        </colgroup>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <div id="ulFooter" style="text-align: center">
                    <asp:Repeater ID="rptFooter" runat="server">
                        <HeaderTemplate>
                            <ul class="ulFooter">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li class="liBedStatus<%#:Eval("cfStandardCodeID") %>">
                                <center>
                                    <b>
                                        <%#:Eval("StandardCodeName") %></b></center>
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
            <td colspan="2">
                <div id="divShow0" class="divShow">
                    <table class="tblContentArea" style="width: 100%;">
                        <tr>
                            <td style="padding: 5px; vertical-align: top" colspan="2">
                                <fieldset id="fsPatientList">
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
                                        <tr id="trGender" runat="server">
                                            <td class="tdLabel" style="width: 120px">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Gender")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboGenderPicks" ClientInstanceName="cboGenderPicks" runat="server"
                                                    Width="180px">
                                                    <ClientSideEvents ValueChanged="function(s,e) { onCboGenderPicksValueChanged(); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 250px; padding: 5px; border: 1px solid #EAEAEA" valign="top">
                                <dxcp:ASPxCallbackPanel ID="cbpBedPicksView" runat="server" Width="100%" ClientInstanceName="cbpBedPicksView"
                                    ShowLoadingPanel="false" OnCallback="cbpBedPicksView_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpBedPicksViewEndCallback(); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Repeater ID="rptRoom" runat="server" OnItemDataBound="rptRoom_ItemDataBound">
                                                <HeaderTemplate>
                                                    <div style="overflow-y: scroll; max-height: 350px">
                                                        <table class="grdSelected" id="tblBedPicksRoom">
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: center">
                                                            <input type="hidden" value='<%#: Eval("RoomID")%>' class="hdnRoomID" />
                                                            <input type="hidden" value='<%#: Eval("RoomCode")%>' class="hdnRoomCode" runat="server" />
                                                            <input type="hidden" value='<%#: Eval("RoomName")%>' class="hdnRoomName" runat="server" />
                                                            <input type="hidden" value='<%#: Eval("ClassID")%>' class="hdnClassID" runat="server" />
                                                            <input type="hidden" value='<%#: Eval("ClassCode")%>' class="hdnClassCode" runat="server" />
                                                            <input type="hidden" value='<%#: Eval("ClassName")%>' class="hdnClassName" runat="server" />
                                                            <input type="hidden" value='<%#: Eval("ChargeClassID")%>' class="hdnChargeClassID"
                                                                runat="server" />
                                                            <input type="hidden" value='<%#: Eval("ChargeClassCode")%>' class="hdnChargeClassCode"
                                                                runat="server" />
                                                            <input type="hidden" value='<%#: Eval("ChargeClassName")%>' class="hdnChargeClassName"
                                                                runat="server" />
                                                            <input type="hidden" value='<%#: Eval("ChargeClassName")%>' class="hdnChargeClassName"
                                                                runat="server" />
                                                            <input type="hidden" value='<%#: Eval("ChargeClassBPJSCode")%>' class="hdnChargeClassBPJSCode"
                                                                runat="server" />
                                                            <input type="hidden" value='<%#: Eval("ChargeClassBPJSType")%>' class="hdnChargeClassBPJSType"
                                                                runat="server" />
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
                                                                            <%=GetLabel(":")%>
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
                                                                            <%=GetLabel(":")%>
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
                                                                            <%=GetLabel(":")%>
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
                            <td align="left" id="footer" valign="top" style="width: auto; height: 350px;">
                                <div style="overflow-x: scroll; overflow-y: scroll; height: 350px">
                                    <dxcp:ASPxCallbackPanel ID="cbpBedPicksViewBed" runat="server" Width="100%" ClientInstanceName="cbpBedPicksViewBed"
                                        ShowLoadingPanel="false" OnCallback="cbpBedPicksViewBed_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpBedPicksViewBedEndCallback(); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent2" runat="server">
                                                <asp:Repeater ID="rptBed" runat="server">
                                                    <HeaderTemplate>
                                                        <ul class="ulBed grdSelected" id="ulBedPicksBedSelected">
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <li class='liBedStatus<%#:Eval("BedCodeSuffix")%>'>
                                                            <table class="fontCustom" style="width: 100%">
                                                                <tr>
                                                                    <td valign="top" style="width: 80px;">
                                                                        <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="100px" /><br />
                                                                        <input type="hidden" value='<%#: Eval("GCGender")%>' class="hdnPatientGender" />
                                                                        <div style="float: left; vertical-align: top; padding-top: 1px">
                                                                            <%#:Eval("cfIsInfectiousCustomText")%></div>
                                                                    </td>
                                                                    <td style="vertical-align: top">
                                                                        <table width="100%" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td style="font-weight: bold; font-size: 1em; width: 50%">
                                                                                    <div style="float: left; vertical-align: bottom; padding-top: 1px; font-size: small">
                                                                                        <%#:Eval("MedicalNo")%></div>
                                                                                </td>
                                                                                <td style="font-weight: bold; font-size: 1.3em; width: 50%">
                                                                                    <div style="float: right; vertical-align: top; padding-top: 1px">
                                                                                        <u>
                                                                                            <%#:Eval("BedCode")%></u></div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <hr style="border-color: White" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-weight: bold" colspan="2">
                                                                                    <label class="lblPatienName" title='<%#:Eval("cfPatientNameTooltips")%>' runat="server"
                                                                                        id="lblPatientName">
                                                                                        <%#:Eval("cfPatientName")%>
                                                                                    </label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <%#:Eval("RegistrationNo")%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <%#:Eval("PatientAge")%>,
                                                                                    <%#:Eval("Religion")%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <%#:Eval("ParamedicName")%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <span title="<%#:Eval("cfTextDischargeDateOrPlanDischargeDateWithNotes")%>" style='color: Red; display: <%#: Eval("CustomPlanDischargeDate") == "" ? "none" : "" %>'>
                                                                                        <%=GetLabel("Rencana Pulang : ")%>
                                                                                        <%#:Eval("CustomPlanDischargeDate")%></span>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <span style='color: Purple'>
                                                                                        <%#:Eval("cfLastUnoccupiedDate")%></span>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <input type="hidden" value='<%#: Eval("BedID")%>' class="hdnBedID" />
                                                            <input type="hidden" value='<%#: Eval("BedCode")%>' class="hdnBedCode" />
                                                            <input type="hidden" value='<%#: Eval("HealthcareServiceUnitID")%>' class="hdnHSUID" />
                                                            <input type="hidden" value='<%#: Eval("RegistrationID")%>' class="hdnRegistrationID" />
                                                            <input type="hidden" value='<%#: Eval("ServiceUnitCode")%>' class="hdnServiceUnitCode" />
                                                            <input type="hidden" value='<%#: Eval("ServiceUnitName")%>' class="hdnServiceUnitName" />
                                                            <input type="hidden" value='<%#: Eval("GCBedStatus")%>' class="hdnGCBedStatus" />
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
