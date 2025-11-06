function customGridView() {
    this.gridID = '';
    this.hdnID = '';
    this.divID = '';
    this.cbp = null;
    this.pageID = '';
    var _self = this;
    this.init = function (gridID, hdnID, divID, cbp, pageID) {
        _self.gridID = gridID;
        _self.hdnID = hdnID;
        _self.divID = divID;
        _self.pageID = pageID;
        _self.cbp = cbp;

        $('#' + _self.gridID + ' > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
            $('#' + _self.gridID + ' > tbody > tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#' + _self.hdnID).val($(this).find('.keyField').html());
        });

        $('#' + _self.gridID + ' > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live("contextmenu", function (e) {
            if (e.button === 2) {
                e.preventDefault();
                $('#' + _self.gridID + ' > tbody > tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#' + _self.hdnID).val($(this).find('.keyField').html());

                showContextMenu($("#ctxMenuMPList"), e);
            }
        });

        $(window).blur(function () {
            $("#ctxMenuMPList").hide();
        });

        $(document).click(function (event) {
            $("#ctxMenuMPList").hide();
        });

        var id = $('#' + _self.hdnID).val();
        if (id != '') {
            $('#' + _self.gridID + ' > tbody > tr').each(function () {
                if ($(this).find('.keyField').html() == id)
                    $(this).click();
            });
        }
        else
            $('#' + _self.gridID + ' > tbody > tr:eq(1)').click();

        if ($.browser.mozilla) {
            $(document).keypress(_self.bodyKeyPress);
        } else {
            $(document).keydown(_self.bodyKeyPress);
        }
    }
    this.changeCursorUp = function () {
        $tr = $('#' + _self.gridID + ' tr.selected');
        if ($('#' + _self.gridID + ' tr').index($tr) > 1) {
            $tr.removeClass('selected');
            //$prevTr = $tr.prev();
            $prevTr = $tr.prevAll(':not(.trDetail):first');
            $prevTr.addClass('selected');
            $('#' + _self.hdnID).val($prevTr.find('.keyField').html());

            var position = $prevTr.position();
            var objDiv = $("#" + _self.divID)[0];
            var newScrollTop = objDiv.scrollTop;
            if (position.top < 1)
                newScrollTop -= $prevTr.height() - position.top;
            objDiv.scrollTop = newScrollTop;
        }
    }
    this.changeCursorDown = function () {
        $tr = $('#' + _self.gridID + ' tr.selected');
        if ($('#' + _self.gridID + ' > tbody > tr').index($tr) < $('#' + _self.gridID + ' > tbody > tr').length - 1) {
            $tr.removeClass('selected');
            //$nextTr = $tr.next();
            $nextTr = $tr.nextAll(':not(.trDetail):first');
            $nextTr.addClass('selected');
            $('#' + _self.hdnID).val($nextTr.find('.keyField').html());

            var position = $nextTr.position();
            var objDiv = $("#" + _self.divID)[0];
            var newScrollTop = objDiv.scrollTop;

            if ($("#" + _self.divID).height() <= position.top) {
                var diff = position.top - $("#" + _self.divID).height();
                newScrollTop += $nextTr.height() + diff;
            }
            objDiv.scrollTop = newScrollTop;
        }
    }
    this.changePage = function (idx) {
        var page = parseInt($('#' + _self.pageID).find('.jPag-current').html());
        if (idx < 0) {
            if (page > 1) {
                $('#' + _self.pageID).find('li:eq(' + (page - 2) + ')').click();
            }
        }
        else {
            if (page < $('#' + _self.pageID).find('li').length)
                $('#' + _self.pageID).find('li:eq(' + page + ')').click();
        }
    }
    this.bodyKeyPress = function (e) {
        if (e.target.tagName.toUpperCase() == 'INPUT') return;
        var charCode = (e.which) ? e.which : e.keyCode;
        switch (charCode) {
            case 38: if (!isLoadingPanelVisible()) _self.changeCursorUp(); break; //up
            case 40: if (!isLoadingPanelVisible()) _self.changeCursorDown(); break; //down
            case 33: if (!isLoadingPanelVisible()) _self.changePage(-1); break; //page down
            case 34: if (!isLoadingPanelVisible()) _self.changePage(1); break; //page up
            case 13: if (!isLoadingPanelVisible()) if (isAllowEditRecord()) cbpMPListProcess.PerformCallback('edit'); break;
        }
        /*if (e.ctrlKey) {
            switch (charCode) {
                case 13: if (!isLoadingPanelVisible()) if (isAllowEditRecord()) cbpMPListProcess.PerformCallback('edit'); break;
            }
        }*/
    }
}

