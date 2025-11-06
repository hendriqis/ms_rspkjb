<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="RoomInformation.aspx.cs" Inherits="QIS.Medinfras.Web.Emergency.Program.RoomInformation" %>

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
                                                                                            <%#:Eval("PatientName")%>
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
    </div>
</asp:Content>
