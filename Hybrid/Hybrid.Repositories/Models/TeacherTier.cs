﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Hybrid.Repositories.Models;

public partial class TeacherTier
{
    public string TierId { get; set; }

    public string TierName { get; set; }

    public string Description { get; set; }

    public virtual ICollection<TeacherSupscription> TeacherSupscriptions { get; set; } = new List<TeacherSupscription>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}