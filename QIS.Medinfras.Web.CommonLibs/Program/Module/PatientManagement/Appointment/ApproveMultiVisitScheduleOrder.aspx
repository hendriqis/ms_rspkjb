<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ApproveMultiVisitScheduleOrder.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ApproveMultiVisitScheduleOrder" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
    <li id="btnApproveOrder" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Approve")%></div>
    </li>
    <li id="btnDeclineOrder" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Decline")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtFromOrderDate.ClientID %>');
            setDatePicker('<%=txtToOrderDate.ClientID %>');
            $('#<%=txtFromOrderDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtToOrderDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, '');

            $('#<%=btnApproveOrder.ClientID %>').click(function () {
                var orderHdID = $('#<%=hdnID.ClientID %>').val();
                var param = getDetailOrder(orderHdID);
                cbpView.PerformCallback('approve|' + param);
            });

            $('#<%=btnDeclineOrder.ClientID %>').click(function () {
                var orderHdID = $('#<%=hdnID.ClientID %>').val();
                cbpView.PerformCallback('decline|' + orderHdID);
            });

            $('#<%=btnRefresh.ClientID %>').click(function () {
                cbpView.PerformCallback('refresh');
            });

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') == 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                    $('#<%=hdnToHealthcareServiceUnitID.ClientID %>').val($(this).find('.hiddenColumn').html());

                    $('#containerPopupEntryData').hide();
                    $('#<%:hdnItemID.ClientID %>').val('');
                    $('#<%:txtItemCode.ClientID %>').val('');
                    $('#<%:txtItemName.ClientID %>').val('');

                    cbpViewDt.PerformCallback('refresh');
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function getDetailOrder(orderHdID) {
            var lstSelectedMember = orderHdID + "^";
            var tbl = $("[id$=grdViewDt]");
            var rows = tbl.find('tr');
            for (var index = 1; index < rows.length; index++) {
                var row = rows[index];
                var key = $(row).find(".keyField").html();
                if (key != null && key != "null") {
                    var qty = $(row).find(".txtItemQty").val();
                    var isSelected = $(row).find(".chkIsSelected input").is(":checked");
                    var startDate = $(row).find(".txtStartScheduleDate").val();
                    var endDate = $(row).find(".txtEndScheduleDate").val();
                    //                var interval = $(row).find(".txtInterval").val();
                    var interval = "1";
                    //                if (isSelected) {
                    //                    lstSelectedMember += key + "~" + qty + "~" + startDate + "~" + endDate + "~" + interval + "&";
                    //                }
                    lstSelectedMember += key + "~" + qty + "~" + startDate + "~" + endDate + "~" + interval + "&";
                }
            }
            return lstSelectedMember;
        }

        function setDatePickerGrid() {
            var tbl = $("[id$=grdViewDt]");
            var rows = tbl.find('tr');
            for (var index = 1; index < rows.length; index++) {
                var row = rows[index];
                var startDate = $(row).find(".txtStartScheduleDate");
                var endDate = $(row).find(".txtEndScheduleDate");
                setDatePickerElement(startDate);
                setDatePickerElement(endDate);
                startDate.datepicker('option', 'minDate', '0');
                endDate.datepicker('option', 'minDate', '0');
            }
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0) {
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();
                }
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }

            cbpViewDt.PerformCallback('refresh');
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
            }
            else if (param[0] == 'approve') {
                if (param[1] == "fail") {
                    displayErrorMessageBox("ERROR", param[2]);
                }
            }
            else if (param[0] == "add") {
                if (param[1] == "fail") {
                    displayErrorMessageBox("ERROR", param[2]);
                }
                else {
                    $('#containerPopupEntryData').hide();
                }
            }
            else {
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
            }
            setDatePickerGrid();
        }
        //#endregion

        $('#lblEntryPopupAddData').live('click', function () {
            $('#containerPopupEntryData').show();
        });
        $('#btnEntryPopupCancel').live('click', function () {
            $('#containerPopupEntryData').hide();
        });
        $('#btnEntryPopupSave').live('click', function () {
            if ($('#<%:hdnIsEdited.ClientID %>').val() == "" || $('#<%:hdnIsEdited.ClientID %>').val() == "0") {
                cbpView.PerformCallback('add|' + $('#<%:hdnID.ClientID %>').val() + "|" + $('#<%:hdnItemID.ClientID %>').val());
            }
            else {
                cbpView.PerformCallback('edit|' + $('#<%:hdnID.ClientID %>').val() + "|" + $('#<%:hdnTestOrderDtID.ClientID %>').val() + "|" + $('#<%:hdnItemID.ClientID %>').val());
                $('#<%:hdnIsEdited.ClientID %>').val("0");
            }
        });
        //#region Add Item
        function GetItemFilterExpression(hsuID, testOrderID) {
            var filterExpression = "IsDeleted = 0 AND HealthcareServiceUnitID = " + hsuID;

            return filterExpression;
        }

        $('#lblItem.lblLink').live('click', function () {
            var hsuID = $('#<%:hdnToHealthcareServiceUnitID.ClientID %>').val();
            var testOrderID = $('#<%:hdnID.ClientID %>').val();
            var filter = GetItemFilterExpression(hsuID, testOrderID);
            openSearchDialog("serviceunititem", filter, function (value) {
                $('#<%:txtItemCode.ClientID %>').val(value);
                ontxtItemCodeChanged(value);
            });
        });

        $('#<%=txtItemCode.ClientID %>').live('change', function () {
            ontxtItemCodeChanged($(this).val());
        });

        function ontxtItemCodeChanged(value) {
            var filterExpression = "ItemCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvServiceUnitItemList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnItemID.ClientID %>').val(result.ItemID);
                    $('#<%:txtItemCode.ClientID %>').val(result.ItemCode);
                    $('#<%:txtItemName.ClientID %>').val(result.ItemName1);
                }
                else {
                    $('#<%:hdnItemID.ClientID %>').val('');
                    $('#<%:txtItemCode.ClientID %>').val('');
                    $('#<%:txtItemName.ClientID %>').val('');
                }
            });
        }
        //#endregion
        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function (evt) {
            $row = $(this).closest('tr').parent().closest('tr');
            if (confirm("Are You Sure Want To Delete This Data?")) {
                var entity = rowToObject($row);
                cbpView.PerformCallback('delete|' + entity.TestOrderDtID);
            }
        });

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%:hdnID.ClientID %>').val(entity.TestOrderID);
            $('#<%:hdnTestOrderDtID.ClientID %>').val(entity.TestOrderDtID);
            $('#<%:hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%:txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%:txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%:hdnIsEdited.ClientID %>').val("1");

            $('#containerPopupEntryData').show();
        });
    </script>
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnToHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnListData" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderDtID" runat="server" value="" />
    <input type="hidden" id="hdnIsEdited" runat="server" value="" />
    <div style="position: relative">
        <tr>
            <table class="tblContentArea" style="width: 100%">
                <colgroup>
                    <col style="width: 5%" />
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Tanggal Order")%></label>
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width: 145px" />
                                <col style="width: 3px" />
                                <col style="width: 145px" />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtFromOrderDate" Width="120px" CssClass="datepicker" runat="server"/>
                                </td>
                                <td>
                                    <%=GetLabel("s/d") %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToOrderDate" Width="120px" CssClass="datepicker" runat="server"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Unit Pelayanan")%></label>
                    </td>
                    <td>
                        <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="15%"
                            runat="server">
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </tr>
        <tr>
            <table style="width: 100">
                <colgroup>
                    <col style="width: 50%" />
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td style="vertical-align: top">
                        <div style="overflow=scroll">
                            <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridProcessList1">
                                            <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                    <asp:BoundField DataField="HealthcareServiceUnitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                                    <asp:BoundField DataField="FromRegistrationNo" HeaderStyle-HorizontalAlign="Left" HeaderText="No. Registrasi Asal"
                                                        HeaderStyle-Width="150px" />
                                                    <asp:BoundField DataField="MedicalNo" HeaderStyle-HorizontalAlign="Left" HeaderText="No. RM"
                                                        HeaderStyle-Width="150px" />
                                                    <asp:BoundField DataField="PatientName" HeaderStyle-HorizontalAlign="Left" HeaderText="Nama Pasien"
                                                        HeaderStyle-Width="150px" />
                                                    <asp:BoundField DataField="TestOrderNo" HeaderStyle-HorizontalAlign="Left" HeaderText="No. Order"
                                                        HeaderStyle-Width="150px" />
                                                    <asp:BoundField DataField="ServiceUnitName" HeaderStyle-HorizontalAlign="Left" HeaderText="Unit Pelayanan"
                                                        HeaderStyle-Width="200px" />

                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("Tidak ada permintaan order multi kunjungan")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                        </div>
                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </td>
                    <td style="vertical-align: top">
                        <div id="containerPopupEntryData" style="display:none">
                            <fieldset id="fsEntryPopup" style="margin:0"> 
                                <input type="hidden" runat="server" id="Hidden1" />
                                <table class="tblEntryDetail">
                                    <colgroup>
                                        <col style="width:20px"/>
                                        <col style="width:150px"/>
                                        <col />
                                    </colgroup>
                                    <tr valign="top">
                                        <td class="tdLabel" style="padding-top:5px">
                                            <label class="lblMandatory lblLink" id="lblItem"><%=GetLabel("Item")%></label>
                                        </td>
                                        <td>
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 120px" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtItemCode" Width="120px" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <center>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' class="btnEntryPopupSave w3-btn w3-hover-blue"/>
                                                        </td>
                                                        <td>
                                                            <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' class="btnEntryPopupCancel w3-btn w3-hover-blue"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <div style="position: relative">
                            <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                OnCallback="cbpViewDt_Callback" ShowLoadingPanel="false">
                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                    EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                        <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridProcessList1">
                                            <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewDt_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="TestOrderDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" Visible=False>
                                                        <HeaderTemplate>
                                                            <input id="chkSelectAll" type="checkbox" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10px" Visible="True">
                                                        <ItemTemplate>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td><img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left;" /></td>
                                                                    <td style="width:3px">&nbsp;</td>
                                                                    <td><img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
                                                                </tr>
                                                            </table>
                                                            <input type="hidden" value="<%#:Eval("TestOrderDtID") %>" bindingfield="TestOrderDtID" />
                                                            <input type="hidden" value="<%#:Eval("TestOrderID") %>" bindingfield="TestOrderID" />
                                                            <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                            <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                            <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderTemplate>
                                                            <div>
                                                                <%=GetLabel("Item")%></div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div>
                                                                <%#: Eval("ItemName1")%></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px"
                                                        ItemStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <div>
                                                                <%=GetLabel("Qty")%></div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtItemQty" CssClass="txtItemQty" Width="50px" style="text-align: center"/>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px"
                                                        ItemStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <div>
                                                                <%=GetLabel("Tanggal Mulai")%></div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtStartScheduleDate" CssClass="datepicker txtStartScheduleDate" Width="120px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px"
                                                        ItemStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <div>
                                                                <%=GetLabel("Tanggal Akhir")%></div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtEndScheduleDate" CssClass="datepicker txtEndScheduleDate" Width="120px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px"
                                                        ItemStyle-HorizontalAlign="Center" Visible=false>
                                                        <HeaderTemplate>
                                                            <div>
                                                                <%=GetLabel("Interval (Hari)")%></div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtInterval" CssClass="txtInterval" TextMode="Number" Width="50px" style="text-align: center" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("Tidak ada item")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                            <div style="width:100%;text-align:center;display:block" id="divContainerAddData" runat="server">
                                <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Tambah Data")%></span>
                            </div>
                            <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </tr>
    </div>
</asp:Content>
