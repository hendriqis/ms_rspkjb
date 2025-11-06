<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneralCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GeneralCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <script src='<%= ResolveUrl("~/Libs/Scripts/socketio/socket.io.js")%>'
    type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery-1.4.3.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery-1.7.min.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery.validate.min.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery.paginate.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery.colorPicker.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>' type="text/javascript"></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>' type="text/javascript"></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.button.js")%>' type="text/javascript"></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.position.js")%>'
    type="text/javascript"></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.autocomplete.js")%>'
    type="text/javascript"></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.datepicker.js")%>'
    type="text/javascript"></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/TinyMce/tiny_mce.js")%>' type="text/javascript"></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/Constant.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/Methods.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/BPJSService.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/IHSService.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/InhealthService.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/AplicaresService.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/EKlaimService.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/KemenKesService.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/EDCService.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/DrugAlertService.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/dropdown/superfish.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery.tmpl.min.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery.maphilight.js")%>' type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/CustomControl/QISClientIntellisenseTextBox.js")%>'
    type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/CustomControl/QISClientSearchTextBox.js")%>'
    type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/CustomControl/QISClientQuickEntry.js")%>'
    type='text/javascript'></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/MedinfrasAPI.js")%>' type='text/javascript'></script>																								 
   
<%--<script src='<%= ResolveUrl("~/Libs/Scripts/signalR/jquery.signalR-1.0.0-rc1.js") %>' type="text/javascript"></script>
<script src="<%= ResolveClientUrl("~/signalr/hubs") %>" type="text/javascript"></script>
<script src='<%= ResolveUrl("~/Libs/Scripts/signalR/Function_signalR.js") %>' type="text/javascript"></script>--%>
<script type="text/javascript">
    $('body').keydown(function (event) {
        if (event.which == 8 && !$('textarea:focus').length && !$('input:focus').length) {
            event.preventDefault();
        }
    });
    var baseUrl = '<%= ResolveUrl("~/") %>';
    window.ResolveUrl = function (url) {
        if (url.indexOf("~/") == 0) {
            url = baseUrl + url.substring(2);
        }
        return url;
    }


    $("input:text.number, input:text.txtCurrency").live('focus', function () { $(this).select(); });

    if ($.browser.mozilla) {
        $(document).keypress(bodyKeyPressGeneral);
    } else {
        $(document).keydown(bodyKeyPressGeneral);
    }
    function bodyKeyPressGeneral(e) {
        if (e.target.tagName.toUpperCase() == 'INPUT' || e.target.tagName.toUpperCase() == 'TEXTAREA') return;
        var charCode = (e.which) ? e.which : e.keyCode;
        switch (charCode) {
            case 39: e.preventDefault(); break; //right
        }
    }
    //#region Get Status Expired License
    $(document).ready(function () {
        getStatusExpiredLicense();
    });

    function getStatusExpiredLicense() {
        var isResult = "";
        Methods.getExpiredLicense(function (result) {
            if (result != null) {
                isResult = result.split('|');
                if (isResult[0] == "1") {
                    $('#DateExpiredLicense').append("<b>" + isResult[1] + "</b>");
                    $('#ExpiredLicense').show();
                } else {
                    $('#ExpiredLicense').hide();

                }
            }
        });

    }
    //#endregion
    //#region Get Status PerRegOutstanding
    function getStatusPerRegOutstanding() {
        var iSOutStanding = false;
        Methods.getStatusPerRegOutstanding(function (result) {
            if (result != null) {
                iSOutStanding = result;
            }
        });

        if (iSOutStanding) {
            $(".CloseRegistration").hide();
        } else {
            $(".CloseRegistration").show();
        }

        return iSOutStanding;
    }
    //#endregion
    //#region Collapse Expand
    function registerCollapseExpandHandler() {
        registerCollapseHandler();
        registerExpandHandler();
    }

    function registerCollapseHandler() {
        $('h4.h4collapsed').click(function () {
            $elm = $(this);
            $(this).next('div.containerTblEntryContent').slideDown('fast', function () {
                $elm.removeClass('h4collapsed');
                $elm.addClass('h4expanded');
                $elm.unbind('click');
                registerExpandHandler();
            });
        });
    }

    function registerExpandHandler() {
        $('h4.h4expanded').click(function () {
            $elm = $(this);
            $(this).next('div.containerTblEntryContent').slideUp('fast', function () {
                $elm.removeClass('h4expanded');
                $elm.addClass('h4collapsed');
                $elm.unbind('click');
                registerCollapseHandler();
            });
        });
    }
    //#endregion
	//#region Report template world Viewer
    function openReportTemplateViewer(reportCode, paramValue) {

        MedinfrasAPI.getTemplateDocumentPrint(reportCode, paramValue, function (result) {
            try {
                var obj = result.split('|');
                if (obj[0] == "1") {
                    var file = obj[1];
                    var pdfDataUrl = 'data:application/pdf;base64,' + file;
                    var win = window.open(pdfDataUrl, reportCode, 'status=1,toolbar=0,menubar=0,resizable=1,location=0,scrollbars=1,width=1150,height=600');
                    win.focus();
                }
                else {
                    showToast('Report Gagal', 'Error Message : ' + obj[2]);
                }

            } catch (err) {
                alert(err);
            }
        });
    }
    //#endregion				
    //#region Report Viewer
    function openReportViewer(reportCode, param) {
        var filterExpression = "ReportCode = '" + reportCode + "'";
        var isUsingPDClient = "0";
        Methods.getObject('GetSettingParameterDtList', "ParameterCode = 'OP0014'", function (result) {
            if (result != null) {
                if (result.ParameterValue == "1") {
                    isUsingPDClient = "1";
                }
            }
        });

        Methods.getObject('GetReportMasterList', filterExpression, function (result) {
            if (result != null) {
                if (result.IsDirectPrint) {
                    $('#<%=hdnReportCode.ClientID %>').val(reportCode);
                    if (isUsingPDClient == "0") {
                        cbpDirectPrintProcess.PerformCallback(param);
                    }
                    else {
                        cbpDirectPrintProcessDirect.PerformCallback(param);
                    }
                }
                else {
                    if (result.GCReportMasterSource == "X641^003") {
                        openReportTemplateViewer(reportCode, param);
                    } else {
                        var isShowError = false;
                        window.setTimeout(function () {
                            showLoadingPanel();
                            var e = new Image(1, 1);
                            e.onerror = function () {
                                if (!isShowError) {
                                    isShowError = true;

                                    var win = window.open("", reportCode.replace('-', ''), 'status=1,toolbar=0,menubar=0,resizable=1,location=0,scrollbars=1,width=1150,height=600');
                                    win.focus();

                                    var mapForm = document.createElement("form");
                                    mapForm.target = reportCode.replace('-', '');
                                    mapForm.method = "POST";
                                    mapForm.action = ResolveUrl('~/Libs/Program/ReportViewer.aspx?id=' + reportCode);

                                    var mapInput = document.createElement("input");
                                    mapInput.type = "hidden";
                                    mapInput.name = "param";
                                    mapInput.value = param;
                                    mapForm.appendChild(mapInput);

                                    document.body.appendChild(mapForm);

                                    mapForm.submit();

                                    $(mapForm).remove();
                                }
                            };
                            var src = "http://localhost:60025/buffertext/dummy.gif?healthcareid=<%=HealthcareID %>&userid=<%=UserID %>&username=<%=UserName %>&userfullname=<%=UserFullName %>&type=report&reportid=" + reportCode + "&param=" + param + "&timestamp=" + new Date().getTime();
                            e.src = src;
                            window.setTimeout(function () {
                                hideLoadingPanel();
                            }, 5000);
                            void 0;
                        }, 0);
                    }
                }
            }
            else
                showToast("Warning", "Report Dengan Kode <b>" + reportCode + "</b> Tidak Ditemukan");
        });
    }

    function openReportViewerExport(reportCode, param) {
        var filterExpression = "ReportCode = '" + reportCode + "'";
        var isUsingPDClient = "0";
        Methods.getObject('GetSettingParameterDtList', "ParameterCode = 'OP0014'", function (result) {
            if (result != null) {
                if (result.ParameterValue == "1") {
                    isUsingPDClient = "1";
                }
            }
        });

        Methods.getObject('GetReportMasterList', filterExpression, function (result) {
            if (result != null) {
                if (result.IsDirectPrint) {
                    $('#<%=hdnReportCode.ClientID %>').val(reportCode);
                    if (isUsingPDClient == "0") {
                        cbpDirectPrintProcess.PerformCallback(param);
                    }
                    else {
                        cbpDirectPrintProcessDirect.PerformCallback(param);
                    }
                }
                else {
                    var isShowError = false;
                    window.setTimeout(function () {
                        showLoadingPanel();
                        var e = new Image(1, 1);
                        e.onerror = function () {
                            if (!isShowError) {
                                isShowError = true;

                                var win = window.open("", reportCode.replace('-', ''), 'status=1,toolbar=0,menubar=0,resizable=1,location=0,scrollbars=1,width=1150,height=600');
                                win.focus();

                                var mapForm = document.createElement("form");
                                mapForm.target = reportCode.replace('-', '');
                                mapForm.method = "POST";
                                mapForm.action = ResolveUrl('~/Libs/Program/ReportViewerExport.aspx?id=' + reportCode);

                                var mapInput = document.createElement("input");
                                mapInput.type = "hidden";
                                mapInput.name = "param";
                                mapInput.value = param;
                                mapForm.appendChild(mapInput);

                                document.body.appendChild(mapForm);

                                mapForm.submit();

                                $(mapForm).remove();
                            }
                        };
                        var src = "http://localhost:60025/buffertext/dummy.gif?healthcareid=<%=HealthcareID %>&userid=<%=UserID %>&username=<%=UserName %>&userfullname=<%=UserFullName %>&type=report&reportid=" + reportCode + "&param=" + param + "&timestamp=" + new Date().getTime();
                        e.src = src;
                        window.setTimeout(function () {
                            hideLoadingPanel();
                        }, 5000);
                        void 0;
                    }, 0);
                }
            }
            else
                showToast("Warning", "Report Dengan Kode <b>" + reportCode + "</b> Tidak Ditemukan");
        });
    }
    //#endregion

    //#region Export Report
    function exportReportToExcel(reportCode, param) {
        var filterExpression = "ReportCode = '" + reportCode + "'";
        Methods.getObject('GetReportMasterList', filterExpression, function (result) {
            if (result != null) {
                if (!result.IsDirectPrint) {
                    $('#<%=hdnReportCode.ClientID %>').val(reportCode);
                    $('#<%=hdnReportParam.ClientID %>').val(param);
                    __doPostBack('<%=btnExport.UniqueID%>', '');
                }
                else {
                    showToast("Warning", "Report Dengan Kode <b>" + reportCode + "</b> Menggunakan Direct Print, Tidak bisa di Export.");
                }
            }
            else {
                showToast("Warning", "Report Dengan Kode <b>" + reportCode + "</b> Tidak Ditemukan");
            }
        });
    }
    //#endregion

    //#region Export Report RAW
    function exportReportToExcelRAW(reportCode, param) {
        var filterExpression = "ReportCode = '" + reportCode + "'";
        Methods.getObject('GetReportMasterList', filterExpression, function (result) {
            if (result != null) {
                if (!result.IsDirectPrint) {
                    $('#<%=hdnReportCode.ClientID %>').val(reportCode);
                    $('#<%=hdnReportParam.ClientID %>').val(param);                    
                    __doPostBack('<%=btnRAWExport.UniqueID%>', '');
                }
                else {
                    showToast("Warning", "Report Dengan Kode <b>" + reportCode + "</b> Menggunakan Direct Print, Tidak bisa di Export.");
                }
            }
            else {
                showToast("Warning", "Report Dengan Kode <b>" + reportCode + "</b> Tidak Ditemukan");
            }
        });
    }
    //#endregion

    //#region HTML Editor
    tinyMCE.init({
        mode: "textareas",
        theme: "advanced",
        editor_selector: "htmlEditor",
        encoding: "xml",
        plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,visualblocks",

        theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
        theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        theme_advanced_statusbar_location: "bottom",
        theme_advanced_resizing: false,

        content_css: ResolveUrl("~/Libs/Styles/TinyMce/content.css"),

        content_css: ResolveUrl("~/Libs/Scripts/TinyMce/lists/template_list.js"),
        content_css: ResolveUrl("~/Libs/Scripts/TinyMce/lists/link_list.js"),
        content_css: ResolveUrl("~/Libs/Scripts/TinyMce/lists/image_list.js"),
        content_css: ResolveUrl("~/Libs/Scripts/TinyMce/lists/media_list.js"),

        style_formats: [
            { title: 'Bold text', inline: 'b' },
            { title: 'Red text', inline: 'span', styles: { color: '#ff0000'} },
            { title: 'Red header', block: 'h1', styles: { color: '#ff0000'} },
            { title: 'Example 1', inline: 'span', classes: 'example1' },
            { title: 'Example 2', inline: 'span', classes: 'example2' },
            { title: 'Table styles' },
            { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
        ],

        template_replace_values: {
            username: "Some User",
            staffid: "991234"
        },

        forced_root_block: "",
        force_br_newlines: true,
        force_p_newlines: false,

        onchange_callback: function (ed) {
            ed.save();
        }
    });

    function setHtmlEditor() {
        //#region HTML Editor
        tinyMCE.init({
            mode: "textareas",
            theme: "advanced",
            editor_selector: "htmlEditor",
            encoding: "xml",
            plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,visualblocks",

            theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
            theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
            theme_advanced_toolbar_location: "top",
            theme_advanced_toolbar_align: "left",
            theme_advanced_statusbar_location: "bottom",
            theme_advanced_resizing: false,

            content_css: ResolveUrl("~/Libs/Styles/TinyMce/content.css"),

            content_css: ResolveUrl("~/Libs/Scripts/TinyMce/lists/template_list.js"),
            content_css: ResolveUrl("~/Libs/Scripts/TinyMce/lists/link_list.js"),
            content_css: ResolveUrl("~/Libs/Scripts/TinyMce/lists/image_list.js"),
            content_css: ResolveUrl("~/Libs/Scripts/TinyMce/lists/media_list.js"),

            style_formats: [
            { title: 'Bold text', inline: 'b' },
            { title: 'Red text', inline: 'span', styles: { color: '#ff0000'} },
            { title: 'Red header', block: 'h1', styles: { color: '#ff0000'} },
            { title: 'Example 1', inline: 'span', classes: 'example1' },
            { title: 'Example 2', inline: 'span', classes: 'example2' },
            { title: 'Table styles' },
            { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
        ],

            template_replace_values: {
                username: "Some User",
                staffid: "991234"
            },

            forced_root_block: "",
            force_br_newlines: true,
            force_p_newlines: false,

            onchange_callback: function (ed) {
                ed.save();
            }
        });
        //#endregion
    }
    //#endregion

    //#region Currency
    Number.prototype.formatMoney = function (c, d, t) {
        var n = this,
    c = isNaN(c = Math.abs(c)) ? 2 : c,
    d = d == undefined ? "." : d,
    t = t == undefined ? "," : t,
    s = n < 0 ? "-" : "",
    i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
    j = (j = i.length) > 3 ? j % 3 : 0;
        return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
    };

    $('.txtCurrency').live('blur', function () {
        if ($(this).attr('readonly') != 'readonly') {
            $(this).trigger('changeValue');
        }
    });

    $('.txtCurrency').live('focus', function () {
        if ($(this).attr('readonly') != 'readonly') {
            $value = $(this).attr('hiddenVal');
            if ($value != null)
                $(this).val($value);
        }
    });
    $('.txtCurrency').die('changeValue');
    $('.txtCurrency').live('changeValue', function () {
        $hiddenVal = $(this).val();
        if ($hiddenVal == '')
            $hiddenVal = '0';
        if ($hiddenVal.indexOf(',') > -1 && $hiddenVal.indexOf('.') > -1) {
        }
        else {
            //$hiddenVal = Math.floor(parseFloat($hiddenVal) * 100 / 100);
            $hiddenVal = parseFloat($hiddenVal).toFixed(2);
            $val = parseFloat($hiddenVal).formatMoney(2, '.', ',');
            $(this).val($val);
            //$(this).val(parseFloat($hiddenVal).toFixed(2));
            $(this).attr('hiddenVal', $hiddenVal);
        }
    })

    $(function () {
        $('.txtCurrency').each(function () {
            $(this).val($(this).val().replace('.00', ''));
            $(this).trigger('changeValue');
        });
    });
    //#endregion

    //#region Context Menu
    function showContextMenu($ctxMenu, e) {
        var top = 0;
        var left = 0;
        if (e.pageY < $(window).height() / 2) {
            top = e.pageY;
            left = e.pageX;
        }
        else {
            top = e.pageY - $ctxMenu.height();
            left = e.pageX;
        }

        if (left + $ctxMenu.width() > $(window).width())
            left = left - $ctxMenu.width();
        $ctxMenu.css({ top: top + "px", left: left + "px" }).show(100);
    }
    //#endregion

    //#region Search Dialog
    var grdSearchID = '<%=grdSearch.ClientID %>';
    var onClickRowSearchDialogHandler = null;
    var searchDialogSearchType = '';
    var searchDialogFilterExpression = '';
    var isInitSearchDialog = false;
    var lastSearchText = '';
    var lastIndex = '';
    $(function () {
        pcSearchDialog.SetHeaderText('<%= GetLabel("Pencarian Data")%>');
        $imgClose = '<%= ResolveUrl("~/Libs/Images/close_icon.gif")%>';
        $td = $('.dxWeb_pcCloseButton').parent();
        $('.dxWeb_pcCloseButton').remove();
        $td.append($("<img src='" + $imgClose + "' height='14'/>"));

        $('#txtSearchResult').keydown(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13)
                highlightText();
        });

        $('#' + grdSearchID + ' tr:gt(0)').live('click', function () {
            if ($('#' + grdSearchID + ' td').length > 1) {
                //showLoadingPanel();
                onClickRowSearchDialogHandler($(this).find('td:eq(0)').html());
                pcSearchDialog.Hide();
                //hideLoadingPanel();
            }
        });

        /*$('#' + grdSearchID + ' th').live('click', function () {
        if ($('#' + grdSearchID + ' td').length > 1) {
        var sortedClassName = 'ASC';
        if ($(this).attr('class') != null && $(this).attr('class').indexOf('ASC') > -1)
        sortedClassName = 'DESC';
        $('#' + grdSearchID + ' th').each(function () {
        $(this).removeClass('ASC');
        $(this).removeClass('DESC');
        });
        $(this).addClass(sortedClassName);
        var idx = parseInt($(this).index()) - 1;
        cbpSearchDialog.PerformCallback('sort|' + idx + '|' + sortedClassName);
        }
        });*/

        function isTextSelected(input) {
            var startPos = input.selectionStart;
            var endPos = input.selectionEnd;
            var doc = document.selection;

            if (doc && doc.createRange().text.length != 0) {
                return true;
            } else if (!doc && input.value.substring(startPos, endPos).length != 0) {
                return true;
            }
            return false;

            /*if (typeof input.selectionStart == "number") {
            return input.selectionStart == 0 && input.selectionEnd == input.value.length;
            } else if (typeof document.selection != "undefined") {
            input.focus();
            return document.selection.createRange().text == input.value;
            }*/
        }

        $('.NoRM').live('keydown', function (e) {
            if (isTextSelected($(this)[0])) {
            }
            else {
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code > 47 && code < 59 || code == 173 || code == 189) {
                    var val = $(this).val();
                    if (val.length == 11) {
                        e.preventDefault();
                    }
                    else {
                        if (val.length == 1 || val.length == 4 || val.length == 7) {
                            var c = String.fromCharCode(e.which);
                            val += c + '-';
                            e.preventDefault();
                        }
                        $(this).val(val);
                    }
                }
                else if (e.ctrlKey || code == 9 || code == 8 || code == 34 || code == 35 || code == 36 || code == 37 || code == 38 || code == 39 || code == 40) {
                }
                else
                    e.preventDefault();
            }
        });
    });

    function openSearchDialog(searchType, filterExpression, functionHandler) {
        searchDialogSearchType = searchType;
        searchDialogFilterExpression = filterExpression;
        onClickRowSearchDialogHandler = functionHandler;
        pcSearchDialog.Show();
    }

    function pad(str, max) {
        str = str.toString();
        return str.length < max ? pad("0" + str, max) : str;
    }

    function FormatMRN(value) {
        var mrn = value;

        if (value.indexOf("-") >= 0) {
            mrn = value;
        }
        else {


            if (value.length < 8) {
                mrn = pad(value, 8);
            }

            var part1 = mrn.substr(0, 2);
            var part2 = mrn.substr(2, 2);
            var part3 = mrn.substr(4, 2);
            var part4 = mrn.substr(6, 2);

            mrn = part1 + '-' + part2 + '-' + part3 + '-' + part4;
        }
        return mrn;
    }

    function FormatRegistrationNo(value) {
        var regNo = value;

        if (value.indexOf("/") >= 0) {
            regNo = value;
        }
        else {
            if (value.length < 16) {
                regNo = pad(value, 16);
            }

            var part1 = regNo.substr(0, 3);
            var part2 = regNo.substr(3, 8);
            var part3 = regNo.substr(11, 5);

            regNo = part1 + '/' + part2 + '/' + part3;
        }
        return regNo;
    }

    function highlightText() {
        var text = $('#txtSearchResult').val();
        var inputText = document.getElementById("containerSearchResult");
        var innerHTML = inputText.innerHTML.replace(/\"/g, '').replace(/<span class=highlightText>([_A-Z0-9a-z-+\.]+)<\/span>/g, '$1').replace(/<SPAN class=highlightText>([_A-Z0-9a-z-+\.]+)<\/SPAN>/g, '$1'); ;

        var index = 0;
        if (lastSearchText != text) {
            index = innerHTML.toUpperCase().indexOf(text.toUpperCase());
        }
        else {
            index = innerHTML.toUpperCase().substr(lastIndex + 1).indexOf(text.toUpperCase());
            index = lastIndex + index + 1;
        }
        if (index >= 0 && lastIndex != index) {
            innerHTML = innerHTML.substring(0, index) + "<span class='highlightText'>" + innerHTML.substring(index, index + text.length) + "</span>" + innerHTML.substring(index + text.length);
            inputText.innerHTML = innerHTML;
            lastIndex = index;
            lastSearchText = text;
        }
        else {
            inputText.innerHTML = innerHTML;
            showToast('Warning', 'No further occurence of "' + text + '" were found');
            lastIndex = 0;
            lastSearchText = '';
        }
    }

    function onCbpSearchDialogEndCallback(s) {
        if (isInitSearchDialog) {
            var intellisenseHints = $.parseJSON('[' + s.cpIntellisenseHints + ']');
            txtQuickSearchDialogHelper.setIntellisenseHints(intellisenseHints);
            txtQuickSearchDialog.setIntellisenseHints(intellisenseHints);
            //txtQuickSearchDialog.SetFocus();
            isInitSearchDialog = false;
        }
        //$th = $('#' + grdSearchID + ' th:eq(' + (parseInt(s.cpSortedIndex) + 1) + ')');
        //$th.addClass(s.cpSortedType);

        $('#containerImgLoadingSearchDialog').hide();
    }
    //#endregion

    //#region Loading Panel
    function showLoadingPanel() {
        $('#loadingPanel').show();
    }

    function hideLoadingPanel() {
        $('#loadingPanel').hide();
    }

    function isLoadingPanelVisible() {
        return $('#loadingPanel').is(":visible");
    }
    //#endregion

    //#region Toast Panel
    var functionHandlerCloseToast = null;
    function showToast(title, message, fn) {
        functionHandlerCloseToast = fn;
        $('#toastPanel .messageHeader').html(title);
        $('#toastPanel .messageText').html(message);
        $('#toastPanel').show();
    }

    function showToastError(title, message, fn) {
        functionHandlerCloseToast = fn;
        $('#toastPanel .messageHeader').html(title);
        $('#toastPanel .messageText').html(message);
        $('#toastPanel').show();
    }

    function hideToast() {
        $('#toastPanel').hide();
        if (functionHandlerCloseToast != null)
            functionHandlerCloseToast();
    }

    var functionHandlerCloseToast = null;
    function showToastConfirmation(message, fn) {
        functionHandlerCloseToast = fn;
        $('#toastConfirmationPanel .messageText').html(message);
        $('#toastConfirmationPanel').show();
    }

    function hideToastConfirmation(result) {
        $('#toastConfirmationPanel').hide();
        if (functionHandlerCloseToast != null)
            functionHandlerCloseToast(result);
    }
    //#endregion

    //#region Delete Confirmation
    var functionHandlerDeleteConfirmation = null;
    function showDeleteConfirmation(fn) {
        functionHandlerDeleteConfirmation = fn;
        var url = ResolveUrl('~/Libs/Controls/DeleteConfirmationCtl.ascx');
        openUserControlPopup(url, '', 'Delete Reason', 400, 230);
    }

    function onDeleteDetailUsingReason(data) {
        pcRightPanelContent.Hide();
        functionHandlerDeleteConfirmation(data);
    }
    //#endregion

    //#region MessageBox
    function displayMessageBox(title, message, fn) {
        functionHandlerCloseToast = fn;
        $('#messageBox .messageTitle').html(title);
        $('#messageBox .messageBody').html(message);
        $('#messageBox').show();
    }

    function displayErrorMessageBox(title, message, fn) {
        functionHandlerCloseToast = fn;
        $('#messageBoxError .messageTitle').html('ERROR : ' + title);
        $('#messageBoxError .messageBody').html(message);
        $('#messageBoxError').show();
    }

    function displayConfirmationMessageBox(title, message, fn) {
        functionHandlerCloseToast = fn;
        $('#messageBoxConfirmation .messageTitle').html(title);
        $('#messageBoxConfirmation .messageBody').html(message);
        $('#messageBoxConfirmation').show();
    }

    function hideConfirmationMessageBox(result) {
        $('#messageBoxConfirmation').hide();
        if (functionHandlerCloseToast != null)
            functionHandlerCloseToast(result);
    }
    //#endregion

    //#region Right Panel & Popup Content
    $(function () {
        var isRightContentExists = false;
        if ($('#divListRightPanel .containerRightPanelContent.tasks').children().length > 0) {
            isRightContentExists = true;
            $('.divOpenRightPanelContent[contentid=tasks]').show();
        }
        if ($('#divListRightPanel .containerRightPanelContent.information').children().length > 0) {
            isRightContentExists = true;
            $('.divOpenRightPanelContent[contentid=information]').show();
        }
        if ($('#divListRightPanel .rightPanelPrintContent').children().length > 0) {
            isRightContentExists = true;
            $('.divOpenRightPanelContent[contentid=print]').show();
        }

        $('#imgRightPanelPrint').click(function () {
            $rbo = $('input[name=rboPrint]:checked');
            if ($rbo.length > 0) {
                var filterExpression = { text: "" };
                var errMessage = { text: "" };
                var reportCode = $rbo.attr('reportcode');
                if (reportCode != '') {
                    var isAllowPrint = true;
                    if (typeof onBeforeRightPanelPrint == 'function') {
                        isAllowPrint = onBeforeRightPanelPrint(reportCode, filterExpression, errMessage);
                    }
                    if (isAllowPrint)
                        openReportViewer(reportCode, filterExpression.text);
                    else
                        displayMessageBox("Warning", errMessage.text);
                }
            }
        });

        $('#imgRightPanelExport').click(function () {
            $rbo = $('input[name=rboPrint]:checked');
            if ($rbo.length > 0) {
                var filterExpression = { text: "" };
                var errMessage = { text: "" };
                var reportCode = $rbo.attr('reportcode');
                if (reportCode != '') {
                    var isAllowPrint = true;
                    if (typeof onBeforeRightPanelPrint == 'function') {
                        isAllowPrint = onBeforeRightPanelPrint(reportCode, filterExpression, errMessage);
                    }
                    if (isAllowPrint)
                        exportReportToExcel(reportCode, filterExpression.text)
                    else
                        displayMessageBox("Warning", errMessage.text);
                }
            }
        });

        $('#imgRightPanelRawExport').click(function () {
            $rbo = $('input[name=rboPrint]:checked');
            if ($rbo.length > 0) {
                var filterExpression = { text: "" };
                var errMessage = { text: "" };
                var reportCode = $rbo.attr('reportcode');
                if (reportCode != '') {
                    var isAllowPrint = true;
                    if (typeof onBeforeRightPanelPrint == 'function') {
                        isAllowPrint = onBeforeRightPanelPrint(reportCode, filterExpression, errMessage);
                    }
                    if (isAllowPrint)
                        exportReportToExcelRAW(reportCode, filterExpression.text)
                    else
                        displayMessageBox("Warning", errMessage.text);
                }
            }
        });

        $('#imgRightPanelDownloadImageToExcel').click(function () {
            $rbo = $('input[name=rboPrint]:checked');
            if ($rbo.length > 0) {
                var filterExpression = { text: "" };
                var errMessage = { text: "" };
                var reportCode = $rbo.attr('reportcode');
                if (reportCode != '') {
                    var isAllowPrint = true;
                    if (typeof onBeforeRightPanelPrint == 'function') {
                        isAllowPrint = onBeforeRightPanelPrint(reportCode, filterExpression, errMessage);
                    }
                    if (isAllowPrint) {
                        openReportViewerExport(reportCode, filterExpression.text);
                    }
                    else {
                        displayMessageBox("Warning", errMessage.text);
                    }
                }
            }
        });

        if (isRightContentExists) {
            $('#containerRightPanel').show();
            var width = $('#divListRightPanel').width() + 12;
            $('#containerRightPanel').css('right', -width + "px");
            $('#containerRightPanel').attr('hideRight', -width + "px");
            var isOpenQuickMenu = false;
            $('.divRightPanelBackground').click(function () {
                if (!isOpenQuickMenu) {
                    isOpenQuickMenu = true;
                    $('#tdOpenRightPanel').addClass('open');
                    $('#containerRightPanel').animate({ "right": "0px" }, 200);
                }

                $('#divListRightPanel .containerRightPanelContent').hide();

                $('.divRightPanelBackground.selected').removeClass('selected');
                $('.divRightPanelBackgroundTop.selected').removeClass('selected');
                $('.divRightPanelBackgroundBottom.selected').removeClass('selected');

                $td = $(this).parent();
                $td.children().each(function () {
                    $(this).addClass('selected');
                });

                $('#headerRightPanelTitle').html($(this).find('.textRightPanel').html());
                $('.containerRightPanelContent.' + $td.parent().attr('contentid')).show();
            });
            $('#imgCloseRightPanel').click(function () {
                if (isOpenQuickMenu) {
                    isOpenQuickMenu = false;
                    $('#tdOpenRightPanel').removeClass('open');
                    $('#containerRightPanel').animate({ "right": $('#containerRightPanel').attr('hideRight') }, 200);
                    $('.divRightPanelBackground.selected').removeClass('selected');
                    $('.divRightPanelBackgroundTop.selected').removeClass('selected');
                    $('.divRightPanelBackgroundBottom.selected').removeClass('selected');
                }
            });
        }

        $('.goRightPanelContent').click(function () {
            if ($(this).attr('enabled') == null) {
                pcRightPanelContent.Hide();
                var url = $(this).attr('url');
                var data = { url: "", width: "", height: "", title: "" };
                var title = $(this).parent().find('.qmtitle').html();
                var width = $(this).attr('pcWidth');
                var height = $(this).attr('pcHeight');
                $('#hdnRightPanelContentIsLoadContent').val('1');
                $('#hdnRightPanelContentFirstTimeLoad').val('1');
                var rightPanelContentParam = '';
                var isAllowToContinue = true;
                var errMessage = { text: "" };
                if (typeof onValidateBeforeLoadRightPanelContent == 'function') {
                    isAllowToContinue = onValidateBeforeLoadRightPanelContent($(this).attr('code'), errMessage);
                }
                if (isAllowToContinue) {
                    if (typeof onBeforeLoadRightPanelContent == 'function') {
                        rightPanelContentParam = onBeforeLoadRightPanelContent($(this).attr('code'));
                    }
                    if (typeof onOverrideLoadRightPanelUrl == 'function') {
                        onOverrideLoadRightPanelUrl($(this).attr('code'), data);
                    }
                    if (data.url != '') {
                        $('#hdnRightPanelContentUrl').val(data.url);
                        title = data.title;
                        width = data.width;
                        height = data.height;
                    }
                    else $('#hdnRightPanelContentUrl').val(url);
                    $('#hdnRightPanelContentCode').val($(this).attr('code'));
                    $('#hdnRightPanelContentParam').val(rightPanelContentParam);
                    $('#hdnPopupControlTitle').val(title);
                    pcRightPanelContent.SetHeaderText(title);
                    pcRightPanelContent.SetSize(width, height);
                    pcRightPanelContent.Show();
                    $('#imgCloseRightPanel').click();
                }
                else {
                    $('#hdnRightPanelContentIsLoadContent').val('0');
                    $('#hdnRightPanelContentUrl').val('');
                    $('#<%=pnlRightPanelContentArea.ClientID %>').empty();
                    displayMessageBox("Warning", errMessage.text);
                }
            }
        });

        $('.rboPrint').change(function () {
            if ($(this).attr('isDisplayPrintCount') == '1') {
                $('#divPrinterLocation').show();
                //                $('#<%=hdnIsMultiLocation.ClientID %>').val('1');
                if ($(this).attr('reportcode') != 'PM-00105'
                        && $(this).attr('reportcode') != 'PM-00125'
                        && $(this).attr('reportcode') != 'PM-00126'
                        && $(this).attr('reportcode') != 'PM-00131'
                        && $(this).attr('reportcode') != 'PM-00154'
                        && $(this).attr('reportcode') != 'PM-00156'
                        && $(this).attr('reportcode') != 'PM-00167'
                        && $(this).attr('reportcode') != 'PM-00169'
                        && $(this).attr('reportcode') != 'PM-00188'
                        && $(this).attr('reportcode') != 'PM-00440'
                        && $(this).attr('reportcode') != 'PM-00655') {
                    $('#divJmlLabel').hide();
                }
                else {
                    $('#divJmlLabel').show();
                    $('#txtJmlLabel').focus();
                }
            }
            else {
                //                $('#<%=hdnIsMultiLocation.ClientID %>').val('0');
                $('#divJmlLabel').hide();
                $('#divPrinterLocation').hide();

                if ($('#<%=hdnLstReportDownloadExcel.ClientID %>').val().includes($(this).attr('reportcode'))) {
                    $('.imgRightPanelExport').show();
                }
                else {
                    $('.imgRightPanelExport').hide();
                }
                if ($('#<%=hdnLstReportDownloadRawExcel.ClientID %>').val().includes($(this).attr('reportcode'))) {
                    $('.imgRightPanelRawExport').show();
                }
                else {
                    $('.imgRightPanelRawExport').hide();
                }
                if ($('#<%=hdnLstReportDownloadImageToExcel.ClientID %>').val().includes($(this).attr('reportcode'))) {
                    $('.imgRightPanelDownloadImageToExcel').show();
                }
                else {
                    $('.imgRightPanelDownloadImageToExcel').hide();
                }
            }
        });
    });

    function openUserControlPopup(url, param, title, width, height, code) {
        code = typeof code !== 'undefined' ? code : '';
        $('#hdnRightPanelContentCode').val(code);
        $('#hdnRightPanelContentIsLoadContent').val('1');
        $('#hdnRightPanelContentUrl').val(url);
        $('#hdnRightPanelContentFirstTimeLoad').val('1');
        $('#hdnRightPanelContentParam').val(param);
        $('#hdnPopupControlTitle').val(title);
        pcRightPanelContent.SetHeaderText(title);
        pcRightPanelContent.SetSize(width, height);
        pcRightPanelContent.Show();
    }

    function onCbpRightPanelContentBeginCallback() {
        showLoadingPanel();
    }

    function onCbpRightPanelContentEndCallback() {
        hideLoadingPanel();
        $('#hdnRightPanelContentFirstTimeLoad').val('0');
    }

    function onPcRightPanelContentClosing() {
        $('#hdnRightPanelContentIsLoadContent').val('0');
        $('#hdnRightPanelContentUrl').val('');
        $('#<%=pnlRightPanelContentArea.ClientID %>').empty();
        if (typeof onAfterPopupControlClosing == 'function') {
            onAfterPopupControlClosing();
        }
        if (typeof onAfterViewPopupControlClosing == 'function') {
            onAfterViewPopupControlClosing();
        }
    }
    //#endregion

    //#region Validate
    jQuery.extend(jQuery.validator.messages, {
        required: "",
        remote: "Please fix this field.",
        confirmpassword: "",
        confirmmobilepin: "",
        email: "",
        url: "Please enter a valid URL.",
        date: "Please enter a valid date.",
        dateISO: "Please enter a valid date (ISO).",
        number: "",
        digits: "",
        time: "",
        datepicker: "",
        creditcard: "Please enter a valid credit card number.",
        equalTo: "Please enter the same value again.",
        accept: "Please enter a value with a valid extension.",
        maxlength: jQuery.validator.format("Please enter no more than {0} characters."),
        minlength: jQuery.validator.format("Please enter at least {0} characters."),
        rangelength: jQuery.validator.format("Please enter a value between {0} and {1} characters long."),
        range: jQuery.validator.format("Please enter a value between {0} and {1}."),
        max: "",
        min: "",
        minDate: ""
    });

    window.IsValid = function (evt, fieldID, validationGroup) {
        var $group = $('#' + fieldID);
        var result = true;
        $group.find(':[validationgroup=' + validationGroup + ']').each(function (i, item) {
            if (typeof $(item).attr('readonly') == "undefined") {
                if (typeof $(item).attr('class') != "undefined") {
                    if ($(item).attr('class').indexOf("required") < 0 && $(item).val() == "") {
                    }
                    else if ($(item).is(':visible')) {
                        if (!$(item).valid())
                            result = false;
                    }
                }
            }
        });
        if (result) {
            if (typeof ASPxClientEdit != 'undefined') {
                result = ASPxClientEdit.ValidateGroup(validationGroup);
            }
        }
        return result;
    }

    function isValidDate(date) {
        var matches = /^(\d{2})[-\/](\d{2})[-\/](\d{4})$/.exec(date);
        if (matches == null) return false;
        var d = matches[1];
        var m = matches[2] - 1;
        var y = matches[3];
        var composedDate = new Date(y, m, d);
        return composedDate.getDate() == d &&
            composedDate.getMonth() == m &&
            composedDate.getFullYear() == y;
    }

    $(function () {
        $('span.required input:radio').each(function () {
            $(this).addClass('required');
        });
        $.validator.addMethod("time", function (value, element) {
            return this.optional(element) || /^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])(:([0-5]?[0-9]))?$/i.test(value);
        }, "");
        $.validator.addMethod("datepicker", function (value, element) {
            return isValidDate(value);
        }, "");
        $.validator.addMethod('minDate', function (v, el, minDate) {
            if (this.optional(el)) {
                return true;
            }
            var curDate = $(el).datepicker('getDate');
            return minDate <= curDate;
        }, "");
    });
    //#endregion

    //#region Datepicker
    function setDatePicker(id) {
        $('#' + id).datepicker({
            defaultDate: "w",
            changeMonth: true,
            changeYear: true,
            dateFormat: "dd-mm-yy",
            showOn: "button",
            //showOn: "both",
            buttonImage: ResolveUrl("~/Libs/Images/calendar.gif"),
            buttonImageOnly: true
        });
    }

    function setDatePickerElement($elm) {
        $elm.datepicker({
            defaultDate: "w",
            changeMonth: true,
            changeYear: true,
            dateFormat: "dd-mm-yy",
            showOn: "button",
            //showOn: "both",
            buttonImage: ResolveUrl("~/Libs/Images/calendar.gif"),
            buttonImageOnly: true
        });
    }

    var disabledTool = new Date();
    disabledTool.setDate(disabledTool.getDate() - 2);
    disabledTool.setHours(0, 0, 0, 0);
    function setDatePickerWithHoliday(id) {
        $('#' + id).datepicker({
            defaultDate: "w",
            changeMonth: true,
            changeYear: true,
            dateFormat: "dd-mm-yy",
            showOn: "button",
            //showOn: "both",
            buttonImage: ResolveUrl("~/Libs/Images/calendar.gif"),
            buttonImageOnly: true,
            beforeShowDay: holidayCheck
        });
    }

    function holidayCheck(in_date) {
        in_date = in_date.getDate() + '/'
                + (in_date.getMonth() + 1) + '/' + in_date.getFullYear();
        var newDate = new Date();
        var day = newDate.getDate();
        var month = newDate.getMonth() + 1;
        var year = newDate.getFullYear();
        if (day < 10) {
            day = '0' + day;
        }

        if (month < 10) {
            month = '0' + month;
        }
        var holidayDate = [];
        var holidayName = [];
        var filterHoliday = "IsDeleted = 0 AND (IsAnnualHoliday = 1 OR HolidayYear = '" + year + "')";
        Methods.getListObject('GetHolidayList', filterHoliday, function (result) {
            for (i = 0; i < result.length; i++) {
                var yyyy = year;
                if (result[i].IsAnnualHoliday) {
                    yyyy = result[i].HolidayYear;
                }
                holidayDate[i] = result[i].HolidayDate + "/" + result[i].HolidayMonth + "/" + yyyy;
                holidayName[i] = result[i].HolidayName;
            }
        });

        if (holidayDate.indexOf(in_date) >= 0) {
            return [false, 'holiday', holidayName[holidayDate.indexOf(in_date)]];
        } else {
            return [true, "", ""];
        }
    }

    /*$(function () {
    $('.datepicker').datepicker({
    defaultDate: "w",
    changeMonth: true,
    changeYear: true,
    dateFormat: "dd-M-yy",
    beforeShowDay: highlightDays
    });

    var dates = ['04/30/2013', '05/01/2013'];
    function highlightDays(date) {
    for (var i = 0; i < dates.length; i++) {
    if (new Date(dates[i]).toString() == date.toString()) {
    return [true, 'holiday'];
    }
    }
    return [true, ''];

    }
    });*/
    //#endregion

    //#region Today Date
    var todayDateInString = '<%:TodayDate%>';
    var DateNow = '<%:defaultDatePickerNow%>';
    var TodayDateNow = '<%:TodayDateNow%>';
    window.todayDate = Methods.stringToDate(todayDateInString);
    function getDateNow() {
        return TodayDateNow;
    }
    function getDateNowDatePickerFormat() {
        /*var now = new Date();
        var date = ('0' + now.getDate()).slice(-2);
        var month = ('0' + (now.getMonth() + 1)).slice(-2);
        var year = now.getFullYear();
        return date + '-' + month + '-' + year;*/

        return DateNow;
    }

    function getTimeNow() {
        var now = new Date();
        var hour = ('0' + now.getHours()).slice(-2);
        var minute = ('0' + now.getMinutes()).slice(-2);

        return hour + ':' + minute;
    }

    //#endregion

    //#region AppSession
    var AppSession = new (function () {
        this.healthcareID = '<%:HealthcareID%>';
        this.userID = parseInt('<%:UserID%>');
    })();
    //#endregion

    //#region Numeric only without decimal
    function checkMinus(value) {
        value = value.replace(',', '');
        var newchar = '';
        value = value.split(',').join(newchar);
        if (isNaN(value)) {
            return 0;
        }
        else {
            if (value < 0) {
                return 0;
            }
            else {
                if (value.indexOf(".") == -1) {
                    return value;
                }
                else {
                    return 0;
                }
            }
        }
    }

    function checkMinusDecimalOK(value) {
        value = value.replace(',', '');
        var newchar = '';
        value = value.split(',').join(newchar);
        if (isNaN(value)) {
            return 0;
        }
        else {
            if (value < 0) {
                return 0;
            }
            else {
                return value;
            }
        }
    }
    //#endregion

    //#region validate two date from and to (from changed)
    function validateDateFromTo(value1, value2) {
        var start = value1;
        var end = value2;

        var from = start.split("-");
        var f = new Date(from[2], from[1] - 1, from[0]);

        var to = end.split("-");
        var t = new Date(to[2], to[1] - 1, to[0]);

        if (f > t) {
            return end;
        }
        else {
            return start;
        }
    }
    //#endregion

    //#region validate two date from and to (to changed)
    function validateDateToFrom(value1, value2) {
        var start = value1;
        var end = value2;

        var from = start.split("-");
        var f = new Date(from[2], from[1] - 1, from[0]);

        var to = end.split("-");
        var t = new Date(to[2], to[1] - 1, to[0]);

        if (t < f) {
            return start;
        }
        else {
            return end;
        }
    }
    //#endregion

    //#region distinct array
    function onlyUnique(value, index, self) {
        return self.indexOf(value) === index;
    }
    //#endregion

    //#region Paging
    function setPaging($elm, pageCount, onPageChanged, display, start) {
        if (display == null)
            display = 12;
        if (start == null)
            start = 1;
        if (pageCount > 0) {
            $elm.closest('.containerPaging').show();
            $elm.paginate({
                count: pageCount,
                start: start,
                display: display,
                border: false,
                text_color: '#79B5E3',
                background_color: 'none',
                text_hover_color: '#FFF',
                background_hover_color: 'none',
                images: false,
                mouse: 'press',
                onChange: function () {
                    var page = $elm.find('.jPag-current').html();
                    onPageChanged(page);
                }
            });
        }
        else
            $elm.closest('.containerPaging').hide();
    }
    //#endregion

    function openMatrixControl(type, param, headerText) {
        var url = ResolveUrl("~/Libs/Controls/MatrixCtl.ascx");
        openUserControlPopup(url, type + '|' + param, headerText, 1000, 500);
    }
    function openMatrixWithParameterControl(type, param, headerText) {
        var url = ResolveUrl("~/Libs/Controls/MatrixWithParameterCtl.ascx");
        openUserControlPopup(url, type + '|' + param, headerText, 1000, 500);
    }

    //#region Utility
    function setCheckBoxEnabled($item, isEnabled) {
        if (isEnabled)
            $item.removeAttr('disabled');
        else
            $item.attr('disabled', 'disabled');
    }

    function rowToObject($row) {
        var selectedObj = {};
        $row.find('input[type=hidden]').each(function () {
            if ($(this).attr('bindingfield') != null)
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
        });
        return selectedObj;
    }
    //#endregion

    function onCbpDirectPrintProcessDirectEndCallback(s) {
        hideLoadingPanel();
        var value = s.cpZebraPrinting;
        if (value != "") {
            var e_id = 'id_' + new Date().getTime();
            if (window.chrome) {
                $('body').append('<a id=\"' + e_id + '\"></a>');
                $('#' + e_id).attr('href', 'PDClient:' + value);
                var a = $('a#' + e_id)[0];
                var evObj = document.createEvent('MouseEvents');
                evObj.initEvent('click', true, true);
                a.dispatchEvent(evObj)
            } else {
                $('body').append('<iframe name=\"' + e_id + '\" id=\"' + e_id + '\" width=\"1\" height=\"1\" style=\"visibility:hidden;position:absolute\" />');
                $('#' + e_id).attr('src', 'PDClient:' + value)
            }
            setTimeout(function () {
                $('#' + e_id).remove()
            }, 5000)
        }
    }

    function FormatMoneyToNumeric(param) {
        return param.replace(/[^0-9-.]/g, '');
    }

    function ShowSnackbarNotification(message) {
        var x = document.getElementById("snackbarNotification");
        x.className = "show";
        x.innerHTML = message;
        setTimeout(function () { x.className = x.className.replace("show", ""); }, 2000);
    }
    function ShowSnackbarWarning(message) {
        var x = document.getElementById("snackbarNotificationWarning");
        x.className = "show";
        x.innerHTML = "<b>WARNING </b> : </br>" + message;
        setTimeout(function () { x.className = x.className.replace("show", ""); }, 2000);
    }
    function ShowSnackbarError(message) {
        var x = document.getElementById("snackbarNotificationError");
        x.className = "show";
        x.innerHTML = "<b>ERROR </b> : </br>" + message;
        setTimeout(function () { x.className = x.className.replace("show", ""); }, 2000);
    }
    function ShowSnackbarSuccess(message) {
        var x = document.getElementById("snackbarNotificationSuccess");
        x.className = "show";
        x.innerHTML = "<b>SUCCESS </b> : </br>" + message;
        setTimeout(function () { x.className = x.className.replace("show", ""); }, 2000);
    }
    function CloseExpiredWindow() {
        window.close();
    }

    window.onload = function () {
        function _aspxGetDocumentScrollTop() {
            return document.documentElement.scrollTop || document.body.scrollTop
        }
        if (window._aspxGetDocumentScrollTop) {
            window._aspxGetDocumentScrollTop = _aspxGetDocumentScrollTop;
        }
        /* Begin -> Correct ScrollLeft */
        function _aspxGetDocumentScrollLeft() {
            return document.documentElement.scrollLeft || document.body.scrollLeft
        }
        if (window._aspxGetDocumentScrollLeft) {
            window._aspxGetDocumentScrollLeft = _aspxGetDocumentScrollLeft;
        }
    }

    //#region Socket
    $(document).ready(function () {
        var url = $('#<%=hdnURLSocketNoification.ClientID %>').val();  ////'ws://localhost:3000'

        //AD >> baru sampel sementara di matikan dulu fiturnya.
        var hdnIsSocketNotification = $('#<%=hdnIsSocketNotification.ClientID %>').val();
        if (hdnIsSocketNotification == "1") {
            var socket = io(url, { transports: ['websocket'] });
            socket.on('chat_message', function (msg) {
                ShowSnackbarNotification(msg);
            });

            socket.on('notification_sendOrder', function (msg) {
                var message = msg.split('|');
                var msgText = message[1];
                /*
                01 = Penunjang, 02 = Farmasi , 03 = radiologi  , 04 = laboratorium
                */
                if (message[0] == "01") {
                    playAudioNotification();
                }
                ShowSnackbarNotification(msgText);

            });
        }
        function playAudioNotification() {
            var x = document.getElementById("myAudioNotif");
            x.play();
        }

        function SendOrderNotifCTL(messageText){
            socket.emit(messageText);
        }

    });

    //#endregion 

    //#region Text Number Only 
    jQuery.fn.TextNumericOnly =
    function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                // allow backspace, tab, delete, enter, arrows, numbers and keypad numbers ONLY
                // home, end, period, and numpad decimal
                return (
                    key == 8 ||
                    key == 9 ||
                    key == 13 ||
                    key == 46 ||
                    key == 110 ||
                    key == 190 ||
                    (key >= 35 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
            });
        });
    };
    //#endregion
    //#region Maximal Input
    function MaxCount(txtBox, e, maxLength) {
        if (txtBox) {
            if (txtBox.value.length > maxLength) {
                txtBox.value = txtBox.value.substring(0, maxLength);
            }
            if (!checkSpecialKeys(e)) {
                return (txtBox.value.length <= maxLength)
            }
        }
    }

    function checkSpecialKeys(e) {
        if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
            return false;
        else
            return true;
    }
    //#endregion
