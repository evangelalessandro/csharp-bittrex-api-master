using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRexSql.Repo
{
    using System;
    using BitRexSql.ModelBitRex;
    using BitRexSql.Entity;
    using Bittrex;

    namespace RepoAndUnitOfWork.Domain.Concrete
    {
        public class UnitOfWork : IDisposable
        {
            //Our database context 
            private BitRexSql.ModelBitRex.ModelBitRex dbContext = new BitRexSql.ModelBitRex.ModelBitRex();

            //Private members corresponding to each concrete repository
            private Repository<GetMarketSummaryResponse> _MarketSummaryResponseRepository;

            //Accessors for each private repository, creates repository if null
            public IRepository<GetMarketSummaryResponse> MarketSummaryResponseRepository
            {
                get
                {
                    if (_MarketSummaryResponseRepository == null)
                    {
                        _MarketSummaryResponseRepository = new Repository<GetMarketSummaryResponse>(dbContext);
                    }
                    return _MarketSummaryResponseRepository;
                }

            }

            //Method to save all changes to repositories
            public void Commit()
            {
                dbContext.SaveChanges();
            }

            //IDisposible implementation
            private bool disposed = false;

            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        dbContext.Dispose();
                    }
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

    }
}
