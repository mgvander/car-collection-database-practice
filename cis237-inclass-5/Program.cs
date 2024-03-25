/// Author: Michael VanderMyde
/// Course: CIS-237
/// Inclass 5

using System;
using System.Linq;
using System.Collections.Generic;
using cis237_inclass_5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace cis237_inclass_5
{
    class Program
    {
        static void Main(string[] args)
        {
            // Make a new instance of the CarContext
            CarContext _carContext = new CarContext();

            /****************************************************************************
             * List out all of the cars in the table
             * *************************************************************************/
            Console.WriteLine("Print the list of cars");

            foreach (Car car in _carContext.Cars)
            {
                Console.WriteLine(CarToString(car));

            }

            /****************************************************************************
             * Find a specific car by any property
             * *************************************************************************/
            // Call the Where() method on the Cars table and pass in a lambda expression
            // for the criteria we are looking for. There is nothing special about the word
            // car in the part that reads: car => car.Id == "V0...". It could be any
            // characters we want it to be.
            // It is just a variable name for the current car we are considering in the
            // expression. This will automagically loop through all the cars, and run the 
            // expression against each of them. When the result is finally true, it will
            // return that car.
            Car _carToFind = _carContext.Cars.Where(car => car.Id == "V0LCD1814").First();

            // Print out the single car that we found
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Find a single car based on the id and where");
            Console.WriteLine(CarToString(_carToFind));

            /****************************************************************************
             * Find a list of cars by any property
             * *************************************************************************/
            List<Car> _challengerCars = _carContext.Cars.Where(car => car.Model == "Challenger").ToList();

            // Print out all the cars that have a model of challenger
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Find all the cars that have a model of Challenger");

            foreach (Car car in _challengerCars)
            {
                Console.WriteLine(CarToString(car));


            }

            /****************************************************************************
             * Find a specific car based on the Primary Key
             * *************************************************************************/
            // Pull out a single car from the table based on the id which is the primary
            // key.
            // If the record does not exit in the database it will return null.
            // Make sure to check what you get back to ensure that you got a result.
            Car _carByPrimaryKey = _carContext.Cars.Find("V0LCD1814");

            // Print out the one we get out with the Find() method.
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Print car found by the Find method");
            Console.WriteLine(CarToString(_carByPrimaryKey));

            /****************************************************************************
             * Add a new Car to the database
             * *************************************************************************/
            // Make a new instance of Car
            Car newCarToAdd = new Car();

            // Assign properties to the parts of the model
            newCarToAdd.Id = "88888";
            newCarToAdd.Make = "Nissan";
            newCarToAdd.Model = "GT-R";
            newCarToAdd.Horsepower = 550;
            newCarToAdd.Cylinders = 8;
            newCarToAdd.Year = "2021";
            newCarToAdd.Type = "Car";

            // Use a try catch to ensure that they cannot add a car with an id that already exists
            try
            {
                // Add the new car to the Cars Collection
                _carContext.Cars.Add(newCarToAdd);

                // This method call actually does the work of saving the ghanges to the database
                _carContext.SaveChanges();

            }
            catch (DbUpdateException e)
            {
                // Remove the new car from the Cars Collection since we can't save it.
                // We don't want it hanging around the next time we go to save changes.
                _carContext.Cars.Remove(newCarToAdd);

                // Write to the Console that there was an error.
                Console.WriteLine("Can't add the record. Already have one with that Primary Key");

            }

            // Fetch out the record we just added and ensure that it exists
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Just added a new car. Going to fetch it and print it to verify");

            _carToFind = _carContext.Cars.Find(newCarToAdd.Id);

            Console.WriteLine(CarToString(_carToFind));

            /****************************************************************************
             * Update the added car
             * *************************************************************************/
            // Find Car to update
            Car _carToUpdate = _carContext.Cars.Find("88888");

            // Output the car to update
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("About to do an update on this car.");
            Console.WriteLine(CarToString(_carToUpdate));
            Console.WriteLine("Doing the update now.");

            // Update some of the properties of the car we found.
            // Don't need to update all of them if we do not want to.
            _carToUpdate.Make = "Nissan"; // Staying the same
            _carToUpdate.Model = "GT-RRRRRRRRRR"; // Changing
            _carToUpdate.Horsepower = 1250; // Changing
            _carToUpdate.Cylinders = 16; // Changing
            _carToUpdate.Year = "2021"; // Staying the same
            _carToUpdate.Type = "Car"; // Staying the same
                                        
            // Save the changes to the database. Since when we pulled out the one to update,
            // all we were really doing was getting a reference to the one in the collection
            // that we wanted to update, there is no need to 'put' the car back into the Cars
            // Collect. It is still there. all we have to do is save the changes.
            _carContext.SaveChanges();

            // Get the car back out of the database
            _carToUpdate = _carContext.Cars.Find("88888");

            // Output the car to verify it got updated
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Outputting the updated car to make sure update works");
            Console.WriteLine(CarToString(_carToUpdate));

            /****************************************************************************
             * Delete the added car
             * *************************************************************************/
            // Get a car out of the database that we want to delete
            Car _carToDelete = _carContext.Cars.Find("88888");

            // Remove the Car form the Cars Collection
            _carContext.Cars.Remove(_carToDelete);

            // Save the changes to the database
            _carContext.SaveChanges();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Delete the added car. Looking to see if it is still in the database");

            // Try to get the car out of the database and print it.
            _carToDelete = _carContext.Cars.Find("88888");

            if (_carToDelete == null)
            {
                Console.WriteLine("The model you are looking for does not exist");

            }
            else
            {
                // Should not see this message.
                Console.WriteLine("The model is still in the database.");

            }

        }

        static string CarToString(Car passCar)
        {
            return $"{passCar.Id} {passCar.Make} {passCar.Model}";

        }
    }
}
