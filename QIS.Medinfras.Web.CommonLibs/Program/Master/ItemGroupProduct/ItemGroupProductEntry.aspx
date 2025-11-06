<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="ItemGroupProductEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ItemGroupProductEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region Parent
            function onGetParentFilterExpression() {
                var filterExpression = "<%:OnGetParentFilterExpression() %>";
                return filterExpression;
            }

            $('#lblParent.lblLink').click(function () {
                openSearchDialog('itemgroup', onGetParentFilterExpression(), function (value) {
                    $('#<%=txtParentCode.ClientID %>').val(value);
                    onTxtParentCodeChanged(value);
                });
            });

            $('#<%=txtParentCode.ClientID %>').change(function () {
                onTxtParentCodeChanged($(this).val());
            });

            function onTxtParentCodeChanged(value) {
                var filterExpression = onGetParentFilterExpression() + " AND ItemGroupCode = '" + value + "'";
                Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParentID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=txtParentName.ClientID %>').val(result.ItemGroupName1);
                    }
                    else {
                        $('#<%=hdnParentID.ClientID %>').val('');
                        $('#<%=txtParentCode.ClientID %>').val('');
                        $('#<%=txtParentName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Revenue Sharing
            $('#lblRevenueSharing.lblLink').click(function () {
                openSearchDialog('revenuesharing', 'IsDeleted = 0', function (value) {
                    $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
                    onTxtRevenueSharingCodeChanged(value);
                });
            });

            $('#<%=txtRevenueSharingCode.ClientID %>').change(function () {
                onTxtRevenueSharingCodeChanged($(this).val());
            });

            function onTxtRevenueSharingCodeChanged(value) {
                var filterExpression = "RevenueSharingCode = '" + value + "'";
                Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                        $('#<%=txtRevenueSharingName.ClientID %>').val(result.RevenueSharingName);
                    }
                    else {
                        $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                        $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                        $('#<%=txtRevenueSharingName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }

        function onCboItemTypeValueChanged() {
            $('#<%=hdnParentID.ClientID %>').val('');
            $('#<%=txtParentCode.ClientID %>').val('');
            $('#<%=txtParentName.ClientID %>').val('');
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnTypeItem" runat="server" value="" />
    <input type="hidden" id="hdnItemGroup" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Item Group")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Kelompok")%></label></td>
                        <td><asp:TextBox ID="txtItemGroupCode" Width="100px" runat="server" /></td>
                    </tr>
                    <tr id="trItemtype" runat="server">
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jenis Item")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboItemType" ClientInstanceName="cboItemType" Width="150px" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { onCboItemTypeValueChanged() }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Kelompok")%></label></td>
                        <td><asp:TextBox ID="txtItemGroupName1" Width="100%" runat="server" /></td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Item Group Name 2")%></label></td>
                        <td><asp:TextBox ID="txtItemGroupName2" Width="300px" runat="server" /></td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblLink" id="lblRevenueSharing"><%=GetLabel("Formula Honor Dokter")%></label></td>
                        <td>
                            <input type="hidden" id="hdnRevenueSharingID" value="" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtRevenueSharingCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtRevenueSharingName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblParent"><%=GetLabel("Kode Induk")%></label></td>
                        <td>
                            <input type="hidden" id="hdnParentID" value="" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtParentCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtParentName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Urutan Cetak")%></label></td>
                        <td><asp:TextBox ID="txtPrintOrder" CssClass="number" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td><asp:CheckBox ID="chkIsHeader" runat="server" /><%=GetLabel("Induk bagi Kelompok Lain")%></td>
                    </tr>
                </table>
            </td>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                        <col style="width:50px"/>
                        <col />
                    </colgroup>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nilai CITO")%></label></td>
                        <td><asp:CheckBox ID="chkIsCITOInPercentage" runat="server" /> %</td>
                        <td><asp:TextBox ID="txtCITOAmount" CssClass="txtCurrency" Width="100px" runat="server" /></td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nilai Penyulit")%></label></td>
                        <td><asp:CheckBox ID="chkIsComplicationInPercentage" runat="server" /> %</td>
                        <td><asp:TextBox ID="txtComplicationAmount" CssClass="txtCurrency" Width="100px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
