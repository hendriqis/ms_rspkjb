<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientInformationPerDoctorDt.ascx.cs" 
Inherits="QIS.Medinfras.Web.EMR.Program.PatientInformationPerDoctorDt" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascipt" id="dxss_patientinformationperdoctordt">

</script>

<input type="hidden" runat="server" value="" id="hdnDepartmentID" />
<input type="hidden" runat="server" value="" id="hdnParamedicIDCtl" />
<input type="hidden" runat="server" value="" id="hdnVisitYear" />
<input type="hidden" runat="server" value="" id="hdnVisitMonth" />
<div class="pageTitle"><%=GetLabel("Detail Information") %></div>
<table class="tblContentArea">
    <tr>
        <td>
            <table class="tblEntryContent">
                <colgroup>
                    <col style="width:160px" />
                    <col/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Department") %></label></td>
                    <td><asp:TextBox ID="txtDepartmentName" ReadOnly="true" Width="180px" runat="server"/></td>
                </tr>
            </table>
            <div style="position:relative; margin-top:10px;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopView" ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" style="height:330px; overflow-y:scroll;">
                                <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" />
                                        <asp:BoundField DataField="VisitDateInString" HeaderText="Tanggal Kunjungan" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"/>
                                        <asp:BoundField DataField= "VisitStatus" HeaderText="Status Kunjungan" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </div>
        </td>
    </tr>
</table>