using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace Motion {
    public partial class World : Form {
        private static Entity[] entities = new Entity[100];

        public World() {
            InitializeComponent();
            Paint += Draw;

            new Thread(new ThreadStart(delegate {
                while (true) {
                    Invalidate();
                    Thread.Sleep(33);
                }
            })) {
                IsBackground = true
            }.Start();

            new Thread(new ThreadStart(delegate {
                while (true) {
                    Tick();
                    Thread.Sleep(10);
                }
            })) {
                IsBackground = true
            }.Start();


            // Temporary testing objects. 
            /*
            entities[0] = new Box(100, new Vector(470, 150), new Vector(0, 0), 100, 200);
            entities[1] = new Box(100, new Vector(600, 220), new Vector(0, 0), .1, -.006, 1, 100, 200);
            /*/
            entities[0] = new Box(100, new Vector(200, 190), new Vector(2, 0), 100, 200);
            entities[1] = new Box(100, new Vector(600, 350), new Vector(-2, 0), .1, -.0003, 1, 100, 200);
            //*/
        }

        /// <summary>
        /// Moves the simulation forward one frame. 
        /// </summary>
        private void Tick() {

            // Temporary implementation. 
            foreach (Entity entity in entities)
                if (entity != null)
                    entity.Update();

            for (Int32 i = 0; i < entities.Length; i++)
                if (entities[i] != null)
                    for (Int32 j = i + 1; j < entities.Length; j++)
                        if (entities[j] != null) {
                            Accelerate(entities[i], entities[j]);
                            Accelerate(entities[j], entities[i]);
                        }

            for (Int32 i = 0; i < entities.Length; i++)
                if (entities[i] != null)
                    for (Int32 j = i + 1; j < entities.Length; j++)
                        if (entities[j] != null) {
                            Separate(entities[i], entities[j]);
                            Separate(entities[j], entities[i]);
                        }
        }

        /// <summary>
        /// Accelerates colliding Entities. 
        /// </summary>
        private void Accelerate(Entity a, Entity b) {
            if (a is Box && b is Box) {
                Box c = a as Box;
                Box d = b as Box;

                Vector[] dvert = new Vector[4];
                for (Int32 i = 0; i < d.Vertices.Length; i++)
                    dvert[i] = (d.Vertices[i] - c.Location).Rotate(-c.Theta);
                for (Int32 i = 0; i < d.Vertices.Length; i++)
                    if (Math.Abs(dvert[i].X) <= c.Width / 2 && Math.Abs(dvert[i].Y) <= c.Height / 2) {
                        c.Accelerate(d, d.Vertices[i]);
                        d.Accelerate(c, d.Vertices[i]);
                    }
            }
        }

        /// <summary>
        /// Separates colliding Entities. 
        /// </summary>
        private void Separate(Entity a, Entity b) {
            if (a is Box && b is Box) {
                Box c = a as Box;
                Box d = b as Box;

                Vector[] dvert = new Vector[4];
                for (Int32 i = 0; i < d.Vertices.Length; i++)
                    dvert[i] = (d.Vertices[i] - c.Location).Rotate(-c.Theta);
                for (Int32 i = 0; i < d.Vertices.Length; i++)
                    if (Math.Abs(dvert[i].X) <= c.Width / 2 && Math.Abs(dvert[i].Y) <= c.Height / 2) {
                        c.Separate(d, d.Vertices[i]);
                        d.Separate(c, d.Vertices[i]);
                    }
            }
        }

        private void Draw(Object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (Entity entity in entities)
                if (entity != null)
                    entity.Draw(g);
        }
    }
}
