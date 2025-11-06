<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="INACBGMasterEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.INACBGMasterEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        //#region Healthcare
        $('#lblHealthcare.lblLink').die('click');
        $('#lblHealthcare.lblLink').live('click', function () {
            var filterExpression = "1=1";
            openSearchDialog('healthcare', filterExpression, function (value) {
                $('#<%=txtHealthcareCode.ClientID %>').val(value);
                ontxtHealthcareCodeChanged(value);
            });
        });

        $('#<%=txtHealthcareCode.ClientID %>').die('change');
        $('#<%=txtHealthcareCode.ClientID %>').live('change', function () {
            ontxtHealthcareCodeChanged($(this).val());
        });

        function ontxtHealthcareCodeChanged(value) {
            var filterExpression = "HealthcareID = '" + value + "'";
            Methods.getObject('GetvHealthcareList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnHealthcareID.ClientID %>').val(result.HealthcareID);
                    $('#<%=txtHealthcareName.ClientID %>').val(result.HealthcareName);
                    $('#<%=txtEKlaimTariffCategory1.ClientID %>').val(result.EKlaimTariffCategory1);
                    $('#<%=txtEKlaimTariffCategory2.ClientID %>').val(result.EKlaimTariffCategory2);
                }
                else {
                    $('#<%=hdnHealthcareID.ClientID %>').val('');
                    $('#<%=txtHealthcareCode.ClientID %>').val('');
                    $('#<%=txtHealthcareName.ClientID %>').val('');
                    $('#<%=txtEKlaimTariffCategory1.ClientID %>').val('');
                    $('#<%=txtEKlaimTariffCategory2.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region ClassCare
        $('#lblClassCare.lblLink').die('click');
        $('#lblClassCare.lblLink').live('click', function () {
            var filterExpression = "IsDeleted = 0 AND IsUsedInChargeClass = 1 AND GCINACBGClass IS NOT NULL";
            openSearchDialog('classcare', filterExpression, function (value) {
                $('#<%=txtClassCode.ClientID %>').val(value);
                ontxtClassCodeChanged(value);
            });
        });

        $('#<%=txtClassCode.ClientID %>').die('change');
        $('#<%=txtClassCode.ClientID %>').live('change', function () {
            ontxtClassCodeChanged($(this).val());
        });

        function ontxtClassCodeChanged(value) {
            var filterExpression = "IsDeleted = 0 AND IsUsedInChargeClass = 1 AND GCINACBGClass IS NOT NULL AND ClassCode = '" + value + "'";
            Methods.getObject('GetvClassCareList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnClassID.ClientID %>').val(result.ClassID);
                    $('#<%=txtClassName.ClientID %>').val(result.ClassName);
                    $('#<%=txtINACBGClass.ClientID %>').val(result.INACBGClass); 
                }
                else {
                    $('#<%=hdnClassID.ClientID %>').val('');
                    $('#<%=txtClassCode.ClientID %>').val('');
                    $('#<%=txtClassName.ClientID %>').val('');
                    $('#<%=txtINACBGClass.ClientID %>').val('');
                }
            });
        }
        //#endregion

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 100px" />
            <col style="width: 300px" />
            <col />
            <col style="width: 40%" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Kode Grouper")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtGrouperCode" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Deskripsi Grouper")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtGrouperDescription" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <input type="hidden" id="hdnHealthcareID" runat="server" value="" />
                <label class="lblLink lblMandatory" id="lblHealthcare">
                    <%=GetLabel("Rumah Sakit")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtHealthcareCode" Width="100%" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtHealthcareName" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Kode Tarif INACBG 1")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtEKlaimTariffCategory1" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Kode Tarif INACBG 2")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtEKlaimTariffCategory2" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <input type="hidden" id="hdnClassID" runat="server" value="" />
                <label class="lblLink lblMandatory" id="lblClassCare">
                    <%=GetLabel("Kelas")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtClassCode" Width="100%" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtClassName" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Kelas INACBG")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtINACBGClass" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tariff INACBG")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtINACBGTariff" CssClass="txtCurrency" Width="100%" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
