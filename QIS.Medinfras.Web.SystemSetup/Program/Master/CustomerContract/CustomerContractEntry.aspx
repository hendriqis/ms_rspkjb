<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="CustomerContractEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.CustomerContractEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtStartDate.ClientID %>');
            setDatePicker('<%=txtEndDate.ClientID %>');
        }

        function onBeforeGoToListPage(mapForm) {
            mapForm.appendChild(createInputHiddenPost("customerID", $('#<%=hdnCustomerID.ClientID %>').val()));
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnCustomerID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Customer Contract")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Contract No")%></label></td>
                        <td><asp:TextBox ID="txtContractNo" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Start Date")%></label></td>
                        <td><asp:TextBox ID="txtStartDate" CssClass="datepicker" Width="120px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("End Date")%></label></td>
                        <td><asp:TextBox ID="txtEndDate" CssClass="datepicker" Width="120px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td><asp:CheckBox ID="chkIsControlCoverageLimit" runat="server" /> <%=GetLabel("Control Coverage Limit")%></td>
                    </tr>
                </table>
                <%=GetLabel("Contract Summary") %><br />
                <asp:TextBox TextMode="MultiLine" Width="100%" Height="300px" ID="txtContractSummary" runat="server" CssClass="htmlEditor" />
            </td>
        </tr>
    </table>
</asp:Content>
