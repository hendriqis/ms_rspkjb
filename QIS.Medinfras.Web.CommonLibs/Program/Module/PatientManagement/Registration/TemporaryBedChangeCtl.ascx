<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemporaryBedChangeCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TemporaryBedChangeCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_bedchangeentryctl">

    //#region temporary location
    $('#<%:chkIsTemporaryLocation.ClientID %>').live('change', function () {
        $chkIsTemporaryLocation = $('#<%:chkIsTemporaryLocation.ClientID %>');
        if ($(this).is(':checked')) {
            $('#<%:trClassRequest.ClientID %>').removeAttr('style');
        }
        else {
            $('#<%:trClassRequest.ClientID %>').attr('style', 'display:none');
        }

        $('#<%:hdnClassRequestID.ClientID %>').val('');
        $('#<%:txtClassRequestCode.ClientID %>').val('');
        $('#<%:txtClassRequestName.ClientID %>').val('');
    });
    //#endregion

    //#region Request Class Care
    $('#<%:lblClassRequest.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('classcare', "IsDeleted = 0 AND IsUsedInChargeClass = 1", function (value) {
            $('#<%:txtClassRequestCode.ClientID %>').val(value);
            onTxtClassRequestCodeChanged(value);
        });
    });

    $('#<%:txtClassRequestCode.ClientID %>').live('change', function () {
        onTxtClassRequestCodeChanged($(this).val());
    });

    function onTxtClassRequestCodeChanged(value) {
        var filterExpression = "IsDeleted = 0 AND ClassCode = '" + value + "'";
        Methods.getObject('GetClassCareList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnClassRequestID.ClientID %>').val(result.ClassID);
                $('#<%:txtClassRequestCode.ClientID %>').val(result.ClassCode);
                $('#<%:txtClassRequestName.ClientID %>').val(result.ClassName);
            }
            else {
                $('#<%:hdnClassRequestID.ClientID %>').val('');
                $('#<%:txtClassRequestCode.ClientID %>').val('');
                $('#<%:txtClassRequestName.ClientID %>').val('');
            }
        });
    }
    //#endregion

</script>
<input type="hidden" value="" id="hdnRegistrationID" runat="server" />
<input type="hidden" value="" id="hdnRegistrationNo" runat="server" />
<input type="hidden" value="" id="hdnDepartmentID" runat="server" />
<div>
    <table class="tblContentArea">
        <tr>
            <td>
                <div>
                    <table class="tblEntryDetail" style="width: 100%">
                        <colgroup>
                            <col style="width: 50%" />
                            <col style="width: 50%" />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <table class="tblEntryContent" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("No. Registrasi")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRegistrationNo" Width="150px" ReadOnly="true" runat="server"
                                                Style="text-align: left" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("No. Rekam Medis")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMedicalNo" Width="100px" ReadOnly="true" runat="server" Style="text-align: left" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Nama Pasien")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPatientName" Width="300px" ReadOnly="true" runat="server" Style="text-align: left" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <div class="lblComponent">
                                    <%=GetLabel("Tempat Tidur")%></div>
                                <table>
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col style="width: 400px" />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Ruang Perawatan")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceUnitCode" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Kamar")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnRoomID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtRoomCode" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRoomName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Tempat Tidur")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnBedID" value="" runat="server" />
                                            <asp:TextBox ID="txtBedCode" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Kelas")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnClassID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtClassCode" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtClassName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Kelas Tagihan")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnChargeClassID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtChargeClassCode" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtChargeClassName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <div class="lblComponent">
                                    <%=GetLabel("Pasien Titipan")%></div>
                                <table>
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col style="width: 400px" />
                                    </colgroup>
                                    <tr>
                                        <td id="tdTemporaryLocation" runat="server">
                                            <asp:CheckBox ID="chkIsTemporaryLocation" runat="server" /><%:GetLabel("Pasien Titipan")%>
                                        </td>
                                    </tr>
                                    <tr id="trClassRequest" runat="server" style="display: none">
                                        <td class="tdLabel">
                                            <label class="lblLink lblMandatory" runat="server" id="lblClassRequest">
                                                <%:GetLabel("Kelas Permintaan")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnClassRequestID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 80px" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtClassRequestCode" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtClassRequestName" ReadOnly="true" Width="100%" runat="server" />
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
            </td>
        </tr>
    </table>
</div>
