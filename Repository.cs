using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using StudentExercises;

namespace StudentExercisesPart5
{
    /// <summary>
    ///  An object to contain all database interactions.
    /// </summary>
    public class Repository
    {
        /// <summary>
        ///  Represents a connection to the database.
        ///   This is a "tunnel" to connect the application to the database.
        ///   All communication between the application and database passes through this connection.
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                // This is "address" of the database
                string _connectionString = "Data Source=DESKTOP-G3F4HEE\\SQLEXPRESS;Initial Catalog=StudentExercises;Integrated Security=True;";
                return new SqlConnection(_connectionString);
            }
        }

        /************************************************************************************
        * Exercises
        ************************************************************************************/
        //Query the database for all the Exercises.
        /// <summary>
        ///  Returns a list of all Exercises in the database
        /// </summary>
        public List<CreateExercise> GetAllExercises()
        {
            using (SqlConnection conn = Connection)
            {
                //opens the connection to the database.
                conn.Open();

                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT Id, ExerciseName, ExerciseLanguage FROM Exercise";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the Exercises we retrieve from the database.
                    List<CreateExercise> Exercise = new List<CreateExercise>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {

                        // Now let's create a new Exercise object using the data from the database.
                        CreateExercise exercise = new CreateExercise()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ExerciseName = reader.GetString(reader.GetOrdinal("ExerciseName")),
                            ExerciseLanguage = reader.GetString(reader.GetOrdinal("ExerciseLanguage"))
                        };

                        // ...and add that department object to our list.
                        Exercise.Add(exercise);
                    }
                    //closes the conncection
                    reader.Close();

                    return Exercise;
                }

            }

        }

        /// <summary>
        ///  Find all the exercises in the database where the language is JavaScript.
        /// </summary>
        public CreateExercise GetJavaScriptExercises(string JavaScript)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, ExerciseName, ExerciseLanguage FROM Exercise WHERE ExerciseLanguage = 'JavaScript'";
                    cmd.Parameters.Add(new SqlParameter("@ExerciseLanguage", JavaScript));
                    SqlDataReader reader = cmd.ExecuteReader();

                    CreateExercise exercise = null;
                    if (reader.Read())
                    {
                        exercise = new CreateExercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ExerciseName = reader.GetString(reader.GetOrdinal("ExerciseName")),
                            ExerciseLanguage = reader.GetString(reader.GetOrdinal("ExerciseLanguage"))
                        };
                    }

                    reader.Close();

                    return exercise;
                }
            }
        }

        /// <summary>
        //Insert a new exercise into the database.
        ///   NOTE: This method sends data to the database,
        ///   it does not get anything from the database, so there is nothing to return.
        /// </summary>
        public void AddExercise(CreateExercise exercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Exercise (ExerciseName, ExerciseLanguage) Values (@ExerciseName, @ExerciseLanguage)";
                    cmd.Parameters.Add(new SqlParameter("@ExerciseName", exercise.ExerciseName));
                    cmd.Parameters.Add(new SqlParameter("@ExerciseLanguage", exercise.ExerciseLanguage));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Assigns an existing exercise to an existing student
        /// </summary>
        public void AssignExercise(int studentId, int exerciseId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (@StudentId, @ExerciseId)";
                    cmd.Parameters.Add(new SqlParameter("@StudentId", studentId));
                    cmd.Parameters.Add(new SqlParameter("@ExerciseId", exerciseId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /************************************************************************************
        * Instructor
        ************************************************************************************/

        /// <summary>
        //Find all instructors in the database.Include each instructor's cohort.   
        /// </summary>
        /// <returns>A list of instructors in which each instructor object contains their cohort object.</returns>
        public List<CreateInstructor> GetAllInstructorsWithCohort()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT i.Id, i.FirstName, i.LastName, i.SlackHandle, i.Speciality, i.CohortId, c.CohortName  " +
                       "FROM Instructor i INNER JOIN Cohort c ON i.CohortID = c.id";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<CreateInstructor> instructors = new List<CreateInstructor>();
                    while (reader.Read())
                    {
                        CreateInstructor instructor = new CreateInstructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            Speciality = reader.GetString(reader.GetOrdinal("Speciality")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            CreateCohort = new CreateCohort
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                            }
                        };

                        instructors.Add(instructor);
                    }
                    reader.Close();
                    return instructors;
                }

            }
        }

        /// <summary>
        ///  Insert a new instructor into the database. Assign the instructor to an existing cohort.
        ///   NOTE: This method sends data to the database,
        ///   it does not get anything from the database, so there is nothing to return.
        /// </summary>
        public void AddInstructor(CreateInstructor instructor)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = "INSERT INTO Instructor (FirstName, LastName, SlackHandle, Speciality, CohortId) " +
                        "VALUES (@FirstName, @LastName, @SlackHandle, @Speciality, @CohortId)";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", instructor.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName, ", instructor.LastName));
                    cmd.Parameters.Add(new SqlParameter("@SlackHandle, ", instructor.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@Speciality, ", instructor.Speciality));
                    cmd.Parameters.Add(new SqlParameter("@CohortId", instructor.CohortId));
                    cmd.ExecuteNonQuery();
                }
            }

        }
    }
}