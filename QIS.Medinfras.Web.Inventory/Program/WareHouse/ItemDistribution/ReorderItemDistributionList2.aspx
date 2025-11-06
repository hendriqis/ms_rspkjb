<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ReorderItemDistributionList2.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ReorderItemDistributionList2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnReorderItemRequestProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtItemOrderDate.ClientID %>');
            $('#<%=txtItemOrderDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=btnReorderItemRequestProcess.ClientID %>').click(function () {
                var allowed = $('#<%=hdnIsDistributionAllowedWithoutRequest.ClientID %>').val();
                if (allowed == "1") {
                    if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                        getCheckedMember();
                        if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                            showToast('Warning', 'Please Select Item First');
                        }
                        else {
                            onCustomButtonClick('approve');
                        }
                    }
                } else {
                    showToast('Failed', 'Proses distribusi hanya diperbolehkan dari permintaan barang.');
                }
            });

            //#region Location From
            function getLocationFilterExpressionFrom() {
                var filterExpression = "<%:filterExpressionLocationFrom %>";
                return filterExpression;
            }

            $('#<%=lblLocation.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('locationroleuser', getLocationFilterExpressionFrom(), function (value) {
                    $('#<%=txtLocationCode.ClientID %>').val(value);
                    onTxtLocationCodeChanged(value);
                });
            });

            $('#<%=txtLocationCode.ClientID %>').live('change', function () {
                onTxtLocationCodeChanged($(this).val());
            });

            function onTxtLocationCodeChanged(value) {
                var filterExpression = getLocationFilterExpressionFrom() + "LocationCode = '" + value + "'";
                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);
                        cbpView.PerformCallback('refresh');
                    }
                    else {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Location To

            function getLocationFilterExpressionTo() {
                var filterExpression = "<%:filterExpressionLocationTo %>";
                return filterExpression;
            }

            $('#<%=lblLocationTo.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('locationroleuser', getLocationFilterExpressionTo(), function (value) {
                    $('#<%=txtLocationCodeTo.ClientID %>').val(value);
                    onTxtLocationToCodeChanged(value);
                });
            });

            $('#<%=txtLocationCodeTo.ClientID %>').live('change', function () {
                onTxtLocationToCodeChanged($(this).val());
            });

            function onTxtLocationToCodeChanged(value) {
                var filterExpression = getLocationFilterExpressionTo() + "LocationCode = '" + value + "'";
                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnLocationIDTo.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationNameTo.ClientID %>').val(result.LocationName);
                        cbpView.PerformCallback('refresh');
                    }
                    else {
                        $('#<%=hdnLocationIDTo.ClientID %>').val('');
                        $('#<%=txtLocationCodeTo.ClientID %>').val('');
                        $('#<%=txtLocationNameTo.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                getCheckedMember();
                cbpView.PerformCallback('changepage|' + page);
            });
        }

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Save Success', 'Permintaan Barang Berhasil Dibuat Dengan No Permintaan <b>' + retval + '</b>', function () {
                $('#<%=hdnItemDistribution.ClientID %>').val('');
                $('#<%=hdnSelectedMember.ClientID %>').val('');
                cbpView.PerformCallback('refresh');
            });
        }

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var lstItemRequest = $('#<%=hdnItemDistribution.ClientID %>').val().split(',');
            var result = '';
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var itemRequest = $(this).closest('tr').find('.txtItemRequest').val();
                    if (lstSelectedMember.indexOf(key) < 0) {
                        lstSelectedMember.push(key);
                        lstItemRequest.push(itemRequest);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var itemRequest = $(this).closest('tr').find('.txtItemRequest').val();
                    if (lstSelectedMember.indexOf(key) > -1) {
                        lstSelectedMember.splice(lstSelectedMember.indexOf(key), 1);
                        lstItemRequest.splice(lstItemRequest.indexOf(itemRequest), 1);
                    }
                }
            });
            $('#<%=hdnItemDistribution.ClientID %>').val(lstItemRequest.join(','));
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        $('.chkIsSelected input').live('change', function () {
            $tr = $(this).closest('tr');
            if ($(this).is(':checked'))
                $tr.find('.txtItemRequest').removeAttr('readonly');
            else
                $tr.find('.txtItemRequest').attr('readonly', 'readonly');
        });

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    getCheckedMember();
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
        $('#<%=rblROPDynamic.ClientID %>').live('change', function () {
            //alert("test");
            cbpView.PerformCallback('refresh');
        });
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" value="" id="hdnIsDistributionQty" runat="server" />
    <input type="hidden" value="" id="hdnItemDistribution" runat="server" />
    <input type="hidden" value="" id="hdnIsDistributionAllowedWithoutRequest" runat="server" />
    <div style="height: 550px; overflow-y: auto; overflow-x: hidden;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocationTo">
                                    <%=GetLabel("Kepada Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDTo" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCodeTo" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationNameTo" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                -
                                <%=GetLabel("Waktu") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="padding-right: 1px;" width="160px">
                                            <asp:TextBox ID="txtItemOrderDate" Width="110px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemOrderTime" Width="100px" CssClass="time" runat="server" Style="text-align: center"
                                                Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("R.O.P")%>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblROPDynamic" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Statis" Value="false" Selected="True" />
                                    <asp:ListItem Text="Dinamis" Value="true" />
                                </asp:RadioButtonList>
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
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-Width="350px" />
                                            <asp:TemplateField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                                                <HeaderTemplate>
                                                    <div>
                                                        Stok (Dari Lokasi)</div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div id="divFromLocationQty" runat="server">
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CustomMinimum" HeaderText="Minimum" HeaderStyle-Width="120px"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomMaximum" HeaderText="Maximum" HeaderStyle-Width="120px"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomEndingBalance" HeaderText="Stok Saat Ini" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomAverageQty" HeaderText="Pemakaian Rata-Rata" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:TemplateField ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" HeaderText="Didistribusi">
                                                <ItemTemplate>
                                                    <input type="text" id="txtItemRequest" runat="server" validationgroup="mpEntry" class="txtItemRequest number max"
                                                        value="0" readonly="readonly" style="width: 50%" />
                                                    &nbsp;
                                                    <%#: Eval("ItemUnit")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CustomQtyOnOrderItemDistribution" HeaderText="Quantity On Distribution"
                                                HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right" />
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
    </div>
</asp:Content>
