<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MCUResultFormViewDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.MCU.Program.MCUResultFormViewDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_MCUResultFormViewDetailCtl">
    $(function () {
        if ($('#<%=hdnFormValueCtl.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnFormValueCtl.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0]) {
                        if (temp.length > 2) {
                            var strValue = "";
                            var finalValue = ""; 
                            for (var i = 0; i < temp.length; i++) {
                                if (i >= 1) {
                                    strValue += temp[i].concat("="); //jika isi value model seperti ini,  Hipertensi Derajat II (>= 160/100 mmHg)
                                }
                            }
                            if (strValue != "") {
                                finalValue = strValue.substr(0, strValue.length - 1);
                            }

                            $(this).val(finalValue);
                        } else {
                            $(this).val(temp[1].toString());
                        }

                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && (temp[1] == "1" || temp[1] == "Ya"))
                        $(this).prop('checked', true);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && (temp[1] == "1" || temp[1] == "Ya"))
                        $(this).prop('checked', true);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }
    });
</script>
<div style="height: auto;">
    <input type="hidden" runat="server" id="hdnIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnGCResultTypeCtl" value="" />
    <input type="hidden" runat="server" id="hdnDivHTMLCtl" value="" />
    <input type="hidden" runat="server" id="hdnFormLayoutCtl" value="" />
    <input type="hidden" runat="server" id="hdnFormValueCtl" value="" />
    <table class="tblContentArea" border="0">
        <tr>
            <td colspan="2">
                <hr style="margin: 0 0 0 0;" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <label class="lblNormal" id="lblTitle" runat="server" style="font-size: large; font-style: oblique;
                    text-decoration: blink">
                </label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr style="margin: 0 0 0 0;" />
            </td>
        </tr>
        <tr style="display:none">
            <td>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divFormContent" runat="server" style="height: 650px; overflow-y: auto;">
                </div>
            </td>
        </tr>
    </table>
</div>
