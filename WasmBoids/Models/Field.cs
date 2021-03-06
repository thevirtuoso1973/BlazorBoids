using System;
using System.Collections.Generic;
using System.Numerics;

namespace WasmBoids.Models
{
    public class Field
    {
        public readonly List<Boid> Boids = new();
        public double Width { get; private set; }
        public double Height { get; private set; }
        
        public (double, double) MousePos { get; set; }

        public void Resize(double width, double height) =>
            (Width, Height) = (width, height);

        public Field(double width, double height, int numBoids, int numPredators)
        {
            Resize(width, height);

            AddRandomBoids(numBoids);
            // TODO: add predators
        }

        public void StepForward(Boid.Behaviours behaviours)
        {
            foreach (var boid in Boids)
                boid.StepForward(Width, Height, Boids, MousePos, behaviours);
        }

        // returns random normalized vector
        private static Vector2 RandomVector2(Random rand)
        {
            var v = new Vector2((float) (rand.NextDouble() * 2) - 1, (float) (rand.NextDouble() * 2) - 1);
            return v / v.Length();
        }


        public void AddRandomBoids(int count = 10)
        {
            var rand = new Random();

            for (var i = 0; i < count; i++)
            {
                var newBoid = new Boid(
                    new Vector2((float) (rand.NextDouble() * Width),
                        (float) (rand.NextDouble() * Height)),
                    RandomVector2(rand),
                    "white"
                );
                Boids.Add(newBoid);
            }
        }
    }
}