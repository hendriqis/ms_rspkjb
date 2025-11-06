<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.Master" 
    AutoEventWireup="true" CodeBehind="PrescriptionStatusEditv2.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrescriptionStatusEditv2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
 <%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>


<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li style="display:none;" id="btnProcess" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Proses")%></div></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        $(document).ready(function () {

            $("#<%=txtTransactioNo.ClientID %>").change(function () {

                $("#<%=hdnTransactionNo.ClientID %>").val(this.value);
                cbpView.PerformCallback('setStatus');
                /////cbpView.PerformCallback('refresh');
                $("#<%=txtTransactioNo.ClientID %>").val("");
            });
        });
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'changepage') {

            } else if (param[0] == 'setStatus') {

                cbpView.PerformCallback('refresh');
            }
           
        }
        
    </script> 
     <input type="hidden" value="containerProses" id="hdnContentID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" runat="server" value="" />
    <input type="hidden" id="hdnTransactionNo" value="" runat="server" />
    <div style="height:435px;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width:100%"/>
            </colgroup>
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <table style="width: 40%;">
                        <colgroup>
                            <col style="width:120px" />
                        </colgroup>
                        <tr class="tdLabel">
                            <td class="tdLabel">
                                <label class="lblNormal"><%=GetLabel("No Resep")%></label>
                            </td>
                             <td class="tdLabel">
                                <asp:TextBox runat="server" ID="txtTransactioNo" Width="100%"/>
                            </td>
                        </tr>
                       
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                   
                    <div id="containerApprove" class="containerTransDt">
                        <div id="containerEntryApprove" style="margin-top:4px;">
                            <div style="position: relative;">
                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){showLoadingPanel()}" EndCallback="function(s,e){onCbpViewEndCallback(s)}"/>
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                           <div class="containerUlTabPage">
                                                <asp:Repeater ID="rptHSU" runat="server">
                                                    <HeaderTemplate>
                                                        <ul class="ulTabPage" id="ulTabMCUTransaction">
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <li class='<%#: Eval("CssClass")%>'><%#: Eval("BarTitle")%>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;">
                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="PrescriptionOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="TransactionNo" HeaderText="Nomor Resep" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" />
                                                           <asp:TemplateField HeaderText = "Informasi Registrasi" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                                            <ItemTemplate>
                                                                <div><%#: Eval("RegistrationNo") %></div>
                                                                 <div><%#: Eval("ParamedicName") %></div>
                                                                 <div><%#: Eval("ServiceUnitName") %></div>
                                                                 <div><%#: Eval("DepartmentID") %></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText = "Informasi Pasien" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <div><%#: Eval("MedicalNo") %></div>
                                                                <div><%#: Eval("PatientName") %> (<%#: Eval("Gender") %>)</div
                                                                <div><%#: string.Format("{0:dd/MM/yyyy}", Eval("DateOfBirth")) %></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada data resep")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>    
                               
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
