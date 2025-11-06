<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JournalTemplateCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.JournalTemplateCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_JournalTemplateCtl">

    $('.txtCurrency').each(function () {
        $(this).trigger('changeValue');
    });

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

    function oncboJournalTemplateTypeChanged() {
        if (cboJournalTemplateType.GetValue() == Constant.JournalTemplateType.ALOKASI) {
            $('#<%=txtTransactionAmount.ClientID %>').removeAttr('readonly');
            $('#<%=txtTransactionAmount.ClientID %>').val("0").trigger('changeValue');
            $('#<%=btnCalculate.ClientID %>').removeAttr('style');

            $('.chkCOA input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    $tr.find('.txtAmount').attr('readonly', 'readonly');
                }
            });

        } else {
            $('#<%=txtTransactionAmount.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtTransactionAmount.ClientID %>').val("0").trigger('changeValue');
            $('#<%=btnCalculate.ClientID %>').attr('style', 'display:none');

            $('.chkCOA input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    $tr.find('.txtAmount').removeAttr('readonly');
                }
            });
        }
    }

    $('#<%=btnCalculate.ClientID %>').live('click', function () {
        if (cboJournalTemplateType.GetValue() == Constant.JournalTemplateType.ALOKASI) {
            var transactionAmount = parseFloat($('#<%=txtTransactionAmount.ClientID %>').attr('hiddenVal'));

            $('.chkCOA input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var AmountPercentage = $tr.find('.AmountPercentage').val();

                    var fixAmount = transactionAmount * AmountPercentage / 100;
                    $tr.find('.txtAmount').val(fixAmount).trigger('changeValue');
                }
            });
        }
    });

    function getDetailDataInput() {
        var lstSelectedID = $('#<%=hdnSelectedID.ClientID %>').val().split('|');
        var lstSelectedCOA = $('#<%=hdnSelectedCOA.ClientID %>').val().split('|');
        var lstSelectedValue = $('#<%=hdnSelectedValue.ClientID %>').val().split('|');
        var lstSelectedRemarks = $('#<%=hdnSelectedRemarks.ClientID %>').val().split('|');
        $('.chkCOA input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var ID = $tr.find('.ID').val();
                var GLAccountID = $tr.find('.GLAccountID').val();
                var txtAmount = $tr.find('.txtAmount').val();
                var txtRemarksDt = $tr.find('.txtRemarksDt').val();
                var idx = lstSelectedID.indexOf(ID);
                if (idx < 0) {
                    lstSelectedID.push(ID);
                    lstSelectedCOA.push(GLAccountID);
                    lstSelectedValue.push(txtAmount);
                    lstSelectedRemarks.push(txtRemarksDt);
                }
                else {
                    lstSelectedCOA[idx] = GLAccountID;
                    lstSelectedValue[idx] = txtAmount;
                    lstSelectedRemarks[idx] = txtRemarksDt;
                }
            }
            else {
                var $tr = $(this).closest('tr');
                var ID = $tr.find('.ID').val();

                var idx = lstSelectedID.indexOf(ID);
                if (idx > -1) {
                    lstSelectedID.splice(idx, 1);
                    lstSelectedCOA.splice(idx, 1);
                    lstSelectedValue.splice(idx, 1);
                    lstSelectedRemarks.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedID.ClientID %>').val(lstSelectedID.join('|'));
        $('#<%=hdnSelectedCOA.ClientID %>').val(lstSelectedCOA.join('|'));
        $('#<%=hdnSelectedValue.ClientID %>').val(lstSelectedValue.join('|'));
        $('#<%=hdnSelectedRemarks.ClientID %>').val(lstSelectedRemarks.join('|'));
    }

    function onBeforeSaveRecord(param) {
        getDetailDataInput();
        var paramInput = $('#<%=hdnSelectedID.ClientID %>').val();
        if (paramInput != "") {
            return true;
        } else {
            return false;
        }
    }
</script>
<input type="hidden" id="hdnTemplateID" runat="server" />
<input type="hidden" id="hdnGCJournalTemplateType" runat="server" />
<input type="hidden" id="hdnGLTransactionIDTemplateCtl" runat="server" />
<input type="hidden" id="hdnJournalDate" runat="server" />
<input type="hidden" id="hdnGCJournalGroup" runat="server" />
<input type="hidden" id="hdnSelectedID" runat="server" />
<input type="hidden" id="hdnSelectedCOA" runat="server" />
<input type="hidden" id="hdnSelectedValue" runat="server" />
<input type="hidden" id="hdnSelectedRemarks" runat="server" />
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
                                        <td style="width: 100%" valign="top">
                                            <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                                ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <input type="hidden" value='<%#:Eval("ID") %>' class="ID" id="ID" runat="server" />
                                                            <input type="hidden" value='<%#:Eval("AmountPercentage") %>' class="AmountPercentage"
                                                                id="AmountPercentage" runat="server" />
                                                            <input type="hidden" value='<%#:Eval("Amount") %>' class="Amount" id="Amount" runat="server" />
                                                            <input type="hidden" value='<%#:Eval("GLAccountID") %>' class="GLAccountID" id="GLAccountID"
                                                                runat="server" />
                                                            <asp:CheckBox ID="chkCOA" runat="server" CssClass="chkCOA" Checked="true" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DisplayOrder" HeaderText="Urutan" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                                                    <asp:TemplateField HeaderText="COA">
                                                        <ItemTemplate>
                                                            <div style="font-size: 14px;">
                                                                <%#:Eval("GLAccountNo") %></div>
                                                            <div style="font-size: 10px;">
                                                                <%#:Eval("GLAccountName") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Segment" HeaderStyle-Width="150px">
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
                                                    <asp:BoundField DataField="Position" HeaderText="Posisi" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                                                    <asp:BoundField DataField="AmountPercentage" HeaderText="Bagian (%)" HeaderStyle-HorizontalAlign="Right"
                                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" />
                                                    <asp:BoundField DataField="Amount" DataFormatString="{0:N2}" HeaderText="Bagian (Rp)"
                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="150px" />
                                                    <asp:TemplateField HeaderText="Jumlah" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <input id="txtAmount" runat="server" type="text" class="txtAmount txtCurrency" style="width: 100%"
                                                                readonly="readonly" value='<%#:Eval("cfAmountInString") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Keterangan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-Width="170px">
                                                        <ItemTemplate>
                                                            <input id="txtRemarksDt" class="txtRemarksDt" runat="server" type="text" style="width: 100%" />
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
