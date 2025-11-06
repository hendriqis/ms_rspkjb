<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientEducationCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientEducationCtl" %>

<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', '0');

    $(function () {
        $(".rblEducationTypeStatus input").change(function () {
            $txt = $(this).closest('tr').parent().closest('tr').find('.txtFreeText');
            if ($(this).val() == '1')
                $txt.show();
            else
                $txt.hide();
        });
        $(".rblEducationTypeStatus").each(function () {
            $(this).find('input[checked=checked]').change();
        });
    });

    function onAfterSaveRecordPatientPageEntry(result) {
        if (typeof onRefreshEducationGrid == 'function')
            onRefreshEducationGrid();
        if (result != "") {
            var param = result.split("|");
            if (typeof onAfterAddEducation == 'function')
                onAfterAddEducation(param[1]);
        }
    }
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
                        <col style="width:150px"/>
                        <col style="width:150px"/>
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam Edukasi")%></label></td>
                        <td><asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                        <td><asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="width: 100%; height: 300px; border:0px solid #AAA; overflow-y: auto; overflow-x:hidden; padding-left: 10px;">
                    <asp:Repeater ID="rptEducationType" runat="server">
                        <HeaderTemplate>
                            <table cellpadding="0" cellspacing="0" style="width:100%">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="labelColumn" style="width:250px;vertical-align:top;padding-top:5px">
                                    <input type="hidden" id="hdnGCEducationType" runat="server" value='<%#:Eval("StandardCodeID") %>' />
                                    <%#: Eval("StandardCodeName") %>
                                </td>
                                <td>
                                    <div style="padding-left:1px">
                                        <asp:RadioButtonList ID="rblEducationTypeStatus" CssClass="rblEducationTypeStatus" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text=" Ya" Value="1"/>
                                            <asp:ListItem Text=" Tidak" Value="0" />
                                        </asp:RadioButtonList>
                                    </div>
                                    <div style="padding-left:5px">
                                        <asp:TextBox ID="txtFreeText" Style="display:none" CssClass="txtFreeText" runat="server" Width="370px" TextMode="Multiline" Rows="2" />
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
