﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ClientInformationEntity
{
    [Key, ForeignKey(nameof(Client))]
    public string ClientId { get; set; } = null!;
    public virtual ClientEntity Client { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Reference { get; set; }

}