function QISClientIntellisenseTextBoxHelper() {
    var _self = this;
    this.isHoverImage = false;
    this.idx = 1;
    this.intellisenseHints = [];
    this.text = [];
    this.clientID = '';
    this.numIntellisenseType = 0;

    this.clientInstance = '';
    this.onSearchClick = '';
    this.setParam = function (clientInstance, onSearchClick) {
        _self.clientInstance = clientInstance;
        _self.onSearchClick = onSearchClick;
    }
    this.setIntellisenseHints = function (intellisenseHints) {
        _self.intellisenseHints = intellisenseHints;
        _self.numIntellisenseType = _self.intellisenseHints.length;

        var intellisenseHintText = '';
        _self.text = [];
        for (var i = 0; i < _self.numIntellisenseType; ++i) {
            if (intellisenseHintText != '')
                intellisenseHintText += ';';
            intellisenseHintText += _self.intellisenseHints[i].text;
            var text = intellisenseHintText;
            _self.text.push(text);
        }
        _self.idx = 1;
    }
    this.init = function (clientID, intellisenseHints) {
        _self.clientID = clientID;
        _self.intellisenseHints = intellisenseHints;
        _self.numIntellisenseType = _self.intellisenseHints.length;

        var intellisenseHintText = '';
        for (var i = 0; i < _self.numIntellisenseType; ++i) {
            if (intellisenseHintText != '')
                intellisenseHintText += ';';
            intellisenseHintText += _self.intellisenseHints[i].text;
            var text = intellisenseHintText;
            _self.text.push(text);
        }

        var width = $('#' + _self.clientID + '_divIntellisense .txtIntellisense').width() - 29;
        $('#' + _self.clientID + '_divIntellisense .txtIntellisense').width(width);

        _self.initializeControl();
    }
    this.initializeControl = function () {
        _self.setIntellisenseInfoText();
        _self.setTextBold();

        $('#' + _self.clientID + '_divIntellisense .txtIntellisense').keypress(function (e) {
//            var code = (e.keyCode ? e.keyCode : e.which);
//            if (code == 40 || code == 38) {
//                e.preventDefault();
//            }
        });

        $('#' + _self.clientID + '_divIntellisense .imgSearch').click(function (e) {
            _self.onSearchClick(_self.clientInstance);
        });

        $('#' + _self.clientID + '_divIntellisense .txtIntellisense').keyup(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 40) { //down
                _self.idx++;
                if (_self.idx > _self.numIntellisenseType)
                    _self.idx = 1;

                _self.setIntellisenseInfoText();
                _self.setTextBold();
            }
            else if (code == 38) { // up
                _self.idx--;
                if (_self.idx < 1)
                    _self.idx = _self.numIntellisenseType;

                _self.setIntellisenseInfoText();
                _self.setTextBold();
            }
            else if (code == 13) { // enter
                _self.onSearchClick(_self.clientInstance);
                e.preventDefault();
            }
            else {
                var numSeparatorCurrIdx = (_self.text[_self.idx - 1].split(";").length - 1);
                var numSeparatorText = ($(this).val().split(";").length - 1);

                if (numSeparatorText > numSeparatorCurrIdx) {
                    _self.idx++;
                    if (_self.idx > _self.numIntellisenseType)
                        _self.idx = _self.numIntellisenseType;
                }
                _self.setIntellisenseInfoText();
                _self.setTextBold();
            }
        });

        $('#' + _self.clientID + '_divIntellisense .txtIntellisense').focus(function () {
            $('#' + _self.clientID + '_divIntellisense .intellisenseBox').fadeIn(function () {
                $(this).css('display', 'inline-block');
            });

            $txt = $('#' + _self.clientID + '_divIntellisense .txtIntellisense');
            $txt.val($txt.attr('val'));
            $txt.removeClass('watermark');
            _self.setIntellisenseInfoText();
            _self.setTextBold();
        });
        $('#' + _self.clientID + '_divIntellisense .txtIntellisense').blur(function (e) {
            if (!_self.isHoverImage) {
                $('#' + _self.clientID + '_divIntellisense .intellisenseBox').fadeOut();

                $txt = $('#' + _self.clientID + '_divIntellisense .txtIntellisense');
                $txt.attr('val', $txt.val());
                setTimeout(function () { _self.setWatermark(); }, 0);
            }
        });

        $('#' + _self.clientID + '_divIntellisense .intellisenseContentInfo img').hover(function () {
            _self.isHoverImage = true;
        }, function () {
            _self.isHoverImage = false;
        });

        $('#' + _self.clientID + '_divIntellisense .imgDown').click(function () {
            _self.idx++;
            if (_self.idx > _self.numIntellisenseType)
                _self.idx = 1;

            _self.setIntellisenseInfoText();
            _self.setTextBold();

            $('#' + _self.clientID + '_divIntellisense .txtIntellisense').focus();
        });

        $('#' + _self.clientID + '_divIntellisense .imgUp').click(function () {
            _self.idx--;
            if (_self.idx < 1)
                _self.idx = _self.numIntellisenseType;

            _self.setIntellisenseInfoText();
            _self.setTextBold();

            $('#' + _self.clientID + '_divIntellisense .txtIntellisense').focus();
        });
        _self.setWatermark();
    }
    this.setWatermark = function () {
        $txt = $('#' + _self.clientID + '_divIntellisense .txtIntellisense');
        if ($txt.attr('val') == null)
            $txt.attr('val', '');
        if ($txt.attr('val') == '') {
            $txt.val($txt.attr('watermark'));
            $txt.addClass('watermark');
        }
        else {
            $txt.val($txt.attr('val'));
            $txt.removeClass('watermark');
        }
    }
    this.setIntellisenseInfoText = function () {
        var text = _self.idx + " of " + _self.numIntellisenseType;
        $('#' + _self.clientID + '_divIntellisense .intellisenseContentInfo .contentText').html(text);
    }
    this.setTextBold = function () {
        if (_self.numIntellisenseType > 0) {
            var textValue = $('#' + _self.clientID + '_divIntellisense .txtIntellisense').val();
            textValue = textValue.substring(0, _self.doGetCaretPosition());
            var numSeparator = (textValue.split(";").length - 1);

            var lstIntellisenseText = _self.text[_self.idx - 1].split(';');
            var intellisenseText = '';
            var intellisenseDescription = '';
            for (var i = 0; i < lstIntellisenseText.length; ++i) {
                if (intellisenseText != '')
                    intellisenseText += ';';

                if (i == numSeparator) {
                    intellisenseText += '<b>' + lstIntellisenseText[i] + '</b>';
                    if (_self.intellisenseHints[i].description != '')
                        intellisenseDescription = '<b>' + _self.intellisenseHints[i].text + '</b> : ' + _self.intellisenseHints[i].description;
                }
                else
                    intellisenseText += lstIntellisenseText[i];
            }
            $('#' + _self.clientID + '_divIntellisense .intellisenseContentText').html(intellisenseText);
            $('#' + _self.clientID + '_divIntellisense .intellisenseDescription').html(intellisenseDescription);
        }
    }

    this.doGetCaretPosition = function () {
        var oField = $('#' + _self.clientID + '_divIntellisense .txtIntellisense')[0];
        var iCaretPos = 0;

        try {
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
        }
        catch (err) { }
        // Return results
        return (iCaretPos);
    }
    this.doSetCaretPosition = function (iCaretPos) {
        var oField = $('#' + _self.clientID + '_divIntellisense .txtIntellisense')[0];
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
}

