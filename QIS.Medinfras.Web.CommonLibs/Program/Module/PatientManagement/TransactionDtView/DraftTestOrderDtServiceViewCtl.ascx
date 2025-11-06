<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DraftTestOrderDtServiceViewCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.DraftTestOrderDtServiceViewCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
     
<script type="text/javascript" id="dxss_drafttestorderdtviewctl">
    $(function () {
        $val = $('#<%=hdnHideCheckbox.ClientID %>').val();
        if ($val == "1") {
            $('.chkSelectAll input').prop('style', 'display:none');
            $('.chkIsSelectedTestOrder input').each(function () {
                $(this).prop('style', 'display:none');
            });
        }
    });

</script>
<input type="hidden" value="0" id="hdnHideCheckbox" runat="server" /> 
<asp:ListView ID="lvwTestOrder" runat="server">
    <EmptyDataTemplate>
        <table id="tblView" runat="server" class="grdNormal" cellspacing="0" rules="all">
            <tr>
                <th style="width: 40px" align="center">
                    <div style="padding: 3px">
                        <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                    </div>
                </th>
                <th>
                    <div style="text-align: left; padding-left: 3px">
                        <%=GetLabel("Deskripsi")%>
                    </div>
                </th>
                <th style="width: 150px">
                    <div style="text-align: left; padding-left: 3px">
                        <%=GetLabel("Diagnosa")%>
                    </div>
                </th>
                <th style="width: 80px">
                    <div style="text-align: center; padding-right: 3px">
                        <%=GetLabel("CITO")%>
                    </div>
                </th>
                <th style="width: 100px">
                    <div style="text-align: center; padding-right: 3px">
                        <%=GetLabel("Remarks")%>
                    </div>
                </th>
                <th>
                    <div style="width: 50px; text-align: center; padding-left: 3px">
                        <%=GetLabel("Petugas")%>
                    </div>
                </th>
            </tr>
            <tr class="trEmpty">
                <td colspan="6">
                    <%=GetLabel("No Data To Display") %>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
            <tr>
                <th style="width: 40px" align="center">
                    <div style="padding: 3px">
                        <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                    </div>
                </th>
                <th>
                    <div style="width: 300px; text-align: left; padding-left: 3px">
                        <%=GetLabel("Deskripsi")%>
                    </div>
                </th>
                <th style="width: 300px">
                    <div style="text-align: left; padding-left: 3px">
                        <%=GetLabel("Diagnosa")%>
                    </div>
                </th>
                <th style="width: 80px">
                    <div style="text-align: center; padding-right: 3px">
                        <%=GetLabel("CITO")%>
                    </div>
                </th>
                <th style="width: 100px">
                    <div style="width: 300px; text-align: center; padding-right: 3px">
                        <%=GetLabel("Remarks")%>
                    </div>
                </th>
                <th>
                    <div style="width: 50px; text-align: center; padding-left: 3px">
                        <%=GetLabel("Petugas")%>
                    </div>
                </th>
            </tr>
            <tr runat="server" id="itemPlaceholder">
            </tr>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td align="center">
                <div style="padding: 3px">
                    <asp:CheckBox ID="chkIsSelectedTestOrder" CssClass="chkIsSelectedTestOrder" runat="server" />
                    <input type="hidden" class="hdnKeyField" value="<%#: Eval("ID")%>" />
                </div>
            </td>
            <td>
                <div style="padding: 3px">
                    <div>
                        <%#: Eval("ItemName1")%></div>
                    <div>
                        <%#: Eval("ParamedicName")%></div>
                    <div class="divTransactionNo">
                        <%#: Eval("ItemCode") %></div>

                </div>
            </td>
            <td>
                <div style="padding: 3px;">
                    <div>
                        <%#: Eval("DiagnoseName")%></div>
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: center;">
                    <div>
                        <%#: Eval("IsCito")%></div>
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: left;">
                    <div>
                        <%#: Eval("Remarks")%></div>
                </div>
            </td>
            <td>
                <div style="padding: 3px; text-align: left;">
                    <div>
                        <%#: Eval("CreatedByFullName")%></div>
                    <div>
                        <%#: Eval("cfCreatedDateInString")%></div>
                </div>
            </td>
        </tr>
    </ItemTemplate>
</asp:ListView>
