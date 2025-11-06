<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="RoomEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.RoomEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onBeforeGoToListPage(mapForm) {
            mapForm.appendChild(createInputHiddenPost("healthcareID", $('#<%=hdnHealthcareID.ClientID %>').val()));
        }

        $('#<%=txtPrefix.ClientID %>').keyup(function () {
            var Prefix = $(this).val();
            if (Prefix.length > 2) {
                Prefix = Prefix.substring(0, Prefix.length - 1);
                $(this).val(Prefix);
                showToast('Warning', 'Maximal 2 Character');
            }
        });

        $(function () {
            //#region Item Service
            $('#lblTarifKamarAddData').live('click', function () {
                openSearchDialog('vitemservice', "IsDeleted = 0 AND GCItemType = '" + Constant.ItemGroupMaster.SERVICE + "' AND GCItemStatus != '" + Constant.ItemStatus.ITEM_STATUS_IN_ACTIVE + "'", function (value) {
                    $('#<%=txtItemServiceCode.ClientID %>').val(value);
                    ontxtItemServiceCodeChanged(value);
                });
            });
        });

        function ontxtItemServiceCodeChanged(value) {
            var filterExpression = "ItemCode ='" + value + "' AND GCItemType = '" + Constant.ItemGroupMaster.SERVICE + "' AND GCItemStatus != '" + Constant.ItemStatus.ITEM_STATUS_IN_ACTIVE + "' AND IsDeleted = 0";
            Methods.getObject('GetvItemServiceList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtItemServiceName.ClientID %>').val(result.ItemName1);
                    $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                }
                else {
                    $('#<%=txtItemServiceName.ClientID %>').val('');
                    $('#<%=txtItemServiceCode.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                }

            });
        }

        $('#<%:txtItemServiceCode.ClientID %>').live('change', function () {
            ontxtItemServiceCodeChanged($(this).val());
        });

    </script>
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareID" runat="server" value="" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Kamar")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRoomCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Kamar")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRoomName" Width="500px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblTarifKamarAddData">
                                <%= GetLabel("Tarif Kamar")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtItemServiceCode" Width="100px" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtItemServiceName" Width="400px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkRoomInPatientWard" runat="server" />
                            <%=GetLabel("Kamar Rawat Inap")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsGenderValidation" runat="server" />
                            <%=GetLabel("Menggunakan Validasi Gender (Aplicares)")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Deposit")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDepositAmount" Width="300px" CssClass="number" Text="0" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tipe Kamar")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboGCRoomType" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Grup Kelas (RM)")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboGCClassGroup" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Grup Lantai (RM)")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboGCFloorGroup" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Prefix Nomor Tiket")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrefix" Width="300px" MaxLength="2" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
