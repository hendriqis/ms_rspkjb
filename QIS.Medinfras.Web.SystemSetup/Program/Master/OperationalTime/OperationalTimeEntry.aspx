<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="OperationalTimeEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.OperationalTimeEntry" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('.time').blur(function () {
                setOperationalTimeRequired();
            });
            $('#<%=txtDisplayColorPicker.ClientID %>').colorPicker();
            $('#<%=txtDisplayColorPicker.ClientID %>').change(function () {
                $('#<%=txtDisplayColor.ClientID %>').val($(this).val());
            });

            $('#<%=txtDisplayColor.ClientID %>').change(function () {
                $('#<%=txtDisplayColorPicker.ClientID %>').val($(this).val());
                $('#<%=txtDisplayColorPicker.ClientID %>').change();
            });

            setOperationalTimeRequired();
        }

        function setOperationalTimeRequired() {
            for (var i = 4; i > 0; --i) {
                $elm = $('.start:eq(' + i + ')');
                $elm2 = $('.end:eq(' + i + ')');

                var val = $elm.val();
                var val2 = $elm2.val();

                if (val == '' && val2 == '') {
                    $elm.removeClass('required');
                    $elm2.removeClass('required');
                    $elm.removeClass('error');
                    $elm2.removeClass('error');
                }
                else {
                    $elm.addClass('required');
                    $elm2.addClass('required');
                    if (i > 0) {
                        for (var j = i - 1; j >= 0; --j) {
                            $('.start:eq(' + j + ')').addClass('required');
                            $('.end:eq(' + j + ')').addClass('required');
                        }
                    }
                    break;
                }

                //
            }  
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:150px"/>
                        <col style="width:100px"/>
                        <col style="width:100px"/>
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode")%></label></td>
                        <td><asp:TextBox ID="txtOperationalTimeCode" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtOperationalTimeName" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Warna Tampilan")%></label></td>
                        <td colspan="3">
                            <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td><asp:TextBox ID="txtDisplayColor" CssClass="colorpicker" Width="100px" runat="server" /></td>
                                <td style="padding-left:5px;text-align:left"><asp:TextBox ID="txtDisplayColorPicker" runat="server" /></td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"></td>
                        <td align="center" ><label style="width:80px"><%=GetLabel("Mulai")%></label></td>
                        <td align="center"><label class="lblNormal"><%=GetLabel("Akhir")%></label></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Session #1")%></label></td>
                        <td><asp:TextBox ID="txtStart1" CssClass="time start" Width="80px" runat="server" MaxLength="5"/></td>
                        <td><asp:TextBox ID="txtEnd1" CssClass="time end" Width="80px" runat="server" MaxLength="5"/></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Session #2")%></label></td>
                        <td><asp:TextBox ID="txtStart2" CssClass="time start" Width="80px" runat="server" MaxLength="5"/></td>
                        <td><asp:TextBox ID="txtEnd2" CssClass="time end" Width="80px" runat="server" MaxLength="5"/></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Session #3")%></label></td>
                        <td style="width:80px"><asp:TextBox ID="txtStart3" CssClass="time start" Width="80px" runat="server" MaxLength="5"/></td>
                        <td style="width:80px"><asp:TextBox ID="txtEnd3" CssClass="time end" Width="80px" runat="server" MaxLength="5"/></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Session #4")%></label></td>
                        <td><asp:TextBox ID="txtStart4" CssClass="time start" Width="80px" runat="server" MaxLength="5"/></td>
                        <td><asp:TextBox ID="txtEnd4" CssClass="time end" Width="80px" runat="server" MaxLength="5"/></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Session #5")%></label></td>
                        <td><asp:TextBox ID="txtStart5" CssClass="time start" Width="80px" runat="server" MaxLength="5"/></td>
                        <td><asp:TextBox ID="txtEnd5" CssClass="time end" Width="80px" runat="server" MaxLength="5"/></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
