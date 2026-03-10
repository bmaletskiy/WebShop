using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebShopInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteCategoryProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categoryname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    categoryinfo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("category_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "changetype",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("changetype_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fullname = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    adress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("customer_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orderstatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    statusname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("orderstatus_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categoryid = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    availableqty = table.Column<int>(type: "integer", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("product_pkey", x => x.id);
                    table.ForeignKey(
                        name: "product_categoryid_fkey",
                        column: x => x.categoryid,
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cart",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customerid = table.Column<int>(type: "integer", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("cart_pkey", x => x.id);
                    table.ForeignKey(
                        name: "cart_customerid_fkey",
                        column: x => x.customerid,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customerid = table.Column<int>(type: "integer", nullable: true),
                    orderstatusid = table.Column<int>(type: "integer", nullable: true),
                    orderdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    totalamount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Order_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Order_customerid_fkey",
                        column: x => x.customerid,
                        principalTable: "customer",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Order_orderstatusid_fkey",
                        column: x => x.orderstatusid,
                        principalTable: "orderstatus",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "cartitem",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cartid = table.Column<int>(type: "integer", nullable: true),
                    productid = table.Column<int>(type: "integer", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("cartitem_pkey", x => x.id);
                    table.ForeignKey(
                        name: "cartitem_cartid_fkey",
                        column: x => x.cartid,
                        principalTable: "cart",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "cartitem_productid_fkey",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "orderhistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    orderid = table.Column<int>(type: "integer", nullable: true),
                    changedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    oldvalue = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    newvalue = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    changedtypeid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("orderhistory_pkey", x => x.id);
                    table.ForeignKey(
                        name: "orderhistory_changedtypeid_fkey",
                        column: x => x.changedtypeid,
                        principalTable: "changetype",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "orderhistory_orderid_fkey",
                        column: x => x.orderid,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orderitem",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    orderid = table.Column<int>(type: "integer", nullable: true),
                    productid = table.Column<int>(type: "integer", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unitprice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    totalprice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("orderitem_pkey", x => x.id);
                    table.ForeignKey(
                        name: "orderitem_orderid_fkey",
                        column: x => x.orderid,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "orderitem_productid_fkey",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cart_customerid",
                table: "cart",
                column: "customerid");

            migrationBuilder.CreateIndex(
                name: "IX_cartitem_cartid",
                table: "cartitem",
                column: "cartid");

            migrationBuilder.CreateIndex(
                name: "IX_cartitem_productid",
                table: "cartitem",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "customer_email_key",
                table: "customer",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_customerid",
                table: "Order",
                column: "customerid");

            migrationBuilder.CreateIndex(
                name: "IX_Order_orderstatusid",
                table: "Order",
                column: "orderstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_orderhistory_changedtypeid",
                table: "orderhistory",
                column: "changedtypeid");

            migrationBuilder.CreateIndex(
                name: "IX_orderhistory_orderid",
                table: "orderhistory",
                column: "orderid");

            migrationBuilder.CreateIndex(
                name: "IX_orderitem_orderid",
                table: "orderitem",
                column: "orderid");

            migrationBuilder.CreateIndex(
                name: "IX_orderitem_productid",
                table: "orderitem",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_product_categoryid",
                table: "product",
                column: "categoryid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cartitem");

            migrationBuilder.DropTable(
                name: "orderhistory");

            migrationBuilder.DropTable(
                name: "orderitem");

            migrationBuilder.DropTable(
                name: "cart");

            migrationBuilder.DropTable(
                name: "changetype");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "orderstatus");

            migrationBuilder.DropTable(
                name: "category");
        }
    }
}
