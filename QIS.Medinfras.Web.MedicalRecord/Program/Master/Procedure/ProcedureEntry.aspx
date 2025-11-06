<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="ProcedureEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.ProcedureEntry" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

            $('#lblEKlaimINAProcedure.lblLink').click(function () {
                openSearchDialog('eklaimproceduresINA', '', function (value) {
                    
                    ontxtEKlaimINAProcedureCodeChanged(value);
                });
            });
            $('#<%=txtEKlaimINAProcedureCode.ClientID %>').change(function () {
                ontxtEKlaimINAProcedureCodeChanged($(this).val());
            });

            function ontxtEKlaimINAProcedureCodeChanged(value) {
                var filterExpression = "BPJSCode = '" + value + "'";
                Methods.getObject('GetvBPJSReferenceProcedureINAGrouperEKlaimList', filterExpression, function (result) {
                    if (result != null) {
                       
                        $('#<%=txtEKlaimINAProcedureName.ClientID %>').val(result.BPJSName);
                        $('#<%=txtEKlaimINAProcedureCode.ClientID %>').val(result.BPJSCode);
                    } else {
                        $('#<%=txtEKlaimINAProcedureCode.ClientID %>').val('');
                        $('#<%=txtEKlaimINAProcedureName.ClientID %>').val('');
                    }
                });
            }


            //#region E-Klaim Procedure
            $('#lblEKlaimProcedure.lblLink').click(function () {
                openSearchDialog('eklaimprocedure', '', function (value) {
                    $('#<%=txtEKlaimProcedureCode.ClientID %>').val(value);
                    ontxtEKlaimProcedureCodeChanged(value);
                });
            });

            $('#<%=txtEKlaimProcedureCode.ClientID %>').change(function () {
                ontxtEKlaimProcedureCodeChanged($(this).val());
            });

            function ontxtEKlaimProcedureCodeChanged(value) {       
                var filterExpression = "BPJSCode = '" + value + "'";
                Methods.getObject('GetvBPJSReferenceProcedureEKlaimList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtEKlaimProcedureName.ClientID %>').val(result.BPJSName);
                    else {
                        $('#<%=txtEKlaimProcedureCode.ClientID %>').val('');
                        $('#<%=txtEKlaimProcedureName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Referensi VClaim
            $('#lblVClaim.lblLink').click(function () {
                openSearchDialog('vclaimProcedure', '', function (value) {
                    $('#<%=txtBPJSReferenceInfo1Code.ClientID %>').val(value);
                    ontxtBPJSReferenceInfo1CodeChanged(value);
                });
            });

            $('#<%=txtBPJSReferenceInfo1Code.ClientID %>').change(function () {
                ontxtBPJSReferenceInfo1CodeChanged($(this).val());
            });

            function ontxtBPJSReferenceInfo1CodeChanged(value) {
                var filterExpression = "BPJSCode = '" + value + "'";
                Methods.getObject('GetvBPJSReferenceProcedureList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtBPJSReferenceInfo1Name.ClientID %>').val(result.BPJSName);
                    else {
                        $('#<%=txtBPJSReferenceInfo1Code.ClientID %>').val('');
                        $('#<%=txtBPJSReferenceInfo1Name.ClientID %>').val('');
                    }
                });
            }

            //#endregion
        }
    </script>

    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:20%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Tindakan")%></label></td>
                        <td><asp:TextBox ID="txtProcedureID" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Tindakan")%></label></td>
                        <td><asp:TextBox ID="txtProcedureName" Width="100%" runat="server" /></td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("INACBG Label")%></label></td>
                        <td><asp:TextBox ID="txtINACBGLabel" Width="100px" runat="server" /></td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("BPJS Reference Info")%></label></td>
                        <td><asp:TextBox ID="txtBPJSReferenceInfo" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblEKlaimProcedure">
                                <%=GetLabel("Prosedur (E-Klaim)")%></label>
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
                                        <asp:TextBox ID="txtEKlaimProcedureCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEKlaimProcedureName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                     <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblEKlaimINAProcedure">
                                <%=GetLabel("Prosedur INA (E-Klaim)")%></label>
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
                                        <asp:TextBox ID="txtEKlaimINAProcedureCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEKlaimINAProcedureName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblVClaim">
                                <%=GetLabel("Referensi VClaim")%></label>
                        </td>
                        <td>
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:100px"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtBPJSReferenceInfo1Code" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtBPJSReferenceInfo1Name" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Keyword")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKeyword" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
