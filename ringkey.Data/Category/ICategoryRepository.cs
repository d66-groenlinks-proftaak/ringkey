using ringkey.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Data
{
    public interface ICategoryRepository : IRepository<Category>
    {
        List<Category> GetCategories();
        void AddCategory(string name, string icon);
        void Remove(string name);
        void HideCategory(string name, bool hidden);
    }
}
