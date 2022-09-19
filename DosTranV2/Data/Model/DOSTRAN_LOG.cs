using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DosTranV2.Data.Model
{
    public class DOSTRAN_LOG
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(10)]
        public string OpID { get; set; }
        public string DataSet { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(10)]
        public string IslemTipi { get; set; }
        public DateTime TarihSaat { get; set; }
    }
}
