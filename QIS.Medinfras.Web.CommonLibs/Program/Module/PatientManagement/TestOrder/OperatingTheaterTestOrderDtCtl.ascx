<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperatingTheaterTestOrderDtCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.OperatingTheaterTestOrderDtCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_testordertemplatesctl">
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
            cbpViewPopup.PerformCallback('refresh');
        }
    });

    $('#tblSelectedItem .txtQty').live('change', function () {
        $row = $(this).closest('tr');
        totalPayer = 0;
        totalPatient = 0;
        grandTotal = 0;
        $('#tblSelectedItem tr').each(function () {
            if ($(this).find('.keyField').val() != undefined) {
                calculateTariffEstimation($(this));
            }
        });
    });

    $(function () {
        hideLoadingPanel();
        addItemFilterRow();
    });

    function onBeforeSaveRecord(errMessage) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMemberItemID.ClientID %>').val() != '')
            return true;
        else {
            errMessage.text = 'Please Select Item First';
            return false;
        }
    }

    function getCheckedMember() {
        var lstSelectedMemberItemID = [];
        var lstSelectedMemberItemTariff = [];
        var lstSelectedMemberIsControlItem = [];
        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {
            var itemID = $(this).find('.keyField').val();
            var isControlItem = $(this).find('.IsControlItem').val();
            var tariff = parseFloat($(this).find('.ItemEndTariff').val().replace(/[^0-9-.]/g, '')); // 12345.99;

            lstSelectedMemberItemID.push(itemID);
            lstSelectedMemberIsControlItem.push(isControlItem);
            lstSelectedMemberItemTariff.push(tariff);
        });
        $('#<%=hdnSelectedMemberItemID.ClientID %>').val(lstSelectedMemberItemID.join(','));
        $('#<%=hdnSelectedMemberIsControlItem.ClientID %>').val(lstSelectedMemberIsControlItem.join(','));
        $('#<%=hdnSelectedMemberItemTariff.ClientID %>').val(lstSelectedMemberItemTariff.join(','));
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        setDatePicker('<%=txtRealizationDate.ClientID %>');
        $('#<%=txtRealizationDate.ClientID %>').datepicker('option', 'maxDate', '0');

        setPaging($("#pagingPopUp"), pageCount, function (page) {
            getCheckedMember();
            cbpViewPopup.PerformCallback('changepage|' + page);
        });

        //#region Procedure Panel
        $('#lblProcedurePanel.lblLink').click(function () {
            var filterExpression = "ProcedurePanelID IN (SELECT ProcedurePanelID FROM ProcedureGroupPanel WHERE ProcedureGroupID = " + $('#<%=hdnProcedureGroupID.ClientID %>').val() + ") AND IsDeleted = 0";
            openSearchDialog('procedurepanelhd', filterExpression, function (value) {
                $('#<%=txtProcedurePanelCode.ClientID %>').val(value);
                onTxtProcedurePanelCodeChange(value);
            });
        });

        $('#<%=txtProcedurePanelCode.ClientID %>').change(function () {
            onTxtProcedurePanelCodeChange($(this).val());
        });

        function onTxtProcedurePanelCodeChange(value) {
            var filterExpression = "ProcedurePanelCode = '" + value + "'";
            Methods.getObject('GetProcedurePanelHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnProcedurePanelID.ClientID %>').val(result.ProcedurePanelID);
                    $('#<%=txtProcedurePanelName.ClientID %>').val(result.ProcedurePanelName);
                }
                else {
                    $('#<%=hdnProcedurePanelID.ClientID %>').val('');
                    $('#<%=txtProcedurePanelCode.ClientID %>').val('');
                    $('#<%=txtProcedurePanelName.ClientID %>').val('');
                }
                getCheckedMember();
                cbpViewPopup.PerformCallback('refresh');
            });
        }
        //#endregion

        //#region Physician
        $('#lblPhysicianOT.lblLink').click(function () {
            var filterParamedic = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "') AND IsDeleted = 0";
            openSearchDialog('paramedic', filterParamedic, function (value) {
                $('#<%=txtPhysicianCodeOT.ClientID %>').val(value);
                onTxtPhysicianCodeOTChange(value);
            });
        });

        $('#<%=txtPhysicianCodeOT.ClientID %>').change(function () {
            onTxtPhysicianCodeOTChange($(this).val());
        });

        function onTxtPhysicianCodeOTChange(value) {
            var filterExpression = "ParamedicCode = '" + value + "'";
            Methods.getObject('GetParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPhysicianIDOT.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianNameOT.ClientID %>').val(result.FullName);
                }
                else {
                    $('#<%=hdnPhysicianIDOT.ClientID %>').val('');
                    $('#<%=txtPhysicianCodeOT.ClientID %>').val('');
                    $('#<%=txtPhysicianNameOT.ClientID %>').val('');
                }
                getCheckedMember();
                cbpViewPopup.PerformCallback('refresh');
            });
        }
        //#endregion
    });

    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopUp"), pageCount, function (page) {
                getCheckedMember();
                cbpViewPopup.PerformCallback('changepage|' + page);
            });
        }
        else { }
        addItemFilterRow();
    }
    //#endregion

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');

            $newTr = $('#tmplSelectedTestItem').html();
            $newTr = $newTr.replace(/\$\{ItemID}/g, $selectedTr.find('.keyField').html());
            $newTr = $newTr.replace(/\$\{IsControlItem}/g, $selectedTr.find('.IsControlItem').html());
            $newTr = $newTr.replace(/\$\{ItemName1}/g, $selectedTr.find('.tdItemName1').html());
            $newTr = $newTr.replace(/\$\{ItemTariff}/g, $selectedTr.find('span[id$="txtItemTariff"]').text());
            $newTr = $newTr.replace(/\$\{FormulaPercent}/g, $selectedTr.find('span[id$="txtFormulaPercent"]').text());
            $newTr = $newTr.replace(/\$\{ItemEndTariff}/g, $selectedTr.find('span[id$="txtItemEndTariff"]').text());
            $newTr = $($newTr);
            $newTr.insertAfter($('#trHeader2'));
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
                var lstSelectedMember = $('#<%=hdnSelectedMemberItemID.ClientID %>').val().split(',');
                lstSelectedMember.splice(lstSelectedMember.indexOf(id), 1);
                $('#<%=hdnSelectedMemberItemID.ClientID %>').val(lstSelectedMember.join(','));
            }
            $(this).closest('tr').remove();
        }
    });
