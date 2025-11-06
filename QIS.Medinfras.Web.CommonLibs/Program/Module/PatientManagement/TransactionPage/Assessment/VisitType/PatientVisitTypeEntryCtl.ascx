<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientVisitTypeEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientVisitTypeEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_chiefcomplaintentryctl">

    //#region Visit Type
    $('#lblVisitType.lblLink').die('click');
    $('#lblVisitType.lblLink').live('click', function () {
        openSearchDialog('visittype', 'IsDeleted = 0', function (value) {
            $('#<%=txtVisitTypeCode.ClientID %>').val(value);
            onTxtVisitTypeChanged(value);
        });
    });

    $('#<%=txtVisitTypeCode.ClientID %>').live('change', function () {
        onTxtVisitTypeChanged($(this).val());
    });

    function onTxtVisitTypeChanged(value) {
        var filterExpression = "IsDeleted = 0 AND VisitTypeCode = '" + value + "'";
        Methods.getObject('GetVisitTypeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                $('#<%=txtVisitTypeCode.ClientID %>').val(result.VisitTypeCode);
                $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
            }
            else {
                $('#<%=hdnVisitTypeID.ClientID %>').val('');
                $('#<%=txtVisitTypeCode.ClientID %>').val('');
                $('#<%=txtVisitTypeName.ClientID %>').val('');
            }
        });
    }
    //#endregion

</script>
<div style="height: 300px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td>
                <table style="width: 100%" id="tblEntryContent">
                    <colgroup>
                        <col style="width: 170px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory lblLink" id="lblVisitType">
                                <%=GetLabel("Jenis Kunjungan")%></label>
                        </td>
                        <td>
                            <input type="hidden" runat="server" id="hdnVisitTypeID" value="" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtVisitTypeCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVisitTypeName" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
