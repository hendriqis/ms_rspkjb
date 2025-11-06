<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="KabupatenEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.KabupatenEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" id="dxss_KabupatenEntry">
        function onLoad() {
            //#region vKlaimKabupaten
            $('#lblVKlaimKabupaten.lblLink').click(function () {
                openSearchDialog('vklaimkabupaten', 'IsDeleted = 0', function (value) {
                    $('#<%=txtBPJSReferenceCode.ClientID %>').val(value);
                    onTxtvKlaimKabupatenChanged(value);
                });
            });

            $('#<%=txtBPJSReferenceCode.ClientID %>').change(function () {
                onTxtvKlaimKabupatenChanged($(this).val());
            });

            function onTxtvKlaimKabupatenChanged(value) {
                var filterExpression = "KabupatenID = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvBPJSKabupatenList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtBPJSReferenceCode.ClientID %>').val(result.KodeKabupaten);
                        $('#<%=txtBPJSReferenceName.ClientID %>').val(result.NamaKabupaten);
                    }
                    else {
                        $('#<%=txtBPJSReferenceCode.ClientID %>').val('');
                        $('#<%=txtBPJSReferenceName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnKodeKabupaten" runat="server" value="" />
    <input type="hidden" id="hdnKabupaten" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitICUID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitPICUID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitNICUID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 40%" />
            <col style="width: 60%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Kabupaten")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKodeKabupaten" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Kabupaten")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNama" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Provinsi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboState" Width="200px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblVKlaimKabupaten">
                                <%=GetLabel("BPJS Reference")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtBPJSReferenceCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBPJSReferenceName" Width="100%" runat="server" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div>
                    <asp:CheckBox ID="chkIsCity" runat="server" />
                    <%=GetLabel("Is City")%></div>
            </td>
        </tr>
    </table>
</asp:Content>
