using EvaluationProjects.Interfaces;
using EvaluationProjects.Models.Entities;
using EvaluationProjects.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EvaluationProjects.Persistence.Repository
{
    public class ProjectRepository : GenericRepository<Project>
    {

        public ProjectRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Project>> GetAllProjectWithIncludes(Expression<Func<Project, bool>>? predicate)
        {
            if (predicate is not null)
            {
                //return await _context.Projects.Include(x => x.Assignment)
                //        .ThenInclude(x => x.Evaluations)
                //            .ThenInclude(x => x.Status)
                //     .Include(x => x.Assignment)
                //        .ThenInclude(x => x.Teacher).
                //     Where(predicate).ToListAsync();
                return new List<Project>();
            }
            else
            {
                //return await _context.Projects.Include(x => x.Assignment)
                //       .ThenInclude(x => x.Evaluations)
                //           .ThenInclude(x => x.Status)
                //    .Include(x => x.Assignment)
                //       .ThenInclude(x => x.Teacher).ToListAsync();
                return new List<Project>();
            }
        }
    }
}
