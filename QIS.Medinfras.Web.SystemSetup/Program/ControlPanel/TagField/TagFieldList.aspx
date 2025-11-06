<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master" AutoEventWireup="true" 
CodeBehind="TagFieldList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.TagFieldList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnTagFieldSave" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Save")%></div></li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnTagFieldSave.ClientID %>').click(function () {
                if (cboBusinessObject.GetSelectedIndex() > -1 && isChangeTagField) {
                    onCustomButtonClick('save');
                    isChangeTagField = false;
                }
            });
        });

        var prevCboBusinessObjectValue = '';
        function onChangeBusinessObject() {
            var isConfirmed = true;
            if (isChangeTagField)
                isConfirmed = confirm('All Changed Data Will Be Lost. Are You Sure?');
            if (isConfirmed) {
                $('#<%=hdnBusinessObject.ClientID %>').val(cboBusinessObject.GetValue());
                cbpView.PerformCallback();
                isChangeTagField = false;
            }
            else
                cboBusinessObject.SetValue(prevCboBusinessObjectValue);
            prevCboBusinessObjectValue = cboBusinessObject.GetValue()
        }

        var isChangeTagField = false;
        function onCbpViewEndCallback() {
            $('#containerImgLoadingView').hide();
            isChangeTagField = false;
            $('.txtValue').change(function () {
                isChangeTagField = true;
            });
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnBusinessObject" runat="server" value="" />
    <div style="position: relative;">
        <table width="50%">
            <colgroup>
                <col width="30%" />
            </colgroup>
            <tr>
                <td class="tdLabel"><label><%=GetLabel("Business Object")%></label></td>
                <td>
                    <dx:ASPxComboBox ID="cboBusinessObject" runat="server" Width="150px" ClientInstanceName="cboBusinessObject">
                        <ClientSideEvents SelectedIndexChanged="onChangeBusinessObject" />
                    </dx:ASPxComboBox>
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:480px;font-size:1.2em">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdNormal" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="Index" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                
                                <asp:BoundField DataField="TagFieldID" HeaderText="Tag Field Name" HeaderStyle-Width="200px" />
                                <asp:TemplateField>
                                    <HeaderTemplate><%=GetLabel("Value") %></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValue" CssClass="txtValue" runat="server" Width="100%" Text='<%#:Eval("Value") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </div>
</asp:Content>