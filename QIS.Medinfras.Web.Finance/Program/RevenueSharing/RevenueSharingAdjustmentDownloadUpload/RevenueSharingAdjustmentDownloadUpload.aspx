<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="RevenueSharingAdjustmentDownloadUpload.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingAdjustmentDownloadUpload" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnDownload" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdownload.png")%>' alt="" /><div>
            <%=GetLabel("Download")%></div>
    </li>
    <li id="btnUpload" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbupload.png")%>' alt="" /><div>
            <%=GetLabel("Upload")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
        }

        $('#<%=rblAdjustment.ClientID%>').live('change', function () {
            var adjustmentGroup = $('#<%=rblAdjustment.ClientID %> input:checked').val();
            var adjGroupPlus = "<%=OnGetAdjustmentGroupPlus() %>";

            if (adjustmentGroup == adjGroupPlus) {
                $('#trAdjustmentTypeAdd').show();
                $('#trAdjustmentTypeMin').hide();
            }
            else {
                $('#trAdjustmentTypeAdd').hide();
                $('#trAdjustmentTypeMin').show();
            }

        });

        //#region Download & Upload

        $('#<%=btnDownload.ClientID %>').live('click', function () {
            onCustomButtonClick('download');
        });

        function downloadRevenueSharingDocument(stringparam) {
            var fileName = $('#<%=hdnFileName.ClientID %>').val();

            var link = document.createElement("a");
            link.href = 'data:text/csv,' + encodeURIComponent(stringparam);
            link.download = fileName;
            link.click();
        }

        $('#<%=btnUpload.ClientID %>').die('change');
        $('#<%=btnUpload.ClientID %>').live('click', function () {

            var adjustmentGroup = $('#<%=rblAdjustment.ClientID %> input:checked').val();
            var adjGroupPlus = "<%=OnGetAdjustmentGroupPlus() %>";

            var adjValuePlus = cboAdjustmentTypeAdd.GetValue();
            var adjValueMin = cboAdjustmentTypeMin.GetValue();

            var isAllowUpload = "0";

            if (adjustmentGroup == adjGroupPlus) {
                if (adjValuePlus != null && adjValuePlus != "") {
                    isAllowUpload = "1";
                }
            }
            else {
                if (adjValueMin != null && adjValueMin != "") {
                    isAllowUpload = "1";
                }
            }

            if (isAllowUpload == "1") {
                document.getElementById('<%=RevenueSharingAdjustmentDocumentUpload.ClientID %>').click();
            } else {
                displayErrorMessageBox('Failed', 'Silahkan pilih jenis penyesuaian terlebih dahulu.');
            }

        });

        $('#<%=RevenueSharingAdjustmentDocumentUpload.ClientID %>').die('change');
        $('#<%=RevenueSharingAdjustmentDocumentUpload.ClientID %>').live('change', function () {
            readURL(this);
        });

        function readURL(input) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#<%=hdnRevenueSharingUploadedFile.ClientID %>').val(e.target.result);

                if ($('#<%=hdnRevenueSharingUploadedFile.ClientID %>').val() != "" && $('#<%=hdnRevenueSharingUploadedFile.ClientID %>').val() != null) {
                    onCustomButtonClick('upload');
                } else {
                    displayErrorMessageBox('Upload Failed', 'Silahkan coba lagi.');
                }
            };
            reader.readAsDataURL(input.files[0]);
        }
        //#endregion

        function onAfterCustomClickSuccess(type, retval) {
            var paramRetval = retval.split('|');
            var errMessage = "";
            if (paramRetval[0] == "failed") {
                errMessage = paramRetval[1];
            }

            if (type == "download") {
                if (retval != "Download Failed") {

                    //NB : jika ubah di sini jangan lupa ubah juga dari code behind saat UPLOAD ya...
                    var columnList = "ParamedicCode,"; //0
                    columnList += "Remarks,"; //1
                    columnList += "RevenueSharingCode,"; //2
                    columnList += "RegistrationNo,"; //3
                    columnList += "RegistrationDate[yyyyMMdd],"; //4
                    columnList += "DischargeDate[yyyyMMdd],"; //5
                    columnList += "ReceiptNo,"; //6
                    columnList += "InvoiceNo,"; //7
                    columnList += "ReferenceNo,"; //8
                    columnList += "BusinessPartnerName,"; //9
                    columnList += "MedicalNo[xx-xx-xx-xx],"; //10
                    columnList += "PatientName,"; //11
                    columnList += "TransactionNo,"; //12
                    columnList += "TransactionDate[yyyyMMdd],"; //13
                    columnList += "ItemName1,"; //14
                    columnList += "ChargedQty,"; //15
                    columnList += "AdjustmentAmountBRUTO,"; //16
                    columnList += "IsTaxed,"; //17
                    columnList += "AdjustmentAmount,"; //18
                    columnList += "GCRSAdjustmentGroup,"; //19
                    columnList += "GCRSAdjustmentType,"; //20

                    downloadRevenueSharingDocument(columnList);
                } else {
                    displayErrorMessageBox('Download Failed', 'Tidak ada data.');
                }
            } else if (type == "upload") {
                if (errMessage != "") {
                    displayErrorMessageBox('Process Failed', errMessage, function () { });
                } else {
                    displayMessageBox('Process Success', 'Proses Transaksi Penyesuaian Jasa Medis berhasil dibuat dengan nomor <b>' + paramRetval[1] + '</b>', function () { });
                }
            }
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnFileName" runat="server" value="" />
    <input type="hidden" id="hdnRevenueSharingUploadedFile" runat="server" value="" />
    <input type="hidden" id="hdnAutoApprovedRevenueSharingAdj" runat="server" value="" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td>
                    <h4>
                        <label class="lblNormal">
                            <%=GetLabel("Detail Upload") %></label>
                    </h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 300px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <label class="lblMandatory">
                                        <%=GetLabel("Kelompok Penyesuaian") %></label>
                                </td>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="rblAdjustment" RepeatDirection="Horizontal" />
                                </td>
                            </tr>
                            <tr id="trAdjustmentTypeAdd">
                                <td>
                                    <label class="lblMandatory" id="lblAdjustmentTypeAdd">
                                        <%=GetLabel("Jenis Penambahan") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ClientInstanceName="cboAdjustmentTypeAdd" ID="cboAdjustmentTypeAdd"
                                        Width="300px" />
                                </td>
                            </tr>
                            <tr style="display: none" id="trAdjustmentTypeMin">
                                <td>
                                    <label class="lblMandatory" id="lblAdjustmentTypeMin">
                                        <%=GetLabel("Jenis Pengurangan") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ClientInstanceName="cboAdjustmentTypeMin" ID="cboAdjustmentTypeMin"
                                        Width="300px" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td>
                                    <asp:FileUpload ID="RevenueSharingAdjustmentDocumentUpload" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
