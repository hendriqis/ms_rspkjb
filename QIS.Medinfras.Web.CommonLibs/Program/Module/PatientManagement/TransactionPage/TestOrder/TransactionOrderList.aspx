<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="TransactionOrderList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionOrderList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

      //#region TAB
        $(function () {
            $('#ulTabLabResult li').click(function () {
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerInfo').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });

            setDatePicker('<%=txtServiceOrderDateFrom.ClientID %>');
            $('#<%=txtServiceOrderDateFrom.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtServiceOrderDateTo.ClientID %>');
            $('#<%=txtServiceOrderDateTo.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtServiceOrderDateFrom.ClientID %>').change(function (evt) {
                cbpView.PerformCallback('refresh');
            });

            $('#<%=txtServiceOrderDateTo.ClientID %>').change(function (evt) {
                cbpView.PerformCallback('refresh');
            });

            $('#btnRefresh').click(function () {
                cbpView.PerformCallback('refresh');
            });

        });
        //#endregion

        $('.lnkDetail a').live('click', function () {
            var OrderID = $(this).closest('tr').find('.keyField').html();
            var OrderType = $(this).closest('tr').find('.OrderType').val();
            var id = OrderID + '|' + OrderType;
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/TestOrder/TransactionOrderDetailListCtl.ascx");
            openUserControlPopup(url, id, 'Test Order Detail', 900, 500);
        });

        $('.lnkHeader a').live('click', function () {
            var ItemID = $(this).closest('tr').find('.keyField').html();
            var RegistrationID = $(this).closest('tr').find('.RegistrationID').val();
            var id = RegistrationID + '|' + ItemID;
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/TestOrder/TransactionOrderHeaderListCtl.ascx");
            openUserControlPopup(url, id, 'Test Order Detail', 1100, 500);
        });

        function onClosePopUp() {
            cbpView.PerformCallback('refresh');
        }
        function getServiceUnitFilterFilterExpression() {
            var filterExpression = "HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vPatientOrderAll WHERE RegistrationID = '" + $('#<%=hdnRegistrationID.ClientID %>').val() +"')";
            return filterExpression;
        }

        $('#<%=lblSrvceUnit.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('serviceunitperhealthcare', getServiceUnitFilterFilterExpression(), function (value) {
                $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                onTxtServiceUnitCodeChanged(value);
            });
        });
        $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
            onTxtServiceUnitCodeChanged($(this).val());
        });

        function onTxtServiceUnitCodeChanged(value) {
            var filterExpression = getServiceUnitFilterFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
                cbpViewDetail.PerformCallback('refresh');
            });
        }


        function onLoad() {
            setDatePicker('<%=txtServiceOrderDateFrom.ClientID %>');
            setDatePicker('<%=txtServiceOrderDateTo.ClientID %>');
          }
    </script>
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 160px" />
                            <col />
                        </colgroup>
                        <tr runat="server" id="trServiceUnit">
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblSrvceUnit">
                                    <%=GetLabel("Unit Pelayanan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitName" Width="100%" runat="server" Enabled="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Order") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtServiceOrderDateFrom" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                             <%=GetLabel("s/d") %>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServiceOrderDateTo" Width="120px" CssClass="datepicker" runat="server"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                           <td>
                              <input type="button" id="btnRefresh" value="Refresh" />
                           </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div class="containerUlTabPage" style="margin-bottom: 3px;">
        <ul class="ulTabPage" id="ulTabLabResult">
            <li contentid="containerHeader" class="selected">
                <%=GetLabel("Per Nomor Order")%></li>
            <li contentid="containerDetail">
                <%=GetLabel("Per Detail")%></li>
        </ul>
    </div>
    <div id="containerHeader" class="containerInfo">
        <table class="tblContentArea">
            <tr>
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em; height: 250px; overflow-y: auto">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="OrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <input type="hidden" class="OrderType" value='<%#: Eval("OrderType")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="cfOrderDateInString" HeaderText="Tanggal" HeaderStyle-Width="90px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="OrderTime" HeaderText="Jam" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="OrderNo" HeaderText="No. Order" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfScheduledDateInString" HeaderText="Tanggal Dijadwalkan" HeaderStyle-Width="90px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign = "Center" />
                                            <asp:BoundField DataField="ScheduledTime" HeaderText="Jam Dijadwalkan" HeaderStyle-Width="30px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign = "Center" />
                                            <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dokter Order" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="200px" />
                                            <asp:BoundField DataField="CreatedByName" HeaderText="Dibuat Oleh" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="200px" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left"/>
                                            <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:HyperLinkField HeaderStyle-Width="50px" DataTextField="ItemComparison" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-CssClass="lnkDetail" HeaderStyle-HorizontalAlign="Center" HeaderText="Detail" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="containerDetail" class="containerInfo" style="display: none">
        <table class="tblContentArea">
            <tr>
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" Width="100%" ClientInstanceName="cbpViewDetail"
                        ShowLoadingPanel="false" OnCallback="cbpViewDetail_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContentDetail1" runat="server">
                                <asp:Panel runat="server" ID="pnlViewDetail" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 250px; overflow-y: auto">
                                    <asp:GridView ID="grdViewDetail" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <input type="hidden" class="RegistrationID" value='<%#: Eval("RegistrationID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="250px" />
                                            <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:HyperLinkField HeaderStyle-Width="100px" DataTextField="QtySum" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lnkHeader" HeaderText="Jumlah" />
                                            <asp:BoundField DataField="UnitName" HeaderText="Satuan" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
