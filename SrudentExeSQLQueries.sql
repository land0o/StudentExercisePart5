--SELECT Id ExerciseName, ExerciseLanguage FROM Exercise;
--SELECT Id, ExerciseName, ExerciseLanguage FROM Exercise WHERE ExerciseLanguage = 'JavaScript';
--INSERT INTO Exercise (ExerciseName, ExerciseLanguage) Values ('StudentExercise', 'Csharp');
SELECT i.Id, i.FirstName, i.LastName, i.SlackHandle, i.Speciality, i.CohortId, c.CohortName 
FROM Instructor i INNER JOIN Cohort c ON i.CohortID = c.id;
--INSERT INTO Instructor (FirstName, LastName, SlackHandle, Speciality, CohortId)
--VALUES ('Andy', 'Rascal', 'Sherif', 'Hipster Fashion' , 1)