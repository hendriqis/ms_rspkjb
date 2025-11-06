<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="NursingItemGroupEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Nursing.Program.NursingItemGroupEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

            //#region Parent
            function getParentFilterExpression() {
                var filterExpression = "IsHeader = 1";
                return filterExpression;
            }

            $('#lblParent.lblLink').click(function () {
                openSearchDialog('nurseItemGroup', getParentFilterExpression(), function (value) {
                    $('#<%=txtParentCode.ClientID %>').val(value);
                    onTxtParentCodeChanged(value);
                });
            });

            $('#<%=txtParentCode.ClientID %>').change(function () {
                onTxtParentCodeChanged($(this).val());
            });

            function onTxtParentCodeChanged(value) {
                var filterExpression = getParentFilterExpression() + " AND NursingItemGroupSubGroupCode = '" + value + "'";
                Methods.getObject('GetNursingItemGroupSubGroupList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParentID.ClientID %>').val(result.NursingItemGroupSubGroupID);
                        $('#<%=txtParentName.ClientID %>').val(result.NursingItemGroupSubGroupText);
                    }
                    else {
                        $('#<%=hdnParentID.ClientID %>').val('');
                        $('#<%=txtParentCode.ClientID %>').val('');
                        $('#<%=txtParentName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#<%=chkIsNursingOutcome.ClientID %>').die('change');
            $('#<%=chkIsNursingOutcome.ClientID %>').live('change', function () {
                var isChecked = $(this).is(":checked");
                if (isChecked) {
                    $('#<%=chkIsSubjectiveObjectiveData.ClientID %>').prop("checked", false)
                }
            });

            $('#<%=chkIsSubjectiveObjectiveData.ClientID %>').die('change');
            $('#<%=chkIsSubjectiveObjectiveData.ClientID %>').live('change', function () {
                var isChecked = $(this).is(":checked");
                if (isChecked) {
                    $('#<%=chkIsNursingOutcome.ClientID %>').prop("checked", false)
                }
            });
        }        
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:200px"/>
                    </colgroup>                    
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode")%></label></td>
                        <td><asp:TextBox ID="txtNursingItemGroupCode" Width="150px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Deskripsi/Keterangan")%></label></td>
                        <td><asp:TextBox ID="txtNursingItemGroupText" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblParent"><%=GetLabel("Kode Induk")%></label></td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnParentID" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:150px"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtParentCode" Width="150px" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtParentName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top"><label class="lblMandatory"><%=GetLabel("Teks tercetak")%></label></td>
                        <td><asp:TextBox ID="txtDisplayCaption" Width="100%" Rows="2" TextMode="MultiLine" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Jenis Diagnosa Keperawatan")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" Width="150px" ID="cboGCNursingDiagnosisType" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Jenis Evaluasi")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" Width="150px" ID="cboGCNursingEvaluation" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Urutan Tampilan")%></label></td>
                        <td><asp:TextBox ID="txtDisplayOrder" CssClass="number" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkIsHeader" Text=" Induk Komponen" />
                            &nbsp;&nbsp;
                            <asp:CheckBox runat="server" ID="chkIsNursingOutcome" Text=" Luaran" />
                            &nbsp;&nbsp;
                            <asp:CheckBox runat="server" ID="chkIsSubjectiveObjectiveData" Text=" Data Subjective/Objective" />
                            &nbsp;&nbsp;
                            <asp:CheckBox runat="server" ID="chkIsShowInReport" Text=" Tampilkan di Cetakan" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
