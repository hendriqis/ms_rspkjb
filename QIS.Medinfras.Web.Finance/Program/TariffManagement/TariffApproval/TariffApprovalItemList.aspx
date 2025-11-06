<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPListEntry.master" AutoEventWireup="true" 
    CodeBehind="TariffApprovalItemList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.TariffApprovalItemList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dxwtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnCreateTariffBookItemBack" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div><%=GetLabel("View")%></div></li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhHeader" runat="server">
    <table style="width:100%">
        <tr>
            <td valign="top">
                <table>
                    <colgroup>
                        <col style="width:120px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Rumah Sakit")%></label></td>
                        <td><asp:TextBox ID="txtHealthcare" Width="300px" ReadOnly="true" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Skema Tarif")%></label></td>
                        <td><asp:TextBox ID="txtTariffScheme" Width="300px" ReadOnly="true" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Mulai Berlaku")%></label></td>
                        <td><asp:TextBox ID="txtEffectiveDate" Width="150px" ReadOnly="true" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Kelompok Item")%></label></td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width:320px">
                                        <dxe:ASPxDropDownEdit ClientInstanceName="ddeItemGroup" ID="ddeItemGroup"
                                            Width="300px" runat="server" EnableAnimation="False">
                                            <DropDownWindowStyle BackColor="#EDEDED" />
                                            <DropDownWindowTemplate>
                                                <dxwtl:ASPxTreeList ID="treeList" ClientInstanceName="treeList" runat="server" AutoGenerateColumns="False" DataSourceID="odsTree"
                                                    Width="100%" KeyFieldName="ItemGroupID" ParentFieldName="ParentID" OnCustomDataCallback="treeList_CustomDataCallback">
                                                    <Columns>
                                                        <dxwtl:TreeListDataColumn FieldName="ItemGroupName1" Caption="Item Group Name" CellStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <SettingsBehavior ExpandCollapseAction="NodeDblClick" />
                                                    <SettingsSelection Enabled="True" AllowSelectAll="true" Recursive="true" />
                                                    <ClientSideEvents CustomDataCallback="function(s, e) { ddeItemGroup.SetText(e.result); }"
                                                        SelectionChanged="function(s, e) { s.PerformCustomDataCallback(''); }" />
                                                </dxwtl:ASPxTreeList>
                                            </DropDownWindowTemplate>
                                        </dxe:ASPxDropDownEdit>
                                        <div style="display:none">
                                            <asp:ObjectDataSource ID="odsTree" runat="server" SelectMethod="GetItemGroupMasterList"
                                                TypeName="QIS.Medinfras.Data.Service.BusinessLayer">
                                                <SelectParameters>
                                                     <asp:SessionParameter DefaultValue="IsDeleted = 0" Name="filterExpression" SessionField="filterExpressionTariffBookItemGroup"
                                                        Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <dxcp:ASPxCallbackPanel ID="cbpGetTreeListSelectedValue" runat="server" Width="100%" ClientInstanceName="cbpGetTreeListSelectedValue"
                                                ShowLoadingPanel="false" OnCallback="cbpGetTreeListSelectedValue_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){onCbpGetTreeListSelectedValueEndCallback(s);}" />
                                            </dxcp:ASPxCallbackPanel>
                                            <input type="hidden" value="" id="hdnListItemGroupID" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkDisplayUpapprovedOnly" Wrap="False" runat="server" Checked="false" Text="Display Un-approved Item Only" />
                                    </td>
                                    <td style="padding-left:20px">
                                        <input type="button" id="btnApply" value='<%= GetLabel("Apply")%>' />
                                    </td>
                                </tr>
                            </table>   
                        </td>     
                    </tr>
                </table>
            </td>
            <td></td>
            <td valign="top" style="width:350px">
                <table>
                    <colgroup>
                        <col style="width:120px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("No. Buku Tarif")%></label></td>
                        <td><asp:TextBox ID="txtDocumentNo" Width="150px" ReadOnly="true" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Tanggal Dibuat")%></label></td>
                        <td><asp:TextBox ID="txtDocumentDate" Width="150px" ReadOnly="true" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("No. Revisi")%></label></td>
                        <td><asp:TextBox ID="txtRevisionNo" Width="150px" ReadOnly="true" CssClass="number" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>        
    </table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        //#region List
        $(function () {
            $('#grdView tr:gt(0):not(.trEmpty)').live('click', function () {
                var idx = $('#grdView tr').index($(this));
                if (idx > 1) {
                    $('#grdView tr.focus').removeClass('focus');
                    $(this).addClass('focus');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                }
            });
            $('#grdView tr:eq(2)').click();
        });

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
                    $('#grdView tr:eq(2)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#grdView tr:eq(2)').click();
        }
        //#endregion

        function onSelectedRowChanged(value) {
            var idx = $('#grdView tr').index($('#grdView tr.focus'));
            idx += value;
            if (idx < 2)
                idx = 2;
            if (idx == $('#grdView tr').length)
                idx--;
            $('#grdView tr:eq(' + idx + ')').click();
        }

        function getSelectedRow() {
            return $('#grdView tr.focus');
        }

        function onButtonCancelClick() {
            $('#grdView tr.selected').removeClass('selected');
        }
        //#endregion

        $(function () {
            $('#btnApply').click(function () {
                var allowApply = true;
                if (isEntryPanelVisible()) {
                    allowApply = confirm("Data Will not be saved. Are you sure?");
                }
                if (allowApply) {
                    hideEntryPanel();
                    showLoadingPanel();
                    cbpGetTreeListSelectedValue.PerformCallback();
                }
            });
        });

        function onCbpGetTreeListSelectedValueEndCallback(s) {            
            $('#<%=hdnListItemGroupID.ClientID %>').val(s.cpResult);
            cbpView.PerformCallback('refresh');
        }

        //#region Entity To Control
        function entityToControl(entity) {
            $('#grdView tr.selected').removeClass('selected');
            $('#<%=txtBaseTariffComp1.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtBaseTariffComp2.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtBaseTariffComp3.ClientID %>').val('0').trigger('changeValue');
            if (entity != null) {
                $('#grdView tr.focus').addClass('selected');

                $('#<%=chkIsApproved.ClientID %>').prop('checked', entity.IsApproved == 'True');
                $('#txtEditItem').val(entity.ItemName1);
                $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
                $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
                $('#<%=txtSuggestedTariff.ClientID %>').val(entity.SuggestedTariff).trigger('changeValue');
                $('#<%=txtBaseTariff.ClientID %>').val(entity.BaseTariff).trigger('changeValue');

                var lstProposedTariff = entity.ProposedTariff.split('|');
                var lstClassID = entity.ClassID.split('|');
                var bookID = $('#<%=hdnBookID.ClientID %>').val();
                Methods.getListObject('GetTariffBookDtCostList', 'BookID = ' + bookID + ' AND ItemID = ' + entity.ItemID, function (result) {
                    $('#ulClassProposed li').each(function () {
                        var classID = $(this).find('.hdnClassID').val();
                        var idx = lstClassID.indexOf(classID);
                        var lstTariff = lstProposedTariff[idx].split(';');
                        $(this).find('.txtClass').val(lstTariff[0]).trigger('changeValue');
                        $(this).find('.txtComponent1').val(lstTariff[1]).trigger('changeValue');
                        $(this).find('.txtComponent2').val(lstTariff[2]).trigger('changeValue');
                        $(this).find('.txtComponent3').val(lstTariff[3]).trigger('changeValue');
                    });
                    if (entity.IsApproved == 'False') {
                        $('#<%=txtApprovedBaseTariff.ClientID %>').val(entity.BaseTariff).trigger('changeValue');
                        $('#ulClassApproved li').each(function () {
                            var classID = $(this).find('.hdnClassID').val();
                            var idx = lstClassID.indexOf(classID);
                            var lstTariff = lstProposedTariff[idx].split(';');
                            $(this).find('.txtClass').val(lstTariff[0]).trigger('changeValue');
                            $(this).find('.txtComponent1').val(lstTariff[1]).trigger('changeValue');
                            $(this).find('.txtComponent2').val(lstTariff[2]).trigger('changeValue');
                            $(this).find('.txtComponent3').val(lstTariff[3]).trigger('changeValue');
                            for (var i = 0; i < result.length; ++i) {
                                if (parseInt(result[i].ClassID) == parseInt(classID)) {
                                    var itemCost = result[i];
                                    $(this).find('.hdnPrevBurden').val(itemCost.PreviousBurden);
                                    $(this).find('.hdnPrevLabor').val(itemCost.PreviousLabor);
                                    $(this).find('.hdnPrevMaterial').val(itemCost.PreviousMaterial);
                                    $(this).find('.hdnPrevOverhead').val(itemCost.PreviousOverhead);
                                    $(this).find('.hdnPrevSubContract').val(itemCost.PreviousSubContract);

                                    $(this).find('.hdnCurrentBurden').val(itemCost.CurrentBurden);
                                    $(this).find('.hdnCurrentLabor').val(itemCost.CurrentLabor);
                                    $(this).find('.hdnCurrentMaterial').val(itemCost.CurrentMaterial);
                                    $(this).find('.hdnCurrentOverhead').val(itemCost.CurrentOverhead);
                                    $(this).find('.hdnCurrentSubContract').val(itemCost.CurrentSubContract);

                                    $(this).find('.hdnTotalBurden').val(itemCost.TotalBurden);
                                    $(this).find('.hdnTotalLabor').val(itemCost.TotalLabor);
                                    $(this).find('.hdnTotalMaterial').val(itemCost.TotalMaterial);
                                    $(this).find('.hdnTotalOverhead').val(itemCost.TotalOverhead);
                                    $(this).find('.hdnTotalSubContract').val(itemCost.TotalSubContract);
                                }
                            }
                        });
                    }
                    else {
                        $('#<%=txtApprovedBaseTariff.ClientID %>').val(entity.ApprovedBaseTariff).trigger('changeValue');
                        var lstApprovedTariff = entity.ApprovedTariff.split('|');
                        $('#ulClassApproved li').each(function () {
                            var classID = $(this).find('.hdnClassID').val();
                            var idx = lstClassID.indexOf(classID);
                            var lstTariff = lstApprovedTariff[idx].split(';');
                            $(this).find('.txtClass').val(lstTariff[0]).trigger('changeValue');
                            $(this).find('.txtComponent1').val(lstTariff[1]).trigger('changeValue');
                            $(this).find('.txtComponent2').val(lstTariff[2]).trigger('changeValue');
                            $(this).find('.txtComponent3').val(lstTariff[3]).trigger('changeValue');
                            for (var i = 0; i < result.length; ++i) {
                                if (parseInt(result[i].ClassID) == parseInt(classID)) {
                                    var itemCost = result[i];
                                    $(this).find('.hdnPrevBurden').val(itemCost.PreviousBurden);
                                    $(this).find('.hdnPrevLabor').val(itemCost.PreviousLabor);
                                    $(this).find('.hdnPrevMaterial').val(itemCost.PreviousMaterial);
                                    $(this).find('.hdnPrevOverhead').val(itemCost.PreviousOverhead);
                                    $(this).find('.hdnPrevSubContract').val(itemCost.PreviousSubContract);

                                    $(this).find('.hdnCurrentBurden').val(itemCost.CurrentBurden);
                                    $(this).find('.hdnCurrentLabor').val(itemCost.CurrentLabor);
                                    $(this).find('.hdnCurrentMaterial').val(itemCost.CurrentMaterial);
                                    $(this).find('.hdnCurrentOverhead').val(itemCost.CurrentOverhead);
                                    $(this).find('.hdnCurrentSubContract').val(itemCost.CurrentSubContract);

                                    $(this).find('.hdnTotalBurden').val(itemCost.TotalBurden);
                                    $(this).find('.hdnTotalLabor').val(itemCost.TotalLabor);
                                    $(this).find('.hdnTotalMaterial').val(itemCost.TotalMaterial);
                                    $(this).find('.hdnTotalOverhead').val(itemCost.TotalOverhead);
                                    $(this).find('.hdnTotalSubContract').val(itemCost.TotalSubContract);
                                }
                            }
                        });
                    }

                    var diff = parseInt($('#<%=txtApprovedBaseTariff.ClientID %>').attr('hiddenVal')) - parseInt($('#<%=txtBaseTariff.ClientID %>').attr('hiddenVal'));
                    var diffPercentage = diff * 100 / parseInt($('#<%=txtBaseTariff.ClientID %>').attr('hiddenVal'));
                    $('#<%=txtDiff.ClientID %>').val(diffPercentage);
                });  
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtSuggestedTariff.ClientID %>').val('').trigger('changeValue');
                $('#<%=txtBaseTariff.ClientID %>').val('').trigger('changeValue');
                $('#ulClassProposed li').each(function () {
                    $(this).find('.txtClass').val('').trigger('changeValue');
                    $(this).find('.hdnPrevBurden').val('0');
                    $(this).find('.hdnPrevLabor').val('0');
                    $(this).find('.hdnPrevMaterial').val('0');
                    $(this).find('.hdnPrevOverhead').val('0');
                    $(this).find('.hdnPrevSubContract').val('0');

                    $(this).find('.hdnCurrentBurden').val('0');
                    $(this).find('.hdnCurrentLabor').val('0');
                    $(this).find('.hdnCurrentMaterial').val('0');
                    $(this).find('.hdnCurrentOverhead').val('0');
                    $(this).find('.hdnCurrentSubContract').val('0');

                    $(this).find('.hdnTotalBurden').val('0');
                    $(this).find('.hdnTotalLabor').val('0');
                    $(this).find('.hdnTotalMaterial').val('0');
                    $(this).find('.hdnTotalOverhead').val('0');
                    $(this).find('.hdnTotalSubContract').val('0');
                });
                $('#txtEditItem').val('');
            }
            $('#<%=chkIsApproved.ClientID %>').focus();
        }
        //#endregion

        $editedLi = null;
        $(function () {
            $('#<%=btnCreateTariffBookItemBack.ClientID %>').click(function () {
                showLoadingPanel();
                var url = ResolveUrl('~/Program/TariffManagement/TariffApproval/TariffApprovalList.aspx');
                document.location = url;
            });

            $('#<%=txtBaseTariff.ClientID %>').change(function () {
                var baseTariff = parseInt($(this).val());
                $('#ulClassProposed li').each(function () {
                    var margin = $(this).find('.hdnMarginPercentage').val();
                    var value = baseTariff * (100 + parseFloat(margin)) / 100;
                    $(this).find('.txtClass').val(value).trigger('changeValue');
                });
            });

            $('#<%=txtApprovedBaseTariff.ClientID %>').change(function () {
                var baseTariff = parseInt($(this).val());
                $('#ulClassApproved li').each(function () {
                    var margin = $(this).find('.hdnMarginPercentage').val();
                    var value = baseTariff * (100 + parseFloat(margin)) / 100;
                    $(this).find('.txtClass').val(value).change();
                });

                var diff = parseInt(baseTariff) - parseInt($('#<%=txtBaseTariff.ClientID %>').attr('hiddenVal'));
                var diffPercentage = diff * 100 / parseInt($('#<%=txtBaseTariff.ClientID %>').attr('hiddenVal'));
                $('#<%=txtDiff.ClientID %>').val(diffPercentage);
            });

            $('#<%=txtBaseTariffComp1.ClientID %>').change(function () {
                var baseTariffComp1 = parseInt($(this).val());
                $('#ulClassApproved li').each(function () {
                    $(this).find('.txtComponent1').val(baseTariffComp1).change();
                });
            });

            $('#<%=txtBaseTariffComp2.ClientID %>').change(function () {
                var baseTariffComp2 = parseInt($(this).val());
                $('#ulClassApproved li').each(function () {
                    $(this).find('.txtComponent2').val(baseTariffComp2).change();
                });
            });

            $('#<%=txtBaseTariffComp3.ClientID %>').change(function () {
                var baseTariffComp3 = parseInt($(this).val());
                $('#ulClassApproved li').each(function () {
                    $(this).find('.txtComponent3').val(baseTariffComp3).change();
                });
            });

            $('#<%=txtDiff.ClientID %>').change(function () {
                var newBaseTariffValue = parseInt($('#<%=txtBaseTariff.ClientID %>').attr('hiddenVal')) * (100 + parseInt($(this).val())) / 100;
                $('#<%=txtApprovedBaseTariff.ClientID %>').val(newBaseTariffValue).trigger('changeValue');
                $('#ulClassApproved li').each(function () {
                    var margin = $(this).find('.hdnMarginPercentage').val();
                    var value = newBaseTariffValue * (100 + parseFloat(margin)) / 100;
                    $(this).find('.txtClass').val(value).change();
                });
            });

            $('.btnEditItemCost').click(function () {
                $li = $(this).closest('li');
                $editedLi = $li;

                $('#txtEditClassName').val($li.find('.hdnClassName').val());

                $('#txtBurdenPrev').val($li.find('.hdnPrevBurden').val()).trigger('changeValue');
                $('#txtLaborPrev').val($li.find('.hdnPrevLabor').val()).trigger('changeValue');
                $('#txtMaterialPrev').val($li.find('.hdnPrevMaterial').val()).trigger('changeValue');
                $('#txtOverheadPrev').val($li.find('.hdnPrevOverhead').val()).trigger('changeValue');
                $('#txtSubContractPrev').val($li.find('.hdnPrevSubContract').val()).trigger('changeValue');

                $('#txtBurdenCurrent').val($li.find('.hdnCurrentBurden').val()).trigger('changeValue');
                $('#txtLaborCurrent').val($li.find('.hdnCurrentLabor').val()).trigger('changeValue');
                $('#txtMaterialCurrent').val($li.find('.hdnCurrentMaterial').val()).trigger('changeValue');
                $('#txtOverheadCurrent').val($li.find('.hdnCurrentOverhead').val()).trigger('changeValue');
                $('#txtSubContractCurrent').val($li.find('.hdnCurrentSubContract').val()).trigger('changeValue');

                $('#txtBurdenTotal').val($li.find('.hdnTotalBurden').val()).trigger('changeValue');
                $('#txtLaborTotal').val($li.find('.hdnTotalLabor').val()).trigger('changeValue');
                $('#txtMaterialTotal').val($li.find('.hdnTotalMaterial').val()).trigger('changeValue');
                $('#txtOverheadTotal').val($li.find('.hdnTotalOverhead').val()).trigger('changeValue');
                $('#txtSubContractTotal').val($li.find('.hdnTotalSubContract').val()).trigger('changeValue');

                pcItemCost.Show();
            });


            $('#btnItemCostSave').click(function () {
                $editedLi.find('.hdnPrevBurden').val($('#txtBurdenPrev').attr('hiddenVal'));
                $editedLi.find('.hdnPrevLabor').val($('#txtLaborPrev').attr('hiddenVal'));
                $editedLi.find('.hdnPrevMaterial').val($('#txtMaterialPrev').attr('hiddenVal'));
                $editedLi.find('.hdnPrevOverhead').val($('#txtOverheadPrev').attr('hiddenVal'));
                $editedLi.find('.hdnPrevSubContract').val($('#txtSubContractPrev').attr('hiddenVal'));

                $editedLi.find('.hdnCurrentBurden').val($('#txtBurdenCurrent').attr('hiddenVal'));
                $editedLi.find('.hdnCurrentLabor').val($('#txtLaborCurrent').attr('hiddenVal'));
                $editedLi.find('.hdnCurrentMaterial').val($('#txtMaterialCurrent').attr('hiddenVal'));
                $editedLi.find('.hdnCurrentOverhead').val($('#txtOverheadCurrent').attr('hiddenVal'));
                $editedLi.find('.hdnCurrentSubContract').val($('#txtSubContractCurrent').attr('hiddenVal'));

                $editedLi.find('.hdnTotalBurden').val($('#txtBurdenTotal').attr('hiddenVal'));
                $editedLi.find('.hdnTotalLabor').val($('#txtLaborTotal').attr('hiddenVal'));
                $editedLi.find('.hdnTotalMaterial').val($('#txtMaterialTotal').attr('hiddenVal'));
                $editedLi.find('.hdnTotalOverhead').val($('#txtOverheadTotal').attr('hiddenVal'));
                $editedLi.find('.hdnTotalSubContract').val($('#txtSubContractTotal').attr('hiddenVal'));

                pcItemCost.Hide();
            });

            $('.txtItemCostCurrent').change(function () {
                $prev = parseInt($(this).closest('tr').find('.txtItemCostPrev').attr('hiddenVal'));
                $current = parseInt($(this).val());
                $total = $prev + $current;
                $(this).closest('tr').find('.txtItemCostTotal').val($total).trigger('changeValue');
            });

            $('.txtClass').change(function () {
                $(this).blur();
                $li = $(this).closest('li');
                var totalClass = parseInt($(this).attr('hiddenVal'));

                $txtComponent1 = $li.find('.txtComponent1');
                $txtComponent2 = $li.find('.txtComponent2');
                $txtComponent3 = $li.find('.txtComponent3');

                var component2 = parseInt($txtComponent2.attr('hiddenVal'));
                var component3 = parseInt($txtComponent3.attr('hiddenVal'));
                var component1 = totalClass - (component2 + component3);
                if (component1 < 0) {
                    $txtComponent1.val(totalClass).trigger('changeValue');
                    $txtComponent2.val('0').trigger('changeValue');
                    $txtComponent3.val('0').trigger('changeValue');
                }
                else {
                    $txtComponent1.val(component1).trigger('changeValue');
                }
            });

            $('.txtComponent').change(function () {
                $(this).blur();
                $li = $(this).closest('li');
                calculateTariffTotalPerClass($li);
            });

            function calculateTariffTotalPerClass($li) {
                $total = 0;
                $li.find('.txtComponent').each(function () {
                    $total += parseInt($(this).attr('hiddenVal'));
                });
                $li.find('.txtClass').val($total).trigger('changeValue');
            }
        });
    </script>
