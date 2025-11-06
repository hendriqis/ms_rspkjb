function restrictCharacters(myfield, e, inputType) {
    if (!e) var e = window.event
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    var character = String.fromCharCode(code);

    if (!e.ctrlKey && code != 9 && code != 8 && code != 36 && code != 37 && code != 38 && (code != 39 || (code == 39 && character == "'")) && code != 40) {
        var re;
        switch (inputType) {
            case 'amount': re = new RegExp("[0-9]+(\.[0-9][0-9]?)?"); break;
            case 'qty': re = new RegExp("((([1-9](\d{1,7})?))(\.\d{1,2})?)|(0(\.\d{1,2})?)"); break;
            case 'money': re = new RegExp("((([1-9](\d{1,7})?))(\.\d{1,2})?)|(0(\.\d{1,2})?)"); break;
            case 'float': re = new RegExp("[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?"); break;
            case 'percentage': re = new RegExp("((([1-9](\d{1,7})?))(\.\d{1,2})?)|(0(\.\d{1,2})?)"); break;
            //case 'percentage': new RegExp("^([1-9]+\d*|[0])\.?(\d*)?$"); break;                                                   
            //case 'amount': re = "^([1-9]+\d*|[0])\.?(\d*)?$"; break;                                                  
        }

        var text = myfield.value;
        text += character;
        if (text.match(re))
            return true;
        return false;
    }
    return true;
}

