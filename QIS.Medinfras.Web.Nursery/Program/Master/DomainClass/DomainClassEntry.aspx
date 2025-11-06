<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="DomainClassEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Nursing.Program.DomainClassEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            var constDomain = "<%=OnGetConstantNursingDomain() %>";

            //#region Parent
            function getParentFilterExpression() {
                var filterExpression = "GCNurseDomainClassType = '" + constDomain + "'";
                return filterExpression;
            }

            $('#lblParent.lblLink').click(function () {
                openSearchDialog('nurseDomainClass', getParentFilterExpression(), function (value) {
                    $('#<%=txtParentCode.ClientID %>').val(value);
                    onTxtParentCodeChanged(value);
                });
            });

            $('#<%=txtParentCode.ClientID %>').change(function () {
                onTxtParentCodeChanged($(this).val());
            });

            function onTxtParentCodeChanged(value) {
                var filterExpression = getParentFilterExpression() + " AND NurseDomainClassCode = '" + value + "'";
                Methods.getObject('GetNursingDomainClassList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParentID.ClientID %>').val(result.NurseDomainClassID);
                        $('#<%=txtParentName.ClientID %>').val(result.NurseDomainClassText);
                    }
                    else {
                        $('#<%=hdnParentID.ClientID %>').val('');
                        $('#<%=txtParentCode.ClientID %>').val('');
                        $('#<%=txtParentName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            onCboDomainClassTypeValueChanged();
        }

        function onCboDomainClassTypeValueChanged() {
            var constDomain = "<%=OnGetConstantNursingDomain() %>";
            if (cboNurseDomainClassType.GetValue() == constDomain)
                $("#trParent").hide();
            else
                $("#trParent").show();
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Klasifikasi Diagnosa Keperawatan")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode")%></label></td>
                        <td>                  
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>                                    
                                        <td><asp:TextBox ID="txtNurseDomainClassCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboNurseDomainClassType" ClientInstanceName="cboNurseDomainClassType" Width="220px" runat="server">
                                                <ClientSideEvents ValueChanged="function(s){ onCboDomainClassTypeValueChanged(); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                            </table>    
                        </td>                  
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Klasifikasi")%></label></td>
                        <td><asp:TextBox ID="txtNurseDomainClassText" Width="351px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal" style="vertical-align: top; padding-top: 5px;"><%=GetLabel("Remarks")%></label></td>
                        <td><asp:TextBox ID="txtRemarksText" TextMode="MultiLine" Rows="2" Width="347px" runat="server" /></td>
                    </tr>
                    <tr id="trParent">
                        <td class="tdLabel"><label class="lblLink lblMandatory" id="lblParent"><%=GetLabel("Parent")%></label></td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnParentID" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtParentCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtParentName" Width="219px" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
