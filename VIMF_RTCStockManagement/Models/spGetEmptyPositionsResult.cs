﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMS.Models
{
    public partial class spGetEmptyPositionsResult
    {
        public int ID { get; set; }
        [StringLength(50)]
        public string PositionCode { get; set; }
        [StringLength(50)]
        public string PositionName { get; set; }
        public int? AreaID { get; set; }
    }
}
