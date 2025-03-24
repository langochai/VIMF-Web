using BMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DASSytemAPI.Repository
{
    public interface IGenericRepository
    {
        Task<List<T>> GetAll<T>() where T : class;

        Task<T> GetById<T>(int id) where T : class;

        Task<T> Insert<T>(T entity) where T : class;

        Task<T> Update<T>(T entity) where T : class;

        Task<T> Delete<T>(T entity) where T : class;

        Task<List<T>> FindByExpression<T>(Expression<Func<T, bool>> predicate) where T : class;

        Task<T> FindModel<T>(Expression<Func<T, bool>> predicate) where T : class;

        // Phương thức cho stored procedures
        Task<List<TResult>> ExecuteStoredProcedure<TResult>(
            Func<IRTCStockManagementContextProcedures, Task<List<TResult>>> procedureCall,
            bool useTransaction = true) where TResult : class;

        Task<(List<TResult>, List<TResult1>)> ExecuteStoredProcedure<TResult, TResult1>(
           Func<IRTCStockManagementContextProcedures, Task<(List<TResult>, List<TResult1>)>> procedureCall,
           bool useTransaction = true) where TResult : class;

        Task<(List<TResult>, List<TResult1>, List<TResult2>)> ExecuteStoredProcedure<TResult, TResult1, TResult2>(
          Func<IRTCStockManagementContextProcedures, Task<(List<TResult>, List<TResult1>, List<TResult2>)>> procedureCall,
          bool useTransaction = true) where TResult : class;
    }

    public class GenericRepository : IGenericRepository
    {
        private readonly RTCStockManagementContext _context;

        public GenericRepository(RTCStockManagementContext context)
        {
            _context = context;
        }

        public async Task<T> Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> GetAll<T>() where T : class
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById<T>(int id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Insert<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Update<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        // Tìm thực thể theo điều kiện (không theo dõi)
        public async Task<List<T>> FindByExpression<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _context.Set<T>()
                .AsNoTracking() // Không theo dõi
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<T> FindModel<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var entity = await _context.Set<T>()
                    .AsNoTracking() // Không theo dõi
                    .FirstOrDefaultAsync(predicate);
            return entity;
        }

        public async Task<(List<TResult>, List<TResult1>)> ExecuteStoredProcedure<TResult, TResult1>(
       Func<IRTCStockManagementContextProcedures, Task<(List<TResult>, List<TResult1>)>> procedureCall,
       bool useTransaction = true) where TResult : class
        {
            try
            {
                if (!useTransaction)
                {
                    return await procedureCall(_context.Procedures);
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var result = await procedureCall(_context.Procedures);
                    await transaction.CommitAsync();
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException(
               $"Error executing stored procedure: {ex.Message}", ex);
            }
        }

        public async Task<List<TResult>> ExecuteStoredProcedure<TResult>(
      Func<IRTCStockManagementContextProcedures, Task<List<TResult>>> procedureCall,
      bool useTransaction = true) where TResult : class
        {
            try
            {
                if (!useTransaction)
                {
                    return await procedureCall(_context.Procedures);
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var result = await procedureCall(_context.Procedures);
                    await transaction.CommitAsync();
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException(
               $"Error executing stored procedure: {ex.Message}", ex);
            }
        }

        public async Task<(List<TResult>, List<TResult1>, List<TResult2>)> ExecuteStoredProcedure<TResult, TResult1, TResult2>(
       Func<IRTCStockManagementContextProcedures, Task<(List<TResult>, List<TResult1>, List<TResult2>)>> procedureCall,
       bool useTransaction = true) where TResult : class
        {
            try
            {
                if (!useTransaction)
                {
                    return await procedureCall(_context.Procedures);
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var result = await procedureCall(_context.Procedures);
                    await transaction.CommitAsync();
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException(
               $"Error executing stored procedure: {ex.Message}", ex);
            }
        }
    }

    public class RepositoryException : Exception
    {
        public RepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}