using System;
using System.Drawing;

namespace Motion {
    class Box : Entity {
        public Vector[] Vertices = new Vector[4];
        public Point[] Points = new Point[4];
        public Double Width;
        public Double Height;

        public Box(Double mass, Vector location, Vector velocity, Double theta, Double omega, Double restitution, Double width, Double height)
            : base(mass, mass * (width * width + height * height) / 12, location, velocity, theta, omega, restitution) {
            Width = width;
            Height = height;
            Vertices[0] = new Vector(-Width / 2, -Height / 2);
            Vertices[1] = new Vector(-Width / 2, Height / 2);
            Vertices[2] = new Vector(Width / 2, -Height / 2);
            Vertices[3] = new Vector(Width / 2, Height / 2);
            for (Int32 i = 0; i < Vertices.Length; i++)
                Vertices[i] = Vertices[i].Rotate(Theta) + Location;
        }

        public Box(Double mass, Vector location, Vector velocity, Double width, Double height)
            : this(mass, location, velocity, 0, 0, 1, width, height) { }

        public Box(Double mass, Vector location, Double theta, Double width, Double height)
            : this(mass, location, Vector.Zero, theta, 0, 1, width, height) { }

        public Box(Double mass, Vector location, Double width, Double height)
            : this(mass, location, Vector.Zero, 0, 0, 1, width, height) { }

        public override void Update() {
            Vertices[0] = new Vector(-Width / 2, -Height / 2);
            Vertices[1] = new Vector(-Width / 2, Height / 2);
            Vertices[2] = new Vector(Width / 2, -Height / 2);
            Vertices[3] = new Vector(Width / 2, Height / 2);
            for (Int32 i = 0; i < Vertices.Length; i++)
                Vertices[i] = Vertices[i].Rotate(Theta) + Location;

            for (Int32 i = 0; i < Points.Length; i++)
                Points[i] = Vertices[i].ToPoint();

            Velocity += Acceleration;
            Omega += Alpha;

            Location += Velocity;
            Theta += Omega;

            Acceleration = Vector.Zero;
            Alpha = 0;
        }

        public override void Accelerate(Entity entity, Vector point) {
            Acceleration -= Velocity;
            Alpha -= Omega;
            Double cor = .5 * (Restitution + entity.Restitution);
            Vector displacement = point - Location;

            // Linear to linear momentum. 
            Vector relativeVelocity = entity.Velocity - Velocity;
            Vector entityMomentum = entity.Mass * entity.Velocity;
            Vector selfMomentum = Mass * Velocity;
            Vector finalVelocity = (selfMomentum + entityMomentum + entity.Mass * cor * relativeVelocity) / (Mass + entity.Mass);

            // Linear to angular momentum. 
            Double entityLinearToAngular = Vector.CrossScalar(point - entity.Location, entity.Mass * entity.Velocity);
            Double selfLinearToAngular = Vector.CrossScalar(point - Location, Mass * Velocity);
            Double finalOmega = (selfLinearToAngular + entityLinearToAngular) / (Moment + entity.Moment);

            // Angular to angular momentum. 
            Double relativeOmega = entity.Omega - Omega;
            Double entityAngularMomentum = entity.Moment * entity.Omega / (point - entity.Location).Magnitude();
            Double selfAngularMomentum = Moment * Omega / displacement.Magnitude();
            //finalOmega += (selfAngularMomentum + entityAngularMomentum + entity.Moment * cor * relativeOmega) / (Moment + entity.Moment);

            // Angular to linear momentum. 
            Vector entityAngularToLinear = entityAngularMomentum * Vector.XAxis.Rotate(entity.Theta) / (point - Location).Magnitude();
            Vector selfAngularToLinear = -selfAngularMomentum * Vector.XAxis.Rotate(Theta) / (point - Location).Magnitude();
            Vector relativeAngularToLinear = entityAngularToLinear - selfAngularToLinear;
            //finalVelocity += (selfAngularToLinear + entityAngularToLinear + entity.Mass * cor * relativeVelocity) / (Mass + entity.Mass);

            Acceleration += finalVelocity;
            Alpha += finalOmega;
        }

        public override void Separate(Entity entity, Vector point) {
            Vector toPointUnit = (point - Location).Unit();
            Vector relpoint = (point - Location).Rotate(-Theta);
            while (Math.Abs(relpoint.X) <= Width / 2 && Math.Abs(relpoint.Y) <= Height / 2) {
                Location -= toPointUnit;
                relpoint = (point - Location).Rotate(-Theta);
            }
        }

        public override Boolean Contains(Vector point) {
            Vector normPoint = (point - Location).Rotate(-Theta);
            return Math.Abs(normPoint.X) <= Width / 2 && Math.Abs(normPoint.Y) <= Height / 2;
        }

        public override void Draw(Graphics g) {
            Pen pen = Pens.Black;
            g.DrawLine(pen, Points[0], Points[1]);
            g.DrawLine(pen, Points[0], Points[2]);
            g.DrawLine(pen, Points[1], Points[3]);
            g.DrawLine(pen, Points[2], Points[3]);
        }
    }
}
