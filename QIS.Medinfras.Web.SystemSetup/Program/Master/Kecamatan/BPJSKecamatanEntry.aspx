<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="BPJSKecamatanEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.BPJSKecamatanEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" id="dxss_patiententryctl" >
        function onLoad() {
            //#region vKlaimKecamatan
            $('#lblVclaimKecamatan.lblLink').click(function () {
                openSearchDialog('vclaimKecamatan', 'IsDeleted = 0', function (value) {
                    $('#<%=txtBPJSReferenceCode.ClientID %>').val(value);
                    ontxtBPJSReferenceCodeChanged(value);
                });
            });

            $('#<%=txtBPJSReferenceCode.ClientID %>').change(function () {
                ontxtBPJSReferenceCodeChanged($(this).val());
            });

            function ontxtBPJSReferenceCodeChanged(value) {
                var filterExpression = "KecamatanID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvBPJSKecamatanList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtBPJSReferenceCode.ClientID %>').val(result.KodeKecamatan);
                    $('#<%=txtBPJSReferenceName.ClientID %>').val(result.NamaKecamatan);
                }
                else {
                    $('#<%=txtBPJSReferenceCode.ClientID %>').val('');
                    $('#<%=txtBPJSReferenceName.ClientID %>').val('');
                }
            });
        }
        $('#lblKabupaten.lblLink').click(function () {
            openSearchDialog('kabupaten', '', function (value) {
                $('#<%=txtKabupatenID.ClientID %>').val(value);
                ontxtKabupatenIDChanged(value);
            });
        });

        $('#<%=txtKabupatenID.ClientID %>').change(function () {
            ontxtKabupatenIDChanged($(this).val());
        });

        function ontxtKabupatenIDChanged(value) {
            var filterExpression = "KodeKabupaten = '" + value + "'";
            Methods.getObject('GetvKabupatenList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnKabupatenID.ClientID %>').val(result.KabupatenID);
                    $('#<%=txtNamaKabupaten.ClientID %>').val(result.NamaKabupaten);
                }
                else {
                    $('#<%=hdnKabupatenID.ClientID %>').val('');
                    $('#<%=txtNamaKabupaten.ClientID %>').val('');
                }
            });
        }
    }
</script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnKodeKecamatan" runat="server" value="" />
    <input type="hidden" id="hdnKecamatan" runat="server" value="" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Kode Pos")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:20%"/>
                    </colgroup>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("BPJS Reference Info")%></label></td>
                        <td><asp:TextBox ID="txtBPJSReferenceInfo" Width="100px" runat="server" /></td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("KabupatenID")%></label></td>
                        <td><asp:TextBox ID="txtKabupatenID" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Kecamatan")%></label></td>
                        <td><asp:TextBox ID="txtKodeKecamatan" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Kecamatan")%></label></td>
                        <td><asp:TextBox ID="txtNamaKecamatan" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                            <td class="tdLabel"><label class="lblLink" id="lblKabupaten"><%=GetLabel("Nama Kabupaten")%></label></td>
                            <td>
                                <input type="hidden" runat="server" id="hdnKabupatenID" value="" />
                                <asp:TextBox ID="txtNamaKabupaten" Width="100%" runat="server" />
                            </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblVclaimKecamatan">
                                <%=GetLabel("Referensi VClaim")%></label>
                        </td>
                        <td>
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:100px"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtBPJSReferenceCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtBPJSReferenceName" Width="100%" runat="server" ReadOnly="true"/></td>
                                    </tr>
                                </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
