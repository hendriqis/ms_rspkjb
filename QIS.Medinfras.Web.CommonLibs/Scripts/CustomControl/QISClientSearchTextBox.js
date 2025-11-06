function QISClientSearchTextBoxHelper() {
    var _self = this;
    this.timer = null;
    this.idxSelectedRow = 0;
    this.numRows = 0;
    this.clientID = '';
    this.valueText = '';
    this.displayText = '';
    this.columns = [];
    this.isAllowOtherValue = false;
    this.onLostFocus = '';
    this.onValueChanged = '';
    this.methodName = '';
    this.isHoverTable = '';
    this.filterExpression = '';
    this.clientInstance = '';
    this.ajax = null;
    this.isTxtChanged = false;
    this.init = function (clientID, columns) {
        _self.columns = columns;
        _self.clientID = clientID;

        //$("#" + _self.clientID + "_divAutoComplete .autoCompleteIntellisenseContentText").html(_self.intellisenseText);
        //alert($('.containerAutoComplete').find('script').attr('id'));
        _self.initializeControl();
        _self.setTextBold();
    }
    this.execInitHandler = function (f) {
        f(_self.clientInstance);
    }
    this.setParam = function (clientInstance, methodName, filterExpression, valueText, displayText, isAllowOtherValue, onLostFocus, onValueChanged) {
        _self.clientInstance = clientInstance;
        _self.valueText = valueText;
        _self.displayText = displayText;
        _self.onLostFocus = onLostFocus;
        _self.onValueChanged = onValueChanged;
        _self.methodName = methodName;
        _self.isAllowOtherValue = isAllowOtherValue;
        _self.filterExpression = filterExpression;
    }
    this.setTextBold = function () {
        var textValue = $('#' + _self.clientID + '_divAutoComplete .txtIntellisense').val();
        textValue = textValue.substring(0, _self.doGetCaretPosition());
        var numSeparator = (textValue.split(";").length - 1);

        var intellisenseText = '';
        var intellisenseDescription = '';
        for (var i = 0; i < _self.columns.length; ++i) {
            if (intellisenseText != '')
                intellisenseText += ';';

            if (i == numSeparator) {
                intellisenseText += '<b>' + _self.columns[i].text + '</b>';
                if (_self.columns[i].description != '')
                    intellisenseDescription = '<b>' + _self.columns[i].text + '</b> : ' + _self.columns[i].description;
            }
            else
                intellisenseText += _self.columns[i].text;
        }
        $('#' + _self.clientID + '_divAutoComplete .autoCompleteIntellisenseContentText').html(intellisenseText);
        $('#' + _self.clientID + '_divAutoComplete .intellisenseDescription').html(intellisenseDescription);
    }
    this.initializeControl = function () {
        var width = 0;
        $('#' + _self.clientID + '_divAutoComplete .tblAutoCompleteContent tr:eq(0)').find('th').each(function () {
            width += $(this).width();
        });
        $("#" + _self.clientID + "_divAutoComplete .containerAutoCompleteIntellisenseBox").width(width);


        $("#" + _self.clientID + "_divAutoComplete .txtIntellisense").keydown(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (_self.ajax != null) {
                _self.ajax.abort();
                _self.ajax = null;
            }
            if (code == 40) { //down
                if (_self.idxSelectedRow < _self.numRows)
                    _self.changeIdxSelectedRow(1);
                e.preventDefault();
            }
            else if (code == 38) { //up
                if (_self.idxSelectedRow > 1)
                    _self.changeIdxSelectedRow(-1);
                e.preventDefault();
            }
            else if (code == 13) { //enter
                if (_self.idxSelectedRow > 0) {
                    $row = $('#' + _self.clientID + '_divAutoComplete .tblAutoCompleteContent tr[class=selected]');
                    if ($row.length > 0)
                        _self.selectRow($row[0]);
                    else {
                        $('#' + _self.clientID + '_divAutoComplete .hdnValueText').val('');
                        $('#' + _self.clientID + '_divAutoComplete .hdnSearchText').val('');
                        $('#' + _self.clientID + '_divAutoComplete .hdnDisplayText').val('');
                        $('#' + _self.clientID + '_divAutoComplete .txtIntellisense').val('');
                    }
                    $('#' + _self.clientID + '_divAutoComplete .autoCompleteBox').fadeOut();

                    _self.setTextBold();
                }
                e.preventDefault();
            }
            else if (code == 9) { //tab
                if (_self.idxSelectedRow > 0) {
                    $row = $('#' + _self.clientID + '_divAutoComplete .tblAutoCompleteContent tr[class=selected]');
                    if ($row.length > 0)
                        _self.selectRow($row[0]);
                    else if ($("#" + _self.clientID + "_divAutoComplete .txtIntellisense").val() == '') {
                        $('#' + _self.clientID + '_divAutoComplete .hdnValueText').val('');
                        $('#' + _self.clientID + '_divAutoComplete .hdnSearchText').val('');
                        $('#' + _self.clientID + '_divAutoComplete .hdnDisplayText').val('');
                        $('#' + _self.clientID + '_divAutoComplete .txtIntellisense').val('');
                    }
                    $('#' + _self.clientID + '_divAutoComplete .autoCompleteBox').fadeOut();

                    _self.setTextBold();
                }
            }
        });

        $("#" + _self.clientID + "_divAutoComplete .txtIntellisense").keyup(function (e) {
            if (_self.timer) {
                clearTimeout(_self.timer);
            }
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 9 || (code === 9 && e.shiftKey)) {
            }
            else {
                if (code == 40 || code == 38 || code == 13) {
                }
                else {
                    _self.setTextBold();
                    if ($(this).val() != '')
                        _self.timer = setTimeout(_self.getAutoComplete, 400);
                    else {
                        $('#' + _self.clientID + '_divAutoComplete .autoCompleteBox').fadeOut();
                        $('#' + _self.clientID + '_divAutoComplete .tblAutoCompleteContent tr[class=selected]').removeClass('selected');
                    }
                }
            }
        });

        $("#" + _self.clientID + "_divAutoComplete .txtIntellisense").change(function () {
            _self.isTxtChanged = true;
        });

        var clicky;

        $(document).mousedown(function (e) {
            // The latest element clicked
            clicky = $(e.target);
        });

        // when 'clicky == null' on blur, we know it was not caused by a click
        // but maybe by pressing the tab key
        $(document).mouseup(function (e) {
            clicky = null;
        });

        $("#" + _self.clientID + "_divAutoComplete .txtIntellisense").focus(function () {
            $('#' + _self.clientID + '_divAutoComplete .tblAutoCompleteContent').find("tr:gt(0)").remove();
            $('#' + _self.clientID + '_divAutoComplete .autoCompleteBox').hide();
            $("#" + _self.clientID + "_divAutoComplete .containerAutoCompleteIntellisenseBox").show();
            $("#" + _self.clientID + "_divAutoComplete .autoCompleteIntellisenseBox").fadeIn();
            //if ($('#' + _self.clientID + '_divAutoComplete .hdnSearchText').val() != '' || !_self.isAllowOtherValue) {
            setTimeout(function () {
                $("#" + _self.clientID + "_divAutoComplete .txtIntellisense").val($('#' + _self.clientID + '_divAutoComplete .hdnSearchText').val());
            }, 0);
            //}
            _self.setTextBold();
        });

        $("#" + _self.clientID + "_divAutoComplete .txtIntellisense").live('blur', function () {
            if (clicky != null) {
                if (clicky.closest('table').attr('class') == 'tblAutoCompleteContent') {
                    if (clicky.closest('tr').is(':not(:first-child)')) {
                        _self.selectRow(clicky.closest('tr'));
                        $('#' + _self.clientID + '_divAutoComplete .autoCompleteBox').fadeOut();
                        _self.isHoverTable = false;
                    }
                }
            }
            _self.hidePanel();
        });

        this.hidePanel = function () {
            setTimeout(function () {
                if (!_self.isHoverTable) {
                    $("#" + _self.clientID + "_divAutoComplete .autoCompleteIntellisenseBox").fadeOut('fast', function () {
                        $("#" + _self.clientID + "_divAutoComplete .containerAutoCompleteIntellisenseBox").hide();
                    });
                }
                $row = $('#' + _self.clientID + '_divAutoComplete .tblAutoCompleteContent tr[class=selected]');
                if ($row.length < 1) {
                    if (!_self.isAllowOtherValue) {
                        if ($("#" + _self.clientID + "_divAutoComplete .txtIntellisense").val() == '') {
                            $('#' + _self.clientID + '_divAutoComplete .hdnValueText').val('');
                            $('#' + _self.clientID + '_divAutoComplete .hdnSearchText').val('');
                            $('#' + _self.clientID + '_divAutoComplete .hdnDisplayText').val('');
                            $('#' + _self.clientID + '_divAutoComplete .txtIntellisense').val('');
                            console.log('length 0');
                        }
                        else {
                            $("#" + _self.clientID + "_divAutoComplete .txtIntellisense").val($('#' + _self.clientID + '_divAutoComplete .hdnDisplayText').val());
                        }
                    }
                    else {
                        var val = $("#" + _self.clientID + "_divAutoComplete .txtIntellisense").val();
                        $('#' + _self.clientID + '_divAutoComplete .hdnValueText').val('');
                        $('#' + _self.clientID + '_divAutoComplete .hdnSearchText').val(val);
                        $('#' + _self.clientID + '_divAutoComplete .hdnDisplayText').val(val);
                    }
                }
                else { //if (!_self.isAllowOtherValue)
                    $("#" + _self.clientID + "_divAutoComplete .txtIntellisense").val($('#' + _self.clientID + '_divAutoComplete .hdnDisplayText').val());
                }
                $("#" + _self.clientID + "_divAutoComplete .autoCompleteBox").fadeOut();

                if (!_self.isHoverTable) {
                    if (_self.isTxtChanged) {
                        setTimeout(function () {
                            _self.onValueChanged(_self.clientInstance);
                        }, 0);
                    }
                }
                _self.onLostFocus(_self.clientInstance);
                _self.isHoverTable = false;
                _self.isTxtChanged = false;
            }, 0);
        }

        //        $('#' + _self.clientID + '_divAutoComplete .tblAutoCompleteContent').find("tr:gt(0)").live("click", function () {
        //            _self.selectRow(this);
        //            $("#" + _self.clientID + "_divAutoComplete .txtIntellisense").focus();
        //            _self.isHoverTable = false;
        //        });

        //        $('#' + _self.clientID + '_divAutoComplete .tblAutoCompleteContent').hover(function () {
        //            _self.isHoverTable = true;
        //        }, function () {
        //            _self.isHoverTable = false;
        //        });
    }

    this.changeIdxSelectedRow = function (val) {
        _self.idxSelectedRow += val;

        $("#" + _self.clientID + "_divAutoComplete .tblAutoCompleteContent").find("tr:gt(0)").attr('class', '');

        $selectedRow = $('#' + _self.clientID + '_divAutoComplete .tblAutoCompleteContent tr:eq(' + _self.idxSelectedRow + ')');
        var position = $selectedRow.position();
        $selectedRow.attr('class', 'selected');

        if (position != null) {
            var objDiv = $("#" + _self.clientID + "_divAutoComplete .autoCompleteContent")[0];
            var newScrollTop = objDiv.scrollTop;

            if ($("#" + _self.clientID + "_divAutoComplete .autoCompleteContent").height() <= position.top) {
                var diff = position.top - $("#" + _self.clientID + "_divAutoComplete .autoCompleteContent").height();
                newScrollTop += $selectedRow.height() + diff;
            }
            else if (position.top < 1)
                newScrollTop -= $selectedRow.height() - position.top;
            objDiv.scrollTop = newScrollTop;
        }
    }

    this.getAutoComplete = function () {
        $("#" + _self.clientID + "_divAutoComplete .imgLoadingAutoComplete").show();

        var filterExpression = _self.generateFilterExpression();
        if (filterExpression != '') {
            if (_self.filterExpression != '')
                filterExpression = _self.filterExpression + ' AND ' + filterExpression;
        }
        else
            filterExpression = _self.filterExpression;
        _self.getListObject(_self.methodName, filterExpression);
    }

    this.generateFilterExpression = function () {
        var result = '';
        if ($("#" + _self.clientID + "_divAutoComplete .txtIntellisense").val() != '') {
            var textValue = $("#" + _self.clientID + "_divAutoComplete .txtIntellisense").val().split(';');
            var i = 0;
            while (true) {
                if (i == textValue.length || i == _self.columns.length)
                    break;
                if (textValue[i] != '*') {
                    if (result != '')
                        result += ' AND ';
                    result += _self.columns[i].fieldName + " LIKE '%" + textValue[i] + "%'";
                }
                i++;
            }
        }
        return result;
    }

    this.getListObject = function (methodName, filterExpression) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: true,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetLimitListObject'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "methodName" : "' + methodName + '", "filterExpression" : "' + filterExpression + '", "pageCount" : "200" }',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                _self.onLoadObjectList(msg.d);
            }
        });     //end ajax
    };

    this.onLoadObjectList = function (result) {
        $('#' + _self.clientID + '_divAutoComplete .imgLoadingAutoComplete').hide();
        $('#' + _self.clientID + '_divAutoComplete .autoCompleteBox').fadeIn();

        $('#' + _self.clientID + '_divAutoComplete .tblAutoCompleteContent').find("tr:gt(0)").remove();
        $('#' + _self.clientID + '_tmplAutoComplete').tmpl(result).appendTo('#' + _self.clientID + '_divAutoComplete .tblAutoCompleteContent');
        _self.numRows = result.length;
        _self.idxSelectedRow = 0;
        var objDiv = $("#" + _self.clientID + "_divAutoComplete .autoCompleteContent")[0];
        objDiv.scrollTop = 0;
        _self.changeIdxSelectedRow(1);
    }

    this.selectRow = function (row) {
        var entity = $.tmplItem(row).data;

        var val = '';
        for (var i = 0; i < _self.columns.length; ++i) {
            if (val != '')
                val += ';';
            val += entity[_self.columns[i].fieldName];
        }

        $('#' + _self.clientID + '_divAutoComplete .hdnValueText').val(entity[_self.valueText]);
        $('#' + _self.clientID + '_divAutoComplete .hdnSearchText').val(val);
        $('#' + _self.clientID + '_divAutoComplete .hdnDisplayText').val(entity[_self.displayText]);

        $('#' + _self.clientID + '_divAutoComplete .txtIntellisense').val(val);

        _self.isTxtChanged = true;
    }

    this.doGetCaretPosition = function () {
        var oField = $('#' + _self.clientID + '_divAutoComplete .txtIntellisense')[0];
        var iCaretPos = 0;
        // IE Support
        if (document.selection) {
            oField.focus();
            var oSel = document.selection.createRange();
            oSel.moveStart('character', -oField.value.length);
            iCaretPos = oSel.text.length;
        }
        // Firefox support
        else if (oField.selectionStart || oField.selectionStart == '0')
            iCaretPos = oField.selectionStart;
        // Return results
        return (iCaretPos);
    }
    this.doSetCaretPosition = function (iCaretPos) {
        var oField = $('#' + _self.clientID + '_divAutoComplete .txtIntellisense')[0];
        // IE Support
        if (document.selection) {
            oField.focus();
            var oSel = document.selection.createRange();
            oSel.moveStart('character', -oField.value.length);
            oSel.moveStart('character', iCaretPos);
            oSel.moveEnd('character', 0);
            oSel.select();
        }

        // Firefox support
        else if (oField.selectionStart || oField.selectionStart == '0') {
            oField.selectionStart = iCaretPos;
            oField.selectionEnd = iCaretPos;
            oField.focus();
        }
    }


    this.setValue = function (value) {
        if (value != '') {
            var filterExpression = _self.valueText + " LIKE '" + value + "'";
            _self.getObject(_self.methodName, filterExpression);
        }
        else {
            $('#' + _self.clientID + '_divAutoComplete .hdnValueText').val('');
            $('#' + _self.clientID + '_divAutoComplete .hdnSearchText').val('');
            $('#' + _self.clientID + '_divAutoComplete .hdnDisplayText').val('');
            $('#' + _self.clientID + '_divAutoComplete .txtIntellisense').val('');
        }
    }
    this.getText = function () {
        return $('#' + _self.clientID + '_divIntellisense .txtIntellisense').val();
    }
    this.setText = function (value) {
        $('#' + _self.clientID + '_divAutoComplete .txtIntellisense').val(value);
        if (_self.isAllowOtherValue) {            
            $('#' + _self.clientID + '_divAutoComplete .hdnValueText').val('');
            $('#' + _self.clientID + '_divAutoComplete .hdnSearchText').val(value);
            $('#' + _self.clientID + '_divAutoComplete .hdnDisplayText').val(value);
        }
    }
    this.setFocus = function () {
        $("#" + _self.clientID + "_divAutoComplete .txtIntellisense").focus();
    }

    this.getObject = function (methodName, filterExpression) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetObject'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "methodName" : "' + methodName + '", "filterExpression" : "' + filterExpression + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                _self.onLoadObject(msg.d);
            }
        });     //end ajax
    };
    this.onLoadObject = function (result) {
        if (result == null) {
            $('#' + _self.clientID + '_divAutoComplete .hdnValueText').val('');
            $('#' + _self.clientID + '_divAutoComplete .hdnSearchText').val('');
            $('#' + _self.clientID + '_divAutoComplete .hdnDisplayText').val('');
            $('#' + _self.clientID + '_divAutoComplete .txtIntellisense').val('');
        }
        else {
            var val = '';
            for (var i = 0; i < _self.columns.length; ++i) {
                if (val != '')
                    val += ';';
                val += result[_self.columns[i].fieldName];
            }

            $('#' + _self.clientID + '_divAutoComplete .hdnValueText').val(result[_self.valueText]);
            $('#' + _self.clientID + '_divAutoComplete .hdnSearchText').val(val);
            var displayText = result[_self.displayText];
            $('#' + _self.clientID + '_divAutoComplete .hdnDisplayText').val(displayText);
            $('#' + _self.clientID + '_divAutoComplete .txtIntellisense').val(displayText);
        }
    }
}

