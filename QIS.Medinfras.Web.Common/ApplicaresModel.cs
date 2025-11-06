using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    public class AplicaresMedinfrasRequest
    {
        public string Type { get; set; }
        public string Parameter { get; set; }
    }

    public class RequestCreateRoom
    {
        public string kodekelas { get; set; }
        public string koderuang { get; set; }
        public string namaruang { get; set; }
        public string kapasitas { get; set; }
        public string tersedia { get; set; }
        public string tersediapria { get; set; }
        public string tersediawanita { get; set; }
        public string tersediapriawanita { get; set; }
    }

    public class CreateRoomResponse
    {
        public MetaData metadata { get; set; }
        public RequestCreateRoom response { get; set; }
    }

    public class RequestUpdateRoom
    {
        public string kodekelas { get; set; }
        public string koderuang { get; set; }
        public string namaruang { get; set; }
        public string kapasitas { get; set; }
        public string tersedia { get; set; }
        public string tersediapria { get; set; }
        public string tersediawanita { get; set; }
        public string tersediapriawanita { get; set; }
    }

    public class UpdateRoomResponse
    {
        public MetaData metadata { get; set; }
        public RequestUpdateRoom response { get; set; }
    }

    public class RequestDeleteRoom
    {
        public string kodekelas { get; set; }
        public string koderuang { get; set; }
    }

    public class DeleteRoomResponse
    {
        public MetaData metadata { get; set; }
        public RequestDeleteRoom response { get; set; }
    }
}