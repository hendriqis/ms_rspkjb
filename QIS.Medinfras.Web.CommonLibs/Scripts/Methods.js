var Methods = new (function () {
    //#region Get Paramedic Work Times
    this.getParamedicWorkTimes = function (healthcareServiceUnitID, paramedicID, selectedDay, date, appointmentID, functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetParamedicWorkTimes'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "healthcareServiceUnitID" : "' + healthcareServiceUnitID + '", "paramedicID" : "' + paramedicID + '", "selectedDay" : "' + selectedDay + '", "date" : "' + date + '", "appointmentID" : "' + appointmentID + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get Suggested Price
    this.getSuggestedPrice = function (bookID, itemID, functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetSuggestedPrice'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "bookID" : "' + bookID + '", "itemID" : "' + itemID + '"}',
            dataType: 'json',
            error: function (msg) {
                displayErrorMessageBox("GET TARIFF", msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get Draft Item Tariff
    this.getDraftItemTariff = function (appointmentID, classID, itemID, transactionDate, functionHandler, testPartnerID) {
        var partnerID = testPartnerID;
        if (!testPartnerID) {
            partnerID = 0;
        }
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetDraftItemTariff'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "appointmentID" : "' + appointmentID + '", "classID" : "' + classID + '", "itemID" : "' + itemID + '", "transactionDate" : "' + transactionDate + '", "testPartnerID" : "' + partnerID + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get Item Tariff
    this.getItemTariff = function (registrationID, visitID, classID, itemID, transactionDate, functionHandler, testPartnerID) {
        var partnerID = testPartnerID;
        if (!testPartnerID) {
            partnerID = 0;
        }
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetItemTariff'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "registrationID" : "' + registrationID + '", "visitID" : "' + visitID + '", "classID" : "' + classID + '", "itemID" : "' + itemID + '", "transactionDate" : "' + transactionDate + '", "testPartnerID" : "' + partnerID + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get INACBG Grouper Tariff
    this.GetINACBGGrouperTariff = function (jnsrawat, klsrawat, diagnosisCode, procedureCode, functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetINACBGGrouperTariff'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "jnsrawat" : "' + jnsrawat + '", "klsrawat" : "' + klsrawat + '", "diagnosisCode" : "' + diagnosisCode + '", "procedureCode" : "' + procedureCode + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get Tariff Estimation
    this.getTariffEstimation = function (classID, businessPartnerID, coverageTypeID, itemID, transactionDate, departmentID, itemType, functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetTariffEstimation'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "businessPartnerID" : "' + businessPartnerID + '", "coverageTypeID" : "' + coverageTypeID + '", "lstItemID" : "' + itemID + '", "classID" : "' + classID + '", "departmentID" : "' + departmentID + '", "itemType" : "' + itemType + '", "transactionDate" : "' + transactionDate + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get Item Revenue Sharing
    this.getItemRevenueSharing = function (itemCode, paramedicID, classID, GCParamedicRole, visitID, chargesHealthcareServiceUnitID, transactionDate, transactionTime, functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetItemRevenueSharing'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "ItemCode" : "' + itemCode + '", "ParamedicID" : "' + paramedicID + '", "ClassID" : "' + classID + '", "GCParamedicRole" : "' + GCParamedicRole + '", "VisitID" : "' + visitID + '", "ChargesHealthcareServiceUnitID" : "' + chargesHealthcareServiceUnitID + '", "TransactionDate" : "' + transactionDate + '", "TransactionTime" : "' + transactionTime + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get Item Qty On Order
    this.getItemQtyOnOrder = function (itemID, locationID, type, functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetItemQtyOnOrder'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "itemID" : "' + itemID + '", "locationID" : "' + locationID + '", "type" : "' + type + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get Item Master Purchase
    this.getItemMasterPurchase = function (itemID, businessPartnerID, functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetItemMasterPurchase'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "itemID" : "' + itemID + '", "businessPartnerID" : "' + businessPartnerID + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get Item Master Purchase List
    this.getItemMasterPurchaseList = function (itemID, businessPartnerID, functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetItemMasterPurchaseList'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "itemID" : "' + itemID + '", "businessPartnerID" : "' + businessPartnerID + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get Item Master Purchase With Price
    this.getItemMasterPurchaseWithDate = function (itemID, businessPartnerID, effectiveDate, functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetItemMasterPurchaseWithDate'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "itemID" : "' + itemID + '", "businessPartnerID" : "' + businessPartnerID + '", "effectiveDate" : "' + effectiveDate + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get Item Master Purchase With Price
    this.getItemMasterPurchaseWithDateList = function (itemID, businessPartnerID, effectiveDate, functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetItemMasterPurchaseWithDateList'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "itemID" : "' + itemID + '", "businessPartnerID" : "' + businessPartnerID + '", "effectiveDate" : "' + effectiveDate + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get GetDateDiffPOPORPerSupplier
    this.getDateDiffPOPORPerSupplier = function (supplierID, functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetDateDiffPOPORPerSupplier'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "supplierID" : "' + supplierID + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get GetPatientChargesValidationDoubleInputList
    this.getPatientChargesValidationDoubleInputList = function (visitID, transactionID, transactionDate, functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetPatientChargesValidationDoubleInputList'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "visitID" : "' + visitID + '", "transactionID" : "' + transactionID + '", "transactionDate" : "' + transactionDate + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get Object
    this.getListObject = function (methodName, filterExpression, functionHandler) {
        showLoadingPanel();
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetListObject'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "methodName" : "' + methodName + '", "filterExpression" : "' + filterExpression + '"}',
            dataType: 'json',
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            }
        });     //end ajax
    };

    this.getObject = function (methodName, filterExpression, functionHandler) {
        showLoadingPanel();
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetObject'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "methodName" : "' + methodName + '", "filterExpression" : "' + filterExpression + '"}',
            dataType: 'json',
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            }
        });     //end ajax
    };

    this.GetPesertaLocalCache = function (noPeserta, functionHandler) {
        showLoadingPanel();
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetPesertaLocalCache'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "noPeserta" : "' + noPeserta + '"}',
            dataType: 'json',
            error: function (msg) {
                hideLoadingPanel();
                alert(msg.responseText);
            },
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            }
        });     //end ajax
    };

    this.getValue = function (methodName, filterExpression, functionHandler) {
        showLoadingPanel();
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetValue'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "methodName" : "' + methodName + '", "filterExpression" : "' + filterExpression + '"}',
            dataType: 'json',
            error: function (msg) {
                hideLoadingPanel();
                alert(msg.responseText);
            },
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            }
        });     //end ajax
    };

    this.getObjectValue = function (methodName, filterExpression, returnField, functionHandler) {
        showLoadingPanel();
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetObjectValue'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "methodName" : "' + methodName + '", "filterExpression" : "' + filterExpression + '", "returnField" : "' + returnField + '"}',
            dataType: 'json',
            error: function (msg) {
                hideLoadingPanel();
                alert(msg.responseText);
            },
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            }
        });     //end ajax
    };

    this.getObjectValueFromSession = function (sessionName, filterBy, filterValue, returnField, functionHandler, $row) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetObjectValueFromSession'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "sessionName" : "' + sessionName + '", "filterBy" : "' + filterBy + '", "filterValue" : "' + filterValue + '", "returnField" : "' + returnField + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                //alert(msg.d);
                if ($row != null)
                    window[functionHandler]($row, msg.d);
                else
                    window[functionHandler](msg.d);
            }
        });     //end ajax
    };

    this.getObjectFromSession = function (sessionName, filterBy, filterValue, functionHandler, $row) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetObjectFromSession'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "sessionName" : "' + sessionName + '", "filterBy" : "' + filterBy + '", "filterValue" : "' + filterValue + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                //alert(msg.d);
                if ($row != null)
                    window[functionHandler]($row, msg.d);
                else
                    window[functionHandler](msg.d);
            }
        });     //end ajax
    };

    this.getSessionValue = function (sessionName, functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetSessionValue'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "sessionName" : "' + sessionName + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };

    this.getCustomObject = function (listParentValue, selectedColumnID, selectedColumnName, valueColumn, valueColumnType, filterExpression, orderByExpression, objectTypeName, functionHandler) {
        showLoadingPanel();
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetCustomObject'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "listParentValue" : "' + listParentValue + '", "selectedColumnID" : "' + selectedColumnID + '", "selectedColumnName" : "' + selectedColumnName + '", "valueColumn" : "' + valueColumn + '", "valueColumnType" : "' + valueColumnType + '", "filterExpression" : "' + filterExpression + '", "orderByExpression" : "' + orderByExpression + '", "objectTypeName" : "' + objectTypeName + '"}',
            dataType: 'json',
            error: function (msg) {
                hideLoadingPanel();
                alert(msg.responseText);
            },
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion

    //#region Get Medication Sequence Time
    this.getMedicationSequenceTime = function (frequency, functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetMedicationTime'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "frequency" : "' + frequency + '"}',
            dataType: 'json',
            error: function (msg) {
                displayErrorMessageBox("GET MEDICATION SEQUENCE TIME", msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion
    //#region Status Outstanding Registration
    this.getStatusPerRegOutstanding = function (functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetStatusRegistrationOutstanding'),
            contentType: 'application/json; charset=utf-8',
            data: '',
            dataType: 'json',
            error: function (msg) {
                displayErrorMessageBox("GetStatusPerRegOutstanding ", msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion
    //#region Status Expierd License
    this.getExpiredLicense = function (functionHandler) {
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetExpiredLicense'),
            contentType: 'application/json; charset=utf-8',
            data: '',
            dataType: 'json',
            error: function (msg) {
                displayErrorMessageBox("GetExpiredLicense ", msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    //#endregion
    this.getHtmlControl = function (controlLocation, queryString, functionHandler, functionOnErrorHandler) {
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetControlHtml'),
            data: "{ controlLocation:'" + controlLocation + "', queryString:'" + queryString + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                functionHandler(msg.d);
            },
            fail: function (msg) {
                alert(msg);
                functionOnErrorHandler(msg.d);
            }
        });
    };

    this.getRequestBatchNo = function (businessPartnerName, date, functionHandler) {
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetMCUBatchNo'),
            data: "{ businessPartnerName :'" + businessPartnerName + "', date:'" + date + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });
    };

    this.getResultTypeByRequestBatchNo = function (requestBatchNo, functionHandler) {
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetMCUResultTypeByRequestBatchNo'),
            data: "{ requestBatchNo :'" + requestBatchNo + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });
    };

    this.onLoadPivot = function (requestBatchNo, resultType, date, functionHandler) {
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetPivotData'),
            data: "{ requestBatchNo :'" + requestBatchNo + "', resultType:'" + resultType + "', date:'" + date + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });
    };

    this.getUserControl = function (controlLocation, queryString, functionHandler, functionOnErrorHandler) {
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/MethodService.asmx/GetUserControl'),
            data: "{ controlLocation:'" + controlLocation + "', queryString:'" + queryString + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                functionHandler(msg.d);
            },
            fail: function (msg) {
                functionOnErrorHandler(msg.d);
            }
        });
    };

    this.getDateFromJSON = function (jsonDate) {
        return new Date(parseInt(jsonDate.substr(6)));
    };

    this.dateToYMD = function (date) {
        var d = date.getDate();
        var m = date.getMonth() + 1;
        var y = date.getFullYear();
        return '' + y + '-' + (m <= 9 ? '0' + m : m) + '-' + (d <= 9 ? '0' + d : d);
    }

    this.dateToDMY = function (date) {
        var d = date.getDate();
        var m = date.getMonth() + 1;
        var y = date.getFullYear();
        return '' + (d <= 9 ? '0' + d : d) + '-' + (m <= 9 ? '0' + m : m) + '-' + y;
    }

    this.stringToDate = function (value) {
        if (value != '') {
            var YYYY = value.substring(0, 4);
            var MM = value.substring(4, 6);
            var DD = value.substring(6);
            var date = new Date(parseInt(YYYY, 10), parseInt(MM, 10) - 1, parseInt(DD, 10));
            return date;
        }
        return new Date();
    }

    this.BPJSDateStringToDate = function (value) {
        if (value != '') {
            var tgl = value.split("-");
            var date = new Date(tgl[0], tgl[1] - 1, tgl[2]);

            var dateStr = padStr(date.getDate()) + '-' + padStr(1 + date.getMonth()) + '-' + padStr(date.getFullYear());
            return dateStr;
        }
        return new Date();
    }

    this.DatePickerToDateFormat = function (value) {
        if (value != '') {
            var YYYY = value.substring(6, 11);
            var MM = value.substring(3, 5);
            var DD = value.substring(0, 2);
            var date = YYYY + MM + DD;
            return date;
        }
        return new Date();
    }

    this.stringToDateTime = function (value) {
        if (value != '') {
            var YYYY = value.substring(0, 4);
            var MM = value.substring(4, 6);
            var DD = value.substring(6, 8);
            var HH = value.substring(8, 10);
            var mm = value.substring(10, 12);
            var date = new Date(parseInt(YYYY, 10), parseInt(MM, 10) - 1, parseInt(DD, 10), parseInt(HH, 10), parseInt(mm, 10));
            return date;
        }
        return new Date();
    }

    this.getDatePickerDate = function (value) {
        if (value != '') {
            var DD = value.substring(0, 2);
            var MM = value.substring(3, 5);
            var YYYY = value.substring(6, 10);
            var date = new Date(parseInt(YYYY, 10), parseInt(MM, 10) - 1, parseInt(DD, 10), 0, 0);
            return date;
        }
        return new Date();
    }
    this.dateToString = function (value) {
        var dateStr = padStr(value.getFullYear()) +
                  padStr(1 + value.getMonth()) +
                  padStr(value.getDate());
        return dateStr;
    }
    this.dateToDatePickerFormat = function (value) {
        var dateStr = padStr(value.getDate()) + '-' + padStr(1 + value.getMonth()) + '-' + padStr(value.getFullYear());
        return dateStr;
    }

    this.convertToDateTime = function (date, time) {
        var date = Methods.getDatePickerDate(date);
        var time = time.split(':');
        var dt = new Date(date.getFullYear(), date.getMonth(), date.getDate(), time[0], time[1]);
        return dt;
    }

    this.calculateEndDate = function (date, time, duration) {
        var date = Methods.getDatePickerDate(date);
        var time = time.split(':');
        var dt = new Date(date.getFullYear(), date.getMonth(), date.getDate(), time[0], time[1]);
        dt.setMinutes(dt.getMinutes() + parseInt(duration));
        return dt;
    }

    this.getTimeFromDate = function (value) {
        var now = new Date();
        var hour = ('0' + value.getHours()).slice(-2);
        var minute = ('0' + value.getMinutes()).slice(-2);

        return hour + ':' + minute;
    }

    function padStr(i) {
        return (i < 10) ? "0" + i : "" + i;
    }

    this.getJSONDateValue = function (jsonDate) {
        var date = new Date(parseInt(jsonDate.substr(6)));
        var dateStr = padStr(date.getDate()) + "-" +
            padStr(1 + date.getMonth()) + "-" +
            padStr(date.getFullYear());
        return dateStr;
    }

    this.ExecuteFunction = function (fn, s) {
        fn(s);
    }

    this.daysInMonth = function (month, year) {
        return new Date(year, month, 0).getDate();
    }

    this.getAgeFromDatePickerFormat = function (dob) {
        var date = Methods.getDatePickerDate(dob);
        var diffDate = Methods.calculateDateDifference(date, todayDate);
        return diffDate;
    }

    this.calculateDateDifference = function (d1, d2) {
        var years, months, days = 0;
        var temp = new Date();
        temp.setDate(d2.getDate());
        days = temp.getDate() - d1.getDate();
        months = temp.getMonth() - d1.getMonth();
        years = temp.getFullYear() - d1.getFullYear();
        if (temp.getMonth() < d1.getMonth()) {
            years--;
            months += 12;
        }
        //        if (d2.getDate() < d1.getDate()) {
        //            months--;
        //            if (d2.getMonth() > 0)
        //                days += new Date(d2.getFullYear(), d2.getMonth(), 0).getDate();
        //            else
        //                days += new Date(d2.getFullYear() - 1, 12, 0).getDate();
        //            if (months < 0) {
        //                years--;
        //                months += 12;
        //            }
        //        }
        while (days < 0) {
            months--;
            if (temp.getMonth() > 0)
                days += new Date(temp.getFullYear(), temp.getMonth(), 0).getDate();
            else
                days += new Date(temp.getFullYear() - 1, 12, 0).getDate();
            if (months < 0) {
                years--;
                months += 12;
            }
        }
        return { "years": years, "months": months, "days": days };
    }

    this.calculateDateTimeDifference = function (d1, d2) {
        var diff = (d2.getTime() - d1.getTime()) / 1000;
        diff /= (60 * 60);
        return Math.abs(Math.round(diff));
    }

    this.checkImageError = function (className, type, gender) {
        setTimeout(function () {
            if (type == 'patient') {
                var imgUrl = ResolveUrl("~/Libs/Images/patient.png");
                var imgUrlM = ResolveUrl("~/Libs/Images/patient_male.png");
                var imgUrlF = ResolveUrl("~/Libs/Images/patient_female.png");
                $('.' + className).each(function () {
                    if (this.src != document.URL) {
                        if (this.src == '') {
                            if (gender == '0003^F')
                                this.src = imgUrlF;
                            else if (gender == '0003^M')
                                this.src = imgUrlM;
                            else
                                this.src = imgUrl;
                        }
                        else {
                            $(this).error(function () {
                                if (gender == '0003^F')
                                    this.src = imgUrlF;
                                else if (gender == '0003^M')
                                    this.src = imgUrlM;
                                else
                                    this.src = imgUrl;
                            }).attr('src', this.src);
                        }
                    }
                });
            }
            else if (type == 'paramedic') {
                var imgUrl = ResolveUrl("~/Libs/Images/physician.png");
                $('.' + className).each(function () {
                    if (this.src != document.URL) {
                        if (this.src == '') {
                            this.src = imgUrl;
                        }
                        else {
                            $(this).error(function () {
                                this.src = imgUrl;
                            }).attr('src', this.src);
                        }
                    }
                });
            }
            else if (type == 'businesspartner') {
                var imgUrl = ResolveUrl("~/Libs/Images/businesspartner.png");
                $('.' + className).each(function () {
                    if (this.src != document.URL) {
                        if (this.src == '') {
                            this.src = imgUrl;
                        }
                        else {
                            $(this).error(function () {
                                this.src = imgUrl;
                            }).attr('src', this.src);
                        }
                    }
                });
            }
        }, 0);
    }

})();

(function ($) {
    $.fn.rptTemplate = function (idInputHdn, clickHandler) {
        var id = this[0].id;
        $('#' + idInputHdn).val('');
        $('#' + id + ' .repeaterDataItemTemplate').live('click', function () {
            $(this).parent().children().attr('class', 'repeaterDataItemTemplate notSelected');
            $(this).attr('class', 'repeaterDataItemTemplate selected');

            var idValue = $(this).find('.repeaterDataItemID').val();
            $('#' + idInputHdn).val(idValue);
            if (clickHandler != null) {
                window[clickHandler](idValue);
            }
        });
    };
})(jQuery);