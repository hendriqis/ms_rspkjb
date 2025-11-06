<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestOrderLabQuickPicksCtl2.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.TestOrderLabQuickPicksCtl2" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_TestOrderLabQuickPicksCtl1">
    $(function () {
        hideLoadingPanel();
    });

    function onCboServiceUnitValueChanged() {
        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(cboServiceUnit.GetValue());
    }

    function getCheckedMember() {
        var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
        var result = '';
        $('.chkIsSelected input:checked').each(function () {
            var key = $(this).closest('.divContainerLabItem').find('.hdnItemID').val();
            lstSelectedMember.push(key);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
    }

    function onBeforeSaveRecordEntryPopup() {
        getCheckedMember();

        var lstItem = $('#<%=hdnSelectedMember.ClientID %>').val();
        var lstItemSplit = lstItem.split(',');
        var lstItem1 = [];
        var lstItem2 = [];
        for (var i = 0; i < lstItemSplit.length; i++) {
            var countData = 0;
            for (var x = 0; x < lstItemSplit.length; x++) {
                if (lstItemSplit[i] == lstItemSplit[x]) {
                    countData += 1;
                }
            }

            if (countData > 1) {
                var ishasData = 0;
                for (var x = 0; x < lstItem1.length; x++) {
                    if (lstItem1[x] == lstItemSplit[i]) {
                        ishasData = 1;
                    }
                }

                if (ishasData == 0) {
                    lstItem1.push(lstItemSplit[i]);
                }
            }
            else {
                lstItem2.push(lstItemSplit[i]);
            }
        }
        $('#<%=hdnSelectedMember.ClientID %>').val(lstItem1.concat(lstItem2).join(','));

        if ($('#<%=hdnSelectedMember.ClientID %>').val() != '' && $('#<%=txtRemarks.ClientID %>').val() != '') {
                return true;
        }
        else {
            if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                displayErrorMessageBox("Order Pemeriksaan", "Belum ada item pemeriksaan yang dipilih");
                return false;
            }
            else if ($('#<%=txtRemarks.ClientID %>').val() == '') {
                displayErrorMessageBox("Order Pemeriksaan", "Catatan Klinis harus disertakan untuk kebutuhan akreditasi");
                return false;            
            }
        }
    }

    $('#lnkDetailItem').die('click');
    $('#lnkDetailItem').live('click', function () {
        var msg = $(this).parent().find('.preconditionNotes').val();
        displayMessageBox('Catatan Sebelum Pemeriksaan Dilakukan', msg);
    });

    function onAfterSaveRecordPatientPageEntry(value) {
        if ($('#<%=hdnGCItemType.ClientID %>').val() == 'X001^004') {
            if (typeof onRefreshLaboratoryGrid == 'function')
                onRefreshLaboratoryGrid();
            if (value != "") {
                var param = value.split("|");
                if (typeof onAfterAddLabTestOrder == 'function')
                    onAfterAddLabTestOrder(param[1]);

                if (typeof onAfterSaveRecord == 'function')
                    onAfterSaveRecord(value);
            }
        }
        else {
            if (typeof onRefreshImagingGrid == 'function')
                onRefreshImagingGrid();
            if (value != "") {
                var param = value.split("|");
                if (typeof onAfterAddImagingTestOrder == 'function')
                    onAfterAddImagingTestOrder(param[1]);
            }
        }
    }

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            onRefreshControl();
            setTimeout(function () {
                s.SetFocus();
            }, 0);
        }, 0);
    }

    function onRefreshControl(filterExpression) {
        $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        getCheckedMember();
        cbpViewPopup.PerformCallback('refresh');
    }

    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<div style="padding: 10px; height: 450px; overflow-x: hidden; overflow-y: scroll">
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnImagingServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnLaboratoryServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" value="" id="hdnTestOrderEntryQuickPicksTestOrderID" />
    <input type="hidden" id="hdnDiagnosisSummary" value="0" runat="server" />
    <input type="hidden" id="hdnChiefComplaint" value="0" runat="server" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" value="" runat="server" />
    <input type="hidden" id="hdnPostSurgeryInstructionID" value="" runat="server" />
    <style type="text/css">
        div#multicolumn
        {
            -moz-column-count: 3;
            -moz-column-gap: 10px;
            -webkit-column-count: 3;
            -webkit-column-gap: 10px;
            column-count: 3;
            column-gap: 10px;
        }
        
        #lnkDetailItem
        {
            color: Black;
            font-weight: bold;
            text-decoration: none;
        }
        
        .redAsterik
        {
            color: Red;
        }
    </style>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
            <col style="width: 80px" />
            <col style="width: 150px" />
            <col style="width: 100px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <%=GetLabel("Unit Pelayanan ") %>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%"
                    runat="server">
                    <ClientSideEvents ValueChanged="function(s,e){ onCboServiceUnitValueChanged(); }"
                        Init="function(s,e){ onCboServiceUnitValueChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
            <td class="tdLabel" style="padding-left: 5px">
                <label class="lblNormal">
                    <%=GetLabel("Diagnosis")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboDiagnose" ClientInstanceName="cboDiagnose"
                    Width="500px" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel" valign="top">
                <label class="lblNormal">
                    <%=GetLabel("Tanggal Pemeriksaan")%></label>
            </td>
            <td valign="top">
                <input type="hidden" runat="server" id="hdnDefaultStartDate" value="" />
                <asp:TextBox ID="txtDefaultStartDate" runat="server" Width="120px" ReadOnly="true"
                    Style="text-align: center" />
            </td>
            <td valign="top">
                <input type="hidden" runat="server" id="hdnDefaultStartTime" value="" />
                <asp:TextBox ID="txtDefaultStartTime" runat="server" Width="50px" ReadOnly="true" />
            </td>
            <td class="tdLabel" valign="top" style="padding-left: 5px">
                <label class="lblMandatory">
                    <%=GetLabel("Catatan Klinis") %>
            </td>
            <td colspan="2" rowspan="2">
                <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="Multiline" Rows="2" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel" valign="top">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="chkIsCITO" runat="server" Checked="false" />
                CITO
            </td>
            <td id="tdPATest" runat="server" colspan="2"><asp:CheckBox ID="chkIsPathologicalAnatomyTest" Width="150px" runat="server" Text=" Pemeriksaan PA" /></td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Quick Filter")%></label>
            </td>
            <td colspan="2">
                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                    Width="100%" Watermark="Search">
                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                    <IntellisenseHints>
                        <qis:QISIntellisenseHint Text="Nama Item" FieldName="ItemName1" />
                        <qis:QISIntellisenseHint Text="Kode Item" FieldName="ItemCode" />
                    </IntellisenseHints>
                </qis:QISIntellisenseTextBox>
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                        ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 1em;">
                                    <asp:Repeater ID="rptView" runat="server" OnItemDataBound="rptView_ItemDataBound">
                                        <ItemTemplate>
                                            <div id="divGroupHeader" runat="server" style="text-align: center; font-size: larger">
                                                <%#Eval("ItemGroupName1") %></div>
                                            <div id="divGroupDetail" runat="server">
                                                <asp:DataList ID="rptDetail" runat="server" RepeatColumns="4" RepeatDirection="Vertical">
                                                    <ItemTemplate>
                                                        <div class="divContainerLabItem" style="width: 260px">
                                                            <input type="hidden" class="hdnItemID" value='<%#Eval("ItemID") %>' />
                                                            <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                            <%# Eval("PreconditionNotes").ToString() == "" ? 
                                                                Eval("ItemName1") : 
                                                                "<input class=\"preconditionNotes\" type=\"hidden\" value=\""+Eval("PreconditionNotes")+"\">"+
                                                                "<a href=\"#\" id=\"lnkDetailItem\">"+Eval("ItemName1")+"</a>"+"<span class=\"redAsterik\">*</span>"
                                                            %>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</div>
