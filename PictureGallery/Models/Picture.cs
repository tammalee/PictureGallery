using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PictureGallery.Models

{
    [Table("gallery")]
    public partial class Picture
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "varchar(75)")]
        public string FileName { get; set; }

    }
}
