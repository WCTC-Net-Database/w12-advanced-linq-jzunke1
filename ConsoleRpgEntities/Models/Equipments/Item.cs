﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleRpgEntities.Models.Equipments;


public class Item
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Attack { get; set; }
    public int Defense { get; set; }

    [Column(TypeName = "decimal(3, 2)")]
    public decimal Weight { get; set; }

    public int Value { get; set; }
}
