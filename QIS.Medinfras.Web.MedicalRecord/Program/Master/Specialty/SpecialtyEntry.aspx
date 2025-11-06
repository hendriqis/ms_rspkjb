<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="SpecialtyEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.SpecialtyEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#lblVClaim.lblLink').click(function () {
                openSearchDialog('vclaimSpecialty', '', function (value) {
                    $('#<%=txtBPJSReferenceInfo1Code.ClientID %>').val(value);
                    ontxtBPJSReferenceInfo1CodeChanged(value);
                });
            });

            $('#<%=txtBPJSReferenceInfo1Code.ClientID %>').change(function () {
                ontxtBPJSReferenceInfo1CodeChanged($(this).val());
            });

            function ontxtBPJSReferenceInfo1CodeChanged(value) {
                var filterExpression = "BPJSCode = '" + value + "'";
                Methods.getObject('GetvBPJSReferenceSpecialityList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtBPJSReferenceInfo1Name.ClientID %>').val(result.BPJSName);
                    else {
                        $('#<%=txtBPJSReferenceInfo1Code.ClientID %>').val('');
                        $('#<%=txtBPJSReferenceInfo1Name.ClientID %>').val('');
                    }
                });
            }
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr style="display: none">
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Reference VClaim")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtBPJSReferenceInfo" Width="100px" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 25%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Spesialisasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSpecialtyID" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Spesialisasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSpecialtyName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Spesialisasi 2")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSpecialtyName2" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Grup Spesialisasi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboGCSpecialtyGroup" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Jenis Kasus")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboVisitCaseType" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblVClaim">
                                <%=GetLabel("Referensi VClaim")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtBPJSReferenceInfo1Code" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBPJSReferenceInfo1Name" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pengelompokan RL")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboRLReportGroup" Width="300px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
