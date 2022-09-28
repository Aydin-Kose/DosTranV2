using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace DosTranV2.DBModel.Model
{
    public class DOSTRAN_LOG
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(10)]
        public string OpID { get; set; }
        [Column(TypeName = "NVARCHAR")]
        public string UserName { get; internal set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(15)]
        public string IP { get; internal set; }
        public string DataSet { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(10)]
        public string IslemTipi { get; set; }
        public DateTime TarihSaat { get; set; }
    }
}
