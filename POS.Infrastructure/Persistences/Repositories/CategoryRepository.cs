using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(POSContext context) : base(context) { }
        
        public async Task<BaseEntityResponse<Category>> ListCategories(BaseFiltersRequest filters)
        {
            var response = new BaseEntityResponse<Category>();

            try
            {

                var categories = GetEntityQuery(x => x.AuditDeleteDate == null && x.AuditDeleteDate == null);

                /*
                var categories = (from c in _context.Categories
                                  where c.AuditDeleteUser == null && c.AuditDeleteDate == null
                                  select c).AsNoTracking().AsQueryable();
                */
                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            categories = categories.Where(x => x.Name!.Contains(filters.TextFilter));
                            break;
                        case 2:
                            categories = categories.Where(x => x.Description!.Contains(filters.TextFilter));
                            break;
                        default:
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    categories = categories.Where(x => x.State.Equals(filters.StateFilter));
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    categories = categories.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) && x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                }

                if (filters.Sort is null) filters.Sort = "Id";

                response.TotalRecords = await categories.CountAsync();
                response.Items = await Ordering(filters, categories, !(bool)filters.Download!).ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

            
            return response;

        }

    }
}
