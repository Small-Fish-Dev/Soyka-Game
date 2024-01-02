namespace Soyka;

public sealed class PlayableAreaComponent : Component
{
	[Property]
	public PrefabFile Ball { get; set; }

	[Property]
	public BBox PlayableBounds { get; set; }

	[Property]
	public BBox PlaceableBounds { get; set; }

	SceneModel _preview;

	protected override void DrawGizmos()
	{
		base.DrawGizmos();

		var draw = Gizmo.Draw;
		draw.LineBBox( PlayableBounds );
		draw.Color = Color.Red;
		draw.LineBBox( PlaceableBounds );
	}

	/// <summary>
	/// Transform a point into the local system
	/// </summary>
	/// <param name="point"></param>
	/// <returns></returns>
	public Vector3 WorldToLocal( Vector3 point ) => Transform.World.PointToLocal( point );

	/// <summary>
	/// Check if the point is inside of the defined playable bounds
	/// </summary>
	/// <param name="point"></param>
	/// <param name="local"></param>
	/// <returns></returns>
	public bool IsInsidePlayableBounds( Vector3 point, bool local = false ) => PlayableBounds.Contains( local ? point : WorldToLocal( point ) );

	/// <summary>
	/// Check if the point is inside of the defined placeable bounds
	/// </summary>
	/// <param name="point"></param>
	/// <param name="local"></param>
	/// <returns></returns>
	public bool IsInsidePlaceableBounds( Vector3 point, bool local = false ) => PlaceableBounds.Contains( local ? point : WorldToLocal( point ) );

	/// <summary>
	/// Gets the mouse position of the main camera
	/// </summary>
	/// <returns></returns>
	public Vector3 GetMousePosition()
	{
		var screenRay = Camera.Main.GetRay( Mouse.Position );
		var screenPosition = Scene.Trace.Ray( screenRay, 9999f )
			.Run();

		return screenPosition.HitPosition;
	}

	/// <summary>
	/// Find the ideal placement position
	/// </summary>
	/// <returns></returns>
	public Vector3 GetPlacementPosition()
	{
		var minY = PlaceableBounds.Mins.y;
		var maxY = PlaceableBounds.Maxs.y;
		var clamped = Math.Clamp( GetMousePosition().y, minY, maxY );

		return new Vector3( Transform.Position.x, clamped, PlaceableBounds.Mins.z );
	}

	protected override void OnStart()
	{
		base.OnStart();

		_preview = new SceneModel( Scene.SceneWorld, "models/dev/sphere.vmdl", Transform.World );
		_preview.ColorTint = Color.White.WithAlpha( 0.5f );
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		if ( _preview != null )
		{
			_preview.Transform = new Transform( GetPlacementPosition() );
		}
	}

	public void OnClick()
	{
		SpawnBall( GetPlacementPosition() );
	}

	public void SpawnBall( Vector3 position )
	{
		var ball = SceneUtility.Instantiate( SceneUtility.GetPrefabScene( Ball ) );
		if ( ball == null )
			return;

		ball.BreakFromPrefab();
		ball.Transform.Position = position;
		ball.Transform.Rotation = Rotation.From( 90, 180, 0 );

		var component = ball.Components.Get<FruitComponent>();
		//component.Fruit = Fruit.All[0];
	}
}