function customGridView2() {
    this.gridID = '';
    this.hdnID = '';
    this.divID = '';
    this.cbp = null;
    this.pageID = '';
    var _self = this;
    this.init = function (gridID, hdnID, divID, cbp, pageID) {
        _self.gridID = gridID;
        _self.hdnID = hdnID;
        _self.divID = divID;
        _self.pageID = pageID;
        _self.cbp = cbp;

        $('.' + _self.gridID + ' > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
            if ($('.' + _self.gridID + ' > tbody > tr').index($(this)) > 1) {
                $('.' + _self.gridID + ' > tbody > tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#' + _self.hdnID).val($(this).find('.keyField').html());
            }
        });

        $('.' + _self.gridID + ' > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live("contextmenu", function (e) {
            if (e.button === 2) {
                e.preventDefault();
                if ($('.' + _self.gridID + ' > tbody > tr').index($(this)) > 1) {
                    $('.' + _self.gridID + ' > tbody > tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#' + _self.hdnID).val($(this).find('.keyField').html());
                    showContextMenu($("#ctxMenuMPList"), e);
                }
            }
        });

        $(window).blur(function () {
            $("#ctxMenuMPList").hide();
        });

        $(document).click(function (event) {
            $("#ctxMenuMPList").hide();
        });


        /*$('.' + _self.gridID + ' tr:gt(0):not(.trEmpty)').live('dblclick', function () {
        if (isAllowEditRecord())
        cbpMPListProcess.PerformCallback('edit');
        });*/
        var id = $('#' + _self.hdnID).val();
        if (id != '') {
            $('.' + _self.gridID + ' > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').each(function () {
                if ($(this).find('.keyField').html() == id)
                    $(this).click();
            });
        }
        else
            $('.' + _self.gridID + ' > tbody > tr:eq(2)').click();

        if ($.browser.mozilla) {
            $(document).keypress(_self.bodyKeyPress);
        } else {
            $(document).keydown(_self.bodyKeyPress);
        }
    }
    this.changeCursorUp = function () {
        $tr = $('.' + _self.gridID + ' tr.selected');
        if ($('.' + _self.gridID + ' tr').index($tr) > 2) {
            $tr.removeClass('selected');
            //$prevTr = $tr.prev();
            $prevTr = $tr.prevAll(':not(.trDetail):first');
            $prevTr.addClass('selected');
            $('#' + _self.hdnID).val($prevTr.find('.keyField').html());

            var position = $prevTr.position();
            var objDiv = $("#" + _self.divID)[0];
            var newScrollTop = objDiv.scrollTop;
            if (position.top < 1)
                newScrollTop -= $prevTr.height() - position.top;
            objDiv.scrollTop = newScrollTop;
        }
    }
    this.changeCursorDown = function () {
        $tr = $('.' + _self.gridID + ' tr.selected');
        if ($('.' + _self.gridID + ' > tbody > tr').index($tr) < $('.' + _self.gridID + ' > tbody > tr').length - 1) {
            $tr.removeClass('selected');
            //$nextTr = $tr.next();
            $nextTr = $tr.nextAll(':not(.trDetail):first');
            $nextTr.addClass('selected');
            $('#' + _self.hdnID).val($nextTr.find('.keyField').html());

            var position = $nextTr.position();
            var objDiv = $("#" + _self.divID)[0];
            var newScrollTop = objDiv.scrollTop;

            if ($("#" + _self.divID).height() <= position.top) {
                var diff = position.top - $("#" + _self.divID).height();
                newScrollTop += $nextTr.height() + diff;
            }
            objDiv.scrollTop = newScrollTop;
        }
    }
    this.changePage = function (idx) {
        var page = parseInt($('#' + _self.pageID).find('.jPag-current').html());
        if (idx < 0) {
            if (page > 1) {
                $('#' + _self.pageID).find('li:eq(' + (page - 2) + ')').click();
            }
        }
        else {
            if (page < $('#' + _self.pageID).find('li').length)
                $('#' + _self.pageID).find('li:eq(' + page + ')').click();
        }
    }
    this.bodyKeyPress = function (e) {
        if (e.target.tagName.toUpperCase() == 'INPUT') return;
        var charCode = (e.which) ? e.which : e.keyCode;
        switch (charCode) {
            case 38: if (!isLoadingPanelVisible()) _self.changeCursorUp(); break; //up
            case 40: if (!isLoadingPanelVisible()) _self.changeCursorDown(); break; //down
            case 33: if (!isLoadingPanelVisible()) _self.changePage(-1); break; //page down
            case 34: if (!isLoadingPanelVisible()) _self.changePage(1); break; //page up
            case 13: if (!isLoadingPanelVisible()) if (isAllowEditRecord()) cbpMPListProcess.PerformCallback('edit'); break;
        }
        /*if (e.ctrlKey) {
        switch (charCode) {
        case 13: if (!isLoadingPanelVisible()) if (isAllowEditRecord()) cbpMPListProcess.PerformCallback('edit'); break;
        }
        }*/
    }
}