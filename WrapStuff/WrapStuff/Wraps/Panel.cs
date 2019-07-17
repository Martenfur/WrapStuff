using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WrapStuff.Wraps
{
	
	public class Panel
	{
		/// <summary>
		/// Tells, on which side relative to the parent the panel will be.
		/// 0 - bottom
		/// 1 - right
		/// 2 - top
		/// 3 - left
		/// </summary>
		public int Side;

		public string Name;
		public Vector2 Offset;
		public Vector2 Size;

		public Panel[] Attachments;
		

		public void Draw()
		{
			// Transform matrices took care of the rotations,
			// so all panels assume they are drawn with no rotation\offset.
			RectangleShape.DrawBySize(Offset - Vector2.UnitY * Size / 2f, Size, true);
			
			CircleShape.Draw(Offset, 4, true);
			
			LineShape.Draw(Offset, Offset - Vector2.UnitY * 8);
			
			if (Attachments == null)
			{
				return;
			}

			// Transform matrices stack.
			GraphicsMgr.AddTransformMatrix(
				Matrix.CreateTranslation(-(Vector2.UnitY * Size / 2f).ToVector3())
			);
			foreach (var panel in Attachments)
			{
				// We need to offset the child panel by half of parent's 
				// width or height depending on the side.
				var length = Size.X / 2f;
				var resultSide = panel.Side;
				if (panel.Side % 2 == 0)
				{
					length = Size.Y / 2f;
				}

				GraphicsMgr.AddTransformMatrix(
					//Matrix.CreateRotationZ(0.3f) // Future reference - use this to pivot the panels.
					Matrix.CreateTranslation(-(Vector2.UnitY * length).ToVector3())
					* Matrix.CreateRotationZ((float)(Math.PI - Math.PI / 2f * resultSide))
					* Matrix.CreateTranslation(Offset.ToVector3())
				);
				panel.Draw();
				GraphicsMgr.ResetTransformMatrix();
			}

			GraphicsMgr.ResetTransformMatrix();
		}

		public void Get3DVertices(List<VertexPositionColorTexture> vertices, Matrix transformMatrix)
		{
			var center = Offset - Vector2.UnitY * Size / 2f;

			// 0 1
			// 3 2

			var v = new VertexPositionColorTexture[6];
			v[0].Position = (center - Size / 2f).ToVector3();
			v[0].TextureCoordinate = new Vector2(0, 0);
			v[1].Position = (center - new Vector2(-Size.X, Size.Y) / 2f).ToVector3();
			v[1].TextureCoordinate = new Vector2(0, 1);
			v[2].Position = (center + new Vector2(-Size.X, Size.Y) / 2f).ToVector3();
			v[2].TextureCoordinate = new Vector2(1, 0);

			v[3].Position = (center - new Vector2(-Size.X, Size.Y) / 2f).ToVector3();
			v[3].TextureCoordinate = new Vector2(0, 1);
			v[4].Position = (center + new Vector2(-Size.X, Size.Y) / 2f).ToVector3();
			v[4].TextureCoordinate = new Vector2(1, 0);
			v[5].Position = (center + Size / 2f).ToVector3();
			v[5].TextureCoordinate = new Vector2(1, 1);


			for (var i = 0; i < 6; i += 1)
			{
				// Applying local transform matrix manually.
				// The engine is not built for 3D, lots of 
				// Has to be done by hand.
				v[i].Position = Vector3.Transform(v[i].Position, transformMatrix);
			}

			vertices.AddRange(v);


			if (Attachments == null)
			{
				return;
			}

			transformMatrix = Matrix.CreateTranslation(-(Vector2.UnitY * Size / 2f).ToVector3()) * transformMatrix;

			foreach (var panel in Attachments)
			{
				// We need to offset the child panel by half of parent's 
				// width or height depending on the side.
				var length = Size.X / 2f;
				var resultSide = panel.Side;
				if (panel.Side % 2 == 0)
				{
					length = Size.Y / 2f;
				}

				var localMatrix = transformMatrix * 
					Matrix.CreateTranslation((Vector2.UnitY * length * 2).ToVector3())
					* Matrix.CreateRotationZ((float)(Math.PI - Math.PI / 2f * resultSide))
					* Matrix.CreateTranslation(Offset.ToVector3());

				panel.Get3DVertices(vertices, localMatrix);
			}


		}




	}
}
