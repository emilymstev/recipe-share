using System.Data;
using System.Data.SqlClient;

namespace RecipeApplication.Models
{
    public class DAL
    {
        private readonly string _connectionString;

        public DAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBCS");
        }

        // Method to create a new recipe
        public int InsertRecipe(Recipe recipe)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using(SqlCommand cmd = new SqlCommand("sp_InsertRecipe", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RecipeName", recipe.RecipeName);
                    cmd.Parameters.AddWithValue("@Description", recipe.Description);
                    cmd.Parameters.AddWithValue("@Servings", recipe.Servings);
                    cmd.Parameters.AddWithValue("@Image", recipe.Image);
                    cmd.Parameters.AddWithValue("@Categories", recipe.Categories);
                    cmd.Parameters.AddWithValue("@Ingredients", recipe.Ingredients);
                    cmd.Parameters.AddWithValue("@Instructions", recipe.Instructions);
                    cmd.Parameters.AddWithValue("@CreatedOn", recipe.CreatedOn);
                    cmd.Parameters.AddWithValue("@FolderName", recipe.FolderName);

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        // Method to get a recipe by Id
        public Recipe GetRecipe(int id)
        {
            Recipe recipe = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetRecipeById", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            recipe = new Recipe
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                RecipeName = Convert.ToString(reader["RecipeName"]),
                                Description = Convert.ToString(reader["Description"]),
                                Servings = Convert.ToInt32(reader["Servings"]),
                                Image = Convert.ToString(reader["Image"]),
                                Categories = Convert.ToString(reader["Categories"]),
                                Ingredients = Convert.ToString(reader["Ingredients"]),
                                Instructions = Convert.ToString(reader["Instructions"]),
                                CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                                FolderName = Convert.ToString(reader["FolderName"])
                            };
                        }
                    }
                }
            }
            return recipe;
        }

        // Method to get all recipes
        public List<Recipe> GetRecipes()
        {
            List<Recipe> recipeList = new List<Recipe>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllRecipes", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Recipe recipe = new Recipe
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                RecipeName = Convert.ToString(reader["RecipeName"]),
                                Description = Convert.ToString(reader["Description"]),
                                Servings = Convert.ToInt32(reader["Servings"]),
                                Image = Convert.ToString(reader["Image"]),
                                Categories = Convert.ToString(reader["Categories"]),
                                Ingredients = Convert.ToString(reader["Ingredients"]),
                                Instructions = Convert.ToString(reader["Instructions"]),
                                CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                                FolderName = Convert.ToString(reader["FolderName"])
                            };
                            recipeList.Add(recipe);
                        }
                    }
                }
            }
            return recipeList;
        }

        // Method to update a recipe

        public bool UpdateRecipe(Recipe recipe)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateRecipe", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", recipe.Id);
                    cmd.Parameters.AddWithValue("@RecipeName", recipe.RecipeName);
                    cmd.Parameters.AddWithValue("@Description", recipe.Description);
                    cmd.Parameters.AddWithValue("@Servings", recipe.Servings);
                    cmd.Parameters.AddWithValue("@Image", recipe.Image);
                    cmd.Parameters.AddWithValue("@Categories", recipe.Categories);
                    cmd.Parameters.AddWithValue("@Ingredients", recipe.Ingredients);
                    cmd.Parameters.AddWithValue("@Instructions", recipe.Instructions);
                    cmd.Parameters.AddWithValue("@CreatedOn", recipe.CreatedOn);
                    cmd.Parameters.AddWithValue("@FolderName", recipe.FolderName);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    return i > 0;
                }
            }
        }

        // Method to delete a recioe
        public bool DeleteRecipe(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteRecipe", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    return i > 0;
                }
            }
        }
    }
}
