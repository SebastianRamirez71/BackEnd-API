using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.ProductDTOs;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Back_End_TPI_PSS.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly PPSContext _context;

        public ProductService(PPSContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProducts(string? priceOrder, string? genre)
        {
            var productsQuery = _context.Products
                .Include(p => p.Colours)
                .Include(p => p.Sizes)
                .Include(p => p.Categories)
                .Where(x => x.Status == true)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(genre))
            {
                productsQuery = productsQuery.Where(p => p.Genre == genre);
            }

            var products = await productsQuery.ToListAsync();

            if (!string.IsNullOrWhiteSpace(priceOrder))
            {
                products = priceOrder == "desc"
                    ? products.OrderByDescending(x => Convert.ToDouble(x.Price)).ToList()
                    : priceOrder == "asc" ? products.OrderBy(x => Convert.ToDouble(x.Price)).ToList()
                    : products;
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return _context.Products
                .Include(p => p.Colours)
                .Include(p => p.Sizes)
                .Include(p => p.Categories)
                .ToList();
        }

        public bool AddProduct(ProductDto productDto)
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.Description == productDto.Description);

            if (existingProduct == null)
            {
                foreach (var categoryId in productDto.CategoryId)
                {
                    var selectedCategory = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
                    if (selectedCategory == null)
                    {
                        throw new ArgumentException($"La categoría con el ID: {categoryId} no existe");
                    }

                    Product newProduct = new Product()
                    {
                        Description = productDto.Description,
                        Genre = productDto.Genre,
                        Category = selectedCategory.CategoryName, // Asignar el nombre de la categoría seleccionada
                        Price = productDto.Price,
                        Image = productDto.Image,
                        CreatedDate = productDto.CreatedDate = DateTime.Now,
                        Status = true
                    };

                    foreach (var colourId in productDto.ColourId)
                    {
                        var existingColour = _context.Colours.FirstOrDefault(c => c.Id == colourId);
                        if (existingColour == null)
                        {
                            throw new ArgumentException($"El Color con el ID: {colourId} no existe");
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

                    foreach (var categoryID in productDto.CategoryId)
                    {
                        var existingCategory = _context.Categories.FirstOrDefault(s => s.Id == categoryID);
                        if (existingCategory == null)
                        {
                            throw new ArgumentException($"El talle con el ID: {categoryID} no existe");
                        }
                        newProduct.Categories.Add(existingCategory);
                    }

                    _context.Products.Add(newProduct);
                    _context.SaveChanges();
                }
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

            // Chequea si el color que se agrega esta en la db y si su estado es falso se cambia a true
            // De esta manera no se duplican registros. Lo mismo debajo para talles y categorias.

            if (existingSize != null && existingSize.Status == false)
            {
                existingSize.Status = true;
                _context.Sizes.Update(existingSize);
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

        public List<Colour> GetColours()
        {
            return _context.Colours.Where(p => p.Status == true).ToList();
        }

        public List<Size> GetSizes()
        {
            return _context.Sizes.Where(p => p.Status == true).ToList();
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.Where(p => p.Status == true).ToList();
        }

        public bool EditProductById(int id, ProductToEditDto productToEditDto)
        {
            var productToEdit = _context.Products
                .Include(p => p.Colours)
                .Include(p => p.Sizes)
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.Id == id);

            if (productToEdit != null)
            {
                // Validar si el producto a editar es igual al producto enviado en el DTO
                if (productToEdit.Description == productToEditDto.Description &&
                    productToEdit.Price == productToEditDto.Price &&
                    productToEdit.Image == productToEditDto.Image &&
                    productToEdit.Genre == productToEditDto.Genre &&
                    productToEdit.Category == productToEditDto.Category &&
                    productToEdit.Categories.All(c => productToEditDto.CategoryId.Any(dto => dto.Id == c.Id)) &&
                    productToEdit.Colours.All(c => productToEditDto.ColourId.Any(dto => dto.Id == c.Id)) &&
                    productToEdit.Sizes.All(c => productToEditDto.SizeId.Any(dto => dto.Id == c.Id)))
                {
                    return false; // Si los productos son iguales, no se realiza la edición
                }

                productToEdit.Description = productToEditDto.Description;
                productToEdit.Price = productToEditDto.Price;
                productToEdit.Image = productToEditDto.Image;
                productToEdit.Genre = productToEditDto.Genre;
                productToEdit.Status = true;


                // Eliminar todas las categorías existentes del producto
                productToEdit.Categories.Clear();

                // Actualizar categorías
                foreach (var categoryId in productToEditDto.CategoryId)
                {
                    var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId.Id);
                    if (category != null)
                    {
                        productToEdit.Categories.Add(category);
                    }
                }

                // Actualizar colores
                productToEdit.Colours.Clear();
                foreach (var colourId in productToEditDto.ColourId)
                {
                    var colour = _context.Colours.FirstOrDefault(c => c.Id == colourId.Id);
                    if (colour != null)
                    {
                        productToEdit.Colours.Add(colour);
                    }
                }

                // Actualizar tamaños
                productToEdit.Sizes.Clear();
                foreach (var sizeId in productToEditDto.SizeId)
                {
                    var size = _context.Sizes.FirstOrDefault(s => s.Id == sizeId.Id);
                    if (size != null)
                    {
                        productToEdit.Sizes.Add(size);
                    }
                }

                _context.SaveChanges();
                return true;
            }
            return false;
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

        public void ChangeProductStatus(int id)
        {
            ChangeEntityStatus<Product>(id);
        }

        public void ChangeColourStatus(int id)
        {
            ChangeEntityStatus<Colour>(id);
        }

        public void ChangeSizeStatus(int id)
        {
            ChangeEntityStatus<Size>(id);
        }

        public void ChangeCategoryStatus(int id)
        {
            ChangeEntityStatus<Category>(id);
        }
    }
}
