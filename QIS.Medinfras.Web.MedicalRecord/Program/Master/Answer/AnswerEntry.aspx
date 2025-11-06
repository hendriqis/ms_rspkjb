<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="AnswerEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.AnswerEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
    <asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

            //#region Parent
            function getParentFilterExpression() {
                var filterExpression = "IsHasChild = 1 AND IsDeleted = 0";
                return filterExpression;
            }

            $('#lblParent.lblLink').click(function () {
                openSearchDialog('Answer', getParentFilterExpression(), function (value) {
                    $('#<%=txtParentCode.ClientID %>').val(value);
                    onTxtParentCodeChanged(value);
                });
            });

            $('#<%=txtParentCode.ClientID %>').change(function () {
                onTxtParentCodeChanged($(this).val());
            });

            function onTxtParentCodeChanged(value) {
                var filterExpression = getParentFilterExpression() + " AND AnswerCode = '" + value + "'";
                Methods.getObject('GetAnswerList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParentID.ClientID %>').val(result.AnswerID);
                        $('#<%=txtParentCode.ClientID %>').val(result.AnswerCode);
                        $('#<%=txtParentName.ClientID %>').val(result.AnswerText1);
                    }
                    else {
                        $('#<%=hdnParentID.ClientID %>').val('');
                        $('#<%=txtParentCode.ClientID %>').val('');
                        $('#<%=txtParentName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#<%=chkIsUsingPrefix.ClientID %>').change(function () {
                if ($(this).is(':checked')) {
                    $('#<%:txtPrefixText.ClientID %>').removeAttr('readonly');
                }
                else
                {
                    $('#<%:txtPrefixText.ClientID %>').attr('readonly', 'readonly');
                }
            });

            $('#<%=chkIsUsingSuffix.ClientID %>').change(function () {
                if ($(this).is(':checked')) {
                    $('#<%:txtSuffixText.ClientID %>').removeAttr('readonly');
                }
                else {
                    $('#<%:txtSuffixText.ClientID %>').attr('readonly', 'readonly');
                }
            });

            $('#<%=chkIsHasChild.ClientID %>').change(function () {
                if ($('#<%:chkIsHasChild.ClientID %>').is(':checked')) {
                    $('#trParent').attr('style', 'display:none');
                } else {
                    $('#trParent').removeAttr('style');
                }
            });
        }

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnItemGroup" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAnswerCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Teks Jawaban #1")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAnswerText1" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Teks Jawaban #2")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAnswerText2" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Teks Prefix")%></label>
                        </td>
                        <td>
                            <table style="margin-left: -3px">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkIsUsingPrefix" Checked="true" runat="server" /><%=GetLabel("IsUsingPrefix")%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrefixText" ReadOnly="true" Width="130px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Teks Suffix")%></label>
                        </td>
                        <td>
                            <table style="margin-left: -3px">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkIsUsingSuffix" Checked="true" runat="server" /><%=GetLabel("IsUsingSuffix")%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSuffixText" ReadOnly="true" Width="130px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnIsHasChild" />
                            <asp:CheckBox ID="chkIsHasChild" runat="server" /><%=GetLabel("IsHasChild")%>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 50px" />
                        <col />
                    </colgroup>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr>
                        <tr style="display: none" id="trParent">
                            <td class="tdLabel">
                                <label class="lblLink lblNormal" id="lblParent">
                                    <%=GetLabel("Parent")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" runat="server" id="hdnParentID" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtParentCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtParentName" Width="320px" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
