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

		// Randomy increase tier of the first drop
		var upgradeTier = Game.Random.Int( 100 );

		if ( upgradeTier <= 40f ) // 40% Chance it's a tier 2 or higher
			IncreaseTier();
		if ( upgradeTier <= 15f ) // 15% Chance it's a tier 3
			IncreaseTier();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		GameObject.Transform.Scale = Vector3.Lerp( GameObject.Transform.Scale, Scale, Time.Delta * 20f );
	}

	public void OnCollisionStart( Collision other )
	{
		Tags.Set( "Fruit", true ); // Only set the tag used to check overflow after it stopped being in mid air

		var obj = other.Other.Collider?.GameObject ?? null;

		if ( obj == null ) return;

		var component = obj?.Components?.Get<FruitComponent>() ?? null;

		if ( component == null ) return;

		if ( component.Tier == Tier && !IsLastTier )
		{
			var isHighestFruit = obj.Transform.Position.z > Transform.Position.z; // Delete the highest one and upgrade the lowest one

			if ( !isHighestFruit )
			{
				component.IncreaseTier();
				GameObject.DestroyImmediate();
			}
			else
			{
				IncreaseTier();
				obj.DestroyImmediate();
			}
		}
	}

	public void OnCollisionUpdate( Collision other ) { }
	public void OnCollisionStop( CollisionStop other ) { }

	public void IncreaseTier()
	{
		if ( IsLastTier ) return;

		Tier++;

		PlayableAreaComponent.Points += Mass;

		UpdateTexture();
	}

	public void UpdateTexture()
	{
		var texture = Tiers[Tier - 1];
		Plane.SceneObject.Batchable = false;
		Plane.SceneObject.Attributes.Set( "FruitTexture", texture );
	}
}
