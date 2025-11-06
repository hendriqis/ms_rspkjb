<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master" AutoEventWireup="true" 
    CodeBehind="MedicationOrderList.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.MedicationOrderList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Program/Prescription/UDD/UDDToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh") %></div>
    </li>
    <li id="btnCompleted" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Completed") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewDt.PerformCallback('refresh');
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Propose
            $('.btnPropose').click(function () {
                $btnPropose = $(this);

                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnPrescriptionOrderID.ClientID %>').val($tr.find('.keyField').html());
                onCustomButtonClick('Propose');
            });
            //#endregion

            //#region Refresh
            $('#<%=btnRefresh.ClientID %>').click(function () {
                onRefreshControl();
            });
            //#endregion

            $('#<%=btnCompleted.ClientID %>').click(function () {
                var url = ResolveUrl("~/Program/Prescription/UDD/MedicationOrder/EditMedicationOrderStatusCtl.ascx");
                var id = $('#<%=hdnID.ClientID %>').val();
                var param = id;
                openUserControlPopup(url, param, 'UDD - Medication Order Status', 500, 300);
            });
        });

        function onRefreshControl() {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
            $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
            cbpView.PerformCallback('refresh');
            cbpViewDt.PerformCallback('refresh');
        }
        
        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

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
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $('.lblDispenseQuantity.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var value = $tr.find('.hdnCfDispenseQuantity').val();
            if (value != '0') {
                var orderDetailID = $tr.find('.hdnOrderDetailID').val();
                var itemID = $tr.find('.hdnItemID').val();
                var itemName = $tr.find('.hdnDrugName').val();
                var paramedicName = $tr.find('.hdnParamedicName').val();
                var param = orderDetailID + '|' + 0 + '|' + itemID + '|' + itemName + '|' + paramedicName;
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/MedicationList/MedicationDispensedInfoCtl.ascx");
                openUserControlPopup(url, param, 'Medication - Dispensed Detail', 800, 500);
            }
        });

        $('.lblTakenQuantity.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var value = $tr.find('.hdnCfTakenQuantity').val();
            if (value != '0') {
                var orderDetailID = $tr.find('.hdnOrderDetailID').val();
                var itemID = $tr.find('.hdnItemID').val();
                var itemName = $tr.find('.hdnDrugName').val();
                var paramedicName = $tr.find('.hdnParamedicName').val();
                var param = orderDetailID + '|' + itemID + '|' + itemName + '|' + paramedicName;
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/MedicationList/MedicationTakenInfoCtl.ascx");
                openUserControlPopup(url, param, 'Medication - Administration Detail', 800, 500);
            }
        });
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:25%"/>
            <col style="width:75%"/>
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
                                            <asp:BoundField DataField="PrescriptionOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="cfPrescriptionDate" HeaderText="Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px"/>
                                            <asp:BoundField DataField="PrescriptionTime" HeaderText="Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px"/>
                                            <asp:TemplateField  HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>    
                                                    <div><%=GetLabel("Order No.")%></div>
                                                    <div><%=GetLabel("Physician")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><%#: Eval("PrescriptionOrderNo")%></div>
                                                    <div><%#: Eval("ParamedicName")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="OrderStatus" HeaderText="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"/>
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
                                        <asp:ListView ID="grdViewDt" runat="server" OnItemDataBound="lvwView_ItemDataBound" >
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" align="center" style="width:30px">
                                                            <div>
                                                                <%=GetLabel("UDD")%></div>
                                                        </th>
                                                        <th rowspan="2" align="center" style="width:30px">
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div>
                                                                <%=GetLabel("Drug Name")%>
                                                                -
                                                                <%=GetLabel("Form")%></div>
                                                            <div>
                                                                <div style="color: Blue; float: left;font-style:italic">
                                                                    <%=GetLabel("Generic Name")%></div>
                                                                    </div>
                                                        </th>
                                                        <th colspan="6" align="center">
                                                            <div>
                                                                <%=GetLabel("Signa")%></div>
                                                        </th>
                                                        <th rowspan="2" align="center" style="padding: 3px; width: 90px;">
                                                            <div>
                                                                <%=GetLabel("Start Date")%></div>
                                                        </th>
                                                        <th colspan="4">
                                                            <div>
                                                                <%=GetLabel("Medication Time") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 100px;">
                                                            <div style="text-align:left">
                                                                <%=GetLabel("Status") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 80px">
                                                            <div style="text-align: left; padding-right: 3px">
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 40px;">
                                                            <div style="text-align:right">
                                                                <%=GetLabel("Frequency") %></div>
                                                        </th>
                                                        <th style="width: 40px;text-align:left">
                                                            <div>
                                                                <%=GetLabel("Timeline") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div style="text-align:right">
                                                                <%=GetLabel("Dose") %></div>
                                                        </th>
                                                        <th style="width: 50px;">
                                                            <div style="text-align:left">
                                                                <%=GetLabel("Unit") %></div>
                                                        </th>
                                                        <th align="center" style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("PRN")%></div>
                                                        </th>
                                                        <th style="width: 100px;">
                                                            <div style="text-align:left">
                                                                <%=GetLabel("Route") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Morning") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Noon") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Evening") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Night") %></div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr class="trFooter">
                                                        <td colspan="17">
                                                            <div style="text-align: right; padding: 0px 3px">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" align="center" style="width:30px">
                                                            <div>
                                                                <%=GetLabel("UDD")%></div>
                                                        </th>
                                                        <th rowspan="2" align="center" style="width:30px">
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div>
                                                                <%=GetLabel("Drug Name")%>
                                                                -
                                                                <%=GetLabel("Form")%></div>
                                                            <div>
                                                                <div style="color: Blue; float: left;font-style:italic">
                                                                    <%=GetLabel("Generic Name")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="6" align="center">
                                                            <div>
                                                                <%=GetLabel("Signa")%></div>
                                                        </th>
                                                        <th rowspan="2" align="center" style="padding: 3px; width: 90px;">
                                                            <div>
                                                                <%=GetLabel("Start Date")%></div>
                                                        </th>
                                                        <th colspan="4">
                                                            <div>
                                                                <%=GetLabel("Medication Time") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 90px;">
                                                            <div style="text-align:center">
                                                                <%=GetLabel("End Date") %></div>
                                                        </th>
                                                        <th colspan="3">
                                                            <div style="text-align:center; font-weight:bold">
                                                                <%=GetLabel("QUANTITY") %></div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 40px;">
                                                            <div style="text-align:right">
                                                                <%=GetLabel("Frequency") %></div>
                                                        </th>
                                                        <th style="width: 40px;text-align:left">
                                                            <div>
                                                                <%=GetLabel("Timeline") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div style="text-align:right">
                                                                <%=GetLabel("Dose") %></div>
                                                        </th>
                                                        <th style="width: 50px;">
                                                            <div style="text-align:left">
                                                                <%=GetLabel("Unit") %></div>
                                                        </th>
                                                        <th align="center" style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("PRN")%></div>
                                                        </th>
                                                        <th style="width: 100px;">
                                                            <div style="text-align:left">
                                                                <%=GetLabel("Route") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Morning") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Noon") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Evening") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Night") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div style="font-weight:bold">
                                                                <%=GetLabel("Dispensed") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div style="font-weight:bold">
                                                                <%=GetLabel("Taken") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div style="font-weight:bold">
                                                                <%=GetLabel("Balance") %></div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr class="trFooter">
                                                        <td colspan="18">
                                                            <div style="text-align: right; padding: 0px 3px">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center" style="width:30px;background:#ecf0f1; vertical-align:middle"><asp:CheckBox ID="chkIsUsingUDD" runat="server" Enabled="false" CssClass="chkIsUsingUDD" Checked='<%# Eval("IsUsingUDD")%>' /></td>
                                                    <td align="center" style="background:#ecf0f1; vertical-align:middle">
                                                        <div <%# Eval("IsUsingUDD").ToString() != "True" ? "Style='display:none'":"" %>>
                                                            <img id="imgStatusImageUri" runat="server" width="24" height="24" alt="" visible="true" />    
                                                        </div>
                                                    </td>
                                                    <td>        
                                                        <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID" class="hdnOrderDetailID"  />
                                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" class = "hdnItemID" />
                                                        <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" class="hdnDrugName"  />
                                                        <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" class="hdnParamedicName"  />
                                                        <input type="hidden" value="<%#:Eval("cfDispenseQuantity") %>" bindingfield="cfDispenseQuantity" class="hdnCfDispenseQuantity" />
                                                        <input type="hidden" value="<%#:Eval("cfTakenQuantity") %>" bindingfield="cfTakenQuantity" class="hdnCfTakenQuantity" />
                                                        <table>
                                                            <tr>
                                                                <td><div><b><%#: Eval("DrugName")%></b></div></td>
                                                                <td rowspan="2">&nbsp;</td>
                                                                <td rowspan="2">
                                                                    <div><img src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' alt="" style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "" %> max-width:30px; min-width: 30px; float: left;' /></div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div style="color: Blue; float: left;font-style:italic"><%#: Eval("GenericName")%></div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">           
                                                        <div><%#: Eval("Frequency")%></div>                                             
                                                    </td>
                                                    <td align="left">           
                                                        <div><%#: Eval("DosingFrequency")%></div>                                             
                                                    </td>
                                                    <td align="right">
                                                        <div><%#: Eval("cfNumberOfDosage")%></div>
                                                    </td>
                                                    <td align="left">
                                                        <div> <%#: Eval("DosingUnit")%></div>
                                                    </td>
                                                    <td align="right">
                                                        <div style="text-align: center;">
                                                            <asp:CheckBox ID="chkIsRequired" runat="server" Enabled="false" Checked='<%# Eval("IsAsRequired")%>' />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div><%#: Eval("Route")%></div>
                                                    </td>
                                                    <td align="center">
                                                        <div><%#: Eval("cfStartDate")%></div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <asp:CheckBox ID="chkIsMorning" runat="server" Enabled="false" Checked='<%# Eval("IsMorning")%>' />
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <asp:CheckBox ID="chkIsNoon" runat="server" Enabled="false" Checked='<%# Eval("IsNoon")%>' />
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <asp:CheckBox ID="chkEvening" runat="server" Enabled="false" Checked='<%# Eval("IsEvening")%>' />
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <asp:CheckBox ID="chkIsNight" runat="server" Enabled="false" Checked='<%# Eval("IsNight")%>' />
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div><%#: Eval("cfEndDate")%></div>
                                                    </td>
                                                    <td valign="middle" style="background:#ecf0f1">
                                                        <div style="text-align: right;color:Black">
                                                            <label class="lblDispenseQuantity lblLink"><%#:Eval("cfDispenseQuantity", "{0:N}")%></label>                                                    
                                                        </div>
                                                    </td>
                                                    <td valign="middle" style="background:#ecf0f1">
                                                        <div style="text-align: right;color:blue">
                                                            <label class="lblTakenQuantity lblLink"><%#:Eval("cfTakenQuantity", "{0:N}")%></label>                                                    
                                                        </div>
                                                    </td>
                                                    <td valign="middle" style="background:#ecf0f1">
                                                        <div style="text-align: right;color:Black">
                                                            <%#:Eval("cfRemainingQuantity", "{0:N}")%>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
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
