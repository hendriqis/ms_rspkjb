<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoVoidAppointmentCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoVoidAppointmentCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_patientvisitctl">
    function onCboInfoVoidAppointmentViewValueChanged(s) {
        cbpInfoVoidAppointmentView.PerformCallback('refresh');
    }

//    setDatePicker('<%=txtInfoVoidAppointmentDate.ClientID %>');
//    $('#<%=txtInfoVoidAppointmentDate.ClientID %>').datepicker('option', 'maxDate', '0');

//    $('#<%=txtInfoVoidAppointmentDate.ClientID %>').change(function (evt) {
//        cbpInfoVoidAppointmentView.PerformCallback('refresh');
//    });

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingInfoVoidAppointmentView"), pageCount, function (page) {
            cbpInfoVoidAppointmentView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpInfoVoidAppointmentViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);

            setPaging($("#pagingInfoVoidAppointmentView"), pageCount, function (page) {
                cbpInfoVoidAppointmentView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

</script>
<table>
    <colgroup>
        <col style="width:130px"/>
        <col/>
    </colgroup>
    <tr>
        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal Perjanjian")%></label></td>
        <td>
            <asp:TextBox ID="txtInfoVoidAppointmentDate" Width="400px" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Klinik")%></label></td>
        <td>
            <asp:TextBox ID="txtServiceUnitName" Width="400px" runat="server" />
        </td>
    </tr>
    <tr>
        <td><label><%=GetLabel("Paramedis") %></label></td>
        <td>
            <asp:TextBox ID="txtParamedicId" Width="400px" runat="server" />
        </td>
    </tr>
</table>
<input type="hidden" id="hdnParamedicID" runat="server" />
<input type="hidden" id="hdnAppointmentDate" runat="server" />
<input type="hidden" id="hdnServiceUnit" runat="server" />
<div style="position: relative;">
    <dxcp:ASPxCallbackPanel ID="cbpInfoVoidAppointmentView" runat="server" Width="100%" ClientInstanceName="cbpInfoVoidAppointmentView"
        ShowLoadingPanel="false" OnCallback="cbpInfoVoidAppointmentView_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
            EndCallback="function(s,e){ onCbpInfoVoidAppointmentViewEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent" runat="server">
                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:320px;font-size:1em;">
                    <asp:ListView runat="server" ID="lvwView">
                         <EmptyDataTemplate>
                            <table id="tblView" runat="server" class="grdSelected" cellspacing="0" rules="all" >
                                <tr>
                                    <th style="width:110px"><%=GetLabel("No. Appointment")%></th>
                                    <th style="width:110px"><%=GetLabel("Data Pasien")%></th>
                                    <th style="width:110px"><%=GetLabel("Nama Paramedis")%></th>
                                    <th style="width:110px"><%=GetLabel("Jam Mulai")%></th>
                                    <th style="width:110px"><%=GetLabel("Jam Selesai")%></th>
                                    <th style="width:110px"><%=GetLabel("Alasan Batal")%></th>
                                </tr>
                                <tr class="trEmpty">
                                    <td colspan="8">
                                        <%=GetLabel("No Data To Display")%>
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                       <LayoutTemplate>
                            <table id="tblView" runat="server" class="grdNormal" cellspacing="0" rules="all" >
                                <tr>
                                    <th style="width:110px"><%=GetLabel("No. Appointment")%></th>
                                    <th style="width:110px"><%=GetLabel("Data Pasien")%></th>
                                    <th style="width:110px"><%=GetLabel("Nama Paramedis")%></th>
                                    <th style="width:110px"><%=GetLabel("Jam Mulai")%></th>
                                    <th style="width:110px"><%=GetLabel("Jam Selesai")%></th>
                                    <th style="width:110px"><%=GetLabel("Alasan Batal")%></th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder" ></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%#: Eval("AppointmentNo")%></td>
                                <td><%#: Eval("cfPatientName")%></td>
                                <td><%#: Eval("ParamedicName")%></td>
                                <td><%#: Eval("StartTime")%></td>
                                <td><%#: Eval("EndTime")%></td>
                                <td><%#: Eval("DeleteReason")%></td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>    
    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
    <div class="containerPaging">
        <div class="wrapperPaging">
            <div id="pagingInfoVoidAppointmentView"></div>
        </div>
    </div> 
</div>