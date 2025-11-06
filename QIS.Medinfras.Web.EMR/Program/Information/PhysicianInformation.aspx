<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="PhysicianInformation.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PhysicianInformation" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle()) %></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
       
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" value="" id="hdnParamedicID" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){showLoadingPanel()}" EndCallback="function(s,e){onCbpViewEndCallback(s)}" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="ParamedicCode" HeaderText="Kode Dokter" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Nama Dokter" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="cfHomeAddress" HeaderText="Alamat" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="50px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Informasi Kontak") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="text-align: left; font-size: 0.95em; font-style: italic">
                                                        <%=GetLabel("No. Telp : ")%> <%#:Eval("MobileNo1") %> &nbsp; <%#:Eval("MobileNo2") %>
                                                    <div>
                                                    <div style="text-align: left; font-size: 0.95em; font-style: italic">
                                                        <%=GetLabel("Alamat Email : ")%> <%#:Eval("EmailAddress1") %> &nbsp; <%#:Eval("EmailAddress2") %>
                                                    <div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="VATRegistrationNo" HeaderText="No. Registrasi Pajak" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Informasi Bank") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="text-align: left; font-size: 0.95em; font-style: italic">
                                                        <%=GetLabel("Nama Bank : ")%>
                                                        <%#:Eval("BankName") %>&nbsp;&nbsp;
                                                        <%=GetLabel("Cabang Bank : ")%>
                                                        <%#:Eval("BankBranch") %></div>
                                                    <div style="text-align: left; font-size: 0.95em; font-style: italic">
                                                        <%=GetLabel("No. Rekening Bank : ")%>
                                                        <%#:Eval("BankAccountNo") %></div>
                                                    <div style="text-align: left; font-size: 0.95em; font-style: italic">
                                                        <%=GetLabel("No. Akun Virtual Bank : ")%>
                                                        <%#:Eval("VirtualAccountNo") %></div>
                                                    <div style="text-align: left; font-size: 0.95em; font-style: italic">
                                                        <%=GetLabel("Nama Rekening Bank : ")%>
                                                        <%#:Eval("BankAccountName") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
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
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