function QISClientSearchTextBox() {
    var _self = this;
    this.clientID = '';
    this.valueText = '';
    this.displayText = '';
    this.methodName = '';
    this.helper = null;
    this.columns = [];
    this.init = function (helper, clientID) {
        _self.helper = helper;
        _self.clientID = clientID;
    }
    this.GetText = function () {
        return _self.helper.getText();
    }
    this.SetText = function (value) {
        _self.helper.setText(value);
    }
    this.SetFocus = function () {
        _self.helper.setFocus();
    }
    this.SetValue = function (value) {
        _self.helper.setValue(value);
    }
    this.GetValueText = function () {
        return $('#' + _self.clientID + '_divAutoComplete .hdnValueText').val();
    }
    this.GetDisplayText = function () {
        return $('#' + _self.clientID + '_divAutoComplete .hdnDisplayText').val();
    }
    this.GetSearchText = function () {
        return $('#' + _self.clientID + '_divAutoComplete .hdnSearchText').val();
    }
    this.SetValueText = function (value) {
        $('#' + _self.clientID + '_divAutoComplete .hdnValueText').val(value);
    }
    this.ClearText = function () {
        $('#' + _self.clientID + '_divAutoComplete .hdnValueText').val('');
        $('#' + _self.clientID + '_divAutoComplete .hdnSearchText').val('');
        $('#' + _self.clientID + '_divAutoComplete .hdnDisplayText').val('');
        $('#' + _self.clientID + '_divAutoComplete .txtIntellisense').val('');
    }
    this.SetDisplayText = function (value) {
        $('#' + _self.clientID + '_divAutoComplete .hdnDisplayText').val(value);
    }
    this.SetSearchText = function (value) {
        $('#' + _self.clientID + '_divAutoComplete .hdnSearchText').val(value);
    }
    this.SetFilterExpression = function (filterExpression) {
        _self.helper.filterExpression = filterExpression;
    }
    this.Validate = function () {
        if ($('#' + _self.clientID + '_divAutoComplete .hdnValueText').val() == '') {
            $("#" + _self.clientID + "_divAutoComplete").find('.tblContainerTextBox').addClass('error');
            return false;
        }
        else {
            $("#" + _self.clientID + "_divAutoComplete").find('.tblContainerTextBox').removeClass('error');
            return true;
        }
    }
}