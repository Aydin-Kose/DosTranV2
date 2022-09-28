using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DosTranV2.Data.Model
{
    public class DOSTRAN_VERSION
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(10)]
        public string Version { get; set; }
        public string Link { get; set; }
    }
}
