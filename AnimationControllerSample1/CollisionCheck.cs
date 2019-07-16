using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Design;



namespace AnimationControllerSample1
{
   public  class CollisionCheck
    {

      

        public void CheckForCollisions( AnimationControllerSample1.XNAnimationSample.WorldObject man,   AnimationControllerSample1.XNAnimationSample.WorldObject obj)
        {

            for (int i = 0; i < man.model.Meshes.Count; i++)
            {

                BoundingSphere manBoundingSphere = man.model.Meshes[i].BoundingSphere;
                manBoundingSphere.Center += man.position;
                manBoundingSphere.Radius *= man.Scale;


                for (int j = 0; j < obj.model.Meshes.Count; j++)
                {
                    BoundingSphere c1BoundingSphere = obj.model.Meshes[j].BoundingSphere;
                    c1BoundingSphere.Center += obj.position;
                    c1BoundingSphere.Radius *= obj.Scale;



                    if (manBoundingSphere.Intersects(c1BoundingSphere))
                    {

                        obj.backup( );


                        return;
                    }




                }
            }

        }




        //internal bool CheckForCollisions(AnimationControllerSample1.XNAnimationSample.WorldObject fighter, AnimationControllerSample1.XNAnimationSample.WorldObject modelCol)
        //{
        //    throw new NotImplementedException();
        //}

        internal void CheckForCollisions(XNAnimationSample.WorldObject fighter, Model RightWall)
        {
            throw new NotImplementedException();
        }
    }
}
