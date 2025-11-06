<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="DiagnoseEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.DiagnoseEntry" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region DTD
            $('#lblDTDNo.lblLink').click(function () {
                openSearchDialog('dtd', '', function (value) {
                    $('#<%=txtDTDNo.ClientID %>').val(value);
                    onTxtDTDNoChanged(value);
                });
            });

            $('#<%=txtDTDNo.ClientID %>').change(function () {
                onTxtDTDNoChanged($(this).val());
            });

            function onTxtDTDNoChanged(value) {
                var filterExpression = "DTDNo = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetDTDList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtDTDName.ClientID %>').val(result.DTDName);
                    else {
                        $('#<%=txtDTDNo.ClientID %>').val('');
                        $('#<%=txtDTDName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region E-Klaim Diagnose

            $('#lblEKlaimDiagnoseINA.lblLink').click(function () {
                openSearchDialog('eklaimdiagnoseINA', '', function (value) {
                    ontxtEKlaimDiagnoseINACodeChanged(value);
                });
            });
            function ontxtEKlaimDiagnoseINACodeChanged(value) {

                var filterExpression = "BPJSCode = '" + value + "'";
                Methods.getObject('GetvBPJSReferenceDiagnoseINAGrouperEKlaimList', filterExpression, function (result) {
                    if (result != null) {

                        $('#<%=txtEKlaimDiagnoseINACode.ClientID %>').val(result.BPJSCode);
                        $('#<%=txtEKlaimDiagnoseINAName.ClientID %>').val(result.BPJSName);
                    }
                    else {
                        $('#<%=txtEKlaimDiagnoseINACode.ClientID %>').val('');
                        $('#<%=txtEKlaimDiagnoseINAName.ClientID %>').val('');
                    }
                });
             }
            

            $('#lblEKlaimDiagnose.lblLink').click(function () {
                openSearchDialog('eklaimdiagnose', '', function (value) {
                  
                    ontxtEKlaimDiagnoseCodeChanged(value);
                });
            });

            $('#<%=txtEKlaimDiagnoseCode.ClientID %>').change(function () {
                ontxtEKlaimDiagnoseCodeChanged($(this).val());
            });

            function ontxtEKlaimDiagnoseCodeChanged(value) {
                var filterExpression = "BPJSCode = '" + value + "'";
                Methods.getObject('GetvBPJSReferenceDiagnoseEKlaimList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtEKlaimDiagnoseCode.ClientID %>').val(result.BPJSCode);
                        $('#<%=txtEKlaimDiagnoseName.ClientID %>').val(result.BPJSName);
                    }
                    else {
                        $('#<%=txtEKlaimDiagnoseCode.ClientID %>').val('');
                        $('#<%=txtEKlaimDiagnoseName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region V-Klaim Diagnose
            $('#lblVKlaimDiagnose.lblLink').click(function () {
                openSearchDialog('vklaimdiagnose', '', function (value) {
                    $('#<%=txtVKlaimDiagnoseCode.ClientID %>').val(value);
                    ontxtVKlaimDiagnoseCodeChanged(value);
                });
            });

            $('#<%=txtVKlaimDiagnoseCode.ClientID %>').change(function () {
                ontxtVKlaimDiagnoseCodeChanged($(this).val());
            });

            function ontxtVKlaimDiagnoseCodeChanged(value) {
                var isBridgingBPJS = $('#<%=hdnIsBridgingBPJS.ClientID %>').val()
                var filterExpression = "BPJSCode = '" + value + "'";
                Methods.getObject('GetvBPJSReferenceDiagnosaList', filterExpression, function (result) {
                    if (isBridgingBPJS == "1") {
                        if (result != null)
                            $('#<%=txtVKlaimDiagnoseName.ClientID %>').val(result.BPJSName);
                        else {
                            $('#<%=txtVKlaimDiagnoseCode.ClientID %>').val('');
                            $('#<%=txtVKlaimDiagnoseName.ClientID %>').val('');
                        }
                    }
                    else {
                        $('#<%=txtVKlaimDiagnoseName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

 
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingBPJS" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 20%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("ID Diagnosa")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDiagnoseID" Width="25%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Diagnosa")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDiagnoseName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblDTDNo">
                                <%=GetLabel("No DTD")%></label>
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
                                        <asp:TextBox ID="txtDTDNo" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDTDName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblEKlaimDiagnose">
                                <%=GetLabel("Diagnosa (E-Klaim)")%></label>
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
                                        <asp:TextBox ID="txtEKlaimDiagnoseCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEKlaimDiagnoseName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                       <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblEKlaimDiagnoseINA">
                                <%=GetLabel("Diagnosa INA (E-Klaim)")%></label>
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
                                        <asp:TextBox ID="txtEKlaimDiagnoseINACode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEKlaimDiagnoseINAName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblVKlaimDiagnose">
                                <%=GetLabel("Diagnosa (V-Klaim)")%></label>
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
                                        <asp:TextBox ID="txtVKlaimDiagnoseCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVKlaimDiagnoseName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel">
                            <label class="lblNormal lblLink">
                                <%=GetLabel("Referensi VClaim")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtINACBGLabel" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Keyword")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKeyword" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsDisease" runat="server" /><%=GetLabel("Penyakit")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsChronicDisease" runat="server" /><%=GetLabel("Penyakit Kronis")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsInfectious" runat="server" /><%=GetLabel("Menular")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsExternalDiagnosis" runat="server" /><%=GetLabel("Sebab Luar")%>
                        </td>
                    </tr>

                    <tr>
                        <td class="tdLabel">
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsPotentialPRB" runat="server" /><%=GetLabel("Potential PRB")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsNutritionDiagnosis" runat="server" /><%=GetLabel("Diagnosa Gizi")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsSpecialCMG" runat="server" /><%=GetLabel("Spesial CMG")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
