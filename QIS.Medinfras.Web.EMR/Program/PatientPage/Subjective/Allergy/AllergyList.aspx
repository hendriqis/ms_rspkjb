<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true" 
    CodeBehind="AllergyList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.AllergyList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }   

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

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

        function onSelectedRowChanged(value) {
            var idx = $('#<%=grdView.ClientID %> tr').index($('#<%=grdView.ClientID %> tr.focus'));
            idx += value;
            if (idx < 1)
                idx = 1;
            if (idx == $('#<%=grdView.ClientID %> tr').length)
                idx--;
            $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();
        }

        function getSelectedRow() {
            return $('#<%=grdView.ClientID %> tr.focus');
        }

        function onButtonCancelClick() {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        }

        $(function () {
            $('#<%=ddlAllergenTypeFilter.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });
        });
    </script>

    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <table>
            <tr>
                <td><%=GetLabel("Allergen Type") %></td>
                <td style="padding-left:20px">
                    <asp:DropDownList ID="ddlAllergenTypeFilter" runat="server" Width="200px">
                        <asp:ListItem Text="All" Value="0" Selected="True" />
                        <asp:ListItem Text="Drug" Value="1" />
                        <asp:ListItem Text="Environmental" Value="2" />
                        <asp:ListItem Text="Pollen" Value="3" />
                        <asp:ListItem Text="Food" Value="4" />
                        <asp:ListItem Text="Animal" Value="5" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("LogDateInString") %>" bindingfield="LogDateInString" />
                                        <input type="hidden" value="<%#:Eval("Allergen") %>" bindingfield="Allergen" />
                                        <input type="hidden" value="<%#:Eval("GCAllergenType") %>" bindingfield="GCAllergenType" />
                                        <input type="hidden" value="<%#:Eval("GCAllergySource") %>" bindingfield="GCAllergySource" />
                                        <input type="hidden" value="<%#:Eval("GCAllergySeverity") %>" bindingfield="GCAllergySeverity" />
                                        <input type="hidden" value="<%#:Eval("DisplayKnownDate") %>" bindingfield="DisplayKnownDate" />
                                        <input type="hidden" value="<%#:Eval("Reaction") %>" bindingfield="Reaction" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="LogDateInString" HeaderText="Log Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Allergen" HeaderText="Allergen Name" HeaderStyle-Width="230px" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AllergySource" HeaderText="Finding Source" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="DisplayKnownDate" HeaderText="Since" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AllergySeverity" HeaderText="Severity" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Reaction" HeaderText="Reaction" HeaderStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Informasi Dibuat")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div><%#: Eval("BindDate")%></div>
                                        <div><%#: Eval("BindUser")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No patient allergy record available")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging"></div>
            </div>
        </div> 
    </div>
</asp:Content>
