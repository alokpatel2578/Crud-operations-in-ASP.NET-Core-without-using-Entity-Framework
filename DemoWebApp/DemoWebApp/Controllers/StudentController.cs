using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DemoWebApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly string _connectionString = "Data Source=DESKTOP-MFLCOI2;Initial Catalog=TestDb;User ID=sa;Password=adk@1234";
        public IActionResult Index()
        {
            List<Student> students = new List<Student>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Student", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                City = reader["City"].ToString()
                            });
                        }
                    }
                }
            }

            return View(students);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("INSERT INTO Student (Name, Email, City) VALUES (@Name, @Email, @City)", connection))
                    {
                        command.Parameters.AddWithValue("@Name", student.Name);
                        command.Parameters.AddWithValue("@Email", student.Email);
                        command.Parameters.AddWithValue("@City", student.City);
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public IActionResult Edit(int id)
        {
            // Retrieve student information from database and populate the form for editing
            Student student = new Student();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Student WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            student.Id = Convert.ToInt32(reader["Id"]);
                            student.Name = reader["Name"].ToString();
                            student.Email = reader["Email"].ToString();
                            student.City = reader["City"].ToString();
                        }
                    }
                }
            }

            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("UPDATE Student SET Name = @Name, Email = @Email, City = @City WHERE Id = @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Name", student.Name);
                        command.Parameters.AddWithValue("@Email", student.Email);
                        command.Parameters.AddWithValue("@City", student.City);
                        command.Parameters.AddWithValue("@Id", student.Id);
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public IActionResult Delete(int id)
        {
            Student studentToDelete = GetStudentById(id);
            if (studentToDelete == null)
            {
                return NotFound();
            }
            return View(studentToDelete);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Retrieve student by id and delete it
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Student WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }

        // Helper method to get student by id
        private Student GetStudentById(int id)
        {
            Student student = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Student WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            student = new Student
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                City = reader["City"].ToString()
                            };
                        }
                    }
                }
            }
            return student;
        }
    }
}

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string City { get; set; }
}

