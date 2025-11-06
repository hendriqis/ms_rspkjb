<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicationOrderInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicationOrderInfoCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
    });

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpView.PerformCallback('refresh');
    }

    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingDt"), pageCount, function (page) {
            cbpViewDt.PerformCallback('changepage|' + page);
        });
    });

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
</script>
<input type="hidden" runat="server" id="hdnItemID" value="" />
<input type="hidden" runat="server" id="hdnItemName" value="" />
<input type="hidden" runat="server" id="hdnSelectedScheduleID" value="" />
<input type="hidden" runat="server" id="hdnPrescriptionOrderDetailID" value="" />
<input type="hidden" runat="server" id="hdnOldFrequency" value="" />
<div>
    <table class="tblEntryContent" style="width:70%">
        <colgroup>
            <col style="width:120px"/>
            <col/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Item Name")%></label></td>
            <td><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
        </tr>  
    </table>

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
                                            <th rowspan="2" align="left">
                                                <div><%=GetLabel("Prescribed By")%></div>
                                            </th>
                                            <th colspan="6" align="center">
                                                <div>
                                                    <%=GetLabel("Signa")%></div>
                                            </th>
                                            <th rowspan="2" align="center" style="padding: 3px; width: 90px;">
                                                <div>
                                                    <%=GetLabel("Start Date")%></div>
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
                                                <div><%=GetLabel("Prescribed By")%></div>
                                            </th>
                                            <th colspan="6" align="center">
                                                <div>
                                                    <%=GetLabel("Signa")%></div>
                                            </th>
                                            <th rowspan="2" align="center" style="padding: 3px; width: 90px;">
                                                <div>
                                                    <%=GetLabel("Start Date")%></div>
                                            </th>
                                            <th rowspan="2" style="width: 90px;">
                                                <div style="text-align:center">
                                                    <%=GetLabel("End Date") %></div>
                                            </th>
                                            <th rowspan="2">
                                                <div style="text-align:center; font-weight:bold">
                                                    <%=GetLabel("ADMINISTRATION") %></div>
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
                                                    <td><div><b><%#: Eval("ParamedicName")%></b></div></td>
                                                    <td rowspan="2">&nbsp;</td>
                                                    <td rowspan="2">
                                                        <div><img src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' alt="" style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "" %> max-width:30px; min-width: 30px; float: left;' /></div>
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
                                            <div><%#: Eval("cfEndDate")%></div>
                                        </td>
                                        <td valign="middle" style="background:#ecf0f1">
                                            <div style="text-align: right;color:blue">
                                                <label class="lblTakenQuantity lblLink"><%#:Eval("cfTakenQuantity", "{0:N}")%></label>                                                    
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
</div>