</script>
<style type="text/css">
    /* #ExpiredLicense{display:block; margin: auto;} */ 
    #snackbarNotification {
        visibility: hidden;
        max-width: 300px;
        margin-left: -125px;
        background-color: #333;
        color: #fff;
        text-align: left;
        border-radius: 15px;
        padding: 16px;
        position: fixed;
        z-index: 99999;
        right: 1%;
        bottom: 10px;
        font-size: 14px;
        border: 1px;
        box-shadow: 10px 10px 5px #aaaaaa;
    }
    #snackbarNotification.show {
        visibility: visible;
        -webkit-animation: fadein 0.5s, fadeout 0.5s 2.5s;
        animation: fadein 0.5s, fadeout 0.5s 2.5s;
    }
    
    #snackbarNotificationWarning {
        visibility: hidden;
        max-width: 300px;
        margin-left: -125px;
        background-color: #FFFF00;
        color: #000000;
        text-align: left;
        border-radius: 15px;
        padding: 16px;
        position: fixed;
        z-index: 99999;
        right: 1%;
        bottom: 10px;
        font-size: 14px;
        border: 1px;
        box-shadow: 10px 10px 5px #aaaaaa;
    }
    #snackbarNotificationWarning.show {
        visibility: visible;
        -webkit-animation: fadein 0.5s, fadeout 0.5s 2.5s;
        animation: fadein 0.5s, fadeout 0.5s 2.5s;
    }
    
    #snackbarNotificationError {
        visibility: hidden;
        max-width: 300px;
        margin-left: -125px;
        background-color: #FF0000;
        color: #000000;
        text-align: left;
        border-radius: 15px;
        padding: 16px;
        position: fixed;
        z-index: 99999;
        right: 1%;
        bottom: 10px;
        font-size: 14px;
        border: 1px;
        box-shadow: 10px 10px 5px #aaaaaa;
    }
    #snackbarNotificationError.show {
        visibility: visible;
        -webkit-animation: fadein 0.5s, fadeout 0.5s 2.5s;
        animation: fadein 0.5s, fadeout 0.5s 2.5s;
    }
    
    #snackbarNotificationSuccess {
        visibility: hidden;
        max-width: 300px;
        margin-left: -125px;
        background: rgb(67,145,60);
        background: linear-gradient(90deg, rgba(67,145,60,1) 0%, rgba(32,179,108,1) 22%, rgba(32,179,71,1) 100%);
        color: #000000;
        text-align: left;
        border-radius: 15px;
        padding: 16px;
        position: fixed;
        z-index: 99999;
        right: 1%;
        bottom: 10px;
        font-size: 14px;
        border: 1px;
        box-shadow: 10px 10px 5px #aaaaaa;
    }
    #snackbarNotificationSuccess.show {
        visibility: visible;
        -webkit-animation: fadein 0.5s, fadeout 0.5s 2.5s;
        animation: fadein 0.5s, fadeout 0.5s 2.5s;
    }

    @-webkit-keyframes fadein {
        from {bottom: 0; opacity: 0;} 
        to {bottom: 10px; opacity: 1;}
    }

    @keyframes fadein {
        from {bottom: 0; opacity: 0;}
        to {bottom: 10px; opacity: 1;}
    }

    @-webkit-keyframes fadeout {
        from {bottom: 10px; opacity: 1;} 
        to {bottom: 0; opacity: 0;}
    }

    @keyframes fadeout {
        from {bottom: 10px; opacity: 1;}
        to {bottom: 0; opacity: 0;}
    }   
    
    #loadingPanel
    {
        display: none;
    }
    #loadingPanel .divBlanket
    {
        background-color: #EEE;
        opacity: 0.65;
        -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=65)";
        -moz-opacity: 0.65;
        -khtml-opacity: 0.65;
        position: fixed;
        z-index: 29001;
        top: 0px;
        left: 0px;
        width: 100%;
        height: 100%;
    }
    #loadingPanel .divLoading
    {
        position: fixed;
        top: 50%;
        left: 50%;
        width: 200px;
        height: 50px;
        margin-top: -15px;
        margin-left: -100px;
        z-index: 29002;
        text-align: center;
        vertical-align: middle;
    }
    #loadingPanel .imgLoading
    {
        float: left;
        margin-top: 3px;
    }
    
    /* #region Right Panel */
    #containerRightPanel
    {
        font-size: 0.9em;
    }
    .divOpenRightPanelContent
    {
        max-height: 105px;
        display: none;
    }
    #tdOpenRightPanel
    {
        opacity: 0.6;
    }
    #tdOpenRightPanel.open
    {
        opacity: 1;
    }
    .textRightPanel
    {
        position: absolute;
        right: -27px;
        top: 33px;
        text-align: center;
        width: 80px;
        color: #FFFFFF;
        -webkit-transform: rotate(-90deg);
        -moz-transform: rotate(-90deg);
        filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=3);
    }
    .divRightPanelBackground
    {
        position: relative;
        cursor: pointer;
        background-image: url('<%=ResolveUrl("~/Libs/Images/Slide/tab_mrotate.png")%>');
        height: 70px;
        width: 30px;
        margin: 0px auto;
        color: White;
        padding-top: 10px;
    }
    .divRightPanelBackgroundTop
    {
        cursor: pointer;
        background-image: url('<%=ResolveUrl("~/Libs/Images/Slide/tab_t.png")%>');
        height: 13px;
        width: 30px;
    }
    .divRightPanelBackgroundBottom
    {
        cursor: pointer;
        background-image: url('<%=ResolveUrl("~/Libs/Images/Slide/tab_btm.png")%>');
        height: 13px;
        width: 30px;
    }
    .divRightPanelBackground:hover .textRightPanel
    {
        text-decoration: underline;
    }
    
    .divRightPanelBackground.selected
    {
        background-image: url('<%=ResolveUrl("~/Libs/Images/Slide/tab_mrotate_sel.png")%>');
    }
    .divRightPanelBackgroundTop.selected
    {
        background-image: url('<%=ResolveUrl("~/Libs/Images/Slide/tab_t_sel.png")%>');
    }
    .divRightPanelBackgroundBottom.selected
    {
        background-image: url('<%=ResolveUrl("~/Libs/Images/Slide/tab_btm_sel.png")%>');
    }
    .divRightPanelBackground.selected .textRightPanel
    {
        color: #000;
    }
    
    .containerRightPanelContent
    {
        display: none;
    }
    #divListRightPanel .rightPanelContent
    {
        width: 100%;
        padding: 10px 2px;
    }
    #divListRightPanel .rightPanelContent div
    {
        margin-right: 20px;
    }
    #divListRightPanel .rightPanelContent div.qmtitle
    {
        font-size: 1.2em !important;
    }
    #divListRightPanel .rightPanelContent div.qmdescription
    {
        font-size: 1em;
    }
    #divListRightPanel .rightPanelContent a
    {
        border: 0px;
        text-decoration: none;
        padding: 4px 10px;
        float: right;
        width: 20px;
    }
    #divListRightPanel .rightPanelContent a[enabled="false"]
    {
        /*background-color: #EAEAEA; color: #BEBEBE;*/
        background-color: #AAA;
        color: #979797;
        cursor: not-allowed;
    }
    .containerRightPanelContent.print
    {
        font-size: 14px;
    }
    #headerRightPanel
    {
        border-bottom: 1px solid #FFF;
        padding-bottom: 5px;
    }
    #imgCloseRightPanel
    {
        cursor: pointer;
    }
    #headerRightPanelTitle
    {
        float: right;
        padding: 0 5px 0 0;
        font-size: 1.6em;
    }
    #imgRightPanelPrint
    {
        cursor: pointer;
    }
    #imgRightPanelPrint:hover
    {
        background-color: #F39200;
    }
    /* #endregion */
