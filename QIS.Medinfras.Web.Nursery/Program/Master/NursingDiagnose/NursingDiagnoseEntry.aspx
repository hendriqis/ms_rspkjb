<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="NursingDiagnoseEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Nursing.Program.NursingDiagnoseEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

            //#region Parent
            function getParentFilterExpression() {
                var filterExpression = "IsDeleted = 0 AND NurseDomainClassParentID = " + cboNurseDomain.GetValue();
                return filterExpression;
            }

            $('#lblNurseClass.lblLink').click(function () {
                openSearchDialog('nurseDomainClass', getParentFilterExpression(), function (value) {
                    $('#<%=txtClassCode.ClientID %>').val(value);
                    onTxtClassCodeChanged(value);
                });
            });

            $('#<%=txtClassCode.ClientID %>').change(function () {
                onTxtClassCodeChanged($(this).val());
            });

            function onTxtClassCodeChanged(value) {
                var filterExpression = getParentFilterExpression() + " AND NurseDomainClassCode = '" + value + "'";
                Methods.getObject('GetNursingDomainClassList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnNurseDomainClassID.ClientID %>').val(result.NurseDomainClassID);
                        $('#<%=txtClassName.ClientID %>').val(result.NurseDomainClassText);
                    }
                    else {
                        $('#<%=hdnNurseDomainClassID.ClientID %>').val('');
                        $('#<%=txtClassCode.ClientID %>').val('');
                        $('#<%=txtClassName.ClientID %>').val('');
                    }
                });
            }
            //#endregion


        }

        function onCboNurseDomainValueChanged() {
            $('#<%=hdnNurseDomainClassID.ClientID %>').val('');
            $('#<%=txtClassCode.ClientID %>').val('');
            $('#<%=txtClassName.ClientID %>').val('');
        }       
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Diagnosa Keperawatan")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>                    
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Diagnosa")%></label></td>
                        <td><asp:TextBox ID="txtNursingDiagnoseCode" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Diagnosa")%></label></td>
                        <td><asp:TextBox ID="txtNursingDiagnoseName" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kategori")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboNurseDomain" ClientInstanceName="cboNurseDomain" Width="250px" runat="server">
                                <ClientSideEvents ValueChanged="function(s){ onCboNurseDomainValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink lblMandatory" id="lblNurseClass"><%=GetLabel("Sub Kategori")%></label></td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnNurseDomainClassID" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtClassCode" Width="100%" runat="server" /></td>
                                    <td><asp:TextBox ID="txtClassName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jenis Diagnosa")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" Width="250px" ID="cboGCNursingDiagnosisType" /></td>
                    </tr>
                    <tr valign="top">
                        <td class="tdLabel" style="margin-top:2px"><label class="lblnormal"><%=GetLabel("Definisi")%></label></td>
                        <td><asp:TextBox ID="txtDescription" TextMode="MultiLine" Rows="5" Width="100%" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
