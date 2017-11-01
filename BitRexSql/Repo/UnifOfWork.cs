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

            //Private members corresponding to each concrete repository
            private Repository<OpenOrder> _OpenOrderRepository;
            //Private members corresponding to each concrete repository
            private Repository<AccountBalance> _AccountBalanceRepository;
            //Private members corresponding to each concrete repository
            private Repository<CompletedOrder> _CompletedOrderRepository;
            //Private members corresponding to each concrete repository
            private Repository<EventLog> _EventLogRepository;
            private Repository<RulesBuySell> _RulesRepository;
            private Repository<OrdiniDelBot> _OrdiniDelBotRepository;

            public IRepository<OrdiniDelBot> OrdiniDelBotRepository
            {
                get
                {
                    if (_OrdiniDelBotRepository == null)
                    {
                        _OrdiniDelBotRepository = new Repository<OrdiniDelBot>(dbContext);
                    }
                    return _OrdiniDelBotRepository;
                }

            }
            //Accessors for each private repository, creates repository if null
            public IRepository<RulesBuySell> RulesRepository
            {
                get
                {
                    if (_RulesRepository == null)
                    {
                        _RulesRepository = new Repository<RulesBuySell>(dbContext);
                    }
                    return _RulesRepository;
                }

            }

            //Accessors for each private repository, creates repository if null
            public IRepository<AccountBalance> AccountBalanceRepository
            {
                get
                {
                    if (_AccountBalanceRepository == null)
                    {
                        _AccountBalanceRepository = new Repository<AccountBalance>(dbContext);
                    }
                    return _AccountBalanceRepository;
                }

            }

            //Accessors for each private repository, creates repository if null
            public IRepository<CompletedOrder> CompletedOrderRepository
            {
                get
                {
                    if (_CompletedOrderRepository == null)
                    {
                        _CompletedOrderRepository = new Repository<CompletedOrder>(dbContext);
                    }
                    return _CompletedOrderRepository;
                }

            }
            //Accessors for each private repository, creates repository if null
            public IRepository<OpenOrder> OpenOrderRepository
            {
                get
                {
                    if (_OpenOrderRepository == null)
                    {
                        _OpenOrderRepository = new Repository<OpenOrder>(dbContext);
                    }
                    return _OpenOrderRepository;
                }
            }


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

            
            //Accessors for each private repository, creates repository if null
            public IRepository<EventLog> EventLogRepository
            {
                get
                {
                    if (_EventLogRepository == null)
                    {
                        _EventLogRepository = new Repository<EventLog>(dbContext);
                    }
                    return _EventLogRepository;
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
