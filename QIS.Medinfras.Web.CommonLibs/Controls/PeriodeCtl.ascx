<%@ Control Language="C#" AutoEventWireup="true" 
CodeBehind="PeriodeCtl.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.PeriodeCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

  <table>
        <colgroup>
            <col style="width: 100px" />
        </colgroup>
        <tr id="trPeriode" runat="server">
            <td class="tdLabel">
                <label class="tdLabel">
                    <%=GetLabel("Periode")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboPeriode" Width="120px" ClientInstanceName="cboPeriode" runat="server">
                    <ClientSideEvents ValueChanged="function(s,e){ onCboPeriodeValueChanged(s); }" />
                </dxe:ASPxComboBox>
            </td>
            <td>
                <asp:TextBox ID="txtValueNum" CssClass="txtValueNum number" runat="server" Width="80px" Style="display:none" Text="1" />
            </td>
            <td class="tdCustomDate" style="display:none">
                <asp:TextBox ID="txtValueDateFrom" CssClass="txtValueDateFrom datepicker" runat="server" Width="120px" />
                -
                <asp:TextBox ID="txtValueDateTo" CssClass="txtValueDateTo datepicker" runat="server" Width="120px" />
            </td>
        </tr>
    </table>