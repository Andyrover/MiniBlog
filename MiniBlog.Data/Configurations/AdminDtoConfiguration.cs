﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniBlog.Data.Dto;

namespace MiniBlog.Data.Configurations
{
    public class AdminDtoConfiguration : IEntityTypeConfiguration<AdminDto>
    {
        public void Configure(EntityTypeBuilder<AdminDto> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.User)
                .IsRequired()
                .HasMaxLength(32);
            builder.Property(m => m.Password)
                .IsRequired()
                .HasMaxLength(32);
            builder.HasData(new AdminDto
            {
                Id = 1,
                User = "admin",
                Password = "123456",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            });
        }
    }
}