</style>
<!--[if IE]>
<style type="text/css">
    .textRightPanel                                 { right: -60px; top: 0;  }
</style>
<![endif]-->
 <audio id="myAudioNotif"> 
  <source src='<%=ResolveUrl("~/Libs/Sounds/bell2.mp3")%>' type="audio/mpeg">
  Your browser does not support the audio element.
</audio>
<input type="hidden" id="hdnIsSocketNotification" runat="server" value="0" />
<input type="hidden" id="hdnURLSocketNoification" runat="server" value="" />
<div id="snackbarNotification">
</div>
<div id="snackbarNotificationWarning">
</div>
<div id="snackbarNotificationError">
</div>
<div id="snackbarNotificationSuccess">
</div>
<div style="position: absolute;">
    <dxpc:ASPxPopupControl ID="pcRightPanelContent" runat="server" ClientInstanceName="pcRightPanelContent"
        EnableHierarchyRecreation="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" FooterText="" HeaderText="" Modal="True" AllowDragging="True"
         Width="950px" Height="600px" CloseAction="CloseButton">
        <ClientSideEvents Shown="function (s,e) { cbpRightPanelContent.PerformCallback(); }"
            Closing="function(s,e) { onPcRightPanelContentClosing(); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div style="text-align: left; width: 100%;">
                    <dxcp:ASPxCallbackPanel ID="cbpRightPanelContent" runat="server" Width="100%" ClientInstanceName="cbpRightPanelContent"
                        ShowLoadingPanel="false" OnCallback="cbpRightPanelContent_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ onCbpRightPanelContentBeginCallback(); }"
                            EndCallback="function(s,e){ onCbpRightPanelContentEndCallback(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="pnlRightPanelContentArea" Style="width: 100%; margin-left: auto;
                                    margin-right: auto">
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</div>
<div id="loadingPanel">
    <div class="divBlanket">
    </div>
    <div class="divLoading">
        <table style="margin-left: auto; margin-right: auto;">
            <tr>
                <td>
                    <img class="imgLoading" src="<%=ResolveUrl("~/Libs/Images/Loading.gif")%>" alt="0" />
                </td>
                <td style="padding-left: 5px">
                    <div class="txtLoading">
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
<div id="containerRightPanel" style="display: none; position: absolute; right: 0px;
    top: 270px; z-index: 20000;">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td style="vertical-align: top; padding-top: 50px;" id="tdOpenRightPanel">
                <table cellpadding="0" cellspacing="0">
                    <tr class="divOpenRightPanelContent" contentid="tasks">
                        <td>
                            <div class="divRightPanelBackgroundTop">
                                &nbsp;</div>
                            <div class="divRightPanelBackground">
                                <div class="textRightPanel">
                                    <%=GetLabel("Tasks")%></div>
                            </div>
                            <div class="divRightPanelBackgroundBottom" />
                        </td>
                    </tr>
                    <tr class="divOpenRightPanelContent" contentid="information">
                        <td>
                            <div class="divRightPanelBackgroundTop">
                                &nbsp;</div>
                            <div class="divRightPanelBackground">
                                <div class="textRightPanel">
                                    <%=GetLabel("Information")%></div>
                            </div>
                            <div class="divRightPanelBackgroundBottom" />
                        </td>
                    </tr>
                    <tr class="divOpenRightPanelContent" contentid="print">
                        <td>
                            <div class="divRightPanelBackgroundTop">
                                &nbsp;</div>
                            <div class="divRightPanelBackground">
                                <div class="textRightPanel">
                                    <%=GetLabel("Print")%></div>
                            </div>
                            <div class="divRightPanelBackgroundBottom" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align: top;">
                <div id="divListRightPanel" style="width: 350px; border: 1px solid #AAA; min-height: 450px;
                    padding: 5px;">
                    <div id="headerRightPanel">
                        <div id="headerRightPanelTitle">
                            <%=GetLabel("Tasks")%></div>
                        <img id="imgCloseRightPanel" src='<%=ResolveUrl("~/Libs/Images/Icon/right.png")%>'
                            alt="C" title="Close" />
                    </div>
                    <div class="containerRightPanelContent tasks" style="height: 350px; overflow-y: scroll">
                        <asp:Repeater ID="rptTasks" runat="server">
                            <ItemTemplate>
                                <div class="rightPanelContent borderBox">
                                    <a class="goRightPanelContent" href="javascript:void(0);" pcwidth="<%#: Eval("Width")%>"
                                        pcheight="<%#: Eval("Height")%>" id="<%#: Eval("ID")%>" code="<%#: Eval("Code")%>"
                                        url="<%#: Eval("Url")%>">Go</a>
                                    <div class='qmtitle'>
                                        <%#: Eval("Title")%></div>
                                    <div class='qmdescription'>
                                        <%#: Eval("Description")%></div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="containerRightPanelContent information" style="height: 350px; overflow-y: scroll">
                        <asp:Repeater ID="rptInformation" runat="server">
                            <ItemTemplate>
                                <div class="rightPanelContent borderBox">
                                    <a class="goRightPanelContent" href="javascript:void(0);" pcwidth="<%#: Eval("Width")%>"
                                        pcheight="<%#: Eval("Height")%>" id="<%#: Eval("ID")%>" code="<%#: Eval("Code")%>"
                                        url="<%#: Eval("Url")%>">Go</a>
                                    <div class='qmtitle'>
                                        <%#: Eval("Title")%></div>
                                    <div class='qmdescription'>
                                        <%#: Eval("Description")%></div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="containerRightPanelContent print" style="z-index: 20000;">
                        <div class="pageTitle">
                            <input type="hidden" id="hdnLstReportDownloadExcel" value="" runat="server" />
                            <input type="hidden" id="hdnLstReportDownloadRawExcel" value="" runat="server" />
                            <input type="hidden" id="hdnLstReportDownloadImageToExcel" value="" runat="server" />
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <img id="imgRightPanelPrint" src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>'
                                            alt="P" title="Print" />
                                        <img id="imgRightPanelExport" class="imgRightPanelExport" src='<%=ResolveUrl("~/Libs/Images/Icon/tbexcel.png")%>'
                                            alt="E" title="Export" style="display:none"/>
                                        <img id="imgRightPanelRawExport" class="imgRightPanelRawExport" src='<%=ResolveUrl("~/Libs/Images/Icon/tbrawexcel.png")%>'
                                            alt="R" title="RAW Excel" style="display:none"/>
                                        <img id="imgRightPanelDownloadImageToExcel" class="imgRightPanelDownloadImageToExcel" src='<%=ResolveUrl("~/Libs/Images/Icon/tbimageexcel.png")%>'
                                            alt="I" title="Export Report To Excel" style="display:none"/>
                                    </td>
                                    <td style="padding-left: 5px">
                                        <div id="divJmlLabel" style="display: none">
                                            <input type="hidden" id="hdnIsMultiLocation" value="0" runat="server" />
                                            <input type="hidden" id="hdnMaxLabelNo" value="20" runat="server" />
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 60px" />
                                                    <col style="width: 50px" />
                                                </colgroup>
                                                <tr>
                                                    <td style="font-size: 0.8em">
                                                        Jumlah
                                                    </td>
                                                    <td style="padding-left: 3px">
                                                        <asp:TextBox runat="server" ID="txtJmlLabel" Text="1" Width="95%" TextMode="Number">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="divPrinterLocation" style="display: none">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td id="tdPrinterLocation" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td style="width: 100px; font-size: 0.8em">
                                                        Lokasi Printer
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboPrinterUrl" ClientInstanceName="cboPrinterUrl" Width="100%"
                                                            runat="server">

                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="rightPanelPrintContent" style="height: 350px; overflow-y: scroll">
                            <asp:Repeater ID="rptPrint" runat="server">
                                <ItemTemplate>
                                    <input type="radio" class="rboPrint" name="rboPrint" value="1" reportcode='<%#: Eval("ReportCode")%>'
                                        isdisplayprintcount='<%#: Eval("isDisplayPrintCount")%>' /><%#: Eval("Title")%><br />
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div style="display: none;">
                            <asp:Button ID="btnExport" Visible="true" runat="server" OnClick="btnExport_Click"
                                Text="Export" UseSubmitBehavior="false" OnClientClick="return true;" />
                            <asp:Button ID="btnRAWExport" Visible="true" runat="server" OnClick="btnRAWExport_Click"
                                Text="RAW Export" UseSubmitBehavior="false" OnClientClick="return true;" />
                        </div>
                        <br />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
