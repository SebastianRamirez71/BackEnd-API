using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.ProductDTOs;

using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Back_End_TPI_PSS.Services.Implementations
{
    public class ProductService : IProductService
    {                                               // solo para tipo de usuario : admin/empleado
        private readonly PPSContext _context;
        public ProductService(PPSContext context)
        {
            _context = context;
        }

        public List<Product> GetProducts()
        {
            return _context.Products
                .Include(p => p.Colours)
                .Include(p => p.Sizes)
                .ToList();
        }

        public bool AddProduct(ProductDto productDto)
        {
            var newProduct = new Product()
            {
                Description = productDto.Description,
                Genre = productDto.Genre,
                Price = productDto.Price,
                Category = productDto.Category,
                Image = productDto.Image,
                Status = true
            };
            
            foreach (var colourId in productDto.ColourId)
            {
                var existingColour = _context.Colours.FirstOrDefault(c => c.Id == colourId);
                if (existingColour == null)
                {
                    throw new ArgumentException($"El Color con el  ID: {colourId} no existe");
                }
                newProduct.Colours.Add(existingColour);
            }

            foreach (var sizeId in productDto.SizeId)
            {
                var existingSize = _context.Sizes.FirstOrDefault(s => s.Id == sizeId);
                if (existingSize == null)
                {
                    throw new ArgumentException($"El talle con el ID: {sizeId} no existe");
                }
                newProduct.Sizes.Add(existingSize);
            }

            _context.Products.Add(newProduct);
            _context.SaveChanges();
            return true;
        }

        public bool CheckIfColourExists(string colour)
        {
            return _context.Colours.Any(c => c.ColourName == colour);
        }
        public bool CheckIfSizeExists(string size)
        {
            return _context.Sizes.Any(s => s.SizeName == size);
        }

        public int AddColour(string colour)
        {
            bool existingColour = CheckIfColourExists(colour);
            if (!existingColour)
            {
                Colour colourToAdd = new ()
                {
                    ColourName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(colour.ToLower()),
                };
                _context.Colours.Add(colourToAdd);
                _context.SaveChanges();
                return colourToAdd.Id;
            }
            return 1;
        }

        public int AddSize(string size)
        {
            bool existingSize = CheckIfSizeExists(size);
            if (!existingSize)
            {
                Size sizeToAdd = new()
                {
                    SizeName = size.ToUpper(),
                };
                _context.Sizes.Add(sizeToAdd);
                _context.SaveChanges();
                return sizeToAdd.Id;
            }
            return 1;
        }

        public List<Colour> GetColours()
        {
            return _context.Colours.ToList();
        }

        public List<Size> GetSizes()
        {
            return _context.Sizes.ToList();
        }
    }
}
