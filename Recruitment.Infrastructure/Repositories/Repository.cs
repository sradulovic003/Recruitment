using Microsoft.EntityFrameworkCore;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Recruitment.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly RecruitmentContext Context;
        protected readonly DbSet<T> DbSet;

        public Repository(RecruitmentContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll() => DbSet.ToList();

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) =>
            DbSet.Where(predicate).ToList();

        public T? GetById(params object[] keyValues) => DbSet.Find(keyValues);

        public void Add(T entity) => DbSet.Add(entity);

        public virtual void Remove(T entity) => DbSet.Remove(entity);

        public void Update(T entity) => DbSet.Update(entity);
    }
}
