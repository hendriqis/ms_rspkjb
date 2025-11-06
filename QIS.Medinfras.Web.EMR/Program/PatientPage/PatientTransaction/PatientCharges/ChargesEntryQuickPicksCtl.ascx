<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChargesEntryQuickPicksCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.ChargesEntryQuickPicksCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_drugslogisticsquickpicksctl">
    $("#<%=rblItemType.ClientID %> input").change(function () {
        var value = $(this).val();
        cbpPopup.PerformCallback('refresh');
    });

    function addItemFilterRow() {
        $trHeader = $('#<%=grdView.ClientID %> tr:eq(0)');
        $trFilter = $("<tr><td></td><td></td></tr>");

        $input = $("<input type='text' id='txtFilterItem' style='width:100%;height:20px' />").val($('#<%=hdnFilterItem.ClientID %>').val());
        $trFilter.find('td').eq(1).append($input);
        $trFilter.insertAfter($trHeader);
    }

    $('#txtFilterItem').live('keypress', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            getCheckedMember();
            $('#<%=hdnFilterItem.ClientID %>').val($(this).val());
            e.preventDefault();
            cbpPopup.PerformCallback('refresh');
        }
    });

    $(function () {
        hideLoadingPanel();
        addItemFilterRow();
    });

    function onBeforeSaveRecord(errMessage) {
//        if (IsValid(null, 'fsDrugsQuickPicks', 'mpDrugsQuickPicks')) {
            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() != '')
                return true;
            else {
                errMessage.text = 'Please Select Item First';
                return false;
            }
//        }
        return false;
    }

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberQty = [];
        var lstSelectedMemberUnit = [];
        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {
            var key = $(this).find('.keyField').val();
            var unit = 'X003^X' //$(this).find('.itemUnit').val();
            var qty = $(this).find('.txtQty').val();
            lstSelectedMember.push(key);
            lstSelectedMemberQty.push(qty);
            lstSelectedMemberUnit.push(unit);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberQty.ClientID %>').val(lstSelectedMemberQty.join(','));
        $('#<%=hdnSelectedMemberUnit.ClientID %>').val(lstSelectedMemberUnit.join(','));
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            getCheckedMember();
            cbpPopup.PerformCallback('changepage|' + page);
        });

        //#region Item Group
        $('#lblItemGroupDrugLogistic.lblLink').click(function () {
            var filterExpression = "GCItemType = 'X001^002' AND IsDeleted = 0";
            openSearchDialog('itemgroup', filterExpression, function (value) {
                $('#<%=txtItemGroupCode.ClientID %>').val(value);
                ontxtItemGroupCodeChanged(value);
            });
        });

        $('#<%=txtItemGroupCode.ClientID %>').change(function () {
            ontxtItemGroupCodeChanged($(this).val());
        });

        function ontxtItemGroupCodeChanged(value) {
            var filterExpression = "ItemGroupCode = '" + value + "'";
            Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                    $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);
                }
                else {
                    $('#<%=hdnItemGroupID.ClientID %>').val('');
                    $('#<%=txtItemGroupCode.ClientID %>').val('');
                    $('#<%=txtItemGroupName.ClientID %>').val('');
                }
                getCheckedMember();
                cbpPopup.PerformCallback('refresh');
            });
        }
        //#endregion
    });

    function onCbpPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                getCheckedMember();
                cbpPopup.PerformCallback('changepage|' + page);
            });
        }
        addItemFilterRow();
    }
    //#endregion

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');

            $newTr = $('#tmplSelectedTestItem').html();
            //            $newTr = $newTr.replace(/\$\{GCItemUnit}/g, $selectedTr.find('.tdItemUnit').html());
            $newTr = $newTr.replace(/\$\{ItemName1}/g, $selectedTr.find('.tdItemName1').html());
            $newTr = $newTr.replace(/\$\{ItemID}/g, $selectedTr.find('.keyField').html());
            $newTr = $($newTr);

            var isQtyAllowChangeForDoctor = $selectedTr.find('.IsQtyAllowChangeForDoctor').html();

            var setvarAlowChangeChargesQty = $('#<%=hdnIsAllowChangeChargesQty.ClientID %>').val();

            if (setvarAlowChangeChargesQty == "1") {
                if (isQtyAllowChangeForDoctor == "True")
                    $newTr.find(".txtQty").removeAttr("disabled");
                else
                    $newTr.find(".txtQty").attr("disabled", "disabled");
            }

            $newTr.insertBefore($('#trFooter'));
        }
        else {
            var id = $(this).closest('tr').find('.keyField').html();
            $('#tblSelectedItem tr').each(function () {
                if ($(this).find('.keyField').val() == id) {
                    $(this).remove();
                }
            });
        }
    });

    $('#tblSelectedItem .chkIsSelected2').die('change');
    $('#tblSelectedItem .chkIsSelected2').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');
            var id = $selectedTr.find('.keyField').val();
            var isFound = false;
            $('#<%=grdView.ClientID %> tr').each(function () {
                if (id == $(this).find('.keyField').html()) {
                    $(this).find('.chkIsSelected').find('input').prop('checked', false);
                    isFound = true;
                }
            });
            if (!isFound) {
                var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
                lstSelectedMember.splice(lstSelectedMember.indexOf(id), 1);
                $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            }
            $(this).closest('tr').remove();
        }
    });
