using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CategoriesResult
    {
        public List<CategoryDto> Categories { get; init; } = [];
    }

    public sealed class CategoryDto
    {
        public int Id { get; init; } = default;
        public string Name { get; init; } = default!;
        public int? Count { get; init; }     // null / omitted when withCounts=false
    }
}
