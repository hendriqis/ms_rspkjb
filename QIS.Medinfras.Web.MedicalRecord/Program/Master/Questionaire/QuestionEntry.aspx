<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="QuestionEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.QuestionEntry" %>

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
                var filterExpression = "IsHeader = 1 AND IsDeleted = 0";
                return filterExpression;
            }

            $('#lblParent.lblLink').click(function () {
                openSearchDialog('questionText', getParentFilterExpression(), function (value) {
                    $('#<%=txtParentCode.ClientID %>').val(value);
                    onTxtParentCodeChanged(value);
                });
            });

            $('#<%=txtParentCode.ClientID %>').change(function () {
                onTxtParentCodeChanged($(this).val());
            });

            function onTxtParentCodeChanged(value) {
                var filterExpression = getParentFilterExpression() + " AND QuestionID = '" + value + "'";
                Methods.getObject('GetQuestionList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParentID.ClientID %>').val(result.QuestionID);
                        $('#<%=txtParentCode.ClientID %>').val(result.QuestionCode);
                        $('#<%=txtParentName.ClientID %>').val(result.QuestionText1);
                    }
                    else {
                        $('#<%=hdnParentID.ClientID %>').val('');
                        $('#<%=txtParentCode.ClientID %>').val('');
                        $('#<%=txtParentName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#<%=chkIsHeader.ClientID %>').change(function () {
                if ($('#<%:chkIsHeader.ClientID %>').is(':checked')) {
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
                            <asp:TextBox ID="txtQuestionCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Teks Pertanyaan #1")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtQuestionText1" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Teks Pertanyaan #2")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtQuestionText2" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kesimpulan 1")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSummarizeDisplayText1" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kesimpulan 2")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSummarizeDisplayText2" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnIsHeader" />
                            <asp:CheckBox ID="chkIsHeader" runat="server" /><%=GetLabel("IsHeader")%>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnUsingInlineAnswer" />
                            <asp:CheckBox ID="chkIsUsinginlineAnswer" runat="server" /><%=GetLabel("IsUsingInlineAnswer")%>
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
                                            <asp:TextBox ID="txtParentName" Width="219px" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