</asp:Content>

<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <style type="text/css">
        .ulClass                { margin: 0; padding: 0; }
        .ulClass li             { list-style-type: none; display: inline-block; width: 150px; margin-bottom: 10px; }                
    </style>
    <input type="hidden" value="" id="hdnListClassID" runat="server" />
    <input type="hidden" value="" id="hdnListClassName" runat="server" />
    <input type="hidden" value="" id="hdnListClassMargin" runat="server" />
    <table style="width:100%" class="tblEntryDetail">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td valign="top">
                <table>
                    <colgroup>
                        <col style="width:150px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Item")%></label></td>
                        <td colspan="2">
                            <input type="hidden" value="" id="hdnItemID" runat="server" />
                            <asp:TextBox ID="txtItemName" Width="300px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Suggested Tariff")%></label></td>
                        <td><asp:TextBox ID="txtSuggestedTariff" ReadOnly="true" CssClass="txtCurrency" Width="150px" runat="server" /> </td>
                    </tr>
                    <tr>
                        <td colspan="2"><div class="lblComponent" style="text-align:left; margin-top: 10px;"><%=GetLabel("TARIF YANG DIAJUKAN :")%></div></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Base Tariff")%></label></td>
                        <td><asp:TextBox ID="txtBaseTariff" ReadOnly="true" CssClass="txtCurrency" Width="150px" runat="server" /> </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal"><%=GetLabel("Kelas")%></label>
                            <div style="margin-top: 32px;" id="divProposedComponentLabel" runat="server">
                                <div style="height:23px"><%=GetTariffComponent1Text()%></div>
                                <div style="height:23px"><%=GetTariffComponent2Text()%></div>
                                <div style="height:23px"><%=GetTariffComponent3Text()%></div>
                            </div>
                        </td>
                        <td>
                            <asp:Repeater id="rptClassCareProposed" runat="server" OnItemDataBound="rptClassCareProposed_ItemDataBound">
                                <HeaderTemplate>
                                    <ul id="ulClassProposed" class="ulClass">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <div class="lblComponent"><%#: Eval("ClassName") %> (<span style="color:Blue"><%#: Eval("MarginPercentage1")%>%</span>)</div>
                                        <div><asp:TextBox ID="txtClass" ReadOnly="true" runat="server" Width="98%" CssClass="txtCurrency txtClass" /></div>
                                        <div style="margin-top: 5px;" id="divProposedComponent" runat="server">
                                            <asp:TextBox ID="txtComponent1" runat="server" Width="98%" ReadOnly="true" CssClass="txtCurrency txtComponent txtComponent1" />
                                            <asp:TextBox ID="txtComponent2" runat="server" Width="98%" ReadOnly="true" CssClass="txtCurrency txtComponent txtComponent2" />
                                            <asp:TextBox ID="txtComponent3" runat="server" Width="98%" ReadOnly="true" CssClass="txtCurrency txtComponent txtComponent3" />
                                        </div>
                                        <input type="hidden" id="hdnMarginPercentage" class="hdnMarginPercentage" value='<%#: Eval("MarginPercentage1")%>' runat="server" />
                                        <input type="hidden" id="hdnClassID" class="hdnClassID" value='<%#: Eval("ClassID")%>' runat="server" />
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><div class="lblComponent" style="text-align:left; margin-top: 10px;"><%=GetLabel("TARIF YANG DISETUJUI :")%></div></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Disetujui")%></label></td>
                        <td><asp:CheckBox ID="chkIsApproved" runat="server" /> </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Base Tariff")%></label></td>
                        <td><asp:TextBox ID="txtApprovedBaseTariff" CssClass="txtCurrency" Width="150px" runat="server" />
                        <label class="lblNormal">Sarana</label>
                        <asp:TextBox ID="txtBaseTariffComp1" CssClass="txtCurrency" Width="150px" runat="server" />
                        <label class="lblNormal">Pelayanan</label>
                        <asp:TextBox ID="txtBaseTariffComp2" CssClass="txtCurrency" Width="150px" runat="server" />
                        <label class="lblNormal">Lain - lain</label>
                        <asp:TextBox ID="txtBaseTariffComp3" CssClass="txtCurrency" Width="150px" runat="server" /> </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Diff")%></label></td>
                        <td><asp:TextBox ID="txtDiff" CssClass="number" Width="150px" runat="server" /> %</td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal"><%=GetLabel("Kelas")%></label>                            
                            <div style="margin-top: 32px;" id="divApprovedComponentLabel" runat="server">
                                <div style="height:23px"><%=GetTariffComponent1Text()%></div>
                                <div style="height:23px"><%=GetTariffComponent2Text()%></div>
                                <div style="height:23px"><%=GetTariffComponent3Text()%></div>
                            </div>
                        </td>
                        <td>
                            <asp:Repeater id="rptClassCareApproved" runat="server" OnItemDataBound="rptClassCareApproved_ItemDataBound">
                                <HeaderTemplate>
                                    <ul id="ulClassApproved" class="ulClass">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <div class="lblComponent"><%#: Eval("ClassName") %> (<span style="color:Blue"><%#: Eval("MarginPercentage1")%>%</span>)</div>
                                        <span><asp:TextBox ID="txtClass" runat="server" Width="84%" CssClass="txtCurrency txtClass" /></span>
                                        <span><input type="button" class="btnEditItemCost" title='<%=GetLabel("Item Cost") %>' value="..." style="width:10%"  /></span>
                                        <div style="margin-top: 5px;" id="divApprovedComponent" runat="server">
                                            <asp:TextBox ID="txtComponent1" runat="server" Width="84%" CssClass="txtCurrency txtComponent txtComponent1" />
                                            <asp:TextBox ID="txtComponent2" runat="server" Width="84%" CssClass="txtCurrency txtComponent txtComponent2" />
                                            <asp:TextBox ID="txtComponent3" runat="server" Width="84%" CssClass="txtCurrency txtComponent txtComponent3" />
                                        </div>
                                        <input type="hidden" id="hdnMarginPercentage" class="hdnMarginPercentage" value='<%#: Eval("MarginPercentage1")%>' runat="server" />
                                        <input type="hidden" id="hdnClassID" class="hdnClassID" value='<%#: Eval("ClassID")%>' runat="server" />
                                        <input type="hidden" class="hdnClassName" value='<%#: Eval("ClassName")%>' />
                                        <input type="hidden" id="hdnPrevBurden" class="hdnPrevBurden" runat="server" />
                                        <input type="hidden" id="hdnPrevLabor" class="hdnPrevLabor" runat="server" />
                                        <input type="hidden" id="hdnPrevMaterial" class="hdnPrevMaterial" runat="server" />
                                        <input type="hidden" id="hdnPrevOverhead" class="hdnPrevOverhead" runat="server" />
                                        <input type="hidden" id="hdnPrevSubContract" class="hdnPrevSubContract" runat="server" />

                                        <input type="hidden" id="hdnCurrentBurden" class="hdnCurrentBurden" runat="server" />
                                        <input type="hidden" id="hdnCurrentLabor" class="hdnCurrentLabor" runat="server" />
                                        <input type="hidden" id="hdnCurrentMaterial" class="hdnCurrentMaterial" runat="server" />
                                        <input type="hidden" id="hdnCurrentOverhead" class="hdnCurrentOverhead" runat="server" />
                                        <input type="hidden" id="hdnCurrentSubContract" class="hdnCurrentSubContract" runat="server" />
                                        
                                        <input type="hidden" id="hdnTotalBurden" class="hdnTotalBurden" runat="server" />
                                        <input type="hidden" id="hdnTotalLabor" class="hdnTotalLabor" runat="server" />
                                        <input type="hidden" id="hdnTotalMaterial" class="hdnTotalMaterial" runat="server" />
                                        <input type="hidden" id="hdnTotalOverhead" class="hdnTotalOverhead" runat="server" />
                                        <input type="hidden" id="hdnTotalSubContract" class="hdnTotalSubContract" runat="server" />
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnBookID" runat="server" />
    <input type="hidden" value="" id="hdnGCItemType" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                        <table id="grdView" class="grdSelected grdPatientPage" cellspacing="0" rules="all" >
                            <tr>
                                <th class="keyField" rowspan="2"></th>
                                <th class="hiddenColumn" rowspan="2"></th>
                                <th style="width:300px" rowspan="2" align="left"><%=GetLabel("Item")%></th>
                                <th style="width:120px" rowspan="2" align="right"><%=GetLabel("Suggested Tariff")%></th>
                                <th style="width:120px" rowspan="2" align="right"><%=GetLabel("Base Tariff")%></th>
                                <th colspan="<%=ClassCount %>"><%=GetLabel("Kelas Tagihan") %></th>
                                <th style="width:80" rowspan="2"><%=GetLabel("Approved")%></th>
                            </tr>
                            <tr>                                
                                <asp:Repeater ID="rptTariffBookClassHeader" runat="server">
                                    <ItemTemplate>
                                        <th style="width:150px"><%#:Eval("ClassName") %> <br /> (<span style="color:Blue"> <%#:Eval("MarginPercentage1")%>% </span> )</th>
                                    </ItemTemplate>
                                </asp:Repeater>     
                            </tr>
                        <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                            <LayoutTemplate>                                
                                <tr runat="server" id="itemPlaceholder" ></tr>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="keyField"><%#: Eval("ItemID") %></td>
                                    <td class="hiddenColumn">
                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                        <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                        <input type="hidden" value="<%#:Eval("SuggestedTariff") %>" bindingfield="SuggestedTariff" />
                                        <input type="hidden" value="<%#:Eval("BaseTariff") %>" bindingfield="BaseTariff" />
                                        <input type="hidden" value="<%#:Eval("ApprovedBaseTariff") %>" bindingfield="ApprovedBaseTariff" />
                                        <input type="hidden" value="<%#:Eval("ClassID") %>" bindingfield="ClassID" />
                                        <input type="hidden" value="<%#:Eval("ProposedTariff") %>" bindingfield="ProposedTariff" />
                                        <input type="hidden" value="<%#:Eval("ApprovedTariff") %>" bindingfield="ApprovedTariff" />
                                        <input type="hidden" value="<%#:Eval("IsApproved") %>" bindingfield="IsApproved" />
                                    </td>
                                    <td><%#: Eval("ItemName1")%></td>
                                    <td align="right"><%#: DataBinder.Eval(Container.DataItem, "SuggestedTariff", "{0:N}")%></td>
                                    <td align="right"><%#: DataBinder.Eval(Container.DataItem, "DisplayBaseTariff", "{0:N}")%></td>
                                    <asp:Repeater ID="rptTariffBookClass" runat="server">
                                        <ItemTemplate>
                                            <td align="right"><%#: String.Format("{0, 0:N2}", Container.DataItem)%></td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <td align="center"><asp:CheckBox ID="chkIsApproved" runat="server" Enabled="false" /></td>
                                </tr>
                            </ItemTemplate>    
                            <EmptyDataTemplate>
                                <tr class="trEmpty">
                                    <td colspan="12">
                                        <%=GetLabel("No Data To Display") %>
                                    </td>
                                </tr>
                            </EmptyDataTemplate>
                        </asp:ListView>
                        </table>
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

    <!-- Popup Item Cost -->
    <dxpc:ASPxPopupControl ID="pcItemCost" runat="server" ClientInstanceName="pcItemCost" CloseAction="CloseButton"
        Height="300px" HeaderText="Item Cost" Width="600px" Modal="True" PopupAction="None"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dxpc:PopupControlContentControl runat="server" ID="pccc1">
                <dx:ASPxPanel ID="pnlItemCost" runat="server" Width="100%">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <fieldset id="fsItemCost" style="margin:0"> 
                                <div style="text-align: left; width: 100%;">
                                    <table>
                                        <colgroup>
                                            <col style="width: 500px"/>
                                        </colgroup>
                                        <tr>
                                            <td valign="top">
                                                <table>
                                                    <colgroup>
                                                        <col style="width:160px"/>
                                                        <col style="width:120px"/>
                                                        <col style="width:120px"/>
                                                        <col style="width:120px"/>
                                                    </colgroup>
                                                    <tr>
                                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Item")%></label></td>
                                                        <td colspan="3"><input id="txtEditItem" readonly="readonly" style="width:100%" /></td>
                                                    </tr>  
                                                    <tr>
                                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Class")%></label></td>
                                                        <td colspan="2"><input id="txtEditClassName" readonly="readonly" style="width:100%" /></td>
                                                    </tr>                            
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td align="center"><div class="lblComponent"><%=GetLabel("Previous")%></div></td>
                                                        <td align="center"><div class="lblComponent"><%=GetLabel("Current")%></div></td>
                                                        <td align="center"><div class="lblComponent"><%=GetLabel("Total")%></div></td>
                                                    </tr>                          
                                                    <tr>
                                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Material")%></label></td>
                                                        <td><input id="txtMaterialPrev" class="txtCurrency txtItemCostPrev" readonly="readonly" style="width:100%" /></td>
                                                        <td><input id="txtMaterialCurrent" class="txtCurrency required txtItemCostCurrent" style="width:100%" /></td>
                                                        <td><input id="txtMaterialTotal" class="txtCurrency txtItemCostTotal" readonly="readonly" style="width:100%" /></td>
                                                    </tr>                           
                                                    <tr>
                                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Labor")%></label></td>
                                                        <td><input id="txtLaborPrev" class="txtCurrency txtItemCostPrev" readonly="readonly" style="width:100%" /></td>
                                                        <td><input id="txtLaborCurrent" class="txtCurrency required txtItemCostCurrent" style="width:100%" /></td>
                                                        <td><input id="txtLaborTotal" class="txtCurrency txtItemCostTotal" readonly="readonly" style="width:100%" /></td>
                                                    </tr>                           
                                                    <tr>
                                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Overhead")%></label></td>
                                                        <td><input id="txtOverheadPrev" class="txtCurrency txtItemCostPrev" readonly="readonly" style="width:100%" /></td>
                                                        <td><input id="txtOverheadCurrent" class="txtCurrency required txtItemCostCurrent" style="width:100%" /></td>
                                                        <td><input id="txtOverheadTotal" class="txtCurrency txtItemCostTotal" readonly="readonly" style="width:100%" /></td>
                                                    </tr>                           
                                                    <tr>
                                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Sub Contract")%></label></td>
                                                        <td><input id="txtSubContractPrev" class="txtCurrency txtItemCostPrev" readonly="readonly" style="width:100%" /></td>
                                                        <td><input id="txtSubContractCurrent" class="txtCurrency required txtItemCostCurrent" style="width:100%" /></td>
                                                        <td><input id="txtSubContractTotal" class="txtCurrency txtItemCostTotal" readonly="readonly" style="width:100%" /></td>
                                                    </tr>                           
                                                    <tr>
                                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Burden")%></label></td>
                                                        <td><input id="txtBurdenPrev" class="txtCurrency txtItemCostPrev" readonly="readonly" style="width:100%" /></td>
                                                        <td><input id="txtBurdenCurrent" class="txtCurrency required txtItemCostCurrent" style="width:100%" /></td>
                                                        <td><input id="txtBurdenTotal" class="txtCurrency txtItemCostTotal" readonly="readonly" style="width:100%" /></td>
                                                    </tr>  
                                                </table>  
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
                                        <tr>
                                            <td>
                                                <input type="button" id="btnItemCostSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnItemCostClose" value='<%= GetLabel("Close")%>' onclick="pcItemCost.Hide();" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>                                
                            </fieldset>                                
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>