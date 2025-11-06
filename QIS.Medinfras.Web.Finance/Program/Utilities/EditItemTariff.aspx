<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="EditItemTariff.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.EditItemTariff" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.txtClass').die('change');
            $('.txtClass').live('change', function () {
                $(this).blur();
                $li = $(this).closest('li');
                var totalClass = parseFloat($(this).val());

                $txtComponent1 = $li.find('.txtComponent1');
                $txtComponent2 = $li.find('.txtComponent2');
                $txtComponent3 = $li.find('.txtComponent3');

                var component2 = parseFloat($txtComponent2.val());
                var component3 = parseFloat($txtComponent3.val());
                var component1 = totalClass - (component2 + component3);

                if (component1 < 0) {
                    $txtComponent1.val(totalClass).trigger('changeValue');
                    $txtComponent2.val('0').trigger('changeValue');
                    $txtComponent3.val('0').trigger('changeValue');
                }
                else {
                    $txtComponent1.val(component1).trigger('changeValue');
                    //                    $txtComponent2.val(component2).trigger('changeValue');
                    //                    $txtComponent3.val(component3).trigger('changeValue');
                }
            });

            $('.txtComponent').die('change');
            $('.txtComponent').live('change', function () {
                $(this).blur();
                $li = $(this).closest('li');
                calculateTariffTotalPerClass($li);
            });

            function calculateTariffTotalPerClass($li) {
                $total = 0;
                $li.find('.txtComponent').each(function () {
                    $total += parseFloat($(this).val().split(",").join(""));
                });
                $li.find('.txtClass').val($total).trigger('changeValue');
            }

            function calculateBaseTariffTotal() {
                var total = 0;
                var base1 = parseFloat($('#<%=hdnBaseTariffComp1.ClientID %>').val());
                var base2 = parseFloat($('#<%=hdnBaseTariffComp2.ClientID %>').val());
                var base3 = parseFloat($('#<%=hdnBaseTariffComp3.ClientID %>').val());

                total = base1 + base2 + base3;
                $('#<%=txtBaseTariff.ClientID %>').val(total);
            }

            $('#<%=txtBaseTariff.ClientID %>').live('change', function () {
                var baseTariff = parseFloat($(this).val());
                $('#ulClass li').each(function () {
                    var margin = $(this).find('.hdnMarginPercentage').val();
                    var value = baseTariff * (100 + parseFloat(margin)) / 100;
                    $(this).find('.txtClass').val(value).change();
                });
                $('#<%=txtBaseTariffComp1.ClientID %>').val("0").change().trigger('changeValue');
                $('#<%=txtBaseTariffComp2.ClientID %>').val("0").change().trigger('changeValue');
                $('#<%=txtBaseTariffComp3.ClientID %>').val("0").change().trigger('changeValue');
            });

            $('#<%=txtBaseTariffComp1.ClientID %>').live('change', function () {
                var baseTariffComp1 = parseFloat($(this).val());
                $('#<%=hdnBaseTariffComp1.ClientID %>').val(baseTariffComp1);
                $('#ulClass li').each(function () {
                    var margin = $(this).find('.hdnMarginPercentage').val();
                    var value = baseTariffComp1 * (100 + parseFloat(margin)) / 100;

                    $(this).find('.txtComponent1').val(value).change();
                });
                calculateBaseTariffTotal();
            });

            $('#<%=txtBaseTariffComp2.ClientID %>').live('change', function () {
                var baseTariffComp2 = parseFloat($(this).val());
                $('#<%=hdnBaseTariffComp2.ClientID %>').val(baseTariffComp2);
                $('#ulClass li').each(function () {
                    var margin = $(this).find('.hdnMarginPercentage').val();
                    var value = baseTariffComp2 * (100 + parseFloat(margin)) / 100;

                    $(this).find('.txtComponent2').val(value).change();
                });
                calculateBaseTariffTotal();
            });

            $('#<%=txtBaseTariffComp3.ClientID %>').live('change', function () {
                var baseTariffComp3 = parseFloat($(this).val());
                $('#<%=hdnBaseTariffComp3.ClientID %>').val(baseTariffComp3);
                $('#ulClass li').each(function () {
                    var margin = $(this).find('.hdnMarginPercentage').val();
                    var value = baseTariffComp3 * (100 + parseFloat(margin)) / 100;

                    $(this).find('.txtComponent3').val(value).change();
                });
                calculateBaseTariffTotal();
            });

            $editedLi = null;
            $('.btnEditItemCost').live('click', function () {
                $li = $(this).closest('li');
                $editedLi = $li;
                $('#txtEditItem').val($('#<%=txtItemName.ClientID %>').val());
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

            $('#btnItemCostSave').live('click', function () {
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

            $('.txtItemCostCurrent').live('change', function () {
                $prev = parseFloat($(this).closest('tr').find('.txtItemCostPrev').attr('hiddenVal'));
                $current = parseFloat($(this).val());
                $total = $prev + $current;
                $(this).closest('tr').find('.txtItemCostTotal').val($total).trigger('changeValue');
            });
        });

        $('#<%=btnProcess.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                onCustomButtonClick('process');
            }
        });

        $('#lblTariffBookNo.lblLink').live('click', function () {
            openSearchDialog('tariffBook', '', function (value) {
                $('#<%=txtDocumentNo.ClientID %>').val(value);
                onTxtDocumentNoChanged(value);
            });
        });

        function onTxtDocumentNoChanged(value) {
            var filterExpression = "DocumentNo = '" + value + "'";
            Methods.getObject('GetvTariffBookHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnBookID.ClientID %>').val(result.BookID);
                    $('#<%=hdnGCTariffScheme.ClientID %>').val(result.GCTariffScheme);
                    $('#<%=txtTariffScheme.ClientID %>').val(result.TariffScheme);
                    $('#<%=hdnStartDate.ClientID %>').val(result.StartingDateInDatePicker);
                    $('#<%=txtStartDate.ClientID %>').val(result.StartingDateInString);

                    $('#<%=txtItemCode.ClientID %>').val('').change();
                }
                else {
                    $('#<%=hdnBookID.ClientID %>').val('');
                    $('#<%=hdnGCTariffScheme.ClientID %>').val('');
                    $('#<%=txtTariffScheme.ClientID %>').val('');
                    $('#<%=hdnStartDate.ClientID %>').val('');
                    $('#<%=txtStartDate.ClientID %>').val('');

                    $('#<%=txtItemCode.ClientID %>').val('').change();
                }
            });
        }

        $('#lblItemName.lblLink').live('click', function () {
            openSearchDialog('item', "IsDeleted=0 AND GCItemStatus != 'X181^999'", function (value) {
                $('#<%=txtItemCode.ClientID %>').val(value);
                onTxtItemNameChanged(value);
            });
        });

        $('#<%=txtItemCode.ClientID %>').live('change', function () {
            onTxtItemNameChanged($(this).val());
        });

        function onTxtItemNameChanged(value) {
            var filterExpression = "ItemCode = '" + value + "' AND IsDeleted=0 AND  GCItemStatus !='X181^999'";
            Methods.getObject('GetItemMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                    $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                    $('#<%=hdnGCItemType.ClientID %>').val(result.GCItemType);
                    var itemID = result.ItemID;
                    var bookID = $('#<%=hdnBookID.ClientID %>').val();

                    if (itemID != '') {
                        Methods.getSuggestedPrice(bookID, itemID, function (result) {
                            $('#<%=txtSuggestedTariff.ClientID %>').val(result.SuggestedTariff).trigger('changeValue');
                            $('#<%=txtBaseTariff.ClientID %>').val(result.BaseTariff).trigger('changeValue');
                            var baseTariff = parseFloat(result.BaseTariff);
                            $('#ulClass li').each(function () {
                                var margin = $(this).find('.hdnMarginPercentage').val();
                                var value = baseTariff * (100 + parseFloat(margin)) / 100;
                                $(this).find('.txtClass').val(value).change();

                                $(this).find('.hdnPrevBurden').val(result.ItemCost.PreviousBurden);
                                $(this).find('.hdnPrevLabor').val(result.ItemCost.PreviousLabor);
                                $(this).find('.hdnPrevMaterial').val(result.ItemCost.PreviousMaterial);
                                $(this).find('.hdnPrevOverhead').val(result.ItemCost.PreviousOverhead);
                                $(this).find('.hdnPrevSubContract').val(result.ItemCost.PreviousSubContract);

                                $(this).find('.hdnCurrentBurden').val(result.ItemCost.CurrentBurden);
                                $(this).find('.hdnCurrentLabor').val(result.ItemCost.CurrentLabor);
                                $(this).find('.hdnCurrentMaterial').val(result.ItemCost.CurrentMaterial);
                                $(this).find('.hdnCurrentOverhead').val(result.ItemCost.CurrentOverhead);
                                $(this).find('.hdnCurrentSubContract').val(result.ItemCost.CurrentSubContract);

                                $(this).find('.hdnTotalBurden').val(result.ItemCost.TotalBurden);
                                $(this).find('.hdnTotalLabor').val(result.ItemCost.TotalLabor);
                                $(this).find('.hdnTotalMaterial').val(result.ItemCost.TotalMaterial);
                                $(this).find('.hdnTotalOverhead').val(result.ItemCost.TotalOverhead);
                                $(this).find('.hdnTotalSubContract').val(result.ItemCost.TotalSubContract);
                            });

                            cbpClassTariff.PerformCallback('refresh');
                        });
                    }
                }
                else clearData();
            });
        }

        function clearData() {
            $('#<%=hdnItemID.ClientID %>').val('');
            $('#<%=txtItemCode.ClientID %>').val('');
            $('#<%=txtItemName.ClientID %>').val('');
            $('#<%=txtSuggestedTariff.ClientID %>').val('').trigger('changeValue');
            $('#<%=txtBaseTariff.ClientID %>').val('').trigger('changeValue');
            $('#ulClass li').each(function () {
                $(this).find('.txtClass').val('0').trigger('changeValue');
                $(this).find('.txtComponent1').val('0').trigger('changeValue');
                $(this).find('.txtComponent2').val('0').trigger('changeValue');
                $(this).find('.txtComponent3').val('0').trigger('changeValue');
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
        }

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Save Success', retval, clearData());
        }

        function onClassTariffCallback(s) {
            hideLoadingPanel();
        }
    </script>
    <style type="text/css">
        #ulClass
        {
            margin: 0;
            padding: 0;
        }
        #ulClass li
        {
            list-style-type: none;
            display: inline-block;
            width: 150px;
            margin-bottom: 10px;
        }
    </style>
    <input type="hidden" value="" id="hdnListClassID" runat="server" />
    <input type="hidden" value="" id="hdnListClassCode" runat="server" />
    <input type="hidden" value="" id="hdnListClassName" runat="server" />
    <input type="hidden" value="" id="hdnListClassMargin" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnBookID" runat="server" />
    <input type="hidden" value="" id="hdnStartDate" runat="server" />
    <input type="hidden" value="" id="hdnGCTariffScheme" runat="server" />
    <input type="hidden" value="" id="hdnGCItemType" runat="server" />
    <input type="hidden" value="0" id="hdnBaseTariffComp1" runat="server" />
    <input type="hidden" value="0" id="hdnBaseTariffComp2" runat="server" />
    <input type="hidden" value="0" id="hdnBaseTariffComp3" runat="server" />
    <div>
        <table style="width: 100%" class="tblEntryDetail">
            <colgroup>
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td valign="top">
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal lblLink" id="lblTariffBookNo">
                                    <%=GetLabel("No. Buku Tariff")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtDocumentNo" Width="150px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label2">
                                    <%=GetLabel("Skema Tariff")%></label>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtTariffScheme" Width="150px" runat="server" ReadOnly="true" />
                                        </td>
                                        <td class="tdLabel" style="padding: 5px;">
                                            <label class="lblNormal" id="lblStartDate">
                                                <%=GetLabel("Mulai Berlaku")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStartDate" Width="120px" runat="server" ReadOnly="true" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal lblLink" id="lblItemName">
                                    <%=GetLabel("Item")%></label>
                            </td>
                            <td colspan="2">
                                <input type="hidden" value="" id="hdnItemID" runat="server" />
                                <asp:TextBox ID="txtItemCode" Width="150px" runat="server" />
                                <asp:TextBox ID="txtItemName" Width="451px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal lblMandatory">
                                    <%=GetLabel("Alasan Ubah")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtRemarks" Width="610px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Suggested Tariff")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSuggestedTariff" ReadOnly="true" CssClass="txtCurrency" Width="150px"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Base Tariff")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBaseTariff" CssClass="txtCurrency" Width="150px" runat="server" />
                                <label class="lblNormal">
                                    Sarana</label>
                                <asp:TextBox ID="txtBaseTariffComp1" CssClass="txtCurrency" Width="150px" runat="server" />
                                <label class="lblNormal">
                                    Pelayanan</label>
                                <asp:TextBox ID="txtBaseTariffComp2" CssClass="txtCurrency" Width="150px" runat="server" />
                                <label class="lblNormal">
                                    Lain - lain</label>
                                <asp:TextBox ID="txtBaseTariffComp3" CssClass="txtCurrency" Width="150px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Class")%></label>
                                <div style="margin-top: 25px;" id="divComponentLabel" runat="server">
                                    <div style="height: 23px"></div>
                                    <div style="height: 23px">
                                        <%=GetTariffComponent1Text()%></div>
                                    <div style="height: 23px">
                                        <%=GetTariffComponent2Text()%></div>
                                    <div style="height: 23px">
                                        <%=GetTariffComponent3Text()%></div>
                                </div>
                            </td>
                            <td>
                                <dxcp:ASPxCallbackPanel ID="cbpClassTariff" runat="server" Width="100%" ClientInstanceName="cbpClassTariff"
                                    ShowLoadingPanel="false" OnCallback="cbpClassTariff_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onClassTariffCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainer">
                                                <asp:Repeater ID="rptClassCare" runat="server">
                                    <HeaderTemplate>
                                        <ul id="ulClass">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <div class="lblComponent" style="color: white">
                                                <%#: Eval("ClassName") %>
                                                (<span style="color: White"><%#: Eval("MarginPercentage1")%>%</span>)</div>
                                            <span>
                                                <asp:TextBox ID="txtClass" runat="server" Width="84%" CssClass="txtCurrency txtClass" Text='<%#: Eval("Tariff", "{0:N2}")%>' /></span>
                                            <span>
                                                <input type="button" class="btnEditItemCost" title='<%=GetLabel("Item Cost") %>'
                                                    value="..." style="width: 10%" /></span>
                                            <div style="margin-top: 5px;" id="divComponent" runat="server">
                                                <asp:TextBox ID="txtComponent1" runat="server" Width="84%" CssClass="txtCurrency txtComponent txtComponent1" Text='<%#: Eval("TariffComponent1", "{0:N2}")%>' />
                                                <asp:TextBox ID="txtComponent2" runat="server" Width="84%" CssClass="txtCurrency txtComponent txtComponent2" Text='<%#: Eval("TariffComponent2", "{0:N2}")%>' />
                                                <asp:TextBox ID="txtComponent3" runat="server" Width="84%" CssClass="txtCurrency txtComponent txtComponent3" Text='<%#: Eval("TariffComponent3", "{0:N2}")%>' />
                                            </div>
                                            <input type="hidden" id="hdnMarginPercentage" class="hdnMarginPercentage" value='<%#: Eval("MarginPercentage1")%>'
                                                runat="server" />
                                            <input type="hidden" id="hdnClassID" class="hdnClassID" value='<%#: Eval("ClassID")%>'
                                                runat="server" />
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
                                            </asp:Panel>
                                        </dx:PanelContent>
                                   </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <!-- Popup Item Cost -->
    <dxpc:ASPxPopupControl ID="pcItemCost" runat="server" ClientInstanceName="pcItemCost"
        CloseAction="CloseButton" Height="300px" HeaderText="Item Costing" Width="600px"
        Modal="True" PopupAction="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dxpc:PopupControlContentControl runat="server" ID="pccc1">
                <dx:ASPxPanel ID="pnlItemCost" runat="server" Width="100%">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <fieldset id="fsItemCost" style="margin: 0">
                                <div style="text-align: left; width: 100%;">
                                    <table>
                                        <colgroup>
                                            <col style="width: 500px" />
                                        </colgroup>
                                        <tr>
                                            <td valign="top">
                                                <table>
                                                    <colgroup>
                                                        <col style="width: 160px" />
                                                        <col style="width: 120px" />
                                                        <col style="width: 120px" />
                                                        <col style="width: 120px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td class="tdLabel">
                                                            <label class="lblNormal">
                                                                <%=GetLabel("Item")%></label>
                                                        </td>
                                                        <td colspan="3">
                                                            <input id="txtEditItem" readonly="readonly" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel">
                                                            <label class="lblNormal">
                                                                <%=GetLabel("Class")%></label>
                                                        </td>
                                                        <td colspan="2">
                                                            <input id="txtEditClassName" readonly="readonly" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td align="center">
                                                            <div class="lblComponent">
                                                                <%=GetLabel("Previous")%></div>
                                                        </td>
                                                        <td align="center">
                                                            <div class="lblComponent">
                                                                <%=GetLabel("Current")%></div>
                                                        </td>
                                                        <td align="center">
                                                            <div class="lblComponent">
                                                                <%=GetLabel("Total")%></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel">
                                                            <label class="lblNormal">
                                                                <%=GetLabel("Material")%></label>
                                                        </td>
                                                        <td>
                                                            <input id="txtMaterialPrev" class="txtCurrency txtItemCostPrev" readonly="readonly"
                                                                style="width: 100%" />
                                                        </td>
                                                        <td>
                                                            <input id="txtMaterialCurrent" class="txtCurrency required txtItemCostCurrent" style="width: 100%" />
                                                        </td>
                                                        <td>
                                                            <input id="txtMaterialTotal" class="txtCurrency txtItemCostTotal" readonly="readonly"
                                                                style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel">
                                                            <label class="lblNormal">
                                                                <%=GetLabel("Labor")%></label>
                                                        </td>
                                                        <td>
                                                            <input id="txtLaborPrev" class="txtCurrency txtItemCostPrev" readonly="readonly"
                                                                style="width: 100%" />
                                                        </td>
                                                        <td>
                                                            <input id="txtLaborCurrent" class="txtCurrency required txtItemCostCurrent" style="width: 100%" />
                                                        </td>
                                                        <td>
                                                            <input id="txtLaborTotal" class="txtCurrency txtItemCostTotal" readonly="readonly"
                                                                style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel">
                                                            <label class="lblNormal">
                                                                <%=GetLabel("Overhead")%></label>
                                                        </td>
                                                        <td>
                                                            <input id="txtOverheadPrev" class="txtCurrency txtItemCostPrev" readonly="readonly"
                                                                style="width: 100%" />
                                                        </td>
                                                        <td>
                                                            <input id="txtOverheadCurrent" class="txtCurrency required txtItemCostCurrent" style="width: 100%" />
                                                        </td>
                                                        <td>
                                                            <input id="txtOverheadTotal" class="txtCurrency txtItemCostTotal" readonly="readonly"
                                                                style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel">
                                                            <label class="lblNormal">
                                                                <%=GetLabel("Sub Contract")%></label>
                                                        </td>
                                                        <td>
                                                            <input id="txtSubContractPrev" class="txtCurrency txtItemCostPrev" readonly="readonly"
                                                                style="width: 100%" />
                                                        </td>
                                                        <td>
                                                            <input id="txtSubContractCurrent" class="txtCurrency required txtItemCostCurrent"
                                                                style="width: 100%" />
                                                        </td>
                                                        <td>
                                                            <input id="txtSubContractTotal" class="txtCurrency txtItemCostTotal" readonly="readonly"
                                                                style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel">
                                                            <label class="lblNormal">
                                                                <%=GetLabel("Burden")%></label>
                                                        </td>
                                                        <td>
                                                            <input id="txtBurdenPrev" class="txtCurrency txtItemCostPrev" readonly="readonly"
                                                                style="width: 100%" />
                                                        </td>
                                                        <td>
                                                            <input id="txtBurdenCurrent" class="txtCurrency required txtItemCostCurrent" style="width: 100%" />
                                                        </td>
                                                        <td>
                                                            <input id="txtBurdenTotal" class="txtCurrency txtItemCostTotal" readonly="readonly"
                                                                style="width: 100%" />
                                                        </td>
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
