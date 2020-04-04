using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PickleAndHope.Models;

namespace PickleAndHope.DataAccess
{
    public class PickleRepository
    {
        static List<Pickle> _pickles = new List<Pickle> 
        {
            new Pickle
            {
                Type = "bread and butter",
                NumberInStock = 3,
                Id = 1
            }
        };
        
        public void Add(Pickle pickle)
        {
            pickle.Id = _pickles.Max(x => x.Id) + 1;
            _pickles.Add(pickle);
        }

        public void Remove(string type)
        {

        }

        public Pickle UpdateNumInStock(Pickle pickle)
        {
            var pickleToUpdate = _pickles.First(p => p.Type == pickle.Type);
            pickleToUpdate.NumberInStock += pickle.NumberInStock;

            return pickleToUpdate;
        }

        public Pickle GetByType(string type)
        {
            return _pickles.FirstOrDefault(p => p.Type == type);
        }

        public List<Pickle> GetAllPickles()
        {
            return _pickles;
        }

        public Pickle GetById(int id)
        {
            return _pickles.FirstOrDefault(p => p.Id == id);
        }
    }
}
