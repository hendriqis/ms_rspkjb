<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="SettingParameterList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.SettingParameterList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
            cboModuleName.SetValue(null);
        });

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        $('#<%=grdView.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
            $tr = $(this).parent();
            $trDetail = $(this).parent().next();
            $imgChild = $(this).find('.imgExpand');
            if ($imgChild.length > 0) {
                if ($trDetail.attr('class') != 'trDetail') {
                    $('#containerDetail').show();
                    $trCollapse = $('.trDetail');
                    $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                    $newTr = $("<tr><td></td><td colspan='4'></td></tr>").attr('class', 'trDetail');
                    $newTr.insertAfter($tr);
                    $newTr.find('td').last().append($('#containerGrdDetail'));
                    if ($trCollapse != null) {
                        $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                        $trCollapse.remove();
                    }
                    $('#<%=hdnExpandID.ClientID %>').val($tr.find('.keyField').html());
                    cbpViewDetail.PerformCallback('refresh');
                }
                else {
                    $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $('#tempContainerGrdDetail').append($('#containerGrdDetail'));
                    $trDetail.remove();
                }
            }
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });
        function onCbpViewBeginCallback() {
            showLoadingPanel();
            $trCollapse = $('.trDetail');
            if ($trCollapse != null) {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $('#tempContainerGrdDetail').append($('#containerGrdDetail'));
                $trCollapse.remove();
            }
        }

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

        function onChangeModuleName() {
            cbpView.PerformCallback('refresh');
        }

        function onGetEntryPopupReturnValue() {
            return '';
        }

        function onAfterSaveRightPanelContent(code, value) {
            if ($('#containerDetail').is(":visible"))
                cbpViewDetail.PerformCallback();
        }

        $('.lnkEditSettingParameterDt').live('click', function () {
            var healthcareID = $(this).closest('tr').find('.keyField').html() + '|' + $('#<%=hdnExpandID.ClientID %>').val();
            var url = ResolveUrl("~/Program/ControlPanel/SettingVariables/SettingParameter/SettingParameterDtCtl.ascx");
            openUserControlPopup(url, healthcareID, 'Setting Parameter', 600, 500);
        });
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnExpandID" runat="server" value="" />
    <div style="position: relative;">
        <table width="50%">
            <colgroup>
                <col width="30%" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Module")%></label>
                </td>
                <td>
                    <dx:ASPxComboBox ID="cboModuleName" runat="server" Width="300px" ClientInstanceName="cboModuleName">
                        <ClientSideEvents SelectedIndexChanged="onChangeModuleName" />
                    </dx:ASPxComboBox>
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ onCbpViewBeginCallback(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ParameterCode" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpand" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <img class='<%# ((bool)Eval("IsByHealthcare") == true ? "imgExpand imgLink" : "imgHidden") %>'
                                            src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ParameterCode" HeaderText="Parameter Code" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                                <asp:BoundField DataField="ParameterName" HeaderText="Parameter Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="450px" />
                                <asp:BoundField DataField="cfParameterValueCustomUI" HeaderText="Parameter Value" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="250px" />
                                <asp:BoundField DataField="Notes" HeaderText="Notes" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="IsUsedBySystem" HeaderText="Is Used By System" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="100px" />
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
    <div id="tempContainerGrdDetail" style="display: none">
        <div id="containerGrdDetail" class="borderBox" style="width: 100%; padding: 10px 5px;">
            <div id="containerDetail" class="containerGrdDt">
                <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" Width="100%" ClientInstanceName="cbpViewDetail"
                    ShowLoadingPanel="false" OnCallback="cbpViewDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail').show(); }"
                        EndCallback="function(s,e){ $('#containerImgLoadingViewDetail').hide(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel2" Style="width: 100%; margin-left: auto; margin-right: auto">
                                <asp:GridView ID="grdDetail" runat="server" CssClass="grdView notAllowSelect grdDetail"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="HealthcareID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                            <HeaderTemplate>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <img class="lnkEditSettingParameterDt imgLink" title="<%=GetLabel("Edit") %>" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="HealthcareName" HeaderText="Healthcare Name" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="cfParameterValueCustomUI" HeaderText="Parameter Value" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="500px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
