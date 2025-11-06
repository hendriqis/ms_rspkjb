<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master" AutoEventWireup="true" 
    CodeBehind="MedicationPRNList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicationPRNList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
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
    <li id="btnCompleted" crudmode="R" runat="server" visible="false">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Completed") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhHeader" runat="server">
    <style>        
        .tdDrugInfoLabel 
        {
            text-align:right;
            vertical-align:top;
            font-size:11pt;
            font-style:italic
        }
        
        .tdDrugInfoValue 
        {
            vertical-align:top;
            font-size:11pt;
            font-weight:bold;            
        }        
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnPrescriptionOrderDtID.ClientID %>').val($(this).find('.keyField').html());
                    $('#<%=hdnPastMedicationID.ClientID %>').val($(this).find('.hiddenColumn').html());
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
            cbpView.PerformCallback('refresh');
            $('.containerPaging').hide();
        }
        
        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
            $('.containerPaging').hide();
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

        $('#lblAddData').live('click', function () {
            var prescriptionOrderDtID = $('#<%=hdnPrescriptionOrderDtID.ClientID %>').val();
            var pastMedicationID = $('#<%=hdnPastMedicationID.ClientID %>').val();

            if (prescriptionOrderDtID != '' && pastMedicationID != '') {
                showLoadingPanel();
                var id = prescriptionOrderDtID + '|' + pastMedicationID;
                var url = ResolveUrl('~/libs/Program/Module/PatientManagement/TransactionPage/UDDProcess/AddMedicationAdministrationCtl.ascx');
                openUserControlPopup(url, id, 'Medication Administration', 550, 350);
            }
            else {
                alert('There is no as required medication for this patient.');
            }
        });

        $('.imgPopupDelete.imgLink').die('click');
        $('.imgPopupDelete.imgLink').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            var id = $tr.find('.keyField').html();
            $('#<%=hdnID.ClientID %>').val(id);
            showDeleteConfirmation(function (data) {
                var param = id + ';' + data.GCDeleteReason + ';' + data.Reason;
                cbpPopupProcess.PerformCallback("delete" + "|" + param);
            });
        });

        $('.imgPopupEdit.imgLink').die('click');
        $('.imgPopupEdit.imgLink').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            var id = $tr.find('.keyField').html();
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/UDDProcess/EditMedicationScheduleCtl.ascx");
            var serviceUnitID = "0";
            var param = serviceUnitID + '|' + id;
            openUserControlPopup(url, param, 'Medication Administration', 550, 350);
        });

        function onCbpPopupProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else {
                    cbpViewDt.PerformCallback('refresh');
                }
            }
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionOrderDtID" runat="server" />
    <input type="hidden" value="" id="hdnPastMedicationID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:35%"/>
            <col style="width:65%"/>
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
                                    <asp:GridView runat="server" ID="grdView" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="PrescriptionOrderDetailID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="PastMedicationID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Drug Name")%> 
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr id="trItemName" runat="server">
                                                            <td width="95%">
                                                                <div style="font-size:14pt;color:#0066FF;font-weight:bold;padding-bottom:5px"><%#: Eval("DrugName")%></div>
                                                            </td>
                                                            <td>
                                                                <div><img class="imgCompound" src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' alt="" style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "" %> max-width:30px; cursor:pointer; min-width: 30px; float: left;' /></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <colgroup>
                                                            <col style="width:120px"/>
                                                            <col style="width:5px"/>
                                                            <col />
                                                            <col style="width:80px"/>
                                                            <col style="width:5px"/>
                                                            <col style="width:100px"/>
                                                        </colgroup>
                                                        <tr>
                                                            <td class="tdDrugInfoLabel">
                                                                <%=GetLabel("Generic Name")%>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                            <td class="tdDrugInfoValue" colspan="5">
                                                                <%#: Eval("GenericName")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdDrugInfoLabel">
                                                                <%=GetLabel("Dosis")%>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                            <td class="tdDrugInfoValue" colspan="4">
                                                                <span><%#: Eval("cfNumberOfDosage")%></span>&nbsp;&nbsp;<asp:CheckBox ID="chkIsAsRequired" runat="server" Enabled="false" Checked='<%# Eval("IsAsRequired")%>' Text="PRN" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdDrugInfoLabel">
                                                                <%=GetLabel("Route")%>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                            <td class="tdDrugInfoValue">
                                                                <%#: Eval("Route")%>
                                                            </td>
                                                            <td class="tdDrugInfoLabel">
                                                                <%=GetLabel("Start Date")%>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                            <td class="tdDrugInfoValue">
                                                                <%#: Eval("cfStartDate")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdDrugInfoLabel">
                                                                <%=GetLabel("Special Instruction")%>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                            <td class="tdDrugInfoValue">
                                                                <span style="color:red"><%#: Eval("MedicationAdministration")%></span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdDrugInfoLabel">
                                                                <%=GetLabel("Physician")%></div>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                            <td class="tdDrugInfoValue" colspan="4">
                                                                <%#: Eval("ParamedicName")%>
                                                            </td>
                                                        </tr>
                                                    </table>                                                
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div style="font-weight:bold; color:red;font-size:1.1em;padding-top:7px"><%=GetLabel("no medication schedulled for this patient")%></div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging" style="display:none">
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
                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td><img class="imgPopupEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" /></td>
                                                            <td><img class="imgPopupDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="MedicationDateInString" HeaderStyle-Width="80px" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center" />
                                            <asp:BoundField DataField="cfMedicationTime" HeaderStyle-Width="50px" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    <%=GetLabel("Dosis") %>    
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div> <%#:Eval("cfNumberOfDosage") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    <%=GetLabel("Unit") %>    
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div> <%#:Eval("DosingUnit") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Catatan Pemberian" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" >
                                                <ItemTemplate>
                                                    <div><%#:Eval("OtherMedicationStatus") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Diberikan Oleh" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" >
                                                <ItemTemplate>
                                                    <div><%#:Eval("ParamedicName") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <span style="font-weight:bold; font-size:11pt"><%=GetLabel("Belum ada pemberian kepada pasien untuk obat yang sedang dipilih")%></span>
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
                    <div style='width: 100%; text-align: center; padding-top:10px'>
                        <span class="lblLink" style="margin-right: 100px;" id="lblAddData"><%= GetLabel("Tambah Pemberian Obat")%></span>
                    </div>   
                </div>
            </td>
        </tr>
    </table>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }"
            EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
