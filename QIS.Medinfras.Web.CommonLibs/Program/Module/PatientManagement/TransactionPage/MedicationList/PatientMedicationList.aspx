<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="PatientMedicationList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientMedicationList" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhHeader" runat="server">
    <style>
        .pnlContainerGridMedicationSchedule {
            width: 900px !important;
            overflow-x: auto;
        }
                
        .highlight    {  background-color:#FE5D15; color: White; }       
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnRefresh.ClientID %>').click(function () {
                onRefreshList();
            })

            $('.imgCompound').live('click', function () {
                var url = ResolveUrl("~/Program/Information/CompoundDetailCtl.ascx");
                var param = $('#<%=hdnPrescriptionOrderDtID.ClientID %>').val();
                openUserControlPopup(url, param, 'UDD - Compound Detail', 650, 300);
            });
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
            $('.containerPaging').hide();
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('.grdPrescriptionOrderDt tr:eq(1)').click();
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('.grdPrescriptionOrderDt tr:eq(1)').click();
        }
        //#endregion

        $('.lblPrescriptionOrder.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var itemID = $tr.find('.hdnItemID').val();
            var itemName = $tr.find('.hdnDrugName').val();
            var param = itemID + '|' + itemName;
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/MedicationList/MedicationOrderInfoCtl.ascx");
            openUserControlPopup(url, param, 'Medication - Order', 1000, 500);
        });

        $('.lblWardTransaction.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var itemID = $tr.find('.hdnItemID').val();
            var itemName = $tr.find('.hdnDrugName').val();
            var param = itemID + '|' + itemName;
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/MedicationList/MedicationChargesInfoCtl.ascx");
            openUserControlPopup(url, param, 'Medication - Service Unit Transaction', 800, 500);
        });

        $('.lblHomeMedication.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var id = $tr.find('.hdnItemID').val();
            var name = $tr.find('.hdnDrugName').val();
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/MedicationList/ExternalMedicationInfoCtl.ascx");
            var param = id + '|' + name;
            openUserControlPopup(url, param, 'Patient Home Medication', 750, 350);
        });

        function onRefreshList() {
            cbpView.PerformCallback('refresh');
            $('.containerPaging').hide();
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var hdnID = $('#<%=hdnID.ClientID %>').val();

            if (hdnID == '' || hdnID == '0') {
                if (code == 'PM-00425') {
                    filterExpression.text = registrationID;
                    return true;
                }
                else if (code == 'PH-00042') {
                    filterExpression.text = visitID;
                    return true;
                }
                else {
                    errMessage.text = 'Pasien tidak memiliki Resep';
                    return false;
                }
            }
        }
    </script>
    <input type="hidden" value="" id="hdnPrescriptionOrderDtID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
    <table style="width: 100%" >
        <tr>
            <td valign="top">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage2">
                                <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all">
                                            <tr>
                                                <th rowspan="2" align="left">
                                                    <div><%=GetLabel("Drug Name")%></div>
                                                </th>
                                                <th rowspan="2" style="width: 200px;">
                                                    <div style="text-align:left">
                                                        <%=GetLabel("Generic Name") %></div>
                                                </th>
                                                <th style="width: 150px;">
                                                    <div style="text-align:left">
                                                        <%=GetLabel("Route") %></div>
                                                </th>
                                                <th colspan="3">
                                                    <div style="text-align:center; font-weight:bold">
                                                        <%=GetLabel("DETAIL INFORMATION") %></div>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 110px;">
                                                    <div>
                                                        <%=GetLabel("Prescription") %></div>
                                                </th>
                                                <th style="width: 110px;">
                                                    <div>
                                                        <%=GetLabel("Unit Transaction") %></div>
                                                </th>
                                                <th style="width: 110px;">
                                                    <div>
                                                        <%=GetLabel("Home Medication") %></div>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all">
                                            <tr>
                                                <th rowspan="2" align="left">
                                                    <div><%=GetLabel("Drug Name")%></div>
                                                </th>
                                                <th rowspan="2" style="width: 200px;">
                                                    <div style="text-align:left">
                                                        <%=GetLabel("Generic Name") %></div>
                                                </th>
                                                <th rowspan="2" style="width: 150px;">
                                                    <div style="text-align:left">
                                                        <%=GetLabel("Route") %></div>
                                                </th>
                                                <th colspan="3">
                                                    <div style="text-align:center; font-weight:bold">
                                                        <%=GetLabel("DETAIL INFORMATION") %></div>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 110px;">
                                                    <div>
                                                        <%=GetLabel("Prescription") %></div>
                                                </th>
                                                <th style="width: 110px;">
                                                    <div>
                                                        <%=GetLabel("Unit Transaction") %></div>
                                                </th>
                                                <th style="width: 110px;">
                                                    <div>
                                                        <%=GetLabel("Home Medication") %></div>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trItemName" runat="server">
                                            <td>
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" class = "hdnItemID" />
                                                <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" class="hdnDrugName"  />
                                                <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                <input type="hidden" value="<%#:Eval("IsCompound") %>" bindingfield="IsCompound" />
                                                <input type="hidden" value="<%#:Eval("GCMedicationRoute") %>" bindingfield="GCMedicationRoute" />
                                                <input type="hidden" value="<%#:Eval("MedicationRoute") %>" bindingfield="MedicationRoute" />
                                                <table>
                                                    <tr>
                                                        <td><div style="font-size:11pt;font-weight:bold;padding-bottom:1px"><%#: Eval("DrugName")%></div></td>
                                                        <td rowspan="2">&nbsp;</td>
                                                        <td rowspan="2">
                                                            <div><img src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' alt="" style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "" %> max-width:30px; min-width: 30px; float: left;' /></div>
                                                        </td>
                                                    </tr>
                                                   </table>
                                            </td>
                                            <td>
                                                <div><%#: Eval("GenericName")%></div>
                                            </td>
                                            <td>
                                                <div><%#: Eval("MedicationRoute")%></div>
                                            </td>
                                            <td valign="middle" style="background:#ecf0f1">
                                                <div <%# Eval("IsHomeMedication").ToString() == "True" ? "Style='display:none'":"style='display:block;text-align: center;color:blue'" %>>
                                                    <label class="lblPrescriptionOrder lblLink">View</label>                                                    
                                                </div>
                                            </td>
                                            <td valign="middle" style="background:#ecf0f1">
                                                <div <%# Eval("IsHomeMedication").ToString() == "True" ? "Style='display:none'":"style='display:block;text-align: center;color:blue'" %>>
                                                    <label class="lblWardTransaction lblLink">View</label>                                                    
                                                </div>
                                            </td>
                                            <td valign="middle" style="background:#ecf0f1">
                                                <div <%# Eval("IsHomeMedication").ToString() != "True" ? "Style='display:none'":"style='display:block;text-align: center;color:blue'" %>>
                                                    <label class="lblHomeMedication lblLink">View</label>                                                    
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="paging">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
