namespace BitRexSql.ModelBitRex
{
    using BitRexSql.Entity;
    using Bittrex;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ModelBitRex : DbContext
    {
        // Il contesto è stato configurato per utilizzare una stringa di connessione 'ModelBitRex' dal file di configurazione 
        // dell'applicazione (App.config o Web.config). Per impostazione predefinita, la stringa di connessione è destinata al 
        // database 'BitRexSql.ModelBitRex.ModelBitRex' nell'istanza di LocalDb. 
        // 
        // Per destinarla a un database o un provider di database differente, modificare la stringa di connessione 'ModelBitRex' 
        // nel file di configurazione dell'applicazione.
        public ModelBitRex()
            : base("name=ModelBitRex")
        {
        }

        // Aggiungere DbSet per ogni tipo di entità che si desidera includere nel modello. Per ulteriori informazioni 
        // sulla configurazione e sull'utilizzo di un modello Code, vedere http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet< GetMarketSummaryResponse> MarketSummary { get; set; }
        public virtual DbSet<OpenOrder> OpenOrder { get; set; }
        public virtual DbSet<CompletedOrder> CompletedOrder { get; set; }
        public virtual DbSet<AccountBalance> AccountBalance { get; set; }

        
        public virtual DbSet<EventLog> EventLog { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ModelBitRex>(new DropCreateDatabaseIfModelChanges<ModelBitRex>());
            modelBuilder.Entity<AccountBalance>().Property(e => e.Available).HasPrecision(19, 8);
            modelBuilder.Entity<AccountBalance>().Property(e => e.Balance).HasPrecision(19, 8);
            modelBuilder.Entity<AccountBalance>().Property(e => e.Pending).HasPrecision(19, 8);
            
            modelBuilder.Entity<CompletedOrder>().Property(e => e.Limit).HasPrecision(19, 8);
            modelBuilder.Entity<CompletedOrder>().Property(e => e.Price).HasPrecision(19, 8);
            modelBuilder.Entity<CompletedOrder>().Property(e => e.Quantity).HasPrecision(19, 8);
            modelBuilder.Entity<CompletedOrder>().Property(e => e.QuantityRemaining).HasPrecision(19, 8);
            modelBuilder.Entity<CompletedOrder>().Property(e => e.Commission).HasPrecision(19, 8);



            modelBuilder.Entity<OpenOrder>().Property(e => e.Limit).HasPrecision(19, 8);
            modelBuilder.Entity<OpenOrder>().Property(e => e.Price).HasPrecision(19, 8);
            modelBuilder.Entity<OpenOrder>().Property(e => e.Quantity).HasPrecision(19, 8);
            modelBuilder.Entity<OpenOrder>().Property(e => e.QuantityRemaining).HasPrecision(19, 8);

            modelBuilder.Entity<GetMarketSummaryResponse>().Property(e => e.High).HasPrecision(19, 8);
            modelBuilder.Entity<GetMarketSummaryResponse>().Property(e => e.Low).HasPrecision(19, 8);
            modelBuilder.Entity<GetMarketSummaryResponse>().Property(e => e.Last).HasPrecision(19, 8);
            modelBuilder.Entity<GetMarketSummaryResponse>().Property(e => e.Bid).HasPrecision(19, 8);
            modelBuilder.Entity<GetMarketSummaryResponse>().Property(e => e.Ask).HasPrecision(19, 8);
        }
    }


   }