Number.prototype.formatMoney = function (c, d, t) {
    var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

function InlineEditing() {
    this.gridID;
    this.isAddRowFromEnter = false;
    this.listParam;
    this.isChanged = false;
    this.oldEditedValue;
    this.imgDeleteUrl = ResolveUrl('~/Libs/Images/Button/delete.png');
    this.cell;
    this.onLblClickHandler = '';
    this.onTxtValueChangedHandler = '';
    this.onLedValueChangedHandler = '';
    this.onBteValueChangedHandler = '';
    this.onBteButtonClickHandler = '';
    this.onIsChangedChangeHandler = '';
    this.onRowDeletedHandler = '';
    this.onCboValueChangedHandler = '';
    this.onCboFocusHandler = '';
    this.onAddRowHandler = '';
    this.onCreateFooterRowHandler = '';
    this.validationGroup = '';
    var _self = this;
    this.init = function (gridID, listParam) {
        _self.gridID = gridID;
        _self.listParam = listParam;
    }
    this.setValidationGroup = function (validationGroup) {
        _self.validationGroup = validationGroup;
    }
    this.setOnAddRowHandler = function (value) {
        _self.onAddRowHandler = value;
    }
    this.setOnLblClickHandler = function (value) {
        _self.onLblClickHandler = value;
    }
    this.setOnLedValueChangedHandler = function (value) {
        _self.onLedValueChangedHandler = value;
    }
    this.setOnRowDeletedHandler = function (value) {
        _self.onRowDeletedHandler = value;
    }
    this.setOnTxtValueChangedHandler = function (value) {
        _self.onTxtValueChangedHandler = value;
    }
    this.setOnCboFocusHandler = function (value) {
        _self.onCboFocusHandler = value;
    }
    this.setOnCboValueChangedHandler = function (value) {
        _self.onCboValueChangedHandler = value;
    }
    this.setOnBteValueChangedHandler = function (value) {
        _self.onBteValueChangedHandler = value;
    }
    this.setOnBteButtonClickHandler = function (value) {
        _self.onBteButtonClickHandler = value;
    }
    this.setOnIsChangedChangeHandler = function (value) {
        _self.onIsChangedChangeHandler = value;
    }

    //#region ASPxComboBox
    this.addCboItem = function (aspxCbo, text, value) {
        var count = aspxCbo.GetItemCount();
        var isAdd = false;
        for (var i = 0; i < count; ++i) {
            var item = aspxCbo.GetItem(i);
            if (text < item.text) {
                aspxCbo.InsertItem(i, text, value);
                isAdd = true;
                break;
            }
        }
        if (!isAdd)
            aspxCbo.AddItem(text, value);
    }

    this.showAspxComboBox = function (event) {
        var isShowDropDown = event.data.isShowDropDown;
        if (isShowDropDown)
            $cboCell = $(this).parent().closest('div');
        else
            $cboCell = $(this).closest('td').parent().closest('div');

        $shownCbo = $cboCell.find('table').first();

        var isEnabled = $shownCbo.find(".isEnabled").val();
        if (isEnabled == '1') {
            var isUnique = $shownCbo.find(".isUnique").val();
            aspxCboID = $shownCbo.find(".aspxCboID").val();

            $shownCbo.attr('style', 'display:none;');
            $('#' + aspxCboID).attr('style', 'height:24px;border-collapse:collapse;border-collapse:collapse;width:100%;display:visible;');

            $cboCell.append($('#' + aspxCboID));
            $('#' + aspxCboID).find("input:eq(1)").focus();

            var aspxCbo = ASPxClientControl.GetControlCollection().GetByName(aspxCboID);

            var text = $shownCbo.find('input:eq(1)').val();
            var value = $shownCbo.find('input').first().val();
            if (isUnique == '1') {
                if (text != "")
                    _self.addCboItem(aspxCbo, text, value);
            }

            aspxCbo.SetValue(value);
            aspxCbo.SetVisible(true);

            _self.oldEditedValue = aspxCbo.GetValue();

            if (isShowDropDown)
                aspxCbo.ShowDropDown();

            var className = $shownCbo.find('input:eq(1)').attr('class').split(' ')[2];
            $row = $cboCell.closest('tr');
            if (typeof _self.onCboFocusHandler == 'function')
                _self.onCboFocusHandler($row, className);
        }
    }

    this.hideAspxComboBox = function (aspxCbo) {
        var aspxCboValue = aspxCbo.GetValue();
        $shownCbo = $cboCell.find('table').first();
        $shownCbo.attr('style', 'height:24px;border-collapse:collapse;border-collapse:collapse;width:100%;display:visible;');

        $shownCbo.find('input').first().val(aspxCbo.GetValue());
        $shownCbo.find('input:eq(1)').val(aspxCbo.GetText());

        aspxCboID = $shownCbo.find(".aspxCboID").val();

        var className = $shownCbo.find('input:eq(1)').attr('class').split(' ')[2];

        $tbl = $('#' + _self.gridID);

        var rowCount = $tbl.find(".trTransaction").length;
        var $row = $cboCell.closest('tr');
        if ($row.html() == $tbl.find(".trTransaction").last().html() && aspxCboValue != null) {
            _self.addRow();
        }

        var isUnique = $shownCbo.find(".isUnique").val();
        if (isUnique == '1') {
            var selectedIndex = aspxCbo.GetSelectedIndex();
            aspxCbo.RemoveItem(selectedIndex);
        }
        aspxCbo.SetSelectedIndex(-1);

        $('#' + aspxCboID).attr('style', 'display:none;');
        $('#containerCbo').append($('#' + aspxCboID));

        if (aspxCboValue != null)
            if (_self.oldEditedValue != aspxCboValue) {
                if (typeof _self.onIsChangedChangeHandler == 'function')
                    _self.onIsChangedChangeHandler(true);
                _self.isChanged = true;
                _self.setRowChanged($row, true);

                if (typeof _self.onCboValueChangedHandler == 'function')
                    _self.onCboValueChangedHandler($row, className, _self.oldEditedValue, aspxCboValue);
            }
    }
    //#endregion
    //#region QISSearchTextBox
    this.showQisSearchTextBox = function () {
        var ctl = this;
        setTimeout(function () {
            $ledCell = $(ctl).closest('td');
            $led = $ledCell.find('div').first();
            var isEnabled = $led.find(".isEnabled").val();

            if (isEnabled == '1') {
                var qisLedID = $led.find(".qisLedID").val();

                $led.find('input[type=text]').first().hide();
                $('#' + qisLedID).attr('style', 'display:block;');
                $('#' + qisLedID).find('.containerAutoComplete').attr('style', 'height:24px;border-collapse:collapse;border-collapse:collapse;width:100%;display:visible;');

                $led.append($('#' + qisLedID));

                _self.oldEditedValue = $led.find(".hiddenValue").val();
                var clientInstanceName = $('#' + qisLedID).find(".hdnClientInstanceName").val();

                eval("var qisLed = " + clientInstanceName);
                qisLed.SetValueText(_self.oldEditedValue);
                qisLed.SetDisplayText($led.find(".hiddenDisplay").val());
                qisLed.SetSearchText($led.find(".hiddenSearch").val());

                $('#' + qisLedID).find("input").focus();
            }
        }, 0);
    }

    this.hideQisSearchTextBox = function (qisLed) {
        $led = $ledCell.find('div').first();
        $led.find('input[type=text]').first().show();

        var valueText = qisLed.GetValueText();
        var displayText = qisLed.GetDisplayText();
        var searchText = qisLed.GetSearchText();

        $led.find(".hiddenDisplay").val(displayText)
        $led.find(".hiddenValue").val(valueText);
        $led.find(".hiddenSearch").val(searchText);
        $led.find('input[type=text]').val(displayText);

        qisLedID = $ledCell.find(".qisLedID").val();
        var className = $led.find('input:eq(1)').attr('class').split(' ')[2];
        $tbl = $('#' + _self.gridID);

        var rowCount = $tbl.find(".trTransaction").length;
        var $row = $ledCell.parent();
        if ($row.html() == $tbl.find(".trTransaction").last().html() && valueText != null) {
            _self.addRow();
        }

        setTimeout(function () {
            qisLed.ClearText('');
        }, 0);
        $('#' + qisLedID).attr('style', 'display:none;');
        $('#containerCbo').append($('#' + qisLedID));

        var $row = $ledCell.parent();
        if (_self.oldEditedValue != valueText) {
            if (typeof _self.onIsChangedChangeHandler == 'function')
                _self.onIsChangedChangeHandler(true);
            _self.isChanged = true;
            _self.setRowChanged($row, true);

            if (typeof _self.onLedValueChangedHandler == 'function')
                _self.onLedValueChangedHandler($row, className, _self.oldEditedValue, valueText);
        }
    }
    //#endregion

    //#region BtnDelete
    this.deleteRow = function () {
        var numRow = $('#' + _self.gridID).find('.trTransaction').length;
        $deletedTr = $(this).closest('tr');
        if (_self.getRowEnabled($deletedTr)) {
            if (parseInt(numRow) > 1) {
                if (confirm('Are You Sure?')) {
                    var objDeleted = {};
                    var ctr = -1;
                    objDeleted['key'] = $deletedTr.find('.keyValue').find('input').val();
                    $deletedTr.children().each(function () {
                        if (ctr > -1 && ctr < _self.listParam.length) {
                            $txt = $(this).find('input').val();
                            var inputType = _self.listParam[ctr].type;
                            var className = _self.listParam[ctr].className;
                            objDeleted[className] = $txt;
                        }
                        ctr++;
                    });
                    _self.removeRow($deletedTr);
                    if (typeof _self.onIsChangedChangeHandler == 'function')
                        _self.onIsChangedChangeHandler(true);
                    if (typeof _self.onRowDeletedHandler == 'function')
                        _self.onRowDeletedHandler(objDeleted);
                }
            }
        }
    }

    this.createButtonDelete = function () {
        $img = $("<img></img>").attr('src', _self.imgDeleteUrl).attr('style', 'width:24px;cursor:pointer;margin:auto;');
        $img.click(_self.deleteRow);
        return $img;
    }
    //#endregion
    //#region TextBox
    this.createTextBox = function (inputType, className, isEnabled, value, isRequired) {
        $div = $('<div></div>').attr('style', 'width:100%;padding: 2px 5px;').addClass('borderBox');
        $txt = $("<input type='text'></input").attr('style', 'width:100%;padding:2px 1px 3px 1px;').addClass('borderBox ' + className);
        if (!isEnabled)
            $txt.attr('readonly', 'readonly');
        if (inputType != '') {
            $txt.css('text-align', 'right');
            $txt.addClass('number');
            /*$txt.keypress(function (event) {
            return restrictCharacters(this, event, "' + inputType + '")
            });*/
        }
        $txt.blur(function () {
            if (!_self.isAddRowFromEnter) {
                if ($(this).closest('tr').html() == $('#' + _self.gridID).find(".trTransaction").last().html() && $(this).val() != '') {
                    _self.addRow();
                }
            }
            _self.isAddRowFromEnter = false;
            var $val = $(this).val();
            $row = $(this).closest('tr');
            $txtHiddenValue = $(this).parent().find('.hiddenValue');
            $txtHiddenValue.val($val);
            if ($val != '') {
                var $inputType = $(this).parent().find('.inputType').val();
                if ($inputType == 'money')
                    $val = parseFloat($val).formatMoney(2, '.', ',');
                else if ($inputType == 'qty')
                    $val = parseFloat($val).formatMoney(0, '.', ',');
                else if ($inputType == 'percentage')
                    $val = $val + '%';
                $(this).val($val);
                if (_self.oldEditedValue != $val) {
                    if (typeof _self.onIsChangedChangeHandler == 'function')
                        _self.onIsChangedChangeHandler(true);
                    _self.isChanged = true;
                    _self.setRowChanged($row, true);
                    if (typeof _self.onTxtValueChangedHandler == 'function') {
                        _self.onTxtValueChangedHandler($row, className, _self.oldEditedValue, $txtHiddenValue.val());
                    }
                }
            }
        });
        $txt.focus(function () {
            _self.oldEditedValue = $(this).val();
            $value = $(this).parent().find('.hiddenValue').val();
            $(this).val($value);
        });
        $txt.keydown(_self.onTxtKeyDown);
        $txt.attr('validationgroup', _self.validationGroup);
        $txtHiddenValue = $("<input type='hidden' class='hiddenValue'></input>");
        $txtInputType = $("<input type='hidden' class='inputType'></input>").val(inputType);
        $txtIsRequired = $("<input type='hidden' class='isRequired'></input>");
        if (isRequired)
            $txtIsRequired.val('1');
        else
            $txtIsRequired.val('0');

        $div.append($txt);
        $div.append($txtHiddenValue);
        $div.append($txtInputType);
        $div.append($txtIsRequired);

        if (value != '') {
            $txtHiddenValue.val(value);

            $displayValue = value;

            if (inputType == 'money')
                $displayValue = parseFloat(value).formatMoney(2, '.', ',');
            else if (inputType == 'qty')
                $displayValue = parseFloat(value).formatMoney(0, '.', ',');
            else if (inputType == 'percentage')
                $displayValue = value + '%';
            $txt.val($displayValue);
        }

        return $div;
    }

    this.getTextBoxValue = function ($row, txtClass) {
        return $row.find('.' + txtClass).parent().find('.hiddenValue').val();
    }

    this.setTextBoxProperties = function ($row, txtClass, options) {
        $txt = $row.find('.' + txtClass);
        if (options.isEnabled != null) {
            if (options.isEnabled) {
                $txt.removeAttr('readonly');
            }
            else {
                $txt.attr('readonly', 'readonly');
            }
        }
        if (options.value != null) {
            $txt = $row.find('.' + txtClass);
            $txtHiddenValue = $txt.parent().find('.hiddenValue');
            $txtHiddenValue.val(options.value);

            $displayValue = options.value;

            var $inputType = $txt.parent().find('.inputType').val();
            if ($inputType == 'money')
                $displayValue = parseFloat(options.value).formatMoney(2, '.', ',');
            else if ($inputType == 'qty')
                $displayValue = parseFloat(options.value).formatMoney(0, '.', ',');
            else if ($inputType == 'percentage')
                $displayValue = options.value + '%';
            $txt.val($displayValue);

        }
    }

    this.getColumnTotal = function (className) {
        var sum = 0;
        $('#' + _self.gridID).find('.' + className).each(function () {
            var val = $(this).parent().find('.hiddenValue').val();
            if (val != "")
                sum += parseFloat(val);
        });
        return sum;
    }
    //#endregion
    //#region ComboBox
    this.createComboBox = function (cboID, className, isUnique, isEnabled, value, isRequired) {
        var styleDisabled = '';
        if (!isEnabled)
            styleDisabled = 'background-color:WhiteSmoke;';
        var tbl = document.createElement("table");

        tbl.setAttribute('class', 'dxeButtonEdit');
        tbl.setAttribute('cellspacing', '1');
        tbl.setAttribute('cellpadding', '0');
        tbl.setAttribute('border', '0');
        tbl.setAttribute('style', 'height:24px;border-collapse:collapse;border-collapse:collapse;width:100%;display:visible;' + styleDisabled);

        var tr = document.createElement("tr");
        var cell = document.createElement("td");
        var cell2 = document.createElement("td");
        cell.setAttribute('class', 'dxic');
        cell.setAttribute('style', 'width:100%;');
        if (isEnabled) {
            cell2.setAttribute('class', 'dxeButtonEditButton');
            cell2.setAttribute('style', '-moz-user-select:none;padding:0 2px;border-left:1px solid #AAA;');
        }
        else
            cell2.setAttribute('style', 'padding:0 2px;border-left:1px solid #AAA;');

        //Create Text Input => Untuk menampilkan text yang dipilih
        var txt = document.createElement("input");
        if (!isEnabled) {
            styleDisabled = 'color:Maroon;background-color:WhiteSmoke;';
            txt.setAttribute('readonly', 'readonly');
        }
        else
            styleDisabled = '';
        txt.setAttribute('class', 'dxeEditArea dxeEditAreaSys ' + className);
        txt.setAttribute('style', "margin:1px 1px 2px 1px;" + styleDisabled);
        $(txt).focus({ isShowDropDown: false }, _self.showAspxComboBox);
        $(txt).keydown(_self.onTxtKeyDown);

        //Create Text Hidden => Untuk menyimpan nilai (value) yang dipilih
        var txtValCbo = document.createElement("input");
        txtValCbo.setAttribute('style', 'display:none;');

        //Create Text Hidden => Untuk menyimpan id aspxComboBox
        var txtAspxCboID = document.createElement("input");
        txtAspxCboID.setAttribute('style', 'display:none;');
        txtAspxCboID.setAttribute('class', 'aspxCboID');
        txtAspxCboID.value = cboID;

        //Create Text Hidden => Untuk menyimpan enabled / disabled
        var txtIsEnabled = document.createElement("input");
        txtIsEnabled.setAttribute('style', 'display:none;');
        txtIsEnabled.setAttribute('class', 'isEnabled');
        if (isEnabled)
            txtIsEnabled.value = '1';
        else
            txtIsEnabled.value = '0';

        //Create Text Required => Untuk menyimpan Is Required
        var txtIsRequired = document.createElement("input");
        txtIsRequired.setAttribute('style', 'display:none;');
        txtIsRequired.setAttribute('class', 'isRequired');
        if (isRequired)
            txtIsRequired.value = '1';
        else
            txtIsRequired.value = '0';

        //Create Text Hidden => Untuk menyimpan unique
        var txtIsUnique = document.createElement("input");
        txtIsUnique.setAttribute('style', 'display:none;');
        txtIsUnique.setAttribute('class', 'isUnique');
        if (isUnique)
            txtIsUnique.value = '1';
        else
            txtIsUnique.value = '0';

        //Create image combobox (image jika diklik muncul list item)
        var img = document.createElement("img");
        img.setAttribute('class', 'dxEditors_edtDropDown');
        img.setAttribute('src', ResolveUrl('~/DXR.axd?r=1_5-QpXT4'));
        img.setAttribute('alt', 'v');
        if (!isEnabled)
            img.setAttribute('style', 'opacity:0.5;');
        $(cell2).click({ isShowDropDown: true }, _self.showAspxComboBox);

        cell.appendChild(txtValCbo);
        cell.appendChild(txt);
        cell.appendChild(txtAspxCboID);
        cell.appendChild(txtIsEnabled);
        cell.appendChild(txtIsUnique);
        cell.appendChild(txtIsRequired);
        cell2.appendChild(img);

        tr.appendChild(cell);
        tr.appendChild(cell2);
        tbl.appendChild(tr);

        $(tbl).addClass('borderBox');
        $div = $('<div></div>').attr('style', 'width:100%;padding: 2px 5px;').addClass('borderBox');
        $div.append($(tbl));
        //if (value != '')
        //    _self.setComboBoxProperties($(tr), className, { "value": value });
        return $div;
    }

    this.setComboBoxProperties = function ($row, cboClass, options) {
        $cbo = $row.find('.' + cboClass);
        if (options.value != null) {
            var aspxCboID = $cbo.parent().find('.aspxCboID').val();
            var aspxCbo = ASPxClientControl.GetControlCollection().GetByName(aspxCboID);
            aspxCbo.SetValue(options.value);
            //$cell = $cbo.parent().parent().parent().parent();
            $cbo.val(aspxCbo.GetText());
            $cbo.parent().find('input').first().val(aspxCbo.GetValue());
            var isUnique = $cbo.parent().find(".isUnique").val();
            if (isUnique == '1') {
                var selectedIndex = aspxCbo.GetSelectedIndex();
                aspxCbo.RemoveItem(selectedIndex);
            }
            aspxCbo.SetSelectedIndex(-1);
        }
        if (options.isEnabled != null) {
            if (options.isEnabled) {
                $tbl = $cbo.closest('table');
                //var style = $tbl.attr('style').replace('background-color:WhiteSmoke;', '');
                //$tbl.attr('style', style);
                $tbl.css('background-color', '');
                //style = $cbo.attr('style').replace('color:Maroon;background-color:WhiteSmoke;', '');
                //$cbo.attr('style', style);
                $cbo.css('color', '');
                $cbo.css('background-color', '');
                $cbo.removeAttr('readonly');

                $cbo.parent().find('.isEnabled').val('1');

                $cell2 = $cbo.closest('td').next('td');
                //style = $cell2.attr('style').replace('border-left:1px solid #AAA;', '');
                //$cell2.attr('style', style);
                $cell2.css('border-left', '');
                $cell2.attr('class', 'dxeButtonEditButton');
                $cell2.find('img').removeAttr('style');

            }
            else {
                $tbl = $cbo.closest('table');
                //var style = $tbl.attr('style').replace('background-color:WhiteSmoke;', '') + 'background-color:WhiteSmoke;';
                //$tbl.attr('style', style);
                $tbl.css('background-color', 'WhiteSmoke');
                //style = $cbo.attr('style').replace('color:Maroon;background-color:WhiteSmoke;', '') + 'color:Maroon;background-color:WhiteSmoke;';
                //$cbo.attr('style', style);
                $cbo.css('color', 'Maroon');
                $cbo.css('background-color', 'WhiteSmoke');
                $cbo.attr('readonly', 'readonly');

                $cbo.parent().find('.isEnabled').val('0');

                $cell2 = $cbo.closest('td').next('td');
                //style = $cell2.attr('style').replace('border-left:1px solid #AAA;', '') + 'border-left:1px solid #AAA;';
                //$cell2.attr('style', style);
                $cell2.css('border-left', '1px solid #AAA');
                $cell2.removeAttr('class');
                $cell2.find('img').attr('style', 'opacity:0.5');
            }
        }
    }

    this.getComboBoxValue = function ($row, cboClass) {
        $cbo = $row.find('.' + cboClass);
        return $cbo.parent().find('input').first().val();
    }
    //#endregion
    //#region ButtonEdit
    this.createButtonEdit = function (inputType, className, isEnabled, isButtonEnabled, value, isRequired) {
        var styleDisabled = '';
        if (!isEnabled)
            styleDisabled = 'background-color:WhiteSmoke;';
        var tbl = document.createElement("table");

        tbl.setAttribute('class', 'dxeButtonEdit');
        tbl.setAttribute('cellspacing', '1');
        tbl.setAttribute('cellpadding', '0');
        tbl.setAttribute('border', '0');
        tbl.setAttribute('style', 'height:24px;border-collapse:collapse;border-collapse:collapse;width:100%;display:visible;');

        var tr = document.createElement("tr");
        var cell = document.createElement("td");
        var cell2 = document.createElement("td");
        cell.setAttribute('class', 'dxic');
        cell.setAttribute('style', styleDisabled);
        if (isButtonEnabled) {
            cell2.setAttribute('class', 'dxeButtonEditButton');
            cell2.setAttribute('style', '-moz-user-select:none;padding:0 2px;border-left:1px solid #AAA;');
        }
        else
            cell2.setAttribute('style', '-moz-user-select:none;padding:0 2px;border-left:1px solid #AAA;' + styleDisabled);

        //Create Text Input => Untuk menampilkan text yang dipilih
        var txt = document.createElement("input");
        if (!isEnabled) {
            styleDisabled = 'color:Maroon;background-color:WhiteSmoke;';
            txt.setAttribute('readonly', 'readonly');
        }
        else
            styleDisabled = '';
        txt.setAttribute('class', 'dxeEditArea dxeEditAreaSys ' + className);
        txt.setAttribute('style', "margin:1px 1px 2px 1px;" + styleDisabled);
        $(txt).focus({ isShowDropDown: false }, _self.showAspxComboBox);
        $(txt).keydown(_self.onBteKeyDown);

        var img = document.createElement("img");
        img.setAttribute('class', 'dxEditors_edtEllipsis');
        img.setAttribute('src', ResolveUrl('~/DXR.axd?r=1_5-QpXT4'));
        img.setAttribute('alt', 'v');
        if (!isButtonEnabled)
            img.setAttribute('style', 'opacity:0.5;');

        $txtInputType = $("<input type='hidden' class='inputType'></input>").val(inputType);
        $txtValBte = $("<input type='hidden' class='txtValBte'></input>");
        $txtIsEnabled = $("<input type='hidden' class='isEnabled'></input>");
        if (isEnabled)
            $txtIsEnabled.val('1');
        else
            $txtIsEnabled.val('0');
        $txtIsButtonEnabled = $("<input type='hidden' class='isButtonEnabled'></input>");
        if (isButtonEnabled)
            $txtIsButtonEnabled.val('1');
        else
            $txtIsButtonEnabled.val('0');

        $txtIsRequired = $("<input type='hidden' class='isRequired'></input>");
        if (isRequired)
            $txtIsRequired.val('1');
        else
            $txtIsRequired.val('0');

        $(cell).append($txtValBte);
        $(cell).append($(txt));
        $(cell).append($txtInputType);
        $(cell).append($txtIsEnabled);
        $(cell).append($txtIsButtonEnabled);
        $(cell).append($txtIsRequired);
        cell2.appendChild(img);
        $(cell2).click(function () {
            var $row = $(this).closest('tr').parent().closest('tr');
            var isButtonEnabled = ($(this).parent().find('.isButtonEnabled').val() == '1');
            if (isButtonEnabled) {
                if (typeof _self.onBteButtonClickHandler == 'function')
                    _self.onBteButtonClickHandler($row, className);
            }
        });

        tr.appendChild(cell);
        tr.appendChild(cell2);
        tbl.appendChild(tr);

        $(tbl).addClass('borderBox');
        $div = $('<div></div>').attr('style', 'width:100%;padding: 2px 5px;').addClass('borderBox');
        $div.append($(tbl));
        //if (value != '')
        //    _self.setComboBoxProperties($(tr), className, { "value": value });
        return $div;
    }

    this.setButtonEditFocus = function ($row, bteClass) {
        return $row.find('.' + bteClass).focus();
    }

    this.setButtonEditProperties = function ($row, bteClass, options) {
        var $bte = $row.find('.' + bteClass);

        if (options.value != null) {
            $bteHiddenValue = $bte.parent().find('input').first();
            $bteHiddenValue.val(options.value);

            $displayValue = options.value;

            var $inputType = $bte.parent().find('.inputType').val();
            if ($inputType == 'money')
                $displayValue = parseFloat(options.value).formatMoney(2, '.', ',');
            else if ($inputType == 'qty')
                $displayValue = parseFloat(options.value).formatMoney(0, '.', ',');
            else if ($inputType == 'percentage')
                $displayValue = options.value + '%';
            $bte.val($displayValue);
        }
        if (options.isEnabled != null) {
            if (options.isButtonEnabled == null)
                options.isButtonEnabled = options.isEnabled;
            if (options.isEnabled) {
                var $tbl = $bte.parent().parent().parent();
                var style = $tbl.attr('style').replace('background-color:WhiteSmoke;', '');
                $tbl.attr('style', style);
                style = $bte.attr('style').replace('color:Maroon;background-color:WhiteSmoke;', '');
                $bte.attr('style', style);
                $bte.removeAttr('readonly');

                $bte.parent().find('.isEnabled').val('1');
            }
            else {
                var $tbl = $bte.parent().parent().parent();
                var style = $tbl.attr('style').replace('background-color:WhiteSmoke;', '') + 'background-color:WhiteSmoke;';
                $tbl.attr('style', style);
                style = $bte.attr('style').replace('color:Maroon;background-color:WhiteSmoke;', '') + 'color:Maroon;background-color:WhiteSmoke;';
                $bte.attr('style', style);
                $bte.attr('readonly', 'readonly');

                $bte.parent().find('.isEnabled').val('0');
            }
        }
        if (options.isButtonEnabled != null) {
            if (options.isButtonEnabled) {
                $cell2 = $bte.parent().next('td');
                var style = $cell2.attr('style').replace('border-left:1px solid #AAA;', '');
                $cell2.attr('style', style);
                $cell2.attr('class', 'dxeButtonEditButton');
                $cell2.find('img').removeAttr('style');

                $bte.parent().find('.isButtonEnabled').val('1');
            }
            else {
                $cell2 = $bte.parent().next('td');
                style = $cell2.attr('style').replace('border-left:1px solid #AAA;', '') + 'border-left:1px solid #AAA;';
                $cell2.attr('style', style);
                $cell2.removeAttr('class');
                $cell2.find('img').attr('style', 'opacity:0.5');

                $bte.parent().find('.isButtonEnabled').val('0');
            }
        }
    }

    this.getButtonEditValue = function ($row, bteClass) {
        $bte = $row.find('.' + bteClass);
        return $bte.parent().find('input').first().val();
    }
    //#endregion
    //#region SearchTextBox
    this.createSearchTextBox = function (ledID, className, isEnabled, value) {
        $div = $('<div></div>').attr('style', 'width:100%;padding: 2px 5px;').addClass('borderBox');
        $txt = $("<input type='text'></input").attr('style', 'width:100%;padding:2px 1px 3px 1px;').addClass('borderBox ' + className);
        if (!isEnabled)
            $txt.attr('readonly', 'readonly');
        $txt.focus(_self.showQisSearchTextBox);

        $txtHiddenValue = $("<input type='hidden' class='hiddenValue'></input>");
        $txtHiddenSearch = $("<input type='hidden' class='hiddenSearch'></input>");
        $txtHiddenDisplay = $("<input type='hidden' class='hiddenDisplay'></input>");
        $txtLedID = $("<input type='hidden' class='qisLedID'></input>").val(ledID);
        $txtIsEnabled = $("<input type='hidden' class='isEnabled'></input>");
        if (isEnabled)
            $txtIsEnabled.val('1');
        else
            $txtIsEnabled.val('0');

        $div.append($txtHiddenValue);
        $div.append($txt);
        $div.append($txtHiddenSearch);
        $div.append($txtHiddenDisplay);
        $div.append($txtLedID);
        $div.append($txtIsEnabled);

        return $div;
    }


    this.setSearchTextBoxProperties = function ($row, ledClass, options) {
        if (options.value != null) {
            $led = $row.find('.' + ledClass).parent();
            var qisLedID = $row.find('.' + ledClass).parent().find(".qisLedID").val();
            var clientInstanceName = $('#' + qisLedID).find('.hdnClientInstanceName').val();
            eval("var qisLed = " + clientInstanceName);
            qisLed.SetValue(options.value);

            var valueText = qisLed.GetValueText();
            var displayText = qisLed.GetDisplayText();
            var searchText = qisLed.GetSearchText();

            $led.find(".hiddenDisplay").val(displayText)
            $led.find(".hiddenValue").val(valueText);
            $led.find(".hiddenSearch").val(searchText);
            $led.find('input:eq(1)').val(displayText);
        }
    }
    this.getSearchTextBoxValue = function ($row, ledClass) {
        $led = $row.find('.' + ledClass);
        return $led.parent().find('input').first().val();
    }
    this.getSearchTextBoxText = function ($row, ledClass) {
        $led = $row.find('.' + ledClass);
        return $led.parent().find('.hiddenDisplay').val();
    }
    //#endregion
    //#region CellHidden
    this.createCellHidden = function (className, value) {
        var cell = document.createElement("td");
        cell.setAttribute("class", className);
        cell.setAttribute("style", "display:none");

        var input = document.createElement("input");
        input.setAttribute("value", value);

        cell.appendChild(input);
        return cell;
    }

    this.setCellHiddenValue = function ($row, className, value) {
        $row.find('.' + className).find('input').val(value);
    }

    this.getCellHiddenValue = function ($row, className) {
        return $row.find('.' + className).find('input').val();
    }
    //#endregion

    //#region KeyDown
    this.onBteKeyDown = function (e) {
        if (!e.ctrlKey && e.keyCode == 13) {//enter
            _self.isAddRowFromEnter = true;
            _self.addRow();
            var className = $(this).attr('class').split(' ')[2];
            $newFocus = $(this).closest('tr').parent().closest('tr').next('tr').find('input:visible').first();
            $newFocus.focus();
        }
        else if (e.keyCode == 36) //home
        {
            var className = $(this).attr('class').split(' ')[2];
            $tbl = $(_self.tblGrdView);
            $newFocus = $tbl.find('.trTransaction').first().find('.' + className);
            $newFocus.focus();
        }
        else if (e.keyCode == 35) //end
        {
            var className = $(this).attr('class').split(' ')[2];
            $tbl = $(_self.tblGrdView);
            $newFocus = $tbl.find('.trTransaction').last().find('.' + className);
            $newFocus.focus();
        }
        else if (e.keyCode == 34)//page down
        {
            var className = $(this).attr('class').split(' ')[2];
            $newFocus = $(this).closest('tr').parent().closest('tr').next('tr').find("." + className);
            if ($(this).next('.' + className) != null) {
                $newFocus.focus();
            }
        }
        else if (e.keyCode == 33)// page up
        {
            var className = $(this).attr('class').split(' ')[2];
            $newFocus = $(this).closest('tr').parent().closest('tr').prev('tr').find("." + className);

            $newFocus.focus();
        }
        else if (e.ctrlKey && !e.shiftKey) {
            if (e.keyCode == 13) {//enter
                var isButtonEnabled = ($(this).parent().find('.isButtonEnabled').val() == '1');
                if (isButtonEnabled) {
                    var className = $(this).attr('class').split(' ')[2];
                    var $row = $(this).closest('tr').parent().closest('tr');
                    if (typeof _self.onBteButtonClickHandler == 'function')
                        _self.onBteButtonClickHandler($row, className);
                }
            }
            else if (e.keyCode == 46) {//delete
                var numRow = $('#' + _self.gridID).find('.trTransaction').length;
                $trDeleted = $(this).closest('tr').parent().closest('tr');
                if (_self.getRowEnabled($trDeleted)) {
                    if (parseInt(numRow) > 1) {
                        if (confirm('Are You Sure?')) {
                            var className = $(this).attr('class').split(' ')[2];

                            $newFocus = $trDeleted.next('tr').find("." + className);
                            if ($newFocus.html() == null)
                                $newFocus = $trDeleted.prev('tr').find("." + className);

                            $newFocus.focus();

                            for (var i = 0; i < _self.listParam.length; ++i) {
                                var inputType = _self.listParam[i].type;
                                if (inputType == 'cbo') {
                                    var aspxCboID = _self.listParam[i].cboID;
                                    var aspxCbo = ASPxClientControl.GetControlCollection().GetByName(aspxCboID);

                                    var isUnique = _self.listParam[i].isUnique;
                                    if (isUnique) {
                                        $cell = $trDeleted.find('td:eq(' + (i + 1) + ')');
                                        $cbo = $cell.find('table').first();
                                        var text = $cbo.find('input:eq(1)').val();
                                        var value = $cbo.find('input').first().val();
                                        if (text != "")
                                            _self.addCboItem(aspxCbo, text, value);
                                    }
                                }
                            }

                            var objDeleted = {};
                            var ctr = -1;
                            objDeleted['key'] = $trDeleted.find('.keyValue').find('input').val();
                            $trDeleted.children().each(function () {
                                if (ctr > -1 && ctr < _self.listParam.length) {
                                    $txt = $(this).find('input').val();
                                    var className = _self.listParam[ctr].className;
                                    objDeleted[className] = $txt;
                                }
                                ctr++;
                            });

                            $trDeleted.remove();

                            if (typeof _self.onIsChangedChangeHandler == 'function')
                                _self.onIsChangedChangeHandler(true);
                            if (typeof _self.onRowDeletedHandler == 'function')
                                _self.onRowDeletedHandler(objDeleted);
                        }
                    }
                }
            }
            else if (e.keyCode == 40) { //down arrow
                var className = $(this).attr('class').split(' ')[2];
                $newFocus = $(this).closest('tr').parent().closest('tr').next('tr').find("." + className);
                if ($(this).next('.' + className) != null) {
                    $newFocus.focus();
                }
            }
            else if (e.keyCode == 38) { //up arrow
                var className = $(this).attr('class').split(' ')[2];
                $newFocus = $(this).closest('tr').parent().closest('tr').prev('tr').find("." + className);

                $newFocus.focus();
            }
            else if (e.keyCode == 37) { //left arrow
                $newFocus = $(this).closest('tr').parent().closest('td').prev('td').find("input");
                $newFocus.focus();
            }
            else if (e.keyCode == 39) { //right arrow
                $newFocus = $(this).closest('tr').parent().closest('td').next('td').find("input");
                $newFocus.focus();
            }
        }

    }
    this.onTxtKeyDown = function (e) {
        if (e.keyCode == 13) {//enter
            _self.isAddRowFromEnter = true;
            _self.addRow();
            var className = $(this).attr('class').split(' ')[1];
            $newFocus = $(this).closest('tr').next('tr').find('input:visible').first();
            $newFocus.focus();
        }
        else if (e.keyCode == 36) //home
        {
            var className = $(this).attr('class').split(' ')[1];
            $newFocus = $('#' + _self.gridID).find('.trTransaction').first().find('.' + className);
            $newFocus.focus();
        }
        else if (e.keyCode == 35) //end
        {
            var className = $(this).attr('class').split(' ')[1];
            $newFocus = $('#' + _self.gridID).find('.trTransaction').last().find('.' + className);
            $newFocus.focus();
        }
        else if (e.keyCode == 34)//page down
        {
            var className = $(this).attr('class').split(' ')[1];
            $newFocus = $(this).closest('tr').next('tr').find("." + className);
            if ($(this).next('.' + className) != null) {
                $newFocus.focus();
            }
        }
        else if (e.keyCode == 33)// page up
        {
            var className = $(this).attr('class').split(' ')[1];
            $newFocus = $(this).closest('tr').prev('tr').find("." + className);

            $newFocus.focus();
        }
        else if (e.ctrlKey && !e.shiftKey) {
            if (e.keyCode == 46) {//delete
                var numRow = $('#' + _self.gridID).find('.trTransaction').length;
                $trDeleted = $(this).closest('tr');
                if (_self.getRowEnabled($trDeleted)) {
                    if (parseInt(numRow) > 1) {
                        if (confirm('Are You Sure?')) {
                            var className = $(this).attr('class').split(' ')[1];

                            $newFocus = $trDeleted.next('tr').find("." + className);
                            if ($newFocus.html() == null)
                                $newFocus = $trDeleted.prev('tr').find("." + className);

                            $newFocus.focus();

                            for (var i = 0; i < _self.listParam.length; ++i) {
                                var inputType = _self.listParam[i].type;
                                if (inputType == 'cbo') {
                                    var aspxCboID = _self.listParam[i].cboID;
                                    var aspxCbo = ASPxClientControl.GetControlCollection().GetByName(aspxCboID);

                                    var isUnique = _self.listParam[i].isUnique;
                                    if (isUnique) {
                                        $cell = $trDeleted.find('td:eq(' + (i + 1) + ')');
                                        $cbo = $cell.find('table').first();
                                        var text = $cbo.find('input:eq(1)').val();
                                        var value = $cbo.find('input').first().val();
                                        if (text != "")
                                            _self.addCboItem(aspxCbo, text, value);
                                    }
                                }
                            }

                            var objDeleted = {};
                            var ctr = -1;
                            objDeleted['key'] = $trDeleted.find('.keyValue').find('input').val();
                            $trDeleted.children().each(function () {
                                if (ctr > -1 && ctr < _self.listParam.length) {
                                    $txt = $(this).find('input').val();
                                    var className = _self.listParam[ctr].className;
                                    objDeleted[className] = $txt;
                                }
                                ctr++;
                            });

                            $trDeleted.remove();

                            if (typeof _self.onIsChangedChangeHandler == 'function')
                                _self.onIsChangedChangeHandler(true);
                            if (typeof _self.onRowDeletedHandler == 'function')
                                _self.onRowDeletedHandler(objDeleted);
                        }
                    }
                }
            }
            else if (e.keyCode == 40) { //down arrow
                var className = $(this).attr('class').split(' ')[1];
                $newFocus = $(this).closest('tr').next('tr').find("." + className);
                if ($(this).next('.' + className) != null) {
                    $newFocus.focus();
                }
            }
            else if (e.keyCode == 38) { //up arrow
                var className = $(this).attr('class').split(' ')[1];
                $newFocus = $(this).closest('tr').prev('tr').find("." + className);

                $newFocus.focus();
            }
            else if (e.keyCode == 37) { //left arrow
                $newFocus = $(this).closest('td').prev('td').find("input");
                $newFocus.focus();
            }
            else if (e.keyCode == 39) { //right arrow
                $newFocus = $(this).closest('td').next('td').find("input");
                $newFocus.focus();
            }
        }

    }
    this.onCboKeyDown = function (s, e) {
        if (e.htmlEvent.keyCode == 115) { s.HideDropDown(); s.ShowDropDown(); }
        else if (e.htmlEvent.keyCode == 36) //home
        {
            var className = $cboCell.find('div').first().find('input:eq(1)').attr('class').split(' ')[2];
            $newFocus = $('#' + _self.gridID).find('.trTransaction').first().find('.' + className);
            _self.hideAspxComboBox(s);
            $newFocus.focus();
            PreventEvent(e.htmlEvent);
        }
        else if (e.htmlEvent.keyCode == 35) //end
        {
            var className = $cboCell.find('table').first().find('input:eq(1)').attr('class').split(' ')[2];
            $newFocus = $('#' + _self.gridID).find('.trTransaction').last().find('.' + className);
            _self.hideAspxComboBox(s);
            $newFocus.focus();
            PreventEvent(e.htmlEvent);
        }
        else {
            if (e.htmlEvent.ctrlKey) {
                if (e.htmlEvent.keyCode == 46) { //delete
                    var numRow = $('#' + _self.gridID).find('.trTransaction').length;
                    $trProcess = $(s.GetInputElement()).parent().parent().parent().parent().parent().parent();
                    if (_self.getRowEnabled($trProcess)) {
                        if (parseInt(numRow) > 1) {
                            if (confirm('Are You Sure?')) {
                                var className = $cboCell.find('table').first().find('input:eq(1)').attr('class').split(' ')[2];
                                $newFocus = $trProcess.next('tr').find("." + className);
                                if ($newFocus.html() == null)
                                    $newFocus = $trProcess.prev('tr').find("." + className);

                                for (var i = 0; i < _self.listParam.length; ++i) {
                                    var par = _self.listParam[i].split('|');
                                    var inputType = par[0];
                                    if (inputType == 'cbo') {
                                        var aspxCboID = par[1];
                                        var aspxCbo = ASPxClientControl.GetControlCollection().GetByName(aspxCboID);

                                        var isUnique = (par[3] == 'true');
                                        if (isUnique) {
                                            $cell = $trProcess.find('td:eq(' + (i + 1) + ')');
                                            $cbo = $cell.find('table').first();
                                            var text = $cbo.find('input:eq(1)').val();
                                            var value = $cbo.find('input').first().val();
                                            if (text != "")
                                                _self.addCboItem(aspxCbo, text, value);
                                        }
                                    }
                                }
                                $newFocus.focus();
                                var objDeleted = {};
                                var ctr = -1;
                                objDeleted['key'] = $trProcess.find('.keyValue').find('input').val();
                                $trProcess.children().each(function () {
                                    if (ctr > -1 && ctr < _self.listParam.length) {
                                        $txt = $(this).find('input').val();
                                        var par = _self.listParam[ctr].split('|');
                                        var inputType = par[0];
                                        var className = '';
                                        if (inputType == 'chk')
                                            className = par[1];
                                        else
                                            className = par[2];
                                        objDeleted[className] = $txt;
                                    }
                                    ctr++;
                                });

                                $trProcess.remove();

                                if (_self.onIsChangedChangeHandler != '')
                                    window[_self.onIsChangedChangeHandler](true);
                                if (_self.onRowDeletedHandler != '')
                                    window[_self.onRowDeletedHandler](objDeleted);
                                PreventEvent(e.htmlEvent);
                            }
                        }
                    }
                }
                else if (e.htmlEvent.keyCode == 40) {  //down arrow
                    $className = $cboCell.find('table').first().find('input:eq(1)').attr('class').split(' ')[2];
                    $newFocus = $(s.GetInputElement()).closest('tr').parent().closest('tr').next('tr').find('.' + $className);
                    _self.hideAspxComboBox(s);
                    $newFocus.focus();
                    PreventEvent(e.htmlEvent);
                }
                else if (e.htmlEvent.keyCode == 38) { //up arrow
                    $className = $cboCell.find('table').first().find('input:eq(1)').attr('class').split(' ')[2];
                    $newFocus = $(s.GetInputElement()).closest('tr').parent().closest('tr').prev('tr').find('.' + $className);
                    _self.hideAspxComboBox(s);
                    $newFocus.focus();
                    PreventEvent(e.htmlEvent);
                }
                else if (e.htmlEvent.keyCode == 37) { //left arrow
                    $newFocus = $(s.GetInputElement()).closest('tr').parent().closest('td').prev('td').find('input');
                    _self.hideAspxComboBox(s);
                    $newFocus.focus();
                    PreventEvent(e.htmlEvent);
                }
                else if (e.htmlEvent.keyCode == 39) { //right arrow
                    $newFocus = $(s.GetInputElement()).closest('tr').parent().closest('td').next('td').find('input');
                    _self.hideAspxComboBox(s);
                    $newFocus.focus();
                    PreventEvent(e.htmlEvent);
                }
            }
        }
    }
    //#endregion

    this.createCell = function (listParam, isButtonDelete) {
        $cell = $("<td></td>");
        if (isButtonDelete) {
            $cell.append(_self.createButtonDelete());
        }
        else {
            if (listParam.type == 'txt') { // txt
                var dataType = listParam.dataType == null ? '' : listParam.dataType;
                var className = listParam.className;
                var isEnabled = listParam.isEnabled;
                var value = listParam.value == null ? '' : listParam.value;
                var isRequired = listParam.isRequired == null ? false : listParam.isRequired;
                $cell.append(_self.createTextBox(dataType, className, isEnabled, value, isRequired));
            }
            else if (listParam.type == 'cbo') {
                var aspxCboID = listParam.cboID;
                var className = listParam.className;
                var isUnique = listParam.isUnique;
                var isEnabled = listParam.isEnabled;
                var value = listParam.value == null ? '' : listParam.value;
                var isRequired = listParam.isRequired == null ? false : listParam.isRequired;
                $cell.append(_self.createComboBox(aspxCboID, className, isUnique, isEnabled, value, isRequired));
            }
            else if (listParam.type == 'led') {
                var qisLedID = listParam.ledID;
                var className = listParam.className;
                var isEnabled = listParam.isEnabled;
                var value = listParam.value == null ? '' : listParam.value;
                $cell.append(_self.createSearchTextBox(qisLedID, className, isEnabled, value));
            }
            else if (listParam.type == 'bte') {
                var dataType = listParam.dataType == null ? '' : listParam.dataType;
                var className = listParam.className;
                var isEnabled = listParam.isEnabled;
                var isButtonEnabled = listParam.isButtonEnabled;
                var isRequired = listParam.isRequired == null ? false : listParam.isRequired;
                var value = listParam.value == null ? '' : listParam.value;
                $cell.append(_self.createButtonEdit(dataType, className, isEnabled, isButtonEnabled, value, isRequired));
            }
        }
        return $cell;
    }
    this.addRow = function (initGridView) {
        $tr = $("<tr></tr>").attr('class', 'trTransaction');

        $tr.append(_self.createCell("", true));
        for (var i = 0; i < _self.listParam.length; ++i) {
            if (_self.listParam[i].type == 'hdn') {
                var value = _self.listParam[i].value == null ? '' : _self.listParam[i].value;
                $tr.append(_self.createCellHidden(_self.listParam[i].className, value));
            }
            else
                $tr.append(_self.createCell(_self.listParam[i], false));
        }
        $tr.append(_self.createCellKeyValue());
        $tr.append(_self.createCellFlagIsChanged());
        $tr.append(_self.createCellFlagIsEnabled());
        $trFooter = $('#' + _self.gridID).find('.trFooter');
        if ($trFooter.length < 1)
            $('#' + _self.gridID).append($tr);
        else 
            $tr.insertBefore($trFooter);

        /*if (!initGridView) {
        if (_self.onAddRowHandler != '')
        window[_self.onAddRowHandler]();
        }*/

    }

    //#region Key Value
    this.createCellKeyValue = function () {
        $td = $("<td class='keyValue' style='display:none'></td>");
        $input = $('<input></input>').val('0');

        $td.append($input);
        return $td;
    }

    this.setKeyValue = function ($row, value) {
        $row.find('.keyValue').find('input').val(value);
    }

    this.getKeyValue = function ($row) {
        return $row.find('.keyValue').find('input').val();
    }
    //#endregion
    //#region Row Enabled
    this.createCellFlagIsEnabled = function () {
        $td = $("<td class='isEnabled' style='display:none'></td>");
        $input = $('<input></input>').val('1');

        $td.append($input);
        return $td;
    }


    this.setRowEnabled = function ($row, isEnabled) {
        var val = '';
        if (isEnabled)
            val = "1";
        else {
            val = "0";
            $imgBtnDelete = $row.find('td').first().find('img');
            $imgBtnDelete.hide();
        }
        $row.find('.isEnabled').find('input').val(val);
        for (var i = 0; i < _self.listParam.length; ++i) {
            var inputType = _self.listParam[i].type;
            if (inputType == 'cbo') {
                var className = _self.listParam[i].className;
                var colIsEnabled = isEnabled;
                if (isEnabled)
                    colIsEnabled = (_self.listParam[i].isEnabled);
                _self.setComboBoxProperties($row, className, { "isEnabled": colIsEnabled });
            }
            else { // txt
                var className = _self.listParam[i].className;
                var colIsEnabled = isEnabled;
                if (isEnabled)
                    colIsEnabled = (_self.listParam[i].isEnabled);
                _self.setTextBoxProperties($row, className, { "isEnabled": colIsEnabled });
            }
        }
    }

    this.getRowEnabled = function ($row) {
        $input = $row.find('.isEnabled').find('input');
        if ($input.val() == "1")
            return true;
        return false;
    }
    //#endregion
    //#region Row Is Changed
    this.createCellFlagIsChanged = function () {
        $td = $("<td class='isChanged' style='display:none'></td>");
        $input = $('<input></input>').val('0');

        $td.append($input);
        return $td;
    }

    this.setRowChanged = function ($row, value) {
        $input = $row.find('.isChanged').find('input');
        if (value)
            $input.val("1");
        else
            $input.val("0");
    }

    this.getRowChanged = function ($row, value) {
        $input = $row.find('.isChanged').find('input');
        if ($input.val() == "1")
            return true;
        return false;
    }
    //#endregion

    //#region Utility
    this.getColumnData = function (columnName, separator) {
        $result = '';
        $('#' + _self.gridID).find('.trTransaction').each(function () {
            if ($result != '')
                $result += separator;
            $result += $(this).find('.' + columnName).parent().find('input').val();
        });
        return $result;
    }

    this.validate = function () {
        var isValid = true;
        $('#' + _self.gridID).find('.trTransaction').each(function () {
            var ctr = -1;
            var isRowEmpty = true;
            $(this).children().each(function () {
                //Karena td pertama isinya hanya button delete, maka diskip
                if (ctr > -1 && ctr < _self.listParam.length) {
                    if (ctr < 1) {
                        $txt = $(this).find('input').val();
                        isRowEmpty = ($txt == "")
                    }
                    if (!isRowEmpty) {
                        if (_self.listParam[ctr].type == 'bte') {
                            $txt = $(this).find('input').val();
                            var isRequired = ($(this).find('.isRequired').val() == '1');
                            var isEnabled = ($(this).find('.isEnabled').val() == '1');
                            var isButtonEnabled = ($(this).find('.isButtonEnabled').val() == '1');
                            if ((isEnabled || isButtonEnabled) && isRequired && $txt == '') {
                                $(this).find('table:eq(0)').css('border', "solid 1px red");
                                isValid = false;
                            }
                            else {
                                $tbl = $(this).find('table:eq(0)');
                                $tbl.attr('class', 'dxeButtonEdit');
                                $tbl.attr('border', '0');
                                $tbl.attr('style', 'height:24px;border-collapse:collapse;border-collapse:collapse;width:100%;display:visible;');
                            }
                        }
                        else if (_self.listParam[ctr].type == 'cbo') {
                            $txt = $(this).find('input').val();
                            var isRequired = ($(this).find('.isRequired').val() == '1');
                            var isEnabled = ($(this).find('.isEnabled').val() == '1');
                            if (isEnabled && isRequired && $txt == '') {
                                $(this).find('table:eq(0)').css('border', "solid 1px red");
                                isValid = false;
                            }
                            else {
                                var styleDisabled = '';
                                if (!isEnabled)
                                    styleDisabled = 'background-color:WhiteSmoke;';
                                $tbl = $(this).find('table:eq(0)');
                                $tbl.attr('class', 'dxeButtonEdit');
                                $tbl.attr('border', '0');
                                $tbl.attr('style', 'height:24px;border-collapse:collapse;border-collapse:collapse;width:100%;display:visible;' + styleDisabled);
                            }
                        }
                        else if (_self.listParam[ctr].type == 'txt') {
                            $txt = $(this).find('input');

                            var isRequired = ($(this).find('.isRequired').val() == '1');
                            var isEnabled = !$txt.is('[readonly]');
                            if (isEnabled && isRequired && $txt.val() == '') {
                                $txt.css('border', "solid 1px red");
                                isValid = false;
                            }
                            else {
                                $txt.attr('style', 'width:100%;padding:2px 1px 3px 1px;');
                            }
                        }
                    }
                }
                ctr++;
            });
        });
        return isValid;
    }

    this.getTableData = function () {
        $result = '';
        $('#' + _self.gridID).find('.trTransaction').each(function () {
            var ctr = -1;
            $rowParam = '';
            $rowParam += $(this).find('.isChanged').find('input').val() + ';';
            $rowParam += $(this).find('.keyValue').find('input').val() + ';';
            $(this).children().each(function () {
                //Karena td pertama isinya hanya button delete, maka diskip
                if (ctr > -1 && ctr < _self.listParam.length) {
                    $txt = $(this).find('input').val();
                    if ($txt !== undefined)
                        $rowParam += $txt + ';';
                }
                ctr++;
            });
            $result += $rowParam.slice(0, -1) + '|';
        });

        return $result.slice(0, -1);
    }

    this.clearTable = function () {
        $('#' + _self.gridID).find('.trTransaction').remove();
    }

    this.getRow = function (idx) {
        $row = $('#' + _self.gridID).find('.trTransaction:eq(' + idx + ')');
        return $row;
    }

    this.getRowCount = function () {
        return $('#' + _self.gridID).find('.trTransaction').length;
    }

    this.removeRow = function ($row) {
        for (var i = 0; i < _self.listParam.length; ++i) {
            var par = _self.listParam[i];
            var inputType = _self.listParam[i].type;
            if (inputType == 'cbo') {
                var aspxCboID = _self.listParam[i].cboID;
                var aspxCbo = ASPxClientControl.GetControlCollection().GetByName(aspxCboID);

                $cell = $row.find('td:eq(' + (i + 1) + ')');
                var isUnique = _self.listParam[i].isUnique;
                if (isUnique) {
                    $cbo = $cell.find('table').first();
                    var text = $cbo.find('input:eq(1)').val();
                    var value = $cbo.find('input').first().val();

                    if (text != "")
                        _self.addCboItem(aspxCbo, text, value);
                }
            }
        }
        $row.remove();
    }
    //#endregion
}