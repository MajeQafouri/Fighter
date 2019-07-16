#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using BoundingVolumeRendering;
using XNAnimation.Effects;
#endregion

namespace BoundingVolumeRendering
{
   
    public static class BoundingSphereRenderer
    {
        private static VertexBuffer vertBuffer;
        private static BasicEffect effect;
        private static int lineCount;
        public static void Initialize(GraphicsDevice graphicsDevice, int sphereResolution)
        {
            // create our effect
            EffectPool mm = new EffectPool();  
            effect = new BasicEffect(graphicsDevice, mm);

            
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;

            // calculate the number of lines to draw for all circles
            lineCount = (sphereResolution + 1) * 3;

            // we need two vertices per line, so we can allocate our vertices
            VertexPositionColor[] verts = new VertexPositionColor[lineCount * 2];

            // compute our step around each circle
            float step = MathHelper.TwoPi / sphereResolution;

            // used to track the index into our vertex array
            int index = 0;

            //create the loop on the XY plane first
            for (float a = 0f; a < MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(a), (float)Math.Sin(a), 0f),
                    Color.Blue);
                verts[index++] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(a + step), (float)Math.Sin(a + step), 0f),
                    Color.Blue);
            }

            //next on the XZ plane
            for (float a = 0f; a < MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(a), 0f, (float)Math.Sin(a)),
                    Color.Red);
                verts[index++] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(a + step), 0f, (float)Math.Sin(a + step)),
                    Color.Red);
            }

            //finally on the YZ plane
            for (float a = 0f; a < MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3(0f, (float)Math.Cos(a), (float)Math.Sin(a)),
                    Color.Green);
                verts[index++] = new VertexPositionColor(
                    new Vector3(0f, (float)Math.Cos(a + step), (float)Math.Sin(a + step)),
                    Color.Green);
            }

            // now we create the vertex buffer and put the vertices in it
            vertBuffer = new VertexBuffer(
                graphicsDevice, typeof(VertexPositionColor), verts.Length, BufferUsage.WriteOnly);
            vertBuffer.SetData(verts);
        }

        public static void Draw(this BoundingSphere sphere, Matrix view, Matrix projection)
        {
            if (effect == null)
                throw new InvalidOperationException("You must call Initialize before you can render any spheres.");

            
            
            // update our effect matrices
            effect.World = Matrix.CreateScale(sphere.Radius) * Matrix.CreateTranslation(sphere.Center);
            effect.View = view;
            effect.Projection = projection;
              
            effect.CommitChanges();
                      
             effect.GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, lineCount);
        }
    }
}
