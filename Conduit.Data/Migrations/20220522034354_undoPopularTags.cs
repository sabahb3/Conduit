using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class undoPopularTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS dbo.PopularTags;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE FUNCTION PopularTags() 
                                    RETURNS TABLE  
                                    AS  
                                    RETURN  
                                        SELECT  TOP 4 Tag
                                        FROM ArticlesTags
                                        GROUP BY Tag
                                        ORDER BY COUNT(Tag) DESC");
        }
    }
}
