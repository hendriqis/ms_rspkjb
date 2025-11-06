<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true" 
    CodeBehind="PatientChargesList1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientChargesList1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhRightToolbar" runat="server">
    <table>
        <tr>
            <td><%=GetLabel("Transaction View Type") %></td>
            <td>
                <asp:DropDownList ID="ddlViewType" runat="server">
                    <asp:ListItem Text="All" Value="0" />
                    <asp:ListItem Text="Entried By Physician Only" Value="1"  Selected="True"/>
                    <asp:ListItem Text="Entried By Nurse Only" Value="2" />
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnTransactionID.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewDt.PerformCallback('refresh');
                }
            });

            $('#<%=grdViewDt.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdViewDt.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnDetailID.ClientID %>').val($(this).find('.keyField').html());
                }
            });

            $('#<%=ddlViewType.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            });

            $('.imgAddDetail.imgLink').die('click');
            $('.imgAddDetail.imgLink').live('click', function () {
                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnTransactionID.ClientID %>').val($tr.find('.keyField').html());
                var url = ResolveUrl("~/Program/PatientPage/PatientTransaction/ChargesEntry/ChargesEntryQuickPicksCtl1.ascx");
                var param = $('#<%=hdnTransactionID.ClientID %>').val() + '|' + '<%=GetQuickPicksParam() %>';
                openUserControlPopup(url, param, 'Add Charges Item', 1200, 600);
            });

            $('.imgPropose.imgLink').die('click');
            $('.imgPropose.imgLink').live('click', function () {
                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnTransactionID.ClientID %>').val($tr.find('.keyField').html());
                var hdnTransactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                var hdnIsShowItemNotificationWhenProposed = $('#<%=hdnIsShowItemNotificationWhenProposed.ClientID %>').val();
                var hdnVisitID;
                var transactionDate;
                var filterExpression = 'TransactionID = ' + hdnTransactionID;
                Methods.getObject('GetPatientChargesHdList', filterExpression, function (result) {
                    hdnVisitID = result.VisitID;
                    transactionDate = result.cfTransactionDateInString;
                });

                var message = '';
                if (hdnIsShowItemNotificationWhenProposed == "1") {
                    Methods.getPatientChargesValidationDoubleInputList(hdnVisitID, hdnTransactionID, transactionDate, function (result) {
                        if (result.check != '') {
                            message = "<b>Item berikut pada hari ini sudah diinput sebelumnya di nomor transaksi lain,</b>";
                            message += result.check + '<BR><b>lanjutkan PROPOSED transaksi ini?</b>';
                        }
                    });
                }

                var param = 'propose;;';
                if (message != '') {
                    displayConfirmationMessageBox('PROPOSE', message, function (result) {
                        if (result) onCustomButtonClick(param);
                    });
                }
                else {
                    onCustomButtonClick(param);
                }
            });

            $('.imgDelete.imgLink').die('click');
            $('.imgDelete.imgLink').live('click', function () {
                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnTransactionID.ClientID %>').val($tr.find('.keyField').html());
                showDeleteConfirmation(function (data) {
                    var param = 'void;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            });

            $('.imgEditDetail.imgLink').die('click');
            $('.imgEditDetail.imgLink').live('click', function () {
                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnDetailID.ClientID %>').val($tr.find('.keyField').html());

                var url = ResolveUrl("~/Program/PatientPage/PatientTransaction/ChargesEntry/EditPatientChargesItemCtl1.ascx");
                var param = $('#<%=hdnTransactionID.ClientID %>').val() + '|' + $('#<%=hdnDetailID.ClientID %>').val();
                openUserControlPopup(url, param, 'Edit Charges Item', 1100, 300);
            });

            $('.imgDeleteDetail.imgLink').die('click');
            $('.imgDeleteDetail.imgLink').live('click', function () {
                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnDetailID.ClientID %>').val($tr.find('.keyField').html());
                showDeleteConfirmation(function (data) {
                    var param = 'deleteDetail;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            });

            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function onAfterSaveAddRecordEntryPopup(param) {
            $('#<%=hdnTransactionID.ClientID %>').val(param);
            cbpView.PerformCallback('refresh');
            cbpViewDt.PerformCallback('refresh');
        }

        function onAfterCustomClickSuccess() {
            cbpView.PerformCallback('refresh');
            cbpViewDt.PerformCallback('refresh');
        }

        function onRefreshControl() {
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
            $('#containerImgLoadingView').hide();;
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt"), pageCount, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onBeforeLoadRightPanelContent(code) {
            return $('#<%:hdnRegistrationID.ClientID %>').val();
        }
    </script>
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnDetailID" runat="server" value="" />  
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnIsValidateBeforeEntry" value="" runat="server" />
    <input type="hidden" id="hdnIsShowItemNotificationWhenProposed" value="" runat="server" />   
    <input type="hidden" id="hdnIsAllowPreviewTariff" runat="server" value="0" /> 
    <table style="width:100%">
        <colgroup>
            <col style="width:30%"/>
            <col style="width:70%"/>
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="TransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                               <img class="imgAddDetail imgLink" title='<%=GetLabel("Add Item")%>' <%# Eval("cfIsEditable").ToString() == "False" ? "Style='display:none'":"" %> src='<%# ResolveUrl("~/Libs/Images/Button/add.png")%>'
                                                                    alt="" style="float: left" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgDelete imgLink" <%# Eval("cfIsEditable").ToString() == "False" ? "Style='display:none'":"" %> title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" style="float: left" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                              <img class="imgPropose imgLink" <%# Eval("cfIsEditable").ToString() == "False" ? "Style='display:none'":"" %> title='<%=GetLabel("Propose")%>' src='<%# ResolveUrl("~/Libs/Images/Button/propose.png")%>'
                                                                    alt="" style="float: left" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>  
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Transaction Date") %> - <%=GetLabel("Time") %>,  <span style="color:blue"><%=GetLabel("Transaction No.") %></span></div>
                                                    <div style="font-weight: bold"><%=GetLabel("Service Unit") %></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><%#: Eval("TransactionDateInString")%>, <%#: Eval("TransactionTime") %>, <span <%# Eval("IsEntryByPhysician").ToString() == "True" ? "Style='color:blue;font-weight:bold'":"" %>> <%#: Eval("TransactionNo")%></span></div>
                                                    <div style="font-weight: bold"><%#: Eval("ServiceUnitName")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="TransactionStatus" HeaderText="Status" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="80px"/>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
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
            </td>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                               <img class="imgEditDetail imgLink" title='<%=GetLabel("Edit Item")%>' <%# Eval("cfIsEditable").ToString() == "False" ? "Style='display:none'":"" %> src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" style="float: left" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgDeleteDetail imgLink" title='<%=GetLabel("Delete Item")%>' <%# Eval("cfIsEditable").ToString() == "False" ? "Style='display:none'":"" %>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" style="float: left" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div style="font-weight:bold"><%=GetLabel("Item Name") %></div>
                                                    <div><span><%=GetLabel("Remarks") %></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="font-weight:bold"><%#: Eval("ItemName1") %></div>
													<div><span style="color:blue" ><%#: Eval("ParamedicName") %></span></div> 
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ChargeClassName" HeaderText="Charge Class" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px">
                                                <HeaderTemplate>
                                                    <div style="font-weight:bold"><%#: IsAllowPreviewTariff() == "0" ? "" : GetLabel("Line Amount")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><%#: IsAllowPreviewTariff() == "0" ? "" : Eval("LineAmount", "{0:N}")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display") %>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingDt"></div>
                        </div>
                    </div> 
                </div>
            </td>
        </tr>
    </table>
    
</asp:Content>
