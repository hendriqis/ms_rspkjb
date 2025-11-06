<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="InhealthTempatTidurEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.InhealthTempatTidurEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region SEARCH DIALOG : BED
            $('#lblBed.lblLink').click(function () {
                var filterExpression = "IsDeleted = 0";
                openSearchDialog('bedlist', filterExpression, function (value) {
                    $('#<%=txtBedCode.ClientID %>').val(value);
                    onTxtItemGroupCodeChanged(value);
                });
            });

            $('#<%=txtBedCode.ClientID %>').change(function () {
                onTxtItemGroupCodeChanged($(this).val());
            });

            function onTxtItemGroupCodeChanged(value) {
                var filterExpression = "BedCode = '" + value + "'";
                Methods.getObject('GetvBedList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnBedID.ClientID %>').val(result.BedID);
                        $('#<%=txtBedCode.ClientID %>').val(result.BedCode);
                        $('#<%=txtBedName.ClientID %>').val(result.BedCode);
                    }
                    else {
                        $('#<%=hdnBedID.ClientID %>').val('');
                        $('#<%=txtBedCode.ClientID %>').val('');
                        $('#<%=txtBedName.ClientID %>').val('');
                    }
                });
            }
            //#endregion        

            //#region SEARCH DIALOG : CLASS
            $('#lblClass.lblLink').click(function () {
                var filterExpression = "IsDeleted = 0";
                openSearchDialog('classcare', filterExpression, function (value) {
                    $('#<%=txtClassCode.ClientID %>').val(value);
                    onTxtClassCodeChanged(value);
                });
            });

            $('#<%=txtClassCode.ClientID %>').change(function () {
                onTxtClassCodeChanged($(this).val());
            });

            function onTxtClassCodeChanged(value) {
                var filterExpression = "ClassCode = '" + value + "'";
                Methods.getObject('GetClassCareList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnClassID.ClientID %>').val(result.ClassID);
                        $('#<%=txtClassCode.ClientID %>').val(result.ClassCode);
                        $('#<%=txtClassName.ClientID %>').val(result.ClassName);
                    }
                    else {
                        $('#<%=hdnClassID.ClientID %>').val('');
                        $('#<%=txtClassCode.ClientID %>').val('');
                        $('#<%=txtClassName.ClientID %>').val('');
                    }
                });
            }
            //#endregion        
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle">
        <%=GetLabel("Tempat Tidur")%></div>
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
                            <label class="lblLink lblMandatory" id="lblBed">
                                <%=GetLabel("Tempat Tidur")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnBedID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtBedCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBedName" Width="100%" runat="server" readonly="true"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblClass">
                                <%=GetLabel("Kelas")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnClassID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtClassCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClassName" Width="100%" runat="server" readonly="true"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Inhealth")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInhealthCode" Width="300px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
