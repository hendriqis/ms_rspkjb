<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SymptomInfoCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.SymptomInfoCtl" %>

<div>
    <script id="dxis_SymptomInfoCtl1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery.maphilight.js")%>' type='text/javascript'></script>
    <script id="dxis_SymptomInfoCtl2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery.tmpl.min.js")%>' type='text/javascript'></script>
    <script type="text/javascript" id="dxss_SymptomInfoCtl">
        $(function () {
            $('.map').maphilight({
                strokeColor: 'AAAAAA',
                strokeWidth: 1,
                fillColor: 'F4921B',
                fillOpacity: 0.6
            });

            $('.imgAreaSymptomInfo').click(function (e) {
                e.preventDefault();

                $('.imgAreaSymptomInfo').each(function () {
                    var data = $(this).mouseout().data('maphilight') || {};
                    data.alwaysOn = false;
                    $(this).data('maphilight', data).trigger('alwaysOn.maphilight');
                });
                var data = $(this).mouseout().data('maphilight') || {};
                data.alwaysOn = true;
                $(this).data('maphilight', data).trigger('alwaysOn.maphilight');
                $('#listSymptomInfo a').html($(this).attr('title'));

                $("#listSymptomInfo div ul").empty();
                $("#listSymptomInfo div img").show();
                var GCBodySystem = $(this).attr('GCBodySystem');
                loadSymptomInfo(GCBodySystem);
            });

            function loadSymptomInfo(GCBodySystem) {
                var filterExpression = "GCBodySystem = '" + GCBodySystem + "' ORDER BY SymptomName ASC";
                Methods.getListObject("GetvSystemSymptomList", filterExpression, function (result) {
                    $div = $("#listSymptomInfo div");
                    $ul = $div.find('ul');
                    $div.find('img').hide();
                    $("#symptomInfoTmpl").tmpl(result).appendTo($ul);                
                });
            }

            $("#listSymptomInfo div ul li").live('click', function () {
                $("#listSymptomInfoDiagnose div ul").empty();
                $("#listSymptomInfoDiagnose div img").show();

                var filterExpression = 'SymptomID = ' + $(this).attr('SymptomID');
                Methods.getListObject("GetvSymptomDiagnosisList", filterExpression, function (result) {
                    $div = $("#listSymptomInfoDiagnose div");
                    $ul = $div.find('ul');
                    $div.find('img').hide();
                    $("#symptomInfoDiagnoseTmpl").tmpl(result).appendTo($ul);
                });

            });
        });
    </script>
    <style type="text/css">
        .accordionSymptomInfo  {
	        width: 100%;
	        margin:0;
	        border: 1px solid black;
        }
        .accordionSymptomInfo div {
	        background-color: #fff;
	        position:relative;
	        overflow-y:auto;
            height: 410px;
        }
        .accordionSymptomInfo div img {
	        position:absolute;
	        left:44%;
        }
        .accordionSymptomInfo div ul 
        {
            margin:0;
            padding:0;
            padding-left:2px;
        }
        .accordionSymptomInfo div ul li {
	        list-style-type: none;
	        vertical-align:middle;
	        font-size:14px;
        }   
        .accordionSymptomInfo div ul li:hover {
            background-color:#F4921B
        }     
        .symptomNameTmpl
        {
            padding-left:5px;
        }
        .accordionSymptomInfo div ul li span
        {
            vertical-align:middle;
            display:table-cell;
        }

        .accordionSymptomInfo p {
	        margin-bottom : 10px;
	        border: none;
	        text-decoration: none;
	        font-weight: bold;
	        font-size: 10px;
	        margin: 0px;
	        padding: 10px;
        }
        .accordionSymptomInfo a {
	        cursor:pointer;
	        display:block;
	        padding:2px;
	        margin-top: 0;
	        text-decoration: none;
	        font-weight: bold;
	        font-size: 11px;
	        color: black;
	        background-color: #9CC525;
	        border-top: 1px solid #FFFFFF;
	        border-bottom: 1px solid #999;	
        }
        #containerSymptomInfo {
            padding: 10px;
            width: 350px;
            height: 450px;
            margin-bottom:10px;
        }
    </style>
    <div>
        <script id="symptomInfoTmpl" type="text/x-jquery-tmpl"> 
            <li SymptomID="${SymptomID}">${SymptomName}</li>
        </script>
        <script id="symptomInfoDiagnoseTmpl" type="text/x-jquery-tmpl"> 
            <li>${DiagnoseName}</li>
        </script>

        
        <table>
            <tr>
                <td style="width:200px;padding-left:50px;">
                    <img runat="server" id="imgFront" border="0" class="map">
                </td>
                <td>
                    <div style="width:200px;">
                        <img runat="server" id="imgBack" border="0" class="map">
                    </div>                    
                </td>
                <td style="vertical-align:top;width:400px;padding-left:100px">
                    <div id="containerSymptomInfo" style="padding:0">
                        <div class="accordionSymptomInfo borderBox" id="listSymptomInfo">
                            <a>Symptom</a>
                            <div>
                                <img style="display:none" src='<%= ResolveUrl("~/Libs/Images/Loading.gif")%>' alt="" />
                                <ul></ul>
                            </div>
	                    </div>
	                </div>
                </td>
                <td style="vertical-align:top;width:300px;padding-left:10px">
                    <div id="containerSymptomInfoDiagnose" style="padding:0">
                        <div class="accordionSymptomInfo borderBox" id="listSymptomInfoDiagnose">
                            <a>Diagnose</a>
                            <div>
                                <img style="display:none" src='<%= ResolveUrl("~/Libs/Images/Loading.gif")%>' alt="" />
                                <ul></ul>
                            </div>
	                    </div>
	                </div>
                </td>
            </tr>
        </table>
    </div>

    <map name="symptominfofrontmale">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^001" title="Head" id="male_head_front" shape="poly" coords="97,5 , 105,5 , 108,9 , 111,9 , 116,14 , 118,18 , 119,21 , 118,27 , 118,32 , 120,35 , 121,39 , 121,42 , 120,45 , 119,46 , 117,48 , 115,46 , 114,52 , 112,55 , 110,57 , 108,59 , 107,59 , 105,60 , 103,61 , 102,61 , 98,62 , 93,61 , 91,61 , 89,59 , 87,57 , 86,55 , 85,53 , 84,51 , 82,46 , 80,48 , 78,46 , 78,42 , 77,35 , 79,32 , 79,20 , 81,15 , 85,11 , 90,8 , 97,5">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^002" title="Neck" id="male_neck_front" shape="poly" coords="114,53 , 113,62 , 116,68 , 118,71 , 122,74 , 126,76 , 132,80 , 133,81 , 123,85 , 119,83 , 105,83 , 100,87 , 99,87 , 95,84 , 77,84 , 67,80 , 71,76 , 75,74 , 80,71 , 84,67 , 84,53 , 87,56 , 88,58 , 90,60 , 92,61 , 97,62 , 99,62 , 103,61 , 107,59 , 112,56  ">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^006" title="Chest" id="male_chest_front" shape="poly" coords="134,81 , 137,82 , 140,82 , 143,83 , 147,85 , 147,86 , 145,108 , 144,115 , 143,125 , 142,135 , 134,149 , 132,161 , 66,161 , 64,149 , 60,142 , 56,137 , 56,134 , 57,131 , 56,127 , 56,125 , 53,106 , 53,104 , 51,98 , 49,87 , 53,85 , 58,82 , 61,81 , 66,80 , 68,81 , 75,84 , 94,84 , 99,87 , 104,83 , 120,83 , 123,85 ">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^007" title="Abdomen" id="male_abdomen_front" shape="poly" coords="132,161 , 132,168 , 134,176 , 135,180 , 135,186 , 135,195 , 134,195 , 123,206 , 116,210 , 111,213 , 106,214 , 102,215 , 97,215 , 91,215 , 87,212 , 83,210 , 80,209 , 77,206 , 74,204 , 71,200 , 68,195 , 65,191 , 63,188 , 64,178 , 66,172 , 66,161 ">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^011" title="Pelvis" id="male_pelvis_front" shape="poly" coords="122,207 , 116,215 , 114,218 , 112,220 , 109,223 , 106,226 , 106,231 , 106,234 , 106,237 , 105,240 , 103,243 , 102,246 , 100,248 , 97,248 , 95,247 , 94,241 , 92,238 , 91,233 , 91,228 , 91,226 , 88,223 , 85,220 , 81,215 , 78,210 , 75,205 , 76,206 , 78,208 , 80,209 , 82,210 , 85,211 , 87,212 , 90,214 , 93,215 , 100,215 , 104,215 , 108,214 , 114,212 ">
        
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^012" title="Right Hip" id="male_right_hip_front" shape="poly" coords="63,189 , 65,190 , 67,193 , 70,198 , 72,201 , 74,205 , 76,207 , 79,213 , 82,217 , 85,221 , 87,223 , 85,239 , 83,247 , 79,256 , 74,268 , 70,273 , 68,276 , 61,279 , 55,281 , 54,282 , 53,272 , 53,260 , 52,250 , 53,237 , 54,226 , 57,213 , 61,199 ">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^013" title="Right Leg" id="male_right_leg_front" shape="poly" coords="87,223 , 91,226 , 91,237 , 94,242 , 95,247 , 96,249 , 97,249 , 94,271 , 92,281 , 92,291 , 92,300 , 92,307 , 91,310 , 90,312 , 90,316 , 90,320 , 90,321 , 88,324 , 86,328 , 85,332 , 85,336 , 85,341 , 86,350 , 87,358 , 87,363 , 87,368 , 86,372 , 84,377 , 82,383 , 81,387 , 79,394 , 78,400 , 77,406 , 77,411 , 77,417 , 77,423 , 77,426 , 78,429 , 76,432 , 73,431 , 69,430 , 67,430 , 64,432 , 62,436 , 63,425 , 63,410 , 56,373 , 55,362 , 56,351 , 57,345 , 59,338 , 60,307 , 59,301 , 57,296 , 56,291 , 54,282 , 68,277 , 75,268 , 85,243 , 86,239 ">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^014" title="Right Feet" id="male_right_feet_front" shape="poly" coords="76,432 , 77,435 , 77,438 , 78,440 , 78,445 , 77,447 , 75,448 , 74,448 , 73,450 , 72,452 , 66,458 , 53,458 , 48,456 , 47,451 , 60,439 , 63,435 , 62,436 , 65,432 , 69,430 , 71,430 , 72,431 ">
        
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^012" title="Left Hip" id="male_left_hip_front" shape="poly" coords="135,195 , 136,198 , 137,202 , 138,207 , 140,213 , 142,218 , 144,224 , 145,228 , 146,233 , 146,237 , 147,244 , 147,252 , 147,258 , 147,260 , 147,264 , 146,269 , 145,274 , 145,278 , 143,284 , 143,285 , 137,281 , 132,273 , 130,269 , 127,263 , 123,257 , 121,250 , 119,245 , 119,239 , 117,229 , 116,221 , 116,216 , 121,209 , 126,204 , 129,201">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^013" title="Left Leg" id="male_left_leg_front" shape="poly" coords="116,216 , 117,231 , 119,238 , 119,243 , 120,248 , 122,252 , 123,256 , 126,261 , 128,266 , 131,271 , 134,276 , 137,281 , 143,285 , 143,289 , 142,295 , 141,298 , 140,302 , 139,307 , 139,312 , 138,317 , 138,324 , 138,333 , 139,339 , 141,345 , 143,353 , 143,358 , 142,368 , 141,379 , 140,385 , 138,394 , 137,403 , 136,410 , 136,417 , 136,424 , 136,433 , 134,433 , 130,430 , 127,430 , 126,432 , 124,434 , 123,434 , 122,432 , 121,428 , 121,398 , 116,381 , 112,369 , 114,337 , 108,313 , 107,302 , 107,279 , 101,248 , 106,240 , 107,226 ">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^014" title="Left Feet" id="male_left_feet_front" shape="poly" coords="135,431 , 138,438 , 142,443 , 146,447 , 150,450 , 150,454 , 146,457 , 144,458 , 138,459 , 133,458 , 129,457 , 126,454 , 126,452 , 126,451 , 125,449 , 123,448 , 120,447 , 120,442 , 120,439 , 122,433 , 123,434 , 127,431 , 130,430 , 133,433 , 134,434 , 136,434 ">

        <area class="imgAreaSymptomInfo" GCBodySystem="X135^003" title="Left Shoulder" id="male_left_shoulder_front" shape="poly" coords="147,85 , 151,87 , 153,89 , 155,91 , 156,93 , 158,96 , 159,99 , 161,103 , 161,107 , 162,112 , 162,117 , 162,120 , 161,121 , 161,122 , 158,122 , 156,119 , 155,115 , 153,109 , 150,104 , 147,100 , 146,97 ">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^004" title="Left Arm" id="male_left_arm_front" shape="poly" coords="146,99 , 151,106 , 153,109 , 155,115 , 156,118 , 157,121 , 162,122 , 163,124 , 164,128 , 165,131 , 166,135 , 166,138 , 167,142 , 168,147 , 169,150 , 171,153 , 173,156 , 175,160 , 177,163 , 178,167 , 179,171 , 180,174 , 182,180 , 183,185 , 183,189 , 185,196 , 187,203 , 189,211 , 190,214 , 189,217 , 187,221 , 183,221 , 181,222 , 179,221 , 177,216 , 174,210 , 170,205 , 164,196 , 159,186 , 157,183 , 156,179 , 155,176 , 152,169 , 150,164 , 149,158 , 148,154 , 147,151 , 146,147 , 145,143 , 143,135 ">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^005" title="Left Hand" id="male_left_hand_front" shape="poly" coords="190,214 , 192,242 , 191,246 , 191,251 , 190,253 , 189,259 , 187,264 , 183,264 , 181,262 , 181,256 , 182,243 , 181,240 , 181,237 , 180,236 , 179,235 , 178,236 , 176,238 , 175,239 , 173,241 , 171,241 , 170,241 , 168,239 , 168,236 , 169,235 , 175,226 , 179,222 , 181,222 , 183,221 , 186,221 , 188,220 , 189,219 ">

        <area class="imgAreaSymptomInfo" GCBodySystem="X135^003" title="Right Shoulder" id="male_right_shoulder_front" shape="poly" coords="51,98 , 51,99 , 49,104 , 49,104 , 48,107 , 47,110 , 46,113 , 46,115 , 45,117 , 43,118 , 41,119 , 39,119 , 37,118 , 36,116 , 36,112 , 36,109 , 37,106 , 38,102 , 40,98 , 41,95 , 43,92 , 44,90 , 46,88 , 49,87 ">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^004" title="Right Arm" id="male_right_arm_front" shape="poly" coords="51,100 , 53,104 , 57,136 , 49,159 , 48,166 , 43,173 , 43,179 , 41,183 , 38,189 , 35,195 , 32,200 , 30,203 , 27,207 , 25,211 , 22,216 , 21,219 , 19,221 , 15,222 , 12,222 , 9,222 , 7,220 , 5,217 , 11,205 , 13,195 , 15,188 , 17,181 , 18,172 , 22,161 , 25,156 , 31,147 , 31,140 , 33,135 , 33,130 , 34,126 , 37,122 , 38,119 , 39,119 , 42,119 , 45,117 , 46,113 ">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^005" title="Right Hand" id="male_right_hand_front" shape="poly" coords="20,221 , 20,223 , 24,228 , 26,231 , 31,236 , 31,239 , 30,241 , 28,241 , 26,240 , 21,236 , 19,236 , 17,244 , 17,253 , 17,261 , 17,264 , 17,265 , 14,265 , 12,261 , 10,257 , 7,244 , 5,219 , 6,220 , 12,222 , 17,222 ">
    </map>
    <map name="symptominfobackmale">        
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^001" title="Head" id="male_head_back" shape="poly" coords="89,10, 94,9, 96,9, 99,9, 103,9, 105,9, 106,10, 107,10, 109,12, 110,12, 112,14, 114,16, 115,17, 116,19, 117,21, 118,23, 118,23, 118,26, 118,29, 118,32, 118,34, 118,36, 119,35, 120,36, 120,38, 120,40, 120,41, 119,44, 119,46, 118,48, 118,50, 117,52, 116,52, 115,52, 114,52, 113,51, 113,49, 111,50, 111,51, 109,51, 108,52, 107,53, 105,54, 104,55, 103,56, 101,57, 100,57, 98,57, 96,57, 95,56, 92,54, 90,52, 89,52, 86,50, 86,49, 85,49, 84,47, 84,49, 84,52, 83,53, 81,52, 79,51, 78,50, 77,49, 76,47, 76,45, 76,44, 76,42, 76,41, 76,39, 76,37, 76,37, 77,35, 78,35, 79,36, 79,33, 79,31, 79,30, 79,28, 79,24, 79,22, 79,20, 80,18, 82,16, 83,15, 85,13, 86,11, 87,11, 87,11">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^002" title="Neck" id="male_neck_back" shape="poly" coords="113,49, 113,51, 113,53, 113,55, 113,57, 113,59, 113,59, 113,60, 113,62, 113,64, 113,65, 113,66, 114,68, 115,69, 117,71, 118,73, 119,73, 120,75, 122,76, 124,77, 126,78, 128,79, 130,80, 131,81, 64,80, 66,80, 68,79, 71,78, 71,77, 73,76, 75,75, 77,74, 80,72, 81,71, 82,69, 83,66, 83,65, 83,63, 83,62, 83,60, 83,58, 83,57, 83,54, 83,53, 83,51, 83,49, 83,47, 84,49, 86,51, 88,52, 89,53, 91,54, 93,55, 95,56, 97,57, 98,58, 101,57, 103,56, 105,55, 107,54, 108, 53, 110,51, 113,50, 113,50" href="#">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^008" title="Back" id="male_back" shape="poly" coords="130,81, 133,81, 136,82, 139,83, 142,85, 144,86, 144,87, 145,91, 145,93, 145,95, 145,96, 145,98, 145,100, 145,104, 145,107, 145,110, 146,116, 146,122, 147,120, 147,121, 146,123, 146,126, 145,128, 144,131, 144,133, 142,137, 141,139, 139,142, 138,145, 137,148, 137,150, 136,151, 135,153, 134,155, 133,157, 133,159, 132,161, 132,163, 132,167, 131,167, 133,167, 129,167, 129,167, 129,167, 66,167, 66,172, 65,169, 65,167, 65,165, 64,163, 64,161, 62,159, 62,158, 61,156, 60,154, 59,152, 59,150, 58,148, 58,147, 56,145, 55,141, 54,140, 54,139, 53,136, 53,134, 52,130, 51,129, 51,127, 50,125, 50,122, 50,120, 50,117, 50,116, 50,114, 50,111, 50,108, 50,105, 50,102, 50,99, 50,94, 50,91, 50,88, 51,86, 53,85, 58,83, 66,81, 68,81">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^009" title="Lower Back" id="male_lower_back" shape="poly" coords="131,167, 131,167, 131,172, 131,174, 132,177, 135,189, 135,198, 61,199, 60,189, 66,176, 65,167">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^010" title="Buttock" id="male_buttock_back" shape="poly" coords="135,198, 141,223, 144,240, 141,243, 122,249, 110,249, 98,244, 98,241, 94,245, 83,249, 74,249, 67,248, 59,247, 57,244, 54,241, 53,240, 53,228, 61,207, 60,198">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^013" title="Right Leg (Back)" id="male_right_leg_back" shape="poly" coords=" 145,276, 143,288, 140,311, 139,324, 139,337, 139,343, 142,355, 144,362, 145,376, 143,385, 141,398, 139,411, 136,422, 136,429, 136,437, 136,441, 130,447, 127,448, 123,446, 122,445, 122,443, 120,436, 121,425, 112,378, 111,368, 113,353, 113,339, 110,329, 108,313, 105,285, 100,261, 98,245, 105,248, 112,250, 119,250, 128,248, 135,246, 141,244, 144,240">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^014" title="Right Feet (Back)" id="male_right_feet_back" shape="poly" coords="136,442, 136,444, 138,446, 142,446, 145,447, 147,450, 146,452, 144,455, 140,456, 138,457, 130,460, 125,460, 121,458, 121,450, 121,448, 121,446, 125,447, 129,448, 136,442">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^013" title="Left Leg (Back)" id="male_left_leg_back" shape="poly" coords="97,243, 97,250, 96,259, 96,259, 92,287, 90,302, 90,317, 85,337, 83,354, 85,362, 87,370, 86,374, 85,382, 83,386, 78,397, 75,431, 77,435, 77,439, 75,443, 74,446, 71,448, 69,449, 66,449, 64,448, 62,447, 61,443, 60,442, 61,434, 60,414, 58,409, 56,403, 55,393, 54,387, 53,357, 58,348, 58,307, 52,259, 52,259, 53,240, 59,247, 89,248, 93,246, 97,243">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^014" title="Left Feet (Back)" id="male_left_feet_back" shape="poly" coords="75,446, 76,454, 76,459, 72,460, 69,461, 64,460, 61,460, 57,457, 54,455, 51,454, 49,453, 48,450, 51,447, 53,447, 59,442, 60,443, 61,446, 64,449, 70,449, 73,448">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^005" title="Right Hand (Back)" id="male_right_hand_back" shape="poly" coords="144,85, 152,90, 160,100, 164,122, 166,130, 169,150, 172,158, 178,169, 181,174, 185,197, 192,224, 192,228, 187,230, 183,231, 178,230, 178,225, 169,208, 163,196, 157,179, 150,162, 143,137, 145,131, 146,127, 147,123, 147,119, 146,112, 145,92, 145,90">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^004" title="Right Arm (Back)" id="male_right_arm_back" shape="poly" coords="192,257, 191,262, 190,265, 190,267, 185,271, 182,271, 181,270, 180,267, 179,266, 180,260, 180,257, 180,254, 180,252, 179,248, 179,243, 176,243, 169,247, 168,247, 165,245, 164,242, 172,237, 172,233, 177,230, 180,231, 184,231, 187,231, 192,229">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^005" title="Left Hand (Back)" id="male_left_hand_back" shape="poly" coords="50,87, 50,120, 52,133, 53,140, 45,172, 42,176, 41,181, 38,188, 35,195, 32,201, 22,214, 18,230, 14,231, 10,230, 7,229, 4,226, 6,216, 9,207, 11,197, 13,188, 15,179, 20,168, 24,158, 29,140, 30,127, 33,121, 35,103, 40,92">
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^004" title="Left Arm (Back)" id="male_left_arm_back" shape="poly" coords="19,231, 25,237, 27,240, 31,243, 31,245, 30,246, 28,246, 29,247, 27,247, 24,245, 21,242, 19,242, 18,244, 17,252, 18,270, 15,270, 15,272, 12,272, 11,271, 10,269, 7,265, 6,258, 6,252, 5,228, 6,229, 13,231">
    </map>
    <map name="symptominfofrontfemale">
	    <area class="imgAreaSymptomInfo" GCBodySystem="X135^001" title="Head" id="female_head_front" shape="poly" coords="53,34, 53,29, 55,23, 58,18, 63,15, 69,13, 75,14, 79,15, 83,17, 87,21, 89,27, 89,29, 90,33, 90,37, 90,41, 92,40, 92,48, 89,54, 88,53, 86,57, 85,60, 83,64, 80,66, 74,68, 67,68, 61,65, 59,63, 57,59, 55,53, 53,53, 52,49, 50,42, 52,39, 53,33" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^002" title="Neck" id="female_neck_front" shape="poly" coords=" 60,63, 61,65, 67,68, 74,68, 74,68, 80,66, 84,63, 83,66, 83,71, 84,78, 86,81, 91,85, 100,89, 90,89, 81,89, 78,89, 75,90, 73,91, 71,92, 69,91, 67,90, 65,89, 63,89, 54,89, 45,89, 42,89, 45,88, 48,87, 52,85, 56,82, 58,80, 60,71, 59,65, 59,62" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^004" title="Right Arm" id="female_right_arm_front" shape="poly" style="display:none;" coords="22,116, 20,156, 16,168, 14,174, 14,197, 12,226, 12,231, 20,229, 20,225, 30,196, 36,171, 37,165, 36,148, 36,135, 36,131, 36,125, 35,118, 35,115, 35,112, 35,107" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^004" title="Left Arm" id="female_left_arm_front" shape="poly" coords="121,116, 123,157, 127,170, 132,227, 123,227, 122,220, 115,199, 111,186, 109,178, 107,170, 107,157, 107,150, 106,141, 107,130, 107,123, 107,121, 108,117, 109,113, 109,110" href="#" >
		<area class="imgAreaSymptomInfo" GCBodySystem="X135^003" title="Right Shoulder" id="female_right_shoulder_front" shape="poly" coords="42,89, 40,90, 29,92, 27,94, 25,96, 23,102, 22,113, 22,116, 33,109, 35,107, 37,105, 40,100, 41,98, 42,92" href="#" >
		<area class="imgAreaSymptomInfo" GCBodySystem="X135^003" title="Left Shoulder" id="female_left_shoulder_front" shape="poly" coords="100,89, 102,90, 113,92, 115,94, 118,97, 119,102, 121,113, 121, 116, 110,109, 107,106, 104,103, 102,100, 101,98, 100,94, 100,92" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^005" title="Right Hand" id="female_right_hand_front" shape="poly" coords="20,228, 12,232, 11,242, 10,248, 11,251, 11,261, 15,271, 17,272, 17,271, 19,271, 20,272, 21,270, 21,267, 20,265, 20,264, 19,260, 18,258, 18,253, 18,250, 18,247, 18,247, 20,246, 20,250, 22,253, 24,254, 25,251, 25,249, 25,246, 24,241, 24,237, 21,232, 20,228, 20,226" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^005" title="Left Hand" id="female_left_hand_front" shape="poly" coords="123,227, 132,227, 134,247, 133,262, 128,272, 126,270, 125,271, 123,270, 122,270, 121,268, 124,265, 122,263, 124,261, 126,257, 126,253, 126,252, 125,242, 123,250, 120,254, 118,252, 119,242, 120,237, 123,231" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^006" title="Chest" id="female_chest_front" shape="poly" coords="100,89, 78,89, 75,90, 73,91, 71,92, 69,91, 67,90, 65,89, 42,89, 41,98, 40,100, 37,105, 35,107, 34,109, 36,125, 36,131, 37,135, 38,139, 38,145, 40,163, 48,166, 94,166, 102,163, 106,134, 107,131, 107,122, 108,117, 109,113, 109,110, 108,107, 107,106, 104,103, 102,100, 101,98, 101,98, 100,94, 100,92" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^007" title="Abdomen" id="female_abdomen_front" shape="poly" coords="40,163, 48,166, 94,166, 102,163, 107,176, 111,194, 101,204, 92,215, 87,221, 76,210, 71,209, 66,210,  55,221, 51,216, 41,204, 32,194" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^011" title="Pelvis" id="female_pelvis_front" shape="poly" coords="87,221, 76,210, 71,209, 66,210, 55,221, 62,230, 68,238, 70,239, 72,239, 74,238, 80,230, 87,221" href="#" >          
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^012" title="Right Hip" id="female_right_hip_front" shape="poly" coords="32,194, 41,204, 51,216, 43,240, 35,282, 31,262, 28,235, 30,210, 31,197" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^012" title="Left Hip" id="female_left_hip_front" shape="poly" coords="111,194, 101,204, 92,216, 98,240, 108,282, 112,262, 115,235, 113,200, 111,194" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^013" title="Right Leg" id="female_right_leg_front" shape="poly" coords="51,216, 43,240, 35,282, 42,306, 44,320, 44,333, 41,346, 42,355, 49,384, 54,407, 66,407, 67,373, 69,356, 67,334, 70,319, 69,312, 68,286, 70,265, 70,245, 68,238, 62,230, 51,216" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^013" title="Left Leg" id="female_left_leg_front" shape="poly" coords="92,216, 98,240, 108,282, 105,294, 101,304, 101,326, 101,339, 102,353, 96,379, 89,407, 76,407, 76,384, 74,357, 77,335, 74,306, 75,282, 73,263, 74,238, 80,230, 87,221, 92,216" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^014" title="Right Feet" id="female_right_feet_front" shape="poly" coords="54,407, 55,415, 55,421, 55,422, 55,425, 53,428, 53,430, 52,431, 45,440, 45,442, 46,444, 49,445, 55,446, 57,446, 60,446, 62,446, 65,445, 66,443, 67,440, 68,437, 70,434, 69,425, 69,422, 67,414, 66,407" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^014" title="Left Feet" id="female_left_feet_front" shape="poly" coords="76,407, 88,407, 88,414, 89,418, 90,425, 90,429, 91,433, 95,437, 99,441, 99,443, 97,445, 92,447, 82,447, 78,446, 77,445, 75,441, 75,425, 74,422, 74,419, 75,414, 76,407" href="#" >
    </map>
    <map name="symptominfobackfemale">
	    <area class="imgAreaSymptomInfo" GCBodySystem="X135^001" title="Head (Back)" id="female_head_back" shape="poly" coords="53,59, 52,58, 51,55, 51,52, 50,53, 48,52, 47,50, 46,47, 45,45, 45,42, 44,39, 45,38, 46,37, 48,37, 47,34, 48,26, 49,23, 50,20, 53,16, 56,14, 57,13, 63,11, 69,11, 73,12, 77,14, 80,16, 83,20, 84,24, 85,29, 85,32, 85,35, 85,38, 87,38, 88,40, 88,41, 87,44, 86,46, 86,49, 85,50, 85,52, 83,52, 82,54, 81,56, 80,58, 79,59, 78,55, 76,53, 56,53, 54,55, 53,59" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^002" title="Neck (Back)" id="female_neck_back" shape="poly" coords="53,58, 54,55, 56,53, 76,53, 78,55, 79,59, 79,73, 80,75, 81,77,  82,78, 86,82, 74,84, 54,84, 46,82, 48,80, 50,78, 53,75, 54,73, 54,62, 53,59" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^008" title="Back" id="female_back" shape="poly" coords="46,82, 54,84, 74,84, 86,82, 96,87, 106,116, 101,130 , 99,138, 97,166, 92,169, 80,170, 50,170, 40,169, 36,166, 36,158, 34,138, 32,125, 31,123, 28,117, 35,87, 40,85, 45,82" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^009" title="Lower Back" id="female_lower_back" shape="poly" coords="36,167, 40,169, 50,170, 80,170, 92,169, 97,166, 101,177, 102,182, 103,189, 105,195, 107,203, 102,201, 32,201, 27,203, 28,193, 32,180, 34,172" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^004" title="Left arm (Back)" id="female_left_arm_back" shape="poly" coords="35,87, 21,91, 19,96, 16,106, 16,135, 16,154, 11,166, 9,186, 9,206,  6,230, 15,230, 16,226, 18,219, 27,188, 27,183, 29,178, 31,173, 31,167, 31,161, 31,152, 31,137, 32,130, 28,117, 35,87" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^004" title="Right arm (Back)" id="female_right_arm_back" shape="poly" coords="96,87, 112,91, 117,104, 118,125, 119,158, 121,163, 123,172, 126,229, 118,229, 115,214, 108,194, 102,173, 103,159, 101,130, 106,116, 96,87" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^005" title="Left Hand (Back)" id="female_left_hand_back" shape="poly" coords="6,231, 5,245, 5,250, 6,252, 6,258, 7,261, 9,267, 10,271, 11,272, 13,271, 13,270, 13,270, 15,269, 16,268, 14,265, 15,262, 14,259, 13,256, 12,252, 13,246, 15,244, 16,244, 17,247, 18,249, 18,251, 21,251, 21,249, 20,246, 19,242, 19,238, 17,234, 15,231" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^005" title="Right Hand (Back)" id="female_right_hand_back" shape="poly" coords="118,229, 126,230, 127,249, 127,258, 126,261, 122,271, 120,271, 117,270, 117,267, 117,266, 115,265, 117,262, 119,257, 121,251, 119,245, 118,245, 117,249, 115,253, 112,253, 112,251, 113,242, 113,237, 114,233, 117,230" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^010" title="Buttock" id="female_buttock" shape="poly" coords="27,203, 32,201, 102,201, 107,203, 109,242, 105,245, 100,247, 95,249, 90,250, 85,250, 80,249, 75,247, 68,244, 66,235, 65,243, 62,247, 58,249, 48,250, 40,250, 35,249, 28,247, 24,245" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^013" title="Left Leg (Back)" id="female_left_leg_back" shape="poly" coords= "65,243, 62,247, 58,249, 48,250, 40,250, 35,249, 28,247, 24,245, 25,255, 29,273, 32,288, 35,301, 36,305, 38,315, 39,325, 36,347, 37,359, 41,374, 44,387, 47,402, 50,412, 50,418, 62,417, 61,411, 62,404, 61,392, 61,383, 61,379, 64,362, 63,353, 61,341, 61,335, 62,333, 65,319, 65,311, 64,299, 64,284, 65,266, 64,249" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^013" title="Right Leg (Back)" id="female_right_leg_back" shape="poly" coords="68,245, 75,247, 80,249, 85,250, 90,250, 95,249, 100,248, 105,245, 109,242, 105,268, 100,290, 96,303, 96,308, 95,329, 96,345, 97,352, 96,363, 93,373, 90,388, 85,405, 84,416, 72,416, 71,381, 70,366, 70,349, 73,334, 70,322, 70,298, 70,275, 68,255, 67,245" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^014" title="Left Feet (Back)" id="female_left_feet_back" shape="poly" coords="62,418, 50,418, 49,426, 48,431, 46,432, 44,433, 42,434, 40,434, 40,438, 44,440, 49,444, 52,445, 56,446, 59,446, 63,444, 63,430, 64,425, 63,421, 62,418" href="#" >
        <area class="imgAreaSymptomInfo" GCBodySystem="X135^014" title="Right Feet (Back)" id="female_right_feet_back" shape="poly" coords="71,416, 84,416, 85,429, 86,433, 93,435, 93,436, 91,439, 89,441, 86,441, 84,443, 79,444, 71,444, 70,444, 69,442, 70,438, 70,435, 70,432, 71,429, 69,423, 71,418" href="#" >
    </map>
</div>
