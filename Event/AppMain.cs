using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventManagerPro.Model.DomainModels;

namespace EventManagerPro.Model
{
    class AppMain
    {

        public static void Main()
        {
            // StudentModel example
            Console.WriteLine("\nTESTING STUDENT MANAGER:");
            Console.WriteLine(StudentModel.create("U096988R","asdqwe","Felix").MatricId);
            var allStudents = StudentModel.getAll();
            foreach (var s in allStudents)
            {
                Console.WriteLine("{0}--{1}--{2}",s.Key,s.Value.Name,s.Value.Password);
            }
            Student student = StudentModel.getByMatricId("U096988R");
            Console.WriteLine("Student's name is {0}",student.Name);
            foreach (var ev in student.OwnedEvents)
            {
                Console.WriteLine("{0} organizes {1} in {2}", student.Name, ev.Name, ev.Venue.Name);
            }
            Console.WriteLine("First auth attempt, {0}",StudentModel.authenticate("U096988R", "asdqwe"));
            Console.WriteLine("Second auth attempt, {0}", StudentModel.authenticate("U096988R", "aqwe"));
     
            // Venue
            Console.WriteLine("\nTESTING VENUE MANAGER:");
            var newVenue = VenueModel.create("LT19 school of computing");
            Console.WriteLine("Created venue: {0} with ID: {1}\n", newVenue.Name, newVenue.Id);
            var venues = VenueModel.getAll();
            Console.WriteLine("A list of venues:");
            foreach (var v in venues)
            {
                Console.WriteLine("{0}.) {1}", v.Id, v.Name);
            }
            
            // EventModel example
            Console.WriteLine("\nTESTING EVENT MANAGER:");
            var newEvent = EventModel.create("U096988R","My new event 2",2,new DateTime(2012,2,5,6,0,0),new DateTime(2012,2,5,9,0,0),100,30,"Just another event");
            Console.WriteLine("{0}, {1}",newEvent.Name,newEvent.Description);
            var allEvents = EventModel.getAll();
            Console.WriteLine("Loading all events:");
            foreach (var s in allEvents)
            {
                Console.WriteLine("EventName:{0},\nVenue:{1}\nOwner:{2}\n", s.Name, s.Venue.Name, s.Owner.Name);
            }

            // student registering event 
            Console.WriteLine("Student register event");
            Console.WriteLine(EventModel.registerGuest("U096988R",2));

            Console.WriteLine("");
            Console.WriteLine("End tester");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
