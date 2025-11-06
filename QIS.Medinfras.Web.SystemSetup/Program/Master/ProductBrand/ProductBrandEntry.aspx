<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="ProductBrandEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ProductBrandEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region Manufacturer
            $('#lblManufacturer.lblLink').click(function () {
                openSearchDialog('manufacturer', 'IsDeleted = 0', function (value) {
                    $('#<%=txtManufacturerCode.ClientID %>').val(value);
                    onTxtManufacturerCodeChanged(value);
                });
            });

            $('#<%=txtManufacturerCode.ClientID %>').change(function () {
                onTxtManufacturerCodeChanged($(this).val());
            });

            function onTxtManufacturerCodeChanged(value) {
                var filterExpression = "ManufacturerCode = '" + value + "'";
                Methods.getObject('GetManufacturerList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnManufacturerID.ClientID %>').val(result.ManufacturerID);
                        $('#<%=txtManufacturerName.ClientID %>').val(result.ManufacturerName);
                    }
                    else {
                        $('#<%=hdnManufacturerID.ClientID %>').val('');
                        $('#<%=txtManufacturerCode.ClientID %>').val('');
                        $('#<%=txtManufacturerName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Product Brand")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Product Brand Code")%></label></td>
                        <td><asp:TextBox ID="txtProductBrandCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Product Brand Name")%></label></td>
                        <td><asp:TextBox ID="txtProductBrandName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink lblMandatory" id="lblManufacturer"><%=GetLabel("Manufacturer")%></label></td>
                        <td>
                            <input type="hidden" id="hdnManufacturerID" value="" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtManufacturerCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtManufacturerName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