<div id="toastPanel" class="toastPanel">
    <div class="tblToast">
        <div class="tblCellToast">
            <div class="toastBox">
                <div class="messageContainer">
                    <h2 class="messageHeader">
                        <%=GetLabel("Warning")%></h2>
                    <div class="messageText" style="padding-top: 20px; min-height: 140px; max-width: 950px; white-space:pre-wrap">
                    </div>
                </div>
                <input type="button" class="btnClose" style="width: 100px;" onclick="hideToast()"
                    value='<%=GetLabel("Close")%>' />
            </div>
        </div>
    </div>
    <div class="blanket">
    </div>
</div>
<div id="toastConfirmationPanel" class="toastPanel">
    <div class="tblToast">
        <div class="tblCellToast">
            <div class="toastBox">
                <div class="messageContainer">
                    <h2 class="messageHeader">
                        <%=GetLabel("Confirmation")%></h2>
                    <div class="messageText">
                    </div>
                </div>
                <input type="button" class="btnOK" style="width: 100px;" onclick="hideToastConfirmation(true)"
                    value='<%=GetLabel("Yes")%>' />
                <input type="button" class="btnClose" style="width: 100px;" onclick="hideToastConfirmation(false)"
                    value='<%=GetLabel("No")%>' />
            </div>
        </div>
    </div>
    <div class="blanket">
    </div>
