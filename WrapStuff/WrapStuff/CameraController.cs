using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.Cameras;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Drawing;
using System;

namespace WrapStuff
{
	/// <summary>
	/// Controls the camera.
	/// </summary>
	public class CameraController : Entity
	{
		Camera Camera;

		float _cameraSpeed = 400;

		float _minZoom = 0.1f;
		float _maxZoom = 30f;

		float _zoomSpeed = 1;

		float _rotationSpeed = 120;

		public const Buttons UpButton = Buttons.Up;
		public const Buttons DownButton = Buttons.Down;
		public const Buttons LeftButton = Buttons.Left;
		public const Buttons RightButton = Buttons.Right;
		public const Buttons ZoomInButton = Buttons.Z;
		public const Buttons ZoomOutButton = Buttons.X;
		

		public CameraController(Layer layer, Camera camera) : base(layer)
		{
			Camera = camera;
			Camera.Offset = Camera.Size / 2;
		}

		public override void Update()
		{
			// Movement.
			var movementVector3 = new Vector3(
				Input.CheckButton(LeftButton).ToInt() - Input.CheckButton(RightButton).ToInt(),
				Input.CheckButton(UpButton).ToInt() - Input.CheckButton(DownButton).ToInt(),
				0
			);
			movementVector3 = Vector3.Transform(
				movementVector3,
				Matrix.CreateRotationZ(MathHelper.ToRadians(Camera.Rotation))
			); // Rotating by the camera's rotation, so camera will always move relatively to screen. 

			var rotatedMovementVector = new Vector2(movementVector3.X, movementVector3.Y);

			Camera.Position += TimeKeeper.GlobalTime(_cameraSpeed / Camera.Zoom) * rotatedMovementVector;
			// Movement.

			// Zoom.
			var zoomDirection = Input.CheckButton(ZoomInButton).ToInt() - Input.CheckButton(ZoomOutButton).ToInt();
			Camera.Zoom += TimeKeeper.GlobalTime(_zoomSpeed) * zoomDirection;

			if (Camera.Zoom < _minZoom)
			{
				Camera.Zoom = _minZoom;
			}
			if (Camera.Zoom > _maxZoom)
			{
				Camera.Zoom = _maxZoom;
			}
			// Zoom.
		}

		public override void Draw()
		{
			Text.CurrentFont = Resources.Fonts.Arial;
			Text.Draw(
				CameraController.UpButton + "/" +
				CameraController.DownButton + "/" +
				CameraController.LeftButton + "/" +
				CameraController.RightButton + " - move the camera. + " +
				Environment.NewLine +
				CameraController.ZoomInButton + "/" +
				CameraController.ZoomOutButton + " - zoom.",
				Vector2.One * 32
			);
		}

		public void Reset()
		{
			Camera.Zoom = 1;
			Camera.Position = Camera.Offset;
		}

	}
}
