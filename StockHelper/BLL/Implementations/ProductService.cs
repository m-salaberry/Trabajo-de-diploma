using BLL.Interfaces;
using DAL.Contracts;
using DAL.Implementations;
using Domain;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
    public class ProductService : GenericBllService<Product, int>
    {
        private readonly ItemService _itemService;
        private readonly DetailProductRepository _detailRepository;
        private ProductService(ProductRepository repository, DetailProductRepository detailRepository) : base(repository)
        {
            _itemService = ItemService.Instance();
            _detailRepository = detailRepository;
        }

        private static ProductService _instance = null;

        public static ProductService Instance()
        {
            if (_instance == null)
            {
                _instance = new ProductService(new ProductRepository(), new DetailProductRepository());
            }
            return _instance;
        }
    }
}
