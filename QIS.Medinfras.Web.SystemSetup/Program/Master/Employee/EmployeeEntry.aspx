<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="EmployeeEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.EmployeeEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
        
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    
    <script type="text/javascript" id="dxss_patiententryctl">
        function onLoad() {
            registerCollapseExpandHandler();
            setDatePicker('<%=txtDOB.ClientID %>');
            

            //#region Zip Code
            $('#lblZipCode.lblLink').click(function () {
                openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
                    $('#<%=txtZipCode.ClientID %>').val(value);
                    onTxtZipCodeChanged(value);
                });
            });

            $('#<%=txtZipCode.ClientID %>').change(function () {
                onTxtZipCodeChanged($(this).val());
            });

            function onTxtZipCodeChanged(value) {
                var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCode.ClientID %>').val(result.ID);
                        $('#<%=txtCity.ClientID %>').val(result.City);
                        $('#<%=txtCounty.ClientID %>').val(result.County);
                        $('#<%=txtDistrict.ClientID %>').val(result.District);
                        $('#<%=txtCity.ClientID %>').val(result.City);
                        $('#<%=txtProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                        $('#<%=txtProvinceName.ClientID %>').val(result.Province);
                    }
                    else {
                        $('#<%=hdnZipCode.ClientID %>').val('');
                        $('#<%=txtZipCode.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Province
            function onGetSCProvinceFilterExpression() {
                var filterExpression = "<%:OnGetSCProvinceFilterExpression() %>";
                return filterExpression;
            }

            $('#lblProvince.lblLink').click(function () {
                openSearchDialog('stdcode', onGetSCProvinceFilterExpression(), function (value) {
                    $('#<%=txtProvinceCode.ClientID %>').val(value);
                    onTxtProvinceCodeChanged(value);
                });
            });

            $('#<%=txtProvinceCode.ClientID %>').change(function () {
                onTxtProvinceCodeChanged($(this).val());
            });

            function onTxtProvinceCodeChanged(value) {
                var filterExpression = onGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtProvinceName.ClientID %>').val(result.StandardCodeName);
                    else {
                        $('#<%=txtProvinceCode.ClientID %>').val('');
                        $('#<%=txtProvinceName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }

    </script>
    
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Pegawai")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td valign="top">
                <table width="100%">
                    <tr>
                        <td style="padding:5px; vertical-align:top">
                            <h4 class="h4expanded"><%=GetLabel("Data Personal")%></h4>
                            <div class="containerTblEntryContent">
                                <table class="tblEntryContent" style="width:100%">
                                    <colgroup>
                                        <col style="width:35%"/>
                                        <col style="width:65%"/>
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Karyawan")%></label></td>
                                        <td><asp:TextBox ID="txtEmployeeCode" Width="120px" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Inisial")%></label></td>
                                        <td><asp:TextBox ID="txtInitial" Width="120px" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Gelar Depan")%></label></td>
                                        <td><dxe:ASPxComboBox ID="cboTitle" ClientInstanceName="cboTitle" Width="120px" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Karyawan")%></label></td>
                                        <td>
                                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width:49%"/>
                                                    <col style="width:3px"/>
                                                    <col/>
                                                </colgroup>
                                                <tr>
                                                    <td><asp:TextBox ID="txtFirstName" Width="100%" runat="server" /></td>
                                                    <td>&nbsp;</td>
                                                    <td><asp:TextBox ID="txtMiddleName" Width="100%" runat="server" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Belakang")%></label></td>
                                        <td><asp:TextBox ID="txtFamilyName" Width="100%" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Gelar Belakang")%></label></td>
                                        <td><dxe:ASPxComboBox ID="cboSuffix" ClientInstanceName="cboSuffix" Width="120px" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tempat Lahir")%></label></td>
                                        <td><asp:TextBox ID="txtBirthPlace" Width="220px" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal Lahir")%></label></td>
                                        <td><asp:TextBox ID="txtDOB" Width="120px" runat="server" CssClass="datepicker" /></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td style="padding:5px; vertical-align:top">
                            <h4 class="h4expanded"><%=GetLabel("Data Alamat")%></h4>
                            <div class="containerTblEntryContent">
                                <input type="hidden" runat="server" id="hdnZipCode" value="" />
                                <table class="tblEntryContent" style="width:100%">
                                    <colgroup>
                                        <col style="width:35%"/>
                                        <col style="width:65%"/>
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblMandatory"><%=GetLabel("Jalan")%></label></td>
                                        <td><asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblLink" id="lblZipCode"><%=GetLabel("Kode Pos")%></label></td>
                                        <td><asp:TextBox ID="txtZipCode" Width="100%" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Desa / Kelurahan")%></label></td>
                                        <td><asp:TextBox ID="txtCounty" Width="100%" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kecamatan")%></label></td>
                                        <td><asp:TextBox ID="txtDistrict" Width="100%" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kota")%></label></td>
                                        <td><asp:TextBox ID="txtCity" Width="100%" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblLink" id="lblProvince"><%=GetLabel("Provinsi")%></label></td>
                                        <td>
                                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width:30%"/>
                                                    <col style="width:3px"/>
                                                    <col/>
                                                </colgroup>
                                                <tr>
                                                    <td><asp:TextBox ID="txtProvinceCode" Width="100%" runat="server" /></td>
                                                    <td>&nbsp;</td>
                                                    <td><asp:TextBox ID="txtProvinceName" Width="100%" runat="server" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table width="100%">
                    <tr>
                        <td style="padding:5px; vertical-align:top">
                            <h4 class="h4expanded"><%=GetLabel("Data Karyawan")%></h4>
                            <div class="containerTblEntryContent">
                                <table class="tblEntryContent" style="width:100%">
                                    <colgroup>
                                        <col style="width:35%"/>
                                        <col style="width:65%"/>
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Bagian")%></label></td>
                                        <td><dxe:ASPxComboBox ID="cboGCDepartment" ClientInstanceName="cboGCDepartment" Width="120px" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jabatan")%></label></td>
                                        <td><dxe:ASPxComboBox ID="cboGCOccupation" ClientInstanceName="cboGCOccupation" Width="120px" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Golongan")%></label></td>
                                        <td><dxe:ASPxComboBox ID="cboGCOccupationLevel" ClientInstanceName="cboGCOccupationLevel" Width="120px" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("NPWP")%></label></td>
                                        <td><asp:TextBox ID="txtVATRegistrationNo" Width="220px" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Status")%></label></td>
                                        <td><dxe:ASPxComboBox ID="cboGCEmployeeStatus" ClientInstanceName="cboGCEmployeeStatus" Width="120px" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding:5px; vertical-align:top">
                            <h4 class="h4expanded"><%=GetLabel("Data Kontak Karyawan")%></h4>
                            <div class="containerTblEntryContent">
                                <table class="tblEntryContent" style="width:100%">
                                    <colgroup>
                                        <col style="width:35%"/>
                                        <col style="width:65%"/>
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No HP")%></label></td>
                                        <td><asp:TextBox ID="txtMobilePhone1" Width="220px" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No HP")%></label></td>
                                        <td><asp:TextBox ID="txtMobilePhone2" Width="220px" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Email")%></label></td>
                                        <td><asp:TextBox ID="txtEmail1" CssClass="email" Width="220px" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Email")%></label></td>
                                        <td><asp:TextBox ID="txtEmail2" CssClass="email" Width="220px" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Ext. Kantor")%></label></td>
                                        <td><asp:TextBox ID="txtOfficeExtension" Width="220px" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding:5px; vertical-align:top">
                            <h4 class="h4expanded"><%=GetLabel("Informasi Lain")%></h4>
                            <div class="containerTblEntryContent">
                                <table class="tblEntryContent" style="width:100%">
                                    <colgroup>
                                        <col style="width:35%"/>
                                        <col style="width:65%"/>
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("File Foto")%></label></td>
                                        <td><asp:TextBox ID="txtPictureFileName" Width="100%" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Catatan")%></label></td>
                                        <td><asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
