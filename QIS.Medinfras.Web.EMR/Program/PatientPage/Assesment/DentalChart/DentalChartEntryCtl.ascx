<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DentalChartEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.DentalChartEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    setDatePicker('<%=txtTreatmentDate.ClientID %>');
    $('#<%=txtTreatmentDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
    
    setDatePicker('<%=txtNextDate.ClientID %>');
    $('#<%=txtNextDate.ClientID %>').datepicker('option', 'minDate', '0');

    $(function () {
        var type = $('#<%=imgToothSurface.ClientID %>').attr('type');
        var toothSurfaces = $('#<%=hdnToothSurfaces.ClientID %>').val();
        if (toothSurfaces != "") {
            var lstToothSurfaces = $('#<%=hdnToothSurfaces.ClientID %>').val().split(';');
            var className = 'imgArea';
            if (type == '2')
                className = 'imgArea2';
            for (var i = 0; i < lstToothSurfaces.length; ++i) {
                $area = $('.' + className + "[val='" + lstToothSurfaces[i] + "']");
                var data = $area.mouseout().data('maphilight') || {};
                data.alwaysOn = true;
                $area.data('maphilight', data).trigger('alwaysOn.maphilight');
            }
        }

        $('.map').maphilight({
            strokeColor: '000000',
            strokeWidth: 2,
            fillColor: 'F4921B',
            fillOpacity: 0.6
        });
        $('.imgArea, .imgArea2').click(function (e) {
            e.preventDefault();
            var data = $(this).mouseout().data('maphilight') || {};
            data.alwaysOn = !data.alwaysOn;
            $(this).data('maphilight', data).trigger('alwaysOn.maphilight');
            setToothSurfaces();
        });

        function setToothSurfaces() {
            var className = 'imgArea';
            if (type == '2')
                className = 'imgArea2';

            var toothSurfaces = "";
            $('.' + className).each(function () {
                var data = $(this).mouseout().data('maphilight') || {};
                if (data.alwaysOn) {
                    if (toothSurfaces != '')
                        toothSurfaces += ';';
                    toothSurfaces += $(this).attr('val');
                }
            });
            $('#<%=hdnToothSurfaces.ClientID %>').val(toothSurfaces);
        }
    });

    function onBeforeSaveRecordEntryPopup() {
        return ledProcedure.Validate();
    }

    function onLedProcedureInit(led) {
        var procedureID = $('#<%=hdnProcedureID.ClientID %>').val();
        if (procedureID != '') {
            led.SetValue(procedureID);
        }

    }

    function onLedProcedureLostFocus(value) {
        $('#<%=hdnProcedureID.ClientID %>').val(value);
    }
</script>
<div>
    <input type="hidden" runat="server" id="hdnToothSurfaces" />
    <input type="hidden" runat="server" id="hdnPatientDentalID" />
    <input type="hidden" runat="server" id="hdnToothID" />
    <table class="tblContentArea">
        <tr>
            <td style="padding:5px;vertical-align:top;">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col width="125px" />
                        <col width="130px" />
                        <col width="100px" />
                        <col width="70px" />
                        <col width="280px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Date")%> - <%=GetLabel("Time")%></label></td>
                        <td><asp:TextBox ID="txtTreatmentDate" Width="100px" CssClass="datepicker" runat="server" /></td>
                        <td><asp:TextBox ID="txtTreatmentTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                        <td align="center"><b><%=GetLabel("Surface") %></b></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Physician")%></label></td>
                        <td colspan="2"><dxe:ASPxComboBox ID="cboParamedicID" Width="200px" runat="server" /></td>
                        <td rowspan="2" style="text-align:center">
                            <div style="width:60px;margin:0 auto;">
                                <img id="imgToothSurface" runat="server" border="0" class="map" width="60" height="60" alt="" />
                            </div>
                            <map name="maptooth">
                                <area class="imgArea" id="mapToothTop" runat="server" shape="poly" style="display:none;" coords="1,1,15,15,45,15,59,1" alt="arrow" />
                                <area class="imgArea" id="mapToothLeft" runat="server" shape="poly" style="display:none;" coords="1,1,15,15,15,45,1,59" alt="arrow" />
                                <area class="imgArea" id="mapToothRight" runat="server" shape="poly" style="display:none;" coords="59,1,45,15,45,45,59,59" alt="arrow" />
                                <area class="imgArea" id="mapToothBottom" runat="server" shape="poly" style="display:none;" coords="1,59,15,45,45,45,59,59" alt="arrow" />
                                <area class="imgArea" id="mapToothCenter" runat="server" shape="poly" style="display:none;" coords="15,15,15,45,45,45,45,15" alt="arrow" />
                            </map>                
                            <map name="maptooth2">
                                <area class="imgArea2" id="mapToothTop2" runat="server" shape="poly" style="display:none;" coords="1,1,15,28,45,28,59,1" alt="arrow" />
                                <area class="imgArea2" id="mapToothLeft2" runat="server" shape="poly" style="display:none;" coords="1,1,15,28,15,32,1,59" alt="arrow" />
                                <area class="imgArea2" id="mapToothRight2" runat="server" shape="poly" style="display:none;" coords="59,1,45,28,45,32,59,59" alt="arrow" />
                                <area class="imgArea2" id="mapToothBottom2" runat="server" shape="poly" style="display:none;" coords="1,59,15,32,45,32,59,59" alt="arrow" />   
                                <area class="imgArea2" id="mapToothCenter2" runat="server" shape="poly" style="display:none;" coords="15,28,15,32,45,32,45,28" alt="arrow" />         
                            </map>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tooth")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtTooth" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tooth Problem")%></label></td>
                        <td colspan="2"><dxe:ASPxComboBox ID="cboToothProblem" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Current Status")%></label></td>
                        <td colspan="2"><dxe:ASPxComboBox ID="cboCurrentStatus" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Procedures")%></label></td>
                        <td colspan="4">
                            <input type="hidden" value="" id="hdnProcedureID" runat="server" />
                            <qis:QISSearchTextBox ID="ledProcedure" ClientInstanceName="ledProcedure" runat="server" Width="99%"
                                ValueText="ProcedureID" FilterExpression="1 = 0" DisplayText="ProcedureName" MethodName="GetvSpecialtyProceduresList" >
                                <ClientSideEvents ValueChanged="function(s){ onLedProcedureLostFocus(s.GetValueText()); }" 
                                    Init="function(s){ onLedProcedureInit(s); }" />
                                <Columns>
                                    <qis:QISSearchTextBoxColumn Caption="Procedure Code" FieldName="ProcedureID" Description="i.e. 1-16" Width="100px" />
                                    <qis:QISSearchTextBoxColumn Caption="Procedure Name" FieldName="ProcedureName" Description="i.e. Consultation" Width="300px" />
                                </Columns>
                            </qis:QISSearchTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Next Status")%></label></td>
                        <td colspan="2"><dxe:ASPxComboBox ID="cboNextStatus" Width="200px" runat="server" /></td>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Next Date")%></label></td>
                        <td><asp:TextBox ID="txtNextDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Notes")%></label></td>
                        <td colspan="4"><asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
