﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Back_End_TPI_PSS.Services.Interfaces;

namespace Back_End_TPI_PSS.Data.Entities
{
    public class Size : IStatusEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SizeName { get; set; }
        public bool Status { get; set; }

    }
}
