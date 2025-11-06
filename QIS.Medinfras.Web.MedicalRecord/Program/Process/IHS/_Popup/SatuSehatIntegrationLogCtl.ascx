<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SatuSehatIntegrationLogCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.MedicalRecord.Program.SatuSehatIntegrationLogCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<input type="hidden" value="" id="hdnID" runat="server" />
<div style="padding: 5px 0;">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
            var detail = $(this).find('.keyField').html();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#divErrorDetail').html('');
            $('#divErrorDetail').append(convert(detail));
        });

        var convert = function (convert) {
            return $('<span />', { html: convert }).text();
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="Hidden1" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <table class="tblEntryContent" style="width: 60%;">
            <colgroup>
                <col style="width: 120px" />
                <col style="width: 250px" />
                <col style="width: 500px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("No. Registrasi")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtRegistrationNo" Width="200px" ReadOnly=true runat="server"/>
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="SendDateTimeInString" HeaderText="Send Date Time" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="120px" />
                                <asp:BoundField DataField="ResponseDateTimeInString" HeaderText="Response Date Time" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="120px" />
                                <%--<asp:HyperLinkField HeaderText="View JSON" Text="View JSON" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkViewJson" HeaderStyle-Width="100px" />--%>
                                <%--<asp:BoundField DataField="Sender" HeaderText="Sender" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Recipient" HeaderText="Recipient" HeaderStyle-Width="80px"
                                    ItemStyle-HorizontalAlign="Center" />--%>
                                <asp:TemplateField HeaderText="Message Text" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div style="height: 150px; overflow-y: hidden; width: 100%; overflow-x: hidden;">
                                            <asp:TextBox ID="txtMessageText" Width="100%" runat="server" TextMode="MultiLine"
                                                Height="100%" ReadOnly="true" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Response" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div style="height: 150px; overflow-y: hidden; width: 100%; overflow-x: hidden;">
                                            <asp:TextBox ID="txtRespose" Width="100%" runat="server" TextMode="MultiLine" Height="100%"
                                                ReadOnly="true" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CheckBoxField DataField="IsSuccess" HeaderText="Is Success" HeaderStyle-Width="50px"
                                    ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <%--<div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>--%>
    </div>
</asp:Content>
