using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace VoipServer.Data.Entities
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Table("cdr", Schema = "asterisk")]
    public class cdr
    {
        public DateTime calldate {get;set;}
        public string clid {get;set;}
        public string src {get;set;}
        public string dst {get;set;}
        public string dcontext {get;set;}
        public string channel {get;set;}
        public string dstchannel {get;set;}
        public string lastapp {get;set;}
        public string lastdata {get;set;}
        public int duration {get;set;}
        public int billsec {get;set;}
        public string disposition {get;set;}
        public int amaflags {get;set;}
        public string accountcode {get;set;}
        public string uniqueid {get;set;}
        public string userfield {get;set;}
        public string nazar {get;set;}
        public string nazarenteghad {get;set;}
        public string enteghad {get;set;}
        public string record {get;set;} 
        public string voicemail {get;set;}
        public string peygiri {get;set;}
        public string mobile {get;set;}
        public string ticket {get;set;}
        public string answer {get;set;}
        public string person {get;set;}
        public int id {get;set;}
        public int price {get;set;}
        public int jalali_year {get;set;}
        public int jalali_month {get;set;}
        public int jalali_day {get;set;}
        public int jalali_hour {get;set;}
        public int? fax {get;set;}
        public int seen { get; set; }
    }
}
