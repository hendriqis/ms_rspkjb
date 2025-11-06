<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" CodeBehind="CustomerDiagnoseEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.CustomerDiagnoseEntry" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
<script type="text/javascript">

    function onLoad() {
  
        //#Region BusinessPartner
        function getCustomerFilterExpression() {
            var filterExpression = "<%:OnGetCustomerFilterExpression() %>";
            return filterExpression;
        }
        $('#lblBusinessPartner.lblLink').click(function () {
            openSearchDialog('businesspartners', getCustomerFilterExpression(), function (value) {
                $('#<%=txtBusinessPartnerCode.ClientID %>').val(value);
                onTxtBusinessPartnerCodeChanged(value);
            });
        });
        $('#<%=txtBusinessPartnerCode.ClientID %>').change(function () {
            onTxtBusinessPartnerCodeChanged($(this).val());
        });

        function onTxtBusinessPartnerCodeChanged(value) {
            var filterExpression = getCustomerFilterExpression() + "AND BusinessPartnerCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnBusinessPartnerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val(result.BusinessPartnerCode);
                    $('#<%=txtBusinesPartnerName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                    $('#<%=txtBusinesPartnerName.ClientID %>').val('');
                }
            });
        }
        //#endregion
    }
</script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />    
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:25%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Customer Diagnose No")%></label></td>
                        <td><asp:TextBox ID="txtCustomerDiagnoseNo" Width="150px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Customer Diagnose Name")%></label></td>
                        <td><asp:TextBox ID="txtCustomerDiagnoseName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink lblMandatory" id="lblBusinessPartner"><%=GetLabel("Instansi")%></label></td>
                            <td>
                                <input type="hidden" id="hdnBusinessPartnerID" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox runat="server" ID="txtBusinessPartnerCode" Width="100%" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox runat="server" ID="txtBusinesPartnerName" Width="100%" ReadOnly="true" /></td>
                                    </tr>
                                </table>
                            </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>