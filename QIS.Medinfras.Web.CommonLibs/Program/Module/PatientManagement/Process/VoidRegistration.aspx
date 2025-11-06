<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="VoidRegistration.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.VoidRegistration" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessVoidRegistration" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function setCustomToolbarVisibility() {
            var isAllowVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();
            if (isAllowVoid != "0") {
                $('#<%=btnProcessVoidRegistration.ClientID %>').hide();
            } else {
                $('#<%=btnProcessVoidRegistration.ClientID %>').show();
            }
        }

        function onLoad() {
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').change(function () {
                onRefreshGridView();
            });

            $('#<%=btnProcessVoidRegistration.ClientID %>').click(function () {
                var errMessage = { text: '' };
                getCheckedMember(errMessage);
                if (errMessage.text == '') {
                    if ($('#<%=hdnSelectedRegistrationID.ClientID %>').val() == '') {
                        showToast('Warning', 'Please Select Registration First');
                    }
                    else {
                        showDeleteConfirmation(function (data) {
                            var param = 'void;' + data.GCDeleteReason + ';' + data.Reason;
                            onCustomButtonClick('voidregistration');
                        });
                    }
                }
            });
        }

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                cbpView.PerformCallback('refresh');
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
            }, 0);
        }

        $(function () {
            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });
        });

        function onAfterCustomClickSuccess(type) {
            showToast('INFORMATION', 'Process void success !', function () {
                onRefreshGridView();
            });
            setCustomToolbarVisibility();
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        function getCheckedMember(errMessage) {
            var lstRegistrationID = $('#<%=hdnSelectedRegistrationID.ClientID %>').val().split(',');
            $('.grdView .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.hdnKeyField').val().trim();
                    var idx = lstRegistrationID.indexOf(key);
                    if (idx < 0) {
                        lstRegistrationID.push(key);
                    }
                }
                else {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.hdnKeyField').val().trim();
                    var idx = lstRegistrationID.indexOf(key);
                    if (idx > -1) {
                        lstRegistrationID.splice(idx, 1);
                    }
                }
            });

            $('#<%=hdnSelectedRegistrationID.ClientID %>').val(lstRegistrationID.join(','));
        }

    </script>
    <input type="hidden" value="" id="hdnMenggunakanValidasiChiefComplaint" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" id="hdnIsBridgingToGateway" runat="server" value="" />
    <input type="hidden" id="hdnProviderGatewayService" runat="server" value="" />
    <input type="hidden" id="hdnIsVoidRegistrationDeleteLinkedReg" runat="server" value="" />
    <table class="tblContentArea" style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <fieldset id="fsPatientList">
                    <table>
                        <colgroup>
                            <col style="width: 160px" />
                        </colgroup>
                        <tr id="trServiceUnit" runat="server">
                            <td>
                                <%=GetServiceUnitLabel()%>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboServiceUnit" Width="400px" ClientInstanceName="cboKlinik"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onRefreshGridView(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trRegistrationDate" runat="server">
                            <td>
                                <%=GetLabel("Tanggal") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Quick Filter")%></label>
                            </td>
                            <td>
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    Width="400px" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                        <qis:QISIntellisenseHint Text="No Register" FieldName="RegistrationNo" />
                                        <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                        <%=GetLabel("Halaman Ini Akan")%>
                        <span class="lblLink" id="lblRefresh">[refresh]</span>
                        <%=GetLabel("Setiap")%>
                        <%=GetRefreshGridInterval() %>
                        <%=GetLabel("Menit")%>
                    </div>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td>
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 10px" align="center">
                                                </th>
                                                <th style="width: 400px">
                                                    <%=GetLabel("No. Registrasi") %>
                                                </th>
                                                <th style="width: 400px">
                                                    <%=GetLabel("Informasi Registrasi") %>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Informasi Pasien") %>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="10">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect lvwView" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 10px" align="center">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th style="width: 400px">
                                                    <%=GetLabel("No. Registrasi") %>
                                                </th>
                                                <th style="width: 400px">
                                                    <%=GetLabel("Informasi Registrasi") %>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Informasi Pasien") %>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </td>
                                            <td class="tdRoleID">
                                                <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                <b>
                                                    <%#: Eval("RegistrationNo")%></b>
                                                <br />
                                                <i>
                                                    <%#: Eval("ParamedicName")%></i>
                                            </td>
                                            <td>
                                                <div>
                                                    <b>
                                                        <%#: Eval("ServiceUnitName")%></b>
                                                    <br />
                                                    <%=GetLabel("Ruang & Bed : ") %><%#: Eval("RoomName")%>
                                                    <%#: Eval("BedCode")%>
                                                </div>
                                            </td>
                                            <td>
                                                <b>
                                                    <%#: Eval("PatientName")%></b>
                                                <br />
                                                <i>
                                                    <%#: Eval("MedicalNo")%></i>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="paging">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