</script>
<div style="padding: 10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${ItemID}' />
                <input type="hidden" class="IsControlItem" value='${IsControlItem}' />
                <input type="hidden" class="ItemEndTariff" value='${ItemEndTariff}' />
            </td>
            <td align="left">${ItemName1}</td>
            <td align="right">${ItemTariff}</td>
            <td align="right">${FormulaPercent}</td>
            <td align="right">${ItemEndTariff}</td>
        </tr>
    </script>
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnSelectedMemberItemID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberItemTariff" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberIsControlItem" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderID" runat="server" value="" />
    <input type="hidden" id="hdnProcedureGroupID" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" id="hdnRegistrationID" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnChargesClassID" runat="server" />
    <input type="hidden" id="hdnTransactionID" runat="server" />
    <table>
        <colgroup>
            <col style="width: 190px" />
            <col style="width: 150px" />
            <col style="width: 150px" />
            <col style="width: 120px" />
            <col style="width: 60px" />
            <col style="width: 150px" />
            <col style="width: 120px" />
            <col style="width: 60px" />
            <col />
        </colgroup>
        <tr id="trTransactionNo" runat="server">
            <td class="tdLabel">
                <label class="lblNormal" id="lblTestOrderTransactionNo">
                    <%=GetLabel("No. Order")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtTransactionNo" ReadOnly="true" Width="100%" runat="server" />
            </td>
            <td class="tdLabel" style="padding-left:10px">
                <%=GetLabel("Tanggal/Jam Order") %>
            </td>
            <td>
                <asp:TextBox ID="txtTestOrderDate" Width="120px" runat="server" Style="text-align: center"
                    ReadOnly="true" />
            </td>
            <td>
                <asp:TextBox ID="txtTestOrderTime" Width="60px" CssClass="time" runat="server" Style="text-align: center"
                    ReadOnly="true" />
            </td>
            <td class="tdLabel" style="padding-left:10px">
                <%=GetLabel("Tanggal/Jam Rencana") %>
            </td>
            <td>
                <asp:TextBox ID="txtScheduledDate" Width="120px" runat="server" Style="text-align: center"
                    ReadOnly="true" />
            </td>
            <td>
                <asp:TextBox ID="txtScheduledTime" Width="60px" CssClass="time" runat="server" Style="text-align: center"
                    ReadOnly="true" />
            </td>
        </tr>
        <tr id="trDiorderOleh" runat="server">
            <td class="tdLabel">
                <label class="lblNormal" id="lblUserOrder">
                    <%=GetLabel("Diorder Oleh")%></label>
            </td>
            <td colspan="7">
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 300px" />
                        <col style="width: 100px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtOrderUser" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                        <td class="tdLabel" style="padding-left:10px">
                            <label class="lblNormal">
                                <%=GetLabel("Dokter")%></label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtOrderPhysicianName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Catatan")%></label>
            </td>
            <td colspan="7">
                <asp:TextBox ID="txtNotes" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblProcedurePanel">
                    <%=GetLabel("Panel Tindakan")%></label>
            </td>
            <td colspan="4">
                <input type="hidden" id="hdnProcedurePanelID" value="0" runat="server" />
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 350px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtProcedurePanelCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtProcedurePanelName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trTransactionDateTime" runat="server">
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Tanggal/Jam Realisasi") %></label>
            </td>
            <td>
                <asp:TextBox ID="txtRealizationDate" Width="120px" runat="server" CssClass="datepicker"
                    Style="text-align: center" />
            </td>
            <td>
                <asp:TextBox ID="txtRealizationTime" Width="60px" CssClass="time" runat="server"
                    Style="text-align: center" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblPhysicianOT">
                    <%=GetLabel("Dokter/Paramedis Pelaksana")%></label>
            </td>
            <td colspan="4">
                <input type="hidden" id="hdnPhysicianIDOT" value="0" runat="server" />
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 350px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPhysicianCodeOT" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhysicianNameOT" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4>
                    <%=GetLabel("Available item :")%></h4>
                <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                    ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="IsControlItem" HeaderStyle-CssClass="IsControlItem hiddenColumn"
                                            ItemStyle-CssClass="IsControlItem hiddenColumn" />
                                        <asp:BoundField DataField="ItemName1" HeaderText="Item Name" ItemStyle-CssClass="tdItemName1" />
                                        <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderText="Tariff">
                                            <ItemTemplate>
                                                <asp:Label ID="txtItemTariff" runat="server" Enabled="False" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderText="Formula(%)">
                                            <ItemTemplate>
                                                <asp:Label ID="txtFormulaPercent" runat="server" Enabled="False" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderText="Tariff">
                                            <ItemTemplate>
                                                <asp:Label ID="txtItemEndTariff" runat="server" Enabled="False" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
                        <div id="pagingPopUp">
                        </div>
                    </div>
                </div>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <h4>
                    <%=GetLabel("Selected item(s) :")%></h4>
                <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all">
                    <tr id="trHeader2">
                        <th style="width: 20px">
                            &nbsp;
                        </th>
                        <th align="left">
                            <%=GetLabel("Item Name")%>
                        </th>
                        <th align="right" style="width: 100px">
                            <%=GetLabel("Tariff")%>
                        </th>
                        <th align="right" style="width: 80px">
                            <%=GetLabel("Formula(%)")%>
                        </th>
                        <th align="right" style="width: 100px">
                            <%=GetLabel("End Tariff")%>
                        </th>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div class="imgLoadingGrdView" id="containerImgLoadingView">
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
</div>
