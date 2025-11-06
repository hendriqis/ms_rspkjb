<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JournalTemplateCtlX.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.JournalTemplateCtlX" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_JournalTemplateCtlX">
    //#region Template No
    function onGetJournalTemplateFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblTemplate.lblLink').live('click', function () {
        openSearchDialog('journaltemplatehd', onGetJournalTemplateFilterExpression(), function (value) {
            $('#<%=txtTemplateCode.ClientID %>').val(value);
            onTxtTemplateCodeChanged(value);
        });
    });

    $('#<%=txtTemplateCode.ClientID %>').live('change', function () {
        onTxtTemplateCodeChanged($(this).val());
    });

    function onTxtTemplateCodeChanged(value) {
        var filterExpression = onGetJournalTemplateFilterExpression() + " AND TemplateCode = '" + value + "'";
        Methods.getObject('GetvJournalTemplateHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnTemplateID.ClientID %>').val(result.TemplateID);
                $('#<%=txtTemplateName.ClientID %>').val(result.TemplateName);
                $('#<%=hdnGCJournalTemplateType.ClientID %>').val(result.GCJournalTemplateType);
                cboJournalTemplateType.SetValue(result.GCJournalTemplateType);
                oncboJournalTemplateTypeChanged();
                $('#<%=txtRemarks.ClientID %>').val(result.Remarks);
            } else {
                $('#<%=hdnTemplateID.ClientID %>').val('0');
                $('#<%=txtTemplateCode.ClientID %>').val('');
                $('#<%=txtTemplateName.ClientID %>').val('');
                $('#<%=hdnGCJournalTemplateType.ClientID %>').val('');
                cboJournalTemplateType.SetValue('');
                $('#<%=txtRemarks.ClientID %>').val('');

                $('#<%=txtTransactionAmount.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtTransactionAmount.ClientID %>').val("0").trigger('changeValue');
                $('#btnCalculate').removeAttr('style');
            }
        });

        cbpEntryPopupView.PerformCallback('refresh');
    }
    //#endregion

    $('.txtCurrency').each(function () {
        $(this).trigger('changeValue');
    });

    function oncboJournalTemplateTypeChanged() {
        if (cboJournalTemplateType.GetValue() == Constant.JournalTemplateType.ALOKASI) {
            $('#<%=txtTransactionAmount.ClientID %>').removeAttr('readonly');
            $('#<%=txtTransactionAmount.ClientID %>').val("0").trigger('changeValue');
            $('#<%=btnCalculate.ClientID %>').removeAttr('style');

            $('#<%=grdViewD.ClientID%>').each(function () {
                $tr = $(this).closest('tr');
                $tr.find('.txtAmountD').attr('readonly', 'readonly');
            });

            $('#<%=grdViewK.ClientID%>').each(function () {
                $tr = $(this).closest('tr');
                $tr.find('.txtAmountK').attr('readonly', 'readonly');
            });

        } else {
            $('#<%=txtTransactionAmount.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtTransactionAmount.ClientID %>').val("0").trigger('changeValue');
            $('#<%=btnCalculate.ClientID %>').attr('style', 'display:none');

            $('#<%=grdViewD.ClientID%>').each(function () {
                $tr = $(this).closest('tr');
                $tr.find('.txtAmountD').removeAttr('readonly');
            });

            $('#<%=grdViewK.ClientID%>').each(function () {
                $tr = $(this).closest('tr');
                $tr.find('.txtAmountK').removeAttr('readonly');
            });
        }
    }

    $('#<%=btnCalculate.ClientID %>').live('click', function () {
        if (cboJournalTemplateType.GetValue() == Constant.JournalTemplateType.ALOKASI) {
            var transactionAmount = parseFloat($('#<%=txtTransactionAmount.ClientID %>').attr('hiddenVal'));

            $('#<%=grdViewD.ClientID%>').each(function () {
                $tr = $(this).closest('tr');
                var AmountPercentageD = $tr.find('.AmountPercentageD').val();
                var AmountD = $tr.find('.AmountD').val();

                var fixAmountD = transactionAmount * AmountPercentageD / 100;
                $tr.find('.txtAmountD').val(fixAmountD).trigger('changeValue');
            });

            $('#<%=grdViewK.ClientID%>').each(function () {
                $tr = $(this).closest('tr');
                var AmountPercentageK = $tr.find('.AmountPercentageK').val();
                var AmountK = $tr.find('.AmountK').val();

                var fixAmountK = transactionAmount * AmountPercentageK / 100;
                $tr.find('.txtAmountK').val(fixAmountK).trigger('changeValue');
            });
        }
    });

    function getDetailDataInput() {
        var valueD = "", valueK = "";

        $('#<%=grdViewD.ClientID%>').each(function () {
            $tr = $(this).closest('tr');
            valueD += "|";
            valueD += $tr.find('.GLAccountID').val() + ";";
            valueD += $tr.find('.txtAmountD').val();
        });

        $('#<%=grdViewK.ClientID%>').each(function () {
            $tr = $(this).closest('tr');
            valueK += "|";
            valueK += $tr.find('.GLAccountID').val() + ";";
            valueK += $tr.find('.txtAmountK').val();
        });

        $('#<%= hdnInsertedAmountD.ClientID%>').val(valueD);
        $('#<%= hdnInsertedAmountK.ClientID%>').val(valueK);

        alert($('#<%= hdnInsertedAmountD.ClientID%>').val());
        alert($('#<%= hdnInsertedAmountK.ClientID%>').val());
    }

    function onBeforeSaveRecord(param) {
        getDetailDataInput();
        return false;
    }
