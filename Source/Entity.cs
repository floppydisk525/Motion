using System;
using System.Drawing;

namespace Motion {
    abstract class Entity {
        public Double Mass;
        public Double Moment;
        public Vector Location = Vector.Zero;
        public Vector Velocity = Vector.Zero;
        public Vector Acceleration = Vector.Zero;
        public Double Theta = 0;
        public Double Omega = 0;
        public Double Alpha = 0;
        public Double Restitution = 0;

        public Entity(Double mass, Double moment, Vector location, Vector velocity, Double theta, Double omega, Double restitution) {
            Mass = mass;
            Moment = moment;
            Location = location;
            Velocity = velocity;
            Theta = theta;
            Omega = omega;
            Restitution = restitution;
        }

        public Entity(Double mass, Double moment, Vector location, Vector velocity)
            : this(mass, moment, location, velocity, 0, 0, 1) { }

        public Entity(Double mass, Double moment, Vector location, Double theta)
            : this(mass, moment, location, Vector.Zero, theta, 0, 1) { }

        public Entity(Double mass, Double moment, Vector location)
            : this(mass, moment, location, Vector.Zero, 0, 0, 1) { }

        public abstract void Update();

        public abstract void Accelerate(Entity entity, Vector point);

        public abstract void Separate(Entity entity, Vector point);

        public abstract Boolean Contains(Vector point);

        public abstract void Draw(Graphics g);
    }
}
