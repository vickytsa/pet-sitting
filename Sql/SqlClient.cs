using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VickyTsao.PetCare.Objects;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;

namespace VickyTsao.PetCare.Sql
{
    public class SqlClient
    { 
        private string _connectionString = null;

        public SqlClient(string connectionString)
        {
            _connectionString = connectionString;
        }


        #region PetSitter

        public IEnumerable<PetSitter> GetAllPetSitters()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT ps.SitterId, FirstName, LastName, Address1, Address2, City, State, Zip, Phone, Email, 
                                           pso.Rate, pso.PetCategoryId, pso.PetSizeId, pso.PetCareOptionId, 
                                           sr.Rating
                                    FROM PetSitter ps
                                    JOIN PetSitterOption pso ON ps.SitterId = pso.SitterId 
                                    LEFT JOIN vwSitterRating sr ON sr.SitterId = ps.SitterId
                                    GROUP BY ps.SitterId, FirstName, LastName, Address1, Address2, City, State, Zip, Phone, Email, 
                                           pso.Rate, pso.PetCategoryId, pso.PetSizeId, pso.PetCareOptionId, sr.Rating";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                conn.Open();

                // Data is accessible through the DataReader object here

                var sitters = new List<PetSitter>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var sitter = new PetSitter();
                        var option = new PetSitterOption();
                        sitter.SitterId = reader.GetInt32(0);
                        sitter.FirstName = reader.GetString(1);
                        sitter.LastName = reader.GetString(2);
                        sitter.Address1 = reader.GetString(3);
                        sitter.Address2 = reader.SafeGetValue<string>(4);
                        sitter.City = reader.GetString(5);
                        sitter.State = reader.GetString(6);
                        sitter.Zip = reader.GetString(7);
                        sitter.Phone = reader.GetString(8);
                        sitter.Email = reader.GetString(9);
                        option.Rate = reader.GetDecimal(10);
                        option.PetCategoryId = reader.GetInt32(11);
                        option.PetSizeId = reader.GetString(12);
                        option.PetCareOptionId = reader.GetInt32(13);
                        sitter.Rating = reader.SafeGetValue<decimal>(14);

