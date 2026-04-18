using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedureLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE PROCEDURE public.sp_record_log(
                    p_endpoint TEXT,
                    p_username TEXT,
                    p_action TEXT
                )
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    INSERT INTO ""ApiLogs"" (""Endpoint"", ""Username"", ""Action"", ""Date"")
                    VALUES (p_endpoint, p_username, p_action, NOW());
                END;
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS public.sp_record_log;");
        }
    }
}
