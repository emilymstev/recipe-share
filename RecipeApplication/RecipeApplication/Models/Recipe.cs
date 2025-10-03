namespace RecipeApplication.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string RecipeName { get; set; }
        public string? Description { get; set; }
        public int Servings { get; set; }
        public string? Image { get; set; }
        public string? Categories { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public DateTime CreatedOn { get; set; }
        public string FolderName { get; set; }
    }
}
