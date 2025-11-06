<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="TransactionLockEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.TransactionLockEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" id="dxss_patiententryctl">
        function onLoad() {
            setDatePicker('<%=txtLockingUntilDate.ClientID %>');
            $('#<%=txtLockingUntilDate.ClientID %>').datepicker('option', 'minDate', '0');
        }

        $('#<%=chkOpenTransaction.ClientID %>').live('change', function () {
            if ($(this).is(":checked")) {
                $('#<%=tdLockingDate.ClientID %>').attr('style', 'display:none');
            }
            else {
                $('#<%=tdLockingDate.ClientID %>').removeAttr('style');
            }
        });
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Transaction Numbering")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent">
                    <colgroup>
                        <col style="width: 10%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Transaction Code")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionCode" Width="100px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" class="lblNormal">
                            <label>
                                <%=GetLabel("Transaction Name")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionName" Width="300px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Transaction Initial")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionInitial" Width="80px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <font color="blue">
                                    <%=GetLabel("Locking Until")%></font></label>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="50%" />
                                </colgroup>
                                <tr>
                                    <td id="tdLockingDate" runat="server">
                                        <asp:TextBox ID="txtLockingUntilDate" Width="120px" runat="server" CssClass="datepicker" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkOpenTransaction" runat="server" Text="Open Locking Transaction" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
