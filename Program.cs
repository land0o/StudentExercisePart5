using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using StudentExercises;

namespace StudentExercisesPart5
{
    class Program
    {
        /// <summary>
        ///  The Main method is the starting point for a .net application.
        /// </summary>
        static void Main(string[] args)
        {
            // We must create an instance of the Repository class in order to use it's methods to
            //  interact with the database.
            Repository repository = new Repository();

            List<CreateExercise> exercise = repository.GetAllExercises();
            foreach (CreateExercise exe in exercise)
            {
                Console.WriteLine(exe.ExerciseLanguage);
                Console.WriteLine(exe.ExerciseName);
            }
        }
    }
}
