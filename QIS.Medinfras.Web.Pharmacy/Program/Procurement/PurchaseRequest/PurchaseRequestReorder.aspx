<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="PurchaseRequestReorder.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.PurchaseRequestReorder" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPurchaseRequestReorderCalculate" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Calculate")%></div></li>
    <li id="btnPurchaseRequestReorderProcess" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Process")%></div></li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtTransactionDate.ClientID %>');

            //#region Location
            $('#lblLocation.lblLink').click(function () {
                var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND (IsHeader = 0 OR (IsHeader = 1 AND IsHasChildren = 1))";
                openSearchDialog('location', filterExpression, function (value) {
                    $('#<%=txtLocationCode.ClientID %>').val(value);
                    onTxtLocationCodeChanged(value);
                });
            });

            $('#<%=txtLocationCode.ClientID %>').change(function () {
                onTxtLocationCodeChanged($(this).val());
            });

            function onTxtLocationCodeChanged(value) {
                var filterExpression = "LocationCode = '" + value + "'";
                Methods.getObject('GetLocationList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);
                        showLoadingPanel();
                        cboLocation.PerformCallback();
                    }
                    else {
                        $('#<%=hdnLocationID.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                        cboLocation.ClearItems();
                    }
                });
            }
            //#endregion
        }
    </script> 
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />  
    <div style="height:500px;overflow-y:auto;">
        <div class="pageTitle">
            <div style="font-size: 1em"><%=GetLabel("Purchase Request Reorder")%></div>
        </div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width:50%"/>
                <col style="width:50%"/>
            </colgroup>
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                            <col/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Date") %></td>
                            <td><asp:TextBox ID="txtTransactionDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblLink" id="lblLocation"><%=GetLabel("Warehouse")%></label></td>
                            <td>
                                <input type="hidden" id="hdnLocationID" value="" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtLocationCode" CssClass="required" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtLocationName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Location") %></td>
                            <td> 
                                <dxe:ASPxComboBox ID="cboLocation" runat="server" ClientInstanceName="cboLocation" Width="200px" OnCallback="cboLocation_Callback">
                                    <ClientSideEvents EndCallback="function(s,e) { hideLoadingPanel(); onCboLocationValueChanged(); }" 
                                        ValueChanged="function(s,e){ onCboLocationValueChanged(); }" />
                                </dxe:ASPxComboBox>    
                            </td>
                        </tr>
                    </table>
                    <div style="padding:7px 0 0 3px;font-size:0.95em">
                        <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("Setiap 10 Menit")%>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                    <input type="hidden" id="hdnServiceAllTotalPatient" runat="server" value="" />
                                    <input type="hidden" id="hdnServiceAllTotalPayer" runat="server" value="" />
                                    <asp:ListView ID="lvwService" runat="server">
                                        <LayoutTemplate>                                
                                            <table id="tblView" runat="server" class="grdService grdNormal notAllowSelect" cellspacing="0" rules="all" >
                                                <tr>  
                                                    <th style="width:80px" rowspan="2"></th>
                                                    <th style="width:100px" rowspan="2">
                                                        <div style="text-align:left;padding-left:3px">
                                                            <%=GetLabel("Code")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div style="text-align:left;padding-left:3px">
                                                            <%=GetLabel("Description")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width:90px">
                                                        <div style="text-align:right;padding-right:3px">
                                                            <%=GetLabel("Unit Tariff")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width:50px">
                                                        <div style="text-align:right;padding-right:3px">
                                                            <%=GetLabel("Qty")%>
                                                        </div>
                                                    </th>
                                                    <th colspan="4"><%=GetLabel("Amount")%></th>
                                                    <th colspan="3"><%=GetLabel("Line Amount")%></th>
                                                    <th rowspan="2" style="width:100px">
                                                        <div style="text-align:right;padding-right:3px">
                                                            <%=GetLabel("Registrar")%>
                                                        </div>
                                                    </th>                                
                                                </tr>
                                                <tr>
                                                    <th style="width:90px">
                                                        <div style="text-align:right;padding-right:3px">
                                                            <%=GetLabel("Tariff")%>
                                                        </div>
                                                    </th>
                                                    <th style="width:90px">
                                                        <div style="text-align:right;padding-right:3px">
                                                            <%=GetLabel("CITO")%>
                                                        </div>
                                                    </th>
                                                    <th style="width:90px">
                                                        <div style="text-align:right;padding-right:3px">
                                                            <%=GetLabel("Complication")%>
                                                        </div>
                                                    </th>
                                                    <th style="width:90px">
                                                        <div style="text-align:right;padding-right:3px">
                                                            <%=GetLabel("Discount")%>
                                                        </div>
                                                    </th>
                                                    <th style="width:100px">
                                                        <div style="text-align:right;padding-right:3px">
                                                            <%=GetLabel("Payer")%>
                                                        </div>
                                                    </th>
                                                    <th style="width:100px">
                                                        <div style="text-align:right;padding-right:3px">
                                                            <%=GetLabel("Patient")%>
                                                        </div>
                                                    </th>
                                                    <th style="width:100px">
                                                        <div style="text-align:right;padding-right:3px">
                                                            <%=GetLabel("Total")%>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder" ></tr>
                                                <tr id="Tr1" class="trFooter" runat="server">
                                                    <td colspan="9" align="right" style="padding-right:3px"><%=GetLabel("Total") %></td>
                                                    <td align="right" style="padding-right:3px" id="tdServiceTotalPayer" class="tdServiceTotalPayer" runat="server"></td>
                                                    <td align="right" style="padding-right:3px" id="tdServiceTotalPatient" class="tdServiceTotalPatient" runat="server"></td>
                                                    <td align="right" style="padding-right:3px" id="tdServiceTotal" class="tdServiceTotal" runat="server"></td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <div>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td><img class="imgServiceEdit imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" /></td>
                                                                <td style="width:1px">&nbsp;</td>
                                                                <td><img class="imgServiceDelete imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
                                                            </tr>
                                                        </table>
                                                        <input type="hidden" value='<%# Eval("ID") %>' bindingfield="ID" />
                                                        <input type="hidden" value='<%# Eval("ItemID") %>' bindingfield="ItemID" />
                                                        <input type="hidden" value='<%# Eval("ItemName1") %>' bindingfield="ItemName1" />
                                                        <input type="hidden" value='<%# Eval("ParamedicID") %>' bindingfield="ParamedicID" />
                                                        <input type="hidden" value='<%# Eval("ChargedQuantity") %>' bindingfield="ChargedQuantity" />
                                                        <input type="hidden" value='<%# Eval("BaseTariff") %>' bindingfield="BaseTariff" />
                                                        <input type="hidden" value='<%# Eval("Tariff") %>' bindingfield="Tariff" />
                                                        <input type="hidden" value='<%# Eval("GrossLineAmount") %>' bindingfield="GrossLineAmount" />
                                                        <input type="hidden" value='<%# Eval("IsAllowCITO") %>' bindingfield="IsAllowCITO" />
                                                        <input type="hidden" value='<%# Eval("IsAllowComplication") %>' bindingfield="IsAllowComplication" />
                                                        <input type="hidden" value='<%# Eval("IsAllowDiscount") %>' bindingfield="IsAllowDiscount" />
                                                        <input type="hidden" value='<%# Eval("IsAllowVariable") %>' bindingfield="IsAllowVariable" />
                                                        <input type="hidden" value='<%# Eval("IsCITO") %>' bindingfield="IsCITO" />
                                                        <input type="hidden" value='<%# Eval("IsCITOInPercentage") %>' bindingfield="IsCITOInPercentage" />
                                                        <input type="hidden" value='<%# Eval("IsComplication") %>' bindingfield="IsComplication" />
                                                        <input type="hidden" value='<%# Eval("IsComplicationInPercentage") %>' bindingfield="IsComplicationInPercentage" />
                                                        <input type="hidden" value='<%# Eval("IsDiscount") %>' bindingfield="IsDiscount" />
                                                        <input type="hidden" value='<%# Eval("IsVariable") %>' bindingfield="IsVariable" />
                                                        <input type="hidden" value='<%# Eval("BaseCITOAmount") %>' bindingfield="BaseCITOAmount" />
                                                        <input type="hidden" value='<%# Eval("CITOAmount") %>' bindingfield="CITOAmount" />
                                                        <input type="hidden" value='<%# Eval("DiscountAmount") %>' bindingfield="DiscountAmount" />
                                                        <input type="hidden" value='<%# Eval("BaseComplicationAmount") %>' bindingfield="BaseComplicationAmount" />
                                                        <input type="hidden" value='<%# Eval("ComplicationAmount") %>' bindingfield="ComplicationAmount" />
                                                        <input type="hidden" value='<%# Eval("PatientAmount") %>' bindingfield="PatientAmount" />
                                                        <input type="hidden" value='<%# Eval("PayerAmount") %>' bindingfield="PayerAmount" />
                                                        <input type="hidden" value='<%# Eval("LineAmount") %>' bindingfield="LineAmount" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding:3px;">
                                                        <div class="divTransactionNo"><%# Eval("ItemCode") %></div>                                         
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding:3px">
                                                        <div><%# Eval("ItemName1")%></div>
                                                        <div><%# Eval("ParamedicName")%></div>                                                    
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding:3px;text-align:right;">
                                                        <div><%# Eval("Tariff", "{0:N}")%></div>                                                   
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding:3px;text-align:right;">
                                                        <div><%# Eval("ChargedQuantity")%></div>                                                   
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding:3px;text-align:right;">
                                                        <div><%# Eval("GrossLineAmount", "{0:N}")%></div>                                                   
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding:3px;text-align:right;">
                                                        <div><%# Eval("CITOAmount", "{0:N}")%></div>                                                   
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding:3px;text-align:right;">
                                                        <div><%# Eval("ComplicationAmount", "{0:N}")%></div>                                                   
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding:3px;text-align:right;">
                                                        <div><%# Eval("DiscountAmount", "{0:N}")%></div>                                                   
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding:3px;text-align:right;">
                                                        <div><%# Eval("PayerAmount", "{0:N}")%></div>                                                   
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding:3px;text-align:right;">
                                                        <div><%# Eval("PatientAmount", "{0:N}")%></div>                                                   
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding:3px;text-align:right;">
                                                        <div><%# Eval("LineAmount", "{0:N}")%></div>                                                   
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding-right:3px;text-align:right;">
                                                        <div><%# Eval("CreatedByUserName")%></div>
                                                        <div><%# Eval("CreatedDateInString")%></div>                                                 
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingService">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>         
    </div>
</asp:Content>
