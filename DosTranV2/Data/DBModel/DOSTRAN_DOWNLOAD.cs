using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DosTranV2.DBModel.Model
{
    public class DOSTRAN_DOWNLOAD
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(10)]
        public string OpID { get; set; }
        public string DataSet { get; set; }
    }
}
