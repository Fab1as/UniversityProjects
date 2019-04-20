using ShopMvc.Data.Interfaces;
using ShopMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopMvc.Data.Repositories
{
    public class LiquidRepository : ILiquidRepository
    {
        private readonly ApplicationDbContext _context;

        public LiquidRepository(ApplicationDbContext context)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(Liquid liquid)
        {
            liquid.Id = 0;
            _context.Liquids.Add(liquid);
            await _context.SaveChangesAsync();
        }

        public Liquid GetById(int? liquidId)
        {
            var liquid = _context.Liquids.Find(liquidId);

            return liquid;
        }

        public Liquid GetByName(string name)
        {
            var liquid = _context.Liquids.Find(name);

            return liquid;
        }

        public async Task Edit(Liquid liquid)
        {
            var entity = _context.Liquids.Find(liquid.Id);
            if (entity == null)
                await _context.Liquids.AddAsync(liquid);
            else
            {
                _context.Liquids.FirstOrDefault(x => x.Id == liquid.Id).Name = liquid.Name;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Liquid liquid)
        {
            _context.Liquids.Remove(liquid);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Liquid> GetAll()
        {
            var liquids = _context.Liquids.AsEnumerable();

            return liquids;
        }
    }
}
