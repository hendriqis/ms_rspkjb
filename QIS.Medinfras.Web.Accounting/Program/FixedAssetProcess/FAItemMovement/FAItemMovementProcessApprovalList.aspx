<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="FAItemMovementProcessApprovalList.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.FAItemMovementProcessApprovalList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnFAItemMovementHdApprove" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Approve")%></div>
    </li>
    <li id="btnFAItemMovementHdDecline" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Re-Open")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=btnFAItemMovementHdApprove.ClientID %>').click(function () {
                if ($('.chkIsSelected input:checked').length < 1) {
                    showToast('Warning', 'Please Select an Item First');
                }
                else {
                    var param = '';
                    $('.chkIsSelected input:checked').each(function () {
                        var faItemMovementHdID = $(this).closest('tr').find('.keyField').html();
                        if (param != '')
                            param += ',';
                        param += faItemMovementHdID;
                        showToast('Approve Success');
                    });
                    $('#<%=hdnParam.ClientID %>').val(param);
                    onCustomButtonClick('approve');
                }
            });

            $('#<%=btnFAItemMovementHdDecline.ClientID %>').click(function () {
                if ($('.chkIsSelected input:checked').length < 1) {
                    showToast('Warning', 'Please Select an Item First');
                }
                else {
                    var param = '';
                    $('.chkIsSelected input:checked').each(function () {
                        var faItemMovementHdID = $(this).closest('tr').find('.keyField').html();
                        if (param != '')
                            param += ',';
                        param += faItemMovementHdID;
                        showToast('Decline Success');
                    });
                    $('#<%=hdnParam.ClientID %>').val(param);
                    onCustomButtonClick('decline');
                }
            });
        });

        $('.lblMovementNo').die('click');
        $('.lblMovementNo').live('click', function () {
            var faItemMovementHdID = $(this).closest('tr').find('.keyField').html();

            var url = ResolveUrl("~/Program/FixedAssetProcess/FAItemMovement/FAItemMovementProcessApprovalDetailCtl.ascx");
            openUserControlPopup(url, faItemMovementHdID, 'Detail Mutasi Gabungan Aset dan Inventaris', 1200, 600);
        });

        function getFALocationFilterExpression() {
            return "IsDeleted = 0";
        }

        //#region FA Location From
        $('#<%=lblFALocationFrom.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('falocation', getFALocationFilterExpression(), function (value) {
                $('#<%=txtFALocationCodeFrom.ClientID %>').val(value);
                onTxtFALocationFromCodeChanged(value);
            });
        });

        $('#<%=txtFALocationCodeFrom.ClientID %>').live('change', function () {
            onTxtFALocationFromCodeChanged($(this).val());
        });

        function onTxtFALocationFromCodeChanged(value) {
            var filterExpression = getFALocationFilterExpression() + " AND FALocationCode = '" + value + "'";
            Methods.getObject('GetFALocationList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnFALocationIDFrom.ClientID %>').val(result.FALocationID);
                    $('#<%=txtFALocationNameFrom.ClientID %>').val(result.FALocationName);
                }
                else {
                    $('#<%=hdnFALocationIDFrom.ClientID %>').val('');
                    $('#<%=txtFALocationCodeFrom.ClientID %>').val('');
                    $('#<%=txtFALocationNameFrom.ClientID %>').val('');
                }
            });
            cbpView.PerformCallback('refresh');
        }
        //#endregion

        //#region Location To
        $('#<%=lblFALocationTo.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('falocation', getFALocationFilterExpression(), function (value) {
                $('#<%=txtFALocationCodeTo.ClientID %>').val(value);
                onTxtFALocationToCodeChanged(value);
            });
        });

        $('#<%=txtFALocationCodeTo.ClientID %>').live('change', function () {
            onTxtFALocationToCodeChanged($(this).val());
        });

        function onTxtFALocationToCodeChanged(value) {
            var filterExpression = getFALocationFilterExpression() + " AND FALocationCode = '" + value + "'";
            Methods.getObject('GetFALocationList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnFALocationIDTo.ClientID %>').val(result.FALocationID);
                    $('#<%=txtFALocationNameTo.ClientID %>').val(result.FALocationName);
                }
                else {
                    $('#<%=hdnFALocationIDTo.ClientID %>').val('');
                    $('#<%=txtFALocationCodeTo.ClientID %>').val('');
                    $('#<%=txtFALocationNameTo.ClientID %>').val('');
                }
            });
            cbpView.PerformCallback('refresh');
        }
        //#endregion

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
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
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="height: 550px; overflow-y: auto; overflow-x: hidden;" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 120px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <label class="lblLink" runat="server" id="lblFALocationFrom">
                                    <%=GetLabel("Dari Lokasi") %></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnFALocationIDFrom" runat="server" />
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="100px" />
                                        <col width="3px" />
                                        <col width="250px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFALocationCodeFrom" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFALocationNameFrom" Width="100%" Enabled="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal lblLink" runat="server" id="lblFALocationTo">
                                    <%=GetLabel("Kepada Lokasi") %></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnFALocationIDTo" runat="server" />
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="100px" />
                                        <col width="3px" />
                                        <col width="250px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFALocationCodeTo" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFALocationNameTo" Width="100%" Enabled="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="MovementID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
											<asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("No Mutasi")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <b>
                                                    <label class="lblLink lblMovementNo">
                                                        <%#:Eval("MovementNo")%></label></b>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="FromFALocationName" HeaderText="Dari Lokasi" HeaderStyle-Width="180px" />
                                            <asp:BoundField DataField="ToFALocationName" HeaderText="Ke Lokasi" HeaderStyle-Width="180px" />
                                            <asp:BoundField DataField="MovementType" HeaderText="Jenis Mutasi" HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="cfMovementDateInString" HeaderText="Tanggal Mutasi" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Keterangan" HeaderStyle-Width="300px" />
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
                </td>
            </tr>
        </table>
</asp:Content>
