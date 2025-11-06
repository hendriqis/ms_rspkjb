<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="GenericNameEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.GenericNameEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region MIMS Reference
            $('#lblMIMSReferece.lblLink').click(function () {
                openSearchDialog('mimsreference', 'IsDeleted = 0', function (value) {
                    onTxtMIMSReferenceChanged(value);
                });
            });

            $('#<%=txtGUIDMIMSReferece.ClientID %>').change(function () {
                onTxtMIMSReferenceGUIDChanged($(this).val());
            });

            function onTxtMIMSReferenceChanged(value) {
                var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetMIMSReferenceList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtGUIDMIMSReferece.ClientID %>').val(result.GUID);
                    }
                    else {
                        $('#<%=txtGUIDMIMSReferece.ClientID %>').val('');
                    }
                });
            }

            function onTxtMIMSReferenceGUIDChanged(value) {
                var filterExpression = "GUID = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetMIMSReferenceList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtGUIDMIMSReferece.ClientID %>').val(result.GUID);
                    }
                    else {
                        $('#<%=txtGUIDMIMSReferece.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Kamus Farmasi dan Alkes Produk
            $('#lblKFAReferenceID.lblLink').click(function () {
                var filterExpression = "GCKFAReferenceType = 'X639^93' AND IsDeleted = 0";
                openSearchDialog('kfaReference', filterExpression, function (value) {
                    $('#<%=txtKFAReferenceCode.ClientID %>').val(value);
                    onTxtKFAReferenceCodeChanged(value);
                });
            });

            $('#<%=txtKFAReferenceCode.ClientID %>').change(function () {
                onTxtKFAReferenceCodeChanged($(this).val());
            });

            function onTxtKFAReferenceCodeChanged(value) {
                var filterExpression = "KFACode = '" + value + "' AND GCKFAReferenceType = 'X639^93'";
                Methods.getObject('GetKFAReferenceList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnKFAReferenceID.ClientID %>').val(result.ID);
                        $('#<%=txtKFAReferenceName.ClientID %>').val(result.KFAName);
                    }
                    else {
                        $('#<%=hdnKFAReferenceID.ClientID %>').val('');
                        $('#<%=txtKFAReferenceCode.ClientID %>').val('');
                        $('#<%=txtKFAReferenceName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Kamus Farmasi dan Alkes Virtual
            $('#lblKFAReferenceIDVirtual.lblLink').click(function () {
                var filterExpression = "GCKFAReferenceType = 'X639^92' AND IsDeleted = 0";
                openSearchDialog('kfaReference', filterExpression, function (value) {
                    $('#<%=txtKFAReferenceCodeVirtual.ClientID %>').val(value);
                    onTxtKFAReferenceCodeChangedVirtual(value);
                });
            });

            $('#<%=txtKFAReferenceCodeVirtual.ClientID %>').change(function () {
                onTxtKFAReferenceCodeChangedVirtual($(this).val());
            });

            function onTxtKFAReferenceCodeChangedVirtual(value) {
                var filterExpression = "KFACode = '" + value + "' AND GCKFAReferenceType = 'X639^92'";
                Methods.getObject('GetKFAReferenceList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnKFAReferenceIDVirtual.ClientID %>').val(result.ID);
                        $('#<%=txtKFAReferenceNameVirtual.ClientID %>').val(result.KFAName);
                    }
                    else {
                        $('#<%=hdnKFAReferenceIDVirtual.ClientID %>').val('');
                        $('#<%=txtKFAReferenceCodeVirtual.ClientID %>').val('');
                        $('#<%=txtKFAReferenceNameVirtual.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Aturan Pakai")%></div>--%>
    <table class="tblContentArea" style="width: 100%">
        <colgroup>
            <col style="width: 200px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Drug Generic Name")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtDrugGenericName" Width="50%" runat="server" />
            </td>
        </tr>
        <tr id="trMIMSReference" runat="server">
            <td class="tdLabel">
                <label class="lblLink" id="lblMIMSReferece">
                    <%=GetLabel("MIMS Reference")%></label>
            </td>
            <td>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txtGUIDMIMSReferece" Width="45%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblKFAReferenceID">
                    <%=GetLabel("KFA Reference Produk")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnKFAReferenceID" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 5%" />
                        <col style="width: 70%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtKFAReferenceCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtKFAReferenceName" ReadOnly="true" Width="45%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblKFAReferenceIDVirtual">
                    <%=GetLabel("KFA Reference Virtual")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnKFAReferenceIDVirtual" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 5%" />
                        <col style="width: 70%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtKFAReferenceCodeVirtual" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtKFAReferenceNameVirtual" ReadOnly="true" Width="45%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
