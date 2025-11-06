<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="MedicationScheduleProcess.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.MedicationScheduleProcess" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/Prescription/UDD/UDDToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessMedicationOrder" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Dispense") %></div>
    </li>
    <li id="btnDiscontinueMedicationOrder" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Discontinue") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhHeader" runat="server">
    <style>
        .pnlContainerGridMedicationSchedule {
            width: 900px !important;
            overflow-x: auto;
        }
        
        .tdDrugInfoLabel 
        {
            text-align:right;
            vertical-align:top;
            font-size:0.9em;
            font-style:italic
        }
        
        .tdDrugInfoValue 
        {
            vertical-align:top;
            font-weight:bold;            
        }        
        
        .txtMedicationTime
        {
            width:70px;
            text-align:center;
            color: Blue;
            font-size:larger
        }
        
        .highlight    {  background-color:#FE5D15; color: White; }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.grdPrescriptionOrderDt > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('.grdPrescriptionOrderDt tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnPrescriptionOrderDtID.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewDt.PerformCallback('refresh');
                }
            });

            $('#<%=btnProcessMedicationOrder.ClientID %>').click(function () {
                getCheckedMember();
                var url = ResolveUrl("~/Program/Prescription/UDD/MedicationProcess/ProcessMedicationScheduleCtl.ascx");
                var serviceUnitID = $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val();
                var prescriptionFeeAmount = $('#<%=hdnPrescriptionFeeAmount.ClientID %>').val();
                var param = serviceUnitID + '|' + prescriptionFeeAmount;
                openUserControlPopup(url, param, 'UDD - Medication Process', 650, 550);
            });

            $('#<%=btnDiscontinueMedicationOrder.ClientID %>').click(function () {
                alert('Discontinue Medication');
            });
        });

        $('.grdPrescriptionOrderDt tr:first').click();

        $('.chkIsSelected input').live('change', function () {
            $tr = $(this).closest('tr');

            var $cell = $(this).closest("td");
            $txtMedicationTime = $tr.find('.txtMedicationTime');
            if ($(this).is(':checked')) {
                $cell.addClass('highlight');
                $txtMedicationTime.addClass('highlight');
            }
            else {
                $cell.removeClass('highlight');
                $txtMedicationTime.removeClass('highlight');
                $txtMedicationTime.val('00:00');
            }
        });

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split('|');
            $('#<%=lvwViewDt.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.txtMedicationTime').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.txtMedicationTime').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join('|'));
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
                    $('.grdPrescriptionOrderDt tr:eq(1)').click();
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('.grdPrescriptionOrderDt tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnPrescriptionOrderDtID" runat="server" />
    <input type="hidden" value="" id="hdnDispensaryServiceUnitID" runat="server" />
    <input type="hidden" value="0" id="hdnPrescriptionFeeAmount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 35%" />
            <col style="width: 65%" />
        </colgroup>
        <tr>
            <td valign="top">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage2">
                                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                    <emptydatatemplate>
                                        <table id="tblView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all">
                                            <tr>
                                                <th class="keyField" rowspan="2">&nbsp;</th>
                                                <th align="left"><%=GetLabel("Medication Schedule")%></th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td>
                                                    <%=GetLabel("No record to display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </emptydatatemplate>
                                    <layouttemplate>
                                        <table id="tblView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all" width="100%">
                                            <tr>
                                                <th class="keyField" rowspan="2">&nbsp;</th>
                                                <th colspan="2" align="left">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <colgroup>
                                                            <col width="60%" />
                                                            <col />                                                            
                                                        </colgroup>
                                                        <tr>
                                                            <td style="vertical-align:top;"><%=GetLabel("Drug Name")%></td>
                                                            <td>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <colgroup>
                                                                        <col style="width:10px;" />
                                                                        <col style="width:5px ; text-align:center" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td><div style="width:10px;background-color:#0066FF">&nbsp;</div></td>
                                                                        <td style="padding-left:2px;font-size:0.9em;font-style:italic">:</td>
                                                                        <td style="padding-left:2px;font-size:0.9em;font-style:italic"><%=GetLabel("Obat Unit Farmasi")%></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td><div style="width:10px;background-color:Red">&nbsp;</div></td>
                                                                        <td style="padding-left:2px;font-size:0.9em;font-style:italic">:</td>
                                                                        <td style="padding-left:2px;font-size:0.9em;font-style:italic"><%=GetLabel("Obat milik Pasien")%></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>                                                    
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </layouttemplate>
                                    <itemtemplate>
                                        <tr>
                                            <td class="keyField"><%#: Eval("PrescriptionOrderDetailID")%></td>
                                            <td id="tdIndicator" runat="server" style="width:10px; background:gray">
                                            </td>
                                            <td>
                                                <div style="font-size:1.1em;color:#0066FF;font-weight:bold;padding-bottom:5px"><%#: Eval("DrugName")%></div>
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <colgroup>
                                                        <col style="width:80px"/>
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
                                                        <td class="tdDrugInfoValue">
                                                            <%#: Eval("GenericName")%>
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
                                                            <%=GetLabel("Signa")%>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td class="tdDrugInfoValue" colspan="4">
                                                            <span><%#: Eval("cfSignaRule")%></span>&nbsp;&nbsp;<asp:CheckBox ID="chkIsAsRequired" runat="server" Enabled="false" Checked='<%# Eval("IsAsRequired")%>' Text="PRN" />
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
                                            </td>
                                        </tr>
                                    </itemtemplate>
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
            <td valign="top">
                <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                    ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                        EndCallback="function(s,e){ $('#containerImgLoadingViewDt').hide(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage2 pnlContainerGridMedicationSchedule">
                                <table id="grdViewDt" class="grdSelected grdPatientPage" cellspacing="0" rules="all" style="overflow: auto;" width="100%">
                                    <tr>
                                        <th style="width:20px">
                                            &nbsp;
                                        </th>
                                        <asp:Repeater ID="rptMedicationDateHeader" runat="server">
                                            <ItemTemplate>
                                                <th style="width: 90px; font-weight:bold" align="center">
                                                    <%#: Eval("MedicationDateInString")%>
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <th>
                                            &nbsp;
                                        </th>
                                    </tr>
                                    <asp:ListView ID="lvwViewDt" runat="server" OnItemDataBound="lvwViewDt_ItemDataBound">
                                        <LayoutTemplate>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="text-align:center; font-weight:bold; vertical-align:middle">
                                                    <%#: Eval("SequenceNo")%>
                                                    <input type="hidden" value="<%#:Eval("SequenceNo") %>" class="SequenceNo" />
                                                </td>
                                                <asp:Repeater ID="rptMedicationTimeDetail" runat="server">
                                                    <ItemTemplate>
                                                        <td align="left">
                                                            <table class="rptMedicationTimeDetail" border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <div <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>>
                                                                            <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                                        </div>
                                                                    </td>
                                                                    <td style="width:80px" class="tdMedicationTime">
                                                                        <div <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>>
                                                                            <input type="text" class="txtMedicationTime" value="<%#:Eval("MedicationTime") %>" />
                                                                        </div>
                                                                        <input type="hidden" value="<%#:Eval("ID") %>" class="ID" />
                                                                        <input type="hidden" value="<%#:Eval("MedicationDateInString") %>" class="MedicationDateInString" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <tr class="trEmpty">
                                                <td colspan="10">
                                                    <%=GetLabel("Tidak ada pasien yang sedang dalam perawatan") %>
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </table>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingDt">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
