namespace Soyka;

using static Sandbox.Component;

partial class FruitComponent : ICollisionListener
{
	public Collider Collider { get; set; }
	public Rigidbody Rigidbody { get; set; }

	protected override void OnStart()
	{
		base.OnStart();

		Plane = GameObject.Components.Get<ModelRenderer>();
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
			var next = Fruit.GetNext();
			Fruit = next;
			
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
