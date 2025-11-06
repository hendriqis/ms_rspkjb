<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="ViewAPIMessageLogList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ViewAPIMessageLogList" %>
    
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
//////            $('.lnkViewJson a').live('click', function () {
//////                var id = $(this).closest('tr').find('.keyField').html();
//////                var url = ResolveUrl("~/Program/Tools/APIMessageLog/APIMessageLogJsonViewerCtl.ascx");
//////                openUserControlPopup(url, id, 'View JSON', 1000, 500);
//////            });

            setDatePicker('<%=txtLogDate.ClientID %>');
            $('#<%=txtLogDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtLogDate.ClientID %>').change(function () {
                $('#divErrorDetail').html('');
                cbpView.PerformCallback('refresh');
            });

            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function onCboSenderValueChanged(evt) {
            $('#divErrorDetail').html('');
            cbpView.PerformCallback('refresh');
        }

        function onCboRecipientValueChanged(evt) {
            $('#divErrorDetail').html('');
            cbpView.PerformCallback('refresh');
        }

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

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
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
                        <%=GetLabel("Message Date")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtLogDate" Width="120px" runat="server" CssClass="datepicker" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Sender")%></label></td>
                <td>
                    <dxe:ASPxComboBox ID="cboSender" ClientInstanceName="cboSender" Width="250px" runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboSenderValueChanged(e); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Recipient")%></label></td>
                <td>
                    <dxe:ASPxComboBox ID="cboRecipient" ClientInstanceName="cboRecipient" Width="250px" runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboRecipientValueChanged(e); }" />
                    </dxe:ASPxComboBox>
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
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="cfMessageDateTimeInFullString" HeaderText="Message Date Time" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="120px" />
                                <%--<asp:HyperLinkField HeaderText="View JSON" Text="View JSON" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkViewJson" HeaderStyle-Width="100px" />--%>
                                <asp:BoundField DataField="Sender" HeaderText="Sender" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Recipient" HeaderText="Recipient" HeaderStyle-Width="80px"
                                    ItemStyle-HorizontalAlign="Center" />
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
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