                        sitter.PetSitterOptions = new List<PetSitterOption>() { option };
                        sitters.Add(sitter);

                    }
                }

                var results = new List<PetSitter>();
                foreach(var sitter in sitters.GroupBy(s=>s.SitterId))
                {
                    var newSitter = new PetSitter()
                    {
                        SitterId = sitter.First().SitterId,
                        FirstName = sitter.First().FirstName,
                        LastName = sitter.First().LastName,
                        Address1 = sitter.First().Address1,
                        Address2 = sitter.First().Address2,
                        City = sitter.First().City,
                        State = sitter.First().State,
                        Zip = sitter.First().Zip,
                        Phone = sitter.First().Phone,
                        Email = sitter.First().Email,
                        Rating = sitter.First().Rating,
                        PetSitterOptions = new List<PetSitterOption>()
                    };

                    foreach(var option in sitter)
                    {

                        ((List<PetSitterOption>)newSitter.PetSitterOptions).Add(new PetSitterOption()
                        {
                            PetCareOptionId = option.PetSitterOptions.First().PetCareOptionId,
                            PetCategoryId = option.PetSitterOptions.First().PetCategoryId,
                            PetSizeId = option.PetSitterOptions.First().PetSizeId,
                            Rate = option.PetSitterOptions.First().Rate,
                            SitterId = option.SitterId,
                        });

                    };
                    results.Add(newSitter);
                }
                return results;
            }

        }

        public PetSitter GetPetSitterById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT ps.SitterId, FirstName, LastName, Address1, Address2, City, State, Zip, Phone, Email, 
                                           pso.Rate, pso.PetCategoryId, pso.PetSizeId, pso.PetCareOptionId, sr.Rating
                                    FROM PetSitter ps
                                    LEFT JOIN PetSitterOption pso ON ps.SitterId = pso.SitterId 
                                    LEFT JOIN vwSitterRating sr ON sr.SitterId = ps.SitterId
                                    WHERE ps.SitterId = @SitterId
                                    GROUP BY ps.SitterId, FirstName, LastName, Address1, Address2, City, State, Zip, Phone, Email, 
                                           pso.Rate, pso.PetCategoryId, pso.PetSizeId, pso.PetCareOptionId, sr.Rating"; 
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.Add(new SqlParameter("@SitterId", id));

                conn.Open();

                // Data is accessible through the DataReader object here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var sitters = new List<PetSitter>();
                    while (reader.Read())
                    {
                        var sitter = new PetSitter();
                        var option = new PetSitterOption();
                        sitter.SitterId = reader.GetInt32(0);
                        sitter.FirstName = reader.GetString(1);
                        sitter.LastName = reader.GetString(2);
                        sitter.Address1 = reader.GetString(3);
                        sitter.Address2 = reader.SafeGetValue<string>(4);
                        sitter.City = reader.GetString(5);
                        sitter.State = reader.GetString(6);
                        sitter.Zip = reader.GetString(7);
                        sitter.Phone = reader.GetString(8);
                        sitter.Email = reader.GetString(9);               
                        option.Rate = reader.GetDecimal(10);
                        option.PetCategoryId = reader.GetInt32(11);
                        option.PetSizeId = reader.GetString(12);
                        option.PetCareOptionId = reader.GetInt32(13);
                        sitter.Rating = reader.SafeGetValue<decimal>(14);

                        sitter.PetSitterOptions = new List<PetSitterOption> { option };
                        sitters.Add(sitter);
                    }

                    if(sitters?.Count() > 0)
                    {
                        var newSitter = new PetSitter()

                        {
                            SitterId = sitters.First().SitterId,
                            FirstName = sitters.First().FirstName,
                            LastName = sitters.First().LastName,
                            City = sitters.First().City,
                            State = sitters.First().State,
                            Zip = sitters.First().Zip,
                            Phone = sitters.First().Phone,
                            Email = sitters.First().Email,
                            Address1 = sitters.First().Address1,
                            Address2 = sitters.First().Address2,
                            Rating = sitters.First().Rating,
                            PetSitterOptions = new List<PetSitterOption>()
                        };
                        foreach (var option in sitters)
                        {
                            ((List<PetSitterOption>)newSitter.PetSitterOptions).Add(new PetSitterOption()
                            {
                                PetCareOptionId = option.PetSitterOptions.First().PetCareOptionId,
                                PetCategoryId = option.PetSitterOptions.First().PetCategoryId,
                                PetSizeId = option.PetSitterOptions.First().PetSizeId,
                                Rate = option.PetSitterOptions.First().Rate,
                                SitterId = option.SitterId,
                            });
                        };
                        return newSitter;
                    }
                    else
                    {
                        return null;
                    }                 
                }
            }         
        }

        public IEnumerable<PetSitter> FindPetSitters(PetSitterRequest request)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = @"SELECT SitterId, FirstName, LastName, Address1, Address2, City, State, Zip, Phone, Email, 
                //                Rate, PetCategoryId, PetSizeId, PetCareOptionId, Rating
                //FROM (

                //SELECT ps.SitterId, FirstName, LastName, Address1, Address2, City, State, Zip, Phone, Email, 
                //       pso.Rate, pso.PetCategoryId, pso.PetSizeId, pso.PetCareOptionId, 
                //       AVG(CAST( CASE WHEN psr.rating IS NULL THEN 0 ELSE psr.rating end as decimal)) AS Rating
                //FROM PetSitter ps
                //INNER JOIN PetSitterOption pso ON ps.SitterId = pso.SitterId 
                //LEFT JOIN PetSitterReview psr ON ps.sitterid = psr.sitterid
                //GROUP BY ps.SitterId, FirstName, LastName, Address1, Address2, City, State, Zip, Phone, Email, 
                //         pso.Rate, pso.PetCategoryId, pso.PetSizeId, pso.PetCareOptionId) t
                //WHERE";


                cmd.CommandText = @"SELECT ps.SitterId, FirstName, LastName, Address1, Address2, City, State, Zip, Phone, Email, 
                                    pso.Rate, pso.PetCategoryId, pso.PetSizeId, pso.PetCareOptionId, 
                                    sr.Rating
                                    FROM PetSitter ps
                                    INNER JOIN PetSitterOption pso ON ps.SitterId = pso.SitterId
                                    LEFT JOIN vwSitterRating sr ON ps.sitterId = sr.SitterId
                                    LEFT JOIN PetSitterBlockDate bd ON bd.SitterId = ps.SitterId
                                    WHERE";


                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;


                cmd.CommandText += " pso.PetSizeId = @PetSizeId and";
                cmd.Parameters.Add(new SqlParameter("@PetSizeId", request.PetSizeId));

                cmd.CommandText += " pso.PetCategoryId = @PetCategoryId and";
                cmd.Parameters.Add(new SqlParameter("@PetCategoryId", request.PetCategoryId));

                cmd.CommandText += " pso.PetCareOptionId = @PetCareOptionId and";
                cmd.Parameters.Add(new SqlParameter("@PetCareOptionId", request.PetCareOptionId));


                if (request.City != null)
                {
                    cmd.CommandText += " City = @City and";
                    cmd.Parameters.Add(new SqlParameter("@City", request.City));
                }

                if (request.State != null)
                {
                    cmd.CommandText += " State = @State and";
                    cmd.Parameters.Add(new SqlParameter("@State", request.State));
                }

                if (request.Zip != null)
                {
                    cmd.CommandText += " Zip = @Zip and";
                    cmd.Parameters.Add(new SqlParameter("@Zip", request.Zip));
                }

                if (request.DropOffDate != null && request.PickUpDate != null)
                {
                    cmd.CommandText += " (BlockDate NOT BETWEEN @DropOffDate AND @PickUpDate OR BlockDate IS NULL) and";
                    cmd.Parameters.Add(new SqlParameter("@DropOffDate", request.DropOffDate));
                    cmd.Parameters.Add(new SqlParameter("@PickUpDate", request.PickUpDate));
                }

                if (request.MinRate != null)
                {
                    cmd.CommandText += " Rate >= @MinRate and";
                    cmd.Parameters.Add(new SqlParameter("@MinRate", request.MinRate));
                }

                if (request.MaxRate != null)
                {
                    cmd.CommandText += " Rate <= @MaxRate and";
                    cmd.Parameters.Add(new SqlParameter("@MaxRate", request.MaxRate));
                }

                if (request.MinRating != null)
                {
                    cmd.CommandText += " sr.Rating >= @MinRating and";
                    cmd.Parameters.Add(new SqlParameter("@MinRating", request.MinRating));
                }


                cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 4);

                cmd.CommandText += @" GROUP BY ps.SitterId, FirstName, LastName, Address1, Address2, City, State, Zip, Phone, Email, 
                                           pso.Rate, pso.PetCategoryId, pso.PetSizeId, pso.PetCareOptionId, sr.Rating";

                conn.Open();
                var sitters = new List<PetSitter>();

                // Data is accessible through the DataReader object here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var sitter = new PetSitter();
                        var option = new PetSitterOption();
                        sitter.SitterId = reader.GetInt32(0);
                        sitter.FirstName = reader.GetString(1);
                        sitter.LastName = reader.GetString(2);
                        sitter.Address1 = reader.GetString(3);
                        sitter.Address2 = reader.SafeGetValue<string>(4);
                        sitter.City = reader.GetString(5);
                        sitter.State = reader.GetString(6);
                        sitter.Zip = reader.GetString(7);
                        sitter.Phone = reader.GetString(8);
                        sitter.Email = reader.GetString(9);                  
                        option.Rate = reader.GetDecimal(10);
                        option.PetCategoryId = reader.GetInt32(11);
                        option.PetSizeId = reader.GetString(12);
                        option.PetCareOptionId = reader.GetInt32(13);
                        sitter.Rating = reader.SafeGetValue<decimal>(14);

                        sitter.PetSitterOptions = new List<PetSitterOption>() { option };
                        sitters.Add(sitter);
                    }
                }

                var results = new List<PetSitter>();

                foreach (var sitter in sitters.GroupBy(s => s.SitterId))
                {
                    var newSitter = new PetSitter()
                    {
                        SitterId = sitter.First().SitterId,
                        FirstName = sitter.First().FirstName,
                        LastName = sitter.First().LastName,
                        City = sitter.First().City,
                        State = sitter.First().State,
                        Zip = sitter.First().Zip,
                        Phone = sitter.First().Phone,
                        Email = sitter.First().Email,
                        Address1 = sitter.First().Address1,
                        Address2 = sitter.First().Address2,
                        Rating = sitter.First().Rating,
                        PetSitterOptions = new List<PetSitterOption>()
                    };

                    foreach (var option in sitter)
	                {
                        ((List<PetSitterOption>)newSitter.PetSitterOptions).Add(new PetSitterOption()
                        {
                            PetCareOptionId = option.PetSitterOptions.First().PetCareOptionId,
                            PetCategoryId = option.PetSitterOptions.First().PetCategoryId,
                            PetSizeId = option.PetSitterOptions.First().PetSizeId,
                            Rate = option.PetSitterOptions.First().Rate,
                            SitterId = option.SitterId,
                        });
	                }
                    results.Add(newSitter);
                }

                //var ints = new [] { 1, 2, 3, 4, 5 };

                //var strs = ints.Select(x => x += 1);


                return results;
            }
        }

        public PetSitter AddPetSitter(PetSitter sitter)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO PetSitter (FirstName, LastName, Address1, Address2, City, State, Zip, Phone, Email)
                                    OUTPUT INSERTED.SitterId
                                    VALUES(@FirstName, @LastName, @Address1, @Address2, @City, @State, @Zip, @Phone, @Email)";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.Add(new SqlParameter("@FirstName", sitter.FirstName));
                cmd.Parameters.Add(new SqlParameter("@LastName", sitter.LastName));
                cmd.Parameters.Add(new SqlParameter("@Address1", sitter.Address1));
                cmd.Parameters.Add(new SqlParameter("@Address2", sitter.Address2 == null ? "" : sitter.Address2));
                cmd.Parameters.Add(new SqlParameter("@City", sitter.City));
                cmd.Parameters.Add(new SqlParameter("@State", sitter.State));
                cmd.Parameters.Add(new SqlParameter("@Zip", sitter.Zip));
                cmd.Parameters.Add(new SqlParameter("@Phone", sitter.Phone));
                cmd.Parameters.Add(new SqlParameter("@Email", sitter.Email));



                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sitter.SitterId = reader.GetInt32(0);
                    }
                }
                return sitter;
            }
        }

        public void DeletePetSitter(int id)
        {
            using (var sqlConn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = @"DELETE FROM PetSitter
                                    WHERE SitterId = @PetSitterId";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConn;

                cmd.Parameters.Add(new SqlParameter("@PetSitterId", id));

                sqlConn.Open();
                //command code here
                cmd.ExecuteNonQuery();

            }
        }


        #endregion

        #region PetSitterBlockDate
        public PetSitterBlockDate AddPetSitterBlockDate(PetSitterBlockDate date)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO PetSitterBlockDate ( SitterId, BlockDate)
                                    VALUES (@SitterId, @BlockDate)";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@SitterId", date.SitterId));
                cmd.Parameters.Add(new SqlParameter("@BlockDate", date.BlockDate));

                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        date.SitterId = reader.GetInt32(0);
                        date.BlockDate = reader.GetDateTime(1);
                    }
                }
                return date;
            }
        }
        #endregion

        #region PetSitterOption

        public PetSitterOption AddNewPetSitterOption(PetSitterOption option)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO PetSitterOption(SitterId, PetCategoryId, PetSizeId, PetCareOptionId, Rate)
                                    VALUES(@SitterId, @PetCategoryId, @PetSizeId, @PetCareOptionId, @Rate)";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@SitterId", option.SitterId));
                cmd.Parameters.Add(new SqlParameter(@"PetCategoryId", option.PetCategoryId));
                cmd.Parameters.Add(new SqlParameter(@"PetSizeId", option.PetSizeId));
                cmd.Parameters.Add(new SqlParameter(@"PetCareOptionId", option.PetCareOptionId));
                cmd.Parameters.Add(new SqlParameter(@"Rate", option.Rate));

                cmd.Connection = conn;
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                return option;
            }
        }

        public void DeletePetSitterOption(PetSitterOption option)
        {
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM PetSitterOption
                                    WHERE SitterId = @SitterId AND PetCategoryId = @PetCategoryId
                                    AND PetSizeId=@PetSizeId AND PetCareOptionId = @PetCareOptionId";

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@SitterId", option.SitterId));
                cmd.Parameters.Add(new SqlParameter("@PetCategoryId", option.PetCategoryId));
                cmd.Parameters.Add(new SqlParameter("@PetSizeId", option.PetSizeId));
                cmd.Parameters.Add(new SqlParameter("@PetCareOptionId", option.PetCareOptionId));

                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region PetSitterReview
        public IEnumerable<PetSitterReview> FindPetSitterReviewsBySitterId(int id)
        {
            using (var sqlconn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT ReviewId, SitterId, PetOwnerId, Rating, Comment
                                    FROM PetSitterReview
                                    WHERE SitterId = @SitterId";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@SitterId", id));
                cmd.Connection = sqlconn;

                sqlconn.Open();

                var reviews = new List<PetSitterReview>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var review = new PetSitterReview()
                        {
                            ReviewId = reader.GetInt32(0),
                            SitterId = reader.GetInt32(1),
                            PetOwnerId = reader.GetInt32(2),
                            Rating = reader.GetInt32(3),
                            Comment = reader.SafeGetValue<string>(4)

                        };
                        reviews.Add(review);
                    }
                }
                return reviews;
            }
        }

        public PetSitterReview AddNewPetSitterReview(PetSitterReview review)
        {
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO PetSitterReview(SitterId, PetOwnerId, Rating, Comment)
                                    OUTPUT INSERTED.ReviewId
                                    VALUES( @SitterId, @PetOwnerId, @Rating, @Comment)";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@SitterId", review.SitterId));
                cmd.Parameters.Add(new SqlParameter("@PetOwnerId", review.PetOwnerId));
                cmd.Parameters.Add(new SqlParameter("@Rating", review.Rating));
                cmd.Parameters.Add(new SqlParameter("@Comment", review.Comment == null? "": review.Comment));

                cmd.Connection = conn;

                conn.Open();

                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        review.ReviewId = reader.GetInt32(0);
                    }
                }

            }
            return review;
        }
        #endregion


        #region PetOwner
        public IEnumerable<PetOwner> GetAllPetOwners()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT po.PetOwnerId, FirstName, LastName, Address1, Address2, City, State, Zip, Phone, Email,
                                           PetId, p.PetOwnerId, PetName, Bod, GenderId, PetCategoryId, PetSizeId, Breed
                                    FROM PetOwner po
                                    LEFT JOIN Pet p ON po.PetOwnerId = p.PetOwnerId";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                conn.Open();

                // Data is accessible through the DataReader object here

                var owners = new List<PetOwner>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var owner = new PetOwner();
                        var pet = new Pet();
                        owner.PetOwnerId = reader.GetInt32(0);
                        owner.FirstName = reader.GetString(1);
                        owner.LastName = reader.GetString(2);
                        owner.Address1 = reader.GetString(3);
                        owner.Address2 = reader.SafeGetValue<string>(4);
                        owner.City = reader.GetString(5);
                        owner.State = reader.GetString(6);
                        owner.Zip = reader.GetString(7);
                        owner.Phone = reader.GetString(8);
                        owner.Email = reader.GetString(9);
                        pet.PetId = reader.SafeGetValue<int>(10);
                        pet.PetOwnerId = reader.SafeGetValue<int>(11);
                        pet.PetName = reader.SafeGetValue<string>(12);
                        pet.Bod = reader.SafeGetValue<DateTime>(13);
                        pet.GenderId = reader.SafeGetValue<string>(14);
                        pet.PetCategoryId = reader.SafeGetValue<int>(15);
                        pet.PetSizeId = reader.SafeGetValue<string>(16);
                        pet.Breed = reader.SafeGetValue<string>(17);

                        owner.Pets = new List<Pet> { pet };
                        owners.Add(owner);
                    }

                    var results = new List<PetOwner>();
                    foreach (var owner in owners.GroupBy(s => s.PetOwnerId))
                    {
                        var newOwner = new PetOwner()
                        {
                            PetOwnerId = owner.First().PetOwnerId,
                            FirstName = owner.First().FirstName,
                            LastName = owner.First().LastName,
                            Address1 = owner.First().Address1,
                            Address2 = owner.First().Address2,
                            City = owner.First().City,
                            State = owner.First().State,
                            Zip = owner.First().Zip,
                            Phone = owner.First().Phone,
                            Email = owner.First().Email,
                            Pets = new List<Pet>()
                        };

                        foreach (var pet in owner)
                        {
                            ((List<Pet>)newOwner.Pets).Add(new Pet()
                            {
                                PetId = pet.Pets.First().PetId,
                                PetOwnerId = pet.Pets.First().PetOwnerId,
                                PetName = pet.Pets.First().PetName,
                                Bod = pet.Pets.First().Bod,
                                GenderId = pet.Pets.First().GenderId,
                                PetCategoryId = pet.Pets.First().PetCategoryId,
                                PetSizeId = pet.Pets.First().PetSizeId,
                                Breed = pet.Pets.First().Breed
                            });
                        };
                        results.Add(newOwner);
                    }
                    return results;
                }    
            }
        }

        public PetOwner GetPetOwnerById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT po.PetOwnerId, FirstName, LastName, Address1, Address2, City, State, Zip, Phone, Email,
                                           PetId, p.PetOwnerId, PetName, Bod, GenderId, PetCategoryId, PetSizeId, Breed
                                    FROM PetOwner po
                                    LEFT JOIN Pet p ON po.PetOwnerId = p.PetOwnerId
                                    WHERE po.PetOwnerId = @PetOwnerId";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.Add(new SqlParameter("@PetOwnerId", id));

                conn.Open();

                // Data is accessible from database through the DataReader object to here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var owners = new List<PetOwner>();
                    while (reader.Read())
                    {
                        var owner = new PetOwner();
                        var pet = new Pet();
                        owner.PetOwnerId = reader.GetInt32(0);
                        owner.FirstName = reader.GetString(1);
                        owner.LastName = reader.GetString(2);
                        owner.Address1 = reader.GetString(3);
                        owner.Address2 = reader.SafeGetValue<string>(4);
                        owner.City = reader.GetString(5);
                        owner.State = reader.GetString(6);
                        owner.Zip = reader.GetString(7);
                        owner.Phone = reader.GetString(8);
                        owner.Email = reader.GetString(9);
                        pet.PetId = reader.SafeGetValue<int>(10);
                        pet.PetOwnerId = reader.SafeGetValue<int>(11);
                        pet.PetName = reader.SafeGetValue<string>(12);
                        pet.Bod = reader.SafeGetValue<DateTime>(13);
                        pet.GenderId = reader.SafeGetValue<string>(14);
                        pet.PetCategoryId = reader.SafeGetValue<int>(15);
                        pet.PetSizeId = reader.SafeGetValue<string>(16);
                        pet.Breed = reader.SafeGetValue<string>(17);

                        owner.Pets = new List<Pet> { pet };
                        owners.Add(owner);
                    }

                    if (owners.Count() > 0)
                    {
                        var newOwner = new PetOwner()
                        {
                            PetOwnerId = owners.First().PetOwnerId,
                            FirstName = owners.First().FirstName,
                            LastName = owners.First().LastName,
                            Address1 = owners.First().Address1,
                            Address2 = owners.First().Address2,
                            City = owners.First().City,
                            State = owners.First().State,
                            Zip = owners.First().Zip,
                            Phone = owners.First().Phone,
                            Email = owners.First().Email,
                            Pets = new List<Pet>()
                        };

                                            
                            foreach (var pet in owners)
                            {
                                ((List<Pet>)newOwner.Pets).Add(new Pet()
                                {
                                    PetId = pet.Pets.First().PetId,
                                    PetOwnerId = pet.Pets.First().PetOwnerId,
                                    PetName = pet.Pets.First().PetName,
                                    Bod = pet.Pets.First().Bod,
                                    GenderId = pet.Pets.First().GenderId,
                                    PetCategoryId = pet.Pets.First().PetCategoryId,
                                    PetSizeId = pet.Pets.First().PetSizeId,
                                    Breed = pet.Pets.First().Breed
                                });
                            }
                        
                        return newOwner;
                    }
                    else{
                       return null;
                    }
                }  
            }
        }

        public PetOwner AddPetOwner(PetOwner owner)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO PetOwner (FirstName, LastName, Address1, Address2, City, State, Zip, Phone, Email)
                                    OUTPUT INSERTED.PetOwnerId
                                    VALUES(@FirstName, @LastName, @Address1, @Address2, @City, @State, @Zip, @Phone, @Email)";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.Add(new SqlParameter("@FirstName", owner.FirstName));
                cmd.Parameters.Add(new SqlParameter("@LastName", owner.LastName));
                cmd.Parameters.Add(new SqlParameter("@Address1", owner.Address1));
                cmd.Parameters.Add(new SqlParameter("@Address2", owner.Address2 == null ? "" : owner.Address2));
                cmd.Parameters.Add(new SqlParameter("@City", owner.City));
                cmd.Parameters.Add(new SqlParameter("@State", owner.State));
                cmd.Parameters.Add(new SqlParameter("@Zip", owner.Zip));
                cmd.Parameters.Add(new SqlParameter("@Phone", owner.Phone));
                cmd.Parameters.Add(new SqlParameter("@Email", owner.Email));


                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        owner.PetOwnerId = reader.GetInt32(0);
                    }
                }


                
                //foreach (var pet in owner.Pets)
                //{
                //    SqlCommand cmd2 = new SqlCommand();
                //    cmd2.CommandText = @"INSERT INTO Pet(PetName, Bod, GenderId, PetCategoryId, PetSizeId, Breed, PetOwnerId)
                //                            OUTPUT INSERTED.PetId
                //                            VALUES(@PetName, @Bod, @GenderId, @PetCategoryId, @PetSizeId, @Breed, @PetOwnerId)";

                //    cmd2.Parameters.Add(new SqlParameter("@PetName", pet.PetName));
                //    cmd2.Parameters.Add(new SqlParameter("@Bod", pet.Bod));
                //    cmd2.Parameters.Add(new SqlParameter("@GenderId", pet.GenderId));
                //    cmd2.Parameters.Add(new SqlParameter("@PetCategoryId", pet.PetCategoryId));
                //    cmd2.Parameters.Add(new SqlParameter("@PetSizeId", pet.PetSizeId));
                //    cmd2.Parameters.Add(new SqlParameter("@Breed", pet.Breed));
                //    cmd2.Parameters.Add(new SqlParameter("@PetOwnerId", owner.PetOwnerId));


                //    cmd2.Connection = conn;

                //    using (SqlDataReader reader2 = cmd2.ExecuteReader())
                //    {
                //        while (reader2.Read())
                //        {
                //            pet.PetId = reader2.GetInt32(0);
                //            pet.PetOwnerId = owner.PetOwnerId;

                //        }
                //    }
                }
                return owner;
            }

        public void DeletePetOwner(int id)
        {
            using(var sqlConn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = @"DELETE FROM PetOwner
                                    WHERE PetOwnerId = @PetOwnerId";
                cmd.Connection = sqlConn;

                cmd.Parameters.Add(new SqlParameter("@PetOwnerId", id));

                sqlConn.Open();

                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Pet
        public Pet AddNewPet(Pet pet)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Pet (PetOwnerId, PetName, Bod, GenderId, PetCategoryId, PetSizeId, Breed)
                                    OUTPUT INSERTED.PetId
                                    VALUES(@PetOwnerId, @PetName, @Bod, @GenderId, @PetCategoryId, @PetSizeId, @Breed)";

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@PetOwnerId", pet.PetOwnerId));
                cmd.Parameters.Add(new SqlParameter("@PetName", pet.PetName));
                cmd.Parameters.Add(new SqlParameter("@Bod", pet.Bod));
                cmd.Parameters.Add(new SqlParameter("@GenderId", pet.GenderId));
                cmd.Parameters.Add(new SqlParameter("@PetCategoryId", pet.PetCategoryId));
                cmd.Parameters.Add(new SqlParameter("@PetSizeId", pet.PetSizeId));
                cmd.Parameters.Add(new SqlParameter("@Breed", pet.Breed));

                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pet.PetId = reader.GetInt32(0);
                    }
                }
                return pet;
            }
        }

        public void DeletePet(int id)
        {
            using (var sqlConn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Pet
                                    WHERE PetId = @PetId";
                cmd.CommandType = CommandType.Text;

                cmd.Connection = sqlConn;
                cmd.Parameters.Add(new SqlParameter("@PetId", id));

                sqlConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region PetCategory/PetSize/PetCareOption
        public IEnumerable<PetCategory> GetAllPetCategoryOptions()
        {
            using(var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT PetCategoryId, PetCategoryName
                                    FROM PetCategory";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                conn.Open();
                var options = new List<PetCategory>();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var category = new PetCategory();
                        category.PetCategoryId = reader.GetInt32(0);
                        category.PetCategoryName = reader.GetString(1);
                        options.Add(category);
                    }
                    return options;
                }
            }
        }

        public IEnumerable<PetSize> GetAllPetSizeOptions()
        {
            using(var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = @"SELECT PetSizeId, PetSizeName
                                    FROM PetSize
                                    ORDER BY OrderByNumber";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                conn.Open();
                var options = new List<PetSize>();
                using( SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var option = new PetSize();
                        option.PetSizeId = reader.GetString(0);
                        option.PetSizeName = reader.GetString(1);
                        options.Add(option);
                    }
                    return options;
                }
            }
        }

        public IEnumerable<PetCareOption> GetAllPetCareOptionOptions()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = @"SELECT PetCareOptionId, PetCareOptionName
                                    FROM PetCareOption";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                conn.Open();
                var options = new List<PetCareOption>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var option = new PetCareOption();
                        option.PetCareOptionId = reader.GetInt32(0);
                        option.PetCareOptionName = reader.GetString(1);
                        options.Add(option);
                    }
                    return options;
                }
            }

        }

        #endregion


        #region Reservation
        public IEnumerable<Reservation> GetAllReservations()
        {
            using(var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT r.ReservationId, OrderDate, DropOffDate, PickUpDate, r.SitterId, 
                                    ps.FirstName + ' ' + ps.LastName AS PetSitterName,
                                     r.PetOwnerId, po.FirstName + ' ' + po.LastName AS PetOwnerName, r.PetCareOptionId, 
                                    pr.ReservationId, p.PetId, p.PetName, pco.PetCareOptionName, pso.Rate, 
                                    pso.Rate * (CAST(DATEDIFF(DAY, DropOffDate, PickUpDate) AS decimal)) AS SubTotal, Status

                                    FROM Reservation r
                                    LEFT JOIN PetReservation pr ON r.ReservationId = pr.ReservationId
                                    LEFT JOIN Pet p ON pr.PetId = p.PetId
                                    LEFT JOIN PetSitter ps On r.SitterId = ps.SitterId
                                    LEFT JOIN PetSitterOption pso ON r.SitterId = pso.SitterId  AND p.petsizeid = pso.petsizeid
                                    AND r.PetCareOptionId = pso.PetCareOptionId
                                    AND p.PetCategoryId = pso.PetCategoryId
                                    LEFT JOIN PetCareOption pco ON pco.PetCareOptionId = r.PetCareOptionId
                                    LEFT JOIN PetOwner po ON po.PetOwnerId = r.PetOwnerId
                                    GROUP BY r.ReservationId, OrderDate, DropOffDate, PickUpDate,
                                             r.SitterId, ps.FirstName, ps.LastName, 
                                             r.PetOwnerId,po.FirstName, po.LastName, r.PetCareOptionId,
                                             pr.ReservationId, p.PetId, p.PetName, r.PetCareOptionId, pco.PetCareOptionName, 
                                             pso.Rate, r.DropOffDate, r.PickUpDate, Status";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                conn.Open();

                // Data is accessible through the DataReader object here
                var reservations = new List<Reservation>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation();
                        var pet = new PetReservation();
                        reservation.ReservationId = reader.GetInt32(0);
                        reservation.OrderDate = reader.GetDateTime(1);
                        reservation.DropOffDate = reader.GetDateTime(2);
                        reservation.PickUpDate = reader.GetDateTime(3);
                        reservation.SitterId = reader.GetInt32(4);
                        reservation.PetSitterName = reader.GetString(5);
                        reservation.PetOwnerId = reader.GetInt32(6);
                        reservation.PetOwnerName = reader.GetString(7);
                        reservation.PetCareOptionId = reader.GetInt32(8);
                        pet.ReservationId = reader.SafeGetValue<int>(9);
                        pet.PetId = reader.SafeGetValue<int>(10);
                        pet.PetName = reader.SafeGetValue<string>(11);
                        pet.PetCareOptionName = reader.SafeGetValue<string>(12);
                        pet.Rate = reader.SafeGetValue<decimal>(13);
                        pet.SubTotal = reader.SafeGetValue<decimal>(14);
                        reservation.Status = (ReservationStatus)Enum.Parse(typeof(ReservationStatus), reader.SafeGetValue<string>(15));

                        reservation.PetReservations = new List<PetReservation> { pet };
                        reservations.Add(reservation);
                    }
                }

                    var results = new List<Reservation> ();
                    foreach(var reservation in reservations.GroupBy(r => r.ReservationId))
                    {
                        var newReservation = new Reservation()
                        {
                            ReservationId = reservation.First().ReservationId,
                            OrderDate = reservation.First().OrderDate,
                            DropOffDate = reservation.First().DropOffDate,
                            PickUpDate = reservation.First().PickUpDate,
                            SitterId = reservation.First().SitterId,
                            PetSitterName = reservation.First().PetSitterName,
                            PetCareOptionId = reservation.First().PetCareOptionId,
                            PetOwnerId = reservation.First().PetOwnerId,
                            PetOwnerName = reservation.First().PetOwnerName,
                            PetReservations = new List<PetReservation>(),
                            Status = reservation.First().Status
                        };

                        foreach(var pet in reservation)
                        {
                            ((List<PetReservation>)newReservation.PetReservations).Add(new PetReservation()
                            {
                                ReservationId = pet.PetReservations.First().ReservationId,
                                PetCareOptionName = pet.PetReservations.First().PetCareOptionName,
                                PetId = pet.PetReservations.First().PetId,
                                PetName = pet.PetReservations.First().PetName,
                                Rate = pet.PetReservations.First().Rate,
                                SubTotal = pet.PetReservations.First().SubTotal

                            });
                        }

                    newReservation.Total = newReservation.PetReservations.Sum(x => x.SubTotal);


                        results.Add(newReservation);
                    }
                    return results;
                
            }
        }

        public Reservation GetReservationsById(int id)
        {
            using(var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT r.ReservationId, OrderDate, DropOffDate, PickUpDate, 
                                    r.SitterId, ps.FirstName +' ' + ps.LastName AS PetSitterName,
                                    r.PetOwnerId, po.FirstName +' '+ po.LastName AS PetOwnerName,
                                    r.PetCareOptionId, 
                                    pr.ReservationId, p.PetId, p.PetName, pco.PetCareOptionName, pso.Rate, 
                                    pso.Rate*(CAST(DATEDIFF(DAY, DropOffDate, PickUpDate) AS decimal)) AS SubTotal, Status

                                    FROM Reservation r
                                    LEFT JOIN PetReservation pr ON r.ReservationId = pr.ReservationId
                                    LEFT JOIN Pet p ON pr.PetId = p.PetId
                                    LEFT JOIN PetSitter ps On r.SitterId = ps.SitterId
                                    LEFT JOIN PetSitterOption pso ON r.SitterId = pso.SitterId  AND p.petsizeid = pso.petsizeid 
                                    AND r.PetCareOptionId = pso.PetCareOptionId
                                    AND p.PetCategoryId = pso.PetCategoryId 
                                    LEFT JOIN PetCareOption pco ON pco.PetCareOptionId = r.PetCareOptionId
                                    LEFT JOIN PetOwner po ON po.PetOwnerId = r.PetOwnerId 
                                    WHERE r.ReservationId = @ReservationId
                                    GROUP BY r.ReservationId, OrderDate, DropOffDate, PickUpDate,
                                             r.SitterId, ps.FirstName, ps.LastName, 
                                             r.PetOwnerId,po.FirstName, po.LastName, r.PetCareOptionId,
                                             pr.ReservationId, p.PetId, p.PetName, r.PetCareOptionId, pco.PetCareOptionName, 
                                             pso.Rate, r.DropOffDate, r.PickUpDate, Status";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@ReservationId", id));
                cmd.Connection = conn;

                conn.Open();

                // Data is accessible through the DataReader object here
                //var reader = new SqlDataReader()
                var reservations = new List<Reservation>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()){
                        var reservation = new Reservation();
                        var pet = new PetReservation();
                        reservation.ReservationId = reader.GetInt32(0);
                        reservation.OrderDate = reader.GetDateTime(1);
                        reservation.DropOffDate = reader.GetDateTime(2);
                        reservation.PickUpDate = reader.GetDateTime(3);
                        reservation.SitterId = reader.GetInt32(4);
                        reservation.PetSitterName = reader.GetString(5);
                        reservation.PetOwnerId = reader.GetInt32(6);
                        reservation.PetOwnerName = reader.GetString(7);
                        reservation.PetCareOptionId = reader.GetInt32(8);
                        pet.ReservationId = reader.SafeGetValue<int>(9);
                        pet.PetId = reader.SafeGetValue<int>(10);
                        pet.PetName = reader.SafeGetValue<string>(11);
                        pet.PetCareOptionName = reader.SafeGetValue<string>(12);
                        pet.Rate = reader.SafeGetValue<decimal>(13);
                        pet.SubTotal = reader.SafeGetValue<decimal>(14);
                        reservation.Status = (ReservationStatus)Enum.Parse(typeof(ReservationStatus), reader.SafeGetValue<string>(15));
                        //reservation.Status = reader.SafeGetValue<ReservationStatus>(15);

                        reservation.PetReservations = new List<PetReservation> { pet };
                        reservations.Add(reservation);
                    }

                    if (reservations.Count() > 0)
                    {
                        var newReservation = new Reservation()
                        {
                            ReservationId = reservations.First().ReservationId,
                            OrderDate = reservations.First().OrderDate,
                            DropOffDate = reservations.First().DropOffDate,
                            PickUpDate = reservations.First().PickUpDate,
                            SitterId = reservations.First().SitterId,
                            PetSitterName = reservations.First().PetSitterName,
                            PetOwnerId = reservations.First().PetOwnerId,
                            PetOwnerName = reservations.First().PetOwnerName,
                            PetCareOptionId = reservations.First().PetCareOptionId,
                            PetReservations = new List<PetReservation>(),
                            Status = reservations.First().Status
                        };

                        foreach (var pet in reservations)
                        {
                            ((List<PetReservation>)newReservation.PetReservations).Add(new PetReservation()
                            {
                                ReservationId = pet.PetReservations.First().ReservationId,
                                PetId = pet.PetReservations.First().PetId,
                                PetName = pet.PetReservations.First().PetName,
                                PetCareOptionName = pet.PetReservations.First().PetCareOptionName,
                                Rate = pet.PetReservations.First().Rate,
                                SubTotal = pet.PetReservations.First().SubTotal
                            });

                            newReservation.Total = newReservation.PetReservations.Sum(x => x.SubTotal);

                        }
                        return newReservation;
                    }

                    else
                    {
                        return null;
                    }

                }
            }
        }

        public Reservation AddNewReservation(Reservation reservation)
        {
            using(var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Reservation(SitterId, PetOwnerId, OrderDate, DropOffDate, PickUpDate, PetCareOptionId, Status)
                                    OUTPUT INSERTED.ReservationId
                                    VALUES(@SitterId, @PetOwnerId, @OrderDate, @DropOffDate, @PickUpDate, @PetCareOptionId, @Status)";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@SitterId", reservation.SitterId));
                cmd.Parameters.Add(new SqlParameter("@PetOwnerId", reservation.PetOwnerId));
                cmd.Parameters.Add(new SqlParameter("@OrderDate", reservation.OrderDate));
                cmd.Parameters.Add(new SqlParameter("@DropOffDate", reservation.DropOffDate));
                cmd.Parameters.Add(new SqlParameter("@PickUpDate", reservation.PickUpDate));
                cmd.Parameters.Add(new SqlParameter("@PetCareOptionId", reservation.PetCareOptionId));
                cmd.Parameters.Add(new SqlParameter("@Status", reservation.Status));
                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservation.ReservationId = reader.GetInt32(0);
                    }
                }
            }
            return reservation;
        }

        public IEnumerable<Reservation> FindReservations(ReservationRequest request)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT r.ReservationId, OrderDate, DropOffDate, PickUpDate, r.SitterId, 
                                    ps.FirstName + ' ' + ps.LastName AS PetSitterName,
                                     r.PetOwnerId, po.FirstName + ' ' + po.LastName AS PetOwnerName, r.PetCareOptionId, 
                                    p.PetId, p.PetName, pco.PetCareOptionName, pso.Rate, 
                                    pso.Rate * (CAST(DATEDIFF(DAY, DropOffDate, PickUpDate) AS decimal)) AS SubTotal, Status

                                    FROM Reservation r
                                    LEFT JOIN PetReservation pr ON r.ReservationId = pr.ReservationId
                                    LEFT JOIN Pet p ON pr.PetId = p.PetId
                                    LEFT JOIN PetSitter ps On r.SitterId = ps.SitterId
                                    LEFT JOIN PetSitterOption pso ON r.SitterId = pso.SitterId  AND p.petsizeid = pso.petsizeid
                                    AND r.PetCareOptionId = pso.PetCareOptionId
                                    AND p.PetCategoryId = pso.PetCategoryId
                                    LEFT JOIN PetCareOption pco ON pco.PetCareOptionId = r.PetCareOptionId
                                    LEFT JOIN PetOwner po ON po.PetOwnerId = r.PetOwnerId
                                    WHERE ";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                if (request.ReservationId != null)
                {
                    cmd.CommandText += " r.ReservationId = @ReservationId and";
                    cmd.Parameters.Add(new SqlParameter("@ReservationId", request.ReservationId));
                }

                if (request.SitterId!= null)
                {
                    cmd.CommandText += " r.SitterId = @SitterId and";
                    cmd.Parameters.Add(new SqlParameter("@SitterId", request.SitterId));
                }

                if (request.PetOwnerId!= null)
                {
                    cmd.CommandText += " r.PetOwnerId = @PetOwnerId and";
                    cmd.Parameters.Add(new SqlParameter("@PetOwnerId", request.PetOwnerId));
                }

                if (request.OrderDate != null)
                {
                    cmd.CommandText += " OrderDate = @OrderDate and";
                    cmd.Parameters.Add(new SqlParameter("@OrderDate", request.OrderDate));
                }

                if (request.FromDate != null)
                {
                    cmd.CommandText += " DropOffDate >= @FromDate and";
                    cmd.Parameters.Add(new SqlParameter("@FromDate", request.FromDate));
                }

                if (request.ToDate != null)
                {
                    cmd.CommandText += " PickUpDate <= @ToDate and";
                    cmd.Parameters.Add(new SqlParameter("@ToDate", request.ToDate));
                }


                cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 4);
                cmd.CommandText += @" GROUP BY r.ReservationId, OrderDate, DropOffDate, PickUpDate,
                                             r.SitterId, ps.FirstName, ps.LastName, 
                                             r.PetOwnerId,po.FirstName, po.LastName, r.PetCareOptionId,
                                             p.PetId, p.PetName, r.PetCareOptionId, pco.PetCareOptionName, 
                                             pso.Rate, r.DropOffDate, r.PickUpDate, Status";

                conn.Open();


                // Data is accessible through the DataReader object here
                var reservations = new List<Reservation>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation();
                        var pet = new PetReservation();
                        reservation.ReservationId = reader.GetInt32(0);
                        reservation.OrderDate = reader.GetDateTime(1);
                        reservation.DropOffDate = reader.GetDateTime(2);
                        reservation.PickUpDate = reader.GetDateTime(3);
                        reservation.SitterId = reader.GetInt32(4);
                        reservation.PetSitterName = reader.GetString(5);
                        reservation.PetOwnerId = reader.GetInt32(6);
                        reservation.PetOwnerName = reader.GetString(7);
                        reservation.PetCareOptionId = reader.GetInt32(8);
                        pet.PetId = reader.GetInt32(9);
                        pet.PetName = reader.GetString(10);
                        pet.PetCareOptionName = reader.GetString(11);
                        pet.Rate = reader.GetDecimal(12);
                        pet.SubTotal = reader.GetDecimal(13);
                        reservation.Status = (ReservationStatus)Enum.Parse(typeof(ReservationStatus),reader.SafeGetValue<string>(14));

                        reservation.PetReservations = new List<PetReservation> { pet };
                        reservations.Add(reservation);
                    }
                }
                var results = new List<Reservation>();
                foreach (var reservation in reservations.GroupBy(r => r.ReservationId))
                {
                    var newReservation = new Reservation()
                    {
                        ReservationId = reservation.First().ReservationId,
                        OrderDate = reservation.First().OrderDate,
                        DropOffDate = reservation.First().DropOffDate,
                        PickUpDate = reservation.First().PickUpDate,
                        SitterId = reservation.First().SitterId,
                        PetSitterName = reservation.First().PetSitterName,
                        PetCareOptionId = reservation.First().PetCareOptionId,
                        PetOwnerId = reservation.First().PetOwnerId,
                        PetOwnerName = reservation.First().PetOwnerName,
                        PetReservations = new List<PetReservation>(),
                        Status = reservations.First().Status

                    };

                    foreach (var pet in reservation)
                    {
                        ((List<PetReservation>)newReservation.PetReservations).Add(new PetReservation()
                        {
                            PetCareOptionName = pet.PetReservations.First().PetCareOptionName,
                            PetId = pet.PetReservations.First().PetId,
                            PetName = pet.PetReservations.First().PetName,
                            Rate = pet.PetReservations.First().Rate,
                            SubTotal = pet.PetReservations.First().SubTotal

                        });
                    }

                    newReservation.Total = newReservation.PetReservations.Sum(x => x.SubTotal);


                    results.Add(newReservation);
                }
                return results;

            }
        }
        #endregion

        #region PetReservation

        public PetReservation AddNewPetReservation(PetReservation petreservation)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO PetReservation(ReservationId, PetId)
                                    VALUES(@ReservationId, @PetId)";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@ReservationId", petreservation.ReservationId));
                cmd.Parameters.Add(new SqlParameter("@PetId", petreservation.PetId));

                cmd.Connection = conn;

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                return petreservation;
            }
        }

        public void DeletePetReservation(PetReservation petreservation)
        {
            using (var sqlConn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM PetReservation
                                    WHERE ReservationId = @ReservationId AND PetId = @PetId";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@ReservationId", petreservation.ReservationId));
                cmd.Parameters.Add(new SqlParameter("@PetId", petreservation.PetId));
                cmd.Connection = sqlConn;


                sqlConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

    }
}



