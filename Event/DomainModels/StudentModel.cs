using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace EventManagerPro.Model.DomainModels
{
    class StudentModel
    {
        public static Student create(string matricId, string password, string name = "student")
        {
            using (var context = new EventContainer())
            {

                var existedStudent = (from s in context.Students.Include("OwnedEvents").Include("RegisteredEvents")
                                      where s.MatricId == matricId
                                      select s).FirstOrDefault();

                if (existedStudent != null)
                {
                    // a student with this matriculation number exists
                    return existedStudent;
                }

                var newStudent = new Student
                {
                    MatricId = matricId,
                    Password = password,
                    Name = name,
                };
                context.Students.Add(newStudent);
                context.SaveChanges();
                return newStudent;
            }
        }

        public static Boolean authenticate(string matricId, string password)
        {
            Student student = getByMatricId(matricId);
            return (student != null && student.Password == password) ? true : false;
        }

        public static Dictionary<string, Student> getAll()
        {
            using (var context = new EventContainer())
            {
                IEnumerable<Student> students = from s in context.Students
                                                    .Include("OwnedEvents")
                                                    .Include("OwnedEvents.Venue")
                                                    .Include("OwnedEvents.Guests")
                                                    .Include("RegisteredEvents")
                                                    .Include("RegisteredEvents.Venue")
                                                    .Include("RegisteredEvents.Guests")
                                                orderby s.MatricId
                                                select s;
                return students.ToDictionary(k => k.MatricId);
            }
        }

        public static Student getByMatricId(string matricId)
        {
            using (var context = new EventContainer())
            {
                var students = from s in context.Students
                                   .Include("OwnedEvents")
                                   .Include("OwnedEvents.Venue")
                                   .Include("OwnedEvents.Guests")
                                   .Include("RegisteredEvents")
                                   .Include("RegisteredEvents.Venue")
                                   .Include("RegisteredEvents.Guests")
                               where s.MatricId == matricId
                               select s;
                return students.FirstOrDefault();
            }
        }


    }
}
