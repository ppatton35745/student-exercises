using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using System.Collections;
using Dapper;

namespace nss.Data
{
    public class DatabaseInterface
    {
        public static SqliteConnection Connection
        {
            get
            {
                /*
                    Mac users: You can create an environment variable in your
                    .zshrc file.
                        export NSS_DB="/path/to/your/project/nss.db"

                    Windows users: You need to use a property window
                        http://www.forbeslindesay.co.uk/post/42833119552/permanently-set-environment-variables-on-windows
                 */
                string env = $"{Environment.GetEnvironmentVariable("NSS_DB")}";
                string _connectionString = $"Data Source={env}";
                return new SqliteConnection(_connectionString);
            }
        }


        public static void CheckCohortTable()
        {
            SqliteConnection db = DatabaseInterface.Connection;

            try
            {
                List<Cohort> cohorts = db.Query<Cohort>
                    ("SELECT Id FROM Cohort").ToList();
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("no such table"))
                {
                    db.Execute(@"CREATE TABLE Cohort (
                        `Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        `Name`	TEXT NOT NULL UNIQUE
                    )");

                    db.Execute(@"INSERT INTO Cohort
                        VALUES (null, 'Evening Cohort 1')");

                    db.Execute(@"INSERT INTO Cohort
                        VALUES (null, 'Day Cohort 10')");

                    db.Execute(@"INSERT INTO Cohort
                        VALUES (null, 'Day Cohort 11')");

                    db.Execute(@"INSERT INTO Cohort
                        VALUES (null, 'Day Cohort 12')");

                    db.Execute(@"INSERT INTO Cohort
                        VALUES (null, 'Day Cohort 13')");

                    db.Execute(@"INSERT INTO Cohort
                        VALUES (null, 'Day Cohort 21')");

                }
            }
        }

        public static void CheckExerciseTable()
        {
            SqliteConnection db = DatabaseInterface.Connection;

            try
            {
                List<Exercise> cohorts = db.Query<Exercise>
                    ("SELECT Id FROM Exercise").ToList();
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("no such table"))
                {
                    db.Execute(@"CREATE TABLE Exercise (
                        `Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        `Name`	TEXT NOT NULL UNIQUE,
                        `Language` TEXT
                    )");

                    db.Execute(@"INSERT INTO Exercise
                        VALUES (null, 'Duck Exe', 'C#'),
                        (null, 'Moose Exe', 'C#'),
                        (null, 'Ant Exe', 'C#'),
                        (null, 'Chimp Exe', 'JavaScript'),
                        (null, 'Ape Exe', 'JavaScript')");

                }
            }
        }

        public static void CheckStudentTable()
        {
            SqliteConnection db = DatabaseInterface.Connection;

            try
            {
                List<Student> students = db.Query<Student>
                    ("SELECT Id FROM Student").ToList();
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("no such table"))
                {
                    db.Execute(@"CREATE TABLE Student (
                        `Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        `FirstName`	TEXT NOT NULL,
                        `LastName` TEXT NOT NULL,
                        `SlackHandle` TEXT NOT NULL UNIQUE,
                        `CohortId` INTEGER NOT NULL

                    )");

                    db.Execute(@"INSERT INTO Student
                        VALUES (null, 'John', 'Brown', 'JohnBrownSlack', (SELECT Id from Cohort WHERE Name = 'Day Cohort 12')),
                        (null, 'Jim', 'Bean', 'JimBeanSlack', (SELECT Id from Cohort WHERE Name = 'Evening Cohort 1')),
                        (null, 'Martavis', 'Bryant', 'MartavisBryantSlack', (SELECT Id from Cohort WHERE Name = 'Day Cohort 10')),
                        (null, 'Larry', 'Fitzgerald', 'LarryFitzgeraldSlack', (SELECT Id from Cohort WHERE Name = 'Day Cohort 12')),
                        (null, 'Allen', 'Hurns', 'AllenHurnSlack', (SELECT Id from Cohort WHERE Name = 'Day Cohort 13')),
                        (null, 'Eli', 'Manning', 'EliManningSlack', (SELECT Id from Cohort WHERE Name = 'Day Cohort 21')),
                        (null, 'Bruce', 'Pearl', 'BrucePearlSlack', (SELECT Id from Cohort WHERE Name = 'Day Cohort 11'))");

                }
            }
        }

        public static void CheckInstructorsTable()
        {
            SqliteConnection db = DatabaseInterface.Connection;

            try
            {
                List<Instructor> toys = db.Query<Instructor>
                    ("SELECT Id FROM Instructor").ToList();
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("no such table"))
                {
                    db.Execute($@"CREATE TABLE Instructor (
                        `Id`	integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                        `FirstName`	varchar(80) NOT NULL,
                        `LastName`	varchar(80) NOT NULL,
                        `SlackHandle`	varchar(80) NOT NULL,
                        `Specialty` varchar(80),
                        `CohortId`	integer NOT NULL,
                        FOREIGN KEY(`CohortId`) REFERENCES `Cohort`(`Id`)
                    )");

                    db.Execute($@"INSERT INTO Instructor
                        SELECT null,
                              'Steve',
                              'Brownlee',
                              '@coach',
                              'Dad jokes',
                              c.Id
                        FROM Cohort c WHERE c.Name = 'Evening Cohort 1'
                    ");

                    db.Execute($@"INSERT INTO Instructor
                        SELECT null,
                              'Joe',
                              'Shepherd',
                              '@joes',
                              'Analogies',
                              c.Id
                        FROM Cohort c WHERE c.Name = 'Day Cohort 13'
                    ");

                    db.Execute($@"INSERT INTO Instructor
                        SELECT null,
                              'Jisie',
                              'David',
                              '@jisie',
                              'Student success',
                              c.Id
                        FROM Cohort c WHERE c.Name = 'Day Cohort 21'
                    ");
                }
            }
        }

        public static void CheckStudentExerciseTable()
        {
            SqliteConnection db = DatabaseInterface.Connection;
            Console.WriteLine("Running Student Excercise Test");

            try
            {
                List<StudentExercise> studentExercises = db.Query<StudentExercise>
                    ("SELECT Id FROM StudentExercise").ToList();
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("no such table"))
                {
                    StudentExercise.Create(db);
                    Console.WriteLine("About to Seed Student Excercise");
                    StudentExercise.Seed(db);

                }
            }
        }

    }
}