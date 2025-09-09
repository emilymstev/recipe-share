using System.Data;
using System.Data.SqlClient;

namespace RecipeApplication.Models
{
    public class DAL
    {
        private readonly string _connectionString;

        public DAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBSC");
        }

        //Method to create a new product
        public int AddRecipe(Recipe recipe)
        {
            int recipeID = 0;

            using(SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                // Insert recipe
                using(SqlCommand cmdRecipe = new SqlCommand("sp_InsertProduct"))
                {
                    cmdRecipe.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdRecipe.Parameters.AddWithValue("@RecipeName", recipe.RecipeName);
                    cmdRecipe.Parameters.AddWithValue("@Description", recipe.Description);
                    cmdRecipe.Parameters.AddWithValue("@Servings", recipe.Servings);
                    cmdRecipe.Parameters.AddWithValue("@Image", recipe.Image);
                    cmdRecipe.Parameters.AddWithValue("@Categories", recipe.Categories);
                    cmdRecipe.Parameters.AddWithValue("@Ingredients", recipe.Ingredients);
                    cmdRecipe.Parameters.AddWithValue("@Instructions", recipe.Instructions);
                    cmdRecipe.Parameters.AddWithValue("@CreatedOn", recipe.CreatedOn);
                    cmdRecipe.Parameters.AddWithValue("@FolderName", recipe.FolderName);

                    object result = cmdRecipe.ExecuteScalar();
                    recipeID = result != null ? Convert.ToInt32(result) : 0;
                }

                // Insert features
                if (recipeID > 0 && recipe.Ingredients != null)
                {
                    foreach (var ingredient in recipe.Ingredients)
                    {
                        using (SqlCommand cmdIngredient = new SqlCommand("sp_InsertIngredient"))
                        {
                            cmdIngredient.CommandType = System.Data.CommandType.StoredProcedure;
                            cmdIngredient.Parameters.AddWithValue("@ProductId", productID);
                            cmdIngredient.Parameters.AddWithValue("@IngredientName", ingredient.IngredientName);
                            cmdIngredient.Parameters.AddWithValue("@Quantity", ingredient.Quantity);
                            cmdIngredient.Parameters.AddWithValue("Unit", ingredient.Unit);

                            object result = cmdIngredient.ExecuteScalar();
                            ingredient.Id = Convert.ToInt32(result);
                        }
                    }
                }

                // Insert categories
                if (recipeID > 0 && recipe.Categories != null)
                {
                    foreach (var category in recipe.Categories)
                    {
                        int categoryID;

                        using (SqlCommand cmdCategory = new SqlCommand("sp_InsertCategory"))
                        {
                            cmdCategory.CommandType = System.Data.CommandType.StoredProcedure;
                            cmdCategory.Parameters.AddWithValue("@Category", category);

                            object result = cmdCategory.ExecuteScalar();
                            categoryID = result != null ? Convert.ToInt32(result) : 0;
                        }

                        // Link product and tag
                        if (categoryID > 0)
                        {
                            using (SqlCommand cmdRecipeCategory = new SqlCommand("sp_InsertProductTag"))
                            {
                                cmdRecipeCategory.CommandType = CommandType.StoredProcedure;
                                cmdRecipeCategory.Parameters.AddWithValue("@RecipeId", recipeID);
                                cmdRecipeCategory.Parameters.AddWithValue("@TagId", categoryID);
                                cmdRecipeCategory.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            return recipeID;
        }
    }
}
