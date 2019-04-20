using ShopMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopMvc.Data.Interfaces
{
    public interface ILiquidRepository
    {
        Task Add(Liquid liquid);
        Liquid GetById(int? liquidId);
        Liquid GetByName(string name);
        Task Edit(Liquid liquid);
        Task Delete(Liquid liquid);
        IEnumerable<Liquid> GetAll();
    }
}
