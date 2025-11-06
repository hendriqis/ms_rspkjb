<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhysicalExamEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPage.PhysicalExamEntryCtl" %>

<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', '0');

    $(function () {
        $(".rblReviewOfSystem input").change(function () {
            $txt = $(this).closest('tr').parent().closest('tr').find('.txtFreeText');
            if ($(this).val() == '3')
                $txt.show();
            else
                $txt.hide();
        });
        $(".rblReviewOfSystem").each(function () {
            $(this).find('input[checked=checked]').change();
        });

        $("#<%=rblCheckAll.ClientID %> input").change(function () {
            var value = $(this).val();
            $(".rblReviewOfSystem").each(function () {
                $(this).find('input[value=' + value + ']').prop('checked', true).change();
            });
        });

    });
</script>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top;">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:100px"/>
                        <col style="width:150px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Date")%> - <%=GetLabel("Time")%></label></td>
                        <td><asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                        <td><asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:RadioButtonList ID="rblCheckAll" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Semua tidak diperiksa" Value="1" />
                                <asp:ListItem Text="Semua Normal" Value="2" />
                                <asp:ListItem Text="Catatan Pemeriksaan" Value="3" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="width: 100%; height: 300px; border:1px solid #AAA; overflow-y: scroll; overflow-x:hidden; padding-left: 10px;">
                    <asp:Repeater ID="rptReviewOfSystem" runat="server">
                        <HeaderTemplate>
                            <table cellpadding="0" cellspacing="0" style="width:100%">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="labelColumn" style="width:250px;vertical-align:top;padding-top:5px">
                                    <input type="hidden" id="hdnGCROSystem" runat="server" value='<%#:Eval("StandardCodeID") %>' />
                                    <%#: Eval("StandardCodeName") %>
                                </td>
                                <td>
                                    <div style="padding-left:1px">
                                        <asp:RadioButtonList ID="rblReviewOfSystem" CssClass="rblReviewOfSystem" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Tidak Diperiksa" Value="1"/>
                                            <asp:ListItem Text="Normal" Value="2" />
                                            <asp:ListItem Text="Hasil Pemeriksaan" Value="3" />
                                        </asp:RadioButtonList>
                                    </div>
                                    <div style="padding-left:5px">
                                        <asp:TextBox ID="txtFreeText" Style="display:none" CssClass="txtFreeText" runat="server" Width="300px" />
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
