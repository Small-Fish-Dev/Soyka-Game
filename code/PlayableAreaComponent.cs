namespace Soyka;

public sealed class PlayableAreaComponent : Component
{
	public static float Points { get; set; } = 0;
	public static GameObject CurrentFruit { get; set; }
	public static GameObject NextFruit { get; set; }

	[Property]
	public SceneFile MenuScene { get; set; }

	[Property]
	public PrefabFile Ball { get; set; }

	[Property]
	public BBox PlayableBounds { get; set; }

	/// <summary>
	/// After how much time a fruit being over the line will make you lose the game
	/// </summary>
	[Property]
	public float MaxOverflowTime { get; set; } = 5f;

	/// <summary>
	/// How much time before you can click again
	/// </summary>
	[Property]
	public float ClickRate { get; set; } = 0.3f;

	public TimeSince OverflowTimer { get; set; } = 0f;
	public TimeSince LastMouseClick { get; set; } = 0f;

	protected override void DrawGizmos()
	{
		base.DrawGizmos();

		var draw = Gizmo.Draw;
		draw.LineBBox( PlayableBounds );
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
		var minY = PlayableBounds.Mins.y;
		var maxY = PlayableBounds.Maxs.y;
		var clamped = Math.Clamp( GetMousePosition().y, minY, maxY );

		return new Vector3( Transform.Position.x, clamped, PlayableBounds.Maxs.z );
	}

	protected override void OnStart()
	{
		base.OnStart();

		CurrentFruit = SpawnBall( GetPlacementPosition() );
		NextFruit = SpawnBall( Camera.Main.Position - Camera.Main.Rotation.Forward * 1000f ); // Hide next fruit behind camera haha :)
		Points = 0f; // Points persist between playsessions???
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		if ( CurrentFruit != null )
			CurrentFruit.Transform.Position = GetPlacementPosition();

		var topLeft = PlayableBounds.Maxs.WithX( Transform.Position.x ).WithY( PlayableBounds.Mins.y );
		var topRight = PlayableBounds.Maxs.WithX( Transform.Position.x );
		var overflowCheck = Scene.Trace.Ray( topLeft, topRight )
			.WithTag( "Fruit" )
			.Run();

		if ( !overflowCheck.Hit )
			OverflowTimer = 0;

		if ( OverflowTimer >= MaxOverflowTime )
			GameManager.ActiveScene.Load( MenuScene );
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public void OnClick()
	{
		if ( LastMouseClick >= ClickRate )
		{
			SetDisabled( CurrentFruit, false );

			CurrentFruit = NextFruit;
			CurrentFruit.Transform.Position = GetPlacementPosition();

			NextFruit = SpawnBall( Camera.Main.Position - Camera.Main.Rotation.Forward * 1000f ); // Hide next fruit behind camera haha :)

			LastMouseClick = 0f;
		}
	}

	public GameObject SpawnBall( Vector3 position, bool disabled = true )
	{
		var ball = SceneUtility.Instantiate( SceneUtility.GetPrefabScene( Ball ) );

		if ( ball == null ) return null;

		ball.BreakFromPrefab();
		ball.Transform.Position = position;
		ball.Transform.Rotation = Rotation.From( 90, 180, 0 );

		if ( disabled )
			SetDisabled( ball );

		return ball;
	}

	void SetDisabled( GameObject ball, bool disabled = true )
	{
		var collider = ball.Components.Get<Collider>( includeDisabled: true );

		if ( collider != null )
			collider.Enabled = !disabled;

		var body = ball.Components.Get<Rigidbody>( includeDisabled: true );

		if ( body != null )
			body.Enabled = !disabled;
	}
}
