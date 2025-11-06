<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/ParamedicPage/MPBaseParamedicPageTrx.master"
    AutoEventWireup="true" CodeBehind="RevenueSharingAdjustmentEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingAdjustmentEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/RevenueSharing/RevenueSharingToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnAdjustmentProcess" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtProcessedDate.ClientID %>');

            $('#lblEntryPopupAddData').click(function (evt) {
                if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
                    $('#<%=hdnID.ClientID %>').val('');
                    $('#<%=txtAdjustmentAmount.ClientID %>').val('0').trigger('changeValue');
                    cboAdjustmentTypeAdd.SetValue('');
                    cboAdjustmentTypeMin.SetValue('');
                    $('#<%=rblAdjustment.ClientID%>').change();
                    $('#<%=chkRevenueSharingFee.ClientID %>').prop('checked', false);
                    $('#<%=txtRemarks.ClientID %>').val('');
                    $('#containerPopupEntryData').show();
                }
            });

            $('#btnEntryPopupCancel').click(function () {
                $('#containerPopupEntryData').hide();
            });

            $('#btnEntryPopupSave').click(function () {
                if (IsValid(null, 'fsEntryPopup', 'mpEntryPopup'))
                    cbpEntryPopupView.PerformCallback('save');
                return false;
            });
        }

        $('#<%=btnAdjustmentProcess.ClientID %>').live('click', function () {
            if ($('#<%=hdnRSTransactionID.ClientID %>').val() == '') {
                showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
            }
            else {
                onCustomButtonClick('process');
            }
        });

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Process Success', 'Proses penyesuaian honor dokter di nomor <b>' + retval + '</b> berhasil dilakukan.', function () {
                cbpView.PerformCallback('refresh');
            });
        }

        //#region Revenue Sharing No
        function onGetRevenueSharingFilterExpression() {
            var filterExpression = "<%:OnGetRevenueSharingFilterExpression() %>";
            return filterExpression;
        };

        $('#lblRevenueSharingNo.lblLink').live('click', function () {
            openSearchDialog('transrevenuesharinghd', onGetRevenueSharingFilterExpression(), function (value) {
                $('#<%=txtRevenueSharingNo.ClientID %>').val(value);
                onTxtRevenueSharingNoChanged(value);
            });
        });

        $('#<%=txtRevenueSharingNo.ClientID %>').live('change', function () {
            onTxtRevenueSharingNoChanged($(this).val());
        });

        function onTxtRevenueSharingNoChanged(value) {
            var filterExpression = onGetRevenueSharingFilterExpression() + " AND RevenueSharingNo = '" + value + "'";
            Methods.getObject('GetTransRevenueSharingHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnRSTransactionID.ClientID %>').val(result.RSTransactionID);
                    $('#<%=hdnRevenueSharingNo.ClientID %>').val(result.RevenueSharingNo); 
                    $('#<%=txtProcessedDate.ClientID %>').val(result.ProcessedDateInString);
                }
                else {
                    $('#<%=hdnRSTransactionID.ClientID %>').val('');
                    $('#<%=txtRevenueSharingNo.ClientID %>').val('');
                    $('#<%=txtProcessedDate.ClientID %>').val('');
                }
            });

            cbpView.PerformCallback();
        };
        //#endregion

        $('#<%=rblAdjustment.ClientID%>').live('change', function () {
            var AdjustmentGroup = $('#<%=rblAdjustment.ClientID %> input:checked').val();
            var RevenueSharingGroupSc = "<%=OnGetRevenueSharingGroupSc() %>";

            if (AdjustmentGroup == RevenueSharingGroupSc) {
                $('#trAdjustmentTypeAdd').show();
                $('#trAdjustmentTypeMin').hide();
            }
            else {
                $('#trAdjustmentTypeAdd').hide();
                $('#trAdjustmentTypeMin').show();
            }

            cbpView.PerformCallback();
        });

        //#region Detail Process
        $('.imgDelete').live('click', function () {
            $tr = $(this).closest('tr');
            var entity = rowToObject($tr);
            $('#<%=hdnID.ClientID %>').val(entity.ID);
            cbpEntryPopupView.PerformCallback('delete');
        });

        $('.imgEdit').live('click', function () {
            $tr = $(this).closest('tr');
            var entity = rowToObject($tr);

            var AdjustmentGroup = $('#<%=rblAdjustment.ClientID %> input:checked').val();
            var RevenueSharingGroupSc = "<%=OnGetRevenueSharingGroupSc() %>";

            $('#<%=hdnID.ClientID %>').val(entity.ID);
            $('#containerPopupEntryData').show();
            if (AdjustmentGroup == RevenueSharingGroupSc) {
                $('#trAdjustmentTypeAdd').show();
                $('#trAdjustmentTypeMin').hide();
                cboAdjustmentTypeAdd.SetValue(entity.GCRSAdjustmentType);
                $('#<%=txtAdjustmentAmount.ClientID %>').val(entity.AdjustmentAmount).trigger('changeValue');
                if (entity.IsTaxed == 'True') {
                    $('#<%=chkRevenueSharingFee.ClientID %>').prop('checked', true);
                }
                else {
                    $('#<%=chkRevenueSharingFee.ClientID %>').prop('checked', false);
                }
                $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
            }
            else {
                $('#trAdjustmentTypeAdd').hide();
                $('#trAdjustmentTypeMin').show();
                cboAdjustmentTypeMin.SetValue(entity.GCRSAdjustmentType);
                $('#<%=txtAdjustmentAmount.ClientID %>').val(entity.AdjustmentAmount).trigger('changeValue');
                $('#<%=chkRevenueSharingFee.ClientID %>').prop('checked', entity.IsTaxed);
                if (entity.IsTaxed == 'True') {
                    $('#<%=chkRevenueSharingFee.ClientID %>').prop('checked', true);
                }
                else {
                    $('#<%=chkRevenueSharingFee.ClientID %>').prop('checked', false);
                }
                $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
            }
        });
        //#endregion

        function onCbpEntryPopupViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    $('#containerPopupEntryData').hide();
                    cbpView.PerformCallback();
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback();
            }
            
            hideLoadingPanel();
        };

    </script>
    <div>
    <input type="hidden" id="hdnPageTitle" runat="server" />
        <fieldset id="fsEntryPopup" style="margin: 0">
        <table class="tblContentArea" style="width: 100%">
            <colgroup>
                <col style="width: 120px"/>
            </colgroup>
            <tr>
                <td><label class="lblNormal lblLink" id="lblRevenueSharingNo"><%=GetLabel("Nomor Bukti")%></label></td>
                <td>
                    <input type="hidden" id="hdnRSTransactionID" runat="server" />
                    <input type="hidden" id="hdnRevenueSharingNo" runat="server" />
                    <asp:TextBox runat="server" ID="txtRevenueSharingNo" Width="220px" />
                </td>
            </tr>
            <tr>
                <td><label class="lblNormal" id="lblDate"><%=GetLabel("Tanggal") %></label></td>
                <td><asp:TextBox runat="server" Width="120px" ID="txtProcessedDate" CssClass="datepicker" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:RadioButtonList runat="server" ID="rblAdjustment" RepeatDirection="Horizontal"/></td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                        <input type="hidden" id="hdnID" runat="server" value="" />
                        <div class="pageTitle"><%=GetLabel("Edit atau Tambah ")%></div>
                            <table class="tblEntryDetail" style="width:100%">
                                <colgroup>
                                    <col style="width:200px"/>
                                    <col />
                                </colgroup>
                                <tr style="display:none" id="trAdjustmentTypeAdd">
                                    <td><label class="lblMandatory" id="lblAdjustmentTypeAdd"><%=GetLabel("Jenis Penambahan") %></label></td>
                                    <td><dxe:ASPxComboBox runat="server" ClientInstanceName="cboAdjustmentTypeAdd" ID="cboAdjustmentTypeAdd" /></td>                                    
                                </tr>
                                <tr style="display:none" id="trAdjustmentTypeMin">
                                    <td><label class="lblMandatory" id="lblAdjustmentTypeMin"><%=GetLabel("Jenis Pengurangan") %></label></td>
                                    <td><dxe:ASPxComboBox runat="server" ClientInstanceName="cboAdjustmentTypeMin" ID="cboAdjustmentTypeMin" /></td>                                    
                                </tr>
                                <tr>
                                    <td></td>
                                    <td><asp:CheckBox ID="chkRevenueSharingFee" runat="server" /><%:GetLabel("Pajak")%></td>
                                </tr>
                                <tr>
                                    <td><label class="lblMandatory" id="lblAdjustmentAmount"><%=GetLabel("Jumlah") %></label></td>
                                    <td><asp:TextBox runat="server" ID="txtAdjustmentAmount" CssClass="txtCurrency" /></td>
                                </tr>
                                <tr>
                                    <td><label class="lblMandatory" id="lblRemarks"><%=GetLabel("Catatan") %></label></td>
                                    <td><asp:TextBox runat="server" ID="txtRemarks" TextMode="MultiLine" Rows="2" /></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                    <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />&nbsp<input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' /></td>
                                </tr>
                            </table>
                    </div>
                </td>
            </tr>
            <tr id="trCbpView">
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" 
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" >
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <input type="hidden" bindingfield="ID" value="<%#: Eval("ID") %>" />
                                                    <input type="hidden" bindingfield="GCRSAdjustmentType" value="<%#: Eval("GCRSAdjustmentType")%>" />
                                                    <input type="hidden" bindingfield="AdjustmentAmount" value="<%#: Eval("AdjustmentAmount")%>" />
                                                    <input type="hidden" bindingfield="IsTaxed" value="<%#: Eval("IsTaxed")%>" />
                                                    <input type="hidden" bindingfield="Remarks" value="<%#: Eval("Remarks")%>" />
                                                    <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                    <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="AdjusmentType" HeaderText="Tipe" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="AdjustmentAmount"  HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="300px" DataFormatString="{0:N}" HeaderText="Jumlah" />
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
                    <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                        <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Add Data")%></span>
                    </div>
                    <dxcp:ASPxCallbackPanel runat="server" ID="cbpEntryPopupView" ClientInstanceName="cbpEntryPopupView" 
                        OnCallback="cbpEntryPopupView_Callback" ShowLoadingPanel="false">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e) { onCbpEntryPopupViewEndCallback(s); }" />
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
        </fieldset>
    </div>
</asp:Content>