function QISClientIntellisenseTextBox() {
    var _self = this;
    this.clientID = '';
    this.intellisenseHints = [];
    this.init = function (clientID, intellisenseHints) {
        _self.clientID = clientID;
        _self.intellisenseHints = intellisenseHints;
    }
    this.setIntellisenseHints = function (intellisenseHints) {
        _self.intellisenseHints = intellisenseHints;
    }
    this.GetVal = function () {
        var textValue = $('#' + _self.clientID + '_divIntellisense .txtIntellisense').attr('val');
        return textValue;
    }
    this.GetText = function () {
        $txt = $('#' + _self.clientID + '_divIntellisense .txtIntellisense');
        if ($txt.hasClass('watermark'))
            return "";
        return $txt.val();
    }
    this.SetText = function (value) {
        $('#' + _self.clientID + '_divIntellisense .txtIntellisense').val(value);
        $('#' + _self.clientID + '_divIntellisense .txtIntellisense').attr('val', value);
        setTimeout(function () { _self.setWatermark(); }, 0);
    }
    this.setWatermark = function () {
        $txt = $('#' + _self.clientID + '_divIntellisense .txtIntellisense');
        if ($txt.attr('val') == null)
            $txt.attr('val', '');
        if ($txt.attr('val') == '') {
            $txt.val($txt.attr('watermark'));
            $txt.addClass('watermark');
        }
        else {
            $txt.val($txt.attr('val'));
            $txt.removeClass('watermark');
        }
    }
    this.SetWatermarkText = function ($val) {
        $txt = $('#' + _self.clientID + '_divIntellisense .txtIntellisense');
        $txt.attr('watermark', $val);
        setTimeout(function () { _self.setWatermark(); }, 0);
    }
    this.SetFocus = function () {
        $('#' + _self.clientID + '_divIntellisense .txtIntellisense').focus();
    }
    this.SetBlur = function () {
        $('#' + _self.clientID + '_divIntellisense .txtIntellisense').blur();
    }
    this.GenerateFilterExpression = function () {
        var textValue = $('#' + _self.clientID + '_divIntellisense .txtIntellisense').attr('val').split(';');
        var i = 0;
        var result = '';
        while (true) {
            if (i == textValue.length || i == _self.intellisenseHints.length)
                break;
            if (textValue[i] != '*') {
                if (result != '') {
                    if (textValue[i] != '') {
                        result += ' AND ';
                    }
                }
                var lstFieldName = _self.intellisenseHints[i].fieldName.split(' ');
                var tempFilterExpression = '';
                for (var k = 0; k < lstFieldName.length; ++k) {
                    if (tempFilterExpression != '')
                        tempFilterExpression += ' OR ';
                    if (textValue[i].trim() != '')
                        tempFilterExpression += lstFieldName[k] + " LIKE '%" + textValue[i].trim() + "%'";
                }
                if (tempFilterExpression != '') result += '(' + tempFilterExpression + ')';
            }
            i++;
        }
        return result;
    }
}