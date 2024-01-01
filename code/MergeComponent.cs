using Sandbox;
using static Sandbox.Component;

public sealed class MergeComponent : Component, ICollisionListener
{
	/// <summary>
	/// This will only merge with others that have the same merge order and add 1 to it
	/// </summary>
	[Property]
	public int MergeOrder { get; set; } = 1;

	public Collider Collider { get; set; }
	public Rigidbody Rigidbody { get; set; }

	protected override void OnStart()
	{
		base.OnStart();

		Collider = GameObject.Components.Get<Collider>();
		Rigidbody = GameObject.Components.Get<Rigidbody>();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

	}

	public void OnCollisionStart( Collision other )
	{
		var otherCollider = other.Other.Collider;
		var otherObject = otherCollider.GameObject;
		var otherMerger = otherObject.Components.Get<MergeComponent>();

		if ( otherMerger == null ) return;

		if ( otherMerger.MergeOrder == MergeOrder )
		{
			MergeOrder++;
			Transform.Scale *= 1.3f;

			otherObject.Destroy();
		}
	}

	public void OnCollisionUpdate( Collision other )
	{
	}

	public void OnCollisionStop( CollisionStop other )
	{

	}
}
