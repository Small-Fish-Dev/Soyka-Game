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

	protected override void OnStart()
	{
		base.OnStart();

	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		Log.Info( Mouse.Position );
	}
}
