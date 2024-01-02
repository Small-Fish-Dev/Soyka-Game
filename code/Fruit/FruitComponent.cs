using static Sandbox.Component;

namespace Soyka;

public partial class FruitComponent : Component, ICollisionListener
{
	[Property]
	public List<Texture> Tiers { get; set; }
	public ModelRenderer Plane { get; private set; }
	public Collider Collider { get; set; }
	public Rigidbody Rigidbody { get; set; }

	public int Tier { get; private set; } = 1;
	public float Mass => Tier * Tier;
	public float Scale => MathF.Sqrt( Mass / (float)Math.PI );
	public bool IsLastTier => Tier == Tiers.Count();

	protected override void OnStart()
	{
		base.OnStart();

		Plane = GameObject.Components.Get<ModelRenderer>();
		Collider = GameObject.Components.Get<Collider>();
		Rigidbody = GameObject.Components.Get<Rigidbody>();

		UpdateTexture();
		UpdateScale();
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

		if ( component.Tier == Tier && !IsLastTier )
		{
			IncreaseTier();
			obj.Destroy();
		}
	}

	public void OnCollisionUpdate( Collision other ) { }
	public void OnCollisionStop( CollisionStop other ) { }

	public void IncreaseTier()
	{
		if ( IsLastTier ) return;

		Tier++;

		UpdateTexture();
		UpdateScale();
	}

	public void UpdateTexture()
	{
		var texture = Tiers[Tier - 1];
		Plane.SceneObject.Batchable = false;
		Plane.SceneObject.Attributes.Set( "FruitTexture", texture );
	}

	public void UpdateScale()
	{
		GameObject.Transform.Scale = Scale;
	}
}
