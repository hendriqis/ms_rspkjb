<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParamedicHospitalFeeCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.ParamedicHospitalFeeCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ParamedicHospitalFeeCtl">
    $(function () {
        setDatePicker('<%=txtStartDate.ClientID %>');
    });

    //#region Revenue Sharing
    function onGetRevenueSharingFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblRevenueSharing.lblLink').live('click', function () {
        openSearchDialog('revenuesharing', onGetRevenueSharingFilterExpression(), function (value) {
            $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
            ontxtRevenueSharingCodeChanged(value);
        });
    });

    $('#<%=txtRevenueSharingCode.ClientID %>').live('change', function () {
        ontxtRevenueSharingCodeChanged($(this).val());
    });

    function ontxtRevenueSharingCodeChanged(value) {
        var filterExpression = onGetRevenueSharingFilterExpression() + " AND RevenueSharingCode = '" + value + "'";
        Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                $('#<%=txtRevenueSharingName.ClientID %>').val(result.RevenueSharingName);
            }
            else {
                $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                $('#<%=txtRevenueSharingName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#lblEntryPopupAddData').live('click', function () {
        var today = new Date();
        var tempDate = "00";
        if (today.getDate() < 10) {
            tempDate = "0" + today.getDate();
        } else {
            tempDate = today.getDate();
        }
        var pad = "00";
        var tmpMonth = (today.getMonth() + 1).toString();
        var month = pad.substring(0, pad.length - tmpMonth.length) + tmpMonth;
        var date = tempDate + '-' + month + '-' + today.getFullYear();

        $('#<%=hdnID.ClientID %>').val("");
        $('#<%=hdnRevenueSharingID.ClientID %>').val('');
        $('#<%=txtRevenueSharingCode.ClientID %>').val('');
        $('#<%=txtRevenueSharingName.ClientID %>').val('');
        $('#<%=txtStartDate.ClientID %>').val(date);
        $('#<%=chkIsPercentage.ClientID %>').prop('checked', false);
        $('#<%=txtAmount.ClientID %>').val("0");

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var ID = $row.find('.ID').val();
            $('#<%=hdnID.ClientID %>').val(ID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.ID').val();
        var ParamedicID = $row.find('.ParamedicID').val();
        var RevenueSharingID = $row.find('.RevenueSharingID').val();
        var RevenueSharingCode = $row.find('.RevenueSharingCode').val();
        var RevenueSharingName = $row.find('.RevenueSharingName').val();
        var cfStartDateInDatePickerString = $row.find('.cfStartDateInDatePickerString').val();
        var IsPercentage = $row.find('.IsPercentage').val();
        var Amount = $row.find('.Amount').val();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnRevenueSharingID.ClientID %>').val(RevenueSharingID);
        $('#<%=txtRevenueSharingCode.ClientID %>').val(RevenueSharingCode);
        $('#<%=txtRevenueSharingName.ClientID %>').val(RevenueSharingName);
        $('#<%=txtStartDate.ClientID %>').val(cfStartDateInDatePickerString);
        $('#<%=chkIsPercentage.ClientID %>').attr('checked', false);
        if (IsPercentage == "True") $('#<%=chkIsPercentage.ClientID %>').attr('checked', true);
        $('#<%=txtAmount.ClientID %>').val(Amount).trigger('changeValue');

        $('#containerPopupEntryData').show();
    });

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingViewPopup').hide();
    }
</script>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" id="hdnParamedicIDCtl" value="" runat="server" />
    <table class="tblContentArea">
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
                            <label class="lblNormal">
                                <%=GetLabel("Dokter / Paramedis")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                    <tr>
                        <td>
                            <label class="lblMandatory lblLink" id="lblRevenueSharing">
                                <%=GetLabel("Jasa Medis")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnRevenueSharingID" runat="server" value="" />
                            <asp:TextBox ID="txtRevenueSharingCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtRevenueSharingName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Start Date")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStartDate" Width="120px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
									<label class="lblNormal">
										<%=GetLabel("Is Percentage")%></label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsPercentage" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Amount")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAmount" Width="200px" runat="server" CssClass="txtCurrency" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Tutup")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="ID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="ParamedicID" value="<%#: Eval("ParamedicID")%>" />
                                                <input type="hidden" class="RevenueSharingID" value="<%#: Eval("RevenueSharingID")%>" />
                                                <input type="hidden" class="RevenueSharingCode" value="<%#: Eval("RevenueSharingCode")%>" />
                                                <input type="hidden" class="RevenueSharingName" value="<%#: Eval("RevenueSharingName")%>" />
                                                <input type="hidden" class="cfStartDateInDatePickerString" value="<%#: Eval("cfStartDateInDatePickerString")%>" />
                                                <input type="hidden" class="IsPercentage" value="<%#: Eval("IsPercentage")%>" />
                                                <input type="hidden" class="Amount" value="<%#: Eval("Amount")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RevenueSharingName" HeaderText="Revenue Sharing" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="cfStartDateInString" HeaderText="Start Date" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="100px" />
                                        <asp:CheckBoxField DataField="IsPercentage" HeaderText="Is Percentage" HeaderStyle-Width="80px"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="cfAmountInString" HeaderText="Amount" HeaderStyle-HorizontalAlign="Right"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="cfCreatedDateInString" HeaderText="Created Date" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="200px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Add Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
