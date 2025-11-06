<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridChangeRegistrationPayerCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Finance.Controls.GridChangeRegistrationPayerCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_gridGridchangeregistrationpayerctl">
    $('.lvwView > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
        showLoadingPanel();
        $('#<%=hdnTransactionNo.ClientID %>').val($(this).find('.hdnVisitID').val());
        __doPostBack('<%=btnOpenTransactionDt.UniqueID%>', '');
    });

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function refreshGrdRegisteredPatient() {
        cbpView.PerformCallback('refresh');
    }

    function onBeforeOpenTransactionDt() {
        return ($('#<%=hdnTransactionNo.ClientID %>').val() != '');
    }
</script>

<div style="display:none"><asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();" OnClick="btnOpenTransactionDt_Click" /></div>
<input type="hidden" runat="server" id="hdnTransactionNo" value="" />

<dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height:365px;overflow-y:scroll;">
                <asp:ListView runat="server" ID="lvwView">
                    <EmptyDataTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0" rules="all" >
                            <tr>
                                <th style="width:300px" align="left"><%=GetLabel("INFORMASI PENDAFTARAN")%></th>
                                <th align="left"><%=GetLabel("INFORMASI PASIEN")%></th>
                                <th style="width:300px" align="left"><%=GetLabel("INFORMASI PEMBAYAR")%></th>
                                <th style="width:30px"></th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="10">
                                    <%=GetLabel("Tidak ada informasi pendaftaran pasien pada tanggal yang dipilih")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0" rules="all" >
                            <tr>
                                <th style="width:300px" align="left"><%=GetLabel("INFORMASI PENDAFTARAN")%></th>
                                <th align="left"><%=GetLabel("INFORMASI PASIEN")%></th>
                                <th style="width:300px" align="left"><%=GetLabel("INFORMASI PEMBAYAR")%></th>
                                <th style="width:30px"></th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder" ></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                <div><b><%#: Eval("RegistrationNo") %></b></div>
                                <div><%=GetLabel("Tgl. Registrasi : ")%><%#: Eval("cfActualVisitDateInString") %></div>        
                                <div>(<%#: Eval("DepartmentID") %>) <%#: Eval("ServiceUnitName") %></div>        
                                <div>(<%#: Eval("ParamedicCode") %>) <%#: Eval("ParamedicName") %></div>                                    
                            </td>
                            <td>
                                <div><b><%#: Eval("PatientName") %></b> (<%#: Eval("cfDateOfBirthInString") %>, <%#: Eval("Gender") %>, <%#: Eval("MedicalNo") %>)</div>                                           
                            </td>
                            <td>
                                <div><%#: Eval("BusinessPartnerName")%> (<%#: Eval("CustomerType") %>)</div>
                            </td>
                            <td align="center">
                                <img class="imgLock" title='<%=GetLabel("TransactionLock")%>' src=' <%# ResolveUrl("~/Libs/Images/Toolbar/unlockdown.png") %>' style='<%# Eval("IsLockDown").ToString() == "True" ? "width:25px" : "width:25px; display:none" %>'
                                alt="" />
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