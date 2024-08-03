using ADOCRUD8_0.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using ADOCRUD8_0.Helpers;
namespace ADOCRUD8_0.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly SqlHelper _sqlHelper;

        public ProductController(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }



        [Route("GetAllProduct")]
        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {


            try
            {
                SqlConnection con = _sqlHelper.GetConnection();
                if (con == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error while creating SQL connection.");
                }

                List<ProductModel> productModels = new List<ProductModel>();
                DataTable dt = new DataTable();
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("Select * from TBLProduct", con);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }



                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductModel productModel = new ProductModel();
                    productModel.ProductID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    productModel.ProductName = dt.Rows[i]["PName"].ToString(); // Assuming ProductName is a string
                    productModel.ProductPrice = Convert.ToDecimal(dt.Rows[i]["PPrice"]);
                    productModel.EntryDate = Convert.ToDateTime(dt.Rows[i]["PEntryDate"]); // Assuming ProductEntryDate is a DateTime
                    productModel.ProductDescription = dt.Rows[i]["PDescription"].ToString();
                    productModel.ProdtModel = dt.Rows[i]["PModel"].ToString();
                    productModels.Add(productModel);
                }

                return Ok(productModels);
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message;
                return BadRequest(exceptionMessage);
            }
        }





        [Route("PostProduct")]
        [HttpPost]
        public async Task<IActionResult> PostProduct(ProductModel obj)
        {
            try
            {
                SqlConnection con = _sqlHelper.GetConnection();
                if (con == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error while creating SQL connection.");
                }

                using (con)
                {
                    string query = "INSERT INTO TBLProduct (PName, PPrice, PDescription, PModel, PEntryDate) VALUES (@PName, @PPrice, @PDescription, @PModel, GETDATE())";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@PName", obj.ProductName);
                        cmd.Parameters.AddWithValue("@PPrice", obj.ProductPrice);
                        cmd.Parameters.AddWithValue("@PDescription", obj.ProductDescription);
                        cmd.Parameters.AddWithValue("@PModel", obj.ProdtModel);

                        con.Open();
                        await cmd.ExecuteNonQueryAsync();
                        con.Close();
                    }
                }
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }





        [Route("UpdateProduct")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductModel obj)
        {
            try
            {
                SqlConnection con = _sqlHelper.GetConnection();
                if (con == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error while creating SQL connection.");
                }

                using (con)
                {
                    string query = "UPDATE TBLProduct SET PName=@PName, PPrice=@PPrice, PDescription=@PDescription, PModel=@PModel WHERE ID=@ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@PName", obj.ProductName);
                        cmd.Parameters.AddWithValue("@PPrice", obj.ProductPrice);
                        cmd.Parameters.AddWithValue("@PDescription", obj.ProductDescription);
                        cmd.Parameters.AddWithValue("@PModel", obj.ProdtModel);
                        cmd.Parameters.AddWithValue("@ID", obj.ProductID);

                        con.Open();
                        await cmd.ExecuteNonQueryAsync();
                        con.Close();
                    }
                }
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }






        [Route("DeleteProduct/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                SqlConnection con = _sqlHelper.GetConnection();
                if (con == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error while creating SQL connection.");
                }

                using (con)
                {
                    string query = "DELETE FROM TBLProduct WHERE ID=@ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        con.Open();
                        await cmd.ExecuteNonQueryAsync();
                        con.Close();
                    }
                }
                return Ok(new { message = "Product deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }

}

