<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="BloodBankOrderRealizationEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Laboratory.Program.BloodBankOrderRealizationEntry" %>

<%@ Register Src="~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientSOAPToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessSaveRecord" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">

        $(function () {
            $('#<%=btnProcessSaveRecord.ClientID %>').click(function () {
                var param = '';
                $('.chkIsExist input').each(function () {
                    var id = $(this).closest('tr').find('.keyField').html();
                    $isComplete = $(this).closest('tr').find('.chkIsCompleted').find('input');
                    if (param != '') {
                        param += '|';
                    }
                    param += id + ',';
                    if ($(this).is(':checked'))
                        param += '1';
                    else
                        param += '0';
                    param += ','
                    if ($isComplete.is(':checked'))
                        param += '1';
                    else
                        param += '0';
                    param += ','
                    $cboType = $(this).closest('tr').find('.cboNotesType');
                    if ($cboType.val() != "") {
                        param += $cboType.val();
                    }
                    param += ','
                    $txtMRStatusNoteOthers = $(this).closest('tr').find('.txtMRStatusNoteOthers');
                    if ($txtMRStatusNoteOthers.val() != "") {
                        param += $txtMRStatusNoteOthers.val();
                    }
                    param += ','
                    $txtRemarks = $(this).closest('tr').find('.txtRemarks');
                    if ($txtRemarks.val() != "") {
                        param += $txtRemarks.val();
                    }
                    param += ','
                    var $txtFormDate = $(this).closest('tr').find('.txtFormDate');
                    var formDate = $txtFormDate.val();
                    if ($txtFormDate.val() != "") {
                        param += $txtFormDate.val();
                    }
                    param += ','
                    var $txtFormTime = $(this).closest('tr').find('.txtFormTime');
                    if ($txtFormTime.val() != "") {
                        param += $txtFormTime.val();
                    }
                });
                $('#<%=hdnParam.ClientID %>').val(param);
                onCustomButtonClick('approve');
            });
            $('.chkIsExist input').each(function () {
                $(this).change();
            });
        });

        $('.chkIsExist input').live('change', function () {
            $tr = $(this).closest('tr');
            $chkHdID = $(this).closest('tr').find('.chkIsCompleted').find('input');
            $txtRemarks = $tr.find('.txtRemarks');
            $cboType = $tr.find('.cboNotesType');
            $txtMRStatusNoteOthers = $tr.find('.txtMRStatusNoteOthers');
            $txtFormDate = $tr.find('.txtFormDate');
            $txtFormTime = $tr.find('.txtFormTime');
            if ($(this).is(':checked')) {
                $chkHdID.removeAttr("disabled");
                $txtFormDate.removeAttr('readonly');
                $txtFormTime.removeAttr('readonly');
                if ($txtFormTime.val() == '') {
                    $txtFormTime.val('00:00');
                }
                setDatePickerElement($txtFormDate);
                $txtFormDate.datepicker('option', 'maxDate', '0');

            }
            else {
                $chkHdID.attr("disabled", true);
                $chkHdID.prop('checked', false);
                $txtRemarks.attr('readonly', 'readonly');
                $cboType.attr("disabled", true);
                $txtMRStatusNoteOthers.attr('readonly', 'readonly');
                $txtFormDate.attr('readonly', 'readonly');
                $txtFormTime.attr('readonly', 'readonly');
                $txtFormDate.removeClass("datepicker");
                $txtFormDate.val('');
                $txtFormTime.val('');
            }
        });

        $('.chkIsCompleted input').live('change', function () {
            $tr = $(this).closest('tr');
            $txtRemarks = $tr.find('.txtRemarks');
            $cboType = $tr.find('.cboNotesType');
            $txtMRStatusNoteOthers = $tr.find('.txtMRStatusNoteOthers');
            $txtFormTime = $tr.find('.txtFormTime');
            if ($(this).is(':checked')) {
                $txtRemarks.removeAttr('readonly');
                $cboType.attr("disabled", false);
                $txtMRStatusNoteOthers.removeAttr('readonly');
            }
            else {
                $txtRemarks.attr('readonly', 'readonly');
                $cboType.attr("disabled", true);
                $txtMRStatusNoteOthers.attr('style', 'display:none');
                $cboType.val('');
            }
        });


        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function onCboDepartmentChanged() {
            cbpView.PerformCallback('refresh');
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            $('.chkIsExist input').each(function () {
                $(this).change();
            });
        }

        $(function () {
            $("#<%=rblCheckAll.ClientID %> input").change(function () {
                var value = $(this).val();
                if (value == 1) {
                    $(".chkIsExist input").each(function () {
                        $(this).attr("disabled", false);
                        $(this).prop('checked', true);
                    });
                    $(".txtFormDate").each(function () {
                        $(this).removeAttr('readonly');
                        setDatePickerElement($(this));
                        $(this).datepicker('enable');
                    });
                    $(".txtFormTime").each(function () {
                        $(this).removeAttr('readonly');
                    });
                    $(".chkIsCompleted input").each(function () {
                        $(this).attr("disabled", false);
                        $(this).prop('checked', true);
                    });
                    $(".cboNotesType").each(function () {
                        $(this).attr("disabled", false);
                    });
                    $(".txtMRStatusNoteOthers").each(function () {
                        $(this).removeAttr('readonly');
                    });
                    $(".txtRemarks").each(function () {
                        $(this).removeAttr('readonly');
                    });
                }
                else if (value == 2) {
                    $(".chkIsExist input").each(function () {
                        $(this).attr("disabled", false);
                        $(this).prop('checked', true);
                    });
                    $(".txtFormDate").each(function () {
                        $(this).removeAttr('readonly');
                        setDatePickerElement($(this));
                        $(this).datepicker('enable');
                    });
                    $(".txtFormTime").each(function () {
                        $(this).removeAttr('readonly');
                    });
                    $(".chkIsCompleted input").each(function () {
                        $(this).attr("disabled", false);
                        $(this).prop('checked', true);
                    });
                    $(".cboNotesType").each(function () {
                        $(this).attr("disabled", false);
                    });
                    $(".txtMRStatusNoteOthers").each(function () {
                        $(this).attr('style', 'display:none');
                    });
                    $(".txtRemarks").each(function () {
                        $(this).removeAttr('readonly');
                    });
                }
                else if (value == 3) {
                    $(".chkIsExist input").each(function () {
                        $(this).attr("disabled", true);
                        $(this).prop('checked', false);
                    });
                    $(".txtFormDate").each(function () {
                        $(this).attr('readonly', 'readonly');
                        setDatePickerElement($(this));
                        $(this).datepicker('disable');
                        if ($(this).val() != '') {
                            $(this).val('');
                        }
                    });
                    $(".txtFormTime").each(function () {
                        $(this).attr('readonly', 'readonly');
                        if ($(this).val() != '') {
                            $(this).val('');
                        }
                    });
                    $(".chkIsCompleted input").each(function () {
                        $(this).attr("disabled", true);
                        $(this).prop('checked', false);
                    });
                    $(".cboNotesType").each(function () {
                        $(this).attr("disabled", true);
                        if ($(this).val() != '') {
                            $(this).val('');
                        }
                    });
                    $(".txtMRStatusNoteOthers").each(function () {
                        $(this).attr('style', 'display:none');
                        if ($(this).val() != '') {
                            $(this).val('');
                        }
                    });
                    $(".txtRemarks").each(function () {
                        $(this).attr('readonly', 'readonly');
                        if ($(this).val() != '') {
                            $(this).val('');
                        }
                    });
                }
            });
        });

        function cboMedicalFileStatusChanged() {

            var value = cboMedicalFileStatus.GetValue();
            cboReason.SetValue('<%=GetReasonBerkasKurangLengkap()%>');
            if (value == '<%=GetMedicalFileStatusReturnToPhysician()%>') {
                trParamedic.style.display = "table-row";
                trReason.style.display = "table-row";
            } else {
                trParamedic.style.display = "none";
                trReason.style.display = "none";
                trRemarks.style.display = "none";
            }
        }

        function cboReasonChanged() {
            var value = cboReason.GetValue();
            if (value == '<%=GetReasonOther()%>') {
                trRemarks.style.display = "table-row";
            } else {
                trRemarks.style.display = "none";
            }
        }

        $('.cboNotesType').live('change', function () {
            $tr = $(this).closest('tr');
            var val = $(this).val();
            if (val != "X222^99") {
                $txtMRStatusNoteOthers = $tr.find('.txtMRStatusNoteOthers');
                $txtMRStatusNoteOthers.attr('style', 'display:none');
                $txtMRStatusNoteOthers.val('');
            }
            else {
                $txtMRStatusNoteOthers = $tr.find('.txtMRStatusNoteOthers');
                $txtMRStatusNoteOthers.removeAttr('style', 'display:none');
            }
        });
    </script>
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnCheckedIsExist" runat="server" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 60%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr id="trServiceUnit" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jenis Analisa Berkas")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboFolderType" Width="300px" ClientInstanceName="cboFolderType"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboDepartmentChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblCheckAll" CssClass="rblCheckAll" runat="server" RepeatDirection="Vertical">
                                    <asp:ListItem Text="Centang Semua (Ada dan Lengkap)" Value="1" />
                                    <asp:ListItem Text="Centang Semua (Ada dan Tidak Lengkap)" Value="2" />
                                    <asp:ListItem Text="Centang Semua (Tidak Ada dan Tidak Lengkap)" Value="3" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table width="90%">
                        <colgroup>
                            <col style="width: 50%" />
                        </colgroup>
                        <tr>
                            <td valign="top">
                                <%=GetLabel("Status Rekam Medis")  %>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboMedicalFileStatus" ClientInstanceName="cboMedicalFileStatus"
                                    Width="275px">
                                    <ClientSideEvents ValueChanged="function(s,e){cboMedicalFileStatusChanged()}" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trParamedic" style="display: none;">
                            <td>
                                <%=GetLabel("Nama Dokter")%>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtParamedicName" Width="272px" />
                            </td>
                        </tr>
                        <tr id="trReason" style="display: none;">
                            <td>
                                <%=GetLabel("Alasan Pengembalian") %>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboReason" ClientInstanceName="cboReason" Width="200px">
                                    <ClientSideEvents ValueChanged="function(s,e){cboReasonChanged()}" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trRemarks" style="display: none;">
                            <td valign="top">
                                <%=GetLabel("Alasan Lain-lain")%>
                            </td>
                            <td id="tdRemarks" runat="server">
                                <asp:TextBox runat="server" ID="txtRemarks" Width="100%" TextMode="MultiLine" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=GetLabel("Pembawa Berkas")%>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtTransporterName" Width="272px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="FormID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="FormCode" HeaderText="Kode Berkas" HeaderStyle-Width="80px"
                                                    HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="FormName" HeaderStyle-CssClass="formName" ItemStyle-CssClass="formName"
                                                    HeaderText="Nama Berkas" HeaderStyle-Width="400px" HeaderStyle-HorizontalAlign="Left" />
                                                <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderText="Ada">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsExist" CssClass="chkIsExist" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderText="Tanggal Berkas">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFormDate" Width="120px" runat="server" Style="text-align: center"
                                                            CssClass="txtFormDate datepicker" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderText="Jam Berkas">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFormTime" runat="server" Width="60px" CssClass="txtFormTime"
                                                            Style="text-align: center" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderText="Lengkap">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsCompleted" CssClass="chkIsCompleted" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderText="Jenis Catatan">
                                                    <ItemTemplate>
                                                        <asp:DropDownList runat="server" ID="cboMRStatusNotes" CssClass="cboNotesType" Width="100%">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtMRStatusNoteOthers" CssClass="txtMRStatusNoteOthers" Width="100%"
                                                            runat="server" Style="display: none" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderText="Catatan Tambahan">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemarks" CssClass="txtRemarks" Width="100%" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("No Data To Display")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table width="100%">
            <tr>
                <td>
                    <div style="width: 600px;">
                        <div class="pageTitle" style="text-align: center">
                            <%=GetLabel("Informasi")%></div>
                        <div style="background-color: #EAEAEA;">
                            <table width="600px" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="150px" />
                                    <col width="30px" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Pemeriksa Terakhir")%></label>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtLastCheckedInfo" Width="300px" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Dibuat Pada")%></label>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtLastCheckedDateInfo" Width="300px" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Status Terakhir")%></label>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtMedicalFileStatus" Width="300px" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr id="trReturnToName" runat="server" style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Dikembalikan ke")%></label>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtReturnToName" Width="300px" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Alasan")%></label>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtReturnReason" Width="300px" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