</div>
<!-- The actual snackbar -->
<div id="snackbar" style="display: none">
    <div class="snackbarMessageText">
        Hello User !
    </div>
</div>
<div id="messageBox" class="w3-modal">
    <div class="w3-modal-content w3-animate-zoom">
        <header class="w3-container w3-pale-blue"> 
      <span onclick="document.getElementById('messageBox').style.display='none'" 
      class="w3-button w3-display-topright">&times;</span>
      <h2 class="messageTitle"></h2>
    </header>
        <div class="w3-container">
            <p>
                <div class="messageBody" style="min-height: 125px; max-height: 300px; overflow: auto">
                </div>
            </p>
        </div>
        <footer class="w3-container w3-pale-blue">
      <p style="text-align:center"><input class="w3-btn w3-orange w3-round-xxlarge w3-hover-blue" style="width:100px" onclick="document.getElementById('messageBox').style.display='none'" value="OK" /></p>
    </footer>
    </div>
</div>
<div id="messageBoxError" class="w3-modal">
    <div class="w3-modal-content w3-animate-zoom">
        <header class="w3-container w3-red"> 
      <span onclick="document.getElementById('messageBoxError').style.display='none'" 
      class="w3-button w3-display-topright">&times;</span>
      <h2 class="messageTitle"></h2>
    </header>
        <div class="w3-container">
            <p>
                <div class="messageBody" class="messageBody" style="min-height: 125px; max-height: 250px;
                    overflow: auto">
                </div>
            </p>
        </div>
        <footer class="w3-container w3-red">
      <p style="text-align:center"><input class="w3-btn w3-black w3-round-xxlarge w3-hover-blue" style="width:100px" onclick="document.getElementById('messageBoxError').style.display='none'" value="OK" /></p>
    </footer>
    </div>
