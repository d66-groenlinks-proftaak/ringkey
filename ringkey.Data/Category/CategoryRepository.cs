using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ringkey.Common.Models.Messages;

namespace ringkey.Data
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {


        public CategoryRepository(RingkeyDbContext context) : base(context)
        {
        }

        public void AddCategory(string name, string icon)
        {
            Category category = new Category
            {
                Id = Guid.NewGuid(),
                Name = name,
                Icon = icon
            };

            _dbContext.Category.Add(category);
            _dbContext.SaveChanges();
        }

        public List<Category> GetCategories()
        {
            List<Category> c = _dbContext.Category.ToList();
            return c;
        }

        public void HideCategory(string name, bool hidden)
        {
            Category c = _dbContext.Category.Where(c => c.Name.Equals(name)).FirstOrDefault();
            _dbContext.Category.Update(c);
            c.Hidden = hidden;
            _dbContext.SaveChanges();
        }

        public void Remove(string name)
        {
            _dbContext.Category.Remove(_dbContext.Category.Where(category => category.Name == name).FirstOrDefault());
            _dbContext.SaveChanges();
        }
    }
}
