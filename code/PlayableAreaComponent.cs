using Sandbox;

public sealed class PlayableAreaComponent : Component
{
	[Property]
	public BBox PlayableBounds { get; set; }

	[Property]
	public BBox PlaceableBounds { get; set; }

	protected override void DrawGizmos()
	{
		base.DrawGizmos();

		var draw = Gizmo.Draw;
		draw.LineBBox( PlayableBounds );
		draw.Color = Color.Red;
		draw.LineBBox( PlaceableBounds );
	}

	public Vector3 WorldToLocal( Vector3 point ) => Transform.World.PointToLocal( point );

	/// <summary>
	/// Check if the point is inside of the defined playable bounds
	/// </summary>
	/// <param name="point"></param>
	/// <returns></returns>
	public bool IsInsidePlayableBounds( Vector3 point ) => PlayableBounds.Contains( WorldToLocal( point ) );

	/// <summary>
	/// Check if the point is inside of the defined placeable bounds
	/// </summary>
	/// <param name="point"></param>
	/// <returns></returns>
	public bool IsInsidePlaceableBounds( Vector3 point ) => PlaceableBounds.Contains( WorldToLocal( point ) );

	protected override void OnStart()
	{
		base.OnStart();

	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		var screenRay = Camera.Main.GetRay( Mouse.Position );
		var screenPosition = Scene.Trace.Ray( screenRay, 9999f )
			.Run();
		Log.Info( screenPosition.HitPosition );
		Log.Info( IsInsidePlayableBounds( screenPosition.HitPosition ) );
	}
}