</div>
<div id="messageBoxConfirmation" class="w3-modal">
    <div class="w3-modal-content w3-animate-zoom">
        <header class="w3-container w3-pale-blue"> 
          <span onclick="document.getElementById('messageBoxConfirmation').style.display='none'" 
          class="w3-button w3-display-topright">&times;</span>
          <h2 class="messageTitle"></h2>
        </header>
        <div class="w3-container">
            <p>
                <div class="messageBody" style="min-height: 125px; max-height: 250px; overflow: auto">
                </div>
            </p>
        </div>
        <footer class="w3-container w3-pale-blue">
          <p style="text-align:center">
            <input class="w3-btn w3-orange w3-round-xxlarge w3-hover-blue" style="width:100px" onclick="hideConfirmationMessageBox(true)" value="YES" />
            <input class="w3-btn w3-orange w3-round-xxlarge w3-hover-blue" style="width:100px" onclick="hideConfirmationMessageBox(false)" value="NO" />
          </p>
        </footer>
    </div>
</div>
<div id="messageBoxWithFooter" class="w3-modal">
    <div class="w3-modal-content w3-animate-zoom">
        <header class="w3-container w3-red"> 
      <span onclick="document.getElementById('messageBox').style.display='none'" 
      class="w3-button w3-display-topright">&times;</span>
      <h2 class="messageTitle"></h2>
    </header>
        <div class="w3-container">
            <p>
                <div class="messageBody">
                </div>
            </p>
        </div>
        <footer class="w3-container w3-red">
      <p></p>
    </footer>
    </div>