</script>
<input type="hidden" id="hdnTemplateID" runat="server" />
<input type="hidden" id="hdnGCJournalTemplateType" runat="server" />
<input type="hidden" id="hdnGLTransactionID" runat="server" />
<input type="hidden" id="hdnJournalDate" runat="server" />
<input type="hidden" id="hdnGCJournalGroup" runat="server" />
<input type="hidden" id="hdnInsertedAmountD" runat="server" />
<input type="hidden" id="hdnInsertedAmountK" runat="server" />
<div style="height: 440px; overflow-y: auto; overflow-x: hidden">
    <table style="width: 100%" class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal lblLink" id="lblTemplate">
                                <%=GetLabel("Template") %></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 120px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtTemplateCode" Width="100%" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtTemplateName" ReadOnly="true" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Keterangan")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtRemarks" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tipe")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboJournalTemplateType" ClientInstanceName="cboJournalTemplateType"
                                Width="150px" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ oncboJournalTemplateTypeChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nilai Transaksi") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="txtCurrency" Width="150px" ID="txtTransactionAmount" />
                            <input type="button" id="btnCalculate" value="Hitung Nilai Alokasi" runat="server" />
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 50%" valign="top">
                                            <h4 style="text-align: center">
                                                <%=GetLabel("DEBET") %></h4>
                                            <asp:GridView ID="grdViewD" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                                ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="COA" HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <div style="font-size: 14px;">
                                                                <%#:Eval("GLAccountNo") %></div>
                                                            <div style="font-size: 10px;">
                                                                <%#:Eval("GLAccountName") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Segment">
                                                        <ItemTemplate>
                                                            <div style="font-size: 10px;">
                                                                DP:
                                                                <%#:Eval("DepartmentID") %></div>
                                                            <div style="font-size: 10px;">
                                                                SU:
                                                                <%#:Eval("ServiceUnitName") %></div>
                                                            <div style="font-size: 10px;">
                                                                RC:
                                                                <%#:Eval("RevenueCostCenterName") %></div>
                                                            <div style="font-size: 10px;">
                                                                CG:
                                                                <%#:Eval("CustomerGroupName") %></div>
                                                            <div style="font-size: 10px;">
                                                                BP:
                                                                <%#:Eval("BusinessPartnerName") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="AmountPercentage" HeaderText="Bagian (%)" HeaderStyle-HorizontalAlign="Right"
                                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px" />
                                                    <asp:BoundField DataField="Amount" DataFormatString="{0:n2}" HeaderText="Bagian (Rp)"
                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
                                                    <asp:TemplateField HeaderText="Jumlah" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <input type="hidden" value='<%#:Eval("AmountPercentage") %>' class="AmountPercentageD"
                                                                id="AmountPercentageD" runat="server" />
                                                            <input type="hidden" value='<%#:Eval("Amount") %>' class="AmountD" id="AmountD" runat="server" />
                                                            <input type="hidden" value='<%#:Eval("GLAccountID") %>' class="GLAccountID" id="GLAccountID" runat="server" />
                                                            <input id="txtAmountD" runat="server" type="text" amountpercentage='<%#:Eval("AmountPercentage") %>'
                                                                class="txtAmountD txtCurrency" style="width: 100%" readonly="readonly" value='<%#:Eval("Amount") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("No Data To Display")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                        <td style="width: 50%" valign="top">
                                            <h4 style="text-align: center">
                                                <%=GetLabel("KREDIT") %></h4>
                                            <asp:GridView ID="grdViewK" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                                ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="COA" HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <div style="font-size: 14px;">
                                                                <%#:Eval("GLAccountNo") %></div>
                                                            <div style="font-size: 10px;">
                                                                <%#:Eval("GLAccountName") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Segment">
                                                        <ItemTemplate>
                                                            <div style="font-size: 10px;">
                                                                DP:
                                                                <%#:Eval("DepartmentID") %></div>
                                                            <div style="font-size: 10px;">
                                                                SU:
                                                                <%#:Eval("ServiceUnitName") %></div>
                                                            <div style="font-size: 10px;">
                                                                RC:
                                                                <%#:Eval("RevenueCostCenterName") %></div>
                                                            <div style="font-size: 10px;">
                                                                CG:
                                                                <%#:Eval("CustomerGroupName") %></div>
                                                            <div style="font-size: 10px;">
                                                                BP:
                                                                <%#:Eval("BusinessPartnerName") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="AmountPercentage" HeaderText="Bagian (%)" HeaderStyle-HorizontalAlign="Right"
                                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px" />
                                                    <asp:BoundField DataField="Amount" DataFormatString="{0:n2}" HeaderText="Bagian (Rp)"
                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
                                                    <asp:TemplateField HeaderText="Jumlah" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <input type="hidden" value='<%#:Eval("AmountPercentage") %>' class="AmountPercentageK"
                                                                id="AmountPercentageK" runat="server" />
                                                            <input type="hidden" value='<%#:Eval("Amount") %>' class="AmountK" id="AmountK" runat="server" />
                                                            <input type="hidden" value='<%#:Eval("GLAccountID") %>' class="GLAccountID" id="GLAccountID" runat="server" />
                                                            <input id="txtAmountK" runat="server" type="text" amountpercentage='<%#:Eval("AmountPercentage") %>'
                                                                class="txtAmountK txtCurrency" style="width: 100%" readonly="readonly" value='<%#:Eval("Amount") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("No Data To Display")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>