</script>

<div style="padding:10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${ItemID}' />
            </td>
            <td>${ItemName1}</td>
            <td style="text-align:right"><input type="text" validationgroup="mpDrugsQuickPicks" id="txtQty" runat="server" class="txtQty number min" min="1" value="1" style="width:40px" /></td>
        </tr>
    </script>
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="0" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnChargeClassID" runat="server" value="" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnSelectedMemberUnit" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
    <input type="hidden" id="hdnServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnLocationID" runat="server" value="" />
    <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnLongConsultationMinutes" runat="server" value="0" />
    <input type="hidden" id="hdnIsAllowChangeChargesQty" runat="server" value="1" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
    <table>
        <colgroup>
            <col style="width:150px"/>
            <col style="width:400px"/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Service Unit")%></label></td>
            <td>
                <dxe:ASPxComboBox ID="cboPopupServiceUnit" ClientInstanceName="cboPopupServiceUnit" Width="100%" runat="server" OnCallback="cboPopupServiceUnit_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" 
                      EndCallback="function(s,e){ hideLoadingPanel(); }" 
                      ValueChanged="function(s,e){ oncboPopupServiceUnitValueChanged(e) }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr style="display:none">
            <td class="tdLabel">
                <label class="lblNormal"><%=GetLabel("Item Location")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox runat="server" ID="cboLocation" ClientInstanceName="cboLocation"
                    Width="100%" OnCallback="cboLocation_Callback">
                    <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }" EndCallback="function() { hideLoadingPanel(); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr style="display:none">
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Item Type")%></label></td>
            <td>
                <asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Pelayanan" Value="1" Selected = "True"  />
                    <asp:ListItem Text="Obat dan Alkes" Value="2" />
                    <asp:ListItem Text="Barang Umum" Value="3" />
                </asp:RadioButtonList>
            </td>            
        </tr>
        <tr style="display:none">
            <td class="tdLabel"><label class="lblLink" id="lblItemGroupDrugLogistic"><%=GetLabel("Kelompok Barang")%></label></td>
            <td>
                <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
                <table style="width:100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width:30%"/>
                        <col style="width:3px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td><asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" /></td>
                        <td>&nbsp;</td>
                        <td><asp:TextBox ID="txtItemGroupName" Width="100%" runat="server" ReadOnly="true"/></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width:100%">
        <colgroup>
            <col style="width:45%"/>
            <col style="width:55%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <h4><%=GetLabel("Available service unit item :")%></h4>
                <dxcp:ASPxCallbackPanel ID="cbpPopup" runat="server" Width="100%" ClientInstanceName="cbpPopup"
                    ShowLoadingPanel="false" OnCallback="cbpPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}"
                        EndCallback="function(s,e){ onCbpPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField"/>
                                        <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemName1" HeaderText="Item Name" ItemStyle-CssClass="tdItemName1" />
                                        <asp:BoundField DataField="IsQtyAllowChangeForDoctor" HeaderStyle-CssClass="IsQtyAllowChangeForDoctor hiddenColumn"
                                            ItemStyle-CssClass="IsQtyAllowChangeForDoctor hiddenColumn" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup"></div>
                    </div>
                </div>
            </td>
            <td style="padding:5px;vertical-align:top">
                <h4><%=GetLabel("Selected item(s) :")%></h4>
                <fieldset id="fsDrugsQuickPicks">
                    <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                        <tr id="trHeader2">
                            <th style="width:20px">&nbsp;</th>
                            <th align="left"><%=GetLabel("Item Name")%></th> 
                            <th align="right"style="width:40px"><%=GetLabel("Qty")%></th> 
                        </tr>
                        <tr id="trFooter"></tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
        
    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div> 
</div>