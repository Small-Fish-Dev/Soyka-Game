using Sandbox;

public sealed class PlayableAreaComponent : Component
{
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

	protected override void OnStart()
	{
		base.OnStart();

		_preview = new SceneModel( Scene.SceneWorld, "models/dev/sphere.vmdl", Transform.World );
		_preview.ColorTint = Color.White.WithAlpha( 0.3f );
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		if ( _preview != null )
		{
			var nearestPosition = PlaceableBounds.ClosestPoint( GetMousePosition() );
			_preview.Transform = new Transform( nearestPosition.WithZ( PlaceableBounds.Mins.z ) );
		}
	}

	public void OnClick()
	{

		Log.Info( $"Is inside playable? {IsInsidePlayableBounds( GetMousePosition() )}" );
		Log.Info( $"Is inside placeable? {IsInsidePlaceableBounds( GetMousePosition() )}" );

	}
}
