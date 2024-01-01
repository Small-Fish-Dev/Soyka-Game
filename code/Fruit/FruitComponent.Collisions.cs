namespace Soyka;

using static Sandbox.Component;

partial class FruitComponent : ICollisionListener
{
	public Collider Collider { get; set; }
	public Rigidbody Rigidbody { get; set; }
	public TimeSince LastMerged { get; set; }

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
		var obj = other.Other.Collider.GameObject;
		var component = obj
			?.Components
			?.Get<FruitComponent>();

		if ( component == null ) 
			return;

		if ( Fruit == component.Fruit && !Fruit.IsLast() )
		{
			Fruit = Fruit.GetNext();
			LastMerged = 0;
			obj.Destroy();
		}
	}

	public void OnCollisionUpdate( Collision other )
	{
	}

	public void OnCollisionStop( CollisionStop other )
	{

	}
}