</div>
<!-- Modal that pops up when you click on "New Message" -->
<div id="id01" class="w3-modal" style="z-index: 4">
    <div class="w3-modal-content w3-animate-zoom">
        <div class="w3-container w3-padding w3-blue">
            <span onclick="document.getElementById('id01').style.display='none'" class="w3-button w3-right w3-xxlarge">
                <i class="fa fa-remove"></i></span>
            <h2>
                Send Message</h2>
        </div>
        <div class="w3-panel">
            <label>
                To</label>
            <input class="w3-input w3-border w3-margin-bottom" type="text" style="width: 98%">
            <label>
                From</label>
            <input class="w3-input w3-border w3-margin-bottom" type="text" style="width: 98%">
            <label>
                Subject</label>
            <input class="w3-input w3-border w3-margin-bottom" type="text" style="width: 98%">
            <input class="w3-input w3-border w3-margin-bottom" style="height: 150px; width: 98%"
                placeholder="What's on your mind?">
            <div class="w3-section">
                <a class="w3-button w3-red" onclick="document.getElementById('id01').style.display='none'">
                    Cancel <i class="fa fa-remove"></i></a><a class="w3-button w3-right" onclick="document.getElementById('id01').style.display='none'">
                        Send <i class="fa fa-paper-plane"></i></a>
            </div>
        </div>
    </div>
</div>
<dxpc:ASPxPopupControl ID="pcSearchDialog" runat="server" ClientInstanceName="pcSearchDialog"
    EnableHierarchyRecreation="True" FooterText="" HeaderText="" Modal="True" AllowDragging="True"
    PopupHorizontalAlign="WindowCenter" Width="1200px" PopupVerticalAlign="WindowCenter"
    CloseAction="CloseButton">
    <ClientSideEvents Shown="function (s,e) {isInitSearchDialog = true; cbpSearchDialog.PerformCallback('open|' + searchDialogSearchType + '|' + searchDialogFilterExpression); }"
        Closing="function(s,e){ txtQuickSearchDialog.SetBlur(); txtQuickSearchDialog.SetText(''); $('#' + grdSearchID).empty(); }" />
    <ContentCollection>
        <dxpc:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
            <table style="float: right; display: none;">
                <tr>
                    <td>
                        <input type="text" id="txtSearchResult" />
                    </td>
                    <td>
                        <input type="button" value="Find Next" onclick="highlightText();" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtQuickSearchDialog"
                            ID="txtQuickSearchDialog" Width="300px" Watermark="Search">
                            <ClientSideEvents SearchClick="function(s){ s.SetBlur(); cbpSearchDialog.PerformCallback('refresh|' + s.GenerateFilterExpression()); s.SetFocus(); }" />
                        </qis:QISIntellisenseTextBox>
                    </td>
                </tr>
            </table>
            <div style="clear: both" />
            <div id="containerSearchResult" style="height: 400px; overflow-y: scroll; position: relative">
                <dxcp:ASPxCallbackPanel ID="cbpSearchDialog" runat="server" Width="100%" ClientInstanceName="cbpSearchDialog"
                    ShowLoadingPanel="false" OnCallback="cbpSearchDialog_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingSearchDialog').show(); }"
                        EndCallback="function(s,e){ onCbpSearchDialogEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlQuickMenuArea" Style="width: 100%; margin-left: auto;
                                margin-right: auto">
                                <asp:GridView ID="grdSearch" Width="100%" runat="server" CssClass="grdView" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada data yang ditemukan")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingSearchDialog">
                    <img src='' alt='' />
                </div>
            </div>
        </dxpc:PopupControlContentControl>
    </ContentCollection>
</dxpc:ASPxPopupControl>
<div style="display: none">
    <dxcp:ASPxCallbackPanel ID="cbpDirectPrintProcess" runat="server" Width="100%" ClientInstanceName="cbpDirectPrintProcess"
        ShowLoadingPanel="false" OnCallback="cbpDirectPrintProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e){
            showLoadingPanel();
        }" EndCallback="function(s,e){       
            var printResult = s.cpZebraPrinting;
            if (printResult != '')
                showToast('Warning', printResult);
            hideLoadingPanel();}" />
    </dxcp:ASPxCallbackPanel>
    <dxcp:ASPxCallbackPanel ID="cbpDirectPrintProcessDirect" runat="server" Width="100%"
        ClientInstanceName="cbpDirectPrintProcessDirect" ShowLoadingPanel="false" OnCallback="cbpDirectPrintProcessDirect_Callback">
        <ClientSideEvents BeginCallback="function(s,e){
            showLoadingPanel();
        }" EndCallback="function(s,e){ onCbpDirectPrintProcessDirectEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
<input type="hidden" id="hdnRightPanelContentIsLoadContent" name="hdnRightPanelContentIsLoadContent"
    value="0" />
<input type="hidden" id="hdnRightPanelContentUrl" name="hdnRightPanelContentUrl"
    value="" />
<input type="hidden" id="hdnRightPanelContentFirstTimeLoad" name="hdnRightPanelContentFirstTimeLoad"
    value="0" />
<input type="hidden" id="hdnRightPanelContentParam" name="hdnRightPanelContentParam"
    value="" />
<input type="hidden" id="hdnRightPanelContentCode" name="hdnRightPanelContentCode"
    value="" />
<input type="hidden" id="hdnPopupControlTitle" name="hdnPopupControlTitle" value="" />
<input type="hidden" id="hdnPopupControlTagField" name="hdnPopupControlTagField"
    value="" />
<input type="hidden" id="hdnIsRightPrintContentExists" name="hdnIsRightPrintContentExists"
    value="0" runat="server" />
<input type="hidden" id="hdnReportCode" runat="server" value="" />
<input type="hidden" id="hdnReportParam" runat="server" value="" />
<input type="hidden" id="hdnPopupActiveControl" name="hdnPopupActiveControl" value="0" />
<!-- region Layout Expired License -->
<div id="ExpiredLicense" class="w3-modal">
    <div class="w3-modal-content w3-card-4 w3-animate-zoom" style="max-width: 600px">
        <div class="w3-card-4">
            <div class="w3-container w3-blue">
                <h2>
                    Expired License</h2>
            </div>
            <div class="w3-container">
                <p>
                    Batas waktu penggunaan aplikasi sudah berakhir tanggal <span id="DateExpiredLicense">
                    </span>, silahkan hubungi info@medinfras.com untuk info lebih lanjut.
                </p>
                <div style="padding: 10px;">
                    <a href="#" class="w3-btn w3-red" style="width: 90%" onclick="cbpCloselogout.PerformCallback();">
                        Close </a>
                </div>
            </div>
        </div>
    </div>
</div>
<div style="display: none; height: 0px; overflow: hidden;">
    <dxcp:ASPxCallbackPanel ID="cbpCloselogout" runat="server" Width="100%" ClientInstanceName="cbpCloselogout"
        ShowLoadingPanel="false" OnCallback="cbpCloselogout_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){                
                    hideLoadingPanel();
                    opener.location.reload(true); 
                    window.close();
                }" />
    </dxcp:ASPxCallbackPanel>
</div>
<!-- -->
