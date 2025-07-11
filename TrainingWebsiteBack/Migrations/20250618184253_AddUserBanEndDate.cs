﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingWebsiteBack.Migrations
{
    /// <inheritdoc />
    public partial class AddUserBanEndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BanEndDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BanEndDate",
                table: "Users");
        }
    }
}
