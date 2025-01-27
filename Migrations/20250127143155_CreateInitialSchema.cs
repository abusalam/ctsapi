using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CTS_BE.Migrations
{
    /// <inheritdoc />
    public partial class CreateInitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "cts_pension");

            migrationBuilder.CreateTable(
                name: "account_heads",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: true),
                    demand_no = table.Column<string>(
                        type: "character varying(2)",
                        maxLength: 2,
                        nullable: true
                    ),
                    major_head = table.Column<string>(
                        type: "character varying(4)",
                        maxLength: 4,
                        nullable: true
                    ),
                    submajor_head = table.Column<string>(
                        type: "character varying(2)",
                        maxLength: 2,
                        nullable: true
                    ),
                    minor_head = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: true
                    ),
                    plan_status = table.Column<string>(
                        type: "character varying(2)",
                        maxLength: 2,
                        nullable: true
                    ),
                    scheme_head = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: true
                    ),
                    detail_head = table.Column<string>(
                        type: "character varying(2)",
                        maxLength: 2,
                        nullable: true
                    ),
                    subdetail_head = table.Column<string>(
                        type: "character varying(2)",
                        maxLength: 2,
                        nullable: true
                    ),
                    voted_charged = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: true
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("account_heads_pkey", x => x.id);
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "banks",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    bank_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("banks_pkey", x => x.id);
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "breakups",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false, comment: "BreakupId")
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    component_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    component_type = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false,
                        comment: "P - Payment; D - Deduction;"
                    ),
                    relief_flag = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        comment: "Relief Allowed (Yes/No)"
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("breakups_pkey", x => x.id);
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "dml_history",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: false),
                    treasury_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    ppo_id = table.Column<int>(type: "integer", nullable: false),
                    updated_table_field = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    from_record_id = table.Column<long>(type: "bigint", nullable: false),
                    to_record_id = table.Column<long>(type: "bigint", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("dml_history_pkey", x => x.id);
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "financial_years",
                schema: "cts_pension",
                columns: table => new
                {
                    current_year = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("financial_years_pkey", x => x.current_year);
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "ppo_id_sequences",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: false),
                    treasury_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    next_sequence_value = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("ppo_id_sequences_pkey", x => x.id);
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "ppo_receipt_sequences",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: false),
                    treasury_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    next_sequence_value = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("ppo_receipt_sequences_pkey", x => x.id);
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "sub_categories",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    sub_category_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("sub_categories_pkey", x => x.id);
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "treasuries",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    treasury_code = table.Column<string>(
                        type: "character varying(4)",
                        maxLength: 4,
                        nullable: false
                    ),
                    treasury_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    district_code = table.Column<string>(
                        type: "character varying(2)",
                        maxLength: 2,
                        nullable: true
                    ),
                    treasury_address = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: true
                    ),
                    address1 = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: true
                    ),
                    address2 = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: true
                    ),
                    phone_no1 = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: true
                    ),
                    phone_no2 = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: true
                    ),
                    fax = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: true
                    ),
                    e_mail = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: true
                    ),
                    pincode = table.Column<string>(
                        type: "character varying(6)",
                        maxLength: 6,
                        nullable: true
                    ),
                    int_treasury_code = table.Column<string>(
                        type: "character varying(5)",
                        maxLength: 5,
                        nullable: true
                    ),
                    pension_flag = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: true
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("treasuries_pkey", x => x.id);
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "uploaded_files",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    file_path = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false
                    ),
                    file_name = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false
                    ),
                    file_mime_type = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    contents = table.Column<byte[]>(type: "bytea", nullable: false),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("uploaded_files_pkey", x => x.id);
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "classifications",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    classification_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    account_head_id = table.Column<long>(type: "bigint", nullable: false),
                    due_draw_flag = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false,
                        comment: "[PD] P - Payment; D - Deduction;"
                    ),
                    classification_flag = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false,
                        comment: "[PD] P - Paid; D - Deducted;"
                    ),
                    commuted_value_pension = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        comment: "[Y/N]"
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("classifications_pkey", x => x.id);
                    table.ForeignKey(
                        name: "classifications_account_head_id_fkey",
                        column: x => x.account_head_id,
                        principalSchema: "cts_pension",
                        principalTable: "account_heads",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "primary_categories",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    account_head_id = table.Column<long>(
                        type: "bigint",
                        nullable: false,
                        comment: "Head of Account: 2071 - 01 - 109 - 00 - 001 - V - 04 - 00"
                    ),
                    primary_category_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("primary_categories_pkey", x => x.id);
                    table.ForeignKey(
                        name: "primary_categories_account_head_id_fkey",
                        column: x => x.account_head_id,
                        principalSchema: "cts_pension",
                        principalTable: "account_heads",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "branches",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    bank_id = table.Column<long>(type: "bigint", nullable: false),
                    branch_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    branch_address = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false
                    ),
                    ifsc_code = table.Column<string>(
                        type: "character varying(11)",
                        maxLength: 11,
                        nullable: false
                    ),
                    district_name = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false
                    ),
                    city_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    state_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    phone_no = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("branches_pkey", x => x.id);
                    table.ForeignKey(
                        name: "branches_bank_id_fkey",
                        column: x => x.bank_id,
                        principalSchema: "cts_pension",
                        principalTable: "banks",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "eppo_receipts",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: false),
                    treasury_code = table.Column<string>(
                        type: "character varying(5)",
                        maxLength: 5,
                        nullable: false
                    ),
                    ppo_id = table.Column<int>(type: "integer", nullable: true),
                    pension_appln_no = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    fresh_revision_flag = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false
                    ),
                    ppo_type_code = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false
                    ),
                    ppo_no = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    issuing_letter_no = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    issuing_letter_date = table.Column<DateOnly>(type: "date", nullable: true),
                    pen_cat_id = table.Column<int>(type: "integer", nullable: false),
                    sanction_authority = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false
                    ),
                    sanction_no = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false
                    ),
                    sanction_date = table.Column<DateOnly>(type: "date", nullable: false),
                    provisional_pension_status = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false
                    ),
                    pensioner_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    religion = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false
                    ),
                    pensioner_address = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    mobile_number = table.Column<string>(
                        type: "character varying(10)",
                        maxLength: 10,
                        nullable: true
                    ),
                    aadhaar_no = table.Column<string>(
                        type: "character varying(12)",
                        maxLength: 12,
                        nullable: true
                    ),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    date_of_retirement = table.Column<DateOnly>(type: "date", nullable: false),
                    date_of_death = table.Column<DateOnly>(type: "date", nullable: true),
                    qualifying_service_gross_years = table.Column<int>(
                        type: "integer",
                        nullable: true
                    ),
                    qualifying_service_gross_months = table.Column<int>(
                        type: "integer",
                        nullable: true
                    ),
                    qualifying_service_gross_days = table.Column<int>(
                        type: "integer",
                        nullable: true
                    ),
                    employee_last_pay = table.Column<int>(type: "integer", nullable: true),
                    employee_last_pay_notional = table.Column<int>(type: "integer", nullable: true),
                    commuted_pension_amount = table.Column<int>(type: "integer", nullable: false),
                    withdrawn = table.Column<bool>(type: "boolean", nullable: true),
                    withdraw_date = table.Column<DateOnly>(type: "date", nullable: true),
                    withdraw_reason = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    photo_file_id = table.Column<long>(type: "bigint", nullable: true),
                    signature_file_id = table.Column<long>(type: "bigint", nullable: true),
                    eppo_file_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("eppo_receipts_pkey", x => x.id);
                    table.ForeignKey(
                        name: "eppo_receipts_eppo_file_id_fkey",
                        column: x => x.eppo_file_id,
                        principalSchema: "cts_pension",
                        principalTable: "uploaded_files",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "eppo_receipts_photo_file_id_fkey",
                        column: x => x.photo_file_id,
                        principalSchema: "cts_pension",
                        principalTable: "uploaded_files",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "eppo_receipts_signature_file_id_fkey",
                        column: x => x.signature_file_id,
                        principalSchema: "cts_pension",
                        principalTable: "uploaded_files",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "eppo_revisions",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: false),
                    treasury_code = table.Column<string>(
                        type: "character varying(5)",
                        maxLength: 5,
                        nullable: false
                    ),
                    ppo_id = table.Column<int>(type: "integer", nullable: true),
                    pension_appln_no = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    ppo_no = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    issuing_letter_no = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    issuing_letter_date = table.Column<DateOnly>(type: "date", nullable: true),
                    fresh_revision_flag = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false
                    ),
                    ppo_type_code = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false
                    ),
                    ppo_sub_type = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false
                    ),
                    pen_cat_id = table.Column<int>(type: "integer", nullable: false),
                    employee_last_pay = table.Column<int>(type: "integer", nullable: true),
                    employee_last_pay_notional = table.Column<int>(type: "integer", nullable: true),
                    commuted_pension_amount = table.Column<int>(type: "integer", nullable: false),
                    eppo_file_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("eppo_revisions_pkey", x => x.id);
                    table.ForeignKey(
                        name: "eppo_revisions_eppo_file_id_fkey",
                        column: x => x.eppo_file_id,
                        principalSchema: "cts_pension",
                        principalTable: "uploaded_files",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    primary_category_id = table.Column<long>(type: "bigint", nullable: false),
                    sub_category_id = table.Column<long>(type: "bigint", nullable: false),
                    category_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false,
                        comment: "primary_category_name - sub_category_name"
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("categories_pkey", x => x.id);
                    table.ForeignKey(
                        name: "categories_primary_category_id_fkey",
                        column: x => x.primary_category_id,
                        principalSchema: "cts_pension",
                        principalTable: "primary_categories",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "categories_sub_category_id_fkey",
                        column: x => x.sub_category_id,
                        principalSchema: "cts_pension",
                        principalTable: "sub_categories",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "bills",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: false),
                    treasury_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    account_head_id = table.Column<long>(type: "bigint", nullable: false),
                    branch_id = table.Column<long>(type: "bigint", nullable: false),
                    bill_no = table.Column<int>(type: "integer", nullable: false),
                    bill_date = table.Column<DateOnly>(type: "date", nullable: false),
                    treasury_voucher_no = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    treasury_voucher_date = table.Column<DateOnly>(type: "date", nullable: true),
                    from_date = table.Column<DateOnly>(type: "date", nullable: false),
                    to_date = table.Column<DateOnly>(type: "date", nullable: false),
                    gross_amount = table.Column<int>(type: "integer", nullable: false),
                    bytransfer_amount = table.Column<int>(type: "integer", nullable: false),
                    net_amount = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("bills_pkey", x => x.id);
                    table.ForeignKey(
                        name: "bills_account_head_id_fkey",
                        column: x => x.account_head_id,
                        principalSchema: "cts_pension",
                        principalTable: "account_heads",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "bills_branch_id_fkey",
                        column: x => x.branch_id,
                        principalSchema: "cts_pension",
                        principalTable: "branches",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "eppo_nominees",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    nominee_type = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false,
                        comment: "P - Pensioner; F - Family; D - Dependent;"
                    ),
                    serial_no = table.Column<int>(type: "integer", nullable: false),
                    nominee_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    relation = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false,
                        comment: "[WEHSDOMRNAFKYCUITJBPVL] E - Employed; L - Widow Daughter; U - Unmarried Daughter; V - Divorced Daughter; N - Minor Son; R - Minor Daughter; P - Handicapped Son; G - Handicapped Daughter; J - Dependent Father; K - Dependent Mother; H - Husband; W - Wife;"
                    ),
                    nominee_share = table.Column<int>(type: "integer", nullable: true),
                    nominee_adult_minor = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: true,
                        comment: "A - Adult; M - Minor;"
                    ),
                    eppo_receipt_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("eppo_nominees_pkey", x => x.id);
                    table.ForeignKey(
                        name: "eppo_nominees_eppo_receipt_id_fkey",
                        column: x => x.eppo_receipt_id,
                        principalSchema: "cts_pension",
                        principalTable: "eppo_receipts",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "ppo_receipts",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: false),
                    treasury_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    treasury_receipt_no = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    ppo_no = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    pensioner_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    date_of_commencement = table.Column<DateOnly>(type: "date", nullable: false),
                    mobile_number = table.Column<string>(
                        type: "character varying(10)",
                        maxLength: 10,
                        nullable: true
                    ),
                    receipt_date = table.Column<DateOnly>(type: "date", nullable: false),
                    psa_code = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false
                    ),
                    ppo_type = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false
                    ),
                    ppo_status = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    receipt_type = table.Column<string>(
                        type: "character varying(30)",
                        maxLength: 30,
                        nullable: false,
                        defaultValueSql: "'PPO'::character varying",
                        comment: "PPO - PpoReceipt; EPPO - EppoReceipt;"
                    ),
                    eppo_receipt_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("ppo_receipts_pkey", x => x.id);
                    table.ForeignKey(
                        name: "ppo_receipts_eppo_receipt_id_fkey",
                        column: x => x.eppo_receipt_id,
                        principalSchema: "cts_pension",
                        principalTable: "eppo_receipts",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "component_rates",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(
                            type: "bigint",
                            nullable: false,
                            comment: "RateId will identify the component rate revised or introduced"
                        )
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    breakup_id = table.Column<long>(type: "bigint", nullable: false),
                    effective_from_date = table.Column<DateOnly>(
                        type: "date",
                        nullable: false,
                        comment: "Effective from date the component rate is revised or introduced"
                    ),
                    rate_amount = table.Column<int>(type: "integer", nullable: false),
                    rate_type = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false,
                        comment: "P - Percentage; A - Amount;"
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("component_rates_pkey", x => x.id);
                    table.ForeignKey(
                        name: "component_rates_breakup_id_fkey",
                        column: x => x.breakup_id,
                        principalSchema: "cts_pension",
                        principalTable: "breakups",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "component_rates_category_id_fkey",
                        column: x => x.category_id,
                        principalSchema: "cts_pension",
                        principalTable: "categories",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "eppo_amounts",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    amount_type = table.Column<string>(
                        type: "character(3)",
                        fixedLength: true,
                        maxLength: 3,
                        nullable: false,
                        comment: "CLS - Classification; EFP - Enhanced Family Pension; BSC - Basic Pension; NFP - Normal Family Pension; BYT - By Transfer;"
                    ),
                    classification_id = table.Column<long>(type: "bigint", nullable: true),
                    from_date = table.Column<DateOnly>(type: "date", nullable: true),
                    to_date = table.Column<DateOnly>(type: "date", nullable: true),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    consolidated = table.Column<bool>(type: "boolean", nullable: false),
                    category_id = table.Column<long>(type: "bigint", nullable: true),
                    eppo_receipt_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("eppo_amounts_pkey", x => x.id);
                    table.ForeignKey(
                        name: "eppo_amounts_category_id_fkey",
                        column: x => x.category_id,
                        principalSchema: "cts_pension",
                        principalTable: "categories",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "eppo_amounts_classification_id_fkey",
                        column: x => x.classification_id,
                        principalSchema: "cts_pension",
                        principalTable: "classifications",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "eppo_amounts_eppo_receipt_id_fkey",
                        column: x => x.eppo_receipt_id,
                        principalSchema: "cts_pension",
                        principalTable: "eppo_receipts",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "pensioners",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: false),
                    treasury_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    receipt_id = table.Column<long>(type: "bigint", nullable: false),
                    ppo_id = table.Column<int>(type: "integer", nullable: false),
                    ppo_no = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    ppo_type = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false,
                        comment: "P - Pension; F - Family Pension; C - CPF;"
                    ),
                    ppo_sub_type = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false,
                        comment: "E - Employed; L - Widow Daughter; U - Unmarried Daughter; V - Divorced Daughter; N - Minor Son; R - Minor Daughter; P - Handicapped Son; G - Handicapped Daughter; J - Dependent Father; K - Dependent Mother; H - Husband; W - Wife;"
                    ),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    branch_id = table.Column<long>(type: "bigint", nullable: false),
                    bank_ac_no = table.Column<string>(
                        type: "character varying(30)",
                        maxLength: 30,
                        nullable: false
                    ),
                    account_holder_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    pay_mode = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false
                    ),
                    pensioner_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    date_of_death = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false,
                        comment: "M - Male; F - Female;"
                    ),
                    mobile_number = table.Column<string>(
                        type: "character varying(10)",
                        maxLength: 10,
                        nullable: true
                    ),
                    email_id = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    pensioner_address = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    identification_mark = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    pan_no = table.Column<string>(
                        type: "character varying(10)",
                        maxLength: 10,
                        nullable: true
                    ),
                    aadhaar_no = table.Column<string>(
                        type: "character varying(12)",
                        maxLength: 12,
                        nullable: true
                    ),
                    date_of_retirement = table.Column<DateOnly>(type: "date", nullable: false),
                    date_of_commencement = table.Column<DateOnly>(type: "date", nullable: false),
                    basic_pension_amount = table.Column<int>(type: "integer", nullable: false),
                    commuted_from_date = table.Column<DateOnly>(type: "date", nullable: true),
                    commuted_upto_date = table.Column<DateOnly>(type: "date", nullable: true),
                    commuted_pension_amount = table.Column<int>(type: "integer", nullable: false),
                    enhance_pension_amount = table.Column<int>(type: "integer", nullable: false),
                    reduced_pension_amount = table.Column<int>(type: "integer", nullable: false),
                    religion = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false,
                        comment: "H - Hindu; M - Muslim; O - Other;"
                    ),
                    efp_amount = table.Column<int>(type: "integer", nullable: true),
                    efp_wef_date = table.Column<DateOnly>(type: "date", nullable: true),
                    efp_upto_date = table.Column<DateOnly>(type: "date", nullable: true),
                    nfp_amount = table.Column<int>(type: "integer", nullable: true),
                    nfp_wef_date = table.Column<DateOnly>(type: "date", nullable: true),
                    notional_pension_amount = table.Column<int>(type: "integer", nullable: true),
                    notional_wef_date = table.Column<DateOnly>(type: "date", nullable: true),
                    gpf_tpf_no = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    pensioner_status = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    first_pension_generated = table.Column<bool>(
                        type: "boolean",
                        nullable: true,
                        defaultValue: false
                    ),
                    health_scheme = table.Column<bool>(
                        type: "boolean",
                        nullable: true,
                        defaultValue: false
                    ),
                    employed_pensioner = table.Column<bool>(
                        type: "boolean",
                        nullable: true,
                        defaultValue: false
                    ),
                    re_employed_pensioner = table.Column<bool>(
                        type: "boolean",
                        nullable: true,
                        defaultValue: false
                    ),
                    double_pension = table.Column<bool>(
                        type: "boolean",
                        nullable: true,
                        defaultValue: false
                    ),
                    adhoc_pension = table.Column<bool>(
                        type: "boolean",
                        nullable: true,
                        defaultValue: false
                    ),
                    provisional_pension = table.Column<bool>(
                        type: "boolean",
                        nullable: true,
                        defaultValue: false
                    ),
                    interim_allowance = table.Column<bool>(
                        type: "boolean",
                        nullable: true,
                        defaultValue: false
                    ),
                    shared_pension = table.Column<bool>(
                        type: "boolean",
                        nullable: true,
                        defaultValue: false
                    ),
                    remarks = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pensioners_pkey", x => x.id);
                    table.ForeignKey(
                        name: "pensioners_branch_id_fkey",
                        column: x => x.branch_id,
                        principalSchema: "cts_pension",
                        principalTable: "branches",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "pensioners_category_id_fkey",
                        column: x => x.category_id,
                        principalSchema: "cts_pension",
                        principalTable: "categories",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "pensioners_receipt_id_fkey",
                        column: x => x.receipt_id,
                        principalSchema: "cts_pension",
                        principalTable: "ppo_receipts",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "life_certificates",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: false),
                    treasury_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    pensioner_id = table.Column<long>(type: "bigint", nullable: false),
                    ppo_id = table.Column<int>(type: "integer", nullable: false),
                    digital_mode = table.Column<bool>(
                        type: "boolean",
                        nullable: true,
                        defaultValue: false
                    ),
                    certificate_submitted = table.Column<bool>(
                        type: "boolean",
                        nullable: true,
                        defaultValue: false
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("life_certificates_pkey", x => x.id);
                    table.ForeignKey(
                        name: "life_certificates_pensioner_id_fkey",
                        column: x => x.pensioner_id,
                        principalSchema: "cts_pension",
                        principalTable: "pensioners",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "nominees",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: false),
                    treasury_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    family_pension = table.Column<bool>(type: "boolean", nullable: true),
                    refused = table.Column<bool>(type: "boolean", nullable: true),
                    nominee_active = table.Column<bool>(type: "boolean", nullable: true),
                    serial_no = table.Column<int>(type: "integer", nullable: false),
                    pensioner_id = table.Column<long>(type: "bigint", nullable: false),
                    ppo_id = table.Column<int>(type: "integer", nullable: false),
                    nominee_name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    relation = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false
                    ),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    date_of_death = table.Column<DateOnly>(type: "date", nullable: true),
                    nominee_type = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: true
                    ),
                    nominee_adult_minor = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: true
                    ),
                    nominee_priority = table.Column<int>(type: "integer", nullable: true),
                    nominee_share = table.Column<int>(type: "integer", nullable: true),
                    bank_ac_no = table.Column<string>(
                        type: "character varying(30)",
                        maxLength: 30,
                        nullable: true
                    ),
                    branch_id = table.Column<long>(type: "bigint", nullable: true),
                    identification_mark = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true
                    ),
                    handicapped = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("nominees_pkey", x => x.id);
                    table.ForeignKey(
                        name: "nominees_branch_id_fkey",
                        column: x => x.branch_id,
                        principalSchema: "cts_pension",
                        principalTable: "branches",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "nominees_pensioner_id_fkey",
                        column: x => x.pensioner_id,
                        principalSchema: "cts_pension",
                        principalTable: "pensioners",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "ppo_bills",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: false),
                    treasury_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    bill_id = table.Column<long>(type: "bigint", nullable: false),
                    pensioner_id = table.Column<long>(type: "bigint", nullable: false),
                    ppo_id = table.Column<int>(type: "integer", nullable: false),
                    bill_type = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: false,
                        comment: "F - First Bill; R - Regular Bill;"
                    ),
                    utr_no = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true,
                        comment: "UTRNo to refer to the actual transaction of the payment"
                    ),
                    utr_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        comment: "UTRAt timestamp when the UTR is received"
                    ),
                    gross_amount = table.Column<int>(type: "integer", nullable: false),
                    bytransfer_amount = table.Column<int>(type: "integer", nullable: false),
                    net_amount = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("ppo_bills_pkey", x => x.id);
                    table.ForeignKey(
                        name: "ppo_bills_bill_id_fkey",
                        column: x => x.bill_id,
                        principalSchema: "cts_pension",
                        principalTable: "bills",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "ppo_bills_pensioner_id_fkey",
                        column: x => x.pensioner_id,
                        principalSchema: "cts_pension",
                        principalTable: "pensioners",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "ppo_component_revisions",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false, comment: "RevisionId")
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    pensioner_id = table.Column<long>(type: "bigint", nullable: false),
                    ppo_id = table.Column<int>(type: "integer", nullable: false),
                    rate_id = table.Column<long>(type: "bigint", nullable: false),
                    from_date = table.Column<DateOnly>(
                        type: "date",
                        nullable: false,
                        comment: "From date is the Date of Commencement of pension of the pensioner"
                    ),
                    to_date = table.Column<DateOnly>(
                        type: "date",
                        nullable: true,
                        comment: "To date (will be null for regular active bills)"
                    ),
                    amount_per_month = table.Column<int>(
                        type: "integer",
                        nullable: false,
                        comment: "Amount per month is the actual amount paid for the mentioned period"
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("ppo_component_revisions_pkey", x => x.id);
                    table.ForeignKey(
                        name: "ppo_component_revisions_pensioner_id_fkey",
                        column: x => x.pensioner_id,
                        principalSchema: "cts_pension",
                        principalTable: "pensioners",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "ppo_component_revisions_rate_id_fkey",
                        column: x => x.rate_id,
                        principalSchema: "cts_pension",
                        principalTable: "component_rates",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "ppo_sanction_details",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    treasury_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    pensioner_id = table.Column<long>(type: "bigint", nullable: false),
                    ppo_id = table.Column<int>(type: "integer", nullable: false),
                    employee_name = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    sanction_authority = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false
                    ),
                    sanction_no = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false
                    ),
                    sanction_date = table.Column<DateOnly>(type: "date", nullable: false),
                    employee_dob = table.Column<DateOnly>(type: "date", nullable: true),
                    employee_gender = table.Column<char>(
                        type: "character(1)",
                        maxLength: 1,
                        nullable: true
                    ),
                    employee_date_of_appointment = table.Column<DateOnly>(
                        type: "date",
                        nullable: true
                    ),
                    employee_office = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    employee_designation = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    employee_last_pay = table.Column<int>(type: "integer", nullable: true),
                    average_emolument = table.Column<int>(type: "integer", nullable: true),
                    employee_hrms_id = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    issuing_authority = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    issuing_letter_no = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    issuing_letter_date = table.Column<DateOnly>(type: "date", nullable: true),
                    qualifying_service_gross_years = table.Column<int>(
                        type: "integer",
                        nullable: true
                    ),
                    qualifying_service_gross_months = table.Column<int>(
                        type: "integer",
                        nullable: true
                    ),
                    qualifying_service_gross_days = table.Column<int>(
                        type: "integer",
                        nullable: true
                    ),
                    qualifying_service_net_years = table.Column<int>(
                        type: "integer",
                        nullable: true
                    ),
                    qualifying_service_net_months = table.Column<int>(
                        type: "integer",
                        nullable: true
                    ),
                    qualifying_service_net_days = table.Column<int>(
                        type: "integer",
                        nullable: true
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("ppo_sanction_details_pkey", x => x.id);
                    table.ForeignKey(
                        name: "ppo_sanction_details_pensioner_id_fkey",
                        column: x => x.pensioner_id,
                        principalSchema: "cts_pension",
                        principalTable: "pensioners",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "ppo_status_flags",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: false),
                    treasury_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    pensioner_id = table.Column<long>(type: "bigint", nullable: false),
                    ppo_id = table.Column<int>(type: "integer", nullable: false),
                    status_wef = table.Column<DateOnly>(type: "date", nullable: false),
                    status_upto = table.Column<DateOnly>(type: "date", nullable: true),
                    reason_remark = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                    status_flag = table.Column<int>(type: "integer", nullable: false),
                    reason_flag = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("ppo_status_flags_pkey", x => x.id);
                    table.ForeignKey(
                        name: "ppo_status_flags_pensioner_id_fkey",
                        column: x => x.pensioner_id,
                        principalSchema: "cts_pension",
                        principalTable: "pensioners",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "bytransfers",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: false),
                    treasury_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    ppo_bill_id = table.Column<long>(type: "bigint", nullable: false),
                    bytransfer_hoa_id = table.Column<int>(type: "integer", nullable: false),
                    bytransfer_wef = table.Column<DateOnly>(type: "date", nullable: false),
                    bytransfer_amount = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("bytransfers_pkey", x => x.id);
                    table.ForeignKey(
                        name: "bytransfers_ppo_bill_id_fkey",
                        column: x => x.ppo_bill_id,
                        principalSchema: "cts_pension",
                        principalTable: "ppo_bills",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateTable(
                name: "ppo_bill_breakups",
                schema: "cts_pension",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    financial_year = table.Column<int>(type: "integer", nullable: false),
                    treasury_code = table.Column<string>(
                        type: "character varying(3)",
                        maxLength: 3,
                        nullable: false
                    ),
                    ppo_id = table.Column<int>(type: "integer", nullable: false),
                    ppo_bill_id = table.Column<long>(
                        type: "bigint",
                        nullable: false,
                        comment: "BillId is to identify the bill on which the actual payment made"
                    ),
                    revision_id = table.Column<long>(
                        type: "bigint",
                        nullable: false,
                        comment: "RevisionId is to identify the component rate applied on the bill"
                    ),
                    from_date = table.Column<DateOnly>(type: "date", nullable: false),
                    to_date = table.Column<DateOnly>(type: "date", nullable: false),
                    breakup_amount = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp without time zone",
                        nullable: true
                    ),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("ppo_bill_breakups_pkey", x => x.id);
                    table.ForeignKey(
                        name: "ppo_bill_breakups_ppo_bill_id_fkey",
                        column: x => x.ppo_bill_id,
                        principalSchema: "cts_pension",
                        principalTable: "ppo_bills",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "ppo_bill_breakups_revision_id_fkey",
                        column: x => x.revision_id,
                        principalSchema: "cts_pension",
                        principalTable: "ppo_component_revisions",
                        principalColumn: "id"
                    );
                },
                comment: "PensionModuleSchema v1"
            );

            migrationBuilder.CreateIndex(
                name: "banks_bank_name_key",
                schema: "cts_pension",
                table: "banks",
                column: "bank_name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_bills_account_head_id",
                schema: "cts_pension",
                table: "bills",
                column: "account_head_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_bills_branch_id",
                schema: "cts_pension",
                table: "bills",
                column: "branch_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_branches_bank_id",
                schema: "cts_pension",
                table: "branches",
                column: "bank_id"
            );

            migrationBuilder.CreateIndex(
                name: "breakups_component_name_key",
                schema: "cts_pension",
                table: "breakups",
                column: "component_name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_bytransfers_ppo_bill_id",
                schema: "cts_pension",
                table: "bytransfers",
                column: "ppo_bill_id"
            );

            migrationBuilder.CreateIndex(
                name: "categories_primary_category_id_sub_category_id_key",
                schema: "cts_pension",
                table: "categories",
                columns: new[] { "primary_category_id", "sub_category_id" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_categories_sub_category_id",
                schema: "cts_pension",
                table: "categories",
                column: "sub_category_id"
            );

            migrationBuilder.CreateIndex(
                name: "classifications_classification_name_key",
                schema: "cts_pension",
                table: "classifications",
                column: "classification_name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_classifications_account_head_id",
                schema: "cts_pension",
                table: "classifications",
                column: "account_head_id"
            );

            migrationBuilder.CreateIndex(
                name: "component_rates_category_id_breakup_id_effective_from_date_key",
                schema: "cts_pension",
                table: "component_rates",
                columns: new[] { "category_id", "breakup_id", "effective_from_date" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_component_rates_breakup_id",
                schema: "cts_pension",
                table: "component_rates",
                column: "breakup_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_eppo_amounts_category_id",
                schema: "cts_pension",
                table: "eppo_amounts",
                column: "category_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_eppo_amounts_classification_id",
                schema: "cts_pension",
                table: "eppo_amounts",
                column: "classification_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_eppo_amounts_eppo_receipt_id",
                schema: "cts_pension",
                table: "eppo_amounts",
                column: "eppo_receipt_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_eppo_nominees_eppo_receipt_id",
                schema: "cts_pension",
                table: "eppo_nominees",
                column: "eppo_receipt_id"
            );

            migrationBuilder.CreateIndex(
                name: "eppo_receipts_pension_appln_no_key",
                schema: "cts_pension",
                table: "eppo_receipts",
                column: "pension_appln_no",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_eppo_receipts_eppo_file_id",
                schema: "cts_pension",
                table: "eppo_receipts",
                column: "eppo_file_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_eppo_receipts_photo_file_id",
                schema: "cts_pension",
                table: "eppo_receipts",
                column: "photo_file_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_eppo_receipts_signature_file_id",
                schema: "cts_pension",
                table: "eppo_receipts",
                column: "signature_file_id"
            );

            migrationBuilder.CreateIndex(
                name: "eppo_revisions_pension_appln_no_key",
                schema: "cts_pension",
                table: "eppo_revisions",
                column: "pension_appln_no",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_eppo_revisions_eppo_file_id",
                schema: "cts_pension",
                table: "eppo_revisions",
                column: "eppo_file_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_life_certificates_pensioner_id",
                schema: "cts_pension",
                table: "life_certificates",
                column: "pensioner_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_nominees_branch_id",
                schema: "cts_pension",
                table: "nominees",
                column: "branch_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_nominees_pensioner_id",
                schema: "cts_pension",
                table: "nominees",
                column: "pensioner_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_pensioners_branch_id",
                schema: "cts_pension",
                table: "pensioners",
                column: "branch_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_pensioners_category_id",
                schema: "cts_pension",
                table: "pensioners",
                column: "category_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_pensioners_receipt_id",
                schema: "cts_pension",
                table: "pensioners",
                column: "receipt_id"
            );

            migrationBuilder.CreateIndex(
                name: "pensioners_ppo_no_key",
                schema: "cts_pension",
                table: "pensioners",
                column: "ppo_no",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_ppo_bill_breakups_ppo_bill_id",
                schema: "cts_pension",
                table: "ppo_bill_breakups",
                column: "ppo_bill_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ppo_bill_breakups_revision_id",
                schema: "cts_pension",
                table: "ppo_bill_breakups",
                column: "revision_id"
            );

            migrationBuilder.CreateIndex(
                name: "ppo_bill_breakups_treasury_code_ppo_id_ppo_bill_id_revision_key",
                schema: "cts_pension",
                table: "ppo_bill_breakups",
                columns: new[]
                {
                    "treasury_code",
                    "ppo_id",
                    "ppo_bill_id",
                    "revision_id",
                    "from_date",
                },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_ppo_bills_bill_id",
                schema: "cts_pension",
                table: "ppo_bills",
                column: "bill_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ppo_bills_pensioner_id",
                schema: "cts_pension",
                table: "ppo_bills",
                column: "pensioner_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ppo_component_revisions_pensioner_id",
                schema: "cts_pension",
                table: "ppo_component_revisions",
                column: "pensioner_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ppo_component_revisions_rate_id",
                schema: "cts_pension",
                table: "ppo_component_revisions",
                column: "rate_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ppo_receipts_eppo_receipt_id",
                schema: "cts_pension",
                table: "ppo_receipts",
                column: "eppo_receipt_id"
            );

            migrationBuilder.CreateIndex(
                name: "ppo_receipts_ppo_no_key",
                schema: "cts_pension",
                table: "ppo_receipts",
                column: "ppo_no",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ppo_receipts_treasury_receipt_no_key",
                schema: "cts_pension",
                table: "ppo_receipts",
                column: "treasury_receipt_no",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_ppo_sanction_details_pensioner_id",
                schema: "cts_pension",
                table: "ppo_sanction_details",
                column: "pensioner_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ppo_status_flags_pensioner_id",
                schema: "cts_pension",
                table: "ppo_status_flags",
                column: "pensioner_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_primary_categories_account_head_id",
                schema: "cts_pension",
                table: "primary_categories",
                column: "account_head_id"
            );

            migrationBuilder.CreateIndex(
                name: "primary_categories_primary_category_name_key",
                schema: "cts_pension",
                table: "primary_categories",
                column: "primary_category_name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "sub_categories_sub_category_name_key",
                schema: "cts_pension",
                table: "sub_categories",
                column: "sub_category_name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "treasuries_treasury_code_key",
                schema: "cts_pension",
                table: "treasuries",
                column: "treasury_code",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "bytransfers", schema: "cts_pension");

            migrationBuilder.DropTable(name: "dml_history", schema: "cts_pension");

            migrationBuilder.DropTable(name: "eppo_amounts", schema: "cts_pension");

            migrationBuilder.DropTable(name: "eppo_nominees", schema: "cts_pension");

            migrationBuilder.DropTable(name: "eppo_revisions", schema: "cts_pension");

            migrationBuilder.DropTable(name: "financial_years", schema: "cts_pension");

            migrationBuilder.DropTable(name: "life_certificates", schema: "cts_pension");

            migrationBuilder.DropTable(name: "nominees", schema: "cts_pension");

            migrationBuilder.DropTable(name: "ppo_bill_breakups", schema: "cts_pension");

            migrationBuilder.DropTable(name: "ppo_id_sequences", schema: "cts_pension");

            migrationBuilder.DropTable(name: "ppo_receipt_sequences", schema: "cts_pension");

            migrationBuilder.DropTable(name: "ppo_sanction_details", schema: "cts_pension");

            migrationBuilder.DropTable(name: "ppo_status_flags", schema: "cts_pension");

            migrationBuilder.DropTable(name: "treasuries", schema: "cts_pension");

            migrationBuilder.DropTable(name: "classifications", schema: "cts_pension");

            migrationBuilder.DropTable(name: "ppo_bills", schema: "cts_pension");

            migrationBuilder.DropTable(name: "ppo_component_revisions", schema: "cts_pension");

            migrationBuilder.DropTable(name: "bills", schema: "cts_pension");

            migrationBuilder.DropTable(name: "pensioners", schema: "cts_pension");

            migrationBuilder.DropTable(name: "component_rates", schema: "cts_pension");

            migrationBuilder.DropTable(name: "branches", schema: "cts_pension");

            migrationBuilder.DropTable(name: "ppo_receipts", schema: "cts_pension");

            migrationBuilder.DropTable(name: "breakups", schema: "cts_pension");

            migrationBuilder.DropTable(name: "categories", schema: "cts_pension");

            migrationBuilder.DropTable(name: "banks", schema: "cts_pension");

            migrationBuilder.DropTable(name: "eppo_receipts", schema: "cts_pension");

            migrationBuilder.DropTable(name: "primary_categories", schema: "cts_pension");

            migrationBuilder.DropTable(name: "sub_categories", schema: "cts_pension");

            migrationBuilder.DropTable(name: "uploaded_files", schema: "cts_pension");

            migrationBuilder.DropTable(name: "account_heads", schema: "cts_pension");
        }
    }
}
