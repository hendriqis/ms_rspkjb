<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="RecalculateMinMax.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.RecalculateMinMax" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
        <li id="btnRecalculateMinMax" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Process")%></div></li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=btnRecalculateMinMax.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    getCheckedMember();
                    if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                        showToast('Warning', 'Belum ada item persediaan yang dipilih untuk diproses !');
                    }
                    else {
                        var message = "Lanjutkan proses rekalkulasi tingkat minimum dan maximum persediaan di lokasi ini ?";
                        showToastConfirmation(message, function (result) {
                            if (result) onCustomButtonClick('recalculate');
                        });
                        
                    }
                }
            });

            //#region Location From
            function getLocationFilterExpression() {
                var filterExpression = "<%:filterExpressionLocation %>";
                return filterExpression;
            }

            $('#<%=lblLocation.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('locationroleuser', getLocationFilterExpression(), function (value) {
                    $('#<%=txtLocationCode.ClientID %>').val(value);
                    onTxtLocationCodeChanged(value);
                });
            });

            $('#<%=txtLocationCode.ClientID %>').live('change', function () {
                onTxtLocationCodeChanged($(this).val());
            });

            function onTxtLocationCodeChanged(value) {
                var filterExpression = getLocationFilterExpression() + " AND LocationCode = '" + value + "'";
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

        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        $('#<%=rblItemType.ClientID %>').live('change', function () {
            onRefreshGrid();
        });

        $('.chkIsSelected input').live('change', function () {
            $tr = $(this).closest('tr');
            if ($(this).is(':checked')) {
                $tr.find('.txtMinStock').removeAttr('readonly');
                $tr.find('.txtMaxStock').removeAttr('readonly');
            }
            else {
                $tr.find('.txtMinStock').attr('readonly', 'readonly');
                $tr.find('.txtMaxStock').attr('readonly', 'readonly');
            }
        });

        $('#<%=rblDisplay.ClientID %>').live('change', function () {
            onRefreshGrid();
        });

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Save Success', 'Rekalkulasi Min Max Stock Berhasil dilakukan', function () {
                $('#<%=hdnMinStock.ClientID %>').val('');
                $('#<%=hdnMaxStock.ClientID %>').val('');
                $('#<%=hdnSelectedMember.ClientID %>').val('');
                cbpView.PerformCallback('refresh');
            });
        }

        function onRefreshGrid() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            getCheckedMember();
            cbpView.PerformCallback('refresh');
        }

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split('|');
            var lstAverageQty = $('#<%=hdnAverageQty.ClientID %>').val().split('|');
            var lstSystemMinStock = $('#<%=hdnSystemMinStock.ClientID %>').val().split('|');
            var lstSystemMaxStock = $('#<%=hdnSystemMaxStock.ClientID %>').val().split('|');
            var lstMinStock = $('#<%=hdnMinStock.ClientID %>').val().split('|');
            var lstMaxStock = $('#<%=hdnMaxStock.ClientID %>').val().split('|');
            var result = '';
            $('.grdItemBalance .chkIsSelected input').each(function () {
                var key = $(this).closest('tr').find('.keyField').html();
                var averageQty = $(this).closest('tr').find('.txtAverageQty').val();
                var systemMin = $(this).closest('tr').find('.txtSystemMinStock').val();
                var systemMax = $(this).closest('tr').find('.txtSystemMaxStock').val();
                var min = $(this).closest('tr').find('.txtMinStock').val();
                var max = $(this).closest('tr').find('.txtMaxStock').val();
                var idx = lstSelectedMember.indexOf(key); 
                if ($(this).is(':checked')) {
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstAverageQty.push(averageQty);
                        lstSystemMinStock.push(systemMin);
                        lstSystemMaxStock.push(systemMax);
                        lstMinStock.push(min);
                        lstMaxStock.push(max);
                    }
                    else {
                        lstAverageQty[idx] = averageQty;
                        lstSystemMinStock[idx] = systemMin;
                        lstSystemMaxStock[idx] = systemMax;
                        lstMinStock[idx] = min;
                        lstMaxStock[idx] = max;
                    }
                }
                else {
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstAverageQty.splice(idx, 1);
                        lstSystemMinStock.splice(idx, 1);
                        lstSystemMaxStock.splice(idx, 1);
                        lstMinStock.splice(idx, 1);
                        lstMaxStock.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnAverageQty.ClientID %>').val(lstAverageQty.join('|'));
            $('#<%=hdnSystemMinStock.ClientID %>').val(lstSystemMinStock.join('|'));
            $('#<%=hdnSystemMaxStock.ClientID %>').val(lstSystemMaxStock.join('|'));
            $('#<%=hdnMinStock.ClientID %>').val(lstMinStock.join('|'));
            $('#<%=hdnMaxStock.ClientID %>').val(lstMaxStock.join('|'));
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join('|'));
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            $('.grdItemBalance tr:eq(1)').click();
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" value="" runat="server" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnAverageQty" runat="server" value="" />
    <input type="hidden" id="hdnSystemMinStock" runat="server" value="" />
    <input type="hidden" id="hdnSystemMaxStock" runat="server" value="" />
    <input type="hidden" id="hdnMinStock" runat="server" value="" />
    <input type="hidden" id="hdnMaxStock" runat="server" value="" />
    <input type="hidden" value="0" id="hdnFactorXMin" runat="server" />
    <input type="hidden" value="0" id="hdnFactorXMax" runat="server" />
    <input type="hidden" value="0" id="hdnPercentageForMaximumStock" runat="server" />
    <input type="hidden" value="0" id="hdnIM0134" runat="server" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                <%=GetLabel("Lokasi")%></label>
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
                            <%=GetLabel("Jenis Item")%>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Semua" Value="0" />
                                <asp:ListItem Text="Obat" Value="2" Selected="True" />
                                <asp:ListItem Text="Alkes" Value="3" />
                                <asp:ListItem Text="Barang Umum" Value="8" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Tampilkan")%>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblDisplay" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Semua" Value="0" Selected="True"/>
                                <asp:ListItem Text="Average > 0" Value="1" />
                                <asp:ListItem Text="Average = 0" Value="2" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td>
                            <asp:CheckBox ID="chkIsConsignmentItem" runat="server" Checked="false" /><%:GetLabel("Termasuk barang konsinyasi")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Quick Search")%></label>
                        </td>
                        <td>
                            <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                Width="400px" Watermark="Search">
                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                <IntellisenseHints>
                                    <qis:QISIntellisenseHint Text="ItemName1" FieldName="ItemName1" />
                                    <qis:QISIntellisenseHint Text="ItemCode" FieldName="ItemCode" />
                                </IntellisenseHints>
                            </qis:QISIntellisenseTextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="height: 650px; overflow-y: auto; overflow-x: hidden;">
        <table class="tblContentArea">
            <tr>
                <td>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdItemBalance grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">&nbsp;</th>
                                                    <th rowspan="2" style="width: 20px">&nbsp;</th>
                                                    <th rowspan="2" align="left"><%=GetLabel("Item Name")%></th>
                                                    <th colspan="2" align="center"><%=GetLabel("ON HAND")%></th>
                                                    <th rowspan="2" align="center" style="width: 100px"><%=GetLabel("AVERAGE")%></th>
                                                    <th colspan="2" align="center"><%=GetLabel("CURRENT")%></th>
                                                    <th colspan="2" align="center"><%=GetLabel("SYSTEM RESULT")%></th>
                                                    <th colspan="2" align="center"><%=GetLabel("NEW")%></th>
                                                    <th rowspan="2" align="center" style="width: 100px"><%=GetLabel("Unit")%></th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 60px" align="center"><%=GetLabel("ALL")%></th>
                                                    <th style="width: 60px" align="center"><%=GetLabel("Location")%></th>
                                                    <th style="width: 60px" align="center"><%=GetLabel("MIN")%></th>
                                                    <th style="width: 60px" align="center"><%=GetLabel("MAX")%></th>
                                                    <th style="width: 60px" align="right"><%=GetLabel("MIN")%></th>
                                                    <th style="width: 60px" align="right"><%=GetLabel("MAX")%></th>
                                                    <th style="width: 60px" align="right"><%=GetLabel("MIN")%></th>
                                                    <th style="width: 60px" align="right"><%=GetLabel("MAX")%></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="12">
                                                        <%=GetLabel("Silahkan Pilih Lokasi yang ingin di Rekalkulasi")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdItemBalance grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">&nbsp;</th>
                                                    <th rowspan="2" style="width: 20px" align="center"><input id="chkSelectAll" type="checkbox" /></th>
                                                    <th rowspan="2" align="left"><%=GetLabel("Item Name")%></th>
                                                    <th colspan="2" align="center"><%=GetLabel("ON HAND")%></th>
                                                    <th rowspan="2" align="center" style="width: 100px"><%=GetLabel("AVERAGE")%></th>
                                                    <th colspan="2" align="center"><%=GetLabel("CURRENT")%></th>
                                                    <th colspan="2" align="center"><%=GetLabel("SYSTEM RESULT")%></th>
                                                    <th colspan="2" align="center"><%=GetLabel("NEW")%></th>
                                                    <th rowspan="2" align="center" style="width: 100px"><%=GetLabel("Unit")%></th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 60px" align="center"><%=GetLabel("ALL")%></th>
                                                    <th style="width: 60px" align="center"><%=GetLabel("Location")%></th>
                                                    <th style="width: 60px" align="center"><%=GetLabel("MIN")%></th>
                                                    <th style="width: 60px" align="center"><%=GetLabel("MAX")%></th>
                                                    <th style="width: 60px" align="right"><%=GetLabel("MIN")%></th>
                                                    <th style="width: 60px" align="right"><%=GetLabel("MAX")%></th>
                                                    <th style="width: 60px" align="right"><%=GetLabel("MIN")%></th>
                                                    <th style="width: 60px" align="right"><%=GetLabel("MAX")%></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("ID")%></td>
                                                <td align="center"><asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" /></td>
                                                <td class="tdItemName"><label class="lblItemName"><%#: Eval("ItemName1")%></label></td>
                                                <td align="right"><label><%#: Eval("QtyOnHandAll")%></label></td>
                                                <td align="right"><label><%#: Eval("QuantityEND")%></label></td>
                                                <td align="right"><asp:TextBox ID="txtAverageQty" Width="75px" runat="server" value="0" CssClass="number txtAverageQty" ReadOnly="true"/></td>
                                                <td align="right"><asp:TextBox ID="txtCurrentMinStock" Width="75px" runat="server" value="0" CssClass="number txtCurrentMinStock" ReadOnly="true"/></td>
                                                <td align="right"><asp:TextBox ID="txtCurrentMaxStock" Width="75px" runat="server" value="0" CssClass="number txtCurrentMaxStock" ReadOnly="true"/></td>
                                                <td align="right"><asp:TextBox ID="txtSystemMinStock" Width="75px" runat="server" value="0" CssClass="number txtSystemMinStock" ReadOnly="true"/></td>
                                                <td align="right"><asp:TextBox ID="txtSystemMaxStock" Width="75px" runat="server" value="0" CssClass="number txtSystemMaxStock" ReadOnly="true"/></td>
                                                <td align="right"><asp:TextBox ID="txtMinStock" Width="75px" runat="server" value="0" CssClass="number txtMinStock" ReadOnly="true"/></td>
                                                <td align="right"><asp:TextBox ID="txtMaxStock" Width="75px" runat="server" value="0" CssClass="number txtMaxStock" ReadOnly="true"/></td>
                                                <td align="left"><label><%#: Eval("ItemUnit")%></label></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
