<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="ItemProductEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ItemProductEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region ATC Class
            $('#lblATCClass.lblLink').click(function () {
                openSearchDialog('atcclass', 'IsDeleted = 0', function (value) {
                    $('#<%=txtATCClassCode.ClientID %>').val(value);
                    onTxtATCClassCodeChanged(value);
                });
            });

            $('#<%=txtATCClassCode.ClientID %>').change(function () {
                onTxtATCClassCodeChanged($(this).val());
            });

            function onTxtATCClassCodeChanged(value) {
                var filterExpression = "ATCClassCode = '" + value + "'";
                Methods.getObject('GetATCClassificationList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnATCClassID.ClientID %>').val(result.ATCClassID);
                        $('#<%=txtATCClassName.ClientID %>').val(result.ATCClassName);
                    }
                    else {
                        $('#<%=hdnATCClassID.ClientID %>').val('');
                        $('#<%=txtATCClassCode.ClientID %>').val('');
                        $('#<%=txtATCClassName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Kamus Farmasi dan Alkes
            $('#lblKFAReferenceID.lblLink').click(function () {
                openSearchDialog('kfaReference', 'IsDeleted = 0', function (value) {
                    $('#<%=txtKFAReferenceCode.ClientID %>').val(value);
                    onTxtKFAReferenceCodeChanged(value);
                });
            });

            $('#<%=txtKFAReferenceCode.ClientID %>').change(function () {
                onTxtKFAReferenceCodeChanged($(this).val());
            });

            function onTxtKFAReferenceCodeChanged(value) {
                var filterExpression = "KFACode = '" + value + "'";
                Methods.getObject('GetKFAReferenceList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnKFAReferenceID.ClientID %>').val(result.ID);
                        $('#<%=txtKFAReferenceName.ClientID %>').val(result.KFAName);
                    }
                    else {
                        $('#<%=hdnKFAReferenceID.ClientID %>').val('');
                        $('#<%=txtKFAReferenceCode.ClientID %>').val('');
                        $('#<%=txtKFAReferenceName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Pharmacogenomics
            $('#lblPharmacogeneticID.lblLink').click(function () {
                openSearchDialog('pharmacogenomic', 'IsDeleted = 0', function (value) {
                    $('#<%=txtPharmacogeneticCode.ClientID %>').val(value);
                    onTxtPharmacogeneticCodeChanged(value);
                });
            });

            $('#<%=txtPharmacogeneticCode.ClientID %>').change(function () {
                onTxtPharmacogeneticCodeChanged($(this).val());
            });

            function onTxtPharmacogeneticCodeChanged(value) {
                var filterExpression = "PharmacogeneticCode = '" + value + "'";
                Methods.getObject('GetPharmacogeneticList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnPharmacogeneticID.ClientID %>').val(result.PharmacogeneticID);
                        $('#<%=txtPharmacogeneticName.ClientID %>').val(result.PharmacogeneticName);
                    }
                    else {
                        $('#<%=hdnPharmacogeneticID.ClientID %>').val('');
                        $('#<%=txtPharmacogeneticCode.ClientID %>').val('');
                        $('#<%=txtPharmacogeneticName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Item Group
            $('#lblItemGroup.lblLink').click(function () {
                var isVisible = $('#<%=hdnIsVisibleAttribute.ClientID %>').val();
                var oID = $('#<%=hdnID.ClientID %>').val();
                if (oID != null && oID != "" && oID != 0) {
                    if (isVisible == "0") {
                        var filterExpression = "GCItemType = '" + $('#<%=hdnGCItemType.ClientID %>').val() + "' AND IsDeleted = 0";
                        openSearchDialog('itemgroup', filterExpression, function (value) {
                            $('#<%=txtItemGroupCode.ClientID %>').val(value);
                            onTxtItemGroupCodeChanged(value);
                        });
                    } else {
                        displayErrorMessageBox("Gagal", "Tidak diperbolehkan mengganti data Kelompok Barang");
                    }
                } else {
                    var filterExpression = "GCItemType = '" + $('#<%=hdnGCItemType.ClientID %>').val() + "' AND IsDeleted = 0";
                    openSearchDialog('itemgroup', filterExpression, function (value) {
                        $('#<%=txtItemGroupCode.ClientID %>').val(value);
                        onTxtItemGroupCodeChanged(value);
                    });
                }
            });

            $('#<%=txtItemGroupCode.ClientID %>').change(function () {
                onTxtItemGroupCodeChanged($(this).val());
            });

            function onTxtItemGroupCodeChanged(value) {
                var filterExpression = "ItemGroupCode = '" + value + "'";
                Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);
                    }
                    else {
                        $('#<%=hdnItemGroupID.ClientID %>').val('');
                        $('#<%=txtItemGroupCode.ClientID %>').val('');
                        $('#<%=txtItemGroupName.ClientID %>').val('');
                    }
                });
            }
            //#endregion 

            //#region IsTemperatur
            $('#<%=chkIsTempereture.ClientID %>').change(function () {
                if ($(this).is(':checked')) {
                    $('#trKeterangan').removeAttr('style');

                }
                else {
                    $('#trKeterangan').attr('style', "display:none");
                }
            });

            $('#<%=chkIsTempereture.ClientID %>').trigger("change");
            //#endregion  

            //#region MIMS Class
            $('#lblMIMSClass.lblLink').click(function () {
                openSearchDialog('mimsclass', 'IsDeleted = 0', function (value) {
                    $('#<%=txtMIMSClassCode.ClientID %>').val(value);
                    onTxtMIMSClassCodeChanged(value);
                });
            });

            $('#<%=txtMIMSClassCode.ClientID %>').change(function () {
                onTxtMIMSClassCodeChanged($(this).val());
            });

            function onTxtMIMSClassCodeChanged(value) {
                var filterExpression = "MIMSClassCode = '" + value + "'";
                Methods.getObject('GetMIMSClassList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnMIMSClassID.ClientID %>').val(result.MIMSClassID);
                        $('#<%=txtMIMSClassName.ClientID %>').val(result.MIMSClassName);
                    }
                    else {
                        $('#<%=hdnMIMSClassID.ClientID %>').val('');
                        $('#<%=txtMIMSClassCode.ClientID %>').val('');
                        $('#<%=txtMIMSClassName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region MIMS Reference
            $('#lblMIMSReferece.lblLink').click(function () {
                openSearchDialog('mimsreference', 'IsDeleted = 0', function (value) {
                    onTxtMIMSReferenceChanged(value);
                });
            });

            $('#<%=txtGUIDMIMSReferece.ClientID %>').change(function () {
                onTxtMIMSReferenceGUIDChanged($(this).val());
            });

            function onTxtMIMSReferenceChanged(value) {
                var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetMIMSReferenceList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnMIMSRefereceID.ClientID %>').val(result.ID);
                        $('#<%=txtGUIDMIMSReferece.ClientID %>').val(result.GUID);
                    }
                    else {
                        $('#<%=hdnMIMSRefereceID.ClientID %>').val('');
                        $('#<%=txtGUIDMIMSReferece.ClientID %>').val('');
                    }
                });
            }

            function onTxtMIMSReferenceGUIDChanged(value) {
                var filterExpression = "GUID = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetMIMSReferenceList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnMIMSRefereceID.ClientID %>').val(result.ID);
                        $('#<%=txtGUIDMIMSReferece.ClientID %>').val(result.GUID);
                    }
                    else {
                        $('#<%=hdnMIMSRefereceID.ClientID %>').val('');
                        $('#<%=txtGUIDMIMSReferece.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Product Brand
            $('#lblProductBrand.lblLink').click(function () {
                openSearchDialog('productbrand', 'IsDeleted = 0', function (value) {
                    $('#<%=txtProductBrandCode.ClientID %>').val(value);
                    onTxtProductBrandCodeChanged(value);
                });
            });

            $('#<%=txtProductBrandCode.ClientID %>').change(function () {
                onTxtProductBrandCodeChanged($(this).val());
            });

            function onTxtProductBrandCodeChanged(value) {
                var filterExpression = "IsDeleted = 0 AND ProductBrandCode = '" + value + "'";
                Methods.getObject('GetProductBrandList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnProductBrandID.ClientID %>').val(result.ProductBrandID);
                        $('#<%=txtProductBrandName.ClientID %>').val(result.ProductBrandName);
                    }
                    else {
                        $('#<%=hdnProductBrandID.ClientID %>').val('');
                        $('#<%=txtProductBrandCode.ClientID %>').val('');
                        $('#<%=txtProductBrandName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

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
                var filterExpression = "IsDeleted = 0 AND ManufacturerCode = '" + value + "'";
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

            //#region Product Line
            function GetFilterExpressionProductLine() {
                var filterPL = "IsDeleted = 0 AND GCItemType IN ('" + $('#<%=hdnGCItemType.ClientID %>').val() + "')";
                return filterPL;
            }

            $('#lblProductLine.lblLink').click(function () {
                var oID = $('#<%=hdnID.ClientID %>').val();
                var isVisible = $('#<%=hdnIsVisibleAttribute.ClientID %>').val();
                var filterPL = GetFilterExpressionProductLine();
                if (oID != null && oID != "" && oID != 0) {
                    if (isVisible == "0") {
                        openSearchDialog('productline', filterPL, function (value) {
                            $('#<%=txtProductLineCode.ClientID %>').val(value);
                            onTxtProductLineCodeChanged(value);
                        });
                    } else {
                        displayErrorMessageBox("Gagal", "Tidak diperbolehkan mengganti data Product Line");
                    }
                } else {
                    openSearchDialog('productline', filterPL, function (value) {
                        $('#<%=txtProductLineCode.ClientID %>').val(value);
                        onTxtProductLineCodeChanged(value);
                    });
                }
            });

            $('#<%=txtProductLineCode.ClientID %>').change(function () {
                onTxtProductLineCodeChanged($(this).val());
            });

            function onTxtProductLineCodeChanged(value) {
                var filterExpression = GetFilterExpressionProductLine() + " AND ProductLineCode = '" + value + "'";
                Methods.getObject('GetProductLineList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnProductLineID.ClientID %>').val(result.ProductLineID);
                        $('#<%=txtProductLineName.ClientID %>').val(result.ProductLineName);

                        if (result.IsInventoryItem) {
                            $('#<%=hdnPLIsInventoryItem.ClientID %>').val("1");
                            $('#<%=chkIsInventoryItem.ClientID %>').prop('checked', true);
                        } else {
                            $('#<%=hdnPLIsInventoryItem.ClientID %>').val("0");
                            $('#<%=chkIsInventoryItem.ClientID %>').prop('checked', false);
                        }

                        if (result.IsFixedAsset) {
                            $('#<%=hdnPLIsFixedAsset.ClientID %>').val("1");
                            $('#<%=chkIsFixedAsset.ClientID %>').prop('checked', true);
                        } else {
                            $('#<%=hdnPLIsFixedAsset.ClientID %>').val("0");
                            $('#<%=chkIsFixedAsset.ClientID %>').prop('checked', false);
                        }

                        if (result.IsConsigmentItem) {
                            $('#<%=hdnPLIsConsigmentItem.ClientID %>').val("1");
                            $('#<%=chkIsConsigmentItem.ClientID %>').prop('checked', true);
                        } else {
                            $('#<%=hdnPLIsConsigmentItem.ClientID %>').val("0");
                            $('#<%=chkIsConsigmentItem.ClientID %>').prop('checked', false);
                        }
                    }
                    else {
                        $('#<%=hdnProductLineID.ClientID %>').val('');
                        $('#<%=txtProductLineCode.ClientID %>').val('');
                        $('#<%=txtProductLineName.ClientID %>').val('');

                        $('#<%=hdnPLIsInventoryItem.ClientID %>').val("0");
                        $('#<%=hdnPLIsFixedAsset.ClientID %>').val("0");
                        $('#<%=hdnPLIsConsigmentItem.ClientID %>').val("0");

                        $('#<%=chkIsInventoryItem.ClientID %>').prop('checked', false);
                        $('#<%=chkIsFixedAsset.ClientID %>').prop('checked', false);
                        $('#<%=chkIsConsigmentItem.ClientID %>').prop('checked', false);
                    }
                });
            }
            //#endregion

            //#region Foto Item
            $('#<%=FileUpload1.ClientID %>').live('change', function () {
                readURL(this);
                var fileName = $('#<%=FileUpload1.ClientID %>').val();
                $('#<%=txtFileName.ClientID %>').val(fileName);
                $('#<%=hdnFileName.ClientID %>').val(fileName);

                //fileName = fileName.split('.')[0];
                if ($('#<%=imgPreview.ClientID %>').width() > $('#<%=imgPreview.ClientID %>').height())
                    $('#<%=imgPreview.ClientID %>').width('150px');
                else
                    $('#<%=imgPreview.ClientID %>').height('150px');
            });

            function readURL(input) {
                if (input.files && input.files[0]) {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#<%=hdnUploadedFile1.ClientID %>').val(e.target.result);
                        $('#<%=imgPreview.ClientID %>').attr('src', e.target.result);
                    }

                    reader.readAsDataURL(input.files[0]);
                }
                else {
                    $('#<%=imgPreview.ClientID %>').attr('src', input.value);
                }
            }

            $('#btnUploadFile').live('click', function () {
                document.getElementById('<%= FileUpload1.ClientID %>').click();
            });

            $('#btnDeleteFile').live('click', function () {
                readURL(this);
                var fileName = $('#<%=FileUpload1.ClientID %>').val();
                $('#<%=txtFileName.ClientID %>').val(fileName);
                $('#<%=hdnFileName.ClientID %>').val(fileName);
            });
            //#endregion

            //#region Billing Group
            $('#lblBillingGroup.lblLink').click(function () {
                openSearchDialog('billinggroup', 'IsDeleted = 0', function (value) {
                    $('#<%=txtBillingGroupCode.ClientID %>').val(value);
                    onBillingGroupCodeChanged(value);
                });
            });

            $('#<%=txtBillingGroupCode.ClientID %>').change(function () {
                onBillingGroupCodeChanged($(this).val());
            });

            function onBillingGroupCodeChanged(value) {
                var filterExpression = "BillingGroupCode = '" + value + "'";
                Methods.getObject('GetBillingGroupList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnBillingGroupID.ClientID %>').val(result.BillingGroupID);
                        $('#<%=txtBillingGroupName.ClientID %>').val(result.BillingGroupName1);
                    }
                    else {
                        $('#<%=hdnBillingGroupID.ClientID %>').val('');
                        $('#<%=txtBillingGroupCode.ClientID %>').val('');
                        $('#<%=txtBillingGroupName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region E-Klaim Parameter
            $('#<%:lblEKlaimParameter.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('eklaimparameter', 'IsDeleted = 0', function (value) {
                    $('#<%=txtEKlaimParameterCode.ClientID %>').val(value);
                    ontxtEKlaimParameterCodeChanged(value);
                });
            });

            $('#<%=txtEKlaimParameterCode.ClientID %>').change(function () {
                ontxtEKlaimParameterCodeChanged($(this).val());
            });

            function ontxtEKlaimParameterCodeChanged(value) {
                var filterExpression = "EKlaimParameterCode = '" + value + "'";
                Methods.getObject('GetEKlaimParameterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnEKlaimParameterID.ClientID %>').val(result.EKlaimParameterID);
                        $('#<%=txtEKlaimParameterName.ClientID %>').val(result.EKlaimParameterName);
                    }
                    else {
                        $('#<%=hdnEKlaimParameterID.ClientID %>').val('');
                        $('#<%=txtEKlaimParameterCode.ClientID %>').val('');
                        $('#<%=txtEKlaimParameterName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Default Signa
            $('#lblSigna.lblLink').click(function () {
                openSearchDialog('signa', 'IsDeleted = 0', function (value) {
                    $('#<%=txtSignaLabel.ClientID %>').val(value);
                    txtSignaLabelChanged(value);
                });
            });

            $('#<%=txtSignaLabel.ClientID %>').change(function () {
                txtSignaLabelChanged($(this).val());
            });

            function txtSignaLabelChanged(value) {
                var filterExpression = "IsDeleted = 0 AND SignaLabel = '" + value + "'";
                Methods.getObject('GetvSignaList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnDefaultSignaID.ClientID %>').val(result.SignaID);
                        $('#<%=txtSignaName1.ClientID %>').val(result.SignaName1);
                    }
                    else {
                        $('#<%=hdnDefaultSignaID.ClientID %>').val('');
                        $('#<%=txtSignaLabel.ClientID %>').val('');
                        $('#<%=txtSignaName1.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Kategori Antibiotik
            $('#<%=chkIsAntibiotics.ClientID %>').change(function () {
                if ($(this).is(':checked')) {
                    $('#trAntibiotics').removeAttr('style');

                }
                else {
                    $('#trAntibiotics').attr('style', "display:none");
                }
            });

            $('#<%=chkIsTempereture.ClientID %>').trigger("change");
            //#endregion  

            registerCollapseExpandHandler();

            $('#<%=chkIsAntibiotics.ClientID %>').trigger("change");
        }

        $('#<%:chkIsInventoryItem.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=hdnPLIsInventoryItem.ClientID %>').val("1");
            } else {
                $('#<%=hdnPLIsInventoryItem.ClientID %>').val("0");
            }
        });

        $('#<%:chkIsConsigmentItem.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=hdnPLIsConsigmentItem.ClientID %>').val("1");
            } else {
                $('#<%=hdnPLIsConsigmentItem.ClientID %>').val("0");
            }
        });

        $('#<%:chkIsFixedAsset.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=hdnPLIsFixedAsset.ClientID %>').val("1");
            } else {
                $('#<%=hdnPLIsFixedAsset.ClientID %>').val("0");
            }
        });

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnRequestID" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" id="hdnIsUsedProductLine" runat="server" value="0" />
    <input type="hidden" id="hdnFileName" runat="server" value="" />
    <input type="hidden" id="hdnIsVisibleAttribute" runat="server" value="" />
    <input type="hidden" id="hdnIsUsingDrugAlert" runat="server" value="" />
    <input type="hidden" id="hdnIsEKlaimParameterMandatory" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Item")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kode Item")%></label>
                            </td>
                            <td>
                                <table style="margin-left: -3px">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtItemCode" Width="130px" runat="server" />
                                        </td>
                                        <td id="tdCashPatient" runat="server">
                                            <asp:CheckBox ID="chkIsChargeToPatient" runat="server" /><%=GetLabel("Dibebankan ke Pasien")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kode Sistem Lama")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOldItemCode" Width="130px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Item #1")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemName1" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Item #2")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemName2" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Alternatif")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAlternateItemName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblProductBrand">
                                    <%=GetLabel("Merek Dagang")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnProductBrandID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProductBrandCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProductBrandName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblManufacturer">
                                    <%=GetLabel("Produsen Obat (Manufacturer)")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnManufacturerID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtManufacturerCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtManufacturerName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblItemGroup">
                                    <%=GetLabel("Kelompok Item")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemGroupName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblBillingGroup">
                                    <%=GetLabel("Kelompok Rincian Transaksi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnBillingGroupID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtBillingGroupCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillingGroupName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblEKlaimParameter" runat="server">
                                    <%=GetLabel("Parameter E-Klaim")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnEKlaimParameterID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtEKlaimParameterCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEKlaimParameterName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Persediaan")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Status Item Persediaan")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 130px" />
                                        <col style="width: 3px" />
                                        <col style="width: 160px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsInventoryItem" runat="server" Checked="true" /><%=GetLabel("Persediaan/Stock")%>
                                        </td>
                                        <td>
                                            &nbsp
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsProductionItem" runat="server" /><%=GetLabel("Produksi/Pengemasan Kembali")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kelompok A/B/C")%></label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblABCClass" runat="server" RepeatDirection="Horizontal" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Satuan Kecil")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboItemUnit" Width="130px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Cara Pemotongan Stock")%></label>
                            </td>
                            <td>
                                <table width="404" cellspacing="1" bgcolor="#ffd27a">
                                    <tr>
                                        <td width="50%" bgcolor="#F5F5F5">
                                            <table cellpadding="1" width="100%">
                                                <tr>
                                                    <td width="50%" class="tdLabel" align="center" style="background-color: #FFE74C3C">
                                                        <%=GetLabel("Penjualan")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="50%">
                                                        <asp:RadioButtonList ID="rblSale" runat="server" RepeatDirection="Vertical">
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="48%" bgcolor="#F5F5F5">
                                            <table cellpadding="1" width="100%">
                                                <tr>
                                                    <td width="50%" class="tdLabel" align="center" style="background-color: #FFE74C3C">
                                                        <%=GetLabel("Pemakaian")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="50%">
                                                        <asp:RadioButtonList ID="rblConsume" runat="server" RepeatDirection="Vertical">
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tipe Transaksi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTransactionType" Width="130px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Status Item")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboItemStatus" Width="130px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Fill Moving")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboMovingClassification" Width="130px" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Cycle Count Interval")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCountInterval" Width="200px" CssClass="number required" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Transaction Restriction")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTransactionRestriction" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Batch Control")%></label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsBatchControl" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Keuangan")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblProductLine">
                                    <%=GetLabel("Product Line")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnProductLineID" value="" runat="server" />
                                <input type="hidden" id="hdnPLIsInventoryItem" value="" runat="server" />
                                <input type="hidden" id="hdnPLIsFixedAsset" value="" runat="server" />
                                <input type="hidden" id="hdnPLIsConsigmentItem" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProductLineCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProductLineName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Skema Margin")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboMarginMarkup" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Harga Eceran Tertinggi (HET)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSuggestedPrice" CssClass="txtCurrency" runat="server" Width="130px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                &nbsp;
                            </td>
                        </tr>
                        <tr id='trUsingStandardPrice' runat="server">
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsUsingStandardPrice" Width="100%" runat="server" Text=" Prioritas Harga Dari HNA (Harga Satuan Kecil)" />
                            </td>
                        </tr>
                        <tr id='trSalesPriceUsingKatalogSupplier' runat="server">
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsSalesPriceUsingKatalogSupplier" Width="100%" runat="server"
                                    Text=" HNA (Harga Satuan Kecil) dari Katalog Supplier" />
                            </td>
                        </tr>
                        <tr id='trTariffRoundTo100' runat="server">
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsTariffRoundTo100" Width="100%" runat="server" Text=" Pembulatan ke Angka 100" />
                            </td>
                        </tr>
                        <tr id="trDiscount" runat="server">
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsDiscountCalculateHNA" Width="100%" runat="server" Text=" Setting Diskon HNA (Harga Satuan Kecil) dari Kelompok Item" />
                            </td>
                        </tr>
                        <tr id="trPPN" runat="server">
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsPPNCalculateHNA" Width="100%" runat="server" Text=" Setting PPN HNA (Harga Satuan Kecil) dari Kelompok Item" />
                            </td>
                        </tr>
                        <tr id="trFixed" runat="server">
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsFixedAsset" Width="130px" runat="server" Text=" Fixed Asset"
                                    Enabled="false" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Margin")%>
                                    (%)</label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMargin" CssClass="number" runat="server" Width="130px" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Foto Item")%></h4>
                <div class="containerTblEntryContent">
                    <table width="100%">
                        <colgroup>
                            <col width="150px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("File Name")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFileName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="4" style="height: 150px; width: 150px; border: 1px solid ActiveBorder;"
                                align="center">
                                <input type="hidden" id="hdnUploadedFile1" runat="server" value="" />
                                <img src="" runat="server" id="imgPreview" width="150" height="150" />
                            </td>
                            <td>
                                <div style="display: none">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                </div>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 120px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <input type="button" style="width: 95%" id="btnUploadFile" value="Upload" />
                                        </td>
                                        <td>
                                            <input type="button" style="width: 95%" id="btnDeleteFile" value="Delete" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=GetLabel("extension") %>
                                : jpg,jpeg,png.
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=GetLabel("Maximum upload size") %>
                                : 10MB
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Panel ID="pnlDrugInformation" runat="server">
                    <h4 class="h4expanded">
                        <%=GetLabel("Informasi Obat")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 30%" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Nama Generik")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGenericName" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kadar")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col style="width: 30%" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtDose" CssClass="number required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboDose" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Bentuk/Kemasan")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 130px" />
                                            <col style="width: 3px" />
                                            <col style="width: 130px" />
                                            <col style="width: 3px" />
                                            <col style="width: 130px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboDrugForm" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblMIMSClass">
                                        <%=GetLabel("Klasifikasi MIMS")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnMIMSClassID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtMIMSClassCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMIMSClassName" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trMIMSReference" runat="server">
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblMIMSReferece">
                                        <%=GetLabel("MIMS Reference")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnMIMSRefereceID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtGUIDMIMSReferece" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblATCClass">
                                        <%=GetLabel("Klasifikasi ATC")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnATCClassID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtATCClassCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtATCClassName" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblKFAReferenceID">
                                        <%=GetLabel("Kamus Farmasi dan Alkes")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnKFAReferenceID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtKFAReferenceCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtKFAReferenceName" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblPharmacogeneticID">
                                        <%=GetLabel("Pharmacogenomics")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnPharmacogeneticID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPharmacogeneticCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPharmacogeneticName" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Klasifikasi Obat")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboDrugClassification" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kategori Pasien Hamil")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboPregnancyCategory" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Coenam Rule")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboCoenamRule" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Rute Pemberian")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMedicationRoute" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Satuan Dosis Pemberian")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboDosingUnit" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblLink" id="lblSigna">
                                        <%=GetLabel("Default Aturan Pakai")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnDefaultSignaID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 31%" />
                                            <col style="width: 62%" />
                                            <col style="width: 10%" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtSignaLabel" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSignaName1" Width="100%" runat="server" ReadOnly="true" TabIndex="999" />
                                            </td>
                                            <td>
                                                &nbsp;
                                                <img height="20" src='<%= ResolveUrl("~/Libs/Images/Warning.png")%>' alt='' title='Default Signa akan tampil sebagai default dalam pembuatan Transaksi Resep Farmasi' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("StatusVEN")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboStatusVEN" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Fungsi Obat")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPurposeOfMedication" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Spesial Instruction")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMedicationAdministration" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Cara Penyimpanan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStorageRemarks" Width="100%" runat="server" TextMode="MultiLine"
                                        Rows="4" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                        <colgroup>
                                            <col width="33%" />
                                            <col width="33%" />
                                            <col width="33%" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsGeneric" runat="server" Text=" Obat Generik" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsPrecursor" runat="server" Text=" Precursor" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkISHAM" runat="server" Text=" High Alert Medication" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsLASA" runat="server" Text=" Look Alike Sound Alike" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsExternalMedication" runat="server" Text=" Obat Luar" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsChronic" runat="server" Text=" Obat Kronis" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsOOT" runat="server" Text=" Obat-obat Tertentu" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsInjection" runat="server" Text=" Obat Injeksi" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsAntibiotics" runat="server" Text=" Antibiotik" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkIsSuplement" Text=" Suplemen/Herbal" />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkIsPriority" Text=" Priority" />
                                            </td>
                                              <td>
                                                <asp:CheckBox ID="chkIsHasResidualEffect" runat="server" Text=" Memiliki Potensi Efek Residual" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkIsTempereture" Text=" Butuh Suhu Penyimpanan" />
                                            </td>
                                            <td  style="display:none">
                                                <asp:CheckBox runat="server" ID="chkIsRestrictiveAntibiotics" Text=" Antibiotik (PPRA)" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trAntibiotics">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kategori Antibiotik")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboAntibioticCategory" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr id="trKeterangan">
                                <td class="tdLabel" style="vertical-align: top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Temperatur")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtTemp" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Lain-lain")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel(" Spesifikasi Item")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col width="50%" />
                                        <col width="50%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkIsAllowUDD" Text=" UDD" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsControlExpired" runat="server" Text=" Kontrol Expired" />
                                        </td>
                                        <td id='tdMedicalInstrument' runat="server">
                                            <asp:CheckBox ID="chkIsMedicalInstrument" runat="server" Text=" Instrumen Medis" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsConsigmentItem" runat="server" Text=" Item Konsinyasi" Enabled="false" />
                                        </td>
                                        <td id='tdFormularium' runat="server">
                                            <asp:CheckBox ID="chkIsFormularium" runat="server" Text=" Formularium" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsCSSD" runat="server" Text=" Item CSSD" />
                                        </td>
                                        <td id='tdFormulariumBPJS' runat="server">
                                            <asp:CheckBox ID="chkIsBPJSFormularium" runat="server" Text=" Formularium BPJS" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id='tdIsImplant' runat="server">
                                            <asp:CheckBox ID="chkIsImplant" runat="server" Text=" Item Implant" />
                                        </td>
                                        <td id='tdFormulariumInHealth' runat="server">
                                            <asp:CheckBox ID="chkIsInhealthFormularium" runat="server" Text=" Formularium Inhealth" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsHiddenItem" runat="server" Text=" Item Khusus/Tersembunyi" />
                                        </td>
                                        <td id='tdFORNAS' runat="server">
                                            <asp:CheckBox ID="chkIsFORNAS" runat="server" Text=" Formularium Nasional" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsUsingSerialNumber" runat="server" Text=" Menggunakan Input Nomor Seri di Transaksi" />
                                        </td>
                                        <td id='tdEmployeeFormularium' runat="server">
                                            <asp:CheckBox ID="chkIsEmployeeFormularium" runat="server" Text=" Formularium Karyawan" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsUsingFixedAsset" runat="server" Text=" Menggunakan Input Aset di Transaksi" />
                                        </td>
                                        <td id='tdGovernmentDrugs' runat="server">
                                            <asp:CheckBox ID="chkGovernmentDrugs" runat="server" Text=" Obat Program Pemerintah" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Panel ID="pnlCustomAttribute" runat="server">
                    <h4 class="h4expanded">
                        <%=GetLabel("Custom Attribute")%></h4>
                    <asp:Repeater ID="rptCustomAttribute" runat="server">
                        <HeaderTemplate>
                            <div class="containerTblEntryContent">
                                <table class="tblEntryContent" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 30%" />
                                    </colgroup>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%#: Eval("Value") %></label>
                                </td>
                                <td>
                                    <input type="hidden" value='<%#: Eval("Code") %>' runat="server" id="hdnTagFieldCode" />
                                    <asp:TextBox ID="txtTagField" Width="300px" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table> </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
