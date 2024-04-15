﻿namespace CleanArchitecture.Domain.Common;

public abstract class BaseDomainModel
{
    public int Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}