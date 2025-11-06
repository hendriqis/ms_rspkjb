<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BodyDiagramAddCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.BodyDiagramAddCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/PatientPage/Objective/BodyDiagram/BodyDiagramTakePhotoCtl.ascx"
    TagName="BodyDiagramTakePhotoCtl" TagPrefix="uc2" %>

    
<script type="text/javascript" id="dxis_bodydiagramctl2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>'></script>
<script type="text/javascript" id="dxis_bodydiagramctl3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>'></script>
<script type="text/javascript" id="dxis_bodydiagramctl4" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.mouse.js")%>'></script>
<script type="text/javascript" id="dxis_bodydiagramctl5" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.draggable.js")%>'></script>
<script type="text/javascript" id="dxis_bodydiagramctl6" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.droppable.js")%>'></script>
<script type="text/javascript" id="dxis_bodydiagramctl7" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.effects.core.js")%>'></script>
<script type="text/javascript" id="dxis_bodydiagramctl8" src='<%= ResolveUrl("~/Libs/Scripts/jquery/scroll/jquery.ad-gallery.js")%>'></script>

<script type="text/javascript" id="dxss_dragctl1">
    //#region Deklarasi Variabel
    var galleries;
    var currentSymbol;   //menyimpan symbol yang baru saja didrop. Misal W1, B1, dst
    var currentSymbolEdit;   //menyimpan symbol yang sedang diedit.
    var $currentCloneSymbol; // Menyimpan symbol yang baru saja diclone
    var memNotesText;
    var animSpeed = 600; //ease amount
    var easeType = 'easeOutCirc'; //tipe animasi
    var ctrW = { val: 0 }; // Counter Symbol W
    var ctrB = { val: 0 }; // Counter Symbol B
    var ctrF = { val: 0 }; // Counter Symbol F
    var ctrC = { val: 0 }; // Counter Symbol C
    var ctrD = { val: 0 }; // Counter Symbol D
    var ctrS = { val: 0 }; // Counter Symbol S
    var totalContentHeight = 0; // Total Height daerah image yang sudah diolah
    var fadeSpeed = 200; // Speed Fading List Image
    var insideDropZone = false; // Menyimpan Apakah Saat Drop image di dalam Drop Zone Atau tidak. Jika tidak, image dihapus.
    var $appliedImageEdit = null; //Menyimpan Applied Image yang sedang diedit
    var isEditAppliedImage = false; //Menyimpan kondisi apakah sedang edit image yang sudah diapply
    var imgPreviewUrl = ''; // Menyimpan url image yang sedang diedit
    var imgEditUrl = '<%= ResolveUrl("~/Libs/Images/Button/edit.png")%>';
    var imgDeleteUrl = '<%= ResolveUrl("~/Libs/Images/Button/delete.png")%>';
    //#endregion

    //#region Init
    Init();

    function Init() {
        /// <summary>Initialisasi Variabel
        /// </summary>

        setDatePicker('<%=txtObservationDate.ClientID %>');
        $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

        /* Set Height Scroller Image Yang Sudah Diolah */
        sliderHeight = $('#tdAppliedImage').css('height').replace("px", "");
        $('#appliedScroller').css('height', sliderHeight);
        GetTotalContentHeight();

        /* Hide Semua Tombol Delete Pada Image Yang sudah Diolah */
        $(".removeimg").hide();

    }

    //#region Image Yang Sudah Diapply
    $('#appliedScroller').mousemove(OnAppliedScrollerMouseMove);
    function OnAppliedScrollerMouseMove(e) {
        if ($('#appliedScroller .container').height() > sliderHeight) {
            var mouseCoords = (e.pageY - this.offsetTop) - 50;
            var mousePercentY = mouseCoords / sliderHeight;
            var destY = -(((totalContentHeight - (sliderHeight)) - sliderHeight) * (mousePercentY));
            var thePosA = mouseCoords - destY;
            var thePosB = destY - mouseCoords;
            if (mouseCoords == destY) {
                $('#appliedScroller .container').stop();
            }
            else if (mouseCoords > destY) {
                //$('#thumbScroller .container').css('left',-thePosA); //without easing
                $('#appliedScroller .container').stop().animate({ top: -thePosA }, animSpeed, easeType); //with easing
            }
            else if (mouseCoords < destY) {
                //$('#thumbScroller .container').css('left',thePosB); //without easing
                $('#appliedScroller .container').stop().animate({ top: thePosB }, animSpeed, easeType); //with easing
            }
        }
    }

    //#region Fading Yang Sudah Diolah
    $('#appliedScroller  .thumb').each(function () {
        $(this).fadeTo(fadeSpeed, 0.6);
    });
    $('#appliedScroller .thumb').hover(
		function () { //mouse over
		    $(this).fadeTo(fadeSpeed, 1);
		},
		function () { //mouse out
		    $(this).fadeTo(fadeSpeed, 0.6);
		}
	);
    //#endregion

    //#region Tombol Delete Pada Image Yang Sudah Diolah
    $('#appliedScroller .content').hover(
        function () {  //this is fired when the mouse hovers over
            $(this).find('.removeimg').show();
        },
        function () {  //this is fired when the mouse hovers out
            $(this).find('.removeimg').hide();
        }
    );
    //#endregion
    //#endregion




    //#region Drag n Drop Symbol
    $('.ad-image-wrapper').droppable({
        drop: function (event, ui) {
            insideDropZone = true;
            var $clone = ui.helper.clone();

            var $newPosX = ui.offset.left - $(this).offset().left;
            var $newPosY = ui.offset.top - $(this).offset().top;

            $clone.css({
                top: $newPosY + "px",
                left: $newPosX + "px"
            })

            $currentCloneSymbol = $clone;
            if (!$clone.is('.insideDropZone')) {
                openPcNotes();
                $clone.addClass('insideDropZone');
                var symbolGroup = $clone.find('.symbolGroupValue').val();
                var ctr;
                switch (symbolGroup) {
                    case 'B': ctr = ctrB; break;
                    case 'W': ctr = ctrW; break;
                    case 'F': ctr = ctrF; break;
                    case 'C': ctr = ctrC; break;
                    case 'D': ctr = ctrD; break;
                    case 'S': ctr = ctrS; break;
                }
                ctr.val++;
                currentSymbol = symbolGroup + ctr.val;
                $symbolCode = $("<div class='symbolCode'>" + currentSymbol + "</div>");
                $clone.append($symbolCode);
                $(this).append($clone);
                $clone.draggable({
                    containment: '.container',
                    drag: function (event, ui) {
                        insideDropZone = false;
                    },
                    stop: function () {
                        if (!insideDropZone) {
                            GetTrNotesBySymbolCode($(this).find('.symbolCode').text()).remove();
                            $(this).remove();
                        }
                    }
                });
                $clone.dblclick(function () {
                    $tr = GetTrNotesBySymbolCode($(this).find('.symbolCode').text());
                    openPcNotes($tr.find("td").eq(2).html(), $tr.find("td").eq(6).html());
                });
            }
        }
    });
    //#endregion

    $('.drag').draggable({
        helper: 'clone',
        drag: function (event, ui) {
            insideDropZone = false;
        }
    });
    //#endregion

    //#region Image Yang Sudah Diapply
    function CreateAppliedImage(saveUrl, listImageSymbol, customSrc) {
        if (!isEditAppliedImage) {
            $content = $("<div class='content'><img src='" + saveUrl + "' class='thumb' /><input class='listImageSymbol' type='hidden' value='" + listImageSymbol + "' customsrc='" + customSrc + "'/><input type='button' value='X' class='removeimg' /></div>");
            $("#containerApplied").append($content);
            $thumb = $content.find('.thumb');
            $removeImg = $content.find('.removeimg');
            $removeImg.bind('click', OnDeleteAppliedImage);
            $removeImg.hide();
            $content.hover(
                function () {  //this is fired when the mouse hovers over
                    $(this).find('.removeimg').show();
                },
                function () {  //this is fired when the mouse hovers out
                    $(this).find('.removeimg').hide();
                });

            var fadeSpeed = 200;
            $thumb.fadeTo(fadeSpeed, 0.6);
            $thumb.hover(
		        function () { //mouse over
		            $(this).fadeTo(fadeSpeed, 1);
		        },
		        function () { //mouse out
		            $(this).fadeTo(fadeSpeed, 0.6);
		        }
	        );
            GetTotalContentHeight();
            $content.bind('click', OnClickAppliedImage);
        }
        else {
            $thumb = $appliedImageEdit.find('.thumb');
            $thumb.attr('src', saveUrl);
            $imageSymbol = $appliedImageEdit.find('.listImageSymbol');
            $imageSymbol.val(listImageSymbol);
        }
        ResetSymbolAndNotes();
    }
    function OnClickAppliedImage(e) {
        ResetSymbolAndNotes();
        $appliedImageEdit = $(this);
        isEditAppliedImage = true;

        $listImageSymbol = $(this).find('.listImageSymbol');
        var param = $listImageSymbol.val().split('|');
        if (parseInt(param[0]) > -1) {
            var found = false;
            var height = 0;
            var width = 0;
            $('.ad-thumb-list li').each(function () {
                if (!found && $(this).find('.bodyDiagramID').val() == param[0]) {
                    imgPreviewUrl = $(this).find('img').attr('src');
                    width = $(this).find('img').width();
                    height = $(this).find('img').height();
                    if (width > height) {
                        height = 300 * height / width;
                        width = 300;
                    }
                    else {
                        width = 300 * width / height;
                        height = 300;
                    }

                    found = true;
                }
            });
            var $imgPreview = $('.ad-image-wrapper').find('img').first();
            $imgPreview.attr('src', imgPreviewUrl);
            $parent = $imgPreview.parent();
            $parent.removeAttr('style');
            $imgPreview.width(width + 'px');
            $imgPreview.height(height + 'px');

            var left = (300 - width) / 2;
            var top = (300 - height) / 2;
            $parent.css('left', left + 'px');
            $parent.css('top', top + 'px');
        }
        else {
            var $imgPreview = $('.ad-image-wrapper').find('img').first();
            imgPreviewUrl = $listImageSymbol.attr('customsrc');
            $imgPreview.attr('src', imgPreviewUrl);
            $parent = $imgPreview.parent();
            $parent.removeAttr('style');

            var height = 0;
            var width = 0;
            width = $(this).find('img').width();
            height = $(this).find('img').height();
            if (width > height) {
                height = 300 * height / width;
                width = 300;
            }
            else {
                width = 300 * width / height;
                height = 300;
            }

            $imgPreview.width(width + 'px');
            $imgPreview.height(height + 'px');

            var left = (300 - width) / 2;
            var top = (300 - height) / 2;
            $parent.css('left', left + 'px');
            $parent.css('top', top + 'px');
        }

        for (var i = 1; i < param.length; ++i) {
            //format arr : id;left;top;symbolCode;notes
            var arr = param[i].split(';');

            var top = parseFloat(arr[2]) * $('.ad-image-wrapper').height() / 100;
            var left = parseFloat(arr[1]) * $('.ad-image-wrapper').width() / 100;

            var urlSymbol;
            var found = false;
            $('#<%=tblContainerSymbol.ClientID %> .drag').each(function () {
                if (!found && $(this).find('.symbolGroupValue').val() == arr[0]) {
                    urlSymbol = $(this).find('img').attr('src');
                    found = true;
                }
            });
            $clone = $("<div class='insideDropZone'><img src='" + urlSymbol + "'><div class='symbolCode'>" + arr[3] + "</div></div>");
            $clone.css({ "position": "absolute", "top": top + "px", "left": left + "px" });
            restoreTableNotesRow(arr[3], arr[4]);

            var ctr;
            switch (arr[3].substr(0, 1)) {
                case 'B': ctr = ctrB; break;
                case 'W': ctr = ctrW; break;
                case 'F': ctr = ctrF; break;
                case 'C': ctr = ctrC; break;
                case 'D': ctr = ctrD; break;
                case 'S': ctr = ctrS; break;
            }
            if (ctr.val < arr[3].substr(1))
                ctr.val = arr[3].substr(1);

            $('.ad-image-wrapper').append($clone.draggable({
                containment: '.container',
                drag: function (event, ui) {
                    insideDropZone = false;
                },
                stop: function () {
                    if (!insideDropZone) {
                        GetTrNotesBySymbolCode($(this).find('.symbolCode').text()).remove();
                        $(this).remove();
                    }
                }
            }));
            $clone.dblclick(function () {
                $tr = GetTrNotesBySymbolCode($(this).find('.symbolCode').text());
                openPcNotes($tr.find("td").eq(0).html(), $tr.find("td").eq(2).html());
            });
        }

        e.preventDefault();
    }
    function OnDeleteAppliedImage(e) {
        if (isEditAppliedImage) {
            if ($appliedImageEdit.find('.thumb').attr('src') == $(this).parent().find('.thumb').attr('src')) {
                var $imgPreview = $('.ad-image-wrapper').find('img');
                $imgPreview.attr('src', '');
                //document.getElementById('imgPreview').src = '';
                imgPreviewUrl = '';
                ResetSymbolAndNotes();
            }
        }

        $(this).parent().remove();
        GetTotalContentHeight();
        if ($('#appliedScroller .container').height() <= sliderHeight) {
            $('#appliedScroller .container').stop().animate({ top: 0 }, animSpeed, easeType);
        }
    }
    //#endregion    

    //#region Tombol Save, Reset, Apply
    $("#btnApply").click(OnApplyClick);
    $("#btnReset").click(OnResetClick);
    $("#btnSave").click(OnSaveClick);
    $("#btnCancel").click(OnCancelClick);
    function OnApplyClick() {
        var $imgPreview = $('.ad-image-wrapper').find('img');
        if ($imgPreview.attr('src') == '') {
            showToast('Warning', 'Please select an image!');
            return;
        }
        var bgImg = new Image;
        $temp = $('.ad-thumb-list').find('img[src="' + $imgPreview.attr('src') + '"]');
        if ($temp.length > 0) {
            bgImg.src = $temp.attr('urlbase64');
            bgImg.onload = function () {
                onApplyClickCreateImage(bgImg, $imgPreview);
            }
        }
        else {
            bgImg.src = $imgPreview.attr('src');
            onApplyClickCreateImage(bgImg, $imgPreview);
        }
    }

    function onApplyClickCreateImage(bgImg, $imgPreview) {
        var canvas = document.createElement('canvas');
        canvas.setAttribute('width', $imgPreview.width());
        canvas.setAttribute('height', $imgPreview.height());
        var ctx = canvas.getContext("2d");

        ctx.drawImage(bgImg, 0, 0, $imgPreview.width(), $imgPreview.height());

        imgPreviewUrl = $imgPreview.attr('src');
        var $item = $('.ad-thumb-list li img[src = "' + imgPreviewUrl + '"]');
        var idxImage = $item.parent().parent().find('.bodyDiagramID').val();
        var customsrc = '';
        if (idxImage === undefined) {
            idxImage = -1;
            customsrc = imgPreviewUrl;
        }
        var listImageSymbol = idxImage;

        //format listImageSymbol : "idxImgSrc|symbol1|symbol2|....;
        //format symbol-n : id;left;top;symbolCode;notes

        //loop untuk mengambil symbol
        $('.insideDropZone').each(function () {
            var left = parseFloat($(this).position().left);
            var top = parseFloat($(this).position().top);
            var width = $(this).width();
            var height = $(this).height();

            var img = new Image;
            var $elem = $(this).find('img');
            var $symbolCode = $(this).find('.symbolCode');
            img.src = $elem.attr("src");

            //find notes from symbol code
            var notes;
            var found = false;
            $('#tblNotes tr').each(function () {
                if (!found && $symbolCode.text() == $(this).find("td").eq(2).html()) {
                    notes = $(this).find("td").eq(6).html();
                    $(this).remove();
                    found = true;
                }
            });
            var $symbolSrc = $('#<%=tblContainerSymbol.ClientID %> .drag img[src = "' + $elem.attr("src") + '"]');
            var idSymbol = $symbolSrc.parent().find('.symbolGroupValue').val();

            var leftPercentage = left * 100 / $('.ad-image-wrapper').width();
            var topPercentage = top * 100 / $('.ad-image-wrapper').height();
            listImageSymbol += '|' + idSymbol + ';' + leftPercentage + ';' + topPercentage + ';' + $symbolCode.text() + ';' + notes;

            var leftImage = left - (($('.ad-image-wrapper').width() - $imgPreview.width()) / 2);
            var topImage = top - (($('.ad-image-wrapper').height() - $imgPreview.height()) / 2);

            ctx.drawImage(img, leftImage, topImage, width, height);
            //Draw Symbol
            ctx.font = "bold 10px Arial";
            ctx.textAlign = "center";
            ctx.fillText($symbolCode.text(), leftImage + (width / 2), topImage - 1);
            $(this).remove();
        });

        var saveUrl = canvas.toDataURL("image/png");
        $imgPreview.attr('src', '');
        imgPreviewUrl = '';
        CreateAppliedImage(saveUrl, listImageSymbol, customsrc);
    }
    function OnResetClick() {
        var $imgPreview = $('.ad-image-wrapper').find('img');
        $imgPreview.attr('src', '');
        imgPreviewUrl = '';
        ResetSymbolAndNotes();
    }
    function OnCancelClick() {
        pcRightPanelContent.Hide();
    }

    function dateToYMD(date) {
        var d = date.getDate();
        var m = date.getMonth() + 1;
        var y = date.getFullYear();
        return '' + y + '-' + (m <= 9 ? '0' + m : m) + '-' + (d <= 9 ? '0' + d : d);
    }

    function OnSaveClick() {
        if ($("#<%=txtObservationTime.ClientID %>").valid()) {
            showLoadingPanel();
            var observationTime = $("#<%=txtObservationTime.ClientID %>").val();
            var observationDate = dateToYMD(Methods.getDatePickerDate($("#<%=txtObservationDate.ClientID %>").val()));
            var paramedicID = cboParamedicID.GetValue();
            
            var success = true;
            var ctrProcess = 0;
            var idxImage = '<%=currRegistrationNumRows%>';
            var totalImage = $("#containerApplied .content").length;
            $("#containerApplied .content").each(function () {
                var image = $(this).find('.thumb').attr('src');
                var customSrc = $(this).find('.listImageSymbol').attr('customsrc');
                image = image.replace('data:image/png;base64,', '');
                image = image.replace('data:image/jpeg;base64,', '');
                image = image.replace('data:image/gif;base64,', '');
                customSrc = customSrc.replace('data:image/png;base64,', '');
                customSrc = customSrc.replace('data:image/jpeg;base64,', '');
                customSrc = customSrc.replace('data:image/gif;base64,', '');
                //customSrc = customSrc.replace('', '');
                var url = '<%= ResolveUrl("~/Libs/Service/UploadService.asmx/UploadBodyDiagram")%>';
                $.ajax({
                    async: false,
                    type: 'POST',
                    url: url,
                    data: '{ "observationDate" : "' + observationDate + '", "observationTime" : "' + observationTime + '", "imageData" : "' + image + '", "idx" : "' + idxImage++ + '", "listSymbol" : "' + $(this).find('.listImageSymbol').val() + '", "customSrc" : "' + customSrc + '", "paramedicID" : "' + paramedicID + '"}',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    error: function (msg) {
                        showToast('Warning', msg.responseText);
                        success = false;
                        if (ctrProcess == totalImage - 1) {
                            showToast('Failed!', '');
                            hideLoadingPanel();
                        }
                        ctrProcess++;
                    },
                    success: function (msg) {
                        if (ctrProcess == totalImage - 1) {
                            success = msg;
                            hideLoadingPanel();
                            if (!success)
                                showToast('Failed!', '');
                            else {
                                if (typeof onRefreshControl != 'undefined')
                                    onRefreshControl('');
                                closePcBodyDiagramNew();
                            }
                        }
                        ctrProcess++;
                    }
                });
            });
        }
    }
    //#endregion

    //#region Popup Notes
    $("#btnNotesOk").click(submitPcNotes);
    $("#btnNotesCancel").click(cancelPcNotes);
    function openPcNotes(symbolCode, text) {
        symbolCode = typeof symbolCode !== 'undefined' ? symbolCode : '';
        text = typeof text !== 'undefined' ? text : '';
        currentSymbolEdit = symbolCode;
        $("#<%=txtFillNotes.ClientID %>").val(text);
        pcNotes.SetHeaderText('Notes');
        pcNotes.Show();
        $("#<%=txtFillNotes.ClientID %>").focus();
    }
    function submitPcNotes() {
        memNotesText = $("#<%=txtFillNotes.ClientID %>").val();
        pcNotes.Hide();
        if (currentSymbolEdit == '')
            addTableNotesRow(memNotesText);
        else {
            GetTrNotesBySymbolCode(currentSymbolEdit).find("td").eq(6).text(memNotesText);
        }
    }
    function cancelPcNotes() {
        pcNotes.Hide();
        if (currentSymbolEdit == '') {
            var ctr;
            switch ($currentCloneSymbol.find('.symbolCode').html().substr(0, 1)) {
                case 'B': ctr = ctrB; break;
                case 'W': ctr = ctrW; break;
                case 'F': ctr = ctrF; break;
                case 'C': ctr = ctrC; break;
                case 'D': ctr = ctrD; break;
                case 'S': ctr = ctrS; break;
            }
            ctr.val--;
            $currentCloneSymbol.remove();
        }
    }
    //#endregion

    //#region Table Notes
    function restoreTableNotesRow(symbolCode, notes) {
        createTableNotesRow(symbolCode, notes);
    }

    function createTableNotesRow(symbolCode, notes) {
        var groupVal = symbolCode.substring(0, 1);

        var $item = $('#<%=tblContainerSymbol.ClientID %> .drag .symbolGroupValue[value = "' + groupVal + '"]').parent();
        $imgSymbol = $item.find('img');
        var title = $imgSymbol.attr('title');
        var imgSrc = $imgSymbol.attr('src');

        var tbl = document.getElementById('tblNotes');
        var row = document.createElement('tr');
        row.appendChild(createImgSymbol(imgSrc, title));
        row.appendChild(createTd(':'));
        row.appendChild(createTd(symbolCode));
        row.appendChild(createTd(':'));
        row.appendChild(createTd(title));
        row.appendChild(createTd(':'));
        row.appendChild(createTd(notes));
        row.appendChild(createBtnEdit());
        row.appendChild(createBtnDelete());
        tbl.appendChild(row);
    }

    function addTableNotesRow(notes) {
        createTableNotesRow(currentSymbol, notes);
    }
    function createTd(val) {
        var td = document.createElement('td');
        td.innerHTML = val;
        return td;
    }
    function OnEditNotes() {
        $tr = $(this).parent().parent();
        openPcNotes($tr.find("td").eq(2).html(), $tr.find("td").eq(6).html());
    }
    function OnDeleteNotes() {
        $tr = $(this).parent().parent();
        var symbolCode = $tr.find("td").eq(2).html();

        var found = false;
        $('.insideDropZone').each(function () {
            if (!found && symbolCode == $(this).find('.symbolCode').text()) {
                $(this).remove();
                found = true;
            }
        });

        $(this).parent().parent().remove();
    }
    function createImgSymbol(imgSrc, toolTip) {
        var td = document.createElement('td');
        var img = document.createElement('img');
        img.src = imgSrc;
        img.setAttribute('title', toolTip);
        img.setAttribute('style', 'height:16px;width:16px');
        td.appendChild(img);
        return td;
    }
    function createBtnEdit() {
        var td = document.createElement('td');
        var img = document.createElement('img');
        img.src = imgEditUrl;
        img.setAttribute('style', 'height:16px;width:16px');
        img.setAttribute('title', 'Edit');
        $(img).click(OnEditNotes);
        td.appendChild(img);
        return td;
    }
    function createBtnDelete() {
        var td = document.createElement('td');
        var img = document.createElement('img');
        img.src = imgDeleteUrl;
        img.setAttribute('title', 'Delete');
        img.setAttribute('style', 'height:16px;width:16px');
        $(img).click(OnDeleteNotes);
        td.appendChild(img);
        return td;
    }
    //#endregion

    //#region Utility Method
    function GetTrNotesBySymbolCode(symbolCode) {
        var $tr = null;
        var found = false;
        $('#tblNotes tr').each(function () {
            if (!found && symbolCode == $(this).find("td").eq(2).html()) {
                $tr = $(this);
                found = true;
            }
        });
        return $tr;
    }
    function GetTotalContentHeight() {
        totalContentHeight = 0;
        $('#appliedScroller .content').each(function () {
            totalContentHeight += $(this).innerHeight();
            totalContentHeight += 20;
            $('#appliedScroller .container').css('height', totalContentHeight);
        });
    }
    function ResetSymbolAndNotes() {
        $appliedImageEdit = null;
        isEditAppliedImage = false;
        $('.insideDropZone').remove();
        $('#tblNotes tr').remove();
        ctrB.val = 0;
        ctrW.val = 0;
        ctrF.val = 0;
        ctrC.val = 0;
        ctrD.val = 0;
        ctrS.val = 0;
    }
    //#endregion

    $(function () {
        galleries = $('.ad-gallery').adGallery();
        galleries[0].settings.effect = 'fade';
        filterImageThumbs('');
        setTimeout(function () {
            filterImageThumbs(null);
        }, 500);
    });

    function onCboImageGroupChanged(s) {
        filterImageThumbs(s.GetValue());
    }

    //#region filter Image Thumbs => dipanggil saat value combobox group berubah

    function filterImageThumbs(groupID) {
        var totalContentWidth = 0;
        $('.ad-thumb-list li').each(function () {
            if (groupID == null || groupID == '' || groupID == $(this).find('.imageGroupValue').val()) {
                $(this).show();
                totalContentWidth += $(this).innerWidth();
            }
            else
                $(this).hide();
        });

        galleries[0].thumbs_wrapper.scrollLeft(0);
        galleries[0]._setThumbListWidth(totalContentWidth);

        //var galleries = $('.ad-gallery').adGallery();
        //galleries[0].settings.effect = 'fade';
        //$('#thumbScroller .container').css('width', totalContentWidth);
        //$('#thumbScroller .container').stop().animate({ left: 0 }, animSpeed, easeType);
    }
    //#endregion

    //var imgSrc = '';
    function onPcTakePhotoHide() {
        if ($("#fc").is(":visible")) {
            Methods.getSessionValue("bodyDiagramUploadWebcam", function (result) {
                var $imgPreview = $('.ad-image-wrapper').find('img').first();
                $imgPreview.attr('src', "data:image/png;base64," + result);
                $parent = $imgPreview.parent();
                $parent.removeAttr('style');
                $imgPreview.width('282px');
                $imgPreview.height('300px');
                $parent.css('left', '9px');
                //$imgPreview.attr('style', 'border:1px solid red');
                //imgSrc = "data:image/gif;base64," + result;
            });
        }
        else {
            var src = $('#imgBrowseResult').attr('src');
            var $imgPreview = $('.ad-image-wrapper').find('img').first();
            $imgPreview.attr('src', src);
            $parent = $imgPreview.parent();
            $parent.removeAttr('style');

            var height = 0;
            var width = 0;
            width = $('#imgBrowseResult').width();
            height = $('#imgBrowseResult').height();
            if (width > height) {
                height = 300 * height / width;
                width = 300;
            }
            else {
                width = 300 * width / height;
                height = 300;
            }

            $imgPreview.width(width + 'px');
            $imgPreview.height(height + 'px');

            var left = (300 - width) / 2;
            var top = (300 - height) / 2;
            $parent.css('left', left + 'px');
            $parent.css('top', top + 'px');
        }
    }

    function closePcBodyDiagramNew() {
        pcRightPanelContent.Hide();
    }
