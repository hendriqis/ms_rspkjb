<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MDTestResultDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MDTestResultDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_MDTestResultDetailCtl">
    $(function () {
        $('#containerEnglish').filter(':visible').hide();

        $('#ulTabLabResult li').live('click', function () {
            $('#ulTabLabResult li.selected').removeAttr('class');
            $('.containerOrder').filter(':visible').hide();
            $contentID = $(this).attr('contentid');
            $('#' + $contentID).show();
            $(this).addClass('selected');
        });

        function getTemplateTextExpression() {
            var filterExpression = "IsDeleted = 0";
            if ($('#<%=hdnIsImagingResultCtlResultDt.ClientID %>').val() == "1") {
                filterExpression += " AND GCTemplateGroup IN ('<%=GCTemplateGroupIS %>')";
            } else {
                filterExpression += " AND GCTemplateGroup IN ('<%=GCTemplateGroupMD %>')";
            }
            filterExpression += " AND TemplateID IN (SELECT a.TemplateID FROM PhysicianTemplateText a WHERE a.ParamedicID = " + $('#<%=hdnParamedicIDCtlResultDt.ClientID %>').val() + ")";
            return filterExpression;
        }

        $('#lblTemplateResult.lblLink').live('click', function () {
            openSearchDialog('templatetext', getTemplateTextExpression(), function (value) {
                var filterExpression = getTemplateTextExpression() + " AND TemplateCode = '" + value + "'";
                Methods.getObjectValue('GetTemplateTextList', filterExpression, "TemplateContent", function (result) {
                    if ($('#containerEnglish').is(':visible'))
                        tinyMCE.get('<%=txtTestResult2.ClientID %>').execCommand('mceSetContent', false, result);
                    else
                        tinyMCE.get('<%=txtTestResult1.ClientID %>').execCommand('mceSetContent', false, result);
                    //tinyMCE.triggerSave();
                });
            });
        });

        setHtmlEditor();

    });

    $('#<%=FileUpload1.ClientID %>').live('change', function () {
        readURL(this);
        var fileName = $('#<%=FileUpload1.ClientID %>').val().replace("C:\\fakepath\\", "");
        var name = fileName.substring(0, fileName.lastIndexOf('.'));
        var extension = fileName.substring(fileName.lastIndexOf('.') + 1);

        $('#txtUploadFile').val(fileName);

        $('#<%=txtUploadFile.ClientID %>').val(fileName);
        $('#<%=txtExtention.ClientID %>').val(extension);

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
        else
            $('#<%=imgPreview.ClientID %>').attr('src', input.value);
    }

    $('#btnUploadFile').live('click', function () {
        document.getElementById('<%= FileUpload1.ClientID %>').click();
    });

</script>
<input type="hidden" id="hdnIsImagingResultCtlResultDt" value="" runat="server" />
<input type="hidden" id="hdnMedicalNo" value="" runat="server" />
<input type="hidden" id="hdnVisitID" value="" runat="server" />
<input type="hidden" id="hdnItemIDCtlResultDt" value="" runat="server" />
<input type="hidden" id="hdnParamedicIDCtlResultDt" value="" runat="server" />
<input type="hidden" id="hdnIDCtlResultDt" value="" runat="server" />
<input type="hidden" id="hdnIsVerifiedCtlResultDt" value="" runat="server" />
<div style="height: 445px; overflow-y: auto;">
    <table class="tblContentArea">
        <colgroup>
            <col width="60%" />
            <col width="40%" />
        </colgroup>
        <tr valign="top">
            <td>
                <table width="100%">
                    <colgroup>
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pemeriksaan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemInfo" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nomor Photo")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhotoNumber" Width="150px" runat="server" />
                        </td>
                    </tr>
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
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Result Border Line")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboBorderLine" ClientInstanceName="cboBorderLine" Width="100%"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td class="tdLabel">
                            <label class="lblNormal lblLink" id="lblTemplateResult">
                                <%=GetLabel("Template Hasil")%></label>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table width="100%">
                    <colgroup>
                        <col width="150px" />
                    </colgroup>
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
                                    <col style="width: 180px" />
                                    <col style="width: 30px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <input type="text" id="txtUploadFile" style="width: 100%" runat="server" readonly="readonly" />
                                    </td>
                                    <td>
                                        <input type="button" id="btnUploadFile" value="Upload" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type="text" id="txtExtention" style="width: 77px" runat="server" readonly="readonly" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("extension") %>
                            : jpg,jpeg,png,txt,doc,docx,pdf,dcm,mp4.
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
                    <asp:TextBox TextMode="MultiLine" Width="100%" Height="300px" ID="txtTestResult1"
                        runat="server" CssClass="htmlEditor" />
                </div>
                <div id="containerEnglish" class="containerOrder">
                    <asp:TextBox TextMode="MultiLine" Width="100%" Height="300px" ID="txtTestResult2"
                        runat="server" CssClass="htmlEditor" />
                </div>
            </td>
        </tr>
    </table>
</div>
