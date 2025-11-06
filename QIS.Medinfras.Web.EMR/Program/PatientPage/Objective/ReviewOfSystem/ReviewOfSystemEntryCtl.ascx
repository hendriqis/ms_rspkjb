<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReviewOfSystemEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.ReviewOfSystemEntryCtl" %>
<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    $(function () {

        $(".rblReviewOfSystem").each(function () {
            $(this).find('input[checked=checked]').change();
        });

        $('.rblReviewOfSystem').click(function (e) {
            if (e.ctrlKey) {
                $(this).find('input').prop('checked', false).change();
            }
         });

        $("#<%=rblCheckAll.ClientID %> input").change(function () {
            var value = $(this).val();
            $(".rblReviewOfSystem").each(function () {
                if (value != 0) {
                    $(this).find('input[value=' + value + ']').prop('checked', true).change();
                }
                else {
                    $(this).find('input').prop('checked', false).change();
                }
            });
        });
    });
</script>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Date")%>
                                -
                                <%=GetLabel("Time")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("Check All")%>
                                :</label>
                        </td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblCheckAll" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Clear All" Value="0" />
                                <asp:ListItem Text="Not Examined" Value="1" />
                                <asp:ListItem Text="Normal" Value="2" />
                                <asp:ListItem Text="Abnormal" Value="3" />
                                <asp:ListItem Text="Other" Value="4" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="width: 97%; height: 400px; border: 1px solid #AAA; overflow-y: scroll;
                    overflow-x: hidden; padding-left: 10px;">
                    <asp:Repeater ID="rptReviewOfSystem" runat="server">
                        <HeaderTemplate>
                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="labelColumn" style="width: 250px; vertical-align: top; padding-top: 5px">
                                    <input type="hidden" id="hdnGCROSystem" runat="server" value='<%#:Eval("StandardCodeID") %>' />
                                    <span style="font-weight: normal">
                                        <%#: Eval("StandardCodeName") %></span>
                                </td>
                                <td>
                                    <div style="padding-left: 1px">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblReviewOfSystem" CssClass="rblReviewOfSystem" runat="server"
                                                        RepeatDirection="Horizontal" CellPadding="2">
                                                        <asp:ListItem Text="Not Examined" Value="1" />
                                                        <asp:ListItem Text="Normal" Value="2" />
                                                        <asp:ListItem Text="Abnormal" Value="3" />
                                                        <asp:ListItem Text="Other" Value="4" />
                                                    </asp:RadioButtonList>
                                                </td>
<%--                                                <td style="padding-left: 10px">
                                                    <label class="lblLink" id="lblReset">
                                                        <%=GetLabel("Reset")%></label>
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="padding-left: 5px">
                                        <asp:TextBox ID="txtFreeText" CssClass="txtFreeText" runat="server" Width="350px"
                                            TextMode="Multiline" Rows="2" />
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </td>
        </tr>
    </table>
</div>
