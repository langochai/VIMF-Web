﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BMS.Models;

public partial class Warehouse
{
    public int Id { get; set; }

    public int? FactoryId { get; set; }

    public string WarehouseCode { get; set; }

    public string WarehouseName { get; set; }
}