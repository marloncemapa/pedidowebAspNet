namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PedidoWeb.Models.PedidoWebContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "PedidoWeb.Models.PedidoWebContext";
        }

        protected override void Seed(PedidoWeb.Models.PedidoWebContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //context.Operacoes.AddOrUpdate(
            //    new PedidoWeb.Models.Operacao { Descricao = "VENDAS NF-E" }                
            //);
            //context.Operacoes.AddOrUpdate(
            //    new PedidoWeb.Models.Operacao { Descricao = "BONIFICAÇÃO" }
            //);
        }
    }
}
