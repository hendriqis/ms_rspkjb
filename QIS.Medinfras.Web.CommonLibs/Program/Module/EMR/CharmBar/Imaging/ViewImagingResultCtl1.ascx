<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewImagingResultCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPage.ViewImagingResultCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $(function () {
        //#region Service Unit
        function getTemplateTextExpression() {
            var filterExpression = "GCTemplateGroup = '<%=GCTemplateGroup %>' AND IsDeleted = 0";
            return filterExpression;
        }

        $('#<%=txtItemName.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtPhotoNumber.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReferenceNo.ClientID %>').attr('readonly', 'readonly');

        $('#containerEnglish').filter(':visible').hide();

        $('#ulTabLabResult li').click(function () {
            $('#ulTabLabResult li.selected').removeAttr('class');
            $('.containerOrder').filter(':visible').hide();
            $contentID = $(this).attr('contentid');
            $('#' + $contentID).show();
            $(this).addClass('selected');
        });
        //#endregion
    });
    //#region Preview
    $('.btnPreview').click(function () {
        $btnPreview = $(this);
        var hdnIsBridgingToRIS = $('#<%=hdnIsBridgingToRIS.ClientID %>').val();
        if (hdnIsBridgingToRIS == "1") 
        {
            if ($('#<%:hdnReferenceNo.ClientID %>').val() == "") {
                showToast("ERROR", 'Error Message : ' + "There is no result to be view !");
            }
            else {
                var referenceNo = $('#<%=txtReferenceNo.ClientID %>').val();

                if ($('#<%=hdnRISVendor.ClientID %>').val() == "X081^06") {
                    var viewerUrl = $('#<%=hdnViewerUrl.ClientID %>').val(); // + postData + "&Username=hisuser&Password=hisuser";

                    var mapForm = document.createElement("form");
                    mapForm.target = "_blank";
                    mapForm.method = "POST"; // or "post" if appropriate
                    mapForm.id = "form2";
                    mapForm.style.display = "none";
                    mapForm.action = viewerUrl;

                    var mapInput2 = document.createElement("input");
                    mapInput2.type = "text";
                    mapInput2.name = "Username";
                    mapInput2.value = "hisuser";
                    mapForm.appendChild(mapInput2);

                    var mapInput3 = document.createElement("input");
                    mapInput3.type = "text";
                    mapInput3.name = "Password";
                    mapInput3.value = "hisuser";
                    mapForm.appendChild(mapInput3);

                    var mapInput1 = document.createElement("input");
                    mapInput1.type = "text";
                    mapInput1.name = "AccessionNumber";
                    mapInput1.value = postData;
                    mapForm.appendChild(mapInput1);

                    document.body.appendChild(mapForm);

                    map = window.open(viewerUrl, '', 'menubar=no,toolbar=no,height=' + (window.screen.availHeight - 30) + ',scrollbars=no,status=no,width=' + (window.screen.availWidth - 10) + ',left=0,top=0,dependent=yes');

                    //                        map = window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                    if (map) {
                        document.getElementById("form2").submit();
                    } else {
                        alert('You must allow popups for this map to work.');
                    }
                }
                else {
                    var result = '<%=GetImagingResultImage() %>';
                    var resultInfo = result.split('|');
                    if (resultInfo[0] == "1") {
                        window.open(resultInfo[1], "popupWindow", "width=600, height=600,scrollbars=yes");
                    }
                    else {
                        showToast('Cannot open result preview', 'Error Message : ' + result[1]);
                    }
                }
            }
         } else {
            var result = $('#<%:hdnURLfile.ClientID %>').val();
            window.open(result, "popupWindow", "width=600, height=600,scrollbars=yes");
        }
    });
    //#endregion
</script>
<style type="text/css">
    .containerOrder
    {
        border: 1px solid #EAEAEA;
        padding: 0 5px;
    }
</style>
<input type="hidden" id="hdnItemID" value="" runat="server" />
<input type="hidden" id="hdnTransactionID" value="" runat="server" />
<input type="hidden" id="hdnTestOrderID" value="" runat="server" />
<input type="hidden" id="hdnID" value="" runat="server" />
<input type="hidden" id="hdnReferenceNo" value="" runat="server" />
<input type="hidden" id="hdnRISVendor" runat="server" value="" />
<input type="hidden" id="hdnViewerUrl" runat="server" value="" />
 <input type="hidden" value="0" id="hdnIsBridgingToRIS" runat="server" />
  <input type="hidden" value="" id="hdnURLfile" runat="server" />
<div style="height: 445px; overflow-y: auto;">
    <table class="tblContentArea">
        <colgroup>
            <col width="60%" />
            <col width="40%" />
        </colgroup>
        <tr valign="top">
            <td>
                <table style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 100px" />
                        <col style="width: 150px" />
                        <col style="width: 120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Rekam Medis")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMedicalNo" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Lahir")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateOfBirth" Width="100%" runat="server" ReadOnly="true" style="text-align:center" />
                        </td>
                        <td style="padding-left :10px;">
                            <input type="button" class="btnPreview" value="View Image" style="width:100px; height:22px;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pemeriksaan")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtItemName" Width="100%" runat="server" ReadOnly="true"  />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Accession No.")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReferenceNo" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Photo")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhotoNumber" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="display: none">
                <table width="100%">
                    <colgroup>
                        <col width="150px" />
                    </colgroup>
                    <tr>
                        <td>
                            <img src="" runat="server" id="imgPreview" width="150" height="150" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="containerUlTabPage" style="margin-bottom: 3px;">
                    <ul class="ulTabPage" id="ulTabLabResult">
                        <li class="selected" contentid="containerIndonesia">
                            <%=GetLabel("Indonesia") %></li>
                        <li contentid="containerEnglish">
                            <%=GetLabel("English")%></li>
                    </ul>
                </div>
                <div id="containerIndonesia" class="containerOrder">
                    <div id="contentIndonesia" runat="server">
                    </div>
                </div>
                <div id="containerEnglish" class="containerOrder">
                    <div id="contentEnglish" runat="server">
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
