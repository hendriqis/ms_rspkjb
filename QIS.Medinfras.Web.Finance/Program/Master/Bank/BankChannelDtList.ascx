<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BankChannelDtList.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.BankChannelDtList" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_bankchanneldtctl">
    $('#lblAdd').die('click');
    $('#lblAdd').live('click', function () {
        cboProviderMethod.SetEnabled(true);
        $('#<%=txtServiceCode.ClientID %>').val('');
        $('#<%=txtChannelID.ClientID %>').val('');
        $('#<%=txtSecretKey.ClientID %>').val('');
        $('#<%=txtCompanyCode.ClientID %>').val('');
        $('#<%=txtBIN.ClientID %>').val('');
        $('#<%=hdnID.ClientID %>').val('');
        cboProviderMethod.SetText('');
        $('#containerBankChannelDt').show();
    });

    $('#btnCancel').die('click');
    $('#btnCancel').live('click', function () {
        $('#containerBankChannelDt').hide();
    });

    $('#btnSave').click(function (evt) {
        if (IsValid(evt, 'fsBankChannelDt', 'mpBankChannelDt'))
            cbpBankChannelDt.PerformCallback('save');
        return false;
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#containerBankChannelDt').show();
        $row = $(this).parent().parent();
        var id = $row.find('.hdnID').val();
        var provider = $row.find('.hdnGCProviderMethod').val();
        var serviceCode = $row.find('.hdnServiceCode').val();
        var channelID = $row.find('.hdnChannelID').val();
        var secretKey = $row.find('.hdnSecretKey').val();
        var companyCode = $row.find('.hdnCompanyCode').val();
        var bin = $row.find('.hdnBIN').val();

        $('#<%=hdnID.ClientID %>').val(id);
        cboProviderMethod.SetValue(provider);
        cboProviderMethod.SetEnabled(false);
        $('#<%=txtServiceCode.ClientID %>').val(serviceCode);
        $('#<%=txtChannelID.ClientID %>').val(channelID);
        $('#<%=txtSecretKey.ClientID %>').val(secretKey);
        $('#<%=txtCompanyCode.ClientID %>').val(companyCode);
        $('#<%=txtBIN.ClientID %>').val(bin);

    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).parent().parent();
            var id = $row.find('.hdnID').val();
            $('#<%=hdnID.ClientID %>').val(id);
            cbpBankChannelDt.PerformCallback('delete');
        }
    });

    function onCbpBankChannelDtEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerBankChannelDt').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#<%=hdnVisitID.ClientID %>').val('');
            }
        }
        hideLoadingPanel();
    }
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnMRN" />
    <input type="hidden" value="" runat="server" id="hdnRegistrationID" />
    <input type="hidden" value="" runat="server" id="hdnClassID" />
    <input type="hidden" value="" runat="server" id="hdnBusinessPartnerID" />
    <input type="hidden" value="" runat="server" id="hdnItemCardFee" />
    <input type="hidden" value="" runat="server" id="hdnScheduleValidationBeforeRegistration" />
    <input type="hidden" id="hdnIsBridgingToGateway" value="0" runat="server" />
    <input type="hidden" id="hdnProviderGatewayService" value="0" runat="server" />
    <input type="hidden" id="hdnMenggunakanValidasiChiefComplaint" runat="server" value="" />
    <input type="hidden" id="hdnIsParamedicInRegistrationUseScheduleCtl1" runat="server" value="" />
    <input type="hidden" id="hdnBankID" runat="server" value="" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 50px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Bank")%></label>
                        </td>
                        <td >
                            <asp:TextBox ID="txtBankName" Width="200px" runat="server" ReadOnly=true />
                        </td>
                    </tr>
                </table>
                <div id="containerBankChannelDt" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry Payment Gateway Channel Configuration")%></div>
                    <input type="hidden" id="hdnVisitID" runat="server" value="" />
                    <fieldset id="fsBankChannelDt" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Provider")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboProviderMethod" ClientInstanceName="cboProviderMethod" Width="50%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Service Code")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtServiceCode" Width="50%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Channel ID")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChannelID" Width="50%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Secret Key")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSecretKey" Width="50%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Company Code")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCompanyCode" Width="50%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("BIN")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBIN" Width="50%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpBankChannelDt" runat="server" Width="100%" ClientInstanceName="cbpBankChannelDt"
                    ShowLoadingPanel="false" OnCallback="cbpBankChannelDt_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpBankChannelDtEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink"
                                                    title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" />
                                                <img class='imgDelete imgLink'
                                                    title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="200px" ItemStyle-VerticalAlign="Top">
                                            <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("Provider")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div class="divVisitNo">
                                                    <%#: Eval("ProviderMethod") %></div>
                                                <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="hdnBankID" value="<%#: Eval("BankID")%>" />
                                                <input type="hidden" class="hdnGCProviderMethod" value="<%#: Eval("GCProviderMethod")%>" />
                                                <input type="hidden" class="hdnServiceCode" value="<%#: Eval("ServiceCode")%>" />
                                                <input type="hidden" class="hdnChannelID" value="<%#: Eval("ChannelID")%>" />
                                                <input type="hidden" class="hdnSecretKey" value="<%#: Eval("SecretKey")%>" />
                                                <input type="hidden" class="hdnCompanyCode" value="<%#: Eval("CompanyCode")%>" />
                                                <input type="hidden" class="hdnBIN" value="<%#: Eval("BIN")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-VerticalAlign="Top">
                                            <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("Service Code")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="text-align: left;">
                                                    <%#: Eval("ServiceCode") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-VerticalAlign="Top">
                                            <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("Channel ID")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="text-align: left;">
                                                    <%#: Eval("ChannelID") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-VerticalAlign="Top">
                                            <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("Secret Key")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="text-align: left;">
                                                    <%#: Eval("SecretKey") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-VerticalAlign="Top">
                                            <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("Company Code")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="text-align: left;">
                                                    <%#: Eval("CompanyCode") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-VerticalAlign="Top">
                                            <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("BIN")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="text-align: left;">
                                                    <%#: Eval("BIN") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblAdd">
                        <%= GetLabel("Add Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
