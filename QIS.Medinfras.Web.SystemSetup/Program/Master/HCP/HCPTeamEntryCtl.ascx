<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HCPTeamEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.HCPTeamEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_paramedicteamentryctl">
    setDatePicker('<%=txtStartDate.ClientID %>');

    $('#lblEntryPopupAddDataParamedicTeam').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnParamedicTeamID.ClientID %>').val('');
        $('#<%=txtParamedicTeamCode.ClientID %>').val('');
        $('#<%=txtParamedicTeamName.ClientID %>').val('');
        $('#<%=hdnRevenueSharingID.ClientID %>').val('');
        $('#<%=txtRevenueSharingCode.ClientID %>').val('');
        $('#<%=txtRevenueSharingName.ClientID %>').val('');
        cboParamedicRole.SetValue('');

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var ID = $row.find('.ID').val();
            $('#<%=hdnID.ClientID %>').val(ID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.ID').val();
        var ParamedicID = $row.find('.ParamedicID').val();
        var ParamedicCode = $row.find('.ParamedicCode').val();
        var ParamedicName = $row.find('.ParamedicName').val();
        var ParamedicParentID = $row.find('.ParamedicParentID').val();
        var RevenueSharingID = $row.find('.RevenueSharingID').val();
        var RevenueSharingCode = $row.find('.RevenueSharingCode').val();
        var RevenueSharingName = $row.find('.RevenueSharingName').val();
        var GCParamedicRole = $row.find('.GCParamedicRole').val();
        var cfStartDateInDatePickerString = $row.find('.cfStartDateInDatePickerString').val();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnParamedicTeamID.ClientID %>').val(ParamedicID);
        $('#<%=txtParamedicTeamCode.ClientID %>').val(ParamedicCode);
        $('#<%=txtParamedicTeamName.ClientID %>').val(ParamedicName);
        $('#<%=hdnRevenueSharingID.ClientID %>').val(RevenueSharingID);
        $('#<%=txtRevenueSharingCode.ClientID %>').val(RevenueSharingCode);
        $('#<%=txtRevenueSharingName.ClientID %>').val(RevenueSharingName);
        cboParamedicRole.SetValue(GCParamedicRole);
        $('#<%=txtStartDate.ClientID %>').val(cfStartDateInDatePickerString);
        
        $('#containerPopupEntryData').show();
    });

    //#region Paramedic
    $('#lblParamedicDetail.lblLink').click(function () {
        openSearchDialog('paramedic', "IsDeleted = 0", function (value) {
            $('#<%=txtParamedicTeamCode.ClientID %>').val(value);
            onTxtParamedicTeamCodeChanged(value);
        });
    });

    $('#<%=txtParamedicTeamCode.ClientID %>').change(function () {
        onTxtParamedicTeamCodeChanged($(this).val());
    });

    function onTxtParamedicTeamCodeChanged(value) {
        var filterExpression = "ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnParamedicTeamID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtParamedicTeamName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%=hdnParamedicTeamID.ClientID %>').val('');
                $('#<%=txtParamedicTeamCode.ClientID %>').val('');
                $('#<%=txtParamedicTeamName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Revenue Sharing
    $('#lblRevenueSharing.lblLink').click(function () {
        openSearchDialog('revenuesharing', "IsDeleted = 0", function (value) {
            $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
            onTxtRevenueSharingCodeChanged(value);
        });
    });

    $('#<%=txtRevenueSharingCode.ClientID %>').change(function () {
        onTxtRevenueSharingCodeChanged($(this).val());
    });

    function onTxtRevenueSharingCodeChanged(value) {
        var filterExpression = "RevenueSharingCode = '" + value + "'";
        Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                $('#<%=txtRevenueSharingName.ClientID %>').val(result.RevenueSharingName);
            }
            else {
                $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                $('#<%=txtRevenueSharingName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingViewPopup').hide();
    }
</script>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" id="hdnParamedicParentID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Dokter / Paramedis")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtParamedicParentName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblLink lblMandatory" id="lblParamedicDetail"><%=GetLabel("Paramedic Team")%></label></td> 
                                <td>
                                    <input type="hidden" value="" id="hdnParamedicTeamID" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:120px"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtParamedicTeamCode" CssClass="required" Width="100%" runat="server" /></td> 
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtParamedicTeamName" ReadOnly="true" CssClass="required" Width="80%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink lblMandatory" id="lblRevenueSharing"><%=GetLabel("Revenue Sharing")%></label></td> 
                                <td>
                                    <input type="hidden" value="" id="hdnRevenueSharingID" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:120px"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtRevenueSharingCode" CssClass="required" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtRevenueSharingName" ReadOnly="true" CssClass="required" Width="80%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Paramedic Role")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboParamedicRole" ClientInstanceName="cboParamedicRole" Width="120px" runat="server">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Start Date")%></label></td>
                                <td><asp:TextBox ID="txtStartDate" Width="120px" runat="server" CssClass="datepicker" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Tutup")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="ID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="ParamedicID" value="<%#: Eval("ParamedicID")%>" />
                                                <input type="hidden" class="ParamedicCode" value="<%#: Eval("ParamedicCode")%>" />
                                                <input type="hidden" class="ParamedicName" value="<%#: Eval("ParamedicName")%>" />
                                                <input type="hidden" class="GCParamedicRole" value="<%#: Eval("GCParamedicRole")%>" />
                                                <input type="hidden" class="ParamedicParentID" value="<%#: Eval("ParamedicParentID")%>" />
                                                <input type="hidden" class="RevenueSharingID" value="<%#: Eval("RevenueSharingID")%>" />
                                                <input type="hidden" class="RevenueSharingCode" value="<%#: Eval("RevenueSharingCode")%>" />
                                                <input type="hidden" class="RevenueSharingName" value="<%#: Eval("RevenueSharingName")%>" />
                                                <input type="hidden" class="cfStartDateInDatePickerString" value="<%#: Eval("cfStartDateInDatePickerString")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ParamedicName" HeaderText="Paramedic Name" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="RevenueSharingName" HeaderText="Revenue Sharing" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="ParamedicRole" HeaderText="Paramedic Role" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="cfStartDateInString" HeaderText="Start Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="CreatedByName" HeaderText="Create By" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddDataParamedicTeam">
                        <%= GetLabel("Add Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
