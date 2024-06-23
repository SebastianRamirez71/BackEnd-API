using AutoMapper;
using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.ProductDTOs;
using Back_End_TPI_PSS.Mappings;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = Back_End_TPI_PSS.Data.Entities.Image;

namespace Back_End_TPI_PSS.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly PPSContext _context;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public ProductService(PPSContext context, IEmailService emailService)
        {
            _context = context;
            _mapper = AutoMapperConfig.Configure();
            _emailService = emailService;
        }

        public async Task<IEnumerable<Product>> GetProducts(string? priceOrder, string? size, string? genre, string? category, string? colour, string? dateOrder)
        {
            var productsQuery = _context.Products
           .Include(p => p.Stocks).ThenInclude(s => s.Images)
           .Include(p => p.Stocks).ThenInclude(s => s.StockSizes).ThenInclude(ss => ss.Size)
           .Include(p => p.Stocks).ThenInclude(s => s.Colour)
           .Include(p => p.Categories)
           .Where(x => x.Status == true)
           .AsQueryable();

            if (!string.IsNullOrWhiteSpace(genre))
            {
                productsQuery = productsQuery.Where(x => x.Genre == genre);
            }
            if (!string.IsNullOrWhiteSpace(category))
            {
                productsQuery = productsQuery.Where(x => x.Categories.Any(c => c.CategoryName == category));
            }
            if (!string.IsNullOrWhiteSpace(colour))
            {
                productsQuery = productsQuery.Where(x => x.Stocks.Any(s => s.Colour.ColourName == colour));
            }
            if (!string.IsNullOrWhiteSpace(size))
            {
                productsQuery = productsQuery.Where(x => x.Stocks.Any(s => s.StockSizes.Any(ss => ss.Size.SizeName == size)));
            }

            var products = await productsQuery.ToListAsync();

            if (!string.IsNullOrWhiteSpace(priceOrder))
            {
                if (priceOrder == "desc")
                {
                    products = products.OrderByDescending(x => x.Price).ToList();
                }
                else if (priceOrder == "asc")
                {
                    products = products.OrderBy(x => x.Price).ToList();
                }
            }

            if (!string.IsNullOrWhiteSpace(dateOrder))
            {
                if (dateOrder == "desc")
                {
                    products = products.OrderByDescending(x => x.CreatedDate).ToList();
                }
                else if (dateOrder == "asc")
                {
                    products = products.OrderBy(x => x.CreatedDate).ToList();
                }
            }
            return products;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products
                .Include(p => p.Stocks).ThenInclude(s => s.Images)
                .Include(p => p.Stocks).ThenInclude(s => s.StockSizes).ThenInclude(ss => ss.Size)
                .Include(p => p.Stocks).ThenInclude(s => s.Colour)
                .Include(p => p.Categories)
                .ToListAsync();

        }

        public async Task<Product> GetByDescription(string description)
        {

            var product = await _context.Products
                .Include(p => p.Stocks).ThenInclude(s => s.Images)
                .Include(p => p.Stocks).ThenInclude(s => s.Colour)
                .Include(p => p.Stocks).ThenInclude(s => s.StockSizes).ThenInclude(ss => ss.Size)
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Description.Equals(description));
            return product;

        }

        public bool EditProductById(int id, ProductToEditDto productToEditDto)
        {
            var productToEdit = _context.Products
                .Include(p => p.Stocks).ThenInclude(s => s.Images)
                .Include(p => p.Stocks).ThenInclude(s => s.StockSizes).ThenInclude(ss => ss.Size)
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.Id == id);

            if (productToEdit != null)
            {
                productToEdit.Description = productToEditDto.Description;
                productToEdit.Price = productToEditDto.Price;
                productToEdit.Image = productToEditDto.Image;
                productToEdit.Genre = productToEditDto.Genre;
                productToEdit.Status = true;

                productToEdit.Categories.Clear();

                foreach (var categoryId in productToEditDto.Category)
                {
                    var selectedCategory = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
                    if (selectedCategory == null)
                    {
                        throw new ArgumentException($"La categoría con el ID: {categoryId} no existe");
                    }
                    productToEdit.Categories.Add(selectedCategory);
                }

                productToEdit.Stocks.Clear();
                foreach (var stockDto in productToEditDto.Stocks)
                {
                    Stock newStock = new Stock
                    {
                        ColourId = stockDto.ColourId,
                        Status = stockDto.Status,
                    };

                    foreach (var sizeDto in stockDto.StockSizes)
                    {
                        var existingSize = _context.Sizes.FirstOrDefault(s => s.Id == sizeDto.SizeId);
                        if (existingSize == null)
                        {
                            throw new ArgumentException($"El talle con el ID: {sizeDto.SizeId} no existe");
                        }
                        newStock.StockSizes.Add(new StockSize { SizeId = sizeDto.SizeId, Quantity = sizeDto.Quantity });
                    }

                    foreach (var imageDto in stockDto.Images)
                    {
                        newStock.Images.Add(new Image { ImageURL = imageDto.Image });
                    }

                    productToEdit.Stocks.Add(newStock);
                }

                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool AddProduct(ProductDto productDto)
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.Description == productDto.Description);

            if (existingProduct == null)
            {
                Product newProduct = new Product()
                {
                    Description = productDto.Description,
                    Genre = productDto.Genre,
                    Price = productDto.Price,
                    Image = productDto.Image,
                    CreatedDate = DateTime.Now,
                    Status = true
                };

                if(newProduct.Price < 20000)
                {
                    _emailService.Notify(newProduct);
                }

                foreach (var categoryId in productDto.Category)
                {
                    var selectedCategory = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
                    if (selectedCategory == null)
                    {
                        throw new ArgumentException($"La categoría con el ID: {categoryId} no existe");
                    }
                    newProduct.Categories.Add(selectedCategory);
                }

                // HashSet para mantener un registro de ColourId únicos en el nuevo producto
                HashSet<int> colourIds = new HashSet<int>();

                foreach (var stockDto in productDto.Stocks)
                {
                    if (colourIds.Contains(stockDto.ColourId))
                    {
                        throw new ArgumentException($"El color con el ID: {stockDto.ColourId} ya está agregado en este producto.");
                    }

                    colourIds.Add(stockDto.ColourId);

                    Stock newStock = new Stock
                    {
                        ColourId = stockDto.ColourId
                    };

                    foreach (var sizeDto in stockDto.StockSizes)
                    {
                        var existingSize = _context.Sizes.FirstOrDefault(s => s.Id == sizeDto.SizeId);
                        if (existingSize == null)
                        {
                            throw new ArgumentException($"El talle con el ID: {sizeDto.SizeId} no existe");
                        }

                        // Verificación para evitar talles duplicados
                        if (newStock.StockSizes.Any(ss => ss.SizeId == sizeDto.SizeId))
                        {
                            throw new ArgumentException($"El talle con el ID: {sizeDto.SizeId} ya está agregado en este stock.");
                        }

                        newStock.StockSizes.Add(new StockSize { SizeId = sizeDto.SizeId, Quantity = sizeDto.Quantity });
                    }

                    foreach (var imageDto in stockDto.Images)
                    {
                        newStock.Images.Add(new Image { ImageURL = imageDto.Image });
                    }

                    newStock.Status = stockDto.Status;
                    newProduct.Stocks.Add(newStock);
                }

                _context.Products.Add(newProduct);
                _context.SaveChanges();
                return true;
            }
            else if (existingProduct != null && existingProduct.Status == false)
            {
                existingProduct.Status = true;
                _context.Products.Update(existingProduct);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool CheckIfColourExists(string colour)
        {
            return _context.Colours.Any(c => c.ColourName == colour);
        }
        public bool CheckIfSizeExists(string size)
        {
            return _context.Sizes.Any(s => s.SizeName == size);
        }
        public bool AddColour(ColourDto colourDto)
        {
            var existingColour = _context.Colours.FirstOrDefault(c => c.ColourName.ToLower() == colourDto.ColourName.ToLower());

            if (existingColour == null)
            {
                Colour colourToAdd = new Colour
                {
                    ColourName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(colourDto.ColourName.ToLower()),
                    Status = true
                };
                _context.Colours.Add(colourToAdd);
                _context.SaveChanges();
                return true;
            }
            if (existingColour != null && existingColour.Status == false)
            {
                existingColour.Status = true;
                _context.Colours.Update(existingColour);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool AddCategory(CategoryDto categoryDto)
        {
            var existingCategory = _context.Categories.FirstOrDefault(c => c.CategoryName.ToLower() == categoryDto.CategoryName.ToLower());

            if (existingCategory == null)
            {
                Category categoryToAdd = new Category
                {
                    CategoryName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(categoryDto.CategoryName.ToLower()),
                    Status = true
                };
                _context.Categories.Add(categoryToAdd);
                _context.SaveChanges();
                return true;
            }
            if (existingCategory != null && existingCategory.Status == false)
            {
                existingCategory.Status = true;
                _context.Categories.Update(existingCategory);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool AddSize(SizeDto sizeDto)
        {
            var existingSize = _context.Sizes.FirstOrDefault(c => c.SizeName.ToLower() == sizeDto.SizeName.ToLower());

            if (existingSize == null)
            {
                Size sizeToAdd = new Size
                {
                    SizeName = sizeDto.SizeName.ToUpper(),
                    Status = true
                };
                _context.Sizes.Add(sizeToAdd);
                _context.SaveChanges();
                return true;
            }

            if (existingSize != null && existingSize.Status == false)
            {
                existingSize.Status = true;
                _context.Sizes.Update(existingSize);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<T> GetActiveEntities<T>() where T : class, IStatusEntity
        {
            return _context.Set<T>().Where(e => e.Status == true).ToList();
        }

        public List<TDto> GetMappedVariants<TEntity, TDto>()
           where TEntity : class, IStatusEntity
           where TDto : class
        {
            var entities = _context.Set<TEntity>().Where(p => p.Status == true).ToList();
            return _mapper.Map<List<TDto>>(entities);
        }

        public void ChangeEntityStatus<T>(int entityId) where T : class, IStatusEntity
        {
            var entityToChangeStatus = _context.Set<T>().FirstOrDefault(e => e.Id == entityId);

            if (entityToChangeStatus != null && entityToChangeStatus.Status == true)
            {
                entityToChangeStatus.Status = false;
                _context.Update(entityToChangeStatus);
                _context.SaveChanges();
            }
            else if (entityToChangeStatus != null && entityToChangeStatus.Status == false)
            {
                entityToChangeStatus.Status = true;
                _context.Update(entityToChangeStatus);
                _context.SaveChanges();
            }
        }
    }
}