</script>    
<style type="text/css">
    img.clone{
	    border:3px solid #fff !important;
	    height:100px;
    }
    #appliedOuterContainer
    {
        background-color:#555; 
        opacity: 0.65;
	    padding:0;
	    height:100%;
    }
    #appliedScroller
    {
        height:100%;
	    overflow:hidden;
	    position:relative;
    }
    #appliedScroller .container{
	    position:relative;
	    left:0;
    }
    #appliedScroller .content{
        vertical-align:middle;
        text-align:center;
        margin:2px 0;
        position:relative;
    }
    #appliedScroller .content .removeimg{
        position:absolute;
        top:0;
        right:0;
        background-color:transparent;
        border:0px;
        font-weight:bolder;
        font-size:20px;
        color:#FFF;
    }
    #appliedScroller img,
    img.clone{
	    border:3px solid #fff;
	    width:90%;
        margin:0 auto;
    }
    #appliedScroller a{
	    padding:2px;
	    outline:none;
    }
    
    .drag img, .insideDropZone img{
        height:24px;
    }
    .drag, .insideDropZone
    {
        cursor:pointer;
        z-index:1000;
    }
    .insideDropZone
    {
        position:relative;
    }
    .insideDropZone .symbolCode
    {
        position:absolute;
        top:-12px;
        width:100%;
        text-align:center;
        font-weight:bold;
        left:0;
        font-size: 11px;
    }
    .clear{clear:both;}
    
    #tblNotes {
        border-collapse:collapse;
        border:1px solid;
        position:relative; 
        width:100%;
    }
    
    #tblNotes tr td {
        vertical-align: top;
        text-align:left;
        padding:2px;
        border-bottom:1px solid;
        border-top:1px solid;
        font-size:12px;
    }
    #tblNotes img {
        cursor:pointer;
    }
    #tblNotes tr:nth-child(odd) {
	    background-color: #DDD; color: black;
    }
    #tblNotes tr:nth-child(even) {
	    background-color: #FFF; color: black;
    }    
    #tblNotes tr td:nth-child(5)  
    {
        font-weight:bolder;    
    }
    
    #gallery
    {
        padding-left: 30px;
    } 
    
    .contentLeft {
        float: left;
        padding: 0px 0px 0px 0px;
        width: 140px;
        border:1px solid #9C9898;
        height:460px;
    }

    .contentMiddle {
        margin-left: 140px;
        margin-right: 50px;
        border:1px solid #9C9898;
        padding-top: 10px;
        height:450px;
        width:1000px;
    }

    .contentRight {
        float: right;
        padding: 10px 10px 0px 10px;
        /*padding-top:10px;
        text-align:center;*/
        width: 30px;
        border:1px solid #9C9898;
        height:450px;
    }   
    .contentFooter {
        padding-top: 10px;
        text-align: center;
        clear: both;
        width:100%;
    }
    .container              { border:1px solid #9C9898; }
    .divBtnImage            { border: 1px solid black; }
</style>

<div id="toolbarArea">
    <table style="width:100%" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table id="Table2" runat="server" cellpadding="0">
                    <tr>
                        <td class="tdLabel"><%=GetLabel("Date") %> - <%=GetLabel("Time") %></td>
                        <td>
                            <table cellpadding="0" cellspacing="5">
                                <tr>
                                    <td style="padding-right: 1px;width:150px"><asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                                    <td><asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                                    <td style="width:100px">&nbsp;</td>
                                    <td class="tdLabel"><%=GetLabel("Physician") %></td>
                                    <td><dxe:ASPxComboBox ID="cboParamedicID" ClientInstanceName="cboParamedicID" Width="200px" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width:300px;text-align:right" valign="top">
                <table id="Table1" runat="server" cellpadding="0">
                    <tr>
                        <td class="tdLabel"><%=GetLabel("Body Diagram Group")%></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboImageGroup" Width="100%" runat="server">
                                <ClientSideEvents Init="function(s) { onCboImageGroupChanged(s); }" 
                                    ValueChanged="function(s) { onCboImageGroupChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>    
    
</div>
<div class="container" style="height:530px">
    <div class="contentLeft" id="tdAppliedImage">
        <div id="appliedOuterContainer">
			<div id="appliedScroller">
				<div class="container" id="containerApplied">       			                    
				</div>
			</div>
		</div>
    </div>
    <div class="contentRight">
        <table id="tblContainerSymbol" runat="server"></table>
    </div>
    <div class="contentMiddle">
        <div id="gallery" class="ad-gallery">              
            <div class="ad-image-wrapper boxShadow" style="float:left;position:relative;">           
            </div>
            <div class="containerNotes">
                <div style="font-size:large;font-weight:bolder;margin:10px;text-align:left">Remarks :</div>
                <div style="position:relative;overflow-y:scroll;height:280px;margin:10px;">
                    <table id="tblNotes" cellpadding="0" cellspacing="0">
                        <colgroup width="20px" />
                        <colgroup width="2px" />
                        <colgroup width="15px" />
                        <colgroup width="2px" />
                        <colgroup width="45px" />
                        <colgroup width="2px" />
                        <colgroup width="*" />
                        <colgroup width="16px" />
                        <colgroup width="16px" />
                    </table>
                </div>
            </div>
            <div class="ad-nav">
                <div class="ad-thumbs" style="background-color:#000;opacity: 0.65;margin-bottom:20px;">
                    <ul class="ad-thumb-list" runat="server" id="ulThumbs">
                    </ul>
                </div>
            </div>       
        </div>
    </div>    
    <div class="contentFooter">
        <table cellpadding="0" cellspacing="10" style="margin-left:auto;margin-right:auto;text-align:left">
            <tr>
                <td><input type="button" value='<%= GetLabel("Take Photo")%>' onclick="pcTakePhoto.Show();" /></td>
                <td><input type="button" value='<%= GetLabel("Reset")%>' id="btnReset" /></td>
                <td><input type="button" value='<%= GetLabel("Apply")%>' id="btnApply" /></td>
                <td><input type="button" value='<%= GetLabel("Save")%>' id="btnSave" /></td>
                <td><input type="button" value='<%= GetLabel("Cancel")%>' id="btnCancel" /></td>
            </tr>
        </table>
    </div>    
</div> 



<!-- Popup Entry Notes -->
<dx:ASPxPopupControl id="pcNotes" runat="server" clientinstancename="pcNotes"
    height="150px" HeaderText="Notes" CloseAction="None" width="350px" Modal="True" PopupAction="None" 
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseButtonImage-Width="0">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server" ID="pccc1">
            <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <div style="text-align:center;width:100%;">
                            <asp:TextBox ID="txtFillNotes" runat="server" Width="320px" Height="50px" TextMode="MultiLine" />
                            <table style="margin-left:auto;margin-right:auto;margin-top:5px;">
                                <tr>
                                    <td><input type="button" id="btnNotesOk" style="width:100px" value='<%= GetLabel("Ok")%>' /></td>
                                    <td><input type="button" id="btnNotesCancel" style="width:100px" value='<%= GetLabel("Cancel")%>' /></td>
                                </tr>
                            </table> 
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>


<dx:ASPxPopupControl ID="pcTakePhoto" runat="server" ClientInstanceName="pcTakePhoto"
    EnableHierarchyRecreation="True" FooterText="" HeaderText="Take Photo" HeaderStyle-HorizontalAlign="Left" Height="400px"
    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
    Width="300px" CloseAction="CloseButton" AllowDragging="true">
    <ClientSideEvents Shown="function (s,e) { initBodyDiagramTakePhoto(); }" 
        Closing="function(s,e){ onPcTakePhotoHide(); }"/>
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
            <div style="text-align: center; width: 100%;">
                <dxcp:ASPxCallbackPanel ID="cbpTakePhoto" runat="server" Width="100%" ClientInstanceName="cbpTakePhoto"
                    ShowLoadingPanelImage="true">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent111" runat="server">
                            <asp:Panel ID="Panel1" Style="width: 100%;" runat="server" Height="350px">
                                <uc2:BodyDiagramTakePhotoCtl ID="ctlTakePhotoCtl" runat="server" />
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>