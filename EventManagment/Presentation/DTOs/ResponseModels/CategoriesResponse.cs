using Application.DTOs;

namespace Presentation.DTOs.ResponseModels
{
    public sealed class CategoriesResponse
    {
        public IReadOnlyList<CategoryDto> Categories { get; init; } = [];
    }
}
