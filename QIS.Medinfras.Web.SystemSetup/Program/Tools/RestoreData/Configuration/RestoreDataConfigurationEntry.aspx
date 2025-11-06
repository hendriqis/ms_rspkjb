<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="RestoreDataConfigurationEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Tools.RestoreDataConfigurationEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function getSelectedGridColumn() {
            var result = '';
            $('.chkColumn:checked').each(function () {
                if (result != '')
                    result += '|';
                result += $(this).closest('tr').find('td').last().html();
            });
            $('#<%=hdnGridColumn.ClientID %>').val(result);
        }

        function resetSelectedGridColumn() {
            $('#<%=hdnGridColumn.ClientID %>').val('');
        }

        $(function () {
            onCbpColumnListEndCallback();
        });

        function onCbpColumnListEndCallback() {
            hideLoadingPanel();
            $('.chkColumn').click(function () {
                getSelectedGridColumn();
            });
        }

        function setTxtAliasText(text) {
            $('#<%=txtAlias.ClientID %>').val(text);
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnGridColumn" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Restore Data")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:35%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Table")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboTableName" Width="100%" runat="server">
                                <ClientSideEvents 
                                    ValueChanged="function(s,e){ resetSelectedGridColumn(); cbpColumnList.PerformCallback(); setTxtAliasText(s.GetText()); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Alias")%></label></td>
                        <td><asp:TextBox ID="txtAlias" Width="150px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Filter Expression")%></label></td>
                        <td><asp:TextBox ID="txtFilterExpression" Width="250px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <h4>Display Column : </h4>
                <dxcp:ASPxCallbackPanel ID="cbpColumnList" runat="server" Width="100%" ClientInstanceName="cbpColumnList"
                    ShowLoadingPanel="false" OnCallback="cbpColumnList_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpColumnListEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Repeater ID="rptColumnList" runat="server" OnItemDataBound="rptColumnList_ItemDataBound">
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30px"/>
                                            <col style="width:5px"/>
                                            <col/>
                                        </colgroup>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><input type="checkbox" id="chkColumn" runat="server" class="chkColumn" /></td>
                                        <td>&nbsp;</td>
                                        <td><%#:Eval("Name") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>    
            </td>

        </tr>
    </table>
</asp:Content>